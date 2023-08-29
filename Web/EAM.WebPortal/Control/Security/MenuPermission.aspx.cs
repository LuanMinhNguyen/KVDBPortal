// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.ApplicationServices;
using System.Web.UI;
using EAM.Business.Services.Security;
using EAM.Data.Entities;
using EAM.WebPortal.Resources.Utilities.Session;
using Telerik.Web.UI;

namespace EAM.WebPortal.Control.Security
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class MenuPermission : Page
    {
        /// <summary>
        /// The role service.
        /// </summary>
        private readonly AA_RolesService roleService = new AA_RolesService();

        /// <summary>
        /// The menu service.
        /// </summary>
        private readonly AA_MenusService menuService = new AA_MenusService();

        /// <summary>
        /// The permission service.
        /// </summary>
        private readonly AA_PermissionsService permissionService = new AA_PermissionsService();

        /// <summary>
        /// The page_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Add("SelectedMainMenu", "System");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            if (!Page.IsPostBack)
            {
                this.LoadScopePanel();
                this.LoadCostContractPanel();
                //this.LoadListPanel();
                this.LoadSystemPanel();

                this.LoadGroupData();
            }
        }

        /// <summary>
        /// RadAjaxManager1  AjaxRequest
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                
            }
        }

        protected void radPbGroup_ItemClick(object sender, RadPanelBarEventArgs e)
        {
            this.LoadPermissionData(Convert.ToInt32(radPbGroup.SelectedItem.Value));
        }

        protected void treePemissionMenus_OnNodeDataBound(object sender, RadTreeNodeEventArgs e)
        {

        }

        /// <summary>
        /// Loads the permission data.
        /// </summary>
        private void LoadPermissionData(int roleId)
        {
            var menus = this.menuService.GetAllMenusByType(1);
            var permittedMenuIds = this.permissionService.GetByRoleId(roleId).Select(x => x.MenuId).ToList();

            var menuPermissions = menus.Select(menu => new MenuMainPermissionWrapper
            {
                Menu = menu,
                IsPermitted = permittedMenuIds.Any(menuId => menuId == menu.Id)
            });

            treePemissionMenus.DataSource = menuPermissions;
            treePemissionMenus.DataBind();

            //To check the all parents checkbox state
            var allNodes = treePemissionMenus.Nodes;
            foreach (RadTreeNode node in allNodes)
            {
                CheckTreeNodeState(node);
            }

        }

        private void LoadGroupData()
        {
            var listRole = new List<AA_Roles>();
            listRole = this.roleService.GetAll(false);

            ////if (UserSession.Current.User.IsAdmin.GetValueOrDefault())
            ////{
            ////    listRole = this.roleService.GetAll();
            ////}
            ////else
            ////{
            ////    var roleObj = this.roleService.GetByID(UserSession.Current.User.RoleId.GetValueOrDefault());
            ////    if (roleObj != null)
            ////    {
            ////        listRole.Add(roleObj);
            ////    }
            ////}

            foreach (var role in listRole)
            {
                role.ParentId = -1;
            }

            listRole.Insert(0, new AA_Roles { Id = -1, Name = "GROUP USER" });

            this.radPbGroup.DataSource = listRole.OrderBy(t => t.Name);
            this.radPbGroup.DataFieldParentID = "ParentId";
            this.radPbGroup.DataFieldID = "Id";
            this.radPbGroup.DataValueField = "Id";
            this.radPbGroup.DataTextField = "Name";
            this.radPbGroup.DataBind();
            this.radPbGroup.Items[0].Expanded = true;

            foreach (RadPanelItem item in this.radPbGroup.Items[0].Items)
            {
                item.ImageUrl = @"~/Resources/Images/group.png";
            }
        }

        /// <summary>
        /// Checks the state of the tree node.
        /// </summary>
        /// <param name="node">The node.</param>
        private static void CheckTreeNodeState(RadTreeNode node)
        {
            if (node.Nodes.Count <= 0) return;

            bool state = true;
            foreach (RadTreeNode childNode in node.Nodes)
            {
                if (childNode.Nodes.Count > 0) CheckTreeNodeState(childNode);
                if (!childNode.Checked) state = false;
            }
            node.Checked = state;
        }

        protected void CustomerMenu_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            if (e.Item.Value == "Save" && this.radPbGroup.SelectedItem != null)
            {
                var selectedRoleId = Convert.ToInt32(this.radPbGroup.SelectedItem.Value);
                var checkedNodes = this.treePemissionMenus.CheckedNodes.Where(x => x.Nodes.Count == 0);

                // Gets the stored permissions
                var permissions = this.permissionService.GetByRoleId(selectedRoleId);

                // Gets the objects have changed status from 'Checked' to 'Unchecked'
                var deletePermissions = permissions.Where(x => !checkedNodes.Any(checkedNode => checkedNode.Value == x.MenuId.ToString()));
                deletePermissions = deletePermissions.Where(x => x.Menu.Type == 1);

                // Get the object have 'Checked' status
                var addPermissions = checkedNodes.Where(node => !permissions.Any(y => y.MenuId.ToString() == node.Value))
                    .Select(node => new AA_Permissions
                    {
                        MenuId = int.Parse(node.Value),
                        RoleId = selectedRoleId
                    });

                var isDeleted = this.permissionService.DeletePermissionss(deletePermissions.ToList());
                var isAdded = this.permissionService.AddPermissionss(addPermissions.ToList());

                this.LoadGroupData();
            }
        }

        private void LoadListPanel()
        {
            var listId = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("ListID"));
            var permissions = this.permissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault(), listId);
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    permission.ParentId = -1;
                    permission.MenuName = permission.Menu.Description;
                }

                permissions.Insert(0, new AA_Permissions() { Id = -1, MenuName = "LIST" });

                this.radPbList.DataSource = permissions;
                this.radPbList.DataFieldParentID = "ParentId";
                this.radPbList.DataFieldID = "Id";
                this.radPbList.DataValueField = "Id";
                this.radPbList.DataTextField = "MenuName";
                this.radPbList.DataBind();
                this.radPbList.Items[0].Expanded = true;

                foreach (RadPanelItem item in this.radPbList.Items[0].Items)
                {
                    item.ImageUrl = @"~/Resources/Images/listmenu.png";
                    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                }
            }
        }

        private void LoadCostContractPanel()
        {
            var listId = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("CostContractID"));
            var permissions = this.permissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault(), listId);
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    permission.ParentId = -1;
                    permission.MenuName = permission.Menu.Description;
                }

                permissions.Insert(0, new AA_Permissions() { Id = -1, MenuName = "CONFIG COST/CONTRACT MANAGE" });

                this.radPbCostContract.DataSource = permissions;
                this.radPbCostContract.DataFieldParentID = "ParentId";
                this.radPbCostContract.DataFieldID = "Id";
                this.radPbCostContract.DataValueField = "Id";
                this.radPbCostContract.DataTextField = "MenuName";
                this.radPbCostContract.DataBind();
                this.radPbCostContract.Items[0].Expanded = true;

                foreach (RadPanelItem item in this.radPbCostContract.Items[0].Items)
                {
                    item.ImageUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Icon;
                    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                }
            }
        }

        private void LoadScopePanel()
        {
            var listId = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("ScopeID"));
            var permissions = this.permissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault(), listId).OrderBy(t => t.Menu.Priority).ToList();
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    permission.ParentId = -1;
                    permission.MenuName = permission.Menu.Description;
                }

                permissions.Insert(0, new AA_Permissions() { Id = -1, MenuName = "CONFIG MANAGEMENT" });

                this.radPbScope.DataSource = permissions;
                this.radPbScope.DataFieldParentID = "ParentId";
                this.radPbScope.DataFieldID = "Id";
                this.radPbScope.DataValueField = "Id";
                this.radPbScope.DataTextField = "MenuName";
                this.radPbScope.DataBind();
                this.radPbScope.Items[0].Expanded = true;

                foreach (RadPanelItem item in this.radPbScope.Items[0].Items)
                {
                    item.ImageUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Icon;
                    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                }
            }
        }

        private void LoadSystemPanel()
        {
            var systemId = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("SystemID"));
            var permissions = this.permissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault(), systemId);
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    permission.ParentId = -1;
                    permission.MenuName = permission.Menu.Description;
                }

                permissions.Insert(0, new AA_Permissions() { Id = -1, MenuName = "SYSTEM" });

                this.radPbSystem.DataSource = permissions;
                this.radPbSystem.DataFieldParentID = "ParentId";
                this.radPbSystem.DataFieldID = "Id";
                this.radPbSystem.DataValueField = "Id";
                this.radPbSystem.DataTextField = "MenuName";
                this.radPbSystem.DataBind();
                this.radPbSystem.Items[0].Expanded = true;

                foreach (RadPanelItem item in this.radPbSystem.Items[0].Items)
                {
                    item.ImageUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Icon;
                    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                    if (item.Text == "Menu Permission")
                    {
                        item.Selected = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// The class represent for menu's permission
    /// </summary>
    public class MenuMainPermissionWrapper
    {
        #region Properties

        /// <summary>
        /// Gets or sets the menu.
        /// </summary>
        /// <value>
        /// The menu.
        /// </value>
        public AA_Menus Menu { get; set; }

        public int MenuId
        {
            get { return Menu.Id; }
        }

        public int? ParentId
        {
            get { return Menu.ParentId; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is permitted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is permitted; otherwise, <c>false</c>.
        /// </value>
        public bool IsPermitted { get; set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return Menu.Description; }
        }

        #endregion

    }
}

