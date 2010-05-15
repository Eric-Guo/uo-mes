using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Base;

namespace UO_Model.Process
{
    [DataContract]
    [Persistent(IdentityField = "specBase_id")]
    public class SpecBase : RevisionBase
    {
        private int specBase_id;
        [FieldAlias("specBase_id")]
        public int SpecBase_ID
        {
            get { return specBase_id; }
            set { specBase_id = value; }
        }

        private Spec currentSpec;
        [DataMember]
        [FieldAlias("currentSpec")]
        override public RevisionedObject Current
        {
            get { return currentSpec; }
            set { currentSpec = value as Spec; }
        }
    }
}
