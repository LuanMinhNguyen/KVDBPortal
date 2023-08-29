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
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.WMS;
using EDMs.Business.Services.Workflow;

namespace EDMs.Web.Controls.Workflow
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class FinalComplete : Page
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

        private readonly TitleService titleService;

        private readonly RoleService roleService;

        private readonly TrackingMOCService mocService;

        private readonly TrackingECRService ecrService;

        private readonly TrackingShutdownReportService shutdownReportService;

        private readonly TrackingBreakdownReportService breakdownReportService;

        private readonly TrackingMorningCallService mcService;

        private readonly HolidayConfigService holidayConfigService;

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
        public FinalComplete()
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

            this.titleService = new TitleService();
            this.roleService = new RoleService();
            this.mocService = new TrackingMOCService();
            this.ecrService = new TrackingECRService();
            this.breakdownReportService = new TrackingBreakdownReportService();
            this.shutdownReportService = new TrackingShutdownReportService();
            this.mcService = new TrackingMorningCallService();
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
            if (this.ddlDepartment.SelectedItem != null)
            {
                var objId = new Guid(this.Request.QueryString["objId"]);
                var objType = this.Request.QueryString["objType"];
                var currentWorkAssignedUserId = new Guid(this.Request.QueryString["currentAssignId"]);
                var currentWorkAssignedUser = this.objAssignedUserService.GetById(currentWorkAssignedUserId);
                if (currentWorkAssignedUser != null)
                {
                    currentWorkAssignedUser.IsComplete = true;
                    currentWorkAssignedUser.ActualDate = DateTime.Now;
                    currentWorkAssignedUser.IsOverDue = currentWorkAssignedUser.PlanCompleteDate.GetValueOrDefault().Date < currentWorkAssignedUser.ActualDate.GetValueOrDefault().Date;

                    currentWorkAssignedUser.FinalAssignDeptId = Convert.ToInt32(this.ddlDepartment.SelectedValue);
                    currentWorkAssignedUser.FinalAssignDeptName = this.ddlDepartment.SelectedItem.Text;
                    this.objAssignedUserService.Update(currentWorkAssignedUser);

                    // Complete assignment for Manamgement User
                    foreach (var objectAssignedManagementUser in this.objAssignedUserService.GetAllIncompleteManagementByDoc(objId, objType))
                    {
                        objectAssignedManagementUser.IsComplete = true;
                        objectAssignedManagementUser.IsOverDue = false;
                        objectAssignedManagementUser.ActualDate = DateTime.Now;

                        this.objAssignedUserService.Update(objectAssignedManagementUser);
                    }
                    // -----------------------------------------------------------------------

                    switch (objType)
                    {
                        case "Material Requisition":
                            var mrObj = this.mrService.GetById(objId);
                            if (mrObj != null)
                            {
                                mrObj.IsWFComplete = true;
                                mrObj.IsInWFProcess = false;
                                mrObj.FinalAssignDeptId = Convert.ToInt32(this.ddlDepartment.SelectedValue);
                                mrObj.FinalAssignDeptName = this.ddlDepartment.SelectedItem.Text;
                                this.mrService.Update(mrObj);
                            }
                            break;
                        case "Work Request":
                            var wrObj = this.wrService.GetById(objId);
                            if (wrObj != null)
                            {
                                wrObj.IsWFComplete = true;
                                wrObj.IsInWFProcess = false;
                                wrObj.FinalAssignDeptId = Convert.ToInt32(this.ddlDepartment.SelectedValue);
                                wrObj.FinalAssignDeptName = this.ddlDepartment.SelectedItem.Text;
                                this.wrService.Update(wrObj);
                            }
                            break;
                        case "MOC":
                            var mocObj = this.mocService.GetById(objId);
                            if (mocObj != null)
                            {
                                mocObj.IsWFComplete = true;
                                mocObj.IsInWFProcess = false;
                                mocObj.FinalAssignDeptId = Convert.ToInt32(this.ddlDepartment.SelectedValue);
                                mocObj.FinalAssignDeptName = this.ddlDepartment.SelectedItem.Text;
                                this.mocService.Update(mocObj);
                            }
                            break;
                        case "ECR":
                            var ecrObj = this.ecrService.GetById(objId);
                            if (ecrObj != null)
                            {
                                ecrObj.IsWFComplete = true;
                                ecrObj.IsInWFProcess = false;
                                ecrObj.FinalAssignDeptId = Convert.ToInt32(this.ddlDepartment.SelectedValue);
                                ecrObj.FinalAssignDeptName = this.ddlDepartment.SelectedItem.Text;
                                this.ecrService.Update(ecrObj);
                            }
                            break;
                        case "Breakdown Report":
                            var brObj = this.breakdownReportService.GetById(objId);
                            if (brObj != null)
                            {
                                brObj.IsWFComplete = true;
                                brObj.IsInWFProcess = false;
                                brObj.FinalAssignDeptId = Convert.ToInt32(this.ddlDepartment.SelectedValue);
                                brObj.FinalAssignDeptName = this.ddlDepartment.SelectedItem.Text;
                                this.breakdownReportService.Update(brObj);
                            }
                            break;
                        case "Shutdown Report":
                            var srObj = this.shutdownReportService.GetById(objId);
                            if (srObj != null)
                            {
                                srObj.IsWFComplete = true;
                                srObj.IsInWFProcess = false;
                                srObj.FinalAssignDeptId = Convert.ToInt32(this.ddlDepartment.SelectedValue);
                                srObj.FinalAssignDeptName = this.ddlDepartment.SelectedItem.Text;
                                this.shutdownReportService.Update(srObj);
                            }
                            break;
                        case "Morning Call":
                            var mcObj = this.mcService.GetById(objId);
                            if (mcObj != null)
                            {
                                mcObj.IsWFComplete = true;
                                mcObj.IsInWFProcess = false;
                                mcObj.FinalAssignDeptId = Convert.ToInt32(this.ddlDepartment.SelectedValue);
                                mcObj.FinalAssignDeptName = this.ddlDepartment.SelectedItem.Text;
                                this.mcService.Update(mcObj);
                            }
                            break;
                    }
                }
            }
            ClientScript.RegisterStartupScript(Page.GetType(), "mykey", !string.IsNullOrEmpty(this.Request.QueryString["flag"]) ? "Close();" : "CloseAndRebind();", true);
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
                }

                var roleList = this.roleService.GetAll(false).Where(t => t.TypeId == 1);
                this.ddlDepartment.DataSource = roleList;
                this.ddlDepartment.DataTextField = "FullNameWithLocation";
                this.ddlDepartment.DataValueField = "Id";
                this.ddlDepartment.DataBind();
            }
        }
    }
}