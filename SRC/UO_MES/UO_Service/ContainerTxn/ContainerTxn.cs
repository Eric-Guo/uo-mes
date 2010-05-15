using Telerik.OpenAccess;
using UO_Model.Execution;
using UO_Model.Execution.Status;
using UO_Model.Physical;
using UO_Model.ServiceHistory;
using UO_Service.Base;
using UO_Service.Resx;

namespace UO_Service.ContainerTxn
{
    abstract public class ContainerTxn : UO_Service.Base.ShopFloor
    {
        #region ShopFloor transaction public properties for users
        private Container container;
        public Container Container
        {
            get { return container; }
            set { container = ResolveContainer(value.ContainerName); }
        }
        public string Container_Name
        {
            get { return container.ContainerName; }
            set { container = ResolveContainer(value); }
        }

        public override Factory Factory
        {
            get
            {
                if (base.Factory == null && Container != null)
                    base.Factory = Container.CurrentStatus.Factory;
                return base.Factory;
            }
        }
        #endregion

        protected override bool ValidateService()
        {
            if (Container == null)
            {
                CompletionMessage = MSG.Container_Is_Need;
                return false;
            }
            if (Container.CurrentHoldCount > 0)
            {
                CompletionMessage = string.Format(MSG.Container_Is_Hold, Container.CurrentHoldCount);
                return false;
            }
            return true;
        }

        protected override bool ModifyEntity()
        {
            Container.LastActivityDate = HistoryMainLine.TxnDate;
            return true;
        }

        protected CurrentStatus ResolveCurrentStatus(CurrentStatus cs)
        {
            const string queryCurrentStatus =
    @"ELEMENT (SELECT * FROM CurrentStatusExtent AS o 
WHERE o.Workflow = $1 AND o.WorkflowStep = $2 AND o.CurrentStepPass = $3
  AND o.Spec = $4 AND o.InProcess = $5 AND o.Resource = $6 AND o.Factory = $7 )";
            object res = null;
            IQuery oqlQuery = ObjScope.GetOqlQuery(queryCurrentStatus);
            IQueryResult result = oqlQuery.Execute(cs.Workflow, cs.WorkflowStep, cs.CurrentStepPass,
                cs.Spec, cs.InProcess, cs.Resource, cs.Factory);
            if (result.Count > 0)
                res = result[0];
            result.Dispose();
            return res as CurrentStatus;
        }

        protected CurrentStatus GetCurrentStatusToModify()
        {
            CurrentStatus cs_new = new CurrentStatus();
            Container.CurrentStatus.AssignToCurrentStatus(cs_new);
            return cs_new;
        }

        protected void ReplaceCurrentStatus(CurrentStatus cs_new)
        {
            CurrentStatus cs_old = Container.CurrentStatus;
            CurrentStatus cs_exist = ResolveCurrentStatus(cs_new);
            if (cs_exist == null)
            {
                ObjScope.Add(cs_new);
                Container.CurrentStatus = cs_new;
            }
            else
                Container.CurrentStatus = cs_exist;
            if (cs_old.Containers.Count == 0)
                ObjScope.Remove(cs_old);
        }

        protected bool CloseContainerWhenEmpty(Container co)
        {
            if (co.Qty > 0)
                return false;
            co.ContainerStatus = ResolveCDO("ContainerStatus", "Closed") as ContainerStatus;
            return true;
        }

        #region AssignTo
        protected override bool AssignToHistoryMainLine(HistoryMainLine h)
        {
            bool sucess = base.AssignToHistoryMainLine(h);
            h.Container = this.Container;
            if (this.Container != null)
            {
                h.Operation = this.Container.CurrentStatus.Spec.Operation;
                h.Spec = this.Container.CurrentStatus.Spec;
                h.Product = this.Container.Product;
            }
            return sucess;
        }
        #endregion

        protected ContainerTxn() : base() { }
        protected ContainerTxn(IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }
}
