using System.Runtime.Serialization;
using UO_Model.Base;
using Telerik.OpenAccess;

namespace UO_Model.Process
{
    [DataContract]
    [Persistent(IdentityField = "uom_id")]
    public class UOM : NamedObject
    {
        private int uom_id;
        [FieldAlias("uom_id")]
        public int UOM_ID
        {
            get { return uom_id; }
            set { uom_id = value; }
        }
    }
}
