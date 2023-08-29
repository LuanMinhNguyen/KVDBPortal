
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResponseManagementPage.aspx.cs" Inherits="EDMs.Web.Controls.Workflow.ResponseManagementPage" %>

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
        <div style="margin-bottom: 3px; margin-top: 3px">
            <telerik:RadButton ID="btnSave" runat="server" Text="Complete Tasks" OnClientClicked="OnClientButtonClicking" Width="140px" style="text-align: center" AutoPostBack="False">
                <Icon PrimaryIconUrl="~/Images/complete.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
            </telerik:RadButton>
        </div>
        <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="False"
            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" AllowMultiRowSelection="True"
            GridLines="None" Skin="Windows7" Height="100%"
            OnNeedDataSource="grdDocument_OnNeedDataSource" 
            PageSize="10" Style="outline: none; overflow: hidden !important;">
            <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%">
                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Contents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                <Columns>
                    <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                            <telerik:GridBoundColumn DataField="CanReject" UniqueName="CanReject" Display="False" />
                            <telerik:GridBoundColumn DataField="ObjectType" UniqueName="ObjectTypeEdit" Display="False" />
                            <telerik:GridBoundColumn DataField="ObjectID" UniqueName="ObjectID" Display="False" />
                            <telerik:GridBoundColumn DataField="Status" UniqueName="ST" Display="False" />
                            <telerik:GridBoundColumn DataField="IsDistributeOnshore" UniqueName="IsDistributeOnshore" Display="False" />
                            <telerik:GridBoundColumn DataField="IsOnshoreComment" UniqueName="IsOnshoreComment" Display="False" />
                                        
                            <telerik:GridClientSelectColumn UniqueName="IsSelected" Display="True">
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
                                        
                            <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="ObjectStatus" HeaderText="Is Reject">
                                <HeaderStyle Width="40"  />
                                <ItemStyle HorizontalAlign="Center"/>
                                <ItemTemplate>
                                    <asp:Image ID="imgDocStatusReject" runat="server" Visible ='<%# Eval("IsReject")%>' ImageUrl="~/Images/checkred.png" Style="cursor: pointer;" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                    
                            <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="IsOverdue" HeaderText="Is Overdue" Display="False">
                                <HeaderStyle Width="60"  />
                                <ItemStyle HorizontalAlign="Center"/>
                                <ItemTemplate>
                                    <asp:Image ID="imgDocStatus" runat="server" Visible ='<%# Eval("IsOverDue")%>' ImageUrl="~/Images/checkred.png" Style="cursor: pointer;" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                    
                        <telerik:GridTemplateColumn HeaderText="Workflow" UniqueName="WorkflowName"
                            AllowFiltering="false">
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
                            AllowFiltering="false" >
                            <HeaderStyle HorizontalAlign="Center" Width="100" />
                            <ItemStyle HorizontalAlign="Left"  />
                            <ItemTemplate>
                                <%# Eval("ObjectType") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Object Number" UniqueName="ObjectNumber"
                            DataField="ObjectNumber" ShowFilterIcon="False" FilterControlWidth="97%" 
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                            <HeaderStyle HorizontalAlign="Center" Width="110" />
                            <ItemStyle HorizontalAlign="Center"/>
                            <ItemTemplate>
                                <asp:Label ID="lblDocNo" runat="server" Text='<%# Eval("ObjectNumber") %>' style="cursor: pointer"/> 
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                                            
                        <telerik:GridTemplateColumn HeaderText="Object Title" UniqueName="ObjectTitle"
                            DataField="ObjectTitle" ShowFilterIcon="False" FilterControlWidth="97%" 
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                            <HeaderStyle HorizontalAlign="Center" Width="250" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%# Eval("ObjectTitle") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    
                            <telerik:GridTemplateColumn HeaderText="Revision" UniqueName="Revision"
                            DataField="Revision" ShowFilterIcon="False" FilterControlWidth="97%" 
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
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
                            AllowFiltering="false" ColumnGroupName="RevisionDetails" Display="False">
                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                            <ItemStyle HorizontalAlign="Center" Width="80"/>
                            <ItemTemplate>
                                <%# Eval("ActualDate","{0:dd/MM/yyyy HH:mm:ss}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    
                        <telerik:GridTemplateColumn HeaderText="Status" UniqueName="IsComplete" AllowFiltering="False" Display="False">
                                <HeaderStyle Width="80" HorizontalAlign="Center" ></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <%# Eval("WorkingStatus") %>
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/ok.png" Visible='<%# Convert.ToBoolean(Eval("IsComplete")) && string.IsNullOrEmpty(Eval("WorkingStatus").ToString()) %>'/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
            <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True" >
                <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="200" UseStaticHeaders="True" />
                <ClientEvents />
            </ClientSettings>
        </telerik:RadGrid>
        
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
            <telerik:RadWindow ID="Recipients" runat="server" Title="Complete Tasks"
                VisibleStatusbar="True" Left="150px"
                ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" OnClientClose="refreshGrid"
                >
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;

                function OnClientButtonClicking() {
                    var grid = $find("<%= grdDocument.ClientID %>");
                    var masterTable = grid.get_masterTableView();
                    var selectedRows = masterTable.get_selectedItems();
                    var listId = "";
                    if (selectedRows.length == 0) {
                        alert("Please select Tasks to complete.");
                    }
                    else {
                        for (var i = 0; i < selectedRows.length; i++) {
                            var row = selectedRows[i];
                            listId += row.getDataKeyValue("ID") + "_";
                        }

                        var owd = $find("<%=Recipients.ClientID %>");
                        owd.setSize(730, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("CompleteMoveNextMultiTask.aspx?curentAssignUserIds=" + listId, "Recipients");
                        //alert("1");
                    }
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
