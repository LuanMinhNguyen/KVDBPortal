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
    public class TrackingProcedureService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly TrackingProcedureDAO repo;
        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingProcedureService"/> class.
        /// </summary>
        public TrackingProcedureService()
        {
            this.repo = new TrackingProcedureDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        #region Get (Advances)
        public List<TrackingProcedure> GetAllByProject(int projectId)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsLeaf.GetValueOrDefault()).ToList();
        }

        public List<TrackingProcedure> GetAllToDoList(int userId)
        {
            return this.repo.GetAll().Where(t => t.IsLeaf.GetValueOrDefault() && !string.IsNullOrEmpty(t.PICIds) && t.PICIds.Split(';').Contains(userId.ToString()) && !t.IsComplete.GetValueOrDefault()).ToList();
        }
        public List<TrackingProcedure> GetAllRevTrackingProcedureOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.OldCode.ToLower().Contains(searchAll.ToLower())
                                            || t.NewCode.ToLower().Contains(searchAll.ToLower())
                                            || t.ProcedureName.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.Checker.ToLower().Contains(searchAll.ToLower())
                                            || t.TargerStage.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.CreateType.ToLower().Contains(searchAll.ToLower())
                                            )).ToList();
        }

        public List<TrackingProcedure> GetAllTrackingProcedureOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && t.IsLeaf.GetValueOrDefault()
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.OldCode.ToLower().Contains(searchAll.ToLower())
                                            || t.NewCode.ToLower().Contains(searchAll.ToLower())
                                            || t.ProcedureName.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.Checker.ToLower().Contains(searchAll.ToLower())
                                            || t.TargerStage.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.CreateType.ToLower().Contains(searchAll.ToLower())
                                            )).ToList();
        }

        public List<TrackingProcedure> GetAllOverDueTrackingProcedureOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsComplete.GetValueOrDefault()
                                                //&& t.Deadline != null
                                                //&& t.Deadline < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null)
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.OldCode.ToLower().Contains(searchAll.ToLower())
                                            || t.NewCode.ToLower().Contains(searchAll.ToLower())
                                            || t.ProcedureName.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.Checker.ToLower().Contains(searchAll.ToLower())
                                            || t.TargerStage.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.CreateType.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingProcedure> GetAllComingDueTrackingProcedureOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsComplete.GetValueOrDefault()
                                                //&& t.Deadline != null
                                                //&& t.Deadline.GetValueOrDefault().AddDays(2) > DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) 
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.OldCode.ToLower().Contains(searchAll.ToLower())
                                            || t.NewCode.ToLower().Contains(searchAll.ToLower())
                                            || t.ProcedureName.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.Checker.ToLower().Contains(searchAll.ToLower())
                                            || t.TargerStage.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.CreateType.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingProcedure> GetAllCompleteTrackingProcedureOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsComplete.GetValueOrDefault()
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.OldCode.ToLower().Contains(searchAll.ToLower())
                                            || t.NewCode.ToLower().Contains(searchAll.ToLower())
                                            || t.ProcedureName.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.Checker.ToLower().Contains(searchAll.ToLower())
                                            || t.TargerStage.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.CreateType.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingProcedure> GetAllInCompleteTrackingProcedureOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsLeaf.GetValueOrDefault() && !t.IsComplete.GetValueOrDefault()
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.SystemName.ToLower().Contains(searchAll.ToLower())
                                            || t.OldCode.ToLower().Contains(searchAll.ToLower())
                                            || t.NewCode.ToLower().Contains(searchAll.ToLower())
                                            || t.ProcedureName.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            || t.Checker.ToLower().Contains(searchAll.ToLower())
                                            || t.TargerStage.ToLower().Contains(searchAll.ToLower())
                                            || t.Remark.ToLower().Contains(searchAll.ToLower())
                                            || t.CreateType.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList(); ;
        }



        public List<TrackingProcedure> GetAllProcedureRev(Guid parentId)
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
        public List<TrackingProcedure> GetAll()
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
        public TrackingProcedure GetById(Guid id)
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
        public Guid? Insert(TrackingProcedure bo)
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
                    ObjectName = "[WMS].[TrackingProcedure]",
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
        public bool Update(TrackingProcedure bo)
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
                        ObjectName = "[WMS].[TrackingProcedure]",
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
        public bool Delete(TrackingProcedure bo)
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
                        ObjectName = "[WMS].[TrackingProcedure]",
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
