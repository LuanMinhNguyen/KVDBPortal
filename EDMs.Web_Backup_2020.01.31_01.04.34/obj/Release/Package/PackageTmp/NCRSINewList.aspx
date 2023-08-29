<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NCRSINewList.aspx.cs" Inherits="EDMs.Web.NCRSINewList" EnableViewState="true" %>

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
        .rgExpandCol {
            width: 1% !important;
        }
        .NullBorder{
            border:none !important;
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

       
        /*#ctl00_ContentPlaceHolder2_grdNCRSI_GridData {
            height: 100% !important;
        }*/

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

        #ctl00_ContentPlaceHolder2_radMenuOutgoing_i2_btnSearch {
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
            height: 100% !important;
        }
          
         #ctl00_ContentPlaceHolder2_grdNCRSI_GridData {
            height: 84% !important;
        }

         #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdIncomingTransPanel{
            height: 100% !important;
        } #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdNCRSIPanel
        {
            height: 94% !important;
        }

          #ctl00_ContentPlaceHolder2_radMenuIncoming_i1_btnSearchIncoming {
              margin-top: -3px;
          }

          #RadWindowWrapper_ctl00_ContentPlaceHolder2_AttachDocList {
              top: 0px !important;
          }

          #ctl00_ContentPlaceHolder2_radMenuOutgoing_i2_divStatus {
              display: inherit !important;
          }

    </style>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Horizontal">
        <telerik:RadPane ID="RadPane3" runat="server" Height="30px" Scrollable="false" Scrolling="None">
            <telerik:RadToolBar ID="radMenuOutgoing" runat="server" Width="100%" OnClientButtonClicking="radMenuOutgoing_OnClientButtonClicking">
                <Items>
                    <telerik:RadToolBarDropDown runat="server" Text="New" ImageUrl="~/Images/addNew.png" >
                        <Buttons>
                            <telerik:RadToolBarButton runat="server" Text="NCR/SI" Value="1" ImageUrl="~/Images/index.png" />
                        </Buttons>
                    </telerik:RadToolBarDropDown>
                                    
                    <telerik:RadToolBarButton runat="server" IsSeparator="true" Visible="False"/>

                    <telerik:RadToolBarButton runat="server" Value="ShowAll">
                        <ItemTemplate>
                            &nbsp;&nbsp;<img src="../../Images/filter.png"/>&nbsp;&nbsp;<b style="color: red">Filter: </b> &nbsp;&nbsp;
                            
                            <img src="../../Images/project.png"/>Selected project:&nbsp;&nbsp;
                            <telerik:RadComboBox ID="ddlProject" runat="server" 
                                Skin="Windows7" Width="250" AutoPostBack="True" 
                                OnItemDataBound="ddlProject_ItemDataBound"
                                OnSelectedIndexChanged="ddlProject_SelectedIndexChanged"/>&nbsp;&nbsp;|&nbsp;&nbsp;
                           
                            <b>Show</b>  
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="rcbReadOnly" Width="200px" style="max-width: 200px"
                                OnSelectedIndexChanged="ddlStatus_OnSelectedIndexChanged" AutoPostBack="True">
                                <Items>
                                    <asp:ListItem Value="All" Text="All NCR/SI"></asp:ListItem>
                                    <asp:ListItem Value="NCR" Text="My NCR"></asp:ListItem>
                                    <asp:ListItem Value="SI" Text="My SI" ></asp:ListItem>
                                    
                                </Items>
                            </asp:DropDownList>

                            &nbsp;&nbsp; | &nbsp;&nbsp;
                            <b>Search All Text Field</b>
                            <asp:TextBox ID="txtSearch" runat="server" Style="width: 150px;" CssClass="defaultTextBox"/>
                            <telerik:RadButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Width="25px" style="text-align: center">
                                <Icon PrimaryIconUrl="~/Images/search.png" PrimaryIconLeft="3" PrimaryIconTop="2" PrimaryIconWidth="16" PrimaryIconHeight="16"/>
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:RadToolBarButton>
                </Items>
            </telerik:RadToolBar>
        </telerik:RadPane>
        <telerik:RadPane ID="RadPane1" runat="server" Scrollable="false" Scrolling="None">
            <telerik:RadGrid AllowCustomPaging="False" AllowPaging="true" AllowSorting="True" 
                    AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                    Height="100%" ID="grdNCRSI"  AllowFilteringByColumn="true" AllowMultiRowSelection="False"
                    OnNeedDataSource="grdNCRSI_OnNeedDataSource" 
                    OnDeleteCommand="grdNCRSI_OnDeleteCommand"
                    OnItemCreated="grdNCRSI_ItemCreated"
                    PageSize="100" runat="server" Style="outline: none" Width="100%">
                    <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
                    <GroupingSettings CaseSensitive="False"></GroupingSettings>
                    <MasterTableView AllowMultiColumnSorting="false"
                        ClientDataKeyNames="ID" DataKeyNames="ID" CommandItemDisplay="Top" 
                        EditMode="InPlace" Font-Size="8pt">
                        <GroupByExpressions>
                            <telerik:GridGroupByExpression>
                                <SelectFields>
                                    <telerik:GridGroupByField FieldAlias="Type" FieldName="TypeName" FormatString="{0:D}"
                                        HeaderValueSeparator=": "></telerik:GridGroupByField>
                                </SelectFields>
                                <GroupByFields>
                                    <telerik:GridGroupByField FieldName="TypeName" SortOrder="Ascending" ></telerik:GridGroupByField>
                                </GroupByFields>
                            </telerik:GridGroupByExpression>
                        </GroupByExpressions> 
                        <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="false"/>
                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; NCR/SI." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    
                        <Columns>
                                        
                            <telerik:GridTemplateColumn HeaderText="No." Groupable="False" AllowFiltering="false" Display="False">
                                <HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblSoTT" runat="server" Text='<%# grdNCRSI.CurrentPageIndex * grdNCRSI.PageSize + grdNCRSI.Items.Count+1 %>'>
                                    </asp:Label>
                                      
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                    
                            <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="EditColumn">
                                <HeaderStyle Width="30"  />
                                <ItemStyle HorizontalAlign="Center"/>
                                <ItemTemplate>
                                    <a href='javascript:ShowEditForm("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                    <asp:Image ID="EditLink" runat="server" ImageUrl="~/Images/edit.png" Style="cursor: pointer;" ToolTip="Edit properties"/>
                                        <a/>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="DeleteColumn">
                                <HeaderStyle Width="30"  />
                                <ItemStyle HorizontalAlign="Center"/>
                                <ItemTemplate>
                                    <div runat="server" visible='<%# !Convert.ToBoolean(Eval("IsAttachWorkflow"))%>'>
                                    <a href='javascript:DeleteNCRSI("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                    <asp:Image ID="delete" runat="server" ImageUrl="~/Images/delete.png" Style="cursor: pointer;" ToolTip="Delete NCR/SI" />
                                        <a/></div>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        
                            <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="ExportExcelForm">
                                <HeaderStyle Width="30"  />
                                <ItemStyle HorizontalAlign="Center"/>
                                <ItemTemplate>
                                    <a href='javascript:ExportExcelForm("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                    <asp:Image ID="imgExcel" runat="server" ImageUrl="~/Images/excelfile1.png" Style="cursor: pointer;" ToolTip="Export NCR/SI Form" Visible='<%# !Convert.ToBoolean(Eval("IsAttachWorkflow"))%>' />
                                    <a/>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="AllAttachFile" >
                                <HeaderStyle Width="30"  />
                                <ItemStyle HorizontalAlign="Center"/>
                                <ItemTemplate>
                                    <a href='javascript:ShowAllAttachFile("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                    <asp:Image ID="imgAllAttach" runat="server" ImageUrl="~/Images/attach.png" Style="cursor: pointer;" ToolTip="Attach Files"/>
                                    <a/>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                    
                            
                                    
                            <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="AttachWorkflow" >
                                <HeaderStyle Width="30"  />
                                <ItemStyle HorizontalAlign="Center"/>
                                <ItemTemplate>
                                    <a href='javascript:AttachWorkflow("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                    <asp:Image ID="imgAttachWorkflow" runat="server" ImageUrl="~/Images/attachwf.png" Style="cursor: pointer;" ToolTip="Attach NCR/SI To Workflow" Visible='<%# !Convert.ToBoolean(Eval("IsAttachWorkflow"))%>'/>
                                        <a/>
                                                
                                        <asp:Image ID="imgAttachWorkflowOk" runat="server" ImageUrl="~/Images/attachwfOk.png" ToolTip="NCR/SI Attached To Workflow" Visible='<%# Convert.ToBoolean(Eval("IsAttachWorkflow"))%>'/>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                       <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="DeleteWorkflow">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:DeleteWorkflow("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="imgDeleteWorkflow" runat="server" ImageUrl="~/Images/deletewf.png" Style="cursor: pointer;" ToolTip="Delete Workflow " Visible='<%# Convert.ToBoolean(Eval("IsAttachWorkflow"))%>'/>
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="WorkflowProcessHistory">
                                <HeaderStyle Width="30"  />
                                <ItemStyle HorizontalAlign="Center"/>
                                <ItemTemplate>
                                    <a href='javascript:ShowWorkflowProcessHistory("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                    <asp:Image ID="imgWorkflowProcessHistory" runat="server" ImageUrl="~/Images/history.png" Style="cursor: pointer;" ToolTip="Workflow process history" Visible='<%# Convert.ToBoolean(Eval("IsAttachWorkflow"))%>'/>
                                        <a/>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                
                            <telerik:GridTemplateColumn HeaderText="Review Result" UniqueName="ReviewResult" DataField="ReviewResult" Display="false"
                                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%# Eval("ReviewResult") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn UniqueName="GroupN" DataField="GroupName" HeaderText="Group">
                                 <HeaderStyle HorizontalAlign="Center" Width="60" />
                                                <ItemStyle HorizontalAlign="Center" Width="60" />
                        <FilterTemplate>
                            <telerik:RadComboBox ID="RadComboBoxTitle" Width="100%"  SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("GroupN").CurrentFilterValue %>'
                                runat="server" OnClientSelectedIndexChanged="TitleIndexChanged">
                                <Items>
                                    <telerik:RadComboBoxItem Text="All"  Value=""/>
                                </Items>
                            </telerik:RadComboBox>
                            <telerik:RadScriptBlock ID="RadScriptBlockFilter" runat="server">
                                <script type="text/javascript">
                                    function TitleIndexChanged(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        tableView.filter("GroupN", args.get_item().get_value(), "EqualTo");
                                }
                                </script>
                            </telerik:RadScriptBlock>
                        </FilterTemplate>
                    </telerik:GridBoundColumn>
                    
                           <telerik:GridTemplateColumn HeaderText="NCR/SI" UniqueName="Number" DataField="Number"
                                    ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                        <%# Eval("Number") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Subject" UniqueName="Subject" DataField="Subject"
                                ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="250" />
                                <ItemStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%# Eval("Subject") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        
                        <telerik:GridDateTimeColumn HeaderText="Issued Date" UniqueName="IssuedDate" DataField="IssuedDate"
                                  DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" >
                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                            <ItemStyle HorizontalAlign="Center"/>
                        </telerik:GridDateTimeColumn> 
                        
                           <telerik:GridDateTimeColumn HeaderText="EPC Update Action Taken" UniqueName="EPCUpdateActionTaken" DataField="EPCUpdateActionTaken"
                                  AllowFiltering="false"  DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" >
                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                            <ItemStyle HorizontalAlign="Center"/>
                        </telerik:GridDateTimeColumn>       
                        
                        <telerik:GridDateTimeColumn HeaderText="Date Of Submission" UniqueName="DateOfSubmission" DataField="DateOfSubmission" Display="false"
                                                    DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" >
                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                            <ItemStyle HorizontalAlign="Center"/>
                        </telerik:GridDateTimeColumn> 
                                        
                              <%--  
                                <telerik:GridTemplateColumn HeaderText="Status" UniqueName="Status" DataField="Status"
                                ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="60" />
                                <ItemStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%# Eval("Status") %>
                                </ItemTemplate>
                                </telerik:GridTemplateColumn>--%> 
                            <telerik:GridBoundColumn UniqueName="Status" DataField="Status" HeaderText="Status">
                                 <HeaderStyle HorizontalAlign="Center" Width="60" />
                                                <ItemStyle HorizontalAlign="Center" Width="60" />
                        <FilterTemplate>
                            <telerik:RadComboBox ID="RadComboBoxStatus" Width="100%"  SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("Status").CurrentFilterValue %>'
                                runat="server" OnClientSelectedIndexChanged="StatusIndexChanged">
                                <Items>
                                    <telerik:RadComboBoxItem Text="All"  Value=""/>
                                   <%--  <telerik:RadComboBoxItem Text="Opening"  Value="Opening"/>
                                     <telerik:RadComboBoxItem Text="Unclose"  Value="Unclose"/>
                                     <telerik:RadComboBoxItem Text="Closing"  Value="Closing"/>
                                     <telerik:RadComboBoxItem Text="Closed"  Value="Closed"/>--%>
                                </Items>
                            </telerik:RadComboBox>
                            <telerik:RadScriptBlock ID="RadScriptBlockFilter1" runat="server">
                                <script type="text/javascript">
                                    function StatusIndexChanged(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        tableView.filter("Status", args.get_item().get_value(), "Contains");
                                }
                                </script>
                            </telerik:RadScriptBlock>
                        </FilterTemplate>
                    </telerik:GridBoundColumn>
                            <telerik:GridDateTimeColumn HeaderText="Closed Date" UniqueName="ClosedDate" DataField="ClosedDate"
                                    DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" >
                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                <ItemStyle HorizontalAlign="Center"/>
                            </telerik:GridDateTimeColumn> 
                        <telerik:GridTemplateColumn  HeaderText="Signed By PMC" UniqueName="SignedByPMC" DataField="SignedByPMC"
                                ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="110" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("SignedByPMC") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn  HeaderText="Signed By PMB" UniqueName="SignedByPMB" DataField="SignedByPMB"
                                ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="110" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("SignedByPMB") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Closed By PMC" UniqueName="ClosedByPMC" DataField="ClosedByPMC"
                                    ShowFilterIcon="False" FilterControlWidth="97%" 
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="110" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("ClosedByPMC") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        
                            <telerik:GridTemplateColumn  HeaderText="Closed By PMB" UniqueName="ClosedByPMB" DataField="ClosedByPMB" Display="false"
                                ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("ClosedByPMB") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                              <telerik:GridTemplateColumn HeaderText="Created By" UniqueName="CreatedByName" DataField="CreatedByName"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="100" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("CreatedByName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Is Cancel" UniqueName="IsCancel" AllowFiltering="False">
                                                <HeaderStyle Width="80" HorizontalAlign="Center" ></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Image ID="imgActive" runat="server" ImageUrl="~/Images/cancel1.png" Visible='<%# Eval("IsCancel")!= null && (bool) Eval("IsCancel")==true %>'/>
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
                <telerik:AjaxUpdatedControl ControlID="grdNCRSI" LoadingPanelID="RadAjaxLoadingPanel1"/>
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
        
        <telerik:AjaxSetting AjaxControlID="btnSearch">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdNCRSI" LoadingPanelID="RadAjaxLoadingPanel1"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
                
        <telerik:AjaxSetting AjaxControlID="grdNCRSI">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdNCRSI" LoadingPanelID="RadAjaxLoadingPanel1"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
                
        <telerik:AjaxSetting AjaxControlID="ddlProject">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdNCRSI" LoadingPanelID="RadAjaxLoadingPanel1"/>
                <telerik:AjaxUpdatedControl ControlID="lblProjectId"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="ddlProjectIncoming">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdIncomingTrans" LoadingPanelID="RadAjaxLoadingPanel1"/>
                <telerik:AjaxUpdatedControl ControlID="lblProjectId"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="ddlStatus">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdNCRSI" LoadingPanelID="RadAjaxLoadingPanel1"/>
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
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="NCR/SI Information"
                VisibleStatusbar="false" IconUrl="~/Images/index.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Reload, Close">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachDocFile" runat="server" Title="NCR/SI Attach Referenced Document Files"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/attach.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" OnClientClose="refreshAttachDocGrid">
            </telerik:RadWindow>
            
             <telerik:RadWindow ID="ContractorAttachDocFile" runat="server" Title="Transmittal Attach Document"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/attach.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachDocList" runat="server" Title="Already Attach Document List"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/documents.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" >
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ETRMAttachFile" runat="server" Title="NCR/SI Attach Files"
                VisibleStatusbar="false" Height="600" Width="700" IconUrl="~/Images/excelfileattach.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Reload, Close">
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
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close, Reload">
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
            
            <telerik:RadWindow ID="AttachWorkflow" runat="server" Title="Attach NCR/SI to Workflow"
                VisibleStatusbar="False" Height="450" Width="610" IconUrl="~/Images/attachwf.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ObjDetailDialog" runat="server" Title="Object Details"
                VisibleStatusbar="False" Height="700" Width="850" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
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
                VisibleStatusbar="False" Height="450" Width="610"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="UpdateMC" runat="server" Title="Update Morning Call"
                VisibleStatusbar="False" Height="700" Width="850" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RejectDocTrans" runat="server" Title="Reject Transmittal"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/reject2.png"
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
            
            <telerik:RadWindow ID="CreateOutgoingTrans" runat="server" Title="Create Outgoing Transmittal"
                VisibleStatusbar="False" IconUrl="~/Images/outgoingTrans.png"
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
    <asp:HiddenField runat="server" ID="lblProjectId"/>
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

            function refreshGrid() {
                var masterTable = $find("<%=grdNCRSI.ClientID%>").get_masterTableView();
                masterTable.rebind();
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

            function ShowRejectForm(objId) {
                var owd = $find("<%=RejectDocTrans.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("ContractorTransmittalAttachDocFileReject.aspx?objId=" + objId, "RejectDocTrans");
                }
            function DeleteWorkflow(Id) {
                var st = prompt("Do you want to delete workflow? \n Please Enter 'OK' to delete workflow.");
                if (st == "OK") {

                    ajaxManager.ajaxRequest("DeleteWorkflow_" + Id);
                }
            }
            function ShowWorkflowProcessHistory(docId) {
                var owd = $find("<%=WFProcessHistory.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.setUrl("../../Controls/Workflow/WorkflowProcessHistory.aspx?objId=" + docId, "WFProcessHistory");
            }

            function ShowAddNew(refChangeRequest) {
                var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.setSize(1000, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("Controls/Document/NCRSIEditForm.aspx?projectId=" + projectId + "&ref=" + refChangeRequest, "CustomerDialog");
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


            function ShowDocumentRevision(docId) {
                <%--var categoryId = document.getElementById("<%= lblCategoryId.ClientID %>").value;--%>
                var owd = $find("<%=RevisionDialog.ClientID %>");
                owd.Show();
                owd.maximize(true);
                owd.setUrl("Controls/Document/ChangeRequestRevisionHistory.aspx?docId=" + docId , "RevisionDialog");
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
                var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.setSize(1000, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("Controls/Document/NCRSIEditForm.aspx?projectId=" + projectId + "&objId=" + objId, "CustomerDialog");
            }
             function ShowIncomingAttachDocFile(objId,upload) {
                var owd = $find("<%=AttachDocFile.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.setUrl("PECC2TransmittalAttachDocFileList.aspx?objId=" + objId +"&Upload="+upload, "AttachDocFile");
            }
            function ShowContractorAttachDocFile(objId,TransObj, forSend) {
                var projId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                var owd = $find("<%=ContractorAttachDocFile.ClientID %>");
                owd.Show();
                owd.maximize();
                //alert(1);
                if (forSend === "1") {
                    owd.setUrl("ContractorTransmittalAttachDocFileList.aspx?objId=" + objId + "&projId=" + projId + "&PECC2TransID=" + TransObj, "ContractorAttachDocFile");
                } else {
                    owd.setUrl("ContractorTransmittalAttachChangeRequestFileList.aspx?objId=" + objId + "&projId=" + projId + "&PECC2TransID=" + TransObj, "ContractorAttachDocFile");
                }
                
            }

            function ShowAttachDocFile(objId) {
                var owd = $find("<%=AttachDocFile.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.setUrl("Controls/Document/ChangeRequestAttachDocFile.aspx?objId=" + objId, "AttachDocFile");
            }
            function DeleteNCRSI(id) {
                if (confirm("Do you want to delete item?") == false) return;
                ajaxManager.ajaxRequest("DeleteNRCIS_" + id);
            }
            function ShowDocumentList(objId, forSendId) {
                var owd = $find("<%=AttachDocList.ClientID %>");
                owd.Show();
                owd.maximize();
                //owd.setSize(830, document.documentElement.offsetHeight);
                if (forSendId === "1") {
                    owd.setUrl("TransmittalDocumentList.aspx?objId=" + objId, "AttachDocList");
                } else {
                    owd.setUrl("TransmittalChangeRequestList.aspx?objId=" + objId, "AttachDocList");
                }
                
            }

            function ExportExcelForm(objId) {
                ajaxManager.ajaxRequest("ExportNCRSIForm_" + objId);
            }

            function ExportContractorETRM(objId) {
                ajaxManager.ajaxRequest("ExportContractorETRM_" + objId);
            }

            function ShowETRMAttachFile(objId) {
                var owd = $find("<%=ETRMAttachFile.ClientID %>");
                owd.setSize(800, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("Controls/Document/ChangeRequestAttachDocument.aspx?objId=" + objId, "ETRMAttachFile");
            }

            function ShowAllAttachFile(objId) {
                var owd = $find("<%=ETRMAttachFile.ClientID %>");
                owd.setSize(800, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("Controls/Document/NCRSIAttachDocument.aspx?objId=" + objId, "ETRMAttachFile");
            }

            function ShowIncomingTransAttachFile(objId) {
                var owd = $find("<%=ETRMAttachFile.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("PECC2TransETRMAttach.aspx?objId=" + objId, "ETRMAttachFile");
            }

            function AttachWorkflow(objId) {
                var owd = $find("<%=AttachWorkflow.ClientID %>");
                 var projId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                 owd.setSize(700, document.documentElement.offsetHeight);
                 owd.Show();
                 owd.setUrl("../../Controls/Workflow/AttachWorkflow.aspx?objId=" + objId + "&projId=" + projId + "&type=3&typename=NCR/SI", "AttachWorkflow");
            }

            function CreateOutgoingTrans(Id, ForSenID) {
                var objId = document.getElementById("<%= lblObjId.ClientID %>").value;
                var owd = $find("<%=CreateOutgoingTrans.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("Controls/Document/ContractorTransmittalEditForm.aspx?changeRequestId=" + objId + "&forSent=" + ForSenID, "CreateOutgoingTrans");
            }

            function SendTrans(objId) {
                if (confirm("Do you want to send this NCR/SI to PECC2?") === false) return;
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


            function refreshOutgoingGrid() {
                var masterTable = $find("<%=grdNCRSI.ClientID%>").get_masterTableView();
                masterTable.rebind();
            }

            function refreshAllGrid() {
                var masterTable = $find("<%=grdNCRSI.ClientID%>").get_masterTableView();
                masterTable.rebind();
            }

            function refreshAttachDocGrid() {
                var masterTable = $find("<%=grdNCRSI.ClientID%>").get_masterTableView();
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
                if (strText.toLowerCase() == "update") {
                    ajaxManager.ajaxRequest("UpdatePackageStatus");
                }

                //Download multi documents Export template data file Attach multi document file
                ////if (strText == "View explorer") {
                ////    window.open("file://WIN-P7KS57HL1HG/DocumentLibrary");
                ////}

                if (strText === "NCR/SI") {
                    var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                    var owd = $find("<%=CustomerDialog.ClientID %>");
                    owd.setSize(1000, document.documentElement.offsetHeight);
                    owd.Show();
                    owd.setUrl("Controls/Document/NCRSIEditForm.aspx?projectId=" + projectId, "CustomerDialog");
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