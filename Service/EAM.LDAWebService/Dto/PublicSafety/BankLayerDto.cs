using System.Runtime.Serialization;
using iCADEventServer.Data.Entities;

namespace iCADEventServer.Service.Dto.PublicSafety
{
    [DataContract]
    public class BankLayerDto
    {
        public BankLayerDto() {}
        public BankLayerDto(BankLayer bankObj)
        {
            this.Name = bankObj.Name;
            this.Lng = bankObj.Lng.GetValueOrDefault();
            this.Lat = bankObj.Lat.GetValueOrDefault();
            this.Address = bankObj.Address;
            this.Website = bankObj.Website;
            this.OpenTime = bankObj.OpenTime;
            this.Phone = bankObj.Phone;
            this.IconPath = bankObj.IconPath;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Website { get; set; }

        [DataMember]
        public string OpenTime { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string IconPath { get; set; }

        [DataMember]
        public double Lng { get; set; }

        [DataMember]
        public double Lat { get; set; }
    }
}