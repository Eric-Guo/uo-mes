using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Physical
{
    [DataContract]
    [Persistent(IdentityField = "customer_id")]
    public class Customer:UO_Model.Base.NamedObject
    {
        private int customer_id;
        [FieldAlias("customer_id")]
        public int Customer_ID
        {
            get { return customer_id; }
            set { customer_id = value; }
        }

        private string company;
        [DataMember]
        [FieldAlias("company")]
        public string Company
        {
            get { return company; }
            set { company = value; }
        }

        private string address;
        [DataMember]
        [FieldAlias("address")]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        private string tel;
        [DataMember]
        [FieldAlias("tel")]
        public string Tel
        {
            get { return tel; }
            set { tel = value; }
        }

        private string fax;
        [DataMember]
        [FieldAlias("fax")]
        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }
    }
}
