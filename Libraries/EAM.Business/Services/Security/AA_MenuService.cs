using System.Collections.Generic;
using System.Linq;
using EAM.Data.DAO.Security;
using EAM.Data.Entities;

namespace EAM.Business.Services.Security
{
    public class AA_MenusService
    {      
        private readonly AA_MenusDAO repo;

        public AA_MenusService()
        {
            this.repo = new AA_MenusDAO();
        }
        #region GET (Basic)
        /// <summary>
        /// Get All Role
        /// </summary>
        /// <returns></returns>
        public List<AA_Menus> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// Get Role By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AA_Menus GetByID(int ID)
        {
            return this.repo.GetByID( ID);
        }
        #endregion

        #region Get (Advances)

        /// <summary>
        /// Gets all AA_Menuss by type.
        /// </summary>
        /// <param name="AA_MenusType">Type of the AA_Menus.</param>
        /// <returns></returns>
        public List<AA_Menus> GetAllMenusByType(int AA_MenusType)
        {
            return this.repo.GetAllAA_MenusByType(AA_MenusType);
        }

        /// <summary>
        /// Gets all related permitted AA_Menus items.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <param name="AA_MenusType">Type of the AA_Menus.</param>
        /// <returns></returns>
        public List<AA_Menus> GetAllRelatedPermittedMenuItems(int roleId, int AA_MenusType)
        {
            return this.repo.GetAllRelatedPermittedAA_MenusItems(roleId, AA_MenusType);
        }

        #endregion

        #region Insert, Update, Delete
        public bool Update(AA_Menus ob)
        {
            return this.repo.Update(ob);
        }
        #endregion
    }
}
