USE [Acumatica22202]
GO

DROP TABLE IF EXISTS [dbo].[PMDocumentGathering]
GO

/****** Object:  Table [dbo].[PMDocumentGathering]    Script Date: 02/23/2023 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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
	[PayrollVerification] [nvarchar](2),
	[PayrollVerLastModifiedDateTime] [datetime],
	[PayrollVerLastModUserName] [nvarchar](256),
	[FirstContactDatePlanDocuments] [datetime],
	[NotesPlanDocuments] [nvarchar](510),
	[PlanDocumentsComplete] [bit],
	[SPD] [nvarchar](2),
	[SPDLastModifiedDateTime] [datetime],
	[SPDLastModUserName] [nvarchar](256),
	[AA] [nvarchar](2),
	[AALastModifiedDateTime] [datetime],
	[AALastModUserName] [nvarchar](256),
	[BPD] [nvarchar](2),
	[BPDLastModifiedDateTime] [datetime],
	[BPDLastModUserName] [nvarchar](256),
	[IRS] [nvarchar](2),
	[IRSLastModifiedDateTime] [datetime],
	[IRSLastModUserName] [nvarchar](256),
	[SATrustree] [nvarchar](2),
	[SATrustreeLastModifiedDateTime] [datetime],
	[SATrustreeLastModUserName] [nvarchar](256),
	[SATPA] [nvarchar](2),
	[SATPALastModifiedDateTime] [datetime],
	[SATPALastModUserName] [nvarchar](256),
	[SAInvestment] [nvarchar](2),
	[SAInvestmentLastModifiedDateTime] [datetime],
	[SAInvestmentLastModUserName] [nvarchar](256),
	[LoanPolicy] [nvarchar](2),
	[LoanPolicyLastModifiedDateTime] [datetime],
	[LoanPolicyLastModUserName] [nvarchar](256),
	[QDIA] [nvarchar](2),
	[QDIALastModifiedDateTime] [datetime],
	[QDIALastModUserName] [nvarchar](256),
	[FirstContactDatePlanDocPreproc] [datetime],
	[NotesPlanDocPreproc] [nvarchar](510),
	[PlanDocPreprocComplete] [bit],
	[EC] [nvarchar](2),
	[ECLastModifiedDateTime] [datetime],
	[ECLastModUserName] [nvarchar](256),
	[PAS] [nvarchar](2),
	[PASLastModifiedDateTime] [datetime],
	[PASLastModUserName] [nvarchar](256),
	[Cont] [nvarchar](2),
	[ContLastModifiedDateTime] [datetime],
	[ContLastModUserName] [nvarchar](256),
	[Dist] [nvarchar](2),
	[DistLastModifiedDateTime] [datetime],
	[DistLastModUserName] [nvarchar](256),
	[Loans] [nvarchar](2),
	[LoansLastModifiedDateTime] [datetime],
	[LoansLastModUserName] [nvarchar](256),
	[DefElection] [nvarchar](2),
	[DefElectionLastModifiedDateTime] [datetime],
	[DefElectionLastModUserName] [nvarchar](256),
	[FirstContactDatePlanDesign] [datetime],
	[NotesPlanDesign] [nvarchar](510),
	[PlanDesignComplete] [bit],
	[PlanProvisions] [nvarchar](2),
	[PlanProvisionsLastModifiedDateTime] [datetime],
	[PlanProvisionsLastModUserName] [nvarchar](256),
	[PreProcessDate] [nvarchar](2),
	[PreProcessDateLastModifiedDateTime] [datetime],
	[PreProcessDateLastModUserName] [nvarchar](256),
	[FirstContactDateAnnualDocuments] [datetime],
	[NotesAnnualDocuments] [nvarchar](510),
	[AnnualDocumentsComplete] [bit],
	[Form5500] [nvarchar](2),
	[Form5500LastModifiedDateTime] [datetime],
	[Form5500LastModUserName] [nvarchar](256),
	[TrustState] [nvarchar](2),
	[TrustStateLastModifiedDateTime] [datetime],
	[TrustStateLastModUserName] [nvarchar](256),
	[TrustCert] [nvarchar](2),
	[TrustCertLastModifiedDateTime] [datetime],
	[TrustCertLastModUserName] [nvarchar](256),
	[FidelityBond] [nvarchar](2),
	[FidelityBondLastModifiedDateTime] [datetime],
	[FidelityBondLastModUserName] [nvarchar](256),
	[ComplianceTest] [nvarchar](2),
	[ComplianceTestLastModifiedDateTime] [datetime],
	[ComplianceTestLastModUserName] [nvarchar](256),
	[RKFeeDisc] [nvarchar](2),
	[RKFeeDiscLastModifiedDateTime] [datetime],
	[RKFeeDiscLastModUserName] [nvarchar](256),
	[InvestAllocation] [nvarchar](2),
	[InvestAllocationLastModifiedDateTime] [datetime],
	[InvestAllocationLastModUserName] [nvarchar](256),
	[FirstContactDateFirstYearPK] [datetime],
	[NotesFirstYearPK] [nvarchar](510),
	[FirstYearPKComplete] [bit],
	[ClientAuth] [nvarchar](2),
	[ClientAuthLastModifiedDateTime] [datetime],
	[ClientAuthLastModUserName] [nvarchar](256),
	[Communication] [nvarchar](2),
	[CommunicationLastModifiedDateTime] [datetime],
	[CommunicationLastModUserName] [nvarchar](256),
	[PY5500] [nvarchar](2),
	[PY5500LastModifiedDateTime] [datetime],
	[PY5500LastModUserName] [nvarchar](256),
	[PYTrustState] [nvarchar](2),
	[PYTrustStateLastModifiedDateTime] [datetime],
	[PYTrustStateLastModUserName] [nvarchar](256),
	[PYTrustCert] [nvarchar](2),
	[PYTrustCertLastModifiedDateTime] [datetime],
	[PYTrustCertLastModUserName] [nvarchar](256),
	[PYCompliance] [nvarchar](2),
	[PYComplianceLastModifiedDateTime] [datetime],
	[PYComplianceLastModUserName] [nvarchar](256),
	[PayrollPreparer] [int],
    [PlanDocPreparer] [int],
    [PlanDesignPreparer] [int],
    [PayrollProvider] [nvarchar](3), 
    [RKAccess] [nvarchar](2),
    [RecordKeeperName] [nvarchar](3),
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
GO

ALTER TABLE [dbo].[PMDocumentGathering] ADD  CONSTRAINT [DF_PMDocumentGathering_ProjectID]  DEFAULT ((0)) FOR [ProjectID]
GO

CREATE UNIQUE NONCLUSTERED INDEX [PMDocumentGathering_ProjectID_Uindex] ON [dbo].[PMDocumentGathering]
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
