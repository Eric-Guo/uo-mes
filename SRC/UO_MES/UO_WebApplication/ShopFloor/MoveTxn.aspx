<%@ Page Title="<%$ Resources:Caption %>" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="MoveTxn.aspx.cs" Inherits="UO_WebApplication.ShopFloor.MoveTxn" %>
<%@ Register TagPrefix="UO" TagName="ContainerInfo" Src="~/ContainerInfo.ascx" %>
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
    <asp:ImageButton ID="btnLoadContainerInfo" runat="server" ImageUrl="~/images/view.gif" OnClick="btnLoadContainerInfo_Click" />
            </td>
        </tr>
    </table>
    <UO:ContainerInfo ID="containerInfo" runat="server" />
</asp:Content>
<asp:Content ID="Footer1" ContentPlaceHolderID="Footer1" runat="server">
    <table>
        <tr>
            <td>
    <asp:Button ID="btnMoveTxn" runat="server" Text="<%$ Resources:Execute %>" onclick="btnMoveTxn_Click"/>
            </td>
        </tr>
        <tr>
            <td><asp:Localize ID="locCompleteMessage" runat="server" Text="<%$ Resources:MES,CompleteMessage %>" /></td>
            <td><asp:Label ID="lblCompleteMsg" runat="server" Text="" /></td>
        </tr>
    </table>
</asp:Content>
