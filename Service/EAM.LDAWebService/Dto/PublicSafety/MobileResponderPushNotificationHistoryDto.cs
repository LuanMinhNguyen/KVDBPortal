using System.Runtime.Serialization;
using iCADEventServer.Data.Entities;

namespace iCADEventServer.Service.Dto.PublicSafety
{
    [DataContract]
    public class MobileResponderPushNotificationHistoryDto
    {
        public MobileResponderPushNotificationHistoryDto() { }
        public MobileResponderPushNotificationHistoryDto(MobileResponderPushNotificationHistory mrNotification)
        {
            this.ResponderId = mrNotification.ResponderId.GetValueOrDefault();
            this.Message = mrNotification.Message;
            this.SendDate = Utilities.Utility.ConvertToTimestamp(mrNotification.SendDate.GetValueOrDefault());
            this.Title  = mrNotification.Title;
        }

        [DataMember]
        public int ResponderId { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public long SendDate { get; set; }

        [DataMember]
        public string Title { get; set; }
    }
}
