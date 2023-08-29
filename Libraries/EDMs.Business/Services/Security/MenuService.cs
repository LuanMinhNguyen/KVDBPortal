namespace EDMs.Business.Services.Security
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;
    using EDMs.Data.DAO.Security;

    public class MenuService
    {      
        private readonly MenuDAO repo;

        public MenuService()
        {
            this.repo = new MenuDAO();
        }
        #region GET (Basic)
        /// <summary>
        /// Get All Role
        /// </summary>
        /// <returns></returns>
        public List<Menu> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// Get Role By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Menu GetByID(int ID)
        {
            return this.repo.GetByID( ID);
        }
        #endregion

        #region Get (Advances)

        /// <summary>
        /// Gets all menus by type.
        /// </summary>
        /// <param name="menuType">Type of the menu.</param>
        /// <returns></returns>
        public List<Menu> GetAllMenusByType(int menuType)
        {
            return this.repo.GetAllMenusByType(menuType);
        }

        /// <summary>
        /// Gets all related permitted menu items.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <param name="menuType">Type of the menu.</param>
        /// <returns></returns>
        public List<Menu> GetAllRelatedPermittedMenuItems(int roleId, int menuType)
        {
            return this.repo.GetAllRelatedPermittedMenuItems(roleId, menuType);
        }

        #endregion

        #region Insert, Update, Delete
        public bool Update(Menu ob)
        {
            return this.repo.Update(ob);
        }
        #endregion
    }
}
