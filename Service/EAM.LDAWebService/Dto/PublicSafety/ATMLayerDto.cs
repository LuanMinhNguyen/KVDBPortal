using System.Runtime.Serialization;
using iCADEventServer.Data.Entities;

namespace iCADEventServer.Service.Dto.PublicSafety
{
    [DataContract]
    public class ATMLayerDto
    {
        public ATMLayerDto() {}
        public ATMLayerDto(ATMLayer atmObj)
        {
            this.Name = atmObj.Name;
            this.Lng = atmObj.Lng.GetValueOrDefault();
            this.Lat = atmObj.Lat.GetValueOrDefault();
            this.Address = atmObj.Address;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public double Lng { get; set; }

        [DataMember]
        public double Lat { get; set; }
    }
}