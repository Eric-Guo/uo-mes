using System.Collections.Generic;
using Telerik.OpenAccess;
using UO_Model.Base;

namespace UO_Model.ServiceHistory
{
    [Persistent(IdentityField = "splitHistory_id")]
    public class SplitHistory : ServiceHistorySummary
    {
        private int splitHistory_id;
        [FieldAlias("splitHistory_id")]
        public int SplitHistory_ID
        {
            get { return splitHistory_id; }
            set { splitHistory_id = value; }
        }

        private bool closeWhenEmpty;
        [FieldAlias("closeWhenEmpty")]
        public bool CloseWhenEmpty
        {
            get { return closeWhenEmpty; }
            set { closeWhenEmpty = value; }
        }

        private IList<SplitHistoryDetail> splitHistoryDetails = new List<SplitHistoryDetail>();
        [FieldAlias("splitHistoryDetails")]
        public override IList<ServiceHistoryDetail> ServiceHistoryDetails
        {
            get { return splitHistoryDetails as IList<ServiceHistoryDetail>; }
        }
    }

    [Persistent(IdentityField = "splitHistoryDetail_id")]
    public class SplitHistoryDetail : ServiceHistoryDetail
    {
        private int splitHistoryDetail_id;
        [FieldAlias("splitHistoryDetail_id")]
        public int SplitHistoryDetail_ID
        {
            get { return splitHistoryDetail_id; }
            set { splitHistoryDetail_id = value; }
        }

        private SplitHistory splitHistory;
        [FieldAlias("splitHistory")]
        public override ServiceHistorySummary ServiceHistorySummary
        {
            get { return splitHistory; }
            set { splitHistory = value as SplitHistory; }
        }

        private string containerName;
        [FieldAlias("containerName")]
        public string ContainerName
        {
            get { return containerName; }
            set { containerName = value; }
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
