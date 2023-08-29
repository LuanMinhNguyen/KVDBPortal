// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
    using System.Web;
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
    using Telerik.Windows.Zip;

    using CheckBox = System.Web.UI.WebControls.CheckBox;
    using Label = System.Web.UI.WebControls.Label;
    using TextBox = System.Web.UI.WebControls.TextBox;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class ViewBySystemTags : Page
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
        /// The status service.
        /// </summary>
        private readonly StatusService statusService = new StatusService();

        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly DisciplineService disciplineService = new DisciplineService();

        /// <summary>
        /// The received from.
        /// </summary>
        private readonly ReceivedFromService receivedFromService = new ReceivedFromService();

        /// <summary>
        /// The language service.
        /// </summary>
        private readonly LanguageService languageService = new LanguageService();

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly FolderService folderService = new FolderService();

        private readonly DocumentService documentService = new DocumentService();

        private readonly DocumentNewService documentNewService = new DocumentNewService();

        private readonly NotificationRuleService notificationRuleService = new NotificationRuleService();

        private readonly GroupDataPermissionService groupDataPermissionService = new GroupDataPermissionService();

        private readonly CategoryService categoryService = new CategoryService();

        private readonly UserService userService = new UserService();

        private readonly AttachFileService attachFileService = new AttachFileService();

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

        private readonly OptionalTypeDetailService optionalTypeDetailService = new OptionalTypeDetailService();

        private readonly  DocPropertiesViewService docPropertiesViewService = new DocPropertiesViewService();

        private readonly OriginatorService originatorService = new OriginatorService();

        private readonly DocPropertiesService docPropertiesService = new DocPropertiesService();

        public int PlantOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("Plant"));
            }
        }

        public int SystempOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("System"));
            }
        }

        public int TagOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("Tagtype"));
            }
        }

        public int DisciplineOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("Discipline"));
            }
        }

        public int DocumentTypeOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("DocumentType"));
            }
        }

        public int ProjectOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("Project"));
            }
        }

        public int BlockOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("Block"));
            }
        }

        public int FieldOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("Field"));
            }
        }

        public int PlatformOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("Platform"));
            }
        }

        public int WellOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("Well"));
            }
        }

        public int RIGOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("RIG"));
            }
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
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            if (!Page.IsPostBack)
            {
                this.LoadObjectTree();
                ////this.LoadDocConfigGridView();
                Session.Add("IsListAll", false);

                if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() && !UserSession.Current.User.Role.IsUpdate.GetValueOrDefault())
                {
                    this.CustomerMenu.Items[0].Visible = false;
                    this.CustomerMenu.Items[1].Visible = false;
                    this.CustomerMenu.Items[2].Visible = false;
                    this.CustomerMenu.Items[3].Visible = false;

                    this.grdDocument.MasterTableView.GetColumn("IsSelected").Visible = false;
                    ////this.grdDocument.MasterTableView.GetColumn("EditColumn").Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                }

                this.LoadListPanel();
                this.LoadSystemPanel();
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
            ////var tempURI = new Uri(originalURL);

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
            var rtvOptionalTypeDetail = (RadTreeView)this.rpbObjTree.FindItemByValue("ObjTree").FindControl("rtvOptionalTypeDetail");
            var pageSize = this.grdDocument.PageSize;
            var currentPage = this.grdDocument.CurrentPageIndex;
            var startingRecordNumber = currentPage * pageSize;
            var listOptionalTypeDetail = new List<OptionalTypeDetail>();
            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));

            var ddlCategory = (RadComboBox)this.rpbObjTree.FindItemByValue("ObjTree").FindControl("ddlCategory");
            if (ddlCategory != null && ddlCategory.Items.Count > 0)
            {
                if (rtvOptionalTypeDetail != null && rtvOptionalTypeDetail.SelectedNode != null)
                {
                    var selectedNodesId =
                        rtvOptionalTypeDetail.SelectedNode.GetAllNodes().Select(t => Convert.ToInt32(t.Value)).ToList();
                    selectedNodesId.Insert(0, Convert.ToInt32(rtvOptionalTypeDetail.SelectedNode.Value));
                    var optionalTypeDetail = this.optionalTypeDetailService.GetById(Convert.ToInt32(rtvOptionalTypeDetail.SelectedNode.Value));
                    if (optionalTypeDetail != null && optionalTypeDetail.OptionalTypeId == this.SystempOptType)
                    {
                        var filterListId =
                            this.optionalTypeDetailService.GetAllChildId(
                                optionalTypeDetail.OptionalTypeId.GetValueOrDefault(), selectedNodesId);

                        listOptionalTypeDetail = this.optionalTypeDetailService
                                                        .GetAllSpecial(ddlCategory.SelectedValue, 0, "0")
                                                        .Where(t => t.OptionalTypeId == this.TagOptType && filterListId.Contains(t.SystemId.GetValueOrDefault())).ToList();
                    }
                    else if (optionalTypeDetail != null && optionalTypeDetail.OptionalTypeId == this.TagOptType)
                    {
                        listOptionalTypeDetail = this.optionalTypeDetailService
                                                        .GetAllSpecial(ddlCategory.SelectedValue, 0, "0")
                                                        .Where(t => t.ID == optionalTypeDetail.ID).ToList();
                    }
                }
                else
                {
                    listOptionalTypeDetail = this.optionalTypeDetailService
                                                    .GetAllSpecial(ddlCategory.SelectedValue, 0, "0")
                                                    .Where(t => t.OptionalTypeId == this.TagOptType && t.SystemId != null).ToList();
                }

                ////this.grdDocument.VirtualItemCount = docList.Count;
                this.grdDocument.DataSource = listOptionalTypeDetail.OrderBy(t => t.SystemName);
                ////this.grdDocument.DataSource = docList.OrderByDescending(t => t.ID).Skip(startingRecordNumber).Take(pageSize);
            }
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
                this.grdDocument.MasterTableView.SortExpressions.Clear();
                this.grdDocument.MasterTableView.GroupByExpressions.Clear();
                this.grdDocument.Rebind();
            }
            else if (e.Argument == "ExportTemplateDataFile")
            {
                var selectedProperty = new List<string>();
                var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
                var ddlCategory = (RadComboBox)this.rpbObjTree.FindItemByValue("ObjTree").FindControl("ddlCategory");
                var deparmentId = isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() ? UserSession.Current.RoleId.ToString() : "0";

                foreach (var docPropertiesView in this.docPropertiesViewService.GetAllSpecial(Convert.ToInt32(ddlCategory.SelectedValue), Convert.ToInt32(deparmentId)))
                {
                    var temp = docPropertiesView.PropertyIndex.Split(',').Where(t => !string.IsNullOrEmpty(t.Trim())).Select(t => t.Trim()).ToList();
                    selectedProperty.AddRange(temp);
                }

                selectedProperty = selectedProperty.Distinct().ToList();
                var listOptionalTypeDetail = this.optionalTypeDetailService.GetAllSpecial(ddlCategory.SelectedValue, 0, deparmentId);
                var listParentId = listOptionalTypeDetail.Select(t => t.ParentId).Distinct().ToList();

                var selectedCategory = this.groupDataPermissionService.GetByRoleId(UserSession.Current.RoleId).Select(t => Convert.ToInt32(t.CategoryIdList)).ToList();

                var listCategory = this.categoryService.GetAll().Where(t => selectedCategory.Contains(t.ID)).ToList();
                var originatorList = this.originatorService.GetAll();
                var revisionList = this.revisionService.GetAll();
                var plantList = listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.PlantOptType && !listParentId.Contains(t.ID)).OrderBy(t => t.Name).ToList();
                var systemList = listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.SystempOptType && !listParentId.Contains(t.ID)).OrderBy(t => t.Name).ToList();
                var disciplineList = listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.DisciplineOptType && !listParentId.Contains(t.ID)).OrderBy(t => t.Name).ToList();
                var documentTypeList = listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.DocumentTypeOptType && !listParentId.Contains(t.ID)).OrderBy(t => t.Name).ToList();
                var tagtypeList = listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.TagOptType && !listParentId.Contains(t.ID)).OrderBy(t => t.Name).ToList();
                var projectList = listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.ProjectOptType && !listParentId.Contains(t.ID)).OrderBy(t => t.Name).ToList();
                var blockList = listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.BlockOptType && t.ParentId != null).OrderBy(t => t.Name).ToList();
                var fieldList = listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.FieldOptType && t.ParentId != null).OrderBy(t => t.Name).ToList();
                var platformList = listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.PlatformOptType && t.ParentId != null).OrderBy(t => t.Name).ToList();
                var wellList = listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.WellOptType && t.ParentId != null).ToList();
                var rigList = listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.RIGOptType && t.ParentId != null).OrderBy(t => t.Name).ToList();

                var filePath = Server.MapPath("Exports") + @"\";

                var workbook = new Workbook();
                workbook.Open(filePath + @"Template\EditDataFileTemplate.xls");

                // Get the first worksheet.
                var worksheet1 = workbook.Worksheets[0];
                var worksheet2 = workbook.Worksheets[1];
                var worksheet3 = workbook.Worksheets[2];

                // Create a range in the second worksheet.
                var rangeCategoryList = worksheet2.Cells.CreateRange("A1", "A" + (listCategory.Count == 0 ? 1 : listCategory.Count));
                var rangeRevisionList = worksheet2.Cells.CreateRange("C1", "C" + (revisionList.Count == 0 ? 1 : revisionList.Count));
                var rangeYearList = worksheet2.Cells.CreateRange("D1", "D" + (DateTime.Now.Year - 1900 + 1));
                var rangePlantList = worksheet2.Cells.CreateRange("E1", "E" + (plantList.Count == 0 ? 1 : plantList.Count));
                var rangeSystemList = worksheet2.Cells.CreateRange("F1", "F" + (systemList.Count == 0 ? 1 : systemList.Count));
                var rangeDisciplineList = worksheet2.Cells.CreateRange("G1", "G" + (disciplineList.Count == 0 ? 1 : disciplineList.Count));
                var rangeDocumentTypeList = worksheet2.Cells.CreateRange("H1", "H" + (documentTypeList.Count == 0 ? 1 : documentTypeList.Count));
                var rangeTagTypeList = worksheet2.Cells.CreateRange("I1", "I" + (tagtypeList.Count == 0 ? 1 : tagtypeList.Count));
                var rangeProjectList = worksheet2.Cells.CreateRange("J1", "J" + (projectList.Count == 0 ? 1 : projectList.Count));
                var rangeBlockList = worksheet2.Cells.CreateRange("K1", "K" + (blockList.Count == 0 ? 1 : blockList.Count));
                var rangeFieldList = worksheet2.Cells.CreateRange("L1", "L" + (fieldList.Count == 0 ? 1 : fieldList.Count));
                var rangePlatformList = worksheet2.Cells.CreateRange("M1", "M" + (platformList.Count == 0 ? 1 : platformList.Count));
                var rangeWellList = worksheet2.Cells.CreateRange("N1", "N" + (wellList.Count == 0 ? 1 : wellList.Count));
                var rangeOriginatorList = worksheet2.Cells.CreateRange("O1", "O" + (originatorList.Count == 0 ? 1 : originatorList.Count));
                var rangeRIGList = worksheet2.Cells.CreateRange("P1", "P" + (rigList.Count == 0 ? 1 : rigList.Count));

                // Name the range.
                rangeCategoryList.Name = "CategoryList";
                rangeRevisionList.Name = "RevisionList";
                rangeYearList.Name = "YearList";
                rangePlantList.Name = "PlantList";
                rangeSystemList.Name = "SystemList";
                rangeDisciplineList.Name = "DisciplineList";
                rangeDocumentTypeList.Name = "DocumentTypeList";
                rangeTagTypeList.Name = "TagTypeList";
                rangeProjectList.Name = "ProjectList";
                rangeBlockList.Name = "BlockList";
                rangeFieldList.Name = "FieldList";
                rangePlatformList.Name = "PlatformList";
                rangeWellList.Name = "WellList";
                rangeOriginatorList.Name = "OriginatorList";
                rangeRIGList.Name = "RIGList";

                // Fill different cells with data in the range.
                for (int j = 0; j < listCategory.Count; j++)
                {
                    rangeCategoryList[j, 0].PutValue(listCategory[j].Name);
                }

                for (int j = 0; j < revisionList.Count; j++)
                {
                    rangeRevisionList[j, 0].PutValue(revisionList[j].Name);
                }

                for (int j = 1900; j <= DateTime.Now.Year; j++)
                {
                    rangeYearList[j - 1900, 0].PutValue(j);
                }

                for (int j = 0; j < plantList.Count; j++)
                {
                    rangePlantList[j, 0].PutValue(plantList[j].Name);
                }

                for (int j = 0; j < systemList.Count; j++)
                {
                    rangeSystemList[j, 0].PutValue(systemList[j].Name);
                }

                for (int j = 0; j < disciplineList.Count; j++)
                {
                    rangeDisciplineList[j, 0].PutValue(disciplineList[j].Name);
                }

                for (int j = 0; j < documentTypeList.Count; j++)
                {
                    rangeDocumentTypeList[j, 0].PutValue(documentTypeList[j].Name);
                }

                for (int j = 0; j < tagtypeList.Count; j++)
                {
                    rangeTagTypeList[j, 0].PutValue(tagtypeList[j].Name);
                }

                for (int j = 0; j < projectList.Count; j++)
                {
                    rangeProjectList[j, 0].PutValue(projectList[j].Name);
                }

                for (int j = 0; j < blockList.Count; j++)
                {
                    rangeBlockList[j, 0].PutValue(blockList[j].Name);
                }

                for (int j = 0; j < fieldList.Count; j++)
                {
                    rangeFieldList[j, 0].PutValue(fieldList[j].Name);
                }

                for (int j = 0; j < platformList.Count; j++)
                {
                    rangePlatformList[j, 0].PutValue(platformList[j].Name);
                }

                for (int j = 0; j < wellList.Count; j++)
                {
                    rangeWellList[j, 0].PutValue(wellList[j].Name);
                }

                for (int j = 0; j < originatorList.Count; j++)
                {
                    rangeOriginatorList[j, 0].PutValue(originatorList[j].Name);
                }

                for (int j = 0; j < rigList.Count; j++)
                {
                    rangeRIGList[j, 0].PutValue(rigList[j].Name);
                }

                // Get the validations collection.
                var validations = worksheet1.Validations;
                this.CreateValidation("CategoryList", validations, 1, 1000, 0, 0);
                this.CreateValidation("RevisionList", validations, 1, 1000, 4, 4);
                this.CreateValidation("YearList", validations, 1, 1000, 8, 8);
                this.CreateValidation("PlantList", validations, 1, 1000, 9, 9);
                this.CreateValidation("SystemList", validations, 1, 1000, 10, 10);
                this.CreateValidation("DisciplineList", validations, 1, 1000, 11, 11);
                this.CreateValidation("DocumentTypeList", validations, 1, 1000, 12, 12);
                this.CreateValidation("TagTypeList", validations, 1, 1000, 13, 13);
                this.CreateValidation("ProjectList", validations, 1, 1000, 14, 14);
                this.CreateValidation("BlockList", validations, 1, 1000, 15, 15);
                this.CreateValidation("FieldList", validations, 1, 1000, 16, 16);
                this.CreateValidation("PlatformList", validations, 1, 1000, 17, 17);
                this.CreateValidation("WellList", validations, 1, 1000, 18, 18);
                this.CreateValidation("OriginatorList", validations, 1, 1000, 29, 29);
                this.CreateValidation("OriginatorList", validations, 1, 1000, 30, 30);
                this.CreateValidation("RIGList", validations, 1, 1000, 33, 33);

                // Hide config column
                var totalProperty = Convert.ToInt32(ConfigurationManager.AppSettings.Get("TotalProperty"));

                for (int i = 2; i < totalProperty + 2; i++)
                {
                    if (!selectedProperty.Contains((i - 1).ToString()))
                    {
                        worksheet1.Cells.HideColumn((byte)i);
                    }
                }

                var tempStr = string.Empty;
                tempStr = selectedProperty.Aggregate(tempStr, (current, t) => current + t + ";");
                worksheet3.Cells[0,0].PutValue(tempStr);

                worksheet2.IsVisible = false;
                worksheet3.IsVisible = false;

                var filename = "InputTemplateDateFile.xls";
                workbook.Save(filePath + filename);
                this.DownloadByWriteByte(filePath + filename, filename, true);
            }
            else if (e.Argument == "DownloadMulti")
            {
                var serverTotalDocPackPath = Server.MapPath("~/Exports/DocPack/" + DateTime.Now.ToBinary() + "_DocPack.rar");
                var docPack = ZipPackage.CreateFile(serverTotalDocPackPath);

                foreach (GridDataItem item in this.grdDocument.MasterTableView.Items)
                {
                    var cboxSelected = (CheckBox)item["IsSelected"].FindControl("IsSelected");
                    if (cboxSelected.Checked)
                    {
                        var docId = Convert.ToInt32(item.GetDataKeyValue("ID"));

                        var name = (Label)item["Index1"].FindControl("lblName");
                        var serverDocPackPath = Server.MapPath("~/Exports/DocPack/" + name.Text + "_" + DateTime.Now.ToString("ddMMyyyhhmmss") + ".rar");

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
                grdDocument.MasterTableView.SortExpressions.Clear();
                grdDocument.MasterTableView.GroupByExpressions.Clear();
                ////grdDocument.MasterTableView.CurrentPageIndex = grdDocument.MasterTableView.PageCount - 1;
                grdDocument.Rebind();
            }
            else if (e.Argument == "SendNotification")
            {
                var listDisciplineId = new List<int>();
                var listSelectedDoc = new List<Document>();
                var count = 0;
                foreach (GridDataItem item in this.grdDocument.MasterTableView.Items)
                {
                    var cboxSelected = (CheckBox)item["IsSelected"].FindControl("IsSelected");
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
                        Credentials = new NetworkCredential(UserSession.Current.User.Email, Utility.Decrypt(UserSession.Current.User.HashCode))
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
                            var listUserId = notificationRule.ReceiverListId.Split(';').Select(t => Convert.ToInt32(t)).ToList();
                            foreach (var userId in listUserId)
                            {
                                var user = this.userService.GetByID(userId);
                                if (user != null)
                                {
                                    message.To.Add(new MailAddress(user.Email));
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(notificationRule.ReceiveGroupId) && string.IsNullOrEmpty(notificationRule.ReceiverListId))
                        {
                            var listGroupId = notificationRule.ReceiveGroupId.Split(';').Select(t => Convert.ToInt32(t)).ToList();
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
                                <td><a href='http://" + Server.MachineName + (!string.IsNullOrEmpty(port) ? ":" + port : string.Empty)
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
            ////var pageSize = this.grdDocument.PageSize;
            ////var currentPage = this.grdDocument.CurrentPageIndex;
            ////var startingRecordNumber = currentPage * pageSize;
            
            ////var rtvOptionalTypeDetail = (RadTreeView)this.rpbObjTree.FindItemByValue("ObjTree").FindControl("rtvOptionalTypeDetail");
            ////if (rtvOptionalTypeDetail != null)
            ////{
            ////    rtvOptionalTypeDetail.ClearSelectedNodes();
            ////}

            this.LoadDocuments(false, isListAll);

            ////var expression = new GridGroupByExpression();
            ////var gridGroupByField = new GridGroupByField { FieldAlias = "Folder", FieldName = "DirName" };

            ////expression.GroupByFields.Add(gridGroupByField);
            ////this.grdDocument.MasterTableView.GroupByExpressions.Add(expression);
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
            var listRelateDoc = this.documentNewService.GetAllRelateDocument(docId);
            if (listRelateDoc != null)
            {
                foreach (var objDoc in listRelateDoc)
                {
                    var attachFiles = this.attachFileService.GetAllByDocId(objDoc.ID);
                    foreach (var attachFile in attachFiles)
                    {
                        this.attachFileService.Delete(attachFile);
                    }

                    this.documentNewService.Delete(objDoc);
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
                var rtvOptionalTypeDetail = (RadTreeView)this.rpbObjTree.FindItemByValue("ObjTree").FindControl("rtvOptionalTypeDetail");
                if (rtvOptionalTypeDetail != null)
                {
                    rtvOptionalTypeDetail.ClearSelectedNodes();
                }
            }

            /*
            if (e.CommandName == RadGrid.RebindGridCommandName)
            {
                this.grdDocument.Rebind();
            }
            else if (e.CommandName == RadGrid.ExportToExcelCommandName)
            {
                var folderId = Convert.ToInt32(this.radTreeFolder.SelectedNode.Value);
                var tempListFolderId = new List<int>();
                var selectedFolder = this.radTreeFolder.SelectedNode;
                var isListAll = this.Session["IsListAll"] != null && Convert.ToBoolean(this.Session["IsListAll"]);
                var strFolderPermission = string.Empty;

                var filePath = Server.MapPath("Exports") + @"\";
                var listDocuments = new List<Document>();
                if (isListAll)
                {
                    var folder = this.folderService.GetById(folderId);
                    
                    var folderPermission =
                        this.groupDataPermissionService.GetByRoleId(
                            UserSession.Current.User.RoleId.GetValueOrDefault()).Where(
                                t => t.CategoryIdList == folder.CategoryID.ToString()).Select(
                                    t => Convert.ToInt32(t.FolderIdList)).ToList();

                    if (selectedFolder.GetAllNodes().Count > 0)
                    {
                        tempListFolderId.AddRange(
                        from folderNode in selectedFolder.GetAllNodes()
                        where folderPermission.Contains(Convert.ToInt32(folderNode.Value))
                        select Convert.ToInt32(folderNode.Value));
                    }
                    else
                    {
                        tempListFolderId.Add(folderId);
                    }
                }
                else
                {
                    tempListFolderId.Add(folderId);
                }

                strFolderPermission = tempListFolderId.Aggregate(strFolderPermission, (current, t) => current + t + ",");

                var categoryId = Convert.ToInt32(this.radPbCategories.SelectedItem.Value);
                var revisionList = this.revisionService.GetAll();
                var statusList = this.statusService.GetAllByCategory(categoryId);
                var disciplineList = this.disciplineService.GetAllByCategory(categoryId);
                var documentTypeList = this.documentTypeService.GetAllByCategory(categoryId);
                var receivedFromList = this.receivedFromService.GetAllByCategory(categoryId);

                var workbook = new Workbook();
                workbook.Open(filePath + @"Template\EditDataFileTemplate.xls");

                // Get the first worksheet.
                var worksheet1 = workbook.Worksheets[0];

                worksheet1.Cells[0, 0].PutValue("ID");
                worksheet1.Cells[0, 1].PutValue("Name");
                worksheet1.Cells[0, 2].PutValue("Document number");
                worksheet1.Cells[0, 3].PutValue("Title");
                worksheet1.Cells[0, 4].PutValue("Revision");
                worksheet1.Cells[0, 5].PutValue("Status");
                worksheet1.Cells[0, 6].PutValue("Discipline");
                worksheet1.Cells[0, 7].PutValue("Document Type");
                worksheet1.Cells[0, 8].PutValue("Received from");
                worksheet1.Cells[0, 9].PutValue("Transmittal number");
                worksheet1.Cells[0, 10].PutValue("Received date");
                worksheet1.Cells[0, 11].PutValue("Remark");
                worksheet1.Cells[0, 12].PutValue("Well");

                var dataset = new DataSet();
                using (var conn = new SqlConnection(ConfigurationSettings.AppSettings["SiteSqlServer"]))
                {
                    using (var cmd = new SqlCommand("GetAllDocBySpecialFolder", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_FolderList", strFolderPermission);
                        conn.Open();
                        using (var da = new SqlDataAdapter())
                        {
                            da.SelectCommand = cmd;
                            da.Fill(dataset);
                        }

                        conn.Close();
                    }
                }

                if (dataset.Tables.Count > 0)
                {
                    // Add a new worksheet and access it.
                    ////var i = workbook.Worksheets.Add();

                    var worksheet2 = workbook.Worksheets[1];

                    // Create a range in the second worksheet.
                    var rangeRevisionList = worksheet2.Cells.CreateRange("A1", "A" + revisionList.Count);
                    var rangeStatusList = worksheet2.Cells.CreateRange("B1", "B" + statusList.Count);
                    var rangeDisciplineList = worksheet2.Cells.CreateRange("C1", "C" + disciplineList.Count);
                    var rangeDocumentTypeList = worksheet2.Cells.CreateRange("D1", "D" + documentTypeList.Count);
                    var rangeReceivedFromList = worksheet2.Cells.CreateRange("E1", "E" + receivedFromList.Count);

                    // Name the range.
                    rangeRevisionList.Name = "RevisionList";
                    rangeStatusList.Name = "StatusList";
                    rangeDisciplineList.Name = "DisciplineList";
                    rangeDocumentTypeList.Name = "DocumentTypeList";
                    rangeReceivedFromList.Name = "ReceivedFromList";

                    // Fill different cells with data in the range.
                    for (int j = 0; j < revisionList.Count; j++)
                    {
                        rangeRevisionList[j, 0].PutValue(revisionList[j].Name);
                    }

                    for (int j = 0; j < statusList.Count; j++)
                    {
                        rangeStatusList[j, 0].PutValue(statusList[j].Name);
                    }

                    for (int j = 0; j < disciplineList.Count; j++)
                    {
                        rangeDisciplineList[j, 0].PutValue(disciplineList[j].Name);
                    }

                    for (int j = 0; j < documentTypeList.Count; j++)
                    {
                        rangeDocumentTypeList[j, 0].PutValue(documentTypeList[j].Name);
                    }

                    for (int j = 0; j < receivedFromList.Count; j++)
                    {
                        rangeReceivedFromList[j, 0].PutValue(receivedFromList[j].Name);
                    }

                    // Get the validations collection.
                    var validations = worksheet1.Validations;
                    this.CreateValidation("RevisionList", validations, 1, dataset.Tables[0].Rows.Count, 4, 4);
                    this.CreateValidation("StatusList", validations, 1, dataset.Tables[0].Rows.Count, 5, 5);
                    this.CreateValidation("DisciplineList", validations, 1, dataset.Tables[0].Rows.Count, 6, 6);
                    this.CreateValidation("DocumentTypeList", validations, 1, dataset.Tables[0].Rows.Count, 7, 7);
                    this.CreateValidation("ReceivedFromList", validations, 1, dataset.Tables[0].Rows.Count, 8, 8);

                    worksheet1.Cells.ImportDataTable(dataset.Tables[0], false, 1, 0, dataset.Tables[0].Rows.Count, 13, true);
                }

                worksheet1.FreezePanes(1, 2, 1, 2);
                worksheet1.AutoFitColumns();
                worksheet1.AutoFitRows();
                Aspose.Cells.Style style = workbook.Styles[workbook.Styles.Add()];
                style.HorizontalAlignment = TextAlignmentType.Center;
                style.VerticalAlignment = TextAlignmentType.Center;
                style.Font.IsBold = true;

                worksheet1.Cells[0, 1].Style = style;
                worksheet1.Cells[0, 2].Style = style;
                worksheet1.Cells[0, 3].Style = style;
                worksheet1.Cells[0, 4].Style = style;
                worksheet1.Cells[0, 5].Style = style;
                worksheet1.Cells[0, 6].Style = style;
                worksheet1.Cells[0, 7].Style = style;
                worksheet1.Cells[0, 8].Style = style;
                worksheet1.Cells[0, 9].Style = style;
                worksheet1.Cells[0, 10].Style = style;
                worksheet1.Cells[0, 11].Style = style;
                worksheet1.Cells[0, 12].Style = style;

                worksheet1.Cells.HideColumn(0);
                worksheet1.AutoFilter.Range = "A1:M" + listDocuments.Count + 1;

                var filename = "EditDataFile.xls";
                workbook.Save(filePath + filename);
                this.DownloadByWriteByte(filePath + filename, "EditDataFile.xls", false);
            }*/
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

        /// <summary>
        /// The get all child folder id.
        /// </summary>
        /// <param name="parentId">
        /// The parent id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<int> GetAllChildFolderId(int parentId, List<int> folderPermission)
        {
            if (!this.listFolderId.Contains(parentId))
            {
                this.listFolderId.Add(parentId);
            }


            var listFolder = this.folderService.GetAllByParentId(parentId, folderPermission);
            foreach (var folder in listFolder)
            {
                this.listFolderId.Add(folder.ID);
                this.GetAllChildFolderId(folder.ID, folderPermission);
            }

            return this.listFolderId;
        }

        /// <summary>
        /// The restore expand state tree view.
        /// </summary>
        private void RestoreExpandStateTreeView()
        {
             //Restore expand state of tree folder
            var rtvOptionalTypeDetail = (RadTreeView)this.rpbObjTree.FindItemByValue("ObjTree").FindControl("rtvOptionalTypeDetail");
            HttpCookie cookie = Request.Cookies["expandedNodesObjTree"];
            if (cookie != null)
            {
                var expandedNodeValues = cookie.Value.Split('*');
                foreach (var nodeValue in expandedNodeValues)
                {
                    RadTreeNode expandedNode = rtvOptionalTypeDetail.FindNodeByValue(HttpUtility.UrlDecode(nodeValue));
                    if (expandedNode != null)
                    {
                        expandedNode.Expanded = true;
                    }
                }
            }
        }

        /// <summary>
        /// The custom folder tree.
        /// </summary>
        /// <param name="radTreeView">
        /// The rad tree view.
        /// </param>
        private void CustomFolderTree(RadTreeNode radTreeView)
        {
            foreach (var node in radTreeView.Nodes)
            {
                var nodetemp = (RadTreeNode)node;
                if (nodetemp.Nodes.Count > 0)
                {
                    this.CustomFolderTree(nodetemp);
                }

                nodetemp.ImageUrl = "Images/folderdir16.png";
            }
        }

        private void LoadListPanel()
        {
            var listId = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("ListID"));
            var permissions = this.permissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault(), listId);
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    permission.ParentId = -1;
                    permission.MenuName = permission.Menu.Description;
                }

                permissions.Insert(0, new Permission() { Id = -1, MenuName = "LIST" });

                ////this.radPbList.DataSource = permissions;
                ////this.radPbList.DataFieldParentID = "ParentId";
                ////this.radPbList.DataFieldID = "Id";
                ////this.radPbList.DataValueField = "Id";
                ////this.radPbList.DataTextField = "MenuName";
                ////this.radPbList.DataBind();
                ////this.radPbList.Items[0].Expanded = true;

                ////foreach (RadPanelItem item in this.radPbList.Items[0].Items)
                ////{
                ////    item.ImageUrl = @"Images/listmenu.png";
                ////    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                ////}
            }
        }

        private void LoadSystemPanel()
        {
            var systemId = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("SystemID"));
            var permissions = this.permissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault(), systemId);
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    permission.ParentId = -1;
                    permission.MenuName = permission.Menu.Description;
                }

                permissions.Insert(0, new Permission() { Id = -1, MenuName = "SYSTEM" });

                ////this.radPbSystem.DataSource = permissions;
                ////this.radPbSystem.DataFieldParentID = "ParentId";
                ////this.radPbSystem.DataFieldID = "Id";
                ////this.radPbSystem.DataValueField = "Id";
                ////this.radPbSystem.DataTextField = "MenuName";
                ////this.radPbSystem.DataBind();
                ////this.radPbSystem.Items[0].Expanded = true;

                ////foreach (RadPanelItem item in this.radPbSystem.Items[0].Items)
                ////{
                ////    item.ImageUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Icon;
                ////    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                ////}
            }
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
            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            var deparmentId = isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() ? UserSession.Current.RoleId.ToString() : "0";

            var ddlCategory = (RadComboBox)this.rpbObjTree.FindItemByValue("ObjTree").FindControl("ddlCategory");

            var selectedCategory = this.groupDataPermissionService.GetByRoleId(UserSession.Current.RoleId).Select(t => Convert.ToInt32(t.CategoryIdList)).ToList();
            var listCategory = this.categoryService.GetAll().Where(t => selectedCategory.Contains(t.ID));
            ddlCategory.DataSource = listCategory;
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataValueField = "ID";
            ddlCategory.DataBind();
            

            if (this.Session["SelectedCategory"] != null)
            {
                ddlCategory.SelectedValue = this.Session["SelectedCategory"].ToString();
            }
            else
            {
                ddlCategory.SelectedIndex = 0;
                Session.Add("SelectedCategory", ddlCategory.SelectedValue);
            }

            var rtvOptionalTypeDetail = (RadTreeView)this.rpbObjTree.FindItemByValue("ObjTree").FindControl("rtvOptionalTypeDetail");

            var listOptionalTypeDetail = this.optionalTypeDetailService.GetAllSpecial(ddlCategory.SelectedValue, 0, deparmentId).Where(t => t.OptionalTypeId == this.SystempOptType || t.OptionalTypeId == this.TagOptType).ToList();

            if (isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault())
            {
                this.RepairList(ref listOptionalTypeDetail);
            }

            var textField = ConfigurationManager.AppSettings.Get("TextFieldOptionalTypeDetail");

            rtvOptionalTypeDetail.DataSource = listOptionalTypeDetail;
            rtvOptionalTypeDetail.DataFieldParentID = "ParentId";
            rtvOptionalTypeDetail.DataTextField = !string.IsNullOrEmpty(textField) ? textField : "Name";
            rtvOptionalTypeDetail.DataValueField = "ID";
            rtvOptionalTypeDetail.DataFieldID = "ID";
            rtvOptionalTypeDetail.DataBind();

            this.RestoreExpandStateTreeView();
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

        protected void ddlCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Session.Add("SelectedCategory", e.Value);
            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            var deparmentId = isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() ? UserSession.Current.RoleId.ToString() : "0";
            var rtvOptionalTypeDetail = (RadTreeView)this.rpbObjTree.FindItemByValue("ObjTree").FindControl("rtvOptionalTypeDetail");

            ////var listOptionalTypeDetail = this.optionalTypeDetailService.GetAllSpecial(e.Value, 0, deparmentId);
            var listOptionalTypeDetail = this.optionalTypeDetailService.GetAllSpecial(e.Value, 0, deparmentId).Where(t => t.OptionalTypeId == this.SystempOptType).ToList();
            rtvOptionalTypeDetail.DataSource = listOptionalTypeDetail;
            rtvOptionalTypeDetail.DataFieldParentID = "ParentId";
            rtvOptionalTypeDetail.DataTextField = "Name";
            rtvOptionalTypeDetail.DataValueField = "ID";
            rtvOptionalTypeDetail.DataFieldID = "ID";
            rtvOptionalTypeDetail.DataBind();

            ////this.LoadDocConfigGridView();
            this.grdDocument.Rebind();
        }

        protected void grdDocument_Init(object sender, EventArgs e)
        {
        }

        ////protected void grdDocument_DataBound(object sender, EventArgs e)
        ////{
        ////    var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
        ////    var selectedProperty = new List<string>();

        ////    var ddlCategory = (RadComboBox)this.rpbObjTree.FindItemByValue("ObjTree").FindControl("ddlCategory");
        ////    var categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
        ////    var deparmentId = isViewByGroup ? UserSession.Current.RoleId : 0;
        ////    foreach (var docPropertiesView in this.docPropertiesViewService.GetAllSpecial(categoryId, deparmentId))
        ////    {
        ////        var temp = docPropertiesView.PropertyIndex.Split(',').Where(t => !string.IsNullOrEmpty(t.Trim())).Select(t => t.Trim()).ToList();
        ////        selectedProperty.AddRange(temp);
        ////    }

        ////    selectedProperty = selectedProperty.Distinct().ToList();

        ////    for (int i = 1; i <= 30; i++)
        ////    {
        ////        var column = this.grdDocument.MasterTableView.GetColumn("Index" + i);
        ////        if (column != null)
        ////        {
        ////            column.Visible = selectedProperty.Contains(i.ToString());
        ////        }
        ////    }
        ////}

        private void LoadDocConfigGridView()
        {
            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            var selectedProperty = new List<string>();

            var ddlCategory = (RadComboBox)this.rpbObjTree.FindItemByValue("ObjTree").FindControl("ddlCategory");
            if (ddlCategory != null && ddlCategory.Items.Count > 0)
            {
                var categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                var deparmentId = isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                                      ? UserSession.Current.RoleId
                                      : 0;
                foreach (var docPropertiesView in this.docPropertiesViewService.GetAllSpecial(categoryId, deparmentId))
                {
                    var temp =
                        docPropertiesView.PropertyIndex.Split(',').Where(t => !string.IsNullOrEmpty(t.Trim())).Select(
                            t => t.Trim()).ToList();
                    selectedProperty.AddRange(temp);
                }

                selectedProperty = selectedProperty.Distinct().ToList();
                var totalProperty = Convert.ToInt32(ConfigurationManager.AppSettings.Get("TotalProperty"));

                for (int i = 1; i <= totalProperty; i++)
                {
                    var column = this.grdDocument.MasterTableView.GetColumn("Index" + i);
                    var revisionColumn = this.grdDocument.MasterTableView.GetColumn("RevisionColumn");
                    if (column != null)
                    {
                        column.Visible = selectedProperty.Contains(i.ToString());
                        if (revisionColumn != null)
                        {
                            if (selectedProperty.Contains("3"))
                            {
                                revisionColumn.Visible = true;
                            }
                            else
                            {
                                revisionColumn.Visible = false;
                            }
                        }
                    }
                }
            }
        }

        protected void rtvOptionalTypeDetail_NodeClick(object sender, RadTreeNodeEventArgs e)
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
            var tagId = item.GetDataKeyValue("ID").ToString();

            var tagObj = this.optionalTypeDetailService.GetById(Convert.ToInt32(tagId));
            if (tagObj != null)
            {
                var docList =
                    this.documentNewService.GetAllCurrentDoc().Where(
                        t => !string.IsNullOrEmpty(t.TagTypeId) && t.TagTypeId.Split(',').Contains(tagId));

                var serverTotalDocPackPath =
                    Server.MapPath("~/Exports/DocPack/" + DateTime.Now.ToBinary() + "_Tag_" + tagObj.Name + ".rar");
                var docTagPack = ZipPackage.CreateFile(serverTotalDocPackPath);
                
                foreach (var docObj in docList)
                {
                    var serverDocPackPath =
                        Server.MapPath("~/Exports/DocPack/" + DateTime.Now.ToBinary() + "_" + docObj.Name + "_Pack.rar");

                    var attachFiles = this.attachFileService.GetAllByDocId(docObj.ID);

                    var temp = ZipPackage.CreateFile(serverDocPackPath);

                    foreach (var attachFile in attachFiles)
                    {
                        if (File.Exists(Server.MapPath(attachFile.FilePath)))
                        {
                            temp.Add(Server.MapPath(attachFile.FilePath));
                        }
                    }

                    docTagPack.Add(serverDocPackPath);
                }

                this.DownloadByWriteByte(serverTotalDocPackPath, tagObj.Name + ".rar", true);
            }
        }

        protected void grdDocument_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridFilteringItem)
            {
                var filterItem = (GridFilteringItem)e.Item;
                var selectedProperty = new List<string>();

                var ddlFilterRev = (RadComboBox)filterItem.FindControl("ddlFilterRev");
                var ddlFilterDocumentType = (RadComboBox)filterItem.FindControl("ddlFilterDocumentType");
                var ddlFilterTagType = (RadComboBox)filterItem.FindControl("ddlFilterTagType");
                var ddlFilterYear = (RadComboBox)filterItem.FindControl("ddlFilterYear");
                var ddlFilterPlant = (RadComboBox)filterItem.FindControl("ddlFilterPlant");
                var ddlFilterSystem = (RadComboBox)filterItem.FindControl("ddlFilterSystem");
                var ddlFilterDiscipline = (RadComboBox)filterItem.FindControl("ddlFilterDiscipline");
                var ddlFilterProject = (RadComboBox)filterItem.FindControl("ddlFilterProject");
                var ddlFilterBlock = (RadComboBox)filterItem.FindControl("ddlFilterBlock");
                var ddlFilterField = (RadComboBox)filterItem.FindControl("ddlFilterField");
                var ddlFilterPlatform = (RadComboBox)filterItem.FindControl("ddlFilterPlatform");
                var ddlFilterWell = (RadComboBox)filterItem.FindControl("ddlFilterWell");
                var ddlFilterFrom = (RadComboBox)filterItem.FindControl("ddlFilterFrom");
                var ddlFilterTo = (RadComboBox)filterItem.FindControl("ddlFilterTo");
                var ddlFilterRIG = (RadComboBox)filterItem.FindControl("ddlFilterRIG");

                var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
                var ddlCategory = (RadComboBox)this.rpbObjTree.FindItemByValue("ObjTree").FindControl("ddlCategory");
                var deparmentId = isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() ? UserSession.Current.RoleId.ToString() : "0";

                foreach (var docPropertiesView in this.docPropertiesViewService.GetAllSpecial(Convert.ToInt32(ddlCategory.SelectedValue), Convert.ToInt32(deparmentId)))
                {
                    var temp = docPropertiesView.PropertyIndex.Split(',').Where(t => !string.IsNullOrEmpty(t.Trim())).Select(t => t.Trim()).ToList();
                    selectedProperty.AddRange(temp);
                }

                selectedProperty = selectedProperty.Distinct().ToList();


                var listOptionalTypeDetail = this.optionalTypeDetailService.GetAllSpecial(ddlCategory.SelectedValue, 0, deparmentId);
                var listParentId = listOptionalTypeDetail.Select(t => t.ParentId).Distinct().ToList();

                if (selectedProperty.Contains("3") && ddlFilterRev != null)
                {
                    var revisionList = this.revisionService.GetAll();
                    ////revisionList.Insert(0, new Revision() { Name = "All" });

                    foreach (var revision in revisionList)
                    {
                        var revItem = new RadComboBoxItem(revision.Name, revision.Name);
                        ddlFilterRev.Items.Add(revItem);
                    }
                }

                if (selectedProperty.Contains("6") && ddlFilterYear != null)
                {
                    for (var i = DateTime.Now.Year; i >= 1975; i--)
                    {
                        var yearItem = new RadComboBoxItem(i.ToString(), i.ToString());
                        ddlFilterYear.Items.Add(yearItem);
                    }
                }

                if (selectedProperty.Contains("7") && ddlFilterPlant != null)
                {
                    foreach (var plant in listOptionalTypeDetail.Where(t => t.OptionalTypeId == 1 && !listParentId.Contains(t.ID)))
                    {
                        var plantItem = new RadComboBoxItem(plant.Name, plant.Name);
                        ddlFilterPlant.Items.Add(plantItem);
                    }
                }

                if (selectedProperty.Contains("8") && ddlFilterSystem != null)
                {
                    foreach (var system in listOptionalTypeDetail.Where(t => t.OptionalTypeId == 2 && !listParentId.Contains(t.ID)))
                    {
                        var systemItem = new RadComboBoxItem(system.Name, system.Name);
                        ddlFilterSystem.Items.Add(systemItem);
                    }
                }

                if (selectedProperty.Contains("9") && ddlFilterDiscipline != null)
                {
                    foreach (var discipline in listOptionalTypeDetail.Where(t => t.OptionalTypeId == 4 && !listParentId.Contains(t.ID)))
                    {
                        var disciplineItem = new RadComboBoxItem(discipline.Name, discipline.Name);
                        ddlFilterDiscipline.Items.Add(disciplineItem);
                    }
                }

                if (selectedProperty.Contains("10") && ddlFilterDocumentType != null)
                {
                    foreach (var docType in listOptionalTypeDetail.Where(t => t.OptionalTypeId == 5 && !listParentId.Contains(t.ID)))
                    {
                        var revItem = new RadComboBoxItem(docType.Name, docType.Name);
                        ddlFilterDocumentType.Items.Add(revItem);
                    }
                }

                if (selectedProperty.Contains("11") && ddlFilterTagType != null)
                {
                    foreach (var tagType in listOptionalTypeDetail.Where(t => t.OptionalTypeId == 3 && !listParentId.Contains(t.ID)))
                    {
                        var revItem = new RadComboBoxItem(tagType.Name, tagType.Name);
                        ddlFilterTagType.Items.Add(revItem);
                    }
                }

                if (selectedProperty.Contains("12") && ddlFilterProject != null)
                {
                    foreach (var project in listOptionalTypeDetail.Where(t => t.OptionalTypeId == 6 && !listParentId.Contains(t.ID)))
                    {
                        var projectItem = new RadComboBoxItem(project.Name, project.Name);
                        ddlFilterProject.Items.Add(projectItem);
                    }
                }

                if (selectedProperty.Contains("13") && ddlFilterBlock != null)
                {
                    foreach (var block in listOptionalTypeDetail.Where(t => t.OptionalTypeId == 7 && t.ParentId != null))
                    {
                        var blockItem = new RadComboBoxItem(block.Name, block.Name);
                        ddlFilterBlock.Items.Add(blockItem);
                    }
                }

                if (selectedProperty.Contains("14") && ddlFilterField != null)
                {
                    foreach (var field in listOptionalTypeDetail.Where(t => t.OptionalTypeId == 8 && t.ParentId != null))
                    {
                        var fieldItem = new RadComboBoxItem(field.Name, field.Name);
                        ddlFilterField.Items.Add(fieldItem);
                    }
                }

                if (selectedProperty.Contains("15") && ddlFilterPlatform != null)
                {
                    foreach (var platform in listOptionalTypeDetail.Where(t => t.OptionalTypeId == 9 && t.ParentId != null))
                    {
                        var platformItem = new RadComboBoxItem(platform.Name, platform.Name);
                        ddlFilterPlatform.Items.Add(platformItem);
                    }
                }

                if (selectedProperty.Contains("16") && ddlFilterWell != null)
                {
                    foreach (var well in listOptionalTypeDetail.Where(t => t.OptionalTypeId == 10 && t.ParentId != null))
                    {
                        var wellItem = new RadComboBoxItem(well.Name, well.Name);
                        ddlFilterWell.Items.Add(wellItem);
                    }
                }


                var listOriginator = this.originatorService.GetAll();
                if (selectedProperty.Contains("28") && ddlFilterFrom != null)
                {
                    foreach (var originator in listOriginator)
                    {
                        var originatorItem = new RadComboBoxItem(originator.Name, originator.Name);
                        ddlFilterFrom.Items.Add(originatorItem);
                    }
                }

                if (selectedProperty.Contains("29") && ddlFilterTo != null)
                {
                    foreach (var originator in listOriginator)
                    {
                        var originatorItem = new RadComboBoxItem(originator.Name, originator.Name);
                        ddlFilterTo.Items.Add(originatorItem);
                    }
                }

                if (selectedProperty.Contains("32") && ddlFilterField != null)
                {
                    foreach (var rig in listOptionalTypeDetail.Where(t => t.OptionalTypeId == 12 && t.ParentId != null))
                    {
                        var rigItem = new RadComboBoxItem(rig.Name, rig.Name);
                        ddlFilterRIG.Items.Add(rigItem);
                    }
                }

                var rtvOptionalTypeDetail = (RadTreeView)this.rpbObjTree.FindItemByValue("ObjTree").FindControl("rtvOptionalTypeDetail");
                if (rtvOptionalTypeDetail.SelectedNode != null)
                {
                    var optionalTypeDetailObj = this.optionalTypeDetailService.GetById(Convert.ToInt32(rtvOptionalTypeDetail.SelectedNode.Value));
                    if (optionalTypeDetailObj != null)
                    {
                        switch (optionalTypeDetailObj.OptionalTypeId)
                        {
                            case 1:
                                ddlFilterPlant.Visible = false;
                                break;
                            case 2:
                                ddlFilterSystem.Visible = false;
                                break;
                            case 3:
                                ddlFilterTagType.Visible = false;
                                break;
                            case 4:
                                ddlFilterDiscipline.Visible = false;
                                break;
                            case 5:
                                ddlFilterDocumentType.Visible = false;
                                break;
                            case 6:
                                ddlFilterProject.Visible = false;
                                break;
                            case 7:
                                ddlFilterBlock.Visible = false;
                                break;
                            case 8:
                                ddlFilterField.Visible = false;
                                break;
                            case 9:
                                ddlFilterPlatform.Visible = false;
                                break;
                            case 10:
                                ddlFilterWell.Visible = false;
                                break;
                            case 12:
                                ddlFilterRIG.Visible = false;
                                break;
                        }
                    }
                }
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

        protected void grdDocument_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            switch (e.DetailTableView.Name)
            {
                case "DocDetail":
                    {
                        string tagId = dataItem.GetDataKeyValue("ID").ToString();
                        var docList = this.documentNewService.GetAllCurrentDoc().Where(t => !string.IsNullOrEmpty(t.TagTypeId) && t.TagTypeId.Split(',').Contains(tagId));
                        e.DetailTableView.DataSource = docList;
                        ////e.DetailTableView.Items[]
                        //e.DetailTableView.DataSource = GetDataTable("SELECT * FROM Orders WHERE CustomerID = '" + CustomerID + "'");
                        break;
                    }
            }
        }

        protected void grdDocument_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (e.Column is GridExpandColumn)
            {
                if (!Request.Browser.Type.Contains("Firefox") && !Request.Browser.Type.Contains("IE"))
                {
                    e.Column.HeaderStyle.Width = new System.Web.UI.WebControls.Unit(10);
                    e.Column.ItemStyle.Width = new System.Web.UI.WebControls.Unit(10);
                }
            }

            if (e.Column is GridGroupSplitterColumn)
            {
                if (!Request.Browser.Type.Contains("Firefox") && !Request.Browser.Type.Contains("IE"))
                {
                    e.Column.HeaderStyle.Width = new System.Web.UI.WebControls.Unit(10);
                    e.Column.ItemStyle.Width = new System.Web.UI.WebControls.Unit(10);
                }
            }
            
        }

        protected void btnDownloadDocPack_Click(object sender, ImageClickEventArgs e)
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
    }
}

