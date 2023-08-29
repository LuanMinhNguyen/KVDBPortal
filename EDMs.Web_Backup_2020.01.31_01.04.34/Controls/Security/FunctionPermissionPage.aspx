<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="FunctionPermissionPage.aspx.cs" Inherits="EDMs.Web.Controls.Security.FunctionPermissionPage" EnableViewState="true" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        /*Hide change page size control*/
        div.RadGrid .rgPager .rgAdvPart     
        {     
        display:none;        
        }

        a.tooltip
        {
            outline: none;
            text-decoration: none;
        }

        a.tooltip strong
        {
            line-height: 30px;
        }

        a.tooltip:hover
        {
            text-decoration: none;
        }

        a.tooltip span
        {
            z-index: 10;
            display: none;
            padding: 14px 20px;
            margin-top: -30px;
            margin-left: 5px;
            width: 240px;
            line-height: 16px;
        }

        a.tooltip:hover span
        {
            display: inline;
            position: absolute;
            color: #111;
            border: 1px solid #DCA;
            background: #fffAF0;
        }

        .callout
        {
            z-index: 20;
            position: absolute;
            top: 30px;
            border: 0;
            left: -12px;
        }

        /*CSS3 extras*/
        a.tooltip span
        {
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            -moz-box-shadow: 5px 5px 8px #CCC;
            -webkit-box-shadow: 5px 5px 8px #CCC;
            box-shadow: 5px 5px 8px #CCC;
        }

        .rgMasterTable {
            table-layout: auto;
        }

        #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_grdDocumentPanel, #ctl00_ContentPlaceHolder2_ctl00_ContentPlaceHolder2_divContainerPanel
        {
            height: 100% !important;
        }

        #ctl00_ContentPlaceHolder2_RadPageView1, #ctl00_ContentPlaceHolder2_RadPageView2,
        #ctl00_ContentPlaceHolder2_RadPageView3, #ctl00_ContentPlaceHolder2_RadPageView4,
        #ctl00_ContentPlaceHolder2_RadPageView5
        {
            height: 100% !important;
        }

        #divContainerLeft
        {
            width: 25%;
            float: left;
            margin: 5px;
            height: 99%;
            border-right: 1px dotted green;
            padding-right: 5px;
        }

        #divContainerRight
        {
            width: 100%;
            float: right;
            margin-top: 5px;
            height: 99%;
        }

        .dotted
        {
            border: 1px dotted #000;
            border-style: none none dotted;
            color: #fff;
            background-color: #fff;
        }

        .exampleWrapper
        {
            width: 100%;
            height: 100%;
            /*background: transparent url(~/Images/background.png) no-repeat top left;*/
            position: relative;
        }

        .tabStrip
        {
            position: absolute;
            top: 0px;
            left: 0px;
        }

        .multiPage
        {
            position: absolute;
            top: 30px;
            left: 0px;
            color: white;
            width: 100%;
            height: 100%;
        }

        /*Fix RadMenu and RadWindow z-index issue*/
        .radwindow
        {
            z-index: 8000 !important;
        }

        .TemplateMenu
        {
            z-index: 10;
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

    <telerik:RadPanelBar ID="radPbCostContract" runat="server" Width="100%"/>
    <telerik:RadPanelBar ID="radPbScope" runat="server" Width="100%"/>
    <telerik:RadPanelBar ID="radPbList" runat="server" Width="100%"/>
    <telerik:RadPanelBar ID="radPbSystem" runat="server" Width="100%"/>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Horizontal" Width="100%">
        <telerik:RadPane ID="RadPane3" runat="server" Height="30px" Scrollable="false" Scrolling="None">
            <telerik:RadToolBar ID="CustomerMenu" runat="server" Width="100%" OnButtonClick="CustomerMenu_ButtonClick">
                <Items>
                    <telerik:RadToolBarButton runat="server" Text="Save" Value="Save" ImageUrl="~/Images/save.png"/>

                    <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                </Items>
            </telerik:RadToolBar>

        </telerik:RadPane>
        <telerik:RadPane ID="RadPane2" runat="server" Scrollable="false" Scrolling="None">
            <telerik:RadSplitter ID="Radsplitter3" runat="server" Orientation="Horizontal">
                <telerik:RadPane ID="Radpane4" runat="server" Scrolling="None" >
                     <telerik:RadSplitter ID="Radsplitter10" runat="server" Orientation="Vertical" Width="">
                         <telerik:RadPane ID="Radpane6" runat="server" Scrolling="Both" Width="460">
                            
                             <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="div6">
                                 <div style="padding-bottom: 10px"><img src="../../Images/group.png"/>&nbsp;&nbsp;Department</div>
                                <asp:DropDownList ID="ddlDept" runat="server" CssClass="min25Percent" Style="width: 455px; max-width: 615px" OnSelectedIndexChanged="ddlDept_OnSelectedIndexChanged" AutoPostBack="True"/>
                            </div><br />
                             
                             
                             <br/>
                             <div style="float: left; padding-top: 5px;" class="qlcbFormItem" runat="server" id="divFileName">
                                 <div style="padding-bottom: 10px"><img src="../../Images/user.png"/>&nbsp;&nbsp;User <br/></div>
                                <telerik:RadListBox RenderMode="Lightweight" ID="lbPIC" runat="server" Width="455" Height="350" OnSelectedIndexChanged="lbPIC_OnSelectedIndexChanged" AutoPostBack="True"/>
                            </div>
                        </telerik:RadPane>
                         
                         <telerik:RadPane ID="Radpane1" runat="server" Scrolling="None" >
                             <dl class="accordion">
                                <dt style="width: 100%;">
                                    <span>WORKING MANAGEMENT</span>
                                </dt>
                            </dl>
                             
                             <table>
                                 <tr>
                                     <td style="width: 250px; padding-left: 10px" >1. MATERIAL REQUISITION</td>
                                     <td><asp:CheckBox runat="server" Text="View" ID="cbMRView"/></td>
                                     <td><asp:CheckBox runat="server" Text="Create" ID="cbMRCreate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Update" ID="cbMRUpdate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Cancel" ID="cbMRCancel"/></td>
                                     <td><asp:CheckBox runat="server" Text="Attach Workflow" ID="cbMRAttachWorkflow"/></td>
                                 </tr>
                                 
                                 <tr>
                                     <td style="padding-left: 10px">2. WORK REQUEST</td>
                                     <td><asp:CheckBox runat="server" Text="View" ID="cbWRView"/></td>
                                     <td><asp:CheckBox runat="server" Text="Create" ID="cbWRCreate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Update" ID="cbWRUpdate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Cancel" ID="cbWRCancel"/></td>
                                     <td><asp:CheckBox runat="server" Text="Attach Workflow" ID="cbWRAttachWorkflow"/></td>
                                 </tr>
                                 <tr>
                                     <td style="padding-left: 10px">3. ENGINEERING CHANGE REQUEST</td>
                                     <td><asp:CheckBox runat="server" Text="View" ID="cbECRView"/></td>
                                     <td><asp:CheckBox runat="server" Text="Create" ID="cbECRCreate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Update" ID="cbECRUpdate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Cancel" ID="cbECRCancel"/></td>
                                     <td><asp:CheckBox runat="server" Text="Attach Workflow" ID="cbECRAttachWorkflow"/></td>
                                 </tr>
                                 <tr>
                                     <td style="padding-left: 10px">4. MANAGEMENT OF CHANGE</td>
                                     <td><asp:CheckBox runat="server" Text="View" ID="cbMOCView"/></td>
                                     <td><asp:CheckBox runat="server" Text="Create" ID="cbMOCCreate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Update" ID="cbMOCUpdate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Cancel" ID="cbMOCCancel"/></td>
                                     <td><asp:CheckBox runat="server" Text="Attach Workflow" ID="cbMOCAttachWorkflow"/></td>
                                 </tr>
                                 <tr>
                                     <td style="padding-left: 10px">5. BREAKDOWN REPORT</td>
                                     <td><asp:CheckBox runat="server" Text="View" ID="cbBRView"/></td>
                                     <td><asp:CheckBox runat="server" Text="Create" ID="cbBRCreate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Update" ID="cbBRUpdate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Cancel" ID="cbBRCancel"/></td>
                                     <td><asp:CheckBox runat="server" Text="Attach Workflow" ID="cbBRAttachWorkflow"/></td>
                                 </tr>
                                 <tr>
                                     <td style="padding-left: 10px">6. SHUTDOWN REPORT</td>
                                     <td><asp:CheckBox runat="server" Text="View" ID="cbSRView"/></td>
                                     <td><asp:CheckBox runat="server" Text="Create" ID="cbSRCreate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Update" ID="cbSRUpdate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Cancel" ID="cbSRCancel"/></td>
                                     <td><asp:CheckBox runat="server" Text="Attach Workflow" ID="cbSRAttachWorkflow"/></td>
                                 </tr>
                             </table>

                             <dl class="accordion">
                                <dt style="width: 100%;">
                                    <span>TRACKING MANAGEMENT</span>
                                </dt>
                            </dl>
                             
                             <table>
                                 <tr>
                                     <td style="width: 250px; padding-left: 10px" >1. TRACKING OPERATION MEETING </td>
                                     <td><asp:CheckBox runat="server" Text="View" ID="cbOperationMeetingView"/></td>
                                     <td><asp:CheckBox runat="server" Text="Create" ID="cbOperationMeetingCreate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Update" ID="cbOperationMeetingUpdate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Cancel" ID="cbOperationMeetingCancel"/></td>
                                 </tr>
                                 
                                 <tr>
                                     <td style="width: 250px; padding-left: 10px" >2. TRACKING LIST OF MORNING CALL </td>
                                     <td><asp:CheckBox runat="server" Text="View" ID="cbMorningCallView"/></td>
                                     <td><asp:CheckBox runat="server" Text="Create" ID="cbMorningCallCreate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Update" ID="cbMorningCallUpdate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Cancel" ID="cbMorningCallCancel"/></td>
                                 </tr>
                                 
                                 <tr>
                                     <td style="width: 250px; padding-left: 10px" >3. TRACKING LIST OF WCR </td>
                                     <td><asp:CheckBox runat="server" Text="View" ID="cbWCRView"/></td>
                                     <td><asp:CheckBox runat="server" Text="Create" ID="cbWCRCreate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Update" ID="cbWCRUpdate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Cancel" ID="cbWCRCancel"/></td>
                                 </tr>
                                 
                                 <tr>
                                     <td style="width: 250px; padding-left: 10px" >4. TRACKING LIST OF PUNCH LIST </td>
                                     <td><asp:CheckBox runat="server" Text="View" ID="cbPunchListView"/></td>
                                     <td><asp:CheckBox runat="server" Text="Create" ID="cbPunchListCreate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Update" ID="cbPunchListUpdate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Cancel" ID="cbPunchListCancel"/></td>
                                 </tr>
                                 
                                 <tr>
                                     <td style="width: 250px; padding-left: 10px" >5. TRACKING LIST OF SAIL LIST </td>
                                     <td><asp:CheckBox runat="server" Text="View" ID="cbSailListView"/></td>
                                     <td><asp:CheckBox runat="server" Text="Create" ID="cbSailListCreate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Update" ID="cbSailListUpdate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Cancel" ID="cbSailListCancel"/></td>
                                 </tr>
                                 
                                 <tr>
                                     <td style="width: 250px; padding-left: 10px" >6. TRACKING LIST OF PROCEDURE </td>
                                     <td><asp:CheckBox runat="server" Text="View" ID="cbProcedureView"/></td>
                                     <td><asp:CheckBox runat="server" Text="Create" ID="cbProcedureCreate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Update" ID="cbProcedureUpdate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Cancel" ID="cbProcedureCancel"/></td>
                                 </tr>
                                 
                                 <tr>
                                     <td style="width: 250px; padding-left: 10px" >7. GENERAL WORKING </td>
                                     <td><asp:CheckBox runat="server" Text="View" ID="cbGeneralWorkingView"/></td>
                                     <td><asp:CheckBox runat="server" Text="Create" ID="cbGeneralWorkingCreate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Update" ID="cbGeneralWorkingUpdate"/></td>
                                     <td><asp:CheckBox runat="server" Text="Cancel" ID="cbGeneralWorkingCancel"/></td>
                                 </tr>
                                 
                                 <tr>
                                     <td colspan="5">
                                         

                                     </td>
                                 </tr>
                             </table>
                         </telerik:RadPane>
                     </telerik:RadSplitter>       

                </telerik:RadPane>
                
            </telerik:RadSplitter>
        </telerik:RadPane>
    </telerik:RadSplitter>

    <span style="display: none">
        

        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radPbGroup">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="treePemissionMenus"  LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlDept">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lbPIC"  LoadingPanelID="RadAjaxLoadingPanel2"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbMRView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMRCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMRUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMRCancel"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMRAttachWorkflow"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbWRView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbWRCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbWRUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbWRCancel"/>
                        <telerik:AjaxUpdatedControl ControlID="cbWRAttachWorkflow"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbECRView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbECRCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbECRUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbECRCancel"/>
                        <telerik:AjaxUpdatedControl ControlID="cbECRAttachWorkflow"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbMOCView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMOCCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMOCUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMOCCancel"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMOCAttachWorkflow"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbBRView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbBRCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbBRUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbBRCancel"/>
                        <telerik:AjaxUpdatedControl ControlID="cbBRAttachWorkflow"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbSRView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbSRCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbSRUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbSRCancel"/>
                        <telerik:AjaxUpdatedControl ControlID="cbSRAttachWorkflow"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbOperationMeetingView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbOperationMeetingCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbOperationMeetingUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbOperationMeetingCancel"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbMorningCallView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMorningCallCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMorningCallUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMorningCallCancel"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbWCRView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbWCRCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbWCRUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbWCRCancel"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbGeneralWorkingView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbGeneralWorkingCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbGeneralWorkingUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbGeneralWorkingCancel"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbProcedureView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbProcedureCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbProcedureUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbProcedureCancel"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbSailListView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbSailListCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbSailListUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbSailListCancel"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbPunchListView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbPunchListCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbPunchListUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbPunchListCancel"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="lbPIC">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="cbMRView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMRCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMRUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMRCancel"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMRAttachWorkflow"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbWRView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbWRCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbWRUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbWRCancel"/>
                        <telerik:AjaxUpdatedControl ControlID="cbWRAttachWorkflow"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbECRView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbECRCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbECRUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbECRCancel"/>
                        <telerik:AjaxUpdatedControl ControlID="cbECRAttachWorkflow"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbMOCView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMOCCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMOCUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMOCCancel"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMOCAttachWorkflow"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbBRView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbBRCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbBRUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbBRCancel"/>
                        <telerik:AjaxUpdatedControl ControlID="cbBRAttachWorkflow"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbSRView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbSRCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbSRUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbSRCancel"/>
                        <telerik:AjaxUpdatedControl ControlID="cbSRAttachWorkflow"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbOperationMeetingView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbOperationMeetingCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbOperationMeetingUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbOperationMeetingCancel"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbMorningCallView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMorningCallCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMorningCallUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbMorningCallCancel"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbWCRView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbWCRCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbWCRUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbWCRCancel"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbGeneralWorkingView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbGeneralWorkingCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbGeneralWorkingUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbGeneralWorkingCancel"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbProcedureView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbProcedureCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbProcedureUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbProcedureCancel"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbSailListView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbSailListCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbSailListUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbSailListCancel"/>
                        
                        <telerik:AjaxUpdatedControl ControlID="cbPunchListView"/>
                        <telerik:AjaxUpdatedControl ControlID="cbPunchListCreate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbPunchListUpdate"/>
                        <telerik:AjaxUpdatedControl ControlID="cbPunchListCancel"/>
                        
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="CustomerMenu">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="treePemissionMenus" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="radPbGroup" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </span>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="User Information"
                VisibleStatusbar="false" Height="350" Width="610" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            <telerik:RadWindow ID="AttachDoc" runat="server" Title="Attach documents"
                VisibleStatusbar="false" Height="555" Width="1100" MinHeight="555" MinWidth="1100" MaxHeight="555" MaxWidth="1100" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="DocList" runat="server" Title="Transmittal - Document List"
                VisibleStatusbar="false" Height="418" Width="1100" MinHeight="418" MinWidth="1100" MaxHeight="418" MaxWidth="1100" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
    <asp:HiddenField runat="server" ID="FolderContextMenuAction"/>
    <asp:HiddenField runat="server" ID="lblFolderId"/>
    <asp:HiddenField runat="server" ID="lblDocId"/>
    <asp:HiddenField runat="server" ID="lblCategoryId"/>
    <input type="hidden" id="radGridClickedRowIndex" name="radGridClickedRowIndex"/>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="../../Scripts/jquery-1.7.1.js"></script>
        <script type="text/javascript">
            
            function clientNodeChecked(sender, eventArgs) {
                var i;
                var node = eventArgs.get_node();
                var childNodes = node.get_allNodes();

                eventArgs.get_node().set_checkable(false);

                //To change the status of all child nodes when the parent checked/ unchecked
                if (childNodes.length > 0) {
                    for (i = 0; i < childNodes.length; i++) {
                        childNodes[i].set_checked(node.get_checked());
                    }
                }

                //To uncheck parent Nodes when any child Node has been unchecked
                if (!node.get_checked()) {
                    while (node.get_parent().set_checked != null) {
                        node.get_parent().set_checked(false);
                        node = node.get_parent();
                    }
                }

                //To check parent Nodes when all child nodes has been checked
                if (node.get_checked()) {
                    var parentNode = node.get_parent();
                    while (parentNode.set_checked != null) {
                        var allNodes = parentNode.get_nodes();
                        for (i = 0; i < allNodes.get_count() ; i++) {
                            if (!allNodes.getNode(i).get_checked()) {
                                eventArgs.get_node().set_checkable(true);
                                return;
                            }
                        }
                        parentNode.set_checked(true);
                        parentNode = parentNode.get_parent();
                    }
                }

                eventArgs.get_node().set_checkable(true);
            }

            var radDocuments;
            function Set_Cookie(name, value, expires, path, domain, secure) {
                // set time, it's in milliseconds
                var today = new Date();
                today.setTime(today.getTime());

                /*
                if the expires variable is set, make the correct 
                expires time, the current script below will set 
                it for x number of days, to make it for hours, 
                delete * 24, for minutes, delete * 60 * 24
                */
                if (expires) {
                    expires = expires * 1000 * 60 * 60 * 24;
                }
                var expires_date = new Date(today.getTime() + (expires));

                document.cookie = name + "=" + escape(value) +
                    ((expires) ? ";expires=" + expires_date.toGMTString() : "") +
                    ((path) ? ";path=" + path : "") +
                    ((domain) ? ";domain=" + domain : "") +
                    ((secure) ? ";secure" : "");
            }
        </script>
        <script type="text/javascript">
            /* <![CDATA[ */
            var toolbar;
            var searchButton;
            var ajaxManager;

            function pageLoad() {
                $('iframe').load(function () { //The function below executes once the iframe has finished loading<---true dat, althoug Is coppypasta from I don't know where
                    //alert($('iframe').contents());
                });

                toolbar = $find("<%= CustomerMenu.ClientID %>");
                ajaxManager = $find("<%=ajaxCustomer.ClientID %>");

                searchButton = toolbar.findButtonByCommandName("doSearch");

                $telerik.$(".searchtextbox")
                    .bind("keypress", function (e) {
                        searchButton.set_imageUrl("~/Images/search.gif");
                        searchButton.set_value("search");
                    });
            }

            function ShowUserEditForm(id, userId) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Security/UserEditForm.aspx?Id=" + id + "&userId=" + userId, "CustomerDialog");
            }
            
            function ShowInsertForm() {
                
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Customers/CustomerEditForm.aspx", "CustomerDialog");

                //window.radopen("Controls/Customers/CustomerEditForm.aspx", "CustomerDialog");
                //return false;
            }
            
            

            function refreshGrid(arg) {
                //alert(arg);
                if (!arg) {
                    ajaxManager.ajaxRequest("Rebind");
                }
                else {
                    ajaxManager.ajaxRequest("RebindAndNavigate");
                }
            }
            
            function refreshTab(arg) {
                $('.EDMsRadPageView' + arg + ' iframe').attr('src', $('.EDMsRadPageView' + arg + ' iframe').attr('src'));
            }

            function performSearch(searchTextBox) {
                if (searchTextBox.get_value()) {
                    searchButton.set_imageUrl("~/Images/clear.gif");
                    searchButton.set_value("clear");
                }

                ajaxManager.ajaxRequest(searchTextBox.get_value());
            }
            function onTabSelecting(sender, args) {
                if (args.get_tab().get_pageViewID()) {
                    args.get_tab().set_postBack(false);
                }
            }
            
        /* ]]> */
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
