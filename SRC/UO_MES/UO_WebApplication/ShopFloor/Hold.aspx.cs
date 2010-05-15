using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UO_WebApplication.ShopFloor
{
    public partial class Hold : MES_PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnHold_Click(object sender, EventArgs e)
        {
            UO_Service.ContainerTxn.Hold s = new UO_Service.ContainerTxn.Hold();
            s.Container_Name = txtContainer.Text;
            s.HoldReason_Name = ddlHoldReason.SelectedValue;
            s.ExecuteService();
            lblCompleteMsg.Text = s.CompletionMessage;
        }
        protected void btnLoadContainerInfo_Click(object sender, ImageClickEventArgs e)
        {
            containerInfo.FillContainerInfo(txtContainer.Text);
        }
    }
}
