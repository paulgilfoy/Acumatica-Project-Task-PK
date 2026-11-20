import {
	PXView,
	PXFieldState,
	PXFieldOptions,
	gridConfig,
	GridPreset,
	GridFastFilterVisibility,
	columnConfig,
	GridColumnType,
	TextAlign,
} from "client-controls";

export class PKProjectStatusFilter extends PXView {
	DepartmentID: PXFieldState<PXFieldOptions.CommitChanges>;
}

// Editable grid (AP202000 / CS205000 pattern). StatusID is read-only on saved rows (graph RowSelected).
@gridConfig({
	preset: GridPreset.Details,
	initNewRow: true,
	showFastFilter: GridFastFilterVisibility.False,
})
export class PKProjectStatus extends PXView {
	@columnConfig({ textAlign: TextAlign.Right })
	StatusID: PXFieldState<PXFieldOptions.CommitChanges>;

	@columnConfig({ width: 300 })
	Description: PXFieldState;

	@columnConfig({ textAlign: TextAlign.Right })
	SortOrder: PXFieldState;

	@columnConfig({ textAlign: TextAlign.Center, type: GridColumnType.CheckBox })
	Active: PXFieldState;
}
