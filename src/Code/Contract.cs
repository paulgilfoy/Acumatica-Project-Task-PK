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
using ProjectTask;   // PKProjectStatusListAttribute (T-21/T-22)

namespace PX.Objects.CT
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class ContractExt : PXCacheExtension<PX.Objects.CT.Contract>
  {
    #region Usr401kStatus
    [PXDBInt]
    [PKProjectStatusList("401K")]   // list maintained on PKPM2001 (table PKProjectStatus); was a fixed PXIntList 1-20 until T-22
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

    #region UsrPGComplexity
    [PXDBInt]
    [PXIntList(new int[] { 01, 02, 03}, new string[] { "Level 1", "Level 2", "Level 3" })]
    [PXUIField(DisplayName = "Project Complexity")]
    public virtual int? UsrPGComplexity { get; set; }
    public abstract class usrPGComplexity : PX.Data.BQL.BqlInt.Field<usrPGComplexity> { }
    #endregion

    #region UsrPGTaxStatus
    [PXDBInt]
    [PKProjectStatusList("TAX")]    // list maintained on PKPM2001; was a fixed PXIntList 1-15 until T-22
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
    [PKProjectStatusList("ACCT")]   // list maintained on PKPM2001; was a fixed PXIntList 1-8 until T-22
    [PXUIField(DisplayName="Acct Status")]
    public virtual int? UsrPGAcctStatus { get; set; }
    public abstract class usrPGAcctStatus : PX.Data.BQL.BqlInt.Field<usrPGAcctStatus> { }
    #endregion

    #region UsrPGAuditStatus
    [PXDBInt]
    [PKProjectStatusList("AUDIT")]  // list maintained on PKPM2001; was a fixed PXIntList 1-11 until T-22
    [PXUIField(DisplayName="Audit Status")]
    public virtual int? UsrPGAuditStatus { get; set; }
    public abstract class usrPGAuditStatus : PX.Data.BQL.BqlInt.Field<usrPGAuditStatus> { }
    #endregion

    #region UsrPGReleasedELF
    [PXDBBool]
    [PXUIField(DisplayName="Released in ELF")]

    public virtual bool? UsrPGReleasedELF { get; set; }
    public abstract class usrPGReleasedELF : PX.Data.BQL.BqlBool.Field<usrPGReleasedELF> { }
        #endregion

    #region UsrPGReleaseDeliverables
    [PXDBBool]
    [PXUIField(DisplayName = "Release of Final Deliverables")]

    public virtual bool? UsrPGReleaseDeliverables { get; set; }
    public abstract class usrPGReleaseDeliverables : PX.Data.BQL.BqlBool.Field<usrPGReleaseDeliverables> { }
        #endregion


    #region UsrPGFinishDate
    [PXDBDate()]
    [PXUIField(DisplayName = "Finish Date")]

    public virtual DateTime? UsrPGFinishDate { get; set; }
    public abstract class usrPGFinishDate : PX.Data.BQL.BqlDateTime.Field<usrPGFinishDate> { }
        #endregion

    #region UsrPGYearEnd
    [PXDBDate()]
    [PXUIField(DisplayName = "Year End")]

    public virtual DateTime? UsrPGYearEnd { get; set; }
    public abstract class usrPGYearEnd : PX.Data.BQL.BqlDateTime.Field<usrPGYearEnd> { }
    #endregion
    }
}