IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PMDocumentGathering')
BEGIN
    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON

    CREATE TABLE [dbo].[PMDocumentGathering](
        [ProjectID] [int] NOT NULL,
        [AdditionalNotes] [nvarchar](255),
        [Active] [bit] NOT NULL,
        [FirstContactDatePayroll] [datetime],
        [NotesPayroll] [nvarchar](510),
        [PayrollComplete] [bit],
        [MPR] [nvarchar](2),
        [MPRLastModifiedDateTime] [datetime],
        [MPRLastModUserName] [nvarchar](256),
        [MPRAshAutire] [bit],
        [PayrollVerification] [nvarchar](2),
        [PayrollVerLastModifiedDateTime] [datetime],
        [PayrollVerLastModUserName] [nvarchar](256),
        [PayrollAshAutire] [bit],
		[Finch] [nvarchar](2),
		[FinchLastModifiedDateTime] [datetime],
		[FinchLastModUserName] [nvarchar](256),
        [FirstContactDatePlanDocuments] [datetime],
        [NotesPlanDocuments] [nvarchar](510),
        [PlanDocumentsComplete] [bit],
        [SPD] [nvarchar](2),
        [SPDLastModifiedDateTime] [datetime],
        [SPDLastModUserName] [nvarchar](256),
        [SPDAshAutire] [bit],
        [AA] [nvarchar](2),
        [AALastModifiedDateTime] [datetime],
        [AALastModUserName] [nvarchar](256),
        [AAAshAutire] [bit],
        [BPD] [nvarchar](2),
        [BPDLastModifiedDateTime] [datetime],
        [BPDLastModUserName] [nvarchar](256),
        [BPDAshAutire] [bit],
        [IRS] [nvarchar](2),
        [IRSLastModifiedDateTime] [datetime],
        [IRSLastModUserName] [nvarchar](256),
        [IRSAshAutire] [bit],
        [SATrustree] [nvarchar](2),
        [SATrustreeLastModifiedDateTime] [datetime],
        [SATrustreeLastModUserName] [nvarchar](256),
        [SATrustreeAshAutire] [bit],
        [SATPA] [nvarchar](2),
        [SATPALastModifiedDateTime] [datetime],
        [SATPALastModUserName] [nvarchar](256),
        [SATPAAshAutire] [bit],
        [SAInvestment] [nvarchar](2),
        [SAInvestmentLastModifiedDateTime] [datetime],
        [SAInvestmentLastModUserName] [nvarchar](256),
        [SAInvestmentAshAutire] [bit],
        [LoanPolicy] [nvarchar](2),
        [LoanPolicyLastModifiedDateTime] [datetime],
        [LoanPolicyLastModUserName] [nvarchar](256),
        [LoanPolicyAshAutire] [bit],
        [QDIA] [nvarchar](2),
        [QDIALastModifiedDateTime] [datetime],
        [QDIALastModUserName] [nvarchar](256),
        [QDIAAshAutire] [bit],
        [FirstContactDatePlanDocPreproc] [datetime],
        [NotesPlanDocPreproc] [nvarchar](510),
        [PlanDocPreprocComplete] [bit],
        [EC] [nvarchar](2),
        [ECLastModifiedDateTime] [datetime],
        [ECLastModUserName] [nvarchar](256),
        [ECAshAutire] [bit],
        [PAS] [nvarchar](2),
        [PASLastModifiedDateTime] [datetime],
        [PASLastModUserName] [nvarchar](256),
        [PASAshAutire] [bit],
        [Cont] [nvarchar](2),
        [ContLastModifiedDateTime] [datetime],
        [ContLastModUserName] [nvarchar](256),
        [ContAshAutire] [bit],
        [Dist] [nvarchar](2),
        [DistLastModifiedDateTime] [datetime],
        [DistLastModUserName] [nvarchar](256),
        [DistAshAutire] [bit],
        [Loans] [nvarchar](2),
        [LoansLastModifiedDateTime] [datetime],
        [LoansLastModUserName] [nvarchar](256),
        [LoansAshAutire] [bit],
        [DefElection] [nvarchar](2),
        [DefElectionLastModifiedDateTime] [datetime],
        [DefElectionLastModUserName] [nvarchar](256),
        [DefElectionAshAutire] [bit],
        [FirstContactDatePlanDesign] [datetime],
        [NotesPlanDesign] [nvarchar](510),
        [PlanDesignComplete] [bit],
        [PlanProvisions] [nvarchar](2),
        [PlanProvisionsLastModifiedDateTime] [datetime],
        [PlanProvisionsLastModUserName] [nvarchar](256),
        [PlanProvisionsAshAutire] [bit],
        [PreProcessDate] [nvarchar](2),
        [PreProcessDateLastModifiedDateTime] [datetime],
        [PreProcessDateLastModUserName] [nvarchar](256),
        [PreProcessDateAshAutire] [bit],
        [FirstContactDateAnnualDocuments] [datetime],
        [NotesAnnualDocuments] [nvarchar](510),
        [AnnualDocumentsComplete] [bit],
        [Form5500] [nvarchar](2),
        [Form5500AshAutire] [bit],
        [Form5500LastModifiedDateTime] [datetime],
        [Form5500LastModUserName] [nvarchar](256),
        [TrustState] [nvarchar](2),
        [TrustStateLastModifiedDateTime] [datetime],
        [TrustStateLastModUserName] [nvarchar](256),
        [TrustStateAshAutire] [bit],
        [TrustCert] [nvarchar](2),
        [TrustCertLastModifiedDateTime] [datetime],
        [TrustCertLastModUserName] [nvarchar](256),
        [TrustCertAshAutire] [bit],
        [FidelityBond] [nvarchar](2),
        [FidelityBondLastModifiedDateTime] [datetime],
        [FidelityBondLastModUserName] [nvarchar](256),
        [FidelityBondAshAutire] [bit],
        [ComplianceTest] [nvarchar](2),
        [ComplianceTestLastModifiedDateTime] [datetime],
        [ComplianceTestLastModUserName] [nvarchar](256),
        [ComplianceTestAshAutire] [bit],
        [RKFeeDisc] [nvarchar](2),
        [RKFeeDiscLastModifiedDateTime] [datetime],
        [RKFeeDiscLastModUserName] [nvarchar](256),
        [RKFeeDiscAshAutire] [bit],
        [InvestAllocation] [nvarchar](2),
        [InvestAllocationLastModifiedDateTime] [datetime],
        [InvestAllocationLastModUserName] [nvarchar](256),
        [InvestAllocationAshAutire] [bit],
        [FirstContactDateFirstYearPK] [datetime],
        [NotesFirstYearPK] [nvarchar](510),
        [FirstYearPKComplete] [bit],
        [ClientAuth] [nvarchar](2),
        [ClientAuthLastModifiedDateTime] [datetime],
        [ClientAuthLastModUserName] [nvarchar](256),
        [ClientAuthAshAutire] [bit],
        [Communication] [nvarchar](2),
        [CommunicationLastModifiedDateTime] [datetime],
        [CommunicationLastModUserName] [nvarchar](256),
        [CommunicationAshAutire] [bit],
        [PY5500] [nvarchar](2),
        [PY5500LastModifiedDateTime] [datetime],
        [PY5500LastModUserName] [nvarchar](256),
        [PY5500AshAutire] [bit],
        [PYTrustState] [nvarchar](2),
        [PYTrustStateLastModifiedDateTime] [datetime],
        [PYTrustStateLastModUserName] [nvarchar](256),
        [PYTrustStateAshAutire] [bit],
        [PYTrustCert] [nvarchar](2),
        [PYTrustCertLastModifiedDateTime] [datetime],
        [PYTrustCertLastModUserName] [nvarchar](256),
        [PYTrustCertAshAutire] [bit],
        [PYCompliance] [nvarchar](2),
        [PYComplianceLastModifiedDateTime] [datetime],
        [PYComplianceLastModUserName] [nvarchar](256),
        [PYComplianceAshAutire] [bit],
		[PayrollPreparer] [int],
		[PlanDocPreparer] [int],
		[PlanDesignPreparer] [int],
		[PayrollProvider] [nvarchar](3),
		[RKAccess] [nvarchar](2),
		[RKAccessAutire] [nvarchar](2),
		[RecordKeeperName] [nvarchar](3),
        [MPRAshAutire] [bit],
        [PayrollAshAutire] [bit],
        [SPDAshAutire] [bit],
        [AAAshAutire] [bit],
        [BPDAshAutire] [bit],
        [IRSAshAutire] [bit],
        [SATrustreeAshAutire] [bit],
        [SATPAAshAutire] [bit],
        [SAInvestmentAshAutire] [bit],
        [LoanPolicyAshAutire] [bit],
        [QDIAAshAutire] [bit],
        [ECAshAutire] [bit],
        [PASAshAutire] [bit],
        [ContAshAutire] [bit],
        [DistAshAutire] [bit],
        [LoansAshAutire] [bit],
        [DefElectionAshAutire] [bit],
        [PlanProvisionsAshAutire] [bit],
        [PreProcessDateAshAutire] [bit],
        [Form5500AshAutire] [bit],
        [TrustStateAshAutire] [bit],
        [TrustCertAshAutire] [bit],
        [FidelityBondAshAutire] [bit],
        [ComplianceTestAshAutire] [bit],
        [RKFeeDiscAshAutire] [bit],
        [InvestAllocationAshAutire] [bit],
        [ClientAuthAshAutire] [bit],
        [CommunicationAshAutire] [bit],
        [PY5500AshAutire] [bit],
        [PYTrustStateAshAutire] [bit],
        [PYTrustCertAshAutire] [bit],
        [PYComplianceAshAutire] [bit],
        [CreatedDateTime] [datetime] NOT NULL,
        [CreatedByID] [uniqueidentifier] NOT NULL,
        [CreatedByScreenID] [char](8) NOT NULL,
        [LastModifiedDateTime] [datetime] NOT NULL,
        [LastModifiedByID] [uniqueidentifier] NOT NULL,
        [LastModifiedByScreenID] [char](8) NOT NULL,
        [tstamp] [timestamp] NOT NULL,
        [NoteID] [uniqueidentifier] NOT NULL,
     CONSTRAINT [PK_PMDocumentGathering] PRIMARY KEY CLUSTERED
    (
        [ProjectID] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[PMDocumentGathering] ADD  CONSTRAINT [DF_PMDocumentGathering_ProjectID]  DEFAULT ((0)) FOR [ProjectID]

    CREATE UNIQUE NONCLUSTERED INDEX [PMDocumentGathering_ProjectID_Uindex] ON [dbo].[PMDocumentGathering]
    (
        [ProjectID] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END
GO


--The section below handles addition of columns.  It first checks to see if the column already exists.
--Finch
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PMDocumentGathering' AND COLUMN_NAME = 'Finch')
BEGIN
    ALTER TABLE [PMDocumentGathering]
    ADD [Finch] [nvarchar](2);
END

--FinchLastModifiedDateTime
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PMDocumentGathering' AND COLUMN_NAME = 'FinchLastModifiedDateTime')
BEGIN
    ALTER TABLE [PMDocumentGathering]
    ADD [FinchLastModifiedDateTime] [datetime];
END

--FinchLastModUserName
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PMDocumentGathering' AND COLUMN_NAME = 'FinchLastModUserName')
BEGIN
    ALTER TABLE [PMDocumentGathering]
    ADD [FinchLastModUserName] [nvarchar](256);
END

--RKAccess
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PMDocumentGathering' AND COLUMN_NAME = 'RKAccess')
BEGIN
    ALTER TABLE [PMDocumentGathering]
    ADD [RKAccess] [nvarchar](2);
END

--RKAccessAutire
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PMDocumentGathering' AND COLUMN_NAME = 'RKAccessAutire')
BEGIN
    ALTER TABLE [PMDocumentGathering]
    ADD [RKAccessAutire] [nvarchar](2);
END

--Check if AshAutire columns exist and add all if they don't
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PMDocumentGathering' AND COLUMN_NAME = 'MPRAshAutire')
BEGIN
    ALTER TABLE [PMDocumentGathering]
    ADD [MPRAshAutire] [bit],
        [PayrollAshAutire] [bit],
        [SPDAshAutire] [bit],
        [AAAshAutire] [bit],
        [BPDAshAutire] [bit],
        [IRSAshAutire] [bit],
        [SATrustreeAshAutire] [bit],
        [SATPAAshAutire] [bit],
        [SAInvestmentAshAutire] [bit],
        [LoanPolicyAshAutire] [bit],
        [QDIAAshAutire] [bit],
        [ECAshAutire] [bit],
        [PASAshAutire] [bit],
        [ContAshAutire] [bit],
        [DistAshAutire] [bit],
        [LoansAshAutire] [bit],
        [DefElectionAshAutire] [bit],
        [PlanProvisionsAshAutire] [bit],
        [PreProcessDateAshAutire] [bit],
        [Form5500AshAutire] [bit],
        [TrustStateAshAutire] [bit],
        [TrustCertAshAutire] [bit],
        [FidelityBondAshAutire] [bit],
        [ComplianceTestAshAutire] [bit],
        [RKFeeDiscAshAutire] [bit],
        [InvestAllocationAshAutire] [bit],
        [ClientAuthAshAutire] [bit],
        [CommunicationAshAutire] [bit],
        [PY5500AshAutire] [bit],
        [PYTrustStateAshAutire] [bit],
        [PYTrustCertAshAutire] [bit],
        [PYComplianceAshAutire] [bit];
END

GO