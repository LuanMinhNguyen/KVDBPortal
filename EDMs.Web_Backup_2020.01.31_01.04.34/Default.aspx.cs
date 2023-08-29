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
    using Telerik.Web.Zip;

    using CheckBox = System.Web.UI.WebControls.CheckBox;
    using Label = System.Web.UI.WebControls.Label;
    using TextBox = System.Web.UI.WebControls.TextBox;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class Default : Page
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


            if (ConfigurationManager.AppSettings.Get("ViewEMDR") == "true")
            {
                Response.Redirect("ToDoListPage.aspx");

                if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() &&
                    !UserSession.Current.User.Role.IsUpdate.GetValueOrDefault())
                {
                    Response.Redirect("Controls/Document/DocumentsLibrary.aspx");
                }
                else
                {
                    Response.Redirect("ProjectProccessReport.aspx");
                }
                //if (UserSession.Current.User.Role.IsAdmin.GetValueOrDefault())
                //{
                //    Response.Redirect("ProjectProccessReport.aspx");
                //}
                //else
                //{
                //    Response.Redirect("EMDR.aspx");
                //}
            }

            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            if (!Page.IsPostBack)
            {
                this.LoadObjectTree();
                this.LoadDocConfigGridView();
                Session.Add("IsListAll", false);

                if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() && !UserSession.Current.User.Role.IsUpdate.GetValueOrDefault())
                {
                    this.RadPane3.Visible = false;
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
            var docList = new List<DocumentNew>();

            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));

            var ddlCategory = (RadComboBox)this.rpbObjTree.FindItemByValue("ObjTree").FindControl("ddlCategory");
            if (ddlCategory != null && ddlCategory.Items.Count > 0)
            {
                var categoryId = Convert.ToInt32(ddlCategory.SelectedValue);

                if (rtvOptionalTypeDetail != null && rtvOptionalTypeDetail.SelectedNode != null)
                {
                    var selectedNodesId =
                        rtvOptionalTypeDetail.SelectedNode.GetAllNodes().Select(t => Convert.ToInt32(t.Value)).ToList();
                    selectedNodesId.Insert(0, Convert.ToInt32(rtvOptionalTypeDetail.SelectedNode.Value));
                    var optionalTypeDetail =
                        this.optionalTypeDetailService.GetById(
                            Convert.ToInt32(rtvOptionalTypeDetail.SelectedNode.Value));

                    if (optionalTypeDetail != null)
                    {
                        var filterListId =
                            this.optionalTypeDetailService.GetAllChildId(
                                optionalTypeDetail.OptionalTypeId.GetValueOrDefault(), selectedNodesId);
                        // var temp = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Plant"));
                        switch (optionalTypeDetail.OptionalTypeId)
                        {
                            case 1:
                                docList = !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                                              ? this.documentNewService.GetAllCurrentDoc(
                                                  categoryId, UserSession.Current.RoleId, UserSession.Current.User.Id).
                                                    Where(t => filterListId.Contains(t.PlantId.GetValueOrDefault())).
                                                    ToList()
                                              : this.documentNewService.GetAllCurrentDoc(categoryId).Where(
                                                  t => filterListId.Contains(t.PlantId.GetValueOrDefault())).ToList();
                                break;
                            case 2:
                                docList = !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                                              ? this.documentNewService.GetAllCurrentDoc(
                                                  categoryId, UserSession.Current.RoleId, UserSession.Current.User.Id).
                                                    Where(t => filterListId.Contains(t.SystemId.GetValueOrDefault())).
                                                    ToList()
                                              : this.documentNewService.GetAllCurrentDoc(categoryId).Where(
                                                  t => filterListId.Contains(t.SystemId.GetValueOrDefault())).ToList();
                                break;
                            case 3:
                                docList = !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                                              ? this.documentNewService.GetAllCurrentDoc(
                                                  categoryId, UserSession.Current.RoleId, UserSession.Current.User.Id).
                                                    Where(
                                                        t =>
                                                        !string.IsNullOrEmpty(t.TagTypeId)
                                                        && filterListId.Any(x => t.TagTypeId.Contains(x.ToString()))).
                                                    ToList()
                                              : this.documentNewService.GetAllCurrentDoc(categoryId).Where(
                                                  t =>
                                                  !string.IsNullOrEmpty(t.TagTypeId)
                                                  && filterListId.Any(x => t.TagTypeId.Split(',').Contains(x.ToString()))).ToList();
                                break;
                            case 4:
                                docList = !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                                              ? this.documentNewService.GetAllCurrentDoc(
                                                  categoryId, UserSession.Current.RoleId, UserSession.Current.User.Id).
                                                    Where(
                                                        t => filterListId.Contains(t.DisciplineId.GetValueOrDefault())).
                                                    ToList()
                                              : this.documentNewService.GetAllCurrentDoc(categoryId).Where(
                                                  t => filterListId.Contains(t.DisciplineId.GetValueOrDefault())).ToList
                                                    ();
                                break;
                            case 5:
                                docList = !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                                              ? this.documentNewService.GetAllCurrentDoc(
                                                  categoryId, UserSession.Current.RoleId, UserSession.Current.User.Id).
                                                    Where(
                                                        t => filterListId.Contains(t.DocumentTypeId.GetValueOrDefault()))
                                                    .ToList()
                                              : this.documentNewService.GetAllCurrentDoc(categoryId).Where(
                                                  t => filterListId.Contains(t.DocumentTypeId.GetValueOrDefault())).
                                                    ToList();
                                break;
                            case 6:
                                docList = !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                                              ? this.documentNewService.GetAllCurrentDoc(
                                                  categoryId, UserSession.Current.RoleId, UserSession.Current.User.Id).
                                                    Where(t => filterListId.Contains(t.ProjectId.GetValueOrDefault())).
                                                    ToList()
                                              : this.documentNewService.GetAllCurrentDoc(categoryId).Where(
                                                  t => filterListId.Contains(t.ProjectId.GetValueOrDefault())).ToList();
                                break;
                            case 7:
                                docList = !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                                              ? this.documentNewService.GetAllCurrentDoc(
                                                  categoryId, UserSession.Current.RoleId, UserSession.Current.User.Id).
                                                    Where(t => filterListId.Contains(t.BlockId.GetValueOrDefault())).
                                                    ToList()
                                              : this.documentNewService.GetAllCurrentDoc(categoryId).Where(
                                                  t => filterListId.Contains(t.BlockId.GetValueOrDefault())).ToList();
                                break;
                            case 8:
                                docList = !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                                              ? this.documentNewService.GetAllCurrentDoc(
                                                  categoryId, UserSession.Current.RoleId, UserSession.Current.User.Id).
                                                    Where(t => filterListId.Contains(t.FieldId.GetValueOrDefault())).
                                                    ToList()
                                              : this.documentNewService.GetAllCurrentDoc(categoryId).Where(
                                                  t => filterListId.Contains(t.FieldId.GetValueOrDefault())).ToList();
                                break;
                            case 9:
                                docList = !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                                              ? this.documentNewService.GetAllCurrentDoc(
                                                  categoryId, UserSession.Current.RoleId, UserSession.Current.User.Id).
                                                    Where(t => filterListId.Contains(t.PlatformId.GetValueOrDefault())).
                                                    ToList()
                                              : this.documentNewService.GetAllCurrentDoc(categoryId).Where(
                                                  t => filterListId.Contains(t.PlatformId.GetValueOrDefault())).ToList();
                                break;
                            case 10:
                                docList = !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                                              ? this.documentNewService.GetAllCurrentDoc(
                                                  categoryId, UserSession.Current.RoleId, UserSession.Current.User.Id).
                                                    Where(t => filterListId.Contains(t.WellId.GetValueOrDefault())).
                                                    ToList()
                                              : this.documentNewService.GetAllCurrentDoc(categoryId).Where(
                                                  t => filterListId.Contains(t.WellId.GetValueOrDefault())).ToList();
                                break;
                            case 12:
                                docList = !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                                              ? this.documentNewService.GetAllCurrentDoc(
                                                  categoryId, UserSession.Current.RoleId, UserSession.Current.User.Id).
                                                    Where(t => filterListId.Contains(t.WellId.GetValueOrDefault())).
                                                    ToList()
                                              : this.documentNewService.GetAllCurrentDoc(categoryId).Where(
                                                  t => filterListId.Contains(t.RIGId.GetValueOrDefault())).ToList();
                                break;
                        }
                    }
                }
                else
                {
                    if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault())
                    {
                        docList = this.documentNewService.GetAllCurrentDoc(
                            categoryId, UserSession.Current.RoleId, UserSession.Current.User.Id);
                    }
                    else
                    {
                        docList = this.documentNewService.GetAllCurrentDoc(categoryId);
                    }
                }

                ////this.grdDocument.VirtualItemCount = docList.Count;
                this.grdDocument.DataSource = docList.OrderByDescending(t => t.ID);
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
                ////var plantList = listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.PlantOptType && !listParentId.Contains(t.ID)).OrderBy(t => t.Name).ToList();
                var plantList = listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.PlantOptType && 
                    (Convert.ToBoolean(ConfigurationManager.AppSettings.Get("GetParentPlant")) || !listParentId.Contains(t.ID))).OrderBy(t => t.Name).ToList();
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
            if (e.CommandName == RadGrid.RebindGridCommandName)
            {
                this.grdDocument.Rebind();
            }
            else if (e.CommandName == RadGrid.ExportToExcelCommandName)
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
                ////var plantList = listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.PlantOptType && !listParentId.Contains(t.ID)).OrderBy(t => t.Name).ToList();
                var plantList = listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.PlantOptType && 
                    (Convert.ToBoolean(ConfigurationManager.AppSettings.Get("GetParentPlant")) || !listParentId.Contains(t.ID))).OrderBy(t => t.Name).ToList();
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
            if (e.Item is Telerik.Web.UI.GridFilteringItem)
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
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                var item = e.Item as GridEditableItem;
                var ddlRevision = item.FindControl("ddlRevision") as RadComboBox;
                var ddlStatus = item.FindControl("ddlStatus") as RadComboBox;
                var ddlDiscipline = item.FindControl("ddlDiscipline") as RadComboBox;
                var ddlDocumentType = item.FindControl("ddlDocumentType") as RadComboBox;
                var ddlReceivedFrom = item.FindControl("ddlReceivedFrom") as RadComboBox;
                var txtReceivedDate = item.FindControl("txtReceivedDate") as RadDatePicker;
                var txtName = item.FindControl("txtName") as TextBox;
                var txtDocumentNumber = item.FindControl("txtDocumentNumber") as TextBox;
                var txtTitle = item.FindControl("txtTitle") as TextBox;
                var txtRemark = item.FindControl("txtRemark") as TextBox;
                var txtWell = item.FindControl("txtWell") as TextBox;
                var txtTransmittalNumber = item.FindControl("txtTransmittalNumber") as TextBox;

                txtReceivedDate.DatePopupButton.Visible = false;

                var name = (item.FindControl("Name") as HiddenField).Value;
                var revisionId = (item.FindControl("RevisionID") as HiddenField).Value;
                var statusId = (item.FindControl("StatusID") as HiddenField).Value;
                var disciplineId = (item.FindControl("DisciplineID") as HiddenField).Value;
                var documentTypeId = (item.FindControl("DocumentTypeID") as HiddenField).Value;
                var receivedFromId = (item.FindControl("ReceivedFromID") as HiddenField).Value;
                var receivedDate = (item.FindControl("ReceivedDate") as HiddenField).Value;
                var remark = (item.FindControl("Remark") as HiddenField).Value;
                var well = (item.FindControl("Well") as HiddenField).Value;
                var title = (item.FindControl("Title") as HiddenField).Value;
                var documentNumber = (item.FindControl("DocumentNumber") as HiddenField).Value;
                var transmittalNumber = (item.FindControl("TransmittalNumber") as HiddenField).Value;

                var categoryId = 0;
                if(!string.IsNullOrEmpty(receivedDate))
                {
                    txtReceivedDate.SelectedDate = Convert.ToDateTime(receivedDate);
                }

                txtName.Text = name;
                txtTitle.Text = title;
                txtRemark.Text = remark;
                txtWell.Text = well;
                txtDocumentNumber.Text = documentNumber;
                txtTransmittalNumber.Text = transmittalNumber;

                var revisionList = this.revisionService.GetAll();
                revisionList.Insert(0, new Revision() { Name = string.Empty });
                ddlRevision.DataSource = revisionList;
                ddlRevision.DataValueField = "ID";
                ddlRevision.DataTextField = "Name";
                ddlRevision.DataBind();
                ddlRevision.SelectedValue = revisionId;

                var documentTypeList = this.documentTypeService.GetAll();
                documentTypeList.Insert(0, new DocumentType() { Name = string.Empty });
                ddlDocumentType.DataSource = documentTypeList;
                ddlDocumentType.DataValueField = "ID";
                ddlDocumentType.DataTextField = "Name";
                ddlDocumentType.DataBind();
                ddlDocumentType.SelectedValue = documentTypeId;

                var statusList = this.statusService.GetAllByCategory(categoryId);
                statusList.Insert(0, new Status { Name = string.Empty });
                ddlStatus.DataSource = statusList;
                ddlStatus.DataValueField = "ID";
                ddlStatus.DataTextField = "Name";
                ddlStatus.DataBind();
                ddlStatus.SelectedValue = statusId;

                var receivedFromList = this.receivedFromService.GetAllByCategory(categoryId);
                receivedFromList.Insert(0, new ReceivedFrom() { Name = string.Empty });
                ddlReceivedFrom.DataSource = receivedFromList;
                ddlReceivedFrom.DataValueField = "ID";
                ddlReceivedFrom.DataTextField = "Name";
                ddlReceivedFrom.DataBind();
                ddlReceivedFrom.SelectedValue = receivedFromId;

                var disciplineList = this.disciplineService.GetAllByCategory(categoryId);
                disciplineList.Insert(0, new Discipline() { Name = string.Empty });
                ddlDiscipline.DataSource = disciplineList;
                ddlDiscipline.DataValueField = "ID";
                ddlDiscipline.DataTextField = "Name";
                ddlDiscipline.DataBind();
                ddlDiscipline.SelectedValue = disciplineId;

                ////var languageList = this.languageService.GetAll();
                ////languageList.Insert(0, new Language() { Name = string.Empty });
                ////this.ddlLanguage.DataSource = languageList;
                ////this.ddlLanguage.DataValueField = "ID";
                ////this.ddlLanguage.DataTextField = "Name";
                ////this.ddlLanguage.DataBind();
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
                var ddlRevision = item.FindControl("ddlRevision") as RadComboBox;
                var ddlStatus = item.FindControl("ddlStatus") as RadComboBox;
                var ddlDiscipline = item.FindControl("ddlDiscipline") as RadComboBox;
                var ddlDocumentType = item.FindControl("ddlDocumentType") as RadComboBox;
                var ddlReceivedFrom = item.FindControl("ddlReceivedFrom") as RadComboBox;
                var txtReceivedDate = item.FindControl("txtReceivedDate") as RadDatePicker;
                var txtName = item.FindControl("txtName") as TextBox;
                var txtDocumentNumber = item.FindControl("txtDocumentNumber") as TextBox;
                var txtTitle = item.FindControl("txtTitle") as TextBox;
                var txtRemark = item.FindControl("txtRemark") as TextBox;
                var txtWell = item.FindControl("txtWell") as TextBox;
                var txtTransmittalNumber = item.FindControl("txtTransmittalNumber") as TextBox;

                var docId = Convert.ToInt32(item.GetDataKeyValue("ID"));

                var objDoc = this.documentService.GetById(docId);
                var oldName = objDoc.Name;
                var currentRevision = objDoc.RevisionID;
                var newRevision = Convert.ToInt32(ddlRevision.SelectedValue);
                var watcherService = new ServiceController(ServiceName);
                if (currentRevision != 0 && currentRevision != null && currentRevision != newRevision)
                {
                    var newObjDoc = new Document
                        {
                            DocIndex = objDoc.DocIndex + 1,
                            ParentID = objDoc.ParentID,
                            FolderID = objDoc.FolderID,
                            DirName = objDoc.DirName,
                            FilePath = objDoc.FilePath,
                            RevisionFilePath = objDoc.RevisionFilePath,
                            FileExtension = objDoc.FileExtension,
                            FileExtensionIcon = objDoc.FileExtensionIcon,
                            FileNameOriginal = objDoc.FileNameOriginal,
                            LanguageID = objDoc.LanguageID,
                            
                            KeyWords = objDoc.KeyWords,
                            IsDelete = false,
                            CreatedDate = DateTime.Now,
                            CreatedBy = UserSession.Current.User.Id,

                            Name = txtName.Text.Trim(),
                            DocumentNumber = txtDocumentNumber.Text.Trim(),
                            Title = txtTitle.Text.Trim(),
                            RevisionID = Convert.ToInt32(ddlRevision.SelectedValue),
                            RevisionName = ddlRevision.Text,
                            StatusID = Convert.ToInt32(ddlStatus.SelectedValue),
                            DisciplineID = Convert.ToInt32(ddlDiscipline.SelectedValue),
                            DocumentTypeID = Convert.ToInt32(ddlDocumentType.SelectedValue),
                            ReceivedFromID = Convert.ToInt32(ddlReceivedFrom.SelectedValue),
                            ReceivedDate = txtReceivedDate.SelectedDate,
                            Remark = txtRemark.Text.Trim(),
                            Well = txtWell.Text.Trim(),
                            TransmittalNumber = txtTransmittalNumber.Text.Trim()
                        }; ////Utilities.Utility.Clone(objDoc);

                    if (!string.IsNullOrEmpty(newObjDoc.RevisionName))
                    {
                        newObjDoc.RevisionFileName = newObjDoc.RevisionName + "_" + newObjDoc.Name;
                    }
                    else
                    {
                        newObjDoc.RevisionFileName = newObjDoc.Name;
                    }


                    newObjDoc.RevisionFilePath = newObjDoc.RevisionFilePath.Substring(
                        0, newObjDoc.RevisionFilePath.LastIndexOf('/')) + "/" + DateTime.Now.ToString("ddMMyyhhmmss") + "_" + newObjDoc.RevisionFileName;

                    // Allway check revision file is exist when update
                    var filePath = Server.MapPath(newObjDoc.FilePath.Replace("/" + HostingEnvironment.ApplicationVirtualPath, "../.."));
                    var revisionFilePath = Server.MapPath(newObjDoc.RevisionFilePath.Replace("/" + HostingEnvironment.ApplicationVirtualPath, "../.."));
                    if (File.Exists(filePath) && !File.Exists(revisionFilePath))
                    {
                        File.Copy(filePath, revisionFilePath, true);
                    }
                    // End check

                    if (currentRevision < newRevision)
                    {
                        objDoc.IsLeaf = false;
                        newObjDoc.IsLeaf = true;
                        if (objDoc.ParentID == null)
                        {
                            newObjDoc.ParentID = objDoc.ID;
                        }

                        this.documentService.Update(objDoc);
                    }
                    else
                    {
                        newObjDoc.IsLeaf = false;

                        filePath = Server.MapPath(newObjDoc.FilePath.Replace("/" + HostingEnvironment.ApplicationVirtualPath, "../.."));
                        revisionFilePath = Server.MapPath(objDoc.RevisionFilePath.Replace("/" + HostingEnvironment.ApplicationVirtualPath, "../.."));
                        if (Utilities.Utility.ServiceIsAvailable(ServiceName))
                        {
                            watcherService.ExecuteCommand(128);
                        }

                        if (File.Exists(revisionFilePath))
                        {
                            File.Copy(revisionFilePath, filePath, true);
                        }

                        if (Utilities.Utility.ServiceIsAvailable(ServiceName))
                        {
                            watcherService.ExecuteCommand(129);
                        }
                    }

                    this.documentService.Insert(newObjDoc);

                    if (objDoc.Name != txtName.Text.Trim())
                    {
                        if (Utilities.Utility.ServiceIsAvailable(ServiceName))
                        {
                            watcherService.ExecuteCommand(128);
                        }

                        if (File.Exists(filePath))
                        {
                            File.Move(filePath, filePath.Replace(oldName, txtName.Text.Trim()));
                        }

                        if (Utilities.Utility.ServiceIsAvailable(ServiceName))
                        {
                            watcherService.ExecuteCommand(129);
                        }

                        var listDocRename = this.documentService.GetSpecificDocument(oldName, objDoc.DirName);
                        foreach (var document in listDocRename)
                        {
                            document.Name = txtName.Text.Trim();
                            document.FileNameOriginal = txtName.Text.Trim();
                            if (!string.IsNullOrEmpty(document.RevisionName))
                            {
                                document.RevisionFileName = document.RevisionName + "_" + txtName.Text.Trim();
                            }
                            else
                            {
                                document.RevisionFileName = txtName.Text.Trim();
                            }

                            document.FilePath = document.FilePath.Replace(oldName, txtName.Text.Trim());

                            this.documentService.Update(document);
                        }
                    }
                }
                else
                {
                    // Allway check revision file is exist when update
                    var filePath = Server.MapPath(objDoc.FilePath.Replace("/" + HostingEnvironment.ApplicationVirtualPath, "../.."));
                    var revisionFilePath = Server.MapPath(objDoc.RevisionFilePath.Replace("/" + HostingEnvironment.ApplicationVirtualPath, "../.."));

                    if (File.Exists(filePath) && !File.Exists(revisionFilePath))
                    {
                        File.Copy(filePath, revisionFilePath, true);
                    }
                    // End check

                    if (objDoc.Name != txtName.Text.Trim())
                    {
                        if (Utilities.Utility.ServiceIsAvailable(ServiceName))
                        {
                            watcherService.ExecuteCommand(128);
                        }

                        if (File.Exists(filePath))
                        {
                            File.Move(filePath, filePath.Replace(oldName, txtName.Text.Trim()));
                        }

                        if (Utilities.Utility.ServiceIsAvailable(ServiceName))
                        {
                            watcherService.ExecuteCommand(129);
                        }

                        var listDocRename = this.documentService.GetSpecificDocument(oldName, objDoc.DirName);
                        foreach (var document in listDocRename)
                        {
                            document.Name = txtName.Text.Trim();
                            document.FileNameOriginal = txtName.Text.Trim();
                            if (!string.IsNullOrEmpty(document.RevisionName))
                            {
                                document.RevisionFileName = document.RevisionName + "_" + txtName.Text.Trim();
                            }
                            else
                            {
                                document.RevisionFileName = txtName.Text.Trim();
                            }

                            document.FilePath = document.FilePath.Replace(oldName, txtName.Text.Trim());

                            if (document.ID == objDoc.ID)
                            {
                                document.DocumentNumber = txtDocumentNumber.Text;
                                document.Title = txtTitle.Text;
                                document.RevisionID = Convert.ToInt32(ddlRevision.SelectedValue);
                                document.RevisionName = ddlRevision.SelectedItem.Text;
                                document.DocumentTypeID = Convert.ToInt32(ddlDocumentType.SelectedValue);
                                document.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                                document.DisciplineID = Convert.ToInt32(ddlDiscipline.SelectedValue);
                                document.ReceivedFromID = Convert.ToInt32(ddlReceivedFrom.SelectedValue);
                                document.ReceivedDate = txtReceivedDate.SelectedDate;
                                document.Remark = txtRemark.Text.Trim();
                                document.Well = txtWell.Text.Trim();
                                document.TransmittalNumber = txtTransmittalNumber.Text.Trim();

                                document.LastUpdatedBy = UserSession.Current.User.Id;
                                document.LastUpdatedDate = DateTime.Now;
                            }

                            this.documentService.Update(document);
                        }
                    }
                    else
                    {
                        objDoc.Name = txtName.Text.Trim();
                        objDoc.DocumentNumber = txtDocumentNumber.Text.Trim();
                        objDoc.Title = txtTitle.Text.Trim();
                        objDoc.RevisionID = Convert.ToInt32(ddlRevision.SelectedValue);
                        objDoc.RevisionName = ddlRevision.Text;
                        objDoc.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                        objDoc.DisciplineID = Convert.ToInt32(ddlDiscipline.SelectedValue);
                        objDoc.DocumentTypeID = Convert.ToInt32(ddlDocumentType.SelectedValue);
                        objDoc.ReceivedFromID = Convert.ToInt32(ddlReceivedFrom.SelectedValue);
                        objDoc.ReceivedDate = txtReceivedDate.SelectedDate;
                        objDoc.Remark = txtRemark.Text.Trim();
                        objDoc.Well = txtWell.Text.Trim();
                        objDoc.TransmittalNumber = txtTransmittalNumber.Text.Trim();
                        if (!string.IsNullOrEmpty(objDoc.RevisionName))
                        {
                            objDoc.RevisionFileName = objDoc.RevisionName + "_" + objDoc.Name;
                        }
                        else
                        {
                            objDoc.RevisionFileName = objDoc.Name;
                        }

                        objDoc.LastUpdatedBy = UserSession.Current.User.Id;
                        objDoc.LastUpdatedDate = DateTime.Now;

                        this.documentService.Update(objDoc);
                    }
                }
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
            }

            var rtvOptionalTypeDetail = (RadTreeView)this.rpbObjTree.FindItemByValue("ObjTree").FindControl("rtvOptionalTypeDetail");

            var listOptionalTypeDetail = this.optionalTypeDetailService.GetAllSpecial(ddlCategory.SelectedValue, 0, deparmentId).OrderBy(t => t.FullName).ToList();

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("HideSystem")) && ConfigurationManager.AppSettings.Get("HideSystem") == "true")
            {
                listOptionalTypeDetail = listOptionalTypeDetail.Where(t => t.OptionalTypeId != this.SystempOptType).ToList();
            }

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

            var listOptionalTypeDetail = this.optionalTypeDetailService.GetAllSpecial(e.Value, 0, deparmentId);

            rtvOptionalTypeDetail.DataSource = listOptionalTypeDetail;
            rtvOptionalTypeDetail.DataFieldParentID = "ParentId";
            rtvOptionalTypeDetail.DataTextField = "Name";
            rtvOptionalTypeDetail.DataValueField = "ID";
            rtvOptionalTypeDetail.DataFieldID = "ID";
            rtvOptionalTypeDetail.DataBind();

            this.LoadDocConfigGridView();
            this.grdDocument.Rebind();
        }

        protected void grdDocument_Init(object sender, EventArgs e)
        {
        }

        protected void grdDocument_DataBound(object sender, EventArgs e)
        {
            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            var selectedProperty = new List<string>();

            var ddlCategory = (RadComboBox)this.rpbObjTree.FindItemByValue("ObjTree").FindControl("ddlCategory");
            var categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            var deparmentId = isViewByGroup ? UserSession.Current.RoleId : 0;
            foreach (var docPropertiesView in this.docPropertiesViewService.GetAllSpecial(categoryId, deparmentId))
            {
                var temp = docPropertiesView.PropertyIndex.Split(',').Where(t => !string.IsNullOrEmpty(t.Trim())).Select(t => t.Trim()).ToList();
                selectedProperty.AddRange(temp);
            }

            selectedProperty = selectedProperty.Distinct().ToList();

            for (int i = 1; i <= 30; i++)
            {
                var column = this.grdDocument.MasterTableView.GetColumn("Index" + i);
                if (column != null)
                {
                    column.Visible = selectedProperty.Contains(i.ToString());
                }
            }
        }

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
            ////var selectedNodesId = e.Node.GetAllNodes().Select(t => Convert.ToInt32(t.Value)).ToList();
            ////selectedNodesId.Insert(0, Convert.ToInt32(e.Node.Value));
            ////var optionalTypeDetail = this.optionalTypeDetailService.GetById(Convert.ToInt32(e.Node.Value));

            ////var ddlCategory = (RadComboBox)this.rpbObjTree.FindItemByValue("ObjTree").FindControl("ddlCategory");
            ////var categoryId = Convert.ToInt32(ddlCategory.SelectedValue);

            ////if (optionalTypeDetail != null)
            ////{
            ////    var filterListId = this.optionalTypeDetailService.GetAllChildId(optionalTypeDetail.OptionalTypeId.GetValueOrDefault(), selectedNodesId);
            ////    var pageSize = this.grdDocument.PageSize;
            ////    var currentPage = this.grdDocument.CurrentPageIndex;
            ////    var startingRecordNumber = currentPage * pageSize;
            ////    var docList = new List<DocumentNew>();
            ////    switch (optionalTypeDetail.OptionalTypeId)
            ////    {
            ////        case 1:
            ////            docList = this.documentNewService.GetAllCurrentDoc(categoryId).Where(t => filterListId.Contains(t.PlantId.GetValueOrDefault())).ToList();
            ////            break;
            ////        case 2:
            ////            docList = this.documentNewService.GetAllCurrentDoc(categoryId).Where(t => filterListId.Contains(t.SystemId.GetValueOrDefault())).ToList();
            ////            break;
            ////        case 3:
            ////            docList =
            ////                this.documentNewService.GetAllCurrentDoc(categoryId).Where(
            ////                    t =>
            ////                    !string.IsNullOrEmpty(t.TagTypeId)
            ////                    && filterListId.Any(x => t.TagTypeId.Contains(x.ToString()))).ToList();
            ////            break;
            ////        case 4:
            ////            docList = this.documentNewService.GetAllCurrentDoc(categoryId).Where(t => filterListId.Contains(t.DisciplineId.GetValueOrDefault())).ToList();
            ////            break;
            ////        case 5:
            ////            docList = this.documentNewService.GetAllCurrentDoc(categoryId).Where(t => filterListId.Contains(t.DocumentTypeId.GetValueOrDefault())).ToList();
            ////            break;
            ////        case 6:
            ////            docList = this.documentNewService.GetAllCurrentDoc(categoryId).Where(t => filterListId.Contains(t.ProjectId.GetValueOrDefault())).ToList();
            ////            break;
            ////        case 7:
            ////            docList = this.documentNewService.GetAllCurrentDoc(categoryId).Where(t => filterListId.Contains(t.BlockId.GetValueOrDefault())).ToList();
            ////            break;
            ////        case 8:
            ////            docList = this.documentNewService.GetAllCurrentDoc(categoryId).Where(t => filterListId.Contains(t.FieldId.GetValueOrDefault())).ToList();
            ////            break;
            ////        case 9:
            ////            docList = this.documentNewService.GetAllCurrentDoc(categoryId).Where(t => filterListId.Contains(t.PlatformId.GetValueOrDefault())).ToList();
            ////            break;
            ////        case 10:
            ////            docList = this.documentNewService.GetAllCurrentDoc(categoryId).Where(t => filterListId.Contains(t.WellId.GetValueOrDefault())).ToList();
            ////            break;
            ////    }

            ////    this.grdDocument.VirtualItemCount = docList.Count;
            ////    this.grdDocument.DataSource = docList.OrderByDescending(t => t.ID).Skip(startingRecordNumber).Take(pageSize);
            ////    this.grdDocument.DataBind();
            ////}
            
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
                    foreach (var plant in listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.PlantOptType && !listParentId.Contains(t.ID)))
                    {
                        var plantItem = new RadComboBoxItem(plant.Name, plant.Name);
                        ddlFilterPlant.Items.Add(plantItem);
                    }
                }

                if (selectedProperty.Contains("8") && ddlFilterSystem != null)
                {
                    foreach (var system in listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.SystempOptType && !listParentId.Contains(t.ID)))
                    {
                        var systemItem = new RadComboBoxItem(system.Name, system.Name);
                        ddlFilterSystem.Items.Add(systemItem);
                    }
                }

                if (selectedProperty.Contains("9") && ddlFilterDiscipline != null)
                {
                    foreach (var discipline in listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.DisciplineOptType && !listParentId.Contains(t.ID)))
                    {
                        var disciplineItem = new RadComboBoxItem(discipline.Name, discipline.Name);
                        ddlFilterDiscipline.Items.Add(disciplineItem);
                    }
                }

                if (selectedProperty.Contains("10") && ddlFilterDocumentType != null)
                {
                    foreach (var docType in listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.DocumentTypeOptType && !listParentId.Contains(t.ID)))
                    {
                        var revItem = new RadComboBoxItem(docType.Name, docType.Name);
                        ddlFilterDocumentType.Items.Add(revItem);
                    }
                }

                if (selectedProperty.Contains("11") && ddlFilterTagType != null)
                {
                    foreach (var tagType in listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.TagOptType && !listParentId.Contains(t.ID)))
                    {
                        var revItem = new RadComboBoxItem(tagType.Name, tagType.Name);
                        ddlFilterTagType.Items.Add(revItem);
                    }
                }

                if (selectedProperty.Contains("12") && ddlFilterProject != null)
                {
                    foreach (var project in listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.ProjectOptType && !listParentId.Contains(t.ID)))
                    {
                        var projectItem = new RadComboBoxItem(project.Name, project.Name);
                        ddlFilterProject.Items.Add(projectItem);
                    }
                }

                if (selectedProperty.Contains("13") && ddlFilterBlock != null)
                {
                    foreach (var block in listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.BlockOptType && t.ParentId != null))
                    {
                        var blockItem = new RadComboBoxItem(block.Name, block.Name);
                        ddlFilterBlock.Items.Add(blockItem);
                    }
                }

                if (selectedProperty.Contains("14") && ddlFilterField != null)
                {
                    foreach (var field in listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.FieldOptType && t.ParentId != null))
                    {
                        var fieldItem = new RadComboBoxItem(field.Name, field.Name);
                        ddlFilterField.Items.Add(fieldItem);
                    }
                }

                if (selectedProperty.Contains("15") && ddlFilterPlatform != null)
                {
                    foreach (var platform in listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.PlatformOptType && t.ParentId != null))
                    {
                        var platformItem = new RadComboBoxItem(platform.Name, platform.Name);
                        ddlFilterPlatform.Items.Add(platformItem);
                    }
                }

                if (selectedProperty.Contains("16") && ddlFilterWell != null)
                {
                    foreach (var well in listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.WellOptType && t.ParentId != null))
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
                    foreach (var rig in listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.RIGOptType && t.ParentId != null))
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
    }
}

