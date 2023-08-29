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
using EDMs.Business.Services.Workflow;
using Telerik.Web.UI;

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
    public partial class DQRETransmittalFromToDoList : Page
    {
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// The transmittal service.
        /// </summary>
        private readonly PECC2TransmittalService transmittalService;
        /// <summary>
        /// The scope project services
        /// </summary>
        private readonly ProjectCodeService projectCodeService;

        /// <summary>
        /// The confidentiality services
        /// </summary>
        private readonly ConfidentialityService confidentialityServices;

        private readonly OrganizationCodeService organizationCodeService;

        private readonly PECC2DocumentsService PECC2DocumentService;

        private readonly ObjectAssignedUserService objectAssignedUserService;

        private readonly AttachDocToTransmittalService attachDocToTransmittalService;

        private readonly WorkflowService wfService;
        private readonly PECC2DocumentAttachFileService PECC2DocumentAttachFileService;
        private readonly PECC2TransmittalAttachDocFileService PECC2TransmittalAttachDocFileService;



        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public DQRETransmittalFromToDoList()
        {
            this.userService = new UserService();
            this.transmittalService = new PECC2TransmittalService();
            this.projectCodeService = new ProjectCodeService();
            this.confidentialityServices = new ConfidentialityService();
            this.organizationCodeService = new OrganizationCodeService();
            this.PECC2DocumentService = new PECC2DocumentsService();
            this.objectAssignedUserService = new ObjectAssignedUserService();
            this.attachDocToTransmittalService = new AttachDocToTransmittalService();
            this.wfService = new WorkflowService();
            this.PECC2DocumentAttachFileService = new PECC2DocumentAttachFileService();
            this.PECC2TransmittalAttachDocFileService = new PECC2TransmittalAttachDocFileService();
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
                    var objTran = new PECC2Transmittal();
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
                    var physicalStoreFolder = Server.MapPath("../../DocumentLibrary/PECC2Transmittal/" + objTran.TransmittalNo + "_" + objTran.CreatedDate.GetValueOrDefault().ToString("ddMMyyyyHHmmss"));
                    Directory.CreateDirectory(physicalStoreFolder);
                    Directory.CreateDirectory(physicalStoreFolder + @"\eTRM File");

                    var serverStoreFolder = (HostingEnvironment.ApplicationVirtualPath == "/"
                        ? string.Empty
                        : HostingEnvironment.ApplicationVirtualPath) + "/DocumentLibrary/PECC2Transmittal/" + objTran.TransmittalNo + "_" + objTran.CreatedDate.GetValueOrDefault().ToString("ddMMyyyyHHmmss");
                    objTran.StoreFolderPath = serverStoreFolder;
                    // --------------------------------------------------------------------------

                    var transId = this.transmittalService.Insert(objTran);
                    if (transId != null)
                    {
                        this.AttachDocToTrans(objTran);
                    }
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
        }

        private void AttachDocToTrans(PECC2Transmittal objTrans)
        {
            var taskIdList = this.Request.QueryString["objIds"].Split('_').Where(t => !string.IsNullOrEmpty(t)).Select(t => new Guid(t)).ToList();
            var taskList = this.objectAssignedUserService.GetSpecialObjectAssignedUsers(taskIdList);
            var docIdList = taskList.Select(t => t.ObjectID.GetValueOrDefault()).ToList();
            foreach (var docId in docIdList)
            {
                if (!this.attachDocToTransmittalService.IsExist(objTrans.ID, docId))
                {
                    var attachDoc = new AttachDocToTransmittal()
                    {
                        TransmittalId = objTrans.ID,
                        DocumentId = docId
                    };

                    this.attachDocToTransmittalService.Insert(attachDoc);

                    // Get document file for outgoing trans
                    var docObj = this.PECC2DocumentService.GetById(docId);
                    var markupCommentAttachDocList = this.PECC2DocumentAttachFileService.GetAllCmtResByDocId(docId);
                    foreach (var attachFile in markupCommentAttachDocList)
                    {
                        File.Copy(Server.MapPath("../.." + attachFile.FilePath), Path.Combine(Server.MapPath("../.." + objTrans.StoreFolderPath), attachFile.FileName), true);

                        var transAttachDocFile = new PECC2TransmittalAttachDocFiles
                        {
                            ID = Guid.NewGuid(),
                            DocumentId = docObj.ID,
                            TransId = objTrans.ID,
                            DocNo = docObj.DocNo,
                            DocTitle = docObj.DocTitle,
                            Revision = docObj.Revision,
                            FileName = attachFile.FileName,
                            ExtensionIcon = attachFile.ExtensionIcon,
                            Extension = attachFile.Extension,
                            FileSize = attachFile.FileSize,
                            FilePath = objTrans.StoreFolderPath + "/" + attachFile.FileName
                        };

                        this.PECC2TransmittalAttachDocFileService.Insert(transAttachDocFile);
                    }
                    // -----------------------------------------------------------------------------------------------
                }
            }

            // Update trans info
            if (docIdList.Count > 0)
            {
                objTrans.IsValid = true;
                objTrans.Status = string.Empty;
                objTrans.ErrorMessage = string.Empty;

                this.transmittalService.Update(objTrans);
            }
            // ---------------------------------------------------------

            // Complete task
            foreach (var currentWorkAssignedUserId in taskIdList)
            {
                var currentWorkAssignedUser = this.objectAssignedUserService.GetById(currentWorkAssignedUserId);
                if (currentWorkAssignedUser != null)
                {
                    var wfObj = this.wfService.GetById(currentWorkAssignedUser.WorkflowId.GetValueOrDefault());
                    var docObj = this.PECC2DocumentService.GetById(currentWorkAssignedUser.ObjectID.GetValueOrDefault());

                    // Update Current work assign
                    currentWorkAssignedUser.CommentContent = "Complete create outgoing transmittal.";
                    currentWorkAssignedUser.IsReject = false;
                    currentWorkAssignedUser.IsComplete = true;
                    currentWorkAssignedUser.ActualDate = DateTime.Now;
                    currentWorkAssignedUser.IsOverDue =
                        currentWorkAssignedUser.PlanCompleteDate.GetValueOrDefault().Date <
                        currentWorkAssignedUser.ActualDate.GetValueOrDefault().Date;
                    this.objectAssignedUserService.Update(currentWorkAssignedUser);
                    // ---------------------------------------------------------------------------------------------

                    // Update pending management work assign
                    var managementPendingList = this.objectAssignedUserService.GetAllManagementIncompleteByDoc(currentWorkAssignedUser.ObjectID.GetValueOrDefault(), wfObj.ID);
                    foreach (var managementAssign in managementPendingList)
                    {
                        managementAssign.IsComplete = true;
                        managementAssign.ActualDate = DateTime.Now;
                        this.objectAssignedUserService.Update(managementAssign);
                    }
                    // ------------------------------------------------------------------------------------------------------------

                    // Check document already in another WF
                    var anotherPending = this.objectAssignedUserService.GetAllIncompleteByDoc(currentWorkAssignedUser.ObjectID.GetValueOrDefault());
                    if (anotherPending.Count == 0)
                    {
                        docObj.IsInWFProcess = false;
                        docObj.IsWFComplete = true;

                        this.PECC2DocumentService.Update(docObj);
                    }
                    // ---------------------------------------------------------------------------------------------
                }
            }
            // -----------------------------------------------------------------------------------------------------
        }

        private void CollectData(PECC2Transmittal objTran)
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

            if (this.ddlProjectCode.SelectedItem != null)
            {
                var incomingTransList = this.transmittalService.GetAllByProject(Convert.ToInt32(this.ddlProjectCode.SelectedValue), 1, string.Empty);
                incomingTransList.Insert(0, new PECC2Transmittal() { ID = Guid.NewGuid() });
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
                incomingTransList.Insert(0, new PECC2Transmittal() {ID = Guid.NewGuid()});
                this.ddlIncomingTrans.DataSource = incomingTransList;
                this.ddlIncomingTrans.DataTextField = "TransmittalNo";
                this.ddlIncomingTrans.DataValueField = "ID";
                this.ddlIncomingTrans.DataBind();
            }
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["objIds"]))
            {
                var taskIdList = this.Request.QueryString["objIds"].Split('_').Where(t => !string.IsNullOrEmpty(t)).Select(t => new Guid(t)).ToList();
                var taskList = this.objectAssignedUserService.GetSpecialObjectAssignedUsers(taskIdList);
                var docIdList = taskList.Select(t => t.ObjectID.GetValueOrDefault()).ToList();
                var docList = this.PECC2DocumentService.GetSpecialDocList(docIdList);

                this.grdDocument.DataSource = docList;
            }
        }
    }
}