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
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	using System.IO;
	using System.Linq;
	using System.Web.Hosting;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using Aspose.Cells;
	using EDMs.Business.Services.Document;
	using EDMs.Business.Services.Library;
	using EDMs.Business.Services.Security;
	using EDMs.Business.Services.Workflow;
	using EDMs.Data.Entities;
	using EDMs.Web.Utilities.Sessions;
	using Telerik.Web.UI;
	using System.Net;
	using System.Net.Mail;
	using System.Text;
	using System.Text.RegularExpressions;

	/// <summary>
	/// The customer edit form.
	/// </summary>
	public partial class ContractorTransmittalAttachDocFileReject : Page
	{
		private readonly ContractorTransmittalService contractorTransmittalService;

		private readonly ContractorTransmittalDocFileService contractorTransmittalDocFileService;

		private readonly PECC2TransmittalService pecc2TransmittalService;

		private readonly  UserService userService = new UserService();
		private readonly ProjectCodeService projectCodeService = new ProjectCodeService();
		private readonly RoleService roleService = new RoleService();
        private readonly DocumentCodeServices documentCodeServices;

        private readonly CustomizeReceivedEmailService ReceivedEmailService = new CustomizeReceivedEmailService();
        private readonly DistributionMatrixService matrixService = new DistributionMatrixService();
        private readonly DistributionMatrixDetailService matrixDetailService = new DistributionMatrixDetailService();

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ContractorTransmittalAttachDocFileReject()
		{
			this.contractorTransmittalService = new ContractorTransmittalService();
			this.contractorTransmittalDocFileService = new ContractorTransmittalDocFileService();
			this.pecc2TransmittalService = new PECC2TransmittalService();
            this.documentCodeServices = new DocumentCodeServices();

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
				if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
				{
					//var obj = this.contractorTransmittalDocFileService.GetById(new Guid(this.Request.QueryString["objId"]));
					//if (obj != null)
					//{
					//    this.txtReason.Text = obj.RejectReason;
					//}
				}
			}
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
				if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
				{
					var objId = new Guid(this.Request.QueryString["objId"]);
					var pecc2Trans = this.pecc2TransmittalService.GetById(objId);

					//var obj = this.contractorTransmittalDocFileService.GetById(objId);
					if (pecc2Trans != null)
					{
						pecc2Trans.Status = "Rejected";
						pecc2Trans.ErrorMessage = "Transmittal Rejected - " + this.txtReason.Text.Trim();
						pecc2Trans.IsReject = true;
						pecc2Trans.CurrentRejectReason = "Transmittal Rejected - " + this.txtReason.Text.Trim();

						this.pecc2TransmittalService.Update(pecc2Trans);

						var contractorTrans = this.contractorTransmittalService.GetById(pecc2Trans.ContractorTransId.GetValueOrDefault());
						if (contractorTrans != null)
						{
							contractorTrans.Status = "Document File Rejected";
							contractorTrans.ErrorMessage = "Transmittal Rejected - " + this.txtReason.Text.Trim();
							contractorTrans.IsReject = true;
							contractorTrans.IsSend = false;

							this.contractorTransmittalService.Update(contractorTrans);
							if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]))
							{
								this.NotifiNewTransmittal(contractorTrans);
							}
						}
					}
				}

				this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
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

		private void NotifiNewTransmittal(ContractorTransmittal transmittal)
		{
			try
            {
                var customObj = this.ReceivedEmailService.GetByType(1, 1);
                if (transmittal != null && customObj != null)
                {

                    var userListid = this.userService.GetListUser(customObj.ToUserIDs.Split(';').Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)).ToList());
                    var projctobj = this.projectCodeService.GetById(transmittal.ProjectId.GetValueOrDefault());
					var pecc2obj = this.pecc2TransmittalService.GetById(transmittal.PECC2TransId.GetValueOrDefault());

					var smtpClient = new SmtpClient
					{
						DeliveryMethod = SmtpDeliveryMethod.Network,
						UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
						EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
						Host = ConfigurationManager.AppSettings["Host"],
						Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
						Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
					};
					int count = 0;
					var containtable = string.Empty;

					var subject = "FYI: Transmittal rejected, " + transmittal.TransNo + ", " + transmittal.TransDate.GetValueOrDefault().ToString("dd/MM/yyyy") + ", " + transmittal.PurposeName.Split(',')[1] + ", " + transmittal.Description;

					var message = new MailMessage();
					message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
					message.Subject = subject;
					message.BodyEncoding = new UTF8Encoding();
					message.IsBodyHtml = true;
                    var toemail = string.Empty;
                    var Userlist = userListid.Where(t => !string.IsNullOrEmpty(t.Email)).Distinct().ToList();
                    foreach (var user in Userlist)
                    {
                        try
                        {
                            if (user.Email.Contains(";"))
                            {
                                foreach (string stemail in user.Email.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList())
                                {
                                    message.To.Add(new MailAddress(stemail));
                                    toemail += stemail + "; ";
                                }
                            }
                            else
                            {
                                message.To.Add(new MailAddress(user.Email));
                                toemail += user.Email + "; ";
                            }
                           
                        }
                        catch { }
                    }
                    var infoUserIds = customObj.CCUserIDs != null
                       ? customObj.CCUserIDs.Split(';').ToList()
                       : new List<string>();
                    var matrixList =
                      this.matrixService.GetAllByList(customObj.DistributionMatrixCCIDs.Split(';')
                           .Where(t => !string.IsNullOrEmpty(t))
                           .Select(t => Convert.ToInt32(t)).ToList());
                    foreach (var matrix in matrixList)
                    {
                        var matrixDetailList = this.matrixDetailService.GetAllByDM(matrix.ID).Where(t => t.GroupCodeId == transmittal.GroupId);
                        infoUserIds.AddRange(matrixDetailList.Select(t => t.UserId.ToString()));
                    }

                    var emailCC = string.Empty;
                    var UsserCC = this.userService.GetListUser(infoUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)).ToList());
                    var listCC = UsserCC.Where(t => !string.IsNullOrEmpty(t.Email)).Distinct().ToList();
                    foreach (var user in listCC)
                    {
                        try
                        {
                            if (user.Email.Contains(";"))
                            {
                                foreach (string stemail in user.Email.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList())
                                {
                                    message.CC.Add(new MailAddress(stemail));
                                    emailCC += stemail + "; ";
                                }
                            }
                            else
                            {
                                message.CC.Add(new MailAddress(user.Email));
                                emailCC += user.Email + "; ";
                            }
                           
                        }
                        catch { }

                    }

                    var bodyContent = @"<head><title></title><style>
							body {font-family:Calibri;font-size:10px;}
                hr {color:#2C4E9C;background-color:#2C4E9C; height:3px;}
                .msg {font-size:16px;}                        
                table {width:98.0%;border-collapse:collapse;margin-left:20px;color:black;background-color:white;border:1px solid #ACCEF5;padding:3px;font-size:16px;}
                td {border:1px solid #ACCEF5;}
                .span1 {font-size:16px;}
                .ch1 {background-color:#F7FAFF;padding:10px;font-weight:bold;color:#2C4E9C;}
                .ch2 {background-color:#F7FAFF;padding:5px;}
                a {color:mediumblue;}
                .system {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                .company {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                .link {font-size:16px;margin-left:30px;}
                .footer {color:darkgray; font-size:12px;}
                /*TYPE OF NOTIFICATION PURPOSE*/
                .action {background-color:#fffda5;}
                .info {background-color:#d1fcbd;}
                .overdue {background-color:#f00;color:white;font-weight:bold;}
                  .header_ {width:50.0%;border:none;border-bottom:solid #98C6EA 1.0pt;mso-border-bottom-alt:solid #98C6EA .75pt;background:#D4EFFC;padding:3.75pt 3.75pt 3.75pt 3.75pt}
                  .footer_ {border:none;border-top:solid #98C6EA 1.0pt;mso-border-top-alt:solid #98C6EA .75pt;background:#D4EFFC;padding:6.0pt 6.0pt 6.0pt 6.0pt}
                  .font_l {font-size:13.5pt;font-family:'Verdana',sans-serif}
                .font_m {font-size:10.0pt;font-family:'Verdana',sans-serif}
                .font_s {font-size:9.0pt;font-family:'Verdana',sans-serif}
                .font_xs {font-size:7.5pt;font-family:'Verdana',sans-serif}
							</style></head>
					<body>
                    <table border='1'>
                  <tr>
                                <td width='50%' class='header_'>
								<b><span class='font_m'>" + projctobj.Description + @"</span></b><br>				
								<b><span class='font_xs' style='color:red'>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"</span></b>
							</td>
							<td width='50%' class='header_'>
								<p class='MsoNormal' align='right' style='text-align:right'>
									<b><span class='font_l' style='color:#000066'>EVN</span></b>
									<em><b><span class='font_l' style='color:red'>PECC2</span></b></em>
								</p>
							</td>
                    </tr>
                  
                  <tr><td colspan='2' > 
                    <p align='center' style='margin-bottom:12.0pt;text-align:center'>
							<span class='font_m'>
								<br><b>Transmittal Rejected:</b> Transmittal <b>" + transmittal.TransNo + @"</b> has been rejected
							</span>
						</p>
						<div align='center' style='border:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'>
                    <table border='1'>
                <tr>
                    <td class='ch2' style='width:167px' ><span class='font_s' style='color:#003399'>From</span> </td><td colspan='3'><span class='font_s' style='color:#003399'>" + UserSession.Current.User.FullNameWithPosition + @"</span></td>
                   
                </tr>
                <tr>
                     <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>To</span></td><td colspan='3'><span class='font_s' style='color:#003399'>
								<a href='mailto:" + toemail + "'>" + toemail + @"</a>
							</span></td>
                   
                </tr>
                 <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>CC</span></td><td colspan='3'><span class='font_s' style='color:#003399'>
								<a href='mailto:" + emailCC + "'>" + emailCC + @"</a>
							</span></td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>Transmittal No.</span></td><td colspan='3' class='font_s' style='color:red'>" + transmittal.TransNo + @"</td>
                   
                </tr>
                      <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>Transmittal Title</span></td><td class='font_s' colspan='3'>" + transmittal.Description + @"</td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>Issued Date</span></td><td class='font_s'>" + transmittal.TransDate.GetValueOrDefault().ToString("dd/MM/yyyy") + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Action Code</span></td><td class='font_s' style='color:red'>" + this.documentCodeServices.GetById(transmittal.PurposeId.GetValueOrDefault()).Description + @"</td>
                  </tr>
                   <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>Reason</span></td><td class='font_s' colspan='3' style='color:red'>" + this.txtReason.Text.Trim() + @"</td>
                  </tr>
                  </table>
                  </div>";

				   
					var st = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/Document/PECC2TransmittalList.aspx?TransNoContractor=" + transmittal.TransNo;
					var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/Document/PECC2TransmittalList.aspx";
					bodyContent += @"<p style='margin-bottom:12.0pt'>
			            <span class='font_m'>
				            <u><b>Useful Links:</b></u>
				            <ul class='font_m'>
					            <li>
								Click <a href='" + st + @"'>here</a> to show <u>this transmittal</u> in DMDC System
							</li>
							<li>
								Click <a href= '" + st1 + @"' > here</a> to show <u>all transmittals</u> in DMDC System
							</li>
						</ul>
					   </span>
						</p>			
						<p  align='center' style='margin-bottom:12.0pt'>
						<span class='font_m'>[THIS IS SYSTEM AUTO-GENERATED NOTIFICATION. PLEASE DO NOT REPLY.]
						</span>
						</p>
						</td>
						</tr>
						<tr>
							<td class='footer_'>
								<b><span class='font_xs'>Power Engineering Consulting Joint Stock Company 2 (PECC2)</span></b>
							</td>
							<td class='footer_'>
								<p  align=right style='text-align:right'>
									<b><span class='font_xs'>32 Ngo Thoi Nhiem,Ward 7, District 3, Ho Chi Minh City<br>Tel: (84 8) 22.211.057 - Fax: (84 8) 22.210.408 - Email: <a href='mailto:info@pecc2.com'>info@pecc2.com</a> 
									</span></b>
								</p>
							</td>
						</tr>
					</table></body>";
					message.Body = bodyContent;
					smtpClient.Send(message);
				}
			}
			catch { }
		}
	}
}