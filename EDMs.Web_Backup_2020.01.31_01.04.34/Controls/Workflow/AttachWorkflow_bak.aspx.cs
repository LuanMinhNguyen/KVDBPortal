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
    public partial class AttachWorkflow_bak : Page
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

        private Guid ObjId
        {
            get
            {
                return new Guid(Request.QueryString["objId"]);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectInfoEditForm"/> class.
        /// </summary>
        public AttachWorkflow_bak()
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
            var wfObj = this.wfService.GetById(Convert.ToInt32(ddlWorkflow.SelectedValue));
            if (wfObj != null)
            {
                var objType = Request.QueryString["objType"];
                switch (objType)
                {
                    case "MR":
                        var mrObj = this.mrService.GetById(this.ObjId);
                        if (mrObj != null)
                        {
                            var wfFirstStepObj = this.wfStepService.GetFirstStep(wfObj.ID);
                            if (wfFirstStepObj != null)
                            {
                                this.ProcessWorkflow(wfFirstStepObj, wfObj, objType, mrObj);
                            }
                        }
                        break;
                    case "WR":
                        var wrObj = this.wrService.GetById(this.ObjId);
                        if (wrObj != null)
                        {
                            var wfFirstStepObj = this.wfStepService.GetFirstStep(wfObj.ID);
                            if (wfFirstStepObj != null)
                            {
                                this.ProcessWorkflow(wfFirstStepObj, wfObj, objType, wrObj);
                            }
                        }
                        break;
                }
            }

            ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true);
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
            var wfList = wfService.GetAll();
            ddlWorkflow.DataSource = wfList;
            ddlWorkflow.DataTextField = "Name";
            ddlWorkflow.DataValueField = "ID";
            ddlWorkflow.DataBind();
        }

        protected void ddlWorkflow_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var wfId = Convert.ToInt32(ddlWorkflow.SelectedValue);
            var wfObj = wfService.GetById(wfId);
            if (wfObj != null)
            {
                txtDescription.Text = wfObj.Description;
            }
        }
    }
}