using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UO_Service.ContainerTxn;

namespace UO_WebApplication.ShopFloor
{
    public partial class CombineLot :MES_PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MESPageService = new Combine();
            }
        }

        protected void ODS_Details_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = MESPageService as Combine;
        }

        protected void ODS_Details_ObjectDisposing(object sender, ObjectDataSourceDisposingEventArgs e)
        {
            MESPageService = e.ObjectInstance as Combine;
            e.Cancel = true;
        }

        protected void btnAddNewDetail_Click(object sender, EventArgs e)
        {
            Combine service = MESPageService as Combine;
            CombineFromDetail sd = new CombineFromDetail();
            sd.FromContainerName = "";
            service.InsertDetail(sd);
            GridView1.DataBind();
        }

        protected void btnCombine_Click(object sender, EventArgs e)
        {
            Combine s = MESPageService as Combine;
            s.Container_Name = txtContainer.Text;
            s.CloseWhenEmpty = chkCloseWhenEmpty.Checked;
            s.ExecuteService();
            lblCompleteMsg.Text = s.CompletionMessage;
            MESPageService = null;
        }
    }
}
