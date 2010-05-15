using System.Collections.Generic;
using UO_Model.Execution;
using UO_Model.Process;
using UO_Model.ServiceHistory;
using UO_Service.Base;

namespace UO_Service.ContainerTxn
{
    public class Serialize : ContainerTxn
    {
        protected override string TxnName { get { return "Serialize"; } }

        #region Serialize transaction public properties for users
        private ContainerLevel childContainerLevel;
        public ContainerLevel ChildContainerLevel
        {
            get { return childContainerLevel; }
            set { childContainerLevel = ResolveCDO(value.GetType(), value.Name) as ContainerLevel; }
        }
        public string ChildContainerLevel_Name
        {
            get { return childContainerLevel.DisplayName; }
            set { childContainerLevel = ResolveCDO("ContainerLevel", value) as ContainerLevel; }
        }

        private UOM childUOM;
        public UOM ChildUOM
        {
            get { return childUOM; }
            set { childUOM = ResolveCDO(value.GetType(), value.Name) as UOM; }
        }
        public string ChildUOM_Name
        {
            get { return childContainerLevel.DisplayName; }
            set { childUOM = ResolveCDO("UOM", value) as UOM; }
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

        private int detailIDCount = 0;
        private IList<SerializeDetail> serializeDetail = new List<SerializeDetail>();
        private IList<SerializeDetail> SerializeDetails
        {
            get { return serializeDetail; }
        }
        #region Method to support ObjectDataSource for details
        public IList<SerializeDetail> SelectDetails()
        {
            return SerializeDetails;
        }
        public void InsertDetail(SerializeDetail s)
        {
            s.ServiceDetailID = detailIDCount++;
            SerializeDetails.Add(s);
        }
        public void DeleteDetail(SerializeDetail s)
        {
            int i = -1;
            foreach (SerializeDetail t in SerializeDetails)
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    i = SerializeDetails.IndexOf(t);
                    break;
                }
            if(-1 != i)
                SerializeDetails.RemoveAt(i);
        }
        public void UpdateDetails(SerializeDetail s)
        {
            foreach (SerializeDetail t in SerializeDetails)
            {
                if (t.ServiceDetailID == s.ServiceDetailID)
                {
                    AssignSerializeDetailToSerializeDetail(s, t);
                }
            }
        }
        #endregion

        protected override bool ValidateService()
        {
            //The container must be active.

            //The container must be a unit container (that is, ChildCount should be equal to zero).
            if (Container != null && Container.ChildContainers.Count > 0)
            {
                CompletionMessage = string.Format("Container is not unit and already has ChildContainers");
                return false;
            }

            //3. The container must not be on hold.
            if (Container != null && Container.CurrentHoldCount > 0)
            {
                CompletionMessage = string.Format("Container is currently hold, hold count: {0}", Container.CurrentHoldCount);
                return false;
            }

            //4. The container must not be in Rework.

            //5. No Thruput must have been done for this container at this step.
            if (Container != null && Container.CurrentThruputQty > 0)
            {
                CompletionMessage = string.Format("Container Thruput Has Been Done");
                return false;
            }

            //6. ChildCount must be equal to ActualChildCount,which is the total number of ContainerNames in all the SerializeDetails created.

            return true;
        }

        protected override bool ModifyEntity()
        {
            bool success = base.ModifyEntity();

            foreach (SerializeDetail d in serializeDetail)
            {
                UO_Model.Execution.Container co = new UO_Model.Execution.Container();
                Container.AssignToContainer(co);
                AssignSerializeDetailsToChildContainer(d, co);
                Container.Qty -= d.ChildQty;
                ObjScope.Add(co);
            }
            
            return success;
        }

        protected override bool RecordServiceHistory()
        {
            SerializeHistory h = new SerializeHistory();
            AssignToSerializeHistory(h);
            foreach (SerializeDetail d in SerializeDetails)
            {
                SerializeHistoryDetail hd = new SerializeHistoryDetail();
                AssignSerializeDetailsToSerializeHistoryDetail(d, hd);
                hd.ServiceHistorySummary = h;
                ObjScope.Add(hd);
            }
            ObjScope.Add(h);
            return true;
        }

        #region AssignTo
        protected virtual void AssignSerializeDetailsToChildContainer(SerializeDetail s, Container t)
        {
            t.ContainerName = s.ChildContainerName;
            t.Qty = s.ChildQty;
            t.Parent = this.Container;
        }

        protected virtual void AssignSerializeDetailToSerializeDetail(SerializeDetail s, SerializeDetail t)
        {
            t.ChildContainerName = s.ChildContainerName;
            t.ChildQty = s.ChildQty;
        }

        protected virtual void AssignToSerializeHistory(SerializeHistory t)
        {
            t.HistoryMainLine = this.HistoryMainLine;
            t.ChildContainerLevel = this.ChildContainerLevel;
            t.ChildUOM = this.ChildUOM;
            t.Product = this.Product;
        }

        protected virtual void AssignSerializeDetailsToSerializeHistoryDetail(SerializeDetail s, SerializeHistoryDetail t)
        {
            t.ChildContainerName = s.ChildContainerName;
            t.ChildQty = s.ChildQty;
        }
        #endregion

        public Serialize() : base() { }
        public Serialize(Telerik.OpenAccess.IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope, reusedHML) { }    
    }

    #region SerializeDetail's field must be all value type, no class type allowed to support ObjectDataSource
    public class SerializeDetail : ServiceDetail
    {
        private string childContainerName;
        public string ChildContainerName
        {
            get { return childContainerName; }
            set { childContainerName = value; }
        }

        private int childQty = 1;
        public int ChildQty
        {
            get { return childQty; }
            set { childQty = value; }
        }
    }
    #endregion
}
