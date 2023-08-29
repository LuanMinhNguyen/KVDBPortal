<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CorrespondenceInfoEditForm.aspx.cs" Inherits="EDMs.Web.Controls.Document.CorrespondenceInfoEditForm" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
	        overflow:auto;
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

        #ddlParent_Input {
            width: 384px !important;
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
       
        <div style="width: 100%">
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
       
         <div style="width: 100%">
            <ul style="list-style-type: none">
                <li style="width: 500px;" Runat="server" ID="UploadControl">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Upload file
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                            <telerik:RadAsyncUpload runat="server" ID="docuploader"
                                MultipleFileSelection="Disabled" MaxFileInputsCount="1" TemporaryFileExpiration="05:00:00" 
                                EnableInlineProgress="true" Width="250px"
                                Localization-Cancel="Cancel" CssClass="min25Percent qlcbFormRequired"
                                Localization-Remove="Remove" Localization-Select="Select"  
                                OnFileUploaded="docuploader_FileUploaded" OnClientFileUploaded="OnClientFilesUploaded">
                            </telerik:RadAsyncUpload>
                            <%--<telerik:RadButton ID="btnCheckDoc" runat="server" Text="OK" Width="50px" style="text-align: center" >
                                <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                            </telerik:RadButton>--%>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
            </ul>
        </div>
        <div style="width: 100%" runat="server" ID="divContent">
            <ul style="list-style-type: none">
                   <li style="width: 500px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">File Name
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="divFileName">
                            <asp:TextBox ID="txtName" runat="server" Style="width: 300px;" Enabled="false" CssClass="min25Percent qlcbFormRequired"/>
                            
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">M Number <span style="color:red">*</span>
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="divDocNo">
                            <asp:TextBox ID="txtDocNumber" runat="server" Style="width: 400px;" CssClass="min25Percent qlcbFormRequired"/>
                              <asp:CustomValidator runat="server" ID="fileNameValidator" CssClass="dnnFormMessage dnnFormErrorModuleEdit" ValidateEmptyText="true"
                            OnServerValidate="ServerValidationFileNameIsExist" Display="Dynamic" ControlToValidate="txtDocNumber"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                 <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 3px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Issue Date <span style="color:red">*</span>
                            </span>
                        </label>
                        <div style="float: left;  padding-top: 5px;"  class="qlcbFormItem" runat="server" id="divIssueDate">
                            <telerik:RadDatePicker ID="txtIssueDate"  runat="server" 
                                ShowPopupOnFocus="True" CssClass=" min25Percent qlcbFormRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>
                            <asp:CustomValidator runat="server" ID="ValidatorIssueDate" ValidateEmptyText="True" CssClass="dnnFormMessage dnnFormErrorModule"
                            OnServerValidate="ValidatorIssueDate_ServerValidate" Display="Dynamic" ControlToValidate="txtIssueDate"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                  <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Reference Docs
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtReferenceDocs" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                 <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 3px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Answer Request
                            </span>
                        </label>
                        <div style="float: left;  padding-top: 5px;"  class="qlcbFormItem">
                            <telerik:RadDatePicker ID="txtAnswerRequest"  runat="server" 
                                ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                 <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Document Title <span style="color:red">*</span>
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="divTitle">
                            <asp:TextBox ID="txtDocumentTitle" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                             <asp:CustomValidator runat="server" ID="ValidatorTitle" ValidateEmptyText="True" CssClass="dnnFormMessage dnnFormErrorModule"
                            OnServerValidate="ValidatorTitle_ServerValidate" Display="Dynamic" ControlToValidate="txtDocumentTitle"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                 <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Discipline 
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlDiscipline" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416PX"  AllowCustomText="true" Skin="Windows7" MarkFirstMatch="true"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                   <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Lead <span style="color:red">*</span>
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" ID="divLead">
                            <telerik:RadComboBox ID="ddlLead" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416PX" CheckBoxes="true" Skin="Windows7" />
                              <asp:CustomValidator runat="server" ID="ValidatorLead" ValidateEmptyText="True" CssClass="dnnFormMessage dnnFormErrorModule"
                            OnServerValidate="ValidatorLead_ServerValidate" Display="Dynamic" ControlToValidate="ddlLead"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                   <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Information
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadComboBox ID="ddlInformation" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416PX" CheckBoxes="true"  Skin="Windows7"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                 <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Reply
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtReply" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
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
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtRemark" runat="server" Style="width: 400px;" CssClass="min25Percent" Rows="2" TextMode="MultiLine"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                 <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">From Unit <span style="color:red">*</span>
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="divFrom">
                            <telerik:RadComboBox ID="ddlFrom" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416PX" AllowCustomText="true"  Skin="Windows7" MarkFirstMatch="true"/>
                               <asp:CustomValidator runat="server" ID="ValidatorFrom" ValidateEmptyText="True" CssClass="dnnFormMessage dnnFormErrorModule"
                            OnServerValidate="ValidatorFrom_ServerValidate" Display="Dynamic" ControlToValidate="ddlFrom"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                 <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">To Unit <span style="color:red">*</span>
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="divTo">
                            <telerik:RadComboBox ID="ddlTo" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416PX" AllowCustomText="true"  Skin="Windows7" MarkFirstMatch="true"/>
                               <asp:CustomValidator runat="server" ID="ValidatorTo" ValidateEmptyText="True" CssClass="dnnFormMessage dnnFormErrorModule"
                            OnServerValidate="ValidatorTo_ServerValidate" Display="Dynamic" ControlToValidate="ddlTo"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                 <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Cos Type <span style="color:red">*</span>
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="divCosType">
                            <telerik:RadComboBox ID="ddlCosType" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416PX"  Skin="Windows7" MarkFirstMatch="true"/>
                               <asp:CustomValidator runat="server" ID="ValidatorCosType" ValidateEmptyText="True" CssClass="dnnFormMessage dnnFormErrorModule"
                            OnServerValidate="ValidatorCosType_ServerValidate" Display="Dynamic" ControlToValidate="ddlCosType"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                <div class="qlcbFormItem" runat="server" ID="CreatedInfo" Visible="False">
                    <div class="dnnFormMessage dnnFormInfo">
                        <div class="dnnFormItem dnnFormHelp dnnClear">
                            <p class="dnnFormRequired" style="float: left;">
                                    <asp:Label ID="lblCreated" runat="server" ></asp:Label>
                                    <asp:Label ID="lblUpdated" runat="server" ></asp:Label>
                            </p>
                            <br />
                        </div>
                    </div>
                </div>
            </ul>
        </div>
        
         <div style="width: 100%">
            <ul style="list-style-type: none">
                <li style="width: 500px; padding-top: 10px; padding-bottom: 3px; text-align: center">
                    <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="70px" style="text-align: center"
>
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                    <%--<telerik:RadButton ID="btncancel" runat="server" Text="Cancel" Width="70px" style="text-align: center"
                        OnClick="btncancel_Click">
                        <Icon PrimaryIconUrl="../../Images/Cancel.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>--%>

                </li>
            </ul>
        </div>
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;
                function OnClientFilesUploaded(sender, args) {
                    var name = args.get_fileName();
                    document.getElementById("txtName").value = name;
                    $find('<%=ajaxDocument.ClientID %>').ajaxRequest();
                }

                function pageLoad() {
                    ajaxManager = $find("<%=ajaxDocument.ClientID %>");
                }

               
                function fileUploading(sender, args) {
                    var name = args.get_fileName();
                    document.getElementById("txtName").value = name;
                    
                    ajaxManager.ajaxRequest("CheckFileName$" + name);
                }
                
               
             
              
            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
