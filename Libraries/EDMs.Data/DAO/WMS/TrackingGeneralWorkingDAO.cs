// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackingGeneralWorkingDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Data.Entities;

namespace EDMs.Data.DAO.WMS
{
    /// <summary>
    /// The category dao.
    /// </summary>
    public class TrackingGeneralWorkingDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingGeneralWorkingDAO"/> class.
        /// </summary>
        public TrackingGeneralWorkingDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<TrackingGeneralWorking> GetIQueryable()
        {
            return this.EDMsDataContext.TrackingGeneralWorkings;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<TrackingGeneralWorking> GetAll()
        {
            return this.EDMsDataContext.TrackingGeneralWorkings.OrderByDescending(t => t.ID).ToList();
        }

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Resource"/>.
        /// </returns>
        public TrackingGeneralWorking GetById(Guid id)
        {
            return this.EDMsDataContext.TrackingGeneralWorkings.FirstOrDefault(ob => ob.ID == id);
        }
       
        #endregion

        #region GET ADVANCE

        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="ob">
        /// The ob.
        /// </param>
        /// <returns>
        /// The <see cref="int?"/>.
        /// </returns>
        public Guid? Insert(TrackingGeneralWorking ob)
        {
            try
            {
                this.EDMsDataContext.AddToTrackingGeneralWorkings(ob);
                this.EDMsDataContext.SaveChanges();
                return ob.ID;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="src">
        /// Entity for update
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True if update success, false if not
        /// </returns>
        public bool Update(TrackingGeneralWorking src)
        {
            try
            {
                TrackingGeneralWorking des = (from rs in this.EDMsDataContext.TrackingGeneralWorkings
                                where rs.ID == src.ID
                                select rs).First();


                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;

                des.WorkCategoryId = src.WorkCategoryId;
                des.WorkCategoryName = src.WorkCategoryName;
                des.WorkContent = src.WorkContent;
                des.AssignUserIds = src.AssignUserIds;
                des.AssignUserName = src.AssignUserName;
                des.BackupUserIds = src.BackupUserIds;
                des.BackupUserName = src.BackupUserName;
                des.VerifyUserIds = src.VerifyUserIds;
                des.VerifyUserName = src.VerifyUserName;
                des.StartDate = src.StartDate;
                des.StartDate1 = src.StartDate1;
                des.Deadline1 = src.Deadline1;
                des.Deadline = src.Deadline;
                des.Status = src.Status;
                des.Description = src.Description;
                des.ParentId = src.ParentId;
                des.IsComplete = src.IsComplete;
                des.IsLeaf = src.IsLeaf;
                des.DeadlineReasonChange = src.DeadlineReasonChange;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedByName = src.UpdatedByName;
                des.UpdatedDate = src.UpdatedDate;

                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="src">
        /// The src.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True if delete success, false if not
        /// </returns>
        public bool Delete(TrackingGeneralWorking src)
        {
            try
            {
                var des = this.GetById(src.ID);
                if (des != null)
                {
                    this.EDMsDataContext.DeleteObject(des);
                    this.EDMsDataContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Delete By ID
        /// </summary>
        /// <param name="ID"></param>
        /// ID of entity
        /// <returns></returns>
        public bool Delete(Guid ID)
        {
            try
            {
                var des = this.GetById(ID);
                if (des != null)
                {
                    this.EDMsDataContext.DeleteObject(des);
                    this.EDMsDataContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
