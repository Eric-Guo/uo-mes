using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UO_WebApplication.ShopFloor
{
    public partial class MoveOut : MES_PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnMoveOut_Click(object sender, EventArgs e)
        {
            UO_Service.CompoundTxn.MoveOut s = new UO_Service.CompoundTxn.MoveOut();
            s.Container_Name = txtContainer.Text;
            s.ExecuteService();
            lblCompleteMsg.Text = s.CompletionMessage;
        }
    }
}
