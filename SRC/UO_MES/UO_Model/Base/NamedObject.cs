using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Base
{
    /// <summary>
    /// Named Data Object
    /// MES modeling objects that can be uniquely identified by its name.
    /// </summary>
    [DataContract]
    [Persistent] 
    public abstract class NamedObject
    {
        /// <summary>
        /// DisplayName is the field generally used to represent an object in a user interface,
        /// especially when the user interface needs to represent an object with just one data element. 
        /// </summary>
        virtual public string DisplayName
        {
            get
            {
                return Name;
            }
        }

        /// <summary>
        /// Named Data Object Name
        /// Unique name for this instance
        /// </summary>
        private string name;
        [DataMember]
        [FieldAlias("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Description
        /// Description of this entity.
        /// </summary>
        private string description;
        [DataMember]
        [FieldAlias("description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        #region AssignTo
        protected void AssignToNamedObjectChangeHistory(NamedObjectChangeHistory t)
        {
            t.Name = this.Name;
            t.Description = this.Description;
        }
        #endregion
    }
}
