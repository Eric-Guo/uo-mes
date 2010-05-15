using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Workflow
{
    [DataContract]
    [Persistent(IdentityField = "path_id")]
    public class Path
    {
        private int path_id;
        [FieldAlias("path_id")]
        public int Path_ID
        {
            get { return path_id; }
            set { path_id = value; }
        }

        private Step fromStep;
        [DataMember]
        [FieldAlias("fromStep")]
        public Step FromStep
        {
            get { return fromStep; }
            set { fromStep = value; }
        }

        private Step toStep;
        [DataMember]
        [FieldAlias("toStep")]
        public Step ToStep
        {
            get { return toStep; }
            set { toStep = value; }
        }
    }
}
