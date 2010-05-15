using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Telerik.OpenAccess;
using UO_Model.Process;
using UO_Model.Base;

namespace UO_Model.Physical
{
    /// <summary>
    /// Employee profile are used to determine default values. 
    /// An instance is associated with each Employee (1 to 1) defined to the MES (that can log-in).
    /// Values can be modified via MESProfileProvider and saved or discarded when needed,
    /// the updated values will be used for initialization at the next log-in (for that Employee).
    /// </summary>
    [DataContract]
    [Persistent(IdentityField = "employeeProfile_id")]
    public class EmployeeProfile
    {
        private int employeeProfile_id;
        [FieldAlias("employeeProfile_id")]
        public int EmployeeProfile_ID
        {
            get { return employeeProfile_id; }
            set { employeeProfile_id = value; }
        }

        private Employee employee;
        [DataMember]
        [FieldAlias("employee")]
        public Employee Employee
        {
            get { return employee; }
            set { employee = value; }
        }

        private DateTime lastUpdatedDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("lastUpdatedDate")]
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }

        private string language;
        [DataMember]
        [FieldAlias("language")]
        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        private string styleTheme;
        [DataMember]
        [FieldAlias("styleTheme")]
        public string StyleTheme
        {
            get { return styleTheme; }
            set { styleTheme = value; }
        }

        private string factory_name;
        [DataMember]
        [FieldAlias("factory_name")]
        public string Factory_Name
        {
            get { return factory_name; }
            set { factory_name = value; }
        }

        private string operation_name;
        [DataMember]
        [FieldAlias("operation_name")]
        public string Operation_Name
        {
            get { return operation_name; }
            set { operation_name = value; }
        }

        private string workCenter_name;
        [DataMember]
        [FieldAlias("workCenter_name")]
        public string WorkCenter_Name
        {
            get { return workCenter_name; }
            set { workCenter_name = value; }
        }
    }
}
