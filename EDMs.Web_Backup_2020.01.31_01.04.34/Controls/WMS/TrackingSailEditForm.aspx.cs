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
    public partial class TrackingSailEditForm : Page
    {
        private readonly TrackingSailService TrackingSailService;

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
        public TrackingSailEditForm()
        {
            this.TrackingSailService = new TrackingSailService();
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

                    var obj = this.TrackingSailService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if(obj!=null)
                    {
                        this.txtFacility.Text = obj.ProjectName;

                        this.txtDate.SelectedDate = obj.DateRaised;
                        this.ddlSource.SelectedValue = obj.SourceName;
                        this.txtName.Text = obj.NameObserver;
                        this.txtLocation.Text = obj.Location;
                        this.txtDescription.Text = obj.Description;
                        this.txtAction.Text = obj.Action;
                        this.txtProposedAction.Text = obj.ProposedAction;
                        this.ddlPriority.SelectedValue = obj.Priority;
                        this.txtTargetClose.Text = obj.TargetClose;
                        this.txtActionClose.Text = obj.ActionTakeClose;
                        this.txtCloseDate.Text = obj.ClosedDate;
                        this.txtHOCTrackingNo.Text = obj.HOCTrackingNo;
                        this.txtMSRStatus.Text = obj.MSRStatus;
                        this.txtWRNo.Text = obj.WRNo;
                        this.txtMOCNo.Text = obj.MOCNo;
                        this.txtECRNo.Text = obj.ECRNo;

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

                    var index = this.numberManagementService.GetByName("TrackingSail", projectId);
                    var objCode = "SL-" + index.NextCount.GetValueOrDefault();

                    this.txtCode.Text = objCode;
                }
            }
        }

        private void GetFuncPermissionConfig()
        {
            // Get Function permission of user for MR Module
            var fncPermission = this.fncPermissionService.GetOne(UserSession.Current.User.Id, 11);
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

                var obj = new TrackingSail();
                obj.ID = Guid.NewGuid();

                obj.ProjectId = projectObj.ID;
                obj.ProjectName = projectObj.Name;

                obj.DateRaised = this.txtDate.SelectedDate;
                obj.SourceName = this.ddlSource.SelectedValue;
                obj.NameObserver = this.txtName.Text;
                obj.Location = this.txtLocation.Text;
                obj.Description = this.txtDescription.Text;
                obj.Action = this.txtAction.Text;
                obj.ProposedAction = this.txtProposedAction.Text;
                obj.Priority = this.ddlPriority.SelectedValue;
                obj.TargetClose = this.txtTargetClose.Text;
                obj.ActionTakeClose = this.txtActionClose.Text;
                obj.ClosedDate = this.txtCloseDate.Text;
                obj.HOCTrackingNo = this.txtHOCTrackingNo.Text;
                obj.MSRStatus = this.txtMSRStatus.Text;

                obj.WRNo = this.txtWRNo.Text;
                obj.MOCNo = this.txtMOCNo.Text;
                obj.ECRNo = this.txtECRNo.Text;

                obj.PICIds = string.Empty;
                obj.PICName = string.Empty;
                foreach (RadTreeNode pic in this.rtvPIC.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                {
                    obj.PICIds += pic.Value + ";";
                    obj.PICName += pic.Text + Environment.NewLine;
                }

                obj.IsComplete = this.cbComplete.Checked;
                obj.IsLeaf = true;
                obj.Code = this.txtCode.Text;

                obj.CreatedByName = UserSession.Current.User.FullName;
                obj.CreatedBy = UserSession.Current.User.Id;
                obj.CreatedDate = DateTime.Now;

                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var objId = new Guid(this.Request.QueryString["objId"]);
                    var objOld = this.TrackingSailService.GetById(objId);
                    if (objOld != null)
                    {
                        objOld.IsLeaf = false;
                        this.TrackingSailService.Update(objOld);

                        obj.ParentId = objOld.ParentId ?? objOld.ID;
                    }
                }
                else
                {
                    var index = this.numberManagementService.GetByName("TrackingSail", projectId);
                    index.NextCount += 1;
                    this.numberManagementService.Update(index);
                }

                var objGUID = this.TrackingSailService.Insert(obj);
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

        private void SendNotification(TrackingSail obj)
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

                message.Subject = "New Sail Update: " + obj.Code;

                // Generate email body
                var bodyContent =
                @"Dear All,<br/><br/>
                Please be informed that the new Sail Update with information:<br/>
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
		                <td>Date</td>
                        <td>" + (obj.DateRaised != null ? obj.DateRaised.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") : string.Empty) + @"</td>
	                </tr>
                    <tr>
		                <td>Source</td>
                        <td>" + obj.SourceName + @"</td>
	                </tr><tr>
		                <td>Name Observer</td>
                        <td>" + obj.NameObserver + @"</td>
	                </tr><tr>
		                <td>Location</td>
                        <td>" + obj.Location + @"</td>
	                </tr>
                    <tr>
		                <td>Description</td>
                        <td>" + obj.Description.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>Immediate Action Taken</td>
                        <td>" + obj.Action.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    
                    <tr>
		                <td>Proposed further action and close out</td>
                        <td>" + obj.ProposedAction.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>Priority</td>
                        <td>" + obj.Priority + @"</td>
	                </tr>
                    <tr>
		                <td>Target to close out</td>
                        <td>" + obj.TargetClose + @"</td>
	                </tr>
                    <tr>
		                <td>Action taken to close out</td>
                        <td>" + obj.ActionTakeClose.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>  <tr>
		                <td>PIC</td>
                        <td>" + obj.PICName.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>  
                    <tr>
		                <td>Status/ Closed  Date</td>
                        <td>" + obj.ClosedDate + @"</td>
	                </tr>                  
                    <tr>
		                <td>HOC Tracking No</td>
                        <td>" + obj.HOCTrackingNo + @"</td>
	                </tr>
                    <tr>
		                <td>MSR Status</td>
                        <td>" + obj.MSRStatus.Replace(Environment.NewLine, "<br/>") + @"</td>
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
                        var shareUserList = this.sharePermissionService.GetByFromUserAndObj(userObj.Id, "8");
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