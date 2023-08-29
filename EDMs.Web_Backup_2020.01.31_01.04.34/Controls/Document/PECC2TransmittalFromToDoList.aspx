<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PECC2TransmittalFromToDoList.aspx.cs" Inherits="EDMs.Web.Controls.Document.PECC2TransmittalFromToDoList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
            overflow-x: hidden;
            overflow-y: auto;
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

         #rtvCCOrganisation {
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
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="Windows7"/>
        
        
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
                <dl class="accordion">
                    <dt style="width: 100%;">
                        <span>Main Detail</span>
                    </dt>
                </dl>
                <li style="width: 600px;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Transmittal No
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="divFileName">
                            <asp:TextBox ID="txtTransmittalNo" runat="server" Style="width: 400px;" CssClass="min25Percent qlcbFormRequired" ReadOnly="True"/>
                            
                            <asp:CustomValidator runat="server" ID="fileNameValidator" ValidateEmptyText="True" CssClass="dnnFormMessage dnnFormErrorModule"
                            OnServerValidate="ServerValidationFileNameIsExist" Display="Dynamic" ControlToValidate="txtTransmittalNo"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 600px">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Subject
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="divDescription">
                            <asp:TextBox ID="txtDescription" runat="server" Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
                            <%--<asp:CustomValidator runat="server" ID="descriptionValidator" ValidateEmptyText="True" CssClass="dnnFormMessage dnnFormErrorModule"
                            OnServerValidate="ServerValidationDescription" Display="Dynamic" ControlToValidate="txtDescription"/>--%>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Ref. Incoming Trans
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtIncomingTrans" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                            <%--<asp:DropDownList ID="ddlIncomingTrans" runat="server" CssClass="min25Percent" Width="416px" Enabled="False"/>--%>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <dl class="accordion" style="display: none">
                    <dt style="width: 100%;">
                        <span>Confidentiality</span>
                    </dt>
                </dl>
                
                <li style="width: 600px; display: none">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Confidentiality
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlConfidentiality" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416px"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <dl class="accordion">
                    <dt style="width: 100%;">
                        <span>Transmittal Classification</span>
                    </dt>
                </dl>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">For Send
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlForSend" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416px">
                                <asp:ListItem Text="Project's Document" Value ="1"/>
                                <asp:ListItem Text="Change Request" Value="2"/>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 600px;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Project Code
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlProjectCode" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416px" OnSelectedIndexChanged="ddlProjectCode_OnSelectedIndexChanged" AutoPostBack="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 600px;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Originating Organisation
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlOriginatingOrganization" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416px"
                                OnSelectedIndexChanged="Organization_OnSelectedIndexChanged" AutoPostBack="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 600px;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Receiving Organisation
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlReceivingOrganization" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416px"
                                OnSelectedIndexChanged="Organization_OnSelectedIndexChanged" AutoPostBack="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">CC Organisation
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadTreeView RenderMode="Lightweight" ID="rtvCCOrganisation" runat="server" CheckBoxes="True" TriStateCheckBoxes="true" CheckChildNodes="true" Width="400" Height="97"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Group
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlGroup" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416px" OnSelectedIndexChanged="ddlGroup_OnSelectedIndexChanged" AutoPostBack="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                 <li style="width: 600px;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Document Categories
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                           <asp:DropDownList ID="ddlCategory" Enabled="false" runat="server" CssClass="min25Percent" Width="416px">
                               <Items>
                                  <%-- <asp:ListItem Value="0" Text="" ></asp:ListItem>--%>
                                  <asp:ListItem Value="1" Text="1. GENERAL"></asp:ListItem>
                                   <asp:ListItem Value="2" Text="2. ENGINEERING"></asp:ListItem>
                                   <asp:ListItem Value="3" Text="3. CONSTRUCTION"></asp:ListItem>
                                   <asp:ListItem Value="4" Text="4. ERECTION"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="5. COMMISSIONING"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="6. PLANT/EQUIPMENT"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="7. O&M"></asp:ListItem>
                                    <asp:ListItem Value="8" Text="8. OTHERS"></asp:ListItem>
                               </Items>
                           </asp:DropDownList>

                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                  <li style="width: 600px;" runat="server" ID="Li1">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Volume Number
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                           <asp:TextBox ID="txtVolumeNumber" ReadOnly="true" runat="server" Style="width: 400px;" CssClass="min25Percent qlcbFormRequired"/>
                        
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                <li style="width: 600px; display: none">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Other Transmittal Number
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                            <asp:TextBox ID="txtOtherTransNo" runat="server" Style="width: 400px;" CssClass="min25Percent"  />
                            <%--<asp:CustomValidator runat="server" ID="descriptionValidator" ValidateEmptyText="True" CssClass="dnnFormMessage dnnFormErrorModule"
                            OnServerValidate="ServerValidationDescription" Display="Dynamic" ControlToValidate="txtDescription"/>--%>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px; display: none;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">From
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtFrom" runat="server" Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" Rows="2" />
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px ; display: none;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">To
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtTo" runat="server" Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" Rows="2" />
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px; display: none;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">CC
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtCC" runat="server" Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" Rows="2" />
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 3px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Issued Date
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" id="divDayIssued">
                            <telerik:RadDatePicker ID="txtDayIssued"  runat="server" CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                 <li style="width: 600px; display: none">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Subject
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtSubject" runat="server" Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" Rows="2" />
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px; display: none;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Transmitted by Name
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlTransmittedByName" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416px" OnSelectedIndexChanged="ddlTransmittedByName_OnSelectedIndexChanged" AutoPostBack="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px; display: none;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Transmitted by Designation
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtTransmittedByDesignation" runat="server" Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" Rows="2" />
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px; display: none;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Acknowledged By Name
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlAcknowledgedByName" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416px" OnSelectedIndexChanged="ddlAcknowledgedByName_OnSelectedIndexChanged" AutoPostBack="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px; display: none;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Acknowledged By Designation
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtAcknowledgedByDesignation" runat="server" Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" Rows="2" />
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px; display: none">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Remarks
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtRemark" runat="server" Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" Rows="3" />
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
                <li style="width: 600px; padding-top: 10px; padding-bottom: 3px; text-align: center">
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
            <telerik:RadGrid AllowCustomPaging="False" AllowPaging="False" AllowSorting="True" 
                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" 
                Height="400" ID="grdDocument"  AllowFilteringByColumn="True" AllowMultiRowSelection="True"
                OnNeedDataSource="grdDocument_OnNeedDataSource" Skin="Windows7"
                runat="server" Style="outline: none" Width="100%">
                <SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                <MasterTableView AllowMultiColumnSorting="false"
                    ClientDataKeyNames="ID" DataKeyNames="ID" CommandItemDisplay="Top" 
                    EditMode="InPlace" Font-Size="8pt">
                    <%--  <GroupByExpressions>
                            <telerik:GridGroupByExpression>
                                <SelectFields>
                                    <telerik:GridGroupByField FieldAlias="-" FieldName="DisciplineFullName" FormatString="{0:D}"
                                        HeaderValueSeparator=""></telerik:GridGroupByField>
                                </SelectFields>
                                <GroupByFields>
                                    <telerik:GridGroupByField FieldName="DisciplineFullName" SortOrder="Ascending" ></telerik:GridGroupByField>
                                </GroupByFields>
                            </telerik:GridGroupByExpression>
                        </GroupByExpressions>    --%>
                    <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="false"/>
                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <ColumnGroups>
                        <%--<telerik:GridColumnGroup HeaderText="First Issue Info" Name="FirstIssueInfo"
                                HeaderStyle-HorizontalAlign="Center"/>
                        <telerik:GridColumnGroup HeaderText="Final Issue Info" Name="FinalIssueInfo"
                                HeaderStyle-HorizontalAlign="Center"/>
                        <telerik:GridColumnGroup HeaderText="Revision Received Info" Name="ReceivedInfo"
                                HeaderStyle-HorizontalAlign="Center"/>
                        <telerik:GridColumnGroup HeaderText="ICA REVIEW DETAILS" Name="ICAReviews"
                                HeaderStyle-HorizontalAlign="Center"/>--%>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                        

                        <telerik:GridTemplateColumn HeaderText="No." Groupable="False" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblSoTT" runat="server" Text='<%# grdDocument.CurrentPageIndex * grdDocument.PageSize + grdDocument.Items.Count+1 %>'>
                                </asp:Label>
                                      
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Document Number" UniqueName="DocNo" DataField="DocNo"
                            ShowFilterIcon="False" FilterControlWidth="97%" 
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                        <HeaderStyle HorizontalAlign="Center" Width="160" />
                        <ItemStyle HorizontalAlign="Left" Width="160" />
                        <ItemTemplate>
                            <asp:Label ID="lblDocNo" runat="server" Text='<%# Eval("DocNo") %>' style="cursor: pointer"/> 
                                            
                        </ItemTemplate>
                        <%--  <EditItemTemplate>
                            <asp:HiddenField ID="DocNo" runat="server" Value='<%# Eval("DocumentNo") %>'/>
                            <asp:Label runat="server" ID="lbldocNo"></asp:Label>
                        </EditItemTemplate>--%>
                    </telerik:GridTemplateColumn>
                                            
                    <telerik:GridTemplateColumn HeaderText="Document Title" UniqueName="DocTitle" DataField="DocTitle"
                        ShowFilterIcon="False" FilterControlWidth="97%" 
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                        <HeaderStyle HorizontalAlign="Center" Width="220" />
                        <ItemStyle HorizontalAlign="Left" Width="220" />
                        <ItemTemplate>
                            <%# Eval("DocTitle") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                    
                    <telerik:GridTemplateColumn HeaderText="Revision" UniqueName="Revision"
                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="Revision"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                        <HeaderStyle HorizontalAlign="Center" Width="55" />
                        <ItemStyle HorizontalAlign="Center" Width="55"/>
                        <ItemTemplate>
                            <%# Eval("Revision") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Status of Review" UniqueName="DocReviewStatusCode"
                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="DocReviewStatusCode"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                        <HeaderStyle HorizontalAlign="Center" Width="50" />
                        <ItemStyle HorizontalAlign="Center" Width="50"/>
                        <ItemTemplate>
                            <%# Eval("DocReviewStatusCode") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Original Doc No." UniqueName="OriginalDocumentNumber" DataField="OriginalDocumentNumber"
                        ShowFilterIcon="False" FilterControlWidth="97%" 
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                        <HeaderStyle HorizontalAlign="Center" Width="160" />
                        <ItemStyle HorizontalAlign="Left" Width="160" />
                        <ItemTemplate>
                            <asp:Label ID="lblContractorDocNo" runat="server" Text='<%# Eval("OriginalDocumentNumber") %>' style="cursor: pointer"/>                                             
                        </ItemTemplate>
                        </telerik:GridTemplateColumn> 
                                        
                    <telerik:GridTemplateColumn HeaderText="Incoming Transmittal" UniqueName="IncomingTransNo" DataField="IncomingTransNo"
                        ShowFilterIcon="False" FilterControlWidth="97%" 
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <%# Eval("IncomingTransNo") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn> 

                          
                </Columns>
            </MasterTableView>
            <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
            <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
            </ClientSettings>
        </telerik:RadGrid>

        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlOriginatingOrganization">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtTransNo"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlAcknowledgedByName"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlTransmittedByName"/>
                        <telerik:AjaxUpdatedControl ControlID="txtAcknowledgedByDesignation"/>
                        <telerik:AjaxUpdatedControl ControlID="txtTransmittedByDesignation"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlReceivingOrganization">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtTransNo"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlAcknowledgedByName"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlTransmittedByName"/>
                        <telerik:AjaxUpdatedControl ControlID="txtAcknowledgedByDesignation"/>
                        <telerik:AjaxUpdatedControl ControlID="txtTransmittedByDesignation"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlGroup">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtTransNo"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlAcknowledgedByName">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtAcknowledgedByDesignation"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlTransmittedByName">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtTransmittedByDesignation"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlProjectCode">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlIncomingTrans"/>
                        <telerik:AjaxUpdatedControl ControlID="txtTransNo"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;
                function OnClientFilesUploaded(sender, args) {
                    var name = args.get_fileName();
                    document.getElementById("txtTransmittalNo").value = name;
                    $find('<%=ajaxDocument.ClientID %>').ajaxRequest();
                }

                function pageLoad() {
                    ajaxManager = $find("<%=ajaxDocument.ClientID %>");
                }

                function ValidateForm() {
                    var name = document.getElementById("<%= txtTransmittalNo.ClientID%>");
                    if (name.value.trim() == "") {
                        alert("Please enter file name.");
                        name.focus();
                        return false;
                    }
                }
                
                function fileUploading(sender, args) {
                    var name = args.get_fileName();
                    document.getElementById("txtTransmittalNo").value = name;
                    
                    ajaxManager.ajaxRequest("CheckFileName$" + name);
                }

            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
