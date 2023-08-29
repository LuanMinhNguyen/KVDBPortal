
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ObjectWorkflowDetailPage.aspx.cs" Inherits="EDMs.Web.Controls.Workflow.ObjectWorkflowDetailPage" %>

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
          .RadButton {
              margin-left: 10px !important;
          } 
    </style>

    <script src="../../Scripts/jquery-1.7.1.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        function CloseAndRebind(args) {
            GetRadWindow().BrowserWindow.refreshGrid(args);
            GetRadWindow().close();
        }
        function CloseWindow() {
            window.close();
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
        function showWin() {
            $find("RadWindow1").show();
        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow)
                oWindow = window.radWindow;
            else if (window.frameElement && window.frameElement.radWindow)
                oWindow = window.frameElement.radWindow;
            return oWindow;
        }
        function CloseModal(args) {
            var oWnd = GetRadWindow();
            if (oWnd) {
                oWnd.BrowserWindow.refreshGrid(args);
                setTimeout(function () { oWnd.close(); }, 0);
            }
        }

            </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
         <div style="width: 100%; height:50px; margin-top:20px;" runat="server">
      
            <label style="width: 75px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                <span style="color: #2E5689; text-align: right; ">Workflow
                </span>
            </label>
                     
                <asp:TextBox ID="txtWorkflowName" runat="server" Style="width: 390px; float:left; margin-left:5px;" CssClass="min25Percent" ReadOnly="True"/>

              <telerik:RadButton ID="btnSave" runat="server" Text="    Attach To Workflow"  OnClick="btnSave_Click" Width="130px" SingleClick="true" SingleClickText="Submitting...">
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
             
             
              <telerik:RadButton ID="btnReload" runat="server" Text="Reload Workflow"  OnClick="btnReload_Click" Width="130px" SingleClick="true">
                  <Icon PrimaryIconUrl="../../Images/refresh.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>

              <telerik:RadButton ID="RadButton1" runat="server" Text="Close" OnClientClicked="CancelEdit" Width="70px" style="text-align: center">
                 <Icon PrimaryIconUrl="../../Images/Cancel.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16" ></Icon>
             </telerik:RadButton>
 
            <div style="float: left; margin-left :20px; padding-top: 5px; " class="qlcbFormItem" runat="server" id="divMessage" >
                <asp:Label runat="server" ID="lblError" Width="300px"></asp:Label>
            </div>
             </div>
         <div style="width: 100%; height:89%;margin:3px; margin-right:3px; margin-bottom:3px;" runat="server">
        <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="False"
            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
            GridLines="None" Skin="Office2007" Height="100%"
            OnNeedDataSource="grdDocument_OnNeedDataSource" 
            OnBatchEditCommand="grdDocument_OnBatchEditCommand"
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
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="false">
                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <%# Eval("ActionApplyName").ToString().Replace(Environment.NewLine, "<br/>") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Duration (Days)" SortExpression="Duration" UniqueName="Duration"
                        DataField="Duration" ReadOnly="False">
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
                    

                    <telerik:GridTemplateColumn HeaderText="Accept Step" UniqueName="NextWorkflowStepID" DataField="NextWorkflowStepID" ReadOnly="true" Display="false">
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
                    
                    <telerik:GridTemplateColumn HeaderText="Reject Step" UniqueName="RejectWorkflowStepID" DataField="RejectWorkflowStepID" ReadOnly="true" Display="false">
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
                </Columns>
            </MasterTableView>
            <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True" >
                <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="200" UseStaticHeaders="True" />
                <ClientEvents />
            </ClientSettings>
        </telerik:RadGrid>
        </div>
         <%--<div style="width: 100%; text-align: center; padding-top: 270px">
        </div>--%>
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        <asp:HiddenField runat="server" ID="ObjectID"/>
        
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument" OnAjaxRequest="ajaxDocument_AjaxRequest">
           <%-- <ClientEvents OnResponseEnd ="EndResponse" />--%>
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="grdDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument"/>
                        <telerik:AjaxUpdatedControl ControlID="ajaxDocument" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSave">
                    <UpdatedControls>
                          <telerik:AjaxUpdatedControl ControlID="divMessage" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnReload">
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
                ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" OnClientClose="refreshGrid">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;
                 function pageLoad() {
                    ajaxManager = $find("<%=ajaxDocument.ClientID %>");
                }
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
                    owd.setSize(860, document.documentElement.offsetHeight);
                    owd.Show();
                    //owd.maximize(true);
                    //owd.setUrl("RecipientPage.aspx?wfdId=" + wfDetailId +"&Temple=True", "Recipients");
                    owd.setUrl("RecipientPage.aspx?wfdId=" + wfDetailId + "&customize=True", "Recipients");
                }

                function refreshGrid() {
                    var masterTable = $find("<%=grdDocument.ClientID%>").get_masterTableView();
                    masterTable.rebind();
                }
                //function EndResponse(sender, args) {
                //    GetRadWindow().BrowserWindow.refreshGrid(args);
                //    GetRadWindow().close();
                //}
          </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
