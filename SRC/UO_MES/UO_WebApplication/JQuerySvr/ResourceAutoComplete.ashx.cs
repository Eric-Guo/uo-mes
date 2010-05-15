using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Text;
using UO_Service.Inquery;

namespace UO_WebApplication.JQuerySvr
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ResourceAutoComplete : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            using (QueryResource q = new QueryResource())
            {
                string query = context.Request["q"];
                string type = context.Request["type"];

                string[] resources;
                switch (type)
                {
                    case "all":
                    default:
                        resources = q.GetAllResourceNames(query);
                        break;
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write(QueryContainer.GetStringWithNewLine(resources));
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
