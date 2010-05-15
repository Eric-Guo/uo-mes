using UO_Model.Execution;
using UO_Model.ServiceHistory;
using UO_Model.Workflow;

namespace UO_Service.ContainerTxn
{
    public class MoveTxn : ContainerTxn
    {
        protected override string TxnName { get { return "MoveTxn"; } }

        #region Move transaction public properties for users
        private Step toWorkflowStep;
        public Step ToWorkflowStep
        {
            get { return toWorkflowStep; }
            set { toWorkflowStep = ResolveCDO(value.GetType(), value.Name) as Step; }
        }
        #endregion

        protected override bool ModifyEntity()
        {
            bool success = base.ModifyEntity();

            CurrentStatus cs = GetCurrentStatusToModify();
            Workflow cs_wf = cs.Workflow;
            Step nextStep = this.ToWorkflowStep;
            if (nextStep == null)
            {
                int currentStepIndex = cs_wf.Steps.IndexOf(cs.WorkflowStep);
                currentStepIndex++;
                if (currentStepIndex < cs_wf.Steps.Count)
                    nextStep = cs_wf.Steps[currentStepIndex];
            }
            cs.WorkflowStep = nextStep;
            ReplaceCurrentStatus(cs);

            this.Container.LastMoveDate = this.HistoryMainLine.TxnDate;
            this.Container.CurrentThruputQty = 0;
            return success;
        }

        protected override bool RecordServiceHistory()
        {
            MoveHistory h = new MoveHistory();
            AssignToMoveHistory(h);
            ObjScope.Add(h);
            return true;
        }

        #region AssignTo
        protected virtual void AssignToMoveHistory(MoveHistory t)
        {
            t.HistoryMainLine = this.HistoryMainLine;
            t.ToWorkflowStep = this.ToWorkflowStep;
        }
        #endregion

        public MoveTxn() : base() { }
        public MoveTxn(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }
}
