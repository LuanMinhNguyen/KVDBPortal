using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;
using Aspose.Cells;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using EAM.Business.Services;

namespace EAM.Notification
{
    public partial class EmailNotification : ServiceBase
    {
        private System.Timers.Timer timerSync;
        private bool flag1 = false;
        private bool flag2 = false;

        private List<string> bccList = ConfigurationManager.AppSettings["BCC"].Split(',')
            .Where(t => !string.IsNullOrEmpty(t)).ToList();

        private bool isTest = ConfigurationManager.AppSettings["IsTesting"] == "true";

        public EmailNotification()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                //timer run each minute
                timerSync = new System.Timers.Timer(60 * 1000);
                timerSync.Enabled = true;
                timerSync.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Fired);
                timerSync.AutoReset = true;
                timerSync.Start();


                this.EventLog.WriteEntry("EAM Notification service started");

            }
            catch (Exception ex)
            {
                this.EventLog.WriteEntry("Have Error: " + ex.Message);
            }

        }

        protected override void OnStop()
        {
            timerSync.Dispose();
            this.EventLog.WriteEntry("EAM Notification service stoped");
        }

        private void Timer_Fired(object sender, EventArgs e)
        {
            try
            {
                timerSync.Stop();
                // Send monthly Part Compare Report
                SendMonthlyReport();

                // Send email notification fow Work
                this.SendTotalEmailNotification();
            }
            catch (Exception ex)
            {
                this.EventLog.WriteEntry("Have Error: " + ex.Message);
            }
            finally
            {
                timerSync.Start();
            }
        }

        private void SendTotalEmailNotification()
        {
            var dbService = new EAMStoreProcedureService();
            try
            {
                string selectCommand = "select * from r5daily where ngay = " + System.DateTime.Now.Day +
                                       " and thang = " + System.DateTime.Now.Month + " and nam = " +
                                       System.DateTime.Now.Year + "";
                var tbReport = dbService.GetDataTable(selectCommand);
                if (tbReport.Rows.Count == 0)
                {
                    string insertCommand = "insert into r5daily(ngay,thang,nam,issend30,issend24) values (" +
                                           System.DateTime.Now.Day + "," + System.DateTime.Now.Month + "," +
                                           System.DateTime.Now.Year + ",'False','False')";
                    dbService.ExcuteQuery(insertCommand);
                }

                this.TaoMoiYeuCau();
            }
            catch (Exception ex)
            {
                this.EventLog.WriteEntry("Have Error: " + ex.Message);
            }
        }

        private void TaoMoiYeuCau()
        {
            var dbService = new EAMStoreProcedureService();
            
            var tbReceiptions = dbService.GetDataTable("SELECT RECEIPTIONS FROM U5R5EMAIL WHERE STATUS ='NEW-WOS-BRKD'");
            var selectCommand =
                "SELECT A.EVT_CODE, OBJ_DESC, OBJ_CODE,EVT_DESC,EVT_UDFCHAR12 LOCATION, EVT_REPORTED NGAYYC, PER_DESC NGUOIYC , CONCAT(CONCAT(CONCAT(CONCAT(EVT_UDFCHAR01,char(10)), CONCAT(EVT_UDFCHAR02,CHAR(10))), CONCAT(CONCAT(EVT_UDFCHAR03,CHAR(10)), CONCAT(EVT_UDFCHAR04,CHAR(10)))), EVT_UDFCHAR05) NDYC FROM R5SENDEMAIL A INNER JOIN R5EVENTS B ON A.EVT_CODE COLLATE SQL_Latin1_General_CP1_CI_AS = B.EVT_CODE INNER JOIN R5OBJECTS C ON B.EVT_OBJECT = C.OBJ_CODE AND EVT_MRC = OBJ_MRC left outer join R5PERSONNEL on PER_CODE = EVT_ORIGIN WHERE CAST(ISSEND AS CHAR(1)) = '0' AND TOSTATUS = 'Q' and evt_org = 'TBMICCO'";
            var tbReport = dbService.GetDataTable(selectCommand);

            for (int i = 0; i < tbReport.Rows.Count; i++)
            {
                var emailSubjects = "Infor EAM - New work request on " + DateTime.Now.Month + "." +
                                       DateTime.Now.Year.ToString();
                var maYeuCau = tbReport.Rows[i]["EVT_CODE"].ToString();
                var dienGiai = tbReport.Rows[i]["EVT_DESC"].ToString();
                var tagname = tbReport.Rows[i]["OBJ_CODE"].ToString();
                var ngaySuCo = tbReport.Rows[i]["NGAYYC"].ToString();
                var nguoiYeuCau = tbReport.Rows[i]["NGUOIYC"].ToString();
                var viTriLapDat = tbReport.Rows[i]["LOCATION"].ToString();
                var noiDungYeuCau = tbReport.Rows[i]["NDYC"].ToString();
                //Khai báo Subjects

                //Lọc lấy danh sách email người cần nhận
                var emailReceiptions = new List<string>();
                if (tbReceiptions.Rows.Count > 0)
                {
                    emailReceiptions = tbReceiptions.Rows[0][0].ToString().Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList();
                }
                var emailBody =
                    "<table cellpadding='2' cellspacing='0' style='width:20cm; font-family: 'Microsoft Sans Serif'; font-size: small;'> <tr> <td style='border: 1px solid #99CCFF; height:0.8cm; text-align:center; color: #CC3300; font-weight: 700; background-color:#00CCFF; padding-left: 2mm;' valign='middle'; colspan='4'>HỆ THỐNG PHẦN MỀM QUẢN LÝ BẢO TRÌ, BẢO DƯỠNG THIẾT BỊ</td></tr> <tr> <td style='border: 1px solid #99CCFF; height:0.8cm; text-align:left; color: #0066FF; font-weight: 700; background-color:#FFFF99; padding-left: 2mm;' valign='middle'; colspan='4'>Thông tin yêu cầu sửa chữa</td></tr><tr><td style= 'border-width: 1px; border-color: #99CCFF; width:4cm; height:0.8cm; text-align:left; font-weight: bold; border-right-style: solid; border-left-style: solid; border-bottom-style: solid; ' valign= 'middle ';>Mã yêu cầu</td> <td style= 'border-width: 1px; border-color: #99CCFF; width:5cm; height:0.8cm; text-align:left; border-bottom-style: solid; ' valign= 'middle '>" +
                    maYeuCau +
                    "</td>   <td style= 'border-width: 1px; border-color: #99CCFF; width:3cm; height:0.8cm; text-align:left; font-weight: bold; border-right-style: solid; border-bottom-style: solid; border-left-style: solid; ' valign= 'middle '>Diễn giải</td> <td style= 'border-width: 1px; border-color: #99CCFF; width:8cm; height:0.8cm; text-align:left; border-bottom-style: solid; border-right-style: solid; ' valign= 'middle '> " +
                    dienGiai +
                    "</td>  </tr>   <tr><td style= 'border-width: 1px; border-color: #99CCFF; width:4cm; height:0.8cm; text-align:left; font-weight: bold; border-right-style: solid; border-left-style: solid; border-bottom-style: solid; ' valign= 'middle ';>Thiết bị</td> <td style= 'border-width: 1px; border-color: #99CCFF; width:5cm; height:0.8cm; text-align:left; border-bottom-style: solid; ' valign= 'middle '>" +
                    tagname +
                    "</td>   <td style= 'border-width: 1px; border-color: #99CCFF; width:3cm; height:0.8cm; text-align:left; font-weight: bold; border-right-style: solid; border-bottom-style: solid; border-left-style: solid; ' valign= 'middle '>Vị trí lắp đặt</td> <td style= 'border-width: 1px; border-color: #99CCFF; width:8cm; height:0.8cm; text-align:left; border-bottom-style: solid; border-right-style: solid; ' valign= 'middle '> " +
                    viTriLapDat +
                    "</td>  </tr>   <tr>   <td style= 'border-width: 1px; border-color: #99CCFF; width:4cm; height:0.8cm; text-align:left; font-weight: bold; border-right-style: solid; border-bottom-style: solid; border-left-style: solid; ' valign= 'middle ';>     Ngày yêu cầu</td><td style= 'border-width: 1px; border-color: #99CCFF; width:5cm; height:0.8cm; text-align:left; border-bottom-style: solid; ' valign= 'middle'> " +
                    ngaySuCo +
                    "</td><td style= 'border-width: 1px; border-color: #99CCFF; width:3cm; height:0.8cm; text-align:left; font-weight: bold; border-right-style: solid; border-bottom-style: solid; border-left-style: solid; ' valign= 'middle '>Người yêu cầu</td> <td style= 'border-width: 1px; border-color: #99CCFF; width:8cm; height:0.8cm; text-align:left; border-right-style: solid; border-bottom-style: solid; ' valign= 'middle '> " +
                    nguoiYeuCau +
                    "</td> </tr>   <tr>   <td style= 'border-left: 1px solid #99CCFF; border-right: 1px solid #99CCFF; border-bottom: 1px solid #99CCFF; height:0.8cm; text-align:left; font-weight: bold; border-top-color: #99CCFF; border-top-width: 1px;' valign= 'middle '; colspan='4'>Nội dung yêu cầu</td>     </tr>   <tr>   <td style= 'border-left: 1px solid #99CCFF; border-right: 1px solid #99CCFF; border-bottom: 1px solid #99CCFF; height:0.8cm; text-align:left; border-top-color: #99CCFF; border-top-width: 1px;' valign= 'middle '; colspan='4'>" +
                    noiDungYeuCau +
                    "</td> </tr>   <tr>   <td style= 'border-width: 1px; border-color: #99CCFF; text-align:left; font-weight: bold; border-right-style: solid; border-left-style: solid; ' valign= 'middle ';>Link hệ thống</td>    <td style= 'border-width: 1px; border-color: #99CCFF; height:0.8cm; text-align:left; border-right-style: solid;' valign= 'middle ' colspan='3'>http://portal.micco.com.vn:9090/web/base/logindisp?tenant=MICCO&FROMEMAIL=YES&SYSTEM_FUNCTION_NAME=WSJOBS&workordernum=" +
                    maYeuCau +
                    "&organization=TBMICCO</td>    </tr>   <tr>   <td style= 'border: 1px solid #99CCFF; height:1.5cm; text-align:left; background-color: #6699FF; color: White; padding-left: 2mm; font-style: italic;' valign='middle'; colspan='4'> <span>MICCO-lưu ý:</span><br /> Nội dung của email này chỉ được gửi và chỉ nhằm mục đích sử dụng cho đúng người nhận. Nếu bạn không đúng là người nhận liên quan thì email này đã được gửi tới bạn do sai sót. Bất kỳ tiết lộ, sao chép, phân phối hoặc hành động dựa trên các thông tin trong email là bị cấm và có thể là trái pháp luật. Nếu bạn vô tình nhận được email này, xin vui lòng liên hệ với người gửi và xóa email này trên máy tính của bạn</td>   </tr> </table>";

                this.SendEmail(isTest, emailSubjects, emailBody, emailReceiptions, new List<string>(), this.bccList, new List<string>());
                //Cập nhật lại nội dung của phiếu là đã được gửi
                bool isSend = true;
                dbService.ExcuteQuery("update r5sendemail set issend = '" + isSend + "' where evt_code = '" + maYeuCau +
                                      "' and tostatus ='Q'");
            }
        }

        private void SendMonthlyReport()
        {
            try
            {
                if (ConfigurationManager.AppSettings["EnableMonthlyReport"] == "true")
                {
                    //Workbook workbook = new Workbook("Schedule.xls");
                    Workbook workbook = new Workbook(ConfigurationManager.AppSettings["DataFile"]);
                    var monthlyData = workbook.Worksheets[0];
                    var weeklyData = workbook.Worksheets[1];

                    var totalDay = Convert.ToInt32(weeklyData.Cells["A1"].Value);

                    var currentMonth = Convert.ToInt32(monthlyData.Cells["A3"].Value);
                    var currentDay = Convert.ToInt32(monthlyData.Cells["A1"].Value);
                    var currentHour = Convert.ToInt32(monthlyData.Cells["B1"].Value);
                    var currentIsSend = Convert.ToBoolean(monthlyData.Cells["B3"].Value);

                    if (currentMonth != DateTime.Now.Month)
                    {
                        monthlyData.Cells["A3"].Value = DateTime.Now.Month;
                        monthlyData.Cells["B3"].Value = false;
                        for (int i = 0; i < totalDay; i++)
                        {
                            weeklyData.Cells["B" + (i + 3)].Value = false;
                        }
                    }
                    else if (currentMonth == DateTime.Now.Month && !currentIsSend && DateTime.Now.Day == currentDay &&
                             DateTime.Now.Hour >= currentHour)
                    {
                        this.SendPartCompareReport();

                        monthlyData.Cells["B3"].Value = true;
                    }

                    //workbook.Save("Schedule.xls");
                    workbook.Save(ConfigurationManager.AppSettings["DataFile"]);
                }
            }
            catch (Exception ex)
            {
                this.EventLog.WriteEntry("Have Error: " + ex.Message);
            }
        }

        private void SendPartCompareReport()
        {
            var dbService = new EAMStoreProcedureService();

            var ds = dbService.GetDataSet("AA_COMPARE_PART");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // Export data file
                Workbook workbook = new Workbook(ConfigurationManager.AppSettings["PartCompareTemplate"]);
                var wsData = workbook.Worksheets[0];
                wsData.Cells.ImportDataTable(ds.Tables[0], false, 1, 0, ds.Tables[0].Rows.Count, 6, false);
                var compareDataFilePath = ConfigurationManager.AppSettings["PartCompareData"] +
                                          "EAM-Fast Part Compare_" +
                                          DateTime.Now.Month + "-" + DateTime.Now.Year + ".xlsx";
                workbook.Save(compareDataFilePath);

                // -----------------------------------------------------------------------------


                var bodyAll = "<table>Dear Mr/Mrs,<br/><br/>" +
                              "Phần mềm Infor EAM TBMICCO gửi đến Mr/Mrs thông tin so sánh dữ liệu vật tư giữa Fast và Infor như file đính kèm.<br/><br/>" +
                              "<b>MICCO-lưu ý:</b> Nội dung của email này chỉ được gửi và chỉ nhằm mục đích sử dụng cho đúng người nhận. Nếu bạn không đúng là người nhận liên quan thì email này đã được gửi tới bạn do sai sót. Bất kỳ tiết lộ, sao chép, phân phối hoặc hành động dựa trên các thông tin trong email là bị cấm và có thể là trái pháp luật. Nếu bạn vô tình nhận được email này, xin vui lòng liên hệ với người gửi và xóa email này trên máy tính của bạn.";

                var subject = "Bảng danh sách thông tin so sánh dữ liệu vật tư giữa Fast và Infor";
                var toList = ConfigurationManager.AppSettings["MonthlyTo"].Split(',')
                    .Where(t => !string.IsNullOrEmpty(t)).ToList();
                
                var attachments = new List<string>() {compareDataFilePath};
                
                this.SendEmail(this.isTest, subject, bodyAll, toList, new List<string>(), this.bccList, attachments);
            }

        }

        private void SendEmail(bool isTest, string subject, string body, List<string> toList, List<string> ccList,
            List<string> bccList, List<string> attachments)
        {
            try
            {
                // Send mail
                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials =
                        Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    Host = ConfigurationManager.AppSettings["Host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Email"],
                        ConfigurationManager.AppSettings["EmailPass"])
                };

                var messageAll = new MailMessage();
                messageAll.From = new MailAddress(ConfigurationManager.AppSettings["Email"],
                    ConfigurationManager.AppSettings["EmailName"]);

                messageAll.BodyEncoding = new UTF8Encoding();
                messageAll.IsBodyHtml = true;
                messageAll.Subject = subject;
                messageAll.Body = body;


                if (isTest)
                {
                    messageAll.To.Add("vuhoanglamnhi@truetech.com.vn");
                    messageAll.To.Add("nguyenvanhong@truetech.com.vn");

                    foreach (var item in attachments)
                    {
                        messageAll.Attachments.Add(new Attachment(item));
                        this.EventLog.WriteEntry("Attach file: " + item);
                    }
                }
                else
                {
                    foreach (var toEmail in toList)
                    {
                        messageAll.To.Add(toEmail.Trim());
                    }

                    foreach (var ccEmail in ccList)
                    {
                        messageAll.CC.Add(ccEmail.Trim());
                    }

                    foreach (var bccEmail in bccList)
                    {
                        messageAll.Bcc.Add(bccEmail.Trim());
                    }

                    foreach (var item in attachments)
                    {
                        messageAll.Attachments.Add(new Attachment(item));
                        this.EventLog.WriteEntry("Attach file: " + item);
                    }
                }

                smtpClient.Send(messageAll);
                this.EventLog.WriteEntry("Sent email notification.");
            }
            catch (Exception ex)
            {
                this.EventLog.WriteEntry("Send mail have Error: " + ex.Message);
            }

        }
    }
}
