using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UO_WebApplication.ShopFloor
{
    public partial class ResourceSetup : MES_PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnResourceSetup_Click(object sender, EventArgs e)
        {
            UO_Service.ResourceTxn.ResourceSetup s = new UO_Service.ResourceTxn.ResourceSetup();
            s.Resource_Name = txtResource.Text;
            s.ResourceToStatus_Name = ddlResourceStatus.SelectedValue;
            s.ResourceStatusReason_Name = ddlResourceStatusReason.SelectedValue;
            s.ExecuteService();
            lblCompleteMsg.Text = s.CompletionMessage;
        }
        protected void btnLoadResourceInfo_Click(object sender, ImageClickEventArgs e)
        {
            resourceInfo.FillResourceInfo(txtResource.Text);
        }
    }
}
