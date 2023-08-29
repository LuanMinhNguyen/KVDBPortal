// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Data;
using System.Collections;
using System.Linq;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Web.UI;
    using Aspose.Cells;
    using Business.Services.Document;
    using Business.Services.Library;
    using Business.Services.Security;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class RFIDetailToDoList : Page
    {
        private readonly RFIService rfiService;
        private readonly RFIDetailService rfiDetailService;
        private readonly UserService userService;
        private readonly ProjectCodeService projectCodeService;
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public RFIDetailToDoList()
        {
            this.rfiService = new RFIService();
            this.rfiDetailService = new RFIDetailService();
            this.userService = new UserService();
            this.projectCodeService = new ProjectCodeService();
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
                this.ActionType.Value = this.Request.QueryString["actionType"];
                this.lbobjId.Value= this.Request.QueryString["objId"];

            
                this.LbcurrentAssignId.Value= this.Request.QueryString["currentAssignId"];
                this.divApplyCodeForAllDoc.Visible = this.Request.QueryString["actionType"] != "2";
                GridColumn rowStateColumn = grdDocumentFile.MasterTableView.GetColumn("EditColumn");
                if(rowStateColumn != null)
                {
                    rowStateColumn.Visible = this.Request.QueryString["actionType"] != "2";
                }
                var listuser = this.userService.GetAll().Where(t => !t.Role.IsAdmin.GetValueOrDefault() && t.Role.IsInternal.GetValueOrDefault()== true && t.RoleId==UserSession.Current.User.RoleId).ToList();
                listuser.Insert(0, new User() { Id = 0, FullName = string.Empty });
                this.ddlengineer.DataSource = listuser.OrderBy(t => t.FullName);
                this.ddlengineer.DataValueField = "Id";
                this.ddlengineer.DataTextField = "FullName";
                this.ddlengineer.DataBind();
            }
        }

        protected void grdDocumentFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var changeRequestId = new Guid(this.Request.QueryString["objId"]);
                this.grdDocumentFile.DataSource = this.rfiDetailService.GetByRFI(changeRequestId).OrderBy(t=> t.Number);
            }
        }

        protected void ajaxDocument_OnAjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument.Contains("SelectEngineer"))
            {
                var listid = e.Argument.Split('_')[1].Split(',').ToList();
                List<RFIDetail> ListRFI = new List<RFIDetail>();
                foreach(var item in listid.Where(t=> !string.IsNullOrEmpty(t)))
                {
                    var rfidetailobj = this.rfiDetailService.GetById(new Guid(item));
                    if(rfidetailobj != null)
                    {
                        ListRFI.Add(rfidetailobj);
                        rfidetailobj.EngineeringActionID = Convert.ToInt32(this.ddlengineer.SelectedValue);
                        rfidetailobj.EngineeringActionName = this.ddlengineer.SelectedItem.Text;
                        this.rfiDetailService.Update(rfidetailobj);
                    }
                }
                if (ListRFI.Count > 0)
                {
                    var userobj = this.userService.GetByID(Convert.ToInt32(this.ddlengineer.SelectedValue));
                    if(Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) && userobj!= null && !string.IsNullOrEmpty(userobj.Email))
                    {
                        var Id = new Guid(this.Request.QueryString["objId"]);

                        SendNotificationRFI(this.rfiService.GetById(Id), userobj, ListRFI);
                    }
                }
                this.ddlengineer.SelectedIndex = 0;
                this.grdDocumentFile.Rebind();
            }
            else if(e.Argument.Contains("ExportRFI"))
            {
                var rfiId = new Guid(this.Request.QueryString["objId"]);
                var rfiObj = this.rfiService.GetById(rfiId);
                ExportCMDRReport(rfiObj);
            }
        }
        private void ExportCMDRReport(RFI rfiObj)
        {
            var filePath = Server.MapPath("~/Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\Pecc2_Form_RFI_action.xlsm");

            var dataSheet = workbook.Worksheets[0];
            var dtFull = new DataTable();
            var projectObj = this.projectCodeService.GetById(rfiObj.ProjectId.GetValueOrDefault());

            dataSheet.Cells["E1"].PutValue(projectObj.FullName);
            dataSheet.Cells["C3"].PutValue(rfiObj.IssuedDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            dataSheet.Cells["I3"].PutValue(rfiObj.Number);
            dataSheet.Cells["D10"].PutValue(rfiObj.SiteManager);
            dataSheet.Cells["F10"].PutValue(rfiObj.QAQCManager);

            var filename = projectObj.Code + "_" + "RFI Cover_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsm";
            dtFull.Columns.AddRange(new[]
            {

                new DataColumn("NoIndex", typeof (String)),
                new DataColumn("Discipline", typeof (String)),
                new DataColumn("WorkTitle", typeof (String)),
                new DataColumn("Description", typeof (String)),
                new DataColumn("Localtion", typeof (String)),
                new DataColumn("Time", typeof (String)),
                new DataColumn("Type", typeof (String)),
                new DataColumn("Contractor", typeof (String)),
                new DataColumn("Remark", typeof (String)),
                 new DataColumn("Action", typeof (String)),
            });


            var rfidetailList = this.rfiDetailService.GetByRFI(rfiObj.ID);
            var disciplineRowCount = 1;
            foreach (var detailObj in rfidetailList)
            {

                var dataRow = dtFull.NewRow();
                dataRow["NoIndex"] = disciplineRowCount;
                dataRow["Discipline"] = detailObj.GroupName;
                dataRow["WorkTitle"] = detailObj.WorkTitle;
                dataRow["Description"] = detailObj.Description;
                dataRow["Localtion"] = detailObj.Location;
                dataRow["Time"] = detailObj.Time.GetValueOrDefault().ToString("dd/MM/yyyy HH:MM");
                dataRow["Type"] = detailObj.InspectionTypeName;
                dataRow["Contractor"] = detailObj.ContractorContact;
                dataRow["Remark"] = detailObj.Remark;
                dataRow["Action"] = detailObj.EngineeringActionName;
                disciplineRowCount += 1;
                dtFull.Rows.Add(dataRow);

            }
            dataSheet.Cells.ImportDataTable(dtFull, false, 4, 1, dtFull.Rows.Count, dtFull.Columns.Count, true);
            var validations = dataSheet.Validations;
            dataSheet.Cells["A7"].PutValue(dtFull.Rows.Count + 7);
            workbook.Save(filePath + filename);
            this.Download_File(filePath + filename);
        }
        private void Download_File(string FilePath)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
        }
        private void SendNotificationRFI(RFI RFIObj, User assignUserObj, List<RFIDetail> Listdoc)
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

                var message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;
                int count = 0;
                message.Subject = "FYA: Assigned RFI, "+ DateTime.Now.ToString("dd/MM/yyyy")+@", "+ RFIObj.Number;
                // Generate email body
                var bodyContent = @"<head><title></title><style>
                            body {font-family:Calibri;font-size:10px;}
                            hr {color:#2C4E9C;background-color:#2C4E9C; height:3px;}
                            .msg {font-size:16px;}
                            table {border-collapse:collapse;margin-left:20px;color:black;background-color:white;border:1px solid #ACCEF5;padding:3px;font-size:16px;}
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
                            </style></head>
                            <body>
                           <h2 style='font-family:'Arial Rounded MT Bold';'><u>Workflow Notification</u></h2>
                            <span class='msg'>Dear All,
                            <br /><br />Please be informed about new RFI assigned:
                            </span>
                            <br /><br />
                            <table border='1'>
                            <tr><td colspan='8' class='ch1'>RFI General</td></tr>
                            <tr>
                                <td class='ch2'>RFI Number</td><td class='ch2'>:</td>
                                <td colspan='5'>"+ RFIObj .Number+ @"</td>
                            </tr>
                            <tr>
                                <td class='ch2'>Date</td><td class='ch2'>:</td>
                                <td colspan='5'>"+ DateTime.Now.ToString("dd/MM/yyyy HH:mm") + @"</td>
                            </tr>
                            <tr><td colspan='8' class='ch1'>RFI Details</td></tr>
                            <tr>
                                <td colspan='2' class='ch2'>Group</td>
                                <td class='ch2'>Work Title</td>
                                <td class='ch2'>Location</td>
                                <td class='ch2'>Time</td>
                                <td class='ch2'>Inspection type</td>
                                <td class='ch2'>Contractor's contact</td>
                                <td class='ch2'>Remark</td>
                                 <td class='ch2'>Action By</td>
                            </tr>";

                foreach (var document in Listdoc)
                {
                    bodyContent += @"<tr>
                                <td colspan='2'>"+ document.GroupName + @"</td>
                                <td>" + document.WorkTitle + @" </td>
                                <td>" + document.Location + @" </td>
                                <td>" + document.Time?.ToString("dd/MM/yyyy HH:mm") + @" </td>
                                <td>" + document.InspectionTypeName + @" </td>
                                <td>" + document.ContractorContact + @" </td>
                                <td>" + document.Remark + @" </td>
                                <td>" + document.EngineeringActionName + @" </td>
                            </tr> ";
                }
           
                var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/RFIList.aspx?RFINo="+RFIObj.Number;
            
                bodyContent += @"</table>
                            <div class='link'>
                            <br />
                            <u><b>Useful Links:</b></u>
                            <ul>
                                <li>
                                    Click <a href='" + st1 + @"'>here</a> to show <u>all your tasks</u> in DMDC System
                                </li>
                            </ul>
                            </div>
                            <br />
                            <h2 class='company'>Power Engineering Consulting JSC 2 (PECC2)</h2>
                            <hr />
                            <h3 class='system'>DMDC System</h3>
                            <br />
                            <span class='footer'>[THIS IS SYSTEM AUTO-GENERATED NOTIFICATION]</span>
                            </body>";
                message.Body = bodyContent;
                if (!string.IsNullOrEmpty(assignUserObj.Email))
                {
                    message.To.Add(assignUserObj.Email);

                    smtpClient.Send(message);
                }
            }
            catch (Exception ex) { }
        }
    }
}