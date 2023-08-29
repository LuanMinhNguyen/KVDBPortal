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
    using EDMs.Web.Controls.Document;
    using EDMs.Web.Utilities.Sessions;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class DocumentCodeEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly DocumentCodeServices documentCodeService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public DocumentCodeEditForm()
        {
            this.userService = new UserService();
            this.documentCodeService = new DocumentCodeServices();
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
                
                if (!string.IsNullOrEmpty(this.Request.QueryString["documentId"]))
                {
                    var obj = this.documentCodeService.GetById(Convert.ToInt32(this.Request.QueryString["documentId"]));
                    this.CreatedInfo.Visible = obj != null;

                    if (obj != null)
                    {
                        this.txtCode.Text = obj.Code;
                        this.txtName.Text = obj.Name;
                        this.txtDescription.Text = obj.Description;
                        this.ddlType.SelectedValue = obj.TypeId.ToString();
                        var createdUser = this.userService.GetByID(obj.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + obj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (obj.LastUpdatedBy != null && obj.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(obj.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + obj.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["documentId"]))
                {
                    var documentId = Convert.ToInt32(this.Request.QueryString["documentId"]);
                    var obj = this.documentCodeService.GetById(documentId);
                    if (obj != null)
                    {
                        obj.Code = this.txtCode.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedDate = DateTime.Now;
                        obj.TypeId = Convert.ToInt32(this.ddlType.SelectedValue);
                        obj.TypeName = this.ddlType.SelectedItem.Text;
                        this.documentCodeService.Update(obj);
                    }
                }
                else
                {
                    var objDocument = new DocumentCode()
                    {
                        Code = this.txtCode.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,
                        TypeId = Convert.ToInt32(this.ddlType.SelectedValue),
                        TypeName = this.ddlType.SelectedItem.Text
                    };

                    this.documentCodeService.Insert(objDocument);
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
                this.fileNameValidator.ErrorMessage = "Please enter Document code.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        private void LoadComboData()
        {
            //var projectList = this.projectService.GetAll();
            //projectList.Insert(0, new ScopeProject() {ID = 0});

            //this.ddlProject.DataSource = projectList;
            //this.ddlProject.DataTextField = "Name";
            //this.ddlProject.DataValueField = "ID";
            //this.ddlProject.DataBind();
        }
    }
}