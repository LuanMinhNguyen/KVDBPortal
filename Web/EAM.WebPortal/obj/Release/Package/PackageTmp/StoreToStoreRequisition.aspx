<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StoreToStoreRequisition.aspx.cs" Inherits="EAM.WebPortal.StoreToStoreRequisition" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2019.3.1023.45, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .RadWizard {
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.3);
            *border: 1px solid #ddd;
        }
        .styleIcon
        {
            margin-left: -25px;
            margin-top: 3px;
            position: fixed;
        }

        .ontop {
            position: fixed;
        }
        .rwzBreadCrumb {
            font: normal 13px helvetica, arial, verdana, sans-serif !important;
        }
        .rwzButton {
            font: normal 13px helvetica, arial, verdana, sans-serif !important;
        }

        #txtSearch {
            font: normal 13px helvetica, arial, verdana, sans-serif !important;
        }
        
        #rtvPart {
            font: normal 13px helvetica, arial, verdana, sans-serif !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <telerik:RadWizard RenderMode="Lightweight" ID="RadWizard1" runat="server" Height="400"  Width="800" OnFinishButtonClick="RadWizard1_OnFinishButtonClick" Localization-Previous="Bước Trước" Localization-Next="Bước Kế Tiếp" Localization-Finish="Kết Thúc">
                <WizardSteps>
                    <telerik:RadWizardStep Title="Chọn Vật Tư - Xuất File Dữ Liệu Mẫu" >
                        <telerik:RadSplitter RenderMode="Lightweight" ID="LeftSplitter" runat="server" Height="100%" Width="100%" Orientation="Horizontal" Skin="Silk">
                            <telerik:RadPane ID="Radpane3" runat="server" Width="100%" Height="26">
                                <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtSearch" runat="server"
                                                    EmptyMessage="Tìm kiếm vật tư" InvalidStyleDuration="100" >
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="btnSearch" runat="server" ImageAlign="AbsMiddle" CssClass="styleIcon"
                                                 ImageUrl="Resources/Images/search20.png" OnClick="btnSearch_OnClick"/>
                            </telerik:RadPane>
                            <telerik:RadPane ID="Radpane4" runat="server" Width="100%" Height="185">
                                 <telerik:RadTreeView RenderMode="Lightweight" ID="rtvPart" runat="server"  CheckBoxes="True" 
                                TriStateCheckBoxes="true" CheckChildNodes="true" Skin="Silk" OnNodeCheck="rtvPart_OnNodeCheck">
                            </telerik:RadTreeView>
                            </telerik:RadPane>
                        </telerik:RadSplitter>
                        <table style="width: 100%">
                            <tr>
                                <td align="center">
                                    <telerik:RadButton ID="btnExport" runat="server" Text="Tải File Mẫu" OnClick="btnExport_Click" >
                                        <Icon PrimaryIconUrl="Resources/Images/excel.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"/>
                                    </telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                    </telerik:RadWizardStep>
                    <telerik:RadWizardStep Title="Tải Nạp File Dữ Liệu">
                        <label style="font: normal 13px helvetica, arial, verdana, sans-serif !important;">Chọn file Yêu cầu cấp vật tư đã được duyệt:</label>
                        <telerik:RadAsyncUpload runat="server" ID="radUpload" MultipleFileSelection="Disabled">
                            <Localization Select="Chọn File"></Localization>
                        </telerik:RadAsyncUpload>
                        <telerik:RadButton ID="btnProcess" runat="server" Text="Tải Nạp Dữ Liệu" OnClick="btnProcess_OnClick" Enabled="True">
                            <Icon PrimaryIconUrl="Resources/Images/process16.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"/>
                        </telerik:RadButton>
                        <asp:Label runat="server" ID="lbltest" Visible="False"></asp:Label>
                        <div runat="server" ID="divMess" style="width: 100%" Visible="False">
                            <br/><b style="color: coral">Thông Báo:</b><br/>
                            <asp:Label runat="server" ID="lblMess"></asp:Label>
                        </div>
                    </telerik:RadWizardStep>
                    
                </WizardSteps>
                
            </telerik:RadWizard>
        </div>
        
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" >
            <Scripts>
            </Scripts>
        </telerik:RadScriptManager>
        
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="MetroTouch"/>
    <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rtvPart" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="btnSearch">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rtvPart" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="rtvPart">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rtvPart" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="radUpload">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="btnProcess" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        
        
        <telerik:AjaxSetting AjaxControlID="btnProcess">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="radUpload" LoadingPanelID="RadAjaxLoadingPanel2"/>
                <telerik:AjaxUpdatedControl ControlID="divMess" LoadingPanelID="RadAjaxLoadingPanel2"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            
        </telerik:RadCodeBlock>
        
    </form>
</body>
</html>
