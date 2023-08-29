using System;
using System.IO;
using System.Web;

namespace EAM.WebPortal
{
    /// <summary>
    /// Summary description for DownLoadFile
    /// </summary>
    public class DownLoadFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Stream stream = null;
            try
            {
                string FileLocation = HttpContext.Current.Request.QueryString["file"];
                if (File.Exists(FileLocation))
                {
                    stream = new FileStream(FileLocation, FileMode.Open, FileAccess.Read, FileShare.Read);
                    long bytesToRead = stream.Length;
                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(FileLocation));
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
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
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