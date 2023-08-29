// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Web.Controls.Library
{
    using System;
    using System.Configuration;
    using System.Linq;
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
    public partial class StatusEditForm : Page
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
        private readonly StatusService statusService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public StatusEditForm()
        {
            this.userService = new UserService();
            this.statusService = new StatusService();
            this.groupDataPermissionService = new GroupDataPermissionService();
            this.categoryService = new CategoryService();
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

                    var objDis = this.statusService.GetById(Convert.ToInt32(this.Request.QueryString["disId"]));
                    if (objDis != null)
                    {

                        this.txtName.Text = objDis.Name;
                        this.txtDescription.Text = objDis.Description;

                        if (objDis.CategoryId != null)
                        {
                            this.ddlCategory.SelectedValue = objDis.CategoryId.ToString();
                        }

                        var createdUser = this.userService.GetByID(objDis.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + objDis.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (objDis.LastUpdatedBy != null && objDis.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(objDis.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + objDis.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
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
                    var disId = Convert.ToInt32(this.Request.QueryString["disId"]);
                    var obj = this.statusService.GetById(disId);
                    if (obj != null)
                    {
                        obj.Name = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        obj.CategoryId = !string.IsNullOrEmpty(this.ddlCategory.SelectedValue) ? Convert.ToInt32(this.ddlCategory.SelectedValue) : (int?)null;

                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedDate = DateTime.Now;

                        this.statusService.Update(obj);
                    }
                }
                else
                {
                    var obj = new Status()
                    {
                        Name = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        CategoryId = !string.IsNullOrEmpty(this.ddlCategory.SelectedValue) ? Convert.ToInt32(this.ddlCategory.SelectedValue) : (int?)null,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now
                    };

                    this.statusService.Insert(obj);
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
            var categoryPermission = this.groupDataPermissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault()).Select(t => t.CategoryIdList).ToList();
            var listCategory = this.categoryService.GetAll().Where(t => categoryPermission.Any(x => x == t.ID.ToString())).ToList();

            listCategory.Insert(0, new Category());

            this.ddlCategory.DataSource = listCategory;
            this.ddlCategory.DataTextField = "Name";
            this.ddlCategory.DataValueField = "ID";
            this.ddlCategory.DataBind();
        }
    }
}