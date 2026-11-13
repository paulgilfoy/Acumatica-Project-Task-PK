# Acumatica-Project-Task-PK

Acumatica customization project **`ProjectTask`** for PriceKubecka (Acumatica 2026 R2, 26.200, **Modern UI**, site PK9).

> Agents: start with `CLAUDE.md` → `scratchpad.md` → `navigation.md`.

## What it adds
- **Project Task fields** (PMTask): Task Status, Due Date, Extended Due Date / Is Extended, Preparer, Reviewer, Tech Reviewer, Department.
- **Project fields** (Contract): Department, Priority, Is Final, Finish Date, Year End, 401(k) status, Complexity, deliverables and status fields.
- **Document Gathering** (`PKPM3001`) and **Sample Testing** (`PKPM3002`) screens for 401(k) audits (workspace *Projects* → *401(k) Audit*).
- Modern UI extensions of *Projects* (PM301000) and *Project Tasks* (PM302000).

## Layout
| Folder | Contents |
|---|---|
| `src/` | All customization source: `Code/`, `Sql/`, `Tables/`, `SiteMap/`, `Pages/`, `FrontendSources/` (synced from PK9, 2026-11-13) |
| `scripts/` | Historical one-off DB scripts |
| `exports/` | Old exported packages (stale, read-only) |
| `docs/` | Reference notes |
| `tools/` | `Test-ModernUI.ps1` — type-check the Modern UI TypeScript |

Other projects depend on these fields (notably [`Acumatica-PK-TimeClock`](../Acumatica-PK-TimeClock)).
