-- PKProjectStatus: department project status lists, maintained on PKPM2001 "Department Statuses" (T-21).
-- Runs on EVERY publish of the ProjectTask project, so it is idempotent:
--   1. creates the table if it does not exist (CompanyID first in the key, like every Acumatica table);
--   2. inserts the 54 seed statuses only where the (CompanyID, DepartmentID, StatusID) row is missing.
--      It never updates or deletes a row, so names / Active / SortOrder edited on PKPM2001 survive later publishes.
-- Seed text = the PXIntList text in src/Code/Contract.cs (what PM301000 shows), with the decisions in docs/status-design.md:
--   D1 PXIntList text wins: ACCT 2 "Initial Review (Acctg)", AUDIT 1 "Ready To Prep", 401K 5 "Initial RECS and Dis";
--   D2 ACCT 0 "Not Started" (CSAttributeDetail only, never a valid value) is NOT seeded;
--   D3 401K 17 keeps its double space ("Waiting  on Management Rep Letter").
-- Target companies: every tenant that has the STATUSTAX attribute (CompanyID > 0 skips the snapshot companies; system
-- company 1 has no STATUSTAX). SortOrder = StatusID, Active = 1, created by the admin user on screen PKPM2001.
-- tools/Test-StatusSeed.ps1 parses the VALUES list below (keep one (N'DEPT', ID, N'Text') row per line).
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PKProjectStatus')
BEGIN
    CREATE TABLE [dbo].[PKProjectStatus](
        [CompanyID] [int] NOT NULL,
        [DepartmentID] [nvarchar](10) NOT NULL,
        [StatusID] [int] NOT NULL,
        [Description] [nvarchar](60) NOT NULL,
        [SortOrder] [int] NOT NULL,
        [Active] [bit] NOT NULL CONSTRAINT [DF_PKProjectStatus_Active] DEFAULT ((1)),
        [CreatedByID] [uniqueidentifier] NOT NULL,
        [CreatedByScreenID] [char](8) NOT NULL,
        [CreatedDateTime] [datetime] NOT NULL,
        [LastModifiedByID] [uniqueidentifier] NOT NULL,
        [LastModifiedByScreenID] [char](8) NOT NULL,
        [LastModifiedDateTime] [datetime] NOT NULL,
        [tstamp] [timestamp] NOT NULL,
     CONSTRAINT [PK_PKProjectStatus] PRIMARY KEY CLUSTERED
    (
        [CompanyID] ASC,
        [DepartmentID] ASC,
        [StatusID] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END

INSERT INTO [dbo].[PKProjectStatus]
    ([CompanyID], [DepartmentID], [StatusID], [Description], [SortOrder], [Active],
     [CreatedByID], [CreatedByScreenID], [CreatedDateTime], [LastModifiedByID], [LastModifiedByScreenID], [LastModifiedDateTime])
SELECT c.[CompanyID], v.[DepartmentID], v.[StatusID], v.[Description], v.[StatusID], 1,
       'B5344897-037E-4D58-B5C3-1BDFD0F47BF9', 'PKPM2001', GETUTCDATE(),
       'B5344897-037E-4D58-B5C3-1BDFD0F47BF9', 'PKPM2001', GETUTCDATE()
FROM (SELECT DISTINCT a.[CompanyID] FROM [dbo].[CSAttribute] a WHERE a.[AttributeID] = 'STATUSTAX' AND a.[CompanyID] > 0) c
CROSS JOIN (VALUES
    (N'401K', 1, N'Setup/Engagement Letter/Invoice'),
    (N'401K', 2, N'Kickoff'),
    (N'401K', 3, N'Preprocessing Doc Gather & Plan Design'),
    (N'401K', 4, N'Data Preprocessing and Validation'),
    (N'401K', 5, N'Initial RECS and Dis'),
    (N'401K', 6, N'Remaining Doc Gather'),
    (N'401K', 7, N'Audit Results'),
    (N'401K', 8, N'Waiting on 5500'),
    (N'401K', 9, N'Risk Assessment & Sample Selection'),
    (N'401K', 10, N'Sample Testing'),
    (N'401K', 11, N'Remaining Audit Steps'),
    (N'401K', 12, N'Finalize Audit Results'),
    (N'401K', 13, N'Initial Review'),
    (N'401K', 14, N'Clearing Points'),
    (N'401K', 15, N'Preparation of Final Deliverables'),
    (N'401K', 16, N'Technical Review'),
    (N'401K', 17, N'Waiting  on Management Rep Letter'),
    (N'401K', 18, N'Final Bill'),
    (N'401K', 19, N'Wrap Up'),
    (N'401K', 20, N'Complete'),
    (N'TAX', 1, N'Ready to Assign'),
    (N'TAX', 2, N'Ready to Autoflow'),
    (N'TAX', 3, N'Information Requested (from Client)'),
    (N'TAX', 4, N'Information Requested (In-House)'),
    (N'TAX', 5, N'Ready to Prep'),
    (N'TAX', 6, N'In Preparation'),
    (N'TAX', 7, N'In Initial Review'),
    (N'TAX', 8, N'Clearing Points'),
    (N'TAX', 9, N'In Final Review'),
    (N'TAX', 10, N'Technical Review'),
    (N'TAX', 11, N'Ready to Deliver'),
    (N'TAX', 12, N'Bill Client'),
    (N'TAX', 13, N'Invoice'),
    (N'TAX', 14, N'Complete'),
    (N'TAX', 15, N'Waiting on 8879 Form'),
    (N'ACCT', 1, N'In Preparation'),
    (N'ACCT', 2, N'Initial Review (Acctg)'),
    (N'ACCT', 3, N'Clearing Points'),
    (N'ACCT', 4, N'In Final Review'),
    (N'ACCT', 5, N'Bill Client'),
    (N'ACCT', 6, N'Invoice'),
    (N'ACCT', 7, N'Complete'),
    (N'ACCT', 8, N'Meet with Client'),
    (N'AUDIT', 1, N'Ready To Prep'),
    (N'AUDIT', 2, N'In Preparation'),
    (N'AUDIT', 3, N'Fieldwork in Process'),
    (N'AUDIT', 4, N'In Initial Review'),
    (N'AUDIT', 5, N'Clearing Points'),
    (N'AUDIT', 6, N'In Final Review'),
    (N'AUDIT', 7, N'Financial Statements'),
    (N'AUDIT', 8, N'Technical Review'),
    (N'AUDIT', 9, N'Ready to Deliver'),
    (N'AUDIT', 10, N'Bill Client'),
    (N'AUDIT', 11, N'Complete')
) v([DepartmentID], [StatusID], [Description])
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[PKProjectStatus] p
                  WHERE p.[CompanyID] = c.[CompanyID] AND p.[DepartmentID] = v.[DepartmentID] AND p.[StatusID] = v.[StatusID])
