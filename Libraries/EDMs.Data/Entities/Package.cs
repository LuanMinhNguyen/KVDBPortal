
namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO.Scope;

    /// <summary>
    /// The package.
    /// </summary>
    public partial class Package
    {
        /// <summary>
        /// Gets the project obj.
        /// </summary>
        public ScopeProject ProjectObj
        {
            get
            {
                var projectDAO = new ScopeProjectDAO();
                return projectDAO.GetById(this.ProjectId.GetValueOrDefault());
            }
        }

        public string FullName
        {
            get
            {
                return !string.IsNullOrEmpty(this.Description) 
                        ? this.Name + " - " + this.Description 
                        : this.Name;
            }
        }
    }
}
