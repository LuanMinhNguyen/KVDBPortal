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
using System.Web.UI;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Security
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class SharePermissionPage : Page
    {
        /// <summary>
        /// The role service.
        /// </summary>
        private readonly RoleService roleService = new RoleService();

        /// <summary>
        /// The menu service.
        /// </summary>
        private readonly MenuService menuService = new MenuService();

        /// <summary>
        /// The permission service.
        /// </summary>
        private readonly PermissionService permissionService = new PermissionService();

        private readonly SharePermissionService sharePermissionService = new SharePermissionService();

        private readonly UserService userService = new UserService();

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
        private void LoadPermissionData(int toUserid)
        {
            this.lbPIC.ClearChecked();
            var sharePermissionObj = this.sharePermissionService.GetByFromToUser(UserSession.Current.User.Id, toUserid);
            if (sharePermissionObj != null && !string.IsNullOrEmpty(sharePermissionObj.ObjectList))
            {
                foreach (RadListBoxItem item in this.lbPIC.Items)
                {
                    item.Checked = !string.IsNullOrEmpty(sharePermissionObj.ObjectList) && sharePermissionObj.ObjectList.Split(';').Contains(item.Value);
                }
            }
        }

        private void LoadGroupData()
        {
            var userList = this.userService.GetAll().Where(t => t.Id != 1).ToList();

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

            foreach (var user in userList)
            {
                user.ParentId = -1;
            }

            userList.Insert(0, new User { Id = -1, FullNameWithPosition = "USER LIST" });

            this.radPbGroup.DataSource = userList.OrderBy(t => t.FullNameWithPosition);
            this.radPbGroup.DataFieldParentID = "ParentId";
            this.radPbGroup.DataFieldID = "Id";
            this.radPbGroup.DataValueField = "Id";
            this.radPbGroup.DataTextField = "FullNameWithPosition";
            this.radPbGroup.DataBind();
            this.radPbGroup.Items[0].Expanded = true;

            foreach (RadPanelItem item in this.radPbGroup.Items[0].Items)
            {
                item.ImageUrl = @"~/Images/user.png";
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
                var toUserId = Convert.ToInt32(this.radPbGroup.SelectedItem.Value);
                var toUserObj = this.userService.GetByID(toUserId);
                var sharePermissionObj = this.sharePermissionService.GetByFromToUser(UserSession.Current.User.Id, toUserId);
                if (sharePermissionObj == null)
                {
                    sharePermissionObj = new SharePermission()
                    {
                        ID = Guid.NewGuid(),
                        ToUserId = toUserId,
                        ToUserName = toUserObj.FullNameWithPosition,
                        ToUserEmail = toUserObj.Email,
                        FromUserId = UserSession.Current.User.Id,
                        FromUserName = UserSession.Current.User.FullNameWithPosition,
                        FromUserEmail = UserSession.Current.User.Email

                    };

                    sharePermissionObj.ObjectList = string.Empty;
                    foreach (RadListBoxItem item in this.lbPIC.CheckedItems)
                    {
                        sharePermissionObj.ObjectList += item.Value + ";";
                    }

                    this.sharePermissionService.Insert(sharePermissionObj);
                }
                else
                {
                    sharePermissionObj.ObjectList = string.Empty;
                    foreach (RadListBoxItem item in this.lbPIC.CheckedItems)
                    {
                        sharePermissionObj.ObjectList += item.Value + ";";
                    }

                    this.sharePermissionService.Update(sharePermissionObj);
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

                permissions.Insert(0, new Permission() { Id = -1, MenuName = "CONFIG COST/CONTRACT MANAGE" });

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

                permissions.Insert(0, new Permission() { Id = -1, MenuName = "CONFIG MANAGEMENT" });

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

                permissions.Insert(0, new Permission() { Id = -1, MenuName = "SYSTEM" });

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
                    if (item.Text == "Share Permission")
                    {
                        item.Selected = true;
                    }
                }
            }
        }
    }

}

