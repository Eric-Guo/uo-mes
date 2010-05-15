using System;
using System.Collections.Generic;
using System.Text;
using UO_Service.Base;
using UO_Model;
using UO_Model.Base;
using UO_Model.Process;

namespace UO_Service.Maint
{
    public class SpecMaint : RevisionedObjectMaint
    {
        public override bool Add(RevisionedObject rdo_obj)
        {
            bool success = base.Add(rdo_obj);
            if (true == success)
            {
                success = RecordHistory(rdo_obj, ActionType.Insert);
            }
            return success;
        }

        protected override bool Update(RevisionedObject rdo_changes, out RevisionedObject to_change)
        {
            bool success = base.Update(rdo_changes, out to_change);
            if (true == success)
            {
                success = RecordHistory(to_change, ActionType.Update);
            }
            return success;
        }

        public override bool Remove(RevisionedObject rdo_obj)
        {
            bool success = base.Remove(rdo_obj);
            if (true == success)
            {
                success = RecordHistory(rdo_obj, ActionType.Delete);
            }
            return success;
        }

        private bool RecordHistory(RevisionedObject rdo_obj, ActionType actionType)
        {
            Spec o = rdo_obj as Spec;
            SpecChangeHistory h;
            if (actionType == ActionType.Delete)
                h = new SpecChangeHistory(null, this.TxnDate, actionType);
            else
                h = new SpecChangeHistory(rdo_obj, this.TxnDate, actionType);
            o.AssignToSpecChangeHistory(h);
            ObjScope.Transaction.Begin();
            ObjScope.Add(h);
            ObjScope.Transaction.Commit();
            return true;
        }
    }
}
