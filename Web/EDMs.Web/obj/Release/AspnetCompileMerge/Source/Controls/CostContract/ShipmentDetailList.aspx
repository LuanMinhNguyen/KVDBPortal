<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShipmentDetailList.aspx.cs" Inherits="EDMs.Web.Controls.CostContract.ShipmentDetailList" EnableViewState="true" %>

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
			width: 250PX !important;
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
			width: 250PX;
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
			   width: 250PX !important;
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
			width: 250PX;
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
		  <dl class="accordion">
				<dt style="width: 100%;">
					<span>Detail</span>
				</dt>
			</dl>
		<div style="width: 100%" runat="server" ID="EditContent">
		 
		<div style="width: 100%; height: 100%" runat="server" ID="divContent">
			 <table>
				<tr>
					<td style="width:50%; vertical-align: top;">
			<ul style="list-style-type: none">
				<li style="width: 600px;">
					<div>
						<label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
							<span style="color: #2E5689; text-align: right; ">Shipment No.
							</span>
						</label>
						<div style="float: left; padding-top: 5px;" class="qlcbFormItem">
							<asp:TextBox ID="txtNumber" Enabled="false" runat="server" Style="width: 250px;" CssClass="min25Percent " ReadOnly="True"/>
						</div>
					</div>
					<div style="clear: both; font-size: 0;"></div>
				</li>
				  <li style="width: 600px;">
					<div>
						<label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
							<span style="color: #2E5689; text-align: right; ">KKS Code
							</span>
						</label>
							<div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
						   <telerik:RadComboBox ID="ddlKKS" runat="server"  AllowCustomText="true" filter="Contains"  AutoCompleteSeparator="False"  MarkFirstMatch="true" CssClass="min25Percent " Width="416px"/>
						</div>
						
					</div>
					<div style="clear: both; font-size: 0;"></div>
				</li>
				</ul>
				 </td>
					 <td style="width:50%; vertical-align: top;">
						   <ul style="list-style-type: none">
				 <li style="width: 600px;">
					<div>
						<label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
							<span style="color: #2E5689; text-align: right; ">Equipment No.
							</span>
						</label>
							<div style="float: left; padding-top: 5px;" class="qlcbFormItem" >
						   <telerik:RadComboBox ID="ddlEquipment" runat="server" CssClass="min25Percent"  Width="416PX" CheckBoxes="false" Skin="Windows7" AllowCustomText="true" filter="Contains"  AutoCompleteSeparator="False"  MarkFirstMatch="true"/>
						</div>
						
					</div>
					<div style="clear: both; font-size: 0;"></div>
				</li>
				<li style="width: 600px;">
					<div>
						<label style="width: 130px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
							<span style="color: #2E5689; text-align: right; ">Quality
							</span>
						</label>
						<div style="float: left; padding-top: 5px;" class="qlcbFormItem">
							<telerik:radnumerictextbox type="Currency" id="txtQuality" runat="server" 
								Style=" min-width: 0px !important; border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;" Width="150px" CssClass="min25Percent">
								<NumberFormat DecimalDigits="0" GroupSizes="3" DecimalSeparator="." GroupSeparator="," NegativePattern="n" PositivePattern="n"></NumberFormat>
							</telerik:radnumerictextbox>
						</div>
					</div>
					<div style="clear: both; font-size: 0;"></div>
				</li>
			  
					</ul>
				</td>
					</tr>
					</table>
			<div style="width:100%;">
				   <ul style="list-style-type: none">
				<li style="width: 600px; padding-top: 5px; padding-bottom: 3px; text-align: center">
					<telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"  Width="70px" style="text-align: center">
						<Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
					</telerik:RadButton>
					<telerik:RadButton ID="btncancel" runat="server" Text="Clear" Width="70px" style="text-align: center"
						OnClick="btncancel_Click">
						<Icon PrimaryIconUrl="../../Images/Cancel.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
					</telerik:RadButton>

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
		</div>
			 </div>
		<telerik:RadGrid AllowCustomPaging="False" AllowPaging="True" AllowSorting="True"  RenderMode="Lightweight"
								AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None"  Skin="Windows7"  
								Height="300" ID="grdDocument"  AllowFilteringByColumn="True" AllowMultiRowSelection="True"
								OnNeedDataSource="grdDocument_OnNeedDataSource"
								OnItemCommand="grdDocument_OnItemCommand"
								 OnDeleteCommand="grdDocument_DeleteCommand"
								PageSize="100" runat="server" Style="outline: none" Width="100%">
								<SortingSettings SortedBackColor="#FFF6D6"></SortingSettings>
								<GroupingSettings CaseSensitive="False"></GroupingSettings>
								<MasterTableView AllowMultiColumnSorting="false"
									ClientDataKeyNames="ID" DataKeyNames="ID" CommandItemDisplay="Top" 
									EditMode="InPlace" Font-Size="8pt">
								   
									<CommandItemSettings  ShowAddNewRecordButton="false" RefreshText="Refresh Data" ShowExportToExcelButton="false"/>
									<PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
									<HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
								   
									<Columns>
										<telerik:GridBoundColumn DataField="ID" UniqueName="ID" Display="False" />
									   
										<telerik:GridTemplateColumn HeaderText="No." Groupable="False" AllowFiltering="false">
											<HeaderStyle HorizontalAlign="Center" Width="30" VerticalAlign="Middle"></HeaderStyle>
											<ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
											<ItemTemplate>
												<asp:Label ID="lblSoTT" runat="server" Text='<%# grdDocument.CurrentPageIndex * grdDocument.PageSize + grdDocument.Items.Count+1 %>'>
												</asp:Label>
									  
											</ItemTemplate>
										</telerik:GridTemplateColumn>

										<telerik:GridButtonColumn UniqueName="EditColumn" CommandName="EditCmd" ButtonType="ImageButton" ImageUrl="~/Images/edit.png">
											<HeaderStyle Width="24px" />
											<ItemStyle HorizontalAlign="Center"  />
										</telerik:GridButtonColumn>
											<telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" HeaderTooltip="Delete document"
										ConfirmText="Do you want to delete document?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
										<HeaderStyle Width="25" />
										<ItemStyle HorizontalAlign="Center" Width="25"  />
									</telerik:GridButtonColumn>

									<telerik:GridTemplateColumn HeaderText="KKS Code" UniqueName="KKSCode" DataField="KKSCode"
										 ShowFilterIcon="False" FilterControlWidth="97%" 
										AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
										<HeaderStyle HorizontalAlign="Center" Width="80" />
										<ItemStyle HorizontalAlign="Center" Width="80" />
										<ItemTemplate>
											 <%# Eval("KKSCode") %>
										</ItemTemplate>
									</telerik:GridTemplateColumn>
										  <telerik:GridTemplateColumn HeaderText="Equipment No" UniqueName="EquipmentNumber"
										ShowFilterIcon="False" FilterControlWidth="97%"  DataField="EquipmentNumber"
										AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
										<HeaderStyle HorizontalAlign="Center" Width="150" />
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<%# Eval("EquipmentNumber") %>
										</ItemTemplate>
									</telerik:GridTemplateColumn>
								   
										 <telerik:GridTemplateColumn HeaderText="Quantity" UniqueName="Quantity"
										ShowFilterIcon="False" FilterControlWidth="97%"  DataField="Quantity"
										AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
										<HeaderStyle HorizontalAlign="Center" Width="80" />
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<%# Eval("Quantity") %>
										</ItemTemplate>
									</telerik:GridTemplateColumn>
									 <telerik:GridTemplateColumn HeaderText="Create By" UniqueName="CreatedByName" DataField="CreatedByName"
										ShowFilterIcon="False" FilterControlWidth="97%" 
										AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
										<HeaderStyle HorizontalAlign="Center" Width="120" />
										<ItemStyle HorizontalAlign="Left" />
										<ItemTemplate>
											<%# Eval("CreatedByName") %>
										</ItemTemplate>
									 </telerik:GridTemplateColumn> 

								</Columns>
							</MasterTableView>
							<ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
							<%--<ClientEvents OnRowContextMenu="RowContextMenu" OnRowClick="RowClick"></ClientEvents>--%>
							<Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="500" UseStaticHeaders="True" />
							</ClientSettings>
						</telerik:RadGrid>

		  <dl class="accordion">
				<dt style="width: 100%;">
					<span>Document</span>
				</dt>
			</dl>
		 <div style="width: 100%" runat="server" id="dIVUploadControl">
			<ul style="list-style-type: none">
				<li style="width: 100%;" Runat="server" ID="UploadControl">
					<div>
						<label style="width: 110px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
							<span style="color: #2E5689; text-align: right; ">Upload multiple file
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
				<li style="width: 600px; text-align: center">
					<telerik:RadButton ID="btnUploadfile" runat="server" Text="Save" OnClick="btnUploadfile_Click" Width="70px" style="text-align: center">
						<Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
					</telerik:RadButton>
				</li>
			</ul>
		</div>
		<div style="width: 100%; height:300px;margin:3px; margin-right:3px; margin-bottom:3px;" runat="server">
		<telerik:RadGrid ID="grdDocShipment" runat="server" AllowPaging="False" AllowAutomaticDeletes="True"
			AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
			GridLines="None" Skin="Windows7" Height="100%"
			OnNeedDataSource="grdDocShipment_NeedDataSource"
			 OnItemDeleted="grdDocShipment_ItemDeleted"
			OnBatchEditCommand="grdDocShipment_BatchEditCommand"
			PageSize="10" Style="outline: none; overflow: hidden !important;">
			<MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%" CommandItemDisplay="TopAndBottom" EditMode="Batch">
				<CommandItemSettings ShowAddNewRecordButton="False"></CommandItemSettings>
				<BatchEditingSettings OpenEditingEvent="Click" EditType="Row"></BatchEditingSettings>
				<PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Contents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
				<HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
				<Columns>
					<telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
					<telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" HeaderTooltip="Delete document"
										ConfirmText="Do you want to delete document?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
										<HeaderStyle Width="25" />
										<ItemStyle HorizontalAlign="Center" Width="25"  />
									</telerik:GridButtonColumn>

								<telerik:GridTemplateColumn AllowFiltering="false" ReadOnly="True" UniqueName="DownloadColumn">
									<HeaderStyle Width="30" />
									<ItemStyle HorizontalAlign="Center" Width="30"/>
									<ItemTemplate>
										<a href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' 
											download='<%# DataBinder.Eval(Container.DataItem, "FileName") %>' target="_blank">
											<asp:Image ID="Image1" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ExtensionIcon") %>'
												Style="cursor: pointer;" ToolTip="Download document" /> 
										</a>
									</ItemTemplate>
								</telerik:GridTemplateColumn>
					<telerik:GridBoundColumn DataField="Number" HeaderText="Number" SortExpression="Number"
						UniqueName="Number" ReadOnly="True">
						<HeaderStyle Width="150px" HorizontalAlign="Center" />
						<ItemStyle HorizontalAlign="Left" Font-Bold="True"/>
					</telerik:GridBoundColumn>

					<telerik:GridTemplateColumn HeaderText="Description" SortExpression="Description" UniqueName="Description"
						DataField="Description" ReadOnly="False">
						<HeaderStyle Width="350" HorizontalAlign="Center" />
						<ItemStyle HorizontalAlign="Center"/>
						<ItemTemplate>
						<asp:Label runat="server" ID="lblDuration" Text='<%# Eval("Description") %>'></asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<span>
								<telerik:RadTextBox Width="100%" runat="server" ID="txtDescription"/>
							</span>
						</EditItemTemplate>
					</telerik:GridTemplateColumn>
					<telerik:GridTemplateColumn HeaderText="Category" UniqueName="TypeId" DataField="TypeId">
						<HeaderStyle Width="250" HorizontalAlign="Center" />
						<ItemStyle HorizontalAlign="Left"/>
						<ItemTemplate>
							<%# Eval("TypeName") %>
						</ItemTemplate>
						<EditItemTemplate>
							<telerik:RadComboBox runat="server" ID="ddlAcceptStep" Width="100%" Skin="Windows7">
								<Items>
									<telerik:RadComboBoxItem Value="-1" Text="" />
									<telerik:RadComboBoxItem Value="0" Text="Introduction Letter" />
									 <telerik:RadComboBoxItem Value="1" Text="Authorization Letter" />
									 <telerik:RadComboBoxItem Value="2" Text="Attached cargo list" />
									 <telerik:RadComboBoxItem Value="3" Text="Invoice" />
									 <telerik:RadComboBoxItem Value="4" Text="Packing List/ General Packing List/ Detail Packing List" />
									 <telerik:RadComboBoxItem Value="5" Text="Bill of Lading" />
									 <telerik:RadComboBoxItem Value="6" Text="Certificate of Origin (CO)" />
									 <telerik:RadComboBoxItem Value="7" Text="Certificate of Quality (CQ)" />
									 <telerik:RadComboBoxItem Value="8" Text="Certificate of Insurance (optional)g" />
									   <telerik:RadComboBoxItem Value="9" Text="Others" />
								</Items>
							</telerik:RadComboBox>
						</EditItemTemplate>
					</telerik:GridTemplateColumn>
								<telerik:GridBoundColumn DataField="CreatedDate" ReadOnly="true" HeaderText="Upload time" UniqueName="CreatedDate"
									DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
									<HeaderStyle HorizontalAlign="Center" Width="80" />
									<ItemStyle HorizontalAlign="Left" Width="80" />
								</telerik:GridBoundColumn>          
				</Columns>
			</MasterTableView>
			<ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True" >
				<Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
				<Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="200" UseStaticHeaders="True" />
				<ClientEvents />
			</ClientSettings>
		</telerik:RadGrid>
		</div>
		<asp:HiddenField runat="server" ID="docUploadedIsExist"/>
		<asp:HiddenField runat="server" ID="docIdUpdateUnIsLeaf"/>
		
		<telerik:RadAjaxManager runat="Server" ID="ajaxDocument" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
			<AjaxSettings> 
				<telerik:AjaxSetting AjaxControlID="ajaxDocument">
					<UpdatedControls>
						<telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
					</UpdatedControls>
				</telerik:AjaxSetting>
				
				 <telerik:AjaxSetting AjaxControlID="btnSave">
					<UpdatedControls>
						<telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
						 <telerik:AjaxUpdatedControl ControlID="ddlKKS" />                        
						<telerik:AjaxUpdatedControl ControlID="ddlEquipment"/>
						<telerik:AjaxUpdatedControl ControlID="txtQuality"/>
					</UpdatedControls>
				</telerik:AjaxSetting>
				
				<telerik:AjaxSetting AjaxControlID="btnClear">
					<UpdatedControls>
						  <telerik:AjaxUpdatedControl ControlID="ddlKKS" />                        
						<telerik:AjaxUpdatedControl ControlID="ddlEquipment"/>
						<telerik:AjaxUpdatedControl ControlID="txtQuality"/>
                          <telerik:AjaxUpdatedControl ControlID="divContent" />
					</UpdatedControls>
				</telerik:AjaxSetting>
				
				<telerik:AjaxSetting AjaxControlID="grdDocument">
					<UpdatedControls>
						<telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"/>
						  <telerik:AjaxUpdatedControl ControlID="ddlKKS" />                        
						<telerik:AjaxUpdatedControl ControlID="ddlEquipment"/>
						<telerik:AjaxUpdatedControl ControlID="txtQuality"/>
					</UpdatedControls>
				</telerik:AjaxSetting>
			</AjaxSettings>
		</telerik:RadAjaxManager>

		<telerik:RadScriptBlock runat="server">
			<script type="text/javascript">
				var ajaxManager;
			  

				function pageLoad() {
					ajaxManager = $find("<%=ajaxDocument.ClientID %>");
				}
				function StopPropagation(e) {
					if (!e) {
						e = window.event;
					}

					e.cancelBubble = true;
				}
				function DeleteDocument(ID) {
					if (confirm("Do you want to delete item?") == false) return;
					ajaxManager.ajaxRequest("Deletedocument_" + id);
				}
			  
			</script>
		</telerik:RadScriptBlock>
	</form>
</body>
</html>
