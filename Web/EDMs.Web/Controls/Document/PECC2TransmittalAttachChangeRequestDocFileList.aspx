<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PECC2TransmittalAttachChangeRequestDocFileList.aspx.cs" Inherits="EDMs.Web.Controls.Document.PECC2TransmittalAttachChangeRequestDocFileList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
	        overflow:hidden;
        }
        
        .RadComboBoxDropDown_Default .rcbHovered {
               background-color: #46A3D3;
               color: #fff;
           }
           .RadComboBoxDropDown .rcbItem, .RadComboBoxDropDown .rcbHovered, .RadComboBoxDropDown .rcbDisabled, .RadComboBoxDropDown .rcbLoading, .RadComboBoxDropDown .rcbCheckAllItems, .RadComboBoxDropDown .rcbCheckAllItemsHovered {
               margin: 0 0px;
           }
           .RadComboBox .rcbInputCell .rcbInput{
            border-left-color:#46A3D3 !important;
            border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;
            border-style: solid;
            border-width: 1px 1px 1px 5px;
            color: #000000;
            float: left;
            font: 12px "segoe ui";
            margin: 0;
            padding: 2px 5px 3px;
            vertical-align: middle;
            width: 283px;
           }
           .RadComboBox table td.rcbInputCell, .RadComboBox .rcbInputCell .rcbInput {
               padding-left: 0px !important;
               padding-right: 0px !important;
           }

              div.rgEditForm label {
               float: right;
            text-align: right;
            width: 72px;
           }
           .rgEditForm {
               text-align: right;
           }
           .RadComboBox {
               width: 115px !important;
               border-bottom: none !important;
           }
           .RadUpload .ruFileWrap {
               overflow: visible !important;
           }

           .demo-container.size-narrow {
                max-width: 500px;
                display: inline-block;
                text-align: left;
                background-color: #FFFFDB;
               padding-left: 5px;
            }

           .demo-container .RadUpload .ruUploadProgress {
                width: 300px;
                display: inline-block;
                overflow: hidden;
                text-overflow: ellipsis;
                white-space: nowrap;
                vertical-align: top;
            }

           html .demo-container .ruFakeInput {
                width: 300px;
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

        #grdDocumentFile_GridData {
            height: 100%;
        }

        #grdDocumentFilePanel {
            height: 100%;
        }

        #Panel1 {
            display: initial !important;
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
            GetRadWindow().BrowserWindow.refreshOutgoingGrid(args);
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
    <form id="form1" runat="server" >
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager>
         <div style="width: 100%" runat="server" id="DivUpload">
            <ul style="list-style-type: none">
                <li style="width: 100%; text-align:center;" Runat="server" ID="UploadControl">
                    <div>
                        <label style="width:45%; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Upload multiple file
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                            <telerik:RadAsyncUpload runat="server" ID="docuploader"
                                MultipleFileSelection="Automatic" TemporaryFileExpiration="05:00:00" 
                                EnableInlineProgress="true" Width="350px"
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
                <li style="width: 100%; text-align: center; padding-top: 10px">
                    <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="70px" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                </li>
            </ul>
        </div>
        <div style="width: 100%; height:100vh;" runat="server" ID="divContent" >
            
            <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Vertical" Width="100%" Height="100%" Skin="Windows7">
                <telerik:RadPane ID="RadPane1" runat="server" Scrollable="false" Scrolling="None"  Height="100%" Skin="Windows7">
                    <asp:Panel ID="Panel1" runat="server" Width="100%" Height="100%">
                        <telerik:RadGrid Skin="Windows7" AllowCustomPaging="False" AllowPaging="False" AllowSorting="True" 
                            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                            Height="100%" ID="grdDocumentFile"  AllowFilteringByColumn="False" AllowMultiRowSelection="False"
                            OnNeedDataSource="grdDocumentFile_OnNeedDataSource" 
                                PageSize="100" runat="server" Style="outline: none" Width="100%">
                                <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
                                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                                <MasterTableView AllowMultiColumnSorting="false"
                                    ClientDataKeyNames="ID" DataKeyNames="ID" CommandItemDisplay="Top" 
                                    EditMode="InPlace" Font-Size="8pt">
                                    <GroupByExpressions>
                                    <telerik:GridGroupByExpression>
                                        <SelectFields>
                                            <telerik:GridGroupByField FieldAlias="Document" FieldName="DocNo" FormatString="{0:D}"
                                                HeaderValueSeparator=": "></telerik:GridGroupByField>
                                        </SelectFields>
                                        <GroupByFields>
                                            <telerik:GridGroupByField FieldName="DocNo" SortOrder="Ascending" ></telerik:GridGroupByField>
                                        </GroupByFields>
                                    </telerik:GridGroupByExpression>
                                </GroupByExpressions>
                                    <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="false"/>
                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="No." Groupable="False" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSoTT" runat="server" Text='<%# grdDocumentFile.CurrentPageIndex * grdDocumentFile.PageSize + grdDocumentFile.Items.Count+1 %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                                
                                          <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="DeleteColumn">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <div runat="server" visible='<%# !string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "ID").ToString())%>'>
                                            <a href='javascript:DeleteFile("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="delete" runat="server" ImageUrl="~/Images/delete.png" Style="cursor: pointer;" ToolTip="Delete Transmittal" />
                                                <a/></div>
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
                                                

                                        <telerik:GridTemplateColumn HeaderText="File Name" UniqueName="FileName" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="200" />
                                            <ItemStyle HorizontalAlign="Left"  />
                                            <ItemTemplate>
                                                <%# Eval("FileName") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    
                                        <telerik:GridTemplateColumn HeaderText="Document No." UniqueName="DocNo" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="150" />
                                            <ItemStyle HorizontalAlign="Left"  />
                                            <ItemTemplate>
                                                <%# Eval("DocNo") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Document Title" UniqueName="DocTitle" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="200" />
                                            <ItemStyle HorizontalAlign="Left"  />
                                            <ItemTemplate>
                                                <%# Eval("DocTitle") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    
                                        <telerik:GridBoundColumn DataField="FileSize" HeaderText="File size(Kb)" UniqueName="FileSize" DataFormatString="{0:0,0.00}">
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center"/>
                                        </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                            <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                            </ClientSettings>
                        </telerik:RadGrid>
                        
                        </asp:Panel>
                </telerik:RadPane>
            </telerik:RadSplitter>
            
        </div>
        
         
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Windows7"/>

    <telerik:RadAjaxManager runat="Server" ID="ajaxDocument" OnAjaxRequest="ajaxDocument_OnAjaxRequest">
        <AjaxSettings> 
            <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel1"/>
                    <telerik:AjaxUpdatedControl ControlID="grdDocumentFile" LoadingPanelID="RadAjaxLoadingPanel1"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSave">
                <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel1"/>
                     <telerik:AjaxUpdatedControl ControlID="docuploader" LoadingPanelID="RadAjaxLoadingPanel1"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlToList">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtTransNo"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
                
            <telerik:AjaxSetting AjaxControlID="grdDocumentFile">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdDocumentFile" LoadingPanelID="RadAjaxLoadingPanel1"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            
            <telerik:AjaxSetting AjaxControlID="ActionMenu">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdDocumentFile" LoadingPanelID="RadAjaxLoadingPanel1"/>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="ddlFromList">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtTransNo"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
                
            <telerik:AjaxSetting AjaxControlID="btnProcessDocNo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdDocumentFile" LoadingPanelID="RadAjaxLoadingPanel1"/>
                    <telerik:AjaxUpdatedControl ControlID="radUploadDoc" LoadingPanelID="RadAjaxLoadingPanel1"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
        
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" Skin="Windows7">
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Transmittal Document File Information"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/attach.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;
                function pageLoad() {
                    ajaxManager = $find("<%=ajaxDocument.ClientID %>");
                }
                function DeleteFile(id) {
                    if (confirm("Do you want to delete item?") == false) return;
                    ajaxManager.ajaxRequest("DeleteFile_" + id);
                }
                function ShowEditForm(objId) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("ContractorTransmittalAttachDocFileEditForm.aspx?objId=" + objId, "CustomerDialog");
                }
            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
