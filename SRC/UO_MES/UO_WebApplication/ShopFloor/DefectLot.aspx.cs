using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UO_Service.ContainerTxn;

namespace UO_WebApplication.ShopFloor
{
    public partial class DefectLot : MES_PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MESPageService = new Defect();
            }
        }

        protected void ODS_Details_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = MESPageService as Defect;
        }

        protected void ODS_Details_ObjectDisposing(object sender, ObjectDataSourceDisposingEventArgs e)
        {
            MESPageService = e.ObjectInstance as Defect;
            e.Cancel = true;
        }

        protected void btnAddNewDetail_Click(object sender, EventArgs e)
        {
            Defect service = MESPageService as Defect;
            DefectDetail sd = new DefectDetail();
            sd.DefectReason_Name = "DefaultDefect";
            service.InsertDetail(sd);
            GridView1.DataBind();
        }

        protected void btnDefect_Click(object sender, EventArgs e)
        {
            Defect s = MESPageService as Defect;
            s.Container_Name = txtContainer.Text;
            s.QtyInspected = int.Parse(txtQtyInspected.Text);
            s.ExecuteService();
            lblCompleteMsg.Text = s.CompletionMessage;
            MESPageService = null;
        }
    }
}
