import {
	PXView,
	PXFieldState,
	PXFieldOptions,
} from "client-controls";

export class Sampletest extends PXView {
	ProjectID: PXFieldState<PXFieldOptions.CommitChanges>;
	PMProject__Usr401kStatus: PXFieldState;
	PMProject__CustomerID_BAccountR_acctName: PXFieldState;
	Complete: PXFieldState;
	Priority: PXFieldState;
}

export class SampletestProperties extends PXView {
	CO01: PXFieldState<PXFieldOptions.CommitChanges>;
	CO01LastModifiedDateTime: PXFieldState;
	CO01LastModUserName: PXFieldState;
	CO02: PXFieldState<PXFieldOptions.CommitChanges>;
	CO02LastModifiedDateTime: PXFieldState;
	CO02LastModUserName: PXFieldState;
	CO03: PXFieldState<PXFieldOptions.CommitChanges>;
	CO03LastModifiedDateTime: PXFieldState;
	CO03LastModUserName: PXFieldState;
	CO04: PXFieldState<PXFieldOptions.CommitChanges>;
	CO04LastModifiedDateTime: PXFieldState;
	CO04LastModUserName: PXFieldState;
	CO05: PXFieldState<PXFieldOptions.CommitChanges>;
	CO05LastModifiedDateTime: PXFieldState;
	CO05LastModUserName: PXFieldState;
	CO06: PXFieldState<PXFieldOptions.CommitChanges>;
	CO06LastModifiedDateTime: PXFieldState;
	CO06LastModUserName: PXFieldState;
	CO07: PXFieldState<PXFieldOptions.CommitChanges>;
	CO07LastModifiedDateTime: PXFieldState;
	CO07LastModUserName: PXFieldState;
	CO08: PXFieldState<PXFieldOptions.CommitChanges>;
	CO08LastModifiedDateTime: PXFieldState;
	CO08LastModUserName: PXFieldState;
	BP01: PXFieldState<PXFieldOptions.CommitChanges>;
	BP01LastModifiedDateTime: PXFieldState;
	BP01LastModUserName: PXFieldState;
	LO01: PXFieldState<PXFieldOptions.CommitChanges>;
	LO01LastModifiedDateTime: PXFieldState;
	LO01LastModUserName: PXFieldState;
	IA08: PXFieldState<PXFieldOptions.CommitChanges>;
	IA08LastModifiedDateTime: PXFieldState;
	IA08LastModUserName: PXFieldState;

	AdditionalNotes: PXFieldState<PXFieldOptions.Multiline>;

	RecordKeeperName: PXFieldState;
	TPA: PXFieldState;
	Tester: PXFieldState;
	FirstContactDate: PXFieldState;
}
