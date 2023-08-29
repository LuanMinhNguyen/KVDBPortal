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
    public class TrackingPunchService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly TrackingPunchDAO repo;
        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingPunchService"/> class.
        /// </summary>
        public TrackingPunchService()
        {
            this.repo = new TrackingPunchDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        #region Get (Advances)

        public List<TrackingPunch> GetAllByProject(int projectId)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsLeaf.GetValueOrDefault()).ToList();
        }

        public List<TrackingPunch> GetAllToDoList(int userId)
        {
            return this.repo.GetAll().Where(t => t.IsLeaf.GetValueOrDefault() && !string.IsNullOrEmpty(t.PICIds) && t.PICIds.Split(';').Contains(userId.ToString()) && !t.IsComplete.GetValueOrDefault()).ToList();
        }

        public List<TrackingPunch> GetAllRevTrackingPunchOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.CatAB.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.Reason.ToLower().Contains(searchAll.ToLower())
                                            || t.DrawingNo.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemNo.ToLower().Contains(searchAll.ToLower())
                                            || t.Location.ToLower().Contains(searchAll.ToLower())
                                            || t.Name.ToLower().Contains(searchAll.ToLower())
                                            || t.ShipOwnerAction.ToLower().Contains(searchAll.ToLower())
                                            || t.MaterialRequire.ToLower().Contains(searchAll.ToLower())
                                            || t.PONo.ToLower().Contains(searchAll.ToLower())
                                            || t.WayForward.ToLower().Contains(searchAll.ToLower())
                                            || t.Status.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.Impact.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            )).ToList();
        }

        public List<TrackingPunch> GetAllTrackingPunchOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && t.IsLeaf.GetValueOrDefault()
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.CatAB.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.Reason.ToLower().Contains(searchAll.ToLower())
                                            || t.DrawingNo.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemNo.ToLower().Contains(searchAll.ToLower())
                                            || t.Location.ToLower().Contains(searchAll.ToLower())
                                            || t.Name.ToLower().Contains(searchAll.ToLower())
                                            || t.ShipOwnerAction.ToLower().Contains(searchAll.ToLower())
                                            || t.MaterialRequire.ToLower().Contains(searchAll.ToLower())
                                            || t.PONo.ToLower().Contains(searchAll.ToLower())
                                            || t.WayForward.ToLower().Contains(searchAll.ToLower())
                                            || t.Status.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.Impact.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            )).ToList();
        }

        public List<TrackingPunch> GetAllOverDueTrackingPunchOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsComplete.GetValueOrDefault()
                                                //&& t.Deadline != null
                                                //&& t.Deadline < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null)
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.CatAB.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.Reason.ToLower().Contains(searchAll.ToLower())
                                            || t.DrawingNo.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemNo.ToLower().Contains(searchAll.ToLower())
                                            || t.Location.ToLower().Contains(searchAll.ToLower())
                                            || t.Name.ToLower().Contains(searchAll.ToLower())
                                            || t.ShipOwnerAction.ToLower().Contains(searchAll.ToLower())
                                            || t.MaterialRequire.ToLower().Contains(searchAll.ToLower())
                                            || t.PONo.ToLower().Contains(searchAll.ToLower())
                                            || t.WayForward.ToLower().Contains(searchAll.ToLower())
                                            || t.Status.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.Impact.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingPunch> GetAllComingDueTrackingPunchOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsComplete.GetValueOrDefault()
                                                //&& t.Deadline != null
                                                //&& t.Deadline.GetValueOrDefault().AddDays(2) > DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) 
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.CatAB.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.Reason.ToLower().Contains(searchAll.ToLower())
                                            || t.DrawingNo.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemNo.ToLower().Contains(searchAll.ToLower())
                                            || t.Location.ToLower().Contains(searchAll.ToLower())
                                            || t.Name.ToLower().Contains(searchAll.ToLower())
                                            || t.ShipOwnerAction.ToLower().Contains(searchAll.ToLower())
                                            || t.MaterialRequire.ToLower().Contains(searchAll.ToLower())
                                            || t.PONo.ToLower().Contains(searchAll.ToLower())
                                            || t.WayForward.ToLower().Contains(searchAll.ToLower())
                                            || t.Status.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.Impact.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingPunch> GetAllCompleteTrackingPunchOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsComplete.GetValueOrDefault()
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.CatAB.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.Reason.ToLower().Contains(searchAll.ToLower())
                                            || t.DrawingNo.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemNo.ToLower().Contains(searchAll.ToLower())
                                            || t.Location.ToLower().Contains(searchAll.ToLower())
                                            || t.Name.ToLower().Contains(searchAll.ToLower())
                                            || t.ShipOwnerAction.ToLower().Contains(searchAll.ToLower())
                                            || t.MaterialRequire.ToLower().Contains(searchAll.ToLower())
                                            || t.PONo.ToLower().Contains(searchAll.ToLower())
                                            || t.WayForward.ToLower().Contains(searchAll.ToLower())
                                            || t.Status.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.Impact.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingPunch> GetAllInCompleteTrackingPunchOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsLeaf.GetValueOrDefault() && !t.IsComplete.GetValueOrDefault()
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.CatAB.ToLower().Contains(searchAll.ToLower())
                                            || t.Description.ToLower().Contains(searchAll.ToLower())
                                            || t.Reason.ToLower().Contains(searchAll.ToLower())
                                            || t.DrawingNo.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemNo.ToLower().Contains(searchAll.ToLower())
                                            || t.Location.ToLower().Contains(searchAll.ToLower())
                                            || t.Name.ToLower().Contains(searchAll.ToLower())
                                            || t.ShipOwnerAction.ToLower().Contains(searchAll.ToLower())
                                            || t.MaterialRequire.ToLower().Contains(searchAll.ToLower())
                                            || t.PONo.ToLower().Contains(searchAll.ToLower())
                                            || t.WayForward.ToLower().Contains(searchAll.ToLower())
                                            || t.Status.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.Impact.ToLower().Contains(searchAll.ToLower()) 
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList(); ;
        }

        public List<TrackingPunch> GetAllPunchRev(Guid parentId)
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
        public List<TrackingPunch> GetAll()
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
        public TrackingPunch GetById(Guid id)
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
        public Guid? Insert(TrackingPunch bo)
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
                    ObjectName = "[WMS].[TrackingPunch]",
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
        public bool Update(TrackingPunch bo)
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
                        ObjectName = "[WMS].[TrackingPunch]",
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
        public bool Delete(TrackingPunch bo)
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
                        ObjectName = "[WMS].[TrackingPunch]",
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
