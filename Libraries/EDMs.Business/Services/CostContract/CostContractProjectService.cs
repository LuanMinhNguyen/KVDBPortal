using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Data.DAO.CostContract;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.CostContract
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class CostContractProjectService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly CostContractProjectDAO repo;

        /// <summary>
        /// The permission package service.
        /// </summary>
        private readonly PermissionProcurementRequirementService permissionPRService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CostContractProjectService"/> class.
        /// </summary>
        public CostContractProjectService()
        {
            this.repo = new CostContractProjectDAO();
            this.permissionPRService = new PermissionProcurementRequirementService();
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
        public List<CostContractProject> GetAllInPermission(int userId)
        {
            var projectIdInPermission = this.permissionPRService.GetProjectInPermission(userId);
            return this.repo.GetAll().Where(t => projectIdInPermission.Contains(t.ID)).ToList();
        }


        public CostContractProject GetByName(string name)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.Name.Trim() == name.Trim());
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<CostContractProject> GetAll()
        {
            return this.repo.GetAll().OrderBy(t => t.Name).ToList();
        }


        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public CostContractProject GetById(int id)
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
        public int? Insert(CostContractProject bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(CostContractProject bo)
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
        public bool Delete(CostContractProject bo)
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
