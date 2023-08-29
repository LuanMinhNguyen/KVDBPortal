using System.Runtime.Serialization;
using iCADEventServer.Data.Entities;

namespace iCADEventServer.Service.Dto.PublicSafety
{
    [DataContract]
    public class MobileResponderChatDto
    {
        public MobileResponderChatDto() {}
        public MobileResponderChatDto(MobileResponderChat chatObj)
        {
            this.FromUserId = chatObj.FromUserId.GetValueOrDefault();
            this.FromUserName = chatObj.FromUserName;
            this.ToUserId = chatObj.ToUserId.GetValueOrDefault();
            this.ToUserName = chatObj.ToUserName;
            this.Message = chatObj.Message;
        }

        [DataMember]
        public int FromUserId { get; set; }

        [DataMember]
        public string FromUserName { get; set; }

        [DataMember]
        public int ToUserId { get; set; }

        [DataMember]
        public string ToUserName { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}