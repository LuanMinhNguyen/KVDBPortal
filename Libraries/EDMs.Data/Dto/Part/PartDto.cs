using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;

namespace EDMs.Data.Dto.Part
{
    public class PartDto
    {
        public string VatTu { get; set; }
        public string TenVatTu { get; set; }
        public string DonVi { get; set; }
        public string QuyCach { get; set; }
        public string NgungSuDung { get; set; }
        public string PhanLoai { get; set; }
        public string NhaCungCapUuTien { get; set; }
        public string PhanNhom { get; set; }
        public string DonViTinh { get; set; }
        public string Nguon { get; set; }
        public string PPTinhGia { get; set; }
        public decimal GiaCoSo { get; set; }
        public decimal GiaTB { get; set; }
        public decimal GiaMoiMua { get; set; }
        public decimal GiaTieuChuan { get; set; }
        public string FullName { get; set; }
    }
}
