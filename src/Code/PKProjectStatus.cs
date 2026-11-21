using System;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.EP;

namespace ProjectTask
{
    /// <summary>
    /// One status of a department's project status list. Maintained on PKPM2001 "Department Statuses"
    /// (graph <see cref="PKProjectStatusMaint"/>); table created and seeded by the Sql item PKProjectStatus.
    /// StatusID is the int stored in Contract.Usr401kStatus / UsrPGTaxStatus / UsrPGAcctStatus / UsrPGAuditStatus,
    /// so it is frozen once saved (GIs and business events test the ints). Retire a status with Active = false;
    /// saved rows are never deleted. Design: docs/status-design.md (T-20), docs/status-screen.md (T-21).
    /// </summary>
    [Serializable]
    [PXCacheName("Department Status")]
    public class PKProjectStatus : PXBqlTable, IBqlTable
    {
        #region Keys
        public class PK : PrimaryKeyOf<PKProjectStatus>.By<departmentID, statusID>
        {
            public static PKProjectStatus Find(PXGraph graph, string departmentID, int? statusID) => FindBy(graph, departmentID, statusID);
        }
        #endregion

        #region DepartmentID
        [PXDBString(10, IsUnicode = true, IsKey = true, InputMask = ">aaaaaaaaaa")]
        [PXDefault(typeof(PKProjectStatusFilter.departmentID))]
        [PXSelector(typeof(Search<EPDepartment.departmentID>), DescriptionField = typeof(EPDepartment.description))]
        [PXUIField(DisplayName = "Department", Visibility = PXUIVisibility.SelectorVisible, Enabled = false)]
        public virtual string DepartmentID { get; set; }
        public abstract class departmentID : BqlString.Field<departmentID> { }
        #endregion

        #region StatusID
        [PXDBInt(IsKey = true, MinValue = 1)]
        [PXDefault]
        [PXUIField(DisplayName = "Status ID", Visibility = PXUIVisibility.SelectorVisible)]
        public virtual int? StatusID { get; set; }
        public abstract class statusID : BqlInt.Field<statusID> { }
        #endregion

        #region Description
        [PXDBString(60, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = "Description", Required = true, Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string Description { get; set; }
        public abstract class description : BqlString.Field<description> { }
        #endregion

        #region SortOrder
        // Defaults to StatusID (graph FieldDefaulting); 0..32767 because the sync writes it to CSAttributeDetail.SortOrder (smallint).
        [PXDBInt(MinValue = 0, MaxValue = 32767)]
        [PXDefault]
        [PXFormula(typeof(Default<statusID>))]
        [PXUIField(DisplayName = "Sort Order")]
        public virtual int? SortOrder { get; set; }
        public abstract class sortOrder : BqlInt.Field<sortOrder> { }
        #endregion

        #region Active
        [PXDBBool]
        [PXDefault(true)]
        [PXUIField(DisplayName = "Active")]
        public virtual bool? Active { get; set; }
        public abstract class active : BqlBool.Field<active> { }
        #endregion

        #region Audit fields
        [PXDBCreatedByID]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : BqlGuid.Field<createdByID> { }

        [PXDBCreatedByScreenID]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : BqlString.Field<createdByScreenID> { }

        [PXDBCreatedDateTime]
        [PXUIField(DisplayName = "Created On", Enabled = false, IsReadOnly = true)]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : BqlDateTime.Field<createdDateTime> { }

        [PXDBLastModifiedByID]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : BqlGuid.Field<lastModifiedByID> { }

        [PXDBLastModifiedByScreenID]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : BqlString.Field<lastModifiedByScreenID> { }

        [PXDBLastModifiedDateTime]
        [PXUIField(DisplayName = "Last Modified On", Enabled = false, IsReadOnly = true)]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : BqlDateTime.Field<lastModifiedDateTime> { }

        [PXDBTimestamp]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : BqlByteArray.Field<tstamp> { }
        #endregion
    }

    /// <summary>Filter of PKPM2001: the department whose statuses the grid shows.</summary>
    [Serializable]
    [PXCacheName("Department Status Filter")]
    public class PKProjectStatusFilter : PXBqlTable, IBqlTable
    {
        #region DepartmentID
        [PXString(10, IsUnicode = true, InputMask = ">aaaaaaaaaa")]
        [PXSelector(typeof(Search<EPDepartment.departmentID>), DescriptionField = typeof(EPDepartment.description))]
        [PXUIField(DisplayName = "Department")]
        public virtual string DepartmentID { get; set; }
        public abstract class departmentID : BqlString.Field<departmentID> { }
        #endregion
    }

    [PXLocalizable]
    public static class PKProjectStatusMessages
    {
        public const string StatusIDMustBePositive = "The status ID must be greater than 0.";
        public const string StatusIDExists = "Status ID {0} already exists for department {1}.";
        public const string StatusIDReadOnly = "The ID of a saved status cannot be changed, because projects, generic inquiries and business events use it. Make the status inactive and add a new one instead.";
        public const string DepartmentReadOnly = "The department of a saved status cannot be changed.";
        public const string DescriptionExists = "The description '{0}' is already used by status {1} of department {2}.";
        public const string CannotDeleteSaved = "A saved status cannot be deleted, because projects, generic inquiries and business events may use its ID. Clear the Active check box instead.";
        public const string UnknownStatus = "{0} is not a status of department {1}. Statuses are maintained on the Department Statuses (PKPM2001) form.";
        public const string InactiveStatus = "The status '{0}' of department {1} is inactive and cannot be selected.";
        public const string AttributeMissing = "Department Statuses: attribute {0} was not found, so the statuses of department {1} were not copied to it.";
        public const string StatusListReadFailed = "Department Statuses: the PKProjectStatus table could not be read: {0}";
        public const string PreviousDepartment = "Previous Department";   // T-24 toolbar arrow (caption + tooltip)
        public const string NextDepartment = "Next Department";           // T-24 toolbar arrow (caption + tooltip)
    }
}
