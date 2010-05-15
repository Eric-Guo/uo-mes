using System;
using System.Collections.Generic;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Process;
using UO_Model.Execution;
using UO_Model.Process.Reason;

namespace UO_Model.ServiceHistory
{
    [Persistent(IdentityField = "componentIssueHistory_id")]
    public class ComponentIssueHistory : ServiceHistorySummary
    {
        private int componentIssueHistory_id;
        [FieldAlias("componentIssueHistory_id")]
        public int ComponentIssueHistory_ID
        {
            get { return componentIssueHistory_id; }
            set { componentIssueHistory_id = value; }
        }

        private DateTime componentIssueDate;
        [FieldAlias("componentIssueDate")]
        public DateTime ComponentIssueDate
        {
            get { return componentIssueDate; }
            set { componentIssueDate = value; }
        }

        private IList<ComponentIssueHistoryDetail> componentIssueHistoryDetails = new List<ComponentIssueHistoryDetail>();
        [FieldAlias("componentIssueHistoryDetails")]
        public override IList<ServiceHistoryDetail> ServiceHistoryDetails
        {
            get { return componentIssueHistoryDetails as IList<ServiceHistoryDetail>; }
        }
    }

    [Persistent(IdentityField = "componentIssueHistoryDetail_id")]
    public class ComponentIssueHistoryDetail : ServiceHistoryDetail
    {
        private int componentIssueHistoryDetail_id;
        [FieldAlias("componentIssueHistoryDetail_id")]
        public int ComponentIssueHistoryDetail_ID
        {
            get { return componentIssueHistoryDetail_id; }
            set { componentIssueHistoryDetail_id = value; }
        }

        private ComponentIssueHistory componentIssueHistory;
        [FieldAlias("componentIssueHistory")]
        public override ServiceHistorySummary ServiceHistorySummary
        {
            get { return componentIssueHistory; }
            set { componentIssueHistory = value as ComponentIssueHistory; }
        }


        private Product product = new Product();
        [FieldAlias("product")]
        public Product Product
        {
            get { return product; }
            set { product = value; }
        }

        private int actualQtyIssued;
        [FieldAlias("actualQtyIssued")]
        public int ActualQtyIssued
        {
            get { return actualQtyIssued; }
            set { actualQtyIssued = value; }
        }

        private int qtyRequired;
        [FieldAlias("qtyRequired")]
        public int QtyRequired
        {
            get { return qtyRequired; }
            set { qtyRequired = value; }
        }

        private int issueControl;
        [FieldAlias("issueControl")]
        public int IssueControl
        {
            get { return issueControl; }
            set { issueControl = value; }
        }

        private IssueReason issueReason;
        [FieldAlias("issueReason")]
        public IssueReason IssueReason
        {
            get { return issueReason; }
            set { issueReason = value; }
        }

        private Container container;
        [FieldAlias("container")]
        public Container Container
        {
            get { return container; }
            set { container = value; }
        }


        private string fromLocation;
        [FieldAlias("fromLocation")]
        public string FromLocation
        {
            get { return fromLocation; }
            set { fromLocation = value; }
        }

        private string toLocation;
        [FieldAlias("toLocation")]
        public string ToLocation
        {
            get { return toLocation; }
            set { toLocation = value; }
        }
    }
}
