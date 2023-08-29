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
using Aspose.Cells;
using EDMs.Business.Services.Workflow;
using EDMs.Web.Utilities;
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
    public partial class PECC2TransmittalFromToDoList : Page
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

        private readonly PECC2DocumentsService pecc2DocumentService;

        private readonly ObjectAssignedUserService objectAssignedUserService;

        private readonly AttachDocToTransmittalService attachDocToTransmittalService;

        private readonly WorkflowService wfService;
        private readonly PECC2DocumentAttachFileService PECC2DocumentAttachFileService;
        private readonly PECC2TransmittalAttachDocFileService PECC2TransmittalAttachDocFileService;
        private readonly GroupCodeService groupCodeService;
        private readonly ContractorTransmittalDocFileService contractorTransmittalDocFileService;

        private readonly PECC2TransmittalAttachFileService pecc2TransmittalAttachFileService;

        private readonly ChangeRequestService changeRequestService = new ChangeRequestService();
        private readonly ChangeRequestAttachFileService changeRequestAttachFileService = new ChangeRequestAttachFileService();
        private readonly ContractorTransmittalService contractorTransmittalService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public PECC2TransmittalFromToDoList()
        {
            this.userService = new UserService();
            this.transmittalService = new PECC2TransmittalService();
            this.projectCodeService = new ProjectCodeService();
            this.confidentialityServices = new ConfidentialityService();
            this.organizationCodeService = new OrganizationCodeService();
            this.pecc2DocumentService = new PECC2DocumentsService();
            this.objectAssignedUserService = new ObjectAssignedUserService();
            this.attachDocToTransmittalService = new AttachDocToTransmittalService();
            this.wfService = new WorkflowService();
            this.PECC2DocumentAttachFileService = new PECC2DocumentAttachFileService();
            this.PECC2TransmittalAttachDocFileService = new PECC2TransmittalAttachDocFileService();
            this.groupCodeService = new GroupCodeService();
            this.contractorTransmittalDocFileService = new ContractorTransmittalDocFileService();
            this.pecc2TransmittalAttachFileService = new PECC2TransmittalAttachFileService();
            this.contractorTransmittalService = new ContractorTransmittalService();
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
                this.LoadInitData();
                
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var incomingTransObj = this.transmittalService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if (incomingTransObj != null)
                    {
                        this.txtDescription.Text = incomingTransObj.Description;
                        if (incomingTransObj.OriginatingOrganizationId.ToString() != "")
                        {
                            this.ddlReceivingOrganization.SelectedValue = incomingTransObj.OriginatingOrganizationId.ToString();
                        }

                        if (incomingTransObj.ReceivingOrganizationId.ToString() != "")
                        {
                            this.ddlOriginatingOrganization.SelectedValue = incomingTransObj.ReceivingOrganizationId.ToString();
                        }

                        if (incomingTransObj.ProjectCodeId.ToString() != "")
                        {
                            this.ddlProjectCode.SelectedValue = incomingTransObj.ProjectCodeId.ToString();
                        }

                        if (incomingTransObj.ConfidentialityId.ToString() != "")
                        {
                            this.ddlConfidentiality.SelectedValue = incomingTransObj.ConfidentialityId.ToString();
                        }
                        this.ddlForSend.SelectedValue = incomingTransObj.ForSentId.GetValueOrDefault().ToString();
                        this.txtIncomingTrans.Text = incomingTransObj.TransmittalNo;

                        this.ddlGroup.SelectedValue = incomingTransObj.GroupId.ToString();
                        this.ddlCategory.SelectedValue = incomingTransObj.CategoryId.GetValueOrDefault().ToString();
                        this.txtVolumeNumber.Text = incomingTransObj.VolumeNumber;
                        this.txtFrom.Text = incomingTransObj.ToValue;
                        this.txtTo.Text = incomingTransObj.FromValue;
                        this.txtCC.Text = incomingTransObj.CCValue;
                        this.txtDayIssued.SelectedDate = DateTime.Now;
                        this.txtSubject.Text = incomingTransObj.Subject;
                        foreach (RadTreeNode actionNode in this.rtvCCOrganisation.Nodes)
                        {
                            actionNode.Checked = !string.IsNullOrEmpty(incomingTransObj.CCOrganizationId) && incomingTransObj.CCOrganizationId.Split(';').ToList().Contains(actionNode.Value);
                        }

                        var userList = this.userService.GetAll();
                        var transmittedUserList = userList.Where(t => t.Role.ContractorId == Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue)).OrderBy(t => t.UserNameWithFullName).ToList();
                        this.ddlTransmittedByName.DataSource = transmittedUserList;
                        this.ddlTransmittedByName.DataTextField = "UserNameWithFullName";
                        this.ddlTransmittedByName.DataValueField = "ID";
                        this.ddlTransmittedByName.DataBind();
                        this.ddlTransmittedByName.SelectedValue = UserSession.Current.User.Id.ToString();

                        var acknowledgedUserList = userList.Where(t => t.Role.ContractorId == Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue)).OrderBy(t => t.UserNameWithFullName).ToList();
                        this.ddlAcknowledgedByName.DataSource = acknowledgedUserList;
                        this.ddlAcknowledgedByName.DataTextField = "UserNameWithFullName";
                        this.ddlAcknowledgedByName.DataValueField = "ID";
                        this.ddlAcknowledgedByName.DataBind();
                        if (this.ddlTransmittedByName.SelectedItem != null)
                        {
                            var transmittedUser = this.userService.GetByID(Convert.ToInt32(this.ddlTransmittedByName.SelectedValue));
                            if (transmittedUser != null)
                            {
                                this.txtTransmittedByDesignation.Text = transmittedUser.Position;
                            }
                        }

                        if (this.ddlAcknowledgedByName.SelectedItem != null)
                        {
                            var acknowledgedUser = this.userService.GetByID(Convert.ToInt32(this.ddlAcknowledgedByName.SelectedValue));
                            if (acknowledgedUser != null)
                            {
                                this.txtAcknowledgedByDesignation.Text = acknowledgedUser.Position;
                            }
                        }
                        this.RegenerateTransNo();
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
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var incomingTransObj = this.transmittalService.GetById(new Guid(this.Request.QueryString["objId"]));
                if (incomingTransObj != null)
                {
                    var outgoingTransObj = new PECC2Transmittal();
                    outgoingTransObj.ID = Guid.NewGuid();

                    this.CollectData(outgoingTransObj);

                    outgoingTransObj.Status = "Missing Attach Document";
                    outgoingTransObj.IsValid = false;
                    outgoingTransObj.IsSend = false;
                    outgoingTransObj.ErrorMessage = "Missing Attach Document";

                    outgoingTransObj.IssuedDate = this.txtDayIssued.SelectedDate;
                    //objTran.DueDate = this.txtDueDate.SelectedDate;
                    outgoingTransObj.CreatedBy = UserSession.Current.User.Id;
                    outgoingTransObj.CreatedDate = DateTime.Now;

                    // Create store folder
                    var physicalStoreFolder =
                        Server.MapPath("../../DocumentLibrary/PECC2Transmittal/" + outgoingTransObj.TransmittalNo + "_" +
                                       outgoingTransObj.CreatedDate.GetValueOrDefault().ToString("ddMMyyyyHHmmss"));
                    Directory.CreateDirectory(physicalStoreFolder);
                    Directory.CreateDirectory(physicalStoreFolder + @"\eTRM File");

                    var serverStoreFolder = (HostingEnvironment.ApplicationVirtualPath == "/"
                        ? string.Empty
                        : HostingEnvironment.ApplicationVirtualPath) + "/DocumentLibrary/PECC2Transmittal/" +
                                            outgoingTransObj.TransmittalNo + "_" +
                                            outgoingTransObj.CreatedDate.GetValueOrDefault().ToString("ddMMyyyyHHmmss");
                    outgoingTransObj.StoreFolderPath = serverStoreFolder;
                    // --------------------------------------------------------------------------

                    var transId = this.transmittalService.Insert(outgoingTransObj);
                    if (transId != null)
                    {
                        incomingTransObj.IsCreateOutGoingTrans = true;
                        this.transmittalService.Update(incomingTransObj);
                        this.AttachDocToTrans(outgoingTransObj, incomingTransObj);

                        this.AddCRSAttachFile(outgoingTransObj, incomingTransObj);


                    }

                    this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
                }
            }
        }

        private void AddCRSAttachFile(PECC2Transmittal outgoingTransObj, PECC2Transmittal incomingTransObj)
        {
            var incomingCRSAttachFile = this.pecc2TransmittalAttachFileService.GetByTrans(incomingTransObj.ID).FirstOrDefault(t => t.TypeId == 2);
            if (incomingCRSAttachFile != null)
            {
                var incomingCRSAttachFileServerPath = Server.MapPath("../.." + incomingCRSAttachFile.FilePath);
                var fileInfo = new FileInfo(incomingCRSAttachFileServerPath);
                if (fileInfo.Exists)
                {
                    var targetFolder = "../.." + outgoingTransObj.StoreFolderPath + "/eTRM File";
                    var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                            + outgoingTransObj.StoreFolderPath + "/eTRM File";

                    var filename = Utility.RemoveSpecialCharacterFileName(outgoingTransObj.TransmittalNo) + "_CRS.xlsm";
                    // Path file to save on server disc
                    var saveFilePath = Path.Combine(Server.MapPath(targetFolder), filename);
                    // Path file to download from server
                    var serverFilePath = serverFolder + "/" + filename;
                    File.Copy(incomingCRSAttachFileServerPath, saveFilePath);
                    var outgoingCRSattachFile = new PECC2TransmittalAttachFiles()
                    {
                        ID = Guid.NewGuid(),
                        TransId = outgoingTransObj.ID,
                        Filename = filename,
                        Extension = "xlsm",
                        FilePath = serverFilePath,
                        ExtensionIcon = "~/images/excelfile.png",
                        FileSize = (double)fileInfo.Length / 1024,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.FullName,
                        CreatedDate = DateTime.Now,
                        TypeId = 2,
                        TypeName = "CRS File"
                    };

                    this.pecc2TransmittalAttachFileService.Insert(outgoingCRSattachFile);

                    // Update "Outgoing trans" info for CRS file
                    //var workbook = new Workbook();
                    //workbook = new Workbook( Server.MapPath(@"../.."+serverFilePath));
                    //var dataControlSheet = workbook.Worksheets[0];
                    //var currentSheetIndex = Convert.ToInt32(dataControlSheet.Cells["A1"].Value) - 1;
                    //var transSheet = workbook.Worksheets[currentSheetIndex];
                    //transSheet.Cells["H5"].PutValue(outgoingTransObj.TransmittalNo);
                    //transSheet.Cells["M5"].PutValue(outgoingTransObj.IssuedDate.GetValueOrDefault());
                    //workbook.Save(saveFilePath);
                    // -----------------------------------------------------------------------------------------------------------------
                }
            }
        }

        private void AttachDocToTrans(PECC2Transmittal outgoingTransObj, PECC2Transmittal incomingTransObj)
        {
            var contractorAttachDocFile = this.contractorTransmittalDocFileService.GetAllByTrans(incomingTransObj.ContractorTransId.GetValueOrDefault());
            var docIdList = contractorAttachDocFile.Where(t => t.PECC2ProjectDocId != null).Select(t => t.PECC2ProjectDocId.GetValueOrDefault()).Distinct().ToList();
            foreach (var docId in docIdList)
            {
                if (!this.attachDocToTransmittalService.IsExist(outgoingTransObj.ID, docId))
                {
                    var attachDoc = new AttachDocToTransmittal()
                    {
                        TransmittalId = outgoingTransObj.ID,
                        DocumentId = docId
                    };

                    this.attachDocToTransmittalService.Insert(attachDoc);

                    var contracDocObj = contractorAttachDocFile.FirstOrDefault(t => t.PECC2ProjectDocId == docId);
                    if (contracDocObj != null && contracDocObj.ChangeRequestTypeId == null )
                    {
                        // Get document file for outgoing trans
                        var docObj = this.pecc2DocumentService.GetById(docId);

                        docObj.OutgoingTransId = outgoingTransObj.ID;
                        docObj.OutgoingTransNo = outgoingTransObj.TransmittalNo;
                        this.pecc2DocumentService.Update(docObj);
                    
                    var markupCommentAttachDocList = this.PECC2DocumentAttachFileService.GetAllConsolidateByDocId(docId);//this.PECC2DocumentAttachFileService.GetAllCmtResByDocId(docId);
                    var i = 1;
                    foreach (var attachFile in markupCommentAttachDocList)
                    {
                        if (i == 1)
                        {
                            File.Copy(Server.MapPath("../.." + attachFile.FilePath), Path.Combine(Server.MapPath("../.." + outgoingTransObj.StoreFolderPath), attachFile.FileName), true);

                            var transAttachDocFile = new PECC2TransmittalAttachDocFiles
                            {
                                ID = Guid.NewGuid(),
                                DocumentId = docObj.ID,
                                TransId = outgoingTransObj.ID,
                                DocNo = docObj.DocNo,
                                DocTitle = docObj.DocTitle,
                                Revision = docObj.Revision,
                                FileName = attachFile.FileName,
                                ExtensionIcon = attachFile.ExtensionIcon,
                                Extension = attachFile.Extension,
                                FileSize = attachFile.FileSize,
                                FilePath = outgoingTransObj.StoreFolderPath + "/" + attachFile.FileName
                            };

                            this.PECC2TransmittalAttachDocFileService.Insert(transAttachDocFile);
                        }
                        i++;
                        }
                    }
                    else
                    {
                        var docObj = this.changeRequestService.GetById(docId);

                        docObj.OutgoingTransId = outgoingTransObj.ID;
                        docObj.OutgoingTransNo = outgoingTransObj.TransmittalNo;
                        this.changeRequestService.Update(docObj);

                        var markupCommentAttachDocList = this.changeRequestAttachFileService.GetByChangeRequest(docId).Where(t=> t.TypeId==3).OrderByDescending(t=> t.CreatedDate);//this.PECC2DocumentAttachFileService.GetAllCmtResByDocId(docId);
                        var i = 1;
                        foreach (var attachFile in markupCommentAttachDocList)
                        {
                            if (i == 1)
                            {
                                File.Copy(Server.MapPath("../.." + attachFile.FilePath), Path.Combine(Server.MapPath("../.." + outgoingTransObj.StoreFolderPath), attachFile.FileName), true);

                                var transAttachDocFile = new PECC2TransmittalAttachDocFiles
                                {
                                    ID = Guid.NewGuid(),
                                    DocumentId = docObj.ID,
                                    TransId = outgoingTransObj.ID,
                                    DocNo = docObj.Number,
                                    DocTitle = docObj.Description,
                                    Revision = docObj.Revision,
                                    FileName = attachFile.FileName,
                                    ExtensionIcon = attachFile.ExtensionIcon,
                                    Extension = attachFile.Extension,
                                    FileSize = attachFile.FileSize,
                                    FilePath = outgoingTransObj.StoreFolderPath + "/" + attachFile.FileName
                                };

                                this.PECC2TransmittalAttachDocFileService.Insert(transAttachDocFile);
                            }
                            i++;
                        }
                    }
                    // -----------------------------------------------------------------------------------------------
                }
            }

            // Update trans info
            if (docIdList.Count > 0)
            {
                outgoingTransObj.IsValid = true;
                outgoingTransObj.Status = string.Empty;
                outgoingTransObj.ErrorMessage = string.Empty;

                this.transmittalService.Update(outgoingTransObj);
            }
            // ---------------------------------------------------------

        }

        private void CollectData(PECC2Transmittal obj)
        {
            obj.TransmittalNo = this.txtTransmittalNo.Text.Trim();
            obj.Description = this.txtDescription.Text.Trim();
            obj.OriginatingOrganizationId = Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue);
            obj.OriginatingOrganizationName = (this.ddlOriginatingOrganization.SelectedItem.Text.Trim());
            obj.ReceivingOrganizationId = Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue);
            obj.ReceivingOrganizationName = (this.ddlReceivingOrganization.SelectedItem.Text.Trim());
            obj.ProjectCodeId = Convert.ToInt32(this.ddlProjectCode.SelectedValue);
            obj.ProjectCodeName = (this.ddlProjectCode.SelectedItem.Text.Split(','))[0].Trim();
            obj.ConfidentialityId = Convert.ToInt32(this.ddlConfidentiality.SelectedValue);
            obj.ConfidentialityName = this.ddlConfidentiality.SelectedItem.Text;
            obj.TypeId = 2;
            var incomingTransObj = this.transmittalService.GetById(new Guid(this.Request.QueryString["objId"]));
            if (incomingTransObj != null)
            {
                obj.RefTransId = incomingTransObj.ID;
                obj.RefTransNo = incomingTransObj.TransmittalNo;
            }

            obj.GroupId = Convert.ToInt32(this.ddlGroup.SelectedValue);
            obj.GroupCode = this.ddlGroup.SelectedItem.Text.Split(',')[0];
            obj.OtherTransNo = this.txtOtherTransNo.Text;
            obj.FromValue = this.txtFrom.Text;
            obj.ToValue = this.txtTo.Text;
            obj.CCValue = this.txtCC.Text;
            //objTran.IssuedDate = this.txtDayIssued.SelectedDate;
            obj.VolumeNumber = this.txtVolumeNumber.Text;
            obj.CategoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
            obj.CategoryName = this.ddlCategory.SelectedItem.Text;
            obj.Subject = this.txtSubject.Text;
            obj.TransmittedById = this.ddlTransmittedByName.SelectedItem != null
                                   ? Convert.ToInt32(this.ddlTransmittedByName.SelectedValue)
                                   : 0;
            obj.TransmittedByName = this.ddlTransmittedByName.SelectedItem != null
                                    ? this.ddlTransmittedByName.SelectedItem.Text
                                    : string.Empty;
            obj.TransmittedByDesignation = this.txtTransmittedByDesignation.Text;

            obj.AcknowledgedId = this.ddlAcknowledgedByName.SelectedItem != null
                                    ? Convert.ToInt32(this.ddlAcknowledgedByName.SelectedValue)
                                    : 0;
            obj.AcknowledgedByName = this.ddlAcknowledgedByName.SelectedItem != null
                                    ? this.ddlAcknowledgedByName.SelectedItem.Text
                                    : string.Empty;
            obj.AcknowledgedByDesignation = this.txtAcknowledgedByDesignation.Text;
            obj.Remark = this.txtRemark.Text;
            obj.Year = DateTime.Now.Year;
            obj.Sequence = Convert.ToInt32(this.txtTransmittalNo.Text.Trim().Split('-')[6]);
            obj.SequenceString = this.txtTransmittalNo.Text.Trim().Split('-')[6];

            obj.ForSentId = Convert.ToInt32(this.ddlForSend.SelectedValue);
            obj.ForSentName = this.ddlForSend.SelectedItem.Text;

            obj.CCOrganizationId = string.Empty;
            obj.CCOrganizationName = string.Empty;
            foreach (RadTreeNode actionNode in this.rtvCCOrganisation.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
            {
                obj.CCOrganizationId += actionNode.Value + ";";
                obj.CCOrganizationName += actionNode.Text + Environment.NewLine;
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
                //this.descriptionValidator.ErrorMessage = "Please enter description.";
                this.divDescription.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadInitData()
        {
            this.txtDayIssued.SelectedDate = DateTime.Now;

            var groupList = this.groupCodeService.GetAll();
            this.ddlGroup.DataSource = groupList;
            this.ddlGroup.DataTextField = "FullName";
            this.ddlGroup.DataValueField = "ID";
            this.ddlGroup.DataBind();

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
            this.rtvCCOrganisation.DataSource = organizationCodeList;
            this.rtvCCOrganisation.DataTextField = "FullName";
            this.rtvCCOrganisation.DataValueField = "Id";
            this.rtvCCOrganisation.DataBind();
            var projectCodeList = this.projectCodeService.GetAll().OrderBy(t => t.Code).ToList();
            this.ddlProjectCode.DataSource = projectCodeList;
            this.ddlProjectCode.DataTextField = "FullName";
            this.ddlProjectCode.DataValueField = "ID";
            this.ddlProjectCode.DataBind();

            //if (this.ddlProjectCode.SelectedItem != null)
            //{
            //    var incomingTransList = this.transmittalService.GetAllByProject(Convert.ToInt32(this.ddlProjectCode.SelectedValue), 1, string.Empty);
            //    incomingTransList.Insert(0, new PECC2Transmittal() { ID = Guid.NewGuid() });
            //    this.ddlIncomingTrans.DataSource = incomingTransList;
            //    this.ddlIncomingTrans.DataTextField = "TransmittalNo";
            //    this.ddlIncomingTrans.DataValueField = "ID";
            //    this.ddlIncomingTrans.DataBind();
            //}
        }

        protected void Organization_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.RegenerateTransNo();

            var userList = this.userService.GetAll();
            var transmittedUserList = userList.Where(t => t.Role.ContractorId == Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue)).OrderBy(t => t.UserNameWithFullName).ToList();
            
            this.ddlTransmittedByName.DataSource = transmittedUserList;
            this.ddlTransmittedByName.DataTextField = "UserNameWithFullName";
            this.ddlTransmittedByName.DataValueField = "ID";
            this.ddlTransmittedByName.DataBind();

            if (this.ddlTransmittedByName.SelectedItem != null)
            {
                var transmittedUser = this.userService.GetByID(Convert.ToInt32(this.ddlTransmittedByName.SelectedValue));
                if (transmittedUser != null)
                {
                    this.txtTransmittedByDesignation.Text = transmittedUser.Position;
                }
            }

            var acknowledgedUserList = userList.Where(t => t.Role.ContractorId == Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue)).OrderBy(t => t.UserNameWithFullName).ToList();
            this.ddlAcknowledgedByName.DataSource = acknowledgedUserList;
            this.ddlAcknowledgedByName.DataTextField = "UserNameWithFullName";
            this.ddlAcknowledgedByName.DataValueField = "ID";
            this.ddlAcknowledgedByName.DataBind();

            if (this.ddlAcknowledgedByName.SelectedItem != null)
            {
                var acknowledgedUser = this.userService.GetByID(Convert.ToInt32(this.ddlAcknowledgedByName.SelectedValue));
                if (acknowledgedUser != null)
                {
                    this.txtAcknowledgedByDesignation.Text = acknowledgedUser.Position;
                }
            }
        }

        protected void ddlProjectCode_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.ddlProjectCode.SelectedItem != null)
            //{
            //    var incomingTransList = this.transmittalService.GetAllByProject(Convert.ToInt32(this.ddlProjectCode.SelectedValue), 1, string.Empty);
            //    incomingTransList.Insert(0, new PECC2Transmittal() {ID = Guid.NewGuid()});
            //    this.ddlIncomingTrans.DataSource = incomingTransList;
            //    this.ddlIncomingTrans.DataTextField = "TransmittalNo";
            //    this.ddlIncomingTrans.DataValueField = "ID";
            //    this.ddlIncomingTrans.DataBind();
            //}
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var incomingTransObj = this.transmittalService.GetById(new Guid(this.Request.QueryString["objId"]));
                if (incomingTransObj != null)
                {
                    var contractorAttachDocFile =
                        this.contractorTransmittalDocFileService.GetAllByTrans(
                            incomingTransObj.ContractorTransId.GetValueOrDefault());
                    var docIdList =
                        contractorAttachDocFile.Where(t => t.PECC2ProjectDocId != null)
                            .Select(t => t.PECC2ProjectDocId.GetValueOrDefault())
                            .Distinct()
                            .ToList();
                    var docList = this.pecc2DocumentService.GetSpecialDocList(docIdList).OrderBy(t => t.DocNo).ToList();
                    
                    if (incomingTransObj.ForSentId == 2)
                    {
                        var contractorTransObj = this.contractorTransmittalService.GetById(incomingTransObj.ContractorTransId.GetValueOrDefault());
                        var changeRequestDocFile = this.contractorTransmittalDocFileService.GetAllByTrans(contractorTransObj.ID).FirstOrDefault(t => t.TypeId == 2);
                        if (changeRequestDocFile != null)
                        {
                            var changeRequestObj =
                                this.changeRequestService.GetById(changeRequestDocFile.PECC2ProjectDocId
                                    .GetValueOrDefault());
                            if (changeRequestObj != null)
                            {
                                docList.Insert(0, new PECC2Documents()
                                {
                                    ID = changeRequestObj.ID,
                                    IsChangeRequest = true,
                                    DocNo = changeRequestObj.Number,
                                    DocActionCode = changeRequestObj.ActionCodeName,
                                    Revision = changeRequestObj.Revision,
                                    DocReviewStatusCode = changeRequestObj.ReviewResultName,
                                    DocTitle = changeRequestDocFile.DocumentTitle,
                                    IncomingTransNo = changeRequestObj.IncomingTransNo,
                                });
                            }
                        }
                    }

                    this.grdDocument.DataSource = docList;
                }
            }
        }

        protected void ddlGroup_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.RegenerateTransNo();
        }

        private void RegenerateTransNo()
        {
            var fromId = Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue);
            var fromObj = this.organizationCodeService.GetById(fromId);

            var toId = Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue);
            var toObj = this.organizationCodeService.GetById(toId);

            this.txtTransmittalNo.Text = this.ddlProjectCode.SelectedItem.Text.Split(',')[0] + "-T-";

            if (fromObj != null)
            {
                this.txtTransmittalNo.Text += fromObj.Code + "-";
            }

            if (toObj != null)
            {
                this.txtTransmittalNo.Text += toObj.Code + "-";
            }

            this.txtTransmittalNo.Text += DateTime.Now.Year.ToString().Substring(2, 2) + "-";
            this.txtTransmittalNo.Text += this.ddlGroup.SelectedItem.Text.Split(',')[0] + "-";

            var sequence = Utilities.Utility.ReturnSequenceString(this.transmittalService.GetCurrentSequence(DateTime.Now.Year, Convert.ToInt32(this.ddlGroup.SelectedValue)), 4);
            this.txtTransmittalNo.Text += sequence;
        }

        protected void ddlTransmittedByName_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var transmittedUser = this.userService.GetByID(Convert.ToInt32(this.ddlTransmittedByName.SelectedValue));
            if (transmittedUser != null)
            {
                this.txtTransmittedByDesignation.Text = transmittedUser.Position;
            }
        }

        protected void ddlAcknowledgedByName_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var acknowledgedUser = this.userService.GetByID(Convert.ToInt32(this.ddlAcknowledgedByName.SelectedValue));
            if (acknowledgedUser != null)
            {
                this.txtAcknowledgedByDesignation.Text = acknowledgedUser.Position;
            }
        }
    }
}