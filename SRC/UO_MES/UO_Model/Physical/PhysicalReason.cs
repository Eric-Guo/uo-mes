using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Physical.Reason
{
    [DataContract]
    [Persistent]
    abstract public class PhysicalReason : UO_Model.Base.UserReason
    {
    }

    [DataContract]
    [Persistent]
    public class ResourceStatusReason : PhysicalReason
    {
        public ResourceStatusReason()
        {
            reasonGroup = "Resource Status Reason"; 
        }
    }

    [DataContract]
    [Persistent]
    public class RollupReason : PhysicalReason
    {
        public RollupReason()
        {
            reasonGroup = "Rollup Reason"; 
        }
    }

    [DataContract]
    [Persistent]
    public class ShippingReason : PhysicalReason
    {
        public ShippingReason()
        {
            reasonGroup = "Shipping Reason"; 
        }
    }
}
