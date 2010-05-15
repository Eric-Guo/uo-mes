using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Base;

namespace UO_Model.Workflow
{
    [DataContract]
    [Persistent(IdentityField = "workflowBase_id")]
    public class WorkflowBase : RevisionBase
    {
        private int workflowBase_id;
        [FieldAlias("workflowBase_id")]
        public int WorkflowBase_ID
        {
            get { return workflowBase_id; }
            set { workflowBase_id = value; }
        }

        private Workflow currentWorkflow;
        [DataMember]
        [FieldAlias("currentWorkflow")]
        override public RevisionedObject Current
        {
            get { return currentWorkflow; }
            set { currentWorkflow = value as Workflow; }
        }
    }
}
