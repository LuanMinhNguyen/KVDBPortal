// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Cells;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
namespace EDMs.Web.Controls.Document
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class DQRETransmittalList : Page
    {
        private readonly ProjectCodeService projectCodeService = new ProjectCodeService();

        private readonly ContractorTransmittalService contractorTransmittalService = new ContractorTransmittalService();

        private readonly ContractorTransmittalDocFileService contractorTransmittalDocFileService = new ContractorTransmittalDocFileService();

        private readonly DQRETransmittalService dqreTransmittalService = new DQRETransmittalService();


        private readonly DQREDocumentService documentService = new DQREDocumentService();

        private readonly OrganizationCodeService organizationCodeService = new OrganizationCodeService();

        private readonly AttachDocToTransmittalService attachDocToTransmittalService = new AttachDocToTransmittalService();

        private readonly DQREDocumentMasterService documentMasterService = new DQREDocumentMasterService();

        private readonly DQREDocumentService documentProjectService = new DQREDocumentService();

        private readonly DQREDocumentAttachFileService documentAttachFileService = new DQREDocumentAttachFileService();

        private readonly RevisionStatuService revisionStatusService = new RevisionStatuService();

        private readonly PurposeCodeService purposeCodeService = new PurposeCodeService();

        private readonly UserService userService = new UserService();

        private readonly RoleService roleService = new RoleService();

        private readonly AreaService areaService = new AreaService();

        private readonly ObjectAssignedUserService objectAssignedUser = new  ObjectAssignedUserService();

        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string UnreadPattern = @"\(\d+\)";


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
            Session.Add("SelectedMainMenu", "Transmittals Management");

            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            var temp = (RadPane)this.Master.FindControl("leftPane");
            temp.Collapsed = true;
            if (!Page.IsPostBack)
            {
                this.LoadComboData();
            }
        }

        private void LoadComboData()
        {
            var ddlProjectOutgoing = (RadComboBox)this.radMenuOutgoing.Items[2].FindControl("ddlProjectOutgoing");
            var ddlProjectIncoming = (RadComboBox)this.radMenuIncoming.Items[1].FindControl("ddlProjectIncoming");
            var projectList = this.projectCodeService.GetAll().OrderBy(t => t.Code);

            if (ddlProjectOutgoing != null)
            {
                ddlProjectOutgoing.DataSource = projectList;
                ddlProjectOutgoing.DataTextField = "FullName";
                ddlProjectOutgoing.DataValueField = "ID";
                ddlProjectOutgoing.DataBind();

                int projectId = Convert.ToInt32(ddlProjectOutgoing.SelectedValue);
                this.lblProjectOutgoingId.Value = projectId.ToString();
                Session.Add("SelectedProject", projectId);
            }

            if (ddlProjectIncoming != null)
            {
                ddlProjectIncoming.DataSource = projectList;
                ddlProjectIncoming.DataTextField = "FullName";
                ddlProjectIncoming.DataValueField = "ID";
                ddlProjectIncoming.DataBind();

                int projectId = Convert.ToInt32(ddlProjectIncoming.SelectedValue);
                this.lblProjectIncomingId.Value = projectId.ToString();
            }
        }

        /// <summary>
        /// RadAjaxManager1  AjaxRequest
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                this.grdIncomingTrans.Rebind();
            }
            else if (e.Argument.Contains("DeleteTrans_"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var transObj = this.dqreTransmittalService.GetById(objId);
                if (!string.IsNullOrEmpty(transObj?.StoreFolderPath))
                {
                    var folderPath = Server.MapPath("../.." + transObj.StoreFolderPath);
                    if (Directory.Exists(folderPath))
                    {
                        Directory.Delete(folderPath,true);
                    }
                }

                this.dqreTransmittalService.Delete(objId);
                this.grdOutgoingTrans.Rebind();
            }
            else if (e.Argument.Contains("Export"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var transObj = this.dqreTransmittalService.GetById(objId);
                if (transObj != null)
                {
                    this.ExportETRM(transObj);
                }
            }
            else if (e.Argument.Contains("ExportContractorETRM"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var transObj = this.contractorTransmittalService.GetById(objId);
                if (transObj != null)
                {
                    this.ExportContractorETRM(transObj);
                }
            }
            else if (e.Argument.Contains("ImportDocument"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var objDQREId = new Guid(e.Argument.Split('_')[2]);
                var contractorTransObj = this.contractorTransmittalService.GetById(objId);
                var dqreTransIn = this.dqreTransmittalService.GetById(objDQREId);
                if (contractorTransObj != null)
                {
                    this.ImportTransDocument(contractorTransObj, objDQREId);
                    this.grdIncomingTrans.Rebind();
                }
            }
            else if (e.Argument.Contains("SendTrans"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var transObj = this.dqreTransmittalService.GetById(objId);
                if (transObj != null)
                {
                    var contractorTrans = new ContractorTransmittal();
                    contractorTrans.ID = Guid.NewGuid();
                    contractorTrans.DQRETransId = transObj.ID;
                    contractorTrans.Status = string.Empty;
                    contractorTrans.IsValid = true;
                    contractorTrans.IsSend = false;
                    contractorTrans.IsOpen = false;
                    contractorTrans.ErrorMessage = string.Empty;

                    contractorTrans.TransNo = transObj.TransmittalNo;
                    contractorTrans.ProjectId = transObj.ProjectCodeId;
                    contractorTrans.ProjectName = transObj.ProjectCodeName;
                    contractorTrans.FromId = transObj.OriginatingOrganizationId;
                    contractorTrans.FromName = transObj.OriginatingOrganizationName;
                    contractorTrans.ToId = transObj.ReceivingOrganizationId;
                    contractorTrans.ToName = transObj.ReceivingOrganizationName;
                    contractorTrans.TransDate = transObj.IssuedDate;
                    contractorTrans.TransDate = transObj.IssuedDate;
                    contractorTrans.DueDate = transObj.DueDate;
                    contractorTrans.ReceivedDate = DateTime.Now;
                    contractorTrans.TypeId = 1;

                    var contractorTransId = this.contractorTransmittalService.Insert(contractorTrans);
                    if (contractorTransId != null)
                    {
                        transObj.ContractorTransId = contractorTransId;
                        transObj.IsSend = true;
                        transObj.ReceivedDate = DateTime.Now;

                        this.dqreTransmittalService.Update(transObj);
                        //update transout on documentproject
                        this.UpdateTransOutToDocument(transObj);
                        //send email to dc contractors
                       if(Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"])) this.NotifiNewTransmittal(transObj);
                        //send email to all user 
                        if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"])) this.NotifiNewTransmittalOut(transObj);
                    }

                    this.grdOutgoingTrans.Rebind();
                }
            }
            else if (e.Argument.Contains("Undo"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var transObj = this.dqreTransmittalService.GetById(objId);
                if (transObj != null)
                {
                    var contractorTrans = this.contractorTransmittalService.GetById(transObj.ContractorTransId.GetValueOrDefault());
                    if (contractorTrans != null) {
                        this.contractorTransmittalService.Delete(contractorTrans);
                        transObj.ContractorTransId = null;
                        transObj.IsSend = false;
                        transObj.ReceivedDate = null;
                        this.dqreTransmittalService.Update(transObj);
                        this.grdOutgoingTrans.Rebind();
                    }
                   
                }
            }
        }

        private void ImportTransDocument(ContractorTransmittal contractorTransObj, Guid objDQREId)
        {
            var dqreIncomingTrans = this.dqreTransmittalService.GetById(contractorTransObj.DQRETransId.GetValueOrDefault());
            var transPurpose = this.purposeCodeService.GetById(contractorTransObj.PurposeId.GetValueOrDefault());
            var revisionStatus = this.revisionStatusService.GetByCode(transPurpose.Code);

            var fullDocList = this.contractorTransmittalDocFileService.GetAllByTrans(contractorTransObj.ID);
            var filterDocList = new List<ContractorTransmittalDocFile>();

            // Remove duplicate Document
            foreach (var document in fullDocList)
            {
                if (filterDocList.All(t => t.DocumentNo != document.DocumentNo))
                {
                    filterDocList.Add(document);
                }
            }
            // --------------------------------------------------------------------------------------------

            // Process import
            foreach (var contractorDoc in filterDocList)
            {
                // Get attach doc list
                var contractorDocAttach = fullDocList.Where(t => t.DocumentNo == contractorDoc.DocumentNo).ToList();
                // ----------------------------------------------------------------------------------------

                var currentProjectDocList = this.documentProjectService.GetAllByProjectDocNo(contractorDoc.DocumentNoFull);

                Guid DQREDocId=contractorDoc.ID;
                // Case: Already have previous document
                if (currentProjectDocList.Count > 0)
                {
                    var currentLeafProjectDoc = currentProjectDocList.FirstOrDefault(t => t.IsLeaf.GetValueOrDefault());
                    if (currentLeafProjectDoc != null)
                    {
                        var projectDoc = currentLeafProjectDoc;

                        if (projectDoc.Revision.ToLower().Trim() == contractorDoc.Revision.ToLower().Trim()
                            && !projectDoc.IsHasAttachFile.GetValueOrDefault())
                        {
                            // Fill incoming trans info
                            projectDoc.IncomingTransId = dqreIncomingTrans.ID;
                            projectDoc.IncomingTransNo = dqreIncomingTrans.TransmittalNo;

                            this.documentProjectService.Update(projectDoc);
                            //-------------------------------------------------------------------------------
                        }
                        else
                        {
                            // Collect new project doc info
                            projectDoc = new DQREDocument();
                            this.CollectProjectDocData(contractorDoc, projectDoc);

                            projectDoc.ProjectCodeId = currentLeafProjectDoc.ProjectCodeId;
                            projectDoc.ProjectCodeName = currentLeafProjectDoc.ProjectCodeName;
                            projectDoc.RevisionSchemaId = currentLeafProjectDoc.RevisionSchemaId;
                            projectDoc.RevisionSchemaName = currentLeafProjectDoc.RevisionSchemaName;
                            if (revisionStatus != null)
                            {
                                projectDoc.RevisionStatusId = revisionStatus.ID;
                                projectDoc.RevisionStatusName = revisionStatus.FullName;
                            }

                            projectDoc.CreatedBy = UserSession.Current.User.Id;
                            projectDoc.CreatedDate = DateTime.Now;
                            projectDoc.IsLeaf = true;
                            projectDoc.IsDelete = false;
                            projectDoc.IsHasAttachFile = true;
                            projectDoc.ParentId = currentLeafProjectDoc.ParentId ?? currentLeafProjectDoc.ID;
                            projectDoc.IsssuedDate = contractorTransObj.TransDate;
                            // Fill incoming trans info
                            projectDoc.IncomingTransId = dqreIncomingTrans.ID;
                            projectDoc.IncomingTransNo = dqreIncomingTrans.TransmittalNo;
                            //-------------------------------------------------------------------------------

                            // get master info
                            if (currentLeafProjectDoc.DocumentMasterId != null)
                            {
                                var masterDoc = this.documentMasterService.GetById(currentLeafProjectDoc.DocumentMasterId.GetValueOrDefault());
                                if (masterDoc != null)
                                {
                                    this.CollectMasterInfo(masterDoc, projectDoc);
                                }
                            }

                            // ------------------------------------------------------------------------------------------------------------------

                            this.documentProjectService.Insert(projectDoc);
                            DQREDocId = projectDoc.ID;
                            // ------------------------------------------------------------------------------------------------------------------
                            if(objDQREId != null && !string.IsNullOrEmpty(objDQREId.ToString()))
                            {
                                var attachDoc = new AttachDocToTransmittal()
                            {
                                TransmittalId = objDQREId,
                                DocumentId = projectDoc.ID
                                };
                            if (!this.attachDocToTransmittalService.IsExist(objDQREId, projectDoc.ID))
                            {
                                this.attachDocToTransmittalService.Insert(attachDoc);
                            }
                            }
                           

                            // Update leaf project doc
                            currentLeafProjectDoc.IsLeaf = false;
                            this.documentProjectService.Update(currentLeafProjectDoc);

                            // -------------------------------------------------------------------------------------------------------------
                        }

                        //Attach doc file to project doc
                        this.AttachDocFileToProjectDoc(contractorDocAttach, projectDoc);
                        // --------------------------------------------------------------------------------------------------------------

                        // Update DQRE Incoming trans info
                        dqreIncomingTrans.Status = string.Empty;
                        dqreIncomingTrans.ErrorMessage = string.Empty;
                        dqreIncomingTrans.IsImport = true;

                        this.dqreTransmittalService.Update(dqreIncomingTrans);
                        // --------------------------------------------------------------------------------------------------------------
                    }
                }
                // -----------------------------------------------------------------------------------------------------
                // Case: Document sent by contractor is new doc
                else
                {
                    var masterDoc = this.documentMasterService.GetOneByDocNo(contractorDoc.DocumentNo);

                    // Create new DocMaster
                    if (masterDoc == null)
                    {
                        masterDoc = new DQREDocumentMaster()
                        {
                            ID = Guid.NewGuid(),
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedDate = DateTime.Now,
                            IsDelete = false
                        };

                        this.CollectMasterDocData(contractorTransObj, contractorDoc, masterDoc);

                        this.documentMasterService.Insert(masterDoc);
                    }
                    // -------------------------------------------------------------------------------------------------

                    // Collect new project doc info
                    var projectDoc = new DQREDocument();
                    this.CollectProjectDocData(contractorDoc, projectDoc);

                    projectDoc.ProjectCodeId = contractorDoc.ProjectId;
                    projectDoc.ProjectCodeName = contractorDoc.ProjectName;
                    projectDoc.RevisionSchemaId = 0;
                    projectDoc.RevisionSchemaName = string.Empty;
                    if (revisionStatus != null)
                    {
                        projectDoc.RevisionStatusId = revisionStatus.ID;
                        projectDoc.RevisionStatusName = revisionStatus.FullName;
                    }

                    projectDoc.CreatedBy = UserSession.Current.User.Id;
                    projectDoc.CreatedDate = DateTime.Now;
                    projectDoc.IsHasAttachFile = true;
                    projectDoc.IsLeaf = true;
                    projectDoc.IsDelete = false;

                    // Fill incoming trans info
                    projectDoc.IncomingTransId = dqreIncomingTrans.ID;
                    projectDoc.IncomingTransNo = dqreIncomingTrans.TransmittalNo;
                    //-------------------------------------------------------------------------------

                    // get master info
                    this.CollectMasterInfo(masterDoc, projectDoc);
                    // -----------------------------------------------------------------------------------------------------

                    this.documentProjectService.Insert(projectDoc);
                    DQREDocId = projectDoc.ID;
                    if (objDQREId != null && !string.IsNullOrEmpty(objDQREId.ToString()))
                    {
                        var attachDoc = new AttachDocToTransmittal()
                        {
                            TransmittalId = objDQREId,
                            DocumentId = projectDoc.ID
                        };
                        if (!this.attachDocToTransmittalService.IsExist(objDQREId, projectDoc.ID))
                        {
                            this.attachDocToTransmittalService.Insert(attachDoc);
                        }
                    }
                    //update contractorTransDocFileObj
                    //Attach doc file to project doc
                    this.AttachDocFileToProjectDoc(contractorDocAttach, projectDoc);
                    // --------------------------------------------------------------------------------------------------------------

                    // Update DQRE Incoming trans info
                    dqreIncomingTrans.Status = string.Empty;
                    dqreIncomingTrans.ErrorMessage = string.Empty;
                    dqreIncomingTrans.IsImport = true;

                    this.dqreTransmittalService.Update(dqreIncomingTrans);
                    // --------------------------------------------------------------------------------------------------------------
                }
                // -----------------------------------------------------------------------------------------------------
                contractorDoc.DQREProjectDocId = DQREDocId != contractorDoc.ID ? DQREDocId: contractorDoc.DQREProjectDocId;
                contractorDoc.IsReject = false;
                this.contractorTransmittalDocFileService.Update(contractorDoc);
            }
            // ---------------------------------------------------------------------------------------------
        }

        private void AttachDocFileToProjectDoc(List<ContractorTransmittalDocFile> attachList, DQREDocument projectDoc)
        {
            var targetFolder = "../../DocumentLibrary/ProjectDocs";
            var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                + "/DocumentLibrary/ProjectDocs";
            foreach (var contractorAttachFile in attachList)
            {
                var docFileName = contractorAttachFile.FileName;

                // Path file to save on server disc
                var saveFilePath = Path.Combine(Server.MapPath(targetFolder), docFileName);
                // Path file to download from server
                var serverFilePath = serverFolder + "/" + docFileName;

                File.Copy(Server.MapPath("../.." + contractorAttachFile.FilePath), saveFilePath, true);

                var attachFile = new DQREDocumentAttachFile()
                {
                    ID = Guid.NewGuid(),
                    ProjectDocumentId = projectDoc.ID,
                    FileName = docFileName,
                    Extension = contractorAttachFile.Extension,
                    FilePath = serverFilePath,
                    ExtensionIcon = contractorAttachFile.ExtensionIcon,
                    FileSize = contractorAttachFile.FileSize,
                    TypeId = 1,
                    TypeName = "Document file",
                    CreatedBy = UserSession.Current.User.Id,
                    CreatedByName = UserSession.Current.User.UserNameWithFullName,
                    CreatedDate = DateTime.Now
                };

                this.documentAttachFileService.Insert(attachFile);
            }
        }


        private void UpdateTransOutToDocument(DQRETransmittal transObj)
        {
            var attachDocToTrans = this.attachDocToTransmittalService.GetAllByTransId(transObj.ID);
            foreach (var docobj in attachDocToTrans)
            {
                var projectDoc = this.documentProjectService.GetById(docobj.DocumentId.GetValueOrDefault());
                projectDoc.OutgoingTransId = transObj.ID;
                projectDoc.OutgoingTransNo = transObj.TransmittalNo;
                projectDoc.IsCreateOutgoingTrans = true;
                this.documentProjectService.Update(projectDoc);
            }
        }
        private void CollectMasterDocData(ContractorTransmittal contractorTransObj, ContractorTransmittalDocFile contractorDoc, DQREDocumentMaster masterDoc)
        {
            masterDoc.SystemDocumentNo = contractorDoc.DocumentNo;

            masterDoc.Title = contractorDoc.DocumentTitle;
            masterDoc.OriginatorId = contractorTransObj.FromId;
            masterDoc.OriginatorName = contractorTransObj.FromName;
            masterDoc.OriginatingOrganizationId = contractorTransObj.FromId;
            masterDoc.OriginatingOrganizationName = contractorTransObj.FromName;

            masterDoc.ReceivingOrganizationId = contractorTransObj.ToId;
            masterDoc.ReceivingOrganizationName = contractorTransObj.ToName;
            
            masterDoc.DocumentTypeId = contractorDoc.DocumentTypeId;
            masterDoc.DocumentTypeName = contractorDoc.DocumentTypeName;

            masterDoc.DisciplineId = contractorDoc.DisciplineCodeId;
            masterDoc.DisciplineName = contractorDoc.DisciplineCodeName;

            masterDoc.MaterialCodeId = contractorDoc.MaterialCodeId;
            masterDoc.MaterialCodeName = contractorDoc.MaterialCodeName;

            masterDoc.WorkCodeId = contractorDoc.WorkCodeId;
            masterDoc.WorkCodeName = contractorDoc.WorkCodeName;

            masterDoc.DrawingCodeId = contractorDoc.DrawingCodeId;
            masterDoc.DrawingCodeName = contractorDoc.DrawingCodeName;
            masterDoc.EquipmentTagName = contractorDoc.EquipmentTagNo;
            masterDoc.DepartmentCode = contractorDoc.DepartmentCode;
            masterDoc.MRSequenceNo = contractorDoc.Sequence;
            masterDoc.DocumentSequenceNo = contractorDoc.SequenceOfFile;
            masterDoc.SheetNo = string.Empty;

            masterDoc.AreaId = contractorDoc.AreaId;
            masterDoc.AreaName = contractorDoc.AreaName;

            var areaObj = this.areaService.GetById(contractorDoc.AreaId.GetValueOrDefault());
            if (areaObj != null)
            {
                masterDoc.PlantId = areaObj.PlantId;
                masterDoc.PlantName = areaObj.PlantCode;
            }

            masterDoc.UnitId = contractorDoc.UnitCodeId;
            masterDoc.UnitName = contractorDoc.UnitCodeName;
        }

        private void CollectProjectDocData(ContractorTransmittalDocFile contractorDoc, DQREDocument projectDoc)
        {
            projectDoc.ID = Guid.NewGuid();
            projectDoc.DocumentNo = contractorDoc.DocumentNoFull;
            projectDoc.DocumentTitle = contractorDoc.DocumentTitle;
            projectDoc.ContractorDocNo = contractorDoc.ContractorRefNo;
            projectDoc.Revision = contractorDoc.Revision;
            projectDoc.IsssuedDate = contractorDoc.IssueDate;
            projectDoc.DocumentClassId = contractorDoc.DocumentClassId;
            projectDoc.DocumentClassName = contractorDoc.DocumentClassName;
            projectDoc.DocumentCodeId = contractorDoc.DocumentCodeId;
            projectDoc.DocumentCodeName = contractorDoc.DocumentCodeName;
            projectDoc.Remark = string.Empty;
            projectDoc.ConfidentialityId = 0;
            projectDoc.ConfidentialityName = string.Empty;
        }

        private void CollectMasterInfo(DQREDocumentMaster masterObj, DQREDocument prjectDoc)
        {
            prjectDoc.M_SystemDocumentNo = masterObj.SystemDocumentNo;
            prjectDoc.DocumentMasterId = masterObj.ID;
            prjectDoc.M_EquipmentTagName = masterObj.EquipmentTagName;
            prjectDoc.M_DepartmentCode = masterObj.DepartmentCode;
            prjectDoc.M_MRSequenceNo = masterObj.MRSequenceNo;
            prjectDoc.M_DocumentSequenceNo = masterObj.DocumentSequenceNo;
            prjectDoc.M_SheetNo = masterObj.SheetNo;
            prjectDoc.M_OriginatorId = masterObj.OriginatorId;
            prjectDoc.M_OriginatorName = masterObj.OriginatorName;
            prjectDoc.M_OriginatingOrganizationId = masterObj.OriginatingOrganizationId;
            prjectDoc.M_OriginatingOrganizationName = masterObj.OriginatingOrganizationName;
            prjectDoc.M_ReceivingOrganizationId = masterObj.ReceivingOrganizationId;
            prjectDoc.M_ReceivingOrganizationName = masterObj.ReceivingOrganizationName;
            prjectDoc.M_DocumentTypeId = masterObj.DocumentTypeId;
            prjectDoc.M_DocumentTypeName = masterObj.DocumentTypeName;
            prjectDoc.M_DisciplineId = masterObj.DisciplineId;
            prjectDoc.M_DisciplineName = masterObj.DisciplineName;
            prjectDoc.M_MaterialCodeId = masterObj.MaterialCodeId;
            prjectDoc.M_MaterialCodeName = masterObj.MaterialCodeName;
            prjectDoc.M_WorkCodeId = masterObj.WorkCodeId;
            prjectDoc.M_WorkCodeName = masterObj.WorkCodeName;
            prjectDoc.M_DrawingCodeId = masterObj.DrawingCodeId;
            prjectDoc.M_DrawingCodeName = masterObj.DrawingCodeName;
            prjectDoc.M_UnitId = masterObj.UnitId;
            prjectDoc.M_UnitName = masterObj.UnitName;
            prjectDoc.M_AreaId = masterObj.AreaId;
            prjectDoc.M_AreaName = masterObj.AreaName;
            prjectDoc.M_PlantId = masterObj.PlantId;
            prjectDoc.M_PlantName = masterObj.PlantName;
        }

        private void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
        }

        private void ExportContractorETRM(ContractorTransmittal transObj)
        {
            var attachDocFileFilter = new List<ContractorTransmittalDocFile>();

            var attachDocFullList = this.contractorTransmittalDocFileService.GetAllByTrans(transObj.ID);

            // Remove duplicate Document
            foreach (var document in attachDocFullList)
            {
                if (attachDocFileFilter.All(t => t.DocumentNo != document.DocumentNo))
                {
                    attachDocFileFilter.Add(document);
                }
            }
            // --------------------------------------------------------------------------------------------

            var filePath = Server.MapPath("../../Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\DQRE_Contractor Transmittal Template.xlsm");
            var workSheets = workbook.Worksheets;
            var transSheet = workSheets[8];
            var fileListSheet = workSheets[9];
            // Export trans Info
            var dtFull = new DataTable();
            dtFull.Columns.AddRange(new[]
            {
                new DataColumn("NoIndex", typeof(String)),
                new DataColumn("DocNo", typeof(String)),
                new DataColumn("1Empty", typeof(String)),
                new DataColumn("Rev", typeof(String)),
                new DataColumn("IssueDate", typeof(DateTime)),
                new DataColumn("DocTitle", typeof(String)),
                new DataColumn("2Empty", typeof(String)),
                new DataColumn("DocClass", typeof(String)),
                new DataColumn("DocCode", typeof(String)),
                new DataColumn("3Empty", typeof(String)),
                new DataColumn("ContractorRefNo", typeof(String)),
                new DataColumn("DocumentTypeGroup", typeof(String)),
                new DataColumn("ProjectCode", typeof(String)),
                new DataColumn("AreaCode", typeof(String)),
                new DataColumn("UnitCode", typeof(String)),
                new DataColumn("DocumentType", typeof(String)),
                new DataColumn("Discipline", typeof(String)),
                new DataColumn("MaterialCode", typeof(String)),
                new DataColumn("WorkCode", typeof(String)),
                new DataColumn("DrawingCode", typeof(String)),
                new DataColumn("DepartmentCode", typeof(String)),
                new DataColumn("EquipmentTagNumber", typeof(String)),
                new DataColumn("SequenceNo", typeof(String)),
                new DataColumn("SerialNo", typeof(String)),
            });

            var count = 1;
            foreach (var doc in attachDocFileFilter)
            {
                var dataRow = dtFull.NewRow();
                dataRow["NoIndex"] = count;
                dataRow["DocNo"] = doc.DocumentNoFull;
                dataRow["IssueDate"] = doc.IssueDate ?? transObj.TransDate;
                dataRow["DocTitle"] = doc.DocumentTitle;
                dataRow["Rev"] = doc.Revision;
                dataRow["DocClass"] = doc.DocumentClassName;
                dataRow["DocCode"] = doc.DocumentCodeName;
                dataRow["ContractorRefNo"] = doc.ContractorRefNo;
                dataRow["DocumentTypeGroup"] = doc.DocumentTypeGroupName;
                dataRow["ProjectCode"] = doc.ProjectName;
                dataRow["AreaCode"] = doc.AreaName;
                dataRow["UnitCode"] = doc.UnitCodeName;
                dataRow["DocumentType"] = doc.DocumentTypeName;
                dataRow["Discipline"] = doc.DisciplineCodeName;
                dataRow["MaterialCode"] = doc.MaterialCodeName;
                dataRow["WorkCode"] = doc.WorkCodeName;
                dataRow["DrawingCode"] = doc.DrawingCodeName;
                dataRow["DepartmentCode"] = doc.DepartmentCode;
                dataRow["EquipmentTagNumber"] = doc.EquipmentTagNo;
                dataRow["SequenceNo"] = doc.Sequence;
                dataRow["SerialNo"] = doc.SerialNo;


                dtFull.Rows.Add(dataRow);
                count += 1;
            }

            transSheet.Cells.ImportDataTable(dtFull, false, 13, 1, dtFull.Rows.Count, dtFull.Columns.Count, true);

            for (int i = 0; i < attachDocFileFilter.Count; i++)
            {
                transSheet.Cells.Merge(14 + i, 2, 1, 2);
                transSheet.Cells.Merge(14 + i, 6, 1, 2);
            }

            transSheet.Cells.DeleteRow(13 + attachDocFileFilter.Count);

            transSheet.Cells["G4"].PutValue(transObj.TransNo);
            transSheet.Cells["E5"].PutValue(transObj.TransDate.GetValueOrDefault());
            if (transObj.DueDate != null)
            {
                transSheet.Cells["G5"].PutValue(transObj.DueDate.Value);
            }
            transSheet.Cells["E6"].PutValue(transObj.ToName);
            transSheet.Cells["E9"].PutValue(transObj.FromName);
            // ---------------------------------------------------------------------

            // Export File list info
            var dtFileList = new DataTable();
            dtFileList.Columns.AddRange(new[]
            {
                        new DataColumn("DocNo", typeof(String)),
                        new DataColumn("FileName", typeof(String)),
                    });

            foreach (var doc in attachDocFullList)
            {
                var dataRow = dtFileList.NewRow();
                dataRow["DocNo"] = doc.DocumentNoFull;
                dataRow["FileName"] = doc.FileName;

                dtFileList.Rows.Add(dataRow);
            }

            fileListSheet.Cells.ImportDataTable(dtFileList, false, 3, 1, dtFileList.Rows.Count,
                dtFileList.Columns.Count, true);
            fileListSheet.Cells.DeleteRow(3 + attachDocFullList.Count);


            // ---------------------------------------------------------------------
            var savePath = Server.MapPath("../.." + transObj.StoreFolderPath) + "\\eTRM File\\";
            var fileName = transObj.TransNo + "_eTRM_" +
                           transObj.TransDate.GetValueOrDefault().ToString("dd-MM-yyyy") + ".xlsm";
            workbook.Save(savePath + fileName);

            this.Download_File(savePath + fileName);
        }

        private void ExportETRM(DQRETransmittal transObj)
        {
            var attachDocToTrans = this.attachDocToTransmittalService.GetAllByTransId(transObj.ID);
            if (attachDocToTrans != null)
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Exports") + @"\";
                //var filePath = Server.MapPath("Exports") + @"\";
                var workbook = new Workbook();
                workbook.Open(filePath + @"Template\TransTemplate.xlsx");

                var dataSheet = workbook.Worksheets[0];

                var dtFull = new DataTable();

                dtFull.Columns.AddRange(new[]
                {
                        new DataColumn("DocumentNo", typeof (String)),
                        new DataColumn("Revision", typeof (String)),
                        new DataColumn("IsssuedDate", typeof (String)),
                        new DataColumn("DocumentTitle", typeof (String)),
                        new DataColumn("DocumentClassName", typeof (String)),
                        new DataColumn("DocumentCodeName", typeof (String))
                    });

                foreach (var docobj in attachDocToTrans)
                {
                    var dataRow = dtFull.NewRow();
                    var documentObj = this.documentService.GetById(docobj.DocumentId.GetValueOrDefault());
                    dataRow["DocumentNo"] = documentObj.DocumentNo;
                    dataRow["Revision"] = documentObj.Revision;
                    dataRow["IsssuedDate"] = Convert.ToDateTime(documentObj.IsssuedDate).ToString("dd-MMM-yy");
                    dataRow["DocumentTitle"] = documentObj.DocumentTitle;
                    dataRow["DocumentClassName"] = documentObj.DocumentClassName;
                    dataRow["DocumentCodeName"] = documentObj.DocumentCodeName;
                    dtFull.Rows.Add(dataRow);
                }
                var filename = transObj.TransmittalNo + "_Trans_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                dataSheet.Cells["G3"].PutValue(transObj.TransmittalNo);
                dataSheet.Cells["E4"].PutValue(DateTime.Now.ToString("dd-MMM-yy"));

                var fromObj = this.organizationCodeService.GetById(transObj.OriginatingOrganizationId.GetValueOrDefault());
                var toObj = this.organizationCodeService.GetById(transObj.ReceivingOrganizationId.GetValueOrDefault());
                dataSheet.Cells["E5"].PutValue(toObj.Description);
                dataSheet.Cells["I5"].PutValue(toObj.Phone);
                dataSheet.Cells["I6"].PutValue(toObj.Fax);
                dataSheet.Cells["E8"].PutValue(fromObj.Description);
                dataSheet.Cells["I8"].PutValue(fromObj.Phone);
                dataSheet.Cells["I9"].PutValue(fromObj.Fax);

                dataSheet.Cells["C30"].PutValue(fromObj.Description);

                int firstrow = 12;
                for (int i = 0; i < dtFull.Rows.Count; i++)
                {
                    firstrow++;
                    dataSheet.Cells["C" + firstrow].PutValue(dtFull.Rows[i]["DocumentNo"]);
                    dataSheet.Cells["E" + firstrow].PutValue(dtFull.Rows[i]["Revision"]);
                    dataSheet.Cells["F" + firstrow].PutValue(dtFull.Rows[i]["IsssuedDate"]);
                    dataSheet.Cells["G" + firstrow].PutValue(dtFull.Rows[i]["DocumentTitle"]);
                    dataSheet.Cells["J" + firstrow].PutValue(dtFull.Rows[i]["DocumentClassName"]);
                    dataSheet.Cells["K" + firstrow].PutValue(dtFull.Rows[i]["DocumentCodeName"]);
                }
                workbook.Save(filePath + filename);
                this.Download_File(filePath + filename);
            }
        }

        protected void ddlProjectOutgoing_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var ddlProjectOutgoing = (RadComboBox)this.radMenuOutgoing.Items[2].FindControl("ddlProjectOutgoing");
            int projectId = Convert.ToInt32(ddlProjectOutgoing.SelectedValue);
            this.lblProjectOutgoingId.Value = projectId.ToString();
            this.grdOutgoingTrans.Rebind();
        }

        protected void ddlProjectIncoming_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var ddlProjectIncoming = (RadComboBox)this.radMenuIncoming.Items[1].FindControl("ddlProjectIncoming");
            int projectId = Convert.ToInt32(ddlProjectIncoming.SelectedValue);
            this.lblProjectIncomingId.Value = projectId.ToString();
            this.grdIncomingTrans.Rebind();
        }

        protected void ddlProjectOutgoing_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.ImageUrl = @"~/Images/project.png";
        }

        protected void ddlProjectIncoming_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.ImageUrl = @"~/Images/project.png";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        protected void grdOutgoingTrans_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var ddlProjectOutgoing = (RadComboBox)this.radMenuOutgoing.Items[2].FindControl("ddlProjectOutgoing");
            var ddlStatusOutgoing = (DropDownList)this.radMenuOutgoing.Items[2].FindControl("ddlStatusOutgoing");
            var txtSearchOutgoing = (TextBox)this.radMenuOutgoing.Items[2].FindControl("txtSearchOutgoing");
            var outgoingTransList = new List<DQRETransmittal>();

            if (ddlProjectOutgoing != null && ddlProjectOutgoing.SelectedItem != null)
            {
                var projectId = Convert.ToInt32(ddlProjectOutgoing.SelectedValue);

                outgoingTransList = this.dqreTransmittalService.GetAllByProject(projectId, 2, txtSearchOutgoing.Text).OrderByDescending(t => t.TransmittalNo).ToList();

                if (ddlStatusOutgoing != null)
                {
                    switch (ddlStatusOutgoing.SelectedValue)
                    {
                        case "Invalid":
                            outgoingTransList = outgoingTransList.Where(t => !string.IsNullOrEmpty(t.Status)).ToList();
                            break;
                        case "Waiting":
                            outgoingTransList = outgoingTransList.Where(t => !string.IsNullOrEmpty(t.Status) && !t.IsSend.GetValueOrDefault()).ToList();
                            break;
                        case "Sent":
                            outgoingTransList = outgoingTransList.Where(t => t.IsSend.GetValueOrDefault()).ToList();
                            break;
                    }
                }

                
            }

            this.grdOutgoingTrans.DataSource = outgoingTransList.OrderByDescending(t=>t.CreatedDate.GetValueOrDefault());
        }

        protected void grdIncomingTrans_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var ddlProjectIncoming = (RadComboBox)this.radMenuIncoming.Items[1].FindControl("ddlProjectIncoming");
            var ddlStatusIncoming = (DropDownList)this.radMenuIncoming.Items[1].FindControl("ddlStatusIncoming");
            var txtSearchIncoming = (TextBox)this.radMenuIncoming.Items[1].FindControl("txtSearchIncoming");
            var incomingTransList = new List<DQRETransmittal>();
            if (ddlProjectIncoming != null && ddlProjectIncoming.SelectedItem != null)
            {
                var projectId = Convert.ToInt32(ddlProjectIncoming.SelectedValue);

                incomingTransList = this.dqreTransmittalService.GetAllByProject(projectId, 1, txtSearchIncoming.Text).OrderByDescending(t => t.TransmittalNo).ToList();

                if (ddlStatusIncoming != null)
                {
                    switch (ddlStatusIncoming.SelectedValue)
                    {
                        case "WaitingImport":
                            incomingTransList = incomingTransList.Where(t => !t.IsImport.GetValueOrDefault()).ToList();
                            break;
                        case "Imported":
                            incomingTransList = incomingTransList.Where(t => t.IsImport.GetValueOrDefault()).ToList();
                            break;
                    }
                }
            }

            this.grdIncomingTrans.DataSource = incomingTransList.OrderByDescending(t=> t.CreatedDate.GetValueOrDefault());
        }

        protected void ddlStatusOutgoing_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.grdOutgoingTrans.Rebind();
        }

        protected void btnSearchOutgoing_Click(object sender, EventArgs e)
        {
            this.grdOutgoingTrans.Rebind();
        }

        protected void ddlStatusIncoming_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.grdIncomingTrans.Rebind();
        }

        protected void btnSearchIncoming_Click(object sender, EventArgs e)
        {
            this.grdIncomingTrans.Rebind();
        }

        protected void grdOutgoingTrans_OnDeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var objId = new Guid(item.GetDataKeyValue("ID").ToString());
            var transObj = this.dqreTransmittalService.GetById(objId);
            if (!string.IsNullOrEmpty(transObj?.StoreFolderPath))
            {
                var folderPath = Server.MapPath("../.." + transObj.StoreFolderPath);
                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath);
                }
            }

            this.dqreTransmittalService.Delete(objId);
            this.grdOutgoingTrans.Rebind();
        }
        private void NotifiNewTransmittal(DQRETransmittal transmittal)
        {
            try
            {
                if (transmittal != null)
                {

                    var userListid = this.userService.GetAllByRoleId(this.roleService.GetByContractor(transmittal.ReceivingOrganizationId.GetValueOrDefault()).Id);

                    var smtpClient = new SmtpClient
                    {
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                        EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                        Host = ConfigurationManager.AppSettings["Host"],
                        Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                        Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                    };
                    int count = 0;
                    var containtable = string.Empty;

                    var subject = "New Transmittal (#Trans#) has been sended from (" + transmittal.OriginatingOrganizationName + ")";

                    var message = new MailMessage();
                    message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "EDMS System");
                    message.Subject = subject.Replace("#Trans#", transmittal.TransmittalNo);
                    message.BodyEncoding = new UTF8Encoding();
                    message.IsBodyHtml = true;

                    var bodyContent = @"<div style=‘text-align: center;’> 
                                    <span class=‘Apple-tab-span’>Dear All,&nbsp;</span><br />
                                   
                                    <p style=‘text-align: center;’><strong><span style=‘font-size: 18px;’>Please be informed that the following document of transmittal (#Trans#)</span></strong></p><br/><br/>

                                       <table border='1' cellspacing='0'>
                                       <tr>
                                       <th style='text-align:center; width:40px'>No.</th>
                                       <th style='text-align:center; width:330px'>Document Number</th>
                                       <th style='text-align:center; width:60px'>Revision</th>
                                       <th style='text-align:center; width:330px'>Document Title</th>
                                       <th style='text-align:center; width:330px'>Project</th>
                                       <th style='text-align:center; width:330px'>Issue Date</th>
                                       <th style='text-align:center; width:330px'>Code</th>
                                       <th style='text-align:center; width:330px'>Trans In</th>
                                       
                                       </tr>";
                    var listDocument = new List<DQREDocument>();

                    var attachDocList = this.attachDocToTransmittalService.GetAllByTransId(transmittal.ID);
                    foreach (var item in attachDocList)
                    {
                        var docObj = this.documentProjectService.GetById(item.DocumentId.GetValueOrDefault());
                        if (docObj != null)
                        {
                            listDocument.Add(docObj);
                        }
                    }
                    
                    var deadline = string.Empty;
                    deadline = transmittal.IssuedDate != null ? transmittal.IssuedDate.Value.ToString("dd/MM/yyyy") : "";

                    foreach (var document in listDocument)
                    {

                        count += 1;

                        bodyContent += @"<tr>
                               <td>" + count + @"</td>
                               <td>" + document.DocumentNo + @"</td>
                               <td>"
                                       + document.Revision + @"</td>
                               <td>"
                                       + document.DocumentTitle + @"</td>
                               <td>"
                                       + document.ProjectCodeName + @"</td>
                               <td>"
                                       + deadline + @"</td>
                               <td>"
                                       + document.DocumentCodeName + @"</td>
                               <td>"
                                       + document.IncomingTransNo + @"</td>";

                    }
                    var st = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/Document/ContractorTransmittalList.aspx";
                    bodyContent += @" </table>
                                       <br/>
                                       <span><br />
                                    &nbsp;This link to access&nbsp;:&nbsp; <a href='" + st + "'>" + st + "</a>" +
                                 @" <br/> &nbsp;&nbsp;&nbsp; EDMS TRANSMITTAL NOTIFICATION </br>
                        [THIS IS SYSTEM GENERATED NOTIFICATION PLEASE DO NOT REPLY]</span>";
                    message.Body = bodyContent.Replace("#Trans#", transmittal.TransmittalNo); ;

                    var Userlist = userListid.Where(t => !string.IsNullOrEmpty(t.Email)).Distinct().ToList();
                    foreach (var user in Userlist)
                    {
                        try
                        {
                            message.To.Add(new MailAddress(user.Email));
                        }
                        catch { }

                    }
                    smtpClient.Send(message);
                }
            }
            catch { }
        }

        private void NotifiNewTransmittalOut(DQRETransmittal transmittal)
        {
            try
            {
                if (transmittal != null)
                {
                    var smtpClient = new SmtpClient
                    {
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                        EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                        Host = ConfigurationManager.AppSettings["Host"],
                        Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                        Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                    };
                    int count = 0;
                    var containtable = string.Empty;

                    var subject = "New Transmittal (#Trans#) has been sent to contractor (" + transmittal.ReceivingOrganizationName+ ")";

                    var message = new MailMessage();
                    message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "EDMS System");
                    message.Subject = subject.Replace("#Trans#", transmittal.TransmittalNo);
                    message.BodyEncoding = new UTF8Encoding();
                    message.IsBodyHtml = true;

                    var bodyContent = @"<div style=‘text-align: center;’> 
                                    <span class=‘Apple-tab-span’>Dear All,&nbsp;</span><br />
                                   
                                    <p style=‘text-align: center;’><strong><span style=‘font-size: 18px;’>Please be informed the document of transmittal (#Trans#)</span></strong></p><br/><br/>

                                       <table border='1' cellspacing='0'>
                                       <tr>
                                       <th style='text-align:center; width:40px'>No.</th>
                                       <th style='text-align:center; width:330px'>Document Number</th>
                                       <th style='text-align:center; width:60px'>Revision</th>
                                       <th style='text-align:center; width:330px'>Document Title</th>
                                        <th style='text-align:center; width:330px'>Project</th>
                                        <th style='text-align:center; width:330px'>Code</th>
                                       </tr>";
                    var listDocument = new List<DQREDocument>();

                    var attachDocList = this.attachDocToTransmittalService.GetAllByTransId(transmittal.ID);
                    foreach (var item in attachDocList)
                    {
                        var docObj = this.documentProjectService.GetById(item.DocumentId.GetValueOrDefault());
                        if (docObj != null)
                        {
                            listDocument.Add(docObj);
                        }
                    }

                    var deadline = string.Empty;
                    deadline = transmittal.DueDate != null ? transmittal.DueDate.Value.ToString("dd/MM/yyyy") : "";

                    foreach (var document in listDocument)
                    {

                        count += 1;

                        bodyContent += @"<tr>
                               <td>" + count + @"</td>
                               <td>" + document.DocumentNo + @"</td>
                               <td>"
                                       + document.Revision + @"</td>
                               <td>"
                                       + document.DocumentTitle + @"</td>
                               <td>"
                                       + document.ProjectCodeName + @"</td>
                               <td>"
                                       + document.DocumentCodeName + @"</td>";

                    }
                    var st = ConfigurationManager.AppSettings["WebAddress"] + @"/AdvanceSearch.aspx?TransOut="+transmittal.TransmittalNo;
                    bodyContent += @" </table>
                                       <br/>
                                       <span><br />
                                    &nbsp;This link to access&nbsp;:&nbsp; <a href='" + st + "'>" + st + "</a>" +
                                 @" <br/> &nbsp; EDMS TRANSMITTAL NOTIFICATION </br>
                        [THIS IS SYSTEM GENERATED NOTIFICATION PLEASE DO NOT REPLY]</span>";
                    message.Body = bodyContent.Replace("#Trans#", transmittal.TransmittalNo); ;


                   List<Guid> ListattachDocList = this.attachDocToTransmittalService.GetAllByTransId(transmittal.ID).Select(t=> (Guid)t.DocumentId).ToList();
                    var ListAllUserInWf = this.objectAssignedUser.GetAllListByDoc(ListattachDocList).Select(t=>(int) t.UserID).Distinct().ToList();
                    var Userlist = this.userService.GetListUser(ListAllUserInWf);
                    foreach (var user in Userlist)
                    {
                        try
                        {
                            message.To.Add(new MailAddress(user.Email));
                        }
                        catch { }

                    }
                    smtpClient.Send(message);
                }
            }
            catch { }
        }
    }
}