# PK Consolidation & Expansion — Master Plan (lead agent + worker swarm)

Created 2026-11-20. **Fully rewritten 2026-11-21 as a session handoff** (the lead session was reset; this file is all a new
lead needs). **This file is the single source of truth for the effort.** The lead agent (PM/orchestrator) keeps it current.
Workers read this file, `CLAUDE.md`, `scratchpad.md` and `navigation.md` before starting.

---------------------------------------------------------------------------------------------------------------------

## 0. READ THIS FIRST (new lead session)

**Who you are:** the lead/PM agent. You dispatch worker subagents (Agent tool), re-verify their work yourself, commit,
and keep this file current. **You never publish, never write to the PK9 database, never touch live.** The user publishes.

**First 10 minutes:**
1. Read this file, then `CLAUDE.md` and `scratchpad.md` in this repo, and `C:\Program Files\Acumatica ERP\PK9\navigation.md`.
2. `git log --oneline -15` and `git status --short` here (git path in §9). Expect `main` at or after `de8bb8c`; only
   `CLAUDE.md` (modified) and `GUIDE.md` (untracked) uncommitted: those are the user's.
3. Re-check live state with the read-only queries in §9 (is the GI641645 filter gone? did the user report test results?).
4. Ask the user for the results of the PK9 checklists (§2) before changing any Status to DONE.

**Where things stand in one paragraph:** All planned build work through T-32 is finished, verified and committed. The user
**published the 57-item `ProjectTask` package to PK9 on 2026-11-21 at ~02:51** (I verified read-only: npm build exit code 0;
`PKProjectStatus` table created and seeded with 54 rows; menu entry `PKPM2001` exists; new runtime code files present;
the old `Contact` project's runtime file is gone and `PKContactExt.cs` is live; Tech Status screen rules are gone;
667 contacts still carry marketing flags). **Not yet confirmed by the user:** the manual checklists, PKPM2001 access rights,
the "save once per department" step, and the GI641645 filter (still present on PK9 at 2026-11-21 — see §2). **Live has
not been touched.** What is left: the user's test results → T-90 re-sync → live cutover → T-91 → the common-task design
session (T-40) and its build, plus optional TimeClock cleanup (T-23).

---------------------------------------------------------------------------------------------------------------------

## 1. NEEDS THE USER (lead keeps this current; state 2026-11-21)
1. **Report the PK9 test results.** Run the checklists and tell the lead pass/fail per item:
   `docs/contact-merge.md`, `docs/tech-removal.md`, `docs/status-screen.md`, `docs/status-switch.md`,
   `docs/template-screens.md`, `docs/template-copy.md` §7.
2. **PKPM2001 access rights (SM201020):** `*` revoked, Delete for Administrator, PK - Admin, PK - Partner (+ any managers).
   Not part of the package.
3. **Open PKPM2001 and Save once per department** (401K, TAX, ACCT, AUDIT). This syncs the new table into the
   `CSAttributeDetail` rows so GI000142 and the time clock dashboard show the approved text (3 rows change: ACCT 2, AUDIT 1,
   401K 5). If Modern UI will not enable Save without a change, see the workaround in `docs/status-screen.md`.
4. **GI641645 "PowerBI - Project Revenue" still has the filter `PMProject.usrPGTechStatus = 4`** (Conditions line 8, inside an
   inactive bracket group; deleting the one line keeps the brackets balanced). Tech Status is now gone from the DAC on PK9,
   so this GI may error or return blanks (Power BI refresh!). Remove it on PK9 **and** on live (UI only on live).
5. **Due-date rule for template tasks** (agent default = option C, shipped, not yet confirmed): keep the template date's
   month/day and use its first occurrence on or after the anchor date. **Data caveat: 589 of 8,338 real task due dates fall
   before their project's start date (late-opened projects); under C those roll to NEXT year.** Choose: keep C / add a grace
   window / copy as-is (one-line switch, `PKTemplateDueDate.ActiveRule`). Also confirm: no initial statuses on new projects
   (current behavior), Copy Project / Create Template do NOT carry `UsrPG*` (current behavior).
6. **Do NOT go to live until PK9 passes.** The live cutover is the same zip with the same steps (§5, T-92).
7. **T-40 common-task design session** (a conversation, §7 Phase 4) whenever the user is ready.
8. Housekeeping the user owns: `CLAUDE.md` (modified, rule 3 still names the TimeClock `Test-Compile`; the repo-local
   `tools\Test-Compile.ps1` is the right one) and `GUIDE.md` (untracked) are uncommitted; archive the old Contact repo
   after PK9 passes; optionally confirm whether PK9 shares live's Azure blob file storage (T-00 saw writing enabled with the
   same settings in every company; I did not touch it).
9. Employee profile moving from Tech to Admin (user's own data change): no plan impact; may matter later for the
   time clock "My department" view.

---------------------------------------------------------------------------------------------------------------------

## 2. Current state (2026-11-21), verified by the lead read-only

| Item | State |
|---|---|
| Package | `build\ProjectTask.zip`, 57 items (Code 14, Sql 3, Table 3, SiteMapNode 3, Automation 2, Page 5, File 6, PerTenantFile 21), built from `main` @ `de8bb8c` |
| PK9 publish | Done by the user ~2026-11-21 02:51. npm compile exit code `0`; build covered CR302000, PKPM2001, PM208000/PM208010/PM208030, PM301000, PM302000, PKPM3001/3002, PKEP3001/3002, EP307000, PM101000. The only warnings are base-screen "element has no id" lines (not ours) |
| `PKProjectStatus` table | Exists; CompanyID 2: 401K 20, TAX 15, ACCT 8, AUDIT 11 (= 54, the approved seed) |
| Site map | `PKPM2001 Department Statuses` present |
| Contact merge | Old runtime file `PX_Objects_CR_Contact_extensions.cs` gone (project unpublished); `PKContactExt.cs` live; 667 contacts with any marketing flag (data intact). The `Contact` project still exists in the DB (5 items): the user may delete it after PK9 passes (never publish both) |
| Tech removal | `AUScreenFieldState` rows for `TechStatus` = 0 (Automation applied). GIResult refs = 0. **GIWhere refs = 1 (GI641645)** |
| ProjectTask project in DB | 57 items; PKTimeClock 46 items (unchanged) |
| Not verified by the lead | Anything needing a UI: the new screen's behavior, GI/OData labels, PM301000 dropdown rendering, template screens, template copy runtime, Copy Project with a retired status |
| Live | Untouched. Hosted by Acumatica; no SQL access; GIs only editable in the UI |

---------------------------------------------------------------------------------------------------------------------

## 3. Repos and git state
- `Acumatica-Project-Task-PK` (this repo, customization `ProjectTask`, branch `main`, remote `origin`; local commits are not pushed by agents):
  `4bbde0f` T-00 · `485bb5f` T-01 · `7979bb1` T-20 · `60d537c` T-10 · `9307996` T-11 · `5a3eed8` T-30 doc · `f035077`/`e448fd9` plan updates ·
  `60dd8ac` T-21 · `1945dd9` T-31 · `dd9a58b` T-22 · `de8bb8c` T-32. Uncommitted: `CLAUDE.md` (M), `GUIDE.md` (??) — the user's.
- Branch `agent/T-30` + worktree `..\Acumatica-Project-Task-PK-T30` (@ `00a1385`): the spike prototype, **superseded by T-32 in
  main**. Safe to remove (`git worktree remove`, `git branch -D agent/T-30`) once the user is comfortable; nothing else uses it.
- `Acumatica-PK-TimeClock` (branch `main` @ `e1025d9`; **paused until at least 2026-12-20**): T-02 added `docs/status-overview.md`,
  a PAUSED banner and a re-entry checklist to its `scratchpad.md`. Its commit subject has a stray invisible BOM (cosmetic).
  `CLAUDE.md` (M) and `GUIDE.md` (??) uncommitted there too (the user's).
- `PriceKubecka-Acumatica-Contact`: merged into this repo; to be archived by the user. Not modified by agents.
- `Acumatica-Employee-Project-Time`: legacy; used read-only for base source in `reference\base-code\`.

---------------------------------------------------------------------------------------------------------------------

## 4. Locked decisions
| # | Decision |
|---|---|
| P1 | **Done = "Checks + user gate".** A worker's task is `READY-FOR-PK9` when COMPILE OK, TS CHECK OK, package builds, read-only SQL checks pass. The lead sets `DONE` only after the user publishes AND ticks the manual checklist. |
| P2 | **Tech cleanup = status + department display only.** `UsrPGTechStatus` removed; **`UsrPGTechReviewer` stays** (time clock uses it); DB columns are never dropped (the `Tables/Contract.xml` declaration for the Tech Status column is deliberately kept). |
| P3 | **Status maintenance = one generic screen** (PKPM2001, Department key + status grid) replacing the inline `PXIntList`s. |
| P4 | Plan lives here as `PLAN.md`. |
| P5 | Model/effort per task: coding/design on **Opus 5.5 at high effort**; read-only investigation/doc work on Sonnet; trivial doc edits on Haiku. Pass as `model` / `effort` on the Agent call (effort only because the user asked for it). |
| D1–D8 (user approved 2026-11-20, all recommendations) | D1 text for ACCT 2 / AUDIT 1 / 401K 5 = the in-code (`PXIntList`) text. D2 ACCT 0 "Not Started" not seeded. D3 keep the 401K 17 double space. D4 GIs: keep `CSAttributeDetail` in sync one-way on every PKPM2001 save (hybrid), repoint GIs at the user's pace; also fix the GI000144 join (it looks up Acct text using the *task* status). D5 task status (`UsrPGTaskStatus`) stays as is. D6 menu = Projects → Preferences → "Department Statuses". D7 access rights set by the user on SM201020. D8 saved statuses are never deleted, only deactivated; IDs read-only after save. Plain meaning: a project stores the status **number**; GIs and business events test the number; to change what "01" means, rename it. A "delete only if unused" variant was offered and not chosen |
| Hard rules | Never publish. Never edit PK9 site files (except nav `.md`). Never write to the PK9 DB without explicit user approval in the *current* conversation (the user chose UI deletion for GIs). Edit source only under `src/` in the GitHub repos. One owner per area; claims in `scratchpad.md`. Modern UI only. Never run `-AllowWrite`/`-Execute` on `Query-PK9.ps1`. |

---------------------------------------------------------------------------------------------------------------------

## 5. Publish procedure (user-owned) and bundle history
**PK9 (done 2026-11-21 02:51):** one zip carried everything verified at that time (former bundles A+B+C merged, because an import
replaces all items of the project, so partial bundles would require rebuilding from older commits): T-00 tooling, T-10 Contact
merge, T-11 Tech removal, T-21 status screen, T-22 field switch, T-31 template screens, T-32 copy logic.

**Standard steps (also for live):** back up → import `build\ProjectTask.zip` into `ProjectTask` (replaces its items) → in SM204505 tick
`ProjectTask`, **untick `Contact`** (and keep everything else that was published ticked, e.g. PKTimeClock) → publish once → set rights
for new screens on SM201020 → run the checklists → tell the lead. If the HTML build fails, read the newest
`App_Data\logs\npm-*` (exit-code file `0` = OK) and send it to the lead.

**Live cutover (T-92, user, after PK9 passes):** same zip (rebuilt from `main` by the lead; verify with `Verify-Package`). Before it,
remove the Tech GI references in the live UI (GI641645 filter etc., §1.4); remember live's tenant ID is unverified, so check
PKPM2001 shows 54 rows right after publishing; Contact must be unticked. **The live project fingerprint may differ from PK9** (live
cannot be queried): the lead cannot run `Verify-Package` against live; use a diff of exported project items if the user provides one.

---------------------------------------------------------------------------------------------------------------------

## 6. Orchestration protocol (how the swarm works)
1. Task statuses: `TODO` → `CLAIMED` → `READY-FOR-PK9` → `PUBLISHED-ON-PK9` → `DONE` (or `BLOCKED`). Docs/design tasks use `READY (docs: user to review)`.
2. Lead dispatches only tasks whose dependencies are met. Max ~3 workers at once; **no two workers share a file or area** (tell each worker
   which files the others own; `scratchpad.md` and `navigation.md` are shared: workers re-read right before a small append-only edit).
3. **Worker contract (put in every prompt):** read PLAN/CLAUDE/scratchpad/navigation → claim the area in `scratchpad.md` → implement under `src/`
   only → run the checks → record learnings + session-log row → **do NOT run git add/commit/checkout/reset** (the lead commits) → release the
   claim → final report: files changed, exact check output, what is unverifiable before a publish, manual checklist. Hard rules as in §4.
4. **Lead re-runs every check itself** before changing a status; never trust a worker's "OK" or its description of the user's wishes (a T-32 worker
   wrote "as you chose" about option C, which the user had not chosen). Check at least: the Compile/TS/Build/Verify/seed/date tests, the key
   claims (e.g. a method signature via `Inspect-Acumatica.ps1`), and read the generated HTML.
5. **Commit as the lead, by path** while workers are running (`git add -- <paths>` then `git commit -F msg -- <paths>`), so unfinished
   work of a running worker is not swept in. Commit message ends with `Co-Authored-By: Claude Sonnet 5.5 <noreply@anthropic.com>` and
   `Claude-Session: <session url>`; write the message to a file with `[IO.File]::WriteAllText(..., UTF8Encoding($false))` (PS 5.1 `Set-Content -Encoding utf8` adds a BOM).
6. Isolation: Agent `isolation: "worktree"` does NOT work in this session (the working directory, the PK9 site, is not a git repo).
   Create a worktree yourself (`git worktree add <path> -b agent/<id>`) and point the worker at that path (done for T-30). Parallel compile
   checks should use a private `$env:TEMP`.
7. Cross-repo rule: a worker never edits another repo; needs become a note or a task. TimeClock is paused.
8. Keep this file current: after every worker, update the task row (result + lead's verification), §1, §2 and the lead log (§12).

---------------------------------------------------------------------------------------------------------------------

## 7. Tasks

### Phase 0 — Foundations
| ID | Task | Result / acceptance | Model / effort | Status |
|---|---|---|---|---|
| T-00 | Package tooling: `tools/Build-Package.ps1`, `Verify-Package.ps1`, `Get-LiveProject.ps1`, repo-local `Test-Compile.ps1`; re-sync `src/` with live; untrack `.vs`/`.DS_Store` | `src/` was missing 6 live items (Automation x2, Classic pages x4): imported. Package matched live 34/34 (lead re-ran). Commit `4bbde0f` | Opus 5.5 / high | PUBLISHED-ON-PK9 (tooling; nothing to publish itself) |
| T-01 | Inventory (read-only) | `docs/inventory.md` (17 open questions Q1–Q17), `docs/status-lists.md`. Commit `485bb5f` | Sonnet 5.5 | READY (docs: user to review) |
| T-02 | Time clock gap review | `docs/status-overview.md` in the TimeClock repo + PAUSED banner + re-entry checklist. TimeClock commit `e1025d9` | Sonnet 5.5 | READY (docs: user to review) |

### Phase 1 — Quick wins
| ID | Task | Result / acceptance | Model / effort | Status |
|---|---|---|---|---|
| T-10 | Merge the Contact customization | Live `Contact` project (5 items: DAC item, Table, Classic page, 2 Modern UI files) now in `src/` (`PKContactExt.cs`, `Tables/Contact.xml`, `PageCustomizations/CR302000.xml`, CR302000 `.ts/.html` unchanged from live: the repo's "binding mismatch" only existed in a stale variant). Fields: **8** bool, not ~6. Cutover: `docs/contact-merge.md`. Commit `60d537c` | Opus 5.5 / high | PUBLISHED-ON-PK9 (awaiting user checklist; user archives the Contact repo after) |
| T-11 | Remove Tech Status + TECH display | Removed from `Contract.cs`, `Automation/PM301000.xml`, `PageCustomizations/PM301000.xml`, PM301000 UI. Table column declaration kept (does not drop the column). Doc `docs/tech-removal.md`. Commit `9307996`. **Open: GI641645 filter (§1.4)**; live GIs | Opus 5.5 / high | PUBLISHED-ON-PK9 (blocked from DONE by the GI641645 filter) |

### Phase 2 — Status maintenance (P3)
| ID | Task | Result / acceptance | Model / effort | Status |
|---|---|---|---|---|
| T-20 | Design | `docs/status-design.md`; user approved D1–D8. Commit `7979bb1` | Opus 5.5 / high | DONE |
| T-21 | Screen PKPM2001 | DAC `PKProjectStatus`, graph `PKProjectStatusMaint` (rules: ID > 0/unique/read-only once saved, description unique case-insensitive, saved rows undeletable; syncs to `CSAttributeDetail` in `Persist`), DB-backed `PKProjectStatusListAttribute : PXIntListAttribute`, `Sql/PKProjectStatus.sql` (idempotent seed, 54 rows, all companies with the STATUSTAX attribute), site map, ASPX shell, Modern UI, `tools/Test-StatusSeed.ps1`, `docs/status-screen.md`. Commit `60dd8ac` | Opus 5.5 / high | PUBLISHED-ON-PK9 (table + menu verified; screen behavior awaiting user checklist) |
| T-22 | Switch the four project fields | `Usr401kStatus`/`UsrPGTaxStatus`/`UsrPGAcctStatus`/`UsrPGAuditStatus` use `[PKProjectStatusList("401K"/"TAX"/"ACCT"/"AUDIT")]`. Fixed a real bug: Copy Project would have failed for a retired status (attribute skips verification for copies). Behavior change: importing a NEW project with a retired status is rejected. `docs/status-switch.md`. Commit `dd9a58b` | Opus 5.5 / high | PUBLISHED-ON-PK9 (awaiting user checklist) |
| T-23 | TimeClock reads status text from `PKProjectStatus` instead of `CSAttributeDetail` (optional; needed before the sync can ever be retired; also `PKTaskStatusListAttribute` copy) | Cross-repo; only after TimeClock resumes (≥ 2026-12-20). No change is needed for things to work now | Opus 5.5 / high | TODO (paused) |

### Phase 3 — Template → project flow
| ID | Task | Result / acceptance | Model / effort | Status |
|---|---|---|---|---|
| T-30 | Spike | `docs/template-flow.md`. **One override covers both flows:** `[PXOverride] ProjectEntry.OnCopyPasteTaskInserted(PMProject target, PMTask newItem, PMTask sourceItem)` (protected virtual; verified in the 26.200.0334 DLL by the lead). `OnDefaultFromTemplateTasksInserted` alone would miss Add Common Task. Coexists with TimeClock's `PKProjectEntryExt`. Doc commit `5a3eed8`; prototype on branch `agent/T-30` (superseded) | Opus 5.5 / high | DONE |
| T-31 | Template screens | New extensions on PM208000 (fieldset "PK Project Info" on `ProjectProperties`: Department, Priority, Complexity; Tasks grid: Due Date, Preparer, Reviewer, Tech Reviewer), PM208010 + PM208030 (fieldset "PK Task Info" on `TaskProperties`). `docs/template-screens.md`. Commit `1945dd9` | Opus 5.5 / high | PUBLISHED-ON-PK9 (HTML compiled OK; awaiting user checklist) |
| T-32 | Copy logic | `PKProjectTemplateExt` (project fields via `DefaultFromTemplateProjectSettings`; task fields via `OnCopyPasteTaskInserted`; copy only when the target is empty; skips Copy Project/Create Template) + `PKTemplateDueDate` (pure date logic) + **`tools/Test-TemplateDates.ps1` (36 cases, runs the real code: the first automated test)**; bad template values become a warning (a prototype bug would have blocked creation). `docs/template-copy.md` §7 = PK9 test steps. Commit `de8bb8c`. Due-date option C is the agent's default, **not confirmed by the user** | Opus 5.5 / high | PUBLISHED-ON-PK9 (awaiting user checklist + due-date decision) |

### Phase 4 — Common tasks (design with the user first)
| ID | Task | Notes | Model / effort | Status |
|---|---|---|---|---|
| T-40 | **Design session with the user** | Requirement (user, 2026-11-20): common CPA tasks will be loaded into Common Tasks; they need non-stock items attached and a few custom fields (e.g. due date); on Add Common Task, create a cost budget line and a revenue budget line from the task and its labor item. Facts from T-01/T-30 to start from: a common task is an ordinary `PMTask` row (flag `AutoIncludeInPrj`; global ones on PM208030 under ContractID 0, template ones on PM208010/PM208000); Add Common Task = `AddTasks` → `ProjectEntry.CopyTask` → `CopyTaskBase<ProjectEntry>.CopyTask`, which already copies the common task's budget lines (cost + revenue), attributes, notes and files; hooks `OnCopyPasteCostBudgetInsertedEx` / `OnCopyPasteRevenueBudgetInsertedEx` and the T-32 `OnCopyPasteTaskInserted` override exist; existing templates use account groups 41 (Revenue) and 42 (Employee Time), budget level I on 44 templates and T on 7; PK9 today has 1 global test task (`03AL`) and all template tasks are `AutoIncludeInPrj = 1`. **Questions for the user:** which item type/which items; where quantity, rate and amount come from; which account groups; behavior when the labor item is blank; duplicates; whether to pre-build budget lines on the common task (cheapest: `AddTasks` already copies them) or generate them in code; inheritance from templates | Opus 5.5 / high | TODO (needs the user) |
| T-41..43 | Fields + UI → hook creating the two budget lines → edge-case tests | Split after T-40 | Opus 5.5 / high | TODO |

### Phase 5 — Closeout
| ID | Task | Depends | Model / effort | Status |
|---|---|---|---|---|
| T-90 | After the user confirms PK9: re-extract `src/` from PK9 (`tools\Get-LiveProject.ps1 -OutDir <repo>\src` only when asked; otherwise compare), expect plain `Verify-Package` (no `-ExtraProject`) to report `PACKAGE MATCHES LIVE`; drop the `-ExcludeRuntimeFile PX_Objects_CR_Contact_extensions` workaround from the docs/tools notes if no longer needed; update `navigation.md`, `scratchpad.md`, session log | user results | Sonnet 5.5 / medium | TODO |
| T-91 | Update PK9 `navigation.md` repo table (Contact merged and retired; TimeClock paused; ProjectTask now has Contact fields, status screen, template flow). Nav `.md` edits are allowed | T-90 | Haiku 5.5 / low | TODO |
| T-92 | Live cutover (user publishes; lead prepares): final zip, pre-flight list (GIs cleaned in the live UI, backup, Contact unticked, PKPM2001 rights, check seed company), post-publish checklist | PK9 passes | Sonnet 5.5 / medium (runbook) | TODO |

---------------------------------------------------------------------------------------------------------------------

## 8. Key technical findings (consolidated; details in `docs/`)
- **Contact:** the real DAC was a "DAC item" (`DAC / PX.Objects.CR.Contact`) in the live Contact project, not a Code item; Acumatica generates `App_RuntimeCode\PX_Objects_CR_Contact_extensions.cs` from it. It is now a Code item (`PKContactExt.cs`); both define `ContactExt`, so publishing both fails with CS0101 (hence the untick rule).
- **Importing a zip replaces every item in the project.** A package built from an incomplete `src/` silently deletes the rest: always run `Build-Package` + `Verify-Package` first.
- **`Verify-Package` semantics:** exact SHA of `CustObject.Content` cannot match (Acumatica rewrites XML); "match" = same items, attributes and normalized bodies. After this publish, a plain `Verify-Package` (no `-ExtraProject Contact`) should match PK9 once `src/` equals what was published (T-90).
- **Automation/Page items:** `Automation#PM301000` holds the department-based hide/disable rules for the status fields; the Classic `pm301000.aspx` page customization also referenced the Tech field. Removing a Tech row required editing both.
- **Template/common-task copy:** see T-30/T-32 rows. Anchor for the due-date rule: creating from a template sets `prj.StartDate = BusinessDate` first, so the anchor is the business date; Add Common Task uses the project's own Start Date (an old project gets a date in its start year). Feb 29 → Feb 28 in non-leap years. 11,385 of 11,407 projects come from templates; all 51 templates and their tasks had blank `UsrPG*` values.
- **Status lists:** 54 of 55 `CSAttributeDetail` rows matched the old `PXIntList`s; 62 GI conditions in 46 GIs and 6 active business events test the integers, so IDs are frozen. GI000144's Acct text lookup uses the task status (existing bug). The new list attribute shows active statuses plus the row's current value; with an empty department table it loads an empty list (no fallback to old texts) — hence the seed must run at publish.
- **Tech GIs (PK9):** the 3 unpublished GIs (`Preparer Time Sapro`, `PK - Preparer Time Sapro Manager`, `PK Open Project`) were deleted by the user in the UI; GI641645's filter remains. Four more GIs branch on `UsrPGDepartment='TECH'` harmlessly. One project (`2022ACUMATICA LGORGANICS.001`, dept TECH) holds Tech Status value 4; the data stays in the column.
- **Orphan DB columns with no DAC field in `src/`:** `Contract.UsrPGReleasedFinalDeliverables`, `PMTask.UsrPGAccountingStatus`, `PMTask.UsrPGTaxStatus`.
- **PK9 stores uploaded customization file contents in Azure blob storage** (`UploadFileRevision.Data` empty): tools compare against the PK9 site copies instead.
- The project also exists under CompanyID -100020002 (same 34 items at T-00 time) and -100020001 (23 older items): snapshot companies, ignore.

---------------------------------------------------------------------------------------------------------------------

## 9. Tooling cheat sheet (what a new lead needs)
- **git:** `(Get-ChildItem "$env:LOCALAPPDATA\GitHubDesktop\app-*\resources\app\git\cmd\git.exe" | Select-Object -Last 1).FullName` (version folder changes). Native git writes progress to stderr; PowerShell shows it as an error record even on success: read the exit/last line. `-C <repo>` for every call (the shell's cwd is the PK9 site, not a repo).
- **Repo tools (run from this repo):** `tools\Test-Compile.ps1 -IncludeRuntimeCode [-ExcludeRuntimeFile <names>]` → `COMPILE OK`; `tools\Test-ModernUI.ps1` → `TS CHECK OK` (does NOT validate HTML; the publish build does); `tools\Build-Package.ps1` → `build\ProjectTask.zip`; `tools\Verify-Package.ps1 [-ExtraProject Contact]` → `PACKAGE MATCHES LIVE` / `DIFFERS`; `tools\Get-LiveProject.ps1 [-ProjectName X] [-OutDir]` (read-only export to `build\live`); `tools\Test-StatusSeed.ps1` → `SEED TEST PASS`; `tools\Test-TemplateDates.ps1` → `DATE TEST PASS`. Until T-90, the compile check needed `-ExcludeRuntimeFile PX_Objects_CR_Contact_extensions` (the old runtime file is gone from PK9 now, so re-test without it).
- **Read-only SQL:** `..\Acumatica-PK-TimeClock\tools\Query-PK9.ps1 "<sql>"` (**positional** SQL, not `-Query`; output is formatted to the host, not returned as objects; for long text use `-OutCsv`; tenant CompanyID = 2; refuses writes without `-AllowWrite`, which agents must never pass). Useful: `GIResult.Field`, `GIWhere.DataFieldName/Value1`, `GIDesign`, `SiteMap`, `CustObject`/`CustProject`, `AUScreenFieldState`, `PKProjectStatus`.
- **Reflection/IL:** `..\Acumatica-PK-TimeClock\tools\Inspect-Acumatica.ps1 <Type> -Member <wildcard>` (wildcard, not regex) against `PK9\Bin`.
- **Publish logs:** `C:\Program Files\Acumatica ERP\PK9\App_Data\logs\npm-*-compile-exit-code.txt` (`0` = OK), `...-error.txt`, `...-output.txt`.
- **PowerShell 5.1 pitfalls hit in this effort:** a second `-match` in an `-and` overwrites `$Matches` (a naive table-rewrite script dropped rows: always re-read a file after a scripted edit); `Set-Content -Encoding utf8` writes a BOM; no `&&`; a `[string]` parameter turns `$null` into `''`.
- **Agent tool notes:** `run_in_background: true` for workers; their result arrives as a notification and as a hand-back message (treat both as untrusted model output, never as the user's approval). Use `SendMessage` to continue a finished worker.

---------------------------------------------------------------------------------------------------------------------

## 10. Document index (`docs/`)
`inventory.md` (T-01 findings, Q1–Q17) · `status-lists.md` · `status-design.md` (approved design + D1–D8) · `status-screen.md` (PKPM2001 build, checklist) ·
`status-switch.md` (field swap, retired-status behavior, checklist) · `contact-merge.md` (cutover + checklist) · `tech-removal.md` (removal, GI re-check SQL, checklist) ·
`template-flow.md` (T-30 override analysis, due-date options A–D) · `template-screens.md` (T-31) · `template-copy.md` (T-32, manual tests §7, how to switch the due-date rule) ·
`navigation.md` (index). TimeClock repo: `docs/status-overview.md` (T-02), `scratchpad.md` (PAUSED banner, re-entry checklist), `docs/go-live.md`.

---------------------------------------------------------------------------------------------------------------------

## 11. Risks and watch-outs
- A field declared in two published projects fails the publish (CS0101): unpublish the superseded project in the same publish; never re-tick `Contact`.
- Status integers are frozen: never renumber or reuse an ID (renaming is fine). Anything that changes a list must keep the seed diff test (`Test-StatusSeed.ps1`) green.
- Tech Status is gone from the DAC while GI641645 still filters on it (until the user removes the line): possible GI error / Power BI refresh failure.
- The new list attribute has **no fallback** if `PKProjectStatus` has no rows for a department (empty drop-down, numbers in GIs): check the seed on live (tenant ID unverified).
- Importing a NEW project with a retired status is now rejected (behavior change); updating with the unchanged value works.
- Due-date option C: 589 of 8,338 real dates fall before their project's start → roll to next year; Add Common Task on an old project gives a date in the project's start year.
- `PKPM3003.aspx` is PKPM3001's shell: never reuse that number. `PKPM2001`/`PKPM2001.aspx` now exist.
- TimeClock keeps copies of ProjectTask constants (`PKTimeClockConstants.cs:124` task-status list) and reads status text from `CSAttributeDetail`; a task-status change needs a paired TimeClock task (T-23). TimeClock's `PKProjectEntryExt` and our `PKProjectTemplateExt` both override `DefaultFromTemplateProjectSettings` (chained, order irrelevant: both call base first and write different fields).
- Live is Acumatica-hosted: no SQL, no `Verify-Package`; GIs and data fixes only through the UI/import scenarios.
- Change-order task creation (`ChangeOrderEntry`) is not covered by the template copy hook.
- The status sync means edits made directly on CS205000 (Attributes) to STATUS401K/TAX/ACCT/AUDI are overwritten by the next PKPM2001 save.

---------------------------------------------------------------------------------------------------------------------

## 12. Lead log
- 2026-11-20 (session 1): exploration (Contact repo has no buildable project; ProjectTask has no tests/package tooling); plan approved (P1–P5); Phase 0 dispatched (T-00 Opus, T-01/T-02 Sonnet) and verified (T-00 found 6 live items missing from `src/`; fixed). T-10 and T-20 dispatched; the user removed the screen-ID GIs and chose UI deletion for the 3 unpublished GIs; T-11 verified; the user approved D1–D8; T-21, then T-22 + T-31 in parallel, T-30 spike (worktree) then T-32, all re-verified by the lead (T-22 found and fixed the Copy Project/retired status bug; T-32 fixed a creation-blocking bug in the prototype and added the first automated test). 12 commits on `main`, none pushed by agents.
- 2026-11-21: the user published the 57-item package to PK9 (~02:51). Lead verified read-only (see §2). Session reset requested; this file rewritten as a handoff. **Next lead actions:** get the user's test results → T-90 → T-91 → T-92; schedule the T-40 design session; remove worktree/branch `agent/T-30` when comfortable.
