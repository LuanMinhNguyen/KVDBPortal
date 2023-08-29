<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePasswordForm.aspx.cs" Inherits="EDMs.Web.ChangePasswordForm" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../Content/styles.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/utils.js" type="text/javascript"></script>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function CloseWindow(sender, args) {
                GetRadWindow().close();
                top.location.href = top.location.href;
            }
        </script>
    </telerik:RadCodeBlock>
    <title>Change password</title>
</head>
<body>
    <form id="frmChangePassword" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" CdnSettings-TelerikCdn="Disabled"/>
        <table width="100%" style="padding-top: 15px">
            <tr>
                <td width="30%" style="text-align: right; padding-right: 5px">
                    <span>Old password</span>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtOldPassword" TextMode="Password" Width="300" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:RequiredFieldValidator 
                        runat="server" 
                        ControlToValidate="txtOldPassword"
                        ValidationGroup="grpChangePassword"
                        Display="Dynamic"
                        ErrorMessage="Please enter old password." />
                    <asp:CompareValidator
                        runat="server"
                        Operator="NotEqual"
                        ControlToCompare="txtNewPassword"
                        ControlToValidate="txtOldPassword"
                        ValidationGroup="grpChangePassword"
                        Display="Dynamic"
                        ErrorMessage="Old and new passwords overlap." />
                    <asp:CustomValidator 
                        ID="OldPasswordValidator" 
                        runat="server" 
                        ControlToValidate="txtOldPassword" 
                        ValidationGroup="grpChangePassword"
                        Display="Dynamic"
                        OnServerValidate="OldPasswordValidator_OnServerValidate"
                        ErrorMessage="Old password is incorrect." />
                </td>
            </tr>
            <tr>
                <td style="text-align: right; padding-right: 5px">New password</td>
                <td><telerik:RadTextBox runat="server" ID="txtNewPassword" TextMode="Password" Width="300" /></td>
            </tr>
            <tr>
                <td></td>
                <td><asp:RequiredFieldValidator
                    runat="server" 
                    ControlToValidate="txtNewPassword"
                    ValidationGroup="grpChangePassword"
                    Display="Dynamic"
                    ErrorMessage="Please enter new password." />

                </td>
            </tr>
            <tr>
                <td style="text-align: right; padding-right: 5px">Re enter new password</td>
                <td><telerik:RadTextBox runat="server" ID="txtNewSecondPassword" TextMode="Password" Width="300" /></td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:CompareValidator
                        runat="server"
                        Operator="Equal"
                        ControlToCompare="txtNewPassword"
                        ControlToValidate="txtNewSecondPassword"
                        ValidationGroup="grpChangePassword"
                        Display="Dynamic"
                        ErrorMessage="The password you enter did not match." />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <telerik:RadButton ID="btnCapNhat" runat="server" Text="Save" OnClick="btnCapNhat_Click" ValidationGroup="grpChangePassword">
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                    <telerik:RadButton ID="btnCancel" runat="server" Text="Cancel" OnClientClicked="CloseWindow">
                        <Icon PrimaryIconUrl="../../Images/Cancel.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                </td>
            </tr>
        </table>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                function pageLoad() {
                    $find("<%=txtOldPassword.ClientID%>").focus();
                }
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
