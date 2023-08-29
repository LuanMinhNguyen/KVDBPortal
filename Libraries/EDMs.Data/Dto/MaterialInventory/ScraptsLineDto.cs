using System;

namespace EDMs.Data.Dto.MaterialInventory
{
    public class ScraptsLineDto
    {
        public string SoGiaoDich { get; set; }
        public decimal MucSo { get; set; }
        public DateTime NgayGiaoDich { get; set; }
        public string VatTu { get; set; }
        public string MoTa { get; set; }
        public decimal Soluong { get; set; }
        public decimal DonGia { get; set; }
    }
}
