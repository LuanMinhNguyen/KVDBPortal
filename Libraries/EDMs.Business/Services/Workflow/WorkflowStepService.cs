using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Business.Services.Security;
using EDMs.Data.DAO.Workflow;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.Workflow
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class WorkflowStepService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly WorkflowStepDAO repo;
        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowStepService"/> class.
        /// </summary>
        public WorkflowStepService()
        {
            this.repo = new WorkflowStepDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        public WorkflowStep GetFirstStep(int wfId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.WorkflowID == wfId && t.IsFirst.GetValueOrDefault());
        }

        #region Get (Advances)

        public Data.Entities.WorkflowStep GetByName(string name)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.Name.Trim() == name.Trim());
        }

        public List<Data.Entities.WorkflowStep> GetAllByWorkflow(int wfId)
        {
            return this.repo.GetAll().Where(t => t.WorkflowID == wfId).OrderBy(t => t.Name).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<Data.Entities.WorkflowStep> GetAll()
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
        public Data.Entities.WorkflowStep GetById(int id)
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
        public int? Insert(Data.Entities.WorkflowStep bo)
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
                    ObjectName = "[WMS].[WorkflowStep]",
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
        public bool Update(Data.Entities.WorkflowStep bo)
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
                        ObjectName = "[WMS].[WorkflowStep]",
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
        public bool Delete(Data.Entities.WorkflowStep bo)
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
                        ObjectName = "[WMS].[WorkflowStep]",
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
