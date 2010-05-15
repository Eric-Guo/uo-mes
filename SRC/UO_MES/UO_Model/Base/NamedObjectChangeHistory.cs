using System;
using Telerik.OpenAccess;

namespace UO_Model.Base
{
    [Persistent]
    abstract public class NamedObjectChangeHistory
    {
        protected NamedObjectChangeHistory() { }
        protected NamedObjectChangeHistory(DateTime actionDate, ActionType actionType)
        {
            this.actionDate = actionDate;
            this.actionType = actionType;
        }

        abstract public NamedObject ToChangeNDO { get; }

        private string name;
        [FieldAlias("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string description;
        [FieldAlias("description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        
        private DateTime actionDate = Constants.NullDateTime;
        [FieldAlias("actionDate")]
        public DateTime ActionDate
        {
            get { return actionDate; }
        }

        private ActionType actionType = ActionType.None;
        [FieldAlias("actionType")]
        public ActionType ActionType
        {
            get { return actionType; }
        }
    }
}
