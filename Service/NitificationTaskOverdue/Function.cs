
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;


namespace NitificationTaskOverdue
{
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Net.Mail;
    using System.IO;
    using System.Net;
    using System.Data;
    using System.Web;
    using System.Data.SqlClient;
    using System.ServiceProcess;
    public  class Function
    {


        private readonly UserService userService;
        private readonly ObjectAssignedUserService objectAssgnedUserService;
        private readonly ConrespondenceService documentService;
        private readonly HolidayConfigService holidayConfigService;
        private readonly ProjectCodeService projectCodeService;
        private readonly HashSet<DateTime> Holidays = new HashSet<DateTime>();
        public Function()
        {
            this.userService = new UserService();
            this.objectAssgnedUserService = new ObjectAssignedUserService();
            this.documentService = new ConrespondenceService();
            this.holidayConfigService = new HolidayConfigService();
            this.projectCodeService = new ProjectCodeService();

            var holidayList = this.holidayConfigService.GetAll();
            foreach (var holidayConfig in holidayList)
            {
                for (DateTime i = holidayConfig.FromDate.GetValueOrDefault(); i < holidayConfig.ToDate.GetValueOrDefault(); i = i.AddDays(1))
                {
                    this.Holidays.Add(i);
                }
            }
        }
        public void GetAllTaskOverdue()
        {
            var listTask = this.objectAssgnedUserService.GetAllOverDueTask();
            var listUser = listTask.Select(t => t.UserID.GetValueOrDefault()).Distinct().ToList();
            foreach(var obj in listUser)
            {
              if(!this.Holidays.Contains(DateTime.Now))  this.SendNotification(listTask.Where(t => t.UserID == obj).ToList(), this.userService.GetByID(obj));
            }
        }


        private void SendNotification(List<ObjectAssignedUser> assignWorkingUser, User assignUserObj)
        {
            try
            {
                // Implement send mail function
                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    Host = ConfigurationManager.AppSettings["Host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                };

                var projctobj = this.projectCodeService.GetById(assignWorkingUser.FirstOrDefault().ObjectProjectId.GetValueOrDefault());
                var message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "EDMS System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;

                message.Subject = "[DMDC " + projctobj.Code + "] OVERDU TASK";


                var bodyContent = @"<head><title></title><style>
							body {tab-interval:36.0pt;}
                            .frame_o {width:98.0%;mso-cellspacing:0cm;border:solid #98C6EA 1.0pt;mso-border-alt:solid #98C6EA .75pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm}
                            .frame_i {width:100.0%;border-collapse:collapse;mso-yfti-tbllook:1184}
                            .font_l {font-size:13.5pt;font-family:'Verdana',sans-serif}
                            .font_m {font-size:10.0pt;font-family:'Verdana',sans-serif}
                            .font_s {font-size:9.0pt;font-family:'Verdana',sans-serif}
                            .font_xs {font-size:7.5pt;font-family:'Verdana',sans-serif}
                            .header_ {width:50.0%;border:none;border-bottom:solid #98C6EA 1.0pt;mso-border-bottom-alt:solid #98C6EA .75pt;background:#D4EFFC;padding:3.75pt 3.75pt 3.75pt 3.75pt}
                            .footer_ {border:none;border-top:solid #98C6EA 1.0pt;mso-border-top-alt:solid #98C6EA .75pt;background:#D4EFFC;padding:6.0pt 6.0pt 6.0pt 6.0pt}
                            .propertyname_ {nowrap;valign=top;border:solid #E8EDFF 1.0pt;border-top:none;mso-border-top-alt:solid #E8EDFF .75pt;mso-border-alt:solid #E8EDFF .75pt;background:#E3F4FD;padding:3.75pt 7.5pt 3.75pt 7.5pt}
                            .propertyvalue_ {border:solid #E8EDFF 1.0pt;mso-border-left-alt:solid #E8EDFF .75pt;mso-border-alt:solid #E8EDFF .75pt;padding:3.75pt 7.5pt 3.75pt 7.5pt}
							</style></head>
							<body>
							<div align=center>
                            <table  border=1 cellspacing=0 cellpadding=0 class='frame_o'>
                            <tr>
		                            <td width='50%' class='header_'>
			                          <b><span class='font_m'>" + projctobj.Description + @"</span></b><br>				
								<b><span class='font_xs' style='color:red'>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"</span></b>
		                            </td>
		                            <td width='50%' class='header_'>
			                            <p class=MsoNormal align=right style='text-align:right'>
				                            <b><span class='font_l' style='color:#000066'>EVN</span></b>
				                            <em><b><span class='font_l' style='color:red'>PECC2</span></b></em>
			                            </p>
		                            </td>
	                            </tr>
	                            <tr >
		                            <td colspan=2 style='border:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'>
                                        Dear " + (assignUserObj.FullName.Contains(" - ") ? assignUserObj.FullName.Split('-')[0] : assignUserObj.FullName) + @",
							<br /><br />
			                            <p align=center style='margin-bottom:12.0pt;text-align:center'>
				                            <span class='font_m' style='color:red'><br><b>Overdue</b></span>
				                            <span class='font_m'><b>To-do-list :</b>You has <b>" + assignWorkingUser.Count+ @"</b> overdue task on DMDC System until today. Please action.</span>
			                            </p>
			                            <div align=center>
				                            <table  border=0 cellspacing=0 cellpadding=0 width='100%' class='frame_i'>
                                <tr>
						<td colspan='4' class='propertyvalue_'>
							<span class='font_s' style='color:#003399'><b> Details</b></span>							
						</td>						
					</tr>		
					<tr>
						<td class='propertyname_'>
							<span class='font_s' style='color:#003399'>No.</span>							
						</td>	
						<td  class='propertyname_'>
							<span class='font_s' style='color:#003399'>Object Number</span>							
						</td>
                        <td  class='propertyname_'>
							<span class='font_s' style='color:#003399'>Object Title</span>							
						</td>
                        <td  class='propertyname_'>
							<span class='font_s' style='color:#003399'>Object Type</span>							
						</td>
                        <td  class='propertyname_'>
							<span class='font_s' style='color:#003399'Received Date</span>							
						</td>
                        <td  class='propertyname_'>
							<span class='font_s' style='color:#003399'>Deadline</span>							
						</td>
                       							
					</tr>	";
                
                var count = 0;

                foreach (var document in assignWorkingUser)
                {

                    count += 1;

                    bodyContent += @"<tr>
                                               <td class='propertyvalue_'>
							<span class='font_s' style='color:#003399'>" + count + @"</span>		
						</td>
                                               <td class='propertyvalue_'>
							<span class='font_s' style='color:#003399'>" + document.ObjectNumber + @"</span></td>
                                               <td class='propertyvalue_'>
							<span class='font_s' style='color:#003399'>"
                                   + document.ObjectTitle + @"</span></td>
                                               <td class='propertyvalue_'>
							<span class='font_s' style='color:#003399'>"
                                   + document.ObjectType + @"</span></td>
                                               <td class='propertyvalue_'>
							<span class='font_s' style='color:#003399'>"
                                   + document.ReceivedDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") + @"</span></td>
                                        <td class='propertyvalue_'>
							<span class='font_s' style='color:#003399'>"
                                   + document.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") + @"</span></td></tr>";

                }

                var st = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx";
                bodyContent += @"</table>
			</div>
			<p style='margin-bottom:12.0pt'>
			<span class='font_m'>
							<u><b>Useful Links:</b></u>
							<ul class='font_m'>
							<li>
								Click <a href='" + st + @"'>here</a> to show <u>all your tasks</u> in DMDC System
							</li>
						</ul>
					   </span>
						</p>			
						<p  align=center style='margin-bottom:12.0pt'>
						<span class='font_m'>[THIS IS SYSTEM AUTO-GENERATED NOTIFICATION. PLEASE DO NOT REPLY.]
						</span>
						</p>
						</td>
						</tr>
						<tr style='mso-yfti-irow:2;mso-yfti-lastrow:yes'>
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
                if (!string.IsNullOrEmpty(assignUserObj.Email))
                {
                    message.To.Add(assignUserObj.Email);
                }

                //foreach (var userObj in this.userService.GetAllByDC())
                //{

                //    if (!string.IsNullOrEmpty(userObj.Email)) message.CC.Add(userObj.Email);

                //}
                message.Bcc.Add(ConfigurationManager.AppSettings["EmailAccount"]);
                smtpClient.Send(message);
                TextWriter file = new StreamWriter(ConfigurationManager.AppSettings["File"], true);
                file.WriteLine("Sent email to " + assignUserObj.Email + " .Time: " + DateTime.Now);
                file.Close();
            }
            catch { }
        }
    }
}
