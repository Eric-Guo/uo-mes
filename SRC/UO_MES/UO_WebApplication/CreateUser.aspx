<%@ Page Title="" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="CreateUser.aspx.cs" Inherits="UO_WebApplication.CreateUser" %>
<asp:Content ID="Holder1" ContentPlaceHolderID="Holder1" runat="server">
      <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" ActiveStepIndex="0" ContinueDestinationPageUrl="~/Default.aspx">
        <WizardSteps>
          <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
          </asp:CreateUserWizardStep>
          <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
          </asp:CompleteWizardStep>
        </WizardSteps>
      </asp:CreateUserWizard>
</asp:Content>
