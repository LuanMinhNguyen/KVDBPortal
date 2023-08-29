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
    using System.ServiceProcess;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Text.RegularExpressions;
    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class CorrespondenceInfoEditForm : Page
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
        public CorrespondenceInfoEditForm()
        {
            this.documentService = new  ConrespondenceService();
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
               
                this.LoadComboData();
                if ((!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() && !UserSession.Current.User.IsDC.GetValueOrDefault()))
                {
                    this.btnSave.Visible = false;
                }

                if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                {
                    this.CreatedInfo.Visible = true;
                    var docId = Convert.ToInt32(this.Request.QueryString["docId"]);
                    var folderId = Convert.ToInt32(Request.QueryString["folId"]);
                    var docObj = documentService.GetById(docId);
                    if (docObj != null)
                    {
                        this.LoadDocInfo(docObj);
                        
                    }
                    var userdataobj = this.userDataPermissionService.GetByUserId(UserSession.Current.User.Id, folderId);
                    if(userdataobj != null && userdataobj.IsFullPermission.GetValueOrDefault() == true)
                    {
                        this.btnSave.Visible = true;
                    }

                }
                else
                {
                    this.CreatedInfo.Visible = false;
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
            Conrespondence objDoc;

            this.Session.Remove("IsFillData");
           

            if (this.Page.IsValid)
            {
                try
                {
                    var lead = string.Empty;
                    var infor = string.Empty;
                    lead = this.ddlLead.CheckedItems.Aggregate(lead, (current, t) => current + t.Value + ", ");
                    infor = this.ddlInformation.CheckedItems.Aggregate(infor, (current, t) => current + t.Value + ", ");
                    if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                    {
                        objDoc = this.documentService.GetById(Convert.ToInt32(this.Request.QueryString["docId"]));
                
                        var filePath = Server.MapPath(HostingEnvironment.ApplicationVirtualPath == "/" ? objDoc.FilePath : objDoc.FilePath.Replace("/" + HostingEnvironment.ApplicationVirtualPath, "../.."));
                        var oldName = objDoc.FileName;
                        if (objDoc.FileName != this.txtName.Text.Trim())
                        {
                            

                            if (File.Exists(filePath))
                            {
                                File.Move(filePath, filePath.Replace(oldName, this.txtName.Text.Trim()));
                            }

                            objDoc.FileName = this.txtName.Text.Trim();
                            this.SaveUploadFile(this.docuploader, ref objDoc);

                            this.documentService.Update(objDoc);
                            
                        }
                        else
                        {
                            objDoc.FileName = this.txtName.Text;
                            objDoc.DocumentNumber = this.txtDocNumber.Text;
                            objDoc.Title = this.txtDocumentTitle.Text;
                            objDoc.DocumentTypeID = this.ddlCosType.SelectedValue != null ? Convert.ToInt32(this.ddlCosType.SelectedValue) : 0;
                            objDoc.DocumentTypeName = this.ddlCosType.SelectedItem != null ?
                                         this.ddlCosType.SelectedItem.Text
                                         : string.Empty;
                            objDoc.FolderID = Convert.ToInt32(Request.QueryString["folId"]);
                            objDoc.Remark = this.txtRemark.Text;
                            objDoc.Reply = this.txtReply.Text;
                            objDoc.ReferenceDocs = this.txtReferenceDocs.Text;
                            objDoc.IssueDate = this.txtIssueDate.SelectedDate;
                            objDoc.AnswerRequestDate = this.txtAnswerRequest.SelectedDate;
                            objDoc.DisciplineID = this.ddlDiscipline.SelectedValue != null ? Convert.ToInt32(this.ddlDiscipline.SelectedValue) : 0;
                            objDoc.DisciplineName = this.ddlDiscipline.SelectedItem != null ? this.ddlDiscipline.SelectedItem.Text : string.Empty;
                            objDoc.FromID = this.ddlFrom.SelectedValue != null ? Convert.ToInt32(this.ddlFrom.SelectedValue) : 0;
                            objDoc.FromName = this.ddlFrom.SelectedItem != null ? this.ddlFrom.SelectedItem.Text : string.Empty;
                            objDoc.ToID = this.ddlTo.SelectedValue != null ? Convert.ToInt32(this.ddlTo.SelectedValue) : 0;
                            objDoc.ToName = this.ddlTo.SelectedItem != null ? this.ddlTo.SelectedItem.Text : string.Empty;
                            objDoc.LeaderId = lead;
                            objDoc.Leader = this.ddlLead.Text;
                            objDoc.UserInforId = infor;
                           objDoc.UserInforName = this.ddlInformation.Text;
                            if (objDoc.CreatedBy == null)
                            {
                                objDoc.CreatedBy = UserSession.Current.User.Id;
                            }

                            objDoc.LastUpdatedBy = UserSession.Current.User.Id;
                            objDoc.LastUpdatedByName = UserSession.Current.User.Username;
                            objDoc.LastUpdatedDate = DateTime.Now;
                            this.documentService.Update(objDoc);
                        }
                    }
                    else
                    {
                        var folder = this.folderService.GetById(Convert.ToInt32(Request.QueryString["folId"]));
                        objDoc = new Conrespondence()
                        {
                            FileName = this.txtName.Text,
                            DocumentNumber = this.txtDocNumber.Text,
                            Title = this.txtDocumentTitle.Text,
                            DocumentTypeID = this.ddlCosType.SelectedValue != null ? Convert.ToInt32(this.ddlCosType.SelectedValue) : 0,
                            DocumentTypeName = this.ddlCosType.SelectedItem != null ?
                                         this.ddlCosType.SelectedItem.Text
                                         : string.Empty,
                            FolderID = Convert.ToInt32(Request.QueryString["folId"]),
                            Remark = this.txtRemark.Text,
                            Reply=this.txtReply.Text,
                            ReferenceDocs=this.txtReferenceDocs.Text,
                            IssueDate=this.txtIssueDate.SelectedDate,
                            AnswerRequestDate=this.txtAnswerRequest.SelectedDate,
                            DisciplineID=this.ddlDiscipline.SelectedValue != null ? Convert.ToInt32(this.ddlDiscipline.SelectedValue):0,
                            DisciplineName=this.ddlDiscipline.SelectedItem!= null? this.ddlDiscipline.SelectedItem.Text:string.Empty,
                            FromID=this.ddlFrom.SelectedValue != null ? Convert.ToInt32(this.ddlFrom.SelectedValue):0,
                            FromName=this.ddlFrom.SelectedItem != null ? this.ddlFrom.SelectedItem.Text: string.Empty,
                            ToID=this.ddlTo.SelectedValue != null? Convert.ToInt32(this.ddlTo.SelectedValue):0,
                            ToName=this.ddlTo.SelectedItem!= null? this.ddlTo.SelectedItem.Text: string.Empty,
                            LeaderId=lead,
                            Leader=this.ddlLead.Text,
                            UserInforId=infor,
                            UserInforName=this.ddlInformation.Text,
                            CreatedByName=UserSession.Current.User.Username,
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedDate = DateTime.Now,
                            IsLeaf = true,
                            IsDelete = false
                        };

                        //if (folder != null)
                        //{
                     

                        //    UploadedFile docFile = this.docuploader.UploadedFiles[0];
                        //    var serverFolder = HostingEnvironment.ApplicationVirtualPath == "/" ? "/" + folder.DirName : HostingEnvironment.ApplicationVirtualPath + "/" + folder.DirName;
                        //    var docFileName = docFile.FileName;

                        //    var serverFilePath = serverFolder + "/" + docFileName;
                        //    var fileExt = docFileName.Substring(docFileName.LastIndexOf(".") + 1, docFileName.Length - docFileName.LastIndexOf(".") - 1);

                        //    objDoc.FolderID = folder.ID;
                        //    objDoc.FilePath = serverFilePath;
                        //    objDoc.FileExtension = fileExt;
                        //    objDoc.FileExtensionIcon = !string.IsNullOrEmpty(Utility.FileIcons[fileExt]) ? Utility.FileIcons[fileExt] : "images/otherfile.png";
                        //    objDoc.DirName = folder.DirName;
                        //}
                        this.SaveUploadFile(this.docuploader, ref objDoc);

                        this.documentService.Insert(objDoc);
                    }


                    this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
                }
                catch (Exception ex)
                {
 
                }
            }
        }

        private void LoadDocInfo (Conrespondence docObjLeaf)
        {
            this.txtName.Text = docObjLeaf.FileName;
            this.txtDocNumber.Text = docObjLeaf.DocumentNumber;
            this.txtDocumentTitle.Text = docObjLeaf.Title;
            this.ddlCosType.SelectedValue = docObjLeaf.DocumentTypeID.GetValueOrDefault().ToString();
            this.txtRemark.Text = docObjLeaf.Remark;
            this.ddlDiscipline.SelectedValue = docObjLeaf.DisciplineID.GetValueOrDefault().ToString();
            this.txtIssueDate.SelectedDate = docObjLeaf.IssueDate;
            this.txtAnswerRequest.SelectedDate = docObjLeaf.AnswerRequestDate;
            this.txtReferenceDocs.Text = docObjLeaf.ReferenceDocs;
            this.txtReply.Text = docObjLeaf.Reply;
            this.ddlFrom.SelectedValue = docObjLeaf.FromID.GetValueOrDefault().ToString();
            this.ddlTo.SelectedValue = docObjLeaf.ToID.GetValueOrDefault().ToString();
            var createdUser = this.userService.GetByID(docObjLeaf.CreatedBy.GetValueOrDefault());

            this.lblCreated.Text = "Created at " + docObjLeaf.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

            if (docObjLeaf.LastUpdatedBy != null && docObjLeaf.LastUpdatedDate != null)
            {
                this.lblCreated.Text += "<br/>";
                var lastUpdatedUser = this.userService.GetByID(docObjLeaf.LastUpdatedBy.GetValueOrDefault());
                this.lblUpdated.Text = "Last modified at " + docObjLeaf.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
            }
            else
            {
                this.lblUpdated.Visible = false;
            }
            var lead = docObjLeaf.LeaderId.Split(',').Where(t=> !string.IsNullOrEmpty(t.Trim())).Select(t=> Convert.ToInt32(t)).ToList();
            foreach (RadComboBoxItem item in this.ddlLead.Items)
            {
                if (lead.Contains(Convert.ToInt32( item.Value)))
                {
                    item.Checked = true;
                }
            }

            var infor = docObjLeaf.UserInforId.Split(',').Where(t => !string.IsNullOrEmpty(t.Trim())).Select(t =>Convert.ToInt32( t)).ToList();
            foreach (RadComboBoxItem item in this.ddlInformation.Items)
            {
                if (infor.Contains(Convert.ToInt32(item.Value)))
                {
                    item.Checked = true;
                }
            }
        }

        private void LoadComboData()
        {
            //add discipline
            var Disciplinelist = this.disciplineService.GetAll();
            Disciplinelist.Insert(0, new Discipline() { ID = 0, Code = string.Empty });
            this.ddlDiscipline.DataSource = Disciplinelist.OrderByDescending(t=> t.FullName);
            this.ddlDiscipline.DataTextField = "FullName";
            this.ddlDiscipline.DataValueField = "ID";
            this.ddlDiscipline.DataBind();

            
            //add discipline
            var documentTypeList = this.documenttypeService.GetAllByParent(486);
            documentTypeList.Insert(0, new DocumentType() { ID = 0, Code = string.Empty });
            this.ddlCosType.DataSource = documentTypeList.OrderByDescending(t=> t.FullName);
            this.ddlCosType.DataTextField = "FullName";
            this.ddlCosType.DataValueField = "ID";
            this.ddlCosType.DataBind();


            var tolist = this._OrganizationcodeService.GetAll();
            tolist.Insert(0, new  OrganizationCode() { ID = 0, Code = string.Empty });
            this.ddlFrom.DataSource = tolist.OrderByDescending(t=> t.Code);
            this.ddlFrom.DataTextField = "Code";
            this.ddlFrom.DataValueField = "ID";
            this.ddlFrom.DataBind();
            this.ddlTo.DataSource = tolist.OrderByDescending(t => t.Code);
            this.ddlTo.DataTextField = "Code";
            this.ddlTo.DataValueField = "ID";
            this.ddlTo.DataBind();

            var listuser = this.userService.GetAll().Where(t => !t.Role.IsAdmin.GetValueOrDefault()).ToList();
            listuser.Insert(0, new User() { Id = 0, FullName = string.Empty });
            this.ddlLead.DataSource = listuser.OrderByDescending(t => t.FullName);
            this.ddlLead.DataTextField= "FullName";
           this.ddlLead.DataValueField = "Id";
            this.ddlLead.DataBind();
            this.ddlInformation.DataSource = listuser.OrderByDescending(t => t.FullName);
            this.ddlInformation.DataTextField = "FullName";
            this.ddlInformation.DataValueField = "Id";
            this.ddlInformation.DataBind();
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
        /// The server validation file name is exist.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void ServerValidationFileNameIsExist(object source, ServerValidateEventArgs args)
        {
            if (this.txtName.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter file name.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                var docId = Convert.ToInt32(Request.QueryString["docId"]);
                var folderId = Convert.ToInt32(Request.QueryString["folId"]);
                this.fileNameValidator.ErrorMessage = "The specified name is already in use.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = !this.documentService.IsDocumentExistUpdate(folderId, this.txtName.Text.Trim(), docId);
            }
        }

        /// <summary>
        /// The rad ajax manager 1_ ajax request.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument.Contains("CheckFileName"))
            {
            //    var fileName = e.Argument.Split('$')[1];
            //    var folderId = Convert.ToInt32(Request.QueryString["folId"]);
                
            //    if(this.documentService.IsConrespondenceExist(folderId, fileName))
            //    {
            //        var docObjLeaf = this.documentService.GetSpecificDocument(folderId, fileName);
            //        if (docObjLeaf != null)
            //        {
                      
            //            this.ddlDocumentType.SelectedValue = docObjLeaf.DocumentTypeID.GetValueOrDefault().ToString();
                      
            //            this.ddlDiscipline.SelectedValue = docObjLeaf.DisciplineID.GetValueOrDefault().ToString();
                       
            //            this.docUploadedIsExist.Value = "true";
            //            this.docIdUpdateUnIsLeaf.Value = docObjLeaf.ID.ToString();
            //        }
            //    }
            }
        }

        protected void docuploader_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            var folderId = Convert.ToInt32(Request.QueryString["folId"]);
            var fileName = e.File.FileName;

            this.txtName.Text = fileName;
            if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
            {
                this.txtName.ReadOnly = false;
            }
            else
            {
                this.txtName.ReadOnly = true;
            }

            if (this.documentService.IsConrespondenceExist(folderId, fileName))
            {
                var docObjLeaf = this.documentService.GetSpecificDocument(folderId, fileName);
                if (docObjLeaf != null)
                {
                    if (this.Session["IsFillData"] == null || this.Session["IsFillData"].ToString() != "false")
                    {
                        this.txtDocNumber.Text = docObjLeaf.DocumentNumber;
                        this.txtDocumentTitle.Text = docObjLeaf.Title;
                        this.ddlCosType.SelectedValue = docObjLeaf.DocumentTypeID.GetValueOrDefault().ToString();
                        this.txtRemark.Text = docObjLeaf.Remark;
                        this.ddlDiscipline.SelectedValue = docObjLeaf.DisciplineID.GetValueOrDefault().ToString();
                        this.txtIssueDate.SelectedDate = docObjLeaf.IssueDate;
                        this.txtAnswerRequest.SelectedDate = docObjLeaf.AnswerRequestDate;
                        this.txtReferenceDocs.Text = docObjLeaf.ReferenceDocs;
                        this.txtReply.Text = docObjLeaf.Reply;
                        this.ddlFrom.SelectedValue = docObjLeaf.FromID.GetValueOrDefault().ToString();
                        this.ddlTo.SelectedValue = docObjLeaf.ToID.GetValueOrDefault().ToString();
                        var lead = docObjLeaf.LeaderId.Split(',').Where(t => !string.IsNullOrEmpty(t)).Select(t => t).ToList();
                        foreach (RadComboBoxItem item in this.ddlLead.Items)
                        {
                            if (lead.Contains(item.Value))
                            {
                                item.Checked = true;
                            }
                        }

                        var infor = docObjLeaf.UserInforId.Split(',').Where(t => !string.IsNullOrEmpty(t)).Select(t => t).ToList();
                        foreach (RadComboBoxItem item in this.ddlInformation.Items)
                        {
                            if (infor.Contains(item.Value))
                            {
                                item.Checked = true;
                            }
                        }
                    }

                    this.Session.Add("IsFillData", "false");

                    this.docUploadedIsExist.Value = "true";
                    this.docIdUpdateUnIsLeaf.Value = docObjLeaf.ID.ToString();
                }
            }
        }


      
        private void SaveUploadFile(RadAsyncUpload uploadDocControl, ref Conrespondence objDoc)
        {
            try
            {
                var listUpload = uploadDocControl.UploadedFiles;
                var folder = this.folderService.GetById(objDoc.FolderID.GetValueOrDefault());
                var targetFolder =  folder.DirName.Replace("../../","~/");
                var serverFolder = HostingEnvironment.ApplicationVirtualPath == "/" ? "/" + folder.DirName.Replace("../../",string.Empty) : HostingEnvironment.ApplicationVirtualPath + "/" + folder.DirName.Replace("../../", string.Empty);
               
                if (listUpload.Count > 0)
                {
                    foreach (UploadedFile docFile in listUpload)
                    {
                        var docFileNameOrignal = docFile.FileName;
                     
                        // Path file to save on server disc
                        var saveFilePath = Path.Combine(Server.MapPath(targetFolder), Regex.Replace(docFileNameOrignal, @"[^0-9a-zA-Z_.-]+", string.Empty) );
                        // Path file to download from server
                        var serverFilePath = serverFolder + "/" + Regex.Replace(docFileNameOrignal, @"[^0-9a-zA-Z_.-]+", string.Empty);
                        var fileExt = docFileNameOrignal.Substring(docFileNameOrignal.LastIndexOf(".") + 1, docFileNameOrignal.Length - docFileNameOrignal.LastIndexOf(".") - 1);
                        fileExt = fileExt.Replace(".", string.Empty);
                        objDoc.FilePath = serverFilePath;
                        objDoc.FileExtension = fileExt;
                        objDoc.FileExtensionIcon = Utility.FileIcons.ContainsKey(fileExt.ToLower()) ? Utility.FileIcons[fileExt.ToLower()] : "~/images/otherfile.png";
                        objDoc.DirName = folder.DirName;
                            docFile.SaveAs(saveFilePath, true);
                 
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void ValidatorIssueDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if(this.txtIssueDate.SelectedDate == null)
            {
                this.ValidatorIssueDate.ErrorMessage = "Please enter Issue Date.";
                this.divIssueDate.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        protected void ValidatorTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.txtDocumentTitle.Text.Trim().Length == 0)
            {
                this.ValidatorTitle.ErrorMessage = "Please enter Document Title.";
                this.divTitle.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        protected void ValidatorLead_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.ddlLead.Text.Trim().Length == 0)
            {
                this.ValidatorLead.ErrorMessage = "Please select Lead.";
                this.divLead.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        protected void ValidatorFrom_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.ddlFrom.SelectedItem == null || this.ddlFrom.Text.Trim().Length==0)
            {
                this.ValidatorFrom.ErrorMessage = "Please choose From Unit.";
                this.divFrom.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        protected void ValidatorTo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.ddlTo.SelectedItem == null || this.ddlTo.Text.Trim().Length==0)
            {
                this.ValidatorTo.ErrorMessage = "Please choose To Unit.";
                this.divTo.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        protected void ValidatorCosType_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.ddlCosType.SelectedItem == null || this.ddlCosType.Text.Trim().Length==0)
            {
                this.ValidatorCosType.ErrorMessage = "Please select Cos Type.";
                this.divCosType.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }
    }
}