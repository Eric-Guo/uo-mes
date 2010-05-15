<%@ Page Title="<%$ Resources:Caption %>" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="ResourceThruput.aspx.cs" Inherits="UO_WebApplication.ShopFloor.ResourceThruput" %>
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
    <asp:Localize ID="locQty" runat="server" Text="<%$ Resources:MES,Qty %>" />:
            </td>
            <td>
    <asp:TextBox ID="txtQty" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
    <asp:Localize ID="locUOM" runat="server" Text="<%$ Resources:MES,UOM %>" />:
            </td>
            <td>
    <asp:DropDownList ID="ddlUOM" runat="server" DataSourceID="OADS_UOM" DataTextField="DisplayName" />
    <telerik:OpenAccessDataSource ID="OADS_UOM" runat="server" 
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Process.UOM" >
    </telerik:OpenAccessDataSource>
            </td>
        </tr>
        <tr>
            <td>
    <asp:Localize ID="locSetup" runat="server" Text="<%$ Resources:MES,Setup %>" />:
            </td>
            <td>
    <asp:DropDownList ID="ddlSetup" runat="server" DataSourceID="OADS_Setup" DataTextField="DisplayName" />
    <telerik:OpenAccessDataSource ID="OADS_Setup" runat="server" 
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Process.Setup" >
    </telerik:OpenAccessDataSource>
            </td>
        </tr>
        <tr>
            <td>
    <asp:Localize ID="locMfgOrder" runat="server" Text="<%$ Resources:MES,MfgOrder %>" />:
            </td>
            <td>
    <asp:DropDownList ID="ddlMfgOrder" runat="server" DataSourceID="OADS_MfgOrder" DataTextField="DisplayName" />
    <telerik:OpenAccessDataSource ID="OADS_MfgOrder" runat="server" 
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Process.MfgOrder" >
    </telerik:OpenAccessDataSource>
            </td>
        </tr>
        <tr>
            <td>
    <asp:Localize ID="locProduct" runat="server" Text="<%$ Resources:MES,Product %>" />:
            </td>
            <td>
    <asp:DropDownList ID="ddlProduct" runat="server" DataSourceID="OADS_Product" DataTextField="DisplayName" />
    <telerik:OpenAccessDataSource ID="OADS_Product" runat="server" 
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Process.Product" >
    </telerik:OpenAccessDataSource>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Footer1" ContentPlaceHolderID="Footer1" runat="server">
    <table>
        <tr>
            <td>
    <asp:Button ID="btnResourceThruput" runat="server" Text="<%$ Resources:Execute %>" onclick="btnResourceThruput_Click"/>
            </td>
        </tr>
        <tr>
            <td><asp:Localize ID="locCompleteMessage" runat="server" Text="<%$ Resources:MES,CompleteMessage %>" /></td>
            <td><asp:Label ID="lblCompleteMsg" runat="server" Text="" /></td>
        </tr>
    </table>
</asp:Content>
