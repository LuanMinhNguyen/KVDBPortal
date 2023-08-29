// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Net;
using System.Net.Mail;
using System.Text;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.IO;
    using System.Web.Hosting;
    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class UploadMultiDocumentFile : Page
    {
        private readonly AttachFilesPackageService attachFileService;

        private readonly DocumentPackageService documentPackageService;

        private readonly ScopeProjectService scopeProjectService;

        private readonly DistributionMatrixDetailService dmDetailService;

        private readonly ToDoListService toDoListService;

        private readonly RevisionService revisionService;

        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public UploadMultiDocumentFile()
        {
            this.attachFileService = new AttachFilesPackageService();
            this.documentPackageService = new DocumentPackageService();
            this.scopeProjectService = new ScopeProjectService();
            this.dmDetailService = new DistributionMatrixDetailService();
            this.toDoListService = new ToDoListService();
            this.revisionService = new RevisionService();
            this.userService = new UserService();
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
            try
            {
                if (!this.IsPostBack)
                {
                    if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() &&
                        !UserSession.Current.User.Role.IsUpdate.GetValueOrDefault())
                    {
                        this.btnSave.Visible = false;
                        this.UploadControl.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Controls/Security/Login.aspx");
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


                List<ToDoList> ListTodolist = new List<ToDoList>();
                List<ToDoList> ListInformation = new List<ToDoList>();
                var uncompleteUploadFileList = new List<string>();
                var fileIcon = new Dictionary<string, string>()
                {
                    {"doc", "~/images/wordfile.png"},
                    {"docx", "~/images/wordfile.png"},
                    {"dotx", "~/images/wordfile.png"},
                    {"xls", "~/images/excelfile.png"},
                    {"xlsx", "~/images/excelfile.png"},
                    {"pdf", "~/images/pdffile.png"},
                    {"7z", "~/images/7z.png"},
                    {"dwg", "~/images/dwg.png"},
                    {"dxf", "~/images/dxf.png"},
                    {"rar", "~/images/rar.png"},
                    {"zip", "~/images/zip.png"},
                    {"txt", "~/images/txt.png"},
                    {"xml", "~/images/xml.png"},
                    {"xlsm", "~/images/excelfile.png"},
                    {"bmp", "~/images/bmp.png"},
                };

                var projectId = Convert.ToInt32(this.Request.QueryString["projectId"]);
                var projectObj = this.scopeProjectService.GetById(projectId);
                if (projectObj != null)
                {
                    var targetFolder = projectObj != null ? "../.." + projectObj.DocsFolderPath : "../../DocumentLibrary/ProjectDocs";
                    var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                        + (projectObj != null ? projectObj.DocsFolderPath : "/DocumentLibrary/ProjectDocs");
                    var listUpload = docuploader.UploadedFiles;

                    foreach (UploadedFile docFile in listUpload)
                    {
                        var docFileName = docFile.FileName.Replace(" ", string.Empty);
                        var docNo = docFileName.Split('.')[0].Split('_')[0];
                        var docRev = docFileName.Split('.')[0].Split('_')[1];
                        var docObj = this.documentPackageService.GetOneByDocNo(docNo, docRev, projectObj.ID);
                        if (docObj != null)
                        {
                            var serverDocFileName = docFileName;

                            // Path file to save on server disc
                            var saveFilePath = Path.Combine(Server.MapPath(targetFolder), serverDocFileName);

                            // Path file to download from server
                            var serverFilePath = serverFolder + "/" + serverDocFileName;
                            var fileExt = docFileName.Substring(docFileName.LastIndexOf(".") + 1, docFileName.Length - docFileName.LastIndexOf(".") - 1);

                            docFile.SaveAs(saveFilePath, true);

                            var attachFile = new AttachFilesPackage()
                            {
                                DocumentPackageID = docObj.ID,
                                FileName = docFileName,
                                Extension = fileExt,
                                FilePath = serverFilePath,
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
                                var dmDetailList =
                                    this.dmDetailService.GetAllByDisAndDocType(docObj.DisciplineId.GetValueOrDefault(),
                                        docObj.DocumentTypeId.GetValueOrDefault());
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
                                            DeadlineDate =
                                                docObj.RevisionActualDate != null
                                                    ? docObj.RevisionActualDate.Value.AddDays(
                                                        projectObj.ReviewDuration.GetValueOrDefault())
                                                    : docObj.RevisionActualDate,
                                            IsComplete = false,
                                            TaskTypeId = 1,
                                            TaksTypeName = "Engineer review comment Document"
                                        };

                                        this.toDoListService.Insert(todoItem);
                                        ListTodolist.Add(todoItem);
                                      
                                    }

                                    if (dmDetail.ActionTypeId == 1)
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
                                                ActionName = dmDetail.ActionTypeFullName
                                            };
                                        ListInformation.Add(todoItem);
                                    
                                    }
                                }
                            }
                            // ----------------------------------------------------------------------------------
                        }
                        else
                        {
                            uncompleteUploadFileList.Add(docFile.FileName);
                        }
                    }

                    // Send Notifications
                    if (projectObj.IsAutoSendNotification.GetValueOrDefault() && ListTodolist.Count()>0)
                    {
                        var engineering = ListTodolist.Select(t => t.UserId.GetValueOrDefault()).Distinct().ToList();
                        
                        foreach (var tem in engineering)
                        {
                            var en = this.userService.GetByID(tem);
                            NotifiListDocument(ListTodolist.Where(t => t.UserId == en.Id).ToList(), en);
                        }

                    }


                    if (projectObj.IsAutoSendNotification.GetValueOrDefault() && ListInformation.Count() > 0)
                    {
                        var engineering = ListInformation.Select(t => t.UserId.GetValueOrDefault()).Distinct().ToList();

                        foreach (var tem in engineering)
                        {
                            var en = this.userService.GetByID(tem);
                            NotifiListInfor(ListInformation.Where(t => t.UserId == en.Id).ToList(), en);
                        }

                    }
                    // --------------------------------------------------------------------------NotifiListInfor


                    this.docuploader.UploadedFiles.Clear();

                    if (uncompleteUploadFileList.Count > 0)
                    {
                        this.blockError.Visible = true;
                        this.lblError.Text = "Can't find some document with Document number: <br/>";
                        foreach (var item in uncompleteUploadFileList)
                        {
                            this.lblError.Text += "<span style='color: blue; font-weight: bold'>'" + item.Split('.')[0].Split('_')[0] + "'</span> and Revision: <b>"+ item.Split('.')[0].Split('_')[1] + "</b> to attach file <span style='color: orange; font-weight: bold'>'" + item + "'</span> <br/>";
                        }
                    }
                    else
                    {
                        this.blockError.Visible = true;
                        this.lblError.Text = "All document files are attach successfull.";
                    }
                }
                else
                {
                    this.blockError.Visible = true;
                    this.lblError.Text = "Have error: Project is not valid, Please re-check project information.";
                }
            }
            catch (Exception ex)
            {
                this.blockError.Visible = true;
                this.lblError.Text = "Have error when upload document file: <br/>'" + ex.Message + "'";
            }
        }

        private void NotifiListDocument(List<ToDoList> Listdoc, User engineer)
        {
            try
            {
                if (engineer != null)
                {

                    if (engineer != null && !string.IsNullOrEmpty(engineer.Email))
                    {
                        var email = ConfigurationManager.AppSettings["Email"];
                        var emailPass = ConfigurationManager.AppSettings["EmailPass"];

                        var smtpClient = new SmtpClient
                        {
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                            EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                            Host = ConfigurationManager.AppSettings["Host"],
                            Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                            Credentials = new NetworkCredential(email, emailPass)
                        };

                        var subject = "Documents has been assigned to #AssignedUser# on DMS System";
                       

                        var message = new MailMessage();
                        message.From = new MailAddress(email, "PVCFC - DMS System");
                        message.Subject = subject.Replace("#AssignedUser#",engineer.FullName);
                        message.BodyEncoding = new UTF8Encoding();
                        message.IsBodyHtml = true;
                        
                        int count = 0;
                        var containtable = string.Empty;

                     
                       

                        var bodyContent = @"<div style=‘text-align: center;’> <span>
                                    ==========================================================<br />
                                    <span class=‘Apple-tab-span’>&nbsp; &nbsp; &nbsp; SPF System&nbsp;</span><br />
                                    <span class=‘Apple-tab-span’>Action Time: &nbsp;</span>&nbsp;" + DateTime.Now.Date.ToString("dd-MM-yyyy") + @"<br />
                                    ==========================================================<br />
                                    </span>
                                    <p style=‘text-align: center;’><strong><span style=‘font-size: 18px;’>Documents Information</span></strong></p><br/><br/>

                                       <table border='1' cellspacing='0'>
                                       <tr>
                                       <th style='text-align:center; width:40px'>No.</th>
                                       <th style='text-align:center; width:330px'>Document Number</th>
                                       <th style='text-align:center; width:330px'>Document Title</th>
                                       <th style='text-align:center; width:60px'>Revision</th>
                                       <th style='text-align:center; width:120px'>Deadline</th>
                                       <th style='text-align:center; width:120px'>Discipline</th>
                                       </tr>";
                        foreach (var document in Listdoc)
                        {
                            var deadline = string.Empty;
                            count += 1;
                            deadline = document.DeadlineDate != null ? document.DeadlineDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                            bodyContent += @"<tr>
                               <td>" + count + @"</td>
                               <td>" + document.DocNumber + @"</td>
                               <td>"
                                           + document.DocTitle + @"</td>
                               <td>"
                                           + document.DocRevName + @"</td>
                               <td>"
                                           + deadline + @"</td>
                               <td>"
                                           + document.DocDisciplineName + @"</td>";

                        }

                        bodyContent += @"</table>
                                       <br/>
                                       <span><br />
                                    &nbsp;Click on the this link to access the CMDR system&nbsp;:&nbsp; " + ConfigurationSettings.AppSettings.Get("WebAddress");




                        message.Body = bodyContent;
                        message.To.Add(new MailAddress(engineer.Email));


                        smtpClient.Send(message);

                    }
                }
            }
            catch { }
        }

        private void NotifiListInfor(List<ToDoList> Listdoc, User engineer)
        {
            try
            {
                if (engineer != null)
                {

                    if (engineer != null && !string.IsNullOrEmpty(engineer.Email))
                    {
                        var email = ConfigurationManager.AppSettings["Email"];
                        var emailPass = ConfigurationManager.AppSettings["EmailPass"];

                        var smtpClient = new SmtpClient
                        {
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                            EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                            Host = ConfigurationManager.AppSettings["Host"],
                            Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                            Credentials = new NetworkCredential(email, emailPass)
                        };

                       // var subject = "Documents has been assigned to #AssignedUser# on EDMS System";


                        var message = new MailMessage();
                        message.From = new MailAddress(email, "PVCFC - DMS System");
                        message.Subject = "New Document Now Available.";
                        message.BodyEncoding = new UTF8Encoding();
                        message.IsBodyHtml = true;

                        int count = 0;
                        var containtable = string.Empty;




                        var bodyContent = @"<div style=‘text-align: center;’> <span>
                                    ==========================================================<br />
                                    <span class=‘Apple-tab-span’>&nbsp; &nbsp; &nbsp; SPF System&nbsp;</span><br />
                                    <span class=‘Apple-tab-span’>Action Time: &nbsp;</span>&nbsp;" + DateTime.Now.Date.ToString("dd-MM-yyyy") + @"<br />
                                    ==========================================================<br />
                                    </span>
                                    <p style=‘text-align: center;’><strong><span style=‘font-size: 18px;’>Documents Information</span></strong></p><br/><br/>

                                       <table border='1' cellspacing='0'>
                                       <tr>
                                       <th style='text-align:center; width:40px'>No.</th>
                                       <th style='text-align:center; width:330px'>Document Number</th>
                                       <th style='text-align:center; width:330px'>Document Title</th>
                                       <th style='text-align:center; width:60px'>Revision</th>
                                       <th style='text-align:center; width:120px'>Discipline</th>
                                       </tr>";
                        foreach (var document in Listdoc)
                        {
                            count += 1;
                            bodyContent += @"<tr>
                               <td>" + count + @"</td>
                               <td>" + document.DocNumber + @"</td>
                               <td>"
                                           + document.DocTitle + @"</td>
                               <td>"
                                           + document.DocRevName + @"</td>
                               <td>"
                                           + document.DocDisciplineName + @"</td>";

                        }

                        bodyContent += @"</table>
                                       <br/>
                                       <span><br />
                                    &nbsp;Click on the this link to access the CMDR system&nbsp;:&nbsp; " + ConfigurationSettings.AppSettings.Get("WebAddress");




                        message.Body = bodyContent;
                        message.To.Add(new MailAddress(engineer.Email));


                        smtpClient.Send(message);

                    }
                }
            }
            catch { }
        }


    }
}