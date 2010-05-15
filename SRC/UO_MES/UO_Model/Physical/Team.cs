using System.Runtime.Serialization;
using Telerik.OpenAccess;

namespace UO_Model.Physical
{
    [DataContract]
    [Persistent(IdentityField = "team_id")]
    public class Team : UO_Model.Base.NamedObject
    {
        private int team_id;
        [FieldAlias("team_id")]
        public int Team_ID
        {
            get { return team_id; }
            set { team_id = value; }
        }
    }
}
