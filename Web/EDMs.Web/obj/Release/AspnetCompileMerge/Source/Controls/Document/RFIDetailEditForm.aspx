<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RFIDetailEditForm.aspx.cs" Inherits="EDMs.Web.Controls.Document.RFIDetailEditForm" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
	        overflow:visible;
        }
        .RadComboBoxDropDown_Windows7 {
            width: 416PX !important;
            height: 300px !important;
        }
        .RadComboBoxDropDown .rcbScroll {
            height: 299px !important;
        }
        .RadComboBoxDropDown_Default .rcbHovered {
               background-color: #46A3D3;
               color: #fff;
           }
           .RadComboBoxDropDown .rcbItem, .RadComboBoxDropDown .rcbHovered, .RadComboBoxDropDown .rcbDisabled, .RadComboBoxDropDown .rcbLoading, .RadComboBoxDropDown .rcbCheckAllItems, .RadComboBoxDropDown .rcbCheckAllItemsHovered {
               margin: 0 0px;
           }
           .RadComboBox .rcbInputCell .rcbInput {
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
            width: 399PX;
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
               width: 399PX !important;
               border-bottom: none !important;
           }
        .RadComboBoxDropDown .rcbItem, .RadComboBoxDropDown .rcbHovered, .RadComboBoxDropDown .rcbDisabled, .RadComboBoxDropDown .rcbLoading, .RadComboBoxDropDown .rcbCheckAllItems, .RadComboBoxDropDown .rcbCheckAllItemsHovered {
               margin: 0 0px;
           }
           .RadComboBox .rcbInputCell .rcbInput{
            /*border-left-color: #FF0000;*/
            /*border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;*/
            border-style: solid;
            border-width: 1px 1px 1px 5px;
            color: #000000;
            float: left;
            font: 12px "segoe ui";
            margin: 0;
            padding: 2px 5px 3px;
            vertical-align: middle;
            width: 399PX;
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
               border-bottom: none !important;
           }
           .RadUpload .ruFileWrap {
               overflow: visible !important;
           }

        .accordion dt a
        {
            /*letter-spacing: 0.1em;*/
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
            /*letter-spacing: 0.1em;*/
            line-height: 1.2;
            margin: 0.5em auto 0.6em;
            padding: 0;
            text-align: left;
            text-decoration: none;
            display: block;
        }

        #ddlParent_Input {
            width: 384px !important;
        }

        #rtvRefDocNo {
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
            </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
         <div style="width: 100%" runat="server" ID="EditContent">
         <div style="width: 100%; display:none;">
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
        </div>
        <div style="width: 100%; height: 100%" runat="server" ID="divContent">
             <table>
                <tr>
                    <td style="width:50%; vertical-align: top;">
            <ul style="list-style-type: none">
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">RFI No.
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtNumber"  runat="server" Style="width: 400px;" CssClass="min25Percent " ReadOnly="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                  <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Group
                            </span>
                        </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                           <asp:DropDownList ID="ddlGroup" runat="server" CssClass="min25Percent " Width="416px"/>
                        </div>
                        
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
           
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Work Title
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                             <asp:TextBox ID="txtWorkTitler"  TextMode="MultiLine" Rows="2" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                  <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Description
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                             <asp:TextBox ID="txtDescription" TextMode="MultiLine" Rows="2" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                  <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Location
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                             <asp:TextBox ID="txtLocation" TextMode="MultiLine" Rows="2" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                </ul>
                     </td>
                      <td style="width:50%; vertical-align: top;">              
                          <ul style="list-style-type: none">
                   <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 3px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Time
                            </span>
                        </label>
                        <div style="float: left;  padding-top: 5px;"  class="qlcbFormItem">
                            <telerik:RadDateTimePicker ID="txtIssueDate" runat="server"  Width="300px  "
                                ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DisplayDateFormat="dd/MM/yyyy HH:mm"  DateFormat="dd/MM/yyyy HH:mm" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDateTimePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                 
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Inspection Type
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddltype" runat="server" CssClass="min25Percent " Width="416px">
                                <asp:ListItem Text="H (hole point)" Value ="H"/>
                                <asp:ListItem Text="W (witness)" Value="W"/>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Contractor' contact
                            </span>
                        </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                           <asp:TextBox ID="txtContractorcontact" TextMode="MultiLine" Rows="2" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                        
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Remark
                            </span>
                        </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                           <asp:TextBox ID="txtRemark" TextMode="MultiLine" Rows="2" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                        
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                 <li style="width: 600px;  display:none;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Engineer Performance
                            </span>
                        </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                           <telerik:RadComboBox ID="ddlengineer" runat="server" CssClass="min25Percent qlcbFormRequired"  Width="416PX" CheckBoxes="false" Skin="Windows7" AllowCustomText="false" filter="Contains"  AutoCompleteSeparator="False"  MarkFirstMatch="false"/>
                        </div>
                        
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

              
                    </ul>
                </td>
                    </tr>
                    </table>
            <div style="width:100%;">
                   <ul style="list-style-type: none">
                <li style="width: 600px; padding-top: 10px; padding-bottom: 3px; text-align: center">
                    <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"  Width="70px" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                    <telerik:RadButton ID="btncancel" runat="server" Text="Clear" Width="70px" style="text-align: center"
                        OnClick="btncancel_Click">
                        <Icon PrimaryIconUrl="../../Images/Cancel.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>

                </li>

                 <li style="width: 400px;" Runat="server" ID="blockError" Visible="False">
                    <div>
                        <label style="width: 60px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: red; text-align: right; ">Warning:
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                            <asp:Label runat="server" ID="lblError" Width="300px"></asp:Label>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
            </ul>
                </div> 
        </div>
             </div>
        <telerik:RadGrid AllowCustomPaging="False" AllowPaging="True" AllowSorting="True" 
                                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None"  Skin="Windows7"  
                                Height="600" ID="grdDocument"  AllowFilteringByColumn="True" AllowMultiRowSelection="True"
                                OnNeedDataSource="grdDocument_OnNeedDataSource"
                                OnItemCommand="grdDocument_OnItemCommand"
                                PageSize="100" runat="server" Style="outline: none" Width="100%">
                                <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
                                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                                <MasterTableView AllowMultiColumnSorting="false"
                                    ClientDataKeyNames="ID" DataKeyNames="ID" CommandItemDisplay="Top" 
                                    EditMode="InPlace" Font-Size="8pt">
                                    <GroupByExpressions>
                                           <%-- <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldAlias="-" FieldName="RFINo" FormatString="{0:D}"
                                                        HeaderValueSeparator=""></telerik:GridGroupByField>
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldName="RFINo" SortOrder="Ascending" ></telerik:GridGroupByField>
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>--%>
                                        </GroupByExpressions>    
                                    <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="false"/>
                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                   
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Display="False" />
                                       
                                        <telerik:GridTemplateColumn HeaderText="No." Groupable="False" AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSoTT" runat="server" Text='<%# grdDocument.CurrentPageIndex * grdDocument.PageSize + grdDocument.Items.Count+1 %>'>
                                                </asp:Label>
                                      
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridButtonColumn UniqueName="EditColumn" CommandName="EditCmd" ButtonType="ImageButton" ImageUrl="~/Images/edit.png">
                                            <HeaderStyle Width="24px" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                        </telerik:GridButtonColumn>
                                           <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="DeleteColumn">
                                            <HeaderStyle Width="30"  />
                                            <ItemStyle HorizontalAlign="Center"/>
                                            <ItemTemplate>
                                                <div>
                                                    <a href='javascript:DeleteAttachFile("<%# DataBinder.Eval(Container.DataItem, "ID") %>")' style="text-decoration: none; color:blue">
                                                    <asp:Image ID="delete" runat="server" ImageUrl="~/Images/delete.png" Style="cursor: pointer;" ToolTip="Delete Transmittal" Visible='<%# DataBinder.Eval(Container.DataItem, "IsCanDelete") %>'/>
                                                    <a/></div>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="RFI Number" UniqueName="RFINo" DataField="RFINo"
                                         ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="160" />
                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                        <ItemTemplate>
                                             <%# Eval("RFINo") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                          <telerik:GridTemplateColumn HeaderText="RFI Detail No" UniqueName="Number"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="Number"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="50" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("Number") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Group" UniqueName="GroupName"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="GroupName"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="50" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("GroupName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                         <telerik:GridTemplateColumn HeaderText="Work Title" UniqueName="WorkTitle"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="WorkTitle"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("WorkTitle") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Description" UniqueName="Description"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="Description"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("Description") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                         <telerik:GridTemplateColumn HeaderText="Location" UniqueName="Location"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="Location"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("Location") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                     <telerik:GridDateTimeColumn HeaderText="Time" UniqueName="Time" DataField="Time"
                                            DataFormatString="{0:dd/MM/yyyy HH:mm}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" >
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn> 
                                        <telerik:GridTemplateColumn HeaderText="Inspection Type" UniqueName="InspectionTypeName"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="InspectionTypeName"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("InspectionTypeName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                   
                                     <telerik:GridTemplateColumn HeaderText="Contractor' contact" UniqueName="ContractorContact" DataField="ContractorContact"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# Eval("ContractorContact") %>
                                        </ItemTemplate>
                                     </telerik:GridTemplateColumn> 
                                        
                                        <telerik:GridTemplateColumn HeaderText="Remark" UniqueName="Remark" DataField="Remark"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# Eval("Remark") %>
                                        </ItemTemplate>
                                     </telerik:GridTemplateColumn> 
                                             
                                        <telerik:GridTemplateColumn HeaderText="Action By" UniqueName="EngineeringActionName"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="EngineeringActionName"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="100" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("EngineeringActionName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                      
                                </Columns>
                            </MasterTableView>
                            <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                            <%--<ClientEvents OnRowContextMenu="RowContextMenu" OnRowClick="RowClick"></ClientEvents>--%>
                            <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
                            </ClientSettings>
                        </telerik:RadGrid>
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                 <telerik:AjaxSetting AjaxControlID="btnSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlGroup" />                        
                        <telerik:AjaxUpdatedControl ControlID="txtWorkTitler"/>
                        <telerik:AjaxUpdatedControl ControlID="txtDescription"/>
                        <telerik:AjaxUpdatedControl ControlID="txtLocation"/>
                        <telerik:AjaxUpdatedControl ControlID="txtIssueDate"/>
                        <telerik:AjaxUpdatedControl ControlID="ddltype"/>
                          <telerik:AjaxUpdatedControl ControlID="txtContractorcontact"/>
                        <telerik:AjaxUpdatedControl ControlID="txtRemark"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnClear">
                    <UpdatedControls>
                         <telerik:AjaxUpdatedControl ControlID="ddlGroup" />                        
                        <telerik:AjaxUpdatedControl ControlID="txtWorkTitler"/>
                        <telerik:AjaxUpdatedControl ControlID="txtDescription"/>
                        <telerik:AjaxUpdatedControl ControlID="txtLocation"/>
                        <telerik:AjaxUpdatedControl ControlID="txtIssueDate"/>
                        <telerik:AjaxUpdatedControl ControlID="ddltype"/>
                          <telerik:AjaxUpdatedControl ControlID="txtContractorcontact"/>
                        <telerik:AjaxUpdatedControl ControlID="txtRemark"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                          <telerik:AjaxUpdatedControl ControlID="ddlGroup" />                        
                        <telerik:AjaxUpdatedControl ControlID="txtWorkTitler"/>
                        <telerik:AjaxUpdatedControl ControlID="txtDescription"/>
                        <telerik:AjaxUpdatedControl ControlID="txtLocation"/>
                        <telerik:AjaxUpdatedControl ControlID="txtIssueDate"/>
                        <telerik:AjaxUpdatedControl ControlID="ddltype"/>
                          <telerik:AjaxUpdatedControl ControlID="txtContractorcontact"/>
                        <telerik:AjaxUpdatedControl ControlID="txtRemark"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;
              

                function pageLoad() {
                    ajaxManager = $find("<%=ajaxDocument.ClientID %>");
                }

                function DeleteAttachFile(id) {
                    if (confirm("Do you want delete the RFI Detail?") == false) return;
                    ajaxManager.ajaxRequest("FileDelete_" + id);

                }
                
               
                
                function StopPropagation(e) {
                    if (!e) {
                        e = window.event;
                    }

                    e.cancelBubble = true;
                }

              
            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
