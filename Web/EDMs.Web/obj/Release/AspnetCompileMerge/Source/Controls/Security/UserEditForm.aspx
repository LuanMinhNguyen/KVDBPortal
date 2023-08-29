<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserEditForm.aspx.cs" Inherits="EDMs.Web.Controls.Security.UserEditForm" ValidateRequest ="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../Content/styles.css" rel="stylesheet" type="text/css" />

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
    <style type="text/css">
        
        .RadComboBoxDropDown_Default .rcbHovered {
               background-color: #46A3D3;
               color: #fff;
           }
           .RadComboBoxDropDown .rcbItem, .RadComboBoxDropDown .rcbHovered, .RadComboBoxDropDown .rcbDisabled, .RadComboBoxDropDown .rcbLoading, .RadComboBoxDropDown .rcbCheckAllItems, .RadComboBoxDropDown .rcbCheckAllItemsHovered {
               margin: 0 0px;
           }
           .RadComboBox .rcbInputCell .rcbInput{
            border-left-color: #FF0000 !important;
            border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;
            border-style: solid;
            border-width: 1px 1px 1px 5px;
            color: #000000;
            float: left;
            font: 12px "segoe ui";
            margin: 0;
            padding: 2px 5px 3px;
            vertical-align: middle;
            width: 100px;
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
               width: 115px !important;
               border-bottom: none !important;
           }
           .RadUpload .ruFileWrap {
               overflow: visible !important;
           }

        #panel
        {
            height: 60px;
            display: none;
        }

        .btn-slide
        {
            background: url(../../images/btnDN.gif) no-repeat;
            display: block;
            font-size: 14px;
            height: 22px;
            line-height: 22px;
            outline: medium none;
            position: relative;
            text-decoration: none;
        }

        .active
        {
            background: url(../../images/btnUP.gif) no-repeat;
        }

        .cfgTitle
        {
            color: #4C607A;
            font-size: 12px;
            font-weight: bold;
            padding: 0 8px 0 20px;
        }
    </style>
    <title></title>
</head>
<body>
    <form id="frmUserEdit" runat="server">
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
                
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">User name
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="divFileName">
                            <asp:TextBox runat="server" ID="txtUsername"  Style="width: 300px;" CssClass="min25Percent qlcbFormRequired"></asp:TextBox>
                            <asp:CustomValidator runat="server" ID="fileNameValidator" ValidateEmptyText="True" CssClass="dnnFormMessage dnnFormErrorModule"
                            OnServerValidate="ServerValidate" Display="Dynamic" ControlToValidate="txtUsername" />

                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Full name
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div1">
                            <asp:TextBox runat="server" ID="txtFullName"  Style="width: 300px;" CssClass="min25Percent qlcbFormRequired"></asp:TextBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Group
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div5">
                            <asp:DropDownList ID="ddlRoles" runat="server" CssClass="min25Percent qlcbFormRequired" Style="width: 315px; max-width: 315px"
                                OnSelectedIndexChanged="ddlRoles_OnSelectedIndexChanged" AutoPostBack="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 600px; display: none">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Role
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div6">
                            <asp:DropDownList ID="ddlTitle" runat="server" CssClass="min25Percent" Style="width: 315px; max-width: 315px"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Position
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox runat="server" ID="txtPosition"  Style="width: 300px;" CssClass="min25Percent qlcbFormRequired"></asp:TextBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">E-mail
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div2">
                            <asp:TextBox runat="server" ID="txtEmail"  Style="width: 300px;" CssClass="min25Percent"></asp:TextBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px; display: none">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Discipline
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlDiscipline" runat="server" CssClass="min25Percent" Style="width: 315px; max-width: 315px"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:RadioButton runat="server" Visible="false" GroupName="Role" Text="Project Director" ID="IsPD"/>
                            <asp:RadioButton runat="server" GroupName="Role" Text="DC/Project Director" ID="IsDC" />
                            <asp:RadioButton runat="server" GroupName="Role" Text="Leader" ID="cbLeader"/>
                            <asp:RadioButton runat="server" GroupName="Role" Text="Engineer" Checked="True" ID="cbEngineer"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 600px;display: none">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Phone
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div3">
                            <asp:TextBox runat="server" ID="txtHomePhone"  Style="width: 300px;" CssClass="min25Percent"></asp:TextBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;display: none">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Cell Phone
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div4">
                            <asp:TextBox runat="server" ID="txtCellPhone"  Style="width: 300px;" CssClass="min25Percent"></asp:TextBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 600px; display: none">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Confidentiality
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlConfidentiality" runat="server" CssClass="min25Percent" Style="width: 315px; max-width: 315px"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Upload Signed Image
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadAsyncUpload runat="server" ID="docuploader"
                                MultipleFileSelection="Disabled" TemporaryFileExpiration="05:00:00" 
                                EnableInlineProgress="true" Width="350px"
                                Localization-Cancel="Cancel" CssClass="min25Percent qlcbFormRequired"
                                Localization-Remove="Remove" Localization-Select="Select"  Skin="Windows7">
                            </telerik:RadAsyncUpload><br />
                            <asp:Label runat="server" Text="Uploaded Signed" ID="lblUploadedSigned"></asp:Label> <br />
                            <asp:Image runat="server" Width="200" ID="imgUserSigned" />
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <%-- <li style="width: 600px;">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div8">
                            <asp:CheckBox runat="server" ID="cbIsDC" Text="Is DC/Project Director"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>--%>
                 <li style="width: 600px; display:none">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div8">
                            <asp:CheckBox runat="server" ID="cbReceivedEmail" Text="Received Exchanges Email "/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                

                <li style="width: 600px; display: none">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div7">
                            <asp:CheckBox runat="server" ID="cbActive" Text="Active"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

            </ul>
        </div>

        <!--Button Command View Pane-->
        <table width="100%">
            <tr>
                <td align="center">
                    <telerik:RadButton ID="btnCapNhat" runat="server" Text="Save" OnClick="btnCapNhat_Click" >
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                    <telerik:RadButton ID="btncancel" runat="server" Text="Cancel" Visible="False"
                        OnClick="btncancel_Click">
                        <Icon PrimaryIconUrl="../../Images/Cancel.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                </td>
            </tr>
        </table>
        
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="ddlRoles">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlTitle"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ProjectControl"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </form>
</body>
</html>
