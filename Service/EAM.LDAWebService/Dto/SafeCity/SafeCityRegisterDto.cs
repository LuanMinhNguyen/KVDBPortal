using System;
using System.Runtime.Serialization;
using iCADEventServer.Data.Entities;

namespace iCADEventServer.Service.Dto.SafeCity
{
    [DataContract]
    public class SafeCityRegisterDto
    {
        public SafeCityRegisterDto() { }
        public SafeCityRegisterDto(SafeCityUserRegister safeCityUser)
        {
            this.PhoneNumber = safeCityUser.PhoneNumber;
            this.IsVerifiedPhoneNo = safeCityUser.IsVerify.GetValueOrDefault();
            this.IsBanned = safeCityUser.IsBanned.GetValueOrDefault();
            this.DeviceInfo = safeCityUser.DeviceInfo;
            this.DeviceToken = safeCityUser.DeviceToken;
            this.ClientVersion = safeCityUser.ClientVersion;
            this.SafeCityUserId = safeCityUser.ID;
        }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string DeviceToken { get; set; }

        [DataMember]
        public string DeviceInfo { get; set; }

        [DataMember]
        public string ClientVersion { get; set; }

        [DataMember]
        public bool IsVerifiedPhoneNo { get; set; }

        [DataMember]
        public bool IsBanned { get; set; }

        [DataMember]
        public int SafeCityUserId { get; set; }
    }
}