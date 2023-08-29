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
    public partial class AssignToAnotherUser : Page
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

       

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectInfoEditForm"/> class.
        /// </summary>
        public AssignToAnotherUser()
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
            var currentWorkAssignedUserId = new Guid(this.Request.QueryString["objId"]);
            var currentWorkAssignedUser = this.objAssignedUserService.GetById(currentWorkAssignedUserId);
            if (currentWorkAssignedUser != null)
            {
                //// Update Current work assign
                //currentWorkAssignedUser.CommentContent = this.txtMessage.Text.Trim();
                //currentWorkAssignedUser.IsReject = false;
                //currentWorkAssignedUser.IsComplete = true;
                //currentWorkAssignedUser.Status = "SO";
                //currentWorkAssignedUser.ActualDate = DateTime.Now;
                //currentWorkAssignedUser.IsOverDue = currentWorkAssignedUser.PlanCompleteDate.GetValueOrDefault().Date < currentWorkAssignedUser.ActualDate.GetValueOrDefault().Date;
                //this.objAssignedUserService.Update(currentWorkAssignedUser);
                ////// ---------------------------------------------------------------------------------------------

                var wfDetailObj = this.wfDetailService.GetByCurrentStep(currentWorkAssignedUser.CurrentWorkflowStepId.GetValueOrDefault());
                var docObj = this.dqreDocumentService.GetById(currentWorkAssignedUser.ObjectID.GetValueOrDefault());

                // Get actual deadline if workflow step detail use only working day
                var actualDeadline = DateTime.Now;
                if (wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault())
                {
                    for (int i = 1; i <= wfDetailObj.Duration.GetValueOrDefault(); i++)
                    {
                        actualDeadline = this.GetNextWorkingDay(actualDeadline);
                    }
                }
                // -------------------------------------------------------------------------

                foreach (RadTreeNode user in this.rtvUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                {

                    var assignWorkingUser = new ObjectAssignedUser
                    {
                        ID = Guid.NewGuid(),
                        ObjectAssignedWorkflowID = currentWorkAssignedUser.ObjectAssignedWorkflowID,
                        UserID = Convert.ToInt32(user.Value),
                        UserFullName = user.Text,
                        ReceivedDate = DateTime.Now,
                        PlanCompleteDate =
                            wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault()
                                ? actualDeadline
                                : DateTime.Now.AddDays(wfDetailObj.Duration.GetValueOrDefault()),
                        IsOverDue = currentWorkAssignedUser.PlanCompleteDate.GetValueOrDefault().Date < currentWorkAssignedUser.ActualDate.GetValueOrDefault().Date,
                        IsComplete = false,
                        IsReject = false,
                        IsLeaf = true,
                        Status="RS",
                        AssignedBy = UserSession.Current.User.Id,
                        WorkflowId = currentWorkAssignedUser.WorkflowId,
                        WorkflowName = currentWorkAssignedUser.WorkflowName,
                        CurrentWorkflowStepName = currentWorkAssignedUser.CurrentWorkflowStepName,
                        CurrentWorkflowStepId = currentWorkAssignedUser.CurrentWorkflowStepId,
                        CanReject = currentWorkAssignedUser.CanReject,
                        IsCanCreateOutgoingTrans = false,
                        IsFinal = wfDetailObj.NextWorkflowStepID == 0,
                        ActionTypeId = currentWorkAssignedUser.ActionTypeId,
                        ActionTypeName = currentWorkAssignedUser.ActionTypeName,
                        WorkingStatus = string.Empty,

                        ObjectID = docObj.ID,
                        ObjectNumber = docObj.DocumentNo,
                        ObjectTitle = docObj.DocumentTitle,
                        ObjectProject = docObj.ProjectCodeName,
                        Revision = docObj.Revision,
                        IsMainWorkflow = currentWorkAssignedUser.IsMainWorkflow,
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

   
        private void SendNotification(ObjectAssignedUser assignWorkingUser, User assignUserObj)
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
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                };


                var message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "EDMS System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;

                message.Subject = "REASSIGN: " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.ObjectTitle + ", " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy");

                // Generate email body
                var bodyContent = @"<<<< FOR REASSIGN >>>>
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
		                                <td>Assign To User</td>
                                        <td>" + assignWorkingUser.UserFullName + @"</td>
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
                                        <td>" + this.userService.GetByID(assignWorkingUser.AssignedBy.GetValueOrDefault()).FullNameWithDeptPosition + @"</td>
	                                </tr>
                                </table><br/>
                                <br/>
                                    &nbsp;Click on the this link to access the PEDMS system&nbsp;:&nbsp; <a href='" + ConfigurationSettings.AppSettings.Get("WebAddress")
                                           + "/ToDoListPage.aspx?DocNo=" + assignWorkingUser.ObjectNumber + "'>" + ConfigurationSettings.AppSettings.Get("WebAddress") + @"</a>
                                    </br>
                         &nbsp;&nbsp;&nbsp; EDMS WORKFLOW NOTIFICATION <br/>
                        [THIS IS SYSTEM GENERATED NOTIFICATION PLEASE DO NOT REPLY] ";
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
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var currentWorkAssignedUserId = new Guid(this.Request.QueryString["objId"]);
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