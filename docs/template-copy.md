# T-32 — Copy template / common-task values into new projects and tasks

Date 2026-11-20, updated 2026-11-21. Acumatica 26.200.0334 (PK9). **Status:** published on PK9 2026-11-21 in the merged package; the
due-date rule was then switched to **as-is (D9, T-32b)**, which ships with the republish; the manual test (§7) **passed (user, 2026-11-22)**. Live untouched. No DB writes by agents.
Design and hook evidence: `docs/template-flow.md` (T-30). Screens that store the values: `docs/template-screens.md` (T-31).
T-31 (entry screens) and T-32 (copy logic) belong together (without T-31 nobody can enter the values; without T-32 they are only stored).

Files:
- `src/Code/PKProjectTemplateExt.cs` — `ProjectTask.PKProjectTemplateExt : PXGraphExtension<ProjectEntry>` with two `[PXOverride]`s
  (code item, `FileType="NewFile"`: the default in `tools/Build-Package.ps1`; TimeClock's live `PKProjectEntryExt` graph
  extension is also `NewFile`, read-only `CustObject` check).
- `src/Code/PKTemplateDueDate.cs` — `ProjectTask.PKTemplateDueDate`, static, **no Acumatica references** (the due-date rule).
- `tools/Test-TemplateDates.ps1` — compiles `PKTemplateDueDate.cs` + a console harness and runs 36 cases.

## 1. What is copied, and when
Rule for every field: **copied only when the new project's / new task's value is still empty**; a value already on the target
is never overwritten. Nothing else is touched: **no status is set** (no `UsrPGTaskStatus`, no department status fields),
no Extended Due Date / Is Extended, no task Department (it keeps defaulting from the project's Department), no Tech Status (removed).

| Field | From | To | Create project from template (PM301000, Template on a new project; also quote → project) | Load from Template (existing project) | Add Common Task (PM301000 Tasks tab; template or global common task) | Copy Project / Create Template |
|---|---|---|---|---|---|---|
| `UsrPGDepartment` (Department) | template (PM208000 *PK Project Info*) | project | yes, if empty | yes, if empty | — | **not affected** (as today) |
| `UsrPGPriority` (Project Priority) | template | project | yes, if empty | yes, if empty | — | not affected |
| `UsrPGComplexity` (Project Complexity) | template | project | yes, if empty | yes, if empty | — | not affected |
| `UsrPGPreparer` | template task (PM208000 grid / PM208010) or common task (PM208030 / template non-auto task) | new task | yes, if empty (tasks with *Automatically Include in Project*) | yes, if empty | yes, if empty | not affected |
| `UsrPGReviewer` | idem | new task | yes, if empty | yes, if empty | yes, if empty | not affected |
| `UsrPGTechReviewer` | idem | new task | yes, if empty | yes, if empty | yes, if empty | not affected |
| `UsrPGDueDate` (Due Date) | idem | new task | **copied as-is** (D9, section 2) | idem | idem | not affected |

Hooks (both verified again for this task, section 6):
- project fields: `[PXOverride] DefaultFromTemplateProjectSettings(PMProject prj, PMProject templ)` (public virtual; runs before any
  task is inserted, so the copied Department is on the project when the task Department default fires);
- task fields: `[PXOverride] OnCopyPasteTaskInserted(PMProject target, PMTask newItem, PMTask sourceItem)` (protected virtual; called
  by `CopyTaskBase<ProjectEntry>.CopyTask` for **both** create-from-template and Add Common Task). Guard: if `newItem` is not the
  row in this graph's `Tasks` cache (Copy Project, Create Template: the row lives in another graph) the hook does nothing, so those
  two actions behave exactly as before.

## 2. Due-date rule: AS-IS (D9, decided by the user 2026-11-21; active)
**The template or common-task due date is copied unchanged, whatever the creation date or the project's start date.** Due dates are
usually static dates the user can modify; projects are created in bulk and the user bulk-updates dates (e.g. all "2026" tax projects to
4/15/2027). A tax project created after 4/15 is back-dated to 4/15/2027 and then the "Extended" checkbox is checked for the IRS extension
filing. A blank template date leaves the field empty. Implemented by `PKTemplateDueDate.ActiveRule = AsIs` (T-32b); the
`tools\Test-TemplateDates.ps1` default is `-ExpectRule AsIs` (36 of 36 cases). The hook still passes the project start date and business
date to `PKTemplateDueDate.GetDueDate`, but `AsIs` ignores them.

### Option C (roll forward): the documented alternative, NOT active
The rule the agent first chose (not confirmed by the user, superseded by D9). Switch back by setting `ActiveRule = DueDateRule.RollForward`
(section 4). Everything below up to the "Known limitation" paragraph describes this alternative only.

**Option C rule: keep the template date's month and day, ignore its year, and use the first such date ON OR AFTER the project's start date.**
Reason: PK due dates are statutory calendar dates per task code (011065 always 3/15, 011040 4/15, 02TXFRA 5/15).

| Template task | Project start | Due date on the new task |
|---|---|---|
| 011065 3/15/2025 | 11/20/2026 (created today) | **3/15/2027** |
| 011040 4/15/2025 | 11/20/2026 | **4/15/2027** |
| 02TXFRA 5/15/2025 | 11/20/2026 | **5/15/2027** |
| 011065 3/15/2025 | 1/5/2026 | 3/15/2026 |
| 011040 4/15/2025 | 1/5/2026 | 4/15/2026 |
| 4/15/2025 | 4/15/2026 (same day) | 4/15/2026 |
| 4/15/2025 | 4/16/2026 | 4/15/2027 |
| 5/15/2030 (template year in the future) | 2/1/2026 | 5/15/2026 |
| 2/29/2024 | 1/5/2026 (2026 not a leap year) | **2/28/2026** |
| 2/29/2024 | 12/1/2027 | 2/29/2028 |
| 2/29/2024 | 3/1/2026 | 2/28/2027 |
| 1/1/1900 (old placeholder) | 11/20/2026 | 1/1/2027 |
| empty | any | empty (nothing copied) |

All rows are cases in `tools/Test-TemplateDates.ps1` (36 cases, output in section 6).

**Which start date (option C only; evidence kept for reference):**
- *Create project from a template* (also Load from Template and quote → project): `ProjectEntry.DefaultFromTemplate` sets
  `prj.StartDate = Accessinfo.BusinessDate` **unconditionally** (IL_00d6–IL_00e2) before it copies the tasks (IL_01c2), so the anchor is
  the **business date of the day the template is chosen**. Changing the project's Start Date afterwards does **not** recalculate the
  due dates (edit them by hand). Note: the IL shows the same assignment also runs for *Load from Template* on an existing project
  (base Acumatica behaviour, not ours).
- *Add Common Task*: `CopyTaskBase.CopyTask` passes `ProjectView.Current` as `target` (IL), i.e. **the project's current Start Date**
  as shown on the screen (unsaved edits included). For a project that started in an earlier year this gives a date in that year
  (e.g. project started 1/5/2024, common task 3/15 → 3/15/2024, test case 23); PK projects are yearly engagements, so this matches the
  project's year, but check it when adding tasks to old projects.
- *No start date*: falls back to the **business date**; if that is empty too, the template date is copied as is. Evidence that
  this is a theoretical case: `PMProject.StartDate` is `[PXDefault(typeof(AccessInfo.businessDate))]` (required, no
  `PersistingCheck.Nothing`), `DefaultFromTemplate` sets it as above, and read-only SQL on PK9: **0 of 11,408 projects** (`BaseType='P'`)
  have no StartDate (all 51 templates have none, which does not matter: the anchor is the target project).
- *29 Feb*: 29 Feb in a leap year, otherwise **28 Feb** (last day of February, never 1 Mar). PK9 has exactly 1 such task due date today.
- Time of day is dropped (the field is a date).

**Known limitation of option C (data; one reason it is not the active rule):** read-only SQL on PK9: of 8,784 project tasks with a due date, 1,035 have a due date
before their project's start date: 446 are 1/1/1900-era placeholders, **589** are real (66 within 31 days, 387 within 32–183 days,
136 later). Those are projects opened after the deadline (late or prior-year work); under option C such a task would get next
year's date and must be corrected by hand. A grace window (e.g. "if the date passed less than N days ago, keep this year") could
be added inside `PKTemplateDueDate.RollForward` if the user wants it; not implemented.

## 3. Edge cases and behaviour
- **Target already has a value** (user set Department before choosing the template; Load from Template on a project that already
  has Priority; a task already holding a Preparer): kept, not overwritten.
- **Template values blank** (all templates today): nothing happens; projects are created exactly as before.
- **Inactive employee on a template task:** `PXEPEmployeeSelector` (on Preparer/Reviewer/Tech Reviewer) is
  `Search<EPEmployee.bAccountID>` **without a status condition** (IL of its constructor), so an inactive employee is **copied
  normally** (same as entering it by hand today); it does not block anything.
- **Invalid value** (e.g. a deleted employee or department): IL of `PXCache<T>.SetValueExt` shows it does **not** throw: it catches the
  `PXSetPropertyException`, `PXUIFieldAttribute.ExceptionHandling` parks it as an **Error** on the field (value not set) and writes a trace
  warning; `PXUIFieldAttribute.CommandPreparing` would then make the **save fail** ("Inserting 'Project Task' record raised at least one
  error"). The prototype's `try/catch` alone would not have caught that. `TrySetValue` therefore reads the error back
  (`PXUIFieldAttribute.GetErrorOnly` = Error/RowError only), clears it (`SetError(..., null)`), and sets a **field warning** instead
  (`SetWarning`, level Warning, which `CommandPreparing` skips) plus `PXTrace.WriteWarning`. The project is created; the field stays
  empty with a yellow warning "The value … was not copied from the template: …". A thrown `PXSetPropertyException` is handled the same way.
  Inside *Load from Template* (long operation + save) the row warning is not visible afterwards; the trace keeps it.
- **Priority / Complexity** are `PXIntList` fields (no verification), **Department** is a `PXSelector` on `EPDepartment`.
- **Copy Project / Create Template**: unchanged (guard); D4 (should they carry `UsrPG*`?) stays open — today Copy Project already
  copies `ContractExt` values by itself (`Project.Insert(CreateCopy(source))`), Create Template does not.
- **Change orders** (`ChangeOrderEntry.CreateTaskFromTemplate`) are not covered (different graph; PK does not use it).
- **TimeClock coexistence**: TimeClock's `PKProjectEntryExt` overrides the same `DefaultFromTemplateProjectSettings`; both are chained
  (binder output, section 6), both call `baseMethod` first and write different fields, so the order does not matter.

## 4. Switching the due-date rule
All due-date logic is in `src/Code/PKTemplateDueDate.cs`; `PKProjectTemplateExt.GetDueDate` only passes
`(template due date, project StartDate, business date)` to `PKTemplateDueDate.GetDueDate`.
- **Option A (copy as is): ACTIVE since T-32b (D9).** `ActiveRule = DueDateRule.AsIs`; `tools\Test-TemplateDates.ps1` defaults to `-ExpectRule AsIs`.
- **Option C (roll forward): the alternative.** Change `public const DueDateRule ActiveRule = DueDateRule.AsIs;` to `DueDateRule.RollForward`, run
  `tools\Test-TemplateDates.ps1 -ExpectRule RollForward` (and change the script's `-ExpectRule` default), rebuild the package.
- **Option B (N days after project start):** needs a new int field on template/common tasks (e.g. `UsrPGDueDays`: DAC field in
  `PMTask.cs`, column in `Tables/PMTask.xml`, T-31 UI); add a rule `OffsetDays` to `PKTemplateDueDate` taking the offset, pass it from
  `PKProjectTemplateExt.GetDueDate`, add cases to the test.
- **Grace window / other tweaks to C (only if C is ever activated):** change `RollForward`; add cases to the test table.
- **Option D (offset from Year End):** larger change (2 fields + recalculation when Year End is entered later), see `template-flow.md` §6.

## 5. Not verifiable before a publish
- That the overrides fire at runtime (IL + Acumatica's own binder say yes, section 6).
- `SetValueExt` on the just-inserted task/project inside these hooks (events, selector validation, the error→warning conversion,
  the warning being visible in the Modern UI grid).
- The task `UsrPGDepartment` default picking up the copied project Department for an unsaved project.
- Add Common Task dialog behaviour in the Modern UI.
- Whether the business date / start date seen by the hook is what section 2 says (IL-derived).

## 6. Checks (2026-11-20, main checkout; historical, run when option C was the default; re-run 2026-11-21 with AsIs: DATE TEST PASS 36/36, COMPILE OK without the Contact exclusion)
```
tools\Test-TemplateDates.ps1          36 cases, passed 36, failed 0  -> DATE TEST PASS (exit 0)
                                      (negative control: a deliberately wrong expectation -> FAIL, exit 1)
tools\Test-Compile.ps1 -IncludeRuntimeCode -ExcludeRuntimeFile PX_Objects_CR_Contact_extensions   (no longer needed, PLAN §9)
                                      Compiling 51 file(s) ... COMPILE OK   (includes App_RuntimeCode\PKProjectEntryExt.cs of PKTimeClock)
tools\Test-ModernUI.ps1               Type-checking 12 file(s) with Version 5.5.2 ... TS CHECK OK
tools\Build-Package.ps1               57 items: Code 14, Sql 3, Table 3, SiteMapNode 3, Automation 2, Page 5, File 6, PerTenantFile 21
tools\Verify-Package.ps1 -ExtraProject Contact
                                      Problems: 23 = 21 before T-32 (5 T-11 DIFFs, 10 T-21 NOT LIVE, 6 T-31 NOT LIVE)
                                      + 2 T-32 NOT LIVE (Code#PKProjectTemplateExt, Code#PKTemplateDueDate); runtime-copy warnings 6 (+2 new)
tools\Test-StatusSeed.ps1             SEED TEST PASS (regression)
```
Signatures (`..\Acumatica-PK-TimeClock\tools\Inspect-Acumatica.ps1`, PX.Objects 26.200.0334):
```
METH  [ProjectEntry] protected virtual Void OnCopyPasteTaskInserted(PMProject target, PMTask newItem, PMTask sourceItem)
METH  [ProjectEntry] public virtual Void DefaultFromTemplateProjectSettings(PMProject prj, PMProject templ)
PROP  [PMProject] Nullable`1 StartDate  @PXDBDateAttribute @PXDefaultAttribute typeof(PX.Data.AccessInfo+businessDate) @PXUIFieldAttribute DisplayName = "Start Date"
```
Acumatica's override binder run offline on the compiled check DLL (`PXExtensionManager.Sort` + `GetExtMethodsGroupByBaseMethod`,
recipe in `template-flow.md` §5):
```
Sort() order: PKTimeClock.PKProjectEntryExt  ->  ProjectTask.PKProjectTemplateExt  ->  PX.Objects.PM.ProjectEntry_CopyTaskExt
BASE  Void DefaultFromTemplateProjectSettings(PX.Objects.PM.PMProject, PX.Objects.PM.PMProject)
   <- [PXOverride] PKTimeClock.PKProjectEntryExt.DefaultFromTemplateProjectSettings (params: 3)
   <- [PXOverride] ProjectTask.PKProjectTemplateExt.DefaultFromTemplateProjectSettings (params: 3)
BASE  Void OnCopyPasteTaskInserted(PX.Objects.PM.PMProject, PX.Objects.PM.PMTask, PX.Objects.PM.PMTask)
   <- [PXOverride] ProjectTask.PKProjectTemplateExt.OnCopyPasteTaskInserted (params: 4)
```

## 7. Manual test on PK9 (after the user republishes the T-32b build; adapted from `template-flow.md` §8). Expected due dates follow the AS-IS rule (D9)
**Result (user, 2026-11-22): ALL PASS on PK9**, including Add Common Task (build `15ee9d4`). The user considers common tasks finished as built; team alpha testers give feedback next. Goes live in release 1.
1. Backup; import `build\ProjectTask.zip`; publish with the usual projects (ProjectTask, PKTimeClock, …; `Contact` unticked).
2. **PM208000**: copy a template to `T32TEST`. *PK Project Info*: Department = TAX, Priority = High, Complexity = Level 2; TimeClock's
   Default Labor Item set. Tasks grid: Task A (*Automatically Include in Project* = on): Preparer, Reviewer, Tech Reviewer,
   Due Date **04/15/2025**; Task B (*Automatically Include* = off → a common task of the template): Preparer, Due Date **03/15/2025**. Save.
3. **PM208030**: global common task `03AL`: Preparer + Due Date **05/15/2025**. Save.
4. **PM301000** → new project, Template = `T32TEST`. Before saving: Department / Priority / Complexity = TAX / High / Level 2; Default
   Labor Item copied (TimeClock still works); Task A: Preparer, Reviewer, Tech Reviewer copied, **Due Date = 04/15/2025, exactly the template
   date, whatever today's date is** (as-is rule); Task A Department = TAX; no task status set. Save, reopen: persisted.
5. Tasks tab → **Add Common Task** → tick Task B and `03AL` → OK. Both new tasks get the Preparer; due dates = the template dates unchanged
   (03/15/2025 and 05/15/2025), regardless of the project's Start Date. Save, reopen.
6. Copy-if-empty: new project, set Department = ACCT first, then choose `T32TEST` → Department stays ACCT, Priority/Complexity filled.
   As-is check on an existing test project with Start Date 01/05/2026: Add Common Task `03AL` → Due Date 05/15/2025 (unchanged, even though it
   is before the start date). Optional: set a template task to 04/15/2027 and confirm 04/15/2027 on a new project.
7. **Copy Project** (PM301000 action) on the project from step 4, and **Create Template** from it: no error; `UsrPG*` values on the
   copy / new template are the same as they would have been before T-32 (the hook skips both; no template values are re-applied,
   no due date is changed).
8. **Inactive employee does not block:** set the Preparer of Task A to an employee, then deactivate that employee (EP203000, Status =
   Inactive); create a project from `T32TEST` → project created and saved; Preparer is copied (the selector has no status filter).
   Optional (only on a throwaway test template): a value that cannot be validated → project still saves; field shows a warning;
   Request Profiler / trace shows "PKProjectTemplateExt: … not copied".
9. Regression: a project from a template with blank PK values is created exactly as before; PM208000/PM208010/PM208030 show the T-31 fields.
10. Read-only check: `SELECT TaskCD, UsrPGPreparer, UsrPGReviewer, UsrPGTechReviewer, UsrPGDueDate, UsrPGTaskStatus FROM PMTask WHERE
    CompanyID=2 AND ProjectID=<new id>` (`UsrPGTaskStatus` stays NULL) and
    `SELECT UsrPGDepartment, UsrPGPriority, UsrPGComplexity FROM Contract WHERE CompanyID=2 AND ContractID=<new id>`.

Rollback: re-import the previous package (removes the two code items); the copy logic only ever wrote to the new rows.


## Update 2026-11-21 (D9): due dates are now copied AS-IS

The user decided that due dates are static dates that are bulk-updated (e.g. all 2026 tax projects to 4/15/2027) and that a project created
after 4/15 keeps the back-dated 4/15/2027 and is then marked Extended for the IRS extension filing. `PKTemplateDueDate.ActiveRule` is now
`AsIs` (the template or common-task date is copied unchanged, whatever the creation date); the RollForward rule (option C) is kept in the
file for reference. `tools\Test-TemplateDates.ps1` now expects `AsIs` by default (36 of 36 cases pass). Manual test: a template task dated
4/15/2027 gives 4/15/2027 on a new project and on Add Common Task, regardless of today's date; a blank template date leaves the field empty.