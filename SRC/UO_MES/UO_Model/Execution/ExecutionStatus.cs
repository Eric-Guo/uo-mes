using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Execution.Status
{
    [DataContract]
    [Persistent]
    abstract public class ExecutionStatus : UO_Model.Base.UserStatus
    {
    }

    [DataContract]
    [Persistent]
    public class ContainerStatus : ExecutionStatus
    {
        public ContainerStatus()
        {
            statusGroup = "Container Status";
        }
    }
}
