<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WODetail.aspx.cs" Inherits="EDMs.Web.Controls.PPM.WODetail" EnableViewState="true" %>
 
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
        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_ToolBarObject_i0_lblObjectNamePanel {
            display: contents !important;
        }

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdPartPanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdPart_GridData {
            height: 100% !important;
        }
        
        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdObjectPartUsagePanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdObjectPartUsage_GridData {
            height: 100% !important;
        }
        
        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdObjectPartAssociatedPanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdObjectPartAssociated_GridData {
            height: 100% !important;
        }
        
        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdObjectDepreciationPanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdObjectDepreciation_GridData {
            height: 100% !important;
        }
        
        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdWOActivitiesPanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdWOActivities_GridData {
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


        .detailLabel {
            font-size: 14px;
            font-weight: normal;
            font-family: helvetica, arial, verdana, sans-serif;
            display: inline;
            overflow: hidden;
            float: right;
        }
        </style>

    <div style=" width: 100%; height: 100%; background-color: #f0f0f0 ">
        <telerik:RadSplitter RenderMode="Lightweight" ID="MainSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
            <telerik:RadPane ID="Radpane1" runat="server" Width="100%" Height="40" ShowContentDuringLoad="False" Scrollable="false" Scrolling="None">
                <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ToolBarObject" EnableRoundedCorners="true" EnableShadows="true" Skin="MetroTouch" Height="40">
                    <Items>
                        <telerik:RadToolBarButton Text="Left" Group="Align">
                            <ItemTemplate>
                                <span style="                                        color: dodgerblue;
                                        font-size: 16px;
                                        font-weight: normal;
                                        font-family: helvetica, arial, verdana, sans-serif;">Phiếu CV: </span>
                                
                                <span style="color: black;font-size: 16px;font-weight: normal;font-family: helvetica, arial, verdana, sans-serif;"><asp:Label runat="server" ID="lblObjectName"></asp:Label> </span>
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
                    <telerik:RadPane ID="PanelObjectList" runat="server" Width="400" MinWidth="300">
                        <telerik:RadSplitter RenderMode="Lightweight" ID="LeftSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
                            <telerik:RadPane ID="Radpane3" runat="server" Width="100%" Height="27">
                                <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtSearch" runat="server"
                                                    EmptyMessage="Search within All Material Lists" InvalidStyleDuration="100">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="btnSearch" runat="server" ImageAlign="AbsMiddle" CssClass="styleIcon"
                                           ImageUrl="~/Images/search20.png" OnClick="btnSearch_OnClick"/>
                            </telerik:RadPane>
                            <telerik:RadPane ID="Radpane4" runat="server" Width="100%" >
                                <telerik:RadListBox RenderMode="Lightweight" runat="server" ID="lbObject" Height="100%" Width="100%" LoadingPanelID="<%# RadAjaxLoadingPanel2.ClientID %>" EnableLoadOnDemand="False" Skin="MetroTouch" OnSelectedIndexChanged="lbObject_OnSelectedIndexChanged" AutoPostBack="True">
                                    <ItemTemplate>
                                        <span class="assetTitle"><%# DataBinder.Eval(Container, "Text")%></span>
                                        <table>
                                            <tr>
                                                <td align="right" style="width: 150px"><span class="assetDetail">Độ ưu tiên:&nbsp;</span></td>
                                                <td><span class="assetDetail"><%# DataBinder.Eval(Container, "Attributes['DoUuTien']") %></span></td>
                                            </tr>
                                            <tr>
                                                <td align="right" ><span class="assetDetail">Trạng thái:&nbsp;</span></td>
                                                <td><span class="assetDetail"><%# DataBinder.Eval(Container, "Attributes['TrangThaiCongViec']") %></span></td>
                                            </tr>
                                            <tr>
                                                <td align="right" ><span class="assetDetail">Đơn vị:&nbsp;</span></td>
                                                <td><span class="assetDetail" ><%# DataBinder.Eval(Container, "Attributes['DonVi']") %></span></td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadListBox>
                            </telerik:RadPane>
                        </telerik:RadSplitter>
                    </telerik:RadPane>
                    <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
                    </telerik:RadSplitBar>
                    <telerik:RadPane ID="PanelObjectDetail" runat="server">
                        <telerik:RadTabStrip RenderMode="Lightweight" ID="TabObjectDetail" runat="server"  Width="100%" Height="100%" Skin="MetroTouch" MultiPageID="MultiPageObject" SelectedIndex="0" EnableDragToReorder="true">
                            <Tabs>
                                <telerik:RadTab Text="Chi tiết"/>
                                <telerik:RadTab Text="Hạng mục thực hiện"/>
                                <telerik:RadTab Text="Vật tư"/>
                                <telerik:RadTab Text="Tài liệu"/>
                            </Tabs>
                        </telerik:RadTabStrip>
                        <telerik:RadMultiPage runat="server" ID="MultiPageObject" SelectedIndex="0">
                            <%--Chi Tiet--%>
                            <telerik:RadPageView runat="server" ID="RadPageView1">
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
                                            <telerik:RadTextBox ID="txtPhieuCV" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                            <telerik:RadTextBox ID="txtDienGiai" runat="server" Skin="MetroTouch" Width="250" ReadOnly="True"/>
                                        </td>
                                        
                                        <td align="right" style="width: 150px; padding-right: 2px">Đơn vị:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtDonVi" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Thiết bị:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtThietBi" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Bộ phận:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtBoPhan" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Kiểu:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtKieu" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Người tạo:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNguoiTao" runat="server" Skin="MetroTouch" Width="200" ReadOnly="True"/>
                                        </td>
                                        
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Trạng thái:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtTrangThai" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True" style="font-weight: bold;"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày tạo:</td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtNgayTao" runat="server" Skin="MetroTouch" Width="150" Enabled="False">
                                                <DateInput DateFormat="dd-MMM-yy"/>
                                            </telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Bảo Hành:</td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbBaoHanh" runat="server" Skin="MetroTouch" Enabled="False"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Thuê ngoài:</td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbThueNgoai" runat="server" Skin="MetroTouch" Enabled="False"/>
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
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày theo lịch:</td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtngayTheoLich" runat="server" Skin="MetroTouch" Width="150" Enabled="False">
                                                <DateInput DateFormat="dd-MMM-yy" ReadOnly="True"/>
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Mã quy trình:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtMaQuyTrinh" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phân loại:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPhanLoai" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phân công bởi:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPhanCongBoi" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày báo cáo:</td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtNgayBaoCao" runat="server" Skin="MetroTouch" Width="150" Enabled="False">
                                                <DateInput DateFormat="dd-MMM-yy" ReadOnly="True"/>
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phân công đến:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPhanCongDen" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
                                        </td>
                                    </tr>
                                <tr>
                                    <td align="right" style="width: 150px; padding-right: 2px">Người báo cáo:</td>
                                    <td>
                                        <telerik:RadTextBox ID="txtNguoiBaoCao" runat="server" Skin="MetroTouch" Width="150" ReadOnly="True"/>
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
                                            <telerik:RadDatePicker ID="txtKeHoachBatDau" runat="server" Skin="MetroTouch" Width="150" Enabled="False">
                                                <DateInput DateFormat="dd-MMM-yy"/>
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Kế hoạch hoàn thành:</td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtKeHoachHoanThanh" runat="server" Skin="MetroTouch" Width="150" Enabled="False">
                                                <DateInput DateFormat="dd-MMM-yy"/>
                                            </telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày bắt đầu:</td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtNgayBatDau" runat="server" Skin="MetroTouch" Width="150" Enabled="False">
                                                <DateInput DateFormat="dd-MMM-yy"/>
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày hoàn thành:</td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtNgayHoanThanh" runat="server" Skin="MetroTouch" Width="150" Enabled="False">
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
                                            <telerik:RadCheckBox ID="cbDatChoPhepSuDung" runat="server" Skin="MetroTouch" Enabled="False"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Không thể sử dụng:</td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbKhongTheSuDung" runat="server" Skin="MetroTouch" Enabled="False"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Sử dụng tạm thời:</td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbSuDungTamThoi" runat="server" Skin="MetroTouch" Enabled="False"/>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ghi chú/kết luận:</td>
                                        <td colspan="3">
                                            <telerik:RadTextBox ID="txtKetLuan" runat="server" Skin="MetroTouch" Width="723" TextMode="MultiLine" Rows="3"  ReadOnly="True"/>
                                        </td>
                                    </tr>
                                    
                                    
                                </table>
                            </telerik:RadPageView>
                            
                            <telerik:RadPageView runat="server" ID="RadPageView3">
                                <telerik:RadGrid ID="grdWOActivities" runat="server" AllowPaging="False"
                                    AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                    GridLines="None" Height="100%" Skin="Metro"
                                    OnNeedDataSource="grdWOActivities_OnNeedDataSource" 
                                    >
                                    <MasterTableView Width="100%" TableLayout="Auto" CssClass="rgMasterTable" >
                                        <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Làm mới dữ liệu" ShowExportToExcelButton="True"/>
                                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <Columns>
                                            
                                            <telerik:GridBoundColumn DataField="HangMuc" HeaderText="Hạng mục" UniqueName="HangMuc">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="TrinhDo" HeaderText="Trình độ" UniqueName="TrinhDo">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="NoiDung" HeaderText="Nội dung" UniqueName="NoiDung">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridTemplateColumn HeaderText="Từ ngày" UniqueName="TuNgay" AllowFiltering="false" >
                                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <%# Eval("TuNgay", "{0:dd-MMM-yy}") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn HeaderText="Đến ngày" UniqueName="DenNgay" AllowFiltering="false" >
                                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <%# Eval("DenNgay", "{0:dd-MMM-yy}") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridBoundColumn DataField="SoGioDuKien" HeaderText="Số giờ dự kiến" UniqueName="SoGioDuKien">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="SoNguoiYeuCau" HeaderText="Số người yêu cầu" UniqueName="SoNguoiYeuCau">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="DinhMucVatTu" HeaderText="Định mức vật tư" UniqueName="DinhMucVatTu">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="DaHoanThanh" HeaderText="Hoàn thành" UniqueName="DaHoanThanh">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="ThueNgoai" HeaderText="Thuê ngoài" UniqueName="ThueNgoai">
                                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="BaoHanh" HeaderText="Bảo hành" UniqueName="BaoHanh">
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
                            <%--Vat tu--%>
                            <telerik:RadPageView runat="server" ID="RadPageView2">
                                <telerik:RadSplitter RenderMode="Lightweight" ID="PMScheduleSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
                                    <telerik:RadPane ID="PMSchedulePanel" runat="server" Width="100%" Height="300">
                                        <telerik:RadGrid ID="grdPart" runat="server" AllowPaging="False"
                                            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                            GridLines="None" Height="100%" Skin="Metro"
                                            OnNeedDataSource="grdPart_OnNeedDataSource" 
                                            >
                                            <MasterTableView Width="100%" TableLayout="Auto" CssClass="rgMasterTable" >
                                                <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Làm mới dữ liệu" ShowExportToExcelButton="False"/>
                                                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <Columns>
                                                    
                                                    <telerik:GridBoundColumn DataField="VatTu" HeaderText="Vật tư" UniqueName="VatTu">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="TenVatTu" HeaderText="Tên vật tư" UniqueName="TenVatTu">
                                                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="DonViTinh" HeaderText="Đơn vị tính" UniqueName="DonViTinh">
                                                        <HeaderStyle HorizontalAlign="Center" Width="100" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="SoLuongKeHoach" HeaderText="Số lượng" UniqueName="SoLuongKeHoach">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="TonKho" HeaderText="Tồn kho" UniqueName="TonKho">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="MuaTrucTiep" HeaderText="Trực tiếp" UniqueName="MuaTrucTiep">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="Kho" HeaderText="Kho" UniqueName="Kho">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="DaSuDung" HeaderText="Đã sử dụng" UniqueName="DaSuDung">
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
                            <%--Tai lieu lien quan--%>
                            <telerik:RadPageView runat="server" ID="RadPageView5">
                                <telerik:RadTreeView RenderMode="Lightweight" ID="rtvObjectDocument"  runat="server" Skin="MetroTouch" OnContextMenuItemClick="rtvObjectDocument_OnContextMenuItemClick">
                                    <ContextMenus>
                                        <telerik:RadTreeViewContextMenu ID="DownloadMenu" runat="server">
                                            <Items>
                                                <telerik:RadMenuItem Value="Download" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Download File" ImageUrl="../../Images/download.png"/>
                                            </Items>
                                            <CollapseAnimation Type="none"></CollapseAnimation>
                                        </telerik:RadTreeViewContextMenu>
                                    </ContextMenus>

                                </telerik:RadTreeView>
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
                
                <telerik:AjaxSetting AjaxControlID="lbObject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lblObjectName"/>
                        <telerik:AjaxUpdatedControl ControlID="TabObjectDetail" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="MultiPageObject" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="lbObject" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lbObject" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdPart">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdPart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdObjectPartUsage">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdObjectPartUsage" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdObjectPartAssociated">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdObjectPartAssociated" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdObjectDepreciation">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdObjectDepreciation" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdObjectEvent">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdObjectEvent" LoadingPanelID="RadAjaxLoadingPanel2"/>
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

                var splitter = $find("<%= SubSplitter.ClientID %>");
                var pane = splitter.getPaneById("<%= PanelObjectList.ClientID %>");

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