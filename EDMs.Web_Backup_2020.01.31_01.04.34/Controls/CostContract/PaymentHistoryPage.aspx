﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentHistoryPage.aspx.cs" Inherits="EDMs.Web.Controls.CostContract.PaymentHistoryPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
	        overflow:hidden;
        }
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
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
                <li style="width: 700px;">
                    <div>
                        <label style="width: 200px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Contract Number
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtContractNumber" runat="server" Style="width: 300px;" CssClass="min25Percent" ReadOnly="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 700px;">
                    <div>
                        <label style="width: 200px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Contract Content
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtContractContent" runat="server" Style="width: 300px;" CssClass="min25Percent" TextMode="MultiLine" Rows="2" ReadOnly="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 700px;">
                    <div>
                        <label style="width: 200px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Payment Name
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtPaymentName" runat="server" Style="width: 300px;" CssClass="min25Percent qlcbFormRequired"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                
                <table>
                    <tr>
                        <td>
                            <dl class="accordion">
                                <dt style="width: 100%;">
                                    <span>PAYMENT PLAN</span>
                                </dt>
                            </dl>
                
                            <li style="width: 350px;">
                                <div>
                                    <label style="width: 100px; float: left; padding-top: 3px; padding-right: 10px; text-align: right;">
                                        <span style="color: #2E5689; text-align: right; ">Date
                                        </span>
                                    </label>
                                    <div style="float: left;  padding-top: 5px;"  class="qlcbFormItem">
                                        <telerik:RadDatePicker ID="txtPaymentPlanDate"  runat="server" 
                                            ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                            <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                        </telerik:RadDatePicker>
                                    </div>
                                </div>
                                <div style="clear: both; font-size: 0;"></div>
                            </li>

                            <li style="width: 350px;">
                                <div>
                                    <label style="width: 100px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                        <span style="color: #2E5689; text-align: right; ">VND
                                        </span>
                                    </label>
                                    <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                        <telerik:radnumerictextbox type="Currency" id="txtPaymentPlanVND" runat="server" 
                                            Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent">
                                            <NumberFormat DecimalDigits="2" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="VND n" PositivePattern="VND n"></NumberFormat>
                                        </telerik:radnumerictextbox>
                                    </div>
                                </div>
                                <div style="clear: both; font-size: 0;"></div>
                            </li>
                
                            <li style="width: 350px;">
                                <div>
                                    <label style="width: 100px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                        <span style="color: #2E5689; text-align: right; ">USD
                                        </span>
                                    </label>
                                    <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                        <telerik:radnumerictextbox type="Currency" id="txtPaymentPlanUSD" runat="server" 
                                            Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent">
                                            <NumberFormat DecimalDigits="2" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="$ n" PositivePattern="$ n"></NumberFormat>
                                        </telerik:radnumerictextbox>
                                    </div>
                                </div>
                                <div style="clear: both; font-size: 0;"></div>
                            </li>
                        </td>
                        <td>
                            <dl class="accordion">
                                <dt style="width: 100%;">
                                    <span>PAYMENT ACTUAL</span>
                                </dt>
                            </dl>

                            <li style="width: 350px;">
                                <div>
                                    <label style="width: 100px; float: left; padding-top: 3px; padding-right: 10px; text-align: right;">
                                        <span style="color: #2E5689; text-align: right; ">Date
                                        </span>
                                    </label>
                                    <div style="float: left;  padding-top: 5px;"  class="qlcbFormItem">
                                        <telerik:RadDatePicker ID="txtPaymentActualDate"  runat="server" 
                                            ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                            <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                        </telerik:RadDatePicker>
                                    </div>
                                </div>
                                <div style="clear: both; font-size: 0;"></div>
                            </li>

                            <li style="width: 350px;">
                                <div>
                                    <label style="width: 100px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                        <span style="color: #2E5689; text-align: right; ">VND
                                        </span>
                                    </label>
                                    <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                        <telerik:radnumerictextbox type="Currency" id="txtPaymentActualVND" runat="server" 
                                            Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent">
                                            <NumberFormat DecimalDigits="2" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="VND n" PositivePattern="VND n"></NumberFormat>
                                        </telerik:radnumerictextbox>
                                    </div>
                                </div>
                                <div style="clear: both; font-size: 0;"></div>
                            </li>
                
                            <li style="width: 350px;">
                                <div>
                                    <label style="width: 100px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                        <span style="color: #2E5689; text-align: right; ">USD
                                        </span>
                                    </label>
                                    <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                        <telerik:radnumerictextbox type="Currency" id="txtPaymentActualUSD" runat="server" 
                                            Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent">
                                            <NumberFormat DecimalDigits="2" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="$ n" PositivePattern="$ n"></NumberFormat>
                                        </telerik:radnumerictextbox>
                                    </div>
                                </div>
                                <div style="clear: both; font-size: 0;"></div>
                            </li>
                        </td>
                    </tr>
                </table>
                
                <li style="width: 800px; padding-top: 10px; padding-bottom: 3px; text-align: center">
                    <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"  Width="70px" style="text-align: center"
>
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                    
                    <telerik:RadButton ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" Width="70px" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/clear.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>

                </li>
            </ul>
        </div>
        
        <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="True"
            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
            GridLines="None" Skin="Windows7" Height="264"
            OnDeleteCommand="grdDocument_DeleteCommand"
            OnItemCommand="grdDocument_OnItemCommand"
            OnNeedDataSource="grdDocument_OnNeedDataSource" 
            PageSize="100" Style="outline: none; overflow: hidden !important;">
            <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%">
                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Contents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                <ColumnGroups>
                    <telerik:GridColumnGroup HeaderText="Payment Plan" Name="PaymentPlan"
                            HeaderStyle-HorizontalAlign="Center"/>
                    <telerik:GridColumnGroup HeaderText="Payment Actual" Name="PaymentActual"
                            HeaderStyle-HorizontalAlign="Center"/>
                </ColumnGroups>
                <Columns>
                    <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                    <telerik:GridButtonColumn UniqueName="EditColumn" CommandName="EditCmd" ButtonType="ImageButton" ImageUrl="~/Images/edit.png">
                        <HeaderStyle Width="3%" />
                        <ItemStyle HorizontalAlign="Center"  />
                    </telerik:GridButtonColumn>

                    <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" ConfirmText="Do you want to delete content?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                        <HeaderStyle Width="3%" />
                            <ItemStyle HorizontalAlign="Center"/>
                    </telerik:GridButtonColumn>
                <%-- **********************************Comment********************************* --%>
                    
                    <telerik:GridBoundColumn DataField="Name" HeaderText="Name" UniqueName="Name" >
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn DataField="PlanDate" HeaderText="Date" UniqueName="PlanDate"
                        DataFormatString="{0:dd/MM/yyyy}" ColumnGroupName="PaymentPlan">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Left" ForeColor="Blue" />
                    </telerik:GridBoundColumn>
                    
                    <telerik:GridTemplateColumn HeaderText="VND" UniqueName="PlanValueVND"
                        AllowFiltering="false" ColumnGroupName="PaymentPlan">
                        <HeaderStyle HorizontalAlign="Center" Width="100" />
                        <ItemStyle HorizontalAlign="Right" ForeColor="Blue"/>
                        <ItemTemplate>
                            <%# Eval("PlanValueVND") != null && Convert.ToDouble(Eval("PlanValueVND")) != 0.0
                                ? Eval("PlanValueVND", "{0:VND ###,##0.##}")
                                : string.Empty%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    <telerik:GridTemplateColumn HeaderText="USD" UniqueName="PlanValueUSD"
                        AllowFiltering="false" ColumnGroupName="PaymentPlan">
                        <HeaderStyle HorizontalAlign="Center" Width="100" />
                        <ItemStyle HorizontalAlign="Right" ForeColor="Blue"/>
                        <ItemTemplate>
                            <%# Eval("PlanValueUSD") != null && Convert.ToDouble(Eval("PlanValueUSD")) != 0.0
                                ? Eval("PlanValueUSD", "{0:$ ###,##0.##}")
                                : string.Empty%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    <telerik:GridBoundColumn DataField="ActualDate" HeaderText="Date" UniqueName="ActualDate"
                        DataFormatString="{0:dd/MM/yyyy}" ColumnGroupName="PaymentActual">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Left" ForeColor="Red"/>
                    </telerik:GridBoundColumn>
                    
                    <telerik:GridTemplateColumn HeaderText="VND" UniqueName="ActualValueVND"
                        AllowFiltering="false" ColumnGroupName="PaymentActual">
                        <HeaderStyle HorizontalAlign="Center" Width="100" />
                        <ItemStyle HorizontalAlign="Right" ForeColor="Red"/>
                        <ItemTemplate>
                            <%# Eval("ActualValueVND") != null && Convert.ToDouble(Eval("ActualValueVND")) != 0.0
                                ? Eval("ActualValueVND", "{0:VND ###,##0.##}")
                                : string.Empty%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    <telerik:GridTemplateColumn HeaderText="USD" UniqueName="ActualValueUSD"
                        AllowFiltering="false" ColumnGroupName="PaymentActual">
                        <HeaderStyle HorizontalAlign="Center" Width="100" />
                        <ItemStyle HorizontalAlign="Right" ForeColor="Red"/>
                        <ItemTemplate>
                            <%# Eval("ActualValueUSD") != null && Convert.ToDouble(Eval("ActualValueUSD")) != 0.0
                                ? Eval("ActualValueUSD", "{0:$ ###,##0.##}")
                                : string.Empty%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                </Columns>
            </MasterTableView>
            <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="200" UseStaticHeaders="True" />
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
                
                <telerik:AjaxSetting AjaxControlID="btnSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentName" />
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentPlanDate"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentPlanVND"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentPlanUSD"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentActualDate"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentActualVND"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentActualUSD"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnClear">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentName" />
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentPlanDate"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentPlanVND"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentPlanUSD"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentActualDate"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentActualVND"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentActualUSD"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentName" />
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentPlanDate"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentPlanVND"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentPlanUSD"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentActualDate"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentActualVND"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPaymentActualUSD"/>
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
