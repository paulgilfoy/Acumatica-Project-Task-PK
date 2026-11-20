import {
	PXFieldState,
	placeAfterProperty,
} from "client-controls";
import { PM208010 } from "src/screens/PM/PM208010/PM208010";
import { TaskProperties } from "src/screens/PM/PM208010/views";

// Project Template Tasks (PM208010): PK task defaults stored on the template task (T-31).
export interface PM208010_PKTaskInfo extends PM208010 {}
export class PM208010_PKTaskInfo {}

export interface PM208010_TaskProperties_PKTaskInfo extends TaskProperties {}
export class PM208010_TaskProperties_PKTaskInfo {
	@placeAfterProperty("VisibleInEA")
	UsrPGDueDate: PXFieldState;

	UsrPGPreparer: PXFieldState;

	UsrPGReviewer: PXFieldState;

	UsrPGTechReviewer: PXFieldState;
}
