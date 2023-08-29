using System;

namespace EDMs.Data.Dto.PurchaseOrder
{
    public class PORDLineDto
    {
        public decimal Muc { get; set; }
        public string VatTu { get; set; }
        public string DienGiai { get; set; }
        public string Kieu { get; set; }
        public string TrangThai { get; set; }
        public decimal SoLuong { get; set; }
        public string TienTe { get; set; }
        public decimal GiamGia { get; set; }
        public decimal TongThue { get; set; }
        public string BaoGia { get; set; }
    }
}
