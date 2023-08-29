﻿<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DQRETransmittalList.aspx.cs" Inherits="EDMs.Web.Controls.Document.DQRETransmittalList" EnableViewState="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <!--[if gte IE 8]>
        <style type="text/css">
            #ContentPlaceHolder2_grdIncomingTrans_ctl00_Header{table-layout:auto !important;}
            #ContentPlaceHolder2_grdIncomingTrans_ctl00{table-layout:auto !important;}
        </style>
    <![endif]-->

    <style type="text/css">
        /*Custom CSS of Grid documents for FF browser*/
        #ContentPlaceHolder2_grdIncomingTrans_ctl00_Header{table-layout:auto !important;}
        #ContentPlaceHolder2_grdIncomingTrans_ctl00{table-layout:auto !important;}
        /*End*/
        @-moz-document url-prefix() {
            #ContentPlaceHolder2_grdIncomingTrans_ctl00_Header{table-layout:auto !important;}
            #ContentPlaceHolder2_grdIncomingTrans_ctl00{table-layout:auto !important;}
        }
        
        #RAD_SPLITTER_PANE_CONTENT_ctl00_rightPane {
            overflow: hidden !important;
        }
        .RadGrid_Windows7 .rgGroupHeader {
            line-height: 19px !important;
        }
        .rgExpandCol {
            width: 1% !important;
        }

        .rgGroupCol {
            width: 1% !important;
        }

        #ContentPlaceHolder2_grdIncomingTrans_ctl00_ctl02_ctl03_txtDate_popupButton {
            display: none;
        }

        .rgExpandCol {
            width: 1% !important;
        }

        .rgGroupCol {
            width: 1% !important;
        }

        .RadGrid .rgSelectedRow
        {
            background-image : none !important;
            background-color: coral !important;
        }

        .rgMasterTable .rgClipCells .rgClipCells {
            table-layout: auto !important;
        }

        .rgGroupCol
        {
            padding-left: 0 !important;
            padding-right: 0 !important;
            font-size:1px !important;
        }
 
        div.RadGrid .rgPager .rgAdvPart     
        {     
        display:none; 
        }

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

        .radwindow
        {
            z-index: 8000 !important;
        }

        .TemplateMenu
        {
            z-index: 10;
        }

        .RadGrid .rgSelectedRow
        {
            background-image : none !important;
            background-color: darkseagreen !important;
        }

       
        #ctl00_ContentPlaceHolder2_grdOutgoingTrans_GridData {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_RadPane1 {
            width: 100% !important;
        }

        .RadGrid .rgSelectedRow
        {
            background-image : none !important;
            background-color: darkseagreen !important;
        }
        .rcbReadOnly {
            border-color: #8E8E8E #B8B8B8 #B8B8B8 #ff0000 !important;
            color: #000000 !important;
            font: 12px "segoe ui" !important;
            border-width: 1px !important;
            border-style: solid !important;
            border-left-width: 5px !important;
            padding: 2px 1px 3px !important;
            vertical-align: middle !important;
            margin: 0 !important;
            padding-left: 5px !important;
            padding-right: 5px !important;
        }

        #ctl00_ContentPlaceHolder2_radMenuOutgoing_i2_btnSearchOutgoing {
            margin-top: -3px
        }
        #ctl00_ContentPlaceHolder2_radMenuIncoming_i2_btnSearchIncoming {
            margin-top: -3px
        }

       #ctl00_ContentPlaceHolder2_IncomingTransView{
            height: 85% !important;
        } 
        #ctl00_ContentPlaceHolder2_OutgoingTransView {
            height: 85% !important;
        }

         #ctl00_ContentPlaceHolder2_grdIncomingTrans_GridData{
            height: 86% !important;
        }
          
         #ctl00_ContentPlaceHolder2_grdOutgoingTrans_GridData {
            height: 84% !important;
        }

         #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdIncomingTransPanel{
            height: 100% !important;
        } #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdOutgoingTransPanel
        {
            height: 100% !important;
        }

    </style>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Horizontal">
        <telerik:RadPane ID="RadPane2" runat="server" Scrollable="false" Scrolling="None">
            <telerik:RadTabStrip ID="RadTabStrip1" runat="server"  width="100%"  MultiPageID="RadMultiPage1"
    SelectedIndex="0">
                <Tabs>
                    <telerik:RadTab ImageUrl="~/Images/incomingTrans.png" Text="Incoming Transmittals" >
                    </telerik:RadTab>
                    <telerik:RadTab ImageUrl="~/Images/outgoingTrans.png" Text="Outgoing Transmittals">
                    </telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
                            
            <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0" Height="100%" Width="100%">
                <telerik:RadPageView runat="server" ID="IncomingTransView">
                    <telerik:RadToolBar ID="radMenuIncoming" runat="server" Width="100%">
                        <Items>
                            <telerik:RadToolBarButton runat="server" IsSeparator="true"/>

                            <telerik:RadToolBarButton runat="server" Value="ShowAll">
                                <ItemTemplate>
                                    &nbsp;&nbsp;<img src="../../Images/filter.png"/>&nbsp;&nbsp;<b style="color: red">Filter: </b> &nbsp;&nbsp;
                            
                                    <img src="../../Images/project.png"/>Selected project:&nbsp;&nbsp;
                                    <telerik:RadComboBox ID="ddlProjectIncoming" runat="server" 
                                        Skin="Windows7" Width="250" AutoPostBack="True" 
                                        OnItemDataBound="ddlProjectIncoming_ItemDataBound"
                                        OnSelectedIndexChanged="ddlProjectIncoming_SelectedIndexChanged"/>&nbsp;&nbsp;|&nbsp;&nbsp;
                                    <b>Show</b>  
                                    <asp:DropDownList ID="ddlStatusIncoming" runat="server" CssClass="rcbReadOnly" Width="150px" style="max-width: 150px"
                                        OnSelectedIndexChanged="ddlStatusIncoming_OnSelectedIndexChanged" AutoPostBack="True">
                                        <Items>
                                            <asp:ListItem Value="All" Text="All Transmittal"></asp:ListItem>
                                            <asp:ListItem Value="WaitingImport" Text="Transmittal Waiting For Import" style="background-color: coral"></asp:ListItem>
                                            <asp:ListItem Value="Imported" Text="Transmittal Imported" style="background-color: greenyellow"></asp:ListItem>
                                            
                                        </Items>
                                    </asp:DropDownList>

                                    &nbsp;&nbsp; | &nbsp;&nbsp;
                                    <b>Search All Text Field</b>
                                    <asp:TextBox ID="txtSearchIncoming" runat="server" Style="width: 200px;" CssClass="defaultTextBox"/>
                                    <telerik:RadButton ID="btnSearchIncoming" runat="server" OnClick="btnSearchIncoming_Click" Width="25px" style="text-align: center">
                                        <Icon PrimaryIconUrl="~/Images/search.png" PrimaryIconLeft="3" PrimaryIconTop="2" PrimaryIconWidth="16" PrimaryIconHeight="16"/>
                                    </telerik:RadButton>
                                </ItemTemplate>
                            </telerik:RadToolBarButton>
                        </Items>
                    </telerik:RadToolBar>

                    <telerik:RadGrid AllowCustomPaging="False" AllowPaging="True" AllowSorting="True" 
                            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                            Height="100%" ID="grdIncomingTrans"  AllowFilteringByColumn="False" AllowMultiRowSelection="False"
                            OnNeedDataSource="grdIncomingTrans_OnNeedDataSource" 
                            PageSize="100" runat="server" Style="outline: none" Width="100%">
                            <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
                            <GroupingSettings CaseSensitive="False"></GroupingSettings>
                            <MasterTableView AllowMultiColumnSorting="false"
                                ClientDataKeyNames="ID" DataKeyNames="ID" CommandItemDisplay="Top" 
                                EditMode="InPlace" Font-Size="8pt">
                                <GroupByExpressions>
                                    <telerik:GridGroupByExpression>
                                        <SelectFields>
                                            <telerik:GridGroupByField FieldAlias="From" FieldName="OriginatingOrganizationName" FormatString="{0:D}"
                                                HeaderValueSeparator=": "></telerik:GridGroupByField>
                                        </SelectFields>
                                        <GroupByFields>
                                            <telerik:GridGroupByField FieldName="OriginatingOrganizationName" SortOrder="Ascending" ></telerik:GridGroupByField>
                                        </GroupByFields>
                                    </telerik:GridGroupByExpression>
                                </GroupByExpressions>  
                                <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="false"/>
                                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    
                                <Columns>
                                        
                                    <telerik:GridTemplateColumn HeaderText="No." Groupable="False" AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSoTT" runat="server" Text='<%# grdIncomingTrans.CurrentPageIndex * grdIncomingTrans.PageSize + grdIncomingTrans.Items.Count+1 %>'>
                                            </asp:Label>
                                      
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn  UniqueName="Status" AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="30" />
                                        <ItemStyle HorizontalAlign="Center"  />
                                        <ItemTemplate>
                                            <asp:Image ID="imgStatus" runat="server" ImageUrl='<%# 
                                                !string.IsNullOrEmpty(Eval("Status").ToString())
                                                    ? "~/Images/warning.png" 
                                                    : "~/Images/complete1.png" %>' 
                                                    Style="cursor: pointer;" />

                                            <telerik:RadToolTip Skin="Simple" runat="server" ID="dirNameToolTip" RelativeTo="Element" AutoCloseDelay="20000" ShowDelay="0" Position="BottomRight" Width="300px" Height="70px" HideEvent="LeaveTargetAndToolTip" TargetControlID="imgStatus" IsClientID="False" Animation="Fade"
                                    Text='<%# "Error Message:<br/>" + Eval("ErrorMessage").ToString().Replace(Environment.NewLine, "<br/>") %>' Visible='<%# !string.IsNullOrEmpty(Eval("Status").ToString())%>'/>
                                         
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                            
                                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="EditColumn" Display="False">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ShowEditForm("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="EditLink" runat="server" ImageUrl="~/Images/edit.png" Style="cursor: pointer;" ToolTip="Edit properties" />
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="AttachDocFile">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ShowContractorAttachDocFile("<%# DataBinder.Eval(Container.DataItem, "ContractorTransId") %>","<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="AttachDoc" runat="server" ImageUrl="~/Images/attach.png" Style="cursor: pointer;" ToolTip="Attach Document" />
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="DocumentList" Display="False">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ShowDocumentList("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="DocList" runat="server" ImageUrl="~/Images/documents.png" Style="cursor: pointer;" ToolTip="Already Attach Document List"/>
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    

                                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="ETRM">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ExportContractorETRM("<%# DataBinder.Eval(Container.DataItem, "ContractorTransId") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="imgExcel" runat="server" ImageUrl="~/Images/excelfile1.png" Style="cursor: pointer;" ToolTip="Export eTRM" />
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="ETRMAtach">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ShowContractorETRMAttachFile("<%# DataBinder.Eval(Container.DataItem, "ContractorTransId") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="imgExcelAttach" runat="server" ImageUrl="~/Images/excelfileattach.png" Style="cursor: pointer;" ToolTip="eTRM Attach File" />
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="ImportTrans">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ImportDocument("<%# DataBinder.Eval(Container.DataItem, "ContractorTransId") %>","<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="imgImportDocument" runat="server" ImageUrl="~/Images/importDoc.png" Style="cursor: pointer;" ToolTip="Import Transmittal" Visible='<%# !Convert.ToBoolean(Eval("IsImport"))%>'/>
                                            <a/>
                                                
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/importDocOk.png" ToolTip="Transmittal Imported" Visible='<%# Convert.ToBoolean(Eval("IsImport"))%>'/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    
                                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="AttachWorkflow">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:AttachWorkflow("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="imgAttachWorkflow" runat="server" ImageUrl="~/Images/attachwf.png" Style="cursor: pointer;" ToolTip="Attach All Document Of Transmittal To Workflow" Visible='<%# Convert.ToBoolean(Eval("IsImport")) && !Convert.ToBoolean(Eval("IsAttachWorkflow"))%>'/>
                                                <a/>
                                                
                                                <asp:Image ID="imgAttachWorkflowOk" runat="server" ImageUrl="~/Images/attachwfOk.png" ToolTip="Transmittal Attached To Workflow" Visible='<%# Convert.ToBoolean(Eval("IsAttachWorkflow"))%>'/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Transmittal Number" UniqueName="TransmittalNo" AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center"  />
                                        <ItemTemplate>
                                            <%# Eval("TransmittalNo") %>
                                            <asp:Image ID="newicon" runat="server" ImageUrl="~/Images/new.png" Visible='<%# !Convert.ToBoolean(Eval("IsOpen"))%>'/> 
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="Description" UniqueName="Description" AllowFiltering="false" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="250" />
                                        <ItemStyle HorizontalAlign="Left"  />
                                        <ItemTemplate>
                                            <%# Eval("Description") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="Transmittal Date" UniqueName="IssuedDate"
                                        AllowFiltering="false" >
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemTemplate>
                                            <%# Eval("IssuedDate","{0:dd/MM/yyyy}") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="Contractor Sent Date" UniqueName="ReceivedDate"
                                        AllowFiltering="false" >
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemTemplate>
                                            <%# Eval("ReceivedDate","{0:dd/MM/yyyy}") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="From" UniqueName="OriginatingOrganizationName"
                                        DataField="OriginatingOrganizationName" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="250" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# Eval("OriginatingOrganizationName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="To" UniqueName="ReceivingOrganizationName"
                                        DataField="ReceivingOrganizationName" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="250" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# Eval("ReceivingOrganizationName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Confidentiality" UniqueName="ConfidentialityName"
                                        DataField="ConfidentialityName" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# Eval("ConfidentialityName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                                 <ClientEvents OnRowClick="RowClick"></ClientEvents>
                        <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadPageView>
                            
                <telerik:RadPageView runat="server" Height="100%" ID="OutgoingTransView" Width="100%">
                    <telerik:RadToolBar ID="radMenuOutgoing" runat="server" Width="100%" OnClientButtonClicking="radMenuOutgoing_OnClientButtonClicking">
                        <Items>
                            <telerik:RadToolBarDropDown runat="server" Text="New" ImageUrl="~/Images/addNew.png">
                                <Buttons>
                                    <telerik:RadToolBarButton runat="server" Text="Outgoing Transmittal" Value="1" ImageUrl="~/Images/outgoingTrans.png" />
                                </Buttons>
                            </telerik:RadToolBarDropDown>
                                    
                            <telerik:RadToolBarButton runat="server" IsSeparator="true"/>

                            <telerik:RadToolBarButton runat="server" Value="ShowAll">
                                <ItemTemplate>
                                    &nbsp;&nbsp;<img src="../../Images/filter.png"/>&nbsp;&nbsp;<b style="color: red">Filter: </b> &nbsp;&nbsp;
                            
                                    <img src="../../Images/project.png"/>Selected project:&nbsp;&nbsp;
                                    <telerik:RadComboBox ID="ddlProjectOutgoing" runat="server" 
                                        Skin="Windows7" Width="250" AutoPostBack="True" 
                                        OnItemDataBound="ddlProjectOutgoing_ItemDataBound"
                                        OnSelectedIndexChanged="ddlProjectOutgoing_SelectedIndexChanged"/>&nbsp;&nbsp;|&nbsp;&nbsp;
                                    <b>Show</b>  
                                    <asp:DropDownList ID="ddlStatusOutgoing" runat="server" CssClass="rcbReadOnly" Width="150px" style="max-width: 150px"
                                        OnSelectedIndexChanged="ddlStatusOutgoing_OnSelectedIndexChanged" AutoPostBack="True">
                                        <Items>
                                            <asp:ListItem Value="All" Text="All Transmittal"></asp:ListItem>
                                            <asp:ListItem Value="Invalid" Text="Invalid Transmittal" style="background-color: red"></asp:ListItem>
                                            <asp:ListItem Value="Waiting" Text="Transmittal Waiting For Send" style="background-color: coral"></asp:ListItem>
                                            <asp:ListItem Value="Sent" Text="Transmittal Sent" style="background-color: greenyellow"></asp:ListItem>
                                            
                                        </Items>
                                    </asp:DropDownList>

                                    &nbsp;&nbsp; | &nbsp;&nbsp;
                                    <b>Search All Text Field</b>
                                    <asp:TextBox ID="txtSearchOutgoing" runat="server" Style="width: 200px;" CssClass="defaultTextBox"/>
                                    <telerik:RadButton ID="btnSearchOutgoing" runat="server" OnClick="btnSearchOutgoing_Click" Width="25px" style="text-align: center">
                                        <Icon PrimaryIconUrl="~/Images/search.png" PrimaryIconLeft="3" PrimaryIconTop="2" PrimaryIconWidth="16" PrimaryIconHeight="16"/>
                                    </telerik:RadButton>
                                </ItemTemplate>
                            </telerik:RadToolBarButton>
                        </Items>
                    </telerik:RadToolBar>

                    <telerik:RadGrid AllowCustomPaging="False" AllowPaging="true" AllowSorting="True" 
                            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                            Height="100%" ID="grdOutgoingTrans"  AllowFilteringByColumn="False" AllowMultiRowSelection="False"
                            OnNeedDataSource="grdOutgoingTrans_OnNeedDataSource" 
                            OnDeleteCommand="grdOutgoingTrans_OnDeleteCommand"

                            PageSize="100" runat="server" Style="outline: none" Width="100%">
                            <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
                            <GroupingSettings CaseSensitive="False"></GroupingSettings>
                            <MasterTableView AllowMultiColumnSorting="false"
                                ClientDataKeyNames="ID" DataKeyNames="ID" CommandItemDisplay="Top" 
                                EditMode="InPlace" Font-Size="8pt">
                                <GroupByExpressions>
                                    <telerik:GridGroupByExpression>
                                        <SelectFields>
                                            <telerik:GridGroupByField FieldAlias="To" FieldName="ReceivingOrganizationName" FormatString="{0:D}"
                                                HeaderValueSeparator=": "></telerik:GridGroupByField>
                                        </SelectFields>
                                        <GroupByFields>
                                            <telerik:GridGroupByField FieldName="ReceivingOrganizationName" SortOrder="Ascending" ></telerik:GridGroupByField>
                                        </GroupByFields>
                                    </telerik:GridGroupByExpression>
                                </GroupByExpressions> 
                                <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="false"/>
                                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    
                                <Columns>
                                        
                                    <telerik:GridTemplateColumn HeaderText="No." Groupable="False" AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSoTT" runat="server" Text='<%# grdOutgoingTrans.CurrentPageIndex * grdOutgoingTrans.PageSize + grdOutgoingTrans.Items.Count+1 %>'>
                                            </asp:Label>
                                      
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn  UniqueName="Status" AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="30" />
                                        <ItemStyle HorizontalAlign="Center"  />
                                        <ItemTemplate>
                                            <asp:Image ID="imgStatus" runat="server" ImageUrl='<%# 
                                                Eval("Status").ToString() == "Missing Attach Document" 
                                                    ? "~/Images/warning.png" 
                                                    : "~/Images/complete1.png" %>' 
                                                    Style="cursor: pointer;" />

                                            <telerik:RadToolTip Skin="Simple" runat="server" ID="dirNameToolTip" RelativeTo="Element" AutoCloseDelay="20000" ShowDelay="0" Position="BottomRight" Width="300px" Height="70px" HideEvent="LeaveTargetAndToolTip" TargetControlID="imgStatus" IsClientID="False" Animation="Fade"
                                    Text='<%# "Error Message:<br/>" + Eval("ErrorMessage").ToString().Replace(Environment.NewLine, "<br/>") %>' Visible='<%# Eval("Status").ToString() == "Missing Attach Document"%>'/>
                                         
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                            
                                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="EditColumn">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ShowEditForm("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="EditLink" runat="server" ImageUrl="~/Images/edit.png" Style="cursor: pointer;" ToolTip="Edit properties" />
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <%--<telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" ConfirmText="Do you want to delete item?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png" >
                                        <HeaderStyle Width="30" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridButtonColumn>--%>
                                     <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="DeleteColumn">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <div runat="server" visible='<%# !Convert.ToBoolean(Eval("IsSend"))%>'>
                                            <a href='javascript:DeleteTransmittal("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="delete" runat="server" ImageUrl="~/Images/delete.png" Style="cursor: pointer;" ToolTip="Delete Transmittal" />
                                                <a/></div>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="AttachDocFile">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ShowAttachDocFile("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="AttachDoc" runat="server" ImageUrl="~/Images/attach.png" Style="cursor: pointer;" ToolTip="Attach Document" />
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="DocumentList">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ShowDocumentList("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="DocList" runat="server" ImageUrl="~/Images/documents.png" Style="cursor: pointer;" ToolTip="Already Attach Document List" Visible='<%# Eval("IsValid")%>'/>
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                        <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="AttachDocFileTrans">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ShowIncomingAttachDocFile("<%# DataBinder.Eval(Container.DataItem, "ID") %>","<%# !Convert.ToBoolean(Eval("IsSend"))%>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="AttachDoctrans" runat="server" ImageUrl="~/Images/attach1.png" Style="cursor: pointer;" ToolTip="Attachment Files To Transmittal" Visible='<%# Eval("IsValid")%>' />
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="ETRM">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ExportETRM("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="imgExcel" runat="server" ImageUrl="~/Images/excelfile1.png" Style="cursor: pointer;" ToolTip="Export eTRM" Visible='<%# Eval("IsValid")%>'/>
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="ETRMAtach">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ShowETRMAttachFile("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="imgExcelAttach" runat="server" ImageUrl="~/Images/excelfileattach.png" Style="cursor: pointer;" ToolTip="eTRM Attach File" Visible='<%# Eval("IsValid")%>'/>
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="SendTrans">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:SendTrans("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="imgSendTrans" runat="server" ImageUrl="~/Images/sendTrans.png" Style="cursor: pointer;" ToolTip="Waiting to send transmittal" Visible='<%# Convert.ToBoolean(Eval("IsValid")) && !Convert.ToBoolean(Eval("IsSend"))%>'/>
                                            <a/>
                                                
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/sendTransOk1.png" ToolTip="Transmittal sent" Visible='<%# Convert.ToBoolean(Eval("IsSend"))%>'/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                       <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="UndoSendTrans">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:UndoSendTrans("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="imgUndoSendTrans" runat="server" ImageUrl="~/Images/undo_16.png" Style="cursor: pointer;" ToolTip="Undo transmittal sended" Visible='<%# Convert.ToBoolean(Eval("IsSend")) && (DateTime.Now - Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "ReceivedDate"))).TotalHours < 24%>'/>
                                            <a/>                       
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Transmittal Number" UniqueName="TransmittalNo" AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center"  />
                                        <ItemTemplate>
                                            <%# Eval("TransmittalNo") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="Ref Incoming Trans" UniqueName="RefTransNo" AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center"  />
                                        <ItemTemplate>
                                            <%# Eval("RefTransNo") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="Description" UniqueName="Description" AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                                        <ItemStyle HorizontalAlign="Left"  />
                                        <ItemTemplate>
                                            <%# Eval("Description") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="Transmittal Date" UniqueName="IssuedDate"
                                        AllowFiltering="false" >
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemTemplate>
                                            <%# Eval("IssuedDate","{0:dd/MM/yyyy}") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="Due Date" UniqueName="DueDate"
                                        AllowFiltering="false" >
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemTemplate>
                                            <%# Eval("DueDate","{0:dd/MM/yyyy}") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="From" UniqueName="OriginatingOrganizationName"
                                        DataField="OriginatingOrganizationName" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="250" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# Eval("OriginatingOrganizationName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="To" UniqueName="ReceivingOrganizationName"
                                        DataField="ReceivingOrganizationName" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="250" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# Eval("ReceivingOrganizationName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Receiver Received Date" UniqueName="ReceivedDate"
                                        AllowFiltering="false" >
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemTemplate>
                                            <%# Eval("ReceivedDate","{0:dd/MM/yyyy}") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Confidentiality" UniqueName="ConfidentialityName"
                                        DataField="ConfidentialityName" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# Eval("ConfidentialityName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                                
                        <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
                            
        </telerik:RadPane>
    </telerik:RadSplitter>       
        
    <telerik:RadContextMenu ID="radMenu" runat="server"
        EnableRoundedCorners="true" EnableShadows="true" OnClientItemClicking="gridMenuClicking" OnClientShowing="gridContextMenuShowing">
        <Items>
            <telerik:RadMenuItem Text="Complete & Move Next" ImageUrl="~/Images/complete.png" Value="Complete"/>
            <telerik:RadMenuItem Text="Complete" ImageUrl="~/Images/complete.png" Value="FinalComplete"/>
            <telerik:RadMenuItem Text="Reject" ImageUrl="~/Images/reject.png" Value="Reject"/>
            <telerik:RadMenuItem IsSeparator="True"/>
            <telerik:RadMenuItem Text="Workflow Process History" ImageUrl="~/Images/history.png" Value="WFHistory"/>
            <telerik:RadMenuItem Text="Object Detail" ImageUrl="~/Images/detail.png" Value="MarkupList"/>
            <telerik:RadMenuItem IsSeparator="True"/>
            <telerik:RadMenuItem Text="E-mail" ImageUrl="~/Images/email.png" Value="Email"/>
            <telerik:RadMenuItem IsSeparator="True"/>
            <telerik:RadMenuItem Text="Re-assign To Working PIC" ImageUrl="~/Images/reassign.png" Value="Reassign"/>

            
        </Items>
    </telerik:RadContextMenu>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Windows7"/>
<telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
    <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdIncomingTrans" LoadingPanelID="RadAjaxLoadingPanel1"/>
                <telerik:AjaxUpdatedControl ControlID="grdOutgoingTrans" LoadingPanelID="RadAjaxLoadingPanel1"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
                
        <telerik:AjaxSetting AjaxControlID="ckbShowAll">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdIncomingTrans" LoadingPanelID="RadAjaxLoadingPanel1"/>
            </UpdatedControls>
        </telerik:AjaxSetting>

        <telerik:AjaxSetting AjaxControlID="radMenu">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdIncomingTrans"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
                
        <telerik:AjaxSetting AjaxControlID="grdIncomingTrans">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdIncomingTrans" LoadingPanelID="RadAjaxLoadingPanel1"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="btnSearchIncoming">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdIncomingTrans" LoadingPanelID="RadAjaxLoadingPanel1"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="btnSearchOutgoing">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdOutgoingTrans" LoadingPanelID="RadAjaxLoadingPanel1"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
                
        <telerik:AjaxSetting AjaxControlID="grdOutgoingTrans">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdOutgoingTrans" LoadingPanelID="RadAjaxLoadingPanel1"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
                
        <telerik:AjaxSetting AjaxControlID="ddlProjectOutgoing">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdOutgoingTrans" LoadingPanelID="RadAjaxLoadingPanel1"/>
                <telerik:AjaxUpdatedControl ControlID="lblProjectOutgoingId"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="ddlProjectIncoming">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdIncomingTrans" LoadingPanelID="RadAjaxLoadingPanel1"/>
                <telerik:AjaxUpdatedControl ControlID="lblProjectIncomingId"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="ddlStatusOutgoing">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdOutgoingTrans" LoadingPanelID="RadAjaxLoadingPanel1"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="ddlStatusIncoming">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdIncomingTrans" LoadingPanelID="RadAjaxLoadingPanel1"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
    <%--OnClientClose="refreshGrid"--%>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Outgoing Transmittal Information"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/outgoingTrans.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachDocFile" runat="server" Title="Transmittal Attach Document"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/attach.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" OnClientClose="refreshAttachDocGrid">
            </telerik:RadWindow>
            
             <telerik:RadWindow ID="ContractorAttachDocFile" runat="server" Title="Transmittal Attach Document"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/attach.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" OnClientClose="refreshContractorAttachDocGrid">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachDocList" runat="server" Title="Already Attach Document List"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/documents.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" >
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ETRMAttachFile" runat="server" Title="eTRM Attach File"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/excelfileattach.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="CommentWnd" runat="server" 
                VisibleStatusbar="false" Height="700" Width="900" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ResponseWnd" runat="server" 
                VisibleStatusbar="false" Height="650" Width="1000" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="UploadMulti" runat="server" Title="Create multiple documents"
                VisibleStatusbar="false" Height="520" MinHeight="520" MaxHeight="520" Width="640" MinWidth="640" MaxWidth="640"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RevisionDialog" runat="server" Title="Revision History"
                VisibleStatusbar="false" Height="550" Width="1250" MinHeight="550"  
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="TransmittalList" runat="server" Title="Transmittal List"
                VisibleStatusbar="false" Height="400" Width="1250" MinHeight="400" MinWidth="1250" MaxHeight="400" MaxWidth="1250" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="SendMail" runat="server" Title="Send mail"
                VisibleStatusbar="false" Height="560" Width="992" MinHeight="560" MinWidth="992" MaxHeight="560" MaxWidth="992" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachDoc" runat="server" Title="Attach document files"
                VisibleStatusbar="false"  
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>

            <telerik:RadWindow ID="MarkupFile" runat="server" Title="Comment List"
                VisibleStatusbar="false" Height="500" Width="800" MinHeight="500" MinWidth="800" MaxHeight="500" MaxWidth="800" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachMulti" runat="server" Title="Attach multi document files"
                VisibleStatusbar="false" Height="500" Width="700" MinHeight="500" MinWidth="700" MaxHeight="500" MaxWidth="700" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachComment" runat="server" Title="Attach Comment/Response"
                VisibleStatusbar="false" Height="500" Width="900" MinHeight="500" MinWidth="900" MaxHeight="500" MaxWidth="900" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ImportData" runat="server" Title="Import master list"
                VisibleStatusbar="false" Height="400" Width="520" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ImportEMDRReport" runat="server" Title="Import EMDR Report"
                VisibleStatusbar="false" Height="400" Width="520" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ShareDocument" runat="server" Title="Share documents"
                VisibleStatusbar="false" Height="600" Width="520" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachWorkflow" runat="server" Title="Attach Document to Workflow"
                VisibleStatusbar="False" Height="450" Width="610" IconUrl="~/Images/attachwf.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ObjDetailDialog" runat="server" Title="Object Details"
                VisibleStatusbar="False" Height="700" Width="850" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" OnClientClose="refreshGrid">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="DistributeOnshore" runat="server" Title="Distribute Onshore Comment"
                VisibleStatusbar="False" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
                
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="CompleteMoveNext" runat="server" Title="Complete Task And Move Next Step"
                VisibleStatusbar="False" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ReassignWorkingPIC" runat="server" Title="Re-assign To Working PIC"
                VisibleStatusbar="False" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RejectForm" runat="server" Title="Reject To Previous Step"
                VisibleStatusbar="False" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="WFProcessHistory" runat="server" Title="Workflow Process History"
                VisibleStatusbar="False" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="CompleteFinal" runat="server" Title="Complete Process Workflow"
                VisibleStatusbar="False" Height="450" Width="610" OnClientClose="refreshGrid"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="UpdateMC" runat="server" Title="Update Morning Call"
                VisibleStatusbar="False" Height="700" Width="850" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RevisionHistoryMC" runat="server" Title="Morning Call Revision History"
                VisibleStatusbar="false" Height="550" Width="1250" MinHeight="550"  
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="UpdateWCR" runat="server" Title="Update WCR"
                VisibleStatusbar="False" Height="700" Width="850" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RevisionHistoryWCR" runat="server" Title="WCR Revision History"
                VisibleStatusbar="false" Height="550" Width="1250" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="UpdatePL" runat="server" Title="Update Punch List"
                VisibleStatusbar="False" Height="700" Width="850" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RevisionHistoryPL" runat="server" Title="Punch List Revision History"
                VisibleStatusbar="false" Height="550" Width="1250" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="UpdateP" runat="server" Title="Update Procedure"
                VisibleStatusbar="False" Height="700" Width="850" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RevisionHistoryP" runat="server" Title="Procedure Revision History"
                VisibleStatusbar="false" Height="550" Width="1250" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="UpdateSL" runat="server" Title="Update Sail List"
                VisibleStatusbar="False" Height="700" Width="850" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RevisionHistorySL" runat="server" Title="Sail List Revision History"
                VisibleStatusbar="false" Height="550" Width="1250" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="UpdateGW" runat="server" Title="Update General Working"
                VisibleStatusbar="False" Height="700" Width="850" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RevisionHistoryGW" runat="server" Title="General Working Revision History"
                VisibleStatusbar="false" Height="550" Width="1250"  
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <asp:HiddenField runat="server" ID="FolderContextMenuAction"/>
    <asp:HiddenField runat="server" ID="lblDisciplineId"/>
    <asp:HiddenField runat="server" ID="lblFolderId"/>
    <asp:HiddenField runat="server" ID="lblObjId"/>
    <asp:HiddenField runat="server" ID="lblOBJInTrans"/>
    <asp:HiddenField runat="server" ID="lblCategoryId"/>
    <asp:HiddenField runat="server" ID="IsFullPermission"/>
    <asp:HiddenField runat="server" ID="CanReject"/>
    <asp:HiddenField runat="server" ID="CanConsolidate"/>
    <asp:HiddenField runat="server" ID="lblProjectOutgoingId"/>
    <asp:HiddenField runat="server" ID="lblProjectIncomingId"/>
    <asp:HiddenField runat="server" ID="ObjectType"/>
    <asp:HiddenField runat="server" ID="ObjectId"/>
    <asp:HiddenField runat="server" ID="IsOnshoreComment"/>
    <asp:HiddenField runat="server" ID="IsDistributeOnshore"/>
    <asp:HiddenField runat="server" ID="IsFinal"/>
    <asp:HiddenField runat="server" ID="ActionType"/>
    

    <input type="hidden" id="radGridClickedRowIndex" name="radGridClickedRowIndex"/>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="../../Scripts/jquery-1.7.1.js"></script>
        <script type="text/javascript">
            
            function OnClientSelectedIndexChanging(sender, eventArgs) {
                var input = sender.get_inputDomElement();
                input.style.background = "url(" + eventArgs.get_item().get_imageUrl() + ") no-repeat";
            }
            function OnClientLoad(sender) {
                var input = sender.get_inputDomElement();
                var selectedItem = sender.get_selectedItem();
                input.style.background = "url(" + selectedItem.get_imageUrl() + ") no-repeat";
            }

            var radDocuments;

            function ShowFilter(obj) {
                if (obj.checked) {
                    $find('<%=grdIncomingTrans.ClientID %>').get_masterTableView().showFilterItem();
                } else {
                    $find('<%=grdIncomingTrans.ClientID %>').get_masterTableView().hideFilterItem();
                }
            }

            function refreshOutgoingGrid() {
                var masterTable = $find("<%=grdOutgoingTrans.ClientID%>").get_masterTableView();
                masterTable.rebind();
            }


            function ExportGrid() {
                var masterTable = $find("<%=grdIncomingTrans.ClientID %>").get_masterTableView();
                masterTable.exportToExcel('PIN_list.xls');
                return false;
            }

            function GetGridObject(sender, eventArgs) {
                radDocuments = sender;
            }

            function onRequestStart(sender, args)
            {
                //alert(args.get_eventTarget()); //|| args.get_eventTarget().indexOf("ajaxCustomer") >= 0
                if (args.get_eventTarget().indexOf("ExportTo") >= 0 || args.get_eventTarget().indexOf("btnDownloadPackage") >= 0 || args.get_eventTarget().indexOf("ajaxCustomer") >= 0)
                 {
                     args.set_enableAjax(false);
                 }
             }

            function onColumnHidden(sender) {
                
                var masterTableView = sender.get_masterTableView().get_element();
                masterTableView.style.tableLayout = "auto";
                //window.setTimeout(function () { masterTableView.style.tableLayout = "auto"; }, 0);
            }

            // Undocked and Docked event slide bar Tree folder
            function OnClientUndocked(sender, args) {
                var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;

                radDocuments.get_masterTableView().showColumn(8);
                radDocuments.get_masterTableView().showColumn(9);
                radDocuments.get_masterTableView().showColumn(10);
                radDocuments.get_masterTableView().showColumn(11);
                
                radDocuments.get_masterTableView().showColumn(12);
                radDocuments.get_masterTableView().showColumn(13);
                radDocuments.get_masterTableView().showColumn(14);
                radDocuments.get_masterTableView().showColumn(15);

                if (selectedFolder != "") {
                    ajaxManager.ajaxRequest("ListAllDocuments");
                }
            }
            
            function OnClientDocked(sender, args) {
                var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;

                radDocuments.get_masterTableView().hideColumn(8);
                radDocuments.get_masterTableView().hideColumn(9);
                radDocuments.get_masterTableView().hideColumn(10);
                radDocuments.get_masterTableView().hideColumn(11);
                
                radDocuments.get_masterTableView().hideColumn(12);
                radDocuments.get_masterTableView().hideColumn(13);
                radDocuments.get_masterTableView().hideColumn(14);
                radDocuments.get_masterTableView().hideColumn(15);
                
                if (selectedFolder != "") {
                    ajaxManager.ajaxRequest("TreeView");
                }
            }

            function RowClick(sender, eventArgs) {
                var Id = eventArgs.getDataKeyValue("ID");
                document.getElementById("<%= lblOBJInTrans.ClientID %>").value = Id;
            }

            function gridMenuClicking(sender, args) {
                var itemValue = args.get_item().get_value();
                var docId = document.getElementById("<%= lblObjId.ClientID %>").value;
                var isFullPermission = document.getElementById("<%= IsFullPermission.ClientID %>").value;

                var packageId = document.getElementById("<%= lblDisciplineId.ClientID %>").value;
                
                if (itemValue.indexOf("Comment") > -1)
                {
                    var contractorId = itemValue.split("_")[1];
                    var owd = $find("<%=CommentWnd.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/Document/CommentForm.aspx?docId=" + docId + "&contId=" + contractorId, "CommentWnd");
                    
                } else if (itemValue.indexOf("Response") > -1) {
                    var contractorId = itemValue.split("_")[1];
                    var owd = $find("<%=ResponseWnd.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/Document/ResponseForm.aspx?docId=" + docId + "&contId=" + contractorId, "ResponseWnd");

                }
                else
                {
                    switch (itemValue) {
                        case "RevisionHistory":
                            var categoryId = document.getElementById("<%= lblCategoryId.ClientID %>").value;
                            var owd = $find("<%=RevisionDialog.ClientID %>");
                            owd.Show();
                            owd.setUrl("Controls/Document/DocumentPackageRevisionHistory.aspx?docId=" + docId + "&categoryId=" + categoryId, "RevisionDialog");
                            break;
                        case "Distribute":
                            var objAssignUserId = document.getElementById("<%= lblObjId.ClientID %>").value;
                            var owd = $find("<%=DistributeOnshore.ClientID %>");
                            owd.Show();
                            owd.setUrl("Controls/Workflow/DistributeOnshoreComment.aspx?objAssignUserId=" + objAssignUserId , "DistributeOnshore");
                            break;
                        case "WFHistory":
                            var objType = document.getElementById("<%= ObjectType.ClientID %>").value == "Material Requisition" ? "MR" : "WR";
                            var objId = document.getElementById("<%= ObjectId.ClientID %>").value;
                            var owd = $find("<%=WFProcessHistory.ClientID %>");
                            owd.Show();
                            owd.maximize();
                            owd.setUrl("Controls/Workflow/WorkflowProcessHistory.aspx?objId=" + objId + "&objType=" + objType, "WFProcessHistory");
                            break;

                        case "Complete":
                            var objType = document.getElementById("<%= ObjectType.ClientID %>").value;
                            var objId = document.getElementById("<%= ObjectId.ClientID %>").value;

                            var owd = $find("<%=CompleteMoveNext.ClientID %>");
                            owd.Show();
                            owd.setUrl("Controls/Workflow/CompleteMoveNext.aspx?objType=" + objType + "&objId=" + objId + "&currentAssignId=" + docId, "CompleteMoveNext");

                            //ajaxManager.ajaxRequest("CompleteAndMove_" + objType + "_" + objId);
                            break;
                        case "Reassign":
                            var objType = document.getElementById("<%= ObjectType.ClientID %>").value;
                            var objId = document.getElementById("<%= ObjectId.ClientID %>").value;

                            var owd = $find("<%=ReassignWorkingPIC.ClientID %>");
                            owd.Show();
                            owd.setUrl("Controls/Workflow/ReassignWorkingPIC.aspx?objType=" + objType + "&objId=" + objId + "&currentAssignId=" + docId, "ReassignWorkingPIC");

                            //ajaxManager.ajaxRequest("CompleteAndMove_" + objType + "_" + objId);
                            break;
                        case "Email":
                            var objType = document.getElementById("<%= ObjectType.ClientID %>").value;
                            var objId = document.getElementById("<%= ObjectId.ClientID %>").value;

                            var owd = $find("<%=SendMail.ClientID %>");
                            owd.Show();
                            owd.setUrl("Controls/WMS/SendMail.aspx?objType=" + objType + "&objId=" + objId + "&currentAssignId=" + docId, "SendMail");

                            //ajaxManager.ajaxRequest("CompleteAndMove_" + objType + "_" + objId);
                            break;
                        case "Reject":

                            if (confirm("Do you want to Reject this Object?") == false) return;

                            var objType = document.getElementById("<%= ObjectType.ClientID %>").value;
                            var objId = document.getElementById("<%= ObjectId.ClientID %>").value;

                            var owd = $find("<%=RejectForm.ClientID %>");
                            owd.Show();
                            owd.setUrl("Controls/Workflow/Reject.aspx?objType=" + objType + "&objId=" + objId + "&currentAssignId=" + docId, "RejectForm");
                            //ajaxManager.ajaxRequest("Reject_" + objType + "_" + objId);
                                break;
                        case "AttachDocument":
                            var owd = $find("<%=AttachDoc.ClientID %>");
                            owd.Show();
                            owd.setUrl("Controls/Document/AttachDocument.aspx?docId=" + docId + "&isFullPermission=" + isFullPermission, "AttachDoc");
                            break;
                        case "AttachWF":
                            var owd = $find("<%=AttachWorkflow.ClientID %>");
                            owd.Show();
                            owd.setUrl("Controls/Workflow/AttachWorkflow.aspx?docId=" + docId, "AttachWorkflow");
                            break;
                        

                        case "FinalComplete":
                            <%--if (confirm("Do you want to Complete this Job?") == false) return;
                            var objType = document.getElementById("<%= ObjectType.ClientID %>").value;
                            var objId = document.getElementById("<%= ObjectId.ClientID %>").value;
                            ajaxManager.ajaxRequest("FinalComplete_" + objType + "_" + objId);--%>

                            var objType = document.getElementById("<%= ObjectType.ClientID %>").value;
                            var objId = document.getElementById("<%= ObjectId.ClientID %>").value;
                            var owd = $find("<%=CompleteFinal.ClientID %>");
                            owd.Show();
                            owd.setUrl("Controls/Workflow/FinalComplete.aspx?objType=" + objType + "&objId=" + objId + "&currentAssignId=" + docId + "&flag=true", "CompleteFinal");
                            break;
                        
                        case "AttachComment":
                            var owd = $find("<%=AttachComment.ClientID %>");
                            owd.Show();
                            owd.setUrl("Controls/Document/AttachComment.aspx?docId=" + docId + "&isFullPermission=" + isFullPermission, "AttachComment");
                            break;
                        case "EditProperties":
                            var projectId = document.getElementById("<%= lblProjectOutgoingId.ClientID %>").value;
                            var owd = $find("<%=CustomerDialog.ClientID %>");
                            owd.Show();
                            owd.setUrl("Controls/Document/DocumentPackageInfoEditForm.aspx?docId=" + docId + "&projId=" + projectId, "CustomerDialog");
                            break;
                        case "Transmittals":
                            var owd = $find("<%=TransmittalList.ClientID %>");
                            owd.Show();
                            owd.setUrl("Controls/Document/TransmittalListByDoc.aspx?docId=" + docId, "TransmittalList");
                            break;
                            case "Consolidate":
                            if (confirm("Do you want to consolidate all Markup file of document?") == false) return;
                            ajaxManager.ajaxRequest("Consolidate_" + docId);
                            break;
                        case "DeleteRev":
                            if (confirm("Do you want to reduction version of document?") == false) return;
                            ajaxManager.ajaxRequest("DeleteRev_" + docId);
                            break;
                        case "CommentResponse":
                            var owd = $find("<%=TransmittalList.ClientID %>");
                            owd.Show();
                            owd.setUrl("Controls/Document/TransmittalListByDoc.aspx?docId=" + docId, "TransmittalList");
                            break;
                    }
                }
            }
            
            function onClientContextMenuItemClicking(sender, args) {
                var menuItem = args.get_menuItem();
                var treeNode = args.get_node();
                menuItem.get_menu().hide();

                switch (menuItem.get_value()) {
                case "Rename":
                    treeNode.startEdit();
                    break;
                case "Delete":
                    var result = confirm("Are you sure you want to delete the folder: " + treeNode.get_text());
                    args.set_cancel(!result);
                    break;
                }
            }
            
            function rtvExplore_OnNodeExpandedCollapsed(sender, eventArgs) {
                var allNodes = eventArgs._node.get_treeView().get_allNodes();

                var i;
                var selectedNodes = "";

                for (i = 0; i < allNodes.length; i++) {
                    if (allNodes[i].get_expanded())
                        selectedNodes += allNodes[i].get_value() + "*";
                }

                Set_Cookie("expandedNodesObjTree", selectedNodes, 30);
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

            function gridContextMenuShowing(menu, args) {
                var canReject = document.getElementById("<%= CanReject.ClientID %>").value;
                <%--var canConsolidate = document.getElementById("<%= CanConsolidate.ClientID %>").value;
                var IsDistributeOnshore = document.getElementById("<%= IsDistributeOnshore.ClientID %>").value;
                var IsOnshoreComment = document.getElementById("<%= IsOnshoreComment.ClientID %>").value;--%>
                var isFinal = document.getElementById("<%= IsFinal.ClientID %>").value;
                var actionType = document.getElementById("<%= ActionType.ClientID %>").value;
                //alert(isFinal);

                if (actionType === "1") {
                    menu.get_allItems()[9].hide();
                    menu.get_allItems()[8].hide();
                    menu.get_allItems()[0].show();
                    menu.get_allItems()[1].show();
                    menu.get_allItems()[2].show();

                    if (canReject === "True") {
                        menu.get_allItems()[2].show();
                    }
                    else {
                        menu.get_allItems()[2].hide();
                    }

                    if (isFinal === "True") {
                        menu.get_allItems()[0].hide();
                        menu.get_allItems()[1].show();
                    }
                    else {
                        menu.get_allItems()[1].hide();
                        menu.get_allItems()[0].show();
                    }
                } else {
                    menu.get_allItems()[9].show();
                    menu.get_allItems()[8].show();
                    menu.get_allItems()[0].hide();
                    menu.get_allItems()[1].hide();
                    menu.get_allItems()[2].hide();
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

            function ShowEditForm(objId) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("DQRETransmittalEditForm.aspx?objId=" + objId, "CustomerDialog");
            }
             function ShowIncomingAttachDocFile(objId,upload) {
                var owd = $find("<%=AttachDocFile.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.setUrl("DQRETransmittalAttachDocFileList.aspx?objId=" + objId +"&Upload="+upload, "AttachDocFile");
            }
            function ShowContractorAttachDocFile(objId,TransObj) {
                var projId = document.getElementById("<%= lblProjectIncomingId.ClientID %>").value;
                var owd = $find("<%=ContractorAttachDocFile.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.setUrl("ContractorTransmittalAttachDocFileList.aspx?objId=" + objId + "&projId=" + projId + "&DQRETransID=" + TransObj, "ContractorAttachDocFile");
            }

            function ShowAttachDocFile(objId) {
                var owd = $find("<%=AttachDocFile.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.setUrl("TransmittalAttachDocument.aspx?objId=" + objId, "AttachDocFile");
            }
            function DeleteTransmittal(id) {
                if (confirm("Do you want to delete item?") == false) return;
                ajaxManager.ajaxRequest("DeleteTrans_" + id);
            }
            function ShowDocumentList(objId) {
                var owd = $find("<%=AttachDocList.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.setUrl("TransmittalDocumentList.aspx?objId=" + objId, "AttachDocList");
            }

            function ExportETRM(objId) {
                ajaxManager.ajaxRequest("Export_" + objId);
            }

            function ExportContractorETRM(objId) {
                ajaxManager.ajaxRequest("ExportContractorETRM_" + objId);
            }

            function ShowETRMAttachFile(objId) {
                var owd = $find("<%=ETRMAttachFile.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("DQRETransETRMAttach.aspx?objId=" + objId, "ETRMAttachFile");
            }

            function ShowContractorETRMAttachFile(objId) {
                var owd = $find("<%=ETRMAttachFile.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("ContractorTransETRMAttach.aspx?objId=" + objId, "ETRMAttachFile");
            }

            function AttachWorkflow(docId) {
                var owd = $find("<%=AttachWorkflow.ClientID %>");
                 var projId = document.getElementById("<%= lblProjectIncomingId.ClientID %>").value;
                 owd.setSize(700, document.documentElement.offsetHeight);
                 owd.Show();
                 owd.setUrl("../../Controls/Workflow/AttachWorkflow.aspx?objId=" + docId + "&projId=" + projId + "&type=1" + "&attachFrom=AttachByTrans", "AttachWorkflow");
            }

            function SendTrans(objId) {
                if (confirm("Do you want to send this transmittal to contractor?") == false) return;
                ajaxManager.ajaxRequest("SendTrans_" + objId);
            }
            function UndoSendTrans(objId) {
                if (confirm("Do you want to undo sended this transmittal?") == false) return;
                ajaxManager.ajaxRequest("Undo_" + objId);
            }
            function ImportDocument(objId,TransID) {
                  var docId = document.getElementById("<%= lblOBJInTrans.ClientID %>").value;
                if (confirm("Do you want to import all Document of this transmittal?") == false) return;
                ajaxManager.ajaxRequest("ImportDocument_" + objId + "_" + TransID);
            }

            function ShowUploadForm(id) {
                var owd = $find("<%=AttachDoc.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Document/UploadDragDrop.aspx?docId=" + id, "AttachDoc");
            }
            
            function ShowRevisionForm(id) {
                var owd = $find("<%=RevisionDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Document/RevisionHistory.aspx?docId=" + id, "RevisionDialog");
            }

            function refreshGrid(arg) {
                if (!arg) {
                    var masterTable = $find("<%=grdIncomingTrans.ClientID%>").get_masterTableView();
                    masterTable.rebind();
                }
                else {
                    var masterTable = $find("<%=grdIncomingTrans.ClientID%>").get_masterTableView();
                    masterTable.rebind();
                }
            }

            function refreshOutgoingGrid() {
                var masterTable = $find("<%=grdOutgoingTrans.ClientID%>").get_masterTableView();
                masterTable.rebind();
            }

            function refreshAttachDocGrid() {
                var masterTable = $find("<%=grdOutgoingTrans.ClientID%>").get_masterTableView();
                masterTable.rebind();
            }

            function refreshContractorAttachDocGrid() {
                var masterTable = $find("<%=grdIncomingTrans.ClientID%>").get_masterTableView();
                masterTable.rebind();
            }
            
            function refreshTab(arg) {
                $('.EDMsRadPageView' + arg + ' iframe').attr('src', $('.EDMsRadPageView' + arg + ' iframe').attr('src'));
            }

            function RowDblClick(sender, eventArgs) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Customers/ViewCustomerDetails.aspx?docId=" + eventArgs.getDataKeyValue("Id"), "CustomerDialog");
                // window.radopen("Controls/Customers/ViewCustomerDetails.aspx?patientId=" + eventArgs.getDataKeyValue("Id"), "CustomerDialog");
            }
            
            function rtvDiscipline_ClientNodeClicking(sender, args) {
                var DisciplineValue = args.get_node().get_value();
                document.getElementById("<%= lblDisciplineId.ClientID %>").value = DisciplineValue;
            }
            
            function radPbCategories_OnClientItemClicking(sender, args)
            {
                var item = args.get_item();
                var categoryId = item.get_value();
                document.getElementById("<%= lblCategoryId.ClientID %>").value = categoryId;
                
            }

            function radMenuOutgoing_OnClientButtonClicking(sender, args) {
                var button = args.get_item();
                var strText = button.get_text();
                var strValue = button.get_value();

                var grid = $find("<%= grdIncomingTrans.ClientID %>");
                var customerId = null;
                var customerName = "";

                if (strText.toLowerCase() == "update") {
                    ajaxManager.ajaxRequest("UpdatePackageStatus");
                }

                //Download multi documents Export template data file Attach multi document file
                ////if (strText == "View explorer") {
                ////    window.open("file://WIN-P7KS57HL1HG/DocumentLibrary");
                ////}

                if (strText === "Outgoing Transmittal") {
                    
                    var objId = document.getElementById("<%= lblObjId.ClientID %>").value;
                    var projectId = document.getElementById("<%= lblProjectOutgoingId.ClientID %>").value;
                    var owd = $find("<%=CustomerDialog.ClientID %>");
                    owd.setSize(730, document.documentElement.offsetHeight);
                    owd.Show();
                    owd.setUrl("DQRETransmittalEditForm.aspx?objId=" + objId + "&projId=" + projectId, "CustomerDialog");
                }
            }
            
            function performSearch(searchTextBox) {
                if (searchTextBox.get_value()) {
                    searchButton.set_imageUrl("images/clear.gif");
                    searchButton.set_value("clear");
                }

                ajaxManager.ajaxRequest(searchTextBox.get_value());
            }
            function onTabSelecting(sender, args) {
                if (args.get_tab().get_pageViewID()) {
                    args.get_tab().set_postBack(false);
                }
            }
            

            function RowContextMenu(sender, eventArgs) {
                var menu = $find("<%=radMenu.ClientID %>");
                var evt = eventArgs.get_domEvent();
                var grid = sender;
                var masterTable = grid.get_masterTableView();
                var row = masterTable.get_dataItems()[eventArgs.get_itemIndexHierarchical()];
                if (evt.target.tagName == "INPUT" || evt.target.tagName == "A") {
                    return;
                }

                var index = eventArgs.get_itemIndexHierarchical();
                document.getElementById("radGridClickedRowIndex").value = index;
                
                var Id = eventArgs.getDataKeyValue("ID");
                document.getElementById("<%= lblObjId.ClientID %>").value = Id;

                var cellCanReject = masterTable.getCellByColumnUniqueName(row, "CanReject");
                var canReject = cellCanReject.innerHTML == "&nbsp;" ? false : cellCanReject.innerHTML;
                document.getElementById("<%= CanReject.ClientID %>").value = canReject;

                var cellObjectType = masterTable.getCellByColumnUniqueName(row, "ObjectTypeEdit");
                var objectType = cellObjectType.innerHTML == "&nbsp;" ? false : cellObjectType.innerHTML;
                document.getElementById("<%= ObjectType.ClientID %>").value = objectType;

                var cellObjectId = masterTable.getCellByColumnUniqueName(row, "ObjectID");
                var objectId = cellObjectId.innerHTML == "&nbsp;" ? false : cellObjectId.innerHTML;
                document.getElementById("<%= ObjectId.ClientID %>").value = objectId;

                var cellIsDistribute = masterTable.getCellByColumnUniqueName(row, "IsDistributeOnshore");
                var isDistribute = cellIsDistribute.innerHTML == "&nbsp;" ? false : cellIsDistribute.innerHTML;
                document.getElementById("<%= IsDistributeOnshore.ClientID %>").value = isDistribute;

                var cellOnshoreComment = masterTable.getCellByColumnUniqueName(row, "IsOnshoreComment");
                var onshoreComment = cellOnshoreComment.innerHTML == "&nbsp;" ? false : cellOnshoreComment.innerHTML;
                document.getElementById("<%= IsOnshoreComment.ClientID %>").value = onshoreComment;

                var cellIsFinal = masterTable.getCellByColumnUniqueName(row, "IsFinal");
                var isFinal = cellIsFinal.innerHTML == "&nbsp;" ? false : cellIsFinal.innerHTML;
                document.getElementById("<%= IsFinal.ClientID %>").value = isFinal;

                var cellAction = masterTable.getCellByColumnUniqueName(row, "ActionTypeId");
                var actionType = cellAction.innerHTML == "&nbsp;" ? false : cellAction.innerHTML;
                document.getElementById("<%= ActionType.ClientID %>").value = actionType;

                sender.get_masterTableView().selectItem(sender.get_masterTableView().get_dataItems()[index].get_element(), true);

                menu.show(evt);

                evt.cancelBubble = true;
                evt.returnValue = false;

                if (evt.stopPropagation) {
                    evt.stopPropagation();
                    evt.preventDefault();
                }
            }
        /* ]]> */
        </script>
    </telerik:RadCodeBlock>
</asp:Content>