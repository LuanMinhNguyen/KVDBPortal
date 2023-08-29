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
    public class MaterialRequisitionService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly MaterialRequisitionDAO repo;

        private readonly WaitingSyncDataService waitingSyncDataService;

        private readonly MaterialRequisitionDetailService mrDetailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialRequisitionService"/> class.
        /// </summary>
        public MaterialRequisitionService()
        {
            this.repo = new MaterialRequisitionDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();
            this.mrDetailService = new MaterialRequisitionDetailService();

        }

        #region Get (Advances)
        public List<MaterialRequisition> GetAll(int projectId,  string searchAll)
        {
            var objIdList = this.mrDetailService.GetAll().Where(t => !string.IsNullOrEmpty(searchAll) && t.SFICode.Contains(searchAll)).Select(t => t.MRId.GetValueOrDefault());

            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || objIdList.Contains(t.ID)
                                                    || (!string.IsNullOrEmpty(t.MRNo) && t.MRNo.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.AMOSWorkOrder) && t.AMOSWorkOrder.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.DepartmentName) && t.DepartmentName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.Justification) && t.Justification.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.MRTypeName) && t.MRTypeName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.FinalAssignDeptName) && t.FinalAssignDeptName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.MRNo).ToList();
        }

        public List<MaterialRequisition> GetAllCompletedMR(int projectId, string searchAll)
        {
            var objIdList = this.mrDetailService.GetAll().Where(t => !string.IsNullOrEmpty(searchAll) && t.SFICode.Contains(searchAll)).Select(t => t.MRId.GetValueOrDefault());

            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && t.IsWFComplete.GetValueOrDefault()
                                                && t.IsCompleteFinal.GetValueOrDefault()
                                                && !t.IsCancel.GetValueOrDefault()
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || objIdList.Contains(t.ID)

                                                    || (!string.IsNullOrEmpty(t.MRNo) && t.MRNo.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.AMOSWorkOrder) && t.AMOSWorkOrder.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.DepartmentName) && t.DepartmentName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.Justification) && t.Justification.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.MRTypeName) && t.MRTypeName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.FinalAssignDeptName) && t.FinalAssignDeptName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.MRNo).ToList();
        }

        public List<MaterialRequisition> GetAllCompletedMRWaitingPrint(int projectId, string searchAll)
        {
            var objIdList = this.mrDetailService.GetAll().Where(t => !string.IsNullOrEmpty(searchAll) && t.SFICode.Contains(searchAll)).Select(t => t.MRId.GetValueOrDefault());

            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && t.IsWFComplete.GetValueOrDefault()
                                                && !t.IsCompleteFinal.GetValueOrDefault()
                                                && !t.IsCancel.GetValueOrDefault()
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || objIdList.Contains(t.ID)

                                                    || (!string.IsNullOrEmpty(t.MRNo) && t.MRNo.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.AMOSWorkOrder) && t.AMOSWorkOrder.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.DepartmentName) && t.DepartmentName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.Justification) && t.Justification.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.MRTypeName) && t.MRTypeName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.FinalAssignDeptName) && t.FinalAssignDeptName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.MRNo).ToList();
        }

        public List<MaterialRequisition> GetAllIncompleteMR(int projectId, string searchAll)
        {
            var objIdList = this.mrDetailService.GetAll().Where(t => !string.IsNullOrEmpty(searchAll) && t.SFICode.Contains(searchAll)).Select(t => t.MRId.GetValueOrDefault());

            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                                                && !t.IsCompleteFinal.GetValueOrDefault()
                                                && (string.IsNullOrEmpty(searchAll)
                                                    || objIdList.Contains(t.ID)

                                                    || (!string.IsNullOrEmpty(t.MRNo) && t.MRNo.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.AMOSWorkOrder) && t.AMOSWorkOrder.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.DepartmentName) && t.DepartmentName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.Justification) && t.Justification.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.MRTypeName) && t.MRTypeName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentAssignUserName) && t.CurrentAssignUserName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowName) && t.CurrentWorkflowName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.CurrentWorkflowStepName) && t.CurrentWorkflowStepName.Contains(searchAll))
                                                    || (!string.IsNullOrEmpty(t.FinalAssignDeptName) && t.FinalAssignDeptName.Contains(searchAll))
                                                    )).OrderByDescending(t => t.MRNo).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<MaterialRequisition> GetAll()
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
        public MaterialRequisition GetById(Guid id)
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
        public Guid? Insert(MaterialRequisition bo)
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
                    ObjectName = "[WMS].[MaterialRequisition]",
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
        public bool Update(MaterialRequisition bo)
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
                        ObjectName = "[WMS].[MaterialRequisition]",
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
        public bool Delete(MaterialRequisition bo)
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
                        ObjectName = "[WMS].[MaterialRequisition]",
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
