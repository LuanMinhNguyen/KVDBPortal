
using EDMs.Data.DAO.Security;

namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO.Scope;

    /// <summary>
    /// The package.
    /// </summary>
    public partial class TemplateManagement
    {
        public string ProjectName
        {
            get
            {
                var projectDAO = new ScopeProjectDAO();
                var project = projectDAO.GetById(this.ProjectId.GetValueOrDefault());
                return project != null ? project.Name : string.Empty;
            }
        }
    }
}
