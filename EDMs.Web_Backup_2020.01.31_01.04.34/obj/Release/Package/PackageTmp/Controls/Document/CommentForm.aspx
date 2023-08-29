
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommentForm.aspx.cs" Inherits="EDMs.Web.Controls.Document.CommentForm" %>

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
        <div style="width: 100%" runat="server" ID="EditContent">
            <ul style="list-style-type: none">
                <%-- **********************************Comment********************************* --%>
                <dl class="accordion">
                    <dt style="width: 100%;">
                        <span>COMMENT INFO:</span>
                    </dt>
                </dl>

                <%--<li style="width: 750px; display: none">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">PVCFC send date
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="divMilestone">
                            <telerik:RadDatePicker ID="txtVSPSendDate"  runat="server" 
                                 CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" CssClass="qlcbFormNonRequired"/>
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 750px; display: none">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">PVCFC send Trans No.
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtVSPSendTrans" runat="server" Style="width: 300px;" CssClass="min25Percent" />
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>--%>
                
                

                <li style="width: 750px;">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">
                                Plan Comment
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div1">
                            <telerik:RadDatePicker ID="txtPlanComment"  runat="server" 
                                 CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" CssClass="qlcbFormNonRequired"/>
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 750px;">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">
                                Actual Comment
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div2">
                            <telerik:RadDatePicker ID="txtActualComment"  runat="server" 
                                 CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" CssClass="qlcbFormNonRequired"/>
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 750px;">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Code
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:DropDownList ID="ddlCode" runat="server" CssClass="min25Percent" Width="316px"/>
                            
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 750px;" >
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Comment Sheet No.
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtCommentSheet" runat="server" Style="width: 300px;" CssClass="min25Percent" />
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <%--<li style="width: 750px;display: none">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Trans No.
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtTransNo" runat="server" Style="width: 300px;" CssClass="min25Percent" />
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>--%>
                
                <dl class="accordion">
                    <dt style="width: 100%;">
                        <span>SEND COMMENT TO DESIGN CONTRACTOR:</span>
                    </dt>
                </dl>

                <li style="width: 750px;" runat="server" id="divPlanSendCMS">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Plan Date
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div3">
                            <telerik:RadDatePicker ID="txtPlanSendCMSToDesign"  runat="server" 
                                 CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" CssClass="qlcbFormNonRequired"/>
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 750px;" runat="server" id="divActualSendCMS">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Actual Date
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div4">
                            <telerik:RadDatePicker ID="txtActualSendCMSToDesign"  runat="server" 
                                 CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" CssClass="qlcbFormNonRequired"/>
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 750px;" runat="server" id="divSendCMSTrans">
                    <div>
                        <label style="width: 125px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Trans No.
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtSendCMDToDesignTransNo" runat="server" Style="width: 300px;" CssClass="min25Percent" />
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 100%; text-align: center; padding-top: 3px">
                    <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="70px" style="text-align: center">
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
            GridLines="None" Skin="Windows7" Height="350"
            OnDeleteCommand="grdDocument_DeleteCommand"
            OnItemCommand="grdDocument_OnItemCommand"
            OnNeedDataSource="grdDocument_OnNeedDataSource" 
            PageSize="10" Style="outline: none; overflow: hidden !important;">
            <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%">
                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Contents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                <ColumnGroups>
                    <telerik:GridColumnGroup HeaderText="COMMENT INFO" Name="CommentInfo"
                            HeaderStyle-HorizontalAlign="Center"/>
                    <telerik:GridColumnGroup HeaderText="SEND COMMENT TO DESIGN CONTRACTOR" Name="SendComment"
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

                    <telerik:GridBoundColumn DataField="ManageSendDate" HeaderText="PVCFC Send Date" UniqueName="ManageSendDate"
                        DataFormatString="{0:dd/MM/yyyy}" Display="False">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </telerik:GridBoundColumn>
                    
                    <telerik:GridBoundColumn DataField="ManageSendTransNumber" HeaderText="PVCFC Send Trans No." UniqueName="ManageSendTransNumber" Display="False">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn DataField="PlanReceiveDate" HeaderText="Plan Comment" UniqueName="PlanReceiveDate"
                        DataFormatString="{0:dd/MM/yyyy}" ColumnGroupName="CommentInfo">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </telerik:GridBoundColumn>
                    
                    <telerik:GridBoundColumn DataField="ActualReceiveDate" HeaderText="Actual Comment" UniqueName="ActualReceiveDate" DataFormatString="{0:dd/MM/yyyy}" ColumnGroupName="CommentInfo">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </telerik:GridBoundColumn>
                    
                    <telerik:GridBoundColumn DataField="ReceiveCodeName" HeaderText="Code" UniqueName="ReceiveCodeName" ColumnGroupName="CommentInfo">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    
                    <telerik:GridBoundColumn DataField="CommentSheetNumber" HeaderText="CMS" UniqueName="CommentSheetNumber" ColumnGroupName="CommentInfo">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    
                    <telerik:GridBoundColumn DataField="ReceiveTransNumber" HeaderText="Trans No." UniqueName="ReceiveTransNumber" Display="False">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    
                    <telerik:GridBoundColumn DataField="PlanSendCMSToDesign" HeaderText="Plan Send CMS" UniqueName="PlanSendCMSToDesign" DataFormatString="{0:dd/MM/yyyy}" ColumnGroupName="SendComment">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </telerik:GridBoundColumn>
                    
                    <telerik:GridBoundColumn DataField="ActualSendCMSToDesign" HeaderText="Actual Send CMS" UniqueName="ActualSendCMSToDesign" DataFormatString="{0:dd/MM/yyyy}" ColumnGroupName="SendComment">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </telerik:GridBoundColumn>
                    
                    <telerik:GridBoundColumn DataField="SendCMSToDesignTransName" HeaderText="Trans No." UniqueName="SendCMSToDesignTransName" ColumnGroupName="SendComment">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
            <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="200" UseStaticHeaders="True" />
            </ClientSettings>
        </telerik:RadGrid>
        
         <%--<div style="width: 100%; text-align: center; padding-top: 270px">
        </div>--%>
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument" OnAjaxRequest="ajaxDocument_AjaxRequest">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="btnSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="txtVSPSendDate" />
                        <telerik:AjaxUpdatedControl ControlID="txtVSPSendTrans"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPlanComment"/>
                        <telerik:AjaxUpdatedControl ControlID="txtActualComment"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlCode"/>
                        <telerik:AjaxUpdatedControl ControlID="txtCommentSheet"/>
                        <telerik:AjaxUpdatedControl ControlID="txtTransNo"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPlanSendCMSToDesign"/>
                        <telerik:AjaxUpdatedControl ControlID="txtActualSendCMSToDesign"/>
                        <telerik:AjaxUpdatedControl ControlID="txtSendCMDToDesignTransNo"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnClear">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtVSPSendDate" />
                        <telerik:AjaxUpdatedControl ControlID="txtVSPSendTrans"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPlanComment"/>
                        <telerik:AjaxUpdatedControl ControlID="txtActualComment"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlCode"/>
                        <telerik:AjaxUpdatedControl ControlID="txtCommentSheet"/>
                        <telerik:AjaxUpdatedControl ControlID="txtTransNo"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPlanSendCMSToDesign"/>
                        <telerik:AjaxUpdatedControl ControlID="txtActualSendCMSToDesign"/>
                        <telerik:AjaxUpdatedControl ControlID="txtSendCMDToDesignTransNo"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="txtVSPSendDate" />
                        <telerik:AjaxUpdatedControl ControlID="txtVSPSendTrans"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPlanComment"/>
                        <telerik:AjaxUpdatedControl ControlID="txtActualComment"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlCode"/>
                        <telerik:AjaxUpdatedControl ControlID="txtCommentSheet"/>
                        <telerik:AjaxUpdatedControl ControlID="txtTransNo"/>
                        <telerik:AjaxUpdatedControl ControlID="txtPlanSendCMSToDesign"/>
                        <telerik:AjaxUpdatedControl ControlID="txtActualSendCMSToDesign"/>
                        <telerik:AjaxUpdatedControl ControlID="txtSendCMDToDesignTransNo"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;

                function fileUploading(sender, args) {
                    var name = args.get_fileName();
                    document.getElementById("txtName").value = name;
                    
                    ajaxManager.ajaxRequest("CheckFileName$" + name);
                }

                function MyClick(sender, eventArgs) {
                    var inputs = document.getElementById("<%= grdDocument.MasterTableView.ClientID %>").getElementsByTagName("input");
                    for (var i = 0, l = inputs.length; i < l; i++) {
                        var input = inputs[i];
                        if (input.type != "radio" || input == sender)
                            continue;
                        input.checked = false;
                    }
                }
                
                function SelectMeOnly(objRadioButton, grdName) {

                    var i, obj;
                    for (i = 0; i < document.all.length; i++) {
                        obj = document.all(i);

                        if (obj.type == "radio") {

                            if (objRadioButton.id.substr(0, grdName.length) == grdName)
                                if (objRadioButton.id == obj.id)
                                    obj.checked = true;
                                else
                                    obj.checked = false;
                        }
                    }
                }
          </script>

        </telerik:RadScriptBlock>
    </form>
</body>
</html>
