<%@ Page Language="C#" Debug="true" AutoEventWireup="true" CodeBehind="ContractorTransmittalChangeRequestAttachDocFile.aspx.cs" Inherits="EDMs.Web.Controls.Document.ContractorTransmittalChangeRequestAttachDocFile" %>

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
        
        #grdChangeRequestDocument_GridData {
            height: 100%;
        }

        #grdChangeRequestDocumentPanel {
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

        <div style="width: 100%; height:100vh;" runat="server" ID="divContent" >
            
            <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Vertical" Width="100%" Height="100%" Skin="Windows7">
                <telerik:RadPane ID="RadPane2" runat="server" Scrollable="true" Scrolling="Y" Width="400" Height="100%">
                     <%-- <dl class="accordion">
                        <dt style="width: 100%;">
                            <span>SELECT MAPPING FILE</span>
                        </dt>
                    </dl>
                    <div class="demo-container size-narrow">
                        <telerik:RadAsyncUpload Skin="Silk" RenderMode="Lightweight" runat="server" ID="RaUploadMapping" MultipleFileSelection="Disabled"
                             FileFilters="*.xls,*.xlsx" Localization-Cancel="Cancel" CssClass="min25Percent qlcbFormRequired"
                                Localization-Remove="Remove" Localization-Select="Select"/>
                    </div>--%>
                    
                    <dl class="accordion">
                        <dt style="width: 100%;">
                            <span>SELECT FILES</span>
                        </dt>
                    </dl>
                    <div class="demo-container size-narrow">
                        <telerik:RadAsyncUpload Skin="Silk" RenderMode="Lightweight" runat="server" ID="radUploadDoc" MultipleFileSelection="Automatic"  OnClientFileUploading="OnClientFileUploading"/>
                    </div>
                    
                    <div style="width: 100%">
                        <ul style="list-style-type: none">
                            <li style="width: 300px; padding-top: 10px; padding-bottom: 3px; text-align: center">
                                <telerik:RadButton ID="btnProcessDocNo" runat="server" Text="Process Document File" OnClick="btnProcessDocNo_OnClick" Width="170px" style="text-align: center" Skin="Windows7">
                                    <Icon PrimaryIconUrl="../../Images/process.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                </telerik:RadButton>
                                <%--<telerik:RadButton ID="btncancel" runat="server" Text="Cancel" Width="70px" style="text-align: center"
                                    OnClick="btncancel_Click">
                                    <Icon PrimaryIconUrl="../../Images/Cancel.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                </telerik:RadButton>--%>

                            </li>
                        </ul>
                    </div>
                   
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitBar1" runat="server" />
            
                <telerik:RadPane ID="RadPane1" runat="server" Scrollable="false" Scrolling="None"  Height="100%" Skin="Windows7">
                    <telerik:RadSplitter ID="RadSplitter1" runat="server" Orientation="Horizontal" Height="100%" Width="100%">
                        <telerik:RadPane ID="RadPane3" runat="server" Height="30px" Scrollable="false" Scrolling="None">
                            <telerik:RadToolBar ID="ActionMenu" runat="server" Width="100%" Skin="Windows7" OnClientButtonClicking="OnClientButtonClicking">
                                <Items>
                                    <telerik:RadToolBarButton runat="server" Text="Delete all in-valid document file" ImageUrl="~/Images/delete.png" Value="DeleteAll"  />
                                    <telerik:RadToolBarButton runat="server" IsSeparator="True"/>
                                    <telerik:RadToolBarButton runat="server" Text="Close" ImageUrl="~/Images/Cancel.png" Value="CloseWindow" />
                                </Items>
                            </telerik:RadToolBar>
                        </telerik:RadPane>
                    
                        <telerik:RadPane ID="RadPane4" runat="server" Scrollable="false" Scrolling="None">
                        <telerik:RadSplitter ID="RadSplitter2" runat="server" Orientation="Horizontal" Height="100%" Width="100%">
                            <telerik:RadPane ID="RadPane5" runat="server" Height="160" Scrollable="false" Scrolling="None">
                                <telerik:RadSplitter ID="RadSplitter3" runat="server" Orientation="Horizontal" Height="100%" Width="100%">
                                    <telerik:RadPane ID="RadPane7" runat="server" Height="40px" Scrollable="false" Scrolling="None">
                                        <dl class="accordion">
                                            <dt style="width: 100%;">
                                                <span>FCR/DCN FILES ATTACHMENT</span>
                                            </dt>
                                        </dl>
                                    </telerik:RadPane>
                                    <telerik:RadPane ID="RadPane8" runat="server" Height="100%" Scrollable="false" Scrolling="None">
                                        
                                        <telerik:RadGrid Skin="Windows7" AllowCustomPaging="False" AllowPaging="False" AllowSorting="True" 
                                            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                                            Height="100%" ID="grdChangeRequestDocument"  AllowFilteringByColumn="false" AllowMultiRowSelection="False"
                                            OnNeedDataSource="grdChangeRequestDocument_OnNeedDataSource" 
                                            OnItemDataBound="grdChangeRequestDocument_OnItemDataBound"
                                            OnDeleteCommand="grdChangeRequestDocument_OnDeleteCommand"
                                                PageSize="100" runat="server" Style="outline: none" Width="100%">
                                                <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
                                                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                                                <MasterTableView AllowMultiColumnSorting="false"
                                                    ClientDataKeyNames="ID" DataKeyNames="ID" CommandItemDisplay="Top" 
                                                    EditMode="InPlace" Font-Size="8pt">
                                    
                                                    <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="false"/>
                                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="ErrorPosition" UniqueName="ErrorPosition" Display="False" />
                                    

                                                        <telerik:GridTemplateColumn HeaderText="No." Groupable="False" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSoTT" runat="server" Text='<%# grdChangeRequestDocument.CurrentPageIndex * grdChangeRequestDocument.PageSize + grdChangeRequestDocument.Items.Count+1 %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                
                                                        <telerik:GridTemplateColumn HeaderText="" UniqueName="Status" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="30" />
                                                            <ItemStyle HorizontalAlign="Center"  />
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgStatus" runat="server" ImageUrl='<%# 
                                                                    Eval("Status").ToString() == "Missing Doc Numbering Part" 
                                                                        ? "~/Images/warning.png" 
                                                                        : (Eval("Status").ToString() == "Missing Doc Numbering"
                                                                            ? "~/Images/error.png"
                                                                            : "~/Images/complete1.png") %>' 
                                                                        Style="cursor: pointer;" />

                                                                <telerik:RadToolTip Skin="Simple" runat="server" ID="dirNameToolTip" RelativeTo="Element" AutoCloseDelay="20000" ShowDelay="0" Position="BottomRight" Width="300px" Height="70px" HideEvent="LeaveTargetAndToolTip" TargetControlID="imgStatus" IsClientID="False" Animation="Fade"
                                                        Text='<%# "Error Message:<br/>" + Eval("ErrorMessage").ToString().Replace(Environment.NewLine, "<br/>") %>' Visible='<%# Eval("Status").ToString() == "Missing Doc Numbering Part" || Eval("Status").ToString() == "Missing Doc Numbering" %>'/>
                                         
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                            
                                                        <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="EditColumn">
                                                            <HeaderStyle Width="30"  />
                                                            <ItemStyle HorizontalAlign="Center"/>
                                                            <ItemTemplate>
                                                                <a href='javascript:ShowChangeRequestEditForm("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                                                    <asp:Image ID="EditLink" runat="server" ImageUrl="~/Images/edit.png" Style="cursor: pointer;" ToolTip="Edit properties" Visible='<%# Eval("Status").ToString() != "Missing Doc Numbering Part" && Eval("Status").ToString() != "Missing Doc Numbering" %>' />
                                                                <a/>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                    
                                                        <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" ConfirmText="Do you want to delete item?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                                                            <HeaderStyle Width="30" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </telerik:GridButtonColumn>
                                                
                                                
                                                        <telerik:GridTemplateColumn HeaderText="" UniqueName="RejectColumn" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="30" />
                                                            <ItemStyle HorizontalAlign="Center"  />
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgReject" runat="server" ImageUrl="~/Images/reject.png" 
                                                                        Style="cursor: pointer;" Visible='<%# Eval("IsReject") %>'/>

                                                                <telerik:RadToolTip Skin="Simple" runat="server" ID="dirRejectToolTip" RelativeTo="Element" AutoCloseDelay="20000" ShowDelay="0" Position="BottomRight" Width="300px" Height="70px" HideEvent="LeaveTargetAndToolTip" TargetControlID="imgReject" IsClientID="False" Animation="Fade"
                                                        Text='<%# "Reject Reason:<br/>" + Eval("RejectReason").ToString().Replace(Environment.NewLine, "<br/>") %>' Visible='<%# Eval("IsReject") %>'/>
                                         
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                    
                                                

                                                        <telerik:GridTemplateColumn HeaderText="File Name" UniqueName="FileName" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="200" />
                                                            <ItemStyle HorizontalAlign="Left"  />
                                                            <ItemTemplate>
                                                                <%# Eval("FileName") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                    
                                                        <telerik:GridTemplateColumn HeaderText="Change Request No." UniqueName="DocumentNo" AllowFiltering="false">
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
                                            
                                                        <telerik:GridTemplateColumn HeaderText="Change Request Title" UniqueName="DocumentTitle" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="200" />
                                                            <ItemStyle HorizontalAlign="Left"  />
                                                            <ItemTemplate>
                                                                <%# Eval("DocumentTitle") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                    
                                        
                                        
                                                            <telerik:GridTemplateColumn Display="False" HeaderText="Change Request Type" UniqueName="ChangeRequestTypeName"
                                                            ShowFilterIcon="False" FilterControlWidth="97%"  DataField="ChangeRequestTypeName"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <%# Eval("ChangeRequestTypeName") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                        
                                                        <telerik:GridTemplateColumn HeaderText="Group Code" UniqueName="GroupCodeName" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                            <ItemStyle HorizontalAlign="Center"  />
                                                            <ItemTemplate>
                                                                <%# Eval("GroupCodeName") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                        
                                                            <telerik:GridTemplateColumn HeaderText="Year" UniqueName="Year"
                                                            ShowFilterIcon="False" FilterControlWidth="97%"  DataField="Year"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                                            <HeaderStyle HorizontalAlign="Center" Width="50" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <%# Eval("Year") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        
                                                        <telerik:GridTemplateColumn HeaderText="Sequence" UniqueName="Sequence" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                            <ItemStyle HorizontalAlign="Center"  />
                                                            <ItemTemplate>
                                                                <%# Eval("Sequence") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                            <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                                            <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                    </telerik:RadPane>
                                </telerik:RadSplitter>
                            </telerik:RadPane>
                            <telerik:RadPane ID="RadPane6" runat="server" Scrollable="false" Scrolling="None">
                                <telerik:RadSplitter ID="RadSplitter5" runat="server" Orientation="Horizontal" Height="100%" Width="100%">
                                    <telerik:RadPane ID="RadPane9" runat="server" Height="40px" Scrollable="false" Scrolling="None">
                                        <dl class="accordion">
                                            <dt style="width: 100%;">
                                                <span>DOCUMENTS TO BE REVISED</span>
                                            </dt>
                                        </dl>
                                    </telerik:RadPane>
                                    <telerik:RadPane ID="RadPane10" runat="server" Height="100%" Scrollable="false" Scrolling="None">
                                        <asp:Panel ID="Panel1" runat="server" Width="100%" Height="100%">
                                            <telerik:RadGrid Skin="Windows7" AllowCustomPaging="False" AllowPaging="False" AllowSorting="True" 
                                            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                                            Height="100%" ID="grdDocumentFile"  AllowFilteringByColumn="false" AllowMultiRowSelection="False"
                                            OnNeedDataSource="grdDocumentFile_OnNeedDataSource" 
                                            OnItemDataBound="grdDocumentFile_OnItemDataBound"
                                            OnDeleteCommand="grdDocumentFile_OnDeleteCommand"
                                                PageSize="100" runat="server" Style="outline: none" Width="100%">
                                                <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
                                                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                                                <MasterTableView AllowMultiColumnSorting="false"
                                                    ClientDataKeyNames="ID" DataKeyNames="ID" CommandItemDisplay="Top" 
                                                    EditMode="InPlace" Font-Size="8pt">
                                    
                                                    <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="false"/>
                                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="ErrorPosition" UniqueName="ErrorPosition" Display="False" />
                                    

                                                        <telerik:GridTemplateColumn HeaderText="No." Groupable="False" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSoTT" runat="server" Text='<%# grdDocumentFile.CurrentPageIndex * grdDocumentFile.PageSize + grdDocumentFile.Items.Count+1 %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                
                                                        <telerik:GridTemplateColumn HeaderText="" UniqueName="Status" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="30" />
                                                            <ItemStyle HorizontalAlign="Center"  />
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgStatus" runat="server" ImageUrl='<%# 
                                                                    Eval("Status").ToString() == "Missing Doc Numbering Part" 
                                                                        ? "~/Images/warning.png" 
                                                                        : (Eval("Status").ToString() == "Missing Doc Numbering"
                                                                            ? "~/Images/error.png"
                                                                            : "~/Images/complete1.png") %>' 
                                                                        Style="cursor: pointer;" />

                                                                <telerik:RadToolTip Skin="Simple" runat="server" ID="dirNameToolTip" RelativeTo="Element" AutoCloseDelay="20000" ShowDelay="0" Position="BottomRight" Width="300px" Height="70px" HideEvent="LeaveTargetAndToolTip" TargetControlID="imgStatus" IsClientID="False" Animation="Fade"
                                                        Text='<%# "Error Message:<br/>" + Eval("ErrorMessage").ToString().Replace(Environment.NewLine, "<br/>") %>' Visible='<%# Eval("Status").ToString() == "Missing Doc Numbering Part" || Eval("Status").ToString() == "Missing Doc Numbering" %>'/>
                                         
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                            
                                                        <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="EditColumn">
                                                            <HeaderStyle Width="30"  />
                                                            <ItemStyle HorizontalAlign="Center"/>
                                                            <ItemTemplate>
                                                                <a href='javascript:ShowEditForm("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                                                    <asp:Image ID="EditLink" runat="server" ImageUrl="~/Images/edit.png" Style="cursor: pointer;" ToolTip="Edit properties" Visible='<%# Eval("Status").ToString() != "Missing Doc Numbering Part" && Eval("Status").ToString() != "Missing Doc Numbering" %>' />
                                                                <a/>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                    
                                                        <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" ConfirmText="Do you want to delete item?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                                                            <HeaderStyle Width="30" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </telerik:GridButtonColumn>
                                                
                                                
                                                        <telerik:GridTemplateColumn HeaderText="" UniqueName="RejectColumn" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="30" />
                                                            <ItemStyle HorizontalAlign="Center"  />
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgReject" runat="server" ImageUrl="~/Images/reject.png" 
                                                                        Style="cursor: pointer;" Visible='<%# Eval("IsReject") %>'/>

                                                                <telerik:RadToolTip Skin="Simple" runat="server" ID="dirRejectToolTip" RelativeTo="Element" AutoCloseDelay="20000" ShowDelay="0" Position="BottomRight" Width="300px" Height="70px" HideEvent="LeaveTargetAndToolTip" TargetControlID="imgReject" IsClientID="False" Animation="Fade"
                                                        Text='<%# "Reject Reason:<br/>" + Eval("RejectReason").ToString().Replace(Environment.NewLine, "<br/>") %>' Visible='<%# Eval("IsReject") %>'/>
                                         
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
                                                
                                                        <telerik:GridTemplateColumn HeaderText="Purpose" UniqueName="PurposeName" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                            <ItemStyle HorizontalAlign="Left"  />
                                                            <ItemTemplate>
                                                                <%# Eval("PurposeName") %>
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
                                                
                                                        <telerik:GridTemplateColumn HeaderText="Contractor Ref. No." UniqueName="ContractorRefNo" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="140" />
                                                            <ItemStyle HorizontalAlign="Left"  />
                                                            <ItemTemplate>
                                                                <%# Eval("ContractorRefNo") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                    
                                                        <telerik:GridTemplateColumn HeaderText="Project Code" UniqueName="ProjectName" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                            <ItemStyle HorizontalAlign="Center"  />
                                                            <ItemTemplate>
                                                                <%# Eval("ProjectName") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                    
                                                        <telerik:GridTemplateColumn HeaderText="Document Type" UniqueName="DocumentTypeName" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                            <ItemStyle HorizontalAlign="Center"  />
                                                            <ItemTemplate>
                                                                <%# Eval("DocumentTypeName") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn HeaderText="Unit Code" UniqueName="UnitCodeName" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                            <ItemStyle HorizontalAlign="Center"  />
                                                            <ItemTemplate>
                                                                <%# Eval("UnitCodeName") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                    
                                                        <telerik:GridTemplateColumn HeaderText="KKS Code" UniqueName="KKSCodeName" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                            <ItemStyle HorizontalAlign="Center"  />
                                                            <ItemTemplate>
                                                                <%# Eval("KKSCodeName") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                
                                                        <telerik:GridTemplateColumn HeaderText="Train No." UniqueName="TrainNo" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                            <ItemStyle HorizontalAlign="Center"  />
                                                            <ItemTemplate>
                                                                <%# Eval("TrainNo") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                    
                                                        <telerik:GridTemplateColumn HeaderText="Discipline Code" UniqueName="DisciplineCodeName" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                            <ItemStyle HorizontalAlign="Center"  />
                                                            <ItemTemplate>
                                                                <%# Eval("DisciplineCodeName") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                    
                                                        <telerik:GridTemplateColumn HeaderText="Originating Organization" UniqueName="OriginatingOrganizationName" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                            <ItemStyle HorizontalAlign="Center"  />
                                                            <ItemTemplate>
                                                                <%# Eval("OriginatingOrganizationName") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                    
                                                        <telerik:GridTemplateColumn HeaderText="Receiving Organization" UniqueName="ReceivingOrganizationName" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                            <ItemStyle HorizontalAlign="Center"  />
                                                            <ItemTemplate>
                                                                <%# Eval("ReceivingOrganizationName") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                    
                                                        <telerik:GridTemplateColumn HeaderText="Group Code" UniqueName="GroupCodeName" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                            <ItemStyle HorizontalAlign="Center"  />
                                                            <ItemTemplate>
                                                                <%# Eval("GroupCodeName") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                    
                                                        <telerik:GridTemplateColumn HeaderText="Sequence" UniqueName="Sequence" AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                            <ItemStyle HorizontalAlign="Center"  />
                                                            <ItemTemplate>
                                                                <%# Eval("Sequence") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                       <%-- 
                                                        <telerik:GridTemplateColumn HeaderText="Revision Remark" UniqueName="RevRemark" AllowFiltering="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                    <ItemStyle HorizontalAlign="Left"  />
                                                    <ItemTemplate>
                                                        <%# Eval("RevRemark") %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>--%>
                                                </Columns>
                                            </MasterTableView>
                                            <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                                            <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                        </asp:Panel>
                                    </telerik:RadPane>
                                </telerik:RadSplitter>
                            </telerik:RadPane>
                        </telerik:RadSplitter>

                            
                        </telerik:RadPane>
                    </telerik:RadSplitter>
                    

                    
                </telerik:RadPane>
                
            </telerik:RadSplitter>
            
        </div>
        
         
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        <asp:HiddenField ID="LbFileName" runat="server" />
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Windows7"/>

    <telerik:RadAjaxManager runat="Server" ID="ajaxDocument" OnAjaxRequest="ajaxDocument_OnAjaxRequest">
        <ClientEvents OnRequestStart="RequestStart" OnResponseEnd="ResponseEnd" />
        <AjaxSettings> 
            <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel1"/>
                    <%--<telerik:AjaxUpdatedControl ControlID="grdDocumentFile" LoadingPanelID="RadAjaxLoadingPanel1"/>--%>
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
            
            <telerik:AjaxSetting AjaxControlID="refreshChangeRequestGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="refreshChangeRequestGrid" LoadingPanelID="RadAjaxLoadingPanel1"/>
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
                    <telerik:AjaxUpdatedControl ControlID="grdChangeRequestDocument" LoadingPanelID="RadAjaxLoadingPanel1"/>
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
            
            <telerik:RadWindow ID="ChangeRequestInfo" runat="server" Title="Change Request Information"
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

                function refreshChangeRequestGrid() {
                    var masterTable = $find("<%=grdChangeRequestDocument.ClientID%>").get_masterTableView();
                    masterTable.rebind();
                }

                function OnClientFileUploading(sender, args) {
                    var fileName = args.get_fileName();
                    for (var i = 0; i < sender.getUploadedFiles().length; i++) {
                        if (sender.getUploadedFiles()[i] === fileName) {
                            alert('duplicated: ' + fileName);
                            args.set_cancel(true);
                            sender.deleteFileInputAt(sender.getUploadedFiles().length);
                            //$(".ErrorHolder").append(fileName + "duplicate file name have been remove automatically.<br/>").fadeIn("slow");
                        }

                    }
                }
                var currentLoadingPanel = null;
                var currentUpdatedControl = null;
                function RequestStart(sender, args) {
                    currentLoadingPanel = $find("<%= RadAjaxLoadingPanel1.ClientID %>");
                    <%--alert("<%= grdDocumentFile.UniqueID %>");
                    alert(args.get_eventTarget());--%>
                    if (args.get_eventTarget() === "<%= btnProcessDocNo.UniqueID %>" ) {
                            currentUpdatedControl = "<%= Panel1.ClientID %>";
                        }

                     //show the loading panel over the updated control
                        currentLoadingPanel.show(currentUpdatedControl);
                }
                    function ResponseEnd() {
                        //hide the loading panel and clean up the global variables
                        if (currentLoadingPanel != null)
                        currentLoadingPanel.hide(currentUpdatedControl);
                        currentUpdatedControl = null;
                        currentLoadingPanel = null;
                    }

                function ShowEditForm(objId) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("ContractorTransmittalAttachDocFileEditForm.aspx?objId=" + objId, "CustomerDialog");
                    }

                function ShowChangeRequestEditForm(objId) {
                var owd = $find("<%=ChangeRequestInfo.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("ContractorTransmittalChangeRequestEditForm.aspx?objId=" + objId, "ChangeRequestInfo");
                }

                function OnClientButtonClicking(sender, args) {
                    var button = args.get_item();
                    var strValue = button.get_value();

                    if (strValue === "DeleteAll") {
                        ajaxManager.ajaxRequest("DeleteAll");
                    }
                    else if(strValue === "CloseWindow")
                    {
                        CancelEdit();
                    }
                }

                function refreshDocfileList() {
                var masterTable = $find("<%=grdDocumentFile.ClientID%>").get_masterTableView();
                masterTable.rebind();
                }

                function OnClientFilesSelected1(sender, args) {
                    var hiddenfield1=document.getElementById("<%= LbFileName.ClientID %>");
                    var filenames = hiddenfield1.value;
                    if (filenames != null) {
                            var currentfiles = args.get_fileName();
                            var filename = filenames.split(",");
                            var duplicate = false;
                            for (var i = 0; i < filename.length; i++) {
                                if (filename[i] == currentfiles) {
                                    alert("Duplicate File");
                                    duplicate = true;
                                    break;
                                }
                                if (duplicate == false) {
                                    document.getElementById("<%= LbFileName.ClientID %>").value = filenames + currentfiles + ",";
                                }
                            }
                    } else {
                        document.getElementById("<%= LbFileName.ClientID %>").value = args.get_fileName()+",";
                    }
                    
                }
            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
