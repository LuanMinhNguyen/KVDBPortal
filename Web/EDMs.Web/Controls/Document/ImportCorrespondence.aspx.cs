// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Linq;
using EDMs.Business.Services.Scope;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Hosting;
    using System.Web.UI;
    using EDMs.Web.Utilities;

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
    public partial class ImportCorrespondence : Page
    {
        /// <summary>
        /// The service name.
        /// </summary>
        protected const string ServiceName = "EDMSFolderWatcher";

        private readonly OrganizationCodeService _OrganizationcodeService;
        private readonly DisciplineService disciplineService;

        private readonly DocumentTypeService documenttypeService;

        private readonly ToListService tolistService;

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly ConrespondenceService documentService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly FolderService folderService;

        private readonly GroupDataPermissionService groupDataPermissionService;
        /// <summary>
        /// The user data permission.
        /// </summary>
        private readonly UserDataPermissionService userDataPermissionService;


        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ImportCorrespondence()
        {
            this.documentService = new ConrespondenceService();
            this._OrganizationcodeService = new OrganizationCodeService();
            this.userService = new UserService();
            this.folderService = new FolderService();
            this.disciplineService = new DisciplineService();
            this.documenttypeService = new DocumentTypeService();
            this.tolistService = new ToListService();
            this.groupDataPermissionService = new GroupDataPermissionService();
            this.userDataPermissionService = new UserDataPermissionService();
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
                //if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                //{
                //    var objDoc = this.documentService.GetById(Convert.ToInt32(this.Request.QueryString["docId"]));
                //    if (objDoc != null)
                //    {
                        
                //    }
                //}
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

            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["folId"]))
                {
                    var folderID = Convert.ToInt32(Request.QueryString["folId"]);
                    var folderObj = this.folderService.GetById(folderID);
                    var currentSheetName = string.Empty;
                    var currentDocumentNo = string.Empty;
                    var documentPackageList = new List<Conrespondence>();
                    var listExistFile = new List<string>();
                    if (folderObj != null)
                    {
                        var targetFolder = folderObj.DirName.Replace("../../", "~/");
                        var serverFolder = HostingEnvironment.ApplicationVirtualPath == "/" ? "/" + folderObj.DirName.Replace("../../", string.Empty) : HostingEnvironment.ApplicationVirtualPath + "/" + folderObj.DirName.Replace("../../", string.Empty);

                        foreach (UploadedFile docFile in this.docuploader.UploadedFiles)
                        {
                            var extension = docFile.GetExtension();
                            if (extension == ".xls" || extension == ".xlsx" || extension == ".xlsm")
                            {
                                var importPath = Server.MapPath("../../Import") + "/" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "_" + docFile.FileName;
                                docFile.SaveAs(importPath);

                                var workbook = new Workbook();
                                workbook.Open(importPath);


                                var datasheet = workbook.Worksheets[2];

                                currentSheetName = datasheet.Name;
                                var dataTable = datasheet.Cells.ExportDataTable(2, 1,
                                datasheet.Cells.MaxRow, 15);
                                foreach (DataRow dataRow in dataTable.Rows)
                                {
                                    if (!string.IsNullOrEmpty(dataRow["Column1"].ToString().Trim()) && dataRow["Column1"].ToString().Trim().Length>1)
                                    {
                                        currentDocumentNo= dataRow["Column1"].ToString().Trim();
                                        var fileName= dataRow["Column14"].ToString().Trim();
                                        var objExist = this.documentService.IsExist(folderID, fileName);
                                        if (objExist == null)
                                        {
                                            var objDoc = new Conrespondence()
                                            {
                                                FolderID = Convert.ToInt32(Request.QueryString["folId"]),
                                                CreatedByName = UserSession.Current.User.Username,
                                                CreatedBy = UserSession.Current.User.Id,
                                                CreatedDate = DateTime.Now,
                                                IsLeaf = true,
                                                IsDelete = false
                                            };
                                            //save infor
                                            objDoc.DocumentNumber = dataRow["Column1"].ToString().Trim();
                                            objDoc.Title = dataRow["Column5"].ToString().Trim();
                                            objDoc.ReferenceDocs = dataRow["Column3"].ToString().Trim();
                                            objDoc.Reply = dataRow["Column9"].ToString().Trim();
                                            objDoc.Remark = dataRow["Column10"].ToString().Trim();

                                            var strissuedate = dataRow["Column2"].ToString().Trim();
                                            var issueDate = new DateTime();
                                            if (Utility.ConvertStringToDateTime(strissuedate, ref issueDate))
                                            {
                                                objDoc.IssueDate = issueDate;
                                            }
                                            var stranswerdate = dataRow["Column4"].ToString().Trim();
                                            var answerDate = new DateTime();
                                            if (Utility.ConvertStringToDateTime(stranswerdate, ref answerDate))
                                            {
                                                objDoc.AnswerRequestDate = answerDate;
                                            }

                                            var stdiscipline = dataRow["Column6"].ToString().Trim();
                                            var disciplineObj = this.disciplineService.GetByCode(stdiscipline);
                                            objDoc.DisciplineID = disciplineObj != null ? disciplineObj.ID : 0;
                                            objDoc.DisciplineName = disciplineObj != null ? disciplineObj.FullName : string.Empty;

                                            var stuserlist = dataRow[6].ToString().Trim();
                                            foreach (var item in stuserlist.Split(',').Where(t=> !string.IsNullOrEmpty(t)))
                                            {
                                                var userObj = this.userService.GetUserByUsername(item.Trim());
                                                if (userObj != null)
                                                {
                                                    objDoc.LeaderId += userObj.Id + ", ";
                                                    objDoc.Leader += userObj.FullName + ", ";

                                                }
                                            }

                                            var stInforlist = dataRow[7].ToString().Trim().Split(',').ToList();
                                            foreach (var item in stInforlist.Where(t => !string.IsNullOrEmpty(t)))
                                            {
                                                var userObj = this.userService.GetUserByUsername(item.Trim());
                                                if (userObj != null)
                                                {
                                                    objDoc.UserInforId += userObj.Id + ", ";
                                                    objDoc.UserInforName += userObj.FullName + ", ";

                                                }
                                            }
                                            var stfrom = dataRow["Column11"].ToString().Trim();
                                            var FromObj = this._OrganizationcodeService.GetByCode(stfrom);
                                            objDoc.FromID = FromObj != null ? FromObj.ID : 0;
                                            objDoc.FromName = FromObj != null ? FromObj.Code : string.Empty;

                                            var stTo = dataRow["Column12"].ToString().Trim();
                                            var ToObj = this._OrganizationcodeService.GetByCode(stTo);
                                            objDoc.ToID = ToObj != null ? ToObj.ID : 0;
                                            objDoc.ToName = ToObj != null ? ToObj.Code : string.Empty;

                                            var stDoctype = dataRow["Column13"].ToString().Trim();
                                            var DoctypeObj = this.documenttypeService.GetByCode(stDoctype);
                                            objDoc.DocumentTypeID = DoctypeObj != null ? DoctypeObj.ID : 0;
                                            objDoc.DocumentTypeName = DoctypeObj != null ? DoctypeObj.FullName : string.Empty;
                                            // Save file

                                            var docFileName = dataRow["Column14"].ToString().Trim();

                                            var docFileNameOrignal = docFileName;

                                            // Path file to save on server disc
                                            var saveFilePath = Path.Combine(Server.MapPath(targetFolder), docFileNameOrignal);

                                            // Path file to download from server
                                            var serverFilePath = serverFolder + "/" + docFileNameOrignal;
                                            var fileExt = docFileNameOrignal.Substring(docFileNameOrignal.LastIndexOf(".") + 1, docFileNameOrignal.Length - docFileNameOrignal.LastIndexOf(".") - 1);
                                            fileExt = fileExt.Replace(".", string.Empty);
                                            objDoc.FileName = docFileNameOrignal;
                                            objDoc.FilePath = serverFilePath;
                                            objDoc.FileExtension = fileExt;
                                            objDoc.FileExtensionIcon = Utility.FileIcons.ContainsKey(fileExt.ToLower()) ? Utility.FileIcons[fileExt.ToLower()] : "~/images/otherfile.png";
                                            objDoc.DirName = folderObj.DirName;

                                            this.documentService.Insert(objDoc);
                                        }else
                                    {
                                        listExistFile.Add(currentDocumentNo);
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
                                    this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this.blockError.Visible = true;
                this.lblError.Text = "Have error when import Progress: <br/>'" + ex.Message + "'";
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

        /// <summary>
        /// The save upload file.
        /// </summary>
        /// <param name="uploadDocControl">
        /// The upload doc control.
        /// </param>
        /// <param name="objDoc">
        /// The obj Doc.
        /// </param>
        private void SaveUploadFile(RadAsyncUpload uploadDocControl, ref Document objDoc)
        {
            var listUpload = uploadDocControl.UploadedFiles;
            if (listUpload.Count > 0)
            {
                foreach (UploadedFile docFile in listUpload)
                {
                    var revisionFilePath = Server.MapPath(objDoc.RevisionFilePath.Replace("/" + HostingEnvironment.ApplicationVirtualPath, "../.."));

                    docFile.SaveAs(revisionFilePath, true);
                }
            }
        }
    }
}