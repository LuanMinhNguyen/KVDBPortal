
namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO.Library;

    public partial class Well
    {
        public Platform Platform
        {
            get
            {
                var platformDAO = new PlatformDAO();
                return platformDAO.GetById(this.PlatformId.GetValueOrDefault());
            }
        }
    }
}
