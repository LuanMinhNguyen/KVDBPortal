using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Configuration;
using iCADEventServer.Business.Services.PublicSafety;
using iCADEventServer.Data.Entities;
using iCADEventServer.Service.Utilities;

namespace iCADEventServer.Service.Dto.PublicSafety
{
    [DataContract]
    public class EmergencyEventProcessDto
    {
        private readonly EmergencyEventProcessAttachFileService emergencyEventProcessAttachFileService;

        public EmergencyEventProcessDto() {}

        public EmergencyEventProcessDto(EmergencyEventProcess eventProcessObj)
        {
            this.emergencyEventProcessAttachFileService = new EmergencyEventProcessAttachFileService();

            this.ID = eventProcessObj.ID;
            this.EventId = eventProcessObj.EventId.GetValueOrDefault();
            this.ResponderId = eventProcessObj.UserId.GetValueOrDefault();
            this.ActionMessage = eventProcessObj.ActionMessage;
            this.UserName = eventProcessObj.UserName;
            this.UnitName = eventProcessObj.UnitName;
            this.UpdatedDate = eventProcessObj.AssignUserUpdateDate != null
                                ? Utility.ConvertToTimestampMR(eventProcessObj.AssignUserUpdateDate.GetValueOrDefault())
                                : 0; ;
            this.StatusId = eventProcessObj.StatusId.GetValueOrDefault();
            this.StatusName = eventProcessObj.StatusName;
            var eventProcessImageList = this.emergencyEventProcessAttachFileService.GetAllByProcessId(eventProcessObj.ID);
            this.ImageList = new List<string>();
            foreach (var eventProcessImage in eventProcessImageList)
            {
                //var filePath = WebConfigurationManager.AppSettings["EventImageFolder"] + eventImage.FileName;
                //if (File.Exists(filePath))
                //{
                //    var eventImageByteArr = File.ReadAllBytes(filePath);
                //    this.ImageList.Add(Convert.ToBase64String(eventImageByteArr));
                //}
                this.ImageList.Add(WebConfigurationManager.AppSettings["HubConnection"] + eventProcessImage.FilePath);
            }
        }

        [DataMember]
        public int ID { get; set; }

        // Properties for create
        [DataMember]
        public int EventId { get; set; }

        [DataMember]
        public int ResponderId { get; set; }

        [DataMember]
        public string ActionMessage { get; set; }

        [DataMember]
        public List<string> ImageList { get; set; }
        // ---------------------------------------------

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string UnitName { get; set; }

        [DataMember]
        public long UpdatedDate { get; set; }

        [DataMember]
        public int StatusId { get; set; }

        [DataMember]
        public string StatusName { get; set; }

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
    }
}