# T-30 — Template → project flow and Add Common Task (spike result)

Date 2026-11-20, branch `agent/T-30`. Acumatica **26.200.0334** (`PX.Objects.dll` FileVersion, PK9 `Bin`).
Answers `docs/inventory.md` Q9, Q11 and prepares Q10 (due dates) and Q12 (department) for the user.

**Method (no publish possible).** Reflection and IL reading on `PK9\Bin\PX.Objects.dll` / `PX.Data.dll` / `PX.Common.dll`
(the `Inspect-Acumatica.ps1` resolver plus a small IL reader: call / field / local-variable operands per instruction),
the pre-26R2 base source `Acumatica-Employee-Project-Time\reference\base-code\PX.Objects.PM.ProjectEntry.cs` only where the
IL confirms it, read-only SQL on PK9 (`Query-PK9.ps1`), a compile of the prototype together with all published
`App_RuntimeCode` (incl. TimeClock's `PKProjectEntryExt.cs`), and an **offline run of Acumatica's own override binder**
(`PXExtensionManager.Sort` + `GetExtMethodsGroupByBaseMethod` from `PX.Data.dll`, invoked by reflection on the compiled
prototype). The helper scripts are throwaway (`%LOCALAPPDATA%\Temp\T30\`), not in the repo.

## 1. Short answer
- **One task-level override covers both flows:** `[PXOverride] ProjectEntry.OnCopyPasteTaskInserted(PMProject target,
  PMTask newItem, PMTask sourceItem)` in a plain `PXGraphExtension<ProjectEntry>`. Both "create project from template" and
  "Add Common Task" insert every task through `CopyTaskBase<ProjectEntry>.CopyTask(task, project, notes, files, udfs)`,
  which ends with `OnCopyPasteTaskInsertedEx(project, newTask, sourceTask)`; `ProjectEntry_CopyTaskExt` forwards that to the
  graph's (empty) `protected virtual OnCopyPasteTaskInserted`. The new row and its source row are both arguments, so no
  `taskMap` lookup is needed.
- **Project-level:** `[PXOverride] DefaultFromTemplateProjectSettings(PMProject prj, PMProject templ)` (public virtual), the
  same method TimeClock already overrides. Acumatica chains both overrides (verified with its own binder, section 5).
- **Not suitable alone:** `ProjectEntry.CopyTask` (3-arg; template creation bypasses it) and
  `OnDefaultFromTemplateTasksInserted` (template creation only; Add Common Task never calls it).
- Prototype `src/Code/PKProjectTemplateExt.cs`: **COMPILE OK**; binder shows both overrides bound (section 5).
- **Not verified:** actual runtime behaviour on PK9 (needs a publish, the user's call; section 8).

## 2. Verified members (26.200.0334, exact signatures)
| Member | Declared on | Access | Overridable from | Called by (IL) |
|---|---|---|---|---|
| `void DefaultFromTemplate(PMProject prj, int? templateID, DefaultFromTemplateSettings settings)` | `ProjectEntry` | public virtual | `PXGraphExtension<ProjectEntry>` | TemplateID field-updated handler (`ProjectEntry::_`), `ExecuteUpdate`, `LoadFromTemplate` long-op lambda, `PMQuoteMaint.ConvertQuoteToProject` |
| `void DefaultFromTemplateProjectSettings(PMProject prj, PMProject templ)` | `ProjectEntry` | public virtual | `PXGraphExtension<ProjectEntry>` (TimeClock does) | `DefaultFromTemplate` IL_001f **only if** `settings.CopyProperties && !_isLoadFromTemplate`; `LoadFromTemplate` IL_0052 directly |
| `void OnDefaultFromTemplateTasksInserted(PMProject project, PMProject template, Dictionary<int,int> taskMap)` | `ProjectEntry` | protected virtual, body = `ret` | `PXGraphExtension<ProjectEntry>` | `DefaultFromTemplate` IL_0cdb only |
| `PMTask CopyTask(PMTask task, int ProjectID, DefaultFromTemplateSettings settings)` | `ProjectEntry` | public virtual | `PXGraphExtension<ProjectEntry>` | `AddTasks` IL_0053, `PMQuoteMaint.AddingTasksToProject`. Body: `GetExtension<ProjectEntry_CopyTaskExt>()?.CopyTask(...)` |
| `void OnCopyPasteTaskInserted(PMProject target, PMTask newItem, PMTask sourceItem)` | `ProjectEntry` | protected virtual, body = `ret` | `PXGraphExtension<ProjectEntry>` | `ProjectEntry_CopyTaskExt.OnCopyPasteTaskInsertedEx` (via `[PXProtectedAccess]`), **also** `ProjectEntry.Copy` IL_05ec and `ProjectEntry.CreateTemplate` IL_0638 (row in another graph, see 4.3) |
| `void OnCopyPasteTasksInserted(ProjectEntry target, Dictionary<int,int> taskMap)` | `ProjectEntry` | protected virtual | idem | `ProjectEntry.Copy` only |
| `abstract class CopyTaskBase<T> : PXGraphExtension<T>` | PX.Objects.PM | — | — | — |
| `PMTask CopyTask(PMTask task, int ProjectID, DefaultFromTemplateSettings settings)` | `CopyTaskBase<T>` | public virtual | `PXGraphExtension<ProjectEntry_CopyTaskExt, ProjectEntry>` | `ProjectEntry.CopyTask`, `ProjectEntry_CopyTaskExt.CopyTask` |
| `PMTask CopyTask(PMTask task, PMProject project, bool? copyNotes, bool? copyFiles, bool? copyUdfs)` | `CopyTaskBase<T>` | public virtual | idem | 3-arg `CopyTask`; **`DefaultFromTemplate` IL_01c2 directly** (`CopyTask(srcTask, prj, true, true, true)`) |
| `void OnCopyPasteTaskInsertedEx(PMProject target, PMTask newItem, PMTask sourceItem)` | `CopyTaskBase<T>` | protected virtual | idem | 5-arg `CopyTask` IL_02fe (last statement before `return newItem`) |
| `OnCopyPasteCostBudgetInsertedEx / RevenueBudgetInsertedEx / RecurringItemInsertedEx (PMProject, new, source)` | `CopyTaskBase<T>` | protected virtual | idem | 3-arg `CopyTask` per copied line (useful for T-40) |
| `[PXProtectedAccess] public abstract class ProjectEntry_CopyTaskExt : CopyTaskBase<ProjectEntry>` | PX.Objects.PM | — | — | overrides `CopyTask` (3-arg: re-selects the source `PMTask` by `projectID`+`taskCD`, then base) and the four `On…InsertedEx` (each forwards to the graph's `protected virtual On…Inserted` declared as `[PXProtectedAccess] protected abstract`) |
| `IEnumerable AddTasks(PXAdapter adapter)`, `void ViewAddCommonTask()` | `ProjectEntry` | public virtual | — | Add Common Task dialog |

Evidence that `[PXOverride]` binds **protected** virtual graph methods and protected virtual methods of lower-level
extensions (so both candidate hooks are legal):
- `PXExtensionManager.GetValidExtMethods(Type troot, …)`: `troot.GetMethods(BindingFlags 52 = Instance|Public|NonPublic)`
  filtered by `IsVirtual` (lambda `b__18_0`); extension methods = `GetMethods(60)` filtered by
  `Attribute.IsDefined(m, typeof(PXOverrideAttribute))` (`b__21_2`), matched by name then signature; lower-level extension
  virtual methods are added to the candidate list recursively (`GetValidExtMethodsRec`, `b__21_7` = `IsVirtual`).
- Acumatica's own code does it: scan of all `[PXOverride]` methods in `PX.Objects.dll` (1004) finds many bound to protected
  virtual graph methods, e.g. `PX.Objects.PM.PMTaskEntryValidator.Activate` → protected virtual on `ProjectTaskEntry`,
  `SOOrderEntryExternalTax.RecalculateExternalTaxes` → protected virtual on `SOOrderEntry`, and 394 bound to lower-level
  extensions (e.g. `UpdatePOOnReleaseExt.SetProjectForPPVTransaction` → protected virtual on `UpdatePOOnRelease`).
  `PX.Objects...ProjectEntryLienWaiverExtension.DefaultFromTemplate` is a built-in `[PXOverride]` on `DefaultFromTemplate`.
- A second-level extension may name an abstract extension only if it carries `[PXProtectedAccess]`
  (`GetDependencyDict`, message "Graph extension {0} cannot refer to the following abstract graph extensions that are not
  marked as [PXProtectedAccess]"); `ProjectEntry_CopyTaskExt` has it, so `PXGraphExtension<ProjectEntry_CopyTaskExt, ProjectEntry>`
  is also legal (alternative, not used).
- The `[PXProtectedAccess]` forwarder is emitted with `OpCodes.Callvirt` (`InterfaceWrapper.EmitMethodCall`: `Call` only for
  static methods), i.e. a virtual call on `Base`, which is the generated graph wrapper type whose override of
  `OnCopyPasteTaskInserted` runs the `[PXOverride]` chain (`CreateWrapperInternal` defines `<Name>GeneratedWrapper`
  methods; only the innermost call to the original body is rewritten from `callvirt` to `call`).

## 3. Flow 1 — create a project from a template (`DefaultFromTemplate`, IL of 26.200)
1. `templ = PMProject.PK.Find(templateID)`; if `CopyProperties && !_isLoadFromTemplate` → `DefaultFromTemplateProjectSettings(prj, templ)`.
2. Attributes, `prj.StartDate = Accessinfo.BusinessDate` (IL_00e2), currency default.
3. `srcTasksByTaskCD` (loc1) = template tasks; `srcTasksByTaskID` (loc2).
4. If `CopyTasks`: for every template task with `AutoIncludeInPrj == true`:
   `GetExtension<ProjectEntry_CopyTaskExt>().CopyTask(srcTask, prj, true, true, true)` (5-arg, **not** `ProjectEntry.CopyTask`)
   → … → `OnCopyPasteTaskInsertedEx(prj, newTask, srcTask)` → `ProjectEntry.OnCopyPasteTaskInserted(prj, newTask, srcTask)`.
   `LoadFromTemplate` then presses Save.
5. `destTasks` (loc3) = tasks of `Project.Current`; **`taskMap` (loc4) is built by TaskCD match:**
   for each `destTask` (V18) with a template task of the same TaskCD (V19): `taskMap.Add(V19.TaskID /*template*/, V18.TaskID /*new*/)`
   (IL_02e7–IL_0335). Keys = template TaskIDs, values = new project TaskIDs (negative placeholders for an unsaved project,
   real IDs after `LoadFromTemplate`'s Save). It contains every project task whose TaskCD matches a template task, also
   pre-existing ones and non-auto ones already on the project, not only the rows just copied.
6. Task attributes, revenue/cost budgets, recurring items (by TaskCD/TaskID maps), then `OnDefaultFromTemplateTasksInserted(prj, templ, taskMap)` (IL_0cdb), then account-task mapping etc.

The template task passed as `sourceItem` comes from a `PXSelect<PMTask>` on the template, so its `PMTaskExt` (DB fields)
values are loaded. `DefaultFromTemplateProjectSettings` (step 1) runs **before** tasks are inserted, so a department copied
there is already on the project when the task's `UsrPGDepartment` default (`Selector<PMTask.projectID, ContractExt.usrPGDepartment>`) fires.

## 4. Flow 2 — Add Common Task (`ViewAddCommonTask` → dialog → `AddTasks`)
1. `AddTasks`: for each `TasksForAddition.Cache.Updated` row with `Selected == true`:
   `ProjectEntry.CopyTask(row, ProjectProperties.Current.ContractID.Value, DefaultFromTemplateSettings.Default)` (IL_0053).
2. `ProjectEntry.CopyTask` → `GetExtension<ProjectEntry_CopyTaskExt>()?.CopyTask(...)`.
3. `ProjectEntry_CopyTaskExt.CopyTask` (3-arg) **re-selects the source** with
   `PXSelect<PMTask, Where<projectID, Equal<Current<projectID>>, And<taskCD, Equal<Current<taskCD>>>>>.SelectSingleBound(Base, new[]{task})`
   → a plain `PMTask` with its `UsrPG*` loaded, then `CopyTaskBase.CopyTask` (3-arg) → 5-arg → `OnCopyPasteTaskInsertedEx`
   → `ProjectEntry.OnCopyPasteTaskInserted(project, newTask, sourceTask)`, then attributes / budgets / recurring if the settings say so.
4. `OnDefaultFromTemplateTasksInserted` is **not** called (confirmed: only caller is `DefaultFromTemplate`).

**How the source is identified:** `sourceItem.ProjectID`. Template common task → `== Project.TemplateID` (non-auto
task of the project's template). Global common task (PM208030) → the non-project, `ProjectDefaultAttribute.NonProject()`
= ContractID **0** (live: task `03AL`, TaskID 77267, `NonProject = 1`, `AutoIncludeInPrj = 0`). In the template-creation flow
`sourceItem.AutoIncludeInPrj == true`, in Add Common Task it is false/null. The prototype treats all sources the same.

### 4.3 Other callers of the same hook (guarded in the prototype)
- `ProjectEntry.Copy` (Copy Project): `this.OnCopyPasteTaskInserted(newProject, newTask, srcTask)` where `newTask` was inserted into
  a **new `ProjectEntry` instance**, not into `Base`. (It also shifts `PlannedStartDate/EndDate` relative to the business date — see 6.)
- `ProjectEntry.CreateTemplate`: `newItem` lives in a `TemplateMaint` graph.
- Prototype guard: `ReferenceEquals(Base.Tasks.Cache.Locate(newItem), newItem)`; otherwise it returns after `baseMethod`.
  So Copy Project / Create Template behave exactly as today (no `UsrPG*` copied). Whether they should copy is a T-32 decision (D4).
- `PMQuoteMaint` (quote → project, add tasks to project) runs `DefaultFromTemplate` / `ProjectEntry.CopyTask` in a `ProjectEntry`
  instance, so it gets the same behaviour as flows 1–2 (desired).
- `ChangeOrderEntry.CreateTaskFromTemplate` uses `ChangeOrderEntry_CopyTaskExt` (a different graph): **not covered** by a
  `ProjectEntry` extension. PK does not use change-order task creation today (not checked further).

## 5. Coexistence with TimeClock `PKProjectEntryExt` (same `DefaultFromTemplateProjectSettings`)
- Compile: `Test-Compile.ps1 -IncludeRuntimeCode` compiles the prototype together with `App_RuntimeCode\PKProjectEntryExt.cs` → COMPILE OK.
- Acumatica's binder, run offline on the compiled DLL (`PXExtensionManager.Sort` then `GetExtMethodsGroupByBaseMethod(typeof(ProjectEntry), …)`):
  ```
  Sort() order: PKTimeClock.PKProjectEntryExt  ->  ProjectTask.PKProjectTemplateExt  ->  PX.Objects.PM.ProjectEntry_CopyTaskExt
  BASE  public virtual ProjectEntry.DefaultFromTemplateProjectSettings(PMProject, PMProject)
     <- [PXOverride] PKTimeClock.PKProjectEntryExt.DefaultFromTemplateProjectSettings (params: 3)
     <- [PXOverride] ProjectTask.PKProjectTemplateExt.DefaultFromTemplateProjectSettings (params: 3)
  BASE  protected virtual ProjectEntry.OnCopyPasteTaskInserted(PMProject, PMTask, PMTask)
     <- [PXOverride] ProjectTask.PKProjectTemplateExt.OnCopyPasteTaskInserted (params: 4)
  ```
- Recipe (PowerShell 5.1, read-only): dot-source `Inspect-Acumatica.ps1` (Bin resolver); `LoadFrom` `PX.Data.dll` and
  `PX.Objects.dll`; `[Reflection.Assembly]::Load([IO.File]::ReadAllBytes('<TEMP>\ProjectTask_CompileCheck.dll'))`; build a
  `List[Type]` of the extensions; `$mgr = PX.Data.PXExtensionManager`; `$sorted = $mgr.GetMethod('Sort').Invoke($null, $args1)`;
  `$mgr.GetMethod('GetExtMethodsGroupByBaseMethod', 'NonPublic,Static').Invoke($null, @($projectEntryType, $sorted))`; print each
  group's `Key` (base method) and members (pass the list as `$list.psobject.BaseObject` inside an `[object[]]`).
- Ordering rule (IL): `PXExtensionManager.Sort` sorts extensions by `Type.FullName` (`String.Compare(..., StringComparison.InvariantCulture)`),
  then `PXBuildManager.SortExts` (hook for explicit `SortExtensionsBy<…>` Autofac modules; none involved here), then a dependency
  (topological) visit. `BuildMethod` builds the chain so that each delegate-style override receives the previous one as `baseMethod`:
  the **later** extension in that order is the outer call. Here: `ProjectTask…` override is entered first → its `baseMethod` runs
  TimeClock's → whose `baseMethod` runs the original. Both call `baseMethod` first and write different fields
  (`UsrPKLaborItemID` vs `UsrPG*`), so **the order does not matter**; neither override replaces the other.
- Two different projects (ProjectTask, PKTimeClock) each declaring an extension of `ProjectEntry` is normal; class names differ
  (`ProjectTask.PKProjectTemplateExt` vs `PKTimeClock.PKProjectEntryExt`), so no duplicate-type risk.

## 6. Due-date semantics — USER DECISION (D1), not decided here
Facts (read-only SQL, PK9 CompanyID 2): 11,407 projects, **11,385 created from a template**; 47 templates hold 284 tasks;
template tasks have **no** `UsrPG*` values and templates have **no** `StartDate`; 8,784 project tasks have a due date, only 1,252
projects have `UsrPGYearEnd`. Due dates are **statutory calendar dates**: per task code the month/day is almost constant while the
offset from project start is not (e.g. `011065` 307 tasks: 1 distinct month/day (3/15), 87 distinct "days after start"; `011040`:
4/15; `02TXFRA`: 5/15). Exception: `011120C` (C-corps, fiscal years) has 7 distinct month/days. Some old tasks carry 1/1/1900 placeholders.

| Option | Behaviour | Needs | Pros | Cons |
|---|---|---|---|---|
| **A. Copy as-is** (prototype today) | template date copied unchanged | template due date visible (T-31) | trivial | templates must be re-dated every year (47 templates); stale year copied silently |
| **B. Offset days from project start** | due = project `StartDate` (= business date on creation) + N | new `int` field on PMTask (e.g. `UsrPGDueDays`, shown only on templates/common tasks), table column, T-31 UI | mirrors Acumatica's own Copy Project (`PlannedStartDate` shift, `ProjectEntry.Copy` IL_049c–IL_059c) | contradicted by the data: the same task's deadline does not depend on when the project is opened |
| **C. Roll forward month/day** (recommended) | keep the template date's month/day, take the first occurrence on/after project start (`RollForward` in the prototype; 29 Feb → 28 Feb) | nothing new: template keeps a sample date in `UsrPGDueDate`; T-31 UI label "(month/day used)" | fits the statutory pattern (3/15, 4/15, 5/15); no new column; also works for Add Common Task (start = project start) | wrong year if a project is opened after that year's deadline for the prior year (user fixes manually); does not fit fiscal-year entities |
| **D. Offset from Year End** | due = `UsrPGYearEnd` + M months, day D | 2 new int fields on PMTask, recalculation when Year End is entered later (FieldUpdated on the project), UI | exact for fiscal-year clients (1120C) | Year End is usually blank at creation (1,252 / 11,407), so a second trigger is required; most work |

Recommendation: **C** now (no schema change, matches 3/15–4/15–5/15 practice), with **D** as a later enhancement if fiscal-year
entities matter. Not recommended: A (silent stale dates), B (does not match how deadlines work here). `UsrPGExtendedDueDate` /
`UsrPGIsExtended` are not copied in any option.

## 7. Prototype design (`src/Code/PKProjectTemplateExt.cs`, namespace `ProjectTask`)
- `PKProjectTemplateExt : PXGraphExtension<ProjectEntry>`, `IsActive() => true`.
- `[PXOverride] DefaultFromTemplateProjectSettings(prj, templ, baseMethod)`: `baseMethod` first; copy `UsrPGDepartment`,
  `UsrPGPriority`, `UsrPGComplexity` from `templ.GetExtension<ContractExt>()` when the project's value is null, via
  `Base.Project.Cache.SetValueExt<…>` (same pattern as TimeClock). Status fields / Year End / Finish Date are not copied (inventory b).
- `[PXOverride] OnCopyPasteTaskInserted(target, newItem, sourceItem, baseMethod)`: `baseMethod` first; guard (4.3); copy
  `UsrPGPreparer`, `UsrPGReviewer`, `UsrPGTechReviewer` when null; `UsrPGDueDate` when null through `GetDueDate(...)`
  (option A today; `RollForward` helper for option C is already in the file). Task `UsrPGDepartment` is not copied (it defaults from the project).
- `TrySetValue<TField>`: `cache.SetValueExt` inside `try/catch (PXSetPropertyException)` → `PXTrace.WriteWarning`, so an
  inactive employee on a template cannot block project creation (whether `SetValueExt` throws here at all is unverified).
- Not wired into anything else; **`tools/Build-Package.ps1` packs every `src/Code/*.cs`**, so a package built from this branch
  WOULD contain the prototype (see Risks).

Compile output (worktree, `$env:TEMP` redirected to a private folder so the parallel T-11 run is not overwritten):
```
Left out of App_RuntimeCode: Contract.cs, PMDocGatheringMaint.cs, PMDocumentGathering.cs, PMSampleTesting.cs, PMSampleTestingMaint.cs, ProjectTask.PMTask.cs, ProjectTask_Constants.cs, ProjectTask_Messages.cs, PX_Objects_CR_Contact_extensions.cs
Compiling 47 file(s) against C:\Program Files\Acumatica ERP\PK9\Bin ...
COMPILE OK
```

## 8. Not verified (needs a runtime test on PK9) and manual test steps
Unverified: the overrides actually fire at runtime (binder output and IL strongly indicate yes); `SetValueExt` on the inserted
task/project inside these hooks (selector validation, `UsrPGDepartment` task default picking up the copied project department
for an unsaved project); Modern UI dialog behaviour of Add Common Task; whether `PXSetPropertyException` is thrown or turned into
a field error; chain order at runtime (irrelevant for correctness).

Test on PK9 only after the user decides to publish a bundle with T-31 (UI to enter the values on templates) + T-32 (final code).
Smoke test before T-31 would need template values set by SQL — only with the user's explicit approval.
1. Backup; import the package; publish with the usual projects (ProjectTask, PKTimeClock, …).
2. PM208000: copy a template to `T30TEST`. Header: Department = TAX, Priority = High, Complexity = Level 2, Default labor item set
   (TimeClock). Task A (Automatically Include in Project = on): Preparer, Reviewer, Tech Reviewer, Due Date 04/15/2025.
   Task B (Automatically Include = off → "common" task of the template): Preparer, Due Date 03/15/2025. Save.
3. PM208030: on global common task `03AL` set Preparer + Due Date (needs the T-31 field there). Save.
4. PM301000 → new project, Template = `T30TEST`. Before saving: Department/Priority/Complexity = template values; labor item
   still copied (TimeClock coexistence); Task A has Preparer/Reviewer/Tech Reviewer and the due date per the chosen option
   (A: 04/15/2025; C: next 04/15 on/after today); task Department = TAX. Save, reopen, values persisted.
5. Tasks tab → Add Common Task → tick Task B and `03AL` → OK. Both new tasks have Preparer and due date. Save, reopen.
6. Copy-if-null: new project, set Department = ACCT first, then choose `T30TEST` → Department stays ACCT. Load from Template
   (Actions) on an existing project with Priority set → Priority unchanged, empty fields filled.
7. Copy Project and "Create Template" from a project: no error; `UsrPG*` behave as today (not copied by the prototype).
8. Deactivate the Preparer employee used on Task A; create a project from `T30TEST` → project still created, Preparer blank
   (warning in the trace), or note the error shown.
9. Read-only check: `SELECT TaskCD, UsrPGPreparer, UsrPGReviewer, UsrPGTechReviewer, UsrPGDueDate FROM PMTask WHERE CompanyID=2 AND ProjectID=<new id>`.
Rollback: re-import the previous package (or unpublish the item) — the copy logic writes only to the new rows.

## 9. Risks
- **Prototype ships if `src/Code` is merged as is**: Build-Package includes every `src/Code/*.cs`. Merge only the docs, or keep the
  file and treat it as the T-32 starting point (it is complete and compiled).
- `OnCopyPasteTaskInserted` is also fired by Copy Project / Create Template with foreign rows; the guard handles it — removing
  the guard would write into the wrong cache.
- Templates still have no UI for the fields (T-31); without it nothing is copied (all template `UsrPG*` are blank today).
- `DefaultFromTemplateProjectSettings` is **skipped** inside `DefaultFromTemplate` when `_isLoadFromTemplate` is true, but
  `LoadFromTemplate` calls it itself before the long operation — covered either way.
- Selector validation of employees/departments on copy (handled defensively, unverified).
- Upgrade risk: `OnCopyPasteTaskInserted` is an empty protected hook that Acumatica still routes to from `CopyTaskBase`
  (the `ProjectEntry_CopyTaskExt` forwarder apparently exists to keep graph-level overrides working after the refactor), but re-check signatures with
  `Inspect-Acumatica.ps1 PX.Objects.PM.ProjectEntry -Member 'OnCopyPaste*'` after each Acumatica upgrade.
- Change-order task creation (`ChangeOrderEntry`) is not covered.

## 10. Follow-ups
**T-31 (template screens)**: PM208000 header — `UsrPGDepartment`, `UsrPGPriority`, `UsrPGComplexity` (Q12: confirm department is
wanted); PM208000 Tasks grid and PM208010 task form — `UsrPGPreparer`, `UsrPGReviewer`, `UsrPGTechReviewer`, `UsrPGDueDate`
(label per D1; option B/D add new int fields + table columns); **PM208030 (global common tasks)** — same task fields (needed for
Add Common Task of global tasks). TimeClock already has `PM208000_PKTimeClock`; use a distinct extension name (e.g. `PM208000_PKProjectInfo`).
**T-32 (copy logic)**: start from `PKProjectTemplateExt.cs`; implement D1 in `GetDueDate`; decide D2–D4; optionally drop the
`try/catch` if the PK9 test shows `SetValueExt` surfaces errors on the field instead of throwing; keep `baseMethod` first.
**T-40**: budget lines on Add Common Task can hook `OnCopyPasteCostBudgetInsertedEx/RevenueBudgetInsertedEx` (extension level) or
add lines in the same `OnCopyPasteTaskInserted` override (row and source task available there).

## 11. User decisions needed
- **D1** Due-date semantics: A / B / C (recommended) / D (section 6).
- **D2** Which project fields to carry: Department, Priority, Complexity (prototype); also an initial department status? (Q12)
- **D3** Which task fields: Preparer, Reviewer, Tech Reviewer, Due Date (prototype); also initial `UsrPGTaskStatus`?
- **D4** Should Copy Project / Create Template from a project also carry `UsrPG*` (today: no; would need a different hook, e.g.
  writing to the foreign row directly)?
