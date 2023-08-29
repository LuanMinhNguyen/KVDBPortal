<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeRequestAllAttachFile.aspx.cs" Inherits="EDMs.Web.Controls.Document.ChangeRequestAllAttachFile" %>

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
            overflow: hidden !important;
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

        .RadGrid .rgSelectedRow
        {
            background-image : none !important;
            background-color: darkseagreen !important;
        } 
        .accordion dt a
        {
            letter-spacing: 0.1em;
            line-height: 1.2;
            margin: 0.5em auto 0.6em;
            padding: 0;
            text-align: left;
            text-decoration: none;
            display: block;
        }

        .accordion dt span {
            color: #085B8F;
            border-bottom: 1px solid #46A3D3;
            font-size: 1.0em;
            font-weight: bold;
            letter-spacing: 0.1em;
            line-height: 1.2;
            margin: 0.5em auto 0.6em;
            padding: 0;
            text-align: left;
            text-decoration: none;
            display: block;
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
        

        <%--<dl class="accordion">
            <dt style="width: 100%;">
                <span>Document Attach Files</span>
            </dt>
        </dl>--%>

        <telerik:RadGrid ID="grdDocumentAttachFile" Visible="False" runat="server" AllowPaging="False"
            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
            GridLines="None" Skin="Windows7" Height="170"
            OnNeedDataSource="grdDocumentAttachFile_OnNeedDataSource" 
            PageSize="10" Style="outline: none; overflow: hidden !important;">
            <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%">
                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="No." Groupable="False" AllowFiltering="false">
                        <HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblSoTT" runat="server" Text='<%# grdDocumentAttachFile.CurrentPageIndex * grdDocumentAttachFile.PageSize + grdDocumentAttachFile.Items.Count+1 %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                                
                    <telerik:GridTemplateColumn HeaderText="" UniqueName="Status" AllowFiltering="false">
                        <HeaderStyle HorizontalAlign="Center" Width="30" />
                        <ItemStyle HorizontalAlign="Center"  />
                        <ItemTemplate>
                            <asp:Image ID="imgStatus" runat="server" ImageUrl='<%# 
                                !string.IsNullOrEmpty(Eval("ErrorMessage").ToString())
                                        ? "~/Images/error.png"
                                        : "~/Images/complete1.png" %>' 
                                    Style="cursor: pointer;" />

                            <telerik:RadToolTip Skin="Simple" runat="server" ID="dirNameToolTip" RelativeTo="Element" AutoCloseDelay="20000" ShowDelay="0" Position="BottomRight" Width="300px" Height="70px" HideEvent="LeaveTargetAndToolTip" TargetControlID="imgStatus" IsClientID="False" Animation="Fade"
                    Text='<%# "Error Message:<br/>" + Eval("ErrorMessage").ToString().Replace(Environment.NewLine, "<br/>") %>' Visible='<%# !string.IsNullOrEmpty(Eval("ErrorMessage").ToString()) %>'/>
                                         
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                            
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                <HeaderStyle Width="35" />
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <a href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' 
                        download='<%# DataBinder.Eval(Container.DataItem, "FileName") %>' target="_blank">
                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ExtensionIcon") %>'
                            Style="cursor: pointer;" ToolTip="Download document" /> 
                    </a>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="" UniqueName="RejectColumn" AllowFiltering="false" Display="false">
                        <HeaderStyle HorizontalAlign="Center" Width="30" />
                        <ItemStyle HorizontalAlign="Center"  />
                        <ItemTemplate>
                            <asp:Image ID="imgReject" runat="server" ImageUrl="~/Images/reject.png" 
                                    Style="cursor: pointer;" Visible='<%# Eval("IsReject") %>'/>

                            <telerik:RadToolTip Skin="Simple" runat="server" ID="dirRejectToolTip" RelativeTo="Element" AutoCloseDelay="20000" ShowDelay="0" Position="BottomRight" Width="300px" Height="70px" HideEvent="LeaveTargetAndToolTip" TargetControlID="imgReject" IsClientID="False" Animation="Fade"
                    Text='<%# "Reject Reason:<br/>" + Eval("RejectReason").ToString().Replace(Environment.NewLine, "<br/>") %>' Visible='<%# Eval("IsReject") %>'/>
                                         
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                   
                      
                                    
                    <telerik:GridTemplateColumn HeaderText="Document No." UniqueName="DocumentNo" AllowFiltering="false">
                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                        <ItemStyle HorizontalAlign="Left"  />
                        <ItemTemplate>
                            <%# Eval("DocumentNo") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                          
                    <telerik:GridTemplateColumn HeaderText="Revision" UniqueName="Revision" AllowFiltering="false">
                        <HeaderStyle HorizontalAlign="Center" Width="60" />
                        <ItemStyle HorizontalAlign="Center"  />
                        <ItemTemplate>
                            <%# Eval("Revision") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                    
                    <telerik:GridTemplateColumn HeaderText="Document Title" UniqueName="DocumentTitle" AllowFiltering="false">
                        <HeaderStyle HorizontalAlign="Center" Width="350" />
                        <ItemStyle HorizontalAlign="Left"  />
                        <ItemTemplate>
                            <%# Eval("DocumentTitle") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                                
                    
                </Columns>
            </MasterTableView>
            <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="200" UseStaticHeaders="True" />
            </ClientSettings>
        </telerik:RadGrid>

        <dl class="accordion">
            <dt style="width: 100%;">
                <span>Change Request's Attach Files</span>
            </dt>
        </dl>

        <telerik:RadGrid ID="grdChangeRequestAttachFile" runat="server" AllowPaging="True"
            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
            GridLines="None" Skin="Windows7" Height="250"
            OnNeedDataSource="grdChangeRequestAttachFile_OnNeedDataSource" 
            PageSize="10" Style="outline: none; overflow: hidden !important;">
            <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%">
                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                <Columns>
                    <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                        <HeaderStyle Width="35" />
                        <ItemStyle HorizontalAlign="Center"/>
                        <ItemTemplate>
                            <a href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' 
                                download='<%# DataBinder.Eval(Container.DataItem, "FileName") %>' target="_blank">
                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ExtensionIcon") %>'
                                    Style="cursor: pointer;" ToolTip="Download document" /> 
                            </a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                            
                    <telerik:GridBoundColumn DataField="FileName" HeaderText="File name" UniqueName="FileName">
                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                        <ItemStyle HorizontalAlign="Left" />
                    </telerik:GridBoundColumn>
                    
                    <telerik:GridBoundColumn DataField="TypeName" HeaderText="Type" UniqueName="TypeName" Display="False">
                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                        <ItemStyle HorizontalAlign="Left"/>
                    </telerik:GridBoundColumn>
                                
                    <telerik:GridBoundColumn DataField="CreatedByName" HeaderText="Upload by" UniqueName="CreatedByName">
                        <HeaderStyle HorizontalAlign="Center" Width="180" />
                        <ItemStyle HorizontalAlign="Left" />
                    </telerik:GridBoundColumn>
                                
                    <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Upload time" UniqueName="CreatedDate"
                        DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                        <ItemStyle HorizontalAlign="Center"/>
                    </telerik:GridBoundColumn>
                                
                    <telerik:GridBoundColumn DataField="FileSize" HeaderText="File size(Kb)" UniqueName="FileSize" DataFormatString="{0:0,0.00}">
                        <HeaderStyle HorizontalAlign="Center" Width="60" />
                        <ItemStyle HorizontalAlign="Center"/>
                    </telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
            <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="200" UseStaticHeaders="True" />
            </ClientSettings>
        </telerik:RadGrid>
        
        
         <%--<div style="width: 100%; text-align: center; padding-top: 270px">
           
        </div>--%>
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="Windows7"/>
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument" OnAjaxRequest="ajaxDocument_AjaxRequest">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="btnSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="docuploader" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <%--<telerik:AjaxUpdatedControl ControlID="btnExportCRS"/>--%>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnExportChangeRequestForm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                        <%--<telerik:AjaxUpdatedControl ControlID="btnExportCRS"/>--%>

                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;

                
          </script>

        </telerik:RadScriptBlock>
    </form>
</body>
</html>
