<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PerformanceReview.aspx.cs" Inherits="EDMs.Web.PerformanceReview" EnableViewState="true" %>

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

        #ctl00_ContentPlaceHolder2_grdDocument_GridData {
            height: 100% !important;
        }
        #ctl00_ContentPlaceHolder2_grdTracking_GridData {
            height: 100% !important;
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
    </style>
    <div style="width: 98%; padding-top: 10px; padding-left: 5px">
         <dl class="accordion">
            <dt style="width: 100%;">
                <span>FILTER CONDITION</span>
            </dt>
        </dl>

        <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/calendar.png" />&nbsp;&nbsp;Date:<br />
         <div style="padding-top: 5px;" class="qlcbFormItem">
                From&nbsp;&nbsp;<telerik:RadDatePicker ID="txtFromDate"  runat="server" 
                    ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired" Width="120" OnSelectedDateChanged="txtFromDate_OnSelectedDateChanged" AutoPostBack="True">
                    <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                </telerik:RadDatePicker>
             
             To&nbsp;&nbsp;<telerik:RadDatePicker ID="txtToDate"  runat="server" 
                    ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired" Width="120" OnSelectedDateChanged="txtFromDate_OnSelectedDateChanged" AutoPostBack="True">
                    <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                </telerik:RadDatePicker>
            </div><br />

        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/group.png" />&nbsp;&nbsp;Department:<br />
        
        <telerik:RadComboBox ID="ddlDepartment" runat="server" 
            Skin="Windows7" Width="100%" AutoPostBack="True" 
            OnItemDataBound="ddlDepartment_ItemDataBound"
            OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged"/>
        <br />
        <br />
        <%--<hr/>--%>
        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/user.png" />&nbsp;&nbsp;Employee:
        <telerik:RadTreeView ID="rtvEmployee" runat="server" 
            Width="100%" Height="100%" ShowLineImages="False"
            OnNodeClick="rtvEmployee_NodeClick" 
            OnNodeDataBound="rtvEmployee_NodeDataBound"
            OnClientNodeClicking="rtvEmployee_ClientNodeClicking"
            >
            <DataBindings>
                <telerik:RadTreeNodeBinding Expanded="false"></telerik:RadTreeNodeBinding>
            </DataBindings>
        </telerik:RadTreeView>
    </div>
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Horizontal">
        
        <telerik:RadPane ID="RadPane2" runat="server" Scrollable="false" Scrolling="None">
            <telerik:RadSplitter ID="Radsplitter3" runat="server" Orientation="Horizontal">
                <telerik:RadPane ID="Radpane4" runat="server" Scrolling="None" >
                    <telerik:RadSplitter ID="Radsplitter10" runat="server" Orientation="Vertical">
                        <telerik:RadPane ID="Radpane6" runat="server" Scrolling="None">
                            
                            <%--<telerik:RadTabStrip ID="RadTabStrip1" runat="server"  width="100%" Skin="MetroTouch" MultiPageID="RadMultiPage1"
            SelectedIndex="0">
                            <Tabs>
                                <telerik:RadTab ImageUrl="~/Images/perform3_32.png" Text="General Info">
                                </telerik:RadTab>
                                <telerik:RadTab ImageUrl="~/Images/performDetail3_32.png" Text="Detail Info">
                                </telerik:RadTab>
                            </Tabs>
                        </telerik:RadTabStrip>--%>
                            
                        <%--<telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0">
                            <telerik:RadPageView runat="server" Height="100%" ID="RadPageView1">--%>
                                
                                <telerik:RadSplitter ID="RadSplitter1" runat="server" Orientation="Horizontal">
                                    <telerik:RadPane ID="RadPane3" runat="server" Height="30px" Scrollable="false" Scrolling="None">
                                        <telerik:RadToolBar ID="CustomerMenu" runat="server" Width="100%" OnClientButtonClicking="OnClientButtonClicking">
                                            <Items>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/Images/excelfile.png" Text="Export Performance Review Form" Value="ExportExcel"/>
                                                <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/Images/attach.png" Text="Attach Performance Review Form" Value="AttachFile"/>

                                            </Items>
                                        </telerik:RadToolBar>

                                        <%--<telerik:RadButton ID="btnExportForm" runat="server" OnClick="btnExportForm_OnClick" Text="Export Performance Review Form" Width="220px" style="text-align: center">
                                            <Icon PrimaryIconUrl="~/Images/excelfile.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"/>
                                        </telerik:RadButton>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <telerik:RadButton ID="btnAttach" runat="server" AutoPostBack="False" Text="Attach Performance Review Form" Width="220px" style="text-align: center" OnClientClicked="ShowAttachFiles" >
                                            <Icon PrimaryIconUrl="~/Images/attach.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"/>
                                        </telerik:RadButton>--%>
                                    </telerik:RadPane>

                                    <telerik:RadPane ID="RadPane1" runat="server" Scrollable="false" Scrolling="None">
                                        <telerik:RadGrid AllowCustomPaging="False" AllowPaging="False" AllowSorting="True" 
                                            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                                            Height="100%" ID="grdDocument"  AllowFilteringByColumn="True" AllowMultiRowSelection="False"
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
                                                                <telerik:GridGroupByField FieldAlias="Project" FieldName="ObjectProject" FormatString="{0:D}"
                                                                    HeaderValueSeparator=": "></telerik:GridGroupByField>
                                                            </SelectFields>
                                                            <GroupByFields>
                                                                <telerik:GridGroupByField FieldName="ObjectProject" SortOrder="Ascending" ></telerik:GridGroupByField>
                                                            </GroupByFields>
                                                        </telerik:GridGroupByExpression>
                                                    </GroupByExpressions> --%>   
                                                <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="false"/>
                                                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    
                                                <Columns>
                                                    <telerik:GridTemplateColumn HeaderText="No." Groupable="False" AllowFiltering="false">
                                                        <HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSoTT" runat="server" Text='<%# grdDocument.CurrentPageIndex * grdDocument.PageSize + grdDocument.Items.Count+1 %>'>
                                                            </asp:Label>
                                      
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    
                                        
                                                    <telerik:GridTemplateColumn HeaderText="Job Type" UniqueName="Title"
                                                        AllowFiltering="false">
                                                        <HeaderStyle HorizontalAlign="Center" Width="200"/>
                                                        <ItemStyle HorizontalAlign="Left"  />
                                                        <ItemTemplate>
                                                            <%# Eval("Title") %>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    
                                                    <telerik:GridTemplateColumn HeaderText="Total Item" UniqueName="TotalObj"
                                                        AllowFiltering="false">
                                                        <HeaderStyle HorizontalAlign="Center" Width="100"/>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <%# Eval("TotalObj") %>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    
                                                    <telerik:GridTemplateColumn HeaderText="Completed Item" UniqueName="CompletedObj"
                                                        AllowFiltering="false">
                                                        <HeaderStyle HorizontalAlign="Center" Width="100"/>
                                                        <ItemStyle HorizontalAlign="Center"  BackColor="Aqua"/>
                                                        <ItemTemplate>
                                                            <%# Eval("CompletedObj") %>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn HeaderText="Overdue Item" UniqueName="OverdueObj"
                                                        AllowFiltering="false">
                                                        <HeaderStyle HorizontalAlign="Center" Width="100"/>
                                                        <ItemStyle HorizontalAlign="Center"  BackColor="Red"/>
                                                        <ItemTemplate>
                                                            <%# Eval("OverdueObj") %>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    
                                                    <telerik:GridTemplateColumn HeaderText="Incomplete Item" UniqueName="IncompleteObj"
                                                        AllowFiltering="false">
                                                        <HeaderStyle HorizontalAlign="Center" Width="100"/>
                                                        <ItemStyle HorizontalAlign="Center"  BackColor="Coral"/>
                                                        <ItemTemplate>
                                                            <%# Eval("IncompleteObj") %>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                                        <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                    </telerik:RadPane>
                                </telerik:RadSplitter>
                            <%--</telerik:RadPageView>--%>
                            
                            <%--<telerik:RadPageView runat="server" Height="100%" ID="RadPageView2">
                                <telerik:RadGrid AllowCustomPaging="False" AllowPaging="False" AllowSorting="True" 
                                    AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                                    Height="100%" ID="grdTracking"  AllowFilteringByColumn="True" AllowMultiRowSelection="False"
                                    OnNeedDataSource="grdTracking_OnNeedDataSource" 
                                    PageSize="100" runat="server" Style="outline: none" Width="100%">
                                    <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
                                    <GroupingSettings CaseSensitive="False"></GroupingSettings>
                                    <MasterTableView AllowMultiColumnSorting="false"
                                        ClientDataKeyNames="ObjId" DataKeyNames="ObjId" CommandItemDisplay="Top" 
                                        EditMode="InPlace" Font-Size="8pt">
                                        <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField FieldAlias="Project" FieldName="ProjectName" FormatString="{0:D}"
                                                            HeaderValueSeparator=": "></telerik:GridGroupByField>
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="ProjectName" SortOrder="Ascending" ></telerik:GridGroupByField>
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
                                                    <asp:Label ID="lblSoTT" runat="server" Text='<%# grdTracking.CurrentPageIndex * grdTracking.PageSize + grdTracking.Items.Count+1 %>'>
                                                    </asp:Label>
                                      
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="EditColumn">
                                                <HeaderStyle Width="30"  />
                                                <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <a href='javascript:ShowTrackingEditForm("<%# DataBinder.Eval(Container.DataItem, "ObjId") %>", <%# DataBinder.Eval(Container.DataItem, "ObjectTypeId") %>, <%# DataBinder.Eval(Container.DataItem, "ProjectId") %>)' style="text-decoration: none; color:blue">
                                                    <asp:Image ID="EditLink" runat="server" ImageUrl="~/Images/edit.png" Style="cursor: pointer;" AlternateText="Edit properties" />
                                                        <a/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="AttachColumn">
                                                <HeaderStyle Width="30"  />
                                                <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <a href='javascript:ShowAttachment("<%# DataBinder.Eval(Container.DataItem, "ObjId") %>", <%# DataBinder.Eval(Container.DataItem, "ObjectTypeId") %>)' style="text-decoration: none; color:blue">
                                                    <asp:Image ID="AttachLink" runat="server" ImageUrl="~/Images/attach.png" Style="cursor: pointer;" AlternateText="Attach Files" />
                                                        <a/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="HistoryColumn">
                                                <HeaderStyle Width="30"  />
                                                <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <a href='javascript:ShowHistory("<%# DataBinder.Eval(Container.DataItem, "ObjId") %>", <%# DataBinder.Eval(Container.DataItem, "ObjectTypeId") %>)' style="text-decoration: none; color:blue">
                                                    <asp:Image ID="HistoryLink" runat="server" ImageUrl="~/Images/history.png" Style="cursor: pointer;" AlternateText="Edit properties" />
                                                        <a/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Object Type" UniqueName="ObjectType"
                                            AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="100" />
                                            <ItemStyle HorizontalAlign="Left"  />
                                            <ItemTemplate>
                                                <%# Eval("ObjectType") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                            
                                        <telerik:GridTemplateColumn HeaderText="Object No." UniqueName="ObjectCode"
                                            DataField="ObjectCode" ShowFilterIcon="False" FilterControlWidth="97%" 
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center" Width="160" />
                                            <ItemStyle HorizontalAlign="Left"/>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDocNo" runat="server" Text='<%# Eval("ObjectCode") %>' style="cursor: pointer"/> 
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                            
                                        <telerik:GridTemplateColumn HeaderText="Object Title" UniqueName="ObjectTitle"
                                            DataField="ObjectTitle" ShowFilterIcon="False" FilterControlWidth="97%" 
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <HeaderStyle HorizontalAlign="Center" Width="400" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%# Eval("ObjectTitle") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Deadline" UniqueName="Deadline"
                                            AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center" Width="80"/>
                                            <ItemTemplate>
                                                <%# Eval("Deadline") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                                
                                <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                                </ClientSettings>
                            </telerik:RadGrid>
                            </telerik:RadPageView>
                        </telerik:RadMultiPage>--%>
                            
                            

                            
                    </telerik:RadPane>

                </telerik:RadSplitter>       

            </telerik:RadPane>
            </telerik:RadSplitter>
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

    <span style="display: none">
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rtvEmployee">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="CustomerMenu" />
                        <telerik:AjaxUpdatedControl ControlID="IsFullPermission" />
                        <telerik:AjaxUpdatedControl ControlID="lblUserId"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="txtToDate">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="txtFromDate">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
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
                        <telerik:AjaxUpdatedControl ControlID="rtvEmployee" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="CustomerMenu" LoadingPanelID="RadAjaxLoadingPanel2"/>
                      
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlDepartment">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChildProject" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="rtvEmployee" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="CustomerMenu"/>
                        <telerik:AjaxUpdatedControl ControlID="lblUserId"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
               <telerik:AjaxSetting AjaxControlID="ChildProject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvEmployee" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="CustomerMenu"/>
                    </UpdatedControls>

                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ckbShowAll">
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
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Document Information"
                VisibleStatusbar="false" Height="600" Width="650" 
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
            
            <telerik:RadWindow ID="AttachPerformanceFile" runat="server" Title="Attach Performance Review files"
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
                VisibleStatusbar="false" Height="450" Width="610" 
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

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
    <asp:HiddenField runat="server" ID="FolderContextMenuAction"/>
    <asp:HiddenField runat="server" ID="lblDisciplineId"/>
    <asp:HiddenField runat="server" ID="lblFolderId"/>
    <asp:HiddenField runat="server" ID="lblDocId"/>
    <asp:HiddenField runat="server" ID="lblCategoryId"/>
    <asp:HiddenField runat="server" ID="IsFullPermission"/>
    <asp:HiddenField runat="server" ID="CanReject"/>
    <asp:HiddenField runat="server" ID="CanConsolidate"/>
    <asp:HiddenField runat="server" ID="lblProjectId"/>
    <asp:HiddenField runat="server" ID="ObjectType"/>
    <asp:HiddenField runat="server" ID="ObjectId"/>
    <asp:HiddenField runat="server" ID="IsOnshoreComment"/>
    <asp:HiddenField runat="server" ID="IsDistributeOnshore"/>
    <asp:HiddenField runat="server" ID="IsFinal"/>
    <asp:HiddenField runat="server" ID="ActionType"/>
    <asp:HiddenField runat="server" ID="lblUserId"/>

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
                //alerrgs.get_eventTarget()); 
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
                document.getElementById("<%= lblDocId.ClientID %>").value = Id;
            }

            function ShowAttachFiles(sender, args) {

                var owd = $find("<%=AttachPerformanceFile.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                        
                owd.setUrl("Controls/WMS/MRAttach.aspx?objId=" + docId, "AttachPerformanceFile");
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
                        case "Distribute":
                            var objAssignUserId = document.getElementById("<%= lblDocId.ClientID %>").value;
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

                        case "MarkupList":
                            var canConsolidate = document.getElementById("<%= CanConsolidate.ClientID %>").value;
                            var objType = document.getElementById("<%= ObjectType.ClientID %>").value;
                            var objId = document.getElementById("<%= ObjectId.ClientID %>").value;
                            var objAssignUserId = document.getElementById("<%= lblDocId.ClientID %>").value;

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

            function ShowEditForm(docId, projId) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Document/DocumentPackageInfoEditForm.aspx?projId=" + projId + "&docId=" + docId, "CustomerDialog");
            }

            function ShowTrackingEditForm(id, objType, projectId) {

                switch (objType) {
                    case 1:
                        var owd = $find("<%=UpdateMC.ClientID %>");
                        owd.setSize(850, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/WMS/TrackingMorningCallEditForm.aspx?objId=" + id + "&projId=" + projectId, "UpdateMC");
                        break;
                    case 2:
                        var owd = $find("<%=UpdateWCR.ClientID %>");
                        owd.setSize(850, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/WMS/TrackingWCREditForm.aspx?objId=" + id + "&projId=" + projectId, "UpdateWCR");
                        break;
                    case 3:
                        var owd = $find("<%=UpdatePL.ClientID %>");
                        owd.setSize(850, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/WMS/TrackingPunchEditForm.aspx?objId=" + id + "&projId=" + projectId, "UpdatePL");
                        break;
                    case 4:
                        var owd = $find("<%=UpdateSL.ClientID %>");
                        owd.setSize(850, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/WMS/TrackingSailEditForm.aspx?objId=" + id + "&projId=" + projectId, "UpdateSL");
                        break;
                    case 5:
                        var owd = $find("<%=UpdateP.ClientID %>");
                        owd.setSize(850, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/WMS/TrackingProcedureEditForm.aspx?objId=" + id + "&projId=" + projectId, "UpdateP");
                        break;
                    case 6:
                        var owd = $find("<%=UpdateGW.ClientID %>");
                        owd.setSize(850, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("Controls/WMS/TrackingGeneralWorkingEditForm.aspx?objId=" + id + "&projId=" + projectId, "UpdateGW");
                    break;
                default:
                }
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

            function ShowHistory(id, objType) {
                switch (objType) {
                    case 1:
                        var owd = $find("<%=RevisionHistoryMC.ClientID %>");
                        owd.Show();
                        owd.maximize();
                        owd.setUrl("Controls/WMS/TrackingMorningCallRevisionHistory.aspx?objId=" + id, "RevisionHistoryMC");
                        break;
                    case 2:
                        var owd = $find("<%=RevisionHistoryWCR.ClientID %>");
                        owd.Show();
                        owd.maximize();
                        owd.setUrl("Controls/WMS/TrackingWCRRevisionHistory.aspx?objId=" + id, "RevisionHistoryWCR");
                        break;
                    case 3:
                        var owd = $find("<%=RevisionHistoryPL.ClientID %>");
                        owd.Show();
                        owd.maximize();
                        owd.setUrl("Controls/WMS/TrackingPunchRevisionHistory.aspx?objId=" + id, "RevisionHistoryPL");
                        break;
                    case 4:
                        var owd = $find("<%=RevisionHistorySL.ClientID %>");
                        owd.Show();
                        owd.maximize();
                        owd.setUrl("Controls/WMS/TrackingSLRevisionHistory.aspx?objId=" + id, "RevisionHistorySL");
                        break;
                    case 5:
                        var owd = $find("<%=RevisionHistoryP.ClientID %>");
                        owd.Show();
                        owd.maximize();
                        owd.setUrl("Controls/WMS/TrackingProcedureRevisionHistory.aspx?objId=" + id, "RevisionHistoryP");
                        break;
                    case 6:
                        var owd = $find("<%=RevisionHistoryGW.ClientID %>");
                        owd.Show();
                        owd.maximize();
                        owd.setUrl("Controls/WMS/TrackingGeneralWorkingRevisionHistory.aspx?objId=" + id, "RevisionHistoryGW");
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
            
            function rtvEmployee_ClientNodeClicking(sender, args) {
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

                if (strValue == "ExportExcel") {
                    ajaxManager.ajaxRequest("ExportExcel");
                }
                else if (strValue == "AttachFile") {
                    var objId = document.getElementById("<%= lblUserId.ClientID %>").value;
                    var owd = $find("<%=AttachPerformanceFile.ClientID %>");
                    owd.setSize(730, document.documentElement.offsetHeight);
                    owd.Show();
                        
                    owd.setUrl("Controls/WMS/PerformanceReviewAttach.aspx?objId=" + objId, "AttachPerformanceFile");
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
                document.getElementById("<%= lblDocId.ClientID %>").value = Id;

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