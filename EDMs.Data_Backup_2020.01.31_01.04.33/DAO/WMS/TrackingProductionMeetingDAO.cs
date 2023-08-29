// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackingProductionMeetingDAO.cs" company="">
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
    public class TrackingProductionMeetingDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingProductionMeetingDAO"/> class.
        /// </summary>
        public TrackingProductionMeetingDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<TrackingProductionMeeting> GetIQueryable()
        {
            return this.EDMsDataContext.TrackingProductionMeetings;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<TrackingProductionMeeting> GetAll()
        {
            return this.EDMsDataContext.TrackingProductionMeetings.OrderByDescending(t => t.ID).ToList();
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
        public TrackingProductionMeeting GetById(Guid id)
        {
            return this.EDMsDataContext.TrackingProductionMeetings.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(TrackingProductionMeeting ob)
        {
            try
            {
                this.EDMsDataContext.AddToTrackingProductionMeetings(ob);
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
        public bool Update(TrackingProductionMeeting src)
        {
            try
            {
                TrackingProductionMeeting des = (from rs in this.EDMsDataContext.TrackingProductionMeetings
                                where rs.ID == src.ID
                                select rs).First();

                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;

                des.BODCommand = src.BODCommand;
                des.MainProblem = src.MainProblem;
                des.DeparmentName = src.DeparmentName;
                des.ManagerIds = src.ManagerIds;
                des.ManagerName = src.ManagerName;
                des.PICName = src.PICName;
                des.PICIds = src.PICIds;

                des.Deadline = src.Deadline;
                des.UpdateComment = src.UpdateComment;
                des.Note = src.Note;
                des.StatusId = src.StatusId;
                des.StatusName = src.StatusName;
                des.Code = src.Code;

                des.ParentId = src.ParentId;
                des.IsComplete = src.IsComplete;
                des.IsLeaf = src.IsLeaf;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedByName = src.UpdatedByName;
                des.UpdatedDate = src.UpdatedDate;
                des.WorkGroup = src.WorkGroup;
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
        public bool Delete(TrackingProductionMeeting src)
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
