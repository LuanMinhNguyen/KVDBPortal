using System;
using System.Runtime.Serialization;
using SaleManagement.Entity;

namespace QLTV.WebService.Dto.Security
{
    [DataContract]
    public class UserInfoUpdateDto
    {
        public UserInfoUpdateDto() {}
        public UserInfoUpdateDto(User userObj)
        {
            this.UserId = userObj.ID;
            this.FullName = userObj.FullName;
            this.Email = userObj.Email;
            this.Phone = userObj.Phone;
        }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Phone { get; set; }
    }
}