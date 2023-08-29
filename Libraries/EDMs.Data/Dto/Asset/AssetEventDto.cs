using System;

namespace EDMs.Data.Dto
{
    public class AssetEventDto
    {
        public string MaPhieu { get; set; }
        public string ChiTiet { get; set; }
        public string Loai { get; set; }
        public string TrangThai { get; set; }
        public DateTime NgayHoanThanh { get; set; }
        public string DonVi { get; set; }
    }
}
