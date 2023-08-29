<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransmittalListByDoc.aspx.cs" Inherits="EDMs.Web.Controls.Document.TransmittalListByDoc" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.7.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        
        function CloseAndRefreshGrid() {
            var oWin = GetRadWindow();
            var parentWindow = oWin.BrowserWindow;
            $(oWin).ready(function () {
                oWin.close();
            });
            parentWindow.refreshGrid();
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including classic dialog
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz as well)

            return oWindow;
        }


    </script>
    <style type="text/css">
        .RadGrid .rgRow td, .RadGrid .rgAltRow td, .RadGrid .rgEditRow td, .RadGrid .rgFooter td, .RadGrid .rgFilterRow td, .RadGrid .rgHeader, .RadGrid .rgResizeCol, .RadGrid .rgGroupHeader td {
            padding-left: 1px !important;
            padding-right: 1px !important;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="content">
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
           <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="True"
                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                GridLines="None" Height="344" AllowFilteringByColumn="false"
                OnNeedDataSource="grdDocument_OnNeedDataSource" 
                PageSize="100" Style="outline: none" Skin="Windows7" >
                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%" TableLayout="Auto" CssClass="rgMasterTable">
                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Transmittals." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                        <telerik:GridTemplateColumn UniqueName="IsGenerate" AllowFiltering="False">
                            <HeaderStyle Width="3%"/>
                            <ItemStyle HorizontalAlign="Center" Width="3%" />
                            <ItemTemplate>
                                <a download='<%# DataBinder.Eval(Container.DataItem, "Name") %>' 
                                    href='<%# DataBinder.Eval(Container.DataItem, "GeneratePath") %>' target="_blank">
                                <asp:Image ID="GenTrans" runat="server" ImageUrl='~/Images/generate.png' 
                                    Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsGenerate")) %>' Style="cursor: pointer;" /> 
                                </a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Name" HeaderText="Name"
                            DataField="Name" ShowFilterIcon="False" FilterControlWidth="99%" 
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                            <HeaderStyle Width="15%" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" Width="15%"/>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'></asp:Label>
                                    <asp:Image ID="newicon" runat="server" ImageUrl="~/Images/new.png" Visible="False" /> 
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="TransmittalNumber" HeaderText="Record No." UniqueName="TransmittalNumber" Display="False"
                            ShowFilterIcon="False" FilterControlWidth="98%" 
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" Width="10%"  />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ProjectName" HeaderText="Project Name" UniqueName="ProjectName"
                            ShowFilterIcon="False" FilterControlWidth="98%" 
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                            <ItemStyle HorizontalAlign="Left"/>
                            
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" UniqueName="IssuseDate" AllowFiltering="False">
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle HorizontalAlign="Center" Width="10%"/>
                            <ItemTemplate>
                                <%# Eval("IssuseDate","{0:dd/MM/yyyy}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="FromList" HeaderText="From" UniqueName="ContractName" AllowFiltering="False">
                            <HeaderStyle HorizontalAlign="Center" Width="25%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ToList" HeaderText="To" UniqueName="ToList" AllowFiltering="False">
                            <HeaderStyle HorizontalAlign="Center" Width="25%"/>
                            <ItemStyle HorizontalAlign="Left" />
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

        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
            

            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
            <Windows>
                
            <telerik:RadWindow ID="AttachDoc" runat="server" Title="Attach document files" 
                VisibleStatusbar="false" Height="500" Width="500" MinHeight="500" MinWidth="500" MaxHeight="500" MaxWidth="500" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
        </div>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                
                function RowDblClick(sender, eventArgs) {
                    sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
                }

                var ajaxManager;

                function pageLoad() {
                ajaxManager = $find("<%=ajaxCustomer.ClientID %>");
            }

                function onRequestStart(sender, args) {
                    if (args.get_eventTarget().indexOf("btnDownloadPackage") >= 0) {
                        args.set_enableAjax(false);
                    }
                }

                function ShowUploadForm(id) {
                    var owd = $find("<%=AttachDoc.ClientID %>");
                    owd.Show();
                    owd.setUrl("UploadDragDrop.aspx?docId=" + id, "AttachDoc");
                    // window.parent.radopen("Controls/Document/DocumentInfoEditForm.aspx?docId=" + id, "DocDialog");
                    // return false;
                }
                
                function ShowInsertForm() {
                    window.radopen("Controls/Customers/CustomerEditForm.aspx", "DocDialog");
                    return false;
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
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
