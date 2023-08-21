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
    /// The Samples will eventually be imported into Autire
    /// </summary>
    
  [Serializable]
  [PXCacheName("PMSampleTesting")]
  public class PMSampleTesting : IBqlTable
  {
    #region ProjectID
    public abstract class projectID : PX.Data.BQL.BqlInt.Field<projectID> { }

    /// copy from PX.Objects.PM.PMTask
    protected Int32? _ProjectID;
    [Project(DisplayName = "Project ID", IsKey = true)]
    [PXParent(typeof(Select<PMProject, Where<PMProject.contractID, Equal<Current<PMSampleTesting.projectID>>>>))]
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

    #region PlanName
    [PXDBString(510, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Plan Name")]
    public virtual string PlanName { get; set; }
    public abstract class planName : PX.Data.BQL.BqlString.Field<planName> { }
    #endregion

    #region AdditionalNotes
    [PXDBString(510, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Notes")]
    public virtual string AdditionalNotes { get; set; }
    public abstract class additionalNotes : PX.Data.BQL.BqlString.Field<additionalNotes> { }
    #endregion

    #region Complete
    [PXDBBool()]
    [PXDefault(false)]
    [PXUIField(DisplayName = "Complete")]
    public virtual bool? Complete { get; set; }
    public abstract class complete : PX.Data.BQL.BqlBool.Field<complete> { }
    #endregion


    #region FirstContactDate
    [PXDBDate()]
    [PXUIField(DisplayName = "First Contact")]
    public virtual DateTime? FirstContactDate { get; set; }
    public abstract class firstContactDate : PX.Data.BQL.BqlDateTime.Field<firstContactDate> { }
    #endregion

    #region Tester
    [PXDBInt]
    [PXEPEmployeeSelector]
    [PXUIField(DisplayName = "Tester", Visibility = PXUIVisibility.SelectorVisible)]
    public virtual int? Tester { get; set; }
    public abstract class tester : PX.Data.BQL.BqlInt.Field<tester> { }
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


    #region Priority
    [PXDBString(1)]
    [PXStringList(
            new string[] 
            {
                SamplePriority.High, SamplePriority.Medium, SamplePriority.Low, SamplePriority.Post, SamplePriority.InitialReview
            }, 
            new string[] 
            {
                MessageSamplePriority.High, MessageSamplePriority.Medium, MessageSamplePriority.Low, MessageSamplePriority.Post, MessageSamplePriority.InitialReview
            })]
    [PXUIField(DisplayName = "Priority")]
    public virtual string Priority { get; set; }
    public abstract class priority : PX.Data.BQL.BqlString.Field<priority> { }
    #endregion

    #region TPA
    [PXDBString(255, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "TPA")]
    public virtual string TPA { get; set; }
    public abstract class tpa : PX.Data.BQL.BqlString.Field<tpa> { }
    #endregion

    #region CO01
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                SampleStatus.Contacted1, SampleStatus.Contacted2, SampleStatus.ClientCalled, SampleStatus.TPARKContacted, SampleStatus.WaitingonInfo, SampleStatus.InProcess, SampleStatus.ClearingPoints, SampleStatus.Completed, SampleStatus.Ghost, SampleStatus.NA, SampleStatus.Reviewed
            }, 
            new string[]
            {
                MessageSampleStatus.Contacted1, MessageSampleStatus.Contacted2, MessageSampleStatus.ClientCalled, MessageSampleStatus.TPARKContacted, MessageSampleStatus.WaitingonInfo, MessageSampleStatus.InProcess, MessageSampleStatus.ClearingPoints, MessageSampleStatus.Completed, MessageSampleStatus.Ghost, MessageSampleStatus.NA, MessageSampleStatus.Reviewed
            })]
    [PXUIField(DisplayName = "CO-01")]
    public virtual string CO01 { get; set; }
    public abstract class co01 : PX.Data.BQL.BqlString.Field<co01> { }
    #endregion

    #region CO01LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? CO01LastModifiedDateTime { get; set; }
    public abstract class co01LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<co01LastModifiedDateTime> { }
    #endregion

    #region CO01LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string CO01LastModUserName { get; set; }
    public abstract class co01LastModUserName : PX.Data.BQL.BqlString.Field<co01LastModUserName> { }
    #endregion

    #region CO02
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                SampleStatus.Contacted1, SampleStatus.Contacted2, SampleStatus.ClientCalled, SampleStatus.TPARKContacted, SampleStatus.WaitingonInfo, SampleStatus.InProcess, SampleStatus.ClearingPoints, SampleStatus.Completed, SampleStatus.Ghost, SampleStatus.NA, SampleStatus.Reviewed
            }, 
            new string[]
            {
                MessageSampleStatus.Contacted1, MessageSampleStatus.Contacted2, MessageSampleStatus.ClientCalled, MessageSampleStatus.TPARKContacted, MessageSampleStatus.WaitingonInfo, MessageSampleStatus.InProcess, MessageSampleStatus.ClearingPoints, MessageSampleStatus.Completed, MessageSampleStatus.Ghost, MessageSampleStatus.NA, MessageSampleStatus.Reviewed
            })]
    [PXUIField(DisplayName = "CO-02")]
    public virtual string CO02 { get; set; }
    public abstract class co02 : PX.Data.BQL.BqlString.Field<co02> { }
    #endregion

    #region CO02LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? CO02LastModifiedDateTime { get; set; }
    public abstract class co02LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<co02LastModifiedDateTime> { }
    #endregion

    #region CO02LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string CO02LastModUserName { get; set; }
    public abstract class co02LastModUserName : PX.Data.BQL.BqlString.Field<co02LastModUserName> { }
    #endregion

    #region CO03
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                SampleStatus.Contacted1, SampleStatus.Contacted2, SampleStatus.ClientCalled, SampleStatus.TPARKContacted, SampleStatus.WaitingonInfo, SampleStatus.InProcess, SampleStatus.ClearingPoints, SampleStatus.Completed, SampleStatus.Ghost, SampleStatus.NA, SampleStatus.Reviewed
            }, 
            new string[]
            {
                MessageSampleStatus.Contacted1, MessageSampleStatus.Contacted2, MessageSampleStatus.ClientCalled, MessageSampleStatus.TPARKContacted, MessageSampleStatus.WaitingonInfo, MessageSampleStatus.InProcess, MessageSampleStatus.ClearingPoints, MessageSampleStatus.Completed, MessageSampleStatus.Ghost, MessageSampleStatus.NA, MessageSampleStatus.Reviewed
            })]
    [PXUIField(DisplayName = "CO-03")]
    public virtual string CO03 { get; set; }
    public abstract class co03 : PX.Data.BQL.BqlString.Field<co03> { }
    #endregion

    #region CO03LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? CO03LastModifiedDateTime { get; set; }
    public abstract class co03LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<co03LastModifiedDateTime> { }
    #endregion

    #region CO03LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string CO03LastModUserName { get; set; }
    public abstract class co03LastModUserName : PX.Data.BQL.BqlString.Field<co03LastModUserName> { }
    #endregion

    #region CO04
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                SampleStatus.Contacted1, SampleStatus.Contacted2, SampleStatus.ClientCalled, SampleStatus.TPARKContacted, SampleStatus.WaitingonInfo, SampleStatus.InProcess, SampleStatus.ClearingPoints, SampleStatus.Completed, SampleStatus.Ghost, SampleStatus.NA, SampleStatus.Reviewed
            }, 
            new string[]
            {
                MessageSampleStatus.Contacted1, MessageSampleStatus.Contacted2, MessageSampleStatus.ClientCalled, MessageSampleStatus.TPARKContacted, MessageSampleStatus.WaitingonInfo, MessageSampleStatus.InProcess, MessageSampleStatus.ClearingPoints, MessageSampleStatus.Completed, MessageSampleStatus.Ghost, MessageSampleStatus.NA, MessageSampleStatus.Reviewed
            })]
    [PXUIField(DisplayName = "CO-04")]
    public virtual string CO04 { get; set; }
    public abstract class co04 : PX.Data.BQL.BqlString.Field<co04> { }
    #endregion

    #region CO04LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? CO04LastModifiedDateTime { get; set; }
    public abstract class co04LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<co04LastModifiedDateTime> { }
    #endregion

    #region CO04LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string CO04LastModUserName { get; set; }
    public abstract class co04LastModUserName : PX.Data.BQL.BqlString.Field<co04LastModUserName> { }
    #endregion

    #region CO05
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                SampleStatus.Contacted1, SampleStatus.Contacted2, SampleStatus.ClientCalled, SampleStatus.TPARKContacted, SampleStatus.WaitingonInfo, SampleStatus.InProcess, SampleStatus.ClearingPoints, SampleStatus.Completed, SampleStatus.Ghost, SampleStatus.NA, SampleStatus.Reviewed
            }, 
            new string[]
            {
                MessageSampleStatus.Contacted1, MessageSampleStatus.Contacted2, MessageSampleStatus.ClientCalled, MessageSampleStatus.TPARKContacted, MessageSampleStatus.WaitingonInfo, MessageSampleStatus.InProcess, MessageSampleStatus.ClearingPoints, MessageSampleStatus.Completed, MessageSampleStatus.Ghost, MessageSampleStatus.NA, MessageSampleStatus.Reviewed
            })]
    [PXUIField(DisplayName = "CO-05")]
    public virtual string CO05 { get; set; }
    public abstract class co05 : PX.Data.BQL.BqlString.Field<co05> { }
    #endregion

    #region CO05LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? CO05LastModifiedDateTime { get; set; }
    public abstract class co05LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<co05LastModifiedDateTime> { }
    #endregion

    #region CO05LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string CO05LastModUserName { get; set; }
    public abstract class co05LastModUserName : PX.Data.BQL.BqlString.Field<co05LastModUserName> { }
    #endregion

    #region CO06
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                SampleStatus.Contacted1, SampleStatus.Contacted2, SampleStatus.ClientCalled, SampleStatus.TPARKContacted, SampleStatus.WaitingonInfo, SampleStatus.InProcess, SampleStatus.ClearingPoints, SampleStatus.Completed, SampleStatus.Ghost, SampleStatus.NA, SampleStatus.Reviewed
            }, 
            new string[]
            {
                MessageSampleStatus.Contacted1, MessageSampleStatus.Contacted2, MessageSampleStatus.ClientCalled, MessageSampleStatus.TPARKContacted, MessageSampleStatus.WaitingonInfo, MessageSampleStatus.InProcess, MessageSampleStatus.ClearingPoints, MessageSampleStatus.Completed, MessageSampleStatus.Ghost, MessageSampleStatus.NA, MessageSampleStatus.Reviewed
            })]
    [PXUIField(DisplayName = "CO-06")]
    public virtual string CO06 { get; set; }
    public abstract class co06 : PX.Data.BQL.BqlString.Field<co06> { }
    #endregion

    #region CO06LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? CO06LastModifiedDateTime { get; set; }
    public abstract class co06LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<co06LastModifiedDateTime> { }
    #endregion

    #region CO06LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string CO06LastModUserName { get; set; }
    public abstract class co06LastModUserName : PX.Data.BQL.BqlString.Field<co06LastModUserName> { }
    #endregion

    #region CO07
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                SampleStatus.Contacted1, SampleStatus.Contacted2, SampleStatus.ClientCalled, SampleStatus.TPARKContacted, SampleStatus.WaitingonInfo, SampleStatus.InProcess, SampleStatus.ClearingPoints, SampleStatus.Completed, SampleStatus.Ghost, SampleStatus.NA, SampleStatus.Reviewed
            }, 
            new string[]
            {
                MessageSampleStatus.Contacted1, MessageSampleStatus.Contacted2, MessageSampleStatus.ClientCalled, MessageSampleStatus.TPARKContacted, MessageSampleStatus.WaitingonInfo, MessageSampleStatus.InProcess, MessageSampleStatus.ClearingPoints, MessageSampleStatus.Completed, MessageSampleStatus.Ghost, MessageSampleStatus.NA, MessageSampleStatus.Reviewed
            })]
    [PXUIField(DisplayName = "CO-07")]
    public virtual string CO07 { get; set; }
    public abstract class co07 : PX.Data.BQL.BqlString.Field<co07> { }
    #endregion

    #region CO07LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? CO07LastModifiedDateTime { get; set; }
    public abstract class co07LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<co07LastModifiedDateTime> { }
    #endregion

    #region CO07LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string CO07LastModUserName { get; set; }
    public abstract class co07LastModUserName : PX.Data.BQL.BqlString.Field<co07LastModUserName> { }
    #endregion

    #region CO08
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                SampleStatus.Contacted1, SampleStatus.Contacted2, SampleStatus.ClientCalled, SampleStatus.TPARKContacted, SampleStatus.WaitingonInfo, SampleStatus.InProcess, SampleStatus.ClearingPoints, SampleStatus.Completed, SampleStatus.Ghost, SampleStatus.NA, SampleStatus.Reviewed
            }, 
            new string[]
            {
                MessageSampleStatus.Contacted1, MessageSampleStatus.Contacted2, MessageSampleStatus.ClientCalled, MessageSampleStatus.TPARKContacted, MessageSampleStatus.WaitingonInfo, MessageSampleStatus.InProcess, MessageSampleStatus.ClearingPoints, MessageSampleStatus.Completed, MessageSampleStatus.Ghost, MessageSampleStatus.NA, MessageSampleStatus.Reviewed
            })]
    [PXUIField(DisplayName = "CO-08")]
    public virtual string CO08 { get; set; }
    public abstract class co08 : PX.Data.BQL.BqlString.Field<co08> { }
    #endregion

    #region CO08LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? CO08LastModifiedDateTime { get; set; }
    public abstract class co08LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<co08LastModifiedDateTime> { }
    #endregion

    #region CO08LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string CO08LastModUserName { get; set; }
    public abstract class co08LastModUserName : PX.Data.BQL.BqlString.Field<co08LastModUserName> { }
    #endregion

    #region BP01
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                SampleStatus.Contacted1, SampleStatus.Contacted2, SampleStatus.ClientCalled, SampleStatus.TPARKContacted, SampleStatus.WaitingonInfo, SampleStatus.InProcess, SampleStatus.ClearingPoints, SampleStatus.Completed, SampleStatus.Ghost, SampleStatus.NA, SampleStatus.Reviewed
            }, 
            new string[]
            {
                MessageSampleStatus.Contacted1, MessageSampleStatus.Contacted2, MessageSampleStatus.ClientCalled, MessageSampleStatus.TPARKContacted, MessageSampleStatus.WaitingonInfo, MessageSampleStatus.InProcess, MessageSampleStatus.ClearingPoints, MessageSampleStatus.Completed, MessageSampleStatus.Ghost, MessageSampleStatus.NA, MessageSampleStatus.Reviewed
            })]
    [PXUIField(DisplayName = "BP-01")]
    public virtual string BP01 { get; set; }
    public abstract class bp01 : PX.Data.BQL.BqlString.Field<bp01> { }
    #endregion

    #region BP01LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? BP01LastModifiedDateTime { get; set; }
    public abstract class bp01LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<bp01LastModifiedDateTime> { }
    #endregion

    #region BP01LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string BP01LastModUserName { get; set; }
    public abstract class bp01LastModUserName : PX.Data.BQL.BqlString.Field<bp01LastModUserName> { }
    #endregion

    #region LO01
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                SampleStatus.Contacted1, SampleStatus.Contacted2, SampleStatus.ClientCalled, SampleStatus.TPARKContacted, SampleStatus.WaitingonInfo, SampleStatus.InProcess, SampleStatus.ClearingPoints, SampleStatus.Completed, SampleStatus.Ghost, SampleStatus.NA, SampleStatus.Reviewed
            }, 
            new string[]
            {
                MessageSampleStatus.Contacted1, MessageSampleStatus.Contacted2, MessageSampleStatus.ClientCalled, MessageSampleStatus.TPARKContacted, MessageSampleStatus.WaitingonInfo, MessageSampleStatus.InProcess, MessageSampleStatus.ClearingPoints, MessageSampleStatus.Completed, MessageSampleStatus.Ghost, MessageSampleStatus.NA, MessageSampleStatus.Reviewed
            })]
    [PXUIField(DisplayName = "LO-01")]
    public virtual string LO01 { get; set; }
    public abstract class lo01 : PX.Data.BQL.BqlString.Field<lo01> { }
    #endregion

    #region LO01LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? LO01LastModifiedDateTime { get; set; }
    public abstract class lo01LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lo01LastModifiedDateTime> { }
    #endregion

    #region LO01LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string LO01LastModUserName { get; set; }
    public abstract class lo01LastModUserName : PX.Data.BQL.BqlString.Field<lo01LastModUserName> { }
    #endregion

    #region IA08
    [PXDBString(2)]
    [PXStringList(
            new string[] 
            {
                SampleStatus.Contacted1, SampleStatus.Contacted2, SampleStatus.ClientCalled, SampleStatus.TPARKContacted, SampleStatus.WaitingonInfo, SampleStatus.InProcess, SampleStatus.ClearingPoints, SampleStatus.Completed, SampleStatus.Ghost, SampleStatus.NA, SampleStatus.Reviewed
            }, 
            new string[]
            {
                MessageSampleStatus.Contacted1, MessageSampleStatus.Contacted2, MessageSampleStatus.ClientCalled, MessageSampleStatus.TPARKContacted, MessageSampleStatus.WaitingonInfo, MessageSampleStatus.InProcess, MessageSampleStatus.ClearingPoints, MessageSampleStatus.Completed, MessageSampleStatus.Ghost, MessageSampleStatus.NA, MessageSampleStatus.Reviewed
            })]
    [PXUIField(DisplayName = "IA-08")]
    public virtual string IA08 { get; set; }
    public abstract class ia08 : PX.Data.BQL.BqlString.Field<ia08> { }
    #endregion

    #region IA08LastModifiedDateTime
    [PXDBDate()]
    [PXUIField(DisplayName = "Last Modified")]
    public virtual DateTime? IA08LastModifiedDateTime { get; set; }
    public abstract class ia08LastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<ia08LastModifiedDateTime> { }
    #endregion

    #region IA08LastModUserName
    [PXDBString(256, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Last Modified By")]
    public virtual string IA08LastModUserName { get; set; }
    public abstract class ia08LastModUserName : PX.Data.BQL.BqlString.Field<ia08LastModUserName> { }
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