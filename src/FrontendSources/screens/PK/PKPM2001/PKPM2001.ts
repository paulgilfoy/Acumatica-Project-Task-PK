import {
	createCollection,
	createSingle,
	graphInfo,
	PXScreen,
	viewInfo,
} from "client-controls";

import {
	PKProjectStatusFilter,
	PKProjectStatus,
} from "./views";

@graphInfo({
	graphType: "ProjectTask.PKProjectStatusMaint",
	primaryView: "Filter",
})
export class PKPM2001 extends PXScreen {
	@viewInfo({ containerName: "Selection" })
	Filter = createSingle(PKProjectStatusFilter);

	@viewInfo({ containerName: "Statuses" })
	Statuses = createCollection(PKProjectStatus);
}
