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
    public class ObjectAssignedWorkflowService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly ObjectAssignedWorkflowDAO repo;
        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectAssignedWorkflowService"/> class.
        /// </summary>
        public ObjectAssignedWorkflowService()
        {
            this.repo = new ObjectAssignedWorkflowDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        #region Get (Advances)
        public Data.Entities.ObjectAssignedWorkflow GetLeafBydoc(Guid objId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.ObjectID == objId && t.IsLeaf.GetValueOrDefault());
        }

        public List<ObjectAssignedWorkflow> GetAllByObj(Guid objId)
        {
            return this.repo.GetAll().Where(t => t.ObjectID == objId).ToList();
        }
        public List< Data.Entities.ObjectAssignedWorkflow> GetAllObjWf(int wfId,Guid objId)
        {
            return this.repo.GetAll().Where(t => t.ObjectID == objId && t.WorkflowID==wfId).ToList();
        }

        public Data.Entities.ObjectAssignedWorkflow GetNextStep(Guid objId, Guid PreviousId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.ObjectID == objId && t.PreviousStepId==PreviousId);
        }

        public Data.Entities.ObjectAssignedWorkflow ObjWfDetail(int objwfdetailId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.ObjectWFDwtailId==objwfdetailId);
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<Data.Entities.ObjectAssignedWorkflow> GetAll()
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
        public Data.Entities.ObjectAssignedWorkflow GetById(Guid id)
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
        public Guid? Insert(Data.Entities.ObjectAssignedWorkflow bo)
        {
            var objId = this.repo.Insert(bo);
            // Trigger data change
            if (objId != null)
            {
                var changeData = new WaitingSyncData()
                {
                    ActionTypeID = 1,
                    ActionTypeName = "Insert",
                    ObjectID = objId,
                    ObjectName = "[WMS].[ObjectAssignedWorkflow]",
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
        public bool Update(Data.Entities.ObjectAssignedWorkflow bo)
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
                        ObjectID = bo.ID,
                        ObjectName = "[WMS].[ObjectAssignedWorkflow]",
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
        public bool Delete(Data.Entities.ObjectAssignedWorkflow bo)
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
        public bool Delete(Guid id)
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
                        ObjectID = id,
                        ObjectName = "[WMS].[ObjectAssignedWorkflow]",
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
