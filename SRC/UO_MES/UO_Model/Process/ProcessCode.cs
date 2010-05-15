using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Process.Code
{
    [DataContract]
    [Persistent]
    abstract public class ProcessCode : UO_Model.Base.UserCode
    {
    }

    [DataContract]
    [Persistent]
    public class OrderTypeCode : ProcessCode
    {
        public OrderTypeCode()
        {
            codeGroup = "Order Type";
        }
    }

    [DataContract]
    [Persistent]
    public class ProductTypeCode : ProcessCode
    {
        public ProductTypeCode()
        {
            codeGroup = "Product Type";
        }
    }

}
