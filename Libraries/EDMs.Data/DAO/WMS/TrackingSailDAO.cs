// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackingSailDAO.cs" company="">
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
    public class TrackingSailDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingSailDAO"/> class.
        /// </summary>
        public TrackingSailDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<TrackingSail> GetIQueryable()
        {
            return this.EDMsDataContext.TrackingSails;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<TrackingSail> GetAll()
        {
            return this.EDMsDataContext.TrackingSails.OrderByDescending(t => t.ID).ToList();
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
        public TrackingSail GetById(Guid id)
        {
            return this.EDMsDataContext.TrackingSails.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(TrackingSail ob)
        {
            try
            {
                this.EDMsDataContext.AddToTrackingSails(ob);
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
        public bool Update(TrackingSail src)
        {
            try
            {
                TrackingSail des = (from rs in this.EDMsDataContext.TrackingSails
                                where rs.ID == src.ID
                                select rs).First();


                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;
                des.DateRaised = src.DateRaised;
                des.SourceId = src.SourceId;
                des.SourceName = src.SourceName;
                des.NameObserver = src.NameObserver;
                des.Location = src.Location;
                des.Description = src.Description;
                des.Action = src.Action;
                des.ProposedAction = src.ProposedAction;
                des.Priority = src.Priority;
                des.TargetClose = src.TargetClose;
                des.ActionTakeClose = src.ActionTakeClose;
                des.ClosedDate = src.ClosedDate;
                des.HOCTrackingNo = src.HOCTrackingNo;
                des.MSRStatus = src.MSRStatus;

                des.PICName = src.PICName;
                des.PICIds = src.PICIds;

                des.ParentId = src.ParentId;
                des.IsComplete = src.IsComplete;
                des.IsLeaf = src.IsLeaf;

                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedByName = src.UpdatedByName;
                des.UpdatedDate = src.UpdatedDate;

                des.WRNo = src.WRNo;
                des.MOCNo = src.MOCNo;
                des.ECRNo = src.ECRNo;

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
        public bool Delete(TrackingSail src)
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
