// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using EDMs.Business.Services.Scope;

namespace EDMs.Web.Controls.Scope
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Controls.Document;
    using EDMs.Web.Utilities.Sessions;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class DocumentStatusEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly StatusService documentStatusService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly ScopeProjectService projectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public DocumentStatusEditForm()
        {
            this.userService = new UserService();
            this.documentStatusService = new StatusService();
            this.projectService = new ScopeProjectService();
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
                    var objDocumentStatus = this.documentStatusService.GetById(Convert.ToInt32(this.Request.QueryString["disId"]));
                    this.CreatedInfo.Visible = objDocumentStatus != null;

                    if (objDocumentStatus != null)
                    {
                        this.txtName.Text = objDocumentStatus.Name;
                        this.txtDescription.Text = objDocumentStatus.Description;
                        this.ddlProject.SelectedValue = objDocumentStatus.ProjectID.GetValueOrDefault().ToString();
                        this.txtPercentCompleteDefault.Value = objDocumentStatus.PercentCompleteDefault;
                        var createdUser = this.userService.GetByID(objDocumentStatus.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + objDocumentStatus.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (objDocumentStatus.LastUpdatedBy != null && objDocumentStatus.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(objDocumentStatus.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + objDocumentStatus.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
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
                    var obj = this.documentStatusService.GetById(documentStatusId);
                    if (obj != null)
                    {
                        obj.Name = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        obj.ProjectID = Convert.ToInt32(this.ddlProject.SelectedValue);
                        obj.ProjectName = this.ddlProject.SelectedItem.Text;
                        obj.PercentCompleteDefault = this.txtPercentCompleteDefault.Value;
                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedDate = DateTime.Now;
                        this.documentStatusService.Update(obj);
                    }
                }
                else
                {
                    var obj = new Status()
                    {
                        Name = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        ProjectID = Convert.ToInt32(this.ddlProject.SelectedValue),
                        ProjectName = this.ddlProject.SelectedItem.Text,
                        PercentCompleteDefault = this.txtPercentCompleteDefault.Value,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now
                    };

                    this.documentStatusService.Insert(obj);
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
            var projectList = this.projectService.GetAll();
            this.ddlProject.DataSource = projectList;
            this.ddlProject.DataTextField = "FullName";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();
        }
    }
}