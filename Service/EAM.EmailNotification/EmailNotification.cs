using System;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.ServiceProcess;
using Aspose.Cells;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace EAM.Notification
{
    public partial class EmailNotification : ServiceBase
    {
        private System.Timers.Timer timerSync ;
        private OracleConnection conn;
        private bool flag1 = false;
        private bool flag2 = false;

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

                var strConn = ConfigurationManager.ConnectionStrings["cmmsdbconnection"].ConnectionString;
                conn = new OracleConnection(strConn);
                conn.Open();
                conn.Close();

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
                // Monthly report
                SendMonthlyReport();

                // Weekly report
                SendWeeklyReport();

                // Copy report/attach file by Monthly
                var copiedMonth = 0;
                var dayOfMonth = Convert.ToInt32(ConfigurationManager.AppSettings["CopyDate"]);

                var desFolder = new DirectoryInfo(ConfigurationManager.AppSettings["EAMCopyDataFolder"]);

                if (DateTime.Now.Day == dayOfMonth && DateTime.Now.Month != copiedMonth)
                {
                    this.EventLog.WriteEntry("Delete older folder");
                    foreach (var subFolder in desFolder.GetDirectories())
                    {
                        Directory.Delete(subFolder.FullName, true);
                    }

                    this.CopyDataFile();
                    copiedMonth = DateTime.Now.Month;
                }
                // --------------------------------------------------------------------------------------------

                //var day1 = Convert.ToInt32(ConfigurationManager.AppSettings["CopyDate"].Split(',')[0].Split('.')[0]);
                //var month1 = Convert.ToInt32(ConfigurationManager.AppSettings["CopyDate"].Split(',')[0].Split('.')[1]);

                //var day2 = Convert.ToInt32(ConfigurationManager.AppSettings["CopyDate"].Split(',')[1].Split('.')[0]);
                //var month2 = Convert.ToInt32(ConfigurationManager.AppSettings["CopyDate"].Split(',')[1].Split('.')[1]);

                //if ((DateTime.Now.Day == day1 && DateTime.Now.Month == month1 ) && flag1 == false)
                //{
                //    this.EventLog.WriteEntry("Delete older folder");
                //    foreach (var subFolder in desFolder.GetDirectories())
                //    {
                //        Directory.Delete(subFolder.FullName,true);
                //    }
                //    this.CopyDataFile();
                //    flag2 = false;
                //    flag1 = true;
                //}
                //else if ((DateTime.Now.Day == day2 && DateTime.Now.Month == month2) && flag2 == false)
                //{
                //    this.EventLog.WriteEntry("Delete Older folder");
                //    foreach (var subFolder in desFolder.GetDirectories())
                //    {
                //        Directory.Delete(subFolder.FullName, true);
                //    }
                //    this.CopyDataFile();
                //    flag2 = true;
                //    flag1 = false;
                //}
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

        private void CopyDataFile()
        {
            
            var ds = new DataSet();
            var strConn = ConfigurationManager.ConnectionStrings["cmmsdbconnection"].ConnectionString;
            conn = new OracleConnection(strConn);
            var da = new OracleDataAdapter();
            var cmd = new OracleCommand();
            cmd.Connection = conn;

            cmd.CommandText = @"SELECT EVT_ORG, EVT_MRC, EVT_CODE, EVT_DESC, DOC_FILENAME
                                FROM r5docentities INNER JOIN R5DOCUMENTS ON DOC_CODE = DAE_DOCUMENT
                                INNER JOIN R5EVENTS ON DAE_CODE = EVT_CODE
                                WHERE DAE_ENTITY = 'EVNT' AND(EVT_PPM LIKE '%-4K' OR EVT_PPM LIKE '%-8K' OR EVT_PPM LIKE '%-PM4K' OR EVT_PPM LIKE '%-PM8K')
                                AND ((YEAR(EVT_SCHEDEND) = YEAR(SYSDATE) AND MONTH(EVT_SCHEDEND) = MONTH(SYSDATE) - 1 AND MONTH(SYSDATE) > 1)
                                OR (YEAR(EVT_SCHEDEND) = YEAR(SYSDATE) - 1 AND MONTH(EVT_SCHEDEND) = 12 AND MONTH(SYSDATE) = 1))
                                ORDER BY EVT_ORG,  EVT_MRC, EVT_CODE";
            cmd.CommandType = CommandType.Text;

            da.SelectCommand = cmd;

            try
            {
                conn.Open();
                da.Fill(ds);
                conn.Close();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var dataList = ds.Tables[0].AsEnumerable().ToList();
                    var sourceFolder = ConfigurationManager.AppSettings["EAMUploadDataFolder"];
                    var desFolder = ConfigurationManager.AppSettings["EAMCopyDataFolder"];
                    var dataGroupByOrg = dataList.GroupBy(t => t["EVT_ORG"].ToString());
                    foreach (var orgGroup in dataGroupByOrg)
                    {
                        var dataGroupByDiscipline = orgGroup.GroupBy(t => t["EVT_MRC"].ToString());
                        foreach (var disciplineGroup in dataGroupByDiscipline)
                        {
                            Directory.CreateDirectory(desFolder + orgGroup.Key + @"\" + disciplineGroup.Key + @"\");
                            foreach (DataRow item in disciplineGroup)
                            {
                                var filePath = sourceFolder + item["DOC_FILENAME"].ToString();
                                var fileInfor = new FileInfo(filePath);
                                if (fileInfor.Exists)
                                {
                                    File.Copy(filePath, desFolder + orgGroup.Key + @"\" + disciplineGroup.Key + @"\" + item["DOC_FILENAME"].ToString(), true);
                                    this.EventLog.WriteEntry("Copy completed: '" + filePath + "'");
                                }
                                else
                                {
                                    this.EventLog.WriteEntry("File not found: '" + filePath + "'");
                                }
                            }
                        }
                    }

                    this.EventLog.WriteEntry("Monthly report copied.");

                }
            }
            catch (Exception ex)
            {
                this.EventLog.WriteEntry("Have Error: " + ex.Message);
            }
        }

        private void SendWeeklyReport()
        {
            try
            {
                if (ConfigurationManager.AppSettings["EnableWeeklyReport"] == "true")
                {
                    //Workbook workbook = new Workbook("Schedule.xls");
                    Workbook workbook = new Workbook(ConfigurationManager.AppSettings["DataFile"]);
                    var weeklyData = workbook.Worksheets[1];
                    var totalDay = Convert.ToInt32(weeklyData.Cells["A1"].Value);
                    for (int i = 0; i < totalDay; i++)
                    {
                        var currentDay = Convert.ToInt32(weeklyData.Cells["A" + (i + 3)].Value);
                        var currentIsSend = Convert.ToBoolean(weeklyData.Cells["B" + (i + 3)].Value);
                        if (currentDay == DateTime.Now.Day && !currentIsSend)
                        {
                            this.SendWeeklyReportForPlatform("CCP");
                            this.SendWeeklyReportForPlatform("MKC");
                            this.SendWeeklyReportForPlatform("DGCP");
                            weeklyData.Cells["B" + (i + 3)].Value = true;
                        }
                    }

                    workbook.Save(ConfigurationManager.AppSettings["DataFile"]);
                    //workbook.Save("Schedule.xls");
                }
                
            }
            catch (Exception ex)
            {
                this.EventLog.WriteEntry("Have Error: " + ex.Message);
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
                    else if (currentMonth == DateTime.Now.Month && !currentIsSend && DateTime.Now.Day == currentDay && DateTime.Now.Hour >= currentHour)
                    {
                        this.SendMonthlyReportForAllPlatform();

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

        private void SendMonthlyReportForAllPlatform()
        {
            var smtpClient = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                Host = ConfigurationManager.AppSettings["Host"],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Email"], ConfigurationManager.AppSettings["EmailPass"])
            };

            var smtpClientLD = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                Host = ConfigurationManager.AppSettings["Host"],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Email"], ConfigurationManager.AppSettings["EmailPass"])
            };

            var bodyAll = @"<table style='width: 800px'>
        <tr>
            <td>
                Dear All,<br />
                Report from InforEAM system about the implementation of maintenance plan in the monthly with the following information as below:
            </td>
        </tr>
        <tr>
            <td>
                <ul style='list-style-type: square;'>
                    <li><b>The implementation of Wos in last month:</b></li>
                </ul>
                <table style='width: 800px;'>
                    <tr>
                        <td width='25%' align='center'>
                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[" + this.GetKPIInMonthAllPlatform() + @"],backgroundColor:'coral'}]}}"" width='200px'><br />
                            Percent WOs Completed in last month: " + this.GetKPIInMonthAllPlatform() + @"%
                        </td>
                        <td width='25%' align='center'>
                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[" + this.GetKPIInMonthOfPlatform("MKC") + @"],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
                            Percent WOs Completed of MKC in last month: " + this.GetKPIInMonthOfPlatform("MKC") + @"%
                        </td>
                        <td width='25%' align='center'>
                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[" + this.GetKPIInMonthOfPlatform("DGCP") + @"],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
                            Percent WOs Completed of DGCP in last month: " + this.GetKPIInMonthOfPlatform("DGCP") + @"%
                        </td>
                        <td width='25%' align='center'>
                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[" + this.GetKPIInMonthOfPlatform("CCP") + @"],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
                            Percent WOs Completed of CCP in last month: " + this.GetKPIInMonthOfPlatform("CCP") + @"%
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
        <tr>
            <td>
                <table border='0' cellspacing='0' cellpadding='0' width='800px' style='border-collapse: collapse'>
                    <tr style='height: 30px'>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Platform</span></b>
                        </td>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Discipline</span></b>
                        </td>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Planned</span></b>
                        </td>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Completed</span></b>
                        </td>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Percent</span></b>
                        </td>
                    </tr>
                    <tr style='height: 33px'>
                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>CCP</b>
                        </td>

                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>ELE</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("CCP", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("CCP", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("CCP", "ELE") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>INS</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPlanned("CCP", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthCompleted("CCP", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPercent("CCP", "INS") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>MEC</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("CCP", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("CCP", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("CCP", "MEC") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>PRO</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPlanned("CCP", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthCompleted("CCP", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPercent("CCP", "PRO") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>SAF</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("CCP", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("CCP", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("CCP", "SAF") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>MKC</b>
                        </td>

                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>ELE</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("MKC", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("MKC", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("MKC", "ELE") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>INS</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPlanned("MKC", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthCompleted("MKC", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPercent("MKC", "INS") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>MEC</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("MKC", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("MKC", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("MKC", "MEC") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>PRO</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                           " + this.GetMonthPlanned("MKC", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthCompleted("MKC", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPercent("MKC", "PRO") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>SAF</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("MKC", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("MKC", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("MKC", "SAF") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>DGCP</b>
                        </td>

                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>ELE</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("DGCP", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("DGCP", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("DGCP", "ELE") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>INS</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPlanned("DGCP", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthCompleted("DGCP", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPercent("DGCP", "INS") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>MEC</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("DGCP", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("DGCP", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("DGCP", "MEC") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>PRO</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                             " + this.GetMonthPlanned("DGCP", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthCompleted("DGCP", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPercent("DGCP", "PRO") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>SAF</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("DGCP", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("DGCP", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("DGCP", "SAF") + @"%
                        </td>
                    </tr>

                </table>
            </td>
        </tr>

        <tr>
            <td>
                <ul style='list-style-type: square;'>
                    <li><b>Wos plan in this month:</b></li>
                </ul>
                <table style='width: 800px;'>
                    <tr>
                        <td width='25%' align='center'>
                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[" + this.GetKPIPlanInMonthAllPlatform() + @"],backgroundColor:'coral'}]}}"" width='200px'><br />
                            Percent WOs Completed in this month: " + this.GetKPIPlanInMonthAllPlatform() + @"%
                        </td>
                        <td width='25%' align='center'>
                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[" + this.GetKPIPlanInMonthOfPlatform("MKC") + @"],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
                            Percent WOs Completed of MKC in this month: " + this.GetKPIPlanInMonthOfPlatform("MKC") + @"%
                        </td>
                        <td width='25%' align='center'>
                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[" + this.GetKPIPlanInMonthOfPlatform("DGCP") + @"],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
                            Percent WOs Completed of DGCP in this month: " + this.GetKPIPlanInMonthOfPlatform("DGCP") + @"%
                        </td>
                        <td width='25%' align='center'>
                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[" + this.GetKPIPlanInMonthOfPlatform("CCP") + @"],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
                            Percent WOs Completed of CCP in this month: " + this.GetKPIPlanInMonthOfPlatform("CCP") + @"%
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
        <tr>
            <td>
                <table border='0' cellspacing='0' cellpadding='0' width='800px' style='border-collapse: collapse'>
                    <tr style='height: 30px'>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Platform</span></b>
                        </td>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Discipline</span></b>
                        </td>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Planned</span></b>
                        </td>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Completed</span></b>
                        </td>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Percent</span></b>
                        </td>
                    </tr>
                    <tr style='height: 33px'>
                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>CCP</b>
                        </td>

                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>ELE</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("CCP", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("CCP", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("CCP", "ELE") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>INS</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPlanned("CCP", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthCompleted("CCP", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPercent("CCP", "INS") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>MEC</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("CCP", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("CCP", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("CCP", "MEC") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>PRO</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPlanned("CCP", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthCompleted("CCP", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPercent("CCP", "PRO") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>SAF</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("CCP", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("CCP", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("CCP", "SAF") + @"%

                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>MKC</b>
                        </td>

                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>ELE</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("MKC", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("MKC", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("MKC", "ELE") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>INS</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPlanned("MKC", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthCompleted("MKC", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPercent("MKC", "INS") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>MEC</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("MKC", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("MKC", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("MKC", "MEC") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>PRO</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPlanned("MKC", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthCompleted("MKC", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPercent("MKC", "PRO") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>SAF</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("MKC", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("MKC", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("MKC", "SAF") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>DGCP</b>
                        </td>

                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>ELE</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("DGCP", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("DGCP", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("DGCP", "ELE") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>INS</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPlanned("DGCP", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthCompleted("DGCP", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPercent("DGCP", "INS") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>MEC</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("DGCP", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("DGCP", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("DGCP", "MEC") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>PRO</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPlanned("DGCP", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthCompleted("DGCP", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPercent("DGCP", "PRO") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>SAF</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("DGCP", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("DGCP", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("DGCP", "SAF") + @"%
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    <tr>
        <td>
            <i>The Attachment As Below:</i><br />
            <ul style='list-style-type: square;'>
                <li>P-HT-006/F6: Báo cáo công việc bảo dưỡng định kỳ thiết bị tháng " + DateTime.Now.AddMonths(-1).Month + @".</li>
                <li>P-HT-006/F7: Báo cáo thống kê sự cố hư hỏng thiết bị tháng " + DateTime.Now.AddMonths(-1).Month + @"</li>
                <li>P-HT-006/F8: Thống kê vật tư, phụ tùng tiêu hao tháng " + DateTime.Now.AddMonths(-1).Month + @"</li>
                <li>P-HT-006/F3a: Kế hoạch công việc bảo dưỡng định kỳ và sửa chữa thiết bị chính tháng " + DateTime.Now.Month + @".</li>
                <li>P-HT-006/F3b: Kế hoạch công việc bảo dưỡng định kỳ và sửa chữa tháng " + DateTime.Now.Month + @".</li>
                <li>Báo cáo giờ chạy máy của thiết bị chính</li>                
                <li>Danh sách công việc điều chuyển cần phê duyệt</li>
            </ul>
        </td>
    </tr>
        <tr>
            <td>
                For Detail information, Please login to InforEAM system via the link: http://cmms.vietsov.com.vn/web/base/logindisp?tenant=XNK
                <br/>
                Thanks & Regards,
                <br/>
                (Reported by InforEAM)
            </td>
        </tr>
    </table>";

            var bodyLD = @"<table style='width: 800px'>
        <tr>
            <td>
                Dear All,<br />
                Report from InforEAM system about the implementation of maintenance plan in the monthly with the following information as below:
            </td>
        </tr>
        <tr>
            <td>
                <ul style='list-style-type: square;'>
                    <li><b>The implementation of Wos in last month:</b></li>
                </ul>
                <table style='width: 800px;'>
                    <tr>
                        <td width='25%' align='center'>
                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[" + this.GetKPIInMonthAllPlatform() + @"],backgroundColor:'coral'}]}}"" width='200px'><br />
                            Percent WOs Completed in last month: " + this.GetKPIInMonthAllPlatform() + @"%
                        </td>
                        <td width='25%' align='center'>
                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[" + this.GetKPIInMonthOfPlatform("MKC") + @"],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
                            Percent WOs Completed of MKC in last month: " + this.GetKPIInMonthOfPlatform("MKC") + @"%
                        </td>
                        <td width='25%' align='center'>
                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[" + this.GetKPIInMonthOfPlatform("DGCP") + @"],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
                            Percent WOs Completed of DGCP in last month: " + this.GetKPIInMonthOfPlatform("DGCP") + @"%
                        </td>
                        <td width='25%' align='center'>
                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[" + this.GetKPIInMonthOfPlatform("CCP") + @"],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
                            Percent WOs Completed of CCP in last month: " + this.GetKPIInMonthOfPlatform("CCP") + @"%
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
        <tr>
            <td>
                <table border='0' cellspacing='0' cellpadding='0' width='800px' style='border-collapse: collapse'>
                    <tr style='height: 30px'>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Platform</span></b>
                        </td>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Discipline</span></b>
                        </td>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Planned</span></b>
                        </td>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Completed</span></b>
                        </td>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Percent</span></b>
                        </td>
                    </tr>
                    <tr style='height: 33px'>
                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>CCP</b>
                        </td>

                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>ELE</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("CCP", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("CCP", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("CCP", "ELE") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>INS</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPlanned("CCP", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthCompleted("CCP", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPercent("CCP", "INS") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>MEC</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("CCP", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("CCP", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("CCP", "MEC") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>PRO</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPlanned("CCP", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthCompleted("CCP", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPercent("CCP", "PRO") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>SAF</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("CCP", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("CCP", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("CCP", "SAF") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>MKC</b>
                        </td>

                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>ELE</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("MKC", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("MKC", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("MKC", "ELE") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>INS</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPlanned("MKC", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthCompleted("MKC", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPercent("MKC", "INS") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>MEC</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("MKC", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("MKC", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("MKC", "MEC") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>PRO</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                           " + this.GetMonthPlanned("MKC", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthCompleted("MKC", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPercent("MKC", "PRO") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>SAF</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("MKC", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("MKC", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("MKC", "SAF") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>DGCP</b>
                        </td>

                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>ELE</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("DGCP", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("DGCP", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("DGCP", "ELE") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>INS</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPlanned("DGCP", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthCompleted("DGCP", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPercent("DGCP", "INS") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>MEC</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("DGCP", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("DGCP", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("DGCP", "MEC") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>PRO</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                             " + this.GetMonthPlanned("DGCP", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthCompleted("DGCP", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetMonthPercent("DGCP", "PRO") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>SAF</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPlanned("DGCP", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthCompleted("DGCP", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetMonthPercent("DGCP", "SAF") + @"%
                        </td>
                    </tr>

                </table>
            </td>
        </tr>

        <tr>
            <td>
                <ul style='list-style-type: square;'>
                    <li><b>Wos plan in this month:</b></li>
                </ul>
                <table style='width: 800px;'>
                    <tr>
                        <td width='25%' align='center'>
                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[" + this.GetKPIPlanInMonthAllPlatform() + @"],backgroundColor:'coral'}]}}"" width='200px'><br />
                            Percent WOs Completed in this month: " + this.GetKPIPlanInMonthAllPlatform() + @"%
                        </td>
                        <td width='25%' align='center'>
                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[" + this.GetKPIPlanInMonthOfPlatform("MKC") + @"],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
                            Percent WOs Completed of MKC in this month: " + this.GetKPIPlanInMonthOfPlatform("MKC") + @"%
                        </td>
                        <td width='25%' align='center'>
                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[" + this.GetKPIPlanInMonthOfPlatform("DGCP") + @"],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
                            Percent WOs Completed of DGCP in this month: " + this.GetKPIPlanInMonthOfPlatform("DGCP") + @"%
                        </td>
                        <td width='25%' align='center'>
                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[" + this.GetKPIPlanInMonthOfPlatform("CCP") + @"],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
                            Percent WOs Completed of CCP in this month: " + this.GetKPIPlanInMonthOfPlatform("CCP") + @"%
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
        <tr>
            <td>
                <table border='0' cellspacing='0' cellpadding='0' width='800px' style='border-collapse: collapse'>
                    <tr style='height: 30px'>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Platform</span></b>
                        </td>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Discipline</span></b>
                        </td>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Planned</span></b>
                        </td>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Completed</span></b>
                        </td>
                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                            <b><span style='color: white'>Percent</span></b>
                        </td>
                    </tr>
                    <tr style='height: 33px'>
                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>CCP</b>
                        </td>

                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>ELE</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("CCP", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("CCP", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("CCP", "ELE") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>INS</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPlanned("CCP", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthCompleted("CCP", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPercent("CCP", "INS") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>MEC</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("CCP", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("CCP", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("CCP", "MEC") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>PRO</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPlanned("CCP", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthCompleted("CCP", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPercent("CCP", "PRO") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>SAF</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("CCP", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("CCP", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("CCP", "SAF") + @"%

                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>MKC</b>
                        </td>

                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>ELE</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("MKC", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("MKC", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("MKC", "ELE") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>INS</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPlanned("MKC", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthCompleted("MKC", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPercent("MKC", "INS") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>MEC</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("MKC", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("MKC", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("MKC", "MEC") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>PRO</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPlanned("MKC", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthCompleted("MKC", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPercent("MKC", "PRO") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>SAF</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("MKC", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("MKC", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("MKC", "SAF") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>DGCP</b>
                        </td>

                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>ELE</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("DGCP", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("DGCP", "ELE") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("DGCP", "ELE") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>INS</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPlanned("DGCP", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthCompleted("DGCP", "INS") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPercent("DGCP", "INS") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>MEC</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("DGCP", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("DGCP", "MEC") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("DGCP", "MEC") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            <b>PRO</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPlanned("DGCP", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthCompleted("DGCP", "PRO") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                            " + this.GetPlanMonthPercent("DGCP", "PRO") + @"%
                        </td>
                    </tr>

                    <tr style='height: 33px'>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            <b>SAF</b>
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPlanned("DGCP", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthCompleted("DGCP", "SAF") + @"
                        </td>
                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                            " + this.GetPlanMonthPercent("DGCP", "SAF") + @"%
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    <tr>
        <td>
            <i>The Attachment As Below:</i><br />
            <ul style='list-style-type: square;'>                
                <li>P-HT-006/F3a: Kế hoạch công việc bảo dưỡng định kỳ và sửa chữa thiết bị chính tháng " + DateTime.Now.Month + @".</li>
                <li>P-HT-006/F3b: Kế hoạch công việc bảo dưỡng định kỳ và sửa chữa tháng " + DateTime.Now.Month + @".</li>
                <li>Báo cáo giờ chạy máy của thiết bị chính</li>
            </ul>
        </td>
    </tr>
        <tr>
            <td>
                For Detail information, Please login to InforEAM system via the link: http://cmms.vietsov.com.vn/web/base/logindisp?tenant=XNK
                <br/>
                Thanks & Regards,
                <br/>
                (Reported by InforEAM)
            </td>
        </tr>
    </table>";

            var messageAll = new MailMessage();
            messageAll.From = new MailAddress(ConfigurationManager.AppSettings["Email"], ConfigurationManager.AppSettings["EmailName"]);

            messageAll.BodyEncoding = new UTF8Encoding();
            messageAll.IsBodyHtml = true;

            messageAll.Subject = "Report on results of repair and maintenance in " +
                              DateTime.Now.AddMonths(-1).ToString("MM.yyyy") +
                              " and plan on repair and maintenance in " + DateTime.Now.ToString("MM.yyyy");
            messageAll.Body = bodyAll;

            var messageLD = new MailMessage();
            messageLD.From = new MailAddress(ConfigurationManager.AppSettings["Email"], ConfigurationManager.AppSettings["EmailName"]);

            messageLD.BodyEncoding = new UTF8Encoding();
            messageLD.IsBodyHtml = true;

            messageLD.Subject = "Report on results of repair and maintenance in " +
                                DateTime.Now.AddMonths(-1).ToString("MM.yyyy") +
                                " and plan on repair and maintenance in " + DateTime.Now.ToString("MM.yyyy");
            messageLD.Body = bodyLD;

            if (ConfigurationManager.AppSettings["IsTesting"] == "true")
            {
                messageAll.To.Add("vuhoanglamnhi@truetech.com.vn");
                messageAll.To.Add("nguyenvanhong@truetech.com.vn");
                messageAll.To.Add("support.cmms@vietsov.com.vn");

                var attachFiles = Directory.GetFiles(ConfigurationManager.AppSettings["ReportFolder"], "*.pdf");
                foreach (var attachFile in attachFiles)
                {
                    messageAll.Attachments.Add(new Attachment(attachFile));
                    this.EventLog.WriteEntry("Attach file: " + attachFile);

                }
            }
            else
            {
                foreach (var toEmail in ConfigurationManager.AppSettings["MonthlyTo"].Split(',').Where(t => !string.IsNullOrEmpty(t)))
                {
                    messageAll.To.Add(toEmail);
                }

                foreach (var ccEmail in ConfigurationManager.AppSettings["MonthlyCC"].Split(',').Where(t => !string.IsNullOrEmpty(t)))
                {
                    messageAll.CC.Add(ccEmail);
                }

                foreach (var toEmailLD in ConfigurationManager.AppSettings["MonthlyToLanhDao"].Split(',').Where(t => !string.IsNullOrEmpty(t)))
                {
                    messageLD.To.Add(toEmailLD);
                }

                var attachFiles = Directory.GetFiles(ConfigurationManager.AppSettings["ReportFolder"],"*.pdf");
                foreach (var attachFile in attachFiles)
                {

                    messageAll.Attachments.Add(new Attachment(attachFile));

                    if (attachFile.Contains("F3a") || attachFile.Contains("F3b") || attachFile.Contains("Báo cáo giờ chạy máy của thiết bị chính"))
                    {
                        messageLD.Attachments.Add(new Attachment(attachFile));
                    }

                    this.EventLog.WriteEntry("Attach file: " + attachFile);

                }

                foreach (var toEmail in ConfigurationManager.AppSettings["BCC"].Split(',').Where(t => !string.IsNullOrEmpty(t)))
                {
                    messageAll.Bcc.Add(toEmail);
                    messageLD.Bcc.Add(toEmail);
                }
            }
            
            smtpClient.Send(messageAll);
            smtpClientLD.Send(messageLD);
        }

        private void SendWeeklyReportForPlatform(string platform)
        {
            //var platform = "CCP";
            DateTimeFormatInfo dtfi = CultureInfo.GetCultureInfo("en-US").DateTimeFormat;
            var smtpClient = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                Host = ConfigurationManager.AppSettings["Host"],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Email"], ConfigurationManager.AppSettings["EmailPass"])
            };

            var message = new MailMessage();
            message.From = new MailAddress(ConfigurationManager.AppSettings["Email"], ConfigurationManager.AppSettings["EmailName"]);

            message.BodyEncoding = new UTF8Encoding();
            message.IsBodyHtml = true;

            message.Subject = "Notice from InforEAM system about the implementation of maintenance plan in " + dtfi.GetMonthName(DateTime.Now.Month);
            message.Body = @"<table style='width: 610px'>
    <tr>
        <td>
            Dear " + platform + @" Platform,<br/>
            Notice from InforEAM system about the implementation of maintenance plan in the monthly with the following information as below:
        </td>
    </tr>
    <tr>
        <td align='center'>
            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[" +
                           this.GetKPIPlanInMonthOfPlatform(platform) +
                           @"],backgroundColor:'cornflowerblue'}]}}"" width='250px'><br/>

            Percent WOs Completed of " + platform + @" in this month: " + this.GetKPIPlanInMonthOfPlatform(platform) +
                           @"%
        </td>
    </tr>
    <tr>
        <td>
            <table border='0' cellspacing='0' cellpadding='0' width='600px' style='border-collapse: collapse'>
                <tr style='height: 30px'>
                    <td width='25%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                        <b><span style='color: white'>Discipline</span></b>
                    </td>
                    <td width='25%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                        <b><span style='color: white'>Planned</span></b>
                    </td>
                    <td width='25%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                        <b><span style='color: white'>Completed</span></b>
                    </td>
                    <td width='25%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
                        <b><span style='color: white'>Percent</span></b>
                    </td>
                </tr>
                <tr style='height: 40px'>
                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                        <b>ELE</b>
                    </td>
                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                        " + this.GetPlanMonthPlanned(platform, "ELE") + @"
                    </td>
                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                        " + this.GetPlanMonthCompleted(platform, "ELE") + @"
                    </td>
                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                        " + (this.GetPlanMonthPlanned(platform, "ELE") == 0 ? 0 : Math.Round(this.GetPlanMonthCompleted(platform, "ELE")/ this.GetPlanMonthPlanned(platform, "ELE") * 100, 0)) + @"%
                    </td>
                </tr>

                <tr style='height: 40px'>
                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                        <b>INS</b>
                    </td>

                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                        " + this.GetPlanMonthPlanned(platform, "INS") + @"
                    </td>

                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                        " + this.GetPlanMonthCompleted(platform, "INS") + @"
                    </td>

                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                        " + (this.GetPlanMonthPlanned(platform, "INS") == 0 ? 0 : Math.Round(this.GetPlanMonthCompleted(platform, "INS") / this.GetPlanMonthPlanned(platform, "INS") * 100, 0)) + @"%
                    </td>
                </tr>

                <tr style='height: 40px'>
                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                        <b>MEC</b>
                    </td>
                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                        " + this.GetPlanMonthPlanned(platform, "MEC") + @"
                    </td>
                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                        " + this.GetPlanMonthCompleted(platform, "MEC") + @"

                    </td>
                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                        " + (this.GetPlanMonthPlanned(platform, "MEC") == 0 ? 0 : Math.Round(this.GetPlanMonthCompleted(platform, "MEC") / this.GetPlanMonthPlanned(platform, "MEC") * 100, 0)) + @"%
                    </td>
                </tr>

                <tr style='height: 40px'>
                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                        <b>PRO</b>
                    </td>

                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                        " + this.GetPlanMonthPlanned(platform, "PRO") + @"
                    </td>

                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                        " + this.GetPlanMonthCompleted(platform, "PRO") + @"
                    </td>

                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
                        " + (this.GetPlanMonthPlanned(platform, "PRO") == 0 ? 0 : Math.Round(this.GetPlanMonthCompleted(platform, "PRO") / this.GetPlanMonthPlanned(platform, "PRO") * 100, 0)) + @"%
                    </td>
                </tr>

                <tr style='height: 40px'>
                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                        <b>SAF</b>
                    </td>
                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                        " + this.GetPlanMonthPlanned(platform, "SAF") + @"
                    </td>
                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                        " + this.GetPlanMonthCompleted(platform, "SAF") + @"
                    </td>
                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
                        " + (this.GetPlanMonthPlanned(platform, "SAF") == 0 ? 0 : Math.Round(this.GetPlanMonthCompleted(platform, "SAF") / this.GetPlanMonthPlanned(platform, "SAF") * 100, 0)) + @"%
                    </td>
                </tr>

            </table>
        </td>
    </tr>
    <tr>
        <td>
            To update any information, please log in the InforEAM system via the link: http://cmms.vietsov.com.vn/web/base/logindisp?tenant=XNK
            <br/>Thanks & Regards,
            <br/>(Reported by InforEAM)
        </td>
    </tr>
</table>";

            if (ConfigurationManager.AppSettings["IsTesting"] == "true")
            {
                message.To.Add("vuhoanglamnhi@truetech.com.vn");
                message.To.Add("nguyenvanhong@truetech.com.vn");
                message.To.Add("support.cmms@vietsov.com.vn");

            }
            else
            {
                switch (platform)
                {
                    case "CCP":
                        foreach (var toEmail in ConfigurationManager.AppSettings["CCPTo"].Split(',').Where(t => !string.IsNullOrEmpty(t)))
                        {
                            message.To.Add(toEmail);
                        }

                        foreach (var toEmail in ConfigurationManager.AppSettings["CCPCC"].Split(',').Where(t => !string.IsNullOrEmpty(t)))
                        {
                            message.CC.Add(toEmail);
                        }

                        var ccpAttachPath = ConfigurationManager.AppSettings["ReportFolder"] + @"\CCP-Danh sách công việc điều chuyển cần phê duyệt-en.pdf";
                        var ccpAttachPath2 = ConfigurationManager.AppSettings["ReportFolder"] + @"\CCP-Báo cáo giờ chạy máy của thiết bị chính-en.pdf";

                        if (File.Exists(ccpAttachPath))
                        {
                            message.Attachments.Add(new Attachment(ccpAttachPath));
                            this.EventLog.WriteEntry("Attach file: " + ccpAttachPath);
                        }

                        if (File.Exists(ccpAttachPath2))
                        {
                            message.Attachments.Add(new Attachment(ccpAttachPath2));
                            this.EventLog.WriteEntry("Attach file: " + ccpAttachPath2);
                        }
                        break;
                    case "MKC":
                        foreach (var toEmail in ConfigurationManager.AppSettings["MKCTo"].Split(',').Where(t => !string.IsNullOrEmpty(t)))
                        {
                            message.To.Add(toEmail);
                        }

                        foreach (var toEmail in ConfigurationManager.AppSettings["MKCCC"].Split(',').Where(t => !string.IsNullOrEmpty(t)))
                        {
                            message.CC.Add(toEmail);
                        }

                        var mkcAttachPath = ConfigurationManager.AppSettings["ReportFolder"] + @"\MKC-Danh sách công việc điều chuyển cần phê duyệt-en.pdf";
                        var mkcAttachPath2 = ConfigurationManager.AppSettings["ReportFolder"] + @"\MKC-Báo cáo giờ chạy máy của thiết bị chính-en.pdf";
                        if (File.Exists(mkcAttachPath))
                        {
                            message.Attachments.Add(new Attachment(mkcAttachPath));
                            this.EventLog.WriteEntry("Attach file: " + mkcAttachPath);
                        }

                        if (File.Exists(mkcAttachPath2))
                        {
                            message.Attachments.Add(new Attachment(mkcAttachPath2));
                            this.EventLog.WriteEntry("Attach file: " + mkcAttachPath2);
                        }
                        break;
                    case "DGCP":
                        foreach (var toEmail in ConfigurationManager.AppSettings["DGCPTo"].Split(',').Where(t => !string.IsNullOrEmpty(t)))
                        {
                            message.To.Add(toEmail);
                        }

                        foreach (var toEmail in ConfigurationManager.AppSettings["DGCPCC"].Split(',').Where(t => !string.IsNullOrEmpty(t)))
                        {
                            message.CC.Add(toEmail);
                        }

                        var dgcpAttachPath = ConfigurationManager.AppSettings["ReportFolder"] + @"\DGCP-Danh sách công việc điều chuyển cần phê duyệt-en.pdf";
                        var dgcpAttachPath2 = ConfigurationManager.AppSettings["ReportFolder"] + @"\DGCP-Báo cáo giờ chạy máy của thiết bị chính-en.pdf";
                        if (File.Exists(dgcpAttachPath))
                        {
                            message.Attachments.Add(new Attachment(dgcpAttachPath));
                            this.EventLog.WriteEntry("Attach file: " + dgcpAttachPath);
                        }

                        if (File.Exists(dgcpAttachPath2))
                        {
                            message.Attachments.Add(new Attachment(dgcpAttachPath2));
                            this.EventLog.WriteEntry("Attach file: " + dgcpAttachPath2);
                        }
                        break;
                }

                foreach (var toEmail in ConfigurationManager.AppSettings["BCC"].Split(',').Where(t => !string.IsNullOrEmpty(t)))
                {
                    message.Bcc.Add(toEmail);
                }
            }

            smtpClient.Send(message);
            this.EventLog.WriteEntry("SMS Gateway: Send weekly report to platform '" + platform + "'");

        }

        private double GetKPIInMonthAllPlatform()
        {
            var dt = new DataTable();

            var strConn = ConfigurationManager.ConnectionStrings["cmmsdbconnection"].ConnectionString;
            conn = new OracleConnection(strConn);
            var da = new OracleDataAdapter();
            var cmd = new OracleCommand();
            cmd.Connection = conn;

            cmd.CommandText = "AA_KPIInMonthAllPlatform";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("returnvalue", OracleType.Number).Direction = ParameterDirection.Output;

            da.SelectCommand = cmd;

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            return Convert.ToDouble(cmd.Parameters["returnvalue"].Value);
        }

        private double GetKPIInMonthOfPlatform(string platform)
        {
            var dt = new DataTable();

            var strConn = ConfigurationManager.ConnectionStrings["cmmsdbconnection"].ConnectionString;
            conn = new OracleConnection(strConn);
            var da = new OracleDataAdapter();
            var cmd = new OracleCommand();
            cmd.Connection = conn;

            cmd.CommandText = "AA_KPIInMonthOfPlatform";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("platform", OracleType.VarChar).Value = platform;
            cmd.Parameters.Add("returnvalue", OracleType.Number).Direction = ParameterDirection.Output;

            da.SelectCommand = cmd;

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            return Convert.ToDouble(cmd.Parameters["returnvalue"].Value);
        }

        private double GetKPIPlanInMonthAllPlatform()
        {
            var dt = new DataTable();

            var strConn = ConfigurationManager.ConnectionStrings["cmmsdbconnection"].ConnectionString;
            conn = new OracleConnection(strConn);
            var da = new OracleDataAdapter();
            var cmd = new OracleCommand();
            cmd.Connection = conn;

            cmd.CommandText = "AA_KPIPlanInMonthAllPlatform";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("returnvalue", OracleType.Number).Direction = ParameterDirection.Output;

            da.SelectCommand = cmd;

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            return Convert.ToDouble(cmd.Parameters["returnvalue"].Value);
        }

        private double GetKPIPlanInMonthOfPlatform(string platform)
        {
            var dt = new DataTable();

            var strConn = ConfigurationManager.ConnectionStrings["cmmsdbconnection"].ConnectionString;
            conn = new OracleConnection(strConn);
            var da = new OracleDataAdapter();
            var cmd = new OracleCommand();
            cmd.Connection = conn;

            cmd.CommandText = "AA_KPIPlanInMonthOfPlatform";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("platform", OracleType.VarChar).Value = platform;
            cmd.Parameters.Add("returnvalue", OracleType.Number).Direction = ParameterDirection.Output;

            da.SelectCommand = cmd;

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            return Convert.ToDouble(cmd.Parameters["returnvalue"].Value);
        }

        private double GetMonthCompleted(string platform, string discipline)
        {
            var dt = new DataTable();

            var strConn = ConfigurationManager.ConnectionStrings["cmmsdbconnection"].ConnectionString;
            conn = new OracleConnection(strConn);
            var da = new OracleDataAdapter();
            var cmd = new OracleCommand();
            cmd.Connection = conn;

            cmd.CommandText = "AA_MonthCompleted";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("platform", OracleType.VarChar).Value = platform;
            cmd.Parameters.Add("discipline", OracleType.VarChar).Value = discipline;
            cmd.Parameters.Add("returnvalue", OracleType.Number).Direction = ParameterDirection.Output;

            da.SelectCommand = cmd;

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            return Convert.ToDouble(cmd.Parameters["returnvalue"].Value);
        }

        private double GetMonthPlanned(string platform, string discipline)
        {
            var dt = new DataTable();

            var strConn = ConfigurationManager.ConnectionStrings["cmmsdbconnection"].ConnectionString;
            conn = new OracleConnection(strConn);
            var da = new OracleDataAdapter();
            var cmd = new OracleCommand();
            cmd.Connection = conn;

            cmd.CommandText = "AA_MonthPlanned";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("platform", OracleType.VarChar).Value = platform;
            cmd.Parameters.Add("discipline", OracleType.VarChar).Value = discipline;
            cmd.Parameters.Add("returnvalue", OracleType.Number).Direction = ParameterDirection.Output;

            da.SelectCommand = cmd;

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            return Convert.ToDouble(cmd.Parameters["returnvalue"].Value);
        }

        private double GetMonthPercent(string platform, string discipline)
        {
            var dt = new DataTable();

            var strConn = ConfigurationManager.ConnectionStrings["cmmsdbconnection"].ConnectionString;
            conn = new OracleConnection(strConn);
            var da = new OracleDataAdapter();
            var cmd = new OracleCommand();
            cmd.Connection = conn;

            cmd.CommandText = "AA_MonthPercent";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("platform", OracleType.VarChar).Value = platform;
            cmd.Parameters.Add("discipline", OracleType.VarChar).Value = discipline;
            cmd.Parameters.Add("returnvalue", OracleType.Number).Direction = ParameterDirection.Output;

            da.SelectCommand = cmd;

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            return Convert.ToDouble(cmd.Parameters["returnvalue"].Value);
        }

        private double GetPlanMonthCompleted(string platform, string discipline)
        {
            var dt = new DataTable();

            var strConn = ConfigurationManager.ConnectionStrings["cmmsdbconnection"].ConnectionString;
            conn = new OracleConnection(strConn);
            var da = new OracleDataAdapter();
            var cmd = new OracleCommand();
            cmd.Connection = conn;

            cmd.CommandText = "AA_PlanMonthCompleted";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("platform", OracleType.VarChar).Value = platform;
            cmd.Parameters.Add("discipline", OracleType.VarChar).Value = discipline;
            cmd.Parameters.Add("returnvalue", OracleType.Number).Direction = ParameterDirection.Output;

            da.SelectCommand = cmd;

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            return Convert.ToDouble(cmd.Parameters["returnvalue"].Value);
        }

        private double GetPlanMonthPlanned(string platform, string discipline)
        {
            var dt = new DataTable();

            var strConn = ConfigurationManager.ConnectionStrings["cmmsdbconnection"].ConnectionString;
            conn = new OracleConnection(strConn);
            var da = new OracleDataAdapter();
            var cmd = new OracleCommand();
            cmd.Connection = conn;

            cmd.CommandText = "AA_PlanMonthPlanned";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("platform", OracleType.VarChar).Value = platform;
            cmd.Parameters.Add("discipline", OracleType.VarChar).Value = discipline;
            cmd.Parameters.Add("returnvalue", OracleType.Number).Direction = ParameterDirection.Output;

            da.SelectCommand = cmd;

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            return Convert.ToDouble(cmd.Parameters["returnvalue"].Value);
        }

        private double GetPlanMonthPercent(string platform, string discipline)
        {
            var dt = new DataTable();

            var strConn = ConfigurationManager.ConnectionStrings["cmmsdbconnection"].ConnectionString;
            conn = new OracleConnection(strConn);
            var da = new OracleDataAdapter();
            var cmd = new OracleCommand();
            cmd.Connection = conn;

            cmd.CommandText = "AA_PlanMonthPercent";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("platform", OracleType.VarChar).Value = platform;
            cmd.Parameters.Add("discipline", OracleType.VarChar).Value = discipline;
            cmd.Parameters.Add("returnvalue", OracleType.Number).Direction = ParameterDirection.Output;

            da.SelectCommand = cmd;

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            return Convert.ToDouble(cmd.Parameters["returnvalue"].Value);
        }

    }
}
