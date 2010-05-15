using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using UO_Service.Base;
using UO_Model.Process;

namespace UO_UnitTest.Maint
{
    [TestFixture, Description("Test UO_Service.RevisionedObjectMaint basic Add and Update.")]
    public class RevisionedObjectMaint_Test
    {
        [Test, Description("Basic add without index property")]
        public void T10_ProductT_r0_Add()
        {
            string product_name = "ProductT";
            string product_revision = "r0";
            AddProduct(product_name, product_revision);
        }

        [Test, Description("Basic add in exist base")]
        public void T11_ProductT_r1_Add()
        {
            string product_name = "ProductT";
            string product_revision = "r1";
            AddProduct(product_name, product_revision);
        }

        [Test, Description("Basic add in exist base")]
        public void T20_ProductT_r1_Update()
        {
            Product pt = new Product();
            pt.RevBase.Name = "ProductT";
            pt.Revision = "r1";
            pt.Description = "ProdcutT(r1) description updated at " + DateTime.Now.ToString();
            RevisionedObjectMaint mt = new RevisionedObjectMaint();
            Assert.IsTrue(mt.Update(pt));
        }

        [Test, Description("Basic delete in exist base")]
        public void T30_ProductT_r0_Delete()
        {
            Product pt = new Product();
            pt.RevBase.Name = "ProductT";
            pt.Revision = "r0";
            RevisionedObjectMaint mt = new RevisionedObjectMaint();
            Assert.IsTrue(mt.Remove(pt));
        }

        [Test, Description("Basic delete in exist base")]
        public void T40_ProductT_r1_Delete()
        {
            Product pt = new Product();
            pt.RevBase.Name = "ProductT";
            pt.Revision = "r1";
            RevisionedObjectMaint mt = new RevisionedObjectMaint();
            Assert.IsTrue(mt.Remove(pt));
        }

        private static void AddProduct(string product_name, string product_revision)
        {
            Product pt = new Product();
            ProductBase pb = new ProductBase();
            pt.RevBase = pb;
            pt.RevBase.Name = product_name;
            pt.Revision = product_revision;
            pt.Description = DateTime.Now.ToString();
            RevisionedObjectMaint mt = new RevisionedObjectMaint();
            Assert.IsTrue(mt.Add(pt));
        }
    }
}
