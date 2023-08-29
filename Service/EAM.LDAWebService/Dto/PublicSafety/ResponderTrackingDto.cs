using System.Runtime.Serialization;
using iCADEventServer.Data.Entities;

namespace iCADEventServer.Service.Dto.PublicSafety
{
    [DataContract]
    public class ResponderTrackingDto
    {
        public ResponderTrackingDto() {}
        public ResponderTrackingDto(MobileResponderTrackingHistory obj)
        {
            this.UserId = obj.UserId.GetValueOrDefault();
            this.Lng = obj.Lng.GetValueOrDefault();
            this.Lat = obj.Lat.GetValueOrDefault();
            this.DeviceInfo = obj.DeviceInfo;
            this.UnitId = obj.UnitId.GetValueOrDefault();
            this.UnitName = obj.UnitName;
            this.UpdateTime = obj.UpdateTime.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss");
        }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string DeviceInfo { get; set; }

        [DataMember]
        public int UnitId { get; set; }

        [DataMember]
        public string UnitName { get; set; }

        [DataMember]
        public double Lat { get; set; }

        [DataMember]
        public double Lng { get; set; }

        [DataMember]
        public string UpdateTime { get; set; }
    }
}