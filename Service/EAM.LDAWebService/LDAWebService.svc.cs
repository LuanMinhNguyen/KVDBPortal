using System.Configuration;
using System.IO;

namespace EAM.LDAWebService
{
    public class LDAWebService : ILDAWebService
    {
        

        public LDAWebService()
        {
           
        }

        public string GetStringTemp()
        {
            return "Test return string service.";
        }

        public void UploadBarcodeFile(string fileName, Stream stream)
        {
            var fullPath = ConfigurationManager.AppSettings["BarcodeUploadPath"] + fileName;

            using (var file = File.Create(fullPath))
            {
                stream.CopyTo(file);
            }
        }

        

        public void UploadFile(string fileName, Stream stream)
        {
            var fullPath = ConfigurationManager.AppSettings["FileUploadPath"] + fileName;

            using (var file = File.Create(fullPath))
            {
                stream.CopyTo(file);
            }
        }

        public byte[] GetFileFromFolder(string filename)
        {
            byte[] filedetails = new byte[0];
            string strTempFolderPath = System.Configuration.ConfigurationManager.AppSettings.Get("FileUploadPath");
            if (File.Exists(strTempFolderPath + filename))
            {
                return File.ReadAllBytes(strTempFolderPath + filename);
            }
            else return filedetails;
        }
    }
}
