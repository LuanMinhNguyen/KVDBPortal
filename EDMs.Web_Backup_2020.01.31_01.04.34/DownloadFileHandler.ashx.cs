using System;
using System.IO;
using System.Web;
using EDMs.Business.Services.Document;

namespace EDMs.Web
{
    /// <summary>
    /// Summary description for DownloadFileHandler
    /// </summary>
    public class DownloadFileHandler : IHttpHandler
    {
        
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["filepath"] != null)
            {
                    var fileInfo = new FileInfo(context.Request.QueryString["filepath"]);
                    try
                    {
                        if (fileInfo.Exists)
                        {
                            context.Response.Clear();
                            context.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileInfo.Name + "\"");
                            context.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                            context.Response.ContentType = "application/octet-stream";
                            context.Response.TransmitFile(fileInfo.FullName);
                            context.Response.Flush();
                        }
                        else
                        {
                            throw new Exception("File not found");
                        }
                    }
                    catch (Exception ex)
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(ex.Message);
                    }
                    finally
                    {
                        context.Response.End();
                    }
            }

            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
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