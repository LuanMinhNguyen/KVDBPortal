<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleListControl.ascx.cs" Inherits="EDMs.Web.RoleListControl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
    #ctl00_ContentPlaceHolder2_ctl00_grdRoles
    {
        height:100% !important;
    }

</style>

<telerik:RadGrid ID="grdRoles"
    runat="server"
    AllowPaging="True"
    AutoGenerateColumns="False"
    CellSpacing="0"
    CellPadding="0" 
    PageSize="30" Height="100%"
    GridLines="None" 
    OnNeedDataSource="grdRoles_OnNeedDataSource"
    OnItemCommand="grdRoles_ItemCommand"
    OnItemDataBound="grdRoles_OnItemDataBound">

    <MasterTableView DataKeyNames="Id" ClientDataKeyNames="Id" CommandItemDisplay="Top" Width="100%" >
        <PagerStyle AlwaysVisible="True" FirstPageToolTip="First page" LastPageToolTip="Last page" NextPagesToolTip="Next page" NextPageToolTip="Next page" PagerTextFormat="Change page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, Total:  &lt;strong&gt;{5}&lt;/strong&gt; Groups." PageSizeLabelText="Row/page: " PrevPagesToolTip="Previous page" PrevPageToolTip="Previous page" />
        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
        <Columns>
            <telerik:GridBoundColumn DataField="Id" HeaderText="Mã" UniqueName="Id" Visible="False" />
            <telerik:GridTemplateColumn>
                <HeaderStyle Width="3%" />
                <ItemTemplate>
                    <asp:ImageButton runat="server" Style="padding-left: 7px; cursor: pointer;" ID="EditLink" ImageUrl="../../Images/edit.png" />
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn>
                <HeaderStyle Width="3%" />
                <ItemTemplate>
                    <!--BtnDeleteClick-->
                    <asp:ImageButton runat="server" Style="padding-left: 7px; cursor: pointer;" ID="btnDelete" ImageUrl="../../Images/delete.png" CommandName="DeleteRoleCommand"
                        OnClientClick="return confirm('Do you want to delete this Group?');"/>
                </ItemTemplate>
            </telerik:GridTemplateColumn>

            <telerik:GridBoundColumn DataField="Name" HeaderText="Name" UniqueName="colName" >
                <HeaderStyle Width="42%" HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle Width="42%" HorizontalAlign="Left"></ItemStyle>
            </telerik:GridBoundColumn>

            <telerik:GridBoundColumn DataField="Description" HeaderText="Description" UniqueName="colDescription" >
                 <HeaderStyle Width="42%" HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle Width="42%" HorizontalAlign="Left"></ItemStyle>
            </telerik:GridBoundColumn>
            
            <telerik:GridTemplateColumn HeaderText="Is Admin" UniqueName="Role Admin">
                 <HeaderStyle Width="10%" HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <asp:Image runat="server" ImageUrl='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsAdmin")) ? "~/Images/ok.png" : "" %>'/>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
            <telerik:GridTemplateColumn HeaderText="Is Admin" UniqueName="Role Update">
                 <HeaderStyle Width="10%" HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsUpdate")) ? "~/Images/ok.png" : "" %>'/>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
            <telerik:GridTemplateColumn HeaderText="Is Admin" UniqueName="Role View">
                 <HeaderStyle Width="10%" HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <asp:Image ID="Image2" runat="server" ImageUrl='<%# !Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsAdmin")) && !Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsUpdate")) ? "~/Images/ok.png" : "" %>'/>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
        </Columns>
        <CommandItemStyle Height="25px"></CommandItemStyle>
        <CommandItemTemplate>
            <a href="#" onclick="return ShowRoleInsertForm();" style="padding-left: 7px">
                <img src="././Images/addNew.png" alt=""/>
                Add group</a>
        </CommandItemTemplate>
    </MasterTableView>
    <ClientSettings>
        <Selecting AllowRowSelect="true"></Selecting>
        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True" ScrollHeight="500"></Scrolling>
    </ClientSettings>
</telerik:RadGrid>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        var gridRoles;
        function ShowRoleEditForm(id, rowIndex) {
            var oWindow = window.radopen("Controls/System/RoleEditForm.aspx?roleId=" + id, "RoleDialog");
            oWindow.setSize(500, 270);
            oWindow.SetTitle("Group Information");
            oWindow.set_visibleStatusbar(false);
            
            var rowControl = gridRoles.get_masterTableView().get_dataItems()[rowIndex].get_element();
            gridRoles.get_masterTableView().selectItem(rowControl, true);
            
            return false;
        }
        
        function ShowRoleInsertForm() {
            var oWindow = window.radopen("Controls/System/RoleEditForm.aspx", "RoleDialog");
            oWindow.setSize(500, 270);
            oWindow.SetTitle("Group Information");
            oWindow.set_visibleStatusbar(false);
            return false;
        }
        
        function refreshGrid(arg) {
            gridRoles.get_masterTableView().rebind();
        }

        /**********************Event Functions*************************/
        function pageLoad() {
            gridRoles = $find("<%= grdRoles.ClientID %>");
        }
        function RowDblClick(sender, eventArgs) {
            window.radopen("Controls/System/RoleEditForm.aspx?roleId=" + eventArgs.getDataKeyValue("Id"), "RoleDialog");
        }
    </script>
</telerik:RadCodeBlock>
