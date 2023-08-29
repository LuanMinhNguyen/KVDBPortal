// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Web.UI;
    using Business.Services.Document;
    using Business.Services.Library;
    using Data.Entities;
    using Utilities.Sessions;
    using System.Data;

    using Aspose.Cells;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ChangeRequestAttachDocFile : Page
    {
        private readonly ChangeRequestService changeRequestService;

        private readonly ChangeRequestDocFileService changeRequestDocFileService;

        private readonly PECC2DocumentsService documentService;

        private readonly DocumentTypeService documentTypeService;

        private readonly PECC2ObjectNumberingService documentNumberingService;

        private readonly ProjectCodeService projectService;

        private readonly UnitService unitService;

        private readonly DrawingService drawingCodeService;

        private readonly MaterialService materialCodeService;

        private readonly WorkService workCodeService;

        private readonly DisciplineService disciplineService;

        private readonly DocumentCodeServices documentCodeServices;

        private readonly AreaService areaService;

        private readonly PlantService plantService;

        private readonly OrganizationCodeService organizationCodeService;

        private readonly KKSIdentificationCodeService kksService;

        private readonly GroupCodeService groupCodeService;
            

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ChangeRequestAttachDocFile()
        {
            this.changeRequestService = new ChangeRequestService();
            this.projectService = new ProjectCodeService();
            this.changeRequestDocFileService = new ChangeRequestDocFileService();
            this.documentService = new PECC2DocumentsService();
            this.documentTypeService = new DocumentTypeService();
            this.documentNumberingService = new PECC2ObjectNumberingService();
            this.drawingCodeService = new DrawingService();
            this.unitService = new UnitService();
            this.workCodeService = new WorkService();
            this.disciplineService = new DisciplineService();
            this.documentCodeServices = new DocumentCodeServices();
            this.materialCodeService = new MaterialService();
            this.areaService = new AreaService();
            this.plantService = new PlantService();
            this.organizationCodeService = new OrganizationCodeService();
            this.kksService = new KKSIdentificationCodeService();
            this.groupCodeService = new GroupCodeService();
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
                var changeRequestId = new Guid(this.Request.QueryString["objId"]);
                var changeRequestObj = this.changeRequestService.GetById(changeRequestId);
                if (changeRequestObj != null)
                {
                    this.RadPane2.Collapsed = changeRequestObj.IsSend.GetValueOrDefault();
                    this.RadPane3.Visible = !changeRequestObj.IsSend.GetValueOrDefault();
                    this.grdDocumentFile.MasterTableView.GetColumn("EditColumn").Visible = !changeRequestObj.IsSend.GetValueOrDefault();
                    this.grdDocumentFile.MasterTableView.GetColumn("DeleteColumn").Visible = !changeRequestObj.IsSend.GetValueOrDefault();
                    this.grdDocumentFile.MasterTableView.GetColumn("DownloadColumn").Visible = changeRequestObj.IsSend.GetValueOrDefault();
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
                    var obj = this.changeRequestService.GetById(objId);
                    if (obj != null)
                    {
                        this.CollectData(obj);

                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedByName = UserSession.Current.User.FullName;
                        obj.LastUpdatedDate = DateTime.Now;

                        this.changeRequestService.Update(obj);
                    }
                }
                else
                {
                    var obj = new ChangeRequest();
                    obj.ID = Guid.NewGuid();
                    this.CollectData(obj);

                    obj.CreatedBy = UserSession.Current.User.Id;
                    obj.CreatedByName = UserSession.Current.User.FullName;
                    obj.CreatedDate = DateTime.Now;
                    this.changeRequestService.Insert(obj);
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
        }

        private void CollectData(ChangeRequest obj)
        {

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

        protected void grdDocumentFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var changeRequestId = new Guid(this.Request.QueryString["objId"]);
                var docFileList = this.changeRequestDocFileService.GetAllByChangeRequest(changeRequestId).OrderBy(t => t.FileName);

                this.grdDocumentFile.DataSource = docFileList;
            }
        }

        protected void btnProcessDocNo_OnClick(object sender, EventArgs e)
        {
            var fileIcon = new Dictionary<string, string>()
                    {
                        { ".doc", "~/images/wordfile.png" },
                        { ".docx", "~/images/wordfile.png" },
                        { ".dotx", "~/images/wordfile.png" },
                        { ".xls", "~/images/excelfile.png" },
                        { ".xlsx", "~/images/excelfile.png" },
                        { ".xlsm", "~/images/excelfile.png" },
                        { ".pdf", "~/images/pdffile.png" },
                        { ".7z", "~/images/7z.png" },
                        { ".dwg", "~/images/dwg.png" },
                        { ".dxf", "~/images/dxf.png" },
                        { ".rar", "~/images/rar.png" },
                        { ".zip", "~/images/zip.png" },
                        { ".txt", "~/images/txt.png" },
                        { ".xml", "~/images/xml.png" },
                        { ".bmp", "~/images/bmp.png" },
                    };

            var changeRequestId = new Guid(this.Request.QueryString["objId"]);
            var changeRequestObj = this.changeRequestService.GetById(changeRequestId);
            if (changeRequestObj != null)
            {
                var physicalPath = Server.MapPath("../.." + changeRequestObj.StoreFolderPath);
                var transFlag = false;
                foreach (UploadedFile docFile in radUploadDoc.UploadedFiles)
                {
                    var errorFlag = false;
                    var fileNameWithoutExtension = docFile.FileName.Replace(docFile.GetExtension(), "");
                    var shortFileName = fileNameWithoutExtension.Split('_')[0] + "_" +
                                        fileNameWithoutExtension.Split('_')[1];
                    var docObj = new ChangeRequestDocFile();
                    if (fileNameWithoutExtension.Split('_').Length >= 3)
                    {
                        // Save doc file
                        var physicalFilePath = Path.Combine(physicalPath, shortFileName + docFile.GetExtension().ToLower());
                        docFile.SaveAs(physicalFilePath, true);
                        // -----------------------------------------------------------------

                        docObj.ID = Guid.NewGuid();
                        docObj.ChangeRequestId = changeRequestObj.ID;
                        //docObj.PurposeId = changeRequestObj.PurposeId;
                        //docObj.PurposeName = changeRequestObj.PurposeName;
                        docObj.Extension = docFile.GetExtension();
                        docObj.ExtensionIcon = fileIcon.ContainsKey(docObj.Extension.ToLower())
                            ? fileIcon[docObj.Extension.ToLower()]
                            : "~/images/otherfile.png";
                        docObj.FilePath = changeRequestObj.StoreFolderPath + "/" + shortFileName + docFile.GetExtension().ToLower();
                        docObj.FileSize = (double) docFile.ContentLength/1024;

                        // Detect DocType
                        var doctypeObj = this.documentTypeService.GetByDocNo(fileNameWithoutExtension.Split('_')[0]);
                        //--------------------------------------

                        if (doctypeObj != null)
                        {
                            // Get document numbering format for Doc type
                            var documentNumbering = this.documentNumberingService.GetByDocType(doctypeObj.ID);
                            //---------------------------------------------

                            if (documentNumbering != null)
                            {
                                var partOfFullDocumentNumbering = documentNumbering.FullFormat.Split('-');
                                var partOfFileName = fileNameWithoutExtension.Split('_')[0].Replace(" ", string.Empty).Split('-');
                                if (documentNumbering.SegmentLenght <= partOfFileName.Length)
                                {
                                    var fullDocNo = string.Empty;
                                    for (int i = 0; i < documentNumbering.SegmentLenght; i++)
                                    {
                                        fullDocNo += i == documentNumbering.SegmentLenght - 1
                                            ? partOfFileName[i]
                                            : partOfFileName[i] + "-";

                                        switch (partOfFullDocumentNumbering[i])
                                        {
                                            case "1":
                                                var projectCode = partOfFileName[i];
                                                var projectCodeObj = this.projectService.GetByCode(projectCode);


                                                if (projectCodeObj != null)
                                                {
                                                    docObj.ProjectName = projectCode;
                                                    docObj.ProjectId = projectCodeObj.ID;
                                                }
                                                else
                                                {
                                                    errorFlag = true;
                                                    transFlag = true;
                                                    docObj.ProjectName = projectCode;
                                                    docObj.Status = "Missing Doc Numbering Part";
                                                    docObj.ErrorMessage += "_ Project code '" + projectCode +
                                                                           "' is invalid" +
                                                                           Environment.NewLine;
                                                    docObj.ErrorPosition += "1$";
                                                }

                                                break;
                                            case "2":
                                                docObj.DocumentTypeId = doctypeObj.ID;
                                                docObj.DocumentTypeName = partOfFileName[i];
                                                docObj.DocumentTypeGroupId = doctypeObj.ParentId;
                                                docObj.DocumentTypeGroupName = doctypeObj.ParentCode;
                                                break;
                                            case "3":
                                                var originatingOrganizationCode = partOfFileName[i];
                                                var originatingOrganizationCodeObj =
                                                    this.organizationCodeService.GetByCode(originatingOrganizationCode);
                                                if (originatingOrganizationCodeObj != null)
                                                {
                                                    docObj.OriginatingOrganizationId = originatingOrganizationCodeObj.ID;
                                                    docObj.OriginatingOrganizationName = originatingOrganizationCode;
                                                }
                                                else
                                                {
                                                    errorFlag = true;
                                                    transFlag = true;
                                                    docObj.OriginatingOrganizationName = originatingOrganizationCode;
                                                    docObj.Status = "Missing Doc Numbering Part";
                                                    docObj.ErrorMessage += "_ Originating Organization code '" +
                                                                           originatingOrganizationCode + "' is invalid" +
                                                                           Environment.NewLine;
                                                    docObj.ErrorPosition += "2$";
                                                }
                                                break;
                                            case "4":
                                                var receivingOrganizationCode = partOfFileName[i];
                                                var receivingOrganizationCodeObj =
                                                    this.organizationCodeService.GetByCode(receivingOrganizationCode);
                                                if (receivingOrganizationCodeObj != null)
                                                {
                                                    docObj.ReceivingOrganizationId = receivingOrganizationCodeObj.ID;
                                                    docObj.ReceivingOrganizationName = receivingOrganizationCode;
                                                }
                                                else
                                                {
                                                    errorFlag = true;
                                                    transFlag = true;
                                                    docObj.ReceivingOrganizationName = receivingOrganizationCode;
                                                    docObj.Status = "Missing Doc Numbering Part";
                                                    docObj.ErrorMessage += "_ Receiving Organization code '" +
                                                                           receivingOrganizationCode + "' is invalid" +
                                                                           Environment.NewLine;
                                                    docObj.ErrorPosition += "3$";
                                                }
                                                break;
                                            case "5":
                                                docObj.Year = partOfFileName[i];
                                                break;
                                            case "6":
                                                var groupCode = partOfFileName[i];
                                                var groupCodeObj = this.groupCodeService.GetByCode(groupCode);
                                                if (groupCodeObj != null)
                                                {
                                                    docObj.GroupCodeId = groupCodeObj.ID;
                                                    docObj.GroupCodeName = groupCode;
                                                }
                                                else
                                                {
                                                    errorFlag = true;
                                                    transFlag = true;
                                                    docObj.GroupCodeName = groupCode;
                                                    docObj.Status = "Missing Doc Numbering Part";
                                                    docObj.ErrorMessage += "_ Group code '" + groupCode + "' is invalid" +
                                                                           Environment.NewLine;
                                                    docObj.ErrorPosition += "4$";
                                                }
                                                break;
                                            case "7":
                                                docObj.Sequence = partOfFileName[i];
                                                break;
                                            case "8":
                                                var contentCode = partOfFileName[i];
                                                var unitCode = contentCode.Substring(0, 2);
                                                var unitObj = this.unitService.GetByCode(unitCode);
                                                if (unitObj != null)
                                                {
                                                    docObj.UnitCodeId = unitObj.ID;
                                                    docObj.UnitCodeName = unitCode;

                                                    var areaObj =
                                                        this.areaService.GetById(unitObj.AreaId.GetValueOrDefault());
                                                    if (areaObj != null)
                                                    {
                                                        docObj.AreaId = areaObj.ID;
                                                        docObj.AreaName = areaObj.Code;
                                                    }
                                                }
                                                else
                                                {
                                                    errorFlag = true;
                                                    transFlag = true;
                                                    docObj.UnitCodeName = unitCode;
                                                    docObj.Status = "Missing Doc Numbering Part";
                                                    docObj.ErrorMessage += "_ Unit code '" + unitCode + "' is invalid" +
                                                                           Environment.NewLine;
                                                    docObj.ErrorPosition += "5$";
                                                }
                                                if (contentCode.Contains('('))
                                                {
                                                    var kksCode = contentCode.Substring(2, contentCode.IndexOf('(') - 2);
                                                    var kksObj = this.kksService.GetByCode(kksCode);
                                                    if (kksObj != null)
                                                    {
                                                        docObj.KKSCodeId = kksObj.ID;
                                                        docObj.KKSCodeName = kksCode;
                                                    }
                                                    else
                                                    {
                                                        errorFlag = true;
                                                        transFlag = true;
                                                        docObj.KKSCodeName = kksCode;
                                                        docObj.Status = "Missing Doc Numbering Part";
                                                        docObj.ErrorMessage += "_ KKS code '" + kksCode +
                                                                               "' is invalid" +
                                                                               Environment.NewLine;
                                                        docObj.ErrorPosition += "6$";
                                                    }

                                                    var trainNo = contentCode.Substring(contentCode.IndexOf('(') + 1,
                                                        contentCode.IndexOf(')') - contentCode.IndexOf('(') - 1);
                                                    docObj.TrainNo = trainNo;
                                                }
                                                else
                                                {
                                                    var kksCode = contentCode.Substring(2, contentCode.Length - 2);
                                                    var kksObj = this.kksService.GetByCode(kksCode);
                                                    if (kksObj != null)
                                                    {
                                                        docObj.KKSCodeId = kksObj.ID;
                                                        docObj.KKSCodeName = kksCode;
                                                    }
                                                    else
                                                    {
                                                        errorFlag = true;
                                                        transFlag = true;
                                                        docObj.KKSCodeName = kksCode;
                                                        docObj.Status = "Missing Doc Numbering Part";
                                                        docObj.ErrorMessage += "_ KKS code '" + kksCode +
                                                                               "' is invalid" +
                                                                               Environment.NewLine;
                                                        docObj.ErrorPosition += "6$";
                                                    }
                                                }
                                                break;
                                            case "9":
                                                var disciplineCode = partOfFileName[i];
                                                var disciplineObj = this.disciplineService.GetByCode(disciplineCode);
                                                if (disciplineObj != null)
                                                {
                                                    docObj.DisciplineCodeId = disciplineObj.ID;
                                                    docObj.DisciplineCodeName = disciplineCode;
                                                }
                                                else
                                                {
                                                    errorFlag = true;
                                                    transFlag = true;
                                                    docObj.DisciplineCodeName = disciplineCode;
                                                    docObj.Status = "Missing Doc Numbering Part";
                                                    docObj.ErrorMessage += "_ Discipline code '" + disciplineCode +
                                                                           "' is invalid" +
                                                                           Environment.NewLine;
                                                    docObj.ErrorPosition += "7$";
                                                }
                                                break;
                                            case "10":
                                                var noSystem = partOfFileName[i];
                                                var disciplineCode1 = noSystem.Substring(0, 2);
                                                var disciplineObj1 = this.disciplineService.GetByCode(disciplineCode1);
                                                if (disciplineObj1 != null)
                                                {
                                                    docObj.DisciplineCodeId = disciplineObj1.ID;
                                                    docObj.DisciplineCodeName = disciplineCode1;
                                                }
                                                else
                                                {
                                                    errorFlag = true;
                                                    transFlag = true;
                                                    docObj.DisciplineCodeName = disciplineCode1;
                                                    docObj.Status = "Missing Doc Numbering Part";
                                                    docObj.ErrorMessage += "_ Discipline code '" + disciplineCode1 +
                                                                           "' is invalid" +
                                                                           Environment.NewLine;
                                                    docObj.ErrorPosition += "7$";
                                                }

                                                var sequence = noSystem.Substring(2, noSystem.Length - 2);
                                                docObj.Sequence = sequence;
                                                break;
                                        }
                                    }

                                    docObj.Revision = fileNameWithoutExtension.Split('_')[1];

                                    if (!string.IsNullOrEmpty(fullDocNo))
                                    {
                                        docObj.DocumentNo = fullDocNo;
                                        var docRegistedObj = this.documentService.GetOneByDocNo(docObj.DocumentNo);
                                        if (docRegistedObj != null)
                                        {
                                            //docObj.DocumentTitle = docRegistedObj.DocTitle;
                                            if (this.documentService.GetOneByDocNoRev(docObj.DocumentNo, docObj.Revision) != null)
                                            {
                                                errorFlag = true;
                                                transFlag = true;
                                                docObj.Status = "Duplicate Project's Document Revision";
                                                docObj.ErrorMessage += "_ Document '" + docObj.DocumentNo +
                                                                       "' with Revision '" + docObj.Revision + "' is already exist in Project's Document list." +
                                                                       Environment.NewLine;
                                                docObj.ErrorPosition += "0$";
                                            }
                                        }
                                        else
                                        {
                                            errorFlag = true;
                                            transFlag = true;
                                            docObj.Status = "Missing Project's Document";
                                            docObj.ErrorMessage += "_ Document '" + docObj.DocumentNo +
                                                                   "' isn't exist in Project's Document list." +
                                                                   Environment.NewLine;
                                            docObj.ErrorPosition += "0$";
                                        }
                                    }

                                    docObj.FileName = shortFileName + docObj.Extension.ToLower();
                                    docObj.DocumentTitle = fileNameWithoutExtension.Split('_')[2];

                                    if (string.IsNullOrEmpty(docObj.GroupCodeName))
                                    {
                                        docObj.GroupCodeId = changeRequestObj.GroupId;
                                        docObj.GroupCodeName = changeRequestObj.GroupName;
                                    }
                                }
                                else
                                {
                                    errorFlag = true;
                                    transFlag = true;
                                    docObj.DocumentNo = fileNameWithoutExtension.Split('_')[0];
                                    docObj.FileName = docFile.FileName;
                                    docObj.Status = "Missing Doc Numbering";
                                    docObj.ErrorMessage += "Document is missing Numbering format" + Environment.NewLine;
                                    docObj.ErrorPosition += "0$";
                                }
                            }
                            else
                            {
                                errorFlag = true;
                                transFlag = true;
                                docObj.DocumentNo = fileNameWithoutExtension.Split('_')[0];
                                docObj.FileName = docFile.FileName;
                                docObj.Status = "Missing Doc Numbering";
                                docObj.ErrorMessage += "Document is missing Numbering format" + Environment.NewLine;
                                docObj.ErrorPosition += "0$";
                            }
                        }
                        else
                        {
                            errorFlag = true;
                            transFlag = true;
                            docObj.DocumentNo = fileNameWithoutExtension.Split('_')[0];
                            docObj.FileName = docFile.FileName;
                            docObj.Status = "Missing Doc Numbering";
                            docObj.ErrorMessage += "Document is missing Numbering format" + Environment.NewLine;
                            docObj.ErrorPosition += "0$";
                        }
                    }
                    else
                    {
                        errorFlag = true;
                        transFlag = true;
                        docObj.DocumentNo = fileNameWithoutExtension.Split('_')[0];
                        docObj.FileName = docFile.FileName;
                        docObj.Status = "File name is invalid";
                        docObj.ErrorMessage += "File name don't follow format: DocNo_Rev_Title" + Environment.NewLine;
                        docObj.ErrorPosition += "0$";
                    }

                    if (!errorFlag)
                    {
                        docObj.ErrorMessage = string.Empty;
                        docObj.Status = string.Empty;
                    }
                    else
                    {
                        docObj.ErrorMessage += Environment.NewLine + "** Please correct ducument file name and re-attach to transmittal.";
                    }
                   
                    docObj.IsReject = false;
                    docObj.RejectReason = string.Empty;
                    this.changeRequestDocFileService.Insert(docObj);   
                }

                if (transFlag)
                {
                    changeRequestObj.Status = "Attach Doc File Invalid";
                    changeRequestObj.ErrorMessage = "Some attach document files are invalid format.";
                    changeRequestObj.IsValid = false;
                }
                else
                {
                    changeRequestObj.Status = string.Empty;
                    changeRequestObj.ErrorMessage = string.Empty;
                    changeRequestObj.IsValid = true;
                }

               this.changeRequestService.Update(changeRequestObj);
                this.grdDocumentFile.Rebind();
            }
        }

        private DataTable GetMappingFile()
        {
            var filePath = Server.MapPath("~/DocumentLibrary") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Mapping/Document Title Mapping List.xlsx");
            var Sheet = workbook.Worksheets[0];
            var datatable = new DataTable();
            datatable = Sheet.Cells.ExportDataTable(1, 0,
                                   Sheet.Cells.MaxRow - 1, 3);
            return datatable;
        }
        private void MappingTitle(ref ChangeRequestDocFile objdoc,DataTable datatable)
        {
           
            foreach (DataRow dataRow in datatable.Rows)
            {
                var DocNo = dataRow["Column1"].ToString();
                var Title = dataRow["column2"].ToString();
                var ContractorNo = dataRow["column3"].ToString();
                if (DocNo.Trim() == objdoc.DocumentNoFull)
                {
                    objdoc.DocumentTitle = Title;
                    objdoc.ContractorRefNo = ContractorNo;
                }
            }
        }
        protected void grdDocumentFile_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var item = e.Item as GridDataItem;
                var errorPosition = item["ErrorPosition"].Text;
                if (!string.IsNullOrEmpty(errorPosition))
                {
                    foreach (var position in errorPosition.Split('$').Where(t => !string.IsNullOrEmpty(t)))
                    {
                        switch (position)
                        {
                            case "0":
                                item["DocumentNo"].BackColor = Color.Red;
                                item["DocumentNo"].BorderColor = Color.Red;
                                break;
                            case "1":
                                item["ProjectName"].BackColor = Color.Red;
                                item["ProjectName"].BorderColor = Color.Red;
                                break;
                            case "2":
                                item["OriginatingOrganizationName"].BackColor = Color.Red;
                                item["OriginatingOrganizationName"].BorderColor = Color.Red;
                                break;
                            case "3":
                                item["ReceivingOrganizationName"].BackColor = Color.Red;
                                item["ReceivingOrganizationName"].BorderColor = Color.Red;
                                break;
                            case "4":
                                item["GroupCodeName"].BackColor = Color.Red;
                                item["GroupCodeName"].BorderColor = Color.Red;
                                break;
                            case "5":
                                item["UnitCodeName"].BackColor = Color.Red;
                                item["UnitCodeName"].BorderColor = Color.Red;
                                break;
                            case "6":
                                item["KKSCodeName"].BackColor = Color.Red;
                                item["KKSCodeName"].BorderColor = Color.Red;
                                break;
                            case "7":
                                item["DisciplineCodeName"].BackColor = Color.Red;
                                item["DisciplineCodeName"].BorderColor = Color.Red;
                                break;
                        }
                    }
                }
            }
        }

        protected void grdDocumentFile_OnDeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var objId = new Guid(item.GetDataKeyValue("ID").ToString());
            var obj = this.changeRequestDocFileService.GetById(objId);
            var changeRequestObj = this.changeRequestService.GetById(obj.ChangeRequestId.GetValueOrDefault());

            var physicalPath = Server.MapPath("../.." + obj.FilePath);
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }

            this.changeRequestDocFileService.Delete(objId);

            var currentTransAttachDocFile = this.changeRequestDocFileService.GetAllByChangeRequest(changeRequestObj.ID);
            if (!currentTransAttachDocFile.Any())
            {
                changeRequestObj.Status = "Missing Doc File";
                changeRequestObj.IsValid = false;
                changeRequestObj.ErrorMessage = "Missing Attach Document File.";
            }
            else if (currentTransAttachDocFile.Any(t => !string.IsNullOrEmpty(t.Status)))
            {
                changeRequestObj.Status = "Attach Doc File Invalid";
                changeRequestObj.ErrorMessage = "Some attach document files are invalid format.";
                changeRequestObj.IsValid = false;
            }
            else
            {
                changeRequestObj.Status = string.Empty;
                changeRequestObj.ErrorMessage = string.Empty;
                changeRequestObj.IsValid = true;
            }

            this.changeRequestService.Update(changeRequestObj);
            this.grdDocumentFile.Rebind();
        }

        protected void ajaxDocument_OnAjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "DeleteAll")
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var changeRequestId = new Guid(this.Request.QueryString["objId"]);
                    var changeRequestObj = this.changeRequestService.GetById(changeRequestId);

                    var errorAttachDocFile = this.changeRequestDocFileService.GetAllByChangeRequest(changeRequestId).Where(t => !string.IsNullOrEmpty(t.ErrorMessage));
                    foreach (var errorItem in errorAttachDocFile)
                    {
                        var physicalPath = Server.MapPath("../.." + errorItem.FilePath);
                        if (File.Exists(physicalPath))
                        {
                            File.Delete(physicalPath);
                        }

                        this.changeRequestDocFileService.Delete(errorItem);
                    }

                    // Update Trans status info
                    var currentTransAttachDocFile = this.changeRequestDocFileService.GetAllByChangeRequest(changeRequestObj.ID);
                    if (!currentTransAttachDocFile.Any())
                    {
                        changeRequestObj.Status = "Missing Doc File";
                        changeRequestObj.IsValid = false;
                        changeRequestObj.ErrorMessage = "Missing Attach Document File.";
                    }
                    else if (currentTransAttachDocFile.Any(t => !string.IsNullOrEmpty(t.Status)))
                    {
                        changeRequestObj.Status = "Attach Doc File Invalid";
                        changeRequestObj.ErrorMessage = "Some attach document files are invalid format.";
                        changeRequestObj.IsValid = false;
                    }
                    else
                    {
                        changeRequestObj.Status = string.Empty;
                        changeRequestObj.ErrorMessage = string.Empty;
                        changeRequestObj.IsValid = true;
                    }

                    this.changeRequestService.Update(changeRequestObj);
                    // ----------------------------------------------------------------------------------------------------------

                    this.grdDocumentFile.Rebind();
                }
            }
        }
    }
}