<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProjectProccessReport.aspx.cs" Inherits="EDMs.Web.ProjectProccessReport" EnableViewState="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <!--[if gte IE 8]>
        <style type="text/css">
            #ctl00_ContentPlaceHolder2_grdDocument_ctl00_Header{table-layout:auto !important;}
            #ctl00_ContentPlaceHolder2_grdDocument_ctl00{table-layout:auto !important;}
        </style>
    <![endif]-->

    <style type="text/css">
        /*Custom CSS of Grid documents for FF browser*/
        /*#ctl00_ContentPlaceHolder2_grdDocument_ctl00_Header{table-layout:auto !important;}
        #ctl00_ContentPlaceHolder2_grdDocument_ctl00{table-layout:auto !important;}*/
        /*End*/
        @-moz-document url-prefix() {
            #ctl00_ContentPlaceHolder2_grdDocument_ctl00_Header{table-layout:auto !important;}
            #ctl00_ContentPlaceHolder2_grdDocument_ctl00{table-layout:auto !important;}
        }
        
        #ctl00_ContentPlaceHolder2_grdDocument_ctl00_ctl02_ctl03_txtDate_popupButton {
            display: none;
        }

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_CustomerMenu_i6_ddlProjectPanel {
            display: inline-block !important;
        }

        .rgExpandCol {
            width: 1% !important;
        }

        .rgGroupCol {
            width: 1% !important;
        }
        div.RadGrid .rgPager .rgAdvPart     
        {     
        display:none; 
        }
        /*#RAD_SPLITTER_PANE_CONTENT_ctl00_leftPane {
            width: 250px !important;
        }
        #RAD_SPLITTER_PANE_CONTENT_ctl00_topLeftPane {
            width: 250px !important;
        }*/

        .RadAjaxPanel {
            height: 100% !important;
        }

        .rpExpandHandle {
            display: none !important;
        }

        .RadGrid .rgRow td, .RadGrid .rgAltRow td, .RadGrid .rgEditRow td, .RadGrid .rgFooter td, .RadGrid .rgFilterRow td, .RadGrid .rgHeader, .RadGrid .rgResizeCol, .RadGrid .rgGroupHeader td {
            padding-left: 0px !important;
            padding-right: 0px !important;
        }

        /*Hide change page size control*/
        /*div.RadGrid .rgPager .rgAdvPart     
        {     
        display:none;        sssssss
        }*/    

        a.tooltip
        {
            outline: none;
            text-decoration: none;
        }

            a.tooltip strong
            {
                line-height: 30px;
            }

            a.tooltip:hover
            {
                text-decoration: none;
            }

            a.tooltip span
            {
                z-index: 10;
                display: none;
                padding: 14px 20px;
                margin-top: -30px;
                margin-left: 5px;
                width: 240px;
                line-height: 16px;
            }

            a.tooltip:hover span
            {
                display: inline;
                position: absolute;
                color: #111;
                border: 1px solid #DCA;
                background: #fffAF0;
            }

        .callout
        {
            z-index: 20;
            position: absolute;
            top: 30px;
            border: 0;
            left: -12px;
        }

        /*CSS3 extras*/
        a.tooltip span
        {
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            -moz-box-shadow: 5px 5px 8px #CCC;
            -webkit-box-shadow: 5px 5px 8px #CCC;
            box-shadow: 5px 5px 8px #CCC;
        }

        .rgMasterTable {
            table-layout: auto;
        }

        
        #ctl00_ContentPlaceHolder2_radTreeFolder {
            overflow: visible !important;
        }

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdDocumentPanel, #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_divContainerPanel
        {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_RadPageView1, #ctl00_ContentPlaceHolder2_RadPageView2,
        #ctl00_ContentPlaceHolder2_RadPageView3, #ctl00_ContentPlaceHolder2_RadPageView4,
        #ctl00_ContentPlaceHolder2_RadPageView5
        {
            height: 100% !important;
        }

        #divContainerLeft
        {
            width: 25%;
            float: left;
            margin: 5px;
            height: 99%;
            border-right: 1px dotted green;
            padding-right: 5px;
        }

        #divContainerRight
        {
            width: 100%;
            float: right;
            margin-top: 5px;
            height: 99%;
        }

        .dotted
        {
            border: 1px dotted #000;
            border-style: none none dotted;
            color: #fff;
            background-color: #fff;
        }

        .exampleWrapper
        {
            width: 100%;
            height: 100%;
            /*background: transparent url(images/background.png) no-repeat top left;*/
            position: relative;
        }

        .tabStrip
        {
            position: absolute;
            top: 0px;
            left: 0px;
        }

        .multiPage
        {
            position: absolute;
            top: 30px;
            left: 0px;
            color: white;
            width: 100%;
            height: 100%;
        }

        /*Fix RadMenu and RadWindow z-index issue*/
        .radwindow
        {
            z-index: 8000 !important;
        }

        .TemplateMenu
        {
            z-index: 10;
        }
    </style>
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div style="width: 100%; height: 100%">
        <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Horizontal">
            <telerik:RadPane ID="RadPane3" runat="server" Height="30px" Scrollable="false" Scrolling="None">
                <telerik:RadToolBar ID="CustomerMenu" Width="100%" runat="server" OnClientButtonClicking="OnClientButtonClicking">
                    <Items>
                        <telerik:RadToolBarButton runat="server" Text="Refresh" Value="1" ImageUrl="~/Images/refresh.png"/>
                        <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                        <telerik:RadToolBarButton runat="server" Text="Print Progress Report" Value="1" ImageUrl="~/Images/report.png"/>
                        <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                    
                        <telerik:RadToolBarDropDown runat="server" Text="Action" ImageUrl="~/Images/action.png">
                            <Buttons>
                                <telerik:RadToolBarButton runat="server" Text="Get latest Progress actual data" Value="7" ImageUrl="~/Images/emdrreport.png" Visible="false"/>
                                <telerik:RadToolBarButton runat="server" IsSeparator="true" Visible="false"/>
                                <telerik:RadToolBarButton runat="server" Text="Export Progress planed template" Value="6" ImageUrl="~/Images/export.png"/>
                                <telerik:RadToolBarButton runat="server" Text="Export Progress actual template" Value="6" ImageUrl="~/Images/export.png"/>
                                <telerik:RadToolBarButton runat="server" Text="Import Progress planed/actual" Value="5" ImageUrl="~/Images/import.png" />                            
                            </Buttons>
                        </telerik:RadToolBarDropDown>
                        
                        <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                    <telerik:RadToolBarButton runat="server" Value="ShowAll">
                        <ItemTemplate>
                            <div style="display: inline-block">
                            <img src="Images/project.png"/> Selected Project: 
                            <asp:DropDownList ID="ddlProject" runat="server" Width="200px" AutoPostBack="True" 
                                    OnSelectedIndexChanged="ddlProject_OnSelectedIndexChanged"
                                    />
                            </div>
                        </ItemTemplate>
                    </telerik:RadToolBarButton>

                    </Items>
                </telerik:RadToolBar>
            </telerik:RadPane>            
            <telerik:RadPane ID="RadPane2" runat="server" Scrollable="false" Scrolling="None">
                <table width="100%">
                    <tr>
                        <td align="center">
                            <telerik:RadHtmlChart runat="server" ID="LineChart" Width="1024" Height="600" Transitions="true">
                                <Appearance>
                                        <FillStyle BackgroundColor="White"></FillStyle>
                                </Appearance>
                                <ChartTitle >
                                        <Appearance Align="Center" BackgroundColor="White" Position="Top">
                                        </Appearance>
                                </ChartTitle>
                                <Legend>
                                        <Appearance BackgroundColor="White" Position="Bottom">
                                        </Appearance>
                                </Legend>
                                <PlotArea>
                                        <Appearance>
                                            <FillStyle BackgroundColor="White"></FillStyle>
                                        </Appearance>
                                        <XAxis  Color="#b3b3b3" BaseUnit="days" Type="Date" DataLabelsField="WeekDate">
                                            <LabelsAppearance DataFormatString="d" RotationAngle="270" Step="7" />
                                            <MajorGridLines Color="#EFEFEF" Width="1"/>
                                            <MinorGridLines Color="#F7F7F7" Width="1"/>
                             
                                        </XAxis>
                                        
                                        <%--<XAxis  Color="Black" Type="Auto" DataLabelsField="WeekName">
                                            <LabelsAppearance DataFormatString="{0}" RotationAngle="270"/>
                                            <MajorGridLines Color="#EFEFEF" Width="1"/>
                                            <MinorGridLines Color="#F7F7F7" Width="1"/>
                             
                                        </XAxis>--%>

                                        <YAxis AxisCrossingValue="0" Color="#b3b3b3" MajorTickSize="1" MajorTickType="Outside"
                                            MaxValue="100" MinorTickSize="1" MinorTickType="Outside" MinValue="0" Reversed="false"
                                            Step="10">
                                            <LabelsAppearance DataFormatString="{0}%" RotationAngle="0" Skip="0" Step="1"/>
                              
                                            <MajorGridLines Color="#EFEFEF" Width="1"/>
                                            <MinorGridLines Color="#F7F7F7" Width="1"/>
                                            <TitleAppearance Position="Center" RotationAngle="0" Text=""/>
                              
                                        </YAxis>
                                        <Series>
                                            <telerik:LineSeries Name="Planed" DataFieldY="Planed" >
                                                <Appearance>
                                                    <FillStyle BackgroundColor="#5ab7de"></FillStyle>
                                                </Appearance>
                                                <LabelsAppearance DataFormatString="{0}" Position="Below" Visible="false"/>
                                   
                                                <LineAppearance Width="2" />
                                                <MarkersAppearance MarkersType="Circle" BackgroundColor="White" Size="6" BorderColor="#5ab7de" BorderWidth="2"/>
                                                <TooltipsAppearance >
                                                    <ClientTemplate>
                                                        Planed of  #= kendo.format(\'{0:d}\', category) #: #= value #%
                                                    </ClientTemplate>

                                                </TooltipsAppearance>
                                            </telerik:LineSeries>

                                            <telerik:LineSeries Name="Actual" MissingValues="Interpolate">
                                                <Appearance>
                                                    <FillStyle BackgroundColor="#FF3300"></FillStyle>
                                                </Appearance>
                                                <LabelsAppearance DataFormatString="{0}" Position="Above" Visible="false">
                                                </LabelsAppearance>
                                                <LineAppearance Width="2" />
                                                <MarkersAppearance MarkersType="Triangle" BackgroundColor="#FF3300" Size="5" BorderColor="#FF3300"
                                                    BorderWidth="2"></MarkersAppearance>
                                                <TooltipsAppearance>
                                                    <ClientTemplate>
                                                        Actual of  #= kendo.format(\'{0:d}\', category) #: #= value #%
                                                    </ClientTemplate>
                                                </TooltipsAppearance>
                                            </telerik:LineSeries>
                                        </Series>
                                </PlotArea>
                            </telerik:RadHtmlChart>
                        </td>
                    </tr>
                </table>
                
                

                
            </telerik:RadPane>

        </telerik:RadSplitter>
        
        
        
        
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
            
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlProject" LoadingPanelID="RadAjaxLoadingPanel2"/>
                         <telerik:AjaxUpdatedControl ControlID="rtvWorkgroup" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="LineChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlProject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvWorkgroup" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="LineChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rtvWorkgroup">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="LineChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
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
        </Windows>
    </telerik:RadWindowManager>
    </div>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="Scripts/jquery-1.7.1.js"></script>
        <script type="text/javascript">
            
            var ajaxManager;

            function pageLoad() {
                ajaxManager = $find("<%=ajaxCustomer.ClientID %>");
            }

            function OnClientSelectedIndexChanging(sender, eventArgs) {
                var input = sender.get_inputDomElement();
                input.style.background = "url(" + eventArgs.get_item().get_imageUrl() + ") no-repeat";
            }
            function OnClientLoad(sender) {
                var input = sender.get_inputDomElement();
                var selectedItem = sender.get_selectedItem();
                input.style.background = "url(" + selectedItem.get_imageUrl() + ") no-repeat";
            }

            function OnClientButtonClicking(sender, args) {
                var button = args.get_item();
                var strText = button.get_text();
                var strValue = button.get_value();

                if (strText.toLowerCase() == "refresh") {
                    ajaxManager.ajaxRequest("RefreshProgressReport");
                }

                if (strText == "Get latest Progress actual data") {
                    ajaxManager.ajaxRequest("GetLatestData");
                }

                if (strText == "Export Progress planed template") {
                    ajaxManager.ajaxRequest("ExportProgress_Planed");
                }

                if (strText == "Export Progress actual template") {
                    ajaxManager.ajaxRequest("ExportProgress_Actual");
                }

                if (strText == "Print Progress Report") {
                    ajaxManager.ajaxRequest("PrintProgress");
                }

                if (strText == "Import Progress planed/actual") {
                    var owd = $find("<%=ImportData.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/Document/ImportData.aspx?type=progress", "ImportData");
                }
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