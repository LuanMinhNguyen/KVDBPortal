// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackingPunchDAO.cs" company="">
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
    public class TrackingPunchDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingPunchDAO"/> class.
        /// </summary>
        public TrackingPunchDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<TrackingPunch> GetIQueryable()
        {
            return this.EDMsDataContext.TrackingPunches;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<TrackingPunch> GetAll()
        {
            return this.EDMsDataContext.TrackingPunches.OrderByDescending(t => t.ID).ToList();
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
        public TrackingPunch GetById(Guid id)
        {
            return this.EDMsDataContext.TrackingPunches.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(TrackingPunch ob)
        {
            try
            {
                this.EDMsDataContext.AddToTrackingPunches(ob);
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
        public bool Update(TrackingPunch src)
        {
            try
            {
                TrackingPunch des = (from rs in this.EDMsDataContext.TrackingPunches
                                where rs.ID == src.ID
                                select rs).First();


                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;
                des.Remark = src.Remark;
                des.CatAB = src.CatAB;
                des.Description = src.Description;
                des.Reason = src.Reason;
                des.DrawingNo = src.DrawingNo;
                des.Location = src.Location;
                des.DateRaised = src.DateRaised;
                des.Name = src.Name;
                des.RaisedBy = src.RaisedBy;
                des.PPSApproval = src.PPSApproval;
                des.ShipOwnerAction = src.ShipOwnerAction;
                des.ShipOwnerApproval = src.ShipOwnerApproval;
                des.MaterialRequire = src.MaterialRequire;
                des.PONo = src.PONo;
                des.TargetDate = src.TargetDate;
                des.Priority = src.Priority;
                des.CloseOutDate = src.CloseOutDate;
                des.Deadline = src.Deadline;
                des.WayForward = src.WayForward;
                des.VerifyBy = src.VerifyBy;
                des.Status = src.Status;
                des.Impact = src.Impact;
                des.SystemNo = src.SystemNo;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedByName = src.UpdatedByName;
                des.UpdatedDate = src.UpdatedDate;

                des.PICName = src.PICName;
                des.PICIds = src.PICIds;

                des.ParentId = src.ParentId;
                des.IsComplete = src.IsComplete;
                des.IsLeaf = src.IsLeaf;
                des.DeadlineReasonChange = src.DeadlineReasonChange;
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
        public bool Delete(TrackingPunch src)
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
