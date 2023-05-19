using System;
using System.Collections;
using PX.Data;
using PX.Objects.PM;
using PX.Data.BQL.Fluent;
using PX.Objects.CT;
using PX.Objects.EP;

namespace ProjectTask
{
    /// <summary>
    /// Used in PriceKubecka's 401k audit practice. 
    /// For the purpose of tracking the gathering of relevant 401k documents 
    /// The documents will eventually be imported into Autire
    /// </summary>
    
  [Serializable]
  [PXCacheName("PMDocumentGathering")]
  public class PMDocumentGathering : IBqlTable
  {
    #region ProjectID
    public abstract class projectID : PX.Data.BQL.BqlInt.Field<projectID> { }

    /// copy from PX.Objects.PM.PMTask
    protected Int32? _ProjectID;
    [Project(DisplayName = "Project ID", IsKey = true)]
    [PXParent(typeof(Select<PMProject, Where<PMProject.contractID, Equal<Current<PMDocumentGathering.projectID>>>>))]
    [PXDBDefault(typeof(PMProject.contractID))]

        public virtual Int32? ProjectID
    {
      get
      {
        return this._ProjectID;
      }
      set
      {
        this._ProjectID = value;
      }
    }
    #endregion

    #region AdditionalNotes
    [PXDBString(255, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Additional Notes")]
    public virtual string AdditionalNotes { get; set; }
    public abstract class additionalNotes : PX.Data.BQL.BqlString.Field<additionalNotes> { }
    #endregion

    #region Active
    [PXDBBool()]
    [PXDefault(true)]
    [PXUIField(DisplayName = "Active")]
    public virtual bool? Active { get; set; }
    public abstract class active : PX.Data.BQL.BqlBool.Field<active> { }
    #endregion


    //Payroll Preprocessing

    #region FirstContactDatePayroll
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDatePayroll { get; set; }
    public abstract class firstContactDatePayroll : PX.Data.BQL.BqlDateTime.Field<firstContactDatePayroll> { }
    #endregion

    #region NotesPayroll
    [PXDBString(510, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Notes")]
    public virtual string NotesPayroll { get; set; }
    public abstract class notesPayroll : PX.Data.BQL.BqlString.Field<notesPayroll> { }
    #endregion

    #region PayrollComplete
    [PXDBBool()]
    [PXUIField(DisplayName = "Complete")]
    public virtual bool? PayrollComplete { get; set; }
    public abstract class payrollComplete : PX.Data.BQL.BqlBool.Field<payrollComplete> { }
    #endregion

    #region PayrollPreparer
    [PXDBInt]
    [PXEPEmployeeSelector]
    [PXUIField(DisplayName = "Payroll Preparer", Visibility = PXUIVisibility.SelectorVisible)]
    public virtual int? PayrollPreparer { get; set; }
    public abstract class payrollPreparer : PX.Data.BQL.BqlInt.Field<payrollPreparer> { }
    #endregion

    #region MPR
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost
            })]
    [PXUIField(DisplayName = "MPR")]
    public virtual string MPR { get; set; }
    public abstract class mpr : PX.Data.BQL.BqlString.Field<mpr> { }
    #endregion

    #region MPRLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? MPRLastModifiedDateTime { get; set; }
    public abstract class mPRLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<mPRLastModifiedDateTime> { }
    #endregion

    #region MPRLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string MPRLastModUserName { get; set; }
    public abstract class mPRLastModUserName : PX.Data.BQL.BqlString.Field<mPRLastModUserName> { }
    #endregion

    #region PayrollVerification
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost
            })]
    [PXUIField(DisplayName = "Payroll Verification")]
    public virtual string PayrollVerification { get; set; }
    public abstract class payrollVerification : PX.Data.BQL.BqlString.Field<payrollVerification> { }
    #endregion

    #region PayrollVerLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PayrollVerLastModifiedDateTime { get; set; }
    public abstract class payrollVerLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<payrollVerLastModifiedDateTime> { }
    #endregion

    #region PayrollVerLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string PayrollVerLastModUserName { get; set; }
    public abstract class payrollVerLastModUserName : PX.Data.BQL.BqlString.Field<payrollVerLastModUserName> { }
    #endregion

    #region RKAccess
    [PXDBString(2)]
    [PXStringList(new string[] { AccessStatus.Requested, AccessStatus.Completed, AccessStatus.NA}, new string[] {MessageAccessStatus.Requested, MessageAccessStatus.Completed, MessageAccessStatus.NA})]
    [PXUIField(DisplayName = "RK Access")]
    public virtual string RKAccess { get; set; }
    public abstract class rKAccess : PX.Data.BQL.BqlString.Field<rKAccess> { }
    #endregion
    
    #region PayrollProvider
    [PXDBString(3)]
    [PXStringList(
            new string[] 
            {
                PayrollProviderList.ADP1,
                PayrollProviderList.ADP2,
                PayrollProviderList.ADP3,
                PayrollProviderList.Alliance,
                PayrollProviderList.CBIZ,
                PayrollProviderList.CertiPay,
                PayrollProviderList.Coastal,
                PayrollProviderList.Dominion,
                PayrollProviderList.ExponentHR,
                PayrollProviderList.GAPartners,
                PayrollProviderList.Heartland,
                PayrollProviderList.ISolved,
                PayrollProviderList.Kelly,
                PayrollProviderList.Netchex,
                PayrollProviderList.Paychex,
                PayrollProviderList.Paycom,
                PayrollProviderList.Paycor,
                PayrollProviderList.Payday,
                PayrollProviderList.Paylocity,
                PayrollProviderList.PayMaster,
                PayrollProviderList.Paytime,
                PayrollProviderList.Proliant,
                PayrollProviderList.Ultimate,
                PayrollProviderList.Viventium,
                PayrollProviderList.OtherPR
            },
            new string[]
            {
                MessagePayrollProviderList.ADP1,
                MessagePayrollProviderList.ADP2,
                MessagePayrollProviderList.ADP3,
                MessagePayrollProviderList.Alliance,
                MessagePayrollProviderList.CBIZ,
                MessagePayrollProviderList.CertiPay,
                MessagePayrollProviderList.Coastal,
                MessagePayrollProviderList.Dominion,
                MessagePayrollProviderList.ExponentHR,
                MessagePayrollProviderList.GAPartners,
                MessagePayrollProviderList.Heartland,
                MessagePayrollProviderList.ISolved,
                MessagePayrollProviderList.Kelly,
                MessagePayrollProviderList.Netchex,
                MessagePayrollProviderList.Paychex,
                MessagePayrollProviderList.Paycom,
                MessagePayrollProviderList.Paycor,
                MessagePayrollProviderList.Payday,
                MessagePayrollProviderList.Paylocity,
                MessagePayrollProviderList.PayMaster,
                MessagePayrollProviderList.Paytime,
                MessagePayrollProviderList.Proliant,
                MessagePayrollProviderList.Ultimate,
                MessagePayrollProviderList.Viventium,
                MessagePayrollProviderList.OtherPR
            })]
    [PXUIField(DisplayName = "Payroll Provider")]
    public virtual string PayrollProvider { get; set; }
    public abstract class payrollProvider : PX.Data.BQL.BqlString.Field<payrollProvider> { }
    #endregion

    #region RecordKeeperName
    [PXDBString(3)]
    [PXStringList(
            new string[] 
            {
                RecordKeeperList.ADP,
                RecordKeeperList.Alerus,
                RecordKeeperList.Ascensus,
                RecordKeeperList.ATRetirement,
                RecordKeeperList.American,
                RecordKeeperList.Betterment,
                RecordKeeperList.BOK,
                RecordKeeperList.CapitalGroup,
                RecordKeeperList.CapitalAmerican,
                RecordKeeperList.CMFG,
                RecordKeeperList.EmployeeFid,
                RecordKeeperList.Empower1,
                RecordKeeperList.Empower2,
                RecordKeeperList.Empower3,
                RecordKeeperList.EPIC,
                RecordKeeperList.Equitable,
                RecordKeeperList.ERISA,
                RecordKeeperList.Fidelity,
                RecordKeeperList.John1,
                RecordKeeperList.John2,
                RecordKeeperList.Lincoln1,
                RecordKeeperList.Lincoln2,
                RecordKeeperList.Leading,
                RecordKeeperList.MidAtlantic,
                RecordKeeperList.MassMutual1,
                RecordKeeperList.MassMutual2,
                RecordKeeperList.Newport,
                RecordKeeperList.Nationwide,
                RecordKeeperList.OneAmerica,
                RecordKeeperList.Paychex,
                RecordKeeperList.Professional,
                RecordKeeperList.Principal,
                RecordKeeperList.Prudential,
                RecordKeeperList.RHIAcquisition,
                RecordKeeperList.Retirement,
                RecordKeeperList.Charles,
                RecordKeeperList.Securian,
                RecordKeeperList.Standard,
                RecordKeeperList.TRowe,
                RecordKeeperList.Transamerica,
                RecordKeeperList.USIConslting,
                RecordKeeperList.Voya,
                RecordKeeperList.OtherRK
    },
            new string[]
            {
                MessageRecordKeeperList.ADP,
                MessageRecordKeeperList.Alerus,
                MessageRecordKeeperList.Ascensus,
                MessageRecordKeeperList.ATRetirement,
                MessageRecordKeeperList.American,
                MessageRecordKeeperList.Betterment,
                MessageRecordKeeperList.BOK,
                MessageRecordKeeperList.CapitalGroup,
                MessageRecordKeeperList.CapitalAmerican,
                MessageRecordKeeperList.CMFG,
                MessageRecordKeeperList.EmployeeFid,
                MessageRecordKeeperList.Empower1,
                MessageRecordKeeperList.Empower2,
                MessageRecordKeeperList.Empower3,
                MessageRecordKeeperList.EPIC,
                MessageRecordKeeperList.Equitable,
                MessageRecordKeeperList.ERISA,
                MessageRecordKeeperList.Fidelity,
                MessageRecordKeeperList.John1,
                MessageRecordKeeperList.John2,
                MessageRecordKeeperList.Lincoln1,
                MessageRecordKeeperList.Lincoln2,
                MessageRecordKeeperList.Leading,
                MessageRecordKeeperList.MidAtlantic,
                MessageRecordKeeperList.MassMutual1,
                MessageRecordKeeperList.MassMutual2,
                MessageRecordKeeperList.Newport,
                MessageRecordKeeperList.Nationwide,
                MessageRecordKeeperList.OneAmerica,
                MessageRecordKeeperList.Paychex,
                MessageRecordKeeperList.Professional,
                MessageRecordKeeperList.Principal,
                MessageRecordKeeperList.Prudential,
                MessageRecordKeeperList.RHIAcquisition,
                MessageRecordKeeperList.Retirement,
                MessageRecordKeeperList.Charles,
                MessageRecordKeeperList.Securian,
                MessageRecordKeeperList.Standard,
                MessageRecordKeeperList.TRowe,
                MessageRecordKeeperList.Transamerica,
                MessageRecordKeeperList.USIConslting,
                MessageRecordKeeperList.Voya,
                MessageRecordKeeperList.OtherRK
            })]
    [PXUIField(DisplayName = "Record Keeper")]
    public virtual string RecordKeeperName { get; set; }
    public abstract class recordKeeperName : PX.Data.BQL.BqlString.Field<recordKeeperName> { }
    #endregion

    //Plan Documents

    #region FirstContactDatePlanDocuments
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDatePlanDocuments { get; set; }
    public abstract class firstContactDatePlanDocuments : PX.Data.BQL.BqlDateTime.Field<firstContactDatePlanDocuments> { }
    #endregion

    #region NotesPlanDocuments
    [PXDBString(510, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Notes")]
    public virtual string NotesPlanDocuments { get; set; }
    public abstract class notesPlanDocuments : PX.Data.BQL.BqlString.Field<notesPlanDocuments> { }
    #endregion

    #region PlanDocumentsComplete
    [PXDBBool()]
    [PXUIField(DisplayName = "Complete")]
    public virtual bool? PlanDocumentsComplete { get; set; }
    public abstract class planDocumentsComplete : PX.Data.BQL.BqlBool.Field<planDocumentsComplete> { }
    #endregion

    #region SPD
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost
            })]
    [PXUIField(DisplayName = "SPD")]
    public virtual string SPD { get; set; }
    public abstract class spd : PX.Data.BQL.BqlString.Field<spd> { }
    #endregion

    #region SPDLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? SPDLastModifiedDateTime { get; set; }
    public abstract class sPDLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<sPDLastModifiedDateTime> { }
    #endregion

    #region SPDLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string SPDLastModUserName { get; set; }
    public abstract class sPDLastModUserName : PX.Data.BQL.BqlString.Field<sPDLastModUserName> { }
    #endregion

    #region AA
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost
            })]
    [PXUIField(DisplayName = "AA + (Cycle 3 Amedment)")]
    public virtual string AA { get; set; }
    public abstract class aA : PX.Data.BQL.BqlString.Field<aA> { }
    #endregion

    #region AALastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? AALastModifiedDateTime { get; set; }
    public abstract class aALastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<aALastModifiedDateTime> { }
    #endregion

    #region AALastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string AALastModUserName { get; set; }
    public abstract class aALastModUserName : PX.Data.BQL.BqlString.Field<aALastModUserName> { }
    #endregion


    #region BPD
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost
            })]
    [PXUIField(DisplayName = "BPD")]
    public virtual string BPD { get; set; }
    public abstract class bpd : PX.Data.BQL.BqlString.Field<bpd> { }
    #endregion

    #region BPDLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? BPDLastModifiedDateTime { get; set; }
    public abstract class bPDLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<bPDLastModifiedDateTime> { }
    #endregion

    #region BPDLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string BPDLastModUserName { get; set; }
    public abstract class bPDLastModUserName : PX.Data.BQL.BqlString.Field<bPDLastModUserName> { }
    #endregion


    #region IRS
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost
            })]
    [PXUIField(DisplayName = "IRS Opinion Letter")]
    public virtual string IRS { get; set; }
    public abstract class irs : PX.Data.BQL.BqlString.Field<irs> { }
    #endregion

    #region IRSLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? IRSLastModifiedDateTime { get; set; }
    public abstract class iRSLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<iRSLastModifiedDateTime> { }
    #endregion

    #region IRSLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string IRSLastModUserName { get; set; }
    public abstract class iRSLastModUserName : PX.Data.BQL.BqlString.Field<iRSLastModUserName> { }
    #endregion


    #region SATrustree
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost
            })]
    [PXUIField(DisplayName = "SA - Trustree/Custodian")]
    public virtual string SATrustree { get; set; }
    public abstract class sATrustree : PX.Data.BQL.BqlString.Field<sATrustree> { }
    #endregion

    #region SATrustreeLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? SATrustreeLastModifiedDateTime { get; set; }
    public abstract class sATrustreeLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<sATrustreeLastModifiedDateTime> { }
    #endregion

    #region SATrustreeLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string SATrustreeLastModUserName { get; set; }
    public abstract class sATrustreeLastModUserName : PX.Data.BQL.BqlString.Field<sATrustreeLastModUserName> { }
    #endregion


    #region SATPA
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost, DocStatus.NA
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost, MessageDocStatus.NA
            })]
    [PXUIField(DisplayName = "SA - TPA")]
    public virtual string SATPA { get; set; }
    public abstract class sATPA : PX.Data.BQL.BqlString.Field<sATPA> { }
    #endregion

    #region SATPALastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? SATPALastModifiedDateTime { get; set; }
    public abstract class sATPALastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<sATPALastModifiedDateTime> { }
    #endregion

    #region SATPALastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string SATPALastModUserName { get; set; }
    public abstract class sATPALastModUserName : PX.Data.BQL.BqlString.Field<sATPALastModUserName> { }
    #endregion

    #region SAInvestment
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost, DocStatus.NA
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost, MessageDocStatus.NA
            })]
    [PXUIField(DisplayName = "SA - Investment Advisor")]
    public virtual string SAInvestment { get; set; }
    public abstract class sAInvestment : PX.Data.BQL.BqlString.Field<sAInvestment> { }
    #endregion

    #region SAInvestmentLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? SAInvestmentLastModifiedDateTime { get; set; }
    public abstract class sAInvestmentLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<sAInvestmentLastModifiedDateTime> { }
    #endregion

    #region SAInvestmentLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string SAInvestmentLastModUserName { get; set; }
    public abstract class sAInvestmentLastModUserName : PX.Data.BQL.BqlString.Field<sAInvestmentLastModUserName> { }
    #endregion

    #region LoanPolicy
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost, DocStatus.NA
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost, MessageDocStatus.NA
            })]
    [PXUIField(DisplayName = "Loan Policy")]
    public virtual string LoanPolicy { get; set; }
    public abstract class loanPolicy : PX.Data.BQL.BqlString.Field<loanPolicy> { }
    #endregion

    #region LoanPolicyLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? LoanPolicyLastModifiedDateTime { get; set; }
    public abstract class loanPolicyLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<loanPolicyLastModifiedDateTime> { }
    #endregion

    #region LoanPolicyLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string LoanPolicyLastModUserName { get; set; }
    public abstract class loanPolicyLastModUserName : PX.Data.BQL.BqlString.Field<loanPolicyLastModUserName> { }
    #endregion

    #region QDIA
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost, DocStatus.NA
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost, MessageDocStatus.NA
            })]
    [PXUIField(DisplayName = "QDIA")]
    public virtual string QDIA { get; set; }
    public abstract class qDIA : PX.Data.BQL.BqlString.Field<qDIA> { }
    #endregion

    #region QDIALastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? QDIALastModifiedDateTime { get; set; }
    public abstract class qDIALastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<qDIALastModifiedDateTime> { }
    #endregion

    #region QDIALastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string QDIALastModUserName { get; set; }
    public abstract class qDIALastModUserName : PX.Data.BQL.BqlString.Field<qDIALastModUserName> { }
    #endregion


    //PlanDoc Preprocessing

    #region FirstContactDatePlanDocPreproc
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDatePlanDocPreproc { get; set; }
    public abstract class firstContactDatePlanDocPreproc : PX.Data.BQL.BqlDateTime.Field<firstContactDatePlanDocPreproc> { }
    #endregion

    #region NotesPlanDocPreproc
    [PXDBString(510, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Notes")]
    public virtual string NotesPlanDocPreproc { get; set; }
    public abstract class notesPlanDocPreproc : PX.Data.BQL.BqlString.Field<notesPlanDocPreproc> { }
    #endregion

    #region PlanDocPreprocComplete
    [PXDBBool()]
    [PXUIField(DisplayName = "Complete")]
    public virtual bool? PlanDocPreprocComplete { get; set; }
    public abstract class planDocPreprocComplete : PX.Data.BQL.BqlBool.Field<planDocPreprocComplete> { }
    #endregion

    #region EC
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost
            })]
    [PXUIField(DisplayName = "EC")]
    public virtual string EC { get; set; }
    public abstract class eC : PX.Data.BQL.BqlString.Field<eC> { }
    #endregion

    #region ECLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? ECLastModifiedDateTime { get; set; }
    public abstract class eCLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<eCLastModifiedDateTime> { }
    #endregion

    #region ECLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string ECLastModUserName { get; set; }
    public abstract class eCLastModUserName : PX.Data.BQL.BqlString.Field<eCLastModUserName> { }
    #endregion


    #region PAS
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost
            })]
    [PXUIField(DisplayName = "PAS")]
    public virtual string PAS { get; set; }
    public abstract class pAS : PX.Data.BQL.BqlString.Field<pAS> { }
    #endregion

    #region PASLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PASLastModifiedDateTime { get; set; }
    public abstract class pASLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<pASLastModifiedDateTime> { }
    #endregion

    #region PASLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string PASLastModUserName { get; set; }
    public abstract class pASLastModUserName : PX.Data.BQL.BqlString.Field<pASLastModUserName> { }
    #endregion


    #region Cont
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost
            })]
    [PXUIField(DisplayName = "Cont")]
    public virtual string Cont { get; set; }
    public abstract class cont : PX.Data.BQL.BqlString.Field<cont> { }
    #endregion

    #region ContLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? ContLastModifiedDateTime { get; set; }
    public abstract class contLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<contLastModifiedDateTime> { }
    #endregion

    #region ContLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string ContLastModUserName { get; set; }
    public abstract class contLastModUserName : PX.Data.BQL.BqlString.Field<contLastModUserName> { }
    #endregion


    #region Dist
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost
            })]
    [PXUIField(DisplayName = "Dist")]
    public virtual string Dist { get; set; }
    public abstract class dist : PX.Data.BQL.BqlString.Field<dist> { }
    #endregion

    #region DistLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? DistLastModifiedDateTime { get; set; }
    public abstract class distLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<distLastModifiedDateTime> { }
    #endregion

    #region DistLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string DistLastModUserName { get; set; }
    public abstract class distLastModUserName : PX.Data.BQL.BqlString.Field<distLastModUserName> { }
    #endregion


    #region Loans
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost, DocStatus.NA
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost, MessageDocStatus.NA
            })]
    [PXUIField(DisplayName = "Loans")]
    public virtual string Loans { get; set; }
    public abstract class loans : PX.Data.BQL.BqlString.Field<loans> { }
    #endregion

    #region LoansLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? LoansLastModifiedDateTime { get; set; }
    public abstract class loansLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<loansLastModifiedDateTime> { }
    #endregion

    #region LoansLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string LoansLastModUserName { get; set; }
    public abstract class loansLastModUserName : PX.Data.BQL.BqlString.Field<loansLastModUserName> { }
    #endregion


    #region DefElection
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost, DocStatus.NA
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost, MessageDocStatus.NA
            })]
    [PXUIField(DisplayName = "Def Elections and Changes")]
    public virtual string DefElection { get; set; }
    public abstract class defElection : PX.Data.BQL.BqlString.Field<defElection> { }
    #endregion

    #region DefElectionLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? DefElectionLastModifiedDateTime { get; set; }
    public abstract class defElectionLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<defElectionLastModifiedDateTime> { }
    #endregion

    #region DefElectionLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string DefElectionLastModUserName { get; set; }
    public abstract class defElectionLastModUserName : PX.Data.BQL.BqlString.Field<defElectionLastModUserName> { }
    #endregion


    //Plan Design

    #region FirstContactDatePlanDesign
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDatePlanDesign { get; set; }
    public abstract class firstContactDatePlanDesign : PX.Data.BQL.BqlDateTime.Field<firstContactDatePlanDesign> { }
    #endregion

    #region NotesPlanDesign
    [PXDBString(510, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Notes")]
    public virtual string NotesPlanDesign { get; set; }
    public abstract class notesPlanDesign : PX.Data.BQL.BqlString.Field<notesPlanDesign> { }
    #endregion

    #region PlanDesignComplete
    [PXDBBool()]
    [PXUIField(DisplayName = "Complete")]
    public virtual bool? PlanDesignComplete { get; set; }
    public abstract class planDesignComplete : PX.Data.BQL.BqlBool.Field<planDesignComplete> { }
    #endregion

    #region PlanProvisions
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                PlanDesignStatus.WaitingonInfo, PlanDesignStatus.InProcess, PlanDesignStatus.Completed
            }, 
            new string[] 
            {
                MessagePlanDesignStatus.WaitingonInfo, MessagePlanDesignStatus.InProcess, MessagePlanDesignStatus.Completed
            })]
    public virtual string PlanProvisions { get; set; }
    public abstract class planProvisions : PX.Data.BQL.BqlString.Field<planProvisions> { }
    #endregion

    #region PlanProvisionsLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PlanProvisionsLastModifiedDateTime { get; set; }
    public abstract class planProvisionsLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<planProvisionsLastModifiedDateTime> { }
    #endregion

    #region PlanProvisionsLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string PlanProvisionsLastModUserName { get; set; }
    public abstract class planProvisionsLastModUserName : PX.Data.BQL.BqlString.Field<planProvisionsLastModUserName> { }
    #endregion

    #region PreProcessDate
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                PlanDesignStatus.WaitingonInfo, PlanDesignStatus.InProcess, PlanDesignStatus.Completed
            }, 
            new string[] 
            {
                MessagePlanDesignStatus.WaitingonInfo, MessagePlanDesignStatus.InProcess, MessagePlanDesignStatus.Completed
            })]
    [PXUIField(DisplayName = "Submitted for PreProcess Date")]
    public virtual string PreProcessDate { get; set; }
    public abstract class preProcessDate : PX.Data.BQL.BqlString.Field<preProcessDate> { }
    #endregion

    #region PreProcessDateLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PreProcessDateLastModifiedDateTime { get; set; }
    public abstract class preProcessDateLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<preProcessDateLastModifiedDateTime> { }
    #endregion

    #region PreProcessDateLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string PreProcessDateLastModUserName { get; set; }
    public abstract class preProcessDateLastModUserName : PX.Data.BQL.BqlString.Field<preProcessDateLastModUserName> { }
    #endregion

    //Annual Documents

    #region FirstContactDateAnnualDocuments
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDateAnnualDocuments { get; set; }
    public abstract class firstContactDateAnnualDocuments : PX.Data.BQL.BqlDateTime.Field<firstContactDateAnnualDocuments> { }
    #endregion

    #region NotesAnnualDocuments
    [PXDBString(510, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Notes")]
    public virtual string NotesAnnualDocuments { get; set; }
    public abstract class notesAnnualDocuments : PX.Data.BQL.BqlString.Field<notesAnnualDocuments> { }
    #endregion

    #region AnnualDocumentsComplete
    [PXDBBool()]
    [PXUIField(DisplayName = "Complete")]
    public virtual bool? AnnualDocumentsComplete { get; set; }
    public abstract class annualDocumentsComplete : PX.Data.BQL.BqlBool.Field<annualDocumentsComplete> { }
    #endregion

    #region Form5500
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost
            })]
    [PXUIField(DisplayName = "5500")]
    public virtual string Form5500 { get; set; }
    public abstract class form5500 : PX.Data.BQL.BqlString.Field<form5500> { }
    #endregion

    #region Form5500LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? Form5500LastModifiedDateTime { get; set; }
    public abstract class form5500LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<form5500LastModifiedDateTime> { }
    #endregion

    #region Form5500LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string Form5500LastModUserName { get; set; }
    public abstract class form5500LastModUserName : PX.Data.BQL.BqlString.Field<form5500LastModUserName> { }
    #endregion


    #region TrustState
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost
            })]
    [PXUIField(DisplayName = "Trust Statement")]
    public virtual string TrustState { get; set; }
    public abstract class trustState : PX.Data.BQL.BqlString.Field<trustState> { }
    #endregion

    #region TrustStateLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? TrustStateLastModifiedDateTime { get; set; }
    public abstract class trustStateLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<trustStateLastModifiedDateTime> { }
    #endregion

    #region TrustStateLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string TrustStateLastModUserName { get; set; }
    public abstract class trustStateLastModUserName : PX.Data.BQL.BqlString.Field<trustStateLastModUserName> { }
    #endregion


    #region TrustCert
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost
            })]
    [PXUIField(DisplayName = "Trust Certification")]
    public virtual string TrustCert { get; set; }
    public abstract class trustCert : PX.Data.BQL.BqlString.Field<trustCert> { }
    #endregion

    #region TrustCertLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? TrustCertLastModifiedDateTime { get; set; }
    public abstract class trustCertLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<trustCertLastModifiedDateTime> { }
    #endregion

    #region TrustCertLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string TrustCertLastModUserName { get; set; }
    public abstract class trustCertLastModUserName : PX.Data.BQL.BqlString.Field<trustCertLastModUserName> { }
    #endregion


    #region FidelityBond
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost
            })]
    [PXUIField(DisplayName = "Fidelity Bond")]
    public virtual string FidelityBond { get; set; }
    public abstract class fidelityBond : PX.Data.BQL.BqlString.Field<fidelityBond> { }
    #endregion

    #region FidelityBondLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? FidelityBondLastModifiedDateTime { get; set; }
    public abstract class fidelityBondLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<fidelityBondLastModifiedDateTime> { }
    #endregion

    #region FidelityBondLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string FidelityBondLastModUserName { get; set; }
    public abstract class fidelityBondLastModUserName : PX.Data.BQL.BqlString.Field<fidelityBondLastModUserName> { }
    #endregion


    #region ComplianceTest
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost
            })]
    [PXUIField(DisplayName = "Compliance Testing")]
    public virtual string ComplianceTest { get; set; }
    public abstract class complianceTest : PX.Data.BQL.BqlString.Field<complianceTest> { }
    #endregion

    #region ComplianceTestLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? ComplianceTestLastModifiedDateTime { get; set; }
    public abstract class complianceTestLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<complianceTestLastModifiedDateTime> { }
    #endregion

    #region ComplianceTestLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string ComplianceTestLastModUserName { get; set; }
    public abstract class complianceTestLastModUserName : PX.Data.BQL.BqlString.Field<complianceTestLastModUserName> { }
    #endregion


    #region RKFeeDisc
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost
            })]
    [PXUIField(DisplayName = "RK Fee Disc")]
    public virtual string RKFeeDisc { get; set; }
    public abstract class rKFeeDisc : PX.Data.BQL.BqlString.Field<rKFeeDisc> { }
    #endregion

    #region RKFeeDiscLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? RKFeeDiscLastModifiedDateTime { get; set; }
    public abstract class rKFeeDiscLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<rKFeeDiscLastModifiedDateTime> { }
    #endregion

    #region RKFeeDiscLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string RKFeeDiscLastModUserName { get; set; }
    public abstract class rKFeeDiscLastModUserName : PX.Data.BQL.BqlString.Field<rKFeeDiscLastModUserName> { }
    #endregion


    #region InvestAllocation
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost, DocStatus.NA
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost, MessageDocStatus.NA
            })]
    [PXUIField(DisplayName = "Investment Allocation Report")]
    public virtual string InvestAllocation { get; set; }
    public abstract class investAllocation : PX.Data.BQL.BqlString.Field<investAllocation> { }
    #endregion

    #region InvestAllocationLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? InvestAllocationLastModifiedDateTime { get; set; }
    public abstract class investAllocationLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<investAllocationLastModifiedDateTime> { }
    #endregion

    #region InvestAllocationLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string InvestAllocationLastModUserName { get; set; }
    public abstract class investAllocationLastModUserName : PX.Data.BQL.BqlString.Field<investAllocationLastModUserName> { }
    #endregion


    //First Year PK

    #region FirstContactDateFirstYearPK
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDateFirstYearPK { get; set; }
    public abstract class firstContactDateFirstYearPK : PX.Data.BQL.BqlDateTime.Field<firstContactDateFirstYearPK> { }
    #endregion

    #region NotesFirstYearPK
    [PXDBString(510, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Notes")]
    public virtual string NotesFirstYearPK { get; set; }
    public abstract class notesFirstYearPK : PX.Data.BQL.BqlString.Field<notesFirstYearPK> { }
    #endregion

    #region FirstYearPKComplete
    [PXDBBool()]
    [PXUIField(DisplayName = "Complete")]
    public virtual bool? FirstYearPKComplete { get; set; }
    public abstract class firstYearPKComplete : PX.Data.BQL.BqlBool.Field<firstYearPKComplete> { }
    #endregion

    #region ClientAuth
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost, DocStatus.NA
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost, MessageDocStatus.NA
            })]
    [PXUIField(DisplayName = "Client Authorization Letter")]
    public virtual string ClientAuth { get; set; }
    public abstract class clientAuth : PX.Data.BQL.BqlString.Field<clientAuth> { }
    #endregion

    #region ClientAuthLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? ClientAuthLastModifiedDateTime { get; set; }
    public abstract class clientAuthLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<clientAuthLastModifiedDateTime> { }
    #endregion

    #region ClientAuthLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string ClientAuthLastModUserName { get; set; }
    public abstract class clientAuthLastModUserName : PX.Data.BQL.BqlString.Field<clientAuthLastModUserName> { }
    #endregion


    #region Communication
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost, DocStatus.NA
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost, MessageDocStatus.NA
            })]
    [PXUIField(DisplayName = "Communication with Predecessor Auditor")]
    public virtual string Communication { get; set; }
    public abstract class communication : PX.Data.BQL.BqlString.Field<communication> { }
    #endregion

    #region CommunicationLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? CommunicationLastModifiedDateTime { get; set; }
    public abstract class communicationLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<communicationLastModifiedDateTime> { }
    #endregion

    #region CommunicationLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string CommunicationLastModUserName { get; set; }
    public abstract class communicationLastModUserName : PX.Data.BQL.BqlString.Field<communicationLastModUserName> { }
    #endregion


    #region PY5500
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost, DocStatus.NA
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost, MessageDocStatus.NA
            })]
    [PXUIField(DisplayName = "PY 5500")]
    public virtual string PY5500 { get; set; }
    public abstract class pY5500 : PX.Data.BQL.BqlString.Field<pY5500> { }
    #endregion

    #region PY5500LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PY5500LastModifiedDateTime { get; set; }
    public abstract class pY5500LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<pY5500LastModifiedDateTime> { }
    #endregion

    #region PY5500LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string PY5500LastModUserName { get; set; }
    public abstract class pY5500LastModUserName : PX.Data.BQL.BqlString.Field<pY5500LastModUserName> { }
    #endregion


    #region PYTrustState
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost, DocStatus.NA
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost, MessageDocStatus.NA
            })]
    [PXUIField(DisplayName = "PY Trust Statement")]
    public virtual string PYTrustState { get; set; }
    public abstract class pYTrustState : PX.Data.BQL.BqlString.Field<pYTrustState> { }
    #endregion

    #region PYTrustStateLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PYTrustStateLastModifiedDateTime { get; set; }
    public abstract class pYTrustStateLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<pYTrustStateLastModifiedDateTime> { }
    #endregion

    #region PYTrustStateLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string PYTrustStateLastModUserName { get; set; }
    public abstract class pYTrustStateLastModUserName : PX.Data.BQL.BqlString.Field<pYTrustStateLastModUserName> { }
    #endregion


    #region PYTrustCert
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost, DocStatus.NA
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost, MessageDocStatus.NA
            })]
    [PXUIField(DisplayName = "PY Trust Certification")]
    public virtual string PYTrustCert { get; set; }
    public abstract class pYTrustCert : PX.Data.BQL.BqlString.Field<pYTrustCert> { }
    #endregion

    #region PYTrustCertLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PYTrustCertLastModifiedDateTime { get; set; }
    public abstract class pYTrustCertLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<pYTrustCertLastModifiedDateTime> { }
    #endregion

    #region PYTrustCertLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string PYTrustCertLastModUserName { get; set; }
    public abstract class pYTrustCertLastModUserName : PX.Data.BQL.BqlString.Field<pYTrustCertLastModUserName> { }
    #endregion


    #region PYCompliance
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                DocStatus.Contacted1, DocStatus.Contacted2, DocStatus.ClientCalled, DocStatus.WaitingonInfo, DocStatus.InProcess, DocStatus.Completed, DocStatus.Ghost, DocStatus.NA
            }, 
            new string[] 
            {
                MessageDocStatus.Contacted1, MessageDocStatus.Contacted2, MessageDocStatus.ClientCalled, MessageDocStatus.WaitingonInfo, MessageDocStatus.InProcess, MessageDocStatus.Completed, MessageDocStatus.Ghost, MessageDocStatus.NA
            })]
    [PXUIField(DisplayName = "PY Compliance Testing")]
    public virtual string PYCompliance { get; set; }
    public abstract class pYCompliance : PX.Data.BQL.BqlString.Field<pYCompliance> { }
    #endregion

    #region PYComplianceLastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PYComplianceLastModifiedDateTime { get; set; }
    public abstract class pYComplianceLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<pYComplianceLastModifiedDateTime> { }
    #endregion

    #region PYComplianceLastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string PYComplianceLastModUserName { get; set; }
    public abstract class pYComplianceLastModUserName : PX.Data.BQL.BqlString.Field<pYComplianceLastModUserName> { }
    #endregion


    #region PlanDocPreparer
    [PXDBInt]
    [PXEPEmployeeSelector]
    [PXUIField(DisplayName = "PlanDoc Preparer", Visibility = PXUIVisibility.SelectorVisible)]
    public virtual int? PlanDocPreparer { get; set; }
    public abstract class planDocPreparer : PX.Data.BQL.BqlInt.Field<planDocPreparer> { }
    #endregion

    #region PlanDesignPreparer
    [PXDBInt]
    [PXEPEmployeeSelector]
    [PXUIField(DisplayName = "PlanDesign Preparer", Visibility = PXUIVisibility.SelectorVisible)]
    public virtual int? PlanDesignPreparer { get; set; }
    public abstract class planDesignPreparer : PX.Data.BQL.BqlInt.Field<planDesignPreparer> { }
    #endregion

    #region CreatedDateTime
    [PXDBCreatedDateTime()]
    public virtual DateTime? CreatedDateTime { get; set; }
    public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
    #endregion

    #region CreatedByID
    [PXDBCreatedByID()]
    public virtual Guid? CreatedByID { get; set; }
    public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
    #endregion

    #region CreatedByScreenID
    [PXDBCreatedByScreenID()]
    public virtual string CreatedByScreenID { get; set; }
    public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
    #endregion

    #region LastModifiedDateTime
    [PXDBLastModifiedDateTime()]
    public virtual DateTime? LastModifiedDateTime { get; set; }
    public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
    #endregion

    #region LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? LastModifiedByID { get; set; }
    public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
    #endregion

    #region LastModifiedByScreenID
    [PXDBLastModifiedByScreenID()]
    public virtual string LastModifiedByScreenID { get; set; }
    public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
    #endregion

    #region Tstamp
    [PXDBTimestamp()]
    [PXUIField(DisplayName = "Tstamp")]
    public virtual byte[] Tstamp { get; set; }
    public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
    #endregion

    #region Noteid
    [PXNote()]
    public virtual Guid? Noteid { get; set; }
    public abstract class noteid : PX.Data.BQL.BqlGuid.Field<noteid> { }
    #endregion
  }
}