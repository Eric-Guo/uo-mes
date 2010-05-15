using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UO_Service.ContainerTxn;

namespace UO_WebApplication.ShopFloor
{
    public partial class SerializeLot : MES_PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MESPageService = new Serialize();
            }
        }

        protected void ODS_Details_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = MESPageService as Serialize;
        }

        protected void ODS_Details_ObjectDisposing(object sender, ObjectDataSourceDisposingEventArgs e)
        {
            MESPageService = e.ObjectInstance as Serialize;
            e.Cancel = true;
        }

        protected void btnAddNewDetail_Click(object sender, EventArgs e)
        {
            Serialize service = MESPageService as Serialize;
            SerializeDetail sd = new SerializeDetail();
            sd.ChildContainerName = txtContainer.Text + '-' + service.SelectDetails().Count;
            service.InsertDetail(sd);
            GridView1.DataBind();
        }

        protected void btnSerialize_Click(object sender, EventArgs e)
        {
            Serialize s = MESPageService as Serialize;
            s.Container_Name = txtContainer.Text;
            s.ChildContainerLevel_Name = ddlChildContainerLevel.SelectedValue;
            s.ChildUOM_Name = ddlChildUOM.SelectedValue;
            s.Product_Revision = ddlProduct.SelectedValue;
            s.ExecuteService();
            lblCompleteMsg.Text = s.CompletionMessage;
            MESPageService = null;
        }
    }
}
