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
    public class TrackingGeneralWorkingService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly TrackingGeneralWorkingDAO repo;
        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingGeneralWorkingService"/> class.
        /// </summary>
        public TrackingGeneralWorkingService()
        {
            this.repo = new TrackingGeneralWorkingDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        #region Get (Advances)
        public List<TrackingGeneralWorking> GetAllByProject(int projectId)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsLeaf.GetValueOrDefault()).ToList();
        }

        public List<TrackingGeneralWorking> GetAllToDoList(int userId)
        {
            return this.repo.GetAll().Where(t => t.IsLeaf.GetValueOrDefault() && 
                                                ((!string.IsNullOrEmpty(t.AssignUserIds) && t.AssignUserIds.Split(';').Contains(userId.ToString()))
                                                  || (!string.IsNullOrEmpty(t.BackupUserIds) && t.BackupUserIds.Split(';').Contains(userId.ToString()))
                                                  || (!string.IsNullOrEmpty(t.VerifyUserIds) && t.VerifyUserIds.Split(';').Contains(userId.ToString()))
                                                  )
                                                && !t.IsComplete.GetValueOrDefault()).ToList();
        }
        public List<TrackingGeneralWorking> GetAllRevTrackingGeneralWorkingOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.WorkCategoryName.ToLower().Contains(searchAll.ToLower())
                                            || t.WorkContent.ToLower().Contains(searchAll.ToLower())
                                            || t.AssignUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.BackupUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.VerifyUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.Status.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.DeadlineReasonChange.ToLower().Contains(searchAll.ToLower())
                                            
                                            )).ToList();
        }

        public List<TrackingGeneralWorking> GetAllTrackingGeneralWorkingOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && t.IsLeaf.GetValueOrDefault()
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.WorkCategoryName.ToLower().Contains(searchAll.ToLower())
                                            || t.WorkContent.ToLower().Contains(searchAll.ToLower())
                                            || t.AssignUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.BackupUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.VerifyUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.Status.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.DeadlineReasonChange.ToLower().Contains(searchAll.ToLower())
                                            )).ToList();
        }

        public List<TrackingGeneralWorking> GetAllOverDueTrackingGeneralWorkingOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId 
                                                && !t.IsComplete.GetValueOrDefault()
                                                && t.Deadline != null 
                                                && t.Deadline < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null)
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.WorkCategoryName.ToLower().Contains(searchAll.ToLower())
                                            || t.WorkContent.ToLower().Contains(searchAll.ToLower())
                                            || t.AssignUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.BackupUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.VerifyUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.Status.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.DeadlineReasonChange.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingGeneralWorking> GetAllComingDueTrackingGeneralWorkingOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId 
                                                && !t.IsComplete.GetValueOrDefault()
                                                && t.Deadline != null 
                                                && t.Deadline.GetValueOrDefault().AddDays(2) > DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) 
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.WorkCategoryName.ToLower().Contains(searchAll.ToLower())
                                            || t.WorkContent.ToLower().Contains(searchAll.ToLower())
                                            || t.AssignUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.BackupUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.VerifyUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.Status.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.DeadlineReasonChange.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingGeneralWorking> GetAllCompleteTrackingGeneralWorkingOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsComplete.GetValueOrDefault() 
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.WorkCategoryName.ToLower().Contains(searchAll.ToLower())
                                            || t.WorkContent.ToLower().Contains(searchAll.ToLower())
                                            || t.AssignUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.BackupUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.VerifyUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.Status.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.DeadlineReasonChange.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingGeneralWorking> GetAllInCompleteTrackingGeneralWorkingOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsLeaf.GetValueOrDefault() && !t.IsComplete.GetValueOrDefault() 
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.WorkCategoryName.ToLower().Contains(searchAll.ToLower())
                                            || t.WorkContent.ToLower().Contains(searchAll.ToLower())
                                            || t.AssignUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.BackupUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.VerifyUserName.ToLower().Contains(searchAll.ToLower())
                                            || t.Status.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.DeadlineReasonChange.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList(); ;
        }

        

        public List<TrackingGeneralWorking> GetAllGeneralWorkingRev(Guid parentId)
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
        public List<TrackingGeneralWorking> GetAll()
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
        public TrackingGeneralWorking GetById(Guid id)
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
        public Guid? Insert(TrackingGeneralWorking bo)
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
                    ObjectName = "[WMS].[TrackingGeneralWorking]",
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
        public bool Update(TrackingGeneralWorking bo)
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
                        ObjectName = "[WMS].[TrackingGeneralWorking]",
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
        public bool Delete(TrackingGeneralWorking bo)
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
                        ObjectName = "[WMS].[TrackingGeneralWorking]",
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
