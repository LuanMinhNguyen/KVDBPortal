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
    public partial class AreaEditForm : Page
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
        private readonly PlantService PlantService;

        private readonly AreaService areaService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public AreaEditForm()
        {
            this.userService = new UserService();
            this.PlantService = new PlantService();
            this.groupDataPermissionService = new GroupDataPermissionService();
            this.categoryService = new CategoryService();
            this.areaService = new AreaService();
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

                var plantlist = this.PlantService.GetAll();
                this.ddlPlant.DataSource = plantlist;
                this.ddlPlant.DataTextField = "Name";
                this.ddlPlant.DataValueField = "ID";
                this.ddlPlant.DataBind();



                if (!string.IsNullOrEmpty(this.Request.QueryString["disId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var objArea = this.areaService.GetById(Convert.ToInt32(this.Request.QueryString["disId"]));
                    if (objArea != null)
                    {
                        this.txtName.Text = objArea.Code;
                        this.txtDescription.Text = objArea.Description;
                        this.ckbActive.Checked = objArea.Active.GetValueOrDefault();
                        this.ddlPlant.SelectedValue = objArea.PlantId.GetValueOrDefault().ToString();
                        var createdUser = this.userService.GetByID(objArea.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + objArea.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (objArea.LastUpdatedBy != null && objArea.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(objArea.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + objArea.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
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
                    var obj = this.areaService.GetById(plantId);
                    if (obj != null)
                    {
                        obj.Code = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        obj.PlantId = Convert.ToInt32(this.ddlPlant.SelectedValue);
                        //obj.Code = this.ddlPlant.SelectedItem.Text;
                        obj.Active = this.ckbActive.Checked;
                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedDate = DateTime.Now;

                        this.areaService.Update(obj);
                    }
                }
                else
                {
                    var obj = new Area()
                    {
                        Code = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        PlantId= Convert.ToInt32(this.ddlPlant.SelectedValue),
                        Active = this.ckbActive.Checked,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now
                    };
                   
                    this.areaService.Insert(obj);
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
                this.fileNameValidator.ErrorMessage = "Please enter Area Code.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }
    }
}