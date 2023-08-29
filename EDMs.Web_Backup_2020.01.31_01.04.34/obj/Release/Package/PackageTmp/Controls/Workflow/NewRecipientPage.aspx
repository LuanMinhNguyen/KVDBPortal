
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewRecipientPage.aspx.cs" Inherits="EDMs.Web.Controls.Workflow.NewRecipientPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        form {
            overflow: auto !important;
        }


        div.RadGrid .rgPager .rgAdvPart     
        {     
            display:none;
        }
        div.rgDataDiv {
            overflow: auto !important;
        }

        
        #btnSavePanel {
            display: inline !important;
        }
        .RadAjaxPanel {
            height: 100% !important;
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

         .RadButton .RadButton_Windows7 .rbLinkButton .rbRounded   .add-button {
            padding: 2px;
            width: 24px;
            height: 24px;
            min-width: 0;
            min-height: 0;
            line-height: 16px;
            vertical-align: middle;
            background: url( "../../Images/search.gif" ) center center no-repeat !important;
        }

         #lbAssignment_Header_btnAssignSearch {
            background: url( "../../Images/search.gif" ) center center no-repeat !important;
         }

         #lbInfoOnly_Header_btnInfoSearch {
            background: url( "../../Images/search.gif" ) center center no-repeat !important;
         }

         #lbManagement_Header_btnManagementSearch {
            background: url( "../../Images/search.gif" ) center center no-repeat !important;
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
        <table width="100%">
            <tr>
                <td align="center">
                    <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_OnClick" Width="70px" style="text-align: center">
                        <Icon PrimaryIconUrl="~/Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                </td>
            </tr>
        </table>
        <div runat="server" ID="divContent">
         <dl class="accordion">
            <dt style="width: 100%;">
                <span>W - WORKING PIC</span>
            </dt>
        </dl>
        
        <table>
            <tr>
                <td align="center"><b>User List</b></td>
                <td align="center"><b>Selected Users</b></td>
            </tr>
            <tr>
                <td colspan="2" style="vertical-align: central">
                    <telerik:RadListBox ID="lbAssignment" runat="server" Width="400px" Height="250px"
                        SelectionMode="Multiple" AllowTransfer="true" TransferToID="lbAssignmentSelected"
                        Skin="Windows7">
                        <HeaderTemplate>
                            <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtAssignInfo" EmptyMessage="Search User" Width="320" Skin="Windows7" />
                            <telerik:RadButton RenderMode="Lightweight" runat="server" ID="btnAssignSearch" Text=""
                                OnClick="btnAssignSearch_OnClick" ButtonType="SkinnedButton" Skin="Windows7" CssClass="add-button">
                            </telerik:RadButton>
                        </HeaderTemplate>
                    </telerik:RadListBox>
                    <telerik:RadListBox ID="lbAssignmentSelected" runat="server" Width="400px" Height="250px"
                        SelectionMode="Multiple" Skin="Windows7">
                    </telerik:RadListBox>
                </td>
            </tr>
        </table>
        <dl class="accordion">
            <dt style="width: 100%;">
                <span>I - INFORMATION ONLY</span>
            </dt>
        </dl>
        <table>
            <tr>
                <td align="center"><b>User List</b></td>
                <td align="center"><b>Selected Users</b></td>
            </tr>
            <tr>
                <td colspan="2" style="vertical-align: central">
                    <telerik:RadListBox ID="lbInfoOnly" runat="server" Width="400px" Height="250px"
                        SelectionMode="Multiple" AllowTransfer="true" TransferToID="lbInfoOnlySelected" 
                        Skin="Windows7">
                        <HeaderTemplate>
                            <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtSearchInfo" EmptyMessage="Search User" Width="320" Skin="Windows7" />
                            <telerik:RadButton RenderMode="Lightweight" runat="server" ID="btnInfoSearch" Text=""
                                OnClick="btnInfoSearch_OnClick" ButtonType="SkinnedButton" Skin="Windows7" CssClass="add-button">
                            </telerik:RadButton>
                        </HeaderTemplate>
                    </telerik:RadListBox>
                    <telerik:RadListBox ID="lbInfoOnlySelected" runat="server" Width="400px" Height="250px"
                        SelectionMode="Multiple" Skin="Windows7">
                    </telerik:RadListBox>
                </td>
            </tr>
        </table>
            
        
        <dl class="accordion">
            <dt style="width: 100%;">
                <span>M - MANAGEMENT</span>
            </dt>
        </dl>
        <table>
            <tr>
                <td align="center"><b>User List</b></td>
                <td align="center"><b>Selected Users</b></td>
            </tr>
            <tr>
                <td colspan="2" style="vertical-align: central">
                    <telerik:RadListBox ID="lbManagement" runat="server" Width="400px" Height="250px"
                        SelectionMode="Multiple" AllowTransfer="true" TransferToID="lbManagementSelected" 
                        Skin="Windows7">
                        <HeaderTemplate>
                            <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtSearchManagement" EmptyMessage="Search User" Width="320" Skin="Windows7" />
                            <telerik:RadButton RenderMode="Lightweight" runat="server" ID="btnManagementSearch" Text=""
                                OnClick="btnManagementSearch_OnClick" ButtonType="SkinnedButton" Skin="Windows7" CssClass="add-button">
                            </telerik:RadButton>
                        </HeaderTemplate>
                    </telerik:RadListBox>
                    <telerik:RadListBox ID="lbManagementSelected" runat="server" Width="400px" Height="250px"
                        SelectionMode="Multiple" Skin="Windows7">
                    </telerik:RadListBox>
                </td>
            </tr>
        </table>
        </div>

        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        
        <telerik:RadAjaxManagerProxy  runat="Server" ID="ajaxDocumentControl">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="grdDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnManagementSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lbManagement" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnInfoSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lbInfoOnly" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnAssignSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lbAssignment" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManagerProxy>
        
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="Recipients" runat="server" Title="Recipients Information"
                VisibleStatusbar="false" Height="480" Width="650" Left="120px"
                ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;
          </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
