using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using UO_Model.Physical;
using UO_Service.Base;

namespace UO_UnitTest.Maint
{
    [TestFixture, Description("Test UO_Service.NamedObjectMaint basic Add and Update.")]
    public class NamedObjectMaint_Test
    {
        [Test, Description("Basic add without index property")]
        public void T10_Enterprise_Add()
        {
            Enterprise ent = new Enterprise();
            ent.Name = "Eric";
            ent.Description = DateTime.Now.ToString();
            ent.ReportHeading = "Eric Guo Reporting Heading";
            NamedObjectMaint mt = new NamedObjectMaint();
            Assert.IsTrue(mt.Add(ent));
        }

        [Test, Description("Basic update without index property")]
        public void T20_Enterprise_Update()
        {
            Enterprise ent = new Enterprise();
            ent.Name = "Eric";
            ent.ReportHeading = "Eric Guo Reporting Heading Updated at " + DateTime.Now.ToString();
            NamedObjectMaint mt = new NamedObjectMaint();
            Assert.IsTrue(mt.Update(ent));
        }

        [Test, Description("Basic delete without index property")]
        public void T30_Enterprise_Delete()
        {
            Enterprise ent = new Enterprise();
            ent.Name = "Eric";
            NamedObjectMaint mt = new NamedObjectMaint();
            Assert.IsTrue(mt.Remove(ent));
        }

        [Test, Description("Add with index property")]
        public void T40_Enterprise_Add_With_Factory()
        {
            Enterprise ent = new Enterprise();
            ent.Name = "UO Soft Company";
            ent.Description = DateTime.Now.ToString();
            ent.ReportHeading = "UO Soft Reporting Heading";
            Factory f1 = new Factory();
            f1.Name = "Factory 1";
            f1.Description = "Description for Factory 1";
            Factory f2 = new Factory();
            f2.Name = "Factory 2";
            f2.Description = "Description for Factory 2";
            ent.Factories.Add(f1);
            ent.Factories.Add(f2);
            NamedObjectMaint mt = new NamedObjectMaint();
            Assert.IsTrue(mt.Add(ent));
        }

        [Test, Description("Dump just added Enterprise")]
        public void T50_Enterprise_Dump_With_Factory()
        {
            Enterprise ent = new Enterprise();
            ent.Name = "UO Soft Company";
            NamedObjectMaint mt = new NamedObjectMaint();
            Assert.IsTrue(mt.Dump(ent));
        }

        [Test, Description("Dump factory")]
        public void T60_Factory_Dump()
        {
            Factory f = new Factory();
            f.Name = "Factory 1";
            NamedObjectMaint mt = new NamedObjectMaint();
            Assert.IsTrue(mt.Dump(f));
        }

        [Test, Description("Update with index property")]
        public void T70_Enterprise_Update_With_Factory()
        {
            Enterprise ent = new Enterprise();
            ent.Name = "UO Soft Company";
            ent.ReportHeading = "Updated UO Soft Reporting Heading";
            Factory f1 = new Factory();
            f1.Name = "Factory 1";
            f1.Description = "Description for Updated Factory 1"; // Exist NDO will only add, not update it's description
            Factory f2 = new Factory();
            f2.Name = "Updated Factory 2";
            f2.Description = "Description for Updated Factory 2";
            ent.Factories.Add(f1);
            ent.Factories.Add(f2);
            NamedObjectMaint mt = new NamedObjectMaint();
            Assert.IsTrue(mt.Update(ent));
        }

        [Test, Description("Delete enterprise with index property has value")]
        public void T80_Enterprise_Delete()
        {
            Enterprise ent = new Enterprise();
            ent.Name = "UO Soft Company";
            NamedObjectMaint mt = new NamedObjectMaint();
            Assert.IsTrue(mt.Remove(ent));
        }

        [Test, Description("Delete all remain factory which is left at above Enterprise remove")]
        public void T90_Factories_Delete()
        {
            Factory f1 = new Factory();
            f1.Name = "Factory 1";
            Factory f2 = new Factory();
            f2.Name = "Factory 2";
            Factory f2u = new Factory();
            f2u.Name = "Updated Factory 2";
            NamedObjectMaint mt = new NamedObjectMaint();
            Assert.IsTrue(mt.Remove(f1));
            Assert.IsTrue(mt.Remove(f2));
            Assert.IsTrue(mt.Remove(f2u));
        }
    }
}