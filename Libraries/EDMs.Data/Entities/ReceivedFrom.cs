namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO;
    using EDMs.Data.DAO.Document;

    public partial class ReceivedFrom
    {
        public Category Category
        {
            get
            {
                var dao = new CategoryDAO();
                return dao.GetById(this.CategoryId.GetValueOrDefault());
            }
        }
    }
}
