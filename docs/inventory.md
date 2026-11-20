# T-01 Inventory (read-only) - 2026-11-20

Evidence: greps of the three repos and the PK9 site; read-only SQL via `Query-PK9.ps1` (CompanyID 2); reflection with
`Inspect-Acumatica.ps1` / IL call scans on `PK9\Bin\PX.Objects.dll` (26.200.0334). Status lists: see `status-lists.md`.

## (a) UsrPGTechStatus / TECH department

### Code and UI (repos)
| Where | What |
|---|---|
| `Acumatica-Project-Task-PK\src\Code\Contract.cs:88-94` | DAC field `UsrPGTechStatus` (`PXIntList` 1-4 "Tech Status 1..4"). Live copy `PK9\App_RuntimeCode\Contract.cs` has it too. |
| `src\Tables\Contract.xml:9` | column definition (column stays in the DB; never drop) |
| `src\FrontendSources\screens\PM\PM301000\extensions\PM301000_PKProjectInfo.ts:39` and `.html:18` | field state + `<field name="UsrPGTechStatus" if.bind="...UsrPGDepartment.value.id == 'TECH'">`. This `.html:18` is the **only** place in the code that branches on department `TECH`. |
| `src\navigation.md:19` | doc mention |
| `Acumatica-PK-TimeClock\docs\reference.md:37,39,57` | docs only (dept list incl. TECH; status list mention; employee departments TECH 2 / TECHNOLOGY 3) |
| `Acumatica-PK-TimeClock\src\**` | **no** use of `TechStatus` or `'TECH'`. It uses `UsrPGTechReviewer` and `UsrPGDepartment` generically (PKClockTask, PKPreparerDashboard, PKPreparerTask, PKTimeClockConstants, PKTimeClockEntry, PKTimeClockFilter, PKTimeClockMessages, PKEP3001/2 views). |
| `Acumatica-Employee-Project-Time` | none |
| `PLAN.md` | planning mentions only |

### Live customization items NOT in `src/` (found in the DB, project `ProjectTask`, CompanyID 2)
`CustObject` rows whose Content contains `TechStatus` in project `ProjectTask` (`871e8ea0-...`): **`Automation#PM301000`**, **Page `~/pages/pm/pm301000.aspx`** (Classic page customization), `Code#Contract`, `Table Contract`.
Another agent's extraction under `build\live\src` shows the details (I did not edit it):
- `build\live\src\Automation\PM301000.xml:7` - screen automation field state for `UsrPGTechStatus` with Hide/Disable condition `66bc0c2c-56d6-4521-aab0-2dd2508d97a9` (same pattern for Acct/Audit/Tax). Needs removal in T-11 together with the condition if it is only used by this field (conditions not inspected).
- `build\live\src\PageCustomizations\PM301000.xml:56` - Classic `PXDropDown` bound to `UsrPGTechStatus`.
**`src/` does not contain these two items, so T-11 as written ("remove from Contract.cs / Contract.xml / ts / html") would leave dangling references in the published project** unless T-00's resync adds them to `src/` first. (Rows with the same text under negative CompanyIDs are old snapshots - ignore; the `PK_WebServiceEndpoints` endpoint hit is an old snapshot, tenant 2 has no `PriceKubecka` endpoint.)

### Generic Inquiries (DB `GIResult`/`GIWhere`/`GIOn`/`GIFilter`/`GISort`, CompanyID 2)
**Uses of the field `usrPGTechStatus` (7 GIs):**
| GI (ScreenID) | Name | OData | How used |
|---|---|---|---|
| GI000140 | PK - Project Status | yes | result column (line 7) |
| GI641660 | Power BI - Preparer Time | yes | result column (line 12) |
| GI641645 | PowerBI - Project Revenue | yes | result column (line 13) **and a `GIWhere` condition `PMProject.usrPGTechStatus = 4`** (line 8, alongside 401k=17, Audit not null, Tax=13, Dept=ACCT) |
| GI641709 | PowerBI - Project Expenses | yes | result column (line 12) |
| (no ScreenID) `b8f7f6c8-...` | Preparer Time Sapro | no | result column (line 12) |
| (no ScreenID) `50e5406b-...` | PK - Preparer Time Sapro Manager | no | result column (line 12) |
| (no ScreenID) `8e31422e-...` | PK Open Project | no | result expression `Switch(Dept='TAX'->TaxStatus, '401K'->Usr401kStatus, 'ACCT'->AcctStatus, 'TECH'->UsrPGTechStatus, 'AUDIT'->...)` (line 6) |

**GIs that branch on `UsrPGDepartment='TECH'` without using the Tech status** (TECH is mapped to ACCT.Description): GI000137 PK - Activity by Task (line 2), GI000144 PK - Task Dashboard (line 2), GI641674 PowerBi - Client Project Hours (line 19), GI000143 PK - Reviewer Dashboard (line 2). Harmless: they keep working if TECH is no longer shown.
**`usrPGTechReviewer`** (must keep): GI000137, GI000143, GI000144, GI641618, GI641699, GI641735, GI641754, GI641627, GI641621, PM-ProjectTasks and others (GIOn joins `usrPGTechReviewer -> bAccountID` in GI000137/GI000143). Not touched by P2.
**GI000142 (Preparer Dashboard) does not reference TechStatus or the TECH department.** Task filters like `taskCD = '03TECHNOLOGY'` / `TECHNOLOGY` / `16TECHREV` are about task codes and account groups, unrelated.
`GIFilter`, `GISort`: no hits for Tech.

### Reports and site map
- Site map: no screen title/URL mentions Tech; `PM301000`/`PM302000`/`PM208000` are plain Classic URLs (Modern UI is picked by the frontend).
- Reports: `ReportsDefault\*.rpx` has **0** matches for `TechStatus` or `UsrPGDepartment`. `ReportsCustomized\` is empty and `UploadFile` has no `.rpx`. Twelve site-map entries `PKPM*` (engagement letters, 401k proposal, billable-rate reports) point to `ReportLauncher.aspx?ID=PK*.rpx`, **but I could not find those .rpx files anywhere on disk or in `UploadFile`** -> **could not verify** whether any custom report uses `UsrPGTechStatus`. (Ask the user where they are stored; likely low risk, they are engagement letters / rate reports.)

### Live data (CompanyID 2)
- `Contract` rows with non-null `UsrPGTechStatus`: **1** (project `2022ACUMATICA / LGORGANICS.001` "Acumatica Consulting", value 4, status Active).
- Projects with `UsrPGDepartment = 'TECH'`: **2** (that one and `2022ACUMATICA / MCMAHONC.001` "Acumatica - Product demo and sale", no tech status). Both Active. Department mix for projects: TAX 7090, 401K 2022, ACCT 1823, AUDIT 451, ADMIN 14, blank 6, TECH 2. `EPDepartment` TECH = "Technology Department" (keep the department record; only the display is removed).
- Projects 'TECH' are not templates/no template uses it (all 51 templates have blank department).

### Conclusion
Removing `UsrPGTechStatus` and the TECH-only UI branch is **safe for users, TimeClock and the DB** (1 data row, no TimeClock use, `UsrPGTechReviewer` independent, column not dropped). It is **not safe to remove the DAC field blindly** because:
1. **Seven GIs reference the field**, four of them OData-exposed for Power BI; one has a `WHERE usrPGTechStatus = 4`. A missing DAC field makes the GI column/condition error out or return blank and can break Power BI refreshes. Remove those GI columns/conditions first (user-approved edit through the UI or an approved script), or keep the DAC property (hidden) for one release.
2. `Automation#PM301000` and the Classic `pm301000.aspx` customization (live, not in `src/`) reference the field.
3. The project with value 4 will keep its value in the DB (data is not lost) but will no longer display it; confirm with the user that nobody needs that value.
Recommendation for T-11: (a) extend the plan to also clean the Automation item, the Classic page customization (after T-00 puts them in `src/`) and the 7 GIs; (b) order of work: GIs first (or in the same bundle), then the DAC/UI removal.

## (b) Custom fields on Contract/PMProject and PMTask
Source: `src/Code/Contract.cs`, `PMTask.cs`, DB column list (`sys.columns` `Usr%`), Modern UI extensions. Template screens: PM208000 (`TemplateMaint`, view `Project`/`ProjectProperties`, `Tasks` grid) and PM208010 (`TemplateTaskMaint`, per-task form); global common tasks PM208030.
**Currently the ProjectTask project has no extension on PM208000/PM208010/PM208030.** (`PLAN.md` says none exists on PM208000 - true for ProjectTask, but `Acumatica-PK-TimeClock` already extends PM208000 with `PM208000_PKTimeClock` showing `UsrPKLaborItemID`, and `PKProjectEntryExt` copies it from template to project.)

### Contract / PMProject (`ContractExt`, screen PM301000 `PM301000_PKProjectInfo`)
| Field | Type / list | Where shown today | Recommend on template (PM208000) |
|---|---|---|---|
| UsrPGDepartment | string(10), selector `EPDepartment` | PM301000 "PK Project Info" (after TagTemplateID) | **Yes** (drives everything) |
| UsrPGPriority | int, Low/Medium/High | PM301000 | **Yes** |
| UsrPGComplexity | int, Level 1-3 | PM301000 | **Yes** (default) |
| UsrPGIsFinal | bool | PM301000 | No (per engagement) |
| UsrPGFinishDate | date | PM301000 | No |
| UsrPGYearEnd | date | PM301000 | No (per client/year) |
| Usr401kStatus / UsrPGTaxStatus / UsrPGAcctStatus / UsrPGAuditStatus | int lists, shown by department | PM301000 "PK Department Status" | Optional: only a default **initial** status (first value); the status is per project |
| UsrPGReleaseDeliverables | bool (401K) | PM301000 | No |
| UsrPGReleasedELF | bool (TAX) | PM301000 | No |
| UsrPGTechStatus | int 1-4 | PM301000 (TECH only) | **Remove** (P2) |
| UsrPKLaborItemID (TimeClock project) | int | PM301000, **PM208000** (TimeClock) | already on templates |
DB-only orphans (columns without a DAC field in `src/`): `Contract.UsrPGReleasedFinalDeliverables` (bit).
Note DB `Contract.UsrPGDepartment` is nvarchar(20) while the DAC/Table XML says string(10).

### PMTask (`PMTaskExt`; PM302000 `PM302000_PKTaskInfo` form, PM301000 `Tasks` grid via `PM301000_Tasks_PKProjectInfo`)
| Field | Type | Where shown today | Recommend on template tasks |
|---|---|---|---|
| UsrPGTaskStatus | int 1-9 | PM302000, PM301000 Tasks grid (after CompletedPctMethod) | Optional (initial status) |
| UsrPGDueDate | date | same | **Yes, but design needed:** absolute date vs offset from project start/year end (T-30) |
| UsrPGExtendedDueDate, UsrPGIsExtended | date, bool | same (Extended date only when Extended) | No |
| UsrPGPreparer, UsrPGReviewer, UsrPGTechReviewer | int, employee selector | same | **Yes** (default staffing per task) - needs the same on common tasks |
| UsrPGDepartment | string(10), defaults from project's department, read-only | same | No - derived (defaults from the project/template department) |
DB-only orphans on `PMTask`: `UsrPGAccountingStatus` (int), `UsrPGTaxStatus` (int) - no DAC field in `src/` (leftover columns).

Live template data: 51 templates (`Contract.BaseType = 'R'`, **not** `IsTemplate`; `IsTemplate` is False on all). All template tasks have blank `UsrPG*` fields. Because `CopyTask` copies fixed properties only, a project created from a template never gets task `UsrPG*` values today (see c).

## (c) Common Tasks in Acumatica 26R2
Verified by reflection/IL scan on `PX.Objects.dll` 26.200.0334 and by the base source `reference\base-code\PX.Objects.PM.ProjectEntry.cs` (**that source file is older than 26R2** - it has no `ViewAddCommonTask` and predates `CopyTaskBase<T>`; every 26R2 statement below is from reflection/IL, base-source lines are cited only where reflection confirms the same member).

**Storage - there is no separate "common task" table.** Common tasks are ordinary `PMTask` rows (table `PMTask`):
- *Global common tasks* (screen **PM208030 "Common Tasks"**, graph `PX.Objects.PM.TemplateGlobalTaskMaint`, `Task` view = `PMTask` left join `PMProject` where `PMProject.NonProject = True`). They sit on a single pseudo project, the non-project (`Contract.ContractID = 0`, `NonProject = 1`; live: 1 task `03AL` "TEST"). Budgets of that task are in `PMBudget` (view `Budget`). Screen source: `FrontendSources\screen\src\screens\PM\PM208030\PM208030.ts:16`.
- *Template tasks* (screen **PM208010 "Project Template Tasks"**, graph `TemplateTaskMaint`, `Task` view = tasks of projects with `NonProject = False` and `BaseType = projectTemplate`('R'); the template itself is maintained on PM208000, `TemplateMaint`, `Tasks` grid). Source: `PM208010.ts:16`, `PM208000.ts:29`.
- The flag that separates "common" from "automatically added" is `PMTask.AutoIncludeInPrj` ("Automatically Include in Project", `PXDBBool`, default False). **Live: all 51 templates' tasks have `AutoIncludeInPrj = 1`**, so today no template task is a "common task"; the dialog would offer only global tasks (1 test task).

**Add Common Task action (project Tasks tab, PM301000, graph `PX.Objects.PM.ProjectEntry`):**
- Tasks grid toolbar action `ViewAddCommonTask` (`PM301000\views.ts:171`; Classic `PM301000.aspx:270`). IL: `ViewAddCommonTask()` -> `TasksForAddition.AskExt()` (opens panel "Add Tasks", `PM301000.html:383-386`, grid view `TasksForAddition` with Selected/TaskCD/Description/ApproverID/PMProject__NonProject; `views.ts:838`).
- `TasksForAddition` = `PXSelectJoin<SelectedTask, LeftJoin<PMProject,...>, Where<SelectedTask.autoIncludeInPrj != True And (SelectedTask.projectID = Current<PMProject.templateID> Or PMProject.nonProject = True)>>`, i.e. non-auto tasks of the project's template **plus** global common tasks; `tasksForAddition()` removes tasks whose `TaskCD` already exists in the project (base source `ProjectEntry.cs:1034-1060`; the nested class `ProjectEntry.SelectedTask : PMTask` adds `Selected`).
- OK button action `AddTasks` (`PM301000.ts:87`; base source `ProjectEntry.cs:1535-1549`; IL confirms 26R2 calls `ProjectEntry.CopyTask` and `DefaultFromTemplateSettings.get_Default`): for each selected row it calls `CopyTask(task, projectID, DefaultFromTemplateSettings.Default)` (public virtual `PMTask ProjectEntry.CopyTask(PMTask,int,DefaultFromTemplateSettings)`), which in 26R2 delegates to `CopyTaskBase<ProjectEntry>.CopyTask` (a `PXGraphExtension<ProjectEntry>`; 3-arg and 5-arg overloads, both `public virtual`).
- **What is copied** (26R2 IL of `CopyTaskBase.CopyTask`, matches base source `ProjectEntry.cs:5396-5564`): TaskCD, description (+translations), rate table/allocation/billing id/billing option, approver, tax category, default sales/expense/accrual accounts+subs and branch (falling back to the project's), completed-% method, bill separately, WIP account group, visibility flags (GL/AP/AR/SO/PO/TA/EA/IN/CA/CR), IsActive, IsDefault, ProgressBillingBase, Type, `TemplateID = source TaskID`, revenue-% rule, default cost code; task attributes (`CopyAttributes`); **cost and revenue budget lines** of that task from `PMBudget` (`CopyBudget`; with project `BudgetLevel = T` revenue lines are aggregated to `<N/A>`/default cost code), recurring billing items (`CopyRecurring`), notes/files/UDF values (`CopyNotes/CopyFiles/CopyUdfs`).
- **Custom `Usr*` fields are NOT in that list**, so `UsrPGPreparer`, `UsrPGDueDate` etc. are not copied by Add Common Task or by template creation (verified by the absence of any extension-field access in the IL; confirm in the T-30 spike).
- Extension points that exist in 26R2: `ProjectEntry.OnDefaultFromTemplateTasksInserted(project, template, Dictionary<int,int> taskMap)` (protected virtual; called by `DefaultFromTemplate`, base source l.5264/5851; **not called by `AddTasks`**), `CopyTaskBase.OnCopyPasteTaskInsertedEx(PMProject, PMTask newTask, PMTask sourceTask)`, `OnCopyPasteCostBudgetInsertedEx`, `OnCopyPasteRevenueBudgetInsertedEx`, `OnCopyPasteRecurringItemInsertedEx` (protected virtual, called from `CopyTaskBase.CopyTask` which both flows use - IL shows `DefaultFromTemplate` calls `CopyTaskBase.CopyTask`). **Best candidate hook for copying task `UsrPG*` fields in both template creation and Add Common Task: override `CopyTask` (public virtual, `[PXOverride]` in a `PXGraphExtension<ProjectEntry>`) or `OnCopyPasteTaskInsertedEx`; which is overridable from a `PXGraphExtension<ProjectEntry>` (vs needing `PXGraphExtension<CopyTaskBase<ProjectEntry>, ProjectEntry>`) is NOT verified - settle in T-30.** `DefaultFromTemplateProjectSettings(prj, templ)` (public virtual) is the project-field hook already used by TimeClock (`PKProjectEntryExt.cs:19`).

**Budget line from a common task - options (T-40 input).**
1. *No code:* put cost and revenue lines on the common task (PM208030 `Budget` tab / template Cost+Revenue budget). `CopyBudget = true` in `Default` already copies them into the project on Add Common Task. Live templates already do this (279 cost, 147 revenue `PMBudget` lines on templates; e.g. AFS: revenue line account group 41 "Revenue", cost lines group 42 "Employee Time", inventory = labor item 10312, UOM HOUR, qty 0).
2. *Code:* in the hook above, insert a revenue and a cost line through `Base.RevenueBudget` / `Base.CostBudget` (`PMRevenueBudget` type 'I', `PMCostBudget` type 'E'; table `PMBudget`, key = ProjectID, ProjectTaskID, AccountGroupID, InventoryID, CostCodeID; fields Qty, UOM, CuryUnitRate, CuryAmount, RevisedQty, CuryRevisedAmount; `NoteID = null`), setting InventoryID = labor item, account groups (41 revenue / 42 employee time), `CostCodeAttribute.GetDefaultCostCode()` and `<N/A>` item when `BudgetLevel = T`. The project's labor item today is `Contract.UsrPKLaborItemID` (TimeClock); `PMTask` has no labor-item field. Constraints to decide in T-40: duplicates (key collision), blank labor item, qty/rate source (task `UsrPG*` field? item default price?), `BudgetLevel` (live: 44 templates I, 7 templates T), account-group IDs (hard-code vs setup field).
Not verified: how the new-project Tasks-tab flow behaves with `Selected` rows in Modern UI (dialog wiring beyond the view/action names), and the decompiled body of `ViewAddCommonTask`/`AddTasks` beyond IL call lists.

## (d) Status lists
See `status-lists.md`.

## Open questions for later tasks
**T-11 (remove Tech status/dept)**
- Q1. OK to also edit the 7 GIs that use `usrPGTechStatus` (4 are OData/Power BI) before or in the same bundle? Who edits them (UI vs approved script)?
- Q2. `Automation#PM301000` and `pm301000.aspx` customizations are not in `src/`; T-00 resync must bring them in, or T-11 cannot remove the references. Is condition `66bc0c2c-...` used by anything else?
- Q3. What to do with the one project with Tech status 4 and the 2 TECH-department projects? Keep department TECH (EPDepartment record stays)? 
- Q4. Where are the 12 custom `PK*.rpx` reports stored (not found on site)? Do any show Tech status?
**T-20..T-22 (status maintenance)**
- Q5. Which text wins for ACCT 02 ("Initial Review (Acctg)" vs "Initial Review") and the case-only differences (AUDIT 01, 401K 05)?
- Q6. Keep `CSAttributeDetail` in sync (GI000142 joins it via 4 `GIOn` rows; ACCT/AUDIT joins lack `CInt`) or repoint GI000142 to the new table?
- Q7. ACCT 00 "Not Started": carry as inactive or drop? Is task status (1-9) in scope (TimeClock has a copy)?
- Q8. Is `SortOrder` expected to be meaningful (currently NULL everywhere)?
**T-30 (template spike)**
- Q9. Which override copies task `UsrPG*` for both flows: `CopyTask` override vs `OnCopyPasteTaskInsertedEx` vs `OnDefaultFromTemplateTasksInserted` (the last one is skipped by Add Common Task)? Is `taskMap` keyed template TaskID -> new TaskID (base source suggests yes: `taskIds[srcTask.TaskID] = destTask.TaskID`, l.5639)? Confirm in the spike.
- Q10. Due-date semantics on templates: copy as-is, or an offset (days) from project start / year end? Needs a new template field.
- Q11. Project-field copy rule: only when target null, and coexistence with TimeClock's `PKProjectEntryExt` (same method `DefaultFromTemplateProjectSettings`, two overrides chain).
- Q12. Should `UsrPGDepartment` on the project be set from the template when the project is created from a template (all 51 templates have blank department today)?
**T-40 (common tasks design)**
- Q13. Today every template task is `AutoIncludeInPrj = 1`; are real "common" tasks wanted on templates (auto = false) or global (PM208030)? Only 1 global test task exists.
- Q14. Labor item: per task (needs a new PMTask field) or the project's `UsrPKLaborItemID`? Blank labor item behavior?
- Q15. Quantity/rate/amount source and account groups (41 Revenue, 42 Employee Time) for the two budget lines; project `BudgetLevel` T vs I.
- Q16. Duplicates when the task already has lines (budget key collision) and what "Add Common Task" does for tasks already present (existing TaskCD is filtered out of the dialog).
- Q17. Template inheritance: if lines are defined on the common task (no code), is that enough?

## Could not verify
- Contents of the 12 custom `.rpx` reports (files not found).
- Which condition IDs in `Automation#PM301000` are shared with other fields.
- Overridability of `CopyTaskBase.OnCopyPasteTaskInsertedEx` from `PXGraphExtension<ProjectEntry>`; full decompiled bodies of `AddTasks` / `ViewAddCommonTask` (only IL call lists, base source is pre-26R2).
- Power BI / OData consumers outside the DB (only the 4 GIs exposed were identified).
