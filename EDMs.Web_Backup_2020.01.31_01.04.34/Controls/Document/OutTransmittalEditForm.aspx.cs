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
    public partial class OutTransmittalEditForm : Page
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
        private readonly DQRETransmittalService transmittalService;
        /// <summary>
        /// The scope project services
        /// </summary>
        private readonly ProjectCodeService projectCodeService;

        /// <summary>
        /// The confidentiality services
        /// </summary>
        private readonly ConfidentialityService confidentialityServices;

        private readonly OrganizationCodeService organizationCodeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public OutTransmittalEditForm()
        {
            this.documentService = new DocumentService();
            this.userService = new UserService();

            this.transmittalService = new DQRETransmittalService();
            this.toListService = new ToListService();
            this.attentionService = new AttentionService();
            this.projectCodeService = new ProjectCodeService();
            this.confidentialityServices = new ConfidentialityService();
            this.organizationCodeService = new OrganizationCodeService();
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
                    Guid id;
                    var objTran = this.transmittalService.GetById(new Guid(this.Request.QueryString["tranId"]));
                    if (objTran != null)
                    {
                        this.txtTransmittalNo.Text = objTran.TransmittalNo;
                        this.txtDescription.Text = objTran.Description;
                        //this.txtOtherTrans.Text = objTran.OtherTransmittal;
                        if (objTran.OriginatingOrganizationId.ToString() != "")
                        { this.ddlOriginatingOrganization.SelectedValue = objTran.OriginatingOrganizationId.ToString(); }
                        if (objTran.ReceivingOrganizationId.ToString() != "")
                        { this.ddlReceivingOrganization.SelectedValue = objTran.ReceivingOrganizationId.ToString(); }
                        if (objTran.ProjectCodeId.ToString() != "")
                        { this.ddlProjectCode.SelectedValue = objTran.ProjectCodeId.ToString(); }
                        if (objTran.ConfidentialityId.ToString() != "")
                        { this.ddlConfidentiality.SelectedValue = objTran.ConfidentialityId.ToString(); }
                        //if (objTran.TransmittalStatus.ToString() != "")
                        //{ this.ddlTransStatus.SelectedValue = objTran.TransmittalStatus.ToString(); }
                        this.txtDayIssued.SelectedDate = objTran.IssuedDate;
                        this.txtDayReceived.SelectedDate = objTran.ReceivedDate;
                        //this.txtReason.Text = objTran.ReasonForIssue;
                        //this.ddlToList.SelectedValue = objTran.ToId.ToString();
                        //this.ddlFromList.SelectedValue = objTran.FromId.ToString();
                        //this.txtDate.SelectedDate = objTran.DateIssused;
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
                //var toId = Convert.ToInt32(this.ddlToList.SelectedValue);
                //var fromId = Convert.ToInt32(this.ddlFromList.SelectedValue);

                //var toObj = this.userService.GetByID(toId);
                //var fromObj = this.userService.GetByID(fromId);

                if (!string.IsNullOrEmpty(this.Request.QueryString["tranId"]))
                {
                    var tranId = new Guid(this.Request.QueryString["tranId"]);
                    var objTran = this.transmittalService.GetById(tranId);
                    if (objTran != null)
                    {
                        objTran.TransmittalNo = this.txtTransmittalNo.Text.Trim();
                        objTran.Description = this.txtDescription.Text.Trim();
                        //objTran.OtherTransmittal = this.txtOtherTrans.Text.Trim();
                        objTran.OriginatingOrganizationId = Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue);
                        objTran.OriginatingOrganizationName = (this.ddlOriginatingOrganization.SelectedItem.Text.Trim());
                        objTran.ReceivingOrganizationId = Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue);
                        objTran.ReceivingOrganizationName = (this.ddlReceivingOrganization.SelectedItem.Text.Trim());
                        objTran.ProjectCodeId = Convert.ToInt32(this.ddlProjectCode.SelectedValue);
                        objTran.ProjectCodeName = (this.ddlProjectCode.SelectedItem.Text.Split(','))[0].Trim();
                        objTran.ConfidentialityId = Convert.ToInt32(this.ddlConfidentiality.SelectedValue);
                        objTran.ConfidentialityName = this.ddlConfidentiality.SelectedItem.Text;
                        //objTran.TransmittalStatus = this.ddlTransStatus.SelectedValue;
                        //objTran.ProjectName = this.ddlProject.SelectedItem.Text;
                        //objTran.ProjectId = Convert.ToInt32(this.ddlProject.SelectedValue);
                        //objTran.ReasonForIssue = this.txtReason.Text.Trim();
                        //objTran.ToId = toObj != null ? toObj.Id : 0;
                        //objTran.ToList = toObj != null
                        //    ? "<span style='color: blue; font-weight: bold'>" + toObj.FullName + "</span>" + "<br/>" +
                        //      toObj.Position
                        //    : string.Empty;

                        //objTran.FromId = fromObj != null ? fromObj.Id : 0;
                        //objTran.FromList = fromObj != null ? "<span style='color: blue; font-weight: bold'>" + fromObj.FullName + "</span>" + "<br/>" + fromObj.Position : string.Empty;

                        //objTran.ToId = Convert.ToInt32(this.ddlToList.SelectedValue);
                        //objTran.ToList = this.ddlToList.SelectedItem.Text;

                        //objTran.FromId = Convert.ToInt32(this.ddlFromList.SelectedValue);
                        //objTran.FromList = this.ddlFromList.SelectedItem.Text;

                        objTran.IssuedDate = this.txtDayIssued.SelectedDate;
                        objTran.ReceivedDate = this.txtDayReceived.SelectedDate;
                        objTran.LastUpdatedBy = UserSession.Current.User.Id;
                        objTran.LastUpdatedDate = DateTime.Now;

                        this.transmittalService.Update(objTran);
                    }
                }
                else
                {
                    var objTran = new DQRETransmittal();
                    objTran.ID = Guid.NewGuid();
                    objTran.TransmittalNo = this.txtTransmittalNo.Text.Trim();
                    objTran.Description = this.txtDescription.Text.Trim();
                    //objTran.OtherTransmittal = this.txtOtherTrans.Text.Trim();
                    objTran.OriginatingOrganizationId = Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue);
                    objTran.OriginatingOrganizationName = (this.ddlOriginatingOrganization.SelectedItem.Text.Trim());
                    objTran.ReceivingOrganizationId = Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue);
                    objTran.ReceivingOrganizationName = (this.ddlReceivingOrganization.SelectedItem.Text.Trim());
                    objTran.ProjectCodeId = Convert.ToInt32(this.ddlProjectCode.SelectedValue);
                    objTran.ProjectCodeName = (this.ddlProjectCode.SelectedItem.Text.Split(','))[0].Trim();
                    objTran.ConfidentialityId = Convert.ToInt32(this.ddlConfidentiality.SelectedValue);
                    objTran.ConfidentialityName = this.ddlConfidentiality.SelectedItem.Text;
                    //objTran.TransmittalStatus = this.ddlTransStatus.SelectedValue;

                    //ProjectName = this.ddlProject.SelectedItem.Text,
                    //ProjectId = Convert.ToInt32(this.ddlProject.SelectedValue),
                    //ReasonForIssue = this.txtReason.Text.Trim(),
                    //ToId = Convert.ToInt32(this.ddlToList.SelectedValue),
                    //ToList = this.ddlToList.SelectedItem.Text,
                    //FromId = Convert.ToInt32(this.ddlFromList.SelectedValue),
                    //FromList = this.ddlFromList.SelectedItem.Text,

                    //TransType = 2,

                    objTran.IssuedDate = this.txtDayIssued.SelectedDate;
                    objTran.ReceivedDate = this.txtDayReceived.SelectedDate;
                    objTran.CreatedBy = UserSession.Current.User.Id;
                    objTran.CreatedDate = DateTime.Now;

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
            if (this.txtTransmittalNo.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter transmittal number.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
            //else if (!string.IsNullOrEmpty(Request.QueryString["tranId"]))
            //{
                //var tranId = Convert.ToInt32(Request.QueryString["tranId"]);
                //var folderId = Convert.ToInt32(Request.QueryString["folId"]);
                //this.fileNameValidator.ErrorMessage = "The specified number is already in use.";
                //this.divFileName.Style["margin-bottom"] = "-26px;";
                //args.IsValid = !this.documentService.IsDocumentExistUpdate(folderId, this.txtTransmittalNo.Text.Trim(), tranId);
            //}
        }

        protected void ServerValidationDescription(object source, ServerValidateEventArgs args)
        {
            if (this.txtDescription.Text.Trim().Length == 0)
            {
                this.descriptionValidator.ErrorMessage = "Please enter description.";
                this.divDescription.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData()
        {
            var confidentialityList = this.confidentialityServices.GetAll(UserSession.Current.User.ConfidentialId.GetValueOrDefault()).OrderBy(t => t.ID).ToList();
            this.ddlConfidentiality.DataSource = confidentialityList;
            this.ddlConfidentiality.DataTextField = "Code";
            this.ddlConfidentiality.DataValueField = "ID";
            this.ddlConfidentiality.DataBind();

            var organizationCodeList = this.organizationCodeService.GetAll().OrderBy(t => t.Code).ToList();
            this.ddlOriginatingOrganization.DataSource = organizationCodeList;
            this.ddlOriginatingOrganization.DataTextField = "FullName";
            this.ddlOriginatingOrganization.DataValueField = "ID";
            this.ddlOriginatingOrganization.DataBind();

            this.ddlReceivingOrganization.DataSource = organizationCodeList;
            this.ddlReceivingOrganization.DataTextField = "FullName";
            this.ddlReceivingOrganization.DataValueField = "ID";
            this.ddlReceivingOrganization.DataBind();

            var projectCodeList = this.projectCodeService.GetAll().OrderBy(t=>t.Code).ToList();
            this.ddlProjectCode.DataSource = projectCodeList;
            this.ddlProjectCode.DataTextField = "FullName";
            this.ddlProjectCode.DataValueField = "ID";
            this.ddlProjectCode.DataBind();

            //var projectInPermission = UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
            //  ? this.scopeProjectService.GetAll()
            //  : this.scopeProjectService.GetAllInPermission(UserSession.Current.User.Id);
            //this.ddlProject.DataSource = projectInPermission;
            //this.ddlProject.DataTextField = "FullName";
            //this.ddlProject.DataValueField = "ID";
            //this.ddlProject.DataBind();

            ////var userList = this.userService.GetAll().OrderBy(t => t.FullName).ToList();

            //var toList = this.toListService.GetAll();

            //this.ddlToList.DataSource = toList;
            //this.ddlToList.DataTextField = "FullName";
            //this.ddlToList.DataValueField = "Id";
            //this.ddlToList.DataBind();

            //this.ddlFromList.DataSource = toList;
            //this.ddlFromList.DataTextField = "FullName";
            //this.ddlFromList.DataValueField = "Id";
            //this.ddlFromList.DataBind();
        }
    }
}