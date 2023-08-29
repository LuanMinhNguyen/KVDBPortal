<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OutgoingTransmittalList.aspx.cs" Inherits="EDMs.Web.Controls.Document.OutgoingTransmittalList" EnableViewState="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .RadAjaxPanel {
            height: 100% !important;
        }

        a.tooltip {
            outline: none;
            text-decoration: none;
        }

            a.tooltip strong {
                line-height: 30px;
            }

            a.tooltip:hover {
                text-decoration: none;
            }

            a.tooltip span {
                z-index: 10;
                display: none;
                padding: 14px 20px;
                margin-top: -30px;
                margin-left: 5px;
                width: 240px;
                line-height: 16px;
            }

            a.tooltip:hover span {
                display: inline;
                position: absolute;
                color: #111;
                border: 1px solid #DCA;
                background: #fffAF0;
            }

        .callout {
            z-index: 20;
            position: absolute;
            top: 30px;
            border: 0;
            left: -12px;
        }

        /*CSS3 extras*/
        a.tooltip span {
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

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdDocumentPanel, #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_divContainerPanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_RadPageView1, #ctl00_ContentPlaceHolder2_RadPageView2,
        #ctl00_ContentPlaceHolder2_RadPageView3, #ctl00_ContentPlaceHolder2_RadPageView4,
        #ctl00_ContentPlaceHolder2_RadPageView5 {
            height: 100% !important;
        }

        #divContainerLeft {
            width: 25%;
            float: left;
            margin: 5px;
            height: 99%;
            border-right: 1px dotted green;
            padding-right: 5px;
        }

        #divContainerRight {
            width: 100%;
            float: right;
            margin-top: 5px;
            height: 99%;
        }

        .dotted {
            border: 1px dotted #000;
            border-style: none none dotted;
            color: #fff;
            background-color: #fff;
        }

        .exampleWrapper {
            width: 100%;
            height: 100%;
            /*background: transparent url(~/Images/background.png) no-repeat top left;*/
            position: relative;
        }

        .tabStrip {
            position: absolute;
            top: 0px;
            left: 0px;
        }

        .multiPage {
            position: absolute;
            top: 30px;
            left: 0px;
            color: white;
            width: 100%;
            height: 100%;
        }

        /*Fix RadMenu and RadWindow z-index issue*/
        .radwindow {
            z-index: 8000 !important;
        }

        .TemplateMenu {
            z-index: 10;
        }

        .RadGrid .rgSelectedRow
        {
            background-image : none !important;
            background-color: darkseagreen !important;
        }
        .rcbReadOnly {
            border-color: #8E8E8E #B8B8B8 #B8B8B8 #ff0000 !important;
            color: #000000 !important;
            font: 12px "segoe ui" !important;
            border-width: 1px !important;
            border-style: solid !important;
            border-left-width: 5px !important;
            padding: 2px 1px 3px !important;
            vertical-align: middle !important;
            margin: 0 !important;
            padding-left: 5px !important;
            padding-right: 5px !important;
        }

        #ctl00_ContentPlaceHolder2_CustomerMenu_i2_btnSearch {
            margin-top: -3px
        }
        #ctl00_ContentPlaceHolder2_grdOutgoingTrans_GridData {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_RadPane1 {
            width: 100% !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Horizontal" Width="100%">
        <telerik:RadPane ID="RadPane3" runat="server" Height="30px" Scrollable="false" Scrolling="None">
            <telerik:RadTabStrip ID="RadTabStrip1" runat="server"  width="100%" MultiPageID="RadMultiPage1"
    SelectedIndex="0">
                <Tabs>
                    <telerik:RadTab ImageUrl="~/Images/incomingTrans.png" Text="Incoming Transmittals">
                    </telerik:RadTab>
                    <telerik:RadTab ImageUrl="~/Images/outgoingTrans.png" Text="Outgoing Transmittals">
                    </telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
            
            <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0">
                <telerik:RadPageView runat="server" Height="100%" ID="RadPageView1">
                    
                </telerik:RadPageView>      
                <telerik:RadPageView runat="server" Height="100%" ID="RadPageView2" Width="100%">
                    <telerik:RadSplitter ID="Radsplitter10" runat="server" Orientation="Horizontal">
                        <telerik:RadPane ID="RadPane1" runat="server" Height="30px" Scrollable="false" Scrolling="None">
                             <telerik:RadToolBar ID="CustomerMenu" runat="server" Width="100%" OnClientButtonClicking="OnClientButtonClicking">
                                <Items>
                                    <telerik:RadToolBarDropDown runat="server" Text="New" ImageUrl="~/Images/addNew.png">
                                        <Buttons>
                                            <telerik:RadToolBarButton runat="server" Text="Transmittal" Value="1" ImageUrl="~/Images/out.png" />
                                        </Buttons>
                                    </telerik:RadToolBarDropDown>
                                    <telerik:RadToolBarButton runat="server" IsSeparator="true" />

                                    <telerik:RadToolBarButton runat="server" Value="ShowAll">
                                        <ItemTemplate>
                                            &nbsp;&nbsp;<img src="../../Images/filter.png" />&nbsp;&nbsp;<b style="color: red">Filter: </b>&nbsp;&nbsp;
                            
                                                    <img src="../../Images/project.png" />Selected project:&nbsp;&nbsp;
                                                    <telerik:RadComboBox ID="ddlProject" runat="server"
                                                        Skin="Windows7" Width="250" AutoPostBack="True"
                                                        OnItemDataBound="ddlProject_ItemDataBound"
                                                        OnSelectedIndexChanged="ddlProject_SelectedIndexChanged" />
                                            &nbsp;&nbsp;|&nbsp;&nbsp;
                                                    <b>Show</b>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="rcbReadOnly" Width="150px" Style="max-width: 150px"
                                                OnSelectedIndexChanged="ddlStatus_OnSelectedIndexChanged" AutoPostBack="True">
                                                <Items>
                                                    <asp:ListItem Value="All" Text="All Transmittal"></asp:ListItem>
                                                    <asp:ListItem Value="Invalid" Text="Invalid Transmittal" style="background-color: red"></asp:ListItem>
                                                    <asp:ListItem Value="Sent" Text="Transmittal Sent" style="background-color: greenyellow"></asp:ListItem>

                                                </Items>
                                            </asp:DropDownList>

                                            &nbsp;&nbsp; | &nbsp;&nbsp;
                                                    <b>Search All Text Field</b>
                                            <asp:TextBox ID="txtSearchAllField" runat="server" Style="width: 200px;" CssClass="defaultTextBox" />
                                            <telerik:RadButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Width="25px" Style="text-align: center">
                                                <Icon PrimaryIconUrl="~/Images/search.png" PrimaryIconLeft="3" PrimaryIconTop="2" PrimaryIconWidth="16" PrimaryIconHeight="16" />
                                            </telerik:RadButton>
                                        </ItemTemplate>
                                    </telerik:RadToolBarButton>

                                </Items>
                            </telerik:RadToolBar>
                        </telerik:RadPane>
                        <telerik:RadPane ID="RadPane4" runat="server" Scrollable="false" Scrolling="None">
                             <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                GridLines="None" Height="100%" AllowFilteringByColumn="True"
                                OnItemDataBound="grdDocument_ItemDataBound"
                                OnDeleteCommand="grdDocument_DeleteCommand"
                                OnItemCreated="grdDocument_ItemCreated"
                                OnNeedDataSource="grdDocument_OnNeedDataSource"
                                PageSize="20" Style="outline: none">
                                <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%" TableLayout="Auto" CssClass="rgMasterTable">
                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Transmittals." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />

                                        <telerik:GridTemplateColumn AllowFiltering="False">
                                            <HeaderStyle Width="30" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Image ID="EditLink" runat="server" ImageUrl="~/Images/edit.png" Style="cursor: pointer;" AlternateText="Edit properties" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Do you want to delete Transmittal?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                                            <HeaderStyle Width="30" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridButtonColumn>

                                        <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="Trans">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ExportTrans("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="imgExcel" runat="server" ImageUrl="~/Images/excelfile1.png" Style="cursor: pointer;" ToolTip="Export Trans Form"/>
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn UniqueName="HasAttachFile" AllowFiltering="False">
                                            <HeaderStyle Width="30" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <a download='<%# DataBinder.Eval(Container.DataItem, "TransmittalNo") %>'
                                                    href='<%# DataBinder.Eval(Container.DataItem, "File") %>' target="_blank">
                                                    <asp:Image ID="GenTrans" runat="server" ImageUrl='~/Images/generate.png'
                                                        Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "HasAttachFile")) %>' Style="cursor: pointer;" />
                                                </a>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TransmittalNo" HeaderText="Trans No"
                                            DataField="TransmittalNo" >
                                            <HeaderStyle Width="150" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TransmittalNo") %>'></asp:Label>
                                                <asp:Image ID="newicon" runat="server" ImageUrl="~/Images/new.png" Visible="False" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="Description" HeaderText="Description" UniqueName="Description" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="200" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn HeaderText="From" UniqueName="FromName"
                                            DataField="OriginatingOrganizationName" ShowFilterIcon="False" FilterControlWidth="97%"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center" Width="250" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%# Eval("OriginatingOrganizationName") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="To" UniqueName="ToName"
                                            DataField="ReceivingOrganizationName" ShowFilterIcon="False" FilterControlWidth="97%"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center" Width="250" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%# Eval("ReceivingOrganizationName") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="ConfidentialityName" HeaderText="Confidentiality" UniqueName="ConfidentialityName" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="200" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn HeaderText="DateIssused" UniqueName="IssuedDate" AllowFiltering="False">
                                            <HeaderStyle HorizontalAlign="Center" Width="100" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <%# Eval("IssuedDate","{0:dd/MM/yyyy}") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="DateReceived" UniqueName="ReceivedDate" AllowFiltering="False">
                                            <HeaderStyle HorizontalAlign="Center" Width="100" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <%# Eval("ReceivedDate","{0:dd/MM/yyyy}") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <%--<telerik:GridBoundColumn DataField="TransmittalStatus" HeaderText="Trans Status" UniqueName="TransmittalStatus" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="200" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>--%>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                                    <Selecting AllowRowSelect="true" />
                                    <ClientEvents OnGridCreated="GetGridObject" />
                                    <ClientEvents OnRowContextMenu="RowContextMenu"></ClientEvents>
                                    <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </telerik:RadPane>

                    </telerik:RadSplitter>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
        </telerik:RadPane>
    </telerik:RadSplitter>
    <telerik:RadContextMenu ID="radMenu" runat="server"
        EnableRoundedCorners="true" EnableShadows="true" OnClientItemClicking="gridMenuClicking">
        <Items>
            <telerik:RadMenuItem Text="Attach documents" ImageUrl="~/Images/attach.png" Value="AttachDocument" />
            <telerik:RadMenuItem Text="Document list" ImageUrl="~/Images/documents.png" Value="DocumentList" />
            <telerik:RadMenuItem Text="Attach transmittal file" ImageUrl="~/Images/import.png" Value="ImportTransFile" />
        </Items>
    </telerik:RadContextMenu>

    <span style="display: none">


        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
            <AjaxSettings>

                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radMenu">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="grdDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContainer" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="grdDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="ddlProject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="ddlStatus">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </span>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Outgoing Transmittal Information"
                VisibleStatusbar="false" Height="450" Width="610"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            <telerik:RadWindow ID="AttachDoc" runat="server" Title="Transmittal - Attach documents"
                VisibleStatusbar="false" Height="560" Width="1100" MinHeight="560" MinWidth="1100" MaxHeight="560" MaxWidth="1100"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" OnClientClose="refreshGrid">
            </telerik:RadWindow>

            <telerik:RadWindow ID="DocList" runat="server" Title="Transmittal - Document List"
                VisibleStatusbar="false" Height="600" Width="1100" MinHeight="600" MinWidth="1100" MaxHeight="600" MaxWidth="1100"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>

            <telerik:RadWindow ID="AttachTransFile" runat="server" Title="Attach transmittal file"
                VisibleStatusbar="false" Height="200" Width="620" OnClientClose="refreshGrid"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>

            <telerik:RadWindow ID="GenerateTransForm" runat="server" Title="Generate Transmittal Form"
                VisibleStatusbar="false" Height="300" Width="620"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
    <asp:HiddenField runat="server" ID="FolderContextMenuAction" />
    <asp:HiddenField runat="server" ID="lblFolderId" />
    <asp:HiddenField runat="server" ID="lblDocId" />
    <asp:HiddenField runat="server" ID="lblCategoryId" />
    <asp:HiddenField runat="server" ID="lblProjectId"/>
    <input type="hidden" id="radGridClickedRowIndex" name="radGridClickedRowIndex" />
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
                var tranId = document.getElementById("<%= lblDocId.ClientID %>").value;

                switch (itemValue) {
                    case "AttachDocument":
                        var owd = $find("<%=AttachDoc.ClientID %>");
                        owd.Show();
                        owd.setUrl("TransmittalAttachDocument.aspx?tranId=" + tranId, "AttachDoc");
                        break;
                    case "DocumentList":
                        var owd = $find("<%=DocList.ClientID %>");
                        owd.Show();
                        owd.setUrl("TransmittalDocumentList.aspx?tranId=" + tranId, "DocList");
                        break;
                    case "ImportTransFile":
                        var owd = $find("<%=AttachTransFile.ClientID %>");
                        owd.Show();
                        owd.setUrl("ImportData.aspx?tranId=" + tranId, "AttachTransFile");
                        break;
                    case "GenerateTransForm":
                        var owd = $find("<%=AttachTransFile.ClientID %>");
                        owd.Show();
                        owd.setUrl("GenerateTransForm.aspx?tranId=" + tranId, "GenerateTransForm");
                        break;
                }
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

            function onRequestStart(sender, args) {
                //alerrgs.get_eventTarget()); 
                if (args.get_eventTarget().indexOf("ExportTo") >= 0 || args.get_eventTarget().indexOf("btnDownloadPackage") >= 0 || args.get_eventTarget().indexOf("ajaxCustomer") >= 0) {
                    args.set_enableAjax(false);
                }
            }

            function ShowEditForm(id) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("OutTransmittalEditForm.aspx?tranId=" + id, "CustomerDialog");
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

            function ExportTrans(objId) {
                ajaxManager.ajaxRequest("Export_" + objId);
            }

            function radPbCategories_OnClientItemClicking(sender, args) {
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


                if (strText.toLowerCase() == "transmittal") {
                    var owd = $find("<%=CustomerDialog.ClientID %>");
                    owd.Show();
                    owd.setUrl("OutTransmittalEditForm.aspx", "CustomerDialog");

                }

                if (strText == "Multi documents") {
                    var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                    if (selectedFolder == "") {
                        alert("Please choice one folder to create new document");
                        return false;
                    }
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
