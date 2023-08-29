// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Web.Hosting;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using System.Linq;

    using EDMs.Web.Utilities.Sessions;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class DQRETransmittalEditForm : Page
    {
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

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
        public DQRETransmittalEditForm()
        {
            this.userService = new UserService();
            this.transmittalService = new DQRETransmittalService();
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

                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    this.CreatedInfo.Visible = true;
                    Guid id;
                    var objTran = this.transmittalService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if (objTran != null)
                    {
                        this.txtTransmittalNo.Text = objTran.TransmittalNo;
                        this.txtDescription.Text = objTran.Description;
                        if (objTran.OriginatingOrganizationId.ToString() != "")
                        { this.ddlOriginatingOrganization.SelectedValue = objTran.OriginatingOrganizationId.ToString(); }
                        if (objTran.ReceivingOrganizationId.ToString() != "")
                        { this.ddlReceivingOrganization.SelectedValue = objTran.ReceivingOrganizationId.ToString(); }
                        if (objTran.ProjectCodeId.ToString() != "")
                        { this.ddlProjectCode.SelectedValue = objTran.ProjectCodeId.ToString(); }
                        if (objTran.ConfidentialityId.ToString() != "")
                        { this.ddlConfidentiality.SelectedValue = objTran.ConfidentialityId.ToString(); }
                        this.txtDayIssued.SelectedDate = objTran.IssuedDate;
                        this.txtDueDate.SelectedDate = objTran.DueDate;
                        this.ddlIncomingTrans.SelectedValue = objTran.RefTransId.ToString();

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
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var objId = new Guid(this.Request.QueryString["objId"]);
                    var objTran = this.transmittalService.GetById(objId);
                    if (objTran != null)
                    {
                        this.CollectData(objTran);

                        objTran.IssuedDate = this.txtDayIssued.SelectedDate;
                        objTran.DueDate = this.txtDueDate.SelectedDate;
                        objTran.LastUpdatedBy = UserSession.Current.User.Id;
                        objTran.LastUpdatedDate = DateTime.Now;

                        this.transmittalService.Update(objTran);
                    }
                }
                else
                {
                    var objTran = new DQRETransmittal();
                    objTran.ID = Guid.NewGuid();

                    this.CollectData(objTran);

                    objTran.Status = "Missing Attach Document";
                    objTran.IsValid = false;
                    objTran.IsSend = false;
                    objTran.ErrorMessage = "Missing Attach Document";

                    objTran.IssuedDate = this.txtDayIssued.SelectedDate;
                    objTran.DueDate = this.txtDueDate.SelectedDate;
                    objTran.CreatedBy = UserSession.Current.User.Id;
                    objTran.CreatedDate = DateTime.Now;

                    // Create store folder
                    var physicalStoreFolder = Server.MapPath("../../DocumentLibrary/DQRETransmittal/" + objTran.TransmittalNo + "_" + objTran.CreatedDate.GetValueOrDefault().ToString("ddMMyyyyHHmmss"));
                    Directory.CreateDirectory(physicalStoreFolder);
                    Directory.CreateDirectory(physicalStoreFolder + @"\eTRM File");


                    var serverStoreFolder = (HostingEnvironment.ApplicationVirtualPath == "/"
                        ? string.Empty
                        : HostingEnvironment.ApplicationVirtualPath) + "/DocumentLibrary/DQRETransmittal/" + objTran.TransmittalNo + "_" + objTran.CreatedDate.GetValueOrDefault().ToString("ddMMyyyyHHmmss");
                    objTran.StoreFolderPath = serverStoreFolder;
                    // --------------------------------------------------------------------------

                    this.transmittalService.Insert(objTran);
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
        }

        private void CollectData(DQRETransmittal objTran)
        {
            objTran.TransmittalNo = this.txtTransmittalNo.Text.Trim();
            objTran.Description = this.txtDescription.Text.Trim();
            objTran.OriginatingOrganizationId = Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue);
            objTran.OriginatingOrganizationName = (this.ddlOriginatingOrganization.SelectedItem.Text.Trim());
            objTran.ReceivingOrganizationId = Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue);
            objTran.ReceivingOrganizationName = (this.ddlReceivingOrganization.SelectedItem.Text.Trim());
            objTran.ProjectCodeId = Convert.ToInt32(this.ddlProjectCode.SelectedValue);
            objTran.ProjectCodeName = (this.ddlProjectCode.SelectedItem.Text.Split(','))[0].Trim();
            objTran.ConfidentialityId = Convert.ToInt32(this.ddlConfidentiality.SelectedValue);
            objTran.ConfidentialityName = this.ddlConfidentiality.SelectedItem.Text;
            objTran.TypeId = 2;
            objTran.RefTransId = new Guid(this.ddlIncomingTrans.SelectedValue);
            objTran.RefTransNo = this.ddlIncomingTrans.SelectedItem.Text;
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
            //else if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            //{
                //var objId = Convert.ToInt32(Request.QueryString["objId"]);
                //var folderId = Convert.ToInt32(Request.QueryString["folId"]);
                //this.fileNameValidator.ErrorMessage = "The specified number is already in use.";
                //this.divFileName.Style["margin-bottom"] = "-26px;";
                //args.IsValid = !this.documentService.IsDocumentExistUpdate(folderId, this.txtTransmittalNo.Text.Trim(), objId);
            //}
        }

        //protected void ServerValidationDescription(object source, ServerValidateEventArgs args)
        //{
        //    if (this.txtDescription.Text.Trim().Length == 0)
        //    {
        //        this.descriptionValidator.ErrorMessage = "Please enter description.";
        //        this.divDescription.Style["margin-bottom"] = "-26px;";
        //        args.IsValid = false;
        //    }
        //}

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

            if (this.ddlProjectCode.SelectedItem != null)
            {
                var incomingTransList = this.transmittalService.GetAllByProject(Convert.ToInt32(this.ddlProjectCode.SelectedValue), 1, string.Empty);
                incomingTransList.Insert(0, new DQRETransmittal() { ID = Guid.NewGuid() });
                this.ddlIncomingTrans.DataSource = incomingTransList;
                this.ddlIncomingTrans.DataTextField = "TransmittalNo";
                this.ddlIncomingTrans.DataValueField = "ID";
                this.ddlIncomingTrans.DataBind();
            }
        }

        protected void Organization_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var fromId = Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue);
            var fromObj = this.organizationCodeService.GetById(fromId);

            var toId = Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue);
            var toObj = this.organizationCodeService.GetById(toId);
            if (fromObj != null)
            {
                this.txtTransmittalNo.Text = "T-" + fromObj.Code + "-";
            }

            if (toObj != null)
            {
                this.txtTransmittalNo.Text += toObj.Code;
            }
        }

        protected void ddlProjectCode_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlProjectCode.SelectedItem != null)
            {
                var incomingTransList = this.transmittalService.GetAllByProject(Convert.ToInt32(this.ddlProjectCode.SelectedValue), 1, string.Empty);
                incomingTransList.Insert(0, new DQRETransmittal() {ID = Guid.NewGuid()});
                this.ddlIncomingTrans.DataSource = incomingTransList;
                this.ddlIncomingTrans.DataTextField = "TransmittalNo";
                this.ddlIncomingTrans.DataValueField = "ID";
                this.ddlIncomingTrans.DataBind();
            }
        }
    }
}