<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EAM_REQList.aspx.cs" Inherits="EDMs.Web.EAM_REQList" EnableViewState="true" %>

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
                                <span style="color: dodgerblue;font-size: 16px;font-weight: normal;font-family: helvetica, arial, verdana, sans-serif;">Yêu cầu mua hàng: </span>
                                
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
                                                    EmptyMessage="Search within All Requisition" InvalidStyleDuration="100">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="btnSearch" runat="server" ImageAlign="AbsMiddle" CssClass="styleIcon"
                                           ImageUrl="~/Images/search20.png" OnClick="btnSearch_OnClick"/>
                            </telerik:RadPane>
                            <telerik:RadPane ID="Radpane4" runat="server" Width="100%" >
                                <telerik:RadListBox RenderMode="Lightweight" runat="server" ID="lbREQ" Height="100%" Width="100%" LoadingPanelID="<%# RadAjaxLoadingPanel2.ClientID %>" EnableLoadOnDemand="False" Skin="MetroTouch" OnSelectedIndexChanged="lbREQ_OnSelectedIndexChanged" AutoPostBack="True">
                                    <ItemTemplate>
                                        <span class="ObjectTitle"><%# DataBinder.Eval(Container, "Text")%></span>
                                        <table>
                                            <tr>
                                                <td align="right" style="width: 120px"><span class="ObjectDetail">Đơn vị:&nbsp;</span></td>
                                                <td><span class="ObjectDetail"><%# DataBinder.Eval(Container, "Attributes['DonVi']") %></span></td>
                                            </tr>
                                            <tr>
                                                <td align="right" ><span class="ObjectDetail">Trạng thái:&nbsp;</span></td>
                                                <td><span class="ObjectDetail"><%# DataBinder.Eval(Container, "Attributes['TrangThai']") %></span></td>
                                            </tr>
                                            <tr>
                                                <td align="right"><span class="ObjectDetail">Kho:&nbsp;</span></td>
                                                <td><span class="ObjectDetail"><%# DataBinder.Eval(Container, "Attributes['Kho']") %></span></td>
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
                                <telerik:RadTab Text="Vật Tư"/>
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
                                        <td align="right" style="width: 150px; padding-right: 2px">Yêu cầu mua hàng:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtYeuCauMuaHang" runat="server" Skin="MetroTouch" Width="150"/>
                                            <telerik:RadTextBox ID="txtDienGiai" runat="server" Skin="MetroTouch" Width="250"/>
                                        </td>
                                        
                                        <td align="right" style="width: 150px; padding-right: 2px">Đơn vị:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtDonVi" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Trạng thái:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtTrangThai" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Kho:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtKho" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Người yêu cầu:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtNguoiYeuCau" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày yêu cầu:</td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtNgayYeuCau" runat="server" Skin="MetroTouch" Width="150">
                                                <DateInput DateFormat="dd-MMM-yy"/>
                                            </telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phiếu công việc:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPhieuCV" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Mục công việc:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtMucCV" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="5">
                                            <dl class="accordion">
                                                <dt style="width: 100%;">
                                                    <span>Phê duyệt</span>
                                                </dt>
                                            </dl>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right" style="width: 150px; padding-right: 2px">Phê duyệt bởi:</td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPheDuyetBoi" runat="server" Skin="MetroTouch" Width="150"/>
                                        </td>
                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày phê duyệt:</td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtNgayPheDuyet" runat="server" Skin="MetroTouch" Width="150">
                                                <DateInput DateFormat="dd-MMM-yy"/>
                                            </telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    
                                </table>
                            </telerik:RadPageView>
                            
                            <telerik:RadPageView runat="server" ID="RadPageView1">
                                <telerik:RadSplitter RenderMode="Lightweight" ID="REQ_linesSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
                                    <telerik:RadPane ID="REQ_linesPanel" runat="server" Width="100%" Height="300">
                                        <telerik:RadGrid ID="grdREQLines" runat="server" AllowPaging="False"
                                            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                            GridLines="None" Height="100%" Skin="Metro"
                                            OnNeedDataSource="grdREQLines_OnNeedDataSource" 
                                            >
                                            <MasterTableView Width="100%" TableLayout="Auto" CssClass="rgMasterTable" >
                                                <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Làm mới dữ liệu" ShowExportToExcelButton="False"/>
                                                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <Columns>
                                                    
                                                    <telerik:GridBoundColumn DataField="MaVatTu" HeaderText="Mã vật tư" UniqueName="MaVatTu">
                                                        <HeaderStyle HorizontalAlign="Center" Width="100" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="TenVatTu" HeaderText="Tên vật tư" UniqueName="TenVatTu">
                                                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="DVT" HeaderText="Đơn vị tính" UniqueName="DVT">
                                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="Kieu" HeaderText="Kiểu" UniqueName="Kieu">
                                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="Dong" HeaderText="Dòng" UniqueName="Dong">
                                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="SoLuong" HeaderText="Số lượng" UniqueName="SoLuong" DataFormatString="{0:N}">
                                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="DonGia" HeaderText="Đơn giá" UniqueName="DonGia">
                                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="TienTe" HeaderText="Tiền tệ" UniqueName="TienTe">
                                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="TrangThai" HeaderText="Trạng thái" UniqueName="TrangThai">
                                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="BaoGia" HeaderText="Báo giá" UniqueName="BaoGia">
                                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridBoundColumn DataField="DongBaoGia" HeaderText="Quotation line" UniqueName="DongBaoGia">
                                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    
                                                    <telerik:GridTemplateColumn HeaderText="Cấp trước ngày" UniqueName="CapTruocngay" AllowFiltering="false" >
                                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <%# Eval("CapTruocngay", "{0:dd-MMM-yy}") %>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridBoundColumn DataField="YeuCauChaoGia" HeaderText="Yêu cầu chào giá" UniqueName="YeuCauChaoGia">
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
                                    <telerik:RadSplitBar ID="RadSplitbar2" runat="server" CollapseMode="Forward">
                                    </telerik:RadSplitBar>
                                    <telerik:RadPane ID="MaterialDetail" runat="server" Width="100%" Visible="False">
                                        <telerik:RadTabStrip RenderMode="Lightweight" ID="RadTabStrip1" runat="server"  Width="100%" Skin="Silk" MultiPageID="RadMultiPage1" SelectedIndex="0">
                                            <Tabs>
                                                <telerik:RadTab Text="Chi Tiết"/>
                                            </Tabs>
                                        </telerik:RadTabStrip>
                                        
                                        <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0">
                                            <telerik:RadPageView runat="server">
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
                                                        <td align="right" style="width: 150px; padding-right: 2px">Yêu cầu mua hàng:</td>
                                                        <td>
                                                            <telerik:RadTextBox ID="RadTextBox1" runat="server" Skin="MetroTouch" Width="150"/>
                                                            <telerik:RadTextBox ID="RadTextBox2" runat="server" Skin="MetroTouch" Width="250"/>
                                                        </td>
                                                        
                                                        <td align="right" style="width: 150px; padding-right: 2px">Đơn vị:</td>
                                                        <td>
                                                            <telerik:RadTextBox ID="RadTextBox3" runat="server" Skin="MetroTouch" Width="150"/>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="width: 150px; padding-right: 2px">Trạng thái:</td>
                                                        <td>
                                                            <telerik:RadTextBox ID="RadTextBox4" runat="server" Skin="MetroTouch" Width="150"/>
                                                        </td>
                                                        <td align="right" style="width: 150px; padding-right: 2px">Kho:</td>
                                                        <td>
                                                            <telerik:RadTextBox ID="RadTextBox5" runat="server" Skin="MetroTouch" Width="150"/>
                                                        </td>
                                                    </tr>
                                                    
                                                    <tr>
                                                        <td align="right" style="width: 150px; padding-right: 2px">Người yêu cầu:</td>
                                                        <td>
                                                            <telerik:RadTextBox ID="RadTextBox6" runat="server" Skin="MetroTouch" Width="150"/>
                                                        </td>
                                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày yêu cầu:</td>
                                                        <td>
                                                            <telerik:RadDatePicker ID="RadDatePicker1" runat="server" Skin="MetroTouch" Width="150">
                                                                <DateInput DateFormat="dd-MMM-yy"/>
                                                            </telerik:RadDatePicker>
                                                        </td>
                                                    </tr>
                                                    
                                                    <tr>
                                                        <td align="right" style="width: 150px; padding-right: 2px">Phiếu công việc:</td>
                                                        <td>
                                                            <telerik:RadTextBox ID="RadTextBox7" runat="server" Skin="MetroTouch" Width="150"/>
                                                        </td>
                                                        <td align="right" style="width: 150px; padding-right: 2px">Mục công việc:</td>
                                                        <td>
                                                            <telerik:RadTextBox ID="RadTextBox8" runat="server" Skin="MetroTouch" Width="150"/>
                                                        </td>
                                                    </tr>
                                                    
                                                    <tr>
                                                        <td colspan="5">
                                                            <dl class="accordion">
                                                                <dt style="width: 100%;">
                                                                    <span>Phê duyệt</span>
                                                                </dt>
                                                            </dl>
                                                        </td>
                                                    </tr>
                                                    
                                                    <tr>
                                                        <td align="right" style="width: 150px; padding-right: 2px">Phê duyệt bởi:</td>
                                                        <td>
                                                            <telerik:RadTextBox ID="RadTextBox9" runat="server" Skin="MetroTouch" Width="150"/>
                                                        </td>
                                                        <td align="right" style="width: 150px; padding-right: 2px">Ngày phê duyệt:</td>
                                                        <td>
                                                            <telerik:RadDatePicker ID="RadDatePicker2" runat="server" Skin="MetroTouch" Width="150">
                                                                <DateInput DateFormat="dd-MMM-yy"/>
                                                            </telerik:RadDatePicker>
                                                        </td>
                                                    </tr>
                                                    
                                                </table>
                                            </telerik:RadPageView>
                                        </telerik:RadMultiPage>
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
                
                <telerik:AjaxSetting AjaxControlID="lbREQ">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lblObjectName"/>
                        <telerik:AjaxUpdatedControl ControlID="TabObjectDetail" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="MultiPageObject" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="MultiPageObject" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lbREQ" LoadingPanelID="RadAjaxLoadingPanel2"/>
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