# T-11 — Remove Tech Status and the TECH-department display (decision P2)

Source change only (2026-11-20). **Nothing is published.** Ships in publish bundle A (`PLAN.md`).
P2: remove `Contract.UsrPGTechStatus` and the TECH-only display; **keep `PMTask.UsrPGTechReviewer`** (the time clock
uses it); **never drop DB columns**.

## What changed in `src/`
| Item (package) | File | Change |
|---|---|---|
| `Graph Code#Contract` | `src/Code/Contract.cs` | Removed the `UsrPGTechStatus` property and its BQL class `usrPGTechStatus` (`PXDBInt`, `PXIntList` 1-4 "Tech Status 1..4"). All other fields unchanged. |
| `Automation#PM301000` | `src/Automation/PM301000.xml` | Removed the field-state row `UsrPGTechStatus` and the condition `66bc0c2c-56d6-4521-aab0-2dd2508d97a9` "Department <> Tech" (`UsrPGDepartment <> 'TECH'`). That condition was used only by the Tech Status row (checked in the item and in the PK9 `AUScreenFieldState` table). The other 5 conditions and 11 field rows are byte-identical to live (verified against `build\live\src`). The remaining condition keeps `Order="6"` (gap at 5 is harmless; not renumbered to keep the diff minimal). |
| `Page ~/pages/pm/pm301000.aspx` (Classic) | `src/PageCustomizations/PM301000.xml` | Removed the `AddItem` `PXDropDown CstPXDropDown7` bound to `UsrPGTechStatus` in the Summary tab. The `pageSource` attribute (base ASPX, no `Usr*` fields) is unchanged. |
| `PerTenantFile ...PM301000_PKProjectInfo.html` | `src/FrontendSources/screens/PM/PM301000/extensions/` | Removed `<field name="UsrPGTechStatus" if.bind="...UsrPGDepartment.value.id == 'TECH'">`, the only TECH-department branch in the code. |
| `PerTenantFile ...PM301000_PKProjectInfo.ts` | same folder | Removed `UsrPGTechStatus: PXFieldState;` from `PM301000_ProjectProperties_PKProjectInfo`. |

**Kept on purpose**
- `UsrPGTechReviewer` everywhere: `Code/PMTask.cs`, `Tables/PMTask.xml`, Automation field row "Tech Reviewer", Classic Tasks grid column, Modern UI Tasks grid (`PM301000_Tasks_PKProjectInfo`) and PM302000.
- `UsrPGDepartment` and the department **TECH** itself (`EPDepartment` record; 2 projects have it). Only the TECH-specific status display is gone; a TECH project now behaves like ADMIN/blank-department projects (no department status field).
- **The DB column `Contract.UsrPGTechStatus` and its declaration in `src/Tables/Contract.xml`** (see below). The one project with a value (`2022ACUMATICA / LGORGANICS.001`, value 4) keeps it in the DB; it is just not shown.

## The column declaration (`Tables/Contract.xml`): kept
**Removing a column from a Table item does not drop the DB column.** Evidence (2026-11-20, read-only):
1. *Observed on PK9:* `PMTask.UsrPGAccountingStatus` and `PMTask.UsrPGTaxStatus` exist in `sys.columns`, yet **no** `CustObject` Table item in any company/project declares them (query below). Columns that left the customization stay in the table.
2. *Engine code:* an IL scan of `PK9\Bin` shows the "DROP COLUMN" DDL only in `PX.DbServices` (`CmdColumnDrop`, constructed only by `AcumaticaDbKeeperBase`, the install/upgrade schema keeper) and in `PXSqlDatabaseProvider.SetReadDeletedCapability` (system column). None of the `PX.Web.Customization*` assemblies construct `CmdColumnDrop` or contain the string, so publishing a Table item only adds/alters columns.

**Decision: keep the `<Column ColumnName="UsrPGTechStatus" .../>` line.** Reasons:
- The column must stay (P2). Keeping it declared states that intent in the package and makes the project, not chance, the owner of the column on every environment (PK9, live, any sandbox/tenant the zip is imported into).
- Precedent: `UsrPGReleasedFinalDeliverables` is already declared in the same Table item without a DAC field, live and harmless.
- Rollback is a pure code/UI revert: re-adding the DAC field needs no table change.
- Smallest diff vs live: the `Table Contract` item is unchanged (`Verify-Package` = OK for it).
- A declared column with no DAC field is not read or written by the application, so there is no runtime effect.

## Publish order (blocking): Generic Inquiries first
A GI that references `PMProject.usrPGTechStatus` breaks (error or blank column) once the DAC field is gone; 4 of the
original 7 were OData/Power BI. **Clean every GI reference on PK9 and on live BEFORE publishing this change.**
User-owned (SM208000, UI). State on PK9 at 2026-11-20:
- GI000140, GI641660, GI641709 and the result column of GI641645: **removed** by the user.
- The 3 unpublished GIs `Preparer Time Sapro` (b8f7f6c8-…), `PK - Preparer Time Sapro Manager` (50e5406b-…), `PK Open Project` (8e31422e-…): **deleted** (no `GIDesign` rows any more).
- **Still open: GI641645 "PowerBI - Project Revenue" (DesignID `17d837a7-db20-4e76-9207-93dfdbcd6f9a`), Conditions line 8 `PMProject.usrPGTechStatus = 4`.** It is inactive (`IsActive = False`), in an inactive bracket group: line 5 opens `(`, line 9 closes `)`, so deleting line 8 alone keeps the brackets balanced. Delete it anyway: an inactive condition still names the field.
- **Live (hosted, no SQL):** repeat the same cleanup in the UI on GI000140, GI641660, GI641645 (result column + Conditions), GI641709 and the 3 unpublished GIs if they exist there. Check with the SM208000 search or by opening each GI.

### Read-only SQL to re-check PK9 (CompanyID 2)
Run from this repo's folder (`Query-PK9.ps1` refuses writes; never pass `-AllowWrite`):
```powershell
$q = '..\Acumatica-PK-TimeClock\tools\Query-PK9.ps1'
# Must return no rows before publishing:
& $q "select 'GIResult' src, d.Name, d.DesignID, r.LineNbr, r.Field from GIResult r join GIDesign d on d.CompanyID=r.CompanyID and d.DesignID=r.DesignID where r.CompanyID=2 and r.Field like '%TechStatus%' union all select 'GIWhere', d.Name, d.DesignID, w.LineNbr, w.DataFieldName from GIWhere w join GIDesign d on d.CompanyID=w.CompanyID and d.DesignID=w.DesignID where w.CompanyID=2 and (w.DataFieldName like '%TechStatus%' or w.Value1 like '%TechStatus%' or w.Value2 like '%TechStatus%')"
# Wider sweep (all counts must be 0; AUScreenFieldState is 1 until this change is published, then 0):
& $q "select 'GIOn' src, count(*) n from GIOn where CompanyID=2 and (ParentField like '%TechStatus%' or ChildField like '%TechStatus%') union all select 'GIFilter', count(*) from GIFilter where CompanyID=2 and (FieldName like '%TechStatus%' or DefaultValue like '%TechStatus%') union all select 'GISort', count(*) from GISort where CompanyID=2 and DataFieldName like '%TechStatus%' union all select 'GIGroupBy', count(*) from GIGroupBy where CompanyID=2 and DataFieldName like '%TechStatus%' union all select 'GINavCond', count(*) from GINavigationCondition where CompanyID=2 and (DataField like '%TechStatus%' or ValueSt like '%TechStatus%') union all select 'BPEventTrig', count(*) from BPEventTriggerCondition where CompanyID=2 and (FieldName like '%TechStatus%' or Value like '%TechStatus%') union all select 'SYMappingField', count(*) from SYMappingField where CompanyID=2 and (FieldName like '%TechStatus%' or Value like '%TechStatus%') union all select 'FilterRow', count(*) from FilterRow where CompanyID=2 and DataField like '%TechStatus%' union all select 'PivotField', count(*) from PivotField where CompanyID=2 and (Expression like '%TechStatus%' or Filter like '%TechStatus%') union all select 'AUScreenFieldState', count(*) from AUScreenFieldState where CompanyID=2 and FieldName like '%TechStatus%'"
```
Result 2026-11-20: first query = 1 row (GI641645 `GIWhere` line 8); sweep = all 0 except `AUScreenFieldState` 1 (the
published Automation item, removed by this change).

### After publishing (read-only)
```powershell
# Only "Table Contract" may still mention TechStatus (the kept column declaration):
& $q "select p.Name proj, o.Type, o.Name from CustObject o join CustProject p on p.ProjID=o.ProjectID and p.CompanyID=o.CompanyID where o.CompanyID=2 and cast(o.Content as nvarchar(max)) like '%TechStatus%'"
# Should be empty (Automation state rebuilt on publish); if a row remains, tell the lead (stale automation state):
& $q "select FieldName, HideCondition from AUScreenFieldState where CompanyID=2 and FieldName='UsrPGTechStatus'; select ConditionID, ConditionName from AUScreenConditionState where CompanyID=2 and ConditionID='66bc0c2c-56d6-4521-aab0-2dd2508d97a9'"
# Data is intact (expect 1 row, value 4):
& $q "select ContractCD, UsrPGDepartment, UsrPGTechStatus from Contract where CompanyID=2 and UsrPGTechStatus is not null"
```

## Checks run (2026-11-20, T-11 worker)
- `tools\Test-Compile.ps1 -IncludeRuntimeCode -ExcludeRuntimeFile PX_Objects_CR_Contact_extensions` → COMPILE OK (46 files; no other runtime code reads the field).
- `tools\Test-ModernUI.ps1` → TS CHECK OK.
- `tools\Build-Package.ps1` → 39 items.
- `tools\Verify-Package.ps1 -ExtraProject Contact` → PACKAGE DIFFERS FROM LIVE with exactly the intended items: `Automation#PM301000`, `Code#Contract`, `Page ~/pages/pm/pm301000.aspx`, `PM301000_PKProjectInfo.html`, `PM301000_PKProjectInfo.ts` (+ the expected warning "App_RuntimeCode Contract differs"). Everything else OK, including `Table Contract`.

## Rollback
The DB column and its value are never touched, so rollback is code only:
1. Re-import the backup package taken before the bundle-A publish (or build `ProjectTask.zip` from the commit before T-11 with `tools\Build-Package.ps1`) and publish it (same project set as before: `Contact` handling per `contact-merge.md`).
2. The Tech Status field, the "Department <> Tech" condition and the TECH display come back with the old value (4) on `LGORGANICS.001`.
3. Re-adding Tech Status to GIs is a manual GI edit (the removed GI columns are not restored by the package).

## Manual test checklist (user, after publish on PK9)
***ALL PASS***
- [1] Before publishing: the first SQL above returns no rows on PK9; live GIs cleaned in the UI.
- [ 1] Publish completes without errors (bundle A).
- [1 ] PM301000, project `2022ACUMATICA / LGORGANICS.001` (department TECH, had Tech status 4): no "Tech Status" field anywhere (Summary tab, *PK Department Status* group). The group shows no status field, like an ADMIN project. Save works.
- [ 1] PM301000, project `2022ACUMATICA / MCMAHONC.001` (TECH, no value): same; saves.
- [1 ] Other departments still show only their own fields: a **TAX** project shows Tax Status + Released in ELF (and its Tasks grid still shows Extended Due Date / Extended); a **401K** project shows 401k Status + Release of Final Deliverables; an **ACCT** project shows Acct Status; an **AUDIT** project shows Audit Status. Switch a test project's department between two values and check the fields swap (do not save if it is a real project).
- [1 ] Tasks tab of any project: Preparer, Reviewer, **Tech Reviewer**, Due Date, Task Status still show and save; PM302000 Project Tasks shows Tech Reviewer.
- [1 ] Time clock / Preparer Dashboard (GI000142) still open and list tasks with Tech Reviewer (TimeClock reads `UsrPGTechReviewer`).
- [1 ] GI641645 / Power BI Project Revenue refresh works (no field error).
- [1 ] Post-publish SQL above: only `Table Contract` mentions TechStatus; no automation state rows; data row still 4.
