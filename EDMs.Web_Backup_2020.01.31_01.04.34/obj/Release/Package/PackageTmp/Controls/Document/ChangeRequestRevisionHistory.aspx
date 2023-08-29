<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeRequestRevisionHistory.aspx.cs" Inherits="EDMs.Web.Controls.Document.ChangeRequestRevisionHistory" %>

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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 100%; height:100%;">
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <telerik:RadGrid AllowCustomPaging="False" AllowPaging="True" AllowSorting="True" 
                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                ID="grdDocument" Skin="Windows7" Height="100%"
                OnDeleteCommand="grdDocument_DeleteCommand" 
                OnNeedDataSource="grdDocument_OnNeedDataSource" 
                OnDetailTableDataBind="grdDocument_DetailTableDataBind"
                PageSize="100" runat="server"  Width="100%">
                <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                <MasterTableView AllowMultiColumnSorting="false" 
                    ClientDataKeyNames="ID" DataKeyNames="ID" Font-Size="8pt">
                        <%--<GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldAlias="-" FieldName="DocumentTypeName" FormatString="{0:D}"
                                            HeaderValueSeparator=""></telerik:GridGroupByField>
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="DocumentTypeName" SortOrder="Ascending" ></telerik:GridGroupByField>
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>  --%>  
                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ColumnGroups>
                           <%-- <telerik:GridColumnGroup HeaderText="First Issue Info" Name="FirstIssueInfo"
                                    HeaderStyle-HorizontalAlign="Center"/>
                            <telerik:GridColumnGroup HeaderText="Final Issue Info" Name="FinalIssueInfo"
                                    HeaderStyle-HorizontalAlign="Center"/>
                            <telerik:GridColumnGroup HeaderText="INCOMING TRANSMITTAL" Name="IncomingTrans"
                                    HeaderStyle-HorizontalAlign="Center"/>
                            <telerik:GridColumnGroup HeaderText="ICA REVIEW DETAILS" Name="ICAReviews"
                                    HeaderStyle-HorizontalAlign="Center"/>--%>
                        </ColumnGroups>
                                    
                        <DetailTables>
                            <telerik:GridTableView DataKeyNames="ID" Name="DocDetail" Width="700px"
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
                                        <HeaderStyle HorizontalAlign="Center" Width="35%" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                    
                                    <telerik:GridBoundColumn DataField="TypeName" HeaderText="Type" UniqueName="TypeName" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left"/>
                                    </telerik:GridBoundColumn>
                                
                                    <telerik:GridBoundColumn DataField="CreatedByName" HeaderText="Upload by" UniqueName="CreatedByName">
                                        <HeaderStyle HorizontalAlign="Center" Width="35%" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                
                                    <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Upload time" UniqueName="CreatedDate"
                                        DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridBoundColumn>
                                
                                    <telerik:GridBoundColumn DataField="FileSize" HeaderText="File size(Kb)" UniqueName="FileSize" DataFormatString="{0:0,0.00}">
                                        <HeaderStyle HorizontalAlign="Center" Width="13%" />
                                        <ItemStyle HorizontalAlign="Center"/>
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
                                <%--<telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="~/Images/edit.png" 
                                    UpdateImageUrl="~/Images/ok.png" CancelImageUrl="~/Images/delete.png" UniqueName="EditColumn">
                                    <HeaderStyle HorizontalAlign="Center" Width="2%"  />
                                    <ItemStyle HorizontalAlign="Center" Width="2%"/>
                                </telerik:GridEditCommandColumn>--%>
                            <%--<telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" HeaderTooltip="Delete document"
                                    ConfirmText="Do you want to delete document?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                                    <HeaderStyle Width="1%" />
                                        <ItemStyle HorizontalAlign="Center" Width="1%"  />
                                </telerik:GridButtonColumn>
                                --%>
                               
                                    <telerik:GridBoundColumn HeaderText="Review Result" UniqueName="ReviewResultName" DataField="ReviewResultName"
                                ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Center" />
                            </telerik:GridBoundColumn>
                                
                        <telerik:GridBoundColumn HeaderText="Outgoing Trans No." UniqueName="OutgoingTransNo" DataField="OutgoingTransNo"
                                ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Center" />
                            </telerik:GridBoundColumn>
                                
                        <telerik:GridTemplateColumn HeaderText="PECC2 Review Result" UniqueName="PECC2ReviewResultName" DataField="PECC2ReviewResultName"
                                                    ShowFilterIcon="False" FilterControlWidth="97%" 
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                            <HeaderStyle HorizontalAlign="Center" Width="100" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# Eval("PECC2ReviewResultName") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Change Request Number" UniqueName="Number" DataField="Number"
                                    ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="120" />
                                <ItemStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                        <%# Eval("Number") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Revision" UniqueName="MajorRev"
                                                    ShowFilterIcon="False" FilterControlWidth="97%"  DataField="Revision"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                            <HeaderStyle HorizontalAlign="Center" Width="50" />
                            <ItemStyle HorizontalAlign="Center" Width="50"/>
                            <ItemTemplate>
                                <%# Eval("Revision") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                
                            <telerik:GridTemplateColumn HeaderText="Description" UniqueName="Description" DataField="Description"
                                ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                <HeaderStyle HorizontalAlign="Center" Width="180" />
                                <ItemStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%# Eval("Description") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                    
                            <telerik:GridTemplateColumn Display="False" HeaderText="Confidentiality" UniqueName="ConfidentialityName"
                                ShowFilterIcon="False" FilterControlWidth="97%"  DataField="ConfidentialityName"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("ConfidentialityName") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                                <telerik:GridTemplateColumn Display="False" HeaderText="Area" UniqueName="AreaCode" DataField="AreaCode"
                                ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="50" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("AreaCode") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                                <telerik:GridTemplateColumn Display="False" HeaderText="Unit" UniqueName="UnitCode" DataField="UnitCode"
                                ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="50" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("UnitCode") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                                <telerik:GridTemplateColumn Display="False" HeaderText="Change Request Type" UniqueName="TypeName"
                                ShowFilterIcon="False" FilterControlWidth="97%"  DataField="TypeName"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("TypeName") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                                <telerik:GridTemplateColumn Display="False" HeaderText="Group" UniqueName="GroupName"
                                ShowFilterIcon="False" FilterControlWidth="97%"  DataField="GroupName"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="50" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("GroupName") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                                <telerik:GridTemplateColumn Display="False" HeaderText="Year" UniqueName="Year"
                                ShowFilterIcon="False" FilterControlWidth="97%"  DataField="Year"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="50" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("Year") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                                <telerik:GridTemplateColumn Display="False" HeaderText="Sequential Number" UniqueName="SequentialNumber"
                                ShowFilterIcon="False" FilterControlWidth="97%"  DataField="SequentialNumber"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("SequentialNumber") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Reason For Change" UniqueName="ReasonForChange"
                                ShowFilterIcon="False" FilterControlWidth="97%"  DataField="ReasonForChange"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="180" />
                                <ItemStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%#Eval("ReasonForChange") != null ? Eval("ReasonForChange").ToString().Replace(Environment.NewLine, "<br/>") : string.Empty%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                                <telerik:GridTemplateColumn HeaderText="Existing Condition" UniqueName="ExistingCondition"
                                ShowFilterIcon="False" FilterControlWidth="97%"  DataField="ExistingCondition"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="180" />
                                <ItemStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%# Eval("ExistingCondition") != null ? Eval("ExistingCondition").ToString().Replace(Environment.NewLine, "<br/>") : string.Empty
                                                %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                                <telerik:GridDateTimeColumn HeaderText="Issued Date" UniqueName="IssuedDate" DataField="IssuedDate"
                                    DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" >
                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                <ItemStyle HorizontalAlign="Center"/>
                            </telerik:GridDateTimeColumn> 
                                        
                                    <telerik:GridDateTimeColumn HeaderText="Closed Date" UniqueName="ClosedDate" DataField="ClosedDate"
                                    DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" >
                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                <ItemStyle HorizontalAlign="Center"/>
                            </telerik:GridDateTimeColumn> 

                                <telerik:GridTemplateColumn HeaderText="Code" UniqueName="ChangeGradeCodeName" DataField="ChangeGradeCodeName"
                                ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="50" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("ChangeGradeCodeName") %>
                                </ItemTemplate>
                                </telerik:GridTemplateColumn> 
                                        
                                <telerik:GridTemplateColumn HeaderText="Status" UniqueName="Status" DataField="Status"
                                ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="60" />
                                <ItemStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%# Eval("Status") %>
                                </ItemTemplate>
                                </telerik:GridTemplateColumn> 
                                        
                                    <telerik:GridTemplateColumn HeaderText="Referenced/Revised Document Number" UniqueName="RefDocNo" DataField="RefDocNo"
                                ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                <HeaderStyle HorizontalAlign="Center" Width="200" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%#Eval("RefDocNo") != null ? Eval("RefDocNo").ToString().Replace(Environment.NewLine, "<br/>") : string.Empty%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                                        
                                        
                                <telerik:GridTemplateColumn HeaderText="PECC2 Review Result" UniqueName="PECC2ReviewResultName" DataField="PECC2ReviewResultName"
                                ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("PECC2ReviewResultName") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                                <telerik:GridTemplateColumn HeaderText="Owner Review Result" UniqueName="OwnerReviewResultName" DataField="OwnerReviewResultName"
                                ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("OwnerReviewResultName") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn> 
                                        
                                <telerik:GridTemplateColumn HeaderText="Document To Be Revised" UniqueName="DocToBeRevisedNo" DataField="DocToBeRevisedNo"
                                ShowFilterIcon="False" FilterControlWidth="97%" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                <HeaderStyle HorizontalAlign="Center" Width="200" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%#Eval("DocToBeRevisedNo") != null ? Eval("DocToBeRevisedNo").ToString().Replace(Environment.NewLine, "<br/>") : string.Empty%>
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
