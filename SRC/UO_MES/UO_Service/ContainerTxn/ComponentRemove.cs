using System;
using System.Collections.Generic;
using System.Text;
using UO_Model.ServiceHistory;
using UO_Model.Execution;
using UO_Model.Process;
using UO_Model.Process.Reason;
using UO_Service.Base;

namespace UO_Service.ContainerTxn
{
    public class ComponentRemove : ContainerTxn
    {
        protected override string TxnName { get { return "ComponentRemove"; } }

        #region ComponentRemove transaction public properties for users
        #endregion

        private int detailIDCount = 0;
        private IList<RemoveDetail> details = new List<RemoveDetail>();
        private IList<RemoveDetail> Details
        {
            get { return details; }
        }
        #region Method to support ObjectDataSource for details
        public IList<RemoveDetail> SelectDetails()
        {
            return Details;
        }
        public void InsertDetail(RemoveDetail s)
        {
            s.ServiceDetailID = detailIDCount++;
            Details.Add(s);
        }
        public void DeleteDetail(RemoveDetail s)
        {
            int i = -1;
            foreach (RemoveDetail t in Details)
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    i = Details.IndexOf(t);
                    break;
                }
            if(-1 != i)
                Details.RemoveAt(i);
        }
        public void UpdateDetails(RemoveDetail s)
        {
            foreach (RemoveDetail t in Details)
            {
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    AssignDetailToDetail(s, t);
                }
            }
        }
        #endregion

        protected override bool RecordServiceHistory()
        {
            ComponentRemoveHistory h = new ComponentRemoveHistory();
            AssignToComponentRemoveHistory(h);
            foreach (RemoveDetail sd in Details)
            {
                ComponentRemoveHistoryDetail hd = new ComponentRemoveHistoryDetail();
                AssignDetailsToHistoryDetail(sd, hd);
                hd.ServiceHistorySummary = h;
                ObjScope.Add(hd);
            }
            ObjScope.Add(h);
            return true;
        }

        #region AssignTo
        protected virtual void AssignDetailToDetail(RemoveDetail f, RemoveDetail t)
        {
            t.IssueControl = f.IssueControl;
            t.Product_Name = f.Product_Name;
            t.DestinationContainer_Name=f.DestinationContainer_Name;
            t.QtyRemoved = f.QtyRemoved;
            t.RemoveReason_Name =f.RemoveReason_Name;
        }


        protected virtual void AssignToComponentRemoveHistory(ComponentRemoveHistory t)
        {
            t.HistoryMainLine = this.HistoryMainLine;
            t.ComponentRemoveDate= this.HistoryMainLine.TxnDate;

        }

        protected virtual void AssignDetailsToHistoryDetail(RemoveDetail s, ComponentRemoveHistoryDetail t)
        {
            t.IssueControl = s.IssueControl;
            t.Product = ResolveCDO("Product", s.Product_Name) as Product;
            t.DestinationContainer = ResolveContainer(s.DestinationContainer_Name);
            t.QtyRemoved = s.QtyRemoved;
            t.RemoveReason = ResolveCDO("RemoveReason", s.RemoveReason_Name) as RemoveReason;
        }
        #endregion

        public ComponentRemove() : base() { }
        public ComponentRemove(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }

    #region RemoveDetail's field must be all value type, no class type allowed to support ObjectDataSource
    public class RemoveDetail : ServiceDetail
    {
        private string product_Name;
        public string Product_Name
        {
            get { return product_Name; }
            set { product_Name = value; }
        }

        private string destinationContainer_Name;
        public string DestinationContainer_Name
        {
            get { return destinationContainer_Name; }
            set { destinationContainer_Name = value; }
        }


        private int qtyRemoved;
        public int QtyRemoved
        {
            get { return qtyRemoved; }
            set { qtyRemoved = value; }
        }

        private int issueControl;
        public int IssueControl
        {
            get { return issueControl; }
            set { issueControl = value; }
        }

        private string removeReason_Name;
        public string RemoveReason_Name
        {
            get { return removeReason_Name; }
            set { removeReason_Name = value; }
        }

        private bool removeAllQty;
        public bool RemoveAllQty
        {
            get { return removeAllQty; }
            set { removeAllQty = value; }
        }
    }
    #endregion
}