using System.Collections.Generic;
using UO_Model.ServiceHistory;
using UO_Service.Base;
using UO_Model.Execution.Reason;
using UO_Model.Execution;

namespace UO_Service.ContainerTxn
{
    public class Defect : ContainerTxn
    {
        protected override string TxnName { get { return "Defect"; } }

        #region Defect transaction public properties for users
        private int qtyInspected;
        public int QtyInspected
        {
            get { return qtyInspected; }
            set { qtyInspected = value; }
        }
        #endregion

        private int detailIDCount = 0;
        private IList<DefectDetail> defectDetails = new List<DefectDetail>();
        private IList<DefectDetail> DefectDetails
        {
            get { return defectDetails; }
        }
        #region Method to support ObjectDataSource for details
        public IList<DefectDetail> SelectDetails()
        {
            return DefectDetails;
        }

        public void InsertDetail(DefectDetail s)
        {
            s.ServiceDetailID = detailIDCount++;
            DefectDetails.Add(s);
        }

        public void DeleteDetail(DefectDetail s)
        {
            int i = -1;
            foreach (DefectDetail t in DefectDetails)
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    i = DefectDetails.IndexOf(t);
                    break;
                }
            if (-1 != i)
                DefectDetails.RemoveAt(i);
        }

        public void UpdateDetails(DefectDetail s)
        {
            foreach (DefectDetail t in DefectDetails)
            {
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    AssignDefectDetailToDefectDetail(s, t);
                }
            }
        }
        #endregion

        protected override bool ModifyEntity()
        {
            bool success = base.ModifyEntity();
            foreach (DefectDetail d in DefectDetails)
            {
                this.Container.Qty -= d.DefectQty;
            }
            return success;
        }

        protected override bool RecordServiceHistory()
        {
            DefectHistory h = new DefectHistory();
            AssignToDefectHistory(h);
            foreach (DefectDetail d in DefectDetails)
            {
                DefectHistoryDetail hd = new DefectHistoryDetail();
                AssignDefectDetailsToDefectHistoryDetail(d, hd);
                hd.ServiceHistorySummary = h;
                ObjScope.Add(hd);
            }
            ObjScope.Add(h);
            return true;
        }

        #region AssignTo
        protected virtual void AssignDefectDetailToDefectDetail(DefectDetail s, DefectDetail t)
        {
            t.DefectQty = s.DefectQty;
            t.DefectReason_Name = t.DefectReason_Name;
            t.Comment = s.Comment;
        }

        protected virtual void AssignToDefectHistory(DefectHistory t)
        {
            t.HistoryMainLine = this.HistoryMainLine;
            t.QtyInspected = this.QtyInspected;
        }

        protected virtual void AssignDefectDetailsToDefectHistoryDetail(DefectDetail s, DefectHistoryDetail t)
        {
            t.DefectQty = s.DefectQty;
            t.DefectReason = ResolveCDO("DefectReason", s.DefectReason_Name) as DefectReason;
            t.Comment = s.Comment;
        }
        #endregion

        public Defect() : base() { }
        public Defect(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }

    #region DefectDetails's field must be all value type, no class type allowed to support ObjectDataSource
    public class DefectDetail : ServiceDetail
    {
        private int defectQty = 0;
        public int DefectQty
        {
            get { return defectQty; }
            set { defectQty = value; }
        }

        private string defectReason_Name;
        public string DefectReason_Name
        {
            get { return defectReason_Name; }
            set { defectReason_Name = value; }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }
    }
    #endregion
}
