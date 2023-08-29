using System;

namespace EAM.Data.Dto
{
    public class PartStockDto
    {
        public string MaVT { get; set; }
        public string MoTa { get; set; }
        public string DVT { get; set; }
        public string NhomChinh { get; set; }
        public string NhomCon { get; set; }
        public string QCDongGoi { get; set; }
        public string Kho { get; set; }
        public string ViTri { get; set; }
        public string Lo { get; set; }
        public DateTime? HSD { get; set; }
        public decimal SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
    }
}
