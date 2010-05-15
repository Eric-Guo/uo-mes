using System;
using System.Collections.Generic;
using System.Text;
using UO_Model.ServiceHistory;
using UO_Service.Base;
using UO_Model.Execution;

namespace UO_Service.ContainerTxn
{
    public class Combine : ContainerTxn
    {
        protected override string TxnName { get { return "Combine"; } }

        #region Combine transaction public properties for users
        private bool closeWhenEmpty = true;
        /// <summary>
        /// A flag that tells says whether to close source Containers when they become empty.
        /// Can be overridden by the detail CDOs if set to false.
        /// </summary>
        public bool CloseWhenEmpty
        {
            get { return closeWhenEmpty; }
            set { closeWhenEmpty = value; }
        }
        #endregion

        private int detailIDCount = 0;
        private IList<CombineFromDetail> fromContainerDetails = new List<CombineFromDetail>();
        private IList<CombineFromDetail> FromContainerDetails
        {
            get { return fromContainerDetails; }
        }
        #region Method to support ObjectDataSource for fromContainerDetails
        public IList<CombineFromDetail> SelectDetails()
        {
            return FromContainerDetails;
        }
        public void InsertDetail(CombineFromDetail s)
        {
            s.ServiceDetailID = detailIDCount++;
            FromContainerDetails.Add(s);
        }
        public void DeleteDetail(CombineFromDetail s)
        {
            int i = -1;
            foreach (CombineFromDetail t in FromContainerDetails)
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    i = FromContainerDetails.IndexOf(t);
                    break;
                }
            if (-1 != i)
                FromContainerDetails.RemoveAt(i);
        }
        public void UpdateDetails(CombineFromDetail s)
        {
            foreach (CombineFromDetail t in FromContainerDetails)
            {
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    AssignCombineFromDetailToCombineFromDetail(s, t);
                }
            }
        }
        #endregion

        protected override bool ModifyEntity()
        {
            bool success = base.ModifyEntity();
            foreach (CombineFromDetail d in FromContainerDetails)
            {
                UO_Model.Execution.Container fromContainer = ResolveContainer(d.FromContainerName);
                if (true == d.CombineAllQty)
                {
                    this.Container.Qty += fromContainer.Qty;
                    fromContainer.Qty = 0;
                }
                else
                {
                    this.Container.Qty += d.Qty;
                    fromContainer.Qty -= d.Qty;
                }
                if (true == this.CloseWhenEmpty && true == d.CloseWhenEmpty)
                {
                    CloseContainerWhenEmpty(fromContainer);
                }
            }
            return success;
        }

        protected override bool RecordServiceHistory()
        {
            CombineHistory h = new CombineHistory();
            AssignToCombineHistory(h);
            foreach (CombineFromDetail hd in FromContainerDetails)
            {
                CombineHistoryDetail shd = new CombineHistoryDetail();
                AssignCombineDetailsToCombineHistoryDetail(hd, shd);
                shd.ServiceHistorySummary = h;
                ObjScope.Add(shd);
            }
            ObjScope.Add(h);
            return true;
        }

        #region AssignTo
        protected virtual void AssignCombineFromDetailToCombineFromDetail(CombineFromDetail s, CombineFromDetail t)
        {
            t.CloseWhenEmpty = s.CloseWhenEmpty;
            t.CombineAllQty = s.CombineAllQty;
            t.FromContainerName = s.FromContainerName;
            t.Qty = s.Qty;
        }

        protected virtual void AssignToCombineHistory(CombineHistory t)
        {
            t.HistoryMainLine = this.HistoryMainLine;
            t.CloseWhenEmpty = this.CloseWhenEmpty;
        }

        protected virtual void AssignCombineDetailsToCombineHistoryDetail(CombineFromDetail s, CombineHistoryDetail t)
        {
            t.CloseWhenEmpty = s.CloseWhenEmpty;
            t.CombineAllQty = s.CombineAllQty;
            t.FromContainerName = s.FromContainerName;
            t.Qty = s.Qty;
        }
        #endregion

        public Combine() : base() { }
        public Combine(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }

    #region CombineFromDetail's field must be all value type, no class type allowed to support ObjectDataSource
    public class CombineFromDetail : ServiceDetail
    {
        private bool closeWhenEmpty = true;
        public bool CloseWhenEmpty
        {
            get { return closeWhenEmpty; }
            set { closeWhenEmpty = value; }
        }

        private bool combineAllQty = false;
        public bool CombineAllQty
        {
            get { return combineAllQty; }
            set { combineAllQty = value; }
        }

        private string fromContainerName;
        public string FromContainerName
        {
            get { return fromContainerName; }
            set { fromContainerName = value; }
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
