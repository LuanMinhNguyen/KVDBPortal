﻿using System;
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
    public class WorkGroupService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly WorkGroupDAO repo;

        private readonly PermissionWorkgroupService permissionWorkGroupService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkGroupService"/> class.
        /// </summary>
        public WorkGroupService()
        {
            this.repo = new WorkGroupDAO();
            this.permissionWorkGroupService = new PermissionWorkgroupService();
        }

        #region Get (Advances)

        /// <summary>
        /// The get all WorkGroup in permission.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="projectId">
        /// The project Id.
        /// </param>
        /// <returns>
        /// The list <see cref="WorkGroup"/>.
        /// </returns>
        public List<WorkGroup> GetAllWorkGroupInPermission(int userId, int projectId)
        {
            var WorkGroupIdInPermission = this.permissionWorkGroupService.GetPackageInPermission(userId);
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && WorkGroupIdInPermission.Contains(t.ID)).ToList();
        }

        /// <summary>
        /// The get all WorkGroup of project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<WorkGroup> GetAllWorkGroupOfProject(int projectId)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId).ToList();
        }

        public WorkGroup GetByName(string name)
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
        public List<WorkGroup> GetAll()
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
        public WorkGroup GetById(int id)
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
        public int? Insert(WorkGroup bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(WorkGroup bo)
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
        public bool Delete(WorkGroup bo)
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
