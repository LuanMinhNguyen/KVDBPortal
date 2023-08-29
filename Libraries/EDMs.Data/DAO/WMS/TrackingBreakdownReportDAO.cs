// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackingBreakdownReportDAO.cs" company="">
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
    public class TrackingBreakdownReportDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingBreakdownReportDAO"/> class.
        /// </summary>
        public TrackingBreakdownReportDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<TrackingBreakdownReport> GetIQueryable()
        {
            return this.EDMsDataContext.TrackingBreakdownReports;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<TrackingBreakdownReport> GetAll()
        {
            return this.EDMsDataContext.TrackingBreakdownReports.OrderByDescending(t => t.ID).ToList();
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
        public TrackingBreakdownReport GetById(Guid id)
        {
            return this.EDMsDataContext.TrackingBreakdownReports.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(TrackingBreakdownReport ob)
        {
            try
            {
                this.EDMsDataContext.AddToTrackingBreakdownReports(ob);
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
        public bool Update(TrackingBreakdownReport src)
        {
            try
            {
                TrackingBreakdownReport des = (from rs in this.EDMsDataContext.TrackingBreakdownReports
                                where rs.ID == src.ID
                                select rs).First();


                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;
                des.SystemName = src.SystemName;
                des.BrekdownDate = src.BrekdownDate;
                des.BreakdownSystemName = src.BreakdownSystemName;
                des.TagNo = src.TagNo;
                des.Priority = src.Priority;
                des.CauseGroup = src.CauseGroup;
                des.Description = src.Description;
                des.FailureDuplication = src.FailureDuplication;
                des.RootCause = src.RootCause;
                des.ProposedAction = src.ProposedAction;
                des.Lesson = src.Lesson;
                des.UnplannedWoNo = src.UnplannedWoNo;
                des.PICIds = src.PICIds;
                des.PICName = src.PICName;
                des.PICDeadline = src.PICDeadline;
                des.PICStatus = src.PICStatus;
                des.CurrentStatus = src.CurrentStatus;
                des.MRWRItem = src.MRWRItem;
                des.Status = src.Status;
                des.Comment = src.Comment;
                des.Cost = src.Cost;

                des.ParentId = src.ParentId;
                des.IsComplete = src.IsComplete;
                des.IsLeaf = src.IsLeaf;
                des.DeadlineReasonChange = src.DeadlineReasonChange;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedByName = src.UpdatedByName;
                des.UpdatedDate = src.UpdatedDate;

                des.IsWFComplete = src.IsWFComplete;
                des.IsInWFProcess = src.IsInWFProcess;

                des.CurrentWorkflowName = src.CurrentWorkflowName;
                des.CurrentWorkflowStepName = src.CurrentWorkflowStepName;
                des.CurrentAssignUserName = src.CurrentAssignUserName;
                des.IsCompleteFinal = src.IsCompleteFinal;
                des.FinalAssignDeptName = src.FinalAssignDeptName;
                des.FinalAssignDeptId = src.FinalAssignDeptId;
                des.IsCancel = src.IsCancel;

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
        public bool Delete(TrackingBreakdownReport src)
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
