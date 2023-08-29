using System;

namespace EDMs.Data.Dto.PPM
{
    public class WODto
    {
        public string FullName { get; set; }
        public string PhieuCongViec { get; set; }
        public string TenCongViec { get; set; }
        public string ThietBi { get; set; }


        public string DoUuTien { get; set; }
        public string TrangThaiCongViec { get; set; }
        public string DonVi { get; set; }
        public string BoPhan { get; set; }
        
        public string PhanLoai { get; set; }
        public string ThueNgoai { get; set; }
        public string BaoHanh { get; set; }
        public string NguoiBaoCao { get; set; }
        public string PhanCongBoi { get; set; }
        public string PhanCongDen { get; set; }
        public string Kieu { get; set; }
        public DateTime NgayTao { get; set; }
        public string DatChoPhepSuDung { get; set; }
        public string SuDungTamThoi { get; set; }
        public string KhongTheSuDung { get; set; }
        public string GhiChuKetLuan { get; set; }
        public string createdby { get; set; }
        public DateTime KeHoachBatDau { get; set; }

        public DateTime KeHoachHoanThanh { get; set; }
        public string MaQuyTrinh { get; set; }
        public DateTime NgayBaoCao { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayHoanThanh { get; set; }
        public DateTime NgayTheoLich { get; set; }


    }
}
