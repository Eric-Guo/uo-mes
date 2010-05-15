using System;
using System.Web.UI.WebControls;

namespace UO_WebApplication
{
    public partial class MES : MES_PageBase
    {
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlFactory.SelectedValue = Context.Profile["Factory_Name"] as string;
                ddlOperation.SelectedValue = Context.Profile["Operation_Name"] as string;
                ddlWorkCenter.SelectedValue = Context.Profile["WorkCenter_Name"] as string;
                foreach (string styleName in styleConfig.StyleNames)
                {
                    ListItem item = new ListItem(styleName);
                    if (string.Compare(styleName, userStyle) == 0)
                        item.Selected = true;
                    ddlStyles.Items.Add(item);
                }
                ddlLanguage.SelectedValue = Context.Profile["Language"] as string;
            }
        }
        
        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            Context.Profile["Factory_Name"] = ddlFactory.SelectedValue;
            Context.Profile["Operation_Name"] = ddlOperation.SelectedValue;
            Context.Profile["WorkCenter_Name"] = ddlWorkCenter.SelectedValue;
            Context.Profile["Language"] = ddlLanguage.SelectedValue;
            string styleName = ddlStyles.SelectedValue;
            userStrategy.ResetUserStyle(styleName);	// Forward and will refresh UI, so do not add code below
        }
    }
}
