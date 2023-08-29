using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Configuration;
using iCADEventServer.Business.Services.PublicSafety;
using iCADEventServer.Data.Entities;

namespace iCADEventServer.Service.Dto.PublicSafety
{
    [DataContract]
    public class MREmergencyEventDto
    {
        private readonly EmergencyImageService eventImageService;
        private readonly EmergencyEventProcessService emergencyEventProcessService;
        public MREmergencyEventDto() {}
        public MREmergencyEventDto(EmergencyEvent eventObj)
        {
            this.eventImageService = new EmergencyImageService();
            this.emergencyEventProcessService = new EmergencyEventProcessService();

            this.ID = eventObj.ID;
            this.Name = eventObj.Name;
            this.LocationName = eventObj.LocaltionName;
            this.Lng = eventObj.Lng.GetValueOrDefault();
            this.Lat = eventObj.Lat.GetValueOrDefault();
            this.RequestUnitTypeId = eventObj.EmergencyTypeId.GetValueOrDefault();
            this.RequestUnitTypeName = eventObj.EmergencyName;
            this.PhoneNumber = eventObj.PhoneNumber;
            this.Message = eventObj.Message;
            this.CreatedTime = eventObj.CreatedTime != null 
                ? Utilities.Utility.ConvertToTimestampMR(eventObj.CreatedTime.GetValueOrDefault())
                : 0;
            this.PriorityLvlId = eventObj.PriorityLvlId.GetValueOrDefault();
            this.IsProcessing = eventObj.IsProcessing.GetValueOrDefault();
            this.CurrentProcessingResponderName = eventObj.CurrentProcessingResponderName;
            this.PriorityLvlName = eventObj.PriorityLvlName;
            this.EventTypeIds = eventObj.EmergencyDetailTypeIds;
            this.EventTypeName = eventObj.EmergencyDetailTypeName;

            var eventImageList = this.eventImageService.GetByEventId(eventObj.ID);
            this.ImageList = new List<string>();
            foreach (var eventImage in eventImageList)
            {
                //var filePath = WebConfigurationManager.AppSettings["EventImageFolder"] + eventImage.FileName;
                //if (File.Exists(filePath))
                //{
                //    var eventImageByteArr = File.ReadAllBytes(filePath);
                //    this.ImageList.Add(Convert.ToBase64String(eventImageByteArr));
                //}
                this.ImageList.Add(WebConfigurationManager.AppSettings["HubConnection"] + eventImage.FilePath);
            }

            var processHistory = this.emergencyEventProcessService.GetAllByEvent(eventObj.ID);
            this.ProcessHistory = processHistory.Select(t => new EmergencyEventProcessHistoryDto(t)).ToList();
        }

        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string LocationName { get; set; }

        [DataMember]
        public double Lng { get; set; }

        [DataMember]
        public double Lat { get; set; }

        [DataMember]
        public int RequestUnitTypeId { get; set; }

        [DataMember]
        public string RequestUnitTypeName { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public List<string> ImageList { get; set; }

        [DataMember]
        public long CreatedTime { get; set; }

        [DataMember]
        public string EventTypeIds { get; set; }

        [DataMember]
        public string EventTypeName { get; set; }

        [DataMember]
        public int PriorityLvlId { get; set; }

        [DataMember]
        public string PriorityLvlName { get; set; }

        [DataMember]
        public string CurrentProcessingResponderName { get; set; }

        [DataMember]
        public bool IsProcessing { get; set; }

        [DataMember]
        public bool IsOwnerProcess { get; set; }

        [DataMember]
        public int ResponderId { get; set; }

        [DataMember]
        public List<EmergencyEventProcessHistoryDto> ProcessHistory { get; set; }

        [DataMember]
        public string Image1 { get; set; }

        [DataMember]
        public string Image2 { get; set; }

        [DataMember]
        public string Image3 { get; set; }

        [DataMember]
        public string Image4 { get; set; }

        [DataMember]
        public string Image5 { get; set; }

        [DataMember]
        public int ProcessStatusByResponder { get; set; }
    }
}