using System;
using System.Collections.Generic;
using System.Text;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Execution.Reason;

namespace UO_Model.ServiceHistory
{
    [Persistent(IdentityField = "defectHistory_id")]
    public class DefectHistory : ServiceHistorySummary
    {
        private int defectHistory_id;
        [FieldAlias("defectHistory_id")]
        public int DefectHistory_ID
        {
            get { return defectHistory_id; }
            set { defectHistory_id = value; }
        }

        private int qtyInspected;
        [FieldAlias("qtyInspected")]
        public int QtyInspected
        {
            get { return qtyInspected; }
            set { qtyInspected = value; }
        }

        private IList<DefectHistoryDetail> defectHistoryDetails = new List<DefectHistoryDetail>();
        [FieldAlias("defectHistoryDetails")]
        public override IList<ServiceHistoryDetail> ServiceHistoryDetails
        {
            get { return defectHistoryDetails as IList<ServiceHistoryDetail>; }
        }
    }

    [Persistent(IdentityField = "defectHistoryDetail_id")]
    public class DefectHistoryDetail : ServiceHistoryDetail
    {
        private int defectHistoryDetail_id;
        [FieldAlias("defectHistoryDetail_id")]
        public int DefectHistoryDetail_ID
        {
            get { return defectHistoryDetail_id; }
            set { defectHistoryDetail_id = value; }
        }

        private DefectHistory defectHistory;
        [FieldAlias("defectHistory")]
        public override ServiceHistorySummary ServiceHistorySummary
        {
            get { return defectHistory; }
            set { defectHistory = value as DefectHistory; }
        }

        private int defectQty;
        [FieldAlias("defectQty")]
        public int DefectQty
        {
            get { return defectQty; }
            set { defectQty = value; }
        }

        private DefectReason defectReason;
        [FieldAlias("defectReason")]
        public DefectReason DefectReason
        {
            get { return defectReason; }
            set { defectReason = value; }
        }

        private string comment;
        [FieldAlias("comment")]
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }
    }
}
