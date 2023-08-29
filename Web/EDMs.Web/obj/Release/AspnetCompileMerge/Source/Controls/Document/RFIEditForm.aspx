<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RFIEditForm.aspx.cs" Inherits="EDMs.Web.Controls.Document.RFIEditForm" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
	        overflow:visible;
        }
        .RadComboBoxDropDown_Windows7 {
            width: 416PX !important;
            height: 300px !important;
        }
        .RadComboBoxDropDown .rcbScroll {
            height: 299px !important;
        }
        .RadComboBoxDropDown_Default .rcbHovered {
               background-color: #46A3D3;
               color: #fff;
           }
           .RadComboBoxDropDown .rcbItem, .RadComboBoxDropDown .rcbHovered, .RadComboBoxDropDown .rcbDisabled, .RadComboBoxDropDown .rcbLoading, .RadComboBoxDropDown .rcbCheckAllItems, .RadComboBoxDropDown .rcbCheckAllItemsHovered {
               margin: 0 0px;
           }
           .RadComboBox .rcbInputCell .rcbInput {
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
            width: 399PX;
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
               width: 399PX !important;
               border-bottom: none !important;
           }
        .RadComboBoxDropDown .rcbItem, .RadComboBoxDropDown .rcbHovered, .RadComboBoxDropDown .rcbDisabled, .RadComboBoxDropDown .rcbLoading, .RadComboBoxDropDown .rcbCheckAllItems, .RadComboBoxDropDown .rcbCheckAllItemsHovered {
               margin: 0 0px;
           }
           .RadComboBox .rcbInputCell .rcbInput{
            /*border-left-color: #FF0000;*/
            /*border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;*/
            border-style: solid;
            border-width: 1px 1px 1px 5px;
            color: #000000;
            float: left;
            font: 12px "segoe ui";
            margin: 0;
            padding: 2px 5px 3px;
            vertical-align: middle;
            width: 399PX;
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

        .accordion dt a
        {
            /*letter-spacing: 0.1em;*/
            line-height: 1.2;
            margin: 0.5em auto 0.6em;
            padding: 0;
            text-align: left;
            text-decoration: none;
            display: block;
        }

        .accordion dt span {
            color: #085B8F;
            border-bottom: 1px solid #46A3D3;
            font-size: 1.0em;
            font-weight: bold;
            /*letter-spacing: 0.1em;*/
            line-height: 1.2;
            margin: 0.5em auto 0.6em;
            padding: 0;
            text-align: left;
            text-decoration: none;
            display: block;
        }

        #ddlParent_Input {
            width: 384px !important;
        }

        #rtvRefDocNo {
             border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3 !important;
            color: #000000 !important;
            font: 12px "segoe ui" !important;
            border-width: 1px !important;
            border-style: solid !important;
            border-left-width: 5px !important;
            padding: 2px 1px 3px !important;
            vertical-align: middle !important;
            margin: 0 !important;
            padding-left: 5px !important;
            padding-right: 5px !important;
         }
    </style>

    <script src="../../Scripts/jquery-1.7.1.js" type="text/javascript"></script>
    
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
            </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
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
        <div style="width: 100%; height: 100%" runat="server" ID="divContent">
            <ul style="list-style-type: none">
                
            <dl class="accordion">
                <dt style="width: 100%;">
                    <span>Main Detail</span>
                </dt>
            </dl>
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Number
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtNumber"  runat="server" Style="width: 400px;" CssClass="min25Percent qlcbFormRequired" ReadOnly="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
            
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Project Code
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtProjectCode"  runat="server" Style="width: 400px;" CssClass="min25Percent qlcbFormRequired"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                  <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Group
                            </span>
                        </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                           <asp:DropDownList ID="ddlGroup" runat="server" OnSelectedIndexChanged="ddlGroup_OnSelectedIndexChanged" CssClass="min25Percent qlcbFormRequired" Width="416px" AutoPostBack="true"/>
                        </div>
                        
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Year
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:radnumerictextbox type="Number" id="txtYear" runat="server" Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="70px" CssClass="min25Percent qlcbFormRequired">
                                <NumberFormat DecimalDigits="0"></NumberFormat>
                            </telerik:radnumerictextbox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Sequential Number
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                             <asp:TextBox ID="txtSequentialNumber" AutoPostBack="true" ReadOnly="true"  OnTextChanged="txtSequentialNumber_TextChanged" MaxLength="4" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                   <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 3px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Issued Date
                            </span>
                        </label>
                        <div style="float: left;  padding-top: 5px;"  class="qlcbFormItem">
                            <telerik:RadDatePicker ID="txtIssueDate"  runat="server" 
                                ShowPopupOnFocus="True" CssClass="qlcbFormRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormRequired" />
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Site Manager
                            </span>
                        </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                           <asp:TextBox ID="txtSiteManager"  runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                        
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">QA/QC Manager
                            </span>
                        </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                           <asp:TextBox ID="txtQAQCManager"  runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                        
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                <div class="qlcbFormItem" runat="server" ID="CreatedInfo" Visible="False">
                    <div class="dnnFormMessage dnnFormInfo">
                        <div class="dnnFormItem dnnFormHelp dnnClear">
                            <p class="dnnFormRequired" style="float: left;">
                                    <asp:Label ID="lblCreated" runat="server" ></asp:Label>
                                    <asp:Label ID="lblUpdated" runat="server" ></asp:Label>
                            </p>
                            <br />
                        </div>
                    </div>
                </div>
                
                <li style="width: 600px; padding-top: 10px; padding-bottom: 3px; text-align: center">
                    <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"  Width="70px" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                    <%--<telerik:RadButton ID="btncancel" runat="server" Text="Cancel" Width="70px" style="text-align: center"
                        OnClick="btncancel_Click">
                        <Icon PrimaryIconUrl="../../Images/Cancel.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>--%>

                </li>

                 <li style="width: 400px;" Runat="server" ID="blockError" Visible="False">
                    <div>
                        <label style="width: 60px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: red; text-align: right; ">Warning:
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                            <asp:Label runat="server" ID="lblError" Width="300px"></asp:Label>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
            </ul>
        </div>
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="txtSequentialNumber">
                     <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtNumber"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlGroup">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtNumber"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlSystem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlSubSystem"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                   
                <telerik:AjaxSetting AjaxControlID="ddlSystem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtOriginator"/>
                         <telerik:AjaxUpdatedControl ControlID="txtOriginating"/>
                         <telerik:AjaxUpdatedControl ControlID="txtReceiving"/>
                         <telerik:AjaxUpdatedControl ControlID="txtDocumenType"/>
                         <telerik:AjaxUpdatedControl ControlID="txtDiscipline"/>
                         <telerik:AjaxUpdatedControl ControlID="txtMaterial"/>
                         <telerik:AjaxUpdatedControl ControlID="txtWork"/>
                         <telerik:AjaxUpdatedControl ControlID="txtDrawing"/>
                         <telerik:AjaxUpdatedControl ControlID="txtEquipmentTagNumber"/>
                         <telerik:AjaxUpdatedControl ControlID="txtDepartmentcode"/>
                         <telerik:AjaxUpdatedControl ControlID="txtMRSequenceNo"/>
                         <telerik:AjaxUpdatedControl ControlID="txtDocumentSequenceNo"/>
                         <telerik:AjaxUpdatedControl ControlID="txtsheetno"/>
                         <telerik:AjaxUpdatedControl ControlID="txtPlant"/>
                         <telerik:AjaxUpdatedControl ControlID="txtArea"/>
                         <telerik:AjaxUpdatedControl ControlID="txtUnit"/>
                         <telerik:AjaxUpdatedControl ControlID="txtDocumentTitle"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;
              

                function pageLoad() {
                    ajaxManager = $find("<%=ajaxDocument.ClientID %>");
                }

               
                
               
                
                function StopPropagation(e) {
                    if (!e) {
                        e = window.event;
                    }

                    e.cancelBubble = true;
                }

              
            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
