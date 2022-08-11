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
    #region UsrPreparer
    [PXDBInt]
    [PXEPEmployeeSelector]
    [PXUIField(DisplayName = "Preparer", Visibility = PXUIVisibility.SelectorVisible)]
    public virtual int? UsrPreparer { get; set; }
    public abstract class usrPreparer : PX.Data.BQL.BqlInt.Field<usrPreparer> { }
    #endregion

    #region UsrPKTaskStatus
    [PXDBInt]
    [PXIntList(new int[] {01, 02, 03, 04}, new string[] {"Not Started", "In Process", "Waiting on Information", "Ready for Review"})]
    [PXUIField(DisplayName="Task Status")]
    public virtual int? UsrPKTaskStatus { get; set; }
    public abstract class usrPKTaskStatus : PX.Data.BQL.BqlInt.Field<usrPKTaskStatus> { }
    #endregion

    #region UsrStatutoryDueDate
    [PXDBDate()]
    [PXUIField(DisplayName = "Statutory Due Date")]
    public virtual DateTime? UsrStatutoryDueDate { get; set; }
    public abstract class usrStatutoryDueDate : PX.Data.BQL.BqlDateTime.Field<usrStatutoryDueDate> { }
    #endregion

    #region UsrPKIsDocumentGathering
    [PXDBBool]
    [PXUIField(DisplayName="401k Document Gathering")]

    public virtual bool? UsrPKIsDocumentGathering { get; set; }
    public abstract class usrPKIsDocumentGathering : PX.Data.BQL.BqlBool.Field<usrPKIsDocumentGathering> { }
    #endregion

    #region UsrPKIsSampleTesting
    [PXDBBool]
    [PXUIField(DisplayName="401k Sample Testing")]

    public virtual bool? UsrPKIsSampleTesting { get; set; }
    public abstract class usrPKIsSampleTesting : PX.Data.BQL.BqlBool.Field<usrPKIsSampleTesting> { }
    #endregion

    #region UsrPK401kDocSPD
    [PXDBBool]
    [PXUIField(DisplayName="SPD")]

    public virtual bool? UsrPK401kDocSPD { get; set; }
    public abstract class usrPK401kDocSPD : PX.Data.BQL.BqlBool.Field<usrPK401kDocSPD> { }
    #endregion

    #region UsrPK401kDocSPDNotes
    [PXDBString(128)]
    [PXUIField(DisplayName="SPD Notes")]

    public virtual string UsrPK401kDocSPDNotes { get; set; }
    public abstract class usrPK401kDocSPDNotes : PX.Data.BQL.BqlString.Field<usrPK401kDocSPDNotes> { }
    #endregion

    #region UsrPK401kDocAA
    [PXDBBool]
    [PXUIField(DisplayName="AA")]

    public virtual bool? UsrPK401kDocAA { get; set; }
    public abstract class usrPK401kDocAA : PX.Data.BQL.BqlBool.Field<usrPK401kDocAA> { }
    #endregion

    #region UsrPK401kDocBPD
    [PXDBBool]
    [PXUIField(DisplayName="BPD")]

    public virtual bool? UsrPK401kDocBPD { get; set; }
    public abstract class usrPK401kDocBPD : PX.Data.BQL.BqlBool.Field<usrPK401kDocBPD> { }
    #endregion

    #region UsrPK401kDocIRSOpinion
    [PXDBBool]
    [PXUIField(DisplayName="IRS Opinion Letter")]

    public virtual bool? UsrPK401kDocIRSOpinion { get; set; }
    public abstract class usrPK401kDocIRSOpinion : PX.Data.BQL.BqlBool.Field<usrPK401kDocIRSOpinion> { }
    #endregion

    #region UsrPK401kDocAANotes
    [PXDBString(128)]
    [PXUIField(DisplayName="AA Notes")]

    public virtual string UsrPK401kDocAANotes { get; set; }
    public abstract class usrPK401kDocAANotes : PX.Data.BQL.BqlString.Field<usrPK401kDocAANotes> { }
    #endregion

    #region UsrPK401kDocBPDNotes
    [PXDBString(128)]
    [PXUIField(DisplayName="BPD Notes")]

    public virtual string UsrPK401kDocBPDNotes { get; set; }
    public abstract class usrPK401kDocBPDNotes : PX.Data.BQL.BqlString.Field<usrPK401kDocBPDNotes> { }
    #endregion

    #region UsrPK401kDocIRSOpinionNotes
    [PXDBString(128)]
    [PXUIField(DisplayName="IRS Opinion Letter Notes")]

    public virtual string UsrPK401kDocIRSOpinionNotes { get; set; }
    public abstract class usrPK401kDocIRSOpinionNotes : PX.Data.BQL.BqlString.Field<usrPK401kDocIRSOpinionNotes> { }
    #endregion

    #region UsrPK401kSampleTCO01
    [PXDBInt]
    [PXIntList(new int[] {01, 02, 03, 04, 05}, new string[] {"Client Data Requested", "Client uploaded", "Prepared", "Reviewed", "Needs Correction"})]
    [PXUIField(DisplayName="CO-01")]
    public virtual int? UsrPK401kSampleTCO01 { get; set; }
    public abstract class usrPK401kSampleTCO01 : PX.Data.BQL.BqlInt.Field<usrPK401kSampleTCO01> { }
    #endregion

    #region UsrPK401kSampleCO01Date
    [PXDBDate()]
    //[PXDefault(typeof(AccessInfo.businessDate))]
    [PXUIField(DisplayName="Date Requested")]
    public virtual DateTime? UsrPK401kSampleCO01Date { get; set; }
    public abstract class usrPK401kSampleCO01Date : PX.Data.BQL.BqlDateTime.Field<usrPK401kSampleCO01Date> { }
    #endregion

    #region UsrPK401kSampleCO02Date
    [PXDBDate()]
    //[PXDefault(typeof(AccessInfo.businessDate))]
    [PXUIField(DisplayName="Date Requested")]
    public virtual DateTime? UsrPK401kSampleCO02Date { get; set; }
    public abstract class usrPK401kSampleCO02Date : PX.Data.BQL.BqlDateTime.Field<usrPK401kSampleCO02Date> { }
    #endregion

    #region UsrPK401kSampleCO03Date
    [PXDBDate()]
    //[PXDefault(typeof(AccessInfo.businessDate))]
    [PXUIField(DisplayName="Date Requested")]
    public virtual DateTime? UsrPK401kSampleCO03Date { get; set; }
    public abstract class usrPK401kSampleCO03Date : PX.Data.BQL.BqlDateTime.Field<usrPK401kSampleCO03Date> { }
    #endregion

    #region UsrPK401kSampleBP01Date
    [PXDBDate()]
    //[PXDefault(typeof(AccessInfo.businessDate))]
    [PXUIField(DisplayName="Date Requested")]
    public virtual DateTime? UsrPK401kSampleBP01Date { get; set; }
    public abstract class usrPK401kSampleBP01Date : PX.Data.BQL.BqlDateTime.Field<usrPK401kSampleBP01Date> { }
    #endregion

    #region UsrPK401kSampleLO01Date
    [PXDBDate()]
    //[PXDefault(typeof(AccessInfo.businessDate))]
    [PXUIField(DisplayName="Date Requested")]
    public virtual DateTime? UsrPK401kSampleLO01Date { get; set; }
    public abstract class usrPK401kSampleLO01Date : PX.Data.BQL.BqlDateTime.Field<usrPK401kSampleLO01Date> { }
    #endregion

    #region UsrPK401kSampleTBP01
    [PXDBInt]
    [PXIntList(new int[] {01, 02, 03, 04, 05}, new string[] {"Client Data Requested", "Client uploaded", "Prepared", "Reviewed", "Needs Correction"})]
    [PXUIField(DisplayName="BP-01")]
    public virtual int? UsrPK401kSampleTBP01 { get; set; }
    public abstract class usrPK401kSampleTBP01 : PX.Data.BQL.BqlInt.Field<usrPK401kSampleTBP01> { }
    #endregion

    #region UsrPK401kSampleTCO02
    [PXDBInt]
    [PXIntList(new int[] {01, 02, 03, 04, 05}, new string[] {"Client Data Requested", "Client uploaded", "Prepared", "Reviewed", "Needs Correction"})]
    [PXUIField(DisplayName="CO-02")]
    public virtual int? UsrPK401kSampleTCO02 { get; set; }
    public abstract class usrPK401kSampleTCO02 : PX.Data.BQL.BqlInt.Field<usrPK401kSampleTCO02> { }
    #endregion

    #region UsrPK401kSampleTCO03
    [PXDBInt]
    [PXIntList(new int[] {01, 02, 03, 04, 05}, new string[] {"Client Data Requested", "Client uploaded", "Prepared", "Reviewed", "Needs Correction"})]
    [PXUIField(DisplayName="CO-03")]
    public virtual int? UsrPK401kSampleTCO03 { get; set; }
    public abstract class usrPK401kSampleTCO03 : PX.Data.BQL.BqlInt.Field<usrPK401kSampleTCO03> { }
    #endregion

    #region UsrPK401kSampleTLO01
    [PXDBInt]
    [PXIntList(new int[] {01, 02, 03, 04, 05}, new string[] {"Client Data Requested", "Client uploaded", "Prepared", "Reviewed", "Needs Correction"})]
    [PXUIField(DisplayName="LO-01")]
    public virtual int? UsrPK401kSampleTLO01 { get; set; }
    public abstract class usrPK401kSampleTLO01 : PX.Data.BQL.BqlInt.Field<usrPK401kSampleTLO01> { }
    #endregion
  }
}