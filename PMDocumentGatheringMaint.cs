using System;
using System.Collections;
using PX.Data;
using PX.Objects.PM;
using PX.Data.BQL.Fluent;

namespace ProjectTask
{
  public class PMDocumentGatheringMaint  : PXGraph<PMDocumentGatheringMaint, PMDocumentGathering>
  {
    
    public SelectFrom<PMDocumentGathering>.View DocumentGathering;
    
    public PXSelectJoin<PMDocumentGathering,
      LeftJoin<PMProject, On<PMDocumentGathering.projectID, Equal<PMProject.contractID>>>,
      Where<PMProject.nonProject, Equal<False>>> DocGathering;
    
		public PXSelect<PMDocumentGathering, Where<PMDocumentGathering.projectID, Equal<Current<PMDocumentGathering.projectID>>>> DocGatheringProperties;
    
    public PXSave<PMDocumentGathering> Save;
    public PXCancel<PMDocumentGathering> Cancel;


    public PXFilter<MasterTable> MasterView;
    public PXFilter<DetailsTable> DetailsView;

    [Serializable]
    public class MasterTable : IBqlTable
    {

    }

    [Serializable]
    public class DetailsTable : IBqlTable
    {

    }


  }
}