# exports/ — old exports (read-only, **stale**)

| File | What it is |
|---|---|
| `project.xml` | Customization project XML, `product-version="21.211"` (2022–2024, Classic UI): customized Classic pages (pm208000/208010/301000/302000), automation, graph `PMTaskExtensions`, tables Contract / EPEmployeeContract / PMTask. **Does not match what is live** (live has more code items, Modern UI files, site map nodes, two SQL scripts). |
| `ProjectTask.zip` | Export containing `project.xml` + `Pages\PK\PKPM3002/3003.aspx(.cs)` |
| `ProjectTask240227.zip`, `ProjectTask240420.zip` | Feb/Apr 2024 exports incl. a compiled `Bin\ProjectTask_Code.dll` |
| `PKPM3001-PKPMD.xml` | Export of an Import/Export mapping (SYMapping) for the Doc Gathering screen (`PKPM3001`) |

Current source of truth: `../src/` (taken from the PK9 DB on 2026-11-13). Do not hand-edit these. If a deployable package is needed,
re-export from PK9 (Customization Projects → Export) or build one like `..\Acumatica-PK-TimeClock\tools\Build-Package.ps1` does.
