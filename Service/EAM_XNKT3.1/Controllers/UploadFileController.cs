using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using EAM_XNKT3._1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace EAM_XNKT3._1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private IConfiguration _configuration;
        public UploadFileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<string> UploadFile([FromForm] FileInformation objFile)
        {
            var eamDocUploadPath = _configuration.GetValue<string>("MySettings:EAMDocUploadPath");
            var oracleConn = _configuration.GetSection("ConnectionStrings").GetSection("OracleDBConn").Value;
            var woId = objFile.file.FileName.Split('_')[0];
            var mess = string.Empty;
            try
            {
                if (objFile.file.Length > 0)
                {
                    if (!Directory.Exists(eamDocUploadPath))
                    {
                        Directory.CreateDirectory(eamDocUploadPath);
                    }

                    var filePath = eamDocUploadPath + objFile.file.FileName;


                    using (FileStream stream = System.IO.File.Create(filePath))
                    {

                        objFile.file.CopyTo(stream);
                        stream.Flush();
                    }

                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("wo_id", OracleDbType.Varchar2, ParameterDirection.Input, woId);
                    dyParam.Add("wo_dfn", OracleDbType.Varchar2, ParameterDirection.Output);
                    var conn = new OracleConnection(oracleConn);
                    //if (conn.State == ConnectionState.Closed)
                    //{
                    //    conn.Open();
                    //}

                    if (conn.State == ConnectionState.Open)
                    {
                        var query = "get_wo_dfn";

                        ////var bbbdFileName = SqlMapper.Query(conn, query, param: dyParam, commandType: CommandType.StoredProcedure);

                        // Merge PDF doc
                        // Open report document
                        ////Document pdfDocument1 = new Document(filePath);
                        // Open check list document
                        ////Document pdfDocument2 = new Document(eamDocUploadPath + bbbdFileName);

                        // Add pages of second document to the first
                        ////pdfDocument1.Pages.Add(pdfDocument2.Pages);

                        ////filePath = eamDocUploadPath + "BB_" + woId + "_"+ DateTime.Now.ToString("yyyyMMddHHmm") + ".pdf";
                        // Save concatenated output file
                        ////pdfDocument1.Save(filePath);
                        // ---------------------------------------------------------------------------------------------------------------------

                        // Insert new document for WO
                        ////query = "ins_wo_fn";
                        ////var finalFile = SqlMapper.Query(conn, query, param: dyParam, commandType: CommandType.StoredProcedure);
                        // ----------------------------------------------------------------------------------------------------------------------
                    }

                    return filePath;
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
