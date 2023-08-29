<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EAM_AssetManagement.aspx.cs" Inherits="EDMs.Web.EAM_AssetManagement" EnableViewState="true" %>

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
        .assetTitle {
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

        .assetDetail {
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
        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_ToolBarAsset_i0_lblAssetNamePanel {
            display: contents !important;
        }

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdAssetPMSchedulePanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdAssetPMSchedule_GridData {
            height: 100% !important;
        }
        
        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdAssetPartUsagePanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdAssetPartUsage_GridData {
            height: 100% !important;
        }
        
        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdAssetPartAssociatedPanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdAssetPartAssociated_GridData {
            height: 100% !important;
        }
        
        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdAssetDepreciationPanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdAssetDepreciation_GridData {
            height: 100% !important;
        }
        
        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdAssetEventPanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdAssetEvent_GridData {
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

        #RAD_SPLITTER_PANE_CONTENT_ctl00_ContentPlaceHolder2_Radpane1 {
            display: flex !important;
        }
        </style>

    <div style=" width: 100%; height: 100%; background-color: #f0f0f0 ">
        <telerik:RadSplitter RenderMode="Lightweight" ID="MainSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
            <telerik:RadPane ID="Radpane1" runat="server" Width="100%" Height="40" ShowContentDuringLoad="False" Scrollable="false" Scrolling="None">
                <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ToolBarAsset" EnableRoundedCorners="true" EnableShadows="true" Skin="MetroTouch" Height="40">
                    <Items>
                        <telerik:RadToolBarButton Text="Left" Group="Align">
                            <ItemTemplate>
                                <span style="color: dodgerblue; font-size: 16px; font-weight: normal; font-family: helvetica, arial, verdana, sans-serif; ">Thiết Bị: </span>
                                
                                <span style="color: black;font-size: 16px;font-weight: normal;font-family: helvetica, arial, verdana, sans-serif;"><asp:Label runat="server" ID="lblAssetName"></asp:Label> </span>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        
                    </Items>
                </telerik:RadToolBar>
                
                <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ViewToolBar" EnableRoundedCorners="true" EnableShadows="true" Skin="MetroTouch" Height="40" Width="105" dir="rtl" OnClientButtonClicking="ViewToolBar_OnClientButtonClicking">
                    <Items>
                        <telerik:RadToolBarButton  ImageUrl="~/Images/expand_right.png" ToolTip="Expand Right" Value="ExpandRight"/>
                        <telerik:RadToolBarButton  ImageUrl="~/Images/toolbar_split.png" ToolTip="Split View" Value="SplitView"/>
                        <telerik:RadToolBarButton  ImageUrl="~/Images/expand_left.png" ToolTip="Expand Left" Value="ExpandLeft"/>
                        
                        
                    </Items>
                </telerik:RadToolBar>
                
                
            </telerik:RadPane>
            <telerik:RadPane ID="Radpane2" runat="server" Width="100%" ShowContentDuringLoad="False">
                <telerik:RadSplitter RenderMode="Lightweight" ID="SubSplitter" runat="server" Height="100%" Width="100%" Skin="Silk">
                    <telerik:RadPane ID="PanelAssetList" runat="server" Width="400" MinWidth="300">
                        <telerik:RadSplitter RenderMode="Lightweight" ID="LeftSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
                            <telerik:RadPane ID="Radpane3" runat="server" Width="100%" Height="27">
                                <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtSearch" runat="server"
                                                    EmptyMessage="Search within All asset" InvalidStyleDuration="100">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="btnSearch" runat="server" ImageAlign="AbsMiddle" CssClass="styleIcon"
                                           ImageUrl="~/Images/search20.png" OnClick="btnSearch_OnClick"/>
                            </telerik:RadPane>
                            <telerik:RadPane ID="Radpane4" runat="server" Width="100%" >
                                <telerik:RadListBox RenderMode="Lightweight" runat="server" ID="lbAsset" Height="100%" Width="100%" LoadingPanelID="<%# RadAjaxLoadingPanel2.ClientID %>" EnableLoadOnDemand="False" Skin="MetroTouch" OnSelectedIndexChanged="lbAsset_OnSelectedIndexChanged" AutoPostBack="True">
                                    <ItemTemplate>
                                        <span class="assetTitle"><%# DataBinder.Eval(Container, "Text")%></span>
                                        <table>
                                            <tr>
                                                <td align="right" style="width: 120px"><span class="assetDetail">Đơn bị:&nbsp;</span></td>
                                                <td><span class="assetDetail"><%# DataBinder.Eval(Container, "Attributes['DonVi']") %></span></td>
                                            </tr>
                                            <tr>
                                                <td align="right" ><span class="assetDetail">Bộ Phận:&nbsp;</span></td>
                                                <td><span class="assetDetail"><%# DataBinder.Eval(Container, "Attributes['BoPhan']") %></span></td>
                                            </tr>
                                            <tr>
                                                <td align="right"><span class="assetDetail">Trạng Thái:&nbsp;</span></td>
                                                <td><span class="assetDetail"><%# DataBinder.Eval(Container, "Attributes['TrangThai']") %></span></td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadListBox>
                            </telerik:RadPane>
                        </telerik:RadSplitter>
                    </telerik:RadPane>
                    <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
                    </telerik:RadSplitBar>
                    <telerik:RadPane ID="PanelAssetDetail" runat="server">
                        <telerik:RadTabStrip RenderMode="Lightweight" ID="TabAssetDetail" runat="server"  Width="100%" Height="100%" Skin="MetroTouch" MultiPageID="MultiPageAsset" SelectedIndex="0" EnableDragToReorder="true">
                            <Tabs>
                                <telerik:RadTab Text="Chi tiết"/>
                                <telerik:RadTab Text="Phân cấp tài sản"/>
                                <telerik:RadTab Text="Kế hoạch bảo dưỡng"/>
                                <telerik:RadTab Text="Vật tư sử dụng"/>
                                <telerik:RadTab Text="Vật tư cấu thành"/>
                                <telerik:RadTab Text="Tài liệu liên quan"/>
                                <telerik:RadTab Text="Lịch sử"/>
                                <telerik:RadTab Text="Khấu hao"/>
                            </Tabs>
                        </telerik:RadTabStrip>
                        <telerik:RadMultiPage runat="server" ID="MultiPageAsset" SelectedIndex="0">
                        <telerik:RadPageView runat="server" ID="RadPageView8">
                                <br/>
                                <table>
                                    <tr>
                                        <td colspan="5">
                                            <dl class="accordion">
                                                <dt style="width: 100%;">
                                                    <span>Thông tin chung</span>
                                                </dt>
                                            </dl>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Thiết bị:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtThietBi" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                            <telerik:RadTextBox ID="txtDienGiai" runat="server" Skin="MetroTouch" Width="250" ReadOnly="True"/>
                                        </td>
                                        
                                        <td align="right" style="width: 150px; padding-right: 2px">Đơn vị:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtDonVi" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Bộ phận:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtBoPhan" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Trạng thái:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtTrangThai" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" style="font-weight: bold;"/>
                                        </td>
                                    </tr>
                                    
                                <tr>
                                    <td colspan="5">
                                        <dl class="accordion">
                                            <dt style="width: 100%;">
                                                <span>Chi tiết</span>
                                            </dt>
                                        </dl>
                                    </td>
                                </tr>

                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phân loại:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPhanLoai" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày vận hành:</td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtNgayVanHanh" runat="server" Skin="MetroTouch" Width="150" Enabled="False">
                                                <DateInput DateFormat="dd-MMM-yy" ReadOnly="True"/>
                                            </telerik:RadDatePicker>
                                        </td>
                                        
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phân nhóm:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPhanNhom" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phụ trách:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPhuTrach" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Độ quan trọng:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtDoQuanTrong" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngưng sử dụng:</td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbNgungSuDung" runat="server" Skin="MetroTouch" Enabled="False"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="5">
                                            <dl class="accordion">
                                                <dt style="width: 100%;">
                                                    <span>Theo dõi</span>
                                                </dt>
                                            </dl>
                                        </td>
                                    </tr>
                                    
                                <tr>
                                    <td align="right" style="width: 150px; padding-right: 2px">Nhà sản xuất:</td>
                                    <td>
                                        <telerik:RadTextBox ID="txtNhaSanXuat" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                    </td>
                                    <td align="right" style="width: 150px; padding-right: 2px">Serial number:</td>
                                    <td>
                                        <telerik:RadTextBox ID="txtSerialNumber" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                    </td>
                                </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Model:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtModel" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Nhà cung cấp:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNhaCungCap" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày nhận thiết bị:</td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtNgaNhanThietBi" runat="server" Skin="MetroTouch" Width="150" Enabled="False">
                                                <DateInput DateFormat="dd-MMM-yy" ReadOnly="True"/>
                                            </telerik:RadDatePicker>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <dl class="accordion">
                                                <dt style="width: 100%;">
                                                    <span>Thông số kỹ thuật</span>
                                                </dt>
                                            </dl>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Thông số kỹ thuật:</td>
                                        <td colspan="3">
                                            <telerik:RadTextBox ID="txtThongSoKyThuat" runat="server" Skin="MetroTouch" Width="723" TextMode="MultiLine" Rows="3"  ReadOnly="True"/>
                                        </td>
                                    </tr>
                                    
                                    
                                </table>
                            </telerik:RadPageView>

                            <%--Phan cap tai san--%>
                            <telerik:RadPageView runat="server" ID="RadPageView1">
                                <telerik:RadTreeView RenderMode="Lightweight" ID="rtvAssetLvl"  runat="server" Skin="MetroTouch"/>
                            </telerik:RadPageView>
                            <%--Ke hoach bao duong--%>
                            <telerik:RadPageView runat="server" ID="RadPageView2">
                                <telerik:RadSplitter RenderMode="Lightweight" ID="PMScheduleSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
                                    <telerik:RadPane ID="PMSchedulePanel" runat="server" Width="100%" Height="300">
                                        <telerik:RadGrid ID="grdAssetPMSchedule" runat="server" AllowPaging="False"
                                            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                            GridLines="None" Height="100%" Skin="Metro"
                                            OnNeedDataSource="grdAssetPMSchedule_OnNeedDataSource" 
                                            >
                                            <MasterTableView Width="100%" TableLayout="Auto" CssClass="rgMasterTable" >
                                                <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Làm mới dữ liệu" ShowExportToExcelButton="False"/>
                                                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <Columns>
                                                    
                                                    <telerik:GridBoundColumn DataField="QUY_TRINH" HeaderText="Quy trình" UniqueName="QUY_TRINH">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="DIEN_GIAI" HeaderText="Mô tả" UniqueName="DIEN_GIAI">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="BO_PHAN" HeaderText="Bộ phận" UniqueName="BO_PHAN">
                                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="DON_VI" HeaderText="Đơn vị" UniqueName="DON_VI">
                                                        <HeaderStyle HorizontalAlign="Center" Width="100" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="CHU_KY_THUC_HIEN" HeaderText="Chu kỳ thực hiện" UniqueName="CHU_KY_THUC_HIEN">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="DON_VI_CHU_KY" HeaderText="Đơn vị thời gian" UniqueName="DON_VI_CHU_KY">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Ngày bắt đầu" UniqueName="NGAY_BAT_DAU" AllowFiltering="false" >
                                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <%# Eval("NGAY_BAT_DAU", "{0:dd-MMM-yy}") %>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    
                                                    <telerik:GridBoundColumn DataField="PHIEU_CONG_VIEC" HeaderText="Phiếu CV" UniqueName="PHIEU_CONG_VIEC">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    
                                                    
                                                    <telerik:GridBoundColumn DataField="DA_CAP_NHAT" HeaderText="Đã cập nhật" UniqueName="DA_CAP_NHAT">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="KIEU_SINH_PHIEU" HeaderText="Kiểu bảo dưỡng" UniqueName="KIEU_SINH_PHIEU">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="KIEU_CONG_VIEC" HeaderText="Loại công việc" UniqueName="KIEU_CONG_VIEC">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="THOI_LUONG" HeaderText="Thời lượng" UniqueName="THOI_LUONG">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="BO_DEM" HeaderText="Bộ đếm" UniqueName="BO_DEM">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="DON_VI_DO" HeaderText="Đơn vị đo" UniqueName="DON_VI_DO">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
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
                                    <telerik:RadSplitBar ID="RadSplitbar2" runat="server" CollapseMode="Forward">
                                    </telerik:RadSplitBar>
                                    <telerik:RadPane ID="PMScheduleDetail" runat="server" Width="100%">

                                    </telerik:RadPane>
                                </telerik:RadSplitter>
                            </telerik:RadPageView>
                            <%--Vat tu su dung--%>
                            <telerik:RadPageView runat="server" ID="RadPageView3">
                                <telerik:RadGrid ID="grdAssetPartUsage" runat="server" AllowPaging="False"
                                    AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                    GridLines="None" Height="100%" Skin="Metro"
                                    OnNeedDataSource="grdAssetPartUsage_OnNeedDataSource" 
                                    >
                                    <MasterTableView Width="100%" TableLayout="Auto" CssClass="rgMasterTable" >
                                        <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Làm mới dữ liệu" ShowExportToExcelButton="True"/>
                                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Ngày" DataField="NGAY_XUAT" UniqueName="NGAY_XUAT" AllowFiltering="false" >
                                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <%# Eval("NGAY_XUAT", "{0:dd-MMM-yy}") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="VAT_TU" HeaderText="Vật tư" UniqueName="VAT_TU">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="TEN_VAT_TU" HeaderText="Mô tả" UniqueName="TEN_VAT_TU">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="DVT" HeaderText="Đơn vị" UniqueName="DVT">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="DON_GIA" HeaderText="Giá" UniqueName="DON_GIA">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="PHIEU_CV" HeaderText="Phiếu công việc" UniqueName="PHIEU_CV">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="SO_LUONG" HeaderText="Số lượng" UniqueName="SO_LUONG">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="GIA_TRI_CUOI" HeaderText="Giá trị cuối" UniqueName="GIA_TRI_CUOI">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="DON_VI_DO" HeaderText="Đơn vị đo" UniqueName="DON_VI_DO">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="KIEU_CONG_VIEC" HeaderText="Kiểu công việc" UniqueName="KIEU_CONG_VIEC">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings Selecting-AllowRowSelect="true">
                                        <Selecting AllowRowSelect="true" />
                                        <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </telerik:RadPageView>
                            <%--Vat tu cau thanh--%>
                            <telerik:RadPageView runat="server" ID="RadPageView4">
                                 <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
                                    <telerik:RadPane ID="RadPane5" runat="server" Width="100%" Height="300">
                                        <telerik:RadGrid ID="grdAssetPartAssociated" runat="server" AllowPaging="False"
                                            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                            GridLines="None" Height="100%" Skin="Metro"
                                            OnNeedDataSource="grdAssetPartAssociated_OnNeedDataSource" 
                                            >
                                            <MasterTableView Width="100%" TableLayout="Auto" CssClass="rgMasterTable" >
                                                <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Làm mới dữ liệu" ShowExportToExcelButton="False"/>
                                                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <Columns>
                                                    
                                                    <telerik:GridBoundColumn DataField="VAT_TU" HeaderText="Vật tư" UniqueName="VAT_TU">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="TEN_VAT_TU" HeaderText="Mô tả" UniqueName="TEN_VAT_TU">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="DVT" HeaderText="Đơn vị" UniqueName="DVT">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="SO_LUONG" HeaderText="Số lượng" UniqueName="SO_LUONG">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridTemplateColumn HeaderText="Ngày cập nhật" DataField="NGAY_CAP_NHAT" UniqueName="NGAY_CAP_NHAT" AllowFiltering="false" >
                                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <%# Eval("NGAY_CAP_NHAT", "{0:dd-MMM-yy}") %>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    
                                                </Columns>
                                            </MasterTableView>
                                            <ClientSettings Selecting-AllowRowSelect="true">
                                                <Selecting AllowRowSelect="true" />
                                                <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                    </telerik:RadPane>
                                    <telerik:RadSplitBar ID="RadSplitbar3" runat="server" CollapseMode="Forward">
                                    </telerik:RadSplitBar>
                                    <telerik:RadPane ID="RadPane6" runat="server" Width="100%">

                                    </telerik:RadPane>
                                </telerik:RadSplitter>
                            </telerik:RadPageView>
                            <%--Tai lieu lien quan--%>
                            <telerik:RadPageView runat="server" ID="RadPageView5">
                                <telerik:RadTreeView RenderMode="Lightweight" ID="rtvAssetDocument"  runat="server" Skin="MetroTouch"/>
                            </telerik:RadPageView>
                            <%--Ly lich--%>
                            <telerik:RadPageView runat="server" ID="RadPageView6">
                                <telerik:RadGrid ID="grdAssetEvent" runat="server" AllowPaging="False"
                                    AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                    GridLines="None" Height="100%" Skin="Metro"
                                    OnNeedDataSource="grdAssetEvent_OnNeedDataSource" 
                                    >
                                    <MasterTableView Width="100%" TableLayout="Auto" CssClass="rgMasterTable" >
                                        <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Làm mới dữ liệu" ShowExportToExcelButton="True"/>
                                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <Columns>
                                            
                                            <telerik:GridBoundColumn DataField="LICH_SU" HeaderText="Lịch sử" UniqueName="LICH_SU">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="DON_VI" HeaderText="Đơn vị" UniqueName="DON_VI">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="KIEU_LICH_SU" HeaderText="Kiểu lịch sử" UniqueName="KIEU_LICH_SU">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="KIEU" HeaderText="Kiểu" UniqueName="KIEU">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="TRANGTHAI" HeaderText="Trạng thái" UniqueName="TRANGTHAI">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="MO_TA" HeaderText="Mô tả" UniqueName="MO_TA">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridTemplateColumn HeaderText="Ngày hoàn thành" DataField="NGAY_HOAN_THANH" UniqueName="NGAY_HOAN_THANH" AllowFiltering="false" >
                                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <%# Eval("NGAY_HOAN_THANH", "{0:dd-MMM-yy}") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn HeaderText="Ngày bắt đầu KH" DataField="NGAY_BAT_DAU_KH" UniqueName="NGAY_BAT_DAU_KH" AllowFiltering="false" >
                                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <%# Eval("NGAY_BAT_DAU_KH", "{0:dd-MMM-yy}") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings Selecting-AllowRowSelect="true">
                                        <Selecting AllowRowSelect="true" />
                                        <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </telerik:RadPageView>
                            <%--Khau Hao--%>
                            <telerik:RadPageView runat="server" ID="RadPageView7">
                                <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter2" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
                                    <telerik:RadPane ID="RadPane7" runat="server" Width="100%" Height="300">
                                        <telerik:RadGrid ID="grdAssetDepreciation" runat="server" AllowPaging="False"
                                            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                            GridLines="None" Height="100%" Skin="Metro"
                                            OnNeedDataSource="grdAssetDepreciation_OnNeedDataSource" 
                                            >
                                            <MasterTableView Width="100%" TableLayout="Auto" CssClass="rgMasterTable" >
                                                <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Làm mới dữ liệu" ShowExportToExcelButton="False"/>
                                                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <Columns>
                                                    
                                                    <telerik:GridBoundColumn DataField="PHUONG_THUC" HeaderText="Phương thức khấu hao" UniqueName="PHUONG_THUC">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="GIA_TRI_DAU_TU" HeaderText="Giá trị đầu tư" UniqueName="GIA_TRI_DAU_TU" DataFormatString="{0:###,###,###,##0.00}">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="NIEN_HAN_SU_DUNG" HeaderText="Niên hạn sử dụng" UniqueName="NIEN_HAN_SU_DUNG">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="DON_VI_TINH" HeaderText="Đơn vị tính" UniqueName="DON_VI_TINH">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="GIA_TRI_CON_LAI" HeaderText="Giá trị còn lại" UniqueName="GIA_TRI_CON_LAI" DataFormatString="{0:###,###,###,##0.00}">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="KIEU_KHAU_HAO" HeaderText="Kiểu khấu hao" UniqueName="KIEU_KHAU_HAO">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
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
                                    <telerik:RadSplitBar ID="RadSplitbar4" runat="server" CollapseMode="Forward">
                                    </telerik:RadSplitBar>
                                    <telerik:RadPane ID="RadPane8" runat="server" Width="100%">

                                    </telerik:RadPane>
                                </telerik:RadSplitter>
                            </telerik:RadPageView>
                        </telerik:RadMultiPage>
                    </telerik:RadPane>
                </telerik:RadSplitter>
            </telerik:RadPane>
        </telerik:RadSplitter>

        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="MetroTouch">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
            
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="lbAsset">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lblAssetName"/>
                        <telerik:AjaxUpdatedControl ControlID="TabAssetDetail"/>
                        <telerik:AjaxUpdatedControl ControlID="MultiPageAsset"/>
                        <telerik:AjaxUpdatedControl ControlID="rtvAssetLvl" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdAssetPMSchedule" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdAssetPartUsage" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdAssetPartAssociated" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="rtvAssetDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdAssetDepreciation" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdAssetEvent" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="lbAsset" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lbAsset" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdAssetPMSchedule">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAssetPMSchedule" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdAssetPartUsage">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAssetPartUsage" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdAssetPartAssociated">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAssetPartAssociated" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdAssetDepreciation">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAssetDepreciation" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdAssetEvent">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAssetEvent" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnRefreshChart">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ColumnChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="Gauge1" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="Gauge2" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="Gauge3" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="Gauge4" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="Gauge5" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="lbl1"/>
                        <telerik:AjaxUpdatedControl ControlID="lbl2"/>
                        <telerik:AjaxUpdatedControl ControlID="lbl3"/>
                        <telerik:AjaxUpdatedControl ControlID="lbl4"/>
                        <telerik:AjaxUpdatedControl ControlID="lbl5"/>
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
        <script src="Scripts/jquery-1.9.1.js"></script>
        <script src="Scripts/bootstrap.min.js"></script>
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

                var splitter = $find("<%= SubSplitter.ClientID %>");
                var pane = splitter.getPaneById("<%= PanelAssetList.ClientID %>");

                if (strValue == "ExpandLeft") {
                    var isCollapseSuccess = pane.collapse();
                }
                else if (strValue == "SplitView") {
                    var isExpandSuccess = pane.expand(pane);
                }
                else if (strValue == "ExpandRight") {
                    window.location.href = "EAM_AssetManagementList.aspx";
                }
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>