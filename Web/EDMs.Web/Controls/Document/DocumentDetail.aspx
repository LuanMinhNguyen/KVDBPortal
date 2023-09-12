<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DocumentDetail.aspx.cs" Inherits="EDMs.Web.Controls.Document.DocumentDetail" %>

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

        #ctl00_ContentPlaceHolder2_grdDoc_GridData {
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
                                    <span style="color: dodgerblue; font-size: 16px; font-weight: normal; font-family: helvetica, arial, verdana, sans-serif;">Tài liệu: </span>

                                    <span style="color: black; font-size: 16px; font-weight: normal; font-family: helvetica, arial, verdana, sans-serif;">
                                        <asp:Label runat="server" ID="lblDocName"></asp:Label>
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
                    <telerik:RadPane ID="PanelDocList" runat="server" Width="400" MinWidth="300">
                        <telerik:RadSplitter RenderMode="Lightweight" ID="LeftSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
                            <telerik:RadPane ID="Radpane3" runat="server" Width="100%" Height="27">
                                <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtSearch" runat="server"
                                    EmptyMessage="Search within All store" InvalidStyleDuration="100">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="btnSearch" runat="server" ImageAlign="AbsMiddle" CssClass="styleIcon"
                                    ImageUrl="~/Images/search20.png" OnClick="btnSearch_Click" />
                            </telerik:RadPane>
                            <telerik:RadPane ID="Radpane4" runat="server" Width="100%">
                                <telerik:RadGrid runat="server" ID="grdDoc"
                                    OnNeedDataSource="grdDoc_NeedDataSource" Skin="MetroTouch" AllowPaging="true"
                                    AutoGenerateColumns="false" OnSelectedIndexChanged="grdDoc_SelectedIndexChanged"
                                    PageSize="300" Width="100%">
                                    <ClientSettings Selecting-AllowRowSelect="true" EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" />
                                    <MasterTableView DataKeyNames="TaiLieu" ShowHeader="false">
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
                                                                    <asp:Label runat="server" ID="lbPhanNhom" class="assetDetail"
                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "PhanNhom") %>'>
                                                                    </asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right"><span class="assetDetail">Phân loại:&nbsp;</span></td>
                                                                <td>
                                                                    <asp:Label runat="server" ID="lbSoPhienBan" class="assetDetail"
                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "SoPhienBan") %>'>
                                                                    </asp:Label>
                                                            </tr>
                                                            <tr>
                                                                <td align="right"><span class="assetDetail">Phân nhóm:&nbsp;</span></td>
                                                                <td>
                                                                    <asp:Label runat="server" ID="lbDuongDan" class="assetDetail" 
                                                                        Style="text-overflow: ellipsis; white-space: nowrap; overflow: hidden"
                                                                        Width="220px"
                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "DuongDan") %>'>
                                                                    </asp:Label>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <asp:HiddenField runat="server" ID="hfDoc" Value='<%# DataBinder.Eval(Container.DataItem, "TaiLieu")%>' />
                                                    <asp:HiddenField runat="server" ID="hfDocFullName" Value='<%# DataBinder.Eval(Container.DataItem, "FullName")%>' />
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
                        <telerik:RadTabStrip RenderMode="Lightweight" ID="TabDocDetail" runat="server" Width="100%" Height="100%" Skin="MetroTouch" MultiPageID="MultiPageDoc" SelectedIndex="0" EnableDragToReorder="true">
                            <Tabs>
                                <telerik:RadTab Text="Chi tiết" />
                            </Tabs>
                        </telerik:RadTabStrip>
                        <telerik:RadMultiPage runat="server" ID="MultiPageDoc" SelectedIndex="0">
                            <telerik:RadPageView runat="server" ID="RadPageView8">
                                <br />
                                <table width="100%">
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Vật tư:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtTaiLieu" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                            <telerik:RadTextBox ID="txtDienGiai" runat="server" Skin="MetroTouch" Width="250" ReadOnly="True" />
                                        </td>

                                        <td align="right" style="width: 150px; padding-right: 2px">Đơn vị:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtDonVi" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Tiêu đề:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtTieuDe" runat="server" Skin="MetroTouch" Width="300" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <dl class="accordion">
                                                <dt style="width: 100%;">
                                                    <span>Chi tiết tài liệu</span>
                                                </dt>
                                            </dl>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Số phiên bản:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtSoPhienBan" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phân nhóm:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPhanNhom" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Loại tài liệu:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtLoaiTaiLieu" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày cập nhật phiên bản:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNgayCapNhatPhienBan" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Tên gốc:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtTenGoc" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày có hiệu lực:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNgayCoHieuLuc" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Đã tải lên:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNgayTaiLen" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày hết hiệu lực:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNgayHetHieuLuc" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Đường dẫn:</td>
                                        <td colspan="3">
                                            <telerik:RadTextBox ID="txtDuongDan" runat="server" Skin="MetroTouch" Width="450" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngưng sử dụng:</td>
                                        <td colspan="3">
                                            <telerik:RadCheckBox ID="cbNgungSuDung" runat="server" Skin="MetroTouch" Enabled="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày tạo:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNgayTao" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày cập nhật:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNgayCapNhat" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Người tạo:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNguoiTao" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Người cập nhật:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNguoiCapNhat" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
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

                <telerik:AjaxSetting AjaxControlID="grdDoc">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lblDocName" />
                        <telerik:AjaxUpdatedControl ControlID="TabDocDetail" LoadingPanelID="RadAjaxLoadingPanel2" />
                        <telerik:AjaxUpdatedControl ControlID="MultiPageDoc" LoadingPanelID="RadAjaxLoadingPanel2" />
                        <telerik:AjaxUpdatedControl ControlID="grdDoc" LoadingPanelID="RadAjaxLoadingPanel2" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDoc" LoadingPanelID="RadAjaxLoadingPanel2" />
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
                var pane = splitter.getPaneById("<%= PanelDocList.ClientID %>");

                if (strValue == "ExpandLeft") {
                    var isCollapseSuccess = pane.collapse();
                }
                else if (strValue == "SplitView") {
                    var isExpandSuccess = pane.expand(pane);
                }
                else if (strValue == "ExpandRight") {
                    window.location.href = "DocumentList.aspx";
                }
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
