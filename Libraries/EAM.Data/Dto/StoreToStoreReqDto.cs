using System.Collections.Generic;

namespace EAM.Data.Dto
{
    public class StoreToStoreReqDto
    {
        public string Seq { get; set; }
        public string FromCode { get; set; }
        public string FromCodeName { get; set; }
        public string ToCode { get; set; }
        public string ToCodeName { get; set; }
        public string UserName { get; set; }
        public List<StoreToStoreReqLineDto> LineList { get; set; }
    }
}
