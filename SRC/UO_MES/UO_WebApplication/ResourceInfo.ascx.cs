using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using UO_Service.Inquery;
using UO_Model.Physical;

namespace UO_WebApplication
{
    public partial class ResourceInfo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void FillResourceInfo(string resourceName)
        {
            using (QueryResource qr = new QueryResource())
            {
                Resource res = qr.GetResource(resourceName);
                if (res != null && res.ResourceProductionInfo != null)
                {
                    txtResourceStatus.Text = res.ResourceProductionInfo.ResourceStatus.DisplayName;
                    txtLastStatusChangeDate.Text = res.ResourceProductionInfo.LastStatusChangeDate.ToString();
                }
            }
        }
    }
}