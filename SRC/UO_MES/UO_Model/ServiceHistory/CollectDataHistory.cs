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
    [Persistent(IdentityField = "collectDataHistory_id")]
    public class CollectDataHistory : ServiceHistorySummary
    {
        private int collectDataHistory_id;
        [FieldAlias("collectDataHistory_id")]
        public int CollectDataHistory_ID
        {
            get { return collectDataHistory_id; }
            set { collectDataHistory_id = value; }
        }

        private DateTime collectDate;
        [FieldAlias("collectDate")]
        public DateTime CollectDate
        {
            get { return collectDate; }
            set { collectDate = value; }
        }

        private IList<CollectDataHistoryDetail> collectDataHistoryDetails = new List<CollectDataHistoryDetail>();
        [FieldAlias("collectDataHistoryDetails")]
        public override IList<ServiceHistoryDetail> ServiceHistoryDetails
        {
            get { return collectDataHistoryDetails as IList<ServiceHistoryDetail>; }
        }
    }

    [Persistent(IdentityField = "collectDataHistoryDetail_id")]
    public class CollectDataHistoryDetail : ServiceHistoryDetail
    {
        private int collectDataHistoryDetail_id;
        [FieldAlias("collectDataHistoryDetail_id")]
        public int CollectDataHistoryDetail_ID
        {
            get { return collectDataHistoryDetail_id; }
            set { collectDataHistoryDetail_id = value; }
        }

        private CollectDataHistory collectDataHistory;
        [FieldAlias("collectDataHistory")]
        public override ServiceHistorySummary ServiceHistorySummary
        {
            get { return collectDataHistory; }
            set { collectDataHistory = value as CollectDataHistory; }
        }

        private DataCollectionDef dataCollectionDef;
        [FieldAlias("dataCollectionDef")]
        public DataCollectionDef DataCollectionDef
        {
            get { return dataCollectionDef; }
            set { dataCollectionDef = value; }
        }

        private string dataCollectionValue;
        [FieldAlias("dataCollectionValue")]
        public string DataCollectionValue
        {
            get { return dataCollectionValue; }
            set { dataCollectionValue = value; }
        }

    }
}
