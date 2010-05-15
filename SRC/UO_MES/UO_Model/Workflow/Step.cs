using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Workflow
{
    [DataContract]
    [Persistent(IdentityField = "workflowStep_id")]
    abstract public class Step : UO_Model.Base.NamedObject
    {
        private int workflowStep_id;
        [FieldAlias("workflowStep_id")]
        public int WorkflowStep_ID
        {
            get { return workflowStep_id; }
            set { workflowStep_id = value; }
        }

        private Path defaultPath;
        [DataMember]
        [FieldAlias("defaultPath")]
        public Path DefaultPath
        {
            get { return defaultPath; }
            set { defaultPath = value; }
        }

        abstract public SpecStep FirstSpecStep { get; }

        private int xLocation;
        [DataMember]
        [FieldAlias("xLocation")]
        public int XLocation
        {
            get { return xLocation; }
            set { xLocation = value; }
        }

        private int yLocation;
        [DataMember]
        [FieldAlias("yLocation")]
        public int YLocation
        {
            get { return yLocation; }
            set { yLocation = value; }
        }

        private Workflow workflow;
        [DataMember]
        [FieldAlias("workflow")]
        public Workflow Workflow
        {
            get { return workflow; }
            set { workflow = value; }
        }
    }
}
