
namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO.Library;

    public partial class Unit
    {
        public string AreaCode
        {
            get
            {
                var DAO = new AreaDAO();
                 return this.AreaId != null ?DAO.GetById(this.AreaId.GetValueOrDefault()).Code :"";
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
