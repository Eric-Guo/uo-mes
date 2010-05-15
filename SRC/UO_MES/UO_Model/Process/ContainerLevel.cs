using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Process
{
    [DataContract]
    [Persistent(IdentityField = "containerLevel_id")]
    public class ContainerLevel : UO_Model.Base.NamedObject
    {
        private int containerLevel_id;
        [FieldAlias("containerLevel_id")]
        public int ContainerLevel_ID
        {
            get { return containerLevel_id; }
            set { containerLevel_id = value; }
        }

        private bool allowMove;
        [DataMember]
        [FieldAlias("allowMove")]
        public bool AllowMove
        {
            get { return allowMove; }
            set { allowMove = value; }
        }

        // public IList<ContainerLevel> parentLevels = new List<ContainerLevel>();
        // public IList<ContainerLevel> childLevels = new List<ContainerLevel>();
    }
}
