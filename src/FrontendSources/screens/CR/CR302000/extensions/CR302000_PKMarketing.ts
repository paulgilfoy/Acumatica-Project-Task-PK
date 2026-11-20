import {
	PXFieldState,
	PXFieldOptions,
	placeAfterProperty,
} from "client-controls";
import { CR302000, ContactDetail } from "src/screens/CR/CR302000/CR302000";

export interface CR302000_PKMarketing extends CR302000 {}
export class CR302000_PKMarketing {}

export interface CR302000_ContactDetail_PKMarketing extends ContactDetail {}
export class CR302000_ContactDetail_PKMarketing {
	@placeAfterProperty("ConsentExpirationDate")
	UsrPG1040Check: PXFieldState;

	UsrPG1040ES: PXFieldState;

	UsrPGAcuMarket: PXFieldState;

	UsrPGDoNotMarket: PXFieldState;

	UsrPGPortalAccess: PXFieldState;

	UsrPGRetire: PXFieldState;

	UsrPGTaxPlan: PXFieldState;

	UsrServiceProviderQ: PXFieldState;
}
