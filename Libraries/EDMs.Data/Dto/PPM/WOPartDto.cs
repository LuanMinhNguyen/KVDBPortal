using System;

namespace EDMs.Data.Dto.PPM
{
    public class WOPartDto
    {
        public string FullName { get; set; }
        public string VatTu { get; set; }
        public string TenVatTu { get; set; }
        public string DonViTinh { get; set; }
        public decimal SoLuongKeHoach { get; set; }
        public string TonKho { get; set; }
        public string MuaTrucTiep { get; set; }
        public string Kho { get; set; }
        public decimal DaSuDung { get; set; }
    }
}
