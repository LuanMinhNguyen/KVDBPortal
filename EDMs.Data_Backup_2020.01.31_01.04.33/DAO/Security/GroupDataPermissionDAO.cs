// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupDataPermissionDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data.DAO.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    /// <summary>
    /// The category dao.
    /// </summary>
    public class GroupDataPermissionDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupDataPermissionDAO"/> class.
        /// </summary>
        public GroupDataPermissionDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<GroupDataPermission> GetIQueryable()
        {
            return this.EDMsDataContext.GroupDataPermissions;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<GroupDataPermission> GetAll()
        {
            return this.EDMsDataContext.GroupDataPermissions.ToList();
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
        public GroupDataPermission GetById(int id)
        {
            return this.EDMsDataContext.GroupDataPermissions.FirstOrDefault(ob => ob.ID == id);
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
        /// The <see cref="GroupDataPermission"/>.
        /// </returns>
        public List<GroupDataPermission> GetByRoleId(int roleId)
        {
            return this.EDMsDataContext.GroupDataPermissions.Where(t => t.RoleId == roleId).ToList();
        }

        public List<GroupDataPermission> GetByRoleId(int roleId, string categoryId)
        {
            return this.EDMsDataContext.GroupDataPermissions.Where(t => t.RoleId == roleId && t.CategoryIdList == categoryId).ToList();
        }

        public GroupDataPermission GetByRoleId(int roleId, string categoryId, string folderId)
        {
            return this.EDMsDataContext.GroupDataPermissions.FirstOrDefault(t => t.RoleId == roleId && t.CategoryIdList == categoryId && t.FolderIdList == folderId);
        }
        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// Deletes the permissions.
        /// </summary>
        /// <param name="permissions">The permissions.</param>
        /// <returns></returns>
        public bool DeleteGroupDataPermission(List<GroupDataPermission> groupDataPermission)
        {
            try
            {
                foreach (var item in groupDataPermission)
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
        public bool AddGroupDataPermissions(List<GroupDataPermission> groupDataPermission)
        {
            try
            {
                foreach (var item in groupDataPermission)
                {
                    this.EDMsDataContext.AddToGroupDataPermissions(item);
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
        /// The insert.
        /// </summary>
        /// <param name="ob">
        /// The ob.
        /// </param>
        /// <returns>
        /// The <see cref="int?"/>.
        /// </returns>
        public int? Insert(GroupDataPermission ob)
        {
            try
            {
                this.EDMsDataContext.AddToGroupDataPermissions(ob);
                this.EDMsDataContext.SaveChanges();
                return ob.ID;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="src">
        /// Entity for update
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True if update success, false if not
        /// </returns>
        public bool Update(GroupDataPermission src)
        {
            try
            {
                GroupDataPermission des = (from rs in this.EDMsDataContext.GroupDataPermissions
                                where rs.ID == src.ID
                                select rs).First();

                des.CategoryIdList = src.CategoryIdList;
                des.FolderIdList = src.FolderIdList;
                des.RoleId = src.RoleId;

                des.LastUpdatedDate = src.LastUpdatedDate;
                des.LastUpdatedBy = src.LastUpdatedBy;

                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
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
        public bool Delete(GroupDataPermission src)
        {
            try
            {
                var des = this.GetById(src.ID);
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
        #endregion
    }
}
