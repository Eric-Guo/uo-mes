using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UO_Service.Inquery;
using UO_Model.Execution;

namespace UO_WebApplication
{
    public partial class ContainerInfo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void FillContainerInfo(string containerName)
        {
            using (QueryContainer qc = new QueryContainer())
            {
                Container co = qc.GetContainer(containerName);
                if (co != null)
                {
                    txtContainerQty.Text = co.Qty.ToString();
                    txtProduct.Text = co.Product.DisplayName;
                    txtContainerStatus.Text = co.ContainerStatus.DisplayName;
                    txtContainerStep.Text = co.CurrentStatus.WorkflowStep.DisplayName;
                }
            }
        }
    }
}