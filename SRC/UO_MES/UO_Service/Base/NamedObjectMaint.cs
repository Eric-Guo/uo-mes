using System;
using System.Collections.Generic;
using System.Text;
using Telerik.OpenAccess;
using UO_Model.Base;
using System.Reflection;

namespace UO_Service.Base
{
    public class NamedObjectMaint : Service
    {
        virtual public bool Add(NamedObject ndo_insert)
        {
            string idProperty_Name = null;
            object check_exist = ResolveNDO(ndo_insert, out idProperty_Name);
            if (check_exist == null)
            {
                foreach (PropertyInfo pi in ndo_insert.GetType().GetProperties())
                {
                    if (pi.Name == idProperty_Name)
                        continue;   // do not update Primary Property
                    object v_changes = pi.GetValue(ndo_insert, null);
                    if (v_changes != null && (pi.CanWrite || v_changes.GetType().IsGenericType))
                        SetProperty(ndo_insert, pi, v_changes);
                }
                ObjScope.Transaction.Begin();
                ObjScope.Add(ndo_insert);
                ObjScope.Transaction.Commit();
                return true;
            }
            else
                return false;
        }

        public bool Update(NamedObject ndo_changes)
        {
            NamedObject to_change = null;
            return Update(ndo_changes, out to_change);
        }

        virtual protected bool Update(NamedObject ndo_changes, out NamedObject to_change)
        {
            string idProperty_Name = null;
            to_change = ResolveNDO(ndo_changes, out idProperty_Name);

            if (to_change != null)
            {
                ObjScope.Transaction.Begin();
                foreach (PropertyInfo pi in ndo_changes.GetType().GetProperties())
                {
                    if (pi.Name == idProperty_Name)
                        continue;   // do not update Primary Property
                    object v_changes = pi.GetValue(ndo_changes, null);
                    if (v_changes != null && (pi.CanWrite || v_changes.GetType().IsGenericType))
                        SetProperty(to_change, pi, v_changes);
                }
                ObjScope.Transaction.Commit();
                return true;
            }
            else
                return false;
        }

        virtual public bool Remove(NamedObject ndo_obj)
        {
            object res = ResolveNDO(ndo_obj);
            if (res != null)
            {
                ObjScope.Transaction.Begin();
                ObjScope.Remove(res);
                ObjScope.Transaction.Commit();
                return true;
            }
            else
                return false;
        }

        virtual public bool Dump(NamedObject ndo_obj)
        {
            object dump_obj = ResolveNDO(ndo_obj);
            if (dump_obj != null)
            {
                DumpCDOObject(dump_obj);
                return true;
            }
            else
                return false;
        }
    }
}
