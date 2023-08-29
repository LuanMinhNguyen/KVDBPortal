// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web.UI;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Workflow
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class WorkflowStepDefinition : Page
    {

        private readonly WorkflowStepService wfStepService = new WorkflowStepService();
        private readonly WorkflowService wfService = new WorkflowService();
        private readonly WorkflowDetailService wfDetailService = new WorkflowDetailService();
        private readonly ProjectCodeService projectService = new ProjectCodeService();
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

        private ProjectCode ProjectObj
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
                this.txtWorkflow.Text = this.WorkflowObj.Name;
                this.ClearData();
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
            if (!string.IsNullOrEmpty(Request.QueryString["wfId"]))
            {
                if (Session["EditingId"] != null)
                {
                    var wfStepId = Convert.ToInt32(Session["EditingId"]);
                    var wfStepObj = this.wfStepService.GetById(wfStepId);
                    if (wfStepObj != null)
                    {
                        this.CollectData(ref wfStepObj);

                        wfStepObj.UpdatedBy = UserSession.Current.User.Id;
                        wfStepObj.UpdatedDate = DateTime.Now;

                        this.wfStepService.Update(wfStepObj);

                        var wfDetail = this.wfDetailService.GetByCurrentStep(wfStepObj.ID);
                        if (wfDetail != null)
                        {
                            this.CollectDataWfDetail(ref wfDetail, wfStepObj);
                            wfDetail.UpdatedBy = UserSession.Current.User.Id;
                            wfDetail.UpdatedDate = DateTime.Now;
                            this.wfDetailService.Update(wfDetail);
                        }
                    }

                    Session.Remove("EditingId");
                }
                else
                {
                    var wfStepObj = new WorkflowStep();
                    var wfDetailObj = new WorkflowDetail();

                    wfStepObj.CreatedBy = UserSession.Current.User.Id;
                    wfStepObj.CreatedDate = DateTime.Now;

                    wfDetailObj.CreatedBy = UserSession.Current.User.Id;
                    wfDetailObj.CreatedDate = DateTime.Now;

                    this.CollectData(ref wfStepObj);
                    this.wfStepService.Insert(wfStepObj);

                    this.CollectDataWfDetail(ref wfDetailObj, wfStepObj);
                    this.wfDetailService.Insert(wfDetailObj);
                }
            }

            this.ClearData();
            this.grdDocument.Rebind();
        }



        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var wfStepId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            var wfDetail = this.wfDetailService.GetByCurrentStep(wfStepId);
            this.wfStepService.Delete(wfStepId);
            this.wfDetailService.Delete(wfDetail);
            this.grdDocument.Rebind();
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var wfid = Convert.ToInt32(this.Request.QueryString["wfid"]);
            var comResList = this.wfStepService.GetAllByWorkflow(wfid);
            this.grdDocument.DataSource = comResList;    
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }

        protected void grdDocument_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem) e.Item;
            if (e.CommandName == "EditCmd")
            {
                var wfStepId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
                var wfStepObj = this.wfStepService.GetById(wfStepId);
                if (wfStepObj != null)
                {
                    Session.Add("EditingId", wfStepObj.ID);
                    //this.ddlLocation.SelectedValue = wfStepObj.LocationId.GetValueOrDefault().ToString();
                    this.txtStepName.Text = wfStepObj.Name;
                    this.cbIsFirst.Checked = wfStepObj.IsFirst.GetValueOrDefault();
                    this.cbCanReject.Checked = wfStepObj.CanReject.GetValueOrDefault();
                    this.cbCanCreateOutgoingTrans.Checked = wfStepObj.IsCanCreateOutgoingTrans.GetValueOrDefault();
                    this.cbiscreate.Checked = wfStepObj.IsCreated.GetValueOrDefault();
                    foreach (RadTreeNode actionNode in this.rtvAction.Nodes)
                    {
                        actionNode.Checked = !string.IsNullOrEmpty(wfStepObj.ActionApplyCode) && wfStepObj.ActionApplyCode.Split(';').ToList().Contains(actionNode.Value);
                    }
                }
            }
        }

        private void ClearData()
        {
            this.txtStepName.Text = string.Empty;
            this.cbCanReject.Checked = false;
            this.cbIsFirst.Checked = false;
            this.cbCanCreateOutgoingTrans.Checked = false;
            this.cbiscreate.Checked = false;
            foreach (RadTreeNode actionNode in this.rtvAction.Nodes)
            {
                actionNode.Checked = false;
            }
            Session.Remove("EditingId");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearData();
        }

        private void CollectData(ref WorkflowStep wfStepObj)
        {
            wfStepObj.Name = this.txtStepName.Text.Trim();
            wfStepObj.WorkflowID = this.WorkflowObj.ID;
            wfStepObj.WorkflowName = this.WorkflowObj.Name;
            wfStepObj.ProjectID = this.ProjectObj.ID;
            wfStepObj.ProjectName = this.ProjectObj.FullName;
            wfStepObj.IsFirst = this.cbIsFirst.Checked;
            wfStepObj.CanReject = this.cbCanReject.Checked;
            wfStepObj.IsCanCreateOutgoingTrans = this.cbCanCreateOutgoingTrans.Checked;
            wfStepObj.IsCreated = this.cbiscreate.Checked;
            wfStepObj.ActionApplyCode = string.Empty;
            wfStepObj.ActionApplyName = string.Empty;
            foreach (RadTreeNode actionNode in this.rtvAction.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
            {
                wfStepObj.ActionApplyCode += actionNode.Value + ";";
                wfStepObj.ActionApplyName += actionNode.Text + Environment.NewLine;
            }
        }

        private void CollectDataWfDetail(ref WorkflowDetail wfDetailObj, WorkflowStep wfStepObj)
        {
            wfDetailObj.WorkflowID = this.WorkflowObj.ID;
            wfDetailObj.WorkflowName = this.WorkflowObj.Name;
            wfDetailObj.CurrentWorkflowStepID = wfStepObj.ID;
            wfDetailObj.CurrentWorkflowStepName = wfStepObj.Name;
            wfDetailObj.ProjectID = this.ProjectObj.ID;
            wfDetailObj.ProjectName = this.ProjectObj.FullName;
            wfDetailObj.IsFirst = wfStepObj.IsFirst;
            wfDetailObj.CanReject = wfStepObj.CanReject;
            wfDetailObj.IsCanCreateOutgoingTrans = wfStepObj.IsCanCreateOutgoingTrans;
            wfDetailObj.IsCreated = wfStepObj.IsCreated;
            wfDetailObj.IsOnlyWorkingDay = false;
            wfDetailObj.ActionApplyCode = wfStepObj.ActionApplyCode;
            wfDetailObj.ActionApplyName = wfStepObj.ActionApplyName;
            if (wfDetailObj.DistributionMatrixIDs == null)
            {
                wfDetailObj.DistributionMatrixIDs = string.Empty;
            }

            if (wfDetailObj.AssignRoleIDs == null)
            {
                wfDetailObj.AssignRoleIDs = string.Empty;
            }

            if (wfDetailObj.AssignUserIDs == null)
            {
                wfDetailObj.AssignUserIDs = string.Empty;
            }

            if (wfDetailObj.InformationOnlyRoleIDs == null)
            {
                wfDetailObj.InformationOnlyRoleIDs = string.Empty;
            }

            if (wfDetailObj.InformationOnlyUserIDs == null)
            {
                wfDetailObj.InformationOnlyUserIDs = string.Empty;
            }

            if (wfDetailObj.AssignTitleIds == null)
            {
                wfDetailObj.AssignTitleIds = string.Empty;
            }

            if (wfDetailObj.ManagementUserIds == null)
            {
                wfDetailObj.ManagementUserIds = string.Empty;
            }
        }
    }
}