using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Physical
{
    [DataContract]
    [Persistent(IdentityField = "factory_id")]
    public class Factory : UO_Model.Base.NamedObject
    {
        private int factory_id;
        [FieldAlias("factory_id")]
        public int Factory_ID
        {
            get { return factory_id; }
            set { factory_id = value; }
        }

        private Enterprise enterprise;  // inverse Enterprise.factories
        [DataMember]
        [FieldAlias("enterprise")]
        public Enterprise Enterprise
        {
            get { return enterprise; }
            set { enterprise = value; }
        }

        private IList<Resource> resources = new List<Resource>();
        [DataMember]
        [FieldAlias("resources")]
        public IList<Resource> Resources
        {
            get { return resources; }
        }        
    }
}
