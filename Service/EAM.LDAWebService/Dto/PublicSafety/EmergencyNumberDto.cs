using System.Runtime.Serialization;

namespace iCADEventServer.Service.Dto.PublicSafety
{
    [DataContract]
    public class EmergencyNumberDto
    {
        [DataMember]
        public string CallNumber { get; set; }

        [DataMember]
        public string SMSNumber { get; set; }
    }
}