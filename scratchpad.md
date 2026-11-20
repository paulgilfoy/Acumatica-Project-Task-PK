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
| 5 | **Drift (T-00, 2026-11-20): live `ProjectTask` has 34 items, `src/` covers 28.** Missing from `src/`: `Automation#PKPM3001`, `Automation#PM301000` (department hide/disable conditions for the status fields on PM301000, incl. `UsrPGTechStatus` / "Department <> Tech" → T-11 must edit it) and 4 Classic `Page` items (`~/pages/pk/pkpm3002.aspx`, `pkpm3003.aspx`, `~/pages/pm/pm301000.aspx`, `pm302000.aspx`). A zip built from `src/` today would **delete** them on import. Fix (needs lead/user OK): copy `build\live\src\Automation\` and `build\live\src\PageCustomizations\` (from `tools\Get-LiveProject.ps1`) into `src\`; then `tools\Verify-Package.ps1` = PACKAGE MATCHES LIVE (proven on a temp copy). All 28 other items match live. | lead |
| 6 | **T-10 cutover pending:** `src/` now contains the whole `Contact` project (5 live items → 5 package items). Publish = import `build\ProjectTask.zip`, publish with `Contact` **unticked** (never both). Steps, rollback, checklist: `docs/contact-merge.md`. After it: T-90 re-sync (then plain `Verify-Package.ps1` should match), T-91 PK9 nav, user archives the Contact repo. | user |

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
- **Packaging (T-00):** `tools\Build-Package.ps1` → `build\ProjectTask.zip`; `tools\Verify-Package.ps1` compares it item by item with live (`CustObject` Type+Name, attributes, bodies with line endings normalized). Code `FileType` (NewFile/NewGraph/NewDac) is kept in the `$CodeFileType` table of Build-Package; new code files default to `NewFile`. Importing a zip **replaces** all items of the project, so anything missing from `src/` is deleted.
- **Uploaded file bodies are not in the PK9 DB:** `UploadFileRevision.Data` is empty and `BlobHandler` points at Azure blob storage (`BlobStorageConfig` = `PXBlobStorageAzureProvider`, account `pricekubeckablobstorage`, same settings as the live tenant). Tools use the site copies instead (`PK9\Pages\PK\`, `…\customizationScreens\Company\screens\`); `UploadFileRevision.Size` is KB rounded up (sanity check).
- Live `CustObject.Content` always has LF: Acumatica parses project.xml as XML, so CRLF inside CDATA becomes LF. Package line endings don't matter.
- `Page` items (`<Page path="~/pages/…" pageSource="…">`) hold a deflate+base64 Classic page source; for PKPM3002/3003 it equals the ASPX shell. They don't affect Modern UI but are part of the project.
- `..\Acumatica-PK-TimeClock\tools\Test-Compile.ps1` compiles **the TimeClock repo's** `src\Code` (it resolves the repo from its own folder) — it never sees this repo's edits. Use **`tools\Test-Compile.ps1 -IncludeRuntimeCode`** here (leaves out our published copies, incl. `ProjectTask.PMTask.cs`).
- The project also exists in CompanyID -100020002 (same 34 items, same hashes) and -100020001 (23 items, older) — snapshot/system rows; tools use CompanyID 2.
- **Contact merge (T-10):** the live `Contact` project had 5 items: `DAC#PX.Objects.CR.Contact` (Data Class editor item; Acumatica generates `App_RuntimeCode\PX_Objects_CR_Contact_extensions.cs` from it), `Table#Contact`, Classic `Page ~/pages/cr/cr302000.aspx`, and 2 `PerTenantFile` (`CR302000_PKMarketing.ts/.html`). No SQL, site map, Automation or graph code. All 5 are now in `src/`; the DAC item is carried as Code item `PKContactExt` (byte copy of the generated `.cs`). `Get-LiveProject.ps1 -ProjectName Contact` works as is (DAC goes to `Other\DAC\`).
- **Never publish `Contact` and `ProjectTask` together** from now on: duplicate `PX.Objects.CR.ContactExt` (CS0101, reproduced with `Test-Compile.ps1 -IncludeRuntimeCode` without `-ExcludeRuntimeFile`). Until the cutover is published, run the checks as `Test-Compile.ps1 -IncludeRuntimeCode -ExcludeRuntimeFile PX_Objects_CR_Contact_extensions` and `Verify-Package.ps1 -ExtraProject Contact`.
- The live CR302000 Modern UI is correct: `ContactCurrent` is the view and `ContactDetail` its class (`ContactCurrent = createSingle(ContactDetail)`); `#tabGeneral` exists. Only the Contact repo's stale `Contact_converted.*` is wrong (`#tabDetails` / `view.bind="ContactDetail"`).
- Compiled Modern UI evidence lives in `PK9\App_Data\TSCompleteScreens\customizationScreens\Company\<SCREEN>.html` (merged HTML with all extensions) and `App_Data\logs\npm-*-compile-*.txt` (exit code, warnings).
- `%TEMP%` is an 8.3 short path here (`C:\Users\ADMINI~1\…`); `Get-ChildItem` returns long names. `Build-Package.ps1 -SrcDir` now expands it with `Get-Item` (it threw "Unexpected Modern UI path" before).
- **Status ints are load-bearing (T-20):** 135 GI result columns in 73 GIs show `Usr401kStatus/UsrPGTaxStatus/UsrPGAcctStatus/UsrPGAuditStatus` (text comes from the list attribute); 62 GI conditions in 46 GIs and 6 active business events test the int values; 401k IDs are tied to task codes in GI expressions (`CInt(Left([PMTask.TaskCD],2))`). Never renumber or reuse an ID; never switch these fields to `PXSelector` (GIs would show numbers). Base patterns for a DB-backed list: `PX.Objects.AP.Box1099NumberAttribute` (PXIntList + `PXDatabase.GetSlot`) and `PX.Objects.CR.CROpportunityStagesAttribute` (active + current value only). `PXIntListAttribute` has no `FieldVerifying`.
- STATUS* attributes are used only as lookup tables (0 `CSAttributeGroup`, 0 `CSAnswers`); 6 GIs join `CSAttributeDetail` on them (GI000142/137/143/144, GI993814, GI641674). GI000144 joins `STATUSACCT` on `PMTask.UsrPGTaskStatus` (existing bug).
- Folder layout changed 2026-11-13 (old → new): `*.cs` + `Helper/` → `src/Code/`, `DBScripts/` → `scripts/`, `project.xml` + `ProjectTask*.zip` + `PKPM3001-PKPMD.xml` → `exports/`, `PKPM3002.aspx` → `src/Pages/PK/`.

## Session log
| Date | Session | Summary |
|---|---|---|
| 2026-11-13 | 1 | Extracted the live project from the PK9 DB (read-only) into `src/`; added Modern UI (PKPM3001/3002 + PM301000/PM302000 extensions) and published Classic shells; found and fixed code drift (see above); restructured on the PK-TimeClock model; added CLAUDE.md, scratchpad.md, navigation.md files, `tools/Test-ModernUI.ps1`, `.gitignore`; updated README and the PK9 navigation. Not committed. |
| 2026-11-20 | T-00 | Package tooling: `tools\Build-Package.ps1`, `Verify-Package.ps1`, `Get-LiveProject.ps1` (read-only live export), repo-local `Test-Compile.ps1`; `build/` git-ignored. Re-sync check: 28 of 34 live items equal `src/`; 2 Automation + 4 Page items missing (Open item 5). Checks: COMPILE OK (both compile scripts), TS CHECK OK. Not committed (lead commits). |
| 2026-11-20 | T-10 | Contact merge (source only, not published): added `src/Code/PKContactExt.cs`, `src/Tables/Contact.xml`, `src/PageCustomizations/CR302000.xml`, `src/FrontendSources/screens/CR/CR302000/extensions/CR302000_PKMarketing.{ts,html}` (all verbatim from live); `Verify-Package.ps1 -ExtraProject` (+ DAC item ↔ code item check); `Build-Package.ps1` short-path fix; `docs/contact-merge.md` (cutover, data, rollback, checklist); nav files + README. Checks: COMPILE OK (with `-ExcludeRuntimeFile PX_Objects_CR_Contact_extensions`), TS CHECK OK, 39 items built, `Verify-Package -ExtraProject Contact` = PACKAGE MATCHES LIVE. Cutover (import + publish with `Contact` unticked) is with the user (bundle A). Not committed (lead commits). |
| 2026-11-20 | T-20 | Design `docs/status-design.md` (table `PKProjectStatus`, DB-backed `PKProjectStatusList` attribute, PKPM2001, CSAttributeDetail sync = Option C, user decisions D1-D8). Read-only SQL + reflection/IL only; no `src/` edits. Not committed. |
