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
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.WMS
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class TrackingMOCEditForm : Page
    {
        private readonly TrackingMOCService trackingMocService;

        private readonly ScopeProjectService projectService;

        private readonly NumberManagementService numberManagementService = new NumberManagementService();

        private readonly UserService userService;

        private readonly IntergrateParamConfigService configService;

        private readonly SharePermissionService sharePermissionService;

        private readonly FunctionPermissionService fncPermissionService;

        private readonly RoleService roleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MREditForm"/> class.
        /// </summary>
        public TrackingMOCEditForm()
        {
            this.trackingMocService = new TrackingMOCService();
            this.projectService = new ScopeProjectService();
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

                    var obj = this.trackingMocService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if(obj!=null)
                    {
                        this.txtFacility.Text = obj.ProjectName;
                        this.txtName.Text = obj.SystemName;
                        this.txtDescription.Text = obj.DescriptionOfChange;
                        this.txtDateIssue.SelectedDate = obj.DateIssued;
                        this.txtReason.Text = obj.ReasonOfChange;
                        this.txtInitRisk.Text = obj.InitRisk;
                        this.ddlInitRiskLvl.SelectedValue = obj.InitRiskLvlId.ToString();
                        this.txtMitigationAction.Text = obj.MigrationAction;
                        this.ddlMitigationRiskLvl.SelectedValue = obj.MigrationRiskLvlId.ToString();
                        this.txtAcceptableDate.Text = obj.DateAccepted;
                        this.txtTechAuth.Text = obj.TechAuthority;

                        foreach (RadTreeNode deptNode in this.rtvPIC.Nodes)
                        {
                            foreach (RadTreeNode userNode in deptNode.Nodes)
                            {
                                if (!string.IsNullOrEmpty(obj.PICId) && obj.PICId.Split(';').Contains(userNode.Value))
                                {
                                    userNode.Checked = true;
                                }
                            }
                        }

                        this.txtRemark.Text = obj.Remark;
                        this.txtClosedClari.Text = obj.ClosedClarification;
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
                    var projectObj = this.projectService.GetById(projectId);
                    this.txtFacility.Text = projectObj.Name;
                    this.CreatedInfo.Visible = false;

                    var index = this.numberManagementService.GetByName("TrackingMOC", projectId);
                    var objCode = "MOC-" + index.NextCount.GetValueOrDefault();

                    this.txtCode.Text = objCode;
                }
            }
        }

        private void GetFuncPermissionConfig()
        {
            // Get Function permission of user for MR Module
            var fncPermission = this.fncPermissionService.GetOne(UserSession.Current.User.Id, 4);
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
        protected void  btnSave_Click(object sender, EventArgs e)
        {
            if (this.Page.IsValid)
            {
                var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
                var projectObj = this.projectService.GetById(projectId);
                var obj = new TrackingMOC();
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var objId = this.Request.QueryString["objId"];
                    obj = trackingMocService.GetById(new Guid(objId));
                    if (obj != null)
                    {
                        obj.UpdatedByName = UserSession.Current.User.FullName;
                        obj.UpdatedBy = UserSession.Current.User.Id;
                        obj.UpdatedDate = DateTime.Now;
                        this.CollectData(obj);

                        this.trackingMocService.Update(obj);
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

                    var index = this.numberManagementService.GetByName("TrackingMOC", projectId);
                    index.NextCount += 1;
                    this.numberManagementService.Update(index);

                    var objGuid = this.trackingMocService.Insert(obj);

                    if (objGuid != null)
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
                            // ignored
                        }
                    }
                }
                
                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
        }

        private void CollectData(TrackingMOC obj)
        {
            obj.SystemName = this.txtName.Text;
            obj.DescriptionOfChange = this.txtDescription.Text;
            obj.DateIssued = this.txtDateIssue.SelectedDate;
            obj.ReasonOfChange = this.txtReason.Text;
            obj.InitRisk = this.txtInitRisk.Text;
            obj.InitRiskLvlId = Convert.ToInt32(this.ddlInitRiskLvl.SelectedValue);
            obj.InitRiskLvlName = this.ddlInitRiskLvl.SelectedItem.Text;
            obj.MigrationAction = this.txtMitigationAction.Text;
            obj.MigrationRiskLvlId = Convert.ToInt32(this.ddlMitigationRiskLvl.SelectedValue);
            obj.MigrationRiskLvlName = this.ddlMitigationRiskLvl.SelectedItem.Text;
            obj.DateAccepted = this.txtAcceptableDate.Text;
            obj.TechAuthority = this.txtTechAuth.Text;
            obj.Remark = this.txtRemark.Text;
            obj.ClosedClarification = this.txtClosedClari.Text;
            obj.StatusId = Convert.ToInt32(this.ddlStatus.SelectedValue);
            obj.StatusName = this.ddlStatus.SelectedItem.Text;

            obj.PICId = string.Empty;
            obj.PICName = string.Empty;
            foreach (RadTreeNode pic in this.rtvPIC.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
            {
                obj.PICId += pic.Value + ";";
                obj.PICName += pic.Text + Environment.NewLine;
            }

            //obj.IsComplete = this.cbComplete.Checked;
            //obj.IsLeaf = true;
            obj.Code = this.txtCode.Text;
        }

        private void SendNotification(TrackingMOC obj)
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

                message.Subject = "New MOC Update: " + obj.Code;

                // Generate email body
                var bodyContent =
                @"Dear All,<br/><br/>
                Please be informed that the new MOC Update with information:<br/>
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
		                <td>Name of equiment /system</td>
                        <td>" + obj.SystemName.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>Description of change</td>
                        <td>" + obj.DescriptionOfChange.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>

                    <tr>
		                <td>Date issues</td>
                        <td>" + (obj.DateIssued != null ? obj.DateIssued.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") : string.Empty) + @"</td>
	                </tr>
                    <tr>
		                <td>Reason of change</td>
                        <td>" + obj.ReasonOfChange.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>Initial Risk Evaluation</td>
                        <td>" + obj.InitRisk.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>Initial Risk level</td>
                        <td>" + obj.InitRiskLvlName + @"</td>
	                </tr>
                    <tr>
		                <td>Mitigation action</td>
                        <td>" + obj.MigrationAction.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>Risk Level after mitigation</td>
                        <td>" + obj.MigrationRiskLvlName + @"</td>
	                </tr>
                    <tr>
		                <td>Latest Acceptable Date</td>
                        <td>" + obj.DateAccepted + @"</td>
	                </tr>
                    <tr>
		                <td>Technical authority/Dept</td>
                        <td>" + obj.TechAuthority + @"</td>
	                </tr>
                    <tr>
		                <td>PIC</td>
                        <td>" + obj.PICName.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>                    
                    <tr>
		                <td>Status</td>
                        <td>" + obj.StatusName + @"</td>
	                </tr>
                    <tr>
		                <td>Remark</td>
                        <td>" + obj.Remark.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                </table></br>

                Thanks and regards,</br>
                WMS System.

                ";
                message.Body = bodyContent;
                // -----------------------------------------------------------

                foreach (var picId in obj.PICId.Split(';').Where(t => !string.IsNullOrEmpty(t)))
                {
                    var userObj = this.userService.GetByID(Convert.ToInt32(picId));
                    if (userObj != null)
                    {
                        var shareUserList = this.sharePermissionService.GetByFromUserAndObj(userObj.Id, "3");
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