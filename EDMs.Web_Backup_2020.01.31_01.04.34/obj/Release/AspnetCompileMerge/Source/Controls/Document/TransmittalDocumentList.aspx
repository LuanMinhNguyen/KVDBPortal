<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransmittalDocumentList.aspx.cs" Inherits="EDMs.Web.Controls.Document.TransmittalDocumentList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../Content/styles.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.7.1.js" type="text/javascript"></script>
    <style type="text/css">
        
        html, body, form {
	        overflow:auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="content">
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="True" Height="545"
                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                GridLines="None" 
                Skin="Windows7"
                OnNeedDataSource="grdDocument_OnNeedDataSource"
                OnDetailTableDataBind="grdDocument_DetailTableDataBind" 
                OnDeleteCommand="grdDocument_DeleteCommand" 
                PageSize="30" Style="outline: none">
                <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%"><%--<GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldAlias="-" FieldName="DocumentTypeName" FormatString="{0:D}"
                                            HeaderValueSeparator=""></telerik:GridGroupByField>
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="DocumentTypeName" SortOrder="Ascending" ></telerik:GridGroupByField>
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>  --%>  
                        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="REVISION DETAILS" Name="RevisionDetails"
                                    HeaderStyle-HorizontalAlign="Center"/>
                            <telerik:GridColumnGroup HeaderText="OUTGOING TRANSMITTAL" Name="OutgoingTrans"
                                    HeaderStyle-HorizontalAlign="Center"/>
                            <telerik:GridColumnGroup HeaderText="INCOMING TRANSMITTAL" Name="IncomingTrans"
                                    HeaderStyle-HorizontalAlign="Center"/>
                            <telerik:GridColumnGroup HeaderText="ICA REVIEW DETAILS" Name="ICAReviews"
                                    HeaderStyle-HorizontalAlign="Center"/>
                        </ColumnGroups>
                                    
                        <DetailTables>
                            <telerik:GridTableView DataKeyNames="ID" Name="DocDetail" Width="100%"
                                AllowPaging="True" PageSize="10">
                                <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                                <Columns>
                                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                                    <HeaderStyle Width="3%" />
                                    <ItemStyle HorizontalAlign="Center" Width="3%"/>
                                    <ItemTemplate>
                                        <a href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' 
                                            download='<%# DataBinder.Eval(Container.DataItem, "FileName") %>' target="_blank">
                                            <asp:Image ID="Image1" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ExtensionIcon") %>'
                                                Style="cursor: pointer;" ToolTip="Download document" /> 
                                        </a>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                            
                                <telerik:GridBoundColumn DataField="FileName" HeaderText="File name" UniqueName="FileName">
                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="Description" HeaderText="Notes" UniqueName="Description">
                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="CmtResFrom" HeaderText="From" UniqueName="CmtResFrom">
                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="CmtResTo" HeaderText="To" UniqueName="CmtResTo">
                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="AttachTypeName" HeaderText="Type" UniqueName="AttachTypeName">
                                    <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                    <ItemStyle HorizontalAlign="Left" Width="9%" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="CreatedByUser" HeaderText="Upload by" UniqueName="CreatedByUser">
                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Upload time" UniqueName="CreatedDate"
                                    DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                    <HeaderStyle HorizontalAlign="Center" Width="16%" />
                                    <ItemStyle HorizontalAlign="Left" Width="16%" />
                                </telerik:GridBoundColumn>
                                </Columns>
                            </telerik:GridTableView>
                        </DetailTables>

                        <Columns>
                                <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
                                <telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" HeaderTooltip="Delete document"
                                            ConfirmText="Do you want to move this document out of transmittal?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                                            <HeaderStyle Width="2%" />
                                            <ItemStyle HorizontalAlign="Center" Width="2%"  />
                                </telerik:GridButtonColumn>
                                <telerik:GridTemplateColumn HeaderText="No." Groupable="False">
                                    <HeaderStyle HorizontalAlign="Center" Width="2%" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="2%"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSoTT" runat="server" Text='<%# grdDocument.CurrentPageIndex * grdDocument.PageSize + grdDocument.Items.Count+1 %>'>
                                        </asp:Label>
                                      
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <%--<telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="~/Images/edit.png" 
                                    UpdateImageUrl="~/Images/ok.png" CancelImageUrl="~/Images/delete.png" UniqueName="EditColumn">
                                    <HeaderStyle HorizontalAlign="Center" Width="2%"  />
                                    <ItemStyle HorizontalAlign="Center" Width="2%"/>
                                </telerik:GridEditCommandColumn>--%>
                            <%--<telerik:GridButtonColumn UniqueName="DeleteColumn" CommandName="Delete" HeaderTooltip="Delete document"
                                    ConfirmText="Do you want to delete document?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                                    <HeaderStyle Width="1%" />
                                        <ItemStyle HorizontalAlign="Center" Width="1%"  />
                                </telerik:GridButtonColumn>
                                --%>
                            <telerik:GridTemplateColumn HeaderText="DOC. No." UniqueName="DocNo">
                                <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                <ItemStyle HorizontalAlign="Left" Width="8%" />
                                <ItemTemplate>
                                    <%# Eval("DocNo") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="DocNo" runat="server" Value='<%# Eval("DocNo") %>'/>
                                    <asp:Label runat="server" ID="lbldocNo"></asp:Label>
                                    <%--<asp:TextBox ID="txtDocNo" runat="server" Width="100%"></asp:TextBox>--%>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                                            
                            <telerik:GridTemplateColumn HeaderText="DOC. Title" UniqueName="DocTitle">
                                <HeaderStyle HorizontalAlign="Center" Width="14%" />
                                <ItemStyle HorizontalAlign="Left" Width="14%" />
                                <ItemTemplate>
                                    <%# Eval("DocTitle") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="DocTitle" runat="server" Value='<%# Eval("DocTitle") %>'/>
                                    <asp:TextBox ID="txtDocTitle" runat="server" Width="98%"></asp:TextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                            <telerik:GridTemplateColumn HeaderText="Deparment" UniqueName="DeparmentName">
                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                <ItemTemplate>
                                    <%# Eval("DeparmentName") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="DeparmentName" runat="server" Value='<%# Eval("DeparmentName") %>'/>
                                    <asp:TextBox ID="txtDeparment" runat="server" Width="98%"></asp:TextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                            <telerik:GridTemplateColumn HeaderText="Start" UniqueName="StartDate">
                                <HeaderStyle HorizontalAlign="Center" Width="4%" />
                                <ItemStyle HorizontalAlign="Center" Width="4%"/>
                                <ItemTemplate>
                                    <%# Eval("StartDate","{0:dd/MM/yyyy}") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="StartDate" runat="server" Value='<%# Eval("StartDate") %>'/>
                                    <telerik:RadDatePicker ID="txtStartDate"  Width="98%" 
                                        runat="server" Skin="Windows7" ShowPopupOnFocus="True"
                                        PopupDirection="BottomRight">
                                        <DateInput ID="txtStartDateInput" runat="server" 
                                            DateFormat="dd/MM/yyyy" ShowButton="False"/>
                                    </telerik:RadDatePicker>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                            <telerik:GridTemplateColumn HeaderText="Planed" UniqueName="PlanedDate">
                                <HeaderStyle HorizontalAlign="Center" Width="4%" />
                                <ItemStyle HorizontalAlign="Center" Width="4%"/>
                                <ItemTemplate>
                                    <%# Eval("PlanedDate","{0:dd/MM/yyyy}") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="PlanedDate" runat="server" Value='<%# Eval("PlanedDate") %>'/>
                                    <telerik:RadDatePicker ID="txtPlanedDate" Width="98%" 
                                        runat="server" Skin="Windows7" ShowPopupOnFocus="True"
                                        PopupDirection="BottomRight">
                                        <DateInput ID="txtPlanedDateInput" runat="server" 
                                            DateFormat="dd/MM/yyyy" ShowButton="False"/>
                                    </telerik:RadDatePicker>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                            <telerik:GridTemplateColumn HeaderText="Rev." UniqueName="Rev"
                                ColumnGroupName="RevisionDetails">
                                <HeaderStyle HorizontalAlign="Center" Width="3%" />
                                <ItemStyle HorizontalAlign="Center" Width="3%"/>
                                <ItemTemplate>
                                    <%# Eval("RevisionName") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="RevisionId" runat="server" 
                                        Value='<%# Eval("RevisionId") %>'/>
                                    <telerik:RadComboBox ID="RadComboBox1" runat="server"  
                                        DropDownWidth="100px" MaxHeight="300px" Width="98%" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                            <telerik:GridTemplateColumn HeaderText="Planed" UniqueName="RevisionPlanedDate"
                                ColumnGroupName="RevisionDetails">
                                <HeaderStyle HorizontalAlign="Center" Width="4%" />
                                <ItemStyle HorizontalAlign="Center" Width="4%"/>
                                <ItemTemplate>
                                    <%# Eval("RevisionPlanedDate","{0:dd/MM/yyyy}") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="RevisionPlanedDate" runat="server" Value='<%# Eval("RevisionPlanedDate") %>'/>
                                    <telerik:RadDatePicker ID="txtRevisionPlanedDate"  Width="98%" 
                                        runat="server" Skin="Windows7" ShowPopupOnFocus="True"
                                        PopupDirection="BottomRight">
                                        <DateInput ID="DateInput1" runat="server" 
                                            DateFormat="dd/MM/yyyy" ShowButton="False"/>
                                    </telerik:RadDatePicker>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Actual" UniqueName="RevisionActualDate"
                                ColumnGroupName="RevisionDetails">
                                <HeaderStyle HorizontalAlign="Center" Width="4%" />
                                <ItemStyle HorizontalAlign="Center" Width="4%"/>
                                <ItemTemplate>
                                    <%# Eval("RevisionActualDate","{0:dd/MM/yyyy}") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="RevisionActualDate" runat="server" Value='<%# Eval("RevisionActualDate") %>'/>
                                    <telerik:RadDatePicker ID="txtRevisionActualDate" Width="98%" 
                                        runat="server" Skin="Windows7" ShowPopupOnFocus="True"
                                        PopupDirection="BottomRight">
                                        <DateInput ID="DateInput2" runat="server" 
                                            DateFormat="dd/MM/yyyy" ShowButton="False"/>
                                    </telerik:RadDatePicker>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                            <telerik:GridTemplateColumn HeaderText="Comment Code" UniqueName="RevisionCommentCode"
                                ColumnGroupName="RevisionDetails">
                                <HeaderStyle HorizontalAlign="Center" Width="4%" />
                                <ItemStyle HorizontalAlign="Left" Width="4%" />
                                <ItemTemplate>
                                    <%# Eval("RevisionCommentCode") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="RevisionCommentCode" runat="server" Value='<%# Eval("DeparmentName") %>'/>
                                    <asp:TextBox ID="txtRevisionCommentCode" runat="server" Width="98%"></asp:TextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                                        
                            <telerik:GridTemplateColumn HeaderText="Complete - %" UniqueName="Complete">
                                <HeaderStyle HorizontalAlign="Center" Width="3%" />
                                <ItemStyle HorizontalAlign="Center" Width="3%" />
                                <ItemTemplate>
                                    <%# Eval("Complete") + "%"%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="Complete" runat="server" Value='<%# Eval("Complete") %>'/>
                                    <telerik:radnumerictextbox type="Percent" id="txtComplete" 
                                        runat="server" Width="98%">
                                        <NumberFormat DecimalDigits="0"></NumberFormat>
                                    </telerik:radnumerictextbox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Weight - %" UniqueName="Weight">
                                <HeaderStyle HorizontalAlign="Center" Width="3%" />
                                <ItemStyle HorizontalAlign="Center" Width="3%" />
                                <ItemTemplate>
                                    <%# Eval("Weight") + "%"%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="Weight" runat="server" Value='<%# Eval("Weight") %>'/>
                                    <telerik:radnumerictextbox type="Percent" id="txtWeight" 
                                        runat="server" Width="98%">
                                        <NumberFormat DecimalDigits="0"></NumberFormat>
                                    </telerik:radnumerictextbox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings >
                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="True" SaveScrollPosition="True" UseStaticHeaders="True" />
                </ClientSettings>
            </telerik:RadGrid>
        </div>
        
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer">
            <ClientEvents OnRequestStart="onRequestStart"></ClientEvents>
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ajaxCustomer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDocument" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadCodeBlock runat="server">
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
