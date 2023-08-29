<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeRequestAttachDocList.aspx.cs" Inherits="EDMs.Web.Controls.MarkupTool.ChangeRequestAttachDocList" %>

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
        #grdDocument_GridData {
            height: 100% !important;
        }
        
        .RadAsyncUpload {
            width:350px !important;
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

        .RadGrid .rgSelectedRow
        {
            background-image : none !important;
            background-color: darkseagreen !important;
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
        
        
        <dl class="accordion">
            <dt style="width: 100%;">
                <span>DOCUMENT FILE LIST</span>
            </dt>
        </dl>

        <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="True"
            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
            GridLines="None" Skin="Windows7" Height="120px"
            OnNeedDataSource="grdDocument_OnNeedDataSource" 
            PageSize="10" Style="outline: none; overflow: hidden !important;">
            <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%">
                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                <Columns>
                    <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />

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

                    <telerik:GridTemplateColumn HeaderText="File Name" UniqueName="FileName" AllowFiltering="false">
                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                        <ItemStyle HorizontalAlign="Left"  />
                        <ItemTemplate>
                            <%# Eval("FileName") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                    
                    <telerik:GridTemplateColumn HeaderText="Document No." UniqueName="DocumentNo" AllowFiltering="false">
                        <HeaderStyle HorizontalAlign="Center" Width="120" />
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
                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                        <ItemStyle HorizontalAlign="Left"  />
                        <ItemTemplate>
                            <%# Eval("DocumentTitle") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
            <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="100" UseStaticHeaders="True" />
            </ClientSettings>
        </telerik:RadGrid>
        
        <dl class="accordion">
            <dt style="width: 100%;">
                <span>ATTACH MARKUP/COMMENT FILE</span>
            </dt>
        </dl>
        
        <div style="width: 100%">
            <ul style="list-style-type: none">
                <li style="width: 100%;" Runat="server" ID="UploadControl">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Upload multiple file
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                            <telerik:RadAsyncUpload runat="server" ID="docuploader"
                                MultipleFileSelection="Automatic" TemporaryFileExpiration="05:00:00" 
                                EnableInlineProgress="true" Width="406px"
                                Localization-Cancel="Cancel" CssClass="min25Percent qlcbFormRequired"
                                Localization-Remove="Remove" Localization-Select="Select"  Skin="Windows7">
                            </telerik:RadAsyncUpload>
                            
                            <%--<telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload1"
                                MultipleFileSelection="Automatic" TemporaryFileExpiration="05:00:00" 
                                EnableInlineProgress="true" Width="250px"
                                Localization-Cancel="Cancel" CssClass="min25Percent qlcbFormRequired"
                                Localization-Remove="Remove" Localization-Select="Select"  Skin="Windows7" DropZones=".DropZone1"
                                FileFilters="*.doc,*.docx,*.xls,*.xlsx,*.pdf">
                            </telerik:RadAsyncUpload>--%>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 100%;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Type
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:RadioButton runat="server" GroupName="AttachType" ID="cbMarkupFile" Text="Document Markup/Comment Files" Checked="True"/><br/>
                            <asp:RadioButton runat="server" GroupName="AttachType" ID="cbConsolidateFile" Text="Document Markup/Comment Consolidate File" />
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                <li style="width: 100%; text-align: center; padding-top: 10px">
                    <div style="padding-bottom: 5px">
                    <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="70px" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                   </div>
                    <div style="padding-bottom: 5px">
                    <telerik:RadButton ID="btnConsolidate" runat="server" Text="Consolidate Markup" OnClick="btnConsolidate_OnClick" Width="150px" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/consolidate.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                   </div>
                </li>
            </ul>
        </div>
        <telerik:RadGrid ID="grdAttachMarkupCommentFile" runat="server" AllowPaging="False"
            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
            GridLines="None" Skin="Windows7" Height="300px"
            OnNeedDataSource="grdAttachMarkupCommentFile_OnNeedDataSource" 
            OnDeleteCommand="grdAttachMarkupCommentFile_OnDeleteCommand"
            Style="outline: none; overflow: hidden !important;">
            <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%" >
                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                <Columns>
                    <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                    <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" ConfirmText="Do you want to delete document file?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                        <HeaderStyle Width="5%" />
                            <ItemStyle HorizontalAlign="Center" Width="5%"  />
                    </telerik:GridButtonColumn>

                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                        <HeaderStyle Width="5%" />
                        <ItemStyle HorizontalAlign="Center" Width="5%"/>
                        <ItemTemplate>
                            <a href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' 
                                download='<%# DataBinder.Eval(Container.DataItem, "FileName") %>' target="_blank">
                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ExtensionIcon") %>'
                                    Style="cursor: pointer;" ToolTip="Download document" /> 
                            </a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                
                    <%--<telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                        <HeaderStyle Width="5%" />
                        <ItemStyle HorizontalAlign="Center" Width="5%"/>
                        <ItemTemplate>
                            <a href='<%# string.Format("../../DownloadFileHandler.ashx?img={0}",DataBinder.Eval(Container.DataItem, "ID")) %>' 
                                download='<%# DataBinder.Eval(Container.DataItem, "FileName") %>' target="_blank">
                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ExtensionIcon") %>'
                                    Style="cursor: pointer;" ToolTip="Download document" /> 
                            </a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                                            
                    <telerik:GridBoundColumn DataField="FileName" HeaderText="File name" UniqueName="FileName">
                        <HeaderStyle HorizontalAlign="Center" Width="35%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </telerik:GridBoundColumn>
                    
                    <telerik:GridBoundColumn DataField="TypeName" HeaderText="Type" UniqueName="TypeName">
                        <HeaderStyle HorizontalAlign="Center" Width="25%" />
                        <ItemStyle HorizontalAlign="Left"/>
                    </telerik:GridBoundColumn>
                                
                    <telerik:GridBoundColumn DataField="CreatedByName" HeaderText="Upload by" UniqueName="CreatedByName">
                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
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
        <asp:HiddenField runat="server" ID="lblUserId"/>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="Windows7"/>
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument" OnAjaxRequest="ajaxDocument_AjaxRequest">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="btnSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAttachMarkupCommentFile" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="docuploader" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdAttachMarkupCommentFile">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAttachMarkupCommentFile" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="btnExportCRS"/>
                    </UpdatedControls>
                </telerik:AjaxSetting> 
                
                <telerik:AjaxSetting AjaxControlID="btnConsolidate">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAttachMarkupCommentFile" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="btnConsolidate" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                 <telerik:AjaxSetting AjaxControlID="btnExportCRS">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAttachMarkupCommentFile" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="btnExportCRS" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdAttachMarkupCommentFile" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;
                function ShowMarkupControl(docId) {
                    var userId = document.getElementById("<%= lblUserId.ClientID %>").value;
                    var url = "MarkupControl.aspx?docId=" + docId + "&userId=" + userId;
                    window.open(url,'_blank');
                }
          </script>

        </telerik:RadScriptBlock>
    </form>
</body>
</html>
