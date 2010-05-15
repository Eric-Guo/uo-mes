using System;
using System.Collections.Generic;
using UO_Model.Base;
using UO_Model.Execution;
using UO_Model.Execution.Reason;
using UO_Model.Execution.Status;
using UO_Model.Process;
using UO_Model.ServiceHistory;
using UO_Model.Workflow;
using UO_Service.Base;

namespace UO_Service.ContainerTxn
{
    public class Start : ContainerTxn
    {
        protected override string TxnName { get { return "Start"; } }

        #region Start transaction public properties for users
        private MfgOrder mfgOrder;
        public MfgOrder MfgOrder
        {
            get { return mfgOrder; }
            set { mfgOrder = ResolveCDO(value.GetType(), value.Name) as MfgOrder; }
        }
        public string MfgOrder_Name
        {
            get { return mfgOrder.Name; }
            set { mfgOrder = ResolveCDO("MfgOrder", value) as MfgOrder; }
        }

        private Workflow workflow;
        public Workflow Workflow
        {
            get { return workflow; }
            set { workflow = ResolveCDO(value.GetType(), value.DisplayName) as Workflow; }
        }
        public string Workflow_Revision
        {
            get { return workflow.DisplayName; }
            set { workflow = ResolveCDO("Workflow", value) as Workflow; }
        }

        private Step workflowStep;
        public Step WorkflowStep
        {
            get { return workflowStep; }
            set { workflowStep = ResolveCDO(value.GetType(), value.Name) as Step; }
        }
        public string WorkflowStep_Name
        {
            get { return workflowStep.DisplayName; }
            set { workflowStep = ResolveCDO("Step", value) as Step; }
        }

        private StartReason startReason;
        public StartReason StartReason
        {
            get { return startReason; }
            set { startReason = ResolveCDO(value.GetType(), value.Name) as StartReason; }
        }
        public string StartReason_Name
        {
            get { return startReason.DisplayName; }
            set { startReason = ResolveCDO("StartReason", value) as StartReason; }
        }
        #endregion

        private int detailIDCount = 0;
        private IList<StartDetail> details = new List<StartDetail>();
        private IList<StartDetail> Details
        {
            get { return details; }
        }
        #region Method to support ObjectDataSource for details
        public IList<StartDetail> SelectDetails()
        {
            return Details;
        }
        
        public void InsertDetail(StartDetail s)
        {
            s.ServiceDetailID = detailIDCount++;
            Details.Add(s);
        }

        public void DeleteDetail(StartDetail s)
        {
            int i = -1;
            foreach (StartDetail t in Details)
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    i = Details.IndexOf(t);
                    break;
                }
            if(-1 != i)
                Details.RemoveAt(i);
        }

        public void UpdateDetails(StartDetail s)
        {
            foreach (StartDetail t in Details)
            {
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    AssignStartDetailToStartDetail(s, t);
                }
            }
        }
        #endregion

        protected override bool ValidateService()
        {
            return true;
        }

        protected override bool ModifyEntity()
        {
            bool success = true;
            CurrentStatus cs_temp = new CurrentStatus();
            AssignToCurrentStatus(cs_temp);
            CurrentStatus cs = ResolveCurrentStatus(cs_temp);
            if (cs == null)
            {
                ObjScope.Add(cs_temp);
                cs = cs_temp;
            }
            foreach (StartDetail sd in Details)
            {
                Container co = new Container();
                co.CurrentStatus = cs;
                AssignToContainer(co);
                AssignStartDetailsToContainer(sd,co);
                ObjScope.Add(co);
                if (null != MfgOrder)
                    MfgOrder.ReleasedQty += co.Qty;
            }
            return success;
        }

        protected override bool RecordServiceHistory()
        {
            StartHistory h = new StartHistory();
            AssignToStartHistory(h);
            foreach (StartDetail sd in Details)
            {
                StartHistoryDetail hd = new StartHistoryDetail();
                AssignStartDetailToStartHistoryDetail(sd, hd);
                hd.ServiceHistorySummary = h;
                ObjScope.Add(hd);
            }
            ObjScope.Add(h);
            return true;
        }

        #region AssignTo
        protected virtual void AssignToContainer(Container t)
        {
            t.StartReason = this.StartReason;
            t.LastActivityDate = HistoryMainLine.TxnDate;
        }

        protected virtual void AssignToCurrentStatus(CurrentStatus t)
        {
            t.Workflow = this.Workflow;
            t.WorkflowStep = this.WorkflowStep;
            t.Factory = this.Factory;
            t.Spec = this.WorkflowStep.FirstSpecStep.Spec;
        }

        protected virtual void AssignStartDetailsToContainer(StartDetail s, Container t)
        {
            t.ContainerName = s.ContainerName;
            t.ContainerLevel = ResolveCDO("ContainerLevel", s.ContainerLevel_Name) as ContainerLevel;
            t.ContainerStatus = ResolveCDO("ContainerStatus", s.ContainerStatus_Name) as ContainerStatus;
            t.DueDate = s.DueDate;
            t.Qty = s.Qty;
            t.UOM = ResolveCDO("UOM", s.UOM_Name) as UOM;
            t.Product = ResolveCDO("Product", s.Product_Revision) as Product;
            if (t.Product != null)
            {
                if (t.Qty == 0)
                {
                    t.Qty = t.Product.StdStartedQty;
                    t.UOM = t.Product.StdStartedUOM;
                }
            }            
        }

        protected virtual void AssignStartDetailToStartDetail(StartDetail s, StartDetail t)
        {
            t.ContainerName = s.ContainerName;
            t.ContainerLevel_Name = s.ContainerLevel_Name;
            t.DueDate = s.DueDate;
            t.Product_Revision = s.Product_Revision;
            t.Qty = s.Qty;
            t.UOM_Name = s.UOM_Name;
        }

        protected virtual void AssignToStartHistory(StartHistory t)
        {
            t.HistoryMainLine = this.HistoryMainLine;
            t.MfgOrder = this.MfgOrder;
            t.Workflow = this.Workflow;
            t.WorkflowStep = this.WorkflowStep;
            t.StartReason = this.StartReason;
        }

        protected virtual void AssignStartDetailToStartHistoryDetail(StartDetail s, StartHistoryDetail t)
        {
            t.ContainerName = s.ContainerName;
            t.ContainerLevel = ResolveCDO("ContainerLevel", s.ContainerLevel_Name) as ContainerLevel;
            t.ContainerStatus = ResolveCDO("ContainerStatus", s.ContainerStatus_Name) as ContainerStatus;
            t.DueDate = s.DueDate;
            t.Qty = s.Qty;
            t.UOM = ResolveCDO("UOM", s.UOM_Name) as UOM;
            t.Product = ResolveCDO("Product", s.Product_Revision) as Product;
            if (t.Product != null)
            {
                if (t.Qty == 0)
                {
                    t.Qty = t.Product.StdStartedQty;
                    t.UOM = t.Product.StdStartedUOM;
                }
            }
        }
        #endregion

        public Start() : base() { }
        public Start(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }

    #region StartDetail's field must be all value type, no class type allowed to support ObjectDataSource
    public class StartDetail : ServiceDetail
    {
        private string containerName;
        public string ContainerName
        {
            get { return containerName; }
            set { containerName = value; }
        }

        private string containerLevel_Name;
        public string ContainerLevel_Name
        {
            get { return containerLevel_Name; }
            set { containerLevel_Name = value; }
        }

        private string containerStatus_Name;
        public string ContainerStatus_Name
        {
            get { return containerStatus_Name; }
            set { containerStatus_Name = value; }
        }

        public DateTime dueDate = UO_Model.Constants.NullDateTime;
        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        private string product_Revision;
        public string Product_Revision
        {
            get { return product_Revision; }
            set { product_Revision = value; }
        }

        private int qty;
        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        private string uom_Name;
        public string UOM_Name
        {
            get { return uom_Name; }
            set { uom_Name = value; }
        }

        private IList<StartDetail> childContainers = new List<StartDetail>();
        public IList<StartDetail> ChildContainers
        {
            get { return childContainers; }
        }
    }
    #endregion
}