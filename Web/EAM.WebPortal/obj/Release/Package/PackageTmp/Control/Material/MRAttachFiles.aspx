<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MRAttachFiles.aspx.cs" Inherits="EAM.WebPortal.Control.Material.MRAttachFiles" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <script src="../../Resources/Scripts/jquery-1.7.1.js" type="text/javascript"></script>
    <style type="text/css">
        #grdDocument_GridData {
            height: 100% !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="content">
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <telerik:RadGrid ID="grdDocument" runat="server" AllowPaging="False"
                AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"
                GridLines="None" Skin="Metro"
                OnNeedDataSource="grdDocument_OnNeedDataSource" 
                PageSize="5" Style="outline: none" Height="100%" AllowFilteringByColumn="True">
                <GroupingSettings CaseSensitive="False"></GroupingSettings>
                <MasterTableView ClientDataKeyNames="ID" DataKeyNames="ID" Width="100%" Height="100%">
                    <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Documents." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Visible="False" />                                        
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DownloadColumn">
                            <HeaderStyle Width="35" />
                            <ItemStyle HorizontalAlign="Center" Width="5%"/>
                            <ItemTemplate>
                                <a href='<%# DataBinder.Eval(Container.DataItem, "FilePath") %>' 
                                    download='<%# DataBinder.Eval(Container.DataItem, "Filename") %>' target="_blank">
                                    <asp:Image ID="Image1" runat="server" ImageUrl='../../Resources/Images/document.png'
                                        Style="cursor: pointer;" ToolTip="Download document" /> 
                                </a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="Filename" HeaderText="Tên file" UniqueName="Filename" ShowFilterIcon="False" FilterControlWidth="98%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                            <HeaderStyle HorizontalAlign="Center" Width="300" />
                            <ItemStyle HorizontalAlign="Left"/>
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="CreatedByName" HeaderText="Người tạo" UniqueName="CreatedByName" ShowFilterIcon="False" FilterControlWidth="98%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                            <HeaderStyle HorizontalAlign="Center" Width="150" />
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridBoundColumn>
                
                        <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Giờ tạo" UniqueName="CreatedDate"
                            DataFormatString="{0:dd-MM-yy hh:mm tt}" AllowFiltering="False">
                            <HeaderStyle HorizontalAlign="Center" Width="130" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                
                        <telerik:GridBoundColumn DataField="FileSize" HeaderText="File size(Kb)" UniqueName="FileSize" DataFormatString="{0:0,0.00}" Display="False" AllowFiltering="False">
                            <HeaderStyle HorizontalAlign="Center" Width="80" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings Selecting-AllowRowSelect="true" AllowColumnHide="True">
                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                    <Scrolling AllowScroll="True" SaveScrollPosition="True" ScrollHeight="200" UseStaticHeaders="True" />
                </ClientSettings>
            </telerik:RadGrid>
            
        </div>
        <telerik:RadCodeBlock runat="server">
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
