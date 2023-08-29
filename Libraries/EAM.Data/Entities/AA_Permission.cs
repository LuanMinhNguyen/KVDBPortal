using EAM.Data.DAO.Security;

namespace EAM.Data.Entities
{
    public partial class AA_Permissions
    {
        private int parentId;

        private string menuName;

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
        /// Gets the menu.
        /// </summary>
        /// <value>
        /// The menu.
        /// </value>
        public AA_Menus Menu
        {
            get
            {
                var dao = new AA_MenusDAO();
                return dao.GetByID(MenuId);
            }
        }

        /// <summary>
        /// Gets the menu.
        /// </summary>
        /// <value>
        /// The menu.
        /// </value>
        public string MenuName
        {
            get
            {
                return this.menuName;
            }
            set
            {
                this.menuName = value;
            }
        }
    }
}
