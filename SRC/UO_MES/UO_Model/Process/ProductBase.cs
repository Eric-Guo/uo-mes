using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Base;

namespace UO_Model.Process
{
    [DataContract]
    [Persistent(IdentityField = "productBase_id")]
    public class ProductBase : RevisionBase
    {
        private int productBase_id;
        [FieldAlias("productBase_id")]
        public int ProductBase_ID
        {
            get { return productBase_id; }
            set { productBase_id = value; }
        }

        private Product currentProduct;
        [DataMember]
        [FieldAlias("currentProduct")]
        override public RevisionedObject Current
        {
            get { return currentProduct; }
            set { currentProduct = value as Product; }
        }
    }
}
