// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentNew.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the DocumentNew type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using EDMs.Data.DAO.Security;

namespace EDMs.Data.Entities
{
    using System.Linq;

    using EDMs.Data.DAO.Document;

    /// <summary>
    /// The document new.
    /// </summary>
    public partial class DocumentPackage
    {
        public string DepartmentShortName
        {
            get
            {
                var roleDAO = new RoleDAO();
                var role = roleDAO.GetByID(this.DeparmentId.GetValueOrDefault());
                return role != null ? role.Name : string.Empty;
            }
        }

        public bool HasAttachFile
        {
            get
            {
                var temp = new AttachFilesPackageDAO();
                return temp.GetAll().Any(t => t.DocumentPackageID == this.ID);
                return false;
            }
        }
    }
}
