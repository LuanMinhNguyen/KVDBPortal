using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using EAM.LDAWebAPICore.Models;
using Microsoft.Extensions.Configuration;

namespace EAM.LDAWebAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private IConfiguration _configuration;
        public UploadFileController(IConfiguration configuration )
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<string> UploadFile([FromForm] FileInformation objFile)
        {
            var outgoingPath = _configuration.GetValue<string>("MySettings:OutgoingPath");
            var incomingPath = _configuration.GetValue<string>("MySettings:IncomingPath");

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



                    using (FileStream stream = System.IO.File.Create(outgoingPath + objFile.file.FileName))
                    {
                        objFile.file.CopyTo(stream);
                        stream.Flush();
                        var docType = string.Empty;
                        var abstractDocument = string.Empty;
                        var docClass = string.Empty;

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
                        }

                        var jsonContent = 
                            @"{
                                ""userCreate"": """",
                                ""code"": """",
                                ""docType"": """+ docType + @""",
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

                        var jsonFileName = objFile.file.FileName.Split('.')[0] + ".json";
                        using (StreamWriter sw = System.IO.File.CreateText(outgoingPath + jsonFileName))
                        {
                            await sw.WriteLineAsync(jsonContent);
                            await sw.FlushAsync();
                        }

                        // Add document attachment
                        
                        // -----------------------------------------------------

                        return outgoingPath + objFile.file.FileName;
                    }
                }
                else
                {
                    return "Don't have file to upload!";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}
