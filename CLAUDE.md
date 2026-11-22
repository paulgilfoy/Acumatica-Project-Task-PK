# Agent instructions — Acumatica-Project-Task-PK

**Status: LIVE.** Customization project `ProjectTask` owns the `UsrPG*` fields on PMTask / Contract and the Doc Gathering
(`PKPM3001`) and Sample Testing (`PKPM3002`) screens. `Acumatica-PK-TimeClock` reads these fields — do not rename or
retype them without checking its `scratchpad.md`.

1. Read `scratchpad.md` first (what is live, drift, dependencies, coordination rules, **work claims**).
2. Use `navigation.md` (here) and the `navigation.md` in each folder to find things.
3. Hard rules:
   - Claim your area in `scratchpad.md` before editing; release it when done.
   - Never publish customizations; never write to the PK9 database without explicit user approval in the current conversation.
   - Edit source only under `src/`. Never edit generated folders in `C:\Program Files\Acumatica ERP\PK9`.
   - After touching `.ts`: `tools/Test-ModernUI.ps1`. After touching `.cs`: `..\Acumatica-PK-TimeClock\tools\Test-Compile.ps1 -IncludeRuntimeCode`.
   - Before finishing, update `scratchpad.md` (status, learnings, session log).
4. Shell is Windows PowerShell 5.1. git is not on PATH — see `..\Acumatica-PK-TimeClock\scratchpad.md` → Learnings.
5. Do not trust `exports/project.xml`: it is a 2024 Classic-era export and is stale (see `exports/navigation.md`). `src/` is current.
6. Working conventions for multi-agent work (navigation system, scratchpad, claims, safety, verification): read `GUIDE.md`.
