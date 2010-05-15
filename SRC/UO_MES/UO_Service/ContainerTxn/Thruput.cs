using UO_Model.Execution;
using UO_Model.Physical;
using UO_Model.ServiceHistory;

namespace UO_Service.ContainerTxn
{
    public class Thruput : ContainerTxn
    {
        protected override string TxnName { get { return "Thruput"; } }

        #region Thruput transaction public properties for users
        private bool thruputAllQty;
        public bool ThruputAllQty
        {
            get { return thruputAllQty; }
            set { thruputAllQty = value; }
        }

        private int qty;
        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        private Resource resource;
        public Resource Resource
        {
            get { return resource; }
            set { resource = ResolveCDO(value.GetType(), value.Name) as Resource; }
        }
        public string Resource_Name
        {
            get { return resource.DisplayName; }
            set { resource = ResolveCDO("Resource", value) as Resource; }
        }
        #endregion

        protected override bool ModifyEntity()
        {
            bool success = base.ModifyEntity();
            if (null == this.Resource)
                this.Resource = Container.CurrentStatus.Resource;
            else
            {
                CurrentStatus cs_new = GetCurrentStatusToModify();
                cs_new.Resource = this.Resource;
                ReplaceCurrentStatus(cs_new);
            }
            if (true == this.ThruputAllQty)
                this.Qty = this.Container.Qty - this.Container.CurrentThruputQty;
            this.Container.CurrentThruputQty += this.Qty;
            return success;
        }

        protected override bool RecordServiceHistory()
        {
            ThruputHistory h = new ThruputHistory();
            AssignToThruputHistory(h);
            ObjScope.Add(h);
            return true;
        }

        #region AssignTo
        protected virtual void AssignToThruputHistory(ThruputHistory t)
        {
            t.HistoryMainLine = this.HistoryMainLine;
            t.Container = this.Container;
            t.Resource = this.Resource;
            t.ThruputAllQty = this.ThruputAllQty;
            t.Qty = this.Qty;
            t.CycleTime = (this.TxnDate - this.Container.LastMoveDate).TotalHours;
        }
        #endregion

        public Thruput() : base() { }
        public Thruput(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }
}