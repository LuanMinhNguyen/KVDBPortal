using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Business.Services.Library;
using EDMs.Data.DAO.CostContract;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.CostContract
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class MaterialService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly MaterialDAO repo;

        /// <summary>
        /// The permission package service.
        /// </summary>
        private readonly PermissionWorkgroupService PermissionWorkgroupService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EDMs.Business.Services.CostContract.MaterialService"/> class.
        /// </summary>
        public MaterialService()
        {
            this.repo = new MaterialDAO();
            this.PermissionWorkgroupService = new PermissionWorkgroupService();
        }

        #region Get (Advances)

        /// <summary>
        /// The get all in permission.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Material> GetAllInPermission(int userId)
        {
            var projectIdInPermission = this.PermissionWorkgroupService.GetProjectInPermission(userId);
            return this.repo.GetAll().Where(t => projectIdInPermission.Contains(t.ID)).ToList();
        }

        public List<Material> GetAllByContract(int contractId)
        {
            return this.repo.GetAll().Where(t => t.ContractID == contractId).OrderBy(t => t.Number).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<Material> GetAll()
        {
            return this.repo.GetAll().OrderBy(t => t.Number).ToList();
        }


        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public Material GetById(int id)
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
        public int? Insert(Material bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(Material bo)
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
        public bool Delete(Material bo)
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
