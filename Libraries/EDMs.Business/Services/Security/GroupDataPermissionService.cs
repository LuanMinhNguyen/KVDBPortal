namespace EDMs.Business.Services.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO.Security;
    using EDMs.Data.Entities;

    /// <summary>
    /// The category service.
    /// </summary>
    public class GroupDataPermissionService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly GroupDataPermissionDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupDataPermissionService"/> class.
        /// </summary>
        public GroupDataPermissionService()
        {
            this.repo = new GroupDataPermissionDAO();
        }

        public bool DeleteGroupDataPermission(List<GroupDataPermission> groupDataPermission)
        {
            return this.repo.DeleteGroupDataPermission(groupDataPermission);
        }

        public bool AddGroupDataPermissions(List<GroupDataPermission> groupDataPermission)
        {
            return this.repo.AddGroupDataPermissions(groupDataPermission);
        }
        #region Get (Advances)
        public List<GroupDataPermission> GetByRoleId(int roleId, string categoryId)
        {
            return this.repo.GetByRoleId(roleId, categoryId);
        }
        public GroupDataPermission GetByRoleId(int roleId, string categoryId, string folderId)
        {
            return this.repo.GetByRoleId(roleId, categoryId, folderId);
        }

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
            return this.repo.GetByRoleId(roleId);
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<GroupDataPermission> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public GroupDataPermission GetById(int id)
        {
            return this.repo.GetById(id);
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public int? Insert(GroupDataPermission bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(GroupDataPermission bo)
        {
            try
            {
                return this.repo.Update(bo);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(GroupDataPermission bo)
        {
            try
            {
                return this.repo.Delete(bo);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete Resource By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            try
            {
                return this.repo.Delete(id);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

    }
}
