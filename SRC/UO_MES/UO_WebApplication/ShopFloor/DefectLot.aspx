<%@ Page Title="<%$ Resources:Caption %>" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="DefectLot.aspx.cs" Inherits="UO_WebApplication.ShopFloor.DefectLot" %>
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
    <asp:Localize ID="locQtyInspected" runat="server" Text="<%$ Resources:MES,QtyInspected %>" />:
            </td>
            <td>
    <asp:TextBox ID="txtQtyInspected" runat="server" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" runat="server" DataSourceID="ODS_Details" DataKeyNames="ServiceDetailID" AutoGenerateColumns="False" >
        <Columns>
            <asp:CommandField ShowEditButton="True" ShowCancelButton="True" ShowDeleteButton="True" ButtonType="Image" EditImageUrl="~/images/dg_edit.gif" CancelImageUrl="~/images/dg_cancel.gif" DeleteImageUrl="~/images/dg_delete.gif" UpdateImageUrl="~/images/dg_update.gif" />
            <asp:BoundField DataField="DefectQty" HeaderText="<%$ Resources:MES,DefectQty %>" SortExpression="DefectQty" />
            <asp:TemplateField>
                <HeaderTemplate><asp:Localize ID="locDefectReason" runat="server" Text="<%$ Resources:MES,DefectReason %>" /></HeaderTemplate>
                <ItemTemplate><%# Eval("DefectReason_Name")%></ItemTemplate>
                <EditItemTemplate>
<asp:DropDownList ID="ddlDefectReason" runat="server"  SelectedValue='<%# Bind("DefectReason_Name") %>' DataSourceID="OADS_DefectReason" DataTextField="DisplayName" />
<telerik:OpenAccessDataSource ID="OADS_DefectReason" runat="server" 
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Execution.Reason.DefectReason">
</telerik:OpenAccessDataSource>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Comment" HeaderText="<%$ Resources:MES,Comment %>" SortExpression="Comment" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="ODS_Details" runat="server" 
        TypeName="UO_Service.ContainerTxn.Defect" SelectMethod="SelectDetails" 
        DataObjectTypeName="UO_Service.ContainerTxn.DefectDetail"
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
<td><asp:Button ID="btnDefect" runat="server" Text="<%$ Resources:Execute %>" onclick="btnDefect_Click"/></td>
        </tr>
        <tr>
            <td><asp:Localize ID="locCompleteMessage" runat="server" Text="<%$ Resources:MES,CompleteMessage %>" /></td>
            <td><asp:Label ID="lblCompleteMsg" runat="server" Text="" /></td>
        </tr>
    </table>
</asp:Content>
