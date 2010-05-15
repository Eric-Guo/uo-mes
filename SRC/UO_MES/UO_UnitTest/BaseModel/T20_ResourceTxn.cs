using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using UO_Model.Physical;
using UO_Model.Process;
using UO_Service.ResourceTxn;
using UO_Model.Execution;
using UO_Model.Physical.Reason;
using UO_Model.Physical.Status;

namespace UO_UnitTest.BaseModel
{
    [TestFixture, Description("Test UO_Service.ResourceTxn")]
    public class T20_ResourceTxn
    {
        [Test]
        public void T10_ResourceSetup_WD01()
        {
            ResourceSetup s = new ResourceSetup();

            s.Resource_Name = "WD-01";

            ResourceStatusReason r = new ResourceStatusReason();
            s.ResourceStatusReason = r;

            ResourceStatus tostatus = new ResourceStatus();
            tostatus.Name = "PM";
            s.ResourceToStatus = tostatus;

            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }

        [Test]
        public void T20_ResourceThruput_WD01()
        {
            ResourceThruput s = new ResourceThruput();

            s.Resource_Name = "WD-01";

            s.Qty = 10;
            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }

        [Test]
        public void T30_CollectResourceData_WD01()
        {
            CollectResourceData s = new CollectResourceData();

            s.Resource_Name = "WD-01";

            ParametricDataDetail d = new ParametricDataDetail();    // d stands for detail
            d.DataCollectionDef_Name = "WIPData(r1)";
            d.DataCollectionValue = "10";
            s.InsertDetail(d);

            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }
    }
}
