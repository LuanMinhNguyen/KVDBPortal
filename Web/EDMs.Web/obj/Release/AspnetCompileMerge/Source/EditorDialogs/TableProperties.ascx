<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor.DialogControls" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>
<script type="text/javascript">
	Type.registerNamespace("Telerik.Web.UI.Widgets");
	Telerik.Web.UI.Widgets.TableProperties = function (element)
	{
		Telerik.Web.UI.Widgets.TableProperties.initializeBase(this, [element]);

		this._clientParameters = null;
		this._editorRef = null;

		this._table = null;
		this._id = "";
		this._skinPath = "";
		this._tableWidth = "";
		this._tableHeight = "";

		//New StyleBuilder
		this._cssText = "";

		this._idInput = null;

		this._imageCaller = null;
		this._spinBoxWidth = null;
		this._spinBoxHeight = null;
		this._spinBoxCellSpacing = null;
		this._spinBoxCellPadding = null;
		this._colorPicker = null;
		this._classSelector = null;
		this._alignmentSelectorTable = null;

		this._tableBorderControl = null;
	}

	Telerik.Web.UI.Widgets.TableProperties.prototype = {
		initialize: function ()
		{
			Telerik.Web.UI.Widgets.TableProperties.callBaseMethod(this, "initialize");
			this._initializeChildren();
		},

		_initializeChildren: function ()
		{
			this._idInput = $get("TableId");
			this._spinBoxWidth = $find("SpinBoxWidth");
			this._spinBoxHeight = $find("SpinBoxHeight");
			this._spinBoxCellSpacing = $find("SpinBoxCellSpacing");
			this._spinBoxCellPadding = $find("SpinBoxCellSPadding");
			this._alignmentSelectorTable = $find("AlignmentSelectorTable");
			this._styleBuilder = $find("StyleBuilder");

			this._initializeChildEvents();
		},

		_initializeChildEvents: function ()
		{
			//NEW: StyleBuilder
			this._styleBuilder.add_valueSelected(Function.createDelegate(this, this._styleBuilderClicked));
		},

		//NEW: StyleBuilder
		_styleBuilderClicked: function (oTool, args)
		{
			//Editor object is supplied to all dialogs in the dialog parameters
			var editor = this._clientParameters.editor;
			var callbackFunction = Function.createDelegate(this, function (sender, args)
			{
				//Returned link - TODO: Use args.get_value() when the dialog returm methods are changed to return proper args object
				var resulTable = args.get_value();

				var cssText = resulTable.style.cssText;
				this._cssText = cssText;
				this._table.style.cssText = cssText;
				this._loadData();
			});

			//NEW: Use EditorCommandEventArgs
			var parameterTable = this._table.cloneNode(true);
			parameterTable = this._update(parameterTable);
			var argument = new Telerik.Web.UI.EditorCommandEventArgs("StyleBuilder", null, parameterTable);

			Telerik.Web.UI.Editor.CommandList._getDialogArguments(argument, "*", editor, "StyleBuilder");
			argument.fontNames = editor.get_fontNames();
			editor.showDialog("StyleBuilder", argument, callbackFunction);
		},

		clientInit: function (clientParameters)
		{
			this._clientParameters = clientParameters;
			if (clientParameters)
			{
				this._table = clientParameters.tableToModify;
			}

			this._editorRef = clientParameters.editor;

			this._imageCaller = $find("ImageCaller");
			this._imageCaller.set_editor(this._editorRef);

			this._colorPicker = $find("ColorPicker");
			if (this._colorPicker._popupVisible) this._colorPicker.hide();
			this._colorPicker.set_items(clientParameters.Colors);
			this._colorPicker.set_addCustomColorText(localization["AddCustomColor"]);

			this._classSelector = $find("ClassSelector");
			//localization
			this._classSelector.set_showText(true);
			this._classSelector.set_clearclasstext(localization["ClearClass"]);
			this._classSelector.set_text(localization["ApplyClass"]);
			this._classSelector.set_items(clientParameters.CssClasses);
			this._classSelector.add_valueSelected(this._cssValueSelected);

			this._tableBorderControl = $find("TableBorder");
			this._tableBorderControl.set_tableToModify(this._table);

			this._tableBorderControl.clientInit(clientParameters);

			this._loadData();
		},

		_cssValueSelected: function (oTool, args)
		{
			if (!oTool) return;
			var commandName = oTool.get_name();

			if ("ApplyClass" == commandName)
			{
				var attribValue = oTool.get_selectedItem();
				oTool.updateValue(attribValue);
			}
		},

		_setCssPropertyValue: function (element, cssProperty, value, attribute)
		{
			if (!element || !cssProperty) return;
			if (attribute) element.removeAttribute(attribute);
			element.style[cssProperty] = value;
		},

		_loadData: function ()
		{
			if (this._table)
			{
				this._cssText = this._table.style.cssText;

				this._idInput.value = this._table.getAttribute("id") ? this._table.getAttribute("id") : "";

				if (this._imageCaller)
				{
					var imagePath = this._table.style.backgroundImage;
					imagePath = (!imagePath) ? "" : imagePath.substring(4, imagePath.length - 1);
					this._imageCaller.set_value(imagePath);
				}

				this._tableWidth = this._table.style.width ? this._table.style.width : this._table.getAttribute("width");
				this._spinBoxWidth.set_value(this._tableWidth);

				this._tableHeight = this._table.style.height ? this._table.style.height : this._table.getAttribute("height");
				this._spinBoxHeight.set_value(this._tableHeight);

				this._spinBoxCellSpacing.set_value(this._table.cellSpacing);
				this._spinBoxCellPadding.set_value(this._table.cellPadding);


				var backgroundColor = this._table.style.backgroundColor;
				this._colorPicker.set_color(backgroundColor);

				if (this._table.className && this._table.className != '') this._classSelector.updateValue(this._table.className);

				this._alignmentSelectorTable.setTagName("TABLE");
				this._alignmentSelectorTable.updateValue(this._table.align);
			}
		},

		_updateTable: function ()
		{
			var table = this._update(this._table);
			var updatedTable = this._tableBorderControl._updateTarget();
			if (updatedTable) table = updatedTable;
			return table;
		},

		_update: function (table)
		{
			//New StyleBuilder
			table.style.cssText = this._cssText;

			var widthValue = this._spinBoxWidth.get_value();
			if (widthValue == "") table.removeAttribute("width", false);
			else if (!this._isValueValid(widthValue)) return null;

			table.removeAttribute("width", false);
			table.style.width = widthValue ? this._convertIntToPixel(widthValue) : ""; //SAFARI - px

			var heightValue = this._spinBoxHeight.get_value();
			if (heightValue == "") table.removeAttribute("height", false);
			else if (!this._isValueValid(heightValue)) return null;

			table.removeAttribute("height", false);
			table.style.height = heightValue ? this._convertIntToPixel(heightValue) : ""; //SAFARI - px

			this._setAttribValue("id", this._idInput.value);
			this._setAttribValue("align", this._alignmentSelectorTable.getAlign());

			var oSpacing = this._spinBoxCellSpacing.get_value();
			if (!isNaN(parseInt(oSpacing))) this._setAttribValue("cellSpacing", oSpacing >= 0 ? oSpacing : "", (oSpacing >= 0));

			var oPadding = this._spinBoxCellPadding.get_value();
			if (!isNaN(parseInt(oPadding))) this._setAttribValue("cellPadding", oPadding >= 0 ? oPadding : "", (oPadding >= 0));


			this._setCssPropertyValue(table, "backgroundColor", this._colorPicker.get_color(), "bgColor");

			var backgroundImage = this._imageCaller.get_value();
			backgroundImage = (backgroundImage) ? "url(" + backgroundImage + ")" : "";
			this._setCssPropertyValue(table, "backgroundImage", backgroundImage, "background");

			table.className = this._classSelector.get_value();

			return table;
		},

		_isValueValid: function (value)
		{
			var valueInt = parseFloat(value, 10);
			if (!isNaN(valueInt))
			{
				//NEW support for all css units
				var isValidPercent = (valueInt + '%' == value);
				var isValidPixel = (valueInt + 'px' == value.toLowerCase());
				var isValidEm = (valueInt + 'em' == value.toLowerCase());
				var isValidEx = (valueInt + 'ex' == value.toLowerCase());
				var isValidIn = (valueInt + 'in' == value.toLowerCase());
				var isValidCm = (valueInt + 'cm' == value.toLowerCase());
				var isValidMm = (valueInt + 'mm' == value.toLowerCase());
				var isValidPt = (valueInt + 'pt' == value.toLowerCase());
				var isValidPc = (valueInt + 'pc' == value.toLowerCase());
				var isValidNumber = (valueInt.toString() == value);

				if (isValidPercent || isValidPixel || isValidEm || isValidEx || isValidIn || isValidCm || isValidMm || isValidPt || isValidPc || isValidNumber)
				{
					return true;
				}
			}
			return false;
		},

		_convertIntToPixel: function (oVal)
		{
			var oNew = "" + oVal;

			if (oNew.indexOf("%") != -1)
			{
				return oNew;
			}
			else
			{
				var arMatch = oNew.match(/(em|ex|px|in|cm|mm|pt|pc)$/); //NEW support for all css units
				oNew = parseFloat(oNew);
				if (!isNaN(oNew))
				{
					oNew = (arMatch) ? oNew + arMatch[0] : oNew + "px"; //NEW support for all css units
					return oNew;
				}
			}
			return oVal;
		},

		_setAttribValue: function (attribName, oVal, forceVal)
		{
			if (oVal || (true == forceVal))//cellSpacing or cellPadding have by default 1 so we might want to set it to 0
			{
				this._table.setAttribute(attribName, oVal);
			}
			else
			{
				this._table.removeAttribute(attribName, false);
			}
		},

		set_tableToModify: function (value)
		{
			this._table = value;
			if (this._tableBorderControl) this._tableBorderControl.set_tableToModify(this._table);
		},

		dispose: function ()
		{
			this._clientParameters = null;
			this._editorRef = null;

			this._table = null;
			this._idInput = null;

			this._imageCaller = null;
			this._spinBoxWidth = null;
			this._spinBoxHeight = null;
			this._spinBoxCellSpacing = null;
			this._spinBoxCellPadding = null;
			this._colorPicker = null;
			this._classSelector = null;
			this._alignmentSelectorTable = null;

			this._tableBorderControl = null;

			Telerik.Web.UI.Widgets.TableProperties.callBaseMethod(this, "dispose");
		}
	}

	Telerik.Web.UI.Widgets.TableProperties.registerClass('Telerik.Web.UI.Widgets.TableProperties', Telerik.Web.UI.RadWebControl, Telerik.Web.IParameterConsumer);
</script>
<style type="text/css">
.reLabelCell
{
	width: 105px;
	padding: 3px;
	text-align: right;
}

.reControlCell
{
	padding: 3px;
}

</style>
<table border="0" cellpadding="0" cellspacing="0" style="margin-left: 4px;">
	<tr>
		<td style="padding-right: 4px;">
			<fieldset style="width:305px;" class="reTablePropertiesDimensions">
				<legend>
					<script type="text/javascript">document.write(localization["Dimensions"]);</script>
				</legend>
				<table border="0" cellpadding="0" cellspacing="0" style="width: 100%;" class="tableWizardTableProperties">
					<tr>
						<td class="reLabelCell">
							<label class="reDialogLabel"><script type="text/javascript">document.write(localization["Height"]);</script>:</label>
						</td>
						<td class="reControlCell">
							<tools:editorspinbox id="SpinBoxHeight" runat="server"></tools:editorspinbox>
							<script type="text/javascript">document.write(localization["PixelsOrPercents"]);</script>
						</td>
					</tr>
					<tr>
						<td class="reLabelCell">
							<label class="reDialogLabel" ><script type="text/javascript">document.write(localization["Width"]);</script>:</label>
						</td>
						<td class="reControlCell">
							<tools:editorspinbox id="SpinBoxWidth" runat="server"></tools:editorspinbox>
							<script type="text/javascript">document.write(localization["PixelsOrPercents"]);</script>
						</td>
					</tr>
				</table>
			</fieldset>
		</td>
		<td rowspan="2" style="vertical-align: top;">
			<fieldset style="height: 394px;width:375px;"  class="reTablePropertiesCssClass">
				<legend>
					<script type="text/javascript">document.write(localization["CssClass"]);</script>
					<script type="text/javascript">document.write(localization["Layout"]);</script>
				</legend>
				<telerik:TableBorder id="TableBorder" runat="server"></telerik:TableBorder>
			</fieldset>
		</td>
	</tr>
	<tr>
		<td>
			<fieldset style="width:305px;"  class="reTablePropertiesLayout">
				<legend>
					<script type="text/javascript">document.write(localization["Layout"]);</script>
				</legend>
				<table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
					<tr>
						<td class="reLabelCell"><label class="reDialogLabel" ><script type="text/javascript">document.write(localization["CellSpacing"]);</script>:</label></td>
						<td class="reControlCell"><tools:EditorSpinBox id="SpinBoxCellSpacing" runat="server"></tools:EditorSpinBox></td>
					</tr>
					<tr>
						<td class="reLabelCell"><label class="reDialogLabel" ><script type="text/javascript">document.write(localization["CellPadding"]);</script>:</label></td>
						<td class="reControlCell"><tools:EditorSpinBox id="SpinBoxCellSPadding" runat="server"></tools:EditorSpinBox></td>
					</tr>
					<tr>
						<td class="reLabelCell"><label class="reDialogLabel" ><script type="text/javascript">document.write(localization["Alignment"]);</script>:</label></td>
						<td class="reControlCell"><tools:AlignmentSelector id="AlignmentSelectorTable" runat="server"></tools:AlignmentSelector></td>
					</tr>
					<tr>
						<td class="reLabelCell"><label class="reDialogLabel" ><script type="text/javascript">document.write(localization["BackgroundColor"]);</script>:</label></td>
						<td class="reControlCell"><tools:ColorPicker id="ColorPicker" runat="server"></tools:ColorPicker></td>
					</tr>
					<tr>
						<td class="reLabelCell"><label class="reDialogLabel" ><script type="text/javascript">document.write(localization["StyleBuilder"]);</script>:</label></td>
						<td class="reControlCell"><tools:StandardButton runat="server" id="StyleBuilder" ToolName="StyleBuilder" /></td>
					</tr>
					<tr>
						<td class="reLabelCell"><label class="reDialogLabel" ><script type="text/javascript">document.write(localization["CssClass"])</script>:</label></td>
						<td class="reControlCell"><tools:ApplyClassDropDown id="ClassSelector" runat="server" Width="100px" /></td>
					</tr>
					<tr>
						<td class="reLabelCell"><label class="reDialogLabel" ><script type="text/javascript">document.write(localization["BackgroundImage"]);</script>:</label></td>
						<td class="reControlCell"><tools:ImageDialogCaller id="ImageCaller" runat="server"></tools:ImageDialogCaller></td>
					</tr>
					<tr>
						<td class="reLabelCell"><label class="reDialogLabel" ><script type="text/javascript">document.write(localization["Id"]);</script>:</label></td>
						<td class="reControlCell"><input type="text" id="TableId" value="" /></td>
					</tr>
				</table>
			</fieldset>
		</td>
	</tr>
</table>