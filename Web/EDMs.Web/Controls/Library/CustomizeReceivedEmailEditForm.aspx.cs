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
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class CustomizeReceivedEmailEditForm : Page
    {
        /// <summary>
        /// The Group service.
        /// </summary>
        private readonly CustomizeReceivedEmailService receivedemailService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly ProjectCodeService projectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomizeReceivedEmailEditForm"/> class.
        /// </summary>
        public CustomizeReceivedEmailEditForm()
        {
            this.userService = new UserService();
            this.receivedemailService = new  CustomizeReceivedEmailService();
            this.projectService = new ProjectCodeService();
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
                LoadComboData();
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var objDis = this.receivedemailService.GetById(Convert.ToInt32(this.Request.QueryString["objId"]));
                    if (objDis != null)
                    {

                        this.txtName.Text = objDis.Name;
                        this.ddlProject.SelectedValue = objDis.ProjectID.GetValueOrDefault().ToString();
                        this.ddlfrom.SelectedValue = objDis.Pecc2SendReceived.GetValueOrDefault().ToString();
                        this.ddlObject.SelectedValue = objDis.TypeID.GetValueOrDefault().ToString();
                        var createdUser = this.userService.GetByID(objDis.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + objDis.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (objDis.UpdatedBy != null && objDis.UpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(objDis.UpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + objDis.UpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
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
                    var objId = Convert.ToInt32(this.Request.QueryString["objId"]);
                    var objDis = this.receivedemailService.GetById(objId);
                    if (objDis != null)
                    {
                        objDis.Name = this.txtName.Text.Trim();
                        objDis.ProjectID = Convert.ToInt32(this.ddlProject.SelectedValue);
                        objDis.ProjectName = this.ddlProject.SelectedItem.Text;
                        objDis.TypeID = Convert.ToInt32(this.ddlObject.SelectedValue);
                        objDis.Pecc2SendReceived = Convert.ToInt32(this.ddlfrom.SelectedValue);
                        objDis.TypeName = this.ddlObject.SelectedItem.Text;
                        objDis.UpdatedBy = UserSession.Current.User.Id;
                        objDis.UpdatedDate = DateTime.Now;

                        this.receivedemailService.Update(objDis);
                        
                    }
                }
                else
                {
                    var objDis = new EDMs.Data.Entities.CustomizeReceivedEmail()
                    {
                        Name = this.txtName.Text.Trim(),
                        ProjectID = Convert.ToInt32(this.ddlProject.SelectedValue),
                        ProjectName = this.ddlProject.SelectedItem.Text,
                        TypeID = Convert.ToInt32(this.ddlObject.SelectedValue),
                        Pecc2SendReceived = Convert.ToInt32(this.ddlfrom.SelectedValue),
                        TypeName = this.ddlObject.SelectedItem.Text,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now
                    };

                    this.receivedemailService.Insert(objDis);
                    
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

       

        private void LoadComboData()
        {
            var projectInPermission = this.projectService.GetAll();
            this.ddlProject.DataSource = projectInPermission;
            this.ddlProject.DataTextField = "FullName";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();
        }
    }
}