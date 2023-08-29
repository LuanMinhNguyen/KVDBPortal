using System.Runtime.Serialization;

namespace iCADEventServer.Service.Dto.PublicSafety
{
    [DataContract]
    public class MobileResponderDeviceAuthenticateDto
    {
        [DataMember]
        public string AuthenticateStatus { get; set; }

        [DataMember]
        public string OlderVersionRunWithoutUpdate { get; set; }

        [DataMember]
        public string CurrentServerVersion { get; set; }

        [DataMember]
        public string CurrentServerUpdateLink { get; set; }
    }
}