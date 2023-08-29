﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TrackingBreakdownReportList.aspx.cs" Inherits="EDMs.Web.Controls.WMS.TrackingBreakdownReportList" EnableViewState="true" %>

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

        .rbSkinnedButton {
            vertical-align: top !important;
        }
    </style>
    
    <div style="width: 98%; padding-top: 10px; padding-left: 5px">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/project.png" /> Selected project:<br />
        
        <telerik:RadComboBox ID="ddlProject" runat="server" 
            Skin="Windows7" Width="100%" AutoPostBack="True" 
            OnItemDataBound="ddlProject_ItemDataBound"
            OnSelectedIndexChanged="ddlProject_SelectedIndexChanged"/>
        <br />
        <hr/>
        Tracking Type:
        <telerik:RadTreeView ID="rtvDiscipline" runat="server" 
            Width="100%" Height="100%" ShowLineImages="False">
            <DataBindings>
                <telerik:RadTreeNodeBinding Expanded="false"></telerik:RadTreeNodeBinding>
            </DataBindings>
            <Nodes>
                <telerik:RadTreeNode runat="server" Text="Tracking List Of Morning Call" ImageUrl="~/Images/trackinglist.png" Value="MorningCall" NavigateUrl="TrackingMorningCallList.aspx"/>
                <%--<telerik:RadTreeNode runat="server" Text="Tracking List Of MR" ImageUrl="~/Images/trackinglist.png" Value="MR" NavigateUrl="TrackingMRList.aspx"/>
                <telerik:RadTreeNode runat="server" Text="Tracking List Of WR" ImageUrl="~/Images/trackinglist.png" Value="WR" NavigateUrl="TrackingWRList.aspx"/>--%>
                <telerik:RadTreeNode runat="server" Text="Tracking List Of ECR" ImageUrl="~/Images/trackinglist.png" Value="ECR" NavigateUrl="TrackingECRList.aspx" Selected="True"/>
                <telerik:RadTreeNode runat="server" Text="Tracking List Of MOC" ImageUrl="~/Images/trackinglist.png" Value="MOC" NavigateUrl="TrackingMOCList.aspx"/>
                <telerik:RadTreeNode runat="server" Text="Tracking List Of Breakdown Report" ImageUrl="~/Images/trackinglist.png" Value="BreakdownReport" NavigateUrl="TrackingBreakdownReportList.aspx" Selected="True"/>
                <telerik:RadTreeNode runat="server" Text="Tracking List Of Shutdown Report" ImageUrl="~/Images/trackinglist.png" Value="ShutdownReport" NavigateUrl="TrackingShutdownReportList.aspx"/>
                <telerik:RadTreeNode runat="server" Text="Tracking List Of WCR" ImageUrl="~/Images/trackinglist.png" Value="WCR" NavigateUrl="TrackingWCRList.aspx"/>
                <telerik:RadTreeNode runat="server" Text="Tracking List Of Punch List" ImageUrl="~/Images/trackinglist.png" Value="Punch" NavigateUrl="TrackingPunchList.aspx"/>
                <telerik:RadTreeNode runat="server" Text="Tracking List Of Sail List" ImageUrl="~/Images/trackinglist.png" Value="Sail" NavigateUrl="TrackingSailList.aspx"/>
                <telerik:RadTreeNode runat="server" Text="Tracking List Of Procedure" ImageUrl="~/Images/trackinglist.png" Value="Procedure" NavigateUrl="TrackingProcedureList.aspx"/>
                <telerik:RadTreeNode runat="server" Text="General Working" ImageUrl="~/Images/trackinglist.png" Value="GeneralWorking" NavigateUrl="TrackingGeneralWorkingList.aspx"/>
            </Nodes>
        </telerik:RadTreeView>
    </div>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Horizontal" Width="100%">
        <telerik:RadPane ID="RadPane3" runat="server" Height="30px" Scrollable="false" Scrolling="None">
            <telerik:RadToolBar ID="CustomerMenu" runat="server" Width="100%" OnClientButtonClicking="OnClientButtonClicking">
                <Items>
                    <telerik:RadToolBarDropDown runat="server" Text="New" ImageUrl="~/Images/addNew.png" Visible="False">
                        <Buttons>
                            <telerik:RadToolBarButton runat="server" Text="Breakdown Report" Value="1" ImageUrl="~/Images/trackinglist.png"/>
                        </Buttons>
                    </telerik:RadToolBarDropDown>
                    
                    <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                    <telerik:RadToolBarButton runat="server" Value="ShowAll">
                        <ItemTemplate>
                            <b style="color: red">Special Filter: </b> <b>Show</b> 
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="min25Percent" Width="200px" style="max-width: 316px"
                                OnSelectedIndexChanged="ddlStatus_OnSelectedIndexChanged" AutoPostBack="True">
                                <Items>
                                    <asp:ListItem Value="1" Text="All Breakdown Report"></asp:ListItem>
                                    <%--<asp:ListItem Value="2" Text="All Latest Breakdown Report" Selected="True"></asp:ListItem>--%>
                                    <asp:ListItem Value="3" Text="Coming Due Breakdown Report" style="background-color: yellow"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Overdue Breakdown Report" style="background-color: red"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="Completed Breakdown Report" style="background-color: greenyellow"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="Incompleted Breakdown Report" style="background-color: coral"></asp:ListItem>
                                </Items>
                            </asp:DropDownList>&nbsp;&nbsp;|&nbsp;&nbsp;
                            <b>Deadline</b> From 
                            <telerik:RadDatePicker ID="txtDeadlineFrom"  runat="server" 
                                ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired" Width="100"
                                OnSelectedDateChanged="txtDeadlineFrom_OnSelectedDateChanged" AutoPostBack="True">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>
                            
                            &nbsp;&nbsp;To
                            <telerik:RadDatePicker ID="txtDeadlineTo"  runat="server" 
                                ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired" Width="100"
                                OnSelectedDateChanged="txtDeadlineTo_OnSelectedDateChanged" AutoPostBack="True">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>
                            
                            &nbsp;&nbsp;|&nbsp;&nbsp;
                            <b>Search All Text Field</b>
                            <asp:TextBox ID="txtSearchAllField" runat="server" Style="width: 300px;" CssClass="min25Percent"/>
                            <telerik:RadButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Width="25px" style="text-align: center">
                                <Icon PrimaryIconUrl="../../Images/search.png" PrimaryIconLeft="3" PrimaryIconTop="2" PrimaryIconWidth="16" PrimaryIconHeight="16"/>
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:RadToolBarButton>
                </Items>
            </telerik:RadToolBar>

        </telerik:RadPane>
        <telerik:RadPane ID="RadPane2" runat="server" Scrollable="false" Scrolling="None">
            <telerik:RadSplitter ID="Radsplitter3" runat="server" Orientation="Horizontal">
                <telerik:RadPane ID="Radpane4" runat="server" Scrolling="None" >
                    
                     <telerik:RadSplitter ID="Radsplitter10" runat="server" Orientation="Vertical">
                         <telerik:RadPane ID="Radpane6" runat="server" Scrolling="None">
                                <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                    GridLines="None" Height="100%"
                                    OnItemDataBound="grdDocument_ItemDataBound"
                                    OnDeleteCommand="grdDocument_DeleteCommand" 
                                    OnItemCreated="grdDocument_ItemCreated"
                                    OnNeedDataSource="grdDocument_OnNeedDataSource" 
                                    PageSize="100" Style="outline: none">
                                    <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%" TableLayout="Auto" CssClass="rgMasterTable" CommandItemDisplay="Top" EditMode="InPlace">
                                        <%--<GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldAlias="Code" FieldName="Code" FormatString="{0:D}"
                                                        HeaderValueSeparator=": "></telerik:GridGroupByField>
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldName="Code" SortOrder="Ascending" ></telerik:GridGroupByField>
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>
                                        </GroupByExpressions> --%>
                                        <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="false"/>

                                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ColumnGroups>
                                            <telerik:GridColumnGroup HeaderText="PIC" Name="PIC"
                                                 HeaderStyle-HorizontalAlign="Center"/>
                                            <telerik:GridColumnGroup HeaderText="Action Plan" Name="ActionPlan"
                                                 HeaderStyle-HorizontalAlign="Center"/>
                                        </ColumnGroups>

                                        <Columns>
                                            <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                                            <telerik:GridBoundColumn DataField="IsInWFProcess" UniqueName="IsInWFProcess" Display="False" />
                                            <telerik:GridBoundColumn DataField="IsWFComplete" UniqueName="IsWFComplete" Display="False" />
                                            
                                            <telerik:GridTemplateColumn UniqueName="CreatedByName" HeaderText="Updated By">
                                                <HeaderStyle Width="200" HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left"/>
                                                <ItemTemplate>
                                                    <%# Eval("CreatedByName")%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridDateTimeColumn HeaderText="Updated Date" UniqueName="CreatedDate" DataField="CreatedDate"
                                                    DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </telerik:GridDateTimeColumn>
                                            
                                            <telerik:GridTemplateColumn HeaderText="Completed" UniqueName="IsComplete" AllowFiltering="False">
                                                <HeaderStyle Width="80" HorizontalAlign="Center" ></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/ok.png" Visible='<%# Eval("IsComplete") %>'/>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridBoundColumn UniqueName="Code" HeaderText="Code" DataField="Code">
                                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </telerik:GridBoundColumn>

                                            <telerik:GridDateTimeColumn UniqueName="BrekdownDate" HeaderText="Equipment Breakdown Date" DataField="BrekdownDate"
                                                 DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </telerik:GridDateTimeColumn>

                                            <telerik:GridTemplateColumn UniqueName="BreakdownSystemName" HeaderText="Name of Breakdown Equipments/System">
                                                <HeaderStyle Width="200" HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BreakdownSystemName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridBoundColumn DataField="TagNo" HeaderText="Breakdown Equipment Name or Tag No" UniqueName="TagNo">
                                                <HeaderStyle HorizontalAlign="Center" Width="200" />
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="SystemName" HeaderText="Equipment/System Name" UniqueName="SystemName">
                                                <HeaderStyle HorizontalAlign="Center" Width="200" />
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="Priority" HeaderText="Priority" UniqueName="Priority">
                                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="CauseGroup" HeaderText="Cause Group" UniqueName="CauseGroup">
                                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </telerik:GridBoundColumn>
                                            

                                            <telerik:GridTemplateColumn  DataField="Description" HeaderText="Defective/Event Descriptions" UniqueName="Description" 
                                                AllowFiltering="false">
                                                <HeaderStyle HorizontalAlign="Center" Width="300" />
                                                <ItemStyle HorizontalAlign="Left"  />
                                                <ItemTemplate>
                                                    <%# Eval("Description").ToString().Replace(Environment.NewLine, "<br/>") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridBoundColumn DataField="FailureDuplication" HeaderText="Failure Duplication" UniqueName="FailureDuplication">
                                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridTemplateColumn  DataField="RootCause" HeaderText="Root Causes Analysis" UniqueName="RootCause" 
                                                AllowFiltering="false">
                                                <HeaderStyle HorizontalAlign="Center" Width="300" />
                                                <ItemStyle HorizontalAlign="Left"  />
                                                <ItemTemplate>
                                                    <%# Eval("RootCause").ToString().Replace(Environment.NewLine, "<br/>") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn  DataField="ProposedAction" HeaderText="Proposed Temporary/Remedial Action" UniqueName="ProposedAction" 
                                                AllowFiltering="false">
                                                <HeaderStyle HorizontalAlign="Center" Width="300" />
                                                <ItemStyle HorizontalAlign="Left"  />
                                                <ItemTemplate>
                                                    <%# Eval("ProposedAction").ToString().Replace(Environment.NewLine, "<br/>") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn  DataField="Lesson" HeaderText="Lesson Learn/Action Taken To Prevent Recurrence" UniqueName="Lesson" 
                                                AllowFiltering="false">
                                                <HeaderStyle HorizontalAlign="Center" Width="300" />
                                                <ItemStyle HorizontalAlign="Left"  />
                                                <ItemTemplate>
                                                    <%# Eval("Lesson").ToString().Replace(Environment.NewLine, "<br/>") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridBoundColumn DataField="UnplannedWoNo" HeaderText="Unplanned WO Number" UniqueName="UnplannedWoNo">
                                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridTemplateColumn  DataField="PICName" HeaderText="PIC" UniqueName="PICName" ColumnGroupName="PIC"
                                                AllowFiltering="false">
                                                <HeaderStyle HorizontalAlign="Center" Width="200" />
                                                <ItemStyle HorizontalAlign="Left"  />
                                                <ItemTemplate>
                                                    <%# Eval("PICName").ToString().Replace(Environment.NewLine, "<br/>") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridDateTimeColumn UniqueName="PICDeadline" HeaderText="Deadline" DataField="PICDeadline" ColumnGroupName="PIC"
                                                 DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </telerik:GridDateTimeColumn>
                                            
                                            <telerik:GridBoundColumn DataField="PICStatus" HeaderText="Status" UniqueName="PICStatus" ColumnGroupName="PIC">
                                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridTemplateColumn  DataField="CurrentStatus" HeaderText="Current Status" UniqueName="CurrentStatus" 
                                                AllowFiltering="false">
                                                <HeaderStyle HorizontalAlign="Center" Width="300" />
                                                <ItemStyle HorizontalAlign="Left"  />
                                                <ItemTemplate>
                                                    <%# Eval("CurrentStatus").ToString().Replace(Environment.NewLine, "<br/>") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridBoundColumn DataField="Status" HeaderText="Closed/Open" UniqueName="Status">
                                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridBoundColumn DataField="MRWRItem" HeaderText="MR & Item/ WR" UniqueName="MRWRItem">
                                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridTemplateColumn  DataField="Comment" HeaderText="Comments" UniqueName="Comment" 
                                                AllowFiltering="false">
                                                <HeaderStyle HorizontalAlign="Center" Width="300" />
                                                <ItemStyle HorizontalAlign="Left"  />
                                                <ItemTemplate>
                                                    <%# Eval("Comment").ToString().Replace(Environment.NewLine, "<br/>") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn HeaderText="Cost" UniqueName="Cost" AllowFiltering="false">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right"  />
                                                <ItemTemplate>
                                                    <%# Eval("Cost", "{0: ###,##0.##}")%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                                        <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                                        <Selecting AllowRowSelect="true" />
                                        <ClientEvents  OnGridCreated="GetGridObject" />
                                        <ClientEvents OnRowContextMenu="RowContextMenu" OnRowClick="RowClick"></ClientEvents>

                                        <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                                    </ClientSettings>
                            </telerik:RadGrid>
                        </telerik:RadPane>

                     </telerik:RadSplitter>       

                </telerik:RadPane>
            </telerik:RadSplitter>
        </telerik:RadPane>
    </telerik:RadSplitter>
    <telerik:RadContextMenu ID="radMenu" runat="server"
        EnableRoundedCorners="true" EnableShadows="true" OnClientItemClicking="gridMenuClicking" OnClientShowing="gridContextMenuShowing">
        <Items>
            <telerik:RadMenuItem Text="Workflow Process History" ImageUrl="~/Images/history.png" Value="WFHistory"/>
            <telerik:RadMenuItem IsSeparator="True"/>
            <telerik:RadMenuItem Text="Update BreakdownReport Info" ImageUrl="~/Images/edit.png" Value="EditProperties" DisabledImageUrl="~/Images/dis_edit.png"/>
            
            <telerik:RadMenuItem Text="BreakdownReport Detail" ImageUrl="~/Images/detail.png" Value="MRDetail" DisabledImageUrl="~/Images/dis_detail.png"/>
            <telerik:RadMenuItem IsSeparator="True"/>
            <telerik:RadMenuItem Text="Attach document" ImageUrl="~/Images/attach.png" Value="AttachDocument" DisabledImageUrl="~/Images/dis_attach.png"/>
            <telerik:RadMenuItem IsSeparator="True"/>
            <telerik:RadMenuItem Text="Cancel BreakdownReport" ImageUrl="~/Images/cancel1.png" Value="CancelBreakdownReport"/>

        </Items>
    </telerik:RadContextMenu>

    <table>
        <tr>
            <td style="width: 250px;"></td>
            <td style="width: 500px;"></td>
        </tr>
    </table>
    <span style="display: none">
        

        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
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
                
                <telerik:AjaxSetting AjaxControlID="ddlProject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lblProjectId" />
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="ddlStatus">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="txtDeadlineFrom">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="txtDeadlineTo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </span>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CreateNewWD" runat="server" Title="Create New Breakdown Report"
                VisibleStatusbar="False" Height="700" Width="850" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="UpdateWD" runat="server" Title="Update Breakdown Report"
                VisibleStatusbar="False" Height="700" Width="850" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachDoc" runat="server" Title="Attach files"
                VisibleStatusbar="false" Height="500" Width="700"  
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RevisionHistory" runat="server" Title="Breakdown Report Revision History"
                VisibleStatusbar="false" Height="550" Width="1250" MinHeight="550"  
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="BreakdownReportDetailDialog" runat="server" Title="BreakdownReport Details"
                VisibleStatusbar="false" Height="700" Width="650" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
                
                 <telerik:RadWindow ID="WFProcessHistory" runat="server" Title="Workflow Process History"
                VisibleStatusbar="False" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="BreakdownReport Information"
                VisibleStatusbar="false" Height="700" Width="650" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
    <asp:HiddenField runat="server" ID="FolderContextMenuAction"/>
    <asp:HiddenField runat="server" ID="lblFolderId"/>
    <asp:HiddenField runat="server" ID="lblDocId"/>
    <asp:HiddenField runat="server" ID="lblCategoryId"/>
    <asp:HiddenField runat="server" ID="lblProjectId"/>
    <asp:HiddenField runat="server" ID="IsInWF"/>
    <asp:HiddenField runat="server" ID="IsWFComplete"/>
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

                toolbar = $find("<%= CustomerMenu.ClientID %>");
                ajaxManager = $find("<%=ajaxCustomer.ClientID %>");

                searchButton = toolbar.findButtonByCommandName("doSearch");

                $telerik.$(".searchtextbox")
                    .bind("keypress", function (e) {
                        searchButton.set_imageUrl("~/Images/search.gif");
                        searchButton.set_value("search");
                    });
            }

            function ShowEditForm(id) {
                var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                var owd = $find("<%=UpdateWD.ClientID %>");
                owd.setSize(850, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("TrackingBreakdownReportEditForm.aspx?objId=" + id + "&projId=" + projectId, "UpdateWD");
            }

            function ShowAttachment(id) {
                var owd = $find("<%=AttachDoc.ClientID %>");
                owd.setSize(700, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("TrackingBreakdownReportAttach.aspx?objId=" + id, "AttachDoc");
            }

            function ShowHistory(id) {
                var owd = $find("<%=RevisionHistory.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.setUrl("TrackingBreakdownReportRevisionHistory.aspx?objId=" + id, "RevisionHistory");
            }
            
            function refreshGrid(arg) {
                //alert(arg);
                if (!arg) {
                    var masterTable = $find("<%=grdDocument.ClientID%>").get_masterTableView();
                    masterTable.rebind();
                }
                else {
                    var masterTable = $find("<%=grdDocument.ClientID%>").get_masterTableView();
                    masterTable.rebind();
                }
            }
            
            function refreshTab(arg) {
                $('.EDMsRadPageView' + arg + ' iframe').attr('src', $('.EDMsRadPageView' + arg + ' iframe').attr('src'));
            }

            <%--function RowDblClick(sender, eventArgs) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Customers/ViewCustomerDetails.aspx?docId=" + eventArgs.getDataKeyValue("Id"), "CustomerDialog");
                // window.radopen("Controls/Customers/ViewCustomerDetails.aspx?patientId=" + eventArgs.getDataKeyValue("Id"), "CustomerDialog");
            }--%>
            
            
            
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

                var grid = $find("<%= grdDocument.ClientID %>");
                var customerId = null;
                var customerName = "";

                //if (grid.get_masterTableView().get_selectedItems().length > 0) {
                //    var selectedRow = grid.get_masterTableView().get_selectedItems()[0];
                //    customerId = selectedRow.getDataKeyValue("Id");
                //    //customerName = selectedRow.Items["FullName"]; 
                //    //customerName = grid.get_masterTableView().getCellByColumnUniqueName(selectedRow, "FullName").innerHTML;
                //}


                
                 
                if (strText == "Breakdown Report") {
                    var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                    var owd = $find("<%=CreateNewWD.ClientID %>");
                    owd.setSize(850, document.documentElement.offsetHeight);
                    owd.Show();
                    owd.setUrl("TrackingBreakdownReportEditForm.aspx?projId=" + projectId, "CustomerDialog");
                }
            }
            
            function gridMenuClicking(sender, args) {
                var itemValue = args.get_item().get_value();
                var docId = document.getElementById("<%= lblDocId.ClientID %>").value;
                switch (itemValue) {
                case "MRDetail":
                    var categoryId = document.getElementById("<%= lblCategoryId.ClientID %>").value;
                    var owd = $find("<%=BreakdownReportDetailDialog.ClientID %>");
                    owd.setSize(850, document.documentElement.offsetHeight);
                    owd.Show();
                    owd.setUrl("TrackingBreakdownReportDetail.aspx?objId=" + docId, "BreakdownReportDetailDialog");
                    break;
                case "WFHistory":
                    var owd = $find("<%=WFProcessHistory.ClientID %>");
                    owd.Show();
                    owd.maximize();
                    owd.setUrl("../../Controls/Workflow/WorkflowProcessHistory.aspx?objId=" + docId + "&objType=BreakdownReport",
                        "WFProcessHistory");
                    break;
                case "AttachDocument":
                    var owd = $find("<%=AttachDoc.ClientID %>");
                    owd.setSize(730, document.documentElement.offsetHeight);
                    owd.Show();

                    owd.setUrl("TrackingBreakdownReportAttach.aspx?objId=" + docId, "AttachDoc");
                    break;
                case "EditProperties":
                    var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                    var owd = $find("<%=CustomerDialog.ClientID %>");
                    owd.setSize(850, document.documentElement.offsetHeight);
                    owd.Show();
                    owd.setUrl("TrackingBreakdownReportEditForm.aspx?objId=" + docId + "&projId=" + projectId,
                        "CustomerDialog");
                    break;
                    case "CancelBreakdownReport":
                        if (confirm("Do you want to cancel BreakdownReport?") === false) return;
                        ajaxManager.ajaxRequest("Cancel_" + docId);
                        break;
                }
            }

            function RowContextMenu(sender, eventArgs) {
                var grid = sender;
                var masterTable = grid.get_masterTableView();
                var row = masterTable.get_dataItems()[eventArgs.get_itemIndexHierarchical()];

                var menu = $find("<%=radMenu.ClientID %>");
                var evt = eventArgs.get_domEvent();

                if (evt.target.tagName == "INPUT" || evt.target.tagName == "A") {
                    return;
                }

                var index = eventArgs.get_itemIndexHierarchical();
                document.getElementById("radGridClickedRowIndex").value = index;
                
                var Id = eventArgs.getDataKeyValue("ID");
                document.getElementById("<%= lblDocId.ClientID %>").value = Id;

                var cellIsInWF = masterTable.getCellByColumnUniqueName(row, "IsInWFProcess");
                var IsInWFProcess = cellIsInWF.innerHTML == "&nbsp;" ? false : cellIsInWF.innerHTML;
                document.getElementById("<%= IsInWF.ClientID %>").value = IsInWFProcess;

                var cellIsWFComplete = masterTable.getCellByColumnUniqueName(row, "IsWFComplete");
                var IsWFComplete = cellIsWFComplete.innerHTML == "&nbsp;" ? false : cellIsWFComplete.innerHTML;
                document.getElementById("<%= IsWFComplete.ClientID %>").value = IsWFComplete;

                sender.get_masterTableView().selectItem(sender.get_masterTableView().get_dataItems()[index].get_element(), true);

                menu.show(evt);

                evt.cancelBubble = true;
                evt.returnValue = false;

                if (evt.stopPropagation) {
                    evt.stopPropagation();
                    evt.preventDefault();
                }
            }

            function gridContextMenuShowing(menu, args) {
                var isInWF = document.getElementById("<%= IsInWF.ClientID %>").value;
                var isWFComplete = document.getElementById("<%= IsWFComplete.ClientID %>").value;

                if (isInWF == "True" || isWFComplete == "True") {
                    menu.get_allItems()[0].enable();

                } else {
                    menu.get_allItems()[0].disable();

                }

                if (isInWF != "True") {
                    menu.get_allItems()[2].enable();
                }
                else {
                    menu.get_allItems()[2].disable();
                }
            }
            
        /* ]]> */
        </script>
    </telerik:RadCodeBlock>
</asp:Content>

<%--Tan.Le Remove here--%>
<%--<uc1:List runat="server" ID="CustomerList"/>--%>
<%-- <div id="EDMsCustomers" runat="server" />--%>
