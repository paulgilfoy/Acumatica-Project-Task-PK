using System;
using PX.Common;
using PX.Data;
using PX.Objects.CT;
using PX.Objects.PM;

namespace ProjectTask
{
    /// <summary>
    /// T-32 (from the T-30 prototype; design and evidence: docs/template-flow.md, behaviour and tests: docs/template-copy.md).
    /// Projects (PM301000, graph ProjectEntry): carry the ProjectTask UsrPG* values from a project template / common task to
    /// the new project and its tasks. Copy rule: only when the target value is still empty. No status is set.
    ///
    /// Hooks (signatures verified by reflection on PX.Objects 26.200.0334):
    /// 1. public virtual ProjectEntry.DefaultFromTemplateProjectSettings(PMProject prj, PMProject templ).
    ///    Called by DefaultFromTemplate (new project from template, quote conversion) and LoadFromTemplate.
    ///    PKTimeClock.PKProjectEntryExt overrides the same method; Acumatica chains both [PXOverride]s (order irrelevant).
    /// 2. protected virtual ProjectEntry.OnCopyPasteTaskInserted(PMProject target, PMTask newItem, PMTask sourceItem), empty in
    ///    the base graph. CopyTaskBase&lt;ProjectEntry&gt;.CopyTask(task, project, notes, files, udfs) ends with
    ///    OnCopyPasteTaskInsertedEx, which ProjectEntry_CopyTaskExt forwards to this graph method. Both flows use that CopyTask:
    ///    "create from template" (DefaultFromTemplate, template tasks with AutoIncludeInPrj) and "Add Common Task"
    ///    (AddTasks -> ProjectEntry.CopyTask -> ProjectEntry_CopyTaskExt.CopyTask). newItem = the task just inserted into
    ///    Base.Tasks; sourceItem = the template / common task (read from the DB, so its UsrPG* values are loaded);
    ///    target = ProjectView.Current (the project on the screen).
    ///    ProjectEntry.Copy (Copy Project) and ProjectEntry.CreateTemplate also call this method, but there newItem lives in
    ///    ANOTHER graph's cache; those calls are skipped on purpose (guard below), so they behave exactly as before.
    /// </summary>
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class PKProjectTemplateExt : PXGraphExtension<ProjectEntry>
    {
        public static bool IsActive() => true;

        #region Project fields: DefaultFromTemplateProjectSettings
        public delegate void DefaultFromTemplateProjectSettingsDelegate(PMProject prj, PMProject templ);

        [PXOverride]
        public virtual void DefaultFromTemplateProjectSettings(PMProject prj, PMProject templ,
            DefaultFromTemplateProjectSettingsDelegate baseMethod)
        {
            baseMethod(prj, templ);

            if (prj == null || templ == null) return;

            ContractExt source = templ.GetExtension<ContractExt>();
            ContractExt target = prj.GetExtension<ContractExt>();
            if (source == null || target == null) return;

            PXCache cache = Base.Project.Cache;
            // Department drives the task department default: copy it first (this runs before any task is inserted).
            if (string.IsNullOrEmpty(target.UsrPGDepartment) && !string.IsNullOrEmpty(source.UsrPGDepartment))
                TrySetValue<ContractExt.usrPGDepartment>(cache, prj, source.UsrPGDepartment);
            if (target.UsrPGPriority == null && source.UsrPGPriority != null)
                TrySetValue<ContractExt.usrPGPriority>(cache, prj, source.UsrPGPriority);
            if (target.UsrPGComplexity == null && source.UsrPGComplexity != null)
                TrySetValue<ContractExt.usrPGComplexity>(cache, prj, source.UsrPGComplexity);
        }
        #endregion

        #region Task fields: OnCopyPasteTaskInserted (create from template AND Add Common Task)
        public delegate void OnCopyPasteTaskInsertedDelegate(PMProject target, PMTask newItem, PMTask sourceItem);

        [PXOverride]
        public virtual void OnCopyPasteTaskInserted(PMProject target, PMTask newItem, PMTask sourceItem,
            OnCopyPasteTaskInsertedDelegate baseMethod)
        {
            baseMethod(target, newItem, sourceItem);

            if (target == null || newItem == null || sourceItem == null) return;

            PXCache cache = Base.Tasks.Cache;
            // Copy Project / Create Template call this hook with a row that belongs to another graph: skip those.
            if (!ReferenceEquals(cache.Locate(newItem), newItem)) return;

            PMTaskExt source = sourceItem.GetExtension<PMTaskExt>();
            PMTaskExt dest = newItem.GetExtension<PMTaskExt>();
            if (source == null || dest == null) return;

            if (dest.UsrPGPreparer == null && source.UsrPGPreparer != null)
                TrySetValue<PMTaskExt.usrPGPreparer>(cache, newItem, source.UsrPGPreparer);
            if (dest.UsrPGReviewer == null && source.UsrPGReviewer != null)
                TrySetValue<PMTaskExt.usrPGReviewer>(cache, newItem, source.UsrPGReviewer);
            if (dest.UsrPGTechReviewer == null && source.UsrPGTechReviewer != null)
                TrySetValue<PMTaskExt.usrPGTechReviewer>(cache, newItem, source.UsrPGTechReviewer);

            if (dest.UsrPGDueDate == null && source.UsrPGDueDate != null)
            {
                DateTime? dueDate = GetDueDate(target, sourceItem, source);
                if (dueDate != null)
                    TrySetValue<PMTaskExt.usrPGDueDate>(cache, newItem, dueDate);
            }
        }

        /// <summary>
        /// Due date of the new task. The rule lives in PKTemplateDueDate (no Acumatica dependencies, unit-tested by
        /// tools\Test-TemplateDates.ps1). Anchor = the project's start date; for a new project from a template that is the
        /// business date (DefaultFromTemplate sets prj.StartDate = Accessinfo.BusinessDate before the tasks are copied); for
        /// Add Common Task it is the project's current start date. Fallback if it is empty: the business date.
        /// </summary>
        protected virtual DateTime? GetDueDate(PMProject target, PMTask sourceTask, PMTaskExt source)
        {
            return PKTemplateDueDate.GetDueDate(source.UsrPGDueDate, target.StartDate, Base.Accessinfo.BusinessDate);
        }
        #endregion

        /// <summary>
        /// Sets a field through the cache (field events, selector validation). A copied value that fails validation (e.g. a
        /// template that still points at a deleted employee or department) must not block creating the project:
        /// PXCache&lt;T&gt;.SetValueExt normally does NOT throw - it catches the PXSetPropertyException and
        /// PXUIFieldAttribute.ExceptionHandling parks it as an Error on the field (value not set), which would make the later
        /// save fail with "... record raised at least one error" (PXUIFieldAttribute.CommandPreparing). So the error is read
        /// back, cleared, and replaced by a field WARNING (does not block the save) plus a trace warning.
        /// </summary>
        private static void TrySetValue<TField>(PXCache cache, object row, object value) where TField : IBqlField
        {
            string field = cache.GetField(typeof(TField));
            string error;
            try
            {
                cache.SetValueExt<TField>(row, value);
                error = PXUIFieldAttribute.GetErrorOnly(cache, row, field);
            }
            catch (PXSetPropertyException ex)
            {
                error = ex.Message;
            }
            if (string.IsNullOrEmpty(error)) return;

            PXUIFieldAttribute.SetError(cache, row, field, null);
            string message = PXMessages.LocalizeFormatNoPrefix(PKTemplateCopyMessages.ValueNotCopied, value, error);
            PXUIFieldAttribute.SetWarning(cache, row, field, message);
            PXTrace.WriteWarning("PKProjectTemplateExt: {0}.{1} not copied: {2}", cache.GetItemType().Name, field, message);
        }
    }

    [PXLocalizable]
    public static class PKTemplateCopyMessages
    {
        public const string ValueNotCopied = "The value {0} was not copied from the template: {1}";
    }
}
