<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="SharePermissionPage.aspx.cs" Inherits="EDMs.Web.Controls.Security.SharePermissionPage" EnableViewState="true" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        /*Hide change page size control*/
        div.RadGrid .rgPager .rgAdvPart     
        {     
        display:none;        
        }

        a.tooltip
        {
            outline: none;
            text-decoration: none;
        }

        a.tooltip strong
        {
            line-height: 30px;
        }

        a.tooltip:hover
        {
            text-decoration: none;
        }

        a.tooltip span
        {
            z-index: 10;
            display: none;
            padding: 14px 20px;
            margin-top: -30px;
            margin-left: 5px;
            width: 240px;
            line-height: 16px;
        }

        a.tooltip:hover span
        {
            display: inline;
            position: absolute;
            color: #111;
            border: 1px solid #DCA;
            background: #fffAF0;
        }

        .callout
        {
            z-index: 20;
            position: absolute;
            top: 30px;
            border: 0;
            left: -12px;
        }

        /*CSS3 extras*/
        a.tooltip span
        {
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            -moz-box-shadow: 5px 5px 8px #CCC;
            -webkit-box-shadow: 5px 5px 8px #CCC;
            box-shadow: 5px 5px 8px #CCC;
        }

        .rgMasterTable {
            table-layout: auto;
        }

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdDocumentPanel, #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_divContainerPanel
        {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_RadPageView1, #ctl00_ContentPlaceHolder2_RadPageView2,
        #ctl00_ContentPlaceHolder2_RadPageView3, #ctl00_ContentPlaceHolder2_RadPageView4,
        #ctl00_ContentPlaceHolder2_RadPageView5
        {
            height: 100% !important;
        }

        #divContainerLeft
        {
            width: 25%;
            float: left;
            margin: 5px;
            height: 99%;
            border-right: 1px dotted green;
            padding-right: 5px;
        }

        #divContainerRight
        {
            width: 100%;
            float: right;
            margin-top: 5px;
            height: 99%;
        }

        .dotted
        {
            border: 1px dotted #000;
            border-style: none none dotted;
            color: #fff;
            background-color: #fff;
        }

        .exampleWrapper
        {
            width: 100%;
            height: 100%;
            /*background: transparent url(~/Images/background.png) no-repeat top left;*/
            position: relative;
        }

        .tabStrip
        {
            position: absolute;
            top: 0px;
            left: 0px;
        }

        .multiPage
        {
            position: absolute;
            top: 30px;
            left: 0px;
            color: white;
            width: 100%;
            height: 100%;
        }

        /*Fix RadMenu and RadWindow z-index issue*/
        .radwindow
        {
            z-index: 8000 !important;
        }

        .TemplateMenu
        {
            z-index: 10;
        }
    </style>

    <telerik:RadPanelBar ID="radPbCostContract" runat="server" Width="100%"/>
    <telerik:RadPanelBar ID="radPbScope" runat="server" Width="100%"/>
    <telerik:RadPanelBar ID="radPbList" runat="server" Width="100%"/>
    <telerik:RadPanelBar ID="radPbSystem" runat="server" Width="100%"/>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Horizontal" Width="100%">
        <telerik:RadPane ID="RadPane3" runat="server" Height="30px" Scrollable="false" Scrolling="None">
            <telerik:RadToolBar ID="CustomerMenu" runat="server" Width="100%" OnButtonClick="CustomerMenu_ButtonClick">
                <Items>
                    <telerik:RadToolBarButton runat="server" Text="Save" Value="Save" ImageUrl="~/Images/save.png"/>

                    <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                    

                    <%--<telerik:RadToolBarButton runat="server">
                        <ItemTemplate>
                            <asp:Label ID="lblSearchLabel" runat="server" Text="  Quick search:  " />
                        </ItemTemplate>
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton Value="searchTextBoxButton" CssClass="searchtextbox" CommandName="searchText">
                        <ItemTemplate>
                            <telerik:RadTextBox
                                runat="server" ID="txtSearch" Width="250px"
                                EmptyMessage="Enter transmittal title, ..." />
                        </ItemTemplate>
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton ImageUrl="~/Images/search.gif" Value="search" CommandName="doSearch" />--%>
                </Items>
            </telerik:RadToolBar>

        </telerik:RadPane>
        <telerik:RadPane ID="RadPane2" runat="server" Scrollable="false" Scrolling="None">
            <telerik:RadSplitter ID="Radsplitter3" runat="server" Orientation="Horizontal">
                <telerik:RadPane ID="Radpane4" runat="server" Scrolling="None" >
                     <telerik:RadSplitter ID="Radsplitter10" runat="server" Orientation="Vertical" Width="">
                         <telerik:RadPane ID="Radpane6" runat="server" Scrolling="Both" Width="460">
                                <telerik:RadPanelBar ID="radPbGroup" runat="server" OnItemClick="radPbGroup_ItemClick" Width="360" style="min-width: 4360px" />
                        </telerik:RadPane>
                         
                         <telerik:RadPane ID="Radpane1" runat="server" Scrolling="None" >
                                <telerik:RadListBox RenderMode="Lightweight" ID="lbPIC" runat="server" CheckBoxes="true" ShowCheckAll="true" Width="600" Height="100%">
                                    <Items>
                                        <telerik:RadListBoxItem runat="server" Text="Tracking List Of Morning Call" Value="1"/>
                                        <telerik:RadListBoxItem runat="server" Text="Tracking List Of ECR" Value="2"/>
                                        <telerik:RadListBoxItem runat="server" Text="Tracking List Of MOC" Value="3"/>
                                        <telerik:RadListBoxItem runat="server" Text="Tracking List Of Breakdown Report" Value="4"/>
                                        <telerik:RadListBoxItem runat="server" Text="Tracking List Of Shutdown Report" Value="5"/>
                                        <telerik:RadListBoxItem runat="server" Text="Tracking List Of WCR" Value="6"/>
                                        <telerik:RadListBoxItem runat="server" Text="Tracking List Of Punch List" Value="7"/>
                                        <telerik:RadListBoxItem runat="server" Text="Tracking List Of Sail Listt" Value="8"/>
                                        <telerik:RadListBoxItem runat="server" Text="Tracking List Of Procedure" Value="9"/>
                                        <telerik:RadListBoxItem runat="server" Text="General Working" Value="10"/>
                                    </Items>
                                </telerik:RadListBox>

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
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radPbGroup">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lbPIC"  LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="CustomerMenu">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lbPIC" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="radPbGroup" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </span>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="User Information"
                VisibleStatusbar="false" Height="350" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            <telerik:RadWindow ID="AttachDoc" runat="server" Title="Attach documents"
                VisibleStatusbar="false" Height="555" Width="1100" MinHeight="555" MinWidth="1100" MaxHeight="555" MaxWidth="1100" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="DocList" runat="server" Title="Transmittal - Document List"
                VisibleStatusbar="false" Height="418" Width="1100" MinHeight="418" MinWidth="1100" MaxHeight="418" MaxWidth="1100" 
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
            
            function clientNodeChecked(sender, eventArgs) {
                var i;
                var node = eventArgs.get_node();
                var childNodes = node.get_allNodes();

                eventArgs.get_node().set_checkable(false);

                //To change the status of all child nodes when the parent checked/ unchecked
                if (childNodes.length > 0) {
                    for (i = 0; i < childNodes.length; i++) {
                        childNodes[i].set_checked(node.get_checked());
                    }
                }

                //To uncheck parent Nodes when any child Node has been unchecked
                if (!node.get_checked()) {
                    while (node.get_parent().set_checked != null) {
                        node.get_parent().set_checked(false);
                        node = node.get_parent();
                    }
                }

                //To check parent Nodes when all child nodes has been checked
                if (node.get_checked()) {
                    var parentNode = node.get_parent();
                    while (parentNode.set_checked != null) {
                        var allNodes = parentNode.get_nodes();
                        for (i = 0; i < allNodes.get_count() ; i++) {
                            if (!allNodes.getNode(i).get_checked()) {
                                eventArgs.get_node().set_checkable(true);
                                return;
                            }
                        }
                        parentNode.set_checked(true);
                        parentNode = parentNode.get_parent();
                    }
                }

                eventArgs.get_node().set_checkable(true);
            }

            var radDocuments;
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
        <script type="text/javascript">
            /* <![CDATA[ */
            var toolbar;
            var searchButton;
            var ajaxManager;

            function pageLoad() {
                $('iframe').load(function () { //The function below executes once the iframe has finished loading<---true dat, althoug Is coppypasta from I don't know where
                    //alert($('iframe').contents());
                });

                toolbar = $find("<%= CustomerMenu.ClientID %>");
                ajaxManager = $find("<%=ajaxCustomer.ClientID %>");

                searchButton = toolbar.findButtonByCommandName("doSearch");

                $telerik.$(".searchtextbox")
                    .bind("keypress", function (e) {
                        searchButton.set_imageUrl("~/Images/search.gif");
                        searchButton.set_value("search");
                    });
            }

            function ShowUserEditForm(id, userId) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Security/UserEditForm.aspx?Id=" + id + "&userId=" + userId, "CustomerDialog");
            }
            
            function ShowInsertForm() {
                
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Customers/CustomerEditForm.aspx", "CustomerDialog");

                //window.radopen("Controls/Customers/CustomerEditForm.aspx", "CustomerDialog");
                //return false;
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
            
            function refreshTab(arg) {
                $('.EDMsRadPageView' + arg + ' iframe').attr('src', $('.EDMsRadPageView' + arg + ' iframe').attr('src'));
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
