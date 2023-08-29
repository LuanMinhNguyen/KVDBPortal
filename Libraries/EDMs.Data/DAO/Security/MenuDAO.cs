namespace EDMs.Data.DAO.Security
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    public class MenuDAO : BaseDAO
    {
        public MenuDAO() : base() { }

        #region GET (Basic)
        public IQueryable<Menu> GetIQueryable()
        {
            return this.EDMsDataContext.Menus;
        }
        
        public List<Menu> GetAll()
        {
            return this.EDMsDataContext.Menus.ToList();
        }

        public Menu GetByID(int ID)
        {
            return this.EDMsDataContext.Menus.FirstOrDefault(ob => ob.Id == ID);
        }
       
        #endregion

        #region Get (Advances)

        /// <summary>
        /// Gets the permitted menus by role id.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <returns></returns>
        private List<Menu> GetPermittedMenusByRoleId(int roleId)
        {
            var permissions = this.EDMsDataContext.Permissions.Where(permission => permission.RoleId == roleId);
            return permissions.Select(permission => this.EDMsDataContext.Menus.FirstOrDefault(ob => 
                ob.Id == permission.MenuId)).Where(menu => menu != null).ToList();
        }

        /// <summary>
        /// Gets the type of all menus by.
        /// </summary>
        /// <param name="menuType">Type of the menu.</param>
        /// <returns></returns>
        public List<Menu> GetAllMenusByType(int menuType)
        {
            return this.EDMsDataContext.Menus.Where(x => x.Type == menuType && x.Active == true).ToList();
        }

        /// <summary>
        /// Gets all parents menu items by children.
        /// </summary>
        /// <param name="children">The children.</param>
        /// <returns></returns>
        private IEnumerable<Menu> GetAllParentsMenuItemsByChildren(IEnumerable<Menu> children)
        {
            var parents = new List<Menu>();
            foreach (var child in children)
            {
                var temp = child;
                while (temp.ParentId != null)
                {
                    var parentMenu = this.GetByID(temp.ParentId.Value);
                    temp = parentMenu;

                    if (parents.All(x => x.Id != parentMenu.Id))
                    {
                        parents.Add(parentMenu);
                    }
                }
            }
            return parents;
        }

        /// <summary>
        /// Gets all related permitted menu items.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <param name="menuType">Type of the menu.</param>
        /// <returns></returns>
        public List<Menu> GetAllRelatedPermittedMenuItems(int roleId, int menuType)
        {
            //Get all menu have permitted for current role.
            var menus = this.GetPermittedMenusByRoleId(roleId);

            if(menus != null)
            {
                //Gets the menus by type.
                menus = menus.Where(menu => menu.Type == menuType).ToList();

                //Gets and adds all parent menu items into the list.
                menus.AddRange(this.GetAllParentsMenuItemsByChildren(menus));
                return menus;
            }
            return new List<Menu>();
        }


        #endregion

        #region Insert, Update, Delete
        public bool Update(Menu ob)
        {
            try
            {
                Menu _ob;

                _ob = (from rs in this.EDMsDataContext.Menus
                       where rs.Id == ob.Id
                       select rs).First();

                _ob.Name = ob.Name;
                _ob.Description = ob.Description;
                _ob.Url = ob.Url;
                _ob.ParentId = ob.ParentId;
                _ob.Active = ob.Active;

                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
