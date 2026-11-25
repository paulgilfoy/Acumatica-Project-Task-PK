# Agent instructions — Acumatica-Project-Task-PK

**Status: LIVE.** Customization project `ProjectTask` owns the `UsrPG*` fields on PMTask / Contract and the Doc Gathering
(`PKPM3001`) and Sample Testing (`PKPM3002`) screens. `Acumatica-PK-TimeClock` reads these fields — do not rename or
retype them without checking its `scratchpad.md`.

1. Read `scratchpad.md` first (what is live, drift, dependencies, coordination rules, **work claims**).
2. Use `navigation.md` (here) and the `navigation.md` in each folder to find things.
3. Hard rules:
   - Claim your area in `scratchpad.md` before editing; release it when done.
   - Publishing, test data, database and live rules: `..\Acumatica-DevEnv\docs\agent-rules.md` (canonical). In short: agents may
     publish on PK9 after the checks, under the lock `C:\Tools\locks\pk9-publish.lock`; no direct SQL writes without explicit user
     approval in the current conversation; never change live.
   - Edit source only under `src/`. Never edit generated folders in `C:\Program Files\Acumatica ERP\PK9`.
   - After touching `.ts`: `tools/Test-ModernUI.ps1`. After touching `.cs`: this repo's `tools\Test-Compile.ps1 -IncludeRuntimeCode`.
   - Before finishing, update `scratchpad.md` (status, learnings, session log).
4. Shell is Windows PowerShell 5.1; git is on PATH in the Bash tool (in PowerShell see `PLAN.md` §9 for the GitHub Desktop git path).
5. Do not trust `exports/project.xml`: it is a 2024 Classic-era export and is stale (see `exports/navigation.md`). `src/` is current.
6. Working conventions for multi-agent work (navigation system, scratchpad, claims, safety, verification): read `GUIDE.md`.
