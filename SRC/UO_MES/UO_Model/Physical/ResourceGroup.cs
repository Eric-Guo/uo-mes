using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Physical
{
    [DataContract]
    [Persistent(IdentityField = "resourceGroup_id")]
    public class ResourceGroup : UO_Model.Base.NamedObject
    {
        private int resourceGroup_id;
        [FieldAlias("resourceGroup_id")]
        public int ResourceGroup_ID
        {
            get { return resourceGroup_id; }
            set { resourceGroup_id = value; }
        }

        private WorkCenter workCenter;  // inverse WorkCenter.resourceGroups
        [DataMember]
        [FieldAlias("workCenter")]
        public WorkCenter WorkCenter
        {
            get { return workCenter; }
            set { workCenter = value; }
        }

        private IList<Resource> resources = new List<Resource>();  // link table
        [DataMember]
        [FieldAlias("resources")]
        public IList<Resource> Resources
        {
            get { return resources; }
        }
    }
}
