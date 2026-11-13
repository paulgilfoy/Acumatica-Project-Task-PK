import {
	createSingle,
	graphInfo,
	PXScreen,
} from "client-controls";

import {
	DocGathering,
	DocGatheringProperties,
} from "./views";

@graphInfo({
	graphType: "ProjectTask.PMDocGatheringMaint",
	primaryView: "DocGathering",
})
export class PKPM3001 extends PXScreen {
	DocGathering = createSingle(DocGathering);
	DocGatheringProperties = createSingle(DocGatheringProperties);
}
