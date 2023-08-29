// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
    public partial class TitleEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly TitleService titleService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;


        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public TitleEditForm()
        {
            this.userService = new UserService();
            this.titleService = new TitleService();
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
                //this.LoadComboData();
                
                if (!string.IsNullOrEmpty(this.Request.QueryString["disId"]))
                {
                    var obj = this.titleService.GetById(Convert.ToInt32(this.Request.QueryString["disId"]));
                    this.CreatedInfo.Visible = obj != null;

                    if (obj != null)
                    {
                        this.txtName.Text = obj.Name;
                        this.txtDescription.Text = obj.Description;
                        this.ddlLocation.SelectedValue = obj.LocationId.GetValueOrDefault().ToString();
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
                    var documentTitleId = Convert.ToInt32(this.Request.QueryString["disId"]);
                    var obj = this.titleService.GetById(documentTitleId);
                    if (obj != null)
                    {
                        obj.Name = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        obj.LocationId = Convert.ToInt32(this.ddlLocation.SelectedValue);
                        obj.LocationName = this.ddlLocation.SelectedItem.Text;
                        obj.UpdatedBy = UserSession.Current.User.Id;
                        obj.UpdatedDate = DateTime.Now;
                        obj.UpdatedName = UserSession.Current.User.FullName;
                        this.titleService.Update(obj);
                    }
                }
                else
                {
                    var obj = new Title()
                    {
                        Name = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        LocationId = Convert.ToInt32(this.ddlLocation.SelectedValue),
                        LocationName = this.ddlLocation.SelectedItem.Text,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedName = UserSession.Current.User.FullName,
                        CreatedDate = DateTime.Now
                    };

                    this.titleService.Insert(obj);
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

        //private void LoadComboData()
        //{
        //    var projectList = this.projectService.GetAll();
        //    this.ddlProject.DataSource = projectList;
        //    this.ddlProject.DataTextField = "FullName";
        //    this.ddlProject.DataValueField = "ID";
        //    this.ddlProject.DataBind();
        //}
    }
}