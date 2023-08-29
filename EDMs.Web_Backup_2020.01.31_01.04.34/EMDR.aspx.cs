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

    using CheckBox = System.Web.UI.WebControls.CheckBox;
    using Label = System.Web.UI.WebControls.Label;
    using TextBox = System.Web.UI.WebControls.TextBox;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class EMDR : Page
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

        private readonly StatusService statusService = new StatusService();

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
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            Session.Add("SelectedMainMenu", "Document Management");

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

                    this.grdDocument.MasterTableView.GetColumn("IsSelected").Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("EditColumn").Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                }

                if (UserSession.Current.User.Role.IsAdmin.GetValueOrDefault())
                {
                    this.IsFullPermission.Value = "true";
                }


                //this.LoadListPanel();
                //this.LoadSystemPanel();
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
            var projectId = this.ddlProject.SelectedItem != null ? Convert.ToInt32(this.ddlProject.SelectedValue) : 0;
            var docList = new List<DocumentPackage>();
            if (this.rtvDiscipline.SelectedNode != null)
            {
                docList = this.documentPackageService.GetAllByDiscipline(Convert.ToInt32(this.rtvDiscipline.SelectedNode.Value), cbShowAll.Checked)
                        .OrderBy(t => t.DocNo)
                        .ToList();

                var txtDisciplineComplete = this.CustomerMenu.Items[3].FindControl("txtDisciplineComplete") as RadNumericTextBox;
                var txtDisciplineWeight = this.CustomerMenu.Items[3].FindControl("txtDisciplineWeight") as RadNumericTextBox;

                if (txtDisciplineComplete != null)
                {
                    double? complete = 0;
                    complete = docList.Aggregate(complete, (current, t) => current + (t.Complete * t.Weight) / 100);
                    txtDisciplineComplete.Value = complete;
                }

                if (txtDisciplineWeight != null)
                {
                    double? weight = 0;
                    weight = docList.Aggregate(weight, (current, t) => current + t.Weight);
                    txtDisciplineWeight.Value = weight;
                }
            }
            else
            {
                docList = this.documentPackageService.GetAllByProject(projectId, cbShowAll.Checked)
                    .OrderBy(t => t.DocNo)
                        .ToList();

                this.CustomerMenu.Items[3].Visible = false;
            }

            this.grdDocument.DataSource = docList.Where(t => !t.IsInternalDocument.GetValueOrDefault()).OrderBy(t => t.DocNo);
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
                this.ExportCMDRDataFile();
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
            else if (e.Argument == "ExportMasterList")
            {
                var projectId = !string.IsNullOrEmpty(this.ddlProject.SelectedValue)
                    ? Convert.ToInt32(this.ddlProject.SelectedValue)
                    : 0;
                var projectObj = this.scopeProjectService.GetById(projectId);

                var listDisciplineInPermission = UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                    ? this.disciplineService.GetAllDisciplineOfProject(projectId).OrderBy(t => t.ID).ToList()
                    : this.disciplineService.GetAllDisciplineInPermission(UserSession.Current.User.Id,projectId).OrderBy(t => t.ID).ToList();

                var filePath = Server.MapPath("Exports") + @"\";
                if (listDisciplineInPermission.Count > 0)
                {
                    var workbook = new Workbook();
                    workbook.Open(filePath + @"Template\NPK_MasterListTemplate.xlsm");
                    var revisionList = this.revisionService.GetAllByProject(projectId);
                    var docNumneringList = this.documentNumberingService.GetAllByProject(projectId).OrderBy(t => t.Name).ToList();
                    var originatorList = this.originatorService.GetAllByProject(projectId).OrderBy(t => t.Name).ToList();
                    var disciplineList = this.disciplineService.GetAllDisciplineOfProject(projectId).OrderBy(t => t.Name).ToList();
                    var docTypeList = this.documentTypeService.GetAll().OrderBy(t => t.Name).ToList();

                    var sheets = workbook.Worksheets;
                    var tempDataSheet = sheets[0];
                    var rangeRevisionList = tempDataSheet.Cells.CreateRange("A1", "A" + (revisionList.Count == 0 ? 1 : revisionList.Count));
                    var rangeDocNumberingList = tempDataSheet.Cells.CreateRange("B1", "B" + (docNumneringList.Count == 0 ? 1 : docNumneringList.Count));
                    var rangeProject = tempDataSheet.Cells.CreateRange("C1", "C1");

                    var rangeOriginatorList = tempDataSheet.Cells.CreateRange("D1", "D" + (originatorList.Count == 0 ? 1 : originatorList.Count));
                    var rangeDisciplineList = tempDataSheet.Cells.CreateRange("E1", "E" + (disciplineList.Count == 0 ? 1 : disciplineList.Count));
                    var rangeDocTypeList = tempDataSheet.Cells.CreateRange("F1", "F" + (docTypeList.Count == 0 ? 1 : docTypeList.Count));



                    rangeRevisionList.Name = "RevisionList";
                    rangeDocTypeList.Name = "DocTypeList";
                    rangeDisciplineList.Name = "DisciplineList";
                    rangeProject.Name = "ProjectCode";
                    rangeDocNumberingList.Name = "DocNumberingList";
                    rangeOriginatorList.Name = "OriginatorList";
                    

                    for (int j = 0; j < revisionList.Count; j++)
                    {
                        rangeRevisionList[j, 0].PutValue(revisionList[j].Name);
                    }

                    rangeProject[0,0].PutValue(projectObj.Name);

                    for (int j = 0; j < docNumneringList.Count; j++)
                    {
                        rangeDocNumberingList[j, 0].PutValue(docNumneringList[j].Name);
                    }

                    for (int j = 0; j < originatorList.Count; j++)
                    {
                        rangeOriginatorList[j, 0].PutValue(originatorList[j].Name);
                    }

                    for (int j = 0; j < disciplineList.Count; j++)
                    {
                        rangeDisciplineList[j, 0].PutValue(disciplineList[j].Name);
                    }

                    for (int j = 0; j < docTypeList.Count; j++)
                    {
                        rangeDocTypeList[j, 0].PutValue(docTypeList[j].Name);
                    }




                    for (int i = 0; i < listDisciplineInPermission.Count; i++)
                    {
                        sheets.AddCopy("Sheet1");
                        sheets[i + 2].Name = listDisciplineInPermission[i].Name;
                        sheets[i + 2].Cells["A1"].PutValue(this.ddlProject.SelectedValue);
                        sheets[i + 2].Cells["D1"].PutValue(this.ddlProject.SelectedItem.Text);
                        sheets[i + 2].Cells["D3"].PutValue("Discipline: " + listDisciplineInPermission[i].FullName);
                        sheets[i + 2].Cells["A2"].PutValue(listDisciplineInPermission[i].ID);

                        var validations = sheets[i + 2].Validations;
                        this.CreateValidation(rangeRevisionList.Name, validations, 5, 3000, 4, 4);
                        this.CreateValidation(rangeDocNumberingList.Name, validations, 5, 3000, 5, 5);
                        this.CreateValidation(rangeProject.Name, validations, 5, 3000, 6, 6);
                        this.CreateValidation(rangeDisciplineList.Name, validations, 5, 3000, 8, 8);
                        this.CreateValidation(rangeDocTypeList.Name, validations, 5, 3000, 9, 9);
                        this.CreateValidation(rangeOriginatorList.Name, validations, 5, 3000, 7, 7);
                    }

                    workbook.Worksheets.RemoveAt(1);
                    workbook.Worksheets[0].IsVisible = false;

                    var filename = this.ddlProject.SelectedItem.Text + "$" + "MasterListTemplate.xls";
                    workbook.Save(filePath + filename);
                    this.Download_File(filePath + filename);
                }
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
                            this.Download_File(filePath + filename);
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
                            this.Download_File(filePath + filename);
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
                //var serverTotalDocPackPath =
                //    Server.MapPath("~/Exports/DocPack/" + DateTime.Now.ToBinary() + "_DocPack.rar");
                //var docPack = ZipPackage.CreateFile(serverTotalDocPackPath);

                //foreach (GridDataItem item in this.grdDocument.MasterTableView.Items)
                //{
                //    var cboxSelected = (CheckBox) item["IsSelected"].FindControl("IsSelected");
                //    if (cboxSelected.Checked)
                //    {
                //        var docId = Convert.ToInt32(item.GetDataKeyValue("ID"));

                //        var name = (Label) item["Index1"].FindControl("lblName");
                //        var serverDocPackPath =
                //            Server.MapPath("~/Exports/DocPack/" + name.Text + "_" +
                //                           DateTime.Now.ToString("ddMMyyyhhmmss") + ".rar");

                //        var attachFiles = this.attachFileService.GetAllByDocId(docId);

                //        var temp = ZipPackage.CreateFile(serverDocPackPath);

                //        foreach (var attachFile in attachFiles)
                //        {
                //            if (File.Exists(Server.MapPath(attachFile.FilePath)))
                //            {
                //                temp.Add(Server.MapPath(attachFile.FilePath));
                //            }
                //        }

                //        docPack.Add(serverDocPackPath);
                //    }
                //}

                //this.Download_File(filePath + filename);

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
            if (e.CommandName == "RebindGrid")
            {

            }
            if (e.CommandName == RadGrid.RebindGridCommandName)
            {
                this.CustomerMenu.Items[3].Visible = false;
                this.rtvDiscipline.UnselectAllNodes();
                this.grdDocument.Rebind();
            }
            else if (e.CommandName == RadGrid.ExportToExcelCommandName)
            {

            }
        }

        /// <summary>
        /// The grd document_ item data bound.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridFilteringItem)
            {

                ////Populate Filters by binding the combo to datasource
                //var filteringItem = (GridFilteringItem)e.Item;
                //var myRadComboBox = (RadComboBox)filteringItem.FindControl("RadComboBoxCustomerProgramDescription");

                //myRadComboBox.DataSource = myDataSet;
                //myRadComboBox.DataTextField = "CustomerProgramDescription";
                //myRadComboBox.DataValueField = "CustomerProgramDescription";
                //myRadComboBox.ClearSelection();
                //myRadComboBox.DataBind();
            }
            if (e.Item is GridDataItem)
            {
                var item = e.Item as GridDataItem;
                if (item["HasAttachFile"].Text == "True")
                {
                    item.BackColor = Color.Aqua;
                    item.BorderColor = Color.Aqua;
                }
            }

            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                var item = e.Item as GridEditableItem;
                var lbldocNo = item.FindControl("lbldocNo") as Label;
                var txtDocTitle = item.FindControl("txtDocTitle") as TextBox;
                var ddlDepartment = item.FindControl("ddlDepartment") as RadComboBox;
                var txtStartDate = item.FindControl("txtStartDate") as RadDatePicker;
                var txtPlanedDate = item.FindControl("txtPlanedDate") as RadDatePicker;

                var ddlRevision = item.FindControl("ddlRevision") as RadComboBox;
                var txtRevisionPlanedDate = item.FindControl("txtRevisionPlanedDate") as RadDatePicker;
                var txtRevisionActualDate = item.FindControl("txtRevisionActualDate") as RadDatePicker;
                var txtRevisionCommentCode = item.FindControl("txtRevisionCommentCode") as TextBox;

                var txtComplete = item.FindControl("txtComplete") as RadNumericTextBox;
                var txtWeight = item.FindControl("txtWeight") as RadNumericTextBox;

                var txtOutgoingTransNo = item.FindControl("txtOutgoingTransNo") as TextBox;
                var txtOutgoingTransDate = item.FindControl("txtOutgoingTransDate") as RadDatePicker;

                var txtIncomingTransNo = item.FindControl("txtIncomingTransNo") as TextBox;
                var txtIncomingTransDate = item.FindControl("txtIncomingTransDate") as RadDatePicker;

                var txtICAReviewOutTransNo = item.FindControl("txtICAReviewOutTransNo") as TextBox;
                var txtICAReviewReceivedDate = item.FindControl("txtICAReviewReceivedDate") as RadDatePicker;
                var txtICAReviewCode = item.FindControl("txtICAReviewCode") as TextBox;

                var cbIsEMDR = item.FindControl("cbIsEMDR") as CheckBox;


                var listRevision = this.revisionService.GetAll();
                listRevision.Insert(0, new Revision() { ID = 0 });
                if (ddlRevision != null)
                {
                    ddlRevision.DataSource = listRevision;
                    ddlRevision.DataTextField = "Name";
                    ddlRevision.DataValueField = "ID";
                    ddlRevision.DataBind();
                }

                if (txtStartDate != null)
                {
                    txtStartDate.DatePopupButton.Visible = false;
                }

                if (txtPlanedDate != null)
                {
                    txtPlanedDate.DatePopupButton.Visible = false;
                }

                if (txtRevisionPlanedDate != null)
                {
                    txtRevisionPlanedDate.DatePopupButton.Visible = false;
                }

                if (txtRevisionActualDate != null)
                {
                    txtRevisionActualDate.DatePopupButton.Visible = false;
                }

                if (txtOutgoingTransDate != null)
                {
                    txtOutgoingTransDate.DatePopupButton.Visible = false;
                }

                if (txtIncomingTransDate != null)
                {
                    txtIncomingTransDate.DatePopupButton.Visible = false;
                }

                if (txtICAReviewReceivedDate != null)
                {
                    txtICAReviewReceivedDate.DatePopupButton.Visible = false;
                }



                var docNo = (item.FindControl("DocNo") as HiddenField).Value;
                var docTitle = (item.FindControl("DocTitle") as HiddenField).Value;
                var deparmentId = (item.FindControl("DeparmentId") as HiddenField).Value;
                var startDate = (item.FindControl("StartDate") as HiddenField).Value;
                var planedDate = (item.FindControl("PlanedDate") as HiddenField).Value;
                var revisionId = (item.FindControl("RevisionId") as HiddenField).Value;
                var revisionPlanedDate = (item.FindControl("RevisionPlanedDate") as HiddenField).Value;
                var revisionActualDate = (item.FindControl("RevisionActualDate") as HiddenField).Value;
                var revisionCommentCode = (item.FindControl("RevisionCommentCode") as HiddenField).Value;
                var complete = (item.FindControl("Complete") as HiddenField).Value;
                var weight = (item.FindControl("Weight") as HiddenField).Value;

                var OutgoingTransNo = (item.FindControl("OutgoingTransNo") as HiddenField).Value;
                var OutgoingTransDate = (item.FindControl("OutgoingTransDate") as HiddenField).Value;
                var IncomingTransNo = (item.FindControl("IncomingTransNo") as HiddenField).Value;
                var IncomingTransDate = (item.FindControl("IncomingTransDate") as HiddenField).Value;
                var ICAReviewOutTransNo = (item.FindControl("ICAReviewOutTransNo") as HiddenField).Value;
                var ICAReviewReceivedDate = (item.FindControl("ICAReviewReceivedDate") as HiddenField).Value;
                var ICAReviewCode = (item.FindControl("ICAReviewCode") as HiddenField).Value;


                var isEMDR = (item.FindControl("IsEMDR") as HiddenField).Value;

                if (!string.IsNullOrEmpty(startDate))
                {
                    txtStartDate.SelectedDate = Convert.ToDateTime(startDate);
                }

                if (!string.IsNullOrEmpty(planedDate))
                {
                    txtPlanedDate.SelectedDate = Convert.ToDateTime(planedDate);
                }

                if (!string.IsNullOrEmpty(revisionPlanedDate))
                {
                    txtRevisionPlanedDate.SelectedDate = Convert.ToDateTime(revisionPlanedDate);
                }

                if (!string.IsNullOrEmpty(revisionActualDate))
                {
                    txtRevisionActualDate.SelectedDate = Convert.ToDateTime(revisionActualDate);
                }

                lbldocNo.Text = docNo;
                txtDocTitle.Text = docTitle;

                var departmentList = this.roleService.GetAll(false);

                if (ddlDepartment != null)
                {
                    departmentList.Insert(0, new Role { Id = 0 });
                    ddlDepartment.DataSource = departmentList;
                    ddlDepartment.DataTextField = "Name";
                    ddlDepartment.DataValueField = "Id";
                    ddlDepartment.DataBind();

                    ddlDepartment.SelectedValue = deparmentId;
                }

                ddlRevision.SelectedValue = revisionId;
                txtRevisionCommentCode.Text = revisionCommentCode;
                txtComplete.Value = Convert.ToDouble(complete);
                txtWeight.Value = Convert.ToDouble(weight);

                txtOutgoingTransNo.Text = OutgoingTransNo;
                if (!string.IsNullOrEmpty(OutgoingTransDate))
                {
                    txtOutgoingTransDate.SelectedDate = Convert.ToDateTime(OutgoingTransDate);
                }

                txtIncomingTransNo.Text = IncomingTransNo;
                if (!string.IsNullOrEmpty(IncomingTransDate))
                {
                    txtIncomingTransDate.SelectedDate = Convert.ToDateTime(IncomingTransDate);
                }

                txtICAReviewOutTransNo.Text = ICAReviewOutTransNo;
                txtICAReviewCode.Text = ICAReviewCode;
                if (!string.IsNullOrEmpty(ICAReviewReceivedDate))
                {
                    txtICAReviewReceivedDate.SelectedDate = Convert.ToDateTime(ICAReviewReceivedDate);
                }

                cbIsEMDR.Checked = Convert.ToBoolean(isEMDR);
            }
        }

        protected void radTreeFolder_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {
            PopulateNodeOnDemand(e, TreeNodeExpandMode.ServerSideCallBack);
        }

        protected void grdDocument_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                var item = e.Item as GridEditableItem;
                var lbldocNo = item.FindControl("lbldocNo") as Label;
                var txtDocTitle = item.FindControl("txtDocTitle") as TextBox;
                var ddlDepartment = item.FindControl("ddlDepartment") as RadComboBox;
                var txtStartDate = item.FindControl("txtStartDate") as RadDatePicker;
                var txtPlanedDate = item.FindControl("txtPlanedDate") as RadDatePicker;

                var ddlRevision = item.FindControl("ddlRevision") as RadComboBox;
                var txtRevisionPlanedDate = item.FindControl("txtRevisionPlanedDate") as RadDatePicker;
                var txtRevisionActualDate = item.FindControl("txtRevisionActualDate") as RadDatePicker;
                var txtRevisionCommentCode = item.FindControl("txtRevisionCommentCode") as TextBox;

                var txtComplete = item.FindControl("txtComplete") as RadNumericTextBox;
                var txtWeight = item.FindControl("txtWeight") as RadNumericTextBox;

                var txtOutgoingTransNo = item.FindControl("txtOutgoingTransNo") as TextBox;
                var txtOutgoingTransDate = item.FindControl("txtOutgoingTransDate") as RadDatePicker;

                var txtIncomingTransNo = item.FindControl("txtIncomingTransNo") as TextBox;
                var txtIncomingTransDate = item.FindControl("txtIncomingTransDate") as RadDatePicker;

                var txtICAReviewOutTransNo = item.FindControl("txtICAReviewOutTransNo") as TextBox;
                var txtICAReviewReceivedDate = item.FindControl("txtICAReviewReceivedDate") as RadDatePicker;
                var txtICAReviewCode = item.FindControl("txtICAReviewCode") as TextBox;

                var cbIsEMDR = item.FindControl("cbIsEMDR") as CheckBox;

                var docId = Convert.ToInt32(item.GetDataKeyValue("ID"));

                var objDoc = this.documentPackageService.GetById(docId);

                var currentRevision = objDoc.RevisionId;
                var newRevision = Convert.ToInt32(ddlRevision.SelectedValue);
                var department = this.roleService.GetByID(Convert.ToInt32(ddlDepartment.SelectedValue));

                var projectid = Convert.ToInt32(this.ddlProject.SelectedValue);
                var projectname = this.ddlProject.SelectedItem.Text;

                if (newRevision > currentRevision)
                {
                    var docObjNew = new DocumentPackage();
                    docObjNew.ProjectId = projectid;
                    docObjNew.ProjectName = projectname;
                    docObjNew.DisciplineId = objDoc.DisciplineId;
                    docObjNew.DisciplineName = objDoc.DisciplineName;
                    docObjNew.DocNo = lbldocNo.Text;
                    docObjNew.DocTitle = txtDocTitle.Text.Trim();
                    docObjNew.DeparmentId = objDoc.DeparmentId;
                    docObjNew.DeparmentName = objDoc.DeparmentName;
                    docObjNew.StartDate = txtStartDate.SelectedDate;

                    docObjNew.PlanedDate = txtPlanedDate.SelectedDate;
                    docObjNew.RevisionId = newRevision;
                    docObjNew.RevisionName = ddlRevision.SelectedItem.Text;
                    docObjNew.RevisionActualDate = txtRevisionActualDate.SelectedDate;
                    docObjNew.RevisionCommentCode = txtRevisionCommentCode.Text.Trim();
                    docObjNew.RevisionPlanedDate = txtRevisionPlanedDate.SelectedDate;
                    docObjNew.Complete = txtComplete.Value.GetValueOrDefault();
                    docObjNew.Weight = txtWeight.Value.GetValueOrDefault();
                    docObjNew.OutgoingTransNo = txtOutgoingTransNo.Text.Trim();
                    docObjNew.OutgoingTransDate = txtOutgoingTransDate.SelectedDate;
                    docObjNew.IncomingTransNo = txtIncomingTransNo.Text.Trim();
                    docObjNew.IncomingTransDate = txtIncomingTransDate.SelectedDate;
                    docObjNew.ICAReviewOutTransNo = txtICAReviewOutTransNo.Text.Trim();
                    docObjNew.ICAReviewCode = txtICAReviewCode.Text.Trim();
                    docObjNew.ICAReviewReceivedDate = txtICAReviewReceivedDate.SelectedDate;

                    docObjNew.DocumentTypeId = objDoc.DocumentTypeId;
                    docObjNew.DocumentTypeName = objDoc.DocumentTypeName;
                    docObjNew.DisciplineId = objDoc.DisciplineId;
                    docObjNew.DisciplineName = objDoc.DisciplineName;
                    docObjNew.PackageId = objDoc.PackageId;
                    docObjNew.PackageName = objDoc.PackageName;
                    docObjNew.Notes = string.Empty;
                    docObjNew.PlatformId = objDoc.PlatformId;
                    docObjNew.PlatformName = objDoc.PlatformName;

                    docObjNew.IsLeaf = true;
                    docObjNew.IsEMDR = cbIsEMDR.Checked;
                    docObjNew.ParentId = objDoc.ParentId ?? objDoc.ID;
                    docObjNew.CreatedBy = UserSession.Current.User.Id;
                    docObjNew.CreatedDate = DateTime.Now;

                    this.documentPackageService.Insert(docObjNew);

                    objDoc.IsLeaf = false;
                }
                else
                {
                    ////objDoc.DocNo = lbldocNo.Text;
                    objDoc.DocTitle = txtDocTitle.Text.Trim();
                    ////objDoc.DeparmentName = department != null ? department.FullName : string.Empty;
                    ////objDoc.DeparmentId = Convert.ToInt32(ddlDepartment.SelectedValue);
                    objDoc.StartDate = txtStartDate.SelectedDate;
                    objDoc.PlanedDate = txtPlanedDate.SelectedDate;
                    objDoc.RevisionId = newRevision;
                    objDoc.RevisionName = ddlRevision.SelectedItem.Text;
                    objDoc.RevisionActualDate = txtRevisionActualDate.SelectedDate;
                    objDoc.RevisionCommentCode = txtRevisionCommentCode.Text.Trim();
                    objDoc.RevisionPlanedDate = txtRevisionPlanedDate.SelectedDate;
                    objDoc.Complete = txtComplete.Value.GetValueOrDefault();
                    objDoc.Weight = txtWeight.Value.GetValueOrDefault();
                    ////objDoc.OutgoingTransNo = txtOutgoingTransNo.Text.Trim();
                    ////objDoc.OutgoingTransDate = txtOutgoingTransDate.SelectedDate;
                    ////objDoc.IncomingTransNo = txtIncomingTransNo.Text.Trim();
                    ////objDoc.IncomingTransDate = txtIncomingTransDate.SelectedDate;
                    ////objDoc.ICAReviewOutTransNo = txtICAReviewOutTransNo.Text.Trim();
                    ////objDoc.ICAReviewCode = txtICAReviewCode.Text.Trim();
                    ////objDoc.ICAReviewReceivedDate = txtICAReviewReceivedDate.SelectedDate;

                    objDoc.IsEMDR = cbIsEMDR.Checked;
                }

                objDoc.UpdatedBy = UserSession.Current.User.Id;
                objDoc.UpdatedDate = DateTime.Now;

                this.documentPackageService.Update(objDoc);
            }
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
            if (UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() || this.permissionDisciplineService.IsFullPermission(UserSession.Current.User.Id,
                Convert.ToInt32(e.Node.Value)))
            {
                this.CustomerMenu.Items[0].Visible = true;
                this.CustomerMenu.Items[1].Visible = true;
                foreach (RadToolBarButton item in ((RadToolBarDropDown)this.CustomerMenu.Items[2]).Buttons)
                {
                    if (item.Value == "Adminfunc")
                    {
                        item.Visible = true;
                    }
                }
                this.grdDocument.MasterTableView.GetColumn("IsSelected").Visible = true;
                this.grdDocument.MasterTableView.GetColumn("EditColumn").Visible = true;
                this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = true;

                this.IsFullPermission.Value = "true";
            }
            else
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

                this.grdDocument.MasterTableView.GetColumn("IsSelected").Visible = false;
                this.grdDocument.MasterTableView.GetColumn("EditColumn").Visible = false;
                this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;

                this.IsFullPermission.Value = "false";
            }

            this.grdDocument.CurrentPageIndex = 0;
            this.grdDocument.Rebind();
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

        private void InitGridContextMenu(int projectId)
        {
            var contractorList = this.contractorService.GetAllByProject(projectId).OrderBy(t => t.TypeID).ThenBy(t => t.Name);
            foreach (var contractor in contractorList)
            {
                if (contractor.TypeID == 1)
                {
                    //this.grdDocument.MasterTableView.ColumnGroups[0].HeaderText = contractor.Name + " - PVCFC";
                }

                this.radMenu.Items.Add(new RadMenuItem()
                {
                    Text = (contractor.TypeID == 1 ? "Response from ": "Comment from ") + contractor.Name,
                    ImageUrl = "~/Images/comment1.png",
                    Value = contractor.TypeID == 1 ? "Response_" + contractor.ID : "Comment_" + contractor.ID,
                    //NavigateUrl = "~/Controls/Document/CommentResponseForm.aspx?contId=" + contractor.ID
                });
            }
        }

        private void ExportCMDRDataFile()
        {
            var filePath = Server.MapPath("Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\NPK_CMDRDataFileTemplate.xlsm");

            var dataSheet = workbook.Worksheets[2];
            var duplicateSheet = workbook.Worksheets[3];
            var tempSheet = workbook.Worksheets[0];
            var countCol = 11;
            var totalColAdded = 0;
            var projectName = this.ddlProject.SelectedItem != null
                ? this.ddlProject.SelectedItem.Text.Replace("&", "-")
                : string.Empty;
            var dtFull = new DataTable();
            var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);
            var projectObj = this.scopeProjectService.GetById(projectId);
            var revisionList = this.revisionService.GetAllByProject(projectId);
            var originatorList = this.originatorService.GetAllByProject(projectId).OrderBy(t => t.Name).ToList();
            var disciplineList = this.disciplineService.GetAllDisciplineOfProject(projectId).OrderBy(t => t.Name).ToList();
            var docTypeList = this.documentTypeService.GetAll().OrderBy(t => t.Name).ToList();
            var statusList = this.statusService.GetAllByProject(projectId).OrderBy(t => t.PercentCompleteDefault).ToList();

            var rangeRevisionList = tempSheet.Cells.CreateRange("A1", "A" + (revisionList.Count == 0 ? 1 : revisionList.Count));
            var rangeProject = tempSheet.Cells.CreateRange("C1", "C1");

            var rangeOriginatorList = tempSheet.Cells.CreateRange("D1", "D" + (originatorList.Count == 0 ? 1 : originatorList.Count));
            var rangeDisciplineList = tempSheet.Cells.CreateRange("E1", "E" + (disciplineList.Count == 0 ? 1 : disciplineList.Count));
            var rangeDocTypeList = tempSheet.Cells.CreateRange("F1", "F" + (docTypeList.Count == 0 ? 1 : docTypeList.Count));
            var rangeStatusList = tempSheet.Cells.CreateRange("G1", "G" + (statusList.Count == 0 ? 1 : statusList.Count));

            rangeDocTypeList.Name = "DocTypeList";
            rangeDisciplineList.Name = "DisciplineList";
            rangeProject.Name = "ProjectCode";
            rangeOriginatorList.Name = "OriginatorList";
            rangeStatusList.Name = "StatusList";
            rangeRevisionList.Name = "RevisionList";


            for (int j = 0; j < revisionList.Count; j++)
            {
                rangeRevisionList[j, 0].PutValue(revisionList[j].Name);
            }

            rangeProject[0, 0].PutValue(projectObj.Name);

            for (int j = 0; j < originatorList.Count; j++)
            {
                rangeOriginatorList[j, 0].PutValue(originatorList[j].Name);
            }

            for (int j = 0; j < disciplineList.Count; j++)
            {
                rangeDisciplineList[j, 0].PutValue(disciplineList[j].Name);
            }

            for (int j = 0; j < docTypeList.Count; j++)
            {
                rangeDocTypeList[j, 0].PutValue(docTypeList[j].Name);
            }

            for (int j = 0; j < statusList.Count; j++)
            {
                rangeStatusList[j, 0].PutValue(statusList[j].Name);
            }

            var filename = projectName.Split('-')[0].Trim() + "_" + "CMDR Data File_Cut off - " + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsm";
            var docList = this.documentPackageService.GetAllByProject(projectId, false);
            if (this.rtvDiscipline.SelectedNode != null)
            {
                docList = docList.Where(t => t.DisciplineId == Convert.ToInt32(this.rtvDiscipline.SelectedNode.Value)).ToList();
                filename = Utility.RemoveSpecialCharacterFileName(this.rtvDiscipline.SelectedNode.Text) + "(" + projectName.Split('-')[0].Trim() + ")" + "_" + "CMDR Data File_Cut off - " + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsm";
            }

            dataSheet.Cells["E1"].PutValue(this.ddlProject.Text);
            dataSheet.Cells["W4"].PutValue(DateTime.Now.ToString("dd/MM/yyyy"));

            dtFull.Columns.AddRange(new[]
            {
                new DataColumn("DocId", typeof (String)),
                new DataColumn("NoIndex", typeof (String)),
                new DataColumn("DocNo", typeof (String)),
                new DataColumn("DocTitle", typeof (String)),
                new DataColumn("RevName", typeof (String)),
                new DataColumn("StatusName", typeof (String)),
                new DataColumn("ProjectCode", typeof (String)),
                new DataColumn("Originator", typeof (String)),
                new DataColumn("DisciplineCode", typeof (String)),
                new DataColumn("DocTypeCode", typeof (String)),
                new DataColumn("SequentialNo", typeof (String)),
                new DataColumn("DrawingSheetNo", typeof (String)),
                new DataColumn("RevReceivedDate", typeof (DateTime)),
                new DataColumn("RevReceivedTransNo", typeof (String)),
                new DataColumn("FirstIssuePlan", typeof (DateTime)),
                new DataColumn("FirstIssueActual", typeof (DateTime)),
                new DataColumn("FirstIssueTransNo", typeof (String)),
                new DataColumn("FinalIssuePlan", typeof (DateTime)),
                new DataColumn("FinalIssueActual", typeof (DateTime)),
                new DataColumn("FinalIssueTransNo", typeof (String)),
                new DataColumn("Complete", typeof (double)),
                new DataColumn("Weight", typeof (double)),
            });

            
            var docGroupByDisciplineList = docList.GroupBy(t => t.DisciplineFullName);
            foreach (var docGroupByDiscipline in docGroupByDisciplineList)
            {
                var disciplineRowCount = 1;
                var docListOfDiscipline = docGroupByDiscipline.ToList();
                var dataRow = dtFull.NewRow();
                dataRow["DocNo"] = docGroupByDiscipline.Key;
                dtFull.Rows.Add(dataRow);
                

                foreach (var docObj in docListOfDiscipline)
                {
                    dataRow = dtFull.NewRow();
                    dataRow["DocId"] = docObj.ID;
                    dataRow["NoIndex"] = disciplineRowCount;
                    dataRow["DocNo"] = docObj.DocNo;
                    dataRow["DocTitle"] = docObj.DocTitle;
                    dataRow["RevName"] = docObj.RevisionName;
                    dataRow["StatusName"] = docObj.StatusName;
                    dataRow["ProjectCode"] = docObj.ProjectName;
                    dataRow["Originator"] = docObj.OriginatorName;
                    dataRow["DisciplineCode"] = docObj.DisciplineName;
                    dataRow["DocTypeCode"] = docObj.DocumentTypeName;
                    dataRow["SequentialNo"] = docObj.SequencetialNumber;
                    dataRow["DrawingSheetNo"] = docObj.DrawingSheetNumber;
                    if (docObj.RevisionActualDate != null)
                    {
                        dataRow["RevReceivedDate"] = docObj.RevisionActualDate;
                    }
                    dataRow["RevReceivedTransNo"] = docObj.RevisionReceiveTransNo;


                    if (docObj.FirstIssuePlanDate != null)
                    {
                        dataRow["FirstIssuePlan"] = docObj.FirstIssuePlanDate;
                    }

                    if (docObj.FirstIssueActualDate != null)
                    {
                        dataRow["FirstIssueActual"] = docObj.FirstIssueActualDate;
                    }
                    dataRow["FirstIssueTransNo"] = docObj.FirstIssueTransNo;
                    if (docObj.FinalIssuePlanDate != null)
                    {
                        dataRow["FinalIssuePlan"] = docObj.FinalIssuePlanDate;
                    }
                    if (docObj.FinalIssueActualDate != null)
                    {
                        dataRow["FinalIssueActual"] = docObj.FinalIssueActualDate;
                    }
                    dataRow["FinalIssueTransNo"] = docObj.FinalIssueTransNo;
                   
                    
                    dataRow["Complete"] = docObj.Complete != null ? docObj.Complete/100 :0;
  
                    dataRow["Weight"] =docObj.Weight != null  ? docObj.Weight/100 :0;


                    disciplineRowCount += 1;
                    dtFull.Rows.Add(dataRow);
                }
            }
            dataSheet.Cells.ImportDataTable(dtFull, false, 7, 1, dtFull.Rows.Count, dtFull.Columns.Count, false);
            duplicateSheet.Cells.ImportDataTable(dtFull, false, 7, 1, dtFull.Rows.Count, dtFull.Columns.Count, false);

            var validations = dataSheet.Validations;
            this.CreateValidation(rangeRevisionList.Name, validations, 7, 3000, 5, 5);
            this.CreateValidation(rangeProject.Name, validations, 7, 3000, 7, 7);
            this.CreateValidation(rangeDisciplineList.Name, validations, 7, 3000, 9, 9);
            this.CreateValidation(rangeDocTypeList.Name, validations, 7, 3000, 10, 10);
            this.CreateValidation(rangeOriginatorList.Name, validations, 7, 3000, 8, 8);
            this.CreateValidation(rangeStatusList.Name, validations, 7, 3000, 6, 6);
            dataSheet.Cells["A7"].PutValue(dtFull.Rows.Count + 7);
            dataSheet.Cells["A1"].PutValue(projectId);

            
            workbook.Save(filePath + filename);
            this.Download_File(filePath + filename);
            
        }

        private void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
        }

        protected void ckbShowAll_CheckedChange(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }
    }
}