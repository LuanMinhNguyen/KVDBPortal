
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ObjectWorkflowPage.aspx.cs" Inherits="EDMs.Web.Controls.Workflow.ObjectWorkflowPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
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
            background-color: darkseagreen !important;
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
        <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="True"
                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                GridLines="None" Height="100%" Skin="Windows7"
                OnItemDataBound="grdDocument_ItemDataBound"
                OnDeleteCommand="grdDocument_DeleteCommand" 
                OnItemCreated="grdDocument_ItemCreated"
                OnNeedDataSource="grdDocument_OnNeedDataSource" 
                PageSize="100" Style="outline: none">
                <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%" TableLayout="Auto" CssClass="rgMasterTable">
                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; items." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />

                        <telerik:GridTemplateColumn Display="False">
                            <HeaderStyle Width="30"  />
                            <ItemStyle HorizontalAlign="Center"/>
                            <ItemTemplate>
                                <asp:Image ID="EditLink" runat="server" ImageUrl="~/Images/edit.png" Style="cursor: pointer;" ToolTip="Edit properties" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Do you want to delete item?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                            <HeaderStyle Width="30" />
                                <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                                            
                        <%-- <telerik:GridTemplateColumn>
                            <HeaderStyle Width="30"  />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <a href='javascript:ShowStep(<%# DataBinder.Eval(Container.DataItem, "ID") %>)' style="text-decoration: none; color:blue">
                                <asp:Image runat="server" ImageUrl="~/Images/workflow1.png" Style="cursor: pointer;" ToolTip="Step definition" />
                                    </a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                                            
                        <telerik:GridTemplateColumn>
                            <HeaderStyle Width="30"  />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <a href='javascript:ShowWorkflowDetail(<%# DataBinder.Eval(Container.DataItem, "ID") %>)' style="text-decoration: none; color:blue">
                                <asp:Image runat="server" ImageUrl="~/Images/workflowDetail.png" Style="cursor: pointer;" ToolTip="Workflow Details" />
                                    </a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                                            
                        <%--3--%>
                        <telerik:GridTemplateColumn UniqueName="Name" HeaderText=" Workflow Name">
                            <HeaderStyle Width="200" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left"/>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="Description" HeaderText="Description" UniqueName="Description">
                            <HeaderStyle HorizontalAlign="Center" Width="200" />
                            <ItemStyle HorizontalAlign="Left"/>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Re_assignUserName" HeaderText="Re-assign User" UniqueName="Re_assignUserName">
                            <HeaderStyle HorizontalAlign="Center" Width="150" />
                            <ItemStyle HorizontalAlign="Left"  />
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                </ClientSettings>
        </telerik:RadGrid>
        
         <%--<div style="width: 100%; text-align: center; padding-top: 270px">
        </div>--%>
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        <asp:HiddenField runat="server" ID="DocId"/>
        
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
                function ShowEditForm(ID) {
                    var docid=document.getElementById("<%=DocId.ClientID %>").value;
                    window.open("../../Controls/Workflow/ObjectWorkflowDetailEdit.aspx?ObjId=" + docid + "&wfId=" + ID, "_blank");
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
