namespace EDMs.Data.DAO.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    public class PermissionDAO : BaseDAO
    {
        public PermissionDAO() : base() { }

        #region GET (Basic)
        public IQueryable<Permission> GetIQueryable()
        {
            return this.EDMsDataContext.Permissions;
        }

        public List<Permission> GetAll()
        {
            return this.EDMsDataContext.Permissions.ToList();
        }

        public Permission GetById(int id)
        {
            return this.EDMsDataContext.Permissions.FirstOrDefault(ob => ob.Id == id);
        }
       
        #endregion

        #region Get (Advances)

        public List<Permission> GetByRoleId(int roleId)
        {
            return this.EDMsDataContext.Permissions.Where(ob => ob.RoleId == roleId).ToList();
        }

        public List<Permission> GetByRoleId(int roleId, int specialParent)
        {
            return this.EDMsDataContext.Permissions.ToArray().Where(ob => ob.RoleId == roleId && 
                                                                    ob.Menu.ParentId == specialParent).ToList();
        }

        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// Deletes the permissions.
        /// </summary>
        /// <param name="permissions">The permissions.</param>
        /// <returns></returns>
        public bool DeletePermissions(List<Permission> permissions)
        {
            try
            {
                foreach (var item in permissions)
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
        /// Adds the permissions.
        /// </summary>
        /// <param name="permissions">The permissions.</param>
        /// <returns></returns>
        public bool AddPermissions(List<Permission> permissions)
        {
            try
            {
                foreach (var item in permissions)
                {
                    this.EDMsDataContext.AddToPermissions(item);
                    this.EDMsDataContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Delete(Permission src)
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


        public int? Insert(Permission ob)
        {
            try
            {
                this.EDMsDataContext.AddToPermissions(ob);
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
