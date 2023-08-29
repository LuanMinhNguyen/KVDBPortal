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
    public class TrackingBreakdownReportService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly TrackingBreakdownReportDAO repo;
        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingBreakdownReportService"/> class.
        /// </summary>
        public TrackingBreakdownReportService()
        {
            this.repo = new TrackingBreakdownReportDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        #region Get (Advances)
        public List<TrackingBreakdownReport> GetAllRevTrackingBreakdownReportOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.BreakdownSystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.TagNo.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.CauseGroup.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                            || t.ProposedAction.ToLower().Contains(searchAll.ToLower())
                                            || t.Lesson.ToLower().Contains(searchAll.ToLower())
                                            || t.UnplannedWoNo.ToLower().Contains(searchAll.ToLower())
                                            || t.CurrentStatus.ToLower().Contains(searchAll.ToLower())
                                            || t.MRWRItem.ToLower().Contains(searchAll.ToLower())
                                            || t.Comment.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.PICStatus.ToLower().Contains(searchAll.ToLower())
                                            )).ToList();
        }

        public List<TrackingBreakdownReport> GetAllTrackingBreakdownReportOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && t.IsLeaf.GetValueOrDefault()
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.BreakdownSystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.TagNo.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.CauseGroup.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                            || t.ProposedAction.ToLower().Contains(searchAll.ToLower())
                                            || t.Lesson.ToLower().Contains(searchAll.ToLower())
                                            || t.UnplannedWoNo.ToLower().Contains(searchAll.ToLower())
                                            || t.CurrentStatus.ToLower().Contains(searchAll.ToLower())
                                            || t.MRWRItem.ToLower().Contains(searchAll.ToLower())
                                            || t.Comment.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.PICStatus.ToLower().Contains(searchAll.ToLower())
                                            )).ToList();
        }

        public List<TrackingBreakdownReport> GetAllOverDueTrackingBreakdownReportOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsComplete.GetValueOrDefault()
                                                && t.PICDeadline != null
                                                //&& t.PICDeadline < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null)
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.BreakdownSystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.TagNo.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.CauseGroup.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                            || t.ProposedAction.ToLower().Contains(searchAll.ToLower())
                                            || t.Lesson.ToLower().Contains(searchAll.ToLower())
                                            || t.UnplannedWoNo.ToLower().Contains(searchAll.ToLower())
                                            || t.CurrentStatus.ToLower().Contains(searchAll.ToLower())
                                            || t.MRWRItem.ToLower().Contains(searchAll.ToLower())
                                            || t.Comment.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.PICStatus.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingBreakdownReport> GetAllComingDueTrackingBreakdownReportOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsComplete.GetValueOrDefault()
                                                && t.PICDeadline != null
                                                //&& t.PICDeadline.GetValueOrDefault().AddDays(2) > DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) 
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.BreakdownSystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.TagNo.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.CauseGroup.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                            || t.ProposedAction.ToLower().Contains(searchAll.ToLower())
                                            || t.Lesson.ToLower().Contains(searchAll.ToLower())
                                            || t.UnplannedWoNo.ToLower().Contains(searchAll.ToLower())
                                            || t.CurrentStatus.ToLower().Contains(searchAll.ToLower())
                                            || t.MRWRItem.ToLower().Contains(searchAll.ToLower())
                                            || t.Comment.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.PICStatus.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingBreakdownReport> GetAllCompleteTrackingBreakdownReportOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsComplete.GetValueOrDefault()
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.BreakdownSystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.TagNo.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.CauseGroup.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                            || t.ProposedAction.ToLower().Contains(searchAll.ToLower())
                                            || t.Lesson.ToLower().Contains(searchAll.ToLower())
                                            || t.UnplannedWoNo.ToLower().Contains(searchAll.ToLower())
                                            || t.CurrentStatus.ToLower().Contains(searchAll.ToLower())
                                            || t.MRWRItem.ToLower().Contains(searchAll.ToLower())
                                            || t.Comment.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.PICStatus.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingBreakdownReport> GetAllInCompleteTrackingBreakdownReportOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsLeaf.GetValueOrDefault() && !t.IsComplete.GetValueOrDefault()
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.BreakdownSystemName.ToLower().Contains(searchAll.ToLower())
                                                    || t.TagNo.ToLower().Contains(searchAll.ToLower())
                                                    || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseGroup.ToLower().Contains(searchAll.ToLower())
                                                    || t.Description.ToLower().Contains(searchAll.ToLower())
                                                    || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                                    || t.ProposedAction.ToLower().Contains(searchAll.ToLower())
                                                    || t.Lesson.ToLower().Contains(searchAll.ToLower())
                                                    || t.UnplannedWoNo.ToLower().Contains(searchAll.ToLower())
                                                    || t.CurrentStatus.ToLower().Contains(searchAll.ToLower())
                                                    || t.MRWRItem.ToLower().Contains(searchAll.ToLower())
                                                    || t.Comment.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICStatus.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList(); ;
        }


        public List<TrackingBreakdownReport> GetAll(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.BreakdownSystemName.ToLower().Contains(searchAll.ToLower())
                                                    || t.TagNo.ToLower().Contains(searchAll.ToLower())
                                                    || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseGroup.ToLower().Contains(searchAll.ToLower())
                                                    || t.Description.ToLower().Contains(searchAll.ToLower())
                                                    || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                                    || t.ProposedAction.ToLower().Contains(searchAll.ToLower())
                                                    || t.Lesson.ToLower().Contains(searchAll.ToLower())
                                                    || t.UnplannedWoNo.ToLower().Contains(searchAll.ToLower())
                                                    || t.CurrentStatus.ToLower().Contains(searchAll.ToLower())
                                                    || t.MRWRItem.ToLower().Contains(searchAll.ToLower())
                                                    || t.Comment.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICStatus.ToLower().Contains(searchAll.ToLower())

                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        public List<TrackingBreakdownReport> GetAllCompletedBreakdownReport(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && t.IsWFComplete.GetValueOrDefault()
                                                && t.IsCompleteFinal.GetValueOrDefault()
                                                && !t.IsCancel.GetValueOrDefault()

                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.BreakdownSystemName.ToLower().Contains(searchAll.ToLower())
                                                    || t.TagNo.ToLower().Contains(searchAll.ToLower())
                                                    || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseGroup.ToLower().Contains(searchAll.ToLower())
                                                    || t.Description.ToLower().Contains(searchAll.ToLower())
                                                    || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                                    || t.ProposedAction.ToLower().Contains(searchAll.ToLower())
                                                    || t.Lesson.ToLower().Contains(searchAll.ToLower())
                                                    || t.UnplannedWoNo.ToLower().Contains(searchAll.ToLower())
                                                    || t.CurrentStatus.ToLower().Contains(searchAll.ToLower())
                                                    || t.MRWRItem.ToLower().Contains(searchAll.ToLower())
                                                    || t.Comment.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICStatus.ToLower().Contains(searchAll.ToLower())

                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        public List<TrackingBreakdownReport> GetAllCompletedBreakdownReportWaitingPrint(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && t.IsWFComplete.GetValueOrDefault()
                                                && !t.IsCompleteFinal.GetValueOrDefault()
                                                && !t.IsCancel.GetValueOrDefault()
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.BreakdownSystemName.ToLower().Contains(searchAll.ToLower())
                                                    || t.TagNo.ToLower().Contains(searchAll.ToLower())
                                                    || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseGroup.ToLower().Contains(searchAll.ToLower())
                                                    || t.Description.ToLower().Contains(searchAll.ToLower())
                                                    || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                                    || t.ProposedAction.ToLower().Contains(searchAll.ToLower())
                                                    || t.Lesson.ToLower().Contains(searchAll.ToLower())
                                                    || t.UnplannedWoNo.ToLower().Contains(searchAll.ToLower())
                                                    || t.CurrentStatus.ToLower().Contains(searchAll.ToLower())
                                                    || t.MRWRItem.ToLower().Contains(searchAll.ToLower())
                                                    || t.Comment.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICStatus.ToLower().Contains(searchAll.ToLower())

                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        public List<TrackingBreakdownReport> GetAllIncompleteBreakdownReport(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsCompleteFinal.GetValueOrDefault()
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.BreakdownSystemName.ToLower().Contains(searchAll.ToLower())
                                                    || t.TagNo.ToLower().Contains(searchAll.ToLower())
                                                    || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseGroup.ToLower().Contains(searchAll.ToLower())
                                                    || t.Description.ToLower().Contains(searchAll.ToLower())
                                                    || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                                    || t.ProposedAction.ToLower().Contains(searchAll.ToLower())
                                                    || t.Lesson.ToLower().Contains(searchAll.ToLower())
                                                    || t.UnplannedWoNo.ToLower().Contains(searchAll.ToLower())
                                                    || t.CurrentStatus.ToLower().Contains(searchAll.ToLower())
                                                    || t.MRWRItem.ToLower().Contains(searchAll.ToLower())
                                                    || t.Comment.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICStatus.ToLower().Contains(searchAll.ToLower())

                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }
        public List<TrackingBreakdownReport> GetAllBreakdownReportRev(Guid parentId)
        {
            return this.repo.GetAll().Where(t => (t.ID == parentId || t.ParentId == parentId)).OrderByDescending(t => t.CreatedDate).ToList();
        }

        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<TrackingBreakdownReport> GetAll()
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
        public TrackingBreakdownReport GetById(Guid id)
        {
            return this.repo.GetById(id);
        }

        public TrackingBreakdownReport GetByCode(string code)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.Code == code && t.IsLeaf.GetValueOrDefault());
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public Guid? Insert(TrackingBreakdownReport bo)
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
                    ObjectName = "[WMS].[TrackingBreakdownReport]",
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
        public bool Update(TrackingBreakdownReport bo)
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
                        ObjectName = "[WMS].[TrackingBreakdownReport]",
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
        public bool Delete(TrackingBreakdownReport bo)
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
                        ObjectName = "[WMS].[TrackingBreakdownReport]",
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
