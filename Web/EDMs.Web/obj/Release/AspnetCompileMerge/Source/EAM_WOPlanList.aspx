<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EAM_WOPlanList.aspx.cs" Inherits="EDMs.Web.EAM_WOPlanList" EnableViewState="true" %>

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

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdWOPartPlanPanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdWOPartPlan_GridData {
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
        </style>

    <div style=" width: 100%; height: 100%; background-color: #f0f0f0 ">
        <telerik:RadSplitter RenderMode="Lightweight" ID="MainSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
            <telerik:RadPane ID="Radpane1" runat="server" Width="100%" Height="40" ShowContentDuringLoad="False">
                <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ToolBarWO" EnableRoundedCorners="true" EnableShadows="true" Skin="MetroTouch" Height="40">
                    <Items>
                        <telerik:RadToolBarButton Text="Left" Group="Align">
                            <ItemTemplate>
                                <span style="                                        color: dodgerblue;
                                        font-size: 16px;
                                        font-weight: normal;
                                        font-family: helvetica, arial, verdana, sans-serif;">Phiếu CV: </span>
                                
                                <span style="color: black;font-size: 16px;font-weight: normal;font-family: helvetica, arial, verdana, sans-serif;"><asp:Label runat="server" ID="lblWOName"></asp:Label> </span>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        
                    </Items>
                </telerik:RadToolBar>
            </telerik:RadPane>
            <telerik:RadPane ID="Radpane2" runat="server" Width="100%" ShowContentDuringLoad="False">
                <telerik:RadSplitter RenderMode="Lightweight" ID="SubSplitter" runat="server" Height="100%" Width="100%" Skin="Silk">
                    <telerik:RadPane ID="PanelWOList" runat="server" Width="400" MinWidth="300">
                        <telerik:RadSplitter RenderMode="Lightweight" ID="LeftSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
                            <telerik:RadPane ID="Radpane3" runat="server" Width="100%" Height="27">
                                <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtSearch" runat="server"
                                                    EmptyMessage="Search within All Work Orders" InvalidStyleDuration="100">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="btnSearch" runat="server" ImageAlign="AbsMiddle" CssClass="styleIcon"
                                           ImageUrl="~/Images/search20.png" OnClick="btnSearch_OnClick"/>
                            </telerik:RadPane>
                            <telerik:RadPane ID="Radpane4" runat="server" Width="100%" >
                                <telerik:RadListBox RenderMode="Lightweight" runat="server" ID="lbWO" Height="100%" Width="100%" LoadingPanelID="<%# RadAjaxLoadingPanel2.ClientID %>" EnableLoadOnDemand="False" Skin="MetroTouch" OnSelectedIndexChanged="lbWO_OnSelectedIndexChanged" AutoPostBack="True">
                                    <ItemTemplate>
                                        <span class="WOTitle"><%# DataBinder.Eval(Container, "Text")%></span>
                                        <table>
                                            <tr>
                                                <td align="right" style="width: 120px"><span class="WODetail">Độ ưu tiên:&nbsp;</span></td>
                                                <td><span class="WODetail"><%# DataBinder.Eval(Container, "Attributes['DoUuTien']") %></span></td>
                                            </tr>
                                            <tr>
                                                <td align="right" ><span class="WODetail">Trạng thái:&nbsp;</span></td>
                                                <td><span class="WODetail"><%# DataBinder.Eval(Container, "Attributes['TrangThai']") %></span></td>
                                            </tr>
                                            <tr>
                                                <td align="right"><span class="WODetail">Đơn vị:&nbsp;</span></td>
                                                <td><span class="WODetail"><%# DataBinder.Eval(Container, "Attributes['DonVi']") %></span></td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadListBox>
                            </telerik:RadPane>
                        </telerik:RadSplitter>
                    </telerik:RadPane>
                    <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
                    </telerik:RadSplitBar>
                    <telerik:RadPane ID="PanelWODetail" runat="server" Scrolling="Y">
                        <telerik:RadTabStrip RenderMode="Lightweight" ID="TabWODetail" runat="server"  Width="100%" Height="100%" Skin="MetroTouch" MultiPageID="MultiPageWO" SelectedIndex="0" EnableDragToReorder="true">
                            <Tabs>
                                <telerik:RadTab Text="Chi Tiết"/>
                            </Tabs>
                        </telerik:RadTabStrip>
                        
                        <telerik:RadMultiPage runat="server" ID="MultiPageWO" SelectedIndex="0">
                            <telerik:RadPageView runat="server" ID="RadPageView2" >
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
                                        <td align="right" style="width: 150px; padding-right: 2px">Phiếu CV:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPhieuCV" runat="server" Skin="MetroTouch" Width="120"/>
                                            <telerik:RadTextBox ID="txtDienGiai" runat="server" Skin="MetroTouch" Width="250"/>
                                        </td>
                                        
                                        <td align="right" style="width: 150px; padding-right: 2px">Đơn vị:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtDonVi" runat="server" Skin="MetroTouch" Width="120"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Thiết bị:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtThietBi" runat="server" Skin="MetroTouch" Width="120"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Bộ phận:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtBoPhan" runat="server" Skin="MetroTouch" Width="120"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Kiểu:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtKieu" runat="server" Skin="MetroTouch" Width="120"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Người tạo:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNguoiTao" runat="server" Skin="MetroTouch" Width="200"/>
                                        </td>
                                        
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Trạng thái:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtTrangThai" runat="server" Skin="MetroTouch" Width="120"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày tạo:</td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtNgayTao" runat="server" Skin="MetroTouch" Width="150">
                                                <DateInput DateFormat="dd-MMM-yy"/>
                                            </telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Bảo Hành:</td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbBaoHanh" runat="server" Skin="MetroTouch"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Thuê ngoài:</td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbThueNgoai" runat="server" Skin="MetroTouch"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Nội dung yêu cầu sửa chữa:</td>
                                        <td colspan="3">
                                            <telerik:RadTextBox ID="txtNoiDungYeuCauSuaChua" runat="server" Skin="MetroTouch" Width="723" TextMode="MultiLine" Rows="3"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <dl class="accordion">
                                                <dt style="width: 100%;">
                                                    <span>Lịch trình</span>
                                                </dt>
                                            </dl>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Kế hoạch bắt đầu:</td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtKeHoachBatDau" runat="server" Skin="MetroTouch" Width="150">
                                                <DateInput DateFormat="dd-MMM-yy"/>
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Kế hoạch hoàn thành:</td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtKeHoachHoanThanh" runat="server" Skin="MetroTouch" Width="150">
                                                <DateInput DateFormat="dd-MMM-yy"/>
                                            </telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày bắt đầu:</td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtNgayBatDau" runat="server" Skin="MetroTouch" Width="150">
                                                <DateInput DateFormat="dd-MMM-yy"/>
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày hoàn thành:</td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtNgayHoanThanh" runat="server" Skin="MetroTouch" Width="150">
                                                <DateInput DateFormat="dd-MMM-yy"/>
                                            </telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="5">
                                            <dl class="accordion">
                                                <dt style="width: 100%;">
                                                    <span>Kết quả bảo dưỡng, sửa chữa</span>
                                                </dt>
                                            </dl>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Đạt, cho phép sử dụng:</td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbDatChoPhepSuDung" runat="server" Skin="MetroTouch"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Không thể sử dụng:</td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbKhongTheSuDung" runat="server" Skin="MetroTouch"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Sử dụng tạm thời:</td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbSuDungTamThoi" runat="server" Skin="MetroTouch"/>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Kết luận sau sửa chữa:</td>
                                        <td colspan="3">
                                            <telerik:RadTextBox ID="txtKetLuanSuaChua" runat="server" Skin="MetroTouch" Width="723" TextMode="MultiLine" Rows="3"/>
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
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer">
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
        </script>
    </telerik:RadCodeBlock>
</asp:Content>