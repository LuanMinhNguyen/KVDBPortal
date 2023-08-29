<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VersionHistory.aspx.cs" Inherits="EDMs.Web.Controls.Document.VersionHistory" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.7.1.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="content">
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="True"
                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                GridLines="None" Skin="Windows7"
                OnNeedDataSource="grdDocument_OnNeedDataSource" PageSize="5" Style="outline: none">
                <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%">
                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />

                        <%--<telerik:GridTemplateColumn>
                            <HeaderStyle Width="3%"  />
                            <ItemStyle HorizontalAlign="Center" Width="3%"  />
                            <ItemTemplate>
                                <asp:Image ID="EditLink" runat="server" ImageUrl="~/Images/edit.png" Style="cursor: pointer;" AlternateText="Edit properties" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>

                        <%--<telerik:GridButtonColumn CommandName="Delete" UniqueName="Delete" 
                            ConfirmText="Do you want to delete document?" ButtonType="ImageButton" ImageUrl="~/Images/delete.png">
                            <HeaderStyle Width="3%" />
                                <ItemStyle HorizontalAlign="Center" Width="3%"  />
                        </telerik:GridButtonColumn>--%>

                        <telerik:GridTemplateColumn>
                            <HeaderStyle Width="3%" />
                            <ItemStyle HorizontalAlign="Center" Width="3%"/>
                            <ItemTemplate>
                                <a download='<%# DataBinder.Eval(Container.DataItem, "RevisionFileName") %>' 
                                    href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' target="_blank">
                                    <asp:Image ID="CallLink" runat="server" ImageUrl='<%# "../../" +  DataBinder.Eval(Container.DataItem, "FileExtensionIcon") %>' 
                                        Style="cursor: pointer;" AlternateText="Download document" /> 
                                </a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--3--%>
                        <telerik:GridBoundColumn DataField="Name" HeaderText="Name" UniqueName="Name">
                            <HeaderStyle HorizontalAlign="Center" Width="14%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridBoundColumn>
                                            
                        <telerik:GridBoundColumn DataField="DocumentNumber" HeaderText="Document Number" UniqueName="DocumentNumber">
                            <HeaderStyle HorizontalAlign="Center" Width="12%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="Title" HeaderText="Title" UniqueName="Title">
                            <HeaderStyle HorizontalAlign="Center" Width="14%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="Revision.Name" HeaderText="Revision" UniqueName="Revision">
                            <HeaderStyle HorizontalAlign="Center" Width="6%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="Status.Name" HeaderText="Status" UniqueName="Status">
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="ReceivedFrom.Name" HeaderText="Received from" UniqueName="ReceivedFrom">
                            <HeaderStyle HorizontalAlign="Center" Width="7%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="CreatedUser.FullName" HeaderText="Created by" UniqueName="CreatedBy">
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Created date" UniqueName="CreatedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                            <HeaderStyle HorizontalAlign="Center" Width="8%" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="LastUpdatedUser.FullName" HeaderText="Last update by" UniqueName="LastUpdatedBy">
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="LastUpdatedDate" HeaderText="Last updated date" UniqueName="LastUpdatedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" >
                            <HeaderStyle HorizontalAlign="Center" Width="8%" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        
                    </Columns>
                </MasterTableView>
                <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                    <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="200" UseStaticHeaders="True" />
                </ClientSettings>
            </telerik:RadGrid>
            
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="CustomerDialog" runat="server" Title="Document Information"
                VisibleStatusbar="false" Height="690" Width="650" 
                Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
        </div>
        <telerik:RadCodeBlock runat="server">
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
