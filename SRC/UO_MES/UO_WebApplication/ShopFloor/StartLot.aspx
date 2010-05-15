<%@ Page Title="<%$ Resources:Caption %>" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="StartLot.aspx.cs" Inherits="UO_WebApplication.ShopFloor.StartLot" %>
<%@ Register Assembly="Telerik.OpenAccess" Namespace="Telerik.OpenAccess" TagPrefix="telerik" %>
<asp:Content ID="Holder1" ContentPlaceHolderID="Holder1" runat="server">
    <table class="table" border="1" cellspacing="0" summary="Start Lot Form">
      <caption><asp:Localize ID="locCaption" runat="server" Text="<%$ Resources:Caption %>" /></caption>
      <tbody>
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
<td><asp:Localize ID="locWorkflow" runat="server" Text="<%$ Resources:MES,Workflow %>" />:</td>
<td><asp:DropDownList ID="ddlWorkflow" runat="server" DataSourceID="OADS_Workflow" DataTextField="DisplayName" DataValueField="Workflow_ID"
    AutoPostBack="true" onselectedindexchanged="ddlWorkflow_SelectedIndexChanged" ondatabound="ddlWorkflow_DataBound" />
<telerik:OpenAccessDataSource ID="OADS_Workflow" runat="server"
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Workflow.Workflow">
</telerik:OpenAccessDataSource>
          </td>
<td><asp:Localize ID="locWorkflowStep" runat="server" Text="<%$ Resources:MES,WorkflowStep %>" />:</td>
<td><asp:DropDownList ID="ddlStartStep" runat="server" DataSourceID="OADS_StartStep" DataTextField="DisplayName" />
<telerik:OpenAccessDataSource ID="OADS_StartStep" runat="server" 
ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Workflow.Step" Where="Workflow.Workflow_ID = @Workflow_ID">
    <WhereParameters>
        <asp:Parameter DefaultValue="" Name="Workflow_ID" Type="Int32" />
    </WhereParameters>
</telerik:OpenAccessDataSource>
          </td>
        </tr>
      </tbody>
    </table>
    <asp:GridView ID="GridView1" runat="server" DataSourceID="ODS_Details" DataKeyNames="ServiceDetailID" AutoGenerateColumns="False" >
        <Columns>
            <asp:CommandField ShowEditButton="True" ShowCancelButton="True" ShowDeleteButton="True" ButtonType="Image" EditImageUrl="~/images/dg_edit.gif" CancelImageUrl="~/images/dg_cancel.gif" DeleteImageUrl="~/images/dg_delete.gif" UpdateImageUrl="~/images/dg_update.gif" />
            <asp:BoundField DataField="ContainerName" HeaderText="<%$ Resources:MES,ContainerName %>" SortExpression="ContainerName" />
            <asp:TemplateField>
                <HeaderTemplate><asp:Localize ID="locContainerLevel" runat="server" Text="<%$ Resources:MES,ContainerLevel %>" /></HeaderTemplate>
                <ItemTemplate><%# Eval("ContainerLevel_Name") %></ItemTemplate>
                <EditItemTemplate>
<asp:DropDownList ID="ddlLevel" runat="server"  SelectedValue='<%# Bind("ContainerLevel_Name") %>' DataSourceID="OADS_Level" DataTextField="DisplayName" />
<telerik:OpenAccessDataSource ID="OADS_Level" runat="server" 
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Process.ContainerLevel">
</telerik:OpenAccessDataSource>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="DueDate" HeaderText="<%$ Resources:MES,DueDate %>" SortExpression="DueDate" />
            <asp:TemplateField>
                <HeaderTemplate><asp:Localize ID="locProduct" runat="server" Text="<%$ Resources:MES,Product %>" /></HeaderTemplate>
                <ItemTemplate><%# Eval("Product_Revision")%></ItemTemplate>
                <EditItemTemplate>
<asp:DropDownList ID="ddlProduct" runat="server"  SelectedValue='<%# Bind("Product_Revision") %>' DataSourceID="OADS_Product" DataTextField="DisplayName" />
<telerik:OpenAccessDataSource ID="OADS_Product" runat="server" 
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Process.Product">
</telerik:OpenAccessDataSource>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Qty" HeaderText="<%$ Resources:MES,Qty %>" SortExpression="Qty" />
            <asp:TemplateField>
                <HeaderTemplate><asp:Localize ID="locUOM" runat="server" Text="<%$ Resources:MES,UOM %>" /></HeaderTemplate>
                <ItemTemplate><%# Eval("UOM_Name")%></ItemTemplate>
                <EditItemTemplate>
<asp:DropDownList ID="ddlUOM" runat="server"  SelectedValue='<%# Bind("UOM_Name") %>' DataSourceID="OADS_UOM" DataTextField="DisplayName" />
<telerik:OpenAccessDataSource ID="OADS_UOM" runat="server" 
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Process.UOM">
</telerik:OpenAccessDataSource>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate><asp:Localize ID="locContainerStatus" runat="server" Text="<%$ Resources:MES,ContainerStatus %>" /></HeaderTemplate>
                <ItemTemplate><%# Eval("ContainerStatus_Name")%></ItemTemplate>
                <EditItemTemplate>
<asp:DropDownList ID="ddlContainerStatus" runat="server"  SelectedValue='<%# Bind("ContainerStatus_Name") %>' DataSourceID="OADS_ContainerStatus" DataTextField="DisplayName" />
<telerik:OpenAccessDataSource ID="OADS_ContainerStatus" runat="server" 
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Execution.Status.ContainerStatus">
</telerik:OpenAccessDataSource>
                </EditItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="ODS_Details" runat="server" 
        TypeName="UO_Service.ContainerTxn.Start" SelectMethod="SelectDetails" 
        DataObjectTypeName="UO_Service.ContainerTxn.StartDetail"
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
<td><asp:Button ID="btnStart" runat="server" Text="<%$ Resources:Execute %>" onclick="btnStart_Click"/></td>
        </tr>
        <tr>
            <td><asp:Localize ID="locCompleteMessage" runat="server" Text="<%$ Resources:MES,CompleteMessage %>" /></td>
            <td><asp:Label ID="lblCompleteMsg" runat="server" Text="" /></td>
        </tr>
    </table>
</asp:Content>