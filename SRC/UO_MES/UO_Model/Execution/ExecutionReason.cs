using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Execution.Reason
{
    [DataContract]
    [Persistent]
    abstract public class ExecutionReason : UO_Model.Base.UserReason
    {
    }

    [DataContract]
    [Persistent]
    public class BonusReason : ExecutionReason
    {
        public BonusReason()
        {
            reasonGroup = "Bonus Reason"; 
        }
    }

    [DataContract]
    [Persistent]
    public class BuyReason : ExecutionReason
    {
        public BuyReason()
        {
            reasonGroup = "Buy Reason"; 
        }
    }

    [DataContract]
    [Persistent]
    public class ChangeStatusReason : ExecutionReason
    {
        public ChangeStatusReason()
        {
            reasonGroup = "Change Status Reason"; 
        }
    }

    [DataContract]
    [Persistent]
    public class DefectReason : ExecutionReason
    {
        public DefectReason()
        {
            reasonGroup = "Defect Reason"; 
        }
    }

    [DataContract]
    [Persistent]
    public class HoldReason : ExecutionReason
    {
        public HoldReason()
        {
            reasonGroup = "Hold Reason"; 
        }
    }

    [DataContract]
    [Persistent]
    public class LossReason : ExecutionReason
    {
        public LossReason()
        {
            reasonGroup = "Loss Reason"; 
        }
    }

    [DataContract]
    [Persistent]
    public class CombineReason : ExecutionReason
    {
        public CombineReason()
        {
            reasonGroup = "Combine Reason"; 
        }
    }

    [DataContract]
    [Persistent]
    public class QtyAdjustReason : ExecutionReason 
    {
        public QtyAdjustReason()
        {
            reasonGroup = "Qty Adjust Reason"; 
        }
    }

    [DataContract]
    [Persistent]
    public class ReleaseReason : ExecutionReason 
    {
        public ReleaseReason()
        {
            reasonGroup = "Release Reason"; 
        }
    }

    [DataContract]
    [Persistent]
    public class SellReason : ExecutionReason 
    {
        public SellReason()
        {
            reasonGroup = "Sell Reason"; 
        }
    }

    [DataContract]
    [Persistent]
    public class SplitReason : ExecutionReason 
    {
        public SplitReason()
        {
            reasonGroup = "Split Reason"; 
        }
    }

    [DataContract]
    [Persistent]
    public class StartReason : ExecutionReason
    {
        public StartReason()
        {
            reasonGroup = "Start Reason";
        }
    }
}
