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
    public partial class PlatformEditForm : Page
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
        /// the platform service
        /// </summary>
        private readonly PlatformService platformService;
        
        
        private readonly ScopeProjectService scopeProjectService;


        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public PlatformEditForm()
        {
            this.userService = new UserService();    
            this.groupDataPermissionService = new GroupDataPermissionService();
            this.categoryService = new CategoryService();
            this.scopeProjectService = new ScopeProjectService();
            this.platformService = new PlatformService();
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
                if(!string.IsNullOrEmpty(this.Request.QueryString["platId"])){
                    var Obj = this.platformService.GetById(Convert.ToInt32(this.Request.QueryString["platId"]));
                    if(Obj!=null){
                        this.txtName.Text = Obj.Name;
                        this.txtDescription.Text = Obj.Description;
                        this.ddlProject.SelectedValue = Obj.ProjectId.ToString();
                        
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["platId"]))
                {
                    var platId = Convert.ToInt32(this.Request.QueryString["platId"]);
                    var obj = this.platformService.GetById(platId);
                    if (obj != null)
                    {
                        obj.Name = this.txtName.Text;
                        obj.Description = this.txtDescription.Text.Trim();
                        obj.ProjectId = Convert.ToInt32(this.ddlProject.SelectedValue);
                        obj.ProjectName = this.ddlProject.SelectedItem.Text;
                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedDate = DateTime.Now;
                        this.platformService.Update(obj);
                    }
                }
                else
                {
                    var platform = new Platform();

                    platform.Name = this.txtName.Text;
                    platform.Description = this.txtDescription.Text.Trim();
                    platform.ProjectName = this.ddlProject.SelectedItem.Text;
                    platform.ProjectId = Convert.ToInt32(this.ddlProject.SelectedValue);
                    platform.CreatedBy = UserSession.Current.User.Id;
                    platform.CreatedDate = DateTime.Now;

                    
                     this.platformService.Insert(platform);
                    
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
                this.fileNameValidator.ErrorMessage = "Please enter Platform name.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        
    }
}