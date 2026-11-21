# T-40 prep: fact brief for the common-task design session (2026-11-22)

Written 2026-11-21 by a read-only worker. **Nothing here is a design or a decision.** It collects facts (PK9 data, Acumatica 26.200.0334 code)
and frames the questions. Data = PK9, CompanyID 2, read-only SQL (appendix A). Code facts = reflection (appendix B) plus `docs/template-flow.md`
and `docs/inventory.md` (IL evidence there). Not verified at runtime: everything marked "unverified".

Requirement as the user stated it (2026-11-20): common CPA tasks will be loaded into Common Tasks; they need non-stock items attached and a few
custom fields (e.g. due date); on Add Common Task, create a cost budget line and a revenue budget line from the task and its labor item.

## 1. What a common task is on PK9 today
A common task is an ordinary `PMTask` row. There is no separate table.

| Kind | Where it lives / screen | Flag | PK9 today |
|---|---|---|---|
| Global common task | PM208030 (Common Tasks), under the pseudo project ContractID **0** (`NonProject = 1`) | n/a | **1 task**: `03AL` "TEST", inactive (`IsActive = 0`), `AutoIncludeInPrj = 0` |
| Template task | PM208010 per task, PM208000 Tasks grid; the template is a `Contract` with `BaseType = 'R'` | `AutoIncludeInPrj` | 51 templates (47 have tasks), **284 template tasks, all `AutoIncludeInPrj = 1`** |
| "Template common task" (template task with the flag off) | same screens | `AutoIncludeInPrj = 0` | **none exist** |

- Create project from a template copies only tasks with `AutoIncludeInPrj = 1`.
- The Add Common Task dialog (`TasksForAddition`) lists tasks with `AutoIncludeInPrj <> 1` that either belong to the project's template or are global
  (ContractID 0), and hides TaskCDs already on the project. So **today the dialog can offer only `03AL`**, and `03AL` is inactive.
- Custom task fields exist on `PMTask`: `UsrPGDueDate`, `UsrPGExtendedDueDate`, `UsrPGIsExtended`, `UsrPGPreparer`, `UsrPGReviewer`, `UsrPGTechReviewer`,
  `UsrPGTaskStatus`, `UsrPGDepartment` (plus two orphan columns). **There is no labor-item field on `PMTask`.** The project has `Contract.UsrPKLaborItemID` (TimeClock).
- T-31/T-32 (published on PK9, not yet retested): PM208030 and PM208010 show Due Date, Preparer, Reviewer, Tech Reviewer; Add Common Task copies them (due date as-is, D9).
- Project labor item usage: 11,407 projects, 2,761 have `UsrPKLaborItemID` set (all point to an `L` item), **8,647 have none**. Templates: 37 of 51 have one, 14 do not.

## 2. Items: non-stock / labor
`InventoryItem` by `ItemType` (all `StkItem = 0`):

| ItemType | Meaning | Count | Active | Base UOM |
|---|---|---|---|---|
| **L** | Labor | 268 | 249 | 266 HOUR, 2 PIECE |
| **N** | Non-stock | 35 | 33 | 33 PIECE, 2 HOUR (one is the placeholder `<N/A>`, status XX) |
| E | Expense | 4 | 4 | PIECE/HOUR/MILES (receipts, mileage) |
| stock items | none | 0 | | |

Observed pattern: **the same engagement code exists as an L item and an N item**, with different jobs.
- **L (labor, HOUR)**: one per return/engagement type, e.g. `1040`, `1065`, `1120S`, `401(K)-AUD`, `ACCT-A`, `TXFRA`; classes STATE-TAX 169, TAX 37, ACCT 26, EBPAUDIT 24, AUDIT 6. Includes `NONBILL` (id **10893**, "Non-billable time", L, HOUR, active): it is a labor item, not N.
  Also add-on charges such as `401(K)-BB`, `401(K)-COR`, `403(B)-RK`.
- **N (non-stock, mostly PIECE)**: fee-like items, e.g. `TECH` (annual technology fee), `AUTIRE` (software fee), `PORTS`, `RETAI` (retainer), `00AM`, `1040B`, `STX`; classes DEFAULT 28, TECH 3, AUDIT 2, TAX 1.
  Many have descriptions that read like engagement-letter text ("Preparation of ... for the year ended").
- Which items templates use (`PMBudget` on templates, 426 lines, 54 distinct items):
  - L items: 248 + 17 cost lines (group WIP TIME) and 134 revenue lines.
  - N items: 14 cost lines (groups TECHNOLOGY 8, AUTIRE 6) and 13 revenue lines (e.g. `TECH` 10 times; `<N/A>` 3 times on budget-level-T templates).
  - Task-level items: tasks have no item field; the item lives only on `PMBudget` lines.
- Templates' own labor item (`Contract.UsrPKLaborItemID`) equals the budget line item on 154 of 279 cost lines and 70 of 147 revenue lines; it differs on 17 / 27 lines
  (extra fee items such as `TECH` N, add-on charges, or another return's item, e.g. 706 template uses `1041` as its project labor item), and 108 / 50 lines sit on templates with no project labor item.

## 3. Budget lines on templates and common tasks
Table `PMBudget`; `Type` `E` = cost line, `I` = revenue line. Account groups: **41 REVENUE** (type Income), **42 WIP TIME "Employee Time"** (type Off-balance),
44 TECHNOLOGY, 40 AUTIRE (software fee); 46/47/48 are expense groups with no template usage. All template lines use the single cost code id 238 (the default).

Template lines (426 lines on 51 templates; 4 templates have no lines; 8 of 284 tasks have no line, 273 have a cost line, 111 have a revenue line):

| Type | Account group | Item type | Budget level | Lines | Templates |
|---|---|---|---|---|---|
| E cost | 42 WIP TIME | L | I | 248 | 44 |
| E cost | 42 WIP TIME | L | T | 17 | 3 |
| E cost | 44 TECHNOLOGY | N | I | 8 | 8 |
| E cost | 40 AUTIRE | N | I | 6 | 6 |
| I revenue | 41 REVENUE | L | I | 134 | 43 |
| I revenue | 41 REVENUE | N | I | 10 | 10 |
| I revenue | 41 REVENUE | N | T | 3 | 3 (the item is `<N/A>`) |

Budget level: **44 templates I (per inventory item), 7 templates T (per task)**; projects: 11,377 I, 31 T. Level T lines carry the `<N/A>` item (revenue) and cost lines still carry the labor item (17 lines).

Typical values (templates):
| Line kind | UOM | Qty | Unit rate | Amount |
|---|---|---|---|---|
| Cost, WIP TIME, L item | HOUR | **0 in 184 of 265 cost lines**; 81 have a non-zero qty | **0 on all but 6** cost lines | 0 on all but 5 |
| Revenue, REVENUE, L item | HOUR | 1 (or 0) | typed per template: e.g. `1040` 950, `1120S`/`1041` 1,700, `TXFRA` 550, `LEASE` 375 or 385, add-on charges 500 to 2,000, `401(K)-AUD` 10,000; many are 0 (rate to be filled per client) | rate x qty on 49 of 137 |
| Revenue, REVENUE, N item | PIECE | 1 | `TECH` always 105; N-type fee lines all have rate and amount | set |
| Cost, TECHNOLOGY/AUTIRE, N item | PIECE | 0 or 1 | 0 (AUTIRE 401(K) audit: 2,976.88) | set on 5 |

How a typical one-task template looks (`1040`, project labor item `1040`): task `011040` = cost line (WIP TIME, item `1040`, HOUR, qty 0, rate 0) + revenue line (REVENUE, item `1040`, HOUR, qty 1, rate 950);
task `03TECHNOLOGY` = cost line (TECHNOLOGY, `TECH`, PIECE, qty 0) + revenue line (REVENUE, `TECH`, PIECE, qty 1, rate 105). Multi-task templates (`AFS`, 30 tasks) have **a cost line on nearly every task and a revenue line on only a few** (AFS: 31 lines for 30 tasks).
No duplicate budget keys (project, task, type, account group, item, cost code) exist on templates (all 426 keys are unique).

**The one global common task already has a budget line:** `03AL` carries one revenue line (group 41 REVENUE, item `AL20C` an L item, qty 1, rate 350, amount 350) on ContractID 0.
Real projects (for scale): 56,197 cost lines (31,378 with a qty, 1,931 with a rate) and 30,730 revenue lines (20,996 with a qty, 26,404 with a rate). So in practice **cost lines mostly carry hours, revenue lines carry the price**.

## 4. What Add Common Task copies today
Chain (26.200.0334, signatures in appendix B): `ProjectEntry.AddTasks` -> `ProjectEntry.CopyTask(task, projectID, DefaultFromTemplateSettings.Default)` -> `ProjectEntry_CopyTaskExt.CopyTask`
(re-selects the source `PMTask` by project + TaskCD) -> `CopyTaskBase<ProjectEntry>.CopyTask(3-arg)` -> `CopyTask(5-arg)`.

| Copied by the base code | Notes |
|---|---|
| Standard task fields (TaskCD, description, rate table, billing, approver, default accounts/subs, visibility flags, completion method, ...), attributes, notes, files, UDFs | See `inventory.md` section (c) |
| **Cost and revenue budget lines of the source task** (`CopyBudget`) | With project budget level T, revenue lines are aggregated to the default cost code / `<N/A>` item (IL-derived, unverified at runtime) |
| Recurring billing items | not used by PK data (not checked) |
| Our fields (T-32): Preparer, Reviewer, Tech Reviewer, Due Date | via `OnCopyPasteTaskInserted`, copy-if-empty, as-is date |
| **Not copied:** task status, extended due date / is extended, task department (defaults from the project) | by design |

Hooks (all protected virtual, overridable as `[PXOverride]` from a `PXGraphExtension<ProjectEntry>`; evidence in `template-flow.md` section 2):
- `ProjectEntry.OnCopyPasteTaskInserted(PMProject target, PMTask newItem, PMTask sourceItem)`: used by T-32 today.
- `ProjectEntry.OnCopyPasteCostBudgetInserted(PMProject target, PMCostBudget newItem, PMCostBudget sourceItem)` and `OnCopyPasteRevenueBudgetInserted(... PMRevenueBudget ...)`:
  graph-level hooks that run once per copied budget line (the `...InsertedEx` versions on `CopyTaskBase<ProjectEntry>` are the lower-level ones that forward to them).
  Both are also fired by Copy Project (with foreign-graph rows) so a guard like the T-32 one is needed (unverified for the budget hooks).
- `CopyTaskBase<ProjectEntry>.CostBudgetView` / `RevenueBudgetView` are the views the copy inserts into.
- Not covered: change-order task creation; `OnDefaultFromTemplateTasksInserted` runs only for create-from-template, not Add Common Task.
- Create project from a template: copies the template's tasks that have `AutoIncludeInPrj = 1` through the same `CopyTask` (5-arg), so budget lines on template tasks reach new projects today, and any new budget behavior hooked at task or budget level will also fire there (see question 9).

## 5. Options for the design (none is recommended as binding)

**A. Pre-build the budget lines on the common task; AddTasks copies them (no code).**
Enter cost and revenue lines on the global task (PM208030 Budget tab) or on the template's non-auto task, exactly like the 426 lines on the templates today.
- Pros: zero code, proven path (`03AL` already has such a line), user sees and edits every line, supports any item/qty/rate/group per line, level I and T copy rules are Acumatica's own.
- Cons: the user keys lines by hand for each common task (labor item is repeated in the line, not derived); qty/rate changes need a re-edit of the common task; no automatic rule ("labor item on the task -> two lines"); only PM208030/PM208010 budget tabs, so the user's "attach a non-stock item to the task" is implicit in the lines, not a field.

**B. Task-level labor item field + code generates the two lines on Add Common Task.**
New `PMTask` field (e.g. labor/non-stock item) plus the T-32 style hook adds a cost line and a revenue line to the new task from it.
- Pros: one field per common task, consistent group/UOM/cost code, rules in one place, can fill the line from a rule (qty 0/1, rate source).
- Cons: new column and field on the template screens, code to maintain (budget keys, level I vs T, cost code, duplicates), the hook must tolerate Copy Project and create-from-template, rate/qty sources must be defined; lines on the source task (option A) would be copied as well, so the code must avoid duplicates.

**C. Hybrid: lines are pre-built on the common task (A) and a hook only fills gaps** (for example sets cost-line qty or revenue rate from the labor item, or adds the missing second line when only one exists).
- Pros: manual path still works; code is small. Cons: two sources of truth, harder to explain.

**D. Use the project's labor item (`Contract.UsrPKLaborItemID`) as the item for the generated lines** (instead of a task field).
- Pros: no new task field, matches TimeClock and 154 of 279 existing template cost lines. Cons: 8,647 of 11,407 projects have no labor item; extra fee items (`TECH`, add-ons) cannot be expressed; all added tasks of a project would share one item.

**E. Item defaults from the item itself** (default price / UOM from the non-stock item) for rate and UOM.
- Not yet explored: whether PK keeps sales prices on items or only on budget lines; the data shows rates typed on budget lines (e.g. `TECH` 105 always, `1040` 950, `LEASE` 375/385).

## 6. Question list for the user (refined with the facts)
1. **Items and item types.** Which items go on a common task: only labor items (L, e.g. `1040`), only non-stock fee items (N, e.g. `TECH`), or both? Today the L item is the time-clock/cost item and the N item is the fee item. Should `NONBILL` (L, 10893) be allowed? Inactive items (19 L, 2 N)?
2. **One item per task or several?** Cost line on the labor item and revenue line on a fee item (the `1040` + `TECH` pattern), or both lines on the same item (the `1040` pattern)? How many lines per task?
3. **Where the item is stored.** Directly on the budget lines (option A), on a new task field (B), or from the project's labor item (D)? A common task has no project, so D only works at copy time.
4. **Quantity / rate / amount.** For cost lines templates mostly have qty 0 (hours get posted later) and no rate. For revenue lines qty 1 and a typed price. Keep that? Where does the price come from: typed on the task each time, an item default price, a client rate table? Or leave amounts empty?
5. **Account groups.** Always 42 Employee Time (cost) and 41 Revenue (revenue), or per item (TECHNOLOGY 44, AUTIRE 40 for fee items)? Hard-coded in code, or a setup value?
6. **Blank item behavior.** If the common task has no item: add no lines silently, add lines with a blank item (not possible, the item is part of the key), or block with a warning?
7. **Duplicates.** The dialog already hides TaskCDs already on the project. If the project already has a budget line with the same key (task, item, group, cost code) from a template task, should the new line be added, merged (qty), or skipped? Same common task added twice is not possible today.
8. **Custom fields on common tasks.** Due Date is done (as-is). Which other fields: the new item field, a default status, an extended-due-date rule, fee/price? PM208030 and PM208010 show the T-31 fields already.
9. **Inheritance from templates.** Template tasks (`AutoIncludeInPrj = 1`) already copy their budget lines on project creation. Should the new rule apply to them too (a labor item on a template task creates lines at project creation), or only to Add Common Task? Should template non-auto tasks ("template common tasks") be used, or only global ones (PM208030)? Today there are none of the former.
10. **Budget level.** 44 templates use level I (lines per item) and 7 use level T (per task, `<N/A>` item for revenue). When the project is level T, should a generated revenue line use `<N/A>` and the amount, with the item only on the cost side? (Acumatica aggregates copied revenue lines itself.)
11. **Existing global task `03AL`.** Keep, delete or reuse as a pattern? It is inactive and already has a revenue line (item `AL20C`, 350).
12. **Which projects need this.** All projects, or only tax/accounting? Common tasks are a global list, so which tasks do you want loaded first (a list, or from the existing items and templates)? How many (rough number)?
13. **Time clock.** The time clock reads the project labor item (`UsrPKLaborItemID`), not task lines. Is the task-level item intended to replace or complement it? (TimeClock repo is paused until at least 2026-12-20; any change touching it is a separate task.)

## 7. Risks and things not verified
- `OnCopyPasteCostBudgetInserted` / `...RevenueBudgetInserted` behavior at runtime (signatures confirmed, behavior unverified); fire also from Copy Project.
- Budget level T aggregation behavior on copy is IL/doc-derived, unverified on PK9.
- Modern UI Add Common Task dialog behavior is unverified (T-32 manual test step 5 exists).
- `03AL` budget line is on ContractID 0; whether PM208030 can show its Budget tab for global tasks was not checked in the UI.
- Counts are as of 2026-11-21 on PK9; live may differ (cannot be queried).

## Appendix A. SQL used (read-only, `..\Acumatica-PK-TimeClock\tools\Query-PK9.ps1`, CompanyID = 2)
```sql
-- 1. kinds of Contract rows; the global task; template tasks by flag; budget level on templates
select c.BaseType, c.NonProject, count(*) from Contract c where c.CompanyID=2 group by c.BaseType, c.NonProject;
select c.ContractID, c.ContractCD, t.TaskID, t.TaskCD, t.Description, t.AutoIncludeInPrj, t.IsActive
  from PMTask t join Contract c on c.CompanyID=t.CompanyID and c.ContractID=t.ProjectID where t.CompanyID=2 and c.NonProject=1;
select t.AutoIncludeInPrj, count(*), count(distinct t.ProjectID)
  from PMTask t join Contract c on c.CompanyID=t.CompanyID and c.ContractID=t.ProjectID where t.CompanyID=2 and c.BaseType='R' group by t.AutoIncludeInPrj;
select count(*), sum(case when BudgetLevel='I' then 1 else 0 end), sum(case when BudgetLevel='T' then 1 else 0 end) from Contract where CompanyID=2 and BaseType='R';
select BudgetLevel, count(*) from Contract where CompanyID=2 and BaseType='P' group by BudgetLevel;
-- 2. items
select ItemType, count(*), sum(case when ItemStatus='AC' then 1 else 0 end), sum(case when StkItem=1 then 1 else 0 end) from InventoryItem where CompanyID=2 group by ItemType;
select InventoryID, InventoryCD, Descr, ItemType, ItemStatus, BaseUnit from InventoryItem where CompanyID=2 and (ItemType in ('L','N','E') or InventoryID=10893);
select ic.ItemClassCD, i.ItemType, count(*) from InventoryItem i left join INItemClass ic on ic.CompanyID=i.CompanyID and ic.ItemClassID=i.ItemClassID where i.CompanyID=2 group by ic.ItemClassCD, i.ItemType;
-- 3. template budget lines: by type / account group / item type / level
select b.Type, ag.GroupCD, ag.Type, i.ItemType, c.BudgetLevel, count(*), count(distinct b.ProjectID)
  from PMBudget b join Contract c on c.CompanyID=b.CompanyID and c.ContractID=b.ProjectID
  left join PMAccountGroup ag on ag.CompanyID=b.CompanyID and ag.GroupID=b.AccountGroupID
  left join InventoryItem i on i.CompanyID=b.CompanyID and i.InventoryID=b.InventoryID
  where b.CompanyID=2 and c.BaseType='R' group by b.Type, ag.GroupCD, ag.Type, i.ItemType, c.BudgetLevel;
select b.Type, b.UOM, count(*), sum(case when b.Qty<>0 then 1 else 0 end), sum(case when b.CuryUnitRate<>0 then 1 else 0 end), sum(case when b.CuryAmount<>0 then 1 else 0 end)
  from PMBudget b join Contract c on c.CompanyID=b.CompanyID and c.ContractID=b.ProjectID where b.CompanyID=2 and c.BaseType='R' group by b.Type, b.UOM;
select GroupID, GroupCD, Description, Type, IsActive from PMAccountGroup where CompanyID=2;
select c.BaseType, b.Type, count(*), sum(case when b.Qty<>0 then 1 else 0 end), sum(case when b.CuryUnitRate<>0 then 1 else 0 end)
  from PMBudget b join Contract c on c.CompanyID=b.CompanyID and c.ContractID=b.ProjectID where b.CompanyID=2 group by c.BaseType, b.Type;
-- template lines vs the template's project labor item
select b.Type, sum(case when b.InventoryID=c.UsrPKLaborItemID then 1 else 0 end), sum(case when c.UsrPKLaborItemID is null then 1 else 0 end), sum(case when b.InventoryID<>c.UsrPKLaborItemID then 1 else 0 end), count(*)
  from PMBudget b join Contract c on c.CompanyID=b.CompanyID and c.ContractID=b.ProjectID where b.CompanyID=2 and c.BaseType='R' group by b.Type;
-- the global common task's budget line
select b.ProjectID, b.ProjectTaskID, b.Type, ag.GroupCD, i.InventoryCD, i.ItemType, b.UOM, b.Qty, b.CuryUnitRate, b.CuryAmount from PMBudget b ... where b.CompanyID=2 and b.ProjectID=0;
-- sample lines (1040, ACCT-Q, CFS-Q1, AFS), duplicate-key check (group by the budget key, count = 1 for all 426), tasks with/without lines
-- 4. columns and project labor item
select name from sys.columns where object_id=object_id('PMTask') and name like 'Usr%';
select i.ItemType, count(*) from Contract c join InventoryItem i on i.CompanyID=c.CompanyID and i.InventoryID=c.UsrPKLaborItemID where c.CompanyID=2 and c.BaseType='P' group by i.ItemType;
select count(*), sum(case when UsrPKLaborItemID is null then 1 else 0 end) from Contract where CompanyID=2 and BaseType='P' and NonProject=0;
```
Note: `PMBudget.Type` `E` = cost, `I` = revenue; `PMAccountGroup.Type` `I` income, `E` expense, `O` off-balance.

## Appendix B. Signatures confirmed (`..\Acumatica-PK-TimeClock\tools\Inspect-Acumatica.ps1`, PX.Objects 26.200.0334)
```
[ProjectEntry] public virtual IEnumerable AddTasks(PXAdapter adapter)
[ProjectEntry] public virtual PMTask CopyTask(PMTask task, Int32 ProjectID, DefaultFromTemplateSettings settings)
[ProjectEntry] protected virtual Void OnCopyPasteTaskInserted(PMProject target, PMTask newItem, PMTask sourceItem)
[ProjectEntry] protected virtual Void OnCopyPasteRevenueBudgetInserted(PMProject target, PMRevenueBudget newItem, PMRevenueBudget sourceItem)
[ProjectEntry] protected virtual Void OnCopyPasteCostBudgetInserted(PMProject target, PMCostBudget newItem, PMCostBudget sourceItem)
[CopyTaskBase`1] public virtual PMTask CopyTask(PMTask task, Int32 ProjectID, DefaultFromTemplateSettings settings)
[CopyTaskBase`1] public virtual PMTask CopyTask(PMTask task, PMProject project, Nullable`1 copyNotes, Nullable`1 copyFiles, Nullable`1 copyUdfs)
[CopyTaskBase`1] protected virtual Void OnCopyPasteTaskInsertedEx(PMProject target, PMTask newItem, PMTask sourceItem)
[CopyTaskBase`1] protected virtual Void OnCopyPasteCostBudgetInsertedEx(PMProject target, PMCostBudget newItem, PMCostBudget sourceItem)
[CopyTaskBase`1] protected virtual Void OnCopyPasteRevenueBudgetInsertedEx(PMProject target, PMRevenueBudget newItem, PMRevenueBudget sourceItem)
[CopyTaskBase`1] properties: ProjectView, TasksView, CostBudgetView, RevenueBudgetView, TaskAnswersView, BillingItemsView
```
Commands: `Inspect-Acumatica.ps1 PX.Objects.PM.ProjectEntry -Member 'OnCopyPaste*'`, `... -Member AddTasks`, `... -Member CopyTask`, `Inspect-Acumatica.ps1 'PX.Objects.PM.CopyTaskBase`1' -Member '*'`.
