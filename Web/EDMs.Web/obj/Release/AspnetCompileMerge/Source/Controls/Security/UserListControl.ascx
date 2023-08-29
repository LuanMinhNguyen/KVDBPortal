<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserListControl.ascx.cs" Inherits="EDMs.Web.Controls.Security.UserListControl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
    #ctl00_ContentPlaceHolder2_ctl00_grdRoles
    {
        height:100% !important;
    }
    /*Hide change page size control*/
    div.RadGrid .rgPager .rgAdvPart     
    {     
    display:none;        
    }
</style>
<telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
    <script type="text/javascript">
        function RowDblClick(sender, eventArgs) {
            window.radopen("Controls/Security/RoleEditForm.aspx?roleId=" + eventArgs.getDataKeyValue("Id"), "RoleDialog");
        }
    </script>
</telerik:RadCodeBlock>
<telerik:RadGrid ID="grdUsers"
    AllowCustomPaging="True"
    runat="server"
    AllowPaging="True"
    AutoGenerateColumns="False"
    CellSpacing="0"
    CellPadding="0" 
    PageSize="25" Height="100%"
    GridLines="None" 
    OnNeedDataSource="grdUsers_OnNeedDataSource"
    OnItemCommand="grdUsers_ItemCommand"
    OnItemDataBound="grdUsers_OnItemDataBound"
    >
    <MasterTableView DataKeyNames="Id" ClientDataKeyNames="Id" CommandItemDisplay="Top" Width="100%">
        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Users." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
        <Columns>
            <telerik:GridBoundColumn DataField="Id" HeaderText="Mã" UniqueName="Id" Visible="False" />
            <telerik:GridBoundColumn DataField="User.Id" HeaderText="Mã người dùng" UniqueName="colUserId" Visible="False" />

            <telerik:GridTemplateColumn AllowFiltering="False">
                <HeaderStyle Width="3%" HorizontalAlign="Center"  />
                <ItemStyle HorizontalAlign="Center" Width="3%"/>
                <ItemTemplate>
                    <asp:ImageButton runat="server" Style="padding-left: 7px; cursor: pointer;" 
                        ID="EditLink" ImageUrl="../../Images/edit.png" />
                </ItemTemplate>
            </telerik:GridTemplateColumn>

            <telerik:GridTemplateColumn AllowFiltering="False">
                <HeaderStyle Width="3%" HorizontalAlign="Center"  />
                <ItemStyle HorizontalAlign="Center" Width="3%"/>
                <ItemTemplate>
                    <!--BtnDeleteClick-->
                    <asp:ImageButton runat="server" Style="padding-left: 7px; cursor: pointer;" ID="btnDelete" CommandName="DeleteUserCommand" ImageUrl="../../Images/delete.png"
                        OnClientClick="return confirm('Do you want to delete this user?');"/>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
            <telerik:GridBoundColumn DataField="User.Username" HeaderText="User name" UniqueName="colUsername" 
                FilterControlWidth="100%" ShowFilterIcon="False" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                <HeaderStyle HorizontalAlign="Center"  Width="20%" />
                <ItemStyle HorizontalAlign="Left" Width="20%"/>
            </telerik:GridBoundColumn>
            
            <telerik:GridBoundColumn DataField="FullName" HeaderText="Full name" UniqueName="colNameFullName" 
                FilterControlWidth="100%" ShowFilterIcon="False" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                <HeaderStyle HorizontalAlign="Center"  Width="20%" />
                <ItemStyle HorizontalAlign="Left" Width="20%"/>
            </telerik:GridBoundColumn>

            <telerik:GridBoundColumn DataField="User.Role.Name" HeaderText="Group" UniqueName="colRoleName" AllowFiltering="False">
                <HeaderStyle HorizontalAlign="Center"  Width="30%" />
                <ItemStyle HorizontalAlign="Left" Width="30%"/>
            </telerik:GridBoundColumn>

            <telerik:GridBoundColumn DataField="Email" HeaderText="Email" UniqueName="Email" 
                FilterControlWidth="100%" ShowFilterIcon="False" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                <HeaderStyle HorizontalAlign="Center"  Width="20%" />
                <ItemStyle HorizontalAlign="Left" Width="20%"/>
            </telerik:GridBoundColumn>
        </Columns>
        <CommandItemStyle Height="25px"></CommandItemStyle>
        <CommandItemTemplate>
            <%--<a href="#" onclick="return ShowUserInsertForm();" style="padding-left: 7px">
                <img src="././Images/addNew.png" alt=""/>
                Add user</a>--%>
            
            <telerik:RadToolBar ID="CustomerMenu" runat="server" Width="100%" OnClientButtonClicking="OnClientButtonClicking">
                <Items>
                    <telerik:RadToolBarButton runat="server" Text="Add User" Value="1" ImageUrl="~/Images/addNew.png"></telerik:RadToolBarButton>
                    <telerik:RadToolBarButton runat="server" IsSeparator="true"/>
                    
                    <%--<telerik:RadToolBarButton runat="server">
                        <ItemTemplate>
                            <asp:Label ID="lblSearchLabel" runat="server" Text="  Quick search:  " />
                        </ItemTemplate>
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton Value="searchTextBoxButton" CssClass="searchtextbox" CommandName="searchText">
                        <ItemTemplate>
                            <telerik:RadTextBox
                                runat="server" ID="txtSearch" Width="250px"
                                EmptyMessage="Enter username, fullname, e-mail..." />
                        </ItemTemplate>
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton ImageUrl="~/Images/search.gif" Value="search" CommandName="doSearch" />--%>
                </Items>
            </telerik:RadToolBar>
        </CommandItemTemplate>
    </MasterTableView>
    <ClientSettings>
        <Selecting AllowRowSelect="true"></Selecting>
        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True" ScrollHeight="500"></Scrolling>
    </ClientSettings>
</telerik:RadGrid>
<telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
</telerik:RadAjaxManagerProxy>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        var gridUsers;
        var searchButton;
        function ShowUserEditForm(id, userId, rowIndex) {
            var oWindow = window.radopen("Controls/Security/UserEditForm.aspx?Id=" + id + "&userId=" + userId, "UserDialog");
            oWindow.setSize(500, 360);
            oWindow.SetTitle("User Information");
            oWindow.set_visibleStatusbar(false);

            var rowControl = gridUsers.get_masterTableView().get_dataItems()[rowIndex].get_element();
            gridUsers.get_masterTableView().selectItem(rowControl, true);
            return false;
        }
        
        function ShowUserInsertForm() {
            var oWindow = window.radopen("Controls/Security/UserEditForm.aspx", "UserDialog");
            oWindow.setSize(500, 360);
            oWindow.SetTitle("User Information");
            oWindow.set_visibleStatusbar(false);
            return false;
        }
        
        function refreshGrid(arg) {
            gridUsers.get_masterTableView().rebind();
        }
        
        /*****************Event Functions******************/
        function pageLoad() {
            gridUsers = $find("<%= grdUsers.ClientID %>");
            searchButton = toolbar.findButtonByCommandName("doSearch");
        }
        
        function OnClientButtonClicking(sender, args) {
            var button = args.get_item();
            var strText = button.get_text();
            if (strText.toLowerCase() == "add user") {
                var oWindow = window.radopen("Controls/Security/UserEditForm.aspx", "UserDialog");
                oWindow.setSize(500, 360);
                oWindow.SetTitle("User Information");
                oWindow.set_visibleStatusbar(false);
                return false;
            } else {
                var commandName = args.get_item().get_commandName();
                if (commandName == "doSearch") {
                    var searchTextBox = sender.findButtonByCommandName("searchText").findControl("txtSearch");
                    if (searchButton.get_value() == "clear") {
                        searchTextBox.set_value("");
                        searchButton.set_imageUrl("images/search.gif");
                        searchButton.set_value("search");
                    }

                    performSearch(searchTextBox);
                }
            }
        }

        function performSearch(searchTextBox) {
            if (searchTextBox.get_value()) {
                searchButton.set_imageUrl("images/clear.gif");
                searchButton.set_value("clear");
            }

            ajaxManager.ajaxRequest(searchTextBox.get_value());
        }
    </script>
</telerik:RadCodeBlock>
