using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Base;

namespace UO_Model.Workflow
{
    [DataContract]
    [Persistent(IdentityField = "workflow_id")]
    public class Workflow : UO_Model.Base.RevisionedObject
    {
        private int workflow_id;
        [FieldAlias("workflow_id")]
        public int Workflow_ID
        {
            get { return workflow_id; }
            set { workflow_id = value; }
        }

        private WorkflowBase revBase;
        [DataMember]
        [FieldAlias("revBase")]
        override public RevisionBase RevBase
        {
            get
            {
                if (revBase == null)
                    revBase = new WorkflowBase();
                return revBase;
            }
            set { revBase = value as WorkflowBase; }
        }

        private IList<Step> steps = new List<Step>();
        [DataMember]
        [FieldAlias("steps")]
        public IList<Step> Steps
        {
            get { return steps; }
        }

        public Step FirstStep
        {
            get { return Steps.Count > 0 ? Steps[0] : null; }
        }
    }
}
