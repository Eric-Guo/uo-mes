<%@ Page Title="<%$ Resources:Caption %>" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="MoveOut.aspx.cs" Inherits="UO_WebApplication.ShopFloor.MoveOut" %>
<%@ Register Assembly="Telerik.OpenAccess" Namespace="Telerik.OpenAccess" TagPrefix="telerik" %>
<asp:Content ID="Head1" ContentPlaceHolderID="Head1" runat="server">
<% if (false) { %>
<script src="/src/jquery-1.3.2-vsdoc.js" type="text/javascript"></script>
<% } %>
<script src="/src/jquery.hintbox-1.2.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function() {
        $('#ctl00_Holder1_txtContainer').hintbox({
            url: '/JQuerySvr/ContainerAutoComplete.ashx',
            params: { type: 'active' }
        });
    });
</script>
</asp:Content>
<asp:Content ID="Holder1" ContentPlaceHolderID="Holder1" runat="server">
    <table><caption><asp:Localize ID="locCaption" runat="server" Text="<%$ Resources:Caption %>" /></caption>
        <tr>
            <td>
    <asp:Localize ID="locContainer" runat="server" Text="<%$ Resources:MES,Container %>" />:
            </td>
            <td>
    <asp:TextBox ID="txtContainer" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
    <asp:Localize ID="locResource" runat="server" Text="<%$ Resources:MES,Resource %>" />:
            </td>
            <td>
    <asp:DropDownList ID="ddlResource" runat="server" DataSourceID="OADS_Resource" DataTextField="DisplayName" />
    <telerik:OpenAccessDataSource ID="OADS_Resource" runat="server" 
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Physical.Resource" >
    </telerik:OpenAccessDataSource>
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
    <asp:Localize ID="locThruputAllQty" runat="server" Text="<%$ Resources:MES,ThruputAllQty %>" />:
            </td>
            <td>
    <asp:CheckBox ID="chkThruputAllQty" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Footer1" ContentPlaceHolderID="Footer1" runat="server">
    <table>
        <tr>
            <td>
    <asp:Button ID="btnMoveOut" runat="server" Text="<%$ Resources:Execute %>" onclick="btnMoveOut_Click"/>
            </td>
        </tr>
        <tr>
            <td><asp:Localize ID="locCompleteMessage" runat="server" Text="<%$ Resources:MES,CompleteMessage %>" /></td>
            <td><asp:Label ID="lblCompleteMsg" runat="server" Text="" /></td>
        </tr>
    </table>
</asp:Content>
