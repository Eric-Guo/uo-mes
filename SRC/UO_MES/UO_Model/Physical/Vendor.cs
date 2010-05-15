using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Physical
{
    [DataContract]
    [Persistent(IdentityField = "vendor_id")]
    public class Vendor : UO_Model.Base.NamedObject
    {
        private int vendor_id;
        [FieldAlias("vendor_id")]
        public int Vendor_ID
        {
            get { return vendor_id; }
            set { vendor_id = value; }
        }
    }
}
