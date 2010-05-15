using System;
using System.Collections.Generic;
using System.Text;
using UO_Service.Base;
using UO_Model.Process;
using UO_Model;
using UO_Model.Base;
using System.ServiceModel;

namespace UO_Service.Maint
{
    #region IMfgOrderMaint
    [ServiceContract]
    public interface IMfgOrderMaint
    {
        [OperationContract]
        bool Add(MfgOrder mfgOrder);
        [OperationContract]
        bool Update(MfgOrder mfgOrder);
        [OperationContract]
        bool Remove(MfgOrder mfgOrder);
    }
    #endregion

    public class MfgOrderMaint : NamedObjectMaint, IMfgOrderMaint
    {
        #region IMfgOrderMaint implement
        bool IMfgOrderMaint.Add(MfgOrder mfgOrder) { return Add(mfgOrder); }
        bool IMfgOrderMaint.Update(MfgOrder mfgOrder) { return Update(mfgOrder); }
        bool IMfgOrderMaint.Remove(MfgOrder mfgOrder) { return Remove(mfgOrder); }
        #endregion

        public override bool Add(NamedObject ndo_obj)
        {
            bool success = base.Add(ndo_obj);
            if (true == success)
            {
                success = RecordHistory(ndo_obj, ActionType.Insert);
            }
            return success;
        }

        protected override bool Update(NamedObject ndo_changes, out NamedObject to_change)
        {
            bool success = base.Update(ndo_changes, out to_change);
            if (true == success)
            {
                success = RecordHistory(to_change, ActionType.Update);
            }
            return success;
        }

        public override bool Remove(NamedObject ndo_obj)
        {
            bool success = base.Remove(ndo_obj);
            if (true == success)
            {
                success = RecordHistory(ndo_obj, ActionType.Delete);
            }
            return success;
        }

        private bool RecordHistory(NamedObject ndo_obj, ActionType actionType)
        {
            MfgOrder o = ndo_obj as MfgOrder;
            MfgOrderChangeHistory h;
            if (actionType == ActionType.Delete)
                h = new MfgOrderChangeHistory(null, this.TxnDate, actionType);
            else
                h = new MfgOrderChangeHistory(ndo_obj, this.TxnDate, actionType);
            o.AssignToMfgOrderChangeHistory(h);
            ObjScope.Transaction.Begin();
            ObjScope.Add(h);
            ObjScope.Transaction.Commit();
            return true;
        }
    }
}
