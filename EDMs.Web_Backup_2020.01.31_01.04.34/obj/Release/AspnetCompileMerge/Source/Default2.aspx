<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default2.aspx.cs" Inherits="EDMs.Web.Default2" EnableViewState="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
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

        
        .RadGrid .rgRow td, .RadGrid .rgAltRow td, .RadGrid .rgEditRow td, .RadGrid .rgFooter td, .RadGrid .rgFilterRow td, .RadGrid .rgHeader, .RadGrid .rgResizeCol, .RadGrid .rgGroupHeader td {
            padding-left: 1px !important;
            padding-right: 1px !important;
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

        
        #ctl00_ContentPlaceHolder2_radTreeFolder {
            overflow: visible !important;
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
            /*background: transparent url(images/background.png) no-repeat top left;*/
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
    <telerik:RadPanelBar ID="radPbSearch" runat="server" Width="100%">
        <Items>
            <telerik:RadPanelItem Text="SEARCH DOCUMENT" runat="server" Expanded="True" Width="100%">
                <Items>
                    <telerik:RadPanelItem runat="server" Text="Search Documents" ImageUrl="Images/search.gif" NavigateUrl="Search.aspx" />
                </Items>
            </telerik:RadPanelItem>
        </Items>
    </telerik:RadPanelBar>
    <telerik:RadPanelBar ID="radPbCategories" runat="server" Visible="False" OnItemClick="radPbCategories_ItemClick" OnClientItemClicking="radPbCategories_OnClientItemClicking" Width="100%">
        <%--<Items>
            <telerik:RadPanelItem Text="BIENDONG POC DOCUMENTS" runat="server" Expanded="True" >
                <Items>
                    <telerik:RadPanelItem runat="server" Text="Admin & HR" ImageUrl="Images/Document.png" Value="2" />
                    <telerik:RadPanelItem runat="server" Text="Drilling & Completions" Value="3" ImageUrl="Images/Document.png" />
                    <telerik:RadPanelItem runat="server" Text="Finance & Accounting" Value="4" ImageUrl="Images/Document.png" />
                    <telerik:RadPanelItem runat="server" Text="HSE" Value="5" ImageUrl="Images/Document.png" />
                    <telerik:RadPanelItem runat="server" Text="Operations" Value="6" ImageUrl="Images/Document.png"/>
                    <telerik:RadPanelItem runat="server" Text="Project Procedures" Value="7" ImageUrl="Images/Document.png" />
                    <telerik:RadPanelItem runat="server" Text="Sub Surface" Value="8" ImageUrl="Images/Document.png" />
                </Items>
            </telerik:RadPanelItem>
        </Items>--%>
    </telerik:RadPanelBar>
    <telerik:RadPanelBar ID="RadPanelBar1" runat="server" Width="100%">
        <Items>
            <telerik:RadPanelItem Text="TRANSMITTAL" runat="server" Expanded="True" Width="100%">
                <Items>
                    <telerik:RadPanelItem runat="server" Text="Transmittal" ImageUrl="Images/Transmittal.png" NavigateUrl="~/Controls/Document/TransmittalList.aspx" />
                </Items>
            </telerik:RadPanelItem>
            <telerik:RadPanelItem Text="LIST" runat="server" Expanded="True" Width="100%">
                <Items>
                    <telerik:RadPanelItem runat="server" Text="Discipline Identifiers" ImageUrl="Images/Document.png" NavigateUrl="DisciplineIdentifier.aspx"/>
                    <telerik:RadPanelItem runat="server" Text="Document Type Identifiers" ImageUrl="Images/Document.png" NavigateUrl="DocumentTypeList.aspx"/>
                    <telerik:RadPanelItem runat="server" Text="Notification Rules" ImageUrl="Images/Document.png" NavigateUrl="NotificationRule.aspx"/>
                </Items>
            </telerik:RadPanelItem>
        </Items>
    </telerik:RadPanelBar>
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Horizontal" Width="100%">
        <telerik:RadPane ID="RadPane3" runat="server" Height="30px" Scrollable="false" Scrolling="None">
            <telerik:RadToolBar ID="CustomerMenu" runat="server" Width="100%" OnClientButtonClicking="OnClientButtonClicking">
                <Items>
                    <telerik:RadToolBarDropDown runat="server" Text="New" ImageUrl="~/Images/addNew.png">
                        <Buttons>
                            <telerik:RadToolBarButton runat="server" Text="Document" Value="1" ImageUrl="~/Images/addDocument.png"></telerik:RadToolBarButton>
                            <telerik:RadToolBarButton runat="server" Text="Multi documents" Value="2" ImageUrl="~/Images/addmulti.png"></telerik:RadToolBarButton>
                        </Buttons>
                    </telerik:RadToolBarDropDown>
                    <telerik:RadToolBarButton runat="server" IsSeparator="true">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarDropDown runat="server" Text="Action" >
                        <Buttons>
                            <telerik:RadToolBarButton runat="server" Text="Send notifications" Value="3" ImageUrl="~/Images/sendnotification.png"></telerik:RadToolBarButton>
                            <telerik:RadToolBarButton runat="server" Text="View explorer" Value="4" ImageUrl="~/Images/expview.png" />
                        </Buttons>
                    </telerik:RadToolBarDropDown>
                    <telerik:RadToolBarButton runat="server" IsSeparator="true">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton runat="server">
                        <ItemTemplate>
                            <asp:Label ID="lblSearchLabel" runat="server" Text="  Quick search:  " />
                        </ItemTemplate>
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton Value="searchTextBoxButton" CssClass="searchtextbox" CommandName="searchText">
                        <ItemTemplate>
                            <telerik:RadTextBox
                                runat="server" ID="txtSearch" Width="250px"
                                EmptyMessage="Enter file name, title, doc number." />
                        </ItemTemplate>
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton ImageUrl="~/Images/search.gif" Value="search" CommandName="doSearch" />
                </Items>
            </telerik:RadToolBar>
            
            

        </telerik:RadPane>
        <telerik:RadPane ID="RadPane2" runat="server" Scrollable="false" Scrolling="None">
            <telerik:RadSplitter ID="Radsplitter3" runat="server" Orientation="Horizontal">
                <telerik:RadPane ID="Radpane4" runat="server" Scrolling="None" >
                    
                     <telerik:RadSplitter ID="Radsplitter10" runat="server" Orientation="Vertical">
                        <telerik:RadPane ID="Radpane1" runat="server" Scrolling="Both" Width="270" MinWidth="270">
                            
                            <telerik:RadSlidingZone ID="SlidingZone1" runat="server" Width="20px" DockedPaneId="RadSlidingPane1" >
                                <telerik:RadSlidingPane ID="RadSlidingPane1" Title="Folder Tree" runat="server" Width="270" Height="100%"
                                    OnClientUndocked="OnClientUndocked" OnClientDocked="OnClientDocked" >
                                    <telerik:RadTreeView ID="radTreeFolder" runat="server" Width="100%" Height="100%" ShowLineImages="False" style
                                        AllowNodeEditing="True" 
                                        LoadingStatusPosition="BeforeNodeText"
                                        OnNodeEdit="radTreeFolder_NodeEdit"
                                        OnContextMenuItemClick="radTreeFolder_ContextMenuItemClick"
                                        OnNodeClick="radTreeFolder_NodeClick"
                                        OnNodeExpand="radTreeFolder_NodeExpand"
                                        OnClientContextMenuItemClicking="onClientContextMenuItemClicking"
                                        OnClientNodeExpanded="rtvExplore_OnNodeExpandedCollapsed"
                                        OnClientNodeCollapsed="rtvExplore_OnNodeExpandedCollapsed"
                                        OnClientNodeClicking="onNodeClicking">
                                        <ContextMenus>
                                            <telerik:RadTreeViewContextMenu ID="MainContextMenu" runat="server">
                                                <Items>
                                                    <telerik:RadMenuItem Value="New" Text="New folder" ImageUrl="Images/addfolder.png"/>
                                                    <telerik:RadMenuItem Value="Rename" Text="Rename" ImageUrl="Images/rename.png"/>
                                                    <telerik:RadMenuItem Value="Delete" Text="Delete" ImageUrl="Images/deletefolder.png"/>
                                            
                                                </Items>
                                            </telerik:RadTreeViewContextMenu>
                                        </ContextMenus>

                                        <DataBindings>
                                            <telerik:RadTreeNodeBinding Expanded="false"></telerik:RadTreeNodeBinding>
                                        </DataBindings>
                                    </telerik:RadTreeView>
                                </telerik:RadSlidingPane>
                            </telerik:RadSlidingZone>
                        </telerik:RadPane>

                         <telerik:RadSplitBar ID="Radsplitbar1" runat="server">
                        </telerik:RadSplitBar>
                         
                         <telerik:RadPane ID="Radpane6" runat="server" Scrolling="None">
                                <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                    GridLines="None" Height="100%"
                                    OnItemDataBound="grdDocument_ItemDataBound"
                                    OnDeleteCommand="grdDocument_DeleteCommand" 
                                    OnItemCreated="grdDocument_ItemCreated"
                                    OnNeedDataSource="grdDocument_OnNeedDataSource" 
                                    OnItemCommand="grdDocument_ItemCommand"
                                    PageSize="20" Style="outline: none">
                                    <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" CommandItemDisplay="Top">
                                        <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" />
                                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                                            <telerik:GridBoundColumn DataField="DisciplineID" UniqueName="DisciplineID" Visible="False" />
                                            <telerik:GridBoundColumn DataField="FilePath" UniqueName="FilePath" Visible="False" />

                                            <telerik:GridTemplateColumn UniqueName="IsSelected">
                                                <HeaderStyle Width="2%"  />
                                                <ItemStyle HorizontalAlign="Center" Width="2%"/>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="IsSelected" runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn>
                                                <HeaderStyle Width="2%"  />
                                                <ItemStyle HorizontalAlign="Center" Width="2%"/>
                                                <ItemTemplate>
                                                    <asp:Image ID="EditLink" runat="server" ImageUrl="~/Images/edit.png" Style="cursor: pointer;" AlternateText="Edit properties" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Do you want to delete document? <br/> Caution: All revision relate with this document will be deleted." ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                                                <HeaderStyle Width="2%" />
                                                 <ItemStyle HorizontalAlign="Center" Width="2%"  />
                                            </telerik:GridButtonColumn>

                                            <telerik:GridTemplateColumn>
                                                <HeaderStyle Width="2%" />
                                                <ItemStyle HorizontalAlign="Center" Width="2%"/>
                                                <ItemTemplate>
                                                    <a download='<%# DataBinder.Eval(Container.DataItem, "Name") %>' 
                                                        href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' target="_blank">
                                                        <asp:Image ID="CallLink" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "FileExtensionIcon") %>' 
                                                            Style="cursor: pointer;" AlternateText="Download document" /> 
                                                    </a>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <%--3--%>
                                            <telerik:GridTemplateColumn UniqueName="Name" HeaderText="Name">
                                                <HeaderStyle Width="12%" HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="12%"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'></asp:Label>
                                                        <asp:Image ID="newicon" runat="server" ImageUrl="Images/new.png" Visible="False" /> 
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <%--<telerik:GridBoundColumn DataField="Name" HeaderText="Name" UniqueName="Name">
                                                <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>--%>
                                            <%--4--%>
                                            <telerik:GridBoundColumn DataField="DocumentNumber" HeaderText="Document Number" UniqueName="DocumentNumber">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </telerik:GridBoundColumn>
                                            <%--5--%>
                                            <telerik:GridBoundColumn DataField="Title" HeaderText="Title" UniqueName="Title">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%"  />
                                            </telerik:GridBoundColumn>
                                            <%--6--%>
                                            <telerik:GridBoundColumn DataField="Revision.Name" HeaderText="Revision" UniqueName="Revision">
                                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%"/>
                                            </telerik:GridBoundColumn>
                                            <%--7--%>
                                            <telerik:GridBoundColumn DataField="Status.Name" HeaderText="Status" UniqueName="Status">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%"/>
                                            </telerik:GridBoundColumn>
                                            <%--8--%>
                                            <telerik:GridBoundColumn DataField="Discipline.Name" HeaderText="Discipline" UniqueName="Discipline">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%"/>
                                            </telerik:GridBoundColumn>
                                            <%--9--%>
                                            <telerik:GridBoundColumn DataField="DocumentType.Name" HeaderText="Document Type" UniqueName="DocumentType">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%"/>
                                            </telerik:GridBoundColumn>
                                            <%--10--%>
                                            <telerik:GridBoundColumn DataField="ReceivedFrom.Name" HeaderText="Received From" UniqueName="ReceivedFrom">
                                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                <ItemStyle HorizontalAlign="Left" Width="5%"/>
                                            </telerik:GridBoundColumn>
                                            <%--11--%>
                                            <telerik:GridBoundColumn DataField="ReceivedDate" HeaderText="Received Date" UniqueName="ReceivedDate" 
                                                DataFormatString="{0:dd/MM/yyyy}" >
                                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%"/>
                                            </telerik:GridBoundColumn>
                                            <%--12--%>
                                            <telerik:GridBoundColumn DataField="Description" HeaderText="Description" UniqueName="Description">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%"/>
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings Selecting-AllowRowSelect="true" EnablePostBackOnRowClick="true" AllowColumnHide="True">
                                        <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                                        <Selecting AllowRowSelect="true" />
                                        <ClientEvents  OnGridCreated="GetGridObject" />
                                        <ClientEvents OnRowContextMenu="RowContextMenu" OnRowClick="RowClick"></ClientEvents>
                                        <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                                    </ClientSettings>
                            </telerik:RadGrid>
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
    <telerik:RadContextMenu ID="radMenu" runat="server"
        EnableRoundedCorners="true" EnableShadows="true" OnClientItemClicking="gridMenuClicking">
        <Items>
            <telerik:RadMenuItem Text="Revision history" ImageUrl="~/Images/revision.png" Value="RevisionHistory">
            </telerik:RadMenuItem>
            <telerik:RadMenuItem Text="Version history" ImageUrl="~/Images/history.png" Value="VersionHistory">
            </telerik:RadMenuItem>
            <telerik:RadMenuItem IsSeparator="True">
            </telerik:RadMenuItem>
            <telerik:RadMenuItem Text="Check out" ImageUrl="~/Images/checkout.png" Value="CheckOut">
            </telerik:RadMenuItem>
            <telerik:RadMenuItem Text="Check in" ImageUrl="~/Images/checkin.png" Value="CheckIn">
            </telerik:RadMenuItem>
        </Items>
    </telerik:RadContextMenu>

    <span style="display: none">
        

        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                
                <telerik:AjaxSetting AjaxControlID="radPbCategories">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radTreeFolder" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radMenu">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radTreeFolder">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radTreeFolder" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="radTreeFolder">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="CustomerMenu"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="grdDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContainer" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="treeCustomerType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <%--<telerik:AjaxSetting AjaxControlID="RadTabStrip1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadMultiPage1" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadTabStrip1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadTabStrip1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadMultiPage1" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadMultiPage1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadMultiPage1" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </span>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Document Information"
                VisibleStatusbar="false" Height="690" Width="650" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="UploadMulti" runat="server" Title="Create multiple documents"
                VisibleStatusbar="false" Height="520" MinHeight="520" MaxHeight="520" Width="640" MinWidth="640" MaxWidth="640"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RevisionDialog" runat="server" Title="Revision history" OnClientClose="refreshGrid"
                VisibleStatusbar="false" Height="482" Width="1200" MinHeight="482" MinWidth="1200" MaxHeight="482" MaxWidth="1200" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="VersionHistory" runat="server" Title="Version history"
                VisibleStatusbar="false" Height="332" Width="1200" MinHeight="332" MinWidth="1200" MaxHeight="332" MaxWidth="1200" 
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
            
            var radDocuments;

            function refreshGrid() {
                var masterTable = $find("<%=grdDocument.ClientID%>").get_masterTableView();
                masterTable.rebind();
            }


            function GetGridObject(sender, eventArgs) {
                radDocuments = sender;
            }

            function onColumnHidden(sender) {
                
                var masterTableView = sender.get_masterTableView().get_element();
                masterTableView.style.tableLayout = "auto";
                //window.setTimeout(function () { masterTableView.style.tableLayout = "auto"; }, 0);
            }

            // Undocked and Docked event slide bar Tree folder
            function OnClientUndocked(sender, args) {
                radDocuments.get_masterTableView().showColumn(7);
                radDocuments.get_masterTableView().showColumn(8);
                radDocuments.get_masterTableView().showColumn(9);
                
                radDocuments.get_masterTableView().showColumn(11);
                radDocuments.get_masterTableView().showColumn(12);
            }
            
            function OnClientDocked(sender, args) {
                radDocuments.get_masterTableView().hideColumn(7);
                radDocuments.get_masterTableView().hideColumn(8);
                radDocuments.get_masterTableView().hideColumn(9);
                
                radDocuments.get_masterTableView().hideColumn(11);
                radDocuments.get_masterTableView().hideColumn(12);
            }

            function RowClick(sender, eventArgs) {
                var Id = eventArgs.getDataKeyValue("ID");
                document.getElementById("<%= lblDocId.ClientID %>").value = Id;
            }

            function gridMenuClicking(sender, args) {
                var itemValue = args.get_item().get_value();
                var docId = document.getElementById("<%= lblDocId.ClientID %>").value;

                

                switch (itemValue) {
                    case "RevisionHistory":
                        var owd = $find("<%=RevisionDialog.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/RevisionHistory.aspx?docId=" + docId, "RevisionDialog");
                        break;
                    case "VersionHistory":
                        var owd = $find("<%=VersionHistory.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/VersionHistory.aspx?docId=" + docId, "VersionHistory");
                        break;
                }
            }
            
            function onClientContextMenuItemClicking(sender, args) {
                var menuItem = args.get_menuItem();
                var treeNode = args.get_node();
                menuItem.get_menu().hide();

                switch (menuItem.get_value()) {
                case "Rename":
                    treeNode.startEdit();
                    break;
                case "Delete":
                    var result = confirm("Are you sure you want to delete the folder: " + treeNode.get_text());
                    args.set_cancel(!result);
                    break;
                }
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

                toolbar = $find("<%= CustomerMenu.ClientID %>");
                ajaxManager = $find("<%=ajaxCustomer.ClientID %>");

                searchButton = toolbar.findButtonByCommandName("doSearch");

                $telerik.$(".searchtextbox")
                    .bind("keypress", function (e) {
                        searchButton.set_imageUrl("images/search.gif");
                        searchButton.set_value("search");
                    });
            }

            function ShowEditForm(id, rowIndex) {
                var grid = $find("<%= grdDocument.ClientID %>");
                var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;

                var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                grid.get_masterTableView().selectItem(rowControl, true);
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Document/DocumentInfoEditForm.aspx?docId=" + id + "&folId=" + selectedFolder, "CustomerDialog");
                
                // window.radopen("Controls/Customers/CustomerEditForm.aspx?patientId=" + id, "CustomerDialog");
                //  return false;
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

            function RowDblClick(sender, eventArgs) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Customers/ViewCustomerDetails.aspx?docId=" + eventArgs.getDataKeyValue("Id"), "CustomerDialog");
                // window.radopen("Controls/Customers/ViewCustomerDetails.aspx?patientId=" + eventArgs.getDataKeyValue("Id"), "CustomerDialog");
            }
            
            function onNodeClicking(sender, args) {
                var folderValue = args.get_node().get_value();
                document.getElementById("<%= lblFolderId.ClientID %>").value = folderValue;
            }
            
            function radPbCategories_OnClientItemClicking(sender, args)
            {
                var item = args.get_item();
                var categoryId = item.get_value();
                document.getElementById("<%= lblCategoryId.ClientID %>").value = categoryId;
                
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
            
            function OnClientButtonClicking(sender, args) {
                var button = args.get_item();
                var strText = button.get_text();
                var strValue = button.get_value();

                var grid = $find("<%= grdDocument.ClientID %>");
                var customerId = null;
                var customerName = "";

                //if (grid.get_masterTableView().get_selectedItems().length > 0) {
                //    var selectedRow = grid.get_masterTableView().get_selectedItems()[0];
                //    customerId = selectedRow.getDataKeyValue("Id");
                //    //customerName = selectedRow.Items["FullName"]; 
                //    //customerName = grid.get_masterTableView().getCellByColumnUniqueName(selectedRow, "FullName").innerHTML;
                //}Send notifications window.open('file://Shared Folder/Users')

                if (strText == "Send notifications") {
                    var grid = $find("<%=grdDocument.ClientID %>");
                    var masterTable = grid.get_masterTableView();
                    var number = 0;
                    for (var i = 0; i < masterTable.get_dataItems().length; i++) {
                        var gridItemElement = masterTable.get_dataItems()[i].findElement("IsSelected");
                        if (gridItemElement.checked) {
                            number++;
                        }
                    }

                    if (number == 0) {
                        alert("Please select documents to send notification");
                    }
                    else {
                        ajaxManager.ajaxRequest("SendNotification");
                    }
                }
                
                ////if (strText == "View explorer") {
                ////    window.open("file://WIN-P7KS57HL1HG/DocumentLibrary");
                ////}

                if (strText == "Document") {

                    var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                    if (selectedFolder == "") {
                        alert("Please choice one folder to create new document");
                        return false;
                    }

                    var owd = $find("<%=CustomerDialog.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/Document/DocumentInfoEditForm.aspx?folId=" + selectedFolder, "CustomerDialog");

                }
                
                if (strText == "Multi documents") {
                    var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                    if (selectedFolder == "") {
                        alert("Please choice one folder to create new document");
                        return false;
                    }

                    var owd = $find("<%=UploadMulti.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/Document/UploadDragDrop.aspx?folId=" + selectedFolder, "UploadMulti");
                }

                if (strText == "Thêm mới") {
                    return ShowInsertForm();
                }
                else if (strText == "Import dữ liệu") {
                    return ShowImportForm();
                }
                else if (strText == "Dữ liệu thô") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_1_" + customerId);
                }
                else if (strText == "Tiềm năng") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_2_" + customerId);
                }
                else if (strText == "Chưa liên hệ được") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_3_" + customerId);
                }
                else if (strText == "Không tiềm năng") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_4_" + customerId);
                }
                else if (strText == "Thông tin sai") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_5_" + customerId);
                }
                else if (strText == "Liên hệ tư vấn") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_6_" + customerId);
                }
                else if (strText == "Hẹn tư vấn") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_7_" + customerId);
                }
                else if (strText == "Đã sử dụng dịch vụ") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_8_" + customerId);
                }
                else {
                    var commandName = args.get_item().get_commandName();
                    if (commandName == "doSearch") {
                        var searchTextBox = sender.findButtonByCommandName("searchText").findControl("txtSearch");
                        if (searchButton.get_value() == "clear") {
                            searchTextBox.set_value("");
                            searchButton.set_imageUrl("images/search.gif");
                            searchButton.set_value("search");
                        }

                        performSearch(searchTextBox);
                    } else if (commandName == "reply") {
                        window.radopen(null, "Edit");
                    }
                }
            }
            
            function performSearch(searchTextBox) {
                if (searchTextBox.get_value()) {
                    searchButton.set_imageUrl("images/clear.gif");
                    searchButton.set_value("clear");
                }

                ajaxManager.ajaxRequest(searchTextBox.get_value());
            }
            function onTabSelecting(sender, args) {
                if (args.get_tab().get_pageViewID()) {
                    args.get_tab().set_postBack(false);
                }
            }
            

            function RowContextMenu(sender, eventArgs) {
                var menu = $find("<%=radMenu.ClientID %>");
                var evt = eventArgs.get_domEvent();

                if (evt.target.tagName == "INPUT" || evt.target.tagName == "A") {
                    return;
                }

                var index = eventArgs.get_itemIndexHierarchical();
                document.getElementById("radGridClickedRowIndex").value = index;
                
                var Id = eventArgs.getDataKeyValue("ID");
                document.getElementById("<%= lblDocId.ClientID %>").value = Id;

                sender.get_masterTableView().selectItem(sender.get_masterTableView().get_dataItems()[index].get_element(), true);

                menu.show(evt);

                evt.cancelBubble = true;
                evt.returnValue = false;

                if (evt.stopPropagation) {
                    evt.stopPropagation();
                    evt.preventDefault();
                }
            }
        /* ]]> */
        </script>
    </telerik:RadCodeBlock>
</asp:Content>

<%--Tan.Le Remove here--%>
<%--<uc1:List runat="server" ID="CustomerList"/>--%>
<%-- <div id="EDMsCustomers" runat="server" />--%>
