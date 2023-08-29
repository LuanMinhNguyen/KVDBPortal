using EAM.Data.DAO.Security;

namespace EAM.Data.Entities
{
    public partial class AA_Roles
    {
        /// <summary>
        /// The parent id.
        /// </summary>
        private int parentId;

        private string fullNameWithLocation;


        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        public int ParentId
        {
            get
            {
                return this.parentId;
            }

            set
            {
                this.parentId = value;
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

        public string NameWithLocation
        {
            get
            {
                return "(" + this.TypeName + " - " + this.Name + ") ";
            }
        }

        public string FullNameWithLocation
        {
            get
            {
                return
                    string.IsNullOrEmpty(this.fullNameWithLocation)
                    ? (!string.IsNullOrEmpty(this.ContractorName)
                        ? "(" + this.ContractorName.Split(',')[0] + ") " + this.Name
                        : this.Name)
                    : this.fullNameWithLocation;
            }
            set { fullNameWithLocation = value; }
        }
    }
}
