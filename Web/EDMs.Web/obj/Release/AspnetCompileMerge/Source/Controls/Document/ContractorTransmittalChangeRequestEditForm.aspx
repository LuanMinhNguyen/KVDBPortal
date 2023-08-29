<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractorTransmittalChangeRequestEditForm.aspx.cs" Inherits="EDMs.Web.Controls.Document.ContractorTransmittalChangeRequestEditForm" %>

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
        
        #rtvDocToBeRevised {
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
            GetRadWindow().BrowserWindow.refreshChangeRequestGrid();
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
        
        <div style="width: 100%">
            <ul style="list-style-type: none">
                <div class="qlcbFormItem">
                    <div class="dnnFormMessage dnnFormInfo">
                        <div class="dnnFormItem dnnFormHelp dnnClear">
                            <p class="dnnFormRequired" style="float: left;">
                                <span style="text-decoration: underline;">Notes</span>: Fields marked with a red are invalid value.
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
                            <span style="color: #2E5689; text-align: right; ">Change Request Number
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtChangeRequestNo"  runat="server" Style="width: 400px;" CssClass="min25Percent qlcbFormRequired" ReadOnly="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
            
            <li style="width: 600px;">
                <div>
                    <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                        <span style="color: #2E5689; text-align: right; ">Change Request Title
                        </span>
                    </label>
                    <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                        <asp:TextBox ID="txtTitle"  runat="server" Style="width: 400px;" CssClass="min25Percent qlcbFormRequiGray" TextMode="MultiLine" Rows="6"/>
                    </div>
                </div>
                <div style="clear: both; font-size: 0;"></div>
            </li>
                
            
               <dl class="accordion">
                    <dt style="width: 100%;">
                        <span>Unit and Area</span>
                    </dt>
                </dl>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Area Code
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlArea" runat="server" CssClass="min25Percent qlcbFormRequiGray" Width="416px" OnSelectedIndexChanged="ddlArea_OnSelectedIndexChanged" AutoPostBack="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Unit Code
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlUnit" runat="server" CssClass="min25Percent qlcbFormRequiGray" Width="416px"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <dl class="accordion">
                    <dt style="width: 100%;">
                        <span>Classification</span>
                    </dt>
                </dl>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Project Code
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtProjectCode"  runat="server" Style="width: 400px;" CssClass="min25Percent qlcbFormRequired" ReadOnly="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Change Request Type
                            </span>
                        </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                           <asp:DropDownList ID="ddlType" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416px"
                               OnSelectedIndexChanged="RegenChangeRequestNo" AutoPostBack="True" Enabled="False"/>
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
                           <asp:DropDownList ID="ddlGroup" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416px"
                               OnSelectedIndexChanged="RegenChangeRequestNo" AutoPostBack="True" Enabled="False"/>
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
                            <telerik:radnumerictextbox type="Number" id="txtYear" runat="server" Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="70px" CssClass="min25Percent qlcbFormRequired"
                                OnTextChanged="txtYear_OnTextChanged" AutoPostBack="True" ReadOnly="True">
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
                             <asp:TextBox ID="txtSequentialNumber"  runat="server" Style="width: 400px;" CssClass="min25Percent qlcbFormRequiGray"
                                 OnTextChanged="txtSequentialNumber_OnTextChanged" AutoPostBack="True" ReadOnly="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <dl class="accordion">
                    <dt style="width: 100%;">
                        <span>Change Request Details</span>
                    </dt>
                </dl>

                  <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Reason For Change
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtReasonForChange"  runat="server" Style="width: 400px;" CssClass="min25Percent qlcbFormRequiGray" TextMode="MultiLine" Rows="6"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Existing Condition
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtExistingCondition"  runat="server" Style="width: 400px;" CssClass="min25Percent qlcbFormRequiGray" TextMode="MultiLine" Rows="6"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
            
            <li style="width: 600px;">
                <div>
                    <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                        <span style="color: #2E5689; text-align: right; ">Description Of Change
                        </span>
                    </label>
                    <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                        <asp:TextBox ID="txtDescription"  runat="server" Style="width: 400px;" CssClass="min25Percent qlcbFormRequiGray" TextMode="MultiLine" Rows="6"/>
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
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Grade Code
                            </span>
                        </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                           <asp:DropDownList ID="ddlChangeGradeCode" runat="server" CssClass="min25Percent qlcbFormRequiGray" Width="416px"/>
                        </div>
                        
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px; display: none">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Status
                            </span>
                        </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                           <asp:DropDownList ID="ddlStatus" runat="server" CssClass="min25Percent qlcbFormRequiGray" Width="416px">
                               <Items>
                                   <asp:ListItem Value="Opening" Text="Opening"/>
                                   <asp:ListItem Value="Closed" Text="Closed"/>
                               </Items>
                           </asp:DropDownList>
                        </div>
                        
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;" runat="server" id="RefDoc">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Reference Document Number
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:Panel ID="panelSearchRefDocNo" runat="server" DefaultButton="btnSearchRefDocNo">
                                <telerik:RadTextBox runat="server" ID="txtSearchRefDocNo" Width="396" CssClass="min25Percent qlcbFormRequiGray" EmptyMessage="Search Reference Document Number" style="border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3 !important;" /> 
                                <asp:ImageButton ID="btnSearchRefDocNo" runat="server" ImageUrl="../../Images/search.png" style="margin-bottom: -5px !important; margin-left: -2px !important;" OnClick="btnSearchRefDocNo_OnClick"/>
                                <telerik:RadTreeView RenderMode="Lightweight" ID="rtvRefDocNo" runat="server" CheckBoxes="True" TriStateCheckBoxes="true" CheckChildNodes="true" Width="400" Height="120" OnNodeCheck="rtvRefDocNo_OnNodeCheck" Skin="Windows7"/>
                            </asp:Panel>
                        </div>
                        
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                
                <li style="width: 600px; padding-top: 10px; padding-bottom: 3px; text-align: center">
                    <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"  Width="70px" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                </li>
                
            </ul>
        </div>
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2"  Skin="Windows7" />
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSearchDocRevised">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvDocToBeRevised" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                 <telerik:AjaxSetting AjaxControlID="rtvDocToBeRevised">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvDocToBeRevised" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="rtvRefDocNo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvRefDocNo" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSearchRefDocNo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvRefDocNo" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtChangeRequestNo"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlGroup">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtChangeRequestNo"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="txtYear">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtChangeRequestNo"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="txtSequentialNumber">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtChangeRequestNo" LoadingPanelID="RadAjaxLoadingPanel2"/>
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
                function OnClientFilesUploaded(sender, args) {
                    var name = args.get_fileName();
                    //document.getElementById("txtName").value = name;
                    $find('<%=ajaxDocument.ClientID %>').ajaxRequest();
                }

                function pageLoad() {
                    ajaxManager = $find("<%=ajaxDocument.ClientID %>");
                }

                function fileUploading(sender, args) {
                    var name = args.get_fileName();
                    document.getElementById("txtName").value = name;
                    
                    ajaxManager.ajaxRequest("CheckFileName$" + name);
                }
                
                
               
                
                function StopPropagation(e) {
                    if (!e) {
                        e = window.event;
                    }

                    e.cancelBubble = true;
                }

                function nodeClicked(sender, args) {
                    var node = args.get_node();
                    if (node.get_checked()) {
                        node.uncheck();
                    } else {
                        node.check();
                    }
                    nodeChecked(sender, args)

                }
            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
