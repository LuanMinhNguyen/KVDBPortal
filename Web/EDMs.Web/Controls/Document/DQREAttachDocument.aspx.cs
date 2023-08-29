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

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class DQREAttachDocument : Page
    {

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly DQREDocumentService _dqreDocumentService;

        private readonly DQREDocumentAttachFileService attachFileService;


        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public DQREAttachDocument()
        {
            this._dqreDocumentService = new  DQREDocumentService();
            this.attachFileService = new   DQREDocumentAttachFileService();
         
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
            this.Session.Remove("IsFillData");
            if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                Guid docId;
                Guid.TryParse(this.Request.QueryString["docId"].ToString(), out docId);
                var docObj = this._dqreDocumentService.GetById(docId);
              
                var flag = false;
                var targetFolder = this.ddlType.SelectedValue == "1" ? "../../DocumentLibrary/ProjectDocs" : "../../DocumentLibrary/MarkupFile";
                var serverFolder = this.ddlType.SelectedValue == "1" 
                    ? (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath) 
                    +  "/DocumentLibrary/ProjectDocs"
                    : (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + "/DocumentLibrary/MarkupFile";


                var listUpload = docuploader.UploadedFiles;

                if (listUpload.Count > 0)
                {
                    foreach (UploadedFile docFile in listUpload)
                    {
                        var docFileName = docFile.FileName;
                        var fileExt = docFile.GetExtension();

                        var serverDocFileName = docFileName;

                        if (this.ddlType.SelectedValue == "2")
                        {
                            serverDocFileName = docFile.GetNameWithoutExtension() + "_MarkupConsolidate_" + UserSession.Current.User.Username + fileExt;
                        }
                        else if (this.ddlType.SelectedValue == "3")
                        {
                            serverDocFileName = docFile.GetNameWithoutExtension() + "_Markup_" + UserSession.Current.User.Username + fileExt;
                        }

                        // Path file to save on server disc
                        var saveFilePath = Path.Combine(Server.MapPath(targetFolder), serverDocFileName);

                        // Path file to download from server
                        var serverFilePath = serverFolder + "/" + serverDocFileName;

                        docFile.SaveAs(saveFilePath, true);

                        var attachFile = new DQREDocumentAttachFile()
                        {
                            ID = Guid.NewGuid(),
                            ProjectDocumentId = docId,
                            FileName = serverDocFileName,
                            Extension = fileExt,
                            FilePath = serverFilePath,
                            ExtensionIcon = Utility.FileIcon.ContainsKey(fileExt.ToLower()) ? Utility.FileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
                            FileSize = (double)docFile.ContentLength / 1024,
                            TypeId = Convert.ToInt32(this.ddlType.SelectedValue),
                            TypeName = this.ddlType.SelectedItem.Text,
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedByName = UserSession.Current.User.UserNameWithFullName,
                            CreatedDate = DateTime.Now
                        };

                        this.attachFileService.Insert(attachFile);

                        // Extract only comment page
                        if (this.ddlType.SelectedValue == "3" && fileExt.ToLower() == ".pdf")
                        {
                            var outputOnlyMarkupPageFileName = docFile.GetNameWithoutExtension() + "_OnlyMarkupPages_" + UserSession.Current.User.Username + fileExt;
                            var outputOnlyMarkupPageFilePath = Server.MapPath("../../DocumentLibrary/MarkupFile/" + outputOnlyMarkupPageFileName);
                            PdfImportedPage importedPage = null;
                            var pdfReader = new PdfReader(saveFilePath);
                            
                            var sourceDocument = new iTextSharp.text.Document(pdfReader.GetPageSizeWithRotation(1));
                            var pdfCopyProvider = new PdfCopy(sourceDocument, new FileStream(outputOnlyMarkupPageFilePath, FileMode.Create));
                            sourceDocument.Open();
                            for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                            {
                                PdfArray array = pdfReader.GetPageN(i).GetAsArray(PdfName.ANNOTS);
                                if (array != null)
                                {
                                    importedPage = pdfCopyProvider.GetImportedPage(pdfReader, i);
                                    pdfCopyProvider.AddPage(importedPage);
                                }
                            }

                            sourceDocument.Close();
                            pdfReader.Close();

                            // Add to attach file
                            var fileInfo = new FileInfo(outputOnlyMarkupPageFilePath);
                            if (fileInfo.Exists)
                            {
                                var onlyMarkupPageAttachFile = new DQREDocumentAttachFile()
                                {
                                    ID = Guid.NewGuid(),
                                    ProjectDocumentId = docId,
                                    FileName = outputOnlyMarkupPageFileName,
                                    Extension = fileExt,
                                    FilePath = "/DocumentLibrary/MarkupFile/" + outputOnlyMarkupPageFileName,
                                    ExtensionIcon = Utility.FileIcon.ContainsKey(fileExt.ToLower()) ? Utility.FileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
                                    FileSize = (double)fileInfo.Length / 1024,
                                    TypeId = Convert.ToInt32(this.ddlType.SelectedValue),
                                    TypeName = this.ddlType.SelectedItem.Text,
                                    CreatedBy = UserSession.Current.User.Id,
                                    CreatedByName = UserSession.Current.User.UserNameWithFullName,
                                    CreatedDate = DateTime.Now,
                                    IsOnlyMarkupPage = true
                                };

                                this.attachFileService.Insert(onlyMarkupPageAttachFile);
                            }
                            // -------------------------------------------------------------------------------------------------------------------
                        }
                        
                        // -------------------------------------------------------------------------------------------------


                    }

                    docObj.IsHasAttachFile = this.attachFileService.GetAllDocId(docId).Any();
                    this._dqreDocumentService.Update(docObj);
                }
            }

            this.docuploader.UploadedFiles.Clear();
            this.grdDocument.Rebind();
        }

        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            Guid docId;
            Guid.TryParse(item.GetDataKeyValue("ID").ToString(), out docId);
            this.attachFileService.Delete(docId);
            this.grdDocument.Rebind();
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                Guid docId;
                Guid.TryParse(this.Request.QueryString["docId"].ToString(), out docId);
               
                var attachList = this.attachFileService.GetAllDocId(docId).OrderBy(t => t.CreatedDate);
                this.btnExportCRS.Visible = attachList.Any(t => t.TypeId == 3);
                this.grdDocument.DataSource = attachList.Where(t=> !t.IsOnlyMarkupPage.GetValueOrDefault());
            }
            else
            {
                this.grdDocument.DataSource = new List<DQRETransmittalAttachFileService>();
            }
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }

        protected void btnExportCRS_OnClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                var consolidateDocFile = this.attachFileService.GetAllDocId(new Guid(Request.QueryString["docId"])).FirstOrDefault(t => t.TypeId == 3);
                if (consolidateDocFile != null)
                {
                    var filePath = Server.MapPath("../../Exports") + @"\";
                    var workbook = new Workbook(filePath + @"Template\DQRE_CRS_Template.xlsm");
                    var dataSheet = workbook.Worksheets[0];

                    var docObj = this._dqreDocumentService.GetById(consolidateDocFile.ProjectDocumentId.GetValueOrDefault());
                    if (docObj != null)
                    {
                        dataSheet.Cells["B4"].PutValue("Doc. No.: " + docObj.DocumentNo);
                        dataSheet.Cells["B5"].PutValue("Rev. No.: " + docObj.Revision);
                        dataSheet.Cells["B6"].PutValue("Transmittal Contractor: " + Environment.NewLine + docObj.IncomingTransNo);
                        dataSheet.Cells["E4"].PutValue(docObj.DocumentTitle);
                        dataSheet.Cells["H4"].PutValue(docObj.DocumentClassName);
                        dataSheet.Cells["H5"].PutValue(docObj.DocumentCodeName);
                        dataSheet.Cells["G6"].PutValue("Transmittal No.: " + Environment.NewLine + docObj.OutgoingTransNo);
                        var dtFull = new DataTable();
                        dtFull.Columns.AddRange(new[]
                        {
                        new DataColumn("NoIndex", typeof (String)),
                        new DataColumn("FileName", typeof (String)),
                        new DataColumn("Page", typeof (String)),
                        new DataColumn("Comment", typeof (String)),
                        new DataColumn("Review", typeof (String)),
                        new DataColumn("User", typeof (String)),
                        new DataColumn("Confirmation", typeof (String)),
                        });

                        var pdfReader = new PdfReader(Server.MapPath("../.." + consolidateDocFile.FilePath));
                        var count = 0;
                        for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                        {
                            PdfArray array = pdfReader.GetPageN(i).GetAsArray(PdfName.ANNOTS);
                            if (array == null) continue;
                            for (int j = 0; j < array.Size; j++)
                            {
                                var annot = array.GetAsDict(j);
                                var text = annot.GetAsString(PdfName.CONTENTS);
                                if (text != null && !string.IsNullOrEmpty(text.ToString()))
                                {
                                    var username = annot.GetAsString(PdfName.T);
                                    count += 1;
                                    var dataRow = dtFull.NewRow();
                                    dataRow["NoIndex"] = count;
                                    dataRow["FileName"] = consolidateDocFile.FileName.Replace("_Consolidate", string.Empty);
                                    dataRow["Page"] = i;
                                    dataRow["Comment"] = text;
                                    dataRow["Review"] = username;
                                    dataRow["User"] = consolidateDocFile.CreatedByName.Split('/')[0];
                                    dataRow["Confirmation"] = string.Empty;

                                    dtFull.Rows.Add(dataRow);
                                }
                            }
                        }
                        dataSheet.Cells.ImportDataTable(dtFull, false, 8, 1, dtFull.Rows.Count, dtFull.Columns.Count, true);
                        dataSheet.Cells.DeleteRow(8 + dtFull.Rows.Count);

                        var filename = Utility.RemoveSpecialCharacterFileName(docObj.DocumentNo) + "_CRS.xlsm";
                        var saveFilePath = Server.MapPath("../../DocumentLibrary/MarkupFile/" + filename);
                        workbook.Save(saveFilePath);
                        // Attach CRS to document Obj
                        var fileInfo = new FileInfo(saveFilePath);
                        if (fileInfo.Exists && this.attachFileService.GetAllDocId(docObj.ID).All(t => t.TypeId != 3))
                        {
                            var attachFile = new DQREDocumentAttachFile()
                            {
                                ID = Guid.NewGuid(),
                                ProjectDocumentId = docObj.ID,
                                FileName = filename,
                                Extension = ".xlsm",
                                FilePath = "/DocumentLibrary/MarkupFile/" + filename,
                                ExtensionIcon = "~/images/excelfile.png",
                                FileSize = (double)fileInfo.Length / 1024,
                                TypeId = 4,
                                TypeName = "Comment Sheet",
                                CreatedBy = UserSession.Current.User.Id,
                                CreatedByName = UserSession.Current.User.UserNameWithFullName,
                                CreatedDate = DateTime.Now
                            };

                            this.attachFileService.Insert(attachFile);
                        }
                        // -------------------------------------------------------------------------------------------------

                        this.Download_File(saveFilePath);
                    }
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
    }
}