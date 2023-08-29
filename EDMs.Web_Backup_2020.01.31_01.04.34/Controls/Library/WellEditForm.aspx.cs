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

    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Controls.Document;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class WellEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly FieldService fieldService;

        private readonly WellService WellService;

        /// <summary>
        /// The role service.
        /// </summary>
        private readonly RoleService roleService;

        /// <summary>
        /// The role service.
        /// </summary>
        private readonly PlatformService platformService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public WellEditForm()
        {
            this.userService = new UserService();
            this.fieldService = new FieldService();
            this.roleService = new RoleService();
            this.platformService = new PlatformService();
            this.WellService = new WellService();
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

                if (!string.IsNullOrEmpty(this.Request.QueryString["id"]))
                {
                    this.CreatedInfo.Visible = true;

                    var obj = this.WellService.GetById(Convert.ToInt32(this.Request.QueryString["id"]));
                    if (obj != null)
                    {
                        this.txtName.Text = obj.Name;
                        this.txtDescription.Text = obj.Description;

                        var roleIds = obj.RoleId;
                        foreach (RadComboBoxItem item in this.ddlDepartment.Items)
                        {
                            if (roleIds.Contains(item.Value))
                            {
                                item.Checked = true;
                            }
                        }

                        this.ddlPlatform.SelectedValue = obj.PlatformId.GetValueOrDefault().ToString();

                        if (obj.StartDate != null)
                        {
                            this.txtStartDate.SelectedDate = obj.StartDate.Value;
                        }

                        if (obj.EndDate != null)
                        {
                            this.txtEndDate.SelectedDate = obj.EndDate.Value;
                        }

                        if (obj.StartDateTesting != null)
                        {
                            this.txtStartDateTesting.SelectedDate = obj.StartDateTesting.Value;
                        }

                        if (obj.EndDateTesting != null)
                        {
                            this.txtEndDateTesting.SelectedDate = obj.EndDateTesting.Value;
                        }


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
                var roleIds = string.Empty;
                var roleNames = string.Empty;
                roleIds = this.ddlDepartment.CheckedItems.Aggregate(roleIds, (current, t) => current + t.Value + ", ");
                roleNames = this.ddlDepartment.CheckedItems.Aggregate(roleNames, (current, t) => current + t.Text + ", ");
                if (!string.IsNullOrEmpty(this.Request.QueryString["id"]))
                {
                    var id = Convert.ToInt32(this.Request.QueryString["id"]);
                    var obj = this.WellService.GetById(id);
                    if (obj != null)
                    {
                        obj.Name = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        obj.PlatformId = Convert.ToInt32(this.ddlPlatform.SelectedValue);
                        obj.RoleId = roleIds;
                        obj.RoleName = roleNames;
                        obj.StartDate = this.txtStartDate.SelectedDate;
                        obj.EndDate = this.txtEndDate.SelectedDate;
                        obj.StartDateTesting = this.txtStartDateTesting.SelectedDate;
                        obj.EndDateTesting = this.txtEndDateTesting.SelectedDate;
                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedDate = DateTime.Now;

                        this.WellService.Update(obj);
                    }
                }
                else
                {
                    var obj = new Well()
                    {
                        Name = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        PlatformId = Convert.ToInt32(this.ddlPlatform.SelectedValue),
                        RoleName = roleNames,
                        RoleId = roleIds,
                        StartDate = this.txtStartDate.SelectedDate,
                        EndDate = this.txtEndDate.SelectedDate,
                        StartDateTesting = this.txtStartDateTesting.SelectedDate,
                        EndDateTesting = this.txtEndDateTesting.SelectedDate,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,
                        Active = true
                    };

                    this.WellService.Insert(obj);
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
                this.fileNameValidator.ErrorMessage = "Please enter Well name.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        private void LoadComboData()
        {
            var listDeparment = this.roleService.GetAllSpecial();
            
            this.ddlDepartment.DataSource = listDeparment;
            this.ddlDepartment.DataTextField = "Name";
            this.ddlDepartment.DataValueField = "Id";
            this.ddlDepartment.DataBind();
            this.ddlDepartment.SelectedIndex = 0;

            var listPlatform = this.platformService.GetAll();
            listPlatform.Insert(0, new Platform() { ID = 0, Name = string.Empty });
            this.ddlPlatform.DataSource = listPlatform;
            this.ddlPlatform.DataTextField = "Name";
            this.ddlPlatform.DataValueField = "Id";
            this.ddlPlatform.DataBind();
            this.ddlPlatform.SelectedIndex = 0;
        }
    }
}