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
    using System.Web;
    using System.Text;
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

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class DepartmentEditForm : Page
    {
        /// <summary>
        /// the folder service
        /// </summary>
        private readonly GroupDataPermissionService groupDataPermissionService;
        /// <summary>
        /// the category service
        /// </summary>
        private readonly CategoryService categoryService;

        /// <summary>
        /// the department service
        /// </summary>
      
        private readonly DepartmentService departmentService;
        
        
        private readonly ScopeProjectService scopeProjectService;


        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public DepartmentEditForm()
        {
            this.userService = new UserService();    
            this.groupDataPermissionService = new GroupDataPermissionService();
            this.categoryService = new CategoryService();
            this.scopeProjectService = new ScopeProjectService();
            this.departmentService = new  DepartmentService ();
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
                if(!string.IsNullOrEmpty(this.Request.QueryString["Id"])){
                    var Obj = this.departmentService.GetById(Convert.ToInt32(this.Request.QueryString["Id"]));
                    if(Obj!=null){
                        this.txtName.Text = Obj.Name;
                        this.txtDescription.Text = Obj.Description;
                        this.ddlLocation.SelectedValue = Obj.TypeId.ToString();
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["Id"]))
                {
                    var platId = Convert.ToInt32(this.Request.QueryString["Id"]);
                    var obj = this.departmentService.GetById(platId);
                    if (obj != null)
                    {
                        obj.Name = this.txtName.Text;
                        obj.Description = this.txtDescription.Text.Trim();
                        obj.TypeId = Convert.ToInt32(this.ddlLocation.SelectedValue);
                        obj.TypeName = this.ddlLocation.SelectedItem.Text;
                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedDate = DateTime.Now;
                        this.departmentService.Update(obj);
                    }
                }
                else
                {
                    var department = new Department();

                    department.Name = this.txtName.Text;
                    department.Description = this.txtDescription.Text.Trim();
                    department.TypeName = this.ddlLocation.SelectedItem.Text;
                    department.TypeId = Convert.ToInt32(this.ddlLocation.SelectedValue);
                    department.CreatedBy = UserSession.Current.User.Id;
                    department.CreatedDate = DateTime.Now;

                    
                     this.departmentService.Insert(department);
                    
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
                this.fileNameValidator.ErrorMessage = "Please enter department name.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        
    }
}