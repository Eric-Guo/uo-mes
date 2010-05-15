using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using UO_Model.Physical;
using UO_Model.Process;
using UO_Service.ContainerTxn;
using UO_Model.Workflow;
using UO_Model.Execution;

namespace UO_UnitTest.BaseModel
{
    [TestFixture, Description("Test UO Excel Modeler function.")]
    public class T10_ContainerTxn
    {
        [Test]
        public void T010_Start_LOT1()
        {
            Start s = new Start();  // s stands for service
            s.Factory_Name = "F1";
            s.MfgOrder_Name = "MfgOrder_PA_r2";
            s.Workflow_Revision = "WF_First(r1)";
            s.WorkflowStep = s.Workflow.FirstStep;
            s.StartReason_Name = "Engineer";

            StartDetail d = new StartDetail();    // d stands for detail
            d.ContainerName = "LOT1";
            d.ContainerLevel_Name = "Lot";
            d.ContainerStatus_Name = "Active";
            d.Product_Revision = "ProductA(r2)";

            s.InsertDetail(d);
            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }

        [Test]
        public void T020_Start_LOT2()
        {
            Start s = new Start();
            s.Factory_Name = "F1";
            s.MfgOrder_Name = "MfgOrder_PA_r2";
            s.Workflow_Revision = "WF_First(r1)";
            s.WorkflowStep = s.Workflow.FirstStep;
            s.StartReason_Name = "Engineer";

            StartDetail d = new StartDetail();
            d.ContainerName = "LOT2";
            d.ContainerLevel_Name = "Lot";
            d.ContainerStatus_Name = "Active";
            d.Product_Revision = "ProductA(r2)";

            s.InsertDetail(d);
            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }

        [Test]
        public void T030_MoveTxn_LOT1()
        {
            MoveTxn s = new MoveTxn();
            s.Container_Name = "LOT1";
            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }

        [Test]
        public void T040_MoveTxn_LOT2()
        {
            MoveTxn s = new MoveTxn();
            s.Container_Name = "LOT2";
            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }

        [Test]
        public void T050_Hold_LOT1()
        {
            Hold s = new Hold();
            Container co = new Container();
            co.ContainerName = "LOT1";
            s.Container = co;
            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }

        [Test]
        public void T060_Hold_LOT1_AGAIN()
        {
            Hold s = new Hold();
            Container co = new Container();
            co.ContainerName = "LOT1";
            s.Container = co;
            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }

        [Test]
        public void T070_Release_LOT1()
        {
            Release s = new Release();
            Container co = new Container();
            co.ContainerName = "LOT1";
            s.Container = co;
            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }

        [Test]
        public void T080_Release_LOT1_AGAIN()
        {
            Release s = new Release();
            Container co = new Container();
            co.ContainerName = "LOT1";
            s.Container = co;
            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }

        [Test]
        public void T090_Split_LOT1()
        {
            Split s = new Split();
            Container co = new Container();
            co.ContainerName = "LOT1";
            s.Container = co;
            s.CloseWhenEmpty = false;

            SplitDetail d = new SplitDetail();
            d.ContainerName = "LOT1-1";
            d.Qty = 2;
            s.InsertDetail(d);

            d = new SplitDetail();
            d.ContainerName = "LOT1-2";
            d.Qty = 2;
            s.InsertDetail(d);

            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }

        [Test]
        public void T100_Combine_LOT1()
        {
            Combine s = new Combine();
            Container co = new Container();
            co.ContainerName = "LOT1";
            s.Container = co;
            s.CloseWhenEmpty = true;

            CombineFromDetail d = new CombineFromDetail();
            d.FromContainerName = "LOT1-1";
            d.Qty = 1;
            s.InsertDetail(d);

            d = new CombineFromDetail();
            d.FromContainerName = "LOT1-2";
            d.CombineAllQty = true;
            s.InsertDetail(d);

            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }

        [Test]
        public void T110_Defect_LOT1()
        {
            Defect s = new Defect();
            Container co = new Container();
            co.ContainerName = "LOT1";
            s.Container = co;

            DefectDetail d = new DefectDetail();
            d.DefectQty = 1;
            s.InsertDetail(d);

            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }

        [Test]
        public void T120_Associate_LOT1()
        {
            Associate s = new Associate();
            Container co = new Container();
            co.ContainerName = "LOT1";
            s.Container = co;

            AssociateDetail sd;
            sd = new AssociateDetail();
            sd.ChildContainerName = "LOT1-1";
            s.InsertDetail(sd);
            sd = new AssociateDetail();
            sd.ChildContainerName = "LOT1-2";
            s.InsertDetail(sd);

            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }

        [Test]
        public void T130_Disassociate_LOT1()
        {
            Disassociate s = new Disassociate();
            s.Container_Name = "LOT1";
            s.DisassociateAll = true;
            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }

        [Test]
        public void T140_CollectData_LOT1()
        {
            CollectData s = new CollectData();

            Container co = new Container();
            co.ContainerName = "LOT1";
            s.Container = co;

            CollectDataDetail d = new CollectDataDetail();    // d stands for detail
            d.DataCollectionDef_Name = "WIPData(r1)";
            d.DataCollectionValue = "10";
            s.InsertDetail(d);

            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }

        [Test]
        public void T150_ComponentIssue()
        {
            ComponentIssue s = new ComponentIssue();
            Container co = new Container();
            co.ContainerName = "LOT1";
            s.Container = co;
     
            ComponentIssueDetail d = new ComponentIssueDetail();    // d stands for detail
            d.IssueControl = 1;
            d.Product_Name = "ProductA(r2)";
            d.Container_Name = "LOT1";
            d.ActualQtyIssued = 2;
            s.InsertDetail(d);

            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }
    }
}
