using System.Collections.Generic;
using Telerik.OpenAccess;
using UO_Model.Base;

namespace UO_Model.ServiceHistory
{
    [Persistent(IdentityField = "combineHistory_id")]
    public class CombineHistory : ServiceHistorySummary
    {
        private int combineHistory_id;
        [FieldAlias("combineHistory_id")]
        public int CombineHistory_ID
        {
            get { return combineHistory_id; }
            set { combineHistory_id = value; }
        }

        private bool closeWhenEmpty;
        [FieldAlias("closeWhenEmpty")]
        public bool CloseWhenEmpty
        {
            get { return closeWhenEmpty; }
            set { closeWhenEmpty = value; }
        }

        private IList<CombineHistoryDetail> combineHistoryDetails = new List<CombineHistoryDetail>();
        [FieldAlias("combineHistoryDetails")]
        public override IList<ServiceHistoryDetail> ServiceHistoryDetails
        {
            get { return combineHistoryDetails as IList<ServiceHistoryDetail>; }
        }
    }

    [Persistent(IdentityField = "combineHistoryDetail_id")]
    public class CombineHistoryDetail : ServiceHistoryDetail
    {
        private int combineHistoryDetail_id;
        [FieldAlias("combineHistoryDetail_id")]
        public int CombineHistoryDetail_ID
        {
            get { return combineHistoryDetail_id; }
            set { combineHistoryDetail_id = value; }
        }

        private CombineHistory combineHistory;
        [FieldAlias("combineHistory")]
        public override ServiceHistorySummary ServiceHistorySummary
        {
            get { return combineHistory; }
            set { combineHistory = value as CombineHistory; }
        }

        private bool closeWhenEmpty;
        [FieldAlias("closeWhenEmpty")]
        public bool CloseWhenEmpty
        {
            get { return closeWhenEmpty; }
            set { closeWhenEmpty = value; }
        }

        private bool combineAllQty;
        [FieldAlias("combineAllQty")]
        public bool CombineAllQty
        {
            get { return combineAllQty; }
            set { combineAllQty = value; }
        }

        private string fromContainerName;
        [FieldAlias("fromContainerName")]
        public string FromContainerName
        {
            get { return fromContainerName; }
            set { fromContainerName = value; }
        }

        private int qty;
        [FieldAlias("qty")]
        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }
    }
}
