import {
	PXFieldState,
	PXFieldOptions,
	placeAfterProperty,
} from "client-controls";
import { PM208000 } from "src/screens/PM/PM208000/PM208000";
import { ProjectProperties, Tasks } from "src/screens/PM/PM208000/views";

// Project Templates (PM208000): PK project and task defaults stored on the template (T-31).
// Coexists with PKTimeClock's PM208000_PKTimeClock (UsrPKLaborItemID on the same ProjectProperties view).
export interface PM208000_PKProjectInfo extends PM208000 {}
export class PM208000_PKProjectInfo {}

export interface PM208000_ProjectProperties_PKProjectInfo extends ProjectProperties {}
export class PM208000_ProjectProperties_PKProjectInfo {
	@placeAfterProperty("TagTemplateID")
	UsrPGDepartment: PXFieldState<PXFieldOptions.CommitChanges>;

	UsrPGPriority: PXFieldState;

	UsrPGComplexity: PXFieldState;
}

export interface PM208000_Tasks_PKProjectInfo extends Tasks {}
export class PM208000_Tasks_PKProjectInfo {
	@placeAfterProperty("Description")
	UsrPGDueDate: PXFieldState;

	UsrPGPreparer: PXFieldState;

	UsrPGReviewer: PXFieldState;

	UsrPGTechReviewer: PXFieldState;
}
