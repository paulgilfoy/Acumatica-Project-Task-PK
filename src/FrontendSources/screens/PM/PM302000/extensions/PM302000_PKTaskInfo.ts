import {
	PXFieldState,
	PXFieldOptions,
	placeAfterProperty,
} from "client-controls";
import { PM302000 } from "src/screens/PM/PM302000/PM302000";
import { TaskProperties } from "src/screens/PM/PM302000/views";

export interface PM302000_PKTaskInfo extends PM302000 {}
export class PM302000_PKTaskInfo {}

export interface PM302000_TaskProperties_PKTaskInfo extends TaskProperties {}
export class PM302000_TaskProperties_PKTaskInfo {
	@placeAfterProperty("VisibleInEA")
	UsrPGDepartment: PXFieldState<PXFieldOptions.Disabled>;

	UsrPGTaskStatus: PXFieldState;

	UsrPGDueDate: PXFieldState;

	UsrPGPreparer: PXFieldState;

	UsrPGReviewer: PXFieldState;

	UsrPGTechReviewer: PXFieldState;
	
	UsrPGIsExtended: PXFieldState;

	UsrPGExtendedDueDate: PXFieldState;
}
