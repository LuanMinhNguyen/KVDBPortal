// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.WMS
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class SendMail : Page
    {
        private readonly UserService userService;
        private readonly ObjectAssignedUserService objAssignedUserService;
        private readonly IntergrateParamConfigService configService;

        public SendMail()
        {
            this.userService = new UserService();
            this.configService = new IntergrateParamConfigService();
            this.objAssignedUserService = new ObjectAssignedUserService();
        }

        /// <summary>
        /// Validation existing patient code
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        protected  void ValidatePatientCode(object source, ServerValidateEventArgs arguments)
        {
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
                this.LoadComboData();
                this.ddlEmail.Focus();
                if (!string.IsNullOrEmpty(this.Request.QueryString["currentAssignId"]))
                {
                    var currentWorkAssignedUserId = new Guid(this.Request.QueryString["currentAssignId"]);
                    var currentWorkAssignedUser = this.objAssignedUserService.GetById(currentWorkAssignedUserId);
                    if (currentWorkAssignedUser != null)
                    {
                        var assignUserObj = this.userService.GetByID(currentWorkAssignedUser.UserID.GetValueOrDefault());
                        this.txtSubject.Text = currentWorkAssignedUser.ObjectType + " \"" + currentWorkAssignedUser.ObjectNumber + "\" being in workflow process";
                        // Generate email body
                        var bodyContent = @"Dear All,<br/><br/>
                                Please be informed that the new update for " + currentWorkAssignedUser.ObjectType + " \"" + currentWorkAssignedUser.ObjectNumber + @""":<br/>
                                <table border='1' cellspacing='0'>
	                                <tr>
		                                <td style=""width: 200px;"">Current Workflow</td>
                                        <td style=""width: 500px;"">" + currentWorkAssignedUser.WorkflowName + @"</td>
	                                </tr>
                                    <tr>
		                                <td>Current Workflow Step</td>
                                        <td>" + currentWorkAssignedUser.CurrentWorkflowStepName + @"</td>
	                                </tr>
                                    <tr>
		                                <td>Title</td>
                                        <td>" + currentWorkAssignedUser.ObjectTitle + @"</td>
	                                </tr>

                                    <tr>
		                                <td>Assign To User</td>
                                        <td>" + assignUserObj.FullNameWithDeptPosition + @"</td>
	                                </tr>
                                    <tr>
		                                <td>Received Date</td>
                                        <td>" + currentWorkAssignedUser.ReceivedDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") + @"</td>
	                                </tr>
                                    <tr>
		                                <td>Updated Date</td>
                                        <td>" + currentWorkAssignedUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") + @"</td>
	                                </tr>
                                    
                                </table></br>

                                Thanks and regards,</br>
                                WMS System.

                                ";
                        this.txtEmailBody.Content = bodyContent;
                    }
                }
            }
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData()
        {
            var listUser = this.userService.GetAll().Where(t => !string.IsNullOrEmpty(t.Email)).OrderBy(t => t.Email);
            this.ddlEmail.DataSource = listUser;
            this.ddlEmail.DataBind();

            this.ddlEmailCC.DataSource = listUser;
            this.ddlEmailCC.DataBind();
        }

        protected void SendMailMenu_OnButtonClick(object sender, RadToolBarEventArgs e)
        {
            if (this.IsValid)
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

                    message.Subject = this.txtSubject.Text.Trim();
                    message.Body = this.txtEmailBody.Content;

                    if (!string.IsNullOrEmpty(this.ddlEmail.Text))
                    {
                        var toList = this.ddlEmail.Text.Split(';').Where(t => !string.IsNullOrEmpty(t));
                        foreach (var to in toList)
                        {
                            message.To.Add(new MailAddress(to));
                        }

                        if (!string.IsNullOrEmpty(this.ddlEmailCC.Text))
                        {
                            var ccList = this.ddlEmailCC.Text.Split(';').Where(t => !string.IsNullOrEmpty(t));
                            foreach (var cc in ccList)
                            {
                                message.CC.Add(new MailAddress(cc));
                            }
                        }

                        try
                        {
                            smtpClient.Send(message);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "Close();", true);
            }
        }

        /// <summary>
        /// The server validation file name is exist.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void ServerValidationEmptyEmailAddress(object source, ServerValidateEventArgs args)
        {
            if (this.ddlEmail.Text.Trim().Length == 0)
            {
                this.selectEmailValidate.ErrorMessage = "Please enter email address.";
                args.IsValid = false;
            }
        }
    }
}