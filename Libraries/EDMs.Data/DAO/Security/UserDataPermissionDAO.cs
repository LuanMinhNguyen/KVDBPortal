// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserDataPermissionDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Data.Entities;

namespace EDMs.Data.DAO.Security
{
    /// <summary>
    /// The category dao.
    /// </summary>
    public class UserDataPermissionDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDataPermissionDAO"/> class.
        /// </summary>
        public UserDataPermissionDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<UserDataPermission> GetIQueryable()
        {
            return EDMsDataContext.UserDataPermissions;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<UserDataPermission> GetAll()
        {
            return EDMsDataContext.UserDataPermissions.ToList();
        }

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Resource"/>.
        /// </returns>
        public UserDataPermission GetById(int id)
        {
            return this.EDMsDataContext.UserDataPermissions.FirstOrDefault(ob => ob.ID == id);
        }
       
        #endregion

        #region GET ADVANCE

        /// <summary>
        /// The get by role id.
        /// </summary>
        /// <param name="roleId">
        /// The role id.
        /// </param>
        /// <returns>
        /// The <see cref="UserDataPermission"/>.
        /// </returns>
        public List<UserDataPermission> GetByUserId(int userId)
        {
            return this.EDMsDataContext.UserDataPermissions.Where(t => t.UserId == userId).ToList();
        }

        //public List<UserDataPermission> GetByRoleId(int roleId, string categoryId)
        //{
        //    return this.EDMsDataContext.UserDataPermissions.Where(t => t.RoleId == roleId && t.CategoryIdList == categoryId).ToList();
        //}

        public UserDataPermission GetByUserId(int userId, int categoryId, int folderId)
        {
            return this.EDMsDataContext.UserDataPermissions.FirstOrDefault(t => t.UserId == userId && t.CategoryId == categoryId && t.FolderId == folderId);
        }
        public UserDataPermission GetByUserId(int userId,  int folderId)
        {
            return this.EDMsDataContext.UserDataPermissions.FirstOrDefault(t => t.UserId == userId  && t.FolderId == folderId);
        }
        /// <summary>
        /// The get all by folder.
        /// </summary>
        /// <param name="folderId">
        /// The folder id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<UserDataPermission> GetAllByFolder(int folderId)
        {
            return this.EDMsDataContext.UserDataPermissions.Where(t => t.FolderId == folderId).ToList();
        }
        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// Deletes the permissions.
        /// </summary>
        /// <returns></returns>
        public bool DeleteUserDataPermission(List<UserDataPermission> UserDataPermission)
        {
            try
            {
                foreach (var item in UserDataPermission)
                {
                    EDMsDataContext.DeleteObject(item);
                    EDMsDataContext.SaveChanges();
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
        public bool AddUserDataPermissions(List<UserDataPermission> UserDataPermission)
        {
            try
            {
                foreach (var item in UserDataPermission)
                {
                    EDMsDataContext.AddToUserDataPermissions(item);
                    EDMsDataContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="ob">
        /// The ob.
        /// </param>
        /// <returns>
        /// The <see cref="int?"/>.
        /// </returns>
        public int? Insert(UserDataPermission ob)
        {
            try
            {
                EDMsDataContext.AddToUserDataPermissions(ob);
                EDMsDataContext.SaveChanges();
                return ob.ID;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="src">
        /// The src.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True if delete success, false if not
        /// </returns>
        public bool Delete(UserDataPermission src)
        {
            try
            {
                var des = this.GetById(src.ID);
                if (des != null)
                {
                    EDMsDataContext.DeleteObject(des);
                    EDMsDataContext.SaveChanges();
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
        /// <param name="ID"></param>
        /// ID of entity
        /// <returns></returns>
        public bool Delete(int ID)
        {
            try
            {
                var des = this.GetById(ID);
                if (des != null)
                {
                    EDMsDataContext.DeleteObject(des);
                    EDMsDataContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
