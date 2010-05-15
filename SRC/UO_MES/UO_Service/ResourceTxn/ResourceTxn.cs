using UO_Service.Base;
using UO_Model.Execution;
using UO_Model.Physical;
using UO_Model.ServiceHistory;
using UO_Service.Resx;

namespace UO_Service.ResourceTxn
{
    abstract public class ResourceTxn : UO_Service.Base.ShopFloor
    {
        #region ShopFloor transaction public properties for users
        private Resource resource;
        virtual public Resource Resource
        {
            get { return resource; }
            set { resource = ResolveCDO(value.GetType(), value.Name) as Resource; }
        }
        virtual public string Resource_Name
        {
            get { return resource.DisplayName; }
            set { resource = ResolveCDO("Resource", value) as Resource; }
        }

        public override Factory Factory
        {
            get
            {
                if (base.Factory == null && Resource != null)
                    base.Factory = Resource.Factory;
                return base.Factory;
            }
        }
        #endregion

        protected override bool ValidateService()
        {
            if (Resource == null)
            {
                CompletionMessage = MSG.Resource_Is_Need;
                return false;
            }
            return true;            
        }

        protected override bool ModifyEntity()
        {
            Resource.LastActivityDate = HistoryMainLine.TxnDate;
            return true;
        }

        protected ResourceProductionInfo ResourceProductionInfo
        {
            get
            {
                if (null == Resource.ResourceProductionInfo)
                {
                    ResourceProductionInfo rpi = new ResourceProductionInfo();
                    Resource.ResourceProductionInfo = rpi;
                    ObjScope.Add(rpi);
                }
                return Resource.ResourceProductionInfo;
            }
        }

        #region AssignTo
        protected override bool AssignToHistoryMainLine(HistoryMainLine h)
        {
            bool sucess = base.AssignToHistoryMainLine(h);
            h.Resource = this.Resource;
            return sucess;
        }
        #endregion

        protected ResourceTxn() : base() { }
        protected ResourceTxn(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }
    }
}
