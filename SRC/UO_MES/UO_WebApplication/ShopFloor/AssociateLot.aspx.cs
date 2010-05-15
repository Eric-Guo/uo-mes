using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UO_Service.ContainerTxn;

namespace UO_WebApplication.ShopFloor
{
    public partial class AssociateLot : MES_PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MESPageService = new Associate();
            }
        }

        protected void ODS_Details_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = MESPageService as Associate;
        }

        protected void ODS_Details_ObjectDisposing(object sender, ObjectDataSourceDisposingEventArgs e)
        {
            MESPageService = e.ObjectInstance as Associate;
            e.Cancel = true;
        }

        protected void btnAddNewDetail_Click(object sender, EventArgs e)
        {
            Associate service = MESPageService as Associate;
            AssociateDetail sd = new AssociateDetail();
            sd.ChildContainerName = "";
            service.InsertDetail(sd);
            GridView1.DataBind();
        }

        protected void btnAssociate_Click(object sender, EventArgs e)
        {
            Associate s = MESPageService as Associate;
            s.Container_Name = txtContainer.Text;
            s.ExecuteService();
            lblCompleteMsg.Text = s.CompletionMessage;
            MESPageService = null;
        }
    }
}
