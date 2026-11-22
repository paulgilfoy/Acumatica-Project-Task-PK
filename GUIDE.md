# GUIDE: Working in the PK Acumatica repos (multi-agent)

For any AI agent (or human lead) working in **`Acumatica-PK-TimeClock`** or **`Acumatica-Project-Task-PK`**.
Goal: **many agents can work at once without colliding, and a cold-start session finds its way in minutes.**

It adapts a general "swarm-ready repo" playbook to this stack: Acumatica 26.200 (2026 R2), C# customization code,
Modern UI (TypeScript/HTML), SQL scripts, PowerShell tooling. Where the playbook assumed Python, cron and a test
suite, this guide uses the equivalents we actually have (`Test-Compile.ps1`, `Test-ModernUI.ps1`, the user-published
package). Both repos share §0–§10; the last section covers each repo's specifics.

`CLAUDE.md` is the short rule list every session auto-loads. **This guide is the long version it points to.**

---

## 0. The ideas on one screen

1. **`navigation.md` in every folder** describes that folder's direct children and links down. Start at the root one.
2. **`scratchpad.md` is the shared brain**: status, locked decisions, coordination rules, **work claims**, learnings, session log.
3. **One home per kind of thing**: `src/` (customization source), `tools/` (agent tooling), `scripts/` (one-off SQL/data), `docs/` (stable reference), `data/` and `build/` (generated, git-ignored), `exports/` (old snapshots).
4. **One owner per area.** Claim it in `scratchpad.md` before editing; release it when done. Use `agent/<area>` branches.
5. **Source of truth is GitHub, not the running site.** PK9 (`C:\Program Files\Acumatica ERP\PK9`) is runtime. Never edit it (except its navigation `.md` files).
6. **The user publishes, the user approves DB writes.** Agents never publish a customization and never write to the PK9 database without explicit approval *in the current conversation*.
7. **Verify before handing over:** `COMPILE OK` for C#, `TS CHECK OK` for TypeScript.
8. **Record findings in `scratchpad.md`, not just in chat.** Update it before finishing.
9. **Verify notes against code.** Navigation and plan text drifts; code is truth. Mark assumptions as assumptions.

---

## 1. Why this works

| Failure | What it looks like | Countermeasure |
|---|---|---|
| **Cold-start cost** | Each new agent re-reads half the repo to find things | `navigation.md` tree + `scratchpad.md` read order |
| **Collisions** | Two agents edit the same file or screen; a published project overwrites another's items | Work claims, one owner per area, unique file names across projects, branches/worktrees |
| **Drift** | Docs, plan and agents' mental model diverge from the code and from PK9 | Same-commit rule for nav files, session log, "verify against code", `exports/project.xml` flagged stale |

---

## 2. The `navigation.md` system

### 2.1 Shape
```
navigation.md                 ← root: map of the repo, related locations, "I want to… → go to"
├── src/navigation.md         ← what the customization project contains, item naming
├── tools/navigation.md       ← each script: purpose, parameters, read-only vs writer
├── scripts/navigation.md     ← each SQL script: read-only or writer, when it was used
├── docs/navigation.md
├── data/navigation.md        ← KINDS of generated files, never names/contents (client data)
└── exports/navigation.md     ← stale snapshots and why (Project-Task)
```
Each file describes **only its direct children** and links down; information lives once, at the level where it matters.
The existing root files use a table: `| Path | What's there | Agent actions |`. Keep that format; the "Agent actions"
column (Read / Edit here only / Run / Writers need user approval) is what makes it useful to an agent.

### 2.2 Size budget
| Level | Lines | Covers |
|---|---|---|
| Root | 40–200 | Purpose; folder table; related locations (other repos, PK9, URLs); "I want to… → go to" table |
| Branch (`src/`, `tools/`, `scripts/`) | 30–100 | Each child's role; naming rules; how pieces connect (DAC ↔ graph ↔ table XML ↔ Modern UI) |
| Leaf | 10–40 | Per file: purpose, parameters, gotchas |

### 2.3 Put in / keep out
**Put in:** structure, roles, entry points, contracts, invariants, gotchas ("this script writes with `-Execute`"), and
"when you change X, also change Y" (e.g. a new dropdown value → `*_Constants.cs` **and** `*_Messages.cs` **and** the DAC list attribute).

**Keep out:** volatile facts: counts, current status, who is working on what, client names, record IDs. Those live in
`scratchpad.md`. A nav file may say "status: see `scratchpad.md`", never state it.

### 2.4 Same-commit rule
Any add / move / rename / delete under a folder updates that folder's `navigation.md` **in the same commit**. A new
folder gets its own `navigation.md` and a row in its parent. Add "Last verified: YYYY-MM-DD @ <short sha>" to leaf files you check.

### 2.5 Optional checker
Nothing enforces the rule today. If drift becomes a problem, add `tools/Check-Navigation.ps1` (PowerShell 5.1) that
fails when a tracked folder lacks a `navigation.md`, a table row points at a missing path, or a tracked child has no
row. Use `git ls-files`, ignoring `.git`, `.vs`, `build`, `data/*` files. Run it before finishing. Don't claim the check
exists until it does.

### 2.6 "Nav notes" loop for swarms
Don't have every agent rewrite nav files mid-task (collisions). Instead each task report ends with **nav notes** for
each folder it touched. After code tasks land, one agent per disjoint subtree writes the nav files from those notes,
**verifying every claim against the code** and listing discrepancies. Writing nav files against real code is a
systematic review; turn the discrepancy list into a cleanup task.

---

## 3. Folder structure: one home per kind of thing

```
repo/
├── CLAUDE.md          ← ~15 lines every session loads; points to scratchpad.md, navigation.md, this guide
├── GUIDE.md           ← this file
├── navigation.md      ← repo map
├── scratchpad.md      ← plan/status/claims/learnings/session log (shared brain)
├── README.md          ← human overview; no volatile status
├── src/               ← ALL customization source (the only place agents edit product code)
│   ├── Code/          ← C#: DACs, graphs, extensions, constants/messages
│   ├── Tables/        ← table XML for new columns on existing tables
│   ├── Sql/           ← CREATE scripts for new tables
│   ├── SiteMap/       ← screen registration
│   ├── Pages/         ← Classic page shells (needed to register screens)
│   └── FrontendSources/screens/<MOD>/<SCREEN>/   ← Modern UI .ts/.html (+ extensions/)
├── tools/             ← agent tooling (compile check, TS check, package build, read-only SQL, reflection)
├── scripts/           ← one-off SQL / data scripts; classify each as read-only or writer
├── docs/              ← stable reference (runbooks, data facts, UI syntax)
├── data/              ← generated outputs, git-ignored except navigation.md
├── build/             ← generated package zip, git-ignored
└── exports/           ← old snapshots; read-only and stale
```
Rules:
- **Edit source only under `src/`** (plus docs/tools/scripts). Never edit generated folders in PK9: `App_RuntimeCode`,
  `CstPublished`, `FrontendSources\screen\src\customizationScreens`, `Scripts\Screens`, `App_Data\TSScreenInfo`. They are rebuilt on every publish and your edit is lost.
- **Client data never goes in git.** `data/` contents are ignored; nav files describe *kinds* of files.
- **Tools vs scripts:** a *tool* is repeatable agent infrastructure (`tools/*.ps1`); a *script* is a one-off with a date and a purpose. Writers need approval.
- **Shared tooling lives in `Acumatica-PK-TimeClock\tools`** and is used from both repos by relative path (`..\Acumatica-PK-TimeClock\tools\Test-Compile.ps1`). Change it in one place only.

---

## 4. Code conventions that keep agents safe (Acumatica)

- **Naming.** C# namespace and class prefix `PK`. New DB fields `UsrPK…`. Legacy fields from older projects are `UsrPG…`: **leave them alone**, don't rename or retype them (other repos consume them). New tables `PKTimeClock…` (TimeClock) or follow the existing prefix of the owning project.
- **File names must be unique across all customization projects.** `src/Code/<Name>.cs` becomes project item `Code#<Name>`; a duplicate silently overwrites another project's item on publish. Check the taken-names list in `Acumatica-PK-TimeClock\scratchpad.md` → Naming conventions before adding a file, and add yours.
- **Screen IDs:** `PK<MOD><4 digits>`. Check `Pages/PK/` and the site map for collisions before picking one.
- **Modern UI extension files:** `<SCREEN>_<ProjectTag>.ts/.html`, classes `<SCREEN>_<View>_<ProjectTag>`.
- **Confirm any Acumatica API exists before using it:** `tools\Inspect-Acumatica.ps1 <Type> -Member <wildcard>`. For base Modern UI screens read `PK9\FrontendSources\screen\src\screens\<MOD>\<SCREEN>\`. Don't code from memory of an older version.
- **A change that spans layers is one change.** New field = DAC property + table XML column (existing table) or SQL (new table) + Modern UI view/field + constants/messages if it is a dropdown. Do all of it or say what is left.
- **Cross-project dependencies are contracts.** `ProjectTask` owns the `UsrPG*` fields on `PMTask`/`Contract`; `PKTimeClock` reads them and copies the task status/priority lists into `PKTimeClockConstants.cs`. If you change a list or field, update the consumer and its `scratchpad.md`. Never edit another project's code from this repo; log the need under Open items.
- **Fail loudly.** "Not found" is not the same as an error; don't return null/empty for both. Don't swallow exceptions in graph logic.
- **Reflect, don't guess, on upserts and persistence:** confirm whether a call inserts, updates or both before writing a note/record to something you haven't confirmed exists.

---

## 5. `scratchpad.md`: the shared brain

One markdown file per repo. Agents read it first and **update it before finishing**. Keep these sections:

| Section | Purpose |
|---|---|
| Read order | What a new agent reads, in order |
| Goal | 3–6 outcomes |
| **Decisions (locked)** | Decided by the user; **do not re-ask** |
| Status | By phase, with dates and commits; what is published/live vs built-not-published |
| Open items / risks | Includes cross-project requests and decisions only the user can make |
| Releases | Date, package, who deployed where |
| Dependencies on other projects | What must stay published together |
| Development workflow | The numbered steps in §6 |
| Coordination rules | §6 |
| **Work claims** | Table: Area, Claimed by, Since, Notes |
| Naming conventions | §4 (including taken file names) |
| Learnings / gotchas | Environment, data model, engine rules, packaging, Modern UI, data facts |
| Session log | One entry per session: what changed, what was learned, what's next |

Rules for entries:
- **Mark each fact verified or assumed**, with date and how (query, reflection, UI test). An unverified rule written as fact becomes code.
- Date everything absolute (`2026-11-13`), never "yesterday".
- Finish with an **open-items list**, not a claim that everything is done. "Built, compiled, not published" is a different state from "live".

---

## 6. Coordination protocol

- **One owner per area.** Areas: `code/<file>`, `ui/<screen>`, `sql`, `docs`, `tools`, `data`. Check Work claims first; if taken, pick other work or ask the user.
- **Claim → work → release.** Add your row before the first edit; remove it when done (or note the handoff).
- **Branches:** `agent/<area>` per agent (worktrees if parallel); small commits; rebase before merging to `main`. The user decides when to merge. **Never push.**
- **Strict ownership.** If you need a change outside your claimed area (or in the other repo), don't make it; write it as a cross-ownership request in your report and in Open items.
- **Relay interface requirements** to other running agents before they finish (e.g. "the task status list changed; the consumer must follow").
- **Development workflow** (per change):
  1. Claim the area.
  2. Confirm APIs (`Inspect-Acumatica.ps1`, base screen sources).
  3. Edit only under `src/`.
  4. Run `tools\Test-Compile.ps1 -IncludeRuntimeCode` → must print `COMPILE OK`. After any `.ts`/`.html` change run `tools\Test-ModernUI.ps1` → `TS CHECK OK`.
  5. `tools\Build-Package.ps1` → `build\<Project>.zip` (TimeClock).
  6. **The user** imports the zip into PK9 and publishes; verify with `tools\Query-PK9.ps1`, `PK9\App_Data\logs`, and the UI. After the user accepts it, the user deploys to live (`docs/go-live.md`).
  7. Commit, update `scratchpad.md` (status, learnings, session log, Releases), release your claim.
- **Every agent report contains:** summary of changes; commit SHAs/diff stat; compile and TS output; cross-ownership requests; **nav notes**; bugs/discrepancies found outside scope (reported, not fixed).
- **Permission boundaries are real.** If an action is refused or needs approval (publish, DB write, `npm run build`, `Sync-ModernUI.ps1 -Copy`), don't route around it and don't ask another agent to. Give the user the exact command.
- **Approval is per conversation and per action.** Approval to write one thing doesn't cover the next.

---

## 7. Sequencing larger work

| Wave | What | Why |
|---|---|---|
| 0 | User answers decisions; tag/branch a recoverable starting point | Safe rollback |
| 1 | Read-only diagnosis (queries, reflection, reading `App_RuntimeCode`) | Facts before design |
| 2 | Any restructure/rename: **one agent, alone, mechanical, no behaviour change** | Later tasks then work on final paths |
| 3 | Parallel feature tasks with **disjoint claims** (e.g. `code/<A>`, `ui/<B>`, `docs`) | Most of the value |
| 4 | Tasks depending on wave 3, then nav files from nav notes | Nav written from final code |
| 5 | Cleanup from discrepancy lists; verification checklist for the user's first publish; README refresh | Close the loop |

Two tasks in the same wave must not edit the same file. If they must, serialize them.
Plan the publish/cutover for a quiet moment: **publishing restarts the site for everyone.**

---

## 8. Production safety

- **PK9 is the running site. Never edit it** (except its `navigation.md`/`CLAUDE.md`). **Never touch the live site or live database.**
- **Database:** `tools\Query-PK9.ps1` is read-only by default; `-AllowWrite` / `-Execute` need explicit approval in the current conversation. To compile-check a writer SQL script without running it, use `SET NOEXEC ON`.
- **Scripts:** classify every file in `scripts/` as read-only or writer in `scripts/navigation.md`. Writers are listed in bold. Make them idempotent and refuse to overwrite; have them emit an `-OutFile` for the user to run when possible.
- **Send values production already accepts.** For a format or ID you can't test, prefer the proven one, comment the untested alternative, and note it in Open items.
- **Data is CompanyID 2** (tenant "PriceKubecka"); negative CompanyIDs are system/shared rows. Always filter.
- **Don't "fix" what you can't test** (cosmetic changes that risk production). Log it as low priority.
- **Backups before cutover:** record the package/version being replaced so the user can roll back by re-importing it.

---

## 9. Testing & verification

There is no unit-test suite; the gates are:
- `tools\Test-Compile.ps1 -IncludeRuntimeCode` → `COMPILE OK` (compiles your code together with the other projects' published code).
- `tools\Test-ModernUI.ps1` → `TS CHECK OK` (type-check only, writes nothing). Syntax reference: `Acumatica-PK-TimeClock\docs\T290.md`.
- **Read-only data checks** with `Query-PK9.ps1` to confirm assumptions (e.g. the exact values an existing field holds).
- **Verification list for the user:** for any built-not-published change, write the exact steps the user should run on PK9 and what they should see, in `scratchpad.md`.
- Pin invariants in docs: when a rule matters ("clock rules", status lists), state it in the learnings section and say how it was verified.

Gate every handover on: compile OK, TS check OK (if UI touched), nav files updated, `scratchpad.md` updated.

---

## 10. Orchestrator pitfalls (learned the hard way)

- **Notes drift from code.** Ask the next agent to verify and list discrepancies.
- **A rule written without evidence becomes code.** Mark assumptions.
- **Stale snapshots mislead.** `exports/project.xml` (Project-Task) is a 2024 Classic-era export; `src/` is current. Don't trust old exports.
- **Generated folders look like source.** `App_RuntimeCode` contains all published code; read it to see what's live, never edit it.
- **Duplicate file names across projects** overwrite each other on publish (§4).
- **Environment quirks:** shell is Windows PowerShell 5.1 (no `&&`/`||`, no `?:`); **git is not on PATH**, so see `scratchpad.md` → Learnings for how to call it; use absolute paths in commands.
- **Track the user's actions.** They may publish or merge while agents run; check branch state and what's live before writing instructions.
- **Refused action = information.** Hand it to the user; don't launder it through another agent.
- **Finish with open items,** not "done".

---

## Appendix: subagent task prompt template

```text
You are executing task <ID> (<name>) in <repo>.

Setup: read CLAUDE.md, GUIDE.md, scratchpad.md (status, decisions, coordination rules, work claims) and the
navigation.md files down to your area. Claim <area> in Work claims. Work on branch agent/<area>. No push; do not
touch main, other branches, PK9 site files, or other projects' code.

Owns (edit only these): <files/screens>. Anything else goes in your report as a cross-ownership request.

Do: <numbered concrete steps; invariants to preserve; behaviour NOT to change>.

Hard rules: do not publish; no DB writes (read-only Query-PK9.ps1 is fine); edit only under src/ (plus docs/tools/scripts).

Verify: tools\Test-Compile.ps1 -IncludeRuntimeCode (COMPILE OK); tools\Test-ModernUI.ps1 if .ts/.html changed (TS CHECK OK).

Report: commits + diff stat; compile/TS output; design decisions (<=10 lines); cross-ownership requests;
bugs/discrepancies outside scope (reported, not fixed); nav notes per folder touched (roles, invariants, gotchas, no
volatile facts). Update scratchpad.md (status, learnings, session log) and release your claim.
```

---

## Repo specifics: Acumatica-Project-Task-PK

- **Customization project:** `ProjectTask` (**live**). Owns the `UsrPG*` fields on `PMTask` / `Contract` (Preparer, Reviewer, TechReviewer, Task Status, Due Date, Department, statuses), Doc Gathering `PKPM3001` and Sample Testing `PKPM3002`. Modern UI extensions: `PM301000_PKProjectInfo.*` (projects), `PM302000_PKTaskInfo.*` (tasks).
- **Consumer to protect:** `Acumatica-PK-TimeClock` (Preparer Dashboard GI000142, `PKEP3001/3002`) reads these fields and copies the task status (1-9) and priority (1-3) lists. **Do not rename or retype a `UsrPG*` field, or change a list, without checking its `scratchpad.md` and logging the follow-up there.**
- **Master coordination rules** live in `..\Acumatica-PK-TimeClock\scratchpad.md`; this repo's own `scratchpad.md` holds what is live, drift from PK9, dependencies, work claims and session log.
- **Source is current, exports are not:** `src/` was synced from PK9 on 2026-11-13. `exports/` (`project.xml`, zips, import-scenario XML) is a 2024 Classic-era snapshot: read-only and stale (see `exports/navigation.md`).
- **Tooling:** the C# compile check is shared: `..\Acumatica-PK-TimeClock\tools\Test-Compile.ps1 -IncludeRuntimeCode`. Modern UI type-check is local: `tools\Test-ModernUI.ps1`. Read-only SQL: `..\Acumatica-PK-TimeClock\tools\Query-PK9.ps1`.
- **Adding a dropdown value** (record keeper, payroll provider, status) means `src/Code/ProjectTask_Constants.cs` **and** `ProjectTask_Messages.cs` **and** the DAC list attribute.
- **`scripts/`** holds historical one-off DB scripts (2022-2026, table creation/changes). Treat them as writers needing user approval; the CREATE scripts for current tables are in `src/Sql/`.
- **Screen ID gotcha:** `Pages/PK/PKPM3003.aspx` exists (it is PKPM3001's URL); don't reuse `PKPM3003` as a new screen ID.
- **Related:** `..\Acumatica-Employee-Project-Time\` (legacy EP307000 stopwatch, retiring). Do not edit from here.