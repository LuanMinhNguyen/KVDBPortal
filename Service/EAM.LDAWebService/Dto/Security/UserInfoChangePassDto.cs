using System;
using System.Runtime.Serialization;
using SaleManagement.Entity;

namespace QLTV.WebService.Dto.Security
{
    [DataContract]
    public class UserInfoChangePassDto
    {
        public UserInfoChangePassDto() {}

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string NewPass { get; set; }

        [DataMember]
        public string OldPass { get; set; }

    }
}