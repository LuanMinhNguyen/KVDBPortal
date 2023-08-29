using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using iCADEventServer.Data.Entities;

namespace iCADEventServer.Service.Dto.PublicSafety
{
    [DataContract]
    public class EmergencyEventDto
    {
        public EmergencyEventDto() {}
        public EmergencyEventDto(EmergencyEvent eventObj)
        {
            this.Name = eventObj.Name;
            this.LocationName = eventObj.LocaltionName;
            this.Lng = eventObj.Lng.GetValueOrDefault();
            this.Lat = eventObj.Lat.GetValueOrDefault();
            this.EmergencyTypeId = eventObj.EmergencyTypeId.GetValueOrDefault();
            this.EmergencyTypeName = eventObj.EmergencyName;
            this.PhoneNumber = eventObj.PhoneNumber;
            this.Message = eventObj.Message;
            this.CreatedTime = eventObj.CreatedTime.GetValueOrDefault();
            this.StatusId = eventObj.ProcessStatusId.GetValueOrDefault();
            this.StatusName = eventObj.ProcessStatusName;
            this.EventId = eventObj.ID;
        }

        [DataMember]
        public int EventId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string LocationName { get; set; }

        [DataMember]
        public double Lng { get; set; }

        [DataMember]
        public double Lat { get; set; }

        [DataMember]
        public int EmergencyTypeId { get; set; }

        [DataMember]
        public string EmergencyTypeName { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public List<string> ImageList { get; set; }

        [DataMember]
        public List<EmergencyEventProcessHistoryDto> ProcessHistoryList { get; set; }

        [DataMember]
        public DateTime CreatedTime { get; set; }

        [DataMember]
        public int StatusId { get; set; }

        [DataMember]
        public string StatusName { get; set; }
    }
}