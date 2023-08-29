<%@ Control Language="C#" %>
<div id="InsertLink" class="reInsertLinkWrapper" style="display: none;">
	<table border="0" cellpadding="0" cellspacing="0" class="reControlsLayout">
		<tr>
			<td>
				<label for="LinkURL" class="reDialogLabelLight">
					<span>[linkurl]</span>
				</label>
			</td>
			<td class="reControlCellLight">
				<input type="text" id="LinkUrl" class="rfdIgnore" />
			</td>
		</tr>
		<tr id="texTextBoxParentNode">
			<td>
				<label for="LinkText" class="reDialogLabelLight">
					<span>[linktext]</span>
				</label>
			</td>
			<td class="reControlCellLight">
				<input type="text" id="LinkText" class="rfdIgnore" />
			</td>
		</tr>
		<tr>
			<td>
				<label for="LinkTargetCombo" class="reDialogLabelLight">
					<span>[linktarget]</span>
				</label>
			</td>
			<td class="reControlCellLight">
				<select id="LinkTargetCombo" class="rfdIgnore">
					<optgroup label="PresetTargets">
						<option value="_none">[none]</option>
						<option value="_self">[targetself]</option>
						<option value="_blank">[targetblank]</option>
						<option value="_parent">[targetparent]</option>
						<option value="_top">[targettop]</option>
						<option value="_search">[targetsearch]</option>
						<option value="_media">[targetmedia]</option>
					</optgroup>
					<optgroup label="CustomTargets">
						<option value="_custom">[addcustomtarget]</option>
					</optgroup>
				</select>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<table border="0" cellpadding="0" cellspacing="0" class="reConfirmCancelButtonsTblLight">
					<tr>
						<td class="reAllPropertiesLight">
							<button type="button" id="lmlAllProperties">
							[allproperties]
							</button>
						</td>
						<td>
							<button type="button" id="lmlInsertBtn">
							[ok]
							</button>
						</td>
						<td>
							<button type="button" id="lmlCancelBtn">
							[cancel]
							</button>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</div>
