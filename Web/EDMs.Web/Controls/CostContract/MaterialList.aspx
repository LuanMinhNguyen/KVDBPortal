<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MaterialList.aspx.cs" Inherits="EDMs.Web.Controls.CostContract.MaterialList" EnableViewState="true" %>

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
        .rtPlus, .rtMinus
        {
            background-image: url('../../Images/plus-minus-icon1.png') !important;
			cursor: pointer;
        }

        .rtIn {
            cursor: pointer;
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

        /*div.RadComboBox .rcbInputCell .rcbInput
        {
            padding-left: 22px;
        }*/
    </style>
    <div style="width: 98%; padding-top: 10px; padding-left: 5px">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/project.png" /> Selected project:<br />
        
        <telerik:RadComboBox ID="ddlProject" runat="server" 
            Skin="Windows7" Width="100%" AutoPostBack="True" 
            OnItemDataBound="ddlProject_ItemDataBound"
            OnSelectedIndexChanged="ddlProject_SelectedIndexChanged"/>
        <br />
        <hr/>
        Procurement Requirement:
        <telerik:RadTreeView ID="rtvPR" runat="server" 
            Width="100%" Height="100%" ShowLineImages="False"
            OnNodeClick="rtvPR_NodeClick" 
            OnNodeDataBound="rtvPR_NodeDataBound"
            OnClientNodeClicking="rtvPR_ClientNodeClicking">
        </telerik:RadTreeView>
    </div>
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Horizontal">
        <telerik:RadPane ID="RadPane3" runat="server" Height="30px" Scrollable="false" Scrolling="None">
            
            <telerik:RadToolBar ID="CustomerMenu" runat="server" Width="100%" OnClientButtonClicking="OnClientButtonClicking">
                <Items>
                    <telerik:RadToolBarDropDown runat="server" Text="Add" ImageUrl="~/Images/addNew.png">
                        <Buttons>
                            <telerik:RadToolBarButton runat="server" Text="Material" Value="1" ImageUrl="~/Images/material.png"></telerik:RadToolBarButton>
                            <%--<telerik:RadToolBarButton runat="server" Text="Multi documents" Value="2" ImageUrl="~/Images/addmulti.png"></telerik:RadToolBarButton>--%>
                        </Buttons>
                    </telerik:RadToolBarDropDown>
                    <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                    
                    <telerik:RadToolBarDropDown runat="server" Text="Action" ImageUrl="~/Images/action.png" Visible="False">
                        <Buttons>
                            <telerik:RadToolBarButton runat="server" Text="Send mail" Value="3" ImageUrl="~/Images/email.png" Visible="False"/>
                            <telerik:RadToolBarButton runat="server" Text="Download multi documents" Value="4" ImageUrl="~/Images/download.png" Visible="False"/>
                            <telerik:RadToolBarButton runat="server" IsSeparator="true" />
                            <telerik:RadToolBarButton runat="server" Text="Export master list template" Value="Adminfunc" ImageUrl="~/Images/export.png"/>
                            <telerik:RadToolBarButton runat="server" Text="Import document master list" Value="Adminfunc" ImageUrl="~/Images/import.png" />
                            <telerik:RadToolBarButton runat="server" IsSeparator="true"  Value="Adminfunc"/>
                            <telerik:RadToolBarButton runat="server" Text="Export EMDR report" Value="7" ImageUrl="~/Images/emdrreport.png"/>
                            <telerik:RadToolBarButton runat="server" Text="Import EMDR report" Value="Adminfunc" ImageUrl="~/Images/upload.png" />
                            <telerik:RadToolBarButton runat="server" IsSeparator="true" Visible="False"/>
                            
                            <telerik:RadToolBarButton runat="server" Text="Attach multi document file" Value="Adminfunc" ImageUrl="~/Images/uploadmulti.png" Visible="False"/>
                            <telerik:RadToolBarButton runat="server" IsSeparator="true" Value="Adminfunc" />
                            <telerik:RadToolBarButton runat="server" Text="Share documents" Value="Adminfunc" ImageUrl="~/Images/share.png" />
                            <telerik:RadToolBarButton runat="server" IsSeparator="true" Value="Adminfunc"/>
                            <telerik:RadToolBarButton runat="server" Text="Delete all selected documents" Value="Adminfunc" ImageUrl="~/Images/delete.png" />
                        </Buttons>
                    </telerik:RadToolBarDropDown>
                    
                    <telerik:RadToolBarButton runat="server" IsSeparator="true" Visible="False"/>
                    <telerik:RadToolBarButton runat="server" Value="ShowAll" Visible="False">
                        <ItemTemplate>
                            <asp:CheckBox ID="ckbShowAll" runat="server" Text="Show all Document revision." AutoPostBack="True" OnCheckedChanged="ckbShowAll_CheckedChange"/>
                        </ItemTemplate>
                    </telerik:RadToolBarButton>

                    <telerik:RadToolBarButton runat="server" Visible="False">
                        <ItemTemplate>
                            Work status: Complete 
                            <telerik:radnumerictextbox type="Percent" id="txtDisciplineComplete" runat="server" Width="60px" CssClass="min25Percent" ReadOnly="True">
                                <NumberFormat DecimalDigits="2"></NumberFormat>
                            </telerik:radnumerictextbox>
                            | Weight 
                            <telerik:radnumerictextbox type="Percent" id="txtDisciplineWeight" runat="server" Width="60px" CssClass="min25Percent" ReadOnly="True">
                                <NumberFormat DecimalDigits="2"></NumberFormat>
                            </telerik:radnumerictextbox>
                        </ItemTemplate>
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton ImageUrl="~/Images/save.png" Text="Update" Value="search" CommandName="UpdatePackageStatus" Visible="False" />
                    
                    <telerik:RadToolBarDropDown runat="server" Text="Clear" ImageUrl="~/Images/clearEMDR.png" Visible="False">
                        <Buttons>
                            <telerik:RadToolBarButton runat="server" Text="Clear EMDR data" Value="12" ImageUrl="~/Images/clearEMDR.png" />
                        </Buttons>
                    </telerik:RadToolBarDropDown>
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
                                Height="100%" ID="grdDocument"  AllowFilteringByColumn="True" AllowMultiRowSelection="False"
                                OnDeleteCommand="grdDocument_DeleteCommand" 
                                OnItemCommand="grdDocument_ItemCommand" 
                                OnItemDataBound="grdDocument_ItemDataBound" 
                                OnNeedDataSource="grdDocument_OnNeedDataSource" 
                                OnUpdateCommand="grdDocument_UpdateCommand"
                                PageSize="100" runat="server" Style="outline: none" Width="100%">
                                <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
                                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                                <MasterTableView AllowMultiColumnSorting="false"
                                    ClientDataKeyNames="ID" DataKeyNames="ID" CommandItemDisplay="Top" 
                                    EditMode="InPlace" Font-Size="8pt">
                                    <%--<GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldAlias="_" FieldName="ProcurementRequirementNumber" FormatString="{0:D}"
                                                        HeaderValueSeparator=""></telerik:GridGroupByField>
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldName="ProcurementRequirementNumber" SortOrder="Ascending" ></telerik:GridGroupByField>
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>
                                        </GroupByExpressions> --%>   
                                    <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="false"/>
                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ColumnGroups>
                                        <telerik:GridColumnGroup HeaderText="Contract Value (Not include 10% VAT)" Name="ContractValue"
                                             HeaderStyle-HorizontalAlign="Center"/>
                                        <telerik:GridColumnGroup HeaderText="Payment Value" Name="Payment"
                                             HeaderStyle-HorizontalAlign="Center"/>
                                        <telerik:GridColumnGroup HeaderText="Remain Value" Name="Remain"
                                             HeaderStyle-HorizontalAlign="Center"/>
                                    </ColumnGroups>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                                        <telerik:GridBoundColumn DataField="HasAttachFile" UniqueName="HasAttachFile" Display="False" />
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

                                        <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="EditColumn" Display="False">
                                                <HeaderStyle Width="30"  />
                                                <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <a href='javascript:ShowEditForm(<%# DataBinder.Eval(Container.DataItem, "ID") %>, <%# DataBinder.Eval(Container.DataItem, "ProjectId") %>)' style="text-decoration: none; color:blue">
                                                    <asp:Image ID="EditLink" runat="server" ImageUrl="~/Images/edit.png" Style="cursor: pointer;" AlternateText="Edit properties" />
                                                        <a/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                    <telerik:GridButtonColumn Display="False" UniqueName="DeleteColumn" CommandName="Delete" HeaderTooltip="Delete document"
                                            ConfirmText="Do you want to delete document?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                                            <HeaderStyle Width="25" />
                                            <ItemStyle HorizontalAlign="Center" Width="25"  />
                                        </telerik:GridButtonColumn>
                                            
                                    <telerik:GridTemplateColumn HeaderText="Contract Number" UniqueName="ContractNumber"
                                        DataField="ContractNumber" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="160" />
                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocNo" runat="server" Text='<%# Eval("ContractNumber") %>' style="cursor: pointer; color: green"/> 
                                            <telerik:RadToolTip Skin="Simple" runat="server" ID="dirNameToolTip" RelativeTo="Element" Title="Contract Information"
                                            AutoCloseDelay="10000" ShowDelay="0" Position="BottomRight"
                                            Width="400px" Height="150px" HideEvent="LeaveTargetAndToolTip"  TargetControlID="lblDocNo" IsClientID="False"
                                            Animation="Fade" Text='<%# "<b>Number:</b> " + Eval("ContractObj.Number") + "<br/>" +
                                                                       "<b>Content:</b> " + (!string.IsNullOrEmpty(Eval("ContractObj.ContractContent").ToString()) 
                                                                                        ? Eval("ContractObj.ContractContent").ToString().Replace("\n", "<br/>")
                                                                                        : string.Empty) + "<br/>"
                                                                    + "<b>Contractor:</b> " + Eval("ContractObj.ContractorSelectedName") + "<br/>"
                                                                    + "<b>Delivery Date:</b> " + Eval("ContractObj.DeliveryDate","{0:dd/MM/yyyy}") + "<br/>"
                                                                    + "<b>Delivery Status:</b> " + Eval("ContractObj.DeliveryStatus") + "<br/>"
                                                                    %>'>
                                         </telerik:RadToolTip>

                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                            
                                    <telerik:GridTemplateColumn HeaderText="Material Number" UniqueName="Number"
                                        DataField="Number" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="160" />
                                        <ItemStyle HorizontalAlign="Left"/>
                                        <ItemTemplate>
                                            <%# Eval("Number") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="Quality" UniqueName="Quality"
                                        AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# Eval("Quality") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                    <telerik:GridTemplateColumn HeaderText="Delivery Plan" UniqueName="DeliveryPlan"
                                        AllowFiltering="false" >
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemTemplate>
                                            <%# Eval("DeliveryPlan","{0:dd/MM/yyyy}") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Actual" UniqueName="DeliveryActual"
                                        AllowFiltering="false" >
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemTemplate>
                                            <%# Eval("DeliveryActual","{0:dd/MM/yyyy}") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Delivery Status" UniqueName="DeliveryStatus"
                                        AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="180" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# Eval("DeliveryStatus") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Note" UniqueName="Note"
                                        AllowFiltering="false" >
                                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("Note") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Complete - %" UniqueName="Complete" AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="65" />
                                        <ItemStyle HorizontalAlign="Center" Width="65" />
                                        <ItemTemplate>
                                            <%# Eval("Complete")!=null && Convert.ToDouble(Eval("Complete")) != 0 ?  Eval("Complete") + "%" : "-"%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Weight - %" UniqueName="Weight" AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="65" />
                                        <ItemStyle HorizontalAlign="Center" Width="65" />
                                        <ItemTemplate>
                                            <%# Eval("Weight")!=null ?  Eval("Weight") + "%" : "-"%>
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
            <telerik:RadMenuItem Text="Edit properties" ImageUrl="~/Images/edit.png" Value="EditProperties"/>
            <telerik:RadMenuItem Text="Share Control" ImageUrl="~/Images/sharecontrol.png" Value="ShareControl" Visible="False"/>
            <telerik:RadMenuItem IsSeparator="True"/>
            <telerik:RadMenuItem Text="Attach Files" ImageUrl="~/Images/attach.png" Value="AttachDocument" Visible="False"/>
            <telerik:RadMenuItem Text="Material List" ImageUrl="~/Images/material.png" Value="MaterialList" Visible="False"/>
        </Items>
    </telerik:RadContextMenu>

    <span style="display: none">
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rtvPR">
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
                        <telerik:AjaxUpdatedControl ControlID="rtvPR" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="CustomerMenu" LoadingPanelID="RadAjaxLoadingPanel2"/>
                      
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlProject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChildProject" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="rtvPR" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="CustomerMenu"/>
                        <telerik:AjaxUpdatedControl ControlID="lblProjectId"/>

                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ChildProject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvPR" LoadingPanelID="RadAjaxLoadingPanel2"/>
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
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Material Information"
                VisibleStatusbar="false" Height="700" Width="650" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="ShareControl" runat="server" Title="Share Control"
                VisibleStatusbar="false" Height="600" Width="700" 
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
            
            <telerik:RadWindow ID="AttachDoc" runat="server" Title="Attach Contract files"
                VisibleStatusbar="false" Height="500" Width="700" MinHeight="500" MinWidth="700" MaxHeight="500" MaxWidth="700" 
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
                var contractId = document.getElementById("<%= lblDocId.ClientID %>").value;
                var isFullPermission = document.getElementById("<%= IsFullPermission.ClientID %>").value;

                var packageId = document.getElementById("<%= lblDisciplineId.ClientID %>").value;
                
                
                switch (itemValue) {
                
                case "AttachDocument":
                    var owd = $find("<%=AttachDoc.ClientID %>");
                    owd.Show();
                    owd.setUrl("AttachContractDocument.aspx?contractId=" + contractId, "AttachDoc");
                    break;
                
                case "EditProperties":
                    var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                    var owd = $find("<%=CustomerDialog.ClientID %>");
                    owd.Show();
                    owd.setUrl("MaterialEditForm.aspx?materialId=" + contractId + "&projId=" + projectId, "CustomerDialog");
                    break;
                    case "ShareControl":
                    var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                    var owd = $find("<%=ShareControl.ClientID %>");
                    owd.Show();
                    owd.setUrl("PermissionContractPage.aspx?contractId=" + contractId + "&projId=" + projectId, "ShareControl");
                    break;
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
                    //menu.get_allItems()[0].enable();
                    //menu.get_allItems()[1].enable();
                    //menu.get_allItems()[3].enable();
                    //menu.get_allItems()[4].enable();
                    //menu.get_allItems()[6].enable();
                }
                else {
                    //menu.get_allItems()[0].disable();
                    //menu.get_allItems()[1].disable();
                    //menu.get_allItems()[3].enable();
                    //menu.get_allItems()[4].enable();
                    //menu.get_allItems()[6].enable();
                }
            }
        </script>
        <script type="text/javascript">
            /* <![CDATA[ */
            var toolbar;
            var searchButton;
            var ajaxManager;

            function pageLoad() {
                toolbar = $find("<%= CustomerMenu.ClientID %>");
                ajaxManager = $find("<%=ajaxCustomer.ClientID %>");
            }

            function ShowEditForm(docId, projId) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("MaterialEditForm.aspx?projId=" + projId + "&docId=" + docId, "CustomerDialog");
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
            
            function rtvPR_ClientNodeClicking(sender, args) {
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
                
                if (strText.toLowerCase() == "import document master list") {
                    var owd = $find("<%=ImportData.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/Document/ImportDocMasterList.aspx", "ImportData");
                }

                if (strText.toLowerCase() == "import emdr report") {
                    var owd = $find("<%=ImportEMDRReport.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/Document/ImportEMDR.aspx", "ImportEMDRReport");
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
                        owd.setUrl("Controls/Document/SendMail.aspx?listDoc=" + listId, "SendMail");
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
                        owd.setUrl("Controls/Document/ShareDocument.aspx?listDoc=" + listId, "ShareDocument");
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

                if (strText.toLowerCase() == "export master list template") {
                    ajaxManager.ajaxRequest("ExportMasterList");
                }

                if (strText.toLowerCase() == "export emdr report") {
                    ajaxManager.ajaxRequest("ExportEMDRReport_New");
                }

                if (strText.toLowerCase() == "attach multi document file") {
                    var owd = $find("<%=AttachMulti.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/Document/UploadMultiDocumentFile.aspx", "AttachMulti");
                }
                
                if (strText.toLowerCase() == "update") {
                    ajaxManager.ajaxRequest("UpdatePackageStatus");
                }

                //Download multi documents Export template data file Attach multi document file
                ////if (strText == "View explorer") {
                ////    window.open("file://WIN-P7KS57HL1HG/DocumentLibrary");
                ////}

                if (strText.toLowerCase() == "material") {
                    
                    <%--var selectedDiscipline = document.getElementById("<%= lblDisciplineId.ClientID %>").value;
                    if (selectedDiscipline == "") {
                        alert("Please choice one Discipline to add new Document.");
                        return false;
                    }
                    else {--%>
                    var projectId = document.getElementById("<%= lblProjectId.ClientID %>").value;
                    var owd = $find("<%=CustomerDialog.ClientID %>");
                    owd.Show();
                    owd.setUrl("MaterialEditForm.aspx?projId=" + projectId, "CustomerDialog");
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
                    owd.setUrl("Controls/Document/UploadDragDrop.aspx?folId=" + selectedFolder, "UploadMulti");
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