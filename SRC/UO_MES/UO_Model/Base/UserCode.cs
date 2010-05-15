using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Base
{
    /// <summary>
    /// User defined Code is most often used to define code through selection lists for data entry fields. 
    /// User Defined Codes are used to allow each customer to specify a list of allowable values for a specific field. 
    /// In many cases a User Defined Code consist of a Name and Description with no additional attributes.     
    /// The name for each Code must be unique within its type. This value is used as an alternate key for lookup-up, 
    /// data entry and validation.
    /// </summary>
    [DataContract]
    [Persistent]
    abstract public class UserCode : NamedObject
    {
        protected string codeGroup = "";
        [FieldAlias("codeGroup")]
        public string CodeGroup
        {
            get { return codeGroup; }
        }
    }
}
