using Telerik.OpenAccess;
using UO_Model.Execution;
using UO_Model.ServiceHistory;
using UO_Model.Workflow;
using UO_Model.Base;
using UO_Model.Process;
using UO_Model.Physical.Reason;
using UO_Model.Physical.Status;

namespace UO_Service.ResourceTxn
{
    public class ResourceThruput : ResourceTxn
    {
        protected override string TxnName { get { return "ResourceThruput"; } }

        #region ResourceThruput transaction public properties for users
        private int qty;
        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        private UOM uom;
        public UOM UOM
        {
            get { return uom; }
            set { uom = ResolveCDO(value.GetType(), value.Name) as UOM; }
        }
        public string UOM_Name
        {
            get { return uom.Name; }
            set { uom = ResolveCDO("UOM", value) as UOM; }
        }

        private Setup setup;
        public Setup Setup
        {
            get { return setup; }
            set { setup = ResolveCDO(value.GetType(), value.DisplayName) as Setup; }
        }
        public string Setup_Revision
        {
            get { return setup.DisplayName; }
            set { setup = ResolveCDO("Setup", value) as Setup; }
        }

        private MfgOrder mfgOrder;
        public MfgOrder MfgOrder
        {
            get { return mfgOrder; }
            set { mfgOrder = ResolveCDO(value.GetType(), value.Name) as MfgOrder; }
        }
        public string MfgOrder_Name
        {
            get { return mfgOrder.Name; }
            set { mfgOrder = ResolveCDO("MfgOrder", value) as MfgOrder; }
        }

        private Product product;
        public Product Product
        {
            get { return product; }
            set { product = ResolveCDO(value.GetType(), value.DisplayName) as Product; }
        }
        public string Product_Revision
        {
            get { return product.DisplayName; }
            set { product = ResolveCDO("Product", value) as Product; }
        }
        #endregion

        protected override bool ModifyEntity()
        {
            bool success = base.ModifyEntity();
            ResourceStatus resourceStatus = new ResourceStatus();
            resourceStatus.Name = "Run";
            ResourceProductionInfo.ResourceStatus = ResolveCDO(resourceStatus.GetType(), resourceStatus.Name) as ResourceStatus;
            return success;
        }

        protected override bool RecordServiceHistory()
        {
            ResourceThruputHistory h = new ResourceThruputHistory();
            AssignToResourceThruputHistory(h);
            ObjScope.Add(h);
            return true;
        }

        #region AssignTo
        protected virtual void AssignToResourceThruputHistory(ResourceThruputHistory t)
        {
            t.HistoryMainLine = this.HistoryMainLine;
            t.Product = this.Product;
            t.MfgOrder = this.MfgOrder;
            t.Setup = this.Setup;
            t.Qty = this.Qty;
            t.UOM = this.UOM;
            t.ThruputDate = this.HistoryMainLine.TxnDate;
        }
        #endregion

        public ResourceThruput() : base() { }
        public ResourceThruput(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }
}