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
    public partial class ContractorEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly ContractorService contractorService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly ScopeProjectService projectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ContractorEditForm()
        {
            this.userService = new UserService();
            this.contractorService = new ContractorService();
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
                    var objContractor = this.contractorService.GetById(Convert.ToInt32(this.Request.QueryString["disId"]));
                    this.CreatedInfo.Visible = objContractor != null;

                    if (objContractor != null)
                    {
                        this.txtName.Text = objContractor.Name;
                        this.txtDescription.Text = objContractor.Description;
                        this.ddlProject.SelectedValue = objContractor.ProjectID.GetValueOrDefault().ToString();
                        this.ddlType.SelectedValue = objContractor.TypeID.ToString();

                        var createdUser = this.userService.GetByID(objContractor.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + objContractor.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (objContractor.UpdatedBy != null && objContractor.UpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(objContractor.UpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + objContractor.UpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
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
                    var contractorId = Convert.ToInt32(this.Request.QueryString["disId"]);
                    var obj = this.contractorService.GetById(contractorId);
                    if (obj != null)
                    {
                        obj.Name = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        obj.ProjectID = Convert.ToInt32(this.ddlProject.SelectedValue);
                        obj.ProjectName = this.ddlProject.SelectedItem.Text;
                        obj.TypeID = Convert.ToInt32(this.ddlType.SelectedValue);
                        obj.TypeName = this.ddlType.SelectedItem.Text;
                        obj.UpdatedBy = UserSession.Current.User.Id;
                        obj.UpdatedDate = DateTime.Now;
                        this.contractorService.Update(obj);
                    }
                }
                else
                {
                    var obj = new Contractor()
                    {
                        Name = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        ProjectID = Convert.ToInt32(this.ddlProject.SelectedValue),
                        ProjectName = this.ddlProject.SelectedItem.Text,
                        TypeID = Convert.ToInt32(this.ddlType.SelectedValue),
                        TypeName = this.ddlType.SelectedItem.Text,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now
                    };

                    this.contractorService.Insert(obj);
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