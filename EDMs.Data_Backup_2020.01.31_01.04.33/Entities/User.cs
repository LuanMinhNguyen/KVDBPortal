using EDMs.Data.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO.Library;
    using EDMs.Data.DAO.Security;

    public partial class User
    {
        private string fullNameWithPosition;


        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public Role Role
        {
            get
            {
                var dao = new RoleDAO();
                return RoleId != null ? dao.GetByID(RoleId.Value) : null;
            }
        }

        public int ParentId { get; set; }

        public string UserNameWithFullName
        {
            get
            {
                return  !string.IsNullOrEmpty(this.FullName) ? this.Username + "/" + this.FullName + " - " + this.RoleName : this.Username + " - " + this.RoleName;
            }
        }

        public string UserNameWithFullNamePosition
        {
            get
            {
                return (!string.IsNullOrEmpty(this.FullName) ? this.Username + "/" + this.FullName : this.Username) + (!string.IsNullOrEmpty(this.Position) ? " (" + this.Position + ")" : string.Empty);
            }
        }

        public string FullNameWithPosition
        {
            get
            {
                return !string.IsNullOrEmpty(fullNameWithPosition) ? fullNameWithPosition : (!string.IsNullOrEmpty(this.Position) ? this.FullName + " (" + this.Position + ")" : this.FullName);
            }
            set
            {
                fullNameWithPosition = value;
            }
        }

        public string FullNameWithDeptPosition
        {
            get
            {
                return !string.IsNullOrEmpty(this.TitleName) 
                    ?  this.Role.NameWithLocation +  this.TitleName + " - " + this.FullName
                    : this.Role.NameWithLocation + this.FullName;
            }
        }

        public string FullNameWithEmail
        {
            get { return this.Email + " (" + this.FullName + ")"; }
        }

        public string ActionTypeName { get; set; }
        public int ActionTypeId { get; set; }

    }
}
