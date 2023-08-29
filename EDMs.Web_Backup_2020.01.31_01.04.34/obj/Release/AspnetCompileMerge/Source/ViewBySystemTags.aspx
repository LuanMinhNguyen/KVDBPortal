<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewBySystemTags.aspx.cs" Inherits="EDMs.Web.ViewBySystemTags" EnableViewState="true" %>

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
        
        #ctl00_ContentPlaceHolder2_grdDocument_ctl00_ctl02_ctl03_txtDate_popupButton {
            display: none;
        }
        .rgExpandCol {
            width: 1% !important;
        }

        .rgGroupCol {
            width: 1% !important;
        }
        /*#RAD_SPLITTER_PANE_CONTENT_ctl00_leftPane {
            width: 250px !important;
        }
        #RAD_SPLITTER_PANE_CONTENT_ctl00_topLeftPane {
            width: 250px !important;
        }*/

        .RadAjaxPanel {
            height: 100% !important;
        }

        .rpExpandHandle {
            display: none !important;
        }

        .RadGrid .rgRow td, .RadGrid .rgAltRow td, .RadGrid .rgEditRow td, .RadGrid .rgFooter td, .RadGrid .rgFilterRow td, .RadGrid .rgHeader, .RadGrid .rgResizeCol, .RadGrid .rgGroupHeader td {
            padding-left: 0px !important;
            padding-right: 0px !important;
        }

        /*Hide change page size control*/
        /*div.RadGrid .rgPager .rgAdvPart     
        {     
        display:none;        sssssss
        }*/    

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

        /*fix layout for view by system & tags on server*/
        #ContentPlaceHolder2_grdDocument_ctl00{
			table-layout: auto !important;
		}
		
		#ContentPlaceHolder2_grdDocument_ctl00_Header{
			table-layout: auto !important;
		}

        /*end*/

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
    <telerik:RadPanelBar ID="radPbSearch" runat="server" Width="100%" >
        <Items>
            <telerik:RadPanelItem Text="SEARCH DOCUMENT" runat="server" Expanded="True" >
                <Items>
                    <telerik:RadPanelItem runat="server" Text="Search Documents" ImageUrl="Images/search.gif" NavigateUrl="Search.aspx" />
                </Items>
            </telerik:RadPanelItem>
        </Items>
    </telerik:RadPanelBar>
    <telerik:RadPanelBar ID="RadPanelBar1" runat="server" Width="100%" Visible="False">
        <Items>
            <telerik:RadPanelItem Text="TRANSMITTAL" runat="server" Expanded="True" Width="100%">
                <Items>
                    <telerik:RadPanelItem runat="server" Text="Transmittal" ImageUrl="~/Images/Transmittal.png" NavigateUrl="~/Controls/Document/TransmittalList.aspx"/>
                </Items>
            </telerik:RadPanelItem>
        </Items>
    </telerik:RadPanelBar>        
    <telerik:RadPanelBar ID="rpbObjTree" runat="server"  Width="100%" >
        <Items>
            <telerik:RadPanelItem Text="DOCUMENT PROPERTIES TREE" runat="server" Expanded="True" >
                <Items>
                    <telerik:RadPanelItem Value="ObjTree" runat="server">
                        <ItemTemplate>
                            <telerik:RadComboBox ID="ddlCategory" runat="server" Skin="Windows7" Width="100%"
                                                 AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"/>
                            <telerik:RadTreeView ID="rtvOptionalTypeDetail" runat="server" Width="100%" Height="100%" ShowLineImages="False"
                                OnNodeClick="rtvOptionalTypeDetail_NodeClick"        
                                OnClientNodeExpanded="rtvExplore_OnNodeExpandedCollapsed"
                                OnClientNodeCollapsed="rtvExplore_OnNodeExpandedCollapsed">
                                <DataBindings>
                                    <telerik:RadTreeNodeBinding Expanded="false"></telerik:RadTreeNodeBinding>
                                </DataBindings>
                            </telerik:RadTreeView>
                        </ItemTemplate>
                    </telerik:RadPanelItem>
                </Items>
            </telerik:RadPanelItem>
        </Items>
    </telerik:RadPanelBar>
   <%-- <telerik:RadPanelBar ID="RadPanelBar1" runat="server" Width="200" Visible="False">
        <Items>
            <telerik:RadPanelItem Text="TRANSMITTAL" runat="server" Expanded="True" Width="100%">
                <Items>
                    <telerik:RadPanelItem runat="server" Text="Transmittal" ImageUrl="Images/Transmittal.png" NavigateUrl="~/Controls/Document/TransmittalList.aspx" />
                </Items>
            </telerik:RadPanelItem>
        </Items>
    </telerik:RadPanelBar>
    
    <telerik:RadPanelBar ID="radPbList" runat="server" Width="200" Visible="False"/>
    <telerik:RadPanelBar ID="radPbSystem" runat="server" Width="200" Visible="False"/>--%>
    
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Horizontal">
        <telerik:RadPane ID="RadPane3" runat="server" Height="30px" Scrollable="false" Scrolling="None" Visible="False">
            
            <telerik:RadToolBar ID="CustomerMenu" runat="server" Width="100%" OnClientButtonClicking="OnClientButtonClicking" >
                <Items>
                    <telerik:RadToolBarDropDown runat="server" Text="New" ImageUrl="~/Images/addNew.png">
                        <Buttons>
                            <telerik:RadToolBarButton runat="server" Text="Document" Value="1" ImageUrl="~/Images/addDocument.png"></telerik:RadToolBarButton>
                            <%--<telerik:RadToolBarButton runat="server" Text="Multi documents" Value="2" ImageUrl="~/Images/addmulti.png"></telerik:RadToolBarButton>--%>
                        </Buttons>
                    </telerik:RadToolBarDropDown>
                    <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                    
                    <telerik:RadToolBarDropDown runat="server" Text="Action" >
                        <Buttons>
                            <telerik:RadToolBarButton runat="server" Text="Send mail" Value="3" ImageUrl="~/Images/email.png"/>
                            <telerik:RadToolBarButton runat="server" Text="Download multi documents" Value="4" ImageUrl="~/Images/download.png"/>
                            <telerik:RadToolBarButton runat="server" IsSeparator="true" />
                            <telerik:RadToolBarButton runat="server" Text="Export template data file" Value="6" ImageUrl="~/Images/export.png" />
                            <telerik:RadToolBarButton runat="server" Text="Import data file" Value="5" ImageUrl="~/Images/import.png" />
                        </Buttons>
                    </telerik:RadToolBarDropDown>
                    <telerik:RadToolBarButton runat="server" IsSeparator="true">
                    </telerik:RadToolBarButton>
                    
                    <%--<telerik:RadToolBarButton runat="server" Value="IsFilter">
                        <ItemTemplate>
                            <asp:CheckBox ID="ckbEnableFilter" runat="server" Text="Enable filter list document" Checked="True" 
                                onclick="ShowFilter(this);"
                                />
                        </ItemTemplate>
                    </telerik:RadToolBarButton>--%>

                    <%--<telerik:RadToolBarButton runat="server">OnCheckedChanged="ckbEnableFilter_OnCheckedChanged" 
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
                    <telerik:RadToolBarButton ImageUrl="~/Images/search.gif" Value="search" CommandName="doSearch" />--%>
                </Items>
            </telerik:RadToolBar>
            
        </telerik:RadPane>
        <telerik:RadPane ID="RadPane2" runat="server" Scrollable="false" Scrolling="None">
            <telerik:RadSplitter ID="Radsplitter3" runat="server" Orientation="Horizontal">
                <telerik:RadPane ID="Radpane4" runat="server" Scrolling="None" >
                    
                     <telerik:RadSplitter ID="Radsplitter10" runat="server" Orientation="Vertical">
                         
                        <telerik:RadPane ID="Radpane6" runat="server" Scrolling="None">
                            <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False"  Height="100%" Width="100%"
                                AllowCustomPaging="False" AllowFilteringByColumn="True"
                                AllowSorting="True"
                                OnNeedDataSource="grdDocument_OnNeedDataSource" 
                                OnItemCommand="grdDocument_ItemCommand"
                                OnDetailTableDataBind="grdDocument_DetailTableDataBind"
                                OnColumnCreated="grdDocument_ColumnCreated"
                                PageSize="20" Style="outline: none">
                                <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
                                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                                <MasterTableView AllowMultiColumnSorting="false"
                                    ClientDataKeyNames="ID" DataKeyNames="ID" CommandItemDisplay="Top" EditMode="InPlace" Font-Size="8pt">
                                    <GroupByExpressions>
                                        <telerik:GridGroupByExpression>
                                            <SelectFields>
                                                <telerik:GridGroupByField FieldAlias="System" FieldName="SystemName" FormatString="{0:D}"
                                                    HeaderValueSeparator=": "></telerik:GridGroupByField>
                                            </SelectFields>
                                            <GroupByFields>
                                                <telerik:GridGroupByField  FieldName="SystemName" SortOrder="Ascending" ></telerik:GridGroupByField>
                                            </GroupByFields>
                                        </telerik:GridGroupByExpression>
                                    </GroupByExpressions>
                                    <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="true"/>
                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Tags." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        
                                    <DetailTables>
                                        <telerik:GridTableView DataKeyNames="ID" Name="DocDetail" Width="100%"
                                            AllowPaging="True" PageSize="10">
                                            <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                            <Columns>
                                                <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn" HeaderTooltip="Download document package" >
                                                    <HeaderStyle Width="1%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="1%"/>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnDownloadDocPack" runat="server" 
                                                            Visible='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "AttachFileCount"))  > 0 %>'
                                                            OnClick="btnDownloadDocPack_Click"
                                                            ImageUrl="~/Images/download.png" ToolTip="Download document package"
                                                            Style="cursor: pointer;" AlternateText="Download document"/> 
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            
                                                <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadDefault" HeaderTooltip="Download default document file">
                                                    <HeaderStyle Width="1%"/>
                                                    <ItemStyle HorizontalAlign="Center" Width="1%"/>
                                                    <ItemTemplate>
                                                        <a href='<%# DataBinder.Eval(Container.DataItem, "DefaultDoc") != null ? DataBinder.Eval(Container.DataItem, "DefaultDoc.FilePath") : string.Empty %>' target="_blank">
                                                            <asp:Image ID="Image1" runat="server" ToolTip="Download default document file"
                                                                Visible='<%# DataBinder.Eval(Container.DataItem, "DefaultDoc") != null %>'
                                                                ImageUrl='<%# DataBinder.Eval(Container.DataItem, "DefaultDoc") != null ? DataBinder.Eval(Container.DataItem, "DefaultDoc.ExtensionIcon") : string.Empty %>' 
                                                                Style="cursor: pointer;" AlternateText="Download document"/> 
                                                        </a>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="Index1" HeaderText="Name" 
                                                    DataField="Name" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                    <HeaderStyle Width="10%" HorizontalAlign="Center"/>
                                                    <ItemStyle HorizontalAlign="Left" Width="10%"/>
                                                    <ItemTemplate>
                                                            <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'></asp:Label>
                                                            <asp:Image ID="newicon" runat="server" ImageUrl="~/Images/new.png" 
                                                                Visible='<%# (DateTime.Now - Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "CreatedDate"))).TotalHours < 24 %>' /> 
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:HiddenField ID="Name" runat="server" Value='<%# Eval("Name") %>'/>
                                                    
                                                        <asp:TextBox ID="txtName" runat="server" Width="100%"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            
                                                <telerik:GridBoundColumn DataField="Description" HeaderText="Description" UniqueName="Index2"
                                                    ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="PlantName" HeaderText="Plant" UniqueName="Index7"
                                                    ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                </telerik:GridBoundColumn>
                                            
                                                <telerik:GridBoundColumn DataField="DisciplineName" HeaderText="Discipline" UniqueName="Index9"
                                                    ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                    <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="8%" />
                                                </telerik:GridBoundColumn>
                                                    
                                                <telerik:GridBoundColumn DataField="DocumentTypeName" HeaderText="Document Type" UniqueName="Index10"
                                                    ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                    <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="8%" />
                                                </telerik:GridBoundColumn>
                                                    
                                                <telerik:GridBoundColumn DataField="AssetNo" HeaderText="Asset No" UniqueName="Index25"
                                                    ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </telerik:GridTableView>
                                    </DetailTables>

                                    <Columns>
                                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                                            
                                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn" HeaderTooltip="Download document package">
                                            <HeaderStyle Width="1%" />
                                            <ItemStyle HorizontalAlign="Center" Width="1%"/>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnDownloadPackage" runat="server" 
                                                    OnClick="btnDownload_Click"
                                                    ImageUrl="~/Images/download.png" ToolTip="Download document package"
                                                    Style="cursor: pointer;" AlternateText="Download document"/> 
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DocumentColumn" HeaderTooltip="Document list" Visible="False">
                                            <HeaderStyle Width="1%"/>
                                            <ItemStyle HorizontalAlign="Center" Width="1%"/>
                                            <ItemTemplate>
                                                <a href='javascript:ShowDocList(<%# DataBinder.Eval(Container.DataItem, "ID") %>)' style="text-decoration: none; color:blue">
                                
                                                <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/documents.png" ToolTip="Document list"
                                                        Style="cursor: pointer;" AlternateText="Document list" /> 
                                                </a>

                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                            
                                        <%--3--%>
                                            
                                        <telerik:GridBoundColumn DataField="Name" HeaderText="Tag name" UniqueName="Name"
                                            ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                            <ItemStyle HorizontalAlign="Left" Width="8%" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Description" HeaderText="Description" UniqueName="Description"
                                            ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                            <ItemStyle HorizontalAlign="Left" Width="8%" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="SystemName" HeaderText="System" UniqueName="SystemName"
                                            AllowFiltering="False" Visible="False">
                                            <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                            <ItemStyle HorizontalAlign="Left" Width="8%" />
                                                
                                        </telerik:GridBoundColumn>
                                            
                                        <telerik:GridBoundColumn DataField="Serial" HeaderText="Serial" UniqueName="Serial"
                                            ShowFilterIcon="False" FilterControlWidth="96%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center" Width="6%" />
                                            <ItemStyle HorizontalAlign="Left" Width="6%" />
                                        </telerik:GridBoundColumn>
                                            
                                        <telerik:GridBoundColumn DataField="Model" HeaderText="Model" UniqueName="Model"
                                            ShowFilterIcon="False" FilterControlWidth="96%" 
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center" Width="6%" />
                                            <ItemStyle HorizontalAlign="Left" Width="6%" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="DutyCapacity" HeaderText="Duty/Capacity" UniqueName="DutyCapacity"
                                            ShowFilterIcon="False" FilterControlWidth="96%" 
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center" Width="6%" />
                                            <ItemStyle HorizontalAlign="Left" Width="6%" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="DesTemp" HeaderText="Des. Temp." UniqueName="DesTemp"
                                            ShowFilterIcon="False" FilterControlWidth="96%" 
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center" Width="6%" />
                                            <ItemStyle HorizontalAlign="Left" Width="6%" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="DesPress" HeaderText="Des. Press." UniqueName="DesPress"
                                            ShowFilterIcon="False" FilterControlWidth="96%" 
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center" Width="6%" />
                                            <ItemStyle HorizontalAlign="Left" Width="6%" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="DiffPres" HeaderText="Diff. Pres." UniqueName="DiffPres"
                                            ShowFilterIcon="False" FilterControlWidth="96%" 
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center" Width="6%" />
                                            <ItemStyle HorizontalAlign="Left" Width="6%" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="TechnicalSpec" HeaderText="P&ID No." UniqueName="Vendor"
                                            ShowFilterIcon="False" FilterControlWidth="96%" 
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center" Width="6%" />
                                            <ItemStyle HorizontalAlign="Left" Width="6%" />
                                        </telerik:GridBoundColumn>
                                            
                                        <telerik:GridBoundColumn DataField="TechnicalSpec" HeaderText="Technical spec" UniqueName="TechnicalSpec" Visible="False"
                                            ShowFilterIcon="False" FilterControlWidth="97%" 
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center" Width="6%" />
                                            <ItemStyle HorizontalAlign="Left" Width="6%" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                                    <ClientEvents  OnGridCreated="GetGridObject" />
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
            <telerik:RadMenuItem Text="Attach document files" ImageUrl="~/Images/attach.png" Value="AttachDoc">
            </telerik:RadMenuItem>
            <telerik:RadMenuItem Text="Revision list" ImageUrl="~/Images/revision.png" Value="RevisionHistory">
            </telerik:RadMenuItem>
        </Items>
    </telerik:RadContextMenu>

    <span style="display: none">
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest"  >
            <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
            <AjaxSettings>
                
                <telerik:AjaxSetting AjaxControlID="rtvOptionalTypeDetail">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radPbCategories">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radTreeFolder" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
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
                        <telerik:AjaxUpdatedControl ControlID="rtvOptionalTypeDetail" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="treeCustomerType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radViewSettingBar">
                    <UpdatedControls>
                        <%--<telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>--%>
                        <telerik:AjaxUpdatedControl ControlID="SlidingZone1" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlCategory">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvOptionalTypeDetail" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
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
            
            <telerik:RadWindow ID="RevisionDialog" runat="server" Title="Document list" 
                VisibleStatusbar="false" Height="700" Width="1250" MinHeight="700" MinWidth="1250" MaxHeight="700" MaxWidth="1250" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="SendMail" runat="server" Title="Send mail"
                VisibleStatusbar="false" Height="560" Width="992" MinHeight="560" MinWidth="992" MaxHeight="560" MaxWidth="992" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachDoc" runat="server" Title="Attach document files" OnClientClose="refreshGrid"
                VisibleStatusbar="false" Height="600" Width="500" MinHeight="600" MinWidth="500" MaxHeight="600" MaxWidth="500" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ImportData" runat="server" Title="Upload data file"
                VisibleStatusbar="false" Height="100" Width="420" 
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
        <script src="Scripts/jquery-1.7.1.js"></script>
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
                 if (args.get_eventTarget().indexOf("ExportTo") >= 0
                     || args.get_eventTarget().indexOf("btnDownloadPackage") >= 0
                     || args.get_eventTarget().indexOf("btnDownloadDocPack") >= 0
                     || args.get_eventTarget().indexOf("ajaxCustomer") >= 0)
                 {
                     args.set_enableAjax(false);
                 }
             }

            function onColumnHidden(sender) {
                
                var masterTableView = sender.get_masterTableView().get_element();
                masterTableView.style.tableLayout = "auto";
                //window.setTimeout(function () { masterTableView.style.tableLayout = "auto"; }, 0);
            }

            // Undocked and Docked event slide bar Tree folder
            function OnClientUndocked(sender, args) {
                var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;

                radDocuments.get_masterTableView().showColumn(8);
                radDocuments.get_masterTableView().showColumn(9);
                radDocuments.get_masterTableView().showColumn(10);
                radDocuments.get_masterTableView().showColumn(11);
                
                radDocuments.get_masterTableView().showColumn(12);
                radDocuments.get_masterTableView().showColumn(13);
                radDocuments.get_masterTableView().showColumn(14);
                radDocuments.get_masterTableView().showColumn(15);

                if (selectedFolder != "") {
                    ajaxManager.ajaxRequest("ListAllDocuments");
                }
            }
            
            function OnClientDocked(sender, args) {
                var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;

                radDocuments.get_masterTableView().hideColumn(8);
                radDocuments.get_masterTableView().hideColumn(9);
                radDocuments.get_masterTableView().hideColumn(10);
                radDocuments.get_masterTableView().hideColumn(11);
                
                radDocuments.get_masterTableView().hideColumn(12);
                radDocuments.get_masterTableView().hideColumn(13);
                radDocuments.get_masterTableView().hideColumn(14);
                radDocuments.get_masterTableView().hideColumn(15);
                
                if (selectedFolder != "") {
                    ajaxManager.ajaxRequest("TreeView");
                }
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
                    case "AttachDoc":
                        var owd = $find("<%=AttachDoc.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/UploadDragDrop.aspx?docId=" + docId, "AttachDoc");
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

                Set_Cookie("expandedNodesObjTree", selectedNodes, 30);
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
            }

            function ShowEditForm(id) {
                var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                var categoryId = document.getElementById("<%= lblCategoryId.ClientID %>").value;
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Document/DocumentInfoEditForm.aspx?docId=" + id + "&folId=" + selectedFolder + "&categoryId=" + categoryId, "CustomerDialog");
                
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
            
            function ShowDocList(id) {
                var owd = $find("<%=RevisionDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Document/DocListByTag.aspx?tagId=" + id , "RevisionDialog");
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
                
                if (strText.toLowerCase() == "import data file") {
                    var owd = $find("<%=ImportData.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/Document/ImportData.aspx", "ImportData");
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
                
                if (strText.toLowerCase() == "download multi documents") {
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
                        alert("Please select documents to download");
                    }
                    else {
                        ajaxManager.ajaxRequest("DownloadMulti");
                    }
                }
                
                if (strText.toLowerCase() == "export template data file") {
                    ajaxManager.ajaxRequest("ExportTemplateDataFile");
                }

                //Download multi documents Export template data file
                ////if (strText == "View explorer") {
                ////    window.open("file://WIN-P7KS57HL1HG/DocumentLibrary");
                ////}

                if (strText.toLowerCase() == "document") {
                    var owd = $find("<%=CustomerDialog.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/Document/DocumentInfoEditForm.aspx", "CustomerDialog");

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
                
                if (strText.toLowerCase() == "export excel") {
                    var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                    if (selectedFolder == "") {
                        alert("Please choice one folder to export master list.");
                        return false;
                    }
                    else {
                        ajaxManager.ajaxRequest("ExportMasterList");
                    }
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
