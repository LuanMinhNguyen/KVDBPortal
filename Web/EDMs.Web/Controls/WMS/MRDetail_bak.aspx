<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MRDetail_bak.aspx.cs" Inherits="EDMs.Web.Controls.WMS.MRDetail" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
	        overflow-x:auto;
	        overflow-y:auto;
        }
        #grdCheckList_GridData {
            height: 100% !important;
        }
        #txtSFICodePanel {
            display: inline-block !important;
        }
        .rbSkinnedButton {
            vertical-align: top !important;
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

        .rlbItem
        {
            float:left !important;
        }
        .rlbGroup, .RadListBox
        {
            width:auto !important;
        }

        #Panel1 {
            display: initial !important;
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
    <telerik:RadScriptManager ID="RadScriptManager2" runat="server"/>
        
        <asp:HiddenField runat="server" ID="ObjectType"/>
        <asp:HiddenField runat="server" ID="ObjectId"/>
        <asp:HiddenField runat="server" ID="ObjAssignUserId"/>
        <div style="width: 100%" runat="server" ID="divContent">
            <table width="100%" style="height: 100%; border-collapse: collapse; border: 1px solid black; border-spacing: 5em; padding-bottom: 5px; padding-top: 5px; padding-left: 3px">
                <tr style="padding-bottom: 10px; border: 1px solid black; border-spacing: 5em; padding-bottom: 5px; padding-top: 5px; padding-left: 3px">
                    <td align="center" colspan="10" style="padding-bottom: 10px; border: 1px solid black; border-spacing: 5em; padding-bottom: 5px; padding-top: 5px; padding-left: 3px">
                        <b style="font-size: 25pt">MATERIALS REQUISITION</b><br/>
                        <span style="font-size: 16pt">(Facility: <asp:Label runat="server" ID="lblProjectName"/>)</span>
                        
                        <div >
                            
                            <telerik:RadButton ID="btnComplete" runat="server" Text="Complete & Move Next"  Width="160px" style="text-align: center" Skin="Windows7"
                                OnClientClicked="btnCompleteClicked">
                                <Icon PrimaryIconUrl="../../Images/complete.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                            </telerik:RadButton>
                            <telerik:RadButton ID="btnCompleteFinal" runat="server" Text="Complete"  Width="120px" style="text-align: center" Skin="Windows7"
                                OnClientClicked="btnCompleteFinalClicked">
                                <Icon PrimaryIconUrl="../../Images/complete.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                            </telerik:RadButton>
                            <br/>
                            <telerik:RadButton ID="btnReject" runat="server" Text="Reject"  Width="120px" style="text-align: center" Skin="Windows7" OnClientClicked="btnRejectClicked">
                                <Icon PrimaryIconUrl="../../Images/reject.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                            </telerik:RadButton>
                        </div>
                    </td>
                    
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td></td>
                                        </tr>
                                    </table>
                                </td>
                                
                                <td>
                                    

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr style="border: 1px solid black; border-spacing: 5em; padding-bottom: 5px; padding-top: 5px; padding-left: 3px">
                    <td style="vertical-align: top; width: 350px;border: 1px solid black; border-spacing: 5em; padding-bottom: 5px; padding-top: 5px; padding-left: 3px"><b>AMOS Work Order No.</b> (if applicable) (1): <asp:Label runat="server" ID="lblAMOSNo" Text="FPSO PTSC LAM SON"/></td>
                    <td style="vertical-align: top;width: 300px; border: 1px solid black; border-spacing: 5em; padding-bottom: 5px; padding-top: 5px; padding-left: 3px"><b>MR No.</b> (2): <asp:Label runat="server" ID="lblMRNo" Text="FPSO PTSC LAM SON"/> 
                        <div style="float: right">
                            
                            <telerik:RadButton ID="btnExportMR" runat="server" Text="Export MR Form" OnClick="btnExportMR_Click" Width="160px" style="text-align: center" Skin="Windows7">
                                <Icon PrimaryIconUrl="../../Images/exexcel.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                            </telerik:RadButton>
                            <br/>
                            <telerik:RadButton ID="btnExportComment" runat="server" Text="Export Comment Form" OnClick="btnExportComment_Click" Width="160px" style="text-align: center" Skin="Windows7">
                                <Icon PrimaryIconUrl="../../Images/exexcel.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                            </telerik:RadButton>
                        </div>
                    </td>
                    <td style="vertical-align: top;border: 1px solid black; border-spacing: 5em; padding-bottom: 5px; padding-top: 5px; padding-left: 3px"><b>Department</b> (3): <asp:Label runat="server" ID="lblDepartment" Text="FPSO PTSC LAM SON"/></td>
                </tr>
                
                <tr style="border: 1px solid black; border-spacing: 5em; padding-bottom: 5px; padding-top: 5px; padding-left: 3px">
                    <td colspan="3" style="border: 1px solid black; border-spacing: 5em; padding-bottom: 5px; padding-top: 5px; padding-left: 3px"><b>JUSTIFICATION FOR REQUISITION</b> (4): <asp:Label runat="server" ID="lblJustification" Text="FPSO PTSC LAM SON"/></td>
                    
                </tr>
                <tr>
                    <td colspan="3" style="border: 1px solid black; border-spacing: 5em; padding-bottom: 5px; padding-top: 5px; padding-left: 3px"><b>Please obtain the following</b> - (tick applicable box): <telerik:RadListBox RenderMode="Lightweight" ID="lbMRType" runat="server" CheckBoxes="true" ShowCheckAll="False"  Skin="Windows7"/></td>
                </tr>
                <tr style="border: 1px solid black; border-spacing: 5em; padding-bottom: 5px; padding-top: 5px; padding-left: 3px">
                    <td style="border: 1px solid black; border-spacing: 5em; padding-bottom: 5px; padding-top: 5px; padding-left: 3px">
                        <b>Date Required </b>(18): <asp:Label runat="server" ID="lblDateRequire" Text="FPSO PTSC LAM SON"/>
                    </td>
                    <td colspan="2" style="border: 1px solid black; border-spacing: 5em; padding-bottom: 5px; padding-top: 5px; padding-left: 3px">
                        <b>Priority Level </b>(19): <telerik:RadListBox RenderMode="Lightweight" ID="lbPriority" runat="server" CheckBoxes="true" ShowCheckAll="False" Skin="Windows7"/>
                    </td>
                </tr >
                
                <tr>
                    <td colspan="3">
                        <table>
                            <tr>
                                <td style="padding-left: 10px"><b>Originator</b></td>
                                <td><div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                        <asp:TextBox ID="txtOriginator" runat="server" Style="width: 150px;min-width: 150px !important" CssClass="min25Percent"/>
                                    </div>

                                </td>
                                
                                <td style="padding-left: 10px"><b>Storeman</b></td>
                                <td><div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                        <asp:TextBox ID="txtStoremanName" runat="server" Style="width: 150px;min-width: 150px !important" CssClass="min25Percent"/>
                                    </div>

                                </td>
                                <td style="padding-left: 10px"><b>Supervisor</b></td>
                                <td><div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                        <asp:TextBox ID="txtSupervisorName" runat="server" Style="width: 150px;min-width: 150px !important" CssClass="min25Percent"/>
                                    </div>

                                </td>
                                <td style="padding-left: 10px"><b>OIM/FM</b></td>
                                <td><div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                        <asp:TextBox ID="txtOIMName" runat="server" Style="width: 150px;min-width: 150px !important" CssClass="min25Percent"/>
                                    </div>

                                </td>
                                <td style="padding-left: 20px">
                                    <telerik:RadButton ID="btnSaveMRSignInfo" runat="server" Text="Save" OnClick="btnSaveMRSignInfo_Click" Width="70px" style="text-align: center">
                                            <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                        </telerik:RadButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 10px">Date</td>
                                <td>
                                    <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                        <telerik:RadDatePicker ID="txtOriginatorDate"  runat="server" 
                                            ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                            <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                        </telerik:RadDatePicker>
                                    </div>

                                </td>
                                <td style="padding-left: 10px">Date</td>
                                <td>
                                    <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                        <telerik:RadDatePicker ID="txtStoremanDate"  runat="server" 
                                            ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                            <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                        </telerik:RadDatePicker>
                                    </div>

                                </td>
                                <td style="padding-left: 10px">Date</td>
                                <td>
                                    <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                        <telerik:RadDatePicker ID="txtSupervisorDate"  runat="server" 
                                            ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                            <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                        </telerik:RadDatePicker>
                                    </div>

                                </td>
                                <td style="padding-left: 10px">Date</td>
                                <td>
                                    <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                        <telerik:RadDatePicker ID="txtOIMDate"  runat="server" 
                                            ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                            <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                        </telerik:RadDatePicker>
                                    </div>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr style="height: 100%; border: 1px solid black; border-spacing: 5em; padding-bottom: 5px; padding-top: 5px; padding-left: 3px">
                    <td colspan="3">
                        <telerik:RadTabStrip ID="RadTabStrip1" runat="server"  width="100%" Skin="MetroTouch" MultiPageID="RadMultiPage1"
            SelectedIndex="0">
                            <Tabs>
                                <telerik:RadTab ImageUrl="~/Images/material2.png" Text="Materials Detail">
                                </telerik:RadTab>
                                <telerik:RadTab ImageUrl="~/Images/checklist1.png" Text="Check List Form">
                                </telerik:RadTab>
                                <telerik:RadTab ImageUrl="~/Images/attach1.png" Text="Attach Files">
                                </telerik:RadTab>
                                <telerik:RadTab ImageUrl="~/Images/comment3.png" Text="Comment">
                                </telerik:RadTab>
                                
                                <telerik:RadTab ImageUrl="~/Images/receive.png" Text="Received Info">
                                </telerik:RadTab>
                                
                                <telerik:RadTab ImageUrl="~/Images/trackinglist1.png" Text="Tracking Info">
                                </telerik:RadTab>
                            </Tabs>
                        </telerik:RadTabStrip>
                        
                        <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0">
                        <telerik:RadPageView runat="server" Height="100%" ID="RadPageView1">
                        <div runat="server" ID="divMRDetail">
                            <fieldset>
                                <legend>Material Info:</legend>
                                
                                <table>
                                    <tr>
                                        <td align="right">SFI Code</td>
                                        <td colspan="2" valign="top">
                                            <div style="padding-top: 5px;" class="qlcbFormItem">
                                                <%--<asp:TextBox runat="server" ID="txtSFICode" Style="width: 120px; min-width: 50px" CssClass="min25Percent qlcbFormRequired"/> --%>
                                                <asp:Panel ID="Panel1" runat="server" Width="116px">
                                                <telerik:RadMaskedTextBox RenderMode="Lightweight" runat="server" Mask="###.###.###.###" Width="116px"
                                                    ID="txtSFICode" CssClass="min25Percent qlcbFormRequired" Style=" min-width: 0px !important;">
                                                </telerik:RadMaskedTextBox>
                                                </asp:Panel>
                                                &nbsp;
                                                <telerik:RadButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Width="25px" style="text-align: center">
                                                    <Icon PrimaryIconUrl="../../Images/search.png" PrimaryIconLeft="3" PrimaryIconTop="2" PrimaryIconWidth="16" PrimaryIconHeight="16"/>
                                                </telerik:RadButton>
                                                
                                            </div>
                                            <%--<div>
                                                <img  src="../../Images/loading.gif" />
                                            </div>--%>
                                            
                                            
                                        </td>
                                        <td align="right" colspan="2">Reference MR</td>
                                        <td colspan="3" valign="top">
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div9">
                                                <asp:DropDownList ID="ddlRefMR" runat="server" CssClass="min25Percent" Width="270" style="max-width: 506px">
                                                    <Items>
                                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                                    </Items>
                                                </asp:DropDownList>
                                            </div>
                                        </td>

                                        
                                    </tr>
                                    
                                    
                                    <tr>
                                        <td align="right">Req ROB Min</td>
                                        <td >
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                <telerik:radnumerictextbox type="Number" id="txtROBMin" runat="server" Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="116px" CssClass="min25Percent">
                                                    <NumberFormat DecimalDigits="0"></NumberFormat>
                                                </telerik:radnumerictextbox>
                                            </div>
                                        </td>
                                    
                                        <td align="right">Req ROB Max</td>
                                        <td>
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                <telerik:radnumerictextbox type="Number" id="txtROBMax" runat="server" Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="66px" CssClass="min25Percent">
                                                    <NumberFormat DecimalDigits="0"></NumberFormat>
                                                </telerik:radnumerictextbox>
                                            </div>
                                        </td>
                                    
                                        <td align="right">ROB</td>
                                        <td>
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                <telerik:radnumerictextbox type="Number" id="txtROB" runat="server" Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="66px" CssClass="min25Percent">
                                                    <NumberFormat DecimalDigits="0"></NumberFormat>
                                                </telerik:radnumerictextbox>
                                            </div>
                                        </td>
                                    
                                        <td align="right">Quantity Required</td>
                                        <td>
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                <telerik:radnumerictextbox type="Number" id="txtQuantityReq" runat="server" Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="66px" CssClass="min25Percent">
                                                    <NumberFormat DecimalDigits="0"></NumberFormat>
                                                </telerik:radnumerictextbox>
                                            </div>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="right">Quantity Remark: Use for job</td>
                                        <td>
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                <telerik:radnumerictextbox type="Number" id="txtUseForJob" runat="server" Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="116px" CssClass="min25Percent">
                                                    <NumberFormat DecimalDigits="0"></NumberFormat>
                                                </telerik:radnumerictextbox>
                                            </div>
                                        </td>
                                        
                                        <td colspan="3" align="right">Quantity Remark: For spares</td>
                                        <td>
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                <telerik:radnumerictextbox type="Number" id="txtForSpare" runat="server" Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="66px" CssClass="min25Percent">
                                                    <NumberFormat DecimalDigits="0"></NumberFormat>
                                                </telerik:radnumerictextbox>
                                            </div>
                                        </td>
                                    
                                        <td align="right">Units</td>
                                        <td>
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                <asp:TextBox runat="server" ID="txtUnit"  Style="width: 50px; min-width: 50px" CssClass="min25Percent"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="right">Material Name</td>
                                        <td colspan="7">
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                                                <asp:TextBox runat="server" ID="txtMaterialName" Style="width: 615px; min-width: 50px" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
                                            </div>
                                        </td>
                                    
                                        
                                    </tr>
                                    <tr>
                                        <td>Maker Name/ Maker Refer</td>
                                        <td>
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                <asp:TextBox runat="server" ID="txtMakerRef"  Style="width: 100px; min-width: 50px" CssClass="min25Percent"></asp:TextBox>
                                            </div>
                                        </td>
                                        
                                        <td align="right">Certificate Required</td>
                                        <td>
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div1">
                                                <asp:DropDownList ID="ddlCertReq" runat="server" CssClass="min25Percent" Width="66" style="max-width: 5006px">
                                                    <Items>
                                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                                        <asp:ListItem Value="Y" Text="Y"></asp:ListItem>
                                                        <asp:ListItem Value="N" Text="N"></asp:ListItem>
                                                    </Items>
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                        
                                        <td>Alternative</td>
                                        <td>
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                <asp:DropDownList ID="ddlAlter" runat="server" CssClass="min25Percent" Width="66" style="max-width: 5006px">
                                                    <Items>
                                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                                        <asp:ListItem Value="Y" Text="Y"></asp:ListItem>
                                                        <asp:ListItem Value="N" Text="N"></asp:ListItem>
                                                    </Items>
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                        
                                        <td align="right">Normal Using Frequency</td>
                                        <td>
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div2">
                                                <asp:TextBox runat="server" ID="txtFrequency"  Style="width: 50px; min-width: 50px" CssClass="min25Percent"></asp:TextBox>
                                            </div>
                                        </td>
                                        
                                        <td align="right">Remark</td>
                                        <td colspan="3">
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                <asp:TextBox runat="server" ID="txtRemark"  Style="width: 150px; min-width: 50px" CssClass="min25Percent"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="20" align="center" style="padding-top: 10px; padding-bottom: 5px">
                                            <telerik:RadButton ID="btnMaterialDetailSave" runat="server" Text="Save" OnClick="btnMaterialDetailSave_Click" Width="70px" style="text-align: center">
                                            <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                        </telerik:RadButton>
                                            
                                            <telerik:RadButton ID="btnMaterialDetailClear" runat="server" Text="Clear" OnClick="btnMaterialDetailClear_Click" Width="70px" style="text-align: center">
                                                <Icon PrimaryIconUrl="../../Images/clear.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                
                                </table>
                            </fieldset>
                            </div>
                            
                            <telerik:RadGrid ID="grdMaterials" runat="server" AllowPaging="False"
                                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                GridLines="None" Skin="Windows7" Height="300" AllowFilteringByColumn="False"
                                OnDeleteCommand="grdMaterials_DeleteCommand"
                                OnItemCommand="grdMaterials_OnItemCommand"
                                OnItemDataBound="grdMaterials_ItemDataBound" 
                                OnNeedDataSource="grdMaterials_OnNeedDataSource" 
                                OnItemCreated="grdMaterials_OnItemCreated"
                                PageSize="100" Style="outline: none; overflow: hidden !important;">
                                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                                <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%">
                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Contents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ColumnGroups>
                                        <telerik:GridColumnGroup HeaderText="Req ROB (6)" Name="ReqROB" HeaderStyle-HorizontalAlign="Center"/>
                                        <telerik:GridColumnGroup HeaderText="Quantity Remark (9)" Name="QuantityRemark" HeaderStyle-HorizontalAlign="Center"/>
                                    </ColumnGroups>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                                        <telerik:GridBoundColumn DataField="IsCancel" UniqueName="IsCancel" Display="False" />

                                        <telerik:GridButtonColumn UniqueName="EditColumn" CommandName="EditCmd" ButtonType="ImageButton" ImageUrl="~/Images/edit.png">
                                            <HeaderStyle Width="30" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                        </telerik:GridButtonColumn>

                                        <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Cancle" ConfirmText="Do you want to cancel MR Detail?" ButtonType="ImageButton" ImageUrl="~/Images/cancel1.png">
                                            <HeaderStyle Width="30" />
                                                <ItemStyle HorizontalAlign="Center"/>
                                        </telerik:GridButtonColumn>
                                        
                                        <telerik:GridTemplateColumn>
                                                <HeaderStyle Width="30"  />
                                                <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Image ID="HistoryLink" runat="server" ImageUrl="~/Images/history.png" Style="cursor: pointer;" AlternateText="MR Detail update history" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                    <%-- **********************************Comment********************************* --%>
                                        
                                        <telerik:GridTemplateColumn HeaderText="No." Groupable="False" AllowFiltering="false" Display="False">
                                            <HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSoTT" runat="server" Text='<%# grdMaterials.CurrentPageIndex * grdMaterials.PageSize + grdMaterials.Items.Count+1 %>'>
                                                </asp:Label>
                                      
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Status" UniqueName="Status">
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Convert.ToBoolean(Eval("IsCancel"))
                                                                ? "Canceled"
                                                                : "Open"%>'>
                                                </asp:Label>
                                      
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="CreatedByName" HeaderText="Updated By" UniqueName="CreatedByName">
                                            <HeaderStyle HorizontalAlign="Center" Width="200" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridDateTimeColumn HeaderText="Updated Date" UniqueName="CreatedDate" DataField="CreatedDate"
                                                DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center"/>
                                        </telerik:GridDateTimeColumn>

                                        <telerik:GridBoundColumn DataField="ReqROBMin" HeaderText="Min" UniqueName="ReqROBMin" ColumnGroupName="ReqROB">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="ReqROBMax" HeaderText="Max" UniqueName="ReqROBMax" ColumnGroupName="ReqROB">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="ROB" HeaderText="ROB" UniqueName="ROB">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="QtyReq" HeaderText="Quantity Required" UniqueName="QtyReq">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="QtyRemarkUseForJob" HeaderText="Use for job" UniqueName="QtyRemarkUseForJob" ColumnGroupName="QuantityRemark">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="QtyRemarkForSpare" HeaderText="For spares" UniqueName="QtyRemarkForSpare" ColumnGroupName="QuantityRemark">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="Units" HeaderText="Units" UniqueName="Units">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="SFICode" HeaderText="SFI Code" UniqueName="SFICode">
                                            <HeaderStyle HorizontalAlign="Center" Width="160" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="Description" HeaderText="Material Name & Specification Descriptions " UniqueName="Description">
                                            <HeaderStyle HorizontalAlign="Center" Width="300" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="MakerName" HeaderText="Maker Name/ Maker Refer" UniqueName="MakerName">
                                            <HeaderStyle HorizontalAlign="Center" Width="100" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="CertificateRequired" HeaderText="Certificate Required (Y/N)" UniqueName="CertificateRequired">
                                            <HeaderStyle HorizontalAlign="Center" Width="100" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="Alternative" HeaderText="Alternative (Y/N)" UniqueName="Alternative">
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="NormalUsingFrequency" HeaderText="Normal Using Frequency" UniqueName="NormalUsingFrequency">
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="Remarks" HeaderText="Remarks" UniqueName="Remarks">
                                            <HeaderStyle HorizontalAlign="Center" Width="200" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                                    <Scrolling AllowScroll="True" SaveScrollPosition="True" UseStaticHeaders="True" />
                                </ClientSettings>
                            </telerik:RadGrid>

                            

                             <div style="width: 100%">
                                <ul style="list-style-type: none">
                                    <li style="width: 550px; padding-top: 10px; padding-bottom: 3px; text-align: center">
                                        
                                        <%--<telerik:RadButton ID="btncancel" runat="server" Text="Cancel" Width="70px" style="text-align: center"
                                            OnClick="btncancel_Click">
                                            <Icon PrimaryIconUrl="../../Images/Cancel.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                        </telerik:RadButton>--%>

                                    </li>
                                </ul>
                            </div>
                        </telerik:RadPageView>
                        <telerik:RadPageView runat="server" Height="100%" ID="RadPageView2">
                            
                            
                            <telerik:RadGrid ID="grdCheckList" runat="server" AllowPaging="False"
                                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                GridLines="None" Skin="Windows7" Height="100%"
                                OnNeedDataSource="grdCheckList_OnNeedDataSource" 
                                OnBatchEditCommand="grdCheckList_OnBatchEditCommand"
                                PageSize="10" Style="outline: none; overflow: hidden !important;">
                                <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%" CommandItemDisplay="TopAndBottom" EditMode="Batch">
                                    <GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldAlias="."  FieldName="ParentDescription" FormatString="{0:D}"
                                                        ></telerik:GridGroupByField>
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldName="ParentDescription" SortOrder="Ascending" ></telerik:GridGroupByField>
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>
                                        </GroupByExpressions> 
                                    <CommandItemSettings ShowAddNewRecordButton="False"></CommandItemSettings>
                                    <BatchEditingSettings OpenEditingEvent="Click" EditType="Row"></BatchEditingSettings>
                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Contents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                    
                                        <telerik:GridBoundColumn DataField="Description" HeaderText="Description" SortExpression="Description"
                                            UniqueName="Description" ReadOnly="True">
                                            <HeaderStyle Width="400px" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Font-Bold="True"/>
                                        </telerik:GridBoundColumn>
                    
                                        <telerik:GridCheckBoxColumn DataField="IsYes" HeaderStyle-Width="80px" HeaderText="Yes" SortExpression="IsYes"
                                            UniqueName="IsYes">
                                            <ItemStyle HorizontalAlign="Center"/>
                                        </telerik:GridCheckBoxColumn>

                                        <telerik:GridCheckBoxColumn DataField="IsNo" HeaderStyle-Width="80px" HeaderText="No" SortExpression="IsNo"
                                            UniqueName="IsNo">
                                            <ItemStyle HorizontalAlign="Center"/>
                                        </telerik:GridCheckBoxColumn>
                                        
                                        <telerik:GridCheckBoxColumn DataField="IsNA" HeaderStyle-Width="80px" HeaderText="N/A" SortExpression="IsNA"
                                            UniqueName="IsNA">
                                            <ItemStyle HorizontalAlign="Center"/>
                                        </telerik:GridCheckBoxColumn>
                                        
                                        <telerik:GridBoundColumn DataField="Remark" HeaderText="Remark" SortExpression="Remark"
                                            UniqueName="Remark">
                                            <HeaderStyle Width="250px" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left"/>
                                            
                                        </telerik:GridBoundColumn>
                                        
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True" >
                                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                                    <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="200" UseStaticHeaders="True" />
                                    <ClientEvents />
                                </ClientSettings>
                            </telerik:RadGrid>
                            


                            
                        </telerik:RadPageView>
                            
                            <%-- Attach File --%>
                        <telerik:RadPageView runat="server" Height="100%" ID="RadPageView3">
                            <div style="width: 100%" runat="server" id="divMRAttachFile">
                                <ul style="list-style-type: none">
                                    <li style="width: 800px;">
                                        <div>
                                            <label style="width: 110px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">Description
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                <asp:TextBox runat="server" ID="txtAttachDescription" style="width: 500px; max-width: 500px" CssClass="min25Percent" TextMode="MultiLine" Rows="2"/>
                                            </div>
                                        </div>
                                        <div style="clear: both; font-size: 0;"></div>
                                    </li>

                                    <li style="width: 600px;" Runat="server" ID="UploadControl">
                                        <div>
                                            <label style="width: 110px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">Select attach file
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                <telerik:RadAsyncUpload runat="server" ID="docuploader"
                                                    MultipleFileSelection="Automatic" TemporaryFileExpiration="05:00:00" 
                                                    EnableInlineProgress="true" Width="350px"
                                                    Localization-Cancel="Cancel" CssClass="min25Percent qlcbFormRequired"
                                                    Localization-Remove="Remove" Localization-Select="Select"  Skin="Windows7">
                                                </telerik:RadAsyncUpload>
                            
                                                <%--<telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload1"
                                                    MultipleFileSelection="Automatic" TemporaryFileExpiration="05:00:00" 
                                                    EnableInlineProgress="true" Width="250px"
                                                    Localization-Cancel="Cancel" CssClass="min25Percent qlcbFormRequired"
                                                    Localization-Remove="Remove" Localization-Select="Select"  Skin="Windows7" DropZones=".DropZone1"
                                                    FileFilters="*.doc,*.docx,*.xls,*.xlsx,*.pdf">
                                                </telerik:RadAsyncUpload>--%>
                                            </div>
                                        </div>
                                        <div style="clear: both; font-size: 0;"></div>
                                    </li>
                                    

                                    <li style="width: 800px; text-align: center">
                                        <telerik:RadButton ID="btnSaveAttachFile" runat="server" Text="Save" OnClick="btnSaveAttachFile_Click" Width="70px" style="text-align: center">
                                            <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                        </telerik:RadButton>
                                        
                                    </li>
                                </ul>
                            </div>
                            <telerik:RadGrid ID="grdAttachFile" runat="server" AllowPaging="False"
                                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                GridLines="None" Skin="Windows7" Height="250"
                                OnDeleteCommand="grdAttachFile_DeleteCommand"
                                OnNeedDataSource="grdAttachFile_OnNeedDataSource" 
                                PageSize="10" Style="outline: none; overflow: hidden !important;">
                                <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%">
                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                                        <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" ConfirmText="Do you want to delete Attach file?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                                            <HeaderStyle Width="30" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%"  />
                                        </telerik:GridButtonColumn>

                                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                                            <HeaderStyle Width="35" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%"/>
                                            <ItemTemplate>
                                                <a href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' 
                                                    download='<%# DataBinder.Eval(Container.DataItem, "FileName") %>' target="_blank">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ExtensionIcon") %>'
                                                        Style="cursor: pointer;" ToolTip="Download document" /> 
                                                </a>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                
                                        <%--<telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%"/>
                                            <ItemTemplate>
                                                <a href='<%# string.Format("../../DownloadFileHandler.ashx?img={0}",DataBinder.Eval(Container.DataItem, "ID")) %>' 
                                                    download='<%# DataBinder.Eval(Container.DataItem, "FileName") %>' target="_blank">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ExtensionIcon") %>'
                                                        Style="cursor: pointer;" ToolTip="Download document" /> 
                                                </a>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>--%>
                                            
                                        <telerik:GridBoundColumn DataField="FileName" HeaderText="File name" UniqueName="FileName">
                                            <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                            <ItemStyle HorizontalAlign="Left" Width="30%" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="Description" HeaderText="Description" UniqueName="Description">
                                            <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                            <ItemStyle HorizontalAlign="Left" Width="30%" />
                                        </telerik:GridBoundColumn>
                                
                                        <telerik:GridBoundColumn DataField="CreatedByName" HeaderText="Upload by" UniqueName="CreatedByName">
                                            <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </telerik:GridBoundColumn>
                                
                                        <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Upload time" UniqueName="CreatedDate"
                                            DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                            <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                                        </telerik:GridBoundColumn>
                                
                                        <telerik:GridBoundColumn DataField="FileSize" HeaderText="File size(Kb)" UniqueName="FileSize" DataFormatString="{0:0,0.00}">
                                            <HeaderStyle HorizontalAlign="Center" Width="13%" />
                                            <ItemStyle HorizontalAlign="Center" Width="13%" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                                    <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="200" UseStaticHeaders="True" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </telerik:RadPageView>
                            
                            
                            <%-- Comment --%>
                        <telerik:RadPageView runat="server" Height="100%" ID="RadPageView4">
                            <div style="width: 100%" runat="server" id="divMRComment">
                                <ul style="list-style-type: none">
                                    <li style="width: 800px;" Runat="server" ID="Li2">
                                        <div>
                                            <label style="width: 110px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">Comment From
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div3">
                                                    <asp:DropDownList ID="ddlCommentFrom" runat="server" CssClass="min25Percent qlcbFormRequired" Width="516px" style="max-width: 5006px">
                                                        <Items>
                                                            <asp:ListItem Value="0" Text="Offshore"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Operation Department"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Technical/ Relevant Department"></asp:ListItem>
                                                            <asp:ListItem Value="3" Text="Board of Director"></asp:ListItem>
                                                        </Items>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="clear: both; font-size: 0;"></div>
                                    </li>

                                    <li style="width: 800px; display: none">
                                        <div>
                                            <label style="width: 110px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">Comment By
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                <asp:TextBox runat="server" ID="txtCommentBy" style="width: 500px; max-width: 500px" CssClass="min25Percent qlcbFormRequired"/>
                                            </div>
                                        </div>
                                        <div style="clear: both; font-size: 0;"></div>
                                    </li>
                                    
                                    <li style="width: 800px;">
                                        <div>
                                            <label style="width: 110px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">Comment
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                <asp:TextBox runat="server" ID="txtComment" style="width: 500px; max-width: 500px" CssClass="min25Percent qlcbFormRequired" TextMode="MultiLine" Rows="3"/>
                                            </div>
                                        </div>
                                        <div style="clear: both; font-size: 0;"></div>
                                    </li>

                                    <li style="width: 800px; text-align: center; padding-top: 10px; padding-bottom: 5px">
                                        <telerik:RadButton ID="btnSaveComment" runat="server" Text="Save" OnClick="btnSaveComment_Click" Width="70px" style="text-align: center">
                                            <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                        </telerik:RadButton>
                                        
                                        <telerik:RadButton ID="btnClearComment" runat="server" Text="Clear" OnClick="btnMaterialDetailClear_Click" Width="70px" style="text-align: center">
                                            <Icon PrimaryIconUrl="../../Images/clear.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                        </telerik:RadButton>
                                    </li>
                                </ul>
                            </div>
                            <telerik:RadGrid ID="grdComment" runat="server" AllowPaging="False"
                                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                GridLines="None" Skin="Windows7" Height="250"
                                OnDeleteCommand="grdComment_DeleteCommand"
                                OnNeedDataSource="grdComment_OnNeedDataSource" 
                                OnItemCommand="grdComment_OnItemCommand"
                                OnItemDataBound="grdComment_OnItemDataBound"
                                
                                PageSize="10" Style="outline: none; overflow: hidden !important;">
                                <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%">
                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                                        <telerik:GridBoundColumn DataField="CommentBy" UniqueName="CommentBy" Display="False" />
                                        
                                        <telerik:GridButtonColumn UniqueName="EditColumn" CommandName="EditCmd" ButtonType="ImageButton" ImageUrl="~/Images/edit.png">
                                            <HeaderStyle Width="30" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                        </telerik:GridButtonColumn>

                                        <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" ConfirmText="Do you want to delete Attach file?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                                            <HeaderStyle Width="30" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%"  />
                                        </telerik:GridButtonColumn>

                                        
                                
                                        <%--<telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%"/>
                                            <ItemTemplate>
                                                <a href='<%# string.Format("../../DownloadFileHandler.ashx?img={0}",DataBinder.Eval(Container.DataItem, "ID")) %>' 
                                                    download='<%# DataBinder.Eval(Container.DataItem, "FileName") %>' target="_blank">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ExtensionIcon") %>'
                                                        Style="cursor: pointer;" ToolTip="Download document" /> 
                                                </a>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>--%>
                                            
                                        <telerik:GridBoundColumn DataField="CommentTypeName" HeaderText="Comment From" UniqueName="CommentTypeName">
                                            <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left"/>
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="CommentByName" HeaderText="Comment By" UniqueName="CommentByName">
                                            <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                
                                        <telerik:GridBoundColumn DataField="Comment" HeaderText="Comment" UniqueName="Comment">
                                            <HeaderStyle HorizontalAlign="Center" Width="50%" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                
                                        <telerik:GridBoundColumn DataField="CommentDate" HeaderText="Comment time" UniqueName="CommentDate"
                                            DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                                    <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="200" UseStaticHeaders="True" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </telerik:RadPageView>
                            
                                <%-- Comment --%>
                        <telerik:RadPageView runat="server" Height="100%" ID="RadPageView5">
                            <div style="width: 100%">
                                <ul style="list-style-type: none">
                                     <li style="width: 800px;">
                                        <div>
                                            <label style="width: 170px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">Purchasing Group
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                <asp:TextBox runat="server" ID="txtPurchasingGroup" style="width: 300px; max-width: 500px" CssClass="min25Percent"/>
                                            </div>
                                        </div>
                                        <div style="clear: both; font-size: 0;"></div>
                                    </li>
                                    
                                    
                                    <li style="width: 800px;" Runat="server" ID="Li1">
                                        <div>
                                            <label style="width: 170px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">Received MR from Facility
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div4">
                                                    <telerik:RadDatePicker ID="txtReceivedMRFromFacility"  runat="server" 
                                                        ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                                        <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                                    </telerik:RadDatePicker>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="clear: both; font-size: 0;"></div>
                                    </li>
                                    
                                    <li style="width: 800px;" Runat="server" ID="Li3">
                                        <div>
                                            <label style="width: 170px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">MR Processing Completed
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div5">
                                                    <telerik:RadDatePicker ID="txtMrProcessComplete"  runat="server" 
                                                        ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                                        <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                                    </telerik:RadDatePicker>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="clear: both; font-size: 0;"></div>
                                    </li>
                                    
                                    
                                    <li style="width: 800px;" Runat="server" ID="Li5">
                                        <div>
                                            <label style="width: 170px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">Forward MR to Technical/ Relevant Department 
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div7">
                                                    <telerik:RadDatePicker ID="txtForwardMRToTech"  runat="server" 
                                                        ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                                        <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                                    </telerik:RadDatePicker>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="clear: both; font-size: 0;"></div>
                                    </li>
                                    
                                    <li style="width: 800px;" Runat="server" ID="Li6">
                                        <div>
                                            <label style="width: 170px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">Received MR from Technical/ Relevant Department 
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div8">
                                                    <telerik:RadDatePicker ID="txtReceiveMRFromTech"  runat="server" 
                                                        ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                                        <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                                    </telerik:RadDatePicker>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="clear: both; font-size: 0;"></div>
                                    </li>
                                    
                                    <li style="width: 800px;" Runat="server" ID="Li4">
                                        <div>
                                            <label style="width: 170px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">Forward MR to Purchasing Group
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div6">
                                                    <telerik:RadDatePicker ID="txtForwardMRToPurchasing"  runat="server" 
                                                        ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                                        <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                                    </telerik:RadDatePicker>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="clear: both; font-size: 0;"></div>
                                    </li>
                                   

                                    <li style="width: 800px; text-align: center; padding-top: 10px; padding-bottom: 5px">
                                        <telerik:RadButton ID="btnSaveReceivedInfo" runat="server" Text="Save" OnClick="btnSaveReceivedInfo_Click" Width="70px" style="text-align: center">
                                            <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                        </telerik:RadButton>
                                    </li>
                                </ul>
                            </div>
                        </telerik:RadPageView>
                            
                        <telerik:RadPageView runat="server" Height="100%" ID="RadPageView6">
                            <fieldset>
                                <legend>MR Detail Tracking Info:</legend>
                                <table>
                                    <tr>
                                        <td>Date Rcv'd MSR</td>
                                        <td>
                                            <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div10">
                                                    <telerik:RadDatePicker ID="txtMRTrackingRecieveDate"  runat="server" 
                                                        ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                                        <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                                    </telerik:RadDatePicker>
                                                </div>
                                            </div>
                                        </td>
                                        <td style="width: 200px">Contract No.</td>
                                        <td>
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                <asp:TextBox runat="server" ID="txtContractNo"  Style="width: 200px; min-width: 50px" CssClass="min25Percent"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td style="width: 150px">Unit Price</td>
                                        <td>
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                <asp:TextBox runat="server" ID="TextBox2"  Style="width: 200px; min-width: 50px" CssClass="min25Percent"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>PIC</td>
                                        <td>
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                <telerik:RadTreeView RenderMode="Lightweight" ID="rtvPIC" runat="server"  CheckBoxes="True" TriStateCheckBoxes="true" CheckChildNodes="true" Width="250" Height="170"/>
                                            </div>
                                        </td>
                                        <td colspan="2" valign="top">
                                            <table>
                                                <tr>
                                                    <td style="width: 200px">Request For Quotation Date</td>
                                                    <td>
                                                        <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div11">
                                                                <telerik:RadDatePicker ID="RadDatePicker1"  runat="server" 
                                                                    ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                                                    <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                                                </telerik:RadDatePicker>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Delivery Date revised by Supplier</td>
                                                    <td>
                                                        <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div15">
                                                                <telerik:RadDatePicker ID="RadDatePicker5"  runat="server" 
                                                                    ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                                                    <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                                                </telerik:RadDatePicker>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td >Rcv'd Quotation Date</td>
                                                    <td>
                                                        <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div12">
                                                                <telerik:RadDatePicker ID="RadDatePicker2"  runat="server" 
                                                                    ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                                                    <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                                                </telerik:RadDatePicker>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>PO Number</td>
                                                    <td>
                                                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                            <asp:TextBox runat="server" ID="TextBox1"  Style="width: 200px; min-width: 50px" CssClass="min25Percent"></asp:TextBox>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>PO Issue Date</td>
                                                    <td>
                                                        <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div13">
                                                                <telerik:RadDatePicker ID="RadDatePicker3"  runat="server" 
                                                                    ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                                                    <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                                                </telerik:RadDatePicker>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                
                                                
                                            </table>
                                        </td>
                                        <td colspan="2" valign="top">
                                            <table>
                                                <tr>
                                                    <td style="width: 150px">Total Price</td>
                                                    <td>
                                                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                            <asp:TextBox runat="server" ID="TextBox3"  Style="width: 200px; min-width: 50px" CssClass="min25Percent"></asp:TextBox>
                                                        </div>
                                                    </td>
                                                </tr>
                                                
                                                <tr>
                                                    <td >Expected Delivery Date</td>
                                                    <td>
                                                        <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div14">
                                                                <telerik:RadDatePicker ID="RadDatePicker4"  runat="server" 
                                                                    ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                                                    <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                                                </telerik:RadDatePicker>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td >Actual delivery date</td>
                                                    <td>
                                                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div16">
                                                            <telerik:RadDatePicker ID="RadDatePicker6"  runat="server" 
                                                                ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                                                            </telerik:RadDatePicker>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Supplier</td>
                                                    <td>
                                                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                            <asp:TextBox runat="server" ID="TextBox4"  Style="width: 200px; min-width: 50px" CssClass="min25Percent"></asp:TextBox>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>MSR Purchasing Status</td>
                                                    <td>
                                                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="min25Percent" Width="134" style="max-width: 5006px">
                                                                <Items>
                                                                    <asp:ListItem Value="Cancel" Text="Cancel"></asp:ListItem>
                                                                    <asp:ListItem Value="Complete" Text="Complete"></asp:ListItem>
                                                                    <asp:ListItem Value="Holding" Text="Holding"></asp:ListItem>
                                                                    <asp:ListItem Value="Order" Text="Order"></asp:ListItem>
                                                                    <asp:ListItem Value="Outstanding" Text="Outstanding"></asp:ListItem>
                                                                    <asp:ListItem Value="Request For Quotation" Text="Request For Quotation"></asp:ListItem>
                                                                </Items>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                                
                                                <tr>
                                                    <td>MSR Item Purchasing Status</td>
                                                    <td>
                                                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                            <asp:DropDownList ID="DropDownList2" runat="server" CssClass="min25Percent" Width="134" style="max-width: 5006px">
                                                                <Items>
                                                                    <asp:ListItem Value="Cancel" Text="Cancel"></asp:ListItem>
                                                                    <asp:ListItem Value="Complete" Text="Complete"></asp:ListItem>
                                                                    <asp:ListItem Value="Holding" Text="Holding"></asp:ListItem>
                                                                    <asp:ListItem Value="Order" Text="Order"></asp:ListItem>
                                                                    <asp:ListItem Value="Outstanding" Text="Outstanding"></asp:ListItem>
                                                                    <asp:ListItem Value="Request For Quotation" Text="Request For Quotation"></asp:ListItem>
                                                                </Items>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        
                                    </tr>

                                    <tr>
                                        <td colspan="6" align="center">
                                            <telerik:RadButton ID="RadButton1" runat="server" Text="Save" OnClick="btnSaveComment_Click" Width="70px" style="text-align: center">
                                                <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                            </telerik:RadButton>
                                        
                                            <telerik:RadButton ID="RadButton2" runat="server" Text="Clear" OnClick="btnMaterialDetailClear_Click" Width="70px" style="text-align: center">
                                                <Icon PrimaryIconUrl="../../Images/clear.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            
                            <telerik:RadGrid ID="grdMRDetailTracking" runat="server" AllowPaging="False"
                                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                                GridLines="None" Skin="Windows7" Height="300" AllowFilteringByColumn="False"
                                OnItemCommand="grdMRDetailTracking_OnItemCommand"
                                OnItemDataBound="grdMRDetailTracking_ItemDataBound" 
                                OnNeedDataSource="grdMRDetailTracking_OnNeedDataSource" 
                                OnItemCreated="grdMRDetailTracking_OnItemCreated"
                                PageSize="100" Style="outline: none; overflow: hidden !important;" Width="100%">
                                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                                <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" >
                                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Contents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ColumnGroups>
                                        <telerik:GridColumnGroup HeaderText="Req ROB (6)" Name="ReqROB" HeaderStyle-HorizontalAlign="Center"/>
                                        <telerik:GridColumnGroup HeaderText="Quantity Remark (9)" Name="QuantityRemark" HeaderStyle-HorizontalAlign="Center"/>
                                    </ColumnGroups>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                                        <telerik:GridBoundColumn DataField="IsCancel" UniqueName="IsCancel" Display="False" />

                                        <telerik:GridButtonColumn UniqueName="EditColumn" CommandName="EditCmd" ButtonType="ImageButton" ImageUrl="~/Images/edit.png">
                                            <HeaderStyle Width="30" />
                                            <ItemStyle HorizontalAlign="Center"  />
                                        </telerik:GridButtonColumn>

                                    <%-- **********************************Comment********************************* --%>
                                        
                                        <telerik:GridTemplateColumn HeaderText="No." Groupable="False" AllowFiltering="false" Display="False">
                                            <HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSoTT" runat="server" Text='<%# grdMaterials.CurrentPageIndex * grdMaterials.PageSize + grdMaterials.Items.Count+1 %>'>
                                                </asp:Label>
                                      
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridBoundColumn DataField="SFICode" HeaderText="SFI Code" UniqueName="SFICode">
                                            <HeaderStyle HorizontalAlign="Center" Width="160" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="Description" HeaderText="Material Name & Specification Descriptions " UniqueName="Description">
                                            <HeaderStyle HorizontalAlign="Center" Width="300" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="MakerName" HeaderText="Maker Name/ Maker Refer" UniqueName="MakerName">
                                            <HeaderStyle HorizontalAlign="Center" Width="100" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="Remarks" HeaderText="Remarks" UniqueName="Remarks">
                                            <HeaderStyle HorizontalAlign="Center" Width="200" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="CertificateRequired" HeaderText="Certificate Required (Y/N)" UniqueName="CertificateRequired">
                                            <HeaderStyle HorizontalAlign="Center" Width="100" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="Units" HeaderText="Units" UniqueName="Units">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="QtyReq" HeaderText="Quantity Required" UniqueName="QtyReq">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="DateReq" HeaderText="Date Req" UniqueName="DateReq"
                                            DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center" Width="80" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="PriorityName" HeaderText="Priority Lvl" UniqueName="PriorityName">
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="MRRecieveDate" HeaderText="Date Rcv'd MSR" UniqueName="MRRecieveDate"
                                            DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center" Width="80" />
                                        </telerik:GridBoundColumn>

                                        
                                        <telerik:GridBoundColumn DataField="DepartmentName" HeaderText="Department" UniqueName="DepartmentName">
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="PICName" HeaderText="PIC" UniqueName="PICName">
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="ContractNumber" HeaderText="Contract No." UniqueName="ContractNumber">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="ReqQuotationDate" HeaderText="Request For Quotation Date" UniqueName="ReqQuotationDate"
                                            DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center" Width="80" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="ReceiveQuotationDate" HeaderText="Rcv'd Quotation Date" UniqueName="ReceiveQuotationDate"
                                            DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center" Width="80" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="PONumber" HeaderText="PO Number" UniqueName="PONumber">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="POIssueDate" HeaderText="PO Issue Date" UniqueName="POIssueDate"
                                            DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center" Width="80" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="UnitPrice" HeaderText="Unit Price" UniqueName="UnitPrice">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="TotalPrice" HeaderText="Total Price" UniqueName="TotalPrice">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="ExpectedDeliveryDate" HeaderText="Expected Delivery Date" UniqueName="ExpectedDeliveryDate"
                                            DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center" Width="80" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="DeliveryDateRevisedBySupplier" HeaderText="Delivery Date Revised By Supplier" UniqueName="DeliveryDateRevisedBySupplier"
                                            DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center" Width="80" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="ActualDeliveryDate" HeaderText="Actual Delivery Date" UniqueName="ActualDeliveryDate"
                                            DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                                            <ItemStyle HorizontalAlign="Center" Width="80" />
                                        </telerik:GridBoundColumn>
                                        

                                        <telerik:GridBoundColumn DataField="SupplierName" HeaderText="Supplier" UniqueName="SupplierName">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="MRPurchasingStatus" HeaderText="MSR Purchasing Status" UniqueName="MRPurchasingStatus">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridBoundColumn DataField="MRDetailPurchasingStatus" HeaderText="MSR Item Purchasing Status" UniqueName="MRDetailPurchasingStatus">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                    
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                                    <Scrolling AllowScroll="True" SaveScrollPosition="True" UseStaticHeaders="True" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </telerik:RadPageView>
                    </telerik:RadMultiPage>

                    </td>
                </tr>
            </table>

</div>
            
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        <asp:HiddenField runat="server" ID="IsView"/>
        <asp:HiddenField runat="server" ID="IsCreate"/>
        <asp:HiddenField runat="server" ID="IsUpdate"/>
        <asp:HiddenField runat="server" ID="IsCancel"/>
        <asp:HiddenField runat="server" ID="IsAttachWF"/>
        
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" Skin="Windows7">
        <Windows>
            <telerik:RadWindow ID="CompleteMoveNext" runat="server" Title="Complete Task And Move Next Step"
                VisibleStatusbar="False" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="CompleteFinal" runat="server" Title="Complete MR Process Workflow"
                VisibleStatusbar="False" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RejectForm" runat="server" Title="Reject To Previous Step"
                VisibleStatusbar="False" Height="450" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RevisionHistory" runat="server" Title="MR Detail Update History"
                VisibleStatusbar="false" Height="500" Width="900" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Windows7"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument" >
            <%--<ClientEvents OnRequestStart="RequestStart" OnResponseEnd="ResponseEnd" />--%>
            <ClientEvents OnRequestStart="RequestStart" OnResponseEnd="ResponseEnd" />
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="grdMaterials">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtSFICode"/>
                        <telerik:AjaxUpdatedControl ControlID="txtROBMin"/>
                        <telerik:AjaxUpdatedControl ControlID="txtROBMax"/>
                        <telerik:AjaxUpdatedControl ControlID="txtROB"/>
                        <telerik:AjaxUpdatedControl ControlID="txtUnit"/>
                        <telerik:AjaxUpdatedControl ControlID="txtMaterialName"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlCertReq"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlAlter"/>
                        <telerik:AjaxUpdatedControl ControlID="txtFrequency"/>
                        <telerik:AjaxUpdatedControl ControlID="txtRemark"/>
                        <telerik:AjaxUpdatedControl ControlID="txtMakerRef"/>
                        <telerik:AjaxUpdatedControl ControlID="txtQuantityReq"/>
                        <telerik:AjaxUpdatedControl ControlID="txtUseForJob"/>
                        <telerik:AjaxUpdatedControl ControlID="txtForSpare"/>
                        <telerik:AjaxUpdatedControl ControlID="grdMaterials"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnMaterialDetailSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtSFICode"/>
                        <telerik:AjaxUpdatedControl ControlID="txtROBMin"/>
                        <telerik:AjaxUpdatedControl ControlID="txtROBMax"/>
                        <telerik:AjaxUpdatedControl ControlID="txtROB"/>
                        <telerik:AjaxUpdatedControl ControlID="txtUnit"/>
                        <telerik:AjaxUpdatedControl ControlID="txtMaterialName"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlCertReq"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlAlter"/>
                        <telerik:AjaxUpdatedControl ControlID="txtFrequency"/>
                        <telerik:AjaxUpdatedControl ControlID="txtRemark"/>
                        <telerik:AjaxUpdatedControl ControlID="grdMaterials"/>
                        <telerik:AjaxUpdatedControl ControlID="txtMakerRef"/>
                        <telerik:AjaxUpdatedControl ControlID="txtQuantityReq"/>
                        <telerik:AjaxUpdatedControl ControlID="txtUseForJob"/>
                        <telerik:AjaxUpdatedControl ControlID="txtForSpare"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnMaterialDetailClear">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtSFICode"/>
                        <telerik:AjaxUpdatedControl ControlID="txtROBMin"/>
                        <telerik:AjaxUpdatedControl ControlID="txtROBMax"/>
                        <telerik:AjaxUpdatedControl ControlID="txtROB"/>
                        <telerik:AjaxUpdatedControl ControlID="txtUnit"/>
                        <telerik:AjaxUpdatedControl ControlID="txtMaterialName"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlCertReq"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlAlter"/>
                        <telerik:AjaxUpdatedControl ControlID="txtFrequency"/>
                        <telerik:AjaxUpdatedControl ControlID="txtRemark"/>
                        <telerik:AjaxUpdatedControl ControlID="grdMaterials"/>
                        <telerik:AjaxUpdatedControl ControlID="txtMakerRef"/>
                        <telerik:AjaxUpdatedControl ControlID="txtQuantityReq"/>
                        <telerik:AjaxUpdatedControl ControlID="txtUseForJob"/>
                        <telerik:AjaxUpdatedControl ControlID="txtForSpare"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlRefMR"/>
                        <telerik:AjaxUpdatedControl ControlID="txtSFICode"/>
                        <telerik:AjaxUpdatedControl ControlID="txtROBMin" />
                        <telerik:AjaxUpdatedControl ControlID="txtROBMax"/>
                        <telerik:AjaxUpdatedControl ControlID="txtROB"/>
                        <telerik:AjaxUpdatedControl ControlID="txtUnit"/>
                        <telerik:AjaxUpdatedControl ControlID="txtMaterialName"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlCertReq"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlAlter"/>
                        <telerik:AjaxUpdatedControl ControlID="txtFrequency"/>
                        <telerik:AjaxUpdatedControl ControlID="txtRemark"/>
                        <telerik:AjaxUpdatedControl ControlID="imgLoading"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                    
                <telerik:AjaxSetting AjaxControlID="rtlMRCheckListDefine">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtCheckListRemark"/>
                        <telerik:AjaxUpdatedControl ControlID="txtCheckListDescription"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnYes"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnNo"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnNA"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnCheckListSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtCheckListRemark"/>
                        <telerik:AjaxUpdatedControl ControlID="txtCheckListDescription"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnYes"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnNo"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnNA"/>
                        <telerik:AjaxUpdatedControl ControlID="rtlMRCheckListDefine"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnCheckListClear">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtCheckListRemark"/>
                        <telerik:AjaxUpdatedControl ControlID="txtCheckListDescription"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnYes"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnNo"/>
                        <telerik:AjaxUpdatedControl ControlID="rbtnNA"/>
                        <telerik:AjaxUpdatedControl ControlID="rtlMRCheckListDefine"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSaveAttachFile">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="docuploader"/>
                        <telerik:AjaxUpdatedControl ControlID="txtAttachDescription"/>
                        <telerik:AjaxUpdatedControl ControlID="grdAttachFile"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdAttachFile">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAttachFile"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="grdCheckList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdCheckList"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdComment">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdComment"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlCommentFrom"/>
                        <telerik:AjaxUpdatedControl ControlID="txtCommentBy"/>
                        <telerik:AjaxUpdatedControl ControlID="txtComment"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSaveComment">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdComment"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlCommentFrom"/>
                        <telerik:AjaxUpdatedControl ControlID="txtCommentBy"/>
                        <telerik:AjaxUpdatedControl ControlID="txtComment"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnClearComment">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdComment"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlCommentFrom"/>
                        <telerik:AjaxUpdatedControl ControlID="txtCommentBy"/>
                        <telerik:AjaxUpdatedControl ControlID="txtComment"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadCodeBlock runat="server">
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

                function ShowHistory(id) {
                var owd = $find("<%=RevisionHistory.ClientID %>");
                owd.Show();
                //owd.maximize();
                owd.setUrl("MRDetailHistory.aspx?objId=" + id, "RevisionHistory");
            }

                function fileUploading(sender, args) {
                    var name = args.get_fileName();
                    document.getElementById("txtName").value = name;

                    ajaxManager.ajaxRequest("CheckFileName$" + name);
                }

                function OnClientItemChecking(Sender, args) {
                    args.set_cancel(true);
                }

                function btnCompleteClicked(sender, args)
                {
                    var objType = document.getElementById("<%= ObjectType.ClientID %>").value;
                    var objId = document.getElementById("<%= ObjectId.ClientID %>").value;
                    var objAssignUserId = document.getElementById("<%= ObjAssignUserId.ClientID %>").value;
                    var owd = $find("<%=CompleteMoveNext.ClientID %>");
                    owd.Show();
                    owd.setUrl("../../Controls/Workflow/CompleteMoveNext.aspx?objType=" + objType + "&objId=" + objId + "&currentAssignId=" + objAssignUserId + "&flag=true", "CompleteMoveNext");
                }

                function btnCompleteFinalClicked(sender, args)
                {
                    var objType = document.getElementById("<%= ObjectType.ClientID %>").value;
                    var objId = document.getElementById("<%= ObjectId.ClientID %>").value;
                    var objAssignUserId = document.getElementById("<%= ObjAssignUserId.ClientID %>").value;
                    var owd = $find("<%=CompleteFinal.ClientID %>");
                    owd.Show();
                    owd.setUrl("../../Controls/Workflow/FinalComplete.aspx?objType=" + objType + "&objId=" + objId + "&currentAssignId=" + objAssignUserId + "&flag=true", "CompleteFinal");
                }

                function btnRejectClicked(sender, args) {
                    if (confirm("Do you want to Reject this Object?") == false) return;

                    var objType = document.getElementById("<%= ObjectType.ClientID %>").value;
                    var objId = document.getElementById("<%= ObjectId.ClientID %>").value;
                    var objAssignUserId = document.getElementById("<%= ObjAssignUserId.ClientID %>").value;

                    var owd = $find("<%=RejectForm.ClientID %>");
                    owd.Show();
                    owd.setUrl("../../Controls/Workflow/Reject.aspx?objType=" + objType + "&objId=" + objId + "&currentAssignId=" + objAssignUserId + "&flag=true", "RejectForm");
                }

                var currentLoadingPanel = null;
                var currentUpdatedControl = null;
                function RequestStart(sender, args) {
                     currentLoadingPanel = $find("<%= RadAjaxLoadingPanel1.ClientID %>");
                        if (args.get_eventTarget() == "<%= btnSearch.UniqueID %>") {
                            currentUpdatedControl = "<%= Panel1.ClientID %>";
                        }

                     //show the loading panel over the updated control
                        currentLoadingPanel.show(currentUpdatedControl);
                }
                    function ResponseEnd() {
                        //hide the loading panel and clean up the global variables
                        if (currentLoadingPanel != null)
                        currentLoadingPanel.hide(currentUpdatedControl);
                        currentUpdatedControl = null;
                        currentLoadingPanel = null;
                            }
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
