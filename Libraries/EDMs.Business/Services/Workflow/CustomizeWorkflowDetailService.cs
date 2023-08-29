﻿using System;
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
    public class CustomizeWorkflowDetailService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly CustomizeWorkflowDetailDAO repo;
        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="CustomizeWorkflowDetailService"/> class.
        /// </summary>
        public CustomizeWorkflowDetailService()
        {
            this.repo = new CustomizeWorkflowDetailDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        #region Get (Advances)
        public CustomizeWorkflowDetail GetByCurrentStep(int wfStepId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.CurrentWorkflowStepID == wfStepId);
        }

        public CustomizeWorkflowDetail GetByCurrentStepCustomizeFromTrans(int wfStepId, Guid transId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.CurrentWorkflowStepID == wfStepId && t.IncomingTransId == transId);
        }

        public CustomizeWorkflowDetail GetByCurrentStepCustomizeFromObj(int wfStepId, Guid objId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.CurrentWorkflowStepID == wfStepId && t.ObjectId == objId);
        }
        public List<CustomizeWorkflowDetail> GetAllByStepWorkflowAndObj(int workflowId, Guid objId)
        {
            return this.repo.GetAll().Where(t => t.WorkflowID == workflowId && t.ObjectId == objId).OrderBy(t => t.CurrentWorkflowStepName).ToList();
        }
        public List<CustomizeWorkflowDetail> GetAllByWorkflow(int wfId)
        {
            return this.repo.GetAll().Where(t => t.WorkflowID == wfId).OrderBy(t => t.CurrentWorkflowStepName).ToList();
        }

        public List<CustomizeWorkflowDetail> GetAllByObjId(Guid objId)
        {
            return this.repo.GetAll().Where(t => t.ObjectId == objId).OrderBy(t => t.CurrentWorkflowStepName).ToList();
        }

        public List<CustomizeWorkflowDetail> GetAllByTransId(Guid transId)
        {
            return this.repo.GetAll().Where(t => t.IncomingTransId == transId).OrderBy(t => t.CurrentWorkflowStepName).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<CustomizeWorkflowDetail> GetAll()
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
        public CustomizeWorkflowDetail GetById(int id)
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
        public int? Insert(CustomizeWorkflowDetail bo)
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
                    ObjectName = "[WMS].[CustomizeWorkflowDetails]",
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
        public bool Update(CustomizeWorkflowDetail bo)
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
                        ObjectName = "[WMS].[CustomizeWorkflowDetails]",
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
        public bool Delete(CustomizeWorkflowDetail bo)
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
                        ObjectName = "[WMS].[CustomizeWorkflowDetails]",
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

        public object GetAllByWorkflowAndObj(int workflowId, Guid objId)
        {
            return this.repo.GetAll().Where(t => t.WorkflowID == workflowId && t.ObjectId == objId).OrderBy(t => t.CurrentWorkflowStepName).ToList();
        }

        public object GetAllByWorkflowAndTrans(int workflowId, Guid objId)
        {
            return this.repo.GetAll().Where(t => t.WorkflowID == workflowId && t.IncomingTransId == objId).OrderBy(t => t.CurrentWorkflowStepName).ToList();
        }
    }
}
