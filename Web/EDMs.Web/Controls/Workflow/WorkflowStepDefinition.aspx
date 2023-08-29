
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkflowStepDefinition.aspx.cs" Inherits="EDMs.Web.Controls.Workflow.WorkflowStepDefinition" %>

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


        #rtvAction {
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
        
        <div style="width: 100%" runat="server" ID="EditContent">
            <ul style="list-style-type: none">
                <%-- **********************************Comment********************************* --%>
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Workflow
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="divMilestone">
                            <asp:TextBox ID="txtWorkflow" runat="server" Style="width: 400px;" CssClass="min25Percent qlcbFormRequired" ReadOnly="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Step Name
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtStepName" runat="server" Style="width: 400px;" CssClass="min25Percent qlcbFormRequired" />
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Apply For Actions
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:RadTreeView RenderMode="Lightweight" ID="rtvAction" runat="server"  CheckBoxes="True" TriStateCheckBoxes="true" CheckChildNodes="true" Width="400" Height="97">
                                <Nodes>
                                    <telerik:RadTreeNode runat="server" Value="R" Text="R - Review"/>
                                    <telerik:RadTreeNode runat="server" Value="C" Text="C - Consolidate"/>
                                    <telerik:RadTreeNode runat="server" Value="A" Text="A - Approve"/>
                                    <telerik:RadTreeNode runat="server" Value="I" Text="I - Information"/>
                                </Nodes>
                            </telerik:RadTreeView>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <%--<li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Location
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div1">
                            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="min25Percent" Style="width: 315px; max-width: 315px">
                                <Items>
                                    <asp:ListItem Value="1" Text="Onshore"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Offshore"></asp:ListItem>
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>--%>

                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div2">
                            <asp:CheckBox runat="server" ID="cbIsFirst" Text="Is First Step"/>
                            <asp:CheckBox runat="server" ID="cbCanReject" Text="Can Reject"/>
                             <asp:CheckBox runat="server" ID="cbiscreate" Text="Is Engineer Create"/>
                            <asp:CheckBox runat="server" ID="cbCanCreateOutgoingTrans" Text="Can Create Outgoing Trans"/>
                           
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
            GridLines="None" Skin="Windows7" Height="420"
            OnDeleteCommand="grdDocument_DeleteCommand"
            OnItemCommand="grdDocument_OnItemCommand"
            OnNeedDataSource="grdDocument_OnNeedDataSource" 
            PageSize="20" Style="outline: none; overflow: hidden !important;">
            <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%">
                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Contents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                <Columns>
                    <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                    <telerik:GridButtonColumn UniqueName="EditColumn" CommandName="EditCmd" ButtonType="ImageButton" ImageUrl="~/Images/edit.png">
                        <HeaderStyle Width="24px" />
                        <ItemStyle HorizontalAlign="Center"  />
                    </telerik:GridButtonColumn>

                    <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" ConfirmText="Do you want to delete content?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                        <HeaderStyle Width="24px" />
                        <ItemStyle HorizontalAlign="Center"/>
                    </telerik:GridButtonColumn>
                    
                    <telerik:GridBoundColumn DataField="Name" HeaderText="Step Name" UniqueName="Name" >
                        <HeaderStyle HorizontalAlign="Center" Width="250px" />
                        <ItemStyle HorizontalAlign="Left" />
                    </telerik:GridBoundColumn>
                    
                    <telerik:GridTemplateColumn HeaderText="Apply For Action" UniqueName="ActionApplyName"
                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="ActionApplyName"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <%# Eval("ActionApplyName").ToString().Replace(Environment.NewLine, "<br/>") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="First Step" UniqueName="IsFirst" AllowFiltering="False" >
                            <HeaderStyle Width="40" HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:Image ID="Image1" runat="server" 
                                Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsFirst"))%>'
                                ImageUrl='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsFirst")) ? "~/Images/ok.png" : "" %>'/>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    <telerik:GridTemplateColumn HeaderText="Can Reject" UniqueName="CanReject" AllowFiltering="False" >
                            <HeaderStyle Width="40" HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:Image ID="Image2" runat="server" 
                                Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "CanReject"))%>'
                                ImageUrl="~/Images/ok.png"/>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Engineer Create" UniqueName="IsCreated" AllowFiltering="False" >
                            <HeaderStyle Width="80" HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:Image ID="imgIsCreated" runat="server" 
                                Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsCreated"))%>'
                                ImageUrl="~/Images/ok.png"/>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Can Create Outgoing Trans" UniqueName="IsCanCreateOutgoingTrans" AllowFiltering="False" >
                            <HeaderStyle Width="80" HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:Image ID="imgCreateOutgoingTrans" runat="server" 
                                Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsCanCreateOutgoingTrans"))%>'
                                ImageUrl="~/Images/ok.png"/>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
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
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="Windows7"/>
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument" OnAjaxRequest="ajaxDocument_AjaxRequest">
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="btnSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="txtStepName" />                        
                        <telerik:AjaxUpdatedControl ControlID="ddlLocation"/>
                        <telerik:AjaxUpdatedControl ControlID="cbIsFirst"/>
                        <telerik:AjaxUpdatedControl ControlID="cbCanReject"/>
                        <telerik:AjaxUpdatedControl ControlID="cbiscreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbCanCreateOutgoingTrans"/>
                        <telerik:AjaxUpdatedControl ControlID="rtvAction"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnClear">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtStepName" />
                        <telerik:AjaxUpdatedControl ControlID="ddlLocation"/>
                        <telerik:AjaxUpdatedControl ControlID="cbIsFirst"/>
                        <telerik:AjaxUpdatedControl ControlID="cbCanReject"/>
                        <telerik:AjaxUpdatedControl ControlID="cbiscreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbCanCreateOutgoingTrans"/>
                        <telerik:AjaxUpdatedControl ControlID="rtvAction"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="grdDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="txtStepName" />
                        <telerik:AjaxUpdatedControl ControlID="ddlLocation"/>
                        <telerik:AjaxUpdatedControl ControlID="cbIsFirst"/>
                        <telerik:AjaxUpdatedControl ControlID="cbCanReject"/>
                        <telerik:AjaxUpdatedControl ControlID="cbiscreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbCanCreateOutgoingTrans"/>
                        <telerik:AjaxUpdatedControl ControlID="rtvAction"/>
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
