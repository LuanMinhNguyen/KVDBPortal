using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace EAM.LDAWebService
{
    
    [ServiceContract]
    public interface ILDAWebService
    {
        //[OperationContract]
        //[WebInvoke(Method = "POST",
        //    ResponseFormat = WebMessageFormat.Json,
        //    RequestFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare,
        //    UriTemplate = "/QLTVProcess")]
        //ResponseDto QLTVProcess(RequestDto request);

        //bool SafeCityUserRegister(SafeCityRegisterDto safeCityRegisterObj);
        
        [OperationContract]
        string GetStringTemp();

        [OperationContract]
        [WebInvoke(UriTemplate = "UploadBarcodeFile?FileName={fileName}")]
        void UploadBarcodeFile(string fileName, Stream stream);
        
        [OperationContract]
        [WebInvoke(UriTemplate = "UploadFile?FileName={fileName}")]
        void UploadFile(string fileName, Stream stream);

        [OperationContract]
        byte[] GetFileFromFolder(string pFileName);
    }
}

