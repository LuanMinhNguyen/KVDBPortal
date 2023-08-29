using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using EAM.LDAWebAPICore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace EAM.LDAWebAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly LDADbContext _context;
        public UploadFileController(IConfiguration configuration, LDADbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile([FromForm] FileInformation objFile)
        {
            var outgoingPath = _configuration.GetValue<string>("MySettings:OutgoingPath");
            var incomingPath = _configuration.GetValue<string>("MySettings:IncomingPath");
            var eamDocUploadPath = _configuration.GetValue<string>("MySettings:EAMDocUploadPath");
            var strConn = _configuration.GetSection("ConnectionStrings").GetSection("SqlDBConn").Value;
            var ds = new DataSet();
            var ds1 = new DataSet();
            var conn = new SqlConnection(strConn);
            var da = new SqlDataAdapter();
            var da1 = new SqlDataAdapter();
            var cmd = new SqlCommand();
            var cmd1 = new SqlCommand();

            try
            {
                if (objFile.file.Length > 0)
                {
                    if (!Directory.Exists(outgoingPath))
                    {
                        Directory.CreateDirectory(outgoingPath);
                    }

                    if (!Directory.Exists(incomingPath))
                    {
                        Directory.CreateDirectory(incomingPath);
                    }
                     
                    if (!Directory.Exists(eamDocUploadPath))
                    {
                        Directory.CreateDirectory(eamDocUploadPath);
                    }

                    var code = objFile.file.FileName.Split('_')[0];
                    var fileDescription = string.Empty;

                    // Get file description by WO Code
                    if (objFile.file.FileName.Contains("CMMS_PhieuYeuCauVatTu_")) 
                    {
                        cmd1.Connection = conn;
                        cmd1.CommandText = "SELECT PIC_DESC FROM R5PICKLISTS WHERE PIC_CODE = " + code;
                        cmd1.CommandType = CommandType.Text;

                        da1.SelectCommand = cmd1;
                        conn.Open();
                        da1.Fill(ds1);
                        conn.Close();
                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            fileDescription = ds1.Tables[0].Rows[0]["PIC_DESC"].ToString();
                        }
                    }
                    // --------------------------------------------------------------

                    using (FileStream stream = System.IO.File.Create(outgoingPath + (string.IsNullOrEmpty(fileDescription) ? objFile.file.FileName : fileDescription + ".pdf")))
                    {

                        objFile.file.CopyTo(stream);
                        stream.Flush();
                        var docType = string.Empty;
                        var abstractDocument = string.Empty;
                        var docClass = string.Empty;
                        var LVB1JsonContent = string.Empty;

                        if (objFile.file.FileName.Contains("CMMS_NghiemThuGioHoatDongThietBi_"))
                        {
                            docType = "LVB5";
                            abstractDocument = "Nghiệm thu giờ hoạt động thiết bị";

                            docClass = "TSVH";
                        }
                        else if (objFile.file.FileName.Contains("CMMS_NghiemThuTongTheGioHoatDongThietBi_"))
                        {
                            docType = "LVB5";
                            abstractDocument = "Nghiệm thu tổng thể giờ hoạt động thiết bị";

                            docClass = "TSVH";
                        }
                        else if (objFile.file.FileName.Contains("CMMS_BM01_"))
                        {
                            docType = "LVB16";
                            abstractDocument = "BM-01: Kế hoạch BDSC tháng/quý/năm";

                            docClass = "BM01";
                        }
                        else if (objFile.file.FileName.Contains("CMMS_BM01PXTM_"))
                        {
                            docType = "LVB16";
                            abstractDocument = "BM-01: Kế hoạch BDSC tháng/quý/năm (PXTM)";

                            docClass = "BM01";
                        }
                        else if (objFile.file.FileName.Contains("CMMS_BM02_"))
                        {
                            docType = "LVB16";
                            abstractDocument = "BM-02: Kế hoạch sửa chữa, bảo dưỡng";

                            docClass = "BM02";
                        }
                        else if (objFile.file.FileName.Contains("CMMS_BM14_"))
                        {
                            docType = "LVB5";
                            abstractDocument = "BM-14: Tổng hợp nghiệm thu tổng thể sữa chữa, bảo dưỡng thường xuyên";

                            docClass = "BM14";
                        }
                        else if (objFile.file.FileName.Contains("CMMS_BM14PXTM_"))
                        {
                            docType = "LVB5";
                            abstractDocument = "BM-14: Tổng hợp nghiệm thu tổng thể sữa chữa, bảo dưỡng thường xuyên (PXTM)";

                            docClass = "BM14";
                        }
                        else if (objFile.file.FileName.Contains("CMMS_BM141_"))
                        {
                            docType = "LVB5";
                            abstractDocument = "BM-14.1: Nghiệm thu tổng thể SCBD theo kế hoạch";

                            docClass = "BM14.1";
                        }
                        else if (objFile.file.FileName.Contains("CMMS_BM142_"))
                        {
                            docType = "LVB5";
                            abstractDocument = "BM-14.2: Nghiệm thu tổng thể SCBD theo kế hoạch";

                            docClass = "BM14.2";
                        }
                        else if (objFile.file.FileName.Contains("CMMS_BBNghiemThuTongTheGiaiThePhucHoiThietBi_"))
                        {
                            docType = "LVB5";
                            abstractDocument = "Biên bản nghiệm thu tổng thể giải thể và phục hồi thiết bị";

                            docClass = "BBPH";
                        }
                        else if (objFile.file.FileName.Contains("CMMS_BBNghiemThuTongTheGiaCongThietBi_"))
                        {
                            docType = "LVB5";
                            abstractDocument = "Nghiệm thu tổng thể gia công thiết bị";

                            docClass = "BBGC";
                        }
                        else if (objFile.file.FileName.Contains("CMMS_PhieuGiaoViec_"))
                        {
                            docType = "LVB15";
                            abstractDocument = "Phiếu giao việc";

                            docClass = "PGV";
                        }
                        else if (objFile.file.FileName.Contains("CMMS_PhieuDatLam_"))
                        {
                            docType = "LVB15";
                            abstractDocument = "Phiếu đặt làm";

                            docClass = "PDL";
                        }
                        else if (objFile.file.FileName.Contains("CMMS_BM13_"))
                        {
                            docType = "LVB16";
                            abstractDocument = "BM-13: Kế hoạch vật tư theo tháng";

                            docClass = "BM13";
                        }
                        else if (objFile.file.FileName.Contains("CMMS_BBCon_"))
                        {
                            docType = "LVB15";
                            abstractDocument = "Phiếu công việc (bao gồm biên bản 4 bước)";

                            docClass = "WO";
                        }
                        else if (objFile.file.FileName.Contains("CMMS_KeHoachHuyDongGioMay_"))
                        {
                            docType = "LVB16";
                            abstractDocument = "Kế hoạch huy động giờ máy";

                            docClass = "KHVH";
                        }
                        else if (objFile.file.FileName.Contains("CMMS_BM04TKV_"))
                        {
                            docType = "LVB1";
                            abstractDocument = "Báo cáo TKV biểu mâu BM04";

                            docClass = "BM04";
                        }
                        else if (objFile.file.FileName.Contains("CMMS_PhieuYeuCauVatTu_"))
                        {
                            docType = "LVB15";
                            abstractDocument = "Phiếu yêu cầu vật tư";

                            docClass = "YCVT";

                            // Get person need to sign information
                            cmd.Connection = conn;
                            cmd.CommandText = @"SELECT * FROM (
                                            SELECT PER_UDFCHAR02 userName, 0 deptLevel, 'userCreate' actionKey, 'false' displaySign FROM R5PERSONNEL INNER JOIN R5PROPERTYVALUES ON PRV_VALUE = PER_CODE WHERE PRV_PROPERTY = 'PIC_PER5' AND PRV_RENTITY = 'PICK' AND PRV_CODE = '" + code + @"'
                                            UNION
                                            SELECT PER_UDFCHAR02 userName, 1 deptLevel, 'action.name.sign' actionKey, 'true' displaySign FROM R5PERSONNEL INNER JOIN R5PROPERTYVALUES ON PRV_VALUE = PER_CODE WHERE PRV_PROPERTY = 'PIC_PER1' AND PRV_RENTITY = 'PICK' AND PRV_CODE = '" + code + @"'
                                            UNION
                                            SELECT PER_UDFCHAR02 userName, 2 deptLevel, 'action.name.approve' actionKey, 'false' displaySign FROM R5PERSONNEL INNER JOIN R5PROPERTYVALUES ON PRV_VALUE = PER_CODE WHERE PRV_PROPERTY = 'PIC_PER6' AND PRV_RENTITY = 'PICK' AND PRV_CODE = '" + code + @"'
                                            UNION
                                            SELECT PER_UDFCHAR02 userName, 3 deptLevel, 'action.name.sign' actionKey, 'true' displaySign FROM R5PERSONNEL INNER JOIN R5PROPERTYVALUES ON PRV_VALUE = PER_CODE WHERE PRV_PROPERTY = 'PIC_PER2' AND PRV_RENTITY = 'PICK' AND PRV_CODE = '" + code + @"'
                                            UNION
                                            SELECT PER_UDFCHAR02 userName, 4 deptLevel, 'action.name.sign' actionKey, 'true' displaySign FROM R5PERSONNEL INNER JOIN R5PROPERTYVALUES ON PRV_VALUE = PER_CODE WHERE PRV_PROPERTY = 'PIC_PER3' AND PRV_RENTITY = 'PICK' AND PRV_CODE = '" + code + @"'
                                            UNION
                                            SELECT PER_UDFCHAR02 userName, 5 deptLevel, 'action.name.sign' actionKey, 'true' displaySign FROM R5PERSONNEL INNER JOIN R5PROPERTYVALUES ON PRV_VALUE = PER_CODE WHERE PRV_PROPERTY = 'PIC_PER4' AND PRV_RENTITY = 'PICK' AND PRV_CODE = '" + code + @"'
                                            UNION
                                            SELECT 'vanthu' userName, 6 deptLevel, 'action.name.sign' actionKey, 'true' displaySign FROM DUAL
                                            UNION
                                            SELECT PER_UDFCHAR02 userName, 7 deptLevel, 'action.name.publish' actionKey, 'false' displaySign FROM R5PERSONNEL INNER JOIN R5PROPERTYVALUES ON PRV_VALUE = PER_CODE WHERE PRV_PROPERTY = 'PIC_PER7' AND PRV_RENTITY = 'PICK' AND PRV_CODE = '" + code + @"'
                                            ) TEMP
                                            ORDER BY deptLevel";
                            cmd.CommandType = CommandType.Text;

                            da.SelectCommand = cmd;
                            conn.Open();
                            da.Fill(ds);
                            conn.Close();

                            

                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                var userCreated = string.Empty;
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        LVB1JsonContent =
                                            @"{
                                                ""userCreate"": """ + ds.Tables[0].Rows[i]["userName"].ToString() + @""",
                                                ""code"": """",
                                                ""docType"": """ + docType + @""",
                                                ""abstractDocument"": """ + abstractDocument + @""",
                                                ""deadLineToReply"": """ + DateTime.Now.AddDays(3).ToString("dd/MM/yyyy") + @""",

                                                ""receiveToKnow"": [
                                                ],

                                                ""numberOfDoc"": 2,
                                                ""security"": ""DM2"",
                                                ""priority"": ""DK2"",
                                                ""isDocOutReply"": true,
                                                ""isLawDoc"": true,
                                                ""isParallelSigning"": true,

                                                ""lstDeptUserGetSign"": [
                                                ";
                                    }
                                    else if (i == ds.Tables[0].Rows.Count - 1)
                                    {
                                        LVB1JsonContent += @"
                                                    {
                                                      ""userName"": """ + ds.Tables[0].Rows[i]["userName"].ToString() + @""",
                                                      ""deptLevel"":" + ds.Tables[0].Rows[i]["deptLevel"].ToString() + @",
                                                      ""actionKey"":""" + ds.Tables[0].Rows[i]["actionKey"].ToString() + @""",
                                                      ""displaySign"":" + ds.Tables[0].Rows[i]["displaySign"].ToString() + @"
                                                    }";
                                    }
                                    else
                                    {
                                        LVB1JsonContent += @"
                                                    {
                                                      ""userName"": """ + ds.Tables[0].Rows[i]["userName"].ToString() + @""",
                                                      ""deptLevel"":" + ds.Tables[0].Rows[i]["deptLevel"].ToString() + @",
                                                      ""actionKey"":""" + ds.Tables[0].Rows[i]["actionKey"].ToString() + @""",
                                                      ""displaySign"":" + ds.Tables[0].Rows[i]["displaySign"].ToString() + @"
                                                    },";
                                    }
                                }

                                LVB1JsonContent += @"
                                                ]
                                            }";
                            }
                            // ------------------------------------------------------------------------------
                        }

                        var jsonContent =
                            @"{
                                ""userCreate"": """",
                                ""code"": """",
                                ""docType"": """ + docType + @""",
                                ""abstractDocument"": """ + abstractDocument + @""",
                                ""deadLineToReply"": """ + DateTime.Now.AddDays(3).ToString("dd/MM/yyyy") + @""",

                                ""receiveToKnow"": [
                                ],

                                ""security"": ""DM1"",
                                ""priority"": ""DK2"",
                                ""isParallelSigning"": false,

                                ""lstDeptUserGetSign"": [
                                ]
                            }";

                        var jsonFileName = string.IsNullOrEmpty(fileDescription) ? objFile.file.FileName.Split('.')[0] + ".json" : fileDescription + ".json";
                        using (StreamWriter sw = System.IO.File.CreateText(outgoingPath + jsonFileName))
                        {
                            if (!string.IsNullOrEmpty(LVB1JsonContent))
                            {
                                await sw.WriteLineAsync(LVB1JsonContent);
                            }
                            else
                            {
                                await sw.WriteLineAsync(jsonContent);
                            }
                            
                            await sw.FlushAsync();
                        }


                        // Add document attachment
                        var stproc = "EXEC TT_Upload_Document '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "', N'" +
                                     abstractDocument + "', '" + docClass + "', N'" + (string.IsNullOrEmpty(fileDescription) ? objFile.file.FileName : fileDescription + ".pdf") + "'";
                        //this._context.Database.ExecuteSqlRaw(stproc);
                        // -----------------------------------------------------

                    }

                    var fileInfor = new FileInfo(outgoingPath + (string.IsNullOrEmpty(fileDescription) ? objFile.file.FileName : fileDescription + ".pdf"));
                    if (fileInfor.Exists)
                    {
                        fileInfor.CopyTo(eamDocUploadPath + (string.IsNullOrEmpty(fileDescription) ? objFile.file.FileName : fileDescription + ".pdf"));
                    }

                    return StatusCode(StatusCodes.Status200OK, outgoingPath + (string.IsNullOrEmpty(fileDescription) ? objFile.file.FileName : fileDescription + ".pdf"));
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Don't have file to upload!");
                }
            }
            catch (Exception ex)
            {
                //LogException();
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
                //return ex.Message.ToString();
            }
        }
    }
}
