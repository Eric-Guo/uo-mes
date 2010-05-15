using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using UO_ExcelModeler;

namespace UO_UnitTest.BaseModel
{
    [TestFixture, Description("Test UO Excel Modeler function.")]
    public class T00_Import
    {
        ExcelModeler excelModeler;
        [TestFixtureSetUp]
        public void Init()
        {
            excelModeler = new ExcelModeler("UO_Model.dll","UO_Service.dll");
        }

        [Test]
        public void T00_MES_Basic_Data_Insert()
        {
            excelModeler.Import(@"..\..\..\..\MES_Model\Base\S00 MES Basic Data.xls");
        }

        [Test]
        public void T10_Physical_Basic_Model_Insert()
        {
            excelModeler.Import(@"..\..\..\..\MES_Model\Base\S10 Physical Basic Model.xls");
        }

        [Test]
        public void T20_Process_Model_Insert()
        {
            excelModeler.Import(@"..\..\..\..\MES_Model\Base\S20 Process Model.xls");
        }

        [Test]
        public void T30_Workflow_Model_Insert()
        {
            excelModeler.Import(@"..\..\..\..\MES_Model\Base\S30 Workflow Model.xls");
        }

        [Test]
        public void T40_Execution_UserReason_Insert()
        {
            excelModeler.Import(@"..\..\..\..\MES_Model\Base\S40 Execution Reason.xls");
        }
    }
}
