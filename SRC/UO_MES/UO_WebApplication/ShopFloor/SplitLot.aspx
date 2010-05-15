<%@ Page Title="<%$ Resources:Caption %>" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="SplitLot.aspx.cs" Inherits="UO_WebApplication.ShopFloor.SplitLot" %>
<%@ Register Assembly="Telerik.OpenAccess" Namespace="Telerik.OpenAccess" TagPrefix="telerik" %>
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
            params: { type:'active' }
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
        <tr>
            <td>
    <asp:Localize ID="locCloseWhenEmpty" runat="server" Text="<%$ Resources:MES,CloseWhenEmpty %>" />:
            </td>
            <td>
    <asp:CheckBox ID="chkCloseWhenEmpty" runat="server" />
            </td>
        </tr>
    </table>
    <UO:ContainerInfo ID="containerInfo" runat="server" />
    <asp:GridView ID="GridView1" runat="server" DataSourceID="ODS_Details" DataKeyNames="ServiceDetailID" AutoGenerateColumns="False" >
        <Columns>
            <asp:CommandField ShowEditButton="True" ShowCancelButton="True" ShowDeleteButton="True" ButtonType="Image" EditImageUrl="~/images/dg_edit.gif" CancelImageUrl="~/images/dg_cancel.gif" DeleteImageUrl="~/images/dg_delete.gif" UpdateImageUrl="~/images/dg_update.gif" />
            <asp:BoundField DataField="ContainerName" HeaderText="<%$ Resources:MES,ContainerName %>" SortExpression="ContainerName" />
            <asp:BoundField DataField="Qty" HeaderText="<%$ Resources:MES,Qty %>" SortExpression="Qty" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="ODS_Details" runat="server" 
        TypeName="UO_Service.ContainerTxn.Split" SelectMethod="SelectDetails" 
        DataObjectTypeName="UO_Service.ContainerTxn.SplitDetail"
        InsertMethod="InsertDetail" DeleteMethod="DeleteDetail" 
        onobjectcreating="ODS_Details_ObjectCreating" 
        onobjectdisposing="ODS_Details_ObjectDisposing"
        UpdateMethod="UpdateDetails">
    </asp:ObjectDataSource>
</asp:Content>
<asp:Content ID="Footer1" ContentPlaceHolderID="Footer1" runat="server">
    <table>
        <tr>
<td><asp:Button ID="btnAddNewDetail" runat="server" Text="<%$ Resources:AddDetail %>" onclick="btnAddNewDetail_Click" /></td>
<td><asp:Button ID="btnSplit" runat="server" Text="<%$ Resources:Execute %>" onclick="btnSplit_Click"/></td>
        </tr>
        <tr>
            <td><asp:Localize ID="locCompleteMessage" runat="server" Text="<%$ Resources:MES,CompleteMessage %>" /></td>
            <td><asp:Label ID="lblCompleteMsg" runat="server" Text="" /></td>
        </tr>
    </table>
</asp:Content>
