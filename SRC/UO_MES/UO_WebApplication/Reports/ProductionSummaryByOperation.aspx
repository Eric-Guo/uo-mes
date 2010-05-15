<%@ Page Title="" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="ProductionSummaryByOperation.aspx.cs" Inherits="UO_WebApplication.ProductionSummaryByOperation" %>
<asp:Content ID="Head1" ContentPlaceHolderID="Head1" runat="server">
<% if (false) { %>
<script type="text/javascript" src="../src/jquery-1.3.2-vsdoc.js"></script>
<% } %>
<%-- We using plugin http://plugins.jquery.com/project/dyndatetime --%>
<script src="../src/jquery.dynDateTime.js" type="text/javascript"></script>
<% if ("zh-CN"  == Context.Profile["Language"].ToString()) { %>
<script src="../src/dynDateTime/calendar-zh.js" type="text/javascript"></script>
<% } else { %>
<script src="../src/dynDateTime/calendar-en.js" type="text/javascript"></script>
<% } %>
<script type="text/javascript">
    jQuery(document).ready(function() {
        jQuery(".dateCutTime").dynDateTime({
            showsTime: true,
            ifFormat: "%Y-%m-%d %H:%M",
            daFormat: "%l;%M %p, %e %m,  %Y",
            electric: false,
            singleClick: false,
            button: ".next().next()"
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Holder1" runat="server">
    <h3 runat="server" id="reportTitle">MFG01 Production Summary Report (By Station)</h3>
    The selected report cut date is <asp:TextBox ID="dateCutTime" runat="server" SkinID="clear" Text="" class="dateCutTime" />
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
        ControlToValidate="dateCutTime" >required!</asp:RequiredFieldValidator>    
    <input type="button" class="button" value="Time Picker" />
    <asp:Button ID="btnRun" runat="server" Text="Run" class="button" onclick="btnRun_Click" />
    <asp:Button ID="btnExcel" runat="server" Text="Excel" class="button" onclick="btnExcel_Click" />
    <asp:Button ID="btnPDF" runat="server" Text="PDF" class="button" onclick="btnPDF_Click" />
    <hr />
    <asp:GridView ID="GridView1" runat="server" class="table" >
    </asp:GridView>
</asp:Content>
