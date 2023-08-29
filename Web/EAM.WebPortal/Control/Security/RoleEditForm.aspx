<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleEditForm.aspx.cs" Inherits="EAM.WebPortal.Control.Security.RoleEditForm" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../Resources/Style/Content/styles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .RadComboBoxDropDown_Windows7 {
            width: 397px !important;
            height: 100px !important;
        }
        .RadComboBoxDropDown .rcbScroll {
            height: 199px !important;
        }
        .RadComboBoxDropDown_Default .rcbHovered {
               background-color: #46A3D3;
               color: #fff;
           }
           .RadComboBoxDropDown .rcbItem, .RadComboBoxDropDown .rcbHovered, .RadComboBoxDropDown .rcbDisabled, .RadComboBoxDropDown .rcbLoading, .RadComboBoxDropDown .rcbCheckAllItems, .RadComboBoxDropDown .rcbCheckAllItemsHovered {
               margin: 0 0px;
           }
           .RadComboBox .rcbInputCell .rcbInput{
            border-left-color:#46A3D3 !important;
            border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;
            border-style: solid;
            border-width: 1px 1px 1px 5px;
            color: #000000;
            float: left;
            font: 12px "segoe ui";
            margin: 0;
            padding: 2px 5px 3px;
            vertical-align: middle;
            width: 392px;
           }
           .RadComboBox table td.rcbInputCell, .RadComboBox .rcbInputCell .rcbInput {
               padding-left: 0px !important;
               padding-right: 0px !important;
           }
              div.rgEditForm label {
               float: right;
            text-align: right;
            width: 78px;
           }
           .rgEditForm {
               text-align: right;
           }
           .RadComboBox {
               width: 115px !important;
               border-bottom: none !important;
           }
           .RadUpload .ruFileWrap {
               overflow: visible !important;
           }
    </style>
    <script type="text/javascript">
        function CloseAndRebind(args) {
            GetRadWindow().BrowserWindow.refreshGrid(args);
            GetRadWindow().close();
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz as well)

            return oWindow;
        }

        function CancelEdit() {
            GetRadWindow().close();
        }
    </script>
    <title></title>
</head>
<body>
    <form id="frmRoleEdit" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager>
         <div style="width: 100%">
            <ul style="list-style-type: none">
                <div class="qlcbFormItem">
                    <div class="dnnFormMessage dnnFormInfo">
                        <div class="dnnFormItem dnnFormHelp dnnClear">
                            <p class="dnnFormRequired" style="float: left;">
                                <span style="text-decoration: underline;">Notes</span>: All fields marked with a red are required.
                            </p>
                            <br />
                        </div>
                    </div>
                </div>
            </ul>
        </div>
        <div style="width: 100%" runat="server" ID="divContent">
            <ul style="list-style-type: none">
                
                 <li style="width: 500px; display: none" runat="server" ID="IndexContractor">
                    <div>
                        <label style="width: 70px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Member Of
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlContractor" runat="server" CssClass="min25Percent" Width="416px" />
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                <li style="width: 500px;">
                    <div>
                        <label style="width: 70px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Name
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="divFileName">
                            <asp:TextBox runat="server" ID="txtRoleName"  Style="width: 400px;" CssClass="min25Percent qlcbFormRequired"></asp:TextBox>
                            <asp:CustomValidator runat="server" ID="fileNameValidator" ValidateEmptyText="True" CssClass="dnnFormMessage dnnFormErrorModule"
                            OnServerValidate="ServerValidate" Display="Dynamic" ControlToValidate="txtRoleName" />

                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                
                
                <li style="width: 505px; display: none">
                    <div>
                        <label style="width: 70px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Category</span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlCategory" runat="server" Skin="Windows7"
                                CheckBoxes="true" EnableCheckAllItemsCheckBox="true"></telerik:RadComboBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 500px;">
                    <div>
                        <label style="width: 70px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Description
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div1">
                            <asp:TextBox runat="server" ID="txtDescription"  Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" Rows="3"></asp:TextBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px; display: none">
                    <div>
                        <label style="width: 70px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Color
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadColorPicker RenderMode="Lightweight" runat="server"
                                ID="txtColor" Preset="Standard" Width="420" Skin="Windows7">
                            </telerik:RadColorPicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px; display: none">
                    <div>
                        <label style="width: 70px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div2">
                            <asp:CheckBox ID="cbIsInternal" runat="server" Text="PECC2 Internal Group"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;display: none">
                    <div>
                        <label style="width: 70px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div5">
                            <asp:CheckBox ID="cbIsLimitedView" runat="server" Text="Limited Data View (MR, WR)"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
            </ul>
        </div>

        <table width="100%">
            <tr>
                <td colspan="2" align="center">
                    <telerik:RadButton ID="btnCapNhat" runat="server" Text="Save" OnClick="btnCapNhat_Click" ValidationGroup="grpRole">
                        <Icon PrimaryIconUrl="../../Resources/Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                    <telerik:RadButton ID="btncancel" runat="server" Text="Cancel"
                        OnClick="btncancel_Click">
                        <Icon PrimaryIconUrl="../../Resources/Images/Cancel.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
