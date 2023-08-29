<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransmittalDocumentList.aspx.cs" Inherits="EDMs.Web.Controls.Document.TransmittalDocumentList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../Content/styles.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.7.1.js" type="text/javascript"></script>
    <style type="text/css">
        html, body, form {
            overflow: auto;
        }
         #ContentPlaceHolder2_grdDocument_ctl00_Header{table-layout:auto !important;}
        #ContentPlaceHolder2_grdDocument_ctl00{table-layout:auto !important;}
        /*End*/
        @-moz-document url-prefix() {
            #ContentPlaceHolder2_grdDocument_ctl00_Header{table-layout:auto !important;}
            #ContentPlaceHolder2_grdDocument_ctl00{table-layout:auto !important;}
        }
        
        #RAD_SPLITTER_PANE_CONTENT_ctl00_rightPane {
            overflow: hidden !important;
        }
        .RadGrid_Windows7 .rgGroupHeader {
            line-height: 19px !important;
        }
        .rgExpandCol {
            width: 1% !important;
        }

        .rgGroupCol {
            width: 1% !important;
        }

        #ContentPlaceHolder2_grdDocument_ctl00_ctl02_ctl03_txtDate_popupButton {
            display: none;
        }

        .rgExpandCol {
            width: 1% !important;
        }

        .rgGroupCol {
            width: 1% !important;
        }

        .RadGrid .rgSelectedRow
        {
            background-image : none !important;
            background-color: coral !important;
        }

        .rgMasterTable .rgClipCells .rgClipCells {
            table-layout: auto !important;
        }

        .rgGroupCol
        {
            padding-left: 0 !important;
            padding-right: 0 !important;
            font-size:1px !important;
        }
 
        div.RadGrid .rgPager .rgAdvPart     
        {     
        display:none; 
        }
        /*#RAD_SPLITTER_PANE_CONTENT_ctl00_leftPane {
            width: 250px !important;
        }
        #RAD_SPLITTER_PANE_CONTENT_ctl00_topLeftPane {
            width: 250px !important;
        }*/

        .RadAjaxPanel {
            height: 100% !important;
        }

        .rpExpandHandle {
            display: none !important;
        }

        .RadGrid .rgRow td, .RadGrid .rgAltRow td, .RadGrid .rgEditRow td, .RadGrid .rgFooter td, .RadGrid .rgFilterRow td, .RadGrid .rgHeader, .RadGrid .rgResizeCol, .RadGrid .rgGroupHeader td {
            padding-left: 0px !important;
            padding-right: 0px !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 100%; height:100%;">
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="True" Height="99%"
                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                GridLines="None"
                Skin="Windows7"
                OnNeedDataSource="grdDocument_OnNeedDataSource"
                OnDetailTableDataBind="grdDocument_DetailTableDataBind"
                OnDeleteCommand="grdDocument_DeleteCommand"
                PageSize="30" Style="outline: none">
                <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%">
                    <%--<GroupByExpressions>
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
                        <telerik:GridColumnGroup HeaderText="First Issue Info" Name="FirstIssueInfo"
                            HeaderStyle-HorizontalAlign="Center" />
                        <telerik:GridColumnGroup HeaderText="Final Issue Info" Name="FinalIssueInfo"
                            HeaderStyle-HorizontalAlign="Center" />
                        <telerik:GridColumnGroup HeaderText="INCOMING TRANSMITTAL" Name="IncomingTrans"
                            HeaderStyle-HorizontalAlign="Center" />
                        <telerik:GridColumnGroup HeaderText="ICA REVIEW DETAILS" Name="ICAReviews"
                            HeaderStyle-HorizontalAlign="Center" />
                    </ColumnGroups>
                      <DetailTables>
                            <telerik:GridTableView DataKeyNames="ID" Name="DocDetail" Width="700px"
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
                                        <HeaderStyle HorizontalAlign="Center" Width="35%" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                    
                                    <telerik:GridBoundColumn DataField="TypeName" HeaderText="Type" UniqueName="TypeName">
                                        <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left"/>
                                    </telerik:GridBoundColumn>
                                
                                    <telerik:GridBoundColumn DataField="CreatedByName" HeaderText="Upload by" UniqueName="CreatedByName">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                
                                    <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Upload time" UniqueName="CreatedDate"
                                        DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridBoundColumn>
                                
                                    <telerik:GridBoundColumn DataField="FileSize" HeaderText="File size(Kb)" UniqueName="FileSize" DataFormatString="{0:0,0.00}">
                                        <HeaderStyle HorizontalAlign="Center" Width="13%" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </telerik:GridTableView>
                        </DetailTables>
                    <Columns>
                      <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />
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
                               
                                    <telerik:GridTemplateColumn HeaderText="Document Number" UniqueName="DocNo" DataField="DocNo"
                                         ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="160" />
                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                        <ItemTemplate>
                                             <%# Eval("DocNo") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                            
                                    <telerik:GridTemplateColumn HeaderText="Document Title" UniqueName="DocTitle" DataField="DocTitle"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="220" />
                                        <ItemStyle HorizontalAlign="Left" Width="220" />
                                        <ItemTemplate>
                                            <%# Eval("DocTitle") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="Revision" UniqueName="MajorRev"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="Revision"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="50" />
                                        <ItemStyle HorizontalAlign="Center" Width="50"/>
                                        <ItemTemplate>
                                            <%# Eval("Revision") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                    <telerik:GridTemplateColumn HeaderText="Confidentiality" UniqueName="ConfidentialityName"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="ConfidentialityName"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("ConfidentialityName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Document Type" UniqueName="DocTypeCode"
                                        ShowFilterIcon="False" FilterControlWidth="97%" Display="false"  DataField="DocTypeCode"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("DocTypeCode") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Originating Organisation" UniqueName="OriginatingOrganisationName"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="OriginatingOrganisationName"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("OriginatingOrganisationName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Receiving Organisation" UniqueName="ReceivingOrganisationName"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="ReceivingOrganisationName"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("ReceivingOrganisationName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Year" UniqueName="Year"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="Year"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="50" />
                                        <ItemStyle HorizontalAlign="Center" Width="50"/>
                                        <ItemTemplate>
                                            <%# Eval("Year") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="GroupCode" UniqueName="GroupCode"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  DataField="GroupCode"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="50" />
                                        <ItemStyle HorizontalAlign="Center" Width="50"/>
                                        <ItemTemplate>
                                            <%# Eval("GroupCode") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                     <telerik:GridTemplateColumn HeaderText="Original Doc No." UniqueName="OriginalDocumentNumber" DataField="OriginalDocumentNumber"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="160" />
                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                        <ItemTemplate>
                                            <%# Eval("OriginalDocumentNumber") %>
                                        </ItemTemplate>
                                     </telerik:GridTemplateColumn> 
                                        
                                        <telerik:GridDateTimeColumn HeaderText="Issue Date" UniqueName="Date" DataField="Date"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn> 
                                        
                                        <telerik:GridDateTimeColumn HeaderText="Response Required Date" UniqueName="ResponseRequiredDate" DataField="ResponseRequiredDate"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>  

                                        <telerik:GridTemplateColumn HeaderText="Response To" UniqueName="ResponseToName" DataField="ResponseToName"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="150"/>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("ResponseToName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Carbon Copy" UniqueName="CarbonCopyName" DataField="CarbonCopyName"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="70" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("CarbonCopyName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Related CS/Letter" UniqueName="RelatedCSLNo" DataField="RelatedCSLNo"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("RelatedCSLNo") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Need Reply" UniqueName="IsNeedReply" AllowFiltering="False" Display="False">
                                             <HeaderStyle Width="60" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/ok.png" Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsNeedReply"))%>'/>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Description" UniqueName="Description" DataField="Description"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="200" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("Description") != null ? Eval("Description").ToString().Replace(Environment.NewLine, "<br/>") : string.Empty %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Treatment" UniqueName="Treatment" DataField="Treatment"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("Treatment") != null ? Eval("Treatment").ToString().Replace(Environment.NewLine, "<br/>") : string.Empty %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                         <telerik:GridTemplateColumn HeaderText="Proposed By" UniqueName="ProposedBy" DataField="ProposedBy"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("ProposedBy") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridDateTimeColumn HeaderText="Proposed Date" UniqueName="ProposedDate" DataField="ProposedDate"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>    

                                        <telerik:GridTemplateColumn HeaderText="Reviewed By" UniqueName="ReviewedBy" DataField="ReviewedBy"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("ReviewedBy") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridDateTimeColumn HeaderText="Reviewed Date" UniqueName="ReviewedDate" DataField="ReviewedDate"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>

                                        <telerik:GridTemplateColumn HeaderText="Approved By" UniqueName="ApprovedBy" DataField="ApprovedBy"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("ApprovedBy") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridDateTimeColumn HeaderText="Approved Date" UniqueName="ApprovedDate" DataField="ApprovedDate"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>
                                        
                                        <telerik:GridDateTimeColumn HeaderText="Issued Date (FROM)" UniqueName="IssuedDateFrom" DataField="IssuedDateFrom"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>
                                        
                                        <telerik:GridDateTimeColumn HeaderText="Received Date (TO)" UniqueName="ReceivedDateTo" DataField="ReceivedDateTo"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>
                                        
                                        <telerik:GridDateTimeColumn HeaderText="Received Date (CC)" UniqueName="ReceivedDateCC" DataField="ReceivedDateCC"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>

                                        <telerik:GridTemplateColumn HeaderText="Area" UniqueName="AreaCode" DataField="AreaCode"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("AreaCode") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Unit" UniqueName="UnitCode" DataField="UnitCode"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("UnitCode") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="System" UniqueName="SystemCode" DataField="SystemCode"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("SystemCode") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Sub-System" UniqueName="SubsystemCode" DataField="SubsystemCode"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("SubsystemCode") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="KKS Code" UniqueName="KKSCode" DataField="KKSCode"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("KKSCode") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Train No" UniqueName="TrainNo" DataField="TrainNo"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("TrainNo") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Discipline" UniqueName="DisciplineCode" DataField="DisciplineCode"
                                        ShowFilterIcon="False" FilterControlWidth="97%"  Display="false"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("DisciplineCode") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Sheet No." UniqueName="SheetNo" DataField="SheetNo"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("SheetNo") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridDateTimeColumn HeaderText="Planned Date" UniqueName="PlannedDate" DataField="PlannedDate"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>
                                        
                                        <telerik:GridDateTimeColumn HeaderText="Actual Date" UniqueName="ActualDate" DataField="ActualDate"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>
                                        
                                         <telerik:GridTemplateColumn HeaderText="Remarks" UniqueName="Remarks" DataField="Remarks"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("Remarks").ToString().Replace(Environment.NewLine, "<br/>") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn> 
                                        
                                       
                                        
                                        <telerik:GridTemplateColumn HeaderText="Revision Schema" UniqueName="RevisionSchemaName" DataField="RevisionSchemaName" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("RevisionSchemaName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Minor Rev." UniqueName="MinorRev" DataField="MinorRev" ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("MinorRev") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                         <telerik:GridDateTimeColumn HeaderText="Rev Date" UniqueName="RevDate" DataField="RevDate"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Rev. Status" UniqueName="RevStatusName" DataField="RevStatusName"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("RevStatusName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                         <telerik:GridTemplateColumn HeaderText="Rev. Remarks" UniqueName="RevRemarks" DataField="RevRemarks"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("RevRemarks").ToString().Replace(Environment.NewLine, "<br/>") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn> 
                                        
                                        <telerik:GridTemplateColumn HeaderText="Document Action Code" UniqueName="DocActionCode" DataField="DocActionCode"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="50" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("DocActionCode") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                         <telerik:GridTemplateColumn HeaderText="Document Review Status" UniqueName="DocReviewStatusCode" DataField="DocReviewStatusCode"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="100" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("DocReviewStatusCode") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                         <telerik:GridTemplateColumn HeaderText="Incoming Transmittal" UniqueName="IncomingTransNo" DataField="IncomingTransNo"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("IncomingTransNo") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn> 

                                        <telerik:GridTemplateColumn HeaderText="Outgoing Transmittal" UniqueName="OutgoingTransNo" DataField="OutgoingTransNo"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("OutgoingTransNo") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn> 
                                      <telerik:GridTemplateColumn HeaderText="Created By" UniqueName="CreatedByName"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center"  />
                                        <ItemTemplate>
                                            <%# Eval("CreatedByName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridDateTimeColumn HeaderText="Date Created" UniqueName="CreatedDate" DataField="CreatedDate" Display="False"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>    
                                     <telerik:GridTemplateColumn HeaderText="Last Update By" UniqueName="LastUpdatedByName" ShowFilterIcon="False" FilterControlWidth="97%"  Display="False"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                        <ItemStyle HorizontalAlign="Center"  />
                                        <ItemTemplate>
                                            <%# Eval("LastUpdatedByName") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                         <telerik:GridDateTimeColumn HeaderText="Last Update Date" UniqueName="LastUpdatedDate" DataField="LastUpdatedDate" Display="False"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterControlWidth="80%" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True">
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </telerik:GridDateTimeColumn>   

                                    
                                          <telerik:GridTemplateColumn HeaderText="Referenced by FCR/DCN No" UniqueName="ChangeRequestNo" DataField="ChangeRequestNo"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="true">
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("ChangeRequestNo") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                          <telerik:GridTemplateColumn HeaderText="Revised by FCR/DCN No" UniqueName="ChangeRequestNoFoRevised" DataField="ChangeRequestNoFoRevised"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="true">
                                        <HeaderStyle HorizontalAlign="Center" Width="150" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("ChangeRequestNoFoRevised") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                          <telerik:GridTemplateColumn HeaderText="FCR/DCN Review Result Code" UniqueName="ChangeRequestReviewResultCode" DataField="ChangeRequestReviewResultCode"
                                        ShowFilterIcon="False" FilterControlWidth="97%" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Display="true">
                                        <HeaderStyle HorizontalAlign="Center" Width="100" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# Eval("ChangeRequestReviewResultCode") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings>
                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="True" SaveScrollPosition="True" UseStaticHeaders="True" />
                </ClientSettings>
            </telerik:RadGrid>
        </div>

        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
        <telerik:RadAjaxManager runat="Server" ID="ajaxCustomer">
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
