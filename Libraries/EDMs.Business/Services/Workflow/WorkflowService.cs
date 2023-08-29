using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Data.DAO.Workflow;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.Workflow
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class WorkflowService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly WorkflowDAO repo;
        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowService"/> class.
        /// </summary>
        public WorkflowService()
        {
            this.repo = new WorkflowDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        #region Get (Advances)

        public List<Data.Entities.Workflow> GetAllByProject(int projectId)
        {
            return this.repo.GetAll().Where(t => t.ProjectID == projectId).ToList();
        }
        public List<Data.Entities.Workflow> GetAllByListWF(List<int> ListId)
        {
            return this.repo.GetAll().Where(t => ListId.Contains(t.ID)).ToList();
        }
        public List<Data.Entities.Workflow> GetAllByProject(int projectId, Boolean isInternal)
        {
            return this.repo.GetAll().Where(t => t.ProjectID == projectId && t.IsInternalWorkflow.GetValueOrDefault() == isInternal).ToList();
        }

        public List<Data.Entities.Workflow> GetAllByProject(int projectId, int objType)
        {
            return this.repo.GetAll().Where(t => t.ProjectID == projectId && t.ObjectTypeId == objType).ToList();
        }

        public Data.Entities.Workflow GetByName(string name)
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
        public List<Data.Entities.Workflow> GetAll()
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
        public Data.Entities.Workflow GetById(int id)
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
        public int? Insert(Data.Entities.Workflow bo)
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
                    ObjectName = "[WMS].[Workflow]",
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
        public bool Update(Data.Entities.Workflow bo)
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
                        ObjectName = "[WMS].[Workflow]",
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
        public bool Delete(Data.Entities.Workflow bo)
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
                        ObjectName = "[WMS].[Workflow]",
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
