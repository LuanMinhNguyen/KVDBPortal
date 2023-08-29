﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WRDetail.aspx.cs" Inherits="EDMs.Web.Controls.WMS.WRDetail" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
	        overflow-x:hidden;
	        overflow-y:auto;
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

        .rlbItem
        {
            float:left !important;
        }
        .rlbGroup, .RadListBox
        {
            width:auto !important;
        }

        #grdAttachFile_GridData {
             height: 100% !important;
        }

        #grdComment_GridData {
             height: 100% !important;
        }

        #rtvPIC {
             border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3 !important;
            color: #000000 !important;
            font: 12px "segoe ui" !important;
            border-width: 1px !important;
            border-style: solid !important;
            border-left-width: 5px !important;
            padding: 2px 1px 3px !important;
            vertical-align: middle !important;
            margin: 0 !important;
            padding-left: 5px !important;
            padding-right: 5px !important;
         }
        
        #rtvHODIC {
             border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3 !important;
            color: #000000 !important;
            font: 12px "segoe ui" !important;
            border-width: 1px !important;
            border-style: solid !important;
            border-left-width: 5px !important;
            padding: 2px 1px 3px !important;
            vertical-align: middle !important;
            margin: 0 !important;
            padding-left: 5px !important;
            padding-right: 5px !important;
         }

         .rlbGroup {
               border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3 !important;
            color: #000000 !important;
            font: 12px "segoe ui" !important;
            border-width: 1px !important;
            border-style: solid !important;
            border-left-width: 5px !important;
            padding: 2px 1px 3px !important;
            vertical-align: middle !important;
            margin: 0 !important;
            padding-left: 5px !important;
            padding-right: 5px !important;
           }
         #divContentPanel {
            height: 100% !important;
        }

         .borderTable {
             border-collapse: collapse;
             border: 1px solid black;
             width: 100%;
             padding-top: 5px;
             padding-bottom: 5px;
             padding-left: 5px;
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
          
        <%--<div style="width: 100%">
            <ul style="list-style-type: none">
                <div class="qlcbFormItem">
                    <div class="dnnFormMessage dnnFormInfo">
                        <div class="dnnFormItem dnnFormHelp dnnClear">
                            <p class="dnnFormRequired" style="float: left;">
                                <span style="text-decoration: underline;">Notes</span>: All fields marked with a red are required.
                            </p>
                            <br />
                        </div>
                    </div>
                </div>
            </ul>
        </div>--%>

        <div style="width: 100%; height: 100%" runat="server" ID="divContent">
        
            <telerik:RadSplitter ID="Radsplitter10" runat="server" Orientation="Vertical" Width="100%" Height="100%">
                <telerik:RadPane ID="Radpane6" runat="server" Scrolling="Both" Width="25%">
                    <div align="center">
                        <b style="font-size: 25pt">Work Request Form</b><br/>
                        <span style="font-size: 16pt">(Facility: <asp:Label runat="server" ID="lblProjectName" Text="FPSO PTSC LAM SON"/>)</span>
                    </div>
                    <div align="center">
                        <telerik:RadButton ID="btnComplete" runat="server" Text="Complete & Move Next"  Width="160px" style="text-align: center" Skin="Windows7"
                            OnClientClicked="btnCompleteClicked">
                            <Icon PrimaryIconUrl="../../Images/complete.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                        </telerik:RadButton>
                        <telerik:RadButton ID="btnCompleteFinal" runat="server" Text="Complete"  Width="120px" style="text-align: center" Skin="Windows7"
                            OnClientClicked="btnCompleteFinalClicked">
                            <Icon PrimaryIconUrl="../../Images/complete.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                        </telerik:RadButton>
                        <br/>
                        <telerik:RadButton ID="btnReject" runat="server" Text="Reject"  Width="160px" style="text-align: center" Skin="Windows7" OnClientClicked="btnRejectClicked">
                            <Icon PrimaryIconUrl="../../Images/reject.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                        </telerik:RadButton>
                    </div>
                    
                    <table class="borderTable" >
                        <tr class="borderTable">
                            <td>
                                <b>WR Number: </b> <asp:Label runat="server" ID="lblWRNo" Text="FPSO PTSC LAM SON"/>
                                <div style="float: right">
                                    <telerik:RadButton ID="btnExportWR" runat="server" Text="Export WR Form" OnClick="btnExportWR_Click" Width="120px" style="text-align: center" Skin="Windows7">
                                        <Icon PrimaryIconUrl="../../Images/exexcel.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                    </telerik:RadButton>
                                </div>
                            </td>
                        </tr>
                        
                        <tr class="borderTable">
                            <td>
                                <b>No. of Attachments: </b><asp:Label runat="server" ID="lblNoAttachment" />
                            </td>
                        </tr>
                        
                        <tr class="borderTable">
                            <td>
                                <b>WR Title:</b><br/> <asp:Label runat="server" ID="lblWRTitle"/>
                            </td>
                        </tr>
                        
                        <tr class="borderTable">
                            <td><b>Originator: </b> <asp:Label runat="server" ID="lblOriginator"/></td>
                        </tr>
                        
                        <tr class="borderTable">
                            <td><b>Job Title: </b><br/> <asp:Label runat="server" ID="lblJobTitle"/></td>
                        </tr>
                        <tr class="borderTable">
                            <td><b>Date Raised: </b> <asp:Label runat="server" ID="lblRaiseDate"/></td>
                        </tr>
                        
                        <tr class="borderTable">
                            <td><b>Date Required: </b> <asp:Label runat="server" ID="lblDateRequire" /></td>
                        </tr>
                        <tr class="borderTable">
                            <td>
                                <b>Description of Request: </b> 
                                <br/>
                                <asp:Label runat="server" ID="lblDescription" />
                            </td>
                        </tr>
                        <tr class="borderTable">
                            <td>
                                <b>Summary of Scope of Services: </b> 
                                <br/>
                                <asp:Label runat="server" ID="lblScope" />
                            </td>
                        </tr>
                        <tr class="borderTable">
                            <td>
                                <b>Reason to raise Work Request: </b> 
                                <br/>
                                <asp:Label runat="server" ID="lblReason"/>
                            </td>
                        </tr>
                        
                        <tr class="borderTable">
                            <td><b>Priority Level </b>(19): <telerik:RadListBox RenderMode="Lightweight" ID="lbPriority" runat="server" CheckBoxes="true" ShowCheckAll="False" OnClientItemChecking="OnClientItemChecking" Skin="Windows7"/></td>
                        </tr>
                    </table>
                </telerik:RadPane>
                
                <telerik:RadPane ID="Radpane1" runat="server" Scrolling="Y" Width="75%">
                    <telerik:RadTabStrip ID="RadTabStrip1" runat="server"  width="100%" Skin="MetroTouch" MultiPageID="RadMultiPage1" SelectedIndex="0">
                        <Tabs>
                            <telerik:RadTab ImageUrl="~/Images/comment3.png" Text="Comment">
                            </telerik:RadTab>
                            <telerik:RadTab ImageUrl="~/Images/attach1.png" Text="Attach Files">
                            </telerik:RadTab>
                            <telerik:RadTab ImageUrl="~/Images/trackinglist1.png" Text="Tracking Info">
                            </telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>
                    
                    <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0">
                            
                            <%-- Comment --%>
                        <telerik:RadPageView runat="server" Height="100%" ID="RadPageView4">
                            <div style="width: 100%" runat="server" id="divWRComment">
                                <ul style="list-style-type: none">
                                    
                                    <li style="width: 800px;" Runat="server" ID="Li2">
                                        <div>
                                            <label style="width: 110px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">Comment From
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div3">
                                                    <asp:DropDownList ID="ddlCommentFrom" runat="server" CssClass="min25Percent qlcbFormRequired" Width="516px" style="max-width: 5006px">
                                                        <Items>
                                                            <asp:ListItem Value="0" Text="Offshore"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Operation Department"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Technical/ Relevant Department"></asp:ListItem>
                                                            <asp:ListItem Value="3" Text="Board of Director"></asp:ListItem>
                                                        </Items>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="clear: both; font-size: 0;"></div>
                                    </li>

                                    <li style="width: 800px; display: none">
                                        <div>
                                            <label style="width: 110px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">Comment By
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                <asp:TextBox runat="server" ID="txtCommentBy" style="width: 500px; max-width: 500px" CssClass="min25Percent qlcbFormRequired"/>
                                            </div>
                                        </div>
                                        <div style="clear: both; font-size: 0;"></div>
                                    </li>
                                    
                                    <li style="width: 800px;">
                                        <div>
                                            <label style="width: 110px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">Comment
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                <asp:TextBox runat="server" ID="txtComment" style="width: 500px; max-width: 500px" CssClass="min25Percent qlcbFormRequired" TextMode="MultiLine" Rows="3"/>
                                            </div>
                                        </div>
                                        <div style="clear: both; font-size: 0;"></div>
                                    </li>

                                    <li style="width: 800px; text-align: center; padding-top: 10px; padding-bottom: 5px">
                                        <telerik:RadButton ID="btnSaveComment" runat="server" Text="Save" OnClick="btnSaveComment_Click" Width="70px" style="text-align: center">
                                            <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                        </telerik:RadButton>
                                        
                                        <telerik:RadButton ID="btnClearComment" runat="server" Text="Clear" OnClick="btnClearComment_OnClick" Width="70px" style="text-align: center">
                                            <Icon PrimaryIconUrl="../../Images/clear.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                        </telerik:RadButton>
                                    </li>
                                </ul>
                            </div>
                            <telerik:RadGrid ID="grdComment" runat="server" AllowPaging="False"
                                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                GridLines="None" Skin="Windows7" Height="100%"
                                OnDeleteCommand="grdComment_DeleteCommand"
                                OnNeedDataSource="grdComment_OnNeedDataSource" 
                                OnItemCommand="grdComment_OnItemCommand"
                                
                                PageSize="10" Style="outline: none; overflow: hidden !important;">
                                <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%">
                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                                        
                                        <telerik:GridButtonColumn UniqueName="EditColumn" CommandName="EditCmd" ButtonType="ImageButton" ImageUrl="~/Images/edit.png">
                                            <HeaderStyle Width="30" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                        </telerik:GridButtonColumn>

                                        <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" ConfirmText="Do you want to delete Attach file?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                                            <HeaderStyle Width="30" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%"  />
                                        </telerik:GridButtonColumn>

                                        
                                
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
                                            
                                        <telerik:GridBoundColumn DataField="CommentTypeName" HeaderText="Comment From" UniqueName="CommentTypeName">
                                            <HeaderStyle HorizontalAlign="Center" Width="150" />
                                            <ItemStyle HorizontalAlign="Left"/>
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="CommentByName" HeaderText="Comment By" UniqueName="CommentByName">
                                            <HeaderStyle HorizontalAlign="Center" Width="150" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                
                                        <telerik:GridBoundColumn DataField="Comment" HeaderText="Comment" UniqueName="Comment">
                                            <HeaderStyle HorizontalAlign="Center" Width="250" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                
                                        <telerik:GridBoundColumn DataField="CommentDate" HeaderText="Comment time" UniqueName="CommentDate"
                                            DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                                    <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="200" UseStaticHeaders="True" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </telerik:RadPageView>

                            <%-- Attach File --%>
                        <telerik:RadPageView runat="server" Height="100%" ID="RadPageView3">
                            <div style="width: 100%" runat="server" id="divWRAttachFile">
                                <ul style="list-style-type: none">
                                    <li style="width: 800px;">
                                        <div>
                                            <label style="width: 110px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">Description
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                <asp:TextBox runat="server" ID="txtAttachDescription" style="width: 500px; max-width: 500px" CssClass="min25Percent" TextMode="MultiLine" Rows="2"/>
                                            </div>
                                        </div>
                                        <div style="clear: both; font-size: 0;"></div>
                                    </li>

                                    <li style="width: 600px;" Runat="server" ID="UploadControl">
                                        <div>
                                            <label style="width: 110px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">Select attach file
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                <span style="color: red">Limited file size < 1.5mb</span>
                                                <telerik:RadAsyncUpload runat="server" ID="docuploader"
                                                    MultipleFileSelection="Automatic" TemporaryFileExpiration="05:00:00" 
                                                    EnableInlineProgress="true" Width="350px"
                                                    Localization-Cancel="Cancel" CssClass="min25Percent qlcbFormRequired"
                                                    Localization-Remove="Remove" Localization-Select="Select"  Skin="Windows7"
                                                    MaxFileSize="1572864">
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
                                    

                                    <li style="width: 800px; text-align: center">
                                        <telerik:RadButton ID="btnSaveAttachFile" runat="server" Text="Save" OnClick="btnSaveAttachFile_Click" Width="70px" style="text-align: center">
                                            <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                        </telerik:RadButton>
                                        
                                    </li>
                                </ul>
                            </div>
                            <telerik:RadGrid ID="grdAttachFile" runat="server" AllowPaging="False"
                                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                GridLines="None" Skin="Windows7" Height="100%"
                                OnDeleteCommand="grdAttachFile_DeleteCommand"
                                OnNeedDataSource="grdAttachFile_OnNeedDataSource" 
                                PageSize="10" Style="outline: none; overflow: hidden !important;">
                                <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%">
                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                                        <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" ConfirmText="Do you want to delete Attach file?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                                            <HeaderStyle Width="30" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%"  />
                                        </telerik:GridButtonColumn>

                                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                                            <HeaderStyle Width="35" />
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
                                            <HeaderStyle HorizontalAlign="Center" Width="150" />
                                            <ItemStyle HorizontalAlign="Left"/>
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="Description" HeaderText="Description" UniqueName="Description">
                                            <HeaderStyle HorizontalAlign="Center" Width="200" />
                                            <ItemStyle HorizontalAlign="Left"/>
                                        </telerik:GridBoundColumn>
                                
                                        <telerik:GridBoundColumn DataField="CreatedByName" HeaderText="Upload by" UniqueName="CreatedByName">
                                            <HeaderStyle HorizontalAlign="Center" Width="150" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                
                                        <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Upload time" UniqueName="CreatedDate"
                                            DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                            <HeaderStyle HorizontalAlign="Center" Width="100" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                
                                        <telerik:GridBoundColumn DataField="FileSize" HeaderText="File size(Kb)" UniqueName="FileSize" DataFormatString="{0:0,0.00}">
                                            <HeaderStyle HorizontalAlign="Center" Width="100" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                                    <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="200" UseStaticHeaders="True" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </telerik:RadPageView>
                            
                        <telerik:RadPageView runat="server" Height="100%" ID="RadPageView6">
                            <div runat="server" ID="divMRDetailTracking">
                                <fieldset>
                                    <legend><b>Work Request Tracking Info:</b></legend>
                                    <table>
                                        <tr>
                                            <td>Risk assessment</td>
                                            <td>
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                    <asp:TextBox runat="server" ID="txtRiskAssessment" Style="width: 200px; min-width: 50px" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
                                                </div>
                                            </td>
                                            
                                            <td>Action plan</td>
                                            <td>
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                    <asp:TextBox runat="server" ID="txtActionPlan" Style="width: 200px; min-width: 50px" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
                                                </div>
                                            </td>
                                            
                                            <td>Remarks</td>
                                            <td>
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                    <asp:TextBox runat="server" ID="txtRemarks" Style="width: 200px; min-width: 50px" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
                                                </div>
                                            </td>
                                            
                                            
                                        </tr>
                                        
                                        <tr>
                                            <td>Date Received (From BOD)</td>
                                            <td>
                                                 <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                    <telerik:RadDatePicker ID="txtReceiveDateFromBOD"  runat="server" 
                                                        ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                                        <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                                    </telerik:RadDatePicker>
                                                </div>
                                            </td>
                                            
                                            <td rowspan="2">Function Dept Update</td>
                                            <td rowspan="2">
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                    <asp:TextBox runat="server" ID="txtFncDeptUpdate" Style="width: 200px; min-width: 50px" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
                                                </div>
                                            </td>
                                            <td>WR Status</td>
                                            <td>
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="min25Percent" Width="216px" style="max-width: 316px">
                                                        <Items>
                                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                                            <asp:ListItem Value="Open" Text="Open"></asp:ListItem>
                                                            <asp:ListItem Value="Close" Text="Close"></asp:ListItem>
                                                            <asp:ListItem Value="Cancel" Text="Cancel"></asp:ListItem>
                                                        </Items>
                                                    </asp:DropDownList>
                                                </div>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td>Person In Charge</td>
                                            <td>
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                    <asp:TextBox runat="server" ID="txtPIC" Style="width: 200px; min-width: 50px" CssClass="min25Percent"/>
                                                </div>
                                            </td>
                                            
                                        </tr>
                                        
                                        <tr>
                                            <td>HOD In Charge</td>
                                            <td>
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                    <asp:TextBox runat="server" ID="txtHODIC" Style="width: 200px; min-width: 50px" CssClass="min25Percent"/>
                                                </div>
                                            </td>
                                            
                                            <td>Deadline to complete</td>
                                            <td>
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                    <telerik:RadDatePicker ID="txtDeadline"  runat="server" 
                                                        ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                                        <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                                    </telerik:RadDatePicker>
                                                </div>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td>Department</td>
                                            <td>
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                    <asp:TextBox runat="server" ID="txtDept" Style="width: 200px; min-width: 50px" CssClass="min25Percent"/>
                                                </div>
                                            </td>
                                            
                                            <td rowspan="2">Overdue reason</td>
                                            <td rowspan="2">
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                    <asp:TextBox runat="server" ID="txtOverdueReason" Style="width: 200px; min-width: 50px" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
                                                </div>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td>Date pass to Function Dept</td>
                                            <td>
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                                                    <telerik:RadDatePicker ID="txtDatePassFncDept"  runat="server" 
                                                        ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                                        <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                                    </telerik:RadDatePicker>
                                                </div>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td colspan="6" align="center">
                                                <telerik:RadButton ID="btnTrackingSave" runat="server" Text="Save" OnClick="btnTrackingSave_Click" Width="70px" style="text-align: center">
                                                    <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>

                        </telerik:RadPageView>   
                    </telerik:RadMultiPage>
                </telerik:RadPane>
            </telerik:RadSplitter>
</div>
            
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        <asp:HiddenField runat="server" ID="ObjectType"/>
        <asp:HiddenField runat="server" ID="ObjectId"/>
        <asp:HiddenField runat="server" ID="ObjAssignUserId"/>
        
        <asp:HiddenField runat="server" ID="IsView"/>
        <asp:HiddenField runat="server" ID="IsCreate"/>
        <asp:HiddenField runat="server" ID="IsUpdate"/>
        <asp:HiddenField runat="server" ID="IsCancel"/>
        <asp:HiddenField runat="server" ID="IsAttachWF"/>

        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="rtlMRCheckListDefine">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtCheckListRemark"/>
                        <telerik:AjaxUpdatedControl ControlID="txtCheckListDescription"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnYes"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnNo"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnNA"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnCheckListSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtCheckListRemark"/>
                        <telerik:AjaxUpdatedControl ControlID="txtCheckListDescription"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnYes"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnNo"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnNA"/>
                        <telerik:AjaxUpdatedControl ControlID="rtlMRCheckListDefine"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnCheckListClear">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtCheckListRemark"/>
                        <telerik:AjaxUpdatedControl ControlID="txtCheckListDescription"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnYes"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnNo"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnNA"/>
                        <telerik:AjaxUpdatedControl ControlID="rtlMRCheckListDefine"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSaveAttachFile">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="docuploader"/>
                        <telerik:AjaxUpdatedControl ControlID="txtAttachDescription"/>
                        <telerik:AjaxUpdatedControl ControlID="grdAttachFile"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdAttachFile">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAttachFile"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdComment">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdComment"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlCommentFrom"/>
                        <telerik:AjaxUpdatedControl ControlID="txtCommentBy"/>
                        <telerik:AjaxUpdatedControl ControlID="txtComment"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSaveComment">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdComment"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlCommentFrom"/>
                        <telerik:AjaxUpdatedControl ControlID="txtCommentBy"/>
                        <telerik:AjaxUpdatedControl ControlID="txtComment"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnClearComment">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdComment"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlCommentFrom"/>
                        <telerik:AjaxUpdatedControl ControlID="txtCommentBy"/>
                        <telerik:AjaxUpdatedControl ControlID="txtComment"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" Skin="Windows7">
        <Windows>
            <telerik:RadWindow ID="CompleteMoveNext" runat="server" Title="Complete Task And Move Next Step"
                VisibleStatusbar="False" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RejectForm" runat="server" Title="Reject To Previous Step"
                VisibleStatusbar="False" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="CompleteFinal" runat="server" Title="Complete MR Process Workflow"
                VisibleStatusbar="False" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;
                function OnClientFilesUploaded(sender, args) {
                    var name = args.get_fileName();
                    document.getElementById("txtName").value = name;
                    $find('<%=ajaxDocument.ClientID %>').ajaxRequest();
                }

                function pageLoad() {
                    ajaxManager = $find("<%=ajaxDocument.ClientID %>");
                }

                function fileUploading(sender, args) {
                    var name = args.get_fileName();
                    document.getElementById("txtName").value = name;

                    ajaxManager.ajaxRequest("CheckFileName$" + name);
                }

                function OnClientItemChecking(Sender, args) {
                    args.set_cancel(true);
                }

                function btnCompleteClicked(sender, args)
                {
                    var objType = document.getElementById("<%= ObjectType.ClientID %>").value;
                    var objId = document.getElementById("<%= ObjectId.ClientID %>").value;
                    var objAssignUserId = document.getElementById("<%= ObjAssignUserId.ClientID %>").value;
                    var owd = $find("<%=CompleteMoveNext.ClientID %>");
                    owd.Show();
                    owd.setUrl("../../Controls/Workflow/CompleteMoveNext.aspx?objType=" + objType + "&objId=" + objId + "&currentAssignId=" + objAssignUserId, "CompleteMoveNext");
                }

                function btnRejectClicked(sender, args) {
                    if (confirm("Do you want to Reject this Object?") == false) return;

                    var objType = document.getElementById("<%= ObjectType.ClientID %>").value;
                    var objId = document.getElementById("<%= ObjectId.ClientID %>").value;
                    var objAssignUserId = document.getElementById("<%= ObjAssignUserId.ClientID %>").value;

                    var owd = $find("<%=RejectForm.ClientID %>");
                    owd.Show();
                    owd.setUrl("../../Controls/Workflow/Reject.aspx?objType=" + objType + "&objId=" + objId + "&currentAssignId=" + objAssignUserId, "RejectForm");
                }

                function btnCompleteFinalClicked(sender, args)
                {
                    var objType = document.getElementById("<%= ObjectType.ClientID %>").value;
                    var objId = document.getElementById("<%= ObjectId.ClientID %>").value;
                    var objAssignUserId = document.getElementById("<%= ObjAssignUserId.ClientID %>").value;
                    var owd = $find("<%=CompleteFinal.ClientID %>");
                    owd.Show();
                    owd.setUrl("../../Controls/Workflow/FinalComplete.aspx?objType=" + objType + "&objId=" + objId + "&currentAssignId=" + objAssignUserId + "&flag=true", "CompleteFinal");
                }
            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
