using System;
using System.Collections.Generic;
using System.Text;
using Telerik.OpenAccess;
using UO_Model.Base;
using System.Reflection;

namespace UO_Service.Base
{
    public class RevisionedObjectMaint : Service
    {
        virtual public bool Add(RevisionedObject rdo_insert)
        {
            string idProperty_Name = null;
            object check_exist = ResolveRDO(rdo_insert, out idProperty_Name);
            if (check_exist == null)
            {
                // Reuse exist RevBase if exist
                object rdo_base_exist = ResolveCDO(rdo_insert.RevBase.GetType(), rdo_insert.RevBase.Name);
                if (rdo_base_exist != null)
                    rdo_insert.RevBase = rdo_base_exist as RevisionBase;

                foreach (PropertyInfo pi in rdo_insert.GetType().GetProperties())
                {
                    if (pi.Name == idProperty_Name || pi.Name == "RevBase")
                        continue;   // do not update Primary Property and RevBase
                    object v_changes = pi.GetValue(rdo_insert, null);
                    if (v_changes != null && (pi.CanWrite || v_changes.GetType().IsGenericType))
                        SetProperty(rdo_insert, pi, v_changes);
                }

                ObjScope.Transaction.Begin();
                ObjScope.Add(rdo_insert);
                ObjScope.Transaction.Commit();
                return true;
            }
            else
                return false;
        }

        public bool Update(RevisionedObject rdo_changes)
        {
            RevisionedObject to_change = null;
            return Update(rdo_changes, out to_change);
        }

        virtual protected bool Update(RevisionedObject rdo_changes, out RevisionedObject to_change)
        {
            string idProperty_Name;
            to_change = ResolveRDO(rdo_changes, out idProperty_Name);

            if (to_change != null)
            {
                ObjScope.Transaction.Begin();
                foreach (PropertyInfo pi in rdo_changes.GetType().GetProperties())
                {
                    if (pi.Name == idProperty_Name || pi.Name == "RevBase")
                        continue;   // do not update Primary Property and RevBase
                    object v_changes = pi.GetValue(rdo_changes, null);
                    if (v_changes != null && (pi.CanWrite || v_changes.GetType().IsGenericType))
                        SetProperty(to_change, pi, v_changes);
                }
                ObjScope.Transaction.Commit();
                return true;
            }
            else
                return false;
        }

        virtual public bool Remove(RevisionedObject rdo_obj)
        {
            RevisionedObject to_delete = ResolveRDO(rdo_obj);
            if (to_delete != null)
            {
                ObjScope.Transaction.Begin();
                if (1 == RDOBaseUsageCount(to_delete))
                    ObjScope.Remove(to_delete.RevBase);
                ObjScope.Remove(to_delete);
                ObjScope.Transaction.Commit();
                return true;
            }
            else
                return false;
        }

        virtual public bool Dump(RevisionedObject rdo_obj)
        {
            object dump_obj = ResolveRDO(rdo_obj);
            if (dump_obj != null)
            {
                DumpCDOObject(dump_obj);
                return true;
            }
            else
                return false;
        }

        private int RDOBaseUsageCount(RevisionedObject rdo_obj)
        {
            const string queryRDOBaseUsageCount = "SELECT COUNT(*) FROM {0}Extent AS o WHERE o.RevBase = $1";
            int count = 0;
            IQuery oqlQuery = ObjScope.GetOqlQuery(string.Format(queryRDOBaseUsageCount,rdo_obj.GetType().Name));
            IQueryResult result = oqlQuery.Execute(rdo_obj.RevBase);
            if (result.Count > 0)
                count = (int)result[0];
            result.Dispose();
            return count;
        }
    }
}
