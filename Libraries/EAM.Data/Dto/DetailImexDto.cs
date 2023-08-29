using System;

namespace EAM.Data.Dto
{
    public class DetailImexDto
    {
        public string Store { get; set; }
        public string PartFullName { get; set; }
        public string CODE { get; set; }
        public DateTime TRA_DATE { get; set; }
        public string DESCRIPTION { get; set; }
        public decimal TRL_PRICE { get; set; }
        public decimal SLNHAP { get; set; }
        public decimal THANHTIENNHAP { get; set; }
        public decimal SLXUAT { get; set; }
        public decimal THANHTIENXUAT { get; set; }
        public decimal STOCKAFTER { get; set; }
        public decimal STOCKVAlUEAFTER { get; set; }

        public string par_code { get; set; }
        public string par_desc { get; set; }
        public string par_uom { get; set; }

    }
}
