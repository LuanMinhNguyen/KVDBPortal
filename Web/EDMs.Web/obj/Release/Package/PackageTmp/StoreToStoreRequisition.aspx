<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StoreToStoreRequisition.aspx.cs" Inherits="EDMs.Web.StoreToStoreRequisition" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <telerik:RadWizard RenderMode="Lightweight" ID="RadWizard1" runat="server" Height="350px">
                <WizardSteps>
                    <telerik:RadWizardStep Title="Log in" CssClass="loginStep">
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserNameTextBox" Text="User name:"></asp:Label>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="UserNameTextBox" runat="server" ></telerik:RadTextBox>
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="PasswordTextBox" Text="Password:"></asp:Label>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="PasswordTextBox" runat="server" ></telerik:RadTextBox>
                    </telerik:RadWizardStep>
                    <telerik:RadWizardStep Title="Attach Database">
                        <label>Please select a data base file:</label>
                        <telerik:RadAsyncUpload RenderMode="Lightweight" ID="DataBaseUpload" runat="server"  AllowedFileExtensions=".mdf"></telerik:RadAsyncUpload>
                    </telerik:RadWizardStep>
                    <telerik:RadWizardStep Title="Approximately Load">
                        <label>Number of visiting users:</label>
                        <telerik:RadComboBox RenderMode="Lightweight" ID="UsersNumber" runat="server"  Width="300px">
                            <Items>
                                <telerik:RadComboBoxItem Text="50-100 Users" Value="50" />
                                <telerik:RadComboBoxItem Text="500-1000 Users" Value="500" />
                                <telerik:RadComboBoxItem Text="Above 2000" Value="2000" />
                            </Items>
                        </telerik:RadComboBox>
                    </telerik:RadWizardStep>
                </WizardSteps>
            </telerik:RadWizard>
        </div>
    </form>
</body>
</html>
