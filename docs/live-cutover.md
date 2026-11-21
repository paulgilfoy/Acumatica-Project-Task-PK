# Live cutover runbook (T-92, DRAFT)

Status: **draft, prepared 2026-11-21. Do NOT start until every item in section 1 is ticked.** The user executes it; the lead
prepares the zip and answers questions. Live is hosted by Acumatica: **no SQL, no `Verify-Package`, UI only.**
Rules: publishing restarts the site for everyone, so pick a quiet time (no one clocked in on the time clock). Allow about 2 hours
with pre-flight; the publish itself is one operation. Tick the boxes as you go and keep notes for the "tell the lead" list (section 6).

What goes live (one zip, `build\ProjectTask.zip`, 57 items): Contact fields merged into `ProjectTask`, Tech Status removed,
Department Statuses screen PKPM2001 + the four project status drop-downs reading from it, new fields on template screens
PM208000/PM208010/PM208030, and the template/common-task copy logic (due dates copied as-is, D9).

---------------------------------------------------------------------------------------------------------------------
## 1. Gate: PK9 must be fully done first
All of these must be ticked by the user (with the lead's confirmation). Do not go live on a partial pass.
- [ ] The lead rebuilt `build\ProjectTask.zip` **from `main`** (including the T-21b PKPM2001 save fix and T-32b as-is due dates) and
      ran `tools\Verify-Package.ps1` against PK9: result is `PACKAGE MATCHES LIVE` (PK9), and the lead gave you the commit id of the zip.
- [ ] That exact zip is the one currently published on PK9 (the republish of the fixed zip was done and retested).
- [ ] `docs/status-screen.md` checklist (PKPM2001) and its "Retest on PK9" list: rename, toggle Active, add (with and without a duplicate
      description), deactivate, first save per department. All pass, and the `CSAttributeDetail` sync was seen working (GI000142 shows the renamed text).
- [ ] `docs/status-switch.md` checklist (PM301000 dropdowns, retired status, Copy Project with a retired status, GI000140 / GI641708 show text).
- [ ] `docs/contact-merge.md` checklist (PK Marketing tab on CR302000, 8 check boxes, old values intact).
- [ ] `docs/tech-removal.md` checklist (no Tech Status on PM301000, other departments fine, GI641645 / Power BI refresh works).
- [ ] `docs/template-screens.md` section 5 checklist (PM208000 / PM208010 / PM208030 fields).
- [ ] `docs/template-copy.md` section 7 manual test, using the as-is due-date rule (D9): a template date of 4/15/2027 gives 4/15/2027 on a new
      project and on Add Common Task, whatever today's date is.
- [ ] No open bug reports on PK9 for any of the above.

---------------------------------------------------------------------------------------------------------------------
## 2. Pre-flight on live (UI only)
### 2.1 Backup and safety copies
- [ ] Ask Acumatica (hosting) for a **database backup/snapshot** taken right before the publish. Write down the date/time and the reference.
- [ ] Note the live Acumatica version/build (Help > About). PK9 runs 26.200.0334. If live differs, tell the lead before continuing.
- [ ] Customization Projects (SM204505): write down which projects are **ticked (published)** today, for example `Contact`, `PKTimeClock`,
      `ProjectTask`, `ProjectTime`, `Velixo...`. This is what the PK9 list looked like on 2026-11-20; **live may differ**. You will need this list in section 3.
- [ ] **Export the live `ProjectTask` project** (open it, Export/"Download package"). Save the zip as `live-ProjectTask-BEFORE-<date>.zip`.
- [ ] **Export the live `Contact` project** the same way: `live-Contact-BEFORE-<date>.zip`.
- [ ] Export any other published project you are not sure about (at least `PKTimeClock`). Not needed for rollback of this change, but cheap.
- [ ] Keep all zips outside the Acumatica server (e.g. your own drive) and give the two `BEFORE` zips to the lead for a diff
      (live `ProjectTask` is expected to match PK9's pre-publish 34 items; **importing the new zip replaces every item of `ProjectTask`, so any extra item that exists only on live would be deleted**). Wait for the lead's "no surprises" before publishing.

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
