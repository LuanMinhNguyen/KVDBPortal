// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Security;
    using EDMs.Business.Services.Library;
    using EDMs.Web.Utilities.Sessions;
    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class SendMail : Page
    {
        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly ConrespondenceService documentService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly CategoryService categoryService;


        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public SendMail()
        {
            this.documentService = new  ConrespondenceService();
            this.userService = new UserService();
            this.categoryService = new CategoryService();
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

                if (!string.IsNullOrEmpty(this.Request.QueryString["Doc"]))
                {
                    var categoryId = 0;
                    var count = 0;
                    var listDocId =  Convert.ToInt32( this.Request.QueryString["Doc"]);
                    var objDoc = this.documentService.GetById(listDocId);
                    var bodyContent = @"<font size=3 >Dear All,<br/>
                                Please be informed. <br/><br/>
                               M-Number       :<b> #M-Number# </b> <br/>
                               Issue Date     :<b>  #IssueDate# </b> <br/>
                               Reference Docs :<b>  #Reference# </b> <br/>
                               Answer Request :<b>  #Answer# </b> <br/> 
                               Document Title :<b>  #Title# </b> <br/>
                               Discipline     :<b>  #Discipline# </b> <br/>
                               Reply          :<b>  #Reply# </b> <br/>
                               From           :<b>  #FROM#  </b><br/>
                               Cos type       :<b>  #TYPE#  </b><br/>
                               Remark         :<b>  #Remark# </b> <br/> 
                                </br>
                               &nbsp;Click on the this link to access the PEDMS system&nbsp;:&nbsp; <a href='" + ConfigurationSettings.AppSettings.Get("WebAddress")
                                           + "/Controls/Document/CorrespondenceList.aspx?DocNo=" + objDoc.DocumentNumber + "'>" + ConfigurationSettings.AppSettings.Get("WebAddress") + @"</a>
                                    </br>
                         &nbsp;&nbsp;&nbsp;  EDMS WORKFLOW NOTIFICATION <br/>
                        [THIS IS SYSTEM GENERATED NOTIFICATION PLEASE DO NOT REPLY]
                            </font>
                                ";
                    this.txtEmailBody.Content = bodyContent.Replace("#M-Number#", objDoc.DocumentNumber)
                        .Replace("#IssueDate#", objDoc.IssueDate!= null ? objDoc.IssueDate.GetValueOrDefault().ToString("dd/MM/yyyy"):string.Empty)
                        .Replace("#Reference#", objDoc.ReferenceDocs)
                        .Replace("#Answer#", objDoc.AnswerRequestDate != null ? objDoc.AnswerRequestDate.GetValueOrDefault().ToString("dd/MM/yyyy") : string.Empty)
                        .Replace("#Title#", objDoc.Title)
                        .Replace("#Discipline#", objDoc.DisciplineName)
                        .Replace("#Reply#", objDoc.Reply)
                        .Replace("#FROM#", objDoc.FromName)
                        .Replace("#TYPE#", objDoc.DocumentTypeName)
                        .Replace("#Remark#", objDoc.Remark);
                    this.txtSubject.Text = "Correspondence New: "+ objDoc.DocumentNumber;

                    var lead = objDoc.LeaderId.Split(',').Where(t => !string.IsNullOrEmpty(t.Trim())).Select(t => Convert.ToInt32(t)).ToList();
                    var userlist = this.userService.GetAll().Where(t => lead.Contains(t.Id) && !string.IsNullOrEmpty(t.Email)).Select(t=> t.Email).ToList();
                    foreach (RadComboBoxItem item in this.ddlEmail.Items)
                    {
                        if (userlist.Contains(item.Text))
                        {
                            item.Checked = true;
                        }
                    }

                    var infor = objDoc.UserInforId.Split(',').Where(t => !string.IsNullOrEmpty(t.Trim())).Select(t => Convert.ToInt32(t)).ToList();
                     userlist = this.userService.GetAll().Where(t => infor.Contains(t.Id) && !string.IsNullOrEmpty(t.Email)).Select(t => t.Email).ToList();
                    foreach (RadComboBoxItem item in this.ddlEmailCC.Items)
                    {
                        if (userlist.Contains(item.Text))
                        {
                            item.Checked = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData()
        {
            var listUser = this.userService.GetAll().Where(t => !string.IsNullOrEmpty(t.Email)).Select(t=> t.Email).Distinct().OrderBy(t => t);
            this.ddlEmail.DataSource = listUser;
            this.ddlEmail.DataBind();

            this.ddlEmailCC.DataSource = listUser;
            this.ddlEmailCC.DataBind();
        }

        protected void SendMailMenu_OnButtonClick(object sender, RadToolBarEventArgs e)
        {
            if (this.IsValid)
            {
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
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "PEDMS System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;
                message.Subject = this.txtSubject.Text;
                message.Body = this.txtEmailBody.Content;

                if (!string.IsNullOrEmpty(this.ddlEmail.Text))
                {
                    var toList = this.ddlEmail.Text.Split(',').Where(t => !string.IsNullOrEmpty(t));
                    var ccList = this.ddlEmailCC.Text.Split(',').Where(t => !string.IsNullOrEmpty(t));

                    foreach (var to in toList)
                    {
                        message.To.Add(new MailAddress(to));
                    }

                    foreach (var cc in ccList)
                    {
                        message.CC.Add(new MailAddress(cc));
                    }

                    smtpClient.Send(message);
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