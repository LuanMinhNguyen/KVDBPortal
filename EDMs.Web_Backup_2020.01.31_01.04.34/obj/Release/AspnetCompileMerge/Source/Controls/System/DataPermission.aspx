<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DataPermission.aspx.cs" Inherits="EDMs.Web.DataPermission" EnableViewState="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" Width="400px">
    
    <!--[if gte IE 8]>
        <style type="text/css">
            #ctl00_ContentPlaceHolder2_grdDocument_ctl00_Header{table-layout:auto !important;}
            #ctl00_ContentPlaceHolder2_grdDocument_ctl00{table-layout:auto !important;}
        </style>
    <![endif]-->

    <style type="text/css">
        /*Custom CSS of Grid documents for FF browser*/
        /*#ctl00_ContentPlaceHolder2_grdDocument_ctl00_Header{table-layout:auto !important;}
        #ctl00_ContentPlaceHolder2_grdDocument_ctl00{table-layout:auto !important;}*/
        /*End*/
        @-moz-document url-prefix() {
            #ctl00_ContentPlaceHolder2_grdDocument_ctl00_Header{table-layout:auto !important;}
            #ctl00_ContentPlaceHolder2_grdDocument_ctl00{table-layout:auto !important;}
        }

        
        #RAD_SPLITTER_PANE_CONTENT_ctl00_leftPane {
            width: 350px !important;
        }
        #RAD_SPLITTER_PANE_CONTENT_ctl00_topLeftPane {
            width: 350px !important;
        }

        #ctl00_ContentPlaceHolder2_Radpane6 {
            overflow: auto !important;
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
    
    <telerik:RadPanelBar ID="radPbGroup" runat="server" OnItemClick="radPbGroup_ItemClick" Width="334" style="min-width: 334px" />

    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Horizontal" Width="100%">
        <telerik:RadPane ID="RadPane3" runat="server" Height="30px" Scrollable="false" Scrolling="None">
             <div style="padding-top: 5px">
            <telerik:RadButton ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" >
                <Icon PrimaryIconUrl="~/Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="20"></Icon>
            </telerik:RadButton>
        </div>
        </telerik:RadPane>

        <telerik:RadPane ID="RadPane2" runat="server" Scrollable="false" Scrolling="None">
            <telerik:RadSplitter ID="Radsplitter3" runat="server" Orientation="Horizontal">
                <telerik:RadPane ID="Radpane4" runat="server" Scrolling="None" >
                    
                     <telerik:RadSplitter ID="Radsplitter10" runat="server" Orientation="Vertical">
                        <telerik:RadPane ID="Radpane1" runat="server" Scrolling="Both" Width="230" MinWidth="230">
                            
                            <telerik:RadSlidingZone ID="SlidingZone1" runat="server" Width="20px" DockedPaneId="RadSlidingPane1" >
                                <telerik:RadSlidingPane ID="RadSlidingPane1" Title="Document categories" runat="server" Width="230" Height="100%">
                                    <telerik:RadTreeView ID="radTreeCategories" runat="server" Width="100%" 
                                        OnNodeClick="radTreeCategories_NodeClick" OnNodeCheck="radTreeCategories_NodeCheck"
                                        OnClientNodeClicked="CategoryTreeNodeClicked"
                                        CheckBoxes="True">
                                        <DataBindings>
                                            <telerik:RadTreeNodeBinding Expanded="true" CheckedField="IsPermitted"></telerik:RadTreeNodeBinding>
                                        </DataBindings>
                                    </telerik:RadTreeView>
                                </telerik:RadSlidingPane>
                            </telerik:RadSlidingZone>
                        </telerik:RadPane>

                         <telerik:RadSplitBar ID="Radsplitbar1" runat="server">
                        </telerik:RadSplitBar>
                         
                         <telerik:RadPane ID="Radpane6" runat="server" Scrolling="Both" Width="845px" >
                                <telerik:RadTreeView ID="radTreeFolder" runat="server" Width="100%"  TriStateCheckBoxes="True"
                                        CheckBoxes="True"
                                        OnNodeCheck="radTreeFolder_NodeCheck">
                                        <DataBindings>
                                            <telerik:RadTreeNodeBinding Expanded="false" CheckedField="IsPermitted"></telerik:RadTreeNodeBinding>
                                        </DataBindings>
                                    </telerik:RadTreeView>
                        </telerik:RadPane>

                     </telerik:RadSplitter>       

                </telerik:RadPane>
            </telerik:RadSplitter>
        </telerik:RadPane>
    </telerik:RadSplitter>
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2"  />
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radTreeCategories" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="radTreeFolder" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="radPbGroup" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="radPbGroup">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radTreeCategories" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="radTreeFolder" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="radTreeCategories">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radTreeFolder" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    

    <asp:HiddenField runat="server" ID="FolderContextMenuAction"/>
    <asp:HiddenField runat="server" ID="lblFolderId"/>
    <asp:HiddenField runat="server" ID="lblDocId"/>
    <asp:HiddenField runat="server" ID="lblCategoryId"/>
    <input type="hidden" id="radGridClickedRowIndex" name="radGridClickedRowIndex"/>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="../../Scripts/jquery-1.7.1.js"></script>
        <script type="text/javascript">
            function onColumnHidden(sender) {
                
                var masterTableView = sender.get_masterTableView().get_element();
                masterTableView.style.tableLayout = "auto";
                //window.setTimeout(function () { masterTableView.style.tableLayout = "auto"; }, 0);
            }

            function RowClick(sender, eventArgs) {
                var Id = eventArgs.getDataKeyValue("ID");
                document.getElementById("<%= lblDocId.ClientID %>").value = Id;
            }

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

            function rtvExplore_OnNodeExpandedCollapsed(sender, eventArgs) {
                var allNodes = eventArgs._node.get_treeView().get_allNodes();

                var i;
                var selectedNodes = "";

                for (i = 0; i < allNodes.length; i++) {
                    if (allNodes[i].get_expanded())
                        selectedNodes += allNodes[i].get_value() + "*";
                }

                Set_Cookie("expandedNodes", selectedNodes, 30);
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
            
            function onNodeClicking(sender, args) {
                var folderValue = args.get_node().get_value();
                
                document.getElementById("<%= lblFolderId.ClientID %>").value = folderValue;
            }

            function CategoryTreeNodeClicked(sender, args) {
                args.get_node().set_checked(true);
                ajaxManager.ajaxRequest("CreateCategorySession");
            }

            function OnClientViewSettingClicking(sender, args) {
                var button = args.get_item();
                var strText = button.get_text();
                var strValue = button.get_value();
                
                if (strText == "Search") {                   

                }
                
                var slidingZone;
                var slidingPane;
                if (strText == "List all documents") {
                    slidingZone = $find("<%= SlidingZone1.ClientID %>");
                    //slidingPane = slidingZone.getPaneById("<%= RadSlidingPane1.ClientID %>");
                    slidingPane = slidingZone.getPanes();
                    //alert(slidingPane[0].get_docked());
                    //slidingPane[0].set_docked(false);
                }
                
                if (strText == "Tree view") {
                    
                    slidingZone = $find("<%= SlidingZone1.ClientID %>");
                    //slidingPane = slidingZone.getPaneById("<%= RadSlidingPane1.ClientID %>"); 
                    slidingPane = slidingZone.getPanes();
                    //slidingPane[0].set_enableDock = true;
                }
                return false;
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
