<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MRDetailHistory.aspx.cs" Inherits="EDMs.Web.Controls.WMS.MRDetailHistory" %>

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
            <telerik:RadGrid ID="grdMaterials" runat="server" AllowPaging="False"
                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                GridLines="None" Skin="Windows7" Height="445" AllowFilteringByColumn="False"
                OnItemDataBound="grdMaterials_ItemDataBound" 
                OnNeedDataSource="grdMaterials_OnNeedDataSource" 
                PageSize="100" Style="outline: none; overflow: hidden !important;" Width="100%">
                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" >
                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Contents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Req ROB (6)" Name="ReqROB" HeaderStyle-HorizontalAlign="Center"/>
                        <telerik:GridColumnGroup HeaderText="Quantity Remark (9)" Name="QuantityRemark" HeaderStyle-HorizontalAlign="Center"/>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                        <telerik:GridBoundColumn DataField="IsCancel" UniqueName="IsCancel" Display="False" />

                        <telerik:GridTemplateColumn HeaderText="No." Groupable="False" AllowFiltering="false" Display="False">
                            <HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblSoTT" runat="server" Text='<%# grdMaterials.CurrentPageIndex * grdMaterials.PageSize + grdMaterials.Items.Count+1 %>'>
                                </asp:Label>
                                      
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                                        
                        <telerik:GridTemplateColumn HeaderText="Status" UniqueName="Status">
                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Convert.ToBoolean(Eval("IsCancel"))
                                                ? "Canceled"
                                                : "Open"%>'>
                                </asp:Label>
                                      
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridBoundColumn DataField="CreatedByName" HeaderText="Updated By" UniqueName="CreatedByName">
                            <HeaderStyle HorizontalAlign="Center" Width="200" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>

                        <telerik:GridDateTimeColumn HeaderText="Updated Date" UniqueName="CreatedDate" DataField="CreatedDate"
                                DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                            <ItemStyle HorizontalAlign="Center"/>
                        </telerik:GridDateTimeColumn>

                        <telerik:GridBoundColumn DataField="ReqROBMin" HeaderText="Min" UniqueName="ReqROBMin" ColumnGroupName="ReqROB">
                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                                        
                        <telerik:GridBoundColumn DataField="ReqROBMax" HeaderText="Max" UniqueName="ReqROBMax" ColumnGroupName="ReqROB">
                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                                        
                        <telerik:GridBoundColumn DataField="ROB" HeaderText="ROB" UniqueName="ROB">
                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                                        
                        <telerik:GridBoundColumn DataField="QtyReq" HeaderText="Quantity Required" UniqueName="QtyReq">
                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                                        
                        <telerik:GridBoundColumn DataField="QtyRemarkUseForJob" HeaderText="Use for job" UniqueName="QtyRemarkUseForJob" ColumnGroupName="QuantityRemark">
                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                                        
                        <telerik:GridBoundColumn DataField="QtyRemarkForSpare" HeaderText="For spares" UniqueName="QtyRemarkForSpare" ColumnGroupName="QuantityRemark">
                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                                        
                        <telerik:GridBoundColumn DataField="Units" HeaderText="Units" UniqueName="Units">
                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                                        
                        <telerik:GridBoundColumn DataField="SFICode" HeaderText="SFI Code" UniqueName="SFICode">
                            <HeaderStyle HorizontalAlign="Center" Width="160" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                                        
                        <telerik:GridBoundColumn DataField="Description" HeaderText="Material Name & Specification Descriptions " UniqueName="Description">
                            <HeaderStyle HorizontalAlign="Center" Width="300" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                                        
                        <telerik:GridBoundColumn DataField="MakerName" HeaderText="Maker Name/ Maker Refer" UniqueName="MakerName">
                            <HeaderStyle HorizontalAlign="Center" Width="100" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                                        
                        <telerik:GridBoundColumn DataField="CertificateRequired" HeaderText="Certificate Required (Y/N)" UniqueName="CertificateRequired">
                            <HeaderStyle HorizontalAlign="Center" Width="100" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                                        
                        <telerik:GridBoundColumn DataField="Alternative" HeaderText="Alternative (Y/N)" UniqueName="Alternative">
                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                                        
                        <telerik:GridBoundColumn DataField="NormalUsingFrequency" HeaderText="Normal Using Frequency" UniqueName="NormalUsingFrequency">
                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                                        
                        <telerik:GridBoundColumn DataField="Remarks" HeaderText="Remarks" UniqueName="Remarks">
                            <HeaderStyle HorizontalAlign="Center" Width="200" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>

                        
                    
                    </Columns>
                </MasterTableView>
                <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                    <Scrolling AllowScroll="True" SaveScrollPosition="True" UseStaticHeaders="True" />
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
