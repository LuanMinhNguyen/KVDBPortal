﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransmittalAttachDocument.aspx.cs" Inherits="EDMs.Web.Controls.Document.TransmittalAttachDocument" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Content/styles.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.7.1.js" type="text/javascript"></script>
    
    <style type="text/css">
        
        html, body, form {
	        overflow:auto;
        }
        .RadGrid .rgSelectedRow
        {
            background-image : none !important;
            background-color: coral !important;
        }
        .accordion dt a
        {
            color: #085B8F;
            border-bottom: 2px solid #46A3D3;
            font-size: 1.5em;
            font-weight: bold;
            letter-spacing: -0.03em;
            line-height: 1.2;
            margin: 0.5em auto 0.6em;
            padding: 0;
            text-align: left;
            text-decoration: none;
            display: block;
        }

        .accordion dt span
        {
            color: #085B8F;
            border-bottom: 2px solid #46A3D3;
            font-size: 1.5em;
            font-weight: bold;
            letter-spacing: -0.03em;
            line-height: 1.2;
            margin: 0.5em auto 0.6em;
            padding: 0;
            text-align: left;
            text-decoration: none;
            display: block;
        }

        .qlcbTooltip {
            line-height: 1.8;
            padding-right: 5px;
            text-align: right;
        }
            .qlcbFormItem input[type="text"], .qlcbFormItem textarea, .qlcbFormItem select {
            border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;
            border-style: solid;
            border-width: 1px 1px 1px 5px;
            color: #000000;
            float: left;
            font: 12px "segoe ui";
            margin: 0;
            padding: 2px 5px 3px;
            vertical-align: middle;
    
        }
            html body .riSingle .riTextBox, html body .riSingle .riTextBox[type="text"] {
   
            border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3!important;
            border-style: solid!important;
            border-width: 1px 1px 1px 5px!important;
            color: #000000!important;
            float: left!important;
            font: 12px "segoe ui"!important;
            margin: 0!important;
            padding: 2px 5px 3px!important;
            vertical-align: middle!important;
        }
            div.RadPicker input.qlcbFormRequired1[type="text"], div.RadPicker_Default input.qlcbFormRequired1[type="text"] {
            border-left-color: Red!important;
            border-left-width: 5px!important;
        }
            .qlcbFormItem input.min25Percent[type="text"], div.qlcbFormItem textarea.min25Percent {
            min-width: 235px;
        }
        .qlcbFormItem input.minFullWidth[type="text"], div.qlcbFormItem textarea.minFullWidth {
            min-width: 626px;
        }
        .qlcbFormItem select.min25Percent {
            min-width: 250px;
        }
        .qlcbFormItem input.min50Percent[type="text"], div.qlcbFormItem textarea.min50Percent, div.qlcbFormItem select.min50Percent {
            min-width: 50%;
        }
        .qlcbFormItem input.qlcbFormRequired[type="text"], div.qlcbFormItem textarea.qlcbFormRequired, div.qlcbFormItem select.qlcbFormRequired {
            border-left-color: #FF0000;
            border-left-width: 5px;
        }
        .qlcbFormItem input.qlcbFormUPPERCASE[type="text"], div.qlcbFormItem textarea.qlcbFormUPPERCASE {
            text-transform: uppercase;
        }
        .qlcbFormItem input[type="text"], div.qlcbFormItem textarea, div.qlcbFormItem select {
            border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;
            border-style: solid;
            border-width: 1px 1px 1px 5px;
            color: #000000;
            float: left;
            font: 12px "segoe ui";
            margin: 0;
            padding: 2px 5px 3px;
            vertical-align: middle;
        }
        .qlcbFormItem input[type="text"]:hover, div.qlcbFormItem select:hover {
            border-color: #000000 #000000 #000000 #46A3D3;
        }
        .qlcbFormItem input.qlcbFormRequired[type="text"]:hover, div.qlcbFormItem select.qlcbFormRequired:hover {
            border-color: #000000 #000000 #000000 #FF0000;
        }
        .RadPicker, div.RadPicker_Default {
            display: inline !important;
            float: left !important;
        }
        .min25Percent {
            min-width: 217px;
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
    <script type="text/javascript">
        function CloseAndRebind(args) {
            GetRadWindow().BrowserWindow.refreshGrid(args);
            GetRadWindow().close();
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz as well)

            return oWindow;
        }

        function CancelEdit() {
            GetRadWindow().close();
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <table style="width: 90%">
            <tr class="qlcbFormItem">
                <td width="20%" class="qlcbTooltip"> <span id="Span2">Selected Project:</span></td>
                <td width="30%"> <asp:DropDownList ID="ddlProject" runat="server" CssClass="min25Percent" Width="316" OnSelectedIndexChanged="ddlProject_SelectedIndexChange" AutoPostBack="True"/></td>
                <td width="20%" class="qlcbTooltip">  <span id="Span3">Workgroups</span></td>
                <td width="*"><asp:DropDownList ID="ddlWorkgroup" runat="server" CssClass="min25Percent" Width="316"/>
                </td>
            </tr>
            <tr><td height="3px" colspan="4"></td></tr>
            <tr class="qlcbFormItem">
                <td class="qlcbTooltip">  <span id="Span6">Document No.:</span></td>
                <td><asp:TextBox ID="txtDocNo" runat="server" CssClass="min25Percent" Width="300px"/></td>
                <td class="qlcbTooltip"> <span id="Span7">Document Title:</span></td>
                <td><asp:TextBox ID="txtDocTitle" runat="server" CssClass="min25Percent" Width="300px"></asp:TextBox>
            </td>
            </tr>
        </table>
            <div style="text-align: center; padding-top: 5px; padding-bottom: 10px">
                <telerik:RadButton ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" Width="70px" style="text-align: center" />
                <telerik:RadButton ID="btnSave" runat="server" Text="Save selected doc" OnClick="btnSave_Click" Width="110px" style="text-align: center" />
                
            </div>
            <div>
            <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="True" Height="405px"
                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                GridLines="None" 
                Skin="Windows7" AllowCustomPaging="True"
                OnNeedDataSource="grdDocument_OnNeedDataSource" 
                OnItemDataBound="grdDocument_OnItemDataBound"
                PageSize="100" Style="outline: none">
                <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%">
                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldAlias="-" FieldName="DocumentTypeName" FormatString="{0:D}"
                                    HeaderValueSeparator=""></telerik:GridGroupByField>
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="DocumentTypeName" SortOrder="Ascending" ></telerik:GridGroupByField>
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>  
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                     <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="REVISION DETAILS" Name="RevisionDetails"
                                HeaderStyle-HorizontalAlign="Center"/>
                        <telerik:GridColumnGroup HeaderText="OUTGOING TRANSMITTAL" Name="OutgoingTrans"
                                HeaderStyle-HorizontalAlign="Center"/>
                        <telerik:GridColumnGroup HeaderText="INCOMING TRANSMITTAL" Name="IncomingTrans"
                                HeaderStyle-HorizontalAlign="Center"/>
                        <telerik:GridColumnGroup HeaderText="ICA REVIEW DETAILS" Name="ICAReviews"
                                HeaderStyle-HorizontalAlign="Center"/>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridBoundColumn DataField="HasAttachFile" UniqueName="HasAttachFile" Display="False" />                       
                         <telerik:GridTemplateColumn UniqueName="IsSelected">
                            <HeaderStyle Width="2%"  />
                            <ItemStyle HorizontalAlign="Center" Width="2%"/>
                            <ItemTemplate>
                                <asp:CheckBox ID="cboxSelectDocTransmittal" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="No." Groupable="False">
                            <HeaderStyle HorizontalAlign="Center" Width="2%" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="2%"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblSoTT" runat="server" Text='<%# grdDocument.CurrentPageIndex * grdDocument.PageSize + grdDocument.Items.Count+1 %>'>
                                </asp:Label>
                                      
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DOC. No." UniqueName="DocNo">
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                            <ItemTemplate>
                                <%# Eval("DocNo") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="DocNo" runat="server" Value='<%# Eval("DocNo") %>'/>
                                <asp:Label runat="server" ID="lbldocNo"></asp:Label>
                                <%--<asp:TextBox ID="txtDocNo" runat="server" Width="100%"></asp:TextBox>--%>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                                            
                        <telerik:GridTemplateColumn HeaderText="DOC. Title" UniqueName="DocTitle">
                            <HeaderStyle HorizontalAlign="Center" Width="14%" />
                            <ItemStyle HorizontalAlign="Left" Width="14%" />
                            <ItemTemplate>
                                <%# Eval("DocTitle") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="DocTitle" runat="server" Value='<%# Eval("DocTitle") %>'/>
                                <asp:TextBox ID="txtDocTitle" runat="server" Width="98%"></asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                                        
                        <telerik:GridTemplateColumn HeaderText="Deparment" UniqueName="DeparmentName" Display="False">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                            <ItemTemplate>
                                <%# Eval("DeparmentName") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="DeparmentName" runat="server" Value='<%# Eval("DeparmentName") %>'/>
                                <asp:TextBox ID="txtDeparment" runat="server" Width="98%"></asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                                        
                        <telerik:GridTemplateColumn HeaderText="Start" UniqueName="StartDate">
                            <HeaderStyle HorizontalAlign="Center" Width="4%" />
                            <ItemStyle HorizontalAlign="Center" Width="4%"/>
                            <ItemTemplate>
                                <%# Eval("StartDate","{0:dd/MM/yyyy}") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="StartDate" runat="server" Value='<%# Eval("StartDate") %>'/>
                                <telerik:RadDatePicker ID="txtStartDate"  Width="98%" 
                                    runat="server" Skin="Windows7" ShowPopupOnFocus="True"
                                    PopupDirection="BottomRight">
                                    <DateInput ID="txtStartDateInput" runat="server" 
                                        DateFormat="dd/MM/yyyy" ShowButton="False"/>
                                </telerik:RadDatePicker>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                                        
                        <telerik:GridTemplateColumn HeaderText="Planed" UniqueName="PlanedDate" Display="False">
                            <HeaderStyle HorizontalAlign="Center" Width="4%" />
                            <ItemStyle HorizontalAlign="Center" Width="4%"/>
                            <ItemTemplate>
                                <%# Eval("PlanedDate","{0:dd/MM/yyyy}") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="PlanedDate" runat="server" Value='<%# Eval("PlanedDate") %>'/>
                                <telerik:RadDatePicker ID="txtPlanedDate" Width="98%" 
                                    runat="server" Skin="Windows7" ShowPopupOnFocus="True"
                                    PopupDirection="BottomRight">
                                    <DateInput ID="txtPlanedDateInput" runat="server" 
                                        DateFormat="dd/MM/yyyy" ShowButton="False"/>
                                </telerik:RadDatePicker>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                                        
                        <telerik:GridTemplateColumn HeaderText="Rev." UniqueName="Rev"
                            ColumnGroupName="RevisionDetails">
                            <HeaderStyle HorizontalAlign="Center" Width="3%" />
                            <ItemStyle HorizontalAlign="Center" Width="3%"/>
                            <ItemTemplate>
                                <%# Eval("RevisionName") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="RevisionId" runat="server" 
                                    Value='<%# Eval("RevisionId") %>'/>
                                <telerik:RadComboBox ID="RadComboBox1" runat="server"  
                                    DropDownWidth="100px" MaxHeight="300px" Width="98%" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                                        
                        <telerik:GridTemplateColumn HeaderText="Planed" UniqueName="RevisionPlanedDate"
                            ColumnGroupName="RevisionDetails">
                            <HeaderStyle HorizontalAlign="Center" Width="4%" />
                            <ItemStyle HorizontalAlign="Center" Width="4%"/>
                            <ItemTemplate>
                                <%# Eval("RevisionPlanedDate","{0:dd/MM/yyyy}") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="RevisionPlanedDate" runat="server" Value='<%# Eval("RevisionPlanedDate") %>'/>
                                <telerik:RadDatePicker ID="txtRevisionPlanedDate"  Width="98%" 
                                    runat="server" Skin="Windows7" ShowPopupOnFocus="True"
                                    PopupDirection="BottomRight">
                                    <DateInput ID="DateInput1" runat="server" 
                                        DateFormat="dd/MM/yyyy" ShowButton="False"/>
                                </telerik:RadDatePicker>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Actual" UniqueName="RevisionActualDate"
                            ColumnGroupName="RevisionDetails">
                            <HeaderStyle HorizontalAlign="Center" Width="4%" />
                            <ItemStyle HorizontalAlign="Center" Width="4%"/>
                            <ItemTemplate>
                                <%# Eval("RevisionActualDate","{0:dd/MM/yyyy}") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="RevisionActualDate" runat="server" Value='<%# Eval("RevisionActualDate") %>'/>
                                <telerik:RadDatePicker ID="txtRevisionActualDate" Width="98%" 
                                    runat="server" Skin="Windows7" ShowPopupOnFocus="True"
                                    PopupDirection="BottomRight">
                                    <DateInput ID="DateInput2" runat="server" 
                                        DateFormat="dd/MM/yyyy" ShowButton="False"/>
                                </telerik:RadDatePicker>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                                        
                        <telerik:GridTemplateColumn HeaderText="Comment Code" UniqueName="RevisionCommentCode"
                            ColumnGroupName="RevisionDetails">
                            <HeaderStyle HorizontalAlign="Center" Width="4%" />
                            <ItemStyle HorizontalAlign="Left" Width="4%" />
                            <ItemTemplate>
                                <%# Eval("RevisionCommentCode") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="RevisionCommentCode" runat="server" Value='<%# Eval("DeparmentName") %>'/>
                                <asp:TextBox ID="txtRevisionCommentCode" runat="server" Width="98%"></asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                                        
                        <telerik:GridTemplateColumn HeaderText="Complete - %" UniqueName="Complete">
                            <HeaderStyle HorizontalAlign="Center" Width="3%" />
                            <ItemStyle HorizontalAlign="Center" Width="3%" />
                            <ItemTemplate>
                                <%# Eval("Complete") + "%"%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="Complete" runat="server" Value='<%# Eval("Complete") %>'/>
                                <telerik:radnumerictextbox type="Percent" id="txtComplete" 
                                    runat="server" Width="98%">
                                    <NumberFormat DecimalDigits="0"></NumberFormat>
                                </telerik:radnumerictextbox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Weight - %" UniqueName="Weight">
                            <HeaderStyle HorizontalAlign="Center" Width="3%" />
                            <ItemStyle HorizontalAlign="Center" Width="3%" />
                            <ItemTemplate>
                                <%# Eval("Weight") + "%"%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="Weight" runat="server" Value='<%# Eval("Weight") %>'/>
                                <telerik:radnumerictextbox type="Percent" id="txtWeight" 
                                    runat="server" Width="98%">
                                    <NumberFormat DecimalDigits="0"></NumberFormat>
                                </telerik:radnumerictextbox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="EMDR" UniqueName="IsEMDR" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" Width="3%" />
                            <ItemStyle HorizontalAlign="Center" Width="3%" />
                            <ItemTemplate>
                                <asp:Image ID="imgIsEMDR" runat="server" ImageUrl="~/Images/ok.png" Visible='<%# Eval("IsEMDR")%>'/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="IsEMDR" runat="server" Value='<%# Eval("IsEMDR") %>'/>
                                <asp:CheckBox ID="cbIsEMDR" runat="server"></asp:CheckBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings Selecting-AllowRowSelect="true" EnablePostBackOnRowClick="true" AllowColumnHide="True">
                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                    <Selecting AllowRowSelect="true" />
                    <ClientEvents  OnGridCreated="GetGridObject" />
                    <ClientEvents OnRowContextMenu="RowContextMenu" OnRowClick="RowClick"></ClientEvents>
                    <Scrolling AllowScroll="True" SaveScrollPosition="True" UseStaticHeaders="True" />
                </ClientSettings>
            </telerik:RadGrid>
        </div>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
        <span style="display: none">
        
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlProject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlWorkgroup" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </span>
    <telerik:RadContextMenu ID="radMenu" runat="server"
        EnableRoundedCorners="true" EnableShadows="true" OnClientItemClicking="gridMenuClicking">
        <Items>
            <telerik:RadMenuItem Text="Revision history" ImageUrl="~/Images/revision.png" Value="RevisionHistory">
            </telerik:RadMenuItem>
            <telerik:RadMenuItem Text="Version history" ImageUrl="~/Images/history.png" Value="VersionHistory">
            </telerik:RadMenuItem>
            <telerik:RadMenuItem IsSeparator="True">
            </telerik:RadMenuItem>
            <telerik:RadMenuItem Text="Check out" ImageUrl="~/Images/checkout.png" Value="CheckOut">
            </telerik:RadMenuItem>
            <telerik:RadMenuItem Text="Check in" ImageUrl="~/Images/checkin.png" Value="CheckIn">
            </telerik:RadMenuItem>
        </Items>
    </telerik:RadContextMenu>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Document Information"
                VisibleStatusbar="false" Height="690" Width="650" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RevisionDialog" runat="server" Title="Revision history" OnClientClose="refreshGrid"
                VisibleStatusbar="false" Height="332" Width="1200" MinHeight="332" MinWidth="1200" MaxHeight="332" MaxWidth="1200" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="VersionHistory" runat="server" Title="Version history"
                VisibleStatusbar="false" Height="332" Width="1200" MinHeight="332" MinWidth="1200" MaxHeight="332" MaxWidth="1200" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
    <asp:HiddenField runat="server" ID="FolderContextMenuAction"/>
    <asp:HiddenField runat="server" ID="lblFolderId"/>
    <asp:HiddenField runat="server" ID="lblDocId"/>
    <input type="hidden" id="radGridClickedRowIndex" name="radGridClickedRowIndex"/>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="../../Scripts/jquery-1.7.1.js"></script>
        <script type="text/javascript">

            var radDocuments;

            function refreshGrid() {
                var masterTable = $find("<%=grdDocument.ClientID%>").get_masterTableView();
                masterTable.rebind();
            }

            function GetGridObject(sender, eventArgs) {
                radDocuments = sender;
            }

            function RowClick(sender, eventArgs) {
                var Id = eventArgs.getDataKeyValue("ID");
                document.getElementById("<%= lblDocId.ClientID %>").value = Id;
            }

            function gridMenuClicking(sender, args) {
                var itemValue = args.get_item().get_value();
                var docId = document.getElementById("<%= lblDocId.ClientID %>").value;



                switch (itemValue) {
                    case "RevisionHistory":
                        var owd = $find("<%=RevisionDialog.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/RevisionHistory.aspx?docId=" + docId, "RevisionDialog");
                        break;
                    case "VersionHistory":
                        var owd = $find("<%=VersionHistory.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/VersionHistory.aspx?docId=" + docId, "VersionHistory");
                        break;
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

            function ShowEditForm(id, rowIndex, folId) {
                var grid = $find("<%= grdDocument.ClientID %>");
                var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;

                var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                grid.get_masterTableView().selectItem(rowControl, true);
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Document/DocumentInfoEditForm.aspx?docId=" + id + "&folId=" + folId, "CustomerDialog");

                // window.radopen("Controls/Customers/CustomerEditForm.aspx?patientId=" + id, "CustomerDialog");
                //  return false;
            }
            function ShowInsertForm() {

                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Customers/CustomerEditForm.aspx", "CustomerDialog");

                //window.radopen("Controls/Customers/CustomerEditForm.aspx", "CustomerDialog");
                //return false;
            }



            function refreshGrid(arg) {
                //alert(arg);
                if (!arg) {
                    ajaxManager.ajaxRequest("Rebind");
                }
                else {
                    ajaxManager.ajaxRequest("RebindAndNavigate");
                }
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

            function onNodeClicking(sender, args) {
                var folderValue = args.get_node().get_value();
                document.getElementById("<%= lblFolderId.ClientID %>").value = folderValue;
            }

            function OnClientButtonClicking(sender, args) {
                var button = args.get_item();
                var strText = button.get_text();
                var strValue = button.get_value();

                var grid = $find("<%= grdDocument.ClientID %>");
                var customerId = null;
                var customerName = "";

                //if (grid.get_masterTableView().get_selectedItems().length > 0) {
                //    var selectedRow = grid.get_masterTableView().get_selectedItems()[0];
                //    customerId = selectedRow.getDataKeyValue("Id");
                //    //customerName = selectedRow.Items["FullName"]; 
                //    //customerName = grid.get_masterTableView().getCellByColumnUniqueName(selectedRow, "FullName").innerHTML;
                //}




                if (strText.toLowerCase() == "documents") {

                    var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                    if (selectedFolder == "") {
                        alert("Please choice one folder to create new document");
                        return false;
                    }

                    var owd = $find("<%=CustomerDialog.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/Document/DocumentInfoEditForm.aspx?folId=" + selectedFolder, "CustomerDialog");

                }

                if (strText == "Thêm mới") {
                    return ShowInsertForm();
                }
                else if (strText == "Import dữ liệu") {
                    return ShowImportForm();
                }
                else if (strText == "Dữ liệu thô") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_1_" + customerId);
                }
                else if (strText == "Tiềm năng") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_2_" + customerId);
                }
                else if (strText == "Chưa liên hệ được") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_3_" + customerId);
                }
                else if (strText == "Không tiềm năng") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_4_" + customerId);
                }
                else if (strText == "Thông tin sai") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_5_" + customerId);
                }
                else if (strText == "Liên hệ tư vấn") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_6_" + customerId);
                }
                else if (strText == "Hẹn tư vấn") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_7_" + customerId);
                }
                else if (strText == "Đã sử dụng dịch vụ") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_8_" + customerId);
                }
                else {
                    var commandName = args.get_item().get_commandName();
                    if (commandName == "doSearch") {
                        var searchTextBox = sender.findButtonByCommandName("searchText").findControl("txtSearch");
                        if (searchButton.get_value() == "clear") {
                            searchTextBox.set_value("");
                            searchButton.set_imageUrl("images/search.gif");
                            searchButton.set_value("search");
                        }

                        performSearch(searchTextBox);
                    } else if (commandName == "reply") {
                        window.radopen(null, "Edit");
                    }
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

                if (evt.target.tagName == "INPUT" || evt.target.tagName == "A") {
                    return;
                }

                var index = eventArgs.get_itemIndexHierarchical();
                document.getElementById("radGridClickedRowIndex").value = index;

                var Id = eventArgs.getDataKeyValue("ID");
                document.getElementById("<%= lblDocId.ClientID %>").value = Id;

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
    </form>
</body>
</html>
