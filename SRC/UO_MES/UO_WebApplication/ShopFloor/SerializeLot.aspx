<%@ Page Title="<%$ Resources:Caption %>" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="SerializeLot.aspx.cs" Inherits="UO_WebApplication.ShopFloor.SerializeLot" %>
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
            </td>
        </tr>
        <tr>
            <td>
    <asp:Localize ID="locChildContainerLevel" runat="server" Text="<%$ Resources:MES,ChildContainerLevel %>" />:
            </td>
            <td>
<asp:DropDownList ID="ddlChildContainerLevel" runat="server" DataSourceID="OADS_Level" DataTextField="DisplayName" />
<telerik:OpenAccessDataSource ID="OADS_Level" runat="server" 
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Process.ContainerLevel">
</telerik:OpenAccessDataSource>
            </td>
        </tr>
        <tr>
            <td>
    <asp:Localize ID="locChildUOM" runat="server" Text="<%$ Resources:MES,ChildUOM %>" />:
            </td>
            <td>
<asp:DropDownList ID="ddlChildUOM" runat="server" DataSourceID="OADS_UOM" DataTextField="DisplayName" />
<telerik:OpenAccessDataSource ID="OADS_UOM" runat="server" 
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Process.UOM">
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
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Process.Product">
</telerik:OpenAccessDataSource>
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" runat="server" DataSourceID="ODS_Details" DataKeyNames="ServiceDetailID" AutoGenerateColumns="False" >
        <Columns>
            <asp:CommandField ShowEditButton="True" ShowCancelButton="True" ShowDeleteButton="True" ButtonType="Image" EditImageUrl="~/images/dg_edit.gif" CancelImageUrl="~/images/dg_cancel.gif" DeleteImageUrl="~/images/dg_delete.gif" UpdateImageUrl="~/images/dg_update.gif" />
            <asp:BoundField DataField="ChildContainerName" HeaderText="<%$ Resources:MES,ChildContainerName %>" SortExpression="ChildContainerName" />
            <asp:BoundField DataField="ChildQty" HeaderText="<%$ Resources:MES,ChildQty %>" SortExpression="ChildQty" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="ODS_Details" runat="server" 
        TypeName="UO_Service.ContainerTxn.Serialize" SelectMethod="SelectDetails" 
        DataObjectTypeName="UO_Service.ContainerTxn.SerializeDetail"
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
<td><asp:Button ID="btnSerialize" runat="server" Text="<%$ Resources:Execute %>" onclick="btnSerialize_Click"/></td>
        </tr>
        <tr>
            <td><asp:Localize ID="locCompleteMessage" runat="server" Text="<%$ Resources:MES,CompleteMessage %>" /></td>
            <td><asp:Label ID="lblCompleteMsg" runat="server" Text="" /></td>
        </tr>
    </table>
</asp:Content>
