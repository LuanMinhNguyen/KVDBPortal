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
using EDMs.Business.Services.Library;
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
    public partial class TrackingProcedureEditForm : Page
    {
        private readonly TrackingProcedureService trackingProcedureService;

        private readonly ScopeProjectService projectService;
        private readonly NumberManagementService numberManagementService;
        private readonly UserService userService;

        private readonly IntergrateParamConfigService configService;

        private readonly SharePermissionService sharePermissionService;

        private readonly ProcedureStageService procedureStageService;
        private readonly ProcedureTypeService procedureTypeService;
        private readonly RoleService roleService;

        private readonly FunctionPermissionService fncPermissionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MREditForm"/> class.
        /// </summary>
        public TrackingProcedureEditForm()
        {
            this.trackingProcedureService = new TrackingProcedureService();
            this.projectService = new ScopeProjectService();
            this.numberManagementService = new NumberManagementService();
            this.userService = new UserService();
            this.configService = new IntergrateParamConfigService();
            this.sharePermissionService = new SharePermissionService();
            this.procedureStageService = new ProcedureStageService();
            this.procedureTypeService = new ProcedureTypeService();
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

                    var obj = this.trackingProcedureService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if(obj!=null)
                    {
                        this.txtFacility.Text = obj.ProjectName;

                        this.txtSystemName.Text = obj.SystemName;
                        this.txtOldCode.Text = obj.OldCode;
                        this.txtNewCode.Text = obj.NewCode;
                        this.txtProcedureName.Text = obj.ProcedureName;
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

                        foreach (RadTreeNode deptNode in this.rtvChecker.Nodes)
                        {
                            foreach (RadTreeNode userNode in deptNode.Nodes)
                            {
                                if (!string.IsNullOrEmpty(obj.CheckerIds) && obj.CheckerIds.Split(';').Contains(userNode.Value))
                                {
                                    userNode.Checked = true;
                                }
                            }
                        }

                        this.ddlStage.SelectedValue = obj.TargerStageId.GetValueOrDefault().ToString();
                        this.ddlType.SelectedValue = obj.TypeId.GetValueOrDefault().ToString();
                        this.txtStartDate.Text = obj.StartDate;
                        this.txtCompleteDate.Text = obj.CompleteDate;
                        this.txtPage.Value = obj.TotalPage;
                        this.ddlLevel.SelectedValue = obj.DifficultLvl;
                        this.txtOfficeManday.Value = obj.OfficeManday;
                        this.txtOffshoreManday.Value = obj.OffshoreManDay;
                        this.ddlCreateType.SelectedValue = obj.CreateType;
                        this.txtPercent.Value = obj.PercentComplete;
                        this.ddlStatus.SelectedValue = obj.Status;
                        this.txtDeadline.Text = obj.Deadline;
                        this.ddlUpdateInAmos.SelectedValue = obj.UpdatedInAMOS;
                        this.txtRemark.Text = obj.Remark;
                        this.txtLevel.Text = obj.LevelName;

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

                    var index = this.numberManagementService.GetByName("TrackingProcedure", projectId);
                    var objCode = "P-" + index.NextCount.GetValueOrDefault();

                    this.txtCode.Text = objCode;
                }
            }
        }

        private void GetFuncPermissionConfig()
        {
            // Get Function permission of user for MR Module
            var fncPermission = this.fncPermissionService.GetOne(UserSession.Current.User.Id, 12);
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
            var procedureTypeList = this.procedureTypeService.GetAll();
            this.ddlType.DataSource = procedureTypeList;
            this.ddlType.DataTextField = "Name";
            this.ddlType.DataValueField = "Id";
            this.ddlType.DataBind();

            var stageList = this.procedureStageService.GetAll();
            this.ddlStage.DataSource = stageList;
            this.ddlStage.DataTextField = "FullName";
            this.ddlStage.DataValueField = "Id";
            this.ddlStage.DataBind();

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

                this.rtvChecker.Nodes.Add(detpNode);

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

                var obj = new TrackingProcedure();
                obj.ID = Guid.NewGuid();

                obj.ProjectId = projectObj.ID;
                obj.ProjectName = projectObj.Name;

                obj.SystemName = this.txtSystemName.Text;
                obj.OldCode = this.txtOldCode.Text;
                obj.NewCode = this.txtNewCode.Text;

                obj.ProcedureName = this.txtProcedureName.Text;
                obj.PICIds = string.Empty;
                obj.PICName = string.Empty;
                foreach (RadTreeNode pic in this.rtvPIC.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                {
                    obj.PICIds += pic.Value + ";";
                    obj.PICName += pic.Text + Environment.NewLine;
                }

                obj.CheckerIds = string.Empty;
                obj.Checker = string.Empty;
                foreach (RadTreeNode checker in this.rtvChecker.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                {
                    obj.CheckerIds += checker.Value + ";";
                    obj.Checker += checker.Text + Environment.NewLine;
                }
                obj.TargerStageId = this.ddlStage.SelectedItem != null ? Convert.ToInt32(this.ddlStage.SelectedValue) : 0;
                obj.TargerStage = this.ddlStage.SelectedItem != null ? this.ddlStage.SelectedItem.Text : String.Empty;
                obj.TypeId = this.ddlType.SelectedItem != null ? Convert.ToInt32(this.ddlType.SelectedValue) : 0;
                obj.TypeName = this.ddlType.SelectedItem != null ? this.ddlType.SelectedItem.Text : String.Empty;
                obj.StartDate = this.txtStartDate.Text;
                obj.CompleteDate = this.txtCompleteDate.Text;
                obj.TotalPage = (int?)this.txtPage.Value;
                obj.DifficultLvl = this.ddlLevel.SelectedValue;
                obj.OfficeManday = this.txtOfficeManday.Value;
                obj.OffshoreManDay = this.txtOffshoreManday.Value;
                obj.CreateType = this.ddlCreateType.SelectedValue;
                obj.PercentComplete = this.txtPercent.Value;
                obj.Status = this.ddlStatus.SelectedValue;
                obj.Deadline = this.txtDeadline.Text;
                obj.UpdatedInAMOS = this.ddlUpdateInAmos.SelectedValue;
                obj.Remark = this.txtRemark.Text;
                obj.LevelName = this.txtLevel.Text;

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
                    var objOld = this.trackingProcedureService.GetById(objId);
                    if (objOld != null)
                    {
                        objOld.IsLeaf = false;
                        this.trackingProcedureService.Update(objOld);

                        obj.ParentId = objOld.ParentId ?? objOld.ID;
                    }
                }
                else
                {
                    var index = this.numberManagementService.GetByName("TrackingProcedure", projectId);
                    index.NextCount += 1;
                    this.numberManagementService.Update(index);
                }

                var objGUID = this.trackingProcedureService.Insert(obj);

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

        private void SendNotification(TrackingProcedure obj)
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

                message.Subject = "New Procedure Update: " + obj.Code;

                // Generate email body
                var bodyContent =
                @"Dear All,<br/><br/>
                Please be informed that the new Procedure Update with information:<br/>
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
		                <td>System Name</td>
                        <td>" + obj.SystemName.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>Old Code</td>
                        <td>" + obj.OldCode.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>New Code</td>
                        <td>" + obj.NewCode.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>Procedure Name</td>
                        <td>" + obj.ProcedureName.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>PIC</td>
                        <td>" + obj.PICName.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr> 
                    <tr>
		                <td>Checker</td>
                        <td>" + obj.Checker.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>     
                    
                    <tr>
		                <td>Target for Each Stage</td>
                        <td>" + obj.TargerStage + @"</td>
	                </tr>
                    
                    <tr>
		                <td>Tentative Start Date</td>
                        <td>" + obj.StartDate + @"</td>
	                </tr>
                    <tr>
		                <td>Tentative Complete Date</td>
                        <td>" + obj.CompleteDate + @"</td>
	                </tr>
                    <tr>
		                <td>Estimate Total Page</td>
                        <td>" + obj.TotalPage + @"</td>
	                </tr><tr>
		                <td>Difficult Level</td>
                        <td>" + obj.DifficultLvl + @"</td>
	                </tr><tr>
		                <td>Office Manday (day)</td>
                        <td>" + obj.TotalPage + @"</td>
	                </tr><tr>
		                <td>Estimate Total Page</td>
                        <td>" + obj.OfficeManday + @"</td>
	                </tr><tr>
		                <td>Offshore Manday (day)</td>
                        <td>" + obj.OffshoreManDay + @"</td>
	                </tr><tr>
		                <td>New Procedure or Revise</td>
                        <td>" + obj.CreateType + @"</td>
	                </tr>
                                     
                    <tr>
		                <td>Open/ Closed</td>
                        <td>" + obj.Status + @"</td>
	                </tr>
                    <tr>
		                <td>Deadline to Issue</td>
                        <td>" + obj.Deadline + @"</td>
	                </tr>
                    <tr>
		                <td>Updated in AMOS</td>
                        <td>" + obj.UpdatedInAMOS + @"</td>
	                </tr>
                    <tr>
		                <td>Remark</td>
                        <td>" + obj.Remark.Replace(Environment.NewLine, "<br/>") + @"</td>
	                </tr>
                    <tr>
		                <td>Level</td>
                        <td>" + obj.LevelName + @"</td>
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
                        var shareUserList = this.sharePermissionService.GetByFromUserAndObj(userObj.Id, "9");
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

                foreach (var checkerId in obj.CheckerIds.Split(';').Where(t => !string.IsNullOrEmpty(t)))
                {
                    var userObj = this.userService.GetByID(Convert.ToInt32(checkerId));
                    if (userObj != null)
                    {
                        var shareUserList = this.sharePermissionService.GetByFromUserAndObj(userObj.Id, "9");
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