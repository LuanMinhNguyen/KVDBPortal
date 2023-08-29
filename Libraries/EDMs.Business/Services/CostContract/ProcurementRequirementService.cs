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
    public class ProcurementRequirementService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly ProcurementRequirementDAO repo;

        /// <summary>
        /// The permission package service.
        /// </summary>
        private readonly PermissionProcurementRequirementService permissionPRService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcurementRequirementService"/> class.
        /// </summary>
        public ProcurementRequirementService()
        {
            this.repo = new ProcurementRequirementDAO();
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
        public List<ProcurementRequirement> GetAllByUser(int userId)
        {
            var projectIdInPermission = this.permissionPRService.GetProjectInPermission(userId);
            return this.repo.GetAll().Where(t => projectIdInPermission.Contains(t.ID)).ToList();
        }

        public List<ProcurementRequirement> GetAllByProject(int projectID)
        {
            return this.repo.GetAll().Where(t => t.ProjectID == projectID).OrderBy(t => t.Number).ToList();
        }

        public List<ProcurementRequirement> GetAllByProjectAndType(int projectID, int typeID)
        {
            return this.repo.GetAll().Where(t => t.ProjectID == projectID
                && t.TypeID == typeID).OrderBy(t => t.Number).ToList();
        }

        public List<ProcurementRequirement> GetAllPRInPermission(int userId, int projectId)
        {
            var prIdInPermission = this.permissionPRService.GetPRInPermission(userId);
            return this.repo.GetAll().Where(t => t.ProjectID == projectId && prIdInPermission.Contains(t.ID)).ToList();
        }

        public List<ProcurementRequirement> GetAllPRHaveFullPermission(int userId, int projectId)
        {
            var prIdInPermission = this.permissionPRService.GetAllByUserHaveFullPermission(userId);
            return this.repo.GetAll().Where(t => t.ProjectID == projectId && prIdInPermission.Contains(t.ID)).ToList();
        }

        public ProcurementRequirement GetByNumber(string number)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.Number == number);
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<ProcurementRequirement> GetAll()
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
        public ProcurementRequirement GetById(int id)
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
        public int? Insert(ProcurementRequirement bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(ProcurementRequirement bo)
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
        public bool Delete(ProcurementRequirement bo)
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
