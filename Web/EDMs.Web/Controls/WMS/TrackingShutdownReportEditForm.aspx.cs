// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.WMS;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.WMS
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class TrackingShutdownReportEditForm : Page
    {
        private readonly TrackingShutdownReportService trackingShutdownReportService;

        private readonly ScopeProjectService projectService;

        private readonly NumberManagementService numberManagementService;

        private readonly UserService userService;

        private readonly IntergrateParamConfigService configService;

        private readonly SharePermissionService sharePermissionService;

        private readonly RoleService roleService;

        private readonly FunctionPermissionService fncPermissionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MREditForm"/> class.
        /// </summary>
        public TrackingShutdownReportEditForm()
        {
            this.trackingShutdownReportService = new TrackingShutdownReportService();
            this.projectService = new ScopeProjectService();
            this.numberManagementService = new NumberManagementService();
            this.userService = new UserService();
            this.configService = new IntergrateParamConfigService();
            this.sharePermissionService = new SharePermissionService();
            this.roleService = new RoleService();
            this.fncPermissionService = new FunctionPermissionService();
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
                this.GetFuncPermissionConfig();
                this.LoadComboData();

                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var obj = this.trackingShutdownReportService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if(obj!=null)
                    {
                        this.txtFacility.Text = obj.ProjectName;

                        this.txtDateShutdown.SelectedDate = obj.DateOfShutdown;
                        this.txtTimeShutdown.SelectedDate = obj.TimeOfShutdown;
                        this.txtDateResume.SelectedDate = obj.DateResume;
                        this.txtTimeResume.SelectedDate = obj.TimeResume;
                        this.txtDowntime.Value = obj.DownTime;
                        this.txtProductionLoss.Value = obj.EstimatedProduction;
                        this.txtCauseShutdown.Text = obj.CauseShutdown;
                        this.txtProcess.Text = obj.CauseClarificationProcess;
                        this.txtDeadline.SelectedDate = obj.Deadline;
                        this.txtPowerLoss.Text = obj.CauseClarificationPowerloss;
                        this.txtFireGas.Text = obj.CauseClarificationFireGas;
                        this.txtRootCause.Text = obj.RootCause;
                        this.txtAreaConcern.Text = obj.AreaConcern;
                        this.txtWayForward.Text = obj.WayForward;
                        this.txtStatus.Text = obj.Status;
                        this.txtLesson.Text = obj.Lesson;
                        foreach (RadTreeNode deptNode in this.rtvPIC.Nodes)
                        {
                            foreach (RadTreeNode userNode in deptNode.Nodes)
                            {
                                if (!string.IsNullOrEmpty(obj.PICIds) && obj.PICIds.Split(';').Contains(userNode.Value))
                                {
                                    userNode.Checked = true;
                                }
                            }
                        }

                        this.txtReasonDeadlineChange.Text = obj.DeadlineReasonChange;
                        this.txtCode.Text = obj.Code;
                        this.cbComplete.Checked = obj.IsComplete.GetValueOrDefault();

                        this.lblCreated.Text = "Updated at " + obj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + obj.CreatedByName;
                        if (obj.UpdatedBy != null && obj.UpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            this.lblUpdated.Text = "Last modified at " + obj.UpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + obj.UpdatedByName;
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }
                    }
                }
                else
                {
                    var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
                    var projectObj = this.projectService.GetById(projectId);
                    this.txtFacility.Text = projectObj.Name;
                    this.CreatedInfo.Visible = false;

                    var index = this.numberManagementService.GetByName("TrackingShutdownReport", projectId);
                    var objCode = "SDR-" + index.NextCount.GetValueOrDefault();

                    this.txtCode.Text = objCode;
                }
            }
        }

        private void GetFuncPermissionConfig()
        {
            // Get Function permission of user for MR Module
            var fncPermission = this.fncPermissionService.GetOne(UserSession.Current.User.Id, 6);
            if (fncPermission != null)
            {
                this.IsView.Value = fncPermission.IsView.GetValueOrDefault().ToString();
                this.IsCreate.Value = fncPermission.IsCreate.GetValueOrDefault().ToString();
                this.IsUpdate.Value = fncPermission.IsUpdate.GetValueOrDefault().ToString();
                this.IsCancel.Value = fncPermission.IsCancel.GetValueOrDefault().ToString();
                this.IsAttachWF.Value = fncPermission.IsAttachWorkflow.GetValueOrDefault().ToString();
            }
            else
            {
                this.IsView.Value = "False";
                this.IsCreate.Value = "False";
                this.IsUpdate.Value = "False";
                this.IsCancel.Value = "False";
                this.IsAttachWF.Value = "False";
            }
            // ----------------------------------------------------------------------------------------
        }

        private void LoadComboData()
        {
            var deptList = this.roleService.GetAll(false).OrderBy(t => t.NameWithLocation);
            foreach (var role in deptList)
            {
                var detpNode = new RadTreeNode(role.FullNameWithLocation);
                var userList1 = this.userService.GetAllByRoleId(role.Id).OrderByDescending(t => t.IsDeptManager).ThenBy(t => t.FullNameWithPosition);
                foreach (var user in userList1)
                {
                    detpNode.Nodes.Add(new RadTreeNode(user.FullNameWithPosition, user.Id.ToString()));
                }

                this.rtvPIC.Nodes.Add(detpNode);
            }

            // Show hide function Control
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                this.btnSave.Visible = Convert.ToBoolean(this.IsUpdate.Value);
            }
            else
            {
                this.btnSave.Visible = Convert.ToBoolean(this.IsCreate.Value);
            }
            
            // --------------------------------------------------------------------------
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
            if (this.Page.IsValid)
            {
                var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
                var projectObj = this.projectService.GetById(projectId);

                var obj = new TrackingShutdownReport();
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    obj.UpdatedByName = UserSession.Current.User.FullName;
                    obj.UpdatedBy = UserSession.Current.User.Id;
                    obj.UpdatedDate = DateTime.Now;
                    this.CollectData(obj);

                    this.trackingShutdownReportService.Update(obj);
                    try
                    {
                        var configObj = configService.GetById(1);
                        if (configObj != null && configObj.IsEnableSendEmailNotification.GetValueOrDefault())
                        {
                            this.SendNotification(obj);
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                else
                {
                    obj.ID = Guid.NewGuid();
                    obj.ProjectId = projectObj.ID;
                    obj.ProjectName = projectObj.Name;
                    obj.CreatedByName = UserSession.Current.User.FullName;
                    obj.CreatedBy = UserSession.Current.User.Id;
                    obj.CreatedDate = DateTime.Now;
                    obj.IsWFComplete = false;
                    obj.IsInWFProcess = false;
                    obj.IsCompleteFinal = false;
                    obj.IsCancel = false;

                    this.CollectData(obj);

                    var index = this.numberManagementService.GetByName("TrackingShutdownReport", projectId);
                    index.NextCount += 1;
                    this.numberManagementService.Update(index);

                    var objGUID = this.trackingShutdownReportService.Insert(obj);

                    if (objGUID != null)
                    {
                        try
                        {
                            var configObj = configService.GetById(1);
                            if (configObj != null && configObj.IsEnableSendEmailNotification.GetValueOrDefault())
                            {
                                this.SendNotification(obj);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
        }

        private void CollectData(TrackingShutdownReport obj)
        {
            obj.DateOfShutdown = this.txtDateShutdown.SelectedDate;
            obj.TimeOfShutdown = this.txtTimeShutdown.SelectedDate;
            obj.DateResume = this.txtDateResume.SelectedDate;
            obj.TimeResume = this.txtTimeResume.SelectedDate;
            obj.DownTime = this.txtDowntime.Value;
            obj.EstimatedProduction = this.txtProductionLoss.Value;
            obj.CauseShutdown = this.txtCauseShutdown.Text;
            obj.CauseClarificationProcess = this.txtProcess.Text;
            obj.Deadline = this.txtDeadline.SelectedDate;
            obj.CauseClarificationPowerloss = this.txtPowerLoss.Text;
            obj.CauseClarificationFireGas = this.txtFireGas.Text;
            obj.RootCause = this.txtRootCause.Text;
            obj.AreaConcern = this.txtAreaConcern.Text;
            obj.WayForward = this.txtWayForward.Text;
            obj.Status = this.txtStatus.Text;
            obj.Lesson = this.txtLesson.Text;
            obj.PICIds = string.Empty;
            obj.PICName = string.Empty;
            foreach (RadTreeNode pic in this.rtvPIC.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
            {
                obj.PICIds += pic.Value + ";";
                obj.PICName += pic.Text + Environment.NewLine;
            }

            obj.DeadlineReasonChange = this.txtReasonDeadlineChange.Text;

            obj.IsComplete = this.cbComplete.Checked;
            obj.Code = this.txtCode.Text;
        }

        private void SendNotification(TrackingShutdownReport obj)
        {
            var configObj = configService.GetById(1);
            if (configObj != null)
            {
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

                message.Subject = "New Shutdown Report Update: " + obj.Code;

                // Generate email body
                var bodyContent =
                @"Dear All,<br/><br/>
                Please be informed that the new Shutdown Report Update with information:<br/>
                <table border='1' cellspacing='0'>
	                <tr>
		                <td style=""width: 200px;"">Updated By</td>
                         <td style=""width: 500px;"">" + obj.CreatedByName + @"</td>
	                </tr>
                    <tr>
		                <td>Updated Date</td>
                        <td>" + obj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") + @"</td>
	                </tr>
                    <tr>
		                <td>Code</td>
                        <td>" + obj.Code + @"</td>
	                </tr>
                    <tr>
		                <td>Date of shutdown</td>
                        <td>" + (obj.DateOfShutdown != null ? obj.DateOfShutdown.GetValueOrDefault().ToString("dd/MM/yyyy") : string.Empty) + @"</td>
	                </tr>
                    <tr>
		                <td>Time of Shutdown</td>
                        <td>" + (obj.TimeOfShutdown != null ? obj.TimeOfShutdown.GetValueOrDefault().ToString("HH:mm:ss") : string.Empty) + @"</td>
	                </tr>
                    <tr>
		                <td>Date Resumed</td>
                        <td>" + (obj.DateResume != null ? obj.DateResume.GetValueOrDefault().ToString("dd/MM/yyyy") : string.Empty) + @"</td>
	                </tr>
                    <tr>
		                <td>Time Resumed</td>
                        <td>" + (obj.TimeResume != null ? obj.TimeResume.GetValueOrDefault().ToString("HH:mm:ss") : string.Empty) + @"</td>
	                </tr>
                    <tr>
		                <td>Downtime (hour)</td>
                        <td>" + obj.DownTime + @"</td>
	                </tr>
                    <tr>
		                <td>Estimated Production loss (Bbls)</td>
                        <td>" + obj.EstimatedProduction + @"</td>
	                </tr>
                    <tr>
		                <td>Cause Of Shutdown</td>
                        <td>" + obj.CauseShutdown + @"</td>
	                </tr>
                    <tr>
		                <td>Cause clarification: Process</td>
                        <td>" + obj.CauseClarificationProcess + @"</td>
	                </tr>
                    <tr>
		                <td>Cause clarification: Power loss</td>
                        <td>" + obj.CauseClarificationPowerloss + @"</td>
	                </tr>
                    <tr>
		                <td>Cause clarification: Fire & Gas</td>
                        <td>" + obj.CauseClarificationFireGas + @"</td>
	                </tr>
                    <tr>
		                <td>Root cause analysis</td>
                        <td>" + obj.RootCause.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>Area concern</td>
                        <td>" + obj.AreaConcern.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>Way Forward</td>
                        <td>" + obj.WayForward.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>PIC</td>
                        <td>" + obj.PICName.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>  
                    <tr>
		                <td>Deadline</td>
                        <td>" + (obj.Deadline != null ? obj.Deadline.GetValueOrDefault().ToString("dd/MM/yyyy") : string.Empty) + @"</td>
	                </tr>
                    <tr>
		                <td>Status</td>
                        <td>" + obj.Status + @"</td>
	                </tr>
                    <tr>
		                <td>Lesson & leart</td>
                        <td>" + obj.Lesson.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                </table></br>

                Thanks and regards,</br>
                WMS System.

                ";
                message.Body = bodyContent;
                // -----------------------------------------------------------

                foreach (var picId in obj.PICIds.Split(';').Where(t => !string.IsNullOrEmpty(t)))
                {
                    var userObj = this.userService.GetByID(Convert.ToInt32(picId));
                    if (userObj != null)
                    {
                        var shareUserList = this.sharePermissionService.GetByFromUserAndObj(userObj.Id, "5");
                        if (!string.IsNullOrEmpty(userObj.Email))
                        {
                            message.To.Add(new MailAddress(userObj.Email));
                        }

                        foreach (var shareUser in shareUserList)
                        {
                            if (!string.IsNullOrEmpty(shareUser.ToUserEmail))
                            {
                                message.To.Add(new MailAddress(shareUser.ToUserEmail));
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(UserSession.Current.User.Email))
                {
                    message.CC.Add(UserSession.Current.User.Email);
                }

                smtpClient.Send(message);
            }
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
            this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CancelEdit();", true);
        }
    }
}