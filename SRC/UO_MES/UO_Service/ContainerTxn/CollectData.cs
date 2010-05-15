using System;
using UO_Model.Execution;
using UO_Model.Process;
using UO_Model.Workflow;
using UO_Service.Base;
using UO_Model.Base;
using System.Collections.Generic;
using UO_Model.ServiceHistory;
using UO_Model.Execution.Reason;
using UO_Model.Execution.Status;

namespace UO_Service.ContainerTxn
{
    public class CollectData : ContainerTxn
    {
        protected override string TxnName { get { return "CollectData"; } }

        #region CollectData transaction public properties for users
        #endregion

        private int detailIDCount = 0;
        private IList<CollectDataDetail> details = new List<CollectDataDetail>();
        private IList<CollectDataDetail> Details
        {
            get { return details; }
        }
        #region Method to support ObjectDataSource for details
        public IList<CollectDataDetail> SelectDetails()
        {
            return Details;
        }
        public void InsertDetail(CollectDataDetail s)
        {
            s.ServiceDetailID = detailIDCount++;
            Details.Add(s);
        }
        public void DeleteDetail(CollectDataDetail s)
        {
            int i = -1;
            foreach (CollectDataDetail t in Details)
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    i = Details.IndexOf(t);
                    break;
                }
            if(-1 != i)
                Details.RemoveAt(i);
        }
        public void UpdateDetails(CollectDataDetail s)
        {
            foreach (CollectDataDetail t in Details)
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
            CollectDataHistory h = new CollectDataHistory();
            AssignToCollectDataHistory(h);
            foreach (CollectDataDetail sd in Details)
            {
                CollectDataHistoryDetail hd = new CollectDataHistoryDetail();
                AssignDetailsToHistoryDetail(sd, hd);
                hd.ServiceHistorySummary = h;
                ObjScope.Add(hd);
            }
            ObjScope.Add(h);
            return true;
        }

        #region AssignTo
        protected virtual void AssignDetailToDetail(CollectDataDetail f, CollectDataDetail t)
        {
            t.DataCollectionDef_Name = f.DataCollectionDef_Name;
            t.DataCollectionValue = f.DataCollectionValue;
        }


        protected virtual void AssignToCollectDataHistory(CollectDataHistory t)
        {
            t.HistoryMainLine = this.HistoryMainLine;
            t.CollectDate = this.HistoryMainLine.TxnDate;
        }

        protected virtual void AssignDetailsToHistoryDetail(CollectDataDetail s, CollectDataHistoryDetail t)
        {
            t.DataCollectionDef = ResolveCDO("DataCollectionDef", s.DataCollectionDef_Name) as DataCollectionDef;
            t.DataCollectionValue = s.DataCollectionValue;
        }
        #endregion

        public CollectData() : base() { }
        public CollectData(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }

    #region CollectDataDetail's field must be all value type, no class type allowed to support ObjectDataSource
    public class CollectDataDetail : ServiceDetail
    {
        private string dataCollectionDef_Name;
        public string DataCollectionDef_Name
        {
            get { return dataCollectionDef_Name; }
            set { dataCollectionDef_Name = value; }
        }

        private string dataCollectionValue;
        public string DataCollectionValue
        {
            get { return dataCollectionValue; }
            set { dataCollectionValue = value; }
        }
    }
    #endregion
}