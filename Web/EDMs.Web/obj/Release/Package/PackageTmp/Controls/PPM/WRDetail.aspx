<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WRDetail.aspx.cs" Inherits="EDMs.Web.Controls.PPM.WRDetail" EnableViewState="true" %>
 
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
        
        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdWRActivitiesPanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdWRActivities_GridData {
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

        .RadAjaxPanel {
            display: inline !important;
        }
        </style>

    <div style=" width: 100%; height: 100%; background-color: #f0f0f0 ">
        <telerik:RadSplitter RenderMode="Lightweight" ID="MainSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
            <telerik:RadPane ID="Radpane1" runat="server" Width="100%" Height="40" ShowContentDuringLoad="False" Scrollable="false" Scrolling="None">
                <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ToolBarObject" EnableRoundedCorners="true" EnableShadows="true" Skin="Metro" Height="40">
                    <Items>
                        <telerik:RadToolBarButton Text="Left" Group="Align">
                            <ItemTemplate>
                                <span style="                                        color: dodgerblue;
                                        font-size: 16px;
                                        font-weight: normal;
                                        font-family: helvetica, arial, verdana, sans-serif;
">Yêu cầu công việc: </span>
                                
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
                                                    EmptyMessage="Search within All Work request" InvalidStyleDuration="100">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="btnSearch" runat="server" ImageAlign="AbsMiddle" CssClass="styleIcon"
                                           ImageUrl="~/Images/search20.png" OnClick="btnSearch_OnClick"/>
                            </telerik:RadPane>
                            <telerik:RadPane ID="Radpane4" runat="server" Width="100%" >
                                <telerik:RadListBox RenderMode="Lightweight" runat="server" ID="lbObject" Height="100%" Width="100%" LoadingPanelID="<%# RadAjaxLoadingPanel2.ClientID %>" EnableLoadOnDemand="False" Skin="Metro" OnSelectedIndexChanged="lbObject_OnSelectedIndexChanged" AutoPostBack="True">
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
                    <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
                        <telerik:RadPane ID="Radpane5" runat="server" Width="100%" Height="35">
                            <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ViewToolBar" EnableRoundedCorners="true" EnableShadows="true" Skin="Metro" Height="35" OnClientButtonClicking="ViewToolBar_OnClientButtonClicking" OnButtonClick="ViewToolBar_OnButtonClick">
                                <Items>
                                    <telerik:RadToolBarButton  ImageUrl="~/Images/expand_left.png" ToolTip="Mở rộng bên trái" Value="ExpandLeft"/>
                                    <telerik:RadToolBarButton  ImageUrl="~/Images/toolbar_split.png" ToolTip="Chế độ xem chia đôi" Value="SplitView"/>
                                    <telerik:RadToolBarButton  ImageUrl="~/Images/expand_right.png" ToolTip="Mở rộng bên phải" Value="ExpandRight" />
                                    <telerik:RadToolBarButton  IsSeparator="true"/>
                                    <telerik:RadToolBarButton  ImageUrl="~/Images/toolbar_new.png" ToolTip="Tạo mới yêu cầu" Value="Insert"/>
                                    <telerik:RadToolBarButton  ImageUrl="~/Images/toolbar_save.png" ToolTip="Cập nhật yêu cầu" Value="Update" Enabled="False"/>
                                    <telerik:RadToolBarButton  ImageUrl="~/Images/toolbar_delete.png" ToolTip="Xóa yêu cầu" Value="Delete" Enabled="False"/>
                                    <telerik:RadToolBarButton  ImageUrl="~/Images/toolbar_reset.png" ToolTip="Reset" Value="Refresh"/>
                            
                                </Items>
                            </telerik:RadToolBar>
                        </telerik:RadPane>
                        
                        <telerik:RadPane ID="DetailPanel" runat="server" Width="100%" >
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
                                    <td align="right" style="width: 150px; padding-right: 2px">YCCV:<img src="../../Images/field-indicators.png" height="16px"/></td>
                                    <td>
                                        <telerik:RadTextBox ID="txtPhieuCV" runat="server" Skin="Metro" Width="80" ReadOnly="True"/>
                                        <telerik:RadTextBox ID="txtDienGiai" runat="server" Skin="Metro" Width="320" EmptyMessage="Tên yêu cầu" Enabled="False"/>
                                    </td>
                                    
                                    <td align="right" style="width: 150px; padding-right: 2px">Đơn vị:</td>
                                    <td>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlOrganization" AllowCustomText="true" runat="server" Width="150" Height="100" DropDownWidth="300"  EmptyMessage="Tìm đơn vị..." Skin="Metro" Filter="Contains" MarkFirstMatch="True" Enabled="False">
                                        </telerik:RadComboBox>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 150px; padding-right: 2px">Thiết bị:<img src="../../Images/field-indicators.png" height="16px"/></td>
                                    <td>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlEquipment" AllowCustomText="true" runat="server" Width="400" Height="100"   EmptyMessage="Tìm mã thiết bị..." Skin="Metro" Filter="Contains" MarkFirstMatch="True" Enabled="False" OnSelectedIndexChanged="ddlEquipment_OnSelectedIndexChanged" AutoPostBack="True">
                                        </telerik:RadComboBox>
                                        
                                    </td>
                                    <td align="right" style="width: 150px; padding-right: 2px">Người lập:</td>
                                    <td>
                                        <telerik:RadTextBox ID="txtNguoiLap" runat="server" Skin="Metro" Width="150" ReadOnly="True" Enabled="False"/>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td align="right" style="width: 150px; padding-right: 2px">Bộ phận:<img src="../../Images/field-indicators.png" height="16px"/></td>
                                    <td>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlDepartment" AllowCustomText="true" runat="server" Width="400" Height="100"  EmptyMessage="Tìm bộ phận..." Skin="Metro" Filter="Contains" MarkFirstMatch="True" Enabled="False">
                                        </telerik:RadComboBox>
                                        
                                    </td>
                                    <td align="right" style="width: 150px; padding-right: 2px">Ngày lập:</td>
                                    <td>
                                        <telerik:RadTextBox ID="txtNgayLap" runat="server" Skin="Metro" Width="150" ReadOnly="True" Enabled="False"/>
                                    </td>
                                    
                                </tr>
                                
                                <tr>
                                    <td align="right" style="width: 150px; padding-right: 2px"></td>
                                    <td>
                                        
                                    </td>
                                    <td align="right" style="width: 150px; padding-right: 2px">Ngày báo cáo:<img src="../../Images/field-indicators.png" height="16px"/></td>
                                    <td>
                                        <telerik:RadDatePicker ID="txtNgayBaoCao" runat="server" Skin="Metro" Width="150" Enabled="False">
                                            <DateInput DateFormat="dd-MMM-yy"/>
                                        </telerik:RadDatePicker>
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
                                    <td align="right" style="width: 150px; padding-right: 2px">Kiểu:<img src="../../Images/field-indicators.png" height="16px"/></td>
                                    <td>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlKieu" AllowCustomText="true" runat="server" Width="200" EmptyMessage="Tìm kiểu..." Skin="Metro" Filter="Contains" MarkFirstMatch="True" Enabled="False">
                                            <Items>
                                                <telerik:RadComboBoxItem runat="server" Value="1" Text="Hiệu chuẩn"/>
                                                <telerik:RadComboBoxItem runat="server" Value="2" Text="Mặc định"/>
                                                <telerik:RadComboBoxItem runat="server" Value="3" Text="Sửa chữa vật tư"/>
                                                <telerik:RadComboBoxItem runat="server" Value="4" Text="Sự cố"/>
                                                <telerik:RadComboBoxItem runat="server" Value="5" Text="Thay mới"/>
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                    <td align="right" style="width: 150px; padding-right: 2px">Ngày bắt đầu - KH:</td>
                                    <td>
                                        <telerik:RadDatePicker ID="txtNgayBatDauKH" runat="server" Skin="Metro" Width="150" Enabled="False">
                                            <DateInput DateFormat="dd-MMM-yy"/>
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>

                                <tr>
                                    <td align="right" style="width: 150px; padding-right: 2px">Phân loại:</td>
                                    <td>
                                        <telerik:RadTextBox ID="txtPhanLoai" runat="server" Skin="Metro" Width="200" Enabled="False"/>
                                    </td>
                                    <td align="right" style="width: 150px; padding-right: 2px">Mã vấn đề:</td>
                                    <td>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlMaVanDe" AllowCustomText="true" runat="server" Width="150" Height="100"  EmptyMessage="Tìm mã vấn đề..." Skin="Metro" Filter="Contains" MarkFirstMatch="True" DropDownWidth="300" Enabled="False">
                                            
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td align="right" style="width: 150px; padding-right: 2px">Tình trạng:<img src="../../Images/field-indicators.png" height="16px"/></td>
                                    <td>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlTinhTrang" AllowCustomText="true" runat="server" Width="200" EmptyMessage="Tìm tình trạng..." Skin="Metro" Filter="Contains" MarkFirstMatch="True" Enabled="False">
                                            <Items>
                                                <telerik:RadComboBoxItem runat="server" Value="1" Text="Yêu cầu sửa chữa"/>
                                                <telerik:RadComboBoxItem runat="server" Value="2" Text="Từ chối thực hiện"/>
                                                <telerik:RadComboBoxItem runat="server" Value="3" Text="Đã phát hành"/>
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                    <td align="right" style="width: 150px; padding-right: 2px">Phân công đến:</td>
                                    <td>
                                        <telerik:RadTextBox ID="txtPhanCongDen" runat="server" Skin="Metro" Width="150" Enabled="False"/>
                                    </td>
                                </tr>
                             <tr>
                                 <td align="right" style="width: 150px; padding-right: 2px">Ưu tiên:</td>
                                 <td>
                                     <telerik:RadComboBox RenderMode="Lightweight" ID="ddlDoUuTien" AllowCustomText="true" runat="server" Width="200" EmptyMessage="Tìm độ ưu tiên..." Skin="Metro" Filter="Contains" MarkFirstMatch="True" Enabled="False">
                                         <Items>
                                             <telerik:RadComboBoxItem runat="server" Value="1" Text="Cao"/>
                                             <telerik:RadComboBoxItem runat="server" Value="2" Text="Mặc định"/>
                                             <telerik:RadComboBoxItem runat="server" Value="3" Text="Trung bình"/>
                                             <telerik:RadComboBoxItem runat="server" Value="4" Text="Thấp"/>
                                         </Items>
                                     </telerik:RadComboBox>
                                 </td>
                                 <td align="right" style="width: 150px; padding-right: 2px">Dự kiến hoàn thành:</td>
                                 <td>
                                     <telerik:RadTextBox ID="txtDuKienHoanThanh" runat="server" Skin="Metro" Width="150" Enabled="False"/>
                                 </td>
                             </tr>

                                <tr>
                                    <td colspan="5">
                                        <dl class="accordion">
                                            <dt style="width: 100%;">
                                                <span>Diễn giải sự cố</span>
                                            </dt>
                                        </dl>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td align="right" style="width: 150px; padding-right: 2px">Chi tiết:<img src="../../Images/field-indicators.png" height="16px"/></td>
                                    <td colspan="3">
                                        <telerik:RadTextBox ID="txtChiTiet" runat="server" Skin="Metro" Width="700" TextMode="MultiLine" Rows="6"  Enabled="False"/>
                                    </td>
                                </tr>
                                 <tr>
                                     <td align="right" style="width: 150px; padding-right: 2px">Chỉ đạo:</td>
                                     <td colspan="3">
                                         <telerik:RadTextBox ID="txtChiDao" runat="server" Skin="Metro" Width="700" TextMode="MultiLine" Rows="4"  Enabled="False"/>
                                     </td>
                                 </tr>
                                
                                 <tr>
                                     <td colspan="5">
                                         <dl class="accordion">
                                             <dt style="width: 100%;">
                                                 <span>Tài liệu đính kèm</span>
                                             </dt>
                                         </dl>
                                     </td>
                                 </tr>
                                 
                                 <tr>
                                     <td align="right" style="width: 150px; padding-right: 2px">Chọn file:</td>
                                     <td colspan="3">
                                         <telerik:RadAsyncUpload runat="server" ID="radUpload"  Skin="Metro" Enabled="False">
                                             <Localization Select="Chọn File"></Localization>
                                         </telerik:RadAsyncUpload>
                                     </td>
                                 </tr>
                                 
                                 <tr>
                                     <td align="right" style="width: 150px; padding-right: 2px">Chọn file:</td>
                                     <td colspan="3">
                                         <telerik:RadGrid RenderMode="Lightweight" ID="grdAttachFile" runat="server" AllowPaging="False" AllowSorting="False" AllowFilteringByColumn="False" CellSpacing="0" GridLines="None" Skin="Metro" Height="110" Width="700"
                                                         OnDeleteCommand="grdAttachFile_DeleteCommand"
                                                         OnNeedDataSource="grdAttachFile_OnNeedDataSource">
                                        <GroupingSettings CaseSensitive="false" />
                                        <MasterTableView AutoGenerateColumns="false" TableLayout="Fixed" ClientDataKeyNames="ID" DataKeyNames="ID">
                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />                                        
                                                        <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" ConfirmText="Bạn có muốn xóa tài liệu này?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                                                            <HeaderStyle Width="30" />
                                                                <ItemStyle HorizontalAlign="Center" Width="5%"  />
                                                        </telerik:GridButtonColumn>

                                                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                                                            <HeaderStyle Width="35" />
                                                            <ItemStyle HorizontalAlign="Center" Width="5%"/>
                                                            <ItemTemplate>
                                                                <a href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' 
                                                                    download='<%# DataBinder.Eval(Container.DataItem, "Filename") %>' target="_blank">
                                                                    <asp:Image ID="Image1" runat="server" ImageUrl='~/Images/document.png'
                                                                        Style="cursor: pointer;" ToolTip="Download document" /> 
                                                                </a>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn DataField="Filename" HeaderText="Tên file" UniqueName="Filename">
                                                            <HeaderStyle HorizontalAlign="Center" Width="300" />
                                                            <ItemStyle HorizontalAlign="Left"/>
                                                        </telerik:GridBoundColumn>
                                                        
                                                        <telerik:GridBoundColumn DataField="CreatedByName" HeaderText="Người tạo" UniqueName="CreatedByName">
                                                            <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </telerik:GridBoundColumn>
                                                
                                                        <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Giờ tạo" UniqueName="CreatedDate"
                                                            DataFormatString="{0:dd-MM-yy hh:mm tt}" >
                                                            <HeaderStyle HorizontalAlign="Center" Width="130" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </telerik:GridBoundColumn>
                                                
                                                        <telerik:GridBoundColumn DataField="FileSize" HeaderText="File size(Kb)" UniqueName="FileSize" DataFormatString="{0:0,0.00}" Display="False">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                        </MasterTableView>
                                        </telerik:RadGrid>
                                     </td>
                                 </tr>
                                
                            </table>
                        </telerik:RadPane>
                        
                    </telerik:RadSplitter>

                       
                        
                    </telerik:RadPane>
                </telerik:RadSplitter>
            </telerik:RadPane>
        </telerik:RadSplitter>
    <asp:HiddenField runat="server" ID="IsDelete" Value="false"/>

        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="Metro">
        </telerik:RadAjaxLoadingPanel>
    <telerik:RadNotification RenderMode="Lightweight" ID="errorNotification" runat="server" Text="Initial text" Position="TopRight" Animation="Fade" EnableRoundedCorners="true" EnableShadow="true" VisibleOnPageLoad="False" Width="500" Title="Thông báo"  TitleIcon="~/Images/error_14.png" ContentIcon="Resources/Images/error_24.png" Skin="MetroTouch"  AnimationDuration="500" AutoCloseDelay="3000">
    </telerik:RadNotification>
    
    <telerik:RadNotification RenderMode="Lightweight" ID="completeNotification" runat="server" Text="Initial text" Position="TopRight" Animation="Fade" EnableRoundedCorners="true" EnableShadow="true" VisibleOnPageLoad="False" Width="500" Title="Thông báo"  TitleIcon="~/Images/complete_16.png" ContentIcon="Resources/Images/complete_24.png" Skin="MetroTouch"  AnimationDuration="500" AutoCloseDelay="3000">
    </telerik:RadNotification>
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
                        <telerik:AjaxUpdatedControl ControlID="lbObject" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ViewToolBar"/>
                        <telerik:AjaxUpdatedControl ControlID="txtDienGiai"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPhieuCV"/>
                        <telerik:AjaxUpdatedControl ControlID="txtNguoiLap"/>
                        <telerik:AjaxUpdatedControl ControlID="txtNgayLap"/>
                        <telerik:AjaxUpdatedControl ControlID="txtNgayBaoCao"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPhanCongDen"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPhanLoai"/>
                        <telerik:AjaxUpdatedControl ControlID="txtDuKienHoanThanh"/>
                        <telerik:AjaxUpdatedControl ControlID="txtChiDao"/>
                        <telerik:AjaxUpdatedControl ControlID="txtChiTiet"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlDepartment"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlEquipment"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlKieu"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlMaVanDe"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlOrganization"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlTinhTrang"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlDoUuTien"/>
                        <telerik:AjaxUpdatedControl ControlID="grdAttachFile"/>
                        <telerik:AjaxUpdatedControl ControlID="radUpload"/>
                        <telerik:AjaxUpdatedControl ControlID="lblObjectName"/>
                        <telerik:AjaxUpdatedControl ControlID="txtNgayBatDauKH"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lbObject" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlEquipment">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlDepartment" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ViewToolBar">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ViewToolBar"/>
                        <telerik:AjaxUpdatedControl ControlID="txtDienGiai"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPhieuCV"/>
                        <telerik:AjaxUpdatedControl ControlID="txtNguoiLap"/>
                        <telerik:AjaxUpdatedControl ControlID="txtNgayLap"/>
                        <telerik:AjaxUpdatedControl ControlID="txtNgayBaoCao"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPhanCongDen"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPhanLoai"/>
                        <telerik:AjaxUpdatedControl ControlID="txtDuKienHoanThanh"/>
                        <telerik:AjaxUpdatedControl ControlID="txtChiDao"/>
                        <telerik:AjaxUpdatedControl ControlID="txtChiTiet"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlDepartment"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlEquipment"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlKieu"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlMaVanDe"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlOrganization"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlTinhTrang"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlDoUuTien"/>
                        <telerik:AjaxUpdatedControl ControlID="grdAttachFile"/>
                        <telerik:AjaxUpdatedControl ControlID="radUpload"/>
                        <telerik:AjaxUpdatedControl ControlID="lblObjectName"/>
                        <telerik:AjaxUpdatedControl ControlID="lbObject"/>
                        <telerik:AjaxUpdatedControl ControlID="txtNgayBatDauKH"/>
                        
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
                    window.location.href = "WRList.aspx";
                }
                else if (strValue == "Refresh") {
                    location.reload();
                }
                else if (strValue == "Delete") {
                    if (confirm('Bạn có muốn xóa yêu cầu này không!?') === false) {
                        return;
                    }

                    document.getElementById('<%= IsDelete.ClientID%>').value = "true";
                }
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>