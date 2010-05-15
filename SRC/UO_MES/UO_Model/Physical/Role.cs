using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Physical
{
    [DataContract]
    [Persistent(IdentityField = "role_id")]
    public class Role : UO_Model.Base.NamedObject
    {
        private int role_id;
        [FieldAlias("role_id")]
        public int Role_ID
        {
            get { return role_id; }
            set { role_id = value; }
        }

        private IList<Employee> employees = new List<Employee>();
        [DataMember]
        [FieldAlias("employees")]
        public IList<Employee> Employees
        {
            get { return employees; }
        }
    }
}
