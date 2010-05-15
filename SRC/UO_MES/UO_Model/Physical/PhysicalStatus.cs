using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Physical.Status
{
    [DataContract]
    [Persistent]
    abstract public class PhysicalStatus : UO_Model.Base.UserStatus
    {
    }

    [DataContract]
    [Persistent]
    public class ResourceStatus : PhysicalStatus
    {
        public ResourceStatus()
        {
            statusGroup = "Resource Status";
        }
    }
}
