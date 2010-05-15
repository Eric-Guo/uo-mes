using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Process.Code;

namespace UO_Model.Process
{
    [DataContract]
    [Persistent(IdentityField = "product_id")]
    public class Product : RevisionedObject
    {
        private int product_id;
        [FieldAlias("product_id")]
        public int Product_ID
        {
            get { return product_id; }
            set { product_id = value; }
        }

        private ProductBase revBase;
        [DataMember]
        [FieldAlias("revBase")]
        override public RevisionBase RevBase
        {
            get
            {
                if (revBase == null)
                    revBase = new ProductBase();
                return revBase;
            }
            set { revBase = value as ProductBase; }
        }

        //public BOM bom;

        private ProductFamily productFamily;      // inverse ProductFamily.products
        [DataMember]
        [FieldAlias("productFamily")]
        public ProductFamily ProductFamily
        {
            get { return productFamily; }
            set { productFamily = value; }
        }

        //public ProductType productType;
        private ProductTypeCode productTypeCode;
        [DataMember]
        [FieldAlias("productTypeCode")]
        public ProductTypeCode ProductTypeCode
        {
            get { return productTypeCode; }
            set { productTypeCode = value; }
        }

        private int stdStartedQty;
        [DataMember]
        [FieldAlias("stdStartedQty")]
        public int StdStartedQty
        {
            get { return stdStartedQty; }
            set { stdStartedQty = value; }
        }

        private int stdStartedQty2;
        [DataMember]
        [FieldAlias("stdStartedQty2")]
        public int StdStartedQty2
        {
            get { return stdStartedQty2; }
            set { stdStartedQty2 = value; }
        }

        private UOM stdStartedUOM;
        [DataMember]
        [FieldAlias("stdStartedUOM")]
        public UOM StdStartedUOM
        {
            get { return stdStartedUOM; }
            set { stdStartedUOM = value; }
        }

        private UOM stdStartedUOM2;
        [DataMember]
        [FieldAlias("stdStartedUOM2")]
        public UOM StdStartedUOM2
        {
            get { return stdStartedUOM2; }
            set { stdStartedUOM2 = value; }
        }

        //private IList<Product> substitutes = new List<Product>();
        //[FieldAlias("substitutes")]
        //public IList<Product> Substitues
        //{
        //    get { return substitutes; }
        //}

        private UO_Model.Workflow.Workflow workflow;
        [FieldAlias("workflow")]
        public UO_Model.Workflow.Workflow Workflow
        {
            get { return workflow; }
            set { workflow = value; }
        }
    }
}
