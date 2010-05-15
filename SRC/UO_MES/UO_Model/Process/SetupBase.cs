using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Base;

namespace UO_Model.Process
{
    [DataContract]
    [Persistent(IdentityField = "setupBase_id")]
    public class SetupBase : RevisionBase
    {
        private int setupBase_id;
        [FieldAlias("setupBase_id")]
        public int SetupBase_ID
        {
            get { return setupBase_id; }
            set { setupBase_id = value; }
        }

        private Setup currentSetup;
        [DataMember]
        [FieldAlias("currentSetup")]
        override public RevisionedObject Current
        {
            get { return currentSetup; }
            set { currentSetup = value as Setup; }
        }
    }
}
