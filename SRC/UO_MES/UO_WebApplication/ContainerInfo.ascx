<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContainerInfo.ascx.cs" Inherits="UO_WebApplication.ContainerInfo" %>
<div>
    <table class="table"><caption><asp:Localize ID="locContainerInfo" runat="server" Text="<%$ Resources:MES,ContainerInfo %>" /></caption>
        <thead>
            <tr>
          <th scope="col" abbr="qty"><asp:Localize ID="locQty" runat="server" Text="<%$ Resources:MES,Qty %>" /></th>
          <th scope="col"><asp:Localize ID="locProduct" runat="server" Text="<%$ Resources:MES,Product %>" /></th>
          <th scope="col" abbr="step"><asp:Localize ID="locContainerStep" runat="server" Text="<%$ Resources:MES,ContainerStep %>" /></th>
          <th scope="col" abbr="status"><asp:Localize ID="locContainerStatus" runat="server" Text="<%$ Resources:MES,ContainerStatus %>" /></th>
            </tr>
        </thead>
        <tr>
            <td><asp:TextBox ID="txtContainerQty" runat="server" ReadOnly="true" /></td>
            <td><asp:TextBox ID="txtProduct" runat="server" ReadOnly="true" /></td>
            <td><asp:TextBox ID="txtContainerStep" runat="server" ReadOnly="true" /></td>
            <td><asp:TextBox ID="txtContainerStatus" runat="server" ReadOnly="true" /></td>
        </tr>
    </table>
</div>