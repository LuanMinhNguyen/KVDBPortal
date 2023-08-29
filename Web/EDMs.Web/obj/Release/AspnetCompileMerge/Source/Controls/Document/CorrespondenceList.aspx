<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CorrespondenceList.aspx.cs" Inherits="EDMs.Web.Controls.Document.CorrespondenceList" EnableViewState="true" %>

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
            padding-right: 2px !important;
            vertical-align: top;
        }

        .RadGrid .rgSelectedRow
        {
            background-image : none !important;
            background-color: coral !important;
        }

        /*Hide change page size control*/
        /*div.RadGrid .rgPager .rgAdvPart     
        {     
        display:none;        sssssss
        }*/    

        .rtPlus, .rtMinus
        {
            background-image: url('../../Images/plus-minus-icon1.png') !important;
			cursor: pointer;
        }

        .rtIn {
            cursor: pointer;
        }
		 
        .new1 .rtPlus, .new1 .rtMinus
        {
            background-image: url('~/Images/plus-minus-icon2.png') !important;
        }
         
        .new2 .rtPlus, .new2 .rtMinus
        {
            background-image: url('~/Images/plus-minus-icon3.png') !important;
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

        .rspSlideHeaderUndockIcon {
            display: none !important;
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
            padding-bottom: 20px;
            padding-left: 223px;
        }

        .RadAjaxPanel {
            height: 100% !important;
        }
    </style>
    <telerik:RadPanelBar ID="radPbSearch" runat="server" Width="100%" Visible="False">
        <Items>
            <telerik:RadPanelItem Text="SEARCH DOCUMENT" runat="server" Expanded="True" Width="100%">
                <Items>
                    <telerik:RadPanelItem runat="server" Text="Search Documents" ImageUrl="~/Images/search.gif" NavigateUrl="~/Search.aspx" />
                </Items>
            </telerik:RadPanelItem>
        </Items>
    </telerik:RadPanelBar>
    <telerik:RadPanelBar ID="radPbCategories" runat="server" Visible="False" OnClientItemClicking="radPbCategories_OnClientItemClicking" Width="100%"></telerik:RadPanelBar>
        
     <telerik:RadPanelBar ID="RadPanelBarTransmittal" runat="server" Width="100%" >
        <Items>
            <telerik:RadPanelItem Text="DOCUMENT MANAGEMENT" runat="server" Expanded="True" Width="100%">
                <Items>
                    <telerik:RadPanelItem runat="server" Text="InComing Transmittal" ImageUrl="~/Images/in.png" NavigateUrl="~/Controls/Document/IncomingTransmittalList.aspx" Visible="False"/>

                    <telerik:RadPanelItem runat="server" Text="Document Library" ImageUrl="~/Images/library.png" NavigateUrl="~/Controls/Document/DocumentsLibrary.aspx" Selected="True" />
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
            <telerik:RadToolBar ID="CustomerMenu" runat="server" Width="100%" OnClientButtonClicking="OnClientButtonClicking">
                <Items>
                    <telerik:RadToolBarButton runat="server" Text="New Document" Value="1" ImageUrl="~/Images/addDocument.png"/>
                    <telerik:RadToolBarButton runat="server" IsSeparator="True"/>
                    <telerik:RadToolBarDropDown runat="server" Text="Action" Visible="true" ImageUrl="~/Images/action.png">
                        <Buttons>
                            <telerik:RadToolBarButton runat="server" Text="Download multi documents" Value="4" ImageUrl="~/Images/download.png"/>
                            <telerik:RadToolBarButton runat="server" Text="Delete all selected files" Value="delete" ImageUrl="~/Images/delete.png" />
                            <telerik:RadToolBarButton runat="server" IsSeparator="True"/>
                            <telerik:RadToolBarButton runat="server" Text="Upload Document" Value="6" ImageUrl="~/Images/upload.png" Visible="false"/>
                            <telerik:RadToolBarButton runat="server" Text="Export Document" Value="5" ImageUrl="~/Images/export.png" />
                            <telerik:RadToolBarButton runat="server" Text="Import Document" Value="5" ImageUrl="~/Images/import.png" />
                        </Buttons>
                    </telerik:RadToolBarDropDown>
                     <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                    
                    <telerik:RadToolBarButton runat="server" Value="IsFilter">
                        <ItemTemplate>
                            <asp:CheckBox ID="ckbEnableFilter" runat="server" Text="Enable filter list overdue answer " AutoPostBack="True"
                                OnCheckedChanged="ckbEnableFilter_CheckedChanged" />
                        </ItemTemplate>
                    </telerik:RadToolBarButton>
                </Items>
            </telerik:RadToolBar>
        </telerik:RadPane>

        <telerik:RadPane ID="RadPane2" runat="server" Scrollable="false" Scrolling="None">
            <telerik:RadSplitter ID="Radsplitter3" runat="server" Orientation="Horizontal">
                <telerik:RadPane ID="Radpane4" runat="server" Scrolling="None" >
                    
                     <telerik:RadSplitter ID="Radsplitter10" runat="server" Orientation="Vertical">
                        <telerik:RadPane ID="Radpane1" runat="server" Scrolling="Both" Width="200" MinWidth="200">
                            
                            <telerik:RadSlidingZone ID="SlidingZone1" runat="server" Width="20px" DockedPaneId="RadSlidingPane1">
                                <telerik:RadSlidingPane ID="RadSlidingPane1" runat="server" 
                                    Title="Folder Tree"  Width="350" Height="100%">
                                    <telerik:RadTreeView ID="radTreeFolder" runat="server" Width="100%" Height="100%"               
                                        ShowLineImages="False"
                                        OnNodeEdit="radTreeFolder_NodeEdit" 
                                        OnContextMenuItemClick="radTreeFolder_ContextMenuItemClick"
                                        OnNodeClick="radTreeFolder_NodeClick"
                                        OnNodeDataBound="radTreeFolder_OnNodeDataBound"
                                        OnClientContextMenuItemClicking="onClientContextMenuItemClicking"
                                        OnClientContextMenuShowing="onClientContextMenuShowing"
                                        OnClientNodeExpanded="rtvExplore_OnNodeExpandedCollapsed"
                                        OnClientNodeCollapsed="rtvExplore_OnNodeExpandedCollapsed"
                                        OnClientNodeClicking="onNodeClicking">
                                        <ContextMenus>
                                            <telerik:RadTreeViewContextMenu ID="MainContextMenu" runat="server">
                                                <Items>
                                                    <telerik:RadMenuItem Value="New" Text="New folder" ImageUrl="~/Images/addfolder.png"/>
                                                    <telerik:RadMenuItem Value="Rename" Text="Rename" ImageUrl="~/Images/rename.png"/>
                                                    <telerik:RadMenuItem Value="Delete" Text="Delete" ImageUrl="~/Images/deletefolder.png"/>
                                                    <telerik:RadMenuItem IsSeparator="True"/>
                                                    <telerik:RadMenuItem Visible="False" Value="MoveFolder" Text="Move" ImageUrl="~/Images/move.png" Enabled="false"/>
                                                    <telerik:RadMenuItem Value="Permission" Visible="true" Text="Permission" ImageUrl="~/Images/datap.png"/>

                                                    
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
                                    AllowMultiRowSelection="True" AllowFilteringByColumn="True"
                                    GridLines="None" Height="100%"
                                    OnNeedDataSource="grdDocument_OnNeedDataSource" 
                                    OnDeleteCommand="grdDocument_DeleteCommand"
                                    PageSize="100" Style="outline: none" Width="100%">
                                    <GroupingSettings CaseSensitive="False"></GroupingSettings>
                                    <MasterTableView
                                        ClientDataKeyNames="ID" DataKeyNames="ID" Font-Size="8pt">
                                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <%--<GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldName="DirName"></telerik:GridGroupByField>
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldName="DirName"></telerik:GridGroupByField>
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>
                                        </GroupByExpressions>--%>
                                        <Columns>
                                            <%--<telerik:GridTemplateColumn UniqueName="IsSelected" AllowFiltering="false">
                                                <HeaderStyle Width="5%"  />
                                                <ItemStyle HorizontalAlign="Center" Width="5%"/>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="IsSelected" runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>--%>
                                            
                                            <telerik:GridClientSelectColumn UniqueName="IsSelected">
                                                <HeaderStyle Width="25"  />
                                                <ItemStyle HorizontalAlign="Center" Width="25"/>
                                            </telerik:GridClientSelectColumn>
                                            
                                            <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="DeleteColumn">
                                            <HeaderStyle Width="25" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle Width="25" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <!--BtnDeleteClick-->
                                                <asp:ImageButton runat="server" Style="padding-left: 7px; cursor: pointer; border-color: transparent" ID="btnDelete" 
                                                    ImageUrl="~/Images/delete.png" CommandName="Delete"
                                                    OnClientClick="return confirm('Do you want to delete document?');"/>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                                                <HeaderStyle Width="25"/>
                                                <ItemStyle HorizontalAlign="Center" Width="25" />
                                                <ItemTemplate>
                                                   <%-- <div runat="server" ID="ViewForFolder" Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsFolder")) %>'>
                                                        <asp:Image ID="Image1" runat="server" style="border-color: transparent"
                                                        ImageUrl='<%# DataBinder.Eval(Container.DataItem, "FileExtensionIcon") %>' />
                                                    </div>--%>
                                                    <div runat="server" ID="ViewForDoc">
                                                  <%--  <a download='<%# DataBinder.Eval(Container.DataItem, "FileName") %>' 
                                                        href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' target="_blank">
                                                        <asp:Image ID="CallLink" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "FileExtensionIcon") %>' 
                                                            Style="cursor: pointer; border-color: transparent" AlternateText="Download document" /> 
                                                    </a>--%>
                                                   <a download='<%# DataBinder.Eval(Container.DataItem, "FileName") %>' 
                                                        href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' target="_blank">
                                                        <asp:Image ID="Image3" runat="server" ToolTip="Download document file"
                                                            Visible='<%# DataBinder.Eval(Container.DataItem, "FilePath") != null? true :false %>'
                                                            ImageUrl=' <%# DataBinder.Eval(Container.DataItem, "FilePath") != null && System.IO.File.Exists(Server.MapPath( DataBinder.Eval(Container.DataItem, "FilePath").ToString()))? DataBinder.Eval(Container.DataItem, "FileExtensionIcon") : "~/Images/error1.png" %>'
                                                            Style="cursor: pointer; border-color: white" AlternateText="Download document"/> 
                                                    </a>
                                                     </div>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="EmailColumn">
                                                <HeaderStyle Width="30"  />
                                                <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <a href='javascript:SendMail(<%# DataBinder.Eval(Container.DataItem, "ID") %>)' style="text-decoration: none; color:blue">
                                                    <asp:Image ID="EmailLink" runat="server" ImageUrl="~/Images/email1.png" Style="cursor: pointer;" AlternateText="Send Email" />
                                                        <a/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                             <telerik:GridTemplateColumn UniqueName="DocumentNumber" HeaderText="M - Number" DataField="DocumentNumber" 
                                                ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                <HeaderStyle Width="200" HorizontalAlign="Center"/>
                                                <ItemStyle HorizontalAlign="Left" Width="200"/>
                                                <ItemTemplate>
                                                    <a href='javascript:ShowEditForm(<%# DataBinder.Eval(Container.DataItem, "ID") %>,<%#DataBinder.Eval(Container.DataItem,"FolderID") %>)' style="text-decoration: none; color:blue">
                                                        <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentNumber") %>'></asp:Label>
                                                    </a>
                                                        <asp:Image ID="newicon"  runat="server" ImageUrl="~/Images/new.png" Visible='<%# (DateTime.Now - Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "CreatedDate"))).TotalHours < 24 %>' /> 
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:HiddenField ID="Name" runat="server" Value='<%# Eval("DocumentNumber") %>'/>
                                                    
                                                    <asp:TextBox ID="txtName" runat="server" Width="100%"></asp:TextBox>
                                                </EditItemTemplate>
                                            </telerik:GridTemplateColumn>
                                          
                                         <telerik:GridDateTimeColumn HeaderText="Issue Date" UniqueName="IssueDate" DataField="IssueDate"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>  
                                    <telerik:GridTemplateColumn HeaderText="Reference Docs" UniqueName="ReferenceDocs" DataField="ReferenceDocs"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Left" Width="150" />
                                        <ItemTemplate>
                                            <%# Eval("ReferenceDocs") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                     
                                     <telerik:GridDateTimeColumn HeaderText="Answer Request" UniqueName="AnswerRequestDate" DataField="AnswerRequestDate"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>  
                                    <telerik:GridTemplateColumn HeaderText="Document Title" UniqueName="Title" DataField="Title"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="220" />
                                        <ItemStyle HorizontalAlign="Left" Width="220" />
                                        <ItemTemplate>
                                            <%# Eval("Title") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn> 
                                   <telerik:GridTemplateColumn HeaderText="Discipline" UniqueName="DisciplineName" DataField="DisciplineName"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Left" Width="150" />
                                        <ItemTemplate>
                                            <%# Eval("DisciplineName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>  
                                    <telerik:GridTemplateColumn HeaderText="Lead" UniqueName="Leader" DataField="Leader"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                        <ItemTemplate>
                                            <%# Eval("Leader") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn> 
                                     <telerik:GridTemplateColumn HeaderText="Information" UniqueName="UserInforName" DataField="UserInforName"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                        <ItemTemplate>
                                            <%# Eval("UserInforName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>   
                                     <telerik:GridTemplateColumn HeaderText="Reply" UniqueName="Reply" DataField="Reply"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                        <ItemTemplate>
                                            <%# Eval("Reply") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn> 
                                     <telerik:GridTemplateColumn HeaderText="Remark" UniqueName="Remark" DataField="Remark"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                        <ItemTemplate>
                                            <%# Eval("Remark") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                     <telerik:GridTemplateColumn HeaderText="From" UniqueName="FromName" DataField="FromName"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                        <ItemTemplate>
                                            <%# Eval("FromName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                   <telerik:GridTemplateColumn HeaderText="To" UniqueName="ToName" DataField="ToName"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                        <ItemTemplate>
                                            <%# Eval("ToName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn> 
                                    <telerik:GridTemplateColumn HeaderText="Document Type" UniqueName="DocumentTypeName" DataField="DocumentTypeName"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                        <ItemTemplate>
                                            <%# Eval("DocumentTypeName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>     
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                                        <ClientEvents OnGridCreated="GetGridObject"/>
                                        <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                                    </ClientSettings>
                            </telerik:RadGrid>
                        </telerik:RadPane>

                     </telerik:RadSplitter>       

                </telerik:RadPane>
                
            </telerik:RadSplitter>
        </telerik:RadPane>
    </telerik:RadSplitter>
    
    <span style="display: none">
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest"  >
            <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="radTreeFolder" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="radTreeFolder">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radTreeFolder" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="CustomerMenu" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
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
                <telerik:AjaxSetting AjaxControlID="radViewSettingBar">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="SlidingZone1" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ckbEnableFilter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </span>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Document Information"
                VisibleStatusbar="false" Height="690" Width="650"  OnClientClose="refreshGrid"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="CheckOut" runat="server" Title="Checkout Document"
                VisibleStatusbar="false" Height="250" Width="650" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>

            <telerik:RadWindow ID="MoveFolder" runat="server" Title="Move Files" OnClientClose="refreshGrid"
                VisibleStatusbar="false" Height="500" Width="650" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="FolderPermission" runat="server" Title="Folder Permission"
                VisibleStatusbar="false" Height="600" Width="650" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="UploadMulti" runat="server" Title="Create multiple documents"
                VisibleStatusbar="true" Height="520" MinHeight="520" MaxHeight="520" Width="640" MinWidth="640" MaxWidth="640"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RevisionDialog" runat="server" Title="Revision history" OnClientClose="refreshGrid"
                VisibleStatusbar="false" Height="482" Width="1250" MinHeight="600" MinWidth="1250" MaxHeight="600" MaxWidth="1250" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="SendMail" runat="server" Title="Send mail"
                VisibleStatusbar="false" Height="560" Width="992" MinHeight="560" MinWidth="992" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="VersionHistory" runat="server" Title="Checkout/in history"
                VisibleStatusbar="false" Height="322" Width="1000" MinHeight="322" MinWidth="1000" MaxHeight="322" MaxWidth="1000" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ImportData" runat="server" Title="Upload data file"
                VisibleStatusbar="false" Height="350" Width="480" OnClientClose="refreshGrid"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
    
            <telerik:RadWindow ID="ExportData" runat="server" Title="Export master list"
                VisibleStatusbar="false" Height="350" Width="480" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
                
                <telerik:RadWindow ID="AttachMulti" runat="server" Title="Upload multi document files" OnClientClose="refreshGrid"
                VisibleStatusbar="false" Height="600" Width="700" MinHeight="600" MinWidth="700" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
    <asp:HiddenField runat="server" ID="IsUpdatePermission"/>
    <asp:HiddenField runat="server" ID="FolderContextMenuAction"/>
    <asp:HiddenField runat="server" ID="lblFolderId"/>
    <asp:HiddenField runat="server" ID="lblDocId"/>
    <asp:HiddenField runat="server" ID="lblCategoryId"/>
    <asp:HiddenField runat="server" ID="isCheckOut"/>
    <asp:HiddenField runat="server" ID="CurrentRoleId"/>
    <asp:HiddenField runat="server" ID="CurrentCheckoutRoleId"/>
    <input type="hidden" id="radGridClickedRowIndex" name="radGridClickedRowIndex"/>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="../../Scripts/jquery-1.7.1.js"></script>
        <script type="text/javascript">
            
            var radDocuments;

            function ShowFilter(obj) {
                if (obj.checked) {
                    $find('<%=grdDocument.ClientID %>').get_masterTableView().showFilterItem();
                } else {
                    $find('<%=grdDocument.ClientID %>').get_masterTableView().hideFilterItem();
                }
            }

            function refreshGrid() {
                var masterTable = $find("<%=grdDocument.ClientID%>").get_masterTableView();
                masterTable.rebind();
            }

            function refreshTreeFolder() {
                ajaxManager.ajaxRequest("RebindTreeFolder");
            }

            function ExportGrid() {
                var masterTable = $find("<%=grdDocument.ClientID %>").get_masterTableView();
                masterTable.exportToExcel('PIN_list.xls');
                return false;
            }

            function GetGridObject(sender, eventArgs) {
                radDocuments = sender;
            }

             function onRequestStart(sender, args)
            {
                 ////alert(args.get_eventTarget());
                 if (args.get_eventTarget().indexOf("ExportTo") >= 0 || args.get_eventTarget().indexOf("ajaxCustomer") >= 0)
                 {
                     args.set_enableAjax(false);
                 }
             }
             function SendMail(Id) {
                 var owd = $find("<%=SendMail.ClientID %>");
                 owd.setSize(730, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("SendMail.aspx?Doc=" +Id, "SendMail");
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
                        var categoryId = document.getElementById("<%= lblCategoryId.ClientID %>").value;
                        var owd = $find("<%=RevisionDialog.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/RevisionHistory.aspx?docId=" + docId + "&categoryId=" + categoryId, "RevisionDialog");
                        break;
                    case "VersionHistory":
                        var owd = $find("<%=VersionHistory.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/VersionHistory.aspx?docId=" + docId, "VersionHistory");
                        break;
                    case "EditDocument":
                        ShowEditForm(docId);
                        break;
                    case "CheckOut":
                        var owd = $find("<%=CheckOut.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/CheckOutDocument.aspx?docId=" + docId, "CheckOut");
                        break;
                    case "CheckIn":
                        ajaxManager.ajaxRequest("CheckInDocument");
                        break;
                    case "CheckOutInHistory":
                        var owd = $find("<%=VersionHistory.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/CheckOutInHistoryList.aspx?docId=" + docId, "VersionHistory");
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
                        // var result = confirm("Are you sure you want to delete the folder: " + treeNode.get_text());
                        var retVal = prompt("Are you sure you want to delete the folder:    " + treeNode.get_text() + ".\n Please fill in the box below. ", "Enter (OK) here");
                        var result = retVal == "OK";
                        args.set_cancel(!result);
                        break;
                    case "MoveFolder":
                        var owd = $find("<%=MoveFolder.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/MoveFileAndFolder.aspx?mode=1&folderId=" + treeNode.get_value(), "MoveFolder");
                        break;
                    case "Permission":
                        ////var childNodeIds = "";

                        ////if (treeNode._hasChildren() == true) {
                        ////    var allNodes = treeNode.get_allNodes();
                        ////    for (var i = 0; i < allNodes.length; i++) {
                        ////        childNodeIds += allNodes[i].get_value() + "*";
                                
                        ////    }
                        ////}
                        ////alert(treeNode.get_value());
                        
                        ////Set_Cookie("allchildfolder", childNodeIds, 1);
                        
                        ////alert(childNodeIds);
                        ////alert(document.cookie);

                        var owd = $find("<%=FolderPermission.ClientID %>");
                        owd.Show();
                        owd.setUrl("FolderPermission.aspx?folderId=" + treeNode.get_value(), "FolderPermission");
                        break;
                }
            }
            
            function gridContextMenuShowing(menu, args) {
                var isCheckOut = document.getElementById("<%= isCheckOut.ClientID %>").value;
                var currentCheckoutRoleId = document.getElementById("<%= CurrentCheckoutRoleId.ClientID %>").value;
                var currentRoleId = document.getElementById("<%= CurrentRoleId.ClientID %>").value;
                
                ////alert(currentRoleId);
                ////alert(currentCheckoutRoleId);
                if (isCheckOut == "True") {
                    

                    if (currentCheckoutRoleId == currentRoleId) {
                        menu.get_allItems()[0].enable();
                        menu.get_allItems()[2].enable();
                        menu.get_allItems()[5].disable();
                        menu.get_allItems()[6].enable();
                    }
                    else {
                        menu.get_allItems()[0].disable();
                        menu.get_allItems()[2].disable();
                        menu.get_allItems()[5].disable();
                        menu.get_allItems()[6].disable();
                    }
                }
                else {
                    menu.get_allItems()[5].enable();
                    menu.get_allItems()[6].disable();
                }
            }
            
            function onClientContextMenuShowing(sender, args) {
                var treeNode = args.get_node();
                var flag = treeNode.get_category();
                //alert(flag);
                var allNodes = treeNode.get_allNodes();
                var isUpdatePermission = document.getElementById("<%= IsUpdatePermission.ClientID %>").value;
                var nodevalue = treeNode.get_value();
                

                if (isUpdatePermission == "false") {
                    args.get_menu().get_items().getItem(0).set_enabled(false);
                    args.get_menu().get_items().getItem(1).set_enabled(false);
                    args.get_menu().get_items().getItem(2).set_enabled(false);
                    args.get_menu().get_items().getItem(3).set_enabled(false);
                    args.get_menu().get_items().getItem(4).set_enabled(false);
                } else {
                    if (allNodes.length == 0) {
                        // 0 refer to the Context Menu item index
                        args.get_menu().get_items().getItem(4).set_enabled(true);
                    }
                    else {
                        args.get_menu().get_items().getItem(4).set_enabled(false);
                    }

                    if (treeNode.get_level() == 0) {
                        args.get_menu().get_items().getItem(0).set_enabled(true);

                        args.get_menu().get_items().getItem(1).set_enabled(false);
                        args.get_menu().get_items().getItem(2).set_enabled(false);
                        
                        args.get_menu().get_items().getItem(3).set_enabled(true);
                        args.get_menu().get_items().getItem(4).set_enabled(true);
                    } else if (treeNode.get_level() == 1) {
                        args.get_menu().get_items().getItem(0).set_enabled(true);
                        args.get_menu().get_items().getItem(1).set_enabled(true);
                        args.get_menu().get_items().getItem(2).set_enabled(true);
                        args.get_menu().get_items().getItem(3).set_enabled(true);
                        args.get_menu().get_items().getItem(4).set_enabled(true);
                    }
                    else if(treeNode.get_level() == 2){
                        args.get_menu().get_items().getItem(0).set_enabled(false);
                        args.get_menu().get_items().getItem(1).set_enabled(true);
                        args.get_menu().get_items().getItem(2).set_enabled(true);
                        args.get_menu().get_items().getItem(3).set_enabled(true);
                        args.get_menu().get_items().getItem(4).set_enabled(true);
                    }
                }

                if (nodevalue == '1355' || nodevalue == '1402') {
                    args.get_menu().get_items().getItem(2).set_enabled(false);
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
            var ajaxManager;

            function pageLoad() {
                $('iframe').load(function () { //The function below executes once the iframe has finished loading<---true dat, althoug Is coppypasta from I don't know where
                    //alert($('iframe').contents());
                });

                ajaxManager = $find("<%=ajaxCustomer.ClientID %>");
            }

          <%--  function ShowEditForm(id) {
                var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                var categoryId = document.getElementById("<%= lblCategoryId.ClientID %>").value;
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Document/DocumentInfoEditForm.aspx?docId=" + id + "&folId=" + selectedFolder + "&categoryId=" + categoryId, "CustomerDialog");
                
                // window.radopen("Controls/Customers/CustomerEditForm.aspx?patientId=" + id, "CustomerDialog");
                //  return false;
            }--%>
            
             function ShowEditForm(id,folder) {
                  var owd = $find("<%=CustomerDialog.ClientID %>");
                    owd.setSize(730, document.documentElement.offsetHeight);
                    owd.Show();
                    owd.setUrl("CorrespondenceInfoEditForm.aspx?docId=" + id + "&folId=" + folder, "CustomerDialog");
                
                // window.radopen("Controls/Customers/CustomerEditForm.aspx?patientId=" + id, "CustomerDialog");
                //  return false;
            }

            function ShowChildFolder(folderId) {
                ajaxManager.ajaxRequest("ShowChildFolder_" + folderId);
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

            function radViewSettingBar_OnClientButtonClicking(sender, args) {
                var button = args.get_item();
                var strText = button.get_text();
                var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                var paneID = "<%= RadSlidingPane1.ClientID %>";
                var slidingZone = $find("<%=SlidingZone1.ClientID %>");
                var dockedPaneId = slidingZone.get_dockedPaneId();

                if (strText.toLowerCase() == "list all documents") {
                    if (selectedFolder == "") {
                        alert("Please choice one folder to view list documents");
                        return false;
                    }
                    else {
                        slidingZone.undockPane(paneID);
                        ajaxManager.ajaxRequest("ListAllDocuments");
                    }
                }
                
                if (strText.toLowerCase() == "tree view") {
                    if (selectedFolder == "") {
                        alert("Please choice one folder to view documents");
                        return false;
                    }
                    else {
                        if (dockedPaneId == null || dockedPaneId != paneID) {
                            slidingZone.collapsePane(paneID);
                            slidingZone.dockPane(paneID);
                        }
                        ajaxManager.ajaxRequest("TreeView");
                    }
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

                if (strText.toLowerCase() == "send notifications") {
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
                
                if (strText.toLowerCase() == "import document") {

                     var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                    if (selectedFolder == "") {
                        alert("Please choice one folder to import document.");
                        return false;
                    }
                    else {
                        var owd = $find("<%=ImportData.ClientID %>");
                        owd.Show();
                        owd.setUrl("ImportCorrespondence.aspx?folId=" + selectedFolder, "ImportData");
                    }
                }

                if (strText.toLowerCase() == "export master list") {
                    var owd = $find("<%=ExportData.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/Document/ExportMasterList.aspx", "ExportData");
                }

                if (strText.toLowerCase() == "move files") {
                    var grid = $find("<%=grdDocument.ClientID %>");
                    var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                    var categoryId = document.getElementById("<%= lblCategoryId.ClientID %>").value;
                    var masterTable = grid.get_masterTableView();
                    var number = 0;
                    var listId = "";
                    for (var i = 0; i < masterTable.get_dataItems().length; i++) {
                        var gridItemElement = masterTable.get_dataItems()[i].findElement("IsSelected");
                        if (gridItemElement.checked) {
                            number++;
                            listId += masterTable.get_dataItems()[i].getDataKeyValue("ID") + ",";
                        }
                    }
                    if (number == 0) {
                        alert("Please select documents to move");
                    }
                    else {
                        var owd = $find("<%=MoveFolder.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/MoveFileAndFolder.aspx?mode=2&docIds=" + listId + "&selectedFolder=" + selectedFolder + "&categoryId=" + categoryId, "MoveFolder");
                    }
                }

                if (strText.toLowerCase() == "send mail") {
                    var grid = $find("<%=grdDocument.ClientID %>");
                    var masterTable = grid.get_masterTableView();
                    var number = 0;
                    var listId = "";
                    for (var i = 0; i < masterTable.get_dataItems().length; i++) {
                        var gridItemElement = masterTable.get_dataItems()[i].findElement("IsSelected");
                        if (gridItemElement.checked) {
                            number++;
                            listId += masterTable.get_dataItems()[i].getDataKeyValue("ID") + ",";
                        }
                    }
                    if (number == 0) {
                        alert("Please select documents to send mail");
                    }
                    else {
                        var owd = $find("<%=SendMail.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/SendMail.aspx?listDoc=" + listId, "SendMail");
                    }
                }
                
                if (strText.toLowerCase() == "upload document") {
                    var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                    if (selectedFolder == "") {
                        alert("Please choice one folder to upload new document");
                        return false;
                    }

                    var owd = $find("<%=AttachMulti.ClientID %>");
                    owd.Show();
                    owd.setUrl("UploadMultiDocumentFile.aspx?folId=" + selectedFolder, "AttachMulti");

                }
                if (strText.toLowerCase() == "new document") {
                    var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                    if (selectedFolder == "") {
                        alert("Please choice one folder to create new document.");
                        return false;
                    }
                    var owd = $find("<%=CustomerDialog.ClientID %>");
                    owd.setSize(730, document.documentElement.offsetHeight);
                    owd.Show();
                    owd.setUrl("CorrespondenceInfoEditForm.aspx?folId=" + selectedFolder, "CustomerDialog");
                
                }
                if (strText.toLowerCase() == "delete all selected files") {
                    if (confirm("Do you want to delete all selected files ?") == false) {
                        return;
                    }
                    ajaxManager.ajaxRequest("DeleteAll");
                }
                
                
                if (strText.toLowerCase() == "multi documents") {
                    var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                    if (selectedFolder == "") {
                        alert("Please choice one folder to create new document.");
                        return false;
                    }

                    var owd = $find("<%=UploadMulti.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/Document/UploadDragDrop.aspx?folId=" + selectedFolder, "UploadMulti");
                }
                
                if (strText.toLowerCase() == "export document") {
                  <%--  var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                    if (selectedFolder == "") {
                        alert("Please choice one folder to export master list.");
                        return false;
                    }
                    else {--%>
                        ajaxManager.ajaxRequest("ExportMasterList");
                    //}
                }

                if (strText.toLowerCase() == "download multi documents") {
                    var grid = $find("<%=grdDocument.ClientID %>");
                    var masterTable = grid.get_masterTableView();

                    var listId = "";

                    var selectedRows = masterTable.get_selectedItems();
                    if (selectedRows.length == 0) {
                        alert("Please select documents to download");
                    }
                    else if (selectedRows.length > 20) {
                        alert("The maximum file download at once time is 20. Current you select " + selectedRows.length + "files.");
                    }
                    else {
                        ajaxManager.ajaxRequest("DownloadMulti");
                    }
                }
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
            

            
        </script>
    </telerik:RadCodeBlock>
</asp:Content>

<%--Tan.Le Remove here--%>
<%--<uc1:List runat="server" ID="CustomerList"/>--%>
<%-- <div id="EDMsCustomers" runat="server" />--%>
