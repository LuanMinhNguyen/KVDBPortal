// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.UI;
using Aspose.Cells;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Data.Entities;
using EDMs.Web.Utilities;
using EDMs.Web.Utilities.Sessions;
using iTextSharp.awt.geom;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.MarkupTool
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ChangeRequestAttachDocList : Page
    {
        private readonly ChangeRequestAttachFileService changeRequestAttachFileService;

        private readonly ChangeRequestService changeRequestService;

        private readonly ChangeRequestReviewResultCodeService changeRequestReviewResultCodeService;

        private readonly ChangeRequestDocFileService changeRequestDocFileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeRequestAttachDocList"/> class.
        /// </summary>
        public ChangeRequestAttachDocList()
        {
            this.changeRequestAttachFileService = new ChangeRequestAttachFileService();
            this.changeRequestDocFileService = new ChangeRequestDocFileService();
            this.changeRequestReviewResultCodeService = new ChangeRequestReviewResultCodeService();
            this.changeRequestService = new ChangeRequestService();
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
                this.lblUserId.Value = UserSession.Current.User.Id.ToString();
                if (!string.IsNullOrEmpty(this.Request.QueryString["actionType"]))
                {
                    this.btnConsolidate.Visible = this.Request.QueryString["actionType"] == "3";
                    this.cbConsolidateFile.Checked = this.Request.QueryString["actionType"] == "3";
                }
                //document code
                //var changeRequestReviewResult = this.changeRequestReviewResultCodeService.GetAll();
                //changeRequestReviewResult.Insert(0, new ChangeRequestReviewResultCode() {ID = 0});

                //this.ddlReviewStatus.DataSource = changeRequestReviewResult.OrderBy(t => t.Code);
                //this.ddlReviewStatus.DataTextField = "Code";
                //this.ddlReviewStatus.DataValueField = "ID";
                //this.ddlReviewStatus.DataBind();

                if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
                {
                    var objId = new Guid(this.Request.QueryString["objId"]);
                    //var changeRequestObj = this.changeRequestService.GetById(objId);
                    //if (changeRequestObj != null && changeRequestObj.PECC2ReviewResultId != null)
                    //{
                    //    this.ddlReviewStatus.SelectedValue =
                    //        changeRequestObj.PECC2ReviewResultId.GetValueOrDefault().ToString();
                    //}

                    if (this.cbConsolidateFile.Checked)
                    {
                        var fileconsolidate = this.changeRequestAttachFileService.GetByUserUploadConsolidateFile(
                                UserSession.Current.User.Id, objId);
                        if (fileconsolidate == null)
                        {
                            GenerateConsolidate();
                        }
                    }
                }


            }
        }


        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                var objId = new Guid(this.Request.QueryString["objId"]);
                this.grdDocument.DataSource = new List<ChangeRequestDocFile>() { this.changeRequestDocFileService.GetById(objId)};
            }
            else
            {
                this.grdDocument.DataSource = new List<ChangeRequestDocFile>();
            }
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }

        protected void grdAttachMarkupCommentFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                var objId = new Guid(this.Request.QueryString["objId"]);
                var attachList = this.changeRequestAttachFileService.GetByChangeRequestDocFile(objId).Where(t => !t.IsOnlyMarkupPage.GetValueOrDefault() && t.TypeId != 1).OrderBy(t => t.CreatedDate);
                this.grdAttachMarkupCommentFile.DataSource = attachList;
                
            }
            else
            {
                this.grdAttachMarkupCommentFile.DataSource = new List<ChangeRequestAttachFile>();
            }

            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                var objId = new Guid(this.Request.QueryString["objId"]);
                var crDocFileObj = this.changeRequestDocFileService.GetById(objId);
                var changeRequestObj = this.changeRequestService.GetById(crDocFileObj.ChangeRequestId.GetValueOrDefault());

                var flag = false;
                var targetFolder = "../.." + changeRequestObj.StoreFolderPath;
                var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/"
                    ? string.Empty
                    : HostingEnvironment.ApplicationVirtualPath)
                                   + changeRequestObj.StoreFolderPath;

                var listUpload = docuploader.UploadedFiles;
                if (listUpload.Count > 0)
                {
                    foreach (UploadedFile docFile in listUpload)
                    {
                        var isUpdateFileConsolidate = false;
                        var consolidateFile = new ChangeRequestAttachFile();
                        var crsFile = new ChangeRequestAttachFile();
                        var docFileName = docFile.FileName;
                        var fileExt = docFile.GetExtension();

                        var serverDocFileName = docFileName;

                        if (this.cbMarkupFile.Checked)
                        {
                            serverDocFileName = docFile.GetNameWithoutExtension() + "_Markup_" +
                                                UserSession.Current.User.Username + fileExt;
                        }
                        else if (this.cbConsolidateFile.Checked)
                        {
                            serverDocFileName = docFile.GetNameWithoutExtension() + "_ConsolidateMarkup_" +
                                                UserSession.Current.User.Username + fileExt;
                            consolidateFile = this.changeRequestAttachFileService.GetByUserUploadConsolidateFile(UserSession.Current.User.Id, objId);
                            if (consolidateFile != null)
                            {
                                serverDocFileName = consolidateFile.FileName;
                                isUpdateFileConsolidate = true;
                                if (File.Exists(consolidateFile.FilePath))
                                {
                                    File.Delete(consolidateFile.FilePath);
                                }
                            }
                        }

                        // Path file to save on server disc
                        var saveFilePath = Path.Combine(Server.MapPath(targetFolder), serverDocFileName);

                        // Path file to download from server
                        var serverFilePath = serverFolder + "/" + serverDocFileName;

                        docFile.SaveAs(saveFilePath, true);

                        var attachFile = new ChangeRequestAttachFile()
                        {
                            ID = Guid.NewGuid(),
                            ChangeRequestId = changeRequestObj.ID,
                            ChangeRequestDocFileId = objId,
                            FileName = serverDocFileName,
                            Extension = fileExt,
                            FilePath = serverFilePath,
                            ExtensionIcon =
                                Utility.FileIcon.ContainsKey(fileExt.ToLower())
                                    ? Utility.FileIcon[fileExt.ToLower()]
                                    : "~/images/otherfile.png",
                            FileSize = (double) docFile.ContentLength/1024,
                            TypeId = this.cbMarkupFile.Checked
                                ? 2
                                : 3,
                            TypeName = this.cbMarkupFile.Checked
                                ? "Document Markup/Comment Files"
                                : "Document Markup/Comment Consolidate Files",
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedByName = UserSession.Current.User.UserNameWithFullName,
                            CreatedDate = DateTime.Now
                        };

                        if (isUpdateFileConsolidate)
                        {
                            consolidateFile.FilePath = serverFilePath;
                            consolidateFile.FileSize = (double) docFile.ContentLength/1024;
                            consolidateFile.CreatedDate = DateTime.Now;
                            this.changeRequestAttachFileService.Update(consolidateFile);

                        }
                        else
                        {
                            this.changeRequestAttachFileService.Insert(attachFile);
                        }
                        // -------------------------------------------------------------------------------------------------
                    }
                }

                changeRequestObj.IsHasAttachFile = this.changeRequestAttachFileService.GetByChangeRequest(objId).Any();
                this.changeRequestService.Update(changeRequestObj);
            }

            this.docuploader.UploadedFiles.Clear();
            this.grdAttachMarkupCommentFile.Rebind();
        }

        protected void grdAttachMarkupCommentFile_OnDeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            Guid objId = new Guid(item.GetDataKeyValue("ID").ToString());
            var fileconsolidate = this.changeRequestAttachFileService.GetById(objId);
            if (fileconsolidate != null)
            {
                if (File.Exists(Server.MapPath(@"../.." + fileconsolidate.FilePath))) { File.Delete(Server.MapPath(@"../.." + fileconsolidate.FilePath)); }
                
            }

            this.changeRequestAttachFileService.Delete(objId);
            this.grdAttachMarkupCommentFile.Rebind();
        }

        protected void btnConsolidate_OnClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                var objId = new Guid(this.Request.QueryString["objId"]);
                
                var fileconsolidate = this.changeRequestAttachFileService.GetByUserUploadConsolidateFile(UserSession.Current.User.Id, objId);
                if (fileconsolidate != null)
                {
                    if (File.Exists(Server.MapPath(@"../.." + fileconsolidate.FilePath)))
                    {
                        File.Delete(Server.MapPath(@"../.." + fileconsolidate.FilePath));
                    }
                     this.changeRequestAttachFileService.Delete(fileconsolidate);
                }

                GenerateConsolidate();
            }
        }


        private void GenerateConsolidate()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                Guid objId = new Guid(this.Request.QueryString["objId"]);
                var crDocFileObj = this.changeRequestDocFileService.GetById(objId);
                var changeRequestObj = this.changeRequestService.GetById(crDocFileObj.ChangeRequestId.GetValueOrDefault());

                var flag = false;
                var targetFolder = "../.." + changeRequestObj.StoreFolderPath;

                var markupfileList = this.changeRequestAttachFileService.GetByChangeRequestDocFile(objId).Where(t => t.TypeId == 2 && (t.Extension.ToLower() == "pdf" || t.Extension.ToLower() == ".pdf")).ToList();
                if (markupfileList.Count > 0)
                {
                    var consolidateFile = new List<string>();

                    // Consolicate markup files
                    var originalFilePath = Server.MapPath("../.." + crDocFileObj.FilePath);
                    // Split original file to files have same page size
                    var splitFiles = this.SplitSamePageSize(originalFilePath, targetFolder);// + "/Temp");
                    // -------------------------------------------------------------------------------------
                    var tempPageCount = 0;
                    var tempFileCount = 0;

                    // Process to consolidate on split file
                    foreach (var splitFile in splitFiles)
                    {

                        tempFileCount += 1;
                        var output = new MemoryStream();
                        var splitReader = new PdfReader(splitFile);
                        var document = new iTextSharp.text.Document(splitReader.GetPageSize(1));
                        var writer = PdfWriter.GetInstance(document, output);
                        writer.CloseStream = false;
                        document.Open();
                        var pageCount = splitReader.NumberOfPages;
                        for (var i = 0; i < pageCount; i++)
                        {
                            tempPageCount += 1;
                            var imp = writer.GetImportedPage(splitReader, (i + 1));
                            writer.DirectContent.AddTemplate(imp, new AffineTransform());

                            foreach (var markupFile in markupfileList)
                            {
                                var markupFilePath = Server.MapPath("../.." + markupFile.FilePath);
                                var markupReader = new PdfReader(markupFilePath);
                                var annots = markupReader.GetPageN(tempPageCount).GetAsArray(PdfName.ANNOTS);

                                if (annots != null && annots.Size != 0)
                                {
                                    foreach (var a in annots)
                                    {
                                        var newannot = new PdfAnnotation(writer, new Rectangle(0, 0));
                                        var annotObj = (PdfDictionary)PdfReader.GetPdfObject(a);
                                        newannot.PutAll(annotObj);
                                        writer.AddAnnotation(newannot);
                                    }
                                }
                            }

                            document.NewPage();
                        }

                        document.Close();
                        splitReader.Close();
                        output.Position = 0;
                        // ---------------------------------------------------------------------------------------------------------------

                        //Path file to save on server disc
                        var consolidateSplitSaveFilePath = Path.Combine(Server.MapPath(targetFolder), Guid.NewGuid().ToString()) + "_Split" + tempFileCount + "Consolidate.pdf"; ;
                        consolidateFile.Add(consolidateSplitSaveFilePath);
                        File.WriteAllBytes(consolidateSplitSaveFilePath, output.ToArray());
                    }

                    // Save Consolidate markup file
                    var consolidateFileName = changeRequestObj.Number + "_ConsolidateMarkup_" + UserSession.Current.User.Username + ".pdf";
                    // Path file to save on server disc
                    var consolidateSaveFilePath = Path.Combine(Server.MapPath(targetFolder), consolidateFileName);
                    // Merge consolidate on split files
                    this.MergePDF(consolidateFile, consolidateSaveFilePath);
                    // ------------------------------------------------------------------------------------

                    var fileInfo = new FileInfo(consolidateSaveFilePath);
                    if (fileInfo.Exists)
                    {
                        var attachFile = new ChangeRequestAttachFile()
                        {
                            ID = Guid.NewGuid(),
                            ChangeRequestId = changeRequestObj.ID,
                            ChangeRequestDocFileId = objId,
                            FileName = consolidateFileName,
                            Extension = "pdf",
                            FilePath = changeRequestObj.StoreFolderPath + "/" + consolidateFileName,
                            ExtensionIcon = "~/images/pdffile.png",
                            FileSize = (double)fileInfo.Length / 1024,
                            TypeId = 3,
                            TypeName = "Document Markup/Comment Consolidate Files",
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedByName = UserSession.Current.User.UserNameWithFullName,
                            CreatedDate = DateTime.Now
                        };
                        
                            this.changeRequestAttachFileService.Insert(attachFile);
                        
                        
                        this.grdAttachMarkupCommentFile.Rebind();
                    }

                    // -----------------------------------------------------------------------------------------------------------------
                }

            }
        }
        private List<string> SplitSamePageSize(string filePath, string targetFolder)
        {
            var splitFile = new List<string>();

            //var originalFilePath = @"D:\01. Desktop\011. EDMS BSR - PECC2\Doc Test\15001-000-CN-0008-001-B.pdf";
            ////var originalFilePath = @"D:\01. Desktop\011. EDMS BSR - PECC2\Doc Test\15001-000-PP-602-B5.PDF";
            var originalReader = new PdfReader(filePath);

            // Consolicate markup files
            var output = new MemoryStream();
            var document = new iTextSharp.text.Document();
            var writer = new PdfCopy(document, output);
            writer.CloseStream = false;
            document.Open();
            var count = 0;
            var originalPageCount = originalReader.NumberOfPages;
            if (originalPageCount == 1)
            {
                int i = originalPageCount;
                var currentPageSize = originalReader.GetPageSize(i);
                    var imp = writer.GetImportedPage(originalReader, (i));
                    writer.AddPage(imp);

                    if (i == originalReader.NumberOfPages)
                    {
                        count += 1;
                        document.Close();
                        originalReader.Close();

                        var filepath = Path.Combine(Server.MapPath(targetFolder), Guid.NewGuid().ToString()) + "_Split" + count + ".pdf";
                        splitFile.Add(filepath);
                        File.WriteAllBytes(filepath, output.ToArray());
                    }

            }
            else
            {
                for (var i = 2; i <= originalPageCount; i++)
                                {
                                    var currentPageSize = originalReader.GetPageSize(i);
                                    var prevPageSize = originalReader.GetPageSize(i - 1);
                                    if (i == 2)
                                    {
                                        var imp = writer.GetImportedPage(originalReader, (i - 1));
                                        writer.AddPage(imp);
                                    }

                                    if (!Equals(currentPageSize, prevPageSize))
                                    {
                                        count += 1;
                                        document.Close();
                                        var filepath = Path.Combine(Server.MapPath(targetFolder), Guid.NewGuid().ToString()) + "_Split" + count + ".pdf";
                                        splitFile.Add(filepath);
                                        File.WriteAllBytes(filepath, output.ToArray());

                                        output = new MemoryStream();
                                        document = new iTextSharp.text.Document();
                                        writer = new PdfCopy(document, output);
                                        writer.CloseStream = false;
                                        document.Open();

                                        var imp = writer.GetImportedPage(originalReader, (i));
                                        writer.AddPage(imp);
                                    }
                                    else
                                    {
                                        var imp = writer.GetImportedPage(originalReader, (i));
                                        writer.AddPage(imp);

                                        if (i == originalReader.NumberOfPages)
                                        {
                                            count += 1;
                                            document.Close();
                                            originalReader.Close();

                                            var filepath = Path.Combine(Server.MapPath(targetFolder), Guid.NewGuid().ToString()) + "_Split" + count + ".pdf";
                                            splitFile.Add(filepath);
                                            File.WriteAllBytes(filepath, output.ToArray());
                                        }
                                    }
                                }
            }
           

            return splitFile;
        }
        private void MergePDF(List<string> fileList, string outputFile)
        {
            var output = new MemoryStream();
            var document = new iTextSharp.text.Document();
            var writer = new PdfCopy(document, output);
            writer.CloseStream = false;
            document.Open();

            foreach (var fileName in fileList)
            {
                var pdfFile = new PdfReader(fileName);
                for (int i = 1; i <= pdfFile.NumberOfPages; i++)
                {
                    var imp = writer.GetImportedPage(pdfFile, (i));
                    writer.AddPage(imp);
                }

                pdfFile.Close();
            }

            document.Close();

            var filepath = outputFile;
            File.WriteAllBytes(filepath, output.ToArray());
        }

        protected void btnExportCRS_OnClick(object sender, EventArgs e)
        {
            
        }
    }
}