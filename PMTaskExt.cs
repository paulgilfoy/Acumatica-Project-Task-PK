
PMTaskExt.usrPGDueDate
[PXDBDate]
[PXUIField(DisplayName="Due Date")]


PMTaskExt.usrPGExtendedDueDate
[PXDBDate]
[PXUIField(DisplayName="Extended Due Date")]


PMTaskExt.usrPGIsExtended
[PXDBBool]
[PXUIField(DisplayName="Extended")]


PMTaskExt.usrPGPreparer
[PXDBInt]
[PXEPEmployeeSelector]
[PXUIField(DisplayName = "Preparer", Visibility = PXUIVisibility.SelectorVisible)]



PMTaskExt.usrPGReviewer
[PXDBInt]
[PXEPEmployeeSelector]
[PXUIField(DisplayName = "Reviewer", Visibility = PXUIVisibility.SelectorVisible)]


PMTaskExt.usrPGTaxStatus
[PXDBInt]
[PXIntList(new int[] {01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12, 13, 14}, new string[] {"Ready to Assign", "Information Requested (from Client)", "Information Requested (In-House)", "Ready to Prep", "In Preparation", "In Initial Review", "Clearing Points", "In Final Review", "Technical Review", "Ready to Deliver", "Bill Client", "Invoice", "Complete", "Waiting on 8879 Form"})]
[PXUIField(DisplayName="Tax Status")]


PMTaskExt.usrPGTechReviewer
[PXDBInt]
[PXEPEmployeeSelector]
[PXUIField(DisplayName = "Tech Reviewer", Visibility = PXUIVisibility.SelectorVisible)]

