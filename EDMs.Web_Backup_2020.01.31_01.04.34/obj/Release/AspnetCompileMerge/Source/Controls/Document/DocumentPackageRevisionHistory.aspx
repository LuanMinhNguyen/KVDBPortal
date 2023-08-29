<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentPackageRevisionHistory.aspx.cs" Inherits="EDMs.Web.Controls.Document.DocumentPackageRevisionHistory" %>

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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="content">
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
                            <telerik:GridColumnGroup HeaderText="REVISION DETAILS" Name="RevisionDetails"
                                    HeaderStyle-HorizontalAlign="Center"/>
                            <telerik:GridColumnGroup HeaderText="OUTGOING TRANSMITTAL" Name="OutgoingTrans"
                                    HeaderStyle-HorizontalAlign="Center"/>
                            <telerik:GridColumnGroup HeaderText="INCOMING TRANSMITTAL" Name="IncomingTrans"
                                    HeaderStyle-HorizontalAlign="Center"/>
                            <telerik:GridColumnGroup HeaderText="ICA REVIEW DETAILS" Name="ICAReviews"
                                    HeaderStyle-HorizontalAlign="Center"/>
                        </ColumnGroups>
                                    
                        <DetailTables>
                            <telerik:GridTableView DataKeyNames="ID" Name="DocDetail" Width="100%"
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
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="Description" HeaderText="Notes" UniqueName="Description">
                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="CmtResFrom" HeaderText="From" UniqueName="CmtResFrom">
                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="CmtResTo" HeaderText="To" UniqueName="CmtResTo">
                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="AttachTypeName" HeaderText="Type" UniqueName="AttachTypeName">
                                    <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                    <ItemStyle HorizontalAlign="Left" Width="9%" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="CreatedByUser" HeaderText="Upload by" UniqueName="CreatedByUser">
                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Upload time" UniqueName="CreatedDate"
                                    DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                    <HeaderStyle HorizontalAlign="Center" Width="16%" />
                                    <ItemStyle HorizontalAlign="Left" Width="16%" />
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
                            <telerik:GridTemplateColumn HeaderText="DOC. No." UniqueName="DocNo">
                                <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                <ItemStyle HorizontalAlign="Left" Width="8%" />
                                <ItemTemplate>
                                    <%# Eval("DocNo") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="DocNo" runat="server" Value='<%# Eval("DocNo") %>'/>
                                    <asp:Label runat="server" ID="lbldocNo"></asp:Label>
                                    <%--<asp:TextBox ID="txtDocNo" runat="server" Width="100%"></asp:TextBox>--%>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                                            
                            <telerik:GridTemplateColumn HeaderText="DOC. Title" UniqueName="DocTitle"
                                        DataField="DocTitle" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="220" />
                                        <ItemStyle HorizontalAlign="Left" Width="220" />
                                        <ItemTemplate>
                                            <%# Eval("DocTitle") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="Rev." UniqueName="Rev"
                                        AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="50" />
                                        <ItemStyle HorizontalAlign="Center" Width="50"/>
                                        <ItemTemplate>
                                            <%# Eval("RevisionName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                    <telerik:GridTemplateColumn HeaderText="Status" UniqueName="StatusName"
                                        AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="100" />
                                        <ItemStyle HorizontalAlign="Center"  />
                                        <ItemTemplate>
                                            <%# Eval("StatusName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Critical" UniqueName="IsCriticalDoc" AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="50" />
                                        <ItemStyle HorizontalAlign="Center"   />
                                        <ItemTemplate>
                                            <%# Convert.ToBoolean(Eval("IsCriticalDoc")) ? "x" : string.Empty %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                    <telerik:GridTemplateColumn HeaderText="Priority" UniqueName="IsPriorityDoc" AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="50" />
                                        <ItemStyle HorizontalAlign="Center"   />
                                        <ItemTemplate>
                                            <%# Convert.ToBoolean(Eval("IsPriorityDoc")) ? "x" : string.Empty %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Vendor" UniqueName="IsVendorDoc" AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="50" />
                                        <ItemStyle HorizontalAlign="Center"   />
                                        <ItemTemplate>
                                            <%# Convert.ToBoolean(Eval("IsVendorDoc")) ? "x" : string.Empty %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Reference From" UniqueName="ReferenceFromName" AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="60" />
                                        <ItemStyle HorizontalAlign="Center"   />
                                        <ItemTemplate>
                                            <%# Eval("ReferenceFromName")%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                    <telerik:GridTemplateColumn HeaderText="Planed" UniqueName="RevisionPlanedDate"
                                        AllowFiltering="false" ColumnGroupName="RevisionDetails">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemTemplate>
                                            <%# Eval("RevisionPlanedDate","{0:dd/MM/yyyy}") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Actual" UniqueName="RevisionActualDate"
                                         AllowFiltering="false" ColumnGroupName="RevisionDetails">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" Width="80"/>
                                        <ItemTemplate>
                                            <%# Eval("RevisionActualDate","{0:dd/MM/yyyy}") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                    <telerik:GridTemplateColumn HeaderText="Trans No." UniqueName="RevisionReceiveTransNo"
                                         AllowFiltering="false" ColumnGroupName="RevisionDetails">
                                        <HeaderStyle HorizontalAlign="Center" Width="40" />
                                        <ItemStyle HorizontalAlign="Center" Width="40"/>
                                        <ItemTemplate>
                                            <%# Eval("RevisionReceiveTransNo") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                    <telerik:GridTemplateColumn HeaderText="Final Code" UniqueName="FinalCodeName"
                                         AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="40" />
                                        <ItemStyle HorizontalAlign="Center" Width="40"/>
                                        <ItemTemplate>
                                            <%# Eval("FinalCodeName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                    <telerik:GridTemplateColumn HeaderText="Complete - %" UniqueName="Complete" AllowFiltering="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="65" />
                                        <ItemStyle HorizontalAlign="Center" Width="65" />
                                        <ItemTemplate>
                                            <%# Eval("Complete")!=null ?  Eval("Complete") + "%" : "-"%>
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
                    <ClientEvents  OnGridCreated="GetGridObject" />
                    <ClientEvents OnRowContextMenu="RowContextMenu" OnRowClick="RowClick"></ClientEvents>
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
