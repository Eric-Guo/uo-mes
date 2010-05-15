using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Base
{
    /// <summary>
    /// User defined Status is used to record some status in other CDO(Class Data Object)
    /// </summary>
    [DataContract]
    [Persistent]
    public abstract class UserStatus : NamedObject
    {
        protected string statusGroup = "";
        [FieldAlias("statusGroup")]
        public string StatusGroup
        {
            get { return statusGroup; }
        }
    }
}
