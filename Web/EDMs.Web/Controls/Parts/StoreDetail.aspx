﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StoreDetail.aspx.cs" Inherits="EDMs.Web.Controls.Store.StoreDetail" %>

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

        #ctl00_ContentPlaceHolder2_grdStore_GridData {
            height: 60vh !important;
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
                                    <span style="color: dodgerblue; font-size: 16px; font-weight: normal; font-family: helvetica, arial, verdana, sans-serif;">Kho: </span>

                                    <span style="color: black; font-size: 16px; font-weight: normal; font-family: helvetica, arial, verdana, sans-serif;">
                                        <asp:Label runat="server" ID="lblStoreName"></asp:Label>
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
                    <telerik:RadPane ID="PanelStoreList" runat="server" Width="400" MinWidth="300">
                        <telerik:RadSplitter RenderMode="Lightweight" ID="LeftSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
                            <telerik:RadPane ID="Radpane3" runat="server" Width="100%" Height="27">
                                <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtSearch" runat="server"
                                    EmptyMessage="Search within All store" InvalidStyleDuration="100">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="btnSearch" runat="server" ImageAlign="AbsMiddle" CssClass="styleIcon"
                                    ImageUrl="~/Images/search20.png" OnClick="btnSearch_Click" />
                            </telerik:RadPane>
                            <telerik:RadPane ID="Radpane4" runat="server" Width="100%">
                                <telerik:RadGrid runat="server" ID="grdStore"
                                    OnNeedDataSource="grdStore_NeedDataSource" Skin="MetroTouch" AllowPaging="true"
                                    AutoGenerateColumns="false" OnSelectedIndexChanged="grdStore_SelectedIndexChanged"
                                    PageSize="300" Width="100%">
                                    <ClientSettings Selecting-AllowRowSelect="true" EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" />
                                    <MasterTableView DataKeyNames="Kho" ShowHeader="false">
                                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                        <Columns>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Font-Size="14px" />
                                                <ItemTemplate>
                                                    <div style="width: 350px">
                                                        <span class="assetTitle" runat="server" id="assetFullname" style="display: block"><%# DataBinder.Eval(Container.DataItem, "FullName")%></span>
                                                        <table>
                                                            <tr>
                                                                <td align="right" style="width: 120px"><span class="assetDetail">Nhóm:&nbsp;</span></td>
                                                                <td>
                                                                    <asp:Label runat="server" ID="lbNhom" class="assetDetail"
                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "Nhom") %>'>
                                                                    </asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right"><span class="assetDetail">Vị trí:&nbsp;</span></td>
                                                                <td>
                                                                    <asp:Label runat="server" ID="lbViTri" class="assetDetail"
                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "ViTri") %>'>
                                                                    </asp:Label>
                                                            </tr>
                                                            <tr>
                                                                <td align="right"><span class="assetDetail">Đơn vị:&nbsp;</span></td>
                                                                <td>
                                                                    <asp:Label runat="server" ID="lbDonVi" class="assetDetail"
                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "DonVi") %>'>
                                                                    </asp:Label>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <asp:HiddenField runat="server" ID="hfStore" Value='<%# DataBinder.Eval(Container.DataItem, "Kho")%>' />
                                                    <asp:HiddenField runat="server" ID="hfStoreFullName" Value='<%# DataBinder.Eval(Container.DataItem, "FullName")%>' />
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
                        <telerik:RadTabStrip RenderMode="Lightweight" ID="TabStoreDetail" runat="server" Width="100%" Height="100%" Skin="MetroTouch" MultiPageID="MultiPageStore" SelectedIndex="0" EnableDragToReorder="true">
                            <Tabs>
                                <telerik:RadTab Text="Chi tiết" />
                            </Tabs>
                        </telerik:RadTabStrip>
                        <telerik:RadMultiPage runat="server" ID="MultiPageStore" SelectedIndex="0">
                            <telerik:RadPageView runat="server" ID="RadPageView8">
                                <br />
                                <table>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Thiết bị:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtKho" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                            <telerik:RadTextBox ID="txtDienGiai" runat="server" Skin="MetroTouch" Width="250" ReadOnly="True" />
                                        </td>

                                        <td align="right" style="width: 150px; padding-right: 2px">Tổ chức:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtDonVi" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Vị trí</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtViTri" runat="server" Skin="MetroTouch" Width="300" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Khu vực</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtKhuVuc" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Nhóm</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNhom" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Kho cha</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtKhoCha" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">PP tính giá</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPPTinhGia" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" />
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngưng sử dụng</td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbNgungSuDung" runat="server" Skin="MetroTouch" Enabled="false" />
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

                <telerik:AjaxSetting AjaxControlID="grdStore">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lblStoreName" />
                        <telerik:AjaxUpdatedControl ControlID="TabStoreDetail" LoadingPanelID="RadAjaxLoadingPanel2" />
                        <telerik:AjaxUpdatedControl ControlID="MultiPageStore" LoadingPanelID="RadAjaxLoadingPanel2" />
                        <telerik:AjaxUpdatedControl ControlID="grdStore" LoadingPanelID="RadAjaxLoadingPanel2" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdStore" LoadingPanelID="RadAjaxLoadingPanel2" />
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
                var pane = splitter.getPaneById("<%= PanelStoreList.ClientID %>");

                if (strValue == "ExpandLeft") {
                    var isCollapseSuccess = pane.collapse();
                }
                else if (strValue == "SplitView") {
                    var isExpandSuccess = pane.expand(pane);
                }
                else if (strValue == "ExpandRight") {
                    window.location.href = "StoreList.aspx";
                }
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>

