# docs/ — stable reference material

Living information (status, drift, decisions, learnings) belongs in `../scratchpad.md`.

| File | What |
|---|---|
| `inventory.md` | T-01 read-only inventory (Tech status uses, template/common-task flow, open questions) |
| `status-lists.md` | T-01: `CSAttributeDetail` status rows vs the `PXIntList`s |
| `tech-removal.md` | T-11: Tech Status / TECH display removed (P2), column kept, GI-first publish order, re-check SQL, rollback, manual checklist |
| `status-design.md` | T-20: approved design of the status maintenance (table, list attribute, CSAttributeDetail sync option C, PKPM2001, decisions D1-D8) |
| `status-screen.md` | T-21: what was built for PKPM2001 / `PKProjectStatus` / seed / sync, publish steps, SM201020 rights, rollback, manual checklist, what needs a publish to verify |
| `status-switch.md` | T-22: the 4 project status fields switched from `PXIntList` to `PKProjectStatusList("<dept>")`; Copy Project fix for retired statuses (IL evidence), PM301000/Automation/GI/TimeClock findings, publish order, empty-table behavior, rollback, manual checklist |
| `template-screens.md` | T-31: `UsrPG*` fields on the template screens PM208000 / PM208010 / PM208030 (views, evidence, coexistence with TimeClock's PM208000 extension, checks, manual checklist); copied into new projects by T-32 |
| `template-copy.md` | T-32: what is copied from templates / common tasks into new projects and tasks (table), due-date rule (as-is since T-32b/D9; the roll-forward option C kept as an alternative) with examples, anchor-date evidence, edge cases (error→warning, inactive employees), switching the rule, checks, manual PK9 test |
| `pk-status.md` | T-25: read-only "PK Status" on the project (`Contract.UsrPKStatusID` / `UsrPKStatus` = status of the project's own department): how it stays current (RowPersisting attribute, PKPM2001 rename UPDATE), backfill/recalculate SQL, publish order of Table vs Sql items (IL), fast-publish note, preview numbers, GI usage, manual PK9 test |
| `common-tasks-brief.md` | T-40-prep: fact brief for the common-task design session (common tasks on PK9, items by type, template budget lines, what Add Common Task copies, hooks, design options, question list, SQL appendix). T-40..43 were removed from the plan 2026-11-25; kept for reference |
| `contact-merge.md` | T-10: what the live `Contact` project contained, where it went, cutover publish, data, rollback, manual checklist |
| `live-cutover.md` | T-92: live cutover runbook (gate, UI-only pre-flight incl. live GI Tech cleanup, publish with Contact unticked, post-publish checks, rollback, report-back list) and release plan; releases 1–3 done (2026-11-25), kept as the template for later live releases |

Shared references live in the PK-TimeClock repo: `..\Acumatica-PK-TimeClock\docs\T290.md` (Modern UI cheat sheet),
`..\Acumatica-PK-TimeClock\docs\reference.md` (GI000142 Preparer Dashboard definition, data facts).
Put long-lived notes about Doc Gathering / Sample Testing here (e.g. business rules per field) as `*.md` and list them in this table.
