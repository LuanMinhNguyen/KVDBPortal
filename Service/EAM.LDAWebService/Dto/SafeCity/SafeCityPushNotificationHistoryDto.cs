using System;
using System.Runtime.Serialization;
using iCADEventServer.Data.Entities;

namespace iCADEventServer.Service.Dto.SafeCity
{
    [DataContract]
    public class SafeCityPushNotificationHistoryDto
    {
        public SafeCityPushNotificationHistoryDto() { }
        public SafeCityPushNotificationHistoryDto(SafeCityPushNotificationHistory safeCityNotification)
        {
            this.SafeCityUserId = safeCityNotification.SafeCityUserId.GetValueOrDefault();
            this.Message = safeCityNotification.Message;
            this.SendDate = Utilities.Utility.ConvertToTimestamp(safeCityNotification.SendDate.GetValueOrDefault());
            this.Title = safeCityNotification.Title;
        }

        [DataMember]
        public int SafeCityUserId { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public long SendDate { get; set; }

        [DataMember]
        public string Title { get; set; }
    }
}