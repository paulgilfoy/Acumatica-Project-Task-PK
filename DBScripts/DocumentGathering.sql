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
	[Additional_Notes] [nvarchar](255),
	[Active] [bit] NOT NULL,
	[FirstContactDate_Payroll] [datetime],
	[Notes_Payroll] [nvarchar](510),
	[Payroll_Complete] [bit],
	[MPR] [nvarchar](2),
	[MPR_LastModifiedDateTime] [datetime],
	[MPR_LastModUserName] [nvarchar](256),
	[PayrollVerification] [nvarchar](2),
	[PayrollVer_LastModifiedDateTime] [datetime],
	[PayrollVer_LastModUserName] [nvarchar](256),
	[FirstContactDate_PlanDocuments] [datetime],
	[Notes_PlanDocuments] [nvarchar](510),
	[PlanDocuments_Complete] [bit],
	[SPD] [nvarchar](2),
	[SPD_LastModifiedDateTime] [datetime],
	[SPD_LastModUserName] [nvarchar](256),
	[AA] [nvarchar](2),
	[AA_LastModifiedDateTime] [datetime],
	[AA_LastModUserName] [nvarchar](256),
	[BPD] [nvarchar](2),
	[BPD_LastModifiedDateTime] [datetime],
	[BPD_LastModUserName] [nvarchar](256),
	[IRS] [nvarchar](2),
	[IRS_LastModifiedDateTime] [datetime],
	[IRS_LastModUserName] [nvarchar](256),
	[SATrustree] [nvarchar](2),
	[SATrustree_LastModifiedDateTime] [datetime],
	[SATrustree_LastModUserName] [nvarchar](256),
	[SATPA] [nvarchar](2),
	[SATPA_LastModifiedDateTime] [datetime],
	[SATPA_LastModUserName] [nvarchar](256),
	[SAInvestment] [nvarchar](2),
	[SAInvestment_LastModifiedDateTime] [datetime],
	[SAInvestment_LastModUserName] [nvarchar](256),
	[LoanPolicy] [nvarchar](2),
	[LoanPolicy_LastModifiedDateTime] [datetime],
	[LoanPolicy_LastModUserName] [nvarchar](256),
	[QDIA] [nvarchar](2),
	[QDIA_LastModifiedDateTime] [datetime],
	[QDIA_LastModUserName] [nvarchar](256),
	[FirstContactDate_PlanDocPreproc] [datetime],
	[Notes_PlanDocPreproc] [nvarchar](510),
	[PlanDocPreproc_Complete] [bit],
	[EC] [nvarchar](2),
	[EC_LastModifiedDateTime] [datetime],
	[EC_LastModUserName] [nvarchar](256),
	[PAS] [nvarchar](2),
	[PAS_LastModifiedDateTime] [datetime],
	[PAS_LastModUserName] [nvarchar](256),
	[Cont] [nvarchar](2),
	[Cont_LastModifiedDateTime] [datetime],
	[Cont_LastModUserName] [nvarchar](256),
	[Dist] [nvarchar](2),
	[Dist_LastModifiedDateTime] [datetime],
	[Dist_LastModUserName] [nvarchar](256),
	[Loans] [nvarchar](2),
	[Loans_LastModifiedDateTime] [datetime],
	[Loans_LastModUserName] [nvarchar](256),
	[DefElection] [nvarchar](2),
	[DefElection_LastModifiedDateTime] [datetime],
	[DefElection_LastModUserName] [nvarchar](256),
	[FirstContactDate_PlanDesign] [datetime],
	[Notes_PlanDesign] [nvarchar](510),
	[PlanDesign_Complete] [bit],
	[PlanProvisions] [nvarchar](2),
	[PlanProvisions_LastModifiedDateTime] [datetime],
	[PlanProvisions_LastModUserName] [nvarchar](256),
	[PreProcessDate] [nvarchar](2),
	[PreProcessDate_LastModifiedDateTime] [datetime],
	[PreProcessDate_LastModUserName] [nvarchar](256),
	[FirstContactDate_AnnualDocuments] [datetime],
	[Notes_AnnualDocuments] [nvarchar](510),
	[AnnualDocuments_Complete] [bit],
	[Form_5500] [nvarchar](2),
	[Form_5500_LastModifiedDateTime] [datetime],
	[Form_5500_LastModUserName] [nvarchar](256),
	[TrustState] [nvarchar](2),
	[TrustState_LastModifiedDateTime] [datetime],
	[TrustState_LastModUserName] [nvarchar](256),
	[TrustCert] [nvarchar](2),
	[TrustCert_LastModifiedDateTime] [datetime],
	[TrustCert_LastModUserName] [nvarchar](256),
	[FidelityBond] [nvarchar](2),
	[FidelityBond_LastModifiedDateTime] [datetime],
	[FidelityBond_LastModUserName] [nvarchar](256),
	[ComplianceTest] [nvarchar](2),
	[ComplianceTest_LastModifiedDateTime] [datetime],
	[ComplianceTest_LastModUserName] [nvarchar](256),
	[RKFeeDisc] [nvarchar](2),
	[RKFeeDisc_LastModifiedDateTime] [datetime],
	[RKFeeDisc_LastModUserName] [nvarchar](256),
	[InvestAllocation] [nvarchar](2),
	[InvestAllocation_LastModifiedDateTime] [datetime],
	[InvestAllocation_LastModUserName] [nvarchar](256),
	[FirstContactDate_FirstYearPK] [datetime],
	[Notes_FirstYearPK] [nvarchar](510),
	[FirstYearPK_Complete] [bit],
	[ClientAuth] [nvarchar](2),
	[ClientAuth_LastModifiedDateTime] [datetime],
	[ClientAuth_LastModUserName] [nvarchar](256),
	[Communication] [nvarchar](2),
	[Communication_LastModifiedDateTime] [datetime],
	[Communication_LastModUserName] [nvarchar](256),
	[PY5500] [nvarchar](2),
	[PY5500_LastModifiedDateTime] [datetime],
	[PY5500_LastModUserName] [nvarchar](256),
	[PYTrustState] [nvarchar](2),
	[PYTrustState_LastModifiedDateTime] [datetime],
	[PYTrustState_LastModUserName] [nvarchar](256),
	[PYTrustCert] [nvarchar](2),
	[PYTrustCert_LastModifiedDateTime] [datetime],
	[PYTrustCert_LastModUserName] [nvarchar](256),
	[PYCompliance] [nvarchar](2),
	[PYCompliance_LastModifiedDateTime] [datetime],
	[PYCompliance_LastModUserName] [nvarchar](256),
	[Payroll_Preparer] [int],
    [PlanDoc_Preparer] [int],
    [PlanDesign_Preparer] [int],
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
