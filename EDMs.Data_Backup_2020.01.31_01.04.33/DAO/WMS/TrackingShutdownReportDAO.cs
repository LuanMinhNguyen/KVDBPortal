// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackingShutdownReportDAO.cs" company="">
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
    public class TrackingShutdownReportDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingShutdownReportDAO"/> class.
        /// </summary>
        public TrackingShutdownReportDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<TrackingShutdownReport> GetIQueryable()
        {
            return this.EDMsDataContext.TrackingShutdownReports;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<TrackingShutdownReport> GetAll()
        {
            return this.EDMsDataContext.TrackingShutdownReports.OrderByDescending(t => t.ID).ToList();
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
        public TrackingShutdownReport GetById(Guid id)
        {
            return this.EDMsDataContext.TrackingShutdownReports.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(TrackingShutdownReport ob)
        {
            try
            {
                this.EDMsDataContext.AddToTrackingShutdownReports(ob);
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
        public bool Update(TrackingShutdownReport src)
        {
            try
            {
                TrackingShutdownReport des = (from rs in this.EDMsDataContext.TrackingShutdownReports
                                where rs.ID == src.ID
                                select rs).First();


                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;
                des.DateOfShutdown = src.DateOfShutdown;
                des.TimeOfShutdown = src.TimeOfShutdown;
                des.DateResume = src.DateResume;
                des.TimeResume = src.TimeResume;
                des.DownTime = src.DownTime;
                des.EstimatedProduction = src.EstimatedProduction;
                des.CauseShutdown = src.CauseShutdown;
                des.CauseClarificationFireGas = src.CauseClarificationFireGas;
                des.CauseClarificationPowerloss = src.CauseClarificationPowerloss;
                des.CauseClarificationProcess = src.CauseClarificationProcess;
                des.RootCause = src.RootCause;
                des.AreaConcern = src.AreaConcern;
                des.WayForward = src.WayForward;
                des.PICName = src.PICName;
                des.PICIds = src.PICIds;
                des.Deadline = src.Deadline;
                des.Status = src.Status;
                des.Lesson = src.Lesson;

                des.ParentId = src.ParentId;
                des.IsComplete = src.IsComplete;
                des.IsLeaf = src.IsLeaf;

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
        public bool Delete(TrackingShutdownReport src)
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
