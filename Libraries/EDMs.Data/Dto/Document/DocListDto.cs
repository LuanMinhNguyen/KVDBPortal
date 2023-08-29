using System;

namespace EDMs.Data.Dto.Document
{
    public class DocListDto
    {
        public string FullName { get; set; }
        public string MaTaiLieu { get; set; }
        public string TenTaiLieu { get; set; }
        public string TieuDe { get; set; }
        public string TenTep { get; set; }
        public string DuongDan { get; set; }
        public DateTime NgayCapNhat { get; set; }
    }
}
