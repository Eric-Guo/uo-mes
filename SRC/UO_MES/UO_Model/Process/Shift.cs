using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Process
{
    [DataContract]
    [Persistent(IdentityField = "shift_id")]
    public class Shift : UO_Model.Base.NamedObject
    {
        private int shift_id;
        [DataMember]
        [FieldAlias("shift_id")]
        public int Shift_ID
        {
            get { return shift_id; }
            set { shift_id = value; }
        }
    }
}
