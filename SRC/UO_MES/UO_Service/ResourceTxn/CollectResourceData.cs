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

namespace UO_Service.ResourceTxn
{
    public class CollectResourceData : ResourceTxn
    {
        protected override string TxnName { get { return "CollectResourceData"; } }

        #region CollectResourceData transaction public properties for users
        #endregion

        private int detailIDCount = 0;
        private IList<ParametricDataDetail> details = new List<ParametricDataDetail>();
        private IList<ParametricDataDetail> Details
        {
            get { return details; }
        }

        #region Method to support ObjectDataSource for details
        public IList<ParametricDataDetail> SelectDetails()
        {
            return Details;
        }
        public void InsertDetail(ParametricDataDetail s)
        {
            s.ServiceDetailID = detailIDCount++;
            Details.Add(s);
        }
        public void DeleteDetail(ParametricDataDetail s)
        {
            int i = -1;
            foreach (ParametricDataDetail t in Details)
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    i = Details.IndexOf(t);
                    break;
                }
            if(-1 != i)
                Details.RemoveAt(i);
        }
        public void UpdateDetails(ParametricDataDetail s)
        {
            foreach (ParametricDataDetail t in Details)
                if (t.ServiceDetailID == s.ServiceDetailID)
                    AssignDetailToDetail(s, t);
        }
        #endregion

        protected override bool RecordServiceHistory()
        {
            CollectResDataHistory h = new CollectResDataHistory();
            AssignToCollectResourceDataHistory(h);
            foreach (ParametricDataDetail sd in Details)
            {
                CollectResDataHistoryDetail hd = new CollectResDataHistoryDetail();
                AssignDetailsToHistoryDetail(sd, hd);
                hd.ServiceHistorySummary = h;
                ObjScope.Add(hd);
            }
            ObjScope.Add(h);
            return true;
        }

        #region AssignTo

        protected virtual void AssignDetailToDetail(ParametricDataDetail f, ParametricDataDetail t)
        {
            t.DataCollectionDef_Name = f.DataCollectionDef_Name;
            t.DataCollectionValue = f.DataCollectionValue;
        }


        protected virtual void AssignToCollectResourceDataHistory(CollectResDataHistory t)
        {
            t.HistoryMainLine = this.HistoryMainLine;
            t.CollectDate = this.HistoryMainLine.TxnDate;

        }

        protected virtual void AssignDetailsToHistoryDetail(ParametricDataDetail s, CollectResDataHistoryDetail t)
        {
            t.DataCollectionDef = ResolveCDO("DataCollectionDef", s.DataCollectionDef_Name) as DataCollectionDef;
            t.DataCollectionValue = s.DataCollectionValue;
        }
        #endregion

        public CollectResourceData() : base() { }
        public CollectResourceData(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }

    #region ParametricDataDetail's field must be all value type, no class type allowed to support ObjectDataSource
    public class ParametricDataDetail : ServiceDetail
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