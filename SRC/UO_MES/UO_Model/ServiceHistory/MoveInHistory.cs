using System;
using System.Collections.Generic;
using Telerik.OpenAccess;
using UO_Model.Base;

namespace UO_Model.ServiceHistory
{
    [Persistent(IdentityField = "moveInHistory_id")]
    public class MoveInHistory : ServiceHistorySummary
    {
        private int moveInHistory_id;
        [FieldAlias("moveInHistory_id")]
        public int MoveInHistory_ID
        {
            get { return moveInHistory_id; }
            set { moveInHistory_id = value; }
        }

        public override IList<ServiceHistoryDetail> ServiceHistoryDetails
        {
            get { return null; }
        }
    }
}
