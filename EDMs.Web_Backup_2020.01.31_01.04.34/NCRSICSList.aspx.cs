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
    public partial class NCRSICSList : Page
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
            Session.Add("SelectedMainMenu", "Project Execution");

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

            var ddlStatus = (DropDownList)this.radMenuOutgoing.Items[2].FindControl("ddlStatus");
            if(ddlStatus != null)
            {
                // ddlStatus.SelectedIndex = 1;
            }

            if (UserSession.Current.User.Role.IsInternal.GetValueOrDefault())
            {
                this.radMenuOutgoing.Items[0].Visible = false;
                this.radMenuOutgoing.Items[1].Visible = false;
                this.grdChangeRequest.MasterTableView.GetColumn("CreateResponseChangeRequest").Visible = false;
                this.grdChangeRequest.MasterTableView.GetColumn("AttachDocFile").Visible = false;
                this.grdChangeRequest.MasterTableView.GetColumn("ETRMAtach").Visible = false;
                this.grdChangeRequest.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                this.grdChangeRequest.MasterTableView.GetColumn("SendChange").Visible = false;
                this.grdChangeRequest.MasterTableView.GetColumn("AttachWorkflow").Visible = true;
                this.grdChangeRequest.MasterTableView.GetColumn("WorkflowProcessHistory").Visible = true;
                this.grdChangeRequest.MasterTableView.GetColumn("PECC2ReviewResultName").Visible = true;
                this.grdChangeRequest.MasterTableView.GetColumn("ReviewResultName").Visible = false;
                this.grdChangeRequest.MasterTableView.GetColumn("AllAttachFile").Visible = true;
            }
            else
            {
                this.radMenuOutgoing.Items[0].Visible = true;
                this.radMenuOutgoing.Items[1].Visible = true;
                this.grdChangeRequest.MasterTableView.GetColumn("CreateResponseChangeRequest").Visible = true;
                this.grdChangeRequest.MasterTableView.GetColumn("AttachDocFile").Visible = true;
                this.grdChangeRequest.MasterTableView.GetColumn("ETRMAtach").Visible = true;
                this.grdChangeRequest.MasterTableView.GetColumn("DeleteColumn").Visible = true;
                this.grdChangeRequest.MasterTableView.GetColumn("SendChange").Visible = true;
                this.grdChangeRequest.MasterTableView.GetColumn("AttachWorkflow").Visible = false;
                this.grdChangeRequest.MasterTableView.GetColumn("WorkflowProcessHistory").Visible = false;
                this.grdChangeRequest.MasterTableView.GetColumn("PECC2ReviewResultName").Visible = false;
                this.grdChangeRequest.MasterTableView.GetColumn("ReviewResultName").Visible = true;
                this.grdChangeRequest.MasterTableView.GetColumn("AllAttachFile").Visible = false;

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
            }
            else if (e.Argument.Contains("DeleteTrans_"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var transObj = this.PECC2TransmittalService.GetById(objId);
                if (!string.IsNullOrEmpty(transObj?.StoreFolderPath))
                {
                    var folderPath = Server.MapPath("../.." + transObj.StoreFolderPath);
                    if (Directory.Exists(folderPath))
                    {
                        Directory.Delete(folderPath,true);
                    }
                }

                this.PECC2TransmittalService.Delete(objId);
                this.grdChangeRequest.Rebind();
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
            else if (e.Argument.Contains("ExportPECC2ETRM"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var transObj = this.PECC2TransmittalService.GetById(objId);
                if (transObj != null)
                {
                    this.ExportETRM(transObj, e.Argument.Split('_')[2]);
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
                    this.grdChangeRequest.Rebind();
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
                        this.grdChangeRequest.Rebind();
                    }
                   
                }
            }
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

        private void ExportETRM(PECC2Transmittal transObj, string forSend)
        {
            var attachDocToTrans = this.attachDocToTransmittalService.GetAllByTransId(transObj.ID);
            if (attachDocToTrans != null)
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Exports") + @"\";
                //var filePath = Server.MapPath("Exports") + @"\";
                var workbook = new Workbook();
                workbook.Open(filePath + @"Template\PECC2TransmittalTemplate.xlsm");

                var dataSheet = workbook.Worksheets[0];

                var dtFull = new DataTable();

                dtFull.Columns.AddRange(new[]
                {
                    new DataColumn("DocumentNo", typeof(String)),
                    new DataColumn("Empty1", typeof(String)),
                    new DataColumn("Empty2", typeof(String)),
                    new DataColumn("Revision", typeof(String)),
                    new DataColumn("DocumentTitle", typeof(String)),
                    new DataColumn("Empty3", typeof(String)),
                    new DataColumn("Empty4", typeof(String)),
                    new DataColumn("Empty5", typeof(String)),
                    new DataColumn("Empty6", typeof(String)),
                    new DataColumn("Empty7", typeof(String)),
                    new DataColumn("Empty8", typeof(String)),
                });

                foreach (var docobj in attachDocToTrans)
                {
                    var dataRow = dtFull.NewRow();
                    switch (forSend)
                    {
                        case "1":
                            var documentObj = this.documentProjectService.GetById(docobj.DocumentId.GetValueOrDefault());
                            dataRow["DocumentNo"] = documentObj.DocNo;
                            dataRow["Revision"] = documentObj.Revision;
                            dataRow["DocumentTitle"] = documentObj.DocTitle;
                            break;
                        case "2":
                            var changeRequestObj =
                                this.changeRequestService.GetById(docobj.DocumentId.GetValueOrDefault());
                            dataRow["DocumentNo"] = changeRequestObj.Number;
                            dataRow["Revision"] = string.Empty;
                            dataRow["DocumentTitle"] = changeRequestObj.Description;
                            break;
                    }
                    
                    dtFull.Rows.Add(dataRow);
                }

                var projectObj = this.projectCodeService.GetById(transObj.ProjectCodeId.GetValueOrDefault());

                var filename = transObj.TransmittalNo + "_eTRM.xlsm";
                dataSheet.Cells["J5"].PutValue(transObj.TransmittalNo);
                dataSheet.Cells["J6"].PutValue(DateTime.Now.ToString("dd-MMM-yy"));
                dataSheet.Cells["B5"].PutValue(projectObj.FullName);
                dataSheet.Cells["B7"].PutValue(transObj.Description);
                dataSheet.Cells["B8"].PutValue(transObj.FromValue);
                dataSheet.Cells["B10"].PutValue(transObj.ToValue);
                dataSheet.Cells["B12"].PutValue(transObj.CCValue);

                dataSheet.Cells.ImportDataTable(dtFull, false, 20, 0, dtFull.Rows.Count, dtFull.Columns.Count, true);

                for (int i = 0; i < dtFull.Rows.Count; i++)
                {
                    dataSheet.Cells.Merge(20 + i, 0, 1, 3);
                    dataSheet.Cells.Merge(20 + i, 4, 1, 7);
                }

                // Fill Signed
                if (!string.IsNullOrEmpty(UserSession.Current.User.SignImageUrl))
                {
                    dataSheet.Pictures.Add(23 + dtFull.Rows.Count, 2, Server.MapPath("../.." + UserSession.Current.User.SignImageUrl));
                }
                // ---------------------------------------------------------------------

                workbook.Save(filePath + filename);
                this.Download_File(filePath + filename);
            }
        }

        protected void ddlProject_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var ddlProject = (RadComboBox)this.radMenuOutgoing.Items[2].FindControl("ddlProject");
            int projectId = Convert.ToInt32(ddlProject.SelectedValue);
            this.lblProjectId.Value = projectId.ToString();
            this.grdChangeRequest.Rebind();
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

        protected void grdChangeRequest_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var ddlProject = (RadComboBox)this.radMenuOutgoing.Items[2].FindControl("ddlProject");
            var ddlStatus = (DropDownList)this.radMenuOutgoing.Items[2].FindControl("ddlStatus");
            var txtSearch = (TextBox)this.radMenuOutgoing.Items[2].FindControl("txtSearch");
            var divStatus = (HtmlControl)this.radMenuOutgoing.Items[2].FindControl("divStatus");
            var changeRequestList = new List<ChangeRequest>();

            if (UserSession.Current.User.Role.IsInternal.GetValueOrDefault())
            {
                ddlStatus.SelectedValue = "Sent";
                divStatus.Visible = false;
            }
            else
            {
                divStatus.Visible = true;
            }

            if (ddlProject?.SelectedItem != null)
            {
                var projectId = Convert.ToInt32(ddlProject.SelectedValue);
                changeRequestList = this.changeRequestService.GetAllByProject(projectId, txtSearch.Text).OrderByDescending(t => t.Number).ToList();

                if (ddlStatus != null)
                {
                    switch (ddlStatus.SelectedValue)
                    {
                        case "Invalid":
                            changeRequestList = changeRequestList.Where(t => !t.IsValid.GetValueOrDefault()).ToList();
                            break;
                        case "Waiting":
                            changeRequestList = changeRequestList.Where(t => t.IsValid.GetValueOrDefault() && !t.IsSend.GetValueOrDefault()).ToList();
                            break;
                        case "Sent":
                            changeRequestList = changeRequestList.Where(t => t.IsSend.GetValueOrDefault()).ToList();
                            break;
                    }
                }
            }

            // Filter owner of change request
            if (!UserSession.Current.User.Role.IsInternal.GetValueOrDefault())
            {
                changeRequestList = changeRequestList.Where(t => t.FromOrganizationId == UserSession.Current.User.Role.ContractorId).OrderByDescending(t => t.IssuedDate.GetValueOrDefault()).ToList();
            }
            // -----------------------------------------------------------------------------------------------------------------

            this.grdChangeRequest.DataSource = changeRequestList.OrderByDescending(t=>t.IssuedDate.GetValueOrDefault());
        }

        protected void ddlStatus_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.grdChangeRequest.Rebind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.grdChangeRequest.Rebind();
        }

        protected void grdChangeRequest_OnDeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var objId = new Guid(item.GetDataKeyValue("ID").ToString());
            //var changeRequestObj = this.changeRequestService.GetById(objId);

            this.changeRequestService.Delete(objId);
            this.grdChangeRequest.Rebind();
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

        protected void grdChangeRequest_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var item = e.Item as GridDataItem;
                if (item["ReviewResultName"].Text == "Disapproved")
                {
                    item["ReviewResultName"].BackColor = Color.Red;
                    item["ReviewResultName"].BorderColor = Color.Red;
                }
            }
        }
    }
}