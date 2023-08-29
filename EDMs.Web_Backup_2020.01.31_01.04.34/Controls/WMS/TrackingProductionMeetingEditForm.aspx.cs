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
    public partial class TrackingProductionMeetingEditForm : Page
    {
        private readonly TrackingProductionMeetingService trackingProductionMeetingService;

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
        public TrackingProductionMeetingEditForm()
        {
            this.trackingProductionMeetingService = new TrackingProductionMeetingService();
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
                this.LoadInitData();
                if(!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var obj = this.trackingProductionMeetingService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if(obj!=null)
                    {
                        this.txtFacility.Text = obj.ProjectName;
                        this.txtBODComment.Text = obj.BODCommand;
                        this.txtUpdateComment.Text = obj.UpdateComment;
                        this.txtWorkContent.Text = obj.MainProblem;
                        this.txtDepartment.Text = obj.DeparmentName;
                        this.txtDeadline.Text = obj.Deadline;
                        this.txtNote.Text = obj.Note;
                        this.txtWorkGroup.Text = obj.WorkGroup;

                        
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

                        foreach (RadTreeNode deptNode in this.rtvManager.Nodes)
                        {
                            foreach (RadTreeNode userNode in deptNode.Nodes)
                            {
                                if (!string.IsNullOrEmpty(obj.ManagerIds) && obj.ManagerIds.Split(';').Contains(userNode.Value))
                                {
                                    userNode.Checked = true;
                                }
                            }
                        }

                        this.ddlStatus.SelectedValue = obj.StatusId.ToString();

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
                    if (projectId == 0)
                    {
                        this.txtFacility.Text = "All Project";
                    }
                    else
                    {
                        var projectObj = this.projectService.GetById(projectId);
                        this.txtFacility.Text = projectObj.Name;
                    }
                    
                    this.CreatedInfo.Visible = false;

                    //var index = this.numberManagementService.GetByName("TrackingProductionMeeting", projectId);
                    //var objCode = "GW-" + index.NextCount.GetValueOrDefault();

                    //this.txtCode.Text = objCode;
                }
            }
        }

        private void GetFuncPermissionConfig()
        {
            // Get Function permission of user for MR Module
            var fncPermission = this.fncPermissionService.GetOne(UserSession.Current.User.Id, 7);
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

        private void LoadInitData()
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

                this.rtvManager.Nodes.Add(detpNode);
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
                var obj = new TrackingProductionMeeting();
                obj.ID = Guid.NewGuid();

                obj.ProjectId = Convert.ToInt32(this.Request.QueryString["projId"]);
                obj.ProjectName = this.txtFacility.Text;

                obj.MainProblem = this.txtWorkContent.Text;
                obj.DeparmentName = this.txtDepartment.Text;
                obj.BODCommand = this.txtBODComment.Text;
                obj.Deadline = this.txtDeadline.Text;
                obj.UpdateComment = this.txtUpdateComment.Text;
                obj.Note = this.txtNote.Text;
                obj.WorkGroup = this.txtWorkGroup.Text;
                obj.ManagerIds = string.Empty;
                obj.ManagerName = string.Empty;
                foreach (RadTreeNode manager in this.rtvManager.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                {
                    obj.ManagerIds += manager.Value + ";";
                    obj.ManagerName += manager.Text + Environment.NewLine;
                }

                obj.PICIds = string.Empty;
                obj.PICName = string.Empty;
                foreach (RadTreeNode pic in this.rtvPIC.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                {
                    obj.PICIds += pic.Value + ";";
                    obj.PICName += pic.Text + Environment.NewLine;
                }

                obj.StatusId = Convert.ToInt32(this.ddlStatus.SelectedValue);
                obj.StatusName = this.ddlStatus.SelectedItem.Text;

                obj.IsComplete = this.cbComplete.Checked;
                obj.IsLeaf = true;
                obj.Code = this.txtCode.Text;

                obj.CreatedByName = UserSession.Current.User.FullName;
                obj.CreatedBy = UserSession.Current.User.Id;
                obj.CreatedDate = DateTime.Now;

                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var objId = new Guid(this.Request.QueryString["objId"]);
                    var objOld = this.trackingProductionMeetingService.GetById(objId);
                    if (objOld != null)
                    {
                        objOld.IsLeaf = false;
                        this.trackingProductionMeetingService.Update(objOld);

                        obj.ParentId = objOld.ParentId ?? objOld.ID;
                    }
                }

               var objGUID = this.trackingProductionMeetingService.Insert(obj);
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

        private void SendNotification(TrackingProductionMeeting obj)
        {
            var configObj = configService.GetById(1);
            if (configObj != null && (!string.IsNullOrEmpty(obj.PICIds) || !string.IsNullOrEmpty(obj.ManagerIds)))
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

                message.Subject = "New Tracking Operation Meeting Update: " + obj.Code;

                // Generate email body
                var bodyContent = @"Dear All,<br/><br/>
                                    Please be informed that the new Tracking Operation Meeting Update with information:<br/>
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
		                                    <td>Work Group</td>
                                            <td>" + obj.WorkGroup.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                    </tr>

                                        <tr>
		                                    <td>Work Description</td>
                                            <td>" + obj.MainProblem.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                    </tr>
                                        <tr>
		                                    <td>BOD Comment</td>
                                            <td>" + obj.BODCommand.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                    </tr>
                                        
                                        <tr>
		                                    <td>Department</td>
                                            <td>" + obj.DeparmentName.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                    </tr>

                                        <tr>
		                                    <td>Manager</td>
                                            <td>" + obj.ManagerName.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                    </tr>

                                        <tr>
		                                    <td>PIC</td>
                                            <td>" + obj.PICName.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                    </tr>

                                        <tr>
		                                    <td>Deadline</td>
                                            <td>" + obj.Deadline.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                    </tr>
                                        
                                        <tr>
		                                    <td>Update Comment</td>
                                            <td>" + obj.UpdateComment.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                    </tr>

                                        <tr>
		                                    <td>Note</td>
                                            <td>" + obj.Note.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                    </tr>
                                     
                                        <tr>
		                                    <td>Status</td>
                                            <td>" + obj.StatusName + @"</td>
	                                    </tr>
                                        
                                    </table></br>

                                    Thanks and regards,</br>
                                    WMS System.

                                    ";
                message.Body = bodyContent;
                // -----------------------------------------------------------

                foreach (var managerUser in obj.ManagerIds.Split(';').Where(t => !string.IsNullOrEmpty(t)))
                {
                    var userObj = this.userService.GetByID(Convert.ToInt32(managerUser));
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

                foreach (var picUser in obj.PICIds.Split(';').Where(t => !string.IsNullOrEmpty(t)))
                {
                    var userObj = this.userService.GetByID(Convert.ToInt32(picUser));
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