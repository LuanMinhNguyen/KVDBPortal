using System;

namespace EDMs.Data.Dto.PurchaseOrder
{
    public class REQListDto
    {
        public string FullName { get; set; }
        public string DonVi { get; set; }
        public string PhieuYC { get; set; }
        public string DienGiai { get; set; }
        public string TrangThai { get; set; }
        public string Kho { get; set; }
        public string NguoiYC { get; set; }
        
        public DateTime NgayYC { get; set; }
        
        public decimal MucCV { get; set; }
        public string PhieuCV { get; set; }
        public string NguoiLap { get; set; }

        public DateTime NgayPheDuyet { get; set; }
        public string NguoiPheDuyet { get; set; }
    }
}
