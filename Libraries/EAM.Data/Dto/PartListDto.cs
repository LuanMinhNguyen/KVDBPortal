namespace EAM.Data.Dto
{
    public class PartListDto
    {
        public string FullName { get; set; }
        public string VatTu { get; set; }
        public string DienGiai { get; set; }
        public string PhanLoai { get; set; }
        public string PhanNhom { get; set; }
        public string NhaCungCapThamKhao { get; set; }
        public string CongCu { get; set; }
        public string DVT { get; set; }
        public string NgungSuDung { get; set; }
        public string NhaSanXuat { get; set; }
        public string PartNumber { get; set; }
        public string ThietBi { get; set; }

        //thong tin don gia
        public decimal GiaTrungBinh { get; set; }
        public decimal GiaCoSo { get; set; }
        public decimal GiaMuaCuoi { get; set; }
        public decimal GiaTieuChuan { get; set; }
        
        
       
        //Thong tin theo doi
        public string MaThueHangHoa { get; set; }
        public string SoNgayBaoHanh { get; set; }
        public string KiemTraTruocKhiNhapKho { get; set; }
        public string TheoDoi { get; set; }
    }
}
