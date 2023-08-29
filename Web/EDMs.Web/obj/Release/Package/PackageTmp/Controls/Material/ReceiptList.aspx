<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReceiptList.aspx.cs" Inherits="EDMs.Web.Controls.Material.ReceiptList" EnableViewState="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .styleIcon
        {
            margin-left: -25px;
            margin-top: 3px;
            position: fixed;
        }
        .riTextBox[type="text"]
        {
            padding-right: 23px !important;
        }
        .WOTitle {
            font-size: 1.25em;
            line-height: 1.5em;
            text-overflow: ellipsis;
            height: 1.5em;
            white-space: nowrap;
            margin-left: 5px;
            overflow: hidden;
            font-weight: normal;
            font-family: helvetica, arial, verdana, sans-serif;
        }

        .WODetail {
            font-size: 1.15em;
            line-height: 1.25em;
            text-overflow: ellipsis;
            height: 1.25em;
            white-space: nowrap;
            margin-left: 5px;
            overflow: hidden;
            font-weight: normal;
            font-family: helvetica, arial, verdana, sans-serif;
        }

        .rlbTemplate {
            border-bottom-width: 1px !important;
            border-style: solid;
            border-width: 0px;
            border-color: #d8d8d8;
        }
        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_ToolBarWO_i0_lblWONamePanel {
            display: contents !important;
        }

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdWOLaborPlanPanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdWOLaborPlan_GridData {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdReceiptlinePanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdReceiptline_GridData {
            height: 100% !important;
        }

        .rgHeader {
            border-right: 1px solid #d8d8d8;
            color: white !important;
            font: bold 12px/18px helvetica, arial, verdana, sans-serif;
            outline: 0;
            background-image: none;
            background-color: #50535a;
        }
        .rtsTxt {
            font: bold 12px/18px helvetica, arial, verdana, sans-serif;
        }

        .accordion dt a
        {
            letter-spacing: 0.1em;
            line-height: 1.2;
            margin: 0.5em auto 0.6em;
            padding: 0;
            text-align: left;
            text-decoration: none;
            display: block;
        }

        .accordion dt span {
            color: #085B8F;
            border-bottom: 1px solid #46A3D3;
            font-size: 1.15em;
            letter-spacing: 0.1em;
            line-height: 1.2;
            margin: 0.5em auto 0.6em;
            padding: 0;
            text-align: left;
            text-decoration: none;
            display: block;
            font-weight: bold;
            font-family: helvetica, arial, verdana, sans-serif;
        }

        #ctl00_ContentPlaceHolder2_txtSearch {
            height: 27px !important;
        }

        #RAD_SPLITTER_PANE_CONTENT_ctl00_ContentPlaceHolder2_Radpane1 {
            display: flex !important;
        }
        </style>

    <div style=" width: 100%; height: 100%; background-color: #f0f0f0 ">
        <telerik:RadSplitter RenderMode="Lightweight" ID="MainSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
            <telerik:RadPane ID="Radpane1" runat="server" Width="100%" Height="40" ShowContentDuringLoad="False">
                <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ToolBarWO" EnableRoundedCorners="true" EnableShadows="true" Skin="MetroTouch" Height="40">
                    <Items>
                        <telerik:RadToolBarButton Text="Left" Group="Align">
                            <ItemTemplate>
                                <span style="color: dodgerblue; font-size: 16px; font-weight: normal; font-family: helvetica, arial, verdana, sans-serif;">Nhập hàng
                                </span>
                                
                                <span style="color: black;font-size: 16px;font-weight: normal;font-family: helvetica, arial, verdana, sans-serif;"><asp:Label runat="server" ID="lblWOName"></asp:Label> </span>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        
                    </Items>
                </telerik:RadToolBar>
                
                <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ViewToolBar" EnableRoundedCorners="true" EnableShadows="true" Skin="MetroTouch" Height="40" Width="530" dir="rtl" OnClientButtonClicking="ViewToolBar_OnClientButtonClicking">
                    <Items>
                        <telerik:RadToolBarButton  ImageUrl="~/Images/clear_24.png" ToolTip="Clear Filter" Value="ClearFilter"/>
                        <telerik:RadToolBarButton runat="server" IsSeparator="true"/>

                        <telerik:RadToolBarButton runat="server" Value="IsFilter">
                            <ItemTemplate>
                                <asp:CheckBox ID="ckbEnableFilter" runat="server" Text="Show Filter Row" AutoPostBack="True"
                                              OnCheckedChanged="ckbEnableFilter_CheckedChange"
                                />
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                        <telerik:RadToolBarButton runat="server" Value="IsFilter">
                            <ItemTemplate>
                                 <telerik:RadDropDownList runat="server" ID="ddlKieuGiaoDich" Width="150" OnSelectedIndexChanged="ddlKieuGiaoDich_OnSelectedIndexChanged" Skin="MetroTouch" AutoPostBack="True">
                                     <Items>
                                         <telerik:DropDownListItem runat="server" Text="All" Value="0"/>
                                         <telerik:DropDownListItem runat="server" Text="PO" Value="1"/>
                                         <telerik:DropDownListItem runat="server" Text="Non-PO" Value="2"/>
                                     </Items>
                                 </telerik:RadDropDownList>&nbsp;&nbsp;:Kiểu giao dịch
                                 
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        
                    </Items>
                </telerik:RadToolBar>
            </telerik:RadPane>
            <telerik:RadPane ID="Radpane2" runat="server" Width="100%" ShowContentDuringLoad="False">
                <telerik:RadGrid ID="grdReceiptline" runat="server" AllowPaging="True"
                    AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" AllowFilteringByColumn="False"
                    GridLines="None" Height="90%" Skin="Metro" PageSize="50"
                    OnNeedDataSource="grdReceiptline_OnNeedDataSource" 
                    >
                    <MasterTableView Width="100%" TableLayout="Auto" CssClass="rgMasterTable" >
                        <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Làm mới dữ liệu" ShowExportToExcelButton="False"/>
                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <Columns>
                            
                            <telerik:GridBoundColumn DataField="SoGiaoDich" HeaderText="Số giao dịch" UniqueName="SoGiaoDich" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="SoPhieuNhap" HeaderText="Số Phiếu nhập" UniqueName="SoPhieuNhap" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="kieuGiaoDich" HeaderText="Kiểu giao dịch" UniqueName="kieuGiaoDich" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="MucSo" HeaderText="Mục số" UniqueName="MucSo" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Ngày giao dịch" DataField="NgayGiaoDich" UniqueName="NgayGiaoDich" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("NgayGiaoDich", "{0:dd-MMM-yy}").Replace("01-Jan-01", string.Empty) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridBoundColumn DataField="VatTu" HeaderText="Vật tư" UniqueName="VatTu" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="MoTa" HeaderText="Mô tả" UniqueName="MoTa" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="Soluong" HeaderText="Số lượng" UniqueName="Soluong" DataFormatString="{0:N}" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                <ItemStyle HorizontalAlign="Right" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="DonGia" HeaderText="Đơn giá" UniqueName="DonGia" DataFormatString="{0:N}" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                <ItemStyle HorizontalAlign="Right" />
                            </telerik:GridBoundColumn>
                            
                            
                            <telerik:GridBoundColumn DataField="SoDonHang" HeaderText="Số đơn hàng" UniqueName="SoDonHang" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            
                            <telerik:GridBoundColumn DataField="MucDonHang" HeaderText="Mục đơn hàng" UniqueName="MucDonHang" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="SoYCMuaSam" HeaderText="Số yêu cầu mua sắm" UniqueName="SoYCMuaSam" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="MucYCMuaSam" HeaderText="Mục yêu cầu mua sắm" UniqueName="MucYCMuaSam" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                             <telerik:GridBoundColumn DataField="MucNhap" HeaderText="Mục nhập" UniqueName="MucNhap" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="PhieuYCCap" HeaderText="Phiếu yêu cầu cấp" UniqueName="PhieuYCCap" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="PhieuCV" HeaderText="Phiếu công việc" UniqueName="PhieuCV" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="MucCV" HeaderText="Mục công việc" UniqueName="MucCV" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="Kho" HeaderText="Kho" UniqueName="Kho" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings Selecting-AllowRowSelect="true">
                        <Selecting AllowRowSelect="true" />
                        <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>

        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="MetroTouch">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" >
            <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
            
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="lbWO">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lblWOName"/>
                        <telerik:AjaxUpdatedControl ControlID="TabWODetail" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="MultiPageWO" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lbWO" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlKieuGiaoDich">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdReceiptline" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdReceiptline">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdReceiptline" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ckbEnableFilter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdReceiptline" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ViewToolBar">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdReceiptline" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="ImportData" runat="server" Title="Import Progress planned"
                VisibleStatusbar="false" Height="100" Width="420" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ChartReport" runat="server"
                VisibleStatusbar="false" Height="550" Width="1250" MinHeight="550"  
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
        
    <asp:HiddenField runat="server" ID="ProjectName"/>
    <asp:HiddenField runat="server" ID="ProjectId"/>
    <asp:HiddenField runat="server" ID="lblFromDate"/>
    <asp:HiddenField runat="server" ID="lblToDate"/>

    </div>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="../../Scripts/jquery-1.9.1.js"></script>
        <script src="../../Scripts/bootstrap.min.js"></script>
        <script type="text/javascript">
            
            var ajaxManager;

            function pageLoad() {
                ajaxManager = $find("<%=ajaxCustomer.ClientID %>");
            }

            function onRequestStart(sender, args) {
                //alert(args.get_eventTarget());
                if (args.get_eventTarget().indexOf("ExportTo") >= 0 || args.get_eventTarget().indexOf("btnDownloadPackage") >= 0 || args.get_eventTarget().indexOf("ajaxCustomer") >= 0)
                {
                    args.set_enableAjax(false);
                }
            }

            function ViewToolBar_OnClientButtonClicking(sender, args) {
                var button = args.get_item();
                var strValue = button.get_value();
                if (strValue == "SplitView") {
                    window.location.href = "WOResultDetail.aspx";
                } else if (strValue == "ClearFilter") {
                    var grid = $find("<%=grdReceiptline.ClientID %>");
                    var masterTableView = grid.get_masterTableView();
                    masterTableView.clearFilter("SoGiaoDich");
                    masterTableView.clearFilter("SoPhieuNhap");
                    masterTableView.clearFilter("MucSo");
                    masterTableView.clearFilter("SoDonHang");
                    masterTableView.clearFilter("MucDonHang");
                    masterTableView.clearFilter("SoYCMuaSam");
                    masterTableView.clearFilter("MucYCMuaSam");
                    masterTableView.clearFilter("NgayGiaoDich");
                    masterTableView.clearFilter("VatTu");
                    masterTableView.clearFilter("MoTa");
                    masterTableView.clearFilter("Soluong");
                    masterTableView.clearFilter("DonGia");
                    masterTableView.clearFilter("MucNhap");
                    masterTableView.clearFilter("kieuGiaoDich");
                    masterTableView.clearFilter("PhieuYCCap");
                    masterTableView.clearFilter("PhieuCV");
                    masterTableView.clearFilter("MucCV");
                    masterTableView.clearFilter("Kho");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>