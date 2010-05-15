using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Execution.Status;
using UO_Model.Execution.Reason;
using UO_Model.Process;

namespace UO_Model.Execution
{
    [DataContract]
    [Persistent(IdentityField = "container_id")]
    public class Container
    {
        private int container_id;
        [FieldAlias("container_id")]
        public int Container_ID
        {
            get { return container_id; }
            set { container_id = value; }
        }

        private string containerName;
        [DataMember]
        [FieldAlias("containerName")]
        public string ContainerName
        {
            get { return containerName; }
            set { containerName = value; }
        }

        private Process.ContainerLevel containerLevel;
        [DataMember]
        [FieldAlias("containerLevel")]
        public Process.ContainerLevel ContainerLevel
        {
            get { return containerLevel; }
            set { containerLevel = value; }
        }

        private int qty;
        [DataMember]
        [FieldAlias("qty")]
        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        private int currentThruputQty;
        [DataMember]
        [FieldAlias("currentThruputQty")]
        public int CurrentThruputQty
        {
            get { return currentThruputQty; }
            set { currentThruputQty = value; }
        }

        private int currentHoldCount;
        /// <summary>
        /// Count of the number of containers on hold in the container structure 
        /// starting at this container and tracing down through of all its descendants.  
        /// If this container itself is currently directly on hold, and 2 of its children 
        /// are also on hold, the CurrentHoldCount would equal 3.
        /// </summary>
        [DataMember]
        [FieldAlias("currentHoldCount")]
        public int CurrentHoldCount
        {
            get { return currentHoldCount; }
            set { currentHoldCount = value; }
        }

        private ContainerStatus containerStatus;
        [DataMember]
        [FieldAlias("containerStatus")]
        public ContainerStatus ContainerStatus
        {
            get { return containerStatus; }
            set { containerStatus = value; }
        }

        private DateTime dueDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("dueDate")]
        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        private DateTime lastActivityDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("lastActivityDate")]
        public DateTime LastActivityDate
        {
            get { return lastActivityDate; }
            set { lastActivityDate = value; }
        }

        private DateTime lastMoveDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("lastMoveDate")]
        public DateTime LastMoveDate
        {
            get { return lastMoveDate; }
            set { lastMoveDate = value; }
        }

        private UOM uom;
        [DataMember]
        [FieldAlias("uom")]
        public UOM UOM
        {
            get { return uom; }
            set { uom = value; }
        }

        private Product product;
        [DataMember]
        [FieldAlias("product")]
        public Product Product
        {
            get { return product; }
            set { product = value; }
        }

        private MfgOrder mfgOrder;
        [DataMember]
        [FieldAlias("mfgOrder")]
        public MfgOrder MfgOrder
        {
            get { return mfgOrder; }
            set { mfgOrder = value; }
        }

        private SalesOrder salesOrder;
        [DataMember]
        [FieldAlias("salesOrder")]
        public SalesOrder SalesOrder
        {
            get { return salesOrder; }
            set { salesOrder = value; }
        }

        private CurrentStatus currentStatus;
        [DataMember]
        [FieldAlias("currentStatus")]
        public CurrentStatus CurrentStatus
        {
            get { return currentStatus; }
            set { currentStatus = value; }
        }

        private StartReason startReason;
        [DataMember]
        [FieldAlias("startReason")]
        public StartReason StartReason
        {
            get { return startReason; }
            set { startReason = value; }
        }

        private Container parent;
        [DataMember]
        [FieldAlias("parent")]
        public Container Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        private IList<Container> childContainers = new List<Container>();
        [DataMember]
        [FieldAlias("childContainers")]
        public IList<Container> ChildContainers
        {
            get { return childContainers; }
        }

        #region AssignTo
        public virtual void AssignToContainer(Container t)
        {
            t.ContainerName = this.ContainerName;
            t.ContainerLevel = this.ContainerLevel;
            t.Qty = this.Qty;
            t.CurrentHoldCount = this.CurrentHoldCount;
            t.ContainerStatus = this.ContainerStatus;
            t.DueDate = this.DueDate;
            t.UOM = this.UOM;
            t.Product = this.Product;
            t.CurrentStatus = this.CurrentStatus;
            t.StartReason = this.StartReason;
            t.Parent = this.Parent;
            // TODO: How to deal with child container need implement
            // t.ChildContainers = this.ChildContainers;
        }
        #endregion
    }
}
