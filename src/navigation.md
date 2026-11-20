# src/ — customization source (→ customization project `ProjectTask`)

**This is the only place to edit customization code.** Claim the file or area in `../scratchpad.md` first.
Content was taken **from the PK9 database** (`CustObject` of project `ProjectTask`, CompanyID 2) and the site's per-tenant/published files on 2026-11-13, so it equals what is live.

| Folder | Is | Rules |
|---|---|---|
| `Code/*.cs` | `<Graph ClassName="<file name>" FileType="…">` items, compiled by Acumatica on publish | File name = item name; must be unique across ALL customization projects. |
| `Sql/*.sql` | `<Sql>` custom scripts that create the new tables (`PMDocumentGathering`, `PMSampleTesting`, `PKProjectStatus` + its 54-row seed, T-21) | Must be idempotent (`IF NOT EXISTS`, insert-only-missing); runs on publish. Checked by `../tools/Test-StatusSeed.ps1` (PKProjectStatus). |
| `Tables/*.xml` | `<Table>` items adding `Usr*` columns to EXISTING tables (`Contract`, `PMTask`, `Contact`) | One file per table, verbatim XML. Each new column needs a DAC extension field. Exceptions on purpose: `Contract.UsrPGReleasedFinalDeliverables` and `Contract.UsrPGTechStatus` (retired fields; columns kept declared, never dropped). Removing a column line never drops the DB column. |
| `Automation/*.xml` | `<Automation screen="…">` workflow items (PM301000, PKPM3001) | Verbatim live XML. |
| `PageCustomizations/*.xml` | `<Page path="~/pages/…">` Classic screen-editor items (PKPM3002, PKPM3003, PM301000, PM302000, CR302000) | Verbatim live XML; Classic UI only. |
| `SiteMap/*.xml` | `<SiteMapNode>` items (screen + Modern UI workspace entry), file named by ScreenID | Verbatim. Workspace *Projects*: PKPM3001/3002 in subcategory *401(k) Audit*, PKPM2001 in *Preferences*. |
| `Pages/PK/*.aspx(.cs)` | `<File AppRelativePath="Pages\PK\…"/>` Classic shells (never rendered) | Minimal; PXDataSource TypeName/PrimaryView only. |
| `FrontendSources/screens/<MOD>/<SCREEN>/` | `<PerTenantFile AppRelativePath="screens\…" ScreenId="<SCREEN>"/>` — **Modern UI** | Path mirrors `PK9\FrontendSources\screen\src\screens\…`. Type-check with `../tools/Test-ModernUI.ps1`. |

## Code items → live files
| Repo file (= DB item) | Contents | Live runtime copy (`PK9\App_RuntimeCode\`) |
|---|---|---|
| `Code/PMTask.cs` | `PMTaskExt`: `UsrPGTaskStatus` (9 statuses), `UsrPGDueDate`, `UsrPGExtendedDueDate`, `UsrPGIsExtended`, `UsrPGPreparer`, `UsrPGReviewer`, `UsrPGTechReviewer`, `UsrPGDepartment` (defaults from the project's department, read-only) | `ProjectTask.PMTask.cs` *(not `PMTask.cs` — that one belongs to Velixo)* |
| `Code/Contract.cs` | `ContractExt`: `UsrPGDepartment`, `UsrPGPriority`, `UsrPGIsFinal`, `UsrPGFinishDate`, `UsrPGYearEnd`, `Usr401kStatus`, `UsrPGComplexity`, `UsrPGReleaseDeliverables`, `UsrPGAcctStatus`, `UsrPGAuditStatus`, `UsrPGTaxStatus`, `UsrPGReleasedELF`. The 4 department status fields use `[PKProjectStatusList("401K"/"TAX"/"ACCT"/"AUDIT")]` (T-22, `../docs/status-switch.md`); Priority/Complexity keep their `PXIntList`. (`UsrPGTechStatus` removed in T-11; its DB column stays and is still declared in `Tables/Contract.xml`, see `../docs/tech-removal.md`) | `Contract.cs` |
| `Code/PMDocGatheringMaint.cs` | Graph of **Doc Gathering** (PKPM3001; Classic page PKPM3003.aspx), primary view `DocGathering` | same name |
| `Code/PMDocumentGathering.cs` | DAC / table `PMDocumentGathering` (one row per project; payroll provider, record keeper, plan docs…) | same name |
| `Code/PMSampleTestingMaint.cs` | Graph of **Sample Testing** (PKPM3002), primary view `Sampletest` | same name |
| `Code/PMSampleTesting.cs` | DAC / table `PMSampleTesting` | same name |
| `Code/ProjectTask_Constants.cs` | Status lists, `RecordKeeperList`, `PayrollProviderList`, `Department`, … (DB codes) | same name |
| `Code/ProjectTask_Messages.cs` | Display strings for the constants | same name |
| `Code/PKProjectStatus.cs` | **T-21, not published.** DAC `PKProjectStatus` (table of the same name: DepartmentID + StatusID key, Description, SortOrder, Active), filter DAC `PKProjectStatusFilter`, `PKProjectStatusMessages` (NewDac) | none yet |
| `Code/PKProjectStatusMaint.cs` | **T-21.** Graph of **Department Statuses** (PKPM2001): rules + one-way sync into `CSAttributeDetail` (STATUS401K/TAX/ACCT/AUDI) on save. See `../docs/status-screen.md` (NewGraph) | none yet |
| `Code/PKProjectStatusListAttribute.cs` | **T-21.** `PKProjectStatusListAttribute : PXIntListAttribute`, DB-backed status drop-down per department (`[PKProjectStatusList("401K")]`); on the 4 `ContractExt` status fields since T-22; copies (Copy Project, Paste) are exempt from the inactive-value check (NewFile) | none yet |
| `Code/PKProjectTemplateExt.cs` | **T-32, not published.** `ProjectTask.PKProjectTemplateExt : PXGraphExtension<ProjectEntry>`: `[PXOverride] DefaultFromTemplateProjectSettings` (template → project Department/Priority/Complexity) and `[PXOverride] OnCopyPasteTaskInserted` (template/common task → new task Preparer/Reviewer/Tech Reviewer/Due Date; create from template AND Add Common Task; skips Copy Project / Create Template). Copy only when empty; invalid values become field warnings. `../docs/template-copy.md` (NewFile) | none yet |
| `Code/PKTemplateDueDate.cs` | **T-32.** Static due-date rule (no Acumatica references): template month/day rolled to the first date on/after the project start (`ActiveRule`, switchable to as-is). Unit test `../tools/Test-TemplateDates.ps1` (NewFile) | none yet |
| `Code/PKContactExt.cs` | **Merged from project `Contact` (T-10).** `PX.Objects.CR.ContactExt : PXCacheExtension<Contact>`, 8 bool fields on Contacts (CR302000): `UsrPGPortalAccess`, `UsrPG1040ES`, `UsrPG1040Check`, `UsrPGTaxPlan`, `UsrPGDoNotMarket`, `UsrPGAcuMarket`, `UsrPGRetire`, `UsrServiceProviderQ`. Byte copy of the code Acumatica generated from Contact's DAC item. Columns: `Tables/Contact.xml` | before cutover: `PX_Objects_CR_Contact_extensions.cs` (Contact project); after: `PKContactExt.cs`. See `../docs/contact-merge.md` |

## Screens
| Screen | Modern UI | Classic shell | Notes |
|---|---|---|---|
| `PKPM2001` Department Statuses (T-21) | `screens/PK/PKPM2001/` (`PKPM2001.ts`, `.html`, `views.ts`): Department filter + status grid; graph `ProjectTask.PKProjectStatusMaint` | `Pages/PK/PKPM2001.aspx(.cs)` | Projects → Preferences. Table + seed: `Sql/PKProjectStatus.sql`. Rights set on SM201020 after publish (D7). |
| `PKPM3001` Document Gathering | `screens/PK/PKPM3001/` (`PKPM3001.ts`, `.html`, `views.ts`) — graph `ProjectTask.PMDocGatheringMaint` | `Pages/PK/PKPM3003.aspx(.cs)` | Site map node ScreenID PKPM3001 points at **PKPM3003.aspx** (historical naming). |
| `PKPM3002` Sample Testing | `screens/PK/PKPM3002/` — graph `ProjectTask.PMSampleTestingMaint` | `Pages/PK/PKPM3002.aspx(.cs)` | |
| `PM301000` Projects (extension) | `screens/PM/PM301000/extensions/PM301000_PKProjectInfo.*` — Contract `UsrPG*` in *Project Properties*, task `UsrPG*` columns in *Tasks* grid | — | `PM301000_PKTimeClock.*` in the same folder belongs to **PKTimeClock**, not this repo. |
| `PM302000` Project Tasks (extension) | `screens/PM/PM302000/extensions/PM302000_PKTaskInfo.*` — task `UsrPG*` fields | — | |
| `PM208000` Project Templates (extension, T-31, not published) | `screens/PM/PM208000/extensions/PM208000_PKProjectInfo.*` — *PK Project Info* fieldset (Department, Priority, Complexity) + Tasks grid columns (Due Date, Preparer, Reviewer, Tech Reviewer) | — | `PM208000_PKTimeClock.*` on the site belongs to **PKTimeClock**. Values only stored until T-32. `../docs/template-screens.md` |
| `PM208010` Template Tasks / `PM208030` Common Tasks (extensions, T-31, not published) | `screens/PM/PM208010/extensions/PM208010_PKTaskInfo.*`, `screens/PM/PM208030/extensions/PM208030_PKTaskInfo.*` — *PK Task Info* fieldset (same 4 task fields) | — | PM208030 = global common tasks (ContractID 0) |
| `CR302000` Contacts (extension, from `Contact`, T-10) | `screens/CR/CR302000/extensions/CR302000_PKMarketing.*`: *PK Marketing* tab after `#tabGeneral`, view `ContactCurrent` (class `ContactDetail`), 8 `ContactExt` fields | `PageCustomizations/CR302000.xml` (Classic tab, same fields) | Verbatim live copies; the view/class pairing is correct (see `../docs/contact-merge.md`). |

> `EP307000_PKTimeClock.*` on the site belongs to another project (`ProjectTime`). `CR302000_PKMarketing.*` still belongs to `Contact` on the site until the T-10 cutover publish.
> `src/` has **no** Modern UI for the Classic shell pages beyond the screens above.
