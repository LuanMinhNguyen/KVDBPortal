// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Workflow
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ResponseManagementPage : Page
    {

        private readonly WorkflowStepService wfStepService = new WorkflowStepService();
        private readonly WorkflowService wfService = new WorkflowService();
        private readonly WorkflowDetailService wfDetailService = new WorkflowDetailService();
        private readonly ScopeProjectService projectService = new ScopeProjectService();
        private readonly ObjectAssignedUserService objAssignedUserService = new ObjectAssignedUserService();
        private int WorkflowId
        {
            get
            {
                return Convert.ToInt32(this.Request.QueryString["wfId"]);
            }
        }

        private Data.Entities.Workflow WorkflowObj
        {
            get { return this.wfService.GetById(this.WorkflowId); }
        }

        private ScopeProject ProjectObj
        {
            get { return this.projectService.GetById(this.WorkflowObj.ProjectID.GetValueOrDefault()); }
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
            if (!this.IsPostBack)
            {
            }
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var objId = new Guid(this.Request.QueryString["objId"]);
                this.grdDocument.DataSource = this.objAssignedUserService.GetAllWorkingHistoryByObj(objId).Where(t => !t.IsComplete.GetValueOrDefault()).OrderBy(t => t.ReceivedDate);
            }
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }

        protected void grdDocument_OnBatchEditCommand(object sender, GridBatchEditingEventArgs e)
        {
            foreach (GridBatchEditingCommand command in e.Commands)
            {
                Hashtable newValues = command.NewValues;
                var wfDetailID = Convert.ToInt32(newValues["ID"].ToString());
                var wfDetailObj = this.wfDetailService.GetById(wfDetailID);

                if (wfDetailObj != null)
                {
                    //var wfStepDefineID = Convert.ToInt32(newValues["StepDefinitionID"].ToString());
                    //var isOnlyWorkingDay = Convert.ToBoolean(newValues["IsOnlyWorkingDay"].ToString());
                    var nextWorkflowStepID = Convert.ToInt32(newValues["NextWorkflowStepID"].ToString());
                    var nextWorkflowStepObj = this.wfStepService.GetById(nextWorkflowStepID);
                    var rejectWorkflowStepID = Convert.ToInt32(newValues["RejectWorkflowStepID"].ToString());
                    var rejectWorkflowStepObj = this.wfStepService.GetById(rejectWorkflowStepID);

                    //wfDetailObj.StepDefinitionID = wfStepDefineID;
                    //wfDetailObj.StepDefinitionName = Utility.WorkflowStepDefine[wfStepDefineID];

                    wfDetailObj.NextWorkflowStepID = nextWorkflowStepObj != null
                        ? nextWorkflowStepObj.ID
                        : 0;
                    wfDetailObj.NextWorkflowStepName = nextWorkflowStepObj != null
                        ? nextWorkflowStepObj.Name
                        : string.Empty;

                    wfDetailObj.RejectWorkflowStepID = rejectWorkflowStepObj != null
                        ? rejectWorkflowStepObj.ID
                        : 0;
                    wfDetailObj.RejectWorkflowStepName = rejectWorkflowStepObj != null
                        ? rejectWorkflowStepObj.Name
                        : string.Empty;

                    wfDetailObj.Duration = !string.IsNullOrEmpty(newValues["Duration"].ToString()) 
                                        ? Convert.ToInt32(newValues["Duration"].ToString())
                                        : (double?) null;
                    wfDetailObj.IsOnlyWorkingDay = true;

                    ////wfDetailObj.AssignRoleIDs = string.Empty;
                    ////wfDetailObj.AssignUserIDs = string.Empty;
                    ////wfDetailObj.InformationOnlyRoleIDs = string.Empty;
                    ////wfDetailObj.InformationOnlyUserIDs = string.Empty;
                    ////wfDetailObj.DistributionMatrixIDs = string.Empty;

                    this.wfDetailService.Update(wfDetailObj);
                }
            }
        }

        protected void grdDocument_OnItemUpdated(object sender, GridUpdatedEventArgs e)
        {
            var x = 0;
            var item = (GridEditableItem)e.Item;
            var id = item.GetDataKeyValue("ID").ToString();
        }

        protected void grdDocument_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            var temp = e.CommandName;
        }

        protected void grdDocument_OnPreRender(object sender, EventArgs e)
        {
            var wfStepList = this.wfStepService.GetAllByWorkflow(this.WorkflowId);

            var ddlAcceptStep = (this.grdDocument.MasterTableView.GetBatchEditorContainer("NextWorkflowStepID") as Panel).FindControl("ddlAcceptStep") as RadComboBox;
            var ddlRejectStep = (this.grdDocument.MasterTableView.GetBatchEditorContainer("RejectWorkflowStepID") as Panel).FindControl("ddlRejectStep") as RadComboBox;

            if (ddlAcceptStep != null && ddlRejectStep != null)
            {
                wfStepList.Insert(0, new WorkflowStep() {ID = 0});
                ddlAcceptStep.DataSource = wfStepList;
                ddlAcceptStep.DataTextField = "Name";
                ddlAcceptStep.DataValueField = "ID";
                ddlAcceptStep.DataBind();

                ddlRejectStep.DataSource = wfStepList;
                ddlRejectStep.DataTextField = "Name";
                ddlRejectStep.DataValueField = "ID";
                ddlRejectStep.DataBind();
            }
        }
    }
}