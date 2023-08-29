<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisciplinePermissionPage.aspx.cs" Inherits="EDMs.Web.Controls.Security.DisciplinePermissionPage" EnableViewState="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .RadAjaxPanel {
            height: 100% !important;
        }

         div.qlcbFormItem select.min25Percent {
             max-width: 100% !important;
         }
        .rtPlus, .rtMinus
        {
            background-image: url('Images/plus-minus-icon1.png') !important;
            cursor: pointer;
        }

        .rtIn {
            cursor: pointer;
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
            /*background: transparent url(images/background.png) no-repeat top left;*/
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

        .RadListBox
        {
            float: left;
        }
    </style>
    
    <telerik:RadPanelBar ID="radPbCostContract" runat="server" Width="100%"/>
    <telerik:RadPanelBar ID="radPbScope" runat="server" Width="100%"/>
    <telerik:RadPanelBar ID="radPbList" runat="server" Width="100%" Visible="False"/>
    <telerik:RadPanelBar ID="radPbSystem" runat="server" Width="100%"/>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadSplitter ID="RadSplitter4" runat="server" Orientation="Horizontal" Width="100%">
        <%--<telerik:RadPane ID="RadPane3" runat="server" Height="30px" Scrollable="false" Scrolling="None">
            <div style="padding-top: 4px; padding-left:10px">
            <telerik:RadButton ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" >
                <Icon PrimaryIconUrl="Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="20"></Icon>
            </telerik:RadButton>
        </div>

        </telerik:RadPane>--%>
        <telerik:RadPane ID="RadPane2" runat="server" Scrollable="false" Scrolling="None">
            <telerik:RadSplitter ID="Radsplitter3" runat="server" Orientation="Horizontal">
                <telerik:RadPane ID="Radpane4" runat="server" Scrolling="None" >
                    
                     <telerik:RadSplitter ID="Radsplitter10" runat="server" Orientation="Vertical">
                         <telerik:RadPane ID="Radpane6" runat="server" Scrolling="Both" Width="30%">
                             <div style="padding-top: 4px; padding-left: 10px; width: 97%">
                                 Selected Project:
                                 <div style="" class="qlcbFormItem">
                                 <asp:DropDownList ID="ddlProject" runat="server" AutoPostBack="True" CssClass="min25Percent" Width="100%" OnSelectedIndexChanged="ddlProject_SelectedIndexChange"/>
                                </div>
                                 <br/>
                                 <br/>
                                 Disciplines:
                                 <telerik:RadTreeView ID="rtvDiscipline" runat="server" style="padding-top: 10px;" 
                                     Width="100%" Height="100%" ShowLineImages="False" 
                                     OnNodeClick="rtvDiscipline_NodeClick"
                                     OnNodeDataBound="rtvDiscipline_OnNodeDataBound">
                                    <DataBindings>
                                        <telerik:RadTreeNodeBinding Expanded="false"></telerik:RadTreeNodeBinding>
                                    </DataBindings>
                                </telerik:RadTreeView>
                            </div>
                        </telerik:RadPane>
                            
                         <telerik:RadPane ID="Radpane1" runat="server" Scrolling="Both" Width="60%">
                             
                             <telerik:RadSplitter ID="RadSplitter1" runat="server" Orientation="Horizontal">
                                <telerik:RadPane ID="RadPane3" runat="server" Height="250" Scrollable="false" Scrolling="None">
                                    <ul style="list-style-type: none">
                                    <li style="width: 100%;">
                                        <div>
                                            <label style="width: 60px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">Permission
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                <asp:RadioButton ID="rbtnFull" runat="server" GroupName="Permission" Text="Full permission"/>
                                                <asp:RadioButton ID="rbtnView" runat="server" GroupName="Permission" Checked="True" Text="View only"/>
                                            </div>
                                        </div>
                                        <div style="clear: both; font-size: 0;"></div>
                                    </li>
                                    <li style="width: 100%;">
                                        <div>
                                            <label style="width: 60px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                                                <span style="color: #2E5689; text-align: right; ">User List
                                                </span>
                                            </label>
                                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                                                <telerik:RadListBox runat="server" ID="ddlUser" Height="160" Width="400px" SelectionMode="Multiple"/>
                                                
                                            </div>
                                        </div>
                                        <div style="clear: both; font-size: 0;"></div>
                                    </li>
                
                                    <li style="width: 100%; padding-top: 10px; padding-bottom: 3px; text-align: center">
                                        <telerik:RadButton ID="btnSave" runat="server" Text="Add Permission" OnClick="btnSave_Click" 
                                            Width="135" style="text-align: center">
                                            <Icon PrimaryIconUrl="~/Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                                        </telerik:RadButton>
                                    </li>
                
                                    <li style="width: 100%;">
                                    </li>
                                </ul>
                                </telerik:RadPane>
                                 <telerik:RadPane ID="RadPane5" runat="server" Scrollable="false" Scrolling="None">
                                     <telerik:RadGrid ID="grdPermission" runat="server" AllowPaging="True" 
                                         AutoGenerateColumns="False" CellSpacing="0" CellPadding="0" 
                                            PageSize="100" Height="95%" GridLines="None"
                                            Skin="Windows7" AllowFilteringByColumn="True"
                                            OnNeedDataSource="grdPermission_OnNeedDataSource"
                                        OnDeleteCommand="grdPermission_OnDeteleCommand">
                                        <GroupingSettings CaseSensitive="False"></GroupingSettings>
                                        <MasterTableView DataKeyNames="Id" ClientDataKeyNames="Id" Width="100%" >
                                            <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; records." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <Columns>
                                                <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" 
                                                    ConfirmText="Do you want to delete item?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                                                    <HeaderStyle Width="3%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="3%"  />
                                                </telerik:GridButtonColumn>

                                                <telerik:GridBoundColumn DataField="UserObj.UserNameWithFullName" HeaderText="User Name/Full Name" UniqueName="UserName" 
                                                    ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                    <HeaderStyle Width="35%" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle Width="35%" HorizontalAlign="Left"></ItemStyle>
                                                </telerik:GridBoundColumn>
                        
                                                <telerik:GridBoundColumn DataField="GroupObj.Name" HeaderText="Group" UniqueName="GroupName" 
                                                    ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                    <HeaderStyle Width="35%" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle Width="35%" HorizontalAlign="Left"></ItemStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="Full permission" UniqueName="IsFullPermission" AllowFiltering="False">
                                                        <HeaderStyle Width="10%" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsFullPermission")) ? "~/Images/ok.png" : "" %>'/>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                
                                            </Columns>
                                            <CommandItemStyle Height="25px"></CommandItemStyle>
                                        </MasterTableView>
                                        <ClientSettings>
                                            <Selecting AllowRowSelect="true"></Selecting>
                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True" ScrollHeight="500"></Scrolling>
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </telerik:RadPane>
                            </telerik:RadSplitter>
                        </telerik:RadPane>
                     </telerik:RadSplitter>       
                </telerik:RadPane>
            </telerik:RadSplitter>
        </telerik:RadPane>
    </telerik:RadSplitter>

    <span style="display: none">
        

        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rtvDiscipline">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdPermission" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlUser" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlGroup" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="ddlProject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtvDiscipline" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdPermission" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlUser" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlGroup" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlGroup">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlUser" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="grdPermission">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdPermission" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlUser" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlGroup" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="btnSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdPermission" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlUser" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="ddlGroup" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </span>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Document Status Information"
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
        <script type="text/javascript">
            
            var radDocuments;


            function GetGridObject(sender, eventArgs) {
                radDocuments = sender;
            }

        </script>
        <script type="text/javascript">
            /* <![CDATA[ */
            var toolbar;
            var searchButton;
            var ajaxManager;

            function pageLoad() {
                ajaxManager = $find("<%=ajaxCustomer.ClientID %>");
            }
            
        /* ]]> */
        </script>
    </telerik:RadCodeBlock>
</asp:Content>