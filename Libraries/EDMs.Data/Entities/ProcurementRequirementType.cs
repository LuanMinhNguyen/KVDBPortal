
namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO.Scope;

    /// <summary>
    /// The package.
    /// </summary>
    public partial class ProcurementRequirementType
    {
        public string FullName
        {
            get
            {
                return !string.IsNullOrEmpty(this.RusName)
                        ? this.Name + " - " + this.RusName 
                        : this.Name;
            }
        }
    }
}
