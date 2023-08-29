<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FolderPermission.aspx.cs" Inherits="EDMs.Web.Controls.Document.FolderPermission" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
	        overflow:hidden;
        }
        
              div.rgEditForm label {
               float: right;
            text-align: right;
            width: 72px;
           }
           .rgEditForm {
               text-align: right;
           }
           .RadUpload .ruFileWrap {
               overflow: visible !important;
           }
        div.qlcbFormItem select.min25Percent {
            max-width: 500px !important;
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
            width: 370px;
           }
           .RadComboBox table td.rcbInputCell, .RadComboBox .rcbInputCell .rcbInput {
               padding-left: 0px !important;
           }
         
            #overlayDiv1 {
            position: fixed;
            top: 0;
            left: 0;
            z-index:100;
        }
    </style>

    <script src="../../Scripts/jquery-1.7.1.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        function CloseAndRebind(args) {
            GetRadWindow().BrowserWindow.refreshTreeFolder(args);
            GetRadWindow().close();
        }
        
        function CloseAndRebindGrid(args) {
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
        
        <div style="width: 100%">
            <ul style="list-style-type: none">
                <div class="qlcbFormItem">
                    <div class="dnnFormMessage dnnFormInfo">
                        <div class="dnnFormItem dnnFormHelp dnnClear">
                            <p class="dnnFormRequired" style="float: left;">
                                <asp:Label ID="lblFolderDirName" runat="server"></asp:Label>
                            </p>
                            <br />
                        </div>
                    </div>
                </div>
            </ul>
        </div>
          
        <div style="width: 100%" runat="server" ID="divContent">
            <ul style="list-style-type: none">
                <li style="width: 500px;">
                    <div>
                        <label style="width: 60px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px; padding-left: 6px;" class="qlcbFormItem">
                            <asp:CheckBox ID="cbApplyAll" runat="server" Text="Apply for all subfolder" Checked="True" />
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                <li style="width: 500px;">
                    <div>
                        <label style="width: 60px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">
                                Permission
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px; padding-left: 6px;" class="qlcbFormItem">
                            <asp:RadioButton ID="rdbRead" runat="server" Checked="true"  Text="Read" GroupName="Permission"/>
                            <asp:RadioButton ID="rdbFullPermission" runat="server"  Text="Full Permission" GroupName="Permission"/>
                            
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                 <li style="width: 500px;">
                    <div>
                        <label style="width: 60px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">
                               
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px; padding-left: 6px;" class="qlcbFormItem">
                             <asp:CheckBox ID="cbApplyAllUser" AutoPostBack="true" OnCheckedChanged="cbApplyAllUser_CheckedChanged" runat="server" Text="Apply for all user in Group" />
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
              

                <li style="width: 500px;">
                    <div>
                        <label style="width: 60px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Groups
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlGroup" runat="server" CssClass="min25Percent" Width="400px" 
                                AutoPostBack="True" OnSelectedIndexChanged="ddlGroup_SelectedIndexChange"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 60px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Users
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlUser" runat="server" CssClass="min25Percent" Width="400px"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                 
                <li style="width: 500px; padding-top: 10px; padding-bottom: 3px; text-align: center">
                    <telerik:RadButton ID="btnSave" runat="server" Text="Add Permission" OnClick="btnSave_Click" 
                        Width="135" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                </li>
                
                <li style="width: 500px;">
                    
                </li>
                 <li style="width: 500px; padding-top: 10px; padding-bottom: 3px; text-align: center">
                    <telerik:RadButton ID="btnClearAll" runat="server" Text="Clear All User" OnClick ="btnClearAll_Click" 
                        Width="135" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/delete.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                </li>
            </ul>
        </div>
        <div style="width: 100%">
            <telerik:RadGrid ID="grdPermission"
                    runat="server" AllowPaging="True" AutoGenerateColumns="False" CellSpacing="0" CellPadding="0" 
                    PageSize="100" Height="300" GridLines="None"
                    Skin="Windows7"
                    OnNeedDataSource="grdPermission_OnNeedDataSource"
                OnDeleteCommand="grdPermission_OnDeteleCommand">
                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                <MasterTableView DataKeyNames="ID" ClientDataKeyNames="ID" Width="100%" >
                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; records." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="IsGroup" UniqueName="IsGroup" Display="False" />
                                
                        <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" 
                            ConfirmText="Do you want to delete item?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                            <HeaderStyle Width="25" />
                            <ItemStyle HorizontalAlign="Center" Width="25"  />
                        </telerik:GridButtonColumn>

                        <telerik:GridBoundColumn DataField="Name" HeaderText="Users" UniqueName="colName" 
                            ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                            <HeaderStyle Width="50%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle Width="50%" HorizontalAlign="Left"></ItemStyle>
                        </telerik:GridBoundColumn>
                                
                        <telerik:GridTemplateColumn HeaderText="Read" UniqueName="IsRead" AllowFiltering="False">
                                <HeaderStyle Width="20%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle Width="20%" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="Image1" runat="server" 
                                    Visible='<%# !Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsFullPermission"))  %>'
                                    ImageUrl='<%# !Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsFullPermission")) ? "~/Images/ok.png" : "" %>'/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Full Permission" UniqueName="IsFullPermission" AllowFiltering="False" >
                                <HeaderStyle Width="20%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle Width="20%" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="Image2" runat="server" 
                                    Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsFullPermission"))%>'
                                    ImageUrl='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsFullPermission")) ? "~/Images/ok.png" : "" %>'/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <CommandItemStyle Height="25px"></CommandItemStyle>
                </MasterTableView>
                <ClientSettings>
                    <Selecting AllowRowSelect="true"></Selecting>
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True" ScrollHeight="500"></Scrolling>
                </ClientSettings>
            </telerik:RadGrid>

        </div>
         
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
         <asp:HiddenField runat="server" ID="DeleteAll"/>
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="ddlGroup">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlUser" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                       <telerik:AjaxUpdatedControl ControlID="grdPermission" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="grdPermission">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdPermission" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlUser" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlGroup" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="cbApplyAllUser">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID ="ddlUser" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdPermission" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlUser" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlGroup" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="cbApplyAll" LoadingPanelID="RadAjaxLoadingPanel2"/>
                       <%-- <telerik:AjaxUpdatedControl ControlID="cbApplyAllUser" LoadingPanelID="RadAjaxLoadingPanel2" />--%>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rdbRead">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="DivExpirationDate"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rdbFullPermission">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="DivExpirationDate"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ViewOnly">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="DivExpirationDate"/>
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
                //function Confirm() {
                //    var confirm_value = document.createElement("INPUT");
                //    confirm_value.type = "hidden";
                //    confirm_value.name = "confirm_value";
                //    if (confirm("Are you sure want to delete all user?")) {
                //        confirm_value.value = "Yes";
                //    } else {
                //        confirm_value.value = "No";
                //    }
                //    document.forms[0].appendChild(confirm_value);
                //}
            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
