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
    public partial class PackageEditForm : Page
    {
        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly GroupDataPermissionService groupDataPermissionService;

        /// <summary>
        /// The category service.
        /// </summary>
        private readonly CategoryService categoryService;

        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly PackageService PackageService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly ScopeProjectService scopeProjectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public PackageEditForm()
        {
            this.userService = new UserService();
            this.PackageService = new PackageService();
            this.groupDataPermissionService = new GroupDataPermissionService();
            this.categoryService = new CategoryService();
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
                var projectInPermission = this.scopeProjectService.GetAll();
                this.ddlProject.DataSource = projectInPermission;
                this.ddlProject.DataTextField = "Name";
                this.ddlProject.DataValueField = "ID";
                this.ddlProject.DataBind();

                if (!string.IsNullOrEmpty(this.Request.QueryString["disId"]))
                {
                    var objPackage = this.PackageService.GetById(Convert.ToInt32(this.Request.QueryString["disId"]));
                    if (objPackage != null)
                    {
                        this.txtName.Text = objPackage.Name;
                        this.txtDescription.Text = objPackage.Description;
                      //  this.txtComplete.Value = objPackage.Complete;
                      //  this.txtWeight.Value = objPackage.Weight;
                        this.ddlProject.SelectedValue = objPackage.ProjectId.ToString();
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
                    var PackageId = Convert.ToInt32(this.Request.QueryString["disId"]);
                    var obj = this.PackageService.GetById(PackageId);
                    if (obj != null)
                    {
                        obj.Name = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                     //   obj.Complete = this.txtComplete.Value.GetValueOrDefault();
                      //  obj.Weight = this.txtWeight.Value.GetValueOrDefault();
                        obj.ProjectId = Convert.ToInt32(this.ddlProject.SelectedValue);
                        obj.ProjectName = this.ddlProject.SelectedItem.Text;

                        this.PackageService.Update(obj);
                    }
                }
                else
                {
                    var obj = new Package()
                    {
                        Name = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                       // Weight = this.txtWeight.Value.GetValueOrDefault(),
                       // Complete = this.txtComplete.Value.GetValueOrDefault(),
                        ProjectId = Convert.ToInt32(this.ddlProject.SelectedValue),
                        ProjectName = this.ddlProject.SelectedItem.Text
                    };

                    this.PackageService.Insert(obj);
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
    }
}