# Live cutover runbook (T-92, DRAFT)

Status: **ready for the user's review (updated 2026-11-21 ~21:00 with the lead's PK9 verification, section 0). Do NOT start the publish (section 3) until every item in section 1 is ticked;** the backup and exports (section 2.1) can be done any time. The user executes it; the lead
prepares the zip and answers questions. Live is hosted by Acumatica: **no SQL, no `Verify-Package`, UI only.**
Rules: publishing restarts the site for everyone, so pick a quiet time (no one clocked in on the time clock). Allow about 2 hours
with pre-flight; the publish itself is one operation. Tick the boxes as you go and keep notes for the "tell the lead" list (section 6).

What goes live (one zip, `build\ProjectTask.zip`, 57 items): Contact fields merged into `ProjectTask`, Tech Status removed,
Department Statuses screen PKPM2001 + the four project status drop-downs reading from it, new fields on template screens
PM208000/PM208010/PM208030, and the template/common-task copy logic (due dates copied as-is, D9).

---------------------------------------------------------------------------------------------------------------------
## 0. PK9 status: what the lead verified (2026-11-21, ~21:00, after the 11:43 republish)
Everything below was checked by the lead **without the UI** (read-only SQL, file comparisons, automated tests). It cannot replace the
UI tests in section 1; those are marked **YOU**.

| Check | Result |
|---|---|
| Republish 11:43 | Modern UI build exit code `0`; runtime code contains T-21b (save fix), T-21c (cache fix) and T-32b (as-is dates) |
| Zip = PK9 | `build\ProjectTask.zip` (57 items, built from `main` @ `15ee9d4`): `Verify-Package` → **PACKAGE MATCHES LIVE** (0 problems; 3 Classic `pageSource` blobs ignored as designed) |
| Compile / Modern UI types | `Test-Compile -IncludeRuntimeCode` → COMPILE OK; `Test-ModernUI` → TS CHECK OK |
| Due-date logic | `Test-TemplateDates` → DATE TEST PASS (36/36, as-is rule) |
| Seed + sync | `Test-StatusSeed` → SEED TEST PASS. Your PK9 edits are in the table and in `CSAttributeDetail` with identical text/active state: ACCT 11 "Ready for Review" **added** (03:29), ACCT re-sorted (11 first, 8 last) |
| Seed does not overwrite edits | Your 03:27–03:30 ACCT edits survived the 11:43 republish (the seed only inserts missing rows) — so a re-publish on live is safe for edited statuses too |
| Project values vs lists | Every status number stored on a project exists in `PKProjectStatus` → no project will show a bare number in the dropdowns |
| Tech removal | 0 references to TechStatus in `GIResult`, `GIWhere`, `GISort`, `AUScreenFieldState` (PK9) |
| Contact merge | All 8 marketing flag counts **equal the 2026-11-20 baseline** (271/254/298/175/10/437/225/92); no contact changed since 11/12 |
| Menu | `PKPM2001 Department Statuses`, `PKPM3001`, `PKPM3002` present |
| Template data | Common task `03AL` has Due Date 4/15/2027 (you set it 11:43 on PM208030 — the field saves). No template/project yet created from a template after the publish → template copy is **untested** |

**Not yet seen on PK9 (needs YOU before live):** 401K, TAX and AUDIT have **never been saved** on PKPM2001 (their `CSAttributeDetail`
rows still have no SortOrder: the first sync for those 3 departments has not run); no rename, deactivate or duplicate-description test
yet; no project created from a template; no Add Common Task; no Copy Project with a retired status; no UI look at PM301000 / CR302000 /
template screens. Note: PK9 now has **55** statuses (your ACCT 11 test row); **live will show 54** after its publish — that is correct.

---------------------------------------------------------------------------------------------------------------------
## 1. Gate: PK9 must be fully done first
All of these must be ticked by the user (with the lead's confirmation). Do not go live on a partial pass.
- [ ] **NEW (2026-11-21 21:45): the zip now also contains T-24 (PKPM2001 Previous/Next Department arrows), which PK9 has NOT run yet.**
      Republish **`build\ProjectTask-T24.zip`** on PK9 (SHA-256 `A9D3020D…C836`, 88,576 bytes; a saved copy, because `build\ProjectTask.zip` is being rebuilt for T-25), then run the T-24 list in
      `docs/status-screen.md`. The lead then re-runs `Verify-Package` (must say `PACKAGE MATCHES LIVE`) and confirms the hash in section 3.
      Live gets **only** a zip that PK9 has run and passed. (Previous verified zip, without T-24: `AF9171C9…06A1` @ `15ee9d4`; it was overwritten by the rebuild.)
- [x] Before T-24: the zip from `main` @ `15ee9d4` matched PK9 (`PACKAGE MATCHES LIVE`, lead, 2026-11-21 ~21:00; republished 11:43).
- [x] Lead confirmed read-only (21:41): the first sync ran for all four departments (`CSAttributeDetail` SortOrder filled for 401K, TAX, ACCT, AUDIT); test renames reverted.
      Your extra **ACCT 11 "Ready for Review"** exists only on PK9: if you want it on live, add it on live's PKPM2001 after the publish (live will start with the 54 seed rows).
- [1] **YOU:** `docs/status-screen.md` "Retest on PK9": rename (e.g. TAX 13 "Invoice" → "Invoice Sent" → back), toggle Active, add with a
      duplicate description (must be refused) and without, deactivate. Sort order and add: **already passed** (ACCT).
- [1] **YOU:** save once on PKPM2001 for **401K, TAX and AUDIT** (ACCT done). The lead can then confirm the sync read-only.
- [1] **YOU:** GI000142 / the time clock show the new texts (ACCT 02 "Initial Review (Acctg)", and after the saves AUDIT 01 "Ready To Prep", 401K 05 "Initial RECS and Dis").
- [1 ] `docs/status-switch.md` checklist (PM301000 dropdowns, retired status, Copy Project with a retired status, GI000140 / GI641708 show text).
- [1] `docs/contact-merge.md` checklist (PK Marketing tab on CR302000, 8 check boxes, old values intact).
- [1] `docs/tech-removal.md` checklist (no Tech Status on PM301000, other departments fine, GI641645 / Power BI refresh works).
- [ ] `docs/template-screens.md` section 5 checklist (PM208000 / PM208010 / PM208030 fields).
- [ ] `docs/template-copy.md` section 7 manual test, using the as-is due-date rule (D9): a template date of 4/15/2027 gives 4/15/2027 on a new
      project and on Add Common Task, whatever today's date is.
- [1] No open bug reports on PK9 for any of the above.

---------------------------------------------------------------------------------------------------------------------
## 2. Pre-flight on live (UI only)
### 2.1 Backup and safety copies
- [1] Ask Acumatica (hosting) for a **database backup/snapshot** taken right before the publish. Write down the date/time and the reference.
- [1] Note the live Acumatica version/build (Help > About). PK9 runs 26.200.0334. If live differs, tell the lead before continuing.
- [1] Customization Projects (SM204505): write down which projects are **ticked (published)** today, for example `Contact`, `PKTimeClock`,
      `ProjectTask`, `ProjectTime`, `Velixo...`. This is what the PK9 list looked like on 2026-11-20; **live may differ**. You will need this list in section 3.
- [1] **Export the live `ProjectTask` project** (open it, Export/"Download package"). Save the zip as `live-ProjectTask-BEFORE-<date>.zip`.
- [1] **Export the live `Contact` project** the same way: `live-Contact-BEFORE-<date>.zip`.
- 1 ] Export any other published project you are not sure about (at least `PKTimeClock`). Not needed for rollback of this change, but cheap.
- [1] Keep all zips outside the Acumatica server (e.g. your own drive) and give the two `BEFORE` zips to the lead for a diff
      (live `ProjectTask` is expected to match PK9's pre-publish 34 items; **importing the new zip replaces every item of `ProjectTask`, so any extra item that exists only on live would be deleted**). Wait for the lead's "no surprises" before publishing.
- [x] **Lead diff of the live exports (2026-11-21 ~22:40, `Downloads\Live-Site-Customizations`, exported 22:30): NO SURPRISES.**
      - Live `ProjectTask` = **34 items, all present in the new zip** (nothing would be deleted); the new zip adds the 23 known items (57 total).
      - 29 of the 34 are identical to the new zip. The 5 that differ (`Contract` code, `Automation PM301000`, Classic pages `pm301000`/`pm302000`,
        and the PM301000 Modern UI `.html/.ts`) are **byte-for-byte (whitespace-insensitive) equal to our pre-change source** (git `4bbde0f`, synced from PK9 on
        2026-11-20): the only differences are our intended changes (Tech removal, status list, Classic re-sync). **No live-only edits are lost.**
        `PKPM3002.aspx`/`PKPM3003.aspx` differ only in whitespace/line endings.
      - **Compile test of live's other projects with the new `ProjectTask`:** the code of live `PKTimeClock` (15 code items), `ProjectTime` (1 + its 2 DAC
        items) and `Velixo2025R1OrOlder` (18) compiled together with the new zip's 14 code items against PK9 26.200 → **exit 0** (Contact left out, as in section 3).
        None of them references Tech Status or the Contact fields. Note: live `PKTimeClock` (38 items) is OLDER than PK9's (46) and live `PKTimeClock` needs
        `ProjectTime` (its `PMTimeActivityExt` fields): **keep `ProjectTime` ticked on live** if it is ticked today.
      - Live build `product-version=26.200` (same major as PK9). Not checkable from exports: the Modern UI (HTML) build of live's older PKTimeClock next to
        the new screens — PK9's build of the same screens passed; watch the publish log for a failed npm step.
        User (2026-11-21): live's current Modern UI works and was tested extensively, so live's existing set of projects builds and runs today; the only
        remaining unknown is the combined build at publish time (old live PKTimeClock + new ProjectTask screens). Low risk: the new screens' extensions
        target PM208000/PM208010/PM208030/PM301000/PM302000/CR302000/PKPM2001, and PK9 built them next to PKTimeClock without errors.

### 2.2 Remove Tech Status from live Generic Inquiries (blocking)
A GI that still names the removed field (`PMProject.usrPGTechStatus`) will error or show a blank column after the publish, and Power BI refreshes fail.
GIs known on PK9 (names on live may differ; the user already cleaned PK9): **GI000140** (PK - Project Status), **GI641660**, **GI641709**,
**GI641645 "PowerBI - Project Revenue"** (a result column and Conditions line 8 `PMProject.usrPGTechStatus = 4`, inactive but still names the field;
delete it, the brackets stay balanced), and three GIs that were unpublished on PK9: `Preparer Time Sapro`, `PK - Preparer Time Sapro Manager`, `PK Open Project`.
The UI has no "search by field" for GIs, so use one of these ways:
- [ ] **Way A (reliable, covers every GI):** on Customization Projects (SM204505) create a throwaway project `ZZ-GISearch`, add **all** Generic Inquiries
      to it (Add > Generic Inquiry, select all), **Export** the zip, unzip it and search the XML files for `TechStatus` (any text editor "find in files",
      case-insensitive). The GI names that hit are the ones to clean. Then **delete the throwaway project** (never publish it).
- [ ] **Way B (by hand):** open each GI named above in SM208000 (Generic Inquiry). Check the tabs **Results Grid**, **Conditions**, **Sort Order**, **Group By**,
      and **Relations**, plus **Navigation** and the **Filter** parameters. Delete every line that names `TechStatus` / `UsrPGTechStatus`. Use Way A to be sure none is missed.
- [ ] After cleaning, check the GIs still **run** (open each one, click refresh). Also look at Business Events (SM302050) and Import/Export scenarios
      (SM206025 / SM206036) for any mention of Tech Status.
- [ ] Re-run Way A on the throwaway project (or just search again) and confirm **zero** hits. Note the cleaned GI names for section 6.

### 2.3 Other live checks
- [ ] **Screen ID PKPM2001 is free.** In the Acumatica menu search (or SM200520 Site Map) search `PKPM2001`: nothing should exist. Also search `Department Statuses`.
      (If live already has `PKPM2001`, stop and tell the lead.)
- [ ] **The live tenant ID is unverified.** On PK9 the data is CompanyID 2 and the seed script picks every company that has the attribute `STATUSTAX`. Check on live
      that you only have the one data tenant you expect (Tenants list) and that attribute `STATUSTAX` exists on CS205000 (Attributes) with 15 values. If there are several tenants, tell the lead.
- [ ] Spot-check that live has the same status texts as PK9 had before (so the seed does not change meanings): open CS205000 for `STATUS401K`, `STATUSTAX`,
      `STATUSACCT`, `STATUSAUDI` and compare counts (401K 20 values, TAX 15, ACCT 9 incl. `00 Not Started`, AUDIT 11).
- [ ] Note the **role names** on live (SM201005 / SM201020). PK9 used `Administrator`, `PK - Admin`, `PK - Partner`; the names on live may differ.
- [ ] Optional: take baseline contact counts for the 8 marketing flags (the PK9 numbers are in `docs/contact-merge.md`), e.g. through a Contacts GI with a count per flag, to compare after the publish.
- [ ] Announce the downtime window; confirm nobody is clocked in on the time clock.

---------------------------------------------------------------------------------------------------------------------
## 3. Publish steps
**Never publish `Contact` together with the new `ProjectTask`** (both define `ContactExt`: CS0101 compile error).
- [ ] SM204505 Customization Projects: open **`ProjectTask`** > **Import** the zip built by the lead (the lead gives you the file and commit id; do not use an older zip). Choose replace.
      The zip to use is `build\ProjectTask-T24.zip` (copy of the T-24 build), 88,576 bytes, written 2026-11-21 21:42:31, SHA-256
      `A9D3020D28D1487D846C7655E9CFB8BACAA453BB676CF23F3FC3CD0FBA60C836` (PowerShell: `Get-FileHash <zip>`) — **only after PK9 ran it and the
      lead confirmed `PACKAGE MATCHES LIVE` against PK9** (gate, section 1). If the lead rebuilds it later, use the new hash the lead gives you.
      Alternatively export `ProjectTask` from PK9 and import that: same content.
- [ ] Back in the project list, **tick `ProjectTask`**, **UNTICK `Contact`**, and **keep every other project ticked** that you noted in 2.1 (including **`PKTimeClock`**, `ProjectTime`, Velixo/others). Do not tick anything that was not ticked before.
- [ ] **Publish** the selected projects once (one operation). Wait for the site restart and the Modern UI build to finish. If it fails with a compile error mentioning `ContactExt` / CS0101, `Contact` is still ticked: untick it and publish again.
- [ ] If the publish fails for another reason: do not retry blindly. Copy the full error text (and screenshot) and send it to the lead; a failed publish leaves the previous state in place.
- [ ] Do not delete the live `Contact` project yet (see rollback).

---------------------------------------------------------------------------------------------------------------------
## 4. Post-publish checks
### 4.1 Right away (data and screens)
- [ ] SM204505: `Contact` shows not published; `ProjectTask` and `PKTimeClock` published, no warnings.
- [ ] **PKPM2001 Department Statuses** (Projects > Preferences) opens in Modern UI. Counts per department: **401K 20, TAX 15, ACCT 8, AUDIT 11 = 54 rows.**
      If a department is empty or all empty, the seed missed your tenant: **stop and tell the lead** (do not add rows by hand before asking). Descriptions match PM301000 (401K 17 has a double space; ACCT 2 "Initial Review (Acctg)", AUDIT 1 "Ready To Prep", 401K 5 "Initial RECS and Dis").
- [ ] **Access rights:** SM201020 (Access Rights by Screen) > screen PKPM2001: set `*` = Revoked, then **Delete** (full) for Administrator, PK - Admin, PK - Partner (and any other managers). Use live's role names from 2.3. A user without rights must not see the menu entry.
- [ ] **Save once per department** (this copies the table into `CSAttributeDetail` for GIs and the time clock): on PKPM2001 select 401K, click Save; then TAX, ACCT, AUDIT. If Save is disabled with no changes, toggle a field and toggle it back, then Save (workaround in `docs/status-screen.md`). Afterwards CS205000 should show ACCT 02 "Initial Review (Acctg)", AUDIT 01 "Ready To Prep", 401K 05 "Initial RECS and Dis", and GI000142 / the time clock show the same texts.
- [ ] Rename-and-back test on one status (e.g. TAX 13 "Invoice" > "Invoice Sent" > back), save each time.

### 4.2 Screens
- [ ] **PM301000 Projects:** one project per department (401K, TAX, ACCT, AUDIT) shows the same status text as before; each drop-down has the full list (401K 20, TAX 15, ACCT 8, AUDIT 11) in order; change one value, save, change back. **No Tech Status field anywhere** (including a TECH-department project). Tasks tab still shows Preparer, Reviewer, Tech Reviewer, Due Date, Task Status.
- [ ] **PK Status (T-25, if the zip contains it):** on a few projects (one each of TAX, ACCT, AUDIT, 401K and one ADMIN/TECH) the read-only *PK Status* in *PK Project Info* equals the department's status text (empty for ADMIN/TECH); change a status, save: PK Status follows. Details: `docs/pk-status.md`.
- [ ] **CR302000 Contacts:** **PK Marketing** tab right after **General**, group *Marketing Attributes* with 8 check boxes; a known contact still has its boxes ticked; tick/untick one, save, reload (F5), restore. Compare with the baseline counts if you took them.
- [ ] **Template screens:** PM208000 (fieldset *PK Project Info*: Department, Priority, Complexity; Tasks grid: Due Date, Preparer, Reviewer, Tech Reviewer), PM208010 and PM208030 (*PK Task Info*). TimeClock's *Default Labor Item* still shows on PM208000. Enter test values on a **test** template (copy one first) and save.
- [ ] **Template copy (due dates as-is, D9):** on the test template put Due Date 04/15/2027 on a task and a Preparer; create a new project from it: Department/Priority/Complexity and task Preparer/Reviewer/Tech Reviewer copied, **Due Date = 04/15/2027 exactly** (not rolled). **Add Common Task** gives the template date unchanged as well. A blank template value leaves the field empty. Delete the test project and test template afterwards.
- [ ] **Copy Project with a retired status:** on PKPM2001 clear Active on one status in use by a project (e.g. TAX 13), save; Copy Project on that project: the copy is created and holds the retired status. Delete the test copy. **Re-activate the status afterwards.**
- [ ] Doc Gathering (PKPM3001), Sample Testing (PKPM3002), PM302000 Project Tasks, EP307000 / time clock (PKEP3001/3002) still open and show the PK fields. Preparer Dashboard GI000142 lists tasks with Tech Reviewer.

### 4.3 Generic Inquiries, OData, Power BI
- [ ] GI000140 and GI641708 show status **text** (GI000140 "Raw401kStatus" stays a number); GI641645 / Power BI Project Revenue refreshes without a field error. Refresh all Power BI / OData consumers once and check for errors.
- [ ] Open the GIs you cleaned in 2.2 and the 6 GIs that join the status attributes (GI000142/137/143/144, GI993814, GI641674); all run.
- [ ] Business events that test status numbers (6 active on PK9) still trigger as expected (check one by changing a test project, or just confirm they are not erroring).
- [ ] Watch the next day: users' first reports (statuses missing, blank drop-down, Power BI failures).

---------------------------------------------------------------------------------------------------------------------
## 5. Rollback plan
Data is not at risk from a rollback: no step deletes project data, the 8 Contact columns and Tech Status column stay in the database, and `PKProjectStatus` and `CSAttributeDetail` rows stay (harmless).
If a post-publish check fails badly (or the publish breaks the site):
- [ ] Decide quickly with the lead; for a data/problem that is only a GI error, fix the GI instead of rolling back.
- [ ] **Re-import the exported `live-ProjectTask-BEFORE` zip into `ProjectTask`** (replace), then tick `ProjectTask`.
- [ ] **Tick `Contact` again** (its project still exists, because you did not delete it). **Order matters:** the old `ProjectTask` has no `ContactExt`, so ticking both is only valid *after* the old `ProjectTask` is the one imported. Never tick `Contact` while the new `ProjectTask` is still imported (CS0101). If unsure, import the old `ProjectTask` first, then tick `Contact`, then publish once.
- [ ] If `Contact`'s own definition was lost, import `live-Contact-BEFORE` into `Contact`.
- [ ] Publish the same project set you noted in 2.1.
- [ ] After a rollback: PM301000 shows the hard-coded status texts again; statuses added on PKPM2001 (IDs above the old ranges) show as numbers; Tech Status is back as a field, but GIs you cleaned stay cleaned (re-add their Tech Status lines by hand only if needed).
- [ ] Optionally rename ACCT 02 / AUDIT 01 / 401K 05 on CS205000 back to "Initial Review" / "Ready to Prep" / "Initial RECs and DIs" and clear Sort Order if you want the pre-change text (see `docs/status-screen.md` rollback).
- [ ] Do not delete `Contact` or the `PKProjectStatus` table. Only after the cutover has been stable for a while (and the lead agrees) delete the live `Contact` project (never re-tick it).
- If the site does not come back at all: ask Acumatica hosting to restore the snapshot from 2.1.

---------------------------------------------------------------------------------------------------------------------
## 6. Tell the lead
Report after each phase:
- [ ] The commit id / date of the zip you imported, and the live Acumatica build number.
- [ ] Which projects were ticked before and after (names).
- [ ] The two `BEFORE` zips (or confirmation the lead has diffed them) and any item in live `ProjectTask` that was not in the new zip.
- [ ] GI cleanup: the GI names where `TechStatus` was found and removed, and the final zero-hit search result.
- [ ] Publish result: success or the exact error text/screenshot; whether `Contact` was unticked.
- [ ] PKPM2001 row counts per department (expected 401K 20, TAX 15, ACCT 8, AUDIT 11) and, if not matching, the tenant list.
- [ ] Roles you gave PKPM2001 rights to and whether the first save per department worked.
- [ ] Results of each check in section 4 (pass/fail), in particular PM301000 drop-downs, CR302000 tab, template copy due dates, Copy Project with a retired status.
- [ ] GI / Power BI refresh results and any error messages from users in the first day.
- [ ] Whether you want the live `Contact` project deleted now or later; whether the Contact repo may be archived.
- [ ] Tell the lead to start T-90/T-91 follow-ups (navigation notes) after the live result.
