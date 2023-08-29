<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrackingSailRevisionHistory.aspx.cs" Inherits="EDMs.Web.Controls.WMS.TrackingSailRevisionHistory" %>

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

        /*Hide change page size control*/
        div.RadGrid .rgPager .rgAdvPart     
        {     
        display:none;        
        }    
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="content">
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <telerik:RadGrid AllowCustomPaging="False" AllowPaging="False" AllowSorting="True" 
                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                ID="grdDocument" Skin="Windows7" Height="600"
                OnNeedDataSource="grdDocument_OnNeedDataSource" 
                OnDetailTableDataBind="grdDocument_DetailTableDataBind"
                PageSize="100" runat="server"  Width="100%">
                <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                <MasterTableView AllowMultiColumnSorting="false" 
                    ClientDataKeyNames="ID" DataKeyNames="ID" Font-Size="8pt">
                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="Initial Risk" Name="InitRisk"
                                    HeaderStyle-HorizontalAlign="Center"/>
                            <telerik:GridColumnGroup HeaderText="Action Plan" Name="ActionPlan"
                                    HeaderStyle-HorizontalAlign="Center"/>
                        </ColumnGroups>
                                    
                        <DetailTables>
                            <telerik:GridTableView DataKeyNames="ID" Name="DocDetail" Width="900px"
                                AllowPaging="True" PageSize="10">
                                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                <Columns>
                                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                                    <HeaderStyle Width="3%" />
                                    <ItemStyle HorizontalAlign="Center" Width="3%"/>
                                    <ItemTemplate>
                                        <a href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' 
                                            download='<%# DataBinder.Eval(Container.DataItem, "FileName") %>' target="_blank">
                                            <asp:Image ID="Image1" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ExtensionIcon") %>'
                                                Style="cursor: pointer;" ToolTip="Download document" /> 
                                        </a>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                            
                                <telerik:GridBoundColumn DataField="FileName" HeaderText="File name" UniqueName="FileName">
                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left"/>
                                </telerik:GridBoundColumn>
                                        
                                <telerik:GridBoundColumn DataField="Description" HeaderText="Description" UniqueName="Description">
                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="CreatedByName" HeaderText="Upload by" UniqueName="CreatedByName">
                                    <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Upload time" UniqueName="CreatedDate"
                                    DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="FileSize" HeaderText="File size(Kb)" UniqueName="FileSize" DataFormatString="{0:0,0.00}">
                                    <HeaderStyle HorizontalAlign="Center" Width="13%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                </Columns>
                            </telerik:GridTableView>
                        </DetailTables>

                        <Columns>
                                <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                                <telerik:GridTemplateColumn HeaderText="No." Groupable="False">
                                    <HeaderStyle HorizontalAlign="Center" Width="2%" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="2%"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSoTT" runat="server" Text='<%# grdDocument.CurrentPageIndex * grdDocument.PageSize + grdDocument.Items.Count+1 %>'>
                                        </asp:Label>
                                      
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="CreatedByName" HeaderText="Updated By">
                                    <HeaderStyle Width="150" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"/>
                                    <ItemTemplate>
                                        <%# Eval("CreatedByName")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                
                            <telerik:GridDateTimeColumn HeaderText="Updated Date" UniqueName="CreatedDate" DataField="CreatedDate"
                                        DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </telerik:GridDateTimeColumn>
                                            
                                <telerik:GridTemplateColumn HeaderText="Completed" UniqueName="IsComplete" AllowFiltering="False">
                                    <HeaderStyle Width="80" HorizontalAlign="Center" ></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/ok.png" Visible='<%# Eval("IsComplete") %>'/>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                                <telerik:GridDateTimeColumn UniqueName="DateRaised" HeaderText="Date" DataField="DateRaised"
                                        DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </telerik:GridDateTimeColumn>
                                            

                                <telerik:GridBoundColumn DataField="SourceName" HeaderText="Source" UniqueName="SourceName">
                                    <HeaderStyle HorizontalAlign="Center" Width="250" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </telerik:GridBoundColumn>
                                            
                                             
                                            
                                <telerik:GridBoundColumn DataField="NameObserver" HeaderText="Name Observer" UniqueName="NameObserver">
                                    <HeaderStyle HorizontalAlign="Center" Width="250" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </telerik:GridBoundColumn>
                                            
                                <telerik:GridBoundColumn DataField="Location" HeaderText="Location" UniqueName="Location">
                                    <HeaderStyle HorizontalAlign="Center" Width="250" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </telerik:GridBoundColumn>

                                <telerik:GridTemplateColumn  DataField="Description" HeaderText="Description" UniqueName="Description" 
                                    AllowFiltering="false">
                                    <HeaderStyle HorizontalAlign="Center" Width="300" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                    <ItemTemplate>
                                        <%# Eval("Description").ToString().Replace(Environment.NewLine, "<br/>") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn  DataField="Action" HeaderText="Immediate Action Taken" UniqueName="Action" 
                                    AllowFiltering="false">
                                    <HeaderStyle HorizontalAlign="Center" Width="300" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                    <ItemTemplate>
                                        <%# Eval("Action").ToString().Replace(Environment.NewLine, "<br/>") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                            
                                <telerik:GridTemplateColumn  DataField="ProposedAction" HeaderText="Proposed further action and close out" UniqueName="ProposedAction" 
                                    AllowFiltering="false">
                                    <HeaderStyle HorizontalAlign="Center" Width="300" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                    <ItemTemplate>
                                        <%# Eval("ProposedAction").ToString().Replace(Environment.NewLine, "<br/>") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                            
                                <telerik:GridBoundColumn DataField="Priority" HeaderText="Priority" UniqueName="Priority">
                                    <HeaderStyle HorizontalAlign="Center" Width="150" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </telerik:GridBoundColumn>
                                            
                                            
                                <telerik:GridDateTimeColumn UniqueName="TargetClose" HeaderText="Target to close out" DataField="TargetClose"
                                        DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </telerik:GridDateTimeColumn>

                                <telerik:GridTemplateColumn  DataField="ActionTakeClose" HeaderText="Action taken to close out" UniqueName="ActionTakeClose" 
                                    AllowFiltering="false">
                                    <HeaderStyle HorizontalAlign="Center" Width="300" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                    <ItemTemplate>
                                        <%# Eval("ActionTakeClose").ToString().Replace(Environment.NewLine, "<br/>") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                            
                                    <telerik:GridDateTimeColumn UniqueName="ClosedDate" HeaderText="Status/ Closed  Date" DataField="ClosedDate"
                                        DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </telerik:GridDateTimeColumn>
                                            
                                            
                                <telerik:GridBoundColumn DataField="HOCTrackingNo" HeaderText="HOC Tracking No" UniqueName="HOCTrackingNo">
                                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </telerik:GridBoundColumn>
                                            
                                <telerik:GridBoundColumn DataField="MSRStatus" HeaderText="MSR No." UniqueName="MSRStatus">
                                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </telerik:GridBoundColumn>
                                            
                                <telerik:GridBoundColumn DataField="WRNo" HeaderText="WR No." UniqueName="WRNo">
                                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </telerik:GridBoundColumn>
                                            
                                <telerik:GridBoundColumn DataField="MOCNo" HeaderText="MOC No." UniqueName="MOCNo">
                                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </telerik:GridBoundColumn>
                                            
                                <telerik:GridBoundColumn DataField="ECRNo" HeaderText="ECR No." UniqueName="ECRNo">
                                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                    <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                    </ClientSettings>
            </telerik:RadGrid>
            
            <telerik:GridDropDownListColumnEditor ID="ddlRevision" runat="server"
                    DropDownStyle-Width="110px">
            </telerik:GridDropDownListColumnEditor>

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
