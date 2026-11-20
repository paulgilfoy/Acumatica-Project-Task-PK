import {
	PXFieldState,
	placeAfterProperty,
} from "client-controls";
import { PM208030 } from "src/screens/PM/PM208030/PM208030";
import { TaskProperties } from "src/screens/PM/PM208030/views";

// Common Tasks (PM208030, global common tasks on the non-project ContractID 0): PK task defaults (T-31).
export interface PM208030_PKTaskInfo extends PM208030 {}
export class PM208030_PKTaskInfo {}

export interface PM208030_TaskProperties_PKTaskInfo extends TaskProperties {}
export class PM208030_TaskProperties_PKTaskInfo {
	@placeAfterProperty("VisibleInEA")
	UsrPGDueDate: PXFieldState;

	UsrPGPreparer: PXFieldState;

	UsrPGReviewer: PXFieldState;

	UsrPGTechReviewer: PXFieldState;
}
