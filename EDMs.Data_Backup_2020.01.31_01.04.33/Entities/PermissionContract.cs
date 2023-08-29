
namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO.Security;

    public partial class PermissionContract
    {
        public User UserObj
        {
            get
            {
                var userDAO = new UserDAO();
                return userDAO.GetByID(this.UserId.GetValueOrDefault());
            }
        }

        public string UserNameWithFullName
        {
            get
            {
                return !string.IsNullOrEmpty(this.UserObj.FullName) ? this.UserObj.Username + "/" + this.UserObj.FullName : this.UserObj.Username;
            }
        }

        public Role GroupObj
        {
            get
            {
                if (this.UserObj != null)
                {
                    var roleDAO = new RoleDAO();
                    return roleDAO.GetByID(this.UserObj.RoleId.GetValueOrDefault());
                }

                return new Role();
            }
        }
    }
}
