using System;
using System.Text;
using System.Web.UI;

namespace UO_WebApplication
{
    public partial class General : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GenerateJavaScriptMsg();
        }

        public void GenerateJavaScriptMsg()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("var msgHide = '{0}';", Resources.SiteMap.Hide));
            sb.AppendLine(string.Format("var msgShow = '{0}';", Resources.SiteMap.Show));
            Page.ClientScript.RegisterStartupScript(this.GetType(), "jsMessages", sb.ToString(), true);
        }
    }
}
