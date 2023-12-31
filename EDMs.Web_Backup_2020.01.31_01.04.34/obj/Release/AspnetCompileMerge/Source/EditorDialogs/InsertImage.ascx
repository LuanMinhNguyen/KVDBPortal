<%@ Control Language="C#" %>
<div id="InsertImage" class="reInsertImageWrapper" style="display: none;">
	<table cellspacing="0" cellpadding="0" border="0" class="reControlsLayout">
		<tr>
			<td style="vertical-align: middle;">
				<label class="reDialogLabelLight" for="ImageSrc">
					<span>[imagesrc]</span>
				</label>
			</td>
			<td class="reControlCellLight">
				<input type="text" id="ImageSrc" class="rfdIgnore" />
			</td>
		</tr>
		<tr>
			<td>
				<label class="reDialogLabelLight" for="ImageAlt">
					<span>[imagealttext]</span>
				</label>
			</td>
			<td class="reControlCellLight">
				<input type="text" id="ImageAlt" class="rfdIgnore" />
			</td>
		</tr>
		<tr>
			<td colspan="2" class="reImgPropertyControlCell">
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td>
							<label class="reDialogLabelLight" for="ImageWidth">
								<span>[width]</span>
							</label>
						</td>
						<td>
							<input type="text" id="ImageWidth" class="rfdIgnore" />&nbsp;&nbsp;px
						</td>
						<td>
							<label class="reDialogLabelLight" for="ImageHeight">
								<span>[height]</span>
							</label>
						</td>
						<td>
							<input type="text" id="ImageHeight" class="rfdIgnore" />&nbsp;&nbsp;px
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<table border="0" cellpadding="0" cellspacing="0" class="reConfirmCancelButtonsTblLight">
					<tr>
						<td class="reAllPropertiesLight" style="padding-left:3px;">
							<button type="button" id="iplAllProperties">
								[allproperties]
							</button>
						</td>
						<td>
							<button type="button" id="iplInsertBtn">
								[ok]
							</button>
						</td>
						<td>
							<button type="button" id="iplCancelBtn">
								[cancel]
							</button>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</div>
