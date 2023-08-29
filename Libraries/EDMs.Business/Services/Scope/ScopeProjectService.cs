using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Data.DAO.Scope;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.Scope
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class ScopeProjectService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly ScopeProjectDAO repo;

        /// <summary>
        /// The permission package service.
        /// </summary>
        private readonly PermissionWorkgroupService PermissionWorkgroupService;

        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeProjectService"/> class.
        /// </summary>
        public ScopeProjectService()
        {
            this.repo = new ScopeProjectDAO();
            this.PermissionWorkgroupService = new PermissionWorkgroupService();
            this.waitingSyncDataService = new WaitingSyncDataService();

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
        public List<ScopeProject> GetAllInPermission(int userId)
        {
            var projectIdInPermission = this.PermissionWorkgroupService.GetProjectInPermission(userId);
            return this.repo.GetAll().Where(t => projectIdInPermission.Contains(t.ID)).ToList();
        }


        public ScopeProject GetByName(string name)
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
        public List<ScopeProject> GetAll()
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
        public ScopeProject GetById(int id)
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
        public int? Insert(ScopeProject bo)
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
                    ObjectName = "[Scope].[ScopeProjects]",
                    EffectDate = DateTime.Now,IsSynced = false
                };

                this.waitingSyncDataService.Insert(changeData);
            }

            // -------------------------------------------------------

            return objId;
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(ScopeProject bo)
        {
            try
            {
                var flag = this.repo.Update(bo);
                if (flag)
                {
                    var changeData = new WaitingSyncData()
                    {
                        ActionTypeID = 2,
                        ActionTypeName = "Update",
                        ObjectID2 = bo.ID,
                        ObjectName = "[Scope].[ScopeProjects]",
                        EffectDate = DateTime.Now,IsSynced = false
                    };

                    this.waitingSyncDataService.Insert(changeData);
                }

                return flag;
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
        public bool Delete(ScopeProject bo)
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
                        ObjectName = "[Scope].[ScopeProjects]",
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
