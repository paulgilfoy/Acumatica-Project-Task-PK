using PX.Data.BQL;
using PX.Data.EP;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data;
using PX.Objects.AR;
using PX.Objects.CM;
using PX.Objects.CN.ProjectAccounting.Descriptor;
using PX.Objects.CN.ProjectAccounting.PM.Descriptor;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.PM;
using PX.Objects.TX;
using PX.Objects;
using PX.TM;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace PX.Objects.PM
{
  public class PMTaskExt : PXCacheExtension<PX.Objects.PM.PMTask>
  {
    #region UsrPGTaxStatus
    [PXDBInt]
    [PXIntList(new int[] {01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12, 13, 14}, new string[] {"Ready to Assign", "Information Requested (from Client)", "Information Requested (In-House)", "Ready to Prep", "In Preparation", "In Initial Review", "Clearing Points", "In Final Review", "Technical Review", "Ready to Deliver", "Bill Client", "Invoice", "Complete", "Waiting on 8879 Form"})]
    [PXUIField(DisplayName="Tax Status")]
    public virtual int? UsrPGTaxStatus { get; set; }
    public abstract class usrPGTaxStatus : PX.Data.BQL.BqlInt.Field<usrPGTaxStatus> { }
    #endregion

    #region UsrPGDueDate
    [PXDBDate]
    [PXUIField(DisplayName="Due Date")]

    public virtual DateTime? UsrPGDueDate { get; set; }
    public abstract class usrPGDueDate : PX.Data.BQL.BqlDateTime.Field<usrPGDueDate> { }
    #endregion

    #region UsrPGExtendedDueDate
    [PXDBDate]
    [PXUIField(DisplayName="Extended Due Date")]

    public virtual DateTime? UsrPGExtendedDueDate { get; set; }
    public abstract class usrPGExtendedDueDate : PX.Data.BQL.BqlDateTime.Field<usrPGExtendedDueDate> { }
    #endregion

    #region UsrPGIsExtended
    [PXDBBool]
    [PXUIField(DisplayName="Extended")]

    public virtual bool? UsrPGIsExtended { get; set; }
    public abstract class usrPGIsExtended : PX.Data.BQL.BqlBool.Field<usrPGIsExtended> { }
    #endregion

    #region UsrPGPreparer
    [PXDBInt]
    [PXEPEmployeeSelector]
    [PXUIField(DisplayName = "Preparer", Visibility = PXUIVisibility.SelectorVisible)]
    public virtual int? UsrPGPreparer { get; set; }
    public abstract class usrPGPreparer : PX.Data.BQL.BqlInt.Field<usrPGPreparer> { }
    #endregion

    #region UsrPGReviewer
    [PXDBInt]
    [PXEPEmployeeSelector]
    [PXUIField(DisplayName = "Reviewer", Visibility = PXUIVisibility.SelectorVisible)]
    public virtual int? UsrPGReviewer { get; set; }
    public abstract class usrPGReviewer : PX.Data.BQL.BqlInt.Field<usrPGReviewer> { }
    #endregion

    #region UsrPGTechReviewer
    [PXDBInt]
    [PXEPEmployeeSelector]
    [PXUIField(DisplayName = "Tech Reviewer", Visibility = PXUIVisibility.SelectorVisible)]
    public virtual int? UsrPGTechReviewer { get; set; }
    public abstract class usrPGTechReviewer : PX.Data.BQL.BqlInt.Field<usrPGTechReviewer> { }
    #endregion

    #region UsrPGTaskStatus
    [PXDBInt]
    [PXIntList(new int[] {01, 02, 03, 04, 05}, new string[] {"Not Started", "In Process", "Waiting on Information", "Ready for Review", "Bill Client"})]
    [PXUIField(DisplayName="Task Status")]
    public virtual int? UsrPGTaskStatus { get; set; }
    public abstract class usrPGTaskStatus : PX.Data.BQL.BqlInt.Field<usrPGTaskStatus> { }
    #endregion

    #region UsrPGDepartment
    [PXDBString(10)]
    [PXSelector(typeof(PX.Objects.EP.EPDepartment.departmentID))]
    [PXUIField(DisplayName="Department")]
    public virtual string UsrPGDepartment { get; set; }
    public abstract class usrPGDepartment : PX.Data.BQL.BqlString.Field<usrPGDepartment> { }
    #endregion
  }
}