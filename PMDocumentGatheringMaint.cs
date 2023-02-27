using System;
using System.Collections;
using PX.Data;
using PX.Objects.PM;
using PX.Data.BQL.Fluent;

namespace ProjectTask
{
  public class PMDocGatheringMaint  : PXGraph<PMDocGatheringMaint, PMDocumentGathering>
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


    protected void PMDocumentGathering_Mpr_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
    {
      
      var row = (PMDocumentGathering)e.Row;
      row.MPR_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
      
    }
    
    protected void PMDocumentGathering_PayrollVerification_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
    {
      
      var row = (PMDocumentGathering)e.Row;
      row.PayrollVerification_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
      
    }

    protected void PMDocumentGathering_Spd_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
    {
      
      var row = (PMDocumentGathering)e.Row;
      row.SPD_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
      
    }

    protected void PMDocumentGathering_Aa_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
    {
      
      var row = (PMDocumentGathering)e.Row;
      row.AA_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;

      
    }

    protected void PMDocumentGathering_Bpd_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
    {
      
      var row = (PMDocumentGathering)e.Row;
      row.BPD_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
      
    }

    protected void PMDocumentGathering_Irs_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
    {
      
      var row = (PMDocumentGathering)e.Row;
      row.IRS_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
      
    }

    protected void PMDocumentGathering_SATrustree_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
    {
      
      var row = (PMDocumentGathering)e.Row;
      row.SATrustree_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
      
    }

    protected void PMDocumentGathering_Satpa_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
    {
      
      var row = (PMDocumentGathering)e.Row;
      row.SATPA_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
      
    }

    protected void PMDocumentGathering_SAInvestment_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
    {
      
      var row = (PMDocumentGathering)e.Row;
      row.SAInvestment_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
      
    }








    protected void PMDocumentGathering_Bpd_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
    {
      
      var row = (PMDocumentGathering)e.Row;
      row.BPD_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
      
    }

    protected void PMDocumentGathering_Bpd_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
    {
      
      var row = (PMDocumentGathering)e.Row;
      row.BPD_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
      
    }

    protected void PMDocumentGathering_Bpd_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
    {
      
      var row = (PMDocumentGathering)e.Row;
      row.BPD_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
      
    }

    protected void PMDocumentGathering_Bpd_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
    {
      
      var row = (PMDocumentGathering)e.Row;
      row.BPD_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
      
    }

    protected void PMDocumentGathering_Bpd_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
    {
      
      var row = (PMDocumentGathering)e.Row;
      row.BPD_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
      
    }


  }
}