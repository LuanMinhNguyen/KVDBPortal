<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PECC2CheckinCRSFile.aspx.cs" Inherits="EDMs.Web.Controls.Document.PECC2CheckinCRSFile" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        
        div.RadGrid .rgPager .rgAdvPart     
        {     
            display:none;
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
        #grdDocument_GridData {
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
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager>
        
        <dl class="accordion">
            <dt style="width: 100%;">
                <span>CSR File</span>
            </dt>
        </dl>
        
        <div style="width: 100%">
            <ul style="list-style-type: none">
                <li style="width: 100%;" Runat="server" ID="UploadControl">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Upload file
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                            <telerik:RadAsyncUpload runat="server" ID="docuploader"
                                MultipleFileSelection="Disabled" TemporaryFileExpiration="05:00:00" 
                                EnableInlineProgress="true" Width="350px"
                                Localization-Cancel="Cancel" CssClass="min25Percent qlcbFormRequired"
                                Localization-Remove="Remove" Localization-Select="Select"  Skin="Windows7">
                            </telerik:RadAsyncUpload>
                            <br/>
                            <asp:Label runat="server" style="color: #085b8f;" Text="CRS File is checkin completed." ID="lblmess" Visible="True"></asp:Label>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 100%; text-align: center; padding-top: 10px">
                    <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="70px" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                    <br />
                    <br />
                    <telerik:RadButton ID="btnExportCRS" runat="server" Visible="false" Text="Generate CRS" OnClick="btnExportCRS_OnClick" Width="120px" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/comment.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                </li>
                
                
            </ul>
        </div>
        
         <%--<div style="width: 100%; text-align: center; padding-top: 270px">
           
        </div>--%>
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        <asp:HiddenField runat="server" ID="lblUserId"/>
         <asp:HiddenField runat="server" ID="IsHasCRSFile" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="Windows7"/>
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument" OnAjaxRequest="ajaxDocument_AjaxRequest">
            
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="btnSave">
                    <UpdatedControls>
                       <telerik:AjaxUpdatedControl ControlID="grdAttachCRSFile" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="docuploader" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="lblmess" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="grdAttachCRSFile">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAttachCRSFile" LoadingPanelID="RadAjaxLoadingPanel2"/>
                       
                    </UpdatedControls>
                </telerik:AjaxSetting> 
                
                <telerik:AjaxSetting AjaxControlID="btnConsolidate">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAttachCRSFile" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="btnConsolidate" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                 <telerik:AjaxSetting AjaxControlID="btnExportCRS">
                    <UpdatedControls>
                      <%--  <telerik:AjaxUpdatedControl ControlID="grdAttachCRSFile" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="btnExportCRS" LoadingPanelID="RadAjaxLoadingPanel2"/>--%>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAttachCRSFile" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
         <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows></Windows></telerik:RadWindowManager>
        <telerik:RadScriptBlock ID="RadScriptBlock" runat="server">
            <script type="text/javascript">
                var ajaxManager;
                function ShowMarkupControl(docId) {
                    var userId = document.getElementById("<%= lblUserId.ClientID %>").value;
                    var url = "MarkupControl.aspx?docId=" + docId + "&userId=" + userId;
                    window.open(url,'_blank');
                }
                function Confirm() {
                    var confirm_value = document.createElement("INPUT");
                    confirm_value.type = "hidden";
                    confirm_value.name = "confirm_value";
                    if (confirm("This will completely delete the project. Are you sure?")) {
                        confirm_value.value = "Yes";
                    }
                    else {
                        confirm_value.value = "No";
                    }
                    document.forms[0].appendChild(confirm_value);
                }
                 function pageLoad() {
               
                ajaxManager = $find("<%=ajaxDocument.ClientID %>");
            }
                function CheckFileCRSExit() {

                    //if (document.getElementById("IsHasCRSFile").value == "True") {
                        if (confirm("CRS already exists. Do you want to overwrite it?")) {
                            ajaxManager.ajaxRequest("UploadCRSFile");

                        } else {
                            return;
                        }
                    //} else {
                    //    ajaxManager.ajaxRequest("UploadCRSFile");
                    //}
                }
                
          </script>

        </telerik:RadScriptBlock>
    </form>
</body>
</html>
