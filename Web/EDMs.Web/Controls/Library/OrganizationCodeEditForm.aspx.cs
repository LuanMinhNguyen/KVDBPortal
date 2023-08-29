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
    public partial class OrganizationCodeEditForm : Page
    {
        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly GroupDataPermissionService groupDataPermissionService;

        /// <summary>
        /// The category service.
        /// </summary>
        private readonly CategoryService categoryService;

        private readonly OrganizationCodeService _OrganizationCodeService = new OrganizationCodeService();
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public OrganizationCodeEditForm()
        {
            this.userService = new UserService();   
            this.groupDataPermissionService = new GroupDataPermissionService();
            this.categoryService = new CategoryService();
            this._OrganizationCodeService = new OrganizationCodeService();
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["disId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var obj = this._OrganizationCodeService.GetById(Convert.ToInt32(this.Request.QueryString["disId"]));
                    if (obj != null)
                    {
                        this.txtName.Text = obj.Code;
                        this.txtDescription.Text = obj.Description;
                        this.ckbIsDefault.Checked = obj.IsDefaultReceiveContractorOutgoingTrans.GetValueOrDefault();
                        this.ddlRole.SelectedValue = obj.Role;
                        this.txtHeadOffice.Text = obj.HeadOffice;
                        this.txtFax.Text = obj.Fax;
                        this.txtPhone.Text = obj.Phone;
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
                    var plantId = Convert.ToInt32(this.Request.QueryString["disId"]);
                    var obj = this._OrganizationCodeService.GetById(plantId);
                    if (obj != null)
                    {
                        obj.Code = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        obj.Role = this.ddlRole.SelectedValue;
                        obj.HeadOffice = this.txtHeadOffice.Text;
                        obj.Phone = this.txtPhone.Text;
                        obj.Fax = this.txtFax.Text;
                        obj.IsDefaultReceiveContractorOutgoingTrans = this.ckbIsDefault.Checked;
                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedDate = DateTime.Now;

                        this._OrganizationCodeService.Update(obj);
                    }
                }
                else
                {
                    var obj = new OrganizationCode()
                    {
                        Code = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        Role=this.ddlRole.SelectedValue,
                        HeadOffice = this.txtHeadOffice.Text,
                        IsDefaultReceiveContractorOutgoingTrans = this.ckbIsDefault.Checked,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,
                        Fax = this.txtFax.Text,
                        Phone = this.txtPhone.Text
                    };

                    this._OrganizationCodeService.Insert(obj);
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
                this.fileNameValidator.ErrorMessage = "Please enter Unit Code.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }
    }
}