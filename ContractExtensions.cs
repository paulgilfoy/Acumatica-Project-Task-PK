using PX.Data.EP;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data.WorkflowAPI;
using PX.Data;
using PX.Objects.AR;
using PX.Objects.CM;
using PX.Objects.Common.Discount;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.CT;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.PM;
using PX.Objects;
using PX.TM;
using System.Collections.Generic;
using System.Collections;
using System;

namespace PX.Objects.CT
{
  public class ContractExt : PXCacheExtension<PX.Objects.CT.Contract>
  {
    #region Usr401kStatus
    [PXDBInt]
    [PXIntList(new int[] {01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12, 13, 14, 15, 16, 17, 18}, new string[] {"Setup/Engagement Letter/Invoice", "Plan Doc Gather and Plan Design", "Payroll Doc Gather", "Data Preprocessing and Validation", "Initial RECs and DIs", "EIC Inquiries", "Risk Assessment and Sample Selection", "Sample Testing", "Remaining Audit Steps", "Deficiency Resolution", "In Initial Review", "Preparation of Final Deliverables", "Technical Review", "Waiting on Management Rep Letter", "Final Bill", "Complete", "Waiting on 5500", "Compliance Testing"})]
    [PXUIField(DisplayName="401k Status")]
    public virtual int? Usr401kStatus { get; set; }
    public abstract class usr401kStatus : PX.Data.BQL.BqlInt.Field<usr401kStatus> { }
    #endregion

    #region UsrPGDepartment
    [PXDBString(10)]
    [PXSelector(typeof(PX.Objects.EP.EPDepartment.departmentID))]
    [PXUIField(DisplayName="Department")]
    public virtual string UsrPGDepartment { get; set; }
    public abstract class usrPGDepartment : PX.Data.BQL.BqlString.Field<usrPGDepartment> { }
    #endregion

    #region UsrPGPriority
    [PXDBInt]
    [PXIntList(new int[] {01, 02, 03}, new string[] {"Low", "Medium", "High"})]
    [PXUIField(DisplayName="Project Priority")]
    public virtual int? UsrPGPriority { get; set; }
    public abstract class usrPGPriority : PX.Data.BQL.BqlInt.Field<usrPGPriority> { }
    #endregion

    #region UsrPGTaxStatus
    [PXDBInt]
    [PXIntList(new int[] {01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12, 13, 14}, new string[] {"Ready to Assign", "Information Requested (from Client)", "Information Requested (In-House)", "Ready to Prep", "In Preparation", "In Initial Review", "Clearing Points", "In Final Review", "Technical Review", "Ready to Deliver", "Bill Client", "Invoice", "Complete", "Waiting on 8879 Form"})]
    [PXUIField(DisplayName="Tax Status")]
    public virtual int? UsrPGTaxStatus { get; set; }
    public abstract class usrPGTaxStatus : PX.Data.BQL.BqlInt.Field<usrPGTaxStatus> { }
    #endregion

    #region UsrPGIsFinal
    [PXDBBool]
    [PXUIField(DisplayName="Final")]
    public virtual bool? UsrPGIsFinal { get; set; }
    public abstract class usrPGIsFinal : PX.Data.BQL.BqlBool.Field<usrPGIsFinal> { }
    #endregion

    #region UsrPGAcctStatus
    [PXDBInt]
    [PXIntList(new int[] {01, 02, 03, 04, 05, 06, 07, 08}, new string[] {"In Preparation", "Initial Review (Acctg)", "Clearing Points", "In Final Review", "Bill Client", "Invoice", "Complete", "Meet with Client"})]
    [PXUIField(DisplayName="Acct Status")]
    public virtual int? UsrPGAcctStatus { get; set; }
    public abstract class usrPGAcctStatus : PX.Data.BQL.BqlInt.Field<usrPGAcctStatus> { }
    #endregion

    #region UsrPGAuditStatus
    [PXDBInt]
    [PXIntList(new int[] {01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11}, new string[] {"Ready to Prep", "In Preparation", "Fieldwork in Process", "In Initial Review", "Clearing Points", "In Final Review", "Financial Statements", "Technical Review", "Ready to Deliver", "Bill Client", "Complete"})]
    [PXUIField(DisplayName="Audit Status")]
    public virtual int? UsrPGAuditStatus { get; set; }
    public abstract class usrPGAuditStatus : PX.Data.BQL.BqlInt.Field<usrPGAuditStatus> { }
    #endregion

    #region UsrPGTechStatus
    [PXDBInt]
    [PXIntList(new int[] {01, 02, 03, 04}, new string[] {"Tech Status 1", "Tech Status 2", "Tech Status 3", "Tech Status 4"})]
    [PXUIField(DisplayName="Tech Status")]
    public virtual int? UsrPGTechStatus { get; set; }
    public abstract class usrPGTechStatus : PX.Data.BQL.BqlInt.Field<usrPGTechStatus> { }
    #endregion

    #region UsrPGReleasedELF
    [PXDBBool]
    [PXUIField(DisplayName="Released in ELF")]

    public virtual bool? UsrPGReleasedELF { get; set; }
    public abstract class usrPGReleasedELF : PX.Data.BQL.BqlBool.Field<usrPGReleasedELF> { }
    #endregion
  }
}