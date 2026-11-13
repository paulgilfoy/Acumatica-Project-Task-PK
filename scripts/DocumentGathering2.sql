ALTER TABLE [PMDocumentGathering]
DROP COLUMN IF EXISTS
    [Payroll_Preparer],
    [PlanDoc_Preparer],
    [PlanDesign_Preparer],
    [PayrollProvider], 
    [RKAccess],
    [RecordKeeperName];


ALTER TABLE [PMDocumentGathering]
ADD 
    [Payroll_Preparer] [int],
    [PlanDoc_Preparer] [int],
    [PlanDesign_Preparer] [int],
    [PayrollProvider] [nvarchar](3), 
    [RKAccess] [nvarchar](2),
    [RecordKeeperName] [nvarchar](3);