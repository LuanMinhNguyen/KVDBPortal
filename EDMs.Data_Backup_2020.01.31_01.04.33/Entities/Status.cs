namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO;
    using EDMs.Data.DAO.Document;

    public partial class Status
    {
        public Category Category
        {
            get
            {
                var dao = new CategoryDAO();
                return dao.GetById(this.CategoryId.GetValueOrDefault());
            }
        }

        public string FullNameWithWeight
        {
            get
            {
                return this.Name +
                       (this.PercentCompleteDefault != null && this.PercentCompleteDefault.Value != 0
                           ? " (" + this.PercentCompleteDefault.Value.ToString() + "%)"
                           : string.Empty);
            }
        }
    }
}
