<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="EDMs.Web.Dashboard" EnableViewState="true" %>

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
                        <telerik:RadToolBarButton runat="server" Value="ShowAll">
                            <ItemTemplate>
                                <div style="display: inline-block">
                                &nbsp;&nbsp;<img src="Images/project.png"/> &nbsp;&nbsp;Selected Project: 
                                <asp:DropDownList ID="ddlProject" runat="server" Width="500px" AutoPostBack="True" 
                                        OnSelectedIndexChanged="ddlProject_OnSelectedIndexChanged"
                                        />
                                </div>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        
                        <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                        
                        <telerik:RadToolBarButton runat="server" Value="DateFilter" Visible="False" >
                            <ItemTemplate>
                                &nbsp;&nbsp;<img src="Images/calendar.png"/>&nbsp;&nbsp;Date:
                                From&nbsp;&nbsp;
                                <telerik:RadDatePicker ID="txtFromDate"  runat="server" 
                                    ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired" Width="120" OnSelectedDateChanged="txtFromDate_OnSelectedDateChanged" AutoPostBack="True" Skin="Windows7">
                                    <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                    <Calendar runat="server"> 
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today"/> 
                                        </SpecialDays> 
                                    </Calendar> 
                                </telerik:RadDatePicker>
             &nbsp;&nbsp;|&nbsp;&nbsp;
                             To&nbsp;&nbsp;
                                <telerik:RadDatePicker ID="txtToDate"  runat="server" 
                                    ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired" Width="120" OnSelectedDateChanged="txtFromDate_OnSelectedDateChanged" AutoPostBack="True" Skin="Windows7">
                                    <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                    <Calendar runat="server"> 
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today"/>
                                        </SpecialDays> 
                                    </Calendar> 
                                </telerik:RadDatePicker>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>

                        <telerik:RadToolBarButton runat="server" Value="RecoveryPlan" Visible="False">
                            <ItemTemplate>
                                <div style="display: inline-block">
                                <img src="Images/recoveryplan.png"/>Selected Recovery Plan: 
                                <asp:DropDownList ID="ddlRecoveryPlan" runat="server" Width="200px" AutoPostBack="True"  OnSelectedIndexChanged="ddlRecoveryPlan_OnSelectedIndexChanged" />
                                </div>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        
                        <telerik:RadToolBarButton runat="server" Text="Update Recovery Plan" Value="1" ImageUrl="~/Images/updaterecoveryplan.png" Visible="False"/>

                    </Items>
                </telerik:RadToolBar>
            </telerik:RadPane>            
                <telerik:RadPane ID="RadPane2" runat="server" Scrollable="false" Scrolling="None">
                    <telerik:RadTabStrip ID="RadTabStrip1" runat="server"  width="100%" Skin="Windows7" MultiPageID="RadMultiPage1"
                SelectedIndex="0">
                        <Tabs>
                            <telerik:RadTab ImageUrl="~/Images/document1.png" Text="Document Management">
                            </telerik:RadTab>
                            <telerik:RadTab ImageUrl="~/Images/project.png" Text="Project Excution">
                            </telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>
                    
                    <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0">
                        <telerik:RadPageView runat="server"  ID="RadPageView1">
                            <table width="100%" cellspacing="5">
                                <tr>
                                   <td align="right">
                                        <div class="panel panel-primary" style="width: 510px; height: 350px; margin-top: 10px; margin-right: 5px">
                                          <div class="panel-heading" align="left"><img src="Images/document1.png"/>&nbsp;&nbsp;Statistics by Response Status (<b>Date: <asp:Label runat="server" ID="lblCurrentDate" /></b>)</div>
                                          <div class="panel-body">
                                            <telerik:RadHtmlChart runat="server" ID="DocumentChart" Width="490" Height="300" Transitions="true" Skin="Office2007">
                                                <PlotArea>
                                                    <Series>
                                                        <telerik:PieSeries StartAngle="90">
                                                            <LabelsAppearance Position="OutsideEnd" DataFormatString="{0}">
                                                            </LabelsAppearance>

                                                            <TooltipsAppearance Color="White" DataFormatString="{0}"></TooltipsAppearance>
                                                            <SeriesItems>
                                                                <telerik:PieSeriesItem BackgroundColor="#ff9900" Exploded="true" Name="AP" Y="134"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#25a0da" Exploded="false" Name="AWC" Y="22"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#cd0f0a" Exploded="false" Name="RFC" Y="67"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#ffd100" Exploded="false" Name="AI" Y="54"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#449941" Exploded="false" Name="AIWC" Y="97"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#ff4cff" Exploded="false" Name="FC" Y="20"/>
                                                            </SeriesItems>
                                                        </telerik:PieSeries>
                                                    </Series>
                                                    
                                                </PlotArea>
                                                <Legend>
                                                    <Appearance BackgroundColor="Transparent" Position="Right"></Appearance>
                                                </Legend>
                                                
                                            </telerik:RadHtmlChart>
                                          </div>
                                        </div>
                                    </td>
                                    
                                    <td align="left">
                                        <div class="panel panel-primary" style="width: 510px; height: 350px; margin-top: 10px">
                                          <div class="panel-heading" align="left"><img src="Images/document1.png"/>&nbsp;&nbsp;Statistics by Category (<b>Date: <asp:Label runat="server" ID="lblCurrentDate1" /></b>)</div>
                                          <div class="panel-body">
                                            <telerik:RadHtmlChart runat="server" ID="RadHtmlChart1" Width="490" Height="300" Transitions="true" Skin="Office2007">
                                                <PlotArea>
                                                    <Series>
                                                        <telerik:PieSeries StartAngle="90">
                                                            <LabelsAppearance Position="OutsideEnd" DataFormatString="{0}">
                                                            </LabelsAppearance>

                                                            <TooltipsAppearance Color="White" DataFormatString="{0}"></TooltipsAppearance>
                                                            <SeriesItems>
                                                                <telerik:PieSeriesItem BackgroundColor="#ff9900" Exploded="true" Name="CORR-CORRESPONDENCE" Y="22"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#25a0da" Exploded="false" Name="ENG-ENGINEERING DOCUMENT" Y="134"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#cd0f0a" Exploded="false" Name="VEN-VENDOR DOCUMENT" Y="67"/>
                                                            </SeriesItems>
                                                        </telerik:PieSeries>
                                                    </Series>
                                                    
                                                </PlotArea>
                                                <Legend>
                                                    <Appearance BackgroundColor="Transparent" Position="Right"></Appearance>
                                                </Legend>
                                                
                                            </telerik:RadHtmlChart>
                                          </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <div class="panel panel-primary" style="width: 510px; height: 350px; margin-top: 10px; margin-right: 5px">
                                          <div class="panel-heading" align="left"><img src="Images/document1.png"/>&nbsp;&nbsp;Statistics by Group (<b>Date: <asp:Label runat="server" ID="lblCurrentDate2" /></b>)</div>
                                          <div class="panel-body">
                                            <telerik:RadHtmlChart runat="server" ID="RadHtmlChart2" Width="490" Height="300" Transitions="true" Skin="Office2007">
                                                <PlotArea>
                                                    <Series>
                                                        <telerik:PieSeries StartAngle="90">
                                                            <LabelsAppearance Position="OutsideEnd" DataFormatString="{0}">
                                                            </LabelsAppearance>

                                                            <TooltipsAppearance Color="White" DataFormatString="{0}"></TooltipsAppearance>
                                                            <SeriesItems>
                                                                <telerik:PieSeriesItem BackgroundColor="#ff9900" Exploded="true" Name="G-General" Y="134"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#25a0da" Exploded="false" Name="C-Civil" Y="22"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#cd0f0a" Exploded="false" Name="M-Mechanical" Y="67"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#ffd100" Exploded="false" Name="E-Electrical" Y="54"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#449941" Exploded="false" Name="IC-Instrument & control" Y="97"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#ff4cff" Exploded="false" Name="CM-Commissioning" Y="20"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#c7f800" Exploded="false" Name="COM-Commercial" Y="32"/>
                                                            </SeriesItems>
                                                        </telerik:PieSeries>
                                                    </Series>
                                                    
                                                </PlotArea>
                                                <Legend>
                                                    <Appearance BackgroundColor="Transparent" Position="Right"></Appearance>
                                                </Legend>
                                                
                                            </telerik:RadHtmlChart>
                                          </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                                
                        </telerik:RadPageView>
                        
                        <telerik:RadPageView runat="server"  ID="RadPageView2">
                            <table width="100%" cellspacing="5">
                                <tr>
                                   <td align="right">
                                        <div class="panel panel-primary" style="width: 510px; height: 350px; margin-top: 10px; margin-right: 5px">
                                          <div class="panel-heading" align="left"><img src="Images/index.png"/>&nbsp;&nbsp;Statistics Change Request by Status (<b>Date: <asp:Label runat="server" ID="lblCurrentDate3" /></b>)</div>
                                          <div class="panel-body">
                                            <telerik:RadHtmlChart runat="server" ID="RadHtmlChart3" Width="490" Height="300" Transitions="true" Skin="Office2007">
                                                <PlotArea>
                                                    <Series>
                                                        <telerik:PieSeries StartAngle="90">
                                                            <LabelsAppearance Position="OutsideEnd" DataFormatString="{0}">
                                                            </LabelsAppearance>

                                                            <TooltipsAppearance Color="White" DataFormatString="{0}"></TooltipsAppearance>
                                                            <SeriesItems>
                                                                <telerik:PieSeriesItem BackgroundColor="#ff9900" Exploded="true" Name="Opening" Y="167"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#25a0da" Exploded="false" Name="Closed" Y="34"/>
                                                            </SeriesItems>
                                                        </telerik:PieSeries>
                                                    </Series>
                                                    
                                                </PlotArea>
                                                <Legend>
                                                    <Appearance BackgroundColor="Transparent" Position="Right"></Appearance>
                                                </Legend>
                                                
                                            </telerik:RadHtmlChart>
                                          </div>
                                        </div>
                                    </td>
                                    
                                    <td align="left">
                                        <div class="panel panel-primary" style="width: 510px; height: 350px; margin-top: 10px">
                                          <div class="panel-heading" align="left"><img src="Images/index.png"/>&nbsp;&nbsp;Statistics Change Request by Review Result (<b>Date: <asp:Label runat="server" ID="lblCurrentDate4" /></b>)</div>
                                          <div class="panel-body">
                                            <telerik:RadHtmlChart runat="server" ID="RadHtmlChart4" Width="490" Height="300" Transitions="true" Skin="Office2007">
                                                <PlotArea>
                                                    <Series>
                                                        <telerik:PieSeries StartAngle="90">
                                                            <LabelsAppearance Position="OutsideEnd" DataFormatString="{0}">
                                                            </LabelsAppearance>

                                                            <TooltipsAppearance Color="White" DataFormatString="{0}"></TooltipsAppearance>
                                                            <SeriesItems>
                                                                <telerik:PieSeriesItem BackgroundColor="#ff9900" Exploded="true" Name="APPROVED" Y="134"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#25a0da" Exploded="false" Name="APPROVED WITH COMMENT" Y="68"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#cd0f0a" Exploded="false" Name="DISAPPROVED" Y="22"/>
                                                            </SeriesItems>
                                                        </telerik:PieSeries>
                                                    </Series>
                                                    
                                                </PlotArea>
                                                <Legend>
                                                    <Appearance BackgroundColor="Transparent" Position="Right"></Appearance>
                                                </Legend>
                                                
                                            </telerik:RadHtmlChart>
                                          </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <div class="panel panel-primary" style="width: 510px; height: 350px; margin-top: 10px; margin-right: 5px">
                                          <div class="panel-heading" align="left"><img src="Images/index.png"/>&nbsp;&nbsp;Statistics NCR by Status (<b>Date: <asp:Label runat="server" ID="lblCurrentDate5" /></b>)</div>
                                          <div class="panel-body">
                                            <telerik:RadHtmlChart runat="server" ID="RadHtmlChart5" Width="490" Height="300" Transitions="true" Skin="Office2007">
                                                <PlotArea>
                                                    <Series>
                                                        <telerik:PieSeries StartAngle="90">
                                                            <LabelsAppearance Position="OutsideEnd" DataFormatString="{0}">
                                                            </LabelsAppearance>

                                                            <TooltipsAppearance Color="White" DataFormatString="{0}"></TooltipsAppearance>
                                                            <SeriesItems>
                                                                <telerik:PieSeriesItem BackgroundColor="#ff9900" Exploded="true" Name="Opening" Y="234"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#25a0da" Exploded="false" Name="Closed" Y="78"/>
                                                            </SeriesItems>
                                                        </telerik:PieSeries>
                                                    </Series>
                                                    
                                                </PlotArea>
                                                <Legend>
                                                    <Appearance BackgroundColor="Transparent" Position="Right"></Appearance>
                                                </Legend>
                                                
                                            </telerik:RadHtmlChart>
                                          </div>
                                        </div>
                                    </td>
                                    
                                    <td align="left">
                                        <div class="panel panel-primary" style="width: 510px; height: 350px; margin-top: 10px; margin-right: 5px">
                                          <div class="panel-heading" align="left"><img src="Images/index.png"/>&nbsp;&nbsp;Statistics SI by Status (<b>Date: <asp:Label runat="server" ID="lblCurrentDate6" /></b>)</div>
                                          <div class="panel-body">
                                            <telerik:RadHtmlChart runat="server" ID="RadHtmlChart6" Width="490" Height="300" Transitions="true" Skin="Office2007">
                                                <PlotArea>
                                                    <Series>
                                                        <telerik:PieSeries StartAngle="90">
                                                            <LabelsAppearance Position="OutsideEnd" DataFormatString="{0}">
                                                            </LabelsAppearance>

                                                            <TooltipsAppearance Color="White" DataFormatString="{0}"></TooltipsAppearance>
                                                            <SeriesItems>
                                                                <telerik:PieSeriesItem BackgroundColor="#ff9900" Exploded="true" Name="Opening" Y="98"/>
                                                                <telerik:PieSeriesItem BackgroundColor="#25a0da" Exploded="false" Name="Closed" Y="12"/>
                                                            </SeriesItems>
                                                        </telerik:PieSeries>
                                                    </Series>
                                                    
                                                </PlotArea>
                                                <Legend>
                                                    <Appearance BackgroundColor="Transparent" Position="Right"></Appearance>
                                                </Legend>
                                                
                                            </telerik:RadHtmlChart>
                                          </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </telerik:RadPageView>
                
                    </telerik:RadMultiPage>

                
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
                        <telerik:AjaxUpdatedControl ControlID="ProjectName" />
                        <telerik:AjaxUpdatedControl ControlID="ProjectId" />
                        <telerik:AjaxUpdatedControl ControlID="lblToDate" />
                        <telerik:AjaxUpdatedControl ControlID="lblFromDate" />


                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="txtToDate">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="WRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ECRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="MOCChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="BRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="SRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="PMChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="lblToDate" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="txtFromDate">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="WRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ECRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="MOCChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="BRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="SRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="PMChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="lblFromDate" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="ddlProject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ProjectName" />
                        <telerik:AjaxUpdatedControl ControlID="ProjectId" />
                        <telerik:AjaxUpdatedControl ControlID="MRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="WRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ECRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="MOCChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="BRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="SRChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="PMChart" LoadingPanelID="RadAjaxLoadingPanel2"/>

                        <telerik:AjaxUpdatedControl ControlID="lblMRPending"/>
                        <telerik:AjaxUpdatedControl ControlID="lblMROverdue"/>
                        <telerik:AjaxUpdatedControl ControlID="lblMRCancel"/>
                        <telerik:AjaxUpdatedControl ControlID="lblMRCompleted"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="lblWRPending"/>
                        <telerik:AjaxUpdatedControl ControlID="lblWROverdue"/>
                        <telerik:AjaxUpdatedControl ControlID="lblWRCancel"/>
                        <telerik:AjaxUpdatedControl ControlID="lblWRCompleted"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="lblECRPending"/>
                        <telerik:AjaxUpdatedControl ControlID="lblECROverdue"/>
                        <telerik:AjaxUpdatedControl ControlID="lblECRCancel"/>
                        <telerik:AjaxUpdatedControl ControlID="lblECRCompleted"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="lblMOCPending"/>
                        <telerik:AjaxUpdatedControl ControlID="lblMOCOverdue"/>
                        <telerik:AjaxUpdatedControl ControlID="lblMOCCanceled"/>
                        <telerik:AjaxUpdatedControl ControlID="lblMOCCompleted"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="lblBRPending"/>
                        <telerik:AjaxUpdatedControl ControlID="lblBROverdue"/>
                        <telerik:AjaxUpdatedControl ControlID="lblBRCanceled"/>
                        <telerik:AjaxUpdatedControl ControlID="lblBRCompleted"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="lblSRPending"/>
                        <telerik:AjaxUpdatedControl ControlID="lblSROverdue"/>
                        <telerik:AjaxUpdatedControl ControlID="lblSRCanceled"/>
                        <telerik:AjaxUpdatedControl ControlID="lblSRCompleted"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="lblPMClosed"/>
                        <telerik:AjaxUpdatedControl ControlID="lblPMOpen"/>
                        <telerik:AjaxUpdatedControl ControlID="lblPMContinous"/>
                        <telerik:AjaxUpdatedControl ControlID="lblPMCancel"/>

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

            function PMChartSeriesClicked(sender, args) {
                var projId = document.getElementById("<%= ProjectId.ClientID %>").value;
                var projName = document.getElementById("<%= ProjectName.ClientID %>").value;


                var seriesName = args.get_seriesName();
                var owd = $find("<%=ChartReport.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.set_title("Operation Meeting Report of \"" + projName + "\" With Status: " + seriesName);
                
                owd.setUrl("Controls/WMS/TrackingProductionMeetingReport.aspx?stausName=" + seriesName + "&projId=" + projId, "ChartReport");
            }

            function GWChartSeriesClicked(sender, args) {
                var projId = document.getElementById("<%= ProjectId.ClientID %>").value;
                var projName = document.getElementById("<%= ProjectName.ClientID %>").value;


                var seriesName = args.get_seriesName();
                var owd = $find("<%=ChartReport.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.set_title("General Working Report of \"" + projName + "\" With Status: " + seriesName);
                
                owd.setUrl("Controls/WMS/TrackingGeneralWorkingReport.aspx?stausName=" + seriesName + "&projId=" + projId, "ChartReport");
            }

            function ProcedureChartSeriesClicked(sender, args) {
                var projId = document.getElementById("<%= ProjectId.ClientID %>").value;
                var projName = document.getElementById("<%= ProjectName.ClientID %>").value;


                var seriesName = args.get_seriesName();
                var owd = $find("<%=ChartReport.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.set_title("Procedure Report of \"" + projName + "\" With Status: " + seriesName);
                
                owd.setUrl("Controls/WMS/TrackingProcedureReport.aspx?stausName=" + seriesName + "&projId=" + projId, "ChartReport");
            }

            function PunchChartSeriesClicked(sender, args) {
                var projId = document.getElementById("<%= ProjectId.ClientID %>").value;
                var projName = document.getElementById("<%= ProjectName.ClientID %>").value;


                var seriesName = args.get_seriesName();
                var owd = $find("<%=ChartReport.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.set_title("Punch List Report of \"" + projName + "\" With Status: " + seriesName);
                
                owd.setUrl("Controls/WMS/TrackingPunchReport.aspx?stausName=" + seriesName + "&projId=" + projId, "ChartReport");
            }

            function WCRChartSeriesClicked(sender, args) {
                var projId = document.getElementById("<%= ProjectId.ClientID %>").value;
                var projName = document.getElementById("<%= ProjectName.ClientID %>").value;


                var seriesName = args.get_seriesName();
                var owd = $find("<%=ChartReport.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.set_title("WCR Report of \"" + projName + "\" With Status: " + seriesName);
                
                owd.setUrl("Controls/WMS/TrackingWCRReport.aspx?stausName=" + seriesName + "&projId=" + projId, "ChartReport");
            }

            function MorningCallChartSeriesClicked(sender, args) {
                var projId = document.getElementById("<%= ProjectId.ClientID %>").value;
                var projName = document.getElementById("<%= ProjectName.ClientID %>").value;


                var seriesName = args.get_seriesName();
                var owd = $find("<%=ChartReport.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.set_title("Morning Call Report of \"" + projName + "\" With Status: " + seriesName);
                
                owd.setUrl("Controls/WMS/TrackingMorningCallReport.aspx?stausName=" + seriesName + "&projId=" + projId, "ChartReport");
            }

            function MRChartSeriesClicked(sender, args) {
                var projId = document.getElementById("<%= ProjectId.ClientID %>").value;
                var projName = document.getElementById("<%= ProjectName.ClientID %>").value;
                var fromDate = document.getElementById("<%= lblFromDate.ClientID %>").value;
                var todate = document.getElementById("<%= lblToDate.ClientID %>").value;


                var seriesName = args.get_seriesName();
                var owd = $find("<%=ChartReport.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.set_title("Material Requisition Overview Report of \"" + projName + "\" - From: " + fromDate + " | To: "+ todate +" - With Status: " + seriesName);
                
                owd.setUrl("Controls/WMS/MRReport.aspx?stausName=" + seriesName + "&projId=" + projId, "ChartReport");
            }

            function WRChartSeriesClicked(sender, args) {
                var projId = document.getElementById("<%= ProjectId.ClientID %>").value;
                var projName = document.getElementById("<%= ProjectName.ClientID %>").value;
                var fromDate = document.getElementById("<%= lblFromDate.ClientID %>").value;
                var todate = document.getElementById("<%= lblToDate.ClientID %>").value;

                var seriesName = args.get_seriesName();
                var owd = $find("<%=ChartReport.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.set_title("Work Request Overview Report of \"" + projName + "\" - From: " + fromDate + " | To: " + todate + " - With Status: " + seriesName);
                
                owd.setUrl("Controls/WMS/WRReport.aspx?stausName=" + seriesName + "&projId=" + projId, "ChartReport");
            }

            function ECRChartSeriesClicked(sender, args) {
                var projId = document.getElementById("<%= ProjectId.ClientID %>").value;
                var projName = document.getElementById("<%= ProjectName.ClientID %>").value;
                var fromDate = document.getElementById("<%= lblFromDate.ClientID %>").value;
                var todate = document.getElementById("<%= lblToDate.ClientID %>").value;

                var seriesName = args.get_seriesName();
                var owd = $find("<%=ChartReport.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.set_title("ECR Overview Report of \"" + projName + "\" - From: " + fromDate + " | To: " + todate + " - With Status: " + seriesName);
                
                owd.setUrl("Controls/WMS/TrackingECRReport.aspx?stausName=" + seriesName + "&projId=" + projId, "ChartReport");
            }

            function MOCChartSeriesClicked(sender, args) {
                var projId = document.getElementById("<%= ProjectId.ClientID %>").value;
                var projName = document.getElementById("<%= ProjectName.ClientID %>").value;
                var fromDate = document.getElementById("<%= lblFromDate.ClientID %>").value;
                var todate = document.getElementById("<%= lblToDate.ClientID %>").value;

                var seriesName = args.get_seriesName();
                var owd = $find("<%=ChartReport.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.set_title("MOC Overview Report of \"" + projName + "\" - From: " + fromDate + " | To: " + todate + " - With Status: " + seriesName);
                
                owd.setUrl("Controls/WMS/TrackingMOCReport.aspx?stausName=" + seriesName + "&projId=" + projId, "ChartReport");
            }

            function BRChartSeriesClicked(sender, args) {
                var projId = document.getElementById("<%= ProjectId.ClientID %>").value;
                var projName = document.getElementById("<%= ProjectName.ClientID %>").value;
                var fromDate = document.getElementById("<%= lblFromDate.ClientID %>").value;
                var todate = document.getElementById("<%= lblToDate.ClientID %>").value;

                var seriesName = args.get_seriesName();
                var owd = $find("<%=ChartReport.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.set_title("Breakdown Report Overview Report of \"" + projName + "\" - From: " + fromDate + " | To: " + todate + " - With Status: " + seriesName);
                
                owd.setUrl("Controls/WMS/TrackingBreakdownReportOverview.aspx?stausName=" + seriesName + "&projId=" + projId, "ChartReport");
            }

            function SRChartSeriesClicked(sender, args) {
                var projId = document.getElementById("<%= ProjectId.ClientID %>").value;
                var projName = document.getElementById("<%= ProjectName.ClientID %>").value;
                var fromDate = document.getElementById("<%= lblFromDate.ClientID %>").value;
                var todate = document.getElementById("<%= lblToDate.ClientID %>").value;

                var seriesName = args.get_seriesName();
                var owd = $find("<%=ChartReport.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.set_title("Shutdown Report Overview Report of \"" + projName + "\" - From: " + fromDate + " | To: " + todate + " - With Status: " + seriesName);
                
                owd.setUrl("Controls/WMS/TrackingShutdownReportOverview.aspx?stausName=" + seriesName + "&projId=" + projId, "ChartReport");
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