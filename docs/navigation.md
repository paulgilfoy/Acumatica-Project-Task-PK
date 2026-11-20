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
| `template-screens.md` | T-31: `UsrPG*` fields on the template screens PM208000 / PM208010 / PM208030 (views, evidence, coexistence with TimeClock's PM208000 extension, checks, manual checklist); values only stored until T-32 |
| `template-copy.md` | T-32: what is copied from templates / common tasks into new projects and tasks (table), due-date rule (month/day rolled forward from the project start) with examples, anchor-date evidence, edge cases (error→warning, inactive employees), switching the rule, checks, manual PK9 test |
| `contact-merge.md` | T-10: what the live `Contact` project contained, where it went, cutover publish, data, rollback, manual checklist |

Shared references live in the PK-TimeClock repo: `..\Acumatica-PK-TimeClock\docs\T290.md` (Modern UI cheat sheet),
`..\Acumatica-PK-TimeClock\docs\reference.md` (GI000142 Preparer Dashboard definition, data facts).
Put long-lived notes about Doc Gathering / Sample Testing here (e.g. business rules per field) as `*.md` and list them in this table.
