// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using EDMs.Business.Services.Scope;
using EDMs.Web.Utilities;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.IO;
    using Aspose.Cells;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ImportProjectDocList : Page
    {
        private readonly PECC2DocumentsService pecc2documentService = new PECC2DocumentsService();
        private readonly ProjectCodeService projectcodeService = new ProjectCodeService();

        private readonly PlantService plantService = new PlantService();

        private readonly AreaService areaService = new AreaService();

        private readonly UnitService unitService = new UnitService();
        private readonly DocumentCodeServices docCodeServices = new DocumentCodeServices();
        private readonly RevisionStatuService revisionStatusService = new RevisionStatuService();
        private readonly ConfidentialityService confidentialityService = new ConfidentialityService();
        private readonly DocumentTypeService documentTypeService = new DocumentTypeService();
        private readonly RevisionSchemaService revisionSchemaService = new RevisionSchemaService();
        private readonly NotificationRuleService notificationRuleService = new NotificationRuleService();

        private readonly UserService userService = new UserService();

        private readonly PECC2DocumentAttachFileService attachFileService = new PECC2DocumentAttachFileService();

        private readonly AttachFilesPackageService attachFilesPackageService = new AttachFilesPackageService();

        private readonly DisciplineService disciplineService = new DisciplineService();

        private readonly RoleService roleService = new RoleService();

        private readonly ProjectCodeService _projectCodeService = new ProjectCodeService();

        private readonly OrganizationCodeService _organizationcodeService = new OrganizationCodeService();

        private readonly SystemCodeService systemCodeService = new SystemCodeService();
        
        private readonly KKSIdentificationCodeService kksService = new KKSIdentificationCodeService();
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
            var currentSheetName = string.Empty;
            var currentDocumentNo = string.Empty;
            var docList = new List<PECC2Documents>();
            var listExistFile = new List<string>();
            try
            {
                foreach (UploadedFile docFile in this.docuploader.UploadedFiles)
                {
                    var extension = docFile.GetExtension();
                    if (extension == ".xls" || extension == ".xlsx" || extension == ".xlsm")
                    {
                        var importPath = Server.MapPath("../../Import") + "/" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "_" + docFile.FileName;
                        docFile.SaveAs(importPath);

                        var workbook = new Workbook();
                        workbook.Open(importPath);

                        for (var i = 2; i < workbook.Worksheets.Count; i++)
                        {
                            var datasheet = workbook.Worksheets[i];

                            currentSheetName = datasheet.Name;
                            var dataTable = datasheet.Cells.ExportDataTable(2, 1,
                            datasheet.Cells.MaxRow, 27);
                            foreach (DataRow dataRow in dataTable.Rows)
                            {
                                if (!string.IsNullOrEmpty(dataRow["Column1"].ToString()))
                                {
                                    currentDocumentNo = dataRow["Column1"].ToString().Trim();

                                    if (!this.pecc2documentService.IsExist(currentDocumentNo))
                                    {
                                        var docObj = new PECC2Documents();
                                        docObj.ID = Guid.NewGuid();
                                        docObj.CategoryId = 1;
                                        docObj.CategoryName = "1. Engineering Document";
                                        docObj.DocNo = dataRow["Column1"].ToString().Trim();
                                        docObj.DocTitle = dataRow["Column2"].ToString().Trim();

                                        // Confidential Info
                                        var confidentialCode = dataRow["Column3"].ToString().Trim();
                                        var confidentialObj = this.confidentialityService.GetByCode(confidentialCode);
                                        docObj.ConfidentialityId = 0;
                                        docObj.ConfidentialityName = string.Empty;
                                        if (confidentialObj != null)
                                        {
                                            docObj.ConfidentialityId = confidentialObj.ID;
                                            docObj.ConfidentialityName = confidentialObj.Code;
                                        }
                                        // --------------------------------------------------------

                                        // Area - Unit info
                                        var unitCode = dataRow["Column5"].ToString().Trim();
                                        var unitObj = this.unitService.GetByCode(unitCode);
                                        docObj.AreaId = 0;
                                        docObj.AreaCode = string.Empty;
                                        docObj.UnitId = 0;
                                        docObj.UnitCode = string.Empty;
                                        if (unitObj != null)
                                        {
                                            docObj.UnitId = unitObj.ID;
                                            docObj.UnitCode = unitObj.Code;

                                            docObj.AreaId = unitObj.AreaId;
                                            docObj.AreaCode = unitObj.AreaCode;
                                        }
                                        // --------------------------------------------------------

                                        // System - Sub-system Info
                                        var subsystemCode = dataRow["Column6"].ToString().Trim();
                                        var subsystemObj = this.systemCodeService.GetByCode(subsystemCode);
                                        docObj.SystemId = 0;
                                        docObj.SystemCode = string.Empty;
                                        docObj.SubsystemId = 0;
                                        docObj.SubsystemCode = string.Empty;
                                        if (subsystemObj != null)
                                        {
                                            docObj.SubsystemId = subsystemObj.ID;
                                            docObj.SubsystemCode = subsystemObj.Code;

                                            docObj.SystemId = subsystemObj.ParentId;
                                            docObj.SystemCode = subsystemObj.ParentCode;
                                        }
                                        // --------------------------------------------------------

                                        // Project Info
                                        var projectCode = dataRow["Column7"].ToString().Trim();
                                        var projectObj = this.projectcodeService.GetByCode(projectCode);
                                        docObj.ProjectId = 0;
                                        docObj.ProjectName = string.Empty;
                                        if (projectObj != null)
                                        {
                                            docObj.ProjectId = projectObj.ID;
                                            docObj.ProjectName = projectObj.Code;
                                        }
                                        // --------------------------------------------------------

                                        // DocType Info
                                        var docTypeCode = dataRow["Column8"].ToString().Trim();
                                        var docTypeObj = this.documentTypeService.GetByCode(docTypeCode);
                                        docObj.DocTypeId = 0;
                                        docObj.DocTypeCode = string.Empty;
                                        if (docTypeObj != null)
                                        {
                                            docObj.DocTypeId = docTypeObj.ID;
                                            docObj.DocTypeCode = docTypeObj.Code;
                                        }
                                        // --------------------------------------------------------

                                        // KKS Info
                                        var kksCode = dataRow["Column9"].ToString().Trim();
                                        var kksObj = this.kksService.GetByCode(kksCode);
                                        docObj.KKSId = 0;
                                        docObj.KKSCode = string.Empty;
                                        if (kksObj != null)
                                        {
                                            docObj.KKSId = kksObj.ID;
                                            docObj.KKSCode = kksObj.Code;
                                        }
                                        // --------------------------------------------------------

                                        docObj.TrainNo = dataRow["Column10"].ToString().Trim();

                                        // Discipline  Info
                                        var disciplineCode = dataRow["Column11"].ToString().Trim();
                                        var dsiciplineObj = this.disciplineService.GetByCode(disciplineCode);
                                        docObj.DisciplineId = 0;
                                        docObj.DisciplineCode = string.Empty;
                                        if (dsiciplineObj != null)
                                        {
                                            docObj.DisciplineId = dsiciplineObj.ID;
                                            docObj.DisciplineCode = dsiciplineObj.Code;
                                        }
                                        // --------------------------------------------------------

                                        docObj.SheetNo = dataRow["Column12"].ToString().Trim();
                                        docObj.Sequence = Convert.ToInt32(dataRow["Column13"].ToString().Trim());
                                        docObj.SequenceText = dataRow["Column13"].ToString().Trim();

                                        // Organisation  Info
                                        var organisationCode = dataRow["Column14"].ToString().Trim();
                                        var organisationObj = this._organizationcodeService.GetByCode(organisationCode);
                                        docObj.OriginatingOrganisationId = 0;
                                        docObj.OriginatingOrganisationName = string.Empty;
                                        if (organisationObj != null)
                                        {
                                            docObj.OriginatingOrganisationId = organisationObj.ID;
                                            docObj.OriginatingOrganisationName = organisationObj.Code;
                                        }
                                        // --------------------------------------------------------

                                        docObj.OriginalDocumentNumber = dataRow["Column15"].ToString().Trim();
                                        docObj.PlannedDate = !string.IsNullOrEmpty(dataRow["Column16"].ToString())
                                            ? (DateTime?)dataRow["Column16"]
                                            : null;

                                        docObj.ManHour = !string.IsNullOrEmpty(dataRow["Column18"].ToString())
                                            ? Convert.ToDouble(dataRow["Column18"])
                                            : 0;
                                        docObj.Remarks = dataRow["Column19"].ToString().Trim();

                                        // rev schema  Info
                                        var revSchemaCode = dataRow["Column20"].ToString().Trim();
                                        var revSchemaObj = this.revisionSchemaService.GetByCode(revSchemaCode);
                                        docObj.RevisionSchemaId = 0;
                                        docObj.RevisionSchemaName = string.Empty;
                                        if (revSchemaObj != null)
                                        {
                                            docObj.RevisionSchemaId = revSchemaObj.ID;
                                            docObj.RevisionSchemaName = revSchemaObj.Code;
                                        }
                                        // --------------------------------------------------------

                                        docObj.Revision = dataRow["Column21"].ToString().Trim();
                                        docObj.MinorRev = dataRow["Column22"].ToString().Trim();

                                        // rev status  Info
                                        var revStatusCode = dataRow["Column23"].ToString().Trim();
                                        var revStatusObj = this.revisionStatusService.GetByCode(revStatusCode);
                                        docObj.RevStatusId = 0;
                                        docObj.RevStatusName = string.Empty;
                                        if (revStatusObj != null)
                                        {
                                            docObj.RevStatusId = revStatusObj.ID;
                                            docObj.RevStatusName = revStatusObj.Code;
                                        }
                                        // --------------------------------------------------------

                                        docObj.RevDate = !string.IsNullOrEmpty(dataRow["Column24"].ToString())
                                            ? (DateTime?)dataRow["Column24"]
                                            : null;
                                        docObj.RevRemarks = dataRow["Column25"].ToString().Trim();

                                        // Doc Action  Info
                                        var docActionCode = dataRow["Column26"].ToString().Trim();
                                        var docActionObj = this.docCodeServices.GetByCode(docActionCode);
                                        docObj.DocActionId = 0;
                                        docObj.DocActionCode = string.Empty;
                                        if (docActionObj != null)
                                        {
                                            docObj.DocActionId = docActionObj.ID;
                                            docObj.DocActionCode = docActionObj.Code;
                                        }
                                        // --------------------------------------------------------

                                        // Doc review  Info
                                        var docReviewCode = dataRow["Column27"].ToString().Trim();
                                        var docreviewObj = this.docCodeServices.GetByCode(docReviewCode);
                                        docObj.DocReviewStatusId = 0;
                                        docObj.DocReviewStatusCode = string.Empty;
                                        if (docreviewObj != null)
                                        {
                                            docObj.DocReviewStatusId = docreviewObj.ID;
                                            docObj.DocReviewStatusCode = docreviewObj.Code;
                                        }
                                        // --------------------------------------------------------

                                        docObj.IsLeaf = true;
                                        docObj.IsDelete = false;
                                        docObj.IsHasAttachFile = false;
                                        docObj.IsCreateOutgoingTrans = false;
                                        docObj.IsCompleteFinal = false;
                                        docObj.IsInWFProcess = false;
                                        docObj.IsWFComplete = false;
                                        docObj.CreatedBy = UserSession.Current.User.Id;
                                        docObj.CreatedByName = UserSession.Current.User.FullName;
                                        docObj.CreatedDate = DateTime.Now;
                                        this.pecc2documentService.Insert(docObj);


                                        //if (!docList.Exists(t => t.DocumentNo == docObj.DocumentNo))
                                        //{
                                        //    //documentPackageList.Add(docObj);
                                        //   var docId= this.pecc2documentService.Insert(docObj);
                                        //    if(docId != null && !string.IsNullOrEmpty(dataRow["Column14"].ToString()))
                                        //    {
                                        //        var targetFolder = "../../DocumentLibrary/ProjectDocs";
                                        //        var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                                        //            + "/DocumentLibrary/ProjectDocs";
                                        //        var fileIcon = new Dictionary<string, string>()
                                        //            {
                                        //                { "doc", "~/images/wordfile.png" },
                                        //                { "docx", "~/images/wordfile.png" },
                                        //                { "dotx", "~/images/wordfile.png" },
                                        //                { "xls", "~/images/excelfile.png" },
                                        //                { "xlsx", "~/images/excelfile.png" },
                                        //                { "pdf", "~/images/pdffile.png" },
                                        //                { "7z", "~/images/7z.png" },
                                        //                { "dwg", "~/images/dwg.png" },
                                        //                { "dxf", "~/images/dxf.png" },
                                        //                { "rar", "~/images/rar.png" },
                                        //                { "zip", "~/images/zip.png" },
                                        //                { "txt", "~/images/txt.png" },
                                        //                { "xml", "~/images/xml.png" },
                                        //                { "xlsm", "~/images/excelfile.png" },
                                        //                { "bmp", "~/images/bmp.png" },
                                        //            };

                                        //        var listfile = dataRow["Column14"].ToString().Trim().Split(';').Where(t=> !string.IsNullOrEmpty(t)).ToList();
                                        //        foreach (var attachFileName in listfile)
                                        //        {

                                        //            var docFileName = attachFileName.Trim();

                                        //            var serverDocFileName = docFileName;

                                        //            // Path file to save on server disc
                                        //            var saveFilePath = Path.Combine(Server.MapPath(targetFolder), serverDocFileName);

                                        //            // Path file to download from server
                                        //            var serverFilePath = serverFolder + "/" + serverDocFileName;
                                        //            var fileExt = docFileName.Substring(docFileName.LastIndexOf(".")+1, docFileName.Length - docFileName.LastIndexOf(".") - 1);
                                        //            if (File.Exists(saveFilePath))
                                        //            {
                                        //                var file = new FileInfo(saveFilePath);

                                        //                var attachFile = new PECC2DocumentAttachFile()
                                        //                {
                                        //                    ID = Guid.NewGuid(),
                                        //                    ProjectDocumentId = docObj.ID,
                                        //                    FileName = docFileName,
                                        //                    Extension = fileExt,
                                        //                    FilePath = serverFilePath,
                                        //                    ExtensionIcon = fileIcon.ContainsKey(fileExt.ToLower()) ? fileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
                                        //                    FileSize = (double)file.Length / 1024,
                                        //                    TypeId = 1,
                                        //                    TypeName = "Document file",
                                        //                    CreatedBy = UserSession.Current.User.Id,
                                        //                    CreatedByName = UserSession.Current.User.UserNameWithFullName,
                                        //                    CreatedDate = DateTime.Now
                                        //                };

                                        //                this.attachFileService.Insert(attachFile);
                                        //            }
                                        //            else
                                        //            {
                                        //                this.lblError.Text +=attachFileName +"; "; 
                                        //            }
                                        //        }
                                        //        docObj.IsHasAttachFile = this.attachFileService.GetAllDocId(docObj.ID).Any();
                                        //        this.pecc2documentService.Update(docObj);
                                        //    }
                                        //}
                                    }
                                    else
                                    {
                                        listExistFile.Add(currentDocumentNo);
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(this.lblError.Text))
                            {
                                var st = this.lblError.Text;
                                this.blockError.Visible = true;
                                this.lblError.Text = "FileName: <br/> " + st + " Don't not exists: <br/> ";
                            }
                        }
                    }

                    if (listExistFile.Count > 0)
                    {
                        this.blockError.Visible = true;
                        this.lblError.Text += "Document already exists: <br/>";
                        foreach (var item in listExistFile)
                        {
                            this.lblError.Text += "<span style='color: blue; font-weight: bold'>'" + item + "'</span> <br/>";
                        }
                    }
                    else
                    {
                        this.blockError.Visible = true;
                        this.lblError.Text =
                            "Data of document master list file is valid. System import successfull!";
                    }

                }
            }
            catch (Exception ex)
            {
                this.blockError.Visible = true;
                this.lblError.Text = "Have error at sheet: '" + currentSheetName + "', document: '" + currentDocumentNo + "', with error: '" + ex.Message + "'";
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
    }
}