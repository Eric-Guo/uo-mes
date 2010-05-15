using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Base
{
    /// <summary>
    /// User defined Reason can be input to record some certain transaction's caused reason.
    /// </summary>
    [DataContract]
    [Persistent]
    abstract public class UserReason : NamedObject
    {
        protected string reasonGroup = "";
        [FieldAlias("reasonGroup")]
        public string ReasonGroup 
        {
            get { return reasonGroup; } 
        }
    }
}
