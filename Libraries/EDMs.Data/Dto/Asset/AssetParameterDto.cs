using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMs.Data.Dto.Asset
{
    public class AssetParameterDto
    {
        public string DonViDo { get; set; }
        public string MoTa { get; set; }
        public int TotalUsage { get; set; }
        public int UsageSinceInstall { get; set; }
        public int UsageSinceLastWO { get; set; }
        public string TypeOfMeter { get; set; }
        public string PhysicalMeter { get; set; }
        public string MeterRollover { get; set; }
        public int LastReading { get; set; }
        public string LastReadingDate { get; set; }
        public string EstDailyUsage { get; set;}
        public string AvgDailyUsageDate { get; set; }
        public string ReadingsForCalc { get; set; }
        public string DaysSinceLastEntry { get; set; }
        public string UpDownMeter { get; set; }
    }
}
