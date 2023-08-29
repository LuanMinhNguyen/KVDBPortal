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
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.WMS;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Workflow
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ReassignWorkingPIC : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly WorkflowService wfService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly ObjectAssignedWorkflowService objAssignedWfService;
        private readonly ObjectAssignedUserService objAssignedUserService;
        private readonly WorkflowStepService wfStepService;
        private readonly WorkflowDetailService wfDetailService;
        private readonly MaterialRequisitionService mrService;
        private readonly WorkRequestService wrService;

        private readonly TrackingMOCService mocService;

        private readonly TrackingECRService ecrService;

        private readonly TrackingShutdownReportService shutdownReportService;

        private readonly TrackingBreakdownReportService breakdownReportService;

        private readonly HolidayConfigService holidayConfigService;

        private readonly HashSet<DateTime> Holidays = new HashSet<DateTime>();

        private readonly RoleService roleService = new RoleService();

        private readonly DQREDocumentService dqreDocumentService = new DQREDocumentService();

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
        public ReassignWorkingPIC()
        {
            this.userService = new UserService();
            this.wfService = new WorkflowService();

            this.objAssignedUserService = new ObjectAssignedUserService();
            this.objAssignedWfService = new ObjectAssignedWorkflowService();

            this.wfStepService = new WorkflowStepService();
            this.wfDetailService = new WorkflowDetailService();
            this.mrService = new MaterialRequisitionService();
            this.wrService = new WorkRequestService();

            this.mocService = new TrackingMOCService();
            this.ecrService = new TrackingECRService();
            this.breakdownReportService = new TrackingBreakdownReportService();
            this.shutdownReportService = new TrackingShutdownReportService();
            this.holidayConfigService = new HolidayConfigService();
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
                var holidayList = this.holidayConfigService.GetAll();
                foreach (var holidayConfig in holidayList)
                {
                    for (DateTime i = holidayConfig.FromDate.GetValueOrDefault(); i < holidayConfig.ToDate.GetValueOrDefault(); i = i.AddDays(1))
                    {
                        this.Holidays.Add(i);
                    }
                }

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
            var currentWorkAssignedManagementUserId = new Guid(this.Request.QueryString["currentAssignId"]);
            var currentWorkAssignedManagementUser = this.objAssignedUserService.GetById(currentWorkAssignedManagementUserId);
            if (currentWorkAssignedManagementUser != null)
            {
                var wfDetailObj = this.wfDetailService.GetByCurrentStep(currentWorkAssignedManagementUser.CurrentWorkflowStepId.GetValueOrDefault());
                var docObj = this.dqreDocumentService.GetById(currentWorkAssignedManagementUser.ObjectID.GetValueOrDefault());

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

                foreach (RadTreeNode user in this.rtvUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                {

                    var assignWorkingUser = new ObjectAssignedUser
                    {
                        ID = Guid.NewGuid(),
                        ObjectAssignedWorkflowID = currentWorkAssignedManagementUser.ObjectAssignedWorkflowID,
                        UserID = Convert.ToInt32(user.Value),
                        ReceivedDate = DateTime.Now,
                        PlanCompleteDate =
                            wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault()
                                ? actualDeadline
                                : DateTime.Now.AddDays(wfDetailObj.Duration.GetValueOrDefault()),
                        IsOverDue = false,
                        IsComplete = false,
                        IsReject = false,
                        AssignedBy = UserSession.Current.User.Id,
                        WorkflowId = currentWorkAssignedManagementUser.WorkflowId,
                        WorkflowName = currentWorkAssignedManagementUser.WorkflowName,
                        CurrentWorkflowStepName = currentWorkAssignedManagementUser.CurrentWorkflowStepName,
                        CurrentWorkflowStepId = currentWorkAssignedManagementUser.CurrentWorkflowStepId,
                        CanReject = currentWorkAssignedManagementUser.CanReject,
                        IsCanCreateOutgoingTrans = wfDetailObj.IsCanCreateOutgoingTrans,
                        IsFinal = wfDetailObj.NextWorkflowStepID == 0,
                        ActionTypeId = 2,
                        ActionTypeName = "C - Comment",
                        WorkingStatus = string.Empty,

                        ObjectID = docObj.ID,
                        ObjectNumber = docObj.DocumentNo,
                        ObjectTitle = docObj.DocumentTitle,
                        ObjectProject = docObj.ProjectCodeName,
                        Revision = docObj.Revision,
                        IsMainWorkflow = currentWorkAssignedManagementUser.IsMainWorkflow,
                        IsAddAnotherDisciplineLead = true,
                    };

                    objAssignedUserService.Insert(assignWorkingUser);

                    // Send email notification
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"])) this.SendNotification(assignWorkingUser, this.userService.GetByID(Convert.ToInt32(user.Value)));
                    // -----------------------------------------------------------------------------------------
                }
            }

            ClientScript.RegisterStartupScript(Page.GetType(), "mykey", !string.IsNullOrEmpty(this.Request.QueryString["flag"]) ? "Close();" : "CloseAndRebind();", true);
        }

        private void ProcessWorkflow(WorkflowStep wfStepObj, int wfId, object obj, int assignedBy, string objType, int assignUserId, Guid assignWorkflowId)
        {
            var assignUserObj = this.userService.GetByID(assignUserId);
            var wfObj = this.wfService.GetById(wfId);
            var wfDetailObj = this.wfDetailService.GetByCurrentStep(wfStepObj.ID);
            if (wfDetailObj != null)
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
                var assignUser = new ObjectAssignedUser
                {
                    ID = Guid.NewGuid(),
                    ObjectAssignedWorkflowID = assignWorkflowId,
                    UserID = assignUserId,
                    ReceivedDate = DateTime.Now,
                    PlanCompleteDate = wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault() ? actualDeadline : DateTime.Now.AddDays(wfDetailObj.Duration.GetValueOrDefault()),
                    IsOverDue = false,
                    IsComplete = false,
                    IsReject = false,
                    AssignedBy = assignedBy,
                    WorkflowId = wfObj.ID,
                    WorkflowName = wfObj.Name,
                    CurrentWorkflowStepName = wfStepObj.Name,
                    CurrentWorkflowStepId = wfStepObj.ID,
                    CanReject = wfStepObj.CanReject,

                    IsDistributeOnshore = currentWFStepDetail.NextWorkflowStepID == 0,
                    IsOnshoreComment = false,
                    IsFinal = wfDetailObj.NextWorkflowStepID == 0,
                    ActionTypeId = 1,
                    ActionTypeName = "Working",
                    WorkingStatus = string.Empty,
                };

                switch (objType)
                {
                    case "Material Requisition":
                        var mrObj = (MaterialRequisition)obj;
                        assignUser.ObjectID = mrObj.ID;
                        assignUser.ObjectNumber = mrObj.MRNo;
                        assignUser.ObjectTitle = mrObj.Justification;
                        assignUser.ObjectProject = mrObj.ProjectName;
                        assignUser.ObjectType = "Material Requisition";
                        break;
                    case "Work Request":
                        var wrObj = (WorkRequest)obj;
                        assignUser.ObjectID = wrObj.ID;
                        assignUser.ObjectNumber = wrObj.WRNo;
                        assignUser.ObjectTitle = wrObj.WRTitle;
                        assignUser.ObjectProject = wrObj.ProjectName;
                        assignUser.ObjectType = "Work Request";
                        break;
                    case "MOC":
                        var mocObj = (TrackingMOC)obj;
                        assignUser.ObjectID = mocObj.ID;
                        assignUser.ObjectNumber = mocObj.Code;
                        assignUser.ObjectTitle = "<b>Equiment/system: </b>" + mocObj.SystemName + "</br> <b>Description of change: </b>" + mocObj.DescriptionOfChange;
                        assignUser.ObjectProject = mocObj.ProjectName;
                        assignUser.ObjectType = "MOC";
                        break;
                    case "ECR":
                        var ecrObj = (TrackingECR)obj;
                        assignUser.ObjectID = ecrObj.ID;
                        assignUser.ObjectNumber = ecrObj.Code;
                        assignUser.ObjectTitle = "<b>ECT Title: </b>" + ecrObj.Title + "</br> <b>ECR Description: </b>" + ecrObj.Description;
                        assignUser.ObjectProject = ecrObj.ProjectName;
                        assignUser.ObjectType = "ECR";
                        break;
                    case "Breakdown Report":
                        var brObj = (TrackingBreakdownReport)obj;
                        assignUser.ObjectID = brObj.ID;
                        assignUser.ObjectNumber = brObj.Code;
                        assignUser.ObjectTitle = "<b>Name of Breakdown Equipments/System: </b>" + brObj.BreakdownSystemName + "</br> <b>Breakdown Equipment Name or Tag No: </b>" + brObj.TagNo + "</br> <b>Equipment/System Name: </b>" + brObj.SystemName + "</br> <b>Defective/Event Descriptions: </b>" + brObj.Description;
                        assignUser.ObjectProject = brObj.ProjectName;
                        assignUser.ObjectType = "Breakdown Report";
                        break;
                    case "Shutdown Report":
                        var srObj = (TrackingShutdownReport)obj;
                        assignUser.ObjectID = srObj.ID;
                        assignUser.ObjectNumber = srObj.Code;
                        assignUser.ObjectTitle = "<b>Date of shutdown: </b>" + srObj.DateOfShutdown.GetValueOrDefault().ToString("dd/MM/yyyy") + "</br> <b>Time of Shutdown: </b>" + srObj.TimeOfShutdown.GetValueOrDefault().ToString("HH:mm") + "</br> <b>Cause Of Shutdown: </b>" + srObj.CauseShutdown;
                        assignUser.ObjectProject = srObj.ProjectName;
                        assignUser.ObjectType = "Shutdown Report";
                        break;
                }

                objAssignedUserService.Insert(assignUser);

                // Update current info for Object
                switch (objType)
                {
                    case "Material Requisition":
                        var mrObj = (MaterialRequisition)obj;
                        mrObj.CurrentAssignUserName = assignUserObj.FullNameWithDeptPosition;
                        this.mrService.Update(mrObj);
                        break;
                    case "Work Request":
                        var wrObj = (WorkRequest)obj;
                        wrObj.CurrentAssignUserName = assignUserObj.FullNameWithDeptPosition;
                        this.wrService.Update(wrObj);
                        break;
                    case "MOC":
                        var mocObj = (TrackingMOC)obj;
                        mocObj.CurrentAssignUserName = assignUserObj.FullNameWithDeptPosition;
                        this.mocService.Update(mocObj);
                        break;
                    case "ECR":
                        var ecrObj = (TrackingECR)obj;
                        ecrObj.CurrentAssignUserName = assignUserObj.FullNameWithDeptPosition;
                        this.ecrService.Update(ecrObj);
                        break;
                    case "Breakdown Report":
                        var brObj = (TrackingBreakdownReport)obj;
                        brObj.CurrentAssignUserName = assignUserObj.FullNameWithDeptPosition;
                        this.breakdownReportService.Update(brObj);
                        break;
                    case "Shutdown Report":
                        var srObj = (TrackingShutdownReport)obj;
                        srObj.CurrentAssignUserName = assignUserObj.FullNameWithDeptPosition;
                        this.shutdownReportService.Update(srObj);
                        break;
                }
                // ----------------------------------------------------------------------------------------------
            }
        }
        private void SendNotification(ObjectAssignedUser assignWorkingUser, User assignUserObj)
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
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "EDMS System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;

                message.Subject = "REASSIGN: " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.ObjectTitle + ", " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy");

                // Generate email body
                var bodyContent = @"<<< FOR REASSIGN >>>
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
                                     <tr>
		                                <td>From User</td>
                                        <td>" + this.userService.GetByID(assignWorkingUser.AssignedBy.GetValueOrDefault()) + @"</td>
	                                </tr>
                                </table><br/>

                              EDMS WORKFLOW NOTIFICATION <br/>
                        [THIS IS SYSTEM GENERATED NOTIFICATION PLEASE DO NOT REPLY]

                                ";
                message.Body = bodyContent;
                if (!string.IsNullOrEmpty(assignUserObj.Email))
                {
                    message.To.Add(assignUserObj.Email);
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
                }
            }

            var roleList = this.roleService.GetAll(false).Where(t => t.TypeId == 1);
            this.ddlRole.DataSource = roleList;
            this.ddlRole.DataTextField = "Name";
            this.ddlRole.DataValueField = "Id";
            this.ddlRole.DataBind();

            if (this.ddlRole.SelectedItem != null)
            {
                var userList = this.userService.GetAllByRoleId(Convert.ToInt32(this.ddlRole.SelectedValue));
                foreach (var user in userList)
                {
                    var userNode = new RadTreeNode(user.UserNameWithFullNamePosition, user.Id.ToString());
                    this.rtvUser.Nodes.Add(userNode);
                }
            }
        }

        protected void ddlRole_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            if (this.ddlRole.SelectedItem != null)
            {
                this.rtvUser.Nodes.Clear();

                var userList = this.userService.GetAllByRoleId(Convert.ToInt32(this.ddlRole.SelectedValue));
                foreach (var user in userList)
                {
                    var userNode = new RadTreeNode(user.UserNameWithFullNamePosition, user.Id.ToString());
                    this.rtvUser.Nodes.Add(userNode);
                }
            }
        }
    }
}