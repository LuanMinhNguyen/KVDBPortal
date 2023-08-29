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
using System.Web.UI.WebControls;
using Aspose.Cells;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Data.Entities;
using EDMs.Web.Controls.Document;
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
    public partial class AttachDocList : Page
    {
        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly PECC2DocumentsService pecc2DocumentService;

        private readonly PECC2DocumentAttachFileService attachFileService;

        private readonly DocumentCodeServices documentCodeServices;

        private readonly ChangeRequestAttachFileService _ChangeRepuestAttachFileService;
        private readonly NCR_SIAttachFileService _NCR_SIAttachFileService;

        private readonly PECC2TransmittalService pecc2TransmittalService;

        private readonly DocumentCodeServices documnetCodeSErvie;

        private readonly PECC2DocumentsService PECC2DocumentService;

        private readonly ChangeRequestAttachFileService changeRequestAttachFileService;

        private readonly ChangeRequestReviewResultCodeService changeRequestReviewResultCodeService;

        private readonly ChangeRequestService changeRequestService;
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public AttachDocList()
        {
            this.pecc2DocumentService = new  PECC2DocumentsService();
            this.attachFileService = new   PECC2DocumentAttachFileService();
            this.documentCodeServices = new DocumentCodeServices();
            this._ChangeRepuestAttachFileService = new ChangeRequestAttachFileService();
            this._NCR_SIAttachFileService = new NCR_SIAttachFileService();
            this.pecc2TransmittalService = new PECC2TransmittalService();
            this.documnetCodeSErvie = new DocumentCodeServices();
            this.PECC2DocumentService = new PECC2DocumentsService();
            this.changeRequestAttachFileService = new ChangeRequestAttachFileService();
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
                this.LbIsHaveFileConsolidate.Value = "false";
                if (!string.IsNullOrEmpty(this.Request.QueryString["actionType"]))
                {
                    this.cbConsolidateFile.Enabled = this.Request.QueryString["actionType"] == "3"? true: false;
                    this.cbMarkupFile.Enabled = this.Request.QueryString["actionType"] == "2"? true: false;
                    this.btnConsolidate.Enabled = this.Request.QueryString["actionType"] == "3" || this.Request.QueryString["actionType"] == "4";
                    this.cbConsolidateFile.Checked = this.Request.QueryString["actionType"] == "3" || this.Request.QueryString["actionType"] == "4";
                    this.cbMarkupFile.Checked = this.Request.QueryString["actionType"] == "2";
                    if (!string.IsNullOrEmpty(this.Request.QueryString["Iscancreatetrans"])  && this.Request.QueryString["Iscancreatetrans"]=="True")
                    {
                        this.cbConsolidateFile.Checked = true;
                        this.cbConsolidateFile.Enabled = true;
                        this.cbMarkupFile.Enabled = false;
                        this.cbMarkupFile.Checked = false;

                        this.btnConsolidate.Enabled = true;
                    }

                }
                //document code
                var documentcodelist = this.documnetCodeSErvie.GetAllReviewStatus();
                documentcodelist.Insert(0, new DocumentCode() { ID = 0, Code = string.Empty });
                this.ddlDocReviewStatus.DataSource = documentcodelist.Where(t => ! t.FullName.Contains("ReFC")).ToList(); 
                this.ddlDocReviewStatus.DataTextField = "FullName";
                this.ddlDocReviewStatus.DataValueField = "ID";
                this.ddlDocReviewStatus.DataBind();
                // ---------------------------------------------------------------------------------

                this.ddlDocReviewStatus1.DataSource = documentcodelist.Where(t=> t.ID==0 || t.FullName.Contains("ReFC"));
                this.ddlDocReviewStatus1.DataTextField = "FullName";
                this.ddlDocReviewStatus1.DataValueField = "ID";
                this.ddlDocReviewStatus1.DataBind();

                // Bind Change Request Review Code
                var changeRequestReviewcodelist = this.changeRequestReviewResultCodeService.GetAll().Where(t => !t.FullName.Contains("ReFC")).ToList();
                changeRequestReviewcodelist.Insert(0, new ChangeRequestReviewResultCode() { ID = 0, Code = string.Empty ,});
                this.ddlChangeRequestReviewCode.DataSource = changeRequestReviewcodelist;
                this.ddlChangeRequestReviewCode.DataTextField = "FullName";
                this.ddlChangeRequestReviewCode.DataValueField = "ID";
                this.ddlChangeRequestReviewCode.DataBind();
                // ---------------------------------------------------------------------------------

                // Set view dll review code
                this.DevChangeRequestReviewCode.Visible = Convert.ToBoolean(this.Request.QueryString["IsChangeRequest"]);
                this.DevCommentCode.Visible = !Convert.ToBoolean(this.Request.QueryString["IsChangeRequest"]);
                // -----------------------------------------------------------------------------------------------------------

                if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
                {
                    var objId = new Guid(this.Request.QueryString["docId"]);
                    if (this.Request.QueryString["IsChangeRequest"] == "True")
                    {
                        var changeRequestObj = this.changeRequestService.GetById(objId);
                        if (changeRequestObj != null && changeRequestObj.ReviewResultId != null)
                        {
                            this.ddlChangeRequestReviewCode.SelectedValue = changeRequestObj.ReviewResultId.GetValueOrDefault().ToString();
                        }
                    }
                    else
                    {
                        var docObj = this.PECC2DocumentService.GetById(objId);
                        if (docObj != null && docObj.DocReviewStatusId != null)
                        {
                            this.ddlDocReviewStatus.SelectedValue = docObj.DocReviewStatusId.GetValueOrDefault().ToString();
                            this.ddlDocReviewStatus1.SelectedValue = docObj.DocReviewStatusId2.GetValueOrDefault().ToString();
                        }

                        try
                        {
                            var convertoint = Convert.ToInt32(docObj.Revision);
                            this.divcomment1.Visible = true;
                        }
                        catch { }
                    }

                  //= !Convert.ToBoolean(this.Request.QueryString["IsChangeRequest"]);

                    if (this.cbConsolidateFile.Checked)
                    {
                        if (this.Request.QueryString["IsChangeRequest"] == "True")
                        {
                            var fileconsolidate = this.changeRequestAttachFileService.GetByUserUploadConsolidateFile(UserSession.Current.User.Id, objId);
                            if (fileconsolidate == null)
                            {
                                this.GenerateChangeRequestConsolidate("_Auto");
                            }

                            this.LbIsHaveFileConsolidate.Value = this.changeRequestAttachFileService.GetByUserUploadConsolidateFile(UserSession.Current.User.Id, objId) != null ? "true" : "false";
                            
                        }
                        else
                        {
                            var fileconsolidate = this.attachFileService.GetByConsolidateFile(objId);
                            if (fileconsolidate == null)
                            {
                                this.GenerateDocumentConsolidate("_Auto");
                            }

                            this.LbIsHaveFileConsolidate.Value = this.attachFileService.GetByConsolidateFile( objId) != null ? "true" : "false";
                            this.btnConsolidate.Visible= this.attachFileService.GetByConsolidateFile(objId) != null ? false : true;
                        }

                        
                    }

                }
            }
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                var docId = new Guid(this.Request.QueryString["docId"]);
                if (this.Request.QueryString["IsChangeRequest"] == "True")
                {
                    var attachList = this.changeRequestAttachFileService.GetAllByChangeRequest(docId).Where(t => t.TypeId == 1 && t.Extension.ToLower().Contains("pdf"));
                    this.grdDocument.DataSource = attachList;
                }
                else
                {
                    var attachList = this.attachFileService.GetAllDocId(docId).Where(t => t.TypeId == 1 && t.Extension.ToLower().Contains("pdf"));
                    this.grdDocument.DataSource = attachList;
                }
                
            }
            else
            {
                this.grdDocument.DataSource = new List<PECC2TransmittalAttachFileService>();
            }
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument== "GenerateConsolidate")
            {
                GenerateFileConsolidate();
                this.grdDocument.Rebind();
            }
            else if (e.Argument == "UploadFile")
            {
                ClientClickUpload();
                this.grdDocument.Rebind();
            }
            else if (e.Argument.Contains("grdAttachMarkupCommentFileDelete"))
            {
                Guid docId = new Guid(e.Argument.Split('_')[1]);
                var fileconsolidate = this.attachFileService.GetById(docId);
                if (fileconsolidate != null)
                {
                    if (File.Exists(Server.MapPath(@"../.." + fileconsolidate.FilePath))) { File.Delete(Server.MapPath(@"../.." + fileconsolidate.FilePath)); }
                    this.attachFileService.Delete(fileconsolidate);
                }
                this.LbIsHaveFileConsolidate.Value = this.attachFileService.GetByConsolidateFile( docId) != null ? "true" : "false";
                this.grdAttachMarkupCommentFile.Rebind();
            }
        }

        protected void grdAttachMarkupCommentFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                var objId = new Guid(this.Request.QueryString["docId"]);
                if (this.Request.QueryString["IsChangeRequest"] == "True")
                {
                    var attachList = this.changeRequestAttachFileService.GetAllByChangeRequest(objId).Where(t => !t.IsOnlyMarkupPage.GetValueOrDefault() && t.TypeId != 1).OrderBy(t => t.CreatedDate);
                    foreach (var changeRequestAttachFile in attachList)
                    {
                        changeRequestAttachFile.IsCanDelete = UserSession.Current.User.Id == changeRequestAttachFile.CreatedBy;
                    }
                    this.grdAttachMarkupCommentFile.DataSource = attachList;
                }
                else
                {
                    var attachList = this.attachFileService.GetAllDocId(objId).Where(t => !t.IsOnlyMarkupPage.GetValueOrDefault() && t.TypeId != 1).OrderBy(t => t.CreatedDate);
                    foreach (var pecc2DocumentAttachFile in attachList)
                    {
                        pecc2DocumentAttachFile.IsCanDelete = UserSession.Current.User.Id == pecc2DocumentAttachFile.CreatedBy;
                    }
                    this.grdAttachMarkupCommentFile.DataSource = attachList;
                }
                this.btnExportCRS.Visible = false;
            }
            else
            {
                this.grdAttachMarkupCommentFile.DataSource = new List<PECC2TransmittalAttachFileService>();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                if (this.Request.QueryString["IsChangeRequest"] == "True")
                {
                    this.AttachForChangeRequestObj();
                }
                else
                {
                    this.AttachForDocumentObj();
                }
            }

            this.docuploader.UploadedFiles.Clear();
            this.grdAttachMarkupCommentFile.Rebind();
        }

        private void AttachForChangeRequestObj()
        {
            Guid changeRequestId = new Guid(this.Request.QueryString["docId"]);
            var changeRequestObj = this.changeRequestService.GetById(changeRequestId);

            var flag = false;
            var targetFolder = "../../DocumentLibrary/MarkupFile";
            var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                + "/DocumentLibrary/MarkupFile";
            var listUpload = docuploader.UploadedFiles;

            if (listUpload.Count > 0)
            {
                foreach (UploadedFile docFile in listUpload)
                {
                    var IsUpdateFileConsolidate = false;
                    var fileconsolidate = new ChangeRequestAttachFile();
                    var docFileName = changeRequestObj.Number + "_" + changeRequestObj.Revision;
                    var fileExt = docFile.GetExtension();

                    var serverDocFileName = changeRequestObj.Number + "_" + changeRequestObj.Revision;

                    if (this.cbMarkupFile.Checked)
                    {
                        serverDocFileName = docFileName + "_Markup_" + UserSession.Current.User.Username + fileExt;
                        var listmarkup = this.changeRequestAttachFileService.GetByUserUploadMarkupFile(UserSession.Current.User.Id, changeRequestId);
                        if (listmarkup.Count > 0)
                        {
                            serverDocFileName = docFileName + "_Markup_" + listmarkup.Count + "_" + UserSession.Current.User.Username + fileExt;
                        }
                    }
                    else
                    {
                        serverDocFileName = docFileName + "_ConsolidateMarkup_" + UserSession.Current.User.Username + fileExt;
                        fileconsolidate = this.changeRequestAttachFileService.GetByUserUploadConsolidateFile(UserSession.Current.User.Id, changeRequestId);
                        if (fileconsolidate != null)
                        {
                            serverDocFileName = fileconsolidate.FileName.Replace("_Auto", string.Empty); ;
                            IsUpdateFileConsolidate = true;
                            //if (File.Exists(Server.MapPath(@"../.." + fileconsolidate.FilePath))) { File.Delete(Server.MapPath(@"../.." + fileconsolidate.FilePath)); }
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
                        ChangeRequestId = changeRequestId,
                        FileName = serverDocFileName,
                        Extension = fileExt,
                        FilePath = serverFilePath,
                        ExtensionIcon = Utility.FileIcon.ContainsKey(fileExt.ToLower()) ? Utility.FileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
                        FileSize = (double)docFile.ContentLength / 1024,
                        TypeId = this.cbMarkupFile.Checked
                                        ? 2
                                        : this.cbConsolidateFile.Checked
                                            ? 3
                                            : 4,
                        TypeName = this.cbMarkupFile.Checked
                                        ? "Document Markup/Comment Files"
                                        : this.cbConsolidateFile.Checked
                                            ? "Document Markup/Comment Consolidate Files"
                                            : "Comment Response Sheet (CRS)",
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.UserNameWithFullName,
                        CreatedDate = DateTime.Now
                    };
                    if (IsUpdateFileConsolidate)
                    {
                        fileconsolidate.FilePath = serverFilePath;
                        fileconsolidate.FileName = serverDocFileName;
                        fileconsolidate.FileSize = (double)docFile.ContentLength / 1024;
                        fileconsolidate.CreatedDate = DateTime.Now;
                        this.changeRequestAttachFileService.Update(fileconsolidate);

                    }
                    else
                    {
                        this.changeRequestAttachFileService.Insert(attachFile);
                    }
                    // -------------------------------------------------------------------------------------------------
                }
            }

            var reviewStatusObj = this.changeRequestReviewResultCodeService.GetById(Convert.ToInt32(this.ddlChangeRequestReviewCode.SelectedValue));
            if (reviewStatusObj != null)
            {
                changeRequestObj.ReviewResultId = reviewStatusObj.ID;
                changeRequestObj.ReviewResultName = reviewStatusObj.Code;
            }

            changeRequestObj.IsHasAttachFile = this.changeRequestAttachFileService.GetAllByChangeRequest(changeRequestId).Any();
            this.changeRequestService.Update(changeRequestObj);
        }

        private void AttachForDocumentObj()
        {
            Guid docId = new Guid(this.Request.QueryString["docId"]);
            var docObj = this.pecc2DocumentService.GetById(docId);

            var flag = false;
            var targetFolder = "../../DocumentLibrary/MarkupFile";
            var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                + "/DocumentLibrary/MarkupFile";


            var listUpload = docuploader.UploadedFiles;



            if (listUpload.Count > 0)
            {
                foreach (UploadedFile docFile in listUpload)
                {
                    var IsUpdateFileConsolidate = false;
                    PECC2DocumentAttachFile fileconsolidate = new PECC2DocumentAttachFile();
                    var docFileName = docObj.DocNo + "_" + docObj.Revision;
                    var fileExt = docFile.GetExtension();

                    var serverDocFileName = docObj.DocNo + "_" + docObj.Revision;

                    if (this.cbMarkupFile.Checked)
                    {
                        serverDocFileName = docFileName + "_Markup_" + UserSession.Current.User.Username + fileExt;
                        var listmarkup = this.attachFileService.GetByUserUploadMarkupFile(UserSession.Current.User.Id, docId);
                        if (listmarkup.Count > 0)
                        {
                            serverDocFileName = docFileName + "_Markup_" + listmarkup.Count + "_" + UserSession.Current.User.Username + fileExt;
                        }
                    }
                    else
                    {
                        serverDocFileName = docFileName + "_ConsolidateMarkup_" + UserSession.Current.User.Username + fileExt;
                        fileconsolidate = this.attachFileService.GetByUserUploadConsolidateFile(UserSession.Current.User.Id, docId);
                        if (fileconsolidate != null)
                        {
                            try
                            {
                                if (File.Exists(Server.MapPath(@"../.." + fileconsolidate.FilePath))) { File.Delete(Server.MapPath(@"../.." + fileconsolidate.FilePath)); }
                            }catch { }
                            serverDocFileName = fileconsolidate.FileName.Replace("_Auto", string.Empty); ;
                            IsUpdateFileConsolidate = true;
                            
                        }
                    }

                    // Path file to save on server disc
                    var saveFilePath = Path.Combine(Server.MapPath(targetFolder), serverDocFileName);

                    // Path file to download from server
                    var serverFilePath = serverFolder + "/" + serverDocFileName;

                    docFile.SaveAs(saveFilePath, true);

                    var attachFile = new PECC2DocumentAttachFile()
                    {
                        ID = Guid.NewGuid(),
                        ProjectDocumentId = docId,
                        FileName = serverDocFileName,
                        Extension = fileExt,
                        FilePath = serverFilePath,
                        ExtensionIcon = Utility.FileIcon.ContainsKey(fileExt.ToLower()) ? Utility.FileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
                        FileSize = (double)docFile.ContentLength / 1024,
                        TypeId = this.cbMarkupFile.Checked
                                        ? 2
                                        : this.cbConsolidateFile.Checked
                                            ? 3
                                            : 4,
                        TypeName = this.cbMarkupFile.Checked
                                        ? "Document Markup/Comment Files"
                                        : this.cbConsolidateFile.Checked
                                            ? "Document Markup/Comment Consolidate Files"
                                            : "Comment Response Sheet (CRS)",
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.UserNameWithFullName,
                        CreatedDate = DateTime.Now
                    };
                    if (IsUpdateFileConsolidate)
                    {
                        fileconsolidate.FilePath = serverFilePath;
                        fileconsolidate.FileName = serverDocFileName;
                        fileconsolidate.FileSize = (double)docFile.ContentLength / 1024;
                        fileconsolidate.CreatedDate = DateTime.Now;
                        this.attachFileService.Update(fileconsolidate);

                    }
                    else
                    {
                        this.attachFileService.Insert(attachFile);
                    }


                    //if (this.cbCRS.Checked)
                    //{
                    //    if (File.Exists(saveFilePath))
                    //    {
                    //        var workbook = new Workbook(saveFilePath);
                    //        var dataSheet = workbook.Worksheets[0];
                    //        var reviewStatusCode = dataSheet.Cells["E7"].Value.ToString()
                    //                .Replace("Document Review Status: ", string.Empty)
                    //                .Replace(Environment.NewLine, string.Empty)
                    //                .Trim();
                    //        var reviewStatusObj = this.documentCodeServices.GetByCode(reviewStatusCode);
                    //        if (reviewStatusObj != null)
                    //        {
                    //            docObj.DocReviewStatusId = reviewStatusObj.ID;
                    //            docObj.DocReviewStatusCode = reviewStatusObj.Code;
                    //        }
                    //    }
                    //}

                    /*
                    // Extract only comment page
                    if (this.cbConsolidateFile.Checked && fileExt.ToLower() == ".pdf")
                    {
                        var outputOnlyMarkupPageFileName = docFile.GetNameWithoutExtension() + "_ConsolidateOnlyMarkupPages_" + UserSession.Current.User.Username + fileExt;
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
                            var onlyMarkupPageAttachFile = new PECC2DocumentAttachFile()
                            {
                                ID = Guid.NewGuid(),
                                ProjectDocumentId = docId,
                                FileName = outputOnlyMarkupPageFileName,
                                Extension = fileExt,
                                FilePath = "/DocumentLibrary/MarkupFile/" + outputOnlyMarkupPageFileName,
                                ExtensionIcon = Utility.FileIcon.ContainsKey(fileExt.ToLower()) ? Utility.FileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
                                FileSize = (double)fileInfo.Length / 1024,
                                TypeId = this.cbMarkupFile.Checked ? 2 : 3,
                                TypeName = this.cbMarkupFile.Checked ? "Document Markup/Comment Files" : "Document Markup/Comment Consolidate Files",
                                CreatedBy = UserSession.Current.User.Id,
                                CreatedByName = UserSession.Current.User.UserNameWithFullName,
                                CreatedDate = DateTime.Now,
                                IsOnlyMarkupPage = true
                            };

                            this.attachFileService.Insert(onlyMarkupPageAttachFile);
                        }
                        // -------------------------------------------------------------------------------------------------------------------
                    }
                    */
                    // -------------------------------------------------------------------------------------------------


                }

            }

            var reviewStatusObj = this.documentCodeServices.GetById(Convert.ToInt32(this.ddlDocReviewStatus.SelectedValue));
            if (reviewStatusObj != null)
            {
                docObj.DocReviewStatusId = reviewStatusObj.ID;
                docObj.DocReviewStatusCode = reviewStatusObj.Code;
            }
            if(UserSession.Current.User.Role.ContractorId == 2)
            {
                docObj.IsOwnerComment = DateTime.Now;
            }
            if (UserSession.Current.User.Role.ContractorId == 1)
            {
                docObj.IsConsultantComment = DateTime.Now;
            }

            docObj.IsHasAttachFile = this.attachFileService.GetAllDocId(docId).Any();
            this.pecc2DocumentService.Update(docObj);
        }

        private void ClientClickUpload()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                Guid docId = new Guid(this.Request.QueryString["docId"]);
                var docObj = this.pecc2DocumentService.GetById(docId);

                var flag = false;
                var targetFolder = "../../DocumentLibrary/MarkupFile";
                var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + "/DocumentLibrary/MarkupFile";


                var listUpload = docuploader.UploadedFiles;



                if (listUpload.Count > 0)
                {
                    foreach (UploadedFile docFile in listUpload)
                    {
                        var IsUpdateFileConsolidate = false;
                        PECC2DocumentAttachFile fileconsolidate = new PECC2DocumentAttachFile();
                        var docFileName = docObj.DocNo + "_" + docObj.Revision;
                        var fileExt = docFile.GetExtension();

                        var serverDocFileName = docObj.DocNo + "_" + docObj.Revision;

                        if (this.cbMarkupFile.Checked)
                        {
                            serverDocFileName = docFileName + "_Markup_" + UserSession.Current.User.Username + fileExt;
                        }
                        else
                        {
                            serverDocFileName = docFileName + "_ConsolidateMarkup_" + UserSession.Current.User.Username + fileExt;
                            fileconsolidate = this.attachFileService.GetByUserUploadConsolidateFile(UserSession.Current.User.Id, docId);
                            if (fileconsolidate != null)
                            {
                                serverDocFileName = fileconsolidate.FileName.Replace("_Auto", string.Empty); ;
                                IsUpdateFileConsolidate = true;
                                if (File.Exists(Server.MapPath(@"../.." + fileconsolidate.FilePath))) { File.Delete(Server.MapPath(@"../.." + fileconsolidate.FilePath)); }
                            }
                        }

                        // Path file to save on server disc
                        var saveFilePath = Path.Combine(Server.MapPath(targetFolder), serverDocFileName);

                        // Path file to download from server
                        var serverFilePath = serverFolder + "/" + serverDocFileName;

                        docFile.SaveAs(saveFilePath, true);

                        var attachFile = new PECC2DocumentAttachFile()
                        {
                            ID = Guid.NewGuid(),
                            ProjectDocumentId = docId,
                            FileName = serverDocFileName,
                            Extension = fileExt,
                            FilePath = serverFilePath,
                            ExtensionIcon = Utility.FileIcon.ContainsKey(fileExt.ToLower()) ? Utility.FileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
                            FileSize = (double)docFile.ContentLength / 1024,
                            TypeId = this.cbMarkupFile.Checked
                                            ? 2
                                            : this.cbConsolidateFile.Checked
                                                ? 3
                                                : 4,
                            TypeName = this.cbMarkupFile.Checked
                                            ? "Document Markup/Comment Files"
                                            : this.cbConsolidateFile.Checked
                                                ? "Document Markup/Comment Consolidate Files"
                                                : "Comment Response Sheet (CRS)",
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedByName = UserSession.Current.User.UserNameWithFullName,
                            CreatedDate = DateTime.Now
                        };
                        if (IsUpdateFileConsolidate)
                        {
                            fileconsolidate.FilePath = serverFilePath;
                            fileconsolidate.FileName = serverDocFileName;
                            fileconsolidate.FileSize = (double)docFile.ContentLength / 1024;
                            fileconsolidate.CreatedDate = DateTime.Now;
                            this.attachFileService.Update(fileconsolidate);

                        }
                        else
                        {
                            this.attachFileService.Insert(attachFile);
                        }
                    }

                }
                var reviewStatusObj = this.documentCodeServices.GetById(Convert.ToInt32(this.ddlDocReviewStatus.SelectedValue));
                if (reviewStatusObj != null)
                {
                    docObj.DocReviewStatusId = reviewStatusObj.ID;
                    docObj.DocReviewStatusCode = reviewStatusObj.Code;
                }

                docObj.IsHasAttachFile = this.attachFileService.GetAllDocId(docId).Any();
                this.pecc2DocumentService.Update(docObj);
            }

            this.docuploader.UploadedFiles.Clear();
            this.grdAttachMarkupCommentFile.Rebind();
        }
        //protected void grdAttachMarkupCommentFile_OnDeleteCommand(object sender, GridCommandEventArgs e)
        //{
        //    var item = (GridDataItem)e.Item;
        //    Guid docId = new Guid(item.GetDataKeyValue("ID").ToString());
        //    var fileconsolidate = this.attachFileService.GetById(docId);
        //    if (fileconsolidate != null)
        //    {
        //        if (File.Exists(Server.MapPath(@"../.." + fileconsolidate.FilePath))) { File.Delete(Server.MapPath(@"../.." + fileconsolidate.FilePath)); }
        //        this.attachFileService.Delete(fileconsolidate);
        //    }
        //    this.LbIsHaveFileConsolidate.Value = this.attachFileService.GetByUserUploadConsolidateFile(UserSession.Current.User.Id, docId) != null ? "true" : "false";
        //    this.grdAttachMarkupCommentFile.Rebind();
        //}

        protected void btnConsolidate_OnClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                var docId = new Guid(this.Request.QueryString["docId"]);
                if (this.Request.QueryString["IsChangeRequest"] == "True")
                {
                    var fileconsolidate = this.changeRequestAttachFileService.GetByUserUploadConsolidateFile(UserSession.Current.User.Id, docId);
                    if (fileconsolidate != null)
                    {
                        if (File.Exists(Server.MapPath(@"../.." + fileconsolidate.FilePath))) { File.Delete(Server.MapPath(@"../.." + fileconsolidate.FilePath)); }
                        this.changeRequestAttachFileService.Delete(fileconsolidate);
                    }

                    this.GenerateChangeRequestConsolidate(string.Empty);

                }
                else
                {
                    var fileconsolidate = this.attachFileService.GetByUserUploadConsolidateFile(UserSession.Current.User.Id, docId);
                    if (fileconsolidate != null)
                    {
                        if (File.Exists(Server.MapPath(@"../.." + fileconsolidate.FilePath))) { File.Delete(Server.MapPath(@"../.." + fileconsolidate.FilePath)); }
                        this.attachFileService.Delete(fileconsolidate);
                    }

                    this.GenerateDocumentConsolidate(string.Empty);

                }
            }
        }
        private void GenerateFileConsolidate()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                var docId = new Guid(this.Request.QueryString["docId"]);
                if (this.Request.QueryString["IsChangeRequest"] == "True")
                {
                    var fileconsolidate = this.changeRequestAttachFileService.GetByUserUploadConsolidateFile(UserSession.Current.User.Id, docId);
                    if (fileconsolidate != null)
                    {
                        if (File.Exists(Server.MapPath(@"../.." + fileconsolidate.FilePath))) { File.Delete(Server.MapPath(@"../.." + fileconsolidate.FilePath)); }
                        this.changeRequestAttachFileService.Delete(fileconsolidate);
                    }

                    this.GenerateChangeRequestConsolidate(string.Empty);
                }
                else
                {
                    var fileconsolidate = this.attachFileService.GetByUserUploadConsolidateFile(UserSession.Current.User.Id, docId);
                    if (fileconsolidate != null)
                    {
                        if (File.Exists(Server.MapPath(@"../.." + fileconsolidate.FilePath))) { File.Delete(Server.MapPath(@"../.." + fileconsolidate.FilePath)); }
                        this.attachFileService.Delete(fileconsolidate);
                    }

                    this.GenerateDocumentConsolidate(string.Empty);
                }
            }
        }

        private void GenerateDocumentConsolidate( string Auto)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                Guid docId = new Guid(this.Request.QueryString["docId"]);
                var docObj = this.pecc2DocumentService.GetById(docId);
                var flag = false;
                var targetFolder = "../../DocumentLibrary/MarkupFile";
                var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + "/DocumentLibrary/MarkupFile";

                var originalAttachDoc = this.attachFileService.GetAllDocId(docId).FirstOrDefault(t => t.TypeId == 1 && (t.Extension.ToLower() == "pdf" || t.Extension.ToLower() == ".pdf"));

                var markupfileList = this.attachFileService.GetAllDocId(docId).Where(t => t.TypeId == 2 && (t.Extension.ToLower() == "pdf" || t.Extension.ToLower() == ".pdf")).ToList();
                if (originalAttachDoc != null && markupfileList.Count > 0)
                {
                    var consolidateFile = new List<string>();

                    // Consolidate markup files
                    var originalFilePath = Server.MapPath("../.." + originalAttachDoc.FilePath);
                    // Split original file to files have same page size
                    var splitFiles = this.SplitSamePageSize(originalFilePath, targetFolder + "/Temp");
                    // -------------------------------------------------------------------------------------
                    var tempPageCount = 0;
                    var tempFileCount = 0;

                    // Process to consolidate on split file
                    foreach (var splitFile in splitFiles)
                    {
                        tempFileCount += 1;
                        var output = new MemoryStream();
                        var splitReader = new PdfReader(splitFile);
                        var document = new iTextSharp.text.Document(splitReader.GetCropBox(1));
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
                    var consolidateFileName = docObj.DocNo +"_"+docObj.Revision + "_ConsolidateMarkup_" + UserSession.Current.User.Username +  Auto +".pdf";
                    // Path file to save on server disc
                    var consolidateSaveFilePath = Path.Combine(Server.MapPath(targetFolder), consolidateFileName);
                    // Merge consolidate on split files
                    this.MergePDF(consolidateFile, consolidateSaveFilePath);
                    // ------------------------------------------------------------------------------------

                    var fileInfo = new FileInfo(consolidateSaveFilePath);
                    if (fileInfo.Exists)
                    {
                        var attachFile = new PECC2DocumentAttachFile()
                        {
                            ID = Guid.NewGuid(),
                            ProjectDocumentId = docId,
                            FileName = consolidateFileName,
                            Extension = "pdf",
                            FilePath = "/DocumentLibrary/MarkupFile/" + consolidateFileName,
                            ExtensionIcon = "~/images/pdffile.png",
                            FileSize = (double)fileInfo.Length / 1024,
                            TypeId = 3,
                            TypeName = "Document Markup/Comment Consolidate Files",
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedByName = UserSession.Current.User.UserNameWithFullName,
                            CreatedDate = DateTime.Now
                        };
                        
                            this.attachFileService.Insert(attachFile);

                        this.LbIsHaveFileConsolidate.Value = "true";
                       this.grdAttachMarkupCommentFile.Rebind();
                    }

                    // -----------------------------------------------------------------------------------------------------------------

                }

            }
        }

        private void GenerateChangeRequestConsolidate(string Auto)
        {try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
                {
                    Guid objId = new Guid(this.Request.QueryString["docId"]);
                    var changeRequestObj = this.changeRequestService.GetById(objId);
                    var flag = false;
                    var targetFolder = "../../DocumentLibrary/MarkupFile";
                    var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                        + "/DocumentLibrary/MarkupFile";

                    var originalAttachDoc = this.changeRequestAttachFileService.GetAllByChangeRequest(objId).FirstOrDefault(t => t.TypeId == 1 && (t.Extension.ToLower() == "pdf" || t.Extension.ToLower() == ".pdf"));

                    var markupfileList = this.changeRequestAttachFileService.GetAllByChangeRequest(objId).Where(t => t.TypeId == 2 && (t.Extension.ToLower() == "pdf" || t.Extension.ToLower() == ".pdf")).ToList();
                    if (originalAttachDoc != null && markupfileList.Count > 0)
                    {
                        var consolidateFile = new List<string>();

                        // Consolicate markup files
                        var originalFilePath = Server.MapPath("../.." + originalAttachDoc.FilePath);
                        // Split original file to files have same page size
                        var splitFiles = this.SplitSamePageSize(originalFilePath, targetFolder + "/Temp");
                        // -------------------------------------------------------------------------------------
                        var tempPageCount = 0;
                        var tempFileCount = 0;

                        // Process to consolidate on split file
                        foreach (var splitFile in splitFiles)
                        {
                            tempFileCount += 1;
                            var output = new MemoryStream();
                            var splitReader = new PdfReader(splitFile);
                            var document = new iTextSharp.text.Document(splitReader.GetCropBox(1));
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
                        var consolidateFileName = changeRequestObj.Number + "_" + changeRequestObj.Revision + "_ConsolidateMarkup_" + UserSession.Current.User.Username + Auto + ".pdf";
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
                                ChangeRequestId = objId,
                                FileName = consolidateFileName,
                                Extension = "pdf",
                                FilePath = "/DocumentLibrary/MarkupFile/" + consolidateFileName,
                                ExtensionIcon = "~/images/pdffile.png",
                                FileSize = (double)fileInfo.Length / 1024,
                                TypeId = 3,
                                TypeName = "Document Markup/Comment Consolidate Files",
                                CreatedBy = UserSession.Current.User.Id,
                                CreatedByName = UserSession.Current.User.UserNameWithFullName,
                                CreatedDate = DateTime.Now
                            };

                            this.changeRequestAttachFileService.Insert(attachFile);

                            this.LbIsHaveFileConsolidate.Value = "true";
                            this.grdAttachMarkupCommentFile.Rebind();
                        }

                        // -----------------------------------------------------------------------------------------------------------------
                    }

                }
            }
            catch { }
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
                splitFile.Add(filePath);
                ////int i = originalPageCount;
                ////var currentPageSize = originalReader.GetPageSize(i);
                ////var imp = writer.GetImportedPage(originalReader, (i));
                ////writer.AddPage(imp);

                ////if (i == originalReader.NumberOfPages)
                ////{
                ////    count += 1;
                ////    document.Close();
                ////    originalReader.Close();

                ////    var filepath = Path.Combine(Server.MapPath(targetFolder), Guid.NewGuid().ToString()) + "_Split" + count + ".pdf";
                ////    splitFile.Add(filepath);
                ////    File.WriteAllBytes(filepath, output.ToArray());
                ////}
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
            if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                var docId = new Guid(Request.QueryString["docId"]);
                
                var consolidateDocFile = this.attachFileService.GetAllDocId(docId).FirstOrDefault(t => t.TypeId == 3);
                if (consolidateDocFile != null)
                {
                    var filePath = Server.MapPath("../../Exports") + @"\";
                    var workbook = new Workbook(filePath + @"Template\PECC2_CRS_Template.xlsm");
                    var dataControlSheet = workbook.Worksheets[0];
                    var currentSheetIndex = Convert.ToInt32(dataControlSheet.Cells["A1"].Value);
                    var transSheet = workbook.Worksheets[currentSheetIndex];

                    if (currentSheetIndex != 1)
                    {
                        transSheet.IsVisible = true;
                    }

                    // Update new current sheet
                    dataControlSheet.Cells["A1"].Value = currentSheetIndex + 1;
                    // -------------------------------------------------------------------------------------------------------

                    var docObj = this.pecc2DocumentService.GetById(consolidateDocFile.ProjectDocumentId.GetValueOrDefault());
                    if (docObj != null)
                    {
                        var transObj = this.pecc2TransmittalService.GetById(docObj.IncomingTransId.GetValueOrDefault());

                        transSheet.Cells["B4"].PutValue("Doc. No.: " + docObj.DocNo);
                        transSheet.Cells["B5"].PutValue("Rev. No.: " + docObj.Revision);
                        transSheet.Cells["B6"].PutValue("Transmittal Contractor: " + Environment.NewLine  + docObj.IncomingTransNo);
                        transSheet.Cells["E4"].PutValue(docObj.DocTitle);
                        transSheet.Cells["E6"].PutValue("Transmittal No.: " + Environment.NewLine + docObj.OutgoingTransNo);

                        transSheet.Cells["B7"].PutValue("Document Action Code: " + Environment.NewLine + docObj.DocActionCode);
                        transSheet.Cells["E7"].PutValue("Document Review Status: " + Environment.NewLine + docObj.DocReviewStatusCode);
                        transSheet.Cells["F7"].PutValue("Consolidate Date: " + Environment.NewLine + consolidateDocFile.CreatedDate.GetValueOrDefault().ToString("dd-MM-yyyy"));

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
                                    dataRow["FileName"] = consolidateDocFile.FileName.Split('_')[0] + ".pdf";
                                    dataRow["Page"] = i;
                                    dataRow["Comment"] = text ;
                                    dataRow["Review"] = username;
                                    dataRow["User"] = consolidateDocFile.CreatedByName.Split('/')[0];
                                    dataRow["Confirmation"] = string.Empty;

                                    dtFull.Rows.Add(dataRow);
                                }
                            }
                        }

                        transSheet.Cells.ImportDataTable(dtFull, false, 9, 1, dtFull.Rows.Count, dtFull.Columns.Count, true);
                        transSheet.Cells.DeleteRow(9 + dtFull.Rows.Count);

                        var filename = Utility.RemoveSpecialCharacterFileName(docObj.DocNo) + "_CRS.xlsm";
                        var saveFilePath = Server.MapPath("../.." + transObj.StoreFolderPath + "/eTRM File/" + filename);
                        workbook.Save(saveFilePath);

                        // Attach CRS to document Obj
                        var fileInfo = new FileInfo(saveFilePath);
                        if (fileInfo.Exists && this.attachFileService.GetAllDocId(docObj.ID).All(t => t.TypeId != 4))
                        {
                            var attachFile = new PECC2DocumentAttachFile()
                            {
                                ID = Guid.NewGuid(),
                                ProjectDocumentId = docObj.ID,
                                FileName = filename,
                                Extension = ".xlsm",
                                FilePath = transObj.StoreFolderPath + "/eTRM File/" + filename,
                                ExtensionIcon = "~/images/excelfile.png",
                                FileSize = (double)fileInfo.Length / 1024,
                                TypeId = 4,
                                TypeName = "Comment Response Sheet (CRS)",
                                CreatedBy = UserSession.Current.User.Id,
                                CreatedByName = UserSession.Current.User.UserNameWithFullName,
                                CreatedDate = DateTime.Now
                            };

                            this.attachFileService.Insert(attachFile);
                        }
                        // -------------------------------------------------------------------------------------------------

                    this.grdAttachMarkupCommentFile.Rebind();
                    }
                }
            }
        }
        protected void fileNameValidator_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (this.ddlDocReviewStatus.SelectedItem == null)
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["actionType"]) && this.Request.QueryString["actionType"] == "4")
                {
                    this.fileNameValidator.ErrorMessage = "Please enter Document Code.";
                    this.divDocCode.Style["margin-bottom"] = "-26px;";
                    args.IsValid = false;
                }
            }
        }

        protected void grdAttachMarkupCommentFile_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            
        }

        protected void ChangeRequestReviewCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.ddlChangeRequestReviewCode.SelectedItem == null)
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["actionType"]) && this.Request.QueryString["actionType"] == "4")
                {
                    this.ChangeRequestReviewCodeValidator.ErrorMessage = "Please enter Document Code.";
                    this.divChangeRequestReviewCode.Style["margin-bottom"] = "-26px;";
                    args.IsValid = false;
                }
            }
        }

        protected void ddlDocReviewStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            var docId = new Guid(this.Request.QueryString["docId"]);
            var docObj = this.PECC2DocumentService.GetById(docId);
            if (docObj != null)
            {
                var reviewStatusObj = this.documentCodeServices.GetById(Convert.ToInt32(this.ddlDocReviewStatus.SelectedValue));
                if (reviewStatusObj != null)
                {
                    docObj.DocReviewStatusId = reviewStatusObj.ID;
                    docObj.DocReviewStatusCode = reviewStatusObj.Code;
                  
                }
                else
                {
                    docObj.DocReviewStatusId = Convert.ToInt32(this.ddlDocReviewStatus.SelectedValue);
                    docObj.DocReviewStatusCode = this.ddlDocReviewStatus.SelectedItem.Text.Split(',')[0]; ;

                }
                this.pecc2DocumentService.Update(docObj);

            }

            }

        protected void ddlDocReviewStatus1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var docId = new Guid(this.Request.QueryString["docId"]);
            var docObj = this.PECC2DocumentService.GetById(docId);
            if (docObj != null)
            {
                var reviewStatusObj = this.documentCodeServices.GetById(Convert.ToInt32(this.ddlDocReviewStatus1.SelectedValue));
                if (reviewStatusObj != null)
                {
                    docObj.DocReviewStatusId2 = reviewStatusObj.ID;
                    docObj.DocReviewStatusCode2= reviewStatusObj.Code;
                   
                }
                else
                {
                    docObj.DocReviewStatusId2 = Convert.ToInt32(this.ddlDocReviewStatus1.SelectedValue);
                    docObj.DocReviewStatusCode2 = this.ddlDocReviewStatus1.SelectedItem.Text.Split(',')[0];

                }
                this.pecc2DocumentService.Update(docObj);



            }
        }
    }
}