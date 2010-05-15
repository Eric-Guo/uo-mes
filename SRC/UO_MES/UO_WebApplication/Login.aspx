<%@ Page Title="" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="UO_WebApplication.Login" %>
<asp:Content ID="Holder1" ContentPlaceHolderID="Holder1" runat="server">
    <asp:Login ID="Login1" runat="server" CreateUserUrl="CreateUser.aspx"
        CreateUserText="<%$ Resources:Create_User %>" 
        PasswordRequiredErrorMessage="<%$ Resources:Password_Required %>"
        UserNameRequiredErrorMessage="<%$ Resources:Username_Required %>"
        UserNameLabelText="<%$ Resources:Username %>"
        PasswordLabelText="<%$ Resources:Password %>"
        TitleText="<%$ Resources:Login_Title %>"
        RememberMeText="<%$ Resources:Remember_Me %>"
        LoginButtonText="<%$ Resources:Login %>"
        FailureText="<%$ Resources:Failure %>">
    </asp:Login>
</asp:Content>
