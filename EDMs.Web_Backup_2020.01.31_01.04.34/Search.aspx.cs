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
    using System.ServiceProcess;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;
    using Telerik.Web.Zip;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class Search : Page
    {
        /// <summary>
        /// The revision service.
        /// </summary>
        private readonly RevisionService revisionService;

        /// <summary>
        /// The document type service.
        /// </summary>
        private readonly DocumentTypeService documentTypeService;

        /// <summary>
        /// The status service.
        /// </summary>
        private readonly StatusService statusService;

        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly DisciplineService disciplineService;

        /// <summary>
        /// The received from.
        /// </summary>
        private readonly ReceivedFromService receivedFromService;

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly DocumentService documentService;

        /// <summary>
        /// The category service.
        /// </summary>
        private readonly CategoryService categoryService;

        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string unreadPattern = @"\(\d+\)";

        protected const string ServiceName = "EDMSFolderWatcher";

        private readonly DocPropertiesViewService docPropertiesViewService;

        private readonly  OptionalTypeDetailService optionalTypeDetailService;

        private readonly OriginatorService originatorService;

        private readonly DocumentNewService documentNewService;

        private readonly AttachFileService attachFileService;

        private readonly GroupDataPermissionService groupDataPermissionService;

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

        public Search()
        {
            this.revisionService = new RevisionService();
            this.documentTypeService = new DocumentTypeService();
            this.documentService = new DocumentService();
            this.statusService = new StatusService();
            this.disciplineService = new DisciplineService();
            this.receivedFromService = new ReceivedFromService();
            this.categoryService = new CategoryService();
            this.docPropertiesViewService = new DocPropertiesViewService();
            this.optionalTypeDetailService = new OptionalTypeDetailService();
            this.originatorService = new OriginatorService();
            this.documentNewService = new DocumentNewService();
            this.attachFileService = new AttachFileService();
            this.groupDataPermissionService = new GroupDataPermissionService();
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
            this.Form.DefaultButton = this.btnSearch.UniqueID;
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            if (!Page.IsPostBack)
            {

                if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() && !UserSession.Current.User.Role.IsUpdate.GetValueOrDefault())
                {
                    ////this.grdDocument.MasterTableView.GetColumn("EditColumn").Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                }

                this.LoadComboData();

                this.LoadDocConfigGridView();
                this.LoadViewPropertiesConfig();
            }
        }

        /// <summary>
        /// Load all document by folder
        /// </summary>
        /// <param name="isbind">
        /// The isbind.
        /// </param>
        protected void LoadDocuments(int pageSize, int startingRecordNumber, bool isbind = false)
        {
            //if (ddlCategory.Items.Count > 0)
            //{
            //    var categoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
            //    var name = this.txtName.Text.Trim();
            //    var title = this.txtTitle.Text.Trim();
            //    var docNumber = this.txtDocumentNumber.Text.Trim();
            //    var keyword = this.txtKeyword.Text.Trim();
            //    var revisionId = Convert.ToInt32(this.ddlRevision.SelectedValue);
            //    var docTypeId = Convert.ToInt32(this.ddlDocumentType.SelectedValue);
            //    var statusId = Convert.ToInt32(this.ddlStatus.SelectedValue);
            //    var receiveFromId = Convert.ToInt32(this.ddlReceivedFrom.SelectedValue);
            //    var disciplineId = Convert.ToInt32(this.ddlDiscipline.SelectedValue);
            //    var languageId = Convert.ToInt32(this.ddlLanguage.SelectedValue);
            //    var dateFrom = this.txtDateFrom.SelectedDate;
            //    var dateTo = this.txtDateTo.SelectedDate;
            //    var transmittalNumber = this.txtTransmittalNumber.Text.Trim();
            //    var lisDoc = this.documentService.SearchDocument(
            //        categoryId,
            //        name,
            //        title,
            //        docNumber,
            //        keyword,
            //        revisionId,
            //        docTypeId,
            //        statusId,
            //        receiveFromId,
            //        disciplineId,
            //        languageId,
            //        dateFrom,
            //        dateTo,
            //        transmittalNumber,
            //        pageSize,
            //        startingRecordNumber);

            //    this.grdDocument.DataSource = lisDoc;
            //    if (isbind)
            //    {
            //        this.grdDocument.DataBind();
            //    }
            //}
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
                grdDocument.MasterTableView.SortExpressions.Clear();
                grdDocument.MasterTableView.GroupByExpressions.Clear();
                grdDocument.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                grdDocument.MasterTableView.SortExpressions.Clear();
                grdDocument.MasterTableView.GroupByExpressions.Clear();
                ////grdDocument.MasterTableView.CurrentPageIndex = grdDocument.MasterTableView.PageCount - 1;
                grdDocument.Rebind();
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
            if (this.ddlCategory.Items.Count > 0)
            {
                var categoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
                var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
                var deparmentId = isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                                      ? UserSession.Current.RoleId
                                      : 0;

                var name = this.txtName.Text.Trim();
                var description = this.txtDescription.Text.Trim();
                var revId = Convert.ToInt32(this.ddlRevision.SelectedValue);
                var vendorName = this.txtVendorName.Text.Trim();
                var drawingNumber = this.txtDrawingNumber.Text.Trim();
                var year = Convert.ToInt32(this.ddlYear.SelectedValue);
                var plantId = !string.IsNullOrEmpty(this.ddlPlant.SelectedValue)
                                  ? Convert.ToInt32(this.ddlPlant.SelectedValue)
                                  : 0;
                var systemId = !string.IsNullOrEmpty(this.ddlSystem.SelectedValue)
                                   ? Convert.ToInt32(this.ddlSystem.SelectedValue)
                                   : 0;
                var disciplineId = !string.IsNullOrEmpty(this.ddlDiscipline.SelectedValue)
                                       ? Convert.ToInt32(this.ddlDiscipline.SelectedValue)
                                       : 0;
                var documentTypeId = !string.IsNullOrEmpty(this.ddlDocumentType.SelectedValue)
                                         ? Convert.ToInt32(this.ddlDocumentType.SelectedValue)
                                         : 0;
                var tagTypeId = this.ddlTagType.SelectedValue;
                    //!string.IsNullOrEmpty(this.ddlTagType.SelectedValue) ? Convert.ToInt32(this.ddlTagType.SelectedValue) : 0;
                var projectId = !string.IsNullOrEmpty(this.ddlProject.SelectedValue)
                                    ? Convert.ToInt32(this.ddlProject.SelectedValue)
                                    : 0;
                var blockId = !string.IsNullOrEmpty(this.ddlBlock.SelectedValue)
                                  ? Convert.ToInt32(this.ddlBlock.SelectedValue)
                                  : 0;
                var fieldId = !string.IsNullOrEmpty(this.ddlField.SelectedValue)
                                  ? Convert.ToInt32(this.ddlField.SelectedValue)
                                  : 0;
                var platformId = !string.IsNullOrEmpty(this.ddlPlatform.SelectedValue)
                                     ? Convert.ToInt32(this.ddlPlatform.SelectedValue)
                                     : 0;
                var wellId = !string.IsNullOrEmpty(this.ddlWell.SelectedValue)
                                 ? Convert.ToInt32(this.ddlWell.SelectedValue)
                                 : 0;
                var rigId = !string.IsNullOrEmpty(this.ddlRIG.SelectedValue)
                                 ? Convert.ToInt32(this.ddlRIG.SelectedValue)
                                 : 0;

                var startDate = this.txtStartDate.SelectedDate;
                var endDate = this.txtEndDate.SelectedDate;
                var numberOfWork = Convert.ToInt32(this.txtNumberOfWork.Value);
                var tagNo = this.txtTagNo.Text.Trim();
                var tagDes = this.txtTagDes.Text.Trim();
                var manufacture = this.txtManufacturers.Text.Trim();
                var serialNo = this.txtSerialNo.Text.Trim();
                var modelNo = this.txtModelNo.Text.Trim();
                var assetNo = this.txtAssetNo.Text.Trim();
                var tableOfContent = this.txtTableOfContents.Text.Trim();
                var publishDate = this.txtPublishDate.SelectedDate;
                var fromId = Convert.ToInt32(this.ddlFrom.SelectedValue);
                var toId = Convert.ToInt32(this.ddlTo.SelectedValue);
                var signer = this.txtSigner.Text.Trim();
                var other = this.txtOther.Text.Trim();
                var kindOfRepair = this.txtKindOfRepair.Text.Trim();

                var searchFullFields = this.txtSearchFullField.Text.Trim();

                var pageSize = this.grdDocument.PageSize;
                var currentPage = this.grdDocument.CurrentPageIndex;
                var startingRecordNumber = currentPage * pageSize;

                var listDoc = this.documentNewService.SearchDocument(
                    categoryId,
                    deparmentId,
                    name,
                    description,
                    revId,
                    vendorName,
                    drawingNumber,
                    year,
                    plantId,
                    systemId,
                    disciplineId,
                    documentTypeId,
                    tagTypeId,
                    projectId,
                    blockId,
                    fieldId,
                    platformId,
                    wellId,
                    startDate,
                    endDate,
                    numberOfWork,
                    tagNo,
                    tagDes,
                    manufacture,
                    serialNo,
                    modelNo,
                    assetNo,
                    tableOfContent,
                    publishDate,
                    fromId,
                    toId,
                    signer,
                    other,
                    rigId,
                    kindOfRepair,
                    searchFullFields);
                this.grdDocument.VirtualItemCount = listDoc.Count;
                this.grdDocument.DataSource =
                    listDoc.OrderByDescending(t => t.ID).Skip(startingRecordNumber).Take(pageSize);
            }
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

            var listRelateDoc = this.documentService.GetAllRelateDocument(docId);
            if (listRelateDoc != null)
            {
                foreach (var objDoc in listRelateDoc)
                {
                    objDoc.IsDelete = true;
                    objDoc.LastUpdatedBy = UserSession.Current.User.Id;
                    objDoc.LastUpdatedDate = DateTime.Now;
                    this.documentService.Update(objDoc);
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
            string abc = e.CommandName;
        }

        /// <summary>
        /// The rad menu_ item click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void radMenu_ItemClick(object sender, RadMenuEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The btn search_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //var pageSize = this.grdDocument.PageSize;
            //var currentPage = this.grdDocument.CurrentPageIndex;
            //var startingRecordNumber = currentPage * pageSize;
            //this.grdDocument.VirtualItemCount = this.documentService.GetItemCount(
            //    this.CategoryID,
            //    this.Name,
            //    this.DocTitle,
            //    this.DocumentNumber,
            //    this.Keywords,
            //    this.RevisionID,
            //    this.DocumentTypeID,
            //    this.StatusID,
            //    this.ReceiveFromID,
            //    this.DisciplineID,
            //    this.LanguageID,
            //    this.DateFrom,
            //    this.DateTo,
            //    this.TransmittalNumber);
            //this.LoadDocuments(pageSize, startingRecordNumber, true);
            
            this.grdDocument.Rebind();
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData()
        {
            var selectedCategory = this.groupDataPermissionService.GetByRoleId(UserSession.Current.RoleId).Select(t => Convert.ToInt32(t.CategoryIdList)).ToList();
            var listCategory = this.categoryService.GetAll().Where(t => selectedCategory.Contains(t.ID));
            this.ddlCategory.DataSource = listCategory;
            this.ddlCategory.DataTextField = "Name";
            this.ddlCategory.DataValueField = "ID";
            this.ddlCategory.DataBind();

            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            ////var categoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
            var deparmentId = isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() ? UserSession.Current.RoleId.ToString() : "0";
            var listOptionalTypeDetail = this.optionalTypeDetailService.GetAllSpecial(this.ddlCategory.SelectedValue, 0, deparmentId);

            var revisionList = this.revisionService.GetAll();
            revisionList.Insert(0, new Revision() { Name = string.Empty });
            this.ddlRevision.DataSource = revisionList;
            this.ddlRevision.DataValueField = "ID";
            this.ddlRevision.DataTextField = "Name";
            this.ddlRevision.DataBind();

            this.rtvOptionalTypeDetailTemp.DataSource = listOptionalTypeDetail;
            this.rtvOptionalTypeDetailTemp.DataFieldParentID = "ParentId";
            this.rtvOptionalTypeDetailTemp.DataTextField = "Name";
            this.rtvOptionalTypeDetailTemp.DataValueField = "ID";
            this.rtvOptionalTypeDetailTemp.DataFieldID = "ID";
            this.rtvOptionalTypeDetailTemp.DataBind();

            for (var i = DateTime.Now.Year; i >= 1975; i--)
            {
                this.ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            this.ddlYear.Items.Insert(0, new ListItem(string.Empty, "0"));
            this.ddlYear.SelectedIndex = 0;

            // Bind Plant object
            this.BindTreeViewCombobox(this.PlantOptType, this.ddlPlant, "rtvPlant", listOptionalTypeDetail);

            // Bind System object
            this.BindTreeViewCombobox(this.SystempOptType, this.ddlSystem, "rtvSystem", listOptionalTypeDetail);

            // Bind TagType object
            this.BindTreeViewCombobox(this.TagOptType, this.ddlTagType, "rtvTagType", listOptionalTypeDetail);

            // Bind Discipline object
            this.BindTreeViewCombobox(this.DisciplineOptType, this.ddlDiscipline, "rtvDiscipline", listOptionalTypeDetail);

            // Bind Document type object
            this.BindTreeViewCombobox(this.DocumentTypeOptType, this.ddlDocumentType, "rtvDocumentType", listOptionalTypeDetail);

            // Bind Project object
            this.BindTreeViewCombobox(this.ProjectOptType, this.ddlProject, "rtvProject", listOptionalTypeDetail);

            // Bind Block object
            this.BindTreeViewCombobox(this.BlockOptType, this.ddlBlock, "rtvBlock", listOptionalTypeDetail);

            // Bind Field object
            this.BindTreeViewCombobox(this.FieldOptType, this.ddlField, "rtvField", listOptionalTypeDetail);

            // Bind Platform object
            this.BindTreeViewCombobox(this.PlatformOptType, this.ddlPlatform, "rtvPlatform", listOptionalTypeDetail);

            // Bind Well object
            this.BindTreeViewCombobox(this.WellOptType, this.ddlWell, "rtvWell", listOptionalTypeDetail);

            // Bind RIG object
            this.BindTreeViewCombobox(this.RIGOptType, this.ddlRIG, "rtvRIG", listOptionalTypeDetail);

            var listOriginator = this.originatorService.GetAll();
            listOriginator.Insert(0, new Originator() { ID = 0, Name = string.Empty });
            this.ddlFrom.DataSource = listOriginator;
            this.ddlFrom.DataValueField = "ID";
            this.ddlFrom.DataTextField = "Name";
            this.ddlFrom.DataBind();
            this.ddlFrom.SelectedIndex = 0;

            this.ddlTo.DataSource = listOriginator;
            this.ddlTo.DataValueField = "ID";
            this.ddlTo.DataTextField = "Name";
            this.ddlTo.DataBind();
            this.ddlTo.SelectedIndex = 0;
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
                var watcherService = new ServiceController("EDMSFolderWatcher");
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
                        if (watcherService.Status == ServiceControllerStatus.Running)
                        {
                            watcherService.ExecuteCommand(128);
                        }

                        if (File.Exists(filePath))
                        {
                            File.Move(filePath, filePath.Replace(oldName, txtName.Text.Trim()));
                        }

                        if (watcherService.Status == ServiceControllerStatus.Running)
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

        protected void grdDocument_ItemDataBound(object sender, GridItemEventArgs e)
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

                txtReceivedDate.DatePopupButton.Visible = false;

                var name = (item.FindControl("Name") as HiddenField).Value;
                var revisionId = (item.FindControl("RevisionID") as HiddenField).Value;
                var statusId = (item.FindControl("StatusID") as HiddenField).Value;
                var disciplineId = (item.FindControl("DisciplineID") as HiddenField).Value;
                var documentTypeId = (item.FindControl("DocumentTypeID") as HiddenField).Value;
                var receivedFromId = (item.FindControl("ReceivedFromID") as HiddenField).Value;
                var receivedDate = (item.FindControl("ReceivedDate") as HiddenField).Value;
                var well = (item.FindControl("Well") as HiddenField).Value;
                var remark = (item.FindControl("Remark") as HiddenField).Value;
                var title = (item.FindControl("Title") as HiddenField).Value;
                var documentNumber = (item.FindControl("DocumentNumber") as HiddenField).Value;
                var transmittalNumber = (item.FindControl("TransmittalNumber") as HiddenField).Value;

                if (!string.IsNullOrEmpty(receivedDate))
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

                var statusList = this.statusService.GetAll();
                statusList.Insert(0, new Status { Name = string.Empty });
                ddlStatus.DataSource = statusList;
                ddlStatus.DataValueField = "ID";
                ddlStatus.DataTextField = "Name";
                ddlStatus.DataBind();
                ddlStatus.SelectedValue = statusId;

                var receivedFromList = this.receivedFromService.GetAll();
                receivedFromList.Insert(0, new ReceivedFrom() { Name = string.Empty });
                ddlReceivedFrom.DataSource = receivedFromList;
                ddlReceivedFrom.DataValueField = "ID";
                ddlReceivedFrom.DataTextField = "Name";
                ddlReceivedFrom.DataBind();
                ddlReceivedFrom.SelectedValue = receivedFromId;

                var disciplineList = this.disciplineService.GetAll();
                disciplineList.Insert(0, new Discipline() { Name = string.Empty });
                ddlDiscipline.DataSource = disciplineList;
                ddlDiscipline.DataValueField = "ID";
                ddlDiscipline.DataTextField = "Name";
                ddlDiscipline.DataBind();
                ddlDiscipline.SelectedValue = disciplineId;
            }
        }

        protected void grdDocument_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
           //// this.LoadDocuments(true);
        }

        private void LoadDocConfigGridView()
        {
            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            var selectedProperty = new List<string>();

            if (this.ddlCategory.Items.Count > 0)
            {
                var categoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
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

        /// <summary>
        /// The ddl category_ selected index changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void ddlCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            this.LoadViewPropertiesConfig();
            this.LoadDocConfigGridView();

            this.grdDocument.Rebind();
        }

        protected void rtvProject_Nodeclick(object sender, RadTreeNodeEventArgs e)
        {
            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            var deparmentId = isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() ? UserSession.Current.RoleId.ToString() : "0";
            var listOptionalTypeDetail = this.optionalTypeDetailService.GetAllSpecial(this.ddlCategory.SelectedValue, 0, deparmentId);

            var selectedNode = this.rtvOptionalTypeDetailTemp.FindNodeByValue(e.Node.Value);
            var childNodeId = selectedNode.GetAllNodes().Select(t => Convert.ToInt32(t.Value));

            var listTemp = listOptionalTypeDetail.Where(t => childNodeId.Contains(t.ID) && t.OptionalTypeId == 7).ToList();
            this.RepairList(ref listTemp);

            var rtvobj = (RadTreeView)this.ddlBlock.Items[0].FindControl("rtvBlock");
            if (rtvobj != null)
            {
                rtvobj.DataSource = listTemp;
                rtvobj.DataFieldParentID = "ParentId";
                rtvobj.DataTextField = "Name";
                rtvobj.DataValueField = "ID";
                rtvobj.DataFieldID = "ID";
                rtvobj.DataBind();
            }
        }

        protected void rtvBlock_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            var deparmentId = isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() ? UserSession.Current.RoleId.ToString() : "0";
            var listOptionalTypeDetail = this.optionalTypeDetailService.GetAllSpecial(this.ddlCategory.SelectedValue, 0, deparmentId);

            var selectedNode = this.rtvOptionalTypeDetailTemp.FindNodeByValue(e.Node.Value);
            var childNodeId = selectedNode.GetAllNodes().Select(t => Convert.ToInt32(t.Value));

            var listTemp = listOptionalTypeDetail.Where(t => childNodeId.Contains(t.ID) && t.OptionalTypeId == 8).ToList();
            this.RepairList(ref listTemp);

            var rtvobj = (RadTreeView)this.ddlField.Items[0].FindControl("rtvField");
            if (rtvobj != null)
            {
                rtvobj.DataSource = listTemp;
                rtvobj.DataFieldParentID = "ParentId";
                rtvobj.DataTextField = "Name";
                rtvobj.DataValueField = "ID";
                rtvobj.DataFieldID = "ID";
                rtvobj.DataBind();
            }
        }

        protected void rtvField_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            var deparmentId = isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() ? UserSession.Current.RoleId.ToString() : "0";
            var listOptionalTypeDetail = this.optionalTypeDetailService.GetAllSpecial(this.ddlCategory.SelectedValue, 0, deparmentId);

            var selectedNode = this.rtvOptionalTypeDetailTemp.FindNodeByValue(e.Node.Value);
            var childNodeId = selectedNode.GetAllNodes().Select(t => Convert.ToInt32(t.Value));

            var listTemp = listOptionalTypeDetail.Where(t => childNodeId.Contains(t.ID) && t.OptionalTypeId == 9).ToList();
            this.RepairList(ref listTemp);

            var rtvobj = (RadTreeView)this.ddlPlatform.Items[0].FindControl("rtvPlatform");
            if (rtvobj != null)
            {
                rtvobj.DataSource = listTemp;
                rtvobj.DataFieldParentID = "ParentId";
                rtvobj.DataTextField = "Name";
                rtvobj.DataValueField = "ID";
                rtvobj.DataFieldID = "ID";
                rtvobj.DataBind();
            }
        }

        protected void rtvPlatform_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            var deparmentId = isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() ? UserSession.Current.RoleId.ToString() : "0";
            var listOptionalTypeDetail = this.optionalTypeDetailService.GetAllSpecial(this.ddlCategory.SelectedValue, 0, deparmentId);

            var selectedNode = this.rtvOptionalTypeDetailTemp.FindNodeByValue(e.Node.Value);
            var childNodeId = selectedNode.GetAllNodes().Select(t => Convert.ToInt32(t.Value));

            var listTemp = listOptionalTypeDetail.Where(t => childNodeId.Contains(t.ID) && t.OptionalTypeId == 10).ToList();
            this.RepairList(ref listTemp);

            var rtvobj = (RadTreeView)this.ddlWell.Items[0].FindControl("rtvWell");
            if (rtvobj != null)
            {
                rtvobj.DataSource = listTemp;
                rtvobj.DataFieldParentID = "ParentId";
                rtvobj.DataTextField = "Name";
                rtvobj.DataValueField = "ID";
                rtvobj.DataFieldID = "ID";
                rtvobj.DataBind();
            }
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
        

        private void RepairList(ref List<OptionalTypeDetail> listOptionalTypeDetail)
        {
            var temp = listOptionalTypeDetail.Where(t => t.ParentId != null).Select(t => t.ParentId).Distinct().ToList();
            var temp2 = listOptionalTypeDetail.Select(t => t.ID).ToList();

            foreach (var x in temp)
            {
                if (!temp2.Contains(x.Value))
                {
                    var tempList = listOptionalTypeDetail.Where(t => t.ParentId == x.Value).ToList();
                    foreach (var optionalTypeDetail in tempList)
                    {
                        optionalTypeDetail.ParentId = null;
                    }
                }
            }
        }

        private void LoadViewPropertiesConfig()
        {
            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            var selectedProperty = new List<string>();
            if (this.ddlCategory.Items.Count > 0)
            {
                var categoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
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
                    var property = (HtmlGenericControl)this.divContent.FindControl("Index" + i);
                    if (property != null)
                    {
                        property.Visible = selectedProperty.Contains(i.ToString());
                    }
                }
            }
        }

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

        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("Search.aspx");
        }
    }
}

