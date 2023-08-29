// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services.Document;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class AttachDocument : Page
    {

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly DocumentPackageService documentPackageService;

        private readonly AttachFilesPackageService attachFileService;

        private readonly ScopeProjectService scopeProjectService;

        private readonly DistributionMatrixDetailService dmDetailService;

        private readonly ToDoListService toDoListService;

        private readonly RevisionService revisionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public AttachDocument()
        {
            this.documentPackageService = new DocumentPackageService();
            this.attachFileService = new AttachFilesPackageService();
            this.scopeProjectService = new ScopeProjectService();
            this.toDoListService = new ToDoListService();
            this.dmDetailService = new DistributionMatrixDetailService();
            this.revisionService = new RevisionService();
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
                this.setGrdRadioButtonOnClick();
                if (this.Request.QueryString["isFullPermission"] != "true")
                {
                    this.btnSave.Visible = false;
                    this.UploadControl.Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                }
                if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                {
                    var objDoc = this.documentPackageService.GetById(Convert.ToInt32(this.Request.QueryString["docId"]));
                    if (objDoc != null)
                    {
                        
                    }
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
            this.Session.Remove("IsFillData");
            if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                var docId = Convert.ToInt32(this.Request.QueryString["docId"]);
                var docObj = this.documentPackageService.GetById(docId);
                var projectObj = this.scopeProjectService.GetById(docObj.ProjectId.GetValueOrDefault());
                var flag = false;
                var targetFolder = projectObj != null ? "../.." + projectObj.DocsFolderPath : "../../DocumentLibrary/ProjectDocs";
                var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath) 
                    + (projectObj != null ? projectObj.DocsFolderPath : "/DocumentLibrary/ProjectDocs");
                var listUpload = docuploader.UploadedFiles;

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

                if (listUpload.Count > 0)
                {
                    foreach (UploadedFile docFile in listUpload)
                    {
                        var docFileName = docFile.FileName;

                        var serverDocFileName = docFileName;

                        // Path file to save on server disc
                        var saveFilePath = Path.Combine(Server.MapPath(targetFolder), serverDocFileName);

                        // Path file to download from server
                        var serverFilePath = serverFolder + "/" + serverDocFileName;
                        var fileExt = docFileName.Substring(docFileName.LastIndexOf(".") + 1, docFileName.Length - docFileName.LastIndexOf(".") - 1);

                        docFile.SaveAs(saveFilePath, true);

                        var attachFile = new AttachFilesPackage()
                            {
                                DocumentPackageID = docId,
                                FileName = docFileName,
                                Extension = fileExt,
                                FilePath = serverFilePath,
                                //FilePathEncrypt = CryptorEngine.Encrypt(serverFilePath, true),
                                ExtensionIcon = fileIcon.ContainsKey(fileExt.ToLower()) ? fileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
                                FileSize = (double)docFile.ContentLength / 1024,
                                AttachType = 1,
                                AttachTypeName = "Document file",
                                CreatedBy = UserSession.Current.User.Id,
                                CreatedDate = DateTime.Now
                            };

                        this.attachFileService.Insert(attachFile);

                        //docObj.RevisionActualDate = DateTime.Now;
                        var docRevObj = this.revisionService.GetById(docObj.RevisionId.GetValueOrDefault());
                        if (docObj.FirstIssueActualDate == null && docRevObj.IsFirst.GetValueOrDefault())
                        {
                            docObj.FirstIssueActualDate = DateTime.Now;
                        }
                        this.documentPackageService.Update(docObj);

                        if (projectObj != null && projectObj.IsAutoDistribute.GetValueOrDefault())
                        {
                            // Create review Task
                            var dmDetailList = this.dmDetailService.GetAllByDisAndDocType(docObj.DisciplineId.GetValueOrDefault(), docObj.DocumentTypeId.GetValueOrDefault());
                            foreach (var dmDetail in dmDetailList)
                            {
                                if (dmDetail.ActionTypeId != 1)
                                {
                                    var todoItem = new ToDoList()
                                    {
                                        DocId = docObj.ID,
                                        DocDisciplineId = docObj.DisciplineId,
                                        DocDisciplineName = docObj.DisciplineFullName,
                                        DocProjectId = docObj.ProjectId,
                                        DocTitle = docObj.DocTitle,
                                        DocReceivedDate = docObj.RevisionActualDate,
                                        DocReceivedTransNo = docObj.RevisionReceiveTransNo,
                                        DocRevName = docObj.RevisionName,
                                        DocNumber = docObj.DocNo,
                                        UserId = dmDetail.UserId,
                                        UserName = dmDetail.UserName,
                                        ActionName = dmDetail.ActionTypeFullName,
                                        DeadlineDate = docObj.RevisionActualDate != null ? docObj.RevisionActualDate.Value.AddDays(projectObj.ReviewDuration.GetValueOrDefault()) : docObj.RevisionActualDate,
                                        IsComplete = false,
                                        TaskTypeId = 1,
                                        TaksTypeName = "Engineer review comment Document"
                                    };

                                    this.toDoListService.Insert(todoItem);

                                    // Send Notifications
                                    if (projectObj.IsAutoSendNotification.GetValueOrDefault())
                                    {

                                    }
                                    // --------------------------------------------------------------------------
                                }
                            }
                            // ----------------------------------------------------------------------------------
                        }
                    }
                }
            }

            this.docuploader.UploadedFiles.Clear();
            this.grdDocument.Rebind();
        }

        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var docId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());

            this.attachFileService.Delete(docId);
            this.grdDocument.Rebind();
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                var docId = Convert.ToInt32(Request.QueryString["docId"]);
                var document = this.documentPackageService.GetById(docId);
                var attachList = this.attachFileService.GetAllDocumentFileByDocId(docId);

                if (document != null)
                {
                    if (document.ProjectId.ToString() == ConfigurationManager.AppSettings.Get("BKTProjectId") && 
                        !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault())
                    {
                        attachList = attachList.Where(t => t.Extension.ToLower() == "pdf").ToList();
                    }
                }

                this.grdDocument.DataSource = attachList;
            }
            else
            {
                this.grdDocument.DataSource = new List<AttachFilesPackage>();
            }
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void setGrdRadioButtonOnClick()
        {
            int i;
            RadioButton radioButton;
            for (i = 0; i < grdDocument.Items.Count; i++)
            {

                radioButton = (RadioButton)grdDocument.Items[i].FindControl("rdSelect");

                radioButton.Attributes.Add("OnClick", "SelectMeOnly(" + radioButton.ClientID + ", " + "'grdDocument'" + ")");
            }
        }

        protected void rbtnDefaultDoc_CheckedChanged(object sender, EventArgs e)
        {
            //((GridItem)((RadioButton)sender).Parent.Parent).Selected = ((RadioButton)sender).Checked;

            //var item = ((RadioButton)sender).Parent.Parent as GridDataItem;
            //var attachFileId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            //var attachFileObj = this.attachFileService.GetById(attachFileId);
            //if (attachFileObj != null)
            //{
            //    var attachFiles = this.attachFileService.GetAllByDocId(attachFileObj.DocumentId.GetValueOrDefault());
            //    foreach (var attachFile in attachFiles)
            //    {
            //        attachFile.IsDefault = attachFile.ID == attachFileId;
            //        this.attachFileService.Update(attachFile);
            //    }
            //}
        }
    }
}