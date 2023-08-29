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
    public class TrackingSailService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly TrackingSailDAO repo;
        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingSailService"/> class.
        /// </summary>
        public TrackingSailService()
        {
            this.repo = new TrackingSailDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        #region Get (Advances)
        public List<TrackingSail> GetAllToDoList(int userId)
        {
            return this.repo.GetAll().Where(t => t.IsLeaf.GetValueOrDefault() && !string.IsNullOrEmpty(t.PICIds) && t.PICIds.Split(';').Contains(userId.ToString()) && !t.IsComplete.GetValueOrDefault()).ToList();
        }
        public List<TrackingSail> GetAllRevTrackingSailOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.SourceName.ToLower().Contains(searchAll.ToLower())
                                            || t.NameObserver.ToLower().Contains(searchAll.ToLower())
                                            || t.Location.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.Action.ToLower().Contains(searchAll.ToLower())
                                            || t.ProposedAction.ToLower().Contains(searchAll.ToLower())
                                            || t.ActionTakeClose.ToLower().Contains(searchAll.ToLower())
                                            || t.HOCTrackingNo.ToLower().Contains(searchAll.ToLower())
                                            || t.MSRStatus.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            )).ToList();
        }

        public List<TrackingSail> GetAllTrackingSailOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && t.IsLeaf.GetValueOrDefault()
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.SourceName.ToLower().Contains(searchAll.ToLower())
                                            || t.NameObserver.ToLower().Contains(searchAll.ToLower())
                                            || t.Location.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.Action.ToLower().Contains(searchAll.ToLower())
                                            || t.ProposedAction.ToLower().Contains(searchAll.ToLower())
                                            || t.ActionTakeClose.ToLower().Contains(searchAll.ToLower())
                                            || t.HOCTrackingNo.ToLower().Contains(searchAll.ToLower())
                                            || t.MSRStatus.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            )).ToList();
        }

        public List<TrackingSail> GetAllOverDueTrackingSailOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsComplete.GetValueOrDefault()
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.SourceName.ToLower().Contains(searchAll.ToLower())
                                            || t.NameObserver.ToLower().Contains(searchAll.ToLower())
                                            || t.Location.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.Action.ToLower().Contains(searchAll.ToLower())
                                            || t.ProposedAction.ToLower().Contains(searchAll.ToLower())
                                            || t.ActionTakeClose.ToLower().Contains(searchAll.ToLower())
                                            || t.HOCTrackingNo.ToLower().Contains(searchAll.ToLower())
                                            || t.MSRStatus.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingSail> GetAllComingDueTrackingSailOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsComplete.GetValueOrDefault()
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.SourceName.ToLower().Contains(searchAll.ToLower())
                                            || t.NameObserver.ToLower().Contains(searchAll.ToLower())
                                            || t.Location.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.Action.ToLower().Contains(searchAll.ToLower())
                                            || t.ProposedAction.ToLower().Contains(searchAll.ToLower())
                                            || t.ActionTakeClose.ToLower().Contains(searchAll.ToLower())
                                            || t.HOCTrackingNo.ToLower().Contains(searchAll.ToLower())
                                            || t.MSRStatus.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingSail> GetAllCompleteTrackingSailOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsComplete.GetValueOrDefault()
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.SourceName.ToLower().Contains(searchAll.ToLower())
                                            || t.NameObserver.ToLower().Contains(searchAll.ToLower())
                                            || t.Location.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.Action.ToLower().Contains(searchAll.ToLower())
                                            || t.ProposedAction.ToLower().Contains(searchAll.ToLower())
                                            || t.ActionTakeClose.ToLower().Contains(searchAll.ToLower())
                                            || t.HOCTrackingNo.ToLower().Contains(searchAll.ToLower())
                                            || t.MSRStatus.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingSail> GetAllInCompleteTrackingSailOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsLeaf.GetValueOrDefault() && !t.IsComplete.GetValueOrDefault()
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.SourceName.ToLower().Contains(searchAll.ToLower())
                                            || t.NameObserver.ToLower().Contains(searchAll.ToLower())
                                            || t.Location.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.Action.ToLower().Contains(searchAll.ToLower())
                                            || t.ProposedAction.ToLower().Contains(searchAll.ToLower())
                                            || t.ActionTakeClose.ToLower().Contains(searchAll.ToLower())
                                            || t.HOCTrackingNo.ToLower().Contains(searchAll.ToLower())
                                            || t.MSRStatus.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList(); ;
        }



        public List<TrackingSail> GetAllSailRev(Guid parentId)
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
        public List<TrackingSail> GetAll()
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
        public TrackingSail GetById(Guid id)
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
        public Guid? Insert(TrackingSail bo)
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
                    ObjectName = "[WMS].[TrackingSail]",
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
        public bool Update(TrackingSail bo)
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
                        ObjectName = "[WMS].[TrackingSail]",
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
        public bool Delete(TrackingSail bo)
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
                        ObjectName = "[WMS].[TrackingSail]",
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
