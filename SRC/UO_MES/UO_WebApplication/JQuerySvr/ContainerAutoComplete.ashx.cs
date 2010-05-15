using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using UO_Service.Inquery;
using UO_Model.Execution;
using System.Text;

namespace UO_WebApplication.Query
{
    /// <summary>
    /// Summary description for ContainerAutoComplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ContainerAutoComplete : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            using (QueryContainer qc = new QueryContainer())
            {
                string query = context.Request["q"];
                string type = context.Request["type"];

                string[] lots;
                switch (type)
                {
                    case "active":
                        lots = qc.GetActiveContainerNames(query);
                        break;
                    case "hold":
                        lots = qc.GetHoldContainerNames(query);
                        break;
                    case "all":
                    default:
                        lots = qc.GetAllContainerNames(query);
                        break;
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write(QueryContainer.GetStringWithNewLine(lots));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
