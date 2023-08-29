using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Business.Services.Security;
using EDMs.Data.DAO.WMS;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.WMS
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class TrackingECRService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly TrackingECRDAO repo;

        private readonly WaitingSyncDataService waitingSyncDataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingECRService"/> class.
        /// </summary>
        public TrackingECRService()
        {
            this.repo = new TrackingECRDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();
        }

        #region Get (Advances)
        public List<TrackingECR> GetAllRevTrackingECROfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.Title.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.PersonInCharge.ToLower().Contains(searchAll.ToLower())
                                            )).ToList();
        }

        public List<TrackingECR> GetAllTrackingECROfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.Title.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.PersonInCharge.ToLower().Contains(searchAll.ToLower())
                                            || t.StatusName.ToLower().Contains(searchAll.ToLower())
                                            || t.ExecutionStatus.ToLower().Contains(searchAll.ToLower())

                                            )).ToList();
        }

        public List<TrackingECR> GetAllOverDueTrackingECROfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsComplete.GetValueOrDefault()
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.Title.ToLower().Contains(searchAll.ToLower())
                                                    || t.Description.ToLower().Contains(searchAll.ToLower())
                                                    || t.Remark.ToLower().Contains(searchAll.ToLower())
                                                    || t.PersonInCharge.ToLower().Contains(searchAll.ToLower())
                                                    || t.StatusName.ToLower().Contains(searchAll.ToLower())
                                                    || t.ExecutionStatus.ToLower().Contains(searchAll.ToLower())

                                                    )).ToList();
        }

        public List<TrackingECR> GetAllComingDueTrackingECROfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsComplete.GetValueOrDefault()
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.Title.ToLower().Contains(searchAll.ToLower())
                                                    || t.Description.ToLower().Contains(searchAll.ToLower())
                                                    || t.Remark.ToLower().Contains(searchAll.ToLower())
                                                    || t.PersonInCharge.ToLower().Contains(searchAll.ToLower())
                                                    || t.StatusName.ToLower().Contains(searchAll.ToLower())
                                                    || t.ExecutionStatus.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingECR> GetAllCompleteTrackingECROfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsComplete.GetValueOrDefault()
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.Title.ToLower().Contains(searchAll.ToLower())
                                                    || t.Description.ToLower().Contains(searchAll.ToLower())
                                                    || t.Remark.ToLower().Contains(searchAll.ToLower())
                                                    || t.PersonInCharge.ToLower().Contains(searchAll.ToLower())
                                                    || t.StatusName.ToLower().Contains(searchAll.ToLower())
                                                    || t.ExecutionStatus.ToLower().Contains(searchAll.ToLower())

                                                    )).ToList();
        }

        public List<TrackingECR> GetAllInCompleteTrackingECROfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsLeaf.GetValueOrDefault() && !t.IsComplete.GetValueOrDefault()
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.Title.ToLower().Contains(searchAll.ToLower())
                                                    || t.Description.ToLower().Contains(searchAll.ToLower())
                                                    || t.Remark.ToLower().Contains(searchAll.ToLower())
                                                    || t.PersonInCharge.ToLower().Contains(searchAll.ToLower())
                                                    || t.StatusName.ToLower().Contains(searchAll.ToLower())
                                                    || t.ExecutionStatus.ToLower().Contains(searchAll.ToLower())

                                                    )).ToList(); ;
        }

        public List<TrackingECR> GetAllECRRev(Guid parentId)
        {
            return this.repo.GetAll().Where(t => (t.ID == parentId || t.ParentId == parentId)).OrderByDescending(t => t.CreatedDate).ToList();
        }

        public List<TrackingECR> GetAll(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.Title.ToLower().Contains(searchAll.ToLower())
                                                    || t.Description.ToLower().Contains(searchAll.ToLower())
                                                    || t.Remark.ToLower().Contains(searchAll.ToLower())
                                                    || t.PersonInCharge.ToLower().Contains(searchAll.ToLower())
                                                    || t.StatusName.ToLower().Contains(searchAll.ToLower())
                                                    || t.ExecutionStatus.ToLower().Contains(searchAll.ToLower())

                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        public List<TrackingECR> GetAllCompletedECR(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && t.IsWFComplete.GetValueOrDefault()
                                                && t.IsCompleteFinal.GetValueOrDefault()
                                                && !t.IsCancel.GetValueOrDefault()

                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.Title.ToLower().Contains(searchAll.ToLower())
                                                    || t.Description.ToLower().Contains(searchAll.ToLower())
                                                    || t.Remark.ToLower().Contains(searchAll.ToLower())
                                                    || t.PersonInCharge.ToLower().Contains(searchAll.ToLower())
                                                    || t.StatusName.ToLower().Contains(searchAll.ToLower())
                                                    || t.ExecutionStatus.ToLower().Contains(searchAll.ToLower())

                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        public List<TrackingECR> GetAllCompletedECRWaitingPrint(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && t.IsWFComplete.GetValueOrDefault()
                                                && !t.IsCompleteFinal.GetValueOrDefault()
                                                && !t.IsCancel.GetValueOrDefault()

                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.Title.ToLower().Contains(searchAll.ToLower())
                                                    || t.Description.ToLower().Contains(searchAll.ToLower())
                                                    || t.Remark.ToLower().Contains(searchAll.ToLower())
                                                    || t.PersonInCharge.ToLower().Contains(searchAll.ToLower())
                                                    || t.StatusName.ToLower().Contains(searchAll.ToLower())
                                                    || t.ExecutionStatus.ToLower().Contains(searchAll.ToLower())

                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        public List<TrackingECR> GetAllIncompleteECR(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsCompleteFinal.GetValueOrDefault()
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.Title.ToLower().Contains(searchAll.ToLower())
                                                    || t.Description.ToLower().Contains(searchAll.ToLower())
                                                    || t.Remark.ToLower().Contains(searchAll.ToLower())
                                                    || t.PersonInCharge.ToLower().Contains(searchAll.ToLower())
                                                    || t.StatusName.ToLower().Contains(searchAll.ToLower())
                                                    || t.ExecutionStatus.ToLower().Contains(searchAll.ToLower())

                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<TrackingECR> GetAll()
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
        public TrackingECR GetById(Guid id)
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
        public Guid? Insert(TrackingECR bo)
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
                    ObjectName = "[WMS].[TrackingECR]",
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
        public bool Update(TrackingECR bo)
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
                        ObjectName = "[WMS].[TrackingECR]",
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
        public bool Delete(TrackingECR bo)
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
                        ObjectName = "[WMS].[TrackingECR]",
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
