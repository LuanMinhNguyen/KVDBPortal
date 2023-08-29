<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VariationLogList.aspx.cs" Inherits="EDMs.Web.VariationLogList" EnableViewState="true" %>

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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Horizontal">
        <telerik:RadPane ID="RadPane3" runat="server" Height="30px" Scrollable="false" Scrolling="None">
            
            <telerik:RadToolBar ID="CustomerMenu" runat="server" Width="100%" OnClientButtonClicking="OnClientButtonClicking">
                <Items>
                    <telerik:RadToolBarDropDown runat="server" Text="Add" ImageUrl="~/Images/addNew.png">
                        <Buttons>
                            <telerik:RadToolBarButton runat="server" Text="Variation Log" Value="1" ImageUrl="~/Images/index.png"></telerik:RadToolBarButton>
                           

                        </Buttons>
                    </telerik:RadToolBarDropDown>
                    <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                    
    <telerik:RadToolBarButton runat="server" Value="ShowAll">
                                <ItemTemplate>
                                    &nbsp;&nbsp;

                                     <img src="../../Images/project.png"/>Selected project:&nbsp;&nbsp;
                            <telerik:RadComboBox ID="ddlProject" runat="server" 
                                Skin="Windows7" Width="250" AutoPostBack="True" 
                                OnItemDataBound="ddlProject_ItemDataBound"
                                OnSelectedIndexChanged="ddlProject_SelectedIndexChanged"/>&nbsp;&nbsp;|&nbsp;&nbsp;
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
                            <telerik:RadGrid AllowCustomPaging="False" AllowPaging="True" AllowSorting="True" 
                                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                                Height="100%" ID="grdDocument"  AllowFilteringByColumn="True" AllowMultiRowSelection="True"
                                OnDeleteCommand="grdDocument_DeleteCommand"
                                OnNeedDataSource="grdDocument_OnNeedDataSource" 
                                PageSize="100" runat="server" Style="outline: none" Width="100%">
                                <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
                                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                                <MasterTableView AllowMultiColumnSorting="false"
                                    ClientDataKeyNames="ID" DataKeyNames="ID" CommandItemDisplay="Top" 
                                    EditMode="InPlace" Font-Size="8pt">
                                  <%--  <GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldAlias="-" FieldName="DisciplineFullName" FormatString="{0:D}"
                                                        HeaderValueSeparator=""></telerik:GridGroupByField>
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldName="DisciplineFullName" SortOrder="Ascending" ></telerik:GridGroupByField>
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>
                                        </GroupByExpressions>    --%>
                                    <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="false"/>
                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ColumnGroups>
                                        <%--<telerik:GridColumnGroup HeaderText="First Issue Info" Name="FirstIssueInfo"
                                             HeaderStyle-HorizontalAlign="Center"/>
                                        <telerik:GridColumnGroup HeaderText="Final Issue Info" Name="FinalIssueInfo"
                                             HeaderStyle-HorizontalAlign="Center"/>
                                        <telerik:GridColumnGroup HeaderText="Revision Received Info" Name="ReceivedInfo"
                                             HeaderStyle-HorizontalAlign="Center"/>
                                        <telerik:GridColumnGroup HeaderText="ICA REVIEW DETAILS" Name="ICAReviews"
                                             HeaderStyle-HorizontalAlign="Center"/>--%>
                                    </ColumnGroups>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Display="False" />
                                        <telerik:GridClientSelectColumn UniqueName="IsSelected">
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

                                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="EditColumn" >
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ShowEditForm("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="EditLink" runat="server" ImageUrl="~/Images/edit.png" Style="cursor: pointer;" ToolTip="Edit properties" />
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" HeaderTooltip="Delete document"
                                        ConfirmText="Do you want to delete document?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                                        <HeaderStyle Width="25" />
                                        <ItemStyle HorizontalAlign="Center" Width="25"  />
                                    </telerik:GridButtonColumn>

                                  <telerik:GridTemplateColumn HeaderText="Title" UniqueName="Title" DataField="Title"
                                         ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                                        <ItemStyle HorizontalAlign="Left" Width="200" />
                                        <ItemTemplate>
                                             <%# Eval("Title") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                            
                                    <telerik:GridTemplateColumn HeaderText="Originater" UniqueName="OriginatingOrganizationName" DataField="OriginatingOrganizationName"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="160" />
                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                        <ItemTemplate>
                                            <%# Eval("OriginatingOrganizationName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="Instruction/ Proposal" UniqueName="InstructionProposal"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="InstructionProposal"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="160" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("InstructionProposal") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        
                                        <telerik:GridTemplateColumn HeaderText="Order" UniqueName="Order"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="Order"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("Order") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="System/Equipment/Material/…" UniqueName="System"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="System"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="250" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("System") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Contract Requirements" UniqueName="ContractRequirement"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="ContractRequirement"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("ContractRequirement") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Description" UniqueName="Description"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="Description"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("Description") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Cost impact" UniqueName="CostImpact"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="CostImpact"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                             <%# Eval("CostImpact") != null && Convert.ToDouble(Eval("CostImpact")) != 0.0
                                                ? Eval("CostImpact", "{0:###,##0.## USD}")
                                                : string.Empty%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Schedule impact" UniqueName="RefDocNo"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="RefDocNo"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("scheduleImpact") %>
                                              <%# Eval("scheduleImpact") != null && Convert.ToDouble(Eval("scheduleImpact")) != 0.0
                                                ? Eval("scheduleImpact", "{0:###, Day}")
                                                : string.Empty%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        <telerik:GridDateTimeColumn HeaderText="Issued Date" UniqueName="IssuedDate" DataField="IssuedDate"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" >
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn> 
                                        <telerik:GridTemplateColumn HeaderText="Other Attachment" UniqueName="OtherAttachment" DataField="OtherAttachment"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# Eval("OtherAttachment") %>
                                        </ItemTemplate>
                                     </telerik:GridTemplateColumn> 
                                        <telerik:GridTemplateColumn HeaderText="Remarks" UniqueName="Remark" DataField="Remark"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="220" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("Remark") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                            <%--<ClientEvents OnRowContextMenu="RowContextMenu" OnRowClick="RowClick"></ClientEvents>--%>
                            <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadPane>

                </telerik:RadSplitter>       

            </telerik:RadPane>
            </telerik:RadSplitter>
        </telerik:RadPane>
    </telerik:RadSplitter>
    <telerik:RadContextMenu ID="radMenu" runat="server" Visible="false"
        EnableRoundedCorners="true" EnableShadows="true" OnClientItemClicking="gridMenuClicking">
        <Items>
            <telerik:RadMenuItem Text="Edit properties" ImageUrl="~/Images/edit.png" Value="EditProperties" Visible="false"/>
            <telerik:RadMenuItem Text="Revision History" ImageUrl="~/Images/revision.png" Value="RevisionHistory"/>
            <telerik:RadMenuItem Text="Reduced version of document" ImageUrl="~/Images/down-icon.png" Value="DeleteRev" Visible="False"/>
            <telerik:RadMenuItem IsSeparator="True"/>

            <telerik:RadMenuItem Text="Attach document" ImageUrl="~/Images/attach.png" Value="AttachDocument"/>
            <telerik:RadMenuItem Text="Attach comment/response" ImageUrl="~/Images/comment.png" Value="AttachComment1" Visible="False"/>
            
            <telerik:RadMenuItem IsSeparator="True" Visible="false"/>
            <telerik:RadMenuItem Text="Transmittals" ImageUrl="~/Images/transmittal.png" Value="Transmittals" Visible="false"/>
            <telerik:RadMenuItem IsSeparator="True" Visible="false"/>
            <telerik:RadMenuItem Text="Comment Sheets" ImageUrl="~/Images/comment.png" Value="CMS" Visible="false"/>
            <telerik:RadMenuItem Text="Response Sheets" ImageUrl="~/Images/comment1.png" Value="RES" Visible="false"/>
            <telerik:RadMenuItem IsSeparator="True" Visible="false"/>
        </Items>
    </telerik:RadContextMenu>

    <span style="display: none">
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rtvTreeNode">
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
                
                <telerik:AjaxSetting AjaxControlID="ddlCategory">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="lblCategoryId"/>
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
                        <telerik:AjaxUpdatedControl ControlID="rtvTreeNode" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="CustomerMenu" LoadingPanelID="RadAjaxLoadingPanel2"/>
                      
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlProject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChildProject" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="rtvTreeNode" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="CustomerMenu"/>
                        <telerik:AjaxUpdatedControl ControlID="lblProjectId"/>

                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ChildProject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvTreeNode" LoadingPanelID="RadAjaxLoadingPanel2"/>
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
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </span>
    <%--OnClientClose="refreshGrid"--%>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Variation Log Information"
                VisibleStatusbar="false" Height="700" Width="800" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ReviseDoc" runat="server" Title="Revise Document"
                VisibleStatusbar="false" Height="700" Width="650" IconUrl="~/Images/revise.png"
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
               VisibleStatusbar="false" Height="560" Width="900" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" >
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
                VisibleStatusbar="false" Height="500" Width="700" IconUrl="~/Images/attach.png"
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
            
            <telerik:RadWindow ID="ImportData" runat="server" Title="Import Document list"
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
            
            <telerik:RadWindow ID="AttachWorkflow" runat="server" Title="Attach Document to Workflow"
                VisibleStatusbar="False" Height="450" Width="610" IconUrl="~/Images/attachwf.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="WFProcessHistory" runat="server" Title="Workflow Process History"
                VisibleStatusbar="False" Height="450" Width="610" IconUrl="~/Images/history.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
              <telerik:RadWindow ID="EditWorkFlow" runat="server" Title="Document In Workflow"
                VisibleStatusbar="False" Height="500" Width="900" MinHeight="500" MinWidth="900" MaxHeight="500" MaxWidth="900"  IconUrl="~/Images/icon_cms.png"
                  OnClientClose="refreshGrid"
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
                 if (args.get_eventTarget().indexOf("ExportTo") >= 0 || args.get_eventTarget().indexOf("btnDownloadPackage") >= 0 ||
                     args.get_eventTarget().indexOf("ajaxCustomer") >= 0)
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
                    case "AttachDocument":
                        var owd = $find("<%=AttachDoc.ClientID %>");
                        owd.setSize(730, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/Document/DQREAttachDocument.aspx?docId=" + docId + "&isFullPermission=" + isFullPermission, "AttachDoc");
                        break;
                    case "CMS":
                        var owd = $find("<%=AttachComment.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/AttachComment.aspx?docId=" + docId + "&taskId=-1", "AttachComment");
                        break;

                        case "RES":
                        var owd = $find("<%=AttachResponse.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/AttachResponse.aspx?docId=" + docId + "&taskId=-1", "AttachResponse");
                        break;
                        case "EditProperties":
                            var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                            var owd = $find("<%=CustomerDialog.ClientID %>");
                            owd.Show();
                            owd.setUrl("Controls/Document/DocumentListProjectInfoEditForm.aspx?docId=" + docId, "CustomerDialog");
                        break;
                    case "Transmittals":
                        var owd = $find("<%=TransmittalList.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/TransmittalListByDoc.aspx?docId=" + docId, "TransmittalList");
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
                var isfullpermission = document.getElementById("<%= IsFullPermission.ClientID %>").value;
                if (isfullpermission == "true") {
                    menu.get_allItems()[0].enable();
                    menu.get_allItems()[1].enable();
                    menu.get_allItems()[3].enable();
                    menu.get_allItems()[4].enable();
                    menu.get_allItems()[6].enable();
                }
                else {
                    menu.get_allItems()[0].disable();
                    menu.get_allItems()[1].disable();
                    menu.get_allItems()[3].enable();
                    menu.get_allItems()[4].enable();
                    menu.get_allItems()[6].enable();
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

            function ShowEditForm(objId) {
                var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                var window = $find("<%=CustomerDialog.ClientID %>");
                window.setSize(900, document.documentElement.offsetHeight);
                window.Show();
                window.setUrl("Controls/Document/VariationLogEditForm.aspx?projectId=" + projectId + "&objId=" + objId, "CustomerDialog");

                
            }

            function ReviseDocument(docId) {
                <%--var owd = $find("<%=ReviseDoc.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("Controls/Document/DocumentListProjectInfoEditForm.aspx?docId=" + docId + "&Revise=True", "ReviseDoc");--%>

                var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                var categoryId = document.getElementById("<%= lblCategoryId.ClientID %>").value;
                var window = $find("<%=ReviseDoc.ClientID %>");
                //alert(projectId);

                switch (categoryId) {
                    case "1":
                        window.setSize(730, document.documentElement.offsetHeight);
                        window.Show();
                        window.setUrl("Controls/Document/DocumentListProjectInfoEditForm_ENG.aspx?projectId=" + projectId + "&categoryId=" + categoryId + "&docId=" + docId + "&Revise=True", "CustomerDialog");
                        break;
                    case "2":
                        window.setSize(730, document.documentElement.offsetHeight);
                        window.Show();
                        window.setUrl("Controls/Document/DocumentListProjectInfoEditForm_VEN.aspx?projectId=" + projectId + "&categoryId=" + categoryId + "&docId=" + docId + "&Revise=True", "CustomerDialog");
                        break;
                    case "3":
                        window.setSize(730, document.documentElement.offsetHeight);
                        window.Show();
                        window.setUrl("Controls/Document/DocumentListProjectInfoEditForm_LMOM.aspx?projectId=" + projectId + "&categoryId=" + categoryId + "&docId=" + docId + "&Revise=True", "CustomerDialog");
                        break;
                    case "4":
                        window.setSize(730, document.documentElement.offsetHeight);
                        window.Show();
                        window.setUrl("Controls/Document/DocumentListProjectInfoEditForm_CS.aspx?projectId=" + projectId + "&categoryId=" + categoryId + "&docId=" + docId + "&Revise=True", "CustomerDialog");
                        break;
                }
            }
            

            function ShowDocumentRevision(docId) {
                var categoryId = document.getElementById("<%= lblCategoryId.ClientID %>").value;
                var owd = $find("<%=RevisionDialog.ClientID %>");
                owd.Show();
                owd.maximize(true);
                owd.setUrl("Controls/Document/ProjectDocumentRevisionHistory.aspx?docId=" + docId + "&categoryId=" + categoryId, "RevisionDialog");
             }

            function ShowAttachfile(docId) {
                var owd = $find("<%=AttachDoc.ClientID %>");
                 var isFullPermission = document.getElementById("<%= IsFullPermission.ClientID %>").value;
                 owd.setSize(730, document.documentElement.offsetHeight);
                 owd.Show();
                 owd.setUrl("Controls/Document/NCRSIAttachDocument.aspx?objId=" + docId + "&isFullPermission=" + isFullPermission, "AttachDoc");
            }

            function AttachWorkflow(docId) {
                var owd = $find("<%=AttachWorkflow.ClientID %>");
                 var projId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                 owd.setSize(700, document.documentElement.offsetHeight);
                 owd.Show();
                 owd.setUrl("Controls/Workflow/AttachWorkflow.aspx?objId=" + docId + "&projId=" + projId + "&type=3" + "&attachFrom=NCRSI&customizeWfFrom=Obj", "AttachWorkflow");
            }

            function ShowWorkflowProcessHistory(docId) {
                var owd = $find("<%=WFProcessHistory.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.setUrl("Controls/Workflow/WorkflowProcessHistory.aspx?objId=" + docId, "WFProcessHistory");
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
            
            function rtvTreeNode_ClientNodeClicking(sender, args) {
                var DisciplineValue = args.get_node().get_value();
                document.getElementById("<%= lblDisciplineId.ClientID %>").value = DisciplineValue;
            }
            
            function radPbCategories_OnClientItemClicking(sender, args)
            {
                var item = args.get_item();
                var categoryId = item.get_value();
                document.getElementById("<%= lblCategoryId.ClientID %>").value = categoryId;
                
            }
            function EditObjectWorkflow(Id) {
                 var owd = $find("<%=EditWorkFlow.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/Workflow/ObjectWorkflowPage.aspx?objId="+Id, "EditWorkFlow");
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
                
                if (strText.toLowerCase() == "import project document register") {
                    var window = $find("<%=ImportData.ClientID %>");
                    window.Show();
                    window.setUrl("Controls/Document/ImportProjectDocList.aspx", "ImportData");
                }

                if (strText.toLowerCase() == "import cmdr data file") {
                    var window = $find("<%=ImportEMDRReport.ClientID %>");
                    window.Show();
                    window.setUrl("Controls/Document/ImportEMDR.aspx", "ImportEMDRReport");
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
                        var window = $find("<%=SendMail.ClientID %>");
                        window.Show();
                        window.setUrl("Controls/Document/SendMail.aspx?listDoc=" + listId, "SendMail");
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
                        var window = $find("<%=ShareDocument.ClientID %>");
                        window.Show();
                        window.setUrl("Controls/Document/ShareDocument.aspx?listDoc=" + listId, "ShareDocument");
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

                        //for (var i = 0; i < selectedRows.length; i++) {
                        //    var row = selectedRows[i];
                        //    //alert(row.getDataKeyValue("ID"));
                        //    listId += row.getDataKeyValue("ID") + ",";
                        //}

                        ajaxManager.ajaxRequest("DeleteAllDoc");
                    }
                }
                
                if (strText.toLowerCase() == "download multi documents") {
                    var grid = $find("<%=grdDocument.ClientID %>");
                    var masterTable = grid.get_masterTableView();
                    var number = 0;
                    var listId = "";
                    //for (var i = 0; i < masterTable.get_dataItems().length; i++) {
                    //    var gridItemElement = masterTable.get_dataItems()[i].findElement("IsSelected");
                    //    if (gridItemElement.checked) {
                    //        number++;
                    //        listId += masterTable.get_dataItems()[i].getDataKeyValue("ID") + ",";
                    //    }
                    //}
                    //if (number == 0) {
                    var selectedRows = masterTable.get_selectedItems();
                    if (selectedRows.length == 0) {
                        alert("Please select documents to download");
                    }
                    else {
                        ajaxManager.ajaxRequest("DownloadMulti");
                    }
                }
                
                if (strText.toLowerCase() == "clear emdr data") {
                    ajaxManager.ajaxRequest("ClearEMDRData");
                }

                if (strText.toLowerCase() == "export project document register template") {
                    ajaxManager.ajaxRequest("ExportMasterList");
                }

                if (strText.toLowerCase() == "export project data file") {
                    ajaxManager.ajaxRequest("ExportEMDRReport");
                }

                if (strText.toLowerCase() == "attach multi document file") {
                    var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                    var window = $find("<%=AttachMulti.ClientID %>");
                    window.Show();
                    window.setUrl("Controls/Document/UploadMultiDocumentFile.aspx?projectId=" + projectId, "AttachMulti");
                }
                
                if (strText.toLowerCase() == "update") {
                    ajaxManager.ajaxRequest("UpdatePackageStatus");
                }

                //Download multi documents Export template data file Attach multi document file
                ////if (strText == "View explorer") {
                ////    window.open("file://WIN-P7KS57HL1HG/DocumentLibrary");
                ////}

                if (strText === "Variation Log") {
                    var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                    var window = $find("<%=CustomerDialog.ClientID %>");
                    //alert(projectId);
                    window.setSize(900, document.documentElement.offsetHeight);
                    window.Show();
                    window.setUrl("Controls/Document/VariationLogEditForm.aspx?projectId=" + projectId, "CustomerDialog");
                    
                }
                
                if (strText.toLowerCase() == "multi documents") {
                    var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                    if (selectedFolder == "") {
                        alert("Please choice one folder to create new document.");
                        return false;
                    }

                    var window = $find("<%=UploadMulti.ClientID %>");
                    window.Show();
                    window.setUrl("Controls/Document/UploadDragDrop.aspx?folId=" + selectedFolder, "UploadMulti");
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
</asp:Content>