<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WODetail.aspx.cs" Inherits="EDMs.Web.Controls.WO.WODetail" %>

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

        #ctl00_ContentPlaceHolder2_grdWO_GridData {
            height: 60vh !important;
        }
    </style>

    <div style="width: 100%; height: 100%; background-color: #f0f0f0">
        <telerik:RadSplitter RenderMode="Lightweight" ID="MainSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
            <telerik:RadPane ID="Radpane1" runat="server" Width="100%" Height="40" ShowContentDuringLoad="False" Scrollable="false" Scrolling="None">
                <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ToolBarPart" EnableRoundedCorners="true" EnableShadows="true" Skin="MetroTouch" Height="40">
                    <Items>
                        <telerik:RadToolBarButton Text="Left" Group="Align" Width="100%">
                            <ItemTemplate>
                                <div style="display: flex">
                                    <span style="color: dodgerblue; font-size: 16px; font-weight: normal; font-family: helvetica, arial, verdana, sans-serif;">Phiếu công việc: </span>

                                    <span style="color: black; font-size: 16px; font-weight: normal; font-family: helvetica, arial, verdana, sans-serif;">
                                        <asp:Label runat="server" ID="lblWOName"></asp:Label>
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
                    <telerik:RadPane ID="PanelWOList" runat="server" Width="400" MinWidth="300">
                        <telerik:RadSplitter RenderMode="Lightweight" ID="LeftSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
                            <telerik:RadPane ID="Radpane3" runat="server" Width="100%" Height="27">
                                <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtSearch" runat="server"
                                    EmptyMessage="Search within All store" InvalidStyleDuration="100">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="btnSearch" runat="server" ImageAlign="AbsMiddle" CssClass="styleIcon"
                                    ImageUrl="~/Images/search20.png" OnClick="btnSearch_Click" />
                            </telerik:RadPane>
                            <telerik:RadPane ID="Radpane4" runat="server" Width="100%">
                                <telerik:RadGrid runat="server" ID="grdWO"
                                    OnNeedDataSource="grdWO_NeedDataSource" Skin="MetroTouch" AllowPaging="true"
                                    AutoGenerateColumns="false" OnSelectedIndexChanged="grdWO_SelectedIndexChanged"
                                    PageSize="300" Width="100%">
                                    <ClientSettings Selecting-AllowRowSelect="true" EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" />
                                    <MasterTableView DataKeyNames="PhieuCongViec" ShowHeader="false">
                                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                        <Columns>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Font-Size="14px" />
                                                <ItemTemplate>
                                                    <div style="width: 350px">
                                                        <span class="assetTitle" style="display: block"><%# DataBinder.Eval(Container.DataItem, "FullName")%></span>
                                                        <table>
                                                            <tr>
                                                                <td align="right" style="width: 120px"><span class="assetDetail">Phân nhóm:&nbsp;</span></td>
                                                                <td>
                                                                    <asp:Label runat="server" ID="lbPhongBan" class="assetDetail"
                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "PhongBan") %>'>
                                                                    </asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right"><span class="assetDetail">Phân loại:&nbsp;</span></td>
                                                                <td>
                                                                    <asp:Label runat="server" ID="lbThietBi" class="assetDetail"
                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "ThietBi") %>'>
                                                                    </asp:Label>
                                                            </tr>
                                                            <tr>
                                                                <td align="right"><span class="assetDetail">Phân nhóm:&nbsp;</span></td>
                                                                <td>
                                                                    <asp:Label runat="server" ID="lbTenThietBi" class="assetDetail" 
                                                                        Style="text-overflow: ellipsis; white-space: nowrap; overflow: hidden"
                                                                        Width="220px"
                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "TenThietBi") %>'>
                                                                    </asp:Label>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <asp:HiddenField runat="server" ID="hfWO" Value='<%# DataBinder.Eval(Container.DataItem, "PhieuCongViec")%>' />
                                                    <asp:HiddenField runat="server" ID="hfWOFullname" Value='<%# DataBinder.Eval(Container.DataItem, "FullName")%>' />
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
                    <telerik:RadPane ID="PanelStoreDetail" runat="server">
                        <telerik:RadTabStrip RenderMode="Lightweight" ID="TabWODetail" runat="server" Width="100%" Height="100%" Skin="MetroTouch" MultiPageID="MultiPageWO" SelectedIndex="0" EnableDragToReorder="true">
                            <Tabs>
                                <telerik:RadTab Text="Chi tiết" />
                            </Tabs>
                        </telerik:RadTabStrip>
                        <telerik:RadMultiPage runat="server" ID="MultiPageWO" SelectedIndex="0">
                            <telerik:RadPageView runat="server" ID="RadPageView8">
                                <br />
                                <table width="100%">
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phiếu công việc:</td>
                                        <td style="display: flex">
                                            <telerik:RadTextBox ID="txtPhieuCongViec" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                            <telerik:RadTextBox ID="txtDienGiai" runat="server" Skin="MetroTouch" Width="250" ReadOnly="True" />
                                        </td>

                                        <td align="right" style="width: 150px; padding-right: 2px">Đơn vị:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtDonVi" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Thiết bị:</td>
                                        <td style="display: flex">
                                            <telerik:RadTextBox ID="txtThietBi" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                            <telerik:RadTextBox ID="txtTenThietBi" runat="server" Skin="MetroTouch" Width="250" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phòng ban:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPhongBan" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Loại bảo dưỡng:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtLoaiBaoDuong" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Trạng thái:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtTrangThai" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Người tạo:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNguoiTao" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Mã quy trình:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtMaQuyTrinh" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Độ ưu tiên:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtDoUuTien" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày tạo:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNgayTao" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày kế hoạch gốc:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNgayKeHoachGoc" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Lệnh sản xuất:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtLenhSanXuat" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <dl class="accordion">
                                                <dt style="width: 100%;">
                                                    <span>Biên bản liên quan</span>
                                                </dt>
                                            </dl>
                                        </td>
                                        <td colspan="2" align="right">
                                            <dl class="accordion">
                                                <dt style="width: 97%;">
                                                    <span>Lịch bảo dưỡng</span>
                                                </dt>
                                            </dl>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Báo cáo tình trạng thiết bị(N):</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtBaoCaoTinhTrangThietBi" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">BB Giám định kỹ thuật:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtBBGiamDinhKiThuat" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Người báo cáo:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNguoiBaoCao" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">BB kiểm tra sự cố TB:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtBBKiemTraSuCoTB" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Biện pháp sửa chữa:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtBienPhapSuaChua" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày báo cáo:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNgayBaoCao" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">BB hiện trường:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtBBHienTruong" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">BB nghiệm thu:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtBBNghiemThu" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Người giao việc:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNguoiGiaoViec" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phiếu bắt toa xe:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPhieuBatToaXe" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phiếu trả toa xe:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPhieuTraToaXe" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Người thực hiện:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNguoiThucHien" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phiếu bắt đầu máy:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPhieuBatDauMay" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phiếu trả đầu máy:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPhieuTraDauMay" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày bắt đầu kế hoạch:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNgayBatDauKeHoach" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">BB Giám định B2:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtBBGiamDinhB2" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày kết thúc kế hoạch:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNgayKetThucKeHoach" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày bắt đầu thực tế:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNgayBatDauThucTe" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày kết thúc thực tế:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNgayKetThucThucTe" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ca/kíp:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtCaKip" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                    </tr>
                                </table>
                            </telerik:RadPageView>
                        </telerik:RadMultiPage>
                    </telerik:RadPane>
                </telerik:RadSplitter>
            </telerik:RadPane>
        </telerik:RadSplitter>

        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="MetroTouch">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="ajaxCustomer_AjaxRequest">
            <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>

            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="grdWO">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lblWOName" />
                        <telerik:AjaxUpdatedControl ControlID="TabWODetail" LoadingPanelID="RadAjaxLoadingPanel2" />
                        <telerik:AjaxUpdatedControl ControlID="MultiPageWO" LoadingPanelID="RadAjaxLoadingPanel2" />
                        <telerik:AjaxUpdatedControl ControlID="grdWO" LoadingPanelID="RadAjaxLoadingPanel2" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdWO" LoadingPanelID="RadAjaxLoadingPanel2" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

            </AjaxSettings>
        </telerik:RadAjaxManager>
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
                var pane = splitter.getPaneById("<%= PanelWOList.ClientID %>");

                if (strValue == "ExpandLeft") {
                    var isCollapseSuccess = pane.collapse();
                }
                else if (strValue == "SplitView") {
                    var isExpandSuccess = pane.expand(pane);
                }
                else if (strValue == "ExpandRight") {
                    window.location.href = "WOList.aspx";
                }
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
