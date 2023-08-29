<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrackingSailEditForm.aspx.cs" Inherits="EDMs.Web.Controls.WMS.TrackingSailEditForm" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
	        overflow:auto;
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

           .rlbGroup {
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
                  #rtvPIC {
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

        <div style="width: 100%" runat="server" ID="divContent">
            <ul style="list-style-type: none">
            <li style="width: 750px;">
                    
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Code
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div3">
                            <asp:TextBox ID="txtCode" runat="server" Style="width: 500px;" CssClass="min25Percent qlcbFormRequired"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 750px;">
                    
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Facility
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div4">
                            <asp:TextBox ID="txtFacility" runat="server" Style="width: 500px;" CssClass="min25Percent qlcbFormRequired" ReadOnly="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Date
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadDatePicker ID="txtDate"  runat="server" 
                                ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li> 
                
                <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Source
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlSource" runat="server" CssClass="min25Percent" Width="516px" style="max-width: 316px">
                                <Items>
                                    <asp:ListItem Value="1 - SAFETY MEETING" Text="1 - SAFETY MEETING"></asp:ListItem>
                                    <asp:ListItem Value="2 - HAZARD OBSERVATION" Text="2 - HAZARD OBSERVATION"></asp:ListItem>
                                    <asp:ListItem Value="3 - INCIDENT REPORT" Text="3 - INCIDENT REPORT"></asp:ListItem>
                                    <asp:ListItem Value="4 - SITE SAFETY INSPECTION" Text="4 - SITE SAFETY INSPECTION"></asp:ListItem>
                                    <asp:ListItem Value="5 - TOOLBOX MEETINGS" Text="5 - TOOLBOX MEETINGS"></asp:ListItem>
                                    <asp:ListItem Value="6 - RISK ASSESSMENT" Text="6 - RISK ASSESSMENT"></asp:ListItem>
                                    <asp:ListItem Value="7 - PERMIT AUDITS" Text="7 - PERMIT AUDITS"></asp:ListItem>
                                    <asp:ListItem Value="8 - SAFETY ALERTS/BULLETINS" Text="8 - SAFETY ALERTS/BULLETINS"></asp:ListItem>
                                    <asp:ListItem Value="9 - MSM AUDITS" Text="9 - MSM AUDITS"></asp:ListItem>
                                    <asp:ListItem Value="10 - EMERGENCY DRILLS/EXE" Text="10 - EMERGENCY DRILLS/EXE"></asp:ListItem>
                                    <asp:ListItem Value="11 - ENGINEERING CHANGE" Text="11 - ENGINEERING CHANGE"></asp:ListItem>
                                    <asp:ListItem Value="12 - CLASS MARINE NOTICES " Text="12 -CLASS MARINE NOTICES "></asp:ListItem>
                                    <asp:ListItem Value="13 - FIELD OR FSO INCIDENTS" Text="13 - FIELD OR FSO INCIDENTS"></asp:ListItem>
                                    <asp:ListItem Value="14 - WEEKLY DRILL/EXERCISE" Text="14 - WEEKLY DRILL/EXERCISE"></asp:ListItem>
                                    <asp:ListItem Value="15 - INDEPENDENT AUDIT" Text="15 - INDEPENDENT AUDIT"></asp:ListItem>
                                    <asp:ListItem Value="16 - CORPORATE AUDIT" Text="16 - CORPORATE AUDIT"></asp:ListItem>
                                    <asp:ListItem Value="17 - INDEPENDENT SURVEY" Text="17 - INDEPENDENT SURVEY"></asp:ListItem>
                                    <asp:ListItem Value="18 - SUGGESTION BOX INPUT" Text="18 - SUGGESTION BOX INPUT"></asp:ListItem>
                                    <asp:ListItem Value="19 - HAZOPS" Text="19 - HAZOPS"></asp:ListItem>
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li> 
                
                <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Name Observer
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                            <asp:TextBox ID="txtName" runat="server" Style="width: 500px;" CssClass="min25Percent "/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Location
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                            <asp:TextBox ID="txtLocation" runat="server" Style="width: 500px;" CssClass="min25Percent "/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                
                <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Description
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtDescription" runat="server" Style="width: 500px;" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Immediate Action Taken
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtAction" runat="server" Style="width: 500px;" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Proposed further action and close out
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtProposedAction" runat="server" Style="width: 500px;" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Priority
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlPriority" runat="server" CssClass="min25Percent" Width="516px" style="max-width: 316px">
                                <Items>
                                    <asp:ListItem Value="High" Text="High"></asp:ListItem>
                                    <asp:ListItem Value="Medium" Text="Medium"></asp:ListItem>
                                    <asp:ListItem Value="Normal" Text="Normal"></asp:ListItem>
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li> 

                
                 <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Target to close out 
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtTargetClose" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                           <%-- <telerik:RadDatePicker ID="txtTargetClose"  runat="server" 
                                ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>--%>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li> 
                

                <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Action taken to close out
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtActionClose" runat="server" Style="width: 500px;" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Status/ Closed  Date
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtCloseDate" runat="server" Style="width: 500px;" CssClass="min25Percent"/>

                            <%--<telerik:RadDatePicker ID="txtCloseDate"  runat="server" 
                                ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>--%>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li> 
                
                <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">PIC
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
          
 <telerik:RadTreeView RenderMode="Lightweight" ID="rtvPIC" runat="server"  CheckBoxes="True" TriStateCheckBoxes="true" CheckChildNodes="true" Width="500" Height="200"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li> 

                <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">HOC Tracking No
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                            <asp:TextBox ID="txtHOCTrackingNo" runat="server" Style="width: 500px;" CssClass="min25Percent "/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">MSR No.
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                            <asp:TextBox ID="txtMSRStatus" runat="server" Style="width: 500px;" CssClass="min25Percent "/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">WR No.
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                            <asp:TextBox ID="txtWRNo" runat="server" Style="width: 500px;" CssClass="min25Percent "/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">MOC No.
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                            <asp:TextBox ID="txtMOCNo" runat="server" Style="width: 500px;" CssClass="min25Percent "/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">ECR No.
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                            <asp:TextBox ID="txtECRNo" runat="server" Style="width: 500px;" CssClass="min25Percent "/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 750px;">
                    <div>
                        <label style="width: 160px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:CheckBox runat="server" ID="cbComplete" Text="Completed"/>
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
                <li style="width: 750px; padding-top: 10px; padding-bottom: 3px; text-align: center">
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
        
        <asp:HiddenField runat="server" ID="IsView"/>
        <asp:HiddenField runat="server" ID="IsCreate"/>
        <asp:HiddenField runat="server" ID="IsUpdate"/>
        <asp:HiddenField runat="server" ID="IsCancel"/>
        <asp:HiddenField runat="server" ID="IsAttachWF"/>

        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlToList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlAttention" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
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
