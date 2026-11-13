# ProjectTask (PMTask/Contract UsrPG fields, Doc Gathering, Sample Testing) — Agent Scratchpad

> **Living document.** Every agent: read this top to bottom before working, claim your work in
> [Work claims](#work-claims), and update [Learnings](#learnings) / [Session log](#session-log) before you finish.
> Master multi-agent rules: `..\Acumatica-PK-TimeClock\scratchpad.md` → Coordination rules.

Last updated: 2026-11-13, session 1 (repo synced from PK9 + restructure).
**State:** LIVE and stable. Change only on user request; other projects depend on the `UsrPG*` fields.

## Read order for a new agent
1. This file. 2. `navigation.md`, then the `navigation.md` of the folder you touch.
3. `..\Acumatica-PK-TimeClock\docs\T290.md` before touching any `.ts` / `.html`.

## What this project is
Customization project **`ProjectTask`** (PK9 DB: `CustProject`, CompanyID 2):
- **PMTask** custom fields (Task Status, Due Date, Extended Due Date / Is Extended, Preparer, Reviewer, Tech Reviewer, Department) and **Contract** custom fields (Department, Priority, Is Final, Finish Date, Year End, 401k Status, Complexity, Release Deliverables, Acct/Audit/Tax/Tech Status, Released ELF).
- **Doc Gathering** screen `PKPM3001` (table `PMDocumentGathering`) and **Sample Testing** screen `PKPM3002` (table `PMSampleTesting`), in workspace *Projects* → *401(k) Audit*.
- Modern UI extensions of Projects (PM301000) and Project Tasks (PM302000) that show the fields.

## What is live on PK9 (verified 2026-11-13)
| Layer | Source here | Live | Status |
|---|---|---|---|
| 8 code items | `src/Code/*.cs` | DB `CustObject` Type=Graph **and** `App_RuntimeCode` | `src/Code` was extracted from the DB; each file equals its runtime copy (normalized line endings) |
| 2 SQL scripts | `src/Sql/*.sql` | DB `CustObject` Type=Sql | extracted from the DB |
| 2 table items | `src/Tables/{Contract,PMTask}.xml` | DB `CustObject` Type=Table | extracted from the DB |
| 2 site map nodes | `src/SiteMap/PKPM3001.xml`, `PKPM3002.xml` | DB `CustObject` Type=SiteMapNode | extracted from the DB |
| Classic shells | `src/Pages/PK/*` | `PK9\CstPublished\Pages_PK\` | copied from there (PKPM3003.aspx(.cs) is new to the repo) |
| Modern UI | `src/FrontendSources/screens/…` | `PK9\FrontendSources\screen\src\customizationScreens\Company\screens\` | copied from there (8 files + 2 extensions pairs); `Test-ModernUI.ps1` = OK |

### Drift found 2026-11-13 (the repo was behind the live site; now fixed)
- Live added **record keepers** (Ameritas, Guideline, Human Interest, Sentinel, Vestwell) and **payroll providers** (Ceridian Day Force, Gusto, PEO) to `ProjectTask_Constants`, `ProjectTask_Messages`, `PMDocumentGathering`, `PMSampleTesting`.
- Live `PMTask` ext: task status list order changed (…, "Clearing Points", "In Final Review", "Bill Client", "Complete" now come before "Waiting on Client", "On Hold", "Stuck, need help"); `UsrPGDepartment` now **defaults from the project's department** and is read-only (was a selector on `EPDepartment`).
- `PMSampleTesting` / `PMDocumentGathering` DACs now derive from `PXBqlTable`.
- `PKPM3003.aspx(.cs)` and Modern UI for PKPM3001/3002 and the PM301000/PM302000 extensions were never in the repo.
- Old repo C# files (`ContractExtensions.cs`, `PMTaskExtensions.CS`, `Helper/*`, …) were replaced by the DB-named files; the old versions remain in git history. `project.xml`/zips were moved to `exports/` untouched.

## Dependencies
| Consumer | Uses |
|---|---|
| `PKTimeClock` (`Acumatica-PK-TimeClock`) | `PMTaskExt.UsrPGPreparer/Reviewer/TechReviewer/TaskStatus/DueDate`, `ContractExt.UsrPGDepartment`, task-status list (it keeps its own copy in `PKTimeClockConstants` — update both if statuses change). Preparer Dashboard GI000142 and PKEP3002 mirror the Preparer columns. |
| Generic Inquiry GI000142 "PK - Preparer Dashboard" | PMTask `UsrPG*` columns |

## Open items
| # | Item | Owner |
|---|---|---|
| 1 | `exports/project.xml` and zips are stale (21.211, Classic). Re-export from PK9 if a deployable package is needed. | user |
| 2 | Sync check on every change: the live DB is the source of truth for CustObject items; re-run the extract (see Learnings) after the user publishes from the Customization Editor to refresh `src/`. | agent |
| 3 | `PKPM3003.aspx` is the Classic page behind site-map ScreenID PKPM3001 (historical naming) — leave as is. | — |
| 4 | `.vs/` and `.DS_Store` are tracked in git although now listed in `.gitignore`; untrack with `git rm -r --cached .vs .DS_Store` when convenient. | user |

## Coordination rules
1. Claim work below before editing; release when done. 2. Edit source only under `src/`.
3. Never publish customizations; never edit generated folders on PK9; never write to the DB without explicit user approval.
4. Record findings here, not just in chat.

## Work claims
| Area | Claimed by | Since | Notes |
|---|---|---|---|
| *(none)* | | | |

## Learnings
- Customization item content is in `CustObject.Content` (XML with a `<CDATA name="Source">` / `CustomScript` body) joined `CustObject.ProjectID = CustProject.ProjID`. A read-only SELECT is enough to re-extract `src/` (filter `CustProject.Name='ProjectTask'`, `CompanyID=2`, Type in Graph/Sql/Table/SiteMapNode). File items (`File#…`, `PerTenantFile#…`) store only a FileID — their bodies are the site files (`CstPublished\Pages_PK`, `FrontendSources\…\customizationScreens\Company\screens`).
- Runtime copies in `App_RuntimeCode` carry other names than the item: PMTask item → `ProjectTask.PMTask.cs`; `PMTask.cs` there is Velixo's.
- `PMTask` and `Contract` code items are `FileType="NewFile"` extensions (`PXCacheExtension`), not DACs; `PMDocumentGathering` / `PMSampleTesting` are `NewDac`; the two `*Maint` are `NewGraph`.
- The Modern UI extensions on the site are named `…_PKProjectInfo` / `…_PKTaskInfo` and belong here; `…_PKTimeClock` ones on the same screens belong to other projects.
- Folder layout changed 2026-11-13 (old → new): `*.cs` + `Helper/` → `src/Code/`, `DBScripts/` → `scripts/`, `project.xml` + `ProjectTask*.zip` + `PKPM3001-PKPMD.xml` → `exports/`, `PKPM3002.aspx` → `src/Pages/PK/`.

## Session log
| Date | Session | Summary |
|---|---|---|
| 2026-11-13 | 1 | Extracted the live project from the PK9 DB (read-only) into `src/`; added Modern UI (PKPM3001/3002 + PM301000/PM302000 extensions) and published Classic shells; found and fixed code drift (see above); restructured on the PK-TimeClock model; added CLAUDE.md, scratchpad.md, navigation.md files, `tools/Test-ModernUI.ps1`, `.gitignore`; updated README and the PK9 navigation. Not committed. |
