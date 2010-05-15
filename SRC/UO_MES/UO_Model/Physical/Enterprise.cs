using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Physical
{
    [DataContract]
    [Persistent(IdentityField = "enterprise_id")]
    public class Enterprise : UO_Model.Base.NamedObject
    {
        private int enterprise_id;
        [FieldAlias("enterprise_id")]
        public int Enterprise_ID
        {
            get { return enterprise_id; }
            set { enterprise_id = value; }
        }

        private string reportHeading;
        [DataMember]
        [FieldAlias("reportHeading")]
        public string ReportHeading
        {
            get { return reportHeading; }
            set { reportHeading = value; }
        }

        private IList<Factory> factories = new List<Factory>();  // inverse Factory.enterprise
        [DataMember]
        [FieldAlias("factories")]
        public IList<Factory> Factories
        {
            get { return factories; }
        }
    }
}
