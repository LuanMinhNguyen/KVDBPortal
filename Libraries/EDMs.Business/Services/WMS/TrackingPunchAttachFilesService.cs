﻿using System;
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
    public class TrackingPunchAttachFileService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly TrackingPunchAttachFileDAO repo;
        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingPunchAttachFileService"/> class.
        /// </summary>
        public TrackingPunchAttachFileService()
        {
            this.repo = new TrackingPunchAttachFileDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        #region Get (Advances)
        public List<TrackingPunchAttachFile> GetAllObjId(Guid objId)
        {
            return this.repo.GetAll().Where(t => t.PunchId == objId).OrderBy(t => t.CreatedDate).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<TrackingPunchAttachFile> GetAll()
        {
            return this.repo.GetAll().OrderBy(t => t.ID).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public TrackingPunchAttachFile GetById(Guid id)
        {
            return this.repo.GetById(id);
        }

        public List<TrackingPunchAttachFile> GetByPunch(Guid PunchId)
        {
            return this.repo.GetAll().Where(t => t.PunchId == PunchId).ToList();
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public Guid? Insert(TrackingPunchAttachFile bo)
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
                    ObjectName = "[WMS].[TrackingPunchAttachFiles]",
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
        public bool Update(TrackingPunchAttachFile bo)
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
                        ObjectName = "[WMS].[TrackingPunchAttachFiles]",
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
        public bool Delete(TrackingPunchAttachFile bo)
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
                        ObjectName = "[WMS].[TrackingPunchAttachFiles]",
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
