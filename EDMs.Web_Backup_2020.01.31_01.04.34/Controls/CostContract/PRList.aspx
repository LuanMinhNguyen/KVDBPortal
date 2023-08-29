<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PRList.aspx.cs" Inherits="EDMs.Web.Controls.CostContract.PRList" EnableViewState="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .RadAjaxPanel {
            height: 100% !important;
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
    <telerik:RadPanelBar ID="radPbList" runat="server" Width="100%" Visible="False"/>
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
                            <telerik:RadToolBarButton runat="server" Text="Procurement Requirement" Value="1" ImageUrl="~/Images/shopping.png"/>
                        </Buttons>
                    </telerik:RadToolBarDropDown>
                    
                    <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                    <telerik:RadToolBarButton runat="server" Value="ProjectList">
                        <ItemTemplate>
                            Selected Project: 
                            <asp:DropDownList ID="ddlProject" runat="server" CssClass="min25Percent" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlProject_OnSelectedIndexChanged"/>
                        </ItemTemplate>
                    </telerik:RadToolBarButton>
                    <%--<telerik:RadToolBarButton runat="server" IsSeparator="true">
                    </telerik:RadToolBarButton>--%>

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
                    <telerik:RadToolBarButton ImageUrl="~/~/Images/search.gif" Value="search" CommandName="doSearch" />--%>
                </Items>
            </telerik:RadToolBar>

        </telerik:RadPane>
        <telerik:RadPane ID="RadPane2" runat="server" Scrollable="false" Scrolling="None">
            <telerik:RadSplitter ID="Radsplitter3" runat="server" Orientation="Horizontal">
                <telerik:RadPane ID="Radpane4" runat="server" Scrolling="None" >
                    
                     <telerik:RadSplitter ID="Radsplitter10" runat="server" Orientation="Vertical">
                         <telerik:RadPane ID="Radpane6" runat="server" Scrolling="None">
                                <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                    GridLines="None" Height="100%"
                                    OnItemDataBound="grdDocument_ItemDataBound"
                                    OnDeleteCommand="grdDocument_DeleteCommand" 
                                    OnItemCreated="grdDocument_ItemCreated"
                                    OnNeedDataSource="grdDocument_OnNeedDataSource" 
                                    PageSize="100" Style="outline: none">
                                    <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%" TableLayout="Auto" CssClass="rgMasterTable">
                                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldAlias="_" FieldName="TypeName" FormatString="{0:D}"
                                                        HeaderValueSeparator=" "></telerik:GridGroupByField>
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldName="TypeName" SortOrder="Ascending" ></telerik:GridGroupByField>
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>
                                        </GroupByExpressions>
                                        <ColumnGroups>
                                            <telerik:GridColumnGroup HeaderText="Bidding Package Value(Not include 10% VAT)" Name="BiddingPackageValue"
                                                 HeaderStyle-HorizontalAlign="Center"/>
                                        </ColumnGroups>
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />

                                            <telerik:GridTemplateColumn>
                                                <HeaderStyle Width="3%"  />
                                                <ItemStyle HorizontalAlign="Center" Width="3%"/>
                                                <ItemTemplate>
                                                    <asp:Image ID="EditLink" runat="server" ImageUrl="~/Images/edit.png" Style="cursor: pointer;" AlternateText="Edit properties" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Do you want to delete item?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                                                <HeaderStyle Width="3%" />
                                                 <ItemStyle HorizontalAlign="Center" Width="3%"  />
                                            </telerik:GridButtonColumn>
                                            
                                            <telerik:GridTemplateColumn>
                                                <HeaderStyle Width="3%"  />
                                                <ItemStyle HorizontalAlign="Center" Width="3%"/>
                                                <ItemTemplate>
                                                    <a href='javascript:ShowAttachForm(<%# DataBinder.Eval(Container.DataItem, "ID") %>)' style="text-decoration: none; color:blue">
                                                    <asp:Image ID="attachLink" runat="server" ImageUrl="~/Images/attach.png" Style="cursor: pointer;" AlternateText="Attach Files" />
                                                        <a/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <%--3--%>
                                            <telerik:GridTemplateColumn UniqueName="Number" HeaderText="Number">
                                                <HeaderStyle Width="14%" HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Number") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Description" UniqueName="Description"
                                        AllowFiltering="false">
                                                <HeaderStyle HorizontalAlign="Center" Width="38%" />
                                                <ItemStyle HorizontalAlign="Left"  />
                                                <ItemTemplate>
                                                    <%# !string.IsNullOrEmpty(Eval("Description").ToString()) 
                                                    ? Eval("Description").ToString().Replace("\n", "<br/>")
                                                    : string.Empty%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridBoundColumn DataField="Code" HeaderText="Code" UniqueName="Code">
                                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                <ItemStyle HorizontalAlign="Left"/>
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridTemplateColumn HeaderText="Main Perform" UniqueName="MainOwnerName"
                                        AllowFiltering="false">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Center"  />
                                                <ItemTemplate>
                                                    <%# !string.IsNullOrEmpty(Eval("MainOwnerName").ToString()) 
                                                    ? Eval("MainOwnerName").ToString().Replace("\n", "<br/>")
                                                    : string.Empty%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridBoundColumn DataField="ContractorChoiceTypeName" HeaderText="Contractor Selection Type" UniqueName="ContractorChoiceTypeName">
                                                <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                <ItemStyle HorizontalAlign="Left"/>
                                            </telerik:GridBoundColumn>

                                            <telerik:GridTemplateColumn HeaderText="Procurement Plan" UniqueName="ProcurementPlanValue" ColumnGroupName="BiddingPackageValue"
                                        AllowFiltering="false">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right"  />
                                                <ItemTemplate>
                                                    <%# Eval("ProcurementPlanValue", "{0:$ ###,##0.##}")%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn HeaderText="Procurement Req." UniqueName="ProcurementRequirementValue" ColumnGroupName="BiddingPackageValue"
                                        AllowFiltering="false">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right"  />
                                                <ItemTemplate>
                                                    <%# Eval("ProcurementRequirementValue", "{0:$ ###,##0.##}")%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn HeaderText="USD Exchange" UniqueName="USDExchangeValue" ColumnGroupName="BiddingPackageValue"
                                        AllowFiltering="false">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right"  />
                                                <ItemTemplate>
                                                    <%# Eval("USDExchangeValue", "{0:$ ###,##0.##}")%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                                        <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                                        <Selecting AllowRowSelect="true" />
                                        <ClientEvents  OnGridCreated="GetGridObject" />
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
        

        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radMenu">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument"  LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlProject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument"  LoadingPanelID="RadAjaxLoadingPanel2"/> 
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContainer" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </span>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Procurement Requirement Information"
                VisibleStatusbar="false" Height="650" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachDoc" runat="server" Title="Attach Procurement Requirement files"
                VisibleStatusbar="false" Height="500" Width="700" MinHeight="500" MinWidth="700" MaxHeight="500" MaxWidth="700" 
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

            function ShowEditForm(id) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("PREditForm.aspx?prId=" + id, "CustomerDialog");
            }
            
            function ShowAttachForm(prId) {
                var owd = $find("<%=AttachDoc.ClientID %>");
                owd.Show();
                owd.setUrl("AttachPRDocument.aspx?prId=" + prId, "AttachDoc");
            }

            function refreshGrid(arg) {
                //alert(arg);
                if (!arg) {
                    var masterTable = $find("<%=grdDocument.ClientID%>").get_masterTableView();
                    masterTable.rebind();
                }
                else {
                    var masterTable = $find("<%=grdDocument.ClientID%>").get_masterTableView();
                    masterTable.rebind();
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
                //}


                
                 
                if (strText.toLowerCase() == "procurement requirement") {
                    var owd = $find("<%=CustomerDialog.ClientID %>");
                    owd.Show();
                    owd.setUrl("PREditForm.aspx", "CustomerDialog");
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

<%--Tan.Le Remove here--%>
<%--<uc1:List runat="server" ID="CustomerList"/>--%>
<%-- <div id="EDMsCustomers" runat="server" />--%>
