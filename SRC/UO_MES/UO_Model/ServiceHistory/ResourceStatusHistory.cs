using System;
using System.Collections.Generic;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Process;
using UO_Model.Physical.Reason;
using UO_Model.Physical.Status;
using UO_Model.Execution;

namespace UO_Model.ServiceHistory
{
    [Persistent(IdentityField = "resourceStatusHistory_id")]
    public class ResourceStatusHistory : ServiceHistorySummary
    {
        private int resourceStatusHistory_id;
        [FieldAlias("resourceStatusHistory_id")]
        public int ResourceStatusHistory_ID
        {
            get { return resourceStatusHistory_id; }
            set { resourceStatusHistory_id = value; }
        }

        private ResourceStatusReason resourceStatusReason;
        [FieldAlias("resourceStatusReason")]
        public ResourceStatusReason ResourceStatusReason
        {
            get { return resourceStatusReason; }
            set { resourceStatusReason = value; }
        }

        private DateTime setupDate;
        [FieldAlias("setupDate")]
        public DateTime SetupDate
        {
            get { return setupDate; }
            set { setupDate = value; }
        }

        public override IList<ServiceHistoryDetail> ServiceHistoryDetails
        {
            get { return null; }
        }
    }
}
