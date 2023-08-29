<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LocationDetail.aspx.cs" Inherits="EDMs.Web.Controls.Asset.LocationDetail" EnableViewState="true" %>
 
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
        
        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdObjectEventPanel {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_grdObjectEvent_GridData {
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
                                        font-family: helvetica, arial, verdana, sans-serif;">Vị trí: </span>
                                
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
                                                    EmptyMessage="Search within All Locations" InvalidStyleDuration="100">
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
                                                <td align="right" style="width: 120px"><span class="assetDetail">Đơn vị:&nbsp;</span></td>
                                                <td><span class="assetDetail"><%# DataBinder.Eval(Container, "Attributes['DonVi']") %></span></td>
                                            </tr>
                                            <tr>
                                                <td align="right" ><span class="assetDetail">Bộ phận:&nbsp;</span></td>
                                                <td><span class="assetDetail"><%# DataBinder.Eval(Container, "Attributes['BoPhan']") %></span></td>
                                            </tr>
                                            <tr>
                                                <td align="right" ><span class="assetDetail">Phân loại:&nbsp;</span></td>

                                                <td><span class="assetDetail"><%# DataBinder.Eval(Container, "Attributes['PhanLoai']") %></span></td>
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
                                <telerik:RadTab Text="Phân cấp"/>
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
                                        <td align="right" style="width: 150px; padding-right: 2px"><div class="detailLabel">Vị trí:</div></td>
                                        <td>
                                            <telerik:RadTextBox ID="txtViTri" runat="server" Skin="MetroTouch" Width="120" ReadOnly="True"/>
                                            <telerik:RadTextBox ID="txtDienGiai" runat="server" Skin="MetroTouch" Width="250" ReadOnly="True"/>
                                        </td>
                                        
                                        <td align="right" style="width: 150px; padding-right: 2px"><div class="detailLabel">Đơn vị:</div></td>
                                        <td>
                                            <telerik:RadTextBox ID="txtDonVi" runat="server" Skin="MetroTouch" Width="120" ReadOnly="True"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="5">
                                            <dl class="accordion">
                                                <dt style="width: 100%;">
                                                    <span>Phân cấp</span>
                                                </dt>
                                            </dl>
                                        </td>
                                    </tr>

                                    <tr> 
                                        <td align="right" style="width: 150px; padding-right: 2px"><div class="detailLabel">Vị trí cha:</div></td>
                                        <td>
                                            <telerik:RadTextBox ID="txtViTriCha" runat="server" Skin="MetroTouch" Width="120" ReadOnly="True"/>
                                        </td>
                                    </tr>
                                    
                                </table>
                            </telerik:RadPageView>
                            <%--Tai lieu lien quan--%>
                            <telerik:RadPageView runat="server" ID="RadPageView5">
                                <telerik:RadTreeView RenderMode="Lightweight" ID="rtvLocationStructure"  runat="server" Skin="MetroTouch" OnNodeExpand="rtvLocationStructure_OnNodeExpand" LoadingStatusPosition="BeforeNodeText"/>
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
                
                <telerik:AjaxSetting AjaxControlID="rtvLocationStructure">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvLocationStructure"/>
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
                    window.location.href = "LocationList.aspx";
                }
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>