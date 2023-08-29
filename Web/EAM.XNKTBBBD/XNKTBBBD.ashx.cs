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
    public class XNKTBBBD : IHttpHandler
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
                string woId = HttpContext.Current.Request.QueryString["woid"];
                if (!string.IsNullOrEmpty(woId))
                {
                    var ds = new DataSet();
                    var partds = new DataSet();

                   

                    var da = new OracleDataAdapter();
                    var cmd = new OracleCommand();
                    cmd.Connection = conn;

                    cmd.CommandText = "get_wo";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("wo_id", OracleType.VarChar).Value = woId;
                    cmd.Parameters.Add("catCur", OracleType.Cursor).Direction = ParameterDirection.Output;
                    section = "Sect 1.1";
                    da.SelectCommand = cmd;

                    var partda = new OracleDataAdapter();
                    var partcmd = new OracleCommand();
                    partcmd.Connection = conn;

                    partcmd.CommandText = "get_part";
                    partcmd.CommandType = CommandType.StoredProcedure;
                    partcmd.Parameters.Add("wo_id", OracleType.VarChar).Value = woId;
                    partcmd.Parameters.Add("catCur", OracleType.Cursor).Direction = ParameterDirection.Output;
                    section = "Sect 1.2";
                    partda.SelectCommand = partcmd;

                    conn.Open();
                    da.Fill(ds);
                    partda.Fill(partds);
                    section = "Sect 1.3";
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var maQT = ds.Tables[0].Rows[0]["MaQT"].ToString();
                        section = "Sect 1.4";
                        if (!string.IsNullOrEmpty(maQT))
                        {
                            var templateFolder = new DirectoryInfo(ConfigurationManager.AppSettings["TemplateFolder"]);
                            if (templateFolder.Exists && templateFolder.GetFiles().Any(t => t.Name.Contains(maQT)))
                            {
                                var templateFile = templateFolder.GetFiles().FirstOrDefault(t => t.Name.Contains(maQT));
                                if (templateFile != null)
                                {
                                    Workbook workbook = new Workbook(templateFile.FullName);
                                    section = "Sect 1.5";
                                    var wsDefine = workbook.Worksheets[0];
                                    var wsData = workbook.Worksheets[1];
                                    section = "Sect 1.6";
                                    wsData.Cells[wsDefine.Cells["B2"].Value.ToString()].Value = "Giàn / Объект: " + (ds.Tables[0].Rows[0]["Gian"] ?? string.Empty);
                                    section = "Sect 1.6.1";
                                    wsData.Cells[wsDefine.Cells["B3"].Value.ToString()].Value = "Số: " + (ds.Tables[0].Rows[0]["So"] ?? string.Empty);
                                    section = "Sect 1.6.2";
                                    wsData.Cells[wsDefine.Cells["B4"].Value.ToString()].Value = "Ngày: " + DateTime.Now.ToString("dd/MM/yyyy");
                                    section = "Sect 1.6.3";
                                    wsData.Cells[wsDefine.Cells["B5"].Value.ToString()].Value = "Tên thiết bị: " + (ds.Tables[0].Rows[0]["TenTB"] ?? string.Empty);
                                    section = "Sect 1.6.4";
                                    wsData.Cells[wsDefine.Cells["B6"].Value.ToString()].Value = "Ký mã hiệu: " + (ds.Tables[0].Rows[0]["KyMaHieu"] ?? string.Empty);
                                    section = "Sect 1.6.5";
                                    wsData.Cells[wsDefine.Cells["B7"].Value.ToString()].Value = "Vị trí lắp đặt:  " + (ds.Tables[0].Rows[0]["ViTri"] ?? string.Empty);
                                    section = "Sect 1.7";
                                    wsData.Cells[wsDefine.Cells["B8"].Value.ToString()].Value = "Dạng bảo dưỡng: " + (ds.Tables[0].Rows[0]["DangBD"] ?? string.Empty);
                                    wsData.Cells[wsDefine.Cells["B9"].Value.ToString()].Value = "Theo hướng dẫn số: " + (ds.Tables[0].Rows[0]["MaQT"] ?? string.Empty);
                                    wsData.Cells[wsDefine.Cells["B10"].Value.ToString()].Value = "Số giờ làm việc:" + (ds.Tables[0].Rows[0]["SoGioLamViec"] ?? string.Empty);
                                    wsData.Cells[wsDefine.Cells["B11"].Value.ToString()].Value = ds.Tables[0].Rows[0]["ChoPhepBD"] ?? string.Empty;

                                    section = "Sect 1.8";
                                    wsData.Cells[wsDefine.Cells["B12"].Value.ToString()].Value = "Người cho phép: " + (ds.Tables[0].Rows[0]["NguoiChoPhep_Ten"] ?? string.Empty);
                                    wsData.Cells[wsDefine.Cells["B13"].Value.ToString()].Value = "Chức danh: " + (ds.Tables[0].Rows[0]["NguoiChoPhep_CD"] ?? string.Empty);

                                    wsData.Cells[wsDefine.Cells["B15"].Value.ToString()].Value = "Họ và tên:" + (ds.Tables[0].Rows[0]["TruongNhomBaoDuong_Ten"] ?? string.Empty);
                                    wsData.Cells[wsDefine.Cells["B16"].Value.ToString()].Value = "Chức danh: " + (ds.Tables[0].Rows[0]["TruongNhomBaoDuong_CD"] ?? string.Empty);

                                    section = "Sect 2";
                                    if (partds.Tables.Count > 0 && partds.Tables[0].Rows.Count > 0)
                                    {
                                        partds.Tables[0].Columns.Add("STT",  System.Type.GetType("System.Int32")).SetOrdinal(0);
                                        var count = 1;
                                        foreach (DataRow item in partds.Tables[0].Rows)
                                        {
                                            item["STT"] = count;
                                            count += 1;
                                        }

                                        partCount = partds.Tables[0].Rows.Count;
                                        wsData.Cells.ImportDataTable(partds.Tables[0], false, Convert.ToInt32(wsDefine.Cells["B23"].Value.ToString()), 0, partds.Tables[0].Rows.Count, 1, true);

                                        var firstRow = Convert.ToInt32(wsDefine.Cells["B23"].Value.ToString());
                                        for (int i = 0; i < partCount; i++)
                                        {
                                            wsData.Cells.Merge(firstRow + i, Convert.ToInt32(wsDefine.Cells["B24"].Value.ToString()), 1, Convert.ToInt32(wsDefine.Cells["B25"].Value.ToString()) - Convert.ToInt32(wsDefine.Cells["B24"].Value.ToString()));

                                            wsData.Cells.Merge(firstRow + i, Convert.ToInt32(wsDefine.Cells["B25"].Value.ToString()), 1, Convert.ToInt32(wsDefine.Cells["B26"].Value.ToString()) - Convert.ToInt32(wsDefine.Cells["B25"].Value.ToString()));

                                            wsData.Cells.Merge(firstRow + i, Convert.ToInt32(wsDefine.Cells["B26"].Value.ToString()), 1, Convert.ToInt32(wsDefine.Cells["B27"].Value.ToString()) - Convert.ToInt32(wsDefine.Cells["B26"].Value.ToString()));

                                            wsData.Cells.Merge(firstRow + i, Convert.ToInt32(wsDefine.Cells["B27"].Value.ToString()), 1, Convert.ToInt32(wsDefine.Cells["B28"].Value.ToString()) - Convert.ToInt32(wsDefine.Cells["B27"].Value.ToString()));

                                            wsData.Cells.Merge(firstRow + i, Convert.ToInt32(wsDefine.Cells["B28"].Value.ToString()), 1, Convert.ToInt32(wsDefine.Cells["B29"].Value.ToString()) - Convert.ToInt32(wsDefine.Cells["B28"].Value.ToString()));

                                            wsData.Cells.Merge(firstRow + i, Convert.ToInt32(wsDefine.Cells["B29"].Value.ToString()), 1, Convert.ToInt32(wsDefine.Cells["B30"].Value.ToString()) - Convert.ToInt32(wsDefine.Cells["B29"].Value.ToString()));

                                            wsData.Cells.Merge(firstRow + i, Convert.ToInt32(wsDefine.Cells["B30"].Value.ToString()), 1, Convert.ToInt32(wsDefine.Cells["B31"].Value.ToString()) - Convert.ToInt32(wsDefine.Cells["B30"].Value.ToString()));

                                            wsData.Cells.Merge(firstRow + i, Convert.ToInt32(wsDefine.Cells["B31"].Value.ToString()), 1, Convert.ToInt32(wsDefine.Cells["B32"].Value.ToString()) - Convert.ToInt32(wsDefine.Cells["B31"].Value.ToString()));

                                            wsData.Cells[firstRow + i, Convert.ToInt32(wsDefine.Cells["B25"].Value.ToString())].Value = partds.Tables[0].Rows[i]["PAR_CODE"];
                                            wsData.Cells[firstRow + i, Convert.ToInt32(wsDefine.Cells["B26"].Value.ToString())].Value = partds.Tables[0].Rows[i]["PAR_DESC"];
                                            wsData.Cells[firstRow + i, Convert.ToInt32(wsDefine.Cells["B27"].Value.ToString())].Value = partds.Tables[0].Rows[i]["PAR_UOM"];
                                            wsData.Cells[firstRow + i, Convert.ToInt32(wsDefine.Cells["B28"].Value.ToString())].Value = partds.Tables[0].Rows[i]["trl_qty"];
                                            wsData.Cells[firstRow + i, Convert.ToInt32(wsDefine.Cells["B29"].Value.ToString())].Value = partds.Tables[0].Rows[i]["dongia"];
                                            wsData.Cells[firstRow + i, Convert.ToInt32(wsDefine.Cells["B30"].Value.ToString())].Value = partds.Tables[0].Rows[i]["thanhtien"];
                                            wsData.Cells[firstRow + i, Convert.ToInt32(wsDefine.Cells["B31"].Value.ToString())].Value = partds.Tables[0].Rows[i]["ghichu"];
                                        }
                                    }

                                    section = "Sect 3";

                                    var clc1 = wsDefine.Cells["B17"].Value.ToString().ToCharArray()[0];
                                    var cln1 = Convert.ToInt32(wsDefine.Cells["B17"].Value.ToString().Substring(1,
                                        wsDefine.Cells["B17"].Value.ToString().Length - 1));
                                    var clc11 = wsDefine.Cells["B18"].Value.ToString().ToCharArray()[0];
                                    var cln11 = Convert.ToInt32(wsDefine.Cells["B18"].Value.ToString().Substring(1,
                                        wsDefine.Cells["B18"].Value.ToString().Length - 1));

                                    var clc2 = wsDefine.Cells["B19"].Value.ToString().ToCharArray()[0];
                                    var cln2 = Convert.ToInt32(wsDefine.Cells["B19"].Value.ToString().Substring(1,
                                        wsDefine.Cells["B19"].Value.ToString().Length - 1));
                                    var clc22 = wsDefine.Cells["B20"].Value.ToString().ToCharArray()[0];
                                    var cln22 = Convert.ToInt32(wsDefine.Cells["B20"].Value.ToString().Substring(1,
                                        wsDefine.Cells["B20"].Value.ToString().Length - 1));

                                    var clc3 = wsDefine.Cells["B21"].Value.ToString().ToCharArray()[0];
                                    var cln3 = Convert.ToInt32(wsDefine.Cells["B21"].Value.ToString().Substring(1,
                                        wsDefine.Cells["B21"].Value.ToString().Length - 1));

                                    section = "Sect 4";

                                    wsData.Cells[clc1.ToString() + (cln1 + partCount)].Value = "    Người chịu trách nhiệm bảo dưỡng: " + (ds.Tables[0].Rows[0]["NguoiBaoDuong_Ten"] ?? string.Empty);
                                    wsData.Cells[clc11.ToString() + (cln11 + partCount)].Value = "Chức danh: " +( ds.Tables[0].Rows[0]["NguoiBaoDuong_CD"] ?? string.Empty);

                                    wsData.Cells[clc2.ToString() + (cln2 + partCount)].Value = "    Người chịu trách nhiệm vận hành: " + (ds.Tables[0].Rows[0]["NguoiVanHanh_Ten"] ?? string.Empty);
                                    wsData.Cells[clc22.ToString() + (cln22 + partCount)].Value = "Chức danh: " + (ds.Tables[0].Rows[0]["NguoiVanHanh_CD"] ?? string.Empty);

                                    wsData.Cells[clc3.ToString() + (cln3 + partCount)].Value = "    Lãnh đạo công trình/Руководитель объекта: " + (ds.Tables[0].Rows[0]["LanhDaoCT"] ?? string.Empty);

                                    section = "Sect 5";

                                    wsDefine.SetVisible(false, true);
                                    var filePath = ConfigurationManager.AppSettings["TemplateFolder"] + @"Export\" +
                                                   ds.Tables[0].Rows[0]["So"].ToString() + "_" + maQT + ".xlsx";
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
                        }
                    }

                    conn.Close();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(section);
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