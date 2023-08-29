// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackingProcedureDAO.cs" company="">
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
    public class TrackingProcedureDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingProcedureDAO"/> class.
        /// </summary>
        public TrackingProcedureDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<TrackingProcedure> GetIQueryable()
        {
            return this.EDMsDataContext.TrackingProcedures;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<TrackingProcedure> GetAll()
        {
            return this.EDMsDataContext.TrackingProcedures.OrderByDescending(t => t.ID).ToList();
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
        public TrackingProcedure GetById(Guid id)
        {
            return this.EDMsDataContext.TrackingProcedures.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(TrackingProcedure ob)
        {
            try
            {
                this.EDMsDataContext.AddToTrackingProcedures(ob);
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
        public bool Update(TrackingProcedure src)
        {
            try
            {
                TrackingProcedure des = (from rs in this.EDMsDataContext.TrackingProcedures
                                where rs.ID == src.ID
                                select rs).First();


                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;
                des.SystemName = src.SystemName;
                des.Remark = src.Remark;
                des.OldCode = src.OldCode;
                des.NewCode = src.NewCode;
                des.ProcedureName = src.ProcedureName;
                des.PICName = src.PICName;
                des.PICIds = src.PICIds;
                des.Checker = src.Checker;
                des.CheckerIds = src.CheckerIds;
                des.TargerStage = src.TargerStage;
                des.StartDate = src.StartDate;
                des.CompleteDate = src.CompleteDate;
                des.TotalPage = src.TotalPage;
                des.DifficultLvl = src.DifficultLvl;
                des.OfficeManday = src.OfficeManday;
                des.OffshoreManDay = src.OffshoreManDay;
                des.CreateType = src.CreateType;
                des.PercentComplete = src.PercentComplete;
                des.Status = src.Status;
                des.Deadline = src.Deadline;
                des.UpdatedInAMOS = src.UpdatedInAMOS;
                des.LevelName = src.LevelName;

                des.ParentId = src.ParentId;
                des.IsComplete = src.IsComplete;
                des.IsLeaf = src.IsLeaf;

                des.TypeId = src.TypeId;
                des.TypeName = src.TypeName;    
                des.TargerStageId = src.TargerStageId;
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
        public bool Delete(TrackingProcedure src)
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
