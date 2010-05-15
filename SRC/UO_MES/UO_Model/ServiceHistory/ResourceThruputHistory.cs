using System;
using System.Collections.Generic;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Process;
using UO_Model.Physical;
using UO_Model.Physical.Reason;
using UO_Model.Physical.Status;
using UO_Model.Execution;

namespace UO_Model.ServiceHistory
{
    [Persistent(IdentityField = "resourceThruputHistory_id")]
    public class ResourceThruputHistory : ServiceHistorySummary
    {
        private int resourceThruputHistory_id;
        [FieldAlias("resourceThruputHistory_id")]
        public int ResourceThruputHistory_ID
        {
            get { return resourceThruputHistory_id; }
            set { resourceThruputHistory_id = value; }
        }

        private Setup setup;
        [FieldAlias("setup")]
        public Setup Setup
        {
            get { return setup; }
            set { setup = value; }
        }

        private int run;
        [FieldAlias("run")]
        public int Run
        {
            get { return run; }
            set { run = value; }
        }

        private MfgOrder mfgOrder;
        [FieldAlias("mfgOrder")]
        public MfgOrder MfgOrder
        {
            get { return mfgOrder; }
            set { mfgOrder = value; }
        }

        private Product product;
        [FieldAlias("product")]
        public Product Product
        {
            get { return product; }
            set { product = value; }
        }

        private int qty;
        [FieldAlias("qty")]
        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        private UOM uom;
        [FieldAlias("uom")]
        public UOM UOM
        {
            get { return uom; }
            set { uom = value; }
        }

        private DateTime thruputDate;
        [FieldAlias("thruputDate")]
        public DateTime ThruputDate
        {
            get { return thruputDate; }
            set { thruputDate = value; }
        }

        public override IList<ServiceHistoryDetail> ServiceHistoryDetails
        {
            get { return null; }
        }
    }
}
