using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Physical
{
    [DataContract]
    [Persistent(IdentityField = "systemSecurityGroup_id")]
    public class SystemSecurityGroup : UO_Model.Base.NamedObject
    {
        private int systemSecurityGroup_id;
        [FieldAlias("systemSecurityGroup_id")]
        public int SystemSecurityGroup_ID
        {
            get { return systemSecurityGroup_id; }
            set { systemSecurityGroup_id = value; }
        }

        private string domainName;
        [DataMember]
        [FieldAlias("domainName")]
        public string DomainName
        {
            get { return domainName; }
            set { domainName = value; }
        }
        private string fullName;
        [DataMember]
        [FieldAlias("fullName")]
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }
        private string systemSecurityGroup;
        [DataMember]
        [FieldAlias("systemSecurityGroup")]
        public string sSystemSecurityGroup
        {
            get { return systemSecurityGroup; }
            set { systemSecurityGroup = value; }
        }
    }
}
