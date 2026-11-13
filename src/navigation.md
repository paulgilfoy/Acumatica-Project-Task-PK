# src/ — customization source (→ customization project `ProjectTask`)

**This is the only place to edit customization code.** Claim the file or area in `../scratchpad.md` first.
Content was taken **from the PK9 database** (`CustObject` of project `ProjectTask`, CompanyID 2) and the site's per-tenant/published files on 2026-11-13, so it equals what is live.

| Folder | Is | Rules |
|---|---|---|
| `Code/*.cs` | `<Graph ClassName="<file name>" FileType="…">` items, compiled by Acumatica on publish | File name = item name; must be unique across ALL customization projects. |
| `Sql/*.sql` | `<Sql>` custom scripts that create the two new tables | Must be idempotent (`IF NOT EXISTS`); runs on every publish. |
| `Tables/*.xml` | `<Table>` items adding `Usr*` columns to EXISTING tables (`Contract`, `PMTask`) | One file per table, verbatim XML. Each column must match a DAC extension field. |
| `SiteMap/*.xml` | `<SiteMapNode>` items (screen + Modern UI workspace entry), file named by ScreenID | Verbatim. Workspace *Projects*, subcategory *401(k) Audit*. |
| `Pages/PK/*.aspx(.cs)` | `<File AppRelativePath="Pages\PK\…"/>` Classic shells (never rendered) | Minimal; PXDataSource TypeName/PrimaryView only. |
| `FrontendSources/screens/<MOD>/<SCREEN>/` | `<PerTenantFile AppRelativePath="screens\…" ScreenId="<SCREEN>"/>` — **Modern UI** | Path mirrors `PK9\FrontendSources\screen\src\screens\…`. Type-check with `../tools/Test-ModernUI.ps1`. |

## Code items → live files
| Repo file (= DB item) | Contents | Live runtime copy (`PK9\App_RuntimeCode\`) |
|---|---|---|
| `Code/PMTask.cs` | `PMTaskExt`: `UsrPGTaskStatus` (9 statuses), `UsrPGDueDate`, `UsrPGExtendedDueDate`, `UsrPGIsExtended`, `UsrPGPreparer`, `UsrPGReviewer`, `UsrPGTechReviewer`, `UsrPGDepartment` (defaults from the project's department, read-only) | `ProjectTask.PMTask.cs` *(not `PMTask.cs` — that one belongs to Velixo)* |
| `Code/Contract.cs` | `ContractExt`: `UsrPGDepartment`, `UsrPGPriority`, `UsrPGIsFinal`, `UsrPGFinishDate`, `UsrPGYearEnd`, `Usr401kStatus`, `UsrPGComplexity`, `UsrPGReleaseDeliverables`, `UsrPGAcctStatus`, `UsrPGAuditStatus`, `UsrPGTaxStatus`, `UsrPGReleasedELF`, `UsrPGTechStatus` | `Contract.cs` |
| `Code/PMDocGatheringMaint.cs` | Graph of **Doc Gathering** (PKPM3001; Classic page PKPM3003.aspx), primary view `DocGathering` | same name |
| `Code/PMDocumentGathering.cs` | DAC / table `PMDocumentGathering` (one row per project; payroll provider, record keeper, plan docs…) | same name |
| `Code/PMSampleTestingMaint.cs` | Graph of **Sample Testing** (PKPM3002), primary view `Sampletest` | same name |
| `Code/PMSampleTesting.cs` | DAC / table `PMSampleTesting` | same name |
| `Code/ProjectTask_Constants.cs` | Status lists, `RecordKeeperList`, `PayrollProviderList`, `Department`, … (DB codes) | same name |
| `Code/ProjectTask_Messages.cs` | Display strings for the constants | same name |

## Screens
| Screen | Modern UI | Classic shell | Notes |
|---|---|---|---|
| `PKPM3001` Document Gathering | `screens/PK/PKPM3001/` (`PKPM3001.ts`, `.html`, `views.ts`) — graph `ProjectTask.PMDocGatheringMaint` | `Pages/PK/PKPM3003.aspx(.cs)` | Site map node ScreenID PKPM3001 points at **PKPM3003.aspx** (historical naming). |
| `PKPM3002` Sample Testing | `screens/PK/PKPM3002/` — graph `ProjectTask.PMSampleTestingMaint` | `Pages/PK/PKPM3002.aspx(.cs)` | |
| `PM301000` Projects (extension) | `screens/PM/PM301000/extensions/PM301000_PKProjectInfo.*` — Contract `UsrPG*` in *Project Properties*, task `UsrPG*` columns in *Tasks* grid | — | `PM301000_PKTimeClock.*` in the same folder belongs to **PKTimeClock**, not this repo. |
| `PM302000` Project Tasks (extension) | `screens/PM/PM302000/extensions/PM302000_PKTaskInfo.*` — task `UsrPG*` fields | — | |

> `CR302000_PKMarketing.*` and `EP307000_PKTimeClock.*` on the site belong to other projects (`Contact`, `ProjectTime`).
> `src/` has **no** Modern UI for the Classic shell pages beyond the screens above.
