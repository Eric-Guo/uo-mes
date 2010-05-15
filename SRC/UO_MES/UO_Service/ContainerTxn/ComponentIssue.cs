using System;
using System.Collections.Generic;
using System.Text;
using UO_Model.ServiceHistory;
using UO_Service.Base;
using UO_Model.Process;
using UO_Model.Process.Reason;
using UO_Model.Execution;

namespace UO_Service.ContainerTxn
{
    public class ComponentIssue : ContainerTxn
    {
        protected override string TxnName { get { return "ComponentIssue"; } }

        #region ComponentIssue transaction public properties for users
        #endregion

        private int detailIDCount = 0;
        private IList<ComponentIssueDetail> details = new List<ComponentIssueDetail>();
        private IList<ComponentIssueDetail> Details
        {
            get { return details; }
        }
        #region Method to support ObjectDataSource for details
        public IList<ComponentIssueDetail> SelectDetails()
        {
            return Details;
        }
        public void InsertDetail(ComponentIssueDetail s)
        {
            s.ServiceDetailID = detailIDCount++;
            Details.Add(s);
        }
        public void DeleteDetail(ComponentIssueDetail s)
        {
            int i = -1;
            foreach (ComponentIssueDetail t in Details)
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    i = Details.IndexOf(t);
                    break;
                }
            if(-1 != i)
                Details.RemoveAt(i);
        }
        public void UpdateDetails(ComponentIssueDetail s)
        {
            foreach (ComponentIssueDetail t in Details)
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
            ComponentIssueHistory h = new ComponentIssueHistory();
            AssignToComponentIssueHistory(h);
            foreach (ComponentIssueDetail sd in Details)
            {
                ComponentIssueHistoryDetail hd = new ComponentIssueHistoryDetail();
                AssignDetailsToHistoryDetail(sd, hd);
                hd.ServiceHistorySummary = h;
                ObjScope.Add(hd);
            }
            ObjScope.Add(h);
            return true;
        }

        #region AssignTo
        protected virtual void AssignDetailToDetail(ComponentIssueDetail f, ComponentIssueDetail t)
        {
            t.IssueControl = f.IssueControl;
            t.Product_Name=f.Product_Name;
            t.QtyRequired = f.QtyRequired;
            t.ActualQtyIssued = f.ActualQtyIssued;
            t.IssueReason_Name= f.IssueReason_Name;
            t.FromLocation = f.FromLocation;
            t.ToLocation = f.ToLocation;            
        }

        protected virtual void AssignToComponentIssueHistory(ComponentIssueHistory t)
        {
            t.HistoryMainLine = this.HistoryMainLine;
            t.ComponentIssueDate = this.HistoryMainLine.TxnDate;

        }

        protected virtual void AssignDetailsToHistoryDetail(ComponentIssueDetail s, ComponentIssueHistoryDetail t)
        {
            t.IssueControl = s.IssueControl;
            t.Product = ResolveCDO("Product", s.Product_Name) as Product;
            t.QtyRequired = s.QtyRequired;
            t.ActualQtyIssued = s.ActualQtyIssued;
            t.IssueReason = ResolveCDO("IssueReason", s.IssueReason_Name) as IssueReason;
            t.Container = ResolveContainer(s.Container_Name) as Container;
            t.FromLocation = s.FromLocation;
            t.ToLocation = s.ToLocation;
        }
        #endregion

        public ComponentIssue() : base() { }
        public ComponentIssue(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }

    #region ComponentIssueDetail's field must be all value type, no class type allowed to support ObjectDataSource
    public class ComponentIssueDetail : ServiceDetail
    {
        private string product_Name;
        public string Product_Name
        {
            get { return product_Name; }
            set { product_Name = value; }
        }

        private int actualQtyIssued;
        public int ActualQtyIssued
        {
            get { return actualQtyIssued; }
            set { actualQtyIssued = value; }
        }

        private int qtyRequired;
        public int QtyRequired
        {
            get { return qtyRequired; }
            set { qtyRequired = value; }
        }

        private int issueControl;
        public int IssueControl
        {
            get { return issueControl; }
            set { issueControl = value; }
        }

        private string issueReason_Name;
        public string IssueReason_Name
        {
            get { return issueReason_Name; }
            set { issueReason_Name = value; }
        }

        private string fromLocation;
        public string FromLocation
        {
            get { return fromLocation; }
            set { fromLocation = value; }
        }

        private string container_Name;
        public string Container_Name
        {
            get { return container_Name; }
            set { container_Name = value; }
        }

        private string toLocation;
        public string ToLocation
        {
            get { return toLocation; }
            set { toLocation = value; }
        }
    }
    #endregion
}