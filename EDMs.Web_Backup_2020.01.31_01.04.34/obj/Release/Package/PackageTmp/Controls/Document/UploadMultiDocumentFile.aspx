<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadMultiDocumentFile.aspx.cs" Inherits="EDMs.Web.Controls.Document.UploadMultiDocumentFile" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
	        overflow-y:visible;
            overflow-x: hidden
        }

        div.RadGrid .rgPager .rgAdvPart     
        {     
            display:none;
        }
        /*div.rgDataDiv {
            overflow: hidden !important;
        }*/

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

        .ruInputs {
            width: 500px !important;
        }
        /*.ruInputs li:not(:last-child)
        {
            display: none;
        }

        li.ruUploading
        {
            display: none;
        }*/
           
    </style>

    <script src="~/Scripts/jquery-1.7.1.js" type="text/javascript"></script>
    
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

        function getRadWindow() {
            var oWindow = null;

            if (window.radWindow)
                oWindow = window.radWindow;
            else if (window.frameElement.radWindow)
                oWindow = window.frameElement.radWindow;

            return oWindow;
        }

        function clientClose(arg) {
            getRadWindow().close(arg);
        }
            </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager>
        <%--<telerik:RadProgressManager runat="server" ID="RadProgressManager1" />--%>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
        
        <div style="width: 100%">
            <ul style="list-style-type: none">
                <div class="qlcbFormItem">
                    <div class="dnnFormMessage dnnFormInfo">
                        <div class="dnnFormItem dnnFormHelp dnnClear">
                            <p class="dnnFormRequired" style="float: left;">
                                <span style="text-decoration: underline;">Notes</span>: System will be auto detect file name with rule: 
                                <font style="color: #FE2E2E">&lt;DocNumber&gt;</font>_<font style="color: chocolate">&lt;Revision&gt;</font>_<font style="color: lightseagreen">&lt;Any description&gt;</font> to attach to correct document.<br/>
                                (<font style="color: blueviolet">Revision list:</font> 00, 01, ..., 10)<br/><br/>
                                Example: <font style="color: #FE2E2E">NPK00-GL-ML-0001</font>_<font style="color: chocolate">00</font>_<font style="color: lightseagreen">Contractor master document register</font>.pdf
                            </p>
                            <br />
                        </div>
                    </div>
                </div>
            </ul>
        </div>
        <div style="width: 100%">
            <ul style="list-style-type: none">
                
                <li style="width: 100%; text-align: center">
                    <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="70px" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                </li>
                
                <li style="width: 100%;" Runat="server" ID="UploadControl">
                    <div>
                        <label style="width: 110px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Upload multiple file
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                            <telerik:RadAsyncUpload runat="server" ID="docuploader"  Width="250px" CssClass="min25Percent qlcbFormRequired"
                                MultipleFileSelection="Automatic" EnableInlineProgress="false"
                               Skin="Windows7"
                                >
                            </telerik:RadAsyncUpload>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 100%;" Runat="server" ID="blockError" Visible="False">
                    <div>
                        <label style="width: 60px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: red; text-align: right; ">Warning:
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                            <asp:Label runat="server" ID="lblError" Width="500"></asp:Label>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                
            </ul>
        </div>
        
        
         <%--<div style="width: 100%; text-align: center; padding-top: 270px">
           
        </div>--%>
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="btnSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="docuploader" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="blockError" LoadingPanelID="RadAjaxLoadingPanel2"/>

                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">


                ////function uploading_file(sender, args) {
                ////    if ($telerik.isIE7 || $telerik.isIE8)
                ////        $telerik.$(args.get_row()).addClass("ruUploading");
                ////}
          </script>

        </telerik:RadScriptBlock>
    </form>
</body>
</html>
