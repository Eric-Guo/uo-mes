using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using UO_ExcelModeler;

namespace UO_UnitTest.TFModel
{
    [TestFixture, Description("Test UO Excel Modeler function.")]
    public class Import_Model
    {
        ExcelModeler excelModeler;
        [TestFixtureSetUp]
        public void Init()
        {
            excelModeler = new ExcelModeler("UO_Model.dll", "UO_Service.dll");
        }

        [Test]
        public void T10_PhysicalBasicModel_Insert()
        {
            excelModeler.Import(@"..\..\..\..\MES_Model\TF\T10 Physical Basic Model.xls");
        }

        [Test]
        public void T20_ResourceModel_Insert()
        {
            excelModeler.Import(@"..\..\..\..\MES_Model\TF\T20 Resource Model.xls");
        }

        [Test]
        public void T30_Process_Insert()
        {
            excelModeler.Import(@"..\..\..\..\MES_Model\TF\T30 Process Model.xls");
        }

        [Test]
        public void T40_WorkFlow_Insert()
        {
            excelModeler.Import(@"..\..\..\..\MES_Model\TF\T40 WorkFlow Model.xls");
        }

        [Test]
        public void T50_ExecutionUserReason_Insert()
        {
            excelModeler.Import(@"..\..\..\..\MES_Model\TF\T50 Execution Reason.xls");
        }

        [Test]
        public void T60_ProcessUserReason_Insert()
        {

            excelModeler.Import(@"..\..\..\..\MES_Model\TF\T60 Process Reason.xls");

        }

        [Test]
        public void T70_PhysicalUserReason_Insert()
        {

            excelModeler.Import(@"..\..\..\..\MES_Model\TF\T70 Physical Reason.xls");

        }
    }
}

