﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Controls.Document;
using EDMs.Web.Utilities.Sessions;

namespace EDMs.Web.Controls.Library
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ProcedureTypeEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly ProcedureTypeService mrTyprService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;


        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ProcedureTypeEditForm()
        {
            this.userService = new UserService();
            this.mrTyprService = new ProcedureTypeService();
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
                    var objDocumentProcedureType = this.mrTyprService.GetById(Convert.ToInt32(this.Request.QueryString["disId"]));
                    this.CreatedInfo.Visible = objDocumentProcedureType != null;

                    if (objDocumentProcedureType != null)
                    {
                        this.txtName.Text = objDocumentProcedureType.Name;
                        this.txtDescription.Text = objDocumentProcedureType.Description;
                        var createdUser = this.userService.GetByID(objDocumentProcedureType.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + objDocumentProcedureType.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (objDocumentProcedureType.UpdatedBy != null && objDocumentProcedureType.UpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(objDocumentProcedureType.UpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + objDocumentProcedureType.UpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
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
                    var documentProcedureTypeId = Convert.ToInt32(this.Request.QueryString["disId"]);
                    var obj = this.mrTyprService.GetById(documentProcedureTypeId);
                    if (obj != null)
                    {
                        obj.Name = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        obj.UpdatedBy = UserSession.Current.User.Id;
                        obj.UpdatedDate = DateTime.Now;
                        obj.UpdatedByName = UserSession.Current.User.FullName;
                        this.mrTyprService.Update(obj);
                    }
                }
                else
                {
                    var obj = new ProcedureType()
                    {
                        Name = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedName = UserSession.Current.User.FullName,
                        CreatedDate = DateTime.Now
                    };

                    this.mrTyprService.Insert(obj);
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