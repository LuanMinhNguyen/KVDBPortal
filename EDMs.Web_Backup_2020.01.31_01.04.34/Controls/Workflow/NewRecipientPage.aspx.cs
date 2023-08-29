// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Workflow
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class NewRecipientPage : Page
    {

        private readonly WorkflowStepService wfStepService = new WorkflowStepService();
        private readonly WorkflowService wfService = new WorkflowService();
        private readonly WorkflowDetailService wfDetailService = new WorkflowDetailService();
        private readonly ScopeProjectService projectService = new ScopeProjectService();

        private readonly RoleService roleService = new RoleService();
        private readonly UserService userService = new UserService();

        private readonly TitleService titleService = new TitleService();
        private int WorkflowDetailId
        {
            get
            {
                return Convert.ToInt32(this.Request.QueryString["wfdId"]);
            }
        }

        private Data.Entities.WorkflowDetail WorkflowDetailObj
        {
            get { return this.wfDetailService.GetById(this.WorkflowDetailId); }
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
            if (!this.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["wfdId"]))
                {
                    if (this.WorkflowDetailObj != null)
                    {
                        this.LoadListBoxData(this.WorkflowDetailObj);
                    }
                }
            }
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }

        private void LoadListBoxData(WorkflowDetail wfDetailobj)
        {
            var wfStepObj = this.wfStepService.GetById(wfDetailobj.CurrentWorkflowStepID.GetValueOrDefault());
            if (wfStepObj != null)
            {
                var userList = wfStepObj.LocationId == 2 
                            ? this.userService.GetAll().Where(t => t.LocationId == 2 && t.ProjectId == wfStepObj.ProjectID).OrderBy(t => t.FullNameWithDeptPosition).ToList() 
                            : this.userService.GetAll().Where(t => t.LocationId == 1).OrderBy(t => t.FullNameWithDeptPosition).ToList();

                var selectedWorkingUserId = wfDetailobj.AssignUserIDs.Split('$').Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t));
                var selectedInfoOnlyUserId = wfDetailobj.InformationOnlyUserIDs.Split('$').Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t));
                var selectedManagementUserId = wfDetailobj.ManagementUserIds.Split('$').Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t));
                // Fill lbAssignment
                foreach (var user in userList.Where(t => !selectedWorkingUserId.Contains(t.Id)))
                {
                    var userItem = new RadListBoxItem()
                    {
                        Value = user.Id.ToString(),
                        Text = user.FullNameWithDeptPosition,
                        ImageUrl = "~/Images/title.png"
                    };

                    this.lbAssignment.Items.Add(userItem);
                }

                foreach (var user in userList.Where(t => selectedWorkingUserId.Contains(t.Id)))
                {
                    var userItem = new RadListBoxItem()
                    {
                        Value = user.Id.ToString(),
                        Text = user.FullNameWithDeptPosition,
                        ImageUrl = "~/Images/title.png"
                    };

                    this.lbAssignmentSelected.Items.Add(userItem);
                }
                // ------------------------------------------------------------------------------

                // Fill lbInformationOnly
                foreach (var user in userList.Where(t => !selectedInfoOnlyUserId.Contains(t.Id)))
                {
                    var userItem = new RadListBoxItem()
                    {
                        Value = user.Id.ToString(),
                        Text = user.FullNameWithDeptPosition,
                        ImageUrl = "~/Images/title.png"
                    };

                    this.lbInfoOnly.Items.Add(userItem);
                }

                foreach (var user in userList.Where(t => selectedInfoOnlyUserId.Contains(t.Id)))
                {
                    var userItem = new RadListBoxItem()
                    {
                        Value = user.Id.ToString(),
                        Text = user.FullNameWithDeptPosition,
                        ImageUrl = "~/Images/title.png"
                    };

                    this.lbInfoOnlySelected.Items.Add(userItem);
                }
                // ------------------------------------------------------------------------------

                // Fill lbManagement
                foreach (var user in userList.Where(t => !selectedManagementUserId.Contains(t.Id)))
                {
                    var userItem = new RadListBoxItem()
                    {
                        Value = user.Id.ToString(),
                        Text = user.FullNameWithDeptPosition,
                        ImageUrl = "~/Images/title.png"
                    };

                    this.lbManagement.Items.Add(userItem);
                }

                foreach (var user in userList.Where(t => selectedManagementUserId.Contains(t.Id)))
                {
                    var userItem = new RadListBoxItem()
                    {
                        Value = user.Id.ToString(),
                        Text = user.FullNameWithDeptPosition,
                        ImageUrl = "~/Images/title.png"
                    };

                    this.lbManagementSelected.Items.Add(userItem);
                }
                // ------------------------------------------------------------------------------

            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            var selectedManagementUserIds = string.Empty;
            var selectedInfoOnlyUserIds = string.Empty;
            var selectedAssignUserIds = string.Empty;
            selectedAssignUserIds = this.lbAssignmentSelected.Items.Aggregate(selectedAssignUserIds, (current, t) => current + t.Value + "$");
            selectedInfoOnlyUserIds = this.lbInfoOnlySelected.Items.Aggregate(selectedInfoOnlyUserIds, (current, t) => current + t.Value + "$");
            selectedManagementUserIds = this.lbManagementSelected.Items.Aggregate(selectedManagementUserIds, (current, t) => current + t.Value + "$");

            this.WorkflowDetailObj.AssignUserIDs = selectedAssignUserIds;
            this.WorkflowDetailObj.InformationOnlyUserIDs = selectedInfoOnlyUserIds;
            this.WorkflowDetailObj.ManagementUserIds = selectedManagementUserIds;

            this.WorkflowDetailObj.Recipients = string.Empty;

            if (this.lbAssignmentSelected.Items.Count > 0)
            {
                this.WorkflowDetailObj.Recipients += "W - " + this.lbAssignmentSelected.Items[0].Text + "</br>";
            }

            if (this.lbInfoOnlySelected.Items.Count > 0)
            {
                this.WorkflowDetailObj.Recipients += "I - " + this.lbInfoOnlySelected.Items[0].Text + "</br>";
            }

            if (this.lbManagementSelected.Items.Count > 0)
            {
                this.WorkflowDetailObj.Recipients += "M - " + this.lbManagementSelected.Items[0].Text + "</br>";
            }
            this.WorkflowDetailObj.Recipients += "...";

            this.wfDetailService.Update(this.WorkflowDetailObj);
        }

        protected void btnManagementSearch_OnClick(object sender, EventArgs e)
        {
            
        }

        protected void btnInfoSearch_OnClick(object sender, EventArgs e)
        {
            
        }

        protected void btnAssignSearch_OnClick(object sender, EventArgs e)
        {
            
        }
    }
}