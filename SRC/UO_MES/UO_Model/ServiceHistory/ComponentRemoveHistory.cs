using System;
using System.Collections.Generic;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Process;
using UO_Model.Execution;
using UO_Model.Process.Reason;

namespace UO_Model.ServiceHistory
{

    [Persistent(IdentityField = "componentRemoveHistory_id")]
    public class ComponentRemoveHistory : ServiceHistorySummary
    {
        private int componentRemoveHistory_id;
        [FieldAlias("componentRemoveHistory_id")]
        public int ComponentRemoveHistory_ID
        {
            get { return componentRemoveHistory_id; }
            set { componentRemoveHistory_id = value; }
        }


        private DateTime componentRemoveDate;
        [FieldAlias("componentRemoveDate")]
        public DateTime ComponentRemoveDate
        {
            get { return componentRemoveDate; }
            set { componentRemoveDate = value; }
        }
        private IList<ComponentRemoveHistoryDetail> componentRemoveHistoryDetails = new List<ComponentRemoveHistoryDetail>();
        [FieldAlias("componentRemoveHistoryDetails")]
        public override IList<ServiceHistoryDetail> ServiceHistoryDetails
        {
            get { return componentRemoveHistoryDetails as IList<ServiceHistoryDetail>; }
        }
    }

    [Persistent(IdentityField = "componentRemoveHistoryDetail_id")]
    public class ComponentRemoveHistoryDetail : ServiceHistoryDetail
    {
        private int componentRemoveHistoryDetail_id;
        [FieldAlias("componentRemoveHistoryDetail_id")]
        public int ComponentRemoveHistoryDetail_ID
        {
            get { return componentRemoveHistoryDetail_id; }
            set { componentRemoveHistoryDetail_id = value; }
        }

        private ComponentRemoveHistory componentRemoveHistory;
        [FieldAlias("componentRemoveHistory")]
        public override ServiceHistorySummary ServiceHistorySummary
        {
            get { return componentRemoveHistory; }
            set { componentRemoveHistory = value as ComponentRemoveHistory; }
        }


        private Product product = new Product();
        [FieldAlias("product")]
        public Product Product
        {
            get { return product; }
            set { product = value; }
        }
        
        private Container destinationContainer = new Container();
        [FieldAlias("destinationContainer")]
        public Container DestinationContainer
        {
            get { return destinationContainer; }
            set { destinationContainer = value; }
        }

        private int qtyRemoved;
        [FieldAlias("qtyRemoved")]
        public int QtyRemoved
        {
            get { return qtyRemoved; }
            set { qtyRemoved = value; }
        }

        private int issueControl;
        [FieldAlias("issueControl")]
        public int IssueControl
        {
            get { return issueControl; }
            set { issueControl = value; }
        }

        private RemoveReason removeReason;
        [FieldAlias("removeReason")]
        public RemoveReason RemoveReason
        {
            get { return removeReason; }
            set { removeReason = value; }
        }

        private int removeAllQty;
        [FieldAlias("removeAllQty")]
        public int RemoveAllQty
        {
            get { return removeAllQty; }
            set { removeAllQty = value; }
        }
    }
}
