using System;
using System.Runtime.Serialization;

namespace iCADEventServer.Service.Dto.PublicSafety
{
    [DataContract]
    public class MobileResponderDeviceRegisterDto
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string DeviceKey { get; set; }

        [DataMember]
        public string InstalledVersion { get; set; }
    }
}