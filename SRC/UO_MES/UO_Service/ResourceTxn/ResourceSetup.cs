using Telerik.OpenAccess;
using UO_Model.Execution;
using UO_Model.ServiceHistory;
using UO_Model.Workflow;
using UO_Model.Base;
using UO_Model.Process;
using UO_Model.Physical.Reason;
using UO_Model.Physical.Status;

namespace UO_Service.ResourceTxn
{
    public class ResourceSetup : ResourceTxn
    {
        protected override string TxnName { get { return "ResourceSetup"; } }

        #region ResourceSetup transaction public properties for users
        private ResourceStatusReason resourceStatusReason;
        public ResourceStatusReason ResourceStatusReason
        {
            get { return resourceStatusReason; }
            set { resourceStatusReason = ResolveCDO(value.GetType(), value.Name) as ResourceStatusReason; }
        }
        public string ResourceStatusReason_Name
        {
            get { return resourceStatusReason.Name; }
            set { resourceStatusReason = ResolveCDO("ResourceStatusReason", value) as ResourceStatusReason; }
        }

        private ResourceStatus resourceToStatus;
        public ResourceStatus ResourceToStatus
        {
            get { return resourceToStatus; }
            set { resourceToStatus = ResolveCDO(value.GetType(), value.Name) as ResourceStatus; }
        }
        public string ResourceToStatus_Name
        {
            get { return resourceToStatus.Name; }
            set { resourceToStatus = ResolveCDO("ResourceStatus", value) as ResourceStatus; }
        }     
        #endregion

        protected override bool ModifyEntity()
        {
            bool success = base.ModifyEntity();
            ResourceProductionInfo.ResourceStatus = ResourceToStatus;
            return success;
        }

        protected override bool RecordServiceHistory()
        {
            ResourceStatusHistory h = new ResourceStatusHistory();
            AssignToResourceStatusHistory(h);
            ObjScope.Add(h);
            return true;
        }

        #region AssignTo
        protected virtual void AssignToResourceStatusHistory(ResourceStatusHistory t)
        {
            t.HistoryMainLine = this.HistoryMainLine;
            t.ResourceStatusReason = this.ResourceStatusReason;
            t.SetupDate = this.HistoryMainLine.TxnDate;
        }
        #endregion

        public ResourceSetup() : base() { }
        public ResourceSetup(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }
}