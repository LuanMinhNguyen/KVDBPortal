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
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using EDMs.Web.Controls.Document;
using EDMs.Web.Utilities.Sessions;

namespace EDMs.Web.Controls.Workflow
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class WorkflowEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly WorkflowService wfService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly ProjectCodeService projectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public WorkflowEditForm()
        {
            this.userService = new UserService();
            this.wfService = new WorkflowService();
            this.projectService = new ProjectCodeService();
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
                this.LoadComboData();
                
                if (!string.IsNullOrEmpty(this.Request.QueryString["disId"]))
                {
                    var obj = this.wfService.GetById(Convert.ToInt32(this.Request.QueryString["disId"]));
                    this.CreatedInfo.Visible = obj != null;

                    if (obj != null)
                    {
                        this.txtName.Text = obj.Name;
                        this.txtDescription.Text = obj.Description;
                        this.ddlProject.SelectedValue = obj.ProjectID.GetValueOrDefault().ToString();
                        this.cbInternalWorkflow.Checked = obj.IsInternalWorkflow.GetValueOrDefault();
                        this.ddlObject.SelectedValue = obj.ObjectTypeId.GetValueOrDefault().ToString();
                        this.ddlReAssignUser.SelectedValue = obj.Re_assignUserId.GetValueOrDefault().ToString();
                        var createdUser = this.userService.GetByID(obj.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + obj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (obj.UpdatedBy != null && obj.UpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(obj.UpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + obj.UpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }
                    }
                }
            }
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["disId"]))
                {
                    var documentStatusId = Convert.ToInt32(this.Request.QueryString["disId"]);
                    var obj = this.wfService.GetById(documentStatusId);
                    if (obj != null)
                    {
                        obj.Name = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        obj.ProjectID = Convert.ToInt32(this.ddlProject.SelectedValue);
                        obj.ProjectName = this.ddlProject.SelectedItem.Text;
                        obj.UpdatedBy = UserSession.Current.User.Id;
                        obj.UpdatedDate = DateTime.Now;
                        obj.IsInternalWorkflow = this.cbInternalWorkflow.Checked;
                        obj.Re_assignUserId = Convert.ToInt32(this.ddlReAssignUser.SelectedValue);
                        obj.Re_assignUserName = this.ddlReAssignUser.SelectedItem.Text;
                        obj.ObjectTypeId = Convert.ToInt32(this.ddlObject.SelectedValue);
                        obj.ObjectTypeName = this.ddlObject.SelectedItem.Text;
                        this.wfService.Update(obj);
                    }
                }
                else
                {
                    var obj = new Data.Entities.Workflow()
                    {
                        Name = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        ProjectID = Convert.ToInt32(this.ddlProject.SelectedValue),
                        ProjectName = this.ddlProject.SelectedItem.Text,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,
                        IsInternalWorkflow = this.cbInternalWorkflow.Checked,
                       Re_assignUserId = Convert.ToInt32(this.ddlReAssignUser.SelectedValue),
                        Re_assignUserName = this.ddlReAssignUser.SelectedItem.Text,
                        ObjectTypeId = Convert.ToInt32(this.ddlObject.SelectedValue),
                        ObjectTypeName = this.ddlObject.SelectedItem.Text
                    };

                    this.wfService.Insert(obj);
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
            if(this.txtName.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter Discipline name.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        private void LoadComboData()
        {
            var userList = this.userService.GetAll().Where(t => t.Id != 1).ToList();
            userList.Insert(0, new User() {Id = 0});
            this.ddlReAssignUser.DataSource = userList.OrderBy(t=> t.UserNameWithFullName);
            this.ddlReAssignUser.DataTextField = "UserNameWithFullName";
            this.ddlReAssignUser.DataValueField = "Id";
            this.ddlReAssignUser.DataBind();

            var projectList = this.projectService.GetAll().OrderBy(t => t.FullName);
            this.ddlProject.DataSource = projectList;
            this.ddlProject.DataTextField = "FullName";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();
        }
    }
}