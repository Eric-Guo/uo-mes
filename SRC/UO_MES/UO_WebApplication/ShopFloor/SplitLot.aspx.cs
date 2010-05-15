using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UO_Service.ContainerTxn;

namespace UO_WebApplication.ShopFloor
{
    public partial class SplitLot : MES_PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MESPageService = new Split();
            }
        }

        protected void ODS_Details_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = MESPageService as Split;
        }

        protected void ODS_Details_ObjectDisposing(object sender, ObjectDataSourceDisposingEventArgs e)
        {
            MESPageService = e.ObjectInstance as Split;
            e.Cancel = true;
        }

        protected void btnAddNewDetail_Click(object sender, EventArgs e)
        {
            Split service = MESPageService as Split;
            SplitDetail sd = new SplitDetail();
            sd.ContainerName = txtContainer.Text+ '-' + service.SelectDetails().Count;
            service.InsertDetail(sd);
            GridView1.DataBind();
        }

        protected void btnSplit_Click(object sender, EventArgs e)
        {
            Split s = MESPageService as Split;
            s.Container_Name = txtContainer.Text;
            s.CloseWhenEmpty = chkCloseWhenEmpty.Checked;
            s.ExecuteService();
            lblCompleteMsg.Text = s.CompletionMessage;
            MESPageService = null;
        }

        protected void btnLoadContainerInfo_Click(object sender, ImageClickEventArgs e)
        {
            containerInfo.FillContainerInfo(txtContainer.Text);
        }
    }
}
