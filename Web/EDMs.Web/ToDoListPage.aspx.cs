// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Drawing;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.WMS;
using EDMs.Business.Services.Workflow;
using Telerik.Windows.Zip;
namespace EDMs.Web
{
    using System;
    using System.Web;
    using System.IO;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web.UI;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;
    using EDMs.Web.Utilities;
    using System.Web.UI.WebControls;

    using Telerik.Web.UI;
    using CheckBox = System.Web.UI.WebControls.CheckBox;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class ToDoListPage : Page
    {
        private readonly OptionalTypeService optionalTypeService = new OptionalTypeService();

        private readonly FolderService folderService = new FolderService();

        private readonly GroupDataPermissionService groupDataPermissionService = new GroupDataPermissionService();

        private readonly UserService userService = new UserService();

        private readonly DisciplineService disciplineService = new DisciplineService();

        private readonly ContractorService contractorService = new ContractorService();

        private readonly ObjectAssignedWorkflowService objAssignedWorkflowService = new ObjectAssignedWorkflowService();

        private readonly ObjectAssignedUserService objAssignedUserService = new ObjectAssignedUserService();
        private readonly WorkflowDetailService wfDetailService = new WorkflowDetailService();
        private readonly WorkflowStepService wfStepService = new WorkflowStepService();
        private readonly WorkflowService wfService = new WorkflowService();

        private readonly MaterialRequisitionService mrService = new MaterialRequisitionService();

        private readonly WorkRequestService wrService = new WorkRequestService();

        protected const string ServiceName = "EDMSFolderWatcher";

        public static RadTreeNode editedNode = null;

        private readonly TrackingMorningCallService morningCallService = new TrackingMorningCallService();
        private readonly TrackingWCRService wcrService = new TrackingWCRService();
        private readonly TrackingPunchService punchService = new TrackingPunchService();
        private readonly TrackingSailService sailService = new TrackingSailService();
        private readonly TrackingProcedureService procedureService = new TrackingProcedureService();
        private readonly TrackingGeneralWorkingService generalWorkingService = new TrackingGeneralWorkingService();

        private readonly DQREDocumentService dqreDocumentService = new DQREDocumentService();

        private readonly HashSet<DateTime> Holidays = new HashSet<DateTime>();

        private readonly DistributionMatrixService matrixService = new DistributionMatrixService();

        private readonly DistributionMatrixDetailService matrixDetailService = new DistributionMatrixDetailService();

        private readonly ObjectAssignedWorkflowService objAssignedWfService = new ObjectAssignedWorkflowService();
        private readonly PECC2TransmittalService pecc2TransmittalService = new PECC2TransmittalService();
        private readonly PECC2DocumentsService PECC2DocumentService = new PECC2DocumentsService();
        private readonly ChangeRequestService changeRequestService= new ChangeRequestService();
        private readonly NCR_SIService ncrSiService= new NCR_SIService();

        private readonly ContractorTransmittalDocFileService contractorTransmittalDocFileService= new ContractorTransmittalDocFileService();

        private readonly ContractorTransmittalAttachFileService contractorTransmittalAttachFileService =new ContractorTransmittalAttachFileService();

        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string UnreadPattern = @"\(\d+\)";
        private List<ObjectAssignedUser> WorkAssigned
        {
            get { return UserSession.Current.User.IsAdmin.GetValueOrDefault()?
                    this.objAssignedUserService.GetAllIncompleteByAdmin(): this.objAssignedUserService.GetAllIncompleteByUser(UserSession.Current.User.Id).ToList(); }
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
            Session.Add("SelectedMainMenu", "To Do List");

            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            var temp = (RadPane)this.Master.FindControl("leftPane");
            temp.Collapsed = true;
            if (!Page.IsPostBack)
            {
                this.grdDocument.MasterTableView.GetColumn("AssignUser").Display = UserSession.Current.User.IsAdmin.GetValueOrDefault();

                if (UserSession.Current.User.IsDC.GetValueOrDefault())
                {
                    //this.grdDocument.AllowMultiRowSelection = true;
                    //this.radMenuIncoming.Visible = true;
                }
                //var ddlStatus = (DropDownList)this.radMenuIncoming.Items[1].FindControl("ddlStatus");
                //if (ddlStatus != null)
                //{
                //    ddlStatus.SelectedIndex = 0;
                //}
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
            else if (e.Argument.Contains("CompleteTask"))
            {
                var currentWorkAssignedUserId = new Guid(e.Argument.Split('_')[1]);
                var currentWorkAssignedUser = this.objAssignedUserService.GetById(currentWorkAssignedUserId);
                if (currentWorkAssignedUser != null)
                {
                    var wfObj = this.wfService.GetById(currentWorkAssignedUser.WorkflowId.GetValueOrDefault());

                    // Update Current work assign
                    currentWorkAssignedUser.IsReject = false;
                    currentWorkAssignedUser.IsComplete = true;
                    currentWorkAssignedUser.ActualDate = DateTime.Now;
                    currentWorkAssignedUser.IsOverDue = currentWorkAssignedUser.PlanCompleteDate.GetValueOrDefault().Date < currentWorkAssignedUser.ActualDate.GetValueOrDefault().Date;
                    this.objAssignedUserService.Update(currentWorkAssignedUser);
                    // ---------------------------------------------------------------------------------------------

                    // Update pending management work assign
                    var managementPendingList = this.objAssignedUserService.GetAllManagementIncompleteByDoc(currentWorkAssignedUser.ObjectID.GetValueOrDefault(), wfObj.ID);
                    foreach (var managementAssign in managementPendingList)
                    {
                        managementAssign.IsComplete = true;
                        managementAssign.ActualDate = DateTime.Now;
                        this.objAssignedUserService.Update(managementAssign);
                    }
                    // ------------------------------------------------------------------------------------------------------------

                    // Process next Step
                    var pendingWorkAssignUserList = this.objAssignedUserService.GetAllIncompleteByDoc(currentWorkAssignedUser.ObjectID.GetValueOrDefault(), wfObj.ID);
                    if (pendingWorkAssignUserList.Count == 0)
                    {
                        var docObj = this.dqreDocumentService.GetById(currentWorkAssignedUser.ObjectID.GetValueOrDefault());
                        var currentWorkAssignedWf = this.objAssignedWorkflowService.GetById(currentWorkAssignedUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                        if (currentWorkAssignedWf != null)
                        {
                            var nextStep = this.wfStepService.GetById(currentWorkAssignedWf.NextWorkflowStepID.GetValueOrDefault());
                            if (nextStep != null)
                            {
                                this.ProcessWorkflow(nextStep, wfObj, docObj);
                            }
                            else
                            {
                                // Check document already in another WF
                                var anotherPending = this.objAssignedUserService.GetAllIncompleteByDoc(currentWorkAssignedUser.ObjectID.GetValueOrDefault());
                                if (anotherPending.Count == 0)
                                {
                                    docObj.IsInWFProcess = false;
                                    docObj.IsWFComplete = true;

                                    this.dqreDocumentService.Update(docObj);
                                }
                                // ---------------------------------------------------------------------------------------------
                            }
                        }
                    }
                    // ---------------------------------------------------------------------------------------------

                    this.grdDocument.Rebind();
                }
            }
            else if (e.Argument.Contains("RejectTask"))
            {
                var currentWorkAssignedUserId = new Guid(e.Argument.Split('_')[1]);
                var currentWorkAssignedUser = this.objAssignedUserService.GetById(currentWorkAssignedUserId);
                if (currentWorkAssignedUser != null)
                {
                    var wfObj = this.wfService.GetById(currentWorkAssignedUser.WorkflowId.GetValueOrDefault());
                    var docObj = this.dqreDocumentService.GetById(currentWorkAssignedUser.ObjectID.GetValueOrDefault());

                    // Update Current work assign
                    currentWorkAssignedUser.IsReject = false;
                    currentWorkAssignedUser.IsComplete = true;
                    currentWorkAssignedUser.ActualDate = DateTime.Now;
                    currentWorkAssignedUser.IsOverDue = currentWorkAssignedUser.PlanCompleteDate.GetValueOrDefault().Date < currentWorkAssignedUser.ActualDate.GetValueOrDefault().Date;
                    this.objAssignedUserService.Update(currentWorkAssignedUser);
                    // ---------------------------------------------------------------------------------------------

                    // Update pending management work assign
                    var managementPendingList = this.objAssignedUserService.GetAllManagementIncompleteByDoc(currentWorkAssignedUser.ObjectID.GetValueOrDefault(), wfObj.ID);
                    foreach (var managementAssign in managementPendingList)
                    {
                        managementAssign.IsComplete = true;
                        managementAssign.ActualDate = DateTime.Now;
                        this.objAssignedUserService.Update(managementAssign);
                    }
                    // ------------------------------------------------------------------------------------------------------------

                    // Complete pending taks
                    var pendingWorkAssignUserList = this.objAssignedUserService.GetAllIncompleteByDoc(currentWorkAssignedUser.ObjectID.GetValueOrDefault(), wfObj.ID);
                    foreach (var pendingWorkAssignUser in pendingWorkAssignUserList)
                    {
                        pendingWorkAssignUser.IsReject = false;
                        pendingWorkAssignUser.IsComplete = true;
                        pendingWorkAssignUser.ActualDate = DateTime.Now;
                        pendingWorkAssignUser.IsOverDue = pendingWorkAssignUser.PlanCompleteDate.GetValueOrDefault().Date < pendingWorkAssignUser.ActualDate.GetValueOrDefault().Date;
                        this.objAssignedUserService.Update(pendingWorkAssignUser);
                    }
                    // ------------------------------------------------------------------------------------------------------------

                    // Process for reject
                    var currentWorkAssignedWf = this.objAssignedWorkflowService.GetById(currentWorkAssignedUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                    if (currentWorkAssignedWf != null)
                    {
                        var prevStep = this.wfStepService.GetById(currentWorkAssignedWf.RejectWorkflowStepID.GetValueOrDefault());
                        if (prevStep != null)
                        {
                            this.ProcessWorkflow(prevStep, wfObj, docObj);
                        }
                    }
                    // ------------------------------------------------------------------------------------------------------------

                    this.grdDocument.Rebind();
                }
            }
            else if (e.Argument == "CompleteMultiTask")
            {
                foreach (GridDataItem selectedItem in this.grdDocument.SelectedItems)
                {
                    var currentWorkAssignedUserId = new Guid(selectedItem.GetDataKeyValue("ID").ToString());
                    var currentWorkAssignedUser = this.objAssignedUserService.GetById(currentWorkAssignedUserId);

                    if (currentWorkAssignedUser != null)
                    {
                        var wfObj = this.wfService.GetById(currentWorkAssignedUser.WorkflowId.GetValueOrDefault());
                        var objId = currentWorkAssignedUser.ObjectID.GetValueOrDefault();
                        var objType = currentWorkAssignedUser.ObjectTypeId.GetValueOrDefault();
                        var currentWorkAssignedWf = this.objAssignedWfService.GetById(currentWorkAssignedUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                        if (currentWorkAssignedWf != null)
                        {
                            var nextStep = this.wfStepService.GetById(currentWorkAssignedWf.NextWorkflowStepID.GetValueOrDefault());
                            if (nextStep == null)
                            {
                                currentWorkAssignedUser.IsComplete = true;
                                currentWorkAssignedUser.IsLeaf = false;
                                currentWorkAssignedUser.ActualDate = DateTime.Now;
                                currentWorkAssignedUser.IsOverDue = currentWorkAssignedUser.PlanCompleteDate.GetValueOrDefault().Date < currentWorkAssignedUser.ActualDate.GetValueOrDefault().Date;
                                this.objAssignedUserService.Update(currentWorkAssignedUser);

                                // Case: Document completed workflow process => Re-check & Update info for incoming trans
                                switch (objType)
                                {
                                    case 1:
                                        var docObj = this.PECC2DocumentService.GetById(objId);
                                        if (docObj != null)
                                        {
                                            docObj.IsInWFProcess = false;
                                            docObj.IsWFComplete = true;
                                            docObj.IsUseIsUseCustomWfFromObj = false;
                                            docObj.IsUseCustomWfFromTrans = false;
                                            this.PECC2DocumentService.Update(docObj);

                                            var docInTrans =
                                                this.PECC2DocumentService.GetAllByIncomingTrans(
                                                    docObj.IncomingTransId.GetValueOrDefault());
                                            if (docInTrans.All(t => t.IsWFComplete.GetValueOrDefault()))
                                            {
                                                var incomingTransObj =
                                                    this.pecc2TransmittalService.GetById(
                                                        docObj.IncomingTransId.GetValueOrDefault());
                                                if (incomingTransObj != null)
                                                {
                                                    incomingTransObj.IsAllDocCompleteWorkflow = true;
                                                    this.pecc2TransmittalService.Update(incomingTransObj);
                                                }
                                            }
                                        }
                                        break;
                                    case 2:
                                        var changeRequestObj = this.changeRequestService.GetById(objId);
                                        if (changeRequestObj != null)
                                        {
                                            changeRequestObj.IsInWFProcess = false;
                                            changeRequestObj.IsWFComplete = true;
                                            changeRequestObj.IsUseIsUseCustomWfFromObj = false;
                                            changeRequestObj.IsUseCustomWfFromTrans = false;
                                            this.changeRequestService.Update(changeRequestObj);

                                            var changeRequestInTrans =
                                                this.changeRequestService.GetAllByIncomingTrans(
                                                    changeRequestObj.IncomingTransId.GetValueOrDefault());
                                            if (changeRequestInTrans.All(t => t.IsWFComplete.GetValueOrDefault()))
                                            {
                                                var incomingTransObj1 =
                                                    this.pecc2TransmittalService.GetById(
                                                        changeRequestObj.IncomingTransId.GetValueOrDefault());
                                                if (incomingTransObj1 != null)
                                                {
                                                    incomingTransObj1.IsAllDocCompleteWorkflow = true;
                                                    this.pecc2TransmittalService.Update(incomingTransObj1);
                                                }
                                            }
                                        }
                                        break;
                                    case 3:
                                        var ncrsiObj = this.ncrSiService.GetById(objId);
                                        if (ncrsiObj != null)
                                        {
                                            ncrsiObj.IsInWFProcess = false;
                                            ncrsiObj.IsWFComplete = true;
                                            ncrsiObj.IsUseIsUseCustomWfFromObj = false;
                                            ncrsiObj.IsUseCustomWfFromTrans = false;
                                            this.ncrSiService.Update(ncrsiObj);
                                        }
                                        break;
                                    case 4:
                                        var incomingTransObj2 = this.pecc2TransmittalService.GetById(objId);
                                        if (incomingTransObj2 != null)
                                        {
                                            incomingTransObj2.IsInWFProcess = false;
                                            incomingTransObj2.IsWFComplete = true;
                                            incomingTransObj2.IsUseCustomWf = false;
                                            this.pecc2TransmittalService.Update(incomingTransObj2);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                this.grdDocument.Rebind();
            }
            else if (e.Argument.Contains("DownloadAll_"))
            {
                try
                {
                    var objId   = new Guid(e.Argument.Split('_')[1]);
                    var pecc2TransObj = this.pecc2TransmittalService.GetById(objId);
                    var filename = "Transmittal_" + pecc2TransObj.TransmittalNo + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".rar";
                    if (pecc2TransObj != null)
                    {
                        var serverTotalDocPackPath = Server.MapPath("~/Exports/DocPack/" + filename);
                        var docPack = ZipPackage.CreateFile(serverTotalDocPackPath);
                        var attachFiles = this.contractorTransmittalDocFileService.GetAllByTrans(pecc2TransObj.ContractorTransId.GetValueOrDefault());

                        foreach (var attachFile in attachFiles)
                        {
                            if (File.Exists(Server.MapPath(attachFile.FilePath)))
                            {
                                docPack.Add(Server.MapPath(attachFile.FilePath));
                            }
                        }

                        var attachFilesCRS = this.contractorTransmittalAttachFileService.GetByTrans(pecc2TransObj.ContractorTransId.GetValueOrDefault());
                        foreach (var attachFile in attachFilesCRS)
                        {
                            if (File.Exists(Server.MapPath(attachFile.FilePath)))
                            {
                                docPack.Add(Server.MapPath(attachFile.FilePath));
                            }
                        }

                        // this.DownloadByWriteByte(serverTotalDocPackPath, "Transmittal_" + pecc2TransObj.TransmittalNo + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".rar", true);

                        this.Download_File(serverTotalDocPackPath);
                    }
                }
                catch (Exception ex)
                { }
            }
        }

        private void Download_File(string FilePath)
        {
            try
            {
                Response.Clear();
                Response.ContentType = ContentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
                Response.WriteFile(FilePath);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
               // Response.End();

            }
            catch (Exception ex)
            {
            }
            finally
            {
                File.SetAttributes(FilePath, FileAttributes.Normal);
                System.IO.File.Delete(FilePath);
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
            var ddlStatusOutgoing = (DropDownList)this.radMenuIncoming.Items[2].FindControl("ddlStatus");
            List<ObjectAssignedUser> ListWork = new List<ObjectAssignedUser>();
            if (ddlStatusOutgoing != null)
            {
                switch (ddlStatusOutgoing.SelectedValue)
                {
                    case "All":
                        ListWork = this.WorkAssigned;
                        break;
                    case "2":
                        ListWork = this.WorkAssigned.Where(t=> t.ObjectTypeId==2).ToList();
                        break;
                    case "3":
                        ListWork = this.WorkAssigned.Where(t => t.ObjectTypeId == 3 && t.ObjectType == "NCR/SI").ToList();
                        break;
                    case "4":
                        ListWork = this.WorkAssigned.Where(t => t.ObjectTypeId == 4).ToList();
                        break;
                    case "5":
                        ListWork = this.WorkAssigned.Where(t => t.ObjectTypeId == 5).ToList();
                        break;
                    case "CS":
                        ListWork = this.WorkAssigned.Where(t => t.ObjectTypeId == 3 && t.ObjectType == "CS").ToList();
                        break;
                    case "6":
                        ListWork = this.WorkAssigned.Where(t => t.ObjectTypeId == 6).ToList();
                        break;
                }
            }
            if (!string.IsNullOrEmpty(Request.QueryString["DocNo"]))
            {
                GridColumn rowStateColumn = grdDocument.MasterTableView.GetColumn("ObjectNumber");
                rowStateColumn.CurrentFilterFunction = GridKnownFunction.Contains;
                rowStateColumn.CurrentFilterValue = Request.QueryString["DocNo"];
                ListWork = ListWork.Where(t => t.ObjectNumber.Contains(Request.QueryString["DocNo"])).ToList();
                var nvc = HttpUtility.ParseQueryString(Request.Url.Query);
                nvc.Remove("DocNo");
                Request.RawUrl.Replace(Request.Url.Query, "");
            }
            this.grdDocument.DataSource = ListWork.OrderByDescending(t => t.ReceivedDate).ToList(); // this.WorkAssigned.OrderByDescending(t=> t.ReceivedDate).ToList();
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
                if (item["ActionTypeId"].Text == "5")
                {
                    item["ActionTypeName"].BackColor = Color.Aqua;
                    item["ActionTypeName"].BorderColor = Color.Aqua;
                }
                else
                {
                    item["ActionTypeName"].BackColor = Color.Coral;
                    item["ActionTypeName"].BorderColor = Color.Coral;
                }

                try
                {    ////// set color for overdue
                    var deadline = new DateTime();
                    Utility.ConvertStringToDateTime(item["PlanCompleteDate"].Text, ref deadline);
                    var today = DateTime.Now.Date;
                    if (deadline.Date < today)
                    {
                        item.BackColor = Color.Red;
                        item.BorderColor = Color.Red;
                    }
                    else if (deadline.Date == today)
                    {
                        item.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB90F");
                        item.BorderColor = System.Drawing.ColorTranslator.FromHtml("#FFB90F");
                    }
                }
                catch { }
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
            e.Node.ImageUrl = @"Images/discipline.png";
        }

        protected void ddlProject_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.ImageUrl = @"Images/project.png";
        }

        private void ProcessWorkflow(WorkflowStep wfStepObj, Workflow wfObj, DQREDocument docObj)
        {
            var wfDetailObj = this.wfDetailService.GetByCurrentStep(wfStepObj.ID);
            if (wfDetailObj != null)
            {
                var assignWorkFlow = new ObjectAssignedWorkflow
                {
                    ID = Guid.NewGuid(),
                    WorkflowID = wfObj.ID,
                    WorkflowName = wfObj.Name,
                    CurrentWorkflowStepID = wfDetailObj.CurrentWorkflowStepID,
                    CurrentWorkflowStepName = wfDetailObj.CurrentWorkflowStepName,
                    NextWorkflowStepID = wfDetailObj.NextWorkflowStepID,
                    NextWorkflowStepName = wfDetailObj.NextWorkflowStepName,
                    RejectWorkflowStepID = wfDetailObj.RejectWorkflowStepID,
                    RejectWorkflowStepName = wfDetailObj.RejectWorkflowStepName,
                    IsComplete = false,
                    IsReject = false,
                    IsLeaf = true,
                    AssignedBy = UserSession.Current.User.Id,
                    CanReject = wfStepObj.CanReject,

                    ObjectID = docObj.ID,
                    ObjectNumber = docObj.DocumentNo,
                    ObjectTitle = docObj.DocumentTitle,
                    ObjectProject = docObj.ProjectCodeName,
                    ObjectType = "Project's Document"
                };

                var assignWorkflowId = this.objAssignedWorkflowService.Insert(assignWorkFlow);
                if (assignWorkflowId != null)
                {
                    // Get actual deadline if workflow step detail use only working day
                    var actualDeadline = DateTime.Now;
                    if (wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault())
                    {
                        for (int i = 1; i <= wfDetailObj.Duration.GetValueOrDefault(); i++)
                        {
                            actualDeadline = this.GetNextWorkingDay(actualDeadline.AddDays(i));
                        }
                    }
                    // -------------------------------------------------------------------------

                    // Get assign User List
                    var wfStepWorkingAssignUser = new List<User>();
                    var infoUserIds = wfDetailObj.InformationOnlyUserIDs.Split(';').ToList();
                    var commentUserIds = wfDetailObj.CommentUserIds.Split(';').ToList();
                    var reviewUserIds = wfDetailObj.ReviewUserIds.Split(';').ToList();
                    var approveUserIds = wfDetailObj.ApproveUserIds.Split(';').ToList();
                    var managementUserIds = wfDetailObj.ManagementUserIds.Split(';').ToList();

                    var matrixList = wfDetailObj.DistributionMatrixIDs.Split(';').Where(t => !string.IsNullOrEmpty(t)).Select(t => this.matrixService.GetById(Convert.ToInt32(t)));
                    foreach (var matrix in matrixList)
                    {
                        var matrixDetailValid = new List<DistributionMatrixDetail>();
                        var matrixDetailList = this.matrixDetailService.GetAllByDM(matrix.ID);

                        // Filter follow matrix rule
                        switch (matrix.TypeId)
                        {
                            // Document have Material/Work Code
                            case 1:
                                matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.DisciplineId == docObj.M_DisciplineId).ToList();
                                break;
                            // Document have Drawing Code (00) Matrix
                            case 2:
                                matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.UnitCodeId == docObj.M_UnitId).ToList();
                                break;
                            // Document have Drawing Code Matrix
                            case 3:
                                matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.DisciplineId == docObj.M_DisciplineId).ToList();
                                break;
                            // AU, CO, PLG, QIR, GTC, PO Matrix
                            case 4:
                                matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId).ToList();
                                break;
                            // EL, ML Matrix
                            case 5:
                                matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.UnitCodeId == docObj.M_UnitId).ToList();
                                break;
                            // PP Matrix
                            case 6:
                                matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.SerialNo == docObj.SerialNo).ToList();
                                break;
                            // Vendor Document Matrix
                            case 7:
                                matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.MaterialCodeId == docObj.M_MaterialCodeId).ToList();
                                break;
                        }

                        infoUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 1).Select(t => t.UserId.ToString()));
                        commentUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 2).Select(t => t.UserId.ToString()));
                        reviewUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 3).Select(t => t.UserId.ToString()));
                        approveUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 4).Select(t => t.UserId.ToString()));
                        managementUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 5).Select(t => t.UserId.ToString()));
                    }

                    foreach (var userId in commentUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                    {
                        var userObj = this.userService.GetByID(userId);
                        if (userObj != null)
                        {
                            userObj.ActionTypeId = 2;
                            userObj.ActionTypeName = "C - Comment";
                            wfStepWorkingAssignUser.Add(userObj);
                        }
                    }

                    foreach (var userId in reviewUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                    {
                        var userObj = this.userService.GetByID(userId);
                        if (userObj != null)
                        {
                            userObj.ActionTypeId = 3;
                            userObj.ActionTypeName = "R - Review";
                            wfStepWorkingAssignUser.Add(userObj);
                        }
                    }

                    foreach (var userId in approveUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                    {
                        var userObj = this.userService.GetByID(userId);
                        if (userObj != null)
                        {
                            userObj.ActionTypeId = 4;
                            userObj.ActionTypeName = "A - Approve";
                            wfStepWorkingAssignUser.Add(userObj);
                        }
                    }

                    foreach (var userId in managementUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                    {
                        var userObj = this.userService.GetByID(userId);
                        if (userObj != null)
                        {
                            userObj.ActionTypeId = 5;
                            userObj.ActionTypeName = "M - Management";
                            wfStepWorkingAssignUser.Add(userObj);
                        }
                    }
                    // ---------------------------------------------------------------------------

                    // Create assign user info
                    foreach (var user in wfStepWorkingAssignUser)
                    {
                        var assignWorkingUser = new ObjectAssignedUser
                        {
                            ID = Guid.NewGuid(),
                            ObjectAssignedWorkflowID = assignWorkflowId,
                            UserID = user.Id,
                            ReceivedDate = DateTime.Now,
                            PlanCompleteDate = wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault() ? actualDeadline : DateTime.Now.AddDays(wfDetailObj.Duration.GetValueOrDefault()),
                            IsOverDue = false,
                            IsComplete = false,
                            IsReject = false,
                            AssignedBy = UserSession.Current.User.Id,
                            WorkflowId = wfObj.ID,
                            WorkflowName = wfObj.Name,
                            CurrentWorkflowStepName = wfStepObj.Name,
                            CurrentWorkflowStepId = wfStepObj.ID,
                            CanReject = wfStepObj.CanReject,
                            IsFinal = wfDetailObj.NextWorkflowStepID == 0,
                            ActionTypeId = user.ActionTypeId,
                            ActionTypeName = user.ActionTypeName,
                            WorkingStatus = string.Empty,

                            ObjectID = docObj.ID,
                            ObjectNumber = docObj.DocumentNo,
                            ObjectTitle = docObj.DocumentTitle,
                            ObjectProject = docObj.ProjectCodeName,
                            Revision = docObj.Revision,
                            IsMainWorkflow = !wfObj.IsInternalWorkflow.GetValueOrDefault()
                        };

                        objAssignedUserService.Insert(assignWorkingUser);
                        if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"])) this.SendNotification(assignWorkingUser, user, infoUserIds);
                    }
                    // ----------------------------------------------------------------------------------

                    // Send notification for Info & Management user

                    ////foreach (var userId in infoUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                    ////{
                    ////    var userObj = this.userService.GetByID(userId);
                    ////    if (userObj != null)
                    ////    {
                    ////        userObj.ActionTypeId = 1;
                    ////        userObj.ActionTypeName = "I - For Information";
                    ////        wfStepWorkingAssignUser.Add(userObj);
                    ////    }
                    ////}

                    ////if (!string.IsNullOrEmpty(wfDetailObj.InformationOnlyUserIDs))
                    ////{
                    ////    var configObj = configService.GetById(1);
                    ////    if (configObj != null && configObj.IsEnableSendEmailNotification.GetValueOrDefault())
                    ////    {
                    ////        infoOnlyUserList = wfDetailObj.InformationOnlyUserIDs.Split('$')
                    ////                                    .Where(t => !string.IsNullOrEmpty(t))
                    ////                                    .Select(t => this.userService.GetByID(Convert.ToInt32(t)))
                    ////                                    .ToList();
                    ////        this.SendNotification(assignWorkingUser, assignUserObj, infoOnlyUserList, managementUserList, configObj);
                    ////    }
                    ////}
                    // ------------------------------------------------------------------------------
                }
            }
        }

        private void SendNotification(ObjectAssignedUser assignWorkingUser, User assignUserObj, List<string> infoUserIds)
        {
            // Implement send mail function
            var smtpClient = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                Host = ConfigurationManager.AppSettings["Host"],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
            };

            var message = new MailMessage();
            message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "EDMS System");
            message.BodyEncoding = new UTF8Encoding();
            message.IsBodyHtml = true;

            message.Subject = "BE ASSIGNMENT: " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.ObjectTitle + ", " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy");

            // Generate email body
            var bodyContent = @"Dear All,<br/><br/>
                                Please be informed that the new update for " + assignWorkingUser.ObjectType + " \"" + assignWorkingUser.ObjectNumber + @""":<br/>
                                <table border='1' cellspacing='0'>
	                                <tr>
		                                <td style=""width: 200px;"">Current Workflow</td>
                                        <td style=""width: 500px;"">" + assignWorkingUser.WorkflowName + @"</td>
	                                </tr>
                                    <tr>
		                                <td>Current Workflow Step</td>
                                        <td>" + assignWorkingUser.CurrentWorkflowStepName + @"</td>
	                                </tr>
                                    <tr>
		                                <td>Title</td>
                                        <td>" + assignWorkingUser.ObjectTitle + @"</td>
	                                </tr>

                                    <tr>
		                                <td>Assign from User</td>
                                        <td>" + assignUserObj.FullNameWithDeptPosition + @"</td>
	                                </tr>
                                    <tr>
		                                <td>Received Date</td>
                                        <td>" + assignWorkingUser.ReceivedDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") + @"</td>
	                                </tr>
                                    <tr>
		                                <td>Updated Date</td>
                                        <td>" + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") + @"</td>
	                                </tr>
                                    
                                </table></br>

                                EDMS WORKFLOW NOTIFICATION </br>
                        [THIS IS SYSTEM GENERATED NOTIFICATION PLEASE DO NOT REPLY]

                                ";
            message.Body = bodyContent;
            if (!string.IsNullOrEmpty(assignUserObj.Email))
            {
                message.To.Add(assignUserObj.Email);
            }

            foreach (var userId in infoUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
            {
                var userObj = this.userService.GetByID(userId);
                if (userObj != null)
                {
                    if (!string.IsNullOrEmpty(userObj.Email)) message.CC.Add(userObj.Email);
                }
            }
            smtpClient.Send(message);
        }

        private bool IsHoliday(DateTime date)
        {
            return Holidays.Contains(date);
        }

        private bool IsWeekEnd(DateTime date)
        {
            return ConfigurationManager.AppSettings["WeekendWork"] == "false" ? date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday : false;
        }

        private DateTime GetNextWorkingDay(DateTime date)
        {
            do
            {
                date = date.AddDays(1);
            }
            while (IsHoliday(date) || IsWeekEnd(date));

            return date;
        }

        protected void grdDocument_OnPreRender(object sender, EventArgs e)
        {
            //for (int i = 0; i < grdDocument.Items.Count; i++)
            //{
            //    if (grdDocument.Items[i]["IsCanCreateOutgoingTrans"].Text == "False" || grdDocument.Items[i]["IsCanCreateOutgoingTrans"].Text == "&nbsp;")
            //    {
            //        grdDocument.Items[i].SelectableMode = GridItemSelectableMode.None;
            //    }
            //    else
            //    {
            //        //var btnOutgoingTrans = (RadButton)this.radMenuIncoming.Items[1].FindControl("btnOutgoingTrans");
            //        //btnOutgoingTrans.Enabled = true;
            //    }
            //}
        }

        private string GetNameUser(List<ObjectAssignedUser> incompleteuser)
        {
            var st = "";
            foreach (var index in incompleteuser)
            {
                st += index.UserFullName + "; ";
            }
            if (st != "") st = st.Substring(0, st.Length - 2);
            return st;
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }
        //private void ProcessWorkflowTem(WorkflowStep wfStepObj, Data.Entities.Workflow wfObj, DQREDocument docObj, ref List<ObjectAssignedUser> ListNextStep, ObjectAssignedUser currentWorkAssignedUser)
        //{
        //    var wfDetailObj = this.wfDetailService.GetByCurrentStep(wfStepObj.ID);
        //    if (wfDetailObj != null)
        //    {

        //            // Get actual deadline if workflow step detail use only working day
        //            var actualDeadline = DateTime.Now;
        //            if (wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault())
        //            {
        //                for (int i = 1; i <= wfDetailObj.Duration.GetValueOrDefault(); i++)
        //                {
        //                    actualDeadline = this.GetNextWorkingDay(actualDeadline.AddDays(i));
        //                }
        //            }
        //            // -------------------------------------------------------------------------

        //            // Get assign User List
        //            var wfStepWorkingAssignUser = new List<User>();
        //            var infoUserIds = wfDetailObj.InformationOnlyUserIDs.Split(';').ToList();
        //            var commentUserIds = wfDetailObj.CommentUserIds.Split(';').ToList();
        //            var reviewUserIds = wfDetailObj.ReviewUserIds.Split(';').ToList();
        //            var approveUserIds = wfDetailObj.ApproveUserIds.Split(';').ToList();
        //            var managementUserIds = wfDetailObj.ManagementUserIds.Split(';').ToList();

        //            var matrixList = wfDetailObj.DistributionMatrixIDs.Split(';').Where(t => !string.IsNullOrEmpty(t)).Select(t => this.matrixService.GetById(Convert.ToInt32(t)));
        //            foreach (var matrix in matrixList)
        //            {
        //                var matrixDetailValid = new List<DistributionMatrixDetail>();
        //                var matrixDetailList = this.matrixDetailService.GetAllByDM(matrix.ID);

        //                // Filter follow matrix rule
        //                switch (matrix.TypeId)
        //                {
        //                    // Document have Material/Work Code
        //                    case 1:
        //                        matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.DisciplineId == docObj.M_DisciplineId).ToList();
        //                        break;
        //                    // Document have Drawing Code (00) Matrix
        //                    case 2:
        //                        matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.UnitCodeId == docObj.M_UnitId).ToList();
        //                        break;
        //                    // Document have Drawing Code Matrix
        //                    case 3:
        //                        matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.DisciplineId == docObj.M_DisciplineId).ToList();
        //                        break;
        //                    // AU, CO, PLG, QIR, GTC, PO Matrix
        //                    case 4:
        //                        matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId).ToList();
        //                        break;
        //                    // EL, ML Matrix
        //                    case 5:
        //                        matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.UnitCodeId == docObj.M_UnitId).ToList();
        //                        break;
        //                    // PP Matrix
        //                    case 6:
        //                        matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.SerialNo == docObj.SerialNo).ToList();
        //                        break;
        //                    // Vendor Document Matrix
        //                    case 7:
        //                        matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.MaterialCodeId == docObj.M_MaterialCodeId).ToList();
        //                        break;
        //                }

        //                infoUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 1).Select(t => t.UserId.ToString()));
        //                commentUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 2).Select(t => t.UserId.ToString()));
        //                reviewUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 3).Select(t => t.UserId.ToString()));
        //                approveUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 4).Select(t => t.UserId.ToString()));
        //                managementUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 5).Select(t => t.UserId.ToString()));
        //            }

        //            foreach (var userId in commentUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
        //            {
        //                var userObj = this.userService.GetByID(userId);
        //                if (userObj != null)
        //                {
        //                    userObj.ActionTypeId = 2;
        //                    userObj.ActionTypeName = "C - Comment";
        //                    wfStepWorkingAssignUser.Add(userObj);
        //                }
        //            }

        //            foreach (var userId in reviewUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
        //            {
        //                var userObj = this.userService.GetByID(userId);
        //                if (userObj != null)
        //                {
        //                    userObj.ActionTypeId = 3;
        //                    userObj.ActionTypeName = "R - Review";
        //                    wfStepWorkingAssignUser.Add(userObj);
        //                }
        //            }

        //            foreach (var userId in approveUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
        //            {
        //                var userObj = this.userService.GetByID(userId);
        //                if (userObj != null)
        //                {
        //                    userObj.ActionTypeId = 4;
        //                    userObj.ActionTypeName = "A - Approve";
        //                    wfStepWorkingAssignUser.Add(userObj);
        //                }
        //            }

        //            foreach (var userId in managementUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
        //            {
        //                var userObj = this.userService.GetByID(userId);
        //                if (userObj != null)
        //                {
        //                    userObj.ActionTypeId = 5;
        //                    userObj.ActionTypeName = "M - Management";
        //                    wfStepWorkingAssignUser.Add(userObj);
        //                }
        //            }
        //            // ---------------------------------------------------------------------------

        //            // Create assign user info
        //            foreach (var user in wfStepWorkingAssignUser)
        //            {
        //                var assignWorkingUser = new ObjectAssignedUser
        //                {
        //                    ID = Guid.NewGuid(),
        //                    ObjectAssignedWorkflowID = currentWorkAssignedUser.ID,
        //                    UserID = user.Id,
        //                    ReceivedDate = currentWorkAssignedUser.ReceivedDate,
        //                    PlanCompleteDate = currentWorkAssignedUser.PlanCompleteDate,
        //                    IsOverDue = !currentWorkAssignedUser.IsComplete.GetValueOrDefault() && currentWorkAssignedUser.PlanCompleteDate.GetValueOrDefault().Date < DateTime.Now.Date,
        //                    IsComplete = false,
        //                    IsReject = false,
        //                    AssignedBy = UserSession.Current.User.Id,
        //                    WorkflowId = currentWorkAssignedUser.WorkflowId,
        //                    WorkflowName = currentWorkAssignedUser.WorkflowName,
        //                    CurrentWorkflowStepName = currentWorkAssignedUser.CurrentWorkflowStepName,
        //                    CurrentWorkflowStepId = currentWorkAssignedUser.CurrentWorkflowStepId,
        //                    CanReject = currentWorkAssignedUser.CanReject,
        //                    IsCanCreateOutgoingTrans = false,
        //                    IsFinal = wfDetailObj.NextWorkflowStepID == 0,
        //                    ActionTypeId = user.ActionTypeId,
        //                    ActionTypeName = user.ActionTypeName,
        //                    WorkingStatus = string.Empty,

        //                    ObjectID = docObj.ID,
        //                    ObjectNumber = docObj.DocumentNo,
        //                    ObjectTitle = docObj.DocumentTitle,
        //                    ObjectProject = docObj.ProjectCodeName,
        //                    Revision = docObj.Revision,
        //                    IsMainWorkflow = currentWorkAssignedUser.IsMainWorkflow,
        //                    IsAddAnotherDisciplineLead = true,
        //                };
        //            ListNextStep.Add(assignWorkingUser);

        //            }
        //    }
        //}
    }
}