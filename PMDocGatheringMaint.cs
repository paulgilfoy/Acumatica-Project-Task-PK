using System;
using System.Collections;
using PX.Data;
using PX.Objects.PM;
using PX.Objects.CT;
using PX.Data.BQL.Fluent;
using PX.Objects.EP;

namespace ProjectTask
{
  public class PMDocGatheringMaint  : PXGraph<PMDocGatheringMaint, PMDocumentGathering>
  {
        private class Task : Constant<string>
        {
            public Payroll() : base("02PAYROLL") { }
            public PlanDoc() : base("03PLANDOC") { }
            public PlanDesign() : base("04PLANDESIGN") { }
        }

        public SelectFrom<PMDocumentGathering>.View DocumentGathering;
    
        public PXSelectJoin<PMDocumentGathering,
          LeftJoin<PMProject, On<PMDocumentGathering.projectID, Equal<PMProject.contractID>>>,
          Where<PMProject.nonProject, Equal<False>, 
              And<PMProject.baseType, Equal<PX.Objects.CT.CTPRType.project>>>> DocGathering;
    
	    public PXSelect<PMDocumentGathering, Where<PMDocumentGathering.projectID, Equal<Current<PMDocumentGathering.projectID>>>> DocGatheringProperties;

        public PXSelectJoin<PMDocumentGathering,
            LeftJoin<PMTask, On<PMDocumentGathering.projectID, Equal<PMTask.projectID>>>,
            Where<PMTask.projectID, Equal<Current<PMDocumentGathering.projectID>>,
                And<PMTask.taskCD, Equal<Task>>>> Task_Payroll;



        public PXSave<PMDocumentGathering> Save;
        public PXCancel<PMDocumentGathering> Cancel;
        
        
        protected void PMDocumentGathering_RowInserting(PXCache cache, PXRowInsertingEventArgs e, PXRowInserting InvokeBaseHandler)
        {
            PMDocumentGathering row = (PMDocumentGathering)e.Row;
            PMDocumentGathering project = PXSelect<PMDocumentGathering, Where<PMDocumentGathering.projectID, Equal<Current<PMDocumentGathering.projectID>>>>.Select(this);
            if (row != null)
            {
                row.ProjectID = project.ProjectID;
            }
            else if (project == null) { return; }

            if (InvokeBaseHandler != null)
                InvokeBaseHandler(cache, e);

        }


        //Payroll Preprocessing Basic Requirement

        protected void PMDocumentGathering_MPR_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.MPR_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.MPR_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_Payroll == null)
          {
            row.FirstContactDate_Payroll = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }
    
        protected void PMDocumentGathering_PayrollVerification_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.PayrollVer_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.PayrollVer_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_Payroll == null)
          {
            row.FirstContactDate_Payroll = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
      
        }



        //Plan Document

        protected void PMDocumentGathering_SPD_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.SPD_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.SPD_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_PlanDocuments == null)
          {
            row.FirstContactDate_PlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_AA_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.AA_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.AA_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_PlanDocuments == null)
          {
            row.FirstContactDate_PlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }

      
        }

        protected void PMDocumentGathering_BPD_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.BPD_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.BPD_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_PlanDocuments == null)
          {
            row.FirstContactDate_PlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_IRS_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.IRS_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.IRS_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_PlanDocuments == null)
          {
            row.FirstContactDate_PlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_SATrustree_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.SATrustree_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.SATrustree_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_PlanDocuments == null)
          {
            row.FirstContactDate_PlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_SATPA_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.SATPA_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.SATPA_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_PlanDocuments == null)
          {
            row.FirstContactDate_PlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_SAInvestment_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.SAInvestment_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.SAInvestment_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_PlanDocuments == null)
          {
            row.FirstContactDate_PlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_LoanPolicy_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.LoanPolicy_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.LoanPolicy_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_PlanDocuments == null)
          {
            row.FirstContactDate_PlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_QDIA_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.QDIA_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.QDIA_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_PlanDocuments == null)
          {
            row.FirstContactDate_PlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }

        }



        //PlanDoc Preprocessing Basic Requirement

        protected void PMDocumentGathering_EC_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.EC_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.EC_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_PlanDocPreproc == null)
          {
            row.FirstContactDate_PlanDocPreproc = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_PAS_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.PAS_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.PAS_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_PlanDocPreproc == null)
          {
            row.FirstContactDate_PlanDocPreproc = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_Cont_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.Cont_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.Cont_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_PlanDocPreproc == null)
          {
            row.FirstContactDate_PlanDocPreproc = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_Dist_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.Dist_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.Dist_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_PlanDocPreproc == null)
          {
            row.FirstContactDate_PlanDocPreproc = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_Loans_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.Loans_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.Loans_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_PlanDocPreproc == null)
          {
            row.FirstContactDate_PlanDocPreproc = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_DefElection_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.DefElection_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.DefElection_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_PlanDocPreproc == null)
          {
            row.FirstContactDate_PlanDocPreproc = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        // Plan Design

        protected void PMDocumentGathering_PlanProvisions_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.PlanProvisions_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.PlanProvisions_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_PlanDesign == null)
          {
            row.FirstContactDate_PlanDesign = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_PreProcessDate_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.PreProcessDate_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.PreProcessDate_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_PlanDesign == null)
          {
            row.FirstContactDate_PlanDesign = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        //Annual Documents

        protected void PMDocumentGathering_Form_5500_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.Form_5500_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.Form_5500_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_AnnualDocuments == null)
          {
            row.FirstContactDate_AnnualDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_TrustState_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.TrustState_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.TrustState_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_AnnualDocuments == null)
          {
            row.FirstContactDate_AnnualDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_TrustCert_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.TrustCert_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.TrustCert_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_AnnualDocuments == null)
          {
            row.FirstContactDate_AnnualDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_FidelityBond_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.FidelityBond_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.FidelityBond_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_AnnualDocuments == null)
          {
            row.FirstContactDate_AnnualDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_ComplianceTest_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.ComplianceTest_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.ComplianceTest_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_AnnualDocuments == null)
          {
            row.FirstContactDate_AnnualDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_RKFeeDisc_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.RKFeeDisc_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.RKFeeDisc_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_AnnualDocuments == null)
          {
            row.FirstContactDate_AnnualDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_InvestAllocation_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.InvestAllocation_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.InvestAllocation_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_AnnualDocuments == null)
          {
            row.FirstContactDate_AnnualDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }


        //First Year PK Client

        protected void PMDocumentGathering_ClientAuth_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.ClientAuth_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.ClientAuth_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_FirstYearPK == null)
          {
            row.FirstContactDate_FirstYearPK = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_Communication_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.Communication_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.Communication_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_FirstYearPK == null)
          {
            row.FirstContactDate_FirstYearPK = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_PY5500_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.PY5500_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.PY5500_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_FirstYearPK == null)
          {
            row.FirstContactDate_FirstYearPK = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_PYTrustState_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.PYTrustState_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.PYTrustState_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_FirstYearPK == null)
          {
            row.FirstContactDate_FirstYearPK = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_PYTrustCert_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.PYTrustCert_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.PYTrustCert_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_FirstYearPK == null)
          {
            row.FirstContactDate_FirstYearPK = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void PMDocumentGathering_PYCompliance_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
      
          var row = (PMDocumentGathering)e.Row;
          row.PYCompliance_LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.PYCompliance_LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDate_FirstYearPK == null)
          {
            row.FirstContactDate_FirstYearPK = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

  }
}