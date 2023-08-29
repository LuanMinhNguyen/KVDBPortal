<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VariationLogEditForm.aspx.cs" Inherits="EDMs.Web.Controls.Document.VariationLogEditForm" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
	
	<style type="text/css">
		html, body, form {
			overflow:visible;
		}
		.RadComboBox_ .RadComboBoxDropDown_Windows7 {
			width: 416PX !important;
			height: 300px !important;
		}
		.RadComboBox_ .RadComboBoxDropDown .rcbScroll {
			height: 299px !important;
		}
		.RadComboBox_.RadComboBoxDropDown_Default .rcbHovered {
			   background-color: #46A3D3;
			   color: #fff;
		   }
		   /*.RadComboBoxDropDown .rcbItem, .RadComboBoxDropDown .rcbHovered, .RadComboBoxDropDown .rcbDisabled, .RadComboBoxDropDown .rcbLoading, .RadComboBoxDropDown .rcbCheckAllItems, .RadComboBoxDropDown .rcbCheckAllItemsHovered {
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
		   }*/
		.RadComboBoxDropDown .rcbItem, .RadComboBoxDropDown .rcbHovered, .RadComboBoxDropDown .rcbDisabled, .RadComboBoxDropDown .rcbLoading, .RadComboBoxDropDown .rcbCheckAllItems, .RadComboBoxDropDown .rcbCheckAllItemsHovered {
			   margin: 0 0px;
		   }
		   .RadComboBox_ .rcbInputCell .rcbInput{
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
			width: 382PX;
		   }
		   .RadComboBox_ table td.rcbInputCell, .RadComboBox_ .rcbInputCell .rcbInput {
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
		   .RadComboBox_ {
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

		#rtvCCOrganisation {
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
				<li style="width: 600px;">
					<div>
						<label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
							<span style="color: #2E5689; text-align: right; ">Title
							</span>
						</label>
						<div style="float: left; padding-top: 5px;" class="qlcbFormItem">
							<asp:TextBox ID="txtTitle" TextMode="MultiLine" Rows="3" runat="server" Style="width: 400px;" CssClass="min25Percent" ReadOnly="false"/>
						</div>
					</div>
					<div style="clear: both; font-size: 0;"></div>
				</li>
				<li style="width: 600px;">
					<div>
						<label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
							<span style="color: #2E5689; text-align: right; ">Originating Organisation
							</span>
						</label>
						<div style="float: left; padding-top: 5px;" class="qlcbFormItem">
							<asp:DropDownList ID="ddlOriginatingOrganization" runat="server" CssClass="min25Percent" Width="416px"/>
						</div>
					</div>
					<div style="clear: both; font-size: 0;"></div>
				</li>
				<li style="width: 600px;">
					<div>
						<label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
							<span style="color: #2E5689; text-align: right; ">Instruction/ Proposal
							</span>
						</label>
						<div style="float: left; padding-top: 5px;" class="qlcbFormItem">
							<asp:TextBox ID="txtinstruction"  runat="server" Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" Rows="3" ReadOnly="true"/>
						</div>
					</div>
					<div style="clear: both; font-size: 0;"></div>
				</li>
			
			<li style="width: 600px;">
					<div>
						<label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
							<span style="color: #2E5689; text-align: right; ">Variation  Order
							</span>
						</label>
						<div style="float: left; padding-top: 5px;" class="qlcbFormItem">
							<asp:TextBox ID="txtorder"  runat="server" Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" ReadOnly="true" Rows="3"/>
						</div>
					</div>
					<div style="clear: both; font-size: 0;"></div>
				</li>
				<li style="width: 600px;">
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
					<li style="width: 600px;">
					<div>
						<label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
							<span style="color: #2E5689; text-align: right; ">System/ Equipment/ Material/…
							</span>
						</label>
						<div style="float: left; padding-top: 5px;" class="qlcbFormItem">
							<asp:TextBox ID="txtSystem"  runat="server" Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
						</div>
					</div>
					<div style="clear: both; font-size: 0;"></div>
				</li>
			
			

			<li style="width: 600px;">
					<div>
						<label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
							<span style="color: #2E5689; text-align: right; ">Contract Requirements
							</span>
						</label>
						<div style="float: left; padding-top: 5px;" class="qlcbFormItem">
							<asp:TextBox ID="txtContract"  runat="server" Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
						</div>
					</div>
					<div style="clear: both; font-size: 0;"></div>
				</li>
				<li style="width: 600px;">
					<div>
						<label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
							<span style="color: #2E5689; text-align: right; ">Description
							</span>
						</label>
						<div style="float: left; padding-top: 5px;" class="qlcbFormItem">
							<asp:TextBox ID="txtDescription"  runat="server" Style="width: 400px;" CssClass="min25Percent" TextMode="MultiLine" Rows="3"/>
						</div>
					</div>
					<div style="clear: both; font-size: 0;"></div>
				</li>
			<li style="width: 600px;">
				<div>
					<label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
						<span style="color: #2E5689; text-align: right; ">Cost impact
						</span>
					</label>
					<div style="float: left; padding-top: 5px;" class="qlcbFormItem">
						 <telerik:radnumerictextbox  NumberFormat-AllowRounding="false" type="Currency" id="txtCost" runat="server" 
                                Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent">
                                <NumberFormat DecimalDigits="2" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="-n USD" PositivePattern="n USD"></NumberFormat>
                            </telerik:radnumerictextbox>
					</div>
				</div>
				<div style="clear: both; font-size: 0;"></div>
			</li>
            <li style="width: 600px;">
				<div>
					<label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
						<span style="color: #2E5689; text-align: right; ">Schedule impact
						</span>
					</label>
					<div style="float: left; padding-top: 5px;" class="qlcbFormItem">
						 <telerik:radnumerictextbox type="Number" id="txtSchedule" runat="server" 
                                Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent">
                                <NumberFormat DecimalDigits="0" NegativePattern="-n Day" PositivePattern="n Day"></NumberFormat>
                            </telerik:radnumerictextbox>
					</div>
				</div>
				<div style="clear: both; font-size: 0;"></div>
			</li>
			<li style="width: 600px; ">
                    <div>
                        <label style="width: 130px; float: left; padding-top: 3px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">Issued Date
                            </span>
                        </label>
                        <div style="float: left;  padding-top: 5px;"  class="qlcbFormItem">
                            <telerik:RadDatePicker ID="txtIssuedDate"  runat="server" 
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
						<span style="color: #2E5689; text-align: right; ">Other Attachment
						</span>
					</label>
					<div style="float: left; padding-top: 5px;" class="qlcbFormItem">
						<asp:TextBox ID="txtAttachment" TextMode="MultiLine" Rows="3" runat="server" Style="width: 400px;" CssClass="min25Percent" ReadOnly="true"/>
					</div>
				</div>
				<div style="clear: both; font-size: 0;"></div>
			</li>
                            <li style="width: 600px;">
				<div>
					<label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
						<span style="color: #2E5689; text-align: right; ">Remark
						</span>
					</label>
					<div style="float: left; padding-top: 5px;" class="qlcbFormItem">
						<asp:TextBox ID="txtRemark" TextMode="MultiLine" Rows="3" runat="server" Style="width: 400px;" CssClass="min25Percent"/>
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
		           
				<dl class="accordion">
					<dt style="width: 100%;">
						<span>Attach Files</span>
					</dt>
				</dl>
                  <li style="width: 100%;" Runat="server" ID="UploadControl">
                    <div>
                        <label style="width: 135px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                            <span style="color: #2E5689; text-align: right; ">
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
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="min25Percent" Width="416px">
                                <Items>
                                    <asp:ListItem Value="1" Text="Instruction/ Proposal"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Order"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Other Files"></asp:ListItem>
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="clear: both; font-size: 0;"></div>
                </li>
                 <li style="width: 100%; text-align: center; padding-top: 10px">
                    <telerik:RadButton ID="btUpLoadFile" runat="server" Text="      Upload File" OnClick="btUpLoadFile_Click" Width="70px" style="text-align: center">
                        <Icon PrimaryIconUrl="../../Images/upload.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
                    </telerik:RadButton>
                </li>
                 <li style="width:  750px;">
                    <div style="padding-top: 5px;">
				<telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="False"
					AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
					GridLines="None" Skin="Windows7" Height="200"
					OnNeedDataSource="grdDocument_OnNeedDataSource" 
                     OnItemCommand="grdDocument_ItemCommand"
					PageSize="100" Style="outline: none; overflow: hidden !important;">
					<MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%" CommandItemDisplay="Top">
                         <CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="false"/>
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
                    <div style="clear: both; font-size: 0;"></div>
                </li>
				<li style="width: 600px; padding-top: 10px; padding-bottom: 3px; text-align: center">
					<telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"  Width="70px" style="text-align: center">
						<Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
					</telerik:RadButton>
					<%--<telerik:RadButton ID="btncancel" runat="server" Text="Cancel" Width="70px" style="text-align: center"
						OnClick="btncancel_Click">
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
		
	<telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" Skin="Windows7"/>
		<telerik:RadAjaxManager runat="Server" ID="ajaxDocument">
             <%--<ClientEvents OnResponseEnd ="EndResponse" />--%>
			<AjaxSettings> 
				<telerik:AjaxSetting AjaxControlID="ajaxDocument">
					<UpdatedControls>
						<telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
					</UpdatedControls>
				</telerik:AjaxSetting>

				   
				<telerik:AjaxSetting AjaxControlID="btUpLoadFile">
					<UpdatedControls>
						
						 <telerik:AjaxUpdatedControl ControlID="docuploader"/>
						 <telerik:AjaxUpdatedControl ControlID="txtinstruction"/>
						 <telerik:AjaxUpdatedControl ControlID="txtorder"/>
						 <telerik:AjaxUpdatedControl ControlID="txtAttachment"/>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument"/>
                        <telerik:AjaxUpdatedControl ControlID="divContent" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
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
				
				
				function checkchanged(obj) {
				    if (obj.checked) {
				        if (confirm("Are you want to cancel this CS?") == false) {
				            obj.checked = false;
				        }
				    }
				}
				
				function StopPropagation(e) {
					if (!e) {
						e = window.event;
					}

					e.cancelBubble = true;
				}
				function EndResponse(sender, args) {
				   var masterTable = $find("<%=grdDocument.ClientID%>").get_masterTableView();
                    masterTable.rebind();
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
