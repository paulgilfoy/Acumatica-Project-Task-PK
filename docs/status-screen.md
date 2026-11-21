# T-21: Department Statuses screen (PKPM2001), table PKProjectStatus, seed, CSAttributeDetail sync

Built 2026-11-20 (source only then). **Status 2026-11-21:** published on PK9 at ~02:51 (the merged 57-item package) and republished
with the T-21b/T-32b fixes; first PK9 retest results are at the end of this file. Live is untouched. (The "bundle B" wording below is
historical: PLAN §5 merged bundles A+B+C into one zip.) Design: `docs/status-design.md` (approved; D1-D8 as recommended). Seed values: `docs/status-lists.md`.
**Not in T-21:** the four project fields (`Usr401kStatus`, `UsrPGTaxStatus`, `UsrPGAcctStatus`, `UsrPGAuditStatus`) were switched
from their `PXIntList` to `PKProjectStatusList("<dept>")` by T-22 (`docs/status-switch.md`). Task status (`UsrPGTaskStatus`) is out of scope (D5).

## What was built
| Package item | File | What |
|---|---|---|
| `Sql#PKProjectStatus` | `src/Sql/PKProjectStatus.sql` | Creates table `PKProjectStatus` if missing, then inserts the 54 seed rows **only where missing** (never updates or deletes). |
| `Code#PKProjectStatus` (NewDac) | `src/Code/PKProjectStatus.cs` | DAC `PKProjectStatus`, filter DAC `PKProjectStatusFilter`, messages `PKProjectStatusMessages`. |
| `Code#PKProjectStatusMaint` (NewGraph) | `src/Code/PKProjectStatusMaint.cs` | Graph of PKPM2001: rules + the one-way sync into `CSAttributeDetail`. |
| `Code#PKProjectStatusListAttribute` (NewFile) | `src/Code/PKProjectStatusListAttribute.cs` | DB-backed drop-down attribute (used by the four project status fields since T-22). |
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
| Status ID > 0 | `FieldVerifying<statusID>` (rejects IDs <= 0 only) + `RowPersisting` |
| Status ID unique per department (T-21b: no cross-row check in `FieldVerifying`) | `RowPersisting` (duplicate ID on a new row) |
| Status ID read-only once saved (UI cell disabled; since T-21b the API/import is no longer rejected by `FieldVerifying`, which only checks > 0) | `RowSelected<PKProjectStatus>` |
| Department of a saved row cannot change (API) | `FieldVerifying<departmentID>` |
| Description required; unique per department (trimmed, case-insensitive) | `PXDefault(NullOrBlank)`; duplicate flagged inline in `RowInserted`/`RowUpdated`, enforced in `RowPersisting` (T-21b) |
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

## List attribute (used by the four fields since T-22) `PKProjectStatusListAttribute : PXIntListAttribute`
`[PXDBInt] [PKProjectStatusList("401K")] [PXUIField(DisplayName = "401k Status")]`. `CacheAttached` loads the full list (active +
inactive) from a `PXDatabase` slot keyed on `PKProjectStatus` (Box1099NumberAttribute pattern); `FieldSelecting` for a row offers
active statuses + the row's current value (CROpportunityStagesAttribute pattern); `FieldVerifying` rejects an unknown or inactive
value that differs from the current one, **except** when the department has no rows at all (table not seeded), so it never blocks
saving before the seed exists. If the table cannot be read, the list is empty (numbers show) and a trace error is written.

## Publish (user; the merged package, see PLAN §5)
Importing the zip **replaces** the whole `ProjectTask` project with `src/`, which also contains T-10 (Contact merge) and T-11
(Tech Status removal). So their prerequisites apply to this zip too: `Contact` unticked when publishing, and GI641645's
`usrPGTechStatus = 4` condition removed first (`docs/contact-merge.md`, `docs/tech-removal.md`; done on PK9, still to do on live).
1. Back up the PK9 database.
2. Run the checks (lead): `tools\Test-Compile.ps1 -IncludeRuntimeCode`, `tools\Test-ModernUI.ps1`, `tools\Build-Package.ps1`,
   `tools\Verify-Package.ps1`, `tools\Test-StatusSeed.ps1`. (The old `-ExcludeRuntimeFile PX_Objects_CR_Contact_extensions` and
   `-ExtraProject Contact` options are no longer needed: the Contact project is merged and its runtime file is gone from PK9, PLAN §9.)
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
- [1] Menu: Projects workspace → Preferences → **Department Statuses** opens (Modern UI).
- [1] Department **401K**: 20 rows, IDs 1-20, text identical to the 401k Status drop-down on PM301000 (17 has the double space).
- [1] **TAX**: 15 rows. **ACCT**: 8 rows (1-8, no 0; "Initial Review (Acctg)" at 2). **AUDIT**: 11 rows ("Ready To Prep" at 1). Total 54.
- [1] All rows Active; Sort Order = ID.
- [1] Before any save: GI000142 still shows the old CSAttributeDetail text for ACCT 02 ("Initial Review"). Select ACCT, click Save:
      GI000142 now shows "Initial Review (Acctg)" for ACCT projects with status 2 (12 projects on 2026-11-20). Repeat Save for 401K, TAX, AUDIT.
      (GI000142 is a per-preparer dashboard; if it lists none of these projects for you, check the value on CS205000 instead.)
- [1] Rename a status (e.g. TAX 13 "Invoice" → "Invoice Sent"; 2 TAX projects had status 13 on 2026-11-20) and Save: GI000142
      shows the new text for those projects (CS205000 STATUSTAX value 13 shows it too). Rename it back afterwards.
      (PM301000 shows the new text only after T-22.)
- [1] Add a status: click + on a department → Status ID defaults to max + 1 and Sort Order to the same; enter a description; Save.
      CS205000 shows the new value ID (2 digits) on the department's attribute.
- [1] Enter a description that already exists in the department (different case, e.g. "complete") → error; empty description → error on Save.
- [1] Enter an existing Status ID on a new row → error "Status ID n already exists…"; enter 0 → error.
- [1] Deactivate a status (clear Active), Save: CS205000 shows the value as Disabled. Re-activate it afterwards.
- [1] Try to delete a saved row → error "A saved status cannot be deleted… Clear the Active check box instead."; the row stays after refresh.
- [1] Status ID of a saved row cannot be edited (cell disabled).
- [1] Delete a new, unsaved row → it disappears (allowed).
- [1] A user without PKPM2001 rights cannot open the screen (after SM201020 is set).
- [1] ACCT 00 "Not Started" still exists on CS205000 (the sync never deletes).

## Not verifiable without a PK9 publish
- That the `Sql` item actually runs during publish and creates/seeds the table (script syntax parsed with ScriptDom and the seed
  SELECT bind-checked read-only; `SET NOEXEC` compile was not used because it needs `Query-PK9.ps1 -AllowWrite`).
- Runtime behavior of the graph: defaults, validations, delete block, the nested `PXTransactionScope` + `CSAttributeMaint` sync
  (APIs verified by reflection/IL only), `CSAttribute.List` rebuild, slot reset.
- The Modern UI HTML (`Test-ModernUI.ps1` type-checks the `.ts` only) and the site map placement / pinned state.
- Whether Modern UI's Save is enabled with no pending changes (the "select department + Save" first-sync step relies on it;
  if not, toggle any field and toggle it back, or edit a row, then Save).
- The list attribute (only used after T-22): GI/OData labels from the dynamic list (design section 2 "Not verified").


## Bug fixed 2026-11-21 (T-21b): saving/renaming statuses failed

**Symptom (user, PK9):** renaming an existing ACCT status gave "Status ID 2 already exists for department ACCT"; adding a status gave
"description cannot be blank" although the description was filled in. Nothing reached the database.

**Root cause:** the Modern UI grid saves a row with `PXCache.Update(keys, values)`, which fills a COPY of the row field by field and
raises `FieldVerifying` for each value, including the key `StatusID`, while the copy's own key fields are still empty. The old
`FieldVerifying<statusID>` scanned the department, found the saved row itself and rejected it as a duplicate (so EVERY edit of a saved
row failed). The resulting session rollback also discarded a pending new row's typed description, which is why the add attempt reported a
blank description.

**Fix:** no cross-row checks in the key-field `FieldVerifying` handlers. Duplicate description is flagged inline in `RowInserted` /
`RowUpdated` (field error, cleared when fixed) and enforced in `RowPersisting`; a duplicate Status ID on a new row is enforced in
`RowPersisting` (friendly message instead of a SQL key violation). `FieldVerifying<statusID>` only rejects values <= 0.

**Retest on PK9 after republishing:** (1) rename a status and save; (2) toggle Active and save; (3) add a status (ID defaults to max+1) with a
description and save; (4) add a status with an existing description (inline error, save blocked); (5) deactivate/reactivate; (6) a saved
row's Status ID is read-only and a saved row cannot be deleted; (7) after the first successful save of a department, check that
`CSAttributeDetail.SortOrder` is no longer NULL for it (the sync ran) and GI000142 shows the renamed text. Not verifiable before a publish:
the behavior of the real grid, and whether Save is enabled when nothing changed (the "first sync" step relies on it).

**First PK9 retest result (user, 2026-11-21 03:27 UTC, republished build):** changing ACCT 8's sort order to 9 **saved**, and the first
`CSAttributeDetail` sync ran (lead verified read-only: `PKProjectStatus` ACCT 8 SortOrder 9; STATUSACCT rows 01-08 have a SortOrder,
ACCT 02 = "Initial Review (Acctg)"). **Still pending from the retest list above:** (1) rename, (2) toggle Active, (3) add with a description,
(4) add with a duplicate description, (5) deactivate/reactivate, (6) read-only Status ID / undeletable saved row, and the sync for TAX, 401K and
AUDIT (and the GI000142 text check).

## Note T-21c (2026-11-21): slot-cache cast error after a republish
The same retest showed first-chance `InvalidCastException "[A]Definition cannot be cast to [B]Definition"` (two different `RuntimeCode_*`
assemblies) in `PKProjectStatusListAttribute.GetStatuses` -> `PXDatabase.GetSlot`: the slot key was constant, so the new build found the
OLD build's cached `Definition` object. Acumatica recovered and the save succeeded. Fix (in `src/`, ships with the next republish): the
slot key now includes `typeof(Definition).Assembly.FullName`, i.e. one slot per compiled assembly. Harmless until the next republish.

## T-24 (2026-11-21): Previous / Next Department arrows
User request: "add left and right arrows to scroll through the different departments". Source only (not published).

**What:** two toolbar buttons on PKPM2001, `PreviousDepartment` and `NextDepartment` (graph `PKProjectStatusMaint`; captions and
tooltips "Previous Department" / "Next Department" in `PKProjectStatusMessages`). They set the filter's Department to the previous/next
department **that has at least one status** (saved rows read from the DB for the current company, plus a department that so far only has
new, unsaved rows), ordered by DepartmentID (ordinal, case-insensitive: today 401K, ACCT, AUDIT, TAX), **wrapping around** at both ends.
No department selected: Next goes to the first, Previous to the last. A selected department without statuses (e.g. ADMIN) moves to its
neighbor by position (ADMIN → Next AUDIT, Previous ACCT). With only one department with statuses, the arrows do nothing.

**Same as the drop-down:** the move is `Filter.Update(copy)` with the new DepartmentID, which raises the same field/row events as picking
the department (incl. the selector check). Pending grid edits are committed to the cache first (button `CommitChanges`, the default) and
**stay pending**, exactly as when the drop-down is used; the arrows never save and never ask. Validation, persist and the CSAttributeDetail
sync are unchanged (the next Save syncs every department with changes plus the selected one, as before).

**Look (verified against PK9\Bin, 26.200):** Acumatica's record navigation buttons are `PXPreviousButton` / `PXNextButton`
(`ImageKey` `PagePrev` / `PageNext`, `DisplayOnMainToolbar`, `IsLockedOnToolbar`, `[PXUIField(MapEnableRights = Select)]`). The arrows use
the same image keys and flags on a plain `PXButton`, because `PXPreviousButton` also carries the confirmation "Any unsaved changes will be
discarded." (the drop-down asks nothing). The Modern UI main toolbar is built on the server (`QPToolBarDataProvider.CreateToolbarActionItem`
→ `ToolbarActionItem`, image set defaults to `main`), and the client draws an item with an image as an icon with the tooltip. No `.ts` /
`.html` change: declaring the actions as `PXActionState` would take them off the toolbar (T290). The action names deliberately avoid
`Previous`/`Next` (those names are treated as system actions by the toolbar builder).

**Not verifiable before a publish:** the exact rendering and position of the two icons on the PKPM2001 toolbar (expected: icon-only arrows
after Save/Cancel, always visible), the hover text, and the runtime behavior with pending changes.

**Manual test list (PK9, after the next republish):**
- [ ] Toolbar shows a left and a right arrow icon (same icons as the record navigation arrows on e.g. Projects PM301000); hover shows
      "Previous Department" / "Next Department".
- [ ] Open PKPM2001 with no department: **Next** → 401K; reopen, **Previous** → TAX.
- [ ] From 401K press **Next** repeatedly: ACCT → AUDIT → TAX → 401K (wraps). **Previous** from 401K → TAX, then AUDIT → ACCT → 401K.
- [ ] The grid shows the selected department's statuses each time (same rows as when picking it from the drop-down).
- [ ] Pick ADMIN (no statuses) in the drop-down: **Next** → AUDIT, **Previous** → ACCT. TECH: **Next** → 401K.
- [ ] Unsaved change: edit a description in ACCT (do not save), press **Next**: no question, no save; go back to ACCT with **Previous**:
      the edit is still there (pending). Compare with doing the same via the drop-down: identical. Then Cancel (or Save) as usual.
- [ ] Save after moving with an arrow: Save works as before (the edited department is saved and synced).
- [ ] A user with view-only rights on PKPM2001 can still use the arrows.