// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.WMS;
using EDMs.Data.Entities;
using EDMs.Web.Utilities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.WMS
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class WREditForm : Page
    {
        private readonly MRTypeService mrTypeService;

        private readonly PriorityLevelService priorityLevelService;

        private readonly RoleService roleService;

        private readonly WorkRequestService wrService;

        private readonly ScopeProjectService projectService;

        private readonly NumberManagementService numberManagementService;

        private readonly FunctionPermissionService fncPermissionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MREditForm"/> class.
        /// </summary>
        public WREditForm()
        {
            this.mrTypeService = new MRTypeService();
            this.priorityLevelService = new PriorityLevelService();
            this.roleService = new RoleService();
            this.wrService = new WorkRequestService();
            this.projectService = new ScopeProjectService();
            this.numberManagementService = new NumberManagementService();
            this.fncPermissionService = new FunctionPermissionService();
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
                this.GetFuncPermissionConfig();
                this.LoadInitData();

                if(!string.IsNullOrEmpty(this.Request.QueryString["wrId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var wrObj = this.wrService.GetById(new Guid(this.Request.QueryString["wrId"]));
                    if(wrObj!=null)
                    {
                        this.txtWRNo.Text = wrObj.WRNo;
                        this.txtWRTitle.Text = wrObj.WRTitle;
                        this.ddlDepartment.SelectedValue = wrObj.DepartmentId.ToString();
                        this.txtOriginator.Text = wrObj.OriginatorName;
                        this.txtJobTitle.Text = wrObj.OriginatorJobTitle;
                        this.txtRaiseDate.SelectedDate = wrObj.RaisedDate;
                        this.txtDateRequire.Text = wrObj.RequriedDate;
                        this.txtDescription.Text = wrObj.Description;
                        this.txtScopeOfService.Text = wrObj.ScopeOfService;
                        this.txtReason.Text = wrObj.Reason;
                        this.ddlPriorityLvl.SelectedValue = wrObj.PriotiyLevelId.ToString();

                        this.lblCreated.Text = "Created at " + wrObj.RaisedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + wrObj.OriginatorName;
                        if (wrObj.UpdatedBy != null && wrObj.UpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            this.lblUpdated.Text = "Last modified at " + wrObj.UpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + wrObj.UpdatedByName;
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }
                    }
                }
                else
                {
                    var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
                    var projectObj = this.projectService.GetById(projectId);
                    var currentWRIndex = this.numberManagementService.GetByName("WorkRequest", projectId);
                    //var wrNo = projectObj.Code + "/" + Utility.ReturnSequenceString(currentWRIndex.NextCount.GetValueOrDefault(), 4) + "/" + DateTime.Now.ToString("yy");
                    var wrNo = Utility.ReturnSequenceString(currentWRIndex.NextCount.GetValueOrDefault(), 4) + "/" + DateTime.Now.ToString("yy");
                    this.txtWRNo.Text = wrNo;
                    this.txtOriginator.Text = UserSession.Current.User.FullName;
                    this.txtRaiseDate.SelectedDate = DateTime.Now;
                    
                    this.CreatedInfo.Visible = false;
                }
            }
        }

        private void LoadInitData()
        {
            var priorityLvl = this.priorityLevelService.GetAll();
            priorityLvl.Insert(0, new PriorityLevel() {ID = 0});
            this.ddlPriorityLvl.DataSource = priorityLvl;
            this.ddlPriorityLvl.DataTextField = "Name";
            this.ddlPriorityLvl.DataValueField = "ID";
            this.ddlPriorityLvl.DataBind();

            var offShoreDetpList = this.roleService.GetAll(false).Where(t => t.TypeId == 2);
            this.ddlDepartment.DataSource = offShoreDetpList;
            this.ddlDepartment.DataTextField = "Name";
            this.ddlDepartment.DataValueField = "ID";
            this.ddlDepartment.DataBind();

            if (UserSession.Current.User.RoleId != null)
            {
                this.ddlDepartment.SelectedValue = UserSession.Current.User.RoleId.ToString();
            }

            var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
            var projectObj = this.projectService.GetById(projectId);

            this.txtFacility.Text = projectObj.Name;

            // Show hide function Control
            if (!string.IsNullOrEmpty(this.Request.QueryString["wrId"]))
            {
                this.btnSave.Visible = Convert.ToBoolean(this.IsUpdate.Value);
            }
            else
            {
                this.btnSave.Visible = Convert.ToBoolean(this.IsCreate.Value);
            }
            
            // --------------------------------------------------------------------------
        }

        private void GetFuncPermissionConfig()
        {
            // Get Function permission of user for MR Module
            var fncPermission = this.fncPermissionService.GetOne(UserSession.Current.User.Id, 2);
            if (fncPermission != null)
            {
                this.IsView.Value = fncPermission.IsView.GetValueOrDefault().ToString();
                this.IsCreate.Value = fncPermission.IsCreate.GetValueOrDefault().ToString();
                this.IsUpdate.Value = fncPermission.IsUpdate.GetValueOrDefault().ToString();
                this.IsCancel.Value = fncPermission.IsCancel.GetValueOrDefault().ToString();
                this.IsAttachWF.Value = fncPermission.IsAttachWorkflow.GetValueOrDefault().ToString();
            }
            else
            {
                this.IsView.Value = "False";
                this.IsCreate.Value = "False";
                this.IsUpdate.Value = "False";
                this.IsCancel.Value = "False";
                this.IsAttachWF.Value = "False";
            }
            // ----------------------------------------------------------------------------------------
        }

        /// <summary>
        /// The btn cap nhat_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.Page.IsValid)
            {
                var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
                var projectObj = this.projectService.GetById(projectId);

                if (!string.IsNullOrEmpty(this.Request.QueryString["wrId"]))
                {
                    var wrId = new Guid(this.Request.QueryString["wrId"]);
                    var wrObj = this.wrService.GetById(wrId);
                    if (wrObj != null)
                    {
                        wrObj.ProjectId = projectObj.ID;
                        wrObj.ProjectName = projectObj.Name;

                        wrObj.WRNo = this.txtWRNo.Text.Trim();
                        wrObj.WRTitle = this.txtWRTitle.Text.Trim();
                        wrObj.DepartmentId = Convert.ToInt32(this.ddlDepartment.SelectedValue);
                        wrObj.DepartmentName = this.ddlDepartment.SelectedItem.Text;
                        //wrObj.OriginatorId = UserSession.Current.User.Id;
                        wrObj.OriginatorName = this.txtOriginator.Text;
                        wrObj.OriginatorJobTitle = this.txtJobTitle.Text.Trim();
                        wrObj.RaisedDate = this.txtRaiseDate.SelectedDate;
                        wrObj.Description = this.txtDescription.Text;
                        wrObj.ScopeOfService = this.txtScopeOfService.Text.Trim();
                        wrObj.Reason = this.txtReason.Text;
                        wrObj.RequriedDate = this.txtDateRequire.Text.Trim();
                        wrObj.PriotiyLevelId = Convert.ToInt32(this.ddlPriorityLvl.SelectedValue);
                        wrObj.PriorityLevelName = this.ddlPriorityLvl.SelectedItem.Text;

                        wrObj.UpdatedBy = UserSession.Current.User.Id;
                        wrObj.UpdatedByName = UserSession.Current.User.FullName;
                        wrObj.UpdatedDate = DateTime.Now;

                        this.wrService.Update(wrObj);
                    }
                }
                else
                {
                    var wrObj = new WorkRequest
                    {
                        ID = Guid.NewGuid(),
                        ProjectId = projectObj.ID,
                        ProjectName = projectObj.Name,
                        WRNo = this.txtWRNo.Text.Trim(),
                        WRTitle = this.txtWRTitle.Text.Trim(),
                        DepartmentId = Convert.ToInt32(this.ddlDepartment.SelectedValue),
                        DepartmentName = this.ddlDepartment.SelectedItem.Text,
                        //OriginatorId = UserSession.Current.User.Id,
                        OriginatorName = this.txtOriginator.Text,
                        OriginatorJobTitle = this.txtJobTitle.Text.Trim(),
                        RaisedDate = this.txtRaiseDate.SelectedDate,
                        Description = this.txtDescription.Text,
                        ScopeOfService = this.txtScopeOfService.Text.Trim(),
                        Reason = this.txtReason.Text,
                        RequriedDate = this.txtDateRequire.Text.Trim(),
                        PriotiyLevelId = Convert.ToInt32(this.ddlPriorityLvl.SelectedValue),
                        PriorityLevelName = this.ddlPriorityLvl.SelectedItem.Text,
                        IsWFComplete = false,
                        IsInWFProcess = false
                    };
                    this.wrService.Insert(wrObj);

                    var currentWRIndex = this.numberManagementService.GetByName("WorkRequest", projectId);
                    currentWRIndex.NextCount += 1;
                    this.numberManagementService.Update(currentWRIndex);
                }
                
                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
        }

        /// <summary>
        /// The btncancel_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btncancel_Click(object sender, EventArgs e)
        {
            this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CancelEdit();", true);
        }

        /// <summary>
        /// The server validation file name is exist.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void ServerValidationFileNameIsExist(object source, ServerValidateEventArgs args)
        {
            if(this.txtWRNo.Text.Trim().Length == 0)
            {
                args.IsValid = false;
            }
        }

        
    }
}