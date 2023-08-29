using System.Runtime.Serialization;
using iCADEventServer.Data.Entities;

namespace iCADEventServer.Service.Dto.Library
{
    [DataContract]
    public class EventProcessDetailStatusDto
    {
        public EventProcessDetailStatusDto() {}
        public EventProcessDetailStatusDto(int id, string name)
        {
            this.Name = name;
            this.ID = id;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int ID { get; set; }
    }
}