<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="EDMs.Web.Search" EnableViewState="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="Content/styles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        
        html, body, form {
	        overflow:hidden;
        }
        
        #RAD_SPLITTER_PANE_CONTENT_ctl00_rightPane {
            overflow:hidden !important;
        }

        .RadAjaxPanel {
            height: 100% !important;
        }
        .accordion dt a
        {
            color: #085B8F;
            border-bottom: 2px solid #46A3D3;
            font-size: 1.5em;
            font-weight: bold;
            letter-spacing: -0.03em;
            line-height: 1.2;
            margin: 0.5em auto 0.6em;
            padding: 0;
            text-align: left;
            text-decoration: none;
            display: block;
        }

        .accordion dt span
        {
            color: #085B8F;
            border-bottom: 2px solid #46A3D3;
            font-size: 1.5em;
            font-weight: bold;
            letter-spacing: -0.03em;
            line-height: 1.2;
            margin: 0.5em auto 0.6em;
            padding: 0;
            text-align: left;
            text-decoration: none;
            display: block;
        }

        .qlcbTooltip {
            line-height: 1.8;
            padding-right: 5px;
            text-align: right;
        }
         .qlcbFormItem input[type="text"], .qlcbFormItem textarea, .qlcbFormItem select {
            border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;
            border-style: solid;
            border-width: 1px 1px 1px 5px;
            color: #000000;
            float: left;
            font: 12px "segoe ui";
            margin: 0;
            padding: 2px 5px 3px;
            vertical-align: middle;
    
        }
         html body .riSingle .riTextBox, html body .riSingle .riTextBox[type="text"] {
   
            border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3!important;
            border-style: solid!important;
            border-width: 1px 1px 1px 5px!important;
            color: #000000!important;
            float: left!important;
            font: 12px "segoe ui"!important;
            margin: 0!important;
            padding: 2px 5px 3px!important;
            vertical-align: middle!important;
        }
         div.RadPicker input.qlcbFormRequired1[type="text"], div.RadPicker_Default input.qlcbFormRequired1[type="text"] {
            border-left-color: Red!important;
            border-left-width: 5px!important;
        }
         .qlcbFormItem input.min25Percent[type="text"], div.qlcbFormItem textarea.min25Percent {
            min-width: 235px;
        }
        .qlcbFormItem input.minFullWidth[type="text"], div.qlcbFormItem textarea.minFullWidth {
            min-width: 626px;
        }
        .qlcbFormItem select.min25Percent {
            min-width: 250px;
        }
        .qlcbFormItem input.min50Percent[type="text"], div.qlcbFormItem textarea.min50Percent, div.qlcbFormItem select.min50Percent {
            min-width: 50%;
        }
        .qlcbFormItem input.qlcbFormRequired[type="text"], div.qlcbFormItem textarea.qlcbFormRequired, div.qlcbFormItem select.qlcbFormRequired {
            border-left-color: #FF0000;
            border-left-width: 5px;
        }
        .qlcbFormItem input.qlcbFormUPPERCASE[type="text"], div.qlcbFormItem textarea.qlcbFormUPPERCASE {
            text-transform: uppercase;
        }
        .qlcbFormItem input[type="text"], div.qlcbFormItem textarea, div.qlcbFormItem select {
            border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;
            border-style: solid;
            border-width: 1px 1px 1px 5px;
            color: #000000;
            float: left;
            font: 12px "segoe ui";
            margin: 0;
            padding: 2px 5px 3px;
            vertical-align: middle;
        }
        .qlcbFormItem input[type="text"]:hover, div.qlcbFormItem select:hover {
            border-color: #000000 #000000 #000000 #46A3D3;
        }
        .qlcbFormItem input.qlcbFormRequired[type="text"]:hover, div.qlcbFormItem select.qlcbFormRequired:hover {
            border-color: #000000 #000000 #000000 #FF0000;
        }
        .RadPicker, div.RadPicker_Default {
            display: inline !important;
            float: left !important;
        }
        .min25Percent {
            min-width: 217px;
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

        /*Hide change page size control*/
        div.RadGrid .rgPager .rgAdvPart     
        {     
        display:none;        
        }   

        .RadGrid .rgRow td, .RadGrid .rgAltRow td, .RadGrid .rgEditRow td, .RadGrid .rgFooter td, .RadGrid .rgFilterRow td, .RadGrid .rgHeader, .RadGrid .rgResizeCol, .RadGrid .rgGroupHeader td {
            padding-left: 1px !important;
            padding-right: 1px !important;
        }

        /*#RAD_SPLITTER_PANE_CONTENT_ctl00_leftPane {
            width: 350px !important;
        }
        #RAD_SPLITTER_PANE_CONTENT_ctl00_topLeftPane {
            width: 350px !important;
        }*/
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
            width: 96%;
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
    </style>
    
    <dl class="accordion">
        <dt style="width: 100%;">
            <span>Search conditions</span>
        </dt>
    </dl>
    <div style="width: 100%" runat="server" ID="divContent">
        <div style="display: none">
            <telerik:RadTreeView runat="server" ID="rtvOptionalTypeDetailTemp" Skin="Windows7">
                <DataBindings>
                    <telerik:RadTreeNodeBinding Expanded="false"></telerik:RadTreeNodeBinding>
                </DataBindings>
            </telerik:RadTreeView>
        </div>
        <div style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Category<br/>
            <telerik:RadComboBox ID="ddlCategory" runat="server" Skin="Windows7"
                style="width: 100% !important; border-left-color: #FF0000 !important; "
                AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"/>
        </div>
        
        <div style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Search full fields<br/>
            <asp:TextBox ID="txtSearchFullField" runat="server" Style="width: 93% !important;"/>
        </div>

        <div runat="server" ID="Index1" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Name<br/>
            <asp:TextBox ID="txtName" runat="server" Style="width: 93% !important;"/>
        </div>
        
        <div runat="server" ID="Index2" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Description<br/>
            <asp:TextBox ID="txtDescription" runat="server" Style="width: 93% !important;"/>
        </div>
        
        <div runat="server" ID="Index3" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Revision<br/>
            <telerik:RadComboBox ID="ddlRevision" runat="server" Skin="Windows7" style="width: 50% !important;"/>
        </div>
        
        <div runat="server" ID="Index4" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Vendor Name<br/>
            <asp:TextBox ID="txtVendorName" runat="server" Style="width: 93% !important;"/>
        </div>
        
        <div runat="server" ID="Index5" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Drawing Number<br/>
            <asp:TextBox ID="txtDrawingNumber" runat="server" Style="width: 93% !important;"/>
        </div>
        
        <div runat="server" ID="Index6" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Year<br/>
            <asp:DropDownList ID="ddlYear" runat="server" Width="50%"/>
        </div>
        
        <div runat="server" ID="Index7" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">  Plant<br/>
            <telerik:RadComboBox ID="ddlPlant" runat="server" Skin="Windows7"
                style="width: 100% !important;" MaxHeight="250"
                OnClientDropDownOpened="ddlPlantDDHandler">
                <ItemTemplate>
                    <div id="div2">
                        <telerik:RadTreeView runat="server" ID="rtvPlant" Skin="Windows7" OnClientNodeClicking="rtvPlantNodeClicking">
                            <DataBindings>
                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                        </DataBindings>
                        </telerik:RadTreeView>
                    </div>
                </ItemTemplate>
                <Items>
                    <telerik:RadComboBoxItem Text="" />
                </Items>
            </telerik:RadComboBox>
        </div>
        
        <div runat="server" ID="Index8" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">System<br/>
            <telerik:RadComboBox ID="ddlSystem" runat="server" Skin="Windows7"
                style="width: 100% !important;" MaxHeight="250"
                OnClientDropDownOpened="ddlSystemDDHandler">
                <ItemTemplate>
                    <div id="div1">
                        <telerik:RadTreeView runat="server" ID="rtvSystem" Skin="Windows7" OnClientNodeClicking="rtvSystemNodeClicking">
                            <DataBindings>
                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                        </DataBindings>
                        </telerik:RadTreeView>
                    </div>
                </ItemTemplate>
                <Items>
                    <telerik:RadComboBoxItem Text="" />
                </Items>
            </telerik:RadComboBox>
        </div>
        
        <div runat="server" ID="Index9" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Discipline<br/>
            <telerik:RadComboBox ID="ddlDiscipline" runat="server" Skin="Windows7"
                style="width: 100% !important;" MaxHeight="250"
                OnClientDropDownOpened="ddlDisciplineDDHandler">
                <ItemTemplate>
                    <div id="div3">
                        <telerik:RadTreeView runat="server" ID="rtvDiscipline" Skin="Windows7" OnClientNodeClicking="rtvDisciplineNodeClicking">
                            <DataBindings>
                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                        </DataBindings>
                        </telerik:RadTreeView>
                    </div>
                </ItemTemplate>
                <Items>
                    <telerik:RadComboBoxItem Text="" />
                </Items>
            </telerik:RadComboBox>
        </div>
        
        <div runat="server" ID="Index10" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Document Type<br/>
            <telerik:RadComboBox ID="ddlDocumentType" runat="server" Skin="Windows7"
                style="width: 100% !important;" MaxHeight="250"
                OnClientDropDownOpened="ddlDocumentTypeDDHandler">
                <ItemTemplate>
                    <div id="div4">
                        <telerik:RadTreeView runat="server" ID="rtvDocumentType" Skin="Windows7" OnClientNodeClicking="rtvDocumentTypeNodeClicking">
                            <DataBindings>
                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                        </DataBindings>
                        </telerik:RadTreeView>
                    </div>
                </ItemTemplate>
                <Items>
                    <telerik:RadComboBoxItem Text="" />
                </Items>
            </telerik:RadComboBox>
        </div>
        
        <div runat="server" ID="Index11" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Tag Type<br/>
            <telerik:RadComboBox ID="ddlTagType" runat="server" Skin="Windows7"
                style="width: 100% !important;" MaxHeight="250"
                OnClientDropDownOpened="ddlTagTypeDDHandler">
                <ItemTemplate>
                    <div id="div6">
                        <telerik:RadTreeView runat="server" ID="rtvTagType" Skin="Windows7" OnClientNodeClicking="rtvTagTypeNodeClicking">
                            <DataBindings>
                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                        </DataBindings>
                        </telerik:RadTreeView>
                    </div>
                </ItemTemplate>
                <Items>
                    <telerik:RadComboBoxItem Text="" />
                </Items>
            </telerik:RadComboBox>
        </div>
        
        <div runat="server" ID="Index12" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Project<br/>
            <telerik:RadComboBox ID="ddlProject" runat="server" Skin="Windows7"
                style="width: 100% !important;" MaxHeight="250"
                OnClientDropDownOpened="ddlProjectDDHandler">
                <ItemTemplate>
                    <div id="div5">
                        <telerik:RadTreeView runat="server" ID="rtvProject" Skin="Windows7" 
                            OnNodeClick="rtvProject_Nodeclick"
                            OnClientNodeClicking="rtvProjectNodeClicking">
                            <DataBindings>
                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                        </DataBindings>
                        </telerik:RadTreeView>
                    </div>
                </ItemTemplate>
                <Items>
                    <telerik:RadComboBoxItem Text="" />
                </Items>
            </telerik:RadComboBox>
        </div>
        
        <div runat="server" ID="Index13" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Block<br/>
            <telerik:RadComboBox ID="ddlBlock" runat="server" Skin="Windows7"
                style="width: 100% !important;" MaxHeight="250"
                OnClientDropDownOpened="ddlBlockDDHandler">
                <ItemTemplate>
                    <div id="div8">
                        <telerik:RadTreeView runat="server" ID="rtvBlock" Skin="Windows7" 
                            OnNodeClick="rtvBlock_NodeClick"
                            OnClientNodeClicking="rtvBlockNodeClicking">
                            <DataBindings>
                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                        </DataBindings>
                        </telerik:RadTreeView>
                    </div>
                </ItemTemplate>
                <Items>
                    <telerik:RadComboBoxItem Text="" />
                </Items>
            </telerik:RadComboBox>
        </div>
        
        <div runat="server" ID="Index14" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Field<br/>
            <telerik:RadComboBox ID="ddlField" runat="server" Skin="Windows7"
                style="width: 100% !important;" MaxHeight="250"
                OnClientDropDownOpened="ddlFieldDDHandler">
                <ItemTemplate>
                    <div id="div9">
                        <telerik:RadTreeView runat="server" ID="rtvField" Skin="Windows7" 
                            OnNodeClick="rtvField_NodeClick"
                            OnClientNodeClicking="rtvFieldNodeClicking">
                            <DataBindings>
                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                        </DataBindings>
                        </telerik:RadTreeView>
                    </div>
                </ItemTemplate>
                <Items>
                    <telerik:RadComboBoxItem Text="" />
                </Items>
            </telerik:RadComboBox>
        </div>
        
        <div runat="server" ID="Index15" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Field<br/>
            <telerik:RadComboBox ID="ddlPlatform" runat="server" Skin="Windows7"
                style="width: 100% !important;" MaxHeight="250"
                OnClientDropDownOpened="ddlPlatformDDHandler">
                <ItemTemplate>
                    <div id="div10">
                        <telerik:RadTreeView runat="server" ID="rtvPlatform" Skin="Windows7" 
                            OnNodeClick="rtvPlatform_NodeClick"
                            OnClientNodeClicking="rtvPlatformNodeClicking">
                            <DataBindings>
                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                        </DataBindings>
                        </telerik:RadTreeView>
                    </div>
                </ItemTemplate>
                <Items>
                    <telerik:RadComboBoxItem Text="" />
                </Items>
            </telerik:RadComboBox>
        </div>
        
        <div runat="server" ID="Index16" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Well<br/>
            <telerik:RadComboBox ID="ddlWell" runat="server" Skin="Windows7"
                style="width: 100% !important;" MaxHeight="250"
                OnClientDropDownOpened="ddlWellDDHandler">
                <ItemTemplate>
                    <div id="div11">
                        <telerik:RadTreeView runat="server" ID="rtvWell" Skin="Windows7" OnClientNodeClicking="rtvWellNodeClicking">
                            <DataBindings>
                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                        </DataBindings>
                        </telerik:RadTreeView>
                    </div>
                </ItemTemplate>
                <Items>
                    <telerik:RadComboBoxItem Text="" />
                </Items>
            </telerik:RadComboBox>
        </div>
        
        <div runat="server" ID="Index17" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Start Date<br/>
            <telerik:RadDatePicker ID="txtStartDate"  runat="server" CssClass="qlcbFormNonRequired">
                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
            </telerik:RadDatePicker>
        </div>
        
        <div runat="server" ID="Index18" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">End Date<br/>
            <telerik:RadDatePicker ID="txtEndDate"  runat="server" CssClass="qlcbFormNonRequired">
                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
            </telerik:RadDatePicker>
        </div>
        
         <div runat="server" ID="Index19" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Number Of Work<br/>
             <telerik:radnumerictextbox type="Number" id="txtNumberOfWork" runat="server" Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="50px" CssClass="min25Percent">
                <NumberFormat DecimalDigits="0"></NumberFormat>
            </telerik:radnumerictextbox>
        </div>
        
        <div runat="server" ID="Index20" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Tag No<br/>
            <asp:TextBox ID="txtTagNo" runat="server" Style="width: 93% !important;"/>
        </div>
        
        <div runat="server" ID="Index21" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Tag Des<br/>
            <asp:TextBox ID="txtTagDes" runat="server" Style="width: 93% !important;"/>
        </div>

        <div runat="server" ID="Index22" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Manufacturers<br/>
            <asp:TextBox ID="txtManufacturers" runat="server" Style="width: 93% !important;"/>
        </div>
        
        <div runat="server" ID="Index23" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Serial No<br/>
            <asp:TextBox ID="txtSerialNo" runat="server" Style="width: 93% !important;"/>
        </div>
        
        <div runat="server" ID="Index24" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Model No<br/>
            <asp:TextBox ID="txtModelNo" runat="server" Style="width: 93% !important;"/>
        </div>
        
        <div runat="server" ID="Index25" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Asset No<br/>
            <asp:TextBox ID="txtAssetNo" runat="server" Style="width: 93% !important;"/>
        </div>
        
        <div runat="server" ID="Index26" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Table Of Contents<br/>
            <asp:TextBox ID="txtTableOfContents" runat="server" Style="width: 93% !important;"/>
        </div>
        
        <div runat="server" ID="Index27" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Publish Date<br/>
            <telerik:RadDatePicker ID="txtPublishDate"  runat="server" CssClass="qlcbFormNonRequired">
                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
            </telerik:RadDatePicker>
        </div>
        
        <div runat="server" ID="Index28" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">From<br/>
            <telerik:RadComboBox ID="ddlFrom" runat="server" Skin="Windows7" style="width: 50% !important;"/>
        </div>
        
        <div runat="server" ID="Index29" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">To<br/>
            <telerik:RadComboBox ID="ddlTo" runat="server" Skin="Windows7" style="width: 50% !important;"/>
        </div>
        
        <div runat="server" ID="Index30" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Signer<br/>
            <asp:TextBox ID="txtSigner" runat="server" Style="width: 93% !important;"/>
        </div>

        <div runat="server" ID="Index31" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Other<br/>
            <asp:TextBox ID="txtOther" runat="server" Style="width: 93% !important;"/>
        </div>
        
        <div runat="server" ID="Index32" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">RIG<br/>
            <telerik:RadComboBox ID="ddlRIG" runat="server" Skin="Windows7"
                style="width: 100% !important;" MaxHeight="250"
                OnClientDropDownOpened="ddlRIGDDHandler">
                <ItemTemplate>
                    <div id="div12">
                        <telerik:RadTreeView runat="server" ID="rtvRIG" Skin="Windows7" OnClientNodeClicking="rtvRIGNodeClicking">
                            <DataBindings>
                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                        </DataBindings>
                        </telerik:RadTreeView>
                    </div>
                </ItemTemplate>
                <Items>
                    <telerik:RadComboBoxItem Text="" />
                </Items>
            </telerik:RadComboBox>
        </div>
        
        <div runat="server" ID="Index33" Visible="False" 
            style="float: left; width: 100%; padding-top: 5px; " class="qlcbFormItem">Kind of repair<br/>
            <asp:TextBox ID="txtKindOfRepair" runat="server" Style="width: 93% !important;"/>
        </div>

        <div style="text-align: center; width: 100%; padding-top: 10px; " class="qlcbFormItem">
            <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="Search"/>
            <asp:Button runat="server" ID="btnClearSearch" OnClick="btnClearSearch_Click" Text="Clear Search"/>
        </div>
    </div>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server" >
    <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="True" Height="100%"
                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                GridLines="None" AllowCustomPaging="True"
                OnPageIndexChanged="grdDocument_PageIndexChanged"
                OnItemDataBound="grdDocument_ItemDataBound"
                OnDeleteCommand="grdDocument_DeleteCommand"
                OnUpdateCommand="grdDocument_UpdateCommand" 
                OnNeedDataSource="grdDocument_OnNeedDataSource" 
                OnItemCommand="grdDocument_ItemCommand"
                PageSize="20" Style="outline: none">
        <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" EditMode="InPlace" Font-Size="8pt" >
            <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
            <Columns>
                <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                <telerik:GridBoundColumn DataField="DisciplineID" UniqueName="DisciplineID" Visible="False" />
                <telerik:GridBoundColumn DataField="FilePath" UniqueName="FilePath" Visible="False" />
                <telerik:GridTemplateColumn UniqueName="IsSelected" AllowFiltering="false" Visible="False">
                    <HeaderStyle Width="2%"  />
                    <ItemStyle HorizontalAlign="Center" Width="2%"/>
                    <ItemTemplate>
                        <asp:CheckBox ID="IsSelected" runat="server" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <%--<telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="~/Images/edit.png" 
                    UpdateImageUrl="~/Images/ok.png" CancelImageUrl="~/Images/delete.png" UniqueName="EditColumn">
                    <HeaderStyle Width="4%"  />
                    <ItemStyle HorizontalAlign="Center" Width="4%"/>
                </telerik:GridEditCommandColumn>--%>
                <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" HeaderTooltip="Delete document"
                    ConfirmText="Do you want to delete document?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                    <HeaderStyle Width="1%" />
                        <ItemStyle HorizontalAlign="Center" Width="1%"  />
                </telerik:GridButtonColumn>
                <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn" HeaderTooltip="Download document package">
                    <HeaderStyle Width="1%" />
                    <ItemStyle HorizontalAlign="Center" Width="1%"/>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDownloadPackage" runat="server" Visible='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "AttachFileCount"))  > 0 %>'
                            OnClick="btnDownload_Click"
                            ImageUrl="~/Images/download.png" ToolTip="Download document package"
                            Style="cursor: pointer;" AlternateText="Download document"/> 
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                                            
                <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadDefault" HeaderTooltip="Download default document file">
                    <HeaderStyle Width="1%"/>
                    <ItemStyle HorizontalAlign="Center" Width="1%"/>
                    <ItemTemplate>
                        <a href='<%# DataBinder.Eval(Container.DataItem, "DefaultDoc") != null ? DataBinder.Eval(Container.DataItem, "DefaultDoc.FilePath") : string.Empty %>' target="_blank">
                            <asp:Image ID="ImageButton1" runat="server" ToolTip="Download default document file"
                                Visible='<%# DataBinder.Eval(Container.DataItem, "DefaultDoc") != null %>'
                                ImageUrl='<%# DataBinder.Eval(Container.DataItem, "DefaultDoc") != null ? DataBinder.Eval(Container.DataItem, "DefaultDoc.ExtensionIcon") : string.Empty %>' 
                                Style="cursor: pointer;" AlternateText="Download document"/> 
                        </a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                                            
                <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="AttachColumn" HeaderTooltip="Attach document files">
                    <HeaderStyle Width="1%"/>
                    <ItemStyle HorizontalAlign="Center" Width="1%"/>
                    <ItemTemplate>
                        <a href='javascript:ShowUploadForm(<%# DataBinder.Eval(Container.DataItem, "ID") %>)' style="text-decoration: none; color:blue">
                                
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/attach.png" ToolTip="Attach document files"
                                Style="cursor: pointer;" AlternateText="Attach document file" /> 
                        </a>

                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                                            
                <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="RevisionColumn" HeaderTooltip="Revision list">
                    <HeaderStyle Width="1%"/>
                    <ItemStyle HorizontalAlign="Center" Width="1%"/>
                    <ItemTemplate>
                        <a href='javascript:ShowRevisionForm(<%# DataBinder.Eval(Container.DataItem, "ID") %>)' style="text-decoration: none; color:blue">
                                
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/revision.png" ToolTip="Revision list"
                                Style="cursor: pointer;" AlternateText="Attach document file" /> 
                        </a>

                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <%--3--%>
                <telerik:GridTemplateColumn UniqueName="Index1" HeaderText="Name" 
                    DataField="Name" ShowFilterIcon="False" FilterControlWidth="97%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                    <HeaderStyle Width="10%" HorizontalAlign="Center"/>
                    <ItemStyle HorizontalAlign="Left" Width="10%"/>
                    <ItemTemplate>
                        <a href='javascript:ShowEditForm(<%# DataBinder.Eval(Container.DataItem, "ID") %>)' style="text-decoration: none; color:blue">
                            <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'></asp:Label>
                        </a>
                            <asp:Image ID="newicon" runat="server" ImageUrl="~/Images/new.png" Visible='<%# (DateTime.Now - Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "CreatedDate"))).TotalHours < 24 %>' /> 
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:HiddenField ID="Name" runat="server" Value='<%# Eval("Name") %>'/>
                                                    
                        <asp:TextBox ID="TextBox1" runat="server" Width="100%"></asp:TextBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                                            
                <telerik:GridBoundColumn DataField="Description" HeaderText="Description" UniqueName="Index2">
                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="RevName" HeaderText="Rev" UniqueName="Index3">
                    <HeaderStyle HorizontalAlign="Center" Width="3%" />
                    <ItemStyle HorizontalAlign="Center" Width="3%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor Name" UniqueName="Index4">
                    <HeaderStyle HorizontalAlign="Center" Width="8%" />
                    <ItemStyle HorizontalAlign="Left" Width="8%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="DrawingNumber" HeaderText="Drawing Number" UniqueName="Index5">
                    <HeaderStyle HorizontalAlign="Center" Width="6%" />
                    <ItemStyle HorizontalAlign="Left" Width="6%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="Year" HeaderText="Year" UniqueName="Index6">
                    <HeaderStyle HorizontalAlign="Center" Width="4%" />
                    <ItemStyle HorizontalAlign="Center" Width="4%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="PlantName" HeaderText="Plant" UniqueName="Index7">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="SystemName" HeaderText="System" UniqueName="Index8">
                    <HeaderStyle HorizontalAlign="Center" Width="8%" />
                    <ItemStyle HorizontalAlign="Left" Width="8%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="DisciplineName" HeaderText="Discipline" UniqueName="Index9">
                    <HeaderStyle HorizontalAlign="Center" Width="8%" />
                    <ItemStyle HorizontalAlign="Left" Width="8%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="DocumentTypeName" HeaderText="Document Type" UniqueName="Index10">
                    <HeaderStyle HorizontalAlign="Center" Width="8%" />
                    <ItemStyle HorizontalAlign="Left" Width="8%" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="TagTypeName" HeaderText="Tag Type" UniqueName="Index11">
                    <HeaderStyle HorizontalAlign="Center" Width="8%" />
                    <ItemStyle HorizontalAlign="Left" Width="8%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="ProjectName" HeaderText="Project" UniqueName="Index12">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="BlockName" HeaderText="Block" UniqueName="Index13">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="FieldName" HeaderText="Field" UniqueName="Index14">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="PlatformName" HeaderText="Platform" UniqueName="Index15">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="WellName" HeaderText="Platform" UniqueName="Index16">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="StartDate" HeaderText="Start Date" UniqueName="Index17" DataFormatString="{0:dd/MM/yyyy}">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                </telerik:GridBoundColumn>
                                            
                    <telerik:GridBoundColumn DataField="EndDate" HeaderText="End Date" UniqueName="Index18" DataFormatString="{0:dd/MM/yyyy}">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="NumberOfWork" HeaderText="No.Of Work" UniqueName="Index19">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="TagNo" HeaderText="Tag No" UniqueName="Index20">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="TagDes" HeaderText="Tag Des" UniqueName="Index21">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="Manufacturers" HeaderText="Manufacturers" UniqueName="Index22">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="SerialNo" HeaderText="Serial No" UniqueName="Index23">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="ModelNo" HeaderText="Model No" UniqueName="Index24">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="AssetNo" HeaderText="Asset No" UniqueName="Index25">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="TableOfContents" HeaderText="Table Of Contents" UniqueName="Index26">
                    <HeaderStyle HorizontalAlign="Center" Width="7%" />
                    <ItemStyle HorizontalAlign="Center" Width="7%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="PublishDate" HeaderText="Publish Date" UniqueName="Index27" DataFormatString="{0:dd/MM/yyyy}">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="FromName" HeaderText="From" UniqueName="Index28">
                    <HeaderStyle HorizontalAlign="Center" Width="4%" />
                    <ItemStyle HorizontalAlign="Center" Width="4%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="ToName" HeaderText="To" UniqueName="Index29">
                    <HeaderStyle HorizontalAlign="Center" Width="4%" />
                    <ItemStyle HorizontalAlign="Center" Width="4%" />
                </telerik:GridBoundColumn>
                                            
                <telerik:GridBoundColumn DataField="Signer" HeaderText="Signer" UniqueName="Index30">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                </telerik:GridBoundColumn>
                
                <telerik:GridBoundColumn DataField="Other" HeaderText="Other" UniqueName="Index31">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="RIGName" HeaderText="RIG" UniqueName="Index32">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="KindOfRepair" HeaderText="Kind of repair" UniqueName="Index33">
                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                </telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
        <ClientSettings Selecting-AllowRowSelect="true"  AllowColumnHide="True">
            <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
            <Selecting AllowRowSelect="true" />
            <ClientEvents  OnGridCreated="GetGridObject" />
            <ClientEvents OnRowContextMenu="RowContextMenu" OnRowClick="RowClick"></ClientEvents>
            <Scrolling AllowScroll="True" SaveScrollPosition="True" UseStaticHeaders="True" />
        </ClientSettings>
</telerik:RadGrid>
    <span style="display: none">
        
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
               <%-- <telerik:AjaxSetting AjaxControlID="btnClearSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2"/>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
                
                <telerik:AjaxSetting AjaxControlID="ddlCategory">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2" UpdatePanelRenderMode="Block" UpdatePanelHeight="50%" />
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="rtvProject">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2" UpdatePanelRenderMode="Block" UpdatePanelHeight="100%"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rtvBlock">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2" UpdatePanelRenderMode="Block" UpdatePanelHeight="100%"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rtvField">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2" UpdatePanelRenderMode="Block" UpdatePanelHeight="100%"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rtvPlatform">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2" UpdatePanelRenderMode="Block" UpdatePanelHeight="100%"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </span>
    <telerik:RadContextMenu ID="radMenu" runat="server"
        EnableRoundedCorners="true" EnableShadows="true" OnClientItemClicking="gridMenuClicking">
        <Items>
            <telerik:RadMenuItem Text="Attach document files" ImageUrl="~/Images/attach.png" Value="AttachDoc">
            </telerik:RadMenuItem>
            <telerik:RadMenuItem Text="Revision list" ImageUrl="~/Images/revision.png" Value="RevisionHistory">
            </telerik:RadMenuItem>
        </Items>
    </telerik:RadContextMenu>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Document Information"
                VisibleStatusbar="false" Height="690" Width="650" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="RevisionDialog" runat="server" Title="Revision list" OnClientClose="refreshGrid"
                VisibleStatusbar="false" Height="700" Width="1250" MinHeight="700" MinWidth="1250" MaxHeight="700" MaxWidth="1250" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="SendMail" runat="server" Title="Send mail"
                VisibleStatusbar="false" Height="560" Width="992" MinHeight="560" MinWidth="992" MaxHeight="560" MaxWidth="992" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            
            <telerik:RadWindow ID="AttachDoc" runat="server" Title="Attach document files"
                VisibleStatusbar="false" Height="600" Width="500" MinHeight="600" MinWidth="500" MaxHeight="600" MaxWidth="500" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
            

        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
    <asp:HiddenField runat="server" ID="FolderContextMenuAction"/>
    <asp:HiddenField runat="server" ID="lblFolderId"/>
    <asp:HiddenField runat="server" ID="lblDocId"/>
    <input type="hidden" id="radGridClickedRowIndex" name="radGridClickedRowIndex"/>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="Scripts/jquery-1.7.1.js"></script>
        <script type="text/javascript">
            
            var radDocuments;

            function refreshGrid() {
                var masterTable = $find("<%=grdDocument.ClientID%>").get_masterTableView();
                masterTable.rebind();
            }

            function GetGridObject(sender, eventArgs) {
                radDocuments = sender;
            }

            function ShowUploadForm(id) {
                var owd = $find("<%=AttachDoc.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Document/UploadDragDrop.aspx?docId=" + id, "AttachDoc");
            }

            function ShowRevisionForm(id) {
                var owd = $find("<%=RevisionDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Document/RevisionHistory.aspx?docId=" + id, "RevisionDialog");
            }

            function RowClick(sender, eventArgs) {
                var Id = eventArgs.getDataKeyValue("ID");
                document.getElementById("<%= lblDocId.ClientID %>").value = Id;
            }

            function gridMenuClicking(sender, args) {
                var itemValue = args.get_item().get_value();
                var docId = document.getElementById("<%= lblDocId.ClientID %>").value;

                

                switch (itemValue) {
                    case "RevisionHistory":
                        var owd = $find("<%=RevisionDialog.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/RevisionHistory.aspx?docId=" + docId , "RevisionDialog");
                        break;
                    case "AttachDoc":
                        var owd = $find("<%=AttachDoc.ClientID %>");
                        owd.Show();
                        owd.setUrl("Controls/Document/UploadDragDrop.aspx?docId=" + docId, "AttachDoc");
                        break;
                }
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

                ajaxManager = $find("<%=ajaxCustomer.ClientID %>");
            }

            function onRequestStart(sender, args) {
                if (args.get_eventTarget().indexOf("btnDownloadPackage") >= 0) {
                    args.set_enableAjax(false);
                }
            }


            function ShowEditForm(id, folId) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Document/DocumentInfoEditForm.aspx?docId=" + id + "&folId=" + folId, "CustomerDialog");
                
                // window.radopen("Controls/Customers/CustomerEditForm.aspx?patientId=" + id, "CustomerDialog");
                //  return false;
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

            function RowDblClick(sender, eventArgs) {
                var owd = $find("<%=CustomerDialog.ClientID %>");
                owd.Show();
                owd.setUrl("Controls/Customers/ViewCustomerDetails.aspx?docId=" + eventArgs.getDataKeyValue("Id"), "CustomerDialog");
                // window.radopen("Controls/Customers/ViewCustomerDetails.aspx?patientId=" + eventArgs.getDataKeyValue("Id"), "CustomerDialog");
            }
            
            function onNodeClicking(sender, args) {
                var folderValue = args.get_node().get_value();
                document.getElementById("<%= lblFolderId.ClientID %>").value = folderValue;
            }

            function OnClientButtonClicking(sender, args) {
                var button = args.get_item();
                var strText = button.get_text();
                var strValue = button.get_value();

                var grid = $find("<%= grdDocument.ClientID %>");
                var customerId = null;
                var customerName = "";

                //if (grid.get_masterTableView().get_selectedItems().length > 0) {
                //    var selectedRow = grid.get_masterTableView().get_selectedItems()[0];
                //    customerId = selectedRow.getDataKeyValue("Id");
                //    //customerName = selectedRow.Items["FullName"]; 
                //    //customerName = grid.get_masterTableView().getCellByColumnUniqueName(selectedRow, "FullName").innerHTML;
                //}


                
                 
                if (strText == "Documents") {

                    var selectedFolder = document.getElementById("<%= lblFolderId.ClientID %>").value;
                    if (selectedFolder == "") {
                        alert("Please choice one folder to create new document");
                        return false;
                    }

                    var owd = $find("<%=CustomerDialog.ClientID %>");
                    owd.Show();
                    owd.setUrl("Controls/Document/DocumentInfoEditForm.aspx?folId=" + selectedFolder, "CustomerDialog");

                }

                if (strText == "Thêm mới") {
                    return ShowInsertForm();
                }
                else if (strText == "Import dữ liệu") {
                    return ShowImportForm();
                }
                else if (strText == "Dữ liệu thô") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_1_" + customerId);
                }
                else if (strText == "Tiềm năng") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_2_" + customerId);
                }
                else if (strText == "Chưa liên hệ được") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_3_" + customerId);
                }
                else if (strText == "Không tiềm năng") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_4_" + customerId);
                }
                else if (strText == "Thông tin sai") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_5_" + customerId);
                }
                else if (strText == "Liên hệ tư vấn") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_6_" + customerId);
                }
                else if (strText == "Hẹn tư vấn") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_7_" + customerId);
                }
                else if (strText == "Đã sử dụng dịch vụ") {
                    if (customerId == null) return;
                    if (confirm("Ban có chắc chắn chuyển trạng khách hàng [" + customerName + "] sang trạng thái [" + strText + "] không ?") == false) return;
                    ajaxManager.ajaxRequest("ChangeStatus_8_" + customerId);
                }
                else {
                    var commandName = args.get_item().get_commandName();
                    if (commandName == "doSearch") {
                        var searchTextBox = sender.findButtonByCommandName("searchText").findControl("txtSearch");
                        if (searchButton.get_value() == "clear") {
                            searchTextBox.set_value("");
                            searchButton.set_imageUrl("images/search.gif");
                            searchButton.set_value("search");
                        }

                        performSearch(searchTextBox);
                    } else if (commandName == "reply") {
                        window.radopen(null, "Edit");
                    }
                }
            }
            
            function performSearch(searchTextBox) {
                if (searchTextBox.get_value()) {
                    searchButton.set_imageUrl("images/clear.gif");
                    searchButton.set_value("clear");
                }

                ajaxManager.ajaxRequest(searchTextBox.get_value());
            }
            function onTabSelecting(sender, args) {
                if (args.get_tab().get_pageViewID()) {
                    args.get_tab().set_postBack(false);
                }
            }
            

            function RowContextMenu(sender, eventArgs) {
                var menu = $find("<%=radMenu.ClientID %>");
                var evt = eventArgs.get_domEvent();

                if (evt.target.tagName == "INPUT" || evt.target.tagName == "A") {
                    return;
                }

                var index = eventArgs.get_itemIndexHierarchical();
                document.getElementById("radGridClickedRowIndex").value = index;
                
                var Id = eventArgs.getDataKeyValue("ID");
                document.getElementById("<%= lblDocId.ClientID %>").value = Id;

                sender.get_masterTableView().selectItem(sender.get_masterTableView().get_dataItems()[index].get_element(), true);

                menu.show(evt);

                evt.cancelBubble = true;
                evt.returnValue = false;

                if (evt.stopPropagation) {
                    evt.stopPropagation();
                    evt.preventDefault();
                }
            }
            
            function ddlSystemDDHandler(sender, eventArgs) {
                var tree = sender.get_items().getItem(0).findControl("rtvSystem");
                var selectedNode = tree.get_selectedNode();
                if (selectedNode) {
                    selectedNode.scrollIntoView();
                }
            }
            function rtvSystemNodeClicking(sender, args) {
                var comboBox = $find("<%= ddlSystem.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }

                function ddlPlantDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvPlant");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvPlantNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlPlant.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }

                function ddlDisciplineDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvDiscipline");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvDisciplineNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlDiscipline.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }

                function ddlDocumentTypeDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvDocumentType");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvDocumentTypeNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlDocumentType.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }

                function ddlTagTypeDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvTagType");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvTagTypeNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlTagType.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }

                function ddlProjectDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvProject");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvProjectNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlProject.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }

                function ddlBlockDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvBlock");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvBlockNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlBlock.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }

                function ddlFieldDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvField");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvFieldNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlField.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }

                function ddlPlatformDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvPlatform");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvPlatformNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlPlatform.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }

                function ddlWellDDHandler(sender, eventArgs) {
                    var tree = sender.get_items().getItem(0).findControl("rtvWell");
                    var selectedNode = tree.get_selectedNode();
                    if (selectedNode) {
                        selectedNode.scrollIntoView();
                    }
                }
                function rtvWellNodeClicking(sender, args) {
                    var comboBox = $find("<%= ddlWell.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }
                
            function ddlRIGDDHandler(sender, eventArgs) {
                var tree = sender.get_items().getItem(0).findControl("rtvRIG");
                var selectedNode = tree.get_selectedNode();
                if (selectedNode) {
                    selectedNode.scrollIntoView();
                }
            }
            function rtvRIGNodeClicking(sender, args) {
                var comboBox = $find("<%= ddlRIG.ClientID %>");

                    var node = args.get_node();

                    comboBox.set_text(node.get_text());

                    comboBox.trackChanges();
                    comboBox.get_items().getItem(0).set_text(node.get_text());
                    comboBox.get_items().getItem(0).set_value(node.get_value());
                    comboBox.commitChanges();
                    comboBox.hideDropDown();
                }
        /* ]]> */
        </script>
    </telerik:RadCodeBlock>
</asp:Content>

<%--Tan.Le Remove here--%>
<%--<uc1:List runat="server" ID="CustomerList"/>--%>
<%-- <div id="EDMsCustomers" runat="server" />--%>
