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
using EDMs.Business.Services.WMS;
using EDMs.Business.Services.Workflow;

namespace EDMs.Web
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
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
    public partial class MR : Page
    {
        private readonly RevisionService revisionService = new RevisionService();

        private readonly DocumentTypeService documentTypeService = new DocumentTypeService();

        private readonly FolderService folderService = new FolderService();

        private readonly NotificationRuleService notificationRuleService = new NotificationRuleService();

        private readonly GroupDataPermissionService groupDataPermissionService = new GroupDataPermissionService();

        private readonly UserService userService = new UserService();

        private readonly AttachFilesPackageService attachFilesPackageService = new AttachFilesPackageService();

        private readonly ScopeProjectService scopeProjectService = new ScopeProjectService();

        private readonly PackageService packageService = new PackageService();

        private readonly DocumentPackageService documentPackageService = new DocumentPackageService();

        private readonly DisciplineService disciplineService = new DisciplineService();

        private readonly RoleService roleService = new RoleService();

        private readonly TemplateManagementService templateManagementService = new TemplateManagementService();

        private readonly ContractorService contractorService = new ContractorService();

        private readonly DocumentNumberingService documentNumberingService = new DocumentNumberingService();

        private readonly OriginatorService originatorService = new OriginatorService();

        private readonly StatusService statusService = new StatusService();

        private readonly MaterialRequisitionService mrService = new MaterialRequisitionService();

        private readonly ObjectAssignedUserService objAssignedUserService = new ObjectAssignedUserService();

        private readonly FunctionPermissionService fncPermissionService = new FunctionPermissionService();

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
            Session.Add("SelectedMainMenu", "Working Management");
            var temp = (RadPane)this.Master.FindControl("leftPane");
            temp.Collapsed = true;
            if (!Page.IsPostBack)
            {
                this.GetFuncPermissionConfig();
                this.LoadObjectTree();
            }
        }

        private void GetFuncPermissionConfig()
        {
            // Get Function permission of user for MR Module
            var fncPermission = this.fncPermissionService.GetOne(UserSession.Current.User.Id, 1);
            if (fncPermission != null)
            {
                this.IsView.Value = fncPermission.IsView.GetValueOrDefault().ToString();
                this.IsCreate.Value = fncPermission.IsCreate.GetValueOrDefault().ToString();
                this.IsUpdate.Value = fncPermission.IsUpdate.GetValueOrDefault().ToString();
                this.IsCancel.Value = fncPermission.IsCancel.GetValueOrDefault().ToString();
                this.IsAttachWF.Value = fncPermission.IsAttachWorkflow.GetValueOrDefault().ToString();
            }
            else
            {
                this.IsView.Value = "False";
                this.IsCreate.Value = "False";
                this.IsUpdate.Value = "False";
                this.IsCancel.Value = "False";
                this.IsAttachWF.Value = "False";
            }
            // ----------------------------------------------------------------------------------------
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
            else if (e.Argument.Contains("Cancel"))
            {
                var ecrId = new Guid(e.Argument.Split('_')[1]);
                var ecrObj = this.mrService.GetById(ecrId);
                if (ecrObj != null)
                {
                    ecrObj.IsCancel = true;

                    this.mrService.Update(ecrObj);
                    this.grdDocument.Rebind();
                }

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
            var ddlProject = (RadComboBox)this.CustomerMenu.Items[2].FindControl("ddlProject");
            if (ddlProject != null && ddlProject.SelectedItem != null)
            {
                if (this.IsView.Value == "True")
                {
                    var ddlStatus = (DropDownList)this.CustomerMenu.Items[2].FindControl("ddlStatus");
                    var txtSearchAllField = (TextBox)this.CustomerMenu.Items[2].FindControl("txtSearchAllField");

                    var projectId = Convert.ToInt32(ddlProject.SelectedValue);
                    var objList = new List<MaterialRequisition>();
                    switch (ddlStatus.SelectedValue)
                    {
                        case "1":
                            objList = this.mrService.GetAll(projectId, txtSearchAllField.Text.Trim());
                            break;
                        case "4":
                            var overDueTask = this.objAssignedUserService.GetAllOverDueTask("Material Requisition").Select(t => t.ObjectID).ToList();
                            objList = this.mrService.GetAll(projectId, txtSearchAllField.Text.Trim()).Where(t => overDueTask.Contains(t.ID)).ToList();
                            break;
                        case "5":
                            objList = this.mrService.GetAllCompletedMR(projectId, txtSearchAllField.Text.Trim());
                            break;
                        case "6":
                            objList = this.mrService.GetAllIncompleteMR(projectId, txtSearchAllField.Text.Trim());
                            break;

                        case "7":
                            objList = this.mrService.GetAllCompletedMRWaitingPrint(projectId, txtSearchAllField.Text.Trim());
                            break;
                    }

                    // Set limited data view for special department
                    if (UserSession.Current.User.Role.IsLimitedView.GetValueOrDefault())
                    {
                        var userIdList = this.userService.GetAllByRoleId(UserSession.Current.RoleId).Select(t => t.Id);
                        var mrInPermissionIdList =
                            this.objAssignedUserService.GetAll()
                                .Where(t => userIdList.Contains(t.UserID.GetValueOrDefault()))
                                .Select(t => t.ObjectID.GetValueOrDefault())
                                .Distinct()
                                .ToList();

                        objList = objList.Where(t => mrInPermissionIdList.Contains(t.ID)).ToList();
                    }
                    // --------------------------------------------------------------------------------------------------------

                    this.grdDocument.DataSource = objList.OrderByDescending(t => t.CreatedByDate);
                }
                else
                {
                    this.grdDocument.DataSource = new List<MaterialRequisition>();
                }
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
            if (e.Item is GridDataItem)
            {
                var item = e.Item as GridDataItem;
                if (item["PriorityId"].Text == "1")
                {
                    item["PriorityName"].BackColor = Color.Aqua;
                    item["PriorityName"].BorderColor = Color.Aqua;
                } 
                else if (item["PriorityId"].Text == "2")
                {
                    item["PriorityName"].BackColor = Color.DarkOrange;
                    item["PriorityName"].BorderColor = Color.DarkOrange;
                }
                else if (item["PriorityId"].Text == "3")
                {
                    item["PriorityName"].BackColor = Color.Red;
                    item["PriorityName"].BorderColor = Color.Red;
                }

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

        private void LoadObjectTree()
        {
            var ddlProject = (RadComboBox)this.CustomerMenu.Items[2].FindControl("ddlProject");
            var projectInPermission = this.scopeProjectService.GetAll().OrderBy(t => t.Name).ToList();

            if (UserSession.Current.User.LocationId == 2)
            {
                projectInPermission = projectInPermission.Where(t => t.ID == UserSession.Current.User.ProjectId.GetValueOrDefault()).ToList();
            }

            ddlProject.DataSource = projectInPermission;
            ddlProject.DataTextField = "FullName";
            ddlProject.DataValueField = "ID";
            ddlProject.DataBind();

            if (Session["SelectedProject"] != null)
            {
                ddlProject.SelectedValue = Session["SelectedProject"].ToString();
            }

            if (ddlProject.SelectedItem != null)
            {
                int projectId = Convert.ToInt32(ddlProject.SelectedValue);
                this.lblProjectId.Value = projectId.ToString();

                Session.Add("SelectedProject", projectId);
            }

            // Show hide function Control
            this.CustomerMenu.Items[0].Visible = Convert.ToBoolean(this.IsCreate.Value);
            // --------------------------------------------------------------------------
        }

        protected void ddlProject_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var ddlProject = (RadComboBox)this.CustomerMenu.Items[2].FindControl("ddlProject");
            int projectId = Convert.ToInt32(ddlProject.SelectedValue);
            this.lblProjectId.Value = projectId.ToString();
            this.grdDocument.Rebind();

            Session.Add("SelectedProject", projectId);
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

        protected void rtvDiscipline_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = @"Images/group.png";
        }

        protected void ddlProject_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.ImageUrl = @"~/Images/project.png";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
        }

        protected void ddlStatus_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }
    }
}