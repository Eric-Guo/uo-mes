using System;
using System.Collections.Generic;
using Telerik.OpenAccess;
using System.Text;
using UO_Model.Process;
using UO_Model.Base;

namespace UO_Model.ServiceHistory
{
    [Persistent(IdentityField = "serializeHistory_id")]
    public class SerializeHistory : ServiceHistorySummary
    {
        private int serializeHistory_id;
        [FieldAlias("serializeHistory_id")]
        public int SerializeHistory_ID
        {
            get { return serializeHistory_id; }
            set { serializeHistory_id = value; }
        }

        private IList<SerializeHistoryDetail> serializeHistoryDetails = new List<SerializeHistoryDetail>();
        [FieldAlias("serializeHistoryDetails")]
        public override IList<ServiceHistoryDetail> ServiceHistoryDetails
        {
            get { return serializeHistoryDetails as IList<ServiceHistoryDetail>; }
        }

        private ContainerLevel childContainerLevel;
        [FieldAlias("childContainerLevel")]
        public ContainerLevel ChildContainerLevel
        {
            get { return childContainerLevel; }
            set { childContainerLevel = value; }
        }

        private UOM childUOM;
        [FieldAlias("childUOM")]
        public UOM ChildUOM
        {
            get { return childUOM; }
            set { childUOM = value; }
        }

        private Product product;
        [FieldAlias("product")]
        public Product Product
        {
            get { return product; }
            set { product = value; }
        }
    }

    [Persistent(IdentityField = "serializeHistoryDetail_id")]
    public class SerializeHistoryDetail : ServiceHistoryDetail
    {
        private int serializeHistoryDetail_id;
        [FieldAlias("serializeHistoryDetail_id")]
        public int SerializeHistoryDetail_ID
        {
            get { return serializeHistoryDetail_id; }
            set { serializeHistoryDetail_id = value; }
        }

        private SerializeHistory serializeHistory;
        [FieldAlias("serializeHistory")]
        public override ServiceHistorySummary ServiceHistorySummary
        {
            get { return serializeHistory; }
            set { serializeHistory = value as SerializeHistory; }
        }

        private string childContainerName;
        [FieldAlias("childContainerName")]
        public string ChildContainerName
        {
            get { return childContainerName; }
            set { childContainerName = value; }
        }

        private int childQty;
        [FieldAlias("childQty")]
        public int ChildQty
        {
            get { return childQty; }
            set { childQty = value; }
        }
    }
}
