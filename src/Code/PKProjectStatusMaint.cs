using System;
using System.Collections.Generic;
using System.Globalization;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.CS;

namespace ProjectTask
{
    /// <summary>
    /// PKPM2001 "Department Statuses": Department filter + editable status grid (AP202000 APVendorPriceMaint pattern).
    /// Rules: StatusID > 0, unique per department, defaults to max + 1, read-only once saved; Description required and
    /// unique per department (case-insensitive); saved rows cannot be deleted (make them inactive).
    /// On save, in the same transaction, the statuses of every touched department (and of the selected one) are copied
    /// one way into CSAttributeDetail (STATUS401K / STATUSTAX / STATUSACCT / STATUSAUDI; ValueID = 2-digit ID,
    /// Disabled = !Active) through CSAttributeMaint, so GI000142 & co. and PKTimeClock keep working (design option C).
    /// The sync never deletes CSAttributeDetail rows.
    /// </summary>
    public class PKProjectStatusMaint : PXGraph<PKProjectStatusMaint>
    {
        /// <summary>Department -> CSAttribute that GIs / PKTimeClock read. Departments not listed are not synced.</summary>
        public static readonly IReadOnlyDictionary<string, string> AttributeByDepartment =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "401K", "STATUS401K" },
                { "TAX", "STATUSTAX" },
                { "ACCT", "STATUSACCT" },
                { "AUDIT", "STATUSAUDI" },
            };

        public PXSave<PKProjectStatusFilter> Save;
        public PXCancel<PKProjectStatusFilter> Cancel;

        public PXFilter<PKProjectStatusFilter> Filter;

        public SelectFrom<PKProjectStatus>
            .Where<PKProjectStatus.departmentID.IsEqual<PKProjectStatusFilter.departmentID.FromCurrent>>
            .OrderBy<PKProjectStatus.sortOrder.Asc, PKProjectStatus.statusID.Asc>
            .View Statuses;

        #region Filter
        protected virtual void _(Events.RowSelected<PKProjectStatusFilter> e)
        {
            Statuses.Cache.AllowInsert = !string.IsNullOrEmpty(e.Row?.DepartmentID);
        }
        #endregion

        #region PKProjectStatus
        protected virtual void _(Events.RowSelected<PKProjectStatus> e)
        {
            if (e.Row == null) return;
            PXUIFieldAttribute.SetEnabled<PKProjectStatus.statusID>(e.Cache, e.Row, !IsSaved(e.Cache, e.Row));
        }

        protected virtual void _(Events.FieldDefaulting<PKProjectStatus, PKProjectStatus.statusID> e)
        {
            string dept = e.Row?.DepartmentID ?? Filter.Current?.DepartmentID;
            if (string.IsNullOrEmpty(dept)) return;
            int max = 0;
            foreach (PKProjectStatus s in StatusesOf(dept))
                if (s.StatusID > max) max = s.StatusID.Value;
            e.NewValue = max + 1;
        }

        protected virtual void _(Events.FieldDefaulting<PKProjectStatus, PKProjectStatus.sortOrder> e)
        {
            if (e.Row?.StatusID != null) e.NewValue = Math.Min(e.Row.StatusID.Value, 32767);
        }

        protected virtual void _(Events.FieldVerifying<PKProjectStatus, PKProjectStatus.departmentID> e)
        {
            if (e.Row == null || e.Row.DepartmentID == null) return;
            if (!string.Equals(e.Row.DepartmentID, e.NewValue as string, StringComparison.OrdinalIgnoreCase) && IsSaved(e.Cache, e.Row))
                throw new PXSetPropertyException(PKProjectStatusMessages.DepartmentReadOnly);
        }

        // IMPORTANT (bug fixed 2026-11-21, T-21b): do NOT look at other rows in the FieldVerifying handlers of key fields.
        // In the grid update path (PXCache.Update(keys, values) -> FillWithValues) Acumatica fills a COPY of the row field by field
        // and raises FieldVerifying while the copy's own key fields are still empty. A department scan then finds the saved row
        // itself ("Status ID 2 already exists for department ACCT" on every rename) and the automatic session rollback also
        // throws away a pending new row's description ("description cannot be blank"). Uniqueness is therefore checked where the
        // whole row is known: RowInserted / RowUpdated (inline field error) and RowPersisting (hard stop).
        protected virtual void _(Events.FieldVerifying<PKProjectStatus, PKProjectStatus.statusID> e)
        {
            if (e.Row == null || !(e.NewValue is int newID)) return;
            if (newID <= 0)
                throw new PXSetPropertyException(PKProjectStatusMessages.StatusIDMustBePositive);
        }

        protected virtual void _(Events.RowInserted<PKProjectStatus> e) => FlagDuplicateDescription(e.Cache, e.Row);

        protected virtual void _(Events.RowUpdated<PKProjectStatus> e) => FlagDuplicateDescription(e.Cache, e.Row);

        /// <summary>Inline field error when another status of the department already has this description; cleared when fixed.</summary>
        private void FlagDuplicateDescription(PXCache cache, PKProjectStatus row)
        {
            if (row == null) return;
            PKProjectStatus dup = FindDuplicateDescription(row, row.Description);
            if (dup != null)
            {
                cache.RaiseExceptionHandling<PKProjectStatus.description>(row, row.Description,
                    new PXSetPropertyException(PKProjectStatusMessages.DescriptionExists, row.Description.Trim(), dup.StatusID, dup.DepartmentID));
            }
            else
            {
                string current = PXUIFieldAttribute.GetError<PKProjectStatus.description>(cache, row);
                if (current != null && current.Contains("is already used by status"))
                    cache.RaiseExceptionHandling<PKProjectStatus.description>(row, row.Description, null);
            }
        }

        protected virtual void _(Events.RowDeleting<PKProjectStatus> e)
        {
            if (e.Row != null && IsSaved(e.Cache, e.Row))
                throw new PXException(PKProjectStatusMessages.CannotDeleteSaved);
        }

        protected virtual void _(Events.RowPersisting<PKProjectStatus> e)
        {
            if (e.Row == null) return;
            PXDBOperation command = e.Operation & PXDBOperation.Command;
            if (command == PXDBOperation.Delete)
                throw new PXException(PKProjectStatusMessages.CannotDeleteSaved);

            if (e.Row.StatusID == null || e.Row.StatusID <= 0)
            {
                if (e.Cache.RaiseExceptionHandling<PKProjectStatus.statusID>(e.Row, e.Row.StatusID,
                        new PXSetPropertyException(PKProjectStatusMessages.StatusIDMustBePositive)))
                    throw new PXRowPersistingException(typeof(PKProjectStatus.statusID).Name, e.Row.StatusID, PKProjectStatusMessages.StatusIDMustBePositive);
            }

            // A new row may not reuse the ID of a saved status of the same department (friendlier than the SQL key violation).
            if (command == PXDBOperation.Insert)
            {
                foreach (PKProjectStatus s in StatusesOf(e.Row.DepartmentID))
                {
                    if (s.StatusID == e.Row.StatusID && !ReferenceEquals(s, e.Row) && e.Cache.GetStatus(s) != PXEntryStatus.Inserted)
                    {
                        var idEx = new PXSetPropertyException(PKProjectStatusMessages.StatusIDExists, e.Row.StatusID, e.Row.DepartmentID);
                        if (e.Cache.RaiseExceptionHandling<PKProjectStatus.statusID>(e.Row, e.Row.StatusID, idEx))
                            throw new PXRowPersistingException(typeof(PKProjectStatus.statusID).Name, e.Row.StatusID, idEx.Message);
                    }
                }
            }

            // Final uniqueness check across all rows being saved (e.g. two descriptions swapped in one save).
            PKProjectStatus dup = FindDuplicateDescription(e.Row, e.Row.Description);
            if (dup != null)
            {
                var ex = new PXSetPropertyException(PKProjectStatusMessages.DescriptionExists, e.Row.Description.Trim(), dup.StatusID, dup.DepartmentID);
                if (e.Cache.RaiseExceptionHandling<PKProjectStatus.description>(e.Row, e.Row.Description, ex))
                    throw new PXRowPersistingException(typeof(PKProjectStatus.description).Name, e.Row.Description, ex.Message);
            }
        }
        #endregion

        #region Persist + CSAttributeDetail sync
        public override void Persist()
        {
            var departments = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (PKProjectStatus row in Statuses.Cache.Inserted) AddDepartment(departments, row.DepartmentID);
            foreach (PKProjectStatus row in Statuses.Cache.Updated) AddDepartment(departments, row.DepartmentID);
            // The selected department is synced on every save, so a plain Save pushes it to CSAttributeDetail
            // (first sync after publish; also repairs edits made on CS205000, which the sync owns now).
            AddDepartment(departments, Filter.Current?.DepartmentID);

            using (var ts = new PXTransactionScope())
            {
                base.Persist();
                if (departments.Count > 0)
                {
                    Dictionary<string, List<PKProjectStatusInfo>> saved = PKProjectStatusListAttribute.ReadFromDatabase();
                    foreach (string dept in departments)
                        SyncAttributeDetails(dept, saved.TryGetValue(dept, out List<PKProjectStatusInfo> list) ? list : new List<PKProjectStatusInfo>());
                }
                ts.Complete();
            }
            PKProjectStatusListAttribute.ResetCache();
        }

        /// <summary>
        /// Upserts the department's saved statuses into CSAttributeDetail of its attribute (ValueID = 2-digit ID,
        /// Description, SortOrder, Disabled = !Active) through CSAttributeMaint, which also rebuilds CSAttribute.List.
        /// Rows that already match are left alone; rows that exist only in CSAttributeDetail (e.g. ACCT 00) are never touched.
        /// </summary>
        protected virtual void SyncAttributeDetails(string departmentID, IEnumerable<PKProjectStatusInfo> statuses)
        {
            if (!AttributeByDepartment.TryGetValue(departmentID, out string attributeID)) return;

            CSAttributeMaint attrGraph = PXGraph.CreateInstance<CSAttributeMaint>();
            CSAttribute attribute = SelectFrom<CSAttribute>
                .Where<CSAttribute.attributeID.IsEqual<@P.AsString>>
                .View.Select(attrGraph, attributeID);
            if (attribute == null)
            {
                PXTrace.WriteWarning(PXMessages.LocalizeFormatNoPrefix(PKProjectStatusMessages.AttributeMissing, attributeID, departmentID));
                return;
            }
            attrGraph.Attributes.Current = attribute;

            var existing = new Dictionary<string, CSAttributeDetail>(StringComparer.OrdinalIgnoreCase);
            foreach (CSAttributeDetail d in attrGraph.AttributeDetails.Select())
                if (d.ValueID != null) existing[d.ValueID.Trim()] = d;

            foreach (PKProjectStatusInfo s in statuses)
            {
                string valueID = s.StatusID.ToString("00", CultureInfo.InvariantCulture);
                short sortOrder = (short)Math.Max(0, Math.Min(s.SortOrder, short.MaxValue));
                bool disabled = !s.Active;

                if (existing.TryGetValue(valueID, out CSAttributeDetail detail))
                {
                    if (detail.Description == s.Description && detail.SortOrder == sortOrder && detail.Disabled == disabled)
                        continue;
                    var copy = (CSAttributeDetail)attrGraph.AttributeDetails.Cache.CreateCopy(detail);
                    copy.Description = s.Description;
                    copy.SortOrder = sortOrder;
                    copy.Disabled = disabled;
                    attrGraph.AttributeDetails.Update(copy);
                }
                else
                {
                    attrGraph.AttributeDetails.Insert(new CSAttributeDetail
                    {
                        AttributeID = attributeID,
                        ValueID = valueID,
                        Description = s.Description,
                        SortOrder = sortOrder,
                        Disabled = disabled,
                    });
                }
            }

            if (attrGraph.IsDirty)
                attrGraph.Persist();
        }
        #endregion

        #region Helpers
        private static void AddDepartment(HashSet<string> set, string departmentID)
        {
            if (!string.IsNullOrWhiteSpace(departmentID)) set.Add(departmentID.Trim());
        }

        /// <summary>A row read from the database (not a new, unsaved row).</summary>
        private static bool IsSaved(PXCache cache, PKProjectStatus row)
        {
            object located = cache.Locate(row);
            if (located == null) return false;
            PXEntryStatus status = cache.GetStatus(located);
            return status == PXEntryStatus.Notchanged || status == PXEntryStatus.Updated
                || status == PXEntryStatus.Deleted || status == PXEntryStatus.Held;
        }

        /// <summary>All statuses of a department: saved rows merged with this graph's unsaved changes.</summary>
        private IEnumerable<PKProjectStatus> StatusesOf(string departmentID)
        {
            if (string.IsNullOrEmpty(departmentID)) yield break;
            foreach (PKProjectStatus s in SelectFrom<PKProjectStatus>
                .Where<PKProjectStatus.departmentID.IsEqual<@P.AsString>>
                .View.Select(this, departmentID))
                yield return s;
        }

        /// <summary>Another status of the same department with the same description (trimmed, case-insensitive).</summary>
        private PKProjectStatus FindDuplicateDescription(PKProjectStatus row, string description)
        {
            string text = description?.Trim();
            if (string.IsNullOrEmpty(text)) return null;
            string dept = row.DepartmentID ?? Filter.Current?.DepartmentID;
            foreach (PKProjectStatus s in StatusesOf(dept))
            {
                if (s.StatusID == row.StatusID) continue;   // the row itself
                if (string.Equals(s.Description?.Trim(), text, StringComparison.OrdinalIgnoreCase))
                    return s;
            }
            return null;
        }
        #endregion
    }
}
