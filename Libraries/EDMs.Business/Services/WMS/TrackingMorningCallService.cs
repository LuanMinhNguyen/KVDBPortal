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
    public class TrackingMorningCallService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly TrackingMorningCallDAO repo;
        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingMorningCallService"/> class.
        /// </summary>
        public TrackingMorningCallService()
        {
            this.repo = new TrackingMorningCallDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        #region Get (Advances)

        public List<TrackingMorningCall> GetAllByProject(int projectId)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsLeaf.GetValueOrDefault()).ToList();
        }

        public List<TrackingMorningCall> GetAllToDoList(int userId)
        {
            return this.repo.GetAll().Where(t => t.IsLeaf.GetValueOrDefault() && !string.IsNullOrEmpty(t.PICId) && t.PICId.Split(';').Contains(userId.ToString()) && !t.IsComplete.GetValueOrDefault()).ToList();
        }

        public List<TrackingMorningCall> GetAllRevTrackingMorningCallOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.EquipmentName.ToLower().Contains(searchAll.ToLower())
                                            || t.IssueDescription.ToLower().Contains(searchAll.ToLower())
                                            || t.ActionPlanDescription.ToLower().Contains(searchAll.ToLower())
                                            || t.InitRiskAssessment.ToLower().Contains(searchAll.ToLower())
                                            || t.CurrentUpdate.ToLower().Contains(searchAll.ToLower())
                                            || t.OffshoreComment.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            )).ToList();
        }

        public List<TrackingMorningCall> GetAllTrackingMorningCallOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                        && t.IsLeaf.GetValueOrDefault()
                                        && (string.IsNullOrEmpty(searchAll)
                                            || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.EquipmentName.ToLower().Contains(searchAll.ToLower())
                                            || t.IssueDescription.ToLower().Contains(searchAll.ToLower())
                                            || t.ActionPlanDescription.ToLower().Contains(searchAll.ToLower())
                                            || t.InitRiskAssessment.ToLower().Contains(searchAll.ToLower())
                                            || t.CurrentUpdate.ToLower().Contains(searchAll.ToLower())
                                            || t.OffshoreComment.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                            )).ToList();
        }

        public List<TrackingMorningCall> GetAllOverDueTrackingMorningCallOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId 
                                                && !t.IsComplete.GetValueOrDefault()
                                                && t.Deadline != null 
                                                //&& t.Deadline < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null)
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.EquipmentName.ToLower().Contains(searchAll.ToLower())
                                            || t.IssueDescription.ToLower().Contains(searchAll.ToLower())
                                            || t.ActionPlanDescription.ToLower().Contains(searchAll.ToLower())
                                            || t.InitRiskAssessment.ToLower().Contains(searchAll.ToLower())
                                            || t.CurrentUpdate.ToLower().Contains(searchAll.ToLower())
                                            || t.OffshoreComment.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingMorningCall> GetAllComingDueTrackingMorningCallOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId 
                                                && !t.IsComplete.GetValueOrDefault()
                                                && t.Deadline != null 
                                                //&& t.Deadline.GetValueOrDefault().AddDays(2) > DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) 
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.EquipmentName.ToLower().Contains(searchAll.ToLower())
                                            || t.IssueDescription.ToLower().Contains(searchAll.ToLower())
                                            || t.ActionPlanDescription.ToLower().Contains(searchAll.ToLower())
                                            || t.InitRiskAssessment.ToLower().Contains(searchAll.ToLower())
                                            || t.CurrentUpdate.ToLower().Contains(searchAll.ToLower())
                                            || t.OffshoreComment.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingMorningCall> GetAllCompleteTrackingMorningCallOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsComplete.GetValueOrDefault() 
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                            || t.EquipmentName.ToLower().Contains(searchAll.ToLower())
                                            || t.IssueDescription.ToLower().Contains(searchAll.ToLower())
                                            || t.ActionPlanDescription.ToLower().Contains(searchAll.ToLower())
                                            || t.InitRiskAssessment.ToLower().Contains(searchAll.ToLower())
                                            || t.CurrentUpdate.ToLower().Contains(searchAll.ToLower())
                                            || t.OffshoreComment.ToLower().Contains(searchAll.ToLower())
                                            || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList();
        }

        public List<TrackingMorningCall> GetAllInCompleteTrackingMorningCallOfProject(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IsLeaf.GetValueOrDefault() && !t.IsComplete.GetValueOrDefault() 
                                                    && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.EquipmentName.ToLower().Contains(searchAll.ToLower())
                                                    || t.IssueDescription.ToLower().Contains(searchAll.ToLower())
                                                    || t.ActionPlanDescription.ToLower().Contains(searchAll.ToLower())
                                                    || t.InitRiskAssessment.ToLower().Contains(searchAll.ToLower())
                                                    || t.CurrentUpdate.ToLower().Contains(searchAll.ToLower())
                                                    || t.OffshoreComment.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    )).ToList(); ;
        }

        public List<TrackingMorningCall> GetAll(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.EquipmentName.ToLower().Contains(searchAll.ToLower())
                                                    || t.IssueDescription.ToLower().Contains(searchAll.ToLower())
                                                    || t.ActionPlanDescription.ToLower().Contains(searchAll.ToLower())
                                                    || t.InitRiskAssessment.ToLower().Contains(searchAll.ToLower())
                                                    || t.CurrentUpdate.ToLower().Contains(searchAll.ToLower())
                                                    || t.OffshoreComment.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        public List<TrackingMorningCall> GetAllCompletedMorningCall(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && t.IsWFComplete.GetValueOrDefault()
                                                && t.IsComplete.GetValueOrDefault()
                                                && !t.IsCancel.GetValueOrDefault()

                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.EquipmentName.ToLower().Contains(searchAll.ToLower())
                                                    || t.IssueDescription.ToLower().Contains(searchAll.ToLower())
                                                    || t.ActionPlanDescription.ToLower().Contains(searchAll.ToLower())
                                                    || t.InitRiskAssessment.ToLower().Contains(searchAll.ToLower())
                                                    || t.CurrentUpdate.ToLower().Contains(searchAll.ToLower())
                                                    || t.OffshoreComment.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        public List<TrackingMorningCall> GetAllCompletedMorningCallWaitingPrint(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && t.IsWFComplete.GetValueOrDefault()
                                                && !t.IsCompleteFinal.GetValueOrDefault()
                                                && !t.IsCancel.GetValueOrDefault()

                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.EquipmentName.ToLower().Contains(searchAll.ToLower())
                                                    || t.IssueDescription.ToLower().Contains(searchAll.ToLower())
                                                    || t.ActionPlanDescription.ToLower().Contains(searchAll.ToLower())
                                                    || t.InitRiskAssessment.ToLower().Contains(searchAll.ToLower())
                                                    || t.CurrentUpdate.ToLower().Contains(searchAll.ToLower())
                                                    || t.OffshoreComment.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        public List<TrackingMorningCall> GetAllIncompleteMorningCall(int projectId, string searchAll)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsCompleteFinal.GetValueOrDefault()
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || t.Code.ToLower().Contains(searchAll.ToLower())
                                                    || t.EquipmentName.ToLower().Contains(searchAll.ToLower())
                                                    || t.IssueDescription.ToLower().Contains(searchAll.ToLower())
                                                    || t.ActionPlanDescription.ToLower().Contains(searchAll.ToLower())
                                                    || t.InitRiskAssessment.ToLower().Contains(searchAll.ToLower())
                                                    || t.CurrentUpdate.ToLower().Contains(searchAll.ToLower())
                                                    || t.OffshoreComment.ToLower().Contains(searchAll.ToLower())
                                                    || t.PICName.ToLower().Contains(searchAll.ToLower())
                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.Code).ToList();
        }

        public List<TrackingMorningCall> GetAllMCRev(Guid parentId)
        {
            return this.repo.GetAll().Where(t => (t.ID == parentId || t.ParentId == parentId)).OrderByDescending(t => t.CreatedDate).ToList();
        }

        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<TrackingMorningCall> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public TrackingMorningCall GetById(Guid id)
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
        public Guid? Insert(TrackingMorningCall bo)
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
                    ObjectName = "[WMS].[TrackingMorningCall]",
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
        public bool Update(TrackingMorningCall bo)
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
                        ObjectName = "[WMS].[TrackingMorningCall]",
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
        public bool Delete(TrackingMorningCall bo)
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
                        ObjectName = "[WMS].[TrackingMorningCall]",
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
