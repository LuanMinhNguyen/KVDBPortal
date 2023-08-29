<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentInfoEditForm.aspx.cs" Inherits="EDMs.Web.Controls.Document.DocumentInfoEditForm" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
	        overflow:auto;
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
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
        <div style="display: none">
            <telerik:RadTreeView runat="server" ID="rtvOptionalTypeDetailTemp" Skin="Windows7">
                <DataBindings>
                    <telerik:RadTreeNodeBinding Expanded="false"></telerik:RadTreeNodeBinding>
                </DataBindings>
            </telerik:RadTreeView>
        </div>
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
        <div style="width: 100%">
            <ul style="list-style-type: none">
                
                <li style="width: 500px;" runat="server" ID="RevisionDoc" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Current Rev editing
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlRevFullDoc" runat="server" Skin="Windows7"
                                style="width: 294px !important; border-left-color: #FF0000 !important; "
                                AutoPostBack="True" OnSelectedIndexChanged="ddlRevFullDoc_SelectedIndexChanged"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 500px;" Runat="server" ID="UploadControl">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Category
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlCategory" runat="server" Skin="Windows7"
                                style="width: 294px !important; border-left-color: #FF0000 !important; "
                                AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Permission
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:RadioButton ID="rbtnPrivate" runat="server" GroupName="DocViewPermission" Text="Private"/>
                            <asp:RadioButton ID="rbtnInGroup" runat="server" GroupName="DocViewPermission" Text="In group" Checked="True"/>
                            <asp:RadioButton ID="rbtnPublish" runat="server" GroupName="DocViewPermission" Text="Publish"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
            </ul>
        </div>

        <div style="width: 100%" runat="server" ID="divContent">
            <ul style="list-style-type: none">
                
                
                <li style="width: 500px;" runat="server" ID ="Index1" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Name
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="divFileName">
                            <asp:TextBox ID="txtName" runat="server" Style="width: 300px;" 
                                CssClass="min25Percent qlcbFormRequired"/>
                            <asp:ImageButton runat="server" ID="btnGetInfo" style="padding-left: 3px" 
                                OnClick="btnGetInfo_Click"
                                ImageUrl="~/Images/info.png" Width="22px" AlternateText="Check revision info"/>
                            <asp:CustomValidator runat="server" ID="fileNameValidator" ValidateEmptyText="True" CssClass="dnnFormMessage dnnFormErrorModuleEdit"
                            OnServerValidate="ServerValidationFileNameIsExist" Display="Dynamic" ControlToValidate="txtName"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 500px;" runat="server" ID ="Index2" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Description
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtDescription" runat="server" Style="width: 300px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index3" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Revision
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlRevision" runat="server" CssClass="min25Percent" Width="150px"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index4" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Vendor Name
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtVendorName" runat="server" Style="width: 300px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index5" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Drawing Number
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtDrawingNumber" runat="server" Style="width: 300px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index6" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Year
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="min25Percent" Width="150px"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index7" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Plant
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlPlant" runat="server" Skin="Windows7"
                                style="width: 294px !important;" MaxHeight="250"
                                OnClientDropDownOpened="ddlPlantDDHandler">
                                <ItemTemplate>
                                    <div id="div2">
                                        <telerik:RadTreeView runat="server" ID="rtvPlant" Skin="Windows7" OnClientNodeClicking="rtvPlantNodeClicking">
                                            <DataBindings>
                                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                                        </DataBindings>
                                        </telerik:RadTreeView>
                                    </div>
                                </ItemTemplate>
                                <Items>
                                    <telerik:RadComboBoxItem Text="" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index8" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">System
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlSystem" runat="server" Skin="Windows7"
                                style="width: 294px !important;" MaxHeight="250"
                                OnClientDropDownOpened="ddlSystemDDHandler">
                                <ItemTemplate>
                                    <div id="div1">
                                        <telerik:RadTreeView runat="server" ID="rtvSystem" Skin="Windows7" OnClientNodeClicking="rtvSystemNodeClicking">
                                            <DataBindings>
                                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                                        </DataBindings>
                                        </telerik:RadTreeView>
                                    </div>
                                </ItemTemplate>
                                <Items>
                                    <telerik:RadComboBoxItem Text="" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index9" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Discipline
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlDiscipline" runat="server" Skin="Windows7"
                                style="width: 294px !important;" MaxHeight="250"
                                OnClientDropDownOpened="ddlDisciplineDDHandler">
                                <ItemTemplate>
                                    <div id="div3">
                                        <telerik:RadTreeView runat="server" ID="rtvDiscipline" Skin="Windows7" OnClientNodeClicking="rtvDisciplineNodeClicking">
                                            <DataBindings>
                                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                                        </DataBindings>
                                        </telerik:RadTreeView>
                                    </div>
                                </ItemTemplate>
                                <Items>
                                    <telerik:RadComboBoxItem Text="" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index10" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Document Type
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlDocumentType" runat="server" Skin="Windows7"
                                style="width: 294px !important;" MaxHeight="250"
                                OnClientDropDownOpened="ddlDocumentTypeDDHandler">
                                <ItemTemplate>
                                    <div id="div4">
                                        <telerik:RadTreeView runat="server" ID="rtvDocumentType" Skin="Windows7" OnClientNodeClicking="rtvDocumentTypeNodeClicking">
                                            <DataBindings>
                                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                                        </DataBindings>
                                        </telerik:RadTreeView>
                                    </div>
                                </ItemTemplate>
                                <Items>
                                    <telerik:RadComboBoxItem Text="" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                 <li style="width: 500px;" runat="server" ID ="Index11" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Tags
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlTagType" runat="server" Skin="Windows7"
                                style="width: 294px !important;" MaxHeight="250"
                                OnClientDropDownOpened="ddlTagTypeDDHandler" after>
                                <ItemTemplate>
                                    <div id="divTagType">
                                        <telerik:RadTreeView runat="server" ID="rtvTagType" Skin="Windows7" 
                                            OnClientNodeChecked="nodeChecked" OnClientNodeClicked="nodeClicked"
                                            CheckBoxes="True">
                                            <DataBindings>
                                            <telerik:RadTreeNodeBinding Expanded="false"></telerik:RadTreeNodeBinding>
                                        </DataBindings>
                                        </telerik:RadTreeView>
                                    </div>
                                </ItemTemplate>
                                <Items>
                                    <telerik:RadComboBoxItem Text="" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index12" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Project
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlProject" runat="server" Skin="Windows7"
                                style="width: 294px !important;" MaxHeight="250"
                                OnClientDropDownOpened="ddlProjectDDHandler">
                                <ItemTemplate>
                                    <div id="div6">
                                        <telerik:RadTreeView runat="server" ID="rtvProject" Skin="Windows7" 
                                            OnNodeClick="rtvProject_Nodeclick"
                                            OnClientNodeClicking="rtvProjectNodeClicking">
                                            <DataBindings>
                                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                                        </DataBindings>
                                        </telerik:RadTreeView>
                                    </div>
                                </ItemTemplate>
                                <Items>
                                    <telerik:RadComboBoxItem Text="" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index13" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Block
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlBlock" runat="server" Skin="Windows7"
                                style="width: 294px !important;" MaxHeight="250"
                                OnClientDropDownOpened="ddlBlockDDHandler">
                                <ItemTemplate>
                                    <div id="div7">
                                        <telerik:RadTreeView runat="server" ID="rtvBlock" Skin="Windows7" 
                                            OnNodeClick="rtvBlock_NodeClick"
                                            OnClientNodeClicking="rtvBlockNodeClicking">
                                            <DataBindings>
                                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                                        </DataBindings>
                                        </telerik:RadTreeView>
                                    </div>
                                </ItemTemplate>
                                <Items>
                                    <telerik:RadComboBoxItem Text="" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index14" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Field
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlField" runat="server" Skin="Windows7"
                                style="width: 294px !important;" MaxHeight="250"
                                OnClientDropDownOpened="ddlFieldDDHandler">
                                <ItemTemplate>
                                    <div id="div8">
                                        <telerik:RadTreeView runat="server" ID="rtvField" Skin="Windows7" 
                                            OnNodeClick="rtvField_NodeClick"
                                            OnClientNodeClicking="rtvFieldNodeClicking">
                                            <DataBindings>
                                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                                        </DataBindings>
                                        </telerik:RadTreeView>
                                    </div>
                                </ItemTemplate>
                                <Items>
                                    <telerik:RadComboBoxItem Text="" />
                                </Items>
                            </telerik:RadComboBox>

                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index15" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Plarform
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlPlatform" runat="server" Skin="Windows7"
                                style="width: 294px !important;" MaxHeight="250"
                                OnClientDropDownOpened="ddlPlatformDDHandler">
                                <ItemTemplate>
                                    <div id="div9">
                                        <telerik:RadTreeView runat="server" ID="rtvPlatform" Skin="Windows7" 
                                            OnNodeClick="rtvPlatform_NodeClick"
                                            OnClientNodeClicking="rtvPlatformNodeClicking">
                                            <DataBindings>
                                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                                        </DataBindings>
                                        </telerik:RadTreeView>
                                    </div>
                                </ItemTemplate>
                                <Items>
                                    <telerik:RadComboBoxItem Text="" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index16" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Well
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlWell" runat="server" Skin="Windows7"
                                style="width: 294px !important;" MaxHeight="250"
                                OnClientDropDownOpened="ddlWellDDHandler">
                                <ItemTemplate>
                                    <div id="div10">
                                        <telerik:RadTreeView runat="server" ID="rtvWell" Skin="Windows7" OnClientNodeClicking="rtvWellNodeClicking">
                                            <DataBindings>
                                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                                        </DataBindings>
                                        </telerik:RadTreeView>
                                    </div>
                                </ItemTemplate>
                                <Items>
                                    <telerik:RadComboBoxItem Text="" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
               <li style="width: 500px;" runat="server" ID ="Index17" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 3px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Start Date
                            </span>
                        </label>
                        <div style="float: left;  padding-top: 5px;"  class="qlcbFormItem">
                            <telerik:RadDatePicker ID="txtStartDate"  runat="server" CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index18" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 3px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">End Date
                            </span>
                        </label>
                        <div style="float: left;  padding-top: 5px;"  class="qlcbFormItem">
                            <telerik:RadDatePicker ID="txtEndDate"  runat="server" CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;"  runat="server" ID ="Index19" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Number Of Work
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:radnumerictextbox type="Number" id="txtNumberOfWork" runat="server" Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="50px" CssClass="min25Percent">
                                <NumberFormat DecimalDigits="0"></NumberFormat>
                            </telerik:radnumerictextbox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index20" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Tag No
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtTagNo" runat="server" Style="width: 300px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index21" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Tag Des
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtTagDes" runat="server" Style="width: 300px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index22" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Manufacturers
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtManufacturers" runat="server" Style="width: 300px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index23" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Serial No
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtSerialNo" runat="server" Style="width: 300px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index24" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Model No
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtModelNo" runat="server" Style="width: 300px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index25" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Asset No
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtAssetNo" runat="server" Style="width: 300px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index26" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Table Of Contents
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtTableOfContents" runat="server" Style="width: 300px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index27" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Date
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadDatePicker ID="txtPublishDate"  runat="server" CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index28" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">From
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlFrom" runat="server" CssClass="min25Percent" Width="250px"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index29" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">To
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlTo" runat="server" CssClass="min25Percent" Width="250px"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index30" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Signer
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtSigner" runat="server" Style="width: 300px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index31" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Other
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtOther" runat="server" Style="width: 300px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;" runat="server" ID ="Index32" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">RIG
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlRIG" runat="server" Skin="Windows7"
                                style="width: 294px !important;" MaxHeight="250"
                                OnClientDropDownOpened="ddlRIGDDHandler">
                                <ItemTemplate>
                                    <div id="div5">
                                        <telerik:RadTreeView runat="server" ID="rtvRIG" Skin="Windows7" 
                                            OnClientNodeClicking="rtvRIGNodeClicking">
                                            <DataBindings>
                                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                                        </DataBindings>
                                        </telerik:RadTreeView>
                                    </div>
                                </ItemTemplate>
                                <Items>
                                    <telerik:RadComboBoxItem Text="" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 500px;" runat="server" ID ="Index33" Visible="False">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Kind of repair
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtKindOfRepair" runat="server" Style="width: 300px;" CssClass="min25Percent"/>
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
        
         <div style="width: 100%">
            <ul style="list-style-type: none">
                <li style="width: 500px; padding-top: 10px; padding-bottom: 3px; text-align: center">
                    <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="70px" style="text-align: center"
>
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                    <%--<telerik:RadButton ID="btncancel" runat="server" Text="Cancel" Width="70px" style="text-align: center"
                        OnClick="btncancel_Click">
                        <Icon PrimaryIconUrl="../../Images/Cancel.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>--%>

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
                
                <telerik:AjaxSetting AjaxControlID="ddlRevFullDoc">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2" UpdatePanelRenderMode="Block" UpdatePanelHeight="100%"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="ddlCategory">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2" UpdatePanelRenderMode="Block" UpdatePanelHeight="100%"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnGetInfo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2" UpdatePanelRenderMode="Block" UpdatePanelHeight="100%"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="rtvProject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2" UpdatePanelRenderMode="Block" UpdatePanelHeight="100%"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rtvBlock">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2" UpdatePanelRenderMode="Block" UpdatePanelHeight="100%"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rtvField">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2" UpdatePanelRenderMode="Block" UpdatePanelHeight="100%"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rtvPlatform">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2" UpdatePanelRenderMode="Block" UpdatePanelHeight="100%"/>
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

                function ValidateForm() {
                    var name = document.getElementById("<%= txtName.ClientID%>");
                    if (name.value.trim() == "") {
                        alert("Please enter file name.");
                        name.focus();
                        return false;
                    }
                }
                
                function fileUploading(sender, args) {
                    var name = args.get_fileName();
                    document.getElementById("txtName").value = name;
                    
                    ajaxManager.ajaxRequest("CheckFileName$" + name);
                }
                
                function ddlSystemDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvSystem");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvSystemNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlSystem.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }
                
                function ddlPlantDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvPlant");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvPlantNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlPlant.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }
                
                function ddlDisciplineDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvDiscipline");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvDisciplineNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlDiscipline.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }
                
                function ddlDocumentTypeDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvDocumentType");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvDocumentTypeNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlDocumentType.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }
                
                function ddlTagTypeDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvTagType");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvTagTypeNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlTagType.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
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

                function nodeChecked(sender, args) {
                    var comboBox = $find("<%= ddlTagType.ClientID %>");

            //check if 'Select All' node has been checked/unchecked
            var tempNode = args.get_node();
            if (tempNode.get_text().toString() == "(Select All)") {
                // check or uncheck all the nodes
            } else {
                var nodes = new Array();
                nodes = sender.get_checkedNodes();
                var vals = "";
                var i = 0;

                for (i = 0; i < nodes.length; i++) {
                    var n = nodes[i];
                    var nodeText = n.get_text().toString();
                    if (nodeText != "(Select All)") {
                        vals = vals + n.get_text().toString() + ",";
                    }
                }

                //prevent  combo from closing
                supressDropDownClosing = true;
                comboBox.set_text(vals);
            }
        }





                function ddlProjectDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvProject");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvProjectNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlProject.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }
                
                function ddlBlockDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvBlock");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvBlockNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlBlock.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }
                
                function ddlFieldDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvField");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvFieldNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlField.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }
                
                function ddlPlatformDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvPlatform");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvPlatformNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlPlatform.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }
                
                function ddlWellDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvWell");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvWellNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlWell.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }
                
                function ddlRIGDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvRIG");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvRIGNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlRIG.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }

            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
