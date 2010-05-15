using System.Collections.Generic;
using UO_Model.Execution;
using UO_Model.ServiceHistory;
using UO_Service.Base;

namespace UO_Service.ContainerTxn
{
    public class Split : ContainerTxn
    {
        protected override string TxnName { get { return "Split"; } }

        #region Split transaction public properties for users
        private bool closeWhenEmpty = true;
        public bool CloseWhenEmpty
        {
            get { return closeWhenEmpty; }
            set { closeWhenEmpty = value; }
        }
        #endregion

        private int detailIDCount = 0;
        private IList<SplitDetail> toContainerDetails = new List<SplitDetail>();
        private IList<SplitDetail> ToContainerDetails
        {
            get { return toContainerDetails; }
        }
        #region Method to support ObjectDataSource for details
        public IList<SplitDetail> SelectDetails()
        {
            return ToContainerDetails;
        }

        public void InsertDetail(SplitDetail s)
        {
            s.ServiceDetailID = detailIDCount++;
            ToContainerDetails.Add(s);
        }

        public void DeleteDetail(SplitDetail s)
        {
            int i = -1;
            foreach (SplitDetail t in ToContainerDetails)
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    i = ToContainerDetails.IndexOf(t);
                    break;
                }
            if (-1 != i)
                ToContainerDetails.RemoveAt(i);
        }

        public void UpdateDetails(SplitDetail s)
        {
            foreach (SplitDetail t in ToContainerDetails)
            {
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    AssignSplitDetailToSplitDetail(s, t);
                }
            }
        }
        #endregion

        protected override bool ModifyEntity()
        {
            bool success = base.ModifyEntity();
            foreach (SplitDetail d in ToContainerDetails)
            {
                UO_Model.Execution.Container co = new UO_Model.Execution.Container();
                Container.AssignToContainer(co);
                co.ContainerName = d.ContainerName;
                co.Qty = d.Qty;
                Container.Qty -= d.Qty;
                ObjScope.Add(co);
            }
            if (true == CloseWhenEmpty)
                CloseContainerWhenEmpty(this.Container);
            return success;
        }

        protected override bool RecordServiceHistory()
        {
            SplitHistory h = new SplitHistory();
            AssignToSplitHistory(h);
            foreach (SplitDetail sd in ToContainerDetails)
            {
                SplitHistoryDetail hd = new SplitHistoryDetail();
                AssignSplitDetailsToSplitHistoryDetail(sd, hd);
                hd.ServiceHistorySummary = h;
                ObjScope.Add(hd);
            }
            ObjScope.Add(h);
            return true;
        }

        #region AssignTo
        protected virtual void AssignSplitDetailToSplitDetail(SplitDetail s, SplitDetail t)
        {
            t.ContainerName = s.ContainerName;
            t.Qty = s.Qty;
        }

        protected virtual void AssignToSplitHistory(SplitHistory t)
        {
            t.HistoryMainLine = this.HistoryMainLine;
            t.CloseWhenEmpty = this.CloseWhenEmpty;
        }

        protected virtual void AssignSplitDetailsToSplitHistoryDetail(SplitDetail s, SplitHistoryDetail t)
        {
            t.ContainerName = s.ContainerName;
            t.Qty = s.Qty;
        }
        #endregion

        public Split() : base() { }
        public Split(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }

    #region SplitDetail's field must be all value type, no class type allowed to support ObjectDataSource
    public class SplitDetail : ServiceDetail
    {
        private string containerName;
        public string ContainerName
        {
            get { return containerName; }
            set { containerName = value; }
        }

        private int qty = 0;
        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }
    }
    #endregion
}
