# T-20 Design: project status maintenance (P3)

Status: **DRAFT for user sign-off** (2026-11-20). Design only; T-21/T-22 implement it.
Inputs: `docs/status-lists.md`, `docs/inventory.md`, `src/Code/Contract.cs`, read-only SQL on PK9 (CompanyID 2), and
reflection/IL on `PK9\Bin` 26.200.0334. Tech Status is out of scope (it is removed in T-11).

## 0. Summary
- One table **`PKProjectStatus`** (key: Department + StatusID int) and one screen **PKPM2001 "Department Statuses"**
  (Department filter + status grid).
- The four project fields keep their int columns and values. Their `PXIntList` is replaced by **one DB-backed list
  attribute `PKProjectStatusList("<dept>")`**. It is still a drop-down, so PM301000, the Automation rules, PKPM3001/3002 and
  the **73 GIs that show these fields** keep showing text. It is not a lookup (`PXSelector`), which would show numbers there.
- GIs: **Option C**. The screen keeps the `CSAttributeDetail` STATUS* rows in step (one-way). GI000142, the 5 other joining
  GIs and TimeClock need no change on day one. The user can repoint the GIs later at their own pace.
- The **IDs are frozen**: 62 GI conditions in 46 GIs test the ints, and 6 active business events are built on those GIs
  (Complete = 401k 20, Acct 7, Audit 11, Tax 14, ...). You can rename a status, retire it, or add a new one with the next ID. You can never renumber an
  ID or give it a new meaning.

## 1. Table `PKProjectStatus`
| Column | SQL type | DAC | Why |
|---|---|---|---|
| CompanyID | int NOT NULL | (implicit) | Standard tenant column, first column of the PK. Acumatica filters on it. The DAC does not declare it. |
| DepartmentID | nvarchar(10) NOT NULL, **PK** | `PXDBString(10, IsUnicode, IsKey, InputMask=">aaaaaaaaaa")`, selector `EPDepartment` | Same as the `EPDepartment.DepartmentID` DAC (10) and `ContractExt.UsrPGDepartment` (10). Values: `401K`, `TAX`, `ACCT`, `AUDIT`. |
| StatusID | int NOT NULL, **PK** | `PXDBInt(IsKey)` | The **same int** that is stored in `Contract.Usr*Status`, so the mapping is 1:1 and no data is migrated. You type it in (it is not an identity column). New rows default to max+1. It is read-only once saved. |
| Description | nvarchar(60) NOT NULL | `PXDBString(60, IsUnicode)`, required | 60 characters fits `CSAttributeDetail.Description` (DAC length 60), which the sync needs. Not localizable (it is user data). |
| SortOrder | int NOT NULL | default = StatusID | Display order. The default keeps today's order. A new status can be placed between existing ones without renumbering. |
| Active | bit NOT NULL default 1 | `PXDBBool` | A retired status is inactive, never deleted. |
| Created*/LastModified*, tstamp | standard | standard | Audit and concurrency, the same as `PMSampleTesting`. |

**Retired status** (`Active = 0`): it still shows its text on projects that hold it, in grids, GIs and exports. It is left
out of the drop-down for other projects. If you save a project with it unchanged, there is no error. You cannot *select*
it again. Saved rows cannot be deleted. Deleting is allowed only for an unsaved new row.

**Seed (T-21 writes it, it runs on publish).** It goes in a Sql item `src/Sql/PKProjectStatus.sql`, which follows the
`PMSampleTesting.sql` pattern. The table is created `IF NOT EXISTS`. Then one fixed `VALUES` list of the 54 `(Dept, ID, Text)`
rows is inserted **only where the row is missing**. The script never updates a row, so a later publish never overwrites
your edits on the screen. Target companies are `SELECT DISTINCT CompanyID FROM CSAttribute WHERE AttributeID =
'STATUSTAX' AND CompanyID > 0`, not a hard-coded 2. Live is hosted and its tenant ID is not verified. This rule also skips
system company 1 and the negative snapshot companies. SortOrder = ID, Active = 1, CreatedByScreenID = `PKPM2001`.

Seed text conflicts (from `status-lists.md`). **USER DECISION D1-D3:**
| # | Row(s) | Options | Recommendation |
|---|---|---|---|
| D1 | ACCT 2 "Initial Review (Acctg)" (PXIntList, 12 projects) vs "Initial Review" (DB). Case only: AUDIT 1 "Ready To Prep"/"Ready to Prep" (0 projects), 401K 5 "Initial RECS and Dis"/"Initial RECs and DIs" (2 projects) | (a) the PXIntList text wins; (b) the DB text wins; (c) cleaned text | **(a)**. PM301000 then shows the same text as today, which is the T-22 acceptance test. With the sync (section 3), GI000142 changes to that text for these 3 rows. Either way one side changes, because they differ today. Fix the wording afterwards on the new screen. That is the point of P3. |
| D2 | ACCT 0 "Not Started" (DB only, 0 projects) | (a) do not seed it; (b) seed it as inactive | **(a)**. 0 has never been a valid value. The sync does not touch the existing `CSAttributeDetail` row. |
| D3 | 401K 17 "Waiting&nbsp;&nbsp;on Management Rep Letter" (double space in both sources) | (a) keep the double space; (b) fix it in the seed | **(a)**. The seed diff test then stays exact. Fix it on the screen after go-live. |

## 2. List attribute (the "selector")
**Recommendation: `PKProjectStatusListAttribute : PXIntListAttribute`, with the department fixed per field.**
```
[PXDBInt] [PKProjectStatusList(Department.n401k)] [PXUIField(DisplayName="401k Status")] Usr401kStatus   // TAX, ACCT, AUDIT likewise
```
- **Which list:** each field belongs to one department (401k Status is always a 401K list), so the department is a
  constructor constant. It is not read from the row's `UsrPGDepartment`. Reason: 5 status values sit on projects of another
  department (401K projects: Tax 1; ACCT: Tax 1; AUDIT: Tax 2, Acct 1; SQL 2026-11-20).
  If the list followed the row's department, those projects would show the wrong text. A future department gets a
  new field with `[PKProjectStatusList("ADMIN")]`, plus statuses on the screen.
- **When the department changes:** nothing changes, the same as today. The other departments' fields keep their values
  and stay hidden. No default status is set on new projects, also the same as today.
- **How it works (verified base patterns):** `PX.Objects.AP.Box1099NumberAttribute : PXIntListAttribute` builds
  `_AllowedValues/_AllowedLabels` in `CacheAttached` from a `PXDatabase.GetSlot<Definition>(name, typeof(AP1099Box))`
  cache, which resets when the table changes (IL verified). `PX.Objects.CR.CROpportunityStagesAttribute`, the per-class
  opportunity stage list, is the closest analog. Its `FieldSelecting` returns only **active stages plus the row's current
  stage**. Its `RowSelected` warns "This stage is not active for the selected opportunity class". Its `Definition` slot reads
  `CROpportunityProbability`/`CROpportunityClassProbability` (IL verified). Our attribute combines the two:
  - The slot holds `Dictionary<Dept, List<(id, text, sort, active)>>` from `PKProjectStatus`.
  - `CacheAttached` sets the **full** list (active and inactive), so every stored value resolves to its text in GIs,
    exports and filters.
  - `FieldSelecting` with a row returns the active rows plus the row's current value. Without a row (column or filter
    state) it returns the full list.
  - `FieldVerifying` rejects an unknown or inactive value that differs from the current one (API or import). Reflection
    shows `PXIntListAttribute` itself has no `FieldVerifying`, so values are not checked today.
- **Description in grids and Modern UI:** it is unchanged. The field stays a `PXIntList`-type drop-down (`qp-drop-down`),
  so `PM301000_PKProjectInfo.ts/.html` need **no change**. The same goes for PKPM3001/3002 `PMProject__Usr401kStatus`
  and the Classic `PXDropDown` customizations. Base Modern UI uses the same per-row list: CR304000 `StageID`
  (`CommitChanges` on `ClassID`/`StageID`, `CR304000.ts:95-97`).
- **Automation `PM301000.xml`:** its hide/disable conditions test only `UsrPGDepartment`, never status values, so it needs no
  change. T-11 removes the Tech row and condition. **T-22 must start after T-11 merges**, because both edit `Contract.cs`
  and the PM301000 html.
- **Alternative (not recommended): `PXSelector`** on `PKProjectStatus.statusID` filtered by the department, with
  `DescriptionField`. It is standard, but the 73 GIs (135 result columns, 45 of them in OData/Power BI GIs) would show **numbers** instead of
  text for these fields, and the html would change from a drop-down to a lookup.
- **Not verified:** that the GI engine and OData output use the dynamic labels exactly as they do for a static `PXIntList`
  today (strongly expected, because the labels come from the same attribute state). T-22 checks this on GI000140 and GI641708.

## 3. GIs and TimeClock that read `CSAttributeDetail`
There are 6 joining GIs (`GIOn`, SQL verified): **GI000142** Preparer Dashboard, **GI000137** Activity by Task,
**GI000143** Reviewer Dashboard (no AUDI join), **GI000144** Task Dashboard, **GI993814** 401k Project Priority (401K only)
and **GI641674** PowerBi - Client Project Hours (**OData**). The STATUS* attributes are used only as lookup tables:
0 `CSAttributeGroup` and 0 `CSAnswers` rows.
| Option | How | + | - |
|---|---|---|---|
| A. Keep in sync | `PKProjectStatusMaint.Persist`: after `base.Persist()`, in the same transaction, upsert `CSAttributeDetail` through a `CSAttributeMaint` instance. Map 401K, TAX, ACCT, AUDIT to STATUS401K, STATUSTAX, STATUSACCT, STATUSAUDI. ValueID = 2-digit ID, Description = Description, SortOrder = SortOrder, Disabled = !Active. It never deletes. | No GI or TimeClock change. Power BI is untouched. | Two copies of the data. An edit made on CS205000 is overwritten by the next save. |
| B. Repoint the GIs | The user replaces each `CSAttributeDetail` alias with `PKProjectStatus`: `DepartmentID = '401K' AND StatusID = [PMProject.Usr401kStatus]` (int to int, so no `CInt`). | One source of truth. | 6 GIs (19 joins) are edited by hand before go-live. Blank text if one is missed. TimeClock must change at the same time, but it is paused. |
| **C. Hybrid (recommended)** | Ship A in T-21. Repoint the GIs (B) whenever the user next edits each one. Drop the sync once no GI or TimeClock reads `CSAttributeDetail` (a later task). | Nothing breaks at publish. No time pressure on the user or on TimeClock. | The sync code lives until the cleanup. |

The seed and the first sync also fix the mismatch the user sees today between PM301000 and GI000142 (D1).
**Existing GI bug found:** GI000144's `ACCT` alias joins `STATUSACCT` on **`[PMTask.UsrPGTaskStatus]`**. Its "Status"
formula uses that alias for ACCT, TECH **and AUDIT** projects. So ACCT and Audit rows show Acct-status text looked up by
the *task* status. Fix it when repointing (join `[PMProject.UsrPGAcctStatus]`, add an AUDIT alias).

**Note for T-23 (TimeClock, read-only here).** `PKPreparerDashboard.LoadDepartmentStatusText()` (l.233) reads
`CSAttributeDetail` through `PKDepartmentStatus.AttributeFor(dept)` (`PKTimeClockConstants.cs:88-107`). With Option C it
needs **no change** and keeps working because of the sync. When TimeClock resumes, read `PKProjectStatus` (keyed by
DepartmentID directly) instead, and delete the `Attribute*` constants and `AttributeFor`. `PKClockTask`/`PKPreparerTask`
read only the ints (`BqlField`), so they don't change. `PKTaskStatusListAttribute` changes only if D5 = (b).

## 4. Screen PKPM2001
- **ScreenID `PKPM2001` is free.** It has no `SiteMap` row in any company, no `CustObject` mentions it, and there is no
  `Pages\PK` file. 20 is Acumatica's maintenance range. Title: **"Department Statuses"**. "Project Status" is already
  GI000140, and DB000006/7/13 already use "... Project Status".
- **Menu (USER DECISION D6):** Projects workspace (`6dbfa68e-...`), subcategory **Preferences** (`8a93637d-...`, next to
  Account Groups PM201000). This is recommended because the screen is setup for all departments. The alternative is
  "401(k) Audit" (`ae2ef594-...`), next to Doc Gathering (10) and Sample Testing (30). `src/SiteMap/PKPM2001.xml` copies
  `PKPM3001.xml` with a new NodeID, Position 21519, `Url=~/Pages/PK/PKPM2001.aspx` and the chosen MUISubcategory row.
- **Access rights (USER DECISION D7):** they are DB data, not in the package. After publish, set them on SM201020,
  following PKPM3001's pattern: `*` = Revoked, then Delete (4) for Administrator, PK - Admin, PK - Partner and the
  managers you name. The drop-downs on PM301000 read the table directly, so users who cannot open PKPM2001 still see
  the lists.
- **Graph** `ProjectTask.PKProjectStatusMaint : PXGraph<PKProjectStatusMaint>`:
  - `PXFilter<PKProjectStatusFilter> Filter` (DepartmentID, `EPDepartment` selector, CommitChanges).
  - `Statuses = SelectFrom<PKProjectStatus>.Where<departmentID = Filter.departmentID>.OrderBy<sortOrder, statusID>`.
  - `PXSave`/`PXCancel` on the filter.
  - Rules: StatusID > 0, unique per department, default max+1, read-only once saved. Description is required and unique per
    department (case-insensitive). Saved rows cannot be deleted. The sync runs in `Persist` (Option C).
  - Base pattern: AP202000 `APVendorPriceMaint` (Filter primary view + editable `Records` grid).
- **Classic shell** `src/Pages/PK/PKPM2001.aspx(.cs)` (never rendered, PKPM3002 pattern): `FormDetail.master`,
  `PXDataSource TypeName="ProjectTask.PKProjectStatusMaint" PrimaryView="Filter"`, a form on `Filter`, a grid on `Statuses`.
- **Modern UI** `src/FrontendSources/screens/PK/PKPM2001/` (`PKPM2001.ts`, `views.ts`, `.html`), following CS205000
  (attribute + values grid) and AP202000:
  - `qp-template 1-1` with a fieldset on `Filter` (DepartmentID).
  - `<qp-grid view.bind="Statuses">` with `GridPreset.Details`, `initNewRow: true`.
  - Columns: StatusID (CommitChanges), Description, SortOrder (right-aligned), Active (checkbox).

## 5. Task status (`PMTask.UsrPGTaskStatus`, 1-9, company-wide). OPEN QUESTION, USER DECISION D5
| Option | What | Cost |
|---|---|---|
| **(a) Keep the PXIntList (recommended now)** | No change. | None. A change to the task list stays a developer change, paired with TimeClock's `PKTaskStatusListAttribute` copy. |
| (b) Same table, reserved pseudo-department `~TASK` | `[PKProjectStatusList("~TASK")]` on PMTaskExt. The filter selector must also offer `~TASK` (it is not an `EPDepartment`). TimeClock uses the shared attribute instead of its copy (T-23). | Done after TimeClock resumes (>= 2026-12-20). GI000144 could then show real task-status text. |
Recommendation: (a) now, and (b) as a follow-up when TimeClock resumes, if you want to maintain task statuses yourself.
The table design already allows it.

## 6. Tasks, tests, risks
**T-21 (screen; files in order):**
1. `src/Sql/PKProjectStatus.sql` (table and seed).
2. `src/Code/PKProjectStatus.cs` (DAC and filter DAC, NewDac).
3. `src/Code/PKProjectStatusMaint.cs` (graph, rules and sync, NewGraph).
4. `src/SiteMap/PKPM2001.xml`.
5. `src/Pages/PK/PKPM2001.aspx(.cs)`.
6. `src/FrontendSources/screens/PK/PKPM2001/*`.
7. Register 2 and 3 in `tools/Build-Package.ps1` `$CodeFileType`. **`tools/` is claimed by T-10 now**, so coordinate with the lead.
8. A new `tools/Test-StatusSeed.ps1` (below).
9. `src/navigation.md` and the scratchpad.

**T-22 (swap):**
1. `src/Code/PKProjectStatusListAttribute.cs` (attribute and slot, NewFile).
2. `ProjectTask_Constants.cs` `Department`: add Tax, Acct and Audit next to `n401k`.
3. `Contract.cs`: swap the 4 `PXIntList` attributes. `[PXDBInt]`, `[PXUIField]` and the columns stay the same.
4. Verify, without editing, that the PM301000 ts/html, Automation, PageCustomizations and PKPM3001/3002 need nothing.
Starts after T-11 is merged.

**Tests:**
1. **Seed diff (mandatory, T-21):** `Test-StatusSeed.ps1` parses the 4 `PXIntList` arrays in `Contract.cs` and the seed
   `VALUES`. Expected: identical ints and text (with the D1-D3 choices applied). After publish on PK9, read-only SQL:
   `PKProjectStatus` = 54 rows, and the only differences from `CSAttributeDetail` are the decided ones (in the sync, all
   0 after the first save).
2. **Display parity on PM301000 (T-22, manual):** one project per used (department, value), using the counts in
   `status-lists.md`, e.g. ACCT 2, 401K 5 and Tax 14. The text must equal the old list. Retire a test status: the project
   holding it still shows it, other projects cannot pick it.
3. **GIs:** GI000142 has no design change. Same rows before and after, and text changes only on the D1 rows. GI000140 and
   GI641708 show text, not numbers. After a rename on PKPM2001, GI000142 shows the new text (the sync works).
4. Checks: COMPILE OK, TS CHECK OK, Build-Package plus Verify-Package (only the intended new items differ).

**Risks:**
1. Someone renumbers or reuses an ID, which breaks business events and GIs. Mitigation: the ID is read-only after save,
   rows cannot be deleted, and the screen tooltip says so.
2. The 401K IDs are tied to task codes in GI expressions (`CInt(Left([PMTask.TaskCD],2))`, "401(K) Automation"), so a
   new 401k status (21+) may need a matching task code. Tell 401k managers.
3. Slot cache: a row changed by raw SQL shows only after a restart. Edit through the screen only.
4. CS205000 edits to STATUS* are overwritten by the sync. CS205000 is no longer the place to edit these.
5. The seed's company selection on hosted live is unverified. Check the tenant there after the bundle B publish.
6. File overlap: T-11 (`Contract.cs`, PM301000 UI, Automation) and T-10 (`tools/`).

## USER DECISIONS: APPROVED 2026-11-20 (user accepted every recommendation below, D1-D8, including D8 deactivate-only)

Plain-language note for D8: a project stores the status **number**; GIs and business events test the number. You can rename, reorder, retire and add statuses freely; the only thing blocked is deleting a saved status or reusing its number for a new meaning. To change what "01" means, rename it. A "delete only if no project uses it" variant was offered and **not** chosen.
| # | Decision | Recommended |
|---|---|---|
| D1 | Text for ACCT 2, AUDIT 1, 401K 5 | PXIntList text (what PM301000 shows) |
| D2 | ACCT 0 "Not Started" | Do not seed it |
| D3 | 401K 17 double space | Keep it in the seed, fix it on the screen later |
| D4 | GI strategy | Option C (sync now, repoint GIs at your pace). Also fix the GI000144 join bug. |
| D5 | Task status in the table? | Not now. Reconsider when TimeClock resumes. |
| D6 | Menu place | Projects, Preferences, "Department Statuses" |
| D7 | Who may edit PKPM2001 | Administrator, PK - Admin, PK - Partner, plus the managers you name |
| D8 | Delete policy | Deactivate only. Saved statuses cannot be deleted. |
