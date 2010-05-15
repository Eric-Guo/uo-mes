using System;
using System.Collections.Generic;
using System.Text;
using UO_Model.ServiceHistory;
using UO_Service.Base;
using UO_Model.Execution;

namespace UO_Service.ContainerTxn
{
    public class Associate : ContainerTxn
    {
        protected override string TxnName { get { return "Associate"; } }

        #region Associate transaction public properties for users
        #endregion

        private int detailIDCount = 0;
        private IList<AssociateDetail> associateDetails = new List<AssociateDetail>();
        private IList<AssociateDetail> AssociateDetails
        {
            get { return associateDetails; }
        }
        #region Method to support ObjectDataSource for associateDetails
        public IList<AssociateDetail> SelectDetails()
        {
            return AssociateDetails;
        }
        public void InsertDetail(AssociateDetail s)
        {
            s.ServiceDetailID = detailIDCount++;
            AssociateDetails.Add(s);
        }
        public void DeleteDetail(AssociateDetail s)
        {
            int i = -1;
            foreach (AssociateDetail t in AssociateDetails)
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    i = AssociateDetails.IndexOf(t);
                    break;
                }
            if (-1 != i)
                AssociateDetails.RemoveAt(i);
        }
        public void UpdateDetails(AssociateDetail s)
        {
            foreach (AssociateDetail t in AssociateDetails)
            {
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    AssignAssociateDetailToAssociateDetail(s, t);
                }
            }
        }
        #endregion

        protected override bool ModifyEntity()
        {
            bool success = base.ModifyEntity();

            CurrentStatus cs = Container.CurrentStatus;
            foreach (AssociateDetail d in AssociateDetails)
            {
                Container childContainer = ResolveContainer(d.ChildContainerName);
                this.Container.Qty += childContainer.Qty;
                childContainer.Parent = Container;
                CurrentStatus child_cs = childContainer.CurrentStatus;
                childContainer.CurrentStatus = Container.CurrentStatus;
                if (child_cs.Containers.Count == 0)
                    ObjScope.Remove(child_cs);
            }
            return success;
        }

        protected override bool RecordServiceHistory()
        {
            AssociateHistory h = new AssociateHistory(); ;
            AssignToAssociateHistory(h);
            foreach (AssociateDetail sd in AssociateDetails)
            {
                AssociateHistoryDetail hd = new AssociateHistoryDetail();
                AssignAssociateDetailToAssociateHistoryDetail(sd, hd);
                hd.ServiceHistorySummary = h;
                ObjScope.Add(hd);
            }
            ObjScope.Add(h);
            return true;
        }

        #region AssignTo
        protected virtual void AssignToAssociateHistory(AssociateHistory t)
        {
            t.HistoryMainLine = this.HistoryMainLine;
        }

        protected virtual void AssignAssociateDetailToAssociateDetail(AssociateDetail s, AssociateDetail t)
        {
            t.ChildContainerName = s.ChildContainerName;
        }

        protected virtual void AssignAssociateDetailToAssociateHistoryDetail(AssociateDetail s, AssociateHistoryDetail t)
        {
            t.ChildContainer = ResolveContainer(s.ChildContainerName);
        }
        #endregion

        public Associate() : base() { }
        public Associate(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }

    #region AssociateDetail's field must be all value type, no class type allowed to support ObjectDataSource
    public class AssociateDetail : ServiceDetail
    {
        private string childContainerName;
        public string ChildContainerName
        {
            get { return childContainerName; }
            set { childContainerName = value; }
        }
    }
    #endregion
}
