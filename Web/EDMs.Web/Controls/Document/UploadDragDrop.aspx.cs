// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
    public partial class UploadDragDrop : Page
    {

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly DocumentService documentService;

        private readonly AttachFileService attachFileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public UploadDragDrop()
        {
            this.documentService = new DocumentService();
            this.attachFileService = new AttachFileService();
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
                if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() && !UserSession.Current.User.Role.IsUpdate.GetValueOrDefault())
                {
                    this.btnSave.Visible = false;
                    this.UploadControl.Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("IsDefault").Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("IsDefaultImg").Visible = true;
                }
                else
                {
                    this.grdDocument.MasterTableView.GetColumn("IsDefault").Visible = true;
                    this.grdDocument.MasterTableView.GetColumn("IsDefaultImg").Visible = false;
                }

                if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                {
                    var objDoc = this.documentService.GetById(Convert.ToInt32(this.Request.QueryString["docId"]));
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
                var flag = false;
                const string TargetFolder = "../../DocumentLibrary";
                var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath) + "/DocumentLibrary";
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

                        var serverDocFileName = DateTime.Now.ToBinary() + "_" + docFileName;

                        // Path file to save on server disc
                        var saveFilePath = Path.Combine(Server.MapPath(TargetFolder), serverDocFileName);

                        // Path file to download from server
                        var serverFilePath = serverFolder + "/" + serverDocFileName;
                        var fileExt = docFileName.Substring(docFileName.LastIndexOf(".") + 1, docFileName.Length - docFileName.LastIndexOf(".") - 1);

                        docFile.SaveAs(saveFilePath, true);

                        var attachFile = new AttachFile()
                            {
                                DocumentId = docId,
                                Filename = docFileName,
                                Extension = fileExt,
                                FilePath = serverFilePath,
                                ExtensionIcon = fileIcon.ContainsKey(fileExt.ToLower()) ? fileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
                                FileSize = (double)docFile.ContentLength / 1024
                            };

                        var attachFileDefault = this.attachFileService.GetAllByDocId(docId).FirstOrDefault(t => t.IsDefault == true);
                        attachFile.IsDefault = attachFileDefault == null;

                        this.attachFileService.Insert(attachFile);
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

            var attachFileObj = this.attachFileService.GetById(docId);
            if (attachFileObj != null && attachFileObj.IsDefault.GetValueOrDefault())
            {
                var attachFiles = this.attachFileService.GetAllByDocId(attachFileObj.DocumentId.GetValueOrDefault()).Where(t => !t.IsDefault.GetValueOrDefault()).ToList();
                if (attachFiles.Count > 0)
                {
                    attachFiles[0].IsDefault = true;
                    this.attachFileService.Update(attachFiles[0]);
                }
            }

            this.attachFileService.Delete(docId);
            this.grdDocument.Rebind();
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                var docId = Convert.ToInt32(Request.QueryString["docId"]);
                this.grdDocument.DataSource = this.attachFileService.GetAllByDocId(docId);
            }
            else
            {
                this.grdDocument.DataSource = new List<AttachFile>();
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
            ((GridItem)((RadioButton)sender).Parent.Parent).Selected = ((RadioButton)sender).Checked;

            var item = ((RadioButton)sender).Parent.Parent as GridDataItem;
            var attachFileId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            var attachFileObj = this.attachFileService.GetById(attachFileId);
            if (attachFileObj != null)
            {
                var attachFiles = this.attachFileService.GetAllByDocId(attachFileObj.DocumentId.GetValueOrDefault());
                foreach (var attachFile in attachFiles)
                {
                    attachFile.IsDefault = attachFile.ID == attachFileId;
                    this.attachFileService.Update(attachFile);
                }
            }
        }
    }
}