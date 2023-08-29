<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ToDoListPage.aspx.cs" Inherits="EDMs.Web.ToDoListPage" EnableViewState="true" %>

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
        .GridRowOver_Telerik
         {
          background-color: darkseagreen !important;
          cursor:pointer;
         }
         .RadGrid .rgHoveredRow
        {
            background: darkseagreen !important;
        }
        #ctl00_ContentPlaceHolder2_radMenuOutgoing_i2_btnSearchOutgoing {
            margin-top: -3px
        }
        #ctl00_ContentPlaceHolder2_radMenuIncoming_i2_btnSearchIncoming {
            margin-top: -3px
        }

       #ctl00_ContentPlaceHolder2_IncomingTransView{
            height: 90% !important;
        } 
        #ctl00_ContentPlaceHolder2_OutgoingTransView {
            height: 90% !important;
        }

         #ctl00_ContentPlaceHolder2_grdIncomingTrans_GridData{
            height: 86% !important;
        }
          
         #ctl00_ContentPlaceHolder2_grdOutgoingTrans_GridData {
            height: 86% !important;
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
       <%-- <telerik:RadPane ID="RadPane3" runat="server" Height="30px" Scrollable="false" Scrolling="None">
            
            <telerik:RadToolBar ID="CustomerMenu" runat="server" Width="100%" OnClientButtonClicking="OnClientButtonClicking">
                <Items>
                    <telerik:RadToolBarDropDown runat="server" Text="Action" ImageUrl="~/Images/action.png">
                        <Buttons>
                            <telerik:RadToolBarButton runat="server" Text="Create Outgoing Transmittal" Value="3" ImageUrl="~/Images/outgoingTrans.png" />
                        </Buttons>
                    </telerik:RadToolBarDropDown>
                </Items>
            </telerik:RadToolBar>
            
        </telerik:RadPane>--%>
        <telerik:RadPane ID="RadPane2" runat="server" Scrollable="false" Scrolling="None">
            <telerik:RadSplitter ID="Radsplitter3" runat="server" Orientation="Horizontal">
                <telerik:RadPane ID="Radpane4" runat="server" Scrolling="None" Height="30">
                    <telerik:RadToolBar ID="radMenuIncoming" runat="server" Width="100%">
                        <Items>
                            <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                              <telerik:RadToolBarButton runat="server" Visible="false" Value="ShowAll">
                                <ItemTemplate>
                                     <telerik:RadButton ID="btnCompleteTask" Text="Complete Multi Task" runat="server" OnClientClicking="OnClientButtonClickingCompleteTask"  Width="150px" style="text-align: center">
                                        <Icon PrimaryIconUrl="~/Images/complete.png" PrimaryIconLeft="3" PrimaryIconTop="2" PrimaryIconWidth="16" PrimaryIconHeight="16"/>
                                    </telerik:RadButton>
                                 </ItemTemplate>
                            </telerik:RadToolBarButton>
                               <telerik:RadToolBarButton runat="server" Value="Show">
                                <ItemTemplate>
                                    &nbsp;&nbsp;
                                     <b>Show</b>  
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="rcbReadOnly" Width="200px" style="max-width: 200px"
                                        OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" AutoPostBack="True">
                                        <Items>
                                            <asp:ListItem Value="All" Text="All"/>
                                            <asp:ListItem Value="4" Text="Document Transmittal"/>
                                            <asp:ListItem Value="2" Text="Change Request"/>
                                             <asp:ListItem Value="3" Text="NCR/SI"></asp:ListItem>
                                            <asp:ListItem Value="CS" Text="CS"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="RFI" />
                                            <asp:ListItem Value="6" Text="Shipmet" />
                                        </Items>
                                    </asp:DropDownList>
                                    </ItemTemplate>
                                   </telerik:RadToolBarButton>
                                </Items>
                    </telerik:RadToolBar>
                </telerik:RadPane>
                <telerik:RadPane ID="Radpane1" runat="server" Scrolling="None">
                    <telerik:RadGrid AllowCustomPaging="False" AllowPaging="true" AllowSorting="True" 
                        AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                        Height="100%" ID="grdDocument"  AllowFilteringByColumn="True" AllowMultiRowSelection="false"
                        OnItemCommand="grdDocument_ItemCommand" 
                        OnItemDataBound="grdDocument_ItemDataBound" 
                        OnNeedDataSource="grdDocument_OnNeedDataSource" 
                        OnPreRender="grdDocument_OnPreRender"
                        PageSize="100" runat="server" Style="outline: none" Width="100%">
                        <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
                        <GroupingSettings CaseSensitive="False"></GroupingSettings>
                        <MasterTableView AllowMultiColumnSorting="false"
                            ClientDataKeyNames="ID" DataKeyNames="ID" CommandItemDisplay="Top" 
                            EditMode="InPlace" Font-Size="8pt">
                            <GroupByExpressions>
                                    <telerik:GridGroupByExpression>
                                        <SelectFields>
                                            <telerik:GridGroupByField HeaderText="" FieldName="ObjectType" FormatString="{0:D}"
                                                HeaderValueSeparator="_"></telerik:GridGroupByField>
                                        </SelectFields>
                                        <GroupByFields>
                                            <telerik:GridGroupByField FieldName="ObjectType" SortOrder="Ascending" ></telerik:GridGroupByField>
                                        </GroupByFields>
                                    </telerik:GridGroupByExpression>
                                </GroupByExpressions>    
                            <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="false"/>
                            <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    
                            <Columns>
                                <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                                <telerik:GridBoundColumn DataField="CanReject" UniqueName="CanReject" Display="False" />
                                <telerik:GridBoundColumn DataField="ObjectType" UniqueName="ObjectTypeEdit" Display="False" />
                                <telerik:GridBoundColumn DataField="ObjectTypeId" UniqueName="ObjectTypeIdEdit" Display="False" />
                                <telerik:GridBoundColumn DataField="ObjectProjectId" UniqueName="ProjectId" Display="False" />
                                <telerik:GridBoundColumn DataField="CategoryId" UniqueName="CategoryId" Display="False" />
                                <telerik:GridBoundColumn DataField="ObjectID" UniqueName="ObjectID" Display="False" /><telerik:GridBoundColumn DataField="ObjectNumber" UniqueName="ObjectNumber1" Display="False" />
                                <telerik:GridBoundColumn DataField="IsDistributeOnshore" UniqueName="IsDistributeOnshore" Display="False" />
                                <telerik:GridBoundColumn DataField="IsOnshoreComment" UniqueName="IsOnshoreComment" Display="False" />
                                <telerik:GridBoundColumn DataField="IsFinal" UniqueName="IsFinal" Display="False" />
                                <telerik:GridBoundColumn DataField="IsReassign" UniqueName="IsReassign" Display="False" />
                                <telerik:GridBoundColumn DataField="IsCanCreateOutgoingTrans" UniqueName="IsCanCreateOutgoingTrans" Display="False" />
                                <telerik:GridBoundColumn DataField="ActionTypeId" UniqueName="ActionTypeId" Display="False" />
                                <telerik:GridBoundColumn DataField="IsAddAnotherDisciplineLead" UniqueName="IsAddAnotherDisciplineLead" Display="False" />

                                <telerik:GridClientSelectColumn UniqueName="IsSelected" Display="false" >
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

                                <%--<telerik:GridTemplateColumn AllowFiltering="False" UniqueName="EditColumn" Display="False">
                                    <HeaderStyle Width="30"  />
                                    <ItemStyle HorizontalAlign="Center"/>
                                    <ItemTemplate>
                                        <a href='javascript:ShowEditForm(<%# DataBinder.Eval(Container.DataItem, "ID") %>)' style="text-decoration: none; color:blue">
                                        <asp:Image ID="EditLink" runat="server" ImageUrl="~/Images/edit.png" Style="cursor: pointer;" AlternateText="Edit properties" />
                                            <a/>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>--%>
                                        <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="WorkflowProcessHistory" >
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ShowMarkupDocList("<%# DataBinder.Eval(Container.DataItem, "ID") %>","<%# DataBinder.Eval(Container.DataItem,"ObjectTypeId") %>","<%# DataBinder.Eval(Container.DataItem,"ObjectID") %>","<%# DataBinder.Eval(Container.DataItem,"ActionTypeId") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="imgMarkupDocFile" runat="server" ImageUrl="~/Images/markup.png" Style="cursor: pointer;" ToolTip="Workflow Object Details" Visible='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ObjectTypeId")) == 2 || Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ObjectTypeId")) == 4 || Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ObjectTypeId")) == 5 || Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ObjectTypeId")) == 6  %>' />
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="ObjectStatus" HeaderText="Is Reject">
                                    <HeaderStyle Width="30"  />
                                    <ItemStyle HorizontalAlign="Center"/>
                                    <ItemTemplate>
                                                
                                        <asp:Image ID="imgDocStatus" runat="server" Visible ='<%# Eval("IsReject")%>' ImageUrl="~/Images/checkred.png" Style="cursor: pointer;" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                    
                                <telerik:GridTemplateColumn HeaderText="Action" UniqueName="ActionTypeName"
                                    AllowFiltering="false">
                                    <HeaderStyle HorizontalAlign="Center" Width="100"/>
                                    <ItemStyle HorizontalAlign="Center"  />
                                    <ItemTemplate>
                                       <%# Eval("ActionTypeName") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Object Type" UniqueName="ObjectType"
                                AllowFiltering="false" Display="False">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Left"  />
                                <ItemTemplate>
                                    <%# Eval("ObjectType") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                            
                            <telerik:GridTemplateColumn HeaderText="Workflow Name" UniqueName="WorkflowName"
                                AllowFiltering="false">
                                <HeaderStyle HorizontalAlign="Center" Width="200" />
                                <ItemStyle HorizontalAlign="Left"  />
                                <ItemTemplate>
                                    <%# Eval("WorkflowName") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                            <telerik:GridTemplateColumn HeaderText="Workflow Step" UniqueName="CurrentWorkflowStepName"
                                AllowFiltering="false">
                                <HeaderStyle HorizontalAlign="Center" Width="200" />
                                <ItemStyle HorizontalAlign="Left"  />
                                <ItemTemplate>
                                    <%# Eval("CurrentWorkflowStepName") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                            
                                <telerik:GridTemplateColumn HeaderText="Project Code" UniqueName="ObjectProject"
                                DataField="ObjectProject" ShowFilterIcon="False" FilterControlWidth="97%"  Display="false"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("ObjectProject") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                            
                            <telerik:GridTemplateColumn HeaderText="Object Number" UniqueName="ObjectNumber"
                                DataField="ObjectNumber" ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="160" />
                                <ItemStyle HorizontalAlign="Center"/>
                                <ItemTemplate>
                                    <asp:Label ID="lblDocNo" runat="server" Text='<%# Eval("ObjectNumber") %>' style="cursor: pointer"/> 
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                            
                                        
                                            
                                <telerik:GridTemplateColumn HeaderText="Object Title/Subject" UniqueName="ObjectTitle"
                                DataField="ObjectTitle" ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="300" />
                                <ItemStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%# Eval("ObjectTitle") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                            <telerik:GridTemplateColumn HeaderText="PIC" UniqueName="AssignUser"
                                DataField="UserFullName" ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                <HeaderStyle HorizontalAlign="Center" Width="200" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("UserFullName") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn> 

                            <telerik:GridTemplateColumn HeaderText="Revision" UniqueName="Revision"
                                DataField="Revision" ShowFilterIcon="False" FilterControlWidth="97%"  Display="false"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("Revision") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                    
                                <telerik:GridDateTimeColumn HeaderText="Received Date" UniqueName="ReceivedDate" DataField="ReceivedDate"
                                DataFormatString="{0:dd/MM/yyyy HH:mm}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True">
                            <HeaderStyle HorizontalAlign="Center" Width="120" />
                            <ItemStyle HorizontalAlign="Center"/>
                        </telerik:GridDateTimeColumn>  
                                    <telerik:GridDateTimeColumn HeaderText="Deadline" UniqueName="PlanCompleteDate" DataField="PlanCompleteDate"
                                DataFormatString="{0:dd/MM/yyyy HH:mm}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True">
                            <HeaderStyle HorizontalAlign="Center" Width="120" />
                            <ItemStyle HorizontalAlign="Center"/>
                        </telerik:GridDateTimeColumn>  
 
                        </Columns>
                    </MasterTableView>
                    <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True"  EnableRowHoverStyle="true">
                    <ClientEvents OnRowContextMenu="RowContextMenu" OnRowClick="RowClick"></ClientEvents>
                    <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                    </ClientSettings>
                </telerik:RadGrid>

                </telerik:RadPane>
            </telerik:RadSplitter>
                    
                    

                                
                            
                    </telerik:RadPane>

                </telerik:RadSplitter>       

           <%-- </telerik:RadPane>
            </telerik:RadSplitter>
        </telerik:RadPane>
    </telerik:RadSplitter>--%>
    <telerik:RadContextMenu ID="radMenu" runat="server"
        EnableRoundedCorners="true" EnableShadows="true" OnClientItemClicking="gridMenuClicking" OnClientShowing="gridContextMenuShowing">
        <Items>
            <telerik:RadMenuItem Text="Complete Task" ImageUrl="~/Images/complete.png" Value="Complete"/>
            <telerik:RadMenuItem Text="Dis-Agree" ImageUrl="~/Images/reject.png" Value="Reject"/>
            <telerik:RadMenuItem Text="Attach To Internal Workflow" ImageUrl="~/Images/attachwf.png" Value="AttachInternalWorkflow"/>
            <telerik:RadMenuItem IsSeparator="True"/>
            <telerik:RadMenuItem Text="Workflow Details Report" ImageUrl="~/Images/history.png" Value="WFHistory"/>
            <telerik:RadMenuItem Text="Object Properties" ImageUrl="~/Images/detail.png" Value="DocProperties"/>
            <telerik:RadMenuItem Text="Markup/CRS Files" ImageUrl="~/Images/markup.png" Value="MarkupTool"/>
            <telerik:RadMenuItem Text="CRS File" ImageUrl="~/Images/markup.png" Value="CRSFile" Visible="false"/>
            <telerik:RadMenuItem IsSeparator="True"/>
            <telerik:RadMenuItem Text="E-mail" ImageUrl="~/Images/email.png" Value="Email" Visible="false"/>
            <telerik:RadMenuItem IsSeparator="True"/>
            <telerik:RadMenuItem Text="Create Outgoing Trans" ImageUrl="~/Images/outgoingTrans.png" Value="CreateOutgoingTrans"/>
            <telerik:RadMenuItem Text="Assign To Another Discipline Lead" ImageUrl="~/Images/reassign.png" Value="Reassign" Visible="false"/>
            <telerik:RadMenuItem Text="Re-assign To Another User" ImageUrl="~/Images/reassign.png" Value="ReassignUser"/>
             <telerik:RadMenuItem Text="Download Package" ImageUrl="~/Images/download.png" Value="DownloadAll" Visible="false"/>

            
        </Items>
    </telerik:RadContextMenu>

        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="Windows7"/>
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
                <telerik:AjaxSetting AjaxControlID="btnOutgoingTrans">
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
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ChildProject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvDiscipline" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="CustomerMenu"/>
                    </UpdatedControls>

                </telerik:AjaxSetting>
                 <telerik:AjaxSetting AjaxControlID="ddlStatusg">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel1"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ckbShowAll">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID ="radMenuIncoming">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

    <%--OnClientClose="refreshGrid"--%>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Object Information"
                VisibleStatusbar="false" Height="700" Width="650" IconUrl="~/Images/detail.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="CommentWnd" runat="server" 
                VisibleStatusbar="false" Height="700" Width="900" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ResponseWnd" runat="server" 
                VisibleStatusbar="false" Height="650" Width="1000" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload"> 
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="UploadMulti" runat="server" Title="Create multiple documents"
                VisibleStatusbar="false" Height="520" MinHeight="520" MaxHeight="520" Width="640" MinWidth="640" MaxWidth="640"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RevisionDialog" runat="server" Title="Revision History"
                VisibleStatusbar="false" Height="550" Width="1250" MinHeight="550"  
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="TransmittalList" runat="server" Title="Transmittal List"
                VisibleStatusbar="false" Height="400" Width="1250" MinHeight="400" MinWidth="1250" MaxHeight="400" MaxWidth="1250" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="SendMail" runat="server" Title="Send mail" IconUrl="~/Images/email.png"
                VisibleStatusbar="false" Height="560" Width="992" MinHeight="560" MinWidth="992" MaxHeight="560" MaxWidth="992" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachDoc" runat="server" Title="Attach document files"
                VisibleStatusbar="false"  
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="MarkupControl" runat="server" Title="Attach document files"
                VisibleStatusbar="false"  IconUrl="~/Images/markup.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="CRSFile" runat="server" Title="CRS File"
                VisibleStatusbar="false"  IconUrl="~/Images/markup.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>

            <telerik:RadWindow ID="MarkupFile" runat="server" Title="Comment List"
                VisibleStatusbar="false" Height="500" Width="800" MinHeight="500" MinWidth="800" MaxHeight="500" MaxWidth="800" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachMulti" runat="server" Title="Attach multi document files"
                VisibleStatusbar="false" Height="500" Width="700" MinHeight="500" MinWidth="700" MaxHeight="500" MaxWidth="700" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachComment" runat="server" Title="Attach Comment/Response"
                VisibleStatusbar="false" Height="500" Width="900" MinHeight="500" MinWidth="900" MaxHeight="500" MaxWidth="900" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ImportData" runat="server" Title="Import master list"
                VisibleStatusbar="false" Height="400" Width="520" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ImportEMDRReport" runat="server" Title="Import EMDR Report"
                VisibleStatusbar="false" Height="400" Width="520" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ShareDocument" runat="server" Title="Share documents"
                VisibleStatusbar="false" Height="600" Width="520" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachWorkflow" runat="server" Title="Attach Document to Internal Workflow"
                VisibleStatusbar="false" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ObjDetailDialog" runat="server" Title="Object Details"
                VisibleStatusbar="False" Height="700" Width="850" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" OnClientClose="refreshGrid" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="DistributeOnshore" runat="server" Title="Distribute Onshore Comment"
                VisibleStatusbar="False" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
                
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="CompleteMoveNext" runat="server" Title="Complete Task"
                VisibleStatusbar="False" Height="450" Width="560" IconUrl="~/Images/complete.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ReassignWorkingPIC" runat="server" Title="Assign To Another Discipline Lead"
                VisibleStatusbar="False" Height="450" Width="610" IconUrl="~/Images/reassign.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ReassignUser" runat="server" Title="Re-assign To Another User"
                VisibleStatusbar="False" Height="450" Width="610" IconUrl="~/Images/reassign.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
              <telerik:RadWindow ID="AssignToAnotherUser" runat="server" Title="Add Another User To Task"
                VisibleStatusbar="False" Height="450" Width="610" IconUrl="~/Images/reassign.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RejectForm" runat="server" Title="Reject To Previous Step"
                VisibleStatusbar="False" Height="450" Width="610" IconUrl="~/Images/reject.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="WFProcessHistory" runat="server" Title="Workflow Details Report"
                VisibleStatusbar="False" Height="450" Width="610" IconUrl="~/Images/history.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="CompleteFinal" runat="server" Title="Complete Process Workflow"
                VisibleStatusbar="False" Height="450" Width="610" OnClientClose="refreshGrid"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="CreateOutgoingTrans" runat="server" Title="Create Outgoing Transmittal"
                VisibleStatusbar="False" OnClientClose="refreshGrid" IconUrl="~/Images/outgoingTrans.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="DocListOfTrans" runat="server" Title="Transmittal Attachment"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/attach.png" OnClientClose="refreshGrid"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="DocListOfChangeRequest" runat="server" Title="Change Request Attach Document Files"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/attach.png" OnClientClose="refreshGrid"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
              <telerik:RadWindow ID="RFIListDetail" runat="server" Title="Detail"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/attach.png" OnClientClose="refreshGrid"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
    <asp:HiddenField runat="server" ID="FolderContextMenuAction"/>
    <asp:HiddenField runat="server" ID="lblDisciplineId"/>
    <asp:HiddenField runat="server" ID="lblFolderId"/>
    <asp:HiddenField runat="server" ID="lblObjAssignUserId"/>
    <asp:HiddenField runat="server" ID="lblCategoryId"/>
    <asp:HiddenField runat="server" ID="IsFullPermission"/>
    <asp:HiddenField runat="server" ID="CanReject"/>
    <asp:HiddenField runat="server" ID="CanConsolidate"/>
    <asp:HiddenField runat="server" ID="lblProjectId"/>
    <asp:HiddenField runat="server" ID="ObjectType"/>
    <asp:HiddenField runat="server" ID="ObjectTypeId"/>
    <asp:HiddenField runat="server" ID="ObjectId"/>
    <asp:HiddenField runat="server" ID="ObjectNumber"/>
    <asp:HiddenField runat="server" ID="ProjectId"/>
    <asp:HiddenField runat="server" ID="IsOnshoreComment"/>
    <asp:HiddenField runat="server" ID="IsDistributeOnshore"/>
    <asp:HiddenField runat="server" ID="IsFinal"/>
    <asp:HiddenField runat="server" ID="ActionType"/>
    <asp:HiddenField runat="server" ID="IsAddAnotherLead"/>
    <asp:HiddenField runat="server" ID="IsReassign"/>
    <asp:HiddenField runat="server" ID="IsCanCreateOutgoingTrans"/>
    <asp:HiddenField runat="server" ID="CategoryId"/>

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
                ////alert(args.get_eventTarget()); || args.get_eventTarget().indexOf("ajaxCustomer") >= 0
                 if (args.get_eventTarget().indexOf("ExportTo") >= 0 || args.get_eventTarget().indexOf("btnDownloadPackage") >= 0 )
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
                document.getElementById("<%= lblObjAssignUserId.ClientID %>").value = Id;
            }
            function ShowMarkupDocList(objAssignUserId,objTypeId, objId, actionType) {
                <%-- var actionType = document.getElementById("<%= ActionType.ClientID %>").value;
               //var objTypeId = document.getElementById("<%= ObjectTypeId.ClientID %>").value;
              //  var objId = document.getElementById("<%= ObjectId.ClientID %>").value;--%>

              ///  var objAssignUserId = document.getElementById("<%= lblObjAssignUserId.ClientID %>").value;
               
                        switch (objTypeId) {
                            case "4":
                                var owd = $find("<%=DocListOfTrans.ClientID %>");
                                owd.Show();
                                owd.maximize();
                                owd.setUrl("Controls/Document/PECC2ToDoListDocOfTrans.aspx?objId=" + objId + "&actionType=" + actionType + "&currentAssignId=" + objAssignUserId, "DocListOfTrans");
                                break;
                            case "2":
                                var owd = $find("<%=DocListOfChangeRequest.ClientID %>");
                                owd.Show();
                                owd.maximize();
                                owd.setUrl("Controls/Document/PECC2ToDoListDocOfTrans.aspx?objId=" + objId + "&actionType=" + actionType + "&currentAssignId=" + objAssignUserId, "DocListOfChangeRequest");
                                break;
                            case "5":
                                var owd = $find("<%=RFIListDetail.ClientID %>");
                                owd.Show();
                                owd.maximize();
                                owd.setUrl("Controls/Document/RFIDetailToDoList.aspx?objId=" + objId + "&actionType=" + actionType + "&currentAssignId=" + objAssignUserId, "RFIListDetail");
                                break;
                             case "6":
                                var owd = $find("<%=RFIListDetail.ClientID %>");
                                owd.Show();
                                owd.maximize();
                                owd.setUrl("Controls/CostContract/ShipmentDetailList.aspx?shipmentId=" + objId + "&actionType=" + actionType + "&Fromtodolist=true&currentAssignId=" + objAssignUserId, "RFIListDetail");
                                break;
                        }
            }
            function gridMenuClicking(sender, args) {
                var itemValue = args.get_item().get_value();
                var docId = document.getElementById("<%= lblObjAssignUserId.ClientID %>").value;
                var objAssignUserId = document.getElementById("<%= lblObjAssignUserId.ClientID %>").value;
                var objId = document.getElementById("<%= ObjectId.ClientID %>").value;
                var objTypeId = document.getElementById("<%= ObjectTypeId.ClientID %>").value;
                var projectId = document.getElementById("<%= ProjectId.ClientID %>").value;
                var categoryId = document.getElementById("<%= CategoryId.ClientID %>").value;
                var isFullPermission = document.getElementById("<%= IsFullPermission.ClientID %>").value;

                
                switch (itemValue) {
                    case "CreateOutgoingTrans":
                        var owd = $find("<%=CreateOutgoingTrans.ClientID %>");
                        owd.setSize(850, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/Document/PECC2TransmittalFromToDoList.aspx?objId=" + objId + "&forSend=1", "CreateOutgoingTrans");
                        break;

                    case "RevisionHistory":
                        var categoryId = document.getElementById("<%= lblCategoryId.ClientID %>").value;
                        var owd = $find("<%=RevisionDialog.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/DocumentPackageRevisionHistory.aspx?docId=" + docId + "&categoryId=" + categoryId, "RevisionDialog");
                        break;

                    case "DownloadAll":
                        ajaxManager.ajaxRequest("DownloadAll_" + objId);
                        break;
                    case "Distribute":
                        var owd = $find("<%=DistributeOnshore.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Workflow/DistributeOnshoreComment.aspx?objAssignUserId=" + objAssignUserId , "DistributeOnshore");
                        break;
                    case "WFHistory":
                        var owd = $find("<%=WFProcessHistory.ClientID %>");
                        owd.Show();
                        owd.maximize();
                        owd.setUrl("Controls/Workflow/WorkflowProcessHistory.aspx?objId=" + objId, "WFProcessHistory");
                        break;
                    case "AttachInternalWorkflow":
                        var owd = $find("<%=AttachWorkflow.ClientID %>");
                         owd.setSize(700, document.documentElement.offsetHeight);
                         owd.Show();
                         owd.setUrl("Controls/Workflow/AttachWorkflow.aspx?objId=" + objId + "&type=2", "AttachWorkflow");
                        break;
                    case "DocProperties":
                        var owd = $find("<%=CustomerDialog.ClientID %>");
                        owd.setSize(930, document.documentElement.offsetHeight);
                        owd.Show();
                        switch (objTypeId) {
                            case "1":
                                alert(categoryId);
                                switch (categoryId) {
                                    case "1":
                                        owd.setUrl("Controls/Document/DocumentListProjectInfoEditForm_ENG.aspx?projectId=" + projectId + "&categoryId=" + categoryId + "&docId=" + objId, "CustomerDialog");
                                        break;
                                    case "2":
                                        owd.setUrl("Controls/Document/DocumentListProjectInfoEditForm_VEN.aspx?projectId=" + projectId + "&categoryId=" + categoryId + "&docId=" + objId, "CustomerDialog");
                                        break;
                                    case "3":
                                        owd.setUrl("Controls/Document/DocumentListProjectInfoEditForm_LMOM.aspx?projectId=" + projectId + "&categoryId=" + categoryId + "&docId=" + objId, "CustomerDialog");
                                        break;
                                    case "4":
                                        owd.setUrl("Controls/Document/DocumentListProjectInfoEditForm_CS.aspx?projectId=" + projectId + "&categoryId=" + categoryId + "&docId=" + objId, "CustomerDialog");
                                        break;
                                }
                                break;
                            case "2":
                                owd.setUrl("Controls/Document/ChangeRequestEditForm.aspx?projectId=" + projectId + "&objId=" + objId, "CustomerDialog");
                                break;
                            case "3":
                                var objNumber = document.getElementById("<%= ObjectNumber.ClientID %>").value;
                                if (objNumber.includes("-CS-")) {
                                    owd.setUrl("Controls/Document/CSEditForm.aspx?projectId=" + projectId + "&objId=" + objId + "&fromToDoList=true", "CustomerDialog");
                                } else {
                                    owd.setUrl("Controls/Document/NCRSIEditForm.aspx?projectId=" + projectId + "&objId=" + objId + "&fromToDoList=true", "CustomerDialog");
                                }

                                
                                break;
                            case "4":
                                owd.setUrl("Controls/Document/PECC2TransmittalEditForm.aspx?objId=" + objId, "CustomerDialog");
                                break;
                            case "6":
                                owd.setUrl("Controls/CostContract/ShipmentEditForm.aspx?ShipmentId=" + objId + "&fromToDoList=true", "CustomerDialog");
                                break;
                        } 

                        
                        break;
                    case "CRSFile":
                        var owd = $find("<%=CRSFile.ClientID %>");
                        owd.setSize(730, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/Document/PECC2DocumentCRSList.aspx?objId=" + objId + "&actionType=" + actionType, "CRSFile");
                        break;

                    case "MarkupTool":
                        var actionType = document.getElementById("<%= ActionType.ClientID %>").value;
                        
                        switch (objTypeId) {
                            case "4":
                                var owd = $find("<%=DocListOfTrans.ClientID %>");
                                owd.Show();
                                owd.maximize();
                                owd.setUrl("Controls/Document/PECC2ToDoListDocOfTrans.aspx?objId=" + objId + "&actionType=" + actionType + "&currentAssignId=" + objAssignUserId, "DocListOfTrans");
                                break;
                            case "2":
                                var owd = $find("<%=DocListOfChangeRequest.ClientID %>");
                                owd.Show();
                                owd.maximize();
                                owd.setUrl("Controls/Document/PECC2ToDoListDocOfTrans.aspx?objId=" + objId + "&actionType=" + actionType + "&currentAssignId=" + objAssignUserId, "DocListOfChangeRequest");
                                break;
                        }
                        
                        break;

                    case "MarkupList":
                        var canConsolidate = document.getElementById("<%= CanConsolidate.ClientID %>").value;
                        var objType = document.getElementById("<%= ObjectType.ClientID %>").value;
                            

                        var owd = $find("<%=ObjDetailDialog.ClientID %>");
                        switch (objType) {
                            case "Material Requisition":
                                owd.Show();
                                owd.maximize();
                                owd.setUrl("Controls/WMS/MRDetail.aspx?mrId=" + objId + "&objAssignUserId=" + objAssignUserId + "&todolist=true" + "&objType=" + objType, "ObjDetailDialog");
                            break;
                            case "Work Request":
                                owd.setSize(850, document.documentElement.offsetHeight);
                                owd.Show();
                                owd.setUrl("Controls/WMS/WRDetail.aspx?wrId=" + objId + "&objAssignUserId=" + objAssignUserId + "&todolist=true" + "&objType=" + objType, "ObjDetailDialog");
                                break;
                            case "MOC":
                                owd.setSize(850, document.documentElement.offsetHeight);
                                owd.Show();
                                owd.setUrl("Controls/WMS/TrackingMOCDetail.aspx?objId=" + objId + "&objAssignUserId=" + objAssignUserId + "&todolist=true" + "&objType=" + objType, "ObjDetailDialog");
                                break;

                            case "ECR":
                                owd.setSize(850, document.documentElement.offsetHeight);
                                owd.Show();
                                owd.setUrl("Controls/WMS/TrackingECRDetail.aspx?objId=" + objId + "&objAssignUserId=" + objAssignUserId + "&todolist=true" + "&objType=" + objType, "ObjDetailDialog");
                                break;
                            case "Shutdown Report":
                                owd.setSize(850, document.documentElement.offsetHeight);
                                owd.Show();
                                owd.setUrl("Controls/WMS/TrackingShutdownReportDetail.aspx?objId=" + objId + "&objAssignUserId=" + objAssignUserId + "&todolist=true" + "&objType=" + objType, "ObjDetailDialog");
                                break;
                            case "Breakdown Report":
                                owd.setSize(850, document.documentElement.offsetHeight);
                                owd.Show();
                                owd.setUrl("Controls/WMS/TrackingBreakdownReportDetail.aspx?objId=" + objId + "&objAssignUserId=" + objAssignUserId + "&todolist=true" + "&objType=" + objType, "ObjDetailDialog");
                                break;
                            case "Morning Call":
                                owd.setSize(850, document.documentElement.offsetHeight);
                                owd.Show();
                                owd.setUrl("Controls/WMS/TrackingMorningCallDetail.aspx?objId=" + objId + "&objAssignUserId=" + objAssignUserId + "&todolist=true" + "&objType=" + objType, "ObjDetailDialog");
                                break;

                        }
                        break;

                    case "Complete":
                        var actionType = document.getElementById("<%= ActionType.ClientID %>").value;
                        var owd = $find("<%=CompleteMoveNext.ClientID %>");
                        owd.setSize(610, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/Workflow/CompleteMoveNext.aspx?&objId=" + objId + "&currentAssignId=" + objAssignUserId + "&actionType=" + actionType, "CompleteMoveNext");
                        break;

                    case "Reject":
                        var owd = $find("<%=RejectForm.ClientID %>");
                        owd.setSize(750, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/Workflow/Reject.aspx?&objId=" + objId + "&currentAssignId=" + objAssignUserId, "RejectForm");
                        break;

                    case "Reassign":
                        var objType = document.getElementById("<%= ObjectType.ClientID %>").value;
                        var owd = $find("<%=ReassignWorkingPIC.ClientID %>");
                        owd.setSize(750, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/Workflow/ReassignWorkingPIC.aspx?objType=" + objType + "&objId=" + objId + "&currentAssignId=" + objAssignUserId, "ReassignWorkingPIC");

                        //ajaxManager.ajaxRequest("CompleteAndMove_" + objType + "_" + objId);
                        break;
                        case "ReassignUser":
                        var objType = document.getElementById("<%= ObjectType.ClientID %>").value;
                        var owd = $find("<%=ReassignUser.ClientID %>");
                        owd.setSize(750, document.documentElement.offsetHeight);
                        owd.Show();
                        //alert("BG");
                        owd.setUrl("Controls/Workflow/ReassignUser.aspx?objType=" + objType + "&objId=" + objId + "&currentAssignId=" + objAssignUserId, "ReassignUser");

                        //ajaxManager.ajaxRequest("CompleteAndMove_" + objType + "_" + objId);
                        break;

                    case "Email":
                        var objType = document.getElementById("<%= ObjectType.ClientID %>").value;

                        var owd = $find("<%=SendMail.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/WMS/SendMail.aspx?objType=" + objType + "&objId=" + objId + "&currentAssignId=" + docId, "SendMail");

                        //ajaxManager.ajaxRequest("CompleteAndMove_" + objType + "_" + objId);
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
                        var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
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
               function ShowWorkflowProcessHistory(docId) {
                var owd = $find("<%=WFProcessHistory.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.setUrl("Controls/Workflow/WorkflowProcessHistory.aspx?objId=" + docId, "WFProcessHistory");
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
                var isReassign = document.getElementById("<%= IsReassign.ClientID %>").value;
                <%--var canConsolidate = document.getElementById("<%= CanConsolidate.ClientID %>").value;
                var IsDistributeOnshore = document.getElementById("<%= IsDistributeOnshore.ClientID %>").value;
                var IsOnshoreComment = document.getElementById("<%= IsOnshoreComment.ClientID %>").value;--%>
                var IsCanCreateOutgoingTrans = document.getElementById("<%= IsCanCreateOutgoingTrans.ClientID %>").value;
                var IsAddAnotherLead = document.getElementById("<%= IsAddAnotherLead.ClientID %>").value;
                var actionType = document.getElementById("<%= ActionType.ClientID %>").value;
                var objTypeId = document.getElementById("<%= ObjectTypeId.ClientID %>").value;
                //alert(isReassign);
                    //menu.get_allItems()[10].hide();
                    //menu.get_allItems()[10].hide();
                    menu.get_allItems()[0].show();
                    menu.get_allItems()[1].show();

                if (objTypeId === "2" || objTypeId === "4" || objTypeId === "5") {
                    menu.get_allItems()[6].show();
                } else {
                    menu.get_allItems()[6].hide();

                }

                    if (IsCanCreateOutgoingTrans === "True") {
                        menu.get_allItems()[9].show();
                    } else {
                        menu.get_allItems()[9].hide();
                    }


                    if (canReject === "True") {
                        menu.get_allItems()[1].show();
                    }
                    else {
                        menu.get_allItems()[1].hide();
                    }

                    if (isReassign === "True") {
                        menu.get_allItems()[0].hide();
                    }
                    else {
                        menu.get_allItems()[0].show();
                    }

                    if (IsAddAnotherLead === "True") {
                        menu.get_allItems()[2].hide();
                    }
                    else {
                        menu.get_allItems()[2].hide();
                    }

                    //if (actionType === "3" || actionType === "4") {
                    //    menu.get_allItems()[7].show();
                    //} else {
                    //    menu.get_allItems()[7].hide();
                    //}
                    //if (objTypeId != "4") {
                    //    menu.get_allItems()[15].hide();
                    //}
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

            function ShowEditForm(docId, projId) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Document/DocumentPackageInfoEditForm.aspx?projId=" + projId + "&docId=" + docId, "CustomerDialog");
            }

            
            function ShowAttachment(id, objType) {
                
                switch (objType) {
                    case 1:
                        var owd = $find("<%=AttachDoc.ClientID %>");
                        owd.setSize(700, document.documentElement.offsetHeight);
                        owd.Show();
                    owd.setUrl("Controls/WMS/TrackingMorningCallAttach.aspx?objId=" + id, "AttachDoc");
                    break;

                    case 2:
                        var owd = $find("<%=AttachDoc.ClientID %>");
                        owd.setSize(700, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/WMS/TrackingWCRAttach.aspx?objId=" + id, "AttachDoc");
                    break;

                    case 3:
                        var owd = $find("<%=AttachDoc.ClientID %>");
                        owd.setSize(700, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/WMS/TrackingPunchAttach.aspx?objId=" + id, "AttachDoc");
                    break;

                    case 4:
                        var owd = $find("<%=AttachDoc.ClientID %>");
                        owd.setSize(700, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/WMS/TrackingSailAttach.aspx?objId=" + id, "AttachDoc");
                    break;

                    case 5:
                        var owd = $find("<%=AttachDoc.ClientID %>");
                        owd.setSize(700, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/WMS/TrackingProcedureAttach.aspx?objId=" + id, "AttachDoc");
                    break;

                    case 6:
                        var owd = $find("<%=AttachDoc.ClientID %>");
                        owd.setSize(700, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/WMS/TrackingGeneralWorkingAttach.aspx?objId=" + id, "AttachDoc");
                    break;
                default:
                }

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
            function AssignToAnotherUser(id) {
                  
                        var owd = $find("<%= AssignToAnotherUser.ClientID %>");
                        owd.setSize(750, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/Workflow/AssignToAnotherUser.aspx?objId=" + id, "AssignToAnotherUser");
            }
            function OnClientButtonClickingCompleteTask() {
                  var grid = $find("<%= grdDocument.ClientID %>");
                var masterTable = grid.get_masterTableView();
                var selectedRows = masterTable.get_selectedItems();
                if (selectedRows.length == 0) {
                    alert("Please select task to complete.");
                }
                else {
                   
                    ajaxManager.ajaxRequest("CompleteMultiTask");
                }
            }
            function OnClientButtonClicking(sender, args) {
                //var button = args.get_item();
                //var strText = button.get_text();
                var grid = $find("<%= grdDocument.ClientID %>");
                var masterTable = grid.get_masterTableView();
                var listId = "";
                //if (strText === "Create Outgoing Transmittal") {
               
                    var selectedRows = masterTable.get_selectedItems();
                    if (selectedRows.length == 0) {
                        alert("Please select documents to create Outgoing transmittal.");
                    }
                    else {
                        
                        //if (confirm("Do you want to delete all selected documents?") == false) return;
                        for (var i = 0; i < selectedRows.length; i++) {
                            var row = selectedRows[i];
                            // alert(row.getDataKeyValue("ID"));
                            // var cell = MasterTable.getCellByColumnUniqueName(row, "ObjectID")
                            listId += row.getDataKeyValue("ID") + "_";
                        }
                      //  alert(listId);
                        var owd = $find("<%=CreateOutgoingTrans.ClientID %>");
                        owd.setSize(730, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/Document/PECC2TransmittalFromToDoList.aspx?objIds=" + listId, "CreateOutgoingTrans");
                    }
                //}
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
                document.getElementById("<%= lblObjAssignUserId.ClientID %>").value = Id;

                var cellIsCanCreateOutgoingTrans = masterTable.getCellByColumnUniqueName(row, "IsCanCreateOutgoingTrans");
                var IsCanCreateOutgoingTrans = cellIsCanCreateOutgoingTrans.innerHTML == "&nbsp;" ? false : cellIsCanCreateOutgoingTrans.innerHTML;
                document.getElementById("<%= IsCanCreateOutgoingTrans.ClientID %>").value = IsCanCreateOutgoingTrans;

                var cellCanReject = masterTable.getCellByColumnUniqueName(row, "CanReject");
                var canReject = cellCanReject.innerHTML == "&nbsp;" ? false : cellCanReject.innerHTML;
                document.getElementById("<%= CanReject.ClientID %>").value = canReject;

                var cellObjectType = masterTable.getCellByColumnUniqueName(row, "ObjectTypeEdit");
                var objectType = cellObjectType.innerHTML == "&nbsp;" ? false : cellObjectType.innerHTML;
                document.getElementById("<%= ObjectType.ClientID %>").value = objectType;

                var cellObjectTypeId = masterTable.getCellByColumnUniqueName(row, "ObjectTypeIdEdit");
                var objectTypeId = cellObjectTypeId.innerHTML == "&nbsp;" ? false : cellObjectTypeId.innerHTML;
                document.getElementById("<%= ObjectTypeId.ClientID %>").value = objectTypeId;

                var cellProjectId = masterTable.getCellByColumnUniqueName(row, "ProjectId");
                var projectId = cellProjectId.innerHTML == "&nbsp;" ? false : cellProjectId.innerHTML;
                document.getElementById("<%= ProjectId.ClientID %>").value = projectId;

                var cellCategoryId = masterTable.getCellByColumnUniqueName(row, "CategoryId");
                var CategoryId = cellCategoryId.innerHTML == "&nbsp;" ? false : cellCategoryId.innerHTML;
                document.getElementById("<%= CategoryId.ClientID %>").value = CategoryId;

                var cellObjectId = masterTable.getCellByColumnUniqueName(row, "ObjectID");
                var objectId = cellObjectId.innerHTML == "&nbsp;" ? false : cellObjectId.innerHTML;
                document.getElementById("<%= ObjectId.ClientID %>").value = objectId;

                var cellObjectNumber = masterTable.getCellByColumnUniqueName(row, "ObjectNumber1");
                var ObjectNumber = cellObjectNumber.innerHTML == "&nbsp;" ? false : cellObjectNumber.innerHTML;
                document.getElementById("<%= ObjectNumber.ClientID %>").value = ObjectNumber;

                var cellIsDistribute = masterTable.getCellByColumnUniqueName(row, "IsDistributeOnshore");
                var isDistribute = cellIsDistribute.innerHTML == "&nbsp;" ? false : cellIsDistribute.innerHTML;
                document.getElementById("<%= IsDistributeOnshore.ClientID %>").value = isDistribute;

                var cellOnshoreComment = masterTable.getCellByColumnUniqueName(row, "IsOnshoreComment");
                var onshoreComment = cellOnshoreComment.innerHTML == "&nbsp;" ? false : cellOnshoreComment.innerHTML;
                document.getElementById("<%= IsOnshoreComment.ClientID %>").value = onshoreComment;

                var cellIsFinal = masterTable.getCellByColumnUniqueName(row, "IsFinal");
                var isFinal = cellIsFinal.innerHTML == "&nbsp;" ? false : cellIsFinal.innerHTML;
                document.getElementById("<%= IsFinal.ClientID %>").value = isFinal;

                var cellIsReassign = masterTable.getCellByColumnUniqueName(row, "IsReassign");
                var isReassign = cellIsReassign.innerHTML == "&nbsp;" ? false : cellIsReassign.innerHTML;
                document.getElementById("<%= IsReassign.ClientID %>").value = isReassign;

                var cellAction = masterTable.getCellByColumnUniqueName(row, "ActionTypeId");
                var actionType = cellAction.innerHTML == "&nbsp;" ? false : cellAction.innerHTML;
                document.getElementById("<%= ActionType.ClientID %>").value = actionType;

                var cellIsAddAnotherLead = masterTable.getCellByColumnUniqueName(row, "IsAddAnotherDisciplineLead");
                var IsAddAnotherLead = cellIsAddAnotherLead.innerHTML == "&nbsp;" ? false : cellIsAddAnotherLead.innerHTML;
                document.getElementById("<%= IsAddAnotherLead.ClientID %>").value = IsAddAnotherLead;
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