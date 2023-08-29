using System.Runtime.Serialization;
using iCADEventServer.Data.Entities;

namespace iCADEventServer.Service.Dto.Library
{
    [DataContract]
    public class EventTypeDto
    {
        public EventTypeDto() {}
        public EventTypeDto(EventType eventTypeObj)
        {
            this.Name = eventTypeObj.Name;
            this.ID = eventTypeObj.ID;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int ID { get; set; }
    }
}