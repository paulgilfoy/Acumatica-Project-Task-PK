using System;
using System.Collections.Generic;

namespace ProjectTask
{
    /// <summary>
    /// T-25: the "PK Status" of a project = the status of the project's OWN department.
    /// Contract.UsrPGDepartment TAX -> UsrPGTaxStatus, ACCT -> UsrPGAcctStatus, AUDIT -> UsrPGAuditStatus, 401K -> Usr401kStatus;
    /// any other department (ADMIN, TECH, ...) or no department -> empty (null ID, null text).
    /// This class is the ONE place in C# that knows which status field belongs to which department. It is used by
    /// <see cref="PKDepartmentStatusAttribute"/> (every project save, any graph) and by PKProjectStatusMaint.Persist
    /// (PKPM2001 renames). The SQL copy of the mapping in src/Sql/PKProjectStatusBackfill.sql and the
    /// [PKProjectStatusList("&lt;dept&gt;")] attributes in src/Code/Contract.cs are checked against it by tools/Test-PKStatus.ps1.
    /// Pure logic: NO Acumatica references, so tools/Test-PKStatus.ps1 compiles and runs this file on its own.
    /// Design and checks: docs/pk-status.md.
    /// </summary>
    public static class PKDepartmentStatus
    {
        /// <summary>Department ID (as in EPDepartment / Contract.UsrPGDepartment) -> name of the Contract status field and DB column.</summary>
        public static readonly IReadOnlyDictionary<string, string> StatusFieldByDepartment =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "TAX", "UsrPGTaxStatus" },
                { "ACCT", "UsrPGAcctStatus" },
                { "AUDIT", "UsrPGAuditStatus" },
                { "401K", "Usr401kStatus" },
            };

        /// <summary>
        /// The department key as written in <see cref="StatusFieldByDepartment"/> (e.g. " tax " -> "TAX"), or null when the
        /// department has no project status field (ADMIN, TECH, blank, null).
        /// </summary>
        public static string CanonicalDepartment(string departmentID)
        {
            string d = departmentID?.Trim();
            if (string.IsNullOrEmpty(d)) return null;
            foreach (string key in StatusFieldByDepartment.Keys)
                if (string.Equals(key, d, StringComparison.OrdinalIgnoreCase)) return key;
            return null;
        }

        /// <summary>Name of the Contract field (= DB column) that holds the department's status, or null for other departments.</summary>
        public static string StatusFieldOf(string departmentID)
        {
            string key = CanonicalDepartment(departmentID);
            return key == null ? null : StatusFieldByDepartment[key];
        }

        /// <summary>
        /// Computes the PK Status of one project row.
        /// <paramref name="readStatusField"/> reads an int field of the row by name (e.g. "UsrPGTaxStatus");
        /// <paramref name="describe"/> returns the status text for (canonical department, status ID) or null if unknown
        /// (the caller looks it up in the cached PKProjectStatus list, active AND inactive statuses).
        /// Department without a status field, or an empty status: both results are null.
        /// A status ID that has no PKProjectStatus row keeps its ID and gets a null text.
        /// </summary>
        public static void Resolve(string departmentID, Func<string, int?> readStatusField, Func<string, int, string> describe,
            out int? statusID, out string statusText)
        {
            if (readStatusField == null) throw new ArgumentNullException(nameof(readStatusField));
            if (describe == null) throw new ArgumentNullException(nameof(describe));
            statusID = null;
            statusText = null;

            string dept = CanonicalDepartment(departmentID);
            if (dept == null) return;
            statusID = readStatusField(StatusFieldByDepartment[dept]);
            if (statusID == null) return;
            string text = describe(dept, statusID.Value);
            statusText = string.IsNullOrEmpty(text) ? null : text;
        }
    }
}
