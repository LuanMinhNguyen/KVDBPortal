// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using EDMs.Business.Services.Scope;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services;
    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using System.Linq;

    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class TransmittalEditForm : Page
    {
        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly DocumentService documentService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// The to list service.
        /// </summary>
        private readonly ToListService toListService;

        /// <summary>
        /// The attention service.
        /// </summary>
        private readonly AttentionService attentionService;

        /// <summary>
        /// The transmittal service.
        /// </summary>
        private readonly TransmittalService transmittalService;

        private readonly ScopeProjectService scopeProjectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public TransmittalEditForm()
        {
            this.documentService = new DocumentService();
            this.userService = new UserService();

            this.transmittalService = new TransmittalService();
            this.toListService = new ToListService();
            this.attentionService = new AttentionService();
            this.scopeProjectService=new ScopeProjectService();
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

                if (!string.IsNullOrEmpty(this.Request.QueryString["tranId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var objTran = this.transmittalService.GetById(Convert.ToInt32(this.Request.QueryString["tranId"]));
                    if (objTran != null)
                    {
                        this.txtName.Text = objTran.Name;
                        this.ddlProject.SelectedValue = objTran.ProjectId.ToString();
                        this.txtTransmittalNumber.Text = objTran.TransmittalNumber;
                        this.txtReason.Text = objTran.ReasonForIssue;
                        this.ddlToList.SelectedValue = objTran.ToId.ToString();
                        this.ddlFromList.SelectedValue = objTran.FromId.ToString();
                        //this.txtDate.SelectedDate = objTran.IssuseDate;
                        var createdUser = this.userService.GetByID(objTran.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + objTran.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (objTran.LastUpdatedBy != null && objTran.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(objTran.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + objTran.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
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
                var toId = Convert.ToInt32(this.ddlToList.SelectedValue);
                var fromId = Convert.ToInt32(this.ddlFromList.SelectedValue);

                var toObj = this.userService.GetByID(toId);
                var fromObj = this.userService.GetByID(fromId);

                if (!string.IsNullOrEmpty(this.Request.QueryString["tranId"]))
                {
                    var tranId = Convert.ToInt32(this.Request.QueryString["tranId"]);
                    var objTran = this.transmittalService.GetById(tranId);
                    if (objTran != null)
                    {
                        objTran.Name = this.txtName.Text.Trim();
                        objTran.TransmittalNumber = this.txtTransmittalNumber.Text.Trim();
                        objTran.ProjectName = this.ddlProject.SelectedItem.Text;
                        objTran.ProjectId = Convert.ToInt32(this.ddlProject.SelectedValue);
                        objTran.ReasonForIssue = this.txtReason.Text.Trim();
                        //objTran.ToId = toObj != null ? toObj.Id : 0;
                        //objTran.ToList = toObj != null
                        //    ? "<span style='color: blue; font-weight: bold'>" + toObj.FullName + "</span>" + "<br/>" +
                        //      toObj.Position
                        //    : string.Empty;

                        //objTran.FromId = fromObj != null ? fromObj.Id : 0;
                        //objTran.FromList = fromObj != null ? "<span style='color: blue; font-weight: bold'>" + fromObj.FullName + "</span>" + "<br/>" + fromObj.Position : string.Empty;

                        objTran.ToId = Convert.ToInt32(this.ddlToList.SelectedValue);
                        objTran.ToList = this.ddlToList.SelectedItem.Text;

                        objTran.FromId = Convert.ToInt32(this.ddlFromList.SelectedValue);
                        objTran.FromList = this.ddlFromList.SelectedItem.Text;

                        //objTran.IssuseDate = this.txtDate.SelectedDate;

                        objTran.LastUpdatedBy = UserSession.Current.User.Id;
                        objTran.LastUpdatedDate = DateTime.Now;

                        this.transmittalService.Update(objTran);
                    }
                }
                else
                {
                    var objTran = new Transmittal()
                    {
                        Name = this.txtName.Text.Trim(),
                        TransmittalNumber = this.txtTransmittalNumber.Text.Trim(),
                        ProjectName = this.ddlProject.SelectedItem.Text,
                        ProjectId = Convert.ToInt32(this.ddlProject.SelectedValue),
                        ReasonForIssue = this.txtReason.Text.Trim(),
                        ToId = Convert.ToInt32(this.ddlToList.SelectedValue),
                        ToList = this.ddlToList.SelectedItem.Text,
                        FromId = Convert.ToInt32(this.ddlFromList.SelectedValue),
                        FromList = this.ddlFromList.SelectedItem.Text,

                        TransType = 1,
                        //IssuseDate = this.txtDate.SelectedDate,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,
                    };

                    this.transmittalService.Insert(objTran);
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
                this.fileNameValidator.ErrorMessage = "Please enter transmittal name.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["tranId"]))
            {
                var tranId = Convert.ToInt32(Request.QueryString["tranId"]);
                var folderId = Convert.ToInt32(Request.QueryString["folId"]);
                this.fileNameValidator.ErrorMessage = "The specified name is already in use.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = !this.documentService.IsDocumentExistUpdate(folderId, this.txtName.Text.Trim(), tranId);
            }
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData()
        {
            var projectInPermission = UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
              ? this.scopeProjectService.GetAll()
              : this.scopeProjectService.GetAllInPermission(UserSession.Current.User.Id);
            this.ddlProject.DataSource = projectInPermission;
            this.ddlProject.DataTextField = "FullName";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();

            //var userList = this.userService.GetAll().OrderBy(t => t.FullName).ToList();

            var toList = this.toListService.GetAll();

            this.ddlToList.DataSource = toList;
            this.ddlToList.DataTextField = "FullName";
            this.ddlToList.DataValueField = "Id";
            this.ddlToList.DataBind();

            this.ddlFromList.DataSource = toList;
            this.ddlFromList.DataTextField = "FullName";
            this.ddlFromList.DataValueField = "Id";
            this.ddlFromList.DataBind();
        }
    }
}