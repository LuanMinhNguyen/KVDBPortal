using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Aspose.Cells;
using EAM.Business.Services;

namespace EAM.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Guid.NewGuid());

            var temp = DateTime.Now.ToString("hh:mm tt dd/MM/yyyy");

            //var dbService = new EAMStoreProcedureService();
            //var ds = dbService.GetDataSet("GetDuplicateDoc");

            //var temp = DateTime.Now.Hour;

            Workbook workbook = new Workbook(@"D:\Form mẫu F04b final.xlsx");
            //var wsDefine = workbook.Worksheets[0];
            var wsData = workbook.Worksheets[2];

           // wsData.Cells.MoveRange(new CellArea() {6, 42, 0, 0}, 45, 6);

            //var range = wsData.GetSelectedRanges()[0]; ;//wsData.Cells.CreateRange(1, 0, 46, 7);
            //range.AddArea(1,0,46,7);
            workbook.Save(@"D:\Form mẫu F04b final_test.xlsx");
            //var section = wsDefine.Cells["B2"].Value.ToString();


            Console.ReadLine();



            //var ds = new DataSet();
            //var strConn = ConfigurationManager.ConnectionStrings["cmmsdbconnection"].ConnectionString;
            //var conn = new OracleConnection(strConn);
            //var da = new OracleDataAdapter();
            //var cmd = new OracleCommand();
            //cmd.Connection = conn;


            //cmd.CommandText = "get_wo";
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("wo_id", OracleType.VarChar).Value = "21484";
            //cmd.Parameters.Add("catCur", OracleType.Cursor).Direction = ParameterDirection.Output;

            //da.SelectCommand = cmd;

            //da.SelectCommand = cmd;
            //try
            //{
            //    Console.WriteLine("Open");

            //    //conn.Open();
            //    Console.WriteLine("Fill");

            //    //da.Fill(ds);
            //    Console.WriteLine("Close");

            //    conn.Close();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    Console.ReadLine();

            //}

            Console.ReadLine();
////            var attachFiles = Directory.GetFiles(@"C:\ElasticSearchDoc", "*.pdf");
////            foreach (var attachFile in attachFiles)
////            {
////            }

////            var dt = new DataTable();

////                var strConn = ConfigurationManager.ConnectionStrings["cmmsdbconnection"].ConnectionString;
////                var conn = new OracleConnection(strConn);
////                var da = new OracleDataAdapter();
////                var cmd = new OracleCommand();
////                cmd.Connection = conn;

////                cmd.CommandText = "AA_KPIInMonthAllPlatform";
////                cmd.CommandType = CommandType.StoredProcedure;
////                cmd.Parameters.Add("returnvalue", OracleType.Number).Direction = ParameterDirection.Output;

////                da.SelectCommand = cmd;

////                conn.Open();
////                cmd.ExecuteNonQuery();
////                conn.Close();

////                var temp = Convert.ToDouble(cmd.Parameters["returnvalue"].Value);

////            Workbook workbook = new Workbook("Schedule.xlsx");
////            var monthlyData = workbook.Worksheets[0];
////            var weeklyData = workbook.Worksheets[1];

////            monthlyData.Cells["A2"].Value = DateTime.Now.Month;
////            monthlyData.Cells["B2"].Value = true;



////            workbook.Save("Schedule.xlsx");

////            //var adPath = "LDAP://172.16.20.11";
////            //var domain = "hongngochospital.vn";
////            //var adAuth = new LdapAuthentication(adPath);
////            //var username = "notieam";
////            //var password = "123456abc1";
////            //if (adAuth.IsAuthenticated(domain, username, password))
////            //{
////            //    var temp = true;
////            //}

////            //var regex = new Regex(@"(\d{1,4}([.\-/])\d{1,2}([.\-/])\d{1,4})");
////            //var temp = "Cấp cho Kho Khoa XN - CĐHA - TDCN (699 Trần Hưng Đạo) theo Bảng PP HCVT 01-01-2020";
////            //var match = regex.Match(temp);
////            DateTimeFormatInfo dtfi = CultureInfo.GetCultureInfo("en-US").DateTimeFormat;
////            var smtpClient = new SmtpClient
////            {
////                DeliveryMethod = SmtpDeliveryMethod.Network,
////                UseDefaultCredentials = false,
////                EnableSsl = false,
////                //Host = "mail.vietsov.com.vn",
////                Host = "mail.biendongpoc.vn",
////                Port = 25,
////                Credentials = new NetworkCredential("doclib@biendongpoc.vn", "123456a@doc")
////                //Credentials = new NetworkCredential("support.cmms@vietsov.com.vn", "Ingr.12345")
////            };

////            var message = new MailMessage();
////            message.From = new MailAddress("doclib@biendongpoc.vn", "CMMS Support");
////            //message.From = new MailAddress("support.cmms@vietsov.com.vn", "CMMS Support");
            
////            message.BodyEncoding = new UTF8Encoding();
////            message.IsBodyHtml = true;

////            var body1 = @"<table style='width: 610px'>
////    <tr>
////        <td>
////            ############### NguyenVanHong - Test Email notification ... ###############<br/>
////            Dear DGCP Platform,<br/>
////            Notice from InforEAM system about the implementation of maintenance plan in the monthly with the following information as below:
////        </td>
////    </tr>
////    <tr>
////        <td align='center'>
////            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[100],backgroundColor:'cornflowerblue'}]}}"" width='250px'><br/>
////            Percent WOs Completed of DGCP in last month: 100%
////        </td>
////    </tr>
////    <tr>
////        <td>
////            <table border='0' cellspacing='0' cellpadding='0' width='600px' style='border-collapse: collapse'>
////                <tr style='height: 30px'>
////                    <td width='25%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
////                        <b><span style='color: white'>Discipline</span></b>
////                    </td>
////                    <td width='25%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
////                        <b><span style='color: white'>Planned</span></b>
////                    </td>
////                    <td width='25%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
////                        <b><span style='color: white'>Completed</span></b>
////                    </td>
////                    <td width='25%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
////                        <b><span style='color: white'>Percent</span></b>
////                    </td>
////                </tr>
////                <tr style='height: 40px'>
////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                        <b>ELE</b>
////                    </td>
////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                        16
////                    </td>
////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                        16
////                    </td>
////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                        100%
////                    </td>
////                </tr>

////                <tr style='height: 40px'>
////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                        <b>INS</b>
////                    </td>

////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                        14
////                    </td>

////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                        14
////                    </td>

////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                        100%
////                    </td>
////                </tr>

////                <tr style='height: 40px'>
////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                        <b>MEC</b>
////                    </td>
////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                        7
////                    </td>
////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                        7
////                    </td>
////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                        100%
////                    </td>
////                </tr>

////                <tr style='height: 40px'>
////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                        <b>PRO</b>
////                    </td>

////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                        10
////                    </td>

////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                        10
////                    </td>

////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                        100%
////                    </td>
////                </tr>

////                <tr style='height: 40px'>
////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                        <b>SAF</b>
////                    </td>
////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                        23
////                    </td>
////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                        23
////                    </td>
////                    <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                        100%
////                    </td>
////                </tr>

////            </table>
////        </td>
////    </tr>
////    <tr>
////        <td>
////            To update any information, please log in the InforEAM system via the link: http://cmms.vietsov.com.vn/web/base/logout?tenant=XNK
////            <br/>Thanks & Regards,
////            <br/>(Reported by InforEAM)
////        </td>
////    </tr>
////</table>";

////            var body2 = @"<table style='width: 800px'>
////        <tr>
////            <td>
////                ############### NguyenVanHong - Test Email notification ... ###############<br/>
////                Dear All,<br />
////                Report from InforEAM system about the implementation of maintenance plan in the monthly with the following information as below:
////            </td>
////        </tr>
////        <tr>
////            <td>
////                <ul style='list-style-type: square;'>
////                    <li><b>The implementation of Wos in last month:</b></li>
////                </ul>
////                <table style='width: 800px;'>
////                    <tr>
////                        <td width='25%' align='center'>
////                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[99],backgroundColor:'coral'}]}}"" width='200px'><br />
////                            Percent WOs Completed in last month: 99%
////                        </td>
////                        <td width='25%' align='center'>
////                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[100],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
////                            Percent WOs Completed of MKC in last month: 100%
////                        </td>
////                        <td width='25%' align='center'>
////                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[100],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
////                            Percent WOs Completed of DGCP in last month: 100%
////                        </td>
////                        <td width='25%' align='center'>
////                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[98],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
////                            Percent WOs Completed of CCP in last month: 98%
////                        </td>
////                    </tr>
////                </table>

////            </td>
////        </tr>
////        <tr>
////            <td>
////                <table border='0' cellspacing='0' cellpadding='0' width='800px' style='border-collapse: collapse'>
////                    <tr style='height: 30px'>
////                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
////                            <b><span style='color: white'>Platform</span></b>
////                        </td>
////                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
////                            <b><span style='color: white'>Discipline</span></b>
////                        </td>
////                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
////                            <b><span style='color: white'>Planned</span></b>
////                        </td>
////                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
////                            <b><span style='color: white'>Completed</span></b>
////                        </td>
////                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
////                            <b><span style='color: white'>Percent</span></b>
////                        </td>
////                    </tr>
////                    <tr style='height: 33px'>
////                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>CCP</b>
////                        </td>

////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>ELE</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            117
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            117
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            100%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            <b>INS</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            19
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            19
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            100%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>MEC</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            67
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            66
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            98%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            <b>PRO</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            28
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            28
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            100%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>SAF</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            29
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            29
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            100%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>MKC</b>
////                        </td>

////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>ELE</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            71
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            71
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            100%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            <b>INS</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            15
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            15
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            100%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>MEC</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            8
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            8
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            100%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            <b>PRO</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            17
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            17
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            100%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>SAF</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            6
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            6
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            100%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>DGCP</b>
////                        </td>

////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>ELE</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            16
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            16
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            100%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            <b>INS</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            14
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            14
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            100%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>MEC</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            14
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            14
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            100%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            <b>PRO</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            10
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            10
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            100%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>SAF</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            23
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            23
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            100%
////                        </td>
////                    </tr>

////                </table>
////            </td>
////        </tr>

////        <tr>
////            <td>
////                <ul style='list-style-type: square;'>
////                    <li><b>Wos plan in next month:</b></li>
////                </ul>
////                <table style='width: 800px;'>
////                    <tr>
////                        <td width='25%' align='center'>
////                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[6],backgroundColor:'coral'}]}}"" width='200px'><br />
////                            Percent WOs Completed in this month: 6%
////                        </td>
////                        <td width='25%' align='center'>
////                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[7],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
////                            Percent WOs Completed of MKC in this month: 7%
////                        </td>
////                        <td width='25%' align='center'>
////                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[3],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
////                            Percent WOs Completed of DGCP in this month: 3%
////                        </td>
////                        <td width='25%' align='center'>
////                            <img src=""https://quickchart.io/chart?c={type:'radialGauge',data:{datasets:[{data:[6],backgroundColor:'cornflowerblue'}]}}"" width='200px'><br />
////                            Percent WOs Completed of CCP in this month: 6%
////                        </td>
////                    </tr>
////                </table>

////            </td>
////        </tr>
////        <tr>
////            <td>
////                <table border='0' cellspacing='0' cellpadding='0' width='800px' style='border-collapse: collapse'>
////                    <tr style='height: 30px'>
////                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
////                            <b><span style='color: white'>Platform</span></b>
////                        </td>
////                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
////                            <b><span style='color: white'>Discipline</span></b>
////                        </td>
////                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
////                            <b><span style='color: white'>Planned</span></b>
////                        </td>
////                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
////                            <b><span style='color: white'>Completed</span></b>
////                        </td>
////                        <td width='20%' valign='middle' align='center' style='border: solid #5b9bd5 1.0pt; background: #5b9bd5;'>
////                            <b><span style='color: white'>Percent</span></b>
////                        </td>
////                    </tr>
////                    <tr style='height: 33px'>
////                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>CCP</b>
////                        </td>

////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>ELE</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            100
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            0
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            0%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            <b>INS</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            39
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            0
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            0%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>MEC</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            71
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            0
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            0%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            <b>PRO</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            25
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            7
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            28%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>SAF</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            30
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            0
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            0%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>MKC</b>
////                        </td>

////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>ELE</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            85
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            12
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            14%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            <b>INS</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            17
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            0
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            0%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>MEC</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            1
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            1
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            100%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            <b>PRO</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            76
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            0
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            0%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>SAF</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            10
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            1
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            10%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td rowspan='5' valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>DGCP</b>
////                        </td>

////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>ELE</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            12
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            0
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            0%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            <b>INS</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            10
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            0
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            0%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>MEC</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            17
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            0
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            0%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            <b>PRO</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            13
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            2
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt;'>
////                            15%
////                        </td>
////                    </tr>

////                    <tr style='height: 33px'>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            <b>SAF</b>
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            23
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            0
////                        </td>
////                        <td valign='middle' align='center' style='border: solid #9cc2e5 1.0pt; background: #deeaf6;'>
////                            0%
////                        </td>
////                    </tr>

////                </table>
////            </td>
////        </tr>
////    <tr>
////        <td>
////            <i>The Attachment As Below:</i><br />
////            <ul style='list-style-type: square;'>
////                <li>P-HT-006/F9: Báo cáo công việc bảo dưỡng định kỳ thiết bị tháng này.</li>
////                <li>P-HT-006/F3a: Kế hoạch công việc bảo dưỡng định kỳ và sửa chữa thiết bị chính tháng tới.</li>
////                <li>P-HT-006/F3b: Kế hoạch công việc bảo dưỡng định kỳ và sửa chữa tháng tới.</li>
////            </ul>
////        </td>
////    </tr>
////        <tr>
////            <td>
////                For Detail information, Please login to InforEAM system via the link: http://cmms.vietsov.com.vn/web/base/logindisp?tenant=XNK
////                <br/>
////                Thanks & Regards,
////                <br/>
////                (Reported by InforEAM)
////            </td>
////        </tr>
////    </table>";

////            //message.Body = body1;
////            //message.Subject = "Notice from InforEAM system about the implementation of maintenance plan in " + dtfi.GetMonthName(DateTime.Now.Month);

////            message.Body = body2;
////            message.Subject = "InforEAM: Báo cáo kết quả bảo dưỡng sửa chữa tháng " + DateTime.Now.AddMonths(-1).ToString("MM.yyyy") + " và kế hoạch bảo dưỡng sửa chữa tháng " + DateTime.Now.ToString("MM.yyyy")
////                ;
////            message.To.Add("nguyenvanhong@truetech.com.vn");
////            //message.CC.Add("inforprojects@truetech.com.vn");
////            smtpClient.Send(message);
        }
    }
}
