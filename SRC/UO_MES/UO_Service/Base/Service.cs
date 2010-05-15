using System;
using System.Reflection;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Execution;
using UO_Service.Resx;
using System.ServiceModel;

namespace UO_Service.Base
{
    #region IService
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        object ResolveCDO(string cdo_type_name, string cdo_name, bool throwException);
        [OperationContract]
        Container ResolveContainer(string container_name);
    }
    #endregion

    /// <summary>
    /// Service class is the base class which provide ORM ObjScope support and other helpful function
    /// </summary>
    abstract public class Service : IDisposable, IService
    {
        protected Service()
        {
            // Always return New object scope ensure that not object access between different service.
            objScope = ORM.GetNewObjectScope();
        }

        protected Service(IObjectScope existObjScope)
        {
            objScope = existObjScope;
        }

        private IObjectScope objScope;
        protected IObjectScope ObjScope
        {
            get { return objScope; }
        }

        private string completionMessage;
        public string CompletionMessage
        {
            get 
            {
                if (completionMessage == null)
                    return MSG.Service_Success;
                else
                    return completionMessage;
            }
            set { completionMessage = value; }
        }

        private DateTime txnDate = UO_Model.Constants.NullDateTime;
        public DateTime TxnDate
        {
            get { return (txnDate == UO_Model.Constants.NullDateTime) ? DateTime.Now : txnDate; }
            set { txnDate = value; }
        }


        #region ResolveContainer
        protected Container ResolveContainer(string container_name)
        {
            const string queryContainerByName = "ELEMENT (SELECT o FROM ContainerExtent AS o WHERE o.ContainerName = $1)";
            object res = null;
            IQuery oqlQuery = ObjScope.GetOqlQuery(queryContainerByName);
            IQueryResult result = oqlQuery.Execute(container_name);
            if (result.Count > 0)
                res = result[0];
            result.Dispose();
            return res as Container;
        }
        #endregion

        #region ResolveCDOByID
        protected object ResolveCDOByID(Type t, string id)
        {
            return ObjScope.GetObjectById(Database.OID.ParseObjectId(t, id));
        }
        #endregion

        #region ResolveCDO
        protected object ResolveCDO(Type cdo_type, string obj_display_name)
        {
            return ResolveCDO(cdo_type.Name, obj_display_name);
        }
        protected object ResolveCDO(string cdo_type_name, string cdo_display_name)
        {
            return ResolveCDO(cdo_type_name, cdo_display_name, false);
        }
        public object ResolveCDO(string cdo_type_name, string cdo_display_name, bool throwException)
        {
            if (string.IsNullOrEmpty(cdo_display_name)) return null;
            string name, revision;
            if (TryParseRevisionedObject(cdo_display_name, out name, out revision))
                return ResolveRDO(cdo_type_name, name, revision, throwException);
            else
                return ResolveNDO(cdo_type_name, cdo_display_name, throwException);
        }
        #endregion

        #region ResolveNDO
        protected NamedObject ResolveNDO(NamedObject ndo_changes)
        {
            string idProperty_Name;
            return ResolveNDO(ndo_changes, out idProperty_Name);
        }

        protected NamedObject ResolveNDO(NamedObject ndo_changes, out string idProperty_Name)
        {
            object to_change = null;
            string idProperty_Value = Service.GetIdentityFieldValue(ndo_changes, out idProperty_Name);
            if (idProperty_Value == null || idProperty_Value == "0")
                to_change = ResolveCDO(ndo_changes.GetType(), ndo_changes.Name);
            else
                to_change = ResolveCDOByID(ndo_changes.GetType(), idProperty_Value);
            return to_change as NamedObject;
        }
        #endregion

        #region ResolveRDO
        protected RevisionedObject ResolveRDO(RevisionedObject rdo_changes)
        {
            string idProperty_Name;
            return ResolveRDO(rdo_changes, out idProperty_Name);
        }
        protected RevisionedObject ResolveRDO(RevisionedObject rdo_changes, out string idProperty_Name)
        {
            object to_change = null;
            string idProperty_Value = Service.GetIdentityFieldValue(rdo_changes, out idProperty_Name);
            if (idProperty_Value == null || idProperty_Value == "0")
                to_change = ResolveCDO(rdo_changes.GetType(), rdo_changes.DisplayName);
            else
                to_change = ResolveCDOByID(rdo_changes.GetType(), idProperty_Value);
            return to_change as RevisionedObject;
        }
        #endregion

        #region ResolveObject
        protected object ResolveObject(object obj, bool throwException)
        {
            Type t = obj.GetType();
            if (t.IsSubclassOf(typeof(NamedObject)) || t.IsSubclassOf(typeof(RevisionedObject)))
            {
                string real_obj_displayname = t.GetProperty("DisplayName").GetValue(obj, null).ToString();
                return ResolveCDO(t.Name, real_obj_displayname, throwException);
            }
            else
                return obj;
        }
        #endregion

        static protected void DumpCDOObject(object dump_obj)
        {
            Type t_res = dump_obj.GetType();
            Console.WriteLine("Class: {0}", t_res.Name);
            PropertyInfo[] propertyInfos = t_res.GetProperties();
            foreach (PropertyInfo pi in propertyInfos)
            {
                object v = pi.GetValue(dump_obj, null);
                if (v != null)
                {
                    Type t_v = v.GetType();
                    Console.WriteLine("{0,-20}: {1,10}", t_v.Name, v.ToString());
                    if (t_v.IsGenericType == true)
                    {
                        PropertyInfo pi_item = t_v.GetProperty("Item");
                        System.Collections.IList i_v_list = v as System.Collections.IList;
                        Console.WriteLine(pi_item.Name + " has " + i_v_list.Count + " element.");
                        for (int i = 0; i < i_v_list.Count; i++)
                        {
                            object[] indexArgs = { i };
                            object pi_item_value = pi_item.GetValue(v, indexArgs);
                            NamedObject pi_item_name = pi_item_value as NamedObject;
                            Console.WriteLine("   Value: {0}, Name: {1}", pi_item_value, pi_item_name.Name);
                        }
                    }
                }
            }
        }

        protected void SetProperty(object to_change, PropertyInfo pi, object changes)
        {
            Type t_v = changes.GetType();
            if (t_v.IsGenericType == true)
            {
                PropertyInfo pi_item = t_v.GetProperty("Item");
                System.Collections.IList v_changes_list = changes as System.Collections.IList;

                // Only update the index property if changes index property has element
                if (v_changes_list.Count > 0)
                {
                    object v_to_change = pi.GetValue(to_change, null);
                    System.Collections.IList v_to_change_list = v_to_change as System.Collections.IList;

                    // Always remove to_change existing element
                    for (int i = v_to_change_list.Count - 1; i >= 0; i--)
                        v_to_change_list.RemoveAt(i);

                    for (int i = 0; i < v_changes_list.Count; i++)
                    {
                        object[] indexArgs = { i };
                        object pi_item_value = pi_item.GetValue(changes, indexArgs);
                        v_to_change_list.Add(ResolveObject(pi_item_value, true));
                    }
                }
            }
            else
                pi.SetValue(to_change, ResolveObject(changes, true), null);
        }

        #region IDisposable
        public void Dispose()
        {
            // Even CompoundTxn will reusing objScope, so only dispose here is enough
            // No need to further handle objScope dispose in subclass
            if (objScope != null)
            {
                objScope.Dispose();
                GC.SuppressFinalize(this);
                objScope = null;
            }
        }

        ~Service()
        {
            if (objScope != null)
                objScope.Dispose();
        }
        #endregion

        #region IService Members
        object IService.ResolveCDO(string cdo_type_name, string cdo_name, bool throwException)
        {
            return ResolveCDO(cdo_type_name, cdo_name, throwException);
        }
        Container IService.ResolveContainer(string container_name)
        {
            return ResolveContainer(container_name);
        }
        #endregion

        #region Private helper method
        static private bool TryParseRevisionedObject(string ref_rdo, out string ref_name, out string ref_revision)
        {
            int lbi = ref_rdo.IndexOf('(');  // left brack position index
            int rbi = ref_rdo.IndexOf(')');
            if (lbi != -1 && rbi != -1 && lbi < rbi)
            {
                ref_name = ref_rdo.Substring(0, lbi);
                ref_revision = ref_rdo.Substring(lbi + 1, rbi - lbi - 1);
                return true;
            }
            else
            {
                ref_name = ref_rdo;
                ref_revision = "";
                return false;
            }
        }

        private object ResolveNDO(string ndo_type_name, string ndo_name, bool throwException)
        {
            const string queryNDObyName = "ELEMENT (SELECT o FROM {0}Extent AS o WHERE o.Name = $1)";
            object res = null;
            IQuery oqlQuery = ObjScope.GetOqlQuery(string.Format(queryNDObyName, ndo_type_name));
            IQueryResult result = oqlQuery.Execute(ndo_name);
            if (result.Count > 0)
                res = result[0];
            result.Dispose();
            if (res == null && throwException == true)
                throw new UOServiceException(string.Format(MSG.Object_not_found, ndo_name, ndo_type_name));
            return res;
        }

        private RevisionedObject ResolveRDO(string rdo_type_name, string rdo_name, string rdo_revision, bool throwException)
        {
            RevisionedObject res = null;
            if (rdo_revision == null)
                rdo_revision = "";
            if (rdo_revision != "")
            {
                const string queryRDObyRevision = "ELEMENT (SELECT o FROM {0}Extent AS o WHERE o.RevBase.Name = $1 AND o.Revision = $2)";
                // Query from revision
                IQuery oqlQuery = ObjScope.GetOqlQuery(string.Format(queryRDObyRevision, rdo_type_name));
                IQueryResult result = oqlQuery.Execute(rdo_name, rdo_revision);
                if (result.Count > 0)
                    res = result[0] as RevisionedObject;
                result.Dispose();
            }
            else
                res = (ResolveNDO(rdo_type_name + "Base", rdo_name, throwException) as RevisionBase).Current;
            if (res == null && throwException == true)
                throw new UOServiceException(string.Format(MSG.Object_not_found, string.Format("{0}({1})", rdo_name, rdo_revision), rdo_type_name));
            return res;
        }

        private static string GetIdentityFieldValue(object cdo_obj, out string idProperty_Name)
        {
            idProperty_Name = null;
            Type t_changes = cdo_obj.GetType();
            string idField_AttributeName = null;
            foreach (CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(t_changes))
                foreach (CustomAttributeNamedArgument cana in cad.NamedArguments)
                    if (cana.MemberInfo.Name == "IdentityField")
                        idField_AttributeName = cana.TypedValue.ToString().Trim('"');

            if (null == idField_AttributeName)
                return null;

            foreach (PropertyInfo pi in t_changes.GetProperties())
                if (pi.Name.ToLower() == idField_AttributeName.ToLower())
                {
                    idProperty_Name = pi.Name;
                    break;
                }

            if (idProperty_Name != null)
            {
                PropertyInfo pi_idField = t_changes.GetProperty(idProperty_Name);
                return pi_idField.GetValue(cdo_obj, null).ToString();
            }
            else
                return null;
        }
        #endregion
    }
}
