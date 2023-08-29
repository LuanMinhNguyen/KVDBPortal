using System.Runtime.Serialization;

namespace EAM.LDAWebService.Dto.Controller
{
    [DataContract]
    public class RequestDto
    {
        [DataMember]
        public string MethodName { get; set; }

        [DataMember]
        public string Data { get; set; }
    }
}