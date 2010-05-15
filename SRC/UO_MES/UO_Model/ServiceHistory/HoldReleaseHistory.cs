using System;
using System.Collections.Generic;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Process;
using UO_Model.Execution.Reason;
using UO_Model.Execution;

namespace UO_Model.ServiceHistory
{
    [Persistent(IdentityField = "holdReleaseHistory_id")]
    public class HoldReleaseHistory : ServiceHistorySummary
    {
        private int holdReleaseHistory_id;
        [FieldAlias("holdReleaseHistory_id")]
        public int HoldReleaseHistory_ID
        {
            get { return holdReleaseHistory_id; }
            set { holdReleaseHistory_id = value; }
        }

        private HoldReason holdReason;
        [FieldAlias("holdReason")]
        public HoldReason HoldReason
        {
            get { return holdReason; }
            set { holdReason = value; }
        }

        private DateTime holdTime = Constants.NullDateTime;
        [FieldAlias("holdTime")]
        public DateTime HoldTime
        {
            get { return holdTime; }
            set { holdTime = value; }
        }

        private ReleaseReason releaseReason;
        [FieldAlias("releaseReason")]
        public ReleaseReason RelaseReason
        {
            get { return releaseReason; }
            set { releaseReason = value; }
        }

        private DateTime releaseTime = Constants.NullDateTime;
        [FieldAlias("releaseTime")]
        public DateTime ReleaseTime
        {
            get { return releaseTime; }
            set { releaseTime = value; }
        }

        private HoldReleaseHistory releaseHistory;
        [FieldAlias("releaseHistory")]
        public HoldReleaseHistory ReleaseHistory
        {
            get { return releaseHistory; }
            set { releaseHistory = value; }
        }

        public override IList<ServiceHistoryDetail> ServiceHistoryDetails
        {
            get { return null; }
        }
    }
}
