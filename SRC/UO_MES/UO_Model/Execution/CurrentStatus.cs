using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Physical;

namespace UO_Model.Execution
{
    [DataContract]
    [Persistent(IdentityField = "currentStatus_id")]
    public class CurrentStatus
    {
        private int currentStatus_id;
        [FieldAlias("currentStatus_id")]
        public int CurrentStatus_ID
        {
            get { return currentStatus_id; }
            set { currentStatus_id = value; }
        }

        private IList<Container> containers = new List<Container>();  // inverse Container.currentStatus
        [DataMember]
        [FieldAlias("containers")]
        public IList<Container> Containers
        {
            get { return containers; }
        }

        private Workflow.Workflow workflow;
        [DataMember]
        [FieldAlias("workflow")]
        public Workflow.Workflow Workflow
        {
            get { return workflow; }
            set { workflow = value; }
        }

        private Workflow.Step workflowStep;
        [DataMember]
        [FieldAlias("workflowStep")]
        public Workflow.Step WorkflowStep
        {
            get { return workflowStep; }
            set { workflowStep = value; }
        }

        private int currentStepPass = 0;
        [DataMember]
        [FieldAlias("currentStepPass")]
        public int CurrentStepPass
        {
            get { return currentStepPass; }
            set { currentStepPass = value; }
        }

        private Process.Spec spec;
        [DataMember]
        [FieldAlias("spec")]
        public Process.Spec Spec
        {
            get { return spec; }
            set { spec = value; }
        }

        private bool inProcess = false;
        [DataMember]
        [FieldAlias("inProcess")]
        public bool InProcess
        {
            get { return inProcess; }
            set { inProcess = value; }
        }

        private Resource resource;
        /// <summary>
        /// If Resource field is not null, Resource is the current resource processing the container
        /// </summary>
        [DataMember]
        [FieldAlias("resource")]
        public Resource Resource
        {
            get { return resource; }
            set { resource = value; }
        }

        private Factory factory;
        [DataMember]
        [FieldAlias("factory")]
        public Factory Factory
        {
            get { return factory; }
            set { factory = value; }
        }

        #region AssignTo
        public virtual void AssignToCurrentStatus(CurrentStatus t)
        {
            t.Workflow = this.Workflow;
            t.WorkflowStep = this.WorkflowStep;
            t.CurrentStepPass = this.CurrentStepPass;
            t.Spec = this.Spec;
            t.InProcess = this.InProcess;
            t.Resource = this.Resource;
            t.Factory = this.Factory;
        }
        #endregion
    }
}
