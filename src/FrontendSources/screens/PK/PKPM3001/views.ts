import {
	PXView,
	PXFieldState,
	PXFieldOptions,
} from "client-controls";

export class DocGathering extends PXView {
	ProjectID: PXFieldState<PXFieldOptions.CommitChanges>;
	PMProject__Usr401kStatus: PXFieldState;
	AdditionalNotes: PXFieldState;
}

export class DocGatheringProperties extends PXView {
	Active: PXFieldState;

	// Payroll Preprocessing
	MPR: PXFieldState<PXFieldOptions.CommitChanges>;
	MPRLastModifiedDateTime: PXFieldState;
	MPRLastModUserName: PXFieldState;
	MPRAshAutire: PXFieldState;
	PayrollVerification: PXFieldState<PXFieldOptions.CommitChanges>;
	PayrollVerLastModifiedDateTime: PXFieldState;
	PayrollVerLastModUserName: PXFieldState;
	PayrollAshAutire: PXFieldState;
	RKAccess: PXFieldState;
	RKAccessAutire: PXFieldState;
	RecordKeeperName: PXFieldState;
	PayrollProvider: PXFieldState;
	Finch: PXFieldState;
	FinchLastModifiedDateTime: PXFieldState;
	FinchLastModUserName: PXFieldState;
	FirstContactDatePayroll: PXFieldState;
	NotesPayroll: PXFieldState<PXFieldOptions.Multiline>;
	PayrollComplete: PXFieldState;
	PayrollPreparer: PXFieldState;

	// Plan Documents
	SPD: PXFieldState<PXFieldOptions.CommitChanges>;
	SPDLastModifiedDateTime: PXFieldState;
	SPDLastModUserName: PXFieldState;
	SPDAshAutire: PXFieldState;
	AA: PXFieldState<PXFieldOptions.CommitChanges>;
	AALastModifiedDateTime: PXFieldState;
	AALastModUserName: PXFieldState;
	AAAshAutire: PXFieldState;
	BPD: PXFieldState<PXFieldOptions.CommitChanges>;
	BPDLastModifiedDateTime: PXFieldState;
	BPDLastModUserName: PXFieldState;
	BPDAshAutire: PXFieldState;
	IRS: PXFieldState<PXFieldOptions.CommitChanges>;
	IRSLastModifiedDateTime: PXFieldState;
	IRSLastModUserName: PXFieldState;
	IRSAshAutire: PXFieldState;
	SATrustree: PXFieldState<PXFieldOptions.CommitChanges>;
	SATrustreeLastModifiedDateTime: PXFieldState;
	SATrustreeLastModUserName: PXFieldState;
	SATrustreeAshAutire: PXFieldState;
	SATPA: PXFieldState<PXFieldOptions.CommitChanges>;
	SATPALastModifiedDateTime: PXFieldState;
	SATPALastModUserName: PXFieldState;
	SATPAAshAutire: PXFieldState;
	SAInvestment: PXFieldState<PXFieldOptions.CommitChanges>;
	SAInvestmentLastModifiedDateTime: PXFieldState;
	SAInvestmentLastModUserName: PXFieldState;
	SAInvestmentAshAutire: PXFieldState;
	LoanPolicy: PXFieldState<PXFieldOptions.CommitChanges>;
	LoanPolicyLastModifiedDateTime: PXFieldState;
	LoanPolicyLastModUserName: PXFieldState;
	LoanPolicyAshAutire: PXFieldState;
	QDIA: PXFieldState<PXFieldOptions.CommitChanges>;
	QDIALastModifiedDateTime: PXFieldState;
	QDIALastModUserName: PXFieldState;
	QDIAAshAutire: PXFieldState;
	FirstContactDatePlanDocuments: PXFieldState;
	NotesPlanDocuments: PXFieldState<PXFieldOptions.Multiline>;
	PlanDocumentsComplete: PXFieldState;
	PlanDocPreparer: PXFieldState;

	// PlanDoc Preprocessing
	EC: PXFieldState<PXFieldOptions.CommitChanges>;
	ECLastModifiedDateTime: PXFieldState;
	ECLastModUserName: PXFieldState;
	ECAshAutire: PXFieldState;
	PAS: PXFieldState<PXFieldOptions.CommitChanges>;
	PASLastModifiedDateTime: PXFieldState;
	PASLastModUserName: PXFieldState;
	PASAshAutire: PXFieldState;
	Cont: PXFieldState<PXFieldOptions.CommitChanges>;
	ContLastModifiedDateTime: PXFieldState;
	ContLastModUserName: PXFieldState;
	ContAshAutire: PXFieldState;
	Dist: PXFieldState<PXFieldOptions.CommitChanges>;
	DistLastModifiedDateTime: PXFieldState;
	DistLastModUserName: PXFieldState;
	DistAshAutire: PXFieldState;
	Loans: PXFieldState<PXFieldOptions.CommitChanges>;
	LoansLastModifiedDateTime: PXFieldState;
	LoansLastModUserName: PXFieldState;
	LoansAshAutire: PXFieldState;
	DefElection: PXFieldState<PXFieldOptions.CommitChanges>;
	DefElectionLastModifiedDateTime: PXFieldState;
	DefElectionLastModUserName: PXFieldState;
	DefElectionAshAutire: PXFieldState;
	FirstContactDatePlanDocPreproc: PXFieldState;
	NotesPlanDocPreproc: PXFieldState<PXFieldOptions.Multiline>;
	PlanDocPreprocComplete: PXFieldState;

	// Plan Design
	PlanProvisions: PXFieldState<PXFieldOptions.CommitChanges>;
	PlanProvisionsLastModifiedDateTime: PXFieldState;
	PlanProvisionsLastModUserName: PXFieldState;
	PlanProvisionsAshAutire: PXFieldState;
	PreProcessDate: PXFieldState<PXFieldOptions.CommitChanges>;
	PreProcessDateLastModifiedDateTime: PXFieldState;
	PreProcessDateLastModUserName: PXFieldState;
	PreProcessDateAshAutire: PXFieldState;
	FirstContactDatePlanDesign: PXFieldState;
	NotesPlanDesign: PXFieldState<PXFieldOptions.Multiline>;
	PlanDesignComplete: PXFieldState;
	PlanDesignPreparer: PXFieldState;

	// Annual Documents
	Form5500: PXFieldState<PXFieldOptions.CommitChanges>;
	Form5500LastModifiedDateTime: PXFieldState;
	Form5500LastModUserName: PXFieldState;
	Form5500AshAutire: PXFieldState;
	TrustState: PXFieldState<PXFieldOptions.CommitChanges>;
	TrustStateLastModifiedDateTime: PXFieldState;
	TrustStateLastModUserName: PXFieldState;
	TrustStateAshAutire: PXFieldState;
	TrustCert: PXFieldState<PXFieldOptions.CommitChanges>;
	TrustCertLastModifiedDateTime: PXFieldState;
	TrustCertLastModUserName: PXFieldState;
	TrustCertAshAutire: PXFieldState;
	FidelityBond: PXFieldState<PXFieldOptions.CommitChanges>;
	FidelityBondLastModifiedDateTime: PXFieldState;
	FidelityBondLastModUserName: PXFieldState;
	FidelityBondAshAutire: PXFieldState;
	ComplianceTest: PXFieldState<PXFieldOptions.CommitChanges>;
	ComplianceTestLastModifiedDateTime: PXFieldState;
	ComplianceTestLastModUserName: PXFieldState;
	ComplianceTestAshAutire: PXFieldState;
	RKFeeDisc: PXFieldState<PXFieldOptions.CommitChanges>;
	RKFeeDiscLastModifiedDateTime: PXFieldState;
	RKFeeDiscLastModUserName: PXFieldState;
	RKFeeDiscAshAutire: PXFieldState;
	InvestAllocation: PXFieldState<PXFieldOptions.CommitChanges>;
	InvestAllocationLastModifiedDateTime: PXFieldState;
	InvestAllocationLastModUserName: PXFieldState;
	InvestAllocationAshAutire: PXFieldState;
	FirstContactDateAnnualDocuments: PXFieldState;
	NotesAnnualDocuments: PXFieldState<PXFieldOptions.Multiline>;
	AnnualDocumentsComplete: PXFieldState;

	// First Year PK Client
	ClientAuth: PXFieldState<PXFieldOptions.CommitChanges>;
	ClientAuthLastModifiedDateTime: PXFieldState;
	ClientAuthLastModUserName: PXFieldState;
	ClientAuthAshAutire: PXFieldState;
	Communication: PXFieldState<PXFieldOptions.CommitChanges>;
	CommunicationLastModifiedDateTime: PXFieldState;
	CommunicationLastModUserName: PXFieldState;
	CommunicationAshAutire: PXFieldState;
	PY5500: PXFieldState<PXFieldOptions.CommitChanges>;
	PY5500LastModifiedDateTime: PXFieldState;
	PY5500LastModUserName: PXFieldState;
	PY5500AshAutire: PXFieldState;
	PYTrustState: PXFieldState<PXFieldOptions.CommitChanges>;
	PYTrustStateLastModifiedDateTime: PXFieldState;
	PYTrustStateLastModUserName: PXFieldState;
	PYTrustStateAshAutire: PXFieldState;
	PYTrustCert: PXFieldState<PXFieldOptions.CommitChanges>;
	PYTrustCertLastModifiedDateTime: PXFieldState;
	PYTrustCertLastModUserName: PXFieldState;
	PYTrustCertAshAutire: PXFieldState;
	PYCompliance: PXFieldState<PXFieldOptions.CommitChanges>;
	PYComplianceLastModifiedDateTime: PXFieldState;
	PYComplianceLastModUserName: PXFieldState;
	PYComplianceAshAutire: PXFieldState;
	FirstContactDateFirstYearPK: PXFieldState;
	NotesFirstYearPK: PXFieldState<PXFieldOptions.Multiline>;
	FirstYearPKComplete: PXFieldState;
}
