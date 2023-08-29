<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PECC2ToDoListDocOfTrans.aspx.cs" Inherits="EDMs.Web.Controls.Document.PECC2ToDoListDocOfTrans" %>

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
            height: 95%;
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
    <form id="form1" runat="server" >
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager>

        <div style="width: 100%; height:100vh;" runat="server" ID="divContent" >
            
            <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Vertical" Width="100%" Height="100%" Skin="Windows7">
                <telerik:RadPane ID="RadPane1" runat="server" Scrollable="false" Scrolling="None"  Height="100%" Skin="Windows7">
                    <asp:Panel ID="Panel2" runat="server" Width="100%" Height="30px">
                        <table style=" margin:1px;">
                            <tr>
                               <td>
                                    <telerik:RadButton ID="btnComplete" runat="server" Text="Complete Task" Width="120px" OnClientClicked ="CompleteMoveNext" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/complete.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon> </telerik:RadButton>
                               </td>
                                 <td>
                                     <div id="DivCRS" runat="server">
                                     <telerik:RadButton ID="btnExportCRS" runat="server" Text="Generate CRS" Width="120px" OnClientClicked="CheckFileCRSExit"  style="text-align: center" SingleClick="True" SingleClickText="Processing">
                        <Icon PrimaryIconUrl="../../Images/comment.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton></div>
                                </td>
                                 <td style="">
                                    <telerik:RadButton ID="CopyClipboard" runat="server" Text="   Copy to Clipboard Folder Path" Width="180px" OnClientClicked ="ShowExplorer" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/copyClib.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon> </telerik:RadButton>
                               </td>
                                <td>
                                    <div id="divApplyCodeForAllDoc" runat="server">
                                      
                        <label style="width: 200px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Apply code for all documents
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="divDocCode">
                            <asp:DropDownList ID="ddlDocReviewStatus" runat="server" CssClass="min25Percent" Width="150px"/>
                        </div> 
                      <label style="width: 200px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="display:none; color: #2E5689; text-align: right; ">Released for Construction
                            </span>
                        </label>
                        <div style="display:none; float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div1">
                            <asp:DropDownList ID="ddlDocreviewStatus2" runat="server" CssClass="min25Percent" Width="150px"/>
                        </div>
                        <div style=" float:left; padding-top: 5px; padding-left:5px;">
                     <telerik:RadButton ID="btnSave" runat="server" Text="Apply" OnClick="btnSave_Click" Width="70px" style="text-align: center" SingleClick="true" SingleClickText="Submitting...">
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton></div>
                    <div style="clear: both; font-size: 0;"></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <asp:Panel ID="Panel1" runat="server" Width="100%" Height="100%">
                        <dl class="accordion">
                            <dt style="width: 100%;">
                                <span>TRANSMITTAL ATTACHMENT</span>
                            </dt>
                        </dl>
                        
                        <telerik:RadGrid ID="grdAttachCRSFile" runat="server" AllowPaging="False"
                        AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                        GridLines="None" Skin="Windows7" Height="70px"
                        OnNeedDataSource="grdAttachCRSFile_OnNeedDataSource" 
                        Style="outline: none; overflow: hidden !important;">
                        <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%" >
                            <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />

                                <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                                    <HeaderStyle Width="25" />
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
                                    <HeaderStyle HorizontalAlign="Center" Width="170" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </telerik:GridBoundColumn>
                    
                                <telerik:GridBoundColumn DataField="TypeName" HeaderText="Type" UniqueName="TypeName">
                                    <HeaderStyle HorizontalAlign="Center" Width="90" />
                                    <ItemStyle HorizontalAlign="Left"/>
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="CreatedByName" HeaderText="Upload by" UniqueName="CreatedByName">
                                    <HeaderStyle HorizontalAlign="Center" Width="120" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Upload time" UniqueName="CreatedDate"
                                    DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="FileSize" HeaderText="File size(Kb)" UniqueName="FileSize" DataFormatString="{0:0,0.00}">
                                    <HeaderStyle HorizontalAlign="Center" Width="70" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                            <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                            <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="200" UseStaticHeaders="True" />
                        </ClientSettings>
                    </telerik:RadGrid>
                         <dl class="accordion">
                            <dt style="width: 100%;">
                                <span>COMMENT RESPONSE SHEET (CRS)</span>
                            </dt>
                        </dl>
                        
                        <telerik:RadGrid ID="grdCRSFilePECC2" runat="server" AllowPaging="False"
                        AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                        GridLines="None" Skin="Windows7" Height="70px"
                        OnNeedDataSource="grdCRSFilePECC2_NeedDataSource" 
                        Style="outline: none; overflow: hidden !important;">
                        <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%" >
                            <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />

                                <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                                    <HeaderStyle Width="25" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                    <ItemTemplate>
                                        <a href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' target="_blank">
                                            <asp:Image ID="Image1" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ExtensionIcon") %>'
                                                Style="cursor: pointer;" ToolTip="Download CRS File" /> 
                                        </a>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                 
                                <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="CheckinCheckOut" >
                                    <HeaderStyle Width="25"  />
                                    <ItemStyle HorizontalAlign="Center"/>
                                    <ItemTemplate>
                                        <a href='javascript:Checkout("<%# DataBinder.Eval(Container.DataItem, "ID") %>","<%# DataBinder.Eval(Container.DataItem, "FilePath") %>")' style="text-decoration: none; color:blue">
                                        <asp:Image ID="imgCheckout" runat="server" ImageUrl="~/Images/checkout.png" Style="cursor: pointer;" ToolTip="Checkout CRS file for manual edit."  Visible='<%# !Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsCheckout")) %>'/>
                                        <a/>
                                        
                                        <a href='javascript:Checkin("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                        <asp:Image ID="imgCheckin" runat="server" ImageUrl="~/Images/checkin.png" Style="cursor: pointer;" ToolTip="Checkin CRS file"   Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsCheckout")) && Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsCanCheckin"))%>'/>

                                        <a/>
                                        
                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/warning.png" Style="cursor: pointer;" ToolTip='<%# "CRS file checked by " + DataBinder.Eval(Container.DataItem, "CheckoutByName")%>'   Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsCheckout")) && !Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsCanCheckin"))%>'/>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                
                                <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="ReuploadCRS" Display="False">
                                    <HeaderStyle Width="25"  />
                                    <ItemStyle HorizontalAlign="Center"/>
                                    <ItemTemplate>
                                        <a href='javascript:GenerateCRS()' style="text-decoration: none; color:blue">
                                        <asp:Image ID="imgUploadFile" runat="server" ImageUrl="~/Images/upload.png" Style="cursor: pointer;" ToolTip="Upload File CRS"  />
                                        <a/>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn DataField="FileName" HeaderText="File name" UniqueName="FileName">
                                    <HeaderStyle HorizontalAlign="Center" Width="170" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </telerik:GridBoundColumn>
                    
                                <telerik:GridBoundColumn DataField="TypeName" HeaderText="Type" UniqueName="TypeName">
                                    <HeaderStyle HorizontalAlign="Center" Width="90" />
                                    <ItemStyle HorizontalAlign="Left"/>
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="CreatedByName" HeaderText="Upload by" UniqueName="CreatedByName">
                                    <HeaderStyle HorizontalAlign="Center" Width="120" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Upload time" UniqueName="CreatedDate"
                                    DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="FileSize" HeaderText="File size(Kb)" UniqueName="FileSize" DataFormatString="{0:0,0.00}">
                                    <HeaderStyle HorizontalAlign="Center" Width="70" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                            <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="200" UseStaticHeaders="True" />
                        </ClientSettings>

                    </telerik:RadGrid>
                         <dl class="accordion">
                            <dt style="width: 100%;">
                                <span>DOCUMENT FILES</span>
                            </dt>
                        </dl>
                        <telerik:RadGrid Skin="Windows7" AllowCustomPaging="False" AllowPaging="False" AllowSorting="True" 
                            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                            Height="65%" ID="grdDocumentFile"  AllowFilteringByColumn="True" AllowMultiRowSelection="False"
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
                                                
                                        <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="WorkflowProcessHistory" >
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ShowMarkupDocList("<%# DataBinder.Eval(Container.DataItem, "ID") %>", "<%# DataBinder.Eval(Container.DataItem, "IsChangeRequest") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="imgMarkupDocFile" runat="server" ImageUrl="~/Images/markup.png" Style="cursor: pointer;" ToolTip="Markup Document File"  />
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                         <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="DocumentRevison">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ShowDocumentRevision("<%# DataBinder.Eval(Container.DataItem, "ID") %>", "<%# DataBinder.Eval(Container.DataItem, "IsChangeRequest") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="revision" runat="server" ImageUrl="~/Images/revision.png" Style="cursor: pointer;" ToolTip="Revision History"/>
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                        <telerik:GridTemplateColumn HeaderText="Document No." UniqueName="DocumentNo" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="150" />
                                            <ItemStyle HorizontalAlign="Left"  />
                                            <ItemTemplate>
                                                <%# Eval("DocNo") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Purpose" UniqueName="PurposeName" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="50" />
                                            <ItemStyle HorizontalAlign="Left"  />
                                            <ItemTemplate>
                                                <%# Eval("DocActionCode") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    
                                        <telerik:GridTemplateColumn HeaderText="Revision" UniqueName="Revision" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                            <ItemTemplate>
                                                <%# Eval("Revision") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Review Status" UniqueName="ReviewStatus" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                            <ItemTemplate>
                                                <%# Eval("DocReviewStatusCode") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                          <telerik:GridTemplateColumn HeaderText="Released for Contruction" UniqueName="ReleasedforContruction" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                            <ItemTemplate>
                                                <%# Eval("DocReviewStatusCode2") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Document Title" UniqueName="DocumentTitle" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="200" />
                                            <ItemStyle HorizontalAlign="Left"  />
                                            <ItemTemplate>
                                                <%# Eval("DocTitle") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                                
                                       <%-- <telerik:GridTemplateColumn HeaderText="Contractor Ref. No." UniqueName="ContractorRefNo" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="140" />
                                            <ItemStyle HorizontalAlign="Left"  />
                                            <ItemTemplate>
                                                <%# Eval("ContractorRefNo") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>--%>
                                    
                                        <telerik:GridTemplateColumn HeaderText="Project Code" UniqueName="ProjectName" AllowFiltering="false" Display="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                            <ItemTemplate>
                                                <%# Eval("ProjectName") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    
                                        <telerik:GridTemplateColumn HeaderText="Document Type" UniqueName="DocumentTypeName" AllowFiltering="false" Display="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                            <ItemTemplate>
                                                <%# Eval("DocTypeCode") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Unit Code" UniqueName="UnitCodeName" AllowFiltering="false" Display="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                            <ItemTemplate>
                                                <%# Eval("UnitCode") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    
                                        <telerik:GridTemplateColumn HeaderText="KKS Code" UniqueName="KKSCodeName" AllowFiltering="false" Display="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                            <ItemTemplate>
                                                <%# Eval("KKSCode") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                                
                                        <telerik:GridTemplateColumn HeaderText="Transmittal In No." UniqueName="TrainNo" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="150" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                            <ItemTemplate>
                                                <%# Eval("IncomingTransNo") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                      
                                          <telerik:GridDateTimeColumn HeaderText="Owner Comment" UniqueName="IsOwnerComment" DataField="IsOwnerComment"
                                DataFormatString="{0:dd/MM/yyyy HH:mm}"  AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" Width="120" />
                            <ItemStyle HorizontalAlign="Center"/>
                        </telerik:GridDateTimeColumn>  
                                         <telerik:GridDateTimeColumn HeaderText="Consultant Comment" UniqueName="IsConsultantComment" DataField="IsConsultantComment"
                                DataFormatString="{0:dd/MM/yyyy HH:mm}"  AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" Width="120" />
                            <ItemStyle HorizontalAlign="Center"/>
                        </telerik:GridDateTimeColumn>  
                                        <telerik:GridTemplateColumn HeaderText="Discipline Code" UniqueName="DisciplineCodeName" AllowFiltering="false" Display="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                            <ItemTemplate>
                                                <%# Eval("DisciplineCode") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    
                                        <telerik:GridTemplateColumn HeaderText="Originating Organization" UniqueName="OriginatingOrganizationName" AllowFiltering="false" Display="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="120" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                            <ItemTemplate>
                                                <%# Eval("OriginatingOrganisationName") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    
                                        <telerik:GridTemplateColumn HeaderText="Receiving Organization" UniqueName="ReceivingOrganizationName" AllowFiltering="false" Display="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="120" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                            <ItemTemplate>
                                                <%# Eval("ReceivingOrganisationName") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    
                                        <telerik:GridTemplateColumn HeaderText="Group Code" UniqueName="GroupCodeName" AllowFiltering="false" Display="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                            <ItemTemplate>
                                                <%# Eval("GroupCode") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    
                                        <telerik:GridTemplateColumn HeaderText="Sequence" UniqueName="Sequence" AllowFiltering="false" Display="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                            <ItemTemplate>
                                                <%# Eval("SequenceText") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Revision Remark" UniqueName="RevRemark" AllowFiltering="false" Display="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="150" />
                                            <ItemStyle HorizontalAlign="Left"  />
                                            <ItemTemplate>
                                                <%# Eval("Remarks") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
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
     <iframe id="my_iframe" style="display:none;"></iframe>
    <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
    <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
    <asp:HiddenField runat="server" ID="lblProjectIncomingId"/>
    <asp:HiddenField runat="server" ID="PECC2TransID"/>
    <asp:HiddenField runat="server" ID="ActionType"/>
    <asp:HiddenField runat="server" ID="lbobjId" />
    <asp:HiddenField runat="server" ID="LbcurrentAssignId" />
    <asp:HiddenField runat="server" ID="ServerName" />
    <asp:HiddenField runat="server" ID="lbStorePath" />
    <asp:HiddenField runat="server" ID="IsHasCRSFile" />
   <asp:HiddenField runat="server" ID="LbIsCancreateOutTrans" />
        
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" Skin="Windows7"/>

    <telerik:RadAjaxManager runat="Server" ID="ajaxDocument" OnAjaxRequest="ajaxDocument_OnAjaxRequest">
        <ClientEvents OnResponseEnd ="EndResponse" />
        <AjaxSettings> 
            <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    <telerik:AjaxUpdatedControl ControlID="grdDocumentFile" LoadingPanelID="RadAjaxLoadingPanel2"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlToList">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtTransNo"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
                
            <telerik:AjaxSetting AjaxControlID="grdDocumentFile">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdDocumentFile" LoadingPanelID="RadAjaxLoadingPanel2"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            
            <telerik:AjaxSetting AjaxControlID="ActionMenu">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdDocumentFile" LoadingPanelID="RadAjaxLoadingPanel2"/>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="ddlFromList">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtTransNo"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
                
            <telerik:AjaxSetting AjaxControlID="btnProcessDocNo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdDocumentFile" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    <telerik:AjaxUpdatedControl ControlID="radUploadDoc" LoadingPanelID="RadAjaxLoadingPanel2"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID ="btnExportCRS">
                <UpdatedControls>
                      <telerik:AjaxUpdatedControl ControlID="grdCRSFilePECC2" LoadingPanelID="RadAjaxLoadingPanel2"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSave">
                <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="grdDocumentFile" LoadingPanelID="RadAjaxLoadingPanel2"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
        
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" Skin="Windows7">
        <Windows>
            <telerik:RadWindow ID="WFProcessHistory" runat="server" Title="Workflow Process History"
                VisibleStatusbar="False" Height="450" Width="610" IconUrl="~/Images/history.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Transmittal Document File Information"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/attach.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" OnClientClose="refreshGrid" Behaviors="Close,Reload">
            </telerik:RadWindow>
              <telerik:RadWindow ID="CRSFile" runat="server" Title="Checkin CRS File" 
                VisibleStatusbar="false"  IconUrl="~/Images/markup.png" OnClientClose="EndResponse"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
             <telerik:RadWindow ID="RevisionDialog" runat="server" Title="Revision History"
               VisibleStatusbar="false" Height="560" Width="900" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            <telerik:RadWindow ID="RejectDocTrans" runat="server" Title="Reject Transmittal Document File"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/reject2.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            <telerik:RadWindow ID="AttachWorkflow" runat="server" Title="Attach Document to Workflow"
                VisibleStatusbar="False" Height="450" Width="610" IconUrl="~/Images/attachwf.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
              <telerik:RadWindow ID="CompleteMoveNext" runat="server" Title="Complete Task"
                VisibleStatusbar="False" Height="450" Width="560" IconUrl="~/Images/complete.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
            <telerik:RadWindow ID="MarkupControl" runat="server" Title="Attach document files"
                VisibleStatusbar="false"  Width="800" MaxWidth="900" IconUrl="~/Images/markup.png" OnClientClose="refreshDocumentGrid"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
             <telerik:RadWindow ID="ETRMAttachFile" runat="server" Title="Transmittal Attach Files"
                VisibleStatusbar="false" Height="600" Width="700" IconUrl="~/Images/excelfileattach.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close,Reload">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;
                function pageLoad() {
                    ajaxManager = $find("<%=ajaxDocument.ClientID %>");
                }

                function ShowMarkupDocList(docId, IsChangeRequest) {
                    var actionType = document.getElementById("<%= ActionType.ClientID %>").value;
                    var owd = $find("<%=MarkupControl.ClientID %>");
                    var iscancreatetrans = document.getElementById("<%= LbIsCancreateOutTrans.ClientID%>").value;
                    owd.setSize(900, document.documentElement.offsetHeight);
                    owd.Show();
                    //owd.setUrl("Controls/MarkupTool/AttachDocList.aspx?docId=" + objId + "&actionType=" + actionType, "MarkupControl");
                    owd.setUrl("../../Controls/MarkupTool/AttachDocList.aspx?docId=" + docId + "&actionType=" + actionType + "&Iscancreatetrans=" + iscancreatetrans + "&IsChangeRequest=" + IsChangeRequest, "MarkupControl");
                }

                function ShowEditForm(objId) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("ContractorTransmittalAttachDocFileEditForm.aspx?objId=" + objId, "CustomerDialog");
                }

                function ShowRejectForm(objId) {
                var owd = $find("<%=RejectDocTrans.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("ContractorTransmittalAttachDocFileReject.aspx?objId=" + objId, "RejectDocTrans");
                }
                function GenerateCRS() {
                var objId = document.getElementById("<%= lbobjId.ClientID %>").value;
                    var objTypeId = document.getElementById("<%= ActionType.ClientID %>").value;
                     var owd = $find("<%=CRSFile.ClientID %>");
                        owd.setSize(730, 510);
                        owd.Show();
                        owd.setUrl("../../Controls/Document/PECC2DocumentCRSList.aspx?objId=" + objId + "&actionType=" + objTypeId, "CRSFile");
                }

                function Checkin(crsId) {
                    var objTypeId = document.getElementById("<%= ActionType.ClientID %>").value;
                    var owd = $find("<%=CRSFile.ClientID %>");
                    owd.setSize(730, 310);
                    owd.Show();
                    owd.setUrl("../../Controls/Document/PECC2CheckinCRSFile.aspx?objId=" + crsId + "&actionType=" + objTypeId, "CRSFile");
                }

                function Checkout(crsId, url) {
                    if (confirm("Do you want to checkout CRS file for edit?")) {
                        document.getElementById('my_iframe').src = url;
                        ajaxManager.ajaxRequest("Checkout_" + crsId);
                    } else {
                        return;
                    }
                }

                function AttachWorkflow(docId) {
                var owd = $find("<%=AttachWorkflow.ClientID %>");
                    var projId = document.getElementById("<%= lblProjectIncomingId.ClientID %>").value;
                     var PECC2Trans = document.getElementById("<%= PECC2TransID.ClientID %>").value;
                 owd.setSize(700, document.documentElement.offsetHeight);
                 owd.Show();
                 owd.setUrl("../../Controls/Workflow/AttachWorkflow.aspx?objId=" + docId + "&projId=" + projId + "&type=1" + "&attachFrom=AttachByDocTrans&customizeWfFrom=Obj", "AttachWorkflow");
                }


                function CompleteMoveNext() {
                    var actionType = document.getElementById("<%= ActionType.ClientID %>").value;
                    var objId = document.getElementById("<%= lbobjId.ClientID %>").value;
                    var objAssignUserId = document.getElementById("<%= LbcurrentAssignId.ClientID %>").value;
                        var owd = $find("<%=CompleteMoveNext.ClientID %>");
                        owd.setSize(610, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("../../Controls/Workflow/CompleteMoveNext.aspx?&objId=" + objId + "&currentAssignId=" + objAssignUserId + "&actionType=" + actionType +"&ReloadPage=true", "CompleteMoveNext");
                }


                function ShowWorkflowProcessHistory(docId) {
                var owd = $find("<%=WFProcessHistory.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.setUrl("../../Controls/Workflow/WorkflowProcessHistory.aspx?objId=" + docId, "WFProcessHistory");
            }

                function ShowTransAttachFile() {
                    var objId = document.getElementById("<%= lbobjId.ClientID %>").value;
                     var owd = $find("<%=ETRMAttachFile.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("PECC2TransETRMAttach.aspx?objId=" + objId+"&Onlyshow=True", "ETRMAttachFile");
                }

                function refreshDocumentGrid() {
                    var masterTable = $find("<%=grdDocumentFile.ClientID%>").get_masterTableView();
                    masterTable.rebind();
                }
                 function copyToClipboardFF(text) {
                window.prompt("Copy to clipboard: Ctrl C, Enter", text);
            }

                function ShowDocumentRevision(docId, IsChangeRequest) {
                    <%--var categoryId = document.getElementById("<%= lblCategoryId.ClientID %>").value;--%>
                    var owd = $find("<%=RevisionDialog.ClientID %>");
                    owd.Show();
                    owd.maximize(true);
                    if (IsChangeRequest === "True") {
                        owd.setUrl("ChangeRequestRevisionHistory.aspx?docId=" + docId, "RevisionDialog");
                    } else {
                        owd.setUrl("ProjectDocumentRevisionHistory.aspx?docId=" + docId, "RevisionDialog");
                    }


                }

                function copyToClipboard(textvalue) {
                var success = true,
                    range = document.createRange(),
                    selection;

                // For IE.
                if (window.clipboardData) {
                    window.clipboardData.setData("Text", textvalue);
                } else {
                    // Create a temporary element off screen.
                    var tmpElem = $('<div>');
                    tmpElem.css({
                        position: "absolute",
                        left: "-1000px",
                        top: "-1000px",
                    });
                    // Add the input value to the temp element.
                    tmpElem.text(textvalue);
                    $("body").append(tmpElem);
                    // Select temp element.
                    range.selectNodeContents(tmpElem.get(0));
                    selection = window.getSelection();
                    selection.removeAllRanges();
                    selection.addRange(range);
                    // Lets copy.
                    try {
                        success = document.execCommand("copy", false, null);
                    }
                    catch (e) {
                        copyToClipboardFF(input.val());
                    }
                    if (success) {
                        alert("The transmittal's path is copied to clipboard. Please paste this path into Windown Exlorer to get the transmittal package.");
                        // remove temp element.
                        tmpElem.remove();
                    }
                }
            }
            function CheckFileCRSExit() {
                if (document.getElementById("IsHasCRSFile").value == "True") {
                    if (confirm("CRS already exists. Do you want to overwrite it?")) {
                        ajaxManager.ajaxRequest("GenerateCRSFile");
                        //EndResponse();
                    } else {
                        return;
                    }
                } else {
                    ajaxManager.ajaxRequest("GenerateCRSFile");
                    //EndResponse();
                }
            }
            function ShowExplorer() {
                var StorePath=document.getElementById("<%= lbStorePath.ClientID %>").value;
                if (StorePath != null) {
                   // ajaxManager.ajaxRequest("OpenFolder_" + ID);
                    
                    var Servername = document.getElementById("<%= ServerName.ClientID %>").value;
                    copyToClipboard(Servername + StorePath.replace("/DocumentLibrary/", "DH2_").replace("/", "\\"));

                } else {
                    alert("Object is Null.");
                }
            }
                function refreshGrid(args) {
                     GetRadWindow().BrowserWindow.refreshGrid(args);
                    GetRadWindow().close();
                }
                function EndResponse(sender, args) {
                    location.reload();
                }
            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
