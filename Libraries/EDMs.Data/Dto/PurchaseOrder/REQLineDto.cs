using System;

namespace EDMs.Data.Dto.PurchaseOrder
{
    public class REQLineDto
    {
        public string MaVatTu { get; set; }
        public string TenVatTu { get; set; }
        public string DVT { get; set; }
        public string Kieu { get; set; }
        public decimal Dong { get; set; }
        public decimal SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public string TienTe { get; set; }
        public string TrangThai { get; set; }
        public string BaoGia { get; set; }
        public decimal DongBaoGia { get; set; }
        public DateTime CapTruocngay { get; set; }
        public string YeuCauChaoGia { get; set; }
    }
}
