# Repo navigation — Acumatica-Project-Task-PK

Customization project **`ProjectTask`** (live): PMTask / Contract `UsrPG*` fields, Doc Gathering (`PKPM3001`), Sample Testing (`PKPM3002`).
Start with **`scratchpad.md`** (what is live, drift, rules, work claims); this file is just the map.

| Path | What's there | Agent actions |
|---|---|---|
| `scratchpad.md` | Living status, what is live on PK9, dependencies, coordination rules, **work claims**, learnings, session log | Read first. Claim work. Update before finishing. |
| `README.md` | Human overview | Keep in sync with `src/` |
| `src/` | **All customization source** of the `ProjectTask` project (synced from PK9 on 2026-11-13) | Edit here only. → `src/navigation.md` |
| `scripts/` | Historical one-off DB scripts (table creation/changes, 2022–2026) | Read. Writers need user approval. → `scripts/navigation.md` |
| `exports/` | Old exported packages / project.xml / import-scenario export | Read-only snapshots, **stale**. → `exports/navigation.md` |
| `docs/` | Stable reference | → `docs/navigation.md` |
| `tools/` | Agent tooling (Modern UI type-check) | Run. → `tools/navigation.md` |

## Related locations (outside this repo)
| Location | Purpose |
|---|---|
| `C:\Program Files\Acumatica ERP\PK9\navigation.md` | Map of the running Acumatica site |
| `..\Acumatica-PK-TimeClock\` | **Consumer** of `PMTaskExt.UsrPGPreparer/Reviewer/TechReviewer/TaskStatus/DueDate/Department` and `ContractExt.UsrPGDepartment` etc. (Preparer Dashboard = GI000142, PKEP3001/3002). Master multi-agent plan lives in its `scratchpad.md`. |
| `..\Acumatica-Employee-Project-Time\` | Legacy EP307000 stopwatch (`ProjectTime`) |
| `http://localhost/PK9/` | The running site. Doc Gathering = `?ScreenId=PKPM3001`, Sample Testing = `PKPM3002`, Projects = `PM301000`, Project Tasks = `PM302000` |

## Common tasks → where
| I want to… | Go to |
|---|---|
| Add/change a field on PMTask (Preparer, Reviewer, status…) | `src/Code/PMTask.cs` + `src/Tables/PMTask.xml` (column) + Modern UI `PM301000_PKProjectInfo.*` (grid) / `PM302000_PKTaskInfo.*` (task form) |
| Add/change a field on Projects (Department, statuses…) | `src/Code/Contract.cs` + `src/Tables/Contract.xml` + `PM301000_PKProjectInfo.*` |
| Change Doc Gathering | `src/Code/PMDocGatheringMaint.cs` (graph), `PMDocumentGathering.cs` (DAC), `src/FrontendSources/screens/PK/PKPM3001/`, `src/Sql/PMDocumentGathering.sql` |
| Change Sample Testing | `src/Code/PMSampleTestingMaint.cs`, `PMSampleTesting.cs`, `src/FrontendSources/screens/PK/PKPM3002/`, `src/Sql/PMSampleTesting.sql` |
| Add a dropdown value (record keeper, payroll provider, status) | `src/Code/ProjectTask_Constants.cs` **and** `ProjectTask_Messages.cs` (+ the DAC list attribute) |
| Type-check Modern UI TypeScript | `tools/Test-ModernUI.ps1`; syntax in `..\Acumatica-PK-TimeClock\docs\T290.md` |
| Compile-check C# | `..\Acumatica-PK-TimeClock\tools\Test-Compile.ps1 -IncludeRuntimeCode` |
| Query data (read-only) | `..\Acumatica-PK-TimeClock\tools\Query-PK9.ps1` |
