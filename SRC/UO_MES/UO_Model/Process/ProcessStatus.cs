using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Process.Status
{
    [DataContract]
    [Persistent]
    abstract public class ProcessStatus : UO_Model.Base.UserStatus
    {
    }

    [DataContract]
    [Persistent]
    public class OrderStatus : ProcessStatus
    {
        public OrderStatus()
        {
            statusGroup = "Order Status";
        }
    }
}
