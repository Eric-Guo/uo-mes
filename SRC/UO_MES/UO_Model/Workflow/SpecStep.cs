using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Process;

namespace UO_Model.Workflow
{
    [DataContract]
    [Persistent]
    public class SpecStep:UO_Model.Workflow.Step
    {
        private Spec spec;
        [FieldAlias("spec")]
        public Spec Spec
        {
            get { return spec; }
            set { spec = value; }
        }

        public override SpecStep FirstSpecStep
        {
            get { return this; }
        }
    }
}
