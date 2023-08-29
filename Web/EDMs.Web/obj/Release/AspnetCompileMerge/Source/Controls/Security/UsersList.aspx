<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="UsersList.aspx.cs" Inherits="EDMs.Web.Controls.Security.UsersList" EnableViewState="true" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .RadAjaxPanel {
            height: 100% !important;
        }
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

        .RadGrid .rgSelectedRow
        {
            background-image : none !important;
            background-color: darkseagreen !important;
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
            <telerik:RadToolBar ID="CustomerMenu" runat="server" Width="100%" OnClientButtonClicking="OnClientButtonClicking">
                <Items>
                    <telerik:RadToolBarDropDown runat="server" Text="New" ImageUrl="~/Images/addNew.png">
                        <Buttons>
                            <telerik:RadToolBarButton runat="server" Text="User" Value="1" ImageUrl="~/Images/user.png"/>
                        </Buttons>
                    </telerik:RadToolBarDropDown>
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
                     <telerik:RadSplitter ID="Radsplitter10" runat="server" Orientation="Vertical">
                         <telerik:RadPane ID="Radpane6" runat="server" Scrolling="None">
                                <telerik:RadGrid ID="grdDocument" AllowFilteringByColumn="True"
                                 runat="server" AllowPaging="True" AutoGenerateColumns="False" CellSpacing="0" CellPadding="0" PageSize="100" Height="100%"
                                GridLines="None" 
                                OnNeedDataSource="grdUsers_OnNeedDataSource"
                                OnItemCommand="grdUsers_ItemCommand"
                                OnItemDataBound="grdUsers_OnItemDataBound">
                                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                                <MasterTableView DataKeyNames="Id" ClientDataKeyNames="Id" Width="100%">
                                    <GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldAlias="Dept." FieldName="Role.FullNameWithLocation" FormatString="{0:D}"
                                                        HeaderValueSeparator=": "></telerik:GridGroupByField>
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldName="Role.FullNameWithLocation" SortOrder="Ascending" ></telerik:GridGroupByField>
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>
                                        </GroupByExpressions>  

                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" 
                                        NextPagesToolTip="Next page" NextPageToolTip="Next page" 
                                        PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Users."
                                        PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="User.Id" HeaderText="Mã người dùng" UniqueName="colUserId" Display="false" />
                                        
                                        <telerik:GridButtonColumn UniqueName="ResetPass" CommandName="ResetPass" ConfirmText="Do you want to Reset password of this user?" ButtonType="ImageButton" ImageUrl="~/Images/reset.png">
                                            <HeaderStyle Width="30" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridButtonColumn>

                                        <telerik:GridTemplateColumn AllowFiltering="False">
                                            <HeaderStyle Width="30" HorizontalAlign="Center"  />
                                            <ItemStyle HorizontalAlign="Center"/>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" Style="cursor: pointer;" 
                                                    Visible='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Id")) != 1%>'
                                                    ID="EditLink" ImageUrl="~/Images/edit.png"/>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn AllowFiltering="False">
                                            <HeaderStyle Width="30" HorizontalAlign="Center"  />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <!--BtnDeleteClick-->
                                                <asp:ImageButton runat="server" Style=" cursor: pointer;" ID="btnDelete" CommandName="DeleteUserCommand" ImageUrl="~/Images/delete.png"
                                                    Visible='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Id")) != 1%>'
                                                    OnClientClick="return confirm('Do you want to delete this user?');"/>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
            
                                        <telerik:GridBoundColumn DataField="Username" HeaderText="User name" UniqueName="colUsername" FilterControlWidth="100%" ShowFilterIcon="False" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                            <HeaderStyle HorizontalAlign="Center"  Width="100" />
                                            <ItemStyle HorizontalAlign="Left"/>
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="DisciplineName" HeaderText="Discipline" UniqueName="DisciplineName" 
                                            FilterControlWidth="100%" ShowFilterIcon="False" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                            <HeaderStyle HorizontalAlign="Center"  Width="100" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
            
                                        <telerik:GridBoundColumn DataField="FullName" HeaderText="Full name" UniqueName="colNameFullName" FilterControlWidth="100%" ShowFilterIcon="False" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center"  Width="180" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="Position" HeaderText="Position" UniqueName="Position" FilterControlWidth="100%" ShowFilterIcon="False" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center"  Width="300" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Role.FullNameWithLocation" HeaderText="Group" UniqueName="colRoleName" FilterControlWidth="100%" ShowFilterIcon="False" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center"  Width="250" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="ProjectName" HeaderText="Facility" UniqueName="ProjectName" FilterControlWidth="100%" ShowFilterIcon="False" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                            <HeaderStyle HorizontalAlign="Center"  Width="150" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="CommentGroupName" HeaderText="Comment Group" UniqueName="CommentGroupName" FilterControlWidth="100%" ShowFilterIcon="False" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                            <HeaderStyle HorizontalAlign="Center"  Width="150" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="Email" HeaderText="Email" UniqueName="Email" 
                                            FilterControlWidth="100%" ShowFilterIcon="False" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center"  Width="200" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="ConfidentialName" HeaderText="Confidentiality" UniqueName="ConfidentialName"  Display="false"
                                            FilterControlWidth="100%" ShowFilterIcon="False" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center"  Width="100" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Dept Manager" UniqueName="IsDeptManager" AllowFiltering="False" Display="False">
                                                <HeaderStyle Width="80" HorizontalAlign="Center" ></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/ok.png" Visible='<%# Eval("IsDeptManager") %>'/>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Manager" UniqueName="IsDeptManager" AllowFiltering="False" Display="False">
                                            <HeaderStyle Width="80" HorizontalAlign="Center" ></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Image ID="imgManager" runat="server" ImageUrl="~/Images/ok.png" Visible='<%# Eval("IsDeptManager") %>'/>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Leader" UniqueName="IsLeader" AllowFiltering="False" Display="true">
                                                <HeaderStyle Width="80" HorizontalAlign="Center" ></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Image ID="imgLeader" runat="server" ImageUrl="~/Images/ok.png" Visible='<%# Eval("IsLeader") %>'/>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Engineer" UniqueName="IsEngineer" AllowFiltering="False" Display="true">
                                                <HeaderStyle Width="80" HorizontalAlign="Center" ></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Image ID="imgEngineer" runat="server" ImageUrl="~/Images/ok.png" Visible='<%# Eval("IsEngineer") %>'/>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Signed Image" Display="false" AllowFiltering="False">
                                                <HeaderStyle Width="120" HorizontalAlign="Center" ></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Image ID="imgSigned" runat="server" ImageUrl='<%# Eval("SignImageUrl") %>' Visible='<%# Eval("SignImageUrl") != null %>' Width="115"/>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Is DC/ Project Director" UniqueName="IsActive" AllowFiltering="False" Display="true">
                                                <HeaderStyle Width="80" HorizontalAlign="Center" ></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Image ID="imgActive" runat="server" ImageUrl="~/Images/ok.png" Visible='<%# Eval("IsDC")== null || (bool) Eval("IsDC")==true %>'/>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <CommandItemStyle Height="25px"></CommandItemStyle>
                                </MasterTableView>
                                <ClientSettings>
                                    <Selecting AllowRowSelect="true"></Selecting>
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True" ScrollHeight="500"></Scrolling>
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

    <span style="display: none">
        

        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="grdDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </span>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="User Information"
                VisibleStatusbar="false" Height="600" Width="610" 
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

            function ShowUserEditForm(id) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("UserEditForm.aspx?userId=" + id, "CustomerDialog");
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

            function radPbCategories_OnClientItemClicking(sender, args)
            {
                var item = args.get_item();
                var categoryId = item.get_value();
                document.getElementById("<%= lblCategoryId.ClientID %>").value = categoryId;
                
            }
            
            function OnClientButtonClicking(sender, args) {
                var button = args.get_item();
                var strText = button.get_text();

                if (strText.toLowerCase() == "user") {
                    var owd = $find("<%=CustomerDialog.ClientID %>");
                    owd.Show();
                    owd.setUrl("UserEditForm.aspx", "CustomerDialog");

                }
                
                if (strText == "Thêm mới") {
                    return ShowInsertForm();
                }
                else {
                    var commandName = args.get_item().get_commandName();
                    if (commandName == "doSearch") {
                        var searchTextBox = sender.findButtonByCommandName("searchText").findControl("txtSearch");
                        if (searchButton.get_value() == "clear") {
                            searchTextBox.set_value("");
                            searchButton.set_imageUrl("~/Images/search.gif");
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
