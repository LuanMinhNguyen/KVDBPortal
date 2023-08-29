// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using EDMs.Business.Services.Document;
using EDMs.Business.Services.Scope;

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
    public partial class DrawingEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly DrawingService drawingService;

        private readonly ProcessActualService processActualService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// 
        /// </summary>
        public DrawingEditForm()
        {
            this.userService = new UserService();
            this.drawingService = new DrawingService();
            this.processActualService = new ProcessActualService();
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["drawingId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var objDrawing = this.drawingService.GetById(Convert.ToInt32(this.Request.QueryString["drawingId"]));
                    if (objDrawing != null)
                    {

                        this.txtCode.Text = objDrawing.Code;
                        this.txtDescription.Text = objDrawing.Description;
                        var createdUser = this.userService.GetByID(objDrawing.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + objDrawing.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (objDrawing.LastUpdatedBy != null && objDrawing.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(objDrawing.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + objDrawing.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["drawingId"]))
                {
                    var drawingId = Convert.ToInt32(this.Request.QueryString["drawingId"]);
                    var objDrawing = this.drawingService.GetById(drawingId);
                    if (objDrawing != null)
                    {
                        objDrawing.Code = this.txtCode.Text.Trim();
                        objDrawing.Description = this.txtDescription.Text.Trim();
                        objDrawing.LastUpdatedBy = UserSession.Current.User.Id;
                        objDrawing.LastUpdatedDate = DateTime.Now;

                        this.drawingService.Update(objDrawing);
                        
                    }
                }
                else
                {
                    var objDrawing = new DrawingCode()
                    {
                        Code = this.txtCode.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now
                    };

                    this.drawingService.Insert(objDrawing);
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
                this.fileNameValidator.ErrorMessage = "Please enter Drawing Code.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        private void LoadComboData()
        {
            //var projectInPermission = this.scopeProjectService.GetAll();
            //this.ddlProject.DataSource = projectInPermission;
            //this.ddlProject.DataTextField = "FullName";
            //this.ddlProject.DataValueField = "ID";
            //this.ddlProject.DataBind();
        }
    }
}