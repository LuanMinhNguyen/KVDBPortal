using EAM.Data.DAO.Security;

namespace EAM.Data.Entities
{
    public partial class AA_Users
    {
        private string fullNameWithPosition;


        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public AA_Roles Role
        {
            get
            {
                var dao = new AA_RolesDAO();
                return RoleId != null ? dao.GetByID(RoleId.Value) : null;
            }
        }

        public int ParentId { get; set; }

        public string UserNameWithFullName
        {
            get
            {
                return !string.IsNullOrEmpty(this.FullName) ? this.Username + "/" + this.FullName + " - " + this.RoleName : this.Username + " - " + this.RoleName;
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
                    ? this.Role.NameWithLocation + this.TitleName + " - " + this.FullName
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
