<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PartList.aspx.cs" Inherits="EDMs.Web.Controls.Material.PartList" EnableViewState="true" %>

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

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdWOLaborPlanPanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdWOLaborPlan_GridData {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdDataPanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdData_GridData {
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

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_ToolBarObject_i0_lblObjectNamePanel {
            display: contents !important;
        }
        </style>

    <div style=" width: 100%; height: 100%; background-color: #f0f0f0 ">
        <telerik:RadSplitter RenderMode="Lightweight" ID="MainSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
            <telerik:RadPane ID="Radpane1" runat="server" Width="100%" Height="40" ShowContentDuringLoad="False">
                <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ToolBarObject" EnableRoundedCorners="true" EnableShadows="true" Skin="MetroTouch" Height="40">
                    <Items>
                        <telerik:RadToolBarButton Text="Left" Group="Align">
                            <ItemTemplate>
                                <span style="color: dodgerblue; font-size: 16px; font-weight: normal; font-family: helvetica, arial, verdana, sans-serif;">Danh mục vật tư</span>
                                
                                <span style="color: black;font-size: 16px;font-weight: normal;font-family: helvetica, arial, verdana, sans-serif;"><asp:Label runat="server" ID="lblWOName"></asp:Label> </span>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
                
                <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ViewToolBar" EnableRoundedCorners="true" EnableShadows="true" Skin="MetroTouch" Height="40" Width="230" dir="rtl" OnClientButtonClicking="ViewToolBar_OnClientButtonClicking">
                    <Items>
                        <telerik:RadToolBarButton  ImageUrl="~/Images/toolbar_split.png" ToolTip="Split View" Value="SplitView"/>
                        <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                        <telerik:RadToolBarButton  ImageUrl="~/Images/clear_24.png" ToolTip="Clear Filter" Value="ClearFilter"/>
                        <telerik:RadToolBarButton runat="server" IsSeparator="true"/>

                        <telerik:RadToolBarButton runat="server" Value="IsFilter">
                            <ItemTemplate>
                                <asp:CheckBox ID="ckbEnableFilter" runat="server" Text="Show Filter Row" AutoPostBack="True"
                                              OnCheckedChanged="ckbEnableFilter_CheckedChange"
                                />
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
            </telerik:RadPane>
            <telerik:RadPane ID="Radpane2" runat="server" Width="100%" ShowContentDuringLoad="False">
                <telerik:RadGrid ID="grdData" runat="server" AllowPaging="True"
                    AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" AllowFilteringByColumn="False"
                    GridLines="None" Height="90%" Skin="Metro"
                    OnNeedDataSource="grdData_OnNeedDataSource"  PageSize="20"
                    >
                    <GroupingSettings CaseSensitive="False"></GroupingSettings>
                    <MasterTableView Width="100%" TableLayout="Auto" CssClass="rgMasterTable" >
                        <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Làm mới dữ liệu" ShowExportToExcelButton="False"/>
                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <Columns>
                            
                            <telerik:GridBoundColumn DataField="VatTu" HeaderText="Vật tư" UniqueName="VatTu" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="DienGiai" HeaderText="Diễn giải" UniqueName="DienGiai" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="250" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="PhanLoai" HeaderText="Phân loại" UniqueName="PhanLoai" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="PhanNhom" HeaderText="Phân nhóm" UniqueName="PhanNhom" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="NhaCungCapThamKhao" HeaderText="Nhà cung cấp" UniqueName="NhaCungCapThamKhao" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="CongCu" HeaderText="Công cụ" UniqueName="CongCu" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="DVT" HeaderText="Đơn vị tính" UniqueName="DVT" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="NgungSuDung" HeaderText="Ngưng sử dụng" UniqueName="NgungSuDung" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="NhaSanXuat" HeaderText="Nhà sản xuất" UniqueName="NhaSanXuat" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="PartNumber" HeaderText="Part Number" UniqueName="PartNumber" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="ThietBi" HeaderText="Thiết bị" UniqueName="ThietBi" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="GiaTrungBinh" HeaderText="Giá trung bình" UniqueName="GiaTrungBinh" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataFormatString="{0:###,###,###,##0.00}">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Right" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="GiaCoSo" HeaderText="Giá cơ sở" UniqueName="GiaCoSo" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataFormatString="{0:###,###,###,##0.00}">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Right" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="GiaMuaCuoi" HeaderText="Giá mua cuối" UniqueName="GiaMuaCuoi" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataFormatString="{0:###,###,###,##0.00}">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Right" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="GiaTieuChuan" HeaderText="Giá tiêu chuẩn" UniqueName="GiaTieuChuan" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataFormatString="{0:###,###,###,##0.00}">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Right" />
                            </telerik:GridBoundColumn>

                            
                            <telerik:GridBoundColumn DataField="MaThueHangHoa" HeaderText="Mã thuế hàng hóa" UniqueName="MaThueHangHoa" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="SoNgayBaoHanh" HeaderText="Số ngày bảo hành" UniqueName="SoNgayBaoHanh" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="KiemTraTruocKhiNhapKho" HeaderText="Kiểm tra trước khi nhập kho" UniqueName="KiemTraTruocKhiNhapKho" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="TheoDoi" HeaderText="Theo dõi" UniqueName="TheoDoi" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
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
                
                <telerik:AjaxSetting AjaxControlID="grdData">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdData" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="txtStartDate">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdData" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="txtEndDate">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdData" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ckbEnableFilter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdData" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ViewToolBar">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdData" LoadingPanelID="RadAjaxLoadingPanel2"/>
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
                    window.location.href = "PartDetail.aspx";
                } else if (strValue == "ClearFilter") {
                    var grid = $find("<%=grdData.ClientID %>");
                    var masterTableView = grid.get_masterTableView();
                    masterTableView.clearFilter("VatTu");
                    masterTableView.clearFilter("DienGiai");
                    masterTableView.clearFilter("PhanLoai");
                    masterTableView.clearFilter("PhanNhom");
                    masterTableView.clearFilter("NhaCungCapThamKhao");
                    masterTableView.clearFilter("CongCu");
                    masterTableView.clearFilter("DVT");
                    masterTableView.clearFilter("NgungSuDung");
                    masterTableView.clearFilter("NhaSanXuat");
                    masterTableView.clearFilter("PartNumber");
                    masterTableView.clearFilter("ThietBi");
                    masterTableView.clearFilter("GiaTrungBinh");
                    masterTableView.clearFilter("GiaCoSo");
                    masterTableView.clearFilter("GiaMuaCuoi");
                    masterTableView.clearFilter("GiaTieuChuan");
                    masterTableView.clearFilter("MaThueHangHoa");
                    masterTableView.clearFilter("SoNgayBaoHanh");
                    masterTableView.clearFilter("KiemTraTruocKhiNhapKho");
                    masterTableView.clearFilter("TheoDoi");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>