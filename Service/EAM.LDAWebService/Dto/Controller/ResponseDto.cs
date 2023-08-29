using System.Runtime.Serialization;

namespace EAM.LDAWebService.Dto.Controller
{
    [DataContract]
    public class ResponseDto
    {
        [DataMember]
        public bool ProcessStatus { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string DataString { get; set; }
    }
}