using System;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Linq;
using System.Web;
using Aspose.Cells;

namespace EAM.XNKTBBBD
{
    /// <summary>
    /// Summary description for DownLoadFile
    /// </summary>
    public class XNKTF04b : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Stream stream = null;
            var strConn = ConfigurationManager.ConnectionStrings["cmmsdbconnection"].ConnectionString;
            var conn = new OracleConnection(strConn);
            var partCount = 0;
            var section = "Sect 1";
            try
            {
                string year = HttpContext.Current.Request.QueryString["year"];
                string org = HttpContext.Current.Request.QueryString["org"];
                if (!string.IsNullOrEmpty(year))
                {
                    var ds = new DataSet();

                    var da = new OracleDataAdapter();
                    var cmd = new OracleCommand();
                    cmd.Connection = conn;

                    cmd.CommandText = "AA_PM_CM_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("year", OracleType.VarChar).Value = year;
                    cmd.Parameters.Add("org", OracleType.VarChar).Value = org;
                    cmd.Parameters.Add("reportCur", OracleType.Cursor).Direction = ParameterDirection.Output;
                    section = "Sect 1.1";
                    da.SelectCommand = cmd;


                    conn.Open();
                    da.Fill(ds);

                    section = "Sect 1.3";
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var templateFolder = new DirectoryInfo(ConfigurationManager.AppSettings["TemplateFolder"]);
                        if (templateFolder.Exists && File.Exists(templateFolder + "F04b.xlsx"))
                        {
                            Workbook workbook = new Workbook(templateFolder + "F04b.xlsx");
                            section = "Sect 1.5";
                            var wsHome = workbook.Worksheets[0];
                            var wsData = workbook.Worksheets[2];
                            var wsDrawData = workbook.Worksheets[3];

                            wsDrawData.Cells.ImportDataTable(ds.Tables[0], false, 1, 0, ds.Tables[0].Rows.Count, ds.Tables[0].Columns.Count, true);
                            wsHome.Cells["C5"].Value = org;
                            wsHome.Cells["F5"].Value = year;
                            wsHome.Cells["M5"].Value = DateTime.Now.ToString("hh:mm tt dd/MM/yyyy");

                            section = "Sect 1.6";
                            var count = 0;

                            var dataList = ds.Tables[0].AsEnumerable().OrderBy(t => t["DEPARTMENT"].ToString()).ThenBy(t => t["TYPE1"].ToString()).ToList();
                            section = "Sect 1.7";
                            var dataGroupByDepartment = dataList.GroupBy(t => t["DEPARTMENT"]);
                            section = "Sect 1.8";
                            foreach (var departmentData in dataGroupByDepartment)
                            {
                                var departmentName = departmentData.Key;
                                var dataGroupByType = departmentData.GroupBy(t => t["TYPE1"]);
                                section = "Sect 1.9";
                                foreach (var typeData in dataGroupByType)
                                {
                                    var typeName = typeData.Key;
                                    var dataGroupByMonth = typeData.GroupBy(t => t["MONTH_COLUMNB"]);
                                    section = "Sect 1.10";
                                    foreach (var monthData in dataGroupByMonth)
                                    {
                                        var monthName = monthData.Key;
                                        wsData.Cells[1 + count, 0].Value = year;
                                        wsData.Cells[1 + count, 1].Value = monthName;
                                        wsData.Cells[1 + count, 2].Value = typeName;
                                        wsData.Cells[1 + count, 3].Value = departmentName;

                                        var monthDataGroupByWOID = monthData.GroupBy(t => t["WOID"]);
                                        var countWO = 0;
                                        foreach (var item in monthDataGroupByWOID)
                                        {
                                            countWO += item.Count();
                                        }
                                        wsData.Cells[1 + count, 4].Value = countWO;
                                        
                                        
                                        section = "Sect 1.11";
                                        wsData.Cells[1 + count, 5].Value = monthData.Sum(t => Convert.ToDouble(t["MATERIALCOST"]));
                                        wsData.Cells[1 + count, 6].Value = monthData.Where(t => t["TYPE2"].ToString() == "HUMAN_LIST").Sum(t => Convert.ToDouble(t["QTY"]));

                                        count += 1;
                                    }
                                }
                            }

                            //wsData.Cells.ImportDataTable(ds.Tables[0], false, 3, 0, ds.Tables[0].Rows.Count, 10, true);

                            

                            var filePath = ConfigurationManager.AppSettings["TemplateFolder"] + @"Export\F04b.xlsx";
                            workbook.Save(filePath);

                            section = "Sect 6";

                            if (File.Exists(filePath))
                            {
                                stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                                long bytesToRead = stream.Length;
                                HttpContext.Current.Response.ContentType = "application/octet-stream";
                                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(filePath));
                                while (bytesToRead > 0)
                                {
                                    if (HttpContext.Current.Response.IsClientConnected)
                                    {
                                        byte[] buffer = new Byte[10000];
                                        int length = stream.Read(buffer, 0, 10000);
                                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                                        HttpContext.Current.Response.Flush();
                                        bytesToRead = bytesToRead - length;
                                    }
                                    else
                                    {
                                        bytesToRead = -1;
                                    }
                                }
                            }
                        }
                    }

                    conn.Close();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(section + ": " + ex.Message);
            }
            finally
            {
                conn.Close();
                if (stream != null)
                    stream.Close();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}