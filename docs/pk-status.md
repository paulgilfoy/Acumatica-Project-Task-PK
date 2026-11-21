# T-25 PK Status: one read-only status per project

Status: **READY-FOR-PK9** (source only; built 2026-11-21; not published). Approved by the user on 2026-11-21, after a review of
the database cost: no extra queries per save, and the values ride in the existing UPDATE.

## 1. What it is
GIs and reports want **one** status per project. The status is spread over four department fields, so today this needs a
`CSAnswers`/`CSAttributeDetail` join. That join stays as it is. T-25 adds two stored columns on `Contract`
(the `PMProject` table), both read-only, both normal database fields that every GI can use:

| Field (DB column) | Display name | Type | Value |
|---|---|---|---|
| `UsrPKStatusID` | PK Status ID | int | the status **number** of the project's own department |
| `UsrPKStatus` | PK Status | nvarchar(60), the same size as `PKProjectStatus.Description` | that status's **text** from PKPM2001 (also when the status is inactive) |

The project's own department is `UsrPGDepartment`:

| Department | Value is taken from |
|---|---|
| TAX | Tax Status (`UsrPGTaxStatus`) |
| ACCT | Acct Status (`UsrPGAcctStatus`) |
| AUDIT | Audit Status (`UsrPGAuditStatus`) |
| 401K | 401k Status (`Usr401kStatus`) |
| any other (ADMIN, TECH, ...) or none | both empty (NULL) |

A status value of another department on the same project is ignored. On PK9 there are 5 such values (status-design.md §2).
A status number with no PKPM2001 row keeps its number and gets an empty text. PK9 has none today.

Shown read-only on **PM301000 Projects**, in the *PK Project Info* group, right after Department.
`UsrPKStatus` is `SelectorVisible`, so it also appears in project lookups that list default columns.

## 2. How it stays current
| When | What updates it | Cost |
|---|---|---|
| A project is saved in **any** graph: ProjectEntry/PM301000, Copy Project, create from template, import scenarios, mass updates, PKTimeClock graphs, the API | `PKDepartmentStatusAttribute` on `UsrPKStatusID` (`RowPersisting`) | 0 extra queries. The two values ride in the same INSERT/UPDATE. The text comes from the cached PKPM2001 list (`PKProjectStatusListAttribute.GetStatuses`, a `PXDatabase` slot) |
| A status is **renamed** on PKPM2001 | `PKProjectStatusMaint.Persist` → `UpdateProjectStatusText`: one set-based `PXDatabase.Update<Contract>` **per renamed status**, inside the existing save transaction, current company only | 1 UPDATE per renamed status. 0 for a plain save, a sort-order change or Active on/off |
| Publish (first time), or a recalculate | `src/Sql/PKProjectStatusBackfill.sql` | one set-based UPDATE, all companies |

**Why a DAC attribute (RowPersisting), not a graph extension or a PXFormula:**
- **Attribute on the field.** It runs for every graph that persists a `Contract`/`PMProject` row, with no list of graphs to
  maintain and no `[PXOverride]` that could collide with TimeClock's `PKProjectEntryExt` or our `PKProjectTemplateExt`.
  A ProjectEntry extension would miss other graphs (imports through other screens, TimeClock, future code).
- **RowPersisting.** IL check (PX.Data 26.200): `PXCache<T>.PersistUpdated`/`PersistInserted` call `OnRowPersisting` first.
  Only then do they call `OnCommandPreparing` for every field, which builds the statement from the row's current values.
  A value set in RowPersisting is therefore written by the same statement. The PKPM2001 cache stays untouched, and nothing
  runs per keystroke.
- **Why not `PXFormula` + `Switch`.** It can produce the ID, but not the text: looking up the text would need a selector or a
  `Search` (a query) or a custom BQL function. A formula also only recalculates when one of its fields raises
  `FieldUpdated`, so a save never repairs a stale row. RowPersisting recalculates on every save, which also heals drift.
- No `FieldVerifying` and no cross-row logic (PLAN §8 lesson). Only plain `SetValue` on the row being saved.
- One mapping in C#: `src/Code/PKDepartmentStatus.cs`. It has no Acumatica references and is unit-tested. The attribute
  and PKPM2001 both use it. The SQL copy in the backfill and the `[PKProjectStatusList("<dept>")]` attributes in
  `Contract.cs` are checked against it by `tools/Test-PKStatus.ps1`.

**Visible effect:** PK Status changes **when you save**, not while you edit. After Save the form shows the new value.

**Side effect of a rename:** the UPDATE changes the affected project rows, which bumps their `tstamp`. A user who has one of
those projects open with unsaved edits gets Acumatica's usual "another process has updated this record" message and must
reload. Renames are rare, so this is acceptable.

**Not covered (drift):** rows changed outside a graph, i.e. raw SQL or `PXDatabase.Update`. No code in ProjectTask or
TimeClock writes the department or status fields that way. Repair: the recalculate below.

## 3. Backfill / recalculate (`src/Sql/PKProjectStatusBackfill.sql`)
- Sets both columns for every `Contract` row with `CompanyID > 0` (all tenants, not the snapshot companies), using the
  same CASE mapping and a LEFT JOIN to `PKProjectStatus`. Rows that should be empty get NULL.
- It is idempotent. It writes only rows whose stored pair differs (`EXISTS (SELECT stored EXCEPT SELECT computed)`, text
  compared case-sensitively), so a second run writes 0 rows.
- It is safe on its own. If either column or the `PKProjectStatus` table is missing it does nothing, and the UPDATE is
  dynamic SQL, so the batch also compiles without the columns.

**Publish order** (IL, PX.Web.Customization 26.200, `CstWebsiteStorage.UpdateDatabase` + `GetUpdatePriority` +
`CstSqlScript.BatchUpdateDatabase`):
1. The items are sorted by key (`Sql#<name>`, `Contract`, ...) and then grouped by priority:
   - Sql with negative `SqlScriptPriority` = 1
   - Sql with a table schema = 2
   - **Table items = 3**
   - **plain Sql scripts = 4**
   - everything else = 5
2. So the **`Contract` Table item, which creates the two columns, runs before every plain Sql item.**
3. Within the Sql group, scripts run by `SqlScriptPriority` + 5, then in key order (stable sort). None of our items sets a
   priority. `Sql#PKProjectStatus` (table + seed) sorts before `Sql#PKProjectStatusBackfill` because it is a prefix of it,
   so the seed runs first.
4. An error in an item stops the publish ("An error while publishing the database item ...").

**Fast publish:** the normal Publish (SM204505, and the Customization Editor's publish) sets `FastPublish = true`. It skips
any database item whose saved XML hash is already in `CustPublishideScripts`; PK9 has 126 such rows. So the backfill runs
on the **first** publish that contains it, and after that only if its text changes. (The same applies to
`Sql#PKProjectStatus`: it does not run again on every publish, contrary to its header comment. That is harmless, because
it only inserts rows that are missing.)

**To recalculate later, use one of these:**
- (a) Publish with **"Execute All Database Scripts (Including Previously Executed)"** (Publish with options), or
  **Publish with Cleanup**. Both reset fast publish.
- (b) Change a comment line in the script (a new hash) and republish.
- (c) On PK9 only, with explicit user approval, run the script in SSMS.

First check whether it is needed (read-only): `tools\Test-PKStatus.ps1` section [4] reports "rows the backfill would
write". After a publish that number is the drift, and 0 is expected.

### Read-only preview (run 2026-11-21, PK9 CompanyID 2, before the publish; columns read as NULL)
`tools\Test-PKStatus.ps1` [4] builds these SELECTs from the backfill's own FROM/JOIN/WHERE. A stand-alone version:
```sql
SELECT ISNULL(UPPER(LTRIM(RTRIM(c.UsrPGDepartment))), '(none)') AS Dept, v.PKStatusID, s.Description AS PKStatus, COUNT(*) AS Projects
FROM dbo.Contract c
CROSS APPLY (SELECT CASE UPPER(LTRIM(RTRIM(c.UsrPGDepartment)))
                        WHEN 'TAX' THEN c.UsrPGTaxStatus WHEN 'ACCT' THEN c.UsrPGAcctStatus
                        WHEN 'AUDIT' THEN c.UsrPGAuditStatus WHEN '401K' THEN c.Usr401kStatus END AS PKStatusID) v
LEFT JOIN dbo.PKProjectStatus s ON s.CompanyID = c.CompanyID
      AND s.DepartmentID = UPPER(LTRIM(RTRIM(c.UsrPGDepartment))) AND s.StatusID = v.PKStatusID
WHERE c.CompanyID = 2
GROUP BY ISNULL(UPPER(LTRIM(RTRIM(c.UsrPGDepartment))), '(none)'), v.PKStatusID, s.Description
ORDER BY 1, 2
```
| Dept | Rows | Gets a PK Status | Stays NULL | ID without text | Inactive status |
|---|---|---|---|---|---|
| TAX | 7,090 | 6,351 | 739 | 0 | 0 |
| ACCT | 1,823 | 1,100 | 723 | 0 | 0 |
| AUDIT | 451 | 374 | 77 | 0 | 0 |
| 401K | 2,022 | 1,910 | 112 | 0 | 0 |
| ADMIN | 14 | 0 | 14 | – | – |
| TECH | 2 | 0 | 2 | – | – |
| (none): 6 projects + 51 templates | 57 | 0 | 57 | – | – |
| **Total** | **11,459** | **9,735** (= rows the first run writes) | **1,724** | 0 | 0 |

The largest values are TAX 14 "Complete" 6,217, 401K 20 "Complete" 1,848, ACCT 7 "Complete" 971, AUDIT 11 "Complete" 349
and ACCT 5 "Bill Client" 100. Every other (department, status) pair has 33 projects or fewer.
Size: about 11.5k rows × (4 + up to 120 bytes), which is about 1 MB.

## 4. Using it in a GI
In SM208000 add the table `PX.Objects.PM.PMProject` (or use the one already there). The fields **PK Status** (`UsrPKStatus`)
and **PK Status ID** (`UsrPKStatusID`) are listed with the other project fields. No join is needed.
- Results grid: `PMProject.UsrPKStatus` (caption "Status"), instead of the `CSAnswers`/`CSAttributeDetail` formula.
- Group by or a pivot on `PMProject.UsrPKStatus`: counts per status across departments ("Complete" of TAX/ACCT/AUDIT/401K
  are four different texts only if they are named differently on PKPM2001; today all four are "Complete").
- Conditions: a status **number** means something only together with the department (TAX 14 = Complete, ACCT 14 does not
  exist). Filter on `UsrPGDepartment = 'TAX' AND UsrPKStatusID = 14`, or on the text `UsrPKStatus = 'Complete'`.
  Business events and existing GIs that test the four department fields keep working unchanged.

## 5. Files
| File | Change |
|---|---|
| `src/Code/Contract.cs` | + `UsrPKStatusID` (`[PXDBInt] [PKDepartmentStatus(typeof(usrPGDepartment), typeof(usrPKStatus))] [PXUIField(DisplayName = "PK Status ID", Enabled = false)]`), + `UsrPKStatus` (`[PXDBString(60, IsUnicode = true)] [PXUIField(DisplayName = "PK Status", Enabled = false, Visibility = SelectorVisible)]`). Every existing field is unchanged |
| `src/Tables/Contract.xml` | + 2 `<Column>` lines (int; string 60). The `UsrPGTechStatus` declaration is kept |
| `src/Code/PKDepartmentStatus.cs` (new Code item, NewFile) | the department → status field mapping and `Resolve` (pure C#) |
| `src/Code/PKDepartmentStatusAttribute.cs` (new Code item, NewFile) | the RowPersisting attribute |
| `src/Code/PKProjectStatusMaint.cs` | `Persist`: + `RenamedStatuses()` (read before `base.Persist`) and `UpdateProjectStatusText` after the CSAttributeDetail sync, in the same transaction. Validation and the sync are unchanged |
| `src/Sql/PKProjectStatusBackfill.sql` (new Sql item) | the backfill / recalculate |
| `src/FrontendSources/screens/PM/PM301000/extensions/PM301000_PKProjectInfo.ts/.html` | `UsrPKStatus: PXFieldState<PXFieldOptions.Disabled>` (the base pattern of `ProjectProperties.QuoteNbr`); `<field name="UsrPKStatus">` after Department in `#PKProjectInfoFieldset` |
| `tools/Test-PKStatus.ps1` (new) | static checks, 16 real-code cases, SQL parse, read-only preview |

Package: 60 items (Code 16, Sql 4, Table 3, SiteMapNode 3, Automation 2, Page 5, File 6, PerTenantFile 21). That is 57 + 2 Code + 1 Sql.

## 6. Checks (worker, 2026-11-21)
`tools\Test-Compile.ps1 -IncludeRuntimeCode` COMPILE OK · `tools\Test-ModernUI.ps1` TS CHECK OK · `tools\Build-Package.ps1` 60 items ·
`tools\Test-PKStatus.ps1` PK STATUS TEST PASS (16/16) · `tools\Test-StatusSeed.ps1` SEED TEST PASS · `tools\Test-TemplateDates.ps1` DATE TEST PASS.

**Not verifiable before a publish:**
- That the Table item creates both columns.
- That the backfill runs after it and writes 9,735 rows. After the publish, run `Test-PKStatus.ps1`: 0 rows to write.
- The RowPersisting behavior in the real save paths, in particular Copy Project and create from template.
- The PKPM2001 rename UPDATE.
- The Modern UI rendering (Test-ModernUI does not check HTML).
- How the GI field list shows the two fields.

## 7. Manual test on PK9 (after the publish)
Before you start, `tools\Test-PKStatus.ps1` (lead, read-only) should report columns that exist and **0** rows to write.
1. **Look:** open a TAX project with Tax Status "Complete". *PK Project Info* shows **PK Status = Complete**, greyed out
   (not editable), right after Department. Check one ACCT, AUDIT and 401K project the same way.
2. **Change status:** on a TAX test project change Tax Status to "In Preparation" and save. PK Status shows
   "In Preparation". Change it back and save.
3. **Change department:** on a test project with both a Tax and an Acct status, switch Department TAX → ACCT and save.
   PK Status is now the Acct status, or empty if the Acct status is empty. Switch back and save.
4. **Non-status department:** an ADMIN or TECH project shows an empty PK Status. Saving it keeps it empty.
5. **Copy Project:** copy a TAX project. The copy shows the same PK Status after its first save. Delete the copy.
6. **Create from template:** create a project from a template that has a Department. PK Status is empty, because a new
   project has no status yet. Set a status and save: PK Status appears. Delete the test project.
7. **PKPM2001 rename:** rename TAX 13 "Invoice" → "Invoice Sent" and save. Open a TAX project with status 13: PK Status
   = "Invoice Sent" without saving the project. A GI on `UsrPKStatus` shows the new text. Rename it back and save, and the
   projects show "Invoice" again.
8. **Inactive status:** on PKPM2001 clear Active on a status that a test project uses, and save. The project still shows
   that text as its PK Status, also after saving the project. Re-activate it.
9. **Sort order / Active only:** changing only Sort Order or Active on PKPM2001 does not change any project's PK Status.
10. **GI:** in a new or test GI on `PMProject`, add *PK Status* and *PK Status ID*. They show the text and the number, and
    a condition `PK Status = Complete` returns the completed projects of all four departments.
11. **Import (optional):** update a project's status through an import scenario. PK Status follows.

Report back: anything that did not match. For step 7, also say whether a project that was open in another tab showed the
"another process has updated" message.
