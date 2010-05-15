using System;
using System.Collections.Generic;
using System.Text;
using UO_Service.ContainerTxn;
using UO_Model.Physical;
using UO_Model.Workflow;
using UO_Model.Execution;

namespace UO_Service.CompoundTxn
{
    public class MoveOut : CompoundTxn
    {
        protected override string TxnName { get { return "MoveOut"; } }

        #region common transaction public properties for users
        public Container Container
        {
            get { return thruputService.Container; }
            set
            {
                thruputService.Container = value;
                moveTxnService.Container = value;
            }
        }
        public string Container_Name
        {
            get { return thruputService.Container_Name; }
            set
            {
                thruputService.Container_Name = value;
                moveTxnService.Container_Name = value;
            }
        }
        #endregion

        #region Thruput transaction public properties for users
        public bool ThruputAllQty
        {
            get { return thruputService.ThruputAllQty; }
            set { thruputService.ThruputAllQty = value; }
        }

        public int Qty
        {
            get { return thruputService.Qty; }
            set { thruputService.Qty = value; }
        }

        public Resource Resource
        {
            get { return thruputService.Resource; }
            set { thruputService.Resource = value; }
        }
        public string Resource_Name
        {
            get { return thruputService.Resource_Name; }
            set { thruputService.Resource_Name = value; }
        }
        #endregion

        #region Move transaction public properties for users
        public Step ToWorkflowStep
        {
            get { return moveTxnService.ToWorkflowStep; }
            set { moveTxnService.ToWorkflowStep = value; }
        }
        #endregion

        protected override bool ValidateService()
        {
            if (thruputService.Validate() == false)
                return false;
            if (moveTxnService.Validate() == false)
                return false;
            return true;
        }

        protected override bool ModifyEntity()
        {
            bool success = thruputService.Execute();
            if (ContinueOrRollback(success) == false)
                return success;

            success = moveTxnService.Execute();
            if (ContinueOrRollback(success) == false)
                return success;
            return success;
        }

        protected override bool RecordServiceHistory()
        {
            // no need to record specific history for MoveTxn and Thruput history is enough
            return true;
        }

        private Thruput thruputService;
        private MoveTxn moveTxnService;
        public MoveOut() : base()
        {
            thruputService = new Thruput(ObjScope, HistoryMainLine);
            moveTxnService = new MoveTxn(ObjScope, HistoryMainLine);
        }
    }
}
