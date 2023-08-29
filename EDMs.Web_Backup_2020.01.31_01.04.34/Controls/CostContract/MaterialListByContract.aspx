<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaterialListByContract.aspx.cs" Inherits="EDMs.Web.Controls.CostContract.MaterialListByContract" %>

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
        <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="True"
            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
            GridLines="None" Skin="Windows7" Height="420"
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
                    <telerik:GridTemplateColumn HeaderText="Contract Number" UniqueName="ContractNumber"
                        DataField="ContractNumber" ShowFilterIcon="False" FilterControlWidth="97%" 
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                        <ItemStyle HorizontalAlign="Left"/>
                        <ItemTemplate>
                            <asp:Label ID="lblDocNo" runat="server" Text='<%# Eval("ContractNumber") %>' style="cursor: pointer; color: green"/> 
                            <telerik:RadToolTip Skin="Simple" runat="server" ID="dirNameToolTip" RelativeTo="Element" Title="Contract Information"
                            AutoCloseDelay="10000" ShowDelay="0" Position="BottomRight"
                            Width="400px" Height="150px" HideEvent="LeaveTargetAndToolTip"  TargetControlID="lblDocNo" IsClientID="False"
                            Animation="Fade" Text='<%# "<b>Number:</b> " + Eval("ContractObj.Number") + "<br/>" +
                                                        "<b>Content:</b> " + (!string.IsNullOrEmpty(Eval("ContractObj.ContractContent").ToString()) 
                                                                        ? Eval("ContractObj.ContractContent").ToString().Replace("\n", "<br/>")
                                                                        : string.Empty) + "<br/>"
                                                    + "<b>Contractor:</b> " + Eval("ContractObj.ContractorSelectedName") + "<br/>"
                                                    + "<b>Delivery Date:</b> " + Eval("ContractObj.DeliveryDate","{0:dd/MM/yyyy}") + "<br/>"
                                                    + "<b>Delivery Status:</b> " + Eval("ContractObj.DeliveryStatus") + "<br/>"
                                                    %>'>
                            </telerik:RadToolTip>

                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                            
                    <telerik:GridTemplateColumn HeaderText="Material Number" UniqueName="Number"
                        DataField="Number" ShowFilterIcon="False" FilterControlWidth="97%" 
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                        <ItemStyle HorizontalAlign="Left"/>
                        <ItemTemplate>
                            <%# Eval("Number") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                    
                    <telerik:GridTemplateColumn HeaderText="Quality" UniqueName="Quality"
                        AllowFiltering="false">
                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%# Eval("Quality") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                        
                    <telerik:GridTemplateColumn HeaderText="Delivery Plan" UniqueName="DeliveryPlan"
                        AllowFiltering="false" >
                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                        <ItemStyle HorizontalAlign="Center" ForeColor="Blue"/>
                        <ItemTemplate>
                            <%# Eval("DeliveryPlan","{0:dd/MM/yyyy}") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                        
                        <telerik:GridTemplateColumn HeaderText="Actual" UniqueName="DeliveryActual"
                        AllowFiltering="false" >
                        <HeaderStyle HorizontalAlign="Center" Width="8%"  />
                        <ItemStyle HorizontalAlign="Center" ForeColor="Red"/>
                        <ItemTemplate>
                            <%# Eval("DeliveryActual","{0:dd/MM/yyyy}") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Delivery Status" UniqueName="DeliveryStatus"
                        AllowFiltering="false">
                        <HeaderStyle HorizontalAlign="Center" Width="20%"  />
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%# Eval("DeliveryStatus") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Note" UniqueName="Note"
                        AllowFiltering="false" >
                        <HeaderStyle HorizontalAlign="Center" Width="20%"  />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <%# Eval("Note") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                        
                        <telerik:GridTemplateColumn HeaderText="Complete - %" UniqueName="Complete" AllowFiltering="false">
                        <HeaderStyle HorizontalAlign="Center" Width="8%"  />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <%# Eval("Complete")!=null && Convert.ToDouble(Eval("Complete")) != 0 ?  Eval("Complete") + "%" : "-"%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Weight - %" UniqueName="Weight" AllowFiltering="false">
                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                        <ItemStyle HorizontalAlign="Center"/>
                        <ItemTemplate>
                            <%# Eval("Weight")!=null ?  Eval("Weight") + "%" : "-"%>
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
