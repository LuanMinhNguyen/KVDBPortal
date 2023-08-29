using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Business.Services.Security;
using EDMs.Data.DAO.WMS;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.WMS
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class WorkRequestService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly WorkRequestDAO repo;

        private readonly WaitingSyncDataService waitingSyncDataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkRequestService"/> class.
        /// </summary>
        public WorkRequestService()
        {
            this.repo = new WorkRequestDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        #region Get (Advances)
        public List<WorkRequest> GetAll(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || (!string.IsNullOrEmpty(t.WRNo) && t.WRNo.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.WRTitle) && t.WRTitle.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.OriginatorName) && t.OriginatorName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.OriginatorJobTitle) && t.OriginatorJobTitle.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.ScopeOfService) && t.ScopeOfService.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.Description) && t.Description.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.Reason) && t.Reason.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.PriorityLevelName) && t.PriorityLevelName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.DepartmentName) && t.DepartmentName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.FinalAssignDeptName) && t.FinalAssignDeptName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.WRNo).ToList();
        }

        public List<WorkRequest> GetAllCompletedWR(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && t.IsWFComplete.GetValueOrDefault()
                                                && t.IsCompleteFinal.GetValueOrDefault()
                                                && !t.IsCancel.GetValueOrDefault()

                                                && (string.IsNullOrEmpty(searchAll)

                                                    || (!string.IsNullOrEmpty(t.WRNo) && t.WRNo.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.WRTitle) && t.WRTitle.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.OriginatorName) && t.OriginatorName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.OriginatorJobTitle) && t.OriginatorJobTitle.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.ScopeOfService) && t.ScopeOfService.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.Description) && t.Description.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.Reason) && t.Reason.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.PriorityLevelName) && t.PriorityLevelName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.DepartmentName) && t.DepartmentName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.FinalAssignDeptName) && t.FinalAssignDeptName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.WRNo).ToList();
        }

        public List<WorkRequest> GetAllCompletedWRWaitingPrint(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && t.IsWFComplete.GetValueOrDefault()
                                                && !t.IsCompleteFinal.GetValueOrDefault()
                                                && !t.IsCancel.GetValueOrDefault()

                                                && (string.IsNullOrEmpty(searchAll)

                                                    || (!string.IsNullOrEmpty(t.WRNo) && t.WRNo.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.WRTitle) && t.WRTitle.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.OriginatorName) && t.OriginatorName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.OriginatorJobTitle) && t.OriginatorJobTitle.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.ScopeOfService) && t.ScopeOfService.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.Description) && t.Description.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.Reason) && t.Reason.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.PriorityLevelName) && t.PriorityLevelName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.DepartmentName) && t.DepartmentName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.FinalAssignDeptName) && t.FinalAssignDeptName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.WRNo).ToList();
        }

        public List<WorkRequest> GetAllIncompleteWR(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsCompleteFinal.GetValueOrDefault()
                                                && (string.IsNullOrEmpty(searchAll)

                                                    || (!string.IsNullOrEmpty(t.WRNo) && t.WRNo.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.WRTitle) && t.WRTitle.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.OriginatorName) && t.OriginatorName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.OriginatorJobTitle) && t.OriginatorJobTitle.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.ScopeOfService) && t.ScopeOfService.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.Description) && t.Description.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.Reason) && t.Reason.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.PriorityLevelName) && t.PriorityLevelName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.DepartmentName) && t.DepartmentName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.FinalAssignDeptName) && t.FinalAssignDeptName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.WRNo).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<WorkRequest> GetAll()
        {
            return this.repo.GetAll().OrderBy(t => t.ID).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public WorkRequest GetById(Guid id)
        {
            return this.repo.GetById(id);
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public Guid? Insert(WorkRequest bo)
        {
            var objId = this.repo.Insert(bo);
            // Trigger data change
            if (objId != null)
            {
                var changeData = new WaitingSyncData()
                {
                    ActionTypeID = 1,
                    ActionTypeName = "Insert",
                    ObjectID = objId,
                    ObjectName = "[WMS].[WorkRequest]",
                    EffectDate = DateTime.Now,IsSynced = false
                };

                this.waitingSyncDataService.Insert(changeData);
            }

            // -------------------------------------------------------

            return objId;
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(WorkRequest bo)
        {
            try
            {
                var flag = this.repo.Update(bo);
                if (flag)
                {
                    var changeData = new WaitingSyncData()
                    {
                        ActionTypeID = 2,
                        ActionTypeName = "Update",
                        ObjectID = bo.ID,
                        ObjectName = "[WMS].[WorkRequest]",
                        EffectDate = DateTime.Now,IsSynced = false
                    };

                    this.waitingSyncDataService.Insert(changeData);
                }

                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(WorkRequest bo)
        {
            try
            {
                return this.repo.Delete(bo);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete Resource By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Guid id)
        {
            try
            {
                var flag = this.repo.Delete(id);
                if (flag)
                {
                    // Trigger data change
                    var changeData = new WaitingSyncData()
                    {
                        ActionTypeID = 3,
                        ActionTypeName = "Delete",
                        ObjectID = id,
                        ObjectName = "[WMS].[WorkRequest]",
                        EffectDate = DateTime.Now,IsSynced = false
                    };

                    this.waitingSyncDataService.Insert(changeData);
                    // ----------------------------------------------------------------------
                }

                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
