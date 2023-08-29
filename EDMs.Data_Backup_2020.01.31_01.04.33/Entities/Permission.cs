using EDMs.Data.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO.Security;

    public partial class Permission
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
        public Menu Menu
        {
            get
            {
                var dao = new MenuDAO();
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
