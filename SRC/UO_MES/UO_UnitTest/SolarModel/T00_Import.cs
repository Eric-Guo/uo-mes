using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using UO_ExcelModeler;

namespace UO_UnitTest.SolarModel
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
        public void T10_BasicData_Insert()
        {
            excelModeler.Import(@"..\..\..\..\MES_Model\Solar\S10 BasicData.xls");
        }

        [Test]
        public void T20_Equipment_Insert()
        {
            excelModeler.Import(@"..\..\..\..\MES_Model\Solar\S20 Equipment.xls");
        }

        [Test]
        public void T30_Employee_Insert()
        {
            excelModeler.Import(@"..\..\..\..\MES_Model\Solar\S30 Employee.xls");
        }
    }
}
