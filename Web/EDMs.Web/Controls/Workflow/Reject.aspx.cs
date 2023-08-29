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
using EDMs.Business.Services.Security;
using EDMs.Business.Services.WMS;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;

namespace EDMs.Web.Controls.Workflow
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class Reject : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly WorkflowService wfService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;
        private readonly ObjectAssignedWorkflowService objAssignedWorkflowService;
        private readonly ObjectAssignedUserService objAssignedUserService;
        private readonly WorkflowStepService wfStepService;
        private readonly WorkflowDetailService wfDetailService;
        private readonly HashSet<DateTime> Holidays = new HashSet<DateTime>();
        private readonly DistributionMatrixService matrixService;
        private readonly DistributionMatrixDetailService matrixDetailService;
        private readonly PECC2DocumentsService PECC2DocumentService;
        private readonly ChangeRequestService changeRequestService;
        private readonly NCR_SIService ncrSiService;
        private readonly CustomizeWorkflowDetailService customizeWorkflowDetailService;
        private readonly PECC2TransmittalService pecc2TransmittalService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectInfoEditForm"/> class.
        /// </summary>
        public Reject()
        {
            this.userService = new UserService();
            this.wfService = new WorkflowService();

            this.objAssignedUserService = new ObjectAssignedUserService();
            this.objAssignedWorkflowService = new ObjectAssignedWorkflowService();
            this.wfStepService = new WorkflowStepService();
            this.wfDetailService = new WorkflowDetailService();
            this.matrixService = new DistributionMatrixService();
            this.matrixDetailService = new DistributionMatrixDetailService();
            this.PECC2DocumentService = new PECC2DocumentsService();
            this.changeRequestService = new ChangeRequestService();
            this.ncrSiService = new NCR_SIService();
            this.customizeWorkflowDetailService = new CustomizeWorkflowDetailService();
            this.pecc2TransmittalService = new PECC2TransmittalService();
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
            if (!IsPostBack)
            {
                LoadComboData();
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
            var currentWorkAssignedUserId = new Guid(this.Request.QueryString["currentAssignId"]);
            var currentWorkAssignedUser = this.objAssignedUserService.GetById(currentWorkAssignedUserId);
            if (currentWorkAssignedUser != null)
            {
                var wfObj = this.wfService.GetById(currentWorkAssignedUser.WorkflowId.GetValueOrDefault());
                var objId = currentWorkAssignedUser.ObjectID.GetValueOrDefault();
                var objType = currentWorkAssignedUser.ObjectTypeId.GetValueOrDefault();

                // Update Current work assign
                currentWorkAssignedUser.CommentContent = this.txtMessage.Text.Trim();
                currentWorkAssignedUser.IsCompleteReject = true;
                //currentWorkAssignedUser.IsReject = false;
                currentWorkAssignedUser.IsComplete = true;
                currentWorkAssignedUser.IsLeaf = false;
                currentWorkAssignedUser.Status = "Dis-Agree";
                currentWorkAssignedUser.ActualDate = DateTime.Now;
                currentWorkAssignedUser.IsOverDue = currentWorkAssignedUser.PlanCompleteDate.GetValueOrDefault().Date < currentWorkAssignedUser.ActualDate.GetValueOrDefault().Date;
                this.objAssignedUserService.Update(currentWorkAssignedUser);
                // ---------------------------------------------------------------------------------------------

                // Complete pending taks
                var pendingWorkAssignUserList = this.objAssignedUserService.GetAllIncompleteByDoc(currentWorkAssignedUser.ObjectID.GetValueOrDefault(), wfObj.ID);
                if (pendingWorkAssignUserList.Count == 0)
                {
                    // Process for reject
                    var currentWorkAssignedWf = this.objAssignedWorkflowService.GetById(currentWorkAssignedUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                    if (currentWorkAssignedWf != null)
                    {
                        var prevStep = this.wfStepService.GetById(currentWorkAssignedWf.RejectWorkflowStepID.GetValueOrDefault());
                        if (prevStep != null)
                        {
                            //var ObjWFDetail = this.objectWorkflowDetailService.GetById(currentWorkAssignedWf.ObjectWFDwtailId.GetValueOrDefault());
                            //ObjWFDetail.CanEdit = true;

                            switch (objType)
                            {
                                case 1:
                                    var docObj = this.PECC2DocumentService.GetById(objId);
                                    if (docObj != null)
                                    {
                                        if (!docObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                            && !docObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                        {
                                            this.ProcessOriginalWorkflow(prevStep, wfObj, docObj, objType);
                                        }
                                        else
                                        {
                                            var customizeWfFrom = docObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                    ? "Trans"
                                                    : "Obj";
                                            this.ProcessCustomizeWorkflow(prevStep, wfObj, docObj, objType, customizeWfFrom);
                                        }
                                    }
                                    break;
                                //case 2:
                                //    var changeRequestObj = this.changeRequestService.GetById(objId);
                                //    if (changeRequestObj != null)
                                //    {
                                //        if (!changeRequestObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                //            && !changeRequestObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                //        {
                                //            this.ProcessOriginalWorkflow(prevStep, wfObj, changeRequestObj, objType);
                                //        }
                                //        else
                                //        {
                                //            var customizeWfFrom = changeRequestObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                //                    ? "Trans"
                                //                    : "Obj";
                                //            this.ProcessCustomizeWorkflow(prevStep, wfObj, changeRequestObj, objType, customizeWfFrom);
                                //        }
                                //    }
                                //    break;
                                case 3:
                                    var ncrsiObj = this.ncrSiService.GetById(objId);
                                    if (ncrsiObj != null)
                                    {
                                        if (!ncrsiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                            && !ncrsiObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                        {
                                            this.ProcessOriginalWorkflow(prevStep, wfObj, ncrsiObj, objType);
                                        }
                                        else
                                        {
                                            var customizeWfFrom = ncrsiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                    ? "Trans"
                                                    : "Obj";
                                            this.ProcessCustomizeWorkflow(prevStep, wfObj, ncrsiObj, objType, customizeWfFrom);
                                        }
                                    }
                                    break;
                                case 2:
                                case 4:
                                    var transObj = this.pecc2TransmittalService.GetById(objId);
                                    if (transObj != null)
                                    {
                                        if (!transObj.IsUseCustomWf.GetValueOrDefault())
                                        {
                                            this.ProcessOriginalWorkflow(prevStep, wfObj, transObj, objType);
                                        }
                                        else
                                        {
                                            this.ProcessCustomizeWorkflow(prevStep, wfObj, transObj, objType, "Trans");
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
                // -------------------------------------------------------------------------------------
                ClientScript.RegisterStartupScript(Page.GetType(), "mykey", !string.IsNullOrEmpty(this.Request.QueryString["flag"]) ? "Close();" : "CloseAndRebind();", true);
            }
        }

        private void ProcessOriginalWorkflow(WorkflowStep wfStepObj, Data.Entities.Workflow wfObj, object obj,int objType)
        {
            var groupId = 0;
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
                    IsReject = true,
                    IsLeaf = true,
                    AssignedBy = UserSession.Current.User.Id,
                    CanReject = wfStepObj.CanReject,
                };

                switch (objType)
                {
                    case 1:
                        var docObj = (PECC2Documents)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf = this.objAssignedWorkflowService.GetLeafBydoc(docObj.ID);
                        if (objAssignedWFLeaf != null)
                        {
                            objAssignedWFLeaf.IsComplete = true;
                            objAssignedWFLeaf.IsLeaf = false;
                            this.objAssignedWorkflowService.Update(objAssignedWFLeaf);
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
                        var transObj1 = (PECC2Transmittal)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf4 = this.objAssignedWorkflowService.GetLeafBydoc(transObj1.ID);
                        if (objAssignedWFLeaf4 != null)
                        {
                            objAssignedWFLeaf4.IsComplete = true;
                            objAssignedWFLeaf4.IsLeaf = false;
                            this.objAssignedWorkflowService.Update(objAssignedWFLeaf4);
                        }
                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = transObj1.ID;
                        assignWorkFlow.ObjectNumber = transObj1.TransmittalNo;
                        assignWorkFlow.ObjectTitle = transObj1.Description;
                        assignWorkFlow.ObjectProject = transObj1.ProjectCodeName;
                        assignWorkFlow.ObjectType = "Change Request";
                        groupId = transObj1.GroupId.GetValueOrDefault();
                        break;
                    case 3:
                        var ncrsiObj = (NCR_SI)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf2 = this.objAssignedWorkflowService.GetLeafBydoc(ncrsiObj.ID);
                        if (objAssignedWFLeaf2 != null)
                        {
                            objAssignedWFLeaf2.IsComplete = true;
                            objAssignedWFLeaf2.IsLeaf = false;
                            this.objAssignedWorkflowService.Update(objAssignedWFLeaf2);
                        }
                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = ncrsiObj.ID;
                        assignWorkFlow.ObjectNumber = ncrsiObj.Number;
                        assignWorkFlow.ObjectTitle = ncrsiObj.Subject;
                        assignWorkFlow.ObjectProject = ncrsiObj.ProjectName;
                        assignWorkFlow.ObjectType = "NCR/SI";
                        groupId = ncrsiObj.GroupId.GetValueOrDefault();
                        break;
                    case 4:
                        var transObj = (PECC2Transmittal)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf3 = this.objAssignedWorkflowService.GetLeafBydoc(transObj.ID);
                        if (objAssignedWFLeaf3 != null)
                        {
                            objAssignedWFLeaf3.IsComplete = true;
                            objAssignedWFLeaf3.IsLeaf = false;
                            this.objAssignedWorkflowService.Update(objAssignedWFLeaf3);
                        }
                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = transObj.ID;
                        assignWorkFlow.ObjectNumber = transObj.TransmittalNo;
                        assignWorkFlow.ObjectTitle = transObj.Description;
                        assignWorkFlow.ObjectProject = transObj.ProjectCodeName;
                        assignWorkFlow.ObjectType = "Document Transmittal";
                        groupId = transObj.GroupId.GetValueOrDefault();
                        break;
                }

                var assignWorkflowId = this.objAssignedWorkflowService.Insert(assignWorkFlow);
                if (assignWorkflowId != null)
                {
                    // Get actual deadline if workflow step detail use only working day
                    var actualDeadline = DateTime.Now;
                    if (wfDetailObj.Duration.GetValueOrDefault() < 1)
                    {
                        actualDeadline = actualDeadline.AddDays(wfDetailObj.Duration.GetValueOrDefault());
                    }
                    else
                    {
                        if (wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault())
                        {
                            for (int i = 1; i <= wfDetailObj.Duration.GetValueOrDefault(); i++)
                            {
                                actualDeadline = this.GetNextWorkingDay(actualDeadline);
                            }
                        }
                    }
                    // -------------------------------------------------------------------------

                    // Get assign User List
                    var wfStepWorkingAssignUser = this.GetAssignUserList(wfDetailObj, wfStepObj, groupId);
                    // ---------------------------------------------------------------------------

                    var assignWorkingUserInfor = new ObjectAssignedUser();
                    // Create assign user info
                    foreach (var user in wfStepWorkingAssignUser.Where(t=> t.ActionTypeId!= 1))
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
                            IsReject = true,
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
                                var transObj1 = (PECC2Transmittal)obj;
                                assignWorkingUser.ObjectID = transObj1.ID;
                                assignWorkingUser.ObjectNumber = transObj1.TransmittalNo;
                                assignWorkingUser.ObjectTitle = transObj1.Description;
                                assignWorkingUser.ObjectProject = transObj1.ProjectCodeName;
                                assignWorkingUser.ObjectProjectId = transObj1.ProjectCodeId;
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
                            case 4:
                                var transObj = (PECC2Transmittal)obj;
                                assignWorkingUser.ObjectID = transObj.ID;
                                assignWorkingUser.ObjectNumber = transObj.TransmittalNo;
                                assignWorkingUser.ObjectTitle = transObj.Description;
                                assignWorkingUser.ObjectProject = transObj.ProjectCodeName;
                                assignWorkingUser.ObjectProjectId = transObj.ProjectCodeId;
                                assignWorkingUser.Categoryid = 0;
                                assignWorkingUser.ObjectType = "Document Transmittal";
                                assignWorkingUser.ObjectTypeId = 4;
                                break;
                        }

                        objAssignedUserService.Insert(assignWorkingUser);
                        assignWorkingUserInfor = assignWorkingUser;
                        // send notification
                        
                        ////if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) && infoUserIds.Count > 0)
                        ////{
                        ////    this.SendNotificationInfor(notifiObjectAssignWfUser, infoUserIds);
                        ////}
                    }
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) &&
                            wfStepWorkingAssignUser.Count > 0)
                        {
                            this.SendNotification(assignWorkingUserInfor, wfStepWorkingAssignUser, this.txtMessage.Text.Trim());
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
                        //case 2:
                        //    var changeRequestObj = (ChangeRequest)obj;
                        //    changeRequestObj.IsInWFProcess = true;
                        //    changeRequestObj.IsWFComplete = false;
                        //    changeRequestObj.CurrentWorkflowName = wfObj.Name;
                        //    changeRequestObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                        //    changeRequestObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                        //    this.changeRequestService.Update(changeRequestObj);
                        //    break;
                        case 3:
                            var ncrsiObj = (NCR_SI)obj;
                            ncrsiObj.IsInWFProcess = true;
                            ncrsiObj.IsWFComplete = false;
                            ncrsiObj.CurrentWorkflowName = wfObj.Name;
                            ncrsiObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            ncrsiObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                            this.ncrSiService.Update(ncrsiObj);
                            break;
                        case 2:
                        case 4:
                            var transObj = (PECC2Transmittal)obj;
                            transObj.IsInWFProcess = true;
                            transObj.IsWFComplete = false;
                            transObj.CurrentWorkflowName = wfObj.Name;
                            transObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            transObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                            this.pecc2TransmittalService.Update(transObj);
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
                            ReceivedDate = DateTime.Now,
                            PlanCompleteDate = wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault() ? actualDeadline : DateTime.Now.AddDays(wfDetailObj.Duration.GetValueOrDefault()),
                            IsOverDue = false,
                            IsComplete = false,
                            IsReject = true,
                            AssignedBy = UserSession.Current.User.Id,
                            WorkflowId = wfObj.ID,
                            WorkflowName = wfObj.Name,
                            CurrentWorkflowStepName = wfStepObj.Name,
                            CurrentWorkflowStepId = wfStepObj.ID,
                            CanReject = wfStepObj.CanReject,
                            IsCanCreateOutgoingTrans = wfDetailObj.IsCanCreateOutgoingTrans,
                            IsFinal = wfDetailObj.NextWorkflowStepID == 0,
                            ActionTypeId = 2,
                            ActionTypeName = "Can't find working user in this step. Please Re-assign to another user.",
                            WorkingStatus = string.Empty,

                            IsMainWorkflow = !wfObj.IsInternalWorkflow.GetValueOrDefault(),
                            IsReassign = true,
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
                                assignWorkingUser.Revision = docObj.Revision;
                                break;
                            //case 2:
                            //    var changeRequestObj = (ChangeRequest)obj;
                            //    assignWorkingUser.ObjectID = changeRequestObj.ID;
                            //    assignWorkingUser.ObjectNumber = changeRequestObj.Number;
                            //    assignWorkingUser.ObjectTitle = changeRequestObj.Description;
                            //    assignWorkingUser.ObjectProject = changeRequestObj.ProjectCode;
                            //    break;
                            case 3:
                                var ncrsiObj = (NCR_SI)obj;
                                assignWorkingUser.ObjectID = ncrsiObj.ID;
                                assignWorkingUser.ObjectNumber = ncrsiObj.Number;
                                assignWorkingUser.ObjectTitle = ncrsiObj.Description;
                                assignWorkingUser.ObjectProject = ncrsiObj.ProjectName;
                                break;
                            case 2:
                            case 4:
                                var transObj = (PECC2Transmittal)obj;
                                assignWorkingUser.ObjectID = transObj.ID;
                                assignWorkingUser.ObjectNumber = transObj.TransmittalNo;
                                assignWorkingUser.ObjectTitle = transObj.Description;
                                assignWorkingUser.ObjectProject = transObj.ProjectCodeName;
                                break;
                        }

                        objAssignedUserService.Insert(assignWorkingUser);
                        //if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]))
                        //{
                        //    this.SendNotification(assignWorkingUser, this.userService.GetByID(wfObj.Re_assignUserId.GetValueOrDefault()), infoUserIds);
                        //}
                    }
                    // ----------------------------------------------------------------------------------
                }
            }
        }

        private void ProcessCustomizeWorkflow(WorkflowStep wfStepObj, Data.Entities.Workflow wfObj, object obj, int objType, string customizeWfFrom)
        {
            var groupId = 0;
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
                    //case 2:
                    //    var changeRequestObj = (ChangeRequest)obj;
                    //    wfDetailObj =
                    //        this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromTrans(wfStepObj.ID,
                    //            changeRequestObj.IncomingTransId.GetValueOrDefault());
                    //    break;
                    case 2:
                    case 4:
                        var transObj = (PECC2Transmittal)obj;
                        wfDetailObj = this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromTrans(wfStepObj.ID,
                                transObj.ID);
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
                    //case 2:
                    //    var changeRequestObj = (ChangeRequest)obj;
                    //    wfDetailObj =
                    //        this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromObj(wfStepObj.ID,
                    //            changeRequestObj.ID);
                    //    break;
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
                    CurrentWorkflowStepID = wfDetailObj.CurrentWorkflowStepID,
                    CurrentWorkflowStepName = wfDetailObj.CurrentWorkflowStepName,
                    NextWorkflowStepID = wfDetailObj.NextWorkflowStepID,
                    NextWorkflowStepName = wfDetailObj.NextWorkflowStepName,
                    RejectWorkflowStepID = wfDetailObj.RejectWorkflowStepID,
                    RejectWorkflowStepName = wfDetailObj.RejectWorkflowStepName,
                    IsComplete = false,
                    IsReject = true,
                    IsLeaf = true,
                    AssignedBy = UserSession.Current.User.Id,
                    CanReject = wfStepObj.CanReject,
                };

                switch (objType)
                {
                    case 1:
                        var docObj = (PECC2Documents)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf = this.objAssignedWorkflowService.GetLeafBydoc(docObj.ID);
                        if (objAssignedWFLeaf != null)
                        {
                            objAssignedWFLeaf.IsComplete = true;
                            objAssignedWFLeaf.IsLeaf = false;
                            this.objAssignedWorkflowService.Update(objAssignedWFLeaf);
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
                        var transObj1 = (PECC2Transmittal)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf4 = this.objAssignedWorkflowService.GetLeafBydoc(transObj1.ID);
                        if (objAssignedWFLeaf4 != null)
                        {
                            objAssignedWFLeaf4.IsComplete = true;
                            objAssignedWFLeaf4.IsLeaf = false;
                            this.objAssignedWorkflowService.Update(objAssignedWFLeaf4);
                        }
                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = transObj1.ID;
                        assignWorkFlow.ObjectNumber = transObj1.TransmittalNo;
                        assignWorkFlow.ObjectTitle = transObj1.Description;
                        assignWorkFlow.ObjectProject = transObj1.ProjectCodeName;
                        assignWorkFlow.ObjectType = "Change Request";
                        groupId = transObj1.GroupId.GetValueOrDefault();
                        break;
                    case 3:
                        var ncrsiObj = (NCR_SI)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf2 = this.objAssignedWorkflowService.GetLeafBydoc(ncrsiObj.ID);
                        if (objAssignedWFLeaf2 != null)
                        {
                            objAssignedWFLeaf2.IsComplete = true;
                            objAssignedWFLeaf2.IsLeaf = false;
                            this.objAssignedWorkflowService.Update(objAssignedWFLeaf2);
                        }
                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = ncrsiObj.ID;
                        assignWorkFlow.ObjectNumber = ncrsiObj.Number;
                        assignWorkFlow.ObjectTitle = ncrsiObj.Subject;
                        assignWorkFlow.ObjectProject = ncrsiObj.ProjectName;
                        assignWorkFlow.ObjectType = "NCR/SI";
                        groupId = ncrsiObj.GroupId.GetValueOrDefault();
                        break;
                    case 4:
                        var transObj = (PECC2Transmittal)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf3 = this.objAssignedWorkflowService.GetLeafBydoc(transObj.ID);
                        if (objAssignedWFLeaf3 != null)
                        {
                            objAssignedWFLeaf3.IsComplete = true;
                            objAssignedWFLeaf3.IsLeaf = false;
                            this.objAssignedWorkflowService.Update(objAssignedWFLeaf3);
                        }
                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = transObj.ID;
                        assignWorkFlow.ObjectNumber = transObj.TransmittalNo;
                        assignWorkFlow.ObjectTitle = transObj.Description;
                        assignWorkFlow.ObjectProject = transObj.ProjectCodeName;
                        assignWorkFlow.ObjectType = "Document Transmittal";
                        groupId = transObj.GroupId.GetValueOrDefault();
                        break;
                }

                var assignWorkflowId = this.objAssignedWorkflowService.Insert(assignWorkFlow);
                if (assignWorkflowId != null)
                {
                    // Get actual deadline if workflow step detail use only working day
                    var actualDeadline = DateTime.Now;
                    if (wfDetailObj.Duration.GetValueOrDefault() < 1)
                    {
                        actualDeadline = actualDeadline.AddDays(wfDetailObj.Duration.GetValueOrDefault());
                    }
                    else
                    {
                        if (wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault())
                        {
                            for (int i = 1; i <= wfDetailObj.Duration.GetValueOrDefault(); i++)
                            {
                                actualDeadline = this.GetNextWorkingDay(actualDeadline);
                            }
                        }
                    }
                    // -------------------------------------------------------------------------

                    // Get assign User List
                    var wfStepWorkingAssignUser = this.GetAssignUserListForCustomizeWorkflow(wfDetailObj, wfStepObj, groupId);
                    // ---------------------------------------------------------------------------

                    var assignWorkingUserInfor = new ObjectAssignedUser();
                    // Create assign user info
                    foreach (var user in wfStepWorkingAssignUser.Where(t => t.ActionTypeId != 1))
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
                            IsReject = true,
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
                                var transObj1 = (PECC2Transmittal)obj;
                                assignWorkingUser.ObjectID = transObj1.ID;
                                assignWorkingUser.ObjectNumber = transObj1.TransmittalNo;
                                assignWorkingUser.ObjectTitle = transObj1.Description;
                                assignWorkingUser.ObjectProject = transObj1.ProjectCodeName;
                                assignWorkingUser.ObjectProjectId = transObj1.ProjectCodeId;
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
                            case 4:
                                var transObj = (PECC2Transmittal)obj;
                                assignWorkingUser.ObjectID = transObj.ID;
                                assignWorkingUser.ObjectNumber = transObj.TransmittalNo;
                                assignWorkingUser.ObjectTitle = transObj.Description;
                                assignWorkingUser.ObjectProject = transObj.ProjectCodeName;
                                assignWorkingUser.ObjectProjectId = transObj.ProjectCodeId;
                                assignWorkingUser.Categoryid = 0;
                                assignWorkingUser.ObjectType = "Document Transmittal";
                                assignWorkingUser.ObjectTypeId = 4;
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
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) &&
                           wfStepWorkingAssignUser.Count > 0)
                    {
                        this.SendNotification(assignWorkingUserInfor, wfStepWorkingAssignUser, this.txtMessage.Text.Trim());
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
                        //case 2:
                        //    var changeRequestObj = (ChangeRequest)obj;
                        //    changeRequestObj.IsInWFProcess = true;
                        //    changeRequestObj.IsWFComplete = false;
                        //    changeRequestObj.CurrentWorkflowName = wfObj.Name;
                        //    changeRequestObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                        //    changeRequestObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                        //    this.changeRequestService.Update(changeRequestObj);
                        //    break;
                        case 3:
                            var ncrsiObj = (NCR_SI)obj;
                            ncrsiObj.IsInWFProcess = true;
                            ncrsiObj.IsWFComplete = false;
                            ncrsiObj.CurrentWorkflowName = wfObj.Name;
                            ncrsiObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            ncrsiObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                            this.ncrSiService.Update(ncrsiObj);
                            break;
                        case 2:
                        case 4:
                            var transObj = (PECC2Transmittal)obj;
                            transObj.IsInWFProcess = true;
                            transObj.IsWFComplete = false;
                            transObj.CurrentWorkflowName = wfObj.Name;
                            transObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            transObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                            this.pecc2TransmittalService.Update(transObj);
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
                            ReceivedDate = DateTime.Now,
                            PlanCompleteDate = wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault() ? actualDeadline : DateTime.Now.AddDays(wfDetailObj.Duration.GetValueOrDefault()),
                            IsOverDue = false,
                            IsComplete = false,
                            IsReject = true,
                            AssignedBy = UserSession.Current.User.Id,
                            WorkflowId = wfObj.ID,
                            WorkflowName = wfObj.Name,
                            CurrentWorkflowStepName = wfStepObj.Name,
                            CurrentWorkflowStepId = wfStepObj.ID,
                            CanReject = wfStepObj.CanReject,
                            IsCanCreateOutgoingTrans = wfDetailObj.IsCanCreateOutgoingTrans,
                            IsFinal = wfDetailObj.NextWorkflowStepID == 0,
                            ActionTypeId = 2,
                            ActionTypeName = "Can't find working user in this step. Please Re-assign to another user.",
                            WorkingStatus = string.Empty,

                            IsMainWorkflow = !wfObj.IsInternalWorkflow.GetValueOrDefault(),
                            IsReassign = true,
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
                                assignWorkingUser.Revision = docObj.Revision;
                                break;
                            //case 2:
                            //    var changeRequestObj = (ChangeRequest)obj;
                            //    assignWorkingUser.ObjectID = changeRequestObj.ID;
                            //    assignWorkingUser.ObjectNumber = changeRequestObj.Number;
                            //    assignWorkingUser.ObjectTitle = changeRequestObj.Description;
                            //    assignWorkingUser.ObjectProject = changeRequestObj.ProjectCode;
                            //    break;
                            case 3:
                                var ncrsiObj = (NCR_SI)obj;
                                assignWorkingUser.ObjectID = ncrsiObj.ID;
                                assignWorkingUser.ObjectNumber = ncrsiObj.Number;
                                assignWorkingUser.ObjectTitle = ncrsiObj.Description;
                                assignWorkingUser.ObjectProject = ncrsiObj.ProjectName;
                                break;
                            case 2:
                            case 4:
                                var transObj = (PECC2Transmittal)obj;
                                assignWorkingUser.ObjectID = transObj.ID;
                                assignWorkingUser.ObjectNumber = transObj.TransmittalNo;
                                assignWorkingUser.ObjectTitle = transObj.Description;
                                assignWorkingUser.ObjectProject = transObj.ProjectCodeName;
                                break;
                        }

                        objAssignedUserService.Insert(assignWorkingUser);
                        //if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]))
                        //{
                        //    this.SendNotification(assignWorkingUser, this.userService.GetByID(wfObj.Re_assignUserId.GetValueOrDefault()), infoUserIds);
                        //}
                    }
                    // ----------------------------------------------------------------------------------
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
                         this.matrixService.GetAllByList(wfDetailObj.DistributionMatrixIDs.Split(';')
                              .Where(t => !string.IsNullOrEmpty(t))
                              .Select(t => Convert.ToInt32(t)).ToList());
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

            

            return wfStepWorkingAssignUser;
        }

        private void SendNotification(ObjectAssignedUser assignWorkingUser, List<User> infoUserIds, string reason)
        {try {
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
            message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
            message.BodyEncoding = new UTF8Encoding();
            message.IsBodyHtml = true;

            message.Subject = "[DMDC " + assignWorkingUser.ObjectProject + "] REJECT TASK.";

            // Generate email body
            var bodyContent = @"<<<<< FOR REJECT >>>>>
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
		                                <td>Reject From User</td>
                                        <td>" +UserSession.Current.User.UserNameWithFullNamePosition + @"</td>
	                                </tr>
                                    <tr>
		                                <td>Received Date</td>
                                        <td>" + assignWorkingUser.ReceivedDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") + @"</td>
	                                </tr>
                                    <tr>
		                                <td>Due Date</td>
                                        <td>" + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") + @"</td>
	                                </tr>
                                     <tr>
		                                <td>Reason</td>
                                        <td>" + reason + @"</td>
	                                </tr>
                                </table><br/>
                                    <br/>
                                    &nbsp;Click :&nbsp; <a href='" + ConfigurationSettings.AppSettings.Get("WebAddress")
                                           + @"/ToDoListPage.aspx'> to access the DMSC system&nbsp;</a>
                                    </br>
                          <br/>
                        [THIS IS SYSTEM GENERATED NOTIFICATION PLEASE DO NOT REPLY]
                                ";
            message.Body = bodyContent;
            //if (!string.IsNullOrEmpty(assignUserObj.Email))
            //{
            //    message.To.Add(assignUserObj.Email);
            //}

            foreach (var userObj in infoUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t.Email)))
            {
               
                    if (!string.IsNullOrEmpty(userObj.Email)) message.To.Add(userObj.Email);
            
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

        /// <summary>
        /// The btncancel_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btncancel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CancelEdit();", true);
        }

        private void LoadComboData()
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["currentAssignId"]))
            {
                var currentWorkAssignedUserId = new Guid(this.Request.QueryString["currentAssignId"]);
                var currentWorkAssignedUser = this.objAssignedUserService.GetById(currentWorkAssignedUserId);

                if (currentWorkAssignedUser != null)
                {
                    this.txtWorkflow.Text = currentWorkAssignedUser.WorkflowName;
                    this.txtCurrentStep.Text = currentWorkAssignedUser.CurrentWorkflowStepName;

                    var currentWorkAssignedWF = this.objAssignedWorkflowService.GetById(currentWorkAssignedUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                    if (currentWorkAssignedWF != null)
                    {
                        var previousStep = this.wfStepService.GetById(currentWorkAssignedWF.RejectWorkflowStepID.GetValueOrDefault());
                        if (previousStep != null)
                        {
                            Session.Add("PreviousStep", previousStep);
                            this.txtPreviousStep.Text = previousStep.Name;
                        }
                    }
                }
            }
        }
    }
}