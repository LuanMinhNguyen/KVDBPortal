<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractorTransETRMAttach.aspx.cs" Inherits="EDMs.Web.Controls.Document.ContractorTransETRMAttach" %>

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
        
          
        <div style="width: 100%; display: none;">
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

        <div style="width: 100%" runat="server" id="divUploadControl">
            <ul style="list-style-type: none">
                <li style="width: 650px;" Runat="server" ID="UploadControl">
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
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                                    
                <li style="width: 100%;">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Type
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:RadioButton runat="server" GroupName="AttachType" Checked="true" ID="cbETRM" 
                                 Text="Transmittal Cover Sheet" /><br/>
                            <asp:RadioButton runat="server" GroupName="AttachType" ID="cbCRS"
                                 Text="CRS Response File" /><br/>
                            <asp:RadioButton runat="server" GroupName="AttachType" ID="cbOther"
                                 Text="Other File" />
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 650px; text-align: center">
                    <telerik:RadButton ID="btnSaveAttachFile" runat="server" Text="Save"  OnClientClicked="ConfirmReplaceFileeTRM" Width="70px" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                    <telerik:RadButton ID="btnCloseAttachFile" runat="server" Text="Close" OnClientClicked="CancelEdit" Width="70px" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/Cancel.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16" ></Icon>
                    </telerik:RadButton>
                                        
                </li>
            </ul>
        </div>
        <telerik:RadGrid ID="grdAttachFile" runat="server" AllowPaging="False"
            AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
            GridLines="None" Skin="Windows7" Height="400"
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
                            <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>

                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                        <HeaderStyle Width="35" />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <a href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' 
                                download='<%# DataBinder.Eval(Container.DataItem, "Filename") %>' target="_blank">
                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ExtensionIcon") %>'
                                    Style="cursor: pointer;" ToolTip="Download document" /> 
                            </a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                            
                    <telerik:GridBoundColumn DataField="Filename" HeaderText="File name" UniqueName="Filename">
                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                        <ItemStyle HorizontalAlign="Left"/>
                    </telerik:GridBoundColumn>
                    
                    <telerik:GridBoundColumn DataField="TypeName" HeaderText="Type" UniqueName="TypeName">
                        <HeaderStyle HorizontalAlign="Center" Width="110" />
                        <ItemStyle HorizontalAlign="Left"/>
                    </telerik:GridBoundColumn>
                                        
                                
                    <telerik:GridBoundColumn DataField="CreatedByName" HeaderText="Upload by" UniqueName="CreatedByName">
                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                        <ItemStyle HorizontalAlign="Left"/>
                    </telerik:GridBoundColumn>
                                
                    <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Upload time" UniqueName="CreatedDate"
                        DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                        <ItemStyle HorizontalAlign="Center"  />
                    </telerik:GridBoundColumn>
                                
                    <telerik:GridBoundColumn DataField="FileSize" HeaderText="File size(Kb)" UniqueName="FileSize" DataFormatString="{0:0,0.00}">
                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                        <ItemStyle HorizontalAlign="Center"/>
                    </telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
            <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="200" UseStaticHeaders="True" />
            </ClientSettings>
        </telerik:RadGrid>

        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        <asp:HiddenField runat="server" ID="IsView"/>
        <asp:HiddenField runat="server" ID="IsCreate"/>
        <asp:HiddenField runat="server" ID="IsUpdate"/>
        <asp:HiddenField runat="server" ID="IsCancel"/>
        <asp:HiddenField runat="server" ID="IsAttachWF"/>
        <asp:HiddenField runat="server" ID="IsHasAttachFileeTRM" />
        <asp:HiddenField runat="server" ID="eTRMChecked" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="Windows7" />
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument" OnAjaxRequest="ajaxDocument_AjaxRequest">
            <ClientEvents OnResponseEnd="EndResponse"/>
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAttachFile" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="grdAttachFile">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAttachFile" ></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="cbETRM">
                    <UpdatedControls>
                         <telerik:AjaxUpdatedControl ControlID="eTRMChecked"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                  <telerik:AjaxSetting AjaxControlID="cbCRS">
                    <UpdatedControls>
                         <telerik:AjaxUpdatedControl ControlID="eTRMChecked"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="cbOther">
                    <UpdatedControls>
                         <telerik:AjaxUpdatedControl ControlID="eTRMChecked"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSaveAttachFile">
                    <UpdatedControls>
                        <%--<telerik:AjaxUpdatedControl ControlID="ajaxDocument" />--%>
                        <%--<telerik:AjaxUpdatedControl ControlID="grdAttachFile" LoadingPanelID="RadAjaxLoadingPanel2"/>--%>
                        <%--<telerik:AjaxUpdatedControl ControlID="txtAttachDescription"/>--%>
                        <telerik:AjaxUpdatedControl ControlID="docuploader"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
         <%--OnClientClose="refreshGrid"--%>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows></Windows></telerik:RadWindowManager>
        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;
                function OnClientFilesUploaded(sender, args) {
                    var name = args.get_Filename();
                    document.getElementById("txtName").value = name;
                    $find('<%=ajaxDocument.ClientID %>').ajaxRequest();
                }

                function pageLoad() {
                    ajaxManager = $find("<%=ajaxDocument.ClientID %>");
                }

                function fileUploading(sender, args) {
                    var name = args.get_Filename();
                    document.getElementById("txtName").value = name;

                    ajaxManager.ajaxRequest("CheckFilename$" + name);
                }
                function ConfirmReplaceFileeTRM() {

                    var radios = document.getElementById("cbETRM").checked;
                    var attach = document.getElementById("IsHasAttachFileeTRM").value;
                    if (attach == "true" && radios == true) {
                        if (confirm("Are you sure to replace transmittal cover sheet?")) {
                            ajaxManager.ajaxRequest("UploadFile");

                        } else {
                            return;
                        }
                    }
                    else {
                       
                        ajaxManager.ajaxRequest("UploadFile");
                    }

                }

                function EndResponse(sender, args) {
                   var masterTable = $find("<%=grdAttachFile.ClientID%>").get_masterTableView();
                    masterTable.rebind();
                }
               
            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>
