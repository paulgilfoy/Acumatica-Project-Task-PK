# PK Consolidation & Expansion — Master Plan (lead agent + worker swarm)

Created 2026-11-20. Rewritten as a session handoff 2026-11-21; **§0–§2 rewritten again 2026-11-22 before the live publish** (the user
clears the lead session after this). **This file is the single source of truth for the effort.** The lead agent (PM/orchestrator) keeps it current.
Workers read this file, `CLAUDE.md`, `scratchpad.md` and `navigation.md` before starting.

---------------------------------------------------------------------------------------------------------------------

## 0. READ THIS FIRST (new lead session)

**Who you are:** the lead/PM agent. You dispatch worker subagents (Agent tool), re-verify their work yourself, commit,
and keep this file current. **You never publish, never write to the PK9 database, never touch live.** The user publishes.

**First 10 minutes:**
1. Read this file, then `CLAUDE.md` and `scratchpad.md` in this repo, `docs/live-cutover.md`, and `C:\Program Files\Acumatica ERP\PK9\navigation.md`.
2. `git log --oneline -15` and `git status --short` here and in `..\Acumatica-PK-TimeClock` (git path in §9). Expected: this repo `main` at or
   after the 2026-11-22 handoff commit; TimeClock `main` at or after `ab86090` (T-26). Only `CLAUDE.md` (M) and `GUIDE.md` (??) uncommitted: the user's.
3. Ask the user how the **live publish (release 1)** went and walk through `docs/live-cutover.md` §4–§6 with them.

**Where things stand (2026-11-22):** **Release 1 is cleared for the live publish tonight.** On PK9 the user tested and passed every checklist:
Contact merge, Tech removal, PKPM2001 Department Statuses (18 tests, incl. the T-21b save fix), status drop-downs (status switch), template screens,
template copy incl. Add Common Task with as-is due dates. PK9 runs the 2026-11-21 11:43 publish = `main` @ `15ee9d4`; the lead rebuilt that
exact state as **`build\ProjectTask-LIVE-15ee9d4.zip`** (SHA-256 `CBDA1855…96E6`, `Verify-Package` vs PK9 → PACKAGE MATCHES LIVE). The live
backup and project exports are done; the lead diffed the exports (no live item lost, no live drift) and compile-tested live's other projects
with the new code (OK). Built since, **not on PK9 yet:** release 2 = T-24 PKPM2001 arrows + T-25 read-only PK Status (`build\ProjectTask-T25.zip`);
release 3 = T-26 EP307000 project lookup in the **PKTimeClock** project (TimeClock repo `build\PKTimeClock.zip`). Common tasks: the user considers
them finished as built (T-32 copy) and is collecting feedback from **alpha testers** on their team; the budget-line idea (T-40..43) is parked.

---------------------------------------------------------------------------------------------------------------------

## 1. WORK LEFT (state 2026-11-25, per the user)
**Done (user, 2026-11-25):** releases 1, 2 (T-24 + T-25) and 3 (T-26 PKTimeClock, ProjectTime unpublished) are published on PK9 **and live**;
live `Contact` cleanup and ACCT 11 handled; TimeClock D19c + D22 published. Alpha testers are giving feedback now.
1. **Housekeeping (user):** remove worktrees `..\Acumatica-Project-Task-PK-T30` (+ branch `agent/T-30`) and `..\Acumatica-Project-Task-PK-live`. (`CLAUDE.md`/`GUIDE.md` already committed.)
2. **T-01 / T-02 docs:** inventory and time clock gap review are written; awaiting the user's review.
3. **T-23:** TimeClock to read status text from `PKProjectStatus`. Paused until at least 2026-12-20.
(T-40..43 common-task budget lines: removed from the plan by the user 2026-11-25.)

---------------------------------------------------------------------------------------------------------------------
## 2. Current state (2026-11-22), verified by the lead read-only

| Item | State |
|---|---|
| PK9 runs | `ProjectTask` 57 items = `main` @ `15ee9d4` (published 2026-11-21 11:43, npm exit 0); `PKTimeClock` 46 items (= TimeClock `src` before T-26); Velixo; `Contact` deleted; ProjectTime not on PK9 |
| Release 1 zip (live tonight) | `build\ProjectTask-LIVE-15ee9d4.zip`, 57 items, 87,341 bytes, SHA-256 `CBDA18554866ABEBC2072170E999C51570FDB8C0135380E2FA129CE9F1F296E6`; `Verify-Package` vs PK9: **PACKAGE MATCHES LIVE**, 0 problems |
| Release 2 zip | `build\ProjectTask-T25.zip`, 60 items (adds Code `PKDepartmentStatus`, `PKDepartmentStatusAttribute`, Sql `PKProjectStatusBackfill`; changes Contract, PKProjectStatus, PKProjectStatusMaint, Table Contract, PM301000 UI), SHA-256 `794EF8A9…6252`; COMPILE/TS/SEED/DATE/PK STATUS tests pass. `build\ProjectTask.zip` = same as T-25 (latest build of `main`) |
| Release 3 zip | TimeClock repo `build\PKTimeClock.zip` (46 items, T-26 inside `PKEmployeeTimeActivitiesExt`), SHA-256 `30FF0D3C…E7E2`; COMPILE OK. Frozen first-release copy `build\cutover\PKTimeClock.zip` untouched |
| PK9 user test results | ALL PASS: contact-merge, tech-removal, status-screen (18), status-switch, template-screens, template-copy (incl. Add Common Task, as-is dates) |
| PKPM2001 data on PK9 | 55 statuses (seed 54 + user's ACCT 11); first sync ran for all four departments (CSAttributeDetail SortOrder filled); test renames reverted |
| Contact data | 8 flag counts = 2026-11-20 baseline (271/254/298/175/10/437/225/92) |
| Tech removal | 0 TechStatus refs in GIResult/GIWhere/GISort/AUScreenFieldState on PK9; **live GIs still to clean (runbook §2.2)** |
| Live exports (2026-11-21 22:30) | `C:\Users\Administrator\Downloads\Live-Site-Customizations` (Contact, PKTimeClock 38, ProjectTask 34, ProjectTime 8, Velixo 135). Diff + compile: no surprises (runbook §2.1) |
| Live | Not yet touched by this effort; backup taken by the user 2026-11-21 |

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
| D9 (user, 2026-11-21) | **Template/common-task due dates are copied AS-IS (option A).** Due dates are usually static dates the user can modify; projects are created in bulk and the user bulk-updates dates (e.g. all "2026" tax projects to 4/15/2027, "shortly"). No roll-forward rule is wanted: a tax project created after 4/15 is back-dated to 4/15/2027 and then the "Extended" checkbox is checked for the IRS extension filing. (If a rule were ever wanted: the next calendar occurrence of the month/day.) Implemented by switching `PKTemplateDueDate.ActiveRule` to AsIs (T-32b) |
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
| T-10 | Merge the Contact customization | Live `Contact` project (5 items: DAC item, Table, Classic page, 2 Modern UI files) now in `src/` (`PKContactExt.cs`, `Tables/Contact.xml`, `PageCustomizations/CR302000.xml`, CR302000 `.ts/.html` unchanged from live: the repo's "binding mismatch" only existed in a stale variant). Fields: **8** bool, not ~6. Cutover: `docs/contact-merge.md`. Commit `60d537c` | Opus 5.5 / high | **DONE on PK9** (user checklist passed 2026-11-21/22; live = release 1) |
| T-11 | Remove Tech Status + TECH display | Removed from `Contract.cs`, `Automation/PM301000.xml`, `PageCustomizations/PM301000.xml`, PM301000 UI. Table column declaration kept (does not drop the column). Doc `docs/tech-removal.md`. Commit `9307996`. GI641645 filter removed by the user 2026-11-21 (verified); the same GI cleanup is still needed on live | Opus 5.5 / high | **DONE on PK9** (user checklist passed 2026-11-21/22; live = release 1) |

### Phase 2 — Status maintenance (P3)
| ID | Task | Result / acceptance | Model / effort | Status |
|---|---|---|---|---|
| T-20 | Design | `docs/status-design.md`; user approved D1–D8. Commit `7979bb1` | Opus 5.5 / high | DONE |
| T-21 | Screen PKPM2001 | DAC `PKProjectStatus`, graph `PKProjectStatusMaint` (rules: ID > 0/unique/read-only once saved, description unique case-insensitive, saved rows undeletable; syncs to `CSAttributeDetail` in `Persist`), DB-backed `PKProjectStatusListAttribute : PXIntListAttribute`, `Sql/PKProjectStatus.sql` (idempotent seed, 54 rows, all companies with the STATUSTAX attribute), site map, ASPX shell, Modern UI, `tools/Test-StatusSeed.ps1`, `docs/status-screen.md`. Commit `60dd8ac` | Opus 5.5 / high | **DONE** (user checklist passed 2026-11-21, with the T-21b fix) |
| T-22 | Switch the four project fields | `Usr401kStatus`/`UsrPGTaxStatus`/`UsrPGAcctStatus`/`UsrPGAuditStatus` use `[PKProjectStatusList("401K"/"TAX"/"ACCT"/"AUDIT")]`. Fixed a real bug: Copy Project would have failed for a retired status (attribute skips verification for copies). Behavior change: importing a NEW project with a retired status is rejected. `docs/status-switch.md`. Commit `dd9a58b` | Opus 5.5 / high | **DONE on PK9** (user checklist passed 2026-11-21/22; live = release 1) |
| **T-21b** | **FIX: PKPM2001 could not save (renames and new statuses).** User reports 2026-11-21: new ACCT status -> "description cannot be blank"; rename -> "Status ID 2 already exists for department ACCT" (stack trace supplied: `PKProjectStatusMaint.FieldVerifying<statusID>` <- `PXCache.FillWithValues` <- `PXCache.Update(keys, values)` <- Modern UI `GridDataHandler.UpdateRow`, then `SessionRollback`). **ROOT CAUSE (confirmed from the stack trace + code):** the grid update path fills a COPY of the row field by field and raises `FieldVerifying` while the copy's own keys are still empty; my cross-row duplicate-ID scan found the saved row itself, so EVERY edit of a saved row failed; the session rollback also discarded a pending new row's typed description (the "blank description" message). **FIX (built 2026-11-21, lead):** no cross-row checks in key-field `FieldVerifying` (it only rejects IDs <= 0); duplicate description flagged inline in `RowInserted`/`RowUpdated` and enforced in `RowPersisting`; duplicate ID on a new row enforced in `RowPersisting`. Documented in `docs/status-screen.md` (retest list). Lead re-ran: COMPILE OK (also WITHOUT the Contact exclusion), TS CHECK OK, 57 items, SEED TEST PASS. **Unverified until the user republishes:** the real grid behavior (rename, add, deactivate, duplicate description), whether Save is enabled with no changes (the first sync relies on it), and the `CSAttributeDetail` sync (never run yet). **Lesson:** Acumatica runs `FieldVerifying` on a half-filled copy in the Update(keys, values) path, so never look at other rows or at the row's own keys there. | `PKProjectStatusMaint.cs` | `docs/status-screen.md` | Opus 5.5 / high (lead did it) | **DONE** (user: all 18 PKPM2001 manual tests passed, 2026-11-21) |
| **T-21c** | **FIX: slot-cache cast error after a republish.** User retest 2026-11-21 03:27 UTC on the republished build: changing ACCT 8 sort order to 9 **saved**, and the first `CSAttributeDetail` sync ran (lead verified read-only: `PKProjectStatus` ACCT 8 SortOrder 9; STATUSACCT rows 01–08 have SortOrder, ACCT 02 = "Initial Review (Acctg)"). The trace showed first-chance `InvalidCastException "[A]Definition cannot be cast to [B]Definition"` (two `RuntimeCode_*` assemblies) in `PKProjectStatusListAttribute.GetStatuses` → `PXDatabase.GetSlot`: the constant slot key let the new build find the OLD build's cached `Definition`. Acumatica recovered (save succeeded). **Fix:** slot key now includes `typeof(Definition).Assembly.FullName`. Lead: COMPILE OK. Harmless until the next republish; ships with it | `PKProjectStatusListAttribute.cs` | Opus 5.5 (lead did it) | PUBLISHED-ON-PK9 (11:43; runtime verified) |
| T-24 | PKPM2001 Previous/Next Department toolbar arrows (user request 2026-11-21). Plain `PXButton`s with the standard `PagePrev`/`PageNext` icons (not `PXPreviousButton`, which adds a "discard changes" confirm); step through departments that have statuses, wrapping; same path as the drop-down (`Filter.Update`). Lead reviewed the diff and re-ran: COMPILE OK, TS CHECK OK, 57 items, DATE/SEED PASS. Manual list in `docs/status-screen.md` | `PKProjectStatusMaint.cs`, `PKProjectStatus.cs` | Opus 5.5 / high | **DONE** (PK9 + live, user 2026-11-25) |
| T-25 | **Read-only "PK Status" on the project (user approved 2026-11-21, after a DB-cost review: no extra queries per save, values ride in the existing UPDATE, ~1 MB).** Two stored columns on `Contract`: `UsrPKStatusID` (int) + `UsrPKStatus` (text) = the status of the project's own department (`UsrPGDepartment` → TAX/ACCT/AUDIT/401K field; other departments → empty). Kept current on every project save in any graph, on PKPM2001 renames/(de)activation (one set-based UPDATE), backfilled by an idempotent publish SQL (which doubles as the "recalculate"). Read-only on PM301000; usable in every GI. Additive: no GI, business event, import or TimeClock change; the CSAnswers view stays | `Contract.cs`, `Tables/Contract.xml`, a Sql item, `PKProjectStatusMaint.cs`, PM301000 UI | Opus 5.5 / high | **DONE** (PK9 + live, user 2026-11-25). Was READY-FOR-PK9 (lead re-ran: COMPILE OK, TS CHECK OK, 60 items, SEED/DATE/PK STATUS TEST PASS; backfill preview 9,735 of 11,459 rows independently re-counted; zip `build\ProjectTask-T25.zip` includes T-24; manual list `docs/pk-status.md` §7) |
| T-26 | **EP307000 Employee Time Activities: project lookup shows only Active and In Planning projects** (user request 2026-11-22; Pending Approval, On Hold, Completed, Cancelled hidden). **User decision: lives in the `PKTimeClock` project / TimeClock repo** (pause lifted for this task only). Server-side restrictor on the activity grid's ProjectID, preferably in TimeClock's existing EP307000 extension `PKEmployeeTimeActivitiesExt`, so Classic and Modern UI follow. Existing entries on other projects must still display and save. **Release impact:** PK9's PKTimeClock has 7 EP307000 items live does not have (the ProjectTime replacement); T-26 reaches live only with that TimeClock release, separate from the ProjectTask cutover | TimeClock repo `src/` | Opus 5.5 / high | **DONE** (PK9 + live, user 2026-11-25). Was READY-FOR-PK9 (in `PKEmployeeTimeActivitiesExt.cs`, grid + filter; lead: COMPILE OK, `ProjectStatus.planned` verified; 8,234 projects leave the lookup, 2,437 stay; TimeClock commit; manual list in TimeClock `docs/ep307000-active-projects.md` §7) |
| T-23 | TimeClock reads status text from `PKProjectStatus` instead of `CSAttributeDetail` (optional; needed before the sync can ever be retired; also `PKTaskStatusListAttribute` copy) | Cross-repo; only after TimeClock resumes (≥ 2026-12-20). No change is needed for things to work now | Opus 5.5 / high | TODO (paused) |

### Phase 3 — Template → project flow
| ID | Task | Result / acceptance | Model / effort | Status |
|---|---|---|---|---|
| T-30 | Spike | `docs/template-flow.md`. **One override covers both flows:** `[PXOverride] ProjectEntry.OnCopyPasteTaskInserted(PMProject target, PMTask newItem, PMTask sourceItem)` (protected virtual; verified in the 26.200.0334 DLL by the lead). `OnDefaultFromTemplateTasksInserted` alone would miss Add Common Task. Coexists with TimeClock's `PKProjectEntryExt`. Doc commit `5a3eed8`; prototype on branch `agent/T-30` (superseded) | Opus 5.5 / high | DONE |
| T-31 | Template screens | New extensions on PM208000 (fieldset "PK Project Info" on `ProjectProperties`: Department, Priority, Complexity; Tasks grid: Due Date, Preparer, Reviewer, Tech Reviewer), PM208010 + PM208030 (fieldset "PK Task Info" on `TaskProperties`). `docs/template-screens.md`. Commit `1945dd9` | Opus 5.5 / high | **DONE on PK9** (user checklist passed 2026-11-21/22; live = release 1) |
| T-32 | Copy logic | `PKProjectTemplateExt` (project fields via `DefaultFromTemplateProjectSettings`; task fields via `OnCopyPasteTaskInserted`; copy only when the target is empty; skips Copy Project/Create Template) + `PKTemplateDueDate` (pure date logic) + **`tools/Test-TemplateDates.ps1` (36 cases, runs the real code: the first automated test)**; bad template values become a warning (a prototype bug would have blocked creation). `docs/template-copy.md` §7 = PK9 test steps. Commit `de8bb8c`. Due-date option C is the agent's default, **not confirmed by the user** | Opus 5.5 / high | **DONE on PK9** (user checklist passed 2026-11-21/22; live = release 1) |

| **T-32b** | **Due-date rule switched to AS-IS (D9).** `PKTemplateDueDate.ActiveRule = AsIs`; `tools\Test-TemplateDates.ps1` default `-ExpectRule AsIs`; lead re-ran: DATE TEST PASS (36/36), COMPILE OK. `docs/template-copy.md` has the update + manual test (4/15/2027 template date gives 4/15/2027 regardless of creation date). Bulk-updating existing project dates remains the user's own workflow | `PKTemplateDueDate.cs`, tools, docs | `docs/template-copy.md` | Sonnet 5.5 / medium (lead did it) | **DONE on PK9** (user checklist passed 2026-11-21/22; live = release 1) |

### Phase 4 — Common tasks (design with the user first)
| ID | Task | Notes | Model / effort | Status |
|---|---|---|---|---|
| T-40-prep | Fact brief for the session: `docs/common-tasks-brief.md` (current common tasks, L/N items, template budget lines, confirmed hook signatures, options A–E, 13 questions). Lead spot-checked: items L 268 / N 35 / E 4; global task only `03AL` (inactive, not auto-include); 51 templates (`BaseType='R'`), 284 tasks, all AutoIncludeInPrj | Sonnet 5.5 / medium | DONE (brief kept for later) |

### Phase 5 — Closeout
| ID | Task | Depends | Model / effort | Status |
|---|---|---|---|---|
| T-90 | After the user confirms PK9: re-extract `src/` from PK9 (`tools\Get-LiveProject.ps1 -OutDir <repo>\src` only when asked; otherwise compare), expect plain `Verify-Package` (no `-ExtraProject`) to report `PACKAGE MATCHES LIVE`; drop the `-ExcludeRuntimeFile PX_Objects_CR_Contact_extensions` workaround from the docs/tools notes if no longer needed; update `navigation.md`, `scratchpad.md`, session log | user results | Sonnet 5.5 / medium | **DONE** (user 2026-11-25). Was MOSTLY DONE (2026-11-22: zip from `15ee9d4` = PK9, PACKAGE MATCHES LIVE; workarounds dropped from docs by T-93; nav updates in T-91) |
| T-90a | `Verify-Package` ignores the re-generated Classic `pageSource` on Page items by default (`-StrictPageSource` restores the old check). Found: pm301000/pm302000 also had anchor `OriginalIndex` renumbering (Acumatica rebased them on the 26.200 base pages on import); lead chose to **re-sync those two `src/PageCustomizations` files from PK9** instead of loosening the check. Lead re-ran after rebuild: all items OK except `Code#PKProjectStatusListAttribute` (= the unpublished T-21c fix, expected) | Opus 5.5 / high | DONE (tooling) |
| T-91 | Update PK9 `navigation.md` repo table (Contact merged and retired; TimeClock paused; ProjectTask now has Contact fields, status screen, template flow). Nav `.md` edits are allowed | T-90 | Haiku 5.5 / low | DONE (2026-11-22, lead) |
| T-92 | Live cutover (user publishes; lead prepares): final zip, pre-flight list (GIs cleaned in the live UI, backup, Contact unticked, PKPM2001 rights, check seed company), post-publish checklist | PK9 passes | Sonnet 5.5 / medium (runbook) | **DONE** (release 1 live; user 2026-11-25) |

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
- **After a publish, 5 Classic page-customization items (`cr302000`, `pkpm3002`, `pkpm3003`, `pm301000`, `pm302000`) show as `DIFF` in `Verify-Package`:** Acumatica re-generates the compressed `pageSource` blob on import, so the bytes differ although the customization is the same (verified 2026-11-21: they differ right at the `pageSource` attribute). Harmless (Classic UI is unused, D1); T-90 can re-sync those 5 files from `build\live\src` or teach `Verify-Package` to ignore `pageSource`. Every other item matched what was published (only the two files changed after the publish differ).
- **Acumatica Modern UI grid saves with `PXCache.Update(keys, values)`, which raises `FieldVerifying` on a half-filled COPY of the row** (key fields still empty) and on any failure runs a `SessionRollback` that discards pending edits of the row. Never scan other rows or rely on the row's own keys in `FieldVerifying` handlers; check uniqueness in `RowInserted`/`RowUpdated`/`RowPersisting`. (The same lesson applies to any future custom grid screen.)
- The project also exists under CompanyID -100020002 (same 34 items at T-00 time) and -100020001 (23 older items): snapshot companies, ignore.

---------------------------------------------------------------------------------------------------------------------

## 9. Tooling cheat sheet (what a new lead needs)
- **git:** `(Get-ChildItem "$env:LOCALAPPDATA\GitHubDesktop\app-*\resources\app\git\cmd\git.exe" | Select-Object -Last 1).FullName` (version folder changes). Native git writes progress to stderr; PowerShell shows it as an error record even on success: read the exit/last line. `-C <repo>` for every call (the shell's cwd is the PK9 site, not a repo).
- **Repo tools (run from this repo):** `tools\Test-Compile.ps1 -IncludeRuntimeCode [-ExcludeRuntimeFile <names>]` → `COMPILE OK`; `tools\Test-ModernUI.ps1` → `TS CHECK OK` (does NOT validate HTML; the publish build does); `tools\Build-Package.ps1` → `build\ProjectTask.zip`; `tools\Verify-Package.ps1 [-ExtraProject Contact]` → `PACKAGE MATCHES LIVE` / `DIFFERS`; `tools\Get-LiveProject.ps1 [-ProjectName X] [-OutDir]` (read-only export to `build\live`); `tools\Test-StatusSeed.ps1` → `SEED TEST PASS`; `tools\Test-TemplateDates.ps1` → `DATE TEST PASS`. The `-ExcludeRuntimeFile PX_Objects_CR_Contact_extensions` workaround is NO LONGER NEEDED (verified 2026-11-21: `Test-Compile.ps1 -IncludeRuntimeCode` passes without it; the old Contact runtime file is gone from PK9). Plain `Verify-Package.ps1` (no `-ExtraProject`) now compares against the 57-item project published on PK9.
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
`live-cutover.md` (T-92 live runbook, release plan) · `common-tasks-brief.md` (T-40 facts/options, parked) · `pk-status.md` (T-25) · `navigation.md` (index). TimeClock repo: `docs/status-overview.md` (T-02), `docs/ep307000-active-projects.md` (T-26), `scratchpad.md` (PAUSED banner, re-entry checklist), `docs/go-live.md` (STALE for the next release).

---------------------------------------------------------------------------------------------------------------------

## 11. Risks and watch-outs
- A field declared in two published projects fails the publish (CS0101): unpublish the superseded project in the same publish; never re-tick `Contact`.
- Status integers are frozen: never renumber or reuse an ID (renaming is fine). Anything that changes a list must keep the seed diff test (`Test-StatusSeed.ps1`) green.
- Tech Status is gone from the DAC on PK9 and the PK9 GIs are clean; **live GIs must be cleaned in the UI before the live publish** (GI641645 filter etc.), otherwise GI errors / Power BI refresh failures.
- The new list attribute has **no fallback** if `PKProjectStatus` has no rows for a department (empty drop-down, numbers in GIs): check the seed on live (tenant ID unverified).
- Importing a NEW project with a retired status is now rejected (behavior change); updating with the unchanged value works.
- Due-date option C: 589 of 8,338 real dates fall before their project's start → roll to next year; Add Common Task on an old project gives a date in the project's start year.
- `PKPM3003.aspx` is PKPM3001's shell: never reuse that number. `PKPM2001`/`PKPM2001.aspx` now exist.
- TimeClock keeps copies of ProjectTask constants (`PKTimeClockConstants.cs:124` task-status list) and reads status text from `CSAttributeDetail`; a task-status change needs a paired TimeClock task (T-23). TimeClock's `PKProjectEntryExt` and our `PKProjectTemplateExt` both override `DefaultFromTemplateProjectSettings` (chained, order irrelevant: both call base first and write different fields).
- Live is Acumatica-hosted: no SQL, no `Verify-Package`; GIs and data fixes only through the UI/import scenarios.
- Change-order task creation (`ChangeOrderEntry`) is not covered by the template copy hook.
- **PKPM2001 on PK9 still has the save bug until the T-21b fix is republished.** Until then: do not ask users to add or rename statuses and do not expect the `CSAttributeDetail` sync; republish T-21b + T-32b together so the user publishes once.
- The status sync means edits made directly on CS205000 (Attributes) to STATUS401K/TAX/ACCT/AUDI are overwritten by the next PKPM2001 save.

---------------------------------------------------------------------------------------------------------------------

## 12. Lead log
- 2026-11-25: user: releases 1–3 (incl. T-24/T-25/T-26, TimeClock D19c + D22) published on PK9 and live; live Contact cleanup + ACCT 11 done; alpha feedback under way. T-24/25/26, T-90, T-92 set DONE; T-40 and T-41..43 deleted at the user's request (brief `docs/common-tasks-brief.md` kept). Left: worktree cleanup, T-01/T-02 doc review, T-23 (paused). §1 rewritten.
- 2026-11-22 (session 3, end): the user tested template screens + template copy (incl. Add Common Task): ALL PASS, so every PK9 checklist has passed. The user considers common tasks finished as built and will gather feedback from team alpha testers (T-40..43 parked). PK9 still runs the 2026-11-21 11:43 publish (`15ee9d4`; T-24/T-25/T-26 never republished there), so the lead rebuilt that exact commit in a detached worktree `..\Acumatica-Project-Task-PK-live` -> `build\ProjectTask-LIVE-15ee9d4.zip` (CBDA1855...96E6), Verify-Package vs PK9 PACKAGE MATCHES LIVE. Releases defined: 1 = tonight (that zip), 2 = T-24 + T-25 (`ProjectTask-T25.zip`, PK9 round first), 3 = PKTimeClock with T-26 (needs ProjectTime unpublished on live; TimeClock go-live.md stale). Updated PLAN §0-§2 and §7 statuses, `docs/live-cutover.md` (release plan, gate passed, zip/hash), template docs, scratchpad, PK9 `navigation.md` + `CLAUDE.md` (T-91), memory. The user clears the session next and asks for one last check before the live push.
- 2026-11-22 (session 3): T-26 (EP307000 project lookup: Active + In Planning) built by a worker in the **TimeClock repo** at the user's request (commit `ab86090` there); lead verified the diff, COMPILE OK, `ProjectStatus.planned`. T-25 (PK Status) built, verified, committed `c42a0d0`; T-24 arrows `b2244de`.
- 2026-11-21 ~22:40: the user exported all live projects (`C:\Users\Administrator\Downloads\Live-Site-Customizations`: Contact, PKTimeClock 38 items, ProjectTask 34, ProjectTime 8, Velixo 135). Lead diff: no live `ProjectTask` item would be deleted; the 5 differing items equal our pre-change source `4bbde0f` (no live drift); live PKTimeClock + ProjectTime (+ generated DAC stubs) + Velixo code compile with the new ProjectTask code (exit 0). Live PKTimeClock depends on ProjectTime's DAC items: keep ProjectTime ticked. Recorded in `docs/live-cutover.md` §2.1. T-25 worker running; live publish to use `build\ProjectTask-T24.zip` after the PK9 T-24 run.
- 2026-11-21 ~21:00 (session 3): the user republished at 11:43 (T-21c included). Lead re-ran everything: Verify-Package **PACKAGE MATCHES LIVE** (zip SHA-256 AF9171C9…06A1, `main` @ 15ee9d4), COMPILE OK, TS CHECK OK, DATE TEST PASS, SEED TEST PASS (after teaching `Test-StatusSeed.ps1` to accept rows added/renamed on PKPM2001 and synced; the user's ACCT 11 "Ready for Review" test row had made it fail). Read-only: all project status values exist in `PKProjectStatus`; 0 TechStatus refs; Contact flag counts = baseline; user edits survived the republish. Still untested (UI): rename/deactivate/duplicate description, first save for 401K/TAX/AUDIT, template copy, Add Common Task, Copy Project with a retired status, screen looks. `docs/live-cutover.md` updated with §0 (lead verification) and the gate split into done/YOU. The user is taking a live backup; live publish only after the gate.
- 2026-11-21 (session 3, newest): the user republished the fixed package (~03:25); lead verified runtime code read-only. Workers: T-92 runbook draft (`docs/live-cutover.md`, committed `7fac27c`), T-40-prep brief (`docs/common-tasks-brief.md`, `e4009da`, numbers spot-checked), T-93 stale-docs cleanup (status-screen/status-switch/template-copy/scratchpad open items; lead checked the as-is §7 expectation), T-90a `Verify-Package` pageSource normalisation (running). Lead fixed T-21c from the user's trace (COMPILE OK). Corrected PK9 `navigation.md` (templates are `BaseType='R'`). Remaining T-93 leftovers: template-copy §3/§5 option-C remarks, status-screen "Not verifiable" list, scratchpad "What is live on PK9" table.
- 2026-11-20 (session 1): exploration (Contact repo has no buildable project; ProjectTask has no tests/package tooling); plan approved (P1–P5); Phase 0 dispatched (T-00 Opus, T-01/T-02 Sonnet) and verified (T-00 found 6 live items missing from `src/`; fixed). T-10 and T-20 dispatched; the user removed the screen-ID GIs and chose UI deletion for the 3 unpublished GIs; T-11 verified; the user approved D1–D8; T-21, then T-22 + T-31 in parallel, T-30 spike (worktree) then T-32, all re-verified by the lead (T-22 found and fixed the Copy Project/retired status bug; T-32 fixed a creation-blocking bug in the prototype and added the first automated test). 12 commits on `main`, none pushed by agents.
- 2026-11-21: the user published the 57-item package to PK9 (~02:51). Lead verified read-only (see §2). Session reset requested; this file rewritten as a handoff. **Late 2026-11-21: user feedback.** The user has not read the docs; reported the PKPM2001 save bug (new status gives "description cannot be blank"), set the access rights, removed the GI641645 filter (lead verified), decided due dates are copied as-is (D9), confirmed no live push until all tests pass, and scheduled the common-task design for 2026-11-22. Lead read the code and checked the DB read-only (nothing saved; sync never ran); hypotheses recorded in T-21b. Session reset requested. **Next lead actions (ordered):** (1) T-21b with the user's extra details; (2) T-32b; (3) rebuild + verify one package, the user republishes on PK9; (4) get checklist results; (5) T-90, T-91, T-92; (6) the T-40 design session with the user. (Older list:) get the user's test results → T-90 → T-91 → T-92; schedule the T-40 design session; remove worktree/branch `agent/T-30` when comfortable.
- 2026-11-21 (late, same session): the user supplied the PKPM2001 stack traces; the lead found the root cause (see T-21b), fixed `PKProjectStatusMaint.cs`, switched the due-date rule to AsIs (D9/T-32b), updated `docs/status-screen.md` + `docs/template-copy.md`, and re-ran: COMPILE OK (without the Contact exclusion), TS CHECK OK, DATE TEST PASS (AsIs, 36/36), SEED TEST PASS, package 57 items; plain `Verify-Package` vs PK9 differs only in the two changed code items plus the 5 Classic page blobs (re-generated by Acumatica on import). **Next lead actions (ordered):** (1) commit is done; ask the user to import `build\ProjectTask.zip` (rebuild from `main` first) and republish on PK9; (2) the user retests PKPM2001 (`docs/status-screen.md`) and saves once per department; (3) get the other checklist results; (4) T-90, T-91, T-92; (5) T-40 design session with the user (scheduled 2026-11-22).
