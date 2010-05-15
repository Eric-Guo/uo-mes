<%@ Page Title="" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="UO_WebApplication.About" %>
<asp:Content ID="Holder1" ContentPlaceHolderID="Holder1" runat="server">
<h1><asp:Localize ID="locCaption" runat="server" Text="<%$ Resources:Caption %>" /></h1>
<hr />
<p><asp:Localize ID="locPara_1" runat="server" Text="<%$ Resources:Para_1 %>" /></p>
</asp:Content>
