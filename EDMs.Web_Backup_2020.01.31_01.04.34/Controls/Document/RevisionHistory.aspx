<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RevisionHistory.aspx.cs" Inherits="EDMs.Web.Controls.Document.RevisionHistory" %>

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
            <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="True"
                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                 GridLines="None" Skin="Windows7"
                OnDataBound="grdDocument_DataBound"
                OnDeleteCommand="grdDocument_DeleteCommand"
                
                OnItemDataBound="grdDocument_ItemDataBound"
                OnNeedDataSource="grdDocument_OnNeedDataSource" PageSize="100" Style="outline: none">
                <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%" EditMode="InPlace" Font-Size="8pt">
                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                        <telerik:GridBoundColumn DataField="DisciplineID" UniqueName="DisciplineID" Visible="False" />
                        <telerik:GridBoundColumn DataField="FilePath" UniqueName="FilePath" Visible="False" />
                        <%--<telerik:GridTemplateColumn UniqueName="IsSelected" AllowFiltering="false">
                            <HeaderStyle Width="2%"  />
                            <ItemStyle HorizontalAlign="Center" Width="2%"/>
                            <ItemTemplate>
                                <asp:CheckBox ID="IsSelected" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>

                        <%--<telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="~/Images/edit.png" 
                            UpdateImageUrl="~/Images/ok.png" CancelImageUrl="~/Images/delete.png" UniqueName="EditColumn">
                            <HeaderStyle Width="4%"  />
                            <ItemStyle HorizontalAlign="Center" Width="4%"/>
                        </telerik:GridEditCommandColumn>--%>

                        <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete"  HeaderTooltip="Delete document"
                             ConfirmText="Do you want to delete document?" 
                            ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                            <HeaderStyle Width="1%"  />
                                <ItemStyle HorizontalAlign="Center" Width="1%"   />
                        </telerik:GridButtonColumn>

                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn" HeaderTooltip="Download document package" Visible="False">
                            <HeaderStyle Width="1%" />
                            <ItemStyle HorizontalAlign="Center" Width="1%"/>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDownloadPackage" runat="server" Visible='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "AttachFileCount"))  > 0 %>'
                                    OnClick="btnDownload_Click"
                                    ImageUrl="~/Images/download.png" ToolTip="Download document package"
                                    Style="cursor: pointer;" AlternateText="Download document"/> 
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                                            
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadDefault" HeaderTooltip="Download default document file">
                            <HeaderStyle Width="1%"/>
                            <ItemStyle HorizontalAlign="Center" Width="1%"/>
                            <ItemTemplate>
                                <a href='<%# DataBinder.Eval(Container.DataItem, "DefaultDoc") != null ? DataBinder.Eval(Container.DataItem, "DefaultDoc.FilePath") : string.Empty %>' target="_blank">
                                    <asp:Image ID="ImageButton1" runat="server" ToolTip="Download default document file"
                                        Visible='<%# DataBinder.Eval(Container.DataItem, "DefaultDoc") != null %>'
                                        ImageUrl='<%# DataBinder.Eval(Container.DataItem, "DefaultDoc") != null ? DataBinder.Eval(Container.DataItem, "DefaultDoc.ExtensionIcon") : string.Empty %>' 
                                        Style="cursor: pointer;" AlternateText="Download document"/> 
                                </a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                                            
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="AttachColumn" HeaderTooltip="Attach document files">
                            <HeaderStyle Width="1%"/>
                            <ItemStyle HorizontalAlign="Center" Width="1%"/>
                            <ItemTemplate>
                                <a href='javascript:ShowUploadForm(<%# DataBinder.Eval(Container.DataItem, "ID") %>)' style="text-decoration: none; color:blue">
                                
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/attach.png" ToolTip="Attach document files"
                                        Style="cursor: pointer;" AlternateText="Attach document file" /> 
                                </a>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--3--%>
                        <telerik:GridTemplateColumn UniqueName="Index1" HeaderText="Name" 
                            DataField="Name" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                            <HeaderStyle Width="10%" HorizontalAlign="Center"/>
                            <ItemStyle HorizontalAlign="Left" Width="10%"/>
                            <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'></asp:Label>
                                    <asp:Image ID="newicon" runat="server" ImageUrl="~/Images/new.png" Visible='<%# (DateTime.Now - Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "CreatedDate"))).TotalHours < 24 %>' /> 
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="Name" runat="server" Value='<%# Eval("Name") %>'/>
                                                    
                                <asp:TextBox ID="txtName" runat="server" Width="100%"></asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                                            
                        <telerik:GridBoundColumn DataField="Description" HeaderText="Description" UniqueName="Index2">
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="RevName" HeaderText="Rev" UniqueName="Index3">
                            <HeaderStyle HorizontalAlign="Center" Width="3%" />
                            <ItemStyle HorizontalAlign="Center" Width="3%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor Name" UniqueName="Index4">
                            <HeaderStyle HorizontalAlign="Center" Width="8%" />
                            <ItemStyle HorizontalAlign="Left" Width="8%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="DrawingNumber" HeaderText="Drawing Number" UniqueName="Index5">
                            <HeaderStyle HorizontalAlign="Center" Width="6%" />
                            <ItemStyle HorizontalAlign="Left" Width="6%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="Year" HeaderText="Year" UniqueName="Index6">
                            <HeaderStyle HorizontalAlign="Center" Width="4%" />
                            <ItemStyle HorizontalAlign="Center" Width="4%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="PlantName" HeaderText="Plant" UniqueName="Index7">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="SystemName" HeaderText="System" UniqueName="Index8">
                            <HeaderStyle HorizontalAlign="Center" Width="8%" />
                            <ItemStyle HorizontalAlign="Left" Width="8%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="DisciplineName" HeaderText="Discipline" UniqueName="Index9">
                            <HeaderStyle HorizontalAlign="Center" Width="8%" />
                            <ItemStyle HorizontalAlign="Left" Width="8%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="DocumentTypeName" HeaderText="Document Type" UniqueName="Index10">
                            <HeaderStyle HorizontalAlign="Center" Width="8%" />
                            <ItemStyle HorizontalAlign="Left" Width="8%" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="TagTypeName" HeaderText="Tag Type" UniqueName="Index11">
                            <HeaderStyle HorizontalAlign="Center" Width="8%" />
                            <ItemStyle HorizontalAlign="Left" Width="8%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="ProjectName" HeaderText="Project" UniqueName="Index12">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="BlockName" HeaderText="Block" UniqueName="Index13">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="FieldName" HeaderText="Field" UniqueName="Index14">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="PlatformName" HeaderText="Platform" UniqueName="Index15">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="WellName" HeaderText="Platform" UniqueName="Index16">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="StartDate" HeaderText="Start Date" UniqueName="Index17" DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                        </telerik:GridBoundColumn>
                                            
                            <telerik:GridBoundColumn DataField="EndDate" HeaderText="End Date" UniqueName="Index18" DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="NumberOfWork" HeaderText="No.Of Work" UniqueName="Index19">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="TagNo" HeaderText="Tag No" UniqueName="Index20">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="TagDes" HeaderText="Tag Des" UniqueName="Index21">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="Manufacturers" HeaderText="Manufacturers" UniqueName="Index22">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="SerialNo" HeaderText="Serial No" UniqueName="Index23">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="ModelNo" HeaderText="Model No" UniqueName="Index24">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="AssetNo" HeaderText="Asset No" UniqueName="Index25">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="TableOfContents" HeaderText="Table Of Contents" UniqueName="Index26">
                            <HeaderStyle HorizontalAlign="Center" Width="7%" />
                            <ItemStyle HorizontalAlign="Center" Width="7%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="PublishDate" HeaderText="Publish Date" UniqueName="Index27" DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="Fromname" HeaderText="From" UniqueName="Index28">
                            <HeaderStyle HorizontalAlign="Center" Width="4%" />
                            <ItemStyle HorizontalAlign="Center" Width="4%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="ToName" HeaderText="To" UniqueName="Index29">
                            <HeaderStyle HorizontalAlign="Center" Width="4%" />
                            <ItemStyle HorizontalAlign="Center" Width="4%" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="Signer" HeaderText="Signer" UniqueName="Index30">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                    <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="350" UseStaticHeaders="True" />
                    <ClientEvents OnRowDblClick="RowDblClick"></ClientEvents>
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
