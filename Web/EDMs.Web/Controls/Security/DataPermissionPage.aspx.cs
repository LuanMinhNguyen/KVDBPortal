﻿// --------------------------------------------------------------------------------------------------------------------
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
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Security
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class DataPermissionPage : Page
    {
        private readonly PermissionService permissionService = new PermissionService();

        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string unreadPattern = @"\(\d+\)";

        private readonly FolderService folderService = new FolderService();

        private readonly RoleService roleService = new RoleService();

        private readonly UserService userService = new UserService();

        private readonly ScopeProjectService scopeProjectService = new ScopeProjectService();

        private readonly WorkGroupService workGroupService = new WorkGroupService();

        private readonly PermissionWorkgroupService PermissionWorkgroupService = new PermissionWorkgroupService();

        private List<int> AdminGroup
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("GroupAdminList").Split(',').Select(t => Convert.ToInt32(t)).ToList();
            }
        }

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
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            if (!Page.IsPostBack)
            {
                this.LoadComboData();
                this.LoadListPanel();
                this.LoadSystemPanel();
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

        /// <summary>
        /// The btn save_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.rtvWorkgroup.SelectedNode != null)
            {
                foreach (RadListBoxItem user in this.ddlUser.SelectedItems)
                {
                    var permissionObj = new PermissionWorkgroup
                    {
                        UserId = Convert.ToInt32(user.Value),
                        WorkgroupId = Convert.ToInt32(this.rtvWorkgroup.SelectedNode.Value),
                        ProjectId = Convert.ToInt32(this.ddlProject.SelectedValue),
                        IsFullPermission = this.rbtnFull.Checked,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now
                    };
                    this.PermissionWorkgroupService.Insert(permissionObj);
                }
                
                this.ReloadUserList();
                this.grdPermission.Rebind();
            }
        }

        /// <summary>
        /// The ddl project_ selected index change.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void ddlProject_SelectedIndexChange(object sender, EventArgs e)
        {
            var listWorkgroup = this.workGroupService.GetAllWorkGroupOfProject(Convert.ToInt32(this.ddlProject.SelectedValue)).OrderBy(t => t.Name);
            this.rtvWorkgroup.DataSource = listWorkgroup;
            this.rtvWorkgroup.DataTextField = "Name";
            this.rtvWorkgroup.DataValueField = "ID";
            this.rtvWorkgroup.DataFieldID = "ID";
            this.rtvWorkgroup.DataBind();

            this.ddlUser.Items.Clear();
            this.grdPermission.Rebind();
        }

        /// <summary>
        /// The rtv package_ node click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void rtvWorkgroup_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            this.LoadPermissionData(e.Node.Value);
            this.grdPermission.Rebind();
        }

        /// <summary>
        /// The rtv package_ on node data bound.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void rtvWorkgroup_OnNodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = "~/Images/workgroup.png";
        }

        /// <summary>
        /// The grd permission_ on need data source.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdPermission_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (this.rtvWorkgroup.SelectedNode != null)
            {
                var packagePermissionList = this.PermissionWorkgroupService.GetAllByPackage(Convert.ToInt32(this.rtvWorkgroup.SelectedNode.Value));

                this.grdPermission.DataSource = packagePermissionList;
            }
            else
            {
                this.grdPermission.DataSource = new List<PermissionWorkgroup>();
            }
        }

        /// <summary>
        /// The grd permission_ on detele command.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdPermission_OnDeteleCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var permissionId = Convert.ToInt32(item.GetDataKeyValue("Id").ToString());
            this.PermissionWorkgroupService.Delete(permissionId);

            this.ReloadUserList();
        }

        /// <summary>
        /// The load permission data.
        /// </summary>
        /// <param name="packageId">
        /// The package id.
        /// </param>
        private void LoadPermissionData(string packageId)
        {
            var usersInPermission = this.PermissionWorkgroupService.GetUserIdListInPermission(Convert.ToInt32(packageId)).Distinct().ToList();
            var listUser = this.userService.GetAll().Where(t => !usersInPermission.Contains(t.Id)).OrderBy(t => t.Username);

            this.ddlUser.DataSource = listUser;
            this.ddlUser.DataTextField = "UserNameWithFullName";
            this.ddlUser.DataValueField = "Id";
            this.ddlUser.DataBind();
        }

        /// <summary>
        /// The load combo data.
        /// </summary>
        private void LoadComboData()
        {
            var projectInPermission = UserSession.Current.User.Id == 1
                ? this.scopeProjectService.GetAll().OrderBy(t => t.Name)
                : this.scopeProjectService.GetAllInPermission(UserSession.Current.User.Id).OrderBy(t => t.Name);

            var ProjectHaveWorkgroup = this.workGroupService.GetAll().Select(t=>t.ProjectId).Distinct().ToList();

           var projectInPermissions = projectInPermission.Where(t => ProjectHaveWorkgroup.Contains(t.ID));
            this.ddlProject.DataSource = projectInPermissions;
            this.ddlProject.DataTextField = "Name";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();

            var listWorkgroupInPermission = UserSession.Current.User.Id == 1
                ? this.workGroupService.GetAllWorkGroupOfProject(Convert.ToInt32(this.ddlProject.SelectedValue)).OrderBy(t => t.Name).ToList()
                : this.workGroupService.GetAllWorkGroupInPermission(UserSession.Current.User.Id, !string.IsNullOrEmpty(this.ddlProject.SelectedValue) ? Convert.ToInt32(this.ddlProject.SelectedValue) : 0)
                .OrderBy(t => t.Name).ToList();

            this.rtvWorkgroup.DataSource = listWorkgroupInPermission;
            this.rtvWorkgroup.DataTextField = "Name";
            this.rtvWorkgroup.DataValueField = "ID";
            this.rtvWorkgroup.DataFieldID = "ID";
            this.rtvWorkgroup.DataBind();
        }

        /// <summary>
        /// The reload user list.
        /// </summary>
        private void ReloadUserList()
        {
            var usersInPermission = this.PermissionWorkgroupService.GetUserIdListInPermission(Convert.ToInt32(this.rtvWorkgroup.SelectedNode.Value)).Distinct().ToList();
            var listUser = this.userService.GetAll().Where(t => !usersInPermission.Contains(t.Id)).OrderBy(t => t.Username);

            this.ddlUser.DataSource = listUser;
            this.ddlUser.DataTextField = "UserNameWithFullName";
            this.ddlUser.DataValueField = "Id";
            this.ddlUser.DataBind();
        }

        /// <summary>
        /// The load list panel.
        /// </summary>
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

                permissions.Insert(0, new Permission() { Id = -1, MenuName = "LIST" });

                this.radPbList.DataSource = permissions;
                this.radPbList.DataFieldParentID = "ParentId";
                this.radPbList.DataFieldID = "Id";
                this.radPbList.DataValueField = "Id";
                this.radPbList.DataTextField = "MenuName";
                this.radPbList.DataBind();
                this.radPbList.Items[0].Expanded = true;

                foreach (RadPanelItem item in this.radPbList.Items[0].Items)
                {
                    item.ImageUrl = @"~/Images/listmenu.png";
                    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                    if (item.Text == "Data Permission")
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// The load system panel.
        /// </summary>
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
                    if (item.Text == "Works Assignment")
                    {
                        item.Selected = true;
                    }
                }
            }
        }
    }
}

