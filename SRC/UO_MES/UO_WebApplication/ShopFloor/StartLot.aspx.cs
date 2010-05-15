using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UO_Service.ContainerTxn;
using UO_Model.Process;
using System.Collections;

namespace UO_WebApplication.ShopFloor
{
    public partial class StartLot : MES_PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MESPageService = new Start();
            }
        }

        protected void ddlWorkflow_SelectedIndexChanged(object sender, EventArgs e)
        {
            OADS_StartStep.WhereParameters["Workflow_ID"].DefaultValue = ddlWorkflow.SelectedValue;
        }

        protected void ddlWorkflow_DataBound(object sender, EventArgs e)
        {
            OADS_StartStep.WhereParameters["Workflow_ID"].DefaultValue = ddlWorkflow.SelectedValue;
        }

        protected void ODS_Details_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = MESPageService as Start;
        }

        protected void ODS_Details_ObjectDisposing(object sender, ObjectDataSourceDisposingEventArgs e)
        {
            MESPageService = e.ObjectInstance as Start;
            e.Cancel = true;
        }

        protected void btnAddNewDetail_Click(object sender, EventArgs e)
        {
            Start s = MESPageService as Start;
            StartDetail sd = new StartDetail();
            sd.ContainerName = "NewLot"+s.SelectDetails().Count;
            sd.ContainerLevel_Name = "Lot";
            sd.DueDate = DateTime.Now;
            sd.Product_Revision = "ProductA(r2)";
            sd.UOM_Name = "Piece";
            sd.ContainerStatus_Name = "Active";
            s.InsertDetail(sd);
            GridView1.DataBind();
        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            Start s = MESPageService as Start;
            s.MfgOrder_Name = ddlMfgOrder.SelectedValue;
            s.Workflow_Revision = ddlWorkflow.SelectedItem.Text;
            s.WorkflowStep_Name = ddlStartStep.SelectedItem.Text;
            s.Factory_Name = Context.Profile["Factory_Name"] as string;
            s.ExecuteService();
            lblCompleteMsg.Text = s.CompletionMessage;
            MESPageService = null;
        }
    }
}
