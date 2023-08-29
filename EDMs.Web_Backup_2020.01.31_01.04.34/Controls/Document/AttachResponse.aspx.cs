// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using EDMs.Business.Services.Library;

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
    public partial class AttachResponse : Page
    {

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly DocumentPackageService documentService;

        private readonly AttachResponseFileService attachFileService;

        private readonly CodeService codeService;

        private readonly ToDoListService toDoListService;


        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public AttachResponse()
        {
            this.documentService = new DocumentPackageService();
            this.attachFileService = new AttachResponseFileService();
            this.codeService = new CodeService();
            this.toDoListService = new ToDoListService();
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
                var docObj = this.documentService.GetById(Convert.ToInt32(this.Request.QueryString["docId"]));
                //var codeList = this.codeService.GetAllByProject(docObj.ProjectId.GetValueOrDefault());
                //codeList.Insert(0, new Code() {ID = 0});
                //this.ddlCode.DataSource = codeList;
                //this.ddlCode.DataTextField = "FullName";
                //this.ddlCode.DataValueField = "ID";
                //this.ddlCode.DataBind();

                //this.ddlCode.SelectedValue = docObj.FinalCodeID.GetValueOrDefault().ToString();

                var taskId = Convert.ToInt32(this.Request.QueryString["taskId"]);
                var taskObj = this.toDoListService.GetById(taskId);
                this.UploadControl.Visible = (taskObj != null && taskObj.TaskTypeId == 1) || taskId == -1;
                this.btnSave.Visible = (taskObj != null && taskObj.TaskTypeId == 1) || taskId == -1;

                this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = (taskObj != null && taskObj.TaskTypeId == 1) || taskId == -1;

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
                var docObj = this.documentService.GetById(docId);
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

                        var attachFile = new AttachResponseFile()
                            {
                                DocumentId = docId,
                                Filename = docFileName,
                                Extension = fileExt,
                                FilePath = serverFilePath,
                                ExtensionIcon = fileIcon.ContainsKey(fileExt.ToLower()) ? fileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
                                FileSize = (double)docFile.ContentLength / 1024,
                                CreatedBy = UserSession.Current.User.Id,
                                CreatedByName = UserSession.Current.User.FullNameWithPosition,
                                CreatedDate = DateTime.Now
                            };

                        this.attachFileService.Insert(attachFile);
                    }
                }

                //docObj.FinalCodeID = this.ddlCode.SelectedItem != null ? Convert.ToInt32(this.ddlCode.SelectedValue) : 0;
                //docObj.FinalCodeName = this.ddlCode.SelectedItem?.Text.Split('-')[0].Trim() ?? string.Empty;

                //this.documentService.Update(docObj);
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
                this.grdDocument.DataSource = this.attachFileService.GetAllByDocId(docId);
            }
            else
            {
                this.grdDocument.DataSource = new List<AttachCommentFile>();
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