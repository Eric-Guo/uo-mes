using System;
using System.Collections.Generic;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Process;
using UO_Model.Execution.Reason;
using UO_Model.Execution;
using UO_Model.Physical;

namespace UO_Model.ServiceHistory
{
    [Persistent(IdentityField = "thruputHistory_id")]
    public class ThruputHistory : ServiceHistorySummary
    {
        private int thruputHistory_id;
        [FieldAlias("thruputHistory_id")]
        public int ThruputHistory_ID
        {
            get { return thruputHistory_id; }
            set { thruputHistory_id = value; }
        }

        private Container container;
        [FieldAlias("container")]
        public Container Container
        {
            get { return container; }
            set { container = value; }
        }

        private Resource resource;
        [FieldAlias("resource")]
        public Resource Resource
        {
            get { return resource; }
            set { resource = value; }
        }

        private bool thruputAllQty;
        [FieldAlias("thruputAllQty")]
        public bool ThruputAllQty
        {
            get { return thruputAllQty; }
            set { thruputAllQty = value; }
        }

        private int qty;
        [FieldAlias("qty")]
        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        private double cycleTime;
        /// <summary>
        /// Equal to Thruput.TxnDate - Container.LastMoveDate
        /// </summary>
        [FieldAlias("cycleTime")]
        public double CycleTime
        {
            get { return cycleTime; }
            set { cycleTime = value; }
        }

        public override IList<ServiceHistoryDetail> ServiceHistoryDetails
        {
            get { return null; }
        }
    }
}
