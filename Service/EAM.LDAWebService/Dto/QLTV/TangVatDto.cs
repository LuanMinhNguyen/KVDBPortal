using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SaleManagement.Entity;
using SaleManagement.Service.QLTV;

namespace QLTV.WebService.Dto.QLTV
{
    [DataContract]
    public class TangVatDto
    {
        public TangVatDto() {}
        public TangVatDto(TangVat obj)
        {
            this.ID = obj.ID;
            this.MaTangVat = obj.Code;
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
            this.CreatedDate = obj.CreatedDate;
            this.CreatedBy = obj.CreatedByName;
            this.UpdatedDate = obj.UpdatedDate;
            this.UpdatedBy = obj.UpdatedByName;
            this.AttachFiles = new List<string>();

        }

        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public string MaTangVat { get; set; }

        [DataMember]
        public int SoTangVatId { get; set; }

        [DataMember]
        public string SoTangVatName { get; set; }

        [DataMember]
        public string BienBanSo { get; set; }

        [DataMember]
        public string BienSo { get; set; }

        [DataMember]
        public DateTime? NgayNhapKho { get; set; }

        [DataMember]
        public string NguoiGiao { get; set; }

        [DataMember]
        public DateTime? NgayTra { get; set; }

        [DataMember]
        public string NguoiNhan { get; set; }

        [DataMember]
        public int LoaiTangVatId { get; set; }

        [DataMember]
        public string LoaiTangVatName { get; set; }

        [DataMember]
        public int LoaiPhuongTienId { get; set; }

        [DataMember]
        public string LoaiPhuongTienName { get; set; }

        [DataMember]
        public string MauSac { get; set; }

        [DataMember]
        public string ThuongHieu { get; set; }

        [DataMember]
        public string GhiChu { get; set; }
        [DataMember]
        public List<string> AttachFiles { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }

        [DataMember]
        public DateTime? UpdatedDate { get; set; }

        [DataMember]
        public string UpdatedBy { get; set; }
    }
}