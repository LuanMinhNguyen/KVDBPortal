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
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;

namespace EDMs.Web.Controls.Document
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class DistributionMatrixEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly DistributionMatrixService DistributionMatrixService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly ScopeProjectService scopeProjectService;

        private readonly DistributionMatrixTypeService distributionMatrixTypeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributionMatrixEditForm"/> class.
        /// </summary>
        public DistributionMatrixEditForm()
        {
            this.userService = new UserService();
            this.DistributionMatrixService = new DistributionMatrixService();
            this.scopeProjectService = new ScopeProjectService();
            this.distributionMatrixTypeService = new DistributionMatrixTypeService();
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

                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var obj = this.DistributionMatrixService.GetById(Convert.ToInt32(this.Request.QueryString["objId"]));
                    if (obj != null)
                    {
                        this.txtName.Text = obj.Name;
                        this.txtDescription.Text = obj.Description;
                        this.ddlReAssignUser.SelectedValue = obj.Re_assignUserId.ToString();

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
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var DistributionMatrixId = Convert.ToInt32(this.Request.QueryString["objId"]);
                    var obj = this.DistributionMatrixService.GetById(DistributionMatrixId);
                    if (obj != null)
                    {
                        obj.Name = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        ////obj.TypeId = Convert.ToInt32(this.ddlMatrixType.SelectedValue);
                        ////obj.TypeName = this.ddlMatrixType.SelectedItem.Text;
                        obj.Re_assignUserId = Convert.ToInt32(this.ddlReAssignUser.SelectedValue);
                        obj.Re_assignUserName = this.ddlReAssignUser.SelectedItem.Text;

                        this.DistributionMatrixService.Update(obj);
                    }
                }
                else
                {
                    var obj = new DistributionMatrix();
                    obj.Name = this.txtName.Text.Trim();
                    obj.Description = this.txtDescription.Text.Trim();
                    ////obj.TypeId = Convert.ToInt32(this.ddlMatrixType.SelectedValue);
                    ////obj.TypeName = this.ddlMatrixType.SelectedItem.Text;
                    obj.Re_assignUserId = Convert.ToInt32(this.ddlReAssignUser.SelectedValue);
                    obj.Re_assignUserName = this.ddlReAssignUser.SelectedItem.Text;
                    obj.CreatedBy = UserSession.Current.User.Id;
                    obj.CreatedDate = DateTime.Now;

                    this.DistributionMatrixService.Insert(obj);
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
            this.ddlReAssignUser.DataSource = userList;
            this.ddlReAssignUser.DataTextField = "UserNameWithFullName";
            this.ddlReAssignUser.DataValueField = "Id";
            this.ddlReAssignUser.DataBind();

            ////var matrixTypeList = this.distributionMatrixTypeService.GetAll();
            ////this.ddlMatrixType.DataSource = matrixTypeList;
            ////this.ddlMatrixType.DataTextField = "Name";
            ////this.ddlMatrixType.DataValueField = "Id";
            ////this.ddlMatrixType.DataBind();
        }
    }
}