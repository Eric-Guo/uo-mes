using System;
using System.Collections.Generic;
using System.Text;
using UO_Model.Execution;
using UO_Model.ServiceHistory;

namespace UO_Service.ContainerTxn
{
    class MoveIn : ContainerTxn
    {
        protected override string TxnName { get { return "MoveIn"; } }

        #region MoveIn transaction public properties for users
        #endregion

        protected override bool ModifyEntity()
        {
            bool success = base.ModifyEntity();

            CurrentStatus cs = GetCurrentStatusToModify();
            cs.InProcess = true;
            ReplaceCurrentStatus(cs);

            return success;
        }

        protected override bool RecordServiceHistory()
        {
            MoveInHistory h = new MoveInHistory();
            AssignToMoveInHistory(h);
            ObjScope.Add(h);
            return true;
        }

        #region AssignTo
        private void AssignToMoveInHistory(MoveInHistory t)
        {
            t.HistoryMainLine = this.HistoryMainLine;
        }
        #endregion

        public MoveIn() : base() { }
        public MoveIn(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }
}
