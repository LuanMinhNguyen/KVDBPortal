<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProjectProccessReport_bak1.aspx.cs" Inherits="EDMs.Web.ProjectProccessReport_bak1" EnableViewState="true" %>

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
        
        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_CustomerMenu_i8_ddlRecoveryPlanPanel {
            display: inherit !important;
        }

        #ctl00_ContentPlaceHolder2_grdDocument_ctl00_ctl02_ctl03_txtDate_popupButton {
            display: none;
        }

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_CustomerMenu_i6_ddlProjectPanel {
            display: inline-block !important;
        }

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_CustomerMenu_i6_ddlRecoveryPlanPanel {
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

        #RAD_SPLITTER_PANE_CONTENT_ctl00_ContentPlaceHolder2_RadPane2 {
            overflow: auto !important;
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
                <telerik:RadToolBar ID="CustomerMenu" Width="100%" runat="server" OnClientButtonClicking="OnClientButtonClicking" style="z-index: 10">
                    <Items>
                        <telerik:RadToolBarButton runat="server" Text="Refresh" Value="1" ImageUrl="~/Images/refresh.png"/>
                        <telerik:RadToolBarButton runat="server" IsSeparator="true" Visible="False"/>
                        <telerik:RadToolBarButton runat="server" Text="Print Progress Report" Value="1" ImageUrl="~/Images/report.png" Visible="False"/>
                        <telerik:RadToolBarButton runat="server" IsSeparator="true" Visible="False"/>
                    
                        <telerik:RadToolBarDropDown runat="server" Text="Action" ImageUrl="~/Images/action.png" Visible="False">
                            <Buttons>
                                <telerik:RadToolBarButton runat="server" Text="Get latest Progress actual data" Value="7" ImageUrl="~/Images/emdrreport.png" Visible="false"/>
                                <telerik:RadToolBarButton runat="server" IsSeparator="true" Visible="false"/>
                                <telerik:RadToolBarButton runat="server" Text="Export Progress planed template" Value="6" ImageUrl="~/Images/export.png"/>
                                <telerik:RadToolBarButton runat="server" Text="Export Progress recovery planed template" Value="6" ImageUrl="~/Images/export.png"/>
                                <telerik:RadToolBarButton runat="server" Text="Export Progress actual template" Value="6" ImageUrl="~/Images/export.png"/>
                                <telerik:RadToolBarButton runat="server" Text="Import Progress planed/actual" Value="5" ImageUrl="~/Images/import.png" />                            
                            </Buttons>
                        </telerik:RadToolBarDropDown>
                        
                        <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                        <telerik:RadToolBarButton runat="server" Value="ShowAll" >
                            <ItemTemplate>
                                <div style="display: inline-block">
                                <img src="Images/project.png"/> Selected Project: 
                                <asp:DropDownList ID="ddlProject" runat="server" Width="200px" AutoPostBack="True" 
                                        OnSelectedIndexChanged="ddlProject_OnSelectedIndexChanged"
                                        />
                                </div>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        
                        <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                        <telerik:RadToolBarButton runat="server" Value="RecoveryPlan" Visible="False">
                            <ItemTemplate>
                                <div style="display: inline-block">
                                <img src="Images/recoveryplan.png"/>Selected Recovery Plan: 
                                <asp:DropDownList ID="ddlRecoveryPlan" runat="server" Width="200px" AutoPostBack="True" 
                                        OnSelectedIndexChanged="ddlRecoveryPlan_OnSelectedIndexChanged"
                                        />
                                </div>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        
                        <telerik:RadToolBarButton runat="server" Text="Update Recovery Plan" Value="1" ImageUrl="~/Images/updaterecoveryplan.png" Visible="False"/>

                    </Items>
                </telerik:RadToolBar>
            </telerik:RadPane>            
            <telerik:RadPane ID="RadPane2" runat="server" Scrollable="false" Scrolling="None">
                <table width="100%">
                        <tr>
                            <td align="center"><h1><b>Working Management Overview Report | Cut off <asp:Label runat="server" ID="lblCurrentDate" /></b></h1></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="1400" style="height: 1000px">
                                    <tr>
                                        <td id="zoneID1"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                    <telerik:RadWindowManager ID="RadWindowManager2" runat="server" EnableShadow="true" RestrictionZoneID="zoneID1"  Style="z-index: 1">
                        <Windows>
                             
                            <telerik:RadWindow ID="MRWindow" runat="server" VisibleOnPageLoad="true" Width="450" Height="255" Top="10" Left="0" InitialBehaviors="Pin"
                            VisibleStatusbar="False" Title="MR" IconUrl="~/Images/material.png"
                            Behaviors="Pin,Move" Skin="Office2007">
                                <ContentTemplate>
                                    <table width="100%" style="height: 50px">
                                        <tr>
                                            <td align="center" ><b>Pending</b></td>
                                            <td align="center" ><b>Overdue</b></td>
                                            <td align="center" ><b>Canceled</b></td>
                                            <td align="center" ><b>Completed</b></td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="font-size: 20pt; color: yellow; font-weight: bold;"><asp:Label runat="server" ID="lblMRPending" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: orange;font-weight: bold"><asp:Label runat="server" ID="lblMROverdue" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: red;font-weight: bold"><asp:Label runat="server" ID="lblMRCancel" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: green;font-weight: bold"><asp:Label runat="server" ID="lblMRCompleted" Text="0"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <telerik:RadHtmlChart runat="server" ID="MRChart" Width="390" Height="140" Transitions="true" Skin="Office2007">
                                                    <PlotArea>
                                                        <Series>
                                                            <telerik:BarSeries Name="Pending" Stacked="false" Gap="1.5" Spacing="0.4">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Yellow"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center">
                                                                </LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Yellow" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="12"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Overdue">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Orange"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Orange" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="30"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Canceled">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Red"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Red" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="89"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Completed">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Green"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Green" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="89"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                        </Series>
                                                        <Appearance>
                                                            <FillStyle BackgroundColor="Transparent"></FillStyle>
                                                        </Appearance>
                                                        
                                                        <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside"
                                                            MinorTickType="None" Reversed="false" MaxValue="100">
                                                            <LabelsAppearance DataFormatString="{0}%" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                                        </YAxis>
                                                    </PlotArea>
                                                    <Appearance>
                                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                                    </Appearance>
                                                    <Legend>
                                                        <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                                    </Legend>
                                                </telerik:RadHtmlChart>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                 
                        </telerik:RadWindow>

                        <telerik:RadWindow ID="WRWindow" runat="server" VisibleOnPageLoad="true" Width="450" Height="255" Top="10" Left="450" InitialBehaviors="Pin"
                            VisibleStatusbar="False" Title="Work Request"
                            Behaviors="Pin,Move" Skin="Office2007" IconUrl="~/Images/workrequest.png">
                            <ContentTemplate>
                                    <table width="100%" style="height: 50px">
                                        <tr>
                                            <td align="center" ><b>Pending</b></td>
                                            <td align="center" ><b>Overdue</b></td>
                                            <td align="center" ><b>Canceled</b></td>
                                            <td align="center" ><b>Completed</b></td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="font-size: 20pt; color: yellow; font-weight: bold;"><asp:Label runat="server" ID="lblWRPending" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: orange;font-weight: bold"><asp:Label runat="server" ID="lblWROverdue" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: red;font-weight: bold"><asp:Label runat="server" ID="lblWRCancel" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: green;font-weight: bold"><asp:Label runat="server" ID="lblWRCompleted" Text="0"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <telerik:RadHtmlChart runat="server" ID="WRChart" Width="390" Height="140" Transitions="true" Skin="Office2007">
                                                    <PlotArea>
                                                        <Series>
                                                            <telerik:BarSeries Name="Pending" Stacked="false" Gap="1.5" Spacing="0.4">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Yellow"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center">
                                                                </LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Yellow" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="12"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Overdue">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Orange"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Orange" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="30"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Canceled">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Red"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Red" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="89"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Completed">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Green"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Green" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="89"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                        </Series>
                                                        <Appearance>
                                                            <FillStyle BackgroundColor="Transparent"></FillStyle>
                                                        </Appearance>
                                                        
                                                        <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside"
                                                            MinorTickType="None" Reversed="false" MaxValue="100">
                                                            <LabelsAppearance DataFormatString="{0}%" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                                        </YAxis>
                                                    </PlotArea>
                                                    <Appearance>
                                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                                    </Appearance>
                                                    <Legend>
                                                        <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                                    </Legend>
                                                </telerik:RadHtmlChart>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                        </telerik:RadWindow>
                             
                        <telerik:RadWindow ID="ECRWindow" runat="server" VisibleOnPageLoad="true" Width="450" Height="255" Top="10" Left="900" InitialBehaviors="Pin"
                                VisibleStatusbar="False" Title="ECR"
                            Behaviors="Pin,Move" Skin="Office2007" IconUrl="~/Images/trackinglist.png">
                            <ContentTemplate>
                                    <table width="100%" style="height: 50px">
                                        <tr>
                                            <td align="center" ><b>Pending</b></td>
                                            <td align="center" ><b>Overdue</b></td>
                                            <td align="center" ><b>Canceled</b></td>
                                            <td align="center" ><b>Completed</b></td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="font-size: 20pt; color: yellow; font-weight: bold;"><asp:Label runat="server" ID="lblECRPending" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: orange;font-weight: bold"><asp:Label runat="server" ID="lblECROverdue" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: red;font-weight: bold"><asp:Label runat="server" ID="lblECRCancel" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: green;font-weight: bold"><asp:Label runat="server" ID="lblECRCompleted" Text="0"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <telerik:RadHtmlChart runat="server" ID="ECRChart" Width="390" Height="140" Transitions="true" Skin="Office2007">
                                                    <PlotArea>
                                                        <Series>
                                                            <telerik:BarSeries Name="Pending" Stacked="false" Gap="1.5" Spacing="0.4">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Yellow"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center">
                                                                </LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Yellow" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="12"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Overdue">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Orange"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Orange" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="30"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Canceled">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Red"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Red" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="89"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Completed">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Green"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Green" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="89"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                        </Series>
                                                        <Appearance>
                                                            <FillStyle BackgroundColor="Transparent"></FillStyle>
                                                        </Appearance>
                                                        
                                                        <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside"
                                                            MinorTickType="None" Reversed="false" MaxValue="100">
                                                            <LabelsAppearance DataFormatString="{0}%" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                                        </YAxis>
                                                    </PlotArea>
                                                    <Appearance>
                                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                                    </Appearance>
                                                    <Legend>
                                                        <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                                    </Legend>
                                                </telerik:RadHtmlChart>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                        </telerik:RadWindow>
                             
                            <telerik:RadWindow ID="MOCWindow" runat="server" VisibleOnPageLoad="true" Width="450" Height="255" Top="260" Left="0" InitialBehaviors="Pin"
                                VisibleStatusbar="False" Title="MOC"
                            Behaviors="Pin,Move" Skin="Office2007" IconUrl="~/Images/moc.png">
                                <ContentTemplate>
                                    <table width="100%" style="height: 50px">
                                        <tr>
                                            <td align="center" ><b>Pending</b></td>
                                            <td align="center" ><b>Overdue</b></td>
                                            <td align="center" ><b>Canceled</b></td>
                                            <td align="center" ><b>Completed</b></td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="font-size: 20pt; color: yellow; font-weight: bold;"><asp:Label runat="server" ID="lblMOCPending" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: orange;font-weight: bold"><asp:Label runat="server" ID="lblMOCOverdue" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: red;font-weight: bold"><asp:Label runat="server" ID="lblMOCCanceled" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: green;font-weight: bold"><asp:Label runat="server" ID="lblMOCCompleted" Text="0"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <telerik:RadHtmlChart runat="server" ID="MOCChart" Width="390" Height="140" Transitions="true" Skin="Office2007">
                                                    <PlotArea>
                                                        <Series>
                                                            <telerik:BarSeries Name="Pending" Stacked="false" Gap="1.5" Spacing="0.4">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Yellow"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center">
                                                                </LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Yellow" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="12"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Overdue">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Orange"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Orange" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="30"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Canceled">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Red"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Red" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="89"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Completed">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Green"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Green" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="89"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                        </Series>
                                                        <Appearance>
                                                            <FillStyle BackgroundColor="Transparent"></FillStyle>
                                                        </Appearance>
                                                        
                                                        <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside"
                                                            MinorTickType="None" Reversed="false" MaxValue="100">
                                                            <LabelsAppearance DataFormatString="{0}%" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                                        </YAxis>
                                                    </PlotArea>
                                                    <Appearance>
                                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                                    </Appearance>
                                                    <Legend>
                                                        <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                                    </Legend>
                                                </telerik:RadHtmlChart>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                        </telerik:RadWindow>
                            
                            <telerik:RadWindow ID="BRWindow" runat="server" VisibleOnPageLoad="true" Width="450" Height="255" Top="260" Left="450" InitialBehaviors="Pin"
                                VisibleStatusbar="False" Title="Breakdown Report"
                            Behaviors="Pin,Move" Skin="Office2007" IconUrl="~/Images/break1.png">
                                <ContentTemplate>
                                    <table width="100%" style="height: 50px">
                                        <tr>
                                            <td align="center" ><b>Pending</b></td>
                                            <td align="center" ><b>Overdue</b></td>
                                            <td align="center" ><b>Canceled</b></td>
                                            <td align="center" ><b>Completed</b></td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="font-size: 20pt; color: yellow; font-weight: bold;"><asp:Label runat="server" ID="lblBRPending" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: orange;font-weight: bold"><asp:Label runat="server" ID="lblBROverdue" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: red;font-weight: bold"><asp:Label runat="server" ID="lblBRCanceled" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: green;font-weight: bold"><asp:Label runat="server" ID="lblBRCompleted" Text="0"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <telerik:RadHtmlChart runat="server" ID="BRChart" Width="390" Height="140" Transitions="true" Skin="Office2007">
                                                    <PlotArea>
                                                        <Series>
                                                            <telerik:BarSeries Name="Pending" Stacked="false" Gap="1.5" Spacing="0.4">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Yellow"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center">
                                                                </LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Yellow" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="12"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Overdue">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Orange"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Orange" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="30"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Canceled">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Red"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Red" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="89"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Completed">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Green"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Green" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="89"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                        </Series>
                                                        <Appearance>
                                                            <FillStyle BackgroundColor="Transparent"></FillStyle>
                                                        </Appearance>
                                                        
                                                        <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside"
                                                            MinorTickType="None" Reversed="false" MaxValue="100">
                                                            <LabelsAppearance DataFormatString="{0}%" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                                        </YAxis>
                                                    </PlotArea>
                                                    <Appearance>
                                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                                    </Appearance>
                                                    <Legend>
                                                        <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                                    </Legend>
                                                </telerik:RadHtmlChart>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                        </telerik:RadWindow>
                             
                            <telerik:RadWindow ID="SRWindow" runat="server" VisibleOnPageLoad="true" Width="450" Height="255" Top="260" Left="900" InitialBehaviors="Pin"
                                VisibleStatusbar="False" Title="Shutdown Report"
                            Behaviors="Pin,Move" Skin="Office2007" IconUrl="~/Images/shutdown.png">
                                <ContentTemplate>
                                    <table width="100%" style="height: 50px">
                                        <tr>
                                            <td align="center" ><b>Pending</b></td>
                                            <td align="center" ><b>Overdue</b></td>
                                            <td align="center" ><b>Canceled</b></td>
                                            <td align="center" ><b>Completed</b></td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="font-size: 20pt; color: yellow; font-weight: bold;"><asp:Label runat="server" ID="lblSRPending" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: orange;font-weight: bold"><asp:Label runat="server" ID="lblSROverdue" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: red;font-weight: bold"><asp:Label runat="server" ID="lblSRCanceled" Text="0"></asp:Label></td>
                                            <td align="center" style="font-size: 20pt; color: green;font-weight: bold"><asp:Label runat="server" ID="lblSRCompleted" Text="0"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <telerik:RadHtmlChart runat="server" ID="SRChart" Width="390" Height="140" Transitions="true" Skin="Office2007">
                                                    <PlotArea>
                                                        <Series>
                                                            <telerik:BarSeries Name="Pending" Stacked="false" Gap="1.5" Spacing="0.4">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Yellow"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center">
                                                                </LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Yellow" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="12"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Overdue">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Orange"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Orange" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="30"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Canceled">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Red"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Red" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="89"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                            <telerik:BarSeries Name="Completed">
                                                                <Appearance>
                                                                    <FillStyle BackgroundColor="Green"></FillStyle>
                                                                </Appearance>
                                                                <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                                                <TooltipsAppearance BackgroundColor="Green" DataFormatString="{0}%" Color="Black"></TooltipsAppearance>
                                                                <SeriesItems>
                                                                    <telerik:CategorySeriesItem Y="89"></telerik:CategorySeriesItem>
                                                                </SeriesItems>
                                                            </telerik:BarSeries>
                                                        </Series>
                                                        <Appearance>
                                                            <FillStyle BackgroundColor="Transparent"></FillStyle>
                                                        </Appearance>
                                                        
                                                        <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside"
                                                            MinorTickType="None" Reversed="false" MaxValue="100">
                                                            <LabelsAppearance DataFormatString="{0}%" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                                        </YAxis>
                                                    </PlotArea>
                                                    <Appearance>
                                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                                    </Appearance>
                                                    <Legend>
                                                        <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                                    </Legend>
                                                </telerik:RadHtmlChart>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                        </telerik:RadWindow>
                             
                    </Windows>
                </telerik:RadWindowManager>
                
                

                
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
                        <telerik:AjaxUpdatedControl ControlID="MRWindow" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="WRWindow" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ECRWindow" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="MOCWindow" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="BRWindow" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="SRWindow" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="MRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="WRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ECRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="MOCChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="BRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="SRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>

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
        <script src="Scripts/bootstrap.min.js"></script>
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

                if (strText == "Export Progress recovery planed template") {
                    ajaxManager.ajaxRequest("ExportProgress_RecoveryPlaned");
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