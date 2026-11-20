# Status lists: `PXIntList` vs `CSAttributeDetail` (T-01 d)

Read-only SQL on PK9, `CompanyID = 2`, 2026-11-20. Source of the lists: `src/Code/Contract.cs` (`ContractExt`).
`CSAttribute` rows exist for `STATUS401K` ("401k Status"), `STATUSTAX` ("Tax Status"), `STATUSACCT` ("Acct Status"),
`STATUSAUDI` ("Audit Status"), all `ControlType = 2` (combo), `IsInternal = 0`. **No `STATUSTECH` attribute exists.**
`CSAttributeDetail` has 55 rows: 401K 20, TAX 15, ACCT 9, AUDI 11. `ValueID` is a zero-padded string ("01") = the int.
**`SortOrder` is NULL on every row and `Disabled` is False on every row.** (So the GI/attribute list order is by ValueID.)

## Result of the comparison
- **Integer keys and text match for 54 of 55 rows**, with these exceptions:

| Attribute | ValueID | `PXIntList` text (Contract.cs) | `CSAttributeDetail.Description` | Kind |
|---|---|---|---|---|
| STATUSACCT | 02 | Initial Review (Acctg) | Initial Review | **Real text mismatch** |
| STATUSACCT | 00 | *(no entry; list is 1..8)* | Not Started | Extra row only in the DB (no project has value 0) |
| STATUSAUDI | 01 | Ready To Prep | Ready to Prep | Case only |
| STATUS401K | 05 | Initial RECS and Dis | Initial RECs and DIs | Case/wording only |
| STATUS401K | 17 | Waiting  on Management Rep Letter | Waiting  on Management Rep Letter | Identical, but contains a **double space** in both (preserve or fix deliberately in the seed) |

- Live data check (`Contract`, CompanyID 2): every stored value lies inside the list (no orphan ints).
  Counts: 401K 1=21 2=9 3=5 4=2 5=2 7=7 10=4 11=3 12=2 16=4 17=3 20=1848; ACCT 1=15 2=12 4=1 5=101 6=1 7=971;
  AUDIT 2=22 4=1 8=2 11=349; TAX 1=3 2=3 3=24 4=8 5=31 6=33 7=4 10=2 11=9 12=19 13=2 14=6217. `UsrPGTechStatus`: one row = 4.
- GI000142 ("PK - Preparer Dashboard", DesignID `598c8d6e-b91e-42f5-925b-6338ac41d46d`) joins `CSAttributeDetail` through `GIOn`:
  `attributeID = 'STATUS401K'|'STATUSTAX'|'STATUSACCT'|'STATUSAUDI'` and `valueID = CInt([PMProject.Usr401kStatus])`,
  `CInt([PMProject.UsrPGTaxStatus])`, **`[PMProject.UsrPGAcctStatus]` and `[PMProject.UsrPGAuditStatus]` without `CInt`** (implicit conversion; works today, fragile).
  GI000142 therefore shows the **DB text**, the project screen shows the **`PXIntList` text** - this is why the ACCT 02 mismatch is visible to users today.

## Full table
Columns: `PXIntList` = int and text in `Contract.cs`; the rest = `CSAttributeDetail`.

### STATUS401K (field `Usr401kStatus`, 20 values)
| Int | PXIntList text | ValueID | Description | SortOrder | Disabled | Match |
|---|---|---|---|---|---|---|
| 1 | Setup/Engagement Letter/Invoice | 01 | Setup/Engagement Letter/Invoice | NULL | 0 | ok |
| 2 | Kickoff | 02 | Kickoff | NULL | 0 | ok |
| 3 | Preprocessing Doc Gather & Plan Design | 03 | Preprocessing Doc Gather & Plan Design | NULL | 0 | ok |
| 4 | Data Preprocessing and Validation | 04 | Data Preprocessing and Validation | NULL | 0 | ok |
| 5 | Initial RECS and Dis | 05 | Initial RECs and DIs | NULL | 0 | case |
| 6 | Remaining Doc Gather | 06 | Remaining Doc Gather | NULL | 0 | ok |
| 7 | Audit Results | 07 | Audit Results | NULL | 0 | ok |
| 8 | Waiting on 5500 | 08 | Waiting on 5500 | NULL | 0 | ok |
| 9 | Risk Assessment & Sample Selection | 09 | Risk Assessment & Sample Selection | NULL | 0 | ok |
| 10 | Sample Testing | 10 | Sample Testing | NULL | 0 | ok |
| 11 | Remaining Audit Steps | 11 | Remaining Audit Steps | NULL | 0 | ok |
| 12 | Finalize Audit Results | 12 | Finalize Audit Results | NULL | 0 | ok |
| 13 | Initial Review | 13 | Initial Review | NULL | 0 | ok |
| 14 | Clearing Points | 14 | Clearing Points | NULL | 0 | ok |
| 15 | Preparation of Final Deliverables | 15 | Preparation of Final Deliverables | NULL | 0 | ok |
| 16 | Technical Review | 16 | Technical Review | NULL | 0 | ok |
| 17 | Waiting  on Management Rep Letter | 17 | Waiting  on Management Rep Letter | NULL | 0 | ok (double space) |
| 18 | Final Bill | 18 | Final Bill | NULL | 0 | ok |
| 19 | Wrap Up | 19 | Wrap Up | NULL | 0 | ok |
| 20 | Complete | 20 | Complete | NULL | 0 | ok |

### STATUSTAX (field `UsrPGTaxStatus`, 15 values) - all 15 rows match exactly
| Int | Text (identical in both) | ValueID | SortOrder | Disabled |
|---|---|---|---|---|
| 1 | Ready to Assign | 01 | NULL | 0 |
| 2 | Ready to Autoflow | 02 | NULL | 0 |
| 3 | Information Requested (from Client) | 03 | NULL | 0 |
| 4 | Information Requested (In-House) | 04 | NULL | 0 |
| 5 | Ready to Prep | 05 | NULL | 0 |
| 6 | In Preparation | 06 | NULL | 0 |
| 7 | In Initial Review | 07 | NULL | 0 |
| 8 | Clearing Points | 08 | NULL | 0 |
| 9 | In Final Review | 09 | NULL | 0 |
| 10 | Technical Review | 10 | NULL | 0 |
| 11 | Ready to Deliver | 11 | NULL | 0 |
| 12 | Bill Client | 12 | NULL | 0 |
| 13 | Invoice | 13 | NULL | 0 |
| 14 | Complete | 14 | NULL | 0 |
| 15 | Waiting on 8879 Form | 15 | NULL | 0 |

### STATUSACCT (field `UsrPGAcctStatus`, PXIntList 8 values, DB 9 rows)
| Int | PXIntList text | ValueID | Description | SortOrder | Disabled | Match |
|---|---|---|---|---|---|---|
| - | *(none)* | 00 | Not Started | NULL | 0 | DB only |
| 1 | In Preparation | 01 | In Preparation | NULL | 0 | ok |
| 2 | Initial Review (Acctg) | 02 | Initial Review | NULL | 0 | **TEXT DIFF** |
| 3 | Clearing Points | 03 | Clearing Points | NULL | 0 | ok |
| 4 | In Final Review | 04 | In Final Review | NULL | 0 | ok |
| 5 | Bill Client | 05 | Bill Client | NULL | 0 | ok |
| 6 | Invoice | 06 | Invoice | NULL | 0 | ok |
| 7 | Complete | 07 | Complete | NULL | 0 | ok |
| 8 | Meet with Client | 08 | Meet with Client | NULL | 0 | ok |

### STATUSAUDI (field `UsrPGAuditStatus`, 11 values)
| Int | PXIntList text | ValueID | Description | SortOrder | Disabled | Match |
|---|---|---|---|---|---|---|
| 1 | Ready To Prep | 01 | Ready to Prep | NULL | 0 | case |
| 2 | In Preparation | 02 | In Preparation | NULL | 0 | ok |
| 3 | Fieldwork in Process | 03 | Fieldwork in Process | NULL | 0 | ok |
| 4 | In Initial Review | 04 | In Initial Review | NULL | 0 | ok |
| 5 | Clearing Points | 05 | Clearing Points | NULL | 0 | ok |
| 6 | In Final Review | 06 | In Final Review | NULL | 0 | ok |
| 7 | Financial Statements | 07 | Financial Statements | NULL | 0 | ok |
| 8 | Technical Review | 08 | Technical Review | NULL | 0 | ok |
| 9 | Ready to Deliver | 09 | Ready to Deliver | NULL | 0 | ok |
| 10 | Bill Client | 10 | Bill Client | NULL | 0 | ok |
| 11 | Complete | 11 | Complete | NULL | 0 | ok |

### Not in the status lists (for T-20/T-22 scope)
- `PMTask.UsrPGTaskStatus` (1-9, company-wide, `PMTask.cs:29`) has **no** `CSAttributeDetail` copy; TimeClock keeps its own copy (`PKTimeClockConstants.cs`).
- `UsrPGTechStatus` (1-4, "Tech Status 1..4" placeholders, `Contract.cs:90`): no attribute; one project uses value 4.
- Other `PXIntList`s on Contract that are **not** statuses: `UsrPGPriority` (Low/Medium/High), `UsrPGComplexity` (Level 1-3).

## Implications for the seed (T-21) and the swap (T-22)
1. The seed must preserve ints 1:1 (all stored values are within 1..20/15/8/11). Decide which text wins for ACCT 02 ("Initial Review (Acctg)" is what users see on the project screen; "Initial Review" is what GI000142 shows) and for the case-only differences.
2. The extra DB row ACCT 00 "Not Started" is unused by data; it can be dropped or carried over as inactive.
3. `SortOrder` is unused; the new table's `SortOrder` should default to the int so display order is unchanged.
4. If `CSAttributeDetail` is retired, GI000142's four `GIOn` joins must be repointed (or the attribute rows kept in sync). See open question Q3 in `inventory.md`.
