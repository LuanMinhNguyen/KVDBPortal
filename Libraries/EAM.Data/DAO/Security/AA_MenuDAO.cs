using System.Collections.Generic;
using System.Linq;
using EAM.Data.Entities;

namespace EAM.Data.DAO.Security
{
    public class AA_MenusDAO : BaseDAO
    {
        public AA_MenusDAO() : base() { }

        #region GET (Basic)
        public IQueryable<AA_Menus> GetIQueryable()
        {
            return this.EDMsDataContext.AA_Menus;
        }
        
        public List<AA_Menus> GetAll()
        {
            return this.EDMsDataContext.AA_Menus.ToList();
        }

        public AA_Menus GetByID(int ID)
        {
            return this.EDMsDataContext.AA_Menus.FirstOrDefault(ob => ob.Id == ID);
        }
       
        #endregion

        #region Get (Advances)

        /// <summary>
        /// Gets the permitted AA_Menus by role id.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <returns></returns>
        private List<AA_Menus> GetPermittedAA_MenusByRoleId(int roleId)
        {
            var permissions = this.EDMsDataContext.AA_Permissions.Where(permission => permission.RoleId == roleId);
            return permissions.Select(permission => this.EDMsDataContext.AA_Menus.FirstOrDefault(ob => 
                ob.Id == permission.MenuId)).Where(AA_Menus => AA_Menus != null).ToList();
        }

        /// <summary>
        /// Gets the type of all AA_Menus by.
        /// </summary>
        /// <param name="AA_MenusType">Type of the AA_Menus.</param>
        /// <returns></returns>
        public List<AA_Menus> GetAllAA_MenusByType(int AA_MenusType)
        {
            return this.EDMsDataContext.AA_Menus.Where(x => x.Type == AA_MenusType && x.Active == true).ToList();
        }

        /// <summary>
        /// Gets all parents AA_Menus items by children.
        /// </summary>
        /// <param name="children">The children.</param>
        /// <returns></returns>
        private IEnumerable<AA_Menus> GetAllParentsAA_MenusItemsByChildren(IEnumerable<AA_Menus> children)
        {
            var parents = new List<AA_Menus>();
            foreach (var child in children)
            {
                var temp = child;
                while (temp.ParentId != null)
                {
                    var parentAA_Menus = this.GetByID(temp.ParentId.Value);
                    temp = parentAA_Menus;

                    if (parents.All(x => x.Id != parentAA_Menus.Id))
                    {
                        parents.Add(parentAA_Menus);
                    }
                }
            }
            return parents;
        }

        /// <summary>
        /// Gets all related permitted AA_Menus items.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <param name="AA_MenusType">Type of the AA_Menus.</param>
        /// <returns></returns>
        public List<AA_Menus> GetAllRelatedPermittedAA_MenusItems(int roleId, int AA_MenusType)
        {
            //Get all AA_Menus have permitted for current role.
            var menuList = this.GetPermittedAA_MenusByRoleId(roleId);

            if(menuList != null)
            {
                //Gets the AA_Menus by type.
                menuList = menuList.Where(AA_Menus => AA_Menus.Type == AA_MenusType).ToList();

                //Gets and adds all parent AA_Menus items into the list.
                menuList.AddRange(this.GetAllParentsAA_MenusItemsByChildren(menuList));
                return menuList;
            }
            return new List<AA_Menus>();
        }


        #endregion

        #region Insert, Update, Delete
        public bool Update(AA_Menus ob)
        {
            try
            {
                AA_Menus _ob;

                _ob = (from rs in this.EDMsDataContext.AA_Menus
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
