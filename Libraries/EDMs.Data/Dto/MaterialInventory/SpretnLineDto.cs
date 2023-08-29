﻿using System;

namespace EDMs.Data.Dto.MaterialInventory
{
    public class SPRETNLineDto
    {

        public string FullName { get; set; }
        public string SoGiaoDich { get; set; }
        public decimal MucSo { get; set; }

        public string DienGiai { get; set; }
        public string kieuGiaoDich { get; set; }
        
        public string SoDonHang { get; set; }
        public decimal MucDonHang { get; set; }
        public DateTime NgayGiaoDich { get; set; }

        public string VatTu { get; set; }
        public string MoTa { get; set; }
        public decimal Soluong { get; set; }
        public decimal DonGia { get; set; }
        public string PhieuNhap { get; set; }
        public string PhieuYCCap { get; set; }
        public string PhieuCV { get; set; }
        public decimal MucCV { get; set; }
        public string Kho { get; set; }
        public string SoYCMuaSam { get; set; }
        public decimal MucYCMuaSam { get; set; }
    }
}
