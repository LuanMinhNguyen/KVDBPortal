// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using EDMs.Business.Services.Scope;

namespace EDMs.Web.Controls.Library
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class OriginatorEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly OriginatorService OriginatorService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly ScopeProjectService scopeProjectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OriginatorEditForm"/> class.
        /// </summary>
        public OriginatorEditForm()
        {
            this.userService = new UserService();
            this.OriginatorService = new OriginatorService();
            this.scopeProjectService = new ScopeProjectService();
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
                    this.CreatedInfo.Visible = true;

                    var objOriginator = this.OriginatorService.GetById(Convert.ToInt32(this.Request.QueryString["disId"]));
                    if (objOriginator != null)
                    {
                        this.txtName.Text = objOriginator.Name;
                        this.txtDescription.Text = objOriginator.Description;
                        this.ddlProject.SelectedValue = objOriginator.ProjectId.GetValueOrDefault().ToString();
                        this.ckbActive.Checked = objOriginator.Active.GetValueOrDefault();
                        var createdUser = this.userService.GetByID(objOriginator.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + objOriginator.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (objOriginator.LastUpdatedBy != null && objOriginator.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(objOriginator.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + objOriginator.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }
                    }
                }
                else
                {
                    this.CreatedInfo.Visible = false;
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
                    var OriginatorId = Convert.ToInt32(this.Request.QueryString["disId"]);
                    var obj = this.OriginatorService.GetById(OriginatorId);
                    if (obj != null)
                    {
                        obj.Name = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        obj.Active = this.ckbActive.Checked;
                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedDate = DateTime.Now;
                        obj.ProjectId = Convert.ToInt32(this.ddlProject.SelectedValue);
                        obj.ProjectName = this.ddlProject.SelectedValue == "0" ? "GENERAL" : this.ddlProject.SelectedItem.Text;
                        this.OriginatorService.Update(obj);
                    }
                }
                else
                {
                    var obj = new Originator()
                    {
                        Name = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        Active = this.ckbActive.Checked,
                        ProjectId = Convert.ToInt32(this.ddlProject.SelectedValue),
                        ProjectName = this.ddlProject.SelectedValue == "0" ? "GENERAL" : this.ddlProject.SelectedItem.Text,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now
                    };

                    this.OriginatorService.Insert(obj);
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
            var projectInPermission = this.scopeProjectService.GetAll();
            this.ddlProject.DataSource = projectInPermission;
            this.ddlProject.DataTextField = "FullName";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();
        }
    }
}