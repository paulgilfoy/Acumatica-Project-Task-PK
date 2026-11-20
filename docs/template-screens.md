# T-31 — `UsrPG*` fields on the template screens (PM208000, PM208010, PM208030)

Date 2026-11-20. Acumatica 26.200.0334 (PK9). Source only: **nothing published, no DB writes, no C# change.**

**These screens only STORE the values.** Nothing flows into new projects or added common tasks until **T-32** (copy logic,
prototype on branch `agent/T-30`, not merged) is built and published. Publish T-31 and T-32 together (bundle C), or T-31
first if the user wants to start entering template values early (harmless: values just sit on the templates).

## 1. What was added
| Screen (graph) | File | View (DAC) | Fields | Placement |
|---|---|---|---|---|
| PM208000 Project Templates (`TemplateMaint`) | `src/FrontendSources/screens/PM/PM208000/extensions/PM208000_PKProjectInfo.ts/.html` | `ProjectProperties` (`PMProject` : `Contract` → `ContractExt`) | `UsrPGDepartment` (CommitChanges), `UsrPGPriority`, `UsrPGComplexity` | New fieldset **"PK Project Info"** (`#PKProjectInfoFieldset`) at the top of the Summary tab, column A, `before="#SummaryTabSlotA01"` (same as PM301000) |
| PM208000, Tasks tab | same `.ts` (grid columns need no HTML) | `Tasks` (`PMTask` → `PMTaskExt`) | `UsrPGDueDate`, `UsrPGPreparer`, `UsrPGReviewer`, `UsrPGTechReviewer` | Columns after *Description* (`@placeAfterProperty("Description")`) |
| PM208010 Project Template Tasks (`TemplateTaskMaint`) | `src/FrontendSources/screens/PM/PM208010/extensions/PM208010_PKTaskInfo.ts/.html` | `TaskProperties` (`PMTask`) | the same 4 task fields | Fieldset **"PK Task Info"** (`#PKTaskInfoFieldset`) at the top of the Summary tab, `before="#SummaryTab01"` (same as PM302000) |
| PM208030 Common Tasks (`TemplateGlobalTaskMaint`, tasks on non-project ContractID 0) | `src/FrontendSources/screens/PM/PM208030/extensions/PM208030_PKTaskInfo.ts/.html` | `TaskProperties` (`PMTask`) | the same 4 task fields | same as PM208010 |

Deliberately **not** added (user decision 2026-11-20): `UsrPGTaskStatus`, `UsrPGExtendedDueDate` / `UsrPGIsExtended`, the
department status fields, task `UsrPGDepartment` (derived from the project), `UsrPGTechStatus` (removed by T-11).
`UsrPGDueDate` keeps its DAC label "Due Date"; if the user picks due-date option C (template-flow.md §6) a label hint such as
"(month/day used)" could be added later (T-32), via `PXUIField` on a graph-level `CacheAttached` or an HTML caption.

`tools/Build-Package.ps1` needed **no change**: it packs every file under `src/FrontendSources` as
`<PerTenantFile AppRelativePath="screens\…" ScreenId="<folder>"/>`. Package now 55 items (PerTenantFile 21 = 15 + 6).

## 2. Evidence
**View names and DACs** (reflection on `PK9\Bin\PX.Objects.dll`, public fields of the graphs; base Modern UI
`PK9\FrontendSources\screen\src\screens\PM\PM2080x0\*.ts` declare the same names):
```
TemplateMaint            (PM208000.ts graphType, primaryView "Project")
  Project           : View<Where<PMProject, …, baseType = projectTemplate>, PMProject>
  ProjectProperties : PXSelect<PMProject, Where<contractID, Equal<Current<contractID>>>>
  Tasks             : PXSelect<PMTask, Where<projectID, Equal<Current<contractID>>>>
TemplateTaskMaint        (PM208010.ts, primaryView "Task")
  Task              : PXSelectJoin<PMTask, LeftJoin<PMProject, …>, Where<nonProject = False, And<baseType = projectTemplate>>>
  TaskProperties    : PXSelect<PMTask, Where<projectID = Current<projectID>, And<taskID = Current<taskID>>>>
TemplateGlobalTaskMaint  (PM208030.ts, primaryView "Task")
  Task              : PXSelectJoin<PMTask, LeftJoin<PMProject, …>, Where<nonProject = True>>
  TaskProperties    : PXSelect<PMTask, Where<projectID = Current<projectID>, And<taskID = Current<taskID>>>>
PMProject base type: PX.Objects.CT.Contract (no PXBreakInheritance)  → ContractExt (PXCacheExtension<Contract>) applies
PMTask: PMTaskExt is PXCacheExtension<PMTask>, no IsActive()          → applies on every PMTask view
```
The live screens use the **identical** view definitions: `ProjectEntry.ProjectProperties` =
`PXSelect<PMProject, Where<contractID, Equal<Current<contractID>>>>` (PM301000, shows `ContractExt` fields today) and
`ProjectTaskEntry.TaskProperties` = `PXSelect<PMTask, Where<projectID…, And<taskID…>>>` (PM302000, shows `PMTaskExt` today).
Base TS view classes used by the extensions: `ProjectProperties`, `Tasks` (`PM208000/views.ts`), `TaskProperties`
(`PM208010/views.ts`, `PM208030/views.ts`); anchors `TagTemplateID`, `Description`, `VisibleInEA` exist in those classes.
HTML anchors `#SummaryTabSlotA01` (PM208000) and `#SummaryTab01` (PM208010, PM208030) occur exactly once per screen
(base `.html` + base `extensions\*.html`); the new ids `PKProjectInfoFieldset` / `PKTaskInfoFieldset` occur nowhere there.

**Two extensions on one screen/view are supported** (ours + TimeClock's `PM208000_PKTimeClock`):
- Base Acumatica ships several extensions on PM208000 already (`extensions\PM208000_ChangeID.*`, `PM208000_MailingPrinting.*`),
  and the compiled screen `PK9\App_Data\TSCompleteScreens\customizationScreens\Company\PM208000.html` merges them with
  TimeClock's (`<!--Extension: PM208000_ChangeID-->`, `UsrPKLaborItemID` after `ProjectGroupID`).
- **Live precedent, two customization projects on the same view:** `…\Company\PM301000.html` lines 36–54 contain ProjectTask's
  `#PKProjectInfoFieldset` (inserted before `#SummaryTabSlotA01`) **and** PKTimeClock's `UsrPKLaborItemID` (after
  `#SummaryTabSlotA01 [name='ProjectGroupID']`), both extending `ProjectProperties`. PM208000 now gets exactly the same pair
  with the same selectors, so the merge is the same.
- Uniqueness rules: file/extension name = class name postfix; ours `PM208000_PKProjectInfo` (classes
  `PM208000_ProjectProperties_PKProjectInfo`, `PM208000_Tasks_PKProjectInfo`), TimeClock's `PM208000_PKTimeClock`
  (`PM208000_ProjectProperties_PKTimeClock`): no clash. Our HTML adds a fieldset before `#SummaryTabSlotA01`; TimeClock adds a
  field inside it after `ProjectGroupID`: different targets, each selector still matches exactly one element.
- `CustObject` (read-only, all companies, all projects; names like `%PM2080%`, `%PKProjectInfo%`, `%PKTaskInfo%`): the only
  template-screen items anywhere are `PKTimeClock`'s `PerTenantFile#screens\PM\PM208000\extensions\PM208000_PKTimeClock.html/.ts`
  (CompanyID 2). No PM208010 / PM208030 item exists in any project. No duplicate of the 6 new item names.

**DB columns:** no new columns. Templates are `Contract` rows (`BaseType='R'`, 51) and template / global common tasks are `PMTask`
rows (284 template tasks, 1 global task on ContractID 0), the same tables as projects. `sys.columns`: `Contract.UsrPGDepartment`
nvarchar(10), `UsrPGPriority` int, `UsrPGComplexity` int; `PMTask.UsrPGDueDate` datetime, `UsrPGPreparer` / `UsrPGReviewer` /
`UsrPGTechReviewer` int, all already declared in `src/Tables/Contract.xml` / `PMTask.xml`. Today 0 templates and 0 template/global
tasks have any of these values.

## 3. Checks (2026-11-20)
```
tools\Test-ModernUI.ps1                 before: Type-checking 9 file(s) … TS CHECK OK
                                        after:  Type-checking 12 file(s) with Version 5.5.2 ... TS CHECK OK
tools\Test-Compile.ps1 -IncludeRuntimeCode -ExcludeRuntimeFile PX_Objects_CR_Contact_extensions
                                        Compiling 49 file(s) … COMPILE OK   (no C# touched by T-31)
tools\Build-Package.ps1                 55 items: Code 12, Sql 3, Table 3, SiteMapNode 3, Automation 2, Page 5, File 6, PerTenantFile 21
tools\Verify-Package.ps1 -ExtraProject Contact
                                        Problems: 21 = 5 T-11 DIFFs + 10 T-21 NOT LIVE + 6 T-31 NOT LIVE (the new files); PACKAGE DIFFERS FROM LIVE (expected)
```
The TS check validates imports, base class names and decorators; it does **not** validate the HTML nor that a field name
exists on the server DAC (the server only resolves those at runtime).

## 4. Not verifiable before a publish
- HTML: selectors (`before="#SummaryTabSlotA01"`, `before="#SummaryTab01"`) and tags are only checked by the publish build
  (`npm run build`; errors in `PK9\App_Data\logs\npm-*`). Reviewed by hand against PM301000/PM302000 (same structure:
  `qp-fieldset` inside `<div slot="A">` of a `1-1` `qp-template` in the Summary tab), no self-closed tags.
- That the template graphs do not hide/disable the fields (no graph code or workflow targets them; the template workflow could in
  theory disable `ProjectProperties` fields by status, as it does for base fields).
- Column position in the Tasks grid (users can reorder; personalization may override).
- Employee selectors on PM208030 (global tasks have no project; the selector is `PXEPEmployeeSelector`, not project-dependent, so expected fine).

## 5. Manual test checklist (after the user publishes; T-31 alone or with T-32)
1. PM208000 → open a template (copy one to a test template first, e.g. `T31TEST`). Summary tab, column A: fieldset
   **PK Project Info** with Department, Project Priority, Project Complexity. Set TAX / High / Level 2.
2. Same screen, *Project Properties*: TimeClock's **Default Labor Item** (`UsrPKLaborItemID`) still shows after Project Group.
3. Tasks tab: columns **Due Date, Preparer, Reviewer, Tech Reviewer** after Description. Enter values on one task. Save.
   Reopen the template: all values persisted.
4. Click a task (ViewTask) → PM208010: fieldset **PK Task Info** at the top of the Summary tab shows the same 4 values; change
   one, save, check the PM208000 grid shows the change.
5. PM208030 → global common task `03AL` (or a new test task): **PK Task Info** with the 4 fields; set Preparer + Due Date, save, reopen.
6. Regression: PM301000 and PM302000 still show their PK fields; PM208000 Copy Template / Change ID dialogs still open.
7. Read-only SQL: `SELECT ContractCD, UsrPGDepartment, UsrPGPriority, UsrPGComplexity FROM Contract WHERE CompanyID=2 AND BaseType='R' AND ContractCD='T31TEST'`
   and `SELECT TaskCD, UsrPGPreparer, UsrPGReviewer, UsrPGTechReviewer, UsrPGDueDate FROM PMTask WHERE CompanyID=2 AND ProjectID IN (<template id>, 0)`.
8. Without T-32: creating a project from `T31TEST` does **not** copy these values (expected). With T-32: follow
   `template-flow.md` §8.

Rollback: re-import the previous package (the extension files are UI-only; values entered on templates stay in the DB columns).
