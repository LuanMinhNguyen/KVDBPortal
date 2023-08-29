<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IntegrateSyncConfig.aspx.cs" Inherits="EDMs.Web.Controls.Security.IntegrateSyncConfig" EnableViewState="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
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

        #RAD_SPLITTER_PANE_CONTENT_ctl00_ContentPlaceHolder2_RadPane3 {
            overflow: auto !important;
        }

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_btnTestConnectPanel {
            display: initial !important;
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
            /*background: transparent url(~/Images/background.png) no-repeat top left;*/
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
            font-size: 1.0em;
            font-weight: bold;
            letter-spacing: 0.1em;
            line-height: 1.2;
            margin: 0.5em auto 0.6em;
            padding: 0;
            text-align: left;
            text-decoration: none;
            display: block;
        }

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_btnSaveAmosConfigPanel {
            display: initial !important;
        }

    </style>
    
    <telerik:RadPanelBar ID="radPbCostContract" runat="server" Width="100%"/>
    <telerik:RadPanelBar ID="radPbScope" runat="server" Width="100%"/>
    <telerik:RadPanelBar ID="radPbList" runat="server" Width="100%" Visible="False"/>
    <telerik:RadPanelBar ID="radPbSystem" runat="server" Width="100%"/>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Horizontal" Width="100%">
        <telerik:RadPane ID="RadPane3" runat="server" Height="100%" Scrollable="false" Scrolling="None">
            <dl class="accordion">
                <dt style="width: 100%;">
                    <span>CONFIG INTEGRATE WITH AMOS DATA</span>
                </dt>
            </dl>
            
            <div style="width: 750px;" runat="server" ID="divAmosContent">
                <ul style="list-style-type: none">
                    <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">Data Source Name (DNS)
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:TextBox ID="txtDataSource" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    
                    <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">User Name
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:TextBox ID="txtAmosUid" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    
                    <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">Password
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:TextBox ID="txtAmosPwd" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    
                    <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">Default AMOS Dept ID
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:TextBox ID="txtAmosDeptId" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>

                    <li style="width: 700px;display: none">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">Data File Path
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:TextBox ID="txtAmosDataFile" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>

                    
                </ul>
            </div>
            <ul style="list-style-type: none">
                    <li style="width: 700px;" runat="server" id="MessControl" Visible="False">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: red; text-align: right; ">Warning: 
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:Label runat="server" ID="lblMess" Width="500"/>
                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    
                     <li style="width: 700px; padding-top: 10px; padding-bottom: 3px; text-align: center">
                        <telerik:RadButton ID="btnTestConnect" runat="server" Text="Test Connection" OnClick="btnTestConnect_OnClick"  Width="150px" style="text-align: center">
                            <Icon PrimaryIconUrl="../../Images/testconnect.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                        </telerik:RadButton>
                         <telerik:RadButton ID="btnSaveAmosConfig" runat="server" Text="Save" OnClick="btnSaveAmosConfig_OnClick"  Width="70px" style="text-align: center">
                            <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                        </telerik:RadButton>
                    </li>
                </ul>
            
            <dl class="accordion">
                <dt style="width: 100%;">
                    <span>CONFIG SYNC DATA CHANGED</span>
                </dt>
            </dl>
            
            <div style="width: 750px;" runat="server" ID="divConfigSync">
                <ul style="list-style-type: none">
                     <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">Export Data Folder
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:TextBox ID="txtExportFolder" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    
                     <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">Import Data Folder
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:TextBox ID="txtImportFolder" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">Default Email
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:TextBox ID="txtEmail" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    
                    <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">Email Name
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:TextBox ID="txtEmailName" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    
                    <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">Password
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:TextBox ID="txtPass" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    
                    <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">SMTP Mail Server
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:TextBox ID="txtMailServer" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    
                    <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">SMTP Port
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:TextBox ID="txtPort" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    
                    <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">POP3 Mail Server
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:TextBox ID="txtPopMailServer" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    
                    <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">POP3 Port
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:TextBox ID="txtPopPort" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    
                    <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:CheckBox runat="server" ID="cbUseDefaultCredentials" Text="Use Default Credentials"/>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li> 
                    
                    <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:CheckBox runat="server" ID="cbEnableSsl" Text="Enable SSL"/>
                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    
                    <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:CheckBox runat="server" ID="cbAuto" Text="Auto Send/Get Data Changed File"/>
                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    
                    <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:CheckBox runat="server" ID="cbEnableNotification" Text="Enable Send Notification"/>
                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>

                    <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">Send Export Data File To (Email)
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:TextBox ID="txtEmailSendExport" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    
                    <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">Connect String To Import Data File
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <asp:TextBox ID="txtConnStr" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    
                    
                </ul>
            </div>

            <ul style="list-style-type: none">
                <li style="width: 700px; padding-top: 10px; padding-bottom: 3px; text-align: center">
                    <telerik:RadButton ID="btnSaveSyncConfig" runat="server" Text="Save" OnClick="btnSaveSyncConfig_OnClick"  Width="70px" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                </li>
            </ul>
            
            <ul style="list-style-type: none">
                <li style="width: 700px;">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">Time To Export Data Change (Daily)
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <telerik:RadTimePicker ID="txtTimeExport" Width="70px" DateInput-DateFormat="HH:mm" runat="server"/> 
                                <telerik:RadButton ID="btnAddTimeExport" runat="server" OnClick="btnAddTimeExport_OnClick"  Width="23px" style="text-align: center">
                                    <Icon PrimaryIconUrl="../../Images/addNew.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                </telerik:RadButton>
                                <telerik:RadGrid ID="grdTimeExport" runat="server" AllowPaging="False"
                                    AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                    GridLines="None" Height="100" Width="200" 
                                    OnNeedDataSource="grdTimeExport_OnNeedDataSource"
                                    OnDeleteCommand="grdTimeExport_OnDeleteCommand"
                                    Style="outline: none">
                                    <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" TableLayout="Auto" CssClass="rgMasterTable">
                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <Columns>
                                            <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" HeaderTooltip="Delete Export Time item"
                                                ConfirmText="Do you want to delete Export time?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png" >
                                                <HeaderStyle Width="25" />
                                                <ItemStyle HorizontalAlign="Center" Width="25"  />
                                            </telerik:GridButtonColumn>
                                            <telerik:GridDateTimeColumn HeaderText="Time" UniqueName="ExportTime" DataField="ExportTime"
                                            DataFormatString="{0:HH:mm}">
                                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </telerik:GridDateTimeColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings Selecting-AllowRowSelect="true">
                                        <Selecting AllowRowSelect="true" />
                                        <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>
                    
                    <li style="width: 700px;display: none">
                        <div>
                            <label style="width: 150px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                <span style="color: #2E5689; text-align: right; ">Time To Import Data Change (Daily)
                                </span>
                            </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                <telerik:RadTimePicker ID="txtImportTime" Width="70px" DateInput-DateFormat="HH:mm" runat="server"/> 
                                <telerik:RadButton ID="btnAddImportTime" runat="server" OnClick="btnAddImportTime_OnClick"  Width="23px" style="text-align: center">
                                    <Icon PrimaryIconUrl="../../Images/addNew.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                </telerik:RadButton>
                                <telerik:RadGrid ID="grdImportTime" runat="server" AllowPaging="False"
                                    AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                    GridLines="None" Height="100" Width="200" 
                                    OnNeedDataSource="grdImportTime_OnNeedDataSource" 
                                    OnDeleteCommand="grdImportTime_OnDeleteCommand"
                                    Style="outline: none">
                                    <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" TableLayout="Auto" CssClass="rgMasterTable">
                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <Columns>
                                            <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" HeaderTooltip="Delete Import Time item"
                                                ConfirmText="Do you want to delete Import time?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png" >
                                                <HeaderStyle Width="25" />
                                                <ItemStyle HorizontalAlign="Center" Width="25"  />
                                            </telerik:GridButtonColumn>
                                            <telerik:GridDateTimeColumn HeaderText="Time" UniqueName="ImportTime" DataField="ImportTime"
                                            DataFormatString="{0:HH:mm}">
                                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </telerik:GridDateTimeColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings Selecting-AllowRowSelect="true">
                                        <Selecting AllowRowSelect="true" />
                                        <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                                    </ClientSettings>
                                </telerik:RadGrid>

                            </div>
                        </div>
                        <div style="clear: both; font-size: 0;"></div>
                    </li>

            </ul>

        </telerik:RadPane>
        
    </telerik:RadSplitter>

    <span style="display: none">
        

        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>

            <AjaxSettings>
                
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radMenu">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument"  LoadingPanelID="RadAjaxLoadingPanel2"  ></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContainer" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdTimeExport">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdTimeExport" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnAddTimeExport">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdTimeExport" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdImportTime">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdImportTime" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnAddImportTime">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdImportTime" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnTestConnect">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MessControl"/>
                        <telerik:AjaxUpdatedControl ControlID="divAmosContent" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="btnTestConnect" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSaveAmosConfig">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MessControl"/>
                        <telerik:AjaxUpdatedControl ControlID="divAmosContent" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="btnSaveAmosConfig" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSaveSyncConfig">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divConfigSync" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="btnSaveSyncConfig" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </span>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Material Requisition Type Information"
                VisibleStatusbar="false" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
    <asp:HiddenField runat="server" ID="FolderContextMenuAction"/>
    <asp:HiddenField runat="server" ID="lblFolderId"/>
    <asp:HiddenField runat="server" ID="lblDocId"/>
    <asp:HiddenField runat="server" ID="lblCategoryId"/>
    <input type="hidden" id="radGridClickedRowIndex" name="radGridClickedRowIndex"/>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="../../Scripts/jquery-1.7.1.js"></script>
        <script type="text/javascript">
            
            var radDocuments;

            function GetGridObject(sender, eventArgs) {
                radDocuments = sender;
            }

            function onColumnHidden(sender) {
                
                var masterTableView = sender.get_masterTableView().get_element();
                masterTableView.style.tableLayout = "auto";
                //window.setTimeout(function () { masterTableView.style.tableLayout = "auto"; }, 0);
            }

            // Undocked and Docked event slide bar Tree folder
            function OnClientUndocked(sender, args) {
                radDocuments.get_masterTableView().showColumn(7);
                radDocuments.get_masterTableView().showColumn(8);
                radDocuments.get_masterTableView().showColumn(9);
                
                radDocuments.get_masterTableView().showColumn(11);
                radDocuments.get_masterTableView().showColumn(12);
            }
            
            function OnClientDocked(sender, args) {
                radDocuments.get_masterTableView().hideColumn(7);
                radDocuments.get_masterTableView().hideColumn(8);
                radDocuments.get_masterTableView().hideColumn(9);
                
                radDocuments.get_masterTableView().hideColumn(11);
                radDocuments.get_masterTableView().hideColumn(12);
            }

            function RowClick(sender, eventArgs) {
                var Id = eventArgs.getDataKeyValue("ID");
                document.getElementById("<%= lblDocId.ClientID %>").value = Id;
            }
            
            
            function Set_Cookie(name, value, expires, path, domain, secure) {
                // set time, it's in milliseconds
                var today = new Date();
                today.setTime(today.getTime());

                /*
                if the expires variable is set, make the correct 
                expires time, the current script below will set 
                it for x number of days, to make it for hours, 
                delete * 24, for minutes, delete * 60 * 24
                */
                if (expires) {
                    expires = expires * 1000 * 60 * 60 * 24;
                }
                var expires_date = new Date(today.getTime() + (expires));

                document.cookie = name + "=" + escape(value) +
                    ((expires) ? ";expires=" + expires_date.toGMTString() : "") +
                    ((path) ? ";path=" + path : "") +
                    ((domain) ? ";domain=" + domain : "") +
                    ((secure) ? ";secure" : "");
            }

            function onRequestStart(sender, args) {
                ////alert(args.get_eventTarget());
                if (args.get_eventTarget().indexOf("ExportTo") >= 0 || args.get_eventTarget().indexOf("btnDownloadPackage") >= 0 ||
                    args.get_eventTarget().indexOf("ajaxCustomer") >= 0) {
                    args.set_enableAjax(false);
                }
            }
        </script>
        <script type="text/javascript">
            /* <![CDATA[ */
            var toolbar;
            var searchButton;
            var ajaxManager;

            function pageLoad() {
                $('iframe').load(function () { //The function below executes once the iframe has finished loading<---true dat, althoug Is coppypasta from I don't know where
                    //alert($('iframe').contents());
                });

                ajaxManager = $find("<%=ajaxCustomer.ClientID %>");
            }

            function ShowEditForm(id) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("MRTypeEditForm.aspx?disId=" + id, "CustomerDialog");
            }
            

            function RowDblClick(sender, eventArgs) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Customers/ViewCustomerDetails.aspx?docId=" + eventArgs.getDataKeyValue("Id"), "CustomerDialog");
                // window.radopen("Controls/Customers/ViewCustomerDetails.aspx?patientId=" + eventArgs.getDataKeyValue("Id"), "CustomerDialog");
            }
            
            
            
            function radPbCategories_OnClientItemClicking(sender, args)
            {
                var item = args.get_item();
                var categoryId = item.get_value();
                document.getElementById("<%= lblCategoryId.ClientID %>").value = categoryId;
                
            }
            
            function OnClientButtonClicking(sender, args) {
                var button = args.get_item();
                var strText = button.get_text();
                var strValue = button.get_value();

                if (strValue == "ExportDataChange") {
                    ajaxManager.ajaxRequest("ExportDataChange");
                }
            }
            
            function performSearch(searchTextBox) {
                if (searchTextBox.get_value()) {
                    searchButton.set_imageUrl("~/Images/clear.gif");
                    searchButton.set_value("clear");
                }

                ajaxManager.ajaxRequest(searchTextBox.get_value());
            }
            function onTabSelecting(sender, args) {
                if (args.get_tab().get_pageViewID()) {
                    args.get_tab().set_postBack(false);
                }
            }
            
        /* ]]> */
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
