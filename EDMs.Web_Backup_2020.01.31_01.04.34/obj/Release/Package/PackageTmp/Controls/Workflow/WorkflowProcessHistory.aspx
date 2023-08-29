
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkflowProcessHistory.aspx.cs" Inherits="EDMs.Web.Controls.Workflow.WorkflowProcessHistory" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        
        div.RadGrid .rgPager .rgAdvPart     
        {     
            display:none;
        }
        div.rgDataDiv {
            overflow: auto !important;
        }

        .DropZone1
        {
            width: 300px;
            height: 250px;
            padding-left: 230px;
            background: #fff url(../../Images/placeholder-add.png) no-repeat center center;
            background-color: #357A2B;
            border-color: #CCCCCC;
            color: #767676;
            float: left;
            text-align: center;
            font-size: 16px;
            color: white;
            position: relative;
        }
        #btnSavePanel {
            display: inline !important;
        }
        .RadAjaxPanel {
            height: 100% !important;
        }

        .RadGrid .rgSelectedRow
        {
            background-image : none !important;
            background-color: darkseagreen !important;
        }
         table.gridtable {
          font-family: verdana,arial,sans-serif;
          font-size:11px;
          color:#333333;
          border-width: 1px;
          border-color: #666666;
          border-collapse: collapse;
          }
          table.gridtable th {
          border-width: 1px;
          padding: 6px;
          border-style: solid;
          border-color: #666666;
          background-color: #dedede;
          }
          table.gridtable td {
          font-size:11px;
          border-width: 1px;
          padding: 3px;
          border-style: solid;
          border-color: #666666;
          background-color: #ffffff;
          }

          .leftth{
          font-family: verdana,arial,sans-serif;
          font-size:11px;
          border-width: 1px;
          padding: 5px;
          border-style: solid;
          border-color: #666666;
          background-color: #dedede;
          font-weight:bold;
          }
          .rightbody {
          font-family: verdana,arial,sans-serif;
          font-size:11px;
          border-width: 1px;
          padding: 5px;
          border-style: solid;
          border-color: #666666;
          background-color: #ffffff;
          }  
    </style>

    <script src="../../Scripts/jquery-1.7.1.js" type="text/javascript"></script>
    
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
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
        <div style="height:70px; border-width:1px; margin:3px; display: none">
            <table class="gridtable"><tr><td colspan="16" style="border-bottom:0;"><u><b>Step Status Legend</b></u><br /><br /></td></tr><tr><td style="border-top:0;border-bottom:0;" /><td style="background-color:#CCFFCC; width:15px;">SO</td><td nowrap="nowrap">Completed(Signed Off)</td><td style="border-top:0;border-bottom:0;" /><td style="background-color:#FF99CC; width:15px;">RJ</td><td nowrap="nowrap">Rejected/Unable to Complete</td><td style="border-top:0;border-bottom:0;" /><td style="background-color:#FFFF80; width:15px;">RS</td><td nowrap="nowrap">Ready to Start</td><td style="border-top:0;border-bottom:0;" /><td style="background-color:#ffffff; width:15px;">NR</td><td nowrap="nowrap">Not Ready</td><td style="border-top:0;border-bottom:0;" /></tr><tr><td colspan="16" style="border-top:0;"><br /></td></tr></table>
        </div>
          <div style="width: 100%; height:89%;margin:3px; margin-right:3px; margin-bottom:3px;" runat="server">
        <telerik:RadGrid AllowCustomPaging="False" AllowPaging="False" AllowSorting="True" 
            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" Skin="Windows7"
            Height="100%" ID="grdDocument"  AllowFilteringByColumn="True" AllowMultiRowSelection="False"
            OnNeedDataSource="grdDocument_OnNeedDataSource" 
            OnItemDataBound="grdDocument_OnItemDataBound"
            PageSize="100" runat="server" Style="outline: none" Width="100%">
            <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
            <GroupingSettings CaseSensitive="False"></GroupingSettings>
            <MasterTableView AllowMultiColumnSorting="false"
                ClientDataKeyNames="ID" DataKeyNames="ID" CommandItemDisplay="Top" 
                EditMode="InPlace" Font-Size="8pt">
                <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField HeaderText="Workflow" FieldName="WorkflowName" FormatString="{0:D}"
                                    HeaderValueSeparator=": "></telerik:GridGroupByField>
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="WorkflowName" SortOrder="Ascending" ></telerik:GridGroupByField>
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
                    <telerik:GridBoundColumn DataField="ObjectID" UniqueName="ObjectID" Display="False" />
                    <telerik:GridBoundColumn DataField="Status" UniqueName="ST" Display="False" />
                    <telerik:GridBoundColumn DataField="IsDistributeOnshore" UniqueName="IsDistributeOnshore" Display="False" />
                    <telerik:GridBoundColumn DataField="IsOnshoreComment" UniqueName="IsOnshoreComment" Display="False" />
                    <telerik:GridBoundColumn DataField="ActionTypeId" UniqueName="ActionTypeId" Display="False" />
                                        
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
                                        
                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="ObjectStatus" HeaderText="Is Reject">
                        <HeaderStyle Width="40"  />
                        <ItemStyle HorizontalAlign="Center"/>
                        <ItemTemplate>
                            <asp:Image ID="imgDocStatusReject" runat="server" Visible ='<%# Eval("IsReject")%>' ImageUrl="~/Images/checkred.png" Style="cursor: pointer;" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="IsOverdue" HeaderText="Is Overdue">
                        <HeaderStyle Width="60"  />
                        <ItemStyle HorizontalAlign="Center"/>
                        <ItemTemplate>
                            <asp:Image ID="imgDocStatus" runat="server" Visible ='<%# Eval("IsOverDue")%>' ImageUrl="~/Images/checkred.png" Style="cursor: pointer;" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                    
                <telerik:GridTemplateColumn HeaderText="Workflow" UniqueName="WorkflowName"
                    AllowFiltering="false" Display="false">
                    <HeaderStyle HorizontalAlign="Center" Width="150" />
                    <ItemStyle HorizontalAlign="Left"  />
                    <ItemTemplate>
                        <%# Eval("WorkflowName") %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                                        
                <telerik:GridTemplateColumn HeaderText="Workflow Step" UniqueName="CurrentWorkflowStepName"
                    AllowFiltering="false">
                    <HeaderStyle HorizontalAlign="Center" Width="150" />
                    <ItemStyle HorizontalAlign="Left"  />
                    <ItemTemplate>
                        <%# Eval("CurrentWorkflowStepName") %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
  
                    <telerik:GridTemplateColumn AllowFiltering="False" DataField="Status" UniqueName="Status" HeaderText="Status" Display="False">
                        <HeaderStyle Width="60"  />
                        <ItemStyle HorizontalAlign="Center"/>
                        <ItemTemplate>
                            <%# Eval("Status") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Object Type" UniqueName="ObjectType"
                    AllowFiltering="false" Display="false" >
                    <HeaderStyle HorizontalAlign="Center" Width="100" />
                    <ItemStyle HorizontalAlign="Left"  />
                    <ItemTemplate>
                        <%# Eval("ObjectType") %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderText="Object Number" UniqueName="ObjectNumber"
                    DataField="ObjectNumber" ShowFilterIcon="False" FilterControlWidth="97%"  Display="false"
                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                    <HeaderStyle HorizontalAlign="Center" Width="180" />
                    <ItemStyle HorizontalAlign="Center"/>
                    <ItemTemplate>
                        <asp:Label ID="lblDocNo" runat="server" Text='<%# Eval("ObjectNumber") %>' style="cursor: pointer"/> 
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                                            
                <telerik:GridTemplateColumn HeaderText="Object Title" UniqueName="ObjectTitle"
                    DataField="ObjectTitle" ShowFilterIcon="False" FilterControlWidth="97%"  Display="false"
                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                    <HeaderStyle HorizontalAlign="Center" Width="180" />
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <%# Eval("ObjectTitle") %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                    
                    <telerik:GridTemplateColumn HeaderText="Revision" UniqueName="Revision"
                    DataField="Revision" ShowFilterIcon="False" FilterControlWidth="97%" 
                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="false">
                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <%# Eval("Revision") %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                    
                    <telerik:GridTemplateColumn HeaderText="Assign User" UniqueName="AssignUser"
                    DataField="UserFullName" ShowFilterIcon="False" FilterControlWidth="97%" 
                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                    <HeaderStyle HorizontalAlign="Center" Width="200" />
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <%# Eval("UserFullName") %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn> 
                
                <telerik:GridTemplateColumn HeaderText="Action" UniqueName="ActionTypeName"
                    DataField="ActionTypeName" ShowFilterIcon="False" FilterControlWidth="97%" 
                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                    <HeaderStyle HorizontalAlign="Center" Width="120" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <%# Eval("ActionTypeName") %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn HeaderText="Status" UniqueName="Status"
                    DataField="Status" ShowFilterIcon="False" FilterControlWidth="97%"  Visible="false"
                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <%# Eval("Status") %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                                       
                    <telerik:GridTemplateColumn HeaderText="Message" UniqueName="CommentContent"
                    DataField="CommentContent" ShowFilterIcon="False" FilterControlWidth="97%" 
                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                    <HeaderStyle HorizontalAlign="Center" Width="200" />
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <%# Eval("CommentContent") %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                        
                <telerik:GridTemplateColumn HeaderText="Received Date" UniqueName="ReceivedDate"
                    AllowFiltering="false" ColumnGroupName="RevisionDetails" >
                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                    <ItemStyle HorizontalAlign="Center" Width="80"/>
                    <ItemTemplate>
                        <%# Eval("ReceivedDate","{0:dd/MM/yyyy HH:mm:ss}") %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                                        
                    <telerik:GridTemplateColumn HeaderText="Deadline Date" UniqueName="PlanCompleteDate"
                    AllowFiltering="false" ColumnGroupName="RevisionDetails" >
                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                    <ItemStyle HorizontalAlign="Center" Width="80"/>
                    <ItemTemplate>
                        <%# Eval("PlanCompleteDate","{0:dd/MM/yyyy HH:mm:ss}") %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                    
                    <telerik:GridTemplateColumn HeaderText="Actual Date" UniqueName="ActualDate"
                    AllowFiltering="false" ColumnGroupName="RevisionDetails" >
                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                    <ItemStyle HorizontalAlign="Center" Width="80"/>
                    <ItemTemplate>
                        <%# Eval("ActualDate","{0:dd/MM/yyyy HH:mm:ss}") %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                    
                <telerik:GridTemplateColumn HeaderText="IsComplete" UniqueName="IsComplete" AllowFiltering="False">
                        <HeaderStyle Width="80" HorizontalAlign="Center" ></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <%# Eval("WorkingStatus") %>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/ok.png" Visible='<%# Convert.ToBoolean(Eval("IsComplete")) && string.IsNullOrEmpty(Eval("WorkingStatus").ToString()) %>'/>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                    
            </Columns>
        </MasterTableView>
        <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
        <ClientEvents ></ClientEvents>
        <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
        </ClientSettings>
    </telerik:RadGrid>
        </div>
         <%--<div style="width: 100%; text-align: center; padding-top: 270px">
        </div>--%>
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument" OnAjaxRequest="ajaxDocument_AjaxRequest">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="grdDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" Skin="Windows7">
        <Windows>
            <telerik:RadWindow ID="Recipients" runat="server" Title="Recipients Information"
                VisibleStatusbar="false" Height="600" Width="840" Left="150px"
                ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true"
                >
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;
                function OnBatchEditOpening(sender, args) {

                    switch (args.get_columnUniqueName()) {
                        case "CurrentWorkflowStepName":
                            args.get_cell().disable = true;
                        break;
                    default:
                    }
                }

                function ShowRecipientsForm(wfDetailId, stepName) {
                    var owd = $find("<%=Recipients.ClientID %>");
                    owd.SetTitle("Recipients Information for WF step " + stepName );
                    owd.Show();
                    owd.setUrl("RecipientPage.aspx?wfdId=" + wfDetailId, "Recipients");
                }

                function refreshGrid() {
                    var masterTable = $find("<%=grdDocument.ClientID%>").get_masterTableView();
                    masterTable.rebind();
                }
          </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
