using System;
using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Physical;

namespace UO_Model.Process
{
    [DataContract]
    [Persistent(IdentityField = "spec_id")]
    public class Spec : RevisionedObject
    {
        private int spec_id;
        [FieldAlias("spec_id")]
        public int Spec_ID
        {
            get { return spec_id; }
            set { spec_id = value; }
        }

        private SpecBase revBase;
        [DataMember]
        [FieldAlias("revBase")]
        override public RevisionBase RevBase
        {
            get 
            {
                if (revBase == null)
                    revBase = new SpecBase();
                return revBase;
            }
            set { revBase = value as SpecBase; }
        }

        #region User maintained properties
        private Operation operation;
        [DataMember]
        [FieldAlias("operation")]
        public Operation Operation
        {
            get { return operation; }
            set { operation = value; }
        }

        private ResourceGroup resourceGroup;
        [DataMember]
        [FieldAlias("resourceGroup")]
        public ResourceGroup ResourceGroup
        {
            get { return resourceGroup; }
            set { resourceGroup = value; }
        }

        private Setup setup;
        [DataMember]
        [FieldAlias("setup")]
        public Setup Setup
        {
            get { return setup; }
            set { setup = value; }
        }
        #endregion

        #region AssignTo
        public virtual void AssignToSpecChangeHistory(SpecChangeHistory t)
        {
            base.AssignToRevisionedObjectChangeHistory(t);
            t.Operation = this.Operation;
            t.ResourceGroup = this.ResourceGroup;
            t.Setup = this.Setup;
        }
        #endregion
    }

    [Persistent(IdentityField = "specChangeHistory_id")]
    public class SpecChangeHistory : RevisionedObjectChangeHistory
    { 
        protected SpecChangeHistory() { }
        public SpecChangeHistory(RevisionedObject toChangeRDO, DateTime actionDate, ActionType actionType) : base(actionDate, actionType)
        {
            spec = toChangeRDO as Spec;
        }

        private int specChangeHistory_id;
        [FieldAlias("specChangeHistory_id")]
        public int SpecChangeHistory_ID
        {
            get { return specChangeHistory_id; }
            set { specChangeHistory_id = value; }
        }

        private Spec spec;
        [FieldAlias("spec")]
        override public RevisionedObject ToChangeRDO
        {
            get { return spec; }
        }

        #region Copy from Spec, do not modify it, only copy from Spec's User Maintained properties region
        private Operation operation;
        [FieldAlias("operation")]
        public Operation Operation
        {
            get { return operation; }
            set { operation = value; }
        }

        private ResourceGroup resourceGroup;
        [FieldAlias("resourceGroup")]
        public ResourceGroup ResourceGroup
        {
            get { return resourceGroup; }
            set { resourceGroup = value; }
        }

        private Setup setup;
        [FieldAlias("setup")]
        public Setup Setup
        {
            get { return setup; }
            set { setup = value; }
        }
        #endregion
    }
}
