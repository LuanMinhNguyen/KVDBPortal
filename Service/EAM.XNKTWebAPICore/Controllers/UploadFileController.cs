using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using EAM.XNKTWebAPICore.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace EAM.XNKTWebAPICore.Controllers
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

            var mess = string.Empty;
            var resultMess = string.Empty;
            try
            {
                if (objFile.file.Length > 0)
                {
                    mess += objFile.file.FileName;
                    var woId = objFile.file.FileName.Split('_')[0];

                    if (!Directory.Exists(eamDocUploadPath))
                    {
                        Directory.CreateDirectory(eamDocUploadPath);
                    }

                    var filePath = eamDocUploadPath + objFile.file.FileName;


                    using (FileStream stream = System.IO.File.Create(filePath))
                    {

                        objFile.file.CopyTo(stream);
                        stream.Flush();
                        mess += "Upload completed: " + objFile.file.FileName;
                    }

                    var conn = new OracleConnection(oracleConn);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                        mess += "</br>DB connected";

                    }

                    if (conn.State == ConnectionState.Open)
                    {
                        OracleCommand objCmd = new OracleCommand();
                        objCmd.Connection = conn;
                        objCmd.CommandText = "get_wo_dfn";
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.Parameters.Add("wo_id", OracleDbType.Varchar2).Value = woId;
                        objCmd.Parameters.Add("wo_dfn", OracleDbType.Varchar2).Direction = ParameterDirection.Output;
                        objCmd.Parameters["wo_dfn"].Size = 300;
                        objCmd.ExecuteNonQuery();

                        var bbbdFileName = objCmd.Parameters["wo_dfn"].Value.ToString();
                        var finalFileName = "BB_" + woId + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".pdf";

                        mess += "</br>bbbdFileName: " + bbbdFileName;

                        if (bbbdFileName != "null" && !string.IsNullOrEmpty(bbbdFileName))
                        {
                            mess += "</br>Get BBBD file name: " + bbbdFileName;
                            // Merge PDF doc
                            // Open report document


                            using (PdfDocument one = PdfReader.Open(filePath, PdfDocumentOpenMode.Import))
                            using (PdfDocument two = PdfReader.Open(eamDocUploadPath + bbbdFileName, PdfDocumentOpenMode.Import))
                            using (PdfDocument outPdf = new PdfDocument())
                            {
                                CopyPages(one, outPdf);
                                CopyPages(two, outPdf);

                                outPdf.Save(eamDocUploadPath + finalFileName);
                                mess += "</br>Save Merge file completed: " + eamDocUploadPath + finalFileName;
                            }

                            void CopyPages(PdfDocument from, PdfDocument to)
                            {
                                for (int i = 0; i < from.PageCount; i++)
                                {
                                    to.AddPage(from.Pages[i]);
                                }
                            }

                            //// ---------------------------------------------------------------------------------------------------------------------

                            
                        }
                        else
                        {
                            mess += "</br>Can't find BBBD";
                            // Merge PDF doc
                            // Open report document

                            using (PdfDocument one = PdfReader.Open(filePath, PdfDocumentOpenMode.Import))
                            using (PdfDocument outPdf = new PdfDocument())
                            {
                                CopyPages(one, outPdf);

                                outPdf.Save(eamDocUploadPath + finalFileName);
                                mess += "</br>Save Merge file completed: " + eamDocUploadPath + finalFileName;
                            }

                            void CopyPages(PdfDocument from, PdfDocument to)
                            {
                                for (int i = 0; i < from.PageCount; i++)
                                {
                                    to.AddPage(from.Pages[i]);
                                }
                            }

                            resultMess = "Can't find 'BBBD' file. Consolidate report completed";
                        }

                        // Insert new document for WO
                        OracleCommand objInsCmd = new OracleCommand();
                        objInsCmd.Connection = conn;
                        objInsCmd.CommandText = "ins_wo_fn";
                        objInsCmd.CommandType = CommandType.StoredProcedure;
                        objInsCmd.Parameters.Add("wo_id", OracleDbType.Varchar2).Value = woId;
                        objInsCmd.Parameters.Add("wo_fileName", OracleDbType.Varchar2).Value = finalFileName;
                        objInsCmd.Parameters["wo_fileName"].Size = 300;
                        objInsCmd.ExecuteNonQuery();
                        mess += "</br>Insert new document after merged: " + finalFileName;

                        ////query = "ins_wo_fn";
                        ////var finalFile = SqlMapper.Query(conn, query, param: dyParam, commandType: CommandType.StoredProcedure);
                        // ----------------------------------------------------------------------------------------------------------------------

                        return resultMess;
                    }

                    return "Consolidate report not complete.";
                }
                else
                {
                    return "Don't have file to upload!";
                }
            }
            catch (Exception ex)
            {
                return mess += "</br>Error: " + ex.Message.ToString();
            }
        }
    }
}
