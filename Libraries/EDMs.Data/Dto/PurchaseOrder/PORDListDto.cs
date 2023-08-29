using System;

namespace EDMs.Data.Dto.PurchaseOrder
{
    public class PORDListDto
    {
        public string FullName { get; set; }
        public string DonHang { get; set; }
        public string DienGiai { get; set; }
        public string DonVi { get; set; }
        public string TrangThai { get; set; }
        public string Kho { get; set; }
        public string NguoiDatHang { get; set; }
        public string SoGoiTheoDoi { get; set; }
        public string ThoiGianCho { get; set; }
        public string DiaChiGiaoNhan { get; set; }
        public decimal TongGiaTri { get; set; }


        public string TienTe { get; set; }
        public string NhaCungCap { get; set; }
        public decimal TiGia { get; set; }
        
    }
}
