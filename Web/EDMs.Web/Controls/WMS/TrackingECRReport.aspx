﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrackingECRReport.aspx.cs" Inherits="EDMs.Web.Controls.WMS.TrackingECRReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.7.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        
        function CloseAndRefreshGrid() {
            var oWin = GetRadWindow();
            var parentWindow = oWin.BrowserWindow;
            $(oWin).ready(function () {
                oWin.close();
            });
            parentWindow.refreshGrid();
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including classic dialog
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz as well)

            return oWindow;
        }


    </script>
    <style type="text/css">
        .RadGrid .rgRow td, .RadGrid .rgAltRow td, .RadGrid .rgEditRow td, .RadGrid .rgFooter td, .RadGrid .rgFilterRow td, .RadGrid .rgHeader, .RadGrid .rgResizeCol, .RadGrid .rgGroupHeader td {
            padding-left: 1px !important;
            padding-right: 1px !important;
        }

        /*Hide change page size control*/
        div.RadGrid .rgPager .rgAdvPart     
        {     
        display:none;        
        }    

        /*#grdDocument_GridData {
            height: 100% !important;
        }*/
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="content">
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <telerik:RadGrid AllowCustomPaging="False" AllowPaging="False" AllowSorting="True" 
                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                ID="grdDocument" Skin="Windows7"  AllowFilteringByColumn="True"
                OnNeedDataSource="grdDocument_OnNeedDataSource" 
                OnDetailTableDataBind="grdDocument_DetailTableDataBind"
                PageSize="100" runat="server"  Width="100%">
                <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                <MasterTableView AllowMultiColumnSorting="false" 
                    ClientDataKeyNames="ID" DataKeyNames="ID" Font-Size="8pt">
                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                        
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldAlias="Department" FieldName="DepartmentName" FormatString="{0:D}"
                                    HeaderValueSeparator=": "></telerik:GridGroupByField>
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="DepartmentName" SortOrder="Ascending" ></telerik:GridGroupByField>
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>    
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                       <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="APPROVAL STATUS" Name="ApprovalStatus"
                                    HeaderStyle-HorizontalAlign="Center"/>
                            <telerik:GridColumnGroup HeaderText="Action Plan" Name="AP"
                                    HeaderStyle-HorizontalAlign="Center"/>
                        </ColumnGroups>
                                    
                        <DetailTables>
                            <telerik:GridTableView DataKeyNames="ID" Name="DocDetail" Width="900px"
                                AllowPaging="True" PageSize="10">
                                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                <Columns>
                                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                                    <HeaderStyle Width="3%" />
                                    <ItemStyle HorizontalAlign="Center" Width="3%"/>
                                    <ItemTemplate>
                                        <a href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' 
                                            download='<%# DataBinder.Eval(Container.DataItem, "FileName") %>' target="_blank">
                                            <asp:Image ID="Image1" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ExtensionIcon") %>'
                                                Style="cursor: pointer;" ToolTip="Download document" /> 
                                        </a>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                            
                                <telerik:GridBoundColumn DataField="FileName" HeaderText="File name" UniqueName="FileName">
                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left"/>
                                </telerik:GridBoundColumn>
                                        
                                <telerik:GridBoundColumn DataField="Description" HeaderText="Description" UniqueName="Description">
                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="CreatedByName" HeaderText="Upload by" UniqueName="CreatedByName">
                                    <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Upload time" UniqueName="CreatedDate"
                                    DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="FileSize" HeaderText="File size(Kb)" UniqueName="FileSize" DataFormatString="{0:0,0.00}">
                                    <HeaderStyle HorizontalAlign="Center" Width="13%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                </Columns>
                            </telerik:GridTableView>
                        </DetailTables>

                        <Columns>
                                <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                                <telerik:GridTemplateColumn HeaderText="No." Groupable="False">
                                    <HeaderStyle HorizontalAlign="Center" Width="2%" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="2%"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSoTT" runat="server" Text='<%# grdDocument.CurrentPageIndex * grdDocument.PageSize + grdDocument.Items.Count+1 %>'>
                                        </asp:Label>
                                      
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Code" HeaderText="Code" UniqueName="Code" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridBoundColumn>

                                    <telerik:GridTemplateColumn UniqueName="Title" HeaderText="ECR Title" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle Width="350" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left"/>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                            
                                    <telerik:GridTemplateColumn  DataField="Description" HeaderText="ECR Description" UniqueName="Description" 
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="300" />
                                        <ItemStyle HorizontalAlign="Left"  />
                                        <ItemTemplate>
                                            <%# Eval("Description").ToString().Replace(Environment.NewLine, "<br/>") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                            
                                    <telerik:GridDateTimeColumn HeaderText="Date Raised" UniqueName="DateRaised" DataField="DateRaised"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>
                                            
                                    <telerik:GridBoundColumn DataField="PriorityName" HeaderText="Priority Level" UniqueName="PriorityName" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="Section1Name" HeaderText="SECTION 1" UniqueName="Section1Name" ColumnGroupName="ApprovalStatus" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridBoundColumn>
                                            
                                    <telerik:GridBoundColumn DataField="Section2Name" HeaderText="SECTION 2" UniqueName="Section2Name" ColumnGroupName="ApprovalStatus" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridBoundColumn>
                                            
                                    <telerik:GridBoundColumn DataField="Section3Name" HeaderText="SECTION 3" UniqueName="Section3Name" ColumnGroupName="ApprovalStatus" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridBoundColumn>
                                            
                                    <telerik:GridBoundColumn DataField="Section4Name" HeaderText="SECTION 4" UniqueName="Section4Name" ColumnGroupName="ApprovalStatus" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridBoundColumn>
                                            
                                    <telerik:GridBoundColumn DataField="Section5Name" HeaderText="SECTION 5" UniqueName="Section5Name" ColumnGroupName="ApprovalStatus" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridBoundColumn>
                                            
                                    <telerik:GridBoundColumn DataField="ApSection3Name" HeaderText="SECTION 3" UniqueName="ApSection3Name" ColumnGroupName="AP" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridBoundColumn>
                                            
                                    <telerik:GridBoundColumn DataField="ApRequirementName" HeaderText="Requirements" UniqueName="ApRequirementName" ColumnGroupName="AP" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="90"/>
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridBoundColumn>

                                    <telerik:GridTemplateColumn  DataField="ExecutionStatus" HeaderText="EXECUTION STATUS" UniqueName="ExecutionStatus" 
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                                        <ItemStyle HorizontalAlign="Left"  />
                                        <ItemTemplate>
                                            <%# Eval("ExecutionStatus").ToString().Replace(Environment.NewLine, "<br/>") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                            
                                    <telerik:GridTemplateColumn  DataField="PersonInCharge" HeaderText="PERSON IN CHARGE" UniqueName="PersonInCharge" 
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                                        <ItemStyle HorizontalAlign="Left"  />
                                        <ItemTemplate>
                                            <%# Eval("PersonInCharge").ToString().Replace(Environment.NewLine, "<br/>") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                            
                                    <telerik:GridBoundColumn DataField="StatusName" HeaderText="ECR Current Status" UniqueName="StatusName" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridBoundColumn>
                                            
                                    <telerik:GridTemplateColumn HeaderText="Cost" UniqueName="Cost" AllowFiltering="false" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Right"  />
                                        <ItemTemplate>
                                            <%# Eval("Cost", "{0: ###,##0.##}")%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn  DataField="Remark" HeaderText="Remarks" UniqueName="Remark" 
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="300" />
                                        <ItemStyle HorizontalAlign="Left"  />
                                        <ItemTemplate>
                                            <%# Eval("Remark").ToString().Replace(Environment.NewLine, "<br/>") %>
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
                    <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                    </ClientSettings>
            </telerik:RadGrid>
            
            <telerik:GridDropDownListColumnEditor ID="ddlRevision" runat="server"
                    DropDownStyle-Width="110px">
            </telerik:GridDropDownListColumnEditor>

        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
            

            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
            <Windows>
                
            <telerik:RadWindow ID="AttachDoc" runat="server" Title="Attach document files" 
                VisibleStatusbar="false" Height="500" Width="500" MinHeight="500" MinWidth="500" MaxHeight="500" MaxWidth="500" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
        </div>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                
                function RowDblClick(sender, eventArgs) {
                    sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
                }

                var ajaxManager;

                function pageLoad() {
                ajaxManager = $find("<%=ajaxCustomer.ClientID %>");
            }

                function onRequestStart(sender, args) {
                    if (args.get_eventTarget().indexOf("btnDownloadPackage") >= 0) {
                        args.set_enableAjax(false);
                    }
                }

                function ShowUploadForm(id) {
                    var owd = $find("<%=AttachDoc.ClientID %>");
                    owd.Show();
                    owd.setUrl("UploadDragDrop.aspx?docId=" + id, "AttachDoc");
                    // window.parent.radopen("Controls/Document/DocumentInfoEditForm.aspx?docId=" + id, "DocDialog");
                    // return false;
                }
                
                function ShowInsertForm() {
                    window.radopen("Controls/Customers/CustomerEditForm.aspx", "DocDialog");
                    return false;
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
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
