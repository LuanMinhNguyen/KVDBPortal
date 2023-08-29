// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackingWCRDAO.cs" company="">
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
    public class TrackingWCRDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingWCRDAO"/> class.
        /// </summary>
        public TrackingWCRDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<TrackingWCR> GetIQueryable()
        {
            return this.EDMsDataContext.TrackingWCRs;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<TrackingWCR> GetAll()
        {
            return this.EDMsDataContext.TrackingWCRs.OrderByDescending(t => t.ID).ToList();
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
        public TrackingWCR GetById(Guid id)
        {
            return this.EDMsDataContext.TrackingWCRs.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(TrackingWCR ob)
        {
            try
            {
                this.EDMsDataContext.AddToTrackingWCRs(ob);
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
        public bool Update(TrackingWCR src)
        {
            try
            {
                TrackingWCR des = (from rs in this.EDMsDataContext.TrackingWCRs
                                where rs.ID == src.ID
                                select rs).First();


                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;
                des.Remark = src.Remark;
                des.Name = src.Name;
                des.Description = src.Description;
                des.Reason = src.Reason;
                des.Action = src.Action;
                des.OffshoreComment = src.OffshoreComment;
                des.Priority = src.Priority;
                des.OffshoreUpdate = src.OffshoreUpdate;
                des.ShipyardUpdate = src.ShipyardUpdate;
                des.OpenDate = src.OpenDate;
                des.CloseDate = src.CloseDate;
                des.Status = src.Status;
                des.OfficeComment = src.OfficeComment;

                des.ParentId = src.ParentId;
                des.IsComplete = src.IsComplete;
                des.IsLeaf = src.IsLeaf;

                des.PICName = src.PICName;
                des.PICIds = src.PICIds;

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
        public bool Delete(TrackingWCR src)
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
