<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RFIDetailToDoList.aspx.cs" Inherits="EDMs.Web.Controls.Document.RFIDetailToDoList" %>

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
                                    <telerik:RadButton ID="btnExportCRS" runat="server" Text="Export RFI" Width="120px" OnClientClicked="ExportRFI"  style="text-align: center" SingleClick="True" SingleClickText="Processing">
                        <Icon PrimaryIconUrl="../../Images/export.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                                </td>
                                 <td>
                                     <div id="divApplyCodeForAllDoc" runat="server">
                                       
                        <label style="width: 200px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Select Engineer
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="divDocCode">
                            <asp:DropDownList ID="ddlengineer" runat="server" CssClass="min25Percent" Width="250px"/>
                        </div>
                        <div style=" float:left; padding-top: 5px; padding-left:5px;">
                     <telerik:RadButton ID="btnSave" runat="server" Text="Apply" OnClientClicked ="AssignUsser" Width="70px" style="text-align: center" SingleClick="true" SingleClickText="Submitting...">
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
                                <span>RFI Detail</span>
                            </dt>
                        </dl>

                        <telerik:RadGrid Skin="Windows7" AllowCustomPaging="False" AllowPaging="False" AllowSorting="false" 
                            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                            Height="70%" ID="grdDocumentFile"  AllowFilteringByColumn="True" AllowMultiRowSelection="true"
                            OnNeedDataSource="grdDocumentFile_OnNeedDataSource" 
                                PageSize="100" runat="server" Style="outline: none; overflow: hidden !important;">
                              
                               <MasterTableView AllowMultiColumnSorting="false"
                                  ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%" CommandItemDisplay="TopAndBottom" 
                                    EditMode="InPlace" Font-Size="8pt">
  
                                   <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="false"/>
                                   <BatchEditingSettings OpenEditingEvent="Click" EditType="Row"></BatchEditingSettings>
                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Display="False" />
                                        <telerik:GridClientSelectColumn UniqueName="IsSelected">
                                                <HeaderStyle Width="25"  />
                                                <ItemStyle HorizontalAlign="Center" Width="25"/>
                                            </telerik:GridClientSelectColumn>

                                        <telerik:GridTemplateColumn HeaderText="No." Groupable="False" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSoTT" runat="server" Text='<%# grdDocumentFile.CurrentPageIndex * grdDocumentFile.PageSize + grdDocumentFile.Items.Count+1 %>'>
                                                </asp:Label>
                                      
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn AllowFiltering="False" Visible="false" UniqueName="EditColumn" >
                                        <HeaderStyle Width="30"  />
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <a href='javascript:ShowEditForm("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                            <asp:Image ID="EditLink" runat="server" ImageUrl="~/Images/edit.png" Style="cursor: pointer;" ToolTip="Edit properties" />
                                                <a/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                          
                                    <telerik:GridBoundColumn HeaderText="RFI Number" UniqueName="RFINo" DataField="RFINo"
                                         ShowFilterIcon="False" FilterControlWidth="97%" ReadOnly="True"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="160" />
                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                       
                                    </telerik:GridBoundColumn>
                                          <telerik:GridBoundColumn HeaderText="RFI Detail No" UniqueName="Number"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="Number" ReadOnly="True"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="50" />
                                        <ItemStyle HorizontalAlign="Center" />
                                      
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Group" UniqueName="GroupName" ReadOnly="True"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="GroupName"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="50" />
                                        <ItemStyle HorizontalAlign="Center" />
                                       
                                    </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn HeaderText="Work Title" UniqueName="WorkTitle" ReadOnly="True"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="WorkTitle"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                      
                                    </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Description" UniqueName="Description" ReadOnly="True"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="Description"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                       
                                    </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn HeaderText="Location" UniqueName="Location" ReadOnly="True"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="Location"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                      
                                    </telerik:GridBoundColumn>
                                     <telerik:GridDateTimeColumn HeaderText="Time" UniqueName="Time" DataField="Time"
                                            DataFormatString="{0:dd/MM/yyyy HH:MM}" FilterControlWidth="80%" ReadOnly="True" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" >
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn> 
                                        <telerik:GridBoundColumn HeaderText="Inspection Type" UniqueName="InspectionTypeName"
                                        ShowFilterIcon="False" FilterControlWidth="97%" ReadOnly="True"  DataField="InspectionTypeName"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" />
                                       
                                    </telerik:GridBoundColumn>
                                   
                                     <telerik:GridBoundColumn HeaderText="Contractor' contact" UniqueName="ContractorContact" DataField="ContractorContact" ReadOnly="True"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Left" />
                                       
                                     </telerik:GridBoundColumn> 
                                        
                                        <telerik:GridTemplateColumn HeaderText="Remark" UniqueName="Remark" DataField="Remark"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  ReadOnly="false"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                           <asp:Label ID="lbremark" runat="server" Text='<%# Eval("Remark")%>'></asp:Label>
                                        </ItemTemplate>
                                              <EditItemTemplate>
                                                <span>
                                                    <telerik:RadTextBox Width="100%" runat="server" ID="txtremark"/>
                                                </span>
                                            </EditItemTemplate>
                                     </telerik:GridTemplateColumn> 
                                             
                                         <telerik:GridTemplateColumn HeaderText="Comment" UniqueName="CommentContent" DataField="CommentContent"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  ReadOnly="false"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                           <asp:Label ID="lbCommentContent" runat="server" Text='<%# Eval("CommentContent")%>'></asp:Label>
                                        </ItemTemplate>
                                              <EditItemTemplate>
                                                <span>
                                                    <telerik:RadTextBox Width="100%" runat="server" ID="txtCommentContent"/>
                                                </span>
                                            </EditItemTemplate>
                                     </telerik:GridTemplateColumn> 
                                        <telerik:GridBoundColumn HeaderText="Action By" UniqueName="EngineeringActionName"
                                        ShowFilterIcon="False" FilterControlWidth="97%" ReadOnly="True"  DataField="EngineeringActionName"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="100" />
                                        <ItemStyle HorizontalAlign="Center" />
                                      
                                    </telerik:GridBoundColumn>

                                      
                                </Columns>
                            </MasterTableView>
                            <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                            <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
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
        <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
    <%--  <ClientEvents OnResponseEnd ="EndResponse" />--%>
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
               <%-- <telerik:AjaxSetting AjaxControlID="btnSave">
                    <UpdatedControls>
                       <telerik:AjaxUpdatedControl ControlID="grdDocumentFile" LoadingPanelID="RadAjaxLoadingPanel1"/>
                 <telerik:AjaxUpdatedControl ControlID="ddlengineer" LoadingPanelID="RadAjaxLoadingPanel1"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
             <%-- <telerik:AjaxSetting AjaxControlID="grdDocumentFile">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdDocumentFile" LoadingPanelID="RadAjaxLoadingPanel2"/>
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
            
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
           
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="RFI Detail"
                VisibleStatusbar="false" Height="600" Width="650" IconUrl="~/Images/attach.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" OnClientClose="refreshDocumentGrid">
            </telerik:RadWindow>
              <telerik:RadWindow ID="CompleteMoveNext" runat="server" Title="Complete Task"
                VisibleStatusbar="False" Height="450" Width="560" IconUrl="~/Images/complete.png"
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
                function ExportRFI() {

                    ajaxManager.ajaxRequest("ExportRFI");
                }
                function AssignUsser() {
                     var grid = $find("<%=grdDocumentFile.ClientID %>");
                    var masterTable = grid.get_masterTableView();
                    var listId = "";
                    var selectedRows = masterTable.get_selectedItems();
                    if (selectedRows.length == 0) {
                        alert("Please select RFI Detail.");
                    }
                    else {
                        
                        for (var i = 0; i < selectedRows.length; i++) {
                            var row = selectedRows[i];
                            listId += row.getDataKeyValue("ID") + ",";
                        }
                        ajaxManager.ajaxRequest("SelectEngineer_" + listId);
                  <%--   var masterTable = $find("<%=grdDocumentFile.ClientID%>").get_masterTableView();
                    masterTable.rebind();--%>
                    }
                }
                function onRequestStart(sender, args) {
                    //alert(args.get_eventTarget());
                    if (args.get_eventTarget().indexOf("btnExportCRS") >= 0 ||
                        args.get_eventTarget().indexOf("ajaxDocument") >= 0) {
                        args.set_enableAjax(false);
                    }
                }
            
                function ShowEditForm(objId) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.setSize(730, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("RFIDetailRemarkEditForm.aspx?rfiId=" + objId, "CustomerDialog");
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


            

                function refreshDocumentGrid() {
                    var masterTable = $find("<%=grdDocumentFile.ClientID%>").get_masterTableView();
                    masterTable.rebind();
                }
                //function EndResponse(sender, args) {
                //      location.reload();
                //}
                function refreshGrid(args) {
                     GetRadWindow().BrowserWindow.refreshGrid(args);
                    GetRadWindow().close();
                }

            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
