# tools/ — agent tooling

| Script | What it does | Writes anything? |
|---|---|---|
| `Test-Compile.ps1 -IncludeRuntimeCode` | Compiles **this repo's** `src/Code/*.cs` with PK9's Roslyn against `PK9\Bin`, plus every published file in `PK9\App_RuntimeCode` except our own published copies (incl. `ProjectTask.PMTask.cs`). Prints `COMPILE OK`. `-ExcludeRuntimeFile X` drops another project's file (e.g. `PX_Objects_CR_Contact_extensions` for the T-10 cutover). | No (DLL/rsp in `%TEMP%`) |
| `Test-ModernUI.ps1` | Type-checks `src/FrontendSources/screens/**/*.ts` against PK9's client-controls typings and base screens (PK9's own Node + TypeScript). Prints `TS CHECK OK`. Does not validate HTML. | No (temp tsconfig in `%TEMP%`) |
| `Build-Package.ps1` | Builds `build\ProjectTask.zip` (project.xml + files) from `src\`: Code, Sql, Tables, SiteMap, Automation, PageCustomizations, Pages (ASPX shells), FrontendSources (Modern UI). `-SrcDir` / `-OutFile` to build from another tree. Does not import or publish. | `build\` only |
| `Verify-Package.ps1` | Compares a zip (default `build\ProjectTask.zip`) item by item with the live `ProjectTask` project (CompanyID 2) and the `App_RuntimeCode` copies. Prints `PACKAGE MATCHES LIVE` (exit 0) or `PACKAGE DIFFERS FROM LIVE` (exit 1). | No (read-only SQL) |
| `Get-LiveProject.ps1` | Read-only export of the live project into a src-shaped tree `build\live\src` + `build\live\items.csv`. File bodies come from the PK9 site copies (PK9 keeps uploads in Azure blob storage). Re-sync of `src\` (T-90): `-OutDir <repo>\src`, only when asked. | `build\live\` only |

Order before handing over a package: `Test-Compile.ps1 -IncludeRuntimeCode` → `Test-ModernUI.ps1` → `Build-Package.ps1` → `Verify-Package.ps1`
(a deliberate change shows as DIFF only on the items you changed; anything else is drift).

`Test-ModernUI.ps1` and `Test-Compile.ps1` are adapted from `..\Acumatica-PK-TimeClock\tools\` — keep in sync if those change.
**Do not use `..\Acumatica-PK-TimeClock\tools\Test-Compile.ps1` for this repo:** it compiles the TimeClock repo's own `src\Code`, so it never sees edits made here
(it is still useful to check that PKTimeClock compiles against what is published).
Other tools live there: `Query-PK9.ps1` (read-only SQL; used by the scripts above), `Inspect-Acumatica.ps1` (reflection).
