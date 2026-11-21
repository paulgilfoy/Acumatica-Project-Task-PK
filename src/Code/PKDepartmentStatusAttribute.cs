using System;
using System.Collections.Generic;
using System.Linq;
using PX.Data;

namespace ProjectTask
{
    /// <summary>
    /// T-25: keeps the stored, read-only "PK Status" of a project current. Placed on ContractExt.UsrPKStatusID:
    /// <code>[PXDBInt] [PKDepartmentStatus(typeof(usrPGDepartment), typeof(usrPKStatus))] [PXUIField(..., Enabled = false)]</code>
    /// On every INSERT or UPDATE of a Contract / PMProject row, in ANY graph (ProjectEntry, Copy Project, create from template,
    /// import scenarios, mass updates, PKTimeClock graphs, ...), RowPersisting sets
    ///   UsrPKStatusID = the status field of the row's own department (<see cref="PKDepartmentStatus"/>), or null;
    ///   UsrPKStatus   = that status's description from the cached PKProjectStatus list (active or inactive), or null.
    /// Why RowPersisting: Acumatica raises it right BEFORE it builds the INSERT/UPDATE from the row's values (PXCache.PersistUpdated
    /// / PersistInserted: OnRowPersisting, then OnCommandPreparing per field; IL 26.200), so the two values ride in the same
    /// statement: no extra query, no extra write. The text comes from PKProjectStatusListAttribute.GetStatuses (PXDatabase slot,
    /// read once per company until PKProjectStatus changes), so a save costs no database round trip either.
    /// It is a DAC attribute, not a graph extension, so it needs no coordination with PKProjectTemplateExt or PKTimeClock's
    /// PKProjectEntryExt. Rows changed outside a graph (raw SQL, PXDatabase.Update) are not covered: PKPM2001 updates the text of
    /// renamed statuses itself, and src/Sql/PKProjectStatusBackfill.sql recalculates everything (docs/pk-status.md).
    /// No FieldVerifying / cross-row logic here (PLAN §8 lesson).
    /// </summary>
    public class PKDepartmentStatusAttribute : PXEventSubscriberAttribute, IPXRowPersistingSubscriber
    {
        private readonly Type _departmentField;
        private readonly Type _textField;
        private string _departmentFieldName;
        private string _textFieldName;

        /// <param name="departmentField">BQL field with the project's department (ContractExt.usrPGDepartment).</param>
        /// <param name="textField">BQL field that receives the status text (ContractExt.usrPKStatus).</param>
        public PKDepartmentStatusAttribute(Type departmentField, Type textField)
        {
            _departmentField = departmentField ?? throw new ArgumentNullException(nameof(departmentField));
            _textField = textField ?? throw new ArgumentNullException(nameof(textField));
        }

        public override void CacheAttached(PXCache sender)
        {
            base.CacheAttached(sender);
            _departmentFieldName = ResolveFieldName(sender, _departmentField);
            _textFieldName = ResolveFieldName(sender, _textField);
        }

        public virtual void RowPersisting(PXCache sender, PXRowPersistingEventArgs e)
        {
            if (e.Row == null) return;
            PXDBOperation command = e.Operation & PXDBOperation.Command;
            if (command != PXDBOperation.Insert && command != PXDBOperation.Update) return;
            Apply(sender, e.Row);
        }

        /// <summary>Recomputes both values on the row (plain SetValue: no field events, no verification, no other rows).</summary>
        protected virtual void Apply(PXCache sender, object row)
        {
            PKDepartmentStatus.Resolve(
                sender.GetValue(row, _departmentFieldName) as string,
                field => sender.GetValue(row, field) as int?,
                Describe,
                out int? statusID, out string statusText);
            sender.SetValue(row, _FieldName, statusID);
            sender.SetValue(row, _textFieldName, statusText);
        }

        /// <summary>Description of a department's status from the cached list (active and inactive statuses); null if unknown.</summary>
        public static string Describe(string departmentID, int statusID)
        {
            IReadOnlyList<PKProjectStatusInfo> statuses = PKProjectStatusListAttribute.GetStatuses(departmentID);
            return statuses.FirstOrDefault(s => s.StatusID == statusID)?.Description;
        }

        private static string ResolveFieldName(PXCache sender, Type bqlField)
        {
            // PXCache.GetField matches the BQL class name against the cache's fields (case-insensitive); fall back to the
            // property naming convention (usrPKStatus -> UsrPKStatus) so a lookup during cache construction cannot yield null.
            string name = sender.GetField(bqlField);
            if (!string.IsNullOrEmpty(name)) return name;
            string n = bqlField.Name;
            return char.ToUpperInvariant(n[0]) + n.Substring(1);
        }
    }
}
