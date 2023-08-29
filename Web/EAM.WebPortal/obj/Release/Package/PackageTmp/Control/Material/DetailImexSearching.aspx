<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetailImexSearching.aspx.cs" Inherits="EAM.WebPortal.Control.Material.DetailImexSearching" %>
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

        .rtOut {
            display: flex !important;
        }

        .rtIn {
            margin-top: -7px;
        }

        .rpOut {
            color: white !important;
            
            outline: 0;
            background-image: none;
            background-color: #50535a;
        }

        #RAD_SPLITTER_PANE_CONTENT_Radpane4 {
            overflow: hidden !important;
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
                            <span style="color: dodgerblue; font-size: 16px; font-weight: normal; font-family: helvetica, arial, verdana, sans-serif;">Tra cứu Sổ chi tiết nhập xuất: </span>
                        </ItemTemplate>
                    </telerik:RadToolBarButton>
                            
                </Items>
            </telerik:RadToolBar>
        </telerik:RadPane>
        <telerik:RadPane ID="Radpane2" runat="server" Width="100%" ShowContentDuringLoad="False">
            
            

            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Height="100%" Width="100%" Orientation="Vertical" Skin="Silk">
                <telerik:RadPane ID="Radpane1" runat="server" Width="300" ShowContentDuringLoad="False">
                    
                    <telerik:RadPanelBar ID="radPbSearch" runat="server" Width="100%" Height="100%">
                        <Items>
                            <telerik:RadPanelItem Text="TỪ NGÀY - ĐẾN NGÀY" runat="server" Expanded="True" Width="100%">
                                <ContentTemplate>
                                    <telerik:RadDatePicker ID="txtFromDate"  runat="server" ShowPopupOnFocus="True"  Culture="English (United States)" Width="120">
                                        <DateInput runat="server" DateFormat="dd-MM-yyyy"/>
                                    </telerik:RadDatePicker>
                                    -
                                    <telerik:RadDatePicker ID="txtToDate"  runat="server" ShowPopupOnFocus="True"  Culture="English (United States)" Width="120">
                                        <DateInput runat="server" DateFormat="dd-MM-yyyy"/>
                                    </telerik:RadDatePicker>
                                </ContentTemplate>
                            </telerik:RadPanelItem>
                    
                            <telerik:RadPanelItem Text="KHO VẬT TƯ" runat="server" Expanded="True" Width="100%">
                                <ContentTemplate>
                                    <telerik:RadComboBox RenderMode="Lightweight" ID="ddlStore" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="100%" EmptyMessage="Chọn Kho" Skin="Metro">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="CD" Value="CD" />
                                            <telerik:RadComboBoxItem Text="TAITRO" Value="TAITRO" />
                                            <telerik:RadComboBoxItem Text="CAP" Value="CAP" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </ContentTemplate>
                            </telerik:RadPanelItem>
                    
                            <telerik:RadPanelItem Text="DANH MỤC VẬT TƯ" runat="server" Expanded="True" Width="100%">
                                <ContentTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtSearch" runat="server"
                                                        EmptyMessage="Tìm kiếm vật tư" InvalidStyleDuration="100" >
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnSearch" runat="server" ImageAlign="AbsMiddle" CssClass="styleIcon"
                                                     ImageUrl="~/Resources/Images/search20.png" OnClick="btnSearch_OnClick"/>
                                    <telerik:RadTreeView RenderMode="Lightweight" ID="rtvPart" runat="server"  CheckBoxes="True" 
                                                         TriStateCheckBoxes="true" CheckChildNodes="true" Skin="Silk" OnNodeCheck="rtvPart_OnNodeCheck" Style="white-space: normal;" Height="300" >
                                    </telerik:RadTreeView>
                                    <br/>
                                    <div align="center" style="width: 100%; margin-top: 10px">
                                        <telerik:RadButton ID="btnClear" runat="server" Text="Xóa Điều Kiện Lọc" OnClick="btnClear_OnClick" Enabled="True">
                                            <Icon PrimaryIconUrl="~/Resources/Images/clear_16.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"/>
                                        </telerik:RadButton>

                                        <telerik:RadButton ID="btnSearchFull" runat="server" Text="Lọc Dữ Liệu" OnClick="btnSearchFull_OnClick" Enabled="True">
                                            <Icon PrimaryIconUrl="~/Resources/Images/process16.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"/>
                                        </telerik:RadButton>
                                    </div>
                                </ContentTemplate>
                            </telerik:RadPanelItem>
                        </Items>

                    </telerik:RadPanelBar>

                    
                </telerik:RadPane>
                
                <telerik:RadPane ID="Radpane4" runat="server"  ShowContentDuringLoad="False">
                    <telerik:RadGrid RenderMode="Lightweight" ID="grdPartInStock" GridLines="None" runat="server"  Skin="Metro" Height="100%"  PageSize="100"  AllowAutomaticUpdates="True" AllowPaging="False" AutoGenerateColumns="False" ShowFooter="True" AllowFilteringByColumn="True"
                                OnNeedDataSource="grdPartInStock_OnNeedDataSource">
                        <GroupingSettings CaseSensitive="False"/>
                        <ExportSettings ExportOnlyData="True" IgnorePaging="True" FileName="HCDC - So chi tiet nhap xuat" />
                        <MasterTableView  DataKeyNames="CODE" CommandItemDisplay="Top" ShowGroupFooter="true">
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldAlias="." FieldName="PartFullName" FormatString="{0:D}"
                                                                  HeaderValueSeparator=" "></telerik:GridGroupByField>
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="PartFullName"></telerik:GridGroupByField>
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>
                            
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="Nhập" Name="Nhap" HeaderStyle-HorizontalAlign="Center"/>
                                <telerik:GridColumnGroup HeaderText="Xuất" Name="Xuat" HeaderStyle-HorizontalAlign="Center"/>
                                <telerik:GridColumnGroup HeaderText="Tồn" Name="Ton" HeaderStyle-HorizontalAlign="Center"/>
                                
                                
                            </ColumnGroups>

                            <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowRefreshButton="True" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="#" Groupable="False" AllowFiltering="false" ReadOnly="True">
                                    <HeaderStyle HorizontalAlign="Center" Width="40" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="40"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSoTT" runat="server" Text='<%# grdPartInStock.CurrentPageIndex * grdPartInStock.PageSize + grdPartInStock.Items.Count + 1 %>'>
                                        </asp:Label>
                  
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Chứng từ" UniqueName="CODE" DataField="CODE" ShowFilterIcon="False" FilterControlWidth="99%" 
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                    <HeaderStyle HorizontalAlign="Center" Width="100" />
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <%# Eval("CODE")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                
                                <telerik:GridDateTimeColumn DataField="TRA_DATE"  HeaderText="Ngày" UniqueName="TRA_DATE" ReadOnly="True"  DataFormatString="{0:dd/MM/yyyy}" PickerType="DatePicker" AutoPostBackOnFilter="true" EnableRangeFiltering="true" FilterControlWidth="110" ShowFilterIcon="false" CurrentFilterFunction="Between" AllowFiltering="False">
                                    <HeaderStyle HorizontalAlign="Center" Width="100" />
                                    <FooterStyle HorizontalAlign="Right" Font-Bold="True"></FooterStyle>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridDateTimeColumn>

                                
                                <telerik:GridTemplateColumn HeaderText="Diễn giải" UniqueName="DESCRIPTION" DataField="DESCRIPTION" ShowFilterIcon="False" FilterControlWidth="99%" 
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                    <HeaderStyle HorizontalAlign="Center" Width="200" />
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <%# Eval("DESCRIPTION")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                
                                <telerik:GridBoundColumn DataField="par_uom"  HeaderText="Đơn Vị Tính" UniqueName="par_uom" ReadOnly="True" ShowFilterIcon="False" FilterControlWidth="99%" 
                                                         AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                    <HeaderStyle HorizontalAlign="Center" Width="100" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                
                                
                                <telerik:GridBoundColumn DataField="Store"  HeaderText="Kho" UniqueName="Kho" ReadOnly="True" ShowFilterIcon="False" FilterControlWidth="99%" 
                                                         AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                    <HeaderStyle HorizontalAlign="Center" Width="60" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridTemplateColumn HeaderText="Đơn giá" UniqueName="TRL_PRICE" DataField="TRL_PRICE"  AllowFiltering="False" FooterText="<b>Tổng cộng:</b> ">
                                    <HeaderStyle HorizontalAlign="Center" Width="120" />
                                    <FooterStyle HorizontalAlign="Right" Font-Bold="True"></FooterStyle>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblDonGia" Text='<%# Eval("TRL_PRICE", "{0:N}") %>'/>
                                    </ItemTemplate>
                                    
                                </telerik:GridTemplateColumn>
                                
                                <telerik:GridTemplateColumn HeaderText="Số lượng" UniqueName="SLNHAP" DataField="SLNHAP"  AllowFiltering="False" ColumnGroupName="Nhap" Aggregate="Sum" FooterAggregateFormatString="{0:N}">
                                    <HeaderStyle HorizontalAlign="Center" Width="120" />
                                    <FooterStyle HorizontalAlign="Right" Font-Bold="True"></FooterStyle>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSLNhap" Text='<%# Eval("SLNHAP", "{0:N}") %>'/>
                                    </ItemTemplate>
                                    
                                </telerik:GridTemplateColumn>
                                
                                <telerik:GridTemplateColumn HeaderText="Thành tiền" UniqueName="THANHTIENNHAP" DataField="THANHTIENNHAP"  AllowFiltering="False" ColumnGroupName="Nhap" Aggregate="Sum" FooterAggregateFormatString="{0:N}">
                                    <HeaderStyle HorizontalAlign="Center" Width="140" />
                                    <FooterStyle HorizontalAlign="Right" Font-Bold="True"></FooterStyle>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblThanhTienNhap" Text='<%# Eval("THANHTIENNHAP", "{0:N}") %>'/>
                                    </ItemTemplate>
                                    
                                </telerik:GridTemplateColumn>
                                
                                <telerik:GridTemplateColumn HeaderText="Số lượng" UniqueName="SLXUAT" DataField="SLXUAT"  AllowFiltering="False" ColumnGroupName="Xuat" Aggregate="Sum" FooterAggregateFormatString="{0:N}">
                                    <HeaderStyle HorizontalAlign="Center" Width="120" />
                                    <FooterStyle HorizontalAlign="Right" Font-Bold="True"></FooterStyle>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSLXuat" Text='<%# Eval("SLXUAT", "{0:N}") %>'/>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                
                                <telerik:GridTemplateColumn HeaderText="Thành tiền" UniqueName="THANHTIENXUAT" DataField="THANHTIENXUAT"  AllowFiltering="False" ColumnGroupName="Xuat" Aggregate="Sum" FooterAggregateFormatString="{0:N}">
                                    <HeaderStyle HorizontalAlign="Center" Width="140" />
                                    <FooterStyle HorizontalAlign="Right" Font-Bold="True"></FooterStyle>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblThanhTienXuat" Text='<%# Eval("THANHTIENXUAT", "{0:N}") %>'/>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                
                                <telerik:GridTemplateColumn HeaderText="Số lượng" UniqueName="STOCKAFTER" DataField="STOCKAFTER"  AllowFiltering="False" ColumnGroupName="Ton" >
                                    <HeaderStyle HorizontalAlign="Center" Width="120" />
                                    <FooterStyle HorizontalAlign="Right" Font-Bold="True"></FooterStyle>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbSLTon" Text='<%# Eval("STOCKAFTER", "{0:N}") %>'/>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                
                                <telerik:GridTemplateColumn HeaderText="Thành tiền" UniqueName="STOCKVAlUEAFTER" DataField="STOCKVAlUEAFTER"  AllowFiltering="False" ColumnGroupName="Ton" >
                                    <HeaderStyle HorizontalAlign="Center" Width="140" />
                                    <FooterStyle HorizontalAlign="Right" Font-Bold="True"></FooterStyle>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblThanhTienTon" Text='<%# Eval("STOCKVAlUEAFTER", "{0:N}") %>'/>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                
                                
                                <%--<telerik:GridTemplateColumn HeaderText="Số lượng" UniqueName="SoLuong" DataField="SoLuong" FooterText="<b>Tổng số lượng:</b> " Aggregate="Sum" FooterAggregateFormatString="{0:N}" AllowFiltering="False">
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
                                    
                                </telerik:GridTemplateColumn>--%>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings Selecting-AllowRowSelect="true">
                            <Selecting AllowRowSelect="true" />
                            <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" FrozenColumnsCount="3"/>
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadPane>
            </telerik:RadSplitter>

            
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
        
        <telerik:AjaxSetting AjaxControlID="btnSearchFull">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdPartInStock" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="btnClear">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdPartInStock" LoadingPanelID="RadAjaxLoadingPanel2"/>
                <telerik:AjaxUpdatedControl ControlID="txtSearch" LoadingPanelID="RadAjaxLoadingPanel2"/>
                <telerik:AjaxUpdatedControl ControlID="rtvPart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                <telerik:AjaxUpdatedControl ControlID="ddlStore" LoadingPanelID="RadAjaxLoadingPanel2"/>
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
        
        <telerik:AjaxSetting AjaxControlID="btnSearch">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rtvPart" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="rtvPart">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rtvPart" LoadingPanelID="RadAjaxLoadingPanel2"/>
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
