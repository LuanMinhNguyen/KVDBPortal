using System;
using System.Runtime.Serialization;
using iCADEventServer.Business.Services.Security;
using iCADEventServer.Data.Entities;

namespace iCADEventServer.Service.Dto.Security
{
    [DataContract]
    public class OnlineResponderDto
    {
        private readonly UserService userService;
        public OnlineResponderDto() {}
        public OnlineResponderDto(MobileResponderTracking responderTracking)
        {
            this.userService = new UserService();

            this.UserId = responderTracking.UserId.GetValueOrDefault();
            this.Lat = responderTracking.Lat.GetValueOrDefault();
            this.Lng = responderTracking.Lng.GetValueOrDefault();
            this.UpdateTime = responderTracking.UpdateTime.GetValueOrDefault();
            //this.StatusId = responderTracking.StatusId.GetValueOrDefault();
            //this.StatusName = responderTracking.StatusName;
            this.DeviceToken = responderTracking.DeviceToken;
            this.DeviceInfo = responderTracking.DeviceInfo;
            this.UnitId = responderTracking.UnitId.GetValueOrDefault();
            this.UnitName = responderTracking.UnitName;

            this.UserInfo = new UserInfoDto(this.userService.GetByID(this.UserId));
        }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public UserInfoDto UserInfo { get; set; }

        [DataMember]
        public double Lat { get; set; }

        [DataMember]
        public double Lng { get; set; }

        [DataMember]
        public DateTime UpdateTime { get; set; }

        [DataMember]
        public int StatusId { get; set; }

        [DataMember]
        public string StatusName { get; set; }

        [DataMember]
        public string DeviceToken { get; set; }

        [DataMember]
        public string DeviceInfo { get; set; }

        [DataMember]
        public string MRAppVersion { get; set; }

        [DataMember]
        public int UnitId { get; set; }

        [DataMember]
        public string UnitName { get; set; }
    }
}