
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecipientPageEmail.aspx.cs" Inherits="EDMs.Web.Controls.Library.RecipientPageEmail" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
            overflow: auto !important;
        }
        div.RadGrid .rgPager .rgAdvPart     
        {     
            display:none;
        }
        div.rgDataDiv {
            overflow: auto !important;
        }

        .DropZone1
        {
            width: 300px;
            height: 250px;
            padding-left: 230px;
            background: #fff url(../../Images/placeholder-add.png) no-repeat center center;
            background-color: #357A2B;
            border-color: #CCCCCC;
            color: #767676;
            float: left;
            text-align: center;
            font-size: 16px;
            color: white;
            position: relative;
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

        #rtvApproveUser {
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

        #rtvReviewUser {
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

        #rtvConsolidateUser {
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

        #rtvInfoUser {
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

        #rtvManagementUser {
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
        
        #rtvMatrix {
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
           
        .RadGrid .rgSelectedRow
        {
            background-image : none !important;
            background-color: darkseagreen !important;
        }
    </style>

    <script src="../../Scripts/jquery-1.7.1.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        function CloseAndRebind(args) {
            //GetRadWindow().BrowserWindow.refreshGrid(args);
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
                <span>ASSIGN BY USER AND DISTRIBUTION MATRIX</span>
            </dt>
        </dl>
        
            <div style="width: 100%; display: inline-flex">
               
                <div style="margin-right: 3px; margin-left:150px; align-items:center;" runat="server" id="divReviewUser">
                    <div style="text-align: center;margin-bottom: 3px"><b>To Email</b></div>
                    <div style="float: inherit; padding-bottom: 5px;" class="qlcbFormItem">
                        <asp:Panel ID="panelSearchReviewUser" runat="server" DefaultButton="btnSearchReviewUser">
                            <telerik:RadTextBox runat="server" ID="txtSearchReviewUser" Width="246" CssClass="min25Percent" EmptyMessage="Search User" style="border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3 !important;" /> 
                            <asp:ImageButton ID="btnSearchReviewUser" runat="server" ImageUrl="../../Images/search.png" style="margin-bottom: -5px !important; margin-left: -2px !important;" OnClick="btnSearchReviewUser_OnClick"/>
                        </asp:Panel>
                    </div>

                    <telerik:RadTreeView RenderMode="Lightweight" ID="rtvReviewUser" runat="server"  CheckBoxes="True" TriStateCheckBoxes="true" CheckChildNodes="true" Width="250" Height="400" Skin="Windows7" OnNodeCheck="rtvReviewUser_OnNodeCheck"/>
                </div>
               
                <div style="margin-right: 3px; margin-left:70px;" runat="server" id="divInformationUser">
                    <div style="text-align: center;margin-bottom: 3px"><b>CC Email </b></div>
                    <div style="float: left; padding-bottom: 5px;" class="qlcbFormItem">
                        <asp:Panel ID="panelSearchInfoUser" runat="server" DefaultButton="btnSearchInfoUser">
                            <telerik:RadTextBox runat="server" ID="txtSearchInfoUser" Width="246" CssClass="min25Percent" EmptyMessage="Search User" style="border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3 !important;" /> 
                            <asp:ImageButton ID="btnSearchInfoUser" runat="server" ImageUrl="../../Images/search.png" style="margin-bottom: -5px !important; margin-left: -2px !important;" OnClick="btnSearchInfoUser_OnClick"/>
                        </asp:Panel>
                    </div>

                    <telerik:RadTreeView RenderMode="Lightweight" ID="rtvInfoUser" runat="server"  CheckBoxes="True" TriStateCheckBoxes="true" CheckChildNodes="true" Width="250" Height="400" Skin="Windows7" OnNodeCheck="rtvInfoUser_OnNodeCheck"/>
                </div>
                <div style="margin-right: 3px;" runat="server"  id="divMatrix">
                    <div style="text-align: center;margin-bottom: 3px"><b>Distribution Matrix </b></div>
                    <div style="float: left; padding-bottom: 5px;" class="qlcbFormItem">
                        <asp:Panel ID="panelSearchDM" runat="server" DefaultButton="btnSearchDM">
                            <telerik:RadTextBox runat="server" ID="txtSearchDM" Width="246" CssClass="min25Percent" EmptyMessage="Search Distribution Matrix" style="border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3 !important;" /> 
                            <asp:ImageButton ID="btnSearchDM" runat="server" ImageUrl="../../Images/search.png" style="margin-bottom: -5px !important; margin-left: -2px !important;" OnClick="btnSearchDM_OnClick"/>
                        </asp:Panel>
                    </div>

                    <telerik:RadTreeView RenderMode="Lightweight" ID="rtvMatrix" runat="server"  CheckBoxes="True" TriStateCheckBoxes="true" CheckChildNodes="true" Width="250" Height="400" Skin="Windows7" OnNodeCheck="rtvMatrix_OnNodeCheck"/>
                </div>
            </div>
        </div>

        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="Windows7"/>
        
        <telerik:RadAjaxManagerProxy  runat="Server" ID="ajaxDocumentControl">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="btnSearchDM">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvMatrix" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSearchInfoUser">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvInfoUser" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSearchConsolidateUser">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvConsolidateUser" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSearchReviewUser">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvReviewUser" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSearchApproveUser">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvApproveUser" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="rtvMatrix">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvMatrix" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="rtvInfoUser">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvInfoUser" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="rtvConsolidateUser">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvConsolidateUser" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="rtvReviewUser">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvReviewUser" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="rtvApproveUser">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvApproveUser" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="btnSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2"/>
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
             
                Telerik.Web.UI.RadTreeView.prototype.saveClientState = function () {
                    return "{\"expandedNodes\":" + this._expandedNodesJson +
                    ",\"collapsedNodes\":" + this._collapsedNodesJson +
                    ",\"logEntries\":" + this._logEntriesJson +
                    ",\"selectedNodes\":" + this._selectedNodesJson +
                    ",\"checkedNodes\":" + this._checkedNodesJson +
                    ",\"scrollPosition\":" + Math.round(this._scrollPosition) + "}";
                }
               
                  function pageLoad() {
                    ajaxManager = $find("<%=ajaxDocumentControl.ClientID %>");
                }
          </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
