using System;
using System.Collections.Generic;
using System.Linq;
using PX.Common;
using PX.Data;

namespace ProjectTask
{
    /// <summary>One saved row of PKProjectStatus, as read by <see cref="PKProjectStatusListAttribute"/> and the PKPM2001 sync.</summary>
    public sealed class PKProjectStatusInfo
    {
        public string DepartmentID { get; set; }
        public int StatusID { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public bool Active { get; set; }
    }

    /// <summary>
    /// Drop-down list of one department's project statuses, read from table PKProjectStatus (screen PKPM2001).
    /// Replaces a fixed PXIntList on an int field; the department is fixed per field:
    /// <code>[PXDBInt] [PKProjectStatusList("401K")] [PXUIField(DisplayName = "401k Status")]</code>
    /// - CacheAttached sets the FULL list (active and inactive), so every saved value shows its text in GIs, exports, filters.
    /// - FieldSelecting for a row offers the active statuses plus the row's current value (CROpportunityStagesAttribute pattern).
    /// - FieldVerifying rejects an unknown or inactive value that differs from the current one (API, import).
    ///   If the department has no rows at all (table not seeded yet), nothing is rejected, as with the old PXIntList.
    /// - The list is cached in a PXDatabase slot (Box1099NumberAttribute pattern). It resets when PKProjectStatus changes
    ///   through Acumatica and on every PKPM2001 save (<see cref="ResetCache"/>). Rows changed by raw SQL show after a restart.
    /// T-21 adds the attribute; T-22 puts it on the four Contract fields.
    /// </summary>
    public class PKProjectStatusListAttribute : PXIntListAttribute, IPXFieldVerifyingSubscriber
    {
        private const string SlotKey = "ProjectTask.PKProjectStatusListAttribute.Definition";

        public string DepartmentID { get; }

        public PKProjectStatusListAttribute(string departmentID) : base(new int[0], new string[0])
        {
            DepartmentID = departmentID ?? throw new ArgumentNullException(nameof(departmentID));
            IsLocalizable = false;   // user data, not developer strings
        }

        #region Data access
        private sealed class Definition : IPrefetchable, IPXCompanyDependent
        {
            public Dictionary<string, List<PKProjectStatusInfo>> ByDepartment = new Dictionary<string, List<PKProjectStatusInfo>>(StringComparer.OrdinalIgnoreCase);

            public void Prefetch()
            {
                try
                {
                    ByDepartment = ReadFromDatabase();
                }
                catch (Exception ex)
                {
                    // Table missing (Sql item not run yet) or unreadable: an empty list shows the raw ints, it does not break the form.
                    PXTrace.WriteError(PXMessages.LocalizeFormatNoPrefix(PKProjectStatusMessages.StatusListReadFailed, ex.Message));
                    ByDepartment = new Dictionary<string, List<PKProjectStatusInfo>>(StringComparer.OrdinalIgnoreCase);
                }
            }
        }

        /// <summary>
        /// Reads every saved PKProjectStatus row of the current company straight from the database (no graph cache, no slot),
        /// grouped by department and sorted by SortOrder, StatusID. Used by the slot and by the PKPM2001 sync (inside its transaction).
        /// </summary>
        public static Dictionary<string, List<PKProjectStatusInfo>> ReadFromDatabase()
        {
            var result = new Dictionary<string, List<PKProjectStatusInfo>>(StringComparer.OrdinalIgnoreCase);
            foreach (PXDataRecord rec in PXDatabase.SelectMulti<PKProjectStatus>(
                new PXDataField<PKProjectStatus.departmentID>(),
                new PXDataField<PKProjectStatus.statusID>(),
                new PXDataField<PKProjectStatus.description>(),
                new PXDataField<PKProjectStatus.sortOrder>(),
                new PXDataField<PKProjectStatus.active>()))
            {
                var info = new PKProjectStatusInfo
                {
                    DepartmentID = (rec.GetString(0) ?? string.Empty).Trim(),
                    StatusID = rec.GetInt32(1) ?? 0,
                    Description = rec.GetString(2),
                    SortOrder = rec.GetInt32(3) ?? rec.GetInt32(1) ?? 0,
                    Active = rec.GetBoolean(4) ?? false,
                };
                if (!result.TryGetValue(info.DepartmentID, out List<PKProjectStatusInfo> list))
                    result[info.DepartmentID] = list = new List<PKProjectStatusInfo>();
                list.Add(info);
            }
            foreach (List<PKProjectStatusInfo> list in result.Values)
                list.Sort((a, b) => a.SortOrder != b.SortOrder ? a.SortOrder.CompareTo(b.SortOrder) : a.StatusID.CompareTo(b.StatusID));
            return result;
        }

        /// <summary>The cached statuses of a department (active and inactive), in display order. Empty if none.</summary>
        public static IReadOnlyList<PKProjectStatusInfo> GetStatuses(string departmentID)
        {
            Definition def = PXDatabase.GetSlot<Definition>(SlotKey, typeof(PKProjectStatus));
            if (def != null && departmentID != null && def.ByDepartment.TryGetValue(departmentID.Trim(), out List<PKProjectStatusInfo> list))
                return list;
            return new List<PKProjectStatusInfo>();
        }

        /// <summary>Drops the cached lists of the current company (called after every PKPM2001 save).</summary>
        public static void ResetCache()
        {
            PXDatabase.ResetSlot<Definition>(SlotKey, typeof(PKProjectStatus));
        }
        #endregion

        #region Events
        public override void CacheAttached(PXCache sender)
        {
            IReadOnlyList<PKProjectStatusInfo> all = GetStatuses(DepartmentID);
            _AllowedValues = all.Select(s => s.StatusID).ToArray();
            _AllowedLabels = all.Select(s => s.Description).ToArray();
            _NeutralAllowedLabels = _AllowedLabels;
            base.CacheAttached(sender);
        }

        public override void FieldSelecting(PXCache sender, PXFieldSelectingEventArgs e)
        {
            base.FieldSelecting(sender, e);
            if (e.Row == null || !(_AttributeLevel == PXAttributeLevel.Item || e.IsAltered))
                return;

            IReadOnlyList<PKProjectStatusInfo> all = GetStatuses(DepartmentID);
            int? current = sender.GetValue(e.Row, _FieldName) as int?;
            List<PKProjectStatusInfo> offered = all.Where(s => s.Active || s.StatusID == current).ToList();
            if (offered.Count == all.Count)
                return;   // nothing inactive to hide; the full list from base is right

            int[] values = offered.Select(s => s.StatusID).ToArray();
            string[] labels = offered.Select(s => s.Description).ToArray();
            e.ReturnState = PXIntState.CreateInstance(e.ReturnState, _FieldName, null, null, null, null,
                values, labels, _ExclusiveValues, typeof(int), null, labels, null);
        }

        public virtual void FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e)
        {
            if (e.Row == null || !(e.NewValue is int newValue))
                return;
            int? current = sender.GetValue(e.Row, _FieldName) as int?;
            if (current == newValue)
                return;   // unchanged (also an inactive one): no error

            IReadOnlyList<PKProjectStatusInfo> all = GetStatuses(DepartmentID);
            if (all.Count == 0)
                return;   // department not set up (seed not run): accept, like the old PXIntList

            PKProjectStatusInfo status = all.FirstOrDefault(s => s.StatusID == newValue);
            if (status == null)
                throw new PXSetPropertyException(PKProjectStatusMessages.UnknownStatus, newValue, DepartmentID);
            if (!status.Active)
                throw new PXSetPropertyException(PKProjectStatusMessages.InactiveStatus, status.Description, DepartmentID);
        }
        #endregion
    }
}
