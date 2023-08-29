using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Business.Services.Security;
using EDMs.Data.DAO.Workflow;
using EDMs.Data.Entities;


namespace EDMs.Business.Services.Workflow
{
  public  class ObjectWorkflowDetailService
    {/// <summary>
     /// The repo.
     /// </summary>
        private readonly ObjectWorkflowDetailDAO repo;
        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowDetailService"/> class.
        /// </summary>
        public ObjectWorkflowDetailService()
        {
            this.repo = new ObjectWorkflowDetailDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        #region Get (Advances)
        public ObjectWorkflowDetail GetByCurrentStep(int wfStepId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.CurrentWorkflowStepID == wfStepId);
        }

        public List<ObjectWorkflowDetail> GetAllByWorkflow(int wfId)
        {
            return this.repo.GetAll().Where(t => t.WorkflowID == wfId).OrderBy(t => t.CurrentWorkflowStepName).ToList();
        }

        public List<ObjectWorkflowDetail> GetAllByObjectWorkflow(int wfId, Guid ObjectId)
        {
            return this.repo.GetAll().Where(t => t.WorkflowID == wfId && t.ObjectID==ObjectId).OrderBy(t => t.CurrentWorkflowStepName).ToList();
        }
        public List<ObjectWorkflowDetail> GetAllByObject( Guid ObjectId)
        {
            return this.repo.GetAll().Where(t => t.ObjectID == ObjectId).OrderBy(t => t.CurrentWorkflowStepName).ToList();
        }
        public ObjectWorkflowDetail GetByWorkflowDetailsID(int wfdetailId, Guid ObjectId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.WorkflowDetailsID == wfdetailId && t.ObjectID == ObjectId);
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<ObjectWorkflowDetail> GetAll()
        {
            return this.repo.GetAll().OrderBy(t => t.CurrentWorkflowStepName).ToList();
        }


        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public ObjectWorkflowDetail GetById(int id)
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
        public int? Insert(ObjectWorkflowDetail bo)
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
                    ObjectName = "[WMS].[ObjectWorkflowDetails]",
                    EffectDate = DateTime.Now,
                    IsSynced = false
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
        public bool Update(ObjectWorkflowDetail bo)
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
                        ObjectName = "[WMS].[ObjectWorkflowDetails]",
                        EffectDate = DateTime.Now,
                        IsSynced = false
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
        public bool Delete(ObjectWorkflowDetail bo)
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
                        ObjectName = "[WMS].[ObjectWorkflowDetails]",
                        EffectDate = DateTime.Now,
                        IsSynced = false
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
