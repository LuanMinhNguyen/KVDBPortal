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
    public class TrackingShutdownReportService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly TrackingShutdownReportDAO repo;
        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingShutdownReportService"/> class.
        /// </summary>
        public TrackingShutdownReportService()
        {
            this.repo = new TrackingShutdownReportDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        #region Get (Advances)
        public List<TrackingShutdownReport> GetAllRevTrackingShutdownReportOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.CauseShutdown.ToLower().Contains(searchAll.ToLower())
                                            || t.CauseClarificationProcess.ToLower().Contains(searchAll.ToLower())
                                            || t.CauseClarificationPowerloss.ToLower().Contains(searchAll.ToLower())
                                            || t.CauseClarificationFireGas.ToLower().Contains(searchAll.ToLower())
                                            || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                            || t.AreaConcern.ToLower().Contains(searchAll.ToLower())
                                            || t.WayForward.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.Status.ToLower().Contains(searchAll.ToLower())
                                            || t.Lesson.ToLower().Contains(searchAll.ToLower())
                                            )).ToList();
        }

        public List<TrackingShutdownReport> GetAllTrackingShutdownReportOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && t.IsLeaf.GetValueOrDefault()
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.CauseShutdown.ToLower().Contains(searchAll.ToLower())
                                            || t.CauseClarificationProcess.ToLower().Contains(searchAll.ToLower())
                                            || t.CauseClarificationPowerloss.ToLower().Contains(searchAll.ToLower())
                                            || t.CauseClarificationFireGas.ToLower().Contains(searchAll.ToLower())
                                            || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                            || t.AreaConcern.ToLower().Contains(searchAll.ToLower())
                                            || t.WayForward.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.Status.ToLower().Contains(searchAll.ToLower())
                                            || t.Lesson.ToLower().Contains(searchAll.ToLower())
                                            )).ToList();
        }

        public List<TrackingShutdownReport> GetAllOverDueTrackingShutdownReportOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsComplete.GetValueOrDefault()
                                                && t.Deadline != null
                                                && t.Deadline < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null)
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseShutdown.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationProcess.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationPowerloss.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationFireGas.ToLower().Contains(searchAll.ToLower())
                                                    || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                                    || t.AreaConcern.ToLower().Contains(searchAll.ToLower())
                                                    || t.WayForward.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.Status.ToLower().Contains(searchAll.ToLower())
                                                    || t.Lesson.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingShutdownReport> GetAllComingDueTrackingShutdownReportOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsComplete.GetValueOrDefault()
                                                && t.Deadline != null
                                                && t.Deadline.GetValueOrDefault().AddDays(2) > DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseShutdown.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationProcess.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationPowerloss.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationFireGas.ToLower().Contains(searchAll.ToLower())
                                                    || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                                    || t.AreaConcern.ToLower().Contains(searchAll.ToLower())
                                                    || t.WayForward.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.Status.ToLower().Contains(searchAll.ToLower())
                                                    || t.Lesson.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingShutdownReport> GetAllCompleteTrackingShutdownReportOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsComplete.GetValueOrDefault()
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseShutdown.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationProcess.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationPowerloss.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationFireGas.ToLower().Contains(searchAll.ToLower())
                                                    || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                                    || t.AreaConcern.ToLower().Contains(searchAll.ToLower())
                                                    || t.WayForward.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.Status.ToLower().Contains(searchAll.ToLower())
                                                    || t.Lesson.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingShutdownReport> GetAllInCompleteTrackingShutdownReportOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsLeaf.GetValueOrDefault() && !t.IsComplete.GetValueOrDefault()
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseShutdown.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationProcess.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationPowerloss.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationFireGas.ToLower().Contains(searchAll.ToLower())
                                                    || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                                    || t.AreaConcern.ToLower().Contains(searchAll.ToLower())
                                                    || t.WayForward.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.Status.ToLower().Contains(searchAll.ToLower())
                                                    || t.Lesson.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList(); ;
        }

        public List<TrackingShutdownReport> GetAll(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseShutdown.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationProcess.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationPowerloss.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationFireGas.ToLower().Contains(searchAll.ToLower())
                                                    || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                                    || t.AreaConcern.ToLower().Contains(searchAll.ToLower())
                                                    || t.WayForward.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.Status.ToLower().Contains(searchAll.ToLower())
                                                    || t.Lesson.ToLower().Contains(searchAll.ToLower())

                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        public List<TrackingShutdownReport> GetAllCompletedShutdownReport(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && t.IsWFComplete.GetValueOrDefault()
                                                && t.IsCompleteFinal.GetValueOrDefault()
                                                && !t.IsCancel.GetValueOrDefault()

                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseShutdown.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationProcess.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationPowerloss.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationFireGas.ToLower().Contains(searchAll.ToLower())
                                                    || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                                    || t.AreaConcern.ToLower().Contains(searchAll.ToLower())
                                                    || t.WayForward.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.Status.ToLower().Contains(searchAll.ToLower())
                                                    || t.Lesson.ToLower().Contains(searchAll.ToLower())

                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        public List<TrackingShutdownReport> GetAllCompletedShutdownReportWaitingPrint(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && t.IsWFComplete.GetValueOrDefault()
                                                && !t.IsCompleteFinal.GetValueOrDefault()
                                                && !t.IsCancel.GetValueOrDefault()

                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseShutdown.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationProcess.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationPowerloss.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationFireGas.ToLower().Contains(searchAll.ToLower())
                                                    || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                                    || t.AreaConcern.ToLower().Contains(searchAll.ToLower())
                                                    || t.WayForward.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.Status.ToLower().Contains(searchAll.ToLower())
                                                    || t.Lesson.ToLower().Contains(searchAll.ToLower())

                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        public List<TrackingShutdownReport> GetAllIncompleteShutdownReport(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsCompleteFinal.GetValueOrDefault()
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseShutdown.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationProcess.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationPowerloss.ToLower().Contains(searchAll.ToLower())
                                                    || t.CauseClarificationFireGas.ToLower().Contains(searchAll.ToLower())
                                                    || t.RootCause.ToLower().Contains(searchAll.ToLower())
                                                    || t.AreaConcern.ToLower().Contains(searchAll.ToLower())
                                                    || t.WayForward.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || t.Status.ToLower().Contains(searchAll.ToLower())
                                                    || t.Lesson.ToLower().Contains(searchAll.ToLower())

                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        public List<TrackingShutdownReport> GetAllShutdownReportRev(Guid parentId)
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
        public List<TrackingShutdownReport> GetAll()
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
        public TrackingShutdownReport GetById(Guid id)
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
        public Guid? Insert(TrackingShutdownReport bo)
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
                    ObjectName = "[WMS].[TrackingShutdownReport]",
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
        public bool Update(TrackingShutdownReport bo)
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
                        ObjectName = "[WMS].[TrackingShutdownReport]",
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
        public bool Delete(TrackingShutdownReport bo)
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
                        ObjectName = "[WMS].[TrackingShutdownReport]",
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
