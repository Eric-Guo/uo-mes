using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Physical;

namespace UO_Model.Process
{
    [DataContract]
    [Persistent(IdentityField = "operation_id")]
    public class Operation : UO_Model.Base.NamedObject
    {
        private int operation_id;
        [FieldAlias("operation_id")]
        public int Operation_ID
        {
            get { return operation_id; }
            set { operation_id = value; }
        }

        private WorkCenter workCenter;
        [DataMember]
        [FieldAlias("workCenter")]
        public WorkCenter WorkCenter
        {
            get { return workCenter; }
            set { workCenter = value; }
        }

        private bool useQueue = false;
        /// <summary>
        /// Defines the usage of queue at this operation.  
        /// If a queue is required, a transaction (MoveIn) must be performed prior to any work being recorded.
        /// </summary>
        [DataMember]
        [FieldAlias("useQueue")]
        public bool UseQueue
        {
            get { return useQueue; }
            set { useQueue = value; }
        }
    }
}
