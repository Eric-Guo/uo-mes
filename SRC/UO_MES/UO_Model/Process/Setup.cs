using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Process.Code;
using UO_Model.Physical;

namespace UO_Model.Process
{
    [DataContract]
    [Persistent(IdentityField = "setup_id")]
    public class Setup : RevisionedObject
    {
        private int setup_id;
        [FieldAlias("setup_id")]
        public int Setup_ID
        {
            get { return setup_id; }
            set { setup_id = value; }
        }

        private SetupBase revBase;
        [DataMember]
        [FieldAlias("revBase")]
        override public RevisionBase RevBase
        {
            get
            {
                if (revBase == null)
                    revBase = new SetupBase();
                return revBase;
            }
            set { revBase = value as SetupBase; }
        }
    
        private ResourceGroup resourceGroup;
        [DataMember]
        [FieldAlias("resourceGroup")]
        public ResourceGroup ResourceGroup
        {
            get { return resourceGroup; }
            set { resourceGroup = value; }
        }

        private int status;
        [DataMember]
        [FieldAlias("status")]
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        private DateTime setupDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("setupDate")]
        public DateTime SetupDate
        {
            get { return setupDate; }
            set { setupDate = value; }
        }
    }
}
