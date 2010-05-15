<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResourceInfo.ascx.cs" Inherits="UO_WebApplication.ResourceInfo" %>
<div>
    <table class="table"><caption><asp:Localize ID="locResourceInfo" runat="server" Text="<%$ Resources:MES,ResourceInfo %>" /></caption>
        <thead>
            <tr>
          <th scope="col" abbr="status"><asp:Localize ID="locResourceStatus" runat="server" Text="<%$ Resources:MES,ResourceStatus %>" /></th>
          <th scope="col"><asp:Localize ID="locLastStatusChangeDate" runat="server" Text="<%$ Resources:MES,LastStatusChangeDate %>" /></th>
            </tr>
        </thead>
        <tr>
            <td><asp:TextBox ID="txtResourceStatus" runat="server" ReadOnly="true" /></td>
            <td><asp:TextBox ID="txtLastStatusChangeDate" runat="server" ReadOnly="true" /></td>
        </tr>
    </table>
</div>