# scripts/ — historical DB scripts (moved from `DBScripts/`, 2026-11-13)

One-off SQL used while creating the two new tables. **Not run by anything.** The authoritative, idempotent scripts that
Acumatica runs on publish are `../src/Sql/*.sql` (these may differ from the files here).

| File | Purpose |
|---|---|
| `DocumentGathering.sql`, `DocumentGathering2.sql`, `DocumentGathering3.sql`, `DocumentGathering_UPDATE.sql` | Original (2022, hard-coded `USE [Acumatica22202]`) create/alter/rename for `PMDocumentGathering` |
| `PMDocumentGathering-2026.sql` | 2026 cleaned version for the project's Database Scripts tab |
| `SampleTesting.sql`, `SampleTesting-2026.sql` | Same for `PMSampleTesting` (2026 version is the cleaned one) |

Writers (anything that changes data/schema) need explicit user approval in the current conversation.
