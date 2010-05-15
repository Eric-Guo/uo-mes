using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using UO_Service.CompoundTxn;

namespace UO_UnitTest.BaseModel
{
    [TestFixture, Description("Test UO_Service.ResourceTxn")]
    public class T30_CompoundTxn
    {
        [Test]
        public void T10_MoveOut_WD01()
        {
            MoveOut s = new MoveOut();

            s.Container_Name = "LOT1-2";
            s.Resource_Name = "WD-01";
            s.ThruputAllQty = true;

            Assert.IsTrue(s.ExecuteService(), s.CompletionMessage);
        }        
    }
}
