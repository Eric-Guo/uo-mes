using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Physical
{
    [DataContract]
    [Persistent(IdentityField = "shipmentDestination_id")]
    public class ShipmentDestination : UO_Model.Base.NamedObject
    {
        private int shipmentDestination_id;
        [FieldAlias("shipmentDestination_id")]
        public int ShipmentDestination_ID
        {
            get { return shipmentDestination_id; }
            set { shipmentDestination_id = value; }
        }

        private Customer customer;
        [DataMember]
        [FieldAlias("customer")]
        public Customer Customer
        {
            get { return customer; }
            set { customer = value; }
        }

        private Factory factory;
        [DataMember]
        [FieldAlias("factory")]
        public Factory Factory
        {
            get { return factory; }
            set { factory = value; }
        }
        //public ContainerStatus localContainerStatus;
        //public ContainerStatus rocalContainerStatus;
        //public Location location;
        private Site remoteSite;
        [DataMember]
        [FieldAlias("remoteSite")]
        public Site RemoteSite
        {
            get { return remoteSite; }
            set { remoteSite = value; }
        }
    }
}
