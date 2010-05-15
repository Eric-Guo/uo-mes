using System;
using System.Collections.Generic;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Process;

namespace UO_Model.ServiceHistory
{
    [Persistent(IdentityField = "moveHistory_id")]
    public class MoveHistory : ServiceHistorySummary
    {
        private int moveHistory_id;
        [FieldAlias("moveHistory_id")]
        public int MoveHistory_ID
        {
            get { return moveHistory_id; }
            set { moveHistory_id = value; }
        }

        private Workflow.Step toWorkflowStep;
        [FieldAlias("toWorkflowStep")]
        public Workflow.Step ToWorkflowStep
        {
            get { return toWorkflowStep; }
            set { toWorkflowStep = value; }
        }

        public override IList<ServiceHistoryDetail> ServiceHistoryDetails
        {
            get { return null; }
        }
    }
}
