<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EAM.WebPortal.Default" EnableViewState="true" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2019.3.1023.45, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        html, body {
            height: 100%;
            margin: 0px;
            font: normal 15px helvetica, arial, verdana, sans-serif;
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
            font-size: 1.25em;
            line-height: 1.6em;
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
            font-size: 1.1em;
            line-height: 1.5em;
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

        #RAD_SPLITTER_PANE_CONTENT_ctl00_ContentPlaceHolder2_Radpane1 {
            display: flex !important;
        }

        .RadInput .riTextBox {
            height: 25px !important;
        }

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_lblDuTruPanel {
            display: inline !important;
        }

        </style>

    <div style=" width: 100%; height: 100%; background-color: #f0f0f0 ">
        <telerik:RadSplitter RenderMode="Lightweight" ID="MainSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
            <telerik:RadPane ID="Radpane1" runat="server" Width="100%" Height="38" ShowContentDuringLoad="False">
                
                <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ToolBarObject" EnableRoundedCorners="true" EnableShadows="true" Skin="MetroTouch"  Width="350">
                    <Items>
                        <telerik:RadToolBarButton Text="Left" Group="Align">
                            <ItemTemplate>
                                <span style="color: dodgerblue; font-size: 16px; font-weight: normal; font-family: helvetica, arial, verdana, sans-serif;">Yêu cầu cấp vật tư: </span>
                                
                                <span style="color: black;font-size: 16px;font-weight: normal;font-family: helvetica, arial, verdana, sans-serif;"><asp:Label runat="server" ID="lblObjectName" Visible="False"></asp:Label> </span>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        
                    </Items>
                </telerik:RadToolBar>
                <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ExportToolBar" EnableRoundedCorners="true" EnableShadows="true" Skin="MetroTouch"  Width="38" OnButtonClick="ExportToolBar_OnButtonClick">
                    <Items>
                        <telerik:RadToolBarButton  ImageUrl="Resources/Images/excel.png" ToolTip="Tải file tổng hợp" Value="ExportExcel"/>
                    </Items>
                </telerik:RadToolBar>
                <telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="ViewToolBar" EnableRoundedCorners="true" EnableShadows="true" Skin="MetroTouch" OnClientButtonClicking="ViewToolBar_OnClientButtonClicking" OnButtonClick="ViewToolBar_OnButtonClick">
                        <Items>
                            <telerik:RadToolBarButton  ImageUrl="Resources/Images/expand_left.png" ToolTip="Mở rộng bên trái" Value="ExpandLeft"/>
                            <telerik:RadToolBarButton  ImageUrl="Resources/Images/toolbar_split.png" ToolTip="Chế độ xem chia đôi" Value="SplitView"/>
                            <telerik:RadToolBarButton  ImageUrl="Resources/Images/expand_right.png" ToolTip="Mở rộng bên phải" Value="ExpandRight" Visible="False"/>
                            <telerik:RadToolBarButton  IsSeparator="true"/>
                            <telerik:RadToolBarButton  ImageUrl="Resources/Images/toolbar_new.png" ToolTip="Tạo mới yêu cầu" Value="Insert"/>
                            <telerik:RadToolBarButton  ImageUrl="Resources/Images/toolbar_save.png" ToolTip="Cập nhật yêu cầu" Value="Update" Enabled="False"/>
                            <telerik:RadToolBarButton  ImageUrl="Resources/Images/toolbar_delete.png" ToolTip="Xóa yêu cầu" Value="Delete" Enabled="False"/>
                            <telerik:RadToolBarButton  ImageUrl="Resources/Images/toolbar_reset.png" ToolTip="Reset" Value="Refresh"/>
                            
                        </Items>
                    </telerik:RadToolBar>
                    </telerik:RadPane>
            <telerik:RadPane ID="Radpane2" runat="server" Width="100%" ShowContentDuringLoad="False">
                <telerik:RadSplitter RenderMode="Lightweight" ID="SubSplitter" runat="server" Height="100%" Width="100%" Skin="Silk">
                    <telerik:RadPane ID="PanelObjectList" runat="server" Width="350" MinWidth="100">
                        <telerik:RadSplitter RenderMode="Lightweight" ID="LeftSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
                            <telerik:RadPane ID="Radpane3" runat="server" Width="100%" Height="27">
                                <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtSearch" runat="server"
                                                    EmptyMessage="Tìm kiếm yêu cầu cấp vật tư" InvalidStyleDuration="100">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="btnSearch" runat="server" ImageAlign="AbsMiddle" CssClass="styleIcon"
                                           ImageUrl="Resources/Images/search20.png" OnClick="btnSearch_OnClick"/>
                            </telerik:RadPane>
                            <telerik:RadPane ID="Radpane4" runat="server" Width="100%" >
                                <telerik:RadListBox RenderMode="Lightweight" runat="server" ID="lbMR" Height="100%" Width="100%" LoadingPanelID="<%# RadAjaxLoadingPanel2.ClientID %>" EnableLoadOnDemand="False" Skin="MetroTouch" OnSelectedIndexChanged="lbMR_OnSelectedIndexChanged" AutoPostBack="True">
                                    <ItemTemplate>
                                        <span class="ObjectTitle"><%# DataBinder.Eval(Container, "Text")%></span>
                                        <table>
                                            <tr>
                                                <td align="right" style="width: 120px"><span class="ObjectDetail">Đơn vị:&nbsp;</span></td>
                                                <td><span class="ObjectDetail"><%# DataBinder.Eval(Container, "Attributes['OrganizationName']") %></span></td>
                                            </tr>
                                            <tr>
                                                <td align="right" ><span class="ObjectDetail">Người yêu cầu:&nbsp;</span></td>
                                                <td><span class="ObjectDetail"><%# DataBinder.Eval(Container, "Attributes['RequestBy']") %></span></td>
                                            </tr>
                                            <tr>
                                                <td align="right"><span class="ObjectDetail">Trạng thái:&nbsp;</span></td>
                                                <td><span class="ObjectDetail"><%# DataBinder.Eval(Container, "Attributes['StatusName']") %></span></td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadListBox>
                            </telerik:RadPane>
                        </telerik:RadSplitter>
                    </telerik:RadPane>
                    <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
                    </telerik:RadSplitBar>
                    <telerik:RadPane ID="PanelObjectDetail" runat="server" Scrolling="Y" Width="100%">
                        <telerik:RadTabStrip RenderMode="Lightweight" ID="TabObjectDetail" runat="server"  Width="100%" Height="100%" Skin="MetroTouch" MultiPageID="MultiPageObject" SelectedIndex="0" EnableDragToReorder="true" OnClientTabSelected="TabObjectDetail_OnClientTabSelected">
                            <Tabs>
                                <telerik:RadTab Text="Chi Tiết" Value="Detail"/>
                                <telerik:RadTab Text="Vật Tư Yêu Cầu" Value="Parts" Visible="False"/>
                            </Tabs>
                        </telerik:RadTabStrip>
                        
                        <telerik:RadMultiPage runat="server" ID="MultiPageObject" SelectedIndex="0" ScrollBars="Vertical">
                            <telerik:RadPageView runat="server" ID="RadPageView2" Height="100%">
                                <table>
                                    <tr>
                                        <td colspan="5">
                                            <dl class="accordion">
                                                <dt style="width: 100%;">
                                                    <span>Thông tin chung: </span>
                                                </dt>
                                            </dl>
                                        </td>
                                    </tr>
                                <tr style="height: 28px;" runat="server" ID="divItemDescription" Visible="False">
                                    <td align="right" style="width: 138px; padding-right: 2px">
                                        <span class="ObjectLabel">Phiếu yêu cầu:</span>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label runat="server" ID="lblItemDescription" style="color: blue;font-weight: bold;"></asp:Label>
                                            
                                    </td>
                                </tr>

                                    <tr style="height: 28px;">
                                        <td align="right" style="width: 138px; padding-right: 2px"><span class="ObjectLabel"></span>
                                            
                                        </td>
                                        <td colspan="3">
                                            <asp:RadioButton runat="server" ID="cbMonthly" Text="Yêu cầu cấp HCVT TTB PCD định kỳ" GroupName="Type" OnCheckedChanged="cbMonthly_OnCheckedChanged" AutoPostBack="True" Enabled="False"/>
                                            <asp:RadioButton runat="server" ID="cbUnexpected" Text="Yêu cầu cấp HCVT TTB PCD đột xuất" GroupName="Type" OnCheckedChanged="cbMonthly_OnCheckedChanged" AutoPostBack="True" Enabled="False"/>
                                            
                                        </td>
                                    </tr>
                                    
                                    <tr style="height: 28px;" runat="server" >
                                        <td align="right" style="width: 138px; padding-right: 2px; display: block"><span class="ObjectLabel">
                                                <asp:Label runat="server" ID="lblDuTru" Text="Dự trù cho tháng"></asp:Label>
                                            </span>
                                            <img src="Resources/Images/field-indicators.png" height="16px"/>
                                        </td>
                                        <td colspan="3">
                                            <telerik:RadTextBox ID="txtRequestForUnexpected" runat="server" Skin="Metro" CssClass="TextBoxInput" Width="150" Visible="False"/>
                                            <telerik:RadDropDownList RenderMode="Lightweight" runat="server" ID="ddlRequestForMonth"  
                                                                     HighlightTemplatedItems="true" Width="150" Skin="Metro" Enabled="False">
                                                <Items>
                                                    <telerik:DropDownListItem runat="server" Text="01" Value="01"/>
                                                    <telerik:DropDownListItem runat="server" Text="02" Value="02"/>
                                                    <telerik:DropDownListItem runat="server" Text="03" Value="03"/>
                                                    <telerik:DropDownListItem runat="server" Text="04" Value="04"/>
                                                    <telerik:DropDownListItem runat="server" Text="05" Value="05"/>
                                                    <telerik:DropDownListItem runat="server" Text="06" Value="06"/>
                                                    <telerik:DropDownListItem runat="server" Text="07" Value="07"/>
                                                    <telerik:DropDownListItem runat="server" Text="08" Value="08"/>
                                                    <telerik:DropDownListItem runat="server" Text="09" Value="09"/>
                                                    <telerik:DropDownListItem runat="server" Text="10" Value="10"/>
                                                    <telerik:DropDownListItem runat="server" Text="11" Value="11"/>
                                                    <telerik:DropDownListItem runat="server" Text="12" Value="12"/>
                                                </Items>
                                            </telerik:RadDropDownList>
                                            
                                        </td>
                                    </tr>
                                    <tr style="height: 28px;display: none">
                                        <td align="right" style="width: 138px; padding-right: 2px"><span class="ObjectLabel">Số phiếu yêu cầu</span>
                                            <img src="Resources/Images/field-indicators.png" height="16px"/>
                                        </td>
                                        <td colspan="3">
                                            <telerik:RadTextBox ID="txtCode" runat="server" Skin="Metro" CssClass="TextBoxInput" Width="150" ReadOnly="True" />
                                            
                                        </td>
                                    </tr>
                                    
                                    <tr style="height: 28px; display: none">
                                        <td align="right" style="width: 138px; padding-right: 2px"><span class="ObjectLabel">Lý do</span>
                                            <img src="Resources/Images/field-indicators.png" height="16px"/>
                                        </td>
                                        <td colspan="3">
                                            <telerik:RadTextBox ID="txtDescription" runat="server"  Width="525" Skin="Metro" CssClass="TextBoxInput"/>
                                        </td>
                                    </tr>

                                    <tr style="height: 28px;">
                                        <td align="right" style="width: 138px; padding-right: 2px"><span class="ObjectLabel">Trạng thái:</span></td>
                                        <td colspan="3">
                                            <telerik:RadDropDownList ID="ddlStatus" runat="server" Skin="Metro" CssClass="DropdownList" Width="300" />
                                            <telerik:RadButton ID="btnProcess" runat="server" Text="Tải nạp phiếu yêu cầu" OnClick="btnProcess_OnClick" Skin="Metro" Visible="False" BackColor="chartreuse">
                                                <Icon PrimaryIconUrl="Resources/Images/process16.png" PrimaryIconLeft="4" PrimaryIconRight="4" PrimaryIconTop="2" PrimaryIconWidth="32" PrimaryIconHeight="32"/>
                                            </telerik:RadButton>
                                            <telerik:RadCheckBox runat="server" ID="cbIsGenMR" Text="Đã tải nạp phiếu yêu cầu" Checked="True" Enabled="False" ForeColor="Blue" Visible="False"></telerik:RadCheckBox>
                                        </td>
                                        
                                    </tr>
                                    
                                    <tr style="height: 28px;">
                                        <td align="right" style="width: 138px; padding-right: 2px"><span class="ObjectLabel">Đơn vị:</span><img src="Resources/Images/field-indicators.png" height="16px"/></td>
                                        <td>
                                            <telerik:RadDropDownList ID="ddlOrg" runat="server" Skin="Metro" CssClass="DropdownList" Width="300" OnSelectedIndexChanged="ddlOrg_OnSelectedIndexChanged" AutoPostBack="True"/>
                                        </td>
                                        <td align="right" style="width: 168px; padding-right: 2px"><span class="ObjectLabel">Người yêu cầu:<img src="Resources/Images/field-indicators.png" height="16px"/></span></td>
                                        <td>
                                            <telerik:RadTextBox ID="txtRequestBy" runat="server" Skin="Metro" CssClass="TextBoxInput" Width="200" ReadOnly="False"/>
                                        </td>
                                    </tr>
                                    
                                    <tr style="height: 28px;">
                                        <td align="right" style="width: 138px; padding-right: 2px"><span class="ObjectLabel">Kho đơn vị:</span><img src="Resources/Images/field-indicators.png" height="16px"/></td>
                                        <td>
                                            <telerik:RadDropDownList ID="ddlStore" runat="server" Skin="Metro" CssClass="DropdownList" Width="300"/>
                                        </td>
                                        <td align="right" style="width: 168px; padding-right: 2px"><span class="ObjectLabel">Ngày yêu cầu:</span></td>
                                        <td>
                                            <telerik:RadTextBox ID="txtRequestDate" runat="server" Skin="Metro" CssClass="TextBoxInput" Width="200" ReadOnly="True"/>
                                        </td>
                                    </tr>
                                    
                                    <tr style="height: 28px;">
                                        <td align="right" style="width: 138px; padding-right: 2px"><span class="ObjectLabel">Ghi chú:</span></td>
                                        <td colspan="3">
                                            <telerik:RadTextBox ID="txtNote" runat="server" Skin="Metro" CssClass="TextBoxInput" Width="679" ReadOnly="False" TextMode="MultiLine" Rows="2"/>
                                        </td>
                                        
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="5">
                                            <dl class="accordion">
                                                <dt style="width: 100%;">
                                                    <span>Thông tin cách ly</span>
                                                </dt>
                                            </dl>
                                        </td>
                                    </tr>
                                    <tr style="height: 28px;">
                                        <td align="right" style="width: 138px; padding-right: 2px"><span class="ObjectLabel">Số TYT:</span></td>
                                        <td>
                                            <telerik:RadTextBox ID="txtSoTYT" runat="server" Skin="Metro" CssClass="TextBoxInput" Width="200" />
                                        </td>
                                        <td align="right" style="width: 168px; padding-right: 2px"><span class="ObjectLabel">Số người cách ly tập trung:<img src="Resources/Images/field-indicators.png" height="16px"/></span></td>
                                        <td>
                                            <telerik:RadTextBox ID="txtSoCachLyTapTrung" runat="server" Skin="Metro" CssClass="TextBoxInput" Width="200" />
                                        </td>
                                        
                                    </tr>
                                    <tr style="height: 28px;">
                                        <td align="right" style="width: 138px; padding-right: 2px"><span class="ObjectLabel">Số người phục vụ trong khu cách ly/Ngày:<img src="Resources/Images/field-indicators.png" height="16px"/></span></td>
                                        <td>
                                            <telerik:RadTextBox ID="txtSoCachLyTaiNha" runat="server" Skin="Metro" CssClass="TextBoxInput" Width="200" />
                                        </td>
                                        <td align="right" style="width: 168px; padding-right: 2px"><span class="ObjectLabel">Số đội lấy mẫu/ngày (mỗi đội 2 người):<img src="Resources/Images/field-indicators.png" height="16px"/></span></td>
                                        <td>
                                            <telerik:RadTextBox ID="txtSoLayMauXN" runat="server" Skin="Metro" CssClass="TextBoxInput" Width="200" />
                                        </td>
                                        
                                    </tr>
                                    
                                <tr runat="server" id="divTitleCV" Visible="False">
                                        <td colspan="5">
                                            <dl class="accordion">
                                                <dt style="width: 100%;">
                                                    <span>Thông tin công văn</span>
                                                </dt>
                                            </dl>
                                        </td>
                                    </tr>
                                <tr runat="server" id="divDetailCV1" Visible="False">
                                    <td align="right" style="width: 138px; padding-right: 2px"><span class="ObjectLabel">Số công văn:<img src="../../Resources/Images/field-indicators.png" height="16px"/></span></td>
                                    <td>
                                        <telerik:RadTextBox ID="txtSoCV" runat="server" Skin="Metro" CssClass="TextBoxInput" Width="200" ReadOnly="True"/>
                                    </td>

                                    <td align="right" style="width: 138px; padding-right: 2px"><span class="ObjectLabel">Ngày công văn:<img src="../../Resources/Images/field-indicators.png" height="16px"/></span></td>
                                    <td>
                                        <telerik:RadDatePicker ID="txtNgayCV" runat="server" Skin="MetroTouch" Width="150" Enabled="False">
                                            <DateInput DateFormat="dd-MMM-yyyy"/>
                                        </telerik:RadDatePicker>
                                    </td>
                                        
                                </tr>
                                
                                <tr runat="server" id="divDetailCV2" Visible="False">
                                    <td align="right" style="width: 138px; padding-right: 2px"><span class="ObjectLabel">Về việc:<img src="../../Resources/Images/field-indicators.png" height="16px"/></span></td>
                                    <td colspan="3">
                                        <telerik:RadTextBox ID="txtNoiDungCV" runat="server" Skin="Metro" CssClass="TextBoxInput" Width="679" ReadOnly="False" TextMode="MultiLine" Rows="2"/>
                                    </td>
                                        
                                </tr>


                                    <tr>
                                        <td colspan="5">
                                            <dl class="accordion">
                                                <dt style="width: 100%;">
                                                    <span>File báo cáo</span>
                                                </dt>
                                            </dl>
                                        </td>
                                    </tr>
                                    
                                    <tr >
                                        <td align="right" valign="top" style="width: 138px; padding-right: 2px"><span class="ObjectLabel">Chọn file báo cáo:<img src="Resources/Images/field-indicators.png" height="16px"/></span></td>
                                        <td colspan="3">
                                            <span style="color: red">Vui lòng chỉ chọn file PDF.</span>
                                            <telerik:RadAsyncUpload runat="server" ID="radUpload" MultipleFileSelection="Disabled" Skin="Metro" Enabled="False" >
                                                <Localization Select="Chọn File"></Localization>
                                                <FileFilters>
                                                    <telerik:FileFilter Description="PDF(pdf)" Extensions="pdf" />
                                                </FileFilters>
                                            </telerik:RadAsyncUpload>
                                            
                                        <telerik:RadGrid RenderMode="Lightweight" ID="grdAttachFile" runat="server" AllowPaging="False" AllowSorting="False" AllowFilteringByColumn="False" CellSpacing="0" GridLines="None" Skin="Metro" Height="110" Width="660"
                                                         OnDeleteCommand="grdAttachFile_DeleteCommand"
                                                         OnNeedDataSource="grdAttachFile_OnNeedDataSource">
                                        <GroupingSettings CaseSensitive="false" />
                                        <MasterTableView AutoGenerateColumns="false" TableLayout="Fixed" ClientDataKeyNames="ID" DataKeyNames="ID">
                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />                                        
                                                        <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" ConfirmText="Do you want to delete Attach file?" ButtonType="ImageButton" ImageUrl="Resources/Images/delete.png">
                                                            <HeaderStyle Width="30" />
                                                                <ItemStyle HorizontalAlign="Center" Width="5%"  />
                                                        </telerik:GridButtonColumn>

                                                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                                                            <HeaderStyle Width="35" />
                                                            <ItemStyle HorizontalAlign="Center" Width="5%"/>
                                                            <ItemTemplate>
                                                                <a href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' 
                                                                    download='<%# DataBinder.Eval(Container.DataItem, "Filename") %>' target="_blank">
                                                                    <asp:Image ID="Image1" runat="server" ImageUrl='Resources/Images/document.png'
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
                            </telerik:RadPageView>
                            
                            <telerik:RadPageView runat="server" ID="RadPageView5">
                                <telerik:RadGrid RenderMode="Lightweight" ID="grdPart" GridLines="None" runat="server" AllowAutomaticDeletes="True" Skin="Metro" Height="100%"
                                    AllowAutomaticInserts="True" PageSize="10"  AllowAutomaticUpdates="True" AllowPaging="False"
                                    AutoGenerateColumns="False" 
                                     OnBatchEditCommand="grdPart_OnBatchEditCommand"
                                    OnNeedDataSource="grdPart_OnNeedDataSource"
                                    OnItemCommand="grdPart_OnItemCommand"
                                                 >
                                    <MasterTableView CommandItemDisplay="None" DataKeyNames="ID" HorizontalAlign="NotSet" EditMode="Batch" AutoGenerateColumns="False">
                                        <BatchEditingSettings OpenEditingEvent="Click"  EditType="Row" HighlightDeletedRows="true"/> 
                                        <CommandItemSettings AddNewRecordText="Thêm mới vật tư" SaveChangesText="Lưu" CancelChangesText="Hủy bỏ" RefreshText="Làm mới dữ liệu"/>
                                        <ColumnGroups>
                                            <telerik:GridColumnGroup HeaderText="Thông Tin Tồn Kho CDC" Name="Group1" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderStyle BackColor="brown"></HeaderStyle>
                                            </telerik:GridColumnGroup>
                                        </ColumnGroups>
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="#" Groupable="False" AllowFiltering="false" ReadOnly="True">
                                                <HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSoTT" runat="server" Text='<%# grdPart.CurrentPageIndex * grdPart.PageSize + grdPart.Items.Count + 1 %>'>
                                                    </asp:Label>
                              
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="DeleteItem" ConfirmText="Bạn có muốn xóa vật tư này không" ButtonType="ImageButton" ImageUrl="Resources/Images/toolbar_delete.png">
                                                <HeaderStyle Width="40" />
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </telerik:GridButtonColumn>

                                            <telerik:GridTemplateColumn HeaderText="Vật tư yêu cầu" UniqueName="RequestPartName" DataField="RequestPartName" Display="False">
                                                <HeaderStyle HorizontalAlign="Center" Width="200" />
                                                <ItemStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblRequestPartName" Text='<%# Eval("RequestPartName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <span>
                                                        <telerik:RadTextBox RenderMode="Lightweight" Width="100%" runat="server" ID="txtRequestPartName" Skin="Metro">
                                                        </telerik:RadTextBox><br/>
                                                        <span style="color: Red">
                                                            <asp:RequiredFieldValidator ID="validateRequestPartName" ControlToValidate="txtRequestPartName" ErrorMessage="* Bắt buộc nhập" runat="server" Display="Dynamic"/>
                                                            
                                                        </span>
                                                    </span>
                                                </EditItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Vật tư yêu cầu" UniqueName="PartCode" DataField="PartCode" >
                                                <HeaderStyle HorizontalAlign="Center" Width="200"/>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <%# Eval("PartDescription")%>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlPart" Skin="Metro" CssClass="DropdownList" DataValueField="FULLNAME" DataTextField="FULLNAME" DataSourceID="SqlDataSource2" AllowCustomText="True" Filter="Contains" ShowDropDownOnTextboxClick="True">
                                                    </telerik:RadComboBox>
                                                    <span style="color: Red">
                                                        <asp:RequiredFieldValidator ID="validatePart" ControlToValidate="ddlPart" ErrorMessage="* Bắt buộc nhập" runat="server" Display="Dynamic"/>
                                                        
                                                    </span>
                                                </EditItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridBoundColumn DataField="PartUMO" HeaderStyle-Width="210px" HeaderText="ĐVT" UniqueName="PartUMO" ReadOnly="True">
                                                <HeaderStyle HorizontalAlign="Center" Width="60" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridTemplateColumn HeaderText="Số lượng tồn" UniqueName="CurrentStock" DataField="CurrentStock">
                                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                <ItemStyle HorizontalAlign="Right" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCurrentStock" Text='<%# Eval("CurrentStock", "{0:N}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <span>
                                                        <telerik:RadNumericTextBox RenderMode="Lightweight" Width="100%" runat="server" ID="tbCurrentStock" Skin="Metro" CssClass="TextBoxInput">
                                                        </telerik:RadNumericTextBox><br/>
                                                        <span style="color: Red">
                                                            <asp:RequiredFieldValidator ID="validateCurrentStock" ControlToValidate="tbCurrentStock" ErrorMessage="* Bắt buộc nhập" runat="server" Display="Dynamic"/>
                                                            
                                                        </span>
                                                    </span>
                                                </EditItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn HeaderText="Số lượng yêu cầu" UniqueName="RequestQty" DataField="RequestQty">
                                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                <ItemStyle HorizontalAlign="Right" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblRequestQty" Text='<%# Eval("RequestQty", "{0:N}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <span>
                                                        <telerik:RadNumericTextBox RenderMode="Lightweight" Width="100%" runat="server" ID="tbRequestQty" Skin="Metro" CssClass="TextBoxInput">
                                                        </telerik:RadNumericTextBox><br/>
                                                        <span style="color: Red">
                                                            <asp:RequiredFieldValidator ID="validateRequestQty" ControlToValidate="tbRequestQty" ErrorMessage="* Bắt buộc nhập" runat="server" Display="Dynamic"/>
                                                            
                                                        </span>
                                                    </span>
                                                </EditItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn HeaderText="Nguồn cấp" UniqueName="FromStoreCode" DataField="FromStoreCode" Display="False">
                                                <HeaderStyle HorizontalAlign="Center" Width="160" />
                                                <ItemStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <%# Eval("FromStoreName")%>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlFromStore" Skin="Metro" CssClass="DropdownList" DataValueField="FULLNAME" DataTextField="FULLNAME" DataSourceID="SqlDataSource1" AllowCustomText="True" Filter="Contains">
                                                    </telerik:RadComboBox>
                                                    <%--<span style="color: Red">
                                                        <asp:RequiredFieldValidator ID="validatePart" ControlToValidate="ddlFromStore" ErrorMessage="* Bắt buộc nhập" runat="server" Display="Dynamic"/>
                                                        
                                                    </span>--%>
                                                </EditItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn HeaderText="Số lượng duyệt cấp" UniqueName="ApprovedQty" DataField="ApprovedQty" Display="False">
                                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                <ItemStyle HorizontalAlign="Right" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblApprovedQty" Text='<%# Eval("ApprovedQty", "{0:N}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <span>
                                                        <telerik:RadNumericTextBox RenderMode="Lightweight" Width="100%" runat="server" ID="tbApprovedQty" Skin="Metro" CssClass="TextBoxInput">
                                                        </telerik:RadNumericTextBox><br/>
                                                        <span style="color: Red">
                                                            <asp:RequiredFieldValidator ID="validateApprovedQty" ControlToValidate="tbApprovedQty" ErrorMessage="* Bắt buộc nhập" runat="server" Display="Dynamic"/>
                                                            
                                                        </span>
                                                    </span>
                                                </EditItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn HeaderText="Kho Chống Dịch" UniqueName="KHO_PCD" DataField="KHO_PCD" ReadOnly="True" Display="False" ColumnGroupName="Group1">
                                                <HeaderStyle HorizontalAlign="Center" Width="120" BackColor="coral"/>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblKHO_PCD" Text='<%# Eval("KHO_PCD", "{0:N}") %>'/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn HeaderText="Kho Tài Trợ" UniqueName="KHOTAITRO" DataField="KHOTAITRO" ReadOnly="True" Display="False" ColumnGroupName="Group1">
                                                <HeaderStyle HorizontalAlign="Center" Width="120" BackColor="green"/>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblKHOTAITRO" Text='<%# Eval("KHOTAITRO", "{0:N}") %>'/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn HeaderText="Kho Dược Cấp" UniqueName="KHO_DUOC" DataField="KHO_DUOC" ReadOnly="True" Display="False" ColumnGroupName="Group1">
                                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                <ItemStyle HorizontalAlign="Right" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblKHO_DUOC" Text='<%# Eval("KHO_DUOC", "{0:N}") %>'/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings AllowKeyboardNavigation="true"></ClientSettings>
                                </telerik:RadGrid>
                            </telerik:RadPageView>
                        
                        
                        </telerik:RadMultiPage>
                    </telerik:RadPane>
                </telerik:RadSplitter>
            </telerik:RadPane>
        </telerik:RadSplitter>

    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SQLConn %>"
                       ProviderName="System.Data.SqlClient" SelectCommand="EXEC getPAvailable;"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SQLConn %>"
                       ProviderName="System.Data.SqlClient" SelectCommand="EXEC getStoreCDC;"></asp:SqlDataSource>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="MetroTouch"/>
    <telerik:RadNotification RenderMode="Lightweight" ID="errorNotification" runat="server" Text="Initial text" Position="TopRight" Animation="Fade" EnableRoundedCorners="true" EnableShadow="true" VisibleOnPageLoad="False" Width="500" Title="Thông báo"  TitleIcon="Resources/Images/error_14.png" ContentIcon="Resources/Images/error_24.png" Skin="MetroTouch"  AnimationDuration="500" AutoCloseDelay="8000">
    </telerik:RadNotification>
    
    <telerik:RadNotification RenderMode="Lightweight" ID="completeNotification" runat="server" Text="Initial text" Position="TopRight" Animation="Fade" EnableRoundedCorners="true" EnableShadow="true" VisibleOnPageLoad="False" Width="500" Title="Thông báo"  TitleIcon="Resources/Images/complete_16.png" ContentIcon="Resources/Images/complete_24.png" Skin="MetroTouch"  AnimationDuration="500" AutoCloseDelay="3000">
    </telerik:RadNotification>
    <asp:HiddenField runat="server" ID="AllowDelete"/>
    <asp:HiddenField runat="server" ID="AllowSave"/>
    <asp:HiddenField runat="server" ID="IsDelete" Value="false"/>

    <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="ajaxCustomer_OnAjaxRequest">
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
                <telerik:AjaxUpdatedControl ControlID="AllowSave"/>
                <telerik:AjaxUpdatedControl ControlID="AllowDelete"/>
                <telerik:AjaxUpdatedControl ControlID="lblObjectName"/>
                <telerik:AjaxUpdatedControl ControlID="lbMR" LoadingPanelID="RadAjaxLoadingPanel2"/>
                <telerik:AjaxUpdatedControl ControlID="TabObjectDetail" LoadingPanelID="RadAjaxLoadingPanel2"/>
                <telerik:AjaxUpdatedControl ControlID="MultiPageObject" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
                
        <telerik:AjaxSetting AjaxControlID="btnSearch">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="lbMR" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="cbMonthly">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="lblDuTru" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="cbUnexpected">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="lblDuTru" LoadingPanelID="RadAjaxLoadingPanel2"/>
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
        
        <telerik:AjaxSetting AjaxControlID="btnProcess">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="ddlStatus" LoadingPanelID="RadAjaxLoadingPanel2"/>
                <telerik:AjaxUpdatedControl ControlID="errorNotification"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script src="Resources/Scripts/jquery-1.9.1.js"></script>
            <script src="Resources/Scripts/bootstrap.min.js"></script>
            
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
                    var pane = splitter.getPaneById("<%= PanelObjectList.ClientID %>");

                    if (strValue == "ExpandLeft") {
                        var isCollapseSuccess = pane.collapse();
                    }
                    else if (strValue == "SplitView") {
                        var isExpandSuccess = pane.expand(pane);
                    }
                    else if (strValue == "ExpandRight") {
                        window.location.href = "PartList.aspx";
                    }
                    else if (strValue == "Refresh") {
                        location.reload();
                    }
                    else if (strValue == "Delete") {
                        if (confirm('Bạn có muốn xóa yêu cầu này không!?') === false) {
                            return;
                        }
                        document.getElementById('<%= IsDelete.ClientID%>').value = "true";

                        //ajaxManager.ajaxRequest("DeleteMR");
                    }

                    return true;
                }

                function TabObjectDetail_OnClientTabSelected(sender, eventArgs) {
                    var toolBar = $find("<%=ViewToolBar.ClientID %>");
                    var saveItem = toolBar.findItemByValue("Update");
                    var deleteItem = toolBar.findItemByValue("Delete");
                    var tab = eventArgs.get_tab();

                    if (tab.get_value() === "Parts") {
                        saveItem.disable();
                        deleteItem.disable();
                    }
                    else if (tab.get_value() === "Detail") {
                        var allowSave = document.getElementById('<%= AllowSave.ClientID%>').value;
                        var allowDelete = document.getElementById('<%= AllowDelete.ClientID%>').value;
                        //alert(allowSave + allowDelete);
                        if (allowSave === "false") {
                            saveItem.disable();
                        } else {
                            saveItem.enable();
                        }

                        if (allowDelete === "false") {
                            deleteItem.disable();
                        } else {
                            deleteItem.enable();
                        }
                    }
                }
            </script>
        </telerik:RadCodeBlock>
    </div>
</asp:Content>