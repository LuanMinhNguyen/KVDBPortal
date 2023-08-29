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
    public class TrackingProductionMeetingService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly TrackingProductionMeetingDAO repo;
        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingProductionMeetingService"/> class.
        /// </summary>
        public TrackingProductionMeetingService()
        {
            this.repo = new TrackingProductionMeetingDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        #region Get (Advances)
        public List<TrackingProductionMeeting> GetAllRevTrackingProductionMeetingOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.BODCommand.ToLower().Contains(searchAll.ToLower())
                                            || t.MainProblem.ToLower().Contains(searchAll.ToLower())
                                            || t.ManagerName.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.DeparmentName.ToLower().Contains(searchAll.ToLower())
                                            || t.Deadline.ToLower().Contains(searchAll.ToLower())
                                            || t.UpdateComment.ToLower().Contains(searchAll.ToLower())
                                            || t.Note.ToLower().Contains(searchAll.ToLower())
                                            || t.StatusName.ToLower().Contains(searchAll.ToLower())
                                            
                                            )).ToList();
        }

        public List<TrackingProductionMeeting> GetAllTrackingProductionMeetingOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && t.IsLeaf.GetValueOrDefault()
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.BODCommand.ToLower().Contains(searchAll.ToLower())
                                            || t.MainProblem.ToLower().Contains(searchAll.ToLower())
                                            || t.ManagerName.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.DeparmentName.ToLower().Contains(searchAll.ToLower())
                                            || t.Deadline.ToLower().Contains(searchAll.ToLower())
                                            || t.UpdateComment.ToLower().Contains(searchAll.ToLower())
                                            || t.Note.ToLower().Contains(searchAll.ToLower())
                                            || t.StatusName.ToLower().Contains(searchAll.ToLower())
                                            )).ToList();
        }

        public List<TrackingProductionMeeting> GetAllProductionMeetingRev(Guid parentId)
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
        public List<TrackingProductionMeeting> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        public List<TrackingProductionMeeting> GetAllByProject(int projectId)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsLeaf.GetValueOrDefault()).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public TrackingProductionMeeting GetById(Guid id)
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
        public Guid? Insert(TrackingProductionMeeting bo)
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
                    ObjectName = "[WMS].[TrackingProductionMeeting]",
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
        public bool Update(TrackingProductionMeeting bo)
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
                        ObjectName = "[WMS].[TrackingProductionMeeting]",
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
        public bool Delete(TrackingProductionMeeting bo)
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
                        ObjectName = "[WMS].[TrackingProductionMeeting]",
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
