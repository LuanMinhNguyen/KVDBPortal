<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssetDetail.aspx.cs" Inherits="EDMs.Web.Controls.Asset.AssetDetail" EnableViewState="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .styleIcon {
            margin-left: -25px;
            margin-top: 3px;
            position: fixed;
        }

        .riTextBox[type="text"] {
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

        .accordion dt a {
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

        #RAD_SPLITTER_PANE_CONTENT_ctl00_ContentPlaceHolder2_Radpane1 {
            display: flex !important;
        }

        #ctl00_ContentPlaceHolder2_txtSearch {
            height: 27px !important;
        }

        #ctl00_ContentPlaceHolder2_grdAsset_GridData {
            height: 63vh !important;
        }

        #ctl00_ContentPlaceHolder2_grdAssetHistory {
            height: 63vh !important;
        }

        #ctl00_ContentPlaceHolder2_grdAssetPMSchedule {
            height: 58vh !important;
        }

        #ctl00_ContentPlaceHolder2_RadSplitter1 {
            height: auto !important
        }
        #ctl00_ContentPlaceHolder2_grdAssetParameters_GridData{
            height: 30vh !important
        }
    </style>

    <div style="width: 100%; height: 100%; background-color: #f0f0f0">
        <telerik:RadSplitter RenderMode="Lightweight" ID="MainSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
            <telerik:RadPane ID="Radpane1" runat="server" Width="100%" Height="40" ShowContentDuringLoad="False" Scrollable="false" Scrolling="None">
                <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ToolBarAsset" EnableRoundedCorners="true" EnableShadows="true" Skin="MetroTouch" Height="40">
                    <Items>
                        <telerik:RadToolBarButton Text="Left" Group="Align" Width="100%">
                            <ItemTemplate>
                                <div style="display: flex">
                                    <span style="color: dodgerblue; font-size: 16px; font-weight: normal; font-family: helvetica, arial, verdana, sans-serif;">Thiết Bị: </span>

                                    <span style="color: black; font-size: 16px; font-weight: normal; font-family: helvetica, arial, verdana, sans-serif;">
                                        <asp:Label runat="server" ID="lblAssetName"></asp:Label>
                                    </span>
                                </div>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>

                    </Items>
                </telerik:RadToolBar>

                <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ViewToolBar" EnableRoundedCorners="true" EnableShadows="true" Skin="MetroTouch" Height="40" Width="105" dir="rtl" OnClientButtonClicking="ViewToolBar_OnClientButtonClicking">
                    <Items>
                        <telerik:RadToolBarButton ImageUrl="~/Images/expand_right.png" ToolTip="Expand Right" Value="ExpandRight" />
                        <telerik:RadToolBarButton ImageUrl="~/Images/toolbar_split.png" ToolTip="Split View" Value="SplitView" />
                        <telerik:RadToolBarButton ImageUrl="~/Images/expand_left.png" ToolTip="Expand Left" Value="ExpandLeft" />
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
                                    ImageUrl="~/Images/search20.png" OnClick="btnSearch_OnClick" />
                            </telerik:RadPane>
                            <telerik:RadPane ID="Radpane4" runat="server" Width="100%">
                                <telerik:RadGrid runat="server" ID="grdAsset"
                                    OnNeedDataSource="grdAsset_NeedDataSource" Skin="MetroTouch" AllowPaging="true"
                                    AutoGenerateColumns="false" OnSelectedIndexChanged="grdAsset_SelectedIndexChanged"
                                    OnItemDataBound="grdAsset_ItemDataBound"
                                    PageSize="300" Width="100%">
                                    <ClientSettings Selecting-AllowRowSelect="true" EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" />
                                    <MasterTableView DataKeyNames="ThietBi" ShowHeader="false">
                                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                        <Columns>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Font-Size="14px" />
                                                <ItemTemplate>
                                                    <div style="width: 350px">
                                                        <span class="assetTitle" runat="server" id="assetFullname" style="display: block"><%# DataBinder.Eval(Container.DataItem, "FullName")%></span>
                                                        <table>
                                                            <tr>
                                                                <td align="right" style="width: 120px"><span class="assetDetail">Tổ chức:&nbsp;</span></td>
                                                                <td>
                                                                    <asp:Label runat="server" ID="lbOrg" class="assetDetail"
                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "ToChuc") %>'>
                                                                    </asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right"><span class="assetDetail">Trạng Thái:&nbsp;</span></td>
                                                                <td>
                                                                    <asp:Label runat="server" ID="lbStatus" class="assetDetail">
                                                                    </asp:Label>
                                                            </tr>
                                                            <tr>
                                                                <td align="right"><span class="assetDetail">Phòng ban/PX:&nbsp;</span></td>
                                                                <td>
                                                                    <asp:Label runat="server" ID="lbPhongBan" class="assetDetail"
                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "PhongBan") %>'>
                                                                    </asp:Label>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <asp:HiddenField runat="server" ID="hfAsset" Value='<%# DataBinder.Eval(Container.DataItem, "ThietBi")%>' />
                                                    <asp:HiddenField runat="server" ID="hfAssetFullName" Value='<%# DataBinder.Eval(Container.DataItem, "FullName")%>' />
                                                    <asp:HiddenField runat="server" ID="hfStatus" Value='<%# DataBinder.Eval(Container.DataItem, "TrangThai") %>' />
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
                        </telerik:RadSplitter>
                    </telerik:RadPane>
                    <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
                    </telerik:RadSplitBar>
                    <telerik:RadPane ID="PanelAssetDetail" runat="server">
                        <telerik:RadTabStrip RenderMode="Lightweight" ID="TabAssetDetail" runat="server" Width="100%" Height="100%" Skin="MetroTouch" MultiPageID="MultiPageAsset" SelectedIndex="0" EnableDragToReorder="true">
                            <Tabs>
                                <telerik:RadTab Text="Chi tiết" />
                                <telerik:RadTab Text="Lịch sử" />
                                <telerik:RadTab Text="Kế hoạch bảo dưỡng" />
                                <telerik:RadTab Text="Phân cấp" />
                                <telerik:RadTab Text="Thông số vận hành" />
                                <%--<telerik:RadTab Text="Tài liệu" />
                                <telerik:RadTab Text="Phân cấp chi tiết" />--%>
                            </Tabs>
                        </telerik:RadTabStrip>
                        <telerik:RadMultiPage runat="server" ID="MultiPageAsset" SelectedIndex="0">
                            <telerik:RadPageView runat="server" ID="RadPageView8">
                                <br />
                                <table>
                                    <tr>
                                        <td colspan="6">
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
                                            <telerik:RadTextBox ID="txtThietBi" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                            <telerik:RadTextBox ID="txtDienGiai" runat="server" Skin="MetroTouch" Width="250" ReadOnly="True" />
                                        </td>

                                        <td align="right" style="width: 150px; padding-right: 2px">Tổ chức:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtToChuc" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Thông số kĩ thuật</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtThongSoKyThuat" runat="server" Skin="MetroTouch" Width="450" TextMode="MultiLine" Rows="3" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px" colspan="2">
                                            <table>
                                                <tr>
                                                    <td align="right" style="width: 150px; padding-right: 2px">Phòng ban/PX</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtPhongBan" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="width: 150px; padding-right: 2px">Trạng thái</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtTrangThai" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" Style="font-weight: bold;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <dl class="accordion">
                                                <dt style="width: 100%;">
                                                    <span>Chi tiết thiết bị</span>
                                                </dt>
                                            </dl>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phân nhóm</td>
                                        <td style="width: 150px; padding-right: 2px">
                                            <telerik:RadTextBox ID="txtPhanNhom" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày BĐ theo dõi</td>
                                        <td align="right" style="width: 150px; padding-right: 2px">
                                            <telerik:RadDatePicker ID="txtNgayBDTheoDoi" runat="server" Skin="MetroTouch" Width="150" Enabled="False">
                                                <DateInput runat="server" DateFormat="dd-MMM-yy" ReadOnly="True" DisplayDateFormat="dd/MM/yyyy" />
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Khu vực</td>
                                        <td align="right" style="width: 150px; padding-right: 2px">
                                            <telerik:RadTextBox ID="txtKhuVuc" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Nhà sản xuất</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNhaSanXuat" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Đơn vị đo</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtDonViDo" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Thiết bị cha</td>
                                        <td align="right" style="width: 150px; padding-right: 2px">
                                            <telerik:RadTextBox ID="txtThietBiCha" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Model</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtModel" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Giá trị</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtGiaTri" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phân loại</td>
                                        <td align="right" style="width: 150px; padding-right: 2px">
                                            <telerik:RadTextBox ID="txtPhanLoai" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Serial Number</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtSerialNumber" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Số đăng ký</td>
                                        <td align="right" style="width: 150px; padding-right: 2px">
                                            <telerik:RadTextBox ID="txtSoDangKy" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Xếp loại (A,B,C)</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtXepLoai" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Người quản lý</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNguoiQuanLy" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                    </tr>
                                </table>
                            </telerik:RadPageView>

                            <%--Lịch sử--%>
                            <telerik:RadPageView runat="server" ID="RadPageView1">
                                <div class="row" style="margin: 0 10px; height: 10%">
                                    <div class="col" style="display: flex; justify-content: space-between">
                                        <div class="col-4" style="display: flex; align-items: center">
                                            <telerik:RadDropDownList runat="server" ID="ddlEventsType" Skin="MetroTouch">
                                                <Items>
                                                    <telerik:DropDownListItem Value="0" Text="All Events" />
                                                </Items>
                                            </telerik:RadDropDownList>
                                        </div>
                                        <div class="col-4"></div>
                                        <div class="col-4" style="display: flex; align-items: center">
                                            <telerik:RadDropDownList runat="server" ID="ddlSearchType" Skin="MetroTouch">
                                                <Items>
                                                    <telerik:DropDownListItem Value="0" Text="Mã phiếu" />
                                                    <telerik:DropDownListItem Value="1" Text="Chi tiết" />
                                                    <telerik:DropDownListItem Value="2" Text="Loại" />
                                                    <telerik:DropDownListItem Value="3" Text="Trạng thái" />
                                                    <telerik:DropDownListItem Value="4" Text="Ngày hoàn thành" />
                                                    <telerik:DropDownListItem Value="5" Text="Đơn vị" />
                                                </Items>
                                            </telerik:RadDropDownList>
                                            <telerik:RadTextBox runat="server" ID="txtSearchText" Skin="MetroTouch"></telerik:RadTextBox>
                                            <telerik:RadLinkButton runat="server" ID="btnSearchHistory" Text="Run" Skin="MetroTouch"></telerik:RadLinkButton>
                                        </div>
                                    </div>
                                </div>
                                <telerik:RadGrid runat="server" ID="grdAssetHistory" OnNeedDataSource="grdAssetHistory_NeedDataSource" Width="100%"
                                    AllowPaging="True"
                                    AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                    GridLines="None" Height="650px" Skin="Metro">
                                    <MasterTableView Width="100%" TableLayout="Auto" CssClass="rgMasterTable" PageSize="30">
                                        <CommandItemSettings ShowAddNewRecordButton="false" RefreshText="Làm mới dữ liệu" ShowExportToExcelButton="False" ShowRefreshButton="true" />
                                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="MaPhieu" HeaderText="Mã phiếu" UniqueName="MaPhieu">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="ChiTiet" HeaderText="Chi tiết" UniqueName="ChiTiet">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="Loai" HeaderText="Loại" UniqueName="LoaiPhieu">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="TrangThai" HeaderText="Trạng thái" UniqueName="TrangThai">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>

                                            <telerik:GridDateTimeColumn DataField="NgayHoanThanh" HeaderText="Ngày hoàn thành" UniqueName="NgayHoanThanh" DataFormatString="{0:dd/MM/yy}">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridDateTimeColumn>

                                            <telerik:GridBoundColumn DataField="DonVi" HeaderText="Đơn vị" UniqueName="DonVi">
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
                            <%--Kế hoạch bảo dưỡng--%>
                            <telerik:RadPageView runat="server" ID="RadPageView2">

                                <div class="row" style="margin: 0 10px; height: 10%">
                                    <div class="col" style="display: flex; justify-content: space-between">
                                        <div class="col-4"></div>
                                        <div class="col-4"></div>
                                        <div class="col-4" style="display: flex; align-items: center">
                                            <telerik:RadDropDownList runat="server" ID="ddlPMScheduleSearchType" Skin="MetroTouch">
                                                <Items>
                                                    <telerik:DropDownListItem Value="0" Text="Quy trình" />
                                                    <telerik:DropDownListItem Value="1" Text="Mô tả" />
                                                    <telerik:DropDownListItem Value="2" Text="Phòng ban/PX" />
                                                    <telerik:DropDownListItem Value="3" Text="Chu kỳ" />
                                                    <telerik:DropDownListItem Value="4" Text="Ngày bắt đầu" />
                                                    <telerik:DropDownListItem Value="5" Text="Tần suất" />
                                                    <telerik:DropDownListItem Value="6" Text="Thời điểm bắt đầu" />
                                                    <telerik:DropDownListItem Value="7" Text="Phiếu công việc" />
                                                    <telerik:DropDownListItem Value="8" Text="Người thực hiện" />
                                                    <telerik:DropDownListItem Value="9" Text="Người giám sát" />
                                                </Items>
                                            </telerik:RadDropDownList>
                                            <telerik:RadTextBox runat="server" ID="txtPPMScheduleSearch" Skin="MetroTouch"></telerik:RadTextBox>
                                            <telerik:RadLinkButton runat="server" ID="RadLinkButton1" Text="Run" Skin="MetroTouch"></telerik:RadLinkButton>
                                        </div>
                                    </div>
                                </div>
                                <telerik:RadGrid ID="grdAssetPMSchedule" runat="server" AllowPaging="true"
                                    AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                    GridLines="None" Height="650px" Skin="Metro"
                                    OnNeedDataSource="grdAssetPMSchedule_OnNeedDataSource">
                                    <MasterTableView Width="100%" TableLayout="Auto" CssClass="rgMasterTable" ShowHeader="true">
                                        <CommandItemSettings ShowAddNewRecordButton="false" RefreshText="Làm mới dữ liệu" ShowExportToExcelButton="False" />
                                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <Columns>

                                            <telerik:GridBoundColumn DataField="QuyTrinh" HeaderText="Quy trình" UniqueName="QuyTrinh">
                                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="MoTa" HeaderText="Mô tả" UniqueName="MoTa">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="PhongBan" HeaderText="Phòng ban/PX" UniqueName="PhongBan">
                                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="ChuKy" HeaderText="Chu kỳ" UniqueName="ChuKy">
                                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </telerik:GridBoundColumn>

                                            <telerik:GridDateTimeColumn DataFormatString="{0:dd/MM/yy}" DataField="NgayBatDau" HeaderText="Ngày bắt đầu" UniqueName="NgayBatDau">
                                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </telerik:GridDateTimeColumn>

                                            <telerik:GridBoundColumn DataField="TanSuat" HeaderText="Tần suất" UniqueName="TanSuat">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>

                                            <telerik:GridDateTimeColumn DataFormatString="{0:dd/MM/yy}" DataField="ThoiDiemBD" HeaderText="Thời điểm bắt đầu" UniqueName="ThoiDiemBD">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridDateTimeColumn>

                                            <telerik:GridBoundColumn DataField="PhieuCongViec" HeaderText="Phiếu công việc" UniqueName="PhieuCongViec">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="NguoiThucHien" HeaderText="Người thực hiện" UniqueName="NguoiThucHien">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="NguoiGiamSat" HeaderText="Người giám sát" UniqueName="NguoiGiamSat">
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
                            <%--Phân cấp--%>
                            <telerik:RadPageView runat="server" ID="RadPageView3">
                                <span style="border-bottom: 1px solid #46A3D3; color: #085B8F; font-size: 1.15em; letter-spacing: 0.1em; line-height: 1.2; margin: 0.5em auto 0.6em; text-align: left; text-decoration: none; display: block; font-weight: bold; font-family: helvetica, arial, verdana, sans-serif; padding-top: 10px">Phân cấp chi tiết
                                </span>
                                <div style="padding-left: 10px">
                                    <telerik:RadTreeView runat="server" ID="RadTreeView1" Skin="Vista" OnNodeDataBound="RadTreeView1_NodeDataBound" RenderMode="Lightweight" Font-Size="Medium">
                                    </telerik:RadTreeView>
                                </div>
                            </telerik:RadPageView>
                            <%--Thông số vận hành--%>
                            <telerik:RadPageView runat="server" ID="RadPageView4">
                                <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Metro">
                                    <telerik:RadPane ID="RadPane5" runat="server" Width="100%" Height="300">
                                        <telerik:RadGrid ID="grdAssetParameters" runat="server" AllowPaging="True"
                                            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                            GridLines="None" Height="100%" Skin="Metro"
                                            OnNeedDataSource="grdAssetParameters_NeedDataSource"
                                            OnSelectedIndexChanged="grdAssetParameters_SelectedIndexChanged">
                                            <ClientSettings Selecting-AllowRowSelect="true" EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" />
                                            <MasterTableView Width="100%" TableLayout="Auto" CssClass="rgMasterTable">
                                                <CommandItemSettings ShowAddNewRecordButton="false" RefreshText="Làm mới dữ liệu" ShowExportToExcelButton="False" />
                                                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <Columns>

                                                    <telerik:GridBoundColumn DataField="DonViDo" HeaderText="Đơn vị đo" UniqueName="DonViDo">
                                                        <HeaderStyle HorizontalAlign="Center" Width="100" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="Mota" HeaderText="Mô tả" UniqueName="Mota">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="TotalUsage" HeaderText="Total Usage" UniqueName="TotalUsage">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="UsageSinceInstall" HeaderText="Usage Since Install" UniqueName="UsageSinceInstall">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="UsageSinceLastWO" HeaderText="Usage Since Last WO" UniqueName="UsageSinceLastWO">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="TypeOfMeter" HeaderText="Type of Meter" UniqueName="TypeOfMeter">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="PhysicalMeter" HeaderText="Physical Meter" UniqueName="PhysicalMeter">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="MeterRollover" HeaderText="Meter Rollover" UniqueName="MeterRollover">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="LastReading" HeaderText="Last Reading" UniqueName="LastReading">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataFormatString="{0:dd/MM/yy HH:mm}" DataField="LastReadingDate"
                                                        HeaderText="Last Reading Date" UniqueName="LastReadingDate">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="EstDailyUsage" HeaderText="Est. Daily Usage" UniqueName="EstDailyUsage">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="AvgDailyUsage" HeaderText="Avg. Daily Usage" UniqueName="AvgDailyUsage">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="ReadingsForCalc" HeaderText="Readings for Calc." UniqueName="ReadingsForCalc">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="DaysSinceLastEntry" HeaderText="Days Since Last Entry" UniqueName="DaysSinceLastEntry">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="UpDownMeter" HeaderText="Up/Down Meter" UniqueName="UpDownMeter">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>

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
                                        <span style="border-bottom: 1px solid #46A3D3; color: #085B8F; font-size: 1.15em; letter-spacing: 0.1em; line-height: 1.2; margin: 0.5em auto 0.6em; text-align: left; text-decoration: none; display: block; font-weight: bold; font-family: helvetica, arial, verdana, sans-serif; padding-top: 10px">Chi tiết</span>
                                        <div style="padding-left: 10px">
                                            <table>
                                                <tr>
                                                    <td align="right">Đơn vị đo</td>
                                                    <td style="display: flex">
                                                        <telerik:RadTextBox ID="txtDonViDoParam" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                                        <telerik:RadTextBox ID="txtMoTaParam" runat="server" Skin="MetroTouch" Width="250" ReadOnly="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">Kiểu nhập</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtTypeOfMeter" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                                    </td>
                                                    <td align="right">Physical Meter</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtPhysicalMeter" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">Tổng sử dụng</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtTotalUsage" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                                    </td>
                                                    <td align="right">MeterRollover</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtMeterRollover" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">Sử dụng từ khi lắp đặt</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtUsageSinceInstall" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                                    </td>
                                                    <td align="right">Giá trị cập nhật cuối</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtLastReading" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">Usage Since Last WO</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtUsageSinceLastWO" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                                    </td>
                                                    <td align="right">Ngày cập nhật cuối</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtLastReadingDate1" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td align="right">Up/Down Meter</td>
                                                    <td>
                                                        <telerik:RadCheckBox runat="server" ID="cbUpDownMeter" Enabled="false"></telerik:RadCheckBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </telerik:RadPane>
                                </telerik:RadSplitter>
                            </telerik:RadPageView>
                            <%--Tài liệu--%>
                            <%--<telerik:RadPageView runat="server" ID="RadPageView5">
                                <telerik:RadTreeView RenderMode="Lightweight" ID="rtvAssetDocument" runat="server" Skin="MetroTouch" OnContextMenuItemClick="rtvObjectDocument_OnContextMenuItemClick">
                                    <ContextMenus>
                                        <telerik:RadTreeViewContextMenu ID="DownloadMenu" runat="server">
                                            <Items>
                                                <telerik:RadMenuItem Value="Download" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Download File" ImageUrl="../../Images/download.png" />
                                            </Items>
                                            <CollapseAnimation Type="none"></CollapseAnimation>
                                        </telerik:RadTreeViewContextMenu>
                                    </ContextMenus>
                                </telerik:RadTreeView>
                            </telerik:RadPageView>--%>
                            <%--Phân cấp chi tiết--%>
                            <%--<telerik:RadPageView runat="server" ID="RadPageView6">
                            </telerik:RadPageView>--%>
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

                <telerik:AjaxSetting AjaxControlID="grdAsset">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lblAssetName" />
                        <telerik:AjaxUpdatedControl ControlID="TabAssetDetail" LoadingPanelID="RadAjaxLoadingPanel2" />
                        <telerik:AjaxUpdatedControl ControlID="MultiPageAsset" LoadingPanelID="RadAjaxLoadingPanel2" />
                        <telerik:AjaxUpdatedControl ControlID="grdAssetPMSchedule" LoadingPanelID="RadAjaxLoadingPanel2" />
                        <telerik:AjaxUpdatedControl ControlID="rtvAssetDocument" LoadingPanelID="RadAjaxLoadingPanel2" />
                        <telerik:AjaxUpdatedControl ControlID="grdAssetHistory" LoadingPanelID="RadAjaxLoadingPanel2" />
                        <telerik:AjaxUpdatedControl ControlID="grdAsset" LoadingPanelID="RadAjaxLoadingPanel2" />
                        <telerik:AjaxUpdatedControl ControlID="RadTreeView1" LoadingPanelID="RadAjaxLoadingPanel2" />
                        <telerik:AjaxUpdatedControl ControlID="grdAssetParameters" LoadingPanelID="RadAjaxLoadingPanel2" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAsset" LoadingPanelID="RadAjaxLoadingPanel2" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="grdAssetParameters">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAssetParameters" LoadingPanelID="RadAjaxLoadingPanel2" />
                        <telerik:AjaxUpdatedControl ControlID="txtDonViDoParam" />
                        <telerik:AjaxUpdatedControl ControlID="txtMoTaParam" />
                        <telerik:AjaxUpdatedControl ControlID="txtTypeOfMeter" />
                        <telerik:AjaxUpdatedControl ControlID="txtPhysicalMeter" />
                        <telerik:AjaxUpdatedControl ControlID="txtTotalUsage" />
                        <telerik:AjaxUpdatedControl ControlID="txtMeterRollover" />
                        <telerik:AjaxUpdatedControl ControlID="txtUsageSinceInstall" />
                        <telerik:AjaxUpdatedControl ControlID="txtLastReading" />
                        <telerik:AjaxUpdatedControl ControlID="txtUsageSinceLastWO" />
                        <telerik:AjaxUpdatedControl ControlID="txtLastReadingDate1" />
                        <telerik:AjaxUpdatedControl ControlID="cbUpDownMeter" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="grdAssetPMSchedule">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAssetPMSchedule" LoadingPanelID="RadAjaxLoadingPanel2" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="grdAssetHistory">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAssetHistory" LoadingPanelID="RadAjaxLoadingPanel2" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

            </AjaxSettings>
        </telerik:RadAjaxManager>

        <asp:HiddenField runat="server" ID="ProjectName" />
        <asp:HiddenField runat="server" ID="ProjectId" />
        <asp:HiddenField runat="server" ID="lblFromDate" />
        <asp:HiddenField runat="server" ID="lblToDate" />

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
                if (args.get_eventTarget().indexOf("ExportTo") >= 0 || args.get_eventTarget().indexOf("btnDownloadPackage") >= 0 || args.get_eventTarget().indexOf("ajaxCustomer") >= 0) {
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
                    window.location.href = "AssetList.aspx";
                }
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
