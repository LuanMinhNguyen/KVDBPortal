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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Aspose.Cells;

using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class NCRSINewList : Page
    {
        private readonly ProjectCodeService projectCodeService = new ProjectCodeService();

        private readonly ContractorTransmittalService contractorTransmittalService = new ContractorTransmittalService();

        private readonly ContractorTransmittalDocFileService contractorTransmittalDocFileService = new ContractorTransmittalDocFileService();

        private readonly PECC2TransmittalService PECC2TransmittalService = new PECC2TransmittalService();

        private readonly OrganizationCodeService organizationCodeService = new OrganizationCodeService();

        private readonly AttachDocToTransmittalService attachDocToTransmittalService = new AttachDocToTransmittalService();

        private readonly PECC2DocumentsService documentProjectService = new PECC2DocumentsService();

        private readonly PECC2DocumentAttachFileService documentAttachFileService = new PECC2DocumentAttachFileService();

        private readonly RevisionStatuService revisionStatusService = new RevisionStatuService();

        private readonly PurposeCodeService purposeCodeService = new PurposeCodeService();

        private readonly UserService userService = new UserService();

        private readonly RoleService roleService = new RoleService();

        private readonly AreaService areaService = new AreaService();

        private readonly ObjectAssignedUserService objectAssignedUser = new  ObjectAssignedUserService();

        private readonly DocumentTypeService documentTypeService = new DocumentTypeService();

        private readonly ChangeRequestService changeRequestService = new ChangeRequestService();

        private readonly ChangeRequestAttachFileService changeRequestAttachFileService = new ChangeRequestAttachFileService();

        private readonly NCR_SIService ncrSiService = new NCR_SIService();

        private readonly NCR_SIAddPictureService ncrsiAddPictureService = new NCR_SIAddPictureService();
        private readonly NCR_SIAttachFileService nsrsiAttachFile = new NCR_SIAttachFileService();

        private readonly ObjectAssignedWorkflowService objAssignedWfService = new ObjectAssignedWorkflowService();
        private readonly ObjectAssignedUserService objAssignedUserService = new ObjectAssignedUserService();
        private readonly GroupCodeService groupCodeService= new GroupCodeService();


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
            Session.Add("SelectedMainMenu", "Project Execution");

           
            if (!Page.IsPostBack)
            {
                this.LoadComboData();
                // Set init control only show for PECC2 internal group
                this.grdNCRSI.MasterTableView.GetColumn("DeleteWorkflow").Visible = ((UserSession.Current.User.IsAdmin.GetValueOrDefault() || UserSession.Current.User.IsDC.GetValueOrDefault() )&& UserSession.Current.User.Role.IsInternal.GetValueOrDefault());
                var isPECC2InternalUser = UserSession.Current.User.Role.IsInternal.GetValueOrDefault();
                this.radMenuOutgoing.Items[0].Visible = isPECC2InternalUser;
                this.radMenuOutgoing.Items[1].Visible = isPECC2InternalUser;
                this.grdNCRSI.MasterTableView.GetColumn("EditColumn").Visible = isPECC2InternalUser;// &&(UserSession.Current.User.IsAdmin.GetValueOrDefault() || UserSession.Current.User.IsDC.GetValueOrDefault());
                this.grdNCRSI.MasterTableView.GetColumn("DeleteColumn").Visible = isPECC2InternalUser;// && (UserSession.Current.User.IsAdmin.GetValueOrDefault() || UserSession.Current.User.IsDC.GetValueOrDefault());
                this.grdNCRSI.MasterTableView.GetColumn("ExportExcelForm").Visible = isPECC2InternalUser;
                this.grdNCRSI.MasterTableView.GetColumn("AttachWorkflow").Visible = isPECC2InternalUser;
                this.grdNCRSI.MasterTableView.GetColumn("WorkflowProcessHistory").Visible = isPECC2InternalUser;
                if (!string.IsNullOrEmpty(this.Request.QueryString["NCRSINo"]))
                {
                    var txtSearch = (TextBox)this.radMenuOutgoing.Items[2].FindControl("txtSearch");
                    txtSearch.Text = this.Request.QueryString["NCRSINo"];
                }
                // --------------------------------------------------------------------------------------------------
                this.Title = ConfigurationManager.AppSettings.Get("AppName");
                var temp = (RadPane)this.Master.FindControl("leftPane");
                temp.Collapsed = true;
            }
        }

        private void LoadComboData()
        {
            var ddlProject = (RadComboBox)this.radMenuOutgoing.Items[2].FindControl("ddlProject");
            var projectList = this.projectCodeService.GetAll().OrderBy(t => t.Code);

            if (ddlProject != null)
            {
                ddlProject.DataSource = projectList;
                ddlProject.DataTextField = "FullName";
                ddlProject.DataValueField = "ID";
                ddlProject.DataBind();

                int projectId = Convert.ToInt32(ddlProject.SelectedValue);
                this.lblProjectId.Value = ddlProject.SelectedValue;
                Session.Add("SelectedProject", projectId);
            }

            //var ddlStatus = (DropDownList)this.radMenuOutgoing.Items[2].FindControl("ddlStatus");
            //if(ddlStatus != null)
            //{
            //    // ddlStatus.SelectedIndex = 1;
            //}

            //if (UserSession.Current.User.Role.IsInternal.GetValueOrDefault())
            //{
            //    this.radMenuOutgoing.Items[0].Visible = false;
            //    this.radMenuOutgoing.Items[1].Visible = false;
            //    this.grdNCRSI.MasterTableView.GetColumn("CreateResponseChangeRequest").Visible = false;
            //    this.grdNCRSI.MasterTableView.GetColumn("AttachDocFile").Visible = false;
            //    this.grdNCRSI.MasterTableView.GetColumn("ETRMAtach").Visible = false;
            //    this.grdNCRSI.MasterTableView.GetColumn("DeleteColumn").Visible = false;
            //    this.grdNCRSI.MasterTableView.GetColumn("CreateOutgoingTrans").Visible = false;
            //    this.grdNCRSI.MasterTableView.GetColumn("AttachWorkflow").Visible = true;
            //    this.grdNCRSI.MasterTableView.GetColumn("WorkflowProcessHistory").Visible = true;
            //    this.grdNCRSI.MasterTableView.GetColumn("PECC2ReviewResultName").Visible = true;
            //    this.grdNCRSI.MasterTableView.GetColumn("ReviewResultName").Visible = false;
            //    this.grdNCRSI.MasterTableView.GetColumn("AllAttachFile").Visible = true;
            //}
            //else
            //{
            //    this.radMenuOutgoing.Items[0].Visible = false;
            //    this.radMenuOutgoing.Items[1].Visible = false;
            //    this.grdNCRSI.MasterTableView.GetColumn("CreateResponseChangeRequest").Visible = true;
            //    this.grdNCRSI.MasterTableView.GetColumn("AttachDocFile").Visible = true;
            //    this.grdNCRSI.MasterTableView.GetColumn("ETRMAtach").Visible = true;
            //    this.grdNCRSI.MasterTableView.GetColumn("DeleteColumn").Visible = true;
            //    this.grdNCRSI.MasterTableView.GetColumn("CreateOutgoingTrans").Visible = true;
            //    this.grdNCRSI.MasterTableView.GetColumn("AttachWorkflow").Visible = false;
            //    this.grdNCRSI.MasterTableView.GetColumn("WorkflowProcessHistory").Visible = false;
            //    this.grdNCRSI.MasterTableView.GetColumn("PECC2ReviewResultName").Visible = false;
            //    this.grdNCRSI.MasterTableView.GetColumn("ReviewResultName").Visible = true;
            //    this.grdNCRSI.MasterTableView.GetColumn("AllAttachFile").Visible = false;

            //}
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
            }
            else if (e.Argument.Contains("DeleteWorkflow_"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var Obj = this.ncrSiService.GetById(objId);
                if (Obj != null)
                {
                    //delete assing user
                    var listassign = this.objAssignedWfService.GetAllByObj(Obj.ID);
                    foreach (var item in listassign)
                    {
                        this.objAssignedWfService.Delete(item);
                    }
                    var listUserAssign = this.objAssignedUserService.GetAllListObjID(Obj.ID);
                    foreach (var item in listUserAssign)
                    {
                        this.objAssignedUserService.Delete(item);
                    }
                    Obj.IsAttachWorkflow = false;
                    Obj.IsInWFProcess = false;
                    Obj.IsWFComplete = false;
                    this.ncrSiService.Update(Obj);
                }
                this.grdNCRSI.Rebind();
            }
            else if (e.Argument.Contains("DeleteNRCIS"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var obj = this.ncrSiService.GetById(objId);
                if (!string.IsNullOrEmpty(obj?.StoreFolderPath))
                {
                    var folderPath = Server.MapPath("../.." + obj.StoreFolderPath);
                    if (Directory.Exists(folderPath))
                    {
                        Directory.Delete(folderPath,true);
                    }
                }

                this.ncrSiService.Delete(objId);
                this.grdNCRSI.Rebind();
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
            else if (e.Argument.Contains("ExportNCRSIForm"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var obj = this.ncrSiService.GetById(objId);
                if (obj != null)
                {
                    this.ExportNCRIS(obj);
                }
            }
            else if (e.Argument.Contains("SendTrans"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var changeRequestObj = this.changeRequestService.GetById(objId);
                if (changeRequestObj != null)
                {
                    changeRequestObj.IsSend = true;
                    changeRequestObj.SendDate = DateTime.Now;
                    changeRequestObj.SendById = UserSession.Current.User.Id;
                    changeRequestObj.SendByName = UserSession.Current.User.FullName;
                    this.changeRequestService.Update(changeRequestObj);
                    ////send email to dc contractors
                    //if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]))
                    //{
                    //    this.NotifiNewTransmittal(transObj);
                    //}
                    ////send email to all user 
                    //if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]))
                    //{
                    //    this.NotifiNewTransmittalOut(transObj);
                    //}
                    this.grdNCRSI.Rebind();
                }
            }
            else if (e.Argument.Contains("Undo"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var transObj = this.PECC2TransmittalService.GetById(objId);
                if (transObj != null)
                {
                    var contractorTrans = this.contractorTransmittalService.GetById(transObj.ContractorTransId.GetValueOrDefault());
                    if (contractorTrans != null) {
                        this.contractorTransmittalService.Delete(contractorTrans);
                        transObj.ContractorTransId = null;
                        transObj.IsSend = false;
                        transObj.ReceivedDate = null;
                        this.PECC2TransmittalService.Update(transObj);
                        this.grdNCRSI.Rebind();
                    }
                   
                }
            }
        }

        private void ImportTransDocument(ContractorTransmittal contractorTransObj, Guid objPECC2Id)
        {
            var pecc2IncomingTrans = this.PECC2TransmittalService.GetById(contractorTransObj.PECC2TransId.GetValueOrDefault());
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
            if (pecc2IncomingTrans.ForSentId == 1)
            {
                foreach (var contractorDoc in filterDocList)
                {
                    this.ProcessImportProjectDocument(fullDocList, contractorDoc, pecc2IncomingTrans, revisionStatus, objPECC2Id);
                }
            }
            else
            {
                foreach (var contractorDoc in filterDocList)
                {
                    this.ProcessImportChangeRequest(fullDocList, contractorDoc, pecc2IncomingTrans, objPECC2Id);
                }
            }
        }

        private void ProcessImportChangeRequest(List<ContractorTransmittalDocFile> fullDocList, ContractorTransmittalDocFile contractorDoc, PECC2Transmittal pecc2IncomingTrans, Guid objPECC2Id)
        {
            // Get attach doc list
            var contractorDocAttach = fullDocList.Where(t => t.DocumentNo == contractorDoc.DocumentNo).ToList();
            // ----------------------------------------------------------------------------------------

            var currentChangeRequest = this.changeRequestService.GetAllByChangeRequestNo(contractorDoc.DocumentNo);
            if (currentChangeRequest != null)
            {
                // Fill incoming trans info
                currentChangeRequest.IncomingTransId = pecc2IncomingTrans.ID;
                currentChangeRequest.IncomingTransNo = pecc2IncomingTrans.TransmittalNo;
                currentChangeRequest.IsHasAttachFile = true;
                this.changeRequestService.Update(currentChangeRequest);
                //-------------------------------------------------------------------------------

                if (objPECC2Id != null && !string.IsNullOrEmpty(objPECC2Id.ToString()))
                {
                    var attachDoc = new AttachDocToTransmittal()
                    {
                        TransmittalId = objPECC2Id,
                        DocumentId = currentChangeRequest.ID
                    };

                    if (!this.attachDocToTransmittalService.IsExist(objPECC2Id, currentChangeRequest.ID))
                    {
                        this.attachDocToTransmittalService.Insert(attachDoc);
                    }
                }

                //Attach doc file to change request obj
                this.AttachDocFileToChangeRequest(contractorDocAttach, currentChangeRequest);
                // ------------------------------------------------------------------------------

                // Update PECC2 Incoming trans info
                pecc2IncomingTrans.Status = string.Empty;
                pecc2IncomingTrans.ErrorMessage = string.Empty;
                pecc2IncomingTrans.IsImport = true;

                this.PECC2TransmittalService.Update(pecc2IncomingTrans);
                // -------------------------------------------------------------------------------
                contractorDoc.PECC2ProjectDocId = currentChangeRequest.ID;
                contractorDoc.IsReject = false;
                this.contractorTransmittalDocFileService.Update(contractorDoc);
            }
            else
            {
                // Collect new project doc info
                var changeRequestObj = new ChangeRequest();
                this.CollectChangeRequestData(contractorDoc, changeRequestObj);

                changeRequestObj.ProjectId = contractorDoc.ProjectId;
                changeRequestObj.ProjectCode = contractorDoc.ProjectName;

                changeRequestObj.CreatedBy = UserSession.Current.User.Id;
                changeRequestObj.CreatedDate = DateTime.Now;
                changeRequestObj.IsHasAttachFile = true;
                changeRequestObj.IsDelete = false;

                // Fill incoming trans info
                changeRequestObj.IncomingTransId = pecc2IncomingTrans.ID;
                changeRequestObj.IncomingTransNo = pecc2IncomingTrans.TransmittalNo;
                //-------------------------------------------------------------------------------

                // get master info
                ////this.CollectMasterInfo(masterDoc, projectDoc);
                // -----------------------------------------------------------------------------------------------------

                this.changeRequestService.Insert(changeRequestObj);
                if (objPECC2Id != null && !string.IsNullOrEmpty(objPECC2Id.ToString()))
                {
                    var attachDoc = new AttachDocToTransmittal()
                    {
                        TransmittalId = objPECC2Id,
                        DocumentId = changeRequestObj.ID
                    };

                    if (!this.attachDocToTransmittalService.IsExist(objPECC2Id, changeRequestObj.ID))
                    {
                        this.attachDocToTransmittalService.Insert(attachDoc);
                    }
                }

                //update contractorTransDocFileObj
                //Attach doc file to project doc
                this.AttachDocFileToChangeRequest(contractorDocAttach, changeRequestObj);
                // --------------------------------------------------------------------------------------------------------------

                // Update PECC2 Incoming trans info
                pecc2IncomingTrans.Status = string.Empty;
                pecc2IncomingTrans.ErrorMessage = string.Empty;
                pecc2IncomingTrans.IsImport = true;

                this.PECC2TransmittalService.Update(pecc2IncomingTrans);
                // --------------------------------------------------------------------------------------------------------------

                contractorDoc.PECC2ProjectDocId = changeRequestObj.ID;
                contractorDoc.IsReject = false;
                this.contractorTransmittalDocFileService.Update(contractorDoc);
            }
        }

        private void ProcessImportProjectDocument(List<ContractorTransmittalDocFile> fullDocList, ContractorTransmittalDocFile contractorDoc, PECC2Transmittal pecc2IncomingTrans, RevisionStatu revisionStatus, Guid objPECC2Id)
        {
            // Get attach doc list
            var contractorDocAttach = fullDocList.Where(t => t.DocumentNo == contractorDoc.DocumentNo).ToList();
            // ----------------------------------------------------------------------------------------

            var currentProjectDocList = this.documentProjectService.GetAllByProjectDocNo(contractorDoc.DocumentNo);

            Guid PECC2DocId = contractorDoc.ID;
            var projectDoc = new PECC2Documents();
            // Case: Already have previous document
            if (currentProjectDocList.Count > 0)
            {
                var currentLeafProjectDoc = currentProjectDocList.FirstOrDefault(t => t.IsLeaf.GetValueOrDefault());
                if (currentLeafProjectDoc != null)
                {
                    projectDoc = currentLeafProjectDoc;

                    if (projectDoc.Revision.ToLower().Trim() == contractorDoc.Revision.ToLower().Trim()
                        && !projectDoc.IsHasAttachFile.GetValueOrDefault())
                    {
                        // Fill incoming trans info
                        projectDoc.GroupId = contractorDoc.GroupCodeId;
                        projectDoc.GroupCode = contractorDoc.GroupCodeName;
                        projectDoc.IncomingTransId = pecc2IncomingTrans.ID;
                        projectDoc.IncomingTransNo = pecc2IncomingTrans.TransmittalNo;
                        projectDoc.DocActionId = pecc2IncomingTrans.PurposeId;
                        projectDoc.DocActionCode = pecc2IncomingTrans.PurposeName.Split(',')[0];
                        this.documentProjectService.Update(projectDoc);
                        //-------------------------------------------------------------------------------

                        // -------------------------------------------------------------------------------
                        if (objPECC2Id != null && !string.IsNullOrEmpty(objPECC2Id.ToString()))
                        {
                            var attachDoc = new AttachDocToTransmittal()
                            {
                                TransmittalId = objPECC2Id,
                                DocumentId = projectDoc.ID
                            };
                            if (!this.attachDocToTransmittalService.IsExist(objPECC2Id, projectDoc.ID))
                            {
                                this.attachDocToTransmittalService.Insert(attachDoc);
                            }
                        }
                    }
                    else
                    {
                        // Collect new project doc info
                        projectDoc = new PECC2Documents();
                        this.CollectProjectDocData(contractorDoc, projectDoc);
                        projectDoc.DocNo = currentLeafProjectDoc.DocNo;
                        projectDoc.DocTitle = currentLeafProjectDoc.DocTitle;
                        projectDoc.ProjectId = currentLeafProjectDoc.ProjectId;
                        projectDoc.ProjectName = currentLeafProjectDoc.ProjectName;
                        projectDoc.RevisionSchemaId = currentLeafProjectDoc.RevisionSchemaId;
                        projectDoc.RevisionSchemaName = currentLeafProjectDoc.RevisionSchemaName;
                        projectDoc.ConfidentialityId = currentLeafProjectDoc.ConfidentialityId;
                        projectDoc.ConfidentialityName = currentLeafProjectDoc.ConfidentialityName;
                        if (revisionStatus != null)
                        {
                            projectDoc.RevStatusId = revisionStatus.ID;
                            projectDoc.RevStatusName = revisionStatus.FullName;

                            if (revisionStatus.Code =="FA")
                            {
                                projectDoc.ActualDate = pecc2IncomingTrans.IssuedDate;
                            }
                        }

                        projectDoc.CreatedBy = UserSession.Current.User.Id;
                        projectDoc.CreatedDate = DateTime.Now;
                        projectDoc.IsLeaf = true;
                        projectDoc.IsDelete = false;
                        projectDoc.IsHasAttachFile = true;
                        projectDoc.ParentId = currentLeafProjectDoc.ParentId ?? currentLeafProjectDoc.ID;
                        projectDoc.DocActionId = pecc2IncomingTrans.PurposeId;
                        projectDoc.DocActionCode = pecc2IncomingTrans.PurposeName.Split(',')[0];
                        //projectDoc.IsssuedDate = contractorTransObj.TransDate;
                        // Fill incoming trans info
                        projectDoc.IncomingTransId = pecc2IncomingTrans.ID;
                        projectDoc.IncomingTransNo = pecc2IncomingTrans.TransmittalNo;
                        //-------------------------------------------------------------------------------

                        this.documentProjectService.Insert(projectDoc);
                        PECC2DocId = projectDoc.ID;
                        // -------------------------------------------------------------------------------
                        if (objPECC2Id != null && !string.IsNullOrEmpty(objPECC2Id.ToString()))
                        {
                            var attachDoc = new AttachDocToTransmittal()
                            {
                                TransmittalId = objPECC2Id,
                                DocumentId = projectDoc.ID
                            };
                            if (!this.attachDocToTransmittalService.IsExist(objPECC2Id, projectDoc.ID))
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

                    // Update PECC2 Incoming trans info
                    pecc2IncomingTrans.Status = string.Empty;
                    pecc2IncomingTrans.ErrorMessage = string.Empty;
                    pecc2IncomingTrans.IsImport = true;

                    this.PECC2TransmittalService.Update(pecc2IncomingTrans);
                    // --------------------------------------------------------------------------------------------------------------
                }
            }
            // -----------------------------------------------------------------------------------------------------

            // Case: Document sent by contractor is new doc
            else
            {
                // Collect new project doc info
                projectDoc = new PECC2Documents();
                this.CollectProjectDocData(contractorDoc, projectDoc);

                projectDoc.ProjectId = contractorDoc.ProjectId;
                projectDoc.ProjectName = contractorDoc.ProjectName;
                projectDoc.RevisionSchemaId = 0;
                projectDoc.RevisionSchemaName = string.Empty;
                if (revisionStatus != null)
                {
                    projectDoc.RevStatusId = revisionStatus.ID;
                    projectDoc.RevStatusName = revisionStatus.FullName;
                }

                projectDoc.CreatedBy = UserSession.Current.User.Id;
                projectDoc.CreatedDate = DateTime.Now;
                projectDoc.IsHasAttachFile = true;
                projectDoc.IsLeaf = true;
                projectDoc.IsDelete = false;

                // Fill incoming trans info
                projectDoc.IncomingTransId = pecc2IncomingTrans.ID;
                projectDoc.IncomingTransNo = pecc2IncomingTrans.TransmittalNo;
                projectDoc.DocActionId = pecc2IncomingTrans.PurposeId;
                projectDoc.DocActionCode = pecc2IncomingTrans.PurposeName.Split(',')[0];
                //-------------------------------------------------------------------------------

                // get master info
                ////this.CollectMasterInfo(masterDoc, projectDoc);
                // -----------------------------------------------------------------------------------------------------

                this.documentProjectService.Insert(projectDoc);

                if (objPECC2Id != null && !string.IsNullOrEmpty(objPECC2Id.ToString()))
                {
                    var attachDoc = new AttachDocToTransmittal()
                    {
                        TransmittalId = objPECC2Id,
                        DocumentId = projectDoc.ID
                    };
                    if (!this.attachDocToTransmittalService.IsExist(objPECC2Id, projectDoc.ID))
                    {
                        this.attachDocToTransmittalService.Insert(attachDoc);
                    }
                }
                //update contractorTransDocFileObj
                //Attach doc file to project doc
                this.AttachDocFileToProjectDoc(contractorDocAttach, projectDoc);
                // --------------------------------------------------------------------------------------------------------------

                // Update PECC2 Incoming trans info
                pecc2IncomingTrans.Status = string.Empty;
                pecc2IncomingTrans.ErrorMessage = string.Empty;
                pecc2IncomingTrans.IsImport = true;

                this.PECC2TransmittalService.Update(pecc2IncomingTrans);
                // --------------------------------------------------------------------------------------------------------------
            }
            // -----------------------------------------------------------------------------------------------------

            contractorDoc.PECC2ProjectDocId = projectDoc.ID;
            contractorDoc.IsReject = false;
            this.contractorTransmittalDocFileService.Update(contractorDoc);
        }

        private void AttachDocFileToProjectDoc(List<ContractorTransmittalDocFile> attachList, PECC2Documents projectDoc)
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

                var attachFile = new PECC2DocumentAttachFile()
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

                projectDoc.IsHasAttachFile = true;
                this.documentProjectService.Update(projectDoc);
                this.documentAttachFileService.Insert(attachFile);
            }
        }

        private void AttachDocFileToChangeRequest(List<ContractorTransmittalDocFile> attachList, ChangeRequest changeRequestObj)
        {
            var targetFolder = "../../DocumentLibrary/ChangeRequest";
            var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                + "/DocumentLibrary/ChangeRequest";
            foreach (var contractorAttachFile in attachList)
            {
                var docFileName = contractorAttachFile.FileName;

                // Path file to save on server disc
                var saveFilePath = Path.Combine(Server.MapPath(targetFolder), docFileName);
                // Path file to download from server
                var serverFilePath = serverFolder + "/" + docFileName;

                File.Copy(Server.MapPath("../.." + contractorAttachFile.FilePath), saveFilePath, true);

                var attachFile = new ChangeRequestAttachFile()
                {
                    ID = Guid.NewGuid(),
                    ChangeRequestId = changeRequestObj.ID,
                    FileName = docFileName,
                    Extension = contractorAttachFile.Extension,
                    FilePath = serverFilePath,
                    ExtensionIcon = contractorAttachFile.ExtensionIcon,
                    FileSize = contractorAttachFile.FileSize,
                    CreatedBy = UserSession.Current.User.Id,
                    CreatedByName = UserSession.Current.User.UserNameWithFullName,
                    CreatedDate = DateTime.Now
                };

                this.changeRequestAttachFileService.Insert(attachFile);
            }
        }

        private void CollectProjectDocData(ContractorTransmittalDocFile contractorDoc, PECC2Documents obj)
        {
            obj.ID = Guid.NewGuid();
            obj.DocNo = contractorDoc.DocumentNo;
            obj.DocTitle = contractorDoc.DocumentTitle;
            obj.OriginalDocumentNumber = contractorDoc.ContractorRefNo;
            obj.Revision = contractorDoc.Revision;
            obj.Date = contractorDoc.IssueDate;
            obj.Remarks = string.Empty;
            obj.ConfidentialityId = 0;
            obj.ConfidentialityName = string.Empty;
            obj.Revision = contractorDoc.Revision;
            obj.GroupId = contractorDoc.GroupCodeId;
            obj.GroupCode = contractorDoc.GroupCodeName;
            obj.AreaId = contractorDoc.AreaId;
            obj.AreaCode = contractorDoc.AreaName;
            obj.UnitId = contractorDoc.UnitCodeId;
            obj.UnitCode = contractorDoc.UnitCodeName;
            obj.DocTypeId = contractorDoc.DocumentTypeId;
            obj.DocTypeCode = contractorDoc.DocumentTypeName;
            obj.DisciplineId = contractorDoc.DisciplineCodeId;
            obj.DisciplineCode = contractorDoc.DisciplineCodeName;
            obj.Sequence = Convert.ToInt32(contractorDoc.Sequence);
            obj.SequenceText = contractorDoc.Sequence;
            obj.GroupId = contractorDoc.GroupCodeId;
            obj.GroupCode = contractorDoc.GroupCodeName;
            obj.KKSId = contractorDoc.KKSCodeId;
            obj.KKSCode = contractorDoc.KKSCodeName;
            obj.TrainNo = contractorDoc.TrainNo;
            obj.OriginatingOrganisationId = contractorDoc.OriginatingOrganizationId;
            obj.OriginatingOrganisationName = contractorDoc.OriginatingOrganizationName;
            obj.ReceivingOrganisationId = contractorDoc.ReceivingOrganizationId;
            obj.ReceivingOrganisationName = contractorDoc.ReceivingOrganizationName;
            var docTypeObj = this.documentTypeService.GetById(obj.DocTypeId.GetValueOrDefault());
            if (docTypeObj != null)
            {
                obj.CategoryId = Convert.ToInt32(docTypeObj.CategoryIds);
                obj.CategoryName = docTypeObj.CategoryName;
            }

            obj.Description = string.Empty;
            obj.Treatment = string.Empty;
            obj.RevRemarks = string.Empty;
        }

        private void CollectChangeRequestData(ContractorTransmittalDocFile contractorDoc, ChangeRequest obj)
        {
            obj.ID = Guid.NewGuid();
            obj.Number = contractorDoc.DocumentNo;
            obj.Description = contractorDoc.DocumentTitle;
            obj.ConfidentialityId = 0;
            obj.ConfidentialityName = string.Empty;
            obj.GroupId = contractorDoc.GroupCodeId;
            obj.GroupName = contractorDoc.GroupCodeName;
            obj.AreaId = contractorDoc.AreaId;
            obj.AreaCode = contractorDoc.AreaName;
            obj.UnitId = contractorDoc.UnitCodeId;
            obj.UnitCode = contractorDoc.UnitCodeName;
            obj.Sequence = Convert.ToInt32(contractorDoc.Sequence);
            obj.SequentialNumber = contractorDoc.Sequence;
            obj.Year = Convert.ToInt32(contractorDoc.Year);
            obj.TypeId = contractorDoc.ChangeRequestTypeId;
            obj.TypeName = contractorDoc.ChangeRequestTypeName;
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
            var attachDocFullList = this.contractorTransmittalDocFileService.GetAllByTrans(transObj.ID);
            var attachDocFileFilter = new List<ContractorTransmittalDocFile>();

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
            workbook.Open(filePath + @"Template\PECC2_ContractorTransTemplate.xlsm");
            var workSheets = workbook.Worksheets;
            var transSheet = workSheets[0];
            //var fileListSheet = workSheets[9];
            // Export trans Info
            var dtFull = new DataTable();
            dtFull.Columns.AddRange(new[]
            {
                new DataColumn("DocNo", typeof(String)),
                new DataColumn("1Empty", typeof(String)),
                new DataColumn("Revision", typeof(string)),
                new DataColumn("ActionCode", typeof(String)),
                new DataColumn("DocTitle", typeof(String)),
                new DataColumn("2Empty", typeof(String)),
                new DataColumn("3Empty", typeof(String)),
                new DataColumn("4Empty", typeof(String)),
                new DataColumn("5Empty", typeof(String)),
                new DataColumn("6Empty", typeof(String)),
                new DataColumn("RevRemark", typeof(String)),
                new DataColumn("7Empty", typeof(String)),
            });

            var count = 1;
            foreach (var doc in attachDocFileFilter)
            {
                var dataRow = dtFull.NewRow();
                dataRow["DocNo"] = doc.DocumentNo;
                dataRow["DocTitle"] = doc.DocumentTitle;
                dataRow["Revision"] = doc.Revision;
                dataRow["ActionCode"] = doc.PurposeName;
                dataRow["RevRemark"] = doc.RevRemark;
                dtFull.Rows.Add(dataRow);
                count += 1;
            }

            transSheet.Cells.ImportDataTable(dtFull, false, 8, 0, dtFull.Rows.Count, dtFull.Columns.Count, true);

            for (int i = 0; i < attachDocFileFilter.Count; i++)
            {
                transSheet.Cells.Merge(8 + i, 0, 1, 2);
                transSheet.Cells.Merge(8 + i, 4, 1, 6);
                transSheet.Cells.Merge(8 + i, 10, 1, 2);
            }

            //transSheet.Cells.DeleteRow(19 + attachDocFileFilter.Count);
            var organisationObj =
                this.organizationCodeService.GetById(transObj.OriginatingOrganizationId.GetValueOrDefault());
            var projectObj = this.projectCodeService.GetById(transObj.ProjectId.GetValueOrDefault());
            transSheet.Cells["A2"].PutValue(projectObj.FullName);
            transSheet.Cells["J1"].PutValue(transObj.TransNo);
            transSheet.Cells["H5"].PutValue(projectObj.Code);
            transSheet.Cells["H2"].PutValue(transObj.TransDate.GetValueOrDefault().ToString("yyyy-MM-dd"));
            transSheet.Cells["C3"].PutValue(transObj.OriginatingOrganizationName);
            if (organisationObj != null)
            {
                transSheet.Cells["C4"].PutValue(organisationObj.HeadOffice);
                transSheet.Cells["C5"].PutValue(organisationObj.Phone);
                transSheet.Cells["C6"].PutValue(organisationObj.Fax);
            }

            // ---------------------------------------------------------------------

            var savePath = Server.MapPath("../.." + transObj.StoreFolderPath) + "\\eTRM File\\";
            var fileName = transObj.TransNo + "_eTRM_" +
                           transObj.TransDate.GetValueOrDefault().ToString("dd-MM-yyyy") + ".xlsm";
            workbook.Save(savePath + fileName);

            this.Download_File(savePath + fileName);
        }

        private void ExportNCRIS(NCR_SI obj)
        {

            var flag = false;
            var targetFolder = "DocumentLibrary/NCRSI";
            var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                + "/DocumentLibrary/NCRSI";

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Exports") + @"\";
            //var filePath = Server.MapPath("Exports") + @"\";
            var workbook = new Workbook();
            if (obj.Number.Contains("-SI-"))
            {
                workbook.Open(filePath + @"Template\PECC2_SI_Template.xlsm");
            }
            else
            {
                workbook.Open(filePath + @"Template\PECC2_NRCSI_Template_new.xlsm");
               
            }

            var dataSheet = workbook.Worksheets[0];

            var dtFull = new DataTable();

            var filename = obj.Number + "_"+ DateTime.Now.ToString("ddMMyyyyHHmmss")+".xlsm";
          
            dataSheet.Cells["J10"].PutValue(obj.IssuedDate);
            dataSheet.Cells["C10"].PutValue(obj.Subject);
            dataSheet.Cells["R10"].PutValue(obj.Subject);
            dataSheet.Cells["D11"].PutValue(obj.Reference);
             dataSheet.Cells["B16"].PutValue(obj.Description);
            dataSheet.Cells["P16"].PutValue(obj.Description);
            // export description

            var imagelist = this.ncrsiAddPictureService.GetByNCRSI(obj.ID, 1).OrderBy(t=> t.CreatedDate);
            int count = 0;
            foreach (var image in imagelist)
            {
               
                if (count > 0)
                {
                    dataSheet.Cells.UnhideRow((16 + (count *2)), 15.75);
                    dataSheet.Cells.UnhideRow((17 + (count * 2)), 237);
                   
                    dataSheet.Cells["B" + (17 + (count * 2))].PutValue(image.Description);
                    dataSheet.Cells["P" + (17 + (count * 2))].PutValue(image.Description);
                    dataSheet.Pictures.Add((17 + (count * 2)), 2, Server.MapPath(image.FilePath));
                   
                }
                else
                {
                    dataSheet.Cells["B" + (17 + count)].PutValue(image.Description);
                    dataSheet.Cells["P" + (17 + count)].PutValue(image.Description);
                    dataSheet.Cells.UnhideRow((17 + (count * 2)), 237);
                    dataSheet.Pictures.Add((17 + (count * 2)), 2, Server.MapPath(image.FilePath));
                    
                }
                dataSheet.AutoFitRow((16 + (count * 2)));
                count++;

            }
          
            
            dataSheet.Cells["B22"].PutValue(obj.ActionTake);

           // dataSheet.Cells.HideRows(44, 33);    
            dataSheet.Cells["J9"].PutValue(obj.Number);
            // Fill Signed
            //if (!string.IsNullOrEmpty(UserSession.Current.User.SignImageUrl))
            //{
            //    dataSheet.Pictures.Add(23 + dtFull.Rows.Count, 2, Server.MapPath("../.." + UserSession.Current.User.SignImageUrl));
            //}
            // ---------------------------------------------------------------------

            workbook.Save(filePath + filename);
            //delete ncr/si form
            var listform = this.nsrsiAttachFile.GetByNCRSI(obj.ID).FirstOrDefault(t => t.TypeId == 1);
            if (listform != null)
            {
                File.Delete(Server.MapPath(listform.FilePath));
                this.nsrsiAttachFile.Delete(listform);
            }


            var saveFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, targetFolder) + @"\" + filename;
            File.Copy(filePath + filename, saveFilePath, true);
            var serverFilePath = serverFolder + "/" + filename;

            var attachFile = new NCR_SIAttachFile()
            {
                ID = Guid.NewGuid(),
                NCR_SIId = obj.ID,
                FileName = filename,
                Extension = ".xlsm",
                FilePath = serverFilePath,
                ExtensionIcon = "~/images/excelfile.png",
                FileSize = (double)(new FileInfo(filePath + filename).Length) / 1024,
                TypeId = 1,
                TypeName = "NCR/SI Form",
                CreatedBy = UserSession.Current.User.Id,
                CreatedByName = UserSession.Current.User.UserNameWithFullName,
                CreatedDate = DateTime.Now
            };

            this.nsrsiAttachFile.Insert(attachFile);

            this.Download_File(filePath + filename);
        }
      
        protected void ddlProject_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var ddlProject = (RadComboBox)this.radMenuOutgoing.Items[2].FindControl("ddlProject");
            int projectId = Convert.ToInt32(ddlProject.SelectedValue);
            this.lblProjectId.Value = projectId.ToString();
            this.grdNCRSI.Rebind();
        }

        protected void ddlProject_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
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

        protected void grdNCRSI_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var ddlProject = (RadComboBox)this.radMenuOutgoing.Items[2].FindControl("ddlProject");
            var ddlStatus = (DropDownList)this.radMenuOutgoing.Items[2].FindControl("ddlStatus");
            var txtSearch = (System.Web.UI.WebControls.TextBox)this.radMenuOutgoing.Items[2].FindControl("txtSearch");
           // var divStatus = (HtmlControl)this.radMenuOutgoing.Items[2].FindControl("divStatus");
            var ncrSIList = new List<NCR_SI>();
            var ncrSIListObject = new List<NCR_SI>();

            if (ddlProject?.SelectedItem != null)
            {
                var projectId = Convert.ToInt32(ddlProject.SelectedValue);
                ncrSIList = this.ncrSiService.GetAllNCRSIByProject(projectId, txtSearch.Text).OrderByDescending(t => t.Number).ToList();
            }

          var listObject  = UserSession.Current.User.Role.IsInternal.GetValueOrDefault()
                                    ? ncrSIList.OrderByDescending(t => t.IssuedDate.GetValueOrDefault()).ToList()
                                    : ncrSIList.Where(t => t.Status.Contains("Closing") || t.Status.Contains("Closed")).OrderByDescending(t => t.IssuedDate.GetValueOrDefault()).ToList();
            switch (ddlStatus.SelectedValue)
            {
                case "All":
                    ncrSIListObject = listObject.ToList();
                    break;
                case "NCR":
                    ncrSIListObject = listObject.Where(t=> t.Type==1 && t.CreatedBy==UserSession.Current.User.Id).ToList();
                    break;
                case "SI":
                    ncrSIListObject = listObject.Where(t => t.Type== 2 && t.CreatedBy == UserSession.Current.User.Id).ToList();
                    break;
            }
            this.grdNCRSI.DataSource = UserSession.Current.User.IsAdmin.GetValueOrDefault() ? ncrSIListObject : ncrSIListObject.Where(t => !t.IsCancel.GetValueOrDefault()).ToList();
        }

        protected void ddlStatus_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.grdNCRSI.Rebind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.grdNCRSI.Rebind();
        }

        protected void grdNCRSI_OnDeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var objId = new Guid(item.GetDataKeyValue("ID").ToString());
            //var changeRequestObj = this.changeRequestService.GetById(objId);

            this.changeRequestService.Delete(objId);
            this.grdNCRSI.Rebind();
        }
        private void NotifiNewTransmittal(PECC2Transmittal transmittal)
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
                    var listDocument = new List<PECC2Documents>();

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
                               <td>" + document.DocNo + @"</td>
                               <td>"
                                       + document.Revision + @"</td>
                               <td>"
                                       + document.DocTitle + @"</td>
                               <td>"
                                       + document.ProjectName + @"</td>
                               <td>"
                                       + deadline + @"</td>
                               <td>"
                                       + string.Empty + @"</td>
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

        private void NotifiNewTransmittalOut(PECC2Transmittal transmittal)
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
                    var listDocument = new List<PECC2Documents>();

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
                               <td>" + document.DocNo + @"</td>
                               <td>"
                                       + document.Revision + @"</td>
                               <td>"
                                       + document.DocTitle + @"</td>
                               <td>"
                                       + document.ProjectName + @"</td>
                               <td>"
                                       + string.Empty + @"</td>";

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


        protected void grdNCRSI_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridFilteringItem)
            {
                var filterItem = (GridFilteringItem)e.Item;

                var ddlGroupfilter = (RadComboBox)filterItem.FindControl("RadComboBoxTitle");
                var groupList = this.groupCodeService.GetAll().OrderBy(t => t.Code).ToList();
               // ddlGroupfilter.SelectedValue = e.Item.OwnerTableView.GetColumn("GroupN").CurrentFilterValue;
               foreach(var item in groupList)
                {
                    var comboitem = new RadComboBoxItem(item.Code, item.Code);
                    ddlGroupfilter.Items.Add(comboitem);
                }
                //RadComboBoxStatus
                var ddlStatusfilter = (RadComboBox)filterItem.FindControl("RadComboBoxStatus");
                var raditem = new RadComboBoxItem("Opening   ", "Opening   ");
                ddlStatusfilter.Items.Add(raditem);
                raditem = new RadComboBoxItem("Unclose   ", "Unclose   ");
                ddlStatusfilter.Items.Add(raditem);
                raditem = new RadComboBoxItem("Closing   ", "Closing   ");
                ddlStatusfilter.Items.Add(raditem);
                raditem = new RadComboBoxItem("Closed    ", "Closed    ");
                ddlStatusfilter.Items.Add(raditem);
               // ddlStatusfilter.SelectedValue = e.Item.OwnerTableView.GetColumn("Status").CurrentFilterValue;

            }
        }
    }
}