using System;
using System.Collections;
using PX.Data;
using PX.Common;
using PX.Objects.PM;
using PX.Objects.CT;
using PX.Data.BQL.Fluent;
using PX.Objects.EP;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using PX.Data.EP;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.SM;
using PX.TM;
using PX.Api;
using PX.Objects;


namespace ProjectTask
{
  public class PMDocGatheringMaint  : PXGraph<PMDocGatheringMaint, PMDocumentGathering>
  {

        public SelectFrom<PMDocumentGathering>.View DocumentGathering;
    
        public PXSelectJoin<PMDocumentGathering,
          LeftJoin<PMProject, On<PMDocumentGathering.projectID, Equal<PMProject.contractID>>>,
          Where<PMProject.nonProject, Equal<False>, 
              And<PMProject.baseType, Equal<PX.Objects.CT.CTPRType.project>>>> DocGathering;

        public PXSelectJoin<PMTask,
         InnerJoin<PMDocumentGathering, On<PMTask.projectID, Equal<Current<PMDocumentGathering.projectID>>>>,
         Where<PMTask.taskCD, Equal<ConstantTaskCD.payroll>>> PayrollProperties;


        //public PXSelectJoin<PMDocumentGathering,
        //  LeftJoin<PMTask, On<PMDocumentGathering.projectID, Equal<PMTask.projectID>>>,
        //  Where<PMTask.taskCD, Equal<ConstantTaskCD.payroll>,
        //      And<PMDocumentGathering.projectID, Equal<Current<PMDocumentGathering.projectID>>>>> PayrollProperties;

        public PXSelectJoin<PMDocumentGathering,
          LeftJoin<PMTask, On<PMDocumentGathering.projectID, Equal<PMTask.projectID>>>,
          Where<PMTask.taskCD, Equal<ConstantTaskCD.plandoc>,
              And<PMDocumentGathering.projectID, Equal<Current<PMDocumentGathering.projectID>>>>> PlanDocProperties;

        public PXSelectJoin<PMDocumentGathering,
          LeftJoin<PMTask, On<PMDocumentGathering.projectID, Equal<PMTask.projectID>>>,
          Where<PMTask.taskCD, Equal<ConstantTaskCD.plandesign>,
              And<PMDocumentGathering.projectID, Equal<Current<PMDocumentGathering.projectID>>>>> PlanDesignProperties;

        public PXSelect<PMDocumentGathering, Where<PMDocumentGathering.projectID, Equal<Current<PMDocumentGathering.projectID>>>> DocGatheringProperties;


        // public PXSave<PMDocumentGathering> Save;
        // public PXCancel<PMDocumentGathering> Cancel;




        //Payroll Preprocessing Basic Requirement

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.mpr> e)
        {
      
          var row = e.Row;
          row.MPRLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.MPRLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDatePayroll == null)
          {
            row.FirstContactDatePayroll = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }
    
        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.payrollVerification> e)
        {
      
          var row = e.Row;
          row.PayrollVerLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.PayrollVerLastModUserName = base.Accessinfo.UserName;

          if (row.FirstContactDatePayroll == null)
          {
            row.FirstContactDatePayroll = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
      
        }



        //Plan Document

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.spd> e)
        {
      
          var row = e.Row;
          row.SPDLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.SPDLastModUserName = base.Accessinfo.UserName;

          if (row.FirstContactDatePlanDocuments == null)
          {
            row.FirstContactDatePlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.aA> e)
        {
      
          var row = e.Row;
          row.AALastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.AALastModUserName = base.Accessinfo.UserName;

          if (row.FirstContactDatePlanDocuments == null)
          {
            row.FirstContactDatePlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }

      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.bpd> e)
        {
      
          var row = e.Row;
          row.BPDLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.BPDLastModUserName = base.Accessinfo.UserName;

          if (row.FirstContactDatePlanDocuments == null)
          {
            row.FirstContactDatePlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.irs> e)
        {
      
          var row = e.Row;
          row.IRSLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.IRSLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDatePlanDocuments == null)
          {
            row.FirstContactDatePlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.sATrustree> e)
        {
      
          var row = e.Row;
          row.SATrustreeLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.SATrustreeLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDatePlanDocuments == null)
          {
            row.FirstContactDatePlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.sATPA> e)
        {
      
          var row = e.Row;
          row.SATPALastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.SATPALastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDatePlanDocuments == null)
          {
            row.FirstContactDatePlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.sAInvestment> e)
        {
      
          var row = e.Row;
          row.SAInvestmentLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.SAInvestmentLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDatePlanDocuments == null)
          {
            row.FirstContactDatePlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.loanPolicy> e)
        {
      
          var row = e.Row;
          row.LoanPolicyLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.LoanPolicyLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDatePlanDocuments == null)
          {
            row.FirstContactDatePlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.qDIA> e)
        {
      
          var row = e.Row;
          row.QDIALastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.QDIALastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDatePlanDocuments == null)
          {
            row.FirstContactDatePlanDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }

        }



        //PlanDoc Preprocessing Basic Requirement

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.eC> e)
        {
      
          var row = e.Row;
          row.ECLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.ECLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDatePlanDocPreproc == null)
          {
            row.FirstContactDatePlanDocPreproc = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.pAS> e)
        {
      
          var row = e.Row;
          row.PASLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.PASLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDatePlanDocPreproc == null)
          {
            row.FirstContactDatePlanDocPreproc = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.cont> e)
        {
      
          var row = e.Row;
          row.ContLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.ContLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDatePlanDocPreproc == null)
          {
            row.FirstContactDatePlanDocPreproc = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.dist> e)
        {
      
          var row = e.Row;
          row.DistLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.DistLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDatePlanDocPreproc == null)
          {
            row.FirstContactDatePlanDocPreproc = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.loans> e)
        {
      
          var row = e.Row;
          row.LoansLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.LoansLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDatePlanDocPreproc == null)
          {
            row.FirstContactDatePlanDocPreproc = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.defElection> e)
        {
      
          var row = e.Row;
          row.DefElectionLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.DefElectionLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDatePlanDocPreproc == null)
          {
            row.FirstContactDatePlanDocPreproc = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        // Plan Design

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.planProvisions> e)
        {
      
          var row = e.Row;
          row.PlanProvisionsLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.PlanProvisionsLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDatePlanDesign == null)
          {
            row.FirstContactDatePlanDesign = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.preProcessDate> e)
        {
      
          var row = e.Row;
          row.PreProcessDateLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.PreProcessDateLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDatePlanDesign == null)
          {
            row.FirstContactDatePlanDesign = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        //Annual Documents

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.form5500> e)
        {
      
          var row = e.Row;
          row.Form5500LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.Form5500LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDateAnnualDocuments == null)
          {
            row.FirstContactDateAnnualDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }
  
        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.trustState> e)
        {
      
          var row = e.Row;
          row.TrustStateLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.TrustStateLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDateAnnualDocuments == null)
          {
            row.FirstContactDateAnnualDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.trustCert> e)
        {
      
          var row = e.Row;
          row.TrustCertLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.TrustCertLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDateAnnualDocuments == null)
          {
            row.FirstContactDateAnnualDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.fidelityBond> e)
        {
      
          var row = e.Row;
          row.FidelityBondLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.FidelityBondLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDateAnnualDocuments == null)
          {
            row.FirstContactDateAnnualDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.complianceTest> e)
        {
      
          var row = e.Row;
          row.ComplianceTestLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.ComplianceTestLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDateAnnualDocuments == null)
          {
            row.FirstContactDateAnnualDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.rKFeeDisc> e)
        {
      
          var row = e.Row;
          row.RKFeeDiscLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.RKFeeDiscLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDateAnnualDocuments == null)
          {
            row.FirstContactDateAnnualDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.investAllocation> e)
        {
      
          var row = e.Row;
          row.InvestAllocationLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.InvestAllocationLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDateAnnualDocuments == null)
          {
            row.FirstContactDateAnnualDocuments = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }


        //First Year PK Client

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.clientAuth> e)
        {
      
          var row = e.Row;
          row.ClientAuthLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.ClientAuthLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDateFirstYearPK == null)
          {
            row.FirstContactDateFirstYearPK = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.communication> e)
        {
      
          var row = e.Row;
          row.CommunicationLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.CommunicationLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDateFirstYearPK == null)
          {
            row.FirstContactDateFirstYearPK = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.pY5500> e)
        {
      
          var row = e.Row;
          row.PY5500LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.PY5500LastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDateFirstYearPK == null)
          {
            row.FirstContactDateFirstYearPK = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.pYTrustState> e)
        {
      
          var row = e.Row;
          row.PYTrustStateLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.PYTrustStateLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDateFirstYearPK == null)
          {
            row.FirstContactDateFirstYearPK = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.pYTrustCert> e)
        {
      
          var row = e.Row;
          row.PYTrustCertLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.PYTrustCertLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDateFirstYearPK == null)
          {
            row.FirstContactDateFirstYearPK = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

        protected void _(Events.FieldUpdated<PMDocumentGathering, PMDocumentGathering.pYCompliance> e)
        {
      
          var row = e.Row;
          row.PYComplianceLastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
          row.PYComplianceLastModUserName = (string)base.Accessinfo.UserName;

          if (row.FirstContactDateFirstYearPK == null)
          {
            row.FirstContactDateFirstYearPK = PX.Common.PXTimeZoneInfo.Now.Date;
          }
      
        }

  }
}