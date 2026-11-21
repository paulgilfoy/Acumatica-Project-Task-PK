-- PKProjectStatusBackfill (T-25): sets the read-only "PK Status" columns Contract.UsrPKStatusID / Contract.UsrPKStatus
-- of EVERY project from its department + status fields + PKProjectStatus, and clears them where they must be empty.
-- Same value as the C# code (src/Code/PKDepartmentStatus.cs, kept current on every save by PKDepartmentStatusAttribute):
--   UsrPGDepartment TAX -> UsrPGTaxStatus, ACCT -> UsrPGAcctStatus, AUDIT -> UsrPGAuditStatus, 401K -> Usr401kStatus;
--   any other department or none -> both NULL; text = PKProjectStatus.Description of that status (active or inactive),
--   NULL if the status has no PKProjectStatus row. tools/Test-PKStatus.ps1 checks this CASE list against the C# mapping.
-- Idempotent: only rows whose stored values differ are written (case-sensitive text compare), so a second run changes nothing.
-- It doubles as the "recalculate": run it again (see docs/pk-status.md) if the stored values ever drift.
-- Companies: every tenant (CompanyID > 0 skips the negative snapshot companies, like Sql/PKProjectStatus.sql).
-- Publish order (verified by IL in PX.Web.Customization 26.200, CstWebsiteStorage.UpdateDatabase / GetUpdatePriority):
--   Table items (priority 3, they add the two columns) run BEFORE plain Sql items (priority 4); Sql items of equal
--   SqlScriptPriority run in item-key order, so Sql#PKProjectStatus (table + seed) runs before Sql#PKProjectStatusBackfill.
--   The script still guards itself: it does nothing unless both columns and the PKProjectStatus table exist, and the
--   UPDATE is dynamic SQL so the batch compiles even when the columns are missing.
-- Fast publish (the default Publish) skips a database item whose content was already applied (table CustPublishideScripts):
--   the script runs on the first publish that contains it and again only after a text change or a publish with
--   "Execute All Database Scripts (Including Previously Executed)" / Publish with Cleanup.
IF COL_LENGTH('dbo.Contract', 'UsrPKStatusID') IS NOT NULL
   AND COL_LENGTH('dbo.Contract', 'UsrPKStatus') IS NOT NULL
   AND OBJECT_ID('dbo.PKProjectStatus', 'U') IS NOT NULL
BEGIN
    EXEC sp_executesql N'
    UPDATE c
    SET c.UsrPKStatusID = v.PKStatusID,
        c.UsrPKStatus   = s.Description
    FROM dbo.Contract c
    CROSS APPLY (SELECT CASE UPPER(LTRIM(RTRIM(c.UsrPGDepartment)))
                            WHEN ''TAX''   THEN c.UsrPGTaxStatus
                            WHEN ''ACCT''  THEN c.UsrPGAcctStatus
                            WHEN ''AUDIT'' THEN c.UsrPGAuditStatus
                            WHEN ''401K''  THEN c.Usr401kStatus
                        END AS PKStatusID) v
    LEFT JOIN dbo.PKProjectStatus s
           ON s.CompanyID = c.CompanyID
          AND s.DepartmentID = UPPER(LTRIM(RTRIM(c.UsrPGDepartment)))
          AND s.StatusID = v.PKStatusID
    WHERE c.CompanyID > 0
      AND EXISTS (SELECT c.UsrPKStatusID, c.UsrPKStatus COLLATE Latin1_General_BIN2
                  EXCEPT
                  SELECT v.PKStatusID, s.Description COLLATE Latin1_General_BIN2)'
END
