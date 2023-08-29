using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Data.DAO.Security;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.Security
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class UserDataPermissionService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly UserDataPermissionDAO repo;

        private readonly WaitingSyncDataService waitingSyncDataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDataPermissionService"/> class.
        /// </summary>
        public UserDataPermissionService()
        {
            this.repo = new UserDataPermissionDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();
        }


        public bool DeleteUserDataPermission(List<UserDataPermission> UserDataPermission)
        {
            return this.repo.DeleteUserDataPermission(UserDataPermission);
        }

        public bool AddUserDataPermissions(List<UserDataPermission> UserDataPermission)
        {
            return this.repo.AddUserDataPermissions(UserDataPermission);
        }
        #region Get (Advances)
        //public List<UserDataPermission> GetByRoleId(int roleId, string categoryId)
        //{
        //    return this.repo.GetByRoleId(roleId, categoryId);
        //}
        public UserDataPermission GetByUserId(int userId, int categoryId, int folderId)
        {
            return this.repo.GetByUserId(userId, categoryId, folderId);
        }
        public UserDataPermission GetByUserId(int userId, int folderId)
        {
            return this.repo.GetByUserId(userId, folderId);
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
            return this.repo.GetAllByFolder(folderId);
        }

        //public List<int> GetAllGroupId(string folder)
        //{
        //    return this.repo.GetAllByFolder(folder).Select(t => t.RoleId.GetValueOrDefault()).Distinct().ToList();
        //}

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
            return this.repo.GetByUserId(userId);
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<UserDataPermission> GetAll()
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
        public UserDataPermission GetById(int id)
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
        public int? Insert(UserDataPermission bo)
        {
            var objId = this.repo.Insert(bo);
            // Trigger data change
            if (objId != null)
            {
                var changeData = new WaitingSyncData()
                {
                    ActionTypeID = 1,
                    ActionTypeName = "Insert",
                    ObjectID2 = objId,
                    ObjectName = "[Library].[PriorityLevel]",
                    EffectDate = DateTime.Now,IsSynced = false
                };

                this.waitingSyncDataService.Insert(changeData);
            }

            // -------------------------------------------------------

            return objId;
        }

        /// <summary>
        /// Delete Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(UserDataPermission bo)
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
                var flag = this.repo.Delete(id);
                if (flag)
                {
                    // Trigger data change
                    var changeData = new WaitingSyncData()
                    {
                        ActionTypeID = 3,
                        ActionTypeName = "Delete",
                        ObjectID2 = id,
                        ObjectName = "[Library].[PriorityLevel]",
                        EffectDate = DateTime.Now,IsSynced = false
                    };

                    this.waitingSyncDataService.Insert(changeData);
                    // ----------------------------------------------------------------------
                }

                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

    }
}
