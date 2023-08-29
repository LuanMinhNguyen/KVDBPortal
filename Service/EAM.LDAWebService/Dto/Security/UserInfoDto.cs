using System;
using System.Runtime.Serialization;
using SaleManagement.Entity;

namespace QLTV.WebService.Dto.Security
{
    [DataContract]
    public class UserInfoDto
    {
        public UserInfoDto() {}
        public UserInfoDto(User userObj)
        {
            this.UserId = userObj.ID;
            this.UserName = userObj.UserName;
            this.Password = userObj.Password;
            this.FullName = userObj.FullName;
            this.RoleId = userObj.GroupId.GetValueOrDefault();
            this.RoleName = userObj.GroupFullName;
            this.Email = userObj.Email;
            this.Phone = userObj.Phone;
        }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public int RoleId { get; set; }

        [DataMember]
        public string RoleName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Phone { get; set; }

    }
}