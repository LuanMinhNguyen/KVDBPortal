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
    public partial class TrackingECREditForm : Page
    {
        private readonly TrackingECRService trackingECRService;

        private readonly ScopeProjectService projectService;

        private readonly NumberManagementService numberManagementService;

        private readonly UserService userService;

        private readonly IntergrateParamConfigService configService;

        private readonly SharePermissionService sharePermissionService;

        private readonly PriorityLevelService priorityLevelService;

        private readonly FunctionPermissionService fncPermissionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MREditForm"/> class.
        /// </summary>
        public TrackingECREditForm()
        {
            this.trackingECRService = new TrackingECRService();
            this.projectService = new ScopeProjectService();
            this.numberManagementService = new NumberManagementService();
            this.userService = new UserService();
            this.configService = new IntergrateParamConfigService();
            this.sharePermissionService = new SharePermissionService();
            this.priorityLevelService = new PriorityLevelService();
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

                    var obj = this.trackingECRService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if(obj!=null)
                    {
                        this.txtFacility.Text = obj.ProjectName;
                        this.txtDateRaised.SelectedDate = obj.DateRaised;
                        this.txtTitle.Text = obj.Title;
                        this.txtDescription.Text = obj.Description;
                        this.ddlApSection1.SelectedValue = obj.Section1Id.ToString();
                        this.ddlApSection2.SelectedValue = obj.Section2Id.ToString();
                        this.ddlApSection3.SelectedValue = obj.Section3Id.ToString();
                        this.ddlApSection4.SelectedValue = obj.Section4Id.ToString();
                        this.ddlApSection5.SelectedValue = obj.Section5Id.ToString();
                        this.ddlSection3.SelectedValue = obj.ApSection3Id.ToString();
                        this.ddlRequirement.SelectedValue = obj.ApRequirementId.ToString();
                        this.ddlPriorityLvl.SelectedValue = obj.PriorityId.ToString();
                        this.txtExecutionStatus.Text = obj.ExecutionStatus;

                        foreach (RadListBoxItem item in this.lbPIC.Items)
                        {
                            item.Checked = !string.IsNullOrEmpty(obj.PersonInChargeIds) && obj.PersonInChargeIds.Split(';').Contains(item.Value);
                        }

                        this.txtRemark.Text = obj.Remark;
                        this.ddlStatus.SelectedValue = obj.StatusId.ToString();
                        this.txtCost.Value = obj.Cost;
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

                    var index = this.numberManagementService.GetByName("TrackingECR", projectId);
                    var objCode = "ECR-" + index.NextCount.GetValueOrDefault();

                    this.txtCode.Text = objCode;
                }
            }
        }

        private void GetFuncPermissionConfig()
        {
            // Get Function permission of user for MR Module
            var fncPermission = this.fncPermissionService.GetOne(UserSession.Current.User.Id, 3);
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
            var priorityLvl = this.priorityLevelService.GetAll();
            priorityLvl.Insert(0, new PriorityLevel() {ID = 0});
            this.ddlPriorityLvl.DataSource = priorityLvl;
            this.ddlPriorityLvl.DataTextField = "Name";
            this.ddlPriorityLvl.DataValueField = "ID";
            this.ddlPriorityLvl.DataBind();

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
                var configObj = configService.GetById(1);

                var obj = new TrackingECR();
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    obj.UpdatedByName = UserSession.Current.User.FullName;
                    obj.UpdatedBy = UserSession.Current.User.Id;
                    obj.UpdatedDate = DateTime.Now;
                    this.CollectData(obj);

                    this.trackingECRService.Update(obj);
                    try
                    {
                        if (configObj != null && configObj.IsEnableSendEmailNotification.GetValueOrDefault())
                        {
                            this.SendNotification(obj, configObj);
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

                    var index = this.numberManagementService.GetByName("TrackingECR", projectId);
                    index.NextCount += 1;
                    this.numberManagementService.Update(index);

                    var objGUID = this.trackingECRService.Insert(obj);

                    if (objGUID != null)
                    {
                        try
                        {
                            if (configObj != null && configObj.IsEnableSendEmailNotification.GetValueOrDefault())
                            {
                                this.SendNotification(obj, configObj);
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

        private void CollectData(TrackingECR obj)
        {
            obj.Code = this.txtCode.Text.Trim();

            obj.DateRaised = this.txtDateRaised.SelectedDate;
            obj.Title = this.txtTitle.Text;
            obj.Description = this.txtDescription.Text;

            obj.Section1Id = Convert.ToInt32(this.ddlApSection1.SelectedValue);
            obj.Section1Name = this.ddlApSection1.SelectedItem.Text;
            obj.Section2Id = Convert.ToInt32(this.ddlApSection2.SelectedValue);
            obj.Section2Name = this.ddlApSection2.SelectedItem.Text;
            obj.Section3Id = Convert.ToInt32(this.ddlApSection3.SelectedValue);
            obj.Section3Name = this.ddlApSection3.SelectedItem.Text;
            obj.Section4Id = Convert.ToInt32(this.ddlApSection4.SelectedValue);
            obj.Section4Name = this.ddlApSection4.SelectedItem.Text;
            obj.Section5Id = Convert.ToInt32(this.ddlApSection5.SelectedValue);
            obj.Section5Name = this.ddlApSection5.SelectedItem.Text;

            obj.ApSection3Id = Convert.ToInt32(this.ddlSection3.SelectedValue);
            obj.ApSection3Name = this.ddlSection3.SelectedItem.Text;
            obj.ApRequirementId = Convert.ToInt32(this.ddlRequirement.SelectedValue);
            obj.ApRequirementName = this.ddlRequirement.SelectedItem.Text;

            obj.ExecutionStatus = this.txtExecutionStatus.Text;

            obj.PersonInCharge = string.Empty;
            obj.PersonInChargeIds = string.Empty;
            foreach (RadListBoxItem item in this.lbPIC.CheckedItems)
            {
                obj.PersonInChargeIds += item.Value + ";";
                obj.PersonInCharge += item.Text + Environment.NewLine;
            }

            obj.Cost = this.txtCost.Value;
            obj.Remark = this.txtRemark.Text;
            obj.StatusId = Convert.ToInt32(this.ddlStatus.SelectedValue);
            obj.StatusName = this.ddlStatus.SelectedItem.Text;
            obj.PriorityId = Convert.ToInt32(this.ddlPriorityLvl.SelectedValue);
            obj.PriorityName = this.ddlPriorityLvl.SelectedItem.Text;
        }

        private void SendNotification(TrackingECR obj, IntergrateParamConfig configObj)
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

            message.Subject = "New ECR Update: " + obj.Code;

            // Generate email body
            var bodyContent = @"Dear All,<br/><br/>
                                Please be informed that the new ECR Update with information:<br/>
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
		                                <td>ECR Title</td>
                                        <td>" + obj.Title.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                </tr>
                                    <tr>
		                                <td>ECR Description</td>
                                        <td>" + obj.Description.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                </tr>

                                    <tr>
		                                <td>Date Raised</td>
                                        <td>" + (obj.DateRaised != null ? obj.DateRaised.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") : string.Empty) + @"</td>
	                                </tr>
                                    <tr>
		                                <td>APPROVAL STATUS: SECTION 1 </td>
                                        <td>" + obj.Section1Name + @"</td>
	                                </tr>
                                    <tr>
		                                <td>APPROVAL STATUS: SECTION 2 </td>
                                        <td>" + obj.Section2Name + @"</td>
	                                </tr>
                                    <tr>
		                                <td>APPROVAL STATUS: SECTION 3 </td>
                                        <td>" + obj.Section3Name + @"</td>
	                                </tr>
                                    <tr>
		                                <td>APPROVAL STATUS: SECTION 4 </td>
                                        <td>" + obj.Section4Name + @"</td>
	                                </tr>
                                    <tr>
		                                <td>APPROVAL STATUS: SECTION 5 </td>
                                        <td>" + obj.Section5Name + @"</td>
	                                </tr>
                                    <tr>
		                                <td>Action Plan: SECTION 3 </td>
                                        <td>" + obj.ApSection3Name + @"</td>
	                                </tr>
                                    <tr>
		                                <td>Action Plan: Requirements </td>
                                        <td>" + obj.ApRequirementName + @"</td>
	                                </tr>
                                    <tr>
		                                <td>EXECUTION STATUS</td>
                                        <td>" + obj.ExecutionStatus.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                </tr>
                                    <tr>
		                                <td>PERSON IN CHARGE</td>
                                        <td>" + obj.PersonInCharge.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                </tr>
                                    <tr>
		                                <td>ECR Current Status</td>
                                        <td>" + obj.StatusName + @"</td>
	                                </tr>
                                    <tr>
		                                <td>Cost</td>
                                        <td>" + obj.Cost + @"</td>
	                                </tr>
                                    <tr>
		                                <td>Remarks</td>
                                        <td>" + obj.Remark.Replace(Environment.NewLine, "<br/>") + @"</td>
	                                </tr>
                                </table></br>

                                Thanks and regards,</br>
                                WMS System.

                                ";
            message.Body = bodyContent;
            // -----------------------------------------------------------

            foreach (var picId in obj.PersonInChargeIds.Split(';').Where(t => !string.IsNullOrEmpty(t)))
            {
                var userObj = this.userService.GetByID(Convert.ToInt32(picId));
                if (userObj != null)
                {
                    var shareUserList = this.sharePermissionService.GetByFromUserAndObj(userObj.Id, "2");
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