# tools/ — agent tooling

| Script | What it does | Writes anything? |
|---|---|---|
| `Test-ModernUI.ps1` | Type-checks `src/FrontendSources/screens/**/*.ts` against PK9's real client-controls typings and base screens (PK9's own Node + TypeScript). Prints `TS CHECK OK`. Does not validate HTML. | No (temp tsconfig in `%TEMP%`) |

Copied from `..\Acumatica-PK-TimeClock\tools\` — keep in sync if that one changes.
Other tools (compile check, read-only SQL, reflection) live there: `Test-Compile.ps1`, `Query-PK9.ps1`, `Inspect-Acumatica.ps1`.
