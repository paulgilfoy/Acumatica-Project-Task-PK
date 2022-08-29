

ContractExt.usr401kStatus
[PXDBInt]
[PXIntList(new int[] {01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12, 13, 14, 15, 16, 17}, new string[] {"Setup/Engagement Letter/Invoice", "Plan Doc Gather and Plan Design", "Payroll Doc Gather", "Data Preprocessing and Validation", "Initial RECs and DIs", "EIC Inquiries", "Risk Assessment and Sample Selection", "Sample Testing", "Remaining Audit Steps", "Deficiency Resolution", "In Initial Review", "Preparation of Final Deliverables", "Technical Review", "Waiting on Management Rep Letter", "Final Bill", "Complete", "Waiting on 5500"})]
[PXUIField(DisplayName="401k Status")]


ContractExt.usrPGDepartment
[PXDBString(10)]
[PXSelector(typeof(PX.Objects.EP.EPDepartment.departmentID))]
[PXUIField(DisplayName="Department")]


ContractExt.usrPGPriority
[PXDBInt]
[PXIntList(new int[] {01, 02, 03}, new string[] {"Low", "Medium", "High"})]
[PXUIField(DisplayName="Project Priority")]


ContractExt.usrPGTaxStatus
[PXDBInt]
[PXIntList(new int[] {01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12, 13, 14}, new string[] {
    "Ready to Assign", 
    "Information Requested (from Client)", 
    "Information Requested (In-House)", 
    "Ready to Prep", 
    "In Preparation", 
    "In Initial Review", 
    "Clearing Points", 
    "In Final Review", 
    "Technical Review", 
    "Ready to Deliver", 
    "Bill Client", 
    "Invoice", 
    "Complete", 
    "Waiting on 8879 Form"})]
[PXUIField(DisplayName="Tax Status")]


ContractExt.usrPGAcctStatus
[PXDBInt]
[PXIntList(new int[] {01, 02, 03, 04, 05, 06, 07, 08}, new string[] {"In Preparation", "Initial Review (Acctg)", "Clearing Points", "In Final Review", "Bill Client", "Invoice", "Complete", "Meet with Client"})]
[PXUIField(DisplayName="Acct Status")]


ContractExt.usrPGAuditStatus
[PXDBInt]
[PXIntList(new int[] {01, 02, 03, 04}, new string[] {"Audit Status 1", "Audit Status 2", "Audit Status 3", "Audit Status 4"})]
[PXUIField(DisplayName="Audit Status")]


ContractExt.usrPGTechStatus
[PXDBInt]
[PXIntList(new int[] {01, 02, 03, 04}, new string[] {"Tech Status 1", "Tech Status 2", "Tech Status 3", "Tech Status 4"})]
[PXUIField(DisplayName="Tech Status")]


=Switch( 
    [PMProject.UsrPGDepartment]='TAX', [PMProject.UsrPGTaxStatus_Value], 
    [PMProject.UsrPGDepartment]='401K', [PMProject.Usr401kStatus_Value], 
    [PMProject.UsrPGDepartment]='ACCT', [PMProject.UsrPGAcctStatus], 
    [PMProject.UsrPGDepartment]='TECH', [PMProject.UsrPGTechStatus], 
    [PMProject.UsrPGDepartment]='AUDIT', [PMProject.UsrPGAuditStatus])


=Switch(     
    [PMProject.UsrPGDepartment]='TAX', [TAX.Description],     
    [PMProject.UsrPGDepartment]='401K', [401K.Description],     
    [PMProject.UsrPGDepartment]='ACCT', [ACCT.Description],     
    [PMProject.UsrPGDepartment]='TECH', [ACCT.Description],     
    [PMProject.UsrPGDepartment]='AUDIT', [ACCT.Description])
