
namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO.Library;

    public partial class Area
    {
        public string PlantCode
        {
            get
            {
                var DAO = new PlantDAO();
                 return this.PlantId != null ?DAO.GetById(this.PlantId.GetValueOrDefault()).Name:"";
            }

        }

        public string FullName
        {
            get
            {
                return !string.IsNullOrEmpty(this.Description)
                        ? this.Code + ", " + this.Description
                        : this.Code;
            }
        }
    }
}
