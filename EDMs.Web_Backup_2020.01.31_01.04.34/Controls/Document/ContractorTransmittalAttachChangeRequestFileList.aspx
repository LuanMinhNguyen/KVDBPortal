﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractorTransmittalAttachChangeRequestFileList.aspx.cs" Inherits="EDMs.Web.Controls.Document.ContractorTransmittalAttachChangeRequestFileList" %>

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

        <div style="width: 100%; height:100vh;" runat="server" ID="divContent" >
            
            <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Vertical" Width="100%" Height="100%" Skin="Windows7">
                <telerik:RadPane ID="RadPane1" runat="server" Scrollable="false" Scrolling="None"  Height="100%" Skin="Windows7">
                    <asp:Panel ID="Panel1" runat="server" Width="100%" Height="100%">
                        <telerik:RadGrid Skin="Windows7" AllowCustomPaging="False" AllowPaging="False" AllowSorting="True" 
                            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                            Height="100%" ID="grdDocumentFile"  AllowFilteringByColumn="True" AllowMultiRowSelection="False"
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
                                            
                                        <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" ConfirmText="Do you want to delete item?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png" Display="False">
                                            <HeaderStyle Width="30" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridButtonColumn>
                                    
                                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                                            <HeaderStyle Width="35" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <a href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' 
                                                    download='<%# DataBinder.Eval(Container.DataItem, "Filename") %>' target="_blank">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ExtensionIcon") %>'
                                                        Style="cursor: pointer;" ToolTip="Download document" /> 
                                                </a>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="EditColumn">
                                            <HeaderStyle Width="30"  />
                                            <ItemStyle HorizontalAlign="Center"/>
                                            <ItemTemplate>
                                                <a href='javascript:ShowEditForm("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                                    <asp:Image ID="EditLink" runat="server" ImageUrl="~/Images/edit.png" Style="cursor: pointer;" ToolTip="Edit properties"/>
                                                <a/>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="RejectColumn" Display="False">
                                            <HeaderStyle Width="30"  />
                                            <ItemStyle HorizontalAlign="Center"/>
                                            <ItemTemplate>
                                                <a href='javascript:ShowRejectForm("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                                    <asp:Image ID="imgReject" runat="server" ImageUrl="~/Images/reject2.png" Style="cursor: pointer;" ToolTip="Reject Document" Visible='<%# !Convert.ToBoolean(Eval("IsReject"))%>'/>
                                                <a/>
                                                    
                                                    <asp:Image ID="imgCompleteReject" runat="server" ImageUrl="~/Images/reject.png" Style="cursor: pointer;" Visible='<%# Eval("IsReject") %>'/>

                                                    <telerik:RadToolTip Skin="Simple" runat="server" ID="dirRejectToolTip" RelativeTo="Element" AutoCloseDelay="20000" ShowDelay="0" Position="BottomRight" Width="300px" Height="70px" HideEvent="LeaveTargetAndToolTip" TargetControlID="imgCompleteReject" IsClientID="False" Animation="Fade"
                                                Text='<%# "Reject Reason:<br/>" + Eval("RejectReason").ToString().Replace(Environment.NewLine, "<br/>") %>' Visible='<%# Eval("IsReject") %>'/>
                                         
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="AttachWorkflow">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:AttachWorkflow("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="imgAttachWorkflow" runat="server" ImageUrl="~/Images/attachwf.png" Style="cursor: pointer;" ToolTip="Attach Document To Workflow" Visible='<%# !Convert.ToBoolean(Eval("IsAttachWorkflow")) && !Convert.ToBoolean(Eval("IsReject")) && Eval("PECC2ProjectDocId") != null%>'/>
                                                <a/>
                                                <asp:Image ID="imgAttachWorkflowOk" runat="server" ImageUrl="~/Images/attachwfOk.png" ToolTip="Document Attached To Workflow" Visible='<%# Convert.ToBoolean(Eval("IsAttachWorkflow"))%>'/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                                
                                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="WorkflowProcessHistory">
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ShowWorkflowProcessHistory("<%# DataBinder.Eval(Container.DataItem, "PECC2ProjectDocId") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="imgWorkflowProcessHistory" runat="server" ImageUrl="~/Images/history.png" Style="cursor: pointer;" ToolTip="Workflow process history"  Visible='<%# Eval("PECC2ProjectDocId") != null%>' />
                                                <a/>
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
                                    
                                                <telerik:GridTemplateColumn HeaderText="Document Title" UniqueName="DocumentTitle" AllowFiltering="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="200" />
                                                    <ItemStyle HorizontalAlign="Left"  />
                                                    <ItemTemplate>
                                                        <%# Eval("DocumentTitle") %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                
                                                <telerik:GridTemplateColumn HeaderText="Project Code" UniqueName="ProjectName" AllowFiltering="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                    <ItemStyle HorizontalAlign="Center"  />
                                                    <ItemTemplate>
                                                        <%# Eval("ProjectName") %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                    
                                                <telerik:GridTemplateColumn HeaderText="Change Request Type" UniqueName="ChangeRequestTypeName" AllowFiltering="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                    <ItemStyle HorizontalAlign="Center"  />
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
                                                
                                                <telerik:GridTemplateColumn HeaderText="Year" UniqueName="Year" AllowFiltering="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                    <ItemStyle HorizontalAlign="Center"  />
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
                        
                        </asp:Panel>
                </telerik:RadPane>
            </telerik:RadSplitter>
            
        </div>
        
         
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        <asp:HiddenField runat="server" ID="lblProjectIncomingId"/>
        <asp:HiddenField runat="server" ID="PECC2TransID"/>
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
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Transmittal Document File Information"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/attach.png"
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
            
            <telerik:RadWindow ID="WFProcessHistory" runat="server" Title="Workflow Process History"
                VisibleStatusbar="False" Height="450" Width="610" IconUrl="~/Images/history.png"
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

                function ShowEditForm(objId) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("ContractorTransmittalAttachChangeRequestFileEditForm.aspx?objId=" + objId, "CustomerDialog");
                }

                function ShowWorkflowProcessHistory(docId) {
                var owd = $find("<%=WFProcessHistory.ClientID %>");
                owd.Show();
                owd.maximize();
                owd.setUrl("../../Controls/Workflow/WorkflowProcessHistory.aspx?objId=" + docId, "WFProcessHistory");
            }

                function ShowRejectForm(objId) {
                var owd = $find("<%=RejectDocTrans.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("ContractorTransmittalAttachDocFileReject.aspx?objId=" + objId, "RejectDocTrans");
                }

                function AttachWorkflow(docId) {
                var owd = $find("<%=AttachWorkflow.ClientID %>");
                    var projId = document.getElementById("<%= lblProjectIncomingId.ClientID %>").value;
                     var PECC2Trans = document.getElementById("<%= PECC2TransID.ClientID %>").value;
                 owd.setSize(700, document.documentElement.offsetHeight);
                 owd.Show();
                 owd.setUrl("../../Controls/Workflow/AttachWorkflow.aspx?objId=" + docId + "&projId=" + projId + "&type=2" + "&attachFrom=ChangeRequest&customizeWfFrom=Obj", "AttachWorkflow");
            }
            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
