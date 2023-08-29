// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using EDMs.Business.Services.Document;
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
    public partial class ConfidentialityEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly ConfidentialityService confidentialityService;

        private readonly ProcessActualService processActualService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisciplineEditForm"/> class.
        /// </summary>
        public ConfidentialityEditForm()
        {
            this.userService = new UserService();
            this.confidentialityService = new ConfidentialityService();
            this.processActualService = new ProcessActualService();
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["confidentialityId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var objConfidentiality = this.confidentialityService.GetById(Convert.ToInt32(this.Request.QueryString["confidentialityId"]));
                    if (objConfidentiality != null)
                    {

                        this.txtCode.Text = objConfidentiality.Code;
                        this.txtDescription.Text = objConfidentiality.Description;
                        var createdUser = this.userService.GetByID(objConfidentiality.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + objConfidentiality.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (objConfidentiality.LastUpdatedBy != null && objConfidentiality.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(objConfidentiality.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + objConfidentiality.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["confidentialityId"]))
                {
                    var confidentialityId = Convert.ToInt32(this.Request.QueryString["confidentialityId"]);
                    var objConfidentiality = this.confidentialityService.GetById(confidentialityId);
                    if (objConfidentiality != null)
                    {
                        objConfidentiality.Code = this.txtCode.Text.Trim();
                        objConfidentiality.Description = this.txtDescription.Text.Trim();
                        objConfidentiality.LastUpdatedBy = UserSession.Current.User.Id;
                        objConfidentiality.LastUpdatedDate = DateTime.Now;

                        this.confidentialityService.Update(objConfidentiality);
                        
                    }
                }
                else
                {
                    var objConfidentiality = new Confidentiality()
                    {
                        Code = this.txtCode.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now
                    };

                    this.confidentialityService.Insert(objConfidentiality);
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
            if(this.txtCode.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter Confidentiality.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        private void LoadComboData()
        {
            //var projectInPermission = this.scopeProjectService.GetAll();
            //this.ddlProject.DataSource = projectInPermission;
            //this.ddlProject.DataTextField = "FullName";
            //this.ddlProject.DataValueField = "ID";
            //this.ddlProject.DataBind();
        }
    }
}