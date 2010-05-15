using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Process
{
    [DataContract]
    [Persistent(IdentityField = "productFamily_id")]
    public class ProductFamily : UO_Model.Base.NamedObject
    {
        private int productFamily_id;
        [FieldAlias("productFamily_id")]
        public int ProductFamily_ID
        {
            get { return productFamily_id; }
            set { productFamily_id = value; }
        }

        private IList<Product> products = new List<Product>();    // inverse Product.productFamily
        [DataMember]
        [FieldAlias("products")]
        public IList<Product> Products
        {
            get { return products; }
            set { products = value; }
        }
        
        private double stdCost;
        [DataMember]
        [FieldAlias("stdCost")]
        public double StdCost
        {
            get { return stdCost; }
            set { stdCost = value; }
        }

        private double plannedCost;
        [DataMember]
        [FieldAlias("plannedCost")]
        public double PlannedCost
        {
            get { return plannedCost; }
            set { plannedCost = value; }
        }

        private int stdQty;
        [DataMember]
        [FieldAlias("stdQty")]
        public int StdQty
        {
            get { return stdQty; }
            set { stdQty = value; }
        }
        private int stdQty2;
        [DataMember]
        [FieldAlias("stdQty2")]
        public int StdQty2
        {
            get { return stdQty2; }
            set { stdQty2 = value; }
        }
        private UOM stdUOM;
        [DataMember]
        [FieldAlias("stdUOM")]
        public UOM StdUOM
        {
            get { return stdUOM; }
            set { stdUOM = value; }
        }
        private UOM stdUOM2;
        [DataMember]
        [FieldAlias("stdUOM2")]
        public UOM StdUOM2
        {
            get { return stdUOM2; }
            set { stdUOM2 = value; }
        }
    }
}
