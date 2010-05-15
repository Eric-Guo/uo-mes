using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Base
{
    /// <summary>
    /// Revisioned Data Object
    /// Attributes that are common to all CDOs that include revision control (for instances). 
    /// There are two CDO Definitions for each; a Base Entity and a Revision Entity.
    /// </summary>
    [DataContract]
    [Persistent]
    public abstract class RevisionedObject
    {
        [DataMember]
        abstract public RevisionBase RevBase { get; set; }

        /// <summary>
        /// DisplayName is the field generally used to represent an object in a user interface,
        /// especially when the user interface needs to represent an object with just one data element. 
        /// </summary>
        virtual public string DisplayName
        {
            get 
            {
                return string.Format("{0}({1})", RevBase.Name, Revision);
            }
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

        /// <summary>
        /// Revision
        /// </summary>
        private string revision;
        [DataMember]
        [FieldAlias("revision")]
        public string Revision
        {
            get { return revision; }
            set { revision = value; }
        }


        /// <summary>
        /// Determines if changes are allowed to this instance
        /// </summary>
        private bool frozen;
        [DataMember]
        [FieldAlias("frozen")]
        public bool Frozen
        {
            get { return frozen; }
            set { frozen = value; }
        }

        #region AssignTo
        protected void AssignToRevisionedObjectChangeHistory(RevisionedObjectChangeHistory t)
        {
            t.Name = this.RevBase.Name;
            t.Revision = this.Revision;
            t.Description = this.Description;
        }
        #endregion
    }
}
