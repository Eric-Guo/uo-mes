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
using UO_Service.ContainerTxn;

namespace UO_WebApplication.ShopFloor
{
    public partial class DisassociateLot : MES_PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MESPageService = new Disassociate();
            }
        }

        protected void ODS_Details_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = MESPageService as Disassociate;
        }

        protected void ODS_Details_ObjectDisposing(object sender, ObjectDataSourceDisposingEventArgs e)
        {
            MESPageService = e.ObjectInstance as Disassociate;
            e.Cancel = true;
        }

        protected void btnAddNewDetail_Click(object sender, EventArgs e)
        {
            Disassociate service = MESPageService as Disassociate;
            DisassociateDetail sd = new DisassociateDetail();
            sd.ChildContainerName = "";
            service.InsertDetail(sd);
            GridView1.DataBind();
        }

        protected void btnDisassociate_Click(object sender, EventArgs e)
        {
            Disassociate s = MESPageService as Disassociate;
            s.DisassociateAll = chkDisassociateAll.Checked;
            s.ExecuteService();
            lblCompleteMsg.Text = s.CompletionMessage;
            MESPageService = null;
        }

        protected void btnLoadChildDetail_Click(object sender, ImageClickEventArgs e)
        {
            Disassociate s = MESPageService as Disassociate;
            s.Container_Name = txtContainer.Text;
            GridView1.DataBind();
        }
    }
}
