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
    public partial class TrackingBreakdownReportEditForm : Page
    {
        private readonly TrackingBreakdownReportService trackingBreakdownReportService;

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
        public TrackingBreakdownReportEditForm()
        {
            this.trackingBreakdownReportService = new TrackingBreakdownReportService();
            this.projectService = new ScopeProjectService();
            this.numberManagementService = new NumberManagementService();
            this.userService = new UserService();
            this.configService = new IntergrateParamConfigService();
            this.roleService = new RoleService();
            this.sharePermissionService = new SharePermissionService();
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

                    var obj = this.trackingBreakdownReportService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if(obj!=null)
                    {
                        this.txtFacility.Text = obj.ProjectName;

                        this.txtBreakdownDate.SelectedDate = obj.BrekdownDate;
                        this.txtBreakdownSystemName.Text = obj.BreakdownSystemName;
                        this.txtTagno.Text = obj.TagNo;
                        this.txtSystemName.Text = obj.SystemName;
                        this.ddlPriority.SelectedValue = obj.Priority;
                        this.txtCauseGroup.Text = obj.CauseGroup;
                        this.txtDescription.Text = obj.Description;
                        this.txtFalureDuplication.Text = obj.FailureDuplication;
                        this.txtRootCause.Text = obj.RootCause;
                        this.txtProposedAction.Text = obj.ProposedAction;
                        this.txtLesson.Text = obj.Lesson;
                        this.txtUnplannedWoNo.Text = obj.UnplannedWoNo;
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

                        this.txtPICDeadline.Text = obj.PICDeadline;
                        this.txtPicStatus.Text = obj.PICStatus;
                        this.txtCurrentStatus.Text = obj.CurrentStatus;
                        this.txtMRWRItem.Text = obj.MRWRItem;
                        this.txtComment.Text = obj.Comment;
                        this.ddlOpen.SelectedValue = obj.Status;
                        this.txtCost.Value = obj.Cost;

                        this.txtReasonDeadlineChange.Text = obj.DeadlineReasonChange;
                        this.txtCode.Text = obj.Code;
                        this.cbComplete.Checked = obj.IsComplete.GetValueOrDefault();

                        this.lblCreated.Text = "Created at " + obj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + obj.CreatedByName;
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

                    var index = this.numberManagementService.GetByName("TrackingBreakdownReport", projectId);
                    var objCode = "BDR-" + index.NextCount.GetValueOrDefault();

                    this.txtCode.Text = objCode;
                }
            }
        }

        private void GetFuncPermissionConfig()
        {
            // Get Function permission of user for MR Module
            var fncPermission = this.fncPermissionService.GetOne(UserSession.Current.User.Id, 5);
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

                var obj = new TrackingBreakdownReport();
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var objId = this.Request.QueryString["objId"];
                    obj = trackingBreakdownReportService.GetById(new Guid(objId));
                    if (obj != null)
                    {
                        obj.UpdatedByName = UserSession.Current.User.FullName;
                        obj.UpdatedBy = UserSession.Current.User.Id;
                        obj.UpdatedDate = DateTime.Now;
                        this.CollectData(obj);

                        this.trackingBreakdownReportService.Update(obj);
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
                    var index = this.numberManagementService.GetByName("TrackingBreakdownReport", projectId);
                    index.NextCount += 1;
                    this.numberManagementService.Update(index);

                    var objGUID = this.trackingBreakdownReportService.Insert(obj);

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


        private void CollectData(TrackingBreakdownReport obj)
        {
            obj.BrekdownDate = this.txtBreakdownDate.SelectedDate;
            obj.BreakdownSystemName = this.txtBreakdownSystemName.Text;
            obj.TagNo = this.txtTagno.Text;
            obj.SystemName = this.txtSystemName.Text;
            obj.Priority = this.ddlPriority.SelectedValue;
            obj.CauseGroup = this.txtCauseGroup.Text;
            obj.Description = this.txtDescription.Text;
            obj.FailureDuplication = this.txtFalureDuplication.Text;
            obj.RootCause = this.txtRootCause.Text;
            obj.ProposedAction = this.txtProposedAction.Text;
            obj.Lesson = this.txtLesson.Text;
            obj.UnplannedWoNo = this.txtUnplannedWoNo.Text;

            obj.PICIds = string.Empty;
            obj.PICName = string.Empty;
            foreach (RadTreeNode pic in this.rtvPIC.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
            {
                obj.PICIds += pic.Value + ";";
                obj.PICName += pic.Text + Environment.NewLine;
            }

            obj.PICDeadline = this.txtPICDeadline.Text;
            obj.PICStatus = this.txtPicStatus.Text;
            obj.CurrentStatus = this.txtCurrentStatus.Text;
            obj.MRWRItem = this.txtMRWRItem.Text;
            obj.Comment = this.txtComment.Text;
            obj.Status = this.ddlOpen.SelectedValue;
            obj.Cost = this.txtCost.Value;

            obj.DeadlineReasonChange = this.txtReasonDeadlineChange.Text;
            obj.Code = this.txtCode.Text;
        }
        private void SendNotification(TrackingBreakdownReport obj)
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

                message.Subject = "New Breakdown Report Update: " + obj.Code;

                // Generate email body
                var bodyContent =
                @"Dear All,<br/><br/>
                Please be informed that the new Breakdown Report Update with information:<br/>
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
		                <td>Equipment Breakdown Date</td>
                        <td>" + (obj.BrekdownDate != null ? obj.BrekdownDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") : string.Empty) + @"</td>
	                </tr>

                    <tr>
		                <td>Name of Breakdown Equipments/System</td>
                        <td>" + obj.BreakdownSystemName.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>Breakdown Equipment Name or Tag No</td>
                        <td>" + obj.TagNo + @"</td>
	                </tr>
                    <tr>
		                <td>Equipment/System Name</td>
                        <td>" + obj.SystemName + @"</td>
	                </tr>
                    <tr>
		                <td>Priority</td>
                        <td>" + obj.Priority + @"</td>
	                </tr>
                    <tr>
		                <td>Cause Group</td>
                        <td>" + obj.CauseGroup + @"</td>
	                </tr>
                    <tr>
		                <td>Defective/Event Descriptions</td>
                        <td>" + obj.Description.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>Failure Duplication</td>
                        <td>" + obj.FailureDuplication + @"</td>
	                </tr>
                    
                    <tr>
		                <td>Root Causes Analysis</td>
                        <td>" + obj.RootCause.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>Proposed Temporary/Remedial Action</td>
                        <td>" + obj.ProposedAction.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    
                    <tr>
		                <td>Lesson Learn/Action Taken To Prevent Recurrence</td>
                        <td>" + obj.Lesson.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>Unplanned WO Number</td>
                        <td>" + obj.UnplannedWoNo + @"</td>
	                </tr>
                    <tr>
		                <td>PIC</td>
                        <td>" + obj.PICName.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr> 

                    <tr>
		                <td>Deadline</td>
                        <td>" + obj.PICDeadline + @"</td>
	                </tr>
                    <tr>
		                <td>Status</td>
                        <td>" + obj.PICStatus + @"</td>
	                </tr>
                    <tr>
		                <td>Current Status</td>
                        <td>" + obj.CurrentStatus.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>Closed/Open</td>
                        <td>" + obj.Status + @"</td>
	                </tr>
                    <tr>
		                <td>MR & Item/ WR</td>
                        <td>" + obj.MRWRItem + @"</td>
	                </tr>
                    <tr>
		                <td>Comments</td>
                        <td>" + obj.Comment.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>Cost</td>
                        <td>" + obj.Cost + @"</td>
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
                        var shareUserList = this.sharePermissionService.GetByFromUserAndObj(userObj.Id, "4");
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