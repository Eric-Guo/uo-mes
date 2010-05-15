using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace UO_WebApplication.ShopFloor
{
    public partial class MoveTxn : MES_PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnMoveTxn_Click(object sender, EventArgs e)
        {
            UO_Service.ContainerTxn.MoveTxn s = new UO_Service.ContainerTxn.MoveTxn();
            s.Container_Name = txtContainer.Text;
            s.ExecuteService();
            lblCompleteMsg.Text = s.CompletionMessage;
        }

        protected void btnLoadContainerInfo_Click(object sender, ImageClickEventArgs e)
        {
            containerInfo.FillContainerInfo(txtContainer.Text);
        }
    }
}
