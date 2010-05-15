using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Process.Reason
{
    [DataContract]
    [Persistent]
    abstract public class ProcessReason : UO_Model.Base.UserReason
    {
    }

    [DataContract]
    [Persistent]
    public class IssueDifferenceReason : ProcessReason
    {
        public IssueDifferenceReason()
        {
            reasonGroup = "Issue Difference Reason"; 
        }
    }

    [DataContract]
    [Persistent]
    public class IssueReason : ProcessReason
    {
        public IssueReason()
        {
            reasonGroup = "Issue Reason"; 
        }
    }

    [DataContract]
    [Persistent]
    public class RemoveReason : ProcessReason
    {
        public RemoveReason()
        {
            reasonGroup = "Remove Reason";
        }
    }

    [DataContract]
    [Persistent]
    public class LocalReworkReason : ProcessReason
    {
        public LocalReworkReason()
        {
            reasonGroup = "Local Rework Reason"; 
        }
    }

    [DataContract]
    [Persistent]
    public class ReworkReason : ProcessReason
    {
        public ReworkReason()
        {
            reasonGroup = "Rework Reason"; 
        }
    }
}
