# T-22: the four project status fields use the Department Statuses table

Built 2026-11-20 (source only then). **Status 2026-11-21:** published on PK9 in the merged package (same zip as T-21,
`docs/status-screen.md`; PLAN §5 merged the old bundles); the checklist below is still unanswered; live is untouched. Design: `docs/status-design.md` (approved, D1-D8).
Task status (`PMTask.UsrPGTaskStatus`, 1-9) is **out of scope** (D5). It keeps its `PXIntList`.

## What changed
| File (package item) | Change |
|---|---|
| `src/Code/Contract.cs` (`Code#Contract`) | `Usr401kStatus`, `UsrPGTaxStatus`, `UsrPGAcctStatus`, `UsrPGAuditStatus`: the inline `[PXIntList(new int[]{…}, new string[]{…})]` became `[PKProjectStatusList("401K")]`, `("TAX")`, `("ACCT")`, `("AUDIT")`. Added `using ProjectTask;`. `[PXDBInt]`, `[PXUIField(DisplayName=…)]`, property types, BQL classes and DB columns did not change. No other field changed. |
| `src/Code/PKProjectStatusListAttribute.cs` (`Code#PKProjectStatusListAttribute`) | `FieldVerifying` no longer checks the value when the graph is inserting a **copy** of an existing record: `ProjectEntry.IsCopyPaste` (Copy Project) or `PXGraph.IsCopyPasteContext` (clipboard Paste). New `protected virtual bool IsCopyOfExistingRecord(PXGraph)`. See (d) below. |
| `tools/Test-StatusSeed.ps1` | New check `[0]`: each of the 4 fields in `src` carries `PKProjectStatusList("<dept>")` with a department that has seed rows. `[1]` now finds the frozen pre-T-22 `PXIntList`s in `src`, then in the PK9 runtime copy, then in the **git history** of `src/Code/Contract.cs`. Before this change it would have reported a false FAIL once bundle B is published. |

The department strings `401K`, `TAX`, `ACCT` and `AUDIT` are exactly the `DepartmentID` values of the seed (`src/Sql/PKProjectStatus.sql`, 54 rows) and of
`EPDepartment` on PK9 (read-only SQL). The attribute compares them case-insensitively.

**Unchanged on purpose (checked, see below):** `PM301000_PKProjectInfo.html/.ts`, `Automation/PM301000.xml`,
`PageCustomizations/PM301000.xml`, PKPM3001/PKPM3002 (code, `.aspx`, Modern UI), `Tables/Contract.xml`, `ProjectTask_Constants.cs`.

## Findings
**(a) PM301000 Modern UI and Automation work as before.**
- `PKProjectStatusListAttribute` is a `PXIntListAttribute` subclass. Its `FieldSelecting` returns the same `PXIntState` type (it calls
  `base.FieldSelecting` and only swaps the value and label arrays with `PXIntState.CreateInstance`). So the field is still an int
  drop-down. The extension markup is a plain `<field name="Usr401kStatus" if.bind=…>` with no control type. The compiled screen
  `PK9\App_Data\TSCompleteScreens\customizationScreens\Company\PM301000.html` has the same plain `<field>` lines, so the
  control comes from the field state at run time. The `.ts` declares the fields as `PXFieldState` (no options), and the field names did not change.
- `Automation/PM301000.xml`: the 4 rows refer to the fields by `FieldName` + `ObjectName="PX.Objects.PM.PMProject"`. Their hide and
  disable conditions only test `UsrPGDepartment` (`<> '401K'`/`'TAX'`/`'ACCT'`/`'AUDIT'`) and never a status value. They set only
  Visible/Enabled, which the list attribute does not touch.
- Classic `PageCustomizations/PM301000.xml` (`PXDropDown` bound to the 4 fields) works with any `PXIntListAttribute`. No change.
- Behavior that changes, as designed: in a project's drop-down, **retired (inactive) statuses are not offered**, except the value
  the project already holds. GIs, filters and exports still get the full list (active and inactive) from `CacheAttached`.

**(b) Other `src` references still compile and make sense.** The only other references are PKPM3001/PKPM3002:
`PMProject__Usr401kStatus` (a column joined from `PMProject` in `PMDocGatheringMaint`/`PMSampleTestingMaint`, display only, no
code sets it), the `.aspx` `PXDropDown`s and the Modern UI `views.ts`/`.html`. The state comes from the `PMProject` cache with
the joined row, so they show the label as before. If the left join finds no project, there is no value and the active list shows.
`tools\Test-Compile.ps1 -IncludeRuntimeCode` compiles all of `src` with the rest of `App_RuntimeCode`: COMPILE OK.

**(c) Runtime code and TimeClock (not edited).** In `PK9\App_RuntimeCode` the only readers outside ProjectTask are TimeClock's
`PKPreparerTask` (projection fields `[PXDBInt(BqlField = typeof(ContractExt.usrPGTaxStatus))]` etc. These read **ints only**
and do not inherit the list attribute) and `PKPreparerDashboard.LoadDepartmentStatusText()`. The second one reads the status **text** from
`CSAttributeDetail` (STATUSTAX/401K/ACCT/AUDI via `PKDepartmentStatus.AttributeFor`) and does not filter `Disabled`, so retired
statuses still show their text. T-21's PKPM2001 sync keeps `CSAttributeDetail` in step, so TimeClock needs **no change** and it
compiled with the new `Contract.cs`.
Until the first PKPM2001 save per department (below), the dashboard and GI000142 show the old `CSAttributeDetail` text for the 3 D1
rows (ACCT 2, AUDIT 1, 401K 5). PM301000 then shows the seed text. That is the same mismatch as today, and it goes away at the first save.
**Follow-up for T-23 (when TimeClock resumes, optional):** read `PKProjectStatus` (for example `ProjectTask.PKProjectStatusListAttribute.GetStatuses(dept)`,
keyed by DepartmentID) instead of `CSAttributeDetail`, and drop the `PKDepartmentStatus.Attribute*` constants and `AttributeFor`. This
removes the last code reader of the STATUS* attributes, a precondition for retiring the sync (design option C). T-23's original
item (the `PKTaskStatusListAttribute` copy) stays not needed, because task status was not touched (D5).

**(d) Copy Project, Create from Template, Import Scenario with a retired status.** Analysis from IL (PK9 Bin 26.200, Mono.Cecil):
- **Copy Project: it would have broken, so it is fixed.** `ProjectEntry.Copy` creates a new `ProjectEntry`, sets `IsCopyPaste = true` on it
  (IL_006c), does `newProject = Project.Cache.CreateCopy(source)` (this copies the `ContractExt` fields, including the 4 statuses), nulls
  some standard fields (none of ours), and calls `target.Project.Insert(newProject)` (IL_0205).
  `PXCache<T>.Insert(object)` creates a **fresh** row and calls `FillWithValues(fresh, ref data)`. For every non-null source value
  this raises `OnFieldVerifying(field, fresh, …)` **before** setting it, so `e.Row` still holds the default (null for our fields). A
  `PXSetPropertyException` is caught, wrapped in `PXFieldValueProcessingException` and kept in `_PendingExceptions`. Back in
  `Insert`, after `OnRowInserting`, the first pending exception is **thrown** (IL_00eb-IL_010d).
  With T-21's rule ("inactive value that differs from the current value → error") the current value is null, so copying any
  project that holds a retired status would fail with "The status '…' of department … is inactive and cannot be selected.". Today Copy Project copies the statuses.
  **Fix:** `FieldVerifying` returns early when `IsCopyOfExistingRecord(graph)` is true. `IsCopyPaste` is set only by `ProjectEntry.Copy`
  and `CopyProject` (IL scan of all of PX.Objects; its setter is not public). The copy keeps the value, as before. The new project's
  drop-down offers the active statuses plus that value, so the user can move it to an active one. Clipboard Paste
  (`IsCopyPasteContext`) inserts a new record the same way and gets the same exemption.
  No project holds a value outside the seeded IDs today (read-only SQL: 0 rows outside 401K 1-20, TAX 1-15, ACCT 1-8, AUDIT 1-11), so
  "unknown value" cannot occur in a copy now. It is exempted too.
- **Updates keep working.** `PXCache<T>.Update(object)` calls `FillWithValues(cachedRow, copy, data)`, which raises
  `OnFieldVerifying(field, cachedRow, …)` while `cachedRow` still holds the **old** value. So "unchanged retired value → no error"
  and "changed to a retired value → error" work as designed (the form, the API and import updates).
- **Create Template (from a project):** `ProjectEntry.CreateTemplate` inserts a `new PMProject()` into `TemplateMaint` and copies a
  fixed list of standard fields only. The statuses are not copied, so no check runs. Nothing changes.
- **Create from Template:** `DefaultFromTemplateProjectSettings` copies standard fields only (no `CreateCopy`, no `Usr*`). All 51
  templates have blank statuses (SQL: `BaseType='R'`, 0 with a status). Nothing changes. **Note for T-32:** if the template copy
  logic later copies a status from a template with `SetValueExt`/`Update`, a retired status on a template is rejected (the current value
  on the new project is null). Copy only active values, or clear retired ones on the template.
- **Import Scenario / API:**
  - Updating an existing project that holds a retired status with the same value (or not mapping the field) works.
  - Setting a retired or unknown value that differs from the current one is rejected with the attribute's message. This is the
    approved design ("you cannot select it again").
  - Inserting a **new** project with a retired status is rejected too. This is consistent with the design, but it differs from
    today, where any int was accepted. Re-activate the status on PKPM2001 for the import, or import an active status.
  - Import by text label: a label of a status that is not offered for the row cannot be matched to an ID (not verified at run time).

**(e) Generic Inquiries and OData: no change expected.** The field is still a list attribute, so GIs, OData and exports render the
label. `CacheAttached` loads the **full** list (active and inactive), and `FieldSelecting` with a row always includes the row's own value.
GI conditions and business events compare the ints, which did not change. PK9 has:
- **GI000140 "PK - Project Status"** (OData): 4 status columns (lines 3-6). Line 20 is the formula `=[PMProject.Usr401kStatus]`
  "Raw401kStatus" and shows the number by design.
- **GI641708 "Audit - Project Information"** (OData): 4 status columns (lines 5, 8, 9, 10).
After the publish, check that both show the same **text** as before (not numbers), on screen and in OData (Power BI refresh). This is
the design's "Not verified" item. The label code path is the same as for a static `PXIntList`, but it cannot be run before the publish.

## Publish (user; merged package, same zip as T-21)
1. Prerequisites as in `docs/status-screen.md` "Publish" (the zip also carries T-10 and T-11: `Contact` unticked, GI641645's
   `usrPGTechStatus = 4` condition removed first). Back up the PK9 database.
2. Checks (lead): `tools\Test-Compile.ps1 -IncludeRuntimeCode`, `tools\Test-ModernUI.ps1`,
   `tools\Build-Package.ps1`, `tools\Verify-Package.ps1`, `tools\Test-StatusSeed.ps1`. (`-ExcludeRuntimeFile PX_Objects_CR_Contact_extensions`
   and `-ExtraProject Contact` are no longer needed, PLAN §9.)
3. Import `build\ProjectTask.zip` into `ProjectTask` and publish once. **The `Sql#PKProjectStatus` item creates and seeds the table during
   the publish**, before the site restarts on the new code. So when users open PM301000 the lists are there. T-21 and T-22 must
   be in the **same** publish (they are, since it is one zip). Never ship T-22 without T-21's Sql item.
4. Check the seed right after the publish (read-only): `tools\Test-StatusSeed.ps1` section `[5]`, or
   `SELECT DepartmentID, COUNT(*) FROM PKProjectStatus WHERE CompanyID = 2 GROUP BY DepartmentID` → 401K 20, ACCT 8, AUDIT 11, TAX 15.
   On **live** (hosted) do the same check on PKPM2001 (each department shows its rows), because the seed picks companies by STATUSTAX and the live tenant ID is not verified.
5. **First save per department** (from `docs/status-screen.md`): open PKPM2001, select 401K, TAX, ACCT and AUDIT in turn, and click **Save**
   each time. This pushes the table into `CSAttributeDetail`, so GI000142, the GIs that join STATUS* and the TimeClock dashboard show
   the same text as PM301000 (D1 rows). It is not needed for PM301000 itself, which reads the table directly.
6. SM201020 rights for PKPM2001 (D7). Users without these rights still see the drop-downs on PM301000.

### What a user sees if a department has no rows (table empty, missing, or the seed skipped the company)
The attribute loads an **empty list** for that department. If the table cannot be read at all, it also writes a trace error
("Department Statuses: the PKProjectStatus table could not be read: …"). Then:
- PM301000: the drop-down has no entries. A project's stored value is still there, but it has no label. It shows as the plain number or
  looks blank, depending on how Modern UI renders a value missing from the list (not verified). Saving the project keeps the value.
- `FieldVerifying` **accepts any value** for that department (`all.Count == 0`), like the old `PXIntList`. Nothing is blocked.
- GIs, OData and exports show the **number** instead of the text.
- The fix is to run the seed (re-publish; the Sql item is idempotent) or enter the rows on PKPM2001. The list cache resets when the
  table changes through Acumatica, and on every PKPM2001 save. Rows inserted by raw SQL show only after a site restart.
There is no fallback to the old hard-coded texts: they are gone from the code.

## Rollback
- Re-import the previous package (built from the commit before T-22 with `tools\Build-Package.ps1`, or the pre-publish backup
  package) and publish. The 4 fields get their fixed `PXIntList`s back. The project data (the ints) was never touched.
- To roll back only T-22 and keep the PKPM2001 screen, revert `src/Code/Contract.cs` (and optionally the attribute) to the T-21 commit, rebuild, and publish.
- After a rollback, PM301000 shows the hard-coded texts again. Renames made on PKPM2001 show only in GIs that join `CSAttributeDetail`
  (through the sync). Statuses **added** on PKPM2001 (IDs above the old ranges) show as numbers on PM301000 for projects that use them.
  The table and the `CSAttributeDetail` rows stay (see `docs/status-screen.md` "Rollback").

## Manual test checklist (user, on PK9 after the bundle B publish)
- [1] Publish succeeds; `PKProjectStatus` has 401K 20, TAX 15, ACCT 8, AUDIT 11 rows (step 4).
- [1] PM301000, one project per department shows the **same status text as before** in *PK Department Status*. Use the counts from
      `docs/status-lists.md`, at least: an **ACCT** project with status 2 → "Initial Review (Acctg)", a **401K** project with 5 → "Initial RECS and Dis"
      and one with 17 (double space), a **TAX** project with 14 → "Complete", an **AUDIT** project (any value).
- [1] Open each drop-down: the same entries in the same order as before (401K 20, TAX 15, ACCT 8, AUDIT 11).
- [1] Change a status on a test project and Save; reopen it: the new value shows. Change it back.
- [1] Retire a status on PKPM2001 (clear Active, Save), e.g. TAX 13 "Invoice" (2 projects on 2026-11-20):
      a project holding 13 still shows "Invoice" and saves without error (also after editing another field); another TAX project's drop-down does
      not offer "Invoice"; trying to set it by import or API gives the "inactive" error. **Re-activate it afterwards.**
- [1] With TAX 13 still retired: **Copy Project** on a project holding 13 → the copy is created, holds "Invoice", and its drop-down offers it.
      Delete the test copy. Copy Project on any other project still works.
- [1] Rename a status on PKPM2001 (e.g. TAX 13 → "Invoice Sent"), Save: PM301000 shows the new text at once (new screen load), and GI000142 shows it through the sync. Rename it back.
- [1] Add a status (e.g. ACCT 9 "Test"), Save: it appears in the ACCT drop-down. Retire it afterwards (it cannot be deleted; D8).
- [1] **GI000140** "PK - Project Status" and **GI641708** "Audit - Project Information": the 4 status columns show **text**, the same as before
      the publish (GI000140 "Raw401kStatus" stays a number); refresh the Power BI / OData consumers of both.
- [1] GI000142 / TimeClock Preparer Dashboard: department status text shows; after the first save per department, ACCT 2 reads "Initial Review (Acctg)".
- [1] PKPM3001 / PKPM3002: the 401k Status column/field shows text.
- [1] Automation still hides and shows the 4 fields by department (switch a test project's department without saving).

## Not verifiable before a publish
- The runtime rendering of the dynamic list in Modern UI (per-row list without retired values; how a value with no label shows).
- GI and OData labels from the dynamic list (expected unchanged: same attribute state path as a static `PXIntList`).
- Copy Project with the exemption, and the import and API behavior. These come from IL reading of `PXCache<T>.Insert/Update/FillWithValues` and `ProjectEntry.Copy`
  in 26.200, not from a run.
- That the Sql item runs before users reach PM301000. This is standard Acumatica publish order, not observed on PK9.
