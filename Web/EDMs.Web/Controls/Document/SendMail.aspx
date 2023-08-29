<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendMail.aspx.cs" Inherits="EDMs.Web.Controls.Document.SendMail" validateRequest="false" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox, Version=3.3.0.22838, Culture=neutral, PublicKeyToken=5962a4e684a48b87" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
	        overflow:auto;
        }
        
        #selectEmailValidate{
            margin-top: 5px;
            margin-bottom: -3px
        }

        .RadComboBoxDropDown_Windows7 .rcbHovered {
            background: coral !important;
        }

        .RadComboBoxDropDown_Default .rcbHovered {
               background-color: #46A3D3;
               color: #fff;
           }
           .RadComboBoxDropDown .rcbItem, .RadComboBoxDropDown .rcbHovered, .RadComboBoxDropDown .rcbDisabled, .RadComboBoxDropDown .rcbLoading, .RadComboBoxDropDown .rcbCheckAllItems, .RadComboBoxDropDown .rcbCheckAllItemsHovered {
               margin: 0 0px;
           }
           .RadComboBox .rcbInputCell .rcbInput{
            border-left-color: #46A3D3 !important;
            border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;
            border-style: solid;
            border-width: 1px 1px 1px 5px;
            color: #000000;
            float: left;
            font: 12px "segoe ui";
            margin: 0;
            padding: 2px 5px 3px;
            vertical-align: middle;
            width: 99%
           }
           .RadComboBox table td.rcbInputCell, .RadComboBox .rcbInputCell .rcbInput {
               padding-left: 0px !important;
           }
              div.rgEditForm label {
               float: right;
            text-align: right;
            width: 72px;
           }
           .rgEditForm {
               text-align: right;
           }
           .RadComboBox {
               border-bottom: none !important;
           }
           .RadUpload .ruFileWrap {
               overflow: visible !important;
           }

        /*#grdContactHistory
        {
            height: 300px !important;
        }

        #grdContactHistory_GridData
        {
            height: 250px !important;
        }*/
    </style>

    <script src="../../Scripts/jquery-1.7.1.js" type="text/javascript"></script>
   
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
        <table style="width: 100%">
            <tr>
                <td colspan="2" style="width: 100%">
                    <telerik:RadToolBar ID="SendMailMenu" runat="server" Width="100%" OnButtonClick="SendMailMenu_OnButtonClick">
                        <Items>
                            <telerik:RadToolBarButton runat="server" Text="Send" Value="SendMail" ImageUrl="~/Images/sendmail.png"></telerik:RadToolBarButton>
                            <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                        </Items>
                    </telerik:RadToolBar>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; padding-right: 5px; width: 5%">
                    To
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlEmail" runat="server" Width="903px" Height="140" Skin="Windows7"
                       CheckBoxes="true">
                    </telerik:RadComboBox>
                    <asp:CustomValidator runat="server" ID="selectEmailValidate" ValidateEmptyText="True" CssClass="dnnFormMessage dnnFormErrorModule"
                    OnServerValidate="ServerValidationEmptyEmailAddress" Display="Dynamic" ControlToValidate="ddlEmail"/>
                </td>
            </tr>
            
            <tr>
                <td style="text-align: left; padding-right: 5px; width: 5%">
                    CC
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlEmailCC" runat="server" Width="903px" Height="140" Skin="Windows7"
                        CheckBoxes="true">
                    </telerik:RadComboBox>
                </td>
            </tr>

            <tr>
                <td style="text-align: left; padding-right: 5px; width: 5%">
                    Subject
                </td>
                <td>
                    <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                        <asp:TextBox ID="txtSubject" runat="server" Width="886" CssClass="min25Percent"/>
                    </div>
                </td>
            </tr>
            <tr>
                <%--<td colspan="2" style="width: 100%">
                    <%--<telerik:RadEditor ID="txtEmailBody" runat="server" Width="950px" Height="350px" Skin="Windows7"
                        ToolbarMode="Default" ToolsFile="~/Controls/Document/XML/ToolsFile.xml" EnableResize="False">
                    </telerik:RadEditor>--%>
                   <%-- <FTB:FreeTextBox id="FreeTextBox1" runat="server" Width="950px" Height="280px"/>--%>
                    <%--<telerik:RadEditor ID="txtEmail" runat="server" Width="950px" Height="350px" Skin="Windows7"
                        ToolbarMode="Default" ToolsFile="~/Controls/Document/XML/ToolsFile.xml" EnableResize="False" OnClientShow="fixEditor">
                        
                    </telerik:RadEditor>--%>
               <td colspan="2" style="width: 100%">
                    <telerik:RadEditor ID="txtEmailBody" runat="server" Width="950px" Height="500px" Skin="Windows7"
                        ToolbarMode="Default" ToolsFile="~/Controls/Document/XML/ToolsFile.xml" EnableResize="False">
                    </telerik:RadEditor>             
                 </td>
            </tr>
        </table>  
        
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument">
            
        </telerik:RadAjaxManager>

        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;
                function pageLoad() {
                    ajaxManager = $find("<%=ajaxDocument.ClientID %>");
                }


                function Close() {
                    GetRadWindow().close();
                }

                function GetRadWindow() {
                    var oWindow = null;
                    if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
                    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz as well)

                    return oWindow;
                }

            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
