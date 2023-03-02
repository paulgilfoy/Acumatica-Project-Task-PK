using System;
using System.Collections;
using PX.Data;
using PX.Objects.PM;
using PX.Data.BQL.Fluent;

namespace ProjectTask
{
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

    #region Additional_Notes
    [PXDBString(50, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Additional Notes")]
    public virtual string Additional_Notes { get; set; }
    public abstract class additional_Notes : PX.Data.BQL.BqlString.Field<additional_Notes> { }
    #endregion

    #region Active
    [PXDBBool()]
    [PXDefault(true)]
    [PXUIField(DisplayName = "Active")]
    public virtual bool? Active { get; set; }
    public abstract class active : PX.Data.BQL.BqlBool.Field<active> { }
    #endregion

    #region FirstContactDate_Payroll
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDate_Payroll { get; set; }
    public abstract class firstContactDate_Payroll : PX.Data.BQL.BqlDateTime.Field<firstContactDate_Payroll> { }
    #endregion

    #region MPR
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "MPR")]
    public virtual int? MPR { get; set; }
    public abstract class mpr : PX.Data.BQL.BqlInt.Field<mpr> { }
    #endregion

    #region MPR_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? MPR_LastModifiedDateTime { get; set; }
    public abstract class mPR_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<mPR_LastModifiedDateTime> { }
    #endregion

    #region MPR_LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? MPR_LastModifiedByID { get; set; }
    public abstract class mPR_LastModifiedByID : PX.Data.BQL.BqlGuid.Field<mPR_LastModifiedByID> { }
    #endregion

    #region PayrollVerification
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Payroll Verification")]
    public virtual int? PayrollVerification { get; set; }
    public abstract class payrollVerification : PX.Data.BQL.BqlInt.Field<payrollVerification> { }
    #endregion

    #region PayrollVerification_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PayrollVer_LastModifiedDateTime { get; set; }
    public abstract class payrollVer_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<payrollVer_LastModifiedDateTime> { }
    #endregion

    #region PayrollVer_LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? PayrollVer_LastModifiedByID { get; set; }
    public abstract class payrollVer_LastModifiedByID : PX.Data.BQL.BqlGuid.Field<payrollVer_LastModifiedByID> { }
    #endregion


    #region FirstContactDate_PlanDocuments
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDate_PlanDocuments { get; set; }
    public abstract class firstContactDate_PlanDocuments : PX.Data.BQL.BqlDateTime.Field<firstContactDate_PlanDocuments> { }
    #endregion

    #region SPD
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "SPD")]
    public virtual int? SPD { get; set; }
    public abstract class spd : PX.Data.BQL.BqlInt.Field<spd> { }
    #endregion

    #region SPD_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? SPD_LastModifiedDateTime { get; set; }
    public abstract class sPD_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<sPD_LastModifiedDateTime> { }
    #endregion

    #region SPD_LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? SPD_LastModifiedByID { get; set; }
    public abstract class sPD_LastModifiedByID : PX.Data.BQL.BqlGuid.Field<sPD_LastModifiedByID> { }
    #endregion


    #region AA
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "AA + (Cycle 3 Amedment)")]
    public virtual int? Aa { get; set; }
    public abstract class aa : PX.Data.BQL.BqlInt.Field<aa> { }
    #endregion

    #region AA_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? AA_LastModifiedDateTime { get; set; }
    public abstract class aA_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<aA_LastModifiedDateTime> { }
    #endregion

    #region AA_LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? AA_LastModifiedByID { get; set; }
    public abstract class aA_LastModifiedByID : PX.Data.BQL.BqlGuid.Field<aA_LastModifiedByID> { }
    #endregion


    #region BPD
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "BPD")]
    public virtual int? BPD { get; set; }
    public abstract class bpd : PX.Data.BQL.BqlInt.Field<bpd> { }
    #endregion

    #region BPD_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? BPD_LastModifiedDateTime { get; set; }
    public abstract class bPD_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<bPD_LastModifiedDateTime> { }
    #endregion

    #region BPD_LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? BPD_LastModifiedByID { get; set; }
    public abstract class bPD_LastModifiedByID : PX.Data.BQL.BqlGuid.Field<bPD_LastModifiedByID> { }
    #endregion


    #region IRS
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "IRS Opinion Letter")]
    public virtual int? IRS { get; set; }
    public abstract class irs : PX.Data.BQL.BqlInt.Field<irs> { }
    #endregion

    #region IRS_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? IRS_LastModifiedDateTime { get; set; }
    public abstract class iRS_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<iRS_LastModifiedDateTime> { }
    #endregion

    #region IRS_LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? IRS_LastModifiedByID { get; set; }
    public abstract class iRS_LastModifiedByID : PX.Data.BQL.BqlGuid.Field<iRS_LastModifiedByID> { }
    #endregion


    #region SATrustree
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "SA - Trustree/Custodian")]
    public virtual int? SATrustree { get; set; }
    public abstract class sATrustree : PX.Data.BQL.BqlInt.Field<sATrustree> { }
    #endregion

    #region SATrustree_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? SATrustree_LastModifiedDateTime { get; set; }
    public abstract class sATrustree_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<sATrustree_LastModifiedDateTime> { }
    #endregion

    #region SATrustree_LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? SATrustree_LastModifiedByID { get; set; }
    public abstract class sATrustree_LastModifiedByID : PX.Data.BQL.BqlGuid.Field<sATrustree_LastModifiedByID> { }
    #endregion


    #region SATPA
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "SA - TPA")]
    public virtual int? SATPA { get; set; }
    public abstract class sATPA : PX.Data.BQL.BqlInt.Field<sATPA> { }
    #endregion

    #region SATPA_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? SATPA_LastModifiedDateTime { get; set; }
    public abstract class sATPA_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<sATPA_LastModifiedDateTime> { }
    #endregion

    #region SATPA_LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? SATPA_LastModifiedByID { get; set; }
    public abstract class sATPA_LastModifiedByID : PX.Data.BQL.BqlGuid.Field<sATPA_LastModifiedByID> { }
    #endregion


    #region SAInvestment
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "SA - Investment Advisor")]
    public virtual int? SAInvestment { get; set; }
    public abstract class sAInvestment : PX.Data.BQL.BqlInt.Field<sAInvestment> { }
    #endregion

    #region SAInvestment_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? SAInvestment_LastModifiedDateTime { get; set; }
    public abstract class sAInvestment_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<sAInvestment_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region LoanPolicy
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Loan Policy")]
    public virtual int? LoanPolicy { get; set; }
    public abstract class loanPolicy : PX.Data.BQL.BqlInt.Field<loanPolicy> { }
    #endregion

    #region LoanPolicy_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? LoanPolicy_LastModifiedDateTime { get; set; }
    public abstract class loanPolicy_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<loanPolicy_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region Qdia
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "QDIA")]
    public virtual int? Qdia { get; set; }
    public abstract class qdia : PX.Data.BQL.BqlInt.Field<qdia> { }
    #endregion

    #region QDIA_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? QDIA_LastModifiedDateTime { get; set; }
    public abstract class qDIA_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<qDIA_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region FirstContactDate_PlanDocPreproc
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDate_PlanDocPreproc { get; set; }
    public abstract class firstContactDate_PlanDocPreproc : PX.Data.BQL.BqlDateTime.Field<firstContactDate_PlanDocPreproc> { }
    #endregion

    #region Ec
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "EC")]
    public virtual int? Ec { get; set; }
    public abstract class ec : PX.Data.BQL.BqlInt.Field<ec> { }
    #endregion

    #region EC_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? EC_LastModifiedDateTime { get; set; }
    public abstract class eC_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<eC_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region Pas
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "PAS")]
    public virtual int? Pas { get; set; }
    public abstract class pas : PX.Data.BQL.BqlInt.Field<pas> { }
    #endregion

    #region PAS_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PAS_LastModifiedDateTime { get; set; }
    public abstract class pAS_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<pAS_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region Cont
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Cont")]
    public virtual int? Cont { get; set; }
    public abstract class cont : PX.Data.BQL.BqlInt.Field<cont> { }
    #endregion

    #region Cont_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? Cont_LastModifiedDateTime { get; set; }
    public abstract class cont_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<cont_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region Dist
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Dist")]
    public virtual int? Dist { get; set; }
    public abstract class dist : PX.Data.BQL.BqlInt.Field<dist> { }
    #endregion

    #region Dist_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? Dist_LastModifiedDateTime { get; set; }
    public abstract class dist_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<dist_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region Loans
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Loans")]
    public virtual int? Loans { get; set; }
    public abstract class loans : PX.Data.BQL.BqlInt.Field<loans> { }
    #endregion

    #region Loans_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? Loans_LastModifiedDateTime { get; set; }
    public abstract class loans_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<loans_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region DefElection
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Def Elections and Changes")]
    public virtual int? DefElection { get; set; }
    public abstract class defElection : PX.Data.BQL.BqlInt.Field<defElection> { }
    #endregion

    #region DefElection_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? DefElection_LastModifiedDateTime { get; set; }
    public abstract class defElection_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<defElection_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region FirstContactDate_AnnualDocuments
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDate_AnnualDocuments { get; set; }
    public abstract class firstContactDate_AnnualDocuments : PX.Data.BQL.BqlDateTime.Field<firstContactDate_AnnualDocuments> { }
    #endregion

    #region Form_5500
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "5500")]
    public virtual int? Form_5500 { get; set; }
    public abstract class form_5500 : PX.Data.BQL.BqlInt.Field<form_5500> { }
    #endregion

    #region Form_5500_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? Form_5500_LastModifiedDateTime { get; set; }
    public abstract class form_5500_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<form_5500_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region TrustState
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Trust Statement")]
    public virtual int? TrustState { get; set; }
    public abstract class trustState : PX.Data.BQL.BqlInt.Field<trustState> { }
    #endregion

    #region TrustState_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? TrustState_LastModifiedDateTime { get; set; }
    public abstract class trustState_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<trustState_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region TrustCert
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Trust Certification")]
    public virtual int? TrustCert { get; set; }
    public abstract class trustCert : PX.Data.BQL.BqlInt.Field<trustCert> { }
    #endregion

    #region TrustCert_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? TrustCert_LastModifiedDateTime { get; set; }
    public abstract class trustCert_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<trustCert_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region FidelityBond
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Fidelity Bond")]
    public virtual int? FidelityBond { get; set; }
    public abstract class fidelityBond : PX.Data.BQL.BqlInt.Field<fidelityBond> { }
    #endregion

    #region FidelityBond_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? FidelityBond_LastModifiedDateTime { get; set; }
    public abstract class fidelityBond_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<fidelityBond_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region ComplianceTest
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Compliance Testing")]
    public virtual int? ComplianceTest { get; set; }
    public abstract class complianceTest : PX.Data.BQL.BqlInt.Field<complianceTest> { }
    #endregion

    #region ComplianceTest_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? ComplianceTest_LastModifiedDateTime { get; set; }
    public abstract class complianceTest_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<complianceTest_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region RKFeeDisc
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "RK Fee Disc")]
    public virtual int? RKFeeDisc { get; set; }
    public abstract class rKFeeDisc : PX.Data.BQL.BqlInt.Field<rKFeeDisc> { }
    #endregion

    #region RKFeeDisc_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? RKFeeDisc_LastModifiedDateTime { get; set; }
    public abstract class rKFeeDisc_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<rKFeeDisc_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region InvestAllocation
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Investment Allocation Report")]
    public virtual int? InvestAllocation { get; set; }
    public abstract class investAllocation : PX.Data.BQL.BqlInt.Field<investAllocation> { }
    #endregion

    #region InvestAllocation_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? InvestAllocation_LastModifiedDateTime { get; set; }
    public abstract class investAllocation_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<investAllocation_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region FirstContactDate_FirstYearPK
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDate_FirstYearPK { get; set; }
    public abstract class firstContactDate_FirstYearPK : PX.Data.BQL.BqlDateTime.Field<firstContactDate_FirstYearPK> { }
    #endregion

    #region ClientAuth
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Client Authorization Letter")]
    public virtual int? ClientAuth { get; set; }
    public abstract class clientAuth : PX.Data.BQL.BqlInt.Field<clientAuth> { }
    #endregion

    #region ClientAuth_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? ClientAuth_LastModifiedDateTime { get; set; }
    public abstract class clientAuth_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<clientAuth_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region Communication
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Communication with Predecessor Auditor")]
    public virtual int? Communication { get; set; }
    public abstract class communication : PX.Data.BQL.BqlInt.Field<communication> { }
    #endregion

    #region Communication_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? Communication_LastModifiedDateTime { get; set; }
    public abstract class communication_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<communication_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region Py5500
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "PY 5500")]
    public virtual int? Py5500 { get; set; }
    public abstract class py5500 : PX.Data.BQL.BqlInt.Field<py5500> { }
    #endregion

    #region PY5500_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PY5500_LastModifiedDateTime { get; set; }
    public abstract class pY5500_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<pY5500_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region PYTrustState
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "PY Trust Statement")]
    public virtual int? PYTrustState { get; set; }
    public abstract class pYTrustState : PX.Data.BQL.BqlInt.Field<pYTrustState> { }
    #endregion

    #region PYTrustState_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PYTrustState_LastModifiedDateTime { get; set; }
    public abstract class pYTrustState_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<pYTrustState_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region PYTrustCert
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "PY Trust Certification")]
    public virtual int? PYTrustCert { get; set; }
    public abstract class pYTrustCert : PX.Data.BQL.BqlInt.Field<pYTrustCert> { }
    #endregion

    #region PYTrustCert_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PYTrustCert_LastModifiedDateTime { get; set; }
    public abstract class pYTrustCert_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<pYTrustCert_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region PYCompliance
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "PY Compliance Testing")]
    public virtual int? PYCompliance { get; set; }
    public abstract class pYCompliance : PX.Data.BQL.BqlInt.Field<pYCompliance> { }
    #endregion

    #region PYCompliance_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PYCompliance_LastModifiedDateTime { get; set; }
    public abstract class pYCompliance_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<pYCompliance_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region FirstContactDate_PlanDesign
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDate_PlanDesign { get; set; }
    public abstract class firstContactDate_PlanDesign : PX.Data.BQL.BqlDateTime.Field<firstContactDate_PlanDesign> { }
    #endregion

    #region PlanProvisions
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Relevant Plan Provisions")]
    public virtual int? PlanProvisions { get; set; }
    public abstract class planProvisions : PX.Data.BQL.BqlInt.Field<planProvisions> { }
    #endregion

    #region PlanProvisions_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PlanProvisions_LastModifiedDateTime { get; set; }
    public abstract class planProvisions_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<planProvisions_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
    #endregion


    #region PreProcessDate
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Submitted for PreProcess Date")]
    public virtual int? PreProcessDate { get; set; }
    public abstract class preProcessDate : PX.Data.BQL.BqlInt.Field<preProcessDate> { }
    #endregion

    #region PreProcessDate_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PreProcessDate_LastModifiedDateTime { get; set; }
    public abstract class preProcessDate_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<preProcessDate_LastModifiedDateTime> { }
    #endregion

    #region _LastModifiedByID
    [PXDBLastModifiedByID()]
    public virtual Guid? _LastModifiedByID { get; set; }
    public abstract class _LastModifiedByID : PX.Data.BQL.BqlGuid.Field<_LastModifiedByID> { }
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