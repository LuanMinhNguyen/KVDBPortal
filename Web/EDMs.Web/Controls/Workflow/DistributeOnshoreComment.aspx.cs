// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Scope;
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
    public partial class DistributeOnshoreComment : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly WorkflowService wfService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly ScopeProjectService projectService;
        private readonly ObjectAssignedWorkflowService objAssignedWfService;
        private readonly ObjectAssignedUserService objAssignedUserService;
        
        private readonly WorkflowStepService wfStepService;
        private readonly WorkflowDetailService wfDetailService;
        private readonly DistributionMatrixService dmService;
        private readonly DistributionMatrixDetailService dmDetailService;

        private readonly MaterialRequisitionService mrService;

        private readonly WorkRequestService wrService;

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
        public DistributeOnshoreComment()
        {
            this.userService = new UserService();
            this.wfService = new WorkflowService();
            this.projectService = new ScopeProjectService();

            this.objAssignedUserService = new ObjectAssignedUserService();
            this.objAssignedWfService = new ObjectAssignedWorkflowService();

            this.wfStepService = new WorkflowStepService();
            this.wfDetailService = new WorkflowDetailService();
            this.dmDetailService = new DistributionMatrixDetailService();
            this.dmService = new DistributionMatrixService();

            this.mrService = new MaterialRequisitionService();
            this.wrService = new WorkRequestService();
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
                this.txtDeadline.SelectedDate = DateTime.Now.AddDays(1);
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
            if (!string.IsNullOrEmpty(Request.QueryString["objAssignUserId"]))
            {
                var objAssignUserId = new Guid(Request.QueryString["objAssignUserId"]);
                var objAssignUser = this.objAssignedUserService.GetById(objAssignUserId);
                if (objAssignUser != null)
                {
                    ScopeProject project = null;

                    switch (objAssignUser.ObjectType)
                    {
                        case "Material Requisition":
                            var mrObj = this.mrService.GetById(objAssignUser.ObjectID.GetValueOrDefault());
                            if (mrObj != null)
                            {
                                project = this.projectService.GetById(mrObj.ProjectId.GetValueOrDefault());
                            }
                            break;
                        case "Work Request":
                            var wrObj = this.wrService.GetById(objAssignUser.ObjectID.GetValueOrDefault());
                            if (wrObj != null)
                            {
                                project = this.projectService.GetById(wrObj.ProjectId.GetValueOrDefault());
                            }
                            break;
                    }

                    var newAssignUser = new ObjectAssignedUser()
                    {
                        ID = Guid.NewGuid(),
                        ObjectAssignedWorkflowID = objAssignUser.ObjectAssignedWorkflowID,
                        UserID = Convert.ToInt32(this.ddlUser.SelectedValue),
                        ReceivedDate = DateTime.Now,
                        PlanCompleteDate = this.txtDeadline.SelectedDate,
                        IsOverDue = false,
                        IsComplete = false,
                        IsReject = false,
                        AssignedBy = UserSession.Current.User.Id,
                        WorkflowId = objAssignUser.WorkflowId,
                        WorkflowName = objAssignUser.WorkflowName,
                        CurrentWorkflowStepName = objAssignUser.CurrentWorkflowStepName,
                        CurrentWorkflowStepId = objAssignUser.CurrentWorkflowStepId,
                        CanReject = objAssignUser.CanReject,

                        ObjectID = objAssignUser.ObjectID,
                        ObjectNumber = objAssignUser.ObjectNumber,
                        ObjectTitle = objAssignUser.ObjectTitle,
                        ObjectProject = objAssignUser.ObjectProject,
                        ObjectType = objAssignUser.ObjectType,

                        
                    };

                    if (project != null)
                    {
                        if (project.DCId == Convert.ToInt32(this.ddlUser.SelectedValue))
                        {
                            newAssignUser.IsDistributeOnshore = true;
                            newAssignUser.IsOnshoreComment = false;
                        }
                        else
                        {
                            newAssignUser.IsDistributeOnshore = false;
                            newAssignUser.IsOnshoreComment = true;
                        }
                    }

                    this.objAssignedUserService.Insert(newAssignUser);

                    objAssignUser.IsComplete = true;
                    this.objAssignedUserService.Update(objAssignUser);

                    ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true);
                }
            }
        }

        private void ProcessWorkflow(WorkflowStep wfStepObj, Data.Entities.Workflow wfObj, string objType, object obj)
        {
            var wfDetailObj = this.wfDetailService.GetByCurrentStep(wfStepObj.ID);
            if (wfDetailObj != null)
            {
                var assignWorkFlow = new ObjectAssignedWorkflow
                {
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
                    CanReject = wfStepObj.CanReject
                };

                switch (objType)
                {
                    case "MR":
                        var mrObj = (MaterialRequisition) obj;
                        assignWorkFlow.ObjectID = mrObj.ID;
                        assignWorkFlow.ObjectNumber = mrObj.MRNo;
                        assignWorkFlow.ObjectTitle = mrObj.Justification;
                        assignWorkFlow.ObjectProject = mrObj.ProjectName;
                        assignWorkFlow.ObjectType = "Material Requisition";
                        break;
                    case "WR":
                        var wrObj = (WorkRequest) obj;
                        assignWorkFlow.ObjectID = wrObj.ID;
                        assignWorkFlow.ObjectNumber = wrObj.WRNo;
                        assignWorkFlow.ObjectTitle = wrObj.WRTitle;
                        assignWorkFlow.ObjectProject = wrObj.ProjectName;
                        assignWorkFlow.ObjectType = "Work Request";
                        break;
                }

                var assignWorkflowId = this.objAssignedWfService.Insert(assignWorkFlow);
                if (assignWorkflowId != null)
                {
                    var assignUserIds = this.GetAssignUserList(wfDetailObj);
                    foreach (var assignUserId in assignUserIds)
                    {
                        var assignUser = new ObjectAssignedUser
                        {
                            ObjectAssignedWorkflowID = assignWorkflowId,
                            UserID = assignUserId,
                            ReceivedDate = DateTime.Now,
                            PlanCompleteDate = DateTime.Now.AddDays(wfDetailObj.Duration.GetValueOrDefault()),
                            IsOverDue = false,
                            IsComplete = false,
                            IsReject = false,
                            AssignedBy = UserSession.Current.User.Id,
                            WorkflowId = wfObj.ID,
                            WorkflowName = wfObj.Name,
                            CurrentWorkflowStepName = wfStepObj.Name,
                            CurrentWorkflowStepId = wfStepObj.ID,
                            CanReject = wfStepObj.CanReject
                        };


                        switch (objType)
                        {
                            case "MR":
                                var mrObj = (MaterialRequisition)obj;
                                assignUser.ObjectID = mrObj.ID;
                                assignUser.ObjectNumber = mrObj.MRNo;
                                assignUser.ObjectTitle = mrObj.Justification;
                                assignUser.ObjectProject = mrObj.ProjectName;
                                assignUser.ObjectType = "Material Requisition";
                                break;
                            case "WR":
                                var wrObj = (WorkRequest)obj;
                                assignUser.ObjectID = wrObj.ID;
                                assignUser.ObjectNumber = wrObj.WRNo;
                                assignUser.ObjectTitle = wrObj.WRTitle;
                                assignUser.ObjectProject = wrObj.ProjectName;
                                assignUser.ObjectType = "Work Request";
                                break;
                        }

                        objAssignedUserService.Insert(assignUser);
                    }

                    switch (objType)
                    {
                        case "MR":
                            var mrObj = (MaterialRequisition)obj;
                            mrObj.IsInWFProcess = true;
                            mrObj.IsWFComplete = false;
                            mrObj.CurrentWorkflowName = wfObj.Name;
                            mrObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            this.mrService.Update(mrObj);
                            break;
                        case "WR":
                            var wrObj = (WorkRequest)obj;
                            wrObj.IsInWFProcess = true;
                            wrObj.IsWFComplete = false;
                            wrObj.CurrentWorkflowName = wfObj.Name;
                            wrObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            this.wrService.Update(wrObj);
                            break;
                    }
                }
            }
        }

        private List<int> GetAssignUserList(WorkflowDetail wfDetailObj)
        {
            var assignUserIds = new List<int>();
            assignUserIds.AddRange(wfDetailObj.AssignUserIDs.Split('$')
                                                .Where(t => !string.IsNullOrEmpty(t))
                                                .Select(t => Convert.ToInt32(t)));
            assignUserIds = assignUserIds.Distinct().ToList();

            return assignUserIds;
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
            var userList =
                this.userService.GetAll()
                    .Where(t => t.Id != 1 && t.Role.TypeId == 1)
                    .OrderBy(t => t.FullNameWithDeptPosition);

            this.ddlUser.DataSource = userList;
            this.ddlUser.DataTextField = "FullNameWithDeptPosition";
            this.ddlUser.DataValueField = "Id";
            this.ddlUser.DataBind();
        }

    }
}