using System;
using System.Linq;

namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO.Document;
    using EDMs.Data.DAO.Security;

    /// <summary>
    /// The role.
    /// </summary>
    public partial class Role
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

        /// <summary>
        /// Gets the categories.
        /// </summary>
        public string Categories
        {
            get
            {
                var groupDataPermissionDAO = new GroupDataPermissionDAO();
                var categoryDAO = new CategoryDAO();
                var strCategory = string.Empty;
                var selectedCategory = groupDataPermissionDAO.GetByRoleId(this.Id);
                strCategory = selectedCategory
                    .Select(groupDataPermission => categoryDAO.GetById(Convert.ToInt32(groupDataPermission.CategoryIdList)))
                    .Where(category => category != null)
                    .Aggregate(strCategory, (current, category) => current + ("_ " + category.Name + "<br/>"));

                return strCategory;
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
