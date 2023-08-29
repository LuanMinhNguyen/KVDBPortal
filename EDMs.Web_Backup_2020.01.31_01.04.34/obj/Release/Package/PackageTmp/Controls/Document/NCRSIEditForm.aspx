<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NCRSIEditForm.aspx.cs" Inherits="EDMs.Web.Controls.Document.NCRSIEditForm" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        html, body, form {
	        overflow:visible;
        }
        .RadComboBoxDropDown_Windows7 {
            width: 416PX !important;
            height: 300px !important;
        }
        .RadComboBoxDropDown .rcbScroll {
            height: 299px !important;
        }
        .RadComboBoxDropDown_Default .rcbHovered {
               background-color: #46A3D3;
               color: #fff;
           }
           .RadComboBoxDropDown .rcbItem, .RadComboBoxDropDown .rcbHovered, .RadComboBoxDropDown .rcbDisabled, .RadComboBoxDropDown .rcbLoading, .RadComboBoxDropDown .rcbCheckAllItems, .RadComboBoxDropDown .rcbCheckAllItemsHovered {
               margin: 0 0px;
           }
           .RadComboBox .rcbInputCell .rcbInput {
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
            width: 399PX;
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
               width: 399PX !important;
               border-bottom: none !important;
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
            width: 399PX;
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
            /*letter-spacing: 0.1em;*/
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
            /*letter-spacing: 0.1em;*/
            line-height: 1.2;
            margin: 0.5em auto 0.6em;
            padding: 0;
            text-align: left;
            text-decoration: none;
            display: block;
        }

        #ddlParent_Input {
            width: 384px !important;
        }

        #rtvRefDocNo {
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

            #example .demo-container .rgEditForm textarea {
            margin: 3px 0;
        }
 
        .demo-container .rgEditForm .ruFakeInput {
            width: 212px;
        }
        .RadUpload .ruFileWrap.ruStyled
        {
            padding-top:7px;
        }
 
        div.RadGrid .rgEditForm
        {
            padding-top:5px;
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
                
            <dl class="accordion">
                <dt style="width: 100%;">
                    <span>Main Detail</span>
                </dt>
            </dl>
                <li style="width: 1000px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Number
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtNumber"  runat="server" Style="width: 400px;" CssClass="min25Percent qlcbFormRequired" ReadOnly="True"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 1000px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Subject
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtSubject"  runat="server" Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
            
            <li style="width: 1000px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Reference
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtReference"  runat="server" Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <dl class="accordion">
                    <dt style="width: 100%;">
                        <span>Classification</span>
                    </dt>
                </dl>
                
                <li style="width: 1000px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Project Code
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtProjectCode"  runat="server" Style="width: 400px;" CssClass="min25Percent qlcbFormRequired"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 1000px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Type
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416px" OnSelectedIndexChanged="ddlGroup_OnSelectedIndexChanged" AutoPostBack="True">
                                <Items>
                                    <asp:ListItem Value="1" Text="NCR"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="SI"></asp:ListItem>
                                </Items>
                            </asp:DropDownList>
                        </div>
                        
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>

                <li style="width: 1000px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Group
                            </span>
                        </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
                           <asp:DropDownList ID="ddlGroup" runat="server" CssClass="min25Percent qlcbFormRequired" Width="416px" OnSelectedIndexChanged="ddlGroup_OnSelectedIndexChanged" AutoPostBack="True"/>
                        </div>
                        
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                <li style="width: 1000px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Year
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <telerik:radnumerictextbox type="Number" id="txtYear" runat="server" Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="70px" CssClass="min25Percent qlcbFormRequired">
                                <NumberFormat DecimalDigits="0"></NumberFormat>
                            </telerik:radnumerictextbox>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
            <li style="width: 1000px;">
                <div>
                    <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                        <span style="color: #2E5689; text-align: right; ">Sequential Number
                        </span>
                    </label>
                    <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                        <asp:TextBox ID="txtSequentialNumber" MaxLength="4"  runat="server" Style="width: 400px;" CssClass="min25Percent" OnTextChanged="txtSequentialNumber_OnTextChanged" AutoPostBack="true"/>
                    </div>
                </div>
                <div style="clear: both; font-size: 0;"></div>
            </li>

                <dl class="accordion">
                    <dt style="width: 100%;">
                        <span>Details</span>
                    </dt>
                </dl>

            <li style="width: 1000px;">
                <div>
                    <label style="width: 130px; float: left; padding-top: 3px; padding-right: 10px; text-align: right;">
                        <span style="color: #2E5689; text-align: right; ">Issued Date
                        </span>
                    </label>
                    <div style="float: left;  padding-top: 5px;"  class="qlcbFormItem">
                        <telerik:RadDatePicker ID="txtIssueDate"  runat="server" 
                                               ShowPopupOnFocus="True" CssClass="qlcbFormRequired">
                            <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormRequired" />
                        </telerik:RadDatePicker>
                    </div>
                </div>
                <div style="clear: both; font-size: 0;"></div>
            </li>
            
            

                  <li style="width: 1000px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Description Of Non-Conformance / Site Instruction
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtDescription"  runat="server" Style="width: 700px;" CssClass="min25Percent" TextMode="MultiLine" Rows="6"/>
                             <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="RadGrid1" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" GridLines="None"  Skin="Windows7"
            OnItemCreated="RadGrid1_ItemCreated" PageSize="3" OnInsertCommand="RadGrid1_InsertCommand"
            OnNeedDataSource="RadGrid1_NeedDataSource" OnDeleteCommand="RadGrid1_DeleteCommand"
            OnUpdateCommand="RadGrid1_UpdateCommand" OnItemCommand="RadGrid1_ItemCommand">
            <PagerStyle Mode="NumericPages" AlwaysVisible="true"></PagerStyle>
            <MasterTableView Width="700px" CommandItemDisplay="Top" DataKeyNames="ID">
                <Columns>
                    <telerik:GridEditCommandColumn>
                        <HeaderStyle Width="36px"></HeaderStyle>
                    </telerik:GridEditCommandColumn>
                  
                    <telerik:GridTemplateColumn HeaderText="Description" UniqueName="Description" DataField="Description">
                       
                        <ItemTemplate>
                            <asp:Label ID="lblDescription" runat="server" Text='<%# TrimDescription(Eval("Description") as string) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="float:left;" class="qlcbFormItem">
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txbDescription" CssClass="min25Percent" Width="400px" runat="server" TextMode="MultiLine"
                                Text='<%# Eval("Description") %>' Height="150px">
                            </telerik:RadTextBox></div>
                        </EditItemTemplate>
                        <ItemStyle VerticalAlign="Top"></ItemStyle>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="FilePath" HeaderText="Image" UniqueName="Upload">
                          <HeaderStyle HorizontalAlign="Center" Width="200" />
                                <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Image ID="Image1" runat="server" Width="150px" Height="100px" ImageUrl='<%#Eval("FilePath") %>'/>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="AsyncUpload1" OnClientFileUploaded="OnClientFileUploaded"
                                AllowedFileExtensions=".JPG,.JPGE,.PNG,.GIF,.jpg,.jpeg,.png,.gif" MaxFileSize="5242880" OnFileUploaded="AsyncUpload1_FileUploaded">
                            </telerik:RadAsyncUpload>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridButtonColumn Text="Delete" CommandName="Delete" ButtonType="ImageButton">
                        <HeaderStyle Width="36px"></HeaderStyle>
                    </telerik:GridButtonColumn>
                </Columns>
                <EditFormSettings>
                    <EditColumn ButtonType="ImageButton">
                    </EditColumn>
                </EditFormSettings>
                <PagerStyle AlwaysVisible="True"></PagerStyle>
            </MasterTableView>
        </telerik:RadGrid>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
            
                <li style="width: 1000px;" id="liActionTaken" visible="false" runat="server">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Action Taken
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtActionTaken"  runat="server" Style="width: 700px;" CssClass="min25Percent" TextMode="MultiLine" Enabled="false" Rows="8"/>
             <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="grActiontaken" Enabled="false"
                 AllowPaging="True" AllowSorting="True" 
            AutoGenerateColumns="False" GridLines="None"  Skin="Windows7"
            OnItemCreated="grActiontaken_ItemCreated" PageSize="3" OnInsertCommand="grActiontaken_InsertCommand"
            OnNeedDataSource="grActiontaken_NeedDataSource" OnDeleteCommand="grActiontaken_DeleteCommand"
            OnUpdateCommand="grActiontaken_UpdateCommand" OnItemCommand="grActiontaken_ItemCommand">
            <PagerStyle Mode="NumericPages" AlwaysVisible="true"></PagerStyle>
            <MasterTableView Width="700px" CommandItemDisplay="Top" DataKeyNames="ID">
                <Columns>
                    <telerik:GridEditCommandColumn>
                        <HeaderStyle Width="36px"></HeaderStyle>
                    </telerik:GridEditCommandColumn>
                  
                    <telerik:GridTemplateColumn HeaderText="Description" UniqueName="Description" DataField="Description">
                       
                        <ItemTemplate>
                            <asp:Label ID="lblDescription" runat="server" Text='<%# TrimDescription(Eval("Description") as string) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="float:left;" class="qlcbFormItem">
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txbDescription" CssClass="min25Percent" Width="400px" runat="server" TextMode="MultiLine"
                                Text='<%# Eval("Description") %>' Height="150px">
                            </telerik:RadTextBox></div>
                        </EditItemTemplate>
                        <ItemStyle VerticalAlign="Top"></ItemStyle>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="FilePath" HeaderText="Image" UniqueName="Upload">
                          <HeaderStyle HorizontalAlign="Center" Width="200" />
                                <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Image ID="Image1" runat="server" Width="150px" Height="100px" ImageUrl='<%#Eval("FilePath") %>'/>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="AsyncUpload2" 
                                AllowedFileExtensions=".JPG,.JPGE,.PNG,.GIF,.jpg,.jpeg,.png,.gif" MaxFileSize="5242880">
                            </telerik:RadAsyncUpload>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridButtonColumn Text="Delete" CommandName="Delete" ButtonType="ImageButton">
                        <HeaderStyle Width="36px"></HeaderStyle>
                    </telerik:GridButtonColumn>
                </Columns>
                <EditFormSettings>
                    <EditColumn ButtonType="ImageButton">
                    </EditColumn>
                </EditFormSettings>
                <PagerStyle AlwaysVisible="True"></PagerStyle>
            </MasterTableView>
        </telerik:RadGrid>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
               
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Signed by PMC
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtSignedbyPMC"  runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Signed by Owner
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtSignedbyPMB"  runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
            <li style="width: 600px; display:none; ">
                <div>
                    <label style="width: 130px; float: left; padding-top: 3px; padding-right: 10px; text-align: right;">
                        <span style="color: #2E5689; text-align: right; ">Date of submission
                        </span>
                    </label>
                    <div style="float: left;  padding-top: 5px;"  class="qlcbFormItem">
                        <telerik:RadDatePicker ID="txtDateofsubmission"  runat="server" 
                                               ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                            <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                        </telerik:RadDatePicker>
                    </div>
                </div>
                <div style="clear: both; font-size: 0;"></div>
            </li>
                
                <li style="width: 600px;">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Status
                            </span>
                        </label>
                            <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                           <asp:DropDownList ID="ddlStatus" runat="server" CssClass="min25Percent qlcbFormRequired"  Width="416px">
                               <Items>
                                   <asp:ListItem Value="Opening" Text="Opening"/> 
                                   <asp:ListItem Value="Unclose" Text="Unclose"/> 
                                    <asp:ListItem Value="Closing" Text="Closing"/> 
                                   <asp:ListItem Value="Closed" Text="Closed"/>
                               </Items>
                           </asp:DropDownList>
                        </div>
                        
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px; ">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 3px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Closed Date
                            </span>
                        </label>
                        <div style="float: left;  padding-top: 5px;"  class="qlcbFormItem">
                            <telerik:RadDatePicker ID="txtClosedDate"  runat="server" 
                                ShowPopupOnFocus="True" CssClass="qlcbFormNonRequired">
                                <DateInput runat="server" DateFormat="dd/MM/yyyy" cssclass="qlcbFormNonRequired" />
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px; ">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Closed by PMC
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtClosedByPMC"  runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
                <li style="width: 600px; ">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Closed by Owner
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                            <asp:TextBox ID="txtClosedByPMB"  runat="server" Style="width: 400px;" CssClass="min25Percent"/>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                
            <li style="width: 600px;">
                <div>
                    <label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                        <span style="color: #2E5689; text-align: right; ">Note
                        </span>
                    </label>
                    <div style="float: left; padding-top: 5px;" class="qlcbFormItem">
                        <asp:TextBox ID="txtNote"  runat="server" Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
                    </div>
                </div>
                <div style="clear: both; font-size: 0;"></div>
            </li>
                
                <li style="width: 600px;" runat="server" id="liCancel" visible="false">
				<div>
					<label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
						<span style="color: #2E5689; text-align: right; ">
						</span>
					</label>
					<div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
						  <asp:CheckBox runat="server" ID="cbIsCancel" OnClick="checkchanged(this)" Text="Is Cancel"/>
					</div>
						
				</div>
				<div style="clear: both; font-size: 0;"></div>
			</li>
                <div class="qlcbFormItem" runat="server" ID="CreatedInfo" Visible="False">
                    <div class="dnnFormMessage dnnFormInfo">
                        <div class="dnnFormItem dnnFormHelp dnnClear">
                            <p class="dnnFormRequired" style="float: left;">
                                    <asp:Label ID="lblCreated" runat="server" ></asp:Label>
                                    <asp:Label ID="lblUpdated" runat="server" ></asp:Label>
                            </p>
                            <br />
                        </div>
                    </div>
                </div>
                <div>
                    <dl class="accordion" id="dlAddtachfile" runat="server">
                        <dt style="width: 100%;">
                            <span>Attach Files</span>
                        </dt>
                    </dl>
                 
                      <li id="LiContractor" runat="server" visible="false" style="width: 600px; padding-top: 10px; padding-bottom: 10px; text-align: center">
                   <telerik:RadButton ID="btnExportNCR" runat="server" Text="Generate NCR/SI" Width="120px"  OnClick="btnExportNCR_Click"  style="text-align: center" SingleClick="True" SingleClickText="Processing">
                        <Icon PrimaryIconUrl="../../Images/comment.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                   <telerik:RadButton ID="btncancel" runat="server"  Text="Upload File Action Taken" Width="160px" style="text-align: center" OnClientClicked="OpenUploadActiontaken">
                        <Icon PrimaryIconUrl="../../Images/attach.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>

                </li>
                 
                <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="False"
                    AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                    GridLines="None" Skin="Windows7" Height="200"
                    OnNeedDataSource="grdDocument_OnNeedDataSource" 
                    Style="outline: none; overflow: hidden !important;">
                    <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%">
                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <Columns>
                            <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                                <HeaderStyle Width="30" />
                                <ItemStyle HorizontalAlign="Center"  />
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
                                <HeaderStyle HorizontalAlign="Center" Width="250" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                    
                            <telerik:GridBoundColumn DataField="TypeName" HeaderText="Type" UniqueName="TypeName">
                                <HeaderStyle HorizontalAlign="Center" Width="130" />
                                <ItemStyle HorizontalAlign="Left"/>
                            </telerik:GridBoundColumn>
                                
                            <telerik:GridBoundColumn DataField="CreatedByName" HeaderText="Upload by" UniqueName="CreatedByName">
                                <HeaderStyle HorizontalAlign="Center" Width="150" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                                
                            <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Upload time" UniqueName="CreatedDate"
                                DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                <HeaderStyle HorizontalAlign="Center" Width="80" />
                                <ItemStyle HorizontalAlign="Center"/>
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
            </div>
                <li style="width: 600px; padding-top: 10px; padding-bottom: 3px; text-align: center">
                    <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"  Width="70px" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                   <%-- <telerik:RadButton ID="btncancel" runat="server" Visible="false" Text="Upload File Action Taken" Width="140px" style="text-align: center" OnClientClicked="OpenUploadActiontaken">
                        <Icon PrimaryIconUrl="../../Images/Cancel.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>--%>

                </li>

                 <li style="width: 400px;" Runat="server" ID="blockError" Visible="False">
                    <div>
                        <label style="width: 60px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: red; text-align: right; ">Warning:
                            </span>
                        </label>
                        <div style="float: left; padding-top: 5px; " class="qlcbFormItem">
                            <asp:Label runat="server" ID="lblError" Width="300px"></asp:Label>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
            </ul>
        </div>
        <asp:HiddenField runat="server" ID="docUploadedIsExist"/>
        <asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
        <asp:HiddenField runat="server" ID="lbNCRSIId" />
        
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="Windows7"/>
        <telerik:RadAjaxManager runat="Server" ID="ajaxDocument">
              <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
            <AjaxSettings> 
                <telerik:AjaxSetting AjaxControlID="ajaxDocument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlGroup">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtNumber" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="ddlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtNumber" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <telerik:AjaxSetting AjaxControlID="txtSequentialNumber">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtNumber" LoadingPanelID="RadAjaxLoadingPanel2"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID ="btnExportNCR">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                 <telerik:AjaxSetting AjaxControlID ="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                 <telerik:AjaxSetting AjaxControlID ="grActiontaken">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grActiontaken"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlSystem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlSubSystem"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                   
                <telerik:AjaxSetting AjaxControlID="ddlSystem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtOriginator"/>
                         <telerik:AjaxUpdatedControl ControlID="txtOriginating"/>
                         <telerik:AjaxUpdatedControl ControlID="txtReceiving"/>
                         <telerik:AjaxUpdatedControl ControlID="txtDocumenType"/>
                         <telerik:AjaxUpdatedControl ControlID="txtDiscipline"/>
                         <telerik:AjaxUpdatedControl ControlID="txtMaterial"/>
                         <telerik:AjaxUpdatedControl ControlID="txtWork"/>
                         <telerik:AjaxUpdatedControl ControlID="txtDrawing"/>
                         <telerik:AjaxUpdatedControl ControlID="txtEquipmentTagNumber"/>
                         <telerik:AjaxUpdatedControl ControlID="txtDepartmentcode"/>
                         <telerik:AjaxUpdatedControl ControlID="txtMRSequenceNo"/>
                         <telerik:AjaxUpdatedControl ControlID="txtDocumentSequenceNo"/>
                         <telerik:AjaxUpdatedControl ControlID="txtsheetno"/>
                         <telerik:AjaxUpdatedControl ControlID="txtPlant"/>
                         <telerik:AjaxUpdatedControl ControlID="txtArea"/>
                         <telerik:AjaxUpdatedControl ControlID="txtUnit"/>
                         <telerik:AjaxUpdatedControl ControlID="txtDocumentTitle"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
          <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
              <telerik:RadWindow ID="ETRMAttachFile" runat="server" Title="NCR/SI Attach Files"
                VisibleStatusbar="false" Height="600" Width="700" IconUrl="~/Images/excelfileattach.png"
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Reload, Close">
            </telerik:RadWindow>
            </Windows>
              </telerik:RadWindowManager>
        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                var ajaxManager;
                var uploadedFilesCount = 0;
                var isEditMode;
                function validateRadUpload(source, e) {
                    // When the RadGrid is in Edit mode the user is not obliged to upload file.
                    if (isEditMode == null || isEditMode == undefined) {
                        e.IsValid = false;

                        if (uploadedFilesCount > 0) {
                            e.IsValid = true;
                        }
                    }
                    isEditMode = null;
                }

                function OnClientFileUploaded(sender, eventArgs) {
                    uploadedFilesCount++;
                }
                function OnClientFilesUploaded(sender, args) {
                    var name = args.get_fileName();
                    //document.getElementById("txtName").value = name;
                    $find('<%=ajaxDocument.ClientID %>').ajaxRequest();
                }

                function pageLoad() {
                    ajaxManager = $find("<%=ajaxDocument.ClientID %>");
                }
                function checkchanged(obj) {
                    if (obj.checked) {
                        if (confirm("Are you want to cancel this NCR/SI?") == false) {
                            obj.checked = false;
                        }
                    }
                }
                function fileUploading(sender, args) {
                    var name = args.get_fileName();
                    document.getElementById("txtName").value = name;
                    
                    ajaxManager.ajaxRequest("CheckFileName$" + name);
                }
                function onRequestStart(sender, args) {
                    //alert(args.get_eventTarget()); //|| args.get_eventTarget().indexOf("ajaxCustomer") >= 0
                    if (args.get_eventTarget().indexOf("ExportTo") >= 0 || args.get_eventTarget().indexOf("btnDownloadPackage") >= 0 || args.get_eventTarget().indexOf("ajaxCustomer") >= 0) {
                        args.set_enableAjax(false);
                    }
                }
                
                function OpenUploadActiontaken() {
                    var objId=document.getElementById("<%=lbNCRSIId.ClientID %>").value;
                     var owd = $find("<%=ETRMAttachFile.ClientID %>");
                owd.setSize(800, document.documentElement.offsetHeight);
                owd.Show();
                owd.setUrl("NCRSIAttachDocument.aspx?objId=" + objId, "ETRMAttachFile");
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
