using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Telerik.OpenAccess;
using UO_Model.Physical.Status;
using UO_Model.Process;
using UO_Model.Base;

namespace UO_Model.Execution
{
    [DataContract]
    [Persistent(IdentityField = "resourceProductionInfo_id")]
    public class ResourceProductionInfo
    {
        private int resourceProductionInfo_id;
        [FieldAlias("resourceProductionInfo_id")]
        public int ResourceProductionInfo_ID
        {
            get { return resourceProductionInfo_id; }
            set { resourceProductionInfo_id = value; }
        }

        private ResourceStatus resourceStatus;
        [DataMember]
        [FieldAlias("resourceStatus")]
        public ResourceStatus ResourceStatus
        {
            get { return resourceStatus; }
            set { resourceStatus = value; }
        }

        private Setup setup;
        [DataMember]
        [FieldAlias("setup")]
        public Setup Setup
        {
            get { return setup; }
            set { setup = value; }
        }

        private DateTime lastStatusChangeDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("lastStatusChangeDate")]
        public DateTime LastStatusChangeDate
        {
            get { return lastThruputDate; }
            set { lastThruputDate = value; }
        }

        private DateTime lastThruputDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("lastThruputDate")]
        public DateTime LastThruputDate
        {
            get { return lastThruputDate; }
            set { lastThruputDate = value; }
        }
    }
}
