using System;
using System.Collections;
using PX.Data;
using PX.Objects.PM;
using PX.Data.BQL.Fluent;
using PX.Objects.CT;

namespace ProjectTask
{
  [Serializable]
  [PXCacheName("PMDocumentGathering")]
  public class PMDocumentGathering : IBqlTable
  {
    #region ProjectID
    public abstract class projectID : PX.Data.BQL.BqlInt.Field<projectID> { }

        private class Department : Constant<string>
        {
            public Department() : base("401K") { }
        }

		/// copy from PX.Objects.PM.PMTask
		protected Int32? _ProjectID;
		[Project(DisplayName = "Project ID", IsKey = true)]
		[PXParent(typeof(Select<PMProject, Where<PMProject.contractID, Equal<Current<PMDocumentGathering.projectID>>>>))]
		[PXDBDefault(typeof(PMProject.contractID))]

        // [PXSelector(
        //     typeof(Search<PMProject.contractID,
        //         Where<PMProject.usrPGDepartment, Equal<Department>>>),
        //     DescriptionField = typeof(PMProject.contractCD))]

        // [PXSelector(
        //     typeof(Search2<PMProject.contractID,
        //         InnerJoin<Contract, On<Contract.contractID,Equal<PMProject.contractID>>>.GetExtension<PX.Objects.CT.ContractExt>(),
        //             Where<Contract.usrPGDepartment, Equal<Department>>>>),
        //     typeof(PMproject.contractCD),
        //     DescriptionField = typeof(PMProject.description))]


        // [PXSelector(typeof(Search5<POOrder.orderNbr,
        //     InnerJoin<POReceiptLine, On<POReceiptLine.pOType, Equal<POOrder.orderType>,
        //         And<POReceiptLine.pONbr, Equal<POOrder.orderNbr>,
        //         And<POReceiptLine.receiptNbr, Equal<Current<POReceipt.receiptNbr>>>>>>,
        //         Aggregate<GroupBy<POOrder.orderType,
        //         GroupBy<POOrder.orderNbr>>>>))]
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
    [PXDBString(255, IsUnicode = true, InputMask = "")]
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

    #region Notes_Payroll
    [PXDBString(510, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Notes")]
    public virtual string Notes_Payroll { get; set; }
    public abstract class notes_Payroll : PX.Data.BQL.BqlString.Field<notes_Payroll> { }
    #endregion

    #region Payroll_Complete
    [PXDBBool()]
    [PXUIField(DisplayName = "Complete")]
    public virtual bool? Payroll_Complete { get; set; }
    public abstract class payroll_Complete : PX.Data.BQL.BqlBool.Field<payroll_Complete> { }
    #endregion

    #region MPR
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "MPR")]
    public virtual string MPR { get; set; }
    public abstract class mpr : PX.Data.BQL.BqlString.Field<mpr> { }
    #endregion

    #region MPR_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? MPR_LastModifiedDateTime { get; set; }
    public abstract class mPR_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<mPR_LastModifiedDateTime> { }
    #endregion

    #region MPR_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string MPR_LastModUserName { get; set; }
    public abstract class mPR_LastModUserName : PX.Data.BQL.BqlString.Field<mPR_LastModUserName> { }
    #endregion

    #region PayrollVerification
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Payroll Verification")]
    public virtual string PayrollVerification { get; set; }
    public abstract class payrollVerification : PX.Data.BQL.BqlString.Field<payrollVerification> { }
    #endregion

    #region PayrollVer_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PayrollVer_LastModifiedDateTime { get; set; }
    public abstract class payrollVer_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<payrollVer_LastModifiedDateTime> { }
    #endregion

    #region PayrollVer_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string PayrollVer_LastModUserName { get; set; }
    public abstract class payrollVer_LastModUserName : PX.Data.BQL.BqlString.Field<payrollVer_LastModUserName> { }
    #endregion


    #region FirstContactDate_PlanDocuments
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDate_PlanDocuments { get; set; }
    public abstract class firstContactDate_PlanDocuments : PX.Data.BQL.BqlDateTime.Field<firstContactDate_PlanDocuments> { }
    #endregion

    #region Notes_PlanDocuments
    [PXDBString(510, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Notes")]
    public virtual string Notes_PlanDocuments { get; set; }
    public abstract class notes_PlanDocuments : PX.Data.BQL.BqlString.Field<notes_PlanDocuments> { }
    #endregion

    #region PlanDocuments_Complete
    [PXDBBool()]
    [PXUIField(DisplayName = "Complete")]
    public virtual bool? PlanDocuments_Complete { get; set; }
    public abstract class planDocuments_Complete : PX.Data.BQL.BqlBool.Field<planDocuments_Complete> { }
    #endregion

    #region SPD
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "SPD")]
    public virtual string SPD { get; set; }
    public abstract class spd : PX.Data.BQL.BqlString.Field<spd> { }
    #endregion

    #region SPD_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? SPD_LastModifiedDateTime { get; set; }
    public abstract class sPD_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<sPD_LastModifiedDateTime> { }
    #endregion

    #region SPD_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string SPD_LastModUserName { get; set; }
    public abstract class sPD_LastModUserName : PX.Data.BQL.BqlString.Field<sPD_LastModUserName> { }
    #endregion

    #region AA
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "AA + (Cycle 3 Amedment)")]
    public virtual string AA { get; set; }
    public abstract class aA : PX.Data.BQL.BqlString.Field<aA> { }
    #endregion

    #region AA_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? AA_LastModifiedDateTime { get; set; }
    public abstract class aA_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<aA_LastModifiedDateTime> { }
    #endregion

    #region AA_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string AA_LastModUserName { get; set; }
    public abstract class aA_LastModUserName : PX.Data.BQL.BqlString.Field<aA_LastModUserName> { }
    #endregion


    #region BPD
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "BPD")]
    public virtual string BPD { get; set; }
    public abstract class bpd : PX.Data.BQL.BqlString.Field<bpd> { }
    #endregion

    #region BPD_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? BPD_LastModifiedDateTime { get; set; }
    public abstract class bPD_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<bPD_LastModifiedDateTime> { }
    #endregion

    #region BPD_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string BPD_LastModUserName { get; set; }
    public abstract class bPD_LastModUserName : PX.Data.BQL.BqlString.Field<bPD_LastModUserName> { }
    #endregion


    #region IRS
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "IRS Opinion Letter")]
    public virtual string IRS { get; set; }
    public abstract class irs : PX.Data.BQL.BqlString.Field<irs> { }
    #endregion

    #region IRS_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? IRS_LastModifiedDateTime { get; set; }
    public abstract class iRS_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<iRS_LastModifiedDateTime> { }
    #endregion

    #region IRS_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string IRS_LastModUserName { get; set; }
    public abstract class iRS_LastModUserName : PX.Data.BQL.BqlString.Field<iRS_LastModUserName> { }
    #endregion


    #region SATrustree
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "SA - Trustree/Custodian")]
    public virtual string SATrustree { get; set; }
    public abstract class sATrustree : PX.Data.BQL.BqlString.Field<sATrustree> { }
    #endregion

    #region SATrustree_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? SATrustree_LastModifiedDateTime { get; set; }
    public abstract class sATrustree_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<sATrustree_LastModifiedDateTime> { }
    #endregion

    #region SATrustree_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string SATrustree_LastModUserName { get; set; }
    public abstract class sATrustree_LastModUserName : PX.Data.BQL.BqlString.Field<sATrustree_LastModUserName> { }
    #endregion


    #region SATPA
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH", "NA"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost", "n/a"})]
    [PXUIField(DisplayName = "SA - TPA")]
    public virtual string SATPA { get; set; }
    public abstract class sATPA : PX.Data.BQL.BqlString.Field<sATPA> { }
    #endregion

    #region SATPA_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? SATPA_LastModifiedDateTime { get; set; }
    public abstract class sATPA_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<sATPA_LastModifiedDateTime> { }
    #endregion

    #region SATPA_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string SATPA_LastModUserName { get; set; }
    public abstract class sATPA_LastModUserName : PX.Data.BQL.BqlString.Field<sATPA_LastModUserName> { }
    #endregion

    #region SAInvestment
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH", "NA"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost", "n/a"})]
    [PXUIField(DisplayName = "SA - Investment Advisor")]
    public virtual string SAInvestment { get; set; }
    public abstract class sAInvestment : PX.Data.BQL.BqlString.Field<sAInvestment> { }
    #endregion

    #region SAInvestment_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? SAInvestment_LastModifiedDateTime { get; set; }
    public abstract class sAInvestment_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<sAInvestment_LastModifiedDateTime> { }
    #endregion

    #region SAInvestment_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string SAInvestment_LastModUserName { get; set; }
    public abstract class sAInvestment_LastModUserName : PX.Data.BQL.BqlString.Field<sAInvestment_LastModUserName> { }
    #endregion

    #region LoanPolicy
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH", "NA"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost", "n/a"})]
    [PXUIField(DisplayName = "Loan Policy")]
    public virtual string LoanPolicy { get; set; }
    public abstract class loanPolicy : PX.Data.BQL.BqlString.Field<loanPolicy> { }
    #endregion

    #region LoanPolicy_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? LoanPolicy_LastModifiedDateTime { get; set; }
    public abstract class loanPolicy_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<loanPolicy_LastModifiedDateTime> { }
    #endregion

    #region LoanPolicy_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string LoanPolicy_LastModUserName { get; set; }
    public abstract class loanPolicy_LastModUserName : PX.Data.BQL.BqlString.Field<loanPolicy_LastModUserName> { }
    #endregion

    #region QDIA
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH", "NA"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost", "n/a"})]
    [PXUIField(DisplayName = "QDIA")]
    public virtual string QDIA { get; set; }
    public abstract class qDIA : PX.Data.BQL.BqlString.Field<qDIA> { }
    #endregion

    #region QDIA_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? QDIA_LastModifiedDateTime { get; set; }
    public abstract class qDIA_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<qDIA_LastModifiedDateTime> { }
    #endregion

    #region QDIA_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string QDIA_LastModUserName { get; set; }
    public abstract class qDIA_LastModUserName : PX.Data.BQL.BqlString.Field<qDIA_LastModUserName> { }
    #endregion

    #region FirstContactDate_PlanDocPreproc
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDate_PlanDocPreproc { get; set; }
    public abstract class firstContactDate_PlanDocPreproc : PX.Data.BQL.BqlDateTime.Field<firstContactDate_PlanDocPreproc> { }
    #endregion

    #region Notes_PlanDocPreproc
    [PXDBString(510, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Notes")]
    public virtual string Notes_PlanDocPreproc { get; set; }
    public abstract class notes_PlanDocPreproc : PX.Data.BQL.BqlString.Field<notes_PlanDocPreproc> { }
    #endregion

    #region PlanDocPreproc_Complete
    [PXDBBool()]
    [PXUIField(DisplayName = "Complete")]
    public virtual bool? PlanDocPreproc_Complete { get; set; }
    public abstract class planDocPreproc_Complete : PX.Data.BQL.BqlBool.Field<planDocPreproc_Complete> { }
    #endregion

    #region EC
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "EC")]
    public virtual string EC { get; set; }
    public abstract class eC : PX.Data.BQL.BqlString.Field<eC> { }
    #endregion

    #region EC_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? EC_LastModifiedDateTime { get; set; }
    public abstract class eC_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<eC_LastModifiedDateTime> { }
    #endregion

    #region EC_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string EC_LastModUserName { get; set; }
    public abstract class eC_LastModUserName : PX.Data.BQL.BqlString.Field<eC_LastModUserName> { }
    #endregion


    #region PAS
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "PAS")]
    public virtual string PAS { get; set; }
    public abstract class pAS : PX.Data.BQL.BqlString.Field<pAS> { }
    #endregion

    #region PAS_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PAS_LastModifiedDateTime { get; set; }
    public abstract class pAS_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<pAS_LastModifiedDateTime> { }
    #endregion

    #region PAS_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string PAS_LastModUserName { get; set; }
    public abstract class pAS_LastModUserName : PX.Data.BQL.BqlString.Field<pAS_LastModUserName> { }
    #endregion


    #region Cont
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Cont")]
    public virtual string Cont { get; set; }
    public abstract class cont : PX.Data.BQL.BqlString.Field<cont> { }
    #endregion

    #region Cont_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? Cont_LastModifiedDateTime { get; set; }
    public abstract class cont_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<cont_LastModifiedDateTime> { }
    #endregion

    #region Cont_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string Cont_LastModUserName { get; set; }
    public abstract class cont_LastModUserName : PX.Data.BQL.BqlString.Field<cont_LastModUserName> { }
    #endregion


    #region Dist
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Dist")]
    public virtual string Dist { get; set; }
    public abstract class dist : PX.Data.BQL.BqlString.Field<dist> { }
    #endregion

    #region Dist_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? Dist_LastModifiedDateTime { get; set; }
    public abstract class dist_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<dist_LastModifiedDateTime> { }
    #endregion

    #region Dist_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string Dist_LastModUserName { get; set; }
    public abstract class dist_LastModUserName : PX.Data.BQL.BqlString.Field<dist_LastModUserName> { }
    #endregion


    #region Loans
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH", "NA"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost", "n/a"})]
    [PXUIField(DisplayName = "Loans")]
    public virtual string Loans { get; set; }
    public abstract class loans : PX.Data.BQL.BqlString.Field<loans> { }
    #endregion

    #region Loans_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? Loans_LastModifiedDateTime { get; set; }
    public abstract class loans_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<loans_LastModifiedDateTime> { }
    #endregion

    #region Loans_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string Loans_LastModUserName { get; set; }
    public abstract class loans_LastModUserName : PX.Data.BQL.BqlString.Field<loans_LastModUserName> { }
    #endregion


    #region DefElection
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Def Elections and Changes")]
    public virtual string DefElection { get; set; }
    public abstract class defElection : PX.Data.BQL.BqlString.Field<defElection> { }
    #endregion

    #region DefElection_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? DefElection_LastModifiedDateTime { get; set; }
    public abstract class defElection_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<defElection_LastModifiedDateTime> { }
    #endregion

    #region DefElection_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string DefElection_LastModUserName { get; set; }
    public abstract class defElection_LastModUserName : PX.Data.BQL.BqlString.Field<defElection_LastModUserName> { }
    #endregion

    #region FirstContactDate_PlanDesign
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDate_PlanDesign { get; set; }
    public abstract class firstContactDate_PlanDesign : PX.Data.BQL.BqlDateTime.Field<firstContactDate_PlanDesign> { }
    #endregion

    #region Notes_PlanDesign
    [PXDBString(510, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Notes")]
    public virtual string Notes_PlanDesign { get; set; }
    public abstract class notes_PlanDesign : PX.Data.BQL.BqlString.Field<notes_PlanDesign> { }
    #endregion

    #region PlanDesign_Complete
    [PXDBBool()]
    [PXUIField(DisplayName = "Complete")]
    public virtual bool? PlanDesign_Complete { get; set; }
    public abstract class planDesign_Complete : PX.Data.BQL.BqlBool.Field<planDesign_Complete> { }
    #endregion

    #region PlanProvisions
    [PXDBString(2)]
    [PXStringList(new string[] {"WI", "IP", "CM"}, new string[] {"Waiting on Info", "In Process", "Completed"})]
    [PXUIField(DisplayName = "Relevant Plan Provisions")]
    public virtual string PlanProvisions { get; set; }
    public abstract class planProvisions : PX.Data.BQL.BqlString.Field<planProvisions> { }
    #endregion

    #region PlanProvisions_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PlanProvisions_LastModifiedDateTime { get; set; }
    public abstract class planProvisions_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<planProvisions_LastModifiedDateTime> { }
    #endregion

    #region PlanProvisions_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string PlanProvisions_LastModUserName { get; set; }
    public abstract class planProvisions_LastModUserName : PX.Data.BQL.BqlString.Field<planProvisions_LastModUserName> { }
    #endregion

    #region PreProcessDate
    [PXDBString(2)]
    [PXStringList(new string[] {"WI", "IP", "CM"}, new string[] {"Waiting on Info", "In Process", "Completed"})]
    [PXUIField(DisplayName = "Submitted for PreProcess Date")]
    public virtual string PreProcessDate { get; set; }
    public abstract class preProcessDate : PX.Data.BQL.BqlString.Field<preProcessDate> { }
    #endregion

    #region PreProcessDate_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PreProcessDate_LastModifiedDateTime { get; set; }
    public abstract class preProcessDate_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<preProcessDate_LastModifiedDateTime> { }
    #endregion

    #region PreProcessDate_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string PreProcessDate_LastModUserName { get; set; }
    public abstract class preProcessDate_LastModUserName : PX.Data.BQL.BqlString.Field<preProcessDate_LastModUserName> { }
    #endregion

    #region FirstContactDate_AnnualDocuments
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDate_AnnualDocuments { get; set; }
    public abstract class firstContactDate_AnnualDocuments : PX.Data.BQL.BqlDateTime.Field<firstContactDate_AnnualDocuments> { }
    #endregion

    #region Notes_AnnualDocuments
    [PXDBString(510, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Notes")]
    public virtual string Notes_AnnualDocuments { get; set; }
    public abstract class notes_AnnualDocuments : PX.Data.BQL.BqlString.Field<notes_AnnualDocuments> { }
    #endregion

    #region AnnualDocuments_Complete
    [PXDBBool()]
    [PXUIField(DisplayName = "Complete")]
    public virtual bool? AnnualDocuments_Complete { get; set; }
    public abstract class annualDocuments_Complete : PX.Data.BQL.BqlBool.Field<annualDocuments_Complete> { }
    #endregion

    #region Form_5500
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "5500")]
    public virtual string Form_5500 { get; set; }
    public abstract class form_5500 : PX.Data.BQL.BqlString.Field<form_5500> { }
    #endregion

    #region Form_5500_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? Form_5500_LastModifiedDateTime { get; set; }
    public abstract class form_5500_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<form_5500_LastModifiedDateTime> { }
    #endregion

    #region Form_5500_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string Form_5500_LastModUserName { get; set; }
    public abstract class form_5500_LastModUserName : PX.Data.BQL.BqlString.Field<form_5500_LastModUserName> { }
    #endregion


    #region TrustState
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Trust Statement")]
    public virtual string TrustState { get; set; }
    public abstract class trustState : PX.Data.BQL.BqlString.Field<trustState> { }
    #endregion

    #region TrustState_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? TrustState_LastModifiedDateTime { get; set; }
    public abstract class trustState_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<trustState_LastModifiedDateTime> { }
    #endregion

    #region TrustState_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string TrustState_LastModUserName { get; set; }
    public abstract class trustState_LastModUserName : PX.Data.BQL.BqlString.Field<trustState_LastModUserName> { }
    #endregion


    #region TrustCert
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Trust Certification")]
    public virtual string TrustCert { get; set; }
    public abstract class trustCert : PX.Data.BQL.BqlString.Field<trustCert> { }
    #endregion

    #region TrustCert_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? TrustCert_LastModifiedDateTime { get; set; }
    public abstract class trustCert_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<trustCert_LastModifiedDateTime> { }
    #endregion

    #region TrustCert_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string TrustCert_LastModUserName { get; set; }
    public abstract class trustCert_LastModUserName : PX.Data.BQL.BqlString.Field<trustCert_LastModUserName> { }
    #endregion


    #region FidelityBond
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Fidelity Bond")]
    public virtual string FidelityBond { get; set; }
    public abstract class fidelityBond : PX.Data.BQL.BqlString.Field<fidelityBond> { }
    #endregion

    #region FidelityBond_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? FidelityBond_LastModifiedDateTime { get; set; }
    public abstract class fidelityBond_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<fidelityBond_LastModifiedDateTime> { }
    #endregion

    #region FidelityBond_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string FidelityBond_LastModUserName { get; set; }
    public abstract class fidelityBond_LastModUserName : PX.Data.BQL.BqlString.Field<fidelityBond_LastModUserName> { }
    #endregion


    #region ComplianceTest
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "Compliance Testing")]
    public virtual string ComplianceTest { get; set; }
    public abstract class complianceTest : PX.Data.BQL.BqlString.Field<complianceTest> { }
    #endregion

    #region ComplianceTest_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? ComplianceTest_LastModifiedDateTime { get; set; }
    public abstract class complianceTest_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<complianceTest_LastModifiedDateTime> { }
    #endregion

    #region ComplianceTest_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string ComplianceTest_LastModUserName { get; set; }
    public abstract class complianceTest_LastModUserName : PX.Data.BQL.BqlString.Field<complianceTest_LastModUserName> { }
    #endregion


    #region RKFeeDisc
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost"})]
    [PXUIField(DisplayName = "RK Fee Disc")]
    public virtual string RKFeeDisc { get; set; }
    public abstract class rKFeeDisc : PX.Data.BQL.BqlString.Field<rKFeeDisc> { }
    #endregion

    #region RKFeeDisc_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? RKFeeDisc_LastModifiedDateTime { get; set; }
    public abstract class rKFeeDisc_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<rKFeeDisc_LastModifiedDateTime> { }
    #endregion

    #region RKFeeDisc_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string RKFeeDisc_LastModUserName { get; set; }
    public abstract class rKFeeDisc_LastModUserName : PX.Data.BQL.BqlString.Field<rKFeeDisc_LastModUserName> { }
    #endregion


    #region InvestAllocation
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH", "NA"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost", "n/a"})]
    [PXUIField(DisplayName = "Investment Allocation Report")]
    public virtual string InvestAllocation { get; set; }
    public abstract class investAllocation : PX.Data.BQL.BqlString.Field<investAllocation> { }
    #endregion

    #region InvestAllocation_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? InvestAllocation_LastModifiedDateTime { get; set; }
    public abstract class investAllocation_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<investAllocation_LastModifiedDateTime> { }
    #endregion

    #region InvestAllocation_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string InvestAllocation_LastModUserName { get; set; }
    public abstract class investAllocation_LastModUserName : PX.Data.BQL.BqlString.Field<investAllocation_LastModUserName> { }
    #endregion


    #region FirstContactDate_FirstYearPK
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDate_FirstYearPK { get; set; }
    public abstract class firstContactDate_FirstYearPK : PX.Data.BQL.BqlDateTime.Field<firstContactDate_FirstYearPK> { }
    #endregion

    #region Notes_FirstYearPK
    [PXDBString(510, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Notes")]
    public virtual string Notes_FirstYearPK { get; set; }
    public abstract class notes_FirstYearPK : PX.Data.BQL.BqlString.Field<notes_FirstYearPK> { }
    #endregion

    #region FirstYearPK_Complete
    [PXDBBool()]
    [PXUIField(DisplayName = "Complete")]
    public virtual bool? FirstYearPK_Complete { get; set; }
    public abstract class firstYearPK_Complete : PX.Data.BQL.BqlBool.Field<firstYearPK_Complete> { }
    #endregion

    #region ClientAuth
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH", "NA"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost", "n/a"})]
    [PXUIField(DisplayName = "Client Authorization Letter")]
    public virtual string ClientAuth { get; set; }
    public abstract class clientAuth : PX.Data.BQL.BqlString.Field<clientAuth> { }
    #endregion

    #region ClientAuth_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? ClientAuth_LastModifiedDateTime { get; set; }
    public abstract class clientAuth_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<clientAuth_LastModifiedDateTime> { }
    #endregion

    #region ClientAuth_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string ClientAuth_LastModUserName { get; set; }
    public abstract class clientAuth_LastModUserName : PX.Data.BQL.BqlString.Field<clientAuth_LastModUserName> { }
    #endregion


    #region Communication
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH", "NA"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost", "n/a"})]
    [PXUIField(DisplayName = "Communication with Predecessor Auditor")]
    public virtual string Communication { get; set; }
    public abstract class communication : PX.Data.BQL.BqlString.Field<communication> { }
    #endregion

    #region Communication_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? Communication_LastModifiedDateTime { get; set; }
    public abstract class communication_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<communication_LastModifiedDateTime> { }
    #endregion

    #region Communication_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string Communication_LastModUserName { get; set; }
    public abstract class communication_LastModUserName : PX.Data.BQL.BqlString.Field<communication_LastModUserName> { }
    #endregion


    #region PY5500
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH", "NA"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost", "n/a"})]
    [PXUIField(DisplayName = "PY 5500")]
    public virtual string PY5500 { get; set; }
    public abstract class pY5500 : PX.Data.BQL.BqlString.Field<pY5500> { }
    #endregion

    #region PY5500_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PY5500_LastModifiedDateTime { get; set; }
    public abstract class pY5500_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<pY5500_LastModifiedDateTime> { }
    #endregion

    #region PY5500_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string PY5500_LastModUserName { get; set; }
    public abstract class pY5500_LastModUserName : PX.Data.BQL.BqlString.Field<pY5500_LastModUserName> { }
    #endregion


    #region PYTrustState
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH", "NA"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost", "n/a"})]
    [PXUIField(DisplayName = "PY Trust Statement")]
    public virtual string PYTrustState { get; set; }
    public abstract class pYTrustState : PX.Data.BQL.BqlString.Field<pYTrustState> { }
    #endregion

    #region PYTrustState_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PYTrustState_LastModifiedDateTime { get; set; }
    public abstract class pYTrustState_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<pYTrustState_LastModifiedDateTime> { }
    #endregion

    #region PYTrustState_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string PYTrustState_LastModUserName { get; set; }
    public abstract class pYTrustState_LastModUserName : PX.Data.BQL.BqlString.Field<pYTrustState_LastModUserName> { }
    #endregion


    #region PYTrustCert
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH", "NA"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost", "n/a"})]
    [PXUIField(DisplayName = "PY Trust Certification")]
    public virtual string PYTrustCert { get; set; }
    public abstract class pYTrustCert : PX.Data.BQL.BqlString.Field<pYTrustCert> { }
    #endregion

    #region PYTrustCert_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PYTrustCert_LastModifiedDateTime { get; set; }
    public abstract class pYTrustCert_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<pYTrustCert_LastModifiedDateTime> { }
    #endregion

    #region PYTrustCert_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string PYTrustCert_LastModUserName { get; set; }
    public abstract class pYTrustCert_LastModUserName : PX.Data.BQL.BqlString.Field<pYTrustCert_LastModUserName> { }
    #endregion


    #region PYCompliance
    [PXDBString(2)]
    [PXStringList(new string[] {"C1", "C2", "CC", "WI", "IP", "CM", "GH", "NA"}, new string[] {"Client Contacted 1", "Client Contacted 2", "Client Called", "Waiting on Info", "In Process", "Completed", "Ghost", "n/a"})]
    [PXUIField(DisplayName = "PY Compliance Testing")]
    public virtual string PYCompliance { get; set; }
    public abstract class pYCompliance : PX.Data.BQL.BqlString.Field<pYCompliance> { }
    #endregion

    #region PYCompliance_LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? PYCompliance_LastModifiedDateTime { get; set; }
    public abstract class pYCompliance_LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<pYCompliance_LastModifiedDateTime> { }
    #endregion

    #region PYCompliance_LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string PYCompliance_LastModUserName { get; set; }
    public abstract class pYCompliance_LastModUserName : PX.Data.BQL.BqlString.Field<pYCompliance_LastModUserName> { }
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