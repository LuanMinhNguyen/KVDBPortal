<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShutdownReport.aspx.cs" Inherits="EDMs.Web.ShutdownReport" EnableViewState="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <!--[if gte IE 8]>
        <style type="text/css">
            #ContentPlaceHolder2_grdDocument_ctl00_Header{table-layout:auto !important;}
            #ContentPlaceHolder2_grdDocument_ctl00{table-layout:auto !important;}
        </style>
    <![endif]-->

    <style type="text/css">
        /*Custom CSS of Grid documents for FF browser*/
        #ContentPlaceHolder2_grdDocument_ctl00_Header{table-layout:auto !important;}
        #ContentPlaceHolder2_grdDocument_ctl00{table-layout:auto !important;}
        /*End*/
        @-moz-document url-prefix() {
            #ContentPlaceHolder2_grdDocument_ctl00_Header{table-layout:auto !important;}
            #ContentPlaceHolder2_grdDocument_ctl00{table-layout:auto !important;}
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

        #ContentPlaceHolder2_grdDocument_ctl00_ctl02_ctl03_txtDate_popupButton {
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
            background-color: darkseagreen !important;
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

        .rbSkinnedButton {
            vertical-align: top !important;
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


    </style>
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Horizontal">
        <telerik:RadPane ID="RadPane3" runat="server" Height="30px" Scrollable="false" Scrolling="None">
            
            <telerik:RadToolBar ID="CustomerMenu" runat="server" Width="100%" OnClientButtonClicking="OnClientButtonClicking">
                <Items>
                    <telerik:RadToolBarDropDown runat="server" Text="Add" ImageUrl="~/Images/addNew.png">
                        <Buttons>
                            <telerik:RadToolBarButton runat="server" Text="Shutdown Report" Value="1" ImageUrl="~/Images/shutdown.png"></telerik:RadToolBarButton>
                            <%--<telerik:RadToolBarButton runat="server" Text="Multi documents" Value="2" ImageUrl="~/Images/addmulti.png"></telerik:RadToolBarButton>--%>
                        </Buttons>
                    </telerik:RadToolBarDropDown>
                    <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                    
                    <telerik:RadToolBarButton runat="server" Value="ShowAll">
                        <ItemTemplate>
                            &nbsp;&nbsp;<img src="Images/filter.png"/>&nbsp;&nbsp;<b style="color: red">Special Filter: </b> &nbsp;&nbsp;
                            
                            <img src="Images/project.png"/>Selected project:&nbsp;&nbsp;
                            <telerik:RadComboBox ID="ddlProject" runat="server" 
                                Skin="Windows7" Width="200" AutoPostBack="True" 
                                OnItemDataBound="ddlProject_ItemDataBound"
                                OnSelectedIndexChanged="ddlProject_SelectedIndexChanged"/>&nbsp;&nbsp;|&nbsp;&nbsp;
                            <b>Show</b> 
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="rcbReadOnly" Width="230px" style="max-width: 316px"
                                OnSelectedIndexChanged="ddlStatus_OnSelectedIndexChanged" AutoPostBack="True">
                                <Items>
                                    <asp:ListItem Value="1" Text="All Shutdown Report"></asp:ListItem>
                                    <%--<asp:ListItem Value="2" Text="All Latest MR" Selected="True"></asp:ListItem>--%>
                                    <%--<asp:ListItem Value="3" Text="Coming Due MR" style="background-color: yellow"></asp:ListItem>--%>
                                    <asp:ListItem Value="4" Text="Overdue Shutdown Report" style="background-color: red"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="Shutdown Report complete Workflow Process" style="background-color: greenyellow"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="Completed Shutdown Report" style="background-color: greenyellow"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="Incompleted Shutdown Report" style="background-color: coral"></asp:ListItem>
                                </Items>
                            </asp:DropDownList>

                            &nbsp;&nbsp; | &nbsp;&nbsp;
                            <b>Search All Text Field</b>
                            <asp:TextBox ID="txtSearchAllField" runat="server" Style="width: 300px;" CssClass="min25Percent"/>
                            <telerik:RadButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Width="25px" style="text-align: center">
                                <Icon PrimaryIconUrl="~/Images/search.png" PrimaryIconLeft="3" PrimaryIconTop="2" PrimaryIconWidth="16" PrimaryIconHeight="16"/>
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:RadToolBarButton>
                </Items>
            </telerik:RadToolBar>
        </telerik:RadPane>
        
        <telerik:RadPane ID="RadPane1" runat="server" Height="32px" Scrollable="false" Scrolling="None">
            <div style="padding-top: 5px;">
                <table>
                    <tr >
                        <td valign="middle"><b style="color: red; padding-left: 10px">Note: </b>&nbsp;&nbsp;</td>
                        <td valign="middle"><asp:Image runat="server" ImageUrl="~/Images/ball_createnew.png" style="padding-top: 2px"/></td>
                        <td valign="middle">New Shutdown Report.&nbsp;&nbsp; | &nbsp;&nbsp;</td>
                        <td valign="middle"><asp:Image runat="server" ImageUrl="~/Images/ball_inwf1.png"/></td>
                        <td valign="middle"> SR being in workflow process.&nbsp;&nbsp; | &nbsp;&nbsp;</td>
                        <td valign="middle"><asp:Image runat="server" ImageUrl="~/Images/ball_completewf.png"/></td>
                        <td valign="middle"> SR completed workflow process.&nbsp;&nbsp; | &nbsp;&nbsp;</td>
                        <td valign="middle"><asp:Image runat="server" ImageUrl="~/Images/ball_completefinal.png"/></td>
                        <td valign="middle"> SR completed attach SR form.&nbsp;&nbsp; | &nbsp;&nbsp;</td>
                        <td valign="middle"><asp:Image runat="server" ImageUrl="~/Images/ball_cancel.png"/></td>
                        <td valign="middle"> Canceled Shutdown Report.</td>
                    </tr>
                </table>
            </div>
        </telerik:RadPane>

        <telerik:RadPane ID="RadPane2" runat="server" Scrollable="false" Scrolling="None">
            <telerik:RadSplitter ID="Radsplitter3" runat="server" Orientation="Horizontal">
                <telerik:RadPane ID="Radpane4" runat="server" Scrolling="None" >
                    <telerik:RadSplitter ID="Radsplitter10" runat="server" Orientation="Vertical">
                        <telerik:RadPane ID="Radpane6" runat="server" Scrolling="None">
                            <telerik:RadGrid AllowCustomPaging="False" AllowPaging="True" AllowSorting="True" 
                                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                                Height="100%" ID="grdDocument"  AllowFilteringByColumn="True" AllowMultiRowSelection="False"
                                OnDeleteCommand="grdDocument_DeleteCommand" 
                                OnItemCommand="grdDocument_ItemCommand" 
                                OnItemDataBound="grdDocument_ItemDataBound" 
                                OnNeedDataSource="grdDocument_OnNeedDataSource" 
                                PageSize="100" runat="server" Style="outline: none" Width="100%">
                                <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
                                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                                <MasterTableView AllowMultiColumnSorting="false"
                                    ClientDataKeyNames="ID" DataKeyNames="ID" CommandItemDisplay="Top" 
                                    EditMode="InPlace" Font-Size="8pt">
                                    <%--<GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldAlias="Department" FieldName="DepartmentName" FormatString="{0:D}"
                                                        HeaderValueSeparator=": "></telerik:GridGroupByField>
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldName="DepartmentName" SortOrder="Ascending" ></telerik:GridGroupByField>
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>
                                        </GroupByExpressions>   --%> 
                                    <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="false"/>
                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ColumnGroups>
                                        <telerik:GridColumnGroup HeaderText="Cause clarification" Name="Cause"
                                                HeaderStyle-HorizontalAlign="Center"/>
                                        <telerik:GridColumnGroup HeaderText="Action Plan" Name="ActionPlan"
                                                HeaderStyle-HorizontalAlign="Center"/>
                                    </ColumnGroups>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Display="False" />
                                        <telerik:GridBoundColumn DataField="IsInWFProcess" UniqueName="IsInWFProcess" Display="False" />
                                        <telerik:GridBoundColumn DataField="IsWFComplete" UniqueName="IsWFComplete" Display="False" />
                                        
                                        <telerik:GridClientSelectColumn UniqueName="IsSelected" Display="False">
                                                <HeaderStyle Width="25"  />
                                                <ItemStyle HorizontalAlign="Center" Width="25"/>
                                            </telerik:GridClientSelectColumn>

                                        <telerik:GridTemplateColumn HeaderText="No." Groupable="False" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSoTT" runat="server" Text='<%# grdDocument.CurrentPageIndex * grdDocument.PageSize + grdDocument.Items.Count+1 %>'>
                                                </asp:Label>
                                      
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="ObjectStatus">
                                            <HeaderStyle Width="30"  />
                                            <ItemStyle HorizontalAlign="Center"/>
                                            <ItemTemplate>
                                                
                                                <asp:Image ID="imgDocStatus" runat="server" 
                                                    ImageUrl='<%# Convert.ToBoolean(Eval("IsCompleteFinal"))
                                                                ? "~/Images/ball_completefinal.png"
                                                                : (Convert.ToBoolean(Eval("IsWFComplete")) 
                                                                    ? "~/Images/ball_completewf.png" 
                                                                    : (Convert.ToBoolean(Eval("IsInWFProcess")) 
                                                                        ? "~/Images/ball_inwf1.png" 
                                                                        : (Convert.ToBoolean(Eval("IsCancel")) ? "~/Images/ball_cancel.png" : "~/Images/ball_createnew.png")))%>' Style="cursor: pointer;" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                    <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" HeaderTooltip="Delete document"
                                            ConfirmText="Do you want to delete document?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png" Display="false">
                                            <HeaderStyle Width="25" />
                                            <ItemStyle HorizontalAlign="Center" Width="25"  />
                                        </telerik:GridButtonColumn>
                                            
                                    <telerik:GridBoundColumn DataField="Code" HeaderText="Code" UniqueName="Code" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridBoundColumn>
                                            
                                    <telerik:GridDateTimeColumn UniqueName="DateOfShutdown" HeaderText="Date of shutdown" DataField="DateOfShutdown"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>
                                            
                                    <telerik:GridDateTimeColumn UniqueName="TimeOfShutdown" HeaderText="Time of Shutdown" DataField="TimeOfShutdown"
                                            DataFormatString="{0:HH:mm}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>
                                            
                                    <telerik:GridDateTimeColumn UniqueName="DateResume" HeaderText="Date Resumed" DataField="DateResume"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>
                                            
                                    <telerik:GridDateTimeColumn UniqueName="TimeResume" HeaderText="Time Resumed" DataField="TimeResume"
                                            DataFormatString="{0:HH:mm}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>
                                            
                                    <telerik:GridTemplateColumn HeaderText="Downtime (hour)" UniqueName="DownTime" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" DataField="DownTime">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Right"  />
                                        <ItemTemplate>
                                            <%# Eval("DownTime", "{0: ###,##0.##}")%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                            
                                    <telerik:GridTemplateColumn HeaderText="Estimated Production loss (Bbls)" UniqueName="EstimatedProduction" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" DataField="EstimatedProduction">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Right"  />
                                        <ItemTemplate>
                                            <%# Eval("EstimatedProduction", "{0: ###,##0.##}")%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridBoundColumn DataField="CauseShutdown" HeaderText="Cause Of Shutdown" UniqueName="CauseShutdown" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                                        <ItemStyle HorizontalAlign="Left"/>
                                    </telerik:GridBoundColumn>
                                            
                                    <telerik:GridBoundColumn DataField="CauseClarificationProcess" HeaderText="Process" UniqueName="CauseClarificationProcess" ColumnGroupName="Cause" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridBoundColumn>
                                            
                                    <telerik:GridBoundColumn DataField="CauseClarificationPowerloss" HeaderText="Power loss" UniqueName="CauseClarificationPowerloss" ColumnGroupName="Cause" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridBoundColumn>
                                            
                                    <telerik:GridBoundColumn DataField="CauseClarificationFireGas" HeaderText="Fire & Gas" UniqueName="CauseClarificationFireGas" ColumnGroupName="Cause" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridBoundColumn>

                                    <telerik:GridTemplateColumn  DataField="RootCause" HeaderText="Root cause analysis" UniqueName="RootCause" 
                                        ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="300" />
                                        <ItemStyle HorizontalAlign="Left"  />
                                        <ItemTemplate>
                                            <%# Eval("RootCause").ToString().Replace(Environment.NewLine, "<br/>") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                            
                                    <telerik:GridTemplateColumn  DataField="AreaConcern" HeaderText="Area concern" UniqueName="AreaConcern" 
                                        ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="300" />
                                        <ItemStyle HorizontalAlign="Left"  />
                                        <ItemTemplate>
                                            <%# Eval("AreaConcern").ToString().Replace(Environment.NewLine, "<br/>") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                            
                                    <telerik:GridTemplateColumn  DataField="WayForward" HeaderText="Way forward" UniqueName="WayForward" 
                                        ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="300" />
                                        <ItemStyle HorizontalAlign="Left"  />
                                        <ItemTemplate>
                                            <%# Eval("WayForward").ToString().Replace(Environment.NewLine, "<br/>") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                            
                                    <telerik:GridTemplateColumn  DataField="PICName" HeaderText="PIC" UniqueName="PICName" 
                                        ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                                        <ItemStyle HorizontalAlign="Left"  />
                                        <ItemTemplate>
                                            <%# Eval("PICName").ToString().Replace(Environment.NewLine, "<br/>") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridDateTimeColumn UniqueName="Deadline" HeaderText="Deadline" DataField="Deadline"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>
                                            
                                        <telerik:GridTemplateColumn  DataField="Status" HeaderText="Status" UniqueName="Status"
                                        ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                                        <ItemStyle HorizontalAlign="Left"  />
                                        <ItemTemplate>
                                            <%# Eval("Status").ToString().Replace(Environment.NewLine, "<br/>") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn  DataField="Lesson" HeaderText="Lesson & leart" UniqueName="Lesson" 
                                        ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="300" />
                                        <ItemStyle HorizontalAlign="Left"  />
                                        <ItemTemplate>
                                            <%# Eval("Lesson").ToString().Replace(Environment.NewLine, "<br/>") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Current Workflow Step" UniqueName="CurrentWorkflowStepName"
                                        DataField="CurrentWorkflowStepName" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Left"  />
                                        <ItemTemplate>
                                            <%# Eval("CurrentWorkflowStepName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Assign To User" UniqueName="CurrentAssignUserName"
                                        DataField="CurrentAssignUserName" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Left"  />
                                        <ItemTemplate>
                                            <%# Eval("CurrentAssignUserName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                    <telerik:GridTemplateColumn HeaderText="Final Assign Dept." UniqueName="FinalAssignDeptName"
                                        DataField="FinalAssignDeptName" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# Eval("FinalAssignDeptName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
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
            <telerik:RadMenuItem Text="Attach Workflow" ImageUrl="~/Images/attachwf.png" Value="AttachWF"/>
            <telerik:RadMenuItem Text="Workflow Process History" ImageUrl="~/Images/history.png" Value="WFHistory"/>
            <telerik:RadMenuItem IsSeparator="True"/>
            <telerik:RadMenuItem Text="Update Shutdown Report Info" ImageUrl="~/Images/edit.png" Value="EditProperties" DisabledImageUrl="~/Images/dis_edit.png"/>
            
            <telerik:RadMenuItem Text="Shutdown Report Detail" ImageUrl="~/Images/detail.png" Value="MRDetail" DisabledImageUrl="~/Images/dis_detail.png"/>
            <telerik:RadMenuItem IsSeparator="True"/>
            <telerik:RadMenuItem Text="Attach document" ImageUrl="~/Images/attach.png" Value="AttachDocument" DisabledImageUrl="~/Images/dis_attach.png"/>
            <telerik:RadMenuItem Text="Attach comment/response" ImageUrl="~/Images/comment.png" Value="AttachComment1" Visible="False"/>
            <telerik:RadMenuItem IsSeparator="True"/>
            <telerik:RadMenuItem Text="Cancel Shutdown Report" ImageUrl="~/Images/cancel1.png" Value="CancelShutdownReport"/>

        </Items>
    </telerik:RadContextMenu>

    <span style="display: none">
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rtvDiscipline">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="CustomerMenu" />
                        <telerik:AjaxUpdatedControl ControlID="IsFullPermission" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ckbShowAll">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="radMenu">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContainer" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="rtvDiscipline" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="CustomerMenu" LoadingPanelID="RadAjaxLoadingPanel2"/>
                      
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlProject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChildProject" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="rtvDiscipline" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="CustomerMenu"/>
                        <telerik:AjaxUpdatedControl ControlID="lblProjectId"/>

                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlProject1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="CustomerMenu"/>
                        <telerik:AjaxUpdatedControl ControlID="lblProjectId"/>

                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="ChildProject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvDiscipline" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="CustomerMenu"/>
                    </UpdatedControls>

                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ckbShowAll">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="CustomerMenu">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
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
    <%--OnClientClose="refreshGrid"--%>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="ShutdownReport Information"
                VisibleStatusbar="false" Height="700" Width="650" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ShutdownReportDetailDialog" runat="server" Title="ShutdownReport Details"
                VisibleStatusbar="false" Height="700" Width="650" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="CommentWnd" runat="server" 
                VisibleStatusbar="false" Height="710" Width="800" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ResponseWnd" runat="server" 
                VisibleStatusbar="false" Height="560" Width="800" 
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
                VisibleStatusbar="false" Height="500" Width="730"  
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachMulti" runat="server" Title="Attach multi document files"
                VisibleStatusbar="false" Height="500" Width="700" MinHeight="500" MinWidth="700" MaxHeight="500" MaxWidth="700" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" OnClientClose="refreshGrid">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachComment" runat="server" Title="Comment Sheets"
                VisibleStatusbar="false" Height="500" Width="900" MinHeight="500" MinWidth="900" MaxHeight="500" MaxWidth="900" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachResponse" runat="server" Title="Response Sheets"
                VisibleStatusbar="false" Height="500" Width="900" MinHeight="500" MinWidth="900" MaxHeight="500" MaxWidth="900" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ImportData" runat="server" Title="Import master list"
                VisibleStatusbar="false" Height="400" Width="520" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" OnClientClose="refreshGrid">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ImportEMDRReport" runat="server" Title="Import CMDR Data File"
                VisibleStatusbar="false" Height="400" Width="520" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" OnClientClose="refreshGrid">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ShareDocument" runat="server" Title="Share documents"
                VisibleStatusbar="false" Height="600" Width="520" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachWorkflow" runat="server" Title="Attach ShutdownReport to Workflow"
                VisibleStatusbar="False" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="WFProcessHistory" runat="server" Title="Workflow Process History"
                VisibleStatusbar="False" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
    <asp:HiddenField runat="server" ID="FolderContextMenuAction"/>
    <asp:HiddenField runat="server" ID="lblDisciplineId"/>
    <asp:HiddenField runat="server" ID="lblFolderId"/>
    <asp:HiddenField runat="server" ID="lblDocId"/>
    <asp:HiddenField runat="server" ID="lblCategoryId"/>
    <asp:HiddenField runat="server" ID="IsFullPermission"/>
    <asp:HiddenField runat="server" ID="lblProjectId"/>
    <asp:HiddenField runat="server" ID="IsInWF"/>
    <asp:HiddenField runat="server" ID="IsWFComplete"/>
    
    <asp:HiddenField runat="server" ID="IsView"/>
    <asp:HiddenField runat="server" ID="IsCreate"/>
    <asp:HiddenField runat="server" ID="IsUpdate"/>
    <asp:HiddenField runat="server" ID="IsCancel"/>
    <asp:HiddenField runat="server" ID="IsAttachWF"/>
    <input type="hidden" id="radGridClickedRowIndex" name="radGridClickedRowIndex"/>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="Scripts/jquery-1.7.1.js"></script>
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
                    $find('<%=grdDocument.ClientID %>').get_masterTableView().showFilterItem();
                } else {
                    $find('<%=grdDocument.ClientID %>').get_masterTableView().hideFilterItem();
                }
            }

            function refreshGrid() {
                var masterTable = $find("<%=grdDocument.ClientID%>").get_masterTableView();
                masterTable.rebind();
            }


            function ExportGrid() {
                var masterTable = $find("<%=grdDocument.ClientID %>").get_masterTableView();
                masterTable.exportToExcel('PIN_list.xls');
                return false;
            }

            function GetGridObject(sender, eventArgs) {
                radDocuments = sender;
            }

            function onRequestStart(sender, args)
            {
                 ////alert(args.get_eventTarget());
                 if (args.get_eventTarget().indexOf("ExportTo") >= 0 || args.get_eventTarget().indexOf("btnDownloadPackage") >= 0)
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
                document.getElementById("<%= lblDocId.ClientID %>").value = Id;
            }

            function gridMenuClicking(sender, args) {
                var itemValue = args.get_item().get_value();
                var docId = document.getElementById("<%= lblDocId.ClientID %>").value;
                var isFullPermission = document.getElementById("<%= IsFullPermission.ClientID %>").value;

                var packageId = document.getElementById("<%= lblDisciplineId.ClientID %>").value;
                
                if (itemValue.indexOf("Comment") > -1)
                {
                    var contractorId = itemValue.split("_")[1];
                    var owd = $find("<%=CommentWnd.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/WMS/CommentForm.aspx?docId=" + docId + "&contId=" + contractorId, "CommentWnd");
                    
                } else if (itemValue.indexOf("Response") > -1) {
                    var contractorId = itemValue.split("_")[1];
                    var owd = $find("<%=ResponseWnd.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/WMS/ResponseForm.aspx?docId=" + docId + "&contId=" + contractorId, "ResponseWnd");

                }
                else
                {
                    switch (itemValue) {
                    case "RevisionHistory":
                        var categoryId = document.getElementById("<%= lblCategoryId.ClientID %>").value;
                        var owd = $find("<%=RevisionDialog.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/WMS/DocumentPackageRevisionHistory.aspx?docId=" + docId + "&categoryId=" + categoryId, "RevisionDialog");
                        break;
                    case "MRDetail":
                        var categoryId = document.getElementById("<%= lblCategoryId.ClientID %>").value;
                        var owd = $find("<%=ShutdownReportDetailDialog.ClientID %>");
                        owd.setSize(850, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/WMS/TrackingShutdownReportDetail.aspx?objId=" + docId, "ShutdownReportDetailDialog");
                        break;
                        case "AttachWF":
                        var projId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                        var owd = $find("<%=AttachWorkflow.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Workflow/AttachWorkflow.aspx?objId=" + docId + "&objType=ShutdownReport" + "&projId=" + projId, "AttachWorkflow");
                        break;
                    case "WFHistory":
                        var owd = $find("<%=WFProcessHistory.ClientID %>");
                        owd.Show();
                        owd.maximize();
                        owd.setUrl("Controls/Workflow/WorkflowProcessHistory.aspx?objId=" + docId + "&objType=ShutdownReport", "WFProcessHistory");
                        break;
                    case "AttachDocument":
                        var owd = $find("<%=AttachDoc.ClientID %>");
                        owd.setSize(730, document.documentElement.offsetHeight);
                        owd.Show();
                        
                        owd.setUrl("Controls/WMS/TrackingShutdownReportAttach.aspx?objId=" + docId, "AttachDoc");
                        break;
                    case "CMS":
                        var owd = $find("<%=AttachComment.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/WMS/AttachComment.aspx?docId=" + docId + "&taskId=-1", "AttachComment");
                        break;

                        case "RES":
                        var owd = $find("<%=AttachResponse.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/WMS/AttachResponse.aspx?docId=" + docId + "&taskId=-1", "AttachResponse");
                        break;
                        case "EditProperties":
                            var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                            var owd = $find("<%=CustomerDialog.ClientID %>");
                            owd.setSize(850, document.documentElement.offsetHeight);
                            owd.Show();
                            owd.setUrl("Controls/WMS/TrackingShutdownReportEditForm.aspx?objId=" + docId + "&projId=" + projectId, "CustomerDialog");
                        break;
                    case "Transmittals":
                        var owd = $find("<%=TransmittalList.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/WMS/TransmittalListByDoc.aspx?docId=" + docId, "TransmittalList");
                        break;
                    case "DeleteRev":
                        if (confirm("Do you want to reduction version of document?") == false) return;
                        ajaxManager.ajaxRequest("DeleteRev_" + docId);
                        break;
                    case "CommentResponse":
                        var owd = $find("<%=TransmittalList.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/WMS/TransmittalListByDoc.aspx?docId=" + docId, "TransmittalList");
                        break;
                        case "CancelShutdownReport":
                            if (confirm("Do you want to cancel Shutdown Report?") === false) return;
                            ajaxManager.ajaxRequest("Cancel_" + docId);
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
                var isInWF = document.getElementById("<%= IsInWF.ClientID %>").value;
                var isWFComplete = document.getElementById("<%= IsWFComplete.ClientID %>").value;

                var isUpdate = document.getElementById("<%= IsUpdate.ClientID %>").value;
                var isCancel = document.getElementById("<%= IsCancel.ClientID %>").value;
                var isAttachWF = document.getElementById("<%= IsAttachWF.ClientID %>").value;



                if (isInWF == "True" || isWFComplete == "True") {
                    menu.get_allItems()[1].enable();

                } else {
                    menu.get_allItems()[1].disable();

                }

                // Case obj in WF Process
                if (isInWF != "True") {
                    menu.get_allItems()[0].enable();
                    menu.get_allItems()[3].enable();

                    if (isAttachWF != "True") {
                        menu.get_allItems()[0].disable();
                    }

                    if (isUpdate != "True") {
                        menu.get_allItems()[3].disable();
                    }
                }
                else {
                    menu.get_allItems()[0].disable();
                    menu.get_allItems()[3].disable();
                }
                // ---------------------------------------------

                if (isCancel != "True") {
                    menu.get_allItems()[8].disable();
                } else {
                    menu.get_allItems()[8].enable();

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

                toolbar = $find("<%= CustomerMenu.ClientID %>");
                ajaxManager = $find("<%=ajaxCustomer.ClientID %>");
            }

            function ShowEditForm(docId, projId) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.setSize(850, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("Controls/WMS/TrackingShutdownReportEditForm.aspx?projId=" + projId + "&objId=" + docId, "CustomerDialog");
            }
            
            function ShowUploadForm(id) {
                var owd = $find("<%=AttachDoc.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/WMS/UploadDragDrop.aspx?docId=" + id, "AttachDoc");
            }
            
            function ShowRevisionForm(id) {
                var owd = $find("<%=RevisionDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/WMS/RevisionHistory.aspx?docId=" + id, "RevisionDialog");
            }

            function refreshGrid(arg) {
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
                //}Send notifications window.open('file://Shared Folder/Users')

                if (strText.toLowerCase() == "send notifications") {
                    var grid = $find("<%=grdDocument.ClientID %>");
                    var masterTable = grid.get_masterTableView();
                    var number = 0;
                    for (var i = 0; i < masterTable.get_dataItems().length; i++) {
                        var gridItemElement = masterTable.get_dataItems()[i].findElement("IsSelected");
                        if (gridItemElement.checked) {
                            number++;
                        }
                    }
                    if (number == 0) {
                        alert("Please select documents to send notification");
                    }
                    else {
                        ajaxManager.ajaxRequest("SendNotification");
                    }
                }
                
                if (strText.toLowerCase() == "import master document register") {
                    var owd = $find("<%=ImportData.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/WMS/ImportDocMasterList.aspx", "ImportData");
                }

                if (strText.toLowerCase() == "import cmdr data file") {
                    var owd = $find("<%=ImportEMDRReport.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/WMS/ImportEMDR.aspx", "ImportEMDRReport");
                }
                
                if (strText.toLowerCase() == "send mail") {
                    var grid = $find("<%=grdDocument.ClientID %>");
                    var masterTable = grid.get_masterTableView();
                    var listId = "";
                    var selectedRows = masterTable.get_selectedItems();
                    if (selectedRows.length == 0) {
                        alert("Please select documents to send mail");
                    } else {
                        for (var i = 0; i < selectedRows.length; i++) {
                            var row = selectedRows[i];
                            //alert(row.getDataKeyValue("ID"));
                            listId += row.getDataKeyValue("ID") + ",";
                        }
                        var owd = $find("<%=SendMail.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/WMS/SendMail.aspx?listDoc=" + listId, "SendMail");
                    }
                }

                if (strText.toLowerCase() == "share documents") {
                    var grid = $find("<%=grdDocument.ClientID %>");
                    var masterTable = grid.get_masterTableView();
                    
                    var listId = "";
                    
                    var selectedRows = masterTable.get_selectedItems();
                    if (selectedRows.length == 0) {
                        alert("Please select documents to share for Document library.");
                    }
                    else {
                        for (var i = 0; i < selectedRows.length; i++) {
                            var row = selectedRows[i];
                            //alert(row.getDataKeyValue("ID"));
                            listId += row.getDataKeyValue("ID") + ",";
                        }
                        var owd = $find("<%=ShareDocument.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/WMS/ShareDocument.aspx?listDoc=" + listId, "ShareDocument");
                    }
                }

                if (strText.toLowerCase() == "delete all selected documents") {
                    var grid = $find("<%=grdDocument.ClientID %>");
                    var masterTable = grid.get_masterTableView();

                    var listId = "";

                    var selectedRows = masterTable.get_selectedItems();
                    if (selectedRows.length == 0) {
                        alert("Please select documents to delete.");
                    }
                    else {
                        if (confirm("Do you want to delete all selected documents?") == false) return;

                        for (var i = 0; i < selectedRows.length; i++) {
                            var row = selectedRows[i];
                            //alert(row.getDataKeyValue("ID"));
                            listId += row.getDataKeyValue("ID") + ",";
                        }

                        ajaxManager.ajaxRequest("DeleteAllDoc");
                    }
                }
                
                if (strText.toLowerCase() == "download multi documents") {
                    var grid = $find("<%=grdDocument.ClientID %>");
                    var masterTable = grid.get_masterTableView();
                    var number = 0;
                    var listId = "";
                    for (var i = 0; i < masterTable.get_dataItems().length; i++) {
                        var gridItemElement = masterTable.get_dataItems()[i].findElement("IsSelected");
                        if (gridItemElement.checked) {
                            number++;
                            listId += masterTable.get_dataItems()[i].getDataKeyValue("ID") + ",";
                        }
                    }
                    if (number == 0) {
                        alert("Please select documents to download");
                    }
                    else {
                        ajaxManager.ajaxRequest("DownloadMulti");
                    }
                }
                
                if (strText.toLowerCase() == "clear emdr data") {
                    ajaxManager.ajaxRequest("ClearEMDRData");
                }

                if (strText.toLowerCase() == "export master document register template") {
                    ajaxManager.ajaxRequest("ExportMasterList");
                }

                if (strText.toLowerCase() == "export cmdr data file") {
                    ajaxManager.ajaxRequest("ExportEMDRReport_New");
                }

                if (strText.toLowerCase() == "attach multi document file") {
                    var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                    var owd = $find("<%=AttachMulti.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/WMS/UploadMultiDocumentFile.aspx?projectId=" + projectId, "AttachMulti");
                }
                
                if (strText.toLowerCase() == "update") {
                    ajaxManager.ajaxRequest("UpdatePackageStatus");
                }

                //Download multi documents Export template data file Attach multi document file
                ////if (strText == "View explorer") {
                ////    window.open("file://WIN-P7KS57HL1HG/DocumentLibrary");
                ////}

                if (strText == "Shutdown Report") {
                    
                    <%--var selectedDiscipline = document.getElementById("<%= lblDisciplineId.ClientID %>").value;
                    if (selectedDiscipline == "") {
                        alert("Please choice one Discipline to add new Document.");
                        return false;
                    }
                    else {--%>
                    var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                    var owd = $find("<%=CustomerDialog.ClientID %>");
                    owd.setSize(850, document.documentElement.offsetHeight);
                    owd.Show();
                    owd.setUrl("Controls/WMS/TrackingShutdownReportEditForm.aspx?projId=" + projectId, "CustomerDialog");
                    //}
                }
                
                if (strText.toLowerCase() == "multi documents") {
                    var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                    if (selectedFolder == "") {
                        alert("Please choice one folder to create new document.");
                        return false;
                    }

                    var owd = $find("<%=UploadMulti.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/WMS/UploadDragDrop.aspx?folId=" + selectedFolder, "UploadMulti");
                }
                
                if (strText.toLowerCase() == "export excel") {
                    var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                    if (selectedFolder == "") {
                        alert("Please choice one folder to export master list.");
                        return false;
                    }
                    else {
                        ajaxManager.ajaxRequest("ExportMasterList");
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
        /* ]]> */
        </script>
    </telerik:RadCodeBlock>
</asp:Content>