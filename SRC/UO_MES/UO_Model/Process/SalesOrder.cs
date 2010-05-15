using System;
using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Physical;
using UO_Model.Process.Code;
using UO_Model.Execution;
using System.Collections.Generic;

namespace UO_Model.Process
{
    [DataContract]
    [Persistent(IdentityField = "salesOrder_id")]
    public class SalesOrder : NamedObject
    {
        private int salesOrder_id;
        [FieldAlias("salesOrder_id")]
        public int SalesOrder_ID
        {
            get { return salesOrder_id; }
            set { salesOrder_id = value; }
        }

        private IList<Container> containers = new List<Container>();
        [DataMember]
        [FieldAlias("containers")]
        public IList<Container> Containers
        {
            get { return containers; }
        }

        private IList<MfgOrder> mfgOrders = new List<MfgOrder>();
        [DataMember]
        [FieldAlias("mfgOrders")]
        public IList<MfgOrder> MfgOrders
        {
            get { return mfgOrders; }
        }

        private Product product;
        [DataMember]
        [FieldAlias("product")]
        public Product Product
        {
            get { return product; }
            set { product = value; }
        }

        private int qty;
        [DataMember]
        [FieldAlias("qty")]
        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        private UOM uom;
        [DataMember]
        [FieldAlias("uom")]
        public UOM UOM
        {
            get { return uom; }
            set { uom = value; }
        }

        private Customer customer;
        [DataMember]
        [FieldAlias("customer")]
        public Customer Customer
        {
            get { return customer; }
            set { customer = value; }
        }

        private DateTime shipDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("shipDate")]
        public DateTime ShipDate
        {
            get { return shipDate; }
            set { shipDate = value; }
        }

        private OrderTypeCode orderTypeCode;
        [DataMember]
        [FieldAlias("orderTypeCode")]
        public OrderTypeCode OrderTypeCode
        {
            get { return orderTypeCode; }
            set { orderTypeCode = value; }
        }
    }
}
