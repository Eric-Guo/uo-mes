using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Physical
{
    [DataContract]
    [Persistent(IdentityField = "workCenter_id")]
    public class WorkCenter : UO_Model.Base.NamedObject
    {
        private int workCenter_id;
        [FieldAlias("workCenter_id")]
        public int WorkCenter_ID
        {
            get { return workCenter_id; }
            set { workCenter_id = value; }
        }

        private IList<ResourceGroup> resourceGroups = new List<ResourceGroup>();  // inverse ResourceGroup.workCenter
        [DataMember]
        [FieldAlias("resourceGroups")]
        public IList<ResourceGroup> ResourceGroups
        {
            get { return resourceGroups; }
        }
    }
}
