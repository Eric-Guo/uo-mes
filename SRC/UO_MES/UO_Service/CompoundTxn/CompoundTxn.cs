using System;
using System.Collections.Generic;
using System.Text;

namespace UO_Service.CompoundTxn
{
    /// <summary>
    /// Compound transactions will allow the combiniation of multiple services 
    /// that would normally execute alone into a single transaction that will have 
    /// a single commit/rollback cycle. 
    /// Services in a compound transaction will therefore depend on each other - if one fails they all do. 
    /// </summary>
    abstract public class CompoundTxn : UO_Service.Base.ShopFloor
    {
        protected CompoundTxn() : base() { }
    }
}
