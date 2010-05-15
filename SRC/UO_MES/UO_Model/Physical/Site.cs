using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Physical
{
    [DataContract]
    [Persistent(IdentityField = "site_id")]
    public class Site : UO_Model.Base.NamedObject
    {
        private int site_id;
        [FieldAlias("site_id")]
        public int Site_ID
        {
            get { return site_id; }
            set { site_id = value; }
        }

        //public DataTransport defaultDataTransport;
        //public DataTransport EmailTransport;
        private string password;
        [DataMember]
        [FieldAlias("password")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        private string URL;
        [DataMember]
        [FieldAlias("URL")]
        public string Url
        {
            get { return URL; }
            set { URL = value; }
        }

        private Employee user;
        [DataMember]
        [FieldAlias("user")]
        public Employee User
        {
            get { return user; }
            set { user = value; }
        }
    }
}
