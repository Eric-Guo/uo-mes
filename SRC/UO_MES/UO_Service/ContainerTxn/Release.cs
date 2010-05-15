using Telerik.OpenAccess;
using UO_Model.Execution;
using UO_Model.Execution.Reason;
using UO_Model.ServiceHistory;
using UO_Service.Resx;

namespace UO_Service.ContainerTxn
{
    public class Release : ContainerTxn
    {
        protected override string TxnName { get { return "Release"; } }

        #region Release transaction public properties for users

        private ReleaseReason releaseReason;
        public ReleaseReason ReleaseReason
        {
            get { return releaseReason; }
            set { releaseReason = value; }
        }
        public string ReleaseReason_Name
        {
            get { return releaseReason.Name; }
            set { releaseReason = ResolveCDO("ReleaseReason", value) as ReleaseReason; }
        }
        #endregion

        protected override bool ValidateService()
        {
            if (Container != null && Container.CurrentHoldCount == 0)
            {
                CompletionMessage = string.Format(MSG.Container_Is_Hold, Container.CurrentHoldCount);
                return false;
            }
            return true;
        }

        protected override bool ModifyEntity()
        {
            bool success = base.ModifyEntity();
            Container.CurrentHoldCount -= 1;
            CompletionMessage = string.Format(MSG.Release_Success, Container.ContainerName);

            return success;
        }

        protected override bool RecordServiceHistory()
        {
            // Define Transaction History Object
            HoldReleaseHistory existHoldHistory =ResolveHoldHistory(Container);
            HoldReleaseHistory newReleaseHistory = new HoldReleaseHistory();
            AssignToHoldReleaseHistory(existHoldHistory,newReleaseHistory);
            ObjScope.Add(newReleaseHistory);
            return true;
        }

        private const string queryHoldHistory =
@"SELECT * FROM HoldReleaseHistoryExtent AS o
WHERE o.HistoryMainLine.Container=$1 And o.ReleaseHistory=nil";

        protected HoldReleaseHistory ResolveHoldHistory(Container co)
        {
            object res = null;
            IQuery oqlQuery = ObjScope.GetOqlQuery(queryHoldHistory);
            IQueryResult result = oqlQuery.Execute(co);
            if (result.Count > 0)
                res = result[0];
            result.Dispose();
            return res as HoldReleaseHistory;
        }

        #region AssignTo
        protected virtual void AssignToHoldReleaseHistory(HoldReleaseHistory existHoldHistory, HoldReleaseHistory releaseHistory)
        {
            releaseHistory.HistoryMainLine = this.HistoryMainLine;
            releaseHistory.RelaseReason = this.ReleaseReason;
            releaseHistory.ReleaseTime = this.HistoryMainLine.TxnDate;
            existHoldHistory.ReleaseHistory = releaseHistory;               
        }
        #endregion 

        public Release() : base() { }
        public Release(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }
}
