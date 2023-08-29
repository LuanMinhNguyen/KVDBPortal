<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="EDMs.Web.Dashboard" EnableViewState="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        #ctl00_ContentPlaceHolder2_RadPane1 {
            border-style: none
        }
        
        #ctl00_ContentPlaceHolder2_RadPane2 {
            border-style: none
        }
        .dashboard {
            padding: 5px;
            margin: 10px;
            border-color: #c2c2c2;
            border-style: solid;
            border-width: 1px;
            background-color: white;
        }

        .divTitle {
            font-size: 16px;
            font-weight: normal;
            font-family: helvetica, arial, verdana, sans-serif;
            line-height: 28px;
            display: inline;
            overflow: hidden;
            float: left;
            padding-left: 20px;
        }

        .btnRefresh {
            background-position: 0 -270px;
            overflow: hidden;
            width: 18px;
            height: 18px;
            background-image: url(Images/tool-sprites.png);
            margin: 0;
        }
        .divBtnRefresh {
            display: inline;
            float: right;
            padding-top: 3px;
            padding-right: 3px;
        }

        .icon1 {
            width: 16px;
            height: 16px;
            background-image: url(Images/icon_inverse.png);
            background-repeat: no-repeat;
            background-color: #D66221;
            float: left;
            margin: 2px 8px 0 0 !important;
        }

        .icon2 {
            width: 16px;
            height: 16px;
            background-image: url(Images/icon_inverse.png);
            background-repeat: no-repeat;
            background-color: #56932A;
            float: left;
            margin: 2px 8px 0 0 !important;
        }

        .icon3 {
            width: 16px;
            height: 16px;
            background-image: url(Images/icon_inverse.png);
            background-repeat: no-repeat;
            background-color: #2578A9;
            float: left;
            margin: 2px 8px 0 0 !important;
        }

        .icon4 {
            width: 16px;
            height: 16px;
            background-image: url(Images/icon_inverse.png);
            background-repeat: no-repeat;
            background-color: #DE8181;
            float: left;
            margin: 2px 8px 0 0 !important;
        }

        .icon5 {
            width: 16px;
            height: 16px;
            background-image: url(Images/icon_inverse.png);
            background-repeat: no-repeat;
            background-color: #EFA836;
            float: left;
            margin: 2px 8px 0 0 !important;
        }
        
        .icon6 {
            width: 16px;
            height: 16px;
            background-image: url(Images/icon_inverse.png);
            background-repeat: no-repeat;
            background-color: #C65F5F;
            float: left;
            margin: 2px 8px 0 0 !important;
        }

        .icon7 {
            width: 16px;
            height: 16px;
            background-image: url(Images/icon_inverse.png);
            background-repeat: no-repeat;
            background-color: #89BF65;
            float: left;
            margin: 2px 8px 0 0 !important;
        }

        .icon8 {
            width: 16px;
            height: 16px;
            background-image: url(Images/icon_inverse.png);
            background-repeat: no-repeat;
            background-color: #AD4242;
            float: left;
            margin: 2px 8px 0 0 !important;
        }

        .icon9 {
            width: 16px;
            height: 16px;
            background-image: url(Images/icon_inverse.png);
            background-repeat: no-repeat;
            background-color: #397514;
            float: left;
            margin: 2px 8px 0 0 !important;
        }
        
        .icon10 {
            width: 16px;
            height: 16px;
            background-image: url(Images/icon_inverse.png);
            background-repeat: no-repeat;
            background-color: #368AC0;
            float: left;
            margin: 2px 8px 0 0 !important;
        }

        .RadListBox .rlbItem {
            height: 30px;
            border-bottom-width: 1px !important;
            border-style: solid;
            border-width: 0px;
            border-color: #d8d8d8;
        }

        .lblGauge {
            font-size: 16px;
            font-weight: normal;
            font-family: helvetica, arial, verdana, sans-serif;
            font-weight: bold;
            color: #2578A9;
        }

        #ctl00_ContentPlaceHolder2_Gauge1 {
            margin-bottom: -40px;
        }
        #ctl00_ContentPlaceHolder2_Gauge2 {
            margin-bottom: -40px;
        }
        #ctl00_ContentPlaceHolder2_Gauge3 {
            margin-bottom: -40px;
        }
        #ctl00_ContentPlaceHolder2_Gauge4 {
            margin-bottom: -40px;
        }
        #ctl00_ContentPlaceHolder2_Gauge5 {
            margin-bottom: -40px;
        }
    </style>

    <div style=" width: 100%; height: 100%; background-color: #f0f0f0">
        <telerik:RadSplitter RenderMode="Lightweight" ID="MainSplitter" runat="server" Height="100%" Width="100%">
            <telerik:RadPane ID="RadPane1" runat="server" Width="500" Locked="True">
                <div class="dashboard" style=" height: 95% ">
                    <div style="border-color: #d8d8d8;border-bottom-width: 1px !important;height: 35px; border-style: solid;border-width: 0px">
                        <div class="divTitle" style="width: 400px">Thông Báo</div>
                        <div class="divBtnRefresh">
                            <asp:ImageButton runat="server" ImageUrl="Images/refresh_20.png" ID="btnRefreshInbox" OnClick="btnRefreshInbox_OnClick"/>
                        </div>
                    </div>
                    <div style="margin-top: 10px;height: 50px">Thư mục: 
                        <telerik:RadComboBox ID="ddlFolder" runat="server" Width="414" Skin="MetroTouch" OnSelectedIndexChanged="ddlFolder_OnSelectedIndexChanged" AutoPostBack="True">
                            <Items>
                                <telerik:RadComboBoxItem runat="server" Text="Vận Hành" Value="1"/>
                                <telerik:RadComboBoxItem runat="server" Text="Theo Dõi Mua Hàng" Value="2"/>
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                    <div>
                        <telerik:RadListBox RenderMode="Lightweight" ID="lbFolder" runat="server" Height="100%" Width="100%" Skin="MetroTouch">
                            <ItemTemplate>
                                <div class='<%# DataBinder.Eval(Container, "Attributes['Color']") %>' ></div>
                                <div style="display: inline;"><%# DataBinder.Eval(Container, "Text")%></div>
                                <div style="float: right;display: inline;"><%# DataBinder.Eval(Container, "Value")%></div>
                            </ItemTemplate>
                            
                        </telerik:RadListBox>
                    </div>
                </div>
            </telerik:RadPane>
            <telerik:RadPane ID="RadPane3" runat="server" Scrolling="Both">
                <telerik:RadSplitter RenderMode="Lightweight" ID="Radsplitter3" runat="server" Height="100%" Width="100%" Orientation="Horizontal">
                    <telerik:RadPane ID="RadPane2" runat="server" Height="390" Locked="True">
                        <div class="dashboard" style="height: 94%">
                            <div style="border-color: #d8d8d8;border-bottom-width: 1px !important;height: 35px; border-style: solid;border-width: 0px">
                                <div class="divTitle" style="width: 400px">Biểu Đồ</div>
                                <div class="divBtnRefresh">
                                    <asp:ImageButton runat="server" ImageUrl="Images/refresh_20.png" ID="btnRefreshChart" OnClick="btnRefreshChart_OnClick"/>
                                </div>
                            </div>
                            <div>
                                <div style="display: inline; float: left">
                                    <telerik:RadHtmlChart runat="server" ID="ColumnChart" Width="600" Height="300" Transitions="true" Skin="MetroTouch">
                                        <PlotArea>
                                            <Series>
                                                <telerik:ColumnSeries Name="" Stacked="false" Gap="0.4" Spacing="0.2" DataFieldY="TotalValue">
                                                    <Appearance>
                                                        <FillStyle BackgroundColor="#2578A9"></FillStyle>
                                                    </Appearance>
                                                    <TooltipsAppearance DataFormatString="{0}" Color="White"></TooltipsAppearance>
                                                </telerik:ColumnSeries>
                                            </Series>
                                            <Appearance>
                                                <FillStyle BackgroundColor="Transparent"></FillStyle>
                                            </Appearance>
                                            <XAxis AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside"
                                                Reversed="false">
                                                <Items>
                                                    <telerik:AxisItem LabelText="Jan"/>
                                                    <telerik:AxisItem LabelText="Feb"/>
                                                    <telerik:AxisItem LabelText="Mar"/>
                                                    <telerik:AxisItem LabelText="Apr"/>
                                                    <telerik:AxisItem LabelText="May"/>
                                                    <telerik:AxisItem LabelText="Jun"/>
                                                    <telerik:AxisItem LabelText="Jul"/>
                                                    <telerik:AxisItem LabelText="Aug"/>
                                                    <telerik:AxisItem LabelText="Sep"/>
                                                    <telerik:AxisItem LabelText="Oct"/>
                                                    <telerik:AxisItem LabelText="Nov"/>
                                                    <telerik:AxisItem LabelText="Dec"/>
                                                </Items>
                                                <LabelsAppearance RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                                <TitleAppearance Position="Center" RotationAngle="0" Text="2020">
                                                </TitleAppearance>
                                            </XAxis>

                                        </PlotArea>
                                        <Appearance>
                                            <FillStyle BackgroundColor="Transparent"></FillStyle>
                                        </Appearance>
                                        <ChartTitle Text="Kế hoạch bảo trì, kiểm tra theo định kỳ các bộ phận">
                                            <Appearance Align="Left" BackgroundColor="Transparent" Position="Top">
                                            </Appearance>
                                        </ChartTitle>
                                        <Legend>
                                            <Appearance BackgroundColor="Transparent" Position="Bottom">
                                            </Appearance>
                                        </Legend>
                                    </telerik:RadHtmlChart>
                                </div>
                        
                                <div style="display: inline; float: left">
                                    <div style="border-color: #d8d8d8;border-bottom-width: 1px !important;height: 35px; border-style: solid;border-width: 0px">
                                        <div class="divTitle" style="width: 400px">Quản Lý Biểu Đồ</div>
                                    </div>
                                    <div style="margin-top: 10px;height: 50px">
                                        <div style="display: inline; float: left; width: 100px;padding-top: 10px;padding-left: 22px;">Kiểu biểu đồ</div>
                                        <div style="display: inline; float: left">
                                            <telerik:RadComboBox runat="server" Width="290" Skin="MetroTouch" ID="ddlColumnChart" OnSelectedIndexChanged="ddlColumnChart_OnSelectedIndexChanged" AutoPostBack="True">
                                                <Items>
                                                    <telerik:RadComboBoxItem runat="server" Value="1" Text="Thống kê kế hoạch bảo trì, kiểm tra theo định kỳ các bộ phận"/>
                                                    <telerik:RadComboBoxItem runat="server" Value="2" Text="Thống kê khối lượng hoàn thành bảo trì, kiểm tra định kỳ các bộ phận"/>
                                                    <telerik:RadComboBoxItem runat="server" Value="3" Text="Thống kê khối lượng sửa chữa phát sinh ngoài kế hoạch"/>
                                                    <telerik:RadComboBoxItem runat="server" Value="4" Text="Thống kê khối lượng hoàn thành sửa chữa phát sinh ngoài kế hoạch"/>
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                
                                    <div style="margin-top: 10px; height: 50px; display: none">
                                        <div style="display: inline; float: left;width: 100px;padding-top: 10px;padding-left: 22px;">Thời gian</div>
                                        <div style="display: inline; float: left">
                                            <telerik:RadComboBox runat="server" Width="290" Skin="MetroTouch"></telerik:RadComboBox>
                                        </div>
                                    </div>
                                
                                    <div style="margin-top: 10px;height: 50px; display: none">
                                        <div style="display: inline; float: left;width: 100px;padding-top: 10px;padding-left: 22px;">Bộ phận</div>
                                        <div style="display: inline; float: left">
                                            <telerik:RadComboBox runat="server" Width="290" Skin="MetroTouch"></telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                    
                        </div>
                    </telerik:RadPane>
                    
                    <telerik:RadPane ID="RadPane4" runat="server">
                        <div class="dashboard" style="height: 91%">
                            <div style="display: inline; float: left; text-align: center">
                                <telerik:RadRadialGauge runat="server" ID="Gauge1" Width="200px" Height="200px">
                                    <Pointer Value="23">
                                        <Cap Size="0.1" />
                                    </Pointer>
                                    <Scale Min="0" Max="100" MajorUnit="25">
                                        <Labels Format="{0} %" />
                                        <Ranges>
                                            <telerik:GaugeRange Color="#8dcb2a" From="0" To="25" />
                                            <telerik:GaugeRange Color="#ffc700" From="26" To="50" />
                                            <telerik:GaugeRange Color="#ff7a00" From="51" To="75" />
                                            <telerik:GaugeRange Color="#c20000" From="76" To="100" />
                                        </Ranges>
                                    </Scale>
                                </telerik:RadRadialGauge><br/>
                                <asp:Label runat="server" ID="lbl1" Text="16%" CssClass="lblGauge"/><br/>
                                Tỉ lệ hoàn thành công việc <br/>theo năm các bộ phận
                            </div>
                            
                            <div style="display: inline; float: left; text-align: center">
                                <telerik:RadRadialGauge runat="server" ID="Gauge2" Width="200px" Height="200px">
                                    <Pointer Value="23">
                                        <Cap Size="0.1" />
                                    </Pointer>
                                    <Scale Min="0" Max="100" MajorUnit="25">
                                        <Labels Format="{0} %" />
                                        <Ranges>
                                            <telerik:GaugeRange Color="#8dcb2a" From="0" To="25" />
                                            <telerik:GaugeRange Color="#ffc700" From="26" To="50" />
                                            <telerik:GaugeRange Color="#ff7a00" From="51" To="75" />
                                            <telerik:GaugeRange Color="#c20000" From="76" To="100" />
                                        </Ranges>
                                    </Scale>
                                </telerik:RadRadialGauge><br/>
                                <asp:Label runat="server" ID="lbl2" Text="16%" CssClass="lblGauge"/><br/>
                                Tỉ lệ hoàn thành công việc <br/>theo quý các bộ phận
                            </div>
                            
                            <div style="display: inline; float: left; text-align: center">
                                <telerik:RadRadialGauge runat="server" ID="Gauge3" Width="200px" Height="200px">
                                    <Pointer Value="23">
                                        <Cap Size="0.1" />
                                    </Pointer>
                                    <Scale Min="0" Max="100" MajorUnit="25">
                                        <Labels Format="{0} %" />
                                        <Ranges>
                                            <telerik:GaugeRange Color="#8dcb2a" From="0" To="25" />
                                            <telerik:GaugeRange Color="#ffc700" From="26" To="50" />
                                            <telerik:GaugeRange Color="#ff7a00" From="51" To="75" />
                                            <telerik:GaugeRange Color="#c20000" From="76" To="100" />
                                        </Ranges>
                                    </Scale>
                                </telerik:RadRadialGauge>
                                <br/>
                                <asp:Label runat="server" ID="lbl3" Text="16%" CssClass="lblGauge"/><br/>
                                Tỉ lệ hoàn thành công việc <br/>theo tháng các bộ phận 
                            </div>
                            
                            <div style="display: inline; float: left; text-align: center">
                                <telerik:RadRadialGauge runat="server" ID="Gauge4" Width="200px" Height="200px">
                                    <Pointer Value="23">
                                        <Cap Size="0.1" />
                                    </Pointer>
                                    <Scale Min="0" Max="100" MajorUnit="25">
                                        <Labels Format="{0} %" />
                                        <Ranges>
                                            <telerik:GaugeRange Color="#8dcb2a" From="0" To="25" />
                                            <telerik:GaugeRange Color="#ffc700" From="26" To="50" />
                                            <telerik:GaugeRange Color="#ff7a00" From="51" To="75" />
                                            <telerik:GaugeRange Color="#c20000" From="76" To="100" />
                                        </Ranges>
                                    </Scale>
                                </telerik:RadRadialGauge><br/>
                                <asp:Label runat="server" ID="lbl4" Text="16%" CssClass="lblGauge"/><br/>
                                Tỉ lệ hoàn thành công việc <br/>theo tuần các bộ phận
                            </div >
                            
                            <div style="display: inline; float: left; text-align: center">
                                <telerik:RadRadialGauge runat="server" ID="Gauge5" Width="200px" Height="200px">
                                    <Pointer Value="23">
                                        <Cap Size="0.1" />
                                    </Pointer>
                                    <Scale Min="0" Max="100" MajorUnit="25">
                                        <Labels Format="{0} %" />
                                        <Ranges>
                                            <telerik:GaugeRange Color="#8dcb2a" From="0" To="25" />
                                            <telerik:GaugeRange Color="#ffc700" From="26" To="50" />
                                            <telerik:GaugeRange Color="#ff7a00" From="51" To="75" />
                                            <telerik:GaugeRange Color="#c20000" From="76" To="100" />
                                        </Ranges>
                                    </Scale>
                                </telerik:RadRadialGauge>
                                <br/>
                                <asp:Label runat="server" ID="lbl5" Text="16%" CssClass="lblGauge"/><br/>
                                Tỉ lệ hoàn thành công việc <br/>được phân công theo nhân sự thực hiện 
                            </div >

                        </div>
                    </telerik:RadPane>


                </telerik:RadSplitter>


                
            </telerik:RadPane>
        </telerik:RadSplitter>
        
        

        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="MetroTouch"/>
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
                
                <telerik:AjaxSetting AjaxControlID="ddlFolder">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lbFolder" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnRefreshInbox">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lbFolder" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlColumnChart">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ColumnChart" LoadingPanelID="RadAjaxLoadingPanel2"/>
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