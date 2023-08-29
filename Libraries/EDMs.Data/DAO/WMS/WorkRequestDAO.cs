// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkRequestDAO.cs" company="">
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
    public class WorkRequestDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkRequestDAO"/> class.
        /// </summary>
        public WorkRequestDAO() : base()
        {
        }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<WorkRequest> GetIQueryable()
        {
            return this.EDMsDataContext.WorkRequests;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<WorkRequest> GetAll()
        {
            return this.EDMsDataContext.WorkRequests.OrderByDescending(t => t.ID).ToList();
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
        public WorkRequest GetById(Guid id)
        {
            return this.EDMsDataContext.WorkRequests.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(WorkRequest ob)
        {
            try
            {
                this.EDMsDataContext.AddToWorkRequests(ob);
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
        public bool Update(WorkRequest src)
        {
            try
            {
                WorkRequest des = (from rs in this.EDMsDataContext.WorkRequests
                    where rs.ID == src.ID
                    select rs).First();

                des.WRNo = src.WRNo;
                des.WRTitle = src.WRTitle;
                des.OriginatorId = src.OriginatorId;
                des.OriginatorJobTitle = src.OriginatorJobTitle;
                des.OriginatorName = src.OriginatorName;
                des.RaisedDate = src.RaisedDate;
                des.RequriedDate = src.RequriedDate;
                des.Description = src.Description;
                des.ScopeOfService = src.ScopeOfService;
                des.Reason = src.Reason;
                des.PriorityLevelName = src.PriorityLevelName;
                des.PriotiyLevelId = src.PriotiyLevelId;

                des.OIMReviewDate = src.OIMReviewDate;
                des.OIMReviewId = src.OIMReviewId;
                des.OIMReviewName = src.OIMReviewName;

                des.OperationManagerReviewDate = src.OperationManagerReviewDate;
                des.OperationManagerReviewId = src.OperationManagerReviewId;
                des.OperationManagerReviewName = src.OperationManagerReviewName;

                des.TechnicalDeptReviewDate = src.TechnicalDeptReviewDate;
                des.TechnicalDeptReviewId = src.TechnicalDeptReviewId;
                des.TechnicalDeptReviewName = src.TechnicalDeptReviewName;

                des.DirectorReviewDate = src.DirectorReviewDate;
                des.DirectorReviewId = src.DirectorReviewId;
                des.DirectorReviewName = src.DirectorReviewName;

                des.UpdatedByName = src.UpdatedByName;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedDate = src.UpdatedDate;

                des.DepartmentName = src.DepartmentName;
                des.DepartmentId = src.DepartmentId;

                des.IsWFComplete = src.IsWFComplete;
                des.IsInWFProcess = src.IsInWFProcess;

                des.CurrentWorkflowName = src.CurrentWorkflowName;
                des.CurrentWorkflowStepName = src.CurrentWorkflowStepName;
                des.CurrentAssignUserName = src.CurrentAssignUserName;
                des.IsCompleteFinal = src.IsCompleteFinal;

                des.FinalAssignDeptName = src.FinalAssignDeptName;
                des.FinalAssignDeptId = src.FinalAssignDeptId;

                des.IsCancel = src.IsCancel;
                des.RiskAssessment = src.RiskAssessment;
                des.DateReceivedFromBOD = src.DateReceivedFromBOD;
                des.PICIds = src.PICIds;
                des.PICName = src.PICName;
                des.HODIds = src.HODIds;
                des.HODName = src.HODName;
                des.DatePassFuncDept = src.DatePassFuncDept;
                des.ActionPlan = src.ActionPlan;
                des.FunctionDeptUpdate = src.FunctionDeptUpdate;
                des.DeadlineToComplete = src.DeadlineToComplete;
                des.OverdueReason = src.OverdueReason;
                des.Remark = src.Remark;
                des.Status = src.Status;

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
        public bool Delete(WorkRequest src)
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
