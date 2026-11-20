# PK Consolidation & Expansion — Master Plan (lead agent + worker swarm)

Created 2026-11-20. **This file is the single source of truth for the effort.** The lead agent (PM/orchestrator) keeps the
Status column current. Workers read this file, `CLAUDE.md`, `scratchpad.md` and `navigation.md` before starting.

## Repos
- `PriceKubecka-Acumatica-Contact` — merged into this repo (T-10), then archived by the user.
- `Acumatica-PK-TimeClock` — built and passing; **paused ~1 month** (until at least 2026-12-20) pending manager/partner requirements. Gap review only (T-02).
- `Acumatica-Project-Task-PK` (this repo, customization `ProjectTask`) — home for all new work.

## Locked decisions
| # | Decision |
|---|---|
| P1 | **Done = "Checks + user gate".** Worker sets `READY-FOR-PK9` when COMPILE OK, TS CHECK OK, package builds, read-only SQL checks pass. Lead sets `DONE` only after the user publishes on PK9 and ticks the task's manual checklist. Work ships in publish bundles. |
| P2 | **Tech cleanup = status + department only.** Remove `UsrPGTechStatus` and the TECH-department display. **Keep `UsrPGTechReviewer`** (the time clock uses it). Never drop DB columns. |
| P3 | **Status maintenance = one generic screen** (Department key + status grid), one table, one selector attribute, replacing the inline `PXIntList`s (Tax 15, 401k 20, Acct 8, Audit 11). |
| P4 | Plan lives here as `PLAN.md`. |
| P5 | **Model / effort per task** (column below), set by the user 2026-11-20: coding and design tasks run on Opus 5.5 at high effort; read-only investigation and doc tasks on cheaper models. The lead passes the column's values as `model` / `effort` when dispatching. |
| Hard rules | Never publish. Never edit PK9 site files (except nav `.md`). Never write to the DB without approval in the current conversation. Edit source only under `src/` in the GitHub repos. One owner per area; claim in `scratchpad.md`. Modern UI only. |

## Key findings (2026-11-20 exploration)
- **Contact repo is not a buildable project:** only TS/HTML (`CR302000_PKMarketing.ts/.html`, stale `Contact_converted.*`, saved pages). No C#, no `project.xml`. The DAC extension exists only in live `App_RuntimeCode\PX_Objects_CR_Contact_extensions.cs`: `ContactExt : PXCacheExtension<Contact>`, **8 bool fields** (`UsrPGPortalAccess`, `UsrPG1040ES`, `UsrPG1040Check`, `UsrPGTaxPlan`, `UsrPGDoNotMarket`, `UsrPGAcuMarket`, `UsrPGRetire`, `UsrServiceProviderQ`). The repo's two HTML variants disagree (`#tabGeneral`/`ContactCurrent` vs `#tabDetails`/`ContactDetail`) and the `.ts` declares fields on `ContactDetail`. **Live PK9 copy under `FrontendSources\screen\src\customizationScreens\Company\screens\CR\CR302000` is the source of truth.**
- **ProjectTask:** `src/` extracted from PK9 on 2026-11-13; no package-build script; no automated tests; `exports/` stale. Status lists are inline `PXIntList` in `PMTask.cs` / `Contract.cs`; GI000142 reads matching `CSAttributeDetail` rows (STATUSTAX/401K/ACCT/AUDI, `ValueID` "01" = the int). TimeClock keeps a copy of the task-status list (`PKTimeClockConstants.cs:124`).
- **Template flow:** no template code here. Pattern to copy: `Acumatica-PK-TimeClock\src\Code\PKProjectEntryExt.cs` (`[PXOverride] DefaultFromTemplateProjectSettings`). Base source: `Acumatica-Employee-Project-Time\reference\base-code\PX.Objects.PM.ProjectEntry.cs` — `DefaultFromTemplate` (l.5566), `DefaultFromTemplateProjectSettings` (l.5923), `CopyTask` (l.5396), **`OnDefaultFromTemplateTasksInserted(project, template, taskMap)` (l.5264, a designed extension point — likely the answer to the 2021 blocker)**. No ProjectTask extension exists on PM208000, so `UsrPG*` fields are not visible on templates today.
- **Tech:** `UsrPGTechStatus` (Contract.cs:88-94, Tables/Contract.xml:9, PM301000_PKProjectInfo.ts:39 / .html:18). `UsrPGTechReviewer` is used by TimeClock (`PKClockTask`, `PKPreparerTask`, PKEP3002) — stays.

## T-01 findings that change the plan (2026-11-20, `docs/inventory.md`)
- **T-11 is bigger than scoped** (see T-11 row): Automation + page customization + 7 GIs reference `UsrPGTechStatus`. Removing only the DAC field is not safe yet.
- **Add Common Task does NOT call `OnDefaultFromTemplateTasksInserted`.** It runs `AddTasks` → `ProjectEntry.CopyTask(...)` (26R2: `CopyTaskBase<ProjectEntry>.CopyTask`), which copies standard task props, attributes, cost/revenue budget lines, notes, files, but **not `Usr*` fields**. Candidates to cover both flows: `CopyTask` (public virtual) or `CopyTaskBase.OnCopyPasteTaskInsertedEx` — overridability from `PXGraphExtension<ProjectEntry>` is unverified; **T-30 must settle it.**
- **Common tasks = ordinary `PMTask` rows** flagged by `AutoIncludeInPrj`; global ones on PM208030 (`TemplateGlobalTaskMaint`, 1 test task `03AL` live), template tasks on PM208010/PM208000. Today all 51 templates' tasks have `AutoIncludeInPrj = 1`, so no template task is a "common" task yet. Budget lines on a common task are already copied by `AddTasks`; T-40 can choose between pre-built budget lines on the common task vs creating them in code. Existing templates use account groups 41 (Revenue) and 42 (Employee Time).
- **Correction:** TimeClock's `PM208000_PKTimeClock` already adds `UsrPKLaborItemID` to PM208000; only ProjectTask has no extension there. Templates are `BaseType='R'` (51), all `UsrPG*` blank.
- **Status lists:** 54 of 55 `CSAttributeDetail` rows match the `PXIntList`s. Differences: ACCT 02 text ("Initial Review (Acctg)" vs "Initial Review"), DB-only ACCT 00 "Not Started", case-only differences (AUDIT 01, 401K 05). No `STATUSTECH` attribute. `SortOrder` NULL everywhere. T-20 must decide which text wins.
- **Orphan DB columns without a DAC field in `src/`:** `Contract.UsrPGReleasedFinalDeliverables`, `PMTask.UsrPGAccountingStatus`, `PMTask.UsrPGTaxStatus`.
- Not verifiable by T-01: custom `.rpx` report contents; Power BI consumers outside the DB.

## Orchestration protocol
1. Task statuses: `TODO` → `CLAIMED` → `READY-FOR-PK9` → `DONE` (or `BLOCKED`).
2. Lead dispatches only tasks whose dependencies are met. Max ~3 workers at once; no two workers share a file or area. Worker branch: `agent/<task-id>`; lead merges to `main`.
3. **Worker contract:** read CLAUDE.md / scratchpad / navigation → claim the area in `scratchpad.md` → implement under `src/` only → run checks → record learnings in the scratchpad → commit (message ends with the Co-Authored-By trailer) → release claim → report: files changed, check output, manual checklist for the user.
4. **Lead re-runs the checks itself** before changing a status; never trusts a worker's "OK". Lead maintains the Publish bundles list below.
5. **Checks (this repo's own tools; the TimeClock `Test-Compile.ps1` only compiles the TimeClock repo's code):** `tools\Test-Compile.ps1 -IncludeRuntimeCode` → `COMPILE OK`; `tools\Test-ModernUI.ps1` → `TS CHECK OK` (UI changes); `tools\Build-Package.ps1` then `tools\Verify-Package.ps1` (live match check; expect `PACKAGE MATCHES LIVE` only before any change, after a change expect only the intended differences); read-only SQL via `..\Acumatica-PK-TimeClock\tools\Query-PK9.ps1`. `tools\Get-LiveProject.ps1` refreshes `build\live\`.
6. A worker never edits another repo; cross-repo needs become a new task or a note.

## Tasks

### Phase 0 — Foundations
| ID | Task | Area | Depends | Acceptance | Model / effort | Status |
|---|---|---|---|---|---|---|
| T-00 | **Package tooling.** `tools/Build-Package.ps1` (model on TimeClock's) → `build/ProjectTask.zip` from `src/`. Re-sync `src/` from live PK9 (read-only), confirm no drift. Untrack `.vs` / `.DS_Store`. Ask the user before committing the stray `CLAUDE.md` / `GUIDE.md` changes. | tools, docs | — | Zip items == `CustObject` fingerprint of live `ProjectTask`. **Result 2026-11-20:** tools built; `src/` was missing 6 live items (Automation#PM301000, Automation#PKPM3001, 4 Classic Page items), now imported from `build/live/src` into `src/Automation` and `src/PageCustomizations`; lead re-ran Test-Compile, Test-ModernUI, Build-Package, Verify-Package: COMPILE OK, TS CHECK OK, 34/34 items, `PACKAGE MATCHES LIVE`. | Opus 5.5 / high | READY-FOR-PK9 (tooling only; nothing to publish yet) |
| T-01 | **Inventory (read-only).** (a) every use of `UsrPGTechStatus` / TECH dept in both repos, GIs, reports, site map + live data counts; (b) custom fields on project and task, and which belong on templates; (c) how **Common Tasks** are stored and how Add Common Task works in 26R2 (graph, DAC, screen — confirm with `Inspect-Acumatica.ps1` / base source, no guessing); (d) CSAttributeDetail rows for STATUSTAX/401K/ACCT/AUDI → `docs/status-lists.md`. | docs | — | `docs/inventory.md`; open questions for later tasks answered or listed. | Sonnet 5.5 / medium | READY (docs: user to review; 17 open questions Q1-Q17 in `docs/inventory.md`) |
| T-02 | **Time clock gap review (read-only).** From the TimeClock scratchpad: built vs open (Phase 3, Phase 4, backlog, open items 1–8, unticked tests). One page: done & passing / waiting on user / waiting on managers & partners / optional. Add re-entry checklist to its scratchpad (PAUSED banner already added). No code. | TimeClock docs | — | `docs/status-overview.md` in the TimeClock repo + scratchpad checklist. | Sonnet 5.5 / medium | READY (docs: user to review) |

### Phase 1 — Quick wins
| ID | Task | Area | Depends | Acceptance | Model / effort | Status |
|---|---|---|---|---|---|---|
| T-10 | **Merge Contact.** Extract the 8-field `ContactExt` from live runtime code into `src/Code/PKContactExt.cs` (unique name, not `PX_Objects_CR_Contact_extensions`); add `src/Tables/Contact.xml`; take CR302000 UI from the **live PK9 screens folder**, fix the view/tab mismatch, place under `src/FrontendSources/screens/CR/CR302000/`. **Cutover:** the `Contact` project is unpublished in the same publish; both must never be published together (same pattern as ProjectTime). | code, tables, CR302000 UI | T-00 | COMPILE OK, TS CHECK OK; fields == live DAC; no duplicate item names. Manual: Contact screen shows tab + 8 fields, saves, old data intact. User then archives the Contact repo. **Result 2026-11-20:** live `Contact` project (5 items: DAC item, Table, Classic page, 2 Modern UI files) fully in `src/` (`PKContactExt.cs`, `Tables/Contact.xml`, `PageCustomizations/CR302000.xml`, CR302000 `.ts/.html`, unchanged from live, no binding mismatch found); lead re-ran: COMPILE OK (with `-ExcludeRuntimeFile PX_Objects_CR_Contact_extensions` until cutover), TS CHECK OK, package 39 items, `Verify-Package -ExtraProject Contact` = `PACKAGE MATCHES LIVE`. Cutover steps + checklist: `docs/contact-merge.md`. | Opus 5.5 / high | READY-FOR-PK9 (bundle A) |
| T-11 | **Remove Tech Status + TECH dept display.** Remove `UsrPGTechStatus` from `Contract.cs`, `Tables/Contract.xml`, `PM301000_PKProjectInfo.ts/.html`, and the TECH-department branch. Keep `UsrPGTechReviewer`. Update docs. **Per T-01, also needed first or in the same bundle:** (1) the live `Automation#PM301000` (has the "Department <> Tech" rule) and Classic page customization `pm301000.aspx` reference the field; **both are now in `src/Automation` and `src/PageCustomizations` (T-00)**, so T-11 must edit them; (2) **7 Generic Inquiries use `usrPGTechStatus`** (GI000140, GI641660, GI641645 [WHERE `= 4`], GI641709, plus 3 with no ScreenID incl. 'PK Open Project'), 4 are OData/Power BI exposed: the **user** must repoint/clean these before the DAC field is removed. Live data: 1 project has a value (4). | code, PM301000 UI, Automation, page customization | T-00, T-01 | Compile/TS OK; grep: no `TechStatus`. Manual: TECH-dept project on PM301000 has no Tech status. **Result 2026-11-20:** removed from `Contract.cs`, `Automation/PM301000.xml` (field row + the "Department <> Tech" condition, used only by that row), `PageCustomizations/PM301000.xml`, PM301000 `.html/.ts`; `Tables/Contract.xml` column declaration deliberately KEPT (removing it does not drop the column; keeps the project owning it on every environment; rollback is code-only). Lead re-ran: COMPILE OK, TS CHECK OK, 39 items; Verify differs from live in exactly the 5 intended items. Cutover/checklist: `docs/tech-removal.md`. **Publish blocked until GI641645's `usrPGTechStatus = 4` filter is removed (PK9 and live).** | Opus 5.5 / high | READY-FOR-PK9 (bundle A; blocked on GI641645 filter) |

### Phase 2 — Status maintenance (P3)
| ID | Task | Area | Depends | Acceptance | Model / effort | Status |
|---|---|---|---|---|---|---|
| T-20 | **Design.** Table `PKProjectStatus` (`DepartmentID`, `StatusID` int, `Description`, `SortOrder`, `Active`), preserving existing ints 1:1. Selector attribute filtered by the project's department. Decide: keep `CSAttributeDetail` in sync (GI000142 reads it) vs repoint the GI. Retired statuses go inactive, never deleted. → `docs/status-design.md`. | docs | T-01 | User signs off. **Result 2026-11-20:** design written; recommends a DB-backed `PKProjectStatusListAttribute : PXIntListAttribute` (not a `PXSelector`, so GIs keep showing text), one-way sync into `CSAttributeDetail` (hybrid GI option C), screen PKPM2001 under Projects → Preferences. **Waiting on user decisions D1–D8** (see the doc's last section) before T-21. | Opus 5.5 / high | READY (design: awaiting user sign-off) |
| T-21 | **Maintenance screen** `PKPM2001` (Department key + status grid): graph, DAC, site map, ASPX shell (follow PKPM3002), Modern UI. Seed SQL with today's lists — **written, not run**. | new screen | T-20 | Compile/TS OK; seed diffed against the current `PXIntList` values, identical. | Opus 5.5 / high | TODO |
| T-22 | **Swap the four project fields** (`Usr401kStatus`, `UsrPGTaxStatus`, `UsrPGAcctStatus`, `UsrPGAuditStatus`) from `PXIntList` to the new selector, same columns/types. **Ask the user first** whether `UsrPGTaskStatus` (1–9, company-wide) is included. | `Contract.cs`, PM301000 UI | T-21 | Existing data shows the same text on PM301000; GI000142 unchanged. | Opus 5.5 / high | TODO |
| T-23 | TimeClock `PKTaskStatusListAttribute` copy stays in sync (only if task status is touched). Cross-repo; do after TimeClock resumes. | TimeClock | T-22 | — | Opus 5.5 / high | TODO |

### Phase 3 — Template → project flow
| ID | Task | Area | Depends | Acceptance | Model / effort | Status |
|---|---|---|---|---|---|---|
| T-30 | **Spike on a throwaway branch.** Prove `DefaultFromTemplateProjectSettings` + `OnDefaultFromTemplateTasksInserted` carry a `UsrPG*` project field and a task field (`UsrPGDueDate`) from template to new project; learn how `taskMap` maps rows; settle due-date semantics (copy as-is vs offset) with the user. | ProjectEntry ext | T-01 | `docs/template-flow.md`. | Opus 5.5 / high | TODO |
| T-31 | **Template screens.** Show all project `UsrPG*` fields on templates (PM208000 extension) and task fields on template tasks. | UI | T-30 | TS OK; fields visible. | Opus 5.5 / high | TODO |
| T-32 | **Copy logic.** `PKProjectTemplateExt : PXGraphExtension<ProjectEntry>` with `[PXOverride]`s; copy only when the target is null; coexists with TimeClock's `PKProjectEntryExt`. | code | T-30, T-31 | Compile OK. Manual: new project from a template gets department, priority etc. and task due dates. | Opus 5.5 / high | TODO |

### Phase 4 — Common tasks (design with the user first)
| ID | Task | Area | Depends | Acceptance | Model / effort | Status |
|---|---|---|---|---|---|---|
| T-40 | **Design session.** Non-stock items + custom fields (e.g. due date) on common tasks; on Add Common Task, auto-create a cost budget line and a revenue budget line from the task's labor item. Decide item type, quantity/rate/amount source, account groups, blank labor item, duplicates, template inheritance. | docs | T-01, T-30 | `docs/common-tasks-design.md` approved. | Opus 5.5 / high | TODO |
| T-41..43 | Fields + UI → add-action hook creating the two budget lines → edge-case tests. Split further after T-40. | TBD | T-40 | — | Opus 5.5 / high | TODO |

### Phase 5 — Closeout
| ID | Task | Depends | Model / effort | Status |
|---|---|---|---|---|
| T-90 | Re-extract `src/` from PK9 after each publish; update `navigation.md`, `scratchpad.md`, session log. | each bundle | Sonnet 5.5 / medium | TODO |
| T-91 | Update PK9 `navigation.md` repo table (Contact merged, TimeClock paused). | T-10 | Haiku 5.5 / low | TODO |

## Publish bundles (user-owned)
1. **A:** T-00 package, T-10 (Contact merge + unpublish `Contact`), T-11.
2. **B:** T-20–T-22 (status screen; user runs the seed).
3. **C:** T-31, T-32 (templates).
4. **D:** common tasks.
Per bundle: backup → import zip → publish with the superseded project unticked → manual checklist → T-90.

## Risks
- A field declared in two published projects fails: unpublish the superseded project in the same publish.
- The status migration must keep integer meanings exact: the seed diff test is mandatory before any selector swap.
- `PKPM3003.aspx` is PKPM3001's shell: never reuse that number.
- TimeClock keeps copies of ProjectTask constants; a status change needs a paired TimeClock task.
- Live is Acumatica-hosted with no SQL access: data changes go through import scenarios or customization SQL.

## Lead log
- 2026-11-20: plan created; Model / effort column added (P5). Next: dispatch T-00, T-01, T-02 in parallel.
- 2026-11-20: T-00, T-01, T-02, T-10, T-20 done/committed (T-10 and T-00 re-verified by the lead). **T-11 GI prerequisite, PK9 state (read-only check):** user removed the GIs that had screen IDs, except **GI641645 PowerBI - Project Revenue still has the filter `PMProject.usrPGTechStatus = 4`** (GIWhere line 8). Three unpublished GIs still use the field and are to be deleted by the user in the UI (SM208000): `Preparer Time Sapro` (b8f7f6c8-321e-4176-b27c-7af26a722c09), `PK - Preparer Time Sapro Manager` (50e5406b-98db-4ee0-822c-ed449965611b), `PK Open Project` (8e31422e-a5bc-41a9-b755-bcbccda7933b). No SiteMap rows, no OData, nothing else references them. The same GI cleanup must be repeated on **live** (hosted, UI only). Awaiting user decisions D1–D8 (`docs/status-design.md`) before T-21.
