using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Execution.Code
{
    [DataContract]
    [Persistent]
    abstract public class ExecutionCode : UO_Model.Base.UserCode
    {
    }

    [DataContract]
    [Persistent]
    public class OwnerCode : ExecutionCode
    {
        public OwnerCode()
        {
            codeGroup = "Owner";
        }
    }

    [DataContract]
    [Persistent]
    public class PriorityCode : ExecutionCode
    {
        public PriorityCode()
        {
            codeGroup = "Priority Code";
        }
    }
}
