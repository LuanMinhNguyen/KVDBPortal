using System.Runtime.Serialization;
using SaleManagement.Entity;

namespace QLTV.WebService.Dto.QLTV
{
    [DataContract]
    public class LoaiSoTangVatDto
    {
        public LoaiSoTangVatDto() {}
        public LoaiSoTangVatDto(LoaiSoTangVat obj)
        {
            this.ID = obj.ID;
            this.Name = obj.Name;
            this.Description = obj.Description;
            this.IsActive = obj.IsActive.GetValueOrDefault();

        }

        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }
}