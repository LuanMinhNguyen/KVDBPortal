using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Business.Services.Library;
using EDMs.Data.DAO.Scope;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.Scope
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class PackageService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly PackageDAO repo;

        private readonly PermissionWorkgroupService PermissionWorkgroupService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageService"/> class.
        /// </summary>
        public PackageService()
        {
            this.repo = new PackageDAO();
            this.PermissionWorkgroupService = new PermissionWorkgroupService();
        }

        #region Get (Advances)

        /// <summary>
        /// The get all package in permission.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="projectId">
        /// The project Id.
        /// </param>
        /// <returns>
        /// The list <see cref="Package"/>.
        /// </returns>
        public List<Package> GetAllPackageInPermission(int userId, int projectId)
        {
            var packageIdInPermission = this.PermissionWorkgroupService.GetPackageInPermission(userId);
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && packageIdInPermission.Contains(t.ID)).ToList();
        }

        /// <summary>
        /// The get all package of project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Package> GetAllPackageOfProject(int projectId)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId).ToList();
        }

        public Package GetByName(string name, int projectId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.Name.Trim() == name.Trim() && t.ProjectId == projectId);
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<Package> GetAll()
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
        public Package GetById(int id)
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
        public int? Insert(Package bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(Package bo)
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
        public bool Delete(Package bo)
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
