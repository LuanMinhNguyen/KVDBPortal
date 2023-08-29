using System;
using System.Runtime.Serialization;
using SaleManagement.Entity;

namespace QLTV.WebService.Dto.QLTV
{
    [DataContract]
    public class TangVatUpdateInforDto
    {
        public TangVatUpdateInforDto() {}
        public TangVatUpdateInforDto(TangVat obj)
        {
            this.ID = obj.ID;
            this.SoTangVatId = obj.LoaiSoTangVatID.GetValueOrDefault();
            this.SoTangVatName = obj.LoaiSoTangVatName;
            this.BienBanSo = obj.RecordNumber;
            this.BienSo = obj.LicensePlate;
            this.NgayNhapKho = obj.DeliveryDate;
            this.NguoiGiao = obj.DeliverByName;
            this.NgayTra = obj.ReceiveData;
            this.NguoiNhan = obj.ReceiveByName;
            this.GhiChu = obj.Note;
            this.MauSac = obj.Color;
            this.ThuongHieu = obj.Brand;
            this.LoaiPhuongTienName = obj.LoaiPhuongTienName;
            this.LoaiPhuongTienId = obj.LoaiPhuongTienID.GetValueOrDefault();
            this.LoaiTangVatId = obj.LoaiTangVatID.GetValueOrDefault();
            this.LoaiTangVatName = obj.LoaiTangVatName;
            this.UpdatedDate = obj.UpdatedDate;
            this.UpdatedById = obj.UpdatedBy.GetValueOrDefault();
            this.UpdatedByName = obj.UpdatedByName;
        }

        [DataMember] public Guid ID { get; set; }
        [DataMember] public int SoTangVatId { get; set; }
        [DataMember] public string SoTangVatName { get; set; }
        [DataMember] public string BienBanSo { get; set; }
        [DataMember] public string BienSo { get; set; }
        [DataMember] public DateTime? NgayNhapKho { get; set; }//dd-MM-yyyy HH:mm:ss
        [DataMember] public string NguoiGiao { get; set; }
        [DataMember] public DateTime? NgayTra { get; set; }//dd-MM-yyyy HH:mm:ss
        [DataMember] public string NguoiNhan { get; set; }
        [DataMember] public int LoaiTangVatId { get; set; }
        [DataMember] public string LoaiTangVatName { get; set; }
        [DataMember] public int LoaiPhuongTienId { get; set; }
        [DataMember] public string LoaiPhuongTienName { get; set; }
        [DataMember] public string MauSac { get; set; }
        [DataMember] public string ThuongHieu { get; set; }
        [DataMember] public string GhiChu { get; set; }
        [DataMember] public DateTime? UpdatedDate { get; set; } //dd-MM-yyyy HH:mm:ss
        [DataMember] public int UpdatedById { get; set; }
        [DataMember] public string UpdatedByName { get; set; } 
    }
}