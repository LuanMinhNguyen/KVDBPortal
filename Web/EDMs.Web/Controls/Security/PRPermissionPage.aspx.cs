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
using EDMs.Business.Services.CostContract;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Security
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class PRPermissionPage : Page
    {
        private readonly PermissionService permissionService = new PermissionService();

        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string unreadPattern = @"\(\d+\)";

        private readonly UserService userService = new UserService();

        private readonly CostContractProjectService projectService = new CostContractProjectService();

        private readonly ProcurementRequirementService prService = new ProcurementRequirementService();

        private readonly PermissionProcurementRequirementService permissionPRService = new PermissionProcurementRequirementService();

        private List<int> AdminGroup
        {
            get
            {
                return ConfigurationManager.AppSettings
                    .Get("GroupAdminList").Split(',')
                    .Select(t => Convert.ToInt32(t)).ToList();
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
                this.LoadCostContractPanel();
                this.LoadScopePanel();
                //this.LoadListPanel();
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
            if (this.rtvPR.SelectedNode != null)
            {
                foreach (RadListBoxItem user in this.ddlUser.SelectedItems)
                {
                    var permissionObj = new PermissionProcurementRequirement()
                    {
                        UserId = Convert.ToInt32(user.Value),
                        ProcurementRequirementID = Convert.ToInt32(this.rtvPR.SelectedNode.Value),
                        ProjectId = Convert.ToInt32(this.ddlProject.SelectedValue),
                        IsFullPermission = this.rbtnFull.Checked,
                        IsShared = this.rbbtnShareControl.Checked,
                        IsViewOnly = this.rbtnView.Checked,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now
                    };
                    this.permissionPRService.Insert(permissionObj);
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
            var prList = this.prService.GetAllByProject(Convert.ToInt32(this.ddlProject.SelectedValue)).OrderBy(t => t.Number);
            this.rtvPR.DataSource = prList;
            this.rtvPR.DataTextField = "Number";
            this.rtvPR.DataValueField = "ID";
            this.rtvPR.DataFieldID = "ID";
            this.rtvPR.DataBind();

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
        protected void rtvPR_NodeClick(object sender, RadTreeNodeEventArgs e)
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
        protected void rtvPR_OnNodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = "~/Images/shopping.png";
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
            if (this.rtvPR.SelectedNode != null)
            {
                var packagePermissionList = this.permissionPRService.GetAllByPR(Convert.ToInt32(this.rtvPR.SelectedNode.Value));

                this.grdPermission.DataSource = packagePermissionList;
            }
            else
            {
                this.grdPermission.DataSource = new List<PermissionDiscipline>();
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
            this.permissionPRService.Delete(permissionId);

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
            var usersInPermission = this.permissionPRService.GetUserIdListInPermission(Convert.ToInt32(packageId)).Distinct().ToList();
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
            var projectList = this.projectService.GetAll().OrderBy(t => t.Name);

            //var ProjectHaveDiscipline = this.DisciplineService.GetAll().Select(t=>t.ProjectId).Distinct().ToList();

           this.ddlProject.DataSource = projectList;
            this.ddlProject.DataTextField = "Name";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();

            var listDisciplineInPermission = UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                ? this.prService.GetAllByProject(Convert.ToInt32(this.ddlProject.SelectedValue)).OrderBy(t => t.Number).ToList()
                : this.prService.GetAllPRInPermission(UserSession.Current.User.Id, !string.IsNullOrEmpty(this.ddlProject.SelectedValue) ? Convert.ToInt32(this.ddlProject.SelectedValue) : 0)
                .OrderBy(t => t.Number).ToList();

            this.rtvPR.DataSource = listDisciplineInPermission;
            this.rtvPR.DataTextField = "Number";
            this.rtvPR.DataValueField = "ID";
            this.rtvPR.DataFieldID = "ID";
            this.rtvPR.DataBind();
        }

        /// <summary>
        /// The reload user list.
        /// </summary>
        private void ReloadUserList()
        {
            var usersInPermission = this.permissionPRService.GetUserIdListInPermission(Convert.ToInt32(this.rtvPR.SelectedNode.Value)).Distinct().ToList();
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
                    if (item.Text == "Data Permission")
                    {
                        item.Selected = true;
                    }
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
    }
}

