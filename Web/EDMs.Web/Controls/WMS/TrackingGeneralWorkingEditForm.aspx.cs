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
    public partial class TrackingGeneralWorkingEditForm : Page
    {
        private readonly TrackingGeneralWorkingService trackingGeneralWorkingService;

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
        public TrackingGeneralWorkingEditForm()
        {
            this.trackingGeneralWorkingService = new TrackingGeneralWorkingService();
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
                if(!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var obj = this.trackingGeneralWorkingService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if(obj!=null)
                    {
                        this.txtFacility.Text = obj.ProjectName;
                        this.ddlWorkCategory.SelectedValue = obj.WorkCategoryId.ToString();
                        this.txtWorkContent.Text = obj.WorkContent;

                        foreach (RadTreeNode deptNode in this.rtvPIC.Nodes)
                        {
                            foreach (RadTreeNode userNode in deptNode.Nodes)
                            {
                                if (!string.IsNullOrEmpty(obj.AssignUserIds) && obj.AssignUserIds.Split(';').Contains(userNode.Value))
                                {
                                    userNode.Checked = true;
                                }
                            }
                        }

                        foreach (RadTreeNode deptNode in this.rtvBackup.Nodes)
                        {
                            foreach (RadTreeNode userNode in deptNode.Nodes)
                            {
                                if (!string.IsNullOrEmpty(obj.BackupUserIds) && obj.BackupUserIds.Split(';').Contains(userNode.Value))
                                {
                                    userNode.Checked = true;
                                }
                            }
                        }

                        foreach (RadTreeNode deptNode in this.rtvAudit.Nodes)
                        {
                            foreach (RadTreeNode userNode in deptNode.Nodes)
                            {
                                if (!string.IsNullOrEmpty(obj.VerifyUserIds) && obj.VerifyUserIds.Split(';').Contains(userNode.Value))
                                {
                                    userNode.Checked = true;
                                }
                            }
                        }

                        if (obj.StartDate != null)
                        {
                            this.txtStartDate.SelectedDate = obj.StartDate;
                        }
                        else
                        {
                            this.txtStartDate1.Text = obj.StartDate1;
                        }

                        if (obj.Deadline != null)
                        {
                            this.txtDeadline.SelectedDate = obj.Deadline;
                        }
                        else
                        {
                            this.txtDeadline1.Text = obj.Deadline1;
                        }

                        this.ddlStatus.SelectedValue = obj.Status;
                        this.txtDescription.Text = obj.Description;

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

                    var index = this.numberManagementService.GetByName("TrackingGeneralWorking", projectId);
                    var objCode = "GW-" + index.NextCount.GetValueOrDefault();

                    this.txtCode.Text = objCode;
                }
            }
        }

        private void GetFuncPermissionConfig()
        {
            // Get Function permission of user for MR Module
            var fncPermission = this.fncPermissionService.GetOne(UserSession.Current.User.Id, 13);
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

            foreach (var role in deptList)
            {
                var detpNode = new RadTreeNode(role.FullNameWithLocation);
                var userList1 = this.userService.GetAllByRoleId(role.Id).OrderByDescending(t => t.IsDeptManager).ThenBy(t => t.FullNameWithPosition);
                foreach (var user in userList1)
                {
                    detpNode.Nodes.Add(new RadTreeNode(user.FullNameWithPosition, user.Id.ToString()));
                }

                this.rtvBackup.Nodes.Add(detpNode);
            }

            foreach (var role in deptList)
            {
                var detpNode = new RadTreeNode(role.FullNameWithLocation);
                var userList1 = this.userService.GetAllByRoleId(role.Id).OrderByDescending(t => t.IsDeptManager).ThenBy(t => t.FullNameWithPosition);
                foreach (var user in userList1)
                {
                    detpNode.Nodes.Add(new RadTreeNode(user.FullNameWithPosition, user.Id.ToString()));
                }

                this.rtvAudit.Nodes.Add(detpNode);
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

                var obj = new TrackingGeneralWorking();
                obj.ID = Guid.NewGuid();

                obj.ProjectId = projectObj.ID;
                obj.ProjectName = projectObj.Name;
                
                obj.WorkCategoryId = Convert.ToInt32(this.ddlWorkCategory.SelectedValue);
                obj.WorkCategoryName = this.ddlWorkCategory.SelectedItem.Text;
                obj.WorkContent = this.txtWorkContent.Text;

                obj.AssignUserIds = string.Empty;
                obj.AssignUserName = string.Empty;
                foreach (RadTreeNode pic in this.rtvPIC.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                {
                    obj.AssignUserIds += pic.Value + ";";
                    obj.AssignUserName += pic.Text + Environment.NewLine;
                }

                obj.BackupUserIds = string.Empty;
                obj.BackupUserName = string.Empty;
                foreach (RadTreeNode backup in this.rtvBackup.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                {
                    obj.BackupUserIds += backup.Value + ";";
                    obj.BackupUserName += backup.Text + Environment.NewLine;
                }

                obj.VerifyUserIds = string.Empty;
                obj.VerifyUserName = string.Empty;
                foreach (RadTreeNode audit in this.rtvAudit.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                {
                    obj.VerifyUserIds += audit.Value + ";";
                    obj.VerifyUserName += audit.Text + Environment.NewLine;
                }

                if (this.txtStartDate.SelectedDate != null)
                {
                    obj.StartDate = this.txtStartDate.SelectedDate;
                    obj.StartDate1 = this.txtStartDate.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy");
                }
                else
                {
                    obj.StartDate1 = this.txtStartDate1.Text;
                }

                if (this.txtDeadline.SelectedDate != null)
                {
                    obj.Deadline = this.txtDeadline.SelectedDate;
                    obj.Deadline1 = this.txtDeadline.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy");
                }
                else
                {
                    obj.Deadline1 = this.txtDeadline1.Text;
                }

                obj.Status = this.ddlStatus.SelectedValue;
                obj.Description = this.txtDescription.Text;

                obj.DeadlineReasonChange = this.txtReasonDeadlineChange.Text;

                obj.IsComplete = this.cbComplete.Checked;
                obj.IsLeaf = true;
                obj.Code = this.txtCode.Text;

                obj.CreatedByName = UserSession.Current.User.FullName;
                obj.CreatedBy = UserSession.Current.User.Id;
                obj.CreatedDate = DateTime.Now;

                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var objId = new Guid(this.Request.QueryString["objId"]);
                    var objOld = this.trackingGeneralWorkingService.GetById(objId);
                    if (objOld != null)
                    {
                        objOld.IsLeaf = false;
                        this.trackingGeneralWorkingService.Update(objOld);

                        obj.ParentId = objOld.ParentId ?? objOld.ID;
                    }
                }
                else
                {
                    var index = this.numberManagementService.GetByName("TrackingGeneralWorking", projectId);
                    index.NextCount += 1;
                    this.numberManagementService.Update(index);
                }

               var objGUID = this.trackingGeneralWorkingService.Insert(obj);
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
                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
        }

        private void SendNotification(TrackingGeneralWorking obj)
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

                message.Subject = "New General Working Update: " + obj.Code;

                // Generate email body
                var bodyContent = @"Dear All,<br/><br/>
                                    Please be informed that the new General Working Update with information:<br/>
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
		                                    <td>Work Category</td>
                                            <td>" + obj.WorkCategoryName.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                    </tr>
                                        <tr>
		                                    <td>Work Content</td>
                                            <td>" + obj.WorkContent.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                    </tr>

                                        <tr>
		                                    <td>User</td>
                                            <td>" + obj.AssignUserName.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                    </tr>

                                        <tr>
		                                    <td>Backup User</td>
                                            <td>" + obj.BackupUserName.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                    </tr>

                                        <tr>
		                                    <td>Auditor</td>
                                            <td>" + obj.VerifyUserName.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                    </tr>
                                        <tr>
		                                    <td>Start Date</td>
                                            <td>" + obj.StartDate1 + @"</td>
	                                    </tr>
                                        <tr>
		                                    <td>Deadline</td>
                                            <td>" + obj.Deadline1 + @"</td>
	                                    </tr>                                        
                                        <tr>
		                                    <td>Status</td>
                                            <td>" + obj.Status + @"</td>
	                                    </tr>
                                        <tr>
		                                    <td>Remark</td>
                                            <td>" + obj.Description.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                    </tr>
                                    </table></br>

                                    Thanks and regards,</br>
                                    WMS System.

                                    ";
                message.Body = bodyContent;
                // -----------------------------------------------------------

                foreach (var assignUser in obj.AssignUserIds.Split(';').Where(t => !string.IsNullOrEmpty(t)))
                {
                    var userObj = this.userService.GetByID(Convert.ToInt32(assignUser));
                    if (userObj != null)
                    {
                        var shareUserList = this.sharePermissionService.GetByFromUserAndObj(userObj.Id, "10");
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

                foreach (var backupUser in obj.BackupUserIds.Split(';').Where(t => !string.IsNullOrEmpty(t)))
                {
                    var userObj = this.userService.GetByID(Convert.ToInt32(backupUser));
                    if (userObj != null)
                    {
                        var shareUserList = this.sharePermissionService.GetByFromUserAndObj(userObj.Id, "10");
                        if (!string.IsNullOrEmpty(userObj.Email))
                        {
                            message.CC.Add(new MailAddress(userObj.Email));
                        }

                        foreach (var shareUser in shareUserList)
                        {
                            if (!string.IsNullOrEmpty(shareUser.ToUserEmail))
                            {
                                message.CC.Add(new MailAddress(shareUser.ToUserEmail));
                            }
                        }
                    }
                }

                foreach (var auditor in obj.VerifyUserIds.Split(';').Where(t => !string.IsNullOrEmpty(t)))
                {
                    var userObj = this.userService.GetByID(Convert.ToInt32(auditor));
                    if (userObj != null)
                    {
                        var shareUserList = this.sharePermissionService.GetByFromUserAndObj(userObj.Id, "10");
                        if (!string.IsNullOrEmpty(userObj.Email))
                        {
                            message.CC.Add(new MailAddress(userObj.Email));
                        }

                        foreach (var shareUser in shareUserList)
                        {
                            if (!string.IsNullOrEmpty(shareUser.ToUserEmail))
                            {
                                message.CC.Add(new MailAddress(shareUser.ToUserEmail));
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