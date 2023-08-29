
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkflowDetailPage.aspx.cs" Inherits="EDMs.Web.Controls.Workflow.WorkflowDetailPage" %>

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
        <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="False"
            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
            GridLines="None" Skin="Windows7" Height="100%"
            OnNeedDataSource="grdDocument_OnNeedDataSource" 
            OnBatchEditCommand="grdDocument_OnBatchEditCommand"
            OnPreRender="grdDocument_OnPreRender"
            PageSize="10" Style="outline: none; overflow: hidden !important;">
            <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%" CommandItemDisplay="TopAndBottom" EditMode="Batch">
                <CommandItemSettings ShowAddNewRecordButton="False"></CommandItemSettings>
                <BatchEditingSettings OpenEditingEvent="Click" EditType="Row"></BatchEditingSettings>
                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Contents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                <Columns>
                    <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                    
                    <telerik:GridBoundColumn DataField="CurrentWorkflowStepName" HeaderText="Name" SortExpression="CurrentWorkflowStepName"
                        UniqueName="CurrentWorkflowStepName" ReadOnly="True">
                        <HeaderStyle Width="250px" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" Font-Bold="True"/>
                    </telerik:GridBoundColumn>
                    
                    <telerik:GridTemplateColumn HeaderText="Apply For Action" UniqueName="ActionApplyName"
                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="ActionApplyName"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <%# Eval("ActionApplyName").ToString().Replace(Environment.NewLine, "<br/>") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Duration (Days)" SortExpression="Duration" UniqueName="Duration"
                        DataField="Duration">
                        <HeaderStyle Width="80px" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center"/>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblDuration" Text='<%# Eval("Duration") != null && Convert.ToDouble(Eval("Duration")) != 0 ? Eval("Duration", "{0:N}") : "-" %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <span>
                                <telerik:RadNumericTextBox Width="100%" runat="server" ID="txtDuration"/>
                            </span>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    <%--<telerik:GridCheckBoxColumn DataField="IsOnlyWorkingDay" HeaderStyle-Width="80px" HeaderText="Is Working Day" SortExpression="IsOnlyWorkingDay"
                        UniqueName="IsOnlyWorkingDay">
                        <ItemStyle HorizontalAlign="Center"/>
                    </telerik:GridCheckBoxColumn>--%>
                    
                    
                    

                    <telerik:GridTemplateColumn HeaderText="Accept Step" UniqueName="NextWorkflowStepID" DataField="NextWorkflowStepID">
                        <HeaderStyle Width="250" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left"/>
                        <ItemTemplate>
                            <%# Eval("NextWorkflowStepName") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadComboBox runat="server" ID="ddlAcceptStep" Width="100%" Skin="Windows7">
                            </telerik:RadComboBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    <telerik:GridTemplateColumn HeaderText="Reject Step" UniqueName="RejectWorkflowStepID" DataField="RejectWorkflowStepID">
                        <HeaderStyle Width="250" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left"/>
                        <ItemTemplate>
                            <%# Eval("RejectWorkflowStepName") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadComboBox runat="server" ID="ddlRejectStep" Width="100%" Skin="Windows7">
                            </telerik:RadComboBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    <telerik:GridTemplateColumn DataField="Recipients" HeaderText="Recipients" SortExpression="Recipients"
                        UniqueName="Recipients" ReadOnly="True">
                        <HeaderStyle Width="250" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right"/>
                        <ItemTemplate>
                            <%# Eval("Recipients") %>
                            <a href='javascript:ShowRecipientsForm(<%# DataBinder.Eval(Container.DataItem, "ID") %>,<%# "\"" + DataBinder.Eval(Container.DataItem, "CurrentWorkflowStepName") + "\""%>)' style="text-decoration: none; color:blue">
                                <asp:Image ID="Recipients" runat="server" ImageUrl="~/Images/group.png" Style="cursor: pointer;" AlternateText="Recipients" />
                            <a/>
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
            <telerik:RadWindow ID="Recipients" runat="server" Title="Recipients Information"
                VisibleStatusbar="false" Left="150px"
                ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" OnClientClose="refreshGrid"
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
                    owd.SetTitle("Recipients Information for WF step " + stepName);
                    owd.setSize(850, document.documentElement.offsetHeight);
                    owd.Show();
                    //owd.maximize(true);
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
