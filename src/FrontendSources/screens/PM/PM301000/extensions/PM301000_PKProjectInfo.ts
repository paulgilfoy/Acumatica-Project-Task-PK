import {
	PXFieldState,
	PXFieldOptions,
	placeAfterProperty,
} from "client-controls";
import { PM301000 } from "src/screens/PM/PM301000/PM301000";
import { ProjectProperties, Tasks } from "src/screens/PM/PM301000/views";

export interface PM301000_PKProjectInfo extends PM301000 {}
export class PM301000_PKProjectInfo {}

export interface PM301000_ProjectProperties_PKProjectInfo extends ProjectProperties {}
export class PM301000_ProjectProperties_PKProjectInfo {
	@placeAfterProperty("TagTemplateID")
	UsrPGDepartment: PXFieldState<PXFieldOptions.CommitChanges>;

	UsrPGPriority: PXFieldState;

	UsrPGIsFinal: PXFieldState;

	UsrPGFinishDate: PXFieldState;

	UsrPGYearEnd: PXFieldState;

	Usr401kStatus: PXFieldState;

	UsrPGComplexity: PXFieldState;

	UsrPGReleaseDeliverables: PXFieldState;

	UsrPGAcctStatus: PXFieldState;

	UsrPGAuditStatus: PXFieldState;

	UsrPGTaxStatus: PXFieldState;

	UsrPGReleasedELF: PXFieldState;

	UsrPGTechStatus: PXFieldState;
}

export interface PM301000_Tasks_PKProjectInfo extends Tasks {}
export class PM301000_Tasks_PKProjectInfo {
	@placeAfterProperty("CompletedPctMethod")
	UsrPGTaskStatus: PXFieldState;

	UsrPGDueDate: PXFieldState;

	UsrPGExtendedDueDate: PXFieldState;

	UsrPGIsExtended: PXFieldState;

	UsrPGPreparer: PXFieldState;

	UsrPGReviewer: PXFieldState;

	UsrPGTechReviewer: PXFieldState;

	UsrPGDepartment: PXFieldState;
}
