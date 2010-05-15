using System;
using System.Text;
using System.Collections.Generic;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Execution;

namespace UO_Model.ServiceHistory
{
    [Persistent(IdentityField = "associateHistory_id")]
    public class AssociateHistory : ServiceHistorySummary
    {
        private int associateHistory_id;
        [FieldAlias("associateHistory_id")]
        public int AssociateHistory_ID
        {
            get { return associateHistory_id; }
            set { associateHistory_id = value; }
        }

        private IList<AssociateHistoryDetail> associateHistoryDetails = new List<AssociateHistoryDetail>();
        [FieldAlias("associateHistoryDetails")]
        public override IList<ServiceHistoryDetail> ServiceHistoryDetails
        {
            get { return associateHistoryDetails as IList<ServiceHistoryDetail>; }
        }
    }

    [Persistent(IdentityField = "associateHistoryDetail_id")]
    public class AssociateHistoryDetail : ServiceHistoryDetail
    {
        private int associateHistoryDetail_id;
        [FieldAlias("associateHistoryDetail_id")]
        public int AssociateHistoryDetail_ID
        {
            get { return associateHistoryDetail_id; }
            set { associateHistoryDetail_id = value; }
        }

        private AssociateHistory associateHistory;
        [FieldAlias("associateHistory")]
        public override ServiceHistorySummary ServiceHistorySummary
        {
            get { return associateHistory; }
            set { associateHistory = value as AssociateHistory; }
        }

        private Container childContainer;
        [FieldAlias("childContainer")]
        public Container ChildContainer
        {
            get { return childContainer; }
            set { childContainer = value; }
        }
    }
}
