
namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO.Library;

    public partial class OptionalTypeDetail
    {
        public OptionalType OptionalType
        {
            get
            {
                var optionalTypeDAO = new OptionalTypeDAO();
                return optionalTypeDAO.GetById(this.OptionalTypeId.GetValueOrDefault());
            }
        }

        public string FullName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Description) && this.Description.Trim() != this.Name.Trim())
                {
                    return this.Name + " - " + this.Description;
                }

                return this.Name;
            }
        }
    }
}
