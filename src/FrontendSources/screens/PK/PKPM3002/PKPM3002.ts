import {
	createSingle,
	graphInfo,
	PXScreen,
} from "client-controls";

import {
	Sampletest,
	SampletestProperties,
} from "./views";

@graphInfo({
	graphType: "ProjectTask.PMSampleTestingMaint",
	primaryView: "Sampletest",
})
export class PKPM3002 extends PXScreen {
	Sampletest = createSingle(Sampletest);
	SampletestProperties = createSingle(SampletestProperties);
}
