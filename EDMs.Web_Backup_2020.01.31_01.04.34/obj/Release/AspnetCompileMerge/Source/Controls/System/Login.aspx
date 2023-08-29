<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EDMs.Web.Login" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../CSS/style.css" rel="stylesheet" type="text/css" />
    
</head>
<body style="overflow:hidden;">
    <div id="login-wrapper" class="png_bg">
        <div id="login-top">
            <img src="../../Images/THEA_Logo_vn.png" alt="The A"/>
        </div>
        <div id="login-content">
            <form id="frmLogin" runat="server">
                <div>
                    <label>User Name</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="text-input"></asp:TextBox>
                </div>
                <div class="login-require"><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUsername" ErrorMessage="Please enter user name." /></div>
                <div>
                    <label>Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="text-input"></asp:TextBox>
                </div>
                <div class="login-require"><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" ErrorMessage="Please enter password." /></div>
                <div>
                    <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="button" OnClick="btnLogin_Click" />
                </div>
                <div><asp:Label ID="lblMessage" runat="server" CssClass="login-error"></asp:Label></div>
            </form>
        </div>
    </div>
    <div id="dummy"></div>
</body>
</html>
