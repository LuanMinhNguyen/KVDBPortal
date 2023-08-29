// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;

namespace EDMs.Web.Controls.Workflow
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class CompleteMoveNextMultiTask : Page
    {
        private readonly WorkflowService wfService;
        private readonly UserService userService;
        private readonly ObjectAssignedWorkflowService objAssignedWfService;
        private readonly ObjectAssignedUserService objAssignedUserService;
        private readonly WorkflowStepService wfStepService;
        private readonly WorkflowDetailService wfDetailService;
        private readonly HolidayConfigService holidayConfigService;
        private readonly HashSet<DateTime> Holidays = new HashSet<DateTime>();
        private readonly DistributionMatrixService matrixService;
        private readonly DistributionMatrixDetailService matrixDetailService;
        private readonly PECC2DocumentsService PECC2DocumentService;
        private readonly DocumentCodeServices documnetCodeSErvie;
        private readonly ChangeRequestService changeRequestService;
        private readonly NCR_SIService ncrSiService;
        private readonly PECC2TransmittalService pecc2TransmittalService;

        private readonly CustomizeWorkflowDetailService customizeWorkflowDetailService;

        private int ObjId
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["objId"]);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectInfoEditForm"/> class.
        /// </summary>
        public CompleteMoveNextMultiTask()
        {
            this.userService = new UserService();
            this.wfService = new WorkflowService();
            this.objAssignedUserService = new ObjectAssignedUserService();
            this.objAssignedWfService = new ObjectAssignedWorkflowService();
            this.wfStepService = new WorkflowStepService();
            this.wfDetailService = new WorkflowDetailService();
            this.holidayConfigService = new HolidayConfigService();
            this.documnetCodeSErvie = new DocumentCodeServices();
            this.matrixService = new DistributionMatrixService();
            this.matrixDetailService = new DistributionMatrixDetailService();
            this.PECC2DocumentService = new PECC2DocumentsService();
            this.changeRequestService = new ChangeRequestService();
            this.ncrSiService = new NCR_SIService();
            this.pecc2TransmittalService = new PECC2TransmittalService();
            this.customizeWorkflowDetailService = new CustomizeWorkflowDetailService();
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
            if (!IsPostBack && !string.IsNullOrEmpty(this.Request.QueryString["curentAssignUserIds"]))
            {
                var holidayList = this.holidayConfigService.GetAll();
                foreach (var holidayConfig in holidayList)
                {
                    for (DateTime i = holidayConfig.FromDate.GetValueOrDefault(); i < holidayConfig.ToDate.GetValueOrDefault(); i = i.AddDays(1))
                    {
                        this.Holidays.Add(i);
                    }
                }

                var currentWorkAssignedUserId = new Guid(this.Request.QueryString["curentAssignUserIds"].Split('_')[0]);
                LoadComboData(currentWorkAssignedUserId);
                
                
                var currentWorkAssignedUser = this.objAssignedUserService.GetById(currentWorkAssignedUserId);
                if (currentWorkAssignedUser != null)
                {
                    if (currentWorkAssignedUser.ActionTypeId != 1
                        && currentWorkAssignedUser.ObjectTypeId == 1)
                    {
                        //this.Visible = true;
                        var objId = currentWorkAssignedUser.ObjectID.GetValueOrDefault();
                        var docObj = this.PECC2DocumentService.GetById(objId);
                        if (docObj != null)
                        {
                            this.ddlDocReviewStatus.SelectedValue = docObj.DocReviewStatusId.GetValueOrDefault().ToString();
                        }
                    }
                    else
                    {
                        //this.DevCommentCode.Visible = false;
                    }
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
            if (!string.IsNullOrEmpty(this.Request.QueryString["curentAssignUserIds"]))
            {
                // Complete all select current task
                foreach (var curentAssignUserId in this.Request.QueryString["curentAssignUserIds"].Split('_').Where(t => !string.IsNullOrEmpty(t)))
                {
                    var currentAssignedUser = this.objAssignedUserService.GetById(new Guid(curentAssignUserId));
                    if (currentAssignedUser != null)
                    {
                        // Update Current work assign
                        currentAssignedUser.CommentContent = this.txtMessage.Text.Trim();
                        currentAssignedUser.IsComplete = true;
                        currentAssignedUser.IsLeaf = false;
                        currentAssignedUser.ActualDate = DateTime.Now;
                        currentAssignedUser.IsOverDue =
                        currentAssignedUser.PlanCompleteDate.GetValueOrDefault().Date <
                        currentAssignedUser.ActualDate.GetValueOrDefault().Date;
                        this.objAssignedUserService.Update(currentAssignedUser);
                    }
                }

                var currentWorkAssignedUserId = new Guid(this.Request.QueryString["curentAssignUserIds"].Split('_')[0]); ;
                var currentWorkAssignedUser = this.objAssignedUserService.GetById(currentWorkAssignedUserId);
                if (currentWorkAssignedUser != null)
                {
                    var wfObj = this.wfService.GetById(currentWorkAssignedUser.WorkflowId.GetValueOrDefault());
                    var objId = currentWorkAssignedUser.ObjectID.GetValueOrDefault();
                    var objType = currentWorkAssignedUser.ObjectTypeId.GetValueOrDefault();

                    var pendingTaskList = this.objAssignedUserService.GetAllIncompleteByDoc(currentWorkAssignedUser.ObjectID.GetValueOrDefault());
                    if (!pendingTaskList.Any())
                    {
                        // Reject Case
                        if (this.objAssignedUserService.IsHaveRejectTask(
                                currentWorkAssignedUser.ObjectID.GetValueOrDefault(),
                                currentWorkAssignedUser.CurrentWorkflowStepId.GetValueOrDefault()))
                        {
                            // Process for reject
                            var currentWorkAssignedWf = this.objAssignedWfService.GetById(currentWorkAssignedUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                            if (currentWorkAssignedWf != null)
                            {
                                var prevStep = this.wfStepService.GetById(currentWorkAssignedWf.RejectWorkflowStepID.GetValueOrDefault());
                                if (prevStep != null)
                                {
                                    switch (objType)
                                    {
                                        case 1:
                                            var docObj = this.PECC2DocumentService.GetById(objId);
                                            if (docObj != null)
                                            {
                                                docObj.DocReviewStatusId = Convert.ToInt32(this.ddlDocReviewStatus.SelectedValue);
                                                docObj.DocReviewStatusCode = this.ddlDocReviewStatus.SelectedItem.Text.Split(',')[0];
                                                this.PECC2DocumentService.Update(docObj);

                                                if (!docObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                    && !docObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                                {
                                                    this.ProcessOriginalWorkflow(prevStep, wfObj, docObj, objType, true);
                                                }
                                                else
                                                {
                                                    var customizeWfFrom = docObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                        ? "Trans"
                                                        : "Obj";
                                                    this.ProcessCustomizeWorkflow(prevStep, wfObj, docObj, objType, true, customizeWfFrom);
                                                }
                                            }
                                            break;
                                        case 2:
                                            var changeRequestObj = this.changeRequestService.GetById(objId);
                                            if (changeRequestObj != null)
                                            {
                                                if (!changeRequestObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                    && !changeRequestObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                                {
                                                    this.ProcessOriginalWorkflow(prevStep, wfObj, changeRequestObj, objType,
                                                        true);
                                                }
                                                else
                                                {
                                                    var customizeWfFrom = changeRequestObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                        ? "Trans"
                                                        : "Obj";
                                                    this.ProcessCustomizeWorkflow(prevStep, wfObj, changeRequestObj, objType, true, customizeWfFrom);
                                                }
                                            }
                                            break;
                                        case 3:
                                            var ncrsiObj = this.ncrSiService.GetById(objId);
                                            if (ncrsiObj != null)
                                            {
                                                if (!ncrsiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                    && !ncrsiObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                                {
                                                    this.ProcessOriginalWorkflow(prevStep, wfObj, ncrsiObj, objType, true);
                                                }
                                                else
                                                {
                                                    var customizeWfFrom = ncrsiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                        ? "Trans"
                                                        : "Obj";
                                                    this.ProcessCustomizeWorkflow(prevStep, wfObj, ncrsiObj, objType, true, customizeWfFrom);
                                                }
                                            }
                                            break;
                                    }

                                }
                            }
                        }
                        // Move next case
                        else
                        {
                            var currentWorkAssignedWf = this.objAssignedWfService.GetById(currentWorkAssignedUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                            if (currentWorkAssignedWf != null)
                            {
                                var nextStep = this.wfStepService.GetById(currentWorkAssignedWf.NextWorkflowStepID.GetValueOrDefault());
                                if (nextStep != null)
                                {
                                    switch (objType)
                                    {
                                        case 1:
                                            var docObj = this.PECC2DocumentService.GetById(objId);
                                            if (docObj != null)
                                            {
                                                docObj.DocReviewStatusId = Convert.ToInt32(this.ddlDocReviewStatus.SelectedValue);
                                                docObj.DocReviewStatusCode = this.ddlDocReviewStatus.SelectedItem.Text.Split(',')[0];
                                                this.PECC2DocumentService.Update(docObj);

                                                if (!docObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                    && !docObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                                {
                                                    this.ProcessOriginalWorkflow(nextStep, wfObj, docObj, objType, false);
                                                }
                                                else
                                                {
                                                    var customizeWfFrom = docObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                      ? "Trans"
                                                      : "Obj";
                                                    this.ProcessCustomizeWorkflow(nextStep, wfObj, docObj, objType, true, customizeWfFrom);
                                                }

                                            }
                                            break;
                                        case 2:
                                            var changeRequestObj = this.changeRequestService.GetById(objId);
                                            if (changeRequestObj != null)
                                            {
                                                if (!changeRequestObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                    && !changeRequestObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                                {
                                                    this.ProcessOriginalWorkflow(nextStep, wfObj, changeRequestObj, objType,
                                                        false);
                                                }
                                                else
                                                {
                                                    var customizeWfFrom = changeRequestObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                      ? "Trans"
                                                      : "Obj";
                                                    this.ProcessCustomizeWorkflow(nextStep, wfObj, changeRequestObj, objType, true, customizeWfFrom);
                                                }
                                            }
                                            break;
                                        case 3:
                                            var ncrsiObj = this.ncrSiService.GetById(objId);
                                            if (ncrsiObj != null)
                                            {
                                                if (!ncrsiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                    && !ncrsiObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                                {
                                                    this.ProcessOriginalWorkflow(nextStep, wfObj, ncrsiObj, objType, false);
                                                }
                                                else
                                                {
                                                    var customizeWfFrom = ncrsiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                      ? "Trans"
                                                      : "Obj";
                                                    this.ProcessCustomizeWorkflow(nextStep, wfObj, ncrsiObj, objType, true, customizeWfFrom);
                                                }
                                            }
                                            break;
                                    }

                                }
                                else
                                {
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
                                    }
                                }
                            }
                        }
                    }

                    ClientScript.RegisterStartupScript(Page.GetType(), "mykey", !string.IsNullOrEmpty(this.Request.QueryString["flag"]) ? "Close();" : "CloseAndRebind();", true);
                }
            }
        }

        private void ProcessCustomizeWorkflow(WorkflowStep wfStepObj, Data.Entities.Workflow wfObj, object obj, int objType, bool isReject, string customizeWfFrom)
        {
            var groupId = 0;

            //var infoOnlyUserList = new List<User>();
            CustomizeWorkflowDetail wfDetailObj = null;

            //var wfDetailObj = this.wfDetailService.GetByCurrentStep(wfStepObj.ID);
            if (customizeWfFrom == "Trans")
            {
                switch (objType)
                {
                    case 1:
                        var docObj = (PECC2Documents)obj;
                        if (docObj.IncomingTransId != null)
                        {
                            wfDetailObj =
                                this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromTrans(wfStepObj.ID,
                                    docObj.IncomingTransId.GetValueOrDefault());
                        }
                        break;
                    case 2:
                        var changeRequestObj = (ChangeRequest)obj;
                        wfDetailObj =
                            this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromTrans(wfStepObj.ID,
                                changeRequestObj.IncomingTransId.GetValueOrDefault());
                        break;
                }
            }
            else
            {
                switch (objType)
                {
                    case 1:
                        var docObj = (PECC2Documents)obj;
                        if (docObj.IncomingTransId != null)
                        {
                            wfDetailObj =
                                this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromObj(wfStepObj.ID,
                                    docObj.ID);
                        }
                        break;
                    case 2:
                        var changeRequestObj = (ChangeRequest)obj;
                        wfDetailObj =
                            this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromObj(wfStepObj.ID,
                                changeRequestObj.ID);
                        break;
                    case 3:
                        var ncrsiObj = (NCR_SI)obj;
                        wfDetailObj =
                            this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromObj(wfStepObj.ID,
                                ncrsiObj.ID);
                        break;
                }
            }

            if (wfDetailObj != null)
            {
                var assignWorkFlow = new ObjectAssignedWorkflow
                {
                    ID = Guid.NewGuid(),
                    WorkflowID = wfObj.ID,
                    WorkflowName = wfObj.Name,
                    ObjectWFDwtailId = wfDetailObj.ID,

                    CurrentWorkflowStepID = wfDetailObj.CurrentWorkflowStepID,
                    CurrentWorkflowStepName = wfDetailObj.CurrentWorkflowStepName,
                    NextWorkflowStepID = wfDetailObj.NextWorkflowStepID,
                    NextWorkflowStepName = wfDetailObj.NextWorkflowStepName,
                    RejectWorkflowStepID = wfDetailObj.RejectWorkflowStepID,
                    RejectWorkflowStepName = wfDetailObj.RejectWorkflowStepName,
                    IsComplete = false,
                    IsReject = isReject,
                    IsLeaf = true,
                    AssignedBy = UserSession.Current.User.Id,
                    CanReject = wfStepObj.CanReject,
                };

                switch (objType)
                {
                    case 1:
                        var docObj = (PECC2Documents)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf = this.objAssignedWfService.GetLeafBydoc(docObj.ID);
                        if (objAssignedWFLeaf != null)
                        {
                            objAssignedWFLeaf.IsComplete = true;
                            objAssignedWFLeaf.IsLeaf = false;
                            this.objAssignedWfService.Update(objAssignedWFLeaf);
                        }
                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = docObj.ID;
                        assignWorkFlow.ObjectNumber = docObj.DocNo;
                        assignWorkFlow.ObjectTitle = docObj.DocTitle;
                        assignWorkFlow.ObjectProject = docObj.ProjectName;
                        assignWorkFlow.ObjectType = "Project's Document";
                        groupId = docObj.GroupId.GetValueOrDefault();
                        break;
                    case 2:
                        var changeRequestObj = (ChangeRequest)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf1 = this.objAssignedWfService.GetLeafBydoc(changeRequestObj.ID);
                        if (objAssignedWFLeaf1 != null)
                        {
                            objAssignedWFLeaf1.IsComplete = true;
                            objAssignedWFLeaf1.IsLeaf = false;
                            this.objAssignedWfService.Update(objAssignedWFLeaf1);
                        }

                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = changeRequestObj.ID;
                        assignWorkFlow.ObjectNumber = changeRequestObj.Number;
                        assignWorkFlow.ObjectTitle = changeRequestObj.Description;
                        assignWorkFlow.ObjectProject = changeRequestObj.ProjectCode;
                        assignWorkFlow.ObjectType = "Change Request";
                        groupId = changeRequestObj.GroupId.GetValueOrDefault();
                        break;
                    case 3:
                        var ncrsiObj = (NCR_SI)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf2 = this.objAssignedWfService.GetLeafBydoc(ncrsiObj.ID);
                        if (objAssignedWFLeaf2 != null)
                        {
                            objAssignedWFLeaf2.IsComplete = true;
                            objAssignedWFLeaf2.IsLeaf = false;
                            this.objAssignedWfService.Update(objAssignedWFLeaf2);
                        }
                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = ncrsiObj.ID;
                        assignWorkFlow.ObjectNumber = ncrsiObj.Number;
                        assignWorkFlow.ObjectTitle = ncrsiObj.Subject;
                        assignWorkFlow.ObjectProject = ncrsiObj.ProjectName;
                        assignWorkFlow.ObjectType = "NCR/SI";
                        groupId = ncrsiObj.GroupId.GetValueOrDefault();
                        break;
                }

                var assignWorkflowId = this.objAssignedWfService.Insert(assignWorkFlow);
                if (assignWorkflowId != null)
                {
                    var actualDeadline = DateTime.Now;
                    if (wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault())
                    {
                        for (int i = 1; i <= wfDetailObj.Duration.GetValueOrDefault(); i++)
                        {
                            actualDeadline = this.GetNextWorkingDay(actualDeadline.AddDays(i));
                        }
                    }

                    var currentWFStepDetail = this.wfDetailService.GetByCurrentStep(wfStepObj.ID);
                    // Create assign user info
                    var wfStepWorkingAssignUser = this.GetAssignUserListForCustomizeWorkflow(wfDetailObj, wfStepObj, groupId);
                    var assignWorkingUserInfor = new ObjectAssignedUser();
                    // Create assign user info
                    foreach (var user in wfStepWorkingAssignUser)
                    {
                        var assignWorkingUser = new ObjectAssignedUser
                        {
                            ID = Guid.NewGuid(),
                            ObjectAssignedWorkflowID = assignWorkflowId,
                            UserID = user.Id,
                            UserFullName = user.FullName,
                            ReceivedDate = DateTime.Now,
                            PlanCompleteDate =
                                wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault()
                                    ? actualDeadline
                                    : DateTime.Now.AddDays(wfDetailObj.Duration.GetValueOrDefault()),
                            IsCompleteReject = false,
                            IsOverDue = false,

                            IsComplete = user.ActionTypeId == 1,
                            IsReject = isReject,
                            AssignedBy = UserSession.Current.User.Id,
                            WorkflowId = wfObj.ID,
                            WorkflowName = wfObj.Name,
                            CurrentWorkflowStepName = wfStepObj.Name,
                            CurrentWorkflowStepId = wfStepObj.ID,
                            CanReject = wfStepObj.CanReject,
                            IsCanCreateOutgoingTrans = wfDetailObj.IsCanCreateOutgoingTrans,
                            IsFinal = wfDetailObj.NextWorkflowStepID == 0,
                            ActionTypeId = user.ActionTypeId,
                            ActionTypeName = user.ActionTypeName,
                            WorkingStatus = string.Empty,
                            //Status = (user.ActionTypeId == 1 && wfDetailObj.IsFirst.GetValueOrDefault()) ? "SO" : wfDetailObj.IsFirst.GetValueOrDefault() ? "RS" : "NR",

                            IsMainWorkflow = true,
                            IsReassign = false,
                            IsLeaf = true
                        };

                        switch (objType)
                        {
                            case 1:
                                var docObj = (PECC2Documents)obj;
                                assignWorkingUser.ObjectID = docObj.ID;
                                assignWorkingUser.ObjectNumber = docObj.DocNo;
                                assignWorkingUser.ObjectTitle = docObj.DocTitle;
                                assignWorkingUser.ObjectProject = docObj.ProjectName;
                                assignWorkingUser.ObjectProjectId = docObj.ProjectId;
                                assignWorkingUser.Revision = docObj.Revision;
                                assignWorkingUser.Categoryid = docObj.CategoryId;
                                assignWorkingUser.ObjectType = "Project's Document";
                                assignWorkingUser.ObjectTypeId = 1;
                                break;
                            case 2:
                                var changeRequestObj = (ChangeRequest)obj;
                                assignWorkingUser.ObjectID = changeRequestObj.ID;
                                assignWorkingUser.ObjectNumber = changeRequestObj.Number;
                                assignWorkingUser.ObjectTitle = changeRequestObj.Description;
                                assignWorkingUser.ObjectProject = changeRequestObj.ProjectCode;
                                assignWorkingUser.ObjectProjectId = changeRequestObj.ProjectId;
                                assignWorkingUser.Categoryid = 0;
                                assignWorkingUser.ObjectType = "Change Request";
                                assignWorkingUser.ObjectTypeId = 2;
                                break;
                            case 3:
                                var ncrsiObj = (NCR_SI)obj;
                                assignWorkingUser.ObjectID = ncrsiObj.ID;
                                assignWorkingUser.ObjectNumber = ncrsiObj.Number;
                                assignWorkingUser.ObjectTitle = ncrsiObj.Subject;
                                assignWorkingUser.ObjectProject = ncrsiObj.ProjectName;
                                assignWorkingUser.ObjectProjectId = ncrsiObj.ProjectId;
                                assignWorkingUser.Categoryid = 0;
                                assignWorkingUser.ObjectType = "NCR/SI";
                                assignWorkingUser.ObjectTypeId = 3;
                                break;
                        }

                        objAssignedUserService.Insert(assignWorkingUser);
                        assignWorkingUserInfor = assignWorkingUser;
                        // send notification
                        ////if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) &&
                        ////    wfStepWorkingAssignUser.Count > 0)
                        ////{
                        ////    //this.SendNotificationInfor(notifiObjectAssignWfUser, wfStepWorkingAssignUser);
                        ////}
                        ////if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) && infoUserIds.Count > 0)
                        ////{
                        ////    this.SendNotificationInfor(notifiObjectAssignWfUser, infoUserIds);
                        ////}
                    }

                    // Update current info for Object
                    switch (objType)
                    {
                        case 1:
                            var docObj = (PECC2Documents)obj;
                            docObj.IsInWFProcess = true;
                            docObj.IsWFComplete = false;
                            docObj.CurrentWorkflowName = wfObj.Name;
                            docObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            docObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                            this.PECC2DocumentService.Update(docObj);
                            break;
                        case 2:
                            var changeRequestObj = (ChangeRequest)obj;
                            changeRequestObj.IsInWFProcess = true;
                            changeRequestObj.IsWFComplete = false;
                            changeRequestObj.CurrentWorkflowName = wfObj.Name;
                            changeRequestObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            changeRequestObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                            this.changeRequestService.Update(changeRequestObj);
                            break;
                        case 3:
                            var ncrsiObj = (NCR_SI)obj;
                            ncrsiObj.IsInWFProcess = true;
                            ncrsiObj.IsWFComplete = false;
                            ncrsiObj.CurrentWorkflowName = wfObj.Name;
                            ncrsiObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            ncrsiObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                            this.ncrSiService.Update(ncrsiObj);
                            break;

                    }
                    // ----------------------------------------------------------------------------------------------

                    // Assign to re-assign user of workflow when can't find working user
                    if (wfStepWorkingAssignUser.Count == 0 && wfObj.Re_assignUserId != null)
                    {
                        var assignWorkingUser = new ObjectAssignedUser
                        {
                            ID = Guid.NewGuid(),
                            ObjectAssignedWorkflowID = assignWorkflowId,
                            UserID = wfObj.Re_assignUserId,
                            UserFullName = wfObj.Re_assignUserName,
                            ReceivedDate = DateTime.Now,
                            PlanCompleteDate =
                                wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault()
                                    ? actualDeadline
                                    : DateTime.Now.AddDays(wfDetailObj.Duration.GetValueOrDefault()),
                            IsOverDue = false,
                            IsComplete = false,
                            IsReject = false,
                            AssignedBy = UserSession.Current.User.Id,
                            WorkflowId = wfObj.ID,
                            WorkflowName = wfObj.Name,
                            CurrentWorkflowStepName = wfStepObj.Name,
                            CurrentWorkflowStepId = wfStepObj.ID,
                            CanReject = wfStepObj.CanReject,
                            IsCanCreateOutgoingTrans = wfDetailObj.IsCanCreateOutgoingTrans,
                            IsFinal = wfDetailObj.NextWorkflowStepID == 0,
                            ActionTypeId = 5,
                            ActionTypeName = "Can't find working user. Re-assign to another user.",
                            WorkingStatus = string.Empty,
                            //Status = "RS",
                            IsMainWorkflow = true,
                            IsReassign = true
                        };

                        switch (objType)
                        {
                            case 1:
                                var docObj = (PECC2Documents)obj;
                                assignWorkingUser.ObjectID = docObj.ID;
                                assignWorkingUser.ObjectNumber = docObj.DocNo;
                                assignWorkingUser.ObjectTitle = docObj.DocTitle;
                                assignWorkingUser.ObjectProject = docObj.ProjectName;
                                assignWorkingUser.Revision = docObj.Revision;
                                break;
                            case 2:
                                var changeRequestObj = (ChangeRequest)obj;
                                assignWorkingUser.ObjectID = changeRequestObj.ID;
                                assignWorkingUser.ObjectNumber = changeRequestObj.Number;
                                assignWorkingUser.ObjectTitle = changeRequestObj.Description;
                                assignWorkingUser.ObjectProject = changeRequestObj.ProjectCode;
                                break;
                            case 3:
                                var ncrsiObj = (NCR_SI)obj;
                                assignWorkingUser.ObjectID = ncrsiObj.ID;
                                assignWorkingUser.ObjectNumber = ncrsiObj.Number;
                                assignWorkingUser.ObjectTitle = ncrsiObj.Description;
                                assignWorkingUser.ObjectProject = ncrsiObj.ProjectName;
                                break;
                        }

                        objAssignedUserService.Insert(assignWorkingUser);
                        ////if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]))
                        ////    this.SendNotification(assignWorkingUser,
                        ////        this.userService.GetByID(wfObj.Re_assignUserId.GetValueOrDefault()), infoUserIds);
                    }
                }
            }
        }

        private void ProcessOriginalWorkflow(WorkflowStep wfStepObj, Data.Entities.Workflow wfObj, object obj, int objType, bool isReject)
        {
            var groupId = 0;

            //var infoOnlyUserList = new List<User>();
            var wfDetailObj = this.wfDetailService.GetByCurrentStep(wfStepObj.ID);
            if (wfDetailObj != null)
            {
                var assignWorkFlow = new ObjectAssignedWorkflow
                {
                    ID = Guid.NewGuid(),
                    WorkflowID = wfObj.ID,
                    WorkflowName = wfObj.Name,
                    ObjectWFDwtailId = wfDetailObj.ID,

                    CurrentWorkflowStepID = wfDetailObj.CurrentWorkflowStepID,
                    CurrentWorkflowStepName = wfDetailObj.CurrentWorkflowStepName,
                    NextWorkflowStepID = wfDetailObj.NextWorkflowStepID,
                    NextWorkflowStepName = wfDetailObj.NextWorkflowStepName,
                    RejectWorkflowStepID = wfDetailObj.RejectWorkflowStepID,
                    RejectWorkflowStepName = wfDetailObj.RejectWorkflowStepName,
                    IsComplete = false,
                    IsReject = isReject,
                    IsLeaf = true,
                    AssignedBy = UserSession.Current.User.Id,
                    CanReject = wfStepObj.CanReject,
                };

                switch (objType)
                {
                    case 1:
                        var docObj = (PECC2Documents)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf = this.objAssignedWfService.GetLeafBydoc(docObj.ID);
                        if (objAssignedWFLeaf != null)
                        {
                            objAssignedWFLeaf.IsComplete = true;
                            objAssignedWFLeaf.IsLeaf = false;
                            this.objAssignedWfService.Update(objAssignedWFLeaf);
                        }
                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = docObj.ID;
                        assignWorkFlow.ObjectNumber = docObj.DocNo;
                        assignWorkFlow.ObjectTitle = docObj.DocTitle;
                        assignWorkFlow.ObjectProject = docObj.ProjectName;
                        assignWorkFlow.ObjectType = "Project's Document";
                        groupId = docObj.GroupId.GetValueOrDefault();
                        break;
                    case 2:
                        var changeRequestObj = (ChangeRequest)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf1 = this.objAssignedWfService.GetLeafBydoc(changeRequestObj.ID);
                        if (objAssignedWFLeaf1 != null)
                        {
                            objAssignedWFLeaf1.IsComplete = true;
                            objAssignedWFLeaf1.IsLeaf = false;
                            this.objAssignedWfService.Update(objAssignedWFLeaf1);
                        }

                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = changeRequestObj.ID;
                        assignWorkFlow.ObjectNumber = changeRequestObj.Number;
                        assignWorkFlow.ObjectTitle = changeRequestObj.Description;
                        assignWorkFlow.ObjectProject = changeRequestObj.ProjectCode;
                        assignWorkFlow.ObjectType = "Change Request";
                        groupId = changeRequestObj.GroupId.GetValueOrDefault();
                        break;
                    case 3:
                        var ncrsiObj = (NCR_SI)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf2 = this.objAssignedWfService.GetLeafBydoc(ncrsiObj.ID);
                        if (objAssignedWFLeaf2 != null)
                        {
                            objAssignedWFLeaf2.IsComplete = true;
                            objAssignedWFLeaf2.IsLeaf = false;
                            this.objAssignedWfService.Update(objAssignedWFLeaf2);
                        }
                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = ncrsiObj.ID;
                        assignWorkFlow.ObjectNumber = ncrsiObj.Number;
                        assignWorkFlow.ObjectTitle = ncrsiObj.Subject;
                        assignWorkFlow.ObjectProject = ncrsiObj.ProjectName;
                        assignWorkFlow.ObjectType = "NCR/SI";
                        groupId = ncrsiObj.GroupId.GetValueOrDefault();
                        break;
                }

                var assignWorkflowId = this.objAssignedWfService.Insert(assignWorkFlow);
                if (assignWorkflowId != null)
                {
                    var actualDeadline = DateTime.Now;
                    if (wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault())
                    {
                        for (int i = 1; i <= wfDetailObj.Duration.GetValueOrDefault(); i++)
                        {
                            actualDeadline = this.GetNextWorkingDay(actualDeadline.AddDays(i));
                        }
                    }

                    var currentWFStepDetail = this.wfDetailService.GetByCurrentStep(wfStepObj.ID);
                    // Create assign user info
                    var wfStepWorkingAssignUser = this.GetAssignUserList(wfDetailObj, wfStepObj, groupId);
                    var assignWorkingUserInfor = new ObjectAssignedUser();
                    // Create assign user info
                    foreach (var user in wfStepWorkingAssignUser)
                    {
                        var assignWorkingUser = new ObjectAssignedUser
                        {
                            ID = Guid.NewGuid(),
                            ObjectAssignedWorkflowID = assignWorkflowId,
                            UserID = user.Id,
                            UserFullName = user.FullName,
                            ReceivedDate = DateTime.Now,
                            PlanCompleteDate =
                                wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault()
                                    ? actualDeadline
                                    : DateTime.Now.AddDays(wfDetailObj.Duration.GetValueOrDefault()),
                            IsCompleteReject = false,
                            IsOverDue = false,
                            
                            IsComplete = user.ActionTypeId == 1,
                            IsReject = isReject,
                            AssignedBy = UserSession.Current.User.Id,
                            WorkflowId = wfObj.ID,
                            WorkflowName = wfObj.Name,
                            CurrentWorkflowStepName = wfStepObj.Name,
                            CurrentWorkflowStepId = wfStepObj.ID,
                            CanReject = wfStepObj.CanReject,
                            IsCanCreateOutgoingTrans = wfDetailObj.IsCanCreateOutgoingTrans,
                            IsFinal = wfDetailObj.NextWorkflowStepID == 0,
                            ActionTypeId = user.ActionTypeId,
                            ActionTypeName = user.ActionTypeName,
                            WorkingStatus = string.Empty,
                            //Status = (user.ActionTypeId == 1 && wfDetailObj.IsFirst.GetValueOrDefault()) ? "SO" : wfDetailObj.IsFirst.GetValueOrDefault() ? "RS" : "NR",

                            IsMainWorkflow = true,
                            IsReassign = false,
                            IsLeaf = true
                        };

                        switch (objType)
                        {
                            case 1:
                                var docObj = (PECC2Documents)obj;
                                assignWorkingUser.ObjectID = docObj.ID;
                                assignWorkingUser.ObjectNumber = docObj.DocNo;
                                assignWorkingUser.ObjectTitle = docObj.DocTitle;
                                assignWorkingUser.ObjectProject = docObj.ProjectName;
                                assignWorkingUser.ObjectProjectId = docObj.ProjectId;
                                assignWorkingUser.Revision = docObj.Revision;
                                assignWorkingUser.Categoryid = docObj.CategoryId;
                                assignWorkingUser.ObjectType = "Project's Document";
                                assignWorkingUser.ObjectTypeId = 1;
                                break;
                            case 2:
                                var changeRequestObj = (ChangeRequest)obj;
                                assignWorkingUser.ObjectID = changeRequestObj.ID;
                                assignWorkingUser.ObjectNumber = changeRequestObj.Number;
                                assignWorkingUser.ObjectTitle = changeRequestObj.Description;
                                assignWorkingUser.ObjectProject = changeRequestObj.ProjectCode;
                                assignWorkingUser.ObjectProjectId = changeRequestObj.ProjectId;
                                assignWorkingUser.Categoryid = 0;
                                assignWorkingUser.ObjectType = "Change Request";
                                assignWorkingUser.ObjectTypeId = 2;
                                break;
                            case 3:
                                var ncrsiObj = (NCR_SI)obj;
                                assignWorkingUser.ObjectID = ncrsiObj.ID;
                                assignWorkingUser.ObjectNumber = ncrsiObj.Number;
                                assignWorkingUser.ObjectTitle = ncrsiObj.Subject;
                                assignWorkingUser.ObjectProject = ncrsiObj.ProjectName;
                                assignWorkingUser.ObjectProjectId = ncrsiObj.ProjectId;
                                assignWorkingUser.Categoryid = 0;
                                assignWorkingUser.ObjectType = "NCR/SI";
                                assignWorkingUser.ObjectTypeId = 3;
                                break;
                        }

                        objAssignedUserService.Insert(assignWorkingUser);
                        assignWorkingUserInfor = assignWorkingUser;
                        // send notification
                        ////if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) &&
                        ////    wfStepWorkingAssignUser.Count > 0)
                        ////{
                        ////    //this.SendNotificationInfor(notifiObjectAssignWfUser, wfStepWorkingAssignUser);
                        ////}
                        ////if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) && infoUserIds.Count > 0)
                        ////{
                        ////    this.SendNotificationInfor(notifiObjectAssignWfUser, infoUserIds);
                        ////}
                    }

                    // Update current info for Object
                    switch (objType)
                    {
                        case 1:
                            var docObj = (PECC2Documents)obj;
                            docObj.IsInWFProcess = true;
                            docObj.IsWFComplete = false;
                            docObj.CurrentWorkflowName = wfObj.Name;
                            docObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            docObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                            this.PECC2DocumentService.Update(docObj);
                            break;
                        case 2:
                            var changeRequestObj = (ChangeRequest)obj;
                            changeRequestObj.IsInWFProcess = true;
                            changeRequestObj.IsWFComplete = false;
                            changeRequestObj.CurrentWorkflowName = wfObj.Name;
                            changeRequestObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            changeRequestObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                            this.changeRequestService.Update(changeRequestObj);
                            break;
                        case 3:
                            var ncrsiObj = (NCR_SI)obj;
                            ncrsiObj.IsInWFProcess = true;
                            ncrsiObj.IsWFComplete = false;
                            ncrsiObj.CurrentWorkflowName = wfObj.Name;
                            ncrsiObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            ncrsiObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                            this.ncrSiService.Update(ncrsiObj);
                            break;
                        
                    }
                    // ----------------------------------------------------------------------------------------------

                    // Assign to re-assign user of workflow when can't find working user
                    if (wfStepWorkingAssignUser.Count == 0 && wfObj.Re_assignUserId != null)
                    {
                        var assignWorkingUser = new ObjectAssignedUser
                        {
                            ID = Guid.NewGuid(),
                            ObjectAssignedWorkflowID = assignWorkflowId,
                            UserID = wfObj.Re_assignUserId,
                            UserFullName = wfObj.Re_assignUserName,
                            ReceivedDate = DateTime.Now,
                            PlanCompleteDate =
                                wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault()
                                    ? actualDeadline
                                    : DateTime.Now.AddDays(wfDetailObj.Duration.GetValueOrDefault()),
                            IsOverDue = false,
                            IsComplete = false,
                            IsReject = false,
                            AssignedBy = UserSession.Current.User.Id,
                            WorkflowId = wfObj.ID,
                            WorkflowName = wfObj.Name,
                            CurrentWorkflowStepName = wfStepObj.Name,
                            CurrentWorkflowStepId = wfStepObj.ID,
                            CanReject = wfStepObj.CanReject,
                            IsCanCreateOutgoingTrans = wfDetailObj.IsCanCreateOutgoingTrans,
                            IsFinal = wfDetailObj.NextWorkflowStepID == 0,
                            ActionTypeId = 5,
                            ActionTypeName = "Can't find working user. Re-assign to another user.",
                            WorkingStatus = string.Empty,
                            //Status = "RS",
                            IsMainWorkflow = true,
                            IsReassign = true
                        };

                        switch (objType)
                        {
                            case 1:
                                var docObj = (PECC2Documents)obj;
                                assignWorkingUser.ObjectID = docObj.ID;
                                assignWorkingUser.ObjectNumber = docObj.DocNo;
                                assignWorkingUser.ObjectTitle = docObj.DocTitle;
                                assignWorkingUser.ObjectProject = docObj.ProjectName;
                                assignWorkingUser.Revision = docObj.Revision;
                                break;
                            case 2:
                                var changeRequestObj = (ChangeRequest)obj;
                                assignWorkingUser.ObjectID = changeRequestObj.ID;
                                assignWorkingUser.ObjectNumber = changeRequestObj.Number;
                                assignWorkingUser.ObjectTitle = changeRequestObj.Description;
                                assignWorkingUser.ObjectProject = changeRequestObj.ProjectCode;
                                break;
                            case 3:
                                var ncrsiObj = (NCR_SI)obj;
                                assignWorkingUser.ObjectID = ncrsiObj.ID;
                                assignWorkingUser.ObjectNumber = ncrsiObj.Number;
                                assignWorkingUser.ObjectTitle = ncrsiObj.Description;
                                assignWorkingUser.ObjectProject = ncrsiObj.ProjectName;
                                break;
                        }

                        objAssignedUserService.Insert(assignWorkingUser);
                        ////if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]))
                        ////    this.SendNotification(assignWorkingUser,
                        ////        this.userService.GetByID(wfObj.Re_assignUserId.GetValueOrDefault()), infoUserIds);
                    }
                }
            }
        }

        private List<User> GetAssignUserList(WorkflowDetail wfDetailObj, WorkflowStep wfStepObj, int groupId)
        {
            // Get assign User List
            var wfStepWorkingAssignUser = new List<User>();
            var infoUserIds = wfDetailObj.InformationOnlyUserIDs != null
                ? wfDetailObj.InformationOnlyUserIDs.Split(';').ToList()
                : new List<string>();
            var reviewUserIds = wfDetailObj.ReviewUserIds != null
                ? wfDetailObj.ReviewUserIds.Split(';').ToList()
                : new List<string>();
            var consolidateUserIds = wfDetailObj.ConsolidateUserIds != null
                ? wfDetailObj.ConsolidateUserIds.Split(';').ToList()
                : new List<string>();
            var approveUserIds = wfDetailObj.ApproveUserIds != null
                ? wfDetailObj.ApproveUserIds.Split(';').ToList()
                : new List<string>();

            var matrixList =
                wfDetailObj.DistributionMatrixIDs.Split(';')
                    .Where(t => !string.IsNullOrEmpty(t))
                    .Select(t => this.matrixService.GetById(Convert.ToInt32(t)));
            foreach (var matrix in matrixList)
            {
                var matrixDetailList = this.matrixDetailService.GetAllByDM(matrix.ID).Where(t => t.GroupCodeId == groupId);
                var acceptAction = wfStepObj.ActionApplyCode.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList();
                var matrixDetailValid = matrixDetailList.Where(t => acceptAction.Contains(t.ActionTypeName)).ToList();

                reviewUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 2).Select(t => t.UserId.ToString()));
                consolidateUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 3).Select(t => t.UserId.ToString()));
                approveUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 4).Select(t => t.UserId.ToString()));
                infoUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 1).Select(t => t.UserId.ToString()));
            }

            foreach (
                var userId in
                    consolidateUserIds.Distinct()
                        .Where(t => !string.IsNullOrEmpty(t))
                        .Select(t => Convert.ToInt32(t)))
            {
                var userObj = this.userService.GetByID(userId);
                if (userObj != null)
                {
                    userObj.ActionTypeId = 3;
                    userObj.ActionTypeName = "C - Consolidate";
                    wfStepWorkingAssignUser.Add(userObj);
                }
            }

            foreach (
                var userId in
                    reviewUserIds.Distinct()
                        .Where(t => !string.IsNullOrEmpty(t))
                        .Select(t => Convert.ToInt32(t)))
            {
                var userObj = this.userService.GetByID(userId);
                if (userObj != null)
                {
                    userObj.ActionTypeId = 2;
                    userObj.ActionTypeName = "R - Review";
                    wfStepWorkingAssignUser.Add(userObj);
                }
            }

            foreach (
                var userId in
                    approveUserIds.Distinct()
                        .Where(t => !string.IsNullOrEmpty(t))
                        .Select(t => Convert.ToInt32(t)))
            {
                var userObj = this.userService.GetByID(userId);
                if (userObj != null)
                {
                    userObj.ActionTypeId = 4;
                    userObj.ActionTypeName = "A - Approve";
                    wfStepWorkingAssignUser.Add(userObj);
                }
            }

            foreach (
                var userId in
                    infoUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
            {
                var userObj = this.userService.GetByID(userId);
                if (userObj != null)
                {
                    userObj.ActionTypeId = 1;
                    userObj.ActionTypeName = "I - For Information";
                    wfStepWorkingAssignUser.Add(userObj);
                }
            }

            return wfStepWorkingAssignUser;
        }

        private List<User> GetAssignUserListForCustomizeWorkflow(CustomizeWorkflowDetail wfDetailObj, WorkflowStep wfStepObj, int groupId)
        {
            // Get assign User List
            var wfStepWorkingAssignUser = new List<User>();
            var infoUserIds = wfDetailObj.InformationOnlyUserIDs != null
                ? wfDetailObj.InformationOnlyUserIDs.Split(';').ToList()
                : new List<string>();
            var reviewUserIds = wfDetailObj.ReviewUserIds != null
                ? wfDetailObj.ReviewUserIds.Split(';').ToList()
                : new List<string>();
            var consolidateUserIds = wfDetailObj.ConsolidateUserIds != null
                ? wfDetailObj.ConsolidateUserIds.Split(';').ToList()
                : new List<string>();
            var approveUserIds = wfDetailObj.ApproveUserIds != null
                ? wfDetailObj.ApproveUserIds.Split(';').ToList()
                : new List<string>();

            var matrixList =
                wfDetailObj.DistributionMatrixIDs.Split(';')
                    .Where(t => !string.IsNullOrEmpty(t))
                    .Select(t => this.matrixService.GetById(Convert.ToInt32(t)));
            foreach (var matrix in matrixList)
            {
                var matrixDetailList = this.matrixDetailService.GetAllByDM(matrix.ID).Where(t => t.GroupCodeId == groupId);
                var acceptAction = wfStepObj.ActionApplyCode.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList();
                var matrixDetailValid = matrixDetailList.Where(t => acceptAction.Contains(t.ActionTypeName)).ToList();

                reviewUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 2).Select(t => t.UserId.ToString()));
                consolidateUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 3).Select(t => t.UserId.ToString()));
                approveUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 4).Select(t => t.UserId.ToString()));
                infoUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 1).Select(t => t.UserId.ToString()));
            }

            foreach (
                var userId in
                    consolidateUserIds.Distinct()
                        .Where(t => !string.IsNullOrEmpty(t))
                        .Select(t => Convert.ToInt32(t)))
            {
                var userObj = this.userService.GetByID(userId);
                if (userObj != null)
                {
                    userObj.ActionTypeId = 3;
                    userObj.ActionTypeName = "C - Consolidate";
                    wfStepWorkingAssignUser.Add(userObj);
                }
            }

            foreach (
                var userId in
                    reviewUserIds.Distinct()
                        .Where(t => !string.IsNullOrEmpty(t))
                        .Select(t => Convert.ToInt32(t)))
            {
                var userObj = this.userService.GetByID(userId);
                if (userObj != null)
                {
                    userObj.ActionTypeId = 2;
                    userObj.ActionTypeName = "R - Review";
                    wfStepWorkingAssignUser.Add(userObj);
                }
            }

            foreach (
                var userId in
                    approveUserIds.Distinct()
                        .Where(t => !string.IsNullOrEmpty(t))
                        .Select(t => Convert.ToInt32(t)))
            {
                var userObj = this.userService.GetByID(userId);
                if (userObj != null)
                {
                    userObj.ActionTypeId = 4;
                    userObj.ActionTypeName = "A - Approve";
                    wfStepWorkingAssignUser.Add(userObj);
                }
            }

            foreach (
                var userId in
                    infoUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
            {
                var userObj = this.userService.GetByID(userId);
                if (userObj != null)
                {
                    userObj.ActionTypeId = 1;
                    userObj.ActionTypeName = "I - For Information";
                    wfStepWorkingAssignUser.Add(userObj);
                }
            }

            return wfStepWorkingAssignUser;
        }

        private void SendNotification(ObjectAssignedUser assignWorkingUser,  List<int> infoUserIds)
        {try
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
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "PEDMS System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;

                message.Subject = "ASSIGNMENT: " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.ObjectTitle + ", To" + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy");

                // Generate email body
                var bodyContent = @"<<<< FOR ACTION >>>>
                            <br/>Please action by due date for " + assignWorkingUser.ObjectType + " \"" + assignWorkingUser.ObjectNumber + @""":<br/>
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
		                                <td>Assign From User</td>
                                        <td>" + this.userService.GetByID(assignWorkingUser.AssignedBy.GetValueOrDefault()).FullNameWithDeptPosition + @"</td>
	                                </tr>
                                    <tr>
		                                <td>Received Date</td>
                                        <td>" + assignWorkingUser.ReceivedDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") + @"</td>
	                                </tr>
                                    <tr>
		                                <td>Due Date</td>
                                        <td>" + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") + @"</td>
	                                </tr>
                                    
                                </table><br/>
<br/>
                                    &nbsp;Click on the this link to access the PEDMS system&nbsp;:&nbsp; <a href='" + ConfigurationSettings.AppSettings.Get("WebAddress")
                                           + "/ToDoListPage.aspx?DocNo=" + assignWorkingUser.ObjectNumber + "'>" + ConfigurationSettings.AppSettings.Get("WebAddress") + @"</a>
                                    </br>
                         &nbsp;&nbsp;&nbsp;  EDMS WORKFLOW NOTIFICATION <br/>
                        [THIS IS SYSTEM GENERATED NOTIFICATION PLEASE DO NOT REPLY]

                                ";
                message.Body = bodyContent;
               

                foreach (var userId in infoUserIds.Distinct())
                {
                    var userObj = this.userService.GetByID(userId);
                    if (userObj != null)
                    {
                        if (!string.IsNullOrEmpty(userObj.Email)) message.To.Add(userObj.Email);
                    }
                }
                smtpClient.Send(message);
            }
            catch { }
        }
        private void SendNotificationInfor(ObjectAssignedUser assignWorkingUser, List<string> infoUserIds)
        {
            try
            {
                // Implement send mail function
                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    Host = ConfigurationManager.AppSettings["Host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Host"] + "\\" + ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                };


                var message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "EDMS System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;

                message.Subject = "INFORMATION: " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.ObjectTitle + ", " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy");

                // Generate email body
                var bodyContent = @"<<<< FOR INFORMATION >>>>
                             Please check information by due date for " + assignWorkingUser.ObjectNumber + @""":<br/>
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
		                                <td>Document No</td>
                                        <td>" + assignWorkingUser.ObjectNumber + @"</td>
	                                </tr>
                                    <tr>
		                                <td>Title</td>
                                        <td>" + assignWorkingUser.ObjectTitle + @"</td>
	                                </tr>
                                     <tr>
		                                <td>Revision</td>
                                        <td>" + assignWorkingUser.Revision + @"</td>
	                                </tr>
                                      <tr>
		                                <td>Assign From User</td>
                                        <td>" + this.userService.GetByID(assignWorkingUser.AssignedBy.GetValueOrDefault()).FullNameWithDeptPosition + @"</td>
	                                </tr>
                                </table></br>
                                   <br/>

                                    &nbsp;Click on the this link to access the PEDMS system&nbsp;:&nbsp; <a href='" + ConfigurationSettings.AppSettings.Get("WebAddress")
                                                   + "/ProjectDocumentsList.aspx'>" + ConfigurationSettings.AppSettings.Get("WebAddress") + @"</a>
                                    </br>
                         &nbsp;&nbsp;&nbsp; EDMS WORKFLOW NOTIFICATION </br>
                        [THIS IS SYSTEM GENERATED NOTIFICATION PLEASE DO NOT REPLY]

                                ";
                message.Body = bodyContent;
                foreach (var userId in infoUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                {
                    var userObj = this.userService.GetByID(userId);
                    if (userObj != null)
                    {
                        if (!string.IsNullOrEmpty(userObj.Email)) message.To.Add(userObj.Email);
                    }
                }
                smtpClient.Send(message);
            }
            catch { }
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
        
        protected void btncancel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CancelEdit();", true);
        }

        private void LoadComboData(Guid currentAssignId)
        {
            var currentWorkAssignedUser = this.objAssignedUserService.GetById(currentAssignId);

            if (currentWorkAssignedUser != null)
            {
                this.txtWorkflow.Text = currentWorkAssignedUser.WorkflowName;
                this.txtCurrentStep.Text = currentWorkAssignedUser.CurrentWorkflowStepName;

                var currentWorkAssignedWF = this.objAssignedWfService.GetById(currentWorkAssignedUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                if (currentWorkAssignedWF != null)
                {
                    var nextStep = this.wfStepService.GetById(currentWorkAssignedWF.NextWorkflowStepID.GetValueOrDefault());
                    if (nextStep != null)
                    {
                        Session.Add("NextStep", nextStep);
                        this.txtNextStep.Text = nextStep.Name;

                        var wfDetailObj = this.wfDetailService.GetByCurrentStep(nextStep.ID);
                        if (wfDetailObj != null)
                        {
                            this.txtDuration.Value = wfDetailObj.Duration;
                            this.DurationControl.Visible = wfDetailObj.Duration == null;
                        }
                    }
                }
            }

            //document code
            //var documentcodelist = this.documnetCodeSErvie.GetAllReviewStatus();
            //documentcodelist.Insert(0, new DocumentCode() { ID = 0, Code = string.Empty });
            //this.ddlDocReviewStatus.DataSource = documentcodelist;
            //this.ddlDocReviewStatus.DataTextField = "FullName";
            //this.ddlDocReviewStatus.DataValueField = "ID";
            //this.ddlDocReviewStatus.DataBind();
        }

        private void SendNotification(ObjectAssignedUser assignWorkingUser, User assignUserObj, List<User> infoOnlyUserList, List<User> managementUserList, IntergrateParamConfig configObj)
        {
            // Implement send mail function
            var smtpClient = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = configObj.Sync_UseDefaultCredentials.GetValueOrDefault(),
                EnableSsl = configObj.Sync_EnableSsl.GetValueOrDefault(),
                Host = configObj.Sync_MailServer,
                Port = Convert.ToInt32(configObj.Sync_Port),
                Credentials = new NetworkCredential(configObj.Sync_DefaultEmail, configObj.Sync_EmailPwd),
                Timeout = (60 * 5 * 1000)
            };

            var message = new MailMessage();
            message.From = new MailAddress(configObj.Sync_DefaultEmail, configObj.Sync_EmailName);
            message.BodyEncoding = new UTF8Encoding();
            message.IsBodyHtml = true;

            message.Subject = assignWorkingUser.ObjectType + " \"" + assignWorkingUser.ObjectNumber + "\" being in workflow process";

            // Generate email body
            var bodyContent = @"<<<FOR ACTION>>>
                            REMINDER, you have the following document past due. Please action by due date for " + assignWorkingUser.ObjectType + " \"" + assignWorkingUser.ObjectNumber + @""":<br/>
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
		                                <td>Due Date</td>
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

            foreach (var infoOnlyUser in infoOnlyUserList)
            {
                if (!string.IsNullOrEmpty(infoOnlyUser.Email))
                {
                    message.CC.Add(infoOnlyUser.Email);
                }
            }

            foreach (var managementUser in managementUserList)
            {
                if (!string.IsNullOrEmpty(managementUser.Email))
                {
                    message.CC.Add(managementUser.Email);
                }
            }

            smtpClient.Send(message);
        }

        protected void fileNameValidator_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (this.ddlDocReviewStatus.SelectedItem ==null)
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["actionType"]) && this.Request.QueryString["actionType"] == "4")
                {
                    this.fileNameValidator.ErrorMessage = "Please enter Document Code.";
                    this.divDocCode.Style["margin-bottom"] = "-26px;";
                    args.IsValid = false;
                }
            }
        }
    }
}