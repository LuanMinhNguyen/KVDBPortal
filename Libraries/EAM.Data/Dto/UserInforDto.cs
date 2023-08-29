using System.Collections.Generic;

namespace EAM.Data.Dto
{
    public class UserInforDto
    {
        public string TENNGUOIDUNG { get; set; }
        public string MANGUOIDUNG { get; set; }
        public string MANHOM { get; set; }
        public string TENNHOM { get; set; }
        public List<OrganizationDto> Organizations { get; set; }
    }
}
