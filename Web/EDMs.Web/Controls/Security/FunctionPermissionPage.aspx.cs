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
    public partial class FunctionPermissionPage : Page
    {
        /// <summary>
        /// The role service.
        /// </summary>
        private readonly RoleService roleService = new RoleService();

        /// <summary>
        /// The menu service.
        /// </summary>
        private readonly MenuService menuService = new MenuService();

        private readonly UserService userService = new UserService();

        private readonly FunctionPermissionService fncPermissionService = new FunctionPermissionService();

        /// <summary>
        /// The permission service.
        /// </summary>
        private readonly PermissionService permissionService = new PermissionService();

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
                this.LoadSystemPanel();

                this.LoadInitData();
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


        protected void treePemissionMenus_OnNodeDataBound(object sender, RadTreeNodeEventArgs e)
        {

        }

        private void LoadInitData()
        {
            var roleList = new List<Role>();
            roleList = this.roleService.GetAll(UserSession.Current.RoleId == 1);

            this.ddlDept.DataSource = roleList.OrderBy(t => t.FullNameWithLocation);
            this.ddlDept.DataValueField = "Id";
            this.ddlDept.DataTextField = "FullNameWithLocation";
            this.ddlDept.DataBind();

            if (this.ddlDept.SelectedItem != null)
            {
                var userList = this.userService.GetAllByRoleId(Convert.ToInt32(this.ddlDept.SelectedValue)).OrderByDescending(t => t.IsDeptManager).ThenBy(t => t.FullNameWithPosition);

                this.lbPIC.DataSource = userList;
                this.lbPIC.DataTextField = "FullNameWithPosition";
                this.lbPIC.DataValueField = "Id";
                this.lbPIC.DataBind();
            }
        }

        protected void CustomerMenu_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            if (this.lbPIC.SelectedItem != null)
            {
                var userId = Convert.ToInt32(this.lbPIC.SelectedValue);
                var userObj = this.userService.GetByID(userId);
                var fncPermissionList = this.fncPermissionService.GetAllByUser(userId);
                for (int i = 1; i < 14; i++)
                {
                    var fncPermissionItem = fncPermissionList.FirstOrDefault(t => t.ObjectTypeId == i);
                    if (fncPermissionItem == null)
                    {
                        fncPermissionItem = new FunctionPermission();
                        fncPermissionItem.UserId = userObj.Id;
                        fncPermissionItem.FullName = userObj.FullName;
                        fncPermissionItem.DeptId = userObj.RoleId;
                        fncPermissionItem.DeptName = userObj.RoleName;
                        fncPermissionItem.ObjectTypeId = i;
                        this.CollectData(fncPermissionItem, i);

                        this.fncPermissionService.Insert(fncPermissionItem);
                    }
                    else
                    {
                        this.CollectData(fncPermissionItem, i);
                        this.fncPermissionService.Update(fncPermissionItem);
                    }
                }
            }
        }

        private void CollectData(FunctionPermission fncPermissionItem, int i)
        {
            switch (i)
            {
                case 1:
                    fncPermissionItem.ObjectTypeName = "MATERIAL REQUISITION";
                    fncPermissionItem.IsView = this.cbMRView.Checked;
                    fncPermissionItem.IsCreate = this.cbMRCreate.Checked;
                    fncPermissionItem.IsUpdate = this.cbMRUpdate.Checked;
                    fncPermissionItem.IsCancel = this.cbMRCancel.Checked;
                    fncPermissionItem.IsAttachWorkflow = this.cbMRAttachWorkflow.Checked;
                    break;
                case 2:
                    fncPermissionItem.ObjectTypeName = "WORK REQUEST";
                    fncPermissionItem.IsView = this.cbWRView.Checked;
                    fncPermissionItem.IsCreate = this.cbWRCreate.Checked;
                    fncPermissionItem.IsUpdate = this.cbWRUpdate.Checked;
                    fncPermissionItem.IsCancel = this.cbWRCancel.Checked;
                    fncPermissionItem.IsAttachWorkflow = this.cbWRAttachWorkflow.Checked;
                    break;
                case 3:
                    fncPermissionItem.ObjectTypeName = "ENGINEERING CHANGE REQUEST";
                    fncPermissionItem.IsView = this.cbECRView.Checked;
                    fncPermissionItem.IsCreate = this.cbECRCreate.Checked;
                    fncPermissionItem.IsUpdate = this.cbECRUpdate.Checked;
                    fncPermissionItem.IsCancel = this.cbECRCancel.Checked;
                    fncPermissionItem.IsAttachWorkflow = this.cbECRAttachWorkflow.Checked;
                    break;
                case 4:
                    fncPermissionItem.ObjectTypeName = "MANAGEMENT OF CHANGE";
                    fncPermissionItem.IsView = this.cbMOCView.Checked;
                    fncPermissionItem.IsCreate = this.cbMOCCreate.Checked;
                    fncPermissionItem.IsUpdate = this.cbMOCUpdate.Checked;
                    fncPermissionItem.IsCancel = this.cbMOCCancel.Checked;
                    fncPermissionItem.IsAttachWorkflow = this.cbMOCAttachWorkflow.Checked;
                    break;
                case 5:
                    fncPermissionItem.ObjectTypeName = "BREAKDOWN REPORT";
                    fncPermissionItem.IsView = this.cbBRView.Checked;
                    fncPermissionItem.IsCreate = this.cbBRCreate.Checked;
                    fncPermissionItem.IsUpdate = this.cbBRUpdate.Checked;
                    fncPermissionItem.IsCancel = this.cbBRCancel.Checked;
                    fncPermissionItem.IsAttachWorkflow = this.cbBRAttachWorkflow.Checked;
                    break;
                case 6:
                    fncPermissionItem.ObjectTypeName = "SHUTDOWN REPORT";
                    fncPermissionItem.IsView = this.cbSRView.Checked;
                    fncPermissionItem.IsCreate = this.cbSRCreate.Checked;
                    fncPermissionItem.IsUpdate = this.cbSRUpdate.Checked;
                    fncPermissionItem.IsCancel = this.cbSRCancel.Checked;
                    fncPermissionItem.IsAttachWorkflow = this.cbSRAttachWorkflow.Checked;
                    break;
                case 7:
                    fncPermissionItem.ObjectTypeName = "TRACKING OPERATION MEETING";
                    fncPermissionItem.IsView = this.cbOperationMeetingView.Checked;
                    fncPermissionItem.IsCreate = this.cbOperationMeetingCreate.Checked;
                    fncPermissionItem.IsUpdate = this.cbOperationMeetingUpdate.Checked;
                    fncPermissionItem.IsCancel = this.cbOperationMeetingCancel.Checked;
                    break;
                case 8:
                    fncPermissionItem.ObjectTypeName = "TRACKING LIST OF MORNING CALL";
                    fncPermissionItem.IsView = this.cbMorningCallView.Checked;
                    fncPermissionItem.IsCreate = this.cbMorningCallCreate.Checked;
                    fncPermissionItem.IsUpdate = this.cbMorningCallUpdate.Checked;
                    fncPermissionItem.IsCancel = this.cbMorningCallCancel.Checked;
                    break;
                case 9:
                    fncPermissionItem.ObjectTypeName = "TRACKING LIST OF WCR";
                    fncPermissionItem.IsView = this.cbWCRView.Checked;
                    fncPermissionItem.IsCreate = this.cbWCRCreate.Checked;
                    fncPermissionItem.IsUpdate = this.cbWCRUpdate.Checked;
                    fncPermissionItem.IsCancel = this.cbWCRCancel.Checked;
                    break;
                case 10:
                    fncPermissionItem.ObjectTypeName = "TRACKING LIST OF PUNCH LIST";
                    fncPermissionItem.IsView = this.cbPunchListView.Checked;
                    fncPermissionItem.IsCreate = this.cbPunchListCreate.Checked;
                    fncPermissionItem.IsUpdate = this.cbPunchListUpdate.Checked;
                    fncPermissionItem.IsCancel = this.cbPunchListCancel.Checked;
                    break;
                case 11:
                    fncPermissionItem.ObjectTypeName = "TRACKING LIST OF SAIL LIST";
                    fncPermissionItem.IsView = this.cbSailListView.Checked;
                    fncPermissionItem.IsCreate = this.cbSailListCreate.Checked;
                    fncPermissionItem.IsUpdate = this.cbSailListUpdate.Checked;
                    fncPermissionItem.IsCancel = this.cbSailListCancel.Checked;
                    break;
                case 12:
                    fncPermissionItem.ObjectTypeName = "TRACKING LIST OF PROCEDURE";
                    fncPermissionItem.IsView = this.cbProcedureView.Checked;
                    fncPermissionItem.IsCreate = this.cbProcedureCreate.Checked;
                    fncPermissionItem.IsUpdate = this.cbProcedureUpdate.Checked;
                    fncPermissionItem.IsCancel = this.cbProcedureCancel.Checked;
                    break;
                case 13:
                    fncPermissionItem.ObjectTypeName = "GENERAL WORKING";
                    fncPermissionItem.IsView = this.cbGeneralWorkingView.Checked;
                    fncPermissionItem.IsCreate = this.cbGeneralWorkingCreate.Checked;
                    fncPermissionItem.IsUpdate = this.cbGeneralWorkingUpdate.Checked;
                    fncPermissionItem.IsCancel = this.cbGeneralWorkingCancel.Checked;
                    break;
            }

            fncPermissionItem.UpdatedBy = UserSession.Current.User.Id;
            fncPermissionItem.UpdatedByName = UserSession.Current.User.FullName;
            fncPermissionItem.UpdatedDate = DateTime.Now;
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
                    if (item.Text == "Function Permission")
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        protected void lbPIC_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.ClearCheck();
            var userId = Convert.ToInt32(this.lbPIC.SelectedValue);
            var fncPermissionList = this.fncPermissionService.GetAllByUser(userId);
            foreach (var item in fncPermissionList)
            {
                switch (item.ObjectTypeId)
                {
                    case 1:
                        this.cbMRView.Checked = item.IsView.GetValueOrDefault();
                        this.cbMRCreate.Checked = item.IsCreate.GetValueOrDefault();
                        this.cbMRCancel.Checked = item.IsCancel.GetValueOrDefault();
                        this.cbMRUpdate.Checked = item.IsUpdate.GetValueOrDefault();
                        this.cbMRAttachWorkflow.Checked = item.IsAttachWorkflow.GetValueOrDefault();
                        break;
                    case 2:
                        this.cbWRView.Checked = item.IsView.GetValueOrDefault();
                        this.cbWRCreate.Checked = item.IsCreate.GetValueOrDefault();
                        this.cbWRCancel.Checked = item.IsCancel.GetValueOrDefault();
                        this.cbWRUpdate.Checked = item.IsUpdate.GetValueOrDefault();
                        this.cbWRAttachWorkflow.Checked = item.IsAttachWorkflow.GetValueOrDefault();
                        break;
                    case 3:
                        this.cbECRView.Checked = item.IsView.GetValueOrDefault();
                        this.cbECRCreate.Checked = item.IsCreate.GetValueOrDefault();
                        this.cbECRCancel.Checked = item.IsCancel.GetValueOrDefault();
                        this.cbECRUpdate.Checked = item.IsUpdate.GetValueOrDefault();
                        this.cbECRAttachWorkflow.Checked = item.IsAttachWorkflow.GetValueOrDefault();
                        break;
                    case 4:
                        this.cbMOCView.Checked = item.IsView.GetValueOrDefault();
                        this.cbMOCCreate.Checked = item.IsCreate.GetValueOrDefault();
                        this.cbMOCCancel.Checked = item.IsCancel.GetValueOrDefault();
                        this.cbMOCUpdate.Checked = item.IsUpdate.GetValueOrDefault();
                        this.cbMOCAttachWorkflow.Checked = item.IsAttachWorkflow.GetValueOrDefault();
                        break;
                    case 5:
                        this.cbBRView.Checked = item.IsView.GetValueOrDefault();
                        this.cbBRCreate.Checked = item.IsCreate.GetValueOrDefault();
                        this.cbBRCancel.Checked = item.IsCancel.GetValueOrDefault();
                        this.cbBRUpdate.Checked = item.IsUpdate.GetValueOrDefault();
                        this.cbBRAttachWorkflow.Checked = item.IsAttachWorkflow.GetValueOrDefault();
                        break;
                    case 6:
                        this.cbSRView.Checked = item.IsView.GetValueOrDefault();
                        this.cbSRCreate.Checked = item.IsCreate.GetValueOrDefault();
                        this.cbSRCancel.Checked = item.IsCancel.GetValueOrDefault();
                        this.cbSRUpdate.Checked = item.IsUpdate.GetValueOrDefault();
                        this.cbSRAttachWorkflow.Checked = item.IsAttachWorkflow.GetValueOrDefault();
                        break;

                    case 7:
                        this.cbOperationMeetingView.Checked = item.IsView.GetValueOrDefault();
                        this.cbOperationMeetingCreate.Checked = item.IsCreate.GetValueOrDefault();
                        this.cbOperationMeetingCancel.Checked = item.IsCancel.GetValueOrDefault();
                        this.cbOperationMeetingUpdate.Checked = item.IsUpdate.GetValueOrDefault();
                        break;
                    case 8:
                        this.cbMorningCallView.Checked = item.IsView.GetValueOrDefault();
                        this.cbMorningCallCreate.Checked = item.IsCreate.GetValueOrDefault();
                        this.cbMorningCallCancel.Checked = item.IsCancel.GetValueOrDefault();
                        this.cbMorningCallUpdate.Checked = item.IsUpdate.GetValueOrDefault();
                        break;
                    case 9:
                        this.cbWCRView.Checked = item.IsView.GetValueOrDefault();
                        this.cbWCRCreate.Checked = item.IsCreate.GetValueOrDefault();
                        this.cbWCRCancel.Checked = item.IsCancel.GetValueOrDefault();
                        this.cbWCRUpdate.Checked = item.IsUpdate.GetValueOrDefault();
                        break;
                    case 10:
                        this.cbPunchListView.Checked = item.IsView.GetValueOrDefault();
                        this.cbPunchListCreate.Checked = item.IsCreate.GetValueOrDefault();
                        this.cbPunchListCancel.Checked = item.IsCancel.GetValueOrDefault();
                        this.cbPunchListUpdate.Checked = item.IsUpdate.GetValueOrDefault();
                        break;
                    case 11:
                        this.cbSailListView.Checked = item.IsView.GetValueOrDefault();
                        this.cbSailListCreate.Checked = item.IsCreate.GetValueOrDefault();
                        this.cbSailListCancel.Checked = item.IsCancel.GetValueOrDefault();
                        this.cbSailListUpdate.Checked = item.IsUpdate.GetValueOrDefault();
                        break;
                    case 12:
                        this.cbProcedureView.Checked = item.IsView.GetValueOrDefault();
                        this.cbProcedureCreate.Checked = item.IsCreate.GetValueOrDefault();
                        this.cbProcedureCancel.Checked = item.IsCancel.GetValueOrDefault();
                        this.cbProcedureUpdate.Checked = item.IsUpdate.GetValueOrDefault();
                        break;
                    case 13:
                        this.cbGeneralWorkingView.Checked = item.IsView.GetValueOrDefault();
                        this.cbGeneralWorkingCreate.Checked = item.IsCreate.GetValueOrDefault();
                        this.cbGeneralWorkingCancel.Checked = item.IsCancel.GetValueOrDefault();
                        this.cbGeneralWorkingUpdate.Checked = item.IsUpdate.GetValueOrDefault();
                        break;
                }
            }
        }

        protected void ddlDept_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var userList = this.userService.GetAllByRoleId(Convert.ToInt32(this.ddlDept.SelectedValue)).OrderByDescending(t => t.IsDeptManager).ThenBy(t => t.FullNameWithPosition);

            this.lbPIC.DataSource = userList;
            this.lbPIC.DataTextField = "FullNameWithPosition";
            this.lbPIC.DataValueField = "Id";
            this.lbPIC.DataBind();

            this.ClearCheck();
        }

        private void ClearCheck()
        {
            this.cbMRView.Checked = false;
            this.cbMRCreate.Checked = false;
            this.cbMRCancel.Checked = false;
            this.cbMRUpdate.Checked = false;
            this.cbMRAttachWorkflow.Checked = false;

            this.cbWRView.Checked = false;
            this.cbWRCreate.Checked = false;
            this.cbWRCancel.Checked = false;
            this.cbWRUpdate.Checked = false;
            this.cbWRAttachWorkflow.Checked = false;

            this.cbECRView.Checked = false;
            this.cbECRCreate.Checked = false;
            this.cbECRCancel.Checked = false;
            this.cbECRUpdate.Checked = false;
            this.cbECRAttachWorkflow.Checked = false;

            this.cbMOCView.Checked = false;
            this.cbMOCCreate.Checked = false;
            this.cbMOCCancel.Checked = false;
            this.cbMOCUpdate.Checked = false;
            this.cbMOCAttachWorkflow.Checked = false;

            this.cbBRView.Checked = false;
            this.cbBRCreate.Checked = false;
            this.cbBRCancel.Checked = false;
            this.cbBRUpdate.Checked = false;
            this.cbBRAttachWorkflow.Checked = false;

            this.cbSRView.Checked = false;
            this.cbSRCreate.Checked = false;
            this.cbSRCancel.Checked = false;
            this.cbSRUpdate.Checked = false;
            this.cbSRAttachWorkflow.Checked = false;

            this.cbOperationMeetingView.Checked = false;
            this.cbOperationMeetingCreate.Checked = false;
            this.cbOperationMeetingCancel.Checked = false;
            this.cbOperationMeetingUpdate.Checked = false;

            this.cbMorningCallView.Checked = false;
            this.cbMorningCallCreate.Checked = false;
            this.cbMorningCallCancel.Checked = false;
            this.cbMorningCallUpdate.Checked = false;

            this.cbWCRView.Checked = false;
            this.cbWCRCreate.Checked = false;
            this.cbWCRCancel.Checked = false;
            this.cbWCRUpdate.Checked = false;

            this.cbPunchListView.Checked = false;
            this.cbPunchListCreate.Checked = false;
            this.cbPunchListCancel.Checked = false;
            this.cbPunchListUpdate.Checked = false;

            this.cbSailListView.Checked = false;
            this.cbSailListCreate.Checked = false;
            this.cbSailListCancel.Checked = false;
            this.cbSailListUpdate.Checked = false;

            this.cbProcedureView.Checked = false;
            this.cbProcedureCreate.Checked = false;
            this.cbProcedureCancel.Checked = false;
            this.cbProcedureUpdate.Checked = false;

            this.cbGeneralWorkingView.Checked = false;
            this.cbGeneralWorkingCreate.Checked = false;
            this.cbGeneralWorkingCancel.Checked = false;
            this.cbGeneralWorkingUpdate.Checked = false;
        }
    }
}

