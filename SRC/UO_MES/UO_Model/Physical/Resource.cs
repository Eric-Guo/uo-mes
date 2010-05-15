using System;
using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Physical.Status;
using UO_Model.Process;
using UO_Model.Execution;
using UO_Model.Base;

namespace UO_Model.Physical
{
    [DataContract]
    [Persistent(IdentityField = "resource_id")]
    public class Resource : UO_Model.Base.NamedObject
    {
        private int resource_id;
        [FieldAlias("resource_id")]
        public int Resource_ID
        {
            get { return resource_id; }
            set { resource_id = value; }
        }

        private Factory factory;  // inverse Factory.resources
        [DataMember]
        [FieldAlias("factory")]
        public Factory Factory
        {
            get { return factory; }
            set { factory = value; }
        }

        private DateTime lastActivityDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("lastActivityDate")]
        public DateTime LastActivityDate
        {
            get { return lastActivityDate; }
            set { lastActivityDate = value; }
        }

        private ResourceProductionInfo resourceProductionInfo;
        [DataMember]
        [FieldAlias("resourceProductionInfo")]
        public ResourceProductionInfo ResourceProductionInfo
        {
            get { return resourceProductionInfo; }
            set { resourceProductionInfo = value; }
        }
    }
}
