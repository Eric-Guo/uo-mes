using System.Runtime.Serialization;
using System.Collections.Generic;
using Telerik.OpenAccess;

namespace UO_Model.Base
{
    /// <summary>
    /// RevisionBase refers to attributes that are common to all CDOs that include revision control (for instances).
    /// There are two CDO Definitions for each; a Base Entity and a Revision Entity.  
    /// The RevisionBase holds information common to all revisions of an object, 
    /// plus information on which revision is the revision of record.
    /// </summary>
    [DataContract]
    [Persistent]
    public abstract class RevisionBase
    {
        /// <summary>
        /// Revision Data Object Name
        /// </summary>
        private string name;
        [DataMember]
        [FieldAlias("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        abstract public RevisionedObject Current { get; set; }
    }
}
