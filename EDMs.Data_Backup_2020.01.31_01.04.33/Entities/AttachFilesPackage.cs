
using EDMs.Data.DAO.Security;

namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO.Scope;

    /// <summary>
    /// The package.
    /// </summary>
    public partial class AttachFilesPackage
    {
        /// <summary>
        /// Gets the project obj.
        /// </summary>
        public string CreatedByUser
        {
            get
            {
                var userDAO = new UserDAO();
                var user = userDAO.GetByID(this.CreatedBy.GetValueOrDefault());
                return user != null ? user.FullName : string.Empty;
            }
        }
    }
}
