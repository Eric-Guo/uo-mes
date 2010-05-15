<%@ Page Title="<%$ Resources:Caption %>" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="MES.aspx.cs" Inherits="UO_WebApplication.MES" %>
<%@ Register Assembly="Telerik.OpenAccess" Namespace="Telerik.OpenAccess" TagPrefix="telerik" %>
<asp:Content ID="Holder1" ContentPlaceHolderID="Holder1" runat="server">
    <table class="table" style="width:400px;">
        <caption><asp:Localize ID="locCaption" runat="server" Text="<%$ Resources:Caption %>" /></caption>
        <thead>
            <tr>
                <th scope="col" abbr="content"><asp:Localize ID="locProfileContent" runat="server" Text="<%$ Resources:ProfileContent %>" /></th>
                <th scope="col" abbr="value"><asp:Localize ID="locProfileValue" runat="server" Text="<%$ Resources:ProfileValue %>" /></th>
            </tr>
        </thead>
        <tr class="table-row-1">
            <td><asp:Localize ID="locFactory" runat="server" Text="<%$ Resources:MES,Factory %>" /></td>
            <td>
<asp:DropDownList ID="ddlFactory" runat="server" DataSourceID="OADS_Factory" DataTextField="DisplayName" />
<telerik:OpenAccessDataSource ID="OADS_Factory" runat="server"
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Physical.Factory">
</telerik:OpenAccessDataSource>
            </td>
        </tr>
        <tr class="table-row-2">
            <td><asp:Localize ID="locOperation" runat="server" Text="<%$ Resources:MES,Operation %>" /></td>
            <td>
<asp:DropDownList ID="ddlOperation" runat="server" DataSourceID="OADS_Operation" DataTextField="DisplayName" />
<telerik:OpenAccessDataSource ID="OADS_Operation" runat="server"
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Process.Operation">
</telerik:OpenAccessDataSource>
            </td>
        </tr>
        <tr class="table-row-1">
            <td><asp:Localize ID="locWorkCenter" runat="server" Text="<%$ Resources:MES,WorkCenter %>" /></td>
            <td>
<asp:DropDownList ID="ddlWorkCenter" runat="server" DataSourceID="OADS_WorkCenter" DataTextField="DisplayName" />
<telerik:OpenAccessDataSource ID="OADS_WorkCenter" runat="server"
    ObjectContextProvider="UO_Service.Base.ORM, UO_Service" TypeName="UO_Model.Physical.WorkCenter">
</telerik:OpenAccessDataSource>
            </td>
        </tr>
        <tr class="table-row-2">
            <td><asp:Localize ID="locLanguage" runat="server" Text="<%$ Resources:MES,Language %>" /></td>
            <td><asp:DropDownList ID="ddlLanguage" runat="server" EnableViewState="true">
            <asp:ListItem Value="en-US">English</asp:ListItem>
            <asp:ListItem Value="zh-CN">中文</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr class="table-row-1">
            <td><asp:Localize ID="locStyleTheme" runat="server" Text="<%$ Resources:MES,StyleTheme %>" /></td>
            <td><asp:DropDownList ID="ddlStyles" runat="server" EnableViewState="true" /></td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Footer1" ContentPlaceHolderID="Footer1" runat="server">
<asp:Button ID="btnSaveProfile" runat="server" onclick="btnSaveProfile_Click" Text="<%$ Resources:SaveProfile %>" />
</asp:Content>
