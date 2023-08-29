<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EAM_PartList.aspx.cs" Inherits="EDMs.Web.EAM_PartList" EnableViewState="true" %>

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
        .ObjectTitle {
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

        .ObjectDetail {
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
        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_ToolBarObject_i0_lblObjectNamePanel {
            display: contents !important;
        }

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdObjectLaborPlanPanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdObjectLaborPlan_GridData {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdStockPanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdStock_GridData {
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
                <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ToolBarObject" EnableRoundedCorners="true" EnableShadows="true" Skin="MetroTouch" Height="40">
                    <Items>
                        <telerik:RadToolBarButton Text="Left" Group="Align">
                            <ItemTemplate>
                                <span style="color: dodgerblue;font-size: 16px;font-weight: normal;font-family: helvetica, arial, verdana, sans-serif;">Vật tư: </span>
                                
                                <span style="color: black;font-size: 16px;font-weight: normal;font-family: helvetica, arial, verdana, sans-serif;"><asp:Label runat="server" ID="lblObjectName"></asp:Label> </span>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        
                    </Items>
                </telerik:RadToolBar>
            </telerik:RadPane>
            <telerik:RadPane ID="Radpane2" runat="server" Width="100%" ShowContentDuringLoad="False">
                <telerik:RadSplitter RenderMode="Lightweight" ID="SubSplitter" runat="server" Height="100%" Width="100%" Skin="Silk">
                    <telerik:RadPane ID="PanelObjectList" runat="server" Width="400" MinWidth="300">
                        <telerik:RadSplitter RenderMode="Lightweight" ID="LeftSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
                            <telerik:RadPane ID="Radpane3" runat="server" Width="100%" Height="27">
                                <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtSearch" runat="server"
                                                    EmptyMessage="Search within All Parts" InvalidStyleDuration="100">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="btnSearch" runat="server" ImageAlign="AbsMiddle" CssClass="styleIcon"
                                           ImageUrl="~/Images/search20.png" OnClick="btnSearch_OnClick"/>
                            </telerik:RadPane>
                            <telerik:RadPane ID="Radpane4" runat="server" Width="100%" >
                                <telerik:RadListBox RenderMode="Lightweight" runat="server" ID="lbPart" Height="100%" Width="100%" LoadingPanelID="<%# RadAjaxLoadingPanel2.ClientID %>" EnableLoadOnDemand="False" Skin="MetroTouch" OnSelectedIndexChanged="lbPart_OnSelectedIndexChanged" AutoPostBack="True">
                                    <ItemTemplate>
                                        <span><%# DataBinder.Eval(Container, "Text")%></span>
                                        <table>
                                            <tr>
                                                <td align="right" style="width: 120px"><span class="ObjectDetail">Phân loại:&nbsp;</span></td>
                                                <td><span class="ObjectDetail"><%# DataBinder.Eval(Container, "Attributes['PhanLoai']") %></span></td>
                                            </tr>
                                            <tr>
                                                <td align="right" ><span class="ObjectDetail">Phân nhóm:&nbsp;</span></td>
                                                <td><span class="ObjectDetail"><%# DataBinder.Eval(Container, "Attributes['PhanNhom']") %></span></td>
                                            </tr>
                                            <tr>
                                                <td align="right"><span class="ObjectDetail">Nhà cung cấp tham khảo:&nbsp;</span></td>
                                                <td><span class="ObjectDetail"><%# DataBinder.Eval(Container, "Attributes['NhaCungCapThamKhao']") %></span></td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadListBox>
                            </telerik:RadPane>
                        </telerik:RadSplitter>
                    </telerik:RadPane>
                    <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
                    </telerik:RadSplitBar>
                    <telerik:RadPane ID="PanelObjectDetail" runat="server" Scrolling="Y">
                        <telerik:RadTabStrip RenderMode="Lightweight" ID="TabObjectDetail" runat="server"  Width="100%" Height="100%" Skin="MetroTouch" MultiPageID="MultiPageObject" SelectedIndex="0" EnableDragToReorder="true">
                            <Tabs>
                                <telerik:RadTab Text="Chi Tiết"/>
                            </Tabs>
                        </telerik:RadTabStrip>
                        
                        <telerik:RadMultiPage runat="server" ID="MultiPageObject" SelectedIndex="0">
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
                                        <td align="right" style="width: 150px; padding-right: 2px">Vật tư:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtVatTu" runat="server" Skin="MetroTouch" Width="150"/>
                                            <telerik:RadTextBox ID="txtDienGiai" runat="server" Skin="MetroTouch" Width="250"/>
                                        </td>
                                        
                                        <td align="right" style="width: 150px; padding-right: 2px">Thiết bị:</td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbThietBi" runat="server" Skin="MetroTouch"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phân loại:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPhanLoai" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Đơn vị tính:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtDonViTinh" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Nhân nhóm:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPhanNhom" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Nhà sản xuất:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNhaSanXuat" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Công cụ:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtCongCu" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Part number:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPartNumber" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngưng sử dụng:</td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbNgungSuDung" runat="server" Skin="MetroTouch"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="5">
                                            <dl class="accordion">
                                                <dt style="width: 100%;">
                                                    <span>Thông tin theo dõi</span>
                                                </dt>
                                            </dl>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Mã thuế hàng hóa:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtMaThuehangHoa" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phương thức theo dõi:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPhuongThucTheoDoi" runat="server" Skin="MetroTouch" Width="200"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Kiểm tra trước khi nhập:</td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbKiemTraNhap" runat="server" Skin="MetroTouch"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Số ngày bảo hành:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtSoNgayBaoHanh" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="5">
                                            <dl class="accordion">
                                                <dt style="width: 100%;">
                                                    <span>Thông tin đơn giá</span>
                                                </dt>
                                            </dl>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Đơn giá tiêu chuẩn:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtDonGiaTieuChuan" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Đơn giá cơ sở:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtDonGiaCoSo" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Đơn giá mua cuối:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtDonGiaMuaCuoi" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Đơn giá trung bình:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtDonGiaTrungBinh" runat="server" Skin="MetroTouch" Width="150"/>
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
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
            
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="lbPart">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lblObjectName"/>
                        <telerik:AjaxUpdatedControl ControlID="TabObjectDetail" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="MultiPageObject" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lbPart" LoadingPanelID="RadAjaxLoadingPanel2"/>
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