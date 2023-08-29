<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeRequestToDoListAttachRefDocFile.aspx.cs" Inherits="EDMs.Web.Controls.Document.ChangeRequestToDoListAttachRefDocFile" %>

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
                        <table style=" margin:2px; border-bottom:inset; border-bottom-width:1px;">
                            <tr>
                               <td>
                                    <telerik:RadButton ID="btnComplete" runat="server" Text="Complete Task" Width="120px" OnClientClicked ="CompleteMoveNext" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/complete.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon> </telerik:RadButton>
                               </td>
                                 <td>
                                     <div id="DivCRS" runat="server">
                                     <telerik:RadButton ID="btnExportCRS" runat="server" Text="Generate CRS" Width="120px" OnClientClicked ="GenerateCRS" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/comment.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton></div>
                                </td>
                                 <td>
                                    <telerik:RadButton ID="FileAttachTrans" runat="server" Text="Transmittal Attach Files" Width="160px" OnClientClicked ="ShowTransAttachFile" style="text-align: center" Visible="False">
                        <Icon PrimaryIconUrl="../../Images/excelfileattach.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon> </telerik:RadButton>
                               </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="Panel1" runat="server" Width="100%" Height="100%">
                        
                        <dl class="accordion">
                            <dt style="width: 100%;">
                                <span>CONTRACTOR'S CHANGE REQUEST ATTACH FILE</span>
                            </dt>
                        </dl>
                        
                        <telerik:RadGrid ID="grdAttachCRSFile" runat="server" AllowPaging="False"
                        AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                        GridLines="None" Skin="Windows7" Height="100px"
                        OnNeedDataSource="grdAttachCRSFile_OnNeedDataSource" 
                        Style="outline: none; overflow: hidden !important;">
                        <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%" >
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
                                <span>DOCUMENT FILES</span>
                            </dt>
                        </dl>

                        <telerik:RadGrid Skin="Windows7" AllowCustomPaging="False" AllowPaging="False" AllowSorting="True" 
                            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                            Height="50%" ID="grdDocumentFile"  AllowFilteringByColumn="True" AllowMultiRowSelection="False"
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
                                            <a href='javascript:ShowMarkupDocList("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="imgMarkupDocFile" runat="server" ImageUrl="~/Images/markup.png" Style="cursor: pointer;" ToolTip="Markup Document File"  />
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                       <telerik:GridTemplateColumn HeaderText="Document No." UniqueName="DocumentNo" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="120" />
                                            <ItemStyle HorizontalAlign="Left"  />
                                            <ItemTemplate>
                                                <%# Eval("DocumentNo") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                                
                                        <telerik:GridTemplateColumn HeaderText="Review Status" UniqueName="Pecc2ReviewStatusName" AllowFiltering="false" Display="False">
                                            <HeaderStyle HorizontalAlign="Center" Width="120" />
                                            <ItemStyle HorizontalAlign="Left"  />
                                            <ItemTemplate>
                                                <%# Eval("Pecc2ReviewStatusName") %>
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
                                                
                                        <telerik:GridTemplateColumn HeaderText="Revision Remark" UniqueName="RevRemark" AllowFiltering="false">
                                    <HeaderStyle HorizontalAlign="Center" Width="150" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                    <ItemTemplate>
                                        <%# Eval("RevRemark") %>
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
         
    <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
    <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
    <asp:HiddenField runat="server" ID="lblProjectIncomingId"/>
    <asp:HiddenField runat="server" ID="PECC2TransID"/>
    <asp:HiddenField runat="server" ID="ActionType"/>
    <asp:HiddenField runat="server" ID="lbobjId" />
    <asp:HiddenField runat="server" ID="LbcurrentAssignId" />
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Windows7"/>

    <telerik:RadAjaxManager runat="Server" ID="ajaxDocument" OnAjaxRequest="ajaxDocument_OnAjaxRequest">
        <AjaxSettings> 
            <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel1"/>
                    <telerik:AjaxUpdatedControl ControlID="grdDocumentFile" LoadingPanelID="RadAjaxLoadingPanel1"/>
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
            <telerik:RadWindow ID="WFProcessHistory" runat="server" Title="Workflow Process History"
                VisibleStatusbar="False" Height="450" Width="610" IconUrl="~/Images/history.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Transmittal Document File Information"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/attach.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" OnClientClose="refreshGrid">
            </telerik:RadWindow>
              <telerik:RadWindow ID="CRSFile" runat="server" Title="CRS File"
                VisibleStatusbar="false"  IconUrl="~/Images/markup.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            <telerik:RadWindow ID="RejectDocTrans" runat="server" Title="Reject Transmittal Document File"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/reject2.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            <telerik:RadWindow ID="AttachWorkflow" runat="server" Title="Attach Document to Workflow"
                VisibleStatusbar="False" Height="450" Width="610" IconUrl="~/Images/attachwf.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
              <telerik:RadWindow ID="CompleteMoveNext" runat="server" Title="Complete Task"
                VisibleStatusbar="False" Height="450" Width="560" IconUrl="~/Images/complete.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            <telerik:RadWindow ID="MarkupControl" runat="server" Title="Attach document files"
                VisibleStatusbar="false"  IconUrl="~/Images/markup.png" OnClientClose="refreshDocumentGrid"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
             <telerik:RadWindow ID="ETRMAttachFile" runat="server" Title="Transmittal Attach Files"
                VisibleStatusbar="false" Height="600" Width="700" IconUrl="~/Images/excelfileattach.png"
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

                function ShowMarkupDocList(docId) {
                    var actionType = document.getElementById("<%= ActionType.ClientID %>").value;
                    var owd = $find("<%=MarkupControl.ClientID %>");
                    owd.setSize(730, document.documentElement.offsetHeight);
                    owd.Show();
                    //owd.setUrl("Controls/MarkupTool/AttachDocList.aspx?docId=" + objId + "&actionType=" + actionType, "MarkupControl");
                    owd.setUrl("../../Controls/MarkupTool/ChangeRequestAttachDocList.aspx?objId=" + docId + "&actionType=" + actionType, "MarkupControl");
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
                        owd.setSize(730, document.documentElement.offsetHeight);
                        owd.Show();
                        owd.setUrl("../../Controls/Document/ChangeRequestCRSList.aspx?objId=" + objId + "&actionType=" + objTypeId, "CRSFile");
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

                function refreshGrid(args) {
                     GetRadWindow().BrowserWindow.refreshGrid(args);
                    GetRadWindow().close();
                }

            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
