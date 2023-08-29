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
    public class TrackingMOCService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly TrackingMOCDAO repo;
        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingMOCService"/> class.
        /// </summary>
        public TrackingMOCService()
        {
            this.repo = new TrackingMOCDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        #region Get (Advances)
        public List<TrackingMOC> GetAllRevTrackingMOCOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.DescriptionOfChange.ToLower().Contains(searchAll.ToLower())
                                            || t.ReasonOfChange.ToLower().Contains(searchAll.ToLower())
                                            || t.InitRisk.ToLower().Contains(searchAll.ToLower())
                                            || t.MigrationAction.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.ClosedClarification.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            )).ToList();
        }

        public List<TrackingMOC> GetAllTrackingMOCOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && t.IsLeaf.GetValueOrDefault()
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.DescriptionOfChange.ToLower().Contains(searchAll.ToLower())
                                            || t.ReasonOfChange.ToLower().Contains(searchAll.ToLower())
                                            || t.InitRisk.ToLower().Contains(searchAll.ToLower())
                                            || t.MigrationAction.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.ClosedClarification.ToLower().Contains(searchAll.ToLower())
                                            )).ToList();
        }

        public List<TrackingMOC> GetAllOverDueTrackingMOCOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsComplete.GetValueOrDefault()
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                                    || t.DescriptionOfChange.ToLower().Contains(searchAll.ToLower())
                                                    || t.ReasonOfChange.ToLower().Contains(searchAll.ToLower())
                                                    || t.InitRisk.ToLower().Contains(searchAll.ToLower())
                                                    || t.MigrationAction.ToLower().Contains(searchAll.ToLower())
                                                    || t.Remark.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.ClosedClarification.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingMOC> GetAllComingDueTrackingMOCOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsComplete.GetValueOrDefault()
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.DescriptionOfChange.ToLower().Contains(searchAll.ToLower())
                                            || t.ReasonOfChange.ToLower().Contains(searchAll.ToLower())
                                            || t.InitRisk.ToLower().Contains(searchAll.ToLower())
                                            || t.MigrationAction.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.ClosedClarification.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingMOC> GetAllCompleteTrackingMOCOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsComplete.GetValueOrDefault()
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.DescriptionOfChange.ToLower().Contains(searchAll.ToLower())
                                            || t.ReasonOfChange.ToLower().Contains(searchAll.ToLower())
                                            || t.InitRisk.ToLower().Contains(searchAll.ToLower())
                                            || t.MigrationAction.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.ClosedClarification.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingMOC> GetAllInCompleteTrackingMOCOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsLeaf.GetValueOrDefault() && !t.IsComplete.GetValueOrDefault()
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.DescriptionOfChange.ToLower().Contains(searchAll.ToLower())
                                            || t.ReasonOfChange.ToLower().Contains(searchAll.ToLower())
                                            || t.InitRisk.ToLower().Contains(searchAll.ToLower())
                                            || t.MigrationAction.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.ClosedClarification.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList(); ;
        }



        public List<TrackingMOC> GetAllMOCRev(Guid parentId)
        {
            return this.repo.GetAll().Where(t => (t.ID == parentId || t.ParentId == parentId)).OrderByDescending(t => t.CreatedDate).ToList();
        }

        public List<TrackingMOC> GetAll(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                                    || t.DescriptionOfChange.ToLower().Contains(searchAll.ToLower())
                                                    || t.ReasonOfChange.ToLower().Contains(searchAll.ToLower())
                                                    || t.InitRisk.ToLower().Contains(searchAll.ToLower())
                                                    || t.MigrationAction.ToLower().Contains(searchAll.ToLower())
                                                    || t.Remark.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.ClosedClarification.ToLower().Contains(searchAll.ToLower())
                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        public List<TrackingMOC> GetAllCompletedMOC(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && t.IsWFComplete.GetValueOrDefault()
                                                && t.IsCompleteFinal.GetValueOrDefault()
                                                && !t.IsCancel.GetValueOrDefault()

                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                                    || t.DescriptionOfChange.ToLower().Contains(searchAll.ToLower())
                                                    || t.ReasonOfChange.ToLower().Contains(searchAll.ToLower())
                                                    || t.InitRisk.ToLower().Contains(searchAll.ToLower())
                                                    || t.MigrationAction.ToLower().Contains(searchAll.ToLower())
                                                    || t.Remark.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.ClosedClarification.ToLower().Contains(searchAll.ToLower())
                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        public List<TrackingMOC> GetAllCompletedMOCWaitingPrint(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && t.IsWFComplete.GetValueOrDefault()
                                                && !t.IsCompleteFinal.GetValueOrDefault()
                                                && !t.IsCancel.GetValueOrDefault()

                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                                    || t.DescriptionOfChange.ToLower().Contains(searchAll.ToLower())
                                                    || t.ReasonOfChange.ToLower().Contains(searchAll.ToLower())
                                                    || t.InitRisk.ToLower().Contains(searchAll.ToLower())
                                                    || t.MigrationAction.ToLower().Contains(searchAll.ToLower())
                                                    || t.Remark.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.ClosedClarification.ToLower().Contains(searchAll.ToLower())
                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        public List<TrackingMOC> GetAllIncompleteMOC(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsCompleteFinal.GetValueOrDefault()
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                                    || t.DescriptionOfChange.ToLower().Contains(searchAll.ToLower())
                                                    || t.ReasonOfChange.ToLower().Contains(searchAll.ToLower())
                                                    || t.InitRisk.ToLower().Contains(searchAll.ToLower())
                                                    || t.MigrationAction.ToLower().Contains(searchAll.ToLower())
                                                    || t.Remark.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.ClosedClarification.ToLower().Contains(searchAll.ToLower())
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
        public List<TrackingMOC> GetAll()
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
        public TrackingMOC GetById(Guid id)
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
        public Guid? Insert(TrackingMOC bo)
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
                    ObjectName = "[WMS].[TrackingMOC]",
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
        public bool Update(TrackingMOC bo)
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
                        ObjectName = "[WMS].[TrackingMOC]",
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
        public bool Delete(TrackingMOC bo)
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
                        ObjectName = "[WMS].[TrackingMOC]",
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
