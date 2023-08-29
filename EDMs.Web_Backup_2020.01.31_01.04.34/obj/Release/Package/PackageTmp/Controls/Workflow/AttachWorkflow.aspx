﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttachWorkflow.aspx.cs" Inherits="EDMs.Web.Controls.Workflow.AttachWorkflow" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
	        overflow:auto;
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
            width: 283px;
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

        function CancelEdit() {
            GetRadWindow().close();
        }
        function Maximize() {
            GetRadWindow().Maximize();
        }
        function RestoreMaximize() {
            GetRadWindow().restore();
        }
            </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
        <%--<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="LoadingPanel1">  --%>

        <div style="width: 100%" runat="server" ID="divContent">
            <ul style="list-style-type: none">
                <li style="width: 500px;">
                    <div>
                        <label style="width: 75px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Workflow
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlWorkflow" runat="server" CssClass="min25Percent qlcbFormRequired" Width="405px"
                                OnSelectedIndexChanged="ddlWorkflow_OnSelectedIndexChanged" AutoPostBack="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px; display: none">
                    <div>
                        <label style="width: 75px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">First Step
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtFirstStep" runat="server" Style="width: 390px;" CssClass="min25Percent" ReadOnly="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px; display: none">
                    <div>
                        <label style="width: 75px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:CheckBox runat="server" Text="Customize Workflow" ID="cbCustomizeWorkflow" Checked="true" OnCheckedChanged="cbCustomizeWorkflow_OnCheckedChanged" AutoPostBack="True"/>
                            
                            
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
            </ul>
        </div>
        
         <div style="width: 100%; margin-top:10px; text-align:center; display:inline-flex">
             <div style="width: 50%; padding: 10px; text-align: right">
                 <telerik:RadButton ID="btnManual" runat="server" Text="Edit Workflow" OnClientClicked="OpenWFDetail" Width="120px" AutoPostBack="false"  Visible="true">
                                <Icon PrimaryIconUrl="../../Images/config.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                            </telerik:RadButton>
             </div>
             <div style="width: 50%; padding: 10px; text-align: left">
                 <telerik:RadButton ID="btnSave" runat="server" Text="Attach To Workflow" OnClick="btnSave_Click" Width="150px" SingleClick="true" SingleClickText="Submitting...">
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
             </div>
        </div><%--</telerik:RadAjaxPanel>
         <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Windows7">
        </telerik:RadAjaxLoadingPanel>--%>
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        <asp:HiddenField runat ="server" ID="AttachFrom" />
        <asp:HiddenField runat ="server" ID="ObjectId" />
        <asp:HiddenField runat ="server" ID="ObjectType" />
        <asp:HiddenField runat="server" ID="CustomizeWfFrom"/>
        <asp:HiddenField runat="server" ID="ClickManual" />
        <asp:HiddenField runat="server" ID="projId" />
        
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlWorkflow">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtFirstStep"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlRole"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlUser"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlRole">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlUser"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="cbCustomizeWorkflow">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="btnManual"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="OpenWFDetail">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ajaxDocument" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Object Information"
                VisibleStatusbar="false" Height="700" Width="650" IconUrl="~/Images/detail.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
              <telerik:RadWindow ID="WFProcessHistory" runat="server" Title="Attach Object To Workflow"
                VisibleStatusbar="False" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Maximize">
            </telerik:RadWindow>
        </Windows></telerik:RadWindowManager>
        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;

                function pageLoad() {
                    ajaxManager = $find("<%=ajaxDocument.ClientID %>");
                } </script>
                <script type="text/javascript">
                    function OpenWFDetail(sender, args) {
                        Maximize();
                    var control = document.getElementById('<%= ddlWorkflow.ClientID %>');
                    var selectedvalue = control.options[control.selectedIndex].value;
                    var objectType = document.getElementById("<%= ObjectType.ClientID %>").value;
                    var objId=document.getElementById("<%=ObjectId.ClientID %>").value;
                    var customizeWfFrom = document.getElementById("<%=CustomizeWfFrom.ClientID %>").value;
                        document.getElementById("<%= ClickManual.ClientID %>").value = "true";
                        var lbprojId = document.getElementById("<%= projId.ClientID%>").value;
                        var lbattachFrom = document.getElementById("<%= AttachFrom.ClientID%>").value;
                    var owd = $find("<%=WFProcessHistory.ClientID %>");
                    owd.Show();
                    owd.maximize();
                    owd.setUrl("../../Controls/Workflow/ObjectWorkflowDetailPage.aspx?ObjId=" + objId + "&ObjectType=" + objectType + "&wfId=" + selectedvalue + "&customizeWfFrom=" + customizeWfFrom + "&projId=" + lbprojId + "&attachFrom="+ lbattachFrom, "WFProcessHistory");
                }
                    function RefreshRadwindown() {
                        RestoreMaximize();
                    }

                    function refreshGrid(args) {
                   GetRadWindow().BrowserWindow.refreshGrid(args);
                   GetRadWindow().close();
            }
            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
