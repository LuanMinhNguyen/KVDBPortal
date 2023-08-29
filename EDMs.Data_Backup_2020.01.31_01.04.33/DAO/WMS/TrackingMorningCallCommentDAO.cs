// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackingMorningCallCommentDAO.cs" company="">
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
    public class TrackingMorningCallCommentDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingMorningCallCommentDAO"/> class.
        /// </summary>
        public TrackingMorningCallCommentDAO() : base()
        {
        }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<TrackingMorningCallComment> GetIQueryable()
        {
            return this.EDMsDataContext.TrackingMorningCallComments;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<TrackingMorningCallComment> GetAll()
        {
            return this.EDMsDataContext.TrackingMorningCallComments.OrderByDescending(t => t.ID).ToList();
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
        public TrackingMorningCallComment GetById(Guid id)
        {
            return this.EDMsDataContext.TrackingMorningCallComments.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(TrackingMorningCallComment ob)
        {
            try
            {
                this.EDMsDataContext.AddToTrackingMorningCallComments(ob);
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
        public bool Update(TrackingMorningCallComment src)
        {
            try
            {
                TrackingMorningCallComment des = (from rs in this.EDMsDataContext.TrackingMorningCallComments
                    where rs.ID == src.ID
                    select rs).First();

                des.MorningCallNo = src.MorningCallNo;
                des.MorningCallId = src.MorningCallId;
                des.CommentTypeId = src.CommentTypeId;
                des.Comment = src.Comment;
                des.CommentBy = src.CommentBy;
                des.CommentByName = src.CommentByName;
                des.CommentDate = src.CommentDate;
                des.CommentTypeName = src.CommentTypeName;

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
        public bool Delete(TrackingMorningCallComment src)
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
