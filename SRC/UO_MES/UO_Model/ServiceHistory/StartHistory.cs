using System;
using System.Collections.Generic;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Process;
using UO_Model.Execution.Reason;
using UO_Model.Execution.Status;

namespace UO_Model.ServiceHistory
{
    [Persistent(IdentityField = "startHistory_id")]
    public class StartHistory : ServiceHistorySummary
    {
        private int startHistory_id;
        [FieldAlias("startHistory_id")]
        public int StartHistory_ID
        {
            get { return startHistory_id; }
            set { startHistory_id = value; }
        }

        private MfgOrder mfgOrder;
        [FieldAlias("mfgOrder")]
        public MfgOrder MfgOrder
        {
            get { return mfgOrder; }
            set { mfgOrder = value; }
        }

        private Workflow.Workflow workflow;
        [FieldAlias("workflow")]
        public Workflow.Workflow Workflow
        {
            get { return workflow; }
            set { workflow = value; }
        }

        private Workflow.Step workflowStep;
        [FieldAlias("workflowStep")]
        public Workflow.Step WorkflowStep
        {
            get { return workflowStep; }
            set { workflowStep = value; }
        }

        private StartReason startReason;
        [FieldAlias("startReason")]
        public StartReason StartReason
        {
            get { return startReason; }
            set { startReason = value; }
        }

        private IList<StartHistoryDetail> startHistoryDetails = new List<StartHistoryDetail>();
        [FieldAlias("startHistoryDetails")]
        public override IList<ServiceHistoryDetail> ServiceHistoryDetails
        {
            get { return startHistoryDetails as IList<ServiceHistoryDetail>; }
        }
    }

    [Persistent(IdentityField = "startHistoryDetail_id")]
    public class StartHistoryDetail : ServiceHistoryDetail
    {
        private int startHistoryDetail_id;
        [FieldAlias("startHistoryDetail_id")]
        public int StartHistoryDetail_ID
        {
            get { return startHistoryDetail_id; }
            set { startHistoryDetail_id = value; }
        }

        private StartHistory startHistory;
        [FieldAlias("startHistory")]
        public override ServiceHistorySummary ServiceHistorySummary
        {
            get { return startHistory; }
            set { startHistory = value as StartHistory; }
        }

        private string containerName;
        [FieldAlias("containerName")]
        public string ContainerName
        {
            get { return containerName; }
            set { containerName = value; }
        }

        private ContainerLevel containerLevel;
        [FieldAlias("containerLevel")]
        public ContainerLevel ContainerLevel
        {
            get { return containerLevel; }
            set { containerLevel = value; }
        }

        private ContainerStatus containerStatus;
        [FieldAlias("containerStatus")]
        public ContainerStatus ContainerStatus
        {
            get { return containerStatus; }
            set { containerStatus = value; }
        }

        private DateTime dueDate = Constants.NullDateTime;
        [FieldAlias("dueDate")]
        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        private Product product;
        [FieldAlias("product")]
        public Product Product
        {
            get { return product; }
            set { product = value; }
        }

        private int qty;
        [FieldAlias("qty")]
        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        private UOM uom;
        [FieldAlias("uom")]
        public UOM UOM
        {
            get { return uom; }
            set { uom = value; }
        }
    }
}
