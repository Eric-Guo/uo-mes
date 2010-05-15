using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Physical;
using UO_Model.Execution;
using UO_Model.Process.Status;

namespace UO_Model.Process
{
    [DataContract]
    [Persistent(IdentityField = "mfgOrder_id")]
    public class MfgOrder : NamedObject
    {
        #region MES maintained properties
        private int mfgOrder_id;
        [FieldAlias("mfgOrder_id")]
        public int MfgOrder_ID
        {
            get { return mfgOrder_id; }
            set { mfgOrder_id = value; }
        }

        private IList<Container> containers = new List<Container>();
        [FieldAlias("containers")]
        public IList<Container> Containers
        {
            get { return containers; }
        }

        private int releasedQty = 0;
        [FieldAlias("releasedQty")]
        public int ReleasedQty
        {
            get { return releasedQty; }
            set { releasedQty = value; }
        }
        #endregion

        #region User maintained properties
        private Product product;
        [DataMember]
        [FieldAlias("product")]
        public Product Product
        {
            get { return product; }
            set { product = value; }
        }

        private Product beginProduct;
        [DataMember]
        [FieldAlias("beginProduct")]
        public Product BeginProduct
        {
            get { return beginProduct; }
            set { beginProduct = value; }
        }

        private DateTime plannedStartDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("plannedStartDate")]
        public DateTime PlannedStartDate
        {
            get { return plannedStartDate; }
            set { plannedStartDate = value; }
        }

        private DateTime plannedCompletionDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("plannedCompletionDate")]
        public DateTime PlannedCompletionDate
        {
            get { return plannedCompletionDate; }
            set { plannedCompletionDate = value; }
        }

        private DateTime releasedDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("releasedDate")]
        public DateTime ReleasedDate
        {
            get { return releasedDate; }
            set { releasedDate = value; }
        }     

        private int planedQty;
        [DataMember]
        [FieldAlias("planedQty")]
        public int PlanedQty
        {
            get { return planedQty; }
            set { planedQty = value; }
        }

        private UOM uom;
        [DataMember]
        [FieldAlias("uom")]
        public UOM UOM
        {
            get { return uom; }
            set { uom = value; }
        }

        private Factory reportingFactory;
        [DataMember]
        [FieldAlias("reportingFactory")]
        public Factory ReportingFactory
        {

            get { return reportingFactory; }
            set { reportingFactory = value; }
        }

        private SalesOrder salesOrder;
        [DataMember]
        [FieldAlias("salesOrder")]
        public SalesOrder SalesOrder
        {
            get { return salesOrder; }
            set { salesOrder = value; }
        }

        private OrderStatus orderStatus;
        [DataMember]
        [FieldAlias("orderStatus")]
        public OrderStatus OrderStatus
        {
            get { return orderStatus; }
            set { orderStatus = value; }
        }
        #endregion

        //public ERPBOM erpBOM;
        //public ERPRouter erpRouter;

        #region AssignTo
        public virtual void AssignToMfgOrderChangeHistory(MfgOrderChangeHistory t)
        {
            base.AssignToNamedObjectChangeHistory(t);
            t.Product = this.Product;
            t.BeginProduct = this.BeginProduct;
            t.PlannedStartDate = this.PlannedStartDate;
            t.PlannedCompletionDate = this.PlannedCompletionDate;
            t.ReleasedDate = this.ReleasedDate;
            t.PlanedQty = this.PlanedQty;
            t.UOM = this.UOM;
            t.ReportingFactory = this.ReportingFactory;
            t.SalesOrder = this.SalesOrder;
            t.OrderStatus = this.OrderStatus;
        }
        #endregion
    }

    [Persistent(IdentityField = "mfgOrderChangeHistory_id")]
    public class MfgOrderChangeHistory : NamedObjectChangeHistory
    {
        protected MfgOrderChangeHistory() { }
        public MfgOrderChangeHistory(NamedObject toChangeNDO, DateTime actionDate, ActionType actionType)
            : base(actionDate, actionType)
        {
            mfgOrder = toChangeNDO as MfgOrder;
        }

        private int mfgOrderChangeHistory_id;
        [FieldAlias("mfgOrderChangeHistory_id")]
        public int MfgOrderChangeHistory_ID
        {
            get { return mfgOrderChangeHistory_id; }
            set { mfgOrderChangeHistory_id = value; }
        }

        private MfgOrder mfgOrder;
        [FieldAlias("mfgOrder")]
        override public NamedObject ToChangeNDO
        {
            get { return mfgOrder; }
        }

        #region Copy from MfgOrder, do not modify it, only copy from MfgOrder's User Maintained properties region
        private Product product;
        [FieldAlias("product")]
        public Product Product
        {
            get { return product; }
            set { product = value; }
        }

        private Product beginProduct;
        [FieldAlias("beginProduct")]
        public Product BeginProduct
        {
            get { return beginProduct; }
            set { beginProduct = value; }
        }

        private DateTime plannedStartDate = Constants.NullDateTime;
        [FieldAlias("plannedStartDate")]
        public DateTime PlannedStartDate
        {
            get { return plannedStartDate; }
            set { plannedStartDate = value; }
        }

        private DateTime plannedCompletionDate = Constants.NullDateTime;
        [FieldAlias("plannedCompletionDate")]
        public DateTime PlannedCompletionDate
        {
            get { return plannedCompletionDate; }
            set { plannedCompletionDate = value; }
        }

        private DateTime releasedDate = Constants.NullDateTime;
        [FieldAlias("releasedDate")]
        public DateTime ReleasedDate
        {
            get { return releasedDate; }
            set { releasedDate = value; }
        }

        private int planedQty;
        [FieldAlias("planedQty")]
        public int PlanedQty
        {
            get { return planedQty; }
            set { planedQty = value; }
        }

        private UOM uom;
        [FieldAlias("uom")]
        public UOM UOM
        {
            get { return uom; }
            set { uom = value; }
        }

        private Factory reportingFactory;
        [FieldAlias("reportingFactory")]
        public Factory ReportingFactory
        {

            get { return reportingFactory; }
            set { reportingFactory = value; }
        }

        private SalesOrder salesOrder;
        [FieldAlias("salesOrder")]
        public SalesOrder SalesOrder
        {
            get { return salesOrder; }
            set { salesOrder = value; }
        }

        private OrderStatus orderStatus;
        [FieldAlias("orderStatus")]
        public OrderStatus OrderStatus
        {
            get { return orderStatus; }
            set { orderStatus = value; }
        }
        #endregion
    }
}
