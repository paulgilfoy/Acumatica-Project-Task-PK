using PX.CS.Contracts.Interfaces;
using PX.Data.BQL.Fluent;
using PX.Data.BQL;
using PX.Data.EP;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data.ReferentialIntegrity;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.AR;
using PX.Objects.CR.MassProcess;
using PX.Objects.CR.Workflows;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.GDPR;
using PX.Objects.SO;
using PX.Objects;
using PX.SM;
using PX.TM;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace PX.Objects.CR
{
	public class ContactExt : PXCacheExtension<PX.Objects.CR.Contact>
	{
		#region UsrPGPortalAccess
		[PXDBBool]
[PXUIField(DisplayName="Portal Access")]

		public virtual bool? UsrPGPortalAccess { get; set; }
		public abstract class usrPGPortalAccess : PX.Data.BQL.BqlBool.Field<usrPGPortalAccess> { }
		#endregion

		#region UsrPG1040ES
		[PXDBBool]
[PXUIField(DisplayName="1040-ES Reminder")]

		public virtual bool? UsrPG1040ES { get; set; }
		public abstract class usrPG1040ES : PX.Data.BQL.BqlBool.Field<usrPG1040ES> { }
		#endregion

		#region UsrPG1040Check
		[PXDBBool]
[PXUIField(DisplayName="1040 Checklist")]

		public virtual bool? UsrPG1040Check { get; set; }
		public abstract class usrPG1040Check : PX.Data.BQL.BqlBool.Field<usrPG1040Check> { }
		#endregion

		#region UsrPGTaxPlan
		[PXDBBool]
[PXUIField(DisplayName="Tax Planning Meeting")]

		public virtual bool? UsrPGTaxPlan { get; set; }
		public abstract class usrPGTaxPlan : PX.Data.BQL.BqlBool.Field<usrPGTaxPlan> { }
		#endregion

		#region UsrPGDoNotMarket
		[PXDBBool]
[PXUIField(DisplayName="Do Not Market")]

		public virtual bool? UsrPGDoNotMarket { get; set; }
		public abstract class usrPGDoNotMarket : PX.Data.BQL.BqlBool.Field<usrPGDoNotMarket> { }
		#endregion

		#region UsrPGAcuMarket
		[PXDBBool]
[PXUIField(DisplayName="Acumatica Marketing List")]

		public virtual bool? UsrPGAcuMarket { get; set; }
		public abstract class usrPGAcuMarket : PX.Data.BQL.BqlBool.Field<usrPGAcuMarket> { }
		#endregion

		#region UsrPGRetire
		[PXDBBool]
[PXUIField(DisplayName="PK Retirement Prospect")]

		public virtual bool? UsrPGRetire { get; set; }
		public abstract class usrPGRetire : PX.Data.BQL.BqlBool.Field<usrPGRetire> { }
		#endregion

		#region UsrServiceProviderQ
		[PXDBBool]
[PXUIField(DisplayName="Service Provider Questionnaire")]

		public virtual bool? UsrServiceProviderQ { get; set; }
		public abstract class usrServiceProviderQ : PX.Data.BQL.BqlBool.Field<usrServiceProviderQ> { }
		#endregion
	}
}