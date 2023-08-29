using System.Runtime.Serialization;
using iCADEventServer.Data.Entities;

namespace iCADEventServer.Service.Dto.PublicSafety
{
    [DataContract]
    public class SOSEventDto
    {
        public SOSEventDto() {}

        [DataMember]
        public string DeviceId { get; set; }
        
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string ZoneCode { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}