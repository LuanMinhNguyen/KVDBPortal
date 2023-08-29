// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackingMorningCallDAO.cs" company="">
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
    public class TrackingMorningCallDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingMorningCallDAO"/> class.
        /// </summary>
        public TrackingMorningCallDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<TrackingMorningCall> GetIQueryable()
        {
            return this.EDMsDataContext.TrackingMorningCalls;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<TrackingMorningCall> GetAll()
        {
            return this.EDMsDataContext.TrackingMorningCalls.OrderByDescending(t => t.ID).ToList();
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
        public TrackingMorningCall GetById(Guid id)
        {
            return this.EDMsDataContext.TrackingMorningCalls.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(TrackingMorningCall ob)
        {
            try
            {
                this.EDMsDataContext.AddToTrackingMorningCalls(ob);
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
        public bool Update(TrackingMorningCall src)
        {
            try
            {
                TrackingMorningCall des = (from rs in this.EDMsDataContext.TrackingMorningCalls
                                where rs.ID == src.ID
                                select rs).First();


                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;

                des.DateRaised = src.DateRaised;
                des.EquipmentName = src.EquipmentName;
                des.IssueDescription = src.IssueDescription;
                des.InitRiskAssessment = src.InitRiskAssessment;
                des.ActionPlanDescription = src.ActionPlanDescription;
                des.ActionPlanRiskLvlId = src.ActionPlanRiskLvlId;
                des.ActionPlanRiskLvlName = src.ActionPlanRiskLvlName;
                des.PICId = src.PICId;
                des.PICName = src.PICName;
                des.Deadline = src.Deadline;
                des.DeadlineHistory = src.DeadlineHistory;
                des.RelativeDoc = src.RelativeDoc;
                des.CurrentUpdate = src.CurrentUpdate;
                des.OffshoreComment = src.OffshoreComment;
                des.InitRiskLvlId = src.InitRiskLvlId;
                des.InitRiskLvlName = src.InitRiskLvlName;
                des.StatusId = src.StatusId;
                des.StatusName = src.StatusName;

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
        public bool Delete(TrackingMorningCall src)
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
