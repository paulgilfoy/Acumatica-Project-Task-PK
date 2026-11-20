# Contact merge (T-10) — cutover, data, rollback, test checklist

Status: **source merged into `src/`, not published.** Written 2026-11-20 by the T-10 worker.
The live customization project **`Contact`** (CompanyID 2) is now fully contained in `ProjectTask`. After the cutover
publish below, `Contact` is no longer needed and the `PriceKubecka-Acumatica-Contact` repo can be archived.

## What the live `Contact` project contains (read-only export 2026-11-20) and where it went

| # | Live item (`CustObject` Type / Name) | What it is | Now in this repo | Carried as |
|---|---|---|---|---|
| 1 | `DAC` / `PX.Objects.CR.Contact` | Data Class editor item: 8 `bool` fields, `[PXDBBool]` + `[PXUIField(DisplayName=…)]`, `StorageName="AddColumn"`. Acumatica generates `App_RuntimeCode\PX_Objects_CR_Contact_extensions.cs` (`PX.Objects.CR.ContactExt : PXCacheExtension<Contact>`) from it. | `src/Code/PKContactExt.cs` | **Code item** `Code#PKContactExt` (`FileType="NewFile"`). The body is a byte-for-byte copy of the generated `PX_Objects_CR_Contact_extensions.cs`, so namespace, class name, field types and attributes are identical. |
| 2 | `Table` / `Contact` | 8 `bit NULL` columns on table `Contact` | `src/Tables/Contact.xml` | verbatim |
| 3 | `Page` / `~/pages/cr/cr302000.aspx` | Classic screen-editor customization: *PK Marketing* tab with group *Marketing Attributes* and 8 check boxes (Classic UI only) | `src/PageCustomizations/CR302000.xml` | verbatim |
| 4 | `PerTenantFile` / `screens\CR\CR302000\extensions\CR302000_PKMarketing.ts` | Modern UI: declares the 8 fields on the `ContactDetail` view class | `src/FrontendSources/screens/CR/CR302000/extensions/` | verbatim copy of the live site file |
| 5 | `PerTenantFile` / `…\CR302000_PKMarketing.html` | Modern UI: *PK Marketing* tab after *General*, fieldset bound to view `ContactCurrent` | same folder | verbatim copy of the live site file |

The 8 fields: `UsrPGPortalAccess` (Portal Access), `UsrPG1040ES` (1040-ES Reminder), `UsrPG1040Check` (1040 Checklist),
`UsrPGTaxPlan` (Tax Planning Meeting), `UsrPGDoNotMarket` (Do Not Market), `UsrPGAcuMarket` (Acumatica Marketing List),
`UsrPGRetire` (PK Retirement Prospect), `UsrServiceProviderQ` (Service Provider Questionnaire).

Nothing in `Contact` was left out. The project has no SQL scripts, site map nodes, Automation (workflow) items, graph
extensions, reports or other files. No other customization project defines `ContactExt` or any of these fields: checked
in `App_RuntimeCode` and in `CustObject` of every project.

**Why a Code item instead of the DAC item:** the plan (T-10) asked for an ordinary code file, like `Contract.cs` / `PMTask.cs`
in this project. Because the code equals what Acumatica generates from the DAC item, the compiled result is the same.
One difference: in the Customization Project Editor the fields now appear under **Code** (`PKContactExt`), not under **Data Access**.

### Modern UI: no view/tab mismatch in the live files
The Contact repo has two variants. `CR302000_PKMarketing.*` there are identical to live. The stale `Contact_converted.*`
use `after="#tabDetails"` / `view.bind="ContactDetail"`, which does not exist on the screen. The **live** files
(kept unchanged here) are correct for 26.200:
- the base `CR302000.ts` declares `ContactCurrent = createSingle(ContactDetail)`. `ContactCurrent` is the **view**
  (used in the HTML), and `ContactDetail` is its **class** (extended in the `.ts`). That is the standard pattern, not a mismatch.
- `#tabGeneral` exists in the base `CR302000.html` (`ContactCurrent_tab` tab bar); `#tabDetails` does not.
- `@placeAfterProperty("ConsentExpirationDate")` refers to a real `ContactDetail` property.
- Evidence it works as deployed: the last Modern UI build (`App_Data\logs\npm-2026-11-17-14-37-34-*`) exited 0, and
  `App_Data\TSCompleteScreens\customizationScreens\Company\CR302000.html` shows the merged *PK Marketing* tab right after *General*.

## Cutover (user, one publish — bundle A)
Both projects declare `PX.Objects.CR.ContactExt` and the same 8 fields. **Never publish `Contact` and the new `ProjectTask` together.**
That fails with `CS0101: The namespace 'PX.Objects.CR' already contains a definition for 'ContactExt'`, as reproduced
by `tools\Test-Compile.ps1 -IncludeRuntimeCode` without the exclusion.

1. Back up first: export the current `ProjectTask` and `Contact` projects from SM204505 (download package). Take a DB backup if available.
2. Run the checks (or have the lead re-run them): `tools\Test-Compile.ps1 -IncludeRuntimeCode -ExcludeRuntimeFile PX_Objects_CR_Contact_extensions`,
   `tools\Test-ModernUI.ps1`, `tools\Build-Package.ps1`, `tools\Verify-Package.ps1 -ExtraProject Contact` → `PACKAGE MATCHES LIVE`.
3. **Customization Projects (SM204505)** → *Import* `build\ProjectTask.zip` into the existing `ProjectTask` project (replace contents).
4. In the project list, **tick** `ProjectTask` and keep ticked every other project that is published today. Before you start, note
   which ones are ticked; on 2026-11-20 the CompanyID 2 list was `Contact`, `PKTimeClock`, `ProjectTask`, `ProjectTime`,
   `Velixo2025R1OrOlder`. Then **untick `Contact`**.
5. *Publish* the selected projects in one operation. Wait for the site restart and the Modern UI build.
6. Run the manual checklist below.
7. Afterwards: leave `Contact` unpublished; it can be deleted from SM204505 once the checklist passes. Then archive
   the `PriceKubecka-Acumatica-Contact` repo. T-90 re-extracts `src/` and T-91 updates the PK9 `navigation.md`.

After the publish, `App_RuntimeCode\PX_Objects_CR_Contact_extensions.cs` should be gone and a `PKContactExt.cs` should be
there. From then on, `Test-Compile.ps1 -IncludeRuntimeCode` needs no exclusion, and `Verify-Package.ps1` without
`-ExtraProject` should report `PACKAGE MATCHES LIVE`.

## What happens to the data
- The 8 columns stay in table `Contact`. Acumatica never drops columns on unpublish, and `ProjectTask` now declares the
  same columns (`Table` item, same type `bit NULL`), so publishing adds nothing and changes nothing.
- The field names are unchanged, so existing values, Generic Inquiries, import/export scenarios and reports that use
  `Contact.UsrPG…` keep working.
- Baseline counts on 2026-11-20 (CompanyID 2, 10,814 contacts), for comparison after publish:

  | Field | Rows = true |
  |---|---|
  | UsrPGPortalAccess | 271 |
  | UsrPG1040ES | 254 |
  | UsrPG1040Check | 298 |
  | UsrPGTaxPlan | 175 |
  | UsrPGDoNotMarket | 10 |
  | UsrPGAcuMarket | 437 |
  | UsrPGRetire | 225 |
  | UsrServiceProviderQ | 92 |

  Re-check, read-only: `..\Acumatica-PK-TimeClock\tools\Query-PK9.ps1 "select sum(case when UsrPGPortalAccess=1 then 1 else 0 end) PortalAccess, sum(case when UsrPGAcuMarket=1 then 1 else 0 end) AcuMarket, count(*) Total from Contact where CompanyID=2"`
  (users may change values between the baseline and the publish; small differences can be normal edits).

## Rollback
If the publish fails or the checklist fails:
1. SM204505: untick `ProjectTask` → import the backup `ProjectTask` package exported in step 1 (or the previous
   `build\ProjectTask.zip` built from `main` before T-10), tick it and **tick `Contact`** again, then publish.
2. Data is not at risk: no step writes to the `Contact` columns, and neither publish removes them.
3. If only the compile fails because both were ticked, untick `Contact` and publish again. A failed publish leaves the
   previously published state in place.

## Manual test checklist (user, after the publish)
- [ ] SM204505: `Contact` shows as not published; `ProjectTask` published with no errors or warnings about `ContactExt`.
- [ ] **Contacts (CR302000)** in Modern UI: open an existing contact. A **PK Marketing** tab appears right after **General**.
- [ ] The tab shows the group **Marketing Attributes** with 8 check boxes: 1040 Checklist, 1040-ES Reminder,
      Acumatica Marketing List, Do Not Market, Portal Access, PK Retirement Prospect, Tax Planning Meeting, Service Provider Questionnaire.
- [ ] Old values intact: open a contact known to have boxes ticked (for example one with *Acumatica Marketing List* = yes).
      The same boxes are ticked as before the publish. Optional: the read-only count query above gives the baseline numbers.
- [ ] Values save: tick and untick one box on a test contact, save, reload (F5). The change persists. Then restore the original value.
- [ ] Any GI or report that uses these fields (if you have one) still runs.
- [ ] The rest of bundle A still works: Projects (PM301000) and Project Tasks (PM302000) PK fields, Doc Gathering (PKPM3001), Sample Testing (PKPM3002).
