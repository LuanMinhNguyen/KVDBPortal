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
using iTextSharp.text.pdf;
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
    public partial class ContractorTransmittalAttachChangeRequestFile : Page
    {
        private readonly ContractorTransmittalService transmittalService;

        private readonly ContractorTransmittalDocFileService contractorTransmittalDocFileService;

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

        private readonly ChangeRequestService changeRequestService;

        private readonly ChangeRequestTypeService changeRequestTypeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ContractorTransmittalAttachChangeRequestFile()
        {
            this.transmittalService = new ContractorTransmittalService();
            this.projectService = new ProjectCodeService();
            this.contractorTransmittalDocFileService = new ContractorTransmittalDocFileService();
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
            this.changeRequestService = new ChangeRequestService();
            this.changeRequestTypeService = new ChangeRequestTypeService();
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
                    var obj = this.transmittalService.GetById(objId);
                    if (obj != null)
                    {
                        this.CollectData(obj);

                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedByName = UserSession.Current.User.FullName;
                        obj.LastUpdatedDate = DateTime.Now;

                        this.transmittalService.Update(obj);
                    }
                }
                else
                {
                    var obj = new ContractorTransmittal();
                    obj.ID = Guid.NewGuid();
                    this.CollectData(obj);

                    obj.CreatedBy = UserSession.Current.User.Id;
                    obj.CreatedByName = UserSession.Current.User.FullName;
                    obj.CreatedDate = DateTime.Now;
                    this.transmittalService.Insert(obj);
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
        }

        private void CollectData(ContractorTransmittal obj)
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
                var transId = new Guid(this.Request.QueryString["objId"]);
                var docFileList = this.contractorTransmittalDocFileService.GetAllByTrans(transId).OrderBy(t => t.FileName);

                this.grdDocumentFile.DataSource = docFileList;
            }
        }

        protected void btnProcessDocNo_OnClick(object sender, EventArgs e)
        {
            var fileIcon = new Dictionary<string, string>()
            {
                {".doc", "~/images/wordfile.png"},
                {".docx", "~/images/wordfile.png"},
                {".dotx", "~/images/wordfile.png"},
                {".xls", "~/images/excelfile.png"},
                {".xlsx", "~/images/excelfile.png"},
                {".xlsm", "~/images/excelfile.png"},
                {".pdf", "~/images/pdffile.png"},
                {".7z", "~/images/7z.png"},
                {".dwg", "~/images/dwg.png"},
                {".dxf", "~/images/dxf.png"},
                {".rar", "~/images/rar.png"},
                {".zip", "~/images/zip.png"},
                {".txt", "~/images/txt.png"},
                {".xml", "~/images/xml.png"},
                {".bmp", "~/images/bmp.png"},
            };

            var transId = new Guid(this.Request.QueryString["objId"]);
            var transObj = this.transmittalService.GetById(transId);
            if (transObj != null)
            {
                var physicalPath = Server.MapPath("../.." + transObj.StoreFolderPath);
                var transFlag = false;
                DataTable datatable = this.GetMappingFile();

                foreach (UploadedFile docFile in radUploadDoc.UploadedFiles)
                {
                    var errorFlag = false;

                    // Save doc file
                    var physicalFilePath = Path.Combine(physicalPath, docFile.FileName);
                    docFile.SaveAs(physicalFilePath, true);
                    // -----------------------------------------------------------------

                    var shortFileName = docFile.FileName.Replace(docFile.GetExtension(), "");
                    var docObj = new ContractorTransmittalDocFile();
                    docObj.ID = Guid.NewGuid();
                    docObj.TransId = transObj.ID;
                    docObj.Extension = docFile.GetExtension();
                    docObj.ExtensionIcon = fileIcon.ContainsKey(docObj.Extension.ToLower())
                        ? fileIcon[docObj.Extension.ToLower()]
                        : "~/images/otherfile.png";
                    docObj.FilePath = transObj.StoreFolderPath + "/" + docFile.FileName;
                    docObj.FileSize = (double) docFile.ContentLength/1024;


                    var partOfFileName = shortFileName.Replace(" ", string.Empty).Split('-');
                    if (partOfFileName.Length == 5)
                    {
                        var fullDocNo = shortFileName;
                        // Check Project Code
                        var projectCode = partOfFileName[0];
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
                            docObj.ErrorMessage += "_ Project code '" + projectCode + "' is invalid" +
                                                    Environment.NewLine;
                            docObj.ErrorPosition += "1$";
                        }
                        // ------------------------------------------------------------------------------

                        // Check Change Request Type Code
                        var typeCode = partOfFileName[1];
                        var typeCodeObj = this.changeRequestTypeService.GetByCode(typeCode);
                        if (typeCodeObj != null)
                        {
                            docObj.ChangeRequestTypeName = typeCode;
                            docObj.ChangeRequestTypeId = typeCodeObj.ID;
                        }
                        else
                        {
                            errorFlag = true;
                            transFlag = true;
                            docObj.ProjectName = typeCode;
                            docObj.Status = "Missing Doc Numbering Part";
                            docObj.ErrorMessage += "_ Change Request Type '" + typeCode + "' is invalid" +
                                                    Environment.NewLine;
                            docObj.ErrorPosition += "2$";
                        }
                        // ------------------------------------------------------------------------------

                        // Check Change Request Type Code
                        var groupCode = partOfFileName[2];
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
                            docObj.ErrorPosition += "3$";
                        }
                        // ------------------------------------------------------------------------------

                        docObj.Year = partOfFileName[3];
                        docObj.Sequence = partOfFileName[4];
                        docObj.DocumentNo = fullDocNo;

                        docObj.FileName = docFile.FileName;
                        docObj.Revision = string.Empty;
                    }
                    else
                    {
                        errorFlag = true;
                        transFlag = true;
                        docObj.DocumentNo = shortFileName;
                        docObj.FileName = docFile.FileName;
                        docObj.Status = "Missing Doc Numbering";
                        docObj.ErrorMessage += "Document is missing Numbering format" + Environment.NewLine;
                        docObj.ErrorPosition += "0$";
                    }

                    if (!errorFlag)
                    {
                        docObj.ErrorMessage = string.Empty;
                        docObj.Status = string.Empty;
                    }
                    else
                    {
                        docObj.ErrorMessage += Environment.NewLine +
                                               "** Please correct ducument file name and re-attach to transmittal.";
                    }

                    docObj.IsReject = false;
                    docObj.RejectReason = string.Empty;
                    this.MappingTitle(ref docObj, datatable);
                    this.contractorTransmittalDocFileService.Insert(docObj);
                }

                if (transFlag)
                {
                    transObj.Status = "Attach Doc File Invalid";
                    transObj.ErrorMessage = "Some attach document files are invalid format.";
                    transObj.IsValid = false;
                }
                else
                {
                    transObj.Status = string.Empty;
                    transObj.ErrorMessage = string.Empty;
                    transObj.IsValid = true;
                }

                this.transmittalService.Update(transObj);
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
        private void MappingTitle(ref ContractorTransmittalDocFile objdoc,DataTable datatable)
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
                                item["ChangeRequestTypeName"].BackColor = Color.Red;
                                item["ChangeRequestTypeName"].BorderColor = Color.Red;
                                break;
                            case "3":
                                item["GroupCodeName"].BackColor = Color.Red;
                                item["GroupCodeName"].BorderColor = Color.Red;
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
            var obj = this.contractorTransmittalDocFileService.GetById(objId);
            var transObj = this.transmittalService.GetById(obj.TransId.GetValueOrDefault());

            var physicalPath = Server.MapPath("../.." + obj.FilePath);
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }

            this.contractorTransmittalDocFileService.Delete(objId);

            var currentTransAttachDocFile = this.contractorTransmittalDocFileService.GetAllByTrans(transObj.ID);
            if (!currentTransAttachDocFile.Any())
            {
                transObj.Status = "Missing Doc File";
                transObj.IsValid = false;
                transObj.ErrorMessage = "Missing Attach Document File.";
            }
            else if (currentTransAttachDocFile.Any(t => !string.IsNullOrEmpty(t.Status)))
            {
                transObj.Status = "Attach Doc File Invalid";
                transObj.ErrorMessage = "Some attach document files are invalid format.";
                transObj.IsValid = false;
            }
            else
            {
                transObj.Status = string.Empty;
                transObj.ErrorMessage = string.Empty;
                transObj.IsValid = true;
            }

            this.transmittalService.Update(transObj);
            this.grdDocumentFile.Rebind();
        }

        protected void ajaxDocument_OnAjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "DeleteAll")
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var transId = new Guid(this.Request.QueryString["objId"]);
                    var transObj = this.transmittalService.GetById(transId);

                    var errorAttachDocFile = this.contractorTransmittalDocFileService.GetAllByTrans(transId).Where(t => !string.IsNullOrEmpty(t.ErrorMessage));
                    foreach (var errorItem in errorAttachDocFile)
                    {
                        var physicalPath = Server.MapPath("../.." + errorItem.FilePath);
                        if (File.Exists(physicalPath))
                        {
                            File.Delete(physicalPath);
                        }

                        this.contractorTransmittalDocFileService.Delete(errorItem);
                    }

                    // Update Trans status info
                    var currentTransAttachDocFile = this.contractorTransmittalDocFileService.GetAllByTrans(transObj.ID);
                    if (!currentTransAttachDocFile.Any())
                    {
                        transObj.Status = "Missing Doc File";
                        transObj.IsValid = false;
                        transObj.ErrorMessage = "Missing Attach Document File.";
                    }
                    else if (currentTransAttachDocFile.Any(t => !string.IsNullOrEmpty(t.Status)))
                    {
                        transObj.Status = "Attach Doc File Invalid";
                        transObj.ErrorMessage = "Some attach document files are invalid format.";
                        transObj.IsValid = false;
                    }
                    else
                    {
                        transObj.Status = string.Empty;
                        transObj.ErrorMessage = string.Empty;
                        transObj.IsValid = true;
                    }

                    this.transmittalService.Update(transObj);
                    // ----------------------------------------------------------------------------------------------------------

                    this.grdDocumentFile.Rebind();
                }
            }
        }
    }
}