namespace EDMs.Business.Services.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO.Scope;
    using EDMs.Data.Entities;

    /// <summary>
    /// The category service.
    /// </summary>
    public class PermissionWorkgroupService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly PermissionWorkgroupDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionWorkgroupService"/> class.
        /// </summary>
        public PermissionWorkgroupService()
        {
            this.repo = new PermissionWorkgroupDAO();
        }

        #region Get (Advances)

        /// <summary>
        /// The get user id list in permission.
        /// </summary>
        /// <param name="packageId">
        /// The package id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<int> GetUserIdListInPermission(int packageId)
        {
            return this.repo.GetAll().Where(t => t.WorkgroupId == packageId).Select(t => t.UserId.GetValueOrDefault()).ToList();
        }

        /// <summary>
        /// The get all by user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<PermissionWorkgroup> GetAllByUser(int userId)
        {
            return this.repo.GetAll().Where(t => t.UserId == userId).ToList();
        }

        public List<PermissionWorkgroup> GetAllByPackage(int packageId)
        {
            return this.repo.GetAll().Where(t => t.WorkgroupId == packageId).ToList();
        }

        public bool IsFullPermission(int userId, int workgroupId)
        {
            var permissionObj =
                this.repo.GetAll().FirstOrDefault(t => t.UserId == userId && t.WorkgroupId == workgroupId);
            return permissionObj != null && permissionObj.IsFullPermission.GetValueOrDefault();
        }
        /// <summary>
        /// The get project in permission.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<int> GetProjectInPermission(int userId)
        {
            return this.repo.GetAll().Where(t => t.UserId == userId).Select(t => t.ProjectId.GetValueOrDefault()).Distinct().ToList();
        }

        /// <summary>
        /// The get package in permission.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<int> GetPackageInPermission(int userId)
        {
            return
                this.repo.GetAll().Where(t => t.UserId == userId).Select(t => t.WorkgroupId.GetValueOrDefault()).ToList();
        }

        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<PermissionWorkgroup> GetAll()
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
        public PermissionWorkgroup GetById(int id)
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
        public int? Insert(PermissionWorkgroup bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Delete Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(PermissionWorkgroup bo)
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
