<%@ Control Language="C#" %>
<div id="InsertTableLight" class="reInsertTableLightWrapper" style="display: none;">
    <table cellspacing="0" cellpadding="0" border="0" class="reControlsLayout">
        <tr>
            <td colspan="2" class="reTablePropertyControlCell">
                <fieldset class="lightTable">
                    <legend>[layout] </legend>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <label class="reDialogLabelLight" for="TableColumns">
                                    <span class="short">[columns]</span>
                                </label>
                            </td>
                            <td>
                                <input type="text" id="Columns" class="rfdIgnore" />
                            </td>
                            <td>
                                <label class="reDialogLabelLight" for="CellPadding">
                                    <span class="short">[cellpadding]</span>
                                </label>
                            </td>
                            <td>
                                <input type="text" id="CellPadding" class="rfdIgnore" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="reDialogLabelLight" for="TableRows">
                                    <span class="short">[rows]</span>
                                </label>
                            </td>
                            <td>
                                <input type="text" id="Rows" class="rfdIgnore" />
                            </td>
                            <td>
                                <label class="reDialogLabelLight" for="CellSpacing">
                                    <span class="short">[cellspacing]</span>
                                </label>
                            </td>
                            <td>
                                <input type="text" id="CellSpacing" class="rfdIgnore" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="reDialogLabelLight" for="TableAlignment">
                                    <span>[alignment]</span>
                                </label>
                            </td>
                            <td>
                                <div class="reDialog reToolWrapper">
                                    <a id="AlignmentSelectorTable" title="AlignmentSelector" class="reTool reSplitButton"
                                        href="#"><span class="AlignmentSelector">&nbsp;</span><span class="split_arrow">&nbsp;</span></a>
                                </div>
                            </td>
                            <td>
                                <label class="reDialogLabelLight" for="BorderWidth">
                                    <span class="short">[border]</span>
                                </label>
                            </td>
                            <td>
                                <input type="text" id="BorderWidth" class="rfdIgnore" />&nbsp;&nbsp;px
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table border="0" cellpadding="0" cellspacing="0" class="reConfirmCancelButtonsTblLight">
                    <tr>
                        <td class="reAllPropertiesLight" style="padding-left: 3px;">
                            <button type="button" id="itlAllProperties">
                                [allproperties]
                            </button>
                        </td>
                        <td>
                            <button type="button" id="itlInsertBtn">
                                [ok]
                            </button>
                        </td>
                        <td>
                            <button type="button" id="itlCancelBtn">
                                [cancel]
                            </button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
