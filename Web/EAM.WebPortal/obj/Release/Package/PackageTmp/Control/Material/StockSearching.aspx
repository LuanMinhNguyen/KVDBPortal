<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockSearching.aspx.cs" Inherits="EAM.WebPortal.Control.Material.StockSearching" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2019.3.1023.45, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        html, body {
            height: 100%;
            margin: 0px;
            font: normal 15px helvetica, arial, verdana, sans-serif;
            overflow: hidden
        }
        .container {
            height: 100%;
            width: 100%;
            background-color: #f0f0f0 
        }
        .RadWizard {
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.3);
            *border: 1px solid #ddd;
        }
        .styleIcon
        {
            margin-left: -25px;
            margin-top: 5px;
            position: fixed;
        }

        .ontop {
            position: fixed;
        }

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
            font-size: 1em;
            line-height: 1.5em;
            text-overflow: ellipsis;
            height: 1.5em;
            white-space: nowrap;
            margin-left: 5px;
            overflow: hidden;
            font-weight: normal;
            font-family: helvetica, arial, verdana, sans-serif;
        }

        .ObjectLabel {
            color: #5c5c5c;
            font: normal 13px/17px helvetica, arial, verdana, sans-serif;
        }

        .ObjectDetail {
            font-size: 0.90em;
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
            border-bottom: 1px solid #46A3D3;
            margin: 0.5em auto 0.6em;
            padding: 0;
            text-align: left;
            text-decoration: none;
            display: block;
            color: #383838;
            font-size: 14px;
            font-weight: normal;
            font-family: helvetica, arial, verdana, sans-serif;
            line-height: 28px;
        }

        #ctl00_ContentPlaceHolder2_txtSearch {
            height: 27px !important;
        }

        #RAD_SPLITTER_PANE_CONTENT_Radpane3 {
            display: flex !important;
        }
        
        #ToolBarObject_i0_lblObjectNamePanel {
            display: contents !important;
        }

        #txtSearch {
            height: 27px !important;
            font: normal 13px/17px helvetica, arial, verdana, sans-serif;
            font-style: italic;
        }

        #ddlStatusPanel {
            display: contents !important;
        }

        #cbIsGenMR {
            opacity: 1;
            color: blue !important;
            font: normal 13px helvetica, arial, verdana, sans-serif !important;
        }

        .TextBoxInput{
            color: #383838;
            padding: 5px;
            background-color: transparent;
            border-radius: 2px;
            font: normal 13px helvetica, arial, verdana, sans-serif !important;
            min-height: 23px;
        }

        .DropdownList {
            color: #383838;
            background-color: transparent;
            border-radius: 2px;
            font: normal 13px helvetica, arial, verdana, sans-serif !important;
            min-height: 23px;
        }

        #btnProcess {
            font: normal 13px helvetica, arial, verdana, sans-serif !important;
        }

        #RAD_SPLITTER_PANE_CONTENT_Radpane2 {
            overflow: hidden !important;
        }
        

        .RadAjaxPanel {
            height: 100% !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="height: 100%">
    
    <telerik:RadSplitter RenderMode="Lightweight" ID="MainSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
        <telerik:RadPane ID="Radpane3" runat="server" Width="100%" Height="40" ShowContentDuringLoad="False">
            <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ToolBarObject" EnableRoundedCorners="true" EnableShadows="true" Skin="MetroTouch" Height="40" Width="100%">
                <Items>
                    <telerik:RadToolBarButton Text="Left" Group="Align">
                        <ItemTemplate>
                            <span style="color: dodgerblue; font-size: 16px; font-weight: normal; font-family: helvetica, arial, verdana, sans-serif;">Tra cứu vật tư tồn kho: </span>
                        </ItemTemplate>
                    </telerik:RadToolBarButton>
                            
                </Items>
            </telerik:RadToolBar>
        </telerik:RadPane>
        <telerik:RadPane ID="Radpane2" runat="server" Width="100%" ShowContentDuringLoad="False">
            <telerik:RadGrid RenderMode="Lightweight" ID="grdPartInStock" GridLines="None" runat="server"  Skin="Metro" Height="100%"  PageSize="50"  AllowAutomaticUpdates="True" AllowPaging="true" AutoGenerateColumns="False" ShowFooter="True" AllowFilteringByColumn="True"
                                OnNeedDataSource="grdPartInStock_OnNeedDataSource">
                <GroupingSettings CaseSensitive="False"/>
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" FileName="HCDC - Vat Tu Ton Kho" />
                <MasterTableView  DataKeyNames="MaVT" CommandItemDisplay="Top">
                    <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowRefreshButton="True" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="#" Groupable="False" AllowFiltering="false" ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblSoTT" runat="server" Text='<%# grdPartInStock.CurrentPageIndex * grdPartInStock.PageSize + grdPartInStock.Items.Count + 1 %>'>
                                </asp:Label>
          
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Mã vật tư" UniqueName="MaVT" DataField="MaVT" ShowFilterIcon="False" FilterControlWidth="99%" 
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                            <HeaderStyle HorizontalAlign="Center" Width="120" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%# Eval("MaVT")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderText="Tên vật tư" UniqueName="MoTa" DataField="MoTa" ShowFilterIcon="False" FilterControlWidth="99%" 
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                            <HeaderStyle HorizontalAlign="Center" Width="200" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%# Eval("MoTa")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridBoundColumn DataField="DVT"  HeaderText="Đơn Vị Tính" UniqueName="DVT" ReadOnly="True" ShowFilterIcon="False" FilterControlWidth="99%" 
                                                 AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                            <HeaderStyle HorizontalAlign="Center" Width="100" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="NhomChinh" HeaderText="Nhóm chính" UniqueName="NhomChinh" ReadOnly="True" ShowFilterIcon="False" FilterControlWidth="99%" 
                                                 AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                            <HeaderStyle HorizontalAlign="Center" Width="120" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="NhomCon"  HeaderText="Nhóm con" UniqueName="NhomCon" ReadOnly="True" ShowFilterIcon="False" FilterControlWidth="99%" 
                                                 AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                            <HeaderStyle HorizontalAlign="Center" Width="120" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="QCDongGoi"  HeaderText="Quy cách đóng gói" UniqueName="QCDongGoi" ReadOnly="True" ShowFilterIcon="False" FilterControlWidth="99%" 
                                                 AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                            <HeaderStyle HorizontalAlign="Center" Width="130" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="Kho"  HeaderText="Kho" UniqueName="Kho" ReadOnly="True" ShowFilterIcon="False" FilterControlWidth="99%" 
                                                 AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                            <HeaderStyle HorizontalAlign="Center" Width="90" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="ViTri"  HeaderText="Vị trí" UniqueName="ViTri" ReadOnly="True" ShowFilterIcon="False" FilterControlWidth="99%" 
                                                 AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                            <HeaderStyle HorizontalAlign="Center" Width="90" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="Lo"  HeaderText="Lô" UniqueName="Lo" ReadOnly="True" ShowFilterIcon="False" FilterControlWidth="99%" 
                                                 AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                            <HeaderStyle HorizontalAlign="Center" Width="90" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridDateTimeColumn DataField="HSD"  HeaderText="Hạn sử dụng" UniqueName="HSD" ReadOnly="True" FooterText="<b>Tổng Cộng:</b> " DataFormatString="{0:dd/MM/yyyy}" PickerType="DatePicker" AutoPostBackOnFilter="true" EnableRangeFiltering="true" FilterControlWidth="110" ShowFilterIcon="false" CurrentFilterFunction="Between" Display="False">
                            <HeaderStyle HorizontalAlign="Center" Width="300" />
                            <FooterStyle HorizontalAlign="Right" Font-Bold="True"></FooterStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridDateTimeColumn>
                        
                        <telerik:GridTemplateColumn HeaderText="Số lượng" UniqueName="SoLuong" DataField="SoLuong" FooterText="<b>Tổng số lượng:</b> " Aggregate="Sum" FooterAggregateFormatString="{0:N}" AllowFiltering="False">
                            <HeaderStyle HorizontalAlign="Center" Width="120" />
                            <FooterStyle HorizontalAlign="Right" Font-Bold="True"></FooterStyle>
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblSoLuong" Text='<%# Eval("SoLuong", "{0:N}") %>'/>
                            </ItemTemplate>
                            
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderText="Đơn giá" UniqueName="DonGia" DataField="DonGia" AllowFiltering="False">
                            <HeaderStyle HorizontalAlign="Center" Width="150" />
                            
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblDonGia" Text='<%# Eval("DonGia", "{0:N}") %>'/>
                            </ItemTemplate>
                            
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderText="Thành tiền" UniqueName="ThanhTien" DataField="ThanhTien" FooterText="<b>Tổng thành tiền:</b> " Aggregate="Sum" FooterAggregateFormatString="{0:N}" AllowFiltering="False">
                            <HeaderStyle HorizontalAlign="Center" Width="150" />
                            <FooterStyle HorizontalAlign="Right" Font-Bold="True"></FooterStyle>
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblThanhTien" Text='<%# Eval("ThanhTien", "{0:N}") %>'/>
                            </ItemTemplate>
                            
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings Selecting-AllowRowSelect="true">
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" FrozenColumnsCount="3"/>
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadPane>
    </telerik:RadSplitter>

        
        
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SQLConn %>"
                       ProviderName="System.Data.SqlClient" SelectCommand="EXEC getPAvailable;"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SQLConn %>"
                       ProviderName="System.Data.SqlClient" SelectCommand="EXEC getStoreCDC;"></asp:SqlDataSource>

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" >
            <Scripts>
            </Scripts>
        </telerik:RadScriptManager>
        
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="MetroTouch"/>
    <telerik:RadNotification RenderMode="Lightweight" ID="errorNotification" runat="server" Text="Initial text" Position="TopRight" Animation="Fade" EnableRoundedCorners="true" EnableShadow="true" VisibleOnPageLoad="False" Width="500" Title="Thông báo"  TitleIcon="../../Resources/Images/error_14.png" ContentIcon="../../Resources/Images/error_24.png" Skin="MetroTouch"  AnimationDuration="500" AutoCloseDelay="3000">
    </telerik:RadNotification>
    
    <telerik:RadNotification RenderMode="Lightweight" ID="completeNotification" runat="server" Text="Initial text" Position="TopRight" Animation="Fade" EnableRoundedCorners="true" EnableShadow="true" VisibleOnPageLoad="False" Width="500" Title="Thông báo"  TitleIcon="../../Resources/Images/complete_16.png" ContentIcon="../../Resources/Images/complete_24.png" Skin="MetroTouch"  AnimationDuration="500" AutoCloseDelay="3000">
    </telerik:RadNotification>
    <asp:HiddenField runat="server" ID="AllowDelete" Value="false"/>
    <asp:HiddenField runat="server" ID="AllowSave" Value="false"/>
    <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rtvPart" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="btnSearch">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rtvPart" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="radUpload">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="btnProcess" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="rtvPart">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rtvPart" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="lbMR">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="lblObjectName"/>
                <telerik:AjaxUpdatedControl ControlID="TabObjectDetail" LoadingPanelID="RadAjaxLoadingPanel2"/>
                <telerik:AjaxUpdatedControl ControlID="MultiPageObject" LoadingPanelID="RadAjaxLoadingPanel2"/>
                <telerik:AjaxUpdatedControl ControlID="grdPart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                <telerik:AjaxUpdatedControl ControlID="ViewToolBar"/>
                <telerik:AjaxUpdatedControl ControlID="ExportToolBar"/>
                <telerik:AjaxUpdatedControl ControlID="AllowSave"/>
                <telerik:AjaxUpdatedControl ControlID="AllowDelete"/>

            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="ViewToolBar">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="lblObjectName"/>
                <telerik:AjaxUpdatedControl ControlID="lbMR" LoadingPanelID="RadAjaxLoadingPanel2"/>
                <telerik:AjaxUpdatedControl ControlID="TabObjectDetail" LoadingPanelID="RadAjaxLoadingPanel2"/>
                <telerik:AjaxUpdatedControl ControlID="MultiPageObject" LoadingPanelID="RadAjaxLoadingPanel2"/>
                <telerik:AjaxUpdatedControl ControlID="AllowSave"/>
                <telerik:AjaxUpdatedControl ControlID="AllowDelete"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
                
        <telerik:AjaxSetting AjaxControlID="btnSearch">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="lbMR" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="ddlOrg">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="ddlStore" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="grdPart">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdPart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                <telerik:AjaxUpdatedControl ControlID="grdPartInStock" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="grdPartInStock">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdPartInStock" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="btnProcess">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="ddlStatus" LoadingPanelID="RadAjaxLoadingPanel2"/>
                <telerik:AjaxUpdatedControl ControlID="errorNotification"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script src="../../Resources/Scripts/jquery-1.9.1.js"></script>
            <script src="../../Resources/Scripts/bootstrap.min.js"></script>
            
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

                
            </script>
        </telerik:RadCodeBlock>
        
    </form>
</body>
</html>
