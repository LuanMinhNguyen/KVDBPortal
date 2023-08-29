<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractEditForm.aspx.cs" Inherits="EDMs.Web.Controls.CostContract.ContractEditForm" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
	        overflow:visible;
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
               border-bottom: none !important;
           }
           .RadUpload .ruFileWrap {
               overflow: visible !important;
           }

        .accordion dt a
        {
            letter-spacing: -0.03em;
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
            letter-spacing: -0.03em;
            line-height: 1.2;
            margin: 0.5em auto 0.6em;
            padding: 0;
            text-align: left;
            text-decoration: none;
            display: block;
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
        <div style="width: 100%; height: 100%" runat="server" ID="divContent">
            <ul style="list-style-type: none">
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Project
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtProjectName" runat="server" Style="width: 300px;" CssClass="min25Percent" ReadOnly="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Procurement Requirement
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlPR" runat="server" CssClass="min25Percent" Width="316px"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Contract Number
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtContractNumber" runat="server" Style="width: 300px;" CssClass="min25Percent qlcbFormRequired"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Contractor
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <%--<asp:DropDownList ID="ddlContractor" runat="server" CssClass="min25Percent" Width="316px"/>--%>
                            <asp:TextBox ID="txtContractor" runat="server" Style="width: 300px;" CssClass="min25Percent"/>

                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Contract Content
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtContractContent" runat="server" Style="width: 300px;" CssClass="min25Percent" TextMode="MultiLine" Rows="4"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 3px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Delivery Date
                            </span>
                        </label>
                        <div style="float: left;  padding-top: 5px;"  class="qlcbFormItem">
                            <telerik:RadDatePicker ID="txtDeliveryDate"  runat="server" 
                                ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Delivery Status
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtDeliveryStatus" runat="server" Style="width: 300px;" CssClass="min25Percent" TextMode="MultiLine" Rows="2"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 3px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Effected Date
                            </span>
                        </label>
                        <div style="float: left;  padding-top: 5px;"  class="qlcbFormItem">
                            <telerik:RadDatePicker ID="txtEffectedDate"  runat="server" 
                                ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 3px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">End Date
                            </span>
                        </label>
                        <div style="float: left;  padding-top: 5px;"  class="qlcbFormItem">
                            <telerik:RadDatePicker ID="txtEndDate"  runat="server" 
                                ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Contract Type
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <%--<asp:DropDownList ID="ddlContractType" runat="server" CssClass="min25Percent" Width="316px"/>--%>
                            <asp:TextBox ID="txtContractType" runat="server" Style="width: 300px;" CssClass="min25Percent" TextMode="MultiLine" Rows="2"/>

                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Contract Status
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtContractStatus" runat="server" Style="width: 300px;" CssClass="min25Percent" TextMode="MultiLine" Rows="2"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Notes
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtNote" runat="server" Style="width: 300px;" CssClass="min25Percent" TextMode="MultiLine" Rows="2"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <dl class="accordion">
                    <dt style="width: 100%;">
                        <span>CONTRACT VALUE (Not include 10% VAT)</span>
                    </dt>
                </dl>

                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Exchange Rate
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:radnumerictextbox type="Currency" id="txtExchangeRate" runat="server" 
                                Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent"
                                OnTextChanged="txtExchangeRate_OnTextChanged" AutoPostBack="True">
                                <NumberFormat DecimalDigits="2" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="n" PositivePattern="n"></NumberFormat>
                            </telerik:radnumerictextbox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">VND
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:radnumerictextbox type="Currency" id="txtContractValueVND" runat="server" 
                                Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent"
                                OnTextChanged="txtExchangeRate_OnTextChanged" AutoPostBack="True">
                                <NumberFormat DecimalDigits="2" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="VND n" PositivePattern="VND n"></NumberFormat>
                            </telerik:radnumerictextbox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">USD
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:radnumerictextbox type="Currency" id="txtContractValueUSD" runat="server" 
                                Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent" 
                                OnTextChanged="txtExchangeRate_OnTextChanged" AutoPostBack="True">
                                <NumberFormat DecimalDigits="2" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="$ n" PositivePattern="$ n"></NumberFormat>
                            </telerik:radnumerictextbox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Arising Value
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:radnumerictextbox type="Currency" id="txtArisingValue" runat="server" 
                                Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent"
                                OnTextChanged="txtExchangeRate_OnTextChanged" AutoPostBack="True">
                                <NumberFormat DecimalDigits="2" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="$ n" PositivePattern="$ n"></NumberFormat>
                            </telerik:radnumerictextbox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Contract Total Value
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:radnumerictextbox type="Currency" id="txtContractTotalValue" runat="server" 
                                Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent" ReadOnly="True">
                                <NumberFormat DecimalDigits="2" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="$ n" PositivePattern="$ n"></NumberFormat>
                            </telerik:radnumerictextbox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <div id="PaymentDiv" runat="server">
                <dl class="accordion">
                    <dt style="width: 100%;">
                        <span>PAYMENT INFORMATION</span>
                    </dt>
                </dl>

                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Payment - VND
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:radnumerictextbox type="Currency" id="txtPaymentVND" runat="server" 
                                Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent">
                                <NumberFormat DecimalDigits="2" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="VND n" PositivePattern="VND n"></NumberFormat>
                            </telerik:radnumerictextbox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Payment - USD
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:radnumerictextbox type="Currency" id="txtPaymentUSD" runat="server" 
                                Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent">
                                <NumberFormat DecimalDigits="2" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="$ n" PositivePattern="$ n"></NumberFormat>
                            </telerik:radnumerictextbox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Payment - USD Exchange
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:radnumerictextbox type="Currency" id="txtPaymentUSDExchange" runat="server" 
                                Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent">
                                <NumberFormat DecimalDigits="2" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="$ n" PositivePattern="$ n"></NumberFormat>
                            </telerik:radnumerictextbox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Remain Value - VND
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:radnumerictextbox type="Currency" id="txtRemainVND" runat="server" 
                                Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent">
                                <NumberFormat DecimalDigits="2" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="VND n" PositivePattern="VND n"></NumberFormat>
                            </telerik:radnumerictextbox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Remain Value - USD
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:radnumerictextbox type="Currency" id="txtRemainUSD" runat="server" 
                                Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent">
                                <NumberFormat DecimalDigits="2" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="$ n" PositivePattern="$ n"></NumberFormat>
                            </telerik:radnumerictextbox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Remain Value - USD Exchange
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:radnumerictextbox type="Currency" id="txtRemainUSDExchange" runat="server" 
                                Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent">
                                <NumberFormat DecimalDigits="2" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="$ n" PositivePattern="$ n"></NumberFormat>
                            </telerik:radnumerictextbox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Difference With PR Value
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:radnumerictextbox type="Currency" id="txtDifferenceWithPR" runat="server" 
                                Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent">
                                <NumberFormat DecimalDigits="2" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="$ n" PositivePattern="$ n"></NumberFormat>
                            </telerik:radnumerictextbox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                </div>

                <dl class="accordion">
                    <dt style="width: 100%;">
                        <span>PROGRESS INFORMATION</span>
                    </dt>
                </dl>
                
                <li style="width: 500px;" >
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Complete-%
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:radnumerictextbox type="Percent" id="txtComplete" runat="server" Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="70px" CssClass="min25Percent">
                                <NumberFormat DecimalDigits="2"></NumberFormat>
                            </telerik:radnumerictextbox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 500px;">
                    <div>
                        <label style="width: 140px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Weight-%
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:radnumerictextbox type="Percent" id="txtWeight" runat="server" Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="70px" CssClass="min25Percent">
                                <NumberFormat DecimalDigits="2"></NumberFormat>
                            </telerik:radnumerictextbox>
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
                
                <li style="width: 500px; padding-top: 10px; padding-bottom: 3px; text-align: center">
                    <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"  Width="70px" style="text-align: center"
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
                
                <telerik:AjaxSetting AjaxControlID="ddlStatus">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtComplete"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="txtExchangeRate">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtContractTotalValue"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="txtContractValueVND">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtContractTotalValue"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="txtContractValueUSD">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtContractTotalValue"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="txtArisingValue">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtContractTotalValue"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;
                function OnClientFilesUploaded(sender, args) {
                    var name = args.get_fileName();
                    //document.getElementById("txtName").value = name;
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
                
                
                
                function StopPropagation(e) {
                    if (!e) {
                        e = window.event;
                    }

                    e.cancelBubble = true;
                }

                function nodeClicked(sender, args) {
                    var node = args.get_node();
                    if (node.get_checked()) {
                        node.uncheck();
                    } else {
                        node.check();
                    }
                    nodeChecked(sender, args)

                }
            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
