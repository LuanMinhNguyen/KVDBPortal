using EDMs.Data.DAO.Security;

namespace EDMs.Data.Entities
{
    /// <summary>
    /// The role.
    /// </summary>
    public partial class UserDataPermission
    {
        public string UserFullName
        {
            get
            {
                var userDao = new UserDAO();
                var userObj = userDao.GetByID(this.UserId.GetValueOrDefault());
                return userObj != null ? userObj.FullName : string.Empty;
            }
        }
    }
}
