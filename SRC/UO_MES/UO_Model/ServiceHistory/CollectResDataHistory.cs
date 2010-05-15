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
    [Persistent(IdentityField = "collectResDataHistory_id")]
    public class CollectResDataHistory : ServiceHistorySummary
    {
        private int collectResDataHistory_id;
        [FieldAlias("collectResDataHistory_id")]
        public int CollectResDataHistory_ID
        {
            get { return collectResDataHistory_id; }
            set { collectResDataHistory_id = value; }
        }
       
        private DateTime collectDate;
        [FieldAlias("collectDate")]
        public DateTime CollectDate
        {
            get { return collectDate; }
            set { collectDate = value; }
        }

        private IList<CollectResDataHistoryDetail> collectResDataHistoryDetails = new List<CollectResDataHistoryDetail>();
        [FieldAlias("collectResDataHistoryDetails")]
        public override IList<ServiceHistoryDetail> ServiceHistoryDetails
        {
            get { return collectResDataHistoryDetails as IList<ServiceHistoryDetail>; }
        }
    }

    [Persistent(IdentityField = "collectResDataHistoryDetail_id")]
    public class CollectResDataHistoryDetail : ServiceHistoryDetail
    {
        private int collectResDataHistoryDetail_id;
        [FieldAlias("collectResDataHistoryDetail_id")]
        public int CollectResDataHistoryDetail_ID
        {
            get { return collectResDataHistoryDetail_id; }
            set { collectResDataHistoryDetail_id = value; }
        }

        private CollectResDataHistory collectResDataHistory;
        [FieldAlias("collectResDataHistory")]
        public override ServiceHistorySummary  ServiceHistorySummary
        {
            get { return collectResDataHistory; }
            set { collectResDataHistory = value as CollectResDataHistory; }
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
