using System;
using System.Collections.Generic;
using System.Linq;
using EAM.Data.Entities;

namespace EAM.Data.DAO.Security
{
    public class AA_PermissionsDAO : BaseDAO
    {
        public AA_PermissionsDAO() : base() { }

        #region GET (Basic)
        public IQueryable<AA_Permissions> GetIQueryable()
        {
            return this.EDMsDataContext.AA_Permissions;
        }

        public List<AA_Permissions> GetAll()
        {
            return this.EDMsDataContext.AA_Permissions.ToList();
        }

        public AA_Permissions GetById(int id)
        {
            return this.EDMsDataContext.AA_Permissions.FirstOrDefault(ob => ob.Id == id);
        }
       
        #endregion

        #region Get (Advances)

        public List<AA_Permissions> GetByRoleId(int roleId)
        {
            return this.EDMsDataContext.AA_Permissions.Where(ob => ob.RoleId == roleId).ToList();
        }

        public List<AA_Permissions> GetByRoleId(int roleId, int specialParent)
        {
            return this.EDMsDataContext.AA_Permissions.ToArray().Where(ob => ob.RoleId == roleId && 
                                                                    ob.Menu.ParentId == specialParent).ToList();
        }

        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// Deletes the AA_Permissions.
        /// </summary>
        /// <param name="AA_Permissions">The AA_Permissions.</param>
        /// <returns></returns>
        public bool DeleteAA_Permissions(List<AA_Permissions> AA_Permissions)
        {
            try
            {
                foreach (var item in AA_Permissions)
                {
                    this.EDMsDataContext.DeleteObject(item);
                    this.EDMsDataContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Adds the AA_Permissions.
        /// </summary>
        /// <param name="AA_Permissions">The AA_Permissions.</param>
        /// <returns></returns>
        public bool AddAA_Permissions(List<AA_Permissions> AA_Permissions)
        {
            try
            {
                foreach (var item in AA_Permissions)
                {
                    this.EDMsDataContext.AddToAA_Permissions(item);
                    this.EDMsDataContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Delete(AA_Permissions src)
        {
            try
            {
                var des = this.GetById(src.Id);
                if (des != null)
                {
                    this.EDMsDataContext.DeleteObject(des);
                    this.EDMsDataContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Delete By ID
        /// </summary>
        /// <param name="id"></param>
        /// ID of entity
        /// <returns></returns>
        public bool Delete(int id)
        {
            try
            {
                var des = this.GetById(id);
                if (des != null)
                {
                    this.EDMsDataContext.DeleteObject(des);
                    this.EDMsDataContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }


        public int? Insert(AA_Permissions ob)
        {
            try
            {
                this.EDMsDataContext.AddToAA_Permissions(ob);
                this.EDMsDataContext.SaveChanges();
                return ob.Id;
            }
            catch
            {
                return null;
            }
        }
        #endregion

    }
}
