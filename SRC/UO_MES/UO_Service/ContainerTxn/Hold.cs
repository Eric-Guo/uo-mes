using UO_Model.Execution;
using UO_Model.ServiceHistory;
using UO_Service.Base;
using UO_Model.Execution.Reason;
using UO_Service.Resx;

namespace UO_Service.ContainerTxn
{
    public class Hold : ContainerTxn
    {
        protected override string TxnName { get { return "Hold"; } }

        #region Hold transaction public properties for users
        private HoldReason holdReason;
        public HoldReason HoldReason
        {
            get { return holdReason; }
            set { holdReason = ResolveCDO(value.GetType(), value.Name) as HoldReason; }
        }
        public string HoldReason_Name
        {
            get { return holdReason.Name; }
            set { holdReason = ResolveCDO("HoldReason", value) as HoldReason; }
        }
        #endregion

        protected override bool ValidateService()
        {
            return true;
        }

        protected override bool ModifyEntity()
        {
            bool success = base.ModifyEntity();
            Container.CurrentHoldCount += 1;
            CompletionMessage = string.Format(MSG.Hold_Success, Container.ContainerName, Container.CurrentHoldCount);

            return success;
        }

        protected override bool RecordServiceHistory()
        {
            HoldReleaseHistory h = new HoldReleaseHistory();
            AssignToHoldReleaseHistory(h);
            ObjScope.Add(h);
            return true;
        }

        #region AssignTo
        protected virtual void AssignToHoldReleaseHistory(HoldReleaseHistory t)
        {            
            t.HistoryMainLine = this.HistoryMainLine;
            t.HoldReason = HoldReason;
            t.HoldTime = this.HistoryMainLine.TxnDate;
        }
        #endregion

        public Hold() : base() { }
        public Hold(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }
}
