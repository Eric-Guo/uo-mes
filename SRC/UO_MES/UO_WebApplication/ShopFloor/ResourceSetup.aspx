<%@ Page Title="<%$ Resources:Caption %>" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="ResourceSetup.aspx.cs" Inherits="UO_WebApplication.ShopFloor.ResourceSetup" %>
<%@ Register Assembly="Telerik.OpenAccess" Namespace="Telerik.OpenAccess" TagPrefix="telerik" %>
<%@ Register TagPrefix="UO" TagName="ResourceInfo" Src="~/ResourceInfo.ascx" %>
<asp:Content ID="Head1" ContentPlaceHolderID="Head1" runat="server">
<% if (false) { %>
<script src="/src/jquery-1.3.2-vsdoc.js" type="text/javascript"></script>
<% } %>
<script src="/src/jquery.hintbox-1.2.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function() {
        $('#ctl00_Holder1_txtResource').hintbox({
            url: '/JQuerySvr/ResourceAutoComplete.ashx',
            params: { type:'all' }
        });
    });
</script>
</asp:Content>
<asp:Content ID="Holder1" ContentPlaceHolderID="Holder1" runat="server">
    <table><caption><asp:Localize ID="locCaption" runat="server" Text="<%$ Resources:Caption %>" /></caption>
        <tr>
            <td>
    <asp:Localize ID="locResource" runat="server" Text="<%$ Resources:MES,Resource %>" />:
            </td>
            <td>
    <asp:TextBox ID="txtResource" runat="server" />
    <asp:ImageButton ID="btnLoadResourceInfo" runat="server" ImageUrl="~/images/view.gif" OnClick="btnLoadResourceInfo_Click" />
    <UO:ResourceInfo ID="resourceInfo" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
    <asp:Localize ID="locResourceStatus" runat="server" Text="<%$ Resources:MES,ResourceStatus %>" />:
            </td>
            <td>
    <asp:DropDownList ID="ddlResourceStatus" runat="server" DataSourceID="OADS_ResourceStatus" DataTextField="DisplayName" />
    <telerik:OpenAccessDataSource ID="OADS_ResourceStatus" runat="server" 
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Physical.Status.ResourceStatus" >
    </telerik:OpenAccessDataSource>
            </td>
        </tr>
        <tr>
            <td>
    <asp:Localize ID="locResourceStatusReason" runat="server" Text="<%$ Resources:MES,ResourceStatusReason %>" />:
            </td>
            <td>
    <asp:DropDownList ID="ddlResourceStatusReason" runat="server" DataSourceID="OADS_ResourceStatusReason" DataTextField="DisplayName" />
    <telerik:OpenAccessDataSource ID="OADS_ResourceStatusReason" runat="server" 
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Physical.Reason.ResourceStatusReason" >
    </telerik:OpenAccessDataSource>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Footer1" ContentPlaceHolderID="Footer1" runat="server">
    <table>
        <tr>
            <td>
    <asp:Button ID="btnResourceSetup" runat="server" Text="<%$ Resources:Execute %>" onclick="btnResourceSetup_Click"/>
            </td>
        </tr>
        <tr>
            <td><asp:Localize ID="locCompleteMessage" runat="server" Text="<%$ Resources:MES,CompleteMessage %>" /></td>
            <td><asp:Label ID="lblCompleteMsg" runat="server" Text="" /></td>
        </tr>
    </table>
</asp:Content>
