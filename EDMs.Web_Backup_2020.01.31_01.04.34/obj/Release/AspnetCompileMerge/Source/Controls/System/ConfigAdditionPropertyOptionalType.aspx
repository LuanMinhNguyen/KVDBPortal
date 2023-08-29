<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfigAdditionPropertyOptionalType.aspx.cs" Inherits="EDMs.Web.Controls.Library.ConfigAdditionPropertyOptionalType" EnableViewState="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .multipleRowsColumns .rcbItem,
        .multipleRowsColumns .rcbHovered
        {
            float:left;
            margin:0 1px;
            min-height:13px;
            overflow:hidden;
            padding:2px 19px 2px 6px;
            width:125px;
        }
       html, body, form {
	        overflow:auto;
        }
        /*.RadComboBoxDropDown_Windows7 {
            width: 397px !important;
            height: 200px !important;
        }*/
        /*.RadComboBoxDropDown .rcbScroll {
            height: 199px !important;
        }*/
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
            width: 150px;
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
           .accordion dt a
            {
                color: #085B8F;
                border-bottom: 2px solid #46A3D3;
                font-size: 1.5em;
                font-weight: bold;
                letter-spacing: -0.03em;
                line-height: 1.2;
                margin: 0.5em auto 0.6em;
                padding: 0;
                text-align: left;
                text-decoration: none;
                display: block;
            }

            .accordion dt span
            {
                color: #085B8F;
                border-bottom: 2px solid #46A3D3;
                font-size: 1.5em;
                font-weight: bold;
                letter-spacing: -0.03em;
                line-height: 1.2;
                margin: 0.5em auto 0.6em;
                padding: 0;
                text-align: left;
                text-decoration: none;
                display: block;
            }
    </style>
    <telerik:RadPanelBar ID="radPbSearch" runat="server" Width="100%" Visible="False">
        <Items>
            <telerik:RadPanelItem Text="SEARCH DOCUMENT" runat="server" Expanded="True" Width="100%">
                <Items>
                    <telerik:RadPanelItem runat="server" Text="Search Documents" ImageUrl="~/Images/search.gif" NavigateUrl="Search.aspx" />
                </Items>
            </telerik:RadPanelItem>
        </Items>
    </telerik:RadPanelBar>
    <telerik:RadPanelBar ID="radPbCategories" runat="server" Visible="False" OnClientItemClicking="radPbCategories_OnClientItemClicking" Width="100%"></telerik:RadPanelBar>
        
     <telerik:RadPanelBar ID="RadPanelBar1" runat="server" Width="100%" Visible="False">
        <Items>
            <telerik:RadPanelItem Text="TRANSMITTAL" runat="server" Expanded="True" Width="100%">
                <Items>
                    <telerik:RadPanelItem runat="server" Text="Transmittal" ImageUrl="~/Images/Transmittal.png" NavigateUrl="~/Controls/Document/TransmittalList.aspx"/>
                </Items>
            </telerik:RadPanelItem>
        </Items>
    </telerik:RadPanelBar>
    
    <telerik:RadPanelBar ID="radPbList" runat="server" Width="100%"/>
    <telerik:RadPanelBar ID="radPbSystem" runat="server" Width="100%"/>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Horizontal" Width="100%">
        <telerik:RadPane ID="RadPane3" runat="server" Height="30px" Scrollable="false" Scrolling="None">
            <telerik:RadToolBar ID="rtbCommand" runat="server" Width="100%" OnClientButtonClicked="OnClientButtonClicking">
                <Items>
                    <telerik:RadToolBarButton runat="server" Text="Save" Value="Save" ImageUrl="~/Images/save.png"/>
                </Items>
            </telerik:RadToolBar>

        </telerik:RadPane>
        <telerik:RadPane ID="RadPane2" runat="server" Scrollable="false" Scrolling="None">
            <telerik:RadSplitter ID="Radsplitter3" runat="server" Orientation="Horizontal">
                <telerik:RadPane ID="Radpane4" runat="server" Scrolling="None" >
                    
                     <telerik:RadSplitter ID="Radsplitter10" runat="server" Orientation="Vertical">
                         <telerik:RadPane ID="Radpane6" runat="server" Scrolling="None">
                             <dl class="accordion">
    <dt style="width: 100%;">
        <span>Config addition properties for Optional type</span>
    </dt>
</dl>
                               <div style="width: 100%" runat="server" ID="divContent">
                                    <ul style="list-style-type: none">
                                        <li style="width: 600px;">
                                            <div>
                                                <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                    <span style="color: #2E5689; text-align: right; ">Optional type</span>
                                                </label>
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                    <telerik:RadComboBox ID="ddlOptionalType" runat="server" Skin="Windows7"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                                                         style="width: 397px !important;"/>
                                                </div>
                                            </div>
                                            <div style="clear: both; font-size: 0;"></div>
                                        </li>
                
                                        <li style="width: 600px;">
                                            <div>
                                                <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                    <span style="color: #2E5689; text-align: right; ">Properties
                                                    </span>
                                                </label>
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                    <telerik:RadComboBox ID="ddlProperties" runat="server" Skin="Windows7"
                                                        DropDownCssClass="multipleRowsColumns" DropDownWidth="460px"
                                                        CheckBoxes="true" EnableCheckAllItemsCheckBox="true"/>
                                                </div>
                                            </div>
                                            <div style="clear: both; font-size: 0;"></div>
                                        </li>
                                    </ul>
                                </div>
                        </telerik:RadPane>

                     </telerik:RadSplitter>       

                </telerik:RadPane>
                
                <%--<telerik:RadSplitBar ID="Radsplitbar4" runat="server" CollapseMode="Both">
                </telerik:RadSplitBar>
                

                <telerik:RadPane ID="Radpane5" runat="server" Scrolling="None" style="height: 100%" >
                    <div runat="server" id="divContainer" style="width: 100%; height: 100%;">
                        <input type="hidden" id="lblDocID" runat="server" />
                        <div id="divContainerRight">

                            <div class="exampleWrapper">
                                <%--<div id="divLoading" style="display:none;top:30px;left:0px; position:relative;">Loading</div>
                                <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" 
                                    CssClass="multiPage" >
                                    <telerik:RadPageView ID="RadPageView2"  runat="server" CssClass="EDMsRadPageView2"/>
                                    
                                    <telerik:RadPageView ID="RadPageView3"  runat="server" CssClass="EDMsRadPageView3"/>
                                </telerik:RadMultiPage>
                                <telerik:RadTabStrip ID="RadTabStrip1" SelectedIndex="0" Width="100%"
                                    CssClass="tabStrip" runat="server" MultiPageID="RadMultiPage1"
                                    OnTabClick="RadTabStrip1_TabClick">
                                    <Tabs>
                                        <telerik:RadTab Text="Revision history">
                                        </telerik:RadTab>
                                        <telerik:RadTab Text="Version history">
                                        </telerik:RadTab>
                                    </Tabs>
                                </telerik:RadTabStrip>
                            </div>
                        </div>
                    </div>
                </telerik:RadPane>--%>
            </telerik:RadSplitter>
        </telerik:RadPane>
    </telerik:RadSplitter>

    <span style="display: none">
        

        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContainer" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlOptionalType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlProperties"  LoadingPanelID="RadAjaxLoadingPanel2"  ></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlDepartment">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlProperties"  LoadingPanelID="RadAjaxLoadingPanel2"  ></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="rtbCommand">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContainer" LoadingPanelID="RadAjaxLoadingPanel2" UpdatePanelRenderMode="Block" UpdatePanelHeight="100%"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </span>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Originator Information"
                VisibleStatusbar="false" Height="350" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
    <asp:HiddenField runat="server" ID="FolderContextMenuAction"/>
    <asp:HiddenField runat="server" ID="lblFolderId"/>
    <asp:HiddenField runat="server" ID="lblDocId"/>
    <asp:HiddenField runat="server" ID="lblCategoryId"/>
    <input type="hidden" id="radGridClickedRowIndex" name="radGridClickedRowIndex"/>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="../../Scripts/jquery-1.7.1.js"></script>
        <script type="text/javascript">
            /* <![CDATA[ */
            var toolbar;
            var searchButton;
            var ajaxManager;

            function pageLoad() {
                $('iframe').load(function () { //The function below executes once the iframe has finished loading<---true dat, althoug Is coppypasta from I don't know where
                    //alert($('iframe').contents());
                });

                ajaxManager = $find("<%=ajaxCustomer.ClientID %>");

                $telerik.$(".searchtextbox")
                    .bind("keypress", function (e) {
                        searchButton.set_imageUrl("~/Images/search.gif");
                        searchButton.set_value("search");
                    });
            }

            function ShowEditForm(id) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("OriginatorEditForm.aspx?disId=" + id, "CustomerDialog");
            }
            
            function refreshGrid(arg) {
                //alert(arg);
                if (!arg) {
                    ajaxManager.ajaxRequest("Rebind");
                }
                else {
                    ajaxManager.ajaxRequest("RebindAndNavigate");
                }
            }
            
            function OnClientButtonClicking(sender, args) {
                var button = args.get_item();
                var strText = button.get_text();
                
                if (strText.toLowerCase() == "save") {
                    
                        ajaxManager.ajaxRequest("Save");
                }
            }

            function refreshTab(arg) {
                $('.EDMsRadPageView' + arg + ' iframe').attr('src', $('.EDMsRadPageView' + arg + ' iframe').attr('src'));
            }

            
            function radPbCategories_OnClientItemClicking(sender, args)
            {
                var item = args.get_item();
                var categoryId = item.get_value();
                document.getElementById("<%= lblCategoryId.ClientID %>").value = categoryId;
                
            }
            
            function performSearch(searchTextBox) {
                if (searchTextBox.get_value()) {
                    searchButton.set_imageUrl("~/Images/clear.gif");
                    searchButton.set_value("clear");
                }

                ajaxManager.ajaxRequest(searchTextBox.get_value());
            }
            function onTabSelecting(sender, args) {
                if (args.get_tab().get_pageViewID()) {
                    args.get_tab().set_postBack(false);
                }
            }
            
        /* ]]> */
        </script>
    </telerik:RadCodeBlock>
</asp:Content>

<%--Tan.Le Remove here--%>
<%--<uc1:List runat="server" ID="CustomerList"/>--%>
<%-- <div id="EDMsCustomers" runat="server" />--%>
