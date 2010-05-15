using System;
using System.Collections.Generic;
using System.Text;
using UO_Model.ServiceHistory;
using UO_Service.Base;
using UO_Model.Execution;
namespace UO_Service.ContainerTxn
{
    public class Disassociate : ContainerTxn
    {
        protected override string TxnName { get { return "Disassociate"; } }

        #region Disassoicate transaction public properties for users
        public new Container Container
        {
            get { return base.Container; }
            set 
            { 
                base.Container = value;
                InsertFullDetails(Container);
            }
        }
        public new string Container_Name
        {
            get { return base.Container.ContainerName; }
            set 
            { 
                base.Container_Name = value;
                InsertFullDetails(Container);
            }
        }

        private bool disassociateAll = false;
        public bool DisassociateAll
        {
            get { return disassociateAll; }
            set { disassociateAll = value; }
        }
        #endregion

        private int detailIDCount = 0;
        private IList<DisassociateDetail> disassociateDetails = new List<DisassociateDetail>();
        private IList<DisassociateDetail> DisassociateDetails
        {
            get { return disassociateDetails; }
        }
        #region Method to support ObjectDataSource for DisassociateDetails
        public IList<DisassociateDetail> SelectDetails()
        {
            return DisassociateDetails;
        }
        public void InsertDetail(DisassociateDetail s)
        {
            s.ServiceDetailID = detailIDCount++;
            DisassociateDetails.Add(s);
        }
        public void DeleteDetail(DisassociateDetail s)
        {
            int i = -1;
            foreach (DisassociateDetail t in DisassociateDetails)
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    i = DisassociateDetails.IndexOf(t);
                    break;
                }
            if (-1 != i)
                DisassociateDetails.RemoveAt(i);
        }
        public void UpdateDetails(DisassociateDetail s)
        {
            foreach (DisassociateDetail t in DisassociateDetails)
            {
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    AssignDisassociateDetailToDisassociateDetail(s, t);
                }
            }
        }
        #endregion

        protected override bool ModifyEntity()
        {
            bool success = base.ModifyEntity();
            if (true == DisassociateAll)
            {
                DisassociateDetails.Clear();
                foreach (UO_Model.Execution.Container d in this.Container.ChildContainers)
                {
                    DisassociateDetail sd = new DisassociateDetail();
                    sd.ChildContainerName = d.ContainerName;
                    DisassociateDetails.Add(sd);
                }
            }
            foreach (DisassociateDetail d in DisassociateDetails)
            {
                Container childContainer = ResolveContainer(d.ChildContainerName);
                this.Container.Qty -= childContainer.Qty;
                childContainer.Parent = null;
            }
            return success;
        }

        protected override bool ValidateService()
        {
            return base.ValidateService();
        }

        protected override bool RecordServiceHistory()
        {
            DisassociateHistory h = new DisassociateHistory();
            AssignToDisassociateHistory(h);
            foreach (DisassociateDetail sd in DisassociateDetails)
            {
                DisassociateHistoryDetail hd = new DisassociateHistoryDetail();
                AssignDisassociateDetailToDisassociateHistoryDetail(sd, hd);
                hd.ServiceHistorySummary = h;
                ObjScope.Add(hd);
            }
            ObjScope.Add(h);
            return true;
        }

        #region AssignTo
        protected virtual void AssignToDisassociateHistory(DisassociateHistory t)
        {
            t.HistoryMainLine = this.HistoryMainLine;
            t.DisassociateAll = this.DisassociateAll;
        }

        protected virtual void AssignDisassociateDetailToDisassociateDetail(DisassociateDetail s, DisassociateDetail t)
        {
            t.ChildContainerName = s.ChildContainerName;
        }

        protected virtual void AssignDisassociateDetailToDisassociateHistoryDetail(DisassociateDetail s, DisassociateHistoryDetail t)
        {
            t.ChildContainer = ResolveContainer(s.ChildContainerName);
        }
        #endregion

        public Disassociate() : base() { }
        public Disassociate(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }

        private void InsertFullDetails(Container co)
        {
            foreach (Container child in co.ChildContainers)
            {
                DisassociateDetail sd = new DisassociateDetail();
                sd.ChildContainerName = child.ContainerName;
                InsertDetail(sd);
            }
        }
    }

    #region DisassociateDetail's field must be all value type, no class type allowed to support ObjectDataSource
    public class DisassociateDetail : ServiceDetail
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
