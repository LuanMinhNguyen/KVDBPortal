using System.Runtime.Serialization;
using iCADEventServer.Data.Entities;

namespace iCADEventServer.Service.Dto.PublicSafety
{
    [DataContract]
    public class CameraLayerDto
    {
        public CameraLayerDto() {}
        public CameraLayerDto(CameraLayer cameraObj)
        {
            this.Name = cameraObj.Name;
            this.Lng = cameraObj.Lng.GetValueOrDefault();
            this.Lat = cameraObj.Lat.GetValueOrDefault();
            this.Address = cameraObj.Address;
            this.UserName = cameraObj.UserName;
            this.Pwd = cameraObj.Pwd;
            this.UrlLink = cameraObj.UrlLink;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string UrlLink { get; set; }

        [DataMember]
        public double Lng { get; set; }

        [DataMember]
        public double Lat { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Pwd { get; set; }
    }
}