// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data;
using System.Linq;
using Aspose.Cells;
using EDMs.Data.Entities;
using EDMs.Web.Utilities;
using iTextSharp.text.pdf;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Hosting;
    using System.Web.UI;
    using EDMs.Business.Services.Document;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;
    using EDMs.Business.Services.Library;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ChangeRequestAttachDocument : Page
    {

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly ChangeRequestService changeRequestervice;

        private readonly ChangeRequestAttachFileService changeRequestAttachFileService;

        private readonly ChangeRequestDocFileService changeRequestDocFileService;

        private readonly ChangeRequestTypeService changeRequestTypeService;

        private readonly PECC2DocumentsService pecc2DocumentsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ChangeRequestAttachDocument()
        {
            this.changeRequestervice = new  ChangeRequestService();
            this.changeRequestAttachFileService = new   ChangeRequestAttachFileService();
            this.changeRequestDocFileService = new ChangeRequestDocFileService();
            this.changeRequestTypeService = new ChangeRequestTypeService();
            this.pecc2DocumentsService = new PECC2DocumentsService();

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
                var objId = new Guid(this.Request.QueryString["objId"]);
                var changeRequestObj = this.changeRequestervice.GetById(objId);
                this.divUpload.Visible = !UserSession.Current.User.Role.IsInternal.GetValueOrDefault() && !changeRequestObj.IsSend.GetValueOrDefault();
                this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = !UserSession.Current.User.Role.IsInternal.GetValueOrDefault() && !changeRequestObj.IsSend.GetValueOrDefault();

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
            var objId = new Guid(this.Request.QueryString["objId"]);
            var changeRequestObj = this.changeRequestervice.GetById(objId);
            var fileIcon = new Dictionary<string, string>()
                    {
                        { "doc", "~/images/wordfile.png" },
                        { "docx", "~/images/wordfile.png" },
                        { "dotx", "~/images/wordfile.png" },
                        { "xls", "~/images/excelfile.png" },
                        { "xlsx", "~/images/excelfile.png" },
                        { "pdf", "~/images/pdffile.png" },
                        { "7z", "~/images/7z.png" },
                        { "dwg", "~/images/dwg.png" },
                        { "dxf", "~/images/dxf.png" },
                        { "rar", "~/images/rar.png" },
                        { "zip", "~/images/zip.png" },
                        { "txt", "~/images/txt.png" },
                        { "xml", "~/images/xml.png" },
                        { "xlsm", "~/images/excelfile.png" },
                        { "bmp", "~/images/bmp.png" },
                    };

            var targetFolder = "../.." + changeRequestObj.StoreFolderPath;
            var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + changeRequestObj.StoreFolderPath;
            foreach (UploadedFile docFile in docuploader.UploadedFiles)
            {
                var fileExt = docFile.FileName.Substring(docFile.FileName.LastIndexOf(".") + 1,
                    docFile.FileName.Length - docFile.FileName.LastIndexOf(".") - 1);
                var filename = docFile.FileName;
                // Path file to save on server disc
                var saveFilePath = Path.Combine(Server.MapPath(targetFolder), filename);

                // Path file to download from server
                var serverFilePath = serverFolder + "/" + filename;
                docFile.SaveAs(saveFilePath, true);
                
                var attachFile = new ChangeRequestAttachFile()
                {
                    ID = Guid.NewGuid(),
                    ChangeRequestId = objId,
                    FileName = filename,
                    Extension = fileExt,
                    FilePath = serverFilePath,
                    ExtensionIcon = fileIcon.ContainsKey(fileExt.ToLower())
                            ? fileIcon[fileExt.ToLower()]
                            : "~/images/otherfile.png",
                    FileSize = (double)docFile.ContentLength / 1024,
                    CreatedBy = UserSession.Current.User.Id,
                    CreatedByName = UserSession.Current.User.FullName,
                    CreatedDate = DateTime.Now,
                    IsPECC2Attach = UserSession.Current.User.Role.IsInternal,
                    TypeId = this.cbChangeRequestForm.Checked
                                ? 1 : 5,
                    TypeName = this.cbChangeRequestForm.Checked
                                ? "Change Request Form"
                                : "CRS Response File",
                };

                this.changeRequestAttachFileService.Insert(attachFile);
            }

            this.docuploader.UploadedFiles.Clear();
            this.grdDocument.Rebind();
        }
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    this.Session.Remove("IsFillData");
        //    if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
        //    {
        //        var objId = new Guid(this.Request.QueryString["objId"]);
        //        var docObj = this._ChangeRequestervice.GetById(objId);

        //        var flag = false;
        //        var targetFolder = this.ddlType.SelectedValue == "1" ? "../../DocumentLibrary/ChangeRequest" : "../../DocumentLibrary/MarkupFile";
        //        var serverFolder = this.ddlType.SelectedValue == "1" 
        //            ? (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath) 
        //            +  "/DocumentLibrary/ChangeRequest"
        //            : (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
        //            + "/DocumentLibrary/MarkupFile";


        //        var listUpload = docuploader.UploadedFiles;

        //        if (listUpload.Count > 0)
        //        {
        //            foreach (UploadedFile docFile in listUpload)
        //            {
        //                var docFileName = docFile.FileName;
        //                var fileExt = docFile.GetExtension();

        //                var serverDocFileName = docFileName;

        //                if (this.ddlType.SelectedValue == "2")
        //                {
        //                    serverDocFileName = docFile.GetNameWithoutExtension() + "_MarkupConsolidate_" + UserSession.Current.User.Username + fileExt;
        //                }
        //                else if (this.ddlType.SelectedValue == "3")
        //                {
        //                    serverDocFileName = docFile.GetNameWithoutExtension() + "_Markup_" + UserSession.Current.User.Username + fileExt;
        //                }

        //                // Path file to save on server disc
        //                var saveFilePath = Path.Combine(Server.MapPath(targetFolder), serverDocFileName);

        //                // Path file to download from server
        //                var serverFilePath = serverFolder + "/" + serverDocFileName;

        //                docFile.SaveAs(saveFilePath, true);

        //                var attachFile = new ChangeRequestAttachFile()
        //                {
        //                    ID = Guid.NewGuid(),
        //                    ChangeRequestId = objId,
        //                    FileName = serverDocFileName,
        //                    Extension = fileExt,
        //                    FilePath = serverFilePath,
        //                    ExtensionIcon = Utility.FileIcon.ContainsKey(fileExt.ToLower()) ? Utility.FileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
        //                    FileSize = (double)docFile.ContentLength / 1024,
        //                    TypeId = Convert.ToInt32(this.ddlType.SelectedValue),
        //                    TypeName = this.ddlType.SelectedItem.Text,
        //                    CreatedBy = UserSession.Current.User.Id,
        //                    CreatedByName = UserSession.Current.User.UserNameWithFullName,
        //                    CreatedDate = DateTime.Now
        //                };

        //                this.attachFileService.Insert(attachFile);

        //                // Extract only comment page
        //                if (this.ddlType.SelectedValue == "3" && fileExt.ToLower() == ".pdf")
        //                {
        //                    var outputOnlyMarkupPageFileName = docFile.GetNameWithoutExtension() + "_OnlyMarkupPages_" + UserSession.Current.User.Username + fileExt;
        //                    var outputOnlyMarkupPageFilePath = Server.MapPath("../../DocumentLibrary/MarkupFile/" + outputOnlyMarkupPageFileName);
        //                    PdfImportedPage importedPage = null;
        //                    var pdfReader = new PdfReader(saveFilePath);

        //                    var sourceDocument = new iTextSharp.text.Document(pdfReader.GetPageSizeWithRotation(1));
        //                    var pdfCopyProvider = new PdfCopy(sourceDocument, new FileStream(outputOnlyMarkupPageFilePath, FileMode.Create));
        //                    sourceDocument.Open();
        //                    for (int i = 1; i <= pdfReader.NumberOfPages; i++)
        //                    {
        //                        PdfArray array = pdfReader.GetPageN(i).GetAsArray(PdfName.ANNOTS);
        //                        if (array != null)
        //                        {
        //                            importedPage = pdfCopyProvider.GetImportedPage(pdfReader, i);
        //                            pdfCopyProvider.AddPage(importedPage);
        //                        }
        //                    }

        //                    sourceDocument.Close();
        //                    pdfReader.Close();

        //                    // Add to attach file
        //                    var fileInfo = new FileInfo(outputOnlyMarkupPageFilePath);
        //                    if (fileInfo.Exists)
        //                    {
        //                        var onlyMarkupPageAttachFile = new ChangeRequestAttachFile()
        //                        {
        //                            ID = Guid.NewGuid(),
        //                            ChangeRequestId = objId,
        //                            FileName = outputOnlyMarkupPageFileName,
        //                            Extension = fileExt,
        //                            FilePath = "/DocumentLibrary/MarkupFile/" + outputOnlyMarkupPageFileName,
        //                            ExtensionIcon = Utility.FileIcon.ContainsKey(fileExt.ToLower()) ? Utility.FileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
        //                            FileSize = (double)fileInfo.Length / 1024,
        //                            TypeId = Convert.ToInt32(this.ddlType.SelectedValue),
        //                            TypeName = this.ddlType.SelectedItem.Text,
        //                            CreatedBy = UserSession.Current.User.Id,
        //                            CreatedByName = UserSession.Current.User.UserNameWithFullName,
        //                            CreatedDate = DateTime.Now,
        //                            IsOnlyMarkupPage = true
        //                        };

        //                        this.attachFileService.Insert(onlyMarkupPageAttachFile);
        //                    }
        //                    // -------------------------------------------------------------------------------------------------------------------
        //                }

        //                // -------------------------------------------------------------------------------------------------
        //            }

        //            docObj.IsHasAttachFile = this.attachFileService.GetByChangeRequest(objId).Any();
        //            this._ChangeRequestervice.Update(docObj);
        //        }
        //    }

        //    this.docuploader.UploadedFiles.Clear();
        //    this.grdDocument.Rebind();
        //}

        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            Guid objId;
            Guid.TryParse(item.GetDataKeyValue("ID").ToString(), out objId);
            this.changeRequestAttachFileService.Delete(objId);
            this.grdDocument.Rebind();
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                Guid objId;
                Guid.TryParse(this.Request.QueryString["objId"].ToString(), out objId);
               
                var attachList = this.changeRequestAttachFileService.GetByChangeRequest(objId).OrderBy(t => t.CreatedDate);
                //this.btnExportCRS.Visible = attachList.Any(t => t.TypeId == 3);
                this.grdDocument.DataSource = attachList.Where(t=> !t.IsOnlyMarkupPage.GetValueOrDefault());
            }
            else
            {
                this.grdDocument.DataSource = new List<ChangeRequestAttachFile>();
            }
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }

        protected void btnExportChangeRequestForm_OnClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                var changeRequestObj = this.changeRequestervice.GetById(new Guid(Request.QueryString["objId"]));
                if (changeRequestObj != null)
                {
                    var changeRequestType = this.changeRequestTypeService.GetById(changeRequestObj.TypeId.GetValueOrDefault());
                    var refDocList = changeRequestObj.RefDocId.Split(';').Where(t => !string.IsNullOrEmpty(t))
                        .Select(t => this.pecc2DocumentsService.GetById(new Guid(t)));
                    var attachdocFileList = this.changeRequestDocFileService.GetAllByChangeRequest(new Guid(Request.QueryString["objId"]));
                    var filePath = Server.MapPath("../../Exports") + @"\";
                    var workbook = new Workbook(filePath + @"Template\PECC2_ChangeRequestNewTemplate.xlsm");
                    var dataSheet = workbook.Worksheets[0];

                    dataSheet.Cells["C2"].PutValue(changeRequestType.Description);
                    dataSheet.Cells["F2"].PutValue(changeRequestType.Code + " No.");
                    dataSheet.Cells["G2"].PutValue(changeRequestObj.Number);
                    dataSheet.Cells["G3"].PutValue(changeRequestObj.ChangeGradeCodeName);

                    // Fill Ref document
                    var dtFull = new DataTable();
                    dtFull.Columns.AddRange(new[]
                    {
                        new DataColumn("DocNo", typeof (String)),
                        new DataColumn("EmptyColumn", typeof (String)),
                        new DataColumn("Rev", typeof (String)),
                        new DataColumn("Title", typeof (String)),
                    });

                    foreach (var refDocument in refDocList)
                    {
                        var dataRow = dtFull.NewRow();
                        dataRow["DocNo"] = refDocument.DocNo;
                        dataRow["Rev"] = refDocument.Revision;
                        dataRow["Title"] = refDocument.DocTitle;
                        dtFull.Rows.Add(dataRow);
                    }

                    dataSheet.Cells.ImportDataTable(dtFull, false, 10, 1, dtFull.Rows.Count, dtFull.Columns.Count, true);
                    for (int i = 1; i < dtFull.Rows.Count; i++)
                    {
                        dataSheet.Cells.Merge(10 + i, 1, 1, 2);
                        dataSheet.Cells.Merge(10 + i, 4, 1, 3);
                    }
                    // -----------------------------------------------------------------------------------------------------

                    dataSheet.Cells[13 + dtFull.Rows.Count - 1, 1].PutValue(changeRequestObj.ReasonForChange);
                    dataSheet.Cells[15 + dtFull.Rows.Count - 1, 1].PutValue(changeRequestObj.ExistingCondition);
                    dataSheet.Cells[17 + dtFull.Rows.Count - 1, 1].PutValue(changeRequestObj.Description);

                    // Fill Attach Doc file
                    var dtAttachDocFileFull = new DataTable();
                    dtAttachDocFileFull.Columns.AddRange(new[]
                    {
                        new DataColumn("DocNo", typeof (String)),
                        new DataColumn("EmptyColumn", typeof (String)),
                        new DataColumn("Rev", typeof (String)),
                        new DataColumn("Title", typeof (String)),
                    });
                    foreach (var changeRequestDocFile in attachdocFileList)
                    {
                        var dataRow = dtAttachDocFileFull.NewRow();
                        dataRow["DocNo"] = changeRequestDocFile.DocumentNo;
                        dataRow["Rev"] = changeRequestDocFile.Revision;
                        dataRow["Title"] = changeRequestDocFile.DocumentTitle;
                        dtAttachDocFileFull.Rows.Add(dataRow);
                    }

                    dataSheet.Cells.ImportDataTable(dtAttachDocFileFull, false, 35 + dtFull.Rows.Count - 1, 1, dtAttachDocFileFull.Rows.Count, dtAttachDocFileFull.Columns.Count, true);
                    for (int i = 1; i < dtAttachDocFileFull.Rows.Count; i++)
                    {
                        dataSheet.Cells.Merge(35 + dtFull.Rows.Count - 1 + i, 1, 1, 2);
                        dataSheet.Cells.Merge(35 + dtFull.Rows.Count - 1 + i, 4, 1, 3);
                    }
                    // -----------------------------------------------------------------------------------------------------

                    dataSheet.Name = changeRequestType.Code;
                    var filename = Utility.RemoveSpecialCharacterFileName(changeRequestObj.Number) + "_ChangeRequestForm.xlsm";
                    var saveFilePath = Server.MapPath("../.."+ changeRequestObj.StoreFolderPath + "/" + filename);
                    workbook.Save(saveFilePath);
                    // Attach CRS to document Obj
                    //var fileInfo = new FileInfo(saveFilePath);
                    //if (fileInfo.Exists && this.changeRequestAttachFileService.GetByChangeRequest(changeRequestObj.ID).All(t => t.TypeId != 1))
                    //{
                    //    var attachFile = new ChangeRequestAttachFile()
                    //    {
                    //        ID = Guid.NewGuid(),
                    //        ChangeRequestId = changeRequestObj.ID,
                    //        FileName = filename,
                    //        Extension = ".xlsm",
                    //        FilePath = changeRequestObj.StoreFolderPath + "/" + filename,
                    //        ExtensionIcon = "~/images/excelfile.png",
                    //        FileSize = (double)fileInfo.Length / 1024,
                    //        TypeId = 1,
                    //        TypeName = "Change Request Form",
                    //        CreatedBy = UserSession.Current.User.Id,
                    //        CreatedByName = UserSession.Current.User.UserNameWithFullName,
                    //        CreatedDate = DateTime.Now,
                    //        IsPECC2Attach = UserSession.Current.User.Role.IsInternal
                    //    };

                    //    this.changeRequestAttachFileService.Insert(attachFile);
                    //}
                    // -------------------------------------------------------------------------------------------------

                    //this.grdDocument.Rebind();
                    this.Download_File(saveFilePath);
                }
            }
        }

        private void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
        }

        protected void grdPECC2AttachFile_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                Guid objId;
                Guid.TryParse(this.Request.QueryString["objId"].ToString(), out objId);
                this.grdPECC2AttachFile.DataSource = this.changeRequestAttachFileService.GetByChangeRequest(objId).Where(t => t.IsPECC2Attach.GetValueOrDefault()).OrderBy(t => t.CreatedDate);
            }
            else
            {
                this.grdPECC2AttachFile.DataSource = new List<ChangeRequestAttachFile>();
            }
        }
    }
}