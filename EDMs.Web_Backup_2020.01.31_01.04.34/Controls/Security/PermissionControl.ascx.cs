// --------------------------------------------------------------------------------------------------------------------
// <copyright file="List.ascx.cs" company="">
//   
// </copyright>
// <summary>
//   The list.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Security
{
    /// <summary>
    /// The role list.
    /// </summary>
    public partial class PermissionControl : UserControl
    {
        #region Fields

        private int? _selectedRoleId ;
        private readonly MenuService _menuService;
        private readonly RoleService _roleService;
        private PermissionService _permissionService;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the selected role id.
        /// </summary>
        /// <value>
        /// The selected role id.
        /// </value>
        public int? SelectedRoleId
        {
            get
            {
                int roleId;
                _selectedRoleId = int.TryParse(ddlRoles.SelectedValue, out roleId) ? roleId : (int?)null;
                return _selectedRoleId;
            }
            set { _selectedRoleId = value; }
        }

        /// <summary>
        /// Gets the type of the selected.
        /// </summary>
        /// <value>
        /// The type of the selected.
        /// </value>
        public int SelectedType
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Gets or sets the permitted menu ids.
        /// </summary>
        /// <value>
        /// The permitted menu ids.
        /// </value>
        public List<int> PermittedMenuIds
        {
            get { return (List<int>) (ViewState["__PermittedMenuIds__"] ?? new List<int>()); }
            set { ViewState["__PermittedMenuIds__"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized
        {
            get
            {
                bool value;
                if (ViewState["__IsInitialized__"] != null &&
                    bool.TryParse(ViewState["__IsInitialized__"].ToString(), out value))
                {
                    return value;
                }
                return false;
            }
            set { ViewState["__IsInitialized__"] = value; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsInitialized)
            {
                LoadRoleData();
                LoadPermissionData();
                IsInitialized = true;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlRoles control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPermissionData();
        }

        /// <summary>
        /// Handles the OnNodeDataBound event of the treePemissionMenus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RadTreeNodeEventArgs"/> instance containing the event data.</param>
        protected void treePemissionMenus_OnNodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            
        }

        /// <summary>
        /// Handles the Click event of the btnCapNhat control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (SelectedRoleId == null) return;

            var checkedNodes = treePemissionMenus.CheckedNodes.Where(x => x.Nodes.Count == 0);

            // Gets the stored permissions
            var permissions = _permissionService.GetByRoleId(SelectedRoleId.Value);

            // Gets the objects have changed status from 'Checked' to 'Unchecked'
            var deletePermissions = permissions.Where(x => !checkedNodes.Any(checkedNode => checkedNode.Value == x.MenuId.ToString()));
            deletePermissions = deletePermissions.Where(x => x.Menu.Type == SelectedType);
            var newDeletePermissions = new List<Permission>();
            foreach (var deletePermission in deletePermissions)
            {
                if (deletePermission.MenuId == 43)
                {
                    newDeletePermissions.Add(this._permissionService.GetByRoleId(deletePermission.RoleId).FirstOrDefault(t => t.MenuId == 59));
                }

                if (deletePermission.MenuId == 46)
                {
                    newDeletePermissions.Add(this._permissionService.GetByRoleId(deletePermission.RoleId).FirstOrDefault(t => t.MenuId == 60));
                }

                if (deletePermission.MenuId == 52)
                {
                    newDeletePermissions.Add(this._permissionService.GetByRoleId(deletePermission.RoleId).FirstOrDefault(t => t.MenuId == 61));
                }
            }

            // Get the object have 'Checked' status
            var addPermissions = checkedNodes.Where(node => !permissions.Any(y => y.MenuId.ToString() == node.Value))
                .Select(node => new Permission
                {
                    MenuId = int.Parse(node.Value),
                    RoleId = SelectedRoleId.Value
                });

            var newAddPermission = new List<Permission>();
            foreach (var addPermission in addPermissions)
            {
                if (addPermission.MenuId == 43)
                {
                    newAddPermission.Add(new Permission() { MenuId = 59, RoleId = addPermission.RoleId });
                }

                if (addPermission.MenuId == 46)
                {
                    newAddPermission.Add(new Permission() { MenuId = 60, RoleId = addPermission.RoleId });
                }

                if (addPermission.MenuId == 52)
                {
                    newAddPermission.Add(new Permission() { MenuId = 61, RoleId = addPermission.RoleId });
                }
            }

            var isDeleted = _permissionService.DeletePermissions(deletePermissions.ToList());
            _permissionService.DeletePermissions(newDeletePermissions);

            var isAdded = _permissionService.AddPermissions(addPermissions.ToList());
            _permissionService.AddPermissions(newAddPermission);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlTypes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPermissionData();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionControl"/> class.
        /// </summary>
        public PermissionControl()
        {
            _menuService = new MenuService();
            _roleService = new RoleService();
            _permissionService = new PermissionService();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Loads the permission data.
        /// </summary>
        private void LoadPermissionData()
        {
            if (SelectedRoleId == null) SelectedRoleId = -1;

            var menus = _menuService.GetAllMenusByType(SelectedType);
            PermittedMenuIds = _permissionService.GetByRoleId(SelectedRoleId.Value).Select(x => x.MenuId).ToList();

            var menuPermissions = menus.Select(menu => new MenuPermissionWrapper
            {
                Menu = menu,
                IsPermitted = PermittedMenuIds.Any(menuId => menuId == menu.Id)
            });

            treePemissionMenus.DataSource = menuPermissions;
            treePemissionMenus.DataBind();

            //To check the all parents checkbox state
            var allNodes = treePemissionMenus.Nodes;
            foreach(RadTreeNode node in allNodes)
            {
                CheckTreeNodeState(node);
            }

        }

        /// <summary>
        /// Loads the role data.
        /// </summary>
        private void LoadRoleData()
        {
            var roles = _roleService.GetAll(UserSession.Current.RoleId == 1);
            ddlRoles.DataSource = roles;
            ddlRoles.DataValueField = "Id";
            ddlRoles.DataTextField = "Name";
            ddlRoles.DataBind();
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

        #endregion

    }

    /// <summary>
    /// The class represent for menu's permission
    /// </summary>
    public class MenuPermissionWrapper
    {
        #region Properties

        /// <summary>
        /// Gets or sets the menu.
        /// </summary>
        /// <value>
        /// The menu.
        /// </value>
        public Data.Entities.Menu Menu { get; set; }

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