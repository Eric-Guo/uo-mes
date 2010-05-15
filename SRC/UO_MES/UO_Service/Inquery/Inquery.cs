using System.Collections.Generic;
using Telerik.OpenAccess;
using UO_Service.Base;
using System.Text;

namespace UO_Service.Inquery
{
    /// <summary>
    /// Inquery is the base class provide all inquery service
    /// Inquery service will not change the MES model and DB
    /// </summary>
    abstract public class Inquery : Service
    {
        protected string[] getObjectNamesViaQuery(string parcialObjectName, string odlQuery)
        {
            List<string> list = new List<string>();
            IQuery query = ObjScope.GetOqlQuery(odlQuery);

            using (IQueryResult result = query.Execute(parcialObjectName + '*'))
            {
                foreach (object res in result)
                    list.Add(res as string);
            }
            return list.ToArray();
        }

        protected string[] getObjectRevisionsViaQuery(string parcialObjectName, string odlQuery)
        {
            List<string> list = new List<string>();
            IQuery query = ObjScope.GetOqlQuery(odlQuery);

            using (IQueryResult result = query.Execute(parcialObjectName + '*'))
            {
                foreach (object res in result)
                {
                    object[] r= res as object[];
                    string revisions = string.Format("{0}({1})", r[0].ToString(), r[1].ToString());
                    list.Add(revisions);
                }
            }
            return list.ToArray();
        }

        public static string GetStringWithNewLine(string[] obj_names)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string co in obj_names)
            {
                sb.Append(co);
                sb.Append('\n');
            }
            return sb.ToString();
        }
    }
}
