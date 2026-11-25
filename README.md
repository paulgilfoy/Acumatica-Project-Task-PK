# Acumatica-Project-Task-PK

Acumatica customization project **`ProjectTask`** for PriceKubecka (Acumatica 2026 R2, 26.200, **Modern UI**, site PK9).

> Agents: start with `CLAUDE.md` → `scratchpad.md` → `navigation.md`.

## What it adds
- **Project Task fields** (PMTask): Task Status, Due Date, Extended Due Date / Is Extended, Preparer, Reviewer, Tech Reviewer, Department.
- **Project fields** (Contract): Department, Priority, Is Final, Finish Date, Year End, 401(k) status, Complexity, deliverables and department status fields (401k, Tax, Acct, Audit; Tech Status was removed in T-11, see `docs/tech-removal.md`).
- **Document Gathering** (`PKPM3001`) and **Sample Testing** (`PKPM3002`) screens for 401(k) audits (workspace *Projects* → *401(k) Audit*).
- **Department Statuses** (`PKPM2001`, workspace *Projects* → *Preferences*; T-21; department arrows T-24): maintain the 401K/TAX/ACCT/AUDIT project status lists in table `PKProjectStatus` (seeded from today's lists); saving keeps the `CSAttributeDetail` STATUS* rows used by GIs in step. See `docs/status-screen.md`. The four project status drop-downs read this table (T-22, `docs/status-switch.md`).
- Modern UI extensions of *Projects* (PM301000) and *Project Tasks* (PM302000).
- **PK Status** (T-25): read-only project field = the status of the project's own department, usable in GIs. See `docs/pk-status.md`.
- **Template values** (T-31/T-32): PK fields on project templates, template tasks and common tasks (PM208000/PM208010/PM208030); new projects and added common tasks get them when empty, due dates copied as-is (D9). See `docs/template-copy.md`.
- **Contact marketing fields** (*PK Marketing* tab on *Contacts*, CR302000): 8 check boxes. Merged from the former `Contact` project; see `docs/contact-merge.md`.

## Layout
| Folder | Contents |
|---|---|
| `src/` | All customization source: `Code/`, `Sql/`, `Tables/`, `SiteMap/`, `Automation/`, `PageCustomizations/`, `Pages/`, `FrontendSources/` |
| `scripts/` | Historical one-off DB scripts |
| `exports/` | Old exported packages (stale, read-only) |
| `docs/` | Reference notes |
| `tools/` | Compile and Modern UI checks, package build/verify, tests (see `tools/navigation.md`) |

Status and plan: `PLAN.md` and `scratchpad.md`. Agent rules: `..\Acumatica-DevEnv\docs\agent-rules.md`.

Other projects depend on these fields (notably [`Acumatica-PK-TimeClock`](../Acumatica-PK-TimeClock)).
