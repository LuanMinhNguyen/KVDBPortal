// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Cells;
using EDMs.Business.Services.CostContract;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;
using Telerik.Web.Zip;
using CheckBox = System.Web.UI.WebControls.CheckBox;
using Label = System.Web.UI.WebControls.Label;
using TextBox = System.Web.UI.WebControls.TextBox;

namespace EDMs.Web.Controls.CostContract
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class ContractList : Page
    {
        private readonly OptionalTypeService optionalTypeService = new OptionalTypeService();

        private readonly PermissionService permissionService = new PermissionService();

        private readonly RevisionService revisionService = new RevisionService();

        private readonly FolderService folderService = new FolderService();

        private readonly DocumentNewService documentNewService = new DocumentNewService();

        private readonly GroupDataPermissionService groupDataPermissionService = new GroupDataPermissionService();

        private readonly AttachFileService attachFileService = new AttachFileService();

        private readonly CostContractProjectService projectService = new CostContractProjectService();


        private readonly DocumentPackageService documentPackageService = new DocumentPackageService();

        private readonly ProcurementRequirementService prService = new ProcurementRequirementService();

        private readonly RoleService roleService = new RoleService();

        private readonly PermissionContractService permissionContractService = new PermissionContractService();

        private readonly ContractorService contractorService = new ContractorService();

        protected const string ServiceName = "EDMSFolderWatcher";

        private readonly ContractService contractService = new ContractService();

        public static RadTreeNode editedNode = null;

        private readonly PaymentHistoryService paymentHistoryService = new PaymentHistoryService();

        private readonly ProcurementRequirementTypeService prTypeService = new ProcurementRequirementTypeService();

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
            if (this.rtvPR.SelectedNode != null)
            {
                var allContractList = this.contractService.GetAllByPR(Convert.ToInt32(this.rtvPR.SelectedValue)).OrderBy(t => t.Number).ToList();
                if (UserSession.Current.User.Role.IsAdmin.GetValueOrDefault())
                {
                    this.grdDocument.DataSource = allContractList;
                }
                else
                {
                    var contractInPermission = this.permissionContractService.GetAllByUser(UserSession.Current.User.Id).Select(t => t.ContractID).ToList();
                    this.grdDocument.DataSource = allContractList.Where(t => contractInPermission.Contains(t.ID));
                }
            }
            else
            {
                if (UserSession.Current.User.Role.IsAdmin.GetValueOrDefault())
                {
                    var listPRId =
                        this.prService.GetAllByProject(Convert.ToInt32(this.ddlProject.SelectedValue))
                            .Select(t => t.ID)
                            .ToList();
                    var contractList = this.contractService.GetAllByPR(listPRId).OrderBy(t => t.Number).ToList();
                    this.grdDocument.DataSource = contractList;
                }
                else
                {
                    var listPRId = this.prService.GetAllPRInPermission(UserSession.Current.User.Id, !string.IsNullOrEmpty(this.ddlProject.SelectedValue) ? Convert.ToInt32(this.ddlProject.SelectedValue) : 0).Select(t => t.ID).ToList();
                    var contractList = this.contractService.GetAllByPR(listPRId).OrderBy(t => t.Number).ToList();

                    var contractIdListInPermission =
                        this.permissionContractService.GetAllByUser(UserSession.Current.User.Id)
                            .Select(t => t.ContractID)
                            .ToList();
                    this.grdDocument.DataSource = contractList.Where(t => contractIdListInPermission.Contains(t.ID));
                }
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
                this.grdDocument.Rebind();
            }
            else if (e.Argument == "ExportEMDRReport_New")
            {
                this.ExportEMDRReportNew();
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
                this.rtvPR.UnselectAllNodes();
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
            e.Node.ImageUrl = "~/Images/folderdir16.png";
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
                nodeFolder.ImageUrl = "~/Images/folderdir16.png";
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
            var projectInPermission = UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                ? this.projectService.GetAll().OrderBy(t => t.Name)
                : this.projectService.GetAllInPermission(UserSession.Current.User.Id).OrderBy(t => t.Name);

            this.ddlProject.DataSource = projectInPermission;
            this.ddlProject.DataTextField = "Name";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();
            if (this.ddlProject.SelectedItem != null)
            {
                var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);
                this.lblProjectId.Value = projectId.ToString();
                var listPRInPermission = UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                    ? this.prService.GetAllByProject(projectId).OrderBy(t => t.Number).ToList()
                    : this.prService.GetAllPRInPermission(UserSession.Current.User.Id, !string.IsNullOrEmpty(this.ddlProject.SelectedValue) ? projectId : 0)
                    .OrderBy(t => t.Number).ToList();

                this.rtvPR.DataSource = listPRInPermission;
                this.rtvPR.DataTextField = "Number";
                this.rtvPR.DataValueField = "ID";
                this.rtvPR.DataFieldID = "ID";
                this.rtvPR.DataBind();

                this.InitGridContextMenu(projectId);
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
                ? this.prService.GetAllByProject(projectId).OrderBy(t => t.Number).ToList()
                : this.prService.GetAllPRInPermission(UserSession.Current.User.Id, !string.IsNullOrEmpty(this.ddlProject.SelectedValue) ? projectId : 0)
                .OrderBy(t => t.Number).ToList();

            this.rtvPR.DataSource = listDisciplineInPermission;
            this.rtvPR.DataTextField = "Number";
            this.rtvPR.DataValueField = "ID";
            this.rtvPR.DataFieldID = "ID";
            this.rtvPR.DataBind();
            this.grdDocument.Rebind();


        }

        protected void grdDocument_Init(object sender, EventArgs e)
        {
        }

        protected void grdDocument_DataBound(object sender, EventArgs e)
        {
        }

        protected void rtvPR_NodeClick(object sender, RadTreeNodeEventArgs e)
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

        protected void rtvPR_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = @"~/Images/shopping.png";
        }

        protected void ddlProject_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.ImageUrl = @"~/Images/project.png";
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
                    this.grdDocument.MasterTableView.ColumnGroups[0].HeaderText = contractor.Name + " - VSP";
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

        private void ExportEMDRReportNew()
        {
            var filePath = Server.MapPath(@"..\..\Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\ContractReportTemplate.xls");

            var dataSheet = workbook.Worksheets[0];

            var projectID = Convert.ToInt32(this.ddlProject.SelectedValue);
            var prTypeList = this.prTypeService.GetAllByProject(projectID);
            var countprType = 1;
            var countRow = 0;
            foreach (var prType in prTypeList)
            {
                // Insert prType info
                dataSheet.Cells["B" + (8 + countRow)].PutValue(1);
                dataSheet.Cells["C" + (8 + countRow)].PutValue(countprType);
                dataSheet.Cells["D" + (8 + countRow)].PutValue(prType.FullName);
                countRow += 1;

                // Get prList
                var prList = this.prService.GetAllByProjectAndType(projectID, prType.ID);
                var countPr = 1;
                foreach (var prObj in prList)
                {
                    dataSheet.Cells["C" + (8 + countRow)].PutValue(countprType + "." + countPr);
                    dataSheet.Cells["D" + (8 + countRow)].PutValue(prObj.Number);
                    dataSheet.Cells["E" + (8 + countRow)].PutValue(prObj.Description);
                    dataSheet.Cells["F" + (8 + countRow)].PutValue(prObj.Code);
                    dataSheet.Cells["G" + (8 + countRow)].PutValue(prObj.MainOwnerName);
                    dataSheet.Cells["H" + (8 + countRow)].PutValue(prObj.ProcurementPlanValue);
                    dataSheet.Cells["I" + (8 + countRow)].PutValue(prObj.ProcurementRequirementValue);
                    dataSheet.Cells["J" + (8 + countRow)].PutValue(prObj.USDExchangeValue);
                    dataSheet.Cells["K" + (8 + countRow)].PutValue(prObj.ContractorChoiceTypeName);

                    // Get contract List
                    var contractList = this.contractService.GetAllByPR(prObj.ID);
                    var countContractList = contractList.Any() ? contractList.Count : 1;
                    // Merge pr info
                    dataSheet.Cells.Merge(7 + countRow, 2, countContractList, 1);
                    dataSheet.Cells.Merge(7 + countRow, 3, countContractList, 1);
                    dataSheet.Cells.Merge(7 + countRow, 4, countContractList, 1);
                    dataSheet.Cells.Merge(7 + countRow, 5, countContractList, 1);
                    dataSheet.Cells.Merge(7 + countRow, 6, countContractList, 1);
                    dataSheet.Cells.Merge(7 + countRow, 7, countContractList, 1);
                    dataSheet.Cells.Merge(7 + countRow, 8, countContractList, 1);
                    dataSheet.Cells.Merge(7 + countRow, 9, countContractList, 1);
                    dataSheet.Cells.Merge(7 + countRow, 10, countContractList, 1);
                    

                    for (int i = 0; i < contractList.Count; i++)
                    {
                        dataSheet.Cells["L" + (8 + countRow)].PutValue(contractList[i].Number);
                        dataSheet.Cells["M" + (8 + countRow)].PutValue(contractList[i].ContractorSelectedName);
                        dataSheet.Cells["N" + (8 + countRow)].PutValue(contractList[i].ContractContent);
                        dataSheet.Cells["O" + (8 + countRow)].PutValue(contractList[i].DeliveryDate != null 
                            ? contractList[i].DeliveryDate.Value.ToString("dd/MM/yyyy")
                            : string.Empty);
                        dataSheet.Cells["P" + (8 + countRow)].PutValue(contractList[i].DeliveryStatus);
                        dataSheet.Cells["Q" + (8 + countRow)].PutValue(contractList[i].EffectedDate != null
                            ? contractList[i].EffectedDate.Value.ToString("dd/MM/yyyy")
                            : string.Empty);
                        dataSheet.Cells["R" + (8 + countRow)].PutValue(contractList[i].EndDate != null
                            ? contractList[i].EndDate.Value.ToString("dd/MM/yyyy")
                            : string.Empty);
                        dataSheet.Cells["S" + (8 + countRow)].PutValue(contractList[i].ContractTypeName);
                        dataSheet.Cells["T" + (8 + countRow)].PutValue(contractList[i].ContractStausName);
                        dataSheet.Cells["U" + (8 + countRow)].PutValue(contractList[i].ContractValueVND);
                        dataSheet.Cells["V" + (8 + countRow)].PutValue(contractList[i].ContractValueUSD);
                        dataSheet.Cells["W" + (8 + countRow)].PutValue(contractList[i].ArisingTotalValue);
                        dataSheet.Cells["X" + (8 + countRow)].PutValue(contractList[i].ContractTotalValue);
                        dataSheet.Cells["Y" + (8 + countRow)].PutValue(contractList[i].ExchangeRate);
                        dataSheet.Cells["Z" + (8 + countRow)].PutValue(contractList[i].PaymentedValueVND);
                        dataSheet.Cells["AA" + (8 + countRow)].PutValue(contractList[i].PaymentedValueUSD);
                        dataSheet.Cells["AB" + (8 + countRow)].PutValue(contractList[i].PaymentValueUSDExchange);
                        dataSheet.Cells["AC" + (8 + countRow)].PutValue(contractList[i].RemainPaymentVND);
                        dataSheet.Cells["AD" + (8 + countRow)].PutValue(contractList[i].RemainPaymentUSD);
                        dataSheet.Cells["AE" + (8 + countRow)].PutValue(contractList[i].RemainPaymentUSDExchange);
                        dataSheet.Cells["AF" + (8 + countRow)].PutValue(contractList[i].DeferenceWithPRValue);
                        dataSheet.Cells["AG" + (8 + countRow)].PutValue(contractList[i].Note);

                        var paymentList = this.paymentHistoryService.GetAllByContract(contractList[i].ID).OrderBy(t => t.PlanDate).ToList();
                        for (int j = 0; j < paymentList.Count; j++)
                        {
                            dataSheet.Cells[countRow,34 + j * 6].PutValue(paymentList[j].ActualValueVND);
                            dataSheet.Cells[countRow,35 + j * 6].PutValue(paymentList[j].ActualValueUSD);
                            dataSheet.Cells[countRow,36 + j * 6].PutValue(paymentList[j].ActualDate != null
                                ? paymentList[j].ActualDate.Value.ToString("dd/MM/yyyy")
                                : string.Empty);

                            dataSheet.Cells[countRow, 37 + j * 6].PutValue(paymentList[j].ActualValueVND);
                            dataSheet.Cells[countRow, 38 + j * 6].PutValue(paymentList[j].PlanValueUSD);
                            dataSheet.Cells[countRow, 39 + j * 6].PutValue(paymentList[j].PlanDate != null
                                ? paymentList[j].PlanDate.Value.ToString("dd/MM/yyyy")
                                : string.Empty);
                        }

                        if (i != contractList.Count - 1)
                        {
                            countRow += 1;
                        }
                    }

                    countPr += 1;
                    countRow += 1;
                }

                countprType += 1;
            }

            dataSheet.Cells["A7"].PutValue(countRow);
            dataSheet.AutoFitRows();
            var filename = "Contract Report " + DateTime.Now.ToString("dd-MM-yyyy") + ".xls";
            workbook.Save(filePath + filename);
            this.DownloadByWriteByte(filePath + filename, filename, true);
        }

        protected void ckbShowAll_CheckedChange(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }
    }
}