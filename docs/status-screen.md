# T-21: Department Statuses screen (PKPM2001), table PKProjectStatus, seed, CSAttributeDetail sync

Source change only (2026-11-20). **Nothing is published and nothing was written to the database.** Ships in publish
bundle B (`PLAN.md`). Design: `docs/status-design.md` (approved; D1-D8 as recommended). Seed values: `docs/status-lists.md`.
**Not in T-21:** the four project fields (`Usr401kStatus`, `UsrPGTaxStatus`, `UsrPGAcctStatus`, `UsrPGAuditStatus`) still
use their `PXIntList`. T-22 switches them to `PKProjectStatusList("<dept>")`. Task status (`UsrPGTaskStatus`) is out of scope (D5).

## What was built
| Package item | File | What |
|---|---|---|
| `Sql#PKProjectStatus` | `src/Sql/PKProjectStatus.sql` | Creates table `PKProjectStatus` if missing, then inserts the 54 seed rows **only where missing** (never updates or deletes). |
| `Code#PKProjectStatus` (NewDac) | `src/Code/PKProjectStatus.cs` | DAC `PKProjectStatus`, filter DAC `PKProjectStatusFilter`, messages `PKProjectStatusMessages`. |
| `Code#PKProjectStatusMaint` (NewGraph) | `src/Code/PKProjectStatusMaint.cs` | Graph of PKPM2001: rules + the one-way sync into `CSAttributeDetail`. |
| `Code#PKProjectStatusListAttribute` (NewFile) | `src/Code/PKProjectStatusListAttribute.cs` | DB-backed drop-down attribute for T-22 (not used by any field yet). |
| `SiteMapNode#d96a84c2-1aaa-4d5c-bd30-8a377926e04a` | `src/SiteMap/PKPM2001.xml` | Menu: workspace **Projects**, subcategory **Preferences** (next to Account Groups PM201000), title "Department Statuses", pinned (like PKPM3001/3002). |
| `File Pages\PK\PKPM2001.aspx(.cs)` | `src/Pages/PK/` | Classic shell (never rendered; it tells Acumatica the graph): `FormDetail.master`, `PXDataSource TypeName="ProjectTask.PKProjectStatusMaint" PrimaryView="Filter"`. |
| `PerTenantFile screens\PK\PKPM2001\*` | `src/FrontendSources/screens/PK/PKPM2001/` | Modern UI: `PKPM2001.ts`, `views.ts`, `PKPM2001.html`. Department filter + grid Status ID / Description / Sort Order / Active. |
| (tooling) | `tools/Build-Package.ps1` | `$CodeFileType`: `PKProjectStatusMaint = NewGraph`, `PKProjectStatus = NewDac` (the attribute defaults to NewFile). |
| (tooling) | `tools/Test-StatusSeed.ps1` | Seed-diff test (read-only). Prints `SEED TEST PASS` / `SEED TEST FAIL`. |

### Table `PKProjectStatus`
`CompanyID int`, `DepartmentID nvarchar(10)`, `StatusID int` (PK = CompanyID, DepartmentID, StatusID), `Description nvarchar(60) NOT NULL`,
`SortOrder int NOT NULL`, `Active bit NOT NULL default 1`, `CreatedByID/ScreenID/DateTime`, `LastModifiedByID/ScreenID/DateTime`, `tstamp`.

### Seed (54 rows: 401K 20, TAX 15, ACCT 8, AUDIT 11)
- Text = the `PXIntList` text in `Contract.cs` (what PM301000 shows today). D1: ACCT 2 "Initial Review (Acctg)", AUDIT 1 "Ready To Prep",
  401K 5 "Initial RECS and Dis". D2: ACCT 0 "Not Started" is **not** seeded. D3: 401K 17 keeps the double space.
- Companies: `SELECT DISTINCT CompanyID FROM CSAttribute WHERE AttributeID = 'STATUSTAX' AND CompanyID > 0` (PK9: only 2).
- `SortOrder = StatusID`, `Active = 1`, `CreatedByScreenID = LastModifiedByScreenID = 'PKPM2001'`, created/modified by the
  admin user `B5344897-037E-4D58-B5C3-1BDFD0F47BF9` (the standard Acumatica admin ID; exists in companies 1 and 2 on PK9), `GETUTCDATE()`.

### Screen rules (graph `PKProjectStatusMaint`)
| Rule | Where |
|---|---|
| Insert is possible only when a Department is selected | `RowSelected<PKProjectStatusFilter>` |
| New row: Status ID = highest ID of the department (saved + unsaved) + 1; Sort Order = Status ID | `FieldDefaulting` |
| Status ID > 0, unique per department | `FieldVerifying<statusID>` + `RowPersisting` (> 0) |
| Status ID read-only once saved (UI disabled; API/import rejected) | `RowSelected<PKProjectStatus>`, `FieldVerifying<statusID>` |
| Department of a saved row cannot change (API) | `FieldVerifying<departmentID>` |
| Description required; unique per department (trimmed, case-insensitive) | `PXDefault(NullOrBlank)`, `FieldVerifying<description>`, re-checked in `RowPersisting` |
| Saved rows cannot be deleted ("Clear the Active check box instead"); new unsaved rows can | `RowDeleting`, plus a `RowPersisting` guard on Delete |
| Sort Order 0..32767 (it is copied to `CSAttributeDetail.SortOrder`, a smallint) | DAC `PXDBInt(MinValue = 0, MaxValue = 32767)` |

## Sync into CSAttributeDetail (design option C)
On **every Save** of PKPM2001, inside the same transaction as the table save (`PXTransactionScope` around `base.Persist()`):
1. Departments synced = every department with an inserted/updated row **plus the department selected in the filter**
   (so a plain Save of a department pushes it; this is how the first sync after publish happens).
2. Mapping: 401K → `STATUS401K`, TAX → `STATUSTAX`, ACCT → `STATUSACCT`, AUDIT → `STATUSAUDI`. Other departments (ADMIN, TECH, a
   future one) are not synced. A missing attribute is skipped with a trace warning.
3. The department's saved rows are read straight from the DB (inside the transaction) and upserted through a
   `CSAttributeMaint` instance (`Attributes.Current` = the attribute; `AttributeDetails` insert/update; `attrGraph.Persist()`):
   `ValueID = ID as 2 digits ("01")`, `Description`, `SortOrder`, `Disabled = !Active`. Rows that already match are not touched.
   `CSAttributeMaint.Persist` also rebuilds `CSAttribute.List` and invalidates the screen-info cache (IL-verified).
4. **Never deletes** a `CSAttributeDetail` row: ACCT `00` "Not Started" stays as it is.
5. After the transaction the attribute's cached lists are reset (`PKProjectStatusListAttribute.ResetCache()`).
If the sync fails, the whole save rolls back (table and attribute rows stay consistent).
Consequence: an edit made on CS205000 to a STATUS* value is overwritten by the next PKPM2001 save of that department.
CS205000 is no longer the place to edit these four attributes.

What the first save of each department changes in `CSAttributeDetail` (from `docs/status-lists.md`):
- every row of the department: `SortOrder` NULL → the ID (no visible effect: the order was by ValueID, which is the same);
- ACCT 02 "Initial Review" → "Initial Review (Acctg)", AUDIT 01 "Ready to Prep" → "Ready To Prep",
  401K 05 "Initial RECs and DIs" → "Initial RECS and Dis" (D1: GI000142 then shows the same text as PM301000).

## List attribute (for T-22) `PKProjectStatusListAttribute : PXIntListAttribute`
`[PXDBInt] [PKProjectStatusList("401K")] [PXUIField(DisplayName = "401k Status")]`. `CacheAttached` loads the full list (active +
inactive) from a `PXDatabase` slot keyed on `PKProjectStatus` (Box1099NumberAttribute pattern); `FieldSelecting` for a row offers
active statuses + the row's current value (CROpportunityStagesAttribute pattern); `FieldVerifying` rejects an unknown or inactive
value that differs from the current one, **except** when the department has no rows at all (table not seeded), so it never blocks
saving before the seed exists. If the table cannot be read, the list is empty (numbers show) and a trace error is written.

## Publish (user; bundle B)
Importing the zip **replaces** the whole `ProjectTask` project with `src/`, which also contains T-10 (Contact merge) and T-11
(Tech Status removal). So bundle A's prerequisites apply to this zip too: `Contact` unticked when publishing, and GI641645's
`usrPGTechStatus = 4` condition removed first (`docs/contact-merge.md`, `docs/tech-removal.md`).
1. Back up the PK9 database.
2. Run the checks (lead): `tools\Test-Compile.ps1 -IncludeRuntimeCode -ExcludeRuntimeFile PX_Objects_CR_Contact_extensions`,
   `tools\Test-ModernUI.ps1`, `tools\Build-Package.ps1`, `tools\Verify-Package.ps1 -ExtraProject Contact`, `tools\Test-StatusSeed.ps1`.
3. SM204505 Customization Projects: open `ProjectTask`, import `build\ProjectTask.zip` (replace), **Publish** with `Contact` unticked
   (until the T-10 cutover is done).
4. **The seed runs as part of the publish:** the `Sql#PKProjectStatus` item executes during publish (DB scripts are part of
   the publish; the script is idempotent, so re-running it on later publishes changes nothing). No separate step is needed.
   Check it: `tools\Test-StatusSeed.ps1` (section [5] now compares the table with the seed) or read-only
   `SELECT DepartmentID, COUNT(*) FROM PKProjectStatus WHERE CompanyID = 2 GROUP BY DepartmentID` → 401K 20, ACCT 8, AUDIT 11, TAX 15.
5. **Access rights (D7, SM201020, not in the package):** screen *Department Statuses* (PKPM2001), following PKPM3001's pattern:
   `*` = Revoked, then **Delete** for **Administrator**, **PK - Admin**, **PK - Partner** (and any managers you name).
   The PM301000 drop-downs (after T-22) read the table directly, so users without PKPM2001 rights still see the lists.
6. First sync: open PKPM2001, select each of 401K, TAX, ACCT, AUDIT and click **Save** once (see the checklist).
7. Live (hosted): same zip, same steps after PK9 sign-off. Check the seeded company there with the read-only query in step 4
   (the tenant ID on live is not verified; the seed targets every company with STATUSTAX).

## Rollback
- **Code/UI:** re-import the previous package (built from git before T-21) and publish. The menu node, screen and code go away.
- **Table:** stays (customization never drops tables). Harmless while no code reads it; drop it only with explicit approval
  (`DROP TABLE PKProjectStatus`), it holds only status rows.
- **CSAttributeDetail:** the sync only changed `Description`/`SortOrder`/`Disabled` of existing rows and may have inserted rows for
  new IDs. To restore the pre-T-21 state on CS205000: ACCT 02 "Initial Review", AUDIT 01 "Ready to Prep", 401K 05 "Initial RECs and DIs",
  Sort Order blank, Disabled cleared; delete value IDs added after go-live only if no project uses them.

## Manual test checklist (user, on PK9 after publish)
- [ ] Menu: Projects workspace → Preferences → **Department Statuses** opens (Modern UI).
- [ ] Department **401K**: 20 rows, IDs 1-20, text identical to the 401k Status drop-down on PM301000 (17 has the double space).
- [ ] **TAX**: 15 rows. **ACCT**: 8 rows (1-8, no 0; "Initial Review (Acctg)" at 2). **AUDIT**: 11 rows ("Ready To Prep" at 1). Total 54.
- [ ] All rows Active; Sort Order = ID.
- [ ] Before any save: GI000142 still shows the old CSAttributeDetail text for ACCT 02 ("Initial Review"). Select ACCT, click Save:
      GI000142 now shows "Initial Review (Acctg)" for ACCT projects with status 2 (12 projects on 2026-11-20). Repeat Save for 401K, TAX, AUDIT.
      (GI000142 is a per-preparer dashboard; if it lists none of these projects for you, check the value on CS205000 instead.)
- [ ] Rename a status (e.g. TAX 13 "Invoice" → "Invoice Sent"; 2 TAX projects had status 13 on 2026-11-20) and Save: GI000142
      shows the new text for those projects (CS205000 STATUSTAX value 13 shows it too). Rename it back afterwards.
      (PM301000 shows the new text only after T-22.)
- [ ] Add a status: click + on a department → Status ID defaults to max + 1 and Sort Order to the same; enter a description; Save.
      CS205000 shows the new value ID (2 digits) on the department's attribute.
- [ ] Enter a description that already exists in the department (different case, e.g. "complete") → error; empty description → error on Save.
- [ ] Enter an existing Status ID on a new row → error "Status ID n already exists…"; enter 0 → error.
- [ ] Deactivate a status (clear Active), Save: CS205000 shows the value as Disabled. Re-activate it afterwards.
- [ ] Try to delete a saved row → error "A saved status cannot be deleted… Clear the Active check box instead."; the row stays after refresh.
- [ ] Status ID of a saved row cannot be edited (cell disabled).
- [ ] Delete a new, unsaved row → it disappears (allowed).
- [ ] A user without PKPM2001 rights cannot open the screen (after SM201020 is set).
- [ ] ACCT 00 "Not Started" still exists on CS205000 (the sync never deletes).

## Not verifiable without a PK9 publish
- That the `Sql` item actually runs during publish and creates/seeds the table (script syntax parsed with ScriptDom and the seed
  SELECT bind-checked read-only; `SET NOEXEC` compile was not used because it needs `Query-PK9.ps1 -AllowWrite`).
- Runtime behavior of the graph: defaults, validations, delete block, the nested `PXTransactionScope` + `CSAttributeMaint` sync
  (APIs verified by reflection/IL only), `CSAttribute.List` rebuild, slot reset.
- The Modern UI HTML (`Test-ModernUI.ps1` type-checks the `.ts` only) and the site map placement / pinned state.
- Whether Modern UI's Save is enabled with no pending changes (the "select department + Save" first-sync step relies on it;
  if not, toggle any field and toggle it back, or edit a row, then Save).
- The list attribute (only used after T-22): GI/OData labels from the dynamic list (design section 2 "Not verified").
