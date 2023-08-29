<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PermissionControl.ascx.cs" Inherits="EDMs.Web.Controls.Security.PermissionControl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<link href="Content/styles.css" rel="stylesheet" type="text/css" />

<telerik:RadSplitter ID="nestedSplitter" ResizeWithParentPane="false" Width="100%" Orientation="Horizontal"
    Height="100%" runat="server">
    <telerik:RadPane ID="headerPane" runat="server" Height="25px">
        <div class="qlcbFormItem">
            <label style="width: 60px; float: left; padding-top: 5px; padding-right: 10px; text-align: right;">
                <span style="color: #2E5689; text-align: right; ">Group
                </span>
            </label>
            <asp:DropDownList ID="ddlRoles" runat="server" AutoPostBack="True" Style="width: 300px;" CssClass="min25Percent" OnSelectedIndexChanged="ddlRoles_SelectedIndexChanged"/>
        </div>
    </telerik:RadPane>
    <telerik:RadPane ID="treeViewPane" runat="server">
        <telerik:RadTreeView
            ID="treePemissionMenus"
            runat="server"
            Height="100%"
            Width="100%"
            CheckBoxes="True"
            TriStateCheckBoxes="False"
            CheckChildNodes="False"
            DataFieldID="MenuId"
            DataTextField="Description"
            DataFieldParentID="ParentId"
            DataValueField="MenuId"
            OnNodeDataBound="treePemissionMenus_OnNodeDataBound"
            OnClientNodeChecked="clientNodeChecked">
            <DataBindings>
                <telerik:RadTreeNodeBinding Expanded="true" CheckedField="IsPermitted"/> 
            </DataBindings> 
        </telerik:RadTreeView>
    </telerik:RadPane>
    <telerik:RadPane ID="RadPane1" runat="server" Height="25px">
        <div>
            <telerik:RadButton ID="btnCapNhat" runat="server" Text="Save" OnClick="btnCapNhat_Click">
                <Icon PrimaryIconUrl="../../Images/save.png" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconWidth="16" PrimaryIconHeight="16"></Icon>
            </telerik:RadButton>
        </div>
    </telerik:RadPane>
</telerik:RadSplitter>

<telerik:RadCodeBlock runat="server">
    <script type="text/javascript">
        function clientNodeChecked(sender, eventArgs) {
            var i;
            var node = eventArgs.get_node();
            var childNodes = node.get_allNodes();
            
            eventArgs.get_node().set_checkable(false);
            
            //To change the status of all child nodes when the parent checked/ unchecked
            if (childNodes.length > 0) {
                for (i = 0; i < childNodes.length; i++) {
                    childNodes[i].set_checked(node.get_checked());
                }
            }

            //To uncheck parent Nodes when any child Node has been unchecked
            if (!node.get_checked()) {
                while (node.get_parent().set_checked != null) {
                    node.get_parent().set_checked(false);
                    node = node.get_parent();
                }
            }
            
            //To check parent Nodes when all child nodes has been checked
            if (node.get_checked()) {
                var parentNode = node.get_parent();
                while(parentNode.set_checked != null) {
                    var allNodes = parentNode.get_nodes();
                    for(i = 0; i<allNodes.get_count(); i++) {
                        if (!allNodes.getNode(i).get_checked()) {
                            eventArgs.get_node().set_checkable(true);
                            return;
                        }
                    }
                    parentNode.set_checked(true);
                    parentNode = parentNode.get_parent();
                }
            }
            
            eventArgs.get_node().set_checkable(true);
        }
    </script>
</telerik:RadCodeBlock>