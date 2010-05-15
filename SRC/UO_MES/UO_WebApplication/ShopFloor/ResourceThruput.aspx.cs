using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UO_WebApplication.ShopFloor
{
    public partial class ResourceThruput : MES_PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnResourceThruput_Click(object sender, EventArgs e)
        {
            UO_Service.ResourceTxn.ResourceThruput s = new UO_Service.ResourceTxn.ResourceThruput();
            s.Resource_Name = txtResource.Text;
            s.Qty = int.Parse(txtQty.Text);
            s.UOM_Name = ddlUOM.SelectedValue;
            s.Setup_Revision = ddlSetup.SelectedValue;
            s.MfgOrder_Name = ddlMfgOrder.SelectedValue;
            s.Product_Revision = ddlProduct.SelectedValue;
            s.ExecuteService();
            lblCompleteMsg.Text = s.CompletionMessage;
        }
        protected void btnLoadResourceInfo_Click(object sender, ImageClickEventArgs e)
        {
            resourceInfo.FillResourceInfo(txtResource.Text);
        }
    }
}
