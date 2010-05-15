using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UO_WebApplication.ShopFloor
{
    public partial class Release : MES_PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnRelease_Click(object sender, EventArgs e)
        {
            UO_Service.ContainerTxn.Release s = new UO_Service.ContainerTxn.Release();
            s.Container_Name = txtContainer.Text;
            s.ReleaseReason_Name = ddlReleaseReason.SelectedValue;
            s.ExecuteService();
            lblCompleteMsg.Text = s.CompletionMessage;
        }
        protected void btnLoadContainerInfo_Click(object sender, ImageClickEventArgs e)
        {
            containerInfo.FillContainerInfo(txtContainer.Text);
        }
    }
}
