using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.PM;

namespace ProjectTask
{
  public class PMSampleTestingMaint : PXGraph<PMSampleTestingMaint, PMSampleTesting>
  {
        public PXSelectJoin<PMSampleTesting,
            LeftJoin<PMProject, On<PMSampleTesting.projectID, Equal<PMProject.contractID>>>,
            Where<PMProject.nonProject, Equal<False>,
                And<PMProject.baseType, Equal<PX.Objects.CT.CTPRType.project>>>> Sampletest;

        public PXSelect<PMSampleTesting, Where<PMSampleTesting.projectID, Equal<Current<PMSampleTesting.projectID>>>> SampletestProperties;

        public SelectFrom<PMSampleTesting>.View SampleTesting;


        protected void _(Events.FieldUpdated<PMSampleTesting, PMSampleTesting.co01> e)
        {

            var row = e.Row;
            row.CO01LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
            row.CO01LastModUserName = (string)base.Accessinfo.UserName;

            if (row.FirstContactDate == null)
            {
                row.FirstContactDate = PX.Common.PXTimeZoneInfo.Now.Date;
            }

        }

        protected void _(Events.FieldUpdated<PMSampleTesting, PMSampleTesting.co02> e)
        {

            var row = e.Row;
            row.CO02LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
            row.CO02LastModUserName = (string)base.Accessinfo.UserName;

            if (row.FirstContactDate == null)
            {
                row.FirstContactDate = PX.Common.PXTimeZoneInfo.Now.Date;
            }

        }

        protected void _(Events.FieldUpdated<PMSampleTesting, PMSampleTesting.co03> e)
        {

            var row = e.Row;
            row.CO03LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
            row.CO03LastModUserName = (string)base.Accessinfo.UserName;

            if (row.FirstContactDate == null)
            {
                row.FirstContactDate = PX.Common.PXTimeZoneInfo.Now.Date;
            }

        }

        protected void _(Events.FieldUpdated<PMSampleTesting, PMSampleTesting.co04> e)
        {

            var row = e.Row;
            row.CO04LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
            row.CO04LastModUserName = (string)base.Accessinfo.UserName;

            if (row.FirstContactDate == null)
            {
                row.FirstContactDate = PX.Common.PXTimeZoneInfo.Now.Date;
            }

        }

        protected void _(Events.FieldUpdated<PMSampleTesting, PMSampleTesting.co05> e)
        {

            var row = e.Row;
            row.CO05LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
            row.CO05LastModUserName = (string)base.Accessinfo.UserName;

            if (row.FirstContactDate == null)
            {
                row.FirstContactDate = PX.Common.PXTimeZoneInfo.Now.Date;
            }

        }

        protected void _(Events.FieldUpdated<PMSampleTesting, PMSampleTesting.co06> e)
        {

            var row = e.Row;
            row.CO06LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
            row.CO06LastModUserName = (string)base.Accessinfo.UserName;

            if (row.FirstContactDate == null)
            {
                row.FirstContactDate = PX.Common.PXTimeZoneInfo.Now.Date;
            }

        }

        protected void _(Events.FieldUpdated<PMSampleTesting, PMSampleTesting.co07> e)
        {

            var row = e.Row;
            row.CO07LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
            row.CO07LastModUserName = (string)base.Accessinfo.UserName;

            if (row.FirstContactDate == null)
            {
                row.FirstContactDate = PX.Common.PXTimeZoneInfo.Now.Date;
            }

        }

        protected void _(Events.FieldUpdated<PMSampleTesting, PMSampleTesting.co08> e)
        {

            var row = e.Row;
            row.CO08LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
            row.CO08LastModUserName = (string)base.Accessinfo.UserName;

            if (row.FirstContactDate == null)
            {
                row.FirstContactDate = PX.Common.PXTimeZoneInfo.Now.Date;
            }

        }

        protected void _(Events.FieldUpdated<PMSampleTesting, PMSampleTesting.bp01> e)
        {

            var row = e.Row;
            row.BP01LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
            row.BP01LastModUserName = (string)base.Accessinfo.UserName;

            if (row.FirstContactDate == null)
            {
                row.FirstContactDate = PX.Common.PXTimeZoneInfo.Now.Date;
            }

        }

        protected void _(Events.FieldUpdated<PMSampleTesting, PMSampleTesting.lo01> e)
        {

            var row = e.Row;
            row.LO01LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
            row.LO01LastModUserName = (string)base.Accessinfo.UserName;

            if (row.FirstContactDate == null)
            {
                row.FirstContactDate = PX.Common.PXTimeZoneInfo.Now.Date;
            }

        }

        protected void _(Events.FieldUpdated<PMSampleTesting, PMSampleTesting.ia08> e)
        {

            var row = e.Row;
            row.IA08LastModifiedDateTime = PX.Common.PXTimeZoneInfo.Now.Date;
            row.IA08LastModUserName = (string)base.Accessinfo.UserName;

            if (row.FirstContactDate == null)
            {
                row.FirstContactDate = PX.Common.PXTimeZoneInfo.Now.Date;
            }

        }


    }
}