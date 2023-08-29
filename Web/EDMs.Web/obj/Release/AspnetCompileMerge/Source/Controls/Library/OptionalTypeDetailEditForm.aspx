<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OptionalTypeDetailEditForm.aspx.cs" Inherits="EDMs.Web.Controls.Library.OptionalTypeDetailEditForm" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
	        overflow:auto;
        }
        .RadComboBoxDropDown_Windows7 {
            width: 397px !important;
            height: 200px !important;
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
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="Windows7" />
          
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
        
        <div style="width: 100%" runat="server" ID="div2">
            <ul style="list-style-type: none">
                
                <li style="width: 505px;">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Name
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="divFileName">
                            <asp:TextBox ID="txtName" runat="server" Style="width: 400px;" CssClass="min25Percent qlcbFormRequired"/>
                            
                            <asp:CustomValidator runat="server" ID="fileNameValidator" ValidateEmptyText="True" CssClass="dnnFormMessage dnnFormErrorModule"
                            OnServerValidate="ServerValidationFileNameIsExist" Display="Dynamic" ControlToValidate="txtName"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 505px;">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Optional type
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlOptionalType" runat="server" Skin="Windows7"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlOptionalType_SelectedIndexChanged"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 505px; display: none">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Is leaf node
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:CheckBox ID="ckbIsRootNode" runat="server"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 505px;">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Category</span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlCategory" runat="server" Skin="Windows7"
                                CheckBoxes="true" EnableCheckAllItemsCheckBox="true"></telerik:RadComboBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 505px;">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Deparment
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlDepartment" runat="server" Skin="Windows7"
                                CheckBoxes="true" EnableCheckAllItemsCheckBox="true"></telerik:RadComboBox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 505px;">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Parent
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div3">
                            <telerik:RadComboBox ID="ddlParent" runat="server" Skin="Windows7" 
                                OnClientDropDownOpened="OnClientDropDownOpenedHandler">
                                <ItemTemplate>
                                    <div id="div1">
                                        <telerik:RadTreeView runat="server" ID="rtvOptionalTypeDetail" Skin="Windows7"                                                                                                          OnClientNodeClicking="nodeClicking"
                                            OnNodeDataBound="rtvOptionalTypeDetail_OnNodeDataBound"
                                            OnClientNodeExpanded="rtvExplore_OnNodeExpandedCollapsed"
                                            OnClientNodeCollapsed="rtvExplore_OnNodeExpandedCollapsed">
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
            </ul>
        </div>

        <div style="width: 100%" runat="server" ID="divContent">
            <ul style="list-style-type: none">
                <li style="width: 505px;" runat="server" ID="Pro6" Visible="False">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">System
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlSystem" runat="server" Skin="Windows7" 
                                OnClientDropDownOpened="ddlSystemDDHandler">
                                <ItemTemplate>
                                    <div id="div4">
                                        <telerik:RadTreeView runat="server" ID="rtvSystem" Skin="Windows7"                                                                      
                                            OnClientNodeClicking="rtvSystemNodeClicking">
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

                <li style="width: 505px;" runat="server" ID="Pro1" Visible="False">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Serial
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtSerial" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                <li style="width: 505px;" runat="server" ID="Pro2" Visible="False">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Model
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtModel" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 505px;" runat="server" ID="Pro7" Visible="False">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Duty/Capacity
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtDutyCapacity" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 505px;" runat="server" ID="Pro8" Visible="False">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Des. Temp.
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtDesTemp" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 505px;" runat="server" ID="Pro9" Visible="False">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Des. Press.
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtDesPress" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 505px;" runat="server" ID="Pro10" Visible="False">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Diff. Pres.
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtDiffPres" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 505px;" runat="server" ID="Pro11" Visible="False">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Vendor
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtVendor" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 505px;" runat="server" ID="Pro3" Visible="False">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">P&ID number
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtTechnicalSpec" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 505px;" runat="server" ID="Pro4" Visible="False">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Start date
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadDatePicker ID="txtStartDate"  runat="server" CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 505px;" runat="server" ID="Pro5" Visible="False">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">End date
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadDatePicker ID="txtEndDate"  runat="server" CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                

                <li style="width: 505px;">
                    <div>
                        <label style="width: 78px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Description
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtDescription" runat="server" Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
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
                <li style="width: 505px; padding-top: 10px; padding-bottom: 3px; text-align: center">
                    <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="78px" style="text-align: center"
>
                        <Icon PrimaryIconUrl="~/Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                    <%--<telerik:RadButton ID="btncancel" runat="server" Text="Cancel" Width="78px" style="text-align: center"
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
                
                <telerik:AjaxSetting AjaxControlID="ddlPlant">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvDocumentType"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ddlParent"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2" UpdatePanelRenderMode="Block" UpdatePanelHeight="100%"/>

                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlCategory">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvDocumentType"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ddlParent"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2" UpdatePanelRenderMode="Block" UpdatePanelHeight="100%"/>

                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlOptionalType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2" UpdatePanelRenderMode="Block" UpdatePanelHeight="100%"/>

                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="ddlToList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlAttention" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;
                var div1 = document.getElementById("div1");
                div1.onclick = StopPropagation;
                function OnClientFilesUploaded(sender, args) {
                    var name = args.get_fileName();
                    document.getElementById("txtName").value = name;
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


                function StopPropagation(e) {
                    if (!e) {
                        e = window.event;
                    }

                    e.cancelBubble = true;
                }

                function OnClientDropDownOpenedHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvDocumentType");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function nodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlParent.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();

                    comboBox.hideDropDown();

                    // Call comboBox.attachDropDown if:
                    //  1) The RadComboBox is inside an AJAX panel.
                    //  2) The RadTreeView has a server-side event handler for the NodeClick event, i.e. it initiates a postback when clicking on a Node.
                    // Otherwise the AJAX postback becomes a normal postback regardless of the outer AJAX panel.

                    //comboBox.attachDropDown();
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
                
                function rtvExplore_OnNodeExpandedCollapsed(sender, eventArgs) {
                    var allNodes = eventArgs._node.get_treeView().get_allNodes();

                    var i;
                    var selectedNodes = "";

                    for (i = 0; i < allNodes.length; i++) {
                        if (allNodes[i].get_expanded())
                            selectedNodes += allNodes[i].get_value() + "*";
                    }

                    Set_Cookie("expandedNodesOptionalTypeDetailEdit", selectedNodes, 30);
                }



                function Set_Cookie(name, value, expires, path, domain, secure) {
                    // set time, it's in milliseconds
                    var today = new Date();
                    today.setTime(today.getTime());

                    /*
                    if the expires variable is set, make the correct 
                    expires time, the current script below will set 
                    it for x number of days, to make it for hours, 
                    delete * 24, for minutes, delete * 60 * 24
                    */
                    if (expires) {
                        expires = expires * 1000 * 60 * 60 * 24;
                    }
                    var expires_date = new Date(today.getTime() + (expires));

                    document.cookie = name + "=" + escape(value) +
                        ((expires) ? ";expires=" + expires_date.toGMTString() : "") +
                        ((path) ? ";path=" + path : "") +
                        ((domain) ? ";domain=" + domain : "") +
                        ((secure) ? ";secure" : "");
                }

            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
