// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data;
using System.Drawing;
using EDMs.Business.Services.Scope;

namespace EDMs.Web
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.ServiceProcess;
    using System.Text;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Aspose.Cells;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;
    using Telerik.Web.Zip;

    using CheckBox = System.Web.UI.WebControls.CheckBox;
    using Label = System.Web.UI.WebControls.Label;
    using TextBox = System.Web.UI.WebControls.TextBox;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class ToDoList_Bak : Page
    {
        private readonly OptionalTypeService optionalTypeService = new OptionalTypeService();

        /// <summary>
        /// The permission service.
        /// </summary>
        private readonly PermissionService permissionService = new PermissionService();

        /// <summary>
        /// The revision service.
        /// </summary>
        private readonly RevisionService revisionService = new RevisionService();

        /// <summary>
        /// The document type service.
        /// </summary>
        private readonly DocumentTypeService documentTypeService = new DocumentTypeService();

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly FolderService folderService = new FolderService();


        private readonly DocumentNewService documentNewService = new DocumentNewService();

        private readonly NotificationRuleService notificationRuleService = new NotificationRuleService();

        private readonly GroupDataPermissionService groupDataPermissionService = new GroupDataPermissionService();

        private readonly UserService userService = new UserService();

        private readonly AttachFileService attachFileService = new AttachFileService();

        private readonly AttachFilesPackageService attachFilesPackageService = new AttachFilesPackageService();

        private readonly ScopeProjectService scopeProjectService = new ScopeProjectService();

        private readonly PackageService packageService = new PackageService();

        private readonly DocumentPackageService documentPackageService = new DocumentPackageService();

        private readonly DisciplineService disciplineService = new DisciplineService();

        private readonly RoleService roleService = new RoleService();

        private readonly TemplateManagementService templateManagementService = new TemplateManagementService();

        private readonly PermissionDisciplineService permissionDisciplineService = new PermissionDisciplineService();

        private readonly ContractorService contractorService = new ContractorService();

        private readonly CommentResponseService commentResponseService = new CommentResponseService();

        private readonly DocumentNumberingService documentNumberingService = new DocumentNumberingService();

        private readonly OriginatorService originatorService = new OriginatorService();

        private readonly ToDoListService toDoListService = new ToDoListService();

        protected const string ServiceName = "EDMSFolderWatcher";

        public static RadTreeNode editedNode = null;

        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string UnreadPattern = @"\(\d+\)";

        /// <summary>
        /// The list folder id.
        /// </summary>
        private List<int> listFolderId = new List<int>();

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
            Session.Add("SelectedMainMenu", "To Do List");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            if (!Page.IsPostBack)
            {
                this.LoadObjectTree();
                Session.Add("IsListAll", false);

                if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault())
                {
                    this.CustomerMenu.Items[0].Visible = false;
                    this.CustomerMenu.Items[1].Visible = false;
                    foreach (RadToolBarButton item in ((RadToolBarDropDown)this.CustomerMenu.Items[2]).Buttons)
                    {
                        if (item.Value == "Adminfunc")
                        {
                            item.Visible = false;
                        }
                    }

                    this.CustomerMenu.Items[3].Visible = false;
                }

                if (UserSession.Current.User.Role.IsAdmin.GetValueOrDefault())
                {
                    this.IsFullPermission.Value = "true";
                }
            }
        }

        /// <summary>
        /// The rad tree view 1_ node click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void radTreeFolder_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            var folder = this.folderService.GetById(Convert.ToInt32(e.Node.Value));
            var temp = (RadToolBarButton)this.CustomerMenu.FindItemByText("View explorer");
            temp.NavigateUrl = ConfigurationSettings.AppSettings.Get("ServerName") + folder.DirName;


            ////var originalURL = @"\\" + ConfigurationSettings.AppSettings.Get("ServerName") + @"\" + folder.DirName.Replace(@"/", @"\");
            ////var tempURI = new Uri(originalURL);/////

            ////var temp = (RadToolBarButton)this.CustomerMenu.FindItemByText("View explorer");
            ////temp.NavigateUrl = tempURI.AbsoluteUri;

            var isListAll = this.Session["IsListAll"] != null && Convert.ToBoolean(this.Session["IsListAll"]);
            this.LoadDocuments(true, isListAll);
        }

        /// <summary>
        /// Load all document by folder
        /// </summary>
        /// <param name="isbind">
        /// The isbind.
        /// </param>
        protected void LoadDocuments(bool isbind = false, bool isListAll = false)
        {
            var cbShowAll = (CheckBox)this.CustomerMenu.Items[4].FindControl("ckbShowAll");

            var taskList = this.toDoListService.GetAllByOwner(UserSession.Current.User.Id);
            if (this.rtvDiscipline.SelectedNode != null)
            {
                taskList = taskList.Where(t => t.DocDisciplineId == Convert.ToInt32(this.rtvDiscipline.SelectedNode.Value)).ToList();
            }

            if (!cbShowAll.Checked)
            {
                taskList = taskList.Where(t => !t.IsComplete.GetValueOrDefault()).ToList();
            }

            this.grdDocument.DataSource = taskList;
        }

        /// <summary>
        /// RadAjaxManager1  AjaxRequest
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                this.grdDocument.Rebind();
            }
            else if (e.Argument == "ExportEMDRReport_New")
            {
                this.ExportEMDRReportNew();
            }
            else if (e.Argument == "DeleteAllDoc")
            {
                foreach (GridDataItem selectedItem in this.grdDocument.SelectedItems)
                {
                    var docId = Convert.ToInt32(selectedItem.GetDataKeyValue("ID"));
                    var docObj = this.documentPackageService.GetById(docId);
                    if (docObj != null)
                    {
                        if (docObj.ParentId == null)
                        {
                            docObj.IsDelete = true;
                            this.documentPackageService.Update(docObj);
                        }
                        else
                        {
                            var listRelateDoc =
                                this.documentPackageService.GetAllRelatedDocument(docObj.ParentId.GetValueOrDefault());
                            if (listRelateDoc != null)
                            {
                                foreach (var objDoc in listRelateDoc)
                                {
                                    objDoc.IsDelete = true;
                                    this.documentPackageService.Update(objDoc);
                                }
                            }
                        }
                    }
                }

                this.grdDocument.Rebind();
            }
            else if (e.Argument == "ClearEMDRData")
            {
                var listDocPack = this.documentPackageService.GetAll();
                foreach (var documentPackage in listDocPack)
                {
                    this.documentPackageService.Delete(documentPackage);
                }

                var attachFilePackage = this.attachFilesPackageService.GetAll();
                foreach (var attachFilesPackage in attachFilePackage)
                {
                    var filePath = Server.MapPath(attachFilesPackage.FilePath);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    this.attachFilesPackageService.Delete(attachFilesPackage);
                }

                this.grdDocument.Rebind();
            }
            
            else if (e.Argument == "ExportEMDRReport")
            {
                var filePath = Server.MapPath("Exports") + @"\";
                var workbook = new Workbook();
                var docList = new List<DocumentPackage>();
                var projectName = string.Empty;
                var dtFull = new DataTable();
                var projectID = Convert.ToInt32(this.ddlProject.SelectedValue);
                var contractorList = this.contractorService.GetAllByProject(projectID);
                dtFull.Columns.AddRange(new[]
                {
                    new DataColumn("DocId", typeof (String)),
                    new DataColumn("NoIndex", typeof (String)),
                    new DataColumn("DocNo", typeof (String)),
                    new DataColumn("DocTitle", typeof (String)),
                    new DataColumn("Department", typeof (String)),
                    new DataColumn("Start", typeof (String)),
                    new DataColumn("Planned", typeof (String)),
                    new DataColumn("RevName", typeof (String)),
                    new DataColumn("RevPlanned", typeof (String)),
                    new DataColumn("RevActual", typeof (String)),
                    new DataColumn("RevCommentCode", typeof (String)),
                    new DataColumn("Complete", typeof (Double)),
                    new DataColumn("Weight", typeof (Double)),
                    new DataColumn("OutgoingNo", typeof (String)),
                    new DataColumn("OutgoingDate", typeof (String)),
                    new DataColumn("IncomingNo", typeof (String)),
                    new DataColumn("IncomingDate", typeof (String)),
                    new DataColumn("ICANo", typeof (String)),
                    new DataColumn("ICADate", typeof (String)),
                    new DataColumn("ICAReviewCode", typeof (String)),
                    new DataColumn("Notes", typeof (String)),
                    new DataColumn("IsEMDR", typeof (String)),
                    new DataColumn("HasAttachFile", typeof (String)),
                });

                foreach (var contractor in contractorList)
                {

                }

                var dtDiscipline = new DataTable();
                dtDiscipline.Columns.AddRange(new[]
                {
                    new DataColumn("DocId", typeof (String)),
                    new DataColumn("NoIndex", typeof (String)),
                    new DataColumn("DocNo", typeof (String)),
                    new DataColumn("DocTitle", typeof (String)),
                    new DataColumn("Start", typeof (DateTime)),
                    new DataColumn("RevName", typeof (String)),
                    new DataColumn("RevPlanned", typeof (DateTime)),
                    new DataColumn("RevActual", typeof (DateTime)),
                    new DataColumn("Complete", typeof (Double)),
                    new DataColumn("Weight", typeof (Double)),
                    new DataColumn("Department", typeof (String)),
                    new DataColumn("Notes", typeof (String)),
                    new DataColumn("IsEMDR", typeof (String)),
                    new DataColumn("HasAttachFile", typeof (String)),
                    new DataColumn("OutgoingNo", typeof (String)),
                    new DataColumn("OutgoingDate", typeof (String)),
                });
                projectName = this.ddlProject.SelectedItem.Text;

                if (this.rtvDiscipline.SelectedNode != null)
                {

                    var templateManagement = this.templateManagementService.GetSpecial(1, projectID);
                    if (templateManagement != null)
                    {
                        workbook.Open(Server.MapPath(templateManagement.FilePath));

                        var sheets = workbook.Worksheets;

                        var DisciplineId = Convert.ToInt32(this.rtvDiscipline.SelectedNode.Value);
                        var Discipline = this.disciplineService.GetById(DisciplineId);
                        if (Discipline != null)
                        {
                            sheets[0].Name = Discipline.Name;
                            sheets[0].Cells["C7"].PutValue(Discipline.Name);
                            sheets[0].Cells["B7"].PutValue(this.ddlProject.SelectedValue + "," + DisciplineId);
                            sheets[0].Cells["M4"].PutValue(DateTime.Now.ToString("dd/MM/yyyy"));
                            docList =
                                this.documentPackageService.GetAllEMDRByDiscipline(DisciplineId, false)
                                    .OrderBy(t => t.DocNo)
                                    .ToList();

                            var count = 1;

                            var listDocumentTypeId =
                                docList.Select(t => t.DocumentTypeId).Distinct().OrderBy(t => t).ToList();
                            foreach (var documentTypeId in listDocumentTypeId)
                            {
                                var documentType = this.documentTypeService.GetById(documentTypeId.GetValueOrDefault());

                                var dataRow = dtDiscipline.NewRow();
                                dataRow["DocId"] = -1;
                                dataRow["NoIndex"] = documentType != null ? documentType.FullName : string.Empty;
                                dtDiscipline.Rows.Add(dataRow);

                                var listDocByDocType = docList.Where(t => t.DocumentTypeId == documentTypeId).ToList();
                                foreach (var document in listDocByDocType)
                                {
                                    dataRow = dtDiscipline.NewRow();
                                    dataRow["DocId"] = document.ID;
                                    dataRow["NoIndex"] = count;
                                    dataRow["DocNo"] = document.DocNo;
                                    dataRow["DocTitle"] = document.DocTitle;
                                    dataRow["Start"] = (object) document.StartDate ?? DBNull.Value;
                                    dataRow["RevName"] = document.RevisionName;
                                    dataRow["RevPlanned"] = (object) document.RevisionPlanedDate ?? DBNull.Value;
                                    dataRow["RevActual"] = (object) document.RevisionActualDate ?? DBNull.Value;
                                    dataRow["Complete"] = document.Complete/100;
                                    dataRow["Weight"] = document.Weight/100;
                                    dataRow["Department"] = document.DeparmentName;
                                    dataRow["Notes"] = document.Notes;
                                    dataRow["IsEMDR"] = document.IsEMDR.GetValueOrDefault() ? "x" : string.Empty;
                                    dataRow["HasAttachFile"] = document.HasAttachFile ? "x" : string.Empty;
                                    dataRow["OutgoingNo"] = document.OutgoingTransNo;
                                    dataRow["OutgoingDate"] = document.OutgoingTransDate != null
                                        ? document.OutgoingTransDate.Value.ToString("dd/MM/yyyy")
                                        : string.Empty;
                                    count += 1;
                                    dtDiscipline.Rows.Add(dataRow);
                                }
                            }

                            sheets[0].Cells["A7"].PutValue(dtDiscipline.Rows.Count);

                            sheets[0].Cells.ImportDataTable(dtDiscipline, false, 7, 1, dtDiscipline.Rows.Count, 16, true);
                            sheets[1].Cells.ImportDataTable(dtDiscipline, false, 7, 1, dtDiscipline.Rows.Count, 16, true);

                            sheets[0].Cells[7 + dtDiscipline.Rows.Count, 2].PutValue("Total");

                            var txtDisciplineComplete =
                                this.CustomerMenu.Items[3].FindControl("txtDisciplineComplete") as RadNumericTextBox;
                            var txtDisciplineWeight =
                                this.CustomerMenu.Items[3].FindControl("txtDisciplineWeight") as RadNumericTextBox;
                            if (txtDisciplineComplete != null)
                            {
                                sheets[0].Cells[7 + dtDiscipline.Rows.Count, 9].PutValue(txtDisciplineComplete.Value/100);
                            }

                            if (txtDisciplineWeight != null)
                            {
                                sheets[0].Cells[7 + dtDiscipline.Rows.Count, 10].PutValue(txtDisciplineWeight.Value/100);
                            }
                            sheets[0].AutoFitRows();
                            sheets[1].IsVisible = false;

                            var filename = projectName + " - " + Discipline.Name + " EMDR Report " +
                                           DateTime.Now.ToString("dd-MM-yyyy") + ".xls";
                            workbook.Save(filePath + filename);
                            this.DownloadByWriteByte(filePath + filename, filename, true);

                        }
                    }
                }
                else
                {
                    var listDisciplineInPermission = UserSession.Current.User.Id == 1
                        ? this.disciplineService.GetAllDisciplineOfProject(Convert.ToInt32(this.ddlProject.SelectedValue))
                            .OrderBy(t => t.ID)
                            .ToList()
                        : this.disciplineService.GetAllDisciplineInPermission(UserSession.Current.User.Id,
                            !string.IsNullOrEmpty(this.ddlProject.SelectedValue)
                                ? Convert.ToInt32(this.ddlProject.SelectedValue)
                                : 0)
                            .OrderBy(t => t.ID).ToList();

                    if (listDisciplineInPermission.Count > 0)
                    {
                        var templateManagement = this.templateManagementService.GetSpecial(2,
                            projectID);
                        if (templateManagement != null)
                        {
                            workbook.Open(Server.MapPath(templateManagement.FilePath));

                            var totalDoc = 0;
                            var totalDocIssues = 0;
                            var totalDocRev0Issues = 0;
                            var totalDocRev1Issues = 0;
                            var totalDocRev2Issues = 0;
                            var totalDocRev3Issues = 0;
                            var totalDocRev4Issues = 0;
                            var totalDocRev5Issues = 0;
                            var totalDocRevIssues = 0;

                            var totalDocDontIssues = 0;

                            var sheets = workbook.Worksheets;
                            var wsSummary = sheets[0];
                            wsSummary.Cells.InsertRows(7, listDisciplineInPermission.Count - 1);

                            for (int i = 0; i < listDisciplineInPermission.Count; i++)
                            {
                                dtFull.Rows.Clear();

                                sheets.AddCopy(1);

                                sheets[i + 2].Name = listDisciplineInPermission[i].Name;
                                sheets[i + 2].Cells["V4"].PutValue(DateTime.Now.ToString("dd/MM/yyyy"));
                                sheets[i + 2].Cells["C7"].PutValue(listDisciplineInPermission[i].Name);

                                // Add hyperlink
                                var linkName = listDisciplineInPermission[i].Name;
                                wsSummary.Cells["B" + (7 + i)].PutValue(linkName);
                                wsSummary.Hyperlinks.Add("B" + (7 + i), 1, 1,
                                    "'" + listDisciplineInPermission[i].Name + "'" + "!D7");

                                docList =
                                    this.documentPackageService.GetAllEMDRByDiscipline(
                                        listDisciplineInPermission[i].ID, false).OrderBy(t => t.DocNo).ToList();
                                var docListHasAttachFile = docList.Where(t => t.HasAttachFile);
                                var wgDoc = docList.Count;
                                var wgDocIssues = docListHasAttachFile.Count();
                                var wgDocRev0Issues = docListHasAttachFile.Count(t => t.RevisionName == "0");
                                var wgDocRev1Issues = docListHasAttachFile.Count(t => t.RevisionName == "1");
                                var wgDocRev2Issues = docListHasAttachFile.Count(t => t.RevisionName == "2");
                                var wgDocRev3Issues = docListHasAttachFile.Count(t => t.RevisionName == "3");
                                var wgDocRev4Issues = docListHasAttachFile.Count(t => t.RevisionName == "4");
                                var wgDocRev5Issues = docListHasAttachFile.Count(t => t.RevisionName == "5");
                                var wgTotalDocRev = wgDocRev0Issues + wgDocRev1Issues + wgDocRev2Issues +
                                                    wgDocRev3Issues + wgDocRev4Issues + wgDocRev5Issues;
                                var wgDocDontIssues = wgDoc - wgDocIssues;

                                totalDoc += wgDoc;
                                totalDocIssues += wgDocIssues;
                                totalDocRev0Issues += wgDocRev0Issues;
                                totalDocRev1Issues += wgDocRev1Issues;
                                totalDocRev2Issues += wgDocRev2Issues;
                                totalDocRev3Issues += wgDocRev3Issues;
                                totalDocRev4Issues += wgDocRev4Issues;
                                totalDocRev5Issues += wgDocRev5Issues;
                                totalDocRevIssues += wgTotalDocRev;
                                totalDocDontIssues = totalDoc - totalDocIssues;

                                wsSummary.Cells["C" + (7 + i)].PutValue(wgDoc);
                                wsSummary.Cells["D" + (7 + i)].PutValue(wgDocDontIssues);
                                wsSummary.Cells["E" + (7 + i)].PutValue(wgDocRev0Issues);
                                wsSummary.Cells["F" + (7 + i)].PutValue(wgDocRev1Issues);
                                wsSummary.Cells["G" + (7 + i)].PutValue(wgDocRev2Issues);
                                wsSummary.Cells["H" + (7 + i)].PutValue(wgDocRev3Issues);
                                wsSummary.Cells["I" + (7 + i)].PutValue(wgDocRev4Issues);
                                wsSummary.Cells["J" + (7 + i)].PutValue(wgDocRev5Issues);
                                wsSummary.Cells["K" + (7 + i)].PutValue(wgTotalDocRev);
                                wsSummary.Cells["L" + (7 + i)].PutValue(wgTotalDocRev);


                                var count = 1;

                                var listDocumentTypeId =
                                    docList.Select(t => t.DocumentTypeId).Distinct().OrderBy(t => t).ToList();

                                double? complete = 0;
                                double? weight = 0;

                                foreach (var documentTypeId in listDocumentTypeId)
                                {
                                    var documentType =
                                        this.documentTypeService.GetById(documentTypeId.GetValueOrDefault());

                                    var dataRow = dtFull.NewRow();
                                    dataRow["NoIndex"] = documentType != null ? documentType.FullName : string.Empty;
                                    dtFull.Rows.Add(dataRow);

                                    var listDocByDocType =
                                        docList.Where(t => t.DocumentTypeId == documentTypeId).ToList();
                                    foreach (var document in listDocByDocType)
                                    {
                                        dataRow = dtFull.NewRow();
                                        dataRow["DocId"] = document.ID;
                                        dataRow["NoIndex"] = count;
                                        dataRow["DocNo"] = document.DocNo;
                                        dataRow["DocTitle"] = document.DocTitle;
                                        dataRow["Department"] = document.DeparmentName;
                                        dataRow["Start"] = document.StartDate != null
                                            ? document.StartDate.Value.ToString("dd/MM/yyyy")
                                            : string.Empty;
                                        dataRow["Planned"] = document.PlanedDate != null
                                            ? document.PlanedDate.Value.ToString("dd/MM/yyyy")
                                            : string.Empty;
                                        dataRow["RevName"] = document.RevisionName;
                                        dataRow["RevPlanned"] = document.RevisionPlanedDate != null
                                            ? document.RevisionPlanedDate.Value.ToString("dd/MM/yyyy")
                                            : string.Empty;
                                        dataRow["RevActual"] = document.RevisionActualDate != null
                                            ? document.RevisionActualDate.Value.ToString("dd/MM/yyyy")
                                            : string.Empty;
                                        dataRow["RevCommentCode"] = document.RevisionCommentCode;
                                        dataRow["Complete"] = document.Complete/100;
                                        dataRow["Weight"] = document.Weight/100;
                                        dataRow["OutgoingNo"] = document.OutgoingTransNo;
                                        dataRow["OutgoingDate"] = document.OutgoingTransDate != null
                                            ? document.OutgoingTransDate.Value.ToString("dd/MM/yyyy")
                                            : string.Empty;
                                        dataRow["IncomingNo"] = document.IncomingTransNo;
                                        dataRow["IncomingDate"] = document.IncomingTransDate != null
                                            ? document.IncomingTransDate.Value.ToString("dd/MM/yyyy")
                                            : string.Empty;
                                        dataRow["ICANo"] = document.ICAReviewOutTransNo;
                                        dataRow["ICADate"] = document.ICAReviewReceivedDate != null
                                            ? document.ICAReviewReceivedDate.Value.ToString("dd/MM/yyyy")
                                            : string.Empty;
                                        dataRow["ICAReviewCode"] = document.ICAReviewCode;
                                        dataRow["Notes"] = document.Notes;
                                        dataRow["IsEMDR"] = document.IsEMDR.GetValueOrDefault() ? "x" : string.Empty;
                                        dataRow["HasAttachFile"] = document.HasAttachFile ? "x" : string.Empty;

                                        count += 1;
                                        dtFull.Rows.Add(dataRow);

                                        complete += (document.Complete/100)*(document.Weight/100);
                                        weight += document.Weight/100;
                                    }
                                }

                                sheets[i + 2].Cells["A7"].PutValue(dtFull.Rows.Count);
                                sheets[i + 2].Cells.ImportDataTable(dtFull, false, 7, 1, dtFull.Rows.Count, 23, true);

                                sheets[i + 2].Cells[7 + dtFull.Rows.Count, 2].PutValue("Total");

                                sheets[i + 2].Cells[7 + dtFull.Rows.Count, 12].PutValue(complete);
                                sheets[i + 2].Cells[7 + dtFull.Rows.Count, 13].PutValue(weight);

                            }

                            wsSummary.Cells["H4"].PutValue(DateTime.Now.ToString("dd/MM/yyyy"));

                            wsSummary.Cells["C" + (7 + listDisciplineInPermission.Count)].PutValue(totalDoc);
                            wsSummary.Cells["D" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocDontIssues);
                            wsSummary.Cells["E" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocRev0Issues);
                            wsSummary.Cells["F" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocRev1Issues);
                            wsSummary.Cells["G" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocRev2Issues);
                            wsSummary.Cells["H" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocRev3Issues);
                            wsSummary.Cells["I" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocRev4Issues);
                            wsSummary.Cells["J" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocRev5Issues);
                            wsSummary.Cells["K" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocRevIssues);
                            wsSummary.Cells["L" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocRevIssues);

                            sheets[1].IsVisible = false;

                            var filename = projectName + " - " + "EMDR Report " +
                                           DateTime.Now.ToString("dd-MM-yyyy") + ".xls";
                            workbook.Save(filePath + filename);
                            this.DownloadByWriteByte(filePath + filename, filename, true);
                        }
                    }
                }
            }
            else if (e.Argument == "UpdatePackageStatus")
            {
                var txtPackageComplete =
                    this.CustomerMenu.Items[2].FindControl("txtPackageComplete") as RadNumericTextBox;
                var txtPackageWeight = this.CustomerMenu.Items[2].FindControl("txtPackageWeight") as RadNumericTextBox;

                var packageobj = this.packageService.GetById(Convert.ToInt32(this.rtvDiscipline.SelectedNode.Value));
                if (packageobj != null)
                {
                    if (txtPackageComplete != null)
                    {
                        packageobj.Complete = txtPackageComplete.Value.GetValueOrDefault();
                    }

                    if (txtPackageWeight != null)
                    {
                        packageobj.Weight = txtPackageWeight.Value.GetValueOrDefault();
                    }

                    this.packageService.Update(packageobj);
                }
            }
            else if (e.Argument.Contains("DeleteRev"))
            {
                string st = e.Argument.ToString();
                int docId = Convert.ToInt32(st.Replace("DeleteRev_", string.Empty));

                var docObj = this.documentPackageService.GetById(docId);
                var listRelateDoc =
                    this.documentPackageService.GetAllRelatedDocument(docObj.ParentId.GetValueOrDefault());
                if (docObj != null && listRelateDoc.Count > 1)
                {

                    docObj.IsDelete = true;
                    docObj.IsLeaf = false;
                    this.documentPackageService.Update(docObj);
                    docId = 0;
                    listRelateDoc =
                        this.documentPackageService.GetAllRelatedDocument(docObj.ParentId.GetValueOrDefault());
                    if (listRelateDoc != null)
                    {
                        foreach (var objDoc in listRelateDoc)
                        {
                            if (docId < objDoc.ID)
                            {
                                docId = objDoc.ID;
                                docObj = objDoc;
                            }
                        }
                    }
                    if (docId != 0)
                    {
                        docObj.IsLeaf = true;
                        this.documentPackageService.Update(docObj);
                        this.grdDocument.Rebind();
                    }
                }
                else
                {
                    Response.Write(
                        "<script>window.alert('Can not be reduced, because this document is only one version.')</script>");
                }
            }
            else if (e.Argument == "DownloadMulti")
            {
                var serverTotalDocPackPath =
                    Server.MapPath("~/Exports/DocPack/" + DateTime.Now.ToBinary() + "_DocPack.rar");
                var docPack = ZipPackage.CreateFile(serverTotalDocPackPath);

                foreach (GridDataItem item in this.grdDocument.MasterTableView.Items)
                {
                    var cboxSelected = (CheckBox) item["IsSelected"].FindControl("IsSelected");
                    if (cboxSelected.Checked)
                    {
                        var docId = Convert.ToInt32(item.GetDataKeyValue("ID"));

                        var name = (Label) item["Index1"].FindControl("lblName");
                        var serverDocPackPath =
                            Server.MapPath("~/Exports/DocPack/" + name.Text + "_" +
                                           DateTime.Now.ToString("ddMMyyyhhmmss") + ".rar");

                        var attachFiles = this.attachFileService.GetAllByDocId(docId);

                        var temp = ZipPackage.CreateFile(serverDocPackPath);

                        foreach (var attachFile in attachFiles)
                        {
                            if (File.Exists(Server.MapPath(attachFile.FilePath)))
                            {
                                temp.Add(Server.MapPath(attachFile.FilePath));
                            }
                        }

                        docPack.Add(serverDocPackPath);
                    }
                }

                this.DownloadByWriteByte(serverTotalDocPackPath, "DocumentPackage.rar", true);

            }
            else if (e.Argument == "RebindAndNavigate")
            {
                this.grdDocument.Rebind();
            }
            else if (e.Argument == "SendNotification")
            {
                var listDisciplineId = new List<int>();
                var listSelectedDoc = new List<Document>();
                var count = 0;
                foreach (GridDataItem item in this.grdDocument.MasterTableView.Items)
                {
                    var cboxSelected = (CheckBox) item["IsSelected"].FindControl("IsSelected");
                    if (cboxSelected.Checked)
                    {
                        count += 1;
                        var docItem = new Document();
                        var disciplineId = item["DisciplineID"].Text != @"&nbsp;"
                            ? item["DisciplineID"].Text
                            : string.Empty;
                        if (!string.IsNullOrEmpty(disciplineId) && disciplineId != "0")
                        {
                            listDisciplineId.Add(Convert.ToInt32(disciplineId));

                            docItem.ID = count;
                            docItem.DocumentNumber = item["DocumentNumber"].Text != @"&nbsp;"
                                ? item["DocumentNumber"].Text
                                : string.Empty;
                            docItem.Title = item["Title"].Text != @"&nbsp;"
                                ? item["Title"].Text
                                : string.Empty;
                            docItem.RevisionName = item["Revision"].Text != @"&nbsp;"
                                ? item["Revision"].Text
                                : string.Empty;
                            docItem.FilePath = item["FilePath"].Text != @"&nbsp;"
                                ? item["FilePath"].Text
                                : string.Empty;
                            docItem.DisciplineID = Convert.ToInt32(disciplineId);
                            listSelectedDoc.Add(docItem);
                        }
                    }
                }

                listDisciplineId = listDisciplineId.Distinct().ToList();

                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    Host = ConfigurationManager.AppSettings["Host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    Credentials =
                        new NetworkCredential(UserSession.Current.User.Email,
                            Utility.Decrypt(UserSession.Current.User.HashCode))
                };

                foreach (var disciplineId in listDisciplineId)
                {
                    var notificationRule = this.notificationRuleService.GetAllByDiscipline(disciplineId);

                    if (notificationRule != null)
                    {
                        var message = new MailMessage();
                        message.From = new MailAddress(UserSession.Current.User.Email, UserSession.Current.User.FullName);
                        message.Subject = "Test send notification from EDMs";
                        message.BodyEncoding = new UTF8Encoding();
                        message.IsBodyHtml = true;
                        message.Body = @"******<br/>
                                        Dear users,<br/><br/>

                                        Please be informed that the following documents are now available on the BDPOC Document Library System for your information.<br/><br/>

                                        <table border='1' cellspacing='0'>
	                                        <tr>
		                                        <th style='text-align:center; width:40px'>No.</th>
		                                        <th style='text-align:center; width:350px'>Document number</th>
		                                        <th style='text-align:center; width:350px'>Document title</th>
		                                        <th style='text-align:center; width:60px'>Revision</th>
	                                        </tr>";

                        if (!string.IsNullOrEmpty(notificationRule.ReceiverListId))
                        {
                            var listUserId =
                                notificationRule.ReceiverListId.Split(';').Select(t => Convert.ToInt32(t)).ToList();
                            foreach (var userId in listUserId)
                            {
                                var user = this.userService.GetByID(userId);
                                if (user != null)
                                {
                                    message.To.Add(new MailAddress(user.Email));
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(notificationRule.ReceiveGroupId) &&
                                 string.IsNullOrEmpty(notificationRule.ReceiverListId))
                        {
                            var listGroupId =
                                notificationRule.ReceiveGroupId.Split(';').Select(t => Convert.ToInt32(t)).ToList();
                            var listUser = this.userService.GetSpecialListUser(listGroupId);
                            foreach (var user in listUser)
                            {
                                message.To.Add(new MailAddress(user.Email));
                            }
                        }

                        var subBody = string.Empty;
                        foreach (var document in listSelectedDoc)
                        {
                            var port = ConfigurationSettings.AppSettings.Get("DocLibPort");
                            if (document.DisciplineID == disciplineId)
                            {
                                subBody += @"<tr>
                                <td>" + document.ID + @"</td>
                                <td><a href='http://" + Server.MachineName +
                                           (!string.IsNullOrEmpty(port) ? ":" + port : string.Empty)
                                           + document.FilePath + "' download='" + document.DocumentNumber + "'>"
                                           + document.DocumentNumber + @"</a></td>
                                <td>"
                                           + document.Title + @"</td>
                                <td>"
                                           + document.RevisionName + @"</td>";
                            }
                        }


                        message.Body += subBody + @"</table>
                                        <br/><br/>
                                        Thanks and regards,<br/>
                                        ******";

                        smtpClient.Send(message);
                    }
                }
            }

        }

        /// <summary>
        /// The rad grid 1_ on need data source.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            var isListAll = this.Session["IsListAll"] != null && Convert.ToBoolean(this.Session["IsListAll"]);
            this.LoadDocuments(false, isListAll);
        }

        /// <summary>
        /// The grd khach hang_ delete command.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var docId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            var docObj = this.documentPackageService.GetById(docId);
            if (docObj != null)
            {
                if (docObj.ParentId == null)
                {
                    docObj.IsDelete = true;
                    this.documentPackageService.Update(docObj);
                }
                else
                {
                    var listRelateDoc = this.documentPackageService.GetAllRelatedDocument(docObj.ParentId.GetValueOrDefault());
                    if (listRelateDoc != null)
                    {
                        foreach (var objDoc in listRelateDoc)
                        {
                            objDoc.IsDelete = true;
                            this.documentPackageService.Update(objDoc);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The grd document_ item command.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CompleteTask")
            {
                GridDataItem item = e.Item as GridDataItem;
                var taskId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());

                var taskObj = this.toDoListService.GetById(taskId);
                taskObj.IsComplete = true;
                this.toDoListService.Update(taskObj);

                if (taskObj.TaskTypeId == 1)
                {
                    // Create task for DC
                    var docObj = this.documentPackageService.GetById(taskObj.DocId.GetValueOrDefault());
                    var projectObj = this.scopeProjectService.GetById(docObj.ProjectId.GetValueOrDefault());
                    var dcUser = this.userService.GetByID(projectObj.DCId.GetValueOrDefault());
                    if (dcUser != null)
                    {
                        var todoItem = new Data.Entities.ToDoList()
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
                            UserId = dcUser.Id,
                            UserName = dcUser.FullName,
                            ActionName = "Send CMS to Design contractor",
                            DeadlineDate = docObj.RevisionActualDate != null ? docObj.RevisionActualDate.Value.AddDays(1) : docObj.RevisionActualDate,
                            IsComplete = false,
                            TaskTypeId = 2,
                            TaksTypeName = "DC Send to design contractor"
                        };

                        this.toDoListService.Insert(todoItem);
                    }
                    // --------------------------------------------------------------------
                }


                this.grdDocument.Rebind();
            }
            if (e.CommandName == RadGrid.RebindGridCommandName)
            {
                this.CustomerMenu.Items[3].Visible = false;
                this.rtvDiscipline.UnselectAllNodes();
                this.grdDocument.Rebind();
            }
        }

        protected void radTreeFolder_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {
            PopulateNodeOnDemand(e, TreeNodeExpandMode.ServerSideCallBack);
        }


        protected void ckbEnableFilter_OnCheckedChanged(object sender, EventArgs e)
        {
            this.grdDocument.AllowFilteringByColumn = ((CheckBox)sender).Checked;
            this.grdDocument.Rebind();
        }

        protected void radTreeFolder_OnNodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = "Images/folderdir16.png";
        }

        private void PopulateNodeOnDemand(RadTreeNodeEventArgs e, TreeNodeExpandMode expandMode)
        {
            var categoryId = this.lblCategoryId.Value;
            var folderPermission =
                this.groupDataPermissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault()).Where(
                    t => t.CategoryIdList == categoryId).Select(t => Convert.ToInt32(t.FolderIdList)).ToList();

            var listFolChild = this.folderService.GetAllByParentId(Convert.ToInt32(e.Node.Value), folderPermission);
            foreach (var folderChild in listFolChild)
            {
                var nodeFolder = new RadTreeNode();
                nodeFolder.Text = folderChild.Name;
                nodeFolder.Value = folderChild.ID.ToString();
                nodeFolder.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                nodeFolder.ImageUrl = "Images/folderdir16.png";
                e.Node.Nodes.Add(nodeFolder);
            }

            e.Node.Expanded = true;
        }

        private void CreateValidation(string formular, ValidationCollection objValidations, int startRow, int endRow, int startColumn, int endColumn)
        {
            // Create a new validation to the validations list.
            Validation validation = objValidations[objValidations.Add()];

            // Set the validation type.
            validation.Type = Aspose.Cells.ValidationType.List;

            // Set the operator.
            validation.Operator = OperatorType.None;

            // Set the in cell drop down.
            validation.InCellDropDown = true;

            // Set the formula1.
            validation.Formula1 = "=" + formular;

            // Enable it to show error.
            validation.ShowError = true;

            // Set the alert type severity level.
            validation.AlertStyle = ValidationAlertType.Stop;

            // Set the error title.
            validation.ErrorTitle = "Error";

            // Set the error message.
            validation.ErrorMessage = "Please select item from the list";

            // Specify the validation area.
            CellArea area;
            area.StartRow = startRow;
            area.EndRow = endRow;
            area.StartColumn = startColumn;
            area.EndColumn = endColumn;

            // Add the validation area.
            validation.AreaList.Add(area);

            ////return validation;
        }

        private bool DownloadByWriteByte(string strFileName, string strDownloadName, bool DeleteOriginalFile)
        {
            try
            {
                //Kiem tra file co ton tai hay chua
                if (!File.Exists(strFileName))
                {
                    return false;
                }
                //Mo file de doc
                FileStream fs = new FileStream(strFileName, FileMode.Open);
                int streamLength = Convert.ToInt32(fs.Length);
                byte[] data = new byte[streamLength + 1];
                fs.Read(data, 0, data.Length);
                fs.Close();

                Response.Clear();
                Response.ClearHeaders();
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Length", data.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strDownloadName);
                Response.BinaryWrite(data);
                if (DeleteOriginalFile)
                {
                    File.SetAttributes(strFileName, FileAttributes.Normal);
                    File.Delete(strFileName);
                }

                Response.Flush();

                Response.End();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private void LoadObjectTree()
        {
            var projectInPermission = this.scopeProjectService.GetAll().OrderBy(t => t.Name).ToList();

            this.ddlProject.DataSource = projectInPermission;
            this.ddlProject.DataTextField = "FullName";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();
            if (this.ddlProject.SelectedItem != null)
            {
                var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);
                this.lblProjectId.Value = projectId.ToString();
                var listDisciplineInPermission = this.disciplineService.GetAllDisciplineOfProject(projectId).OrderBy(t => t.Name).ToList();

                this.rtvDiscipline.DataSource = listDisciplineInPermission;
                this.rtvDiscipline.DataTextField = "FullName";
                this.rtvDiscipline.DataValueField = "ID";
                this.rtvDiscipline.DataFieldID = "ID";
                this.rtvDiscipline.DataBind();

                //this.InitGridContextMenu(projectId);
            }
        }

        /// <summary>
        /// The repair list.
        /// </summary>
        /// <param name="listOptionalTypeDetail">
        /// The list optional type detail.
        /// </param>
        private void RepairList(ref List<OptionalTypeDetail> listOptionalTypeDetail)
        {
            var temp = listOptionalTypeDetail.Where(t => t.ParentId != null).Select(t => t.ParentId).Distinct().ToList();
            var temp2 = listOptionalTypeDetail.Select(t => t.ID).ToList();
            var tempList = new List<OptionalTypeDetail>();
            foreach (var x in temp)
            {
                if (!temp2.Contains(x.Value))
                {
                    tempList.AddRange(listOptionalTypeDetail.Where(t => t.ParentId == x.Value).ToList());
                }
            }

            var listOptionalType = tempList.Where(t => t.OptionalTypeId != null).Select(t => t.OptionalTypeId).Distinct().ToList();

            foreach (var optionalTypeId in listOptionalType)
            {
                var optionalType = this.optionalTypeService.GetById(optionalTypeId.Value);
                var tempOptTypeDetail = new OptionalTypeDetail() { ID = optionalType.ID * 9898, Name = optionalType.Name + "s" };
                listOptionalTypeDetail.Add(tempOptTypeDetail);
                ////tempList.Add(tempOptTypeDetail);
                OptionalType type = optionalType;
                foreach (var optionalTypeDetail in tempList.Where(t => t.OptionalTypeId == type.ID).ToList())
                {
                    optionalTypeDetail.ParentId = tempOptTypeDetail.ID;
                }
            }
        }

        protected void ddlProject_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ////this.CustomerMenu.Items[2].Visible = false;
            int projectId = Convert.ToInt32(this.ddlProject.SelectedValue);
            this.lblProjectId.Value = projectId.ToString();
            this.CustomerMenu.Items[3].Visible = false;

            var listDisciplineInPermission = UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                ? this.disciplineService.GetAllDisciplineOfProject(projectId).OrderBy(t => t.Name).ToList()
                : this.disciplineService.GetAllDisciplineInPermission(UserSession.Current.User.Id, !string.IsNullOrEmpty(this.ddlProject.SelectedValue) ? projectId : 0)
                .OrderBy(t => t.Name).ToList();

            this.rtvDiscipline.DataSource = listDisciplineInPermission;
            this.rtvDiscipline.DataTextField = "Name";
            this.rtvDiscipline.DataValueField = "ID";
            this.rtvDiscipline.DataFieldID = "ID";
            this.rtvDiscipline.DataBind();
            this.grdDocument.Rebind();


        }

        protected void grdDocument_Init(object sender, EventArgs e)
        {
        }

        protected void grdDocument_DataBound(object sender, EventArgs e)
        {
        }

        protected void rtvDiscipline_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            this.grdDocument.CurrentPageIndex = 0;
            this.grdDocument.Rebind();
        }

        /// <summary>
        /// The btn download_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnDownload_Click(object sender, ImageClickEventArgs e)
        {
            var item = ((ImageButton)sender).Parent.Parent as GridDataItem;
            var docId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            var docObj = this.documentNewService.GetById(docId);
            var docPackName = string.Empty;
            if (docObj != null)
            {
                docPackName = docObj.Name;
                var serverDocPackPath = Server.MapPath("~/Exports/DocPack/" + DateTime.Now.ToBinary() + "_" + docObj.Name + "_Pack.rar");

                var attachFiles = this.attachFileService.GetAllByDocId(docId);

                var temp = ZipPackage.CreateFile(serverDocPackPath);

                foreach (var attachFile in attachFiles)
                {
                    if (File.Exists(Server.MapPath(attachFile.FilePath)))
                    {
                        temp.Add(Server.MapPath(attachFile.FilePath));
                    }
                }

                this.DownloadByWriteByte(serverDocPackPath, docPackName + ".rar", true);
            }
        }

        protected void grdDocument_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridFilteringItem)
            {
                var filterItem = (GridFilteringItem)e.Item;
                var selectedProperty = new List<string>();

                var ddlFilterRev = (RadComboBox)filterItem.FindControl("ddlFilterRev");
            }
        }

        protected DateTime? SetPublishDate(GridItem item)
        {
            if (item.OwnerTableView.GetColumn("Index27").CurrentFilterValue == string.Empty)
            {
                return new DateTime?();
            }
            else
            {
                return DateTime.Parse(item.OwnerTableView.GetColumn("Index27").CurrentFilterValue);
            }
        }

        /// <summary>
        /// The bind tree view combobox.
        /// </summary>
        /// <param name="optionalType">
        /// The optional type.
        /// </param>
        /// <param name="ddlObj">
        /// The ddl obj.
        /// </param>
        /// <param name="rtvName">
        /// The rtv name.
        /// </param>
        /// <param name="listOptionalTypeDetailFull">
        /// The list optional type detail full.
        /// </param>
        private void BindTreeViewCombobox(int optionalType, RadComboBox ddlObj, string rtvName, IEnumerable<OptionalTypeDetail> listOptionalTypeDetailFull)
        {
            var rtvobj = (RadTreeView)ddlObj.Items[0].FindControl(rtvName);
            if (rtvobj != null)
            {
                var listOptionalTypeDetail = listOptionalTypeDetailFull.Where(t => t.OptionalTypeId == optionalType).ToList();
                this.RepairList(ref listOptionalTypeDetail);

                rtvobj.DataSource = listOptionalTypeDetail;
                rtvobj.DataFieldParentID = "ParentId";
                rtvobj.DataTextField = "Name";
                rtvobj.DataValueField = "ID";
                rtvobj.DataFieldID = "ID";
                rtvobj.DataBind();
            }
        }

        protected void rtvDiscipline_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = @"Images/discipline.png";
        }

        protected void ddlProject_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.ImageUrl = @"Images/project.png";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void ExportEMDRReportNew()
        {
            var filePath = Server.MapPath("Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\CMDRReportTemplate_New.xls");

            var dataSheet = workbook.Worksheets[1];
            var countCol = 11;
            var totalColAdded = 0;
            var docList = new List<DocumentPackage>();
            var projectName = this.ddlProject.SelectedItem != null
                ? this.ddlProject.SelectedItem.Text.Replace("&", "-")
                : string.Empty;
            var dtFull = new DataTable();
            var projectID = Convert.ToInt32(this.ddlProject.SelectedValue);
            var contractorList = this.contractorService.GetAllByProject(projectID).OrderBy(t => t.TypeID);
            var designContName = string.Empty;
            if (contractorList.Any(t => t.TypeID == 1))
            {
                designContName = contractorList.FirstOrDefault(t => t.TypeID == 1).Name;
            }

            dataSheet.Cells["B5"].PutValue(contractorList.Count(t => t.TypeID != 1));
            dataSheet.Cells["V4"].PutValue(DateTime.Now.ToString("dd/MM/yyyy"));

            dtFull.Columns.AddRange(new[]
            {
                new DataColumn("DocId", typeof (String)),
                new DataColumn("NoIndex", typeof (String)),
                new DataColumn("DocNo", typeof (String)),
                new DataColumn("DocTitle", typeof (String)),
                new DataColumn("RevName", typeof (String)),
                new DataColumn("StatusName", typeof (String)),
                new DataColumn("Critical", typeof (String)),
                new DataColumn("Priority", typeof (String)),
                new DataColumn("Vendor", typeof (String)),
                new DataColumn("ReferenceFrom", typeof (String)),
            });

            foreach (var contractor in contractorList)
            {
                totalColAdded += 1;

                var columnDate = new DataColumn("DateCont_" + contractor.ID, typeof (String));
                var columnTrans = new DataColumn("TransCont_" + contractor.ID, typeof (String));

                dtFull.Columns.Add(columnDate);
                dtFull.Columns.Add(columnTrans);

                if (contractor.TypeID == 1)
                {
                    dataSheet.Cells[4, countCol].PutValue(contractor.Name + " to VSP");
                }
                else
                {
                    dataSheet.Cells[4, countCol].PutValue(contractor.Name + "'s Comment");
                }

                dataSheet.Cells[4, countCol + 1].PutValue("Trans No.");
                countCol += 2;
            }

            foreach (var contractor in contractorList.Where(t => t.TypeID != 1).OrderByDescending(t => t.TypeID))
            {
                totalColAdded += 1;

                var columnCode = new DataColumn("ReviewingCont_" + contractor.ID, typeof(String));
                dtFull.Columns.Add(columnCode);
                dataSheet.Cells[4, countCol].PutValue(contractor.Name + " Reviewing");

                countCol += 1;
            }

            foreach (var contractor in contractorList.Where(t => t.TypeID != 1).OrderByDescending(t => t.TypeID))
            {
                totalColAdded += 1;

                var columnCode = new DataColumn("CodeCont_" + contractor.ID, typeof (String));
                dtFull.Columns.Add(columnCode);

                if (contractor.TypeID == 1)
                {
                    dataSheet.Cells[4, countCol].PutValue(contractor.Name + " Status");
                }
                else
                {
                    dataSheet.Cells[4, countCol].PutValue(contractor.Name + " Code");
                }

                countCol += 1;
            }

            

            dataSheet.Cells[4, countCol].PutValue("Final Code");
            //dataSheet.Cells[4, countCol + 1].PutValue("Complete");
            dataSheet.Cells[4, countCol + 1].PutValue("Weight");

            totalColAdded += 2;

            var columnFinalCode = new DataColumn("FinalCode", typeof (String));
            //var columnComplete = new DataColumn("Complete", typeof(String));

            var columnWeight = new DataColumn("Weight", typeof (String));
            dtFull.Columns.Add(columnFinalCode);
            dtFull.Columns.Add(columnWeight);
            //dtFull.Columns.Add(columnComplete);

            var filename = string.Empty;

            if (this.rtvDiscipline.SelectedNode != null)
            {
                dtFull.Rows.Clear();
                var count = 1;

                var dataRow = dtFull.NewRow();
                dataRow["DocNo"] = this.rtvDiscipline.SelectedNode.Text;
                dtFull.Rows.Add(dataRow);

                docList = this.documentPackageService.GetAllByDiscipline(Convert.ToInt32(this.rtvDiscipline.SelectedNode.Value), false);
                foreach (var document in docList)
                {
                    dataRow = dtFull.NewRow();
                    dataRow["DocId"] = document.ID;
                    dataRow["NoIndex"] = count;
                    dataRow["DocNo"] = document.DocNo;
                    dataRow["DocTitle"] = document.DocTitle;
                    dataRow["RevName"] = document.RevisionName;
                    dataRow["StatusName"] = document.StatusName;

                    dataRow["Critical"] = document.IsCriticalDoc.GetValueOrDefault()
                        ? "x"
                        : string.Empty;
                    dataRow["Priority"] = document.IsPriorityDoc.GetValueOrDefault()
                        ? "x"
                        : string.Empty;
                    dataRow["Vendor"] = document.IsVendorDoc.GetValueOrDefault()
                        ? "x"
                        : string.Empty;

                    dataRow["ReferenceFrom"] = document.ReferenceFromName;

                    dataRow["FinalCode"] = document.FinalCodeName;
                    dataRow["Weight"] = document.Weight + "%";

                    foreach (var contractor in contractorList)
                    {
                        if (contractor.TypeID == 1)
                        {
                            dataRow["DateCont_" + contractor.ID] = document.RevisionActualDate != null
                                                                    ? document.RevisionActualDate.Value.ToString("dd/MM/yyyy")
                                                                    : string.Empty;
                            dataRow["TransCont_" + contractor.ID] = document.RevisionReceiveTransNo;
                        }
                        else
                        {
                            var commentList = this.commentResponseService.GetAllByContDoc(contractor.ID, document.ID)
                                    .OrderByDescending(t => t.ActualReceiveDate).ToList();
                            dataRow["DateCont_" + contractor.ID] = commentList.Any() && commentList[0].ActualReceiveDate != null
                                ? commentList[0].ActualReceiveDate.Value.ToString("dd/MM/yyyy")
                                : string.Empty;
                            dataRow["TransCont_" + contractor.ID] = commentList.Any()
                                ? commentList[0].ReceiveTransNumber
                                : string.Empty;
                            dataRow["CodeCont_" + contractor.ID] = commentList.Any()
                                ? commentList[0].ReceiveCodeName
                                : string.Empty;
                            var reviewStatus = string.Empty;
                            if (commentList.Any())
                            {

                                if ((commentList[0].ManageSendDate == null && commentList[0].ActualReceiveDate == null)
                                    || (commentList[0].ManageSendDate != null && commentList[0].ActualReceiveDate != null)
                                    || commentList[0].IsFinal)
                                {
                                    reviewStatus = designContName;
                                }
                                else if ((commentList[0].ManageSendDate != null && commentList[0].ActualReceiveDate == null))
                                {
                                    reviewStatus = "Reviewing";
                                }
                                else if (document.RevisionActualDate != null
                                         && commentList[0].ManageSendDate == null)
                                {
                                    reviewStatus = "PVCFC";
                                }

                                dataRow["ReviewingCont_" + contractor.ID] = reviewStatus;
                            }
                            else
                            {
                                reviewStatus = document.RevisionActualDate != null ? "PVCFC" : designContName;

                                dataRow["ReviewingCont_" + contractor.ID] = reviewStatus;
                            }
                        }
                    }

                    count += 1;
                    dtFull.Rows.Add(dataRow);
                }

                dataSheet.Cells["A7"].PutValue(dtFull.Rows.Count);
                dataSheet.Cells["E1"].PutValue(projectName);
                dataSheet.Cells.ImportDataTable(dtFull, false, 6, 1, dtFull.Rows.Count, dtFull.Columns.Count, false);

                filename = projectName + "_" + this.rtvDiscipline.SelectedNode.Text + "_" + "EMDR Report " +
                       DateTime.Now.ToString("dd-MM-yyyy") + ".xls";
            }
            else
            {
                var listDisciplineInPermission = UserSession.Current.User.Id == 1
                    ? this.disciplineService.GetAllDisciplineOfProject(projectID).OrderBy(t => t.ID).ToList()
                    : this.disciplineService.GetAllDisciplineInPermission(UserSession.Current.User.Id,
                        !string.IsNullOrEmpty(this.ddlProject.SelectedValue) ? projectID : 0)
                        .OrderBy(t => t.ID).ToList();

                if (listDisciplineInPermission.Count > 0)
                {
                    dtFull.Rows.Clear();
                    docList = this.documentPackageService.GetAllByDisciplineInPermission(listDisciplineInPermission.Select(t => t.ID).ToList(), false).ToList();
                    var count = 1;
                    foreach (var discipline in listDisciplineInPermission)
                    {
                        
                        var dataRow = dtFull.NewRow();
                        dataRow["DocNo"] = discipline.Name;
                        dtFull.Rows.Add(dataRow);

                        var listDocGroupByDiscipline = docList.Where(t => t.DisciplineId == discipline.ID).ToList();
                        foreach (var document in listDocGroupByDiscipline)
                        {
                            dataRow = dtFull.NewRow();
                            dataRow["DocId"] = document.ID;
                            dataRow["NoIndex"] = count;
                            dataRow["DocNo"] = document.DocNo;
                            dataRow["DocTitle"] = document.DocTitle;
                            dataRow["RevName"] = document.RevisionName;
                            dataRow["StatusName"] = document.StatusName;

                            dataRow["Critical"] = document.IsCriticalDoc.GetValueOrDefault()
                                ? "x"
                                : string.Empty;
                            dataRow["Priority"] = document.IsPriorityDoc.GetValueOrDefault()
                                ? "x"
                                : string.Empty;
                            dataRow["Vendor"] = document.IsVendorDoc.GetValueOrDefault()
                                ? "x"
                                : string.Empty;
                            
                            dataRow["ReferenceFrom"] = document.ReferenceFromName;

                            dataRow["FinalCode"] = document.FinalCodeName;
                            dataRow["Weight"] = document.Weight + "%";

                            foreach (var contractor in contractorList)
                            {
                                if (contractor.TypeID == 1)
                                {
                                    dataRow["DateCont_" + contractor.ID] = document.RevisionActualDate != null 
                                                                            ? document.RevisionActualDate.Value.ToString("dd/MM/yyyy")
                                                                            : string.Empty;
                                    dataRow["TransCont_" + contractor.ID] = document.RevisionReceiveTransNo;
                                }
                                else
                                {
                                    var commentList = this.commentResponseService.GetAllByContDoc(contractor.ID, document.ID)
                                            .OrderByDescending(t => t.ActualReceiveDate).ToList();
                                    dataRow["DateCont_" + contractor.ID] = commentList.Any() && commentList[0].ActualReceiveDate != null 
                                        ? commentList[0].ActualReceiveDate.Value.ToString("dd/MM/yyyy")
                                        : string.Empty;
                                    dataRow["TransCont_" + contractor.ID] = commentList.Any()
                                        ? commentList[0].ReceiveTransNumber
                                        : string.Empty;
                                    dataRow["CodeCont_" + contractor.ID] = commentList.Any()
                                        ? commentList[0].ReceiveCodeName
                                        : string.Empty;
                                    var reviewStatus = string.Empty;
                                    if (commentList.Any())
                                    {
                                        
                                        if ((commentList[0].ManageSendDate == null && commentList[0].ActualReceiveDate == null)
                                            || (commentList[0].ManageSendDate != null && commentList[0].ActualReceiveDate != null)
                                            || commentList[0].IsFinal)
                                        {
                                            reviewStatus = designContName;
                                        }
                                        else if ((commentList[0].ManageSendDate != null && commentList[0].ActualReceiveDate == null))
                                        {
                                            reviewStatus = "Reviewing";
                                        }
                                        else if (document.RevisionActualDate != null
                                                 && commentList[0].ManageSendDate == null)
                                        {
                                            reviewStatus = "PVCFC";
                                        }

                                        dataRow["ReviewingCont_" + contractor.ID] = reviewStatus;
                                    }
                                    else
                                    {
                                        reviewStatus = document.RevisionActualDate != null ? "PVCFC" : designContName;

                                        dataRow["ReviewingCont_" + contractor.ID] = reviewStatus;
                                    }
                                }
                            }

                            count += 1;
                            dtFull.Rows.Add(dataRow);
                        }
                    }
                    
                    dataSheet.Cells["A7"].PutValue(dtFull.Rows.Count);
                    dataSheet.Cells["E1"].PutValue("DETAIL ENGINEERING SERVICE FOR " + projectName);
                    dataSheet.Cells.ImportDataTable(dtFull, false, 6, 1, dtFull.Rows.Count, dtFull.Columns.Count, false);

                    filename = projectName + "_" + "EMDR Report " +
                           DateTime.Now.ToString("dd-MM-yyyy") + ".xls";
                }
            }

            
            workbook.Save(filePath + filename);
            this.DownloadByWriteByte(filePath + filename, filename, true);

            //if (this.rtvDiscipline.SelectedNode != null)
            //{
            //    var sheets = workbook.Worksheets;

            //    var DisciplineId = Convert.ToInt32(this.rtvDiscipline.SelectedNode.Value);
            //    var Discipline = this.disciplineService.GetById(DisciplineId);
            //    if (Discipline != null)
            //    {
            //        sheets[0].Name = Discipline.Name;
            //        sheets[0].Cells["C7"].PutValue(Discipline.Name);
            //        sheets[0].Cells["B7"].PutValue(this.ddlProject.SelectedValue + "," + DisciplineId);
            //        sheets[0].Cells["M4"].PutValue(DateTime.Now.ToString("dd/MM/yyyy"));
            //        docList =
            //            this.documentPackageService.GetAllEMDRByDiscipline(DisciplineId, false)
            //                .OrderBy(t => t.DocNo)
            //                .ToList();

            //        var count = 1;

            //        var listDocumentTypeId =
            //            docList.Select(t => t.DocumentTypeId).Distinct().OrderBy(t => t).ToList();
            //        foreach (var documentTypeId in listDocumentTypeId)
            //        {
            //            var documentType = this.documentTypeService.GetById(documentTypeId.GetValueOrDefault());

            //            var dataRow = dtDiscipline.NewRow();
            //            dataRow["DocId"] = -1;
            //            dataRow["NoIndex"] = documentType != null ? documentType.FullName : string.Empty;
            //            dtDiscipline.Rows.Add(dataRow);

            //            var listDocByDocType = docList.Where(t => t.DocumentTypeId == documentTypeId).ToList();
            //            foreach (var document in listDocByDocType)
            //            {
            //                dataRow = dtDiscipline.NewRow();
            //                dataRow["DocId"] = document.ID;
            //                dataRow["NoIndex"] = count;
            //                dataRow["DocNo"] = document.DocNo;
            //                dataRow["DocTitle"] = document.DocTitle;
            //                dataRow["Start"] = (object) document.StartDate ?? DBNull.Value;
            //                dataRow["RevName"] = document.RevisionName;
            //                dataRow["RevPlanned"] = (object) document.RevisionPlanedDate ?? DBNull.Value;
            //                dataRow["RevActual"] = (object) document.RevisionActualDate ?? DBNull.Value;
            //                dataRow["Complete"] = document.Complete/100;
            //                dataRow["Weight"] = document.Weight/100;
            //                dataRow["Department"] = document.DeparmentName;
            //                dataRow["Notes"] = document.Notes;
            //                dataRow["IsEMDR"] = document.IsEMDR.GetValueOrDefault() ? "x" : string.Empty;
            //                dataRow["HasAttachFile"] = document.HasAttachFile ? "x" : string.Empty;
            //                dataRow["OutgoingNo"] = document.OutgoingTransNo;
            //                dataRow["OutgoingDate"] = document.OutgoingTransDate != null
            //                    ? document.OutgoingTransDate.Value.ToString("dd/MM/yyyy")
            //                    : string.Empty;
            //                count += 1;
            //                dtDiscipline.Rows.Add(dataRow);
            //            }
            //        }

            //        sheets[0].Cells["A7"].PutValue(dtDiscipline.Rows.Count);

            //        sheets[0].Cells.ImportDataTable(dtDiscipline, false, 7, 1, dtDiscipline.Rows.Count, 16, true);
            //        sheets[1].Cells.ImportDataTable(dtDiscipline, false, 7, 1, dtDiscipline.Rows.Count, 16, true);

            //        sheets[0].Cells[7 + dtDiscipline.Rows.Count, 2].PutValue("Total");

            //        var txtDisciplineComplete =
            //            this.CustomerMenu.Items[3].FindControl("txtDisciplineComplete") as RadNumericTextBox;
            //        var txtDisciplineWeight =
            //            this.CustomerMenu.Items[3].FindControl("txtDisciplineWeight") as RadNumericTextBox;
            //        if (txtDisciplineComplete != null)
            //        {
            //            sheets[0].Cells[7 + dtDiscipline.Rows.Count, 9].PutValue(txtDisciplineComplete.Value/100);
            //        }

            //        if (txtDisciplineWeight != null)
            //        {
            //            sheets[0].Cells[7 + dtDiscipline.Rows.Count, 10].PutValue(txtDisciplineWeight.Value/100);
            //        }
            //        sheets[0].AutoFitRows();
            //        sheets[1].IsVisible = false;

            //        var filename = projectName + " - " + Discipline.Name + " EMDR Report " +
            //                       DateTime.Now.ToString("dd-MM-yyyy") + ".xls";
            //        workbook.Save(filePath + filename);
            //        this.DownloadByWriteByte(filePath + filename, filename, true);

            //    }
            //}
            //else
            //{
            //    var listDisciplineInPermission = UserSession.Current.User.Id == 1
            //        ? this.disciplineService.GetAllDisciplineOfProject(Convert.ToInt32(this.ddlProject.SelectedValue))
            //            .OrderBy(t => t.ID)
            //            .ToList()
            //        : this.disciplineService.GetAllDisciplineInPermission(UserSession.Current.User.Id,
            //            !string.IsNullOrEmpty(this.ddlProject.SelectedValue)
            //                ? Convert.ToInt32(this.ddlProject.SelectedValue)
            //                : 0)
            //            .OrderBy(t => t.ID).ToList();

            //    if (listDisciplineInPermission.Count > 0)
            //    {
            //        var templateManagement = this.templateManagementService.GetSpecial(2,
            //            projectID);
            //        if (templateManagement != null)
            //        {
            //            workbook.Open(Server.MapPath(templateManagement.FilePath));

            //            var totalDoc = 0;
            //            var totalDocIssues = 0;
            //            var totalDocRev0Issues = 0;
            //            var totalDocRev1Issues = 0;
            //            var totalDocRev2Issues = 0;
            //            var totalDocRev3Issues = 0;
            //            var totalDocRev4Issues = 0;
            //            var totalDocRev5Issues = 0;
            //            var totalDocRevIssues = 0;

            //            var totalDocDontIssues = 0;

            //            var sheets = workbook.Worksheets;
            //            var wsSummary = sheets[0];
            //            wsSummary.Cells.InsertRows(7, listDisciplineInPermission.Count - 1);

            //            for (int i = 0; i < listDisciplineInPermission.Count; i++)
            //            {
            //                dtFull.Rows.Clear();

            //                sheets.AddCopy(1);

            //                sheets[i + 2].Name = listDisciplineInPermission[i].Name;
            //                sheets[i + 2].Cells["V4"].PutValue(DateTime.Now.ToString("dd/MM/yyyy"));
            //                sheets[i + 2].Cells["C7"].PutValue(listDisciplineInPermission[i].Name);

            //                // Add hyperlink
            //                var linkName = listDisciplineInPermission[i].Name;
            //                wsSummary.Cells["B" + (7 + i)].PutValue(linkName);
            //                wsSummary.Hyperlinks.Add("B" + (7 + i), 1, 1,
            //                    "'" + listDisciplineInPermission[i].Name + "'" + "!D7");

            //                docList =
            //                    this.documentPackageService.GetAllEMDRByDiscipline(listDisciplineInPermission[i].ID,
            //                        false).OrderBy(t => t.DocNo).ToList();
            //                var docListHasAttachFile = docList.Where(t => t.HasAttachFile);
            //                var wgDoc = docList.Count;
            //                var wgDocIssues = docListHasAttachFile.Count();
            //                var wgDocRev0Issues = docListHasAttachFile.Count(t => t.RevisionName == "0");
            //                var wgDocRev1Issues = docListHasAttachFile.Count(t => t.RevisionName == "1");
            //                var wgDocRev2Issues = docListHasAttachFile.Count(t => t.RevisionName == "2");
            //                var wgDocRev3Issues = docListHasAttachFile.Count(t => t.RevisionName == "3");
            //                var wgDocRev4Issues = docListHasAttachFile.Count(t => t.RevisionName == "4");
            //                var wgDocRev5Issues = docListHasAttachFile.Count(t => t.RevisionName == "5");
            //                var wgTotalDocRev = wgDocRev0Issues + wgDocRev1Issues + wgDocRev2Issues +
            //                                    wgDocRev3Issues + wgDocRev4Issues + wgDocRev5Issues;
            //                var wgDocDontIssues = wgDoc - wgDocIssues;

            //                totalDoc += wgDoc;
            //                totalDocIssues += wgDocIssues;
            //                totalDocRev0Issues += wgDocRev0Issues;
            //                totalDocRev1Issues += wgDocRev1Issues;
            //                totalDocRev2Issues += wgDocRev2Issues;
            //                totalDocRev3Issues += wgDocRev3Issues;
            //                totalDocRev4Issues += wgDocRev4Issues;
            //                totalDocRev5Issues += wgDocRev5Issues;
            //                totalDocRevIssues += wgTotalDocRev;
            //                totalDocDontIssues = totalDoc - totalDocIssues;

            //                wsSummary.Cells["C" + (7 + i)].PutValue(wgDoc);
            //                wsSummary.Cells["D" + (7 + i)].PutValue(wgDocDontIssues);
            //                wsSummary.Cells["E" + (7 + i)].PutValue(wgDocRev0Issues);
            //                wsSummary.Cells["F" + (7 + i)].PutValue(wgDocRev1Issues);
            //                wsSummary.Cells["G" + (7 + i)].PutValue(wgDocRev2Issues);
            //                wsSummary.Cells["H" + (7 + i)].PutValue(wgDocRev3Issues);
            //                wsSummary.Cells["I" + (7 + i)].PutValue(wgDocRev4Issues);
            //                wsSummary.Cells["J" + (7 + i)].PutValue(wgDocRev5Issues);
            //                wsSummary.Cells["K" + (7 + i)].PutValue(wgTotalDocRev);
            //                wsSummary.Cells["L" + (7 + i)].PutValue(wgTotalDocRev);


            //                var count = 1;

            //                var listDocumentTypeId =
            //                    docList.Select(t => t.DocumentTypeId).Distinct().OrderBy(t => t).ToList();

            //                double? complete = 0;
            //                double? weight = 0;

            //                foreach (var documentTypeId in listDocumentTypeId)
            //                {
            //                    var documentType = this.documentTypeService.GetById(documentTypeId.GetValueOrDefault());

            //                    var dataRow = dtFull.NewRow();
            //                    dataRow["NoIndex"] = documentType != null ? documentType.FullName : string.Empty;
            //                    dtFull.Rows.Add(dataRow);

            //                    var listDocByDocType =
            //                        docList.Where(t => t.DocumentTypeId == documentTypeId).ToList();
            //                    foreach (var document in listDocByDocType)
            //                    {
            //                        dataRow = dtFull.NewRow();
            //                        dataRow["DocId"] = document.ID;
            //                        dataRow["NoIndex"] = count;
            //                        dataRow["DocNo"] = document.DocNo;
            //                        dataRow["DocTitle"] = document.DocTitle;
            //                        dataRow["Department"] = document.DeparmentName;
            //                        dataRow["Start"] = document.StartDate != null
            //                            ? document.StartDate.Value.ToString("dd/MM/yyyy")
            //                            : string.Empty;
            //                        dataRow["Planned"] = document.PlanedDate != null
            //                            ? document.PlanedDate.Value.ToString("dd/MM/yyyy")
            //                            : string.Empty;
            //                        dataRow["RevName"] = document.RevisionName;
            //                        dataRow["RevPlanned"] = document.RevisionPlanedDate != null
            //                            ? document.RevisionPlanedDate.Value.ToString("dd/MM/yyyy")
            //                            : string.Empty;
            //                        dataRow["RevActual"] = document.RevisionActualDate != null
            //                            ? document.RevisionActualDate.Value.ToString("dd/MM/yyyy")
            //                            : string.Empty;
            //                        dataRow["RevCommentCode"] = document.RevisionCommentCode;
            //                        dataRow["Complete"] = document.Complete/100;
            //                        dataRow["Weight"] = document.Weight/100;
            //                        dataRow["OutgoingNo"] = document.OutgoingTransNo;
            //                        dataRow["OutgoingDate"] = document.OutgoingTransDate != null
            //                            ? document.OutgoingTransDate.Value.ToString("dd/MM/yyyy")
            //                            : string.Empty;
            //                        dataRow["IncomingNo"] = document.IncomingTransNo;
            //                        dataRow["IncomingDate"] = document.IncomingTransDate != null
            //                            ? document.IncomingTransDate.Value.ToString("dd/MM/yyyy")
            //                            : string.Empty;
            //                        dataRow["ICANo"] = document.ICAReviewOutTransNo;
            //                        dataRow["ICADate"] = document.ICAReviewReceivedDate != null
            //                            ? document.ICAReviewReceivedDate.Value.ToString("dd/MM/yyyy")
            //                            : string.Empty;
            //                        dataRow["ICAReviewCode"] = document.ICAReviewCode;
            //                        dataRow["Notes"] = document.Notes;
            //                        dataRow["IsEMDR"] = document.IsEMDR.GetValueOrDefault() ? "x" : string.Empty;
            //                        dataRow["HasAttachFile"] = document.HasAttachFile ? "x" : string.Empty;

            //                        count += 1;
            //                        dtFull.Rows.Add(dataRow);

            //                        complete += (document.Complete/100)*(document.Weight/100);
            //                        weight += document.Weight/100;
            //                    }
            //                }

            //                sheets[i + 2].Cells["A7"].PutValue(dtFull.Rows.Count);
            //                sheets[i + 2].Cells.ImportDataTable(dtFull, false, 7, 1, dtFull.Rows.Count, 23, true);

            //                sheets[i + 2].Cells[7 + dtFull.Rows.Count, 2].PutValue("Total");

            //                sheets[i + 2].Cells[7 + dtFull.Rows.Count, 12].PutValue(complete);
            //                sheets[i + 2].Cells[7 + dtFull.Rows.Count, 13].PutValue(weight);

            //            }

            //            wsSummary.Cells["H4"].PutValue(DateTime.Now.ToString("dd/MM/yyyy"));

            //            wsSummary.Cells["C" + (7 + listDisciplineInPermission.Count)].PutValue(totalDoc);
            //            wsSummary.Cells["D" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocDontIssues);
            //            wsSummary.Cells["E" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocRev0Issues);
            //            wsSummary.Cells["F" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocRev1Issues);
            //            wsSummary.Cells["G" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocRev2Issues);
            //            wsSummary.Cells["H" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocRev3Issues);
            //            wsSummary.Cells["I" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocRev4Issues);
            //            wsSummary.Cells["J" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocRev5Issues);
            //            wsSummary.Cells["K" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocRevIssues);
            //            wsSummary.Cells["L" + (7 + listDisciplineInPermission.Count)].PutValue(totalDocRevIssues);

            //            sheets[1].IsVisible = false;

            //            var filename = projectName + " - " + "EMDR Report " +
            //                           DateTime.Now.ToString("dd-MM-yyyy") + ".xls";
            //            workbook.Save(filePath + filename);
            //            this.DownloadByWriteByte(filePath + filename, filename, true);
            //        }
            //    }
            //}
        }

        protected void ckbShowAll_CheckedChange(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }
    }
}